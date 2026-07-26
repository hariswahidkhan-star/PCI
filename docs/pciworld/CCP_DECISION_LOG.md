# PCI World CCP — Decision Log

Companion to `CCP_PHASE0_BASELINE.md`. One row per architectural decision, per specification §29.

Status values: **Accepted** (decided, evidence-based, within delegated authority) ·
**Proposed** (recommendation recorded; needs owner sign-off before it binds) ·
**Blocked** (cannot be decided without an external input).

---

## D-001 — Authoritative model for jobs and applications

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |
| Context | §13.0 requires exactly one authoritative model. Main-PCI has `job_postings` / `job_applications` (+ 4 tables), single-tenant, anonymous apply, admin-scoped under `/api/admin/careers`, and unreachable from a World-only deployment. |

**Options considered**

1. *Extract and evolve the existing tables with World ownership/tenant fields.* Rejected: forces
   employer tenancy, job versioning, Passport-only apply and immutable consented snapshots onto a
   working production feature that is explicitly out of scope, with a large blast radius on
   main-PCI's own hiring board. It also cannot serve a World-only deployment without opening
   `/api/careers`, which §2.3 forbids.
2. *Migrate/backfill main-PCI careers into a World-authoritative model with a timed cutover.*
   Rejected: main-PCI still needs its internal board. Migration would move records out from under a
   live feature for no product benefit — the two audiences are disjoint.
3. *Shared canonical model used by both products with product-scoped APIs.* Rejected for Release 1:
   requires rewriting main-PCI careers concurrently with building the World ATS, doubling risk on
   both. Reconsider only if PCI later wants its own vacancies published on World.
4. **Selected — a new World-authoritative model, with main-PCI careers left untouched and out of
   scope.**

**Reason.** These are different domains, not duplicates. Main-PCI careers is PCI's single-tenant
internal vacancy board where anyone may apply anonymously by name/email. PCI World Careers is a
multi-employer marketplace with verified employer tenants, organization-scoped RBAC, governed job
versioning, Passport-only applications, and immutable consented application snapshots. Not one of
those properties exists today (verified: `organization_id` appears **0 times** in either schema
file; `job_postings.organisation` is free text).

**Why this is not a §28.14 violation.** §28.14 forbids "a second disconnected careers engine
*without a documented migration decision*". This entry is that decision: ownership is disjoint, no
record is shared or copied, there is no dual write, and historical URL behaviour is unchanged
because main-PCI keeps `/careers/*` while World serves `/world/careers/*`.

**Reuse retained (design level, not routes):** `Careers.JobJsonLd` structured-data builder
(`Careers.cs:484`); the 9-type screening-question taxonomy (`:29`); the frozen `answers_json`
submit-time snapshot pattern (`:144`); `{{placeholder}}` template rendering delivered through
`Comms.Enqueue` (`:452-476`).

**Migration/rollback impact.** Additive only — new tables, no ALTER of main-PCI careers tables, no
backfill, no reconciliation counts needed. Rollback is dropping the new tables behind the feature flag.

---

## D-002 — Authoritative model for the forum

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |
| Context | Main-PCI `forum_threads` / `forum_posts` / `forum_actions` is fully anonymous (free-text `name`, no `Auth` call on any public route), with 5 categories hardcoded in C# (`Forum.cs:15-22`), and is unreachable from a World-only deployment. |

**Selected:** a new World-authoritative forum model. The legacy main-PCI forum stays in place,
unmigrated, continuing to serve the Institute site.

**Reason.** The requirement is a Passport-authenticated professional forum with dynamic taxonomy,
trust levels, versioned posts and structured data. Legacy rows carry no account identity at all —
only a display name and a one-way IP hash. §9.1 explicitly forbids assigning anonymous historical
content to real members, so there is nothing to migrate *into* a member-authored model.

**Deferred, not dropped.** Importing legacy threads as read-only "Legacy Guest" content is a
genuine option §9.1 offers. It is deferred to a post-Release-1 decision because it needs an
editorial judgement on which legacy threads are worth surfacing under the PCI World brand. Owner:
Editorial. Recorded here so it is not silently lost.

**Migration/rollback impact.** Additive only.

---

## D-003 — Editorial engine: extend, never duplicate

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |
| Context | §3.6, §11.1 and §28.14 all require extending the existing World editorial engine. Audit confirms it is strong: immutable versions, corrections-only-with-note, maker-checker, sources with claim linkage, entity mentions gated on legal review, four independent review kinds. Contributor intake does not exist. |

**Selected:** extend `pciworld_articles` and its satellite tables. Build no second CMS.

**Shape.** Additive columns on `pciworld_articles` (contributor World user reference distinct from
the editorial owner; contributor-terms version; conflict / sponsorship / AI-assistance / originality
/ rights declarations) plus new `pciworld_contributor_*` tables (submission status history, editor
assignments, contributor↔editor message thread, media-rights metadata, withdrawal and correction
requests) keyed to `pciworld_articles.id`.

**Invariants preserved.** `Snapshot()` remains the only writer of versions; `Correct()` remains the
only path that changes published text; maker-checker (`approver ≠ author_id`) extends to cover
contributor authors so §11.3 "an author cannot approve their own article" holds through the API,
not only the UI.

**Migration/rollback impact.** Additive columns + new tables; no rewrite of existing article rows.

---

## D-004 — Table naming: `pciworld_` prefix

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |
| Context | Specification §13 names new tables `world_*`. All 30 existing World tables use `pciworld_*`. |

**Selected:** use `pciworld_*` for all new tables, mapping specification names 1:1 (e.g. spec
`world_community_rooms` → `pciworld_community_rooms`, `world_job_applications` →
`pciworld_job_applications`).

**Reason.** §13 opens by instructing "First reconcile these names with existing schema and
conventions. Do not blindly create duplicates." Two prefixes for one product's tables is a durable
maintenance and review hazard, and the World-only deployment's mental model is "everything
`pciworld_`". The mapping is mechanical and recorded, so the specification's table list stays
traceable.

---

## D-005 — MySQL-authoritative requirement vs shipped SQLite posture

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Proposed — needs owner sign-off** |
| Context | §2.2 requires MySQL authoritative in local integration testing, staging and production, with SQLite confined to isolated unit tests and explicitly *not* a production/development fallback. |

**Conflict.** The platform ships SQLite as a *supported* production posture, deliberately. The boot
validator (`Program.cs:110-134`) permits SQLite when the file is on a writable `/data` persistent
mount, or under `ALLOW_SQLITE_IN_PRODUCTION`, or under `PCIWORLD_ONLY` + `PCIWORLD_ALLOW_SQLITE`.
The in-code rationale at `Program.cs:88` is explicit: a previous fail-closed MySQL change "bricked
every existing SQLite-on-disk deploy". Flipping this globally would repeat that outage.

**Proposed resolution.** Scope the §2.2 requirement to the new increment rather than the platform:

1. New CCP tables and features require MySQL. The CCP feature flag refuses to enable on a
   non-MySQL provider, with a clear operator message.
2. Existing surfaces keep the current interim posture unchanged — no redeploy is bricked.
3. CCP integration tests run against real MySQL only; SQLite is used solely for isolated unit tests
   that cannot mask provider behaviour, per §2.2's own carve-out.
4. The platform-wide SQLite→MySQL cutover stays a separate, already-documented programme
   (`docs/MYSQL_MIGRATION.md`).

This satisfies §2.2's intent for everything this increment builds without an unrelated production
outage. **It changes the operating posture of a live deployment, so it needs the owner's decision.**

---

## D-006 — SignalR is net-new; multi-instance strategy undecided

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Blocked — needs a deployment-topology decision** |
| Context | Verified: zero SignalR/WebSocket/SSE anywhere in `backend/`; no SignalR package in `PCI.Backend.csproj`. The only in-house precedent is `secureexam/PCI.SecureExam.Server/Hubs/ProctorHub.cs`, a different solution. |

**Accepted now:** the hub route is `/api/world/hubs/community`, which already falls inside
`WorldOnly.Allowed()` — no allowlist change. MySQL + a durable outbox remain authoritative for
accepted messages, so reconnect recovers by sequence regardless of transport choice (§8.3).

**Blocked:** the scale-out choice — sticky sessions on a single instance, a Redis backplane with
documented outage semantics, or Azure SignalR. This depends on the production topology and budget,
which are not recorded in the repository. Until it is decided, Release 1 must either run
single-instance or accept documented reconnect behaviour under scale-out.

---

## D-007 — Legal prerequisites gate Phases 1–2

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Blocked — needs PCI legal counsel** |

§19.3 makes these hard prerequisites *before* open guest rooms and image sharing launch: minimum
age and supported jurisdictions; grooming/enticement escalation; specialist illegal-media detection
and reporting arrangements; emergency, preservation, reporting and legal-hold runbooks; reviewer
training and welfare protection.

None of these decisions exist in the repository. Engineering can build the pipeline and keep it
feature-flagged off, but §8.5 and the Phase 2 exit gate cannot be signed off without counsel. Image
sharing must not launch on a general classifier alone — §28.6 forbids claiming a general image
classifier detects every illegal image.

---

## D-008 — Moderation provider and benchmark corpus

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Blocked — needs a vendor decision and a corpus** |

§8.4 requires a provider-neutral interface with versioned PCI thresholds, benchmarked on
PCI-specific English, Arabic, Urdu, code-switched and obfuscated corpora, with **calibrated**
confidence bands — §8.4.1 warns a raw score from one provider does not equal another's.

Neither a selected provider nor any benchmark corpus exists in the repository. The adapter
interface and the policy matrix can be built as data and tested with synthetic fixtures, but the
confidence bands cannot be calibrated, and therefore the §8.4 decision matrix cannot be validated,
until a provider is contracted and a corpus is authored. A word list alone is never sufficient.

---

## D-009 — New World UI is React only; existing inline JS migrates incrementally

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |
| Context | ≈1,520 lines of inline vanilla JS across `WorldAdmin.cs` (554) and `WorldPages.cs` (258 + 657 + ~51). `frontend/src/` contains zero World code. |

**Selected:** all new authenticated CCP functionality is built as React + TypeScript apps; existing
inline-JS surfaces migrate incrementally, and legacy pages keep working until their React
replacements pass parity tests (§2.4). Per §28.15 the inline blocks are never expanded.

**Public SEO pages stay server-rendered.** §18.3 requires crawlable forum, job, employer and article
pages, and the existing server-rendered World pages already satisfy that. Only the authenticated
surfaces (Passport areas, employer portal, World Admin) become React.

---

## D-010 — Reuse the existing OAuth 2.1 + PKCE bridge for cross-product navigation

| | |
|---|---|
| Date | 2026-07-26 |
| Status | **Accepted** |

§17.3 and PW-US-002/003 require a premium cross-product switcher over the approved identity bridge.
`Endpoints/WorldOAuth.cs` already implements authorization-code + PKCE (S256 only, exact redirect
matching, server-enforced consent, 120-second single-use codes, replay mitigation that kills the
session a stolen code minted). Building a second bridge would add an authentication attack surface
for no benefit. Reuse it; do not rebuild. The narrow fixed scope string ("never your certifications,
exams, or billing") is exactly the §2.1 product-scoping guarantee and must not be widened casually.
