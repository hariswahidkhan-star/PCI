# Calculation Assurance Statement — PCI AI Certification BoK Suite

**Programme:** PCI AI Certification BoK Suite audit · **Report date:** 2026-08-03  
**Gate:** Gate 4 — Calculation verification  
**Scope:** PCL-AI (13 domains), PML-AI (16 domains), PFL-AI (16 domains)

---

## 1. What was verified, and by what method

Two independent methods were applied to the two draft volumes, and one to the PCL-AI volume.

### Method 1 — the golden-answer suite (machine, exhaustive)

`docs/books/_build/verify_formulas.py` recomputes every printed *result* from its stated inputs using
`decimal.Decimal` at 28-digit precision and compares against the printed value. It covers worked-example
Substitution and Result lines, in-text figures, **every numeric MCQ option — not only the correct one** —
exercise answers, case-study figures, and numbers inside figure specifications. Each module additionally
pins the domain's teaching **invariants** (an identity, a breakeven, a bound, an inequality), so a claim
cannot silently stop being true when a number changes.

| Volume | Checks | Status |
|---|---|---|
| PFL-AI (16 domains + cross-domain modules + capstones) | 7,935 | all passing |
| PML-AI (16 domains + capstones) | 7,678 | all passing |
| **Total across 58 check modules** | **15,613** | **all passing** |

Both loaders are fail-closed, and that is established by test rather than assumed: a module that raises
exits non-zero through either path. The suite's own machine-readable `TALLY` line is what the appendices
report, and the appendix generator refuses to write an appendix at all if the tally fails to reconcile
against the emitted PASS/FAIL lines.

One verifier established that its passes were **derivations rather than transcriptions** by perturbing
five printed values and confirming the checks then fail — the control that distinguishes a real
verification suite from one that merely restates the book.

### Method 2 — independent recomputation (human-directed, sampled)

Three independent technical reviews recomputed printed results from the rendered PDFs, without reference
to the check modules, using separate arithmetic. This is the second engine required by §9.3: it does not
inherit the first engine's formulas.

| Volume | Results independently recomputed | Substantive defects found |
|---|---|---|
| PCL-AI | ~40 worked examples, exercises, MCQs and the integrated capstone across all 13 domains | 0 numeric; 1 figure/text data-set mismatch; 1 untrue verification record |
| PML-AI | ~25 across Domains 4, 6, 7, 8, 11 — including a 32-outcome exact enumeration and a merge-bias conditional integral | 1 interpretive (total stated as incremental); 1 garbled path label; 1 false-precision figure |
| PFL-AI | ~80 across TVM, appraisal, modelling, gearing, debt sizing, coverage ratios, drawdown and the capstones | 1 (an addend cluster that did not sum to its printed total) |

## 2. Defects found and their disposition

Every defect below is corrected in source and re-verified.

| ID | Volume | Location | Defect class | Disposition |
|---|---|---|---|---|
| CA-01 | PFL-AI | Domain 14, WE 14.1.2 Substitution and the Domain 14 summary | Arithmetic — printed addends did not sum to the printed total | Both equations now carry the unrounded addends and are true as written; the derived result (9,924,564) and its 70/30 splits are unchanged, because the rounded total was correct and the *addends* were the defect |
| CA-02 | PML-AI | Domain 7, WE 7.3.3 | Incorrect interpretation — a total efficiency loss described as a further, incremental loss | Restated as 6.97 points further on top of 9.43 already lost, 16.40 % below the budgeted rate in total; the invariant 9.43 + 6.97 = 16.40 is now pinned by check |
| CA-03 | PML-AI | Domain 8, WE 8.A.2 | False precision — cent-level precision on a numerically integrated quantity | Restated as ≈ USD 5,550 with a one-clause reason; the derived value remains pinned, plus a check that it rounds to the printed figure |
| CA-04 | PML-AI | Domain 6, MCQ 6.2-C rationale | Incorrect label — critical path mis-stated (the arithmetic was right) | Corrected to A–B–D–E–F (2+6+7+5+4) |
| CA-05 | PCL-AI | Fig 10.3.1 | Incorrect chart — figure priced activities the worked examples never crash, at rates the examples contradict | Figure re-specified to the worked examples' own data (crash B at 5,000/day, then D at 8,000/day); SVG regenerated |
| CA-06 | PCL-AI | Toolkit 13.T | Incorrect interpretation — a sign-off row recorded an AI-drafted EAC of 1,180,000 as "recomputed and verified", but no method on that data yields it | The row now shows the verification **catching** the discrepancy: the AI draft is returned unreleased and the verified 1,104,167 is reported. The record is now true, and the example teaches the control working |
| CA-07 | PFL-AI | Domain 4 vs Domain 8 | Symbol collision — `EAC` used for equivalent annual value in D4 and estimate at completion in D8 | D4 now uses `EAV` throughout, matching the shared formula registry; no arithmetic changed |
| CA-08 | PFL-AI, PML-AI | Appendix D captions | Caption truncation from a build regex stopping at the first full stop | Regex fixed in the appendix generator; four captions restored |

## 3. Required certification

| Measure | Count |
|---|---|
| Calculations identified and machine-verified (PML-AI + PFL-AI) | 15,613 |
| Recomputed by the machine suite | 15,613 (100 %) |
| Independently recomputed by a second, non-inheriting method | ~145 sampled across all three volumes |
| Passing after correction | all |
| Corrected during this programme | 8 defect clusters (table above) |
| Requiring professional judgement rather than arithmetic | see §4 |
| Requiring external legal, tax or accounting input | see §4 |
| **Unresolved calculation defects** | **0** |

## 4. The limits of this statement — read before quoting §3

**Coverage is not uniform across the three volumes.** The 15,613-check figure covers PML-AI and PFL-AI.
The PCL-AI volume has **no equivalent machine suite**; its assurance rests on the independent
recomputation of roughly forty results across all thirteen domains and the capstone, which found no
substantive numeric error but is a *sample*, not exhaustive coverage. Extending the golden-answer harness
to PCL-AI is the single largest outstanding item in the calculation-assurance programme, and until it is
done the two claims should never be stated as one number.

**What a passing suite establishes is narrow.** It establishes that the arithmetic is right and that the
numbers printed are the numbers the stated methods produce. It does **not** establish that the method
chosen is the one a competent practitioner would choose, that the assumption behind an input is
reasonable, that the worked example teaches the right lesson, or that the interpretation drawn from a
correct number is sound. Four of the eight defects above (CA-02, CA-03, CA-05, CA-06) were *not*
arithmetic errors — they were interpretation, precision, illustration and record-keeping defects that a
machine suite passed and a human reviewer caught. That ratio is the argument for §5.

**Jurisdiction-sensitive figures.** Every tax rate, contract-law rule and regulatory threshold appearing
in an example is illustrative. No calculation in any volume should be relied upon for a live decision in
a specific jurisdiction without qualified local advice, and the volumes say so in their front matter.

## 5. Sign-off status

| Role | Status |
|---|---|
| Machine verification (golden-answer suite, PML-AI + PFL-AI) | **complete** — 15,613/15,613 passing |
| Independent recomputation (sampled, all three volumes) | **complete** — 8 defect clusters found, all corrected |
| Machine verification, PCL-AI | **not built** — outstanding |
| Named human calculation reviewer | **not appointed** — outstanding |

Gate 4 is **conditionally met for PML-AI and PFL-AI** and **not met for PCL-AI**. No volume has a named
human calculation reviewer, and this statement is not a substitute for one.
