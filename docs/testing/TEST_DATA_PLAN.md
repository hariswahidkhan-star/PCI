# PCI Platform — Test Data Plan

_How test data is created, named, isolated and cleaned up — and the hard rule that no real personal
data is ever used. Complements `TEST_ENVIRONMENTS.md`._

## 1. Golden rule: synthetic only, no PII

No real personal data enters the tests, fixtures, logs or repository — ever. This explicitly includes
credentials, payment card data, TOTP secrets, recovery codes, government-ID images and any private
student information. Government-ID / IDV flows are exercised with tiny synthetic blobs (e.g. an 8-byte
PNG header), never a real document; the assertions check **metadata only** (reference + sha256 + size)
and confirm raw bytes are encrypted at rest and never emitted in JSON.

## 2. Naming conventions (reserved namespaces)

Fixtures use reserved prefixes so a test can never collide with, or mutate, seeded/production-shaped
rows:

| Namespace | Used by |
|---|---|
| `zephyrNN…` (e.g. `zephyr57-*`) | integration section fixtures (groups, campaigns, cohorts, sources) |
| `-NN@ex.co` (e.g. `sam-55@ex.co`) | per-section synthetic email addresses |
| `seoNN-*`, `PCI-…`, `PAY-…`, `FOUND-…` | fictitious paths / references / codes |
| test-account flag | one-click fully-unlocked accounts, excluded from reports (`is_test`) |

Discount/founding codes minted in tests use throwaway values (`CASEI25`, `SAVE25`, `Q458-XXXXXXXX`,
`GOLD2026`), never the seeded demo code `SAVE10` (which is UNIQUE-constrained in the schema).

## 3. Seeded data & the borrow-then-restore pattern

`Migrate.Run` seeds a small set of rows (pricing_rules, certification id=1, the demo discount code
`SAVE10`, some `site_settings`, comm sender profiles). Suites that must read a seeded table use a
**clear-then-insert** or **delta-based** approach so earlier sections can't skew counts, and any
**borrowed** setting (e.g. a capability ceiling, the default sender profile) is **restored** at the end
of the section. `comm_triggers` / `comm_routing_rules` / `comm_outbox` are empty in the migrated test DB
(CommsSeed runs only at app startup, not in `Migrate.Run`) — unit tests seed exactly what they assert.

## 4. Isolation

- **Per-run temp state:** each backend boot uses its own `DATABASE_FILE` and `STORAGE_ROOT` and a free
  port; MySQL runs use a dedicated database.
- **xUnit:** `TestEnv.NewMigratedDb()` gives each test a fresh migrated temp-SQLite `Db` plus a temp
  storage root; FK enforcement is ON, so a real `users` row is seeded before dependent inserts.
- **Frontend:** data/auth hooks and the fetch client are mocked at the module boundary; RTL clears the
  DOM between tests; no shared network state.
- **Rate limits:** endpoints throttled per client IP are driven with distinct `X-Forwarded-For`
  last-hops per simulated client so throttles see one clean client each.

## 5. Cleanup

Temp DB/storage/log files are created under the test dir or scratchpad and removed on the next run;
they are `.gitignore`d and never committed. The dual-provider suite is run **sequentially** (shared
storage would otherwise race). MySQL migration-integrity uses a **dedicated** database
(`pci_migint`, never the harness's `pci`).

## 6. Synthetic account catalogue (representative)

| Kind | How created | Purpose |
|---|---|---|
| Demo student (`student@pci.local`) | seed | happy-path sign-in E2E; flagged to change/deactivate before launch |
| Seeded owner admin | seed | admin console gate + forced-password-change gate |
| Viewer admin (`{overview,reports}`) | DB insert | RBAC 403 section sweep |
| Paid student (`make_paid_user`) | live purchase via signed webhook | exam/credential lifecycle |
| Partner user | DB insert | partner-portal lockout + isolation |
| Test user (fully unlocked) | admin one-click | journey viewer / report-exclusion checks |

## 7. Payment & webhook data

Stripe interactions use **test-mode / placeholder** keys; webhooks are **signed** with the test webhook
secret and shaped in-suite (Charge/Dispute/Invoice/Subscription objects). No real card data or live
Stripe objects are used. Real Stripe sandbox runs are Operator/External-pending
(`EXTERNAL_PROVIDER_TEST_PLAN.md`).
