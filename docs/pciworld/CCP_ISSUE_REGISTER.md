# PCI World CCP — Issue Register

Companion to `CCP_PHASE0_BASELINE.md`. Per specification §29. Nothing blocked or deferred is
hidden; each such row names its exact dependency.

Severity per §26 step 4. Status: Discovered · Reproduced · Diagnosed · Specified · Fix in progress ·
Fixed · Testing · Verified · Deferred · Blocked · Not reproducible.

---

## CCP-P0-001 — Backend cannot be built, booted or tested in this environment

| Field | Value |
|---|---|
| Phase | 0 |
| Module | Toolchain / CI environment |
| Requirement | §23, §24, §26 step 7, §28.20, §31; Phase 0 exit gate |
| Severity | **P0** |
| Status | **Blocked** |
| Owner | Platform / environment owner |

**Issue.** The .NET SDK is absent and cannot be installed; there is no Docker daemon and no
MySQL/MariaDB server. The ASP.NET Core backend therefore cannot be compiled, started, or tested.

**Reproduction** (branch `claude/pci-world-increment-gyabvz`, commit `c17717d`, no feature flags):

```
$ dotnet --version
bash: dotnet: command not found
$ ls -d ~/.dotnet /usr/share/dotnet /usr/lib/dotnet
ls: cannot access ... : No such file or directory        # all three

$ curl -sSL https://dot.net/v1/dotnet-install.sh
curl: (56) CONNECT tunnel failed, response 403
$ curl -sS "$HTTPS_PROXY/__agentproxy/status"
  "kind": "connect_rejected",
  "detail": "gateway answered 403 to CONNECT (policy denial or upstream failure)",
  "host": "builds.dotnet.microsoft.com:443"

$ docker info
failed to connect to the docker API at unix:///var/run/docker.sock: no such file or directory

$ which mysqld mariadbd            # no output
```

Expected: `dotnet build -c Release` succeeds and the xUnit + integration + smoke suites run.
Actual: no compiler, no runtime, no database, and no container fallback.

**Root cause.** Environment provisioning. `AGENTS.md` documents .NET SDK 8 at `~/.dotnet`, but that
describes a *Cursor Cloud* container; this Claude Code remote container was provisioned without it,
and the egress policy blocks Microsoft's distribution hosts.

**Impact — what cannot be produced:**

- `dotnet build`, `dotnet test` (74 xUnit files, including 18 World suites)
- `smoke-test.sh`, `integration_test.py`, `sweep_500_test.py`
- All 27 Playwright E2E specs (need a running backend)
- The entire MySQL parity gate
- 6 Python suites that shell out to `dotnet`: `migration_versioning`, `migration_integrity`,
  `production_config`, `worker_leasing`, `payments_replay`, `impersonation_readonly` — each aborts
  with `FileNotFoundError: [Errno 2] ... 'dotnet'` before asserting anything. **These are
  environmental failures, not code defects.**

**What still verifies here:** frontend typecheck (exit 0), 296 vitest tests across 45 files
(exit 0), production build (exit 0), and 7 Python logic suites against real SQLite (all exit 0).

**Proposed fix.** Allowlist `builds.dotnet.microsoft.com` and `dot.net` at the egress proxy, **or**
provide a session image with .NET SDK 8 plus a MariaDB service (CI already uses `mariadb:10.11`).

**Risk if not fixed.** Phase 1 is overwhelmingly C# — SignalR hub, moderation state machine,
outbox, ~16 tables and migrations. Delivering it here would mean shipping uncompiled, untested,
safety-critical code and making completion claims with no evidence, which §28.3/§28.20 forbid.

---

## CCP-P1-002 — §2.2 MySQL-authoritative conflicts with the shipped SQLite production posture

| Field | Value |
|---|---|
| Phase | 0 |
| Module | `Program.cs` boot validator / `Data/Db.cs` |
| Requirement | §2.2 |
| Severity | **P1** |
| Status | **Blocked** (decision recorded as D-005, Proposed) |
| Owner | Platform owner |

**Issue.** §2.2 requires MySQL authoritative in local integration testing, staging and production,
with SQLite confined to isolated unit tests and explicitly not a production/development fallback.
The platform deliberately ships the opposite: `Program.cs:110-134` permits SQLite in production on
a writable `/data` mount, under `ALLOW_SQLITE_IN_PRODUCTION`, or under
`PCIWORLD_ONLY` + `PCIWORLD_ALLOW_SQLITE`.

**Evidence.** `Program.cs:88` states in-code that a previous fail-closed MySQL change "bricked every
existing SQLite-on-disk deploy" — this posture is a deliberate remediation, not an oversight.

**Proposed fix (D-005).** Scope the requirement to the increment: CCP features refuse to enable on a
non-MySQL provider; existing surfaces keep the current posture; CCP integration tests run on real
MySQL only. Satisfies §2.2's intent for new work without an unrelated production outage.

**Dependency.** Owner decision — this changes the operating posture of a live deployment.

---

## CCP-P1-003 — Legal prerequisites for guest rooms and image sharing are undecided

| Field | Value |
|---|---|
| Phase | 0 → gates 1 and 2 |
| Module | Community / image safety / trust & safety operations |
| Requirement | §7.1, §8.5, §19.3, §31 |
| Severity | **P1** |
| Status | **Blocked** (D-007) |
| Owner | PCI legal counsel + Trust & Safety lead |

**Issue.** §19.3 makes the following hard prerequisites before open guest rooms and images launch,
and none exists in the repository: minimum age and supported jurisdictions; grooming/enticement
detection and trained escalation; specialist illegal-media detection, reporting and preservation
arrangements; emergency and legal-hold runbooks; reviewer training and welfare protection.

**Impact.** Engineering can build the pipeline feature-flagged off, but the Phase 2 exit gate
("specialist legal/child-safety prerequisites are approved before images launch") cannot be signed
off. §28.6/§28.7 forbid representing a general image classifier as complete illegal-content control
or surfacing suspected illegal media in ordinary admin thumbnails.

**Proposed fix.** Obtain written counsel decisions before Phase 1 exit; keep the image feature flag
off until then. Build the restricted evidence store and escalation path so the controls exist when
the decisions land.

---

## CCP-P1-004 — No moderation provider and no benchmark corpus

| Field | Value |
|---|---|
| Phase | 0 → gates 1 and 2 |
| Module | Moderation policy engine |
| Requirement | §8.4, §8.4.1 |
| Severity | **P1** |
| Status | **Blocked** (D-008) |
| Owner | Trust & Safety lead + procurement |

**Issue.** §8.4.1 requires calibrated confidence bands derived from the configured provider and a
PCI benchmark corpus, warning that a raw score from one provider does not equal another's. No
provider is selected or contracted, and no corpus exists for English, Arabic, Urdu, code-switched
or obfuscated text.

**Impact.** The provider-neutral adapter and the versioned policy matrix can be built and unit
tested with synthetic fixtures, but the bands cannot be calibrated, so the §8.4.1 decision matrix
cannot be validated and the Phase 1 exit gate ("strict high-confidence ejection and ambiguous
quarantine work") cannot be evidenced against real language. §28.5 additionally forbids turning a
low-confidence result into an irreversible sanction — untuned bands would do exactly that.

**Proposed fix.** Select and contract a provider; author the PCI corpus (including Arabic/Urdu and
obfuscation fixtures per §22); calibrate and record bands as versioned policy data.

---

## CCP-P1-005 — SignalR multi-instance strategy undecided

| Field | Value |
|---|---|
| Phase | 0 → 1 |
| Module | Real-time transport |
| Requirement | §8.3, §20 |
| Severity | **P1** |
| Status | **Blocked** (D-006) |
| Owner | Platform / DevOps |

**Issue.** SignalR is net-new to `backend/` (verified: 0 matches for SignalR/Hub/WebSocket/SSE
across `backend/**`; no SignalR package in `PCI.Backend.csproj`). §8.3 requires a *measured*
deployment choice between sticky sessions, a Redis backplane with documented outage semantics, or
Azure SignalR. Production topology and budget are not recorded in the repository.

**Mitigation already settled.** MySQL plus a durable outbox remain authoritative for accepted
messages, so reconnect recovers by sequence regardless of the transport choice; and the hub route
`/api/world/hubs/community` already falls inside `WorldOnly.Allowed()`, so no allowlist change is
needed.

**Proposed fix.** Decide the topology before Phase 1 exit, or ship Release 1 single-instance with
the scale-out path documented and the drain/reconnect behaviour tested (PW-US-044).

---

## CCP-P2-006 — Specification §3.4 misstates the careers baseline

| Field | Value |
|---|---|
| Phase | 0 |
| Module | Documentation / planning input |
| Requirement | §3.4, §3.8 |
| Severity | **P2** |
| Status | **Verified** (corrected in `CCP_PHASE0_BASELINE.md` §2.4) |

**Issue.** §3.4 describes the existing careers module as already including "scorecards … reports,
exports" and directs inspection of `backend/Endpoints/Applications.cs`. Verified against `c17717d`:

- **Scorecards do not exist** — zero occurrences of "scorecard" in `backend/`.
- **Exports do not exist** — no CSV/export endpoint; `Csv.cs` is not referenced by `Careers.cs`.
- **`Applications.cs` is not a careers file** — it has zero routes and is a single helper
  `GrantExamEntitlement(...)` for sponsor-funded *exam* entitlements.
- Messages are **one-way** (admin→candidate) with no candidate reply endpoint; interviews are one
  free-text event row; statuses are a flat hardcoded C# list, not configurable stages.
- **Employer tenancy does not exist** — `organization_id`/`org_id`/`tenant_id` appear **0 times**
  in `schema.sql` and `schema.mysql.sql`; `job_postings.organisation` is a free-text display string.

**Impact.** The ATS effort in Phase 4 is materially larger than the specification implies —
scorecards, exports, bidirectional messaging, real interview objects, configurable stages and the
entire employer tenancy layer are all net-new rather than reusable.

---

## CCP-P3-007 — Pre-existing minor defects in main-PCI chat (out of CCP scope)

| Field | Value |
|---|---|
| Phase | 0 |
| Module | `backend/Endpoints/Chat.cs`, `Data/Migrate.cs` |
| Severity | **P3** |
| Status | **Deferred** — main-PCI support chat, outside this increment's scope (§2.1) |

1. Comment/code mismatch: `Chat.cs:139` and `Chat.cs:206` say "gated: content" while every admin
   route actually gates `"inbox"`. The code is correct; the comments mislead reviewers.
2. Dead columns: `chat_sessions.assigned_to` and `chat_sessions.linked_user_id`
   (`Migrate.cs:740-741`) are never read or written by `Chat.cs` or `admin-chat.html`.

Neither affects PCI World. Recorded so they are not rediscovered as new findings later.

---

## Deferred scope (explicit, per §4.2 and §26 step 10)

| Item | Reason | Target |
|---|---|---|
| Voice/video rooms, live streaming, recording | §4.2 — needs a separately approved safety phase (transcription, multilingual audio moderation, consent/recording rules, specialist staffing, child-safety/legal review) | Post-Release 1 |
| Guest direct messages | §4.2 — no DMs in Release 1 | Post-Release 1 |
| Legacy main-PCI forum import as read-only "Legacy Guest" content | D-002 — needs an editorial judgement on which threads merit the PCI World brand | Post-Release 1, Editorial owner |
| Named external ATS vendor integrations | §10.8 — contracts only until sandbox credentials exist; §28 forbids marking integration complete on mocks | Phase 5, gated on credentials |
| Platform-wide SQLite → MySQL cutover | Separate existing programme (`docs/MYSQL_MIGRATION.md`); CCP scopes its own requirement via D-005 | Separate programme |
| Automated AI candidate ranking/rejection | §4.2, §10.7 — prohibited in Release 1 | Not planned |
