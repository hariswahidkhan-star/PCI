# Running the platform on MySQL 8 / MariaDB

The application is written once in the SQLite dialect and translated at runtime (`Data/Db.cs`).
That keeps ~430 call sites provider-agnostic, and it means a statement valid on one engine and not
another is **invisible until a production boot**. Schema installation logs errors and continues, so
one rejection silently abandons every statement after it — the failure mode is not an error page,
it is tables that quietly do not exist.

This document is how to prove parity rather than assume it.

## Verifying against a real server

```bash
# 1. a server
sudo apt-get install -y mariadb-server
sudo mkdir -p /run/mysqld && sudo chown mysql:mysql /run/mysqld
sudo mariadbd --user=mysql --bind-address=127.0.0.1 &

# 2. a database
sudo mariadb -e "CREATE DATABASE pcitest CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
                 CREATE USER IF NOT EXISTS 'pci'@'%' IDENTIFIED BY 'pcitest';
                 GRANT ALL ON pcitest.* TO 'pci'@'%'; FLUSH PRIVILEGES;"

# 3. the WHOLE integration suite against it (the harness supports this directly)
pip install pymysql
TEST_DB_PROVIDER=mysql MYSQL_HOST=127.0.0.1 MYSQL_USER=pci MYSQL_PASSWORD=pcitest \
  MYSQL_DATABASE=pcitest MYSQL_SSL=false python3 backend/tests/integration_test.py
```

Compare the result with the same suite on SQLite (`python3 backend/tests/integration_test.py`).
Any difference is a parity defect.

## Where the engines actually differ

| | SQLite | MariaDB | MySQL 8 |
|---|---|---|---|
| `CREATE INDEX IF NOT EXISTS` | yes | yes | **syntax error** → stripped for MySQL, duplicate-index error absorbed |
| index on a lone `TEXT` column | yes | silently prefixed | **rejected** (1170) → retried with an explicit prefix |
| `TEXT` inside a composite key | yes | rejected (1071) | rejected (1071) → those columns are bounded `VARCHAR` |
| reserved words (`current_role`, `rank`, `groups`, …) | none | reserved | reserved → **back-quote every dynamically-assembled identifier** |
| `date('now','-1 day')` | yes | needs rewriting | needs rewriting |

`Db.IsMariaDb` distinguishes the two engines from the server's version banner, because
`DB_PROVIDER=mysql` covers both and they do not agree.

## Rules for new code

1. **Never interpolate a bare column name.** Back-quote it: `` $"`{col}`=?" ``. SQLite, MySQL and
   MariaDB all accept back-quotes, so one form works everywhere. A column called `current_role`
   broke every profile save on both MySQL engines and was invisible on SQLite.
2. **Never index a `TEXT` column.** Declare a bounded `VARCHAR`; a datetime stamp is 19 characters.
   `WorldMySqlParityTests` walks the installed schema and fails the build if this is violated — and
   it runs on SQLite, so ordinary CI catches it with no server.
3. **Only use relative-time forms the translator knows** (`datetime('now','±N unit')`,
   `date('now','±N unit')`). An unrecognised form is not an error; it is valid SQL that compares
   against a literal string and silently matches nothing.

## Current parity status (verified on MariaDB 10.11)

- **PCI World: full parity.** Clean install creates all 25 tables with zero indexed `TEXT` columns;
  50 challenges, 10 articles, the rotation ledger; anonymous session → attempt → grading works; all
  public pages, the sitemap and robots serve; every admin endpoint answers 200.
- **Platform: 1141 / 1158 integration assertions pass** (SQLite: 1158 / 1158).
- **Platform: 1158 / 1158 on BOTH providers**, including against a storage directory left over from
  a run under the *other* provider — the condition that used to fail (see below).

### The 17 download failures were not a MySQL bug

They looked like one, and an earlier revision of this document said so. They were not.

Running the suite on **SQLite** against a storage directory left behind by a **MySQL** run
reproduced the same 17 failures exactly. The provider was a red herring; what mattered was that
artefacts already existed on disk from a run with a different encryption key.

The real defect was in `Storage.Put`. Artefacts are content-addressed on the plaintext hash and
persisted as ciphertext, and the write was skipped whenever a file already sat at the target path:

```csharp
if (!File.Exists(full)) File.WriteAllBytes(full, enc);   // "content-addressed → dedupe for free"
```

That is only sound while the encryption key never changes. The derived fallback key mixes in
`DATABASE_FILE` and `MYSQL_DATABASE`, so **moving from SQLite to MySQL changes it** — as does
setting `CREDENTIAL_ENCRYPTION_KEY` for the first time, or rotating it. After any of those, every
stored file is undecryptable, and the dedupe skip made that permanent: re-uploading byte-identical
content took the same path, skipped the write again, and the store served `file_missing` for ever
with no signal and no repair route short of deleting files by hand.

`Storage.Put` now verifies rather than assumes — it dedupes only against a file it can actually
read back, and rewrites one it cannot. The store is self-healing across a key change.

**This mattered directly for the migration this document describes.** Switching the live deployment
from SQLite to MySQL would have silently made every existing document, Body-of-Knowledge PDF and
evidence file undownloadable.

### Previously recorded here and now resolved

- ~~**Known gap — 17 assertions, document/BoK downloads only.**~~ The student, partner and watermarked
  download paths return `file_missing` on MySQL while the admin download of the same document
  succeeds. Investigation so far: the `documents` rows and their `storage_ref` values are intact and
  correctly formed, the upload writes the file, and the admin path serves it — but the artefact is
  absent from disk by the time a student download runs. Storage is content-addressed with
  write-dedup and the suite exercises an age-0 retention purge, which is the most likely interaction.
  This is a pre-existing platform defect, unrelated to PCI World, and it is **not fixed**. It should
  be resolved before the Institute platform itself is migrated to MySQL; PCI World is unaffected.
