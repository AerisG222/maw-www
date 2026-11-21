# Notes for Deployment

To deploy the application, an ansible playbook is provided to easily deploy this site to
one or more nodes. It makes a couple of key assumptions:

1. Remote OS is Linux and has SSH running that can be accessed from the machine where the scripts run.
2. This playbook only focuses on getting this application running in a podman pod, and is expected that a different deployment will be made to serve as the reverse proxy (i.e. maw-gateway).
3. Trying to keep this simple for now - I went crazy building out an advanced playbook last time, but there are so many steps and I don't need to run often, so I rather not run it and don't want to revamp it for the new solution.
4. Once deployed, you currently will still need to run the following steps manually:
    1. run the db deploy steps (src/db-postgres/deploy.sh) to create the db
    2. run the apply-migration step to load data from legacy database (/tools/legacy_db_migration)
    3. set the password for the svc_maw_www db role

## Control Node Setup

1. Make sure python/pip are installed
2. `cd deploy/` within project
3. Setup a python environment for ansible in the deploy dir: `python -m venv .venv`
4. Enter the environment: `source .venv/bin/activate`
5. If you want to exit that environment, just run `deactivate`
6. Install Ansible
    1. `pip install ansible`
    2. `pip install passlib`
    3. to upgrade: `pip install --upgrade ansible`

## Managed Node Setup

1. Python needs to be installed (note: ansible is agentless, so that does not need to be installed on managed nodes)

If the venv stops working (e.g. `No module named 'ansible'` after the OS moves to a newer python), rerun
`./prepare-control-env.sh` - it rebuilds the venv from scratch.

## Upgrading Postgres

Each postgres major keeps its data in its own directory (`pg-data/<major>`), and the upgrade carries the data
across with a dump and restore:

1. Set `pg_major` in `vars/maw-www.yml` to the new version (check the release notes' incompatibilities first)
2. `./deploy.sh pg-upgrade` and pick the environment - dev first, then staging, then prod

`dev` upgrades the local dev pod (the one `tools/db-dev-start.py` creates) with the same playbook, through
`inventories/dev.yml` and `vars/dev.yml` - it runs as you, needs no become password, and recreates the dev container on
the new image instead of deploying.  The dev tools read `pg_major` from the same vars file, so dev, staging and prod
always agree on the version, and `db-dev-start.py` has the same guard as the deploy below.

The site is down for a few minutes while it:

1. stops the website and dumps the old database with the new version's tools, into `pg-upgrade/<old>-to-<new>-<time>`
2. stops postgres and restores into a new cluster, then checks every role, table row count and sequence against the dump
3. moves the new cluster into place as `pg-data/<new>` and starts the new image on it - a normal deploy for staging
   and prod, a new container for dev

If anything fails before that, postgres is restarted on the old version with its data untouched.  A plain
`./deploy.sh` refuses to start a new `pg_major` while only older data exists, so the upgrade can't be skipped
by accident.

Settings made with `ALTER SYSTEM` are not carried over - the upgrade lists any it finds so they can be reapplied.

Afterwards, the old data stays in `pg-data/<old>`:

- to roll back: set `pg_major` back and `./deploy.sh` - for dev, `podman rm -f maw-www-postgres && tools/db-dev-start.py`
  (changes made since the upgrade are lost).  remove `pg-data/<new>` before upgrading again
- once happy: as the service account, `podman unshare rm -rf ~/maw-www/pg-data/<old> ~/maw-www/pg-upgrade/<run>`
