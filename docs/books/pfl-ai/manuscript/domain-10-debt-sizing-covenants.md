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
target `DSCR`, and distinguish a base-case test from a minimum-period test; show why exchanging a
lower coverage requirement for a lower cash case is not necessarily a concession, and compute the
stress at which the exchange turns negative; build a sculpted debt-service schedule against an
uneven `CFADS` profile, including the tax circularity, and derive the effective rate at which
sculpted debt discounts; compute and interpret `DSCR`, `LLCR`, `PLCR` and `ICR`, state what each
sees that the others do not, and show which of them repayment structure can and cannot move;
stress a coverage position against several covenants at once and identify the covenant that binds
first; explain reserve accounts, size a debt-service reserve, express it as the shortfall it
survives, and choose between funding it with equity, debt or a letter of credit; compute the effect
of a cash sweep on the repayment profile and the loan's average life; describe covenant types, the
lock-up mechanism, events of default and cure rights; size an equity cure under both of the
standard drafting treatments; explain why lenders prefer cash-based to accounting-based covenants;
and govern AI-assisted covenant and model analysis.

**The master financing.** Kestrel Water SPC continues from Domains 1–4. Its senior loan is
**USD 42,000,000 at 6.0 % over 12 years** (Domain 3's schedule: annual instalment
**USD 5,009,635**, year-one interest **USD 2,520,000**, year-one principal **USD 2,489,635**).
Domain 2 derived its first-year **`CFADS` of USD 6,384,000** on the facility's documented definition
— after working-capital movements. Equity at close was **USD 18,000,000**, giving 70/30 gearing. The
project's operating life is **25 years**; the loan's is 12.

**The `CFADS` relation this domain works with.** Several of the calculations below need `CFADS` as a
*function* rather than as a single figure, and Domains 2 and 6 have already supplied every component
of one. On Kestrel's bank case — revenue `R` flat at 12,000,000, cash operating cost 4,500,000 of
which 750,000 varies with output, depreciation 2,400,000, tax at 20 % of taxable profit, a
600,000 working-capital movement — `CFADS` in a period is

```
CFADS = 0.75 × R − 3,120,000 + 0.20 × I(t)
```

where `I(t)` is that period's interest. At `R` = 12,000,000 and `I` = 2,520,000 it returns
**6,384,000**, and with interest fixed at the year-one figure it collapses to Domain 6's
one-variable form `CFADS = 0.75 R − 2,616,000` (Domain 6, Exercise 6.4). Two consequences run
through the whole domain. **A revenue shock moves `CFADS` by 0.75 of itself**, not by all of it,
because variable cost and tax absorb a quarter of the loss — which is why headroom quoted in
`CFADS` terms overstates the room available to a team that manages revenue. And **`CFADS` depends
on the debt**, through the interest deduction: as the loan amortises the tax shield shrinks, cash
tax rises and `CFADS` falls. Domain 6 (Worked example 6.4.1b) computed the consequence on this
facility — `CFADS` declining from 6,384,000 in year one to **5,936,713** in year twelve, `DSCR`
from **1.2743** to **1.1851**, `LLCR` **1.2395** rather than the level-case 1.2743. That declining
profile, not the level illustration, is the case this domain sizes, sculpts and stresses against.

**Kestrel's three coverage thresholds**, which are used throughout and are routinely conflated: a
**1.25× distribution condition** (cash may leave the structure only above it), a **1.20× financial
covenant** (below it the facility is in breach), and a **1.15× lock-up trigger** (below it retained
cash is blocked or applied to prepayment rather than merely undistributed). In cash they are
**6,262,044**, **6,011,562** and **5,761,081** of annual `CFADS`. Which of them actually binds is
the subject of KA 10.4.

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
   One thing this calculation quietly assumes deserves stating, because most sizing arguments turn
   on it: **it is a single-period test.** `CFADS` was taken as level and the coverage requirement was
   applied once, so 41,171,123 is the debt that clears 1.30× *in the period tested* — conventionally
   the first full operating year, which is why lenders call it the base-case test. A requirement that
   1.30× be met in **every** period is a different and much tighter constraint, and against Kestrel's
   actual declining profile it produces a materially smaller answer (Worked example 10.1.3). A leader
   who agrees a target `DSCR` without agreeing whether it is tested on the base case or on the
   minimum has agreed the easy half of the term.

> **Fig 10.1.1 — Debt capacity as a function of coverage and tenor.** Line chart, x-axis target
> `DSCR` 1.10–1.60, y-axis maximum debt (USD m), three lines for 10-, 12- and 15-year tenors at
> 6 %. The 12-year line passes through **41.17m at 1.30×** (marked) and **48.66m at 1.10×**. A
> horizontal reference at the 42.0m request, with its intersection on the 12-year line at
> **1.2743×** annotated "the coverage the requested amount actually delivers". Source: PCI
> original. Alt text: downward-sloping curves showing maximum debt falling as required coverage
> rises, with longer tenors supporting more debt at every coverage level.

**Worked example 10.1.2b — the coverage-for-case exchange, and the stress at which it turns
against you.**

1. **Setup.** The most common concession offered in a sizing negotiation is a lower target `DSCR`.
   The most common condition attached to it is that the lower ratio be applied to the lender's own
   stressed case rather than to the sponsor's base case. Kestrel's lead bank offers **1.25×** in
   place of **1.30×**, applied to a case **5 %** below the base-case `CFADS` of 6,384,000. Rate 6 %,
   tenor 12 years, `AF(0.06, 12) = 8.383844`. Is that a concession, and at what stress does the
   answer change?
2. **Formula.** Capacity = `CFADS ÷ target DSCR × AF(r, n)`. Two offers give the same capacity when
   `CFADS × (1 − s) ÷ λ₂ = CFADS ÷ λ₁`, so the **indifference stress** is
   `s* = 1 − λ₂/λ₁` — a function of the two ratios alone, independent of `CFADS`, rate and tenor.
3. **Substitution.** `s* = 1 − 1.25/1.30`. Capacity at 1.30× on base cash
   `6,384,000/1.30 × 8.383844`; capacity at 1.25× on `6,384,000 × 0.95 = 6,064,800`, that is
   `6,064,800/1.25 × 8.383844`.
4. **Result.** Indifference stress **s\* = 3.8462 %**, equivalent to a `CFADS` of
   **USD 6,138,462**. The bank's stress of 5 % exceeds it, so the offer is worth **less** than the
   term it replaces: **40,677,069** against **41,171,123**, a loss of **USD 494,053** of debt
   capacity for what was presented as a five-basis-point-of-coverage relaxation.
5. **Interpretation.** The general rule is worth memorising in the form the algebra gives it:
   **a coverage relaxation from `λ₁` to `λ₂` is worth exactly the stress `1 − λ₂/λ₁`, and nothing
   more.** Moving from 1.30× to 1.25× buys 3.8462 % of cash; moving from 1.30× to 1.20× buys
   **7.6923 %**. Any stressed case deeper than that leaves the sponsor worse off, and because the
   result is independent of rate, tenor and the absolute size of `CFADS`, it can be computed in the
   meeting rather than after it. Three professional points follow. First, **the two halves of the
   term must be negotiated together or not at all** — a sponsor who concedes the case definition on
   Tuesday and negotiates the ratio on Thursday has given away the only lever that matters, which is
   the same error Domain 15 (KA 15.3.4) identifies in refinancings negotiated on margin before sweep.
   Second, the rule shows what a *genuine* concession looks like: the bank must either hold the case
   and move the ratio, or move the ratio by more than the case costs — here 1.25× on a 5 % stress
   would need to fall to `1.30 × 0.95 =` **1.2350×** to be neutral. Third, the caution: this is an
   arithmetic equivalence, not a risk equivalence. A stressed case and a base case are different
   statements about the world, and a lender who insists on sizing against a stress it believes has
   done something a ratio adjustment cannot undo — which is why the honest response to the offer is
   usually to argue about the stress's evidence base (Domain 7's demand work) rather than to trade
   ratio points against it.

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

**The circularity, and the rate that resolves it.** Sculpting looks like a one-line rule and is not,
because in any structure where interest is tax-deductible `CFADS` depends on the interest, the
interest depends on the balance, and the balance depends on the debt service that `CFADS` was
supposed to determine. Domain 6 (KA 6.3.1) names the three honest resolutions — charge on the
opening balance, iterate to convergence, or solve algebraically — and for a sculpted schedule the
algebra is available in closed form, which is much the best answer because it is deterministic.
Write `CFADS(t) = A + T × I(t)` where `A` is the cash flow before the tax shield and `T` the tax
rate, and let `λ` be the target coverage. Then `DS(t) = (A + T·r·B(t−1))/λ` and the balance rolls
forward as

```
B(t) = B(t−1) × (1 + r − T·r/λ) − A/λ
```

so requiring `B(n) = 0` gives, exactly,

```
Maximum sculpted debt = (A ÷ λ) × AF(r*, n)      with the effective sculpting rate  r* = r × (1 − T/λ)
```

The tax shield therefore does not merely add cash: it **discounts the sculpted profile at a lower
rate than the loan rate**, because every unit of interest returns `T` of tax saving into
the same `CFADS` line the coverage test divides. That single expression removes the circularity, and
it is the reason a sculpted structure should never be built by iterating a workbook until the
closing balance looks like zero.

**Worked example 10.1.3 — sculpting Kestrel's declining profile, and the three different answers to
"how much can it borrow at 1.30×".**

1. **Setup.** Kestrel's bank case: `CFADS(t) = 5,880,000 + 0.20 × I(t)` (the relation derived in
   this domain's opening, with revenue flat at 12,000,000), loan rate **6.0 %**, tenor **12 years**,
   tax **20 %**, required coverage **1.30×**. Size the facility three ways — level service tested on
   the base case, level service tested on the minimum period, and sculpted service holding 1.30× in
   every period — and reconcile the differences.
2. **Formula.** Base-case level: `CFADS(1)/λ × AF(r, n)` (10.1.2). Minimum-period level: the binding
   period is the last, where `I = r·DS/(1 + r)` because the final payment retires the balance, so
   `DS = A ÷ (λ − T·r/(1 + r))` and debt = `DS × AF(r, n)`. Sculpted:
   `(A/λ) × AF(r*, n)` with `r* = r(1 − T/λ)`.
3. **Substitution.** Base-case level: `6,384,000/1.30 × 8.383844`. Minimum-period level:
   `5,880,000 ÷ (1.30 − 0.20 × 0.06/1.06) = 5,880,000/1.28867925`, then `× 8.383844`. Sculpted:
   `r* = 0.06 × (1 − 0.20/1.30) = 0.05076923`; `AF(0.05076923, 12) = 8.824924`; debt
   `= 5,880,000/1.30 × 8.824924 = 4,523,076.92 × 8.824924`.
4. **Result.**

   | Sizing basis | Debt service | Debt supported | `DSCR` year 1 | `DSCR` year 12 |
   |---|---|---|---|---|
   | Level, base-case (year-one) test at 1.30× | 4,910,769.23 | **41,171,123** | 1.2980 | **1.2087** |
   | Level, minimum-period test at 1.30× | 4,562,811.13 | **38,253,896** | 1.3893 | 1.3000 |
   | **Sculpted, 1.30× in every period** | 4,891,530.57 falling to 4,562,811.13 | **39,915,812** | 1.3000 | 1.3000 |

   The sculpted schedule, in outline: year one interest 2,394,949, `CFADS` 6,358,990, debt service
   4,891,531, principal 2,496,582; year six debt service 4,763,995 on principal 3,198,030; year
   twelve debt service 4,562,811 retiring the closing balance of 4,304,539 exactly. Total debt
   service **56,888,034**, of which **16,972,222** is interest.
5. **Interpretation.** Start with the number that decides the negotiation: **insisting that 1.30× be
   met in every period, with level service, costs 2,917,226 of debt capacity** — 41,171,123 against
   38,253,896 — and **sculpting recovers 1,661,916 of it**, or 56.97 %, without relaxing the
   requirement by a single basis point in any period. That is the whole commercial case for sculpting
   in two figures, and it is available on any declining or ramping profile. Then notice the
   structural identity hiding in the table: **the sculpted schedule's final payment, 4,562,811.13,
   is exactly the level payment that a minimum-period test would have imposed for the whole loan.**
   That is not a coincidence but the same equation solved twice, and it is the clearest statement of
   what level sizing does wrong against uneven cash — *it makes every year pay what only the worst
   year needed to pay.* Three further readings and two cautions. The **effective sculpting rate**
   of 5.076923 % against a 6.0 % loan rate is a real and often-missed effect: at a 30 % tax rate the
   same structure would discount at `0.06 × (1 − 0.30/1.30) =` 4.6154 %, so **the value of sculpting
   rises with the tax rate**, which is a jurisdictional matter and must be checked with local tax
   advice rather than assumed (Domain 9's treatment of deductibility, and the counsel pointer
   there). The **sculpted year-one `CFADS` of 6,358,990 is lower than the base case's 6,384,000**,
   because a smaller loan generates less interest and therefore a smaller shield — a reviewer who
   sculpts against a `CFADS` line computed on the *original* debt has over-sized the facility, and
   this is the commonest error in sculpted models. And the sculpted profile here **declines**
   (4,891,531 down to 4,562,811), which is the opposite of the back-ended shape most practitioners
   associate with sculpting: the shape follows the cash, and Kestrel's cash falls because its tax
   shield amortises. The cautions are that a sculpted schedule is **computed on a forecast** — if
   revenue departs from 12,000,000 the profile is wrong in every remaining period, not just the
   current one, so the facility must say whether and how it is re-cut, and who pays for the exercise
   — and that an irregular profile is materially harder to hedge, because an interest-rate swap has
   to be written against an amortisation schedule that is no longer a standard shape (Domain 11,
   KA 11.3). Domain 7 (KA 7.A.2) works the same machinery against a contract cliff rather than a tax
   shield and reaches the same conclusion from the other direction: level service against uneven cash
   destroyed 11,185,896 of capacity there.

> **Fig 10.1.2 — Three ways to size the same project at 1.30×.** Line chart, x-axis loan years 1–12,
> y-axis `DSCR` 1.18–1.42. Three series against Kestrel's declining bank-case `CFADS`: a slate line
> for level service sized on the **base-case** test (41,171,123), falling from **1.2980** to
> **1.2087**; a grey line for level service sized on the **minimum-period** test (38,253,896),
> falling from **1.3893** to **1.3000**; and a brand-blue **flat** line at **1.3000** for the
> sculpted structure (39,915,812). A crimson dashed horizontal at the 1.30× requirement, and a
> crimson dashed horizontal at the **1.20× covenant** with the base-case line's year-twelve 1.2087
> marked "inside the covenant, outside the sizing test". Right-hand labels give each structure's debt
> in USD m (**41.17 · 38.25 · 39.92**) and the header states the two results: the minimum-period test
> costs **2,917,226** of capacity, of which sculpting recovers **1,661,916**. Source: PCI original.
> Alt text: three coverage profiles over twelve years, two sloping and one flat, showing that only
> the sculpted structure holds the required coverage in every period while carrying more debt than
> the level structure sized on the worst year.

**Two related shapes, and what they are really buying.** A **cash sweep** applies a defined share of
surplus cash above the required coverage to accelerated repayment — protecting lenders and
shortening the effective tenor while reducing distributions; its mechanics and its effect on the
loan's average life are worked at 10.3.3, and Domain 15 (KA 15.3.4) prices it as the cost of a
consent. A **balloon or bullet** leaves principal at maturity, cutting periodic debt service and
creating refinancing risk, exactly as Domain 3 (Case study B) priced it. A balloon deserves one
piece of arithmetic here, because it is the structural answer to 10.1.2's shortfall: the balloon
that would let 42,000,000 clear a 1.30× base-case test is
`(42,000,000 − 4,910,769.23 × 8.383844) ÷ 1.06⁻¹² =` **USD 1,667,864**, only **3.97 %** of the
facility. In other words the 828,877 capacity gap that Case study A closes with equity could instead
be closed by deferring under four per cent of the principal to maturity — which is precisely why the
next Knowledge Area asks what the coverage ratios do and do not notice when structure changes
(Worked example 10.2.3).

### AI in this KA

Model-building and scenario generation are legitimate machine work at this scale, and there is a
specific failure mode here worth naming: an assistant asked to "size the debt" will apply the
*textbook* `CFADS` definition rather than the facility's, and produce a defensible-looking number
that the lender's own model will contradict. Verification is therefore definitional before it is
arithmetical — check that the model's `CFADS` line implements the documented definition clause by
clause, then recompute one period's ratio by hand. A sculpted schedule adds a second, purely
arithmetical check that is worth automating: a machine-built sculpt must reproduce
`(A/λ) × AF(r(1 − T/λ), n)` to the cent, and a solver that has merely iterated until the closing
balance rounds to zero will not. **AI proposes; the professional verifies, decides and remains
accountable.**

### Key terms — KA 10.1

| Term | Meaning |
|---|---|
| **`CFADS`** | Cash available for debt service in a period, on the facility's documented definition. |
| **Debt capacity** | `CFADS`/target `DSCR` × `AF(r, n)`; a function of cash, coverage, rate and tenor only. |
| **Target `DSCR`** | The coverage the lender requires; the divisor in sizing. |
| **Sculpting** | Setting each period's debt service to hold coverage constant against uneven cash. |
| **Effective sculpting rate `r*`** | `r × (1 − T/λ)`; the rate at which a sculpted profile discounts once the interest tax shield feeds back into `CFADS`. |
| **Base-case vs minimum-period test** | Coverage required in the period tested (usually year one) against coverage required in every period; on Kestrel the difference is 2,917,226 of debt. |
| **Indifference stress `s*`** | `1 − λ₂/λ₁`; the cash stress at which a lower coverage requirement stops being a concession. |
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

**MCQ 10.1-D `[10.1.3 · Application]`** `CFADS` before the interest tax shield is 5,880,000, the
loan rate is 6.0 %, tax is 20 %, the tenor 12 years and the target coverage 1.30×. The maximum
sculpted debt is closest to:
- A. USD 41,171,123
- B. USD 39,915,812 ✅
- C. USD 37,920,771
- D. USD 38,253,896

*Rationale:* `r* = 0.06 × (1 − 0.20/1.30) = 0.05076923`; `AF(0.05076923, 12) = 8.824924`;
`5,880,000/1.30 × 8.824924 = 39,915,812` (10.1.3). A is the level, base-case answer on year-one
`CFADS` of 6,384,000, which does not hold 1.30× in later periods; C ignores the tax shield's
feedback and discounts at the full 6.0 % (`4,523,076.92 × 8.383844`); D is level service sized on the
minimum period, which sculpting beats by 1,661,916.

**MCQ 10.1-E `[10.1.2 · Evaluation]`** A lender offers to reduce the target `DSCR` from 1.30× to
1.25× provided the test is run on a case 5 % below base. The sponsor should conclude that:
- A. the offer is a concession worth 5 basis points of coverage
- B. the offer destroys 494,053 of capacity, because the indifference stress at these two ratios is only 3.8462 % ✅
- C. the offer is neutral, since ratio and case move in opposite directions
- D. the offer cannot be evaluated without knowing the rate and tenor

*Rationale:* `s* = 1 − 1.25/1.30 = 3.8462 %`, so a 5 % stress more than absorbs the relaxation:
40,677,069 against 41,171,123 (10.1.2b). A prices the ratio and ignores the case; C asserts an
offset without testing its size; D is wrong because `s*` depends only on the two ratios — rate and
tenor scale both sides equally.

**MCQ 10.1-F `[10.1.3 · Evaluation]`** Sizing Kestrel at a 1.30× target gives 41,171,123 on the
base-case test, 38,253,896 on the minimum-period test and 39,915,812 sculpted. An arranger asks the
sponsor's adviser to confirm "the debt capacity". The soundest answer is:
- A. 41,171,123, since the base-case test is what the market conventionally applies
- B. that none of the three is "the" capacity: the figure is meaningless until the term states whether
  coverage is tested on the base case or in every period, and whether service is level or sculpted —
  so the sizing basis must be agreed before any number circulates ✅
- C. 39,915,812, because sculpting is the technically superior structure
- D. 38,253,896, because prudence requires the lowest of the available answers

*Rationale:* The three answers differ by 2,917,226 on identical cash flows, and the whole difference
is definitional — it belongs in the term sheet, not in a footnote (10.1.3, Toolkit 10.T.4). A
concedes the tested period without negotiating it, which is the easy half of the term. C recommends a
structure before its documentation, modelling and re-cutting cost has been weighed, and before the
facility has said who re-cuts a sculpted profile when the forecast moves. D mistakes conservatism for
analysis and surrenders the 1,661,916 that sculpting recovers without relaxing any period's
requirement by a basis point.

**MCQ 10.1-G `[10.1.2 · Comprehension]`** A 1.30× target tested on the base case and the same 1.30×
required in every period differ in that:
- A. they are the same requirement expressed in two ways
- B. the first requires the coverage only in the period tested — conventionally the first full
  operating year — while the second requires it in the weakest period, so on any uneven profile the
  second is the tighter constraint on the same cash flow ✅
- C. the minimum-period test is the looser of the two, because it disregards the early years
- D. the distinction matters only where cash flow is level

*Rationale:* One ratio, two tests; the gap between them is a property of the shape of the cash flow
rather than of the ratio (10.1.2, 10.1.3). A ignores that a single-period test says nothing about the
other eleven periods. C inverts the definition — the minimum-period test binds precisely because it
is measured on the worst period. D reverses the condition: where cash is level the two coincide, and
it is unevenness that separates them.

**MCQ 10.1-H `[10.1.2 · Evaluation]`** The sponsors have asked for 42,000,000 and the 1.30× target
supports 41,171,123, leaving the **828,877** gap. Kestrel's offtake runs 25 years inside a 27-year
concession and the facility is drawn for 12. Of the resolutions available, the one a leader should
test **first** is:
- A. contribute the 828,877 as additional equity, which is the arithmetic residual and closes the gap
  with certainty
- B. one additional year of tenor: at 13 years the same 1.30× target supports **43,473,483**, which
  clears the request with 1,473,483 to spare, and the concession and offtake terms plainly accommodate
  a 13-year facility ✅
- C. argue the target down from 1.30× to the 1.2743 the requested amount delivers, since that is the
  coverage the project actually produces
- D. raise `CFADS`, which is the only lever that improves the lender's position as well as the
  sponsors'

*Rationale:* Debt capacity depends on cash, coverage, rate and tenor and on nothing else, so the
question is which of the four is genuinely available — and tenor is available here, bounded by the
offtake and concession rather than by appetite, while equity is the residual that is contributed when
the other levers fail (10.1.2). A is defensible and is the answer of last resort: it closes the gap by
funding it, and it should be priced against a lever that costs the sponsors nothing. C asks a credit
committee to abandon the margin its target exists to create, which is the request least likely to
succeed and the one that damages the negotiation elsewhere. D is the defensible weaker course on the
right principle: it does help both parties, and the uplift required is **128,526** a year — **2.0132 %**
of `CFADS` — which has to come from a revenue or cost commitment somebody will have to make good,
whereas the thirteenth year is a drafting change. The two cautions on B belong with it: the extra year
must sit comfortably inside the tail lenders require, and at 42,000,000 over 13 years the year-one
`DSCR` becomes **1.3456**, which is the number the committee will actually test.

### Self-check — KA 10.1

1. *Name the only four variables debt capacity depends on.* — `CFADS`, target coverage, rate,
   tenor. (Equity contribution is the residual, not a driver.)
2. *Why can two correct `DSCR` figures differ for one year?* — Different `CFADS` definitions; only
   the facility's governs.
3. *What does sculpting buy and what does it cost?* — More debt against uneven cash; complexity in
   documentation, modelling and re-cutting.
4. *State the effective rate at which sculpted debt discounts, and why it is below the loan rate.* —
   `r(1 − T/λ)`; each dollar of interest returns `T` of tax saving into the `CFADS` the coverage test
   divides. On Kestrel, 5.076923 % against a 6.0 % loan.
5. *When is a lower target `DSCR` not a concession?* — Whenever the stress attached to it exceeds
   `1 − λ₂/λ₁` — 3.8462 % for a move from 1.30× to 1.25×.
6. *Which of the three sizing answers is "the" debt capacity?* — None on its own: the term must say
   whether coverage is tested on the base case or in every period, and whether service is level or
   sculpted. The three answers on Kestrel are 41,171,123, 38,253,896 and 39,915,812.

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
   There is a wider version of the identity, and it is the most useful thing in this Knowledge Area.
   Sculpting sets `DS(t) = CFADS(t)/λ`, so the debt it supports is `PV(CFADS)/λ`, which rearranges to
   **`λ = PV(CFADS) ÷ debt` — the `LLCR`.** In words: **the constant `DSCR` that sculpting can deliver
   at a given debt level *is* the `LLCR` of that debt level**, and the level-cash case above is
   simply the special case where the sculpted profile happens to be an annuity. That gives the
   reviewer a fast reading of any coverage pack. Where `LLCR` exceeds the minimum `DSCR`, the gap is
   the coverage that a sculpted profile could recover; where the two are equal, the schedule is
   already optimally shaped and no re-cutting will help. On Kestrel's declining bank case the `LLCR`
   at 42,000,000 is Domain 6's **1.2395** against a minimum `DSCR` of **1.1851** — a gap of 0.0544
   of coverage lying idle in the early years. One qualification keeps this honest: the identity is
   exact only where `CFADS` is independent of the schedule. Kestrel's is not, because of the interest
   tax shield, so the constant coverage a sculpted 42,000,000 actually delivers is **1.2387** rather
   than 1.2395 — the eight-basis-point difference being the shield the smaller early balances no
   longer generate. A reviewer who quotes `LLCR` as the achievable sculpted coverage in a
   tax-paying structure is right to two decimal places and wrong in principle, and should say which
   number is which.

### 10.2.3 `ICR` and leverage

`ICR` = `EBIT` (or `EBITDA`) ÷ interest — an **accounting** coverage measure that ignores principal
entirely. Domain 2 computed Kestrel's `EBIT`-based interest cover at 2.02× and, on `EBITDA`,
**2.98×**. Leverage measures — debt/`EBITDA`, gearing — describe the balance sheet:
Kestrel's 42,000,000 debt against 18,000,000 equity is **2.33:1**, the 70/30 structure of Domain 1.

Why project lenders covenant on `DSCR` rather than `ICR`: `ICR` can look comfortable while principal
goes unpaid, and it is computed on accounting figures exposed to the classification judgments
Domain 2 (Case study B) showed can flip a covenant without any change in cash. `ICR` appears mainly
in corporate facilities and as a secondary project covenant.

**Worked example 10.2.3 — three shapes of the same loan: what repayment structure can move, and
what it cannot.**

1. **Setup.** The same 42,000,000 at 6.0 % over 12 years, against level `CFADS` of 6,384,000, in
   three repayment shapes. **A — fully amortising:** the annuity instalment of 5,009,635.23.
   **B — 25 % balloon:** 10,500,000 repaid at maturity, the balance amortised by level payments.
   **C — bullet:** interest only, with the whole 42,000,000 at maturity. `EBITDA` 7,500,000, `EBIT`
   5,100,000, year-one interest 2,520,000, `AF(0.06, 12) = 8.383844`, `1.06⁻¹² = 0.496969`.
   Compute each structure's four ratios and say what a credit committee should read from them.
2. **Formula.** For B, the level payment is `(debt − balloon × DF(12)) ÷ AF(r, 12)`; for C it is
   `debt × r`. `DSCR` = `CFADS` ÷ debt service; `LLCR` = `CFADS × AF(r, 12) ÷ debt`;
   `PLCR` = `CFADS × AF(r, 25) ÷ debt`; `ICR` = `EBITDA` ÷ interest.
3. **Substitution.** B: `(42,000,000 − 10,500,000 × 0.496969)/8.383844 = 4,387,226.43`. C:
   `42,000,000 × 0.06 = 2,520,000`. `LLCR` and `PLCR` in every case:
   `6,384,000 × 8.383844/42,000,000` and `6,384,000 × 12.783356/42,000,000`.
4. **Result.**

   | | A — amortising | B — 25 % balloon | C — bullet |
   |---|---|---|---|
   | Year-one debt service | 5,009,635.23 | **4,387,226.43** | **2,520,000.00** |
   | `DSCR` | 1.2743 | **1.4551** | **2.5333** |
   | `LLCR` | 1.2743 | 1.2743 | 1.2743 |
   | `PLCR` | 1.9431 | 1.9431 | 1.9431 |
   | `ICR` (on `EBITDA`) | 2.9762 | 2.9762 | 2.9762 |
   | `DSCR ÷ LLCR` | **1.0000** | **1.1419** | **1.9880** |
   | Payment due at maturity | 5,009,635.23 | 14,887,226.43 | 44,520,000.00 |

5. **Interpretation.** Read the rows, not the columns. **`DSCR` almost doubles across the three
   structures and `LLCR`, `PLCR` and `ICR` do not move at all** — and nothing whatever has changed
   about the project, the cash it generates or the amount owed. That is the single most important
   caution in this Knowledge Area: **`DSCR` is a property of the repayment schedule as much as of the
   project, so a `DSCR` quoted without its amortisation profile is not a credit statement.** `LLCR`
   is immune because it discounts all the cash to maturity against all the debt outstanding, and is
   therefore blind to *when* principal is scheduled; `ICR` is immune because it never looked at
   principal in the first place. The diagnostic that falls out of the table is the ratio in the
   penultimate row. **Where debt service is a level annuity against level cash, `DSCR ÷ LLCR` = 1
   exactly (10.2.2); any excess above 1 measures principal deferred beyond the periods being
   tested.** At 1.1419 it says a balloon of moderate size; at 1.9880 it says almost all of the
   principal sits at maturity. This is a reading, not a market metric, and it should be labelled as
   such in a paper — but it takes one division and it catches a structure whose comfortable coverage
   is manufactured rather than earned. Three consequences for practice. First, the **maturity row is
   the real risk statement**: structure C's `DSCR` of 2.5333 is arithmetically correct and
   professionally worthless beside a 44,520,000 payment the project must refinance, and structure B's
   apparently healthy 1.4551 conceals a year-twelve obligation of 14,887,226 — against which that
   year's `DSCR`, on the same 6,384,000 of cash, is **0.4288**. Second, **this is why lenders
   covenant on a ratio set rather than on `DSCR` alone**, and why 10.A.2's discipline on stating the
   refinancing assumption is not optional: a balloon converts a credit question into a market
   question, and the market is not a party to the facility. Third, the honest converse — **balloons
   are not a defect.** They match debt service to a genuine cash profile (a concession whose
   revenue ends, an asset whose residual value is real), they are cheaper than the equity they
   displace, and 10.1.3's 1,667,864 balloon would have closed Kestrel's sizing gap for a deferral of
   3.97 % of principal. The professional position is that a balloon must be *sized against a stated
   refinancing plan and stress-tested*, and that the `DSCR` it produces must never be reported
   without the maturity obligation beside it.

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

Where the capital structure has more than one debt tranche, the set has to be computed twice — on
senior debt service alone and on total debt service — because project-level subordinated or
mezzanine debt consumes the same `CFADS` the senior covenant measures. Domain 9 (KA 9.2.2 and
Fig 9.2.1) works that arithmetic on Kestrel and is the reference; this domain's ratios are the
senior-only case throughout.

**Worked example 10.2.4 — four covenants, one stress: which one binds first?**

1. **Setup.** Kestrel's facility as mandated at **42,000,000** carries four financial covenants:
   **`DSCR` ≥ 1.20×**, tested annually; **`LLCR` ≥ 1.15×**; **debt/`EBITDA`** ≤ **6.00×**; and
   **`ICR` on `EBITDA` ≥ 2.50×**. Apply a single common stress — a fall in revenue from the base
   12,000,000, with cash operating cost partly fixed as this domain's opening relation specifies —
   and find the fall at which each covenant is first crossed. Year-one interest 2,520,000; debt
   service 5,009,635.23; year-twelve `CFADS` 5,936,713 (Domain 6); the present value of `CFADS` over
   the loan life at the loan rate **52,060,092**; `AF(0.06, 12) = 8.383844`.
2. **Formula.** A revenue fall of `x` reduces `CFADS` by `0.75 × 12,000,000 × x = 9,000,000x` and
   `EBITDA` by `0.9375 × 12,000,000 × x = 11,250,000x`; interest is unchanged. Set each covenant to
   equality and solve for `x`: `(CFADS − 9,000,000x)/DS = 1.20`;
   `(PV − 9,000,000x × AF)/debt = 1.15`; `debt/(EBITDA − 11,250,000x) = 6.00`;
   `(EBITDA − 11,250,000x)/interest = 2.50`.
3. **Substitution.** `DSCR` (year one): `(6,384,000 − 9,000,000x) = 6,011,562.28`. `DSCR` (year
   twelve): `(5,936,712.85 − 9,000,000x) = 6,011,562.28`. Leverage:
   `7,500,000 − 11,250,000x = 42,000,000/6 = 7,000,000`. `LLCR`:
   `52,060,092.41 − 9,000,000x × 8.383844 = 1.15 × 42,000,000 = 48,300,000`. `ICR`:
   `7,500,000 − 11,250,000x = 2.50 × 2,520,000 = 6,300,000`.
4. **Result.**

   | Covenant | Threshold in its own units | Revenue fall that breaches it | Revenue at the trigger |
   |---|---|---|---|
   | `DSCR` ≥ 1.20× (year **twelve**) | `CFADS` 6,011,562 | **already breached, by 74,849** | 12,099,799 needed |
   | `DSCR` ≥ 1.20× (year **one**) | `CFADS` 6,011,562 | **4.1382 %** | 11,503,416 |
   | debt/`EBITDA` ≤ 6.00× | `EBITDA` 7,000,000 | **4.4444 %** | 11,466,667 |
   | `LLCR` ≥ 1.15× | `PV(CFADS)` 48,300,000 | **4.9833 %** | 11,402,010 |
   | `ICR` ≥ 2.50× | `EBITDA` 6,300,000 | **10.6667 %** | 10,720,000 |

5. **Interpretation.** Four results, in ascending order of usefulness. First, the operational one:
   **on the mandated facility the binding covenant is not merely the tightest, it is already
   breached** — Domain 6's year-twelve `DSCR` of 1.1851 is 74,849 of cash short of the 1.20×
   threshold, so revenue would have to run **0.8317 % above** base for the covenant to hold in every
   year. That figure is the arithmetic behind the 828,877 of resizing Case study A argues for on
   entirely different grounds, and it is why the facility did not close at 42,000,000. Second, the
   ranking: **the period test binds first and by a wide margin.** The year-one `DSCR` fails at a
   4.1382 % revenue fall while `ICR` survives to 10.6667 % — two and a half times as much — which is
   the quantitative version of 10.2.3's argument about why project lenders covenant on cash coverage.
   Third, and least expected, **the accounting leverage covenant binds second, at 4.4444 %**, only
   thirty basis points of revenue behind the `DSCR`. A treasury function monitoring `DSCR` alone
   would have three-tenths of a percentage point of warning before a second covenant fell, and
   leverage covenants are precisely the ones that move for reasons unconnected with cash — Domain 2
   (Case study B) showed a classification judgment flipping an accounting covenant with no change in
   economics at all. Fourth, the caution that stops this from being over-read: **the ordering is a
   property of the thresholds, not a law of finance.** Set the `ICR` covenant at 2.90× instead of
   2.50× and it binds first of all four, at a revenue fall of **1.7067 %**. Nothing about the project
   changed; the drafting did. So the professional output of this analysis is not "`DSCR` is the tight
   one" but a table like the one above, recomputed for the facility actually signed, with the smallest
   number in the third column circled — and the sanity check that a facility whose covenants all
   trigger within a few tenths of a percentage point of each other has no graduated warning at all,
   which defeats the purpose set out in 10.4.1. Note finally that the `LLCR` covenant here is 1.15×
   and so is the `DSCR` lock-up trigger of 10.4.2: the same number attached to two different tests on
   two different quantities. That coincidence is common in real facilities and a reviewer must never
   treat it as a link.

> **Fig 10.2.2 — Which covenant binds first.** Horizontal bar chart, one bar per covenant, x-axis
> revenue fall from base 0–12 %. Bars in brand blue: `DSCR` year one **4.1382 %**, debt/`EBITDA`
> **4.4444 %**, `LLCR` **4.9833 %**, `ICR` at 2.50× **10.6667 %**. The `DSCR` year-twelve test is
> drawn in crimson extending to the **left** of the axis to **−0.8317 %**, labelled "already
> breached — 74,849 short". A slate marker at **1.7067 %** on the `ICR` bar shows where the same
> covenant would bind if drafted at 2.90× rather than 2.50×, annotated "drafting, not economics".
> Each bar is labelled with its trigger in its own units (`CFADS` 6,011,562 · `EBITDA` 7,000,000 ·
> `PV(CFADS)` 48,300,000 · `EBITDA` 6,300,000). Source: PCI original. Alt text: horizontal bars
> showing that the period cash-coverage covenant is crossed by a much smaller revenue fall than the
> horizon or accounting covenants, with the worst-period test already breached before any stress.

### AI in this KA

Computing four ratios across a 25-year model is exactly what machines should do, and their outputs
are dangerously plausible because the arithmetic is simple and the *definitions* are not. The
invariants of 10.A.3 are the defence, and the `LLCR` = `DSCR` identity of 10.2.2 is the cheapest
single check available on any level-cash model. Two further checks are worth building into any
machine-produced coverage pack because they are one division each and they catch the errors a scanner
misses: **`DSCR ÷ LLCR`**, which flags coverage manufactured by deferral rather than earned (10.2.3),
and the **binding-covenant stress**, because a compliance dashboard that reports every covenant as
"pass" has told the reader nothing about which one is closest to failing (10.2.4). Where an AI
produces a covenant-compliance summary across a portfolio of facilities, the verification duty is to
test it against the documents on a sample — because the failure will not be arithmetic, it will be a
definition read from the wrong agreement.

### Key terms — KA 10.2

| Term | Meaning |
|---|---|
| **`DSCR`** | `CFADS` ÷ debt service; the period test lenders covenant on. |
| **`LLCR`** | PV of `CFADS` to loan maturity ÷ debt outstanding; the loan-horizon test. |
| **`PLCR`** | PV of `CFADS` to end of project life ÷ debt outstanding; counts the tail. |
| **Tail** | Project life beyond loan maturity; supports refinancing. |
| **`ICR`** | Earnings ÷ interest; ignores principal, exposed to accounting judgment. |
| **Headroom** | Cash that can be lost before a covenant threshold is crossed. |
| **`DSCR ÷ LLCR`** | A reading, not a market metric: 1.0000 for a level annuity against level cash; any excess measures principal deferred beyond the periods tested (1.1419 for a 25 % balloon, 1.9880 for a bullet). |
| **Binding covenant** | The covenant crossed by the smallest common stress; identified by solving each covenant for the same stress variable, not by comparing ratio levels. |

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

**MCQ 10.2-E `[10.2.3 · Analysis]`** A 42,000,000 facility is restructured from a full amortisation
to a 25 % balloon. Against level `CFADS` of 6,384,000 the `DSCR` rises from 1.2743 to 1.4551. The
`LLCR` will:
- A. rise in the same proportion
- B. be unchanged at 1.2743, because it discounts all the cash to maturity against all the debt outstanding and is blind to when principal is scheduled ✅
- C. fall, because more debt is outstanding for longer
- D. become undefined, since there is no level instalment

*Rationale:* `LLCR = 6,384,000 × 8.383844/42,000,000 = 1.2743` on any repayment profile (10.2.3).
A confuses the period test with the horizon test; C describes the interest cost, which `LLCR` does
not measure; D confuses `LLCR` with `DSCR`, which does need a periodic debt-service figure.

**MCQ 10.2-F `[10.2.4 · Evaluation]`** A facility carries `DSCR` ≥ 1.20× (breached by a 4.1382 %
revenue fall), debt/`EBITDA` ≤ 6.00× (4.4444 %), `LLCR` ≥ 1.15× (4.9833 %) and `ICR` ≥ 2.50×
(10.6667 %). The most useful conclusion for the treasury function is:
- A. the `ICR` covenant is redundant and should be removed
- B. `DSCR` binds first, so monitoring `DSCR` is sufficient
- C. `DSCR` binds first but debt/`EBITDA` follows only 0.31 percentage points of revenue behind it, so both must be monitored — and the ordering would reverse if `ICR` were drafted at 2.90× ✅
- D. the covenants are inconsistent and one of them must be wrong

*Rationale:* The gap between the first and second triggers is what determines whether monitoring one
covenant is enough, and the ordering is a drafting outcome — at 2.90× the `ICR` binds at 1.7067 %
(10.2.4). A treats a loose covenant as a useless one; B ignores the 0.31-point gap; D mistakes
different thresholds for an inconsistency.

**MCQ 10.2-G `[10.2.3 · Evaluation]`** A sponsor's credit paper reports a `DSCR` of 1.4551 on a 25 %
balloon structure, against 1.2743 fully amortising, as evidence of a stronger credit. The soundest
professional position is:
- A. the paper is right: `DSCR` is the covenanted ratio, and 1.4551 is more comfortable than 1.2743
- B. the coverage is deferred rather than earned — `DSCR ÷ LLCR` is 1.1419 and the year-twelve
  obligation is 14,887,226, against which that year's coverage on the same cash is 0.4288 — so the
  balloon is defensible only if it is sized against a stated refinancing plan, stress-tested, and
  reported with the maturity obligation beside the ratio ✅
- C. balloons should not be used, because they convert a credit question into a market question
- D. the two structures are equivalent, since `LLCR`, `PLCR` and `ICR` are identical in both

*Rationale:* Nothing about the project, the cash it generates or the amount owed has changed, so a
higher period ratio is information about the schedule and not about the credit (10.2.3). A reports an
arithmetically correct figure that misdescribes the risk. C is the opposite failure of judgment: a
balloon matched to a genuine cash profile is cheaper than the equity it displaces, and 10.1.3's
1,667,864 balloon would have closed Kestrel's sizing gap for a deferral of 3.97 % of principal. D
uses the horizon ratios' immunity to conclude that nothing differs, when the thing that differs —
14,887,226 falling due on one date — is the entire exposure.

**MCQ 10.2-H `[10.2.2 · Comprehension]`** `PLCR` exceeds `LLCR` whenever a project outlives its loan
because:
- A. `PLCR` discounts the same cash flow at a lower rate
- B. `PLCR` discounts `CFADS` to the end of the project's economic life while `LLCR` stops at loan
  maturity, so `PLCR` counts the tail — cash the lenders have no contractual claim on, which is why
  they rely on it least ✅
- C. `PLCR` is computed on `EBITDA` and `LLCR` on `CFADS`
- D. `PLCR` adds the asset's residual value to its numerator

*Rationale:* The two ratios differ only in the horizon of the numerator, and the extra cash is
exactly the cash beyond the lenders' claim (10.2.2). A invents a rate difference; both discount at
the loan rate. C confuses `PLCR` with `ICR`, which is the accounting measure. D adds a terminal value
that neither ratio contains.

### Self-check — KA 10.2

1. *When must `LLCR` equal `DSCR`?* — Level `CFADS` with annuity debt service discounted at the
   loan rate; divergence signals an inconsistency.
2. *Why do lenders rely least on `PLCR`?* — It counts cash beyond their maturity, on which they have
   no contractual claim.
3. *State Kestrel's headroom to covenant in cash terms.* — USD 372,438 per year, 5.8 % of `CFADS`.
4. *Which ratios does repayment structure move, and which does it not?* — It moves `DSCR` only;
   `LLCR`, `PLCR` and `ICR` are unchanged across full amortisation, balloon and bullet on the same
   debt and cash.
5. *What does the gap between `LLCR` and the minimum `DSCR` tell you?* — Approximately the coverage a
   sculpted profile could recover — 0.0544 of coverage on Kestrel's bank case — exactly where cash is
   independent of the schedule, and to within eight basis points where a tax shield is present.
6. *How is the binding covenant identified?* — By solving every covenant for the same stress variable
   and comparing the stresses, never by comparing ratio levels or headroom in the ratios' own units.

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

Maintenance-reserve sizing is worked in full elsewhere and is not repeated here: Domain 8 (Worked
example 8.1.3) prices Kestrel's lifecycle programme at a present value of **6,881,021**, derives the
economically equivalent level charge of **644,606** a year, and shows that without a reserve the
membrane-replacement year's `DSCR` collapses to **0.4561** — a payment default, not a covenant
breach. Three points from that arithmetic belong in a coverage discussion. The level charge is
**not** the deposit schedule: the money must be in the account before the first overhaul falls due,
which usually means either a higher early contribution or an opening balance funded at close, and
that difference is a real requirement in the sources and uses (Domain 14). Whether the charge sits
**above or below `CFADS`** is a definitional negotiation of exactly the kind 10.1.1 warns about, and
it decides whether the reserve protects the coverage ratio or merely the payment. And a reserve
converts a covenant event into a **distribution** event, which is the trade in one sentence: the
600,000 a year Kestrel actually charges is **77.5 %** of its base-case dividend (Domain 15,
KA 15.2.2).

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
   The same arithmetic gives the reserve negotiation a price, which is what turns "three months or
   six?" from a preference into a decision. Tolerance to a `CFADS` shortfall with `m` months funded is
   `1 − DS × (1 − m/12) ÷ CFADS`, so on Kestrel it runs **21.5283 %** with no reserve, **41.1462 %**
   at three months and **60.7641 %** at six. Because the expression is linear in `m`, **each
   additional month buys exactly 6.5393 percentage points of cash tolerance and costs 417,470 of
   funded cash** — that is **63,840 of cash per percentage point of tolerance**, and it is the figure
   to put beside a lender's request for a twelfth month. Two cautions on over-applying it. The
   tolerance is a **single-year** measure: a reserve drawn in one year must be replenished before
   distributions resume (10.3.3), so a two-year shortfall of half the size is a far worse event than
   a one-year shortfall of the whole, and the linear rule says nothing about it. And the tolerance is
   only meaningful against the **project's own downside case**: six months is the market convention
   and twelve is common through a ramp-up, but the month count worth paying for is the one whose
   tolerance covers the stress the sponsor and the lender have already agreed is plausible
   (Domain 11's allocation work, Domain 7's demand cases).

**Worked example 10.3.2b — funding the reserve: equity, debt or a letter of credit.**

1. **Setup.** Kestrel must have **2,504,818** in its DSRA at first drawdown. Three routes are on
   offer. **Equity:** the sponsors fund it in cash at close; the account earns a deposit rate of
   **3.00 %** (Domain 15, KA 15.4) against a cost of equity of **15.42 %** (Domain 9, KA 9.1.3).
   **Debt:** the facility is increased by the same amount, at 6.0 % with tax relief at 20 %.
   **Letter of credit:** an acceptable bank issues an LC in favour of the security trustee for an
   annual fee, and no cash is funded at all. Price each route and identify the breakeven LC fee.
2. **Formula.** Equity carry = reserve × (cost of equity − deposit rate). After-tax debt cost =
   reserve × `r` × (1 − `T`). LC cost = reserve × fee rate. Breakeven fee = the rate at which the LC
   cost equals the route it displaces. For the debt route, the coverage consequence is that the
   reserve consumes capacity: capex funding available = debt capacity − reserve.
3. **Substitution.** Equity: `2,504,817.62 × (0.1542 − 0.0300)`. Debt:
   `2,504,817.62 × 0.06 × 0.80`. LC at a market rate of 1.25 %: `2,504,817.62 × 0.0125`. Capacity:
   `41,171,123 − 2,504,818`.
4. **Result.** **Equity carry USD 311,098 a year.** **After-tax debt cost USD 120,231 a year** — but
   the 2,504,818 comes out of the 41,171,123 the project can borrow at 1.30×, leaving only
   **38,666,305** for capex and widening the equity requirement against the funding plan's
   42,000,000 of senior debt from 828,877 to **USD 3,333,695**. **LC at 1.25 %: USD 31,310 a year**,
   saving **279,788** a year
   against the equity route — **USD 2,108,505** of present value at 8 % over the loan life. The
   **breakeven LC fee is 12.42 %** a year.
5. **Interpretation.** The breakeven fee is the result that decides the question, and its size is the
   point: **letter-of-credit pricing is not remotely close to 12.42 %**, so wherever the
   facility permits it, an LC-backed reserve is the sponsor's answer and the negotiation is about the
   LC's terms rather than its price. The debt route looks cheapest per dollar and is the trap:
   **it does not avoid the equity requirement, it relocates and enlarges it**, because a reserve
   funded from a coverage-constrained facility displaces capex debt one for one — 2,504,818 of reserve
   costs 2,504,818 of capex borrowing and therefore 2,504,818 of additional equity, on top of the
   828,877 that Case study A already had to find. That is a general result for any structure sized
   on coverage rather than on cost: **inside a binding coverage constraint every use of debt competes
   with every other use at par**, which is why sources and uses must be agreed before reserve levels
   are (Domain 14, KA 14.1). Three qualifications keep the LC answer honest. An LC **substitutes bank
   credit risk for cash**, so the lenders will require an issuer of specified standing and a
   replacement obligation if it is downgraded — the sponsor has therefore bought a cheaper reserve and
   sold a contingent obligation to refinance it at the worst possible moment. An LC **consumes the
   sponsor's own credit lines**, an opportunity cost that appears nowhere in the fee and can be larger
   than it for a sponsor with limited headroom. And whether the LC is a demand instrument or a
   documentary one, when it may be drawn, how long it must run and what happens on expiry are
   **enforceability-sensitive drafting matters that differ by jurisdiction and by issuing bank** —
   a point for qualified counsel, not for a model. The reviewer's check across all three routes is the
   same: the reserve must appear in the **uses** of funds as well as in the operating cash flow, which
   is exactly the class-one model-audit finding Domain 13 (Worked example 13.2.3, finding F-03) prices
   at 2,504,818 of unfunded requirement.

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
unavailable. The magnitude is easy to underestimate. Kestrel's base-case cash available for
distribution is 774,364.77 a year (Domain 15, KA 15.2.3), so a DSRA drawn by **2,009,635** — the
gap in the bad year of MCQ 10.3-B — takes **2.5952 years** of the entire dividend to replace. One
year of shortfall therefore buys roughly two and a half years of distribution drought, before any
covenant or lock-up test has been applied at all; Domain 15 (KA 15.2.4) prices what a drought of
that length does to an equity return.

**Worked example 10.3.3 — a cash sweep, and the average life the lender is really buying.**

1. **Setup.** Kestrel's facility as signed: 42,000,000 at 6.0 %, level instalment 5,009,635.23 over
   12 years, `CFADS` 6,384,000, maintenance-reserve charge 600,000, so cash available for
   distribution is **774,364.77** a year (Domain 15, KA 15.2.3). The lenders require a **50 % cash
   sweep**: half of the cash that would otherwise be distributed is applied to mandatory prepayment,
   in addition to the scheduled instalment. Compute when the loan retires, the reduction in the
   loan's **weighted average life**, the interest saved, and what the sweep costs equity in timing.
2. **Formula.** Sweep per period = 50 % × distributable cash. Each period the balance rolls forward as
   `B(t) = B(t−1) × (1 + r) − instalment − sweep`, with the final payment truncated to the balance
   outstanding. Weighted average life = `Σ t × principal(t) ÷ Σ principal(t)`, in years. Interest =
   total paid − principal.
3. **Substitution.** Sweep `774,364.77/2 = 387,182.39`; total annual application
   `5,009,635.23 + 387,182.39 = 5,396,817.62`. Roll the balance forward from 42,000,000 until it is
   extinguished; compute the two average lives from the two principal profiles.
4. **Result.** The loan **retires in year 11**, with a final payment of **4,326,132.35** in place of
   the scheduled 5,009,635.23. **Weighted average life falls from 7.1887 years to 6.4660 years**, a
   reduction of **0.7227 years**. Total interest falls from **18,115,622.76** to **16,294,308.50** —
   a saving of **USD 1,821,314**. Equity pays **387,182.39** a year for ten years, **3,871,824** in
   total, and gets back **683,502.88** more than base in year eleven plus the whole of year twelve's
   avoided debt service of **5,009,635.23**; discounted at Domain 4's 8 % the timing costs equity
   **USD 315,488** of present value.
5. **Interpretation.** The lender's benefit is best stated as the **average life**, not the maturity
   date, and this is why: the facility's stated tenor is unchanged at twelve years, the *maturity* is
   unchanged until the year the loan happens to extinguish, but the weighted average time the lenders'
   money is at risk falls by **0.7227 years — 10.05 % of the exposure period.** Average life is the
   measure a credit committee prices margin against and a treasury desk uses to set the swap notional
   profile, and a sweep is best described to both as an average-life reduction rather than as
   "prepayment protection". The **1,821,314 of interest saved is a benefit to equity, not to the
   lenders** — they lose it — which explains why sweeps are conceded in exchange for something else
   (a covenant reset, a longer tenor, a lower margin) rather than granted, and why Domain 15
   (KA 15.3.4) treats a sweep as part of the price of consent and computes its breakeven share at
   40.3334 % in that context. Note carefully what the equity arithmetic does and does not say. The
   present-value cost of **315,488** is modest against a 3,871,824 nominal diversion, because prepaying
   6 % debt while discounting equity cash at 8 % is close to value-neutral in isolation; the real cost
   to a sponsor is not present value but **leverage and flexibility** — a swept structure de-gears
   faster, so the equity return falls even where present value barely moves, and cash committed to a
   sweep is cash unavailable for anything else in the year it arises. Three cautions. The result above
   assumes distributable cash is **level**; on Kestrel's actual declining profile the sweep shrinks
   year by year with the tax shield, so a model that applies a constant sweep has overstated the
   prepayment in every later period. A sweep interacts with the **coverage tests**, because the
   scheduled instalment is unchanged while the balance falls faster, so the `DSCR` improves through
   the loan life and the facility becomes progressively less likely to breach — a real second-order
   benefit that sponsors rarely claim in negotiation. And the drafting matters more than the
   percentage: whether the sweep is taken **before or after** reserve top-ups, whether it is computed
   on distributable cash or on cash above a coverage threshold, and whether it is applied in order of
   maturity or pro rata, will change the answer above by more than moving the share from 50 % to 60 %
   would.

### AI in this KA

Reserve and prepayment mechanics are arithmetically simple and structurally fiddly, which is exactly
the profile on which a machine is useful and a machine's answer is dangerous. **Where it earns its
place:** rebuilding a sweep or a sculpted profile across many periods and many scenarios, so that
the average-life and retirement-year consequences of a proposed sweep share can be tabulated in
minutes rather than days; reconciling reserve required-balance schedules against the facility's own
tables period by period; and flagging periods in which a reserve top-up, a sweep and a distribution
test compete for the same cash — the class of interaction that hand-built waterfalls get wrong.
**Where it must not go:** it must not decide the **order** of the waterfall, because that order is a
contractual reading and every mechanical answer downstream depends on it; it must not choose the
reserve's funding route, which turns on a credit judgment about an LC issuer and on tax advice that
is jurisdiction-specific (10.3.2b); and it must not assume the market convention where the document
is silent — a silent document is a question for counsel, not a default. **Verification, concretely:**
recompute one period of the waterfall by hand against the clause, including the top-up and the
distribution test, and confirm that the reserve appears in the **uses** of funds as well as in the
operating cash flow. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 10.3

| Term | Meaning |
|---|---|
| **DSRA** | Debt service reserve; buys payment continuity and time, not compliance. |
| **MRA** | Maintenance reserve; smooths lumpy major maintenance (sized in Domain 8, KA 8.1.3). |
| **Reserve tolerance** | `1 − DS × (1 − m/12) ÷ CFADS`; the single-year `CFADS` shortfall a reserve of `m` months survives — 6.5393 points per month on Kestrel, at 417,470 of funded cash each. |
| **LC-backed reserve** | A reserve satisfied by a letter of credit rather than cash; cheapest for the sponsor, and a substitution of bank credit risk for cash. |
| **Weighted average life** | `Σ t × principal(t) ÷ Σ principal(t)`; the measure a sweep actually shortens (7.1887 → 6.4660 years on Kestrel). |
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

**MCQ 10.3-D `[10.3.2 · Evaluation]`** A sponsor must satisfy a 2,504,818 DSRA. Cash funding costs
the spread between a 15.42 % cost of equity and a 3.00 % deposit rate; an LC is available at 1.25 %.
The strongest professional conclusion is:
- A. fund it from the senior facility, since after-tax debt at 4.80 % is the cheapest source
- B. use the LC: the breakeven fee is 12.42 % a year, so 1.25 % saves 279,788 a year, and the residual questions are the issuer's standing and the LC's drafting ✅
- C. fund it in cash, because lenders will not accept an LC
- D. the routes are economically equivalent because the reserve is returned at maturity

*Rationale:* Breakeven fee `= 0.1542 − 0.0300 = 12.42 %`; at 1.25 % the LC costs 31,310 against an
equity carry of 311,098 (10.3.2b). A is the trap: inside a binding coverage constraint the debt
route displaces capex borrowing one for one and enlarges the equity requirement to 3,333,695. C
asserts a market position that is simply untrue where the facility permits an LC. D confuses the
return of principal with the cost of carrying it for twelve years.

**MCQ 10.3-E `[10.3.3 · Application]`** A 42,000,000 loan at 6.0 % with a 5,009,635.23 level
instalment over 12 years is subjected to a 50 % sweep of 774,364.77 of annual distributable cash.
The metric that best states what the lenders have gained is:
- A. the tenor, which is unchanged at 12 years
- B. the reduction in weighted average life from 7.1887 to 6.4660 years — 0.7227 years, or 10.05 % of the exposure period ✅
- C. the 1,821,314 of interest saved
- D. the retirement in year 11 rather than year 12

*Rationale:* Average life is the measure of exposure that margin is priced against and swap notionals
are set from (10.3.3). A is true and uninformative; C is a benefit to *equity* — the lenders forgo
that interest; D is a single date rather than the exposure profile, and it moves with the cash case.

**MCQ 10.3-F `[10.3.3 · Evaluation]`** Lenders ask for a 50 % cash sweep. The sponsor's treasury
recommends conceding it, on the ground that the present-value cost to equity is only 315,488 at 8 %
against a 3,871,824 nominal diversion. The soundest position is:
- A. concede it: 315,488 is immaterial beside the 1,821,314 of interest the sweep saves the lenders
- B. concede it only in exchange for something, and only on drafting settled first: the present-value
  figure understates the cost because a swept structure de-gears faster, so the equity return falls
  even where present value barely moves ✅
- C. refuse any sweep, since diverting distributable cash always destroys equity value
- D. concede it and present the 0.7227-year reduction in weighted average life as a benefit shared
  with the lenders

*Rationale:* Prepaying 6 % debt while discounting equity cash at 8 % is close to value-neutral in
present value, which is precisely why present value is the wrong test here; the real costs are
leverage, flexibility and cash unavailable in the year it arises — and whether the sweep is taken
before or after reserve top-ups will move the answer by more than raising the share from 50 % to 60 %
would (10.3.3). A misattributes the interest saving, which accrues to equity: the lenders forgo it. C
denies a trade routinely worth making against a covenant reset, a longer tenor or a lower margin. D
gives away the consideration the concession should have bought — the shorter average life is the
lenders' benefit, not a mutual one.

**MCQ 10.3-G `[10.3.2 · Comprehension]`** A newly appointed director is told that a debt service
reserve "protects the covenant". Restated correctly, what the reserve does is:
- A. buy time — it keeps scheduled debt service being paid through a short cash shortfall, converting
  a liquidity failure into a negotiation, while the coverage ratio, computed on `CFADS`, fails
  regardless ✅
- B. raise `CFADS` in the year it is drawn, because the cash reaches the lenders
- C. reduce the required coverage ratio in proportion to the months funded
- D. stand in place of a covenant, which is why facilities carrying a reserve covenant looser ratios

*Rationale:* A reserve is a liquidity instrument and a covenant is a cash-flow test, so a fully funded
six-month reserve preserves payment and does not prevent breach — Kestrel can absorb a collapse of
`CFADS` to 2,504,818, some 39 % of base case, and still pay in full while the ratio is far below 1.20
(10.3.2). B is the error the definition of `CFADS` excludes: it counts cash the project found rather
than cash it generated. C and D assert relationships between reserves and ratios that no facility
creates — the month count is negotiated for how long lenders want before they must act.

**MCQ 10.3-H `[10.3.2 · Evaluation]`** In final negotiation the lenders ask for a **twelfth** month of
debt service reserve in place of six. Each month buys **6.5393 percentage points** of single-year cash
tolerance and costs **417,470** of funded cash — **63,840** per percentage point — and the facility
permits an LC-backed reserve at a market fee of 1.25 % against a breakeven of 12.42 %. The soundest
response is:
- A. refuse: six months is the market convention and twelve is excessive for an operating asset
- B. accept the twelve months and satisfy them with a letter of credit: the quantity question is worth
  arguing only if cash must be funded, and at 1.25 % against a 12.42 % breakeven the incremental
  2,504,818 of cover costs 31,310 a year rather than 311,098 ✅
- C. accept the twelve months and fund them from the senior facility, since after-tax debt at 4.80 %
  is the cheapest source
- D. offer nine months as a midpoint, which buys 80.3821 % of tolerance

*Rationale:* Twelve months takes single-year tolerance from **60.7641 %** to **100 %** — 39.2359 points
for a further 2,504,818 — and the instrument, not the month count, is what determines whether that is
expensive (10.3.2, 10.3.2b). A defends a convention against a request whose price the sponsor can make
trivial. C is the trap the worked example exists to expose: inside a binding coverage constraint every
use of debt competes with every other at par, so the reserve displaces capex borrowing one for one and
enlarges the equity requirement. D is the defensible weaker course — a genuine midpoint, correctly
computed, which spends negotiating capital on a quantity that has stopped mattering once the
instrument question is settled, and buys back a month the lenders may value more than the sponsor
gives up.

### Self-check — KA 10.3

1. *What does a DSRA actually buy?* — Payment continuity and time to negotiate; not covenant
   compliance.
2. *How should a reserve be expressed to a board?* — As the shortfall it survives, not as a number
   of months.
3. *Why must distribution forecasts model reserve replenishment?* — Top-ups rank above equity, so
   unreplenished reserves make forecast distributions contractually unavailable; on Kestrel a
   2,009,635 drawdown takes 2.5952 years of the whole dividend to replace.
4. *What does each extra month of DSRA buy, and at what price?* — 6.5393 percentage points of
   single-year cash tolerance, for 417,470 of funded cash: 63,840 per percentage point.
5. *Why is funding a reserve from the senior facility rarely the cheap option it looks?* — Inside a
   binding coverage constraint it displaces capex debt one for one, so it relocates and enlarges the
   equity requirement rather than avoiding it.
6. *State what a cash sweep gives the lender and what it takes from equity.* — A shorter weighted
   average life (0.7227 years on Kestrel); cash diverted in the years it arises, and faster
   de-gearing — the present-value cost, 315,488 at 8 %, understates the return effect.

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

**Worked example 10.4.1 — historic against forward-looking, on the same facility.**

1. **Setup.** Kestrel's facility tests `DSCR` ≥ **1.20×** annually on two bases: a **historic** test
   on the twelve months just ended and a **forward-looking** test on the twelve months about to
   begin, both computed on the same documented `CFADS` definition and the same debt service of
   5,009,635.23. On the bank case `CFADS` declines from 6,384,000 to 5,936,713 over the loan
   (Domain 6), so the year-by-year figures around the end of the loan are: year 10 **6,040,690**,
   year 11 **5,990,216**, year 12 **5,936,713**. Which test breaches first, and by how much time?
2. **Formula.** At the test date ending year `t`: historic `DSCR` = `CFADS(t)/DS`; forward `DSCR` =
   `CFADS(t+1)/DS`. Threshold in cash = `1.20 × 5,009,635.23 = 6,011,562`.
3. **Substitution.** End of year 10: historic `6,040,690/5,009,635.23`, forward
   `5,990,216/5,009,635.23`. End of year 11: historic `5,990,216/5,009,635.23`, forward
   `5,936,713/5,009,635.23`.
4. **Result.**

   | Test date | Historic `DSCR` | Historic result | Forward `DSCR` | Forward result |
   |---|---|---|---|---|
   | End of year 9 | 1.2153 | pass | 1.2058 | pass |
   | **End of year 10** | **1.2058** | **pass** | **1.1957** | **breach** |
   | End of year 11 | 1.1957 | breach | 1.1851 | breach |
   | End of year 12 | 1.1851 | breach | — | — |

   The forward test breaches **one full test date earlier** than the historic test.

5. **Interpretation.** The result generalises into a rule worth carrying: **on a declining cash
   profile the forward-looking test binds; on a rising profile the historic test binds; and which of
   them is "the tighter covenant" is therefore a property of the project's cash shape, not of the
   drafting.** Kestrel's profile declines because its interest tax shield amortises, so its forward
   test is the operative covenant and its historic test is decorative — while on Domain 6's sponsor
   case, where coverage rises from 1.2743 to 1.5940, the reverse is true and the historic test is the
   only one that could ever fail. A sponsor who negotiated hard on the forward test's assumptions
   without noticing which way its own cash slopes has negotiated the wrong clause. Three practical
   consequences. **The year the forward test buys is the whole point of it**: at the end of year ten
   the project is compliant on everything that has happened and in breach on what is about to happen,
   which is exactly the moment at which a cure, a waiver or an operational fix is still cheap — the
   design intent of this Knowledge Area, made arithmetic. **A facility with both tests effectively has
   the earlier of the two as its covenant**, so compliance certificates must report both, and a
   certificate that reports only the historic figure states compliance in the very period the
   facility fails. And **a forward test is a test on a forecast**, which raises the questions 10.A.1
   sets out — whose model, on what assumptions, reviewed by whom — with real consequence here: the
   difference between breaching at the end of year ten and the end of year eleven is decided by a
   number nobody can observe yet. That is why forward tests are normally accompanied by a defined
   basis of preparation and, often, by the lenders' technical adviser's assumptions prevailing on
   specified inputs.

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

**Three thresholds, not two.** Kestrel's graduated set is the standard shape and was stated in
cash at the head of this domain (1.25× · 1.20× · 1.15×, or 6,262,044 · 6,011,562 · 5,761,081 of annual
`CFADS`). The distinction sponsors miss is between the first and the third. Failing the distribution
condition means *this period's* cash waits, and is usually released when the test is next passed.
Falling through the lock-up trigger means the cash stops being the sponsor's at all.

**Worked example 10.4.2 — where the lock-up should sit, and which threshold actually bites.**

1. **Setup.** Kestrel's mandated 42,000,000 facility, tested against Domain 6's bank case in which
   `CFADS` declines from 6,384,000 to 5,936,713 over the loan's twelve years. Debt service
   5,009,635.23; maintenance-reserve charge 600,000, so cash available for distribution in year `t`
   is `CFADS(t) − 5,009,635.23 − 600,000`. Test three candidate thresholds — the 1.25 ×
   distribution condition as drafted, the 1.20 × covenant, and the 1.15 × lock-up trigger — and
   report which years fail each, how much cash is affected and what it is worth.
2. **Formula.** Threshold in cash = threshold × debt service. A year is caught when `CFADS(t)` falls
   below it. Cash affected = Σ distributable cash in the caught years; its value to equity = the same
   sum discounted at Domain 4's **8 %** appraisal rate.
3. **Substitution.** Cash thresholds `5,009,635.23 × 1.25 / 1.20 / 1.15`. Compare each year's
   `CFADS(t)` against them; sum and discount the distributable cash of the caught years.
4. **Result.**

   | Threshold | Cash trigger | Years caught | Cash affected | Present value at 8 % |
   |---|---|---|---|---|
   | Distribution condition **1.25×** | 6,262,044 | **years 5–12 (eight of twelve)** | **USD 3,956,574** | **USD 2,165,274** |
   | Financial covenant **1.20×** | 6,011,562 | years 11–12 | 707,658 | — |
   | Lock-up trigger **1.15×** | 5,761,081 | **none** | nil | nil |

   The eight caught years are **57.61 %** of the 6,867,502 of distributable cash the loan period
   generates. The minimum `CFADS` over the loan is 5,936,713, which never reaches the 1.15 × trigger.

5. **Interpretation.** The headline is uncomfortable and entirely typical: **on its own bank case the
   threshold that traps most of Kestrel's dividend is the one nobody negotiated.** The 1.15 × lock-up
   trigger — argued over, documented, cited in every summary of the structure — never engages at all,
   because the project's coverage never falls that far. The 1.20 × covenant catches two years. The
   1.25 × distribution condition catches **eight**, and 3,956,574 of cash with a present value of
   2,165,274 against total equity of 18,000,000. A sponsor who modelled the covenant, or even the
   covenant and the lock-up, and reported a twelve-year dividend stream to its board has overstated
   distributable equity cash by more than half. Three professional consequences. First, **the
   distribution condition is the operative constraint in most healthy projects and the lock-up
   trigger is the operative constraint in sick ones** — they are not alternatives and they are not
   redundant, and a negotiation that spends its capital on the second while conceding the first has
   optimised for a state of the world the project is unlikely to reach. Second, **the graduated
   design is doing exactly what 10.4.1 says it should**: the thresholds engage in order, cash is
   retained long before breach, and by the time the covenant is crossed in year eleven the structure
   has already accumulated retained cash from seven earlier years. That is the mechanism working, not
   failing — the failure is in the sponsor's forecast, not in the drafting. Third, the caution about
   generalising this result: it is computed on a *flat-revenue* case whose coverage declines only
   because the interest tax shield amortises, which is a slow and entirely predictable slope. Put a
   revenue stress on top and the caught years extend earlier; put Domain 6's escalating sponsor case
   underneath and coverage rises to 1.5940 and **no** threshold is ever touched. So the output that
   belongs in a board paper is not a number but a **table of caught years per case**, and the
   discipline is to run the distribution profile through the tests on the lender's case, not the
   sponsor's. Domain 15 (KA 15.2.2 and 15.2.4) then prices what those trapped years do to an equity
   return, and shows the block account funding a later reserve shortfall out of the very cash the
   sponsor resented losing.

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

**Worked example 10.4.3 — the same breach, two drafting treatments, two cheques.**

1. **Setup.** Kestrel's mandated 42,000,000 facility breaches its 1.20 × covenant in years eleven and
   twelve of the bank case: `CFADS` of **5,990,216** and **5,936,713** against a threshold of
   **6,011,562** (Domain 6 quantified the two shortfalls at 21,347 and 74,849). The sponsors hold
   unused cure rights. The facility could treat cure cash in either of the two standard ways:
   **(a)** the injected amount is **deemed to be `CFADS`** for the tested period, or **(b)** it is
   **applied to prepayment** and the tested debt service is reduced by the amount prepaid. Debt
   service 5,009,635.23; closing balance after year eleven 4,726,071; prepayments applied in reverse
   order of maturity as the facility provides. Size the cure under each treatment.
2. **Formula.** Treatment (a): `C = λ × DS − CFADS`. Treatment (b): `P = DS − CFADS/λ`. Since
   `P = (λ·DS − CFADS)/λ`, the two are related exactly by **`P = C ÷ λ`** — the prepayment cure is
   the `CFADS` cure divided by the covenant level, because it works on the denominator, which the
   ratio itself levers. Under (b) the prepayment also reduces the following period's balance,
   interest and final payment, so the year-twelve requirement must be recomputed.
3. **Substitution.** Year 11, (a): `1.20 × 5,009,635.23 − 5,990,215.54`. Year 11, (b):
   `5,009,635.23 − 5,990,215.54/1.20`. Then the year-twelve knock-on: balance
   `4,726,071 − 17,789 = 4,708,282`; new interest `× 0.06`; new final payment `× 1.06`; new
   `CFADS(12) = 5,880,000 + 0.20 ×` new interest; then `P₁₂ = DS₁₂ − CFADS(12)/1.20`.
4. **Result** (money to whole units).

   | | Treatment (a) — deemed `CFADS` | Treatment (b) — applied to prepayment |
   |---|---|---|
   | Year 11 cure | **21,347** | **17,789** |
   | Year 12 debt service after the year-11 cure | 5,009,635 | **4,990,779** |
   | Year 12 `DSCR` before curing | 1.1851 | **1.1895** |
   | Year 12 cure | **74,849** | **43,696** |
   | **Total cash injected** | **USD 96,196** | **USD 61,485** |

   Treatment (b) costs **34,711 less** — **36.08 %** less — for exactly the same compliance
   outcome.
5. **Interpretation.** The identity is the transferable result: **a prepayment cure is the deemed-
   `CFADS` cure divided by the covenant level, so treatment (b) always costs `1 − 1/λ` less in the
   period cured** — 16.6667 % at a 1.20 × covenant, 23.0769 % at 1.30 ×, so the drafting is worth more
   the tighter the covenant. The rest of the saving here, and the larger part of it, comes from the
   second-order effect the table makes visible: **prepayment permanently reduces the debt, so it cures
   the current period and improves every later one.** The year-eleven cure lifts the year-twelve
   `DSCR` from 1.1851 to 1.1895 unaided, cutting the second cure from 74,849 to 43,696. On a declining
   coverage profile that compounding is worth having; on a single isolated breach it is worth nothing,
   which is why the identity, not the total, is the thing to remember. Four cautions belong with this
   arithmetic. **Lenders do not concede treatment (b)
   lightly**, because deemed `CFADS` leaves their exposure unchanged while prepayment reduces it —
   which is exactly why they prefer it, and why the negotiation is often about *both* limbs (cash in,
   and where it goes). **A cure computed to the last cent breaches again on any further slippage**:
   treatment (a) restores the year-twelve ratio to precisely 1.2000, so a single dollar of adverse
   variance re-breaches, which is why Case study B's sponsors deliberately injected 3,700,000 against
   a bare minimum of 3,550,000. **Cures are limited and consumed**: a maximum number over the loan
   life, a maximum in consecutive periods, and rules on whether the cash counts once or permanently,
   so the cheapest cure is not always the right one — Domain 6 records the auditor rejecting an
   otherwise trivial 96,196 of curing on the ground that *a structure designed to need a cure has
   consumed an option it should be holding in reserve*. And **whether an injection is equity,
   subordinated debt or a shareholder loan changes its tax, accounting and insolvency treatment**,
   which is jurisdiction-specific and a matter for counsel and tax advice rather than for the model.

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
- **Model every threshold, not just the covenant — and expect the distribution condition to be the
  one that bites.** Sponsors are hurt by trapped cash long before default, and boards are routinely
  surprised by a distribution profile nobody tested; on Kestrel's own bank case the threshold that
  got the negotiating attention never engages at all (10.4.2).
- **Know which test basis binds, historic or forward.** It is decided by the slope of the project's
  cash, not by the drafting: a declining profile breaches the forward test a full test date before
  the historic one (10.4.1), which is the year in which a cure or an operational fix is still cheap.
  Report both figures on every compliance certificate.

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
| **Historic vs forward-looking test** | Measured on what happened vs on projection; which binds follows the slope of the cash profile, not the drafting. |
| **Distribution condition** | The coverage level above which cash may leave the structure at all (1.25× on Kestrel); usually the operative constraint in a healthy project. |
| **Distribution lock-up** | Trapping equity cash when a test fails, short of default; on Kestrel the 1.15× trigger, operative only in a sick project. |
| **Event of default** | Defined breach entitling lenders to remedies including acceleration. |
| **Equity cure** | Sponsor cash injected to restore a ratio; limited by the facility. |
| **Cure identity `P = C ÷ λ`** | A prepayment cure costs `1 − 1/λ` less than a deemed-`CFADS` cure of the same breach — 16.6667 % at a 1.20× covenant — and also reduces later periods' debt service. |
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

**MCQ 10.4-D `[10.4.3 · Application]`** A `DSCR` covenant of 1.20× is breached with `CFADS` of
5,990,216 against debt service of 5,009,635.23. Under the two standard treatments the cure required
is:
- A. 21,346.73 whether the cash is deemed `CFADS` or applied to prepayment
- B. 21,346.73 if deemed `CFADS`; 17,788.94 if applied to prepayment, since the prepayment reduces the denominator and `P = C ÷ λ` ✅
- C. 21,346.73 if deemed `CFADS`; 25,616.08 if applied to prepayment
- D. the breach cannot be cured with cash, only waived

*Rationale:* `C = 1.20 × 5,009,635.23 − 5,990,215.54 = 21,346.73`, and
`P = 5,009,635.23 − 5,990,215.54/1.20 = 17,788.94 = C/1.20` (10.4.3). A ignores that the two
treatments act on different sides of the ratio; C multiplies by the covenant instead of dividing —
a nameable sign error that makes the prepayment route look dearer; D confuses a cure right with a
waiver.

**MCQ 10.4-E `[10.4.2 · Evaluation]`** A facility has a 1.25× distribution condition, a 1.20×
covenant and a 1.15× lock-up trigger. On the lenders' bank case the project's `DSCR` runs from
1.2743 down to 1.1851. The sponsor's negotiating priority should be:
- A. the 1.15× lock-up trigger, since it has the most severe consequence
- B. the 1.25× distribution condition, which catches eight of the twelve years and 3,956,574 of dividend, while the 1.15× trigger is never reached on this case ✅
- C. the 1.20× covenant, since breach is the event that matters
- D. all three equally, since they are tested on the same ratio

*Rationale:* The threshold that binds on the case being tested is the one worth negotiating capital
on; here the lock-up trigger never engages (10.4.2). A optimises for a state the project does not
reach on this case; C is the two-year problem rather than the eight-year one; D ignores that the same
ratio crosses three different levels at three different times.

**MCQ 10.4-F `[10.4.1 · Evaluation]`** A facility tests `DSCR` on both a historic and a
forward-looking basis. At the end of year ten the historic test passes at 1.2058 and the forward test
fails at 1.1957. The compliance certificate reports the historic figure only. The soundest
professional position is:
- A. the certificate is adequate: the historic figure is the only one that can be observed
- B. a facility with both tests effectively has the earlier of the two as its covenant, so a
  certificate reporting only the historic figure certifies compliance in the very period the facility
  fails; both figures must be reported, on an agreed basis of preparation for the forward test ✅
- C. only the forward figure should be reported, since the forward test is the tighter covenant on any
  project
- D. the forward test should be resisted altogether, because it tests a forecast nobody can observe

*Rationale:* On a declining coverage profile the forward test breaches a full test date earlier, and
that test date is exactly when a cure, a waiver or an operational fix is still cheap — the design
intent of the covenant set, made arithmetic (10.4.1). A treats observability as the criterion for
disclosure. C overgeneralises: which basis binds follows the slope of the project's cash, and on
Domain 6's rising sponsor case the historic test is the only one that could ever fail. D discards the
year of warning the forward test buys; the answer to a test on a forecast is a defined basis of
preparation with stated prevailing assumptions, not removal of the test.

**MCQ 10.4-G `[10.4.3 · Evaluation]`** Kestrel's mandated 42,000,000 facility breaches its 1.20×
covenant in years eleven and twelve of the bank case. Curing both breaches costs **96,196** if the cash
is deemed to be `CFADS` and **61,485** if it is applied to prepayment. Counsel proposes spending the
remaining negotiating capital on securing the prepayment treatment. The better judgement is:
- A. agree: 34,711 is a 36.08 % saving, and `P = C ÷ λ` makes the drafting worth more the tighter the
  covenant
- B. the drafting point is real and minor: a facility whose **base case** needs a cure has consumed an
  option that should be held for a downside, so the capital belongs on the sizing — the year-twelve
  shortfall of 74,849 is the 828,877 resizing question in another form ✅
- C. disagree: cure cash is always deemed to be `CFADS`, so there is nothing to negotiate
- D. disagree: cure rights are unlimited in number, so their cost is immaterial

*Rationale:* The cure arithmetic is correct and answers the smaller question, while the facility is
being sized to breach on its own base case — which is a capacity problem the cure conceals, and the
ground on which Domain 6's model auditor rejected an otherwise trivial 96,196 of curing (10.4.3,
10.1.2). A is the defensible weaker course: the identity and the saving are both real, and both are
small beside a structure that starts in breach. C asserts a single treatment where the domain describes
two standard ones, and the negotiation is ordinarily about both limbs — cash in, and where it goes. D
is false: cures are limited in number and in consecutive periods, and each one consumed is unavailable
later.

**MCQ 10.4-H `[10.4.2 · Comprehension]`** A sponsor's board is told that the facility has "a 1.20×
covenant and a 1.15× lock-up, so there are two levels to watch". The 1.25× distribution condition is
not mentioned. The clearest statement of what the three thresholds do is:
- A. the distribution condition decides whether this period's cash may leave the structure at all,
  the covenant decides whether the facility is in breach, and the lock-up decides whether retained
  cash stops being the sponsor's — three different consequences at three different levels ✅
- B. the three are alternative drafting formulations of the same test, and only the lowest binds
- C. the distribution condition and the lock-up trigger are the same mechanism, one expressed as a
  ratio and the other as a cash figure
- D. the covenant is the operative constraint and the other two are consequences of breaching it

*Rationale:* The thresholds engage in order and do different things, which is what makes the design
graduated: on Kestrel they sit at 6,262,044, 6,011,562 and 5,761,081 of annual `CFADS` (10.4.2). B
denies the sequencing the structure depends on. C conflates two mechanisms whose difference is the
whole point — failing the distribution condition delays this period's cash, while falling through the
lock-up trigger stops the cash being the sponsor's. D reverses the order of engagement: cash is
retained long before a breach becomes an event of default, and on Kestrel's own bank case the
distribution condition catches eight of twelve years while the lock-up trigger never engages at all.

### Self-check — KA 10.4

1. *What does a lock-up achieve that a covenant alone does not?* — It retains cash as coverage
   deteriorates, short of default, reducing exposure while the project is still fixable.
2. *Why do lenders rarely accelerate a sound project?* — Enforcement destroys value and leaves them
   owning the asset; renegotiation from strength is better.
3. *What is the most valuable asset in a covenant negotiation?* — A track record of early, accurate
   disclosure.
4. *Distinguish the distribution condition from the lock-up trigger.* — The first decides whether
   this period's cash may leave; the second decides whether the cash remains the sponsor's at all.
   On Kestrel, 1.25× and 1.15×.
5. *Which test basis breaches first on a declining coverage profile, and what does that buy?* — The
   forward-looking test, one test date earlier — the period in which a cure or an operational fix is
   still available.
6. *State the cure identity and what it is worth.* — `P = C ÷ λ`: a prepayment cure costs `1 − 1/λ`
   less than a deemed-`CFADS` cure — 16.6667 % at 1.20× — and reduces later periods' debt service as
   well.

---

## Advanced topics — Domain 10

### 10.A.1 Forward-looking tests and whose forecast counts

A forward `DSCR` test measures a projection, which raises three questions the documents must answer:
whose model, on what assumptions, and reviewed by whom. Facilities typically require the borrower's
model updated to an agreed basis, sometimes with the lenders' technical adviser's assumptions
prevailing on specified inputs. The practical consequence is that **assumption control becomes a
covenant matter** — a sponsor who cannot defend an assumption cannot pass a forward test on it — and
it is why Domain 13's model audit and Domain 6's model governance are contractual, not merely
prudent. Worked example 10.4.1 gives that abstraction a price on Kestrel: the difference between
breaching at the end of year ten and the end of year eleven turns entirely on a projected `CFADS`
figure that nobody can yet observe, and the whole of the forward test's early-warning value — its
one test date of notice — is bought with that unobservable number. Three drafting features are what
make the trade acceptable rather than arbitrary, and a leader should look for all three: a **defined
basis of preparation** (which case, which conventions, which vintage of assumptions); a **change
protocol** stating who may alter an assumption between tests and on what evidence; and a **tie-break**
naming whose assumption prevails on specified inputs, usually the lenders' technical adviser's on
availability and degradation. A forward test without those three is not a covenant but an invitation
to argue at the worst possible moment.

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

Six further invariants earned by this domain's worked examples, each cheap to test:

- **`DSCR ÷ LLCR` = 1.0000 for a level annuity against level cash**, and any excess measures
  principal deferred beyond the periods tested (1.1419 for a 25 % balloon, 1.9880 for a bullet).
  A coverage pack reporting a strong `DSCR` and a flat `LLCR` is reporting structure, not performance
  (10.2.3).
- **A sculpted schedule discounts at `r(1 − T/λ)`, not at `r`.** A sculpted model built by iterating
  until the closing balance looks like zero should reproduce
  `(A/λ) × AF(r(1 − T/λ), n)` to the cent; if it does not, the circularity has not converged
  (10.1.3).
- **A sculpted profile's final payment equals the level payment a minimum-period test would have
  imposed** — 4,562,811.13 on Kestrel. Two independent calculations that must agree.
- **`CFADS` used for sculpting must be the `CFADS` of the resized debt**, not of the original request:
  Kestrel's sculpted year-one `CFADS` is 6,358,990, not 6,384,000, because a smaller balance generates
  a smaller tax shield.
- **The binding covenant is found by solving every covenant for one common stress**, never by
  comparing headroom in each covenant's own units — and the answer changes with the thresholds, not
  with the project (10.2.4).
- **Every reserve appears in the uses of funds as well as in the operating cash flow**, whatever its
  funding route (10.3.2b; Domain 13, finding F-03).

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
- **Mining and resources.** The project life that `PLCR` measures is bounded by the **reserve**, not
  by the asset, so the tail is a physical quantity subject to its own technical dispute; coverage
  requirements are set well above infrastructure levels, sculpting follows the mine plan rather than
  a tax shield, and cash sweeps are close to universal because the lenders' objective is to be repaid
  out of the highest-grade years rather than across the life.

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

**The outcome.** The facility closed at the sized maximum: **41,171,123** of senior debt — debt
service `41,171,123/8.383844 =` **4,910,769**, a base-case `DSCR` of exactly **1.30000**, which is
what a credit committee documents when it has set a requirement and means it — with the residual
**828,877 funded as additional equity**, a six-month DSRA (2,504,818), sculpted service against the
declining coverage profile, and a 1.15× lock-up. Equity rose from 18,000,000 to **18,828,877**,
gearing from 70/30 to **68.62/31.38** (debt/equity **2.1866:1**), and the sponsors' modelled IRR fell
by roughly 40 basis points — the true price of the coverage requirement, paid in equity rather than
argued away. Note that the 828,877 is not a new number: it is precisely the gap Worked example 10.1.2
computed at the top of this domain, which is the point. **The sizing arithmetic did not describe the
negotiation; it determined its outcome**, and every hour spent arguing about the amount was spent
arguing with a division. Domain 6 models this structure and Domain 15 operates it, both on the same
41,171,123.

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
- **Whether the coverage target is a base-case or an every-period test.** It is the single largest
  unstated term in a sizing negotiation: on Kestrel the two readings of the same 1.30 × differ by
  **2,917,226** of debt, and sculpting recovers 1,661,916 of the difference without conceding a basis
  point (10.1.3).
- **What structure is being asked to do for the ratios.** A balloon or a bullet moves `DSCR` and moves
  nothing else; if coverage improved without the project changing, the improvement is deferral and
  the maturity obligation belongs in the same sentence (10.2.3).
- **The reserve's funding route.** Equity, debt or a letter of credit is a decision with a computable
  answer — 311,098 a year, a 2,504,818 displacement of capex borrowing, or a fee against a 12.42 %
  breakeven — and it is decided far too often by whoever drafted the term sheet (10.3.2b).
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

**Exercise 10.5** A project generates `CFADS` of **8,400,000** a year before the interest tax shield,
borrows at **7.0 %** over **10 years**, pays tax at **25 %**, and must hold **1.35×** coverage in
every period. Compute the effective sculpting rate and the maximum sculpted debt, and state the error
made by a modeller who discounts the sculpted profile at the loan rate.
*Solution.* `r* = 0.07 × (1 − 0.25/1.35) =` **5.703704 %**; `AF(0.05703704, 10) =` **7.464519**;
the sizing basis is `A/λ = 8,400,000/1.35 =` **6,222,222**, and maximum sculpted debt
`6,222,222 × 7.464519 =` **USD 46,445,895**. Actual debt service *falls* with the shrinking shield,
from `(8,400,000 + 0.25 × 0.07 × 46,445,895)/1.35 =` **6,824,299** in the first period to
**6,298,528** in the tenth — `A/λ` is the annuity-equivalent basis, not the first payment.
Discounting at the full 7.0 % gives `AF(0.07, 10) = 7.023582` and **43,702,285** — an
**understatement of 2,743,609**, because it ignores the tax saving that each dollar of interest returns to the same `CFADS` line the
coverage test divides. *Common error:* solving the circularity by iterating a workbook until the
closing balance "looks like" zero; the closed form is deterministic and should tie to the cent.

**Exercise 10.6** A 40,000,000 facility runs **10 years at 6.5 %** against level `CFADS` of
**7,000,000**. Compare full amortisation with a **30 % balloon**: compute the level payment, `DSCR`,
`LLCR`, the `DSCR ÷ LLCR` reading, and the year-ten obligation with that year's coverage.
*Solution.* `AF(0.065, 10) = 7.188830`; `1.065⁻¹⁰ = 0.532726`. **Full amortisation:** payment
`40,000,000/7.188830 =` **5,564,188**, `DSCR` **1.2580**, `LLCR`
`7,000,000 × 7.188830/40,000,000 =` **1.2580** — equal, as the level-cash identity requires, so
`DSCR ÷ LLCR` = **1.0000**. **30 % balloon (12,000,000):** payment
`(40,000,000 − 12,000,000 × 0.532726)/7.188830 =` **4,674,931**, `DSCR` **1.4973**, `LLCR`
**unchanged at 1.2580**, `DSCR ÷ LLCR` = **1.1902**. Year-ten obligation
`4,674,931 + 12,000,000 =` **16,674,931**, against which that year's coverage is **0.4198**.
*Common error:* reporting the balloon structure's 1.4973 as an improvement in credit quality; nothing
about the project changed and 1.1902 is the measure of the deferral that produced it.

**Exercise 10.7** A facility carries `DSCR` ≥ **1.20×**, `ICR` on `EBITDA` ≥ **3.00×** and
debt/`EBITDA` ≤ **5.50×**. Base case: revenue **20,000,000**, `EBITDA` **9,000,000** (of which cash
cost 10 % of revenue is variable), depreciation 3,000,000, interest 2,600,000, tax 25 %, debt
**40,000,000**, debt service **6,500,000**. Identify the covenant that binds first under a common
revenue stress.
*Solution.* `CFADS = 9,000,000 − 0.25 × (9,000,000 − 3,000,000 − 2,600,000) =` **8,150,000**, so
`DSCR` = **1.2538**. A revenue fall of `x` reduces `EBITDA` by `0.9 × 20,000,000x = 18,000,000x` and
`CFADS` by `0.75 × 18,000,000x = 13,500,000x`. `DSCR`:
`(8,150,000 − 13,500,000x) = 7,800,000` → **2.5926 %**. `ICR`:
`9,000,000 − 18,000,000x = 7,800,000` → **6.6667 %**. Leverage:
`9,000,000 − 18,000,000x = 40,000,000/5.5 = 7,272,727` → **9.5960 %**. **`DSCR` binds first, at a
2.5926 % revenue fall** — less than half the next covenant's tolerance. *Common error:* ranking the
covenants by headroom in their own units (0.0538 of `DSCR`, 0.4615 of `ICR`, 1.0556 of leverage),
which are not comparable quantities; only a common stress variable ranks them.

**Exercise 10.8** A 1.25× `DSCR` covenant is breached: `CFADS` **7,600,000** against debt service
**6,500,000**. Size the equity cure under each drafting treatment and state the identity that links
them.
*Solution.* Deemed `CFADS`: `1.25 × 6,500,000 − 7,600,000 =` **525,000**. Applied to prepayment:
`6,500,000 − 7,600,000/1.25 =` **420,000**. The prepayment cure is `C ÷ λ`, so it costs
`1 − 1/1.25 =` **20.0000 %** less — **105,000** on these figures — and it also reduces later periods'
debt service. *Common error:* multiplying by the covenant instead of dividing (656,250), which makes
the prepayment route look dearer and reverses the negotiating position.

**Exercise 10.9** Debt service is **6,500,000** a year and base-case `CFADS` **8,150,000**. Compute
the single-year shortfall tolerance with no reserve, with three months and with six months; state what
each month buys; and find the breakeven letter-of-credit fee for a three-month reserve where the cost
of equity is **14.0 %** and the reserve account earns **2.5 %**.
*Solution.* Tolerance = `1 − DS × (1 − m/12) ÷ CFADS`: **20.2454 %** with no reserve, **40.1840 %** at
three months (1,625,000 funded), **60.1227 %** at six. Each month buys **6.6462** percentage points
for **541,667** of funded cash. Breakeven LC fee `= 14.0 − 2.5 =` **11.50 %** a year; the equity
carry on a three-month reserve is `1,625,000 × 0.115 =` **186,875** a year, against **16,250** for an
LC at 1.0 % — a saving of **170,625** a year. *Common error:* comparing the LC fee with the reserve's
*principal* rather than with the carry on it, which makes cash funding look free.

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
- [ ] Every covenant has a modelled test date; **distribution condition and lock-up trigger modelled
      as well as the covenant**, and both test bases (historic and forward) reported.
- [ ] `DSCR ÷ LLCR` computed; any excess over 1.0000 explained by the amortisation profile, and the
      maturity obligation stated beside the ratio.
- [ ] Sculpted schedules reproduce `(A/λ) × AF(r(1 − T/λ), n)` to the cent, and the `CFADS` line used
      is the resized structure's, not the original request's.
- [ ] Weighted average life reported for the base profile and for every sweep case.
- [ ] Every reserve appears in the **uses** of funds, with its funding route (equity, debt or LC) and
      that route's annual cost stated.
- [ ] The binding covenant identified by solving all covenants for one common stress.
- [ ] AI-produced covenant summaries sampled against the documents; verifier named.

### Toolkit 10.T.4 — Sizing-basis statement (one page, agreed before any capacity number circulates)

Nine lines, each with a named owner and a document reference. **1 — `CFADS` definition** and its
clause. **2 — the case**: base, bank or sponsor, with its revenue and escalation assumptions.
**3 — the test**: base-case (single-period) or minimum-across-all-periods, and which period is
expected to bind. **4 — the target coverage**, and whether it is a sizing target, a covenant, or both.
**5 — the service profile**: level, sculpted or structured, and if sculpted, the target `λ`, the
effective rate `r(1 − T/λ)` and the re-cut provision. **6 — deferred principal**: balloon or bullet
amount, the maturity obligation in cash, and the stated refinancing plan. **7 — reserves**: each
reserve, its required balance, its funding route and that route's annual cost. **8 — prepayment**:
sweep share, its base, its priority in the waterfall, and the average life before and after.
**9 — the thresholds**: distribution condition, covenant and lock-up trigger, each in ratio *and* in
`CFADS`, with the years each catches on the agreed case. Rule: a debt-capacity figure quoted without
lines 1 to 5 agreed is an opinion, not a number — and the three answers of Worked example 10.1.3 are
what happens when it is quoted anyway.

## Exam preparation — Domain 10

**The traps.** Sizing debt on full `CFADS` without the coverage divisor (Exercise 10.1) · quoting a
`DSCR` without its `CFADS` definition (10.1.1) · treating covenant breach and payment default as the
same event (Exercise 10.4, Case study B) · discounting `CFADS` at an equity rate in `LLCR`
(Exercise 10.2) · reporting average rather than minimum `DSCR` · modelling the covenant but not the
lock-up (10.4.2) · assuming a DSRA prevents breach (MCQ 10.3-B) · using accounting rather than cash
tax in `CFADS` · inverting `DSCR` · relying on `PLCR` as a lender comfort · quoting a debt capacity
without saying whether the coverage test is base-case or every-period (10.1.3) · discounting a
sculpted profile at the loan rate instead of at `r(1 − T/λ)` (Exercise 10.5) · sculpting against the
`CFADS` of the *original* request rather than of the resized structure (10.1.3) · treating a lower
target `DSCR` attached to a deeper stressed case as a concession without computing `1 − λ₂/λ₁`
(10.1.2b) · reading a balloon's improved `DSCR` as improved credit quality when `LLCR` has not moved
(10.2.3) · ranking covenants by headroom in their own units instead of by a common stress (10.2.4) ·
comparing a letter-of-credit fee with a reserve's principal rather than with the carry on it
(Exercise 10.9) · funding a reserve from a coverage-constrained facility and believing the equity
requirement has been avoided (10.3.2b) · multiplying rather than dividing by the covenant when sizing
a prepayment cure (Exercise 10.8) · reporting the tenor rather than the weighted average life as what
a sweep delivers (10.3.3).

**Reflection questions.**
1. For your facility: state the `CFADS` definition from memory, then check it against the document.
   How close were you, and what would the difference have done to your reported ratio?
2. At what `CFADS` level does your first covenant fail, and is that number on anyone's dashboard?
3. Does your distribution forecast pass the lock-up tests, or only the covenant tests?
4. Is your coverage target tested on the base case or in every period, and can you say which document
   settles it? What would the other reading do to your facility size?
5. Which of your facility's thresholds actually catches a year on the lenders' own case — and is it
   the one your team spent its negotiating capital on?
6. If your schedule carries deferred principal, what is the obligation at maturity in cash, and what
   is that year's coverage against it?

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

Four results deepen that summary and are the domain's own contribution. **A coverage target is
meaningless until it says which periods it applies to.** Kestrel's 1.30 × supports 41,171,123 as a
base-case test and only **38,253,896** as an every-period test against the declining bank case — a
difference of **2,917,226** — and **sculpting recovers 1,661,916 of it** at **39,915,812**, holding
1.30 × in every year, because the sculpted profile discounts at the effective rate
**`r(1 − T/λ)` = 5.076923 %** rather than at the 6.0 % loan rate. The sculpted schedule's final
payment, 4,562,811.13, is precisely the level payment a minimum-period test would have imposed
throughout — which is the clearest statement of what level sizing does wrong against uneven cash.
**Structure moves `DSCR` and moves nothing else.** The same 42,000,000 against the same cash reports
1.2743 fully amortising, **1.4551** with a 25 % balloon and **2.5333** as a bullet, while `LLCR`,
`PLCR` and `ICR` do not move at all; the `DSCR ÷ LLCR` reading (1.0000 · 1.1419 · 1.9880) measures the
deferral, and the balloon structure's year-twelve coverage against its 14,887,226 obligation is
**0.4288**. **The covenant that binds is found by stress, not by inspection.** On the mandated
facility a revenue fall of **4.1382 %** breaches the year-one `DSCR`, **4.4444 %** the leverage
covenant, **4.9833 %** the `LLCR` and **10.6667 %** the `ICR` — while the year-twelve `DSCR` is
already breached by **74,849** before any stress at all, and an `ICR` drafted at 2.90 × instead of
2.50 × would bind first of the four at **1.7067 %**. **And the threshold that traps cash is rarely the
one that was negotiated:** Kestrel's 1.25 × distribution condition catches **eight of twelve years**
and **3,956,574** of dividend — 57.61 % of the loan period's distributable cash, worth 2,165,274 at
8 % — while the 1.15 × lock-up trigger never engages. Alongside these, three pieces of practical
arithmetic: each month of DSRA buys **6.5393** percentage points of single-year cash tolerance for
417,470 of funded cash; the reserve's funding route is a real decision with a **12.42 %** breakeven
letter-of-credit fee and a debt route that relocates rather than avoids the equity requirement; and a
50 % cash sweep shortens the loan's weighted average life from **7.1887 to 6.4660 years**, which is
what the lender is buying, at a present-value cost to equity of 315,488. Finally, the cure identity
**`P = C ÷ λ`**: the same breach costs 96,196 cured as deemed `CFADS` and **61,485** cured by
prepayment, because prepayment works on the denominator and improves every later period as well.

Domain 11 allocates the risks these ratios are stressed against; Domain 13 audits the model that
computes them; Domain 15 operates the waterfall and handles the restructuring end.
