#!/bin/bash
PSQLUSER=$1
DB=$2

podman run -it --rm \
    --pod dev-pod \
    --name dev-pg-query \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-dev/data/pgpwd:/secrets" \
    docker.io/library/postgres:17 \
        psql \
        -h localhost \
        -U "${PSQLUSER}" \
        -d "${DB}"
