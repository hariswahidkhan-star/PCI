#!/usr/bin/env bash
# Consistent, gzipped logical backup of the PCI MySQL database.
# Uses --single-transaction (non-locking, consistent on InnoDB). Reads MYSQL_* env vars (the same
# ones the app uses) unless overridden by flags. Store the output encrypted and off-box; schedule
# daily via cron or your provider's scheduler. Prove restores periodically (a backup is only real
# once restored) — see docs/MYSQL_MIGRATION.md section 5.
set -euo pipefail

HOST="${MYSQL_HOST:-127.0.0.1}"
PORT="${MYSQL_PORT:-3306}"
DB="${MYSQL_DATABASE:-pci}"
USER="${MYSQL_USER:-pci}"
PASS="${MYSQL_PASSWORD:-}"
OUTDIR="${BACKUP_DIR:-./backups}"

mkdir -p "$OUTDIR"
STAMP="$(date -u +%Y%m%dT%H%M%SZ)"
OUT="$OUTDIR/pci-${DB}-${STAMP}.sql.gz"

echo "[backup] dumping ${DB}@${HOST}:${PORT} -> ${OUT}"
MYSQL_PWD="$PASS" mysqldump \
  --host="$HOST" --port="$PORT" --user="$USER" \
  --single-transaction --quick --routines --triggers --events \
  --default-character-set=utf8mb4 \
  "$DB" | gzip -9 > "$OUT"

echo "[backup] done: $(du -h "$OUT" | cut -f1) ${OUT}"

# Optional retention: delete gzipped dumps older than BACKUP_RETENTION_DAYS (default 14).
RETAIN="${BACKUP_RETENTION_DAYS:-14}"
find "$OUTDIR" -name "pci-${DB}-*.sql.gz" -type f -mtime +"$RETAIN" -print -delete || true
