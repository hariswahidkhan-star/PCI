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

**Batch 12 (loop iteration 10) — PML-AI Domain 3, Governance, organization and decision rights.**
(~9.2k words) The domain Domains 1, 2 and 8 all depend on, and the one that turns a habitually
qualitative subject into a computable one. Its central claim: **governance is a delivery variable
with a price, not a compliance overhead** — and a leader who cannot price it will be asked to accept
a structure that guarantees delay.

- KA 3.1 what governance is *for*, defined by exclusion (not management, not assurance, not
  administration — an organisation can hold every governance artefact and have no governance,
  because nobody in the chain can decide); the four testable products — decidability, timeliness,
  legitimacy, traceability; structures across functional, matrix, projectised and multi-party forms
  each with its **characteristic weakness and countermeasure** (undefined matrix precedence does not
  split authority, it awards it to escalation behaviour; unanimity is indistinguishable from an
  inability to decide); and governing iterative delivery through a bounded envelope rather than
  either abandoning control or destroying cadence.
- KA 3.2 the sponsor as **seven testable obligations** with an evidence test each, and the four
  sponsor failure modes with their detection tests; the five steering-committee design faults
  (including the under-noticed one that a committee's effective authority is the *minimum* of its
  members' on the decision in hand); committee capacity computed — **104** item-slots against **77**
  of demand, of which 26 are standing reports consuming a quarter of the scarcest resource in the
  programme.
- **The formula this domain contributes:** `E[wait] = M/2 + L`. The expected wait for a committee
  decision is half the meeting interval plus the *whole* paper lead time — so Meridian's "monthly"
  committee imposes a full **4.0-week** wait, not the fortnight everyone assumes, and the two levers
  are unequal: **cutting the paper lead time by a week saves a full week; cutting the meeting
  interval by a week saves only half of one.** The administrative deadline, which is free to change,
  is twice the lever meeting frequency is. The harness re-derives the formula independently by
  numeric integration over arrival times.
- Delegation priced: at a 10,000 threshold Meridian escalates **36** of 60 changes, costing
  **USD 514,080** a year in delay; at 25,000 it costs **342,720** — a **171,360** saving against a
  worst case of **81,600** *even if the delegate decided every one of the twelve delegated changes
  wrongly and destroyed 40 % of its value*. Breakeven value destruction **84 %** per decision;
  breakeven critical-path share **11.9 %**. The general result — the cost of escalation is certain,
  recurring and invisible while the cost of a delegated error is uncertain, occasional and highly
  visible, so organisations optimise against the visible cost — with three stated cautions
  (irreversibility and externality are not priced by this test; the critical-path share is an
  assumption; delegation without information is abdication).
- KA 3.3 gates as the purchase of **optionality against irreversibility**, with gate economics
  computed: Meridian's design gate is worth **USD 56,520**, stops paying beyond **9.96 weeks** of
  elapsed time (the arithmetic behind the usually unquantified complaint that assurance has become an
  obstacle) and needs a detection probability above **55.85 %** to be worth holding at all. Three
  assurance lines with capture named as the worst failure because the product still looks
  independent. Escalation as a **timed** pathway: Meridian's three tiers total **15.5 weeks** and
  **USD 221,340** for one decision, of which the quarterly committee alone is **61 %** — reducible to
  4.0 weeks (**74.2 %** saving) by removing a tier no one can justify, or 1.0 week (**93.5 %**) with a
  written-resolution route. And the decision record, whose most consequential missing field is the
  **versioned reference to the information relied on**, because the retrospective question is never
  "was it right?" but "was it reasonable on what was known?" RACI single-A audit: **25.0 %** defect
  rate, with two-A and zero-A classes failing differently under stress.
- Advanced topics (governance under stress and the recovery structure; governance *of* AI-assisted
  delivery, with the three questions a body must be able to answer about any AI-informed
  recommendation; the reviewer's governance eye), industry variations, two case studies — **"the
  four-week month"**, in which the same complaint that had gone nowhere for two quarters as a
  cultural objection was approved in one meeting as arithmetic; and **"the decision nobody made"**, in
  which four changes each individually within authority and cumulatively worth 700,000 never reached
  a decision record, fixed by adding reversibility, externality and a **cumulative test** to the
  delegation schedule — executive perspective, 4 exercises, 3 toolkits, exam prep, 15 tagged MCQs.
- Figures 3.2.1 (governance latency and its two levers) and 3.3.1 (the price of an escalation path).
- Harness: **581 golden checks, all passing** (was 483), including an independent re-derivation of
  the latency formula. It caught one broken MCQ during authoring — 3.2-B had two correct options
  because at `M` = 4, `L` = 2 both levers happen to reach 3.0 weeks — rebuilt on a one-week cut so
  the asymmetry is the answer. `E[wait]`, cost of delay, gate net value and committee capacity
  registered as verified formulas.
- PML-AI volume now typesets at **129 pp** across Domains 1, 2, 3, 6, 7 and 8 (13 figures, 137 index
  entries), with the **Part Two** divider live. Both volumes together stand at **224 typeset pages**.

**Batch 13 (loop iteration 11) — PML-AI Domain 4, Integration and delivery architecture.**
(~10.4k words) **PML-AI Part One is now complete.** The domain that assembles Domains 1–3 into a
project, on the observation that integration fails in one characteristic way: **the parts are managed
and the joins are not.**

- KA 4.1 the charter as a conferral of authority rather than a plan, with the authority statement
  named as its irreplaceable content; the plan of plans integrated by consistency checks that only
  appear when plans are read against each other — including the one where a report produced three
  days after the steering committee's papers close makes a programme report last month's position
  *every month, for its whole life*; and tailoring as a recorded decision with three stated limits.
- KA 4.2 the WBS and the **hundred-per-cent rule as an arithmetic invariant, therefore auditable**:
  Meridian's five level-2 elements sum to **2,332,000** against an approved **2,400,000**, and the
  missing element is **clinician training and enabling change** at **214,000** — the same column
  Domain 2's benefits map omitted, for the same reason (it belongs to somebody else, so nobody
  decomposes it). The honest baseline is **6.1 %** above approved and still returns NPV
  **+1,186,898** against Domain 2's +1,332,898, an **11.0 %** reduction: the omission bought nothing.
- **Interface economics, the domain's flagship arithmetic.** Components grow linearly and interfaces
  combinatorially. Meridian's 12 components admit **66** point-to-point interfaces costing
  **USD 1,188,000** against **12** plus a layer at **536,000** — a **54.9 %** saving, worth building
  below **972,000**. The decisive figure is marginal: a thirteenth component costs **216,000** meshed
  and **18,000** layered, a factor of 12 that grows with every component, so the architecture is a
  bet on the future component count. Three cautions stated (partial meshes, the layer's single point
  of failure and throughput constraint, and the averaged unit cost).
- KA 4.3 the baseline as one three-dimensional statement, with the invariant that **the time-phased
  cost baseline must move whenever the schedule does** — the first thing abandoned under pressure,
  after which earned value measures the distance between two documents. Configuration audit with its
  finding classes *not* totalled: of 340 items, 28 unidentified, 11 ambiguous and **5 whose recorded
  version differs from what is deployed** — a 12.94 % headline rate that understates the position,
  because those five invalidate any verification performed against the register.
- **Baseline drift quantified**: Meridian's baseline moved **12.1 %** (USD 291,176) through 34
  individually authorised changes averaging **0.28 %** each, with no decision anywhere on the total —
  Domain 3's Case study B mechanism with a number attached. And the honest follow-through: a
  "100,000 in a rolling 90 days" cumulative rule **would not have caught it**, because a quarter's
  changes aggregate to **57,800**; catching it needs a threshold below 57,800 at 90 days or the same
  100,000 at 180 days (115,600). A cumulative test set at a round number without reference to the
  observed change rate has the appearance of a control and none of the function.
- KA 4.4 the change flow with change/clarification/defect separated (a defect misclassified as a
  change pays a supplier twice); **assessed impact versus quoted cost** — a change quoted at
  **40,000** truly costs **131,560**, **3.29×**, of which the quote is **30.4 %** — and the
  structural consequence that connects straight back to Domain 3: **a delegation threshold applied to
  quoted direct cost is not a control**, since a change quoted at 22,000 with two weeks of
  critical-path impact truly costs 50,560, twice the threshold, and is decided without escalation.
  The remedy is one sentence in the delegation schedule. Plus the rejection entry as the change-log
  record most often missing and the reason requests recur.
- Advanced topics (integration across organisational boundaries and the four contract provisions that
  address it; **architectural decisions as governance decisions** — a decision costing 40,000 to take
  and 2,000,000 to reverse is not a 40,000 decision, which a value-only delegation schedule cannot
  see; the reviewer's integration eye), industry variations, two case studies — **"the interface
  nobody owned"**, where the required count was **31** (not the theoretical 66 nor the architecture's
  promised 12), **9** had no owner on the far side, and the 342,000 of unplanned interface effort
  came from planning to the promise; and **"the baseline that could no longer answer the question"**,
  a third baseline reporting `CPI` 0.99 where performance against the original was `CPI` **0.87** —
  a 14.9 % overrun that three replacements had made invisible without ever stating an untruth —
  executive perspective, 4 exercises, 3 toolkits, exam prep, 17 tagged MCQs.
- Figures 4.2.1 (interface growth, mesh against layered) and 4.4.1 (the change-cost waterfall with
  the delegation threshold drawn across it).
- Harness: **661 golden checks, all passing** (was 581). It caught three drifted values during
  authoring — a case-study week count that exceeded its own overrun, a `CPI`-to-overrun conversion
  stated as 13 % where 0.87 implies 14.9 %, and an MCQ distractor labelled as omitting rework that
  actually omitted the direct cost. Interface counts, the hundred-per-cent rule, assessed total impact
  and baseline drift registered as verified formulas.
- PML-AI volume now typesets at **156 pp** across Domains 1, 2, 3, 4, 6, 7 and 8 (15 figures, 165
  index entries). Both volumes together stand at **251 typeset pages**.

**Batch 14 (scaled authorship) — the remaining 20 domains, authored concurrently.**
The parallel-safe contribution points were built for exactly this and then used: 20 domains authored
at once, each adding a manuscript, a `figures_src` module and a `checks` module, none of them touching
a shared file. Five agents per domain — one author, three read-only reviewers (master-thread
continuity; pattern conformance and padding; MCQ integrity and IP/legal safety), and one
verifier/fixer who was the only agent permitted to edit after the author.

**Both books now carry their full 16-domain structure.**

| | Domains | Typeset | Figures | Worked examples | Index entries |
|---|---|---|---|---|---|
| **PML-AI** | 16 | **502 pp** | 33 | 95 | 479 |
| **PFL-AI** | 16 | **490 pp** | 36 | 128 | 397 |
| **Total** | **32** | **992 pp** | **69** | **223** | **876** |

The golden-answer suite stands at **4,675 checks, all passing**, across `verify_formulas.py` (the
first twelve domains) and 14 per-domain modules averaging ~240 checks each. The new domains run
15,000–21,000 words against the 7,000–12,000 of the first twelve — materially deeper, and the depth is
in worked examples and Interpretation steps rather than in prose volume.

**What went wrong, and what it cost.** The run was interrupted by a session restart after roughly
eight hours, which dropped the workflow's task handle. The work itself survived on disk — every
manuscript and every figure module completed — but **six domains lost their verification stage**:
PML-AI D5 and D16, PFL-AI D5, D6, D8 and D16. Their arithmetic was therefore entirely unchecked, which
is a gate failure and was recorded as one rather than absorbed. A focused six-agent run is completing
that verification; until it lands, nothing in those six domains should be relied on.

**Two pattern deviations, found by audit and normalised corpus-wide.** New domains had used
`## Summary — Domain N` where the pattern spec §4.10 and the approved book both specify
`## Domain N summary` (21 files), and one domain numbered its toolkits `### 16.T.n` rather than
`### Toolkit 16.T.n`. All 32 summaries and all 96 toolkits are now uniform.

**Hard-constraint audit across all 32 manuscripts — clean.** No trademark symbols. One reference to a
third-party body (AACE's Total Cost Management class progression), cited by name with the text stating
explicitly that it is described in this book's own words. Standards cited by number only (ISO 19650,
ISO 8000, ISO 9000/9001, IFRS 15, IFRS 16, IAS 37) with no content reproduced. Zero instances of
invented-evidence phrasing ("studies show", "research demonstrates" and eleven similar patterns). Every
domain joins an existing master thread and none introduced a third — Meridian and Auriga split
sensibly across PML-AI by scale, Kestrel runs through all sixteen PFL-AI domains.

**Master-thread numeric continuity — verified by sweep.** Kestrel's instalment (5,009,635), `CFADS`
(6,384,000), debt (42,000,000), equity (18,000,000), NPV (16,179,360) and `DSCR` (1.2743) appear
identically across 7–14 domains each. Four near-miss values were investigated individually and all are
legitimately different quantities: a year-8 `DSCR` of 1.2723 in a coverage table, a negotiation
settlement of 684,940, a retesting value of 689,585, and one that was a substring of 16,380,000.

**Front matter corrected.** Both volumes still declared themselves a "Phase 1 production prototype of
one domain" — false about a 500-page complete draft, on the one page whose entire job is to say what
the reader is holding. Replaced with a status page that separates what has been verified from what has
not, states plainly that the draft is AI-drafted and requires human editorial and technical review
before release and is attributed to no named expert, and carries the legal/jurisdictional note. The
domain count is now interpolated from the manuscripts actually built, so it cannot drift again.

## Next production batch

1. **Close the verification gap** — the six unverified domains (PML-AI D5, D16; PFL-AI D5, D6, D8,
   D16). Until every one has a passing checks module, neither book passes gate, whatever the page
   count says.
2. **Apply the registry updates** each verifier reports, and flip the remaining ⏳ rows. `WACC` is now
   ✅ — PFL-AI D9 derives it and discharges D4's use of 8 % as a given. `EVA(benefit)` waits on
   `pml_d16`.
3. **Phase 2 gate report**, then the phases the charter §8 sequences and this batch did NOT do:
   Phase 3 cases and question banks, Phase 4 glossaries and appendices, Phase 5 indexes, Phase 6
   accessibility, Phase 7 pilot review. Scaled authorship produced the domain corpus; it did not
   produce the back matter, and the page count must not be read as though it had.
4. **Human review remains outstanding and is not optional.** The charter requires editorial and
   technical review before release; 992 pages of AI-drafted material have had neither. The
   verification suite establishes that the arithmetic is right, which is a different claim from the
   book being correct, well-judged and publishable.
