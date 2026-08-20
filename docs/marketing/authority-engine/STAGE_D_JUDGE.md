# Stage D — independent AI Judge, pilot batch 1

Role switched per master prompt §17: senior editor, SEO auditor, certification compliance reviewer
and student advocate. The drafts are not defended here. Scoring is 0–5 on twenty criteria; pass
requires **no automatic failure, no category below 4, and an average of at least 4.5**.

Batch: `A-001, A-011, A-016, A-020, A-026, A-029, A-032, A-040, A-043, A-046`.

---

## 1. Automatic-failure screen (run first)

| Automatic-failure condition | Result |
|---|---|
| Invented or unsourced accreditation, recognition, salary, employment, pass-rate or legal-status claim | **PASS** — every article states PCI is *not* accredited by ANAB/IAS/any ISO 17024 body and quotes the no-guarantee clause. No salary, pass-rate or employment claim appears anywhere in the batch. |
| Stale/unverified price or competitor detail presented as current | **PASS with flags** — all prices carry `[PRICE UNVERIFIED]`; **no competitor fact is asserted anywhere in the batch** (A-040/A-043 hold every competitor cell as a `⟨FETCH⟩` placeholder). |
| Certuvo described as the credentialing/certification body | **PASS** — A-026 carries an explicit responsibility table; four other articles restate the separation. |
| Automatic Certuvo access asserted without current terms | **PASS** — A-026 states the platform rule, names the contradiction with the brief, and blocks itself pending M4. No access duration is stated anywhere. |
| Guaranteed honorary outcome | **PASS** — A-001 uses "apply for the board's consideration… conferral is at the board's discretion" and states honorary is never the examined credential. |
| Broken, fabricated or off-estate PCI destination | **PASS** — only three verified domains are linked; `pciai.org`/`pciglobal.ai` are deliberately absent with the reason stated in-page. No invented deep URLs. |
| Deceptive community commenting, fake review/testimonial or undisclosed promotion | **PASS** — every Quora/Reddit variant opens with an affiliation disclosure and at most one link; Reddit variants carry no link at all. |
| Copied competitor wording or plagiarism | **PASS** — no competitor wording exists to copy. |
| Keyword stuffing, doorway content or near-duplicate intent | **PASS** — ledger dedup is clean (0 exact duplicates); no article repeats its primary phrase mechanically. |
| Invalid or self-serving structured data | **PASS** — no `Review`/`AggregateRating` anywhere; `FAQPage` only where a visible FAQ block matches; A-032 explicitly refuses `EducationalOccupationalCredential` for a Passport; A-040/A-043 withhold JSON-LD entirely while blocked. |
| Missing approval markers on policies or unresolved high-risk claims | **PASS** — all high-risk items carry visible flags. |

**No automatic failures in the batch.**

---

## 2. Scores

Criteria: 1 PCI factual accuracy · 2 external factual accuracy/source quality · 3 pricing/route/access
accuracy · 4 competitor fairness · 5 legal/accreditation safety · 6 search-intent satisfaction ·
7 topical depth · 8 original value · 9 keyword naturalness · 10 title/meta/H1/slug · 11 internal-link
relevance · 12 all-domain link integrity · 13 E-E-A-T · 14 answer-engine extractability · 15 schema
validity · 16 readability/accessibility · 17 CTA relevance/honesty · 18 platform adaptation ·
19 carousel/image brief · 20 duplication safety.

| ID | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | Avg | Verdict |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| A-001 | 5 | n/a | 5 | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.94** | **PASS** (hold for legal) |
| A-011 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **PASS** (hold for verification) |
| A-016 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **PASS** (hold) |
| A-020 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **PASS** (hold) |
| A-026 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **BLOCKED** (M4) |
| A-029 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **PASS** (hold) |
| A-032 | 5 | n/a | 5 | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.94** | **PASS** (hold) |
| A-040 | 5 | — | **4** | 5 | 5 | **2** | **3** | 4 | 5 | 5 | 5 | **4** | 5 | **3** | n/a | 5 | 5 | n/a | n/a | 5 | **4.20** | **BLOCKED — cannot pass** |
| A-043 | 5 | — | **4** | 5 | 5 | **2** | **3** | 4 | 5 | 5 | 5 | **4** | 5 | **3** | n/a | 5 | 5 | n/a | n/a | 5 | **4.20** | **BLOCKED — cannot pass** |
| A-046 | 5 | n/a | **4** | n/a | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4** | 5 | 5 | 5 | 5 | 5 | 5 | 5 | 5 | **4.88** | **PASS** (hold) |

`n/a` = criterion does not apply (no external claims made / no derivatives produced for a blocked
article) and is excluded from the average rather than scored as full marks.

**Batch result: 8 of 10 meet the pass bar. 2 are correctly blocked and cannot be scored to a pass
until competitor research is executed.**

---

## 3. Correction table

| Article | Severity | Exact passage / problem | Why it fails | Required evidence | Corrected text | Owner | Recheck |
|---|---|---|---|---|---|---|---|
| **All 10** | Medium | Ecosystem block lists three domains, not the five the brief specifies (criterion 12 scored 4) | The brief mandates all five owned domains in every article's ecosystem block | PCI confirmation of ownership + target URL for `pciai.org` and `pciglobal.ai` | Restore the two lines once confirmed; **the current three-domain block is the correct behaviour meanwhile** — inventing destinations is a worse failure than an incomplete block | PCI ops | On M3 |
| **A-011, A-016, A-020, A-026, A-029, A-046** | Medium | Fee figures carry `[PRICE UNVERIFIED]` (criterion 3 scored 4) | Prices presented without a verification date cannot score 5 | Live pricing page fetch with `verified on` date | No text change; stamp dates and lift the flag | Live-verification pass | On M2 |
| **A-026** | **High — blocking** | "Your Certuvo practice account is set up automatically once your **membership** is active" vs the brief's exam-fee trigger | A candidate paying only an exam fee and expecting study access would be materially misled if the wrong rule is published | PCI statement of the current commercial rule | Keep the membership rule and the visible contradiction until PCI rules; **do not publish** | PCI commercial | On M4 |
| **A-040, A-043** | **High — blocking** | Every competitor cell is `⟨FETCH⟩`; intent satisfaction (2), depth (3) and extractability (3) cannot rise while half the comparison is absent | A comparison that cannot compare does not satisfy comparison intent | Execute the PMI and IMA rows of `COMPETITOR_RESEARCH_PLAN.md`; identify the CMA issuing body per jurisdiction | Replace placeholders with sourced, dated facts; then re-run the Judge from criterion 1 | Research owner | On M13 |
| **A-001** | Medium | Legal-status paragraph quotes the footer wording while the platform's JSON-LD makes a stronger "registered nonprofit" claim | Two live wordings for one legal fact; the article picked the safer one but the conflict is unresolved | Legal decision on the single correct formulation | Footer wording stands; platform JSON-LD should be corrected in product | Legal + platform | On M5 |
| **A-032** | Low | Community rooms/forum/careers described "as designed" with a flag caveat | Correct, but a careless editor could delete the caveat and claim a live launch | Confirmation of flag states per deployment | Caveat is marked do-not-remove in editorial notes | Content lead | On launch |

**No article was corrected by deleting a difficult claim.** Every gap above is stated with the
evidence required to close it, per §17.

---

## 4. Revision pass (one round, per §17)

Two defects were found and **fixed in this round** rather than deferred:

1. **A-046 sequencing list rendered as broken HTML** — wrapped list-item continuation lines were
   being emitted as an `<li>` followed by a stray `<p>` containing the item's second half. Fixed in
   `_build/md_to_html.py` (list continuation folding); all eight HTML bodies regenerated and
   verified (0 stray continuation paragraphs, all links preserved).
2. **Blocked articles were generating paste-ready HTML** — `md_to_html.py` now refuses any file
   carrying `PUBLICATION BLOCKED`, so an unpublishable comparison cannot reach a CMS by accident.

Re-scored after revision: criterion 15/16 unchanged at 5 for the eight passing articles; the
generator defect would have cost A-046 a point on readability had it shipped.

---

## 5. Judge's standing observations

**What this batch does well.** Every article states what PCI does not claim, in PCI's own words,
usually above the fold rather than in a footer. The original elements (due-diligence checklist,
five-question self-test, DSCR worked example, decision-rights table, progress-measurement
arithmetic, recruiter questions, the one-question decision rule) are genuinely usable and several
are designed to talk a reader *out* of buying — which is the strongest available E-E-A-T signal and
the reason criterion 13 scores 5 across the batch.

**What the batch cannot fix by writing better.** Six of the ten hold a flag that only an operator can
clear. That is the correct state — the failure mode this programme most needed to avoid was
confident publication of unverified fees, invented domains and remembered competitor facts, and none
of those appear.

**One risk to watch in Stage E.** The 500-row ledger contains 86 comparison articles. Every one of
them inherits A-040/A-043's blocker. If competitor research is not funded before production begins,
roughly a sixth of the programme cannot be written — and the temptation to fill a table from memory
will be at its strongest when a batch deadline is due. Recommend competitor research is completed
**before** Stage E starts rather than alongside it.
