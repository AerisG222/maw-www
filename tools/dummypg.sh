#!/bin/bash
podman pod create pg

podman run -d --rm \
    --pod pg \
    --name pg17 \
    --env-file ./dummy.env \
    postgres:17
