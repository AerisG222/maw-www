#!/bin/bash
PSQLUSER=postgres
DB=maw_www

podman run --rm \
    --pod pod-maw-www \
    --name dev-www-pg-dumpall \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    docker.io/library/postgres:18-trixie \
        pg_dumpall \
            -h localhost \
            -U "${PSQLUSER}" \
            --roles-only \
    > roles.dev.dump

podman run --rm \
    --pod pod-maw-www \
    --name dev-www-pg-dump \
    --env "POSTGRES_PASSWORD_FILE=/secrets/psql-${PSQLUSER}" \
    --volume "/home/mmorano/maw-www/dev/pg-secrets:/secrets" \
    docker.io/library/postgres:18-trixie \
        pg_dump \
            -Fc \
            -h localhost \
            -U "${PSQLUSER}" \
            -d "${DB}" \
    > maw_www.dev.dump
