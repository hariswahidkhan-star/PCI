# CCP Phase 4 — PCI World Careers (design)

Scope: §10. A multi-employer jobs marketplace: verified employer tenants with organization-scoped
access, Passport-only applications, immutable consented application snapshots, and crawlable
`JobPosting` pages (§18.3). Built new per decision **D-001**; main-PCI's single-tenant careers board
stays in place, unmigrated, still serving `/careers/*` and PCI's own internal hiring. Reuse from it
is at **design level only** — four named assets, no shared routes, no shared rows.

Ships behind `pciworld_careers_enabled`, seeded `'0'`. Like Phase 3, nothing here is gated on
`CCP-P1-003` or `CCP-P1-004` — there are no images in this slice and no anonymous participation.
But the exit gate has one dependency engineering cannot close: **no employer can honestly reach
`verified` until someone outside this repository defines what verification means** (§11). The code
enforces the gate; it cannot author the procedure behind it.

The schema lands in `Data/CareersSchema.cs`, an idempotent `Ensure(db)` called from
`WorldSchema.Ensure` exactly like the community, media and forum installers before it — and, exactly
like them, **installing the schema is not launching the feature**.

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
| Tenancy | none — one implicit tenant (PCI) | **verified employer rows**, every employer-side query scoped through one |
| Who publishes | a PCI admin | an employer member — **only after the employer is verified** |
| Apply | anonymous, name + email | **Passport account, always** |
| Duplicate control | `UNIQUE(job_id, email)` | `UNIQUE(posting_id, applicant_user_id)` (§13.6) |
| Who sees applicants | every `content` admin, globally | only members of the employer the applicant consented to |
| Consent | implicit in the form | **explicit, purpose- and policy-versioned, per employer, withdrawable** |
| Answers | one frozen `answers_json` blob | frozen **per-answer rows** carrying their own prompt snapshot |
| SEO | JSON-LD returned by an API endpoint | server-rendered crawlable pages + sitemap (§18.3, D-009) |

**Why this is not a §28.14 violation.** §28.14 forbids a second disconnected careers engine *without
a documented migration decision*. D-001 is that decision: ownership is disjoint, no record is shared
or copied, there is no dual write, and `/careers/*` behaviour is unchanged because World serves
`/world/careers/*` — which already falls inside `WorldOnly.Allowed()`, so no allowlist change.

**What is reused, and at what level** (D-001, verified file:line):

1. The `Careers.JobJsonLd` structured-data builder (`Careers.cs:484`) — as the *shape* of a new
   World builder, not a call into main-PCI code, which reads main-PCI settings and table columns.
2. The screening-question taxonomy (`Careers.cs:29`) — as the proven ceiling. Release 1 ships three
   kinds (`text|boolean|choice`); each further kind is validation surface added when a real employer
   needs it, and the legacy `consent` question kind is deliberately **not** carried over, because
   here consent is a first-class record (§5.3), not a form field an employer words themselves.
3. The frozen submit-time snapshot pattern (`Careers.cs:144`) — strengthened from one JSON blob to
   per-answer rows, see §5.2.
4. `{{placeholder}}` template rendering delivered through `Comms.Enqueue` (`Careers.cs:452-476`) —
   candidate notifications go through the same suppression-aware outbox, never a raw `Mailer` call.

---

## 2. What must never happen

These are not aspirations; each maps to a structural control below.

| Prohibition | Control |
|---|---|
| An unverified employer publishes a job | §4.3 — the publish transition and the public read path both predicate on `state='verified'`; no route skips either |
| Anyone but the consented employer (or an audited World admin) reads a CV | §6 — private content-addressed storage, four-condition authorization on every download, per-view audit |
| A submitted application is modified in place (§28) | §5.2 — answers, CV reference and consent freeze at submission; answer rows are self-contained snapshots; everything afterwards is a recorded state change |
| A closed or unpublished job is advertised to a crawler (§18.3) | §7 — `JobPosting` markup and sitemap membership are computed from live state at render time, never cached past a close |
| Consent withdrawal destroys the record that consent existed | §5.3 — withdrawal is a timestamp on the consent row, never its deletion |
| An employer reads another employer's applicants | every employer-facing query carries the `employer_id` predicate; asserted by tests, not convention |
| Automated candidate ranking or rejection (§10.7) | not built — there is no scoring code path to misconfigure |
| A double-submit gives an employer two versions of one candidacy | `UNIQUE(posting_id, applicant_user_id)` — a schema impossibility, not a debounce |

---

## 3. Feature flag

```
pciworld_careers_enabled   -- site setting, INSERT OR IGNORE seeded '0'
```

Every write path checks it; with the flag off, `/world/careers*` returns the standard World 404,
apply and employer endpoints refuse, no `JobPosting` markup or sitemap entries are emitted, and
existing rows are untouched — the same kill-switch semantics as `pciworld_forum_enabled`. Turning it
on is a human decision made after §11's owners have signed off; the flag does not assert that they
have.

---

## 4. Employer tenancy and verification

### 4.1 Employers and acting rights

An employer is a tenant, not staff, so its people do **not** get `WorldRbac` roles. Members are
ordinary Passport accounts (`WorldAccount.FromReq`) joined through a membership row — acting rights
are rows, not a password shared around an office:

```
pciworld_employers(
  id, slug UNIQUE, name, website,
  state,                -- draft|pending_verification|verified|suspended
  verified_at, verified_by_admin_id,
  created_at, updated_at, version)

pciworld_employer_members(
  id, employer_id, user_id,
  role,                 -- owner|member
  created_at)
  -- UNIQUE(employer_id, user_id): one membership row per person per employer.
  -- A role change is an UPDATE; a second row would give "may this person act
  -- for this employer, and as what" two answers.
```

The employer slug is **globally** unique because the employer page is a crawlable URL — a duplicate
slug is a permanently ambiguous address, the same reasoning as forum categories. `version` is the
§13.6 optimistic-concurrency column on every mutable row in this phase. The World Admin side
extends `WorldRbac` with `careers.read`, `careers.verify` and `careers.moderate` action groups,
following the community precedent that a person who moderates a forum has no business verifying an
employer and vice versa.

### 4.2 Verification is an attributable act, not a flag

`verified` is an assertion **PCI makes to candidates** about who is behind a posting. So the row
records *who* asserted it and *when* (`verified_by_admin_id`, `verified_at`) — a bare boolean would
be an unattributable claim that nobody could later explain to a candidate who relied on it.
Suspension and re-verification are audited state changes (`pciworld_audit`, extended per the
baseline §13.5 note), so the assertion's history survives every edit.

Whether verification additionally requires a second approver (the `community.sanction.approve`
maker-checker shape) is deliberately **not** decided here: it belongs to the verification procedure
itself — evidence standard, staffing, SLA — which is owned outside engineering (§11, CCP-P4-013).
The schema loses nothing by waiting: an approval column is a guarded `AddCol` the day the procedure
demands one.

### 4.3 Why verify-before-publish, and why Phase 3's optimistic model is wrong here

Phase 3 lets a trusted author publish immediately and withdraws on an adverse verdict, because the
exposure window costs at most a bad post. That trade does not transfer. **A fake employer is not a
spammer; it is a data-harvesting attack.** The prize is a pile of CVs — names, employment histories,
contact details — collected from applicants who believed the employer was real, and once
exfiltrated that harm is irreversible. The tempting alternatives are therefore rejected explicitly:

- *Publish first, review after* — rejected. The window between publish and takedown is exactly the
  window in which applications, and therefore CVs, arrive. Withdrawal cannot un-disclose them.
- *Automated domain verification alone* — rejected as a decision-maker. Proving control of
  `acme-controls-jobs.example` proves control of a domain, not the existence of Acme; a lookalike
  domain passes every automated check. Domain control is *evidence toward* a human decision, and the
  admin UI says so — the same honesty rule as Phase 1's "layered deterrent, not ban enforcement" and
  Phase 2's "routes suspicion to humans; does not detect".

The structural control is **draft anything, publish nothing**. An unverified employer can create its
row, invite members and draft postings — everything private. It cannot make anything public, and
that is enforced twice, independently:

1. **At the transition.** The publish endpoint re-reads the employer row inside the same transaction
   that flips the posting to `published` and refuses unless `state='verified'`. A predicate in the
   transaction, not a UI guard.
2. **At the read.** Every public listing, job page, JSON-LD emission and sitemap entry joins through
   the employer and predicates on `state='verified'`. Suspending an employer therefore unpublishes
   its entire surface in one `UPDATE`, mid-session, without touching a posting row — and the same
   check on the application-review and CV-download paths cuts a suspended employer's access to
   applicant data at the same instant. A fake employer discovered late loses the *data*, not just
   the listing.

---

## 5. Postings, Passport-only apply, and the frozen application

### 5.1 Why anonymous apply is refused here

Main-PCI careers accepts anonymous applications, deliberately, and keeps doing so. Here the same
choice would be wrong, for three reasons that are about integrity, not friction:

1. **Consent must be attributable and withdrawable.** The application discloses personal data to a
   third-party tenant, not to PCI. An anonymous applicant has no authenticated channel through which
   to later prove they are the person who applied — which makes withdrawal unverifiable and the
   consent record legally hollow. Main-PCI's anonymous apply discloses only to PCI itself, which is
   why the same design is acceptable there.
2. **The snapshot must bind to an identity.** An immutable record of "who applied with what" is
   worth nothing if "who" is a free-text string anyone can type.
3. **Abuse control has an anchor.** `UNIQUE(posting_id, applicant_user_id)` and per-account budgets
   are enforceable; per-email uniqueness is defeated by plus-addressing in seconds.

The cost is stated honestly: the marketplace forgoes drive-by applications, and a listing here will
convert fewer visitors than one with an anonymous form. That is the price of consented,
withdrawable, attributable applications, and §10 chooses to pay it.

### 5.2 The posting is a working copy; the application is the record

```
pciworld_job_postings(
  id, employer_id, slug, title, description, location, employment_type,
  salary_min_minor, salary_max_minor, currency,     -- INTEGER minor units, never floating point
  state,                -- draft|published|closed|withdrawn
  published_at, closes_at,
  created_at, updated_at, version)
  -- UNIQUE(employer_id, slug): unique per employer, not globally — two employers
  -- may each legitimately post "senior-planner"; within one employer a duplicate
  -- slug is a permanently ambiguous URL.

pciworld_job_questions(
  id, posting_id, sort, kind,   -- text|boolean|choice
  prompt, required, created_at)

pciworld_applications(
  id, posting_id, applicant_user_id,
  state,                -- draft|submitted|withdrawn|shortlisted|rejected
  cv_ref, cv_sha256, cv_name,   -- FROZEN at submission (§6)
  submitted_at, withdrawn_at,
  created_at, updated_at, version)
  -- UNIQUE(posting_id, applicant_user_id) — deliberately with NO partial WHERE
  -- clause: Db.Translate strips predicates from partial indexes on MySQL, so a
  -- partial index would leave the two providers agreeing only by accident.

pciworld_application_answers(   -- append-only; the §28 record
  id, application_id, question_id,        -- question_id is provenance, never the text's source of truth
  prompt_snapshot, answer_snapshot, created_at)
```

Salary is integer minor units with its currency beside it — a `REAL` salary rounds, and a rounded
salary is a wrong salary shown to a person deciding whether to apply; a number without its currency
is not an amount at all. Datetimes are `VARCHAR(32)` per the increment-wide rule (a `TEXT` datetime
becomes `LONGTEXT` on MySQL and an index touching it blows the InnoDB key limit; the public-listing
recency index touches these).

**Where the immutability boundary sits is the central design choice of this phase**, and it is
different from the forum's. The forum versions its *published content* because the post **is** the
record. Here the posting and its questions are the employer's **working copy** — freely editable,
guarded by `version` and audited, but not versioned — and the *application* is the record. §28
forbids modifying a submitted application in place, so at submission, in one transaction, the
application freezes everything a dispute could turn on:

- **the answers** — one row per answer, each carrying `prompt_snapshot` *copied from the question at
  that moment*. Editing the question later cannot change what this applicant appears to have been
  asked; deleting it cannot orphan what they said. The answer row is self-contained evidence, which
  is stronger than the legacy single-blob `answers_json` it descends from: `question_id` stays as
  provenance only, because the moment a join back to `pciworld_job_questions` becomes the source of
  truth, in-place mutability has been reintroduced through the back door.
- **the CV reference and its `cv_sha256`** — the content-addressed hash pins the exact bytes; the
  applicant replacing their stored CV afterwards changes nothing the employer sees (§6).
- **the consent** — §5.3.

A *full posting revision history* (the forum's model applied to jobs) was considered and deferred:
it would be a second versioning engine protecting text the honesty rules of §7 already govern, and
the record a hiring decision must be justified by — what the applicant said, to which prompts, with
what consent — is already immutable without it. Deferred, not dropped (§13); a bait-and-switch edit
to a live posting is visible in `pciworld_audit` meanwhile.

**Withdrawal holds the slot.** The unique index has no `WHERE state!='withdrawn'` escape, so
withdrawing and reapplying is a state change on the row the person already owns, never a second row
— an employer can never hold two versions of one candidacy. Every state change after submission
(`submitted → shortlisted|rejected`, withdrawal, reinstatement) is written as a recorded transition
with its actor — the "no silent status changes" discipline main-PCI's `job_app_events` already
follows, kept total here: notes are events, never overwritten columns (main-PCI still overwrites
`admin_note` in place, `Careers.cs:377`). Candidate-facing notifications on state changes render
admin-managed templates through `Comms.Enqueue` (reuse asset 4) — best-effort, never breaking the
recruiting action, and the only channel by which employer words reach an applicant's inbox.

### 5.3 Consent: purpose-scoped, policy-versioned, withdrawable without amnesia

```
pciworld_application_consents(
  id, application_id, employer_id, purpose,
  granted_at, withdrawn_at, policy_version, created_at)
```

What the applicant consents to is specific and named: *this application's contents, disclosed to
this employer, for this purpose, under this policy text* — `policy_version` pins which words they
actually saw, because consent to last year's policy is not consent to this year's. Applying to two
employers writes two independent consents; there is no marketplace-wide blanket.

Withdrawal is a **timestamp, not a deletion**. From the instant `withdrawn_at` is stamped, the
employer's access to the application and CV is refused — condition (c) in §6 fails — and the
applicant's own view says so. But the row survives, because *"was there consent on the day the
employer read this?"* must stay answerable after the consent is withdrawn: that question is
precisely the dispute the record exists to settle, and an employer accused of misusing data must be
answerable from records, not memory. After the retention window the snapshot content and CV bytes
are purged; the purge consults the Phase 2 legal-hold discipline and skips held rows, asserted by a
test.

Withdrawal is not erasure. Data-subject erasure is a separate, stronger flow (main-PCI's
`Erasure.Anonymise` is the platform precedent; its World-side coverage of these tables is part of
this phase's build), and the retention periods it interacts with are a decision owned outside
engineering (§11, CCP-P4-015).

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
  that checks, on every request: (a) the requester is a member of the employer, (b) the application
  belongs to one of that employer's postings, (c) consent is granted and not withdrawn, (d) the
  employer is `verified` and not suspended. Every download — employer or World admin — writes a
  per-view audit row, the same per-view logging main-PCI already does for CV views (`Careers.cs:438`)
  and Phase 2 does for evidence access. The file is served `Content-Disposition: attachment` with
  the stored MIME and a `DocStore.SafeName`-derived filename — never inline, because an inline PDF
  in a browser viewer is an execution surface this feature does not need.

What is *not* pretended: the CV's contents are whatever the applicant wrote, contact details
included. Consent plus audit is the control; redaction of a free-form document is not promised,
because it cannot be delivered honestly.

---

## 7. Crawlable job pages and the honesty rule (§18.3)

Public reading is server-rendered, per D-009: `/world/careers` (list), `/world/careers/{emp}/{slug}`
(job detail), `/world/employers/{slug}` (employer profile). The authenticated surfaces — employer
portal, applicant "my applications", World Admin verification queue — are React. All routes fall
inside `WorldOnly.Allowed()` today; no allowlist change.

Job pages embed `JobPosting` JSON-LD built by a World-side builder following the `Careers.JobJsonLd`
shape: `identifier`, `hiringOrganization` (the **verified employer**, with its recorded website as
`sameAs` — never a free-text string), `employmentType`, `baseSalary` from the minor-unit columns
when the employer chose to publish salary, `validThrough` from `closes_at`, `directApply: true`.
Employer pages emit `Organization` markup only once verified.

**The honesty rule: never advertise a job that is closed or unpublished.** Structured data is a
claim to a search engine, and a stale claim is a lie that outlives the page:

- `JobPosting` markup is emitted only while the posting is `published`, not past `closes_at`, and
  its employer is `verified` — computed at render time, so honesty cannot depend on a background
  sweep having stamped `closed` yet.
- A **closed** posting's URL stays reachable (an applicant's saved link should explain itself, not
  404) but renders "no longer accepting applications", carries **no** `JobPosting` block, is
  `noindex`, and leaves the sitemap. The apply endpoint refuses independently — the page saying
  "closed" is courtesy; the server enforcing it is the control.
- A **draft** or **withdrawn** posting, or any posting of a non-verified employer, is a public
  **404** — the existence of a tenant's draft is tenant-confidential.
- `world-sitemap.xml` (already allowlisted; `WorldSeo.Sitemap` is the seam) lists published, open
  postings of verified employers, and nothing else.
- **No external apply URLs.** A listing that bounces applicants to an employer's own site defeats
  the consent, snapshot and audit model this phase exists to provide, and is the classic phishing
  vector on job boards. In-platform apply only.

---

## 8. Rate limiting and abuse

Per account and per employer, not per IP — the same shared-office argument as Phase 3 §8, and the
`ForumTrust.HourlyPostBudget` → `429 rate_limited` pattern is the template. The two threat classes
this surface adds:

**Fake employers.** The verification gate (§4) is the primary control; everything else assumes it
will be probed. Pre-verification, an employer can read nothing but its own drafts — there is no
applicant data to steal because there are no applicants. Verification submissions are budgeted per
account so the review queue cannot be flooded. Suspension is checked per request, not per login, so
a discovered fake loses data access mid-session (§4.3). Applicants can report a posting; a report
opens a case in the same shared case machinery Phases 1–3 use — there is no second moderation
console.

**Scraping applicant data.** Applicant data is never public and never crosses a tenant boundary, so
the scraping surface is a compromised or malicious *employer member*. Controls: the four-condition
per-request authorization on every CV download (§6); per-view audit rows that make bulk access
visible ("one account pulled 400 CVs in an hour" is a query, not a forensic project); download
budgets per member per hour that a legitimate reviewer never hits; and authorization on every id on
every path, so nothing rests on references being unguessable.

Apply-side: `UNIQUE(posting_id, applicant_user_id)` makes duplicates a schema impossibility, a
per-account daily application budget bounds spam-applying, and CV sizes stay inside the platform's
6 MB Kestrel body cap. Employer-side: a per-employer posting budget bounds listing spam by a
verified-then-rogue tenant, and `withdrawn` exists as a posting state precisely so a takedown does
not have to be a deletion.

---

## 9. Test plan — what must fail first

Written before implementation; no test may weaken a threshold, an authorization check or a
validation rule to pass (§28.3).

**Unit (no DB)** — the employer and posting state machines, including every refused transition; the
honesty predicate (published × not past `closes_at` × employer verified) for markup, sitemap and
apply, row by row; the JSON-LD builder omits salary unless chosen, always carries `validThrough`
when `closes_at` is set, and emits nothing for a closed posting; CV allowlist and sniff rejections;
`SafeName` output.

**Repository (real MySQL as well as SQLite, per the parity gate)** — idempotent re-`Ensure`; the
flag seeds `'0'` and an operator's change survives a reboot; `UNIQUE(posting_id,
applicant_user_id)` under concurrency, **including that a withdrawn application still holds the
slot**; employer slugs globally unique; posting slugs unique per employer but not across employers;
one membership row per person per employer; salary columns declared `INTEGER`, travelling with
`currency`; editing a question does not change any `prompt_snapshot`; deleting a question leaves its
answers readable; consent withdrawal stamps `withdrawn_at` and deletes nothing; optimistic
concurrency (`version`) conflicts on stale employer and posting writes.

**Integration / abuse (live HTTP)** — the phase's whole point:

- an **unverified** employer cannot publish by any route: the transition endpoint, a direct state
  write, a re-publish after suspension — every path refuses;
- **suspending an employer** removes its postings from the list, the pages' markup and the sitemap,
  and refuses a CV download in the same session that succeeded a moment before;
- **anonymous apply** is refused with 401 — with no session, with a forged body, and with a guest
  community session (a guest ticket is not a Passport);
- after submission, **no endpoint mutates** an answer row, the CV reference or the consent — and the
  employer's view of the application is byte-identical before and after the applicant edits their
  profile and replaces their stored CV;
- employer A's member, including its `owner`, **cannot read** employer B's drafts, applications or
  CVs by id or by list;
- a **withdrawn** application refuses the employer's CV download and detail view from that instant,
  while the consent row and history remain readable to the applicant and to an audited admin;
- a **closed** posting serves a page with no `JobPosting` block, is absent from the sitemap, and
  refuses an apply POST regardless of what the page said;
- CV download without membership, or by a removed member, is refused **and audited**; the per-member
  download budget returns 429 before a bulk pull completes;
- retention purge with a legal hold in effect deletes nothing.

**Accessibility** — the careers list, job page and apply flow scanned with axe in a real browser,
including reflow at 320px, as Phases 1–3 established; apply-form validation errors are announced and
not conveyed by colour alone.

---

## 10. Blocked on decisions outside engineering

Named owners, per the D-007/D-008 discipline: the code ships flag-off and complete; these gate the
*launch*, and claiming otherwise would violate §28.20. Proposed for `CCP_ISSUE_REGISTER.md` as
CCP-P4-013 … CCP-P4-016 (next free numbers in the register's global sequence).

| # | Decision needed | Owner |
|---|---|---|
| CCP-P4-013 | The employer verification standard: what evidence proves an employer is real (registry lookups, domain control, documents), who reviews it, to what SLA, whether a second approver is required (§4.2), and the suspension/re-verification triggers. Until this exists, no employer can honestly reach `verified` and the flag stays off. | Trust & Safety lead + Operations |
| CCP-P4-014 | Employer terms of service, the applicant-facing privacy notice and consent wording (`policy_version` v1 must be authored words, not a placeholder), and the data-protection role split — PCI vs employer as controller/processor for disclosed application data. | PCI legal counsel + Data Protection Officer |
| CCP-P4-015 | Retention periods for applications and CVs per supported jurisdiction, and the erasure-request procedure's interaction with withdrawn-but-retained records (§5.3). | Data Protection Officer + legal counsel |
| CCP-P4-016 | The commercial model — free listings, paid listings, or tiers. Does not block this build (nothing here touches Stripe), but blocks any payment wiring and shapes the verification queue's expected volume. | Commercial owner |

---

## 11. What this phase does not do

- **No migration of main-PCI careers.** D-001 is explicit: `/careers/*`, its tables and its admin
  screens are untouched and out of scope. No dual write, no backfill, no reconciliation job.
- **No external ATS integrations.** §10.8 — Phase 5, contracts only until sandbox credentials exist;
  §28 forbids marking an integration complete on mocks.
- **No automated ranking, screening or rejection of candidates.** §10.7 prohibits it in Release 1.
  There is no match score, no auto-shortlist, no model-ordered queue — deliberately no code path,
  not a disabled one.
- **No posting revision history.** The application record is immutable without it (§5.2); a full
  content-versioning engine for postings is deferred until the audit trail proves insufficient.
- **No external apply URLs** (§7) — revisit only with its own safety design.
- **No employer↔applicant free-form messaging.** Templated, records-first notifications only (§5.2)
  — a two-way channel is a distinct moderation problem, the same argument as Phase 3's no-DMs.
- **No paid or promoted listings.** Blocked on CCP-P4-016; nothing here touches Stripe.
- **No CV parsing or enrichment.** The employer reviews the document the applicant consented to
  disclose — not a machine's paraphrase of it.
- **The wider question taxonomy.** Three kinds ship; the proven 9-type list is the design ceiling,
  grown per real need rather than speculatively (§1).
