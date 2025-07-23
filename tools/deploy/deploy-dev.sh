#!/bin/bash
    #--connection-password-file ~/maw/dev/www/ansible/connection-password-file \
ansible-playbook \
    --become-password-file ~/maw/dev/www/ansible/become-password-file \
    --inventory inventories/dev.yml \
    --extra-vars @vars.yml \
    --extra-vars @~/maw/dev/www/ansible/secrets.yml \
    main.yml
