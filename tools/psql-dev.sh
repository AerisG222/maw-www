#!/bin/bash
PSQLUSER=$1
DB=$2

podman run -it --rm \
    --pod pod-maw-www \
    --name maw-www-query \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    docker.io/library/postgres:18-trixie \
        psql \
        -h localhost \
        -U "${PSQLUSER}" \
        -d "${DB}"
