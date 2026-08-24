#!/bin/bash
DBNAME="maw_www"
IMAGE="docker.io/library/postgres:18-trixie"
PODNAME="pod-maw-www"
PWDFILEDIR=$1

# the scripts are all written to be re-runnable (CREATE ... IF NOT EXISTS), so a
# repeat deploy emits a NOTICE for every object that is already there.  raising
# the client threshold to warning drops that noise and leaves warnings and
# errors standing out.
PSQL_OPTIONS="-c client_min_messages=warning"

# where the sql lives from psql's point of view.  empty when psql runs on this
# host, since the scripts are then addressed relative to the working directory.
CONTEXT_PREFIX=""

# the deploy is assembled as a single psql script of \i includes and executed in
# one session, rather than one psql invocation per file.
#
# the reason is container churn.  a container per script meant a create/teardown
# cycle per script - each mounting the postgres image, each writing to podman's
# database.  podman removes a --rm container in a separate step after it exits,
# so anything that interrupts a run (a failed script, ctrl-c, an OOM) strands
# containers permanently, and `podman ps -a` slows down in proportion to how many
# have piled up.  two containers per deploy cannot meaningfully accumulate.
#
# semantics are unchanged: ON_ERROR_STOP still aborts on the first error, and
# psql reports the *included* file and line, so a failure still names the script
# that caused it.  no script sets session state - there is no top level BEGIN,
# no session level SET and no \c anywhere in the tree - so sharing one session
# between them changes nothing.
DRIVER=""

function showUsage() {
    echo "deploy.sh <pwddir>"
}

function header() {
    DRIVER+="\\echo ''"$'\n'
    DRIVER+="\\echo '** ${1} **'"$'\n'
}

function queue() {
    DRIVER+="\\echo '    - ${1}'"$'\n'
    DRIVER+="\\i ${CONTEXT_PREFIX}${1}"$'\n'
}

# runs whatever has been queued so far, in one psql session, and aborts the
# deploy if any of it fails.  the scripts are ordered by dependency, so
# continuing past a failure only produces more failures against a schema that is
# already wrong.
function run_driver() {
    local status=0

    printf '%s' "${DRIVER}" | psql_run "${DBNAME}"
    status=$?

    DRIVER=""

    if [ ${status} -ne 0 ]
    then
        echo "" >&2
        echo "** DEPLOY FAILED (exit ${status}) - see the psql error above for the script and line **" >&2
        exit 1
    fi
}

# one script, on its own, for the case where a non-zero exit is expected.
# CREATE DATABASE has no IF NOT EXISTS, so it fails on every run after the first.
function run_single_script() {
    local script=$1
    local db=$2

    echo "    - ${script}"

    printf '%s' "\\i ${CONTEXT_PREFIX}${script}"$'\n' | psql_run "${db}"

    return 0
}

# reads the script to run from stdin.  -i on podman run is load bearing: without
# it the container gets no stdin and psql silently does nothing.
function psql_run() {
    local db=$1

    if [ "${PODNAME}" == "" ]
    then
        PGOPTIONS="${PSQL_OPTIONS}" psql \
            -d "${db}" \
            -q \
            -v ON_ERROR_STOP=1
    else
        podman run --rm -i \
            --pod "${PODNAME}" \
            --env "POSTGRES_PASSWORD_FILE=/secrets/psql-postgres" \
            --env "PGOPTIONS=${PSQL_OPTIONS}" \
            --volume "${PWDFILEDIR}":/secrets:ro \
            --volume "$(pwd)":/tmp/context:ro \
            --security-opt label=disable \
            "${IMAGE}" \
                psql \
                    -h 127.0.0.1 \
                    -U postgres \
                    -d "${db}" \
                    -q \
                    -v ON_ERROR_STOP=1
    fi
}

# postgres may still be starting when a deploy begins.  the very first script
# would then fail to connect, and since any failure now aborts the run, a startup
# race would read as a broken deploy.
function wait_for_postgres() {
    local attempts=60
    local i=1

    while [ ${i} -le ${attempts} ]
    do
        if postgres_is_ready
        then
            return 0
        fi

        sleep 1
        i=$((i + 1))
    done

    echo "" >&2
    echo "** DEPLOY FAILED: postgres was not reachable after ${attempts} attempts **" >&2
    exit 1
}

function postgres_is_ready() {
    if [ "${PODNAME}" == "" ]
    then
        psql -d postgres -q -c "SELECT 1" > /dev/null 2>&1
    else
        podman run --rm \
            --pod "${PODNAME}" \
            --env "POSTGRES_PASSWORD_FILE=/secrets/psql-postgres" \
            --volume "${PWDFILEDIR}":/secrets:ro \
            --security-opt label=disable \
            "${IMAGE}" \
                psql \
                    -h 127.0.0.1 \
                    -U postgres \
                    -d postgres \
                    -q \
                    -c "SELECT 1" > /dev/null 2>&1
    fi
}

function main() {
    if [ "${PODNAME}" != "" ]
    then
        CONTEXT_PREFIX="/tmp/context/"

        echo ""
        echo "** pull latest postgres image **"
        podman pull "${IMAGE}"
    fi

    wait_for_postgres

    echo ""
    echo "** database ${DBNAME} **"
    run_single_script "database/maw_www.sql" "postgres" &> /dev/null

    header "roles"
    queue "roles/website.sql"

    header "users"
    queue "users/svc_maw_www.sql"

    header "schemas"
    queue "schemas/blog.sql"

    header "tables"
    queue "tables/blog.blog.sql"
    queue "tables/blog.post.sql"

    header "seed"
    queue "seed/blog.blog.sql"

    header "functions"
    queue "functions/blog.add_post.sql"
    queue "functions/blog.get_blogs.sql"
    queue "functions/blog.get_posts.sql"

    run_driver

    echo ""
    echo "** completed ${DBNAME} **"
}

if [ "${PWDFILEDIR}" = "" ]; then
    showUsage
    exit
fi

main
