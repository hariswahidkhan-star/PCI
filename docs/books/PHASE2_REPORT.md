# Phase 2 Report — Foundation Domains (running)

**Phase:** 2 of 8 · **Scope:** foundation parts of both books (PML-AI D1–D4; PFL-AI D1–D2 and
D4 — D3 delivered as the Phase 1 prototype) · **Status:** in progress under the production loop.

## Batch log

**Batch 4 (loop iteration 2) — PFL-AI Domain 4, Investment appraisal and capital budgeting.**
First Phase 2 domain authored to the family pattern (~6.2k words first pass, apparatus-complete):

- Master appraisal thread continues Kestrel Water SPC (I₀ 60m; 8.9m × 15y; 8 %): NPV +16,179,360
  · IRR 12.19 % · MIRR 9.73 % · payback 6.74 / discounted 10.07 years · PI 1.270.
- KA 4.1 NPV/IRR/MIRR with the three IRR pathologies (dual-root example verified at exactly
  10 %/20 %); KA 4.2 payback, PI, EAV (unequal-lives pump decision); KA 4.3 mutually exclusive
  choices (incremental IRR 10.42 % crossover), capital rationing (including a verified example
  where greedy PI packing loses to enumeration), limits-of-the-numbers.
- Advanced topics (profile reading, inflation-consistent appraisal, reviewer invariants),
  industry variations, two case studies (the intake decision; the fund that bought percentages),
  executive perspective, 5 exercises, 3 toolkits, exam prep, 12 tagged MCQs.
- Figures 4.1.1 (NPV profile), 4.2.1 (two paybacks), 4.3.1 (crossover) — PCI-original SVGs.
- Harness: **218 golden checks, all passing** (was 165). It caught two imprecise goldens and one
  mis-described MCQ distractor during authoring — all corrected before commit.
- PFL-AI volume now typesets at **43 pp** across Domains 3–4 (9 figures, 34 index entries).
- Formula registry: NPV, IRR/MIRR, PI, EAV, FV(x), DF(t), Fisher symbols, annuity `A`, ES,
  SPI(t), TF/FF flipped to ✅ (verified golden examples exist).

**Batch 5 (loop iteration 3) — PFL-AI Domain 1, Foundations of project finance leadership.**
The book's opening domain (~6.4k words first pass): the leader's role across the lifecycle;
the recourse spectrum (what limited recourse buys and costs); the SPV and its stakeholder
table; the infrastructure-finance market and asset-capital matching; value–cash–risk logic
with two fully worked demonstrations (profitable-but-out-of-cash: profit +2.0m vs operating
cash −1.5m; leverage's two faces: 26 %/16 %/6 %/0 % levered vs 12 %/9 %/6 %/4.2 % unlevered,
with the −65 % equity-zero cliff); the bankability triangle; ethics/fiduciary/conflicts
(daylight test) and the responsible-AI foundations. Two case studies (how Kestrel chose
project finance; the adviser with two hats), 9 tagged MCQs, 3 exercises, 3 toolkits, industry
variations, exam prep. Figures 1.1.1 (recourse spectrum), 1.1.2 (SPV hub), 1.2.1 (bankability
triangle). Harness: **241 golden checks, all passing**. PFL-AI volume: **56 pp** across
Domains 1, 3, 4 (12 figures, 50 index entries).

**Batch 6 (loop iteration 4) — PML-AI Domain 7, Cost, resources and commercial awareness.**
The book's cost/EVM flagship (~7.6k words), continuing Project Auriga from Domain 6 with money
attached (`BAC` 4,000,000; at week 13 `PV` 2,080,000, `EV` 1,920,000, `AC` 2,120,000):

- KA 7.1 estimating methods and accuracy classes, three-point cost estimating (mean 780,000 vs
  mode 750,000), and the estimate→control-account→baseline hierarchy with contingency inside the
  baseline and management reserve outside it.
- KA 7.2 actual-cost measurement (accruals, commitments, open-commitment hygiene), the
  forecasting question, and baseline integrity.
- KA 7.3 the full earned-value set: `CV` (200,000), `SV` (160,000), `CPI` 0.91, `SPI` 0.92,
  48.0 % complete against 53.0 % spent; the four-method `EAC` family spanning **USD 408,056** on
  identical data (4,200,000 / 4,416,667 / 4,608,056); `VAC` and `TCPI` (1.11 required against
  0.91 demonstrated) — including the taught identity that `TCPI` to an `EAC` of `BAC/CPI` equals
  the current `CPI`, i.e. that forecast *is* "nothing changes".
- KA 7.4 blended rates (130.63/h), the five contract models by who carries cost risk, incentive
  fee arithmetic and the **point of total assumption** (2,428,571, where the buyer's outlay
  equals the ceiling exactly — verified as an invariant), and cash flow versus profit.
- Advanced topics (earned schedule closing `SPI`'s late-project blind spot, EVM's stated limits,
  reviewer invariants), industry variations, two case studies (the forecast the board actually
  needed; past the point of total assumption), executive perspective, 5 exercises, 3 toolkits,
  exam prep, 14 tagged MCQs, self-checks.
- Figures 7.3.1 (earned-value S-curves at the data date) and 7.3.2 (the `EAC` fan beside the
  `TCPI` gap).
- Harness: **276 golden checks, all passing** (was 241). PML-AI volume now typesets at **45 pp**
  across Domains 6–7 (7 figures, 47 index entries).

**Batch 7 (loop iteration 5) — PML-AI Domain 1, The project leadership profession.**
The book's opening domain (~7.1k words), introducing the master programme **Meridian Care
Records** (40-clinic public-health records rollout) that returns in Domains 2 and 16:

- KA 1.1 the delivery landscape (project/programme/portfolio as different objects of management,
  each with its own success test), project vs operational leadership (irreversibility as the
  defining failure mode; lead time as the cheapest resource), the temporary organisation, and the
  suite's responsible-AI principle stated for the first time.
- KA 1.2 accountability defined precisely — responsibility is delegable, the obligation to answer
  is not, one name per outcome — from which the AI corollary follows structurally rather than by
  policy: a tool cannot be asked to answer, so accountability never moves to it. Plus the
  four-direction obligation set, the escalation duty, the honesty asymmetry, and the standard of
  care.
- KA 1.3 systems thinking (feedback and delay, local optimisation, pressure relocating rather
  than vanishing) and the outputs→outcomes→benefits→value chain made **arithmetic**: Meridian's
  40 installed clinics yield **USD 685,440**/yr at 70 % adoption, not the **USD 979,200** an
  output-based claim asserts — a **30.0 %** overstatement that is exactly the non-adoption rate
  reappearing as fictitious value. Cost of delay priced at **USD 14,280/week**, acceleration
  breaking even at **4.20 weeks**, and an adoption sensitivity (50/70/90 % →
  489,600 / 685,440 / 881,280) showing the leader's attention belongs on adoption, not schedule.
- KA 1.4 professional ethics (the daylight test), the four responsible-AI obligations, the honest
  failure-mode list including over-trust through fluency, and the leader's three concrete acts
  (name the owner, proportionate verification, protect the team's judgment).
- Advanced topics (authority vs influence in borrowed teams, the multiple-verdict problem,
  reviewer invariants), industry variations, two case studies (Meridian under scrutiny — praised
  then called a failure on true facts both times, with adoption owned by nobody; the plan nobody
  could critique), executive perspective, 3 exercises, 3 toolkits, exam prep, 13 tagged MCQs.
- Figure 1.3.1 (the value chain and where it leaks).
- Harness: **296 golden checks, all passing** (was 276), including the invariant that the
  overstatement equals the non-adoption rate. PML-AI volume now typesets at **64 pp** across
  Domains 1, 6 and 7 (8 figures, 69 index entries), with the Part One divider now leading.

**Batch 8 (loop iteration 6) — PFL-AI Domain 2, Accounting and financial-statement foundations.**
This **completes PFL-AI Part One** (~7.3k words), discharging the obligation Domain 1 left open —
if profit does not pay debt service, how do you get from one to the other:

- KA 2.1 accrual vs cash (each governing different covenants), recognition and measurement as
  tests rather than preferences, and articulation as the reader's diagnostic.
- KA 2.2 the three statements built on Kestrel's first operating year, **tied numerically to
  Domains 3 and 4**: the plant is Domain 4's `I₀` 60,000,000 over 25 years, interest is Domain 3's
  year-one 2,520,000, and the 2,489,635 of principal is Domain 3's own schedule row — so
  `EBITDA` 7,500,000 → `EBIT` 5,100,000 → PBT 2,580,000 → net income 2,064,000, bridged to
  **operating cash flow 3,864,000** and proven to articulate.
- KA 2.3 the four decisive treatments: working capital moving `DSCR` from **1.39 to 1.27** on the
  *documented* `CFADS` definition (the domain's central professional point — a ratio is only as
  good as its defined term); revenue recognition following performance and needing to agree with
  the progress evidence PML-AI Domain 7 uses for earned value; capex vs opex moving year-one
  profit by **1,080,000** on 1,200,000 of spend while **cash is identical**; and provisions
  recognised on tests, never created as reserves.
- KA 2.4 ratio families, interest cover **2.02×** against debt/`EBITDA` **5.27×** (both true,
  pointing different ways — revenue certainty, not the ratio, is what makes leverage tolerable),
  and the project/ledger interfaces including the **five meanings of "spend"**.
- Advanced topics (deferred tax and why cash tax ≠ accounting tax in `CFADS`, leases and stale
  off-balance-sheet intuitions, reviewer invariants), industry variations, two case studies (the
  profitable year that came within ~USD 100,000 of a covenant breach; capitalised into a
  better-looking year, restated into a breach with no cash change), executive perspective,
  4 exercises, 3 toolkits, exam prep, 14 tagged MCQs.
- Figure 2.2.1 (the accrual-to-cash bridge as a waterfall).
- Harness: **334 golden checks, all passing** (was 296), including the cross-domain ties to
  Domain 3's instalment split. PFL-AI volume now typesets at **76 pp** across Domains 1–4
  (13 figures, 71 index entries) — **Part One complete**.

## Next production batch

Queued in loop order: (1) PML-AI Domain 8 (Risk, uncertainty and resilience) — Domain 7's
contingency treatment depends on it, and it registers `EMV`; (2) PML-AI Domain 2 (Strategy,
selection and business alignment), continuing the Meridian thread into benefits mapping and
completing PML-AI Part One alongside Domains 3–4; (3) PFL-AI Domain 10 (Debt sizing, covenants and
credit metrics), which Domains 2–4 have now fully prepared (`CFADS` defined, discounting built,
appraisal in place) and which registers `DSCR`/`LLCR`/`PLCR`. Phase 2 completes when all
foundation domains of both books pass gates; PFL-AI Part One is now done.
