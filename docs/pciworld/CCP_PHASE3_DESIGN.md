# CCP Phase 3 — the PCI World forum (design)

Scope: §9. A Passport-authenticated professional forum with dynamic taxonomy, trust levels,
versioned posts and crawlable structured data. Built new (decision **D-002**); the legacy main-PCI
forum stays in place, unmigrated, still serving the Institute site.

Ships behind `pciworld_forum_enabled`, seeded `'0'`. Unlike Phase 2, **nothing here is gated on an
external decision** — the forum has no image pipeline and no anonymous participation, so
`CCP-P1-003` and `CCP-P1-004` do not block it. Its exit gate is achievable from this repository.

---

## 1. What makes this different from Phase 1 rooms

Phase 1 is a *guest* surface: no account, ephemeral identity, everything moderated before it is
visible, retained briefly. The forum is the opposite on every axis, and the differences are the
design:

| | Community rooms (Phase 1) | Forum (Phase 3) |
|---|---|---|
| Identity | anonymous guest session | **Passport account, always** |
| Persistence | transient transcript | durable, indexed, crawlable |
| Moderation timing | **pre**-publication, every message | **post**-publication for trusted authors; pre for new ones |
| Editing | none | versioned, with visible history |
| Taxonomy | fixed room list | admin-managed categories and tags |
| SEO | `noindex` | server-rendered with JSON-LD (§18.3) |

The one thing they share is the moderation engine. `CommunityModeration.Resolve()` already takes a
`contentType`, so the forum adds a `post` matrix beside `text` and `image` rather than a third
decision path — the same argument as Phase 2: two engines would be two places for the publication
rule to be wrong.

---

## 2. Why post-publication moderation here, and why that is not a weakening

Holding every forum post until a classifier clears it would make the forum unusable — a professional
discussion where each reply appears minutes later is not a discussion — and it would be *less* safe
than it sounds, because the pre-publication queue for a busy forum becomes a backlog nobody reads.

So the rule is **trust-graded**, and the grading is earned rather than assumed:

- **A new account's first posts are held.** Untrusted authors are pre-moderated exactly like Phase 1,
  because a brand-new account is the cheapest thing an abuser has.
- **A trusted author publishes immediately, and is classified in parallel.** A confident adverse
  verdict *withdraws* the post — it is removed from view, the author is told, and the case is opened.

**The invariant is that trust changes TIMING, never OUTCOME.**

It has to be stated that way round, because the tempting phrasing — "severe categories are
pre-moderated at every trust level" — is not implementable and it would be dishonest to write it as
though it were. A post's category is not known until it has been classified, so no amount of policy
can hold *only* the severe posts before classification; that would require knowing the answer before
asking the question.

What is real, and is what the tests assert:

- `Resolve()` returns the **identical decision** for a post regardless of the author's trust level.
  Trust is not an input to the policy engine and must never become one.
- A verdict that withholds, escalates or sanctions does all of those things for a `trusted` author
  exactly as for a `new` one. Trust buys the post a head start, not an exemption.
- The **exposure window** for an optimistically published post is therefore real and is stated
  plainly rather than glossed: it is the classifier's latency. That is the price of a usable forum,
  and it is bounded by classifying on the write path rather than on a slow queue, by withdrawing
  automatically, and by the fact that only accounts with an accepted-posting record get it at all.

---

## 3. Trust levels

Five levels, earned by observable behaviour, never self-declared:

| Level | Reached by | Gains |
|---|---|---|
| `new` | signing up | read; post held for review; no links; no images |
| `basic` | verified email + a first accepted post | post held only for severe categories; links allowed |
| `member` | sustained accepted posting, no upheld reports | immediate publication; can edit own posts; can flag |
| `trusted` | long record, community flags that proved accurate | flags weighted; can see a limited review queue |
| `staff` | granted by a World admin | moderation actions, per `WorldRbac` |

Promotion is computed from recorded facts (accepted posts, upheld reports against, time since first
post) by a pure function so it is testable without a clock or a database, and it is **recorded as an
event**, never as a bare column overwrite — a demotion has to be explainable.

**Demotion exists.** A trust level that only goes up is a ratchet an abuser plays for. An upheld
report demotes, and the demotion is an append-only event like every other decision in this increment.

---

## 4. Versioned posts (§28.11)

*Never modify published content in place.* An edit writes a **new revision** and repoints the post's
`current_revision_id`; the prior revision stays. This gives three things that matter:

1. A reader can see that a post was edited, and when. Silent edits are how quote-mining works.
2. A moderator reviewing a report sees **the revision that was reported**, not whatever the author
   has since changed it to.
3. Withdrawal is a state change on the post, not a deletion of its history — an appeal has something
   to appeal about.

```
pciworld_forum_posts(
  id, thread_id, author_user_id, current_revision_id,
  state,                 -- pending|published|withheld|withdrawn|deleted
  kind,                  -- opening|reply
  reply_to_post_id, published_at, edited_at,
  flag_count, decision_id, created_at, updated_at, version)

pciworld_forum_post_revisions(
  id, post_id, revision_no, body, body_rendered, body_hash,
  edited_by_user_id, edit_reason, created_at)
```

`body_rendered` is sanitised at write time via `HtmlSanitize`, so the read path never renders
unsanitised author input and a sanitiser change cannot retroactively expose an old post. The raw
`body` is kept for editing and for evidence.

**Deletion by an author hides; it does not erase.** The post's state becomes `deleted` and the body
stops being served, but revisions and moderation history survive for the report and appeal windows —
the same distinction Phase 2 draws between hiding a message and destroying evidence.

---

## 5. Dynamic taxonomy

The legacy forum hardcodes five categories in C#. Here they are data:

```
pciworld_forum_categories(id, slug UNIQUE, title, description, sort,
                          min_trust_to_post, state, locale, created_at, updated_at, version)
pciworld_forum_tags(id, slug UNIQUE, label, created_at)
pciworld_forum_thread_tags(thread_id, tag_id)          -- PK(thread_id, tag_id)
```

`min_trust_to_post` is what makes a category like *Announcements* read-only to most people without a
separate permission system. Categories are managed through the existing admin CRUD factory pattern,
so adding one is configuration rather than a deployment.

---

## 6. Threads and reading

```
pciworld_forum_threads(
  id, category_id, slug, title, author_user_id,
  state,                 -- open|locked|archived|hidden
  is_pinned, reply_count, last_post_at, last_post_user_id,
  view_count, solved_post_id, created_at, updated_at, version)
```

`solved_post_id` marks an accepted answer, which is both a courtesy to the next reader and the thing
that makes `QAPage` structured data honest.

**Reading is server-rendered** (§18.3, D-009). Thread pages must be crawlable, so the public read
path renders HTML with JSON-LD (`DiscussionForumPosting`, and `QAPage` where a thread has an accepted
answer) rather than being a React shell. The authenticated composer and the moderation console are
React, exactly as D-009 splits it.

---

## 7. Reports and moderation

Reports reuse `pciworld_community_reports` shape but reference a post, and cases reuse
`CommunityCases` unchanged — a report on a forum post opens the same kind of case, appears in the
same queue, and follows the same append-only decision history as a room report. There is no second
moderation console.

What is new is the **flag**, which is not a report: a member marking something as low quality feeds a
counter that can hide a post pending review once it crosses a threshold weighted by the flaggers'
trust. Two guards, both tested:

- **A flag is never a sanction.** It can hide a post pending review; it cannot ban, demote or delete.
- **Flag weight is capped**, so no single trusted account and no small coordinated group can hide
  arbitrary content. Crossing the threshold opens a case for a human; it does not decide one.

---

## 8. Rate limiting and abuse

Per account, not per IP (a shared office must not throttle a team), and graded by trust: a `new`
account gets a small posting budget and a minimum interval between posts; `member` and above get
practical limits. New accounts cannot post links, which removes the single most common spam payload
before moderation has to catch it.

---

## 9. Test plan — what must fail first

**Unit (no DB)** — trust promotion and demotion from recorded facts; the `post` policy matrix row by
row, plus the property that **the decision is independent of trust** (same signal, same verdict, at
every level); flag weight capping; revision numbering.

**Repository (real MySQL as well as SQLite)** — idempotent install; `UNIQUE` on category and tag
slugs; a revision chain that cannot skip or reuse a number; a post whose `current_revision_id` always
resolves.

**Integration / abuse (live HTTP)** —
- a `new` account's first post is **not** visible to a second reader until it clears;
- a trusted author's post is visible immediately, and a confident adverse verdict **withdraws** it;
- an edit does not change what a moderator sees for an existing report;
- an author deleting a post hides it but leaves the moderation history intact;
- flags from a coordinated group cannot exceed the cap;
- a category's `min_trust_to_post` cannot be bypassed by posting a reply instead of a thread.

**Accessibility** — thread pages and the composer scanned with axe in a real browser, including
reflow at 320px, as Phase 1 and 2 established.

No test may weaken a threshold, an authorization check or a validation rule to pass (§28.3).

---

## 10. What this phase does not do

- No legacy import. D-002 defers "Legacy Guest" read-only import to a post-Release-1 editorial call.
- No private messaging. Not in §9, and a DM system is a distinct moderation problem.
- No images in posts in this slice — the Phase 2 pipeline exists and can be attached later, but it
  is still flag-off pending `CCP-P1-003`, so wiring it here would inherit that block for no gain.
