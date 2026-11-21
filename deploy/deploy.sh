#!/bin/bash
get_value() {
    local prompt=$1
    local secure=$2
    local default=$3
    local val=

    while [ "${val}" = "" ]
    do
        if [ "${secure}" = "y" ]; then
            read -e -r -s -p "${prompt}" val
        else
            read -e -r -p "${prompt}" val
        fi

        if [ "${val}" = "" -a "${default}" != "" ]; then
            val="${default}"
        fi
    done

    echo "${val}"
}

get_file() {
    local prompt=$1
    local file=$2

    while [ "${file}" = "" -o ! -f "${file}" ]
    do
        file=$(get_value "${prompt}" 'n')
    done

    echo "${file}"
}

# the environments to offer, e.g. get_maw_env staging prod
get_maw_env() {
    local env=
    local allowed=" $* "

    while [ "${env}" = "" ]
    do
        env=$(get_value "Enter deployment environment [${*// / | }]: " 'n')

        if [[ "${allowed}" != *" ${env} "* ]]; then
            env=
        fi
    done

    echo "${env}"
}

get_action() {
    local action=

    while [ "${action}" = "" ]
    do
        action=$(get_value 'What would you like to do? [deploy | pg-upgrade] (deploy): ' 'n' 'deploy')

        if [ "${action}" != "deploy" -a "${action}" != "pg-upgrade" ]; then
            action=
        fi
    done

    echo "${action}"
}

# deploy.sh            deploy the site
# deploy.sh pg-upgrade carry the postgres data forward to the pg_major in vars/maw-www.yml,
#                      then deploy on it.  dev is offered too, for the local dev pod
# deploy.sh            with no argument, asks which
ACTION="${1:-$(get_action)}"

case "${ACTION}" in
    deploy)
        PLAYBOOK=maw-www-playbook.yml
        MAW_ENV=$(get_maw_env staging prod)
        ;;
    pg-upgrade)
        PLAYBOOK=pg-upgrade-playbook.yml
        MAW_ENV=$(get_maw_env dev staging prod)
        PG_MAJOR=$(sed -nE 's/^pg_major:[[:space:]]*([0-9]+).*/\1/p' vars/maw-www.yml)
        CONFIRM=$(get_value "Upgrade ${MAW_ENV} to postgres ${PG_MAJOR}?  The database is down for a few minutes [y/N]: " 'n' 'n')

        if [ "${CONFIRM}" != "y" ]; then
            echo 'Cancelled'
            exit
        fi
        ;;
    *)
        echo "usage: deploy.sh [deploy | pg-upgrade]"
        exit 1
        ;;
esac

if [ ! -d '.venv' ]; then
    echo 'Please run the prepare script first first!'
    exit
else
    source .venv/bin/activate
fi

# dev is the local pod tools/db-dev-start.py creates: it runs as you, with no become
# password or secrets, and its settings are in the repo (vars/dev.yml)
if [ "${MAW_ENV}" = "dev" ]; then
    exec ansible-playbook \
        --inventory inventories/dev.yml \
        --extra-vars @vars/dev.yml \
        "${PLAYBOOK}"
fi

# the next line may be needed if sshkeys are not configured on target
#--connection-password-file ~/maw-www/staging/ansible/connection-password-file \

ansible-playbook \
    --become-password-file "~/maw-www/${MAW_ENV}/ansible/become-password-file" \
    --inventory "inventories/${MAW_ENV}.yml" \
    --extra-vars "@~/maw-www/${MAW_ENV}/ansible/vars.yml" \
    --extra-vars "mawenv=${MAW_ENV}" \
    "${PLAYBOOK}"
