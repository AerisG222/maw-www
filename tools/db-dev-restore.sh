#!/bin/bash
source "$(dirname "$0")/pg-image.sh"
PSQLUSER=postgres
DB=maw_www

podman run --rm \
    --pod pod-maw-www \
    --name dev-www-pg-load \
    --security-opt label=disable \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    --volume "$(pwd):/input" \
    "${PG_IMAGE}" \
        psql \
            -h localhost \
            -U "${PSQLUSER}" \
            -X \
            -f /input/roles.dev.dump

podman run --rm \
    --pod pod-maw-www \
    --name dev-www-pg-load \
    --security-opt label=disable \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    --volume "$(pwd):/input" \
    "${PG_IMAGE}" \
        pg_restore \
            -h localhost \
            -U "${PSQLUSER}" \
            -d postgres \
            --create \
            --clean \
            --if-exists \
            /input/maw_www.dev.dump
