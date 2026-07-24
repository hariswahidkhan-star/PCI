# PCI World Expansion — Phase 4 delivery report

_Baseline: `main` @ `99e88f1` (Phase 3). Scope: the Phase 4 row of EXPANSION_PHASE0.md §10 — the
editorial platform and the first article batch._

## 1. The editorial platform

One CMS serves the blog and the newsroom, because they differ in **obligations** rather than in
machinery. The governance's four promises about published writing are implemented as properties of
the code, not as reminders in a style guide:

| Promise | How it is enforced |
|---|---|
| Nothing is edited silently | There is no code path that edits published text. `Correct()` requires a note of at least ten characters, appends a dated public correction record, and writes a **new** version — the version a reader was served earlier still exists, unchanged |
| Nobody approves their own work | Maker-checker in SQL at both approval *and* review, so a fact-check by the author is refused, not just an approval |
| A news claim traces to a source | A `news` article cannot be approved with zero recorded sources; each source link records **which claim** it supports, because a bibliography proves nothing |
| Authorship is never invented | The validator requires an author; the batch uses the transparent "PCI World Editorial" byline, which is one of the two options the governance permits (the other being a real named person) |

Two further gates: naming a registered entity blocks approval until a legal review is recorded, and
articles under 200 words are refused outright — thin pages are prohibited by the SEO policy, not
merely discouraged. Publication **re-validates**, so an entity mention or a removed source added
between approval and publish blocks the publish.

**Schema:** `pciworld_articles` + `_article_versions` (immutable snapshots), `pciworld_sources` +
`_article_sources` (claim-level provenance with tier and retrieval date), `pciworld_entities` +
`_entity_mentions` (logo permission defaults to *no*), `pciworld_article_reviews` (append-only
evidence: who checked what, when, and what they concluded).

**Rendering safety:** the body is a deliberately small Markdown subset, escaped **first** and then
decorated from a fixed vocabulary. A hostile author gets escaped visible text, not markup —
`<script>`, `<img onerror>`, `javascript:`, `data:` and protocol-relative URLs all fail to become
links and survive as words on the page, so a reviewer can see exactly what was written.

**Public surfaces:** `/world/blog`, `/world/blog/{slug}`, `/world/news`, `/world/news/{slug}` —
paginated, breadcrumbed, with BlogPosting/NewsArticle and BreadcrumbList structured data describing
only what the visible page actually shows. Corrections render as a dated public block.

**Admin:** a new Editorial tab — list, create, draft edit, workflow transitions, review recording,
approve, publish, correct, and source capture with the claim it supports.

## 2. The first article batch

Ten original articles, published, attributed to *PCI World Editorial*:

1. Why your SPI recovers as the project ends
2. The 100% rule is not bureaucracy
3. Contingency is not a number you inherit
4. Float is an asset — decide who owns it
5. Three EAC methods and the assumption behind each
6. Read the histogram before you promise the date
7. What a good change log looks like
8. Weight before percentage
9. When the data is wrong before the project is
10. The path nobody watches

**Why these, and why they are publishable without a research desk:** each explains how a technique
behaves and where practitioners get caught by it, resting on the same body of knowledge the
challenge library is built from. There are no statistics, no research citations, no company names
and no claims about current events. Those require saved, verifiable sources — which is the
newsroom's job and its separate obligation.

Each article pairs with challenges in the library, so a reader can practise the thing they just
read about. That is the internal-linking strategy: useful to the reader first, and incidentally
what the SEO policy asks for.

## 3. Test evidence

- .NET: **760 passed / 0 failed** — 12 new editorial tests covering the gate matrix, maker-checker
  at both review and approval, the correction path (including that version 1 survives), rendering
  safety against hostile input, and structured data.
- Python integration: **1148 / 1148**.
- Playwright PCI World: **19 / 19** — including a blog journey and an axe scan of an article page.

Three defects were found by these tests and fixed rather than worked around:

1. **The link regex silently broke every internal link.** `[^\s)&quot;]` inside a character class
   excludes the individual characters `& q u o t ;` — so any URL containing an "o" or a "t" failed
   to match. `/world/archive` was rendering as literal text.
2. **An article page threw** when rendered outside the endpoint, because reading time was passed in
   by the caller. It is computed from the version body now.
3. **A real contrast failure**: a secondary button on the dark call-to-action card was ink-on-noir.
   Caught by the new axe scan of an article page — exactly the blind spot Phase 2 widened coverage
   to close.

## 4. What is NOT done, and why

**Ninety more articles.** The programme calls for 100 in batches. Ten are published. The governance
is explicit that *publication pauses whenever human review capacity falls behind generation*, and
there is no named editorial reviewer yet (open decision 3). Generating ninety more articles into a
queue nobody can review would be exactly the "scaled thin content" the SEO policy prohibits, and
would make the count a claim rather than a fact. The platform is ready; the batches follow the
reviewers.

**The newsroom has no items.** The CMS supports `kind=news` fully, and the source-provenance
requirement is enforced and tested. But a news item asserts things about the world, and every
material claim must trace to a saved, reachable, dated source that someone actually read. That is
research, not generation — Phase 5 — and inventing sources to fill a newsroom is the single most
damaging thing this programme could do. Zero news items is the honest state.

**Still open:** share-asset images (Phase 7, with the rasteriser decision), admin TOTP, the full
SEO layer including sitemaps and hreflang.

## 5. Open decisions

1. **Managed MySQL 8 provider + credentials** — the launch gate.
2. **Named editorial authors/reviewers** — now blocking article batches 2–5, not just desirable.
3. Institute URL mapping for contextual links.
4. Company-logo permissions (default: none, enforced in the schema).
5. Arabic review capacity before the localization phase exits.
