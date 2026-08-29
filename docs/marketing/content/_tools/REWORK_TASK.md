# Rework task — acting on the judges' findings

A judge read your batch and failed some files. Fix every item in every `must_fix` list on every file
whose `verdict` is `REWORK`, in `_tools/reports/judge-<N>.json`.

Read `_tools/REMEDIATION_TASK.md` first: the link rules, the claims register and the voice
constraints in it are unchanged and still binding.

The judge has already read these files. Act on what it found rather than re-litigating whether it
was right — **except** where an instruction would break a rule below, in which case follow the rule
and record the refusal in your report.

## Where the judges were wrong, and you should not follow them

Four findings recur across reports and are mistaken. Do not act on them.

**1. Same-domain link counts on off-site pieces.** A judge argued that pieces on DEV Community and
Hashnode were stripped of internal links they should keep, comparing them to own-site articles that
carry three hub links. That comparison does not hold. An own-site hub article linking to
`projectcontrolsinstitute.org` is making an *internal* link, which `_LINK_ARCHITECTURE.md` §3
requires two to three of. A DEV or Hashnode republication has no own domain, so the same URL is a
*cross-estate* link and the one-per-domain cap in §2 applies. `link_audit.py` makes exactly this
distinction and is right to. **Do not add hub links back to off-site pieces.**

**2. Schema type.** Several judges noted that files declare `Article` while carrying a FAQ block,
against §4's "`FAQPage` where there is a FAQ block". This is a run-wide convention question, not a
per-file defect, and it is being settled separately by a mechanical pass. Ignore any `must_fix`
about the `schema:` field.

**3. Front-matter `word_count` drift.** Counts understate the prose by 100–300 words in many files
because the remediation pass added sentences. That is real and is being fixed mechanically. Ignore
any `must_fix` about it.

**4. Already fixed, centrally.** Do not redo these; they are done and re-editing risks undoing them:
the CPI error in `pbc-today-article.md`; the 40/40/20-as-exam-weighting sentences in articles 010,
011 and 149; the credentialfinder.org ownership disclosure and the four "beats any provider's own
page" sentences.

## What to prioritise, in this order

**Claims first.** A wrong claim costs the Institute its standing; everything else costs ranking.
Where a judge scored `claims` below 8, fix that before anything else in the file. The recurring
kinds: an unsourced ranking or statistic presented as fact ("usually about twice the size",
"named the number-one driver in contractor surveys" with no survey named); an examination weighting
stated or implied, including "weighted towards delivery, contract and governance" and any phrasing
where 40/40/20 hangs off the verb "examines" or supports a claim about passing; a standard's own
wording reproduced rather than described in PCI's words (IAS 16, IAS 23, IAS 37 and IFRS 15 are the
ones judges caught); a price or fee PCI has not published.

**Then arithmetic inside worked examples.** These are the findings most likely to be cited and be
wrong, and no checker sees them because each number is individually well-formed. Judges found: a
recovery schedule promising 23 days its own float table caps at 20; an F1 score described as
under-measurement when F1 is symmetric between over- and under-measurement; a constraint said to
collapse float when it is set at the date already calculated, so it changes nothing; a funding
window stated as months three to six when the table shows cash negative from period one; a printed
sum that does not reconcile with its own printed inputs. **Recompute the example. If the prose and
the table disagree, the table usually holds and the prose is the edit.**

**Then the notes that instruct rather than record.** A trailing note telling a publisher to drop two
more hub URLs "in the comments" is a live instruction someone will follow, and three links to one
domain is the density the architecture forbids whether they sit in the body or under it. Rewrite
those notes to record what is in the piece. Where the same instruction and the same anchor pair
appear across several platform assets, vary them or drop them: four platforms pointing at two hub
pages with identical anchors is a correlated footprint.

Also drop instructions to build reciprocal links — a PCI domain linking back to a LinkedIn article,
or a satellite linking back to another satellite because that satellite linked to it. Manufactured
symmetry is the specific pattern §3 exists to prevent.

**Then near-duplicate content on one domain.** Judges found the same EAC table with identical "what
it assumes" cells across four `pciworld.org` pages, and the same cash-conversion-cycle example on
two `pciglobal.ai` pages that link to each other. Two pages on one domain competing on one subject
is self-cannibalisation. Differentiate them: keep the full treatment on the page whose primary
keyword owns the subject, and cut the other to a short statement that links to it.

**Then everything else** in the `must_fix` lists.

## What not to do

Do not rewrite whole pieces. These are good articles and most findings are one or two sentences.
Do not add a link to hit a number. Do not invent a statistic, a source or a case study to satisfy a
judge who wanted one — if a claim cannot be sourced, cut the sentence, which is what the brief says
and what the judge should have said. Do not run any build or git command.

## What to write

`_tools/reports/rework-<N>.json`:

```json
{"batch": N,
 "files": [{"file": "...", "fixed": ["..."], "refused": ["... and why"],
            "not_a_defect": ["... the judge's finding you checked and disagreed with, with evidence"]}]}
```

Use `not_a_defect` properly. The judges were told to be hard and several said in their own reports
which calls they doubted. If you check one and it does not hold, say so with the evidence rather
than making a change to satisfy it — a wrong edit made to close a finding is worse than the finding.
Then reply with three lines: files changed, the most serious thing you fixed, and anything you
refused.
