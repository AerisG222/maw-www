import os
import subprocess
from podman import PodmanClient

POD = 'dev-pod'
PG_CONTAINER = 'pg-dev'
DATADIR = os.path.join(os.path.dirname(os.path.dirname(__file__)), '.data')
PGDATA = f"{DATADIR}/pgdata"
PGPWD = f"{DATADIR}/pgpwd"

# make sure we start the podman service which is needed by the python api
subprocess.run(['systemctl', '--user', 'start', 'podman.socket'])

client = PodmanClient()

if not client.pods.exists(POD):
    client.pods.create(POD)

pod = client.pods.get(POD)
pod.start()

if not os.path.exists(PGDATA):
    os.makedirs(PGDATA)

if not client.containers.exists(PG_CONTAINER):
    client.containers.create(
        image = 'postgres:17',
        name = PG_CONTAINER,
        pod = POD,
        environment = {
            "POSTGRES_PASSWORD_FILE": f"/secrets/postgres.pwd"
        },
        mounts = [
            {
                'type': 'bind',
                'source': PGDATA,
                'target': '/var/lib/postgresql/data',
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
