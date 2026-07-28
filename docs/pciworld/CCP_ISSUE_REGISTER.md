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
| Status | **Verified — resolved 2026-07-26** |
| Owner | Platform / environment owner |

> **RESOLUTION.** No proxy change was needed after all. `builds.dotnet.microsoft.com` and
> `dot.net` remain blocked, but **Ubuntu 24.04's own archive carries `dotnet-sdk-8.0`**
> (`noble-updates/main`, candidate `8.0.129-0ubuntu1~24.04.1`), and `packages.microsoft.com`
> answers 200. The toolchain was restored in-container:
>
> ```
> apt-get install -y dotnet-sdk-8.0 mariadb-server mariadb-client
> $ dotnet --version   →  8.0.129        (SDK at /usr/lib/dotnet/sdk)
> $ mariadb -e 'SELECT VERSION()' →  10.11.14-MariaDB   (CI uses mariadb:10.11)
> ```
>
> Verified after restore:
> - `dotnet build -c Release` → **succeeded, 0 warnings, 0 errors**
> - `python3 tools/sqlite_to_mysql.py --check` → `schema.mysql.sql is current`
> - The five `dotnet`-dependent suites that previously aborted now pass: `migration_versioning`,
>   `production_config`, `worker_leasing`, `impersonation_readonly`, `payments_replay` — all exit 0,
>   no `FAIL`/`✗` markers.
>
> One incidental repair was required: the system `cryptography` build panics under
> `pyo3_runtime.PanicException` when imported by `pymysql`, blocking the MySQL harness. Worked
> around in-session; **this is an environment quirk, not a repository defect**, and CI is unaffected
> because it uses `actions/setup-dotnet` and a MariaDB service container.
>
> **Recommendation for future sessions:** add `apt-get install -y dotnet-sdk-8.0 mariadb-server`
> to the environment setup (or a SessionStart hook) so the backend is testable from the first turn.

**Issue (as originally found).** The .NET SDK was absent; there was no Docker daemon and no
MySQL/MariaDB server. The ASP.NET Core backend therefore could not be compiled, started, or tested.

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

**Fix applied.** Installed `dotnet-sdk-8.0` and `mariadb-server` from the Ubuntu archive (see the
resolution box above). Phase 1 can now be built and tested with real evidence at every step.

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
| Status | **Partially resolved — engineering complete, decisions outstanding** (D-007) |
| Owner | PCI legal counsel + Trust & Safety lead |

**Built (2026-07-28).** The age and jurisdiction gate is implemented, enforced at guest entry and
fail-closed: the jurisdiction allowlist ships EMPTY and an empty list admits nobody, so the service
is closed until someone with authority opens it. Counsel's values land through the world-admin
settings endpoint, which validates shape (ISO alpha-2, a sane age band) and takes no view on the
values. Refusals and admissions are recorded as evidence that the gate was applied — a coarse age
band, the jurisdiction, the minimum that applied, the outcome and the policy version. **No date of
birth is stored anywhere**, asserted by a test. Grooming-category escalation, the restricted
evidence store, two-person access and the preservation/legal-hold path shipped with Phase 2.

**This is a declaration gate, not age verification**, and the code, the admin response
(`age_assurance: "self_declared_only"`) and the participant-facing copy all say so.

**Runbooks drafted (2026-07-28).** `CCP_RUNBOOKS.md` covers emergency room abuse, legal hold,
suspected illegal material, reviewer welfare, provider outage and policy rollback. Every value that
is a legal or organisational decision is marked **[COUNSEL]** or **[T&S]** and left blank on purpose
— a plausible-looking number in a runbook is worse than an obvious gap, because it stops anybody
asking. They close the DRAFTING limb only: a runbook is a control once a named person has approved
it and the roles are filled by trained people.

**Issue.** §19.3 makes the following hard prerequisites before open guest rooms and images launch,
and none exists in the repository: minimum age and supported jurisdictions; grooming/enticement
detection and trained escalation; specialist illegal-media detection, reporting and preservation
arrangements; emergency and legal-hold runbooks; reviewer training and welfare protection.

**Impact.** Engineering can build the pipeline feature-flagged off, but the Phase 2 exit gate
("specialist legal/child-safety prerequisites are approved before images launch") cannot be signed
off. §28.6/§28.7 forbid representing a general image classifier as complete illegal-content control
or surfacing suspected illegal media in ordinary admin thumbnails.

**Still outstanding — none of it is engineering work.**

| Outstanding | Owner |
|---|---|
| The minimum age, and the list of jurisdictions PCI will serve | Counsel |
| A specialist illegal-media detection, reporting and preservation arrangement (a membership/contract, not code) | Counsel + Trust & Safety |
| Trained escalation for grooming/enticement — the people, not the route | Trust & Safety |
| Emergency and legal-hold runbooks approved for use | Counsel + Trust & Safety |
| Reviewer training and welfare protection | Trust & Safety |
| Whether identity-grade age assurance is required, given its own privacy consequences | Counsel |

**Proposed fix.** Enter the age and jurisdiction values through World Admin once counsel rules;
conclude the specialist arrangement; keep the image feature flag off until both are done.

---

## CCP-P1-004 — No moderation provider and no benchmark corpus

| Field | Value |
|---|---|
| Phase | 0 → gates 1 and 2 |
| Module | Moderation policy engine |
| Requirement | §8.4, §8.4.1 |
| Severity | **P1** |
| Status | **Partially resolved — engineering complete, corpus and contract outstanding** (D-008) |
| Owner | Trust & Safety lead + procurement |

**Built (2026-07-28).** `Core/ModerationCalibration.cs` implements the score→band mapping as
per-provider versioned data and, more importantly, the gate that decides whether a band set may be
called calibrated at all. `Certify()` refuses on: fewer than 200 samples; any category with fewer
than 30; high-band precision below 95 %; a corpus missing Arabic or Urdu (§22); fewer than 30
deliberately obfuscated examples; or a category with no configured thresholds — which is scored as
*not measured* rather than as a pile of correct Low verdicts. `MaySanctionOn()` answers **false**
for anything uncertified, so §28.5's prohibition is the default rather than a rule someone must
remember. Every shortfall is reported together, so preparing a corpus is one pass rather than a
week of discovering bars.

**Issue.** §8.4.1 requires calibrated confidence bands derived from the configured provider and a
PCI benchmark corpus, warning that a raw score from one provider does not equal another's. No
provider is selected or contracted, and no corpus exists for English, Arabic, Urdu, code-switched
or obfuscated text.

**Impact.** The provider-neutral adapter and the versioned policy matrix can be built and unit
tested with synthetic fixtures, but the bands cannot be calibrated, so the §8.4.1 decision matrix
cannot be validated and the Phase 1 exit gate ("strict high-confidence ejection and ambiguous
quarantine work") cannot be evidenced against real language. §28.5 additionally forbids turning a
low-confidence result into an irreversible sanction — untuned bands would do exactly that.

**Still outstanding — none of it is engineering work.**

| Outstanding | Owner |
|---|---|
| Select and contract a moderation provider | Procurement |
| Author and human-label the PCI benchmark corpus (English, Arabic, Urdu, code-switched, obfuscated) | Trust & Safety |
| Run the corpus against the contracted provider and record the certified bands | Trust & Safety |

No amount of engineering substitutes for real labelled examples, and this repository must not
manufacture them. What exists now is the harness, the scoring, the bars and the refusal — so the day
a corpus and a provider exist, calibrating is running a report rather than writing a system.

`Core/ModerationCorpus.cs` completes that path: the corpus is a JSON-lines file Trust & Safety drops
in, strict by default — a malformed line is an error carrying its line number, never a row quietly
dropped, because half a corpus that looks whole would produce a report as convincing as a correct
one. A missing `harmful` label is refused rather than defaulted (defaulting it would invent a
reviewer's judgement), and a mistyped category is caught at load rather than surfacing later as an
unexplained "no category was scored".

**Not built, deliberately:** an HTTP provider adapter. The `ITextModerator` seam is the right level
of readiness — writing a concrete wire format before a vendor is contracted would be guessing at
somebody's API and would need rewriting when the real one arrives.

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

## CCP-P2-008 — World unit suites race each other on the shared MySQL test database

| Field | Value |
|---|---|
| Phase | 1 |
| Module | `backend/tests/PCI.Backend.Tests` harness |
| Requirement | §23 (provider-sensitive integration tests must run on real MySQL) |
| Severity | **P2** |
| Status | **Reproduced** — pre-existing, not introduced by this increment |
| Owner | Platform / test harness |

**Issue.** All 17 existing World test classes call `WorldSchema.Ensure(db)`, and **none** declares
`[Collection(DbCollection.Name)]`. xUnit runs distinct collections in parallel, and on MySQL every
class shares one run database, so they race on `WorldSchema`'s non-idempotent admin seed.

**Reproduction** (commit `6c7ee0f`, MariaDB 10.11.14, `TEST_DB_PROVIDER=mysql`):

```
dotnet test --filter "…CommunitySchemaTests|…WorldEditorialTests"
  → MySqlException: Duplicate entry 'owner@pciworld.local' for key 'email'
  → MySqlException: Duplicate entry 'why-your-spi-recovers-as-the-project-ends' for key 'slug'

dotnet test --filter "…WorldEditorialTests"      (alone)  → 12/12 PASSED
```

**Evidence it is pre-existing.** With this increment's changes **stashed**, the full unit suite
against MySQL on this machine reports **170 failed / 991 passed / 1164 total**, spanning suites this
increment never touches (`SimStepTests`, `CommsReminderTests`, `WorldRotationTests`). With the
changes applied the failures are equivalent. The contention scales with core count: CI's
`backend-unit-mysql` job passes on a 2-core runner, while this container has substantially more.

**Consequence for verification.** The full MySQL unit run is **not a usable local gate** on a
high-core machine. The gates that do hold: the full SQLite suite (**1177/1177**), per-suite MySQL
runs in isolation, and CI's own `backend-unit-mysql` job.

**Proposed fix.** Either add `[Collection(DbCollection.Name)]` to the World suites so they serialise
against the shared database, or make `WorldSchema`'s admin/article seeds idempotent
(`INSERT OR IGNORE`) so a concurrent re-seed is a no-op. The second is preferable — it fixes the
race rather than hiding it behind serialisation, and it costs no test wall-clock. **Deliberately not
done here:** it touches 17 existing test files or a load-bearing seed path, neither of which belongs
in a community-rooms commit.

**Risk if unfixed.** A latent flake that widens as CI runners gain cores, and a misleading local
signal that invites engineers to "fix" failures they did not cause — which cost real time in this
session before the stashed baseline settled it.

---

## CCP-P2-009 — Hub connect-time authorization is not covered by an automated test

| Field | Value |
|---|---|
| Phase | 1 |
| Module | `backend/Core/CommunityHub.cs` |
| Requirement | §8.3 (hub authorization re-checked per invocation), E2E-012 |
| Severity | **P2** |
| Status | **Deferred** — needs a WebSocket-capable test client |
| Owner | CCP |

**Issue.** `CommunityHub.OnConnectedAsync` aborts any connection without a valid guest session, and
`Acknowledge`/`Replay` re-check the session on every invocation so an ejected participant's live
connection stops working. **None of that is exercised by an automated test.**

The live suite asserts only that the transport is mounted (C32), because SignalR's `negotiate`
handshake is unauthenticated *by design* — authorization happens at connect. A passing `negotiate`
therefore proves reachability and nothing about authorization, and the assertion is named to say so
rather than implying broader coverage.

**Why not covered.** Driving a real connection needs a WebSocket client; neither `websockets` nor
`websocket-client` is available in the test environment, and the repository's Python suites are
deliberately dependency-light (stdlib + `pymysql`/`pypdf`).

**What IS covered meanwhile.** The security property has a second, tested enforcement point: the
HTTP accept path re-resolves the session on every send, so an ejected guest cannot post regardless
of connection state (`C19` asserts the session is dead immediately after ejection). The hub cannot
be used to send at all — sending is not a hub method — so the untested surface is limited to
*receiving* broadcasts and replay.

**Proposed fix.** Add a Playwright spec that opens a real SignalR connection (the repo already has
a Playwright harness with browser context), asserting: an anonymous connect is refused; an ejected
guest's live connection stops receiving; and a reconnect replays only messages after the
acknowledged sequence. That is E2E-012 and part of E2E-028.

**Risk if unfixed.** A regression in `OnConnectedAsync` would let an unauthenticated client receive
room broadcasts, and no test would fail.

---

## CCP-P2-010 — An IP-derived risk key restricts everyone behind a shared address

| Field | Value |
|---|---|
| Phase | 1 |
| Module | `backend/Endpoints/CommunityPublic.cs` (`RiskKey`), `pciworld_risk_restrictions` |
| Requirement | §7.1 (layered controls, honest limits), §28.5 (no irreversible sanction on weak signal) |
| Severity | **P2** |
| Status | **Accepted for Release 1, documented** |
| Owner | Trust & Safety + Platform |

**Issue.** The abuse identifier is `SHA256(pepper ‖ client-IP ‖ rotation-period)`. Ejecting one
participant therefore restricts **every visitor sharing that IP** — an office NAT, a university, a
school, a café, or carrier-grade NAT. CGNAT is the common case on mobile networks across much of the
world, including regions PCI World specifically targets for Arabic and Urdu participants, so the
collateral is not a corner case.

**How it was found.** A live-suite failure, not review: ejecting one guest made an unrelated guest's
re-entry fail with `access_restricted`, because the harness ran every simulated participant from
`127.0.0.1`. The test was fixed to give each participant its own forwarded hop, which is realistic —
but the underlying product behaviour is unchanged and real.

**Why it is accepted rather than removed.** §8.6 requires a temporary guest restriction to exist as
a sanction, and an ejection with no consequence is not a sanction. The mitigations already in place:

- the restriction is **room-scoped and 24 hours**, not global or permanent;
- it is **appealable without an account** — the ejected guest is handed a reference and credential in
  the ejection response itself, and an overturn lifts the restriction immediately (asserted by
  `C54`–`C61`);
- §7.1's honesty requirement is met — this is described as a deterrent, never as ban enforcement,
  and clearing a cookie or changing network defeats it anyway.

**Proposed fix (post-Release 1).** Blend device-level and behavioural signals into the key so a
single shared address is not the whole identity, and downgrade an IP-only match from a hard block to
a risk score that raises the proof-of-human bar rather than refusing entry. Both need the abuse
telemetry that only real traffic provides, so they are deliberately not guessed at now.

**Risk accepted.** During Release 1 a genuine participant behind a shared address may be refused
entry for up to 24 hours because of someone else's behaviour. They can appeal, and the appeal path is
tested end to end — but they should not have to, and this should not survive contact with scale.

---

## CCP-P2-011 — Layout-dependent accessibility rules have no automated coverage

| Field | Value |
|---|---|
| Phase | 1 |
| Module | `frontend/src/world/community/` |
| Requirement | §18.1 (WCAG 2.2 AA), PW-US-040/041 |
| Severity | **P2** |
| Status | **Verified — closed** (browser pass added, see resolution) |
| Owner | CCP |

**Issue.** The community UI has an automated axe pass
(`CommunityApp.a11y.test.tsx`, 6 scans across catalogue, empty state, entry form, entry error, room
transcript and ejection screen). It runs in jsdom, which has **no rendering engine**, so a family of
WCAG rules cannot be evaluated and axe silently skips them:

- **colour contrast** (1.4.3) — explicitly disabled in the scan rather than left to report a false pass
- **target size** (2.5.8) — likewise
- **reflow at 400% zoom** (1.4.10) and **text spacing** (1.4.12)
- **focus-visible appearance** (2.4.11/2.4.13)
- anything depending on real focus order or scroll behaviour

**What IS covered.** The structural rules — accessible names, form labelling and error association,
ARIA validity, heading and landmark order, list semantics. Those are the ones this interface was
most likely to get wrong, and the pass **already caught a genuine defect**: `role="log"` had been
placed directly on the transcript `<ol>`, which overrides the implicit list role and orphans every
`<li>` from the accessibility tree. Written by hand, believed correct, and wrong. The live region is
now a wrapper and a regression test pins the list role.

**Why not closed now.** A Playwright axe run would need `/world-app/` served, and CI's `e2e` job does
not build the React bundles into `wwwroot` — closing this means changing a shared job, which is a
larger change than belongs in the same commit as the UI it would test.

**Proposed fix.** Add a bundle-build step to the `e2e` job, then a `community-a11y.spec.ts` that
enables the feature flag, seeds a room, and runs axe with contrast and target-size **enabled** at
several viewports, plus a keyboard-only traversal and a 400% zoom reflow check.

**RESOLUTION.** Closed by `frontend/e2e/community-a11y.spec.ts` — 7 checks in a real Chromium, with
`colour-contrast`, `target-size` and the rest **enabled**, over the catalogue, entry screen, entry
error state, populated transcript, keyboard-only traversal and a 320px reflow check. The CI `e2e`
job now builds the React bundles into `wwwroot` first, because Playwright runs the backend directly
rather than the Docker image, so without that step every React route 404s.

Enabling the feature in the spec goes through a new `PATCH /api/world-admin/community/settings`
rather than reaching into the database, so the fixture exercises the same path an operator uses and
cannot drift from it. That endpoint allowlists the two community keys instead of writing whatever it
is given — a settings route that takes arbitrary keys is a privilege-escalation primitive.

Two defects in the spec itself were found and fixed rather than worked around: an empty `<ol>` is not
exposed as a list by Chromium, so the list-role regression guard had to run against a populated
transcript; and the tests shared a room with fixed display names, so the second to run was refused
`name_taken` — names are now unique per test, which is what the product enforces anyway.

**Residual.** Only Chromium runs this spec. Firefox and WebKit have their own accessibility-tree
quirks (the empty-list difference above is exactly that class of thing), so a cross-browser pass
remains worthwhile but is not blocking.

---

## CCP-P2-012 — Phase 2 is built and tested, but its exit gate cannot be signed off here

| Field | Value |
|---|---|
| Phase | 2 |
| Module | Community image safety |
| Requirement | §8.5, §19.3, §31, §28.6, §28.7 |
| Severity | **P1** |
| Status | **Blocked** (on CCP-P1-003 and CCP-P1-004) |
| Owner | PCI legal counsel + Trust & Safety lead + procurement |

**Where the code stands.** The pipeline is complete and exercised on both providers: sanitiser
(re-encode, EXIF/GPS removal, header-bounded decode, SVG and animation refused), image policy
matrix, durable scan queue with crash recovery, upload and serve endpoints, the moderation surface
with two-person access to restricted evidence, retention exemption for held material, and the
participant UI. `pciworld_community_images_enabled` is seeded `'0'` and a per-room grant is also
required.

**Why the gate is still open.** Nothing in this repository can satisfy §19.3's prerequisites
(CCP-P1-003) or calibrate the confidence bands against a real provider and corpus (CCP-P1-004).
Until both land:

- the flag stays off, and the admin surface reports the outstanding prerequisites rather than a tick;
- the bands driving the matrix are **uncalibrated**, which is why no image rule ejects anybody
  (§28.5) — asserted as a property over the whole table, not row by row;
- the restricted path is a *container and escalation route*, not detection. §28.6 forbids claiming a
  general classifier finds every illegal image, and this system makes no such claim: it routes
  suspicion to trained people and preserves the material under a hold.

**What would close it.** Written counsel decisions per CCP-P1-003; a contracted provider plus the
PCI benchmark corpus per CCP-P1-004; then band calibration recorded as versioned policy data, and a
deliberate decision by a named person to turn the flag on.

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
