#!/bin/bash
podman pod stop dev-www-pod
podman rm dev-www-pg
sudo rm -rf ~mmorano/maw-www/dev/pg-data
