#!/usr/bin/env python3
import os
import re
import subprocess
import sys
from pathlib import Path
from podman import PodmanClient

# overridable - the pg-upgrade playbook passes these, and they allow trying it on a copy
POD = os.environ.get('MAW_DEV_POD', 'pod-maw-www')

# the same postgres as staging and prod: pg_major / img_postgres in deploy/vars/maw-www.yml
def read_pg_image():
    text = (Path(__file__).resolve().parent.parent / 'deploy' / 'vars' / 'maw-www.yml').read_text()
    major = re.search(r'^pg_major:\s*(\d+)', text, re.MULTILINE).group(1)
    image = re.search(r'^img_postgres:\s*"?([^"\n]+)"?', text, re.MULTILINE).group(1)

    return int(major), image.replace('{{ pg_major }}', major)

PG_MAJOR, PG_IMG = read_pg_image()
PG_CONTAINER = os.environ.get('MAW_DEV_PG_CONTAINER', 'maw-www-postgres')
DATADIR = os.environ.get('MAW_DEV_DIR', '/home/mmorano/maw-www/dev')
PGDATA = f"{DATADIR}/pg-data"
PGPWD = f"{DATADIR}/pg-secrets"

# make sure we start the podman service which is needed by the python api
subprocess.run(['systemctl', '--user', 'start', 'podman.socket'])

client = PodmanClient()

if not client.pods.exists(POD):
    client.pods.create(
        POD,
        portmappings = [
            {
                'container_port': 5432,
                'host_port': 5432
            }
        ]
    )

pod = client.pods.get(POD)
pod.start()

if not os.path.exists(PGDATA):
    os.makedirs(PGDATA)

# each major keeps its data in pg-data/<major>.  creating the container for a newer major
# while only older data exists would start it on an empty database, so stop and point at
# the upgrade instead - the same guard the deploy has
versions = [int(d) for d in os.listdir(PGDATA) if d.isdigit()]
older = [v for v in versions if v < PG_MAJOR]

if PG_MAJOR not in versions and older and not client.containers.exists(PG_CONTAINER):
    sys.exit(f"pg-data holds postgres {max(older)} data but there is no postgres {PG_MAJOR} data yet - "
             f"run ./deploy.sh pg-upgrade in deploy/ and pick dev")

client.images.pull(PG_IMG)

if not client.containers.exists(PG_CONTAINER):
    client.containers.create(
        image = PG_IMG,
        name = PG_CONTAINER,
        pod = POD,
        environment = {
            "POSTGRES_PASSWORD_FILE": f"/secrets/psql-postgres"
        },
        mounts = [
            {
                'type': 'bind',
                'source': PGDATA,
                'target': '/var/lib/postgresql',
                'read_only': False,
                'relabel': 'Z'
            },
            {
                'type': 'bind',
                'source': PGPWD,
                'target': '/secrets',
                'read_only': True,
                'relabel': 'Z'
            }
        ]
    )

container = client.containers.get(PG_CONTAINER)
container.start()
