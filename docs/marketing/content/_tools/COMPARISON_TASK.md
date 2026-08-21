# Comparison writing task — read in full before writing

You are writing comparison content for Project Controls Institute Global: pieces that set the
PCI credentials beside the established certifications a candidate would otherwise choose.

## Read these first. They are binding.

- `../_COMPARISON_FRAMEWORK.md` — the rules for this content type. **Every rule in it is
  absolute.** It exists because comparison content naming PMI, AACE, IIA, ISACA, CFA Institute
  and IMA is the highest-legal-risk category PCI publishes.
- `../_BRIEF.md` — claims register, SEO/AEO/GEO rules, voice, banned phrasing.
- `../_LINK_ARCHITECTURE.md` — link discipline.

## The two decisions already taken, which you may not vary

**1. Every comparison page discloses that PCI operates credentialfinder.org.** In the body,
early, where a reader sees it. Not in a footer, not in small print. Wording to adapt:

> credentialfinder.org is operated by Project Controls Institute Global, which awards the
> PCL-AI, PFL-AI and PML-AI credentials described here. We say what each credential examines
> and link to every body's own page so you can check it yourself.

**2. Every piece states plainly that PCI is not accredited.** Every piece, not once per
cluster, because a reader arriving from search may see only that one:

> PCI is a new, independent certifying body. It is not accredited by ANAB, UKAS, IAS or any
> other ISO/IEC 17024 accreditation body, and does not claim to be. The scheme is built with
> reference to ISO/IEC 17024 principles. Read the published Body of Knowledge before you
> decide anything.

On a short social piece where that paragraph will not fit, the shortest honest form is "PCI is
new and not accredited — read the syllabus and judge it." It still goes in.

## What you compare on, and what you must never compare on

**Compare on scope.** What each credential publicly states it examines. Which domains. Which
side of the finance-and-delivery line. What it assumes the candidate already knows.

**Never compare on:** fees · question counts · exam length · domain counts · pass marks · pass
rates · difficulty · prestige · recognition · employer preference · salary outcomes.

Those change, they come mostly from third-party study sites, and a wrong figure about a named
body is a false statement about that body. Where a reader needs a fee or an exam specific, say
so and point them at that body's own page.

**The test for every sentence about another credential:** if challenged, could you point at
that body's own published page and show them where it says that? If not, cut the sentence.

## Never disparage

Describe each external credential as its own holders would recognise. The frame is always
"here is what this examines", never "here is what this fails to do".

- **Right:** "PMI-SP examines schedule development, maintenance and control. Revenue
  recognition sits outside its scope, as it does for every scheduling credential."
- **Wrong:** "PMI-SP ignores the financial side, which is why schedulers miss cost problems."

The first is checkable and fair. The second is disparagement, and it is weaker — a reader
holding PMI-SP stops reading.

Do not reproduce protected text from any body's syllabus, exam outline or handbook. Name them,
describe them in PCI's own words, link to their page.

## Say "take the other one" where it is true

A comparison PCI always wins is an advertisement and reads as one. Where a reader's need is
squarely served by another credential — a pure IT auditor, a candidate whose employer mandates
PMP, somebody who needs an accredited credential today — say so plainly.

That single move is what makes everything else in the piece believable.

## The figures you may publish

Only these: 13/16/16 domains and 61/61/63 knowledge areas · 92 sector case studies · 113 PCI
Standards carrying 532 process requirements · 40/40/20 as the **Body of Knowledge's**
proportions and never an exam weighting · 15,613 machine calculation checks **only** in a
sentence that also says it covers PFL-AI and PML-AI, because PCL-AI has no equivalent suite.

Nothing else. No exam weightings, worked-example counts, student numbers, pass rates, salary
uplift or holder numbers, for PCI or anyone else.

## Structure of a core comparison piece

1. The direct answer in the first 60 words — which credential suits which person, plainly.
2. The ownership disclosure line.
3. A comparison table on scope axes only. Tables are the most-cited format there is.
4. What each credential examines, a short section each, in our words, linking to their page.
5. Where the overlap sits, with a worked example if it earns its place.
6. The accreditation statement.
7. How to decide — honestly including "take the other one" where that is right.
8. FAQ, 4–6 real questions, 40–80 words each.
9. The internal-linking note.

## Links

One link per cross-estate domain, two to three internal links to other credentialfinder.org
pages, never all five PCI domains in one piece. Only link to pages that exist — check
`url_map.json` and `valid_urls.json`. External links to the other bodies' own pages are
expected and correct in this content type; link to the body's own domain, never to a
third-party study site.

## Platform variants

Where your piece is a platform variant, it derives from the named core piece but is written
for its platform, not pasted. LinkedIn Articles, Substack and Quora must be **original or
substantially rewritten** — never a copy. Medium carries the canonical to the
credentialfinder.org original.

## Voice

British English. Short sentences. Practitioner register. The banned phrases in `_BRIEF.md` §7
must not appear. Write like somebody who has held one of these credentials and is telling a
colleague what it actually covers.

## Output

One markdown file per piece in `../comparisons/`, named `<slug>.md`, with the standard front
matter from `_BRIEF.md` §8 plus `compares: [list of credential acronyms]`. Then the piece.
