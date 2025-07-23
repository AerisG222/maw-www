#!/bin/bash
ansible-playbook \
    --connection-password-file ~/maw/dev/www/ansible/connection-password-file \
    --become-password-file ~/maw/dev/www/ansible/become-password-file \
    --inventory inventories/dev.yml \
    --extra-vars @vars.yml \
    main.yml
