# Remediation task — read this in full before editing anything

You are working in the PCI marketing content run at `/home/user/PCI/docs/marketing/content`.

## Read first, in full. These are binding.

- `_LINK_ARCHITECTURE.md` — how links are chosen, how many, which way they flow
- `_BRIEF.md` — claims register, SEO/AEO/GEO/AIO rules, voice, banned phrasing

## The five domains and the territory each owns

| Domain | Owns |
|---|---|
| `projectcontrolsinstitute.org` | **The hub.** The Institute, the credentials, the PCI Standards, earned value, cost control, scheduling |
| `pciai.org` | AI in project controls — governance, tooling, model evaluation, what AI may never decide |
| `credentialfinder.org` | Verification and comparison — how to verify, how credentials compare, what an examinable standard requires |
| `pciworld.org` | Careers and community — career paths, salary bands, interview questions |
| `pciglobal.ai` | Regional — country and region guides: Gulf, India, UK, US, and local market context |

## Absolute rules on links. Breaking any of these is worse than doing nothing.

1. **Never link to all five domains from one piece.** That pattern is the private-blog-network
   footprint Google's link spam policy names, and it gets all five domains devalued together.
2. **At most one link to any given cross-estate domain per piece.** Two is a tell.
3. **Two to three cross-estate links maximum.** Most pieces should have one or two.
4. A piece published on one of the five domains additionally carries **two to three internal links
   to other pages on its own domain**. Those are not cross-estate and do not count against the cap.
   They build topical authority and carry no scheme risk. The piece's own domain is named in its
   front matter `platform` field as `Own site — <domain>`.
5. **A link goes in only because the sentence it sits in raises a question the target answers.**
   If you cannot name that question, there is no link. Never add a link to hit a number.
6. **Anchor text describes the destination**, differs between pieces, is never the bare domain,
   never "click here", never "read more", and is not the exact primary keyword every time.
7. **Links go in the body, inside sentences.** Never a block at the end, never a list of links,
   never an "our other sites" section, never a reciprocal footer.
8. Supporting domains link to the hub more often than the hub links out. Satellite-to-satellite
   links (`pciworld.org` → `pciglobal.ai` and the like) are rare and always for a stated reason.

## Only link to pages that exist

`_tools/valid_urls.json` is the authoritative list of every URL the estate can serve: the 100
own-site pages authored in this run, every real page on the live hub in both its `.html` and
extensionless form, and the server-rendered `/certifications/{slug}` pages. **Every link you write
must appear in that list.** `_tools/url_map.json` gives the 100 run pages with their host, slug,
title, primary keyword and pillar, so you can choose a target on subject rather than on guesswork.

Never invent a slug. A broken link in a published article is worse than no link.

### Known-bad URLs and their corrections

These already appear in some files and are wrong. Fix them wherever you meet them:

| Broken | Use instead | Why |
|---|---|---|
| `credentialfinder.org/how-to-verify-a-certification` | `https://projectcontrolsinstitute.org/verify.html` | No such page was authored; the hub's verification page is real |
| `credentialfinder.org/verify` | `https://projectcontrolsinstitute.org/verify.html` | same |
| `pciai.org/ai-governance-in-project-controls` | `https://pciai.org/ai-policy-for-project-controls` | That is the AI-governance page that exists |
| `pciglobal.ai/cost-control-in-construction` | `https://projectcontrolsinstitute.org/cost-control-in-construction` | Cost control is hub territory, not regional |
| `projectcontrolsinstitute.org/what-is-an-epc-contract` | *delete the link* | No EPC page exists in the run or on the hub. Keep the sentence, drop the link |
| `pciglobal.ai/project-controls-courses-in-dubai` | `https://pciglobal.ai/project-controls-courses-dubai` | Typo — the real slug has no "in" |
| `pciglobal.ai/primavera-p6-course-dubai` | `https://pciglobal.ai/primavera-p6-course-in-dubai` | Typo the other way — the real slug has "in" |

## Claims — absolute, from `_BRIEF.md` section 3

- **15,613** machine calculation checks may only appear in a sentence that also says it covers
  **PFL-AI and PML-AI only**. Unscoped, the figure is false.
- **40/40/20** describes the *Body of Knowledge's* proportions, never an examination weighting.
- No examination weighting, worked-example counts, question counts, student numbers, pass rates or
  salary uplift may be published.
- No accreditation, recognition, endorsement, affiliation or partnership may be implied.
- Never invent a statistic, testimonial, case study or source.
- Do not reproduce protected text from ISO, IFRS, IAS, PMI or AACE. Name them, describe them in
  PCI's own words.

## Voice

British English, short sentences, practitioner register. These must not appear: "delve",
"landscape" as a metaphor, "leverage" as a verb, "unlock", "seamless", "game-changer", "it's
important to note", "navigate the complexities", "testament to", "in today's fast-paced world",
"robust solution", "tapestry".

## What the job codes in `_tools/worklist.json` mean

**EMBED** — The piece carries a linking note at the end telling a publisher which links to add, and
the links were never put into the article. **This is the main job of this run.** Read the note, then
place those links *inside the prose* — in the sentence that raises the question each answers. Never
paste them as a block. Where the note proposes links that break the rules above (three links to one
domain, all five domains, an anchor already used elsewhere), follow the rules, not the note, and
drop or retarget the surplus. Then rewrite the trailing note so it records what is now in the body
and why, plus any genuine reciprocal link another piece should make back to this one.

**RATIONALISE** — Too many links, several to one domain, or all five linked. Cut to the cap. Keep
the one in the strongest sentence; where two point at the same page, keep the better-placed one and
delete the other rather than retargeting it to something irrelevant.

**INTERNAL** — Short of same-domain internal links. Add two or three to other pages on its own host
from `url_map.json`, each in a sentence that genuinely raises what that page answers. Prefer the
pillar page for its pillar plus one or two siblings.

**ANCHOR** — Replace a weak anchor with one that describes the destination.

**QUALITY** — A mechanical SEO/AEO/GEO miss found by `_tools/quality_check.py`. Fix it in the prose.

- The commonest and most valuable is *"primary_kw not in first 60 words"*: AEO wants the title's
  question **answered**, using the primary keyword, inside the first 40–60 words after the H1,
  before any preamble. Rewrite the opening so the answer comes first and the keyword sits in it
  naturally. Never keyword-stuff — if the keyword cannot sit in an honest opening, leave it and say
  so in your report.
- *"no FAQ block"* — add 4–6 real questions with 40–80 word answers.
- *"no table"* — add a comparison table only where the axes genuinely exist. Never invent data.
- Title or meta out of range — rewrite to 50–60 and 140–158 characters.
- **Some QUALITY flags are false positives.** A definition-style opening that answers the question
  perfectly without containing the literal keyword string is *correct*: leave it and record the
  call. The check is a string match; you are the judgement.

## How to work

Use Read and Edit. Preserve front matter, structure and voice. These are good pieces — the work is
surgical. Change the least that does the job. Do not run any build or git command.

## What to write when you are done

Write `_tools/reports/remediate-<N>.json` for your batch number N:

```json
{"batch": N,
 "files": [{"file": "...", "edited": true,
            "links_placed": [{"url": "...", "anchor": "...", "kind": "internal|cross-estate",
                              "question_answered": "the question the sentence raises"}],
            "quality_fixes": ["..."], "summary": "...", "left_undone": "..."}]}
```

Write that file even if some pieces defeated you, and especially then.
