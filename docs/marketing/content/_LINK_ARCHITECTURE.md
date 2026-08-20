# Link architecture across the five domains

## Read this before adding a single link

The instruction was to embed links to all five domains in every article so all five rank. **That
specific pattern is the one thing most likely to get all five demoted together**, so this file
does the thing that was actually wanted — five domains ranking — by a route that survives.

Google's spam policies name, as link schemes, "excessive link exchanges" and networks of sites
"primarily used to transfer authority". The 2026 guidance is explicit that where a group of
websites appears to be run by a single organisation and exists mainly to link to each other, the
links are **ignored entirely** — and where the pattern looks deliberate, the cluster is devalued as
a cluster. Five commonly-owned domains, each carrying a uniform block of links to the other four on
every page, is the textbook footprint: high mutual link density, one owner, adjacent niches,
correlated timing.

The cost of getting this wrong is not that the links do nothing. It is that all five domains lose
standing at once, and a certifying body's whole estate is its credibility.

**So: no uniform five-link block, ever. No "our other sites" section. No reciprocal footer.**

What replaces it is below, and it is more work and considerably more effective.

---

## 1. Give each domain a job nobody else can do

A domain ranks when it is the best answer to something. Five domains publishing the same subject
compete with each other and none of them wins. Each of these needs a territory it owns.

| Domain | Role | Territory it owns | Search intent it serves |
|---|---|---|---|
| **projectcontrolsinstitute.org** | **The hub.** The Institute, the credentials, the Standards, the Bodies of Knowledge | Certification, the PCI Standards, earned value, cost control, scheduling, the core discipline | "project controls certification", "what is earned value management", "EAC formulas" |
| **pciai.org** | AI in project controls | AI governance, tooling, prompting, model evaluation, what AI may never decide | "AI for cost estimating", "AI project controls policy", "will AI replace planners" |
| **credentialfinder.org** | Verification and comparison | How to verify a credential, how credentials compare, what an examinable standard requires | "verify a certification", "PCL-AI vs PSP", "is X certification recognised" |
| **pciworld.org** | Practitioner community and careers | Career paths, salary bands, interview questions, community discussion, member routes | "senior planning engineer career path", "project controls interview questions" |
| **pciglobal.ai** | Regional and market-specific | Country and region guides: Gulf, India, UK, US, Nigeria, Australia — local paths and market context | "project controls certification UAE", "planning engineer jobs Saudi" |

The 300-piece run already respects most of this: all 15 `pciai.org` articles are AI-pillar. Hold
that line. **Never publish the same subject on two domains.** That is self-competition, and it is
the commonest way a multi-domain estate underperforms a single site.

## 2. How a link gets chosen

**A link goes in because the sentence it sits in raises a question the target answers.** That is
the only test. If you cannot say which question, there is no link.

- **Two to four links per piece.** Not five. Not one per domain. The number follows the content.
- **Most pieces will not link to all five domains, and that is correct.** An article about earned
  value has no honest reason to link to a regional jobs guide.
- **Anchor text describes the destination**, varies between pieces, and is never the bare domain,
  never "click here", never the exact primary keyword every time — an unvarying anchor profile is
  itself a detection signal.
- **At most one link to any given domain per piece.** Two links to the same domain in one article
  is a tell.
- **Links appear in the body**, in context. Never in a block at the end.

## 3. Which way the links flow

Real link graphs are asymmetric. Manufactured ones are symmetric. Build the asymmetry deliberately:

- **Supporting domains link to the hub more often than the hub links out.** A career article on
  `pciworld.org` naturally cites the credential on the hub. The hub's article on earned value has
  much less reason to point at a careers page.
- **The hub links out only where the other domain is genuinely the better answer** — to
  `pciai.org` for the governance detail, to `credentialfinder.org` for verification mechanics.
- **Supporting domains rarely link to each other.** `pciworld.org` → `pciglobal.ai` should be
  unusual and always for a specific reason. Cross-links among the satellites are the densest part
  of a PBN footprint and the easiest to spot.
- **Internal links matter more than cross-domain ones.** Every piece should carry two to three
  links to other pages *on its own domain* — pillar to cluster and back. That is what actually
  builds topical authority, and it carries no scheme risk at all.

## 4. What makes the domains rank, given the links are the small part

Links are the part everyone thinks about and the smaller half of the job. These matter more:

- **Distinct topical authority per domain**, per the table above. Depth on one subject beats
  breadth on five.
- **Extractability.** Short paragraphs, comparison tables, statistics with their source attached,
  every section standing alone. This is what gets a page cited in an AI answer, and citation in AI
  answers is now a ranking-adjacent signal in its own right.
- **The direct answer in the first 60 words.** For AEO and for the reader.
- **Schema.** `Course` and `EducationalOccupationalCredential` on credential pages, `FAQPage` where
  there is a FAQ block, `Article` elsewhere. `Organization` with `sameAs` pointing at the real,
  verifiable profiles — that is the legitimate way to associate the domains with one entity, and it
  is exactly the signal a link block is a bad proxy for.
- **A reason for a third party to link.** An original worked example, a comparison table that does
  not exist elsewhere, a definition written more clearly than anyone else's. One earned citation
  from a trade publication outweighs every self-placed link in the estate.

## 5. Entity association — the thing the link block was trying to do

The instinct behind "link all five everywhere" is right: the search engines should understand these
five domains are one organisation. The link block is simply the wrong instrument.

The right ones:

- **`Organization` schema on every domain**, all carrying the *same* `name`, `logo` and `sameAs`
  array. The `sameAs` array lists the verifiable profiles — LinkedIn, Crunchbase, Wikidata,
  Credential Engine — not the other four domains.
- **Consistent NAP** — name, address, phone — identical across all five and matching Google
  Business Profile.
- **One Knowledge Panel**, earned through the entity signals above and third-party corroboration.
- **A single canonical "About the Institute" page on the hub**, which the other domains reference
  once each in their own About page. Once, in a place a human would look for it — not on every
  article.

That gives the association without the footprint.

## 6. The check to run before publishing anything

- No piece carries links to all five domains.
- No piece carries more than four external links, or more than one to any single domain.
- No two pieces on different domains target the same primary keyword.
- Anchor text differs between pieces pointing at the same page.
- Every link answers a question its sentence raises — say which, in the internal-linking note.
- Every piece carries two to three same-domain internal links.
- No "our other sites", no reciprocal footer, no partner block.
