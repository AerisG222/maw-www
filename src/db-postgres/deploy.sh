#!/bin/bash
DBNAME="maw_www"
IMAGE="docker.io/library/postgres:18-trixie"
PODNAME="pod-maw-www"
PWDFILEDIR=$1

function header() {
    echo "** ${1} **"
}

function run_psql_script() {
    local script=$1
    local db=$2

    if [ "${db}" == "" ]
    then
        db="${DBNAME}"
    fi

    if [ "${PODNAME}" == "" ]
    then
        psql -d "${db}" -q -f "${script}";
    else
        podman run -it --rm \
            --pod "${PODNAME}" \
            --env "POSTGRES_PASSWORD_FILE=/secrets/psql-postgres" \
            --volume "${PWDFILEDIR}":/secrets:ro \
            --volume "$(pwd)":/tmp/context:ro \
            --security-opt label=disable \
            "${IMAGE}" \
                psql \
                    -h 127.0.0.1 \
                    -U postgres \
                    -d "${db}" \
                    -q \
                    -f "/tmp/context/${script}"
    fi
}

header "pull latest postgres image"
podman pull "${IMAGE}"

header "database ${DBNAME}"
run_psql_script "database/maw_www.sql" "postgres" &> /dev/null

header "roles"
run_psql_script "roles/website.sql"

header "users"
run_psql_script "users/svc_maw_www.sql"

header "schemas"
run_psql_script "schemas/blog.sql"

header "tables"
run_psql_script "tables/blog.blog.sql"
run_psql_script "tables/blog.post.sql"

header "seed"
run_psql_script "seed/blog.blog.sql"

header "functions"
run_psql_script "functions/blog.add_post.sql"
run_psql_script "functions/blog.get_blogs.sql"
run_psql_script "functions/blog.get_posts.sql"

header "completed ${DBNAME}"
