# Domain 15 — Operations, Performance and Restructuring

## Why this domain exists

Every preceding domain answered a question that can be closed. Is the project worth building
(Domain 4)? Can it carry the debt (Domain 10)? Are the risks allocated (Domain 11), the
contracts sound (Domain 12), the diligence complete (Domain 13), the construction funded and
certified (Domain 14)? Financial close answers all of them once. **Operations answers them
again every quarter for twenty-five years**, against outturn rather than forecast, and with the
project's contractual machinery now live: tests that trap cash, reserves that must be topped up
before shareholders are paid, and remedies that engage automatically when a threshold is
crossed. The question this domain closes is the one the others left open — *what actually
happens to the cash, period by period, and who decides.*

The central claim is that **the operating phase is governed by the distribution test, not by
the covenant.** Domain 10 established the covenant, the lock-up and the reserve as the lender's
control architecture, and computed Kestrel Water's headroom to breach as USD 372,437.72 of
annual cash. That number is real but it is not the one that binds. The condition a sponsor must
satisfy to be paid is stricter than the condition it must satisfy to avoid default, it is tested
forwards as well as backwards, and it sits below a reserve top-up that has first call on the
same cash. A project can be fully compliant, paying every obligation on time, and still pay its
shareholders nothing for four consecutive years — which is exactly what Kestrel does below. From
there the domain follows the money outwards: monitoring and covenant testing (KA 15.1), the
waterfall and the distributions that fall out of it (KA 15.2), refinancing and the negotiated
amendments that change the terms mid-life (KA 15.3), and the arithmetic of distress,
restructuring, exit and handback (KA 15.4).

**Learning objectives.** After this domain a candidate can: build an operating bridge from
physical performance drivers to `CFADS` and reconcile it to the reported financial statements;
convert a covenant, lock-up and distribution threshold into a cash trigger and then into each
driver's own units; distinguish backward-looking, rolling and forward-looking tests and explain
why a rolling test lags a deterioration by its own window length; operate a full cash waterfall
period by period, including reserve top-ups, block accounts and the distributable amount that
falls out; explain and compute why deferred distributions cost equity more than the cash
deferred; price a refinancing as a net present value against break costs and fees and solve for
its breakeven margin; price a waiver, an equity cure and a covenant amendment against one
another and explain why cure rights are an option with a scarcity value; compare a maturity
extension, a principal haircut and an equity injection on both lender recovery and equity value,
and locate the enforcement floor that bounds the negotiation; size a handback reserve as a
sinking fund and explain why early funding can cost equity more in present value than late
funding; value an exit and separate genuine value creation from the arithmetic flattery of early
crystallisation; and govern AI-assisted monitoring, covenant certification and restructuring
analysis.

**The master financing, now operating.** Kestrel Water SPC reached commercial operations with the
structure Domains 3, 9 and 10 built: capital cost **USD 60,000,000**, funded **70/30** as
**USD 42,000,000** of senior debt at **6.0 %** over **12 years** and **USD 18,000,000** of
equity; annual instalment **USD 5,009,635.23**, of which year one is interest **2,520,000** and
principal **2,489,635**; a **25-year** operating life; documented first-year `CFADS` of
**USD 6,384,000** (**6,984,000** before working-capital movements) on `EBITDA` of **7,500,000**
and `EBIT` of **5,100,000**; a base-case `DSCR` of **1.2743**, identically equal to `LLCR`, with
`PLCR` at **1.9431** (Domain 10, KA 10.2.2). The facility carries a **1.20×** `DSCR` covenant, a
**1.15×** lock-up trigger, a six-month debt service reserve of **2,504,817.62**, and — the term
this domain adds — a **distribution condition requiring 1.25× on both a backward and a
forward-looking test**. This domain builds the operating bridge that produces the 6,384,000, runs
the waterfall for six years of real outturn, refinances at the end of year five, prices the
amendment that resolves a breach, restructures the project on a permanent-deterioration branch,
and funds an **8,000,000** handback obligation.

---

## Knowledge Area 15.1 — Operational monitoring, financial reporting and covenant testing

*Topics: 15.1.1 what changes at commercial operations · 15.1.2 the operating bridge from drivers
to `CFADS` · 15.1.3 triggers in driver units · 15.1.4 backward, rolling and forward-looking
tests.*

### 15.1.1 What changes at commercial operations

**Definition.** The **operating régime** is the standing set of reporting, testing and
certification obligations that replaces the construction régime once the completion tests of
Domain 14 are satisfied. Its four components are worth naming because each has a different
failure mode. **Information covenants** require delivery of management accounts, audited
statements, an annual operating budget and a periodic update of the financial model, each by a
contractual date; failure is a breach in its own right, independent of performance. **Compliance
certificates** are signed statements that the tested ratios have been computed on the documented
definitions and that no default subsists — the point at which a number stops being management
information and becomes a representation. **Financial covenant testing** measures the ratios on
defined dates. **Distribution conditions** are the positive tests a borrower must pass before it
may pay anything to shareholders.

Three consequences follow that surprise first-time operators. The **calendar becomes
contractual**: test and delivery dates are fixed and do not move because a system implementation
slipped. The **model becomes a deliverable**, not an internal tool — which is why Domain 6's
model governance and Domain 13's model audit are contractual rather than merely prudent. And the
**definitions become load-bearing in the other direction**: at close, a favourable `CFADS`
definition raised debt capacity; in operations, the same definition determines whether a
distribution is lawful.

### 15.1.2 The operating bridge from drivers to `CFADS`

**Definition.** The **operating bridge** is the reconciliation that carries physical performance
— availability, output, unit costs — through the accrual statements to the cash figure the
covenant is computed on. It matters because the two ends are owned by different people. Plant
availability belongs to the operator; `CFADS` belongs to the finance director; and the covenant
is breached by the first and reported by the second. Without an explicit bridge, nobody can say
what a one-point loss of availability does to a distribution.

**Worked example 15.1.2 — Kestrel's first operating year, from membranes to `CFADS`.**

1. **Setup.** Contracted capacity **30,000 m³/day**; guaranteed availability **95.0 %**;
   capacity payment **USD 7,300,000** per year at guaranteed availability, abated pro rata below
   it; volume payment **USD 0.55/m³** on water delivered. Year one delivered **10,000,000 m³** at
   **95.0 %** availability. Operating costs: fixed operations and maintenance **2,100,000**,
   energy **0.26/m³**, chemicals and consumables **0.04/m³**, insurance **200,000**.
   Depreciation is straight-line over the 25-year life. Cash tax is **20 %** of profit before
   tax. Working capital is receivables at 45 days of revenue (360-day convention), payables at
   90 days of cash operating cost, and an inventory of chemicals and membrane spares of
   **325,000**, built from nil during the first year.
2. **Formula.** Revenue = capacity payment × min(1, availability ÷ guaranteed) + volume price ×
   volume. `EBITDA` = revenue − cash operating cost. `EBIT` = `EBITDA` − depreciation. Cash tax
   = 20 % × (`EBIT` − interest). `CFADS` = `EBITDA` − cash tax ± movement in working capital, per
   the facility's documented definition (Domain 10, KA 10.1.1).
3. **Substitution.** Capacity payment `7,300,000 × 1.00`; volume payment `0.55 × 10,000,000`;
   operating cost `2,100,000 + 0.26 × 10,000,000 + 0.04 × 10,000,000 + 200,000`; depreciation
   `60,000,000 ÷ 25`; tax `0.20 × (5,100,000 − 2,520,000)`; working capital
   `12,800,000 × 45/360 − 5,300,000 × 90/360 + 325,000`.
4. **Result.**

| Line | USD | Note |
|---|---|---|
| Capacity payment | 7,300,000 | availability at guarantee, no abatement |
| Volume payment | 5,500,000 | 0.55 × 10,000,000 m³ |
| **Revenue** | **12,800,000** | |
| Cash operating cost | (5,300,000) | 2,100,000 + 2,600,000 + 400,000 + 200,000 |
| **`EBITDA`** | **7,500,000** | |
| Depreciation | (2,400,000) | 60,000,000 ÷ 25 |
| **`EBIT`** | **5,100,000** | |
| Interest | (2,520,000) | Domain 3's year-one interest |
| **Profit before tax** | **2,580,000** | |
| Cash tax at 20 % | (516,000) | |
| **`CFADS` before working capital** | **6,984,000** | `EBITDA` − cash tax |
| Movement in working capital | (600,000) | 1,600,000 − 1,325,000 + 325,000, built from nil |
| **`CFADS` as defined** | **6,384,000** | `DSCR` **1.2743** |

5. **Interpretation.** The table is the domain's single most useful artefact because it is the
   only place where an engineer's number and a lender's number are visibly the same number. Read
   downwards, it prices performance: the **cash-to-revenue gearing is 0.80** — a dollar of lost
   revenue costs only eighty cents of `CFADS`, because cash tax falls by twenty cents with it,
   which means every headroom figure expressed in `CFADS` understates the revenue loss the
   project can absorb by a quarter. Read upwards, it prices the definition: the 600,000
   working-capital line is the same choice Domain 2 (KA 2.3.1) used to move the reported `DSCR`
   from 1.39 to 1.27, and it is now a live monitoring exposure rather than a modelling
   convention — because working capital moves *with* revenue. When revenue falls, receivables
   fall and working capital releases cash, flattering `CFADS` in the year of decline; when
   revenue recovers, working capital rebuilds and penalises `CFADS` in the year of recovery. A
   deteriorating project therefore reports a covenant ratio that is *better* than its trading and
   a recovering project one that is *worse*, and a finance director who does not decompose the
   movement will misread both. The professional caution is to report `CFADS` in two lines — before
   and after working capital — in every internal pack, while certifying only the defined figure
   to the lenders.

### 15.1.3 Triggers in driver units

**Definition.** A **trigger in driver units** is a covenant, lock-up or distribution threshold
restated in the physical or commercial quantity that management actually controls. Domain 10
established the first translation, from ratio to cash. This is the second, and it is the one that
puts a covenant on an operations dashboard rather than a finance dashboard.

Kestrel's three thresholds, against annual debt service of 5,009,635.23:

| Test | Ratio | `CFADS` trigger | Headroom from 6,384,000 | % of `CFADS` |
|---|---|---|---|---|
| Distribution condition | 1.25× | **6,262,044.04** | **121,955.96** | 1.9103 % |
| Financial covenant | 1.20× | **6,011,562.28** | **372,437.72** | 5.8339 % |
| Distribution lock-up | 1.15× | **5,761,080.51** | **622,919.49** | 9.7575 % |

The first line is the domain's opening claim in arithmetic. **The distribution condition binds at
1.9103 % of `CFADS`; the covenant at 5.8339 %.** Kestrel is three times closer to losing its
dividend than to breaching its loan, and only one of those two numbers appeared in the financing
paper.

Translating the covenant headroom of 372,437.72 into drivers divides by the 0.80 cash-to-revenue
gearing to reach **465,547.16** of pre-tax revenue or cost, and then by each driver's own unit
economics:

| Driver | Unit economics | Move that reaches the 1.20× covenant | Move that reaches the 1.25× distribution test |
|---|---|---|---|
| Availability | 76,842.11 per percentage point (7,300,000 ÷ 95) | **6.0585 points** — availability floor **88.9415 %** | **1.9839 points** — floor **93.0161 %** |
| Delivered volume | 0.25/m³ cash contribution (0.55 − 0.26 − 0.04) | **1,862,188.62 m³**, 18.6219 % of output | **609,779.81 m³**, 6.0978 % |
| Energy unit cost | 10,000,000 m³ exposed | **+0.046555/m³** (+17.9 % on 0.26) | **+0.015244/m³** (+5.9 %) |

Two readings of that table are worth carrying. First, **the drivers are not equally dangerous**:
a 6-point availability loss and a 19 % volume loss both cost the same cash, but one is a plausible
outcome of membrane fouling and the other is not, so the availability line is the one that belongs
in a monthly review. Second, **the energy column is where an unhedged input becomes a covenant
risk** — a 5.9 % rise in the delivered cost of power, which no operator would call a crisis,
removes the entire dividend.

### 15.1.4 Backward, rolling and forward-looking tests

**Definitions.** A **backward-looking test** measures the period that has ended. A **rolling
test** measures the trailing window — commonly four quarters — and is the standard form because it
removes seasonality. A **forward-looking test** measures the projected next window on an agreed
basis. Most facilities test the covenant on a rolling backward basis and the distribution
condition on both bases, and the reason is the arithmetic below.

**Worked example 15.1.4 — the four quarters a rolling test cannot see.**

1. **Setup.** Kestrel's reported quarterly `CFADS` in year one is **1,700,000 · 1,640,000 ·
   1,560,000 · 1,484,000** (summing to the documented 6,384,000) and in year two
   **1,610,000 · 1,520,000 · 1,440,000 · 1,393,894.11** (summing to 5,963,894.11). Because the
   loan is a level annuity paid annually, **every rolling twelve-month window contains exactly one
   instalment of 5,009,635.23**, so the denominator is constant. At the year-one test date the
   board's re-forecast of year two is **5,500,000**.
2. **Formula.** Rolling `DSCR` = Σ (last four quarters' `CFADS`) ÷ 5,009,635.23. Forward `DSCR` =
   forecast next-twelve-month `CFADS` ÷ 5,009,635.23.
3. **Substitution.** Successive four-quarter sums: `6,384,000`; `6,294,000`; `6,174,000`;
   `6,054,000`; `5,963,894.11`. Forward: `5,500,000 ÷ 5,009,635.23`.
4. **Result.**

| Test date | Rolling four-quarter `CFADS` | Backward `DSCR` | 1.25× distribution | 1.20× covenant | 1.15× lock-up |
|---|---|---|---|---|---|
| End Y1 Q4 | 6,384,000.00 | **1.2743** | pass | pass | clear |
| End Y2 Q1 | 6,294,000.00 | **1.2564** | pass | pass | clear |
| End Y2 Q2 | 6,174,000.00 | **1.2324** | **fail** | pass | clear |
| End Y2 Q3 | 6,054,000.00 | **1.2085** | **fail** | pass | clear |
| End Y2 Q4 | 5,963,894.11 | **1.1905** | **fail** | **breach** | clear |

   The forward test at end Y1 Q4, on the 5,500,000 re-forecast, gives **1.0979** — failing both
   the distribution condition and the covenant threshold **four quarters before the backward test
   registers anything at all**.
5. **Interpretation.** This is the most important table in the knowledge area, and its lesson is
   structural rather than numerical: **a rolling backward test lags a deterioration by the length
   of its own window.** Kestrel's cash begins falling in the second quarter of year one and its
   covenant does not break until the fourth quarter of year two — by which time the problem is
   eighteen months old, the remediation window has closed, and the negotiation happens from a
   position of fact rather than forecast. The forward test exists precisely to close that gap, and
   it does: it fails at the first available date. Three professional consequences follow. **Whose
   forecast counts becomes a covenant question** — Domain 10 (KA 10.A.1) named this, and here it
   has a price, because at the end of year one the sponsors' own re-forecast is the instrument
   that stops their dividend. **A forward test punishes candour**, which is the uncomfortable
   truth of the mechanism: a management team that revises its forecast honestly triggers a
   consequence a team that does not revise avoids for a year, and the only defence against that
   perverse incentive is a documented re-forecast basis with a named owner and an independent
   review. And **the outturn will differ from the forecast anyway**: year two actually delivered
   **5,963,894.11**, some **463,894.11 above** the re-forecast that stopped the dividend. The
   distribution was correctly withheld on the information available, and the information available
   was wrong by nearly half a million dollars. Reporting that reconciliation — forecast, outturn,
   variance, cause — is what converts a forward test from a trap into a control.

### AI in this KA

**Where it earns its place.** Operating monitoring is the strongest genuine application of machine
work in this book: high-frequency, high-volume, structurally repetitive. Three uses are
straightforwardly valuable. **Driver-level anomaly detection** — flagging that specific energy
consumption per cubic metre has drifted 3 % above its trailing distribution weeks before the
quarterly pack would show it, because that drift is the leading indicator of the covenant.
**Automated bridge reconciliation** — checking that the operating bridge of 15.1.2 still ties from
the meter readings to the ledger to the reported `CFADS`, every month, and raising the specific
line that broke. **Reporting-calendar assurance** — tracking every information covenant against
its contractual date across a portfolio, which is pure administration and a real source of
avoidable breach.

**Where it must not go.** An assistant must not compute the certified ratio, and must not draft
the compliance certificate. The certificate is a representation by a named officer that the
number was computed on the documented definition; a model that has read the definition well is
still a model that has read it, and the distinction between a plausible reading and the correct
one is the entire content of Domain 10's KA 10.1.1. Nor should a model produce the forward
forecast that a distribution test will be measured on — that forecast has a contractual
consequence and therefore needs an owner who can be questioned.

**Verification, concretely.** Recompute one period's certified `DSCR` by hand from the
statements, every test date, and tie it to the bridge of 15.1.2. Reconcile the machine's `CFADS`
line to the facility definition clause by clause once per model version (Toolkit 15.T.1).
Back-test the anomaly detector against the last eight quarters and record its false-negative
rate, because a monitoring tool that has never been measured is an assumption, not a control.
**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 15.1

| Term | Meaning |
|---|---|
| **Operating régime** | The standing reporting, testing and certification obligations that follow completion. |
| **Compliance certificate** | A signed representation that tested ratios were computed on the documented definitions and no default subsists. |
| **Operating bridge** | The reconciliation from physical drivers through the accrual statements to defined `CFADS`. |
| **Cash-to-revenue gearing** | The fraction of a revenue movement that reaches `CFADS` after cash tax — 0.80 for Kestrel. |
| **Trigger in driver units** | A ratio threshold restated in availability points, volume or unit cost. |
| **Rolling test** | A covenant measured on a trailing window; it lags a deterioration by the window length. |
| **Forward-looking test** | A covenant or distribution condition measured on a projected window on an agreed basis. |

### Sample MCQs — KA 15.1

**MCQ 15.1-A `[15.1.3 · Application]`** Kestrel's debt service is 5,009,635.23 and `CFADS` is
6,384,000. Which figure states the headroom to the **distribution condition** of 1.25×?
- A. USD 372,437.72
- B. USD 121,955.96 ✅
- C. USD 622,919.49
- D. 0.0243 of a ratio point

*Rationale:* `6,384,000 − 5,009,635.23 × 1.25 = 121,955.96`, or 1.9103 % of `CFADS`. A is the
headroom to the 1.20× covenant and is the standard confusion this KA exists to remove; C is the
headroom to the 1.15× lock-up; D expresses the gap in ratio points, which conveys no magnitude
(Domain 10, KA 10.2.1).

**MCQ 15.1-B `[15.1.2 · Analysis]`** Kestrel's revenue falls by 500,000 in a year. Holding
working capital constant, `CFADS` falls by:
- A. USD 500,000
- B. USD 400,000 ✅
- C. USD 625,000
- D. USD 100,000

*Rationale:* Revenue falls to `EBITDA` one-for-one, profit before tax falls by the same amount and
cash tax falls by 20 % of it, so `CFADS` falls by `0.80 × 500,000 = 400,000` — the cash-to-revenue
gearing of 15.1.2. A ignores the tax shield entirely; C divides by 0.80 instead of multiplying,
which is the correct arithmetic run backwards (it converts a `CFADS` gap into a revenue gap); D is
the tax saving mistaken for the cash effect.

**MCQ 15.1-C `[15.1.4 · Analysis]`** A project's `CFADS` begins declining in the second quarter of
year one. Its covenant is a rolling four-quarter `DSCR`. The earliest date on which that covenant
can fail, and the reason, is:
- A. the same quarter, because the test is continuous
- B. up to four quarters later, because a trailing window dilutes each weak quarter with three earlier stronger ones ✅
- C. never, provided debt service is paid
- D. immediately, because rolling tests are more sensitive than annual tests

*Rationale:* Kestrel's window carries the decline for four quarters before the sum falls through
the threshold (15.1.4). A misdescribes a trailing measure; C confuses breach with payment default
(Domain 10, KA 10.2.1); D reverses the effect — smoothing reduces sensitivity, which is why the
forward test is needed alongside.

**MCQ 15.1-D `[15.1.3 · Application]`** Kestrel's capacity payment is 7,300,000 at a guaranteed
availability of 95.0 %, abated pro rata. Availability alone must fall to roughly what level before
the 1.20× covenant is breached?
- A. 93.02 %
- B. 88.94 % ✅
- C. 90.15 %
- D. 87.43 %

*Rationale:* Covenant headroom 372,437.72 ÷ 0.80 = 465,547.16 of revenue, ÷ 76,842.11 per point =
6.0585 points below 95.0 %. A is the floor for the 1.25× distribution condition (1.9839 points);
C omits the 0.80 cash-to-revenue gearing altogether (372,437.72 ÷ 76,842.11 = 4.8468 points);
D applies the gearing twice (465,547.16 ÷ 0.80 = 581,933.95, giving 7.5731 points).

**MCQ 15.1-E `[15.1.4 · Evaluation]`** At the year-one test date the sponsors' own honest
re-forecast of **5,500,000** gave a forward `DSCR` of **1.0979** and stopped a dividend of
774,364.77. Year two in fact delivered **5,963,894.11** — **463,894.11 above** the re-forecast. A
director argues at the year-two board that the re-forecast should have been left unrevised. What is
the sound response?
- A. the director is right — the outturn shows the re-forecast was wrong, so the distribution was wrongly withheld
- B. the withholding was correct on the information available and the outturn does not change that; the defence against a test that punishes candour is a documented re-forecast basis with a named owner and independent review, plus a forecast-to-outturn reconciliation in every pack ✅
- C. the director is right in principle — a sponsor should not be required to produce the forecast that stops its own dividend, and forward tests should be resisted in negotiation
- D. the re-forecast should be prepared on the lenders' model rather than the sponsors'

*Rationale:* A decision is judged on the information available when it was taken, and the honest
answer to the perverse incentive is process, not a worse forecast (15.1.4). A is outcome bias, and
accepting it institutionalises optimism in a number that has contractual force. C is a defensible
negotiating position in the abstract but answers a question that is already closed — the test is in
the documents, and it is what closed the four-quarter lag a rolling backward test cannot avoid. D
moves the same unowned judgment to a different party's spreadsheet.

**MCQ 15.1-F `[15.1.2 · Comprehension]`** Which statement best restates why a deteriorating project
reports a covenant ratio *better* than its trading, and a recovering one *worse*?
- A. because depreciation does not move with revenue, so `EBIT` falls faster than `EBITDA`
- B. because receivables and payables move with revenue, so a decline releases working capital into `CFADS` in the year it happens and a recovery reabsorbs it in the year of recovery ✅
- C. because cash tax falls when revenue falls, so only eighty cents of each lost dollar reaches `CFADS`
- D. because the covenant is measured on a rolling window, which smooths a decline across four quarters

*Rationale:* The asymmetry is a working-capital effect and nothing else (15.1.2). C is true and is a
different mechanism — the 0.80 cash-to-revenue gearing dampens movements in *both* directions
symmetrically, so it cannot flatter one and penalise the other. D is a real lag (15.1.4) but
describes when the test registers a change, not why the reported figure is flattered. A concerns an
accrual measure, and the covenant is computed on cash.

**MCQ 15.1-G `[15.1.3 · Evaluation]`** The operations director's monthly pack reports availability against
the 95.0 % guarantee, and the finance director wants the covenant regime on it. Kestrel's thresholds bind
at **1.9103 %** of `CFADS` (the 1.25× distribution condition) and **5.8339 %** (the 1.20× covenant), and
translate into availability floors of **93.0161 %** and **88.9415 %**. What belongs on the operations
dashboard?
- A. the 1.20× covenant in ratio terms, because that is the test the facility agreement contains and the one
  a default turns on
- B. both thresholds as availability floors — 93.0161 % and 88.9415 % — led by the distribution condition,
  because it binds first and availability is the quantity the operator actually controls ✅
- C. the volume tolerance of **1,862,188.62 m³**, **18.6219 %** of output, because volume is the larger
  revenue line
- D. the covenant headroom of **372,437.72** in cash, because cash is the unit the waterfall works in

*Rationale:* A trigger becomes a control only when it is expressed in the units of the person who can move
it, and the translation runs ratio → cash → driver (15.1.3); leading with the distribution condition
follows from its binding three times closer than the covenant. A hands an operations meeting a ratio it
cannot act on. C is correctly computed and useless as a control: a 19 % volume loss is not a plausible
consequence of membrane fouling whereas six availability points is, so ranking the drivers by revenue size
ranks them by the wrong property. D is Domain 10's translation and stops one step short of the dashboard —
it is the right figure for the finance pack and the wrong one for the operator.

### Self-check — KA 15.1

1. *Why does a deteriorating project report a flattered covenant ratio?* — Falling revenue
   releases working capital, which is added to `CFADS` in the year of decline and reversed in the
   year of recovery (15.1.2).
2. *State the lag structure of a rolling four-quarter covenant.* — It cannot register a
   deterioration for up to four quarters; the forward test exists to close that gap and failed
   for Kestrel four quarters earlier, at 1.0979.
3. *Which of Kestrel's three thresholds binds first, and by how much cash?* — The 1.25×
   distribution condition, at 121,955.96 or 1.9103 % of `CFADS`.

---

## Knowledge Area 15.2 — The cash waterfall in operation, reserves and distributions

*Topics: 15.2.1 the operating waterfall in priority order · 15.2.2 reserve top-ups and the block
account · 15.2.3 the distributable amount that falls out · 15.2.4 what a distribution drought
costs equity.*

### 15.2.1 The operating waterfall in priority order

**Definition.** The **operating cash waterfall** is the contractual priority order in which each
period's project cash is applied. Domain 10 (KA 10.3.3) built its top — operating costs, taxes,
senior debt service — and stated the principle that reserve top-ups rank above distributions.
This is the rest of it, in the order the accounts actually work:

```
Revenue collected into the proceeds account
  1  operating and maintenance costs, insurance, SPV administration
  2  cash taxes
  3  senior fees, agency and account-bank charges
  4  senior interest
  5  senior scheduled principal
  6  debt service reserve top-up, to the required balance
  7  maintenance reserve top-up, to the required balance
  8  handback / decommissioning reserve top-up, from the defined year
  9  mandatory prepayment and cash sweep, if triggered
 10  subordinated and shareholder debt: interest, then principal
 11  distributions to equity — only if every distribution condition is satisfied
 12  otherwise: retained in the distribution-block account
```

Three features of the order do the work. **Steps 1 to 5 are obligations**; steps 6 to 9 are
**restorations**, which is a different thing — they do not fall due to a counterparty but they
have first call on cash and no negotiating partner. **Steps 10 and 11 are permissions**: the
economic meaning of subordination is not a lower ranking in an insolvency but the absence of an
entitlement in the ordinary course. And **step 12 is where the sponsor's money goes when a test
fails** — not away, but into an account it cannot reach, which is the mechanism the rest of this
knowledge area is about.

Kestrel's reserve requirements: a **debt service reserve** of six months' forward debt service,
**2,504,817.62** (constant, because a level annuity's next six months never change), and a
**maintenance reserve** funded at **600,000 per year** as the levelised charge for a
**3,000,000** membrane replacement every five years. There is no subordinated debt: the
18,000,000 of equity is subscribed as share capital, so step 10 is empty and step 11 is where all
sponsor cash arises.

### 15.2.2 Reserve top-ups and the block account

**Definition.** A **top-up** is the application of period cash to restore a reserve to its
required balance; the **distribution-block account** is where cash that fails a distribution test
is held. The pair is what makes the waterfall a liquidity system rather than a payment order,
because cash trapped in one period funds a restoration in another.

**Worked example 15.2.2 — six years of Kestrel's waterfall, and what the block account paid for.**

1. **Setup.** Kestrel's actual outturn over six operating years, built from the drivers of
   15.1.2. Year two suffers an unhedged power-price shock (energy 0.26 → 0.40/m³) with
   availability at 92.0 % on 9,500,000 m³; year three worsens as membranes foul (88.0 %,
   9,000,000 m³, energy 0.42); year four is the remediation year (94.5 %, 9,900,000 m³, energy
   hedged at 0.32) in which the membrane replacement is brought forward from year five; years
   five and six run at 95.0 % and 10,000,000 m³ with energy at 0.30. Debt service is the level
   5,009,635.23; the maintenance-reserve charge is 600,000 a year; the distribution condition
   requires 1.25× on both a backward and a forward test.
2. **Formula.** For each year: residual = `CFADS` − debt service − maintenance-reserve top-up.
   If the distribution conditions are satisfied, distribution = residual + block-account balance,
   and the block account resets to nil; otherwise the residual is added to the block account. A
   top-up that the year's residual cannot fund is drawn from the block account.
3. **Substitution.** Year three: `5,202,936.48 − 5,009,635.23 = 193,301.25`, against a 600,000
   top-up, leaving `600,000 − 193,301.25 = 406,698.75` to be drawn from the block account. Year
   five: `6,480,627.99 − 5,009,635.23 − 600,000 = 870,992.76`, plus the block-account balance of
   1,206,931.59.
4. **Result.**

| Year | `CFADS` | Backward `DSCR` | Residual after service and reserve | Distribution | Block account, closing |
|---|---|---|---|---|---|
| 1 | 6,384,000.00 | 1.2743 | 774,364.77 | **nil** | 774,364.77 |
| 2 | 5,963,894.11 | **1.1905** | 354,258.88 | **nil** | 1,128,623.65 |
| 3 | 5,202,936.48 | **1.0386** | (406,698.75) | **nil** | 721,924.90 |
| 4 | 6,094,641.91 | 1.2166 | 485,006.68 | **nil** | 1,206,931.59 |
| 5 | 6,480,627.99 | 1.2936 | 870,992.76 | **2,077,924.35** | nil |
| 6 | 6,495,588.34 | 1.2966 | 885,953.11 | **885,953.11** | nil |

   Year four additionally required a sponsor injection of **1,500,000**: the membrane replacement
   brought forward cost 3,000,000 against a maintenance-reserve balance of 1,800,000, a gap of
   **1,200,000**, with the remaining **300,000** restoring the reserve.
5. **Interpretation.** Four observations, in ascending order of professional value. First, **the
   debt service reserve was never drawn.** The worst year's `CFADS` of 5,202,936.48 exceeded debt
   service by 193,301.25; the 2,504,817.62 reserve sat untouched throughout. Everything that went
   wrong went wrong above the reserve's pay-line, which is the general case: reserves are sized
   for the failure that does not happen, and the failures that do happen bind on the distribution
   test. Second, **year one is the paradox this domain exists to teach.** The project met its
   covenant at 1.2743 and passed the backward distribution test at 1.25×, yet paid nothing,
   because the forward test failed on the sponsors' own re-forecast (15.1.4). There was
   774,364.77 of cash, contractually available to nobody. Third, **the block account is not a
   penalty box; it is pre-funding.** The 406,698.75 that year three could not contribute to the
   maintenance reserve came out of the cash years one and two had been forced to retain — the
   trapped dividend paid for the reserve shortfall that the deterioration caused. A sponsor that
   had successfully argued for a weaker distribution test in negotiation would have paid that
   774,364.77 out in year one and been asked for it back, as new equity, in year three. Fourth,
   **the reserve top-up is the ranking that hurts.** In year three the maintenance charge consumed
   the entire residual and more; had the replacement not been brought forward, the same cash would
   still have been unavailable. A distribution forecast that models debt service but not reserve
   restoration overstates equity cash by the full reserve charge in every year — 600,000 a year
   for Kestrel, which is 77.5 % of the base-case dividend.

> **Fig 15.2.1 — Kestrel's operating waterfall and the block account it fills.** Two panels.
> Left: stacked bars for operating years 1–6, each bar showing that year's `CFADS` allocated in
> contractual priority — senior interest, senior scheduled principal, maintenance-reserve top-up,
> then either cash trapped by the distribution test or cash distributed to equity — with the
> backward `DSCR` annotated above each bar (1.2743 · 1.1905 · 1.0386 · 1.2166 · 1.2936 · 1.2966)
> and horizontal dashed references at the three thresholds in cash (1.25× distribution 6,262,044;
> 1.20× covenant 6,011,562; 1.15× lock-up 5,761,081). Year three is annotated "reserve short
> 406,699 — funded from the block account". Right: the closing balance of the distribution-block
> account (774,364.77 · 1,128,623.65 · 721,924.90 · 1,206,931.59 · nil · nil), with the year-three
> drawdown and the year-five release into the 2,077,924.35 distribution marked in crimson.
> Source: PCI original. Alt text: a stacked bar chart of six operating years showing each year's
> cash consumed first by interest and principal and then by a reserve charge, with the residual
> trapped in years one to four and released in year five, beside a line chart of the trapped
> balance rising, dipping to fund a reserve shortfall, rising again and falling to zero.

### 15.2.3 The distributable amount that falls out

**Definition.** The **distributable amount** is what remains after every prior step, and in a
level base case it is a residual with a closed form:

```
Distributable = CFADS − debt service − reserve top-ups   (subject to every distribution condition)
```

For Kestrel's base case: `6,384,000 − 5,009,635.23 − 600,000 =` **774,364.77** per year, a cash
yield of **4.3020 %** on the 18,000,000 subscribed. That number deserves to be sat with, because
it is the honest shape of project-finance equity and it surprises people. A project appraised at
a Domain 4 `NPV` of +16,179,360 with an `IRR` of 12.19 % pays its owners a 4.30 % cash yield for
twelve years. The reason is arithmetic, not disappointment: while the loan is outstanding, debt
service consumes 78.5 % of `CFADS`, and equity is the residual claimant on a leveraged asset. Once
the loan retires the same `CFADS` produces **5,784,000** a year — a **32.13 %** yield on the
original subscription — and from year sixteen the handback charge of **697,844.05** (KA 15.4)
reduces it to **5,086,155.95**, before rising to **5,686,155.95** once the maintenance-reserve
charge ends after year twenty.

The whole 25-year base-case profile, discounted at the 8.0 % the board owns, gives an equity
`NPV` of **+5,027,733.03** and an **equity `IRR` of 9.8591 %**, on total distributions of
**80,505,936.71** — a money multiple of **4.4726×**. Two cautions belong beside those figures.
They are **not** Domain 4's appraisal restated: Domain 4 valued the unlevered project over a
15-year horizon on a different net-inflow basis, and comparing the two numbers as though one
should reproduce the other is a category error that appears in real board packs. And the 9.8591 %
is **back-end loaded to an extreme degree** — **88.5 %** of total distributions (71,213,559.47 of
80,505,936.71) fall in the last thirteen years — which is why the exit and refinancing questions of
KA 15.3 and 15.4 dominate sponsor behaviour in a way that a level-yield instrument would not.

### 15.2.4 What a distribution drought costs equity

**Definition.** A **distribution drought** is a run of periods in which cash is generated but not
distributable. Its cost to equity has two components that must be separated, because only one of
them is a performance failure.

**Worked example 15.2.4 — decomposing Kestrel's six-year cost to equity.**

1. **Setup.** Base case: 774,364.77 distributed in each of years 1–6. Actual: nil in years 1–4,
   2,077,924.35 in year 5, 885,953.11 in year 6, less a 1,500,000 injection at the end of year 4.
   Discount rate 8.0 %.
2. **Formula.** Present value of each profile at 8 %; the difference is the total cost. The cash
   cost is the undiscounted difference; the balance is the timing cost.
3. **Substitution.** Base `774,364.77 × AF(0.08, 6)`. Actual
   `2,077,924.35 × DF(0.08, 5) + 885,953.11 × DF(0.08, 6) − 1,500,000 × DF(0.08, 4)`.
4. **Result.** Base-case present value **3,579,795.15**. Actual present value **1,972,501.14**,
   less the injection's **1,102,544.78**, giving **869,956.36**. **Loss of present value
   2,709,838.79.** The undiscounted cash difference is **3,182,311.16**, which reconciles exactly:
   the six-year `CFADS` shortfall of **1,682,311.16** (36,621,688.84 actual against 38,304,000
   base) plus the **1,500,000** injection.
5. **Interpretation.** The reconciliation is the point. Of the 3,182,311.16 of cash equity lost,
   only **52.9 %** is a trading shortfall; **47.1 %** is a capital call caused by a reserve that
   was not big enough for an event brought forward by one year. That decomposition changes who is
   accountable for what: the operator owns the 1,682,311.16, and the finance director owns the
   1,500,000, because a maintenance reserve sized on the assumption that a five-year replacement
   cycle will run to five years is a financing decision, not an engineering one. Note also that
   the present-value loss of 2,709,838.79 is **less** than the cash loss of 3,182,311.16, because
   **2,963,877.46** of distributions were ultimately paid — four and five years late rather than
   never. Deferral is expensive and it is not destruction, and conflating the two produces the
   commoner of the two errors in distressed-project board papers: treating a lock-up as a
   write-off. The other error is the mirror image, treating it as a timing matter of no
   consequence, and the arithmetic settles that one decisively: **2,709,838.79 is 53.90 % of the
   entire 25-year equity `NPV` of 5,027,733.03.** Six poor years out of twenty-five removed more
   than half the base-case equity value, because they fell at the front of the profile where
   discounting bites least.

### AI in this KA

**Where it earns its place.** Waterfall computation is deterministic, rule-heavy and tedious —
ideal machine work, and the natural home of a well-tested calculation engine. Two applications
add real value beyond speed. **Forward waterfall projection under scenarios**: running the
distribution conditions, block-account mechanics and reserve top-ups across hundreds of driver
scenarios to produce a *distribution* distribution rather than a point forecast, which is the
only honest answer to "when will we be paid?". And **block-account and reserve reconciliation**:
tying account-bank statements to the modelled balances every period, which catches the
operational errors — a top-up made from the wrong account, a release taken before the test date —
that no ratio would reveal.

**Where it must not go.** A model must not determine whether a distribution is permitted. That
determination rests on the satisfaction of every condition, including qualitative ones ("no
default subsisting", "no material adverse change"), and it is a decision with legal consequence
for the officers who authorise the payment. Nor should generated code implement the waterfall
without independent reconstruction: the ordering of steps 6 to 11 is where an assistant will
confidently produce a defensible-looking but wrong sequence, because the *conventional* order and
the *documented* order differ in most facilities by at least one step.

**Verification, concretely.** Reconstruct one period's waterfall by hand from the accounts and
tie every step to its clause. Prove the closing identity in every period — opening balances plus
receipts less applications equals closing balances, account by account. Test the engine on the
three adversarial cases: a period where the residual cannot fund a top-up (Kestrel's year three);
a period where a test fails on the forward leg only (year one); and a period where a release and
a top-up fall due simultaneously. An engine that has not been run against those three has not
been tested.

### Key terms — KA 15.2

| Term | Meaning |
|---|---|
| **Operating cash waterfall** | The contractual priority order for applying each period's project cash. |
| **Top-up / restoration** | Application of cash to restore a reserve to its required balance; ranks above distributions. |
| **Distribution-block account** | Where cash that fails a distribution test is held; a pre-funding mechanism, not a penalty. |
| **Distributable amount** | `CFADS` less debt service less reserve top-ups, subject to every distribution condition. |
| **Cash yield** | Distribution ÷ equity subscribed — 4.3020 % for Kestrel while the loan runs, 32.13 % after. |
| **Distribution drought** | A run of periods generating cash that is not distributable; costs equity in cash and in timing. |

### Sample MCQs — KA 15.2

**MCQ 15.2-A `[15.2.3 · Application]`** Kestrel's `CFADS` is 6,384,000, debt service 5,009,635.23
and the maintenance-reserve charge 600,000. The base-case distributable amount is:
- A. USD 1,374,364.77
- B. USD 774,364.77 ✅
- C. USD 6,384,000
- D. USD 774,364.77 less the six-month debt service reserve of 2,504,817.62

*Rationale:* `6,384,000 − 5,009,635.23 − 600,000 = 774,364.77`. A omits the reserve top-up, which
is the single most common distribution-forecasting error (15.2.2); C is `CFADS` itself; D
double-counts a reserve that was funded at close and requires no top-up while debt service is
level.

**MCQ 15.2-B `[15.2.2 · Analysis]`** In Kestrel's year one the backward `DSCR` is 1.2743 against a
1.20× covenant and a 1.25× distribution condition, and 774,364.77 of cash remains after debt
service and the reserve charge. Nothing is distributed. The correct explanation is:
- A. the covenant was breached
- B. the distribution condition also requires a forward-looking test, which failed at 1.0979 on the sponsors' re-forecast ✅
- C. the debt service reserve had to be topped up first
- D. distributions are prohibited in the first operating year

*Rationale:* Both backward tests passed; the forward leg of the distribution condition failed
(15.1.4, 15.2.2). A is contradicted by 1.2743 > 1.20; C is false — a level annuity's six-month
reserve never needs topping up; D invents a term.

**MCQ 15.2-C `[15.2.4 · Analysis]`** Kestrel's equity lost 3,182,311.16 of cash over six years but
only 2,709,838.79 of present value at 8 %. The reason is:
- A. an arithmetic inconsistency between the two measures
- B. 1,463,877.46 of the withheld cash was eventually distributed, so part of the loss is deferral rather than destruction ✅
- C. the discount rate should have been the loan rate of 6 %
- D. the 1,500,000 injection should not be counted as a cost to equity

*Rationale:* Deferral costs the time value, not the principal (15.2.4). C would change both
figures without changing the relationship; D is wrong because an injection is cash equity provides
and does not get back other than through later distributions already counted.

**MCQ 15.2-D `[15.2.1 · Recall]`** In the operating waterfall, a handback or decommissioning
reserve top-up ranks:
- A. above senior debt service, being a statutory obligation
- B. below distributions to equity, being a long-dated liability
- C. above distributions to equity and below senior debt service, alongside the other reserve restorations ✅
- D. at the same level as cash taxes

*Rationale:* Restorations sit between obligations and permissions (15.2.1). A inverts the security
architecture; B is the error that leaves a handback obligation unfunded (Case study B); D confuses
a reserve with a period cost.

**MCQ 15.2-E `[15.2.4 · Evaluation]`** Two draft board papers describe the same six operating years.
Paper 1 writes the **3,182,311.16** off as lost equity value. Paper 2 calls it a timing matter of no
consequence, since **2,963,877.46** of distributions were ultimately paid. Which position is sounder,
and what should the paper say?
- A. paper 1 — cash not received when it was due is value lost, and calling it timing is how distressed projects are misreported
- B. neither: the present-value loss is 2,709,838.79 — **53.90 %** of the entire 25-year equity `NPV` of 5,027,733.03 — so deferral is expensive without being destruction, and the paper should split the 3,182,311.16 into 1,682,311.16 of trading shortfall and a 1,500,000 capital call with different owners ✅
- C. paper 2 — the block account is pre-funding rather than a penalty, so on the facts nothing was lost
- D. paper 1, provided the loss is measured at the 6 % loan rate rather than the 8 % equity rate

*Rationale:* Both papers state a half-truth and the arithmetic disposes of each: 2,963,877.46 came
back, so it is not a write-off, and 2,709,838.79 is more than half the project's whole equity value,
so it is not immaterial (15.2.4). C quotes a correct description of the mechanism (15.2.2) to reach a
false conclusion about value — pre-funding still costs the time value of money and, here, a
1,500,000 injection. D changes the discount rate to change the answer, which is the least defensible
move available; the rate belongs to the board, not to the conclusion it wants.

**MCQ 15.2-F `[15.2.3 · Comprehension]`** A shareholder asks how a project appraised at a 12.19 %
`IRR` can pay a cash yield of only **4.3020 %**. The clearest explanation is:
- A. the appraisal was optimistic, and the cash yield is the figure to believe
- B. while the loan runs, debt service consumes 78.47 % of `CFADS`, so equity is the residual claimant on a leveraged asset; the same `CFADS` yields 32.13 % once the loan retires ✅
- C. the difference is the 600,000 maintenance-reserve charge, which the appraisal did not carry
- D. an `IRR` and a cash yield measure the same quantity on different bases, so one of the two has been computed incorrectly

*Rationale:* The gap is leverage and timing, not error or optimism (15.2.3). C names a real charge
that is genuinely large against the dividend — 77.5 % of it — but cannot explain the shape: add it
back and the yield is still only 7.6354 %. D is the misconception the topic exists to remove: a
return over a whole life and a level annual yield are different measures, and both are correct. A
treats a leveraged residual as evidence about the appraisal.

**MCQ 15.2-G `[15.2.2 · Evaluation]`** A sponsor reviewing the term sheet argues for a distribution
condition set at the **1.20×** covenant level rather than **1.25×**, on the ground that a test stricter
than the covenant traps cash for no lender benefit. On Kestrel's outturn the base-case distributable amount
is **774,364.77** a year, year one's was trapped in full, and year three's residual after debt service was
**193,301.25** against a 600,000 maintenance-reserve charge — a shortfall of **406,698.75** drawn from the
block account. Is the argument sound?
- A. yes — cash in shareholders' hands is worth more than the same cash later, the block account earns
  equity nothing, and the lenders already have a covenant
- B. not on these facts: the year-one dividend the 1.25× test trapped is what funded the 406,698.75
  shortfall — **52.52 %** of the 774,364.77 — so a weaker test would have paid that cash out and then
  called it back as new equity; the block account is pre-funding rather than a penalty, and the real
  question is whether the sponsor would rather hold the cash or avoid the call ✅
- C. yes — a distribution condition and a covenant test the same ratio on the same cash, so the stricter of
  the two adds nothing
- D. no — 1.25× is a market standard for a contracted water project and is not negotiable

*Rationale:* Cash trapped by a distribution test is not lost but pre-committed, and here it paid for a
restoration that ranks above distributions and has no negotiating partner (15.2.1, 15.2.2). A is the
strongest form of the sponsor's case and names a real cost — deferral is expensive — but it prices the
timing benefit while ignoring the capital call the same cash would have funded. C is the misconception the
domain opens by correcting: the distribution condition is a **permission**, tested forwards as well as
backwards, and it binds at 1.9103 % of `CFADS` where the covenant binds at 5.8339 %. D substitutes an
assertion about the market for an analysis; nothing in a facility is un-negotiable at a price.

### Self-check — KA 15.2

1. *Why was Kestrel's debt service reserve never drawn across six deteriorating years?* — Worst-year
   `CFADS` of 5,202,936.48 still exceeded debt service by 193,301.25; the failures bound on the
   distribution test, above the reserve's pay-line.
2. *What did the block account actually pay for?* — The 406,698.75 maintenance-reserve shortfall in
   year three, out of dividends trapped in years one and two.
3. *State Kestrel's base-case cash yield before and after the loan retires.* — 4.3020 % on
   774,364.77 while the loan runs; 32.13 % on 5,784,000 afterwards.

---

## Knowledge Area 15.3 — Refinancing, waivers and amendments

*Topics: 15.3.1 why operating-phase debt is mispriced · 15.3.2 pricing a refinancing ·
15.3.3 waiver, cure and amendment, priced against one another · 15.3.4 the cash sweep as the
price of consent.*

### 15.3.1 Why operating-phase debt is mispriced

**Definition.** **Refinancing** is the replacement of an existing facility with a new one, and in
project finance its economic driver is specific: the original facility was priced for a risk that
has since been retired. A construction-phase margin compensates a lender for completion risk,
technology risk and the possibility of a project that never operates. Once the completion tests of
Domain 14 are passed and two or three years of operating history exist, none of those risks
remain, but the margin does — because a term loan's price is fixed at signing and the market's
view is not.

Three structural features make the opportunity larger than a margin comparison suggests. **The
tail**: Domain 10 (KA 10.2.2) measured Kestrel's `PLCR` at 1.9431 against an `LLCR` of 1.2743,
quantifying thirteen years of project life beyond the loan's maturity; a refinancing can lend
against part of that tail, which no margin reduction can replicate. **The amortisation profile**:
a schedule sculpted for a construction-phase risk view is usually faster than an operating asset
requires. And **the covenant package**: the ratios, reserves and distribution conditions
negotiated when the project was a drawing can be reset against observed performance. The
professional discipline is therefore to price the *whole* new term sheet, not the margin — and the
worked example exists to show how badly the margin alone misleads.

### 15.3.2 Pricing a refinancing

**Worked example 15.3.2 — Kestrel refinances at the end of year five.**

1. **Setup.** Senior debt outstanding **27,965,694.77** with **7 years** remaining at an all-in
   **6.00 %** (a 3.00 % base rate swapped fixed for the full tenor, plus a 300 basis point
   margin). Annual service 5,009,635.23. The market will lend against the operating asset at a
   **145 basis point** margin, an all-in **4.45 %**. Costs: the interest-rate swap must be broken
   — the seven-year market swap rate has fallen to **2.20 %**, so the borrower is paying 80 basis
   points above market on the amortising notional; an arrangement fee of **1.00 %**; a prepayment
   fee under the existing facility of **0.50 %**; and **320,000** of legal, model-audit,
   technical-adviser and agency cost. Two structures are on offer: the same 7-year tenor, or a
   **10-year** tenor lending three years into the tail. Discount incremental cash to equity at
   8.0 %.
2. **Formula.** New instalment = outstanding ÷ `AF(new rate, tenor)`. Incremental cash to equity
   in each year = old service ceasing less new service arising. Swap break cost =
   Σ (old fixed − market) × notional in each remaining year, discounted at the market rate. Net
   present value = present value of incremental cash − total transaction cost.
3. **Substitution.** Break cost: `0.80 % ×` the seven remaining opening balances
   (27,965,694.77 · 24,634,001.23 · 21,102,406.07 · 17,358,915.21 · 13,390,814.89 ·
   9,184,628.55 · 4,726,071.04), each discounted at 2.20 % → 218,909.55 + 188,678.82 +
   158,150.03 + 127,294.31 + 96,082.11 + 64,483.14 + 32,466.39. Fees:
   `0.0100 × 27,965,694.77` and `0.0050 × 27,965,694.77`. Instalments:
   `27,965,694.77 ÷ AF(0.0445, 7)` and `÷ AF(0.0445, 10)`.
4. **Result.** Swap break cost **886,064.34**; arrangement fee **279,656.95**; prepayment fee
   **139,828.47**; advisers **320,000** — **total transaction cost 1,625,549.77**.

| Option | New instalment | Base-case `DSCR` | PV of incremental cash at 8 % | Net present value |
|---|---|---|---|---|
| 4.45 % over 7 years | **4,737,139.41** | 1.3476 | 1,418,714.07 | **(206,835.69)** |
| 4.45 % over 10 years | **3,525,588.23** | 1.8108 | 2,425,030.89 | **+799,481.12** |

   Breakeven all-in rate on the 7-year option: **4.2206 %**, a margin of **122.06 basis points**.
   Breakeven all-in rate on the 10-year option: **5.1308 %**.
5. **Interpretation.** The headline is the trap. **A refinancing that cuts the margin by 155 basis
   points destroys 206,835.69 of value.** The reason is that the margin saving is a *thin annual
   flow on a rapidly amortising balance* — 272,495.82 in year one, falling every year — while the
   costs are a *thick lump today*, and the swap break alone consumes 54.5 % of them. The
   professional discipline that follows is to solve for the breakeven before opening a
   negotiation: Kestrel's margin must fall by **177.94 basis points**, not 155, to pay for the
   transaction, so the market's best offer is **22.94 basis points short**. That single figure is
   the whole mandate brief. What rescues the transaction is not price but **structure**: extending
   three years into the tail turns the same margin into +799,481.12, and the decomposition shows
   why — of the 2,425,030.89 of present value, **1,418,714.07 is the margin, 586,108.78 is the
   tenor measured alone, and 420,208.04 is the interaction** between the two, which exists only
   because the extension is priced at the lower rate. None of the three components pays for the
   transaction by itself. Two cautions complete the analysis. The discount rate matters to
   magnitude but not to sign here, and a refinancing paper should state it and test it, because
   debt-service savings discounted at the cost of debt look larger than the same savings
   discounted at the cost of equity and neither is wrong in the abstract. And the extension is not
   free of consequence: the `LLCR` rises to **1.8108** and the `PLCR` to **2.9824**, coverage a
   lender will not simply hand over — which is what KA 15.3.4 is about.

### 15.3.3 Waiver, cure and amendment, priced against one another

**Definitions.** A **waiver** is the lenders' consent to disregard a specific breach, usually for
a fee. An **equity cure** is the sponsors' contractual right to inject cash treated as `CFADS` (or
applied to prepayment) so that a ratio is restored; facilities cap the number available over the
loan's life. An **amendment** changes the terms — most commonly resetting a covenant level for a
defined period — in exchange for a fee, a margin uplift and usually tighter controls.

Kestrel breaches at two successive test dates, and the two situations look identical and are not.

| | End of year 2 | End of year 3 |
|---|---|---|
| `CFADS` | 5,963,894.11 | 5,202,936.48 |
| Backward `DSCR` | **1.1905** | **1.0386** |
| Covenant cash requirement at 1.20× | 6,011,562.28 | 6,011,562.28 |
| **Equity cure required** | **47,668.16** | **808,625.80** |
| Waiver fee at 15 basis points on debt outstanding | **55,307.03** | — |
| Amendment: 25 bp fee on 36,871,351.43 | **92,178.38** | — |
| Amendment: present value at 8 % of a 40 bp margin uplift on years 3–12 | **651,258.24** | — |
| **Total amendment cost** | **743,436.62** | — |

**Worked example 15.3.3 — the cheapest option that was the wrong one.**

1. **Setup.** At the year-two test date the breach is marginal: `DSCR` 1.1905 against 1.20. The
   sponsors hold **two** equity cures over the loan's life and have used none. Three responses are
   available on the numbers above. At the year-three date, nine months later, the breach is
   severe.
2. **Formula.** Cure = covenant cash requirement − actual `CFADS`. Waiver fee = basis points ×
   debt outstanding. Amendment cost = fee + Σ (margin uplift × opening balance in each remaining
   year), discounted at 8 %.
3. **Substitution.** Year two: `6,011,562.28 − 5,963,894.11`. Year three:
   `6,011,562.28 − 5,202,936.48`. Uplift: `0.0040 ×` each opening balance from year three to year
   twelve (147,485.41 · 136,295.99 · 124,435.21 · 111,862.78 · 98,536.00 · 84,409.62 · 69,435.66 ·
   53,563.26 · 36,738.51 · 18,904.28), discounted at 8 %.
4. **Result.** The year-two cure of **47,668.16** is the cheapest cash response — **7,638.87 less
   than the waiver** and 695,768.46 less than the amendment. The year-three cure would cost
   **808,625.80**, which is **808,625.80 ÷ 47,668.16 = 17.0 times** the year-two requirement; put
   the other way, the year-two breach was **5.8950 %** of the year-three breach. And the amendment
   at 743,436.62 is within **5,290.97** — seven-tenths of one per cent — of the present value of
   that year-three cure (**748,727.59** discounted one year at 8 %).
5. **Interpretation.** Two conclusions, and the second is the one that separates a practitioner
   from an analyst. First, on price, **the amendment and the second cure are indistinguishable**:
   a difference of 5,290.97 on obligations of three-quarters of a million is inside the error of
   any forecast underlying either, so the decision cannot be taken on cost and anyone who presents
   it as a cost comparison has not understood it. The real differences are structural — the
   amendment resets four test dates and costs 40 basis points for the remaining life; the cure
   fixes one date, leaves the covenant profile intact, and consumes an irreplaceable option. And
   note an asymmetry the raw comparison hides: **a cure is an investment and a fee is not.** Cure
   cash enters the project and is recovered through later distributions; a waiver fee and a margin
   uplift leave permanently. On a like-for-like basis the cure is better than 5,290.97 suggests.
   Second, on the actual decision, **spending a cure on the year-two breach was the error.** The
   sponsors paid 47,668.16 and preserved 7,638.87 of fee, and in doing so consumed half of a
   facility that nine months later would have been worth 808,625.80 of cover. The correct posture
   is a rule, not a calculation: **cure rights are scarce options and must be spent in proportion
   to what they buy** — a marginal breach is waived, and the incremental cost of doing so
   (7,638.87, or **0.9447 %** of the value of the option preserved) is the cheapest insurance
   premium in this book. The reflex to reach for the cheapest cash response at each test date, in
   isolation, is exactly how sponsors arrive at the third breach with no options and no
   negotiating position.

A third caution belongs here. **An amendment must be sized against the stressed case, not the
current one.** A covenant reset to 1.10× negotiated at the year-two date, on the then-current
1.1905, would have been breached again at year three's 1.0386 — a second breach, a second
negotiation and a second fee, from a materially worse position. The amendment Kestrel actually
took reset the covenant to 1.00× for two test dates before stepping to 1.10× and 1.20×, which is
what "sized against the stress" means in practice.

### 15.3.4 The cash sweep as the price of consent

**Definition.** A **cash sweep** applies a defined share of cash that would otherwise be
distributed to mandatory prepayment. Domain 10 (KA 10.1.3) named it; here it is priced, because a
sweep is how lenders take a share of a refinancing or amendment gain, and it is routinely
under-priced by sponsors who read it as a liquidity provision rather than a transfer of value.

Take the successful 10-year refinancing of 15.3.2. Post-refinancing distributable cash rises from
774,364.77 to **2,258,411.77** a year — a 12.5467 % cash yield where there had been 4.3020 %. A
lender asked to lend three years further into the tail will want a share of that, and the standard
ask is **50 % of cash available for distribution** applied to prepayment.

| Structure | Loan retires | Equity `IRR` | Equity `NPV` at 8 % |
|---|---|---|---|
| No refinancing | year 13 | 9.8591 % | 5,027,733.03 |
| Refinanced, no sweep, net of the 1,625,549.77 costs | year 15 | — | **5,571,846.45** |
| Refinanced, 50 % sweep, net of costs | year 13 | — | **4,925,518.59** |

The sweep costs equity **646,327.86** of present value and 39.76 basis points of `IRR` on the
pre-cost profile, and it removes two years of debt life for the lender. Against a refinancing gain
of **544,113.42** at time zero (the 799,481.12 net present value of 15.3.2 discounted five years
at 8 %), that is decisive: **with a 50 % sweep the refinancing is worth 102,214.44 less to equity
than doing nothing.** The **breakeven sweep share is 40.3334 %** — above it, the transaction
destroys equity value.

The professional content is a habit rather than a formula. **Never evaluate a refinancing on the
margin and the tenor and then negotiate the sweep separately.** The sweep is part of the price,
its effect is second-order in appearance and first-order in magnitude, and the breakeven share is
computable before the first meeting. A sponsor who walks into a refinancing knowing that 40.33 %
is its indifference point negotiates from a position no amount of relationship can substitute for.

### AI in this KA

**Where it earns its place.** Three uses are strong. **Solving for breakevens** across the
transaction's whole parameter space — margin, tenor, fee, sweep share, break cost — is exactly the
sort of repeated root-finding a machine should do, and it produces the single most useful output
of a refinancing analysis, which is the boundary of the acceptable rather than a point answer.
**Term-sheet comparison** across competing offers, normalising to a common present value, catches
the option that looks cheapest on margin and is not. **Amendment-history extraction** across a
portfolio — every covenant reset, fee and margin uplift ever granted, with dates — builds the
institutional memory that makes the next negotiation cheaper.

**Where it must not go.** A model must not determine whether a cure right remains available, or
how many, or on what conditions. That is a reading of the facility's cure clause and its history
of use, and it is the specific fact on which the year-two decision above turned. Nor should AI
draft the request that goes to a lender: the tone, sequencing and candour of a waiver request is a
relationship instrument, and Domain 10 (KA 10.4.4) established that the relationship is the asset
that determines the terms of every future consent.

**Verification, concretely.** Recompute the break cost by hand on the notional schedule and tie
each period's notional to the amortisation table (Domain 3's checks). Confirm that the incremental
cash-flow set is complete — old service ceasing, new service arising, fees, and the sweep — because
the characteristic machine error is an omitted leg, not a wrong multiplication. Re-derive the
breakeven independently and check that the base case sits on the correct side of it. Where a model
has read an amendment or waiver clause, check its reading against the document before anyone quotes
it.

### Key terms — KA 15.3

| Term | Meaning |
|---|---|
| **Refinancing** | Replacement of a facility priced for a risk since retired; value comes from margin, tenor and covenant reset together. |
| **Swap break cost** | Present value of the difference between the contracted fixed rate and the market rate over the remaining notional profile. |
| **Breakeven margin** | The new all-in rate at which a refinancing's net present value is nil — 4.2206 % for Kestrel's 7-year option. |
| **Waiver** | Consent to disregard a specific breach, usually for a fee. |
| **Equity cure** | Contractual injection restoring a ratio; a scarce option, capped in number. |
| **Amendment** | A change of terms — usually a covenant reset — for a fee, a margin uplift and tighter controls. |
| **Cash sweep** | A defined share of distributable cash applied to mandatory prepayment; part of the price of consent. |

### Sample MCQs — KA 15.3

**MCQ 15.3-A `[15.3.2 · Application]`** Kestrel's 7-year refinancing at 4.45 % saves 272,495.82 a
year with a present value of 1,418,714.07 against total costs of 1,625,549.77. The correct
conclusion is:
- A. proceed — a 155 basis point margin saving is material
- B. reject on these terms: net present value is (206,835.69), and the margin must fall 177.94 basis points to break even ✅
- C. proceed — the `DSCR` improves from 1.2743 to 1.3476
- D. reject — refinancing an operating asset is never economic

*Rationale:* `1,418,714.07 − 1,625,549.77 = (206,835.69)`; the breakeven all-in rate is 4.2206 %,
a 122.06 bp margin (15.3.2). A prices the headline and not the transaction; C cites a coverage
improvement that has no cash value to equity; D overgeneralises — the 10-year variant is worth
+799,481.12.

**MCQ 15.3-B `[15.3.2 · Analysis]`** Of the 2,425,030.89 present value in Kestrel's 10-year
refinancing, how much is attributable to the margin reduction measured alone?
- A. USD 2,425,030.89
- B. USD 1,418,714.07 ✅
- C. USD 586,108.78
- D. USD 420,208.04

*Rationale:* The margin component is the 7-year option's present value (15.3.2). A is the total,
which treats the whole gain as margin; C is the tenor component measured alone; D is the
interaction term that exists only because the extension is priced at the lower rate.

**MCQ 15.3-C `[15.3.3 · Analysis]`** The sponsors hold two equity cures. At a test date the
`DSCR` is 1.1905 against a 1.20× covenant, requiring a cure of 47,668.16; a waiver is available
for 55,307.03. The better decision, and why, is:
- A. cure — it is 7,638.87 cheaper in cash
- B. waive — paying 7,638.87 more preserves an option worth 808,625.80 of cover nine months later, or 0.9447 % of the value preserved ✅
- C. neither — a marginal breach may be disregarded
- D. cure — cure cash counts as `CFADS` and so also improves the reported ratio

*Rationale:* Cures are scarce options and must be spent in proportion to what they buy (15.3.3).
A optimises one test date in isolation, which is precisely the error; C invents a materiality
threshold facilities do not contain; D is true of the mechanics and irrelevant to the choice.

**MCQ 15.3-D `[15.3.4 · Application]`** The lenders consent to a 10-year refinancing worth
544,113.42 to equity at time zero, in exchange for a 50 % sweep of distributable cash that costs
equity 646,327.86 of present value. Equity should:
- A. accept — the tenor extension improves coverage to 1.8108
- B. decline or renegotiate: net of the sweep the transaction destroys 102,214.44, and the breakeven sweep share is 40.3334 % ✅
- C. accept — a sweep only accelerates repayment and does not reduce total distributions
- D. accept — the `IRR` cost of 39.76 basis points is immaterial

*Rationale:* The sweep exceeds the gain (15.3.4). A cites coverage, which is the lenders' benefit;
C is false — accelerated prepayment reduces the present value of distributions even where the
undiscounted total is similar; D judges materiality on the wrong measure, since 646,327.86 exceeds
the whole gain.

**MCQ 15.3-E `[15.3.2 · Evaluation]`** The 7-year option at 4.45 % all-in is worth **(206,835.69)**
against a breakeven all-in rate of **4.2206 %** — the market's offer is 22.94 basis points short. The
treasurer proposes going back to the banks to press for a further **25 basis points** of margin, and
nothing else. Assess that course.
- A. sound — 22.94 basis points is the entire gap and 25 basis points closes it, so the transaction becomes positive
- B. sound in arithmetic and the weaker course: at 4.20 % all-in the 7-year option is worth only about +18,542, while the same 4.45 % over 10 years is worth +799,481.12, so the negotiation belongs on tenor rather than on the last basis points of price ✅
- C. unsound — a refinancing with a negative net present value should be abandoned rather than renegotiated
- D. unsound — the breakeven rate depends on the discount rate chosen, so it cannot support a negotiating position

*Rationale:* A is arithmetically correct and is the trap: winning the price argument converts a small
loss into a small gain and leaves 780,939 of tenor value on the table, because the margin saving is a
thin flow on a rapidly amortising balance while the extension lends into the 1.9431 `PLCR` tail
(15.3.1, 15.3.2). C generalises one priced structure into a rejection of the transaction. D is a real
caution — the paper should state and test its rate — but the sign does not turn on it here, so it is
a reason to disclose an assumption, not a reason to abandon a computable negotiating position.

**MCQ 15.3-F `[15.3.3 · Evaluation]`** At the year-two test date the backward `DSCR` is **1.1905** against
a 1.20× covenant, and the forward view at the next test date, nine months out, is **1.0386**. The lenders
will grant an amendment, and the sponsors hold **two** equity cures over the loan's life, neither used. The
sponsor's adviser proposes resetting the covenant to **1.10×**, which clears the current ratio with margin
and prices below a deeper reset. The soundest recommendation is to:
- A. reset to 1.10× and keep a cure for the next test, which against that reset costs **307,662.27** — one
  of the two cures the sponsors hold, and cheaper in fee and margin uplift than a deeper reset
- B. reset to **1.00×** for two test dates before stepping to 1.10× and then 1.20×, so that the reset itself
  carries the deterioration and both cures survive it — a reset sized on the current ratio pays a fee for a
  covenant the next test breaches, and then spends an irreplaceable option to hold it ✅
- C. reset to 1.00× for the remaining life, since a covenant that has been breached has been shown to be set
  too high
- D. take a waiver at each test date instead: at **55,307.03** a waiver is far cheaper than the
  **743,436.62** the amendment costs

*Rationale:* An amendment must be sized against the **stressed** case rather than the current one, and the
deterioration was already visible (15.3.3). **A** is genuinely arguable and is not arithmetically wrong:
307,662.27 is **38.05 %** of the **808,625.80** the same test would cost as a cure against the unamended
1.20×, and a sponsor expecting a recovery may rationally buy the shallower reset and bridge one date. It is
the weaker course because a cure is an irreplaceable option whose value rises with the severity of the
breach it meets — the 17.0× asymmetry of 15.3.3 — so spending one on a breach the amendment could have
absorbed repeats the year-two error at a higher price, and the fee saved buys a covenant that fails at the
very next test. **C** gives away the control permanently to solve a defined period of weakness, and no
lender prices that kindly. **D** is right about a single date and wrong about a persistent profile: each
waiver is a fresh consent sought from a worse position, and the amendment Kestrel took resets four test
dates.

**MCQ 15.3-G `[15.3.1 · Comprehension]`** Which statement best explains why a refinancing opportunity
exists in an operating project financing at all?
- A. market interest rates fall over time, so any long-dated facility eventually becomes expensive
- B. the facility was priced for completion and technology risks that the completion tests retired, while a
  term loan's price is fixed at signing; and the project life beyond the loan's maturity is lendable
  capacity that no margin reduction can reach ✅
- C. an operating project generates more cash than one under construction, so it can carry more debt
- D. lenders are required to reprice a facility once the independent engineer certifies completion

*Rationale:* The driver is a mismatch between a price fixed at signing and a risk profile that has changed,
and the value has three sources — margin, tenor into the tail, and the covenant package — which is why a
`PLCR` of 1.9431 against an `LLCR` of 1.2743 measures something a margin cut cannot deliver (15.3.1). A is
sometimes true and is not the mechanism: a refinancing can pay in a rising market where the risk retired is
large enough. C explains why the tail exists, not why the facility is mispriced. D invents an obligation —
a refinancing is a new transaction, negotiated.

### Self-check — KA 15.3

1. *Why does a 155 basis point margin saving fail to pay for Kestrel's refinancing?* — The saving
   is a thin flow on a rapidly amortising balance; the breakeven reduction is 177.94 basis points.
2. *What actually makes the transaction work?* — Tenor: three years into the tail, plus the
   interaction of tenor with the lower rate — 586,108.78 and 420,208.04 of the 2,425,030.89.
3. *State the rule for spending an equity cure.* — In proportion to what it buys; a marginal breach
   is waived, because the cure is a capped option with a scarcity value.

---

## Knowledge Area 15.4 — Distress, restructuring, exit and handback

*Topics: 15.4.1 what distress is, financially · 15.4.2 the three restructurings, compared ·
15.4.3 the enforcement floor and the bargaining range · 15.4.4 exit and the arithmetic of
crystallisation · 15.4.5 handback and the residual obligation.*

### 15.4.1 What distress is, financially

**Definition.** A project is in **financial distress** when its forecast cash cannot sustain its
contracted debt service on the agreed profile — which is a different condition from a covenant
breach (a test failure at a date) and from insolvency (an inability to pay). The distinction is
operational: a breach is cured, waived or amended; distress is **restructured**, because no fee or
one-off injection fixes a profile mismatch that persists for the life of the loan.

The diagnostic is simple and worth stating as a rule. Compute the **sustainable debt service** —
revised `CFADS` divided by the target coverage — and compare it with the scheduled service. If
scheduled service exceeds sustainable service for a single period, the project has a liquidity
problem and reserves exist for it. If it exceeds sustainable service for the remaining life, the
project has a **capital structure problem**, and only three levers can close it: **time**
(extend), **principal** (reduce), or **new money** (inject).

### 15.4.2 The three restructurings, compared

**Worked example 15.4.2 — Kestrel on the permanent-deterioration branch.**

1. **Setup.** Suppose the year-three deterioration of KA 15.2 had proved **permanent** rather than
   remediable: revised `CFADS` of **5,202,936.48** flat for years 4 to 25. Senior debt outstanding
   at the end of year three is **34,073,997.28**, with **9 years** remaining at 6.00 % and
   scheduled service of 5,009,635.23 — a `DSCR` of **1.0386**. The lenders require **1.20×**
   restored. Lender recovery is measured as the present value of what they receive, both at the
   contract rate of 6.00 % and at a **7.00 %** risk-adjusted required return; equity value is the
   present value at 8.0 % of distributions after the 600,000 maintenance charge to year twenty and
   the 697,844.05 handback charge from year sixteen.
2. **Formula.** Sustainable service = revised `CFADS` ÷ 1.20. Extension: solve for the tenor `n`
   with `outstanding ÷ AF(0.06, n) ≤` sustainable service. Haircut: supported debt = sustainable
   service × `AF(0.06, 9)`. Injection: prepay the difference and re-amortise over the original
   9 years.
3. **Substitution.** Sustainable service `5,202,936.48 ÷ 1.20 = 4,335,780.40`. Required annuity
   factor `34,073,997.28 ÷ 4,335,780.40 = 7.858792`, against `AF(0.06, 10) = 7.360087` and
   `AF(0.06, 11) = 7.886875` → 11 years. Haircut: `4,335,780.40 × 6.801692`.
4. **Result.**

| | A — extend to 11 years | B — principal haircut | C — sponsor injection |
|---|---|---|---|
| Amount written off / injected | — | **4,583,353.23** (13.4512 %) | **4,583,353.23** |
| New instalment | **4,320,342.23** | **4,335,780.40** | **4,335,780.40** |
| Restored `DSCR` | **1.2043** | **1.2000** | **1.2000** |
| Lender recovery, PV at 6.00 % | **100.0000 %** | **86.5488 %** | **100.0000 %** |
| Lender recovery, PV at 7.00 % | **95.0779 %** | **82.9037 %** | **96.3549 %** |
| Equity value, PV at 8 % | **14,898,548.64** | **18,656,183.22** | **14,072,829.99** |
| Annual equity cash while debt runs | 282,594.25 | 267,156.08 | 267,156.08 |

5. **Interpretation.** The table separates three questions that boards routinely merge. **What
   does each side prefer?** Lenders rank C, then A, then B; equity ranks B (18,656,183.22), then A
   (14,898,548.64), then C (14,072,829.99). The rankings are exactly opposed on the haircut, which
   is why haircuts are demanded and rarely granted. **Why does equity prefer the extension over the
   injection**, when the injection produces a lower debt service? Because the injection costs
   4,583,353.23 today to buy a debt-service saving whose present value at 8 % is less — a gap of
   **825,718.65**, which is the arithmetic reason sponsors resist new money even when they have it,
   and the reason a lender should read a refusal to inject as information about the sponsor's
   discount rate rather than about its confidence. **And what does the extension cost the lenders?**
   At the contract rate, nothing: extending a loan at its own coupon returns exactly par, because
   the present value of a longer annuity at the same rate is the same principal. That is the
   arithmetic identity behind every "amend and extend", and it is why extensions are the default
   restructuring. But at the 7.00 % the lenders now require, the extension recovers **95.0779 %** —
   a real loss of 4.92 points of present value, which the accounting may or may not recognise. The
   professional caution is precisely there: **an extension is described as par and is not**, and a
   credit committee that measures recovery only at the contract rate cannot see what it has given
   away. The sensitivity is worth carrying as a rule of thumb — for Kestrel, every 100 basis points
   of required-return increase costs roughly five points of extension recovery.

### 15.4.3 The enforcement floor and the bargaining range

**Definition.** The **enforcement floor** is the recovery a lender would achieve by enforcing its
security and realising the asset, net of costs and delay. It is the boundary of every
restructuring negotiation, because no lender rationally accepts a proposal recovering less than the
floor, and no sponsor rationally accepts one leaving less than the value of its equity elsewhere.

**Worked example 15.4.3 — computing the floor, and what it excludes.**

1. **Setup.** On enforcement, Kestrel would be sold to a buyer who assumes lower performance and a
   higher cost base than the incumbent — `CFADS` of **4,900,000** over the remaining 22 years — and
   who requires **13.0 %** on a project acquired out of enforcement with a broken operations and
   maintenance arrangement. Enforcement and sale costs are **6 %** of proceeds, and the process
   takes **twelve months**, over which the lenders discount at their 6.00 % contract rate.
2. **Formula.** Floor = [buyer's `CFADS` × `AF`(buyer's rate, remaining life) × (1 − costs)] ÷
   (1 + lender rate).
3. **Substitution.** `4,900,000 × AF(0.13, 22) = 4,900,000 × 7.169513`; `× 0.94`; `÷ 1.06`.
4. **Result.** Gross enterprise value **35,130,615.34**; costs **2,107,836.92**; net
   **33,022,778.42**; discounted twelve months **31,153,564.55** — a recovery of **91.4291 %**.
5. **Interpretation.** The floor decides the negotiation before it starts. Option B's haircut
   recovers **82.9037 %** on the lenders' own required return — **8.5254 points below** what
   enforcement pays — so it is not a hard bargain, it is **outside the feasible set**, and a
   sponsor proposing it has spent credibility on an option that could never be accepted. Working
   the constraint in reverse gives the more useful number: the **maximum haircut a lender can
   accept is 2,920,432.73**, or **8.5709 %** of the outstanding, at which recovery equals the
   floor. But that haircut supports an instalment of **4,580,266.69**, a `DSCR` of only
   **1.1359** — still below the 1.20 required. **The largest haircut the lenders can grant does
   not solve the problem**, which is the decisive finding of the whole analysis: on these numbers
   the restructuring *must* include an extension, and the only remaining questions are how long
   and at what price. Two professional qualifications. The floor is a **model of a distressed
   sale**, and every input is contestable — the buyer's `CFADS`, the required return, the cost
   percentage, the delay — so the discipline is to present it as a range and to state which input
   the conclusion depends on (here, the buyer's required return: at 10.0 % the floor would exceed
   par and no restructuring would be rational at all). And the floor systematically **overstates**
   what most lenders will really do, because enforcement carries consequences no discounted cash
   flow captures: the offtaker's step-in rights (Domain 12's direct agreements), regulatory and
   political exposure, reputational cost, and the simple institutional fact that enforcing leaves
   a bank owning a desalination plant. That is why Domain 10 (KA 10.4.3) observed that lenders
   rarely accelerate a fundamentally sound project — the floor bounds the negotiation, but the
   negotiation is conducted well inside it.

> **Fig 15.4.1 — The restructuring frontier.** Scatter plot for Kestrel at the end of operating
> year three on the permanent-deterioration branch. X-axis: lender recovery as the present value at
> a 7.00 % required return, expressed as a percentage of the 34,073,997.28 outstanding, from 80 %
> to 100 %. Y-axis: equity value as the present value at 8 % in USD millions, from nil to 21.
> Four plotted options: maturity extension to 11 years (95.0779 %, 14.8985m); principal haircut of
> 4,583,353 (82.9037 %, 18.6562m); sponsor injection of 4,583,353 (96.3549 %, 14.0728m);
> enforcement and sale (91.4291 %, nil). A crimson dashed vertical at the 91.4291 % enforcement
> floor with the region to its left tinted and labelled "lenders reject — enforcement pays them
> more", and a solid line at par. Caption note: the haircut is the option equity most prefers and
> the only one outside the feasible set, recovering 8.5254 points less than enforcement.
> Source: PCI original. Alt text: a scatter chart of four restructuring options plotting lender
> recovery against equity value, with a vertical enforcement-floor line that places the principal
> haircut — the option most valuable to equity — inside the rejected region.

### 15.4.4 Exit and the arithmetic of crystallisation

**Definition.** An **exit** is the sale of the sponsor's equity interest, priced as the present
value of the distributions the buyer expects at the buyer's required return. It is the ordinary
end of a sponsor's involvement — development-stage sponsors sell to long-term infrastructure
holders as a matter of business model — and it is where `IRR` is most often used to claim credit
for arithmetic.

**Worked example 15.4.4 — Kestrel sold at the end of year eight.**

1. **Setup.** The base-case profile of 15.2.3. By the end of year eight the sponsors have received
   **6,194,918.16** of distributions on 18,000,000 subscribed. A long-term holder will buy the
   remaining seventeen years of distributions. Price it at required returns of 6.50 %, 7.00 %,
   7.50 % and at the sponsors' own 8.00 %.
2. **Formula.** Price = Σ (distributions in years 9–25) discounted at the buyer's required return
   to the end of year eight. Seller's `IRR` = the rate zeroing the flow (18,000,000) at time zero,
   distributions in years 1–8, and the price at year eight.
3. **Substitution.** Discount 774,364.77 (years 9–12), 5,784,000 (13–15), 5,086,155.95 (16–20) and
   5,686,155.95 (21–25) at each rate.
4. **Result.**

| Buyer's required return | Price at end of year 8 | Seller's `IRR` | Seller's `NPV` at 8 % |
|---|---|---|---|
| 6.50 % | **39,260,418.77** | 13.4196 % | 7,661,177.40 |
| 7.00 % | **37,541,787.43** | 12.8568 % | 6,732,654.36 |
| 7.50 % | **35,919,098.27** | 12.3059 % | 5,855,965.90 |
| 8.00 % | **34,386,097.04** | 11.7666 % | **5,027,733.03** |
| Hold to maturity | — | **9.8591 %** | **5,027,733.03** |

5. **Interpretation.** The bottom two rows contain the lesson, and it is one of the most useful
   invariants in this book. **Selling at the sponsors' own 8.00 % discount rate raises the reported
   `IRR` from 9.8591 % to 11.7666 % and creates exactly nothing** — the `NPV` at 8 % is
   **identical at 5,027,733.03**, to the cent, because a sale at the discount rate is by
   construction a swap of the same value at a different date. The entire `IRR` uplift of 1.9075
   points is the arithmetic of early crystallisation: a positive `NPV` realised sooner produces a
   higher internal rate, which is Domain 4's reinvestment fiction (KA 4.1.2) appearing in a
   transaction rather than a forecast. What *is* real is the difference between the buyer's
   required return and the sponsors': selling at 7.50 % rather than 8.00 % is worth
   **1,533,001.23** at the end of year eight, or **828,232.87** at time zero — which reconciles
   exactly to the `NPV` difference of 828,232.87 between the two rows. That 828,232.87 is **yield
   compression**, and it is a view about the capital market, not an achievement of the project.
   The professional discipline is therefore to report an exit in three separated numbers: the value
   created by operating the asset (which is the base-case `NPV`), the value created by yield
   compression (which is a market call, and should be attributed as one), and the `IRR`, which
   explains neither and should never carry the credit for both. A sponsor whose realised `IRR` of
   12.3059 % is presented as operating performance has misdescribed a 9.8591 % project sold into a
   favourable market — and the same arithmetic will run against them when the market moves the
   other way, at which point the honest baseline they did not establish is what they will need.

### 15.4.5 Handback and the residual obligation

**Definition.** **Handback** is the transfer of the asset to the grantor or offtaker at the end of
the concession, in a condition the agreement specifies. The **residual obligation** is the cost of
achieving that condition, and it is the last item in the waterfall and the first one forgotten,
because it falls due after every person who negotiated it has left.

Kestrel's obligation is an independently estimated **8,000,000** in year-25 money — plant
condition works, a final membrane and pump replacement to a defined residual standard, and
restoration of the intake structure — funded by a reserve earning **3.00 %**.

**Worked example 15.4.5 — when to start funding, and the invariant that decides it.**

1. **Setup.** Two funding profiles: level annual contributions over **years 16 to 25** (ten years),
   or over **years 6 to 25** (twenty years). Equity discounts at 8.0 %.
2. **Formula.** Contribution = obligation ÷ `FVAF`(reserve rate, years). Present cost to equity =
   contribution × `AF(0.08, years)` × `DF(0.08, years before the first contribution)`.
3. **Substitution.** `8,000,000 ÷ FVAF(0.03, 10) = 8,000,000 ÷ 11.463879`;
   `8,000,000 ÷ FVAF(0.03, 20) = 8,000,000 ÷ 26.870374`. Then
   `× AF(0.08, 10) × DF(0.08, 15)` and `× AF(0.08, 20) × DF(0.08, 5)`.
4. **Result.**

| Profile | Annual contribution | Total contributed | Present cost at 8 % |
|---|---|---|---|
| Years 16–25 (10 years) | **697,844.05** | 6,978,440.53 | **1,476,147.78** |
| Years 6–25 (20 years) | **297,725.66** | 5,954,513.22 | **1,989,422.56** |

   Starting ten years earlier more than halves the annual charge and reduces the undiscounted total
   by 1,023,927.31 — and costs equity **513,274.78 more** in present value, **34.7712 %** more.
5. **Interpretation.** The result is counterintuitive until the mechanism is named, and then it is
   obvious and permanently useful. **A pre-funded reserve is equity lending money to itself at the
   reserve's earning rate.** Kestrel's reserve earns 3.00 %; its equity requires 8.00 %; every
   dollar contributed early is a dollar invested at a 500 basis point negative spread. The
   invariant that proves it: **when the reserve rate equals the discount rate, the funding profile
   is irrelevant** — the present cost of any profile that reaches 8,000,000 at year 25 is exactly
   `8,000,000 × DF(0.08, 25) =` **1,168,143.24**, and the indifference rate is therefore precisely
   the discount rate, 8.0000 %. That single fact organises the whole negotiation: below the
   discount rate, sponsors rationally defer and grantors rationally insist; above it, the interests
   align and the argument disappears. It also tells a leader where to spend effort — not on the
   funding profile, which is worth 513,274.78, but on **the credit quality of the reserve's
   investments and the rate they earn**, which is worth more. And it names the reason grantors
   insist anyway, which is not arithmetic but counterparty risk: a reserve funded in the last ten
   years of a concession depends on the concessionaire's solvency in years 16 to 25, and the
   grantor has no claim on a sponsor that has already sold. The resolution used in practice — and
   the recommendation here — is a **reserve plus a handback bond or parent guarantee**, which
   separates the funding question from the credit question instead of using the first as a
   substitute for the second.

Three further mechanics complete the topic. The obligation is fixed by a **condition survey**,
typically two to five years before expiry, and the survey is where the estimate meets reality —
Case study B prices what happens when it does. **Residual obligations survive handback**: a
defects period, a retention against the handback works, and warranties on replaced equipment.
And **the asset's residual value belongs to whoever the concession says it belongs to**, which is
usually the grantor and never automatically the sponsor: Domain 4's appraisal treated the tail as
project cash, and the concession may treat the terminal asset as worth nothing at all to equity.

### AI in this KA

**Where it earns its place.** Restructuring analysis is combinatorial — tenor, haircut, injection,
sweep, covenant reset and fee interact — and enumerating the option space against two objective
functions (lender recovery, equity value) is exactly the work a machine should do. Generating the
**frontier** of Fig 15.4.1, and locating the enforcement floor across a range of buyer
assumptions, converts a negotiation about positions into an inspection of a feasible set. Two
further uses: **early-warning classification** across a portfolio, ranking assets by distance to
the sustainable-service condition of 15.4.1 rather than by current ratio; and **handback obligation
tracking**, holding every asset's survey date, estimate vintage and reserve balance in one place,
because the failure mode in Case study B is an information failure a register would have prevented.

**Where it must not go.** Nothing in the legal characterisation of distress may be delegated:
whether facts constitute an event of default, what remedies follow, what a security package
actually secures, and how an insolvency régime would treat any of it are questions for qualified
counsel in the relevant jurisdiction, and they differ fundamentally between jurisdictions in ways
no general model should be trusted to reflect. Nor should a model be used to select the
restructuring proposal: the choice among feasible options is a judgment about counterparties,
relationships and reputational consequence over decades, and it belongs to named people.

**Verification, concretely.** Re-derive the sustainable-service test by hand; confirm that each
option's restored ratio meets the requirement stated. Check the extension identity — present value
at the contract rate must return exactly par — as the cheapest single audit of any restructuring
model. Test the enforcement floor's sensitivity to the buyer's required return and report the
range, not the point. For handback, verify that the sinking fund reaches the obligation in the
final year and that the indifference-rate invariant holds in the model. Record every assumption
about a distressed buyer as an assumption, with an owner.

### Key terms — KA 15.4

| Term | Meaning |
|---|---|
| **Financial distress** | Forecast cash cannot sustain contracted debt service for the remaining life — a capital structure problem, not a liquidity one. |
| **Sustainable debt service** | Revised `CFADS` ÷ target coverage; the diagnostic that separates distress from a bad period. |
| **Maturity extension** | Lengthening the tenor; returns par at the contract rate and less at any higher required return. |
| **Principal haircut** | Writing down principal; preferred by equity, feasible only above the enforcement floor. |
| **Enforcement floor** | Net recovery from enforcing security and realising the asset; bounds every negotiation. |
| **Yield compression** | Value created by selling at a lower required return than the seller's own — a market view, not performance. |
| **Handback** | Transfer of the asset at concession end in a specified condition; the residual obligation and its reserve. |
| **Condition survey** | The pre-expiry inspection that fixes the handback obligation against the funded estimate. |

### Sample MCQs — KA 15.4

**MCQ 15.4-A `[15.4.2 · Analysis]`** A lender extends a distressed loan at its existing contract
rate so that coverage is restored. Measured at the contract rate, its recovery is:
- A. below par, because payment is deferred
- B. exactly par, because the present value of a longer annuity at the same rate is the same principal ✅
- C. above par, because more interest is received in total
- D. indeterminate without the sponsor's discount rate

*Rationale:* The extension identity of 15.4.2 — which is why "amend and extend" is the default and
why a committee measuring recovery only at the contract rate sees no loss. The real cost appears at
a higher required return: 95.0779 % at 7.00 % for Kestrel. C confuses total nominal interest with
present value; D is irrelevant to the lender's measure.

**MCQ 15.4-B `[15.4.3 · Analysis]`** Kestrel's enforcement floor is a 91.4291 % recovery. A
proposed haircut recovers 82.9037 % on the lenders' 7.00 % required return. The correct
characterisation is:
- A. an aggressive but negotiable proposal
- B. outside the feasible set — enforcement pays the lenders 8.5254 points more ✅
- C. acceptable, because enforcement destroys value for everyone
- D. acceptable, because equity value of 18,656,183.22 is the highest of the options

*Rationale:* No lender accepts less than the floor (15.4.3). A misreads infeasibility as
negotiating distance; C is true of the parties jointly and irrelevant to the lender's individual
choice; D states equity's preference, which is not a constraint on lenders.

**MCQ 15.4-C `[15.4.4 · Analysis]`** A sponsor holding a 9.8591 % `IRR` project sells at the end of
year eight at exactly its own 8.0 % discount rate and reports an `IRR` of 11.7666 %. The value
created is:
- A. the 1.9075 percentage point `IRR` uplift
- B. nil — the `NPV` at 8 % is identical at 5,027,733.03; the uplift is the arithmetic of early crystallisation ✅
- C. USD 828,232.87
- D. USD 34,386,097.04

*Rationale:* A sale at the discount rate is a swap of equal value at a different date (15.4.4).
A mistakes a rate for value; C is the value of yield compression at 7.50 % rather than 8.00 %,
which is a different transaction; D is the price, not the gain.

**MCQ 15.4-D `[15.4.5 · Application]`** An 8,000,000 handback obligation at year 25 can be funded
by a reserve earning 3.00 % over years 16–25 (697,844.05 a year) or years 6–25 (297,725.66 a
year). At an 8 % equity discount rate, the earlier profile:
- A. is cheaper, because the total contributed is 1,023,927.31 lower
- B. costs equity 513,274.78 more in present value, because the reserve earns 500 basis points less than equity requires ✅
- C. costs the same, because both reach 8,000,000 at year 25
- D. is cheaper, because contributions are smaller

*Rationale:* Pre-funding is equity lending to itself at the reserve rate (15.4.5). A and D compare
undiscounted amounts across different decades; C would be true only if the reserve earned the 8 %
discount rate — the indifference-rate invariant, at which every profile costs
`8,000,000 × DF(0.08, 25) = 1,168,143.24`.

**MCQ 15.4-E `[15.4.2 · Evaluation]`** A credit committee paper recommends the maturity extension on
the ground that lender recovery is **100.0000 %** of the 34,073,997.28 outstanding, measured at the
6.00 % contract rate. Is that recommendation supported by the evidence it cites?
- A. yes — the present value of a longer annuity at the same rate returns the same principal, so recovery is par
- B. no — par at the contract rate is an arithmetic identity that holds however far the credit has deteriorated, so it carries no information; at the 7.00 % the lenders now require the extension recovers **95.0779 %**, a real loss of 4.92 points the paper does not disclose ✅
- C. no — the sponsor injection recovers 96.3549 % and should therefore be recommended instead
- D. yes, provided the paper also shows the restored `DSCR` of 1.2043 clearing the 1.20 requirement

*Rationale:* A states a true fact and mistakes it for evidence; because the identity is
insensitive to the very deterioration the committee is being asked to approve, quoting it is what
conceals the concession (15.4.2). C names the option lenders prefer but it is not theirs to grant —
the sponsor rationally refuses, the injection costing 4,583,353.23 today for a saving worth
825,718.65 less. D adds a necessary condition and treats it as a sufficient one: coverage restored
says the loan can be serviced, not what the lenders gave up to get there.

**MCQ 15.4-F `[15.4.4 · Evaluation]`** The sponsors sell at the end of operating year eight to a holder
requiring **7.50 %**, and the board paper reports the realised `IRR` of **12.3059 %** as evidence of
operating performance against a hold-to-maturity **9.8591 %**. The soundest assessment of that paper is:
- A. it is sound — a realised `IRR` is the return the shareholders actually earned, and 12.3059 % is a fact
- B. it misattributes: a sale at the sponsors' own 8.00 % would have lifted the reported `IRR` to
  **11.7666 %** while creating exactly nothing, so the genuine gain from selling at 7.50 % is the
  **828,232.87** of yield compression — a market view — and it belongs in the paper separately from the
  **5,027,733.03** the asset itself created ✅
- C. it understates performance: the present value at 8 % of the 7.50 % sale is **5,855,965.90** against
  5,027,733.03 on a hold, so the paper should claim the larger figure
- D. it is sound provided the buyer's required return is disclosed alongside the `IRR`

*Rationale:* A sale at the seller's own discount rate is a swap of equal value at a different date, so
**1.9075** points of the uplift is early crystallisation and none of it is performance (15.4.4). A states a
true number and lets it carry a claim it cannot support. C makes the mirror error to the paper's: the
828,232.87 is real value and it is a view about the capital market rather than about the asset, so claiming
it as performance is the same misattribution in a larger figure. D is necessary and not sufficient —
disclosing the rate does not separate the three things a board needs kept apart: the value the asset
created, the compression, and the `IRR`, which explains neither.

**MCQ 15.4-G `[15.4.1 · Comprehension]`** A covenant breach, financial distress and insolvency are three
distinct conditions. Which statement distinguishes them correctly?
- A. they are the same condition at three degrees of severity, so the response differs only in urgency
- B. a breach is a test failure at a date, which is cured, waived or amended; insolvency is an inability to
  pay; distress is a forecast that cannot sustain the contracted debt service for the remaining life, which
  is a capital-structure problem and is restructured ✅
- C. distress is a breach that has occurred at two or more consecutive test dates
- D. distress is present once the debt service reserve has been drawn

*Rationale:* The three are separated by what they are conditions *of* — a test, a profile and a payment —
and the diagnostic between the second and third is whether scheduled service exceeds sustainable service
for one period or for the remaining life (15.4.1). C counts breaches, which says nothing about the
forecast. D describes a liquidity event reserves exist to absorb: Kestrel's debt service reserve was never
drawn across six deteriorating years, while the distribution test bound throughout.

### Self-check — KA 15.4

1. *What single test distinguishes distress from a bad period?* — Whether scheduled service exceeds
   sustainable service (`CFADS` ÷ target coverage) for one period or for the remaining life.
2. *Why could Kestrel's restructuring not be solved by a haircut alone?* — The largest haircut above
   the enforcement floor is 2,920,432.73, which supports a `DSCR` of only 1.1359 against the 1.20
   required.
3. *Why does a grantor insist on early handback funding when it costs the sponsor more?* — Not
   arithmetic but counterparty risk: a late-funded reserve depends on the concessionaire's solvency
   in the final decade. The clean answer is a reserve plus a bond or guarantee.

---

## Advanced topics — Domain 15

### 15.A.1 The restatement problem: when a test date moves under you

Operating covenants are tested on reported figures, and reported figures change — through an audit
adjustment, a prior-period error, a reclassification of maintenance cost between capital and
revenue, or a superseded forecast basis. Facilities handle this unevenly, so three questions must
be settled before they matter: **is a certified test re-opened if the underlying figures are
restated**, and within what period; **does a later-discovered breach rank from the original test
date or from discovery**, which determines whether the cure and waiver windows have already
expired; and **whose basis governs a forward test that has been superseded** — the forecast as
made, or as it should have been made. The protection is procedural rather than contractual: certify
only on figures that have passed the same controls as the statutory accounts, keep every certified
ratio's computation and inputs frozen and retrievable, and disclose a material re-forecast when it
is made rather than when it is tested. Domain 10 (KA 10.4.4) established that early disclosure is
the asset; this is where it earns its return, because a lender told in advance treats a restated
breach as administration and a lender told afterwards treats it as a discovery.

### 15.A.2 Portfolio operations and the shared-covenant problem

The disciplines of this domain scale badly when each asset is run in isolation, and three portfolio
effects deserve naming. **Test-date clustering**: facilities testing on the same quarter-end turn
one adverse market event into simultaneous breaches and simultaneous negotiations, exactly when
negotiating capacity is scarcest — staggering test dates at the point of documentation costs
nothing. **Cross-default and cross-acceleration**: a breach in one ring-fenced project reaches
other assets through guarantees, holding-company facilities or change-of-control provisions, so the
true exposure of a project-level covenant is a portfolio question no project-level model can see.
**Cure-capacity concentration**: a sponsor with four assets each holding two cures does not have
eight cures, it has as many as its liquidity supports, which in a correlated downturn is fewer. The
response is one consolidated covenant register across the portfolio — every test, date, threshold,
cash trigger, cure remaining and cross-default link — owned by one person with authority over all
of it. Its absence is the commonest reason a group of individually sound projects experiences a
group-level liquidity event.

### 15.A.3 The reviewer's operating eye

Invariants to test on any operating, waterfall or restructuring model — each one a defect indicator
if it fails:

- The operating bridge ties: revenue less cash opex equals `EBITDA`; `EBITDA` less cash tax equals
  `CFADS` before working capital; the working-capital movement equals the change in the balance
  sheet position, not a plug.
- The same `CFADS` line feeds the covenant, the lock-up, every distribution test and the reported
  ratio (Domain 10, KA 10.A.3), and the certified figure is the defined one.
- Every rolling test window contains exactly the debt service the documents say it does; for a
  level annuity paid annually, exactly one instalment.
- Forward-looking tests are computed on a stated, dated forecast with a named owner, and the
  outturn is reconciled to it afterwards.
- The waterfall closes in every period: opening balances plus receipts less applications equals
  closing balances, account by account, with no residual.
- Reserve top-ups rank above distributions, and a period whose residual cannot fund a top-up draws
  from the block account rather than reducing the top-up.
- Distributions are nil in any period where any condition fails, including the forward leg and the
  qualitative conditions — a model that distributes on the backward test alone is wrong.
- `LLCR` equals `DSCR` wherever cash is level and service is an annuity at the loan rate (Domain
  10, KA 10.2.2) — still true after a refinancing, at the new rate.
- Refinancing arithmetic is complete: old service ceasing, new service arising, break cost on the
  actual notional profile, every fee, and the sweep.
- A maturity extension at the contract rate returns exactly par in present value; if the model says
  otherwise, the model is wrong.
- Restructuring options are tested against the enforcement floor before they are presented, and
  the floor's sensitivity to the distressed buyer's required return is shown as a range.
- The handback sinking fund reaches the obligation in its final year; the funding profile is
  irrelevant when the reserve rate equals the discount rate.
- Minimum, not average, coverage over the remaining life is reported, and the distribution profile
  is reported after tests rather than before.

---

## Industry variations — Domain 15

- **Contracted water and availability-based PPPs.** Deductions are formulaic and performance-linked,
  so the operating bridge is unusually tight and the covenant is predictable from operating data —
  Kestrel's case. The distinctive operating risk is an **unhedged input**: a pass-through that was
  assumed and is not contractual, which is how a 5.9 % power-price rise removes an entire dividend.
- **Contracted power and renewables.** Resource variability makes a single year's coverage
  uninformative, so structures use rolling multi-year tests and larger reserves; the characteristic
  operating-phase event is a **curtailment or grid-constraint régime change** that no contract
  allocated, and the characteristic financing event is a refinancing into a tighter market once
  operating history exists.
- **Transport concessions.** Patronage risk plus high operating leverage means `CFADS` falls faster
  than revenue, and ramp-up years dominate the covenant profile. Handback obligations are large,
  physical and surveyed — pavement, structures, tolling systems — which makes KA 15.4.5 the sector's
  defining operating-phase discipline rather than a footnote (Case study B).
- **Regulated utilities.** Regulatory reset cycles produce step-changes in allowed revenue, so
  covenant testing and reserve sizing must straddle resets; the operating-phase negotiation is
  frequently a **covenant reset timed to the determination**, and a rolling test that spans a reset
  measures two different businesses.
- **Digital infrastructure.** Short asset lives compress the tail, so refinancing capacity is
  limited and the tenor lever of KA 15.3.2 is weaker; contract renewal rather than performance is
  the covenant risk, and technology obsolescence makes the handback and residual-value questions
  genuinely open rather than formulaic.
- **Mining and resources.** Price exposure dominates and distress is cyclical rather than
  idiosyncratic, so restructurings are negotiated against a commodity view; the enforcement floor is
  unusually volatile, which widens the bargaining range and makes the sensitivity discipline of
  15.4.3 essential rather than good practice.

---

## Case study — Domain 15: the dividend the forward test cancelled (water / desalination)

**Situation.** Kestrel Water reached the end of its first operating year having done everything
right. Availability had held at the 95.0 % guarantee, 10,000,000 m³ had been delivered, `CFADS` of
**6,384,000** matched the base case to the dollar, and the backward `DSCR` of **1.2743** cleared
both the 1.20× covenant and the 1.25× distribution condition. After debt service of 5,009,635.23
and the 600,000 maintenance-reserve charge, **774,364.77** sat in the proceeds account, and the
board's papers showed it as the first dividend. Between the year-end and the test date, the
operator revised its power-cost forecast: a supply contract had rolled onto merchant pricing, and
the delivered cost of energy would rise from 0.26 to roughly 0.40 per cubic metre. Management
re-forecast year two at **5,500,000** of `CFADS`.

**What happened.** The distribution condition required 1.25× on a forward-looking as well as a
backward-looking basis. The re-forecast gave a forward `DSCR` of **1.0979**, and the dividend was
not payable. The 774,364.77 went into the distribution-block account. The board's first reaction
was that the test had been misapplied — the project had met every covenant — and the second was
that the re-forecast should be revisited. Both reactions were understandable and the second was
dangerous: the finance director's position, that a forecast prepared to stop a dividend and a
forecast prepared to support one cannot be different documents, held, and the re-forecast stood.
Year two delivered `CFADS` of **5,963,894.11** — **463,894.11 above** the forecast that cancelled
the dividend, and still a `DSCR` of **1.1905**, a covenant breach. The sponsors cured it for
**47,668.16**, the cheapest available response, consuming one of two lifetime cures. Year three was
worse: fouling took availability to 88.0 %, `CFADS` fell to **5,202,936.48**, `DSCR` to **1.0386**,
and the year's residual after debt service was **193,301.25** against a 600,000 reserve charge —
so **406,698.75** of the maintenance top-up was drawn from the block account.

**How it resolved.** With one cure left and a breach requiring **808,625.80** to cure, the sponsors
negotiated rather than paid: a covenant reset to 1.00× for two test dates stepping to 1.10× and
1.20×, a **40 basis point** margin uplift with a present value of **651,258.24**, an amendment fee
of **92,178.38**, monthly reporting, and a committed **1,500,000** of remediation equity. The
membrane replacement was brought forward from year five and cost 3,000,000 against a reserve of
1,800,000 — the **1,200,000** gap and **300,000** of reserve restoration being exactly what the
injection funded. Power was hedged at 0.32 for year four and 0.30 thereafter. Year four recovered
to `CFADS` **6,094,641.91** (`DSCR` 1.2166 — still below the 1.25 distribution test), and year five
cleared both legs at **1.2936** backward and **1.2966** forward, releasing the whole
**1,206,931.59** block-account balance alongside the year's **870,992.76** as a
**2,077,924.35** distribution. Over six years equity lost **3,182,311.16** of cash —
**1,682,311.16** of trading shortfall and **1,500,000** of capital call — and **2,709,838.79** of
present value at 8 %.

**What the domain teaches here.** Three things, in order of how often they are got wrong. The
project was never in danger: the debt service reserve of 2,504,817.62 was never touched, because
even the worst year's cash exceeded debt service by 193,301.25. What bound was the distribution
test, at 1.9103 % of `CFADS` against the covenant's 5.8339 % — and the sponsors had modelled the
covenant. The block account was not a penalty but the pre-funding that paid for year three's
reserve shortfall out of year one's cancelled dividend, which is the strongest available argument
against negotiating a weaker distribution test. And the 47,668.16 cure was the expensive decision
of the whole sequence: paying **7,638.87** more for a waiver would have preserved an option worth
808,625.80 nine months later, and the amendment that replaced it cost **743,436.62**.

## Case study B — Domain 15: the handback nobody had surveyed (transport concession)

**Situation.** A thirty-year urban toll-road concession approached expiry. The concession agreement
required the concessionaire to hand the asset back in a defined condition and to fund a handback
reserve over the **final five years**, against an independent estimate to be prepared in **year
25**. That estimate came in at **42,000,000**, producing a contribution of **7,910,892.00** a year
into a reserve earning 3.00 % — comfortably affordable against distributable cash of
**21,000,000**, leaving **13,089,108.00** a year for shareholders. The estimate was prepared from
design records and a visual inspection. No intrusive condition survey was commissioned, because the
agreement did not require one until year 28.

**What happened.** The year-28 survey found the obligation was **58,000,000**, not 42,000,000 — an
understatement of **16,000,000**, or **38.0952 %**. Two causes, both foreseeable: pavement
rehabilitation was required over a substantially longer length than the design records implied,
because a mid-life resurfacing had been thinner than specified; and the tolling system needed
wholesale replacement, which the original estimate had treated as the incoming operator's cost on a
reading of the agreement that the grantor did not share. By the end of year 28 the reserve held
**24,451,776.08**, which would grow to **25,940,889.24** by expiry — leaving a gap of
**32,059,110.76** to be funded from two remaining years.

**How it resolved.** The required additional contribution was **15,792,665.40** a year, taking the
total reserve charge to **23,703,557.40** — **112.8741 %** of distributable cash. The final two
years therefore produced no distributions at all and a shortfall of **2,703,557.40** a year, met by
a sponsor injection of **5,407,114.79** in aggregate — the additional contribution of
**31,585,330.80** over two years consuming the **26,178,216.00** of dividends those years would
have paid and 5,407,114.79 more besides. The grantor additionally held a retention of
**2,900,000** — 5 % of the works —
for twelve months against defects. The concession completed and handed back on time, and the
sponsor's realised return on a thirty-year asset was set, in the end, by an estimate prepared from
records in year 25.

**What the domain teaches here.** The obligation was affordable and the timing was not. Had the
58,000,000 been known at year 20 and funded over ten years, the charge would have been
**5,059,369.38** a year — **24.0922 %** of distributable cash, absorbed without comment. The
**26,178,216.00** of forgone distributions and **5,407,114.79** of injected equity was therefore the
price of an **information** failure, not a cash failure: a survey commissioned eight years earlier
than the
agreement required would have cost a fraction of one year's contribution. Two further lessons.
An estimate prepared from design records is an estimate of the asset as designed, and a thirty-year
asset is not the asset as designed. And a reserve is not a substitute for a credit instrument: the
grantor's protection came from the retention and the sponsor's willingness to inject, not from a
reserve that was funded on a schedule the agreement itself had made too late.

---

## Executive perspective — Domain 15

What a project finance director cannot delegate in this domain:

- **The distribution conditions, in full and in cash.** Not the covenant — the test that stops the
  dividend, including its forward leg, its qualitative limbs and the reserve charges that rank
  above it. For Kestrel that is 6,262,044.04 of `CFADS`, 1.9103 % below base case, and it is the
  number that belongs at the top of the operating dashboard.
- **The re-forecast basis, and its owner.** A forward-looking test makes the company's own forecast
  the instrument that stops its own dividend. The basis, the timing, the reviewer and the reason a
  forecast prepared to stop a payment is the same document as one prepared to support it are the
  director's to establish before the first test date, not during it.
- **The reserve sizing assumption.** Kestrel's 1,500,000 capital call — 47.1 % of the six-year cost
  to equity — was caused by a maintenance reserve levelised on the assumption that a five-year
  replacement cycle runs to five years. That is a financing judgment wearing engineering clothes,
  and it is the director's.
- **The cure inventory.** How many cures remain, what each is worth, and the standing rule that a
  marginal breach is waived rather than cured. Kestrel's 47,668.16 decision cost an option worth
  808,625.80, and it was taken by someone optimising a single test date.
- **The whole term sheet on any refinancing or amendment.** Margin, tenor, fees, break cost and
  sweep priced together as one net present value, with the breakeven computed before the first
  meeting. A 155 basis point saving that destroys 206,835.69 of value, and a consent whose sweep
  costs more than the gain it consents to, are both signed by directors who priced one line.
- **The handback obligation, from the first operating year.** Its estimate vintage, the date of the
  next intrusive survey, the reserve's earning rate, and the credit instrument standing behind it.
  It falls due after everyone who negotiated it has gone, which is precisely why it needs an owner
  now.

## Calculation exercises — Domain 15

**Exercise 15.1** Annual debt service is 7,200,000 and `CFADS` is 9,100,000. The facility has a
1.20× covenant, a 1.10× lock-up and a 1.30× distribution condition. The cash tax rate is 25 %, and
the capacity payment abates by 120,000 per percentage point of availability. State each trigger in
cash, the headroom, and the availability movement that reaches the covenant and the distribution
test.

*Solution.* `DSCR = 9,100,000/7,200,000 =` **1.2639**. Covenant `7,200,000 × 1.20 =`
**8,640,000**, headroom **460,000** (**5.0549 %** of `CFADS`); lock-up `× 1.10 =` **7,920,000**,
headroom **1,180,000** (**12.9670 %**); distribution `× 1.30 =` **9,360,000**, a **shortfall of
260,000** (**−2.8571 %**) — the project already fails its distribution test while comfortably
inside its covenant. Cash-to-revenue gearing is `1 − 0.25 = 0.75`, so revenue headroom to the
covenant is `460,000/0.75 =` **613,333.33**, which is `÷ 120,000 =` **5.1111** availability points;
the distribution test is already breached by `346,666.67` of revenue, i.e. **2.8889 points** of
availability must be *recovered* to restore it. *Common error:* computing availability movement
from the `CFADS` headroom directly (460,000 ÷ 120,000 = 3.83 points), which omits the tax shield
and understates the tolerance by a quarter.

**Exercise 15.2** In a period, `CFADS` is 8,450,000, debt service 7,200,000 and the required
maintenance-reserve top-up 450,000. The distribution condition is 1.30× backward and forward; the
forward twelve-month forecast is 7,650,000. State the distributable amount and what happens to it.

*Solution.* After debt service **1,250,000**; after the reserve top-up **800,000**. Backward `DSCR`
`8,450,000/7,200,000 =` **1.1736**; forward `7,650,000/7,200,000 =` **1.0625**. The test requires
`7,200,000 × 1.30 =` **9,360,000** on both legs, so **both fail** and the **800,000 is trapped in
the distribution-block account**, available for a future reserve top-up or release when the tests
are next satisfied. *Common error:* reporting 1,250,000 as the distributable amount by omitting the
reserve top-up that ranks above equity — the single most common error in operating distribution
forecasts (15.2.2).

**Exercise 15.3** A facility has 31,500,000 outstanding with 8 years remaining at an all-in
5.75 %. The market offers 4.55 % over the same 8 years. Costs: arrangement fee 1.00 %, prepayment
fee 0.25 %, advisers 260,000, swap break 640,000. Discount incremental cash at 8 %. Compute the net
present value and the breakeven all-in rate.

*Solution.* `AF(0.0575, 8) = 6.271705`, so the existing instalment is
`31,500,000/6.271705 =` **5,022,557.82**. `AF(0.0455, 8) = 6.582433`, so the new instalment is
**4,785,464.53** and the annual saving **237,093.28**. `AF(0.08, 8) = 5.746639`, giving a present
value of **1,362,489.49**. Costs `315,000 + 78,750 + 260,000 + 640,000 =` **1,293,750**. **Net
present value +68,739.49.** The breakeven all-in rate is **4.6112 %** — the margin must fall by
**113.88 basis points** against the **120** on offer, leaving **6.12 basis points** of cushion.
*Common error:* discounting the saving over the loan's full original tenor rather than its remaining
8 years, which inflates the present value and can turn a marginal transaction into an apparently
comfortable one.

**Exercise 15.4** A distressed project has 24,000,000 outstanding with 7 years remaining at 5.5 %,
and revised `CFADS` of 3,600,000 flat. Lenders require 1.25× restored and will not accept a
recovery below an enforcement floor of 90.0 %. Determine the extension tenor required, the haircut
the coverage requirement implies, and whether a haircut is feasible.

*Solution.* Sustainable service `3,600,000/1.25 =` **2,880,000** against the current instalment of
`24,000,000/AF(0.055, 7) = 24,000,000/5.682967 =` **4,223,146.03** (`DSCR` **0.8524** — deeply
distressed). Required annuity factor `24,000,000/2,880,000 =` **8.333333**; `AF(0.055, 11) =
8.092536` (instalment 2,965,695.68, `DSCR` **1.2139** — insufficient) and `AF(0.055, 12) =
8.618518` (instalment **2,784,701.55**, `DSCR` **1.2928**), so the tenor must extend to **12
years**, five years beyond the current maturity. A haircut retaining the 7-year tenor supports only
`2,880,000 × 5.682967 =` **16,366,945.30**, a haircut of **7,633,054.70** (**31.8044 %**) and a
recovery of **68.1956 %** — far below the 90.0 % floor, so **infeasible**. The largest feasible
haircut is `24,000,000 × 10.0 % =` **2,400,000**, which supports an instalment of **3,800,831.42**
and a `DSCR` of only **0.9472**: as with Kestrel, **the haircut cannot solve the problem and an
extension is unavoidable**. *Common error:* computing the required annuity factor and rounding the
tenor down (11 years), which leaves coverage at 1.2139 and the covenant still breached — restructuring
tenors round up.

**Exercise 15.5** A 12,000,000 handback obligation falls due at the end of year 25. A reserve earns
2.5 %; equity discounts at 8 %. Compare funding over years 16–25, years 11–25 and years 6–25 on
present cost, and state the rate at which the choice ceases to matter.

*Solution.* `FVAF(0.025, 10) = 11.203382` → **1,071,105.16** a year, present cost
`1,071,105.16 × AF(0.08, 10) × DF(0.08, 15) =` **2,265,706.06**. `FVAF(0.025, 15) = 17.931927` →
**669,197.47** a year, present cost **2,653,163.73**. `FVAF(0.025, 20) = 25.544658` →
**469,765.54** a year, present cost **3,139,004.45**. Latest funding is cheapest; funding twenty
years ahead costs **873,298.39 more** than funding ten. The choice ceases to matter when the reserve
rate equals the discount rate, at which point every profile costs
`12,000,000 × DF(0.08, 25) =` **1,752,214.86**. *Common error:* ranking the profiles by total
contributions (**9,395,310.90** over twenty years against **10,711,051.58** over ten) and concluding
that early funding is cheaper — a comparison of undiscounted sums across different decades, which
reverses the correct answer.

## Practitioner's toolkit — Domain 15

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable — and
set a retention period against each. These registers are the evidence that a decision was taken
properly, so each is retained at least as long as the obligation it supports, in a form that opens
without the tool that created it, with a named custodian who holds it once the engagement ends.
The applicable minimum periods are set by the organisation's own policy and by jurisdiction-specific
statutory, tax and limitation requirements, which this book does not state. Where a register holds
information about identified individuals, the retention period and any minimisation or deletion
obligation that cuts across it are settled with the organisation's data-protection adviser before
the register is adopted.*

### Toolkit 15.T.1 — The operating bridge (one per reporting period)

A single sheet running from physical performance to certified `CFADS`, in fixed rows: availability
and its abatement effect · delivered volume and its unit contribution · each unit cost with its
exposure quantity · revenue · cash operating cost · `EBITDA` · depreciation · `EBIT` · interest ·
profit before tax · cash tax · `CFADS` before working capital · working-capital movement,
decomposed into receivables, payables and inventory · **`CFADS` as defined**, with the clause
reference. Two columns beside each row: budget and prior period. Two rules: the working-capital
movement is a derived figure from balance-sheet positions and never a plug; and the sheet carries
the cash-to-revenue gearing at the foot, so every variance can be read straight into covenant
headroom.

### Toolkit 15.T.2 — Operating covenant and distribution register (one per facility, per test date)

Per test: name and clause · test date and frequency · backward, rolling or forward-looking · window
length · threshold · computed value · **the `CFADS` level at which it triggers** · headroom in
currency and as a percentage of base case · **the same headroom in each driver's own units** ·
reserve balances against required levels · block-account balance and what it is earmarked for ·
cures used and remaining, with the value of the largest breach each remaining cure would cover ·
information covenants due and delivered · cross-default links to other facilities (15.A.2). Front
line, in this order: **which test binds first, by how much cash, in which driver, and how many
cures remain.**

### Toolkit 15.T.3 — Transaction decision sheet (refinancing, amendment or restructuring)

- [ ] Incremental cash-flow set complete: existing service ceasing, new service arising, every fee,
      break costs on the actual notional profile, and the sweep.
- [ ] Discount rate stated, owned and sensitivity-tested; the sign of the answer confirmed at both
      the cost of debt and the cost of equity.
- [ ] **Breakeven computed before the first meeting** — margin, tenor, fee or sweep share, whichever
      is being negotiated.
- [ ] Value decomposed: how much is margin, how much tenor, how much interaction, how much covenant
      reset.
- [ ] For an amendment: sized against the stressed case, not the current one; the reset tested
      against the downside forecast at every date it must survive.
- [ ] For a restructuring: sustainable service computed; each option's restored coverage verified;
      the extension identity checked (par at the contract rate); recovery reported at the contract
      rate **and** at a risk-adjusted rate.
- [ ] Enforcement floor computed, with its sensitivity to the distressed buyer's required return
      shown as a range; any option below the floor removed before presentation.
- [ ] Equity value and lender recovery reported for every option, on one page (Fig 15.4.1).
- [ ] Cure inventory stated; the option value of each remaining cure quantified.
- [ ] AI-produced analysis: the breakeven re-derived independently, one period recomputed by hand,
      every clause reading checked against the document, verifier named.

## Exam preparation — Domain 15

**What is assessed.** Whether a candidate can operate a financing rather than arrange one: build
and reconcile an operating bridge; convert every threshold into cash and into driver units;
distinguish backward, rolling and forward tests and reason about the lag; run a waterfall period by
period including reserve top-ups and a block account; separate the cash cost of a drought from its
timing cost; price a refinancing, a waiver, a cure, an amendment and a sweep against one another;
compare restructuring options on both sides of the table and locate the enforcement floor; size a
handback sinking fund; and value an exit without confusing crystallisation with performance.

**The calculations to do under time pressure.** Trigger in cash (debt service × threshold) and
headroom, then headroom ÷ cash-to-revenue gearing to reach the driver. Distributable = `CFADS` −
debt service − reserve top-ups. Rolling four-quarter sums from a quarterly series. Refinancing net
present value = saving × `AF`(discount rate, remaining years) − total costs, and the breakeven rate
by inspection of `AF`. Sustainable service = revised `CFADS` ÷ target coverage, then the required
annuity factor and the tenor from the factor table. Haircut = outstanding − sustainable service ×
`AF`. Sinking-fund contribution = obligation ÷ `FVAF`.

**The traps, each cross-referenced.** Omitting the reserve top-up from a distribution forecast
(15.2.2, Exercise 15.2) · quoting covenant headroom when the distribution test binds (15.1.3,
MCQ 15.1-A) · converting cash headroom into driver units without the tax gearing (15.1.2, Exercise
15.1) · expecting a rolling backward test to register a current deterioration (15.1.4, MCQ 15.1-C) ·
reading a working-capital release as improved trading (15.1.2) · treating a lock-up as a write-off,
or as a matter of no consequence (15.2.4) · pricing a refinancing on the margin alone (15.3.2,
MCQ 15.3-A) · discounting a refinancing saving over the original rather than the remaining tenor
(Exercise 15.3) · negotiating a sweep separately from the transaction it pays for (15.3.4) ·
spending a scarce cure on a marginal breach (15.3.3, MCQ 15.3-C) · sizing an amendment against the
current case rather than the stress (15.3.3) · reporting an extension as par without a
risk-adjusted recovery (15.4.2, MCQ 15.4-A) · proposing a haircut below the enforcement floor
(15.4.3, MCQ 15.4-B) · rounding a restructuring tenor down (Exercise 15.4) · claiming an `IRR`
uplift from early crystallisation as value created (15.4.4, MCQ 15.4-C) · ranking handback funding
profiles on undiscounted totals (15.4.5, Exercise 15.5).

**How the domain connects.** Backwards: Domain 2 supplies the accrual-to-cash bridge and the
`CFADS` definition this domain reports on; Domain 3 the amortisation schedule whose opening
balances drive every break-cost and margin-uplift calculation; Domain 4 the appraisal outturn is
measured against, and the `IRR` pathologies that reappear in the exit arithmetic of 15.4.4;
Domain 6 the model that is now a contractual deliverable; Domain 7 the revenue mechanics behind the
operating bridge; Domain 10 every ratio, reserve and covenant this domain operates; Domain 12 the
security package that makes the enforcement floor real; Domain 13 the model audit certification
depends on; Domain 14 the completion tests that start the operating clock. Forwards: Domain 16
governs the automation of KA 15.1 and 15.2 — where most organisations will actually put AI to work
in finance.

## Domain 15 summary
The operating phase is governed by the distribution test, not the covenant. Kestrel Water's
covenant binds at `CFADS` of **6,011,562.28** — **372,437.72**, or 5.8339 %, below base case — but
its distribution condition binds at **6,262,044.04**, only **121,955.96** or **1.9103 %** below,
and that is the number that decides whether shareholders are paid. The operating bridge from
availability and volume through `EBITDA` of 7,500,000 and cash tax of 516,000 to the defined
`CFADS` of **6,384,000** makes the translation possible in both directions: a **0.80**
cash-to-revenue gearing means every headroom figure understates the revenue tolerance by a quarter,
so the covenant is reached by **6.0585** points of availability and the distribution test by
**1.9839**. Rolling backward tests lag by their own window — Kestrel's covenant does not fail until
1.1905 four quarters after the decline begins, while the forward test fails at **1.0979**
immediately — which is why the forward leg exists and why the re-forecast basis is a governance
matter. Running the waterfall for six real years produces the domain's central demonstration: the
**2,504,817.62** debt service reserve was never drawn, because even the worst year's
**5,202,936.48** of `CFADS` exceeded debt service by 193,301.25, while the distribution test
withheld four consecutive dividends and the block account rose to **1,206,931.59** before releasing
into a **2,077,924.35** payment in year five. That trapped cash was not lost but deployed — the
**406,698.75** maintenance-reserve shortfall of year three was funded from the dividend cancelled
in year one — and the six-year cost to equity of **3,182,311.16** in cash and **2,709,838.79** in
present value decomposes cleanly into **1,682,311.16** of trading shortfall and **1,500,000** of
capital call, which are two different people's accountabilities. Base-case equity earns
**774,364.77** a year, a **4.3020 %** cash yield rising to 32.13 % once the loan retires, for a
25-year `IRR` of **9.8591 %** and an `NPV` at 8 % of **5,027,733.03**. Mid-life, arithmetic
discipline replaces enthusiasm: a **155 basis point** margin reduction destroys **206,835.69**
because the breakeven reduction is **177.94** basis points, and only the tenor extension —
1,418,714.07 of margin, 586,108.78 of tenor and 420,208.04 of interaction — makes the transaction
worth **+799,481.12**, which a 50 % cash sweep costing **646,327.86** then more than takes back
(breakeven sweep share **40.3334 %**). A waiver at **55,307.03** and a cure at **47,668.16** differ
by 7,638.87 and by an option worth **808,625.80**; an amendment at **743,436.62** and that second
cure differ by **5,290.97**, which means the decision was never about price. In distress the three
levers are time, principal and new money, and the enforcement floor of **91.4291 %** decides among
them: the extension recovers **95.0779 %** at a risk-adjusted 7 % and leaves equity
**14,898,548.64**; the injection recovers **96.3549 %** and leaves **14,072,829.99**; the haircut
equity most wants recovers **82.9037 %** and is infeasible, and even the largest feasible haircut
of **2,920,432.73** restores only a **1.1359** `DSCR` — so an extension is unavoidable. An exit
at the sponsor's own discount rate raises the `IRR` from 9.8591 % to **11.7666 %** while the `NPV`
stays identical at 5,027,733.03, and only the **828,232.87** of yield compression is real. And the
last obligation is the first forgotten: an **8,000,000** handback funded over the final decade
costs **1,476,147.78** in present value against **1,989,422.56** funded over two, because a reserve
earning 3 % is equity lending to itself at a 500 basis point negative spread — an argument that
disappears entirely when the reserve rate reaches the discount rate, at which every profile costs
**1,168,143.24**. Domain 16 governs the automation of all of it.
