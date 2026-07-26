# PCI World Community, Careers & Publishing (CCP) — Phase 0 Baseline & Audit

_Phase 0 is discovery/design only. **No feature code was changed in this phase.**_

| | |
|---|---|
| Branch | `claude/pci-world-increment-gyabvz` |
| Commit audited | `c17717d1ddfbdd55ec4b8012024ccefc20b22391` |
| Worktree at audit start | clean |
| Date | 2026-07-26 |
| Companions | `CCP_DECISION_LOG.md`, `CCP_ISSUE_REGISTER.md` |

The task specification cites commit `b678f70` as its evidence checkpoint. That commit is not the
head of this branch, so **every §3 baseline claim was re-verified against `c17717d`**. Corrections
are recorded in §2 below. (For reference, the older World ADR states baseline `47f8d51` and
`EXPANSION_PHASE0.md` states `4224641`; all three predate this audit.)

---

## 1. Environment: what could and could not be executed

This is the single most important finding of Phase 0, because it governs whether any later phase
can produce the evidence the specification demands (§26 step 7, §28.20, §31).

| Tool | State | Consequence |
|---|---|---|
| Python 3.11 | present | Backend logic suites run |
| Node 22 / npm | present | Frontend typecheck, unit tests, build run |
| **.NET SDK 8** | **absent, and not installable** | Backend cannot be compiled, booted, or unit-tested |
| **Docker daemon** | **absent** (`/var/run/docker.sock` missing) | No containerised .NET or MariaDB fallback |
| **MySQL / MariaDB server** | **absent** | No provider-authoritative integration testing |

`.NET` cannot be installed here: the environment's egress policy rejects Microsoft's distribution
hosts at the proxy.

```
$ curl -sSL https://dot.net/v1/dotnet-install.sh
curl: (56) CONNECT tunnel failed, response 403

$ curl -sS "$HTTPS_PROXY/__agentproxy/status"
"recentRelayFailures": [{ "kind": "connect_rejected",
  "detail": "gateway answered 403 to CONNECT (policy denial or upstream failure)",
  "host": "builds.dotnet.microsoft.com:443" }]
```

### 1.1 Measured baseline (what actually ran)

**Frontend — green.**

| Check | Result |
|---|---|
| `npm run typecheck` | exit 0 |
| `npm run test` (vitest) | **45 files / 296 tests passed**, exit 0 |
| `npm run build` | exit 0 (student `dist/` + admin `dist-admin/` both emitted) |

**Backend Python suites — green where they do not need `dotnet`.**

| Suite | Exit | Note |
|---|---|---|
| `lifecycle_test.py` | 0 | pass |
| `publication_test.py` | 0 | pass |
| `settings_test.py` | 0 | pass |
| `storage_test.py` | 0 | pass |
| `release_test.py` | 0 | pass |
| `casework_test.py` | 0 | pass |
| `backup_restore_test.py` | 0 | pass |
| `migration_versioning_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |
| `migration_integrity_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |
| `production_config_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |
| `worker_leasing_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |
| `payments_replay_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |
| `impersonation_readonly_test.py` | 1 | **blocked** — `FileNotFoundError: 'dotnet'` |

The six failures are **environmental, not defects**: each aborts inside `subprocess` while trying
to invoke `dotnet`, before asserting anything. No suite emitted a `FAIL`/`✗` marker.

**Not runnable at all here:** `dotnet build`, `dotnet test` (74 xUnit files incl. 18 World suites),
`smoke-test.sh`, `integration_test.py`, `sweep_500_test.py`, all Playwright E2E (27 specs), and the
entire MySQL parity gate.

### 1.2 Why this blocks Phase 1 (issue **CCP-P0-001**)

The specification's own rules make un-compilable code unacceptable:

- §26 step 7 requires focused unit + MySQL integration + API/hub authorization + E2E verification per fix.
- §28.20 — "Never state 'complete' without evidence for the exact acceptance criterion."
- §31 — acceptance requires "all affected applications build" and MySQL migrations to apply and reconcile.
- Phase 0 exit gate requires "existing critical World journeys still pass" — unverifiable here.

Phase 1's vertical slice is **almost entirely C#**: a SignalR hub, a moderation state machine, an
outbox, ~16 new tables and their migrations. Writing that without a compiler, a database, or a test
runner would produce unverified code and unevidenced claims — precisely what §28 forbids. Phase 0
is therefore complete and reported; Phase 1 is **held pending a decision** (see §7).

---

## 2. Section 3 baseline re-verification

### 2.1 §3.1 Deployment boundary — **CONFIRMED, and more favourable than assumed**

`backend/Core/WorldLifecycle.cs:13-24` — `WorldOnly.Allowed()` permits, by segment boundary:

`/` · `/world/*` · `/world-admin/*` · `/api/world/*` · `/api/world-admin/*` · `/api/health` ·
`/assets/fonts/*` · `/robots.txt` · `/world-sitemap.xml` · `/sitemap.xml` · `/favicon.{ico,svg}`

Enforced in `backend/Program.cs:447`. Main-PCI `/api/chat`, `/api/forum`, `/api/careers`,
`/api/admin/*`, `/api/me/applications` are all excluded — confirming the specification's premise
that main-PCI implementations cannot be reused through their existing URLs.

> **Every route in specification §6 already falls inside this allowlist, including the SignalR hub
> at `/api/world/hubs/community`. No change to `WorldOnly.Allowed()` is required.** This removes an
> anticipated risk.

### 2.2 §3.2 Support chat — **CONFIRMED**

`backend/Endpoints/Chat.cs`, `backend/wwwroot/assets/chat.js`. HTTP short polling at
`POLL_MS = 4000` (`chat.js:23`), polling only while the widget is open. One `chat_sessions` row =
one visitor conversation; `chat_messages.sender ∈ {visitor, bot, agent}`. No participants table, no
room concept, no visitor↔visitor path. Bot is a deterministic keyword matcher over `chat_kb`, not an LLM.

**No SignalR, `Hub`, `MapHub`, `IHubContext`, `UseWebSockets`, or SSE exists anywhere in `backend/`**
(0 grep matches across `.cs`/`.csproj`/`.json`/`.js`); no `Microsoft.AspNetCore.SignalR*` package in
`backend/PCI.Backend.csproj`. Real-time is genuinely net-new. In-house precedent exists but only in
the separate Windows solution: `secureexam/PCI.SecureExam.Server/Hubs/ProctorHub.cs`.

Minor defect noted for later cleanup: `Chat.cs:139,206` comments say gate `"content"` while the code
gates `"inbox"`; and `chat_sessions.assigned_to` / `linked_user_id` (`Migrate.cs:740-741`) are never
read or written — dead columns.

### 2.3 §3.3 Forum — **CONFIRMED**

`backend/Endpoints/Forum.cs`. Fully anonymous — author identity is a free-text `name` per post
(`Forum.cs:83,110`); `Auth` is never called on any public route; writes log `log(null, …)`.
Five categories **hardcoded in C#** (`Forum.cs:15-22`), not a table — adding one needs a redeploy.
Identity substitute is a salted truncated IP hash never selected back. Moderation is
report → 3 flags auto-hide → admin hide/restore/delete/lock. No bans, no edit, no move/merge/pin.

### 2.4 §3.4 Careers — **PARTIALLY INCORRECT in the specification; corrected here**

`backend/Endpoints/Careers.cs` (532 lines). Tables `job_postings`, `job_applications`,
`job_questions`, `job_app_events`, `career_taxonomy`, `career_email_templates` (DDL
`Data/Migrate.cs:186-247`).

| §3.4 claim | Verified reality |
|---|---|
| "inspect `backend/Endpoints/Applications.cs`" | **Wrong file.** `Applications.cs` has zero routes; it is a single helper `GrantExamEntitlement(...)` for sponsor-funded **exam** entitlements. Unrelated to job applications. |
| "scorecards" | **Absent.** Zero occurrences of "scorecard" in `backend/`. No ratings, no evaluations. |
| "exports" | **Absent.** No CSV/export endpoint; `Csv.cs` is not referenced. Only a single CV binary download. |
| "messages" | Present but **one-way only** (admin→candidate). No candidate reply endpoint. |
| "interviews" | Minimal: one `job_app_events` row with a free-text `scheduled_at` string. No panel, interviewers, ICS, reschedule, or cancel. |
| "statuses" | Flat hardcoded C# list (`Careers.cs:32`), **not** configurable stages. No stage table, no ordering, no transition gating. |
| "assignment" | Single assignee (`assigned_to`), no teams or panels. |
| "reports" | Counts by status + per-job counts + 5 totals. No funnel, time-to-hire, or source analytics. |
| anonymous apply | **Confirmed.** `POST /api/careers/{id}/apply` never checks auth (`Careers.cs:111-114`); `user_id` from `Auth.UserFromReq` is optional enrichment only (`Careers.cs:145-147`). Duplicate control is `UNIQUE(job_id, email)`, not per-user. |

**Employer/organization tenancy: none whatsoever.** `organization_id`/`org_id`/`tenant_id` appear
**0 times** in `schema.sql` and `schema.mysql.sql`. `job_postings.organisation` is a free-text
`VARCHAR` display string (`Migrate.cs:186`), not a foreign key. No admin query carries a tenant
predicate — every admin with `content` permission sees every posting and application globally
(`Careers.cs:157`). This is a **single-tenant internal job board**, not a multi-employer ATS.

Reusable at design level (not routes): `Careers.JobJsonLd` (`Careers.cs:484`), the 9-type question
taxonomy (`Careers.cs:29`), the frozen `answers_json` submit pattern (`Careers.cs:144`), and the
`{{placeholder}}` template render + `Comms.Enqueue` outbox delivery (`Careers.cs:452-476`).

### 2.5 §3.5 Passport identity — **CONFIRMED, and richer than described**

- Accounts `pciworld_users` (bcrypt). Sessions `pciworld_user_sessions`, `RandomHex(32)` stored as
  SHA-256, **30 days**, via `X-World-Account` header or HttpOnly `SameSite=Strict` cookie
  (`WorldAccount.cs:27-38,163`). Admin sessions are a separate realm, Bearer only, **8 hours**.
- **Three** student-identity bridges, not one: `LinkStudent` (`WorldAccount.cs:254`) with an
  explicit anti-hijack guard (`:272`); `WorldIdentity.Decide` LINKED/CREATED/CONFLICT rules
  (`Core/WorldIdentity.cs:146-166`); and a login-time canonical password fallback (`:124-142`).
  Handoff codes live **2 minutes**, single-use, delivered in the URL **fragment** (`:1024-1029`),
  and refuse impersonated support sessions (`:1002`).
- **An OAuth 2.1 + PKCE layer already exists** (`Endpoints/WorldOAuth.cs`, 234 lines): S256 only,
  exact redirect-URI matching, server-enforced consent for non-first-party clients, 120-second
  single-use codes, and stolen-code replay mitigation that deletes the session the code minted.
  This is the correct seam for the §17.3 cross-product switcher — it should be reused, not rebuilt.
- **Public Passport token is never authentication.** `passport_token_sha` feeds only three
  render-only GET routes; it is not consulted by `FromReq`/`AccountState` and no mutating endpoint
  accepts it. Absent/unpublished/suspended/expired all collapse to the same null (`:1201-1206`).
  Registration defaults all three disclosure flags to `0` (privacy-safe).

### 2.6 §3.6 Editorial engine — **CONFIRMED; extend, never duplicate**

`Core/WorldEditorial.cs` + `pciworld_articles` / `pciworld_article_versions` / `pciworld_sources` /
`pciworld_article_sources` / `pciworld_entities` / `pciworld_entity_mentions` /
`pciworld_article_reviews`.

State machine (`:33`): `idea → drafting → technical_review → fact_check → legal_review → seo_review
→ approved → published → archived`.

- Immutable versions: only `Snapshot()` (`:271`) writes versions (`INSERT OR IGNORE`); public
  readers get `LiveVersion()` (`:291`), never the working copy.
- `Correct()` (`:247`) is the **only** path that changes published text — requires a ≥10-char note,
  appends a dated public correction, bumps the version, re-snapshots, re-validates.
- Maker-checker enforced in `Review()`/`Approve()` (`:206,218`): approver ≠ `author_id`.
- Gates: body ≥200 words, SEO title ≤70 / desc ≤165, **news requires ≥1 source**, **any entity
  mention requires a passed legal review**. Re-validated at approve, publish, and after correction.
- Body is a constrained Markdown subset, HTML-escaped first, links limited to `https?://` or
  `/`-relative non-protocol-relative.

**Contributor intake does not exist.** Every article-write route is `POST/PUT
/api/world-admin/articles*` behind an admin bearer. No contributor/submission table exists.
This is a genuine gap to fill by extension.

### 2.7 §3.7 World admin & storage — **CONFIRMED (admin); understated (storage)**

**Admin roles are coarse and content-only** — `WorldRbac.Roles` (`Core/WorldLifecycle.cs:84`):
`owner, author, reviewer, publisher, viewer`, over five action groups
`read | author | review | publish | admin` (`:88-96`). Nothing exists for community, moderation,
employers, jobs, recruitment, privacy, or integrations. The §7.4 permission set (28 permissions,
20 roles) is entirely net-new. Good existing guards to preserve: last-active-owner cannot be
suspended or demoted, no self-role-change, role change kills that admin's sessions, password change
revokes other sessions.

**The World UI is server-rendered HTML + inline vanilla JS, with no React at all:**

| Surface | Inline JS |
|---|---|
| `Endpoints/WorldAdmin.cs` `AdminShell` | lines 923–1477 ≈ **554 lines** (~37% of the file) |
| `Core/WorldPages.cs` `WorkspaceJs` | lines 825–1083 ≈ **258 lines** |
| `Core/WorldPages.cs` `AccountJs` | lines 1706–2363 ≈ **657 lines** |
| `Core/WorldPages.cs` misc blocks | ~51 lines |

≈ **1,520 lines of inline JavaScript** across the World surfaces. `frontend/src/` contains **zero**
World code — confirming §28.15 (build new authenticated functionality in React) and §12 (migrate
incrementally).

**Storage claim is understated.** §3.7 says `DocStore.ScanClean()` is "only a basic seam". In fact
`Core/UploadScan.cs` is a **single fail-closed malware policy for every upload path** (RES-021):
built-in signatures (PE/ELF/Mach-O/Java class, EICAR, OLE/PDF embedded executables) plus an optional
external scanner via `UPLOAD_SCANNER_URL`, and it **rejects rather than passes** when a configured
scanner is unreachable. `DocStore.ScanClean` (`:107`) delegates into this.

The §3.7 conclusion still holds for the new work, but for a different reason: `UploadScan` is a
sound **malware** gate and a correct fail-closed pattern to reuse, but it performs **no image safety
work at all** — no decode/re-encode, no EXIF strip, no dimension/decompression limits, no OCR, no
visual classification, and no specialist illegal-media control. The §8.5 pipeline is net-new and
must not be represented as already covered.

### 2.8 Reusable platform infrastructure (verified)

| Asset | File | Reuse |
|---|---|---|
| Atomic worker claiming | `Core/WorkerLease.cs` | Directly — single conditional UPDATE, lease expiry recovery, MySQL+SQLite safe. Exactly what §15 requires. |
| Outbox dispatcher pattern | `Core/OutboxDispatcher.cs` | Pattern for `pciworld_community_outbox`: 15s drain, claim-per-row, exponential backoff, never throws out of the loop. |
| Fail-closed upload malware policy | `Core/UploadScan.cs` | Directly, as stage 4 of the §8.5 image pipeline. |
| Object storage abstraction | `Core/Storage.cs`, `Core/DocStore.cs`, `Core/DocAccess.cs` | Directly for quarantine + private objects. |
| Dual-provider data layer | `Data/Db.cs` | Directly. SQLite dialect authored, translated to MySQL at runtime. |
| Comms outbox + templates | `Core/Comms*.cs` | For §16 notifications. |
| HTML sanitiser | `Core/HtmlSanitize.cs` | For admin-authored content. |
| Sitemap/SEO/structured data | `Core/Sitemap.cs`, `Core/SeoTags.cs`, `Core/WorldSeo.cs` | For §18.3. |
| OAuth 2.1 + PKCE bridge | `Endpoints/WorldOAuth.cs` | For §17.3 cross-product navigation. |
| Playwright + axe harness | `frontend/e2e/` (27 specs incl. `portal-world.spec.ts`) | For §23/§24. |

### 2.9 Existing World schema (30 tables, all `pciworld_`-prefixed)

Authoring/versions: `challenges`, `challenge_versions` · Rotation: `calendar`, `rotation_periods`,
`rotation_order`, `rotation_runs`, `rotation_lock` · Anonymous play: `sessions`, `attempts`,
`invites` · Identity: `users`, `user_sessions`, `user_tokens`, `handoff_codes`, `participants`,
`user_map` · OAuth: `oauth_clients`, `oauth_codes` · Admin/audit: `admin_users`, `admin_sessions`,
`audit` · Moderation: `reports` · Editorial: `articles`, `article_versions`, `sources`,
`article_sources`, `entities`, `entity_mentions`, `article_reviews` · Analytics: `events`.

Installer `Data/WorldSchema.cs` is idempotent and runs on every boot.

---

## 3. Authoritative-model decisions (§13.0 gate)

Full rationale in `CCP_DECISION_LOG.md`. Summary:

| Domain | Decision | Rationale |
|---|---|---|
| **Jobs & applications** | **New World-authoritative model. No migration of main-PCI careers; no dual write; no shared records.** | The two are different domains, not duplicates: main-PCI careers is PCI's own single-tenant internal hiring board with anonymous apply; PCI World Careers is a multi-employer marketplace with verified tenants, Passport-only apply and immutable consented snapshots. Retrofitting tenancy + Passport-only apply onto `job_postings`/`job_applications` would break a working production feature that is out of scope, and it is unreachable from a World-only deployment anyway. Reuse is at service/design level only (JSON-LD builder, question taxonomy, frozen-answers pattern, template+outbox delivery). |
| **Forum** | **New World-authoritative model. Legacy main-PCI forum left in place, unmigrated.** | Legacy content is anonymous free-text display names with no accounts. §9.1 forbids assigning anonymous historical content to real members, and the legacy forum still serves the Institute site. An optional read-only "Legacy Guest" import is deferred with an explicit decision record, not silently dropped. |
| **Editorial** | **Extend `pciworld_articles` and its version/source/review tables. Build no second CMS.** | §3.6, §11.1 and §28.14 all require this, and the engine is genuinely strong. Contributor intake becomes additive columns plus `pciworld_contributor_*` tables keyed to `pciworld_articles.id`. |
| **Table prefix** | **`pciworld_*`, mapping specification `world_*` names 1:1.** | §13 instructs reconciliation with existing conventions rather than blind duplication. All 30 existing World tables use `pciworld_`; mixing prefixes inside one product is a durable maintenance hazard. e.g. spec `world_community_rooms` → `pciworld_community_rooms`. |

**No indefinite dual writes are proposed anywhere**, and no record is copied between products, so no
backfill or reconciliation-count job is required for the chosen shape.

---

## 4. Reuse / extend / migrate / defer matrix

| Requirement | Existing asset | Decision |
|---|---|---|
| Route boundary (§2.3) | `WorldOnly.Allowed()` | **Reuse unchanged** — all §6 routes already permitted |
| Real-time transport (§8.3) | none in `backend/` | **Build** — `AddSignalR` + hub; pattern from `ProctorHub.cs` |
| Guest sessions (§7.1) | `pciworld_sessions` (anonymous play) | **Build separate** — different lifecycle, risk keys, rules acceptance |
| Text moderation (§8.4) | none | **Build** — provider-neutral adapter + versioned policy matrix |
| Image safety (§8.5) | `UploadScan` (malware only) | **Extend** — reuse as stage 4; build the other 9 stages |
| Durable outbox (§14.6) | `Core/OutboxDispatcher.cs`, `WorkerLease` | **Reuse pattern**, new World table |
| Forum (§9) | main-PCI `forum_*` | **Build new**; legacy left in place |
| Employers/jobs/ATS (§10) | main-PCI `job_*` | **Build new**; reuse design-level services |
| Passport identity (§7.2) | `pciworld_users` + sessions | **Reuse unchanged** |
| Cross-product bridge (§17.3) | `WorldOAuth.cs`, `handoff_codes` | **Reuse** |
| Contributor publishing (§11) | `WorldEditorial` | **Extend** |
| World Admin RBAC (§7.4) | `WorldRbac` (5 roles) | **Extend** — 28 permissions, 20 roles |
| World Admin UI (§12) | ~1,520 lines inline JS | **Migrate incrementally to React**; new work React-only |
| Notifications (§16) | `Comms*` outbox | **Reuse** |
| Audit (§13.5) | `pciworld_audit` | **Extend** — actor/reason/scope/prev-state/correlation |
| Voice/video/DMs | — | **Deferred** (§4.2) |
| External ATS vendors | — | **Deferred to Phase 5**, contracts only until sandbox credentials exist |

---

## 5. P0 / P1 blockers

| ID | Sev | Title |
|---|---|---|
| **CCP-P0-001** | P0 | No .NET SDK, no Docker, no MySQL — backend cannot be built, booted, or tested (§1.2) |
| **CCP-P1-002** | P1 | §2.2 requires MySQL-authoritative; the platform ships SQLite as a supported production posture — conflict needs an owner decision |
| **CCP-P1-003** | P1 | Legal prerequisites for guest rooms/images are undecided (minimum age, jurisdictions, CSAM detection/reporting, evidence preservation) — hard gate on Phases 1–2 |
| **CCP-P1-004** | P1 | Moderation provider not selected/contracted; no PCI benchmark corpus exists for EN/AR/UR + obfuscation |
| **CCP-P1-005** | P1 | SignalR multi-instance strategy undecided (sticky sessions vs Redis backplane vs Azure SignalR) |

Detail, reproduction and proposed resolutions in `CCP_ISSUE_REGISTER.md`.

---

## 6. Phase 0 exit gate assessment — honest status

| Gate condition | Status |
|---|---|
| Architecture and data decisions are evidence-based | **Met** — §2 re-verified against `c17717d` with file:line evidence |
| Authoritative-model / cutover choices explicit | **Met** — §3, no dual writes, no silent duplication |
| Existing critical World journeys still pass | **NOT VERIFIABLE** — needs `dotnet` (CCP-P0-001) |
| No unresolved P0/P1 design blocker | **NOT MET** — CCP-P0-001 … CCP-P1-005 open |
| No destructive migration | **Met** — nothing executed; all planned migrations additive |

**Phase 0 is complete as a discovery and design phase. Its exit gate is not passed**, because one
condition cannot be evaluated in this environment and four decisions require the owner. Per the
specification's own instruction — "proceed to Phase 1 only after its ownership, migration, safety,
legal-dependency and blocker decisions are resolved" — Phase 1 has not been started.

---

## 7. Recommended next step

Phase 1's slice (room list → guest entry → SignalR join → pre-broadcast moderation → durable MySQL
message/outbox → ordered broadcast → report/eject/appeal → admin evidence → E2E + abuse tests) is
overwhelmingly C#. It needs a compiler, a MySQL instance, and a test runner to be delivered to the
standard this specification sets.

Options, in order of preference:

1. **Restore the toolchain** — allowlist `builds.dotnet.microsoft.com` / `dot.net` at the egress
   proxy, or provide an image with .NET SDK 8 and a MariaDB service. Phase 1 then proceeds normally
   with real evidence at every step.
2. **Design-complete Phase 1** — produce the full migration DDL, state machines, DTO/API contracts,
   policy matrix as data, and the test plan as reviewable artefacts, explicitly marked unbuilt and
   unverified, for implementation in an equipped environment.
3. **Frontend-first slice** — build the React World participant/admin shells and component tests,
   which are fully verifiable here (typecheck + vitest + build all pass today), and defer all
   backend work.

Option 1 is strongly preferred; the safety-critical parts of this increment should not be written
blind.
