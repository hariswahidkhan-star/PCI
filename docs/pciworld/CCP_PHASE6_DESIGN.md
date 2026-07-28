# CCP Phase 6 — PCI World contributor publishing (design)

Scope: contributor publishing, named as this phase in `CCP_PHASE1_DESIGN.md` (slice boundary,
"contributor publishing (Phase 6)") and shaped in advance by decision **D-003**. Identified members
of PCI World may write articles that are edited, approved and published by PCI's editorial staff
under the existing World editorial engine — never around it. Written before implementation, per the
§26 step 5 discipline Phases 1–4 followed.

A note on specification citations: the master specification is not in this repository. The only
section numbers used here are ones the existing CCP documents already use with a fixed meaning —
§11.1 ("extend the existing World editorial engine") and §11.3 ("an author cannot approve their own
article") as D-003 cites them, §18.3 (crawlable structured data, honestly emitted), §13.6
(optimistic concurrency / idempotency), §23 (test plan), §28.3/§28.5/§28.11/§28.14/§28.20, §26 and
§29. Where this document needs a rule the existing docs never number, it states the rule in words
and does not invent a citation.

Ships behind `pciworld_contributors_enabled`, seeded `'0'`. Like Phases 3 and 4, nothing here is
gated on `CCP-P1-003` or `CCP-P1-004` — there are no images in this slice and no anonymous
participation. But, like Phase 4, the exit gate has dependencies engineering cannot close: **no
contributor article can honestly be published until someone outside this repository authors the
editorial acceptance policy and the contributor terms** (§12). The code enforces the workflow; it
cannot author the standard behind it.

The schema lands in `Data/ContributorSchema.cs`, an idempotent `Ensure(db)` called from
`WorldSchema.Ensure` exactly like the community, forum and careers installers before it — and,
exactly like them, **installing the schema is not launching the feature**.

---

## 1. One publishing engine, not three — what is reused and what must not be touched

Three candidate engines exist in this repository, and the choice among them is the D-001-style
decision of this phase. Verified row by row:

| | Main-PCI blog | World editorial engine | A new contributor CMS |
|---|---|---|---|
| Store | `blog_posts` (`Migrate.cs:1329`) | `pciworld_articles` (`WorldSchema.cs:409`) | would be a third |
| Authors | `blog_authors` display rows + admin `author_id`/`reviewer_id`/`editor_id` columns (`Migrate.cs:1334`) — no account of the author's own | `author_id` → `pciworld_admin_users.id` (`WorldSchema.cs:423`) | — |
| Body | admin-authored HTML (or markdown-lite), sanitised **at render** by `HtmlSanitize.Clean` (`BlogRender.cs:334`) | Markdown subset, escaped first, rendered from a fixed vocabulary (`WorldEditorial.RenderBody`, `WorldEditorial.cs:115`) | — |
| Versioning | none — the row is the article | immutable `pciworld_article_versions`, `Snapshot()` the only writer (`WorldEditorial.cs:271`) | — |
| Maker-checker | none | `approver ≠ author`, enforced in the API (`WorldEditorial.cs:218`) | — |
| Corrections | edit in place | `Correct()` — new version + dated public note, the only path that changes published text (`WorldEditorial.cs:247`) | — |
| Reachable from a World-only deployment | **no** — `/blog` is outside `WorldOnly.Allowed()` (`WorldLifecycle.cs:13-24`) | yes — `/world/blog/{slug}` (`World.cs:157-160`) | — |

**Selected: extend the World editorial engine, exactly as D-003 already decided.** The main-PCI
blog is disqualified twice over: a World-only deployment cannot serve its routes at all, and it has
none of the invariants contributor content needs — no immutable versions, no maker-checker, no
correction discipline, and a body model (author-supplied HTML) that §7 below rejects for
contributors. A new CMS is disqualified by §28.14 and by D-003 in terms: "Build no second CMS."

**What is reused, at implementation level this time** (D-003 authorises extending, not copying):

1. `pciworld_articles` and its satellites — sources (`WorldSchema.cs:467`), reviews (`:496`),
   versions (`:436`) — extended by additive `AddCol` columns, never rewritten.
2. The whole `WorldEditorial` lifecycle: `Validate` (`WorldEditorial.cs:53`), the status chain
   (`:33`), `Review`/`Approve` maker-checker (`:206`, `:218`), `Publish` with re-validation
   (`:229`), `Correct` (`:247`), `Snapshot` (`:271`), `LiveVersion` (`:291`).
3. The public read path unchanged: `/world/blog/{slug}` serves the immutable snapshot or 404
   (`World.cs:142-155`), `WorldPages.ArticlePage` renders it with JSON-LD (`WorldPages.cs:1630`),
   `WorldSeo.Sitemap` lists published articles only (`WorldSeo.cs:61-64`). A contributor article is
   an ordinary `pciworld_articles` row, so it flows through all three with **zero new render code**.
4. `ForumTrust` — as recorded evidence feeding a human grant, not as an automatic gate (§4).
5. The `pciworld_audit` append-only log (`WorldSchema.cs:381`) and the `WorldRbac` action-group
   discipline (`WorldLifecycle.cs:94-142`).

**What must not be reused:** `HtmlSanitize` for contributor bodies (§7); the forum's
publish-immediately-for-trusted-authors timing model (§5.1); and `WorldLifecycle`'s challenge
tables — the challenge state machine is the *precedent* for revise-while-the-snapshot-serves
(`WorldLifecycle.cs:145-155`), but articles already have their own version machinery and a second
copy of it would be the two-engines mistake Phase 3 §1 names.

---

## 2. What must never happen

These are not aspirations; each maps to a structural control below.

| Prohibition | Control |
|---|---|
| A contributor approves or publishes their own article (§11.3) | §5.2 — maker-checker in `Approve` extended to compare the contributor identity, plus a recorded admin↔World-account link so the check spans both identity systems; publishing requires `WorldRbac` `publish`, which no contributor holds |
| A published article is modified in place (§28.11) | §6 — `pciworld_article_versions` is the record; `Snapshot()` remains the only writer of versions and `Correct()` the only path that changes published text; contributor revisions are working drafts until an editor republishes, and the last published snapshot serves throughout |
| Contributor input becomes markup on a PCI page | §7 — no HTML intake exists; the body is the Markdown subset rendered escape-first from a fixed vocabulary (`RenderBody`), and JSON-LD uses the default encoder (`ForumRender.cs:35-48` precedent) |
| PCI's domain authority is sold as SEO link equity | §7 — every external link in a rendered body carries `rel="nofollow noopener"` structurally (`WorldEditorial.cs:168-170`); there is no path to a followed contributor link |
| An unvetted stranger publishes under PCI's brand | §4 — contributor status is a granted, attributable, revocable record, and §5's workflow has no publish path that skips a staff editor |
| A sanction, or a classifier score, unpublishes or bans automatically (§28.5) | §5.3, §10 — there is no classifier wired into this surface and no automated sanction code path; revocation and takedown are human acts, recorded with their actor |
| A withdrawn or archived article is advertised to a crawler (§18.3) | §9 — the public page, the JSON-LD and the sitemap all read through the `status='published'` predicate at render time (`World.cs:145`, `WorldSeo.cs:61`) |
| Moderation evidence, internal legal notes or another contributor's drafts leak through the contributor surface | §5.4 — the contributor API returns only the contributor's own rows, and only the fields addressed to them; internal review notes and legal-review records are never in its SELECT list |
| A takedown or unattribution request vanishes without a decision | §8 — requests are rows with a state and a deciding actor, never an email thread |

---

## 3. Feature flag

```
pciworld_contributors_enabled   -- site setting, INSERT OR IGNORE seeded '0'
```

Every contributor write path checks it; with the flag off the contributor API refuses, the
application/grant surface is absent, and **already-published contributor articles keep serving** —
the flag gates intake, not the published record, because unpublishing a real article is an editorial
act (§8), not a side effect of an operator toggling a setting. The same kill-switch semantics as
`pciworld_forum_enabled` (`ForumPosts.cs:28`) otherwise. Turning it on is a human decision made
after §12's owners have signed off; the flag does not assert that they have.

---

## 4. Who may contribute — a grant, not a threshold

### 4.1 Why the forum's ladder is evidence here, not the gate

The forum's `ForumTrust` ladder is the closest precedent and it is deliberately **not** reused as
the deciding mechanism. The ladder answers "how much friction before this person's *forum post*
appears", and it is pure recorded fact (`ForumTrust.cs:59-61`, computed server-side only,
`ForumPublic.cs:59-78`). Contributor status answers a different question: *may this person write
under PCI's own byline surface, on pages PCI's brand vouches for to search engines?* That is an
assertion PCI makes about a person — the same shape as Phase 4's employer `verified`, which is "an
attributable act, not a flag" (`CareersSchema.cs:24-27`). A threshold crossing cannot make that
assertion; a named person can.

So: **the ladder is reused as the eligibility floor and as evidence in front of the human; the
grant itself is a recorded staff decision.**

- Applications are open to Passport accounts (`pciworld_users`, `WorldSchema.cs:221`) with a
  verified email, and the application surfaces the applicant's forum standing — accepted posts,
  upheld reports, `ForumTrust.Of` level — to the reviewing editor.
- Where the forum is live, a floor of `ForumTrust.Level.Member` applies to *applying* (an accepted
  posting record is cheap, honest evidence). Stated honestly: `pciworld_forum_enabled` seeds `'0'`
  and may never be turned on, so the floor is **conditional on the forum being enabled** — with the
  forum off, the floor is verified email alone and the editor decides on the rest of the
  application. A floor that silently made contribution impossible would be a dead feature wearing a
  policy's clothes.
- Rejecting the pure auto-earn alternative explicitly: the forum's own demotion logic shows why. An
  earned-only gate is "a ratchet an abuser plays for" (`ForumTrust.cs:88-90`) — post innocuously
  until the gate opens, then publish spam under PCI's brand. The forum bounds that damage by
  withdrawing a post; a published article is indexed under PCI's name, which is exactly the
  irreversibility argument that made Phase 4 reject optimistic publication (§5.1).

### 4.2 The grant record

```
pciworld_contributors(
  id, user_id UNIQUE,             -- one standing row per person; a state change is an UPDATE,
                                  -- never a second row (the employer-member precedent,
                                  -- CareersSchema.cs:73-77)
  state,                          -- applied|granted|declined|revoked
  statement,                      -- the applicant's own words, bounded
  terms_version,                  -- the contributor terms they accepted — pinned, like consent
                                  -- policy_version in Phase 4 §5.3; consent to last year's terms
                                  -- is not consent to this year's
  granted_at, granted_by_admin_id,
  revoked_at, revoked_by_admin_id, revoke_reason,
  created_at, updated_at, version)
```

Grant and revocation record *who* and *when*, for the same reason `verified_by_admin_id` does: a
bare state that nobody can later explain is an unattributable claim. Both are audited to
`pciworld_audit`. Revocation is a human act with a recorded reason — never an automatic consequence
of a report count or a classifier score (§28.5); what report counts do is put the row in front of a
human. The World Admin side adds `editorial.contributors` as a `WorldRbac` action group (grant,
revoke, review applications), following the careers precedent that verifying a person's standing and
editing their text are different jobs (`WorldLifecycle.cs:125-130`) — `author`-role admins do not
hold it; it sits with `owner` and whichever editorial role the rollout assigns.

`terms_version` v1 must be **authored words, not a placeholder** — the same rule as
`pciworld_careers_policy_version` (register, CCP-P4-014): while it is unset, the application
endpoint answers 503 rather than recording acceptance of nothing. That is part of what §12 blocks
launch on.

---

## 5. The editorial workflow, end to end

### 5.1 Every contributor article is human-reviewed before publication — why the forum's timing model is rejected

Phase 3 lets a trusted author publish immediately because the exposure window costs at most a bad
post, and withdrawal closes it. That trade does not transfer, for the Phase 4 §4.3 reason: **the
harm window here is indexing under PCI's brand**, and withdrawal cannot un-crawl a page. `ForumRender`
itself states the asymmetry — forum output "is CACHED BY SEARCH ENGINES … a leak here can outlive
the deletion by months, in a cache nobody at PCI controls" (`ForumRender.cs:18-23`) — and an article
page is built to rank harder than any thread. So there is no optimistic path and no trust level that
buys one: every contributor article passes through the existing status chain
(`idea → drafting → technical_review → fact_check → legal_review → seo_review → approved →
published`, `WorldEditorial.cs:33-34`) with a staff editor at the wheel. Trust affected *whether you
may submit at all* (§4); it never affects what happens to a submission. This is stricter than the
forum, not looser, and it is stated as the design rather than an operational hope.

A text classifier is deliberately **not** wired into this surface. The moderation engine takes a
`contentType` and could grow an `article` matrix beside `text`/`image`/`post`
(`CommunityModeration.cs:110`), but every submission already gets the strongest review the platform
has — a human editor with the power to refuse — so a classifier could only re-order a queue, and
wiring it in would create the §28.5 surface (a low-confidence score near a sanction) this phase
otherwise structurally lacks. Deferred until queue volume proves a need, and recorded here so the
absence is read as a decision rather than an omission.

### 5.2 Submission, assignment, and the maker-checker across two identity systems

Additive columns on `pciworld_articles` (guarded `AddCol`, per the `CareersSchema.cs:235-239`
pattern):

```
contributor_user_id       -- pciworld_users.id; NULL = house content. Distinct from author_id
                          -- (the editorial owner, a pciworld_admin_users.id) per D-003.
contributor_terms_version -- pinned at submission
declarations_json         -- conflict / sponsorship / AI-assistance / originality / rights
                          -- declarations (D-003's list), frozen into the submission event
```

New satellite tables, all `pciworld_`-prefixed (D-004), datetimes `VARCHAR(32)` never `TEXT`,
`version` on mutable rows, no partial unique indexes — the `CareersSchema` conventions in full:

```
pciworld_contributor_events(      -- append-only submission status history (D-003)
  id, article_id, event, from_status, to_status,
  actor_kind,                     -- contributor|editor|system
  actor_id, note, declarations_json, created_at)

pciworld_contributor_assignments( -- which editor owns which manuscript
  id, article_id, editor_admin_id, assigned_at, unassigned_at, created_at)

pciworld_contributor_messages(    -- contributor↔editor thread, per article (D-003)
  id, article_id, sender_kind,    -- contributor|editor
  sender_id, body, created_at)    -- append-only; plain text, rendered escaped
```

The message thread looks like the free-form messaging Phase 4 §11 refused, so the difference is
argued rather than assumed: employer↔applicant messaging is stranger-to-stranger through PCI's
pipes, a moderation problem PCI would own without an editor in the loop. This thread has an
accountable staff member as one of its two parties on every message, exists only per-manuscript,
and is the alternative to the same conversation happening in unrecorded email — which it otherwise
would. Bodies are bounded and rendered escaped; there is no markup in messages.

The contributor writes through a Passport-authenticated contributor API (`/api/world/contributor/*`,
inside `WorldOnly.Allowed()` today, React surface per D-009) that can touch **only** rows where
`contributor_user_id` is the session's user, only while `WorldEditorial.CanEdit` says the status is
editable (`WorldEditorial.cs:188`), and returns only the contributor-facing fields — the submission
history addressed to them, the message thread, review outcomes. Internal review notes, legal-review
rows, other people's drafts and anything in the moderation evidence store are simply not in its
queries; the isolation is asserted by tests the way Phase 4 asserts tenant isolation, not by
convention.

**The maker-checker is the phase's §11.3 obligation, and it has a real gap to close.** Today
`Approve` compares `author_id` to the approver (`WorldEditorial.cs:218`) — both
`pciworld_admin_users` ids. A contributor is a `pciworld_users` row; the two id spaces are disjoint
and share no recorded link (verified: `pciworld_admin_users`, `WorldSchema.cs:361-371`, has no
World-account column). Comparing ids across the two systems is the classic wrong-namespace bug, and
comparing emails is folklore, not identity. The structural fix:

1. `AddCol pciworld_admin_users.world_user_id` — an explicit, owner-managed link between an admin
   account and its holder's Passport account, audited when set.
2. `Review`, `Approve` and `Publish` refuse (`maker_checker`) when the acting admin's
   `world_user_id` equals the article's `contributor_user_id` — in the same query-and-refuse shape
   as the existing check, in the API, not the UI.
3. Stated honestly: an admin whose link is unset and who also holds an undeclared Passport account
   defeats the check. The code cannot prove two accounts are one person; that residue is a
   staffing-integrity matter and belongs to the conflict-of-interest limb of the editorial policy
   (§12, CCP-P6-020). The declared-conflicts field exists so the honest case is easy; the check
   exists so the recorded case is impossible.

**When the only available reviewer is the author, the article waits.** Maker-checker means a second
person, and an unavailable second person is a staffing gap, not a waiver — exactly the posture of
Phase 4's verification gate ("the code enforces the gate; it cannot author the procedure behind
it"). There is no emergency-bypass parameter, deliberately: a bypass that exists gets used. Review
capacity and SLA are part of CCP-P6-020's operational limb.

### 5.3 A sanctioned author

Sanctions on the account (community/forum machinery) and contributor standing interact in exactly
one direction: a sanction is **evidence for a human revocation decision**, never an automatic one
(§28.5; the flag-vs-sanction split of Phase 3 §7).

- **Approved but not yet published, author sanctioned or revoked in between:** `Publish` already
  re-validates because "approval is necessary and never sufficient" (`WorldEditorial.cs:224-227`).
  This phase adds the contributor-eligibility predicate to that same gate: publish refuses
  (`contributor_not_eligible`) when the article carries a `contributor_user_id` whose grant is not
  `granted` or whose account is not active. Checked inside `Publish`, in the transaction, not in
  the UI — the Phase 4 "predicate in the transaction" discipline.
- **Already published, author later sanctioned:** the article stays up. A sanction is about
  conduct; unpublishing is about content, and conflating them would turn every sanction into
  retroactive content destruction — the same separation Phase 2 draws between hiding a message and
  destroying evidence. What the sanction does is open an editorial review of the author's published
  pieces; archiving any of them is a distinct, audited human act (§8).

---

## 6. Where the immutability boundary sits

Phase 4 §5.2 had to choose between the working copy and the record, and so does this phase. The
answer here is the one the editorial engine already embodies: **the working copy is the
`pciworld_articles` row; the record is `pciworld_article_versions`** — immutable by construction
(`INSERT OR IGNORE` under `UNIQUE(article_id, version)`, `WorldSchema.cs:449`; `Snapshot()` the only
writer, `WorldEditorial.cs:271-276`). The public reader is never served the working copy
(`LiveVersion`, `WorldEditorial.cs:291-297`; `World.cs:147`).

The §28.11 rule — *revision means a new version, with the last published snapshot still serving
until republished* — therefore needs no new machinery, only a disciplined flow for the contributor's
side of it:

- A contributor's **proposed revision to a published article is not an edit of anything.** It lands
  as a request row (§8's table) carrying the proposed text; the working `pciworld_articles` row and
  the served snapshot are both untouched while it waits. This is deliberately more conservative than
  `WorldLifecycle.Revise`, which flips the working copy back to `draft` while the snapshot serves
  (`WorldLifecycle.cs:145-155`, `:212-215`): the challenge model trusts the working copy to staff;
  here the proposer is not staff, and a contributor able to rewrite the working copy of a published
  article would be a contributor able to stage text that the next unwary `Correct()` republishes.
- An editor who accepts the proposal applies it **through `Correct()`** — new version, dated public
  correction note, full re-validation, maker-checker upstream (`WorldEditorial.cs:247-269`). The
  D-003 invariants are preserved verbatim: `Snapshot()` remains the only writer of versions,
  `Correct()` remains the only path that changes published text, and this phase adds **no third
  writer of either**.
- Unpublished drafts revise freely in place — before publication the working copy *is* the
  manuscript, guarded by `CanEdit` status checks and the `pciworld_articles` audit trail, exactly as
  house content behaves today (`WorldAdmin.cs:348-362`).

---

## 7. Contributor-authored content — what markup is permitted, and why an allowlist

**Contributors do not author HTML at all.** The body is the same Markdown subset every
`pciworld_articles` row already stores in `body_md`, rendered by `WorldEditorial.RenderBody`
(`WorldEditorial.cs:115-153`): text is HTML-escaped **first**, then markup is *generated* from a
fixed vocabulary — `##`/`###` headings, paragraphs, bullets, numbered lists, blockquotes, bold,
italic, code, and `[text](https://…)` links. Anything outside the vocabulary degrades to visible
escaped text, not to markup. That is the allowlist argument in one sentence: an allowlist's failure
mode is ugly text; a blocklist's failure mode is script execution, because a blocklist must
enumerate every bad construct in advance and attackers publish new ones.

The two in-repo alternatives are rejected by name:

- **`HtmlSanitize` — rejected for contributor bodies.** It is scoped, by its own doc comment, to
  "operator-edited rich-text blocks" (`HtmlSanitize.cs:6-13`), and its allowances are calibrated to
  that trust level: a global `style` attribute and `class` (`:39`), `img` with arbitrary
  external `src` (`:33-34`), tables and `div`s, and a `SafeUrl` that is itself a **blocklist** of
  three schemes (`:126-131`). Admin-authored HTML through it is the platform's accepted risk;
  contributor-authored HTML through it would hand every granted stranger a styling and
  remote-image surface on indexed PCI pages. The honest footnote: the forum *does* pass member text
  through `HtmlSanitize.Clean` at write time (`ForumPosts.cs:218`). This phase does not copy that,
  and does not need to argue it away either — the editorial engine's intake is already `body_md`,
  so choosing `RenderBody` is choosing the engine's own format rather than adding an HTML intake
  that does not exist today.
- **Extending `RenderBody` with raw-HTML passthrough for "trusted" contributors — rejected.** Trust
  earns submission, never markup (§4, §5.1).

Consequences of the vocabulary, stated as the design:

- **Images: none in this slice.** `RenderBody` has no image syntax (verified — the vocabulary at
  `WorldEditorial.cs:113-114` ends at links), and adding one would mean either hotlinking (a
  privacy leak and a defacement vector — the remote server decides what the page shows tomorrow) or
  the Phase 2 media pipeline, which is flag-off pending `CCP-P1-003`; wiring it here would inherit
  that block for no gain — the same reasoning as Phase 3 §10. Media-rights metadata
  (D-003's `pciworld_contributor_media_rights`) is deferred with it, so the table ships when the
  thing it describes can exist.
- **Links: permitted, and permanently second-class.** An article without references is a thin page,
  so links stay; but the rendered anchor for any external URL carries
  `rel="nofollow noopener" target="_blank"` structurally (`WorldEditorial.cs:168-170`), and the
  scheme grammar admits only `https?://` or site-relative-not-protocol-relative. `javascript:`,
  `data:` and friends never match and remain visible text. Nofollow is the economic control, not
  just a courtesy: the single strongest motive for contributor-content abuse on any site with
  domain authority is selling followed links, and here there is no code path that emits one.
- **Bounds.** `Validate` already floors the body at 200 words (`WorldEditorial.cs:64-65`); this
  phase adds a ceiling (`MaxBodyLength = 100_000` characters — five times the forum's
  `ForumPosts.MaxBodyLength` of 20,000, an article being an argument's long form) because an
  unbounded TEXT column is a denial-of-service lever (`ForumPosts.cs:31-33`). Message and statement
  fields are bounded likewise.
- **JSON-LD stays on the default encoder** — titles and deks are now stranger-authored, which is
  exactly the `</script>`-in-a-title attack `ForumRender` documents and defends against
  (`ForumRender.cs:35-48`). `WorldPages.Json` output embedded in `ArticlePage` must keep that
  property; the test plan feeds it a title of that shape (§11).

---

## 8. Attribution, retraction, takedown

A published contributor article carries the contributor's real name as its byline — the governance
admits "a real named person or the transparent 'PCI World Editorial' byline … and no others"
(`WorldEditorial.cs:22-24`, enforced at `:67-69`). Whether pseudonymous contribution is ever
acceptable is editorial policy, not engineering (§12, CCP-P6-020); the code ships the two-byline
rule it inherits.

Requests about a published article are rows, not emails:

```
pciworld_contributor_requests(
  id, article_id, contributor_user_id,
  kind,                 -- revision|unattribution|takedown
  note, proposed_body,  -- proposed_body used by kind='revision' (§6)
  state,                -- open|actioned|declined
  decided_by_admin_id, decided_at, decision_note,
  created_at)
```

- **Unattribution** ("keep the article, remove my name"): actioned through `Correct()` — a new
  version bylined `PCI World Editorial`, a dated correction note, the snapshot chain intact. From
  that instant the public page, its JSON-LD and the feeds carry no name, because they all read the
  live version (`World.cs:147`, `WorldPages.cs:1636`). The name survives in prior version
  snapshots, which are **internal record, never publicly served** (no public route reads
  `pciworld_article_versions` except through `LiveVersion` — verified across `Endpoints/`), and
  that survival is deliberate: "who wrote the words PCI published in March" must stay answerable —
  the Phase 4 §5.3 withdrawal-without-amnesia rule applied to bylines.
- **Takedown** ("remove the article"): actioned through `Archive` (`WorldEditorial.cs:287`) — the
  page 404s (`World.cs:145`) and leaves the sitemap (`WorldSeo.cs:61`) at once. This is the
  opposite call from Phase 4's closed-job page, and the reversal is argued: a closed job stays
  readable because its content is still true and an applicant's saved link should explain itself;
  a taken-down article usually comes down *because of its content* (rights, defamation, the
  author's safety), so continuing to serve any of it — even inside a tombstone — may be exactly the
  harm. A tombstone page that says only "removed" without touching the content needs wording that
  is itself a legal decision, so it is deferred into CCP-P6-021 rather than guessed at.
- **Retraction** is not takedown: a piece PCI no longer stands behind but must be seen to have
  corrected stays published, with the retraction as a correction note — the corrections block is
  already rendered as a dated public record (`WorldPages.cs:1642-1648`).
- **What is not pretended:** none of this un-indexes third-party caches. Search engines converge on
  recrawl of the 404 or the corrected page; PCI controls its origin, not the internet's memory.
  The contributor-facing copy for these requests must say so — the §7.1-style honesty rule.
- **Erasure is the stronger, separate flow.** Verified: `Erasure.Anonymise` (`Erasure.cs:19`)
  touches no `pciworld_` table today, so World-side coverage of the tables this phase adds is part
  of this phase's build. The hard case is a data-subject erasure demand reaching the byline inside
  immutable version snapshots: legal obligation outranks the record's immutability, and the honest
  mechanism is a **recorded redaction** — the name overwritten with a redaction marker across
  version rows in one audited act that itself leaves a row — never a silent edit. When that
  applies, versus when the retained-record interest prevails, is the DPO's call (§12, CCP-P6-021).

---

## 9. Crawlable pages and structured data (§18.3, D-009)

Public reading is server-rendered and already exists: contributor articles serve through
`/world/blog/{slug}` with `BlogPosting` + `BreadcrumbList` JSON-LD (`WorldPages.cs:1664-1676`,
`:1704`). The contributor composer, the application flow and the editorial console are React,
exactly as D-009 splits it. All routes fall inside `WorldOnly.Allowed()` today; no allowlist change.

The honesty rules, in the Phase 4 §7 shape:

- Markup is emitted only for a `status='published'` article's live snapshot — computed at render
  time by the same query that decides whether the page exists at all (`World.cs:145-148`), so a
  withdrawn article and its structured data disappear in the same instant, and honesty never
  depends on a cache or a sweep. The sitemap applies the identical predicate (`WorldSeo.cs:61-64`).
- A **draft, in-review, approved-but-unpublished or archived** article is a public 404 — a
  manuscript's existence is the contributor's and the desk's business, the same
  tenant-confidentiality rule as Phase 4's draft postings.
- **The author becomes a `Person`.** Today `ArticlePage` marks every author as
  `@type: Organization` (`WorldPages.cs:1672`) — tolerable while every byline was house, wrong the
  day a byline is a human being. Contributor articles emit
  `author: { @type: "Person", name: … }` from the live snapshot's byline, and nothing else about
  the person — no URLs, no affiliation the page does not show, per the "structured data describing
  exactly what the page shows" rule already stated at `WorldPages.cs:1662-1663`.
- **Contributor articles are `kind='blog'` only in this slice.** `NewsArticle` markup rides on the
  newsroom's source-tracing obligation (`WorldEditorial.cs:77-79`) and its entity-mention legal
  gate; opening the news kind to contributors would either dilute that or demand a
  claim-verification desk this phase does not staff. An editor may always take a contributor's
  reporting in-house through the existing newsroom workflow.
- Unattribution and correction propagate to markup automatically because JSON-LD reads the live
  version — there is no second copy of the byline to forget.

---

## 10. Rate limiting and abuse

Per account, never per IP — the Phase 3 §8 shared-office argument, unchanged — and returned as
`429 rate_limited` in the `ForumTrust.HourlyPostBudget` pattern. The abuse surface this phase adds
is not volume but **quality-washing**: using PCI's domain to launder spam, SEO placements or
generated sludge. The controls, in order of load-bearing:

1. The human editor (§5.1) — every submission costs a person's attention, which is precisely why
   budgets protect that attention: a bounded number of open submissions per contributor (drafts in
   review at once), a daily submission budget, and message/request budgets a genuine author never
   notices.
2. Nofollow-always links (§7) — removes the economic prize before moderation has to catch it, the
   same shape as the forum keeping links from `new` accounts (`ForumTrust.cs:119-121`).
3. The grant is revocable with reasons (§4.2), and an upheld pattern of bad-faith submissions is
   exactly what the revocation surface is for — decided by a person, evidenced by the append-only
   event history.
4. AI-assistance and originality declarations (§5.2) are recorded at submission. Stated honestly:
   a declaration is a lever for accountability after the fact, not detection — this repository
   must not pretend it can detect generated text, and the admin UI must not imply it (§28-family
   honesty; the Phase 1 "layered deterrent, not ban enforcement" rule applied to provenance).
5. Body, message and statement bounds; optimistic-concurrency `version` columns on mutable rows so
   two editors cannot silently overwrite each other (§13.6).

---

## 11. Test plan — what must fail first

Written before implementation; no test may weaken a threshold, an authorization check or a
validation rule to pass (§28.3).

**Unit (no DB)** — the eligibility floor function, pure like `ForumTrust.Of`, including the
forum-off fallback; `RenderBody` fed contributor-shaped hostility: raw HTML, `<script>`,
`javascript:`/`data:`/protocol-relative links, a `</script>` title through the JSON-LD path (the
`ForumRender` attack), the 100k body ceiling; every external link in rendered output carries
nofollow, asserted as a property over the rendered corpus, not per-case; `Validate` still refuses a
missing byline and a sub-200-word body for contributor rows.

**Repository (real MySQL as well as SQLite, per the parity gate)** — idempotent re-`Ensure`; the
flag seeds `'0'` and an operator's change survives a reboot; `UNIQUE(user_id)` on
`pciworld_contributors` under concurrent applications; the `AddCol` columns converge on a database
created before them; datetimes bounded `VARCHAR(32)`; `pciworld_contributor_events` has no UPDATE
path anywhere in the codebase (asserted by grep-style test, the `pciworld_application_events`
precedent); version rows for a corrected contributor article are append-only and gap-free.

**Integration / abuse (live HTTP)** — the phase's whole point:

- a contributor **cannot approve, review or publish their own article by any route**: the admin
  endpoints refuse a linked admin (`maker_checker`), the contributor API simply has no such
  endpoints, and a forged status value through `Submit` cannot reach `approved` or `published`
  (`WorldEditorial.cs:192` already refuses both; re-asserted here for contributor rows);
- **publishing a contributor article whose author was revoked or sanctioned after approval is
  refused** (`contributor_not_eligible`) in the publish transaction;
- after publication, **no contributor-reachable endpoint changes served text**: a revision request
  leaves the page and its JSON-LD byte-identical until an editor acts; the accepted revision serves
  the *old* snapshot up to the instant `Correct()` commits and the *new* one after, with the
  correction note rendered;
- contributor A **cannot read** contributor B's drafts, events, messages or requests, by id or by
  list; an anonymous or guest-community session reaches nothing;
- the contributor API response for a reviewed article **contains no internal review notes, no
  legal-review rows and no moderation-evidence references** — asserted on the JSON, field by field;
- an **archived** article 404s, leaves the sitemap and emits no structured data in the same
  request cycle; an unattributed article's page, JSON-LD and feed entries carry no name;
- with the flag **off**, intake refuses everywhere while a published contributor article still
  serves;
- submission/message/request budgets return 429 before a flood lands; a stale `version` write
  conflicts.

**Accessibility** — the contributor composer, application flow and the request surfaces scanned
with axe in a real Chromium including reflow at 320px, per the Phase 1–4 pattern
(CCP-P2-011's browser harness); validation and refusal states announced, never colour-alone. The
public article page is already served HTML; it joins the scan to pin the byline and corrections
blocks.

---

## 12. Blocked on decisions outside engineering

Named owners, per the D-007/D-008 discipline: the code ships flag-off and complete; these gate the
*launch*, and claiming otherwise would violate §28.20. Proposed for `CCP_ISSUE_REGISTER.md` as
**CCP-P6-020 … CCP-P6-023** (the register's global sequence continues from Phase 5's CCP-P5-017…019, which were registered while this document was being drafted; the sequence ends at
CCP-P4-016). Role titles, not names — this repository does not know who holds them.

| # | Decision needed | Owner |
|---|---|---|
| CCP-P6-020 | The editorial acceptance policy for contributor content: quality bar, subject scope, byline/pseudonym rules, AI-assistance and originality policy, the conflict-of-interest standard (including the undeclared-second-account residue in §5.2), review staffing and SLA. Until this exists no submission can honestly be accepted, and the flag stays off. | Editorial lead + Trust & Safety lead |
| CCP-P6-021 | The defamation/legal review procedure for contributor text, the takedown procedure (who decides, to what SLA, counter-notice, tombstone wording if any), the retraction standard, and the erasure-vs-record rule for bylines inside immutable versions (§8). | PCI legal counsel + Data Protection Officer |
| CCP-P6-022 | The contributor terms: rights and licence in the words (what PCI may keep serving after revocation or a takedown request, and why), `terms_version` v1 as authored words — the application endpoint answers 503 while it is unset rather than recording acceptance of nothing. | PCI legal counsel + Editorial lead |
| CCP-P6-023 | Whether contributors are ever paid, and the sponsorship/disclosure rules their declarations feed. Does not block this build (nothing here touches Stripe), but blocks any payment wiring and shapes expected submission volume. | Commercial owner + Editorial lead |

---

## 13. What this phase does not do

- **No second CMS, and no migration of the main-PCI blog.** D-003 and §28.14; `blog_posts`, its
  admin screens and `/blog/*` are untouched and out of scope.
- **No images or media in contributor articles.** The Phase 2 pipeline stays flag-off pending
  `CCP-P1-003`; media-rights metadata ships when media can (§7).
- **No contributor access to the news kind, entity mentions or the sources workflow** — blog kind
  only (§9); the newsroom's obligations stay staff-side.
- **No automated classification, scoring or sanctioning of submissions.** Human review is the gate;
  there is deliberately no classifier code path on this surface (§5.1), not a disabled one.
- **No trust level that publishes without an editor.** Trust affects eligibility to submit, never
  publication timing (§5.1).
- **No raw-HTML intake, at any trust level, ever** (§7).
- **No public version-history browsing** — versions are the internal record; the public sees the
  live snapshot and the dated corrections list, which is what the correction discipline already
  promises readers.
- **No comments on articles.** Discussion belongs to the forum, which exists for the purpose.
- **No co-authoring, no contributor-to-contributor visibility, no contributor directory pages.**
  Each is its own privacy/SEO decision; none is needed for the slice to be honest and complete.
- **No payment to contributors.** Blocked on CCP-P6-023; nothing here touches Stripe.
