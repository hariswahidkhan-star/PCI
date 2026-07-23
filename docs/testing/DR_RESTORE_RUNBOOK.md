# PCI Platform — Disaster-Recovery Restore Runbook

_How to back up, restore and **prove** the PCI datastore, plus the RPO/RTO targets and the rule that a
backup is only real once restored. The DB half uses `backend/tools/mysql_backup.sh`; the file half uses
`STORAGE_ROOT`. Live prod rehearsal on Render + managed MySQL is Operator-executed (DR-3)._

## 1. What must be recoverable

A full recovery needs **both** halves, taken as close together as possible:
1. **Database** — the MySQL logical dump (all tables, routines, triggers, events).
2. **Blob storage** — the `STORAGE_ROOT` tree (certificates, IDV documents, evidence, uploads). Blobs
   are **encrypted at rest**; the encryption key (`CREDENTIAL_ENCRYPTION_KEY` / envelope keys) is an
   operator secret and must be backed up **separately and securely** — without it the restored blobs
   are unreadable. The key is never stored with the data or in the repo.

## 2. Targets

| Metric | Target | Basis |
|---|---|---|
| **RPO** (max data loss) | ≤ 24h (daily backup); ≤ 1h if hourly scheduled | backup cadence |
| **RTO** (max downtime) | ≤ 2h for a single-instance restore | dump size + boot + migration |
| Backup retention | 14 days rolling (`BACKUP_RETENTION_DAYS`) | `mysql_backup.sh` |
| Restore rehearsal | quarterly (off-box, encrypted verify) | this runbook §6 |

## 3. Backup (scheduled)

```
# DB: consistent (--single-transaction), gzipped logical dump
MYSQL_HOST=… MYSQL_USER=… MYSQL_PASSWORD=… MYSQL_DATABASE=pci \
  BACKUP_DIR=/secure/backups backend/tools/mysql_backup.sh
# → /secure/backups/pci-pci-YYYYMMDDTHHMMSSZ.sql.gz

# Files: snapshot the storage tree (same window as the DB dump)
tar -czf /secure/backups/pci-storage-$(date -u +%Y%m%dT%H%M%SZ).tgz -C "$STORAGE_ROOT" .
```

Then **encrypt** both artifacts and copy them **off-box** (different provider/region). Schedule daily
(cron / Render scheduler); back up the encryption key to a separate secret store.

## 4. Restore (into a scratch environment — never over production)

```
# 1. Create a fresh, empty scratch database
mysql -h "$H" -u "$U" -p"$P" -e "CREATE DATABASE pci_restore CHARACTER SET utf8mb4"

# 2. Restore the DB dump
gunzip -c pci-pci-YYYYMMDDTHHMMSSZ.sql.gz | mysql -h "$H" -u "$U" -p"$P" pci_restore

# 3. Restore the storage tree
mkdir -p /restore/storage && tar -xzf pci-storage-*.tgz -C /restore/storage

# 4. Boot the app against the restored set (migrations are idempotent — a re-run is a safe no-op)
DB_PROVIDER=mysql MYSQL_DATABASE=pci_restore STORAGE_ROOT=/restore/storage \
  CREDENTIAL_ENCRYPTION_KEY=… dotnet backend/bin/Release/net8.0/PCI.Backend.dll
```

## 5. Prove the restore (a backup is not real until this passes)

1. **Boots clean:** `/api/health` returns ok; migration re-run is a no-op (idempotency is asserted by
   `migration_integrity_test.py`).
2. **DB integrity:** row counts for key tables (users, payments, credentials, certifications) match the
   source-of-record; no duplicated seed rows.
3. **Refs resolve to bytes (DR-2):** pick a credential/document row and confirm its `storage_ref`
   resolves to a file under the restored `STORAGE_ROOT`, and that the file **decrypts** with the backed-
   up key to the expected sha256 (proves DB↔file↔key are a consistent set).
4. **A student can read their record:** sign in as a synthetic restored account and load `/api/me`.

## 6. Off-box encrypted verify (quarterly rehearsal)

Restore the **encrypted, off-box** copies (not the local originals) on a clean host with the key pulled
from its separate store, then run §5. Record: backup timestamp, dump size, restore wall-clock (RTO),
data-loss window (RPO), and the §5 pass/fail. File the record with the release evidence.

## 7. In-CI candidate (DR-1)

A thin CI addition can run `mysql_backup.sh` against the `backend-mysql` service, assert a non-empty
gzip is produced, restore it into a scratch DB and boot the app against it (reusing the double-boot
harness). This automates the DB half of the round-trip; the full off-box + key + storage rehearsal
remains Operator-executed (DR-3).

## 8. Failure modes & responses

| Failure | Response |
|---|---|
| Dump present but restore errors | check MySQL version parity (dump uses utf8mb4); restore into a matching-major scratch DB |
| Blobs unreadable after restore | wrong/absent encryption key — restore the key from its separate store; never store the key with the data |
| Ref with no file / file with no ref | dangling reference (handled as 404 at runtime); note in the rehearsal record and investigate the backup window skew |
| Migration errors on boot | migrations are idempotent; a real error is schema drift — compare against `schema.sql` (parity is asserted by `migration_integrity_test.py`) |
