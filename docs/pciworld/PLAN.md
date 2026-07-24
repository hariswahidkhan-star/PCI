# PCI World — Phased Plan, Traceability, Test Plan, Rollback

## Phase status

| Phase | Scope | Status |
|---|---|---|
| **0 — Foundation** | ADR, threat model, data/content model, schema on both providers, separate admin realm, validator, Institute-link config | **This change** |
| **1 slice — Anonymous vertical journey** | Premium server-rendered home/challenge/result, 10 reviewed pilot challenges, anonymous start→submit→result, deterministic scoring + decision profiles, share/verify tokens, challenge-a-friend, separate admin author→review→approve→publish→revise→retire with audit + calendar | **This change** |
| 1b — Accounts & Passport | pciworld_users, email verification, basic Passport, save-after-result upsell | Backlog (next) |
| 2 — Foundation launch | 30 reviewed challenges, archive search/filter, Decision Replay UI, Coach (behind the AI gate below), content correction flow, full E2E/a11y/security passes | Backlog |
| 3 — Retention | 90 challenges, daily calendar automation, recommendations, Human vs AI, return comms, friend comparison views | Backlog |
| 4 — Ecosystem | 180 challenges, World Project Series, real university cohorts, real employer missions, governed talent opt-in, ranking thresholds | Backlog |
| 5 — Annual library | 365 unique approved challenges, localization expansion, load/DR review | Backlog |

Content counts are never claimed ahead of review: this slice ships **10 pilot challenges**, each
passing the deterministic validator and reference solve in CI. 30/90/180/365 are release gates,
not seeds.

## Traceability to the master prompt (§ = prompt section)

| Requirement | Where |
|---|---|
| §1 distinct product, separate admin, no links from PCI admin | `/world*` module, `pciworld_admin_*` realm, `/world-admin` UI; zero references in `frontend/src/admin`; grep-tested |
| §1 MySQL 8 production | Platform `Db` provider translation; `WorldSchema` runs on both providers; migration-parity gate |
| §2 Institute links + transparency | Fixed header/footer strings on every page (`WorldPages`), settings-driven URL |
| §2 no certification implication | `WorldPages.PracticeNotice` constant on challenge, result, verification, about |
| §7–8 content model, immutable versions | `pciworld_challenge_versions`, validator, publish flow |
| §9 workspace behaviours | autosave, idempotent submit, resume, no leakage (allow-list public view) |
| §10 deterministic scoring | `WorldScore` + `SimCalc`; stored dimensions, versions, audit |
| §11–12 profiles + result page | `WorldScore.Profile`, result page with dimensions, consequences, next step |
| §13–14 share + invite | opaque revocable tokens, OG metadata, no PII in URLs, WhatsApp/LinkedIn/X links |
| §19 no empty leaderboards | none rendered; thresholds documented before any ranking ships |
| §21 admin lifecycle + maker-checker | `WorldAdmin` endpoints; approver ≠ author enforced server-side |
| §23 integrity | synthetic-data declaration required by validator; no real-world proprietary content in pilots |
| §26 security | THREAT_MODEL.md and its listed mitigations, each with a test |
| §35 definition of done | Honest partial: this table + final report state exactly what shipped |

## Test plan (this slice — all runnable in CI today)

- Unit (xunit): validator accept/reject matrix; scoring accuracy/tolerance/partial credit;
  profile mapping determinism; version pinning and immutability (publish → revise → old attempt
  replays identically); token opacity (sha stored, revocation); lifecycle transitions incl.
  maker-checker refusal; RBAC per role; cross-realm token rejection; anonymous-session ownership.
- Existing suites must stay green: full backend xunit, Simulation Lab filter, `integration_test.py`,
  `migration_integrity_test.py` (provider parity picks up `pciworld_*` tables automatically).
- Deferred (with their phases): Playwright public/admin journeys, axe/a11y automation, Arabic/RTL
  passes, load tests at release boundaries, MySQL-live integration in CI, AI red-team gate.

## AI Coach gate (must pass before Coach ships in Phase 2)

grounding-only answers; evidence citation; deterministic-tool delegation; assessment/reveal
withholding; prompt-injection resistance; no cross-user data; Arabic quality; latency/cost
budget; provider-failure degradation. (Mirrors the Simulation Lab coach eval suite.)

## Rollback & recovery

- The module is additive: no existing table, endpoint or page is modified. Rollback = revert the
  PR; `pciworld_*` tables are inert if the code is absent (installer is idempotent, data is
  preserved for re-deploy).
- Kill switch: `site_settings.world_enabled='0'` returns 403/holding copy on all `/world*` and
  `/api/world*` surfaces without redeploying.
- Backups: `pciworld_*` rides the platform's existing backup/restore (no separate datastore).
- Publication mistakes: `retire` hides a challenge from rotation instantly; published versions
  stay for historical replay; a corrected revision publishes as a new version.

## Explicit non-claims

Not built in this change (do not present as existing): participant accounts, Passport, email,
share-image PNG rendering, Coach, Human vs AI, World Project Series, universities, employers,
rankings, localization beyond English copy structure, Playwright/a11y automation for `/world`,
MFA on world-admin, CAPTCHA, separate hostname wiring (documented deployment mapping only).
