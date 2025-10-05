#!/bin/bash
podman pod stop pod-maw-www
podman rm maw-www-postgres
sudo rm -rf ~mmorano/maw-www/dev/pg-data
