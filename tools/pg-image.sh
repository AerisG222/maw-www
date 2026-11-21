# sourced by the dev db scripts.  sets PG_MAJOR and PG_IMAGE from deploy/vars/maw-www.yml,
# so dev runs the same postgres as staging and prod and an upgrade is one change there
PG_VARS="$(dirname "${BASH_SOURCE[0]}")/../deploy/vars/maw-www.yml"

PG_MAJOR=$(sed -nE 's/^pg_major:[[:space:]]*([0-9]+).*/\1/p' "${PG_VARS}")
PG_IMAGE=$(sed -nE 's/^img_postgres:[[:space:]]*"?([^"]*)"?.*/\1/p' "${PG_VARS}")
PG_IMAGE="${PG_IMAGE//\{\{ pg_major \}\}/${PG_MAJOR}}"

if [ -z "${PG_MAJOR}" ] || [ -z "${PG_IMAGE}" ] || [[ "${PG_IMAGE}" == *"{{"* ]]; then
    echo "could not read pg_major / img_postgres from ${PG_VARS}" >&2
    exit 1
fi
