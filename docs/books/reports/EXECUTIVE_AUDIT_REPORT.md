# Executive Audit Report — PCI AI Certification BoK Suite

**Programme:** PCI AI Certification BoK Suite — audit, verification, law framework, renaming, indexing
and publication  
**Report date:** 2026-08-03  
**Volumes:** PCL-AI (PCI AI Project Controls Leader) · PFL-AI (PCI AI Project Finance Leader) ·
PML-AI (PCI Project Management Leader – AI)

---

## 1. The headline

The three volumes are, on their technical substance, in far better condition than their presentation
suggested. Across roughly 145 independently recomputed results — spanning earned value, IFRS 15
percentage-of-completion, CPM and crashing, PERT, Monte Carlo and merge bias, time value of money,
debt sizing and sculpting, coverage ratios and drawdown — **one genuine arithmetic defect was found**,
in a PFL-AI drawdown example whose printed addends did not sum to their printed total. That is an
unusually low defect rate for 1.1 million words of quantitative reference material, and it
substantiates the programme's machine-verification claim.

What blocked publication was not the mathematics. It was that all three volumes were **build
artifacts wearing the clothes of finished books**: internal production metadata printed on all 45
domain title pages, planning labels in reader-facing headings and running heads, repository paths in
the appendices, two stub appendices promising content that did not exist, and — in the two draft
volumes — an answer-key marker that silently vanished in print, deleting the correct answer from all
813 sample MCQs.

All of that is now fixed. The volumes rebuild clean, carry a coherent PCI Professional Law framework,
and the retired PCP-AI identity is gone from published content. **What remains outstanding is the one
thing no amount of further automated work can supply: named human subject-matter review.**

## 2. Publication readiness

| | PCL-AI | PFL-AI | PML-AI |
|---|---|---|---|
| Pages (rebuilt) | 947 | 802 | 822 |
| Domains / Knowledge Areas | 13 / 61 | 16 / 61 | 16 / 63 |
| Worked examples | 146 | 179 | 161 |
| Sample MCQs | 309 | 450 | 363 |
| Case studies | 26 | 33 | 33 |
| Figures | 33 | 46 | 51 |
| Machine-verified calculations | **none — no suite exists** | 7,935 | 7,678 |
| Professional Laws (foundational + certification) | 14 + 20 | 14 + 24 | 14 + 24 |
| **Readiness** | **Amber** — releasable as study material once reviewed | **Amber** | **Amber** |

No volume is Green, and none should be described as release-ready, for one reason stated plainly in
§5. No volume is Red: there is no longer a known defect that would mislead a candidate.

## 3. Critical findings, and their disposition

| # | Finding | Volumes | Status |
|---|---|---|---|
| C-1 | Internal production spec blocks ("Target: ~68 pages. Binds to: …") printed on every domain title page — several containing impossible ordinals ("Domain 7 of 6") | all three (45 domains) | **Fixed** — stripped from source, preserved separately for notation reinstatement |
| C-2 | MCQ correct-answer markers rendered as blank space: the emoji glyph exists in no build-host font, so the answer key was invisible in print for all 813 MCQs | PFL, PML | **Fixed** — replaced with a font-safe boxed "✓ CORRECT" chip; verified 450 and 363 render |
| C-3 | Appendix E (self-check answers) and Appendix F (MCQ bank) were stubs promising content the book did not contain, in a volume presented as a First Edition | PCL | **Fixed** — 129 self-check answers and all 309 MCQs extracted into real appendices |
| C-4 | Volume presented as a finished First Edition with no AI-drafting disclosure, while its sibling volumes carried one | PCL | **Fixed** — production, verification, laws, errata and release-approval front matter added |
| C-5 | Retired credential identity (PCP-AI / Certified Project Controls Professional) live throughout the volume the platform had already migrated away from | PCL + platform | **Fixed** — 392 replacements across 87 files; zero unauthorised residuals |
| C-6 | Two governing principles in circulation across one credential family | all three | **Fixed** — one wording everywhere; legacy wording retired with a recorded note |
| C-7 | The PCL law set was severed from the foundational hierarchy it is declared subordinate to — zero cross-references where its siblings had 39 and 55 | laws | **Fixed** — 41 cross-references added |
| C-8 | An implied ISO/IEC 17024 accreditation claim, and voluntary/supervisory instruments (Basel, COSO, Scrum Guide, FAST, IESBA, OECD) characterised so they could read as binding | laws | **Fixed** — every instrument verified with its publisher; non-binding status stated at point of use |

## 4. Major findings

- **A symbol collision that would mislead a candidate.** `EAC` meant *equivalent annual cost* in
  PFL-AI Domain 4 and *estimate at completion* in Domain 8 — two different quantities, in different
  units of meaning, under one symbol, in one book. Domain 4 now uses `EAV`, matching the shared
  registry. No arithmetic changed.
- **A verification record that recorded a verification that could not have happened.** A PCL-AI
  toolkit logged an AI-drafted EAC of 1,180,000 as "recomputed and verified", but no method on that
  data yields it. The row now shows the check *catching* the discrepancy and returning the figure
  unreleased — which is what the control is for, and a better lesson than the original.
- **A figure illustrating a different data set from the text it sat beside** (PCL-AI Fig 10.3.1,
  crash costs). Re-specified to the worked examples' own numbers.
- **Date-sensitive standards taught as current.** IFRS 18 replaces IAS 1 for periods from 1 January
  2027 — under six months out — and ISO/IEC 27701:2025 became standalone rather than an extension.
  Forward notes added; the register now carries verification dates.
- **Register defects.** The external-authority register tagged the IFRS *Conceptual Framework* as an
  accounting standard (the IASB states expressly that it is not one) and recorded that Basel appeared
  in no law file when it is cited six times. Both corrected; eleven instruments the laws cite that no
  register carried are now registered.

## 5. What is NOT done, and why it matters most

**No named human subject-matter expert has reviewed any of this material.** The corpus was AI-drafted
end to end. Everything this programme added — 82 Professional Laws, the registers, the appendices, the
corrections — was also AI-produced, reviewed by independent adversarial agents rather than by people.

A passing verification suite and a clean red-team pass establish something real but narrow: the
arithmetic is right, the citations resolve to real instruments, the structure conforms, and no
voluntary framework is described as legislation. **They do not establish that the pedagogy is sound,
that the professional judgements are ones an experienced practitioner would endorse, that the emphasis
is right, or that nothing important is missing.**

The evidence for that limit is in this programme's own results: of the eight calculation-assurance
defects found, **four were not arithmetic errors at all** — they were interpretation, precision,
illustration and record-keeping defects that a machine suite passed and a human reading caught. That
ratio is the argument for Gate 13.

Nothing in these volumes should be presented to a candidate, an employer, a regulator or an
accreditation body as reviewed material until that review is recorded.

## 6. Gate status

| Gate | Status | Note |
|---|---|---|
| 1 — Inventory complete | **Pass** | Scripted inventory across all 45 domains; see §2 |
| 2 — Naming complete | **Pass** | Zero unauthorised retired-name occurrences; see the Naming Migration Report |
| 3 — Technical review complete | **FAIL — not started** | Requires named subject reviewers |
| 4 — Calculation verification | **Partial** | 15,613 checks passing for PFL + PML; **no suite exists for PCL-AI** |
| 5 — Laws framework complete | **Pass, with gaps recorded** | 82 laws; seven domains carry no anchored law (listed in the issue register) |
| 6 — External references verified | **Pass** | 70 registered + 11 added; 41 independently verified with publishers |
| 7 — Cross-book consistency | **Pass** | One principle, one credential-name set; 8 legitimate term collisions documented, not collapsed |
| 8 — Examination review complete | **Partial** | Structural checks pass and answer keys now print; no examiner has reviewed the items |
| 9 — Copyright and originality | **Pass on evidence available** | No standard text reproduced; all cases fictitious; no third-party diagram used |
| 10 — Editorial and accessibility | **Partial** | Call-out system never relies on colour alone; no accessibility audit of the rendered PDFs |
| 11 — Indexing complete | **Partial** | Subject, figure, formula, glossary, standards, toolkit, capstone and laws indexes present; no consolidated cross-book concept index |
| 12 — Independent red-team | **Pass** | Two adversarial passes on the laws; three on the volumes |
| 13 — Final human approval | **FAIL — not started** | The gate that matters, and the one that cannot be delegated |

## 7. Recommended release decision

**Do not release as a professional standard. Release is defensible only as clearly-labelled study
material, and only after Gate 3.**

In priority order:

1. **Appoint named technical reviewers** — one per volume, plus a finance reviewer for PFL-AI and an
   examiner for the MCQ banks. This is the binding constraint on everything else.
2. **Build the golden-answer suite for PCL-AI.** It is the only volume whose numbers rest on sampling,
   and it is the volume currently presented as a First Edition.
3. **Decide the page-length question.** Both draft volumes sit ~800 pages against a 1,200-page target.
   The programme's own analysis is that the gap cannot be closed honestly within the current syllabus;
   the three defensible answers (extend the syllabus, split into core and advanced volumes, or revise
   the target) are a scheme-owner decision, not an editorial one.
4. **Decide the website question.** The retired governing principle remains on roughly 220 live public
   pages (306 occurrences), including SEO structured data where it appears as a registered slogan.
   The books and the site would otherwise state different governing principles for the same credential
   family. This was deliberately left unchanged — rewriting the public marketing estate is a
   publishing decision, not an audit correction — but it should not stay open long.
5. **Close the seven unanchored domains** in the law framework, most notably PCL-AI Domain 2, the
   IFRS 15 flagship, which carries no law at all.
