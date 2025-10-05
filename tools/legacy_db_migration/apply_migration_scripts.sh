#!/bin/bash
PODNAME="pod-maw-www"
PWDFILEDIR="/home/mmorano/maw-www/dev/pg-secrets"
DBNAME="maw_www"
IMAGE="docker.io/library/postgres:18-trixie"

function run_psql_script() {
    local script=$1

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
                -d "${DBNAME}" \
                -q \
                -f "${script}"
}

run_psql_script "/tmp/context/blog.post.sql"
