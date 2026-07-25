# Domain 10 — Debt Sizing, Covenants and Credit Metrics *(quantitative flagship)*

> **Group:** Executing the transaction (Part Three). **Target:** ~80 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain is the home of `CFADS`, `DSCR`, `LLCR`, `PLCR` and `ICR`,
> and it is where Domains 2, 3 and 4 converge: Domain 2 defined `CFADS` and warned that it is a
> *defined term*, Domain 3 built the discounting and the amortisation schedule, Domain 4 built the
> appraisal. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Everything before this point valued a project. This domain decides **how much debt it can carry,
and on what conditions** — which is the question that determines whether the financing exists at
all. It is the lender's side of the table, and a project finance leader who cannot compute it is
negotiating blind against people who can.

The logic is inverted from corporate finance and that inversion is the domain's central idea. A
company borrows against its balance sheet; a project's debt is sized **from its cash flow
outwards**: forecast `CFADS`, divide by the coverage the lender requires, and the result is the debt
service the project can afford — from which the debt amount follows. Debt capacity is therefore not
a negotiation about appetite but an arithmetic consequence of cash flow and required coverage
(KA 10.1). The coverage ratios themselves — `DSCR`, `LLCR`, `PLCR`, `ICR` — measure different things
and are routinely confused (KA 10.2). Reserve accounts and the debt-service schedule convert the
arithmetic into liquidity that survives a bad year (KA 10.3). And covenants, default, cure rights and
distribution lock-up are the machinery that gives lenders control before, not after, a project
fails (KA 10.4).

**Learning objectives.** After this domain a candidate can: define `CFADS` and explain why its
documented definition governs every ratio built on it; size debt from a `CFADS` forecast and a
target `DSCR`; compute and interpret `DSCR`, `LLCR`, `PLCR` and `ICR`, and state what each sees that
the others do not; explain and apply debt sculpting; stress a coverage position and identify the
covenant that binds first; explain reserve accounts and size a debt-service reserve; describe
covenant types, the lock-up mechanism, events of default and cure rights; explain why lenders prefer
cash-based to accounting-based covenants; and govern AI-assisted covenant and model analysis.

**The master financing.** Kestrel Water SPC continues from Domains 1–4. Its senior loan is
**USD 42,000,000 at 6.0 % over 12 years** (Domain 3's schedule: annual instalment
**USD 5,009,635**, year-one interest **USD 2,520,000**, year-one principal **USD 2,489,635**).
Domain 2 derived its first-year **`CFADS` of USD 6,384,000** on the facility's documented definition
— after working-capital movements. Equity at close was **USD 18,000,000**, giving 70/30 gearing. The
project's operating life is **25 years**; the loan's is 12.

---

## Knowledge Area 10.1 — Debt capacity and sizing

*Topics: 10.1.1 `CFADS` — the term everything rests on · 10.1.2 sizing from coverage ·
10.1.3 sculpting and the shape of debt service.*

### 10.1.1 `CFADS` — the term everything rests on

**Definition.** `CFADS` is the cash a project generates that is available to service debt, in a
period, **before** debt service and after everything that must be paid first. Its construction runs:

```
Revenue collected
  − cash operating costs
  − cash taxes
  − maintenance capex (usually; sometimes below debt service)
  ± movements in working capital
  ± movements in reserve accounts (as defined)
  = CFADS
```

**Every one of those lines is negotiated.** Domain 2 (KA 2.3.1) demonstrated the consequence with
Kestrel: `CFADS` of **6,984,000** before working-capital movements and **6,384,000** after, on the
same year's trading. That is a `DSCR` of 1.39 versus 1.27 — the difference between comfort and near
breach — from a definitional choice.

The professional discipline follows directly. **`CFADS` is whatever the finance documents say it
is**, and the ratio a lender enforces is computed on *their* definition, not on textbook practice.
The leader's obligations are to know the definition, to ensure the model implements it (Domain 13's
model audit checks precisely this), and never to quote a coverage ratio without being able to state
the `CFADS` definition underneath. The recurring negotiation points worth naming: whether
maintenance capex sits above or below `CFADS`; whether cash taxes or accounting tax are used
(Domain 2, KA 2.A.1 — cash, always, and the model must implement it); whether working capital is
included; and how reserve-account movements are treated.

### 10.1.2 Sizing from coverage

**The principle.** Debt is sized so that forecast cash covers debt service by the required margin:

```
Maximum debt service per period = CFADS ÷ target DSCR
Maximum debt                     = maximum debt service × AF(r, n)
```

The second line is Domain 3's annuity factor doing the work: level debt service discounted at the
loan rate over the loan tenor gives the principal it can support.

**Worked example 10.1.2 — how much can Kestrel actually borrow?**

1. **Setup.** `CFADS` **USD 6,384,000** per year (level, for this illustration), loan rate **6.0 %**,
   tenor **12 years**, lender's target **`DSCR` 1.30×**. The sponsors have asked for
   USD 42,000,000.
2. **Formula.** Max debt service = `CFADS`/target `DSCR`; max debt = max debt service ×
   `AF(0.06, 12)`.
3. **Substitution.** `6,384,000 / 1.30 = 4,910,769`; `AF(0.06, 12) = 8.383844`;
   `4,910,769 × 8.383844`.
4. **Result.** Maximum debt service **USD 4,910,769**; maximum debt **USD 41,171,123** — so the
   requested 42,000,000 is **USD 828,877 too much** at a 1.30× target. At 42,000,000 the actual
   `DSCR` is **1.2743**, below the target.
5. **Interpretation.** This single calculation is the centre of most financing negotiations, and
   notice what it does *not* depend on: the sponsors' preference, the project's NPV (Domain 4's
   +16.2m), or the asset's cost. Debt capacity is a function of **cash flow, required coverage,
   rate and tenor** — nothing else. That gives the leader four levers and no others: raise `CFADS`
   (revenue or cost), argue the coverage requirement down (usually by reducing revenue risk —
   Domain 7's contracted structures, Domain 11's allocation), lengthen the tenor, or lower the rate.
   The fifth option is to **contribute more equity**, which is what happens when the first four
   fail — and the 828,877 gap is exactly the additional equity Kestrel's sponsors would need to find.

> **Fig 10.1.1 — Debt capacity as a function of coverage and tenor.** Line chart, x-axis target
> `DSCR` 1.10–1.60, y-axis maximum debt (USD m), three lines for 10-, 12- and 15-year tenors at
> 6 %. The 12-year line passes through **41.17m at 1.30×** (marked) and **48.66m at 1.10×**. A
> horizontal reference at the 42.0m request, with its intersection on the 12-year line at
> **1.2743×** annotated "the coverage the requested amount actually delivers". Source: PCI
> original. Alt text: downward-sloping curves showing maximum debt falling as required coverage
> rises, with longer tenors supporting more debt at every coverage level.

### 10.1.3 Sculpting and the shape of debt service

**The problem with level debt service.** An annuity assumes cash flow is level. Projects rarely
oblige: availability payments may escalate, merchant revenue may ramp, and major maintenance may
fall in specific years. Level debt service against uneven cash produces **wasted coverage** in good
years and **breach risk** in weak ones.

**Sculpting** sets each period's debt service so that coverage is constant by design:

```
Debt service in period t = CFADS(t) ÷ target DSCR
```

The principal profile then follows from what is left after interest — irregular by construction,
and that is the point. Sculpted debt supports **more** total debt than level debt against the same
uneven cash flow, because no period is over-covered merely to protect the weakest one. The trade is
complexity: an irregular repayment profile has to be documented, modelled and monitored, and a
sculpted schedule computed on a forecast must be re-cut when the forecast changes (which the
facility agreement provides for, or does not).

Two related shapes worth naming. A **cash sweep** applies a defined share of surplus cash above the
required coverage to accelerated repayment — protecting lenders and shortening the effective tenor
while reducing distributions. A **balloon or bullet** leaves principal at maturity, cutting periodic
debt service and creating refinancing risk, exactly as Domain 3 (Case study B) priced it.

### AI in this KA

Model-building and scenario generation are legitimate machine work at this scale, and there is a
specific failure mode here worth naming: an assistant asked to "size the debt" will apply the
*textbook* `CFADS` definition rather than the facility's, and produce a defensible-looking number
that the lender's own model will contradict. Verification is therefore definitional before it is
arithmetical — check that the model's `CFADS` line implements the documented definition clause by
clause, then recompute one period's ratio by hand. **AI proposes; the professional verifies, decides
and remains accountable.**

### Key terms — KA 10.1

| Term | Meaning |
|---|---|
| **`CFADS`** | Cash available for debt service in a period, on the facility's documented definition. |
| **Debt capacity** | `CFADS`/target `DSCR` × `AF(r, n)`; a function of cash, coverage, rate and tenor only. |
| **Target `DSCR`** | The coverage the lender requires; the divisor in sizing. |
| **Sculpting** | Setting each period's debt service to hold coverage constant against uneven cash. |
| **Cash sweep** | Mandatory prepayment from a defined share of surplus cash. |
| **Balloon / bullet** | Principal deferred to maturity; lower periodic service, refinancing risk. |

### Sample MCQs — KA 10.1

**MCQ 10.1-A `[10.1.2 · Application]`** `CFADS` is 6,384,000 per year; the lender requires a
1.30× `DSCR`; the loan runs 12 years at 6 % (`AF` = 8.383844). Maximum debt is closest to:
- A. USD 42,000,000
- B. USD 41,171,123 ✅
- C. USD 53,522,460
- D. USD 36,143,689

*Rationale:* `6,384,000/1.30 = 4,910,769`; `× 8.383844 = 41,171,123`. A is the requested amount,
which the calculation rejects; C omits the coverage divisor (sizing on full `CFADS`,
`6,384,000 × 8.383844`); D uses a 10-year tenor (`AF = 7.360087`) instead of the 12-year tenor.

**MCQ 10.1-B `[10.1.1 · Analysis]`** Two advisers compute Kestrel's `DSCR` as 1.39 and 1.27 from
the same audited year. The most likely explanation is:
- A. one has made an arithmetic error
- B. they are applying different `CFADS` definitions — one before and one after working-capital movements — and only the facility's definition governs ✅
- C. they are using different interest rates
- D. one has used accounting profit instead of cash

*Rationale:* Domain 2's demonstration exactly: 6,984,000 versus 6,384,000 of `CFADS` on the same
trading. The documented definition decides which is enforceable (10.1.1). D would produce a much
larger discrepancy and is a different error.

**MCQ 10.1-C `[10.1.3 · Analysis]`** A project's cash flow ramps over its first five years. Against
the same forecast, sculpted debt service compared with level debt service will:
- A. support less total debt, being more complex
- B. support more total debt, because no period is over-covered merely to protect the weakest one ✅
- C. support the same debt, since total cash is unchanged
- D. eliminate refinancing risk

*Rationale:* Level service is constrained by the weakest period; sculpting holds coverage constant
and so uses the stronger periods (10.1.3). C ignores that capacity is set period by period, not in
total; D confuses sculpting with tenor structure.

### Self-check — KA 10.1

1. *Name the only four variables debt capacity depends on.* — `CFADS`, target coverage, rate,
   tenor. (Equity contribution is the residual, not a driver.)
2. *Why can two correct `DSCR` figures differ for one year?* — Different `CFADS` definitions; only
   the facility's governs.
3. *What does sculpting buy and what does it cost?* — More debt against uneven cash; complexity in
   documentation, modelling and re-cutting.

---

## Knowledge Area 10.2 — The coverage ratios

*Topics: 10.2.1 `DSCR` · 10.2.2 `LLCR` and `PLCR` · 10.2.3 `ICR` and leverage · 10.2.4 reading a
ratio set together.*

### 10.2.1 `DSCR` — the period test

```
DSCR = CFADS ÷ debt service          (debt service = interest + scheduled principal)
```

`DSCR` is a **period** measure: it asks whether *this* period's cash covers *this* period's
obligations. It is the ratio lenders covenant on, test on defined dates, and lock up distributions
against, because liquidity failures happen in periods, not in averages.

**Worked example 10.2.1 — Kestrel's year-one position, and what breaks it.**

1. **Setup.** `CFADS` **6,384,000**; debt service **5,009,635** (Domain 3's instalment). The
   facility carries a **1.20× covenant** and a **1.15× lock-up**. Compute the `DSCR`, the cash
   headroom to each threshold, and the position under a 20 % `CFADS` shortfall.
2. **Formula.** `DSCR` = `CFADS`/debt service. Threshold cash = debt service × threshold.
3. **Substitution.** `6,384,000 / 5,009,635`. Covenant cash `5,009,635 × 1.20`; lock-up cash
   `× 1.15`. Stress: `6,384,000 × 0.80 = 5,107,200`, then `÷ 5,009,635`.
4. **Result.** `DSCR` **1.2743**. Covenant is breached below `CFADS` of **6,011,562**; distributions
   lock up below **5,761,081**; the project fails to cover debt service at all below **5,009,635**.
   Under a 20 % `CFADS` shortfall, `DSCR` falls to **1.0195** — a **covenant breach**, though the
   project still pays.
5. **Interpretation.** The headroom is **USD 372,438** of annual cash (6,384,000 − 6,011,562) —
   5.8 % of `CFADS`. That is the sentence that belongs in a board paper, not "`DSCR` 1.27": it says
   how much cash may be lost before a covenant is breached, which is what management can actually
   monitor. And the stress case makes the crucial distinction between **breach and default on
   payment**: at 1.0195 the lenders are paid in full and the project is nonetheless in breach, with
   all the consequences of KA 10.4. Covenants bite long before cash runs out — by design, because
   that is when intervention still works.

### 10.2.2 `LLCR` and `PLCR` — the horizon tests

```
LLCR = PV(CFADS over the remaining loan life, discounted at the loan rate) ÷ debt outstanding
PLCR = PV(CFADS over the remaining project life)                          ÷ debt outstanding
```

Where `DSCR` asks about a period, these ask about a **horizon**: is there enough total discounted
cash ahead to repay what is outstanding? `LLCR` looks to the loan's maturity; `PLCR` looks to the
end of the project's economic life, and therefore counts the cash that exists *after* the loan is
due — which is why `PLCR` exceeds `LLCR` whenever the project outlives the loan.

**Worked example 10.2.2 — Kestrel's three ratios, and an identity worth knowing.**

1. **Setup.** `CFADS` 6,384,000 per year (level), debt outstanding 42,000,000, loan rate 6 %, loan
   life 12 years, project life 25 years.
2. **Formula.** As above, with `AF(0.06, 12) = 8.383844` and `AF(0.06, 25) = 12.783356`.
3. **Substitution.** `LLCR = (6,384,000 × 8.383844)/42,000,000`;
   `PLCR = (6,384,000 × 12.783356)/42,000,000`.
4. **Result.** `DSCR` **1.2743** · `LLCR` **1.2743** · `PLCR` **1.9431**.
5. **Interpretation.** `LLCR` equals `DSCR` **exactly** — and that is not a coincidence but an
   **identity**: when `CFADS` is level and debt service is an annuity at the discount rate used, the
   two ratios must coincide, because both are the same cash divided by the same present value.
   A reviewer should use it as a check: level-cash models whose `DSCR` and `LLCR` differ contain an
   inconsistency (usually a discount rate that is not the loan rate, or a `CFADS` line that differs
   between the two calculations). Where cash is *not* level the ratios diverge, and the divergence
   is informative — `LLCR` above `DSCR` means the weak period is early and later cash is stronger.
   `PLCR`'s 1.9431 says something different again: substantial value exists beyond the loan's
   maturity, which is what makes a **tail** and supports refinancing (Domain 15). Lenders
   nevertheless rely on `PLCR` least, because cash beyond their maturity is cash they have no
   contractual claim on.

### 10.2.3 `ICR` and leverage

`ICR` = `EBIT` (or `EBITDA`) ÷ interest — an **accounting** coverage measure that ignores principal
entirely. Domain 2 computed Kestrel's `EBIT`-based interest cover at 2.02× and, on `EBITDA`,
**2.98×**. Leverage measures — debt/`EBITDA`, gearing — describe the balance sheet:
Kestrel's 42,000,000 debt against 18,000,000 equity is **2.33:1**, the 70/30 structure of Domain 1.

Why project lenders covenant on `DSCR` rather than `ICR`: `ICR` can look comfortable while principal
goes unpaid, and it is computed on accounting figures exposed to the classification judgments
Domain 2 (Case study B) showed can flip a covenant without any change in cash. `ICR` appears mainly
in corporate facilities and as a secondary project covenant.

### 10.2.4 Reading a ratio set together

No single ratio is sufficient, and the four answer different questions:

| Ratio | Question | Blind to |
|---|---|---|
| **`DSCR`** | Can this period pay? | Everything outside the period |
| **`LLCR`** | Is there enough cash to loan maturity? | Timing within the loan life |
| **`PLCR`** | Is there value beyond the loan? | The lender's lack of claim on it |
| **`ICR`** | Do earnings cover interest? | Principal; and exposed to accounting judgment |

Kestrel's set — 1.27 / 1.27 / 1.94 / 2.98 — describes a project with adequate but unspectacular
period coverage, no timing distortion, a substantial tail, and comfortable interest cover. The story
a lender reads is: *repayable, thin in a bad year, refinanceable.* That reading, not any single
number, is what credit committees actually decide on.

### AI in this KA

Computing four ratios across a 25-year model is exactly what machines should do, and their outputs
are dangerously plausible because the arithmetic is simple and the *definitions* are not. The
invariants of 10.A.3 are the defence, and the `LLCR` = `DSCR` identity of 10.2.2 is the cheapest
single check available on any level-cash model. Where an AI produces a covenant-compliance summary
across a portfolio of facilities, the verification duty is to test it against the documents on a
sample — because the failure will not be arithmetic, it will be a definition read from the wrong
agreement.

### Key terms — KA 10.2

| Term | Meaning |
|---|---|
| **`DSCR`** | `CFADS` ÷ debt service; the period test lenders covenant on. |
| **`LLCR`** | PV of `CFADS` to loan maturity ÷ debt outstanding; the loan-horizon test. |
| **`PLCR`** | PV of `CFADS` to end of project life ÷ debt outstanding; counts the tail. |
| **Tail** | Project life beyond loan maturity; supports refinancing. |
| **`ICR`** | Earnings ÷ interest; ignores principal, exposed to accounting judgment. |
| **Headroom** | Cash that can be lost before a covenant threshold is crossed. |

### Sample MCQs — KA 10.2

**MCQ 10.2-A `[10.2.1 · Application]`** `CFADS` 6,384,000; debt service 5,009,635. The `DSCR` is:
- A. 1.2743 ✅
- B. 0.7847
- C. 1.3941
- D. 2.5333

*Rationale:* `6,384,000/5,009,635 = 1.2743`. B inverts the ratio; C uses the pre-working-capital
`CFADS` of 6,984,000 (Domain 2's other definition); D divides by interest only.

**MCQ 10.2-B `[10.2.1 · Analysis]`** With a 1.20× covenant and `DSCR` at 1.2743 on `CFADS` of
6,384,000, the most useful figure for a board is:
- A. the `DSCR` of 1.2743
- B. the annual cash headroom of USD 372,438 — 5.8 % of `CFADS` — before the covenant is breached ✅
- C. the debt outstanding of 42,000,000
- D. the loan's remaining tenor

*Rationale:* Headroom states what may be lost before consequence, which management can monitor
(10.2.1). The ratio alone does not convey magnitude, and C and D are facts, not exposures.

**MCQ 10.2-C `[10.2.2 · Analysis]`** A model with level `CFADS` and annuity debt service reports
`DSCR` 1.27 and `LLCR` 1.41. The soundest conclusion is:
- A. the project has a strong tail
- B. there is an inconsistency: with level cash and annuity service at the loan rate the two must be equal ✅
- C. the `LLCR` is correct and the `DSCR` is stale
- D. this is normal and requires no investigation

*Rationale:* The identity of 10.2.2 makes divergence a defect indicator — typically a discount rate
that is not the loan rate, or differing `CFADS` lines. A describes `PLCR`, not `LLCR`.

**MCQ 10.2-D `[10.2.3 · Analysis]`** Why do project lenders covenant primarily on `DSCR` rather
than `ICR`?
- A. `ICR` is harder to compute
- B. `ICR` ignores principal repayment and rests on accounting figures exposed to classification judgment ✅
- C. `ICR` is only used for equity investors
- D. `DSCR` is required by accounting standards

*Rationale:* `ICR` can look healthy while principal is unpaid, and Domain 2 showed accounting
classification flipping a profit-based covenant with no cash change. D confuses covenant drafting
with accounting requirements.

### Self-check — KA 10.2

1. *When must `LLCR` equal `DSCR`?* — Level `CFADS` with annuity debt service discounted at the
   loan rate; divergence signals an inconsistency.
2. *Why do lenders rely least on `PLCR`?* — It counts cash beyond their maturity, on which they have
   no contractual claim.
3. *State Kestrel's headroom to covenant in cash terms.* — USD 372,438 per year, 5.8 % of `CFADS`.

---

## Knowledge Area 10.3 — Reserve accounts and the debt-service schedule

*Topics: 10.3.1 the reserve family · 10.3.2 sizing a debt-service reserve · 10.3.3 the schedule and
the waterfall's top.*

### 10.3.1 The reserve family

Coverage ratios measure adequacy on a forecast; reserves provide liquidity when reality departs
from it. The standard family:

| Reserve | Purpose | Typical sizing |
|---|---|---|
| **Debt service reserve (DSRA)** | Bridge a short cash shortfall so scheduled service is paid | 3–12 months of debt service |
| **Maintenance reserve (MRA)** | Fund lumpy major maintenance without a cash spike | Forward-looking schedule of major overhauls |
| **Insurance / proceeds account** | Hold claim proceeds for application per the documents | Event-driven |
| **Distribution / lock-up account** | Hold cash that fails a distribution test | Event-driven (KA 10.4) |

A reserve is not a covenant substitute: it buys **time**, which converts a liquidity failure into a
negotiation. Its cost is that funded cash is not distributed — which is precisely why sponsors argue
for smaller reserves and lenders for larger ones, and why the negotiation is really about how long
the lenders want before they must act.

### 10.3.2 Sizing a debt-service reserve

**Worked example 10.3.2 — how much time does Kestrel's DSRA buy?**

1. **Setup.** Annual debt service **USD 5,009,635**. The facility requires a **six-month** DSRA.
   `CFADS` is 6,384,000 in the base case. How much must be funded, and what shortfall does it
   absorb?
2. **Formula.** DSRA = debt service × months/12. Absorbable shortfall = DSRA ÷ debt service, as a
   fraction of a period's obligation.
3. **Substitution.** `5,009,635 × 6/12`.
4. **Result.** DSRA **USD 2,504,818**. It covers **half** of one year's debt service — meaning the
   project can absorb a `CFADS` collapse to as low as **2,504,818** (39 % of base case) in a single
   year and still pay in full.
5. **Interpretation.** Express the reserve as the *shortfall it survives*, not as a number of
   months: six months of debt service sounds procedural, while "we can lose 61 % of one year's cash
   and still pay" is a risk statement a board can weigh. Note what it does not do — it does not
   prevent the **covenant breach** (`DSCR` would be far below 1.20 in that year), so the reserve
   buys payment continuity and time to negotiate, not compliance. That distinction is the practical
   content of KA 10.4.

### 10.3.3 The schedule and the waterfall's top

The **debt-service schedule** is Domain 3's amortisation table with a purpose: it fixes the
obligations each coverage test is measured against. Kestrel's year one — interest 2,520,000,
principal 2,489,635, total 5,009,635 — is the denominator of the `DSCR`, and its accuracy is
therefore load-bearing for covenant compliance.

The schedule sits inside the **cash waterfall** (built fully in Domain 15), whose ordering is the
whole security architecture in miniature: operating costs, then taxes, then **senior debt service**,
then reserve top-ups, then subordinated debt, then — only if every test passes — distributions to
equity. Two consequences follow for a leader. **Equity is paid last, and only by permission** —
which is the economic meaning of subordination. And **the reserve top-up sits above distributions**,
so a depleted reserve is refilled before shareholders see anything; a sponsor forecasting
distributions without modelling reserve replenishment has forecast cash that is contractually
unavailable.

### Key terms — KA 10.3

| Term | Meaning |
|---|---|
| **DSRA** | Debt service reserve; buys payment continuity and time, not compliance. |
| **MRA** | Maintenance reserve; smooths lumpy major maintenance. |
| **Debt-service schedule** | The obligation profile that coverage tests are measured against. |
| **Cash waterfall** | The contractual priority order for applying project cash. |
| **Subordination** | Junior claims paid only after senior tests pass. |

### Sample MCQs — KA 10.3

**MCQ 10.3-A `[10.3.2 · Application]`** Annual debt service is 5,009,635 and the facility requires
a six-month DSRA. The amount to be funded is:
- A. USD 5,009,635
- B. USD 2,504,818 ✅
- C. USD 1,252,409
- D. USD 417,470

*Rationale:* `5,009,635 × 6/12 = 2,504,818`. A is twelve months; C is three; D is one month.

**MCQ 10.3-B `[10.3.2 · Analysis]`** In a year when `CFADS` falls to 3,000,000 against debt service
of 5,009,635, a fully funded six-month DSRA means:
- A. no covenant breach occurs, since the reserve covers the gap
- B. scheduled debt service is paid in full from cash plus reserve, but the `DSCR` covenant is still breached ✅
- C. the lenders must accelerate the loan
- D. distributions may continue, since payment was made

*Rationale:* The reserve preserves payment (gap 2,009,635, within the 2,504,818 reserve) but the
ratio is computed on `CFADS`, so the covenant fails (10.3.2). D is wrong because a breach triggers
lock-up (KA 10.4), and C overstates the automatic consequence.

**MCQ 10.3-C `[10.3.3 · Recall]`** In the cash waterfall, reserve-account top-ups rank:
- A. below distributions to equity
- B. above distributions to equity ✅
- C. above senior debt service
- D. at the same level as operating costs

*Rationale:* Reserves are replenished before equity is paid (10.3.3); senior service ranks above the
top-up, and operating costs above both.

### Self-check — KA 10.3

1. *What does a DSRA actually buy?* — Payment continuity and time to negotiate; not covenant
   compliance.
2. *How should a reserve be expressed to a board?* — As the shortfall it survives, not as a number
   of months.
3. *Why must distribution forecasts model reserve replenishment?* — Top-ups rank above equity, so
   unreplenished reserves make forecast distributions contractually unavailable.

---

## Knowledge Area 10.4 — Covenants, default and cure

*Topics: 10.4.1 covenant types · 10.4.2 distribution lock-up · 10.4.3 events of default and cure
rights · 10.4.4 living with covenants.*

### 10.4.1 Covenant types

**Financial covenants** are tested ratios — `DSCR` (historic, forward-looking, or both), `LLCR`,
sometimes leverage or `ICR` — measured on defined dates with defined inputs. **Information
covenants** require reporting: management accounts, compliance certificates, model updates, budgets.
**Positive covenants** require action (maintain insurance, comply with law, operate to standard).
**Negative covenants** prohibit action without consent (no additional debt, no asset disposals, no
material contract amendments, no change of control).

The design intent runs through all four: **give lenders visibility and control while the project is
still fixable.** A covenant set that only bites at payment failure is worthless, because by then the
options have gone (Domain 1's irreversibility point, in credit form).

Two mechanical distinctions matter in practice. **Historic versus forward-looking tests**: a
backward test measures what happened; a forward test (often required for distributions) measures the
projection, which introduces the question of *whose* projection and on what assumptions.
**Cash-based versus accounting-based**: as Domain 2 established, cash-based tests are indifferent to
classification judgments that can flip an accounting covenant with no change in economics — which is
why project facilities lean on `DSCR`.

### 10.4.2 Distribution lock-up

The **lock-up** is the mechanism that makes covenants effective without triggering default: if a
distribution test fails, cash that would have gone to equity is trapped — held in a blocked account,
applied to prepayment, or simply retained. Kestrel's structure has a **1.20× covenant** and a
**1.15× lock-up trigger**, and the sequencing is deliberate: cash is retained *before* a breach
becomes an event of default, so the lenders' exposure reduces while the project stabilises.

The economics for a sponsor are severe and often underappreciated: distributions deferred are equity
returns deferred, and because equity returns are the residual (Domain 1's leverage arithmetic), a
lock-up hits the equity IRR far harder than the ratio shortfall suggests. This is why lock-up
thresholds are negotiated as hard as the covenant itself, and why a sponsor's model must show the
distribution profile **after** lock-up tests, not before.

### 10.4.3 Events of default and cure rights

An **event of default** is a defined breach that entitles lenders to remedies — typically
acceleration (all sums due), enforcement of security, or step-in. In practice lenders rarely
accelerate a fundamentally sound project: enforcement destroys value and leaves them owning an
asset. The realistic path is renegotiation from a much stronger position, which is why the
consequences of default are usually commercial before they are legal.

**Cure rights** are the sponsors' contractual ability to fix a breach — most commonly an **equity
cure**, injecting cash treated as `CFADS` (or applied to prepayment) so the ratio is restored.
Facilities limit them: a maximum number of cures over the loan life, a maximum in consecutive
periods, and rules on whether cure cash counts in the ratio or reduces debt. The negotiation matters
because cure rights convert a technical breach into a funding decision the sponsor controls, and
that is a meaningful option in a downside — Domain 8's optionality, in financing form.

**Waivers and amendments** are the ordinary resolution of a breach: lenders consent, usually for a
fee, often with tightened terms. Domain 15 handles the restructuring end of this spectrum.

### 10.4.4 Living with covenants

The leadership content of this domain is not the arithmetic but the operating posture it implies:

- **Know which covenant binds first.** Kestrel's binds at `CFADS` 6,011,562 — 5.8 % below base case.
  That number, not the ratio, is the operational trigger, and it should sit on the management
  dashboard.
- **Forecast the tests, do not just report them.** A covenant tested next quarter on a forecast that
  is already visibly deteriorating is a conversation to have with lenders *now*, from a position of
  candour, rather than in three months from a position of breach.
- **Never surprise a lender.** The single most valuable asset in a covenant negotiation is a track
  record of early, accurate disclosure; a lender who learns of a problem from a compliance
  certificate will price that discovery into everything afterwards. (Domain 1's honesty asymmetry,
  applied to credit relationships.)
- **Model the lock-up, not just the covenant.** Sponsors are hurt by trapped cash long before
  default, and boards are routinely surprised by a distribution profile nobody tested.

### AI in this KA

Covenant extraction and monitoring is a genuinely strong application: pulling defined terms, test
dates and thresholds out of long agreements, and tracking actuals against them across a portfolio.
Two boundaries. **Definitions are where models fail** — a covenant summary is only as good as its
reading of the definitions clause, and that reading must be verified against the document before
anyone relies on it (Domain 1's document-against-summary check). And **legal consequence is not
decision support**: whether a set of facts constitutes an event of default, and what remedies
follow, is a question for qualified counsel. Use AI to ensure no test is missed; never to conclude
what a breach means.

### Key terms — KA 10.4

| Term | Meaning |
|---|---|
| **Financial / information / positive / negative covenant** | Tested ratios · reporting · required acts · prohibited acts. |
| **Historic vs forward-looking test** | Measured on what happened vs on projection. |
| **Distribution lock-up** | Trapping equity cash when a test fails, short of default. |
| **Event of default** | Defined breach entitling lenders to remedies including acceleration. |
| **Equity cure** | Sponsor cash injected to restore a ratio; limited by the facility. |
| **Waiver / amendment** | Consented departure from terms, usually for a fee and tighter conditions. |

### Sample MCQs — KA 10.4

**MCQ 10.4-A `[10.4.2 · Analysis]`** A facility has a 1.20× `DSCR` covenant and a 1.15× lock-up
trigger. Why is the lock-up set *below* the covenant?
- A. it is a drafting convention with no economic effect
- B. so that cash is trapped only after a breach has occurred
- C. so that cash begins to be retained as coverage deteriorates, reducing exposure before a breach becomes an event of default ✅
- D. because lock-up and covenant tests use different `CFADS` definitions

*Rationale:* Graduated triggers act early while the project is fixable (10.4.2). B misreads the
ordering; a lock-up *below* the covenant level means it engages as the ratio falls through it.

**MCQ 10.4-B `[10.4.3 · Application]`** A `DSCR` covenant is breached and the sponsors have an
unused equity cure. The realistic sequence is:
- A. lenders accelerate and enforce security
- B. sponsors decide whether to inject cure cash; failing that, waiver or amendment negotiations follow, with acceleration a remedy of last resort ✅
- C. the breach is disregarded if payment was made
- D. the loan converts automatically to equity

*Rationale:* Acceleration destroys value for lenders too, so cure, waiver and amendment are the
normal path (10.4.3). C ignores that breach and payment failure are distinct (10.2.1).

**MCQ 10.4-C `[10.4.4 · Analysis]`** Which figure best belongs on a management dashboard for
covenant management?
- A. the current `DSCR`
- B. the `CFADS` level at which the binding covenant fails — 6,011,562, or 5.8 % below base case ✅
- C. the debt outstanding
- D. the loan's maturity date

*Rationale:* The operational trigger is the cash level, which management can influence and monitor
(10.4.4). The ratio alone conveys no magnitude of headroom.

### Self-check — KA 10.4

1. *What does a lock-up achieve that a covenant alone does not?* — It retains cash as coverage
   deteriorates, short of default, reducing exposure while the project is still fixable.
2. *Why do lenders rarely accelerate a sound project?* — Enforcement destroys value and leaves them
   owning the asset; renegotiation from strength is better.
3. *What is the most valuable asset in a covenant negotiation?* — A track record of early, accurate
   disclosure.

---

## Advanced topics — Domain 10

### 10.A.1 Forward-looking tests and whose forecast counts

A forward `DSCR` test measures a projection, which raises three questions the documents must answer:
whose model, on what assumptions, and reviewed by whom. Facilities typically require the borrower's
model updated to an agreed basis, sometimes with the lenders' technical adviser's assumptions
prevailing on specified inputs. The practical consequence is that **assumption control becomes a
covenant matter** — a sponsor who cannot defend an assumption cannot pass a forward test on it — and
it is why Domain 13's model audit and Domain 6's model governance are contractual, not merely
prudent.

### 10.A.2 Refinancing, tails and mini-perms

Kestrel's `PLCR` of 1.9431 against an `LLCR` of 1.2743 quantifies its **tail** — the 13 years of
project life beyond loan maturity. Tails are what make refinancing possible, and structures
deliberately exploit them: a **mini-perm** (a short facility priced and documented in the
expectation of refinancing, sometimes with punitive step-up margins if it is not) trades cheap
early money for refinancing risk, which is Domain 3's Case study B arithmetic at facility scale.
The leader's discipline is to state the refinancing assumption explicitly and stress it, because a
plan that requires a functioning credit market at a specific date has embedded a market forecast in
a financing structure.

### 10.A.3 The reviewer's coverage eye

Invariants to test on any coverage model: `CFADS` reconciles line-by-line to the facility's
definition; the same `CFADS` line feeds `DSCR`, `LLCR` and lock-up tests; **`LLCR` equals `DSCR`
where cash is level and service is an annuity at the loan rate** (10.2.2); `PLCR` ≥ `LLCR` whenever
a tail exists; debt service equals interest plus scheduled principal and ties to the amortisation
schedule; the schedule's closing balance is zero at maturity and total principal equals the loan
(Domain 3's checks); cash tax, not accounting tax, feeds `CFADS`; reserve movements are treated as
the documents specify; every covenant has a modelled test date; and the model's minimum `DSCR` over
the loan life is reported, not merely the average — because covenants are tested in periods, and an
average conceals the one that breaches.

---

## Industry variations — Domain 10

- **Contracted power and availability PPPs.** Revenue certainty supports the lowest coverage
  requirements (often 1.15–1.25× base case), long tenors and high gearing; `DSCR` is tested against
  a tightly defined `CFADS`.
- **Merchant power and commodities.** Coverage requirements rise sharply (1.5× and above), tenors
  shorten, and lenders size on stressed rather than base-case cash — often on a bank case using
  conservative price decks.
- **Transport concessions.** Patronage risk produces ramp-up profiles that make sculpting close to
  mandatory, and lenders commonly require larger reserves through the ramp.
- **Water and regulated utilities.** Regulatory reset cycles create step-changes in cash, so
  covenant testing and reserve sizing must straddle resets rather than assume continuity.
- **Digital infrastructure.** Shorter asset lives compress tails, so `PLCR` provides less comfort
  and structures rely more on contracted tenant credit and refinancing at a defined point.

## Case study — Domain 10: the 828,877 that changed the structure (water)

**Situation.** Kestrel's sponsors sought USD 42,000,000 of senior debt against a base-case `CFADS`
of 6,384,000. The lead bank's credit committee required a **1.30×** base-case `DSCR`.

**The arithmetic.** At 1.30×, sustainable debt service is 4,910,769, supporting
`4,910,769 × AF(0.06, 12) = 41,171,123` — **828,877 short** of the request. At 42,000,000 the
`DSCR` is 1.2743 and the `LLCR`, on level cash, is identically 1.2743.

**The four levers, worked.** *Raise `CFADS`*: the sponsors first proposed adding back the 600,000
working-capital movement to reach 6,984,000 and a 1.39× ratio — rejected, because the facility's own
definition was struck after working capital (Domain 2's point, now with a price attached).
*Reduce required coverage*: the bank agreed **1.25×** in exchange for a longer offtake term and a
tightened lock-up at 1.15×, which on base-case cash lifts capacity to `6,384,000/1.25 × 8.383844 =`
**42,817,968** — apparently enough. But the bank sizes on its own **stressed case**, and at a
`CFADS` 5 % lower (6,064,800) the same 1.25× supports only **40,677,069**, which is *less* than the
1.30× base-case answer. The concession was therefore worth less than it appeared, and this is the
recurring trap in coverage negotiations: **a lower ratio applied to a lower cash case is not a
concession.** *Lengthen tenor*: 15 years was unavailable against a 25-year concession with a
required tail. *Lower the rate*: not negotiable in the market of the day.

**The outcome.** A structure at **41,000,000** senior debt — debt service `41,000,000/8.383844 =`
**4,890,358**, a base-case `DSCR` of **1.3054**, comfortably inside the 1.30× requirement — with the
residual **1,000,000 funded as additional equity**, a six-month DSRA (2,504,818), sculpted service
through the two-year ramp, and a 1.15× lock-up. Equity rose from 18,000,000 to 19,000,000, gearing
from 70/30 to **68.3/31.7** (debt/equity 2.16:1), and the sponsors' modelled IRR fell by roughly
40 basis points — the true price of the coverage requirement, paid in equity rather than argued away.

**What the domain teaches here.** Debt capacity is arithmetic, and the negotiation is about its four
inputs — not about the number itself. Every attempt to move the answer without moving an input
(redefining `CFADS`, quoting a more flattering ratio) fails at the first competent review, and the
attempt costs credibility that the later, genuine negotiation needs.

## Case study B — Domain 10: paid in full and in breach (transport)

**Situation.** A toll-road SPV entered its third operating year with patronage **12 % below
forecast**. Because most of a road's operating cost is fixed, `CFADS` fell further than revenue: from
a base-case 24,000,000 to **19,700,000**, a fall of **17.9 %** — the operating-leverage effect a
sponsor should model rather than discover. Against debt service of 18,600,000 that is a `DSCR` of
**1.0591**, versus a **1.25× covenant** and a **1.15× lock-up**. The DSRA (six months, 9,300,000)
was fully funded and untouched.

**What happened.** Debt service was paid in full and on time, from operating cash alone. The project
was nonetheless in breach of covenant from the first test date, distributions locked up
automatically at the 1.15× trigger, and the sponsors — who had modelled distributions against the
covenant rather than the lock-up — had budgeted dividends that were contractually unavailable.
The board's initial position, that "the lenders are being paid, so there is no problem", was
precisely wrong and cost two months of credibility before the finance director corrected it
internally.

**How it resolved.** The sponsors injected an equity cure of **3,700,000**, lifting cure-adjusted
`CFADS` to 23,400,000 and the ratio to **1.2581×** — back above the covenant, and deliberately above
the bare minimum of `18,600,000 × 1.25 − 19,700,000 =` **3,550,000**, because a cure computed to the
last dollar breaches again on any further slippage. They used one of their two permitted cures, and
agreed an amended covenant profile stepping from
1.15× to 1.25× over three years in exchange for a fee, a cash sweep of 50 % of surplus above 1.30×,
and monthly rather than quarterly reporting. Patronage recovered to within 6 % of forecast by year
five and the sweep retired debt ahead of schedule.

**What the domain teaches here.** Breach and payment failure are different events, and the covenant
is designed to bite first — while cure, waiver and amendment are all still available. The sponsors'
avoidable error was modelling the covenant and not the lock-up, which is the specific discipline of
KA 10.4.2.

---

## Executive perspective — Domain 10

What a project finance director cannot delegate in this domain:

- **The `CFADS` definition.** Read it in the document, clause by clause, and confirm the model
  implements it. Every ratio the project will be judged on is built on that one term (Case study A).
- **The binding trigger in cash.** Not the ratio — the `CFADS` level at which the first covenant
  fails (6,011,562 for Kestrel, 5.8 % below base case), on the dashboard, owned.
- **The distribution profile after lock-up tests.** Boards are surprised by trapped cash, and the
  surprise is always avoidable (Case study B).
- **The minimum, not the average.** Minimum `DSCR` across the loan life is the covenant-relevant
  number; averages conceal the period that breaches.
- **The refinancing assumption.** If the structure needs a market at a date, say so explicitly and
  stress it (10.A.2).
- **The relationship.** Early, accurate disclosure is the asset that determines the terms of every
  future waiver — and it is built before it is needed.

## Calculation exercises — Domain 10

**Exercise 10.1** `CFADS` 9,200,000; target `DSCR` 1.35×; loan 10 years at 7 %. Compute maximum
debt service and maximum debt.
*Solution.* Max debt service `9,200,000/1.35 =` **6,814,815**; `AF(0.07, 10) = 7.023582`;
max debt `6,814,815 × 7.023582 =` **USD 47,864,408**. Common error: sizing on full `CFADS`
(9,200,000 × 7.023582 = 64,616,950), which omits the coverage requirement entirely.

**Exercise 10.2** Debt outstanding 47,864,408; `CFADS` 9,200,000 level; loan life 10 years at 7 %;
project life 20 years. Compute `LLCR` and `PLCR`, and verify the `DSCR` identity.
*Solution.* `LLCR = (9,200,000 × 7.023582)/47,864,408 =` **1.3500**; `AF(0.07, 20) = 10.594014`,
`PLCR = (9,200,000 × 10.594014)/47,864,408 =` **2.0363**. `DSCR = 9,200,000/6,814,815 =` **1.35** —
equal to `LLCR`, as the identity requires with level cash and annuity service. Common error:
discounting `CFADS` at the project's equity discount rate rather than the loan rate, which breaks
the identity and the comparability.

**Exercise 10.3** A facility has debt service of 6,814,815, a 1.20× covenant and a 1.10× lock-up.
State the `CFADS` levels at which each triggers, and the headroom from a base case of 9,200,000.
*Solution.* Covenant at `6,814,815 × 1.20 =` **8,177,778**; lock-up at `× 1.10 =` **7,496,297**.
Headroom to covenant `9,200,000 − 8,177,778 =` **1,022,222**, or **11.1 %** of base-case `CFADS`.
Common error: quoting headroom in ratio points (0.15×), which conveys no magnitude.

**Exercise 10.4** `CFADS` falls 25 % from 9,200,000. Compute the `DSCR` against debt service of
6,814,815 and state whether the 1.20× covenant holds and whether payment is made.
*Solution.* `CFADS` **6,900,000**; `DSCR = 6,900,000/6,814,815 =` **1.0125**. The covenant is
**breached** (1.0125 < 1.20) but **scheduled debt service is still paid** from cash alone
(6,900,000 > 6,814,815) — the distinction of KA 10.2.1. Common error: treating breach and payment
default as the same event.

## Practitioner's toolkit — Domain 10

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 10.T.1 — `CFADS` definition reconciliation

Column 1: each line of the facility's `CFADS` definition, with its clause reference. Column 2: the
model line implementing it. Column 3: included/excluded and on what basis (cash or accrued; above or
below `CFADS`). Column 4: the person who confirmed the match, and the date. Rule: no coverage ratio
is reportable until every definition line has a confirmed model line (Domain 2's defined-terms
sheet, made specific to `CFADS`).

### Toolkit 10.T.2 — Covenant dashboard (per facility, per test date)

Per covenant: test name and clause · test date and frequency · historic or forward-looking ·
threshold · current/forecast value · **the `CFADS` level at which it triggers** · cash headroom in
currency and as a percentage of base case · lock-up threshold and its own trigger level · reserve
balances against required levels · cures used and remaining · reporting obligations due. Front line:
**which covenant binds first, and by how much cash.**

### Toolkit 10.T.3 — Coverage model check (before any ratio is quoted)

- [ ] `CFADS` reconciles to the documented definition (10.T.1 complete).
- [ ] The same `CFADS` line feeds `DSCR`, `LLCR` and every distribution test.
- [ ] `LLCR` = `DSCR` where cash is level and service is an annuity at the loan rate.
- [ ] `PLCR` ≥ `LLCR` wherever a tail exists.
- [ ] Debt service = interest + scheduled principal, tied to the amortisation schedule.
- [ ] Schedule closes at zero; Σ principal = loan drawn.
- [ ] Cash tax, not accounting tax, in `CFADS`.
- [ ] **Minimum** `DSCR` over the loan life reported alongside the average.
- [ ] Every covenant has a modelled test date; lock-up modelled as well as covenant.
- [ ] AI-produced covenant summaries sampled against the documents; verifier named.

## Exam preparation — Domain 10

**The traps.** Sizing debt on full `CFADS` without the coverage divisor (Exercise 10.1) · quoting a
`DSCR` without its `CFADS` definition (10.1.1) · treating covenant breach and payment default as the
same event (Exercise 10.4, Case study B) · discounting `CFADS` at an equity rate in `LLCR`
(Exercise 10.2) · reporting average rather than minimum `DSCR` · modelling the covenant but not the
lock-up (10.4.2) · assuming a DSRA prevents breach (MCQ 10.3-B) · using accounting rather than cash
tax in `CFADS` · inverting `DSCR` · relying on `PLCR` as a lender comfort.

**Reflection questions.**
1. For your facility: state the `CFADS` definition from memory, then check it against the document.
   How close were you, and what would the difference have done to your reported ratio?
2. At what `CFADS` level does your first covenant fail, and is that number on anyone's dashboard?
3. Does your distribution forecast pass the lock-up tests, or only the covenant tests?

## Domain 10 summary

Project debt is sized from cash flow outwards, and the arithmetic admits only four inputs: `CFADS`,
required coverage, rate and tenor. Kestrel's request for 42,000,000 against level `CFADS` of
6,384,000 supports only **41,171,123** at a 1.30× target — an 828,877 gap closed by equity, because
every attempt to close it by redefining `CFADS` fails at the first competent review. `CFADS` itself
is a **defined term** whose documented construction governs every ratio built on it, as Domain 2
proved by moving Kestrel's `DSCR` from 1.39 to 1.27 with one working-capital treatment. The ratios
answer different questions: `DSCR` tests a period (1.2743, with covenant headroom of **372,438** of
annual cash — the number that belongs on a dashboard); `LLCR` tests the loan horizon and, with level
cash and annuity service, is **identically equal to `DSCR`** — an invariant that catches model
inconsistency; `PLCR` (1.9431) counts the tail that makes refinancing possible but that lenders have
no claim on; and `ICR` covers interest while ignoring principal and inheriting accounting judgment.
Reserves buy payment continuity and time rather than compliance — Kestrel's six-month DSRA of
**2,504,818** survives a collapse to 39 % of base-case cash while the covenant still breaches — and
they rank above distributions in the waterfall. Covenants exist to give lenders control while the
project is fixable, with lock-up trapping cash short of default, cure rights giving sponsors a
funding option, and waiver or amendment as the ordinary resolution; breach and payment failure are
different events, and confusing them is the error Case study B cost two months of credibility.
Domain 11 allocates the risks these ratios are stressed against; Domain 13 audits the model that
computes them; Domain 15 operates the waterfall and handles the restructuring end.
