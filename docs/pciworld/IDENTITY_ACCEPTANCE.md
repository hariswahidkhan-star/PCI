# PCI World Shared Identity — §20 acceptance sweep

The master prompt's final acceptance criteria, answered honestly in three columns: **met with
evidence**, **partial / deliberately scoped out**, and **operator action required**. Evidence cites
the CI-gated suites (17 Python logic suites + xUnit on SQLite and MariaDB) and the merged PRs
(#186 → `caa15b3`, #188 → `57fadf1`, both `main`-CI green; wave 3+4 on PR #189).
Companion detail: `IDENTITY_PHASE0_BASELINE.md` §4a–4f.

## Met, with evidence

| Criterion (§20) | Evidence |
|---|---|
| One canonical identity/profile/password authority | `WorldIdentity` single-source preserved; split-identity registration closed (`WorldIdentityLinkTests`, Phase 1b) |
| All account-creation paths issue one Student Number transactionally | Six paths through `StudentNumbers.GetOrIssue`; `student_number_test.py` 16/16; xUnit both engines |
| Historic numbers preserved; uniqueness/non-reuse; registry reconciliation | Registry + guarded index; backfill/quarantine/reconcile/retire — `student_number_backfill_test.py` 24/24 |
| No read endpoint creates a number (cutover) | `identity_lazy_backstop` flag, default ON; pure read once Health shows zero missing — gate is the Health report, not a date |
| No public screen mislabels `users.id` | Books watermark fixed; integration 18m inverted to *forbid* the old label |
| Concurrent signup → one identity | Atomic reservation as the claim; collision leaves the loser numberless, never wrong |
| Maker-checker merges | `identity_merge_test.py` 47/47 — survivor keeps number, loser's resolves privately, sessions revoked |
| Both handoff directions, replay/leak-safe | `portal_handoff_test.py` 33/33 — hashed one-use 90 s codes, fragment-only, purpose isolation, generic failures |
| Same authoritative Passport in both portals, no iframe/copy | `passport_summary_test.py` 33/33 value-identity with the World side; shared component family |
| Same Student Number across portals/Passport/PDF/verification | Summary DTO, documents, verification all read the one projection |
| Number verification: POST-only, neutral, throttled, noindex, no-store | `world_passport_verify_test.py` 40/40 — byte-identical neutral across 15 states |
| Event capacity/windows/cancellation backend-enforced, no final-seat race | `events_defects_test.py` 37/37 (ID-10/11/12) |
| Attendance and CPD separated, CPD exactly-once | `source_event_id` unique constraint + one transaction |
| Passport disclosure protects API/HTML/metadata/PDF | OG derivation from public state only, XSS-proof (`world_og_metadata_test.py` 75/75); per-artifact canonical (ID-09) |
| Release-1 share actions use supported official mechanisms | URL share/intents only; capability-honesty footnote; no fake Instagram posting; caption from public fields (`world_share_caption_test.py` 34/34) |
| Shared challenges expose no answers, pin versions; share→respond→claim | `world_claim_referral_test.py` 43/43 — one-winner race, tamper-proof scoring, authorizer proof World writes only `pciworld_*` |
| Printable documents pass dimension/QR/privacy checks | `passport_documents_test.py` 77/77 — exact page boxes, ≥0.45 mm modules, 4-module quiet zone, opaque payload, refusal matrix |
| Passport QR resolves only an opaque live URL; scan never admits anyone | `/world/pd/{hash}` one-way route, unpublish kills every printed QR; no admission surface exists |
| World/Global admin separation; backend authorization tests | `identity` and `events` permission groups outside role bundles; RBAC facts CI-gated |

## Partial / deliberately scoped out (design decisions, not omissions)

- **Event admission system** (entry passes, gates, staff scanner, offline, dynamic QR — §7A bulk):
  out of the Release-1 slice by explicit scoping; the download sheet states the QR is
  verification-only. This is the largest remaining build and needs event-operations decisions
  (holder sizes, print pilots, staffing) the spec itself assigns to later phases (5A pilot → 5B).
- **Provider APIs / comment sync** (§8.8 optional, Phase 8): unbuilt by design — the spec keeps
  them disabled pending provider approvals; the capability matrix says so honestly in the UI.
- **Server-generated share images** (1200×630 per-entity artwork): a stable branded versioned
  fallback ships; the per-entity image pipeline is future work.
- **Backfill execution + backstop cutover**: machinery is merged and tested; *running* it against
  production data, then flipping `identity_lazy_backstop` off, is an operator runbook step gated on
  a clean Health report.
- **World-app document sheet**: the documents endpoint is MyPCI-bearer-authenticated; the World app
  keeps its existing PDF. Unifying auth for it is a small follow-up.
- **Load/performance objectives (§13) and physical scan matrix (§16.7A)**: not measurable from this
  environment; the print pilot and load tests are operational tasks.

## Operator action required

1. **Render deploy is failing** for every commit (email received, commit #185): consistent with
   `DEPLOY.md` §"Deploys suddenly failing" — a service on the SQLite-in-production posture exits 78
   at health check while the prior deploy stays live. Fix in Render → Environment: managed MySQL
   (`DB_PROVIDER=mysql` + `MYSQL_*`, then `tools/migrate_sqlite_to_mysql.py`) **or**
   `ALLOW_SQLITE_IN_PRODUCTION=true`. Until then, nothing merged here is live.
2. **Run the backfill** (admin → identity → backfill preview, then run) and flip
   `identity_lazy_backstop` off once Health reports zero missing numbers.
3. Grant the new `identity` / `events` permissions to the intended admins (they are deliberately in
   no role bundle).

## Verification environment caveat (applies to every line above)

The authoring environment has no .NET SDK and no docker daemon; every C# claim rests on CI
(compile + xUnit on SQLite and MariaDB + live-HTTP integration + Playwright), never on local
execution. The Python suites run locally against the real `schema.sql` and are all CI-gated.
