#!/bin/sh
set -e

PG_HOST="${PG_HOST:-postgres}"
PSQL="psql -v ON_ERROR_STOP=1 -h $PG_HOST -U $PG_USER -d $PG_DB"

echo "Ensuring _migrations tracking table..."
$PSQL -c "CREATE TABLE IF NOT EXISTS _migrations (
  filename   TEXT PRIMARY KEY,
  applied_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);"

for f in /migrations/*.sql; do
  bn=$(basename "$f")
  applied=$($PSQL -tAc "SELECT 1 FROM _migrations WHERE filename = '$bn'")
  if [ "$applied" = "1" ]; then
    echo "  skip:  $bn"
    continue
  fi
  echo "  apply: $bn"
  $PSQL -f "$f"
  $PSQL -c "INSERT INTO _migrations (filename) VALUES ('$bn');"
done

echo "All migrations applied"
