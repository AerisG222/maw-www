#!/bin/bash
podman run -it --rm \
    --pod dev-pod \
    --name dev-pg-query \
    --env "POSTGRES_PASSWORD_FILE=/secrets/postgres.pwd" \
    --volume "../.data/pgpwd:/secrets" \
    postgres:17 \
        psql \
        -h localhost \
        -U postgres \
        -d postgres
