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

**Batch 9 (loop iteration 7) — PML-AI Domain 8, Risk, uncertainty and resilience.**
(~7.5k words) Closes the two obligations Domains 6 and 7 left open — where schedule ranges come
from, and how the contingency inside the baseline is actually sized:

- KA 8.1 risk/uncertainty/issue kept apart, the **cause → event → consequence** statement (each
  part mapping to a different response type), identification methods *with their blind spots*, and
  the observation that registers running 90 % threats reflect defensive framing rather than reality.
- KA 8.2 the analysis ladder: qualitative screening and why ordinal scores must not be multiplied
  as money; `EMV` on Auriga's register (**278,000**, 6.95 % of `BAC`; 314,000 if the opportunity is
  ignored) with the two cautions that `EMV` averages outcomes that will not occur and that ranking
  by it reorders the register; a decision tree pricing the **value of information** at
  **59,000** against a 25,000 survey — plus the sensitivity where the same survey *destroys*
  value; and aggregation to a stated confidence: mean 278,000, σ **252,642**, **P80 ≈ 490,624**,
  set against the worst-case sum (1,140,000 = 28.5 % of `BAC`), the `EMV` sum and the reasoning-free
  10 % rule of thumb.
- KA 8.3 responses as investments judged on `EMV` reduction (reusing Domain 6's fast-track at
  **+33,000**), the rule that impact governs survivability even at low probability, secondary
  risks, reserve authority, and the draw-protocol/retirement governance.
- KA 8.4 resilience as distinct from prediction (buffers, optionality, modularity, redundancy, fast
  detection — bought at an efficiency cost that should be chosen), a bias table with countermeasures
  including the pre-mortem, the crisis sequence, and AI risk sensing with its structural blind spot
  (novel risk) and its real failure mode (displaced judgment).
- Advanced topics: correlation and why aggregate risk exceeds the independent estimate; schedule
  risk analysis and **merge bias** (two 0.80 paths converging give **0.64**, which deterministic
  CPM cannot see — the arithmetic behind Domain 6's convergence warning); reviewer invariants.
- Industry variations, two case studies (the survey Auriga did not commission — the same event
  Domain 6 recovered, now priced; the "diversified" portfolio where 14 of 22 risks shared one
  six-person team), executive perspective, 4 exercises, 3 toolkits, exam prep, 12 tagged MCQs.
- Figure 8.2.1 (the survey decision tree).
- Harness: **368 golden checks, all passing** (was 334); it caught two drifted figures during
  authoring (a P80 and an overstatement percentage), both corrected before commit. `EMV` flipped to
  verified in the shared registry. PML-AI volume now typesets at **81 pp** across Domains 1, 6, 7
  and 8 (9 figures, 90 index entries).

**Batch 10 (loop iteration 8) — PML-AI Domain 2, Strategy, selection and business alignment.**
(~8.7k words) Answers the question Domain 1 left prior: how work gets chosen, and how anyone knows
it was worth choosing:

- KA 2.1 strategy→portfolio (the three symptoms of a gap), drivers with their misidentification
  failure modes, hard vs **soft** constraints (only soft ones are tradeable), and alignment as a
  repeated test that decays after approval.
- KA 2.2 the business case as decision instrument — the test being whether it **could have concluded
  "no"** — genuine options sets versus straw ones, and the domain's central arithmetic: Meridian's
  approved case claimed full potential (**979,200**, Domain 1's own output-based claim) from year
  one for NPV **+3,447,096**; the same facts ramped as adoption actually arrives (40/60/70 % to a
  **685,440** steady state, tying to Domain 1) give NPV **+1,332,898** — an overstatement of
  **USD 2,114,198**, or **158.6 %** of the honest figure, which changed no approval decision and
  therefore was never challenged. Breakeven sustained adoption **41.05 %** is offered as the
  sentence a board can actually monitor. Selection: weighted scoring (Beta 4.05 over Meridian 3.95)
  and constrained ranking where **Beta + Gamma's 2,100,000 beats Meridian's 1,693,072** inside a
  3-unit capacity limit — the delivery twin of PFL-AI's capital rationing, with the same
  lumpy-candidate caveat.
- KA 2.3 benefits mapping with the **enabling change** column most maps omit (and where Meridian
  stalled at 40 %), baselines measured before not reconstructed after, attribution,
  double-counting with one claimant per benefit, cash-releasing versus capacity benefits, ESG as
  constraint *or* value but never confused, and the assumption register with falsifying triggers.
- KA 2.4 strategic termination: forward-looking NPV only — **780,000 of value for 900,000 of spend
  is a stop**, whatever the 1,800,000 already spent suggests — escalation of commitment as a
  structural problem, and kill criteria whose power is entirely in advance agreement.
- Advanced topics (real options thinking, portfolio balance vs ranking, reviewer invariants),
  industry variations, two case studies (the Meridian case that should have been written; the
  platform that could not be stopped — resolved by descoping to where remaining benefit exceeds
  remaining cost), executive perspective, 4 exercises, 3 toolkits, exam prep, 13 tagged MCQs.
- Figures 2.2.1 (two business cases from identical facts) and 2.3.1 (benefits map with the enabling
  change restored).
- Harness: **403 golden checks, all passing** (was 368); it caught one drifted figure and one
  rounding-boundary tolerance during authoring. PML-AI volume now typesets at **101 pp** across
  Domains 1, 2, 6, 7 and 8 (11 figures, 112 index entries).

**Batch 11 (loop iteration 9) — PFL-AI Domain 10, Debt sizing, covenants and credit metrics.**
(~7.9k words) The volume's quantitative flagship, and the point where Domains 2, 3 and 4 converge:
Domain 2 defined `CFADS` as a *documented* term, Domain 3 built the annuity factor and amortisation
schedule, Domain 4 valued the project — this domain decides how much debt it can carry.

- KA 10.1 `CFADS` line-by-line with the negotiation points named, then the domain's central
  arithmetic: Kestrel's `CFADS` of **6,384,000** at a **1.30×** target supports debt service of
  **4,910,769** and therefore **41,171,123** of debt — **828,877 short** of the 42,000,000 request,
  which is exactly the additional equity the sponsors must find. Sizing depends on cash, coverage,
  rate and tenor and on nothing else, which is what makes the four negotiating levers enumerable.
  Sculpting, cash sweeps and balloons complete the shape of debt service.
- KA 10.2 the four ratios and what each is blind to, with **an identity worth teaching**: where
  `CFADS` is level and debt service is an annuity at the loan rate, `LLCR` equals `DSCR` **exactly**
  (both **1.2743** for Kestrel) — a reviewer's check, because level-cash models whose two ratios
  differ contain an inconsistency. `PLCR` **1.9431** quantifies the 13-year tail. Headroom is
  restated in cash: the covenant fails below `CFADS` of **6,011,562**, i.e. **372,438** (5.8 %) of
  annual cash — the sentence that belongs in a board paper. A 20 % shortfall gives `DSCR` **1.0195**:
  breach, with the lenders paid in full — the domain's key distinction.
- KA 10.3 the reserve family and the waterfall's top; the six-month DSRA of **2,504,818** expressed
  as the shortfall it survives (a collapse to 39 % of base-case cash) rather than as months, with
  the explicit note that it buys payment continuity, not covenant compliance.
- KA 10.4 covenant types, the lock-up as a graduated remedy short of default, events of default,
  cure rights and equity cures, and living with covenants (early disclosure as the negotiating
  asset).
- Advanced topics (forward-looking tests and whose forecast counts; refinancing, tails and
  mini-perms; the reviewer's coverage eye), industry variations, two case studies — **"the 828,877
  that changed the structure"**, in which the bank's 1.25× concession applied to its own 5 %-stressed
  case supports only **40,677,069**, *less* than the 1.30× base-case answer (a lower ratio on a lower
  cash case is not a concession), resolved at **41,000,000** of senior debt (`DSCR` **1.3054**) plus
  1,000,000 of equity; and **"paid in full and in breach"**, a toll road at `DSCR` **1.0591** whose
  sponsors modelled the covenant and not the lock-up — executive perspective, 4 exercises, 3
  toolkits, exam prep, 13 tagged MCQs.
- Figure 10.1.1 (debt capacity against coverage and tenor, with the request line and the coverage it
  actually delivers).
- Harness: **483 golden checks, all passing** (was 403). It caught a drifted maximum-debt figure in
  Exercise 10.1 (47,861,672 → **47,864,408**) and the two values downstream of it in Exercise 10.2,
  plus a truncated capacity figure and an over-rounded equity-cure ratio in the case studies — all
  corrected before commit. `DSCR`, `LLCR`, `PLCR`, `ICR`, `D/E` and five sizing/reserve formulas
  flipped to verified in the shared registry.
- PFL-AI volume now typesets at **95 pp** across Domains 1–4 and 10 (14 figures, 93 index entries),
  with the **Part Three** divider live. Both volumes together stand at **196 typeset pages**.

## Next production batch

Queued in loop order: (1) PML-AI Domain 3 (Governance, organization and decision rights) — Domains
1, 2 and 8 all now depend on it, and it is the first half of what completes PML-AI Part One;
(2) PML-AI Domain 4 (Integration and delivery architecture), which closes Part One; (3) PFL-AI
Domain 5 (Cost of capital and capital structure), which Domain 10's coverage work now motivates
directly. Phase 2 completes when all foundation domains of both books pass gates: **PFL-AI Part One
is done** (and Part Three opened with Domain 10); PML-AI Part One is half done.
