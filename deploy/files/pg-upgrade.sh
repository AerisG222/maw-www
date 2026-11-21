#!/bin/bash
# carries the postgres data from one major version to the next by dump and restore.
# pg-upgrade-playbook.yml (`./deploy.sh pg-upgrade` on staging and prod) and
# tools/db-dev-upgrade.sh (dev) run it, and stop and start postgres around the two steps:
#
#   pg-upgrade.sh dump <old> <new> <run>      the old server running, the site stopped
#   pg-upgrade.sh restore <old> <new> <run>   the pod stopped
#
# dump reads the old server with the new version's client tools, as postgres recommends,
# and records a fingerprint of what it read.  restore builds the new cluster in
# pg-data/<new>-upgrading with a temporary container, loads the dump, checks the result
# against that fingerprint, and only then renames it to pg-data/<new> - so pg-data/<new>
# only ever exists complete.  nothing writes to pg-data/<old>, which stays as the way back.
set -euo pipefail

# configured through the environment, so the one script serves every setup:
#
#   PGU_POD        the pod in which 127.0.0.1 is the running old server
#   PGU_IMAGE      the new postgres image
#   PGU_PGDATA     the host directory postgres has mounted at /var/lib/postgresql
#   PGU_PGSECRETS  the host directory holding psql-postgres, mounted at /secrets
#   PGU_WORKROOT   where each run keeps its dump and fingerprints
#   PGU_USERNS     optional --userns for the new cluster's container
#   PGU_USER       optional --user for the containers
#
# the last two have to match how postgres runs, or the new cluster would not belong to
# the user that later runs it.  the deployed pod runs postgres as uid 1654 in a keep-id
# user namespace (kube.yml.j2); the dev pod uses neither, leaving the image to run as its
# own postgres user
POD="${PGU_POD:?}"
IMAGE="${PGU_IMAGE:?}"
DIR_PGDATA="${PGU_PGDATA:?}"
DIR_PGSECRETS="${PGU_PGSECRETS:?}"
DIR_PGUPGRADE="${PGU_WORKROOT:?}"

USER_ARGS=()
USERNS_ARGS=()

if [ -n "${PGU_USER:-}" ]; then
    USER_ARGS=(--user "${PGU_USER}")
fi

if [ -n "${PGU_USERNS:-}" ]; then
    USERNS_ARGS=(--userns "${PGU_USERNS}")
fi

# ---------------------------------------------------------------------------------------
# run inside the containers, connected to the server over 127.0.0.1 (trusted by the
# image's pg_hba.conf).  passed in with declare -f
# ---------------------------------------------------------------------------------------

# what has to come through unchanged: every role with its attributes and password (as a
# checksum - the hashes themselves stay out of the file), and per database the exact row
# count of every table, every sequence position and the number of functions per schema
fingerprint() {
    local workdir=$1
    local dbs db

    mapfile -t dbs < "${workdir}/databases.txt"

    psql -X -At -d postgres -c "
        select 'role ' || rolname || ' '
               || concat_ws(',', rolsuper, rolinherit, rolcreaterole, rolcreatedb, rolcanlogin,
                            rolreplication, rolbypassrls, rolconnlimit, rolvaliduntil)
               || ' ' || coalesce(md5(rolpassword), 'no-password')
          from pg_authid
         where rolname !~ '^pg_'
         order by rolname"

    for db in "${dbs[@]}"; do
        psql -X -At -v ON_ERROR_STOP=1 -v db="${db}" -d "${db}" <<'SQL'
select format('select %L || '' '' || count(*) from %I.%I',
              'rows ' || :'db' || ' ' || table_schema || '.' || table_name,
              table_schema, table_name)
  from information_schema.tables
 where table_type = 'BASE TABLE'
   and table_schema not in ('pg_catalog', 'information_schema')
 order by table_schema, table_name
\gexec

select 'sequence ' || :'db' || ' ' || schemaname || '.' || sequencename || ' '
       || coalesce(last_value::text, 'unused')
  from pg_sequences
 order by schemaname, sequencename;

select 'functions ' || :'db' || ' ' || n.nspname || ' ' || count(*)
  from pg_proc p
  join pg_namespace n on n.oid = p.pronamespace
 where n.nspname not in ('pg_catalog', 'information_schema')
   and not exists (select 1 from pg_depend d where d.objid = p.oid and d.deptype = 'e')
 group by n.nspname
 order by n.nspname;
SQL
    done
}

inside_dump() {
    local old=$1
    local new=$2
    local workdir=$3
    local version dbs db ext

    if [ "${PG_MAJOR}" != "${new}" ]; then
        echo "** the image is postgres ${PG_MAJOR}, expected ${new} **" >&2
        return 1
    fi

    version=$(psql -X -At -d postgres -c 'show server_version_num')

    if [ "$((version / 10000))" != "${old}" ]; then
        echo "** expected the running server to be postgres ${old}, it is ${version} **" >&2
        return 1
    fi

    psql -X -At -d postgres -c "
        select datname
          from pg_database
         where datallowconn
           and not datistemplate
         order by datname" > "${workdir}/databases.txt"

    mapfile -t dbs < "${workdir}/databases.txt"

    # an extension the new image does not ship would fail the restore, so find out now
    for db in "${dbs[@]}"; do
        for ext in $(psql -X -At -d "${db}" -c "select extname from pg_extension where extname <> 'plpgsql'"); do
            if [ ! -f "/usr/share/postgresql/${new}/extension/${ext}.control" ]; then
                echo "** database ${db} uses extension ${ext}, which the postgres ${new} image does not have **" >&2
                return 1
            fi
        done
    done

    # ALTER SYSTEM settings live in the old cluster's postgresql.auto.conf, which no dump
    # includes.  recorded so the playbook can point them out
    psql -X -At -d postgres -c "
        select name || ' = ' || setting
          from pg_file_settings
         where sourcefile like '%postgresql.auto.conf'
         order by seqno" > "${workdir}/alter-system.txt"

    # roles keep their passwords here, unlike the nightly backups.  the restore has no
    # other way to give svc_maw_www its password back
    pg_dumpall --globals-only --file "${workdir}/globals.sql"

    for db in "${dbs[@]}"; do
        pg_dump --format custom --file "${workdir}/${db}.dump" "${db}"
    done

    fingerprint "${workdir}" > "${workdir}/before.txt"
}

inside_restore() {
    local workdir=$1
    local dbs db

    mapfile -t dbs < "${workdir}/databases.txt"

    # roles first, everything after refers to them.  initdb has already made postgres, so
    # its CREATE ROLE would fail - drop just that line.  the ALTER ROLE after it still sets
    # postgres's attributes and password
    sed '/^CREATE ROLE postgres;$/d' "${workdir}/globals.sql" \
        | psql -X -q -v ON_ERROR_STOP=1 -d postgres -f -

    for db in "${dbs[@]}"; do
        if [ "${db}" = "postgres" ]; then
            pg_restore --exit-on-error --dbname postgres "${workdir}/${db}.dump"
        else
            pg_restore --exit-on-error --create --dbname postgres "${workdir}/${db}.dump"
        fi
    done

    # a restore brings the data but not the planner statistics
    vacuumdb --all --analyze-in-stages --quiet

    fingerprint "${workdir}" > "${workdir}/after.txt"
}

# ---------------------------------------------------------------------------------------
# run on the host
# ---------------------------------------------------------------------------------------

fail() {
    echo "** pg-upgrade: $* **" >&2
    exit 1
}

usage() {
    echo "usage: pg-upgrade.sh dump|restore <old major> <new major> <run name>" >&2
    exit 1
}

dump() {
    mkdir -p "${WORKDIR}"
    chmod 700 "${WORKDIR}"

    echo "dumping postgres ${OLD} with the postgres ${NEW} client into ${WORKDIR}"

    # attached to the pod, where 127.0.0.1 is the running server.  --restart no is load
    # bearing: a container joining a kube play pod inherits the pod's restart policy,
    # always, even with --rm - without it the dump runs again every time it exits, and
    # rewrites these files under the restore that is reading them
    podman run --rm \
        --restart no \
        --pod "${POD}" \
        "${USER_ARGS[@]}" \
        --read-only \
        --env PGHOST=127.0.0.1 \
        --env PGUSER=postgres \
        --volume "${WORKDIR}:/upgrade:Z" \
        "${IMAGE}" \
            bash -c "set -euo pipefail; $(declare -f fingerprint inside_dump); inside_dump \"\$@\"" \
                _ "${OLD}" "${NEW}" /upgrade

    if [ -s "${WORKDIR}/alter-system.txt" ]; then
        echo "WARNING: these ALTER SYSTEM settings are not carried over - reapply any still wanted:"
        sed 's/^/    /' "${WORKDIR}/alter-system.txt"
    fi

    echo "dumped: $(tr '\n' ' ' < "${WORKDIR}/databases.txt")"
}

restore() {
    local staging="${DIR_PGDATA}/${NEW}-upgrading"
    local target="${DIR_PGDATA}/${NEW}"
    local i

    # global rather than local: the EXIT trap below runs after restore has returned
    CONTAINER="pg-upgrade-${NEW}"

    [ ! -e "${target}" ] || fail "${target} already exists"
    [ -s "${WORKDIR}/before.txt" ] || fail "no dump in ${WORKDIR} - run dump first"

    # only ever left behind by an earlier run that failed - it is never live
    podman unshare rm -rf "${staging}"

    trap 'podman rm --force --time 0 "${CONTAINER}" > /dev/null 2>&1 || true' EXIT

    echo "creating postgres ${NEW} in ${staging}"

    podman run --detach --replace \
        --name "${CONTAINER}" \
        "${USERNS_ARGS[@]}" \
        "${USER_ARGS[@]}" \
        --read-only \
        --env POSTGRES_PASSWORD_FILE=/secrets/psql-postgres \
        --env PGDATA="/var/lib/postgresql/${NEW}-upgrading/docker" \
        --env PGHOST=127.0.0.1 \
        --env PGUSER=postgres \
        --volume "${DIR_PGDATA}:/var/lib/postgresql:Z" \
        --volume "${DIR_PGSECRETS}:/secrets:ro,Z" \
        --volume "${WORKDIR}:/upgrade:Z" \
        "${IMAGE}" \
            postgres > /dev/null

    # over tcp on purpose: on first start the image runs initdb under a temporary server
    # that only listens on the unix socket, then restarts it.  the socket answering would
    # only mean that temporary server is up
    for i in $(seq 1 60); do
        if podman exec "${CONTAINER}" pg_isready -q -h 127.0.0.1 2> /dev/null; then
            break
        fi

        if [ "$(podman container inspect --format '{{.State.Running}}' "${CONTAINER}")" != "true" ]; then
            podman logs "${CONTAINER}" >&2
            fail "postgres ${NEW} exited during startup"
        fi

        [ "${i}" -lt 60 ] || fail "postgres ${NEW} did not start"
        sleep 2
    done

    echo "restoring"

    podman exec "${CONTAINER}" \
        bash -c "set -euo pipefail; $(declare -f fingerprint inside_restore); inside_restore \"\$@\"" \
            _ /upgrade

    if ! diff -u "${WORKDIR}/before.txt" "${WORKDIR}/after.txt"; then
        fail "the restored data does not match what was dumped (diff above)"
    fi

    echo "verified: $(grep -c '^role ' "${WORKDIR}/after.txt") roles, $(grep -c '^rows ' "${WORKDIR}/after.txt") tables, row counts and sequences match"

    # a clean shutdown before the directory is moved under it
    podman stop --time 60 "${CONTAINER}" > /dev/null
    podman rm "${CONTAINER}" > /dev/null

    podman unshare mv "${staging}" "${target}"

    echo "postgres ${NEW} data is ready in ${target}"
}

[ $# -eq 4 ] || usage

ACTION=$1
OLD=$2
NEW=$3
RUN=$4
WORKDIR="${DIR_PGUPGRADE}/${RUN}"

case "${ACTION}" in
    dump)    dump ;;
    restore) restore ;;
    *)       usage ;;
esac
