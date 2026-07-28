# CCP Phase 4 — PCI World Careers (design)

Scope: §10. A multi-employer jobs marketplace: verified employer tenants with organization-scoped
access, governed job publication, Passport-only applications, immutable consented application
snapshots, and crawlable `JobPosting` pages (§18.3). Built new per decision **D-001**; main-PCI's
single-tenant careers board stays in place, unmigrated, still serving `/careers/*` and its own
internal hiring. Reuse from it is at **design level only** — four named assets, no shared routes,
no shared rows.

Ships behind `pciworld_careers_enabled`, seeded `'0'`. Like Phase 3, nothing here is gated on
`CCP-P1-003` or `CCP-P1-004` — there are no images in this slice and no anonymous participation.
But the exit gate has one dependency engineering cannot close: **no employer can reach `verified`
until someone outside this repository defines what verification means** (§11). The code enforces
the gate; it cannot author the procedure behind it.

---

## 1. What makes this different from main-PCI careers

D-001's core finding, re-verified in the baseline (§2.4): these are different domains, not
duplicates. Main-PCI careers is PCI's own internal vacancy board — `organization_id` appears **zero
times** in either schema file, `job_postings.organisation` is a free-text display string, anyone may
apply anonymously by name and email, and every admin with `content` permission sees every
application globally (`Careers.cs:157`). None of that is wrong for an internal board. All of it is
disqualifying for a marketplace, and the differences are the design:

| | Main-PCI careers | PCI World Careers (Phase 4) |
|---|---|---|
| Tenancy | none — one implicit tenant (PCI) | **verified employer organizations**, every query org-scoped |
| Who publishes | a PCI admin | an employer member — **only after the org is verified** |
| Apply | anonymous, name + email | **Passport account, always** |
| Duplicate control | `UNIQUE(job_id, email)` | `UNIQUE(job_id, applicant_user_id)` (§13.6) |
| Who sees applicants | every `content` admin, globally | only members of the org the applicant consented to |
| Consent | implicit in the form | **explicit, versioned, per employer, withdrawable** |
| After submission | `status`/`admin_note` updated in place | frozen snapshot; **append-only events only** |
| Job edits | in-place `UPDATE` | versioned revisions (§28.11) |
| SEO | JSON-LD returned by an API endpoint | server-rendered crawlable pages + sitemap (§18.3, D-009) |

**Why this is not a §28.14 violation.** §28.14 forbids a second disconnected careers engine *without
a documented migration decision*. D-001 is that decision: ownership is disjoint, no record is shared
or copied, there is no dual write, and `/careers/*` behaviour is unchanged because World serves
`/world/careers/*` — which already falls inside `WorldOnly.Allowed()`, so no allowlist change.

**What is reused, and at what level** (D-001, verified file:line):

1. The `Careers.JobJsonLd` structured-data builder (`Careers.cs:484`) — as the *shape* of a new
   World builder, not a call into main-PCI code, which reads main-PCI settings and table columns.
2. The 9-type screening-question taxonomy (`Careers.cs:29`) — the types are proven; they carry over.
3. The frozen `answers_json` submit-time snapshot pattern (`Careers.cs:144`) — extended, see §5.2.
4. `{{placeholder}}` template rendering delivered through `Comms.Enqueue` (`Careers.cs:452-476`) —
   candidate notifications go through the same suppression-aware outbox, never a raw `Mailer` call.

---

## 2. What must never happen

These are not aspirations; each maps to a structural control below.

| Prohibition | Control |
|---|---|
| An unverified employer publishes a job | §3.3 — the publish transition and the public read path both predicate on `state='verified'`; there is no route that skips either |
| Anyone but the consented employer (or an audited World admin) reads a CV | §6 — private content-addressed storage, four-condition authorization on every download, per-view audit |
| A submitted application is modified in place (§28.11) | §5.2 — answers, CV reference and consent freeze at submission; everything afterwards is an append-only event |
| A closed or unpublished job is advertised to a crawler (§18.3) | §7 — `JobPosting` markup and sitemap membership are computed from live state, never cached past a close |
| Consent withdrawal destroys the audit trail | §5.3 — withdrawal is an append-only consent event that cuts access; it deletes nothing |
| An employer reads another employer's applicants | every employer-facing query carries the org predicate; asserted by tests, not convention |
| Automated candidate ranking or rejection (§10.7) | not built — there is no scoring code path to misconfigure |
| Production data deleted by a sweep while under hold | retention runs on the application window with the Phase 2 legal-hold check; a hold blocks every purge path |

---

## 3. Employer tenancy and verification

### 3.1 Organizations and org-scoped roles

An employer organization is a tenant, not staff, so it does **not** get `WorldRbac` roles. Its
members are ordinary Passport accounts (`WorldAccount.FromReq`) joined through a membership row with
an org role: `org_owner` (manage members, submit for verification), `recruiter` (draft jobs, review
applications), `viewer` (read only). The World Admin side extends `WorldRbac` with `careers.read`,
`careers.verify`, `careers.verify.approve` and `careers.moderate` action groups — the same
separation-of-duties shape as `community.sanction` / `community.sanction.approve`, and for the same
reason: the person who recommends verification must not be the person who approves it.

```
pciworld_employer_orgs(
  id, slug UNIQUE, name, legal_name, website_domain, locale,
  state,                -- draft|applied|under_review|verified|suspended|revoked
  verified_at, state_reason,
  created_by_user_id, created_at, updated_at, version)

pciworld_employer_members(
  id, org_id, world_user_id,
  role,                 -- org_owner|recruiter|viewer
  state,                -- active|removed
  invited_by_user_id, created_at, updated_at, version)
  -- UNIQUE(org_id, world_user_id): one membership row per person per org

pciworld_employer_verifications(          -- append-only
  id, org_id, kind,     -- evidence|note|decision
  evidence_ref, body,
  submitted_by_user_id, decided_by_admin_id, approved_by_admin_id,
  outcome, reason_code, created_at)
```

`version` is the §13.6 optimistic-concurrency column on every mutable row. Verification history is
append-only like every other decision trail in this increment: a revocation writes a new decision
row; it never rewrites the one that granted.

### 3.2 Why verify-before-publish, and why the forum's optimistic model is wrong here

Phase 3 lets a trusted author publish immediately and withdraws on an adverse verdict, because the
exposure window costs at most a bad post. That trade does not transfer. **A fake employer is not a
spammer; it is a data-harvesting attack.** The prize is a pile of CVs — names, employment histories,
contact details — collected from applicants who believed the employer was real, and once exfiltrated
that harm is irreversible. So the tempting alternatives are rejected explicitly:

- *Publish first, review after* — rejected. The window between publish and takedown is exactly the
  window in which applications (and therefore CVs) arrive. Withdrawal cannot un-disclose them.
- *Automated domain verification alone* — rejected as a decision-maker. Proving control of
  `acme-careers-jobs.example` proves control of a domain, not the existence of Acme. A lookalike
  domain passes every automated check. Domain control is recorded as *evidence toward* a human
  decision, and the admin UI says so — the same honesty rule as Phase 1's "layered deterrent, not
  ban enforcement" and Phase 2's "routes suspicion to humans; does not detect".

Verification is therefore a human decision, maker-checker enforced server-side
(`approved_by_admin_id ≠ decided_by_admin_id`), recorded with its evidence. What the procedure
actually demands — which documents, which registries, what staffing — is an operational decision
this repository cannot make (§11).

### 3.3 The structural control: draft anything, publish nothing

An unverified org can do everything *private*: create the org, invite members, draft jobs, preview
them. It cannot make anything public, and that is enforced twice, independently:

1. **At the transition.** The publish endpoint re-reads the org row inside the same transaction that
   flips the job to `published` and refuses unless `state='verified'`. Not a UI guard, not a cached
   flag — a predicate in the transaction.
2. **At the read.** Every public listing, job page, JSON-LD emission and sitemap entry joins through
   the org and predicates on `state='verified'`. Suspending an org therefore unpublishes its entire
   surface in one `UPDATE`, mid-session, without touching a single job row — and the same check on
   the application-review and CV-download paths cuts a suspended org's access to applicant data at
   the same instant. A fake employer discovered late loses the data access, not just the listing.

---

## 4. Jobs: versioned, and closed honestly

A job is a container with a state; its text lives in append-only revisions — the same §28.11
discipline as forum posts, and for the recruitment-specific reason on top of the general one: an
applicant answered *a particular text*. If the employer can quietly rewrite the ad after
applications arrive ("we never promised remote"), the application record stops meaning anything.
Every application therefore pins the `job_revision_id` it was submitted against, and screening
questions belong to the revision, so the pair (what was asked, what was answered) is stable forever.

```
pciworld_jobs(
  id, org_id, slug, current_revision_id,
  state,                -- draft|published|closed|archived
  published_at, closes_at, closed_at,
  created_by_user_id, created_at, updated_at, version)
  -- UNIQUE(org_id, slug): unique per org, not globally — two employers may both post "project-controls-lead"

pciworld_job_revisions(                   -- append-only
  id, job_id, revision_no, title, description, description_rendered,
  location_city, location_country, remote_type, employment_type,
  salary_min, salary_max, salary_currency, salary_period, salary_visible,
  edited_by_user_id, created_at)
  -- UNIQUE(job_id, revision_no): two concurrent edits cannot both be revision 4

pciworld_job_questions(
  id, job_revision_id, qtype, label, options, required, sort)
  -- qtype: the proven 9-type taxonomy (short_text|long_text|yesno|single|multi|number|date|dropdown|consent)
```

`description_rendered` is sanitised at write time via `HtmlSanitize`, exactly as
`pciworld_forum_post_revisions.body_rendered` is — the public read path never renders unsanitised
employer input, and a sanitiser change cannot retroactively expose an old revision. Datetimes are
`VARCHAR(32)` per the increment-wide rule (a `TEXT` datetime becomes `LONGTEXT` on MySQL and blows
the InnoDB index-key limit; several of these columns are indexed). The installer is an idempotent
`Ensure(db)` in `Data/WorldCareersSchema.cs`, called from `WorldSchema.Ensure`, mirroring
`ForumSchema` — and installing the schema is not launching the feature; the flag seeds `'0'`.

State machine: `draft → published → closed → archived`. `closes_at` closes the job automatically —
enforced on the read path (a job past `closes_at` is treated as closed even before a sweep stamps
it), because the honesty rule of §7 cannot depend on a background job having run.

---

## 5. Passport-only apply and the immutable snapshot

### 5.1 Why anonymous apply is refused here

Main-PCI careers accepts anonymous applications, deliberately, and keeps doing so. Here the same
choice would be wrong, for three reasons that are about integrity, not friction:

1. **Consent must be attributable and withdrawable.** The application discloses personal data to a
   third-party tenant, not to PCI. An anonymous applicant has no authenticated channel through which
   to prove, later, that they are the person who applied — which makes withdrawal unverifiable and
   the consent record legally hollow. Main-PCI's anonymous apply discloses only to PCI itself, which
   is why the same design is acceptable there.
2. **The snapshot must bind to an identity.** An immutable record of "who applied with what" is
   worth nothing if "who" is a free-text string anyone can type.
3. **Abuse control has an anchor.** `UNIQUE(job_id, applicant_user_id)` and per-account budgets are
   enforceable; per-email uniqueness is defeated by plus-addressing in seconds.

The cost is stated honestly: the marketplace forgoes drive-by applications, and an employer's
listing will convert fewer visitors than one with an anonymous form. That is the price of consented,
withdrawable, attributable applications, and §10 chooses to pay it.

### 5.2 The snapshot — what freezes, and when

Submission is one transaction that writes the application row with everything the employer will ever
see, frozen:

- **the answers** — `answers_json` extends the proven `Careers.cs:144` pattern: per question, the
  question *text*, type and answer value are captured, so even the deletion of a question row later
  cannot change what the record says was asked;
- **the CV reference and its `cv_sha256`** — the content-addressed hash pins the exact bytes; the
  applicant later replacing the file in their profile changes nothing the employer sees;
- **the consent** — a `granted` consent event naming the org, the job, the terms version, and a
  `snapshot_sha256` over the frozen material, binding the consent to exactly what was disclosed;
- **the `job_revision_id`** — what the job said when they applied (§4).

```
pciworld_job_applications(
  id, job_id, job_revision_id, applicant_user_id,
  public_reference UNIQUE,  -- random, non-sequential; safe to quote in support mail (Phase 1 §2.4 discipline)
  state,                    -- submitted|in_review|shortlisted|interview|offer|hired|declined|withdrawn
  answers_json,             -- FROZEN at submit
  cv_ref, cv_sha256, cv_name,  -- FROZEN at submit
  submitted_at, created_at, updated_at, version)
  -- UNIQUE(job_id, applicant_user_id): idempotent double-click, one application per person per job (§13.6)

pciworld_application_events(              -- append-only
  id, application_id, kind,               -- status|note|message
  from_state, to_state, body,
  actor_kind, actor_ref, created_at)

pciworld_application_consents(            -- append-only
  id, application_id, kind,               -- granted|withdrawn
  terms_version, snapshot_sha256, created_at)
```

**After commit, no code path updates the frozen columns.** §28 forbids modifying a submitted
application in place — the same rule §28.11 states for published content, applied to the record an
appeal or dispute would turn on. The only mutable things on the row are `state` and `version`, and
every state change writes an `pciworld_application_events` row naming its actor — the discipline
main-PCI's `job_app_events` already follows ("no silent status changes"), made total: main-PCI still
overwrites `admin_note` in place (`Careers.cs:377`); here a note is an event, never a column.
Candidate-facing notifications on state changes render admin-managed templates and go through
`Comms.Enqueue` (reuse asset 4), best-effort, never breaking the recruiting action.

### 5.3 Consent and withdrawal — access ends, history survives

What the applicant consents to is specific and named: *this snapshot, disclosed to this employer
organization, for this job, under this terms version*. Not "PCI World Careers" in general — the
consent row carries the org id, and applying to two employers writes two independent consents.

Withdrawal is an append-only `withdrawn` consent event plus a `withdrawn` application state. From
that instant the employer's access to the snapshot and the CV is refused — the four-condition check
in §6 fails on condition (c) — and the applicant's timeline says so. What withdrawal does **not** do
is delete: the events, the consent history, and the fact that an application existed survive for the
retention window, because "you looked at my CV before I withdrew" is precisely the dispute the audit
trail exists to answer, and an employer accused of misusing data must be answerable from records,
not from memory. After the retention window the snapshot content and the CV bytes are purged; the
purge consults the Phase 2 legal-hold discipline and skips held rows, asserted by a test.

Withdrawal is not erasure. Data-subject erasure is a separate, stronger flow (main-PCI's
`Erasure.Anonymise` is the platform precedent; a World-side equivalent covering these tables is part
of this phase's build), and the retention periods it interacts with are a decision owned outside
engineering (§11).

---

## 6. CVs — the document discipline, and never a public URL

Intake reuses `DocStore`'s discipline wholesale, with a narrower allowlist: **PDF, DOC, DOCX only**.
No images (main-PCI's CV path, built on `Storage.DecodeDataUri`, accepts JPEG/PNG/WebP; a
marketplace reviewing at volume should not normalise photographed documents), no HTML, no SVG, no
archives. Each upload passes the magic-byte signature check (a renamed `.exe` fails), the size cap,
and `UploadScan.Scan` — the single fail-closed malware policy every upload door already shares
(RES-021): a configured-but-unreachable scanner **rejects**, never passes.

Bytes land content-addressed in the **private** object store under a dedicated `world-cv` category.
Two deliberate differences from main-PCI's `cv` category:

- main-PCI protects `cv` from the retention sweep forever (`Storage.cs` protected-category list);
  World CVs instead live under the application's governed retention window (§5.3) — a marketplace
  holding every CV indefinitely is a liability, not an archive;
- there is **no public URL, ever**. The reference resolves only through one authenticated endpoint
  that checks, on every request: (a) the requester is an active member of the org, (b) the
  application belongs to one of that org's jobs, (c) consent is not withdrawn, (d) the org is
  `verified` and not suspended. Every download — employer or World admin — writes a per-view audit
  row, the same per-view logging main-PCI already does for CV views (`Careers.cs:438`) and Phase 2
  does for evidence. The file is served `Content-Disposition: attachment` with the stored MIME and a
  `DocStore.SafeName`-derived filename — never inline, because an inline PDF in a browser viewer is
  an execution surface this feature does not need.

What is *not* pretended: the CV's contents are whatever the applicant wrote, contact details
included. Consent plus audit is the control; redaction of a free-form document is not promised,
because it cannot be delivered honestly.

---

## 7. Crawlable job pages and the honesty rule (§18.3)

Public reading is server-rendered, per D-009: `/world/careers` (list and facets),
`/world/careers/{org}/{slug}` (job detail), `/world/employers/{slug}` (employer profile). The
authenticated surfaces — employer portal, applicant "my applications", World Admin verification
queue — are React. All routes fall inside `WorldOnly.Allowed()` today; no allowlist change.

Job detail pages embed `JobPosting` JSON-LD built by a World-side builder following the
`Careers.JobJsonLd` shape: `identifier`, `hiringOrganization` (the **verified org**, with its
verified `website_domain` as `sameAs` — never a free-text string), `employmentType`, `jobLocation` /
`TELECOMMUTE`, `baseSalary` only when the employer chose `salary_visible`, `validThrough` from
`closes_at`, `directApply: true`. Employer pages emit `Organization` markup only once verified.

**The honesty rule: never advertise a job that is closed or unpublished.** Structured data is a
claim to a search engine, and a stale claim is a lie that outlives the page:

- `JobPosting` markup is emitted only while the job is `published`, not past `closes_at`, and its
  org is `verified` — computed at render time, not cached past a close (§4's read-path rule).
- A **closed** job's URL stays reachable (an applicant's saved link should explain itself, not 404)
  but renders "no longer accepting applications", carries **no** `JobPosting` block, is `noindex`,
  and leaves the sitemap. The apply endpoint refuses independently — the page saying "closed" is
  courtesy; the server enforcing it is the control.
- A **draft** job, or any job of a non-verified org, is a public **404** — the existence of a
  tenant's draft is tenant-confidential.
- `world-sitemap.xml` (already allowlisted; `WorldSeo.Sitemap` is the seam) lists published, open
  jobs of verified orgs, and nothing else.

---

## 8. Rate limiting and abuse

Per account and per org, not per IP — the same shared-office argument as Phase 3 §8, and the
`ForumTrust.HourlyPostBudget` → `429 rate_limited` pattern is the template. The two threat classes
this surface adds:

**Fake employers.** The verification gate (§3) is the primary control; everything else assumes it
can be probed. Pre-verification, an org can read nothing but its own drafts — there is no applicant
data to steal because there are no applicants. Verification submissions themselves are budgeted per
account so the review queue cannot be flooded. Suspension is checked per request, not per login, so
a discovered fake loses data access mid-session (§3.3). Applicants can report a job; a report opens
a case in the same shared case machinery Phases 1–3 use — there is no second moderation console.

**Scraping applicant data.** Applicant data is never public and never crosses a tenant boundary, so
the scraping surface is a compromised or malicious *employer member*. Controls: the four-condition
per-request authorization on every CV download (§6); per-view audit rows that make bulk access
visible ("one account pulled 400 CVs in an hour" is a query, not a forensic project); download
budgets per member per hour that a legitimate reviewer never hits; and `public_reference` values
that are random, so application IDs cannot be enumerated even by an authorized-but-curious client —
though authorization, not obscurity, remains the control on every path.

Apply-side: `UNIQUE(job_id, applicant_user_id)` makes duplicates a schema impossibility, a
per-account daily application budget bounds spam-applying, and the request body caps stay inside
the platform's 6 MB Kestrel limit. Employer-side: a per-org posting budget bounds listing spam by a
verified-then-rogue tenant.

---

## 9. Feature flag

```
pciworld_careers_enabled   -- site setting, INSERT OR IGNORE seeded '0'
```

Every write path checks it; with the flag off, `/world/careers*` returns the standard World 404,
apply and employer endpoints refuse, no `JobPosting` markup or sitemap entries are emitted, and
existing rows are untouched — the same kill-switch semantics as `pciworld_forum_enabled`. Turning it
on is a human decision made after §11's owners have signed off; the flag does not assert that they
have.

---

## 10. Notifications

Status changes, interview messages and receipt acknowledgements render admin-managed
`{{placeholder}}` templates and enqueue through `Comms.Enqueue` (design-reuse asset 4), category
`operational`, honouring the outbox's suppression and consent handling. Employer free text reaches
an applicant only inside a template through this path — never as a raw email — which keeps the
one-way, records-first shape of main-PCI's candidate messaging and closes the door on the
free-form-messaging problem deferred in §13.

---

## 11. Blocked on decisions outside engineering

Named owners, per the D-007/D-008 discipline: the code ships flag-off and complete; these gate the
*launch*, and pretending otherwise would violate §28.20. Proposed for `CCP_ISSUE_REGISTER.md` as
CCP-P4-013 … CCP-P4-016 (next free numbers in the register's global sequence).

| # | Decision needed | Owner |
|---|---|---|
| CCP-P4-013 | The employer verification standard: what evidence proves an employer is real (registry lookups, domain control, documents), who reviews it, to what SLA, and the revocation triggers. Until this exists, no org can honestly reach `verified` and the flag stays off. | Trust & Safety lead + Operations |
| CCP-P4-014 | Employer terms of service, the applicant-facing privacy notice and consent wording, and the data-protection role split (PCI vs employer as controller/processor for snapshot data). The consent event stores a `terms_version`; someone must author version 1. | PCI legal counsel + Data Protection Officer |
| CCP-P4-015 | Retention periods for applications and CVs per supported jurisdiction, and the erasure-request procedure's interaction with withdrawn-but-retained records (§5.3). | Data Protection Officer + legal counsel |
| CCP-P4-016 | The commercial model — free listings, paid listings, or tiers. Does not block this build (nothing here touches Stripe), but blocks any payment wiring and shapes the verification queue's expected volume. | Commercial owner |

---

## 12. Test plan — what must fail first

Written before implementation; no test may weaken a threshold, an authorization check or a
validation rule to pass (§28.3).

**Unit (no DB)** — org and job state machines, including every refused transition; the honesty
predicate (published × not past `closes_at` × org verified) for markup, sitemap and apply, row by
row; snapshot serialization captures question text, not question id references; `snapshot_sha256`
stability; CV allowlist and sniff rejections; `SafeName` output; JSON-LD builder omits salary when
not visible and always carries `validThrough` when `closes_at` is set.

**Repository (real MySQL as well as SQLite)** — idempotent re-`Ensure`; `UNIQUE(org_id, slug)`,
`UNIQUE(job_id, revision_no)`, `UNIQUE(job_id, applicant_user_id)`, `UNIQUE(org_id, world_user_id)`
all enforced under concurrency; optimistic-concurrency (`version`) conflict on stale org and job
writes; append-only tables have no UPDATE path.

**Integration / abuse (live HTTP)** — the phase's whole point:

- an **unverified** org cannot publish by any route: the transition endpoint, a direct state PATCH,
  a re-publish of a previously published job after revocation — every path refuses;
- **suspending an org** removes its jobs from the list, the job pages' markup, and the sitemap, and
  refuses a CV download in the same session that succeeded a moment before;
- **anonymous apply** is refused with 401 — with no session, a forged body, and a guest community
  session (a guest ticket is not a Passport);
- after submission, **no endpoint mutates** `answers_json`, `cv_ref` or the consent — and the
  employer's view of the snapshot is byte-identical before and after the applicant edits their
  profile and replaces their stored CV;
- employer A's member, including its `org_owner`, **cannot read** employer B's job drafts,
  applications or CVs by id, by reference, or by list;
- a **withdrawn** application refuses the employer's CV download and snapshot view from that instant,
  while the event and consent history remain readable to the applicant and to an audited admin;
- a **closed** job serves a page with no `JobPosting` block, is absent from the sitemap, and refuses
  an apply POST regardless of what the page said;
- CV download without org membership, or by a removed member, is refused **and audited**; the
  per-member download budget returns 429 before a bulk pull completes;
- retention purge with a legal hold in effect deletes nothing.

**Accessibility** — the careers list, job page and apply flow scanned with axe in a real browser,
including reflow at 320px, as Phases 1–3 established; the apply form's validation errors are
announced and not conveyed by colour alone.

---

## 13. What this phase does not do

- **No migration of main-PCI careers.** D-001 is explicit: `/careers/*`, its tables and its admin
  screens are untouched and out of scope. No dual write, no backfill, no reconciliation job.
- **No external ATS integrations.** §10.8 — Phase 5, contracts only until sandbox credentials exist;
  §28 forbids marking an integration complete on mocks.
- **No automated ranking, screening or rejection of candidates.** §10.7 prohibits it in Release 1.
  There is no match score, no auto-shortlist, no model-ordered queue — deliberately no code path,
  not a disabled one.
- **No external apply URLs.** A listing that bounces applicants to an employer's own site defeats
  the consent, snapshot and audit model this phase exists to provide, and is the classic phishing
  vector on job boards. In-platform apply only; revisit only with its own safety design.
- **No employer↔applicant free-form messaging.** Templated, records-first notifications only (§10) —
  a two-way channel is a distinct moderation problem, the same argument as Phase 3's no-DMs.
- **No paid or promoted listings.** Blocked on CCP-P4-016; nothing here touches Stripe.
- **No CV parsing or enrichment.** The employer reviews the document the applicant consented to
  disclose — not a machine's paraphrase of it.
