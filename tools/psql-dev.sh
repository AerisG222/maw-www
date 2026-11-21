#!/bin/bash
source "$(dirname "$0")/pg-image.sh"
PSQLUSER=$1
DB=$2

podman run -it --rm \
    --pod pod-maw-www \
    --name maw-www-query \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    "${PG_IMAGE}" \
        psql \
        -h localhost \
        -U "${PSQLUSER}" \
        -d "${DB}"
