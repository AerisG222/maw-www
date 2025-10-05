#!/bin/bash
PSQLUSER=$1
DB=$2

podman run -it --rm \
    --pod dev-www-pod \
    --name dev-pg-query \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-pwd:/secrets" \
    docker.io/library/postgres:18-trixie \
        psql \
        -h localhost \
        -U "${PSQLUSER}" \
        -d "${DB}"
