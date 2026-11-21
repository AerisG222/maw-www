#!/bin/bash

# helper to prepare the controller node as documented in README.md
# --clear rebuilds it from scratch, so rerunning this also repairs a venv left pointing
# at a python version the os has since moved on from
python -m venv --clear .venv

source .venv/bin/activate

pip install \
    ansible \
    ansible-dev-tools \
    passlib
