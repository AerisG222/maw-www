#!/bin/bash
DBNAME="maw_www"
PODNAME=$1
ENVFILE=$2

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
            --env-file "${ENVFILE}" \
            --volume "$(pwd)":/tmp/context:ro \
            --security-opt label=disable \
            postgres:17 \
                psql \
                    -h 127.0.0.1 \
                    -U postgres \
                    -d "${db}" \
                    -q \
                    -f "/tmp/context/${script}"
    fi
}

header "database ${DBNAME}"
run_psql_script "database/maw_www.sql" "postgres" &> /dev/null

header "roles"
run_psql_script "roles/website.sql"

header "users"
run_psql_script "users/svc_maw_www.sql"

header "schemas"
run_psql_script "schemas/maw.sql"
run_psql_script "schemas/blog.sql"

header "tables"
run_psql_script "tables/maw.role.sql"
run_psql_script "tables/maw.user.sql"
run_psql_script "tables/maw.user_role.sql"
run_psql_script "tables/blog.blog.sql"
run_psql_script "tables/blog.post.sql"

header "seed"
run_psql_script "seed/blog.blog.sql"

header "functions"
run_psql_script "functions/blog.add_post.sql"
run_psql_script "functions/blog.get_blogs.sql"
run_psql_script "functions/blog.get_posts.sql"

header "completed ${DBNAME}"
