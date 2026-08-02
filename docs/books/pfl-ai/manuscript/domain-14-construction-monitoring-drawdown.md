# Domain 14 — Construction Monitoring and Drawdown *(quantitative)*

> **Group:** Operating and the future (Domain 14 of 16, opening Part Four). **Target:** ~75 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain owns no new symbol. It composes registered ones —
> `CFADS`, `DSCR`, `AF(r, n)`, `NPV`, `PI`, `EAC`, `ETC`, `PoC`, `EMV`, `DSRA`, `D/E` — into the
> four tests a construction lender actually runs: the **draw request**, the **funds sufficiency
> (in-balance) test**, the **contingency coverage test on the remainder**, and the **coverage test
> at the first repayment date**. Where a figure was derived in an earlier domain it is **cited, not
> re-derived**: the construction funding profile and the pro-rata capitalised interest of
> **USD 2,114,597** come from Domain 6 (KA 6.2.1); the capitalised-interest area rule and the
> economic cost of a month of slip come from Domain 8 (KA 8.2, 8.4); the daily cost of a slip at
> the commercial operations date comes from Domain 5 (KA 5.4.2); the contract limits come from
> Domain 12 (KA 12.1); the coverage machinery comes from Domain 10. British English; USD (+SAR
> where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domain 13 closed the transaction. Every domain before it answered a question about a project that
did not yet exist: is it worth building, can it carry debt, who bears which risk, what do the
documents say. This domain answers the first question about a project that does exist and is being
built with other people's money, and it is a narrower and harder question than any of them:
**on the evidence available this month, may the next tranche of funds be released?**

That question is not "is the project going well?" and it is not "what will it finally cost?" It is
a test with a binary answer, run against a defined evidence pack, on a defined date, by a person
who signs. The whole apparatus of construction monitoring exists because limited-recourse debt is
advanced **before** the asset that secures it exists, so the lender's only protection between
financial close and commercial operations is the discipline with which each release of funds is
conditioned on certified progress and on a demonstration that the remaining money still finishes
the job. Everything in this domain follows from that single structural fact.

The domain's central claim is that **the construction-phase tests are not a finance version of
project controls; they are a different discipline reading the same data spine, and the differences
are systematic, nameable and computable.** A project controls forecast produces a range and a
narrative (PML-AI Domain 7's `EAC` family). A lender's cost-to-complete produces one number, a
date and, when it fails, a cash call — and it is built on a **commitment** basis, not a
performance-extrapolation basis, so on a fixed-price contract the two can diverge by millions while
both are correct on their own terms. Reconciling them, line by line, is the single most valuable
professional skill this domain teaches, and Toolkit 14.T.2 exists to make it routine.

**Learning objectives.** After this domain a candidate can: restate a sources-and-uses statement at
a data date into drawn, committed and available columns and explain why the identity is a
construction, not a check; build a draw request from certified value through retention and advance
recovery to a net funding requirement and its pro-rata debt and equity shares; compute the
capitalised interest consequence of a funding order (pro rata, equity-first, debt-first) and state
whose interest each order serves; compute a lender's cost-to-complete on a commitment basis and
reconcile it, line by line, to a `BAC/CPI` or bottom-up `EAC` from the same data spine; explain why
a blended `CPI` misattributes an overrun across fixed-price and owner-retained scope; run a funds
sufficiency test and identify the double-count that makes most of them pass wrongly; compute a
contingency coverage ratio on the remainder and defend the choice of denominator; value the same
month's work on milestone, measured and cost-incurred certification bases and quantify the debt
advanced against work not in place; compute the marginal `DSCR` of a variation and the debt share
at which it becomes coverage-dilutive; distinguish the **funded** cost of a month of delay from its
**economic** cost and show that one liquidated-damages rate can over-recover the first while
under-recovering the second; compute the `DSCR` at a calendar-fixed first repayment date under a
late commercial operations date, with its covenant, payment and reserve breakevens; compute a
buy-down that restores a stated coverage level; and govern AI-assisted certification, cost-report
and covenant analysis under the family's verification rule.

**The master construction.** Kestrel Water SPC is being built. Domain 6 (KA 6.2.1) fixed the
funding: a **USD 60,000,000** envelope, **USD 42,000,000** senior debt at **6.0 %** and
**USD 18,000,000** equity, funded **70/30** against certified spend over **eight construction
quarters** on a profile of **6, 9, 13, 16, 17, 15, 13 and 11 per cent**; uses of EPC price
**48,000,000**, owner's costs and land **3,600,000**, capitalised development **1,800,000**,
arrangement and financing fees **840,000**, a balancing contingency of **3,645,403** and
capitalised interest of **2,114,597**. The certified-spend base is therefore
**55,245,403**, and cumulative debt drawn reaches **31,990,655** by the end of quarter six.
Domain 12 (KA 12.1.1) fixed the contract limits: delay damages **20,000 per day** capped at
**4,800,000** (day 240), performance damages capped at 4,800,000, aggregate liability
9,600,000. Domain 10 fixed the operating regime: annual debt service **USD 5,009,635.23**, base
`CFADS` **6,384,000**, `DSCR` **1.2743**, a **1.20×** covenant biting at `CFADS` of
**6,011,562** (annual headroom **372,438**), a **1.15×** lock-up and a six-month debt service
reserve of **2,504,818**. This domain takes that structure to the site and runs it, quarter by
quarter, to the completion tests.

---

## Knowledge Area 14.1 — Sources and uses, draw requests and conditions

*Topics: 14.1.1 the sources-and-uses statement as a control document · 14.1.2 the draw request ·
14.1.3 funding order, and who pays for it.*

### 14.1.1 The sources-and-uses statement as a control document

**Definition.** A **restated sources-and-uses statement** is the financial-close statement
re-expressed at a data date into three columns — **drawn to date**, **remaining to be funded**, and
**available commitment** — so that the identity that held at close (sources = uses) can be tested
prospectively rather than merely admired retrospectively.

Domain 6 established that sources equal uses is an *identity*, not a check: a model can always be
made to satisfy it, and what makes it informative is knowing which line balances. In construction
monitoring the same point returns with teeth. **The close statement is a plan; the restated
statement is a test.** At close, the balancing line was contingency. At a data date the balancing
line is whatever the sponsors will inject when the columns do not reconcile, and its size is the
number this whole knowledge area exists to compute.

Three columns, and one rule for each. **Drawn to date** is a fact, reconciled to the facility
agent's records and the SPV's bank statements, not to the model. **Remaining to be funded** is a
forecast, and the whole of KA 14.2 is about how it is built. **Available commitment** is a
contractual quantity — undrawn debt plus uncalled equity — and it carries the trap that catches
most first drafts: **the undrawn commitment already includes the undrawn contingency.** Contingency
is not a separate source of money; it is a *use* funded by the same 70/30 commitment as every other
use. Adding "remaining contingency" to "undrawn debt plus uncalled equity" double-counts it, and
the error flatters the in-balance test by exactly the contingency balance — which is precisely the
period when the test most needs to be honest.

**Worked example 14.1.1 — Kestrel's restated statement at the quarter-five data date.**

1. **Setup.** Five construction quarters are complete. On Domain 6's profile the planned cumulative
   certified spend is `55,245,403 × 0.61 =` **33,699,696**, funded by cumulative debt of
   **25,917,751** and equity of **11,107,608**, with capitalised interest of **685,663** accrued to
   date (quarterly amounts 27,720, 62,816, 115,682, 192,307 and 287,138) and **1,428,934** planned
   for quarters six to eight. Actual certified spend is higher: the owner-retained scope has cost
   **4,200,000** against a budget value of work done of **2,520,000**, and **465,403** of
   variations have been certified — so actual cumulative certified spend is **33,945,403**, or
   **245,707** above plan, drawn in quarter five. Restate the statement and test the balance.
2. **Formula.** Additional draw = actual − planned certified spend, split 70/30. Available
   commitment = (42,000,000 − cumulative debt) + (18,000,000 − cumulative equity). Additional
   interest = additional debt draw × 0.015 × remaining quarters. Balance = available − remaining
   uses.
3. **Substitution.** `245,707 × 0.70 = 171,995`; `245,707 × 0.30 = 73,712`;
   `(42,000,000 − 26,089,746) + (18,000,000 − 11,181,320)`;
   `171,995 × 0.015 × 3 = 7,740`; remaining base-scope spend
   `55,245,403 × 0.39 − 245,707 = 21,300,000`.
4. **Result.**

   | Restated statement at the quarter-five data date | Drawn (USD) | Remaining (USD) | Total (USD) |
   |---|---|---|---|
   | Fees and capitalised development | 2,640,000 | — | 2,640,000 |
   | EPC contract price | 29,280,000 | 18,720,000 | 48,000,000 |
   | Owner-retained scope, at budget | 2,520,000 | 1,080,000 | 3,600,000 |
   | Owner-scope overrun and certified variations, from contingency | 2,145,403 | 1,500,000 | 3,645,403 |
   | Interest during construction | 685,663 | 1,436,674 | 2,122,337 |
   | **Total uses** | **37,271,066** | **22,736,674** | **60,007,740** |
   | Senior debt | 26,089,746 | 15,910,254 | 42,000,000 |
   | Equity | 11,181,320 | 6,818,680 | 18,000,000 |
   | **Total sources / available commitment** | **37,271,066** | **22,728,934** | **60,000,000** |

   Remaining uses **22,736,674** against available commitment **22,728,934** — the project is
   **out of balance by USD 7,740** on the plan's own assumptions.
5. **Interpretation.** Read the 7,740 first, because it is the whole discipline in miniature.
   Nothing has gone wrong that the plan did not contain: 245,707 of contingency was drawn three
   quarters earlier than the model assumed it would be, and `171,995 × 0.015 × 3` is the interest
   that early draw costs. **Drawing contingency early is not free, and the price is computable to
   the dollar.** A facility whose availability is measured to the last dollar of commitment is
   therefore out of balance the moment any use is accelerated, which is why prudent structures
   either fund a small unallocated headroom above the modelled envelope or provide expressly that
   the sponsor tops up timing differences — and why the sponsor should know which of those two the
   documents say before the first draw, not during the fifth. The second reading is the composition
   of the remaining column. **Only 18,720,000 of the 22,736,674 is a fixed contractual commitment**;
   1,080,000 is a budget for owner-retained work whose demonstrated cost efficiency is 0.60
   (KA 14.2), 1,500,000 is contingency against risks not yet run, and 1,436,674 is interest that
   depends on the draw profile of everything above it. Three of the four lines are forecasts, and
   the in-balance test is only as strong as the weakest of them. Third, notice what the identity
   does *not* tell you. The columns reconcile within 7,740 while — as KA 14.2 will show — the
   lender's own cost-to-complete on the same data date is **1,927,740** short. **A sources-and-uses
   statement built from the model balances by construction; a sources-and-uses statement built from
   the commitments does not, and the gap between them is the report.**

### 14.1.2 The draw request

**Definition.** A **draw request** (or drawdown notice, or advance request) is the borrower's
certified application for the release of funds, and it is a composite document: an *arithmetic*
claim (this much money, split this way), an *evidential* claim (this work is in place, certified by
this person), and a *representational* claim (the conditions to drawing are satisfied and the
project remains in balance). Lenders decline draws far more often on the third limb than on the
first.

The arithmetic runs in one direction and each step has a reason:

```
Gross certified value of work executed in the period
  − retention withheld
  − recovery of the advance payment
  + owner-retained costs incurred and evidenced
  + fees, taxes and premiums due in the period
  + interest and commitment fees accrued
  = period funding requirement
      × gearing            → senior debt draw
      × (1 − gearing)      → equity draw (or equity contribution certificate)
```

The **conditions to each drawing** are a shorter and more consequential list than the conditions
precedent to first drawing (Domain 13, KA 13.3): no default or potential default continuing; the
representations repeated and still true; the certificate of the independent engineer or lender's
technical adviser attached; insurances in force and premiums paid; the project **in balance**; and
the drawing within the availability period and within the commitment. Two of these are judgments
rather than facts — "no potential default" and "in balance" — and they are where a monitoring
relationship is actually conducted.

**Worked example 14.1.2 — the quarter-five draw, built line by line.**

1. **Setup.** Quarter five on Domain 6's profile: planned certified spend `55,245,403 × 0.17 =`
   **9,391,718**; interest accrued on the opening debt balance of 19,142,551 at 1.5 % is
   **287,138**. The additional certified contingency spend of **245,707** identified in 14.1.1 falls
   in this quarter. Gearing 70/30. Compute the request and the resulting balances.
2. **Formula.** Requirement = certified spend + accrued interest; debt draw = 0.70 × requirement;
   equity draw = 0.30 × requirement.
3. **Substitution.** `9,391,717.86 + 245,706.76 + 287,139.20 = 9,924,563.82`, i.e. **9,924,564**;
   `× 0.70`; `× 0.30`.
4. **Result.** Period funding requirement **USD 9,924,564**; senior debt draw **6,947,195**;
   equity draw **2,977,369**. Closing cumulative debt **26,089,746** (62.12 % of commitment) and
   equity **11,181,320** (62.12 % of commitment); undrawn commitment **22,728,934**.
5. **Interpretation.** Three things in that line are worth a leader's attention. **The interest
   line is a draw.** 287,138 of the request is money the project is borrowing in order to pay
   interest on money it has already borrowed, which is exactly what capitalised interest means and
   exactly why Domain 8's area rule matters: the interest row grows with the drawn balance, so it is
   largest in the quarters when the certified-spend row is already falling. In quarter eight
   Kestrel's interest row is 560,308 against a spend row of 6,076,994 — **8.44 %** of the period
   requirement, against **0.83 %** in quarter one. A sponsor budgeting equity calls from a
   construction spend curve alone will under-call late in the programme, every time. **The equity
   draw is a call, and calls have lead times.** 2,977,369 has to arrive from shareholders whose own approval processes
   (PML-AI Domain 3's governance latency) may run to weeks; a draw request that reveals the call
   is a draw request submitted too late. The professional practice is a rolling twelve-week funding
   forecast issued to shareholders, reconciled to the draw schedule, so that no call is news.
   **And the pro-rata split is a contractual mechanic, not an accounting one.** Each draw carries
   its own 70/30 split, which is why the cumulative percentages move in lockstep — 62.12 % of both
   commitments drawn — and why any departure from that lockstep is either a documented funding
   order (14.1.3) or an error worth finding immediately.

### 14.1.3 Funding order, and who pays for it

**Definition.** The **funding order** is the contractual rule determining, for each period's
requirement, how much comes from debt and how much from equity. Three orders are standard:
**pro rata**, in which every draw carries the gearing ratio; **equity-first**, in which the sponsors'
whole commitment is exhausted before any debt is drawn; and **debt-first** (sometimes "debt-first
with an equity backstop"), in which the facility is drawn to its limit before equity is called. The
choice is usually settled in a single sub-clause of the facility agreement and is routinely treated
as boilerplate. It is not: it moves capitalised interest, credit exposure and the sponsor's return,
in different directions, by amounts that dwarf most of what is negotiated around it.

**Worked example 14.1.3 — the same project under three funding orders.**

1. **Setup.** Kestrel's certified-spend base **55,245,403** on the eight-quarter profile, fees and
   development of **2,640,000** funded at close, interest at **1.5 %** per quarter on the opening
   debt balance, draws treated as made at period end. Scope and contingency are held identical
   across the three orders so that only the sequencing changes. Compute total capitalised interest,
   the resulting funding requirement, and the sponsor's share of funds in the ground at each draw
   date.
2. **Formula.** For each period: `interest = opening debt balance × 0.015`;
   `requirement = certified spend + interest`; the split follows the order; total uses =
   2,640,000 + 55,245,403 + Σ interest.
3. **Substitution.** Pro rata: `debt draw = 0.70 × requirement` throughout. Equity-first:
   `equity draw = min(requirement, 18,000,000 − equity drawn)`, the balance from debt. Debt-first:
   `debt draw = min(requirement, 42,000,000 − debt drawn)`, the balance from equity.
4. **Result.**

   | Funding order | Capitalised interest (USD) | Total uses (USD) | Debt drawn (USD) | Equity drawn (USD) | `D/E` |
   |---|---|---|---|---|---|
   | **Equity first** | **1,338,006** | 59,223,409 | 41,223,409 | 18,000,000 | 2.2902 |
   | **Pro rata 70/30** | **2,114,597** | 60,000,000 | 42,000,000 | 18,000,000 | 2.3333 |
   | **Debt first** | **2,804,070** | 60,689,473 | 42,000,000 | 18,689,473 | 2.2473 |

   Equity-first saves **776,591** of capitalised interest against pro rata (**36.73 %**);
   debt-first costs **689,473** more (**32.61 %**). The spread from best to worst is
   **USD 1,466,064**, or **2.4434 %** of the envelope. Under debt-first no sponsor cash is in the
   ground until quarter six, and at the quarter-five data date the lender's exposure is
   **37,323,907** against **25,917,751** pro rata — a difference of **11,406,157** — with
   **zero** equity contributed against **11,107,608**.
5. **Interpretation.** Start with the size of the number, because it settles a question of
   priorities. Kestrel's total drawn-balance exposure over the build is `2,114,597 / 0.015 =`
   **140,973,150** quarter-dollars, so a ten-basis-point movement in the margin is worth
   `140,973,150 × 0.001/4 =` **35,243** across the whole construction period. The funding-order
   spread of 1,466,064 is **41.6 times** that. Financings are routinely negotiated for weeks over
   the margin and settle the funding order by accepting the lender's draft, which is an allocation
   of professional attention almost exactly inverted. **Second, the three orders serve three
   different interests and the arithmetic says so.** Equity-first is cheapest for the *project* and
   safest for the *lender*, because it puts sponsor money in first — which is why lenders draft it
   and why sponsors resist it. Debt-first is dearest for the project and worst for the lender, and
   the sponsor still prefers it: discounting each order's equity draws at Domain 4's 8 % (a
   quarterly 1.9427 %), pro rata has a present value of **16,476,605** and debt-first
   **16,293,592**, so deferring the cheque is worth **183,013** of present value to the sponsor
   even though it costs the project 689,473 of capitalised interest, which the sponsor then funds as
   additional equity. **A sponsor arguing for debt-first is buying 183,013 of present value for
   689,473 of nominal cost, and both statements are true.** Whether that is rational depends on the
   sponsor's own cost of capital, which is exactly the conversation the clause deserves and rarely
   gets. **Third, at a fixed envelope the saving reappears as protection.** Hold the 60,000,000
   envelope and re-solve for the balancing contingency under equity-first, and contingency rises
   from 3,645,403 to **4,388,050** — **9.14 %** of the EPC price against 7.59 %, an extra
   **742,647** of funded protection bought with a sequencing clause and no additional money. That is
   the version of this calculation to take to a credit committee, because it converts an interest
   saving into the currency the committee actually cares about. **The professional caution:** none
   of this survives contact with a project that stops. Equity-first maximises the sponsor's
   irrecoverable exposure if the works are abandoned early, and a sponsor with a weak balance sheet
   may simply be unable to fund it — which is why the honest framing is not "equity-first is best"
   but "the funding order allocates construction-phase risk and cost between three parties, and the
   allocation must be priced before it is signed."

> **Fig 14.1.1 — Funding order: who is exposed, and what the interest costs.** Two-panel figure.
> Left: line chart, x-axis the nine draw dates (financial close plus eight construction quarters),
> y-axis the sponsor's share of cumulative funding drawn (0–100 %). Three series: pro rata flat at
> **30.00 %**; equity-first falling from **100.00 %** through **66.79 %** at quarter four to
> **30.39 %** at quarter eight; debt-first at **0.00 %** until quarter six, then rising through
> 9.03 % and 22.20 % to **30.80 %**. A dashed vertical at quarter five annotated "at Q5 the
> lender's exposure differs by USD 11,406,157". Right: three horizontal bars of total capitalised
> interest — equity-first **1,338,006** (brand blue), pro rata **2,114,597** (slate), debt-first
> **2,804,070** (crimson) — with a footer stating the spread of **1,466,064**, being **2.4434 %**
> of the 60,000,000 envelope and **41.6×** the cost of a ten-basis-point margin move.
> Source: PCI original. Alt text: a line chart showing three very different profiles of sponsor
> money in the ground across a construction programme, flat for pro-rata funding, front-loaded for
> equity-first and absent until late for debt-first, beside three bars showing that the cheapest
> capitalised interest belongs to the order in which the sponsor funds first.

### AI in this KA

**Where it earns its place.** Draw-request assembly is repetitive, document-heavy and
deadline-driven, which is exactly where machine assistance pays: extracting certified quantities and
values from a certifier's schedules into the draw template; reconciling the requested amount against
the facility agent's drawn-to-date records and flagging every discrepancy; checking that each
condition to drawing has a corresponding, in-date piece of evidence in the pack (insurance
certificates, the technical adviser's certificate, the compliance certificate) and listing what is
missing; and maintaining the rolling funding forecast so that no equity call is news. Recomputing
the whole funding table under an alternative funding order — the arithmetic of 14.1.3 — is likewise
machine work, and it is work almost nobody does by hand, which is precisely why the clause goes
unnegotiated.

**Where it must not go.** Nothing in this knowledge area may generate a **representation**. The
"no default continuing" and "in balance" statements in a draw request are certifications by named
officers with consequences attached, and a model that concludes them has produced a draft for a
human to verify and sign, never a signature. Nor should an assistant be permitted to *reconcile*
a discrepancy it finds between the requested amount and the agent's records; its job is to surface
the difference, because the reconciliation is where an error becomes either a correction or a
misstatement.

**Verification, concretely.** Recompute one period's requirement by hand from the certifier's
gross value — retention — advance recovery + owner costs + accrued interest, and require agreement
to the dollar. Independently recompute accrued interest from the opening balance and the day-count
basis (Domain 8's area rule reproduces the whole IDC line from the profile, the rate and the
gearing, and disagreement is a defect not a rounding difference). Confirm that cumulative debt and
equity percentages are equal under pro-rata funding, and that any divergence traces to a documented
funding order. Tie drawn-to-date to the agent's statement, not to the model.
**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 14.1

| Term | Meaning |
|---|---|
| **Restated sources and uses** | The close statement re-expressed at a data date into drawn, remaining and available columns; a test rather than an identity. |
| **Available commitment** | Undrawn debt + uncalled equity. Already includes undrawn contingency — adding contingency again double-counts. |
| **Draw request** | The certified application for funds: an arithmetic, an evidential and a representational claim. |
| **Conditions to each drawing** | The per-draw conditions: no default, representations repeated, certificates attached, insurances in force, in balance, within availability and commitment. |
| **Funding order** | Pro rata, equity-first or debt-first; allocates capitalised interest, credit exposure and sponsor return. |
| **Equity contribution certificate** | Evidence that the equity share of a draw has been provided, conditioning the debt share. |

### Sample MCQs — KA 14.1

**MCQ 14.1-A `[14.1.1 · Application]`** At a data date, undrawn senior debt is 15,910,254 and
uncalled equity is 6,818,680; remaining unallocated contingency is 1,500,000. Available commitment
for the in-balance test is:

- A. USD 24,228,934
- B. USD 22,728,934 ✅
- C. USD 15,910,254
- D. USD 21,228,934

*Rationale:* Available commitment is `15,910,254 + 6,818,680 = 22,728,934`. A adds the 1,500,000 of
contingency, which is a **use** funded by that same commitment — the double-count of 14.1.1, and it
flatters the test by exactly the contingency balance. C counts only debt and ignores the equity
commitment. D deducts the contingency instead of ignoring it, an error in the opposite direction.

**MCQ 14.1-B `[14.1.3 · Analysis]`** On identical scope and contingency, Kestrel's capitalised
interest is 1,338,006 under equity-first funding and 2,804,070 under debt-first. The soundest
reading is:

- A. debt-first is a modelling error, since total funding is fixed at 60,000,000
- B. the 1,466,064 spread is a real cost of the sequencing clause; equity-first is cheapest for the project and safest for the lender, while debt-first defers the sponsor's cheque and is worth 183,013 of present value to the sponsor ✅
- C. the orders are economically equivalent because the sponsor funds the difference either way
- D. the difference is capitalised, so it has no effect on the project's economics

*Rationale:* The sequencing changes the drawn balance on which interest accrues, so it changes total
uses (59,223,409 / 60,000,000 / 60,689,473) — the arithmetic of 14.1.3. A misreads a fixed envelope
as a fixed requirement. C ignores that the sponsor's *present value* improves while the project's
nominal cost worsens. D is the commonest form of the error: capitalised interest enters the
depreciable base and the coverage arithmetic and is therefore paid, with interest, over the loan
life.

**MCQ 14.1-C `[14.1.2 · Application]`** Kestrel's quarter-eight draw carries certified spend of
6,076,994 and accrued interest of 560,308. As a share of the period requirement, the interest line
is closest to:

- A. 0.83 %
- B. 8.44 % ✅
- C. 9.22 %
- D. 3.52 %

*Rationale:* `560,308 / (6,076,994 + 560,308) = 8.44 %`. A is the quarter-one share, the point being
that the interest row grows as the spend row falls. C divides interest by certified **spend** rather
than by the period requirement, the commonest slip. D is total capitalised interest as a share of the
60,000,000 envelope (Domain 6), a different denominator entirely.

**MCQ 14.1-D `[14.1.2 · Recall]`** Which condition to a drawing is a *judgment* rather than a
verifiable fact?

- A. the drawing is within the availability period
- B. the requested amount is within the undrawn commitment
- C. no potential event of default is continuing, and the project remains in balance ✅
- D. the technical adviser's certificate is attached

*Rationale:* A, B and D are checkable against a calendar, a ledger and a document schedule. C
requires a forecast of remaining cost and a view on what constitutes a *potential* default — which
is why those two limbs are where the monitoring relationship is conducted (14.1.2).

**MCQ 14.1-E `[14.1.3 · Evaluation]`** The facility agreement's funding-order sub-clause is still
open. Equity-first capitalises **1,338,006** of interest against **2,114,597** pro rata and
**2,804,070** debt-first; at a fixed 60,000,000 envelope, equity-first re-solves the balancing
contingency from 3,645,403 to **4,388,050**. Deferring the equity cheque is worth **183,013** of
present value to the sponsor at 8 %. What should the transaction lead recommend to the sponsor's
board?
- A. debt-first — it is worth 183,013 of present value to the shareholders, which is the only party the board represents
- B. equity-first — it converts the 776,591 of interest saving into 742,647 of additional funded contingency, 9.14 % of the EPC price against 7.59 %, provided the sponsor can carry the larger irrecoverable exposure if the works are abandoned early ✅
- C. pro rata — it is neutral between the parties and is what the lender's draft provides
- D. debt-first — capitalised interest is not a cash cost during construction, so the order is presentational

*Rationale:* At a fixed envelope the sequencing buys funded protection with no additional money,
which is the form of the argument a credit committee and a board can both act on (14.1.3) — and the
condition attached to it is the real one, because equity-first maximises the sponsor's exposure to an
abandoned works. A is defensible and is what sponsors argue — the 183,013 is a genuine present-value
gain to the shareholders at the board's own 8 %, and it is already net of the extra 689,473 the
sponsor must fund. It is weaker for two reasons the recommendation has to state. It prices only the
timing of the sponsor's own cheque and puts nothing at all against the 742,647 of funded protection
the same decision forgoes, whose value shows up precisely in the state where the works overrun. And
its ground is wrong: a sponsor board that signs a funding order is also fixing the project's
irrecoverable exposure and the lender's, so shareholder present value is the decisive interest only
once the protection question has been answered. C accepts a settlement in place of an analysis. D is
false: capitalised interest is drawn, enters the debt balance and is repaid with interest over the
loan life.

**MCQ 14.1-F `[14.1.1 · Comprehension]`** Which statement best restates why the restated
sources-and-uses statement is a **test** where the financial-close statement is only an **identity**?
- A. the close statement was reviewed by the model auditor and the restated one is not
- B. at close one line is solved so that the columns agree, whereas at a data date the two columns are built from independent sources — the agent's records, certified progress and a commitment schedule — so a gap between them carries information ✅
- C. the restated statement replaces forecasts with actual costs
- D. the restated statement adds remaining contingency as a source, which the close statement omits

*Rationale:* An identity can always be satisfied by choosing the balancing line; a test can fail
because nothing is free to move (14.1.1). C is only partly true and misleads on the important part —
three of the four lines in Kestrel's remaining column are still forecasts. D describes the
double-count 14.1.1 exists to forbid: contingency is a *use* funded by the same commitment.

**MCQ 14.1-G `[14.1.2 · Evaluation]`** The quarter-five draw request must repeat the representation that
the project is **in balance**. The certified cost report supporting it shows the columns reconciling; the
finance team's own rolling funding forecast, issued internally the same week, shows a prospective
shortfall of **1,927,740** on a commitment basis. The soundest course is to:
- A. sign the request on the certified cost report, because a representation is made on certified
  information and a forecast is not certified
- B. not sign the in-balance representation on a basis the team knows to be superseded: disclose the
  forecast position to the agent, name the cure and the party funding it, and submit the request with the
  cure — because the cure is dated by the next draw and not by the reporting date ✅
- C. sign the request now and disclose the forecast position in next month's report, when the cost report
  will have caught up with it
- D. withhold the draw request altogether until the cost report has been reissued on the forecast basis

*Rationale:* "In balance" and "no potential default" are the two limbs of a draw request that are
judgments rather than facts, and both are certifications by named officers with consequences attached
(14.1.2); a certification cannot rest on a basis its signer knows to be out of date. A treats a
representation as a document-assembly step. C is the commonest course and puts the misstatement and the
delay together — the shortfall surfaces a month later, having consumed part of the time available to raise
the money (14.2.3). D avoids the misstatement by converting a disclosure problem into a schedule problem
at 415,000 a month of funded cost and 947,000 of economic cost (14.4.1); the shortfall is **4.6452 months**
of that funded cost, so the delay route can consume the whole amount in a quarter and a half without
curing any of it.

### Self-check — KA 14.1

1. *Why is a restated sources-and-uses statement a test where the close statement is an identity?*
   — At close one line balances by construction; at a data date the columns are built from
   independent sources (the agent's records, certified progress, a commitment schedule), so a gap
   is information rather than a plug.
2. *State the double-count that flatters most in-balance tests.* — Adding remaining contingency to
   undrawn debt plus uncalled equity; contingency is a use funded by that same commitment.
3. *Who benefits from each funding order?* — Equity-first: the project (lowest capitalised
   interest, 1,338,006) and the lender (sponsor money in first). Debt-first: the sponsor's present
   value (+183,013) at the project's cost (+689,473). Pro rata: neither, which is why it is the
   common settlement.

---

## Knowledge Area 14.2 — Cost-to-complete and contingency draw

*Topics: 14.2.1 the lender's cost-to-complete, and the reconciliation to `EAC` · 14.2.2 contingency
draw and coverage on the remainder · 14.2.3 the in-balance condition and the cash call.*

### 14.2.1 The lender's cost-to-complete, and the reconciliation to `EAC`

**Definition.** A **lender's cost-to-complete** is the money still required to reach the commercial
operations date, built on a **commitment** basis: remaining contractual entitlement under signed
contracts, plus approved variations not yet certified, plus the assessed exposure on notified but
unagreed claims, plus a bottom-up re-estimate of owner-retained scope, plus the financing costs —
interest, fees, premiums — that will accrue between the data date and completion. It is not
`EAC − AC`, and the difference is not an error in either number.

Domain 8 (KA 8.4.1) established the bridge in principle: a project controller's report ends at an
`EAC` column and an honest range; a lender reads the same figures and produces a date and an
amount. PML-AI Domain 7 (KA 7.3.3) supplies the `EAC` family — remaining work at the budgeted rate,
`BAC/CPI`, the `CPI × SPI` variant, and bottom-up re-estimation. This topic supplies the missing
piece: **why the two disciplines diverge systematically on a fixed-price contract, and how to
reconcile them line by line so that the divergence becomes evidence instead of an argument.**

The structural reason is the wrap. Under a fixed lump-sum contract certified against milestones,
the amount certified *is* the amount budgeted for the milestone, so on the EPC scope `EV` and `AC`
are identically equal and **`CPI` on that scope is 1.000 by construction.** Cost variance on the
largest single line of the project is therefore structurally invisible to earned value; the risk
shows up instead as *schedule*, as *claims*, and as *contractor solvency*. Meanwhile the
owner-retained scope — land, permits, the owner's engineering team, utility diversions, the items no
contractor would price — is where the SPV bears cost risk directly, and it is small enough to be
overlooked and volatile enough to be dangerous.

**Worked example 14.2.1 — Kestrel's two cost-to-completes at the quarter-five data date, and the
bridge between them.**

1. **Setup.** The construction control accounts carry `BAC` **51,600,000** (EPC 48,000,000 plus
   owner-retained 3,600,000; contingency and financing sit outside the control accounts). At the
   data date the EPC scope is **61 %** certified, so `EV` = `AC` = **29,280,000** on that scope. The
   owner-retained scope is **70 %** physically complete — `EV` **2,520,000** — at an actual cost of
   **4,200,000**, a scope `CPI` of **0.6000**. Off the control accounts: **465,403** of variations
   already certified, **840,000** of variations approved but not yet certified, and notified but
   unagreed contractor claims whose assessed exposure the technical adviser and the SPV's quantity
   surveyor put at **1,260,000**. A bottom-up re-estimate of the remaining owner-retained scope
   comes back at **2,400,000**. Remaining capitalised interest to completion is **1,436,674**
   (14.1.1). Compute both cost-to-completes and reconcile them.
2. **Formula.** EVM side: `CPI = EV/AC`; `EAC(a) = AC + (BAC − EV)`; `EAC(b) = BAC/CPI`;
   `EAC(d) = AC + bottom-up ETC`; `CTC = EAC − AC`. Lender side: `CTC = remaining committed contract
   value + approved variations + assessed claim exposure + bottom-up owner scope + remaining
   financing costs`.
3. **Substitution.** `CPI = 31,800,000/33,480,000`; `EAC(b) = 51,600,000 × 33,480,000/31,800,000`;
   `EAC(d) = 48,000,000 + 4,200,000 + 2,400,000`; lender side
   `18,720,000 + 840,000 + 1,260,000 + 2,400,000 + 1,436,674`.
4. **Result.** `CPI` **0.949821** blended, **1.0000** on EPC scope, **0.6000** on owner-retained
   scope.

   | Forecast | `EAC` (USD) | `CTC` = `EAC − AC` (USD) |
   |---|---|---|
   | (a) remaining work at the budgeted rate | 53,280,000 | 19,800,000 |
   | (b) `BAC/CPI` | 54,326,038 | 20,846,038 |
   | (d) bottom-up re-estimate | 54,600,000 | 21,120,000 |

   The reconciliation to the lender's number:

   | Bridge from the bottom-up `CTC` | USD |
   |---|---|
   | `CTC(d)` = remaining EPC 18,720,000 + remaining owner scope 2,400,000 | 21,120,000 |
   | + approved variations, outside `BAC` | 840,000 |
   | + assessed exposure on notified claims, outside `BAC` | 1,260,000 |
   | + remaining interest during construction, outside `BAC` | 1,436,674 |
   | **= lender's cost-to-complete** | **24,656,674** |

   Against available commitment of **22,728,934**, the project is **out of balance by
   USD 1,927,740**.
5. **Interpretation.** Four readings, in order of professional value. **The bridge is the whole
   lesson: the two numbers differ by items each discipline is structurally blind to.** Earned value
   does not see capitalised interest (it is not work), it does not see approved variations until
   they are baselined, and it does not see claim exposure at all — 3,536,674 of the lender's number
   is invisible to the cost report, which is 14.34 % of it. Conversely the lender's number does not
   see cost-performance trend on the fixed-price scope, because there is none to see. Neither number
   is wrong; a report that presents one without the other is. **Second, the blended `CPI` gets close
   to the right total for entirely the wrong reasons, and that is worse than being wrong.**
   `EAC(b)`'s uplift over `BAC` is 2,726,038, which sits plausibly between the 2,400,000 overrun
   implied by the owner-scope `CPI` and the 3,000,000 implied by the bottom-up. But of that uplift
   `48,000,000 × (1/0.949821 − 1) =` **2,535,849** is attributed to a fixed-price scope that cannot
   overrun, and only **190,189** to the scope that is actually overrunning by 2,400,000 to
   3,000,000. A reviewer who checks the total passes it; a reviewer who checks the composition finds
   that the forecast is right by coincidence and will stop being right the moment the mix changes.
   **Disaggregate before extrapolating: a `CPI` blended across scopes with different cost-risk
   ownership is an average of a constant and a variable, and it forecasts neither.** **Third, the
   `CPI × SPI` method has no honest home here.** It requires an `SPI`, and a milestone-certified
   fixed-price scope produces a lumpy, contractually-defined value curve that does not support one;
   the schedule question must be answered from the programme and the independent engineer's
   assessment (KA 14.4), not from an index. **Fourth, the 1,927,740 is a cash call, and its date is
   the next draw.** The forecast has stopped being a forecast. The commercial consequence is that
   **the choice of basis is a negotiation, not a technique** — the sponsor will argue that claims
   assessed at 1,260,000 are worth far less and that the bottom-up re-estimate is pessimistic, and
   both arguments may be right; what cannot survive is a report that omits the lines rather than
   arguing about them.

### 14.2.2 Contingency draw and coverage on the remainder

**Definition.** **Contingency coverage on the remainder** is the ratio of unallocated contingency
still available to the assessed cost of everything it must still cover:

```
Contingency coverage = remaining unallocated contingency
                       ÷ (known committed claims on contingency + P80 of the open risk register)
```

Domain 8 (KA 8.3) sized contingency at sanction and warned that the confidence level chosen is
usually unnamed. This topic runs the same test *during* construction, where the denominator has two
parts that behave completely differently, and where the standard health check is worthless.

The standard health check is the **draw rate**: contingency drawn as a percentage of contingency
funded, compared with percentage progress. Kestrel at the quarter-five data date has drawn
`(4,200,000 − 2,520,000) + 465,403 =` **2,145,403** of its 3,645,403, or **58.85 %**, against
**61.00 %** certified progress. That looks not merely acceptable but slightly better than
proportionate, and it is the number most monthly reports lead with. It is meaningless. **Contingency
is not consumed pro rata to progress, because risk is not distributed pro rata to progress** — some
risks retire early (ground conditions), some cluster at the end (commissioning, performance
testing, permit conditions), and a draw rate that tracks progress is a coincidence, not evidence.
The only test that means anything is coverage on what remains.

**Worked example 14.2.2 — Kestrel's contingency, tested twice.**

1. **Setup.** Contingency funded at close **3,645,403**. The sanction risk register carried six
   items: ground conditions at the intake structure (p 0.30, 2,600,000), a brine-outfall permit
   condition variation (0.25, 1,200,000), membrane supply escalation or substitution (0.35,
   900,000), commissioning re-test and extended supervision (0.35, 700,000), an owner-supplied
   power-connection delay (0.30, 600,000) and interface and utility diversions in owner-retained
   scope (0.40, 800,000). By the quarter-five data date the ground-conditions and interface risks
   have **materialised** (they are the owner-scope overrun and the certified variations), the
   membrane risk has been **retired**, and four items remain open — with the claims risk restated as
   "notified claims settle above the assessed exposure" (p 0.40, 900,000). Known committed claims on
   contingency are the 840,000 of approved variations, the 1,260,000 of assessed claim exposure and
   the **1,320,000** by which the bottom-up owner-scope re-estimate of 2,400,000 exceeds the
   1,080,000 of remaining budget. Compute coverage at close and at the data date.
2. **Formula.** Register mean = Σ `EMV` = Σ `p × impact`; variance = Σ `p(1 − p) × impact²`;
   P80 ≈ mean + 0.8416σ (the normal approximation of Domain 8, KA 8.3). Coverage = remaining
   contingency ÷ denominator.
3. **Substitution.** At close: mean `780,000 + 300,000 + 315,000 + 245,000 + 180,000 + 320,000`;
   variance `0.21×2,600,000² + 0.1875×1,200,000² + 0.2275×900,000² + 0.2275×700,000² +
   0.21×600,000² + 0.24×800,000²`. At the data date: mean
   `360,000 + 300,000 + 245,000 + 180,000`; denominator `3,420,000 + P80`.
4. **Result.**

   | Test | Contingency available (USD) | Denominator (USD) | Coverage |
   |---|---|---|---|
   | At financial close, against the sanction register P80 | 3,645,403 | 3,392,416 | **1.0746** |
   | At the data date, open risk only, on the mean | 1,500,000 | 1,085,000 | **1.3825** |
   | At the data date, open risk only, at P80 | 1,500,000 | 1,764,289 | **0.8502** |
   | At the data date, known claims + open risk at P80 | 1,500,000 | 5,184,289 | **0.2893** |

   Sanction register: mean **2,140,000**, σ **1,488,136**, P80 **3,392,416** — so the funded
   contingency of 3,645,403 sat at `z = 1.0116`, about the **P84** of that register. Open register
   at the data date: mean **1,085,000**, σ **807,140**, P80 **1,764,289**. The shortfall on the
   honest test is **3,684,289**.
5. **Interpretation.** **The ratio a certifier reports depends entirely on what is admitted to the
   denominator, and the four answers span 1.38 to 0.29 on identical facts.** That is not a technical
   quibble; it is the reason contingency certification must be a defined procedure rather than a
   judgment. The 1.3825 version — remaining contingency against the *mean* of open risk — is the one
   that appears most often in practice, because it is the easiest to compute and the most
   comfortable to sign; it fails because contingency exists precisely to cover the tail, and
   covering the mean is by construction a coin-flip. The 0.8502 version is defensible arithmetic and
   still wrong here, because it excludes 3,420,000 of claims on contingency that are not risks at
   all — they are approved, assessed or re-estimated amounts. **The honest denominator adds what is
   already known to what might still happen.** At 0.2893 the correct professional conclusion is not
   "coverage is thin" but "contingency has been fully committed and the project requires additional
   funding" — which is the in-balance conclusion of 14.2.1 arrived at by a completely independent
   route, and the agreement between the two is a check worth running deliberately. **Second, the
   collapse from 1.0746 to 0.2893 happened without a single surprise.** Every item that materialised
   was on the sanction register at sanction, with a probability and an impact; ground conditions at
   an intake structure ran at p 0.30 and it happened. Nothing unlikely occurred. What went wrong was
   the *confidence level*: 3,645,403 was a P84 provision against the *whole* register, and once the
   two largest items materialised the remainder was funded at a level nobody had named — Domain 8's
   point, now with a date attached. **Third, and most practically: the draw-rate check must be
   struck from the report.** 58.85 % drawn at 61 % progress is the sentence that let two quarters
   pass without a funding conversation. Replace it with the coverage ratio, its denominator itemised,
   and the shortfall in currency.

### 14.2.3 The in-balance condition and the cash call

**Definition.** The **in-balance condition** is a term of the facility requiring that, at each
drawdown and at each reporting date, the funds available be at least the cost to complete on the
basis the documents specify. Failure is a condition failure, not a default: the consequence is that
**the facility does not fund** until the position is cured. Curing is a funding event, and the
documents will name its permitted forms.

Four cures, in descending order of how much they cost a sponsor. **Additional equity** — the base
case, and the one every other cure is measured against. **Cost-overrun support** (Domain 5,
KA 5.2.3): a pre-agreed, capped sponsor undertaking to fund overruns, which converts a negotiation
into a drawing on a facility the sponsor has already granted. A **standby or contingent facility**:
committed debt available only on the in-balance test, which is cheaper than equity for the sponsor
and priced accordingly by lenders, and which raises gearing exactly when the project can least
support it. And **reallocation** — moving a funded but unspent line (usually a residual owner's-cost
or fee allocation) to the deficient line, which requires lender consent and is the cure that most
often turns out on inspection to be a re-labelling of contingency that has already been committed.

Two mechanical points a leader must own. **The order of the tests matters.** A project can be in
balance on the model and out of balance on the commitments (14.1.1 against 14.2.1); it can pass the
in-balance test and fail the contingency-adequacy test, or the reverse. Facilities that condition
drawing on one test and reporting on another produce exactly the gap Case study A exploits. And
**the cure is dated by the next draw, not by the reporting date.** A cost report issued in the month
after a data date has already consumed part of the time available to raise money; the practical
consequence is that the sponsor's treasury must be told about a prospective shortfall from the
*forecast*, not from the report — which is what the rolling funding forecast of 14.1.2 is for.

### AI in this KA

**Where it earns its place.** Three tasks, all high-volume and all currently done badly. **Claims
and variation registers**: extracting notified claims from correspondence, matching each to a
contractual head, a quantum claimed and an assessed exposure, and keeping the register reconciled to
the cost report — the register is usually maintained in three incompatible places, and a machine that
reconciles them earns its keep in a month. **Disaggregated `CPI` reporting**: computing performance
indices separately for fixed-price and owner-retained scope and flagging any report that blends
them, which is the defect of 14.2.1 and is trivially detectable. **Register roll-forward**:
tracking which sanction-register items have materialised, retired or been restated, and recomputing
the coverage ratio on the remainder at every data date with the denominator itemised.

**Where it must not go.** An assessed claim exposure is a **legal and commercial judgment** about
entitlement under a specific contract in a specific jurisdiction, and it must not be produced by a
model. The same applies to the certification that contingency remaining is adequate for risk
remaining: that is a signed opinion, and the arithmetic supporting it is an input to the opinion,
not a substitute for it. Nor may an assistant select the denominator — the choice between 1,085,000,
1,764,289 and 5,184,289 is the whole content of the test, and a tool that picks the flattering one
because it was the last one prompted for has industrialised the failure this topic exists to
prevent.

**Verification, concretely.** Recompute the register mean and P80 by hand for two items and confirm
the aggregation; require that the denominator be printed with its components, never as a single
figure. Reconcile the in-balance shortfall against the contingency shortfall — the two routes should
tell a consistent story, and Kestrel's do (1,927,740 of in-balance shortfall against contingency
committed 1,920,000 beyond its balance, plus the 7,740 of early-draw interest). Require the bridge
of 14.2.1 to be presented explicitly whenever an `EAC` and a lender's cost-to-complete appear in the
same pack, with every bridge line attributed to the discipline that is blind to it.

### Key terms — KA 14.2

| Term | Meaning |
|---|---|
| **Lender's cost-to-complete** | Money to completion on a commitment basis: remaining contract value + approved variations + assessed claim exposure + bottom-up owner scope + remaining financing costs. |
| **In-balance condition** | Facility term requiring available funds ≥ cost to complete; failure stops funding rather than causing default. |
| **Assessed claim exposure** | The SPV's and adviser's judgment of the probable settled cost of notified but unagreed claims. |
| **Blended `CPI` artefact** | A performance index averaged across fixed-price and owner-retained scope; misattributes an overrun and forecasts neither scope. |
| **Contingency coverage on the remainder** | Unallocated contingency ÷ (known committed claims + P80 of the open register). |
| **Draw-rate check** | Contingency drawn as a share of contingency funded, against progress; coincidental, and to be struck from reports. |
| **Cost-overrun support** | A capped sponsor undertaking to fund overruns, converting a negotiation into a drawing. |

### Sample MCQs — KA 14.2

**MCQ 14.2-A `[14.2.1 · Application]`** At a data date the remaining committed EPC value is
18,720,000, approved but uncertified variations are 840,000, assessed claim exposure is 1,260,000,
the bottom-up remaining owner-retained scope is 2,400,000 and remaining capitalised interest is
1,436,674. The lender's cost-to-complete is:

- A. USD 21,120,000
- B. USD 24,656,674 ✅
- C. USD 23,220,000
- D. USD 20,846,038

*Rationale:* All five lines sum to 24,656,674. A is the bottom-up `CTC(d)`, which omits the three
lines earned value cannot see. C omits remaining capitalised interest — the single most commonly
dropped line, because it is not work. D is `CTC` on the `BAC/CPI` method, a different basis
entirely.

**MCQ 14.2-B `[14.2.1 · Analysis]`** A project's blended `CPI` is 0.949821 across a fixed-price EPC
scope of 48,000,000 (certified against milestones) and owner-retained scope of 3,600,000 running at
a scope `CPI` of 0.60. Applying `BAC/CPI` to the blended index:

- A. is correct, since `BAC/CPI` is the standard persistence forecast
- B. attributes 2,535,849 of uplift to a scope that cannot overrun and only 190,189 to the scope that is overrunning, so the total is right only by coincidence ✅
- C. understates the forecast, because fixed-price scope carries the greater risk
- D. is invalid because `CPI` cannot be computed on a milestone-certified contract

*Rationale:* The composition of the 2,726,038 uplift is the defect (14.2.1). A treats a method as
valid irrespective of the scope mix it is applied to. C inverts the risk ownership under a wrap. D
overstates: `CPI` on that scope is computable and equals 1.000 by construction — which is exactly
why blending destroys the signal.

**MCQ 14.2-C `[14.2.2 · Analysis]`** Remaining unallocated contingency is 1,500,000. Known
committed claims on contingency total 3,420,000 and the open risk register has a mean of 1,085,000
and a P80 of 1,764,289. The defensible coverage ratio on the remainder is:

- A. 1.3825
- B. 0.8502
- C. 0.2893 ✅
- D. 0.4386

*Rationale:* `1,500,000 / (3,420,000 + 1,764,289) = 0.2893`. A tests contingency against the *mean*
of open risk only — covering the mean is a coin-flip and it also ignores the 3,420,000 already
committed. B is the P80 test on open risk only, which is defensible arithmetic on the wrong
denominator. D divides by the 3,420,000 of known committed claims alone
(`1,500,000/3,420,000`), omitting the open register entirely.

**MCQ 14.2-D `[14.2.2 · Analysis]`** A monthly report states that 58.85 % of contingency has been
drawn at 61.00 % certified progress and concludes that contingency consumption is "in line". The
correct professional response is:

- A. accept it; the draw rate is below progress, so consumption is favourable
- B. reject the inference — contingency is not consumed pro rata to progress, and the only meaningful test is coverage on the remainder ✅
- C. accept it if the draw rate has tracked progress for three consecutive periods
- D. recompute the draw rate on physical rather than certified progress

*Rationale:* The draw rate is coincidental because risk is not uniformly distributed through a
programme (14.2.2); here the honest coverage on the remainder is 0.2893. C makes a trend out of a
coincidence. D refines a measure that should not be relied on at all.

**MCQ 14.2-E `[14.2.2 · Evaluation]`** A certifier reports contingency coverage on the remainder of
**0.2893** — 1,500,000 against 3,420,000 of known committed claims plus a 1,764,289 P80 on the open
register — and concludes: "coverage is thin; we recommend monthly monitoring of the contingency
position." Should that conclusion be accepted?
- A. yes — 0.2893 is thin, and closer monitoring is the proportionate response to a thin ratio
- B. no — a ratio below one means contingency is already fully committed and 3,684,289 short, so the finding is a funding requirement, corroborated by the independently derived 1,927,740 in-balance shortfall; monitoring a known deficit does not fund it ✅
- C. no — the 3,420,000 of known claims does not belong in the denominator, and on open risk alone coverage is 0.8502
- D. yes, provided the ratio is recomputed at every data date with its denominator itemised

*Rationale:* `1,500,000 − 5,184,289 = −3,684,289`: the number does not describe a margin that is
narrow, it describes protection that has run out, and the in-balance route reaches the same
conclusion from different inputs (14.2.1, 14.2.2). A and D are the defensible-sounding responses and
both fail for the same reason — they answer a funding event with a reporting action. C is the
argument the sponsor will make and 14.2.2 disposes of it: approved variations, assessed claim
exposure and a bottom-up re-estimate are not risks that might happen, they are claims that have.

**MCQ 14.2-F `[14.2.1 · Comprehension]`** A project controller's cost report and the lender's
monitor produce cost-to-complete figures differing by 3,536,674 on identical underlying data. The
explanation a board should be given is:
- A. one of the two contains an error, and reconciliation will identify which
- B. each discipline is structurally blind to items the other counts — earned value cannot see capitalised interest, approved but unbaselined variations or claim exposure, while a commitment basis cannot see cost-performance trend on scope that is fixed-price ✅
- C. the monitor has applied a more conservative contingency allowance
- D. the difference is the unallocated contingency balance

*Rationale:* `840,000 + 1,260,000 + 1,436,674 = 3,536,674`, or 14.34 % of the lender's number, and
every line of it is outside the control accounts by construction rather than by anyone's error
(14.2.1). A is the instinct the bridge exists to correct. C invents a judgment difference where the
difference is one of scope. D names a line that appears in neither cost-to-complete.

**MCQ 14.2-G `[14.2.1 · Evaluation]`** The lender's cost-to-complete of **24,656,674** exceeds available
commitment of **22,728,934** by **1,927,740**. The sponsor disputes two lines: the **1,260,000** of
assessed exposure on notified but unagreed claims ("worth far less, and unagreed in any case") and the
**2,400,000** bottom-up re-estimate of remaining owner-retained scope ("pessimistic"). Both arguments may
have merit. The soundest treatment in the monitoring report is to:
- A. adopt the sponsor's figures, since the SPV knows its own contracts and its own scope better than the
  monitor does
- B. hold both lines at the assessor's figures, name the assessor, and record the sponsor's case and its
  basis beside them — because the disagreement is about the quantum of an exposure rather than about
  whether it exists, and a report that omits a line instead of arguing about it cannot be relied on by
  anyone ✅
- C. exclude the claim exposure until the claims are agreed, since an unagreed claim is not yet a liability,
  and hold the owner-scope line
- D. publish two cost-to-completes, the monitor's and the sponsor's, and let the lenders choose between them

*Rationale:* The choice of basis is a negotiation rather than a technique, and both sponsor arguments may
be right — what cannot survive is a report that drops the lines rather than stating them with their basis
and their assessor (14.2.1). C is the accrual instinct, and it is arithmetically insufficient as well:
removing the 1,260,000 closes **65.36 %** of the gap and still leaves the project **667,740** out of
balance. A hands the assessment to the party that will fund the cure. D looks even-handed and evades the
purpose of a monitoring report, which is a number a drawing can be conditioned on; two numbers and no
recommendation is the same omission as one number with a line missing.

### Self-check — KA 14.2

1. *Name the three lines a lender's cost-to-complete contains that an `EAC` cannot see.* — Approved
   variations not yet baselined, assessed claim exposure, and remaining capitalised interest and
   fees: 3,536,674 of Kestrel's 24,656,674.
2. *Why is `CPI` on a milestone-certified fixed-price scope identically 1.000, and what follows?* —
   Certified value equals budgeted milestone value, so `EV` = `AC`; cost risk on that scope shows up
   as schedule, claims and contractor solvency, and blending the index with owner-retained scope
   destroys the only real signal.
3. *State Kestrel's contingency coverage on the remainder and its denominator.* — 0.2893:
   1,500,000 against 3,420,000 of known committed claims plus a 1,764,289 P80 on the open register.

---

## Knowledge Area 14.3 — Progress certification and change control

*Topics: 14.3.1 certification bases, and what each one funds · 14.3.2 advance payment, retention and
the cash chain · 14.3.3 change control and the marginal coverage of a variation.*

### 14.3.1 Certification bases, and what each one funds

**Definition.** **Progress certification** is the independent determination of the value of work
executed in a period, and it is the hinge of the whole drawdown mechanic: it converts physical
progress into a payment entitlement and a funding requirement. Three bases are in general use and
they are not interchangeable. **Milestone certification** values only milestones actually achieved,
each with a defined pre-agreed value — binary, lumpy, and the only basis that ties directly to a
contractual entitlement. **Measured or percentage-of-completion certification** (`PoC`) values work
in place on measured quantities or an assessed completion percentage of each activity — smoother,
and requiring judgment. **Cost-incurred certification** values what the contractor has spent,
including materials procured but not yet delivered to site — smoothest of all, and the basis on
which the SPV's money leaves the country.

Who certifies matters as much as how. In a limited-recourse financing the certifier is usually an
**independent engineer** or the **lender's technical adviser**, appointed under a deed the SPV
cannot vary alone, and the certificate is a condition to each drawing. The certifier's independence
is the structural protection; the certification *basis* is the structural exposure.

**Worked example 14.3.1 — one quarter's work, valued three ways.**

1. **Setup.** In construction quarter six, Kestrel's EPC contractor executes work whose scheduled
   milestone value for the quarter is **7,200,000**. Milestones actually achieved total
   **5,760,000**; one further milestone worth **1,440,000** is assessed by the independent engineer
   as **92 %** physically complete but not achieved. The contractor has additionally procured
   **690,000** of membrane racks, paid for and stored at the vendor's works, not delivered. Retention
   is **5 %** of certified value. Gearing 70/30, quarterly interest 1.5 %, two quarters remain.
   Value the quarter on each basis and quantify the exposure the choice creates.
2. **Formula.** Milestone basis = Σ achieved milestone values. Measured basis = Σ achieved +
   `PoC` × value of the incomplete milestone. Cost-incurred basis = measured + off-site materials.
   Net for payment = certified × (1 − retention). Debt advanced = net × gearing. Over-certification
   = (cost-incurred − milestone) × (1 − retention).
3. **Substitution.** `5,760,000`; `5,760,000 + 0.92 × 1,440,000 = 7,084,800`;
   `7,084,800 + 690,000 = 7,774,800`; each `× 0.95`; each `× 0.70`.
4. **Result.**

   | Basis | Certified value (USD) | Net of 5 % retention (USD) | Senior debt advanced (USD) |
   |---|---|---|---|
   | Milestone | 5,760,000 | 5,472,000 | 3,830,400 |
   | Measured (`PoC`) | 7,084,800 | 6,730,560 | 4,711,392 |
   | Cost-incurred | 7,774,800 | 7,386,060 | 5,170,242 |

   The spread on the same quarter's work is **2,014,800** of certified value — **34.98 %** of the
   milestone figure. Relative to the milestone basis, the cost-incurred basis releases **1,914,060**
   net, of which **1,339,842** is senior debt, and accelerates that requirement by up to two
   quarters at a cost of `1,339,842 × 0.015 × 2 =` **40,195** of additional capitalised interest.
5. **Interpretation.** **The certification basis is a credit decision disguised as an accounting
   convention, and its magnitude is a third of a quarter's draw.** Start with what each basis buys.
   Milestone certification is the most conservative and the only one whose numbers are contractual:
   92 % of a milestone is worth exactly nothing, which is harsh on a contractor's cash flow and
   perfectly aligned with the lender's security, because an unachieved milestone is an unfinished
   thing. Measured certification is fairer to the contractor and introduces judgment — the 92 % is
   an opinion, and opinions drift upward under schedule pressure. Cost-incurred certification funds
   the contractor's balance sheet, and it is the basis on which lenders lose money. **The 1,339,842
   of senior debt advanced against work not in place is 27.91 % of the 4,800,000 performance bond**,
   and it is unsecured in substance unless three things are true: the SPV holds a **vesting
   certificate** or equivalent transferring title in the off-site materials; the materials are
   **identified and segregated** at the vendor's works; and they are **insured in the SPV's name**
   against loss and against the vendor's insolvency. Absent any one of those, the money has bought a
   claim in someone else's liquidation — which is precisely what Case study B costs. **Second, the
   basis must be stated once and held.** Drift is the real risk: a project that begins on milestones
   and slides toward cost-incurred as the contractor tightens is not making a series of small
   accommodations, it is progressively converting secured lending into unsecured lending, one
   certificate at a time, and nobody signs the decision. The controls are simple and worth insisting
   on: the basis is defined in the facility and the certifier's deed; any off-site materials
   certification is separately identified, capped as a percentage of the contract price, and
   conditioned on vesting, segregation and insurance; and the cumulative off-site balance is a
   reported line. **Third, note the small number.** The 40,195 of extra capitalised interest is real
   but trivial beside the 1,339,842 of exposure — which is the right lesson about certification: it
   is not a cost question, it is a security question, and pricing it as a cost question is how it
   gets conceded.

### 14.3.2 Advance payment, retention and the cash chain

**Definitions.** An **advance payment** (or mobilisation payment) is a sum paid to the contractor at
or shortly after contract signature, secured by an **advance payment bond** and recovered by
deduction from subsequent certifications. **Retention** is a percentage withheld from each
certification, usually capped as a percentage of the contract price, released in tranches at
completion and at the end of the defects liability period. Both are timing mechanisms, and both have
a price a project finance leader must be able to state.

**Worked example 14.3.2 — what Kestrel's payment terms cost in capitalised interest.**

1. **Setup.** The base case is Domain 6's funding profile with certified spend paid as it arises. The
   alternative applies the actual contract terms: an advance payment of **10 %** of the 48,000,000
   EPC price — **4,800,000** — paid at financial close and recovered pro rata against each
   certification, and retention of **5 %** of each EPC certification capped at **2.5 %** of the
   contract price, being **1,200,000**, released half at the commercial operations date and half
   twelve months later. Interest at 1.5 % per quarter on opening drawn balances, gearing 70/30.
   Compute the effect of each term and of both together on capitalised interest.
2. **Formula.** Re-run the quarterly funding model with the period requirement adjusted:
   `requirement = certified spend − advance recovery − retention withheld + accrued interest`,
   with the advance added to the close-date requirement. Compare total capitalised interest.
3. **Substitution.** Close requirement `2,640,000 + 4,800,000`; quarterly recovery
   `4,800,000 × profile percentage`; retention `min(0.05 × EPC certified, remaining cap)`.
4. **Result.**

   | Payment terms | Capitalised interest (USD) | Change vs base (USD) |
   |---|---|---|
   | Base: certified spend paid as it arises | 2,114,597 | — |
   | Advance payment only (4,800,000 at close) | 2,369,194 | **+254,597** |
   | Retention only (1,200,000 withheld to completion) | 2,052,008 | **−62,589** |
   | Both terms, as contracted | 2,306,605 | **+192,008** |

   The advance costs **254,597** of capitalised interest — **5.30 %** of the advance itself, and
   **0.53 %** of the EPC contract price. Retention saves **62,589**, one quarter of what the advance
   costs.
5. **Interpretation.** **An advance payment is a loan from the project to the contractor, and
   254,597 is its price.** That reframing is the whole content of the topic, and it changes the
   negotiation. The SPV borrows at 6.0 % to fund the advance; the contractor, whose alternative is
   to finance its own mobilisation, borrows at whatever its own credit commands — typically more. If
   the contractor's marginal cost of funds exceeds the project's, the advance is genuinely
   value-creating for the transaction *as a whole*, and the correct professional position is
   therefore not "refuse the advance" but **"price it"**: a contractor unwilling to reduce the
   contract price by at least 254,597 — 0.53 % — in exchange for a 4,800,000 advance is being lent
   money for nothing. That is a computation the contractor can check and argue about honestly, which
   is what makes it a negotiating position rather than a posture. **Second, retention is worth far
   less than sponsors assume, in both directions.** 62,589 of interest saving over a two-year build
   is immaterial; retention's value is entirely as security for defect rectification, and it should
   be argued on that basis, against the alternative of a retention bond, which releases the cash to
   the contractor and substitutes a bank's credit for the withholding. **Third, and much the most
   dangerous: the second retention tranche falls after the commercial operations date.** Kestrel's
   600,000 becomes payable twelve months after completion, which is inside the first operating year
   and outside any plausible availability period. If it is paid from operating cash, `CFADS` falls to
   `6,384,000 − 600,000 =` **5,784,000** and the `DSCR` at the first test falls to **1.1546** —
   **below the 1.20 covenant**. Domain 10 measured Kestrel's annual headroom at **372,438**; a
   600,000 retention release exceeds it by **227,562**. So a payment term agreed by a commercial
   team, in a contract, two years earlier, breaches a financial covenant. The remedies are all
   structural and all cheap if taken early: extend availability to cover retention releases, fund
   the releases into a dedicated retention account at completion, take a retention bond so no cash is
   withheld or released, or define `CFADS` to exclude construction-contract retention releases. What
   is not cheap is discovering the interaction in the first operating year, which is where it is
   normally discovered.

### 14.3.3 Change control and the marginal coverage of a variation

**Definition.** A **variation** (change order) alters the scope, price or time of a construction
contract. In a corporate setting a variation has two numbers, cost and time. In a limited-recourse
financing it has **four**: the *price*, the *time*, the **funding source** (which pocket pays — the
contractor's, contingency, additional debt, additional equity), and the **coverage consequence**
(what the variation does to the ratios the project is judged on). The third and fourth are the ones
change-control procedures usually omit, and they are the ones that cannot be fixed later.

The test that connects them is the **marginal coverage of a variation**: whether the incremental
`CFADS` a variation generates supports the incremental debt service its funding creates.

```
Marginal DSCR = Δ CFADS ÷ Δ debt service,   where Δ debt service = debt-funded cost ÷ AF(r, n)

Maximum coverage-neutral debt funding = (Δ CFADS ÷ target DSCR) × AF(r, n)
```

**Worked example 14.3.3 — the variation that creates value and destroys coverage.**

1. **Setup.** Kestrel's operator proposes an additional membrane train: capital cost
   **1,850,000**, adding **240,000** per year of `CFADS` for the project's remaining 25-year
   operating life. The loan runs 12 years at 6.0 % (`AF(0.06, 12) = 8.383844`); the board's
   appraisal rate is 8.0 % (`AF(0.08, 25) = 10.674776`); base `DSCR` is 1.2743 and the covenant is
   1.20×. Assess the variation on value and on coverage, and determine how it should be funded.
2. **Formula.** `NPV = ΔCFADS × AF(0.08, 25) − cost`; `PI = PV/cost`;
   `Δ debt service = debt-funded amount ÷ AF(0.06, 12)`;
   `marginal DSCR = ΔCFADS ÷ Δ debt service`; maximum coverage-neutral debt funding as above.
3. **Substitution.** `240,000 × 10.674776 = 2,561,946`; `− 1,850,000`;
   `1,850,000 / 8.383844 = 220,663`; `240,000 / 220,663`;
   `(240,000 / 1.274344) × 8.383844`.
4. **Result.** **`NPV` +USD 711,946** and **`PI` 1.3848** — comfortably value-accretive on
   Domain 4's tests. Funded **entirely by debt**, incremental debt service is **220,663** and the
   **marginal `DSCR` is 1.0876** — below the 1.20 covenant and far below the 1.2743 base. Funded
   **70/30** like every other use, the debt share is 1,295,000, incremental debt service
   **154,464** and the marginal `DSCR` **1.5538** — accretive. The maximum debt funding that holds
   base coverage constant is `(240,000/1.274344) × 8.383844 =` **USD 1,578,947**, being **85.35 %**
   of the cost; the residual **271,053** must come from equity or contingency. To be
   coverage-neutral at full debt funding the variation would need **281,200** per year of `CFADS`,
   **41,200** more than it delivers.
5. **Interpretation.** **A variation can be value-accretive and coverage-dilutive at the same time,
   and the funding decision follows the coverage test, not the `NPV`.** That is the sentence to
   carry out of this topic. The reason the two tests disagree is structural: `NPV` discounts 25 years
   of incremental cash at 8 %, while `DSCR` compares one year of incremental cash against debt
   service compressed into 12 years at 6 %. A long-lived, modestly-returning addition therefore
   looks good on value and thin on coverage, every time — and the ratio that binds is the one in the
   facility agreement. **The general rule is worth memorising in the form that makes it usable:**
   a variation is coverage-neutral if its debt-funded share does not exceed `ΔCFADS/target DSCR ×
   AF(r, n)` — here 85.35 % of cost — so at Kestrel's 70 % gearing the variation is comfortably
   accretive and at 100 % debt funding it is not. **Most variations are silently funded at 100 %
   from whatever is undrawn, which is the funding order nobody chose.** Third, the aggregate effect
   is small and therefore easy to wave through: fully debt-funded, this variation moves the blended
   `DSCR` from 1.2743 to **1.2665**, the covenant cash trigger from 6,011,562 to **6,276,357** and
   annual headroom from 372,438 to **347,643** — a loss of **24,795** of headroom, 6.66 % of it. One
   variation is nothing. Eight are a covenant. **The control that works is a threshold in the
   change-control procedure**: every variation above a stated value carries a computed marginal
   `DSCR` and a named funding source before approval, and the cumulative headroom consumed by
   approved variations is a reported line — the financing analogue of PML-AI Domain 4's
   baseline-drift test. **The professional caution:** the 240,000 of incremental `CFADS` is a
   forecast produced by the party proposing the variation, and a marginal `DSCR` of 1.0876 is inside
   the error bar of most operating forecasts. Where the coverage test is close, the honest conclusion
   is that the variation should not be debt-funded at all.

### AI in this KA

**Where it earns its place.** Certification packs are large, repetitive and structured, which makes
them good machine territory: cross-checking claimed quantities against the measurement schedule and
the previous certificate; detecting **certification drift** by tracking, period by period, the share
of certified value attributable to achieved milestones, assessed percentages and off-site materials
— a trend no monthly report currently shows and which a machine can produce for nothing; verifying
that every off-site materials certification has a matching vesting certificate, segregation
confirmation and insurance endorsement, and listing the exceptions; and computing the four numbers
of a variation (price, time, funding source, marginal `DSCR`) at the point the variation is logged,
so that the coverage consequence exists before the approval rather than after it.

**Where it must not go.** A **certification is a professional opinion with liability attached** and
must not be generated, and an assessed percentage of completion must not be inferred from photographs
or progress narrative and presented as a measurement. Entitlement questions — whether a claimed
variation is a variation at all, whether the contractor bears the cost, whether a delay is
excusable — are contractual and jurisdictional and belong to the certifier and to counsel.

**Verification, concretely.** Recompute one certificate end to end from measured quantities to net
payment, including advance recovery and retention, and require agreement to the dollar; confirm the
retention cap has not been exceeded cumulatively. Reconcile cumulative certified value to cumulative
milestone value and separately identify the off-site balance. For every variation above the
threshold, recompute `Δ debt service` and the marginal `DSCR` by hand and check the funding source
recorded matches the funding actually drawn. Require that any tool computing a marginal `DSCR` print
the `AF(r, n)` it used and the target ratio it compared against — those two inputs carry the answer.

### Key terms — KA 14.3

| Term | Meaning |
|---|---|
| **Milestone certification** | Value of milestones actually achieved; binary, lumpy, tied to contractual entitlement. |
| **Measured / `PoC` certification** | Value of work in place on measured quantities or assessed completion; requires judgment. |
| **Cost-incurred certification** | Value of contractor spend including off-site materials; funds the contractor's balance sheet. |
| **Vesting certificate** | Instrument transferring title in off-site materials to the SPV; with segregation and insurance, the condition of any off-site certification. |
| **Certification drift** | Progressive migration from milestone toward cost-incurred certification; converts secured lending into unsecured lending without a decision. |
| **Advance payment / bond** | Pre-mobilisation payment recovered from certifications, secured by a bond; a loan from the project to the contractor. |
| **Retention / retention bond** | Percentage withheld from certifications, capped and released in tranches; a bond substitutes bank credit for the withholding. |
| **Marginal `DSCR` of a variation** | `ΔCFADS ÷ Δ debt service`; coverage-neutral debt funding = `(ΔCFADS/target DSCR) × AF(r, n)`. |

### Sample MCQs — KA 14.3

**MCQ 14.3-A `[14.3.1 · Application]`** A quarter's work comprises achieved milestones of 5,760,000
and one milestone worth 1,440,000 assessed at 92 % complete; the contractor has also procured
690,000 of off-site materials. Retention is 5 %. The senior debt advanced at 70 % gearing under a
cost-incurred basis exceeds that under a milestone basis by:

- A. USD 2,014,800
- B. USD 1,914,060
- C. USD 1,339,842 ✅
- D. USD 690,000

*Rationale:* `(7,774,800 − 5,760,000) × 0.95 × 0.70 = 1,339,842`. A is the gross certified spread,
before retention and before the gearing split. B applies retention but not gearing. D counts only
the off-site materials and omits the 92 % milestone.

**MCQ 14.3-B `[14.3.2 · Analysis]`** Kestrel's second retention tranche of 600,000 falls due twelve
months after the commercial operations date and is paid from operating cash. The consequence is:

- A. none; retention is part of the EPC price already funded
- B. `CFADS` falls to 5,784,000 and the `DSCR` to 1.1546, breaching the 1.20 covenant, because the release exceeds the 372,438 of annual headroom by 227,562 ✅
- C. the `DSCR` is unaffected, since retention is a balance-sheet movement
- D. the release is deducted from debt service, so coverage improves

*Rationale:* The release is a cash outflow in an operating period, and coverage is computed on cash
(Domain 10). A confuses being *funded* with being *available* — the availability period has ended.
C misstates a cash payment as an accrual. D reverses the direction of the effect.

**MCQ 14.3-C `[14.3.3 · Application]`** A variation costs 1,850,000 and adds 240,000 a year of
`CFADS`. The loan factor is `AF(0.06, 12) = 8.383844` and base `DSCR` is 1.2743. The maximum
debt-funded amount that leaves base coverage unchanged is closest to:

- A. USD 1,850,000
- B. USD 1,578,947 ✅
- C. USD 1,295,000
- D. USD 2,012,123

*Rationale:* `(240,000/1.274344) × 8.383844 = 1,578,947`, being 85.35 % of the cost. A is full debt
funding, which yields a marginal `DSCR` of 1.0876. C is the 70 % pro-rata debt share — inside the
limit but not the limit. D applies `AF` to the full `ΔCFADS` without the coverage divisor, the
sizing error of Domain 10, Exercise 10.1.

**MCQ 14.3-D `[14.3.3 · Analysis]`** A variation has an `NPV` of +711,946 at the board's 8 % rate
and a marginal `DSCR` of 1.0876 against a 1.20× covenant if fully debt-funded. The correct
conclusion is:

- A. approve and fund from the facility; a positive `NPV` is decisive
- B. reject; a marginal `DSCR` below the covenant disqualifies the variation
- C. approve, but cap debt funding at the coverage-neutral 1,578,947 and fund the residual 271,053 from equity or contingency ✅
- D. approve and renegotiate the covenant

*Rationale:* Value and coverage answer different questions and both bind; the funding mix is the
lever that satisfies both (14.3.3). A ignores the covenant. B discards 711,946 of value that a
funding decision can capture. D proposes to renegotiate a covenant over 271,053, which no lender
would price kindly.

**MCQ 14.3-E `[14.3.1 · Evaluation]`** The contractor asks for **690,000** of membrane racks, paid
for and stored at the vendor's works, to be certified on a cost-incurred basis. The SPV's finance
team supports the request, noting that the only consequence is a small amount of additional
capitalised interest. Reviewing the request, what should be challenged first?
- A. the additional capitalised interest, which the contractor rather than the SPV should bear
- B. whether vesting of title in the SPV, identification and segregation at the vendor's works, and insurance in the SPV's name are all in place — the certification advances 458,850 of senior debt against goods in a third party's possession, which is a security question, not a cost question ✅
- C. the independent engineer's 92 % assessment of the incomplete milestone, since an assessed percentage is an opinion that drifts upward under schedule pressure
- D. nothing — the materials are paid for, identifiable and represent real value to the project

*Rationale:* `690,000 × 0.95 × 0.70 = 458,850` of debt advanced against work not in place, and absent
vesting, segregation and insurance the money has bought a claim in someone else's liquidation
(14.3.1). A is the finance team's own framing and is the specific error the topic names: pricing a
security question as a cost question is how certification drift gets conceded, one certificate at a
time. C raises a real and separate concern about the measured basis, but it is not what *this* request
changes. D confuses the vendor's title with the SPV's. Whether a given vesting arrangement in fact
defeats the vendor's insolvency, and what has to be filed or registered for it to do so, is a
question of the law governing the goods and the contract on which this book states no jurisdiction's
position; the reviewer's job is to require the question be put to counsel before the certificate is
signed, not to answer it (14.3.1).

**MCQ 14.3-F `[14.3.2 · Evaluation]`** The contractor requests the contracted **10 %** advance payment —
**4,800,000** on the 48,000,000 price, secured by an advance payment bond — and declines any reduction in
the contract price. Re-running the funding model shows the advance costs **254,597** of capitalised
interest: **5.30 %** of the advance and **0.53 %** of the EPC price. The SPV's finance lead proposes
refusing the advance. The soundest position is to:
- A. refuse — an advance payment is a loan from the project to the contractor, and a project financing
  exists to fund the works rather than the contractor's balance sheet
- B. treat the 254,597 as the price of the advance and negotiate against it: where the contractor's own cost
  of funds exceeds the project's the advance creates value for the transaction as a whole, so the
  defensible outcomes are a price reduction of at least 0.53 %, a smaller advance, or an accepted and
  recorded cost — not a refusal on principle ✅
- C. accept — the advance payment bond covers the amount advanced, so the SPV carries neither cost nor
  exposure
- D. accept, and recover the 254,597 from the delay-damages account if the contractor completes late

*Rationale:* An advance is a priced loan, and pricing it converts a posture into a position the contractor
can check and argue about honestly (14.3.2). A is the reflex and forgoes a genuine joint gain wherever the
contractor's marginal cost of funds is the higher of the two. C confuses a bond, which secures **recovery**
if the advance is not earned, with the **carrying cost** of having advanced it — the 254,597 is incurred
whether or not the bond is ever called, and it is **4.068 times** the 62,589 that retention saves in the
other direction. D applies damages calibrated to delay against a cost caused by a payment term, and helps
itself to a cap Domain 12 reserves for a different head.

**MCQ 14.3-G `[14.3.1 · Comprehension]`** Setting the amounts aside, which statement best explains why
**milestone** certification is the basis most closely aligned with the lenders' security?
- A. it produces the smallest certified figure, so the least money leaves the account
- B. a milestone has either been achieved or has not, so certified value corresponds to a completed thing
  the security attaches to; a measured percentage is an opinion, and cost incurred may be goods in a third
  party's possession ✅
- C. it is the only basis an independent engineer is professionally qualified to certify
- D. it removes the need for retention, since nothing is certified until it is complete

*Rationale:* The three bases differ in **what they measure** — an achieved contractual event, an assessed
percentage, and the contractor's spend — and only the first is binary and contractually defined, which is
why 92 % of a milestone is worth exactly nothing and why that harshness *is* the alignment (14.3.1). A
states a consequence rather than the reason, and a smaller figure is not by itself a virtue. C is false:
certifiers apply all three, and the measured basis is the standard one on long linear works. D confuses
the certification basis with retention, which secures defect rectification and is withheld on any basis.

### Self-check — KA 14.3

1. *State the three conditions that make off-site materials certification defensible.* — Vesting of
   title in the SPV, identification and segregation at the vendor's works, and insurance in the
   SPV's name including against vendor insolvency.
2. *What does Kestrel's 10 % advance payment cost, and what is the negotiating consequence?* —
   254,597 of capitalised interest, 5.30 % of the advance and 0.53 % of the EPC price; the advance
   should be priced, not simply refused or granted.
3. *Give the coverage-neutral funding rule for a variation.* — Debt funding must not exceed
   `ΔCFADS/target DSCR × AF(r, n)` — for Kestrel's train, 1,578,947, or 85.35 % of cost.

---

## Knowledge Area 14.4 — Schedule delay, lender reporting and completion tests

*Topics: 14.4.1 the funded cost of a month of slip · 14.4.2 coverage at a calendar-fixed first
repayment date · 14.4.3 lender reporting and the completion tests.*

### 14.4.1 The funded cost of a month of slip

**Definition.** The **funded cost of delay** is the cash the project must raise, per unit of time of
slip, to keep paying while it is not yet earning. It is a strict subset of the **economic cost of
delay**, which adds the cash the project would have earned. The distinction sounds pedantic and is
worth millions: **the drawdown and in-balance tests see only the funded cost, and the liquidated
damages that under-recover the economic cost can substantially over-recover the funded one.**

Domain 5 (KA 5.4.2) priced a slip at the commercial operations date at **24,733.33 per day** — 7,000
of interest on a fully drawn 42,000,000 plus 17,733.33 of forgone `CFADS`, on a 30/360 basis — and
Domain 8 (KA 8.4.2) priced a slip declared *during* construction, where escalation on unbought
scope appears and forgone revenue is deferred rather than lost. This topic completes the family by
splitting the same event along a different axis: **not economic against contractual, but funded
against forgone.**

**Worked example 14.4.1 — a month of extension at Kestrel's commercial operations date.**

1. **Setup.** At the scheduled commercial operations date the facility is fully drawn at
   **42,000,000** at **6.0 %**. Prolongation costs the SPV bears directly: extended owner's team and
   site supervision **138,000** per month, extended construction all-risks and delay-in-start-up
   premium **46,000**, and independent engineer, facility agent and technical adviser monitoring fees
   **21,000**. Operating `CFADS` would have been **6,384,000** per year. Delay damages are
   **20,000 per day**, capped at **4,800,000** (Domain 12). The availability period runs to **six
   months** beyond the scheduled commercial operations date. Compute the funded and economic costs
   per month, the recovery each enjoys, and the exposure the mismatch creates.
2. **Formula.** Funded cost = interest on drawn debt + prolongation costs =
   `42,000,000 × 0.06/12 + 138,000 + 46,000 + 21,000`. Forgone `CFADS` = `6,384,000/12`. Full
   economic cost = funded cost + forgone `CFADS`. Recovery = damages ÷ cost. Months covered =
   damages cap ÷ monthly cost.
3. **Substitution.** `210,000 + 138,000 + 46,000 + 21,000`; `6,384,000/12 = 532,000`;
   `415,000 + 532,000`; `600,000/415,000`; `600,000/947,000`; `4,800,000/415,000`;
   `4,800,000/947,000`.
4. **Result.**

   | Per month of slip at the commercial operations date | USD | Damages recovery |
   |---|---|---|
   | Interest on drawn debt (42,000,000 at 6.0 %) | 210,000 | |
   | Extended owner's team and site supervision | 138,000 | |
   | Extended all-risks and delay-in-start-up premium | 46,000 | |
   | Independent engineer, agent and adviser fees | 21,000 | |
   | **Funded cost of delay** | **415,000** | **144.58 %** |
   | Forgone `CFADS` (6,384,000 ÷ 12) | 532,000 | |
   | **Full economic cost of delay** | **947,000** | **63.36 %** |
   | *Domain 5's basis (interest + forgone `CFADS` only)* | *742,000* | *80.86 %* |
   | Delay damages at 20,000 per day (30 days) | 600,000 | |

   Damages of 4,800,000 cover **11.5663 months** of funded cost but only **5.0686 months** of full
   economic cost. Over an eight-month slip — the day the cap binds — the funded cost is
   **3,320,000** against damages of 4,800,000, a **surplus of 1,480,000**, while the full economic
   cost is **7,576,000**, leaving **2,776,000** uncovered. The availability period funds only
   `6 × 415,000 =` **2,490,000** of extension cost.
5. **Interpretation.** **One damages rate, three coverage percentages — 144.58 %, 80.86 % and
   63.36 % — and which one you quote depends entirely on which cost basis you are defending.** That
   is the professional content, and it explains a pattern practitioners see and misdiagnose: a
   delayed project whose *drawdown tests keep passing* while equity value is being destroyed. The
   in-balance test measures funds against cost-to-complete, and forgone revenue is in neither
   column; a delay that over-recovers the funded cost therefore leaves the facility comfortable and
   the sponsor poorer, month after month, with no covenant to notice it. **The report must carry
   both numbers or it is not a report.** Second, note the two constraints that bind before the
   damages cap does, because they are the ones sponsors miss. **The availability period binds at six
   months, not eleven and a half**: beyond month six the facility no longer funds the extension and
   the sponsor pays 415,000 a month in cash even though damages will eventually reimburse it — a
   **timing** exposure of up to `(11.5663 − 6) × 415,000 =` about **2,310,000** that is invisible in
   a coverage table. And **damages are recovered later than the cost is incurred**, often much later
   and sometimes only after adjudication, so the sponsor is financing the contractor's breach in the
   meantime. Third, the composition is instructive: **prolongation costs the SPV bears directly —
   205,000 a month — are 49.4 % of the funded cost and are entirely absent from Domain 5's
   calibration basis.** A damages rate calibrated on interest plus forgone `CFADS` alone therefore
   understates the loss by those 205,000, and the correct calibration statement is that Kestrel's
   full monthly cost is 947,000, or **31,566.67 per day** — against which 20,000 recovers 63.36 %,
   not the 80.86 % Domain 5's narrower basis suggests. Both figures are honest; the negotiation
   should use the wider one, and say so.

### 14.4.2 Coverage at a calendar-fixed first repayment date

**The mechanic.** Facility agreements set repayment dates. Some set them by reference to the
**actual** commercial operations date; many set them by **calendar date**, fixed at financial close
with a cushion for expected slip. A calendar-fixed first repayment date has a consequence that
almost no construction report models: **a slip does not postpone the first instalment, it shortens
the operating period that funds it.** With `m` months of slip, the first annual test sees
`(12 − m)/12` of a year's `CFADS` against a full year's debt service.

**Worked example 14.4.2 — Kestrel's commercial operations date slips four months.**

1. **Setup.** Debt service **USD 5,009,635.23** falls due twelve months after the *scheduled*
   commercial operations date, by calendar date. Annual operating `CFADS` **6,384,000**, accruing
   evenly from the actual commercial operations date. The covenant is **1.20×**; the six-month debt
   service reserve holds **2,504,818**. Delay damages of **20,000 per day** are recoverable, capped
   at 4,800,000. Compute the `DSCR` at the first test for a four-month slip, the reserve
   consumption, and the slip at which each threshold is crossed.
2. **Formula.** `CFADS at first test = 6,384,000 × (12 − m)/12`; `DSCR = CFADS ÷ 5,009,635.23`.
   Breakevens solve `6,384,000 × (12 − m)/12 = k` for `k` equal to the covenant cash trigger
   (1.20 × debt service), to debt service, and to debt service less the reserve.
3. **Substitution.** `6,384,000 × 8/12 = 4,256,000`; `÷ 5,009,635.23`. Covenant:
   `12 × (1 − 6,011,562.28/6,384,000)`. Payment: `12 × (1 − 5,009,635.23/6,384,000)`. With reserve:
   `12 × (1 − (5,009,635.23 − 2,504,817.62)/6,384,000)`.
4. **Result.**

   | Slip (months) | `CFADS` at first test (USD) | `DSCR` | Shortfall vs debt service (USD) | Share of DSRA consumed |
   |---|---|---|---|---|
   | 0 | 6,384,000 | **1.2743** | — | — |
   | 1 | 5,852,000 | **1.1681** | — | — |
   | 2 | 5,320,000 | **1.0620** | — | — |
   | 3 | 4,788,000 | **0.9558** | 221,635 | 8.85 % |
   | 4 | 4,256,000 | **0.8496** | 753,635 | 30.09 % |
   | 6 | 3,192,000 | **0.6372** | 1,817,635 | 72.57 % |
   | 8 | 2,128,000 | **0.4248** | 2,881,635 | 115.04 % |

   Breakevens: the **1.20× covenant is breached beyond 0.7001 months — 21.00 days**; the project can
   no longer pay debt service from operating cash beyond **2.5834 months (77.50 days)**; and it can
   no longer pay from cash plus a fully funded reserve beyond **7.2917 months (218.75 days)**. If
   delay damages are creditable to `CFADS`, a four-month slip yields `20,000 × 120 =` **2,400,000**
   of damages, lifting `CFADS` to **6,656,000** and the `DSCR` to **1.3286** — **0.4791 higher**,
   and above the undelayed base case.
5. **Interpretation.** **Three weeks of slip breaches the covenant.** That is the result, and it is
   the single most under-modelled number in construction-phase finance. Nothing has gone wrong with
   the project: the plant works, the tariff is contracted, `CFADS` is running exactly at forecast on
   an annualised basis, and the covenant fails at the first test because the *test period* contains
   less operating time than the *obligation period*. Sponsors discover this at the first compliance
   certificate, which is the worst possible moment, and the discovery is unforced. **Second, whether
   delay damages count in `CFADS` is worth 0.4791 of coverage — more than the entire distance from
   the base case to the covenant.** Domain 10 established that `CFADS` is a defined term and that
   its documented construction governs every ratio built on it; this is that principle at its most
   valuable. Damages at 20,000 a day are 600,000 a month against forgone `CFADS` of 532,000, so they
   **over-compensate** the coverage effect of delay, and the sign of the delay's impact on the first
   test flips on a definitional clause. A leader who negotiates one thing in the `CFADS` definition
   should negotiate this. Note the tail carefully, though: once the damages cap binds at eight
   months the damages line stops growing while the `CFADS` line keeps shrinking, so the
   damages-credited coverage peaks at **1.3829** and then falls, crossing 1.20 at **9.7226 months**
   and 1.00 at **11.6059** — the cap converts a comfortable position into a cliff. **Third, the
   reserve is smaller protection than it looks.** A six-month debt service reserve does not survive
   six months of slip; it survives **7.2917 months** only because operating cash covers most of the
   obligation, and it is consumed 30.09 % by a four-month slip that also breaches the covenant and
   must then be replenished ahead of any distribution (Domain 10, KA 10.3.3). **Fourth, the fixes are
   all structural and all cheap at signing.** Set the first repayment date by reference to the actual
   commercial operations date with a long-stop; or annualise the first test on a look-forward basis;
   or size the reserve on the delay scenario rather than on a convention of six months; or provide
   expressly that delay damages and delay-in-start-up insurance proceeds are `CFADS`. Each is a
   drafting point worth more than the margin. **The professional caution:** none of these fixes
   should be pursued quietly as a technical amendment. A lender asked to move a repayment date late
   in a delayed project reads the request as news about the project, which is why the structure is
   negotiated before the delay and the disclosure discipline of Domain 10, KA 10.4.4 applies from the
   first month of slip.

> **Fig 14.4.1 — Coverage at a calendar-fixed first repayment date.** Line chart, x-axis months of
> slip in the commercial operations date (0–12), y-axis `DSCR` at the first test (0.00–1.45). Two
> series: **operating cash only**, falling linearly from **1.2743** through **0.8496** at four
> months to **0.4248** at eight; and **with delay damages of 20,000 a day credited to `CFADS`**,
> *rising* from 1.2743 to a peak of **1.3829** where the 4,800,000 cap binds at eight months, then
> collapsing to **0.9582** at twelve. Dashed horizontals at the **1.20** covenant (crimson) and at
> **1.00** where operating cash ceases to cover debt service. Crimson markers at the three
> breakevens on the cash-only line — **0.7001 months** (covenant breached), **2.5834 months** (cash
> no longer pays) and **7.2917 months** (cash plus the 2,504,818 reserve exhausted) — and blue
> markers at the cap and at **9.7226 months** where the damages-credited line re-crosses 1.20.
> Source: PCI original. Alt text: a falling line showing coverage at the first repayment date
> collapsing with each month of late completion and crossing the covenant threshold after only three
> weeks, against a second line that rises while liquidated damages accrue and then falls sharply
> once the damages cap is reached.

### 14.4.3 Lender reporting and the completion tests

**The reporting obligation.** Construction-phase information covenants are heavier than operating
ones because the security does not yet exist. The standard pack, monthly or quarterly: certified
progress against the programme, with the certifier's own commentary; cost incurred against the
sources and uses, restated per 14.1.1; the cost-to-complete and the in-balance statement (14.2.1);
the contingency position with coverage on the remainder (14.2.2); the variation and claims registers
with movements; the schedule position, critical path and forecast commercial operations date;
health, safety and environmental performance and any reportable incident; insurance status and
claims; and the compliance certificate signed by named officers. Two disciplines make the pack
useful rather than ceremonial: **the same numbers reconcile across sections** — the contingency
balance in the contingency section equals the contingency line in the restated sources and uses, and
a pack in which they differ has told the lender something the sponsor did not intend — and **the
forecast commercial operations date is stated as a single date with a stated confidence, not as "on
programme"**, because "on programme" is the phrase that precedes every delay notification.

**The completion sequence.** Completion is not one event, and conflating its stages is the commonest
drafting and reporting error of the phase. **Mechanical completion** confirms the plant is built and
can be safely energised. **Commissioning and reliability testing** demonstrates that it runs.
**Performance testing** demonstrates that it runs *to guarantee* — output, quality, efficiency,
consumption. **Provisional acceptance or the commercial operations date** is the contractual moment
from which revenue accrues and, in the financing, the moment the construction facility converts to
term debt, the operating covenant regime begins, the completion guarantees and cost-overrun support
fall away, the first retention tranche is released and the debt service reserve must be funded.
**Final acceptance** follows the defects liability period and releases the second retention tranche.
The financing attaches consequences to each, and the sponsor who has read only the EPC contract's
definitions will be surprised by the facility's.

**Conditional and partial completion.** Plants routinely complete late, or on time with a punch
list, or on time at less than guaranteed output. The first is KA 14.4.1 and 14.4.2. The second is
managed by valuing the punch list and withholding, which is arithmetic. The third is the interesting
case, because a permanent output shortfall is a permanent `CFADS` shortfall and therefore a
permanent coverage problem, remedied by **performance liquidated damages** or a **buy-down** — a
lump sum, usually applied to prepay debt, calibrated so that the financing survives a smaller plant
(Domain 12, KA 12.1.3).

**Worked example 14.4.3 — the buy-down that restores which coverage?**

1. **Setup.** Kestrel completes on time but the performance test settles at an operating `CFADS` of
   **5,900,000** rather than 6,384,000. Debt outstanding at completion **42,000,000**, `AF(0.06, 12)
   = 8.383844`, covenant **1.20×**, base `DSCR` **1.2743**, performance damages cap
   **4,800,000**. Compute the coverage as completed and the buy-down that restores (i) the covenant
   and (ii) the coverage the lenders originally priced.
2. **Formula.** `DSCR = CFADS ÷ (debt ÷ AF)`. Supportable debt at a target ratio =
   `(CFADS ÷ target) × AF`. Buy-down = 42,000,000 − supportable debt.
3. **Substitution.** `5,900,000 / (42,000,000/8.383844)`; `(5,900,000/1.20) × 8.383844`;
   `(5,900,000/1.274344) × 8.383844`.
4. **Result.** As completed the `DSCR` is **1.1777** — the **covenant is breached from the first
   test**. Restoring the 1.20× covenant requires debt of **41,220,566**, a buy-down of
   **779,434**. Restoring the 1.2743 base coverage requires debt of **38,815,789**, a buy-down of
   **3,184,211**. Both sit inside the 4,800,000 performance cap; the full cap would take debt to
   37,200,000 and coverage to **1.3297**.
5. **Interpretation.** **The buy-down that restores the covenant is a quarter of the buy-down that
   restores the coverage the lenders priced, and the difference — 2,404,777 — is settled by
   drafting, not by negotiation after the test.** That is the transferable lesson. A buy-down formula
   expressed as "the amount required to restore the `DSCR` to the covenant level" is worth 779,434
   here; one expressed as "to restore the base-case `DSCR` in the financial model at close" is worth
   3,184,211; and the two clauses look almost identical to a reader who is not computing them. The
   sponsor should also see what the difference costs *it*: at 41,220,566 of debt the project sits
   exactly on its covenant with zero headroom, so the first bad operating year breaches — a position
   no sponsor should accept in exchange for keeping 2,404,777 of contractor money that would
   otherwise have deleveraged the project. **Second, note that the buy-down is a prepayment, not
   compensation.** It repairs coverage by shrinking debt, so it repairs the *lender's* position
   directly and the sponsor's only through the coverage headroom it restores; the equity return
   still falls, because the plant is permanently smaller. **Third, the cap is the binding question
   again.** Here both buy-downs fit inside 4,800,000, but a 10 % shortfall rather than a 7.6 % one
   would not, and the residue would sit on equity exactly as Domain 12 computed for the delay head.
   The professional sequence at a failed performance test is therefore: compute the permanent
   `CFADS` effect; compute both buy-downs; read the clause to see which one you are entitled to;
   check it against the cap and against the aggregate cap after any delay claim; and only then open
   the negotiation.

### AI in this KA

**Where it earns its place.** The construction report pack is a reconciliation problem, and machines
reconcile. Cross-checking that every figure appearing in two sections of the pack agrees, and
listing the exceptions, is the single highest-value automation available in this knowledge area, and
it catches the defect that most embarrasses sponsors. Beyond that: recomputing the coverage-at-first-
repayment table (14.4.2) at every data date from the current forecast commercial operations date, so
that the covenant breakeven in *days of slip* is a live number rather than an annual discovery;
maintaining the delay-cost ledger with funded and economic columns and the cumulative damages
position against the cap and its binding day; and drafting the narrative sections of the pack from
the underlying registers, which reduces the drift between what the numbers say and what the
commentary says.

**Where it must not go.** A **forecast commercial operations date** is a schedule opinion belonging
to the certifier and the delivery organisation, and a model that produces one from trend data has
produced an input to that opinion, not a substitute for it. Whether a delay is excusable, whether an
extension of time is due, whether a completion test has been passed, and what remedies a failed test
entitles anyone to are contractual determinations for the certifier and counsel. And no machine
output may constitute the compliance certificate: that is signed.

**Verification, concretely.** Recompute the `DSCR` at the first repayment date by hand for the
current forecast slip and confirm the covenant breakeven in days; require that any tool reporting it
state whether delay damages have been treated as `CFADS` and cite the definition clause relied on.
Recompute the monthly funded and economic costs of delay independently and confirm the damages
recovery percentages against both. For a buy-down, recompute the supportable debt at both target
ratios and check which the clause specifies. Tie the cumulative damages position to the cap and its
binding day, and confirm nothing in the pack assumes recovery beyond it.

### Key terms — KA 14.4

| Term | Meaning |
|---|---|
| **Funded cost of delay** | Cash the project must raise per period of slip: interest on drawn debt plus prolongation costs. Excludes forgone `CFADS`. |
| **Economic cost of delay** | Funded cost plus forgone `CFADS`; the correct calibration basis for delay damages. |
| **Availability period** | The window in which the facility may be drawn; often binds on a delay long before the damages cap does. |
| **Calendar-fixed repayment date** | A first instalment set by date rather than by actual completion; a slip shortens the operating period that funds it. |
| **Mechanical completion / commissioning / performance test / commercial operations date / final acceptance** | The completion sequence; the financing attaches distinct consequences to each. |
| **Buy-down** | A lump sum, usually applied to prepay debt, calibrated so the financing survives a permanently smaller plant. |

### Sample MCQs — KA 14.4

**MCQ 14.4-A `[14.4.2 · Application]`** Kestrel's first instalment of 5,009,635.23 falls on a fixed
calendar date twelve months after the scheduled commercial operations date. `CFADS` accrues evenly
from actual completion at 6,384,000 a year. If completion slips four months, the `DSCR` at the first
test is:

- A. 1.2743
- B. 0.8496 ✅
- C. 1.1681
- D. 0.4248

*Rationale:* `6,384,000 × 8/12 = 4,256,000`; `÷ 5,009,635.23 = 0.8496`. A assumes the test date
moves with completion. C is a one-month slip. D is an eight-month slip.

**MCQ 14.4-B `[14.4.2 · Analysis]`** With a 1.20× covenant, debt service of 5,009,635.23 and
`CFADS` of 6,384,000 a year accruing from actual completion, the slip at which the first covenant
test fails is closest to:

- A. six months
- B. three weeks ✅
- C. two months
- D. it cannot fail, since annualised `CFADS` is unchanged

*Rationale:* `12 × (1 − 6,011,562.28/6,384,000) = 0.7001` months, 21.00 days. A and C overstate the
tolerance by an order of magnitude. D is the error the calculation exists to kill: the covenant is
tested on the *period*, not on an annualised run rate.

**MCQ 14.4-C `[14.4.1 · Analysis]`** Kestrel's funded cost of a month of slip at completion is
415,000 and its full economic cost 947,000; delay damages are 600,000 a month. The most useful
observation for a board is:

- A. damages recover 80.86 % of the cost of delay
- B. damages over-recover the funded cost by 44.58 % while recovering only 63.36 % of the economic cost, so the drawdown tests keep passing while equity value is destroyed ✅
- C. damages fully cover the delay, so no action is required
- D. the delay is cost-neutral because damages exceed the funded cost

*Rationale:* The two bases give 144.58 % and 63.36 % recovery, and only the second is a statement
about value (14.4.1). A quotes Domain 5's narrower basis, which omits the 205,000 of monthly
prolongation cost. C and D mistake a passing funding test for an absence of loss.

**MCQ 14.4-D `[14.4.3 · Application]`** A performance test settles `CFADS` at 5,900,000 against
debt of 42,000,000, `AF(0.06, 12) = 8.383844`. The buy-down required to restore the base-case
`DSCR` of 1.2743 is closest to:

- A. USD 779,434
- B. USD 3,184,211 ✅
- C. USD 4,800,000
- D. nil, since 1.1777 exceeds 1.00

*Rationale:* `42,000,000 − (5,900,000/1.274344) × 8.383844 = 3,184,211`. A restores only the 1.20×
covenant, leaving zero headroom. C is the performance damages cap, not the calibrated amount. D
confuses paying debt service with satisfying a covenant (Domain 10, KA 10.2.1).

**MCQ 14.4-E `[14.4.2 · Evaluation]`** The term sheet provides a calendar-fixed first repayment date.
On Kestrel's figures the 1.20× covenant fails beyond **0.7001 months** of slip, and the delivery team
regards a slip of up to four months as a live risk. Four amendments are available and the sponsor can
press for one. Which should be pressed first?
- A. size the debt service reserve on the four-month slip scenario rather than on the six-month convention
- B. set the first repayment date by reference to the actual commercial operations date, with a long-stop — it removes the mismatch between the test period and the obligation period instead of funding its consequences ✅
- C. rely on delay damages being creditable to `CFADS`, which at a four-month slip is worth 0.4791 of coverage and turns a breach into a 1.3286 ratio
- D. accept the calendar-fixed date and request a covenant waiver if the slip occurs

*Rationale:* The defect is that the first test period contains less operating time than the
obligation it is tested against, and only B removes it (14.4.2). A is defensible and cheap, but a
reserve pays an instalment — it does not raise `CFADS`, so the covenant still fails at 0.7001 months
and the reserve must then be replenished ahead of any distribution. C is genuinely valuable and worth
negotiating alongside B, but it is a protection that expires: once the 4,800,000 cap binds at eight
months the damages line stops growing while `CFADS` keeps shrinking, so the damages-credited ratio
peaks at 1.3829 and re-crosses 1.20 at 9.7226 months. D is the worst available course — a waiver
requested inside a delayed project is read as news about the project.

**MCQ 14.4-F `[14.4.1 · Evaluation]`** The works are a month late at the commercial operations date. The
contractor's negotiator argues that the contracted **20,000 a day** — **600,000** a month — is generous,
because it recovers **144.58 %** of the **415,000** the project must actually raise while it is not yet
earning. The soundest response is to:
- A. accept the point — a damages rate that over-recovers the cash the project must raise is by definition
  adequate, and the drawdown and in-balance tests confirm it
- B. reject the framing: the funded cost excludes the **532,000** a month of forgone `CFADS`, so against the
  full economic cost of **947,000** the same rate recovers **63.36 %** — the signature of a project whose
  funding tests keep passing while equity value drains, which is the loss a damages rate exists to
  compensate ✅
- C. reject the rate on the ground that a recovery above 100 % shows the sum to be a penalty rather than a
  genuine pre-estimate of loss
- D. quote **80.86 %**, the recovery on interest plus forgone `CFADS`, as the neutral middle figure between
  the two positions

*Rationale:* One rate produces three recovery percentages, and the choice of basis is the whole argument:
the economic cost is the correct calibration basis, while the drawdown and in-balance tests see only the
funded one, which is why a delayed project can pass every test while destroying value (14.4.1). A mistakes
a passing funding test for an absence of loss. C imports a legal conclusion the arithmetic cannot support —
whether and how a contractual damages provision is enforceable is a question of the law governing the
particular contract, on which this book states no jurisdiction's position, and a recovery above 100 % of
*one* cost basis says nothing about the character of the sum. D is honest arithmetic on Domain 5's narrower
basis and understates the monthly loss by the **205,000** of prolongation cost the SPV bears directly —
**49.4 %** of the funded cost — so it is a figure to disclose, not the figure to negotiate on.

**MCQ 14.4-G `[14.4.3 · Comprehension]`** Which statement best describes what a **buy-down** does and does
not do when a plant completes on time at less than guaranteed output?
- A. it compensates equity for the output permanently lost, restoring the return the sponsors priced
- B. it is a lump sum, usually applied to prepay debt and calibrated so the financing survives a permanently
  smaller plant — repairing the lenders' coverage directly and equity's only through the headroom it
  restores; the equity return still falls, because the plant is smaller ✅
- C. it is a reserve the contractor funds against the possibility of future underperformance
- D. it replaces the delay-damages head once the commercial operations date has passed

*Rationale:* A buy-down is a **prepayment**, not compensation: it shrinks the debt so that a smaller
`CFADS` still supports it, which is why the drafting question — restore the covenant level, or restore the
base-case coverage — is worth the difference between the two amounts (14.4.3). A is the misreading that
makes a buy-down look like a settlement of equity's claim. C describes a reserve, which is funded before an
event rather than paid after a failed test. D confuses two heads of damages, each with its own sub-cap
under one aggregate cap (Domain 12, KA 12.1.2).

### Self-check — KA 14.4

1. *Distinguish the funded and economic costs of a month of Kestrel's delay, with figures.* —
   Funded 415,000 (interest 210,000 plus 205,000 of prolongation); economic 947,000, adding 532,000
   of forgone `CFADS`. Damages of 600,000 recover 144.58 % and 63.36 % respectively.
2. *Why does a three-week slip breach a 1.20× covenant on an otherwise perfect project?* — A
   calendar-fixed first repayment date shortens the operating period funding a full year's debt
   service; `12 × (1 − 6,011,562/6,384,000) = 0.7001` months.
3. *What is the drafting difference between the two buy-downs, in money?* — 2,404,777: restoring the
   1.20× covenant costs 779,434, restoring the 1.2743 base coverage costs 3,184,211.

---

## Advanced topics — Domain 14

### 14.A.1 The availability period, long-stop dates and the commitment that lapses

An **availability period** is the window in which a facility may be drawn; a **long-stop date** is
the date by which completion must occur before defined consequences follow. Both are dates set at
close against a programme that has not yet started, and both are routinely set with a cushion
calibrated to optimism. The construction-phase consequence is asymmetric and often misunderstood:
**an expired availability period does not accelerate the loan, it simply stops funding** — the
undrawn commitment lapses and every remaining use becomes an equity obligation. For Kestrel, a
six-month cushion funds `6 × 415,000 =` **2,490,000** of extension cost against delay damages that
would cover **11.5663 months**, so the binding constraint on a delay is the availability period and
not the damages cap, and the exposure is a **timing** exposure of roughly 2,310,000 rather than a
loss. Three practices follow. Model the availability period against the P80 completion date, not the
programme date. Check that the availability period covers every use that falls *after* completion —
Kestrel's second retention tranche of 600,000 falls twelve months after the commercial operations
date and is outside any plausible window (KA 14.3.2). And treat an availability-period extension as
a *pre-agreed* mechanic where possible, because a request made during a delay is priced as news.

### 14.A.2 Certifying value while liability is disputed

The hardest recurring problem in construction monitoring is that certification and entitlement move
on different clocks. Work is in place, the SPV disputes that the contractor is entitled to be paid
for it, and the draw request must nonetheless state a number. Three conventions are in use and each
has a cost. **Certify and reserve**: the certifier values the work and the certificate records the
dispute, so cash moves and the argument continues — which protects the programme and funds a
position the SPV may later have to recover. **Withhold and escalate**: nothing is certified, which
protects the money and stops the site, converting a commercial dispute into a schedule loss at
415,000 a month of funded cost and 947,000 of economic cost. **Certify into escrow**: the funds are
drawn and held, which costs the interest and preserves both positions — the compromise, and the one
that requires the facility to permit a drawing into an account the SPV does not control. The finance
leader's contribution here is not legal but arithmetical: **price the three options before the
meeting**, because the ordinary failure mode is to withhold on principle for two months and discover
that principle cost 1,894,000 of economic value against a claim worth 1,260,000.

### 14.A.3 The reviewer's drawdown eye

Invariants to test on any construction monitoring pack. Drawn-to-date ties to the facility agent's
records, not to the model. Cumulative debt and equity percentages of commitment are equal under
pro-rata funding, and any divergence traces to a documented funding order. Available commitment is
undrawn debt plus uncalled equity and **excludes** contingency, which is a use. The restated
sources-and-uses columns reconcile, and any imbalance is explained in cash terms rather than
absorbed. Capitalised interest to date reproduces Domain 8's area rule from the profile, the rate
and the gearing, to the dollar; interest is computed on opening, not closing, balances. `CPI` is
reported **separately** for fixed-price and owner-retained scope, and equals 1.000 on
milestone-certified fixed-price scope; no blended index feeds a `BAC/CPI` forecast. The lender's
cost-to-complete contains remaining committed value, approved variations, assessed claim exposure,
a bottom-up owner-scope estimate **and** remaining financing costs, and reconciles to the `EAC`-based
`CTC` through an explicit bridge. Contingency coverage on the remainder is reported with its
denominator itemised, and the in-balance shortfall and the contingency shortfall tell a consistent
story. Certified value reconciles to milestone value with the off-site materials balance separately
identified, and every off-site amount has vesting, segregation and insurance. Retention held has not
exceeded its cap, and each release tranche has a funding source that is not operating cash. Every
variation above the threshold carries a marginal `DSCR` and a funding source, and cumulative
headroom consumed by variations is reported. The delay ledger shows funded **and** economic costs and
the cumulative damages position against the cap and its binding day. And the `DSCR` at the first
repayment date is computed from the *current forecast* completion date, with the covenant breakeven
stated in days and the treatment of delay damages in `CFADS` cited to a clause.

---

## Industry variations — Domain 14

- **Water and desalination.** Long commissioning and reliability-test periods mean the gap between
  mechanical completion and the commercial operations date is months rather than weeks, so the
  calendar-fixed repayment-date problem of KA 14.4.2 is at its sharpest; permit conditions
  (discharge, brine outfall) generate owner-retained variations late in the programme, exactly where
  contingency coverage on the remainder is thinnest.
- **Solar and wind, contracted power.** Modular construction gives smooth, measurable progress and
  short commissioning, so certification is comparatively easy — but equipment is procured early and
  in bulk, which makes **off-site materials certification** the dominant exposure and vesting the
  dominant control. Grid-connection dates are owner-retained risk and are the usual cause of a
  commercial operations date slip the EPC contractor does not pay for.
- **Transport concessions.** Long linear works, extensive land and utility interfaces, and
  measured rather than milestone certification, so `PoC` judgment dominates and the certifier's
  independence is the whole control. Variations are numerous and small, which is precisely the
  regime in which cumulative headroom consumption (KA 14.3.3) escapes notice.
- **Digital infrastructure — data centres.** Very long lead times on electrical and cooling
  equipment, so advance payments and off-site certification are structural rather than exceptional,
  and the vesting question of Case study B is a first-order credit issue. Phased handover means the
  completion sequence is repeated per hall, and the facility must define whether revenue from an
  early phase is `CFADS` before overall completion.
- **Oil, gas and process plant.** Performance testing is elaborate and multi-parameter (throughput,
  yield, energy consumption, emissions), so partial completion and buy-down arithmetic (KA 14.4.3)
  is a normal rather than exceptional outcome, and the interaction of delay and performance sub-caps
  under an aggregate cap (Domain 12, KA 12.1.2) is tested in practice.
- **Social infrastructure PPPs — availability-based.** Payment begins on availability rather than
  output, so a delay is pure revenue deferral with no volume dimension; the sharp edge is that
  unitary-charge deductions begin immediately at service commencement, so a project that completes
  late *and* commissions imperfectly is penalised twice, and the contingency held for the
  commissioning period must cover both.

---

## Case study — Domain 14: the quarter-five draw that did not fund (water / desalination)

**Situation.** Kestrel Water SPC reached the end of construction quarter five with certified
progress at **61 %**, cumulative certified spend of **33,945,403**, and a monthly report whose
headline was that contingency was **58.85 % drawn against 61.00 % progress** — "consumption
proportionate, no funding issue arising". The same report showed a blended `CPI` of **0.949821** and
a `BAC/CPI` forecast of **54,326,038** against a `BAC` of 51,600,000, described as "a 2.7 million
forecast overrun within the funded contingency of 3,645,403". Two quarters had passed on that
reading. The quarter-five draw request was submitted for **9,924,564**.

**What happened.** The lenders' technical adviser declined to certify. Three findings, each
independently sufficient. **The `CPI` was blended across scopes with different cost-risk
ownership.** Of the 2,726,038 of uplift in the `BAC/CPI` forecast, **2,535,849** was attributed to a
fixed lump-sum EPC scope certified against milestones — where `CPI` is 1.000 by construction and
cannot overrun — and only **190,189** to the owner-retained scope, which was running at a scope
`CPI` of **0.6000** and had already consumed **1,680,000** of contingency. Disaggregated, the
bottom-up remaining owner-retained estimate was **2,400,000** against a remaining budget of
1,080,000. **The cost-to-complete omitted three lines earned value cannot see.** On a commitment
basis it was `18,720,000 + 840,000 + 1,260,000 + 2,400,000 + 1,436,674 =` **24,656,674** against
available commitment of `15,910,254 + 6,818,680 =` **22,728,934** — out of balance by
**1,927,740**. **And the contingency test failed independently.** Known committed claims on
contingency were **3,420,000** (840,000 of approved variations, 1,260,000 of assessed claim
exposure, 1,320,000 of bottom-up owner-scope uplift) against remaining contingency of
**1,500,000**; adding the open register's P80 of **1,764,289** gave coverage of **0.2893**. The
report's 58.85 % draw-rate check had been a coincidence throughout: risk had never been distributed
pro rata to progress, and the two largest sanction-register items — ground conditions at the intake
and owner-retained interface diversions — had both materialised, leaving the remainder funded at a
confidence level nobody had computed. The original 3,645,403 had been a **P84** provision against
the whole register; what remained was a P-nothing.

**How it resolved.** The sponsors injected **1,927,740** of additional equity as a condition to the
quarter-five drawing, taking equity to **19,927,740** and total funding to **61,927,740**. Because
senior debt was unchanged at 42,000,000, **every coverage ratio was untouched** — `DSCR` stayed at
1.2743 — and the whole overrun landed on equity: gearing moved from 70.0/30.0 to **67.82/32.18**
(`D/E` **2.1076**), the equity cheque rose **10.71 %**, and Domain 4's project `NPV` of +16,179,360
fell to **+14,251,620** on the same operating forecasts. The monitoring regime was rewritten with
three changes and no new money: disaggregated `CPI` reporting by scope with the blended figure
banned; a contingency coverage ratio on the remainder replacing the draw-rate check, with the
denominator itemised at every data date; and the in-balance test computed on the commitment basis at
every draw and every reporting date, reconciled against the contingency shortfall as a cross-check.

**What the domain teaches here.** **Two independent tests reached the same conclusion — 1,927,740 of
in-balance shortfall against 1,920,000 of contingency committed beyond its balance, plus the 7,740
of interest on an early draw — and the monthly report contained neither.** What it contained instead
were two numbers that looked reassuring and were arithmetically empty: a draw rate compared with
progress, and a blended `CPI` that happened to produce a plausible total by attributing 93 % of an
overrun to the one scope incapable of overrunning. The lesson is not that the sponsors were
negligent; it is that **the reassuring figures are the default outputs of an ordinary cost report,
and the tests that matter have to be specified, itemised and demanded.** That is the entire purpose
of Toolkit 14.T.2.

## Case study B — Domain 14: the switchgear that was never ours (digital infrastructure)

**Situation.** A data-centre SPV financed a first phase at an envelope of **420,000,000** —
**294,000,000** senior debt and **126,000,000** equity — with a funded contingency of 12,600,000.
Lead times on medium-voltage switchgear and generators ran to fourteen months, and the electrical
subcontractor's cash flow could not carry them. With the general contractor's support, the
certification basis was allowed to drift: over five months the independent engineer certified
**38,400,000** of off-site equipment at the vendors' works. Vesting certificates, segregation
confirmations and insurance endorsements in the SPV's name were obtained for 22,000,000 of it. For
**16,400,000** — **3.90 %** of the envelope — they were not; the paperwork was described in the
monthly report as "in progress with the vendors".

**What happened.** The electrical subcontractor entered insolvency proceedings sixteen months into a
twenty-two-month programme. The vested equipment was traced and released. Of the 16,400,000 that had
been certified without vesting, segregation or insurance, the administrator treated the equipment as
the subcontractor's asset; **4,100,000** was ultimately recovered through negotiated release of
identifiable items, a **25 %** recovery. The loss was **12,300,000**. Of the certified amount,
**11,480,000** had been advanced as senior debt at 70 % gearing, and it had been outstanding for
fourteen months at 5.5 %, adding **736,633** of capitalised interest carried on an asset that did
not exist. Total cost **13,036,633** — **10.35 %** of the equity cheque. Against remaining
contingency of **5,600,000** the in-balance test failed by **6,700,000**, which the sponsors funded
as equity; gearing moved to **68.90/31.10**. The plant completed five months late, by which point the
delay damages cap had bound and the availability period had been extended once, for a fee.

**How it resolved.** The immediate remedy was procedural and should have been in place from the
first draw: certification of off-site materials was capped at a stated percentage of the contract
price; each certification was conditioned on a vesting certificate, a segregation confirmation from
an independent inspector and an insurance endorsement naming the SPV, delivered *before* the
certificate rather than promised after it; and the cumulative off-site balance became a reported
line with an ageing analysis. The certification-basis mix — achieved milestones, assessed
percentages, off-site materials — was reported every month thereafter, which made drift visible
within one period instead of five.

**What the domain teaches here.** **Certification drift is never decided; it accumulates.** No
single certificate was unreasonable, every one had a commercial justification, and the cumulative
effect was to convert 11,480,000 of secured lending into an unsecured claim in a third party's
insolvency — a decision no committee took and no minute records. The arithmetic that would have
stopped it is trivial and was never run: **16,400,000 certified without vesting was 3.90 % of the
envelope and 130.16 % of the whole 12,600,000 of contingency funded at close**, and its exposure
was fully computable on the day of each certificate. Compare Kestrel: the exposure there was
measured on the same basis at 1,339,842 in a single quarter, or 27.91 % of the performance bond. The control is not
sophistication; it is a reported line and a condition that must exist before, not after, the money
moves.

---

## Executive perspective — Domain 14

What a project finance director cannot delegate in this domain:

- **The certification basis, and the off-site balance.** Which basis the facility specifies, whether
  the mix has drifted, and how much has been certified against work not in place — with vesting,
  segregation and insurance confirmed before the money moves, never promised after it (Case study B,
  where the answer was 16,400,000 and cost 13,036,633).
- **The in-balance test on the commitment basis, at every draw.** Not the model's balance, which
  reconciles by construction, but the commitment number: Kestrel's 24,656,674 against 22,728,934,
  and the double-count that would have hidden it by adding contingency to available funds.
- **The contingency coverage on the remainder, with its denominator itemised.** 0.2893, not the
  1.3825 that appears when only the mean of open risk is counted, and never the draw-rate check that
  let two quarters pass.
- **The funding order.** A single sub-clause worth 1,466,064 of capitalised interest and 11,406,157
  of mid-build credit exposure — 41.6 times the ten-basis-point margin the same team spent three
  weeks negotiating.
- **The date the first instalment falls due, and what counts as `CFADS` when it does.** A
  calendar-fixed date breaches a 1.20× covenant on **three weeks** of slip, and whether delay
  damages are `CFADS` is worth **0.4791** of coverage at that test. Both are drafting decisions taken
  before there is a delay.
- **The two costs of delay, stated together.** 415,000 funded and 947,000 economic a month, with the
  damages recovering 144.58 % of the first and 63.36 % of the second — because a project whose
  drawdown tests keep passing while equity value drains is the failure mode this domain exists to
  make visible.

## Calculation exercises — Domain 14

**Exercise 14.1** A project has a total funding envelope of 180,000,000, of which 96,000,000 has
been drawn. At the data date the remaining committed contract value is 74,000,000; approved
variations not yet certified are 3,200,000; assessed exposure on notified claims is 2,600,000;
remaining owner's costs are 4,100,000; and remaining capitalised interest and fees are 2,900,000.
Remaining unallocated contingency is 5,000,000. Run the in-balance test.
*Solution.* Available commitment `= 180,000,000 − 96,000,000 =` **84,000,000**. Cost-to-complete
`= 74,000,000 + 3,200,000 + 2,600,000 + 4,100,000 + 2,900,000 =` **86,800,000**. The project is
**out of balance by 2,800,000**, which must be cured before the next drawing. *Common error:* adding
the 5,000,000 of remaining contingency to available funds, giving 89,000,000 and a comfortable
surplus of 2,200,000 — contingency is a **use** funded by the same undrawn commitment, so the
addition double-counts it and reverses the answer.

**Exercise 14.2** A four-quarter construction programme spends 20,000,000 per quarter, funded 75/25
at 8.0 % per annum, interest accruing at 2.0 % per quarter on the opening drawn debt balance with
draws at period end. Compute total capitalised interest under pro-rata funding and under
equity-first funding.
*Solution.* Pro rata: interest accrues from quarter two on a growing 75 %-geared balance, giving
**1,818,068** and total uses of 81,818,068 (debt 61,363,551, equity 20,454,517). Equity-first: the
20,000,000 equity commitment funds the whole of quarter one, so debt is first drawn in quarter two
and first accrues interest in quarter three, giving **1,208,000** and total uses of
81,208,000. The saving is
**610,068**, or **33.56 %**. *Common error:* computing interest on closing rather than opening
balances, which charges a full period of interest to every draw and overstates capitalised interest —
Domain 8's Exercise 8.2 measured that error at 42 % on a comparable profile.

**Exercise 14.3** An EPC contract has a price of 90,000,000, an advance payment of 15 % recovered
pro rata against certified value, and retention of 10 % of each certification capped at 5 % of the
contract price. Cumulative certified value before this month is 36,000,000; this month's certified
value is 7,200,000. Compute this month's net payment and the cumulative retention held.
*Solution.* Advance `= 13,500,000`; recovery this month `= 0.15 × 7,200,000 =` **1,080,000**.
Retention cap `= 0.05 × 90,000,000 =` 4,500,000; held before `= 0.10 × 36,000,000 =` 3,600,000, so
the cap permits a further 900,000 and this month's 10 % of 7,200,000 = **720,000** fits. Net payment
`= 7,200,000 − 1,080,000 − 720,000 =` **5,400,000**; cumulative retention held **4,320,000**.
*Common error:* applying retention to the amount net of advance recovery — `0.10 × (7,200,000 −
1,080,000) = 612,000`, giving a net payment of 5,508,000 and **over-paying by 108,000**. Retention
is withheld from gross certified value; advance recovery is a separate deduction.

**Exercise 14.4** A variation costs 6,200,000 and will add 600,000 a year of `CFADS`. The loan runs
15 years at 6.5 % and the target `DSCR` is 1.25×. Compute the maximum coverage-neutral debt funding,
the equity or contingency required, and the marginal `DSCR` if the variation were fully debt-funded.
*Solution.* `AF(0.065, 15) = 9.402669`. Maximum debt funding `= (600,000/1.25) × 9.402669 =`
**4,513,281**, being **72.79 %** of the cost; the residual **1,686,719** must come from equity or
contingency. Fully debt-funded, incremental debt service `= 6,200,000/9.402669 =` **659,387** and
the marginal `DSCR` `= 600,000/659,387 =` **0.9099** — the variation would not even service the debt
raised to build it. *Common error:* sizing on the full `ΔCFADS` without the coverage divisor
(`600,000 × 9.402669 = 5,641,601`), which is the Domain 10 sizing error transplanted into change
control and would leave the project 1,128,320 over-levered on this variation alone.

**Exercise 14.5** A facility has annual debt service of 11,200,000 falling due on a calendar-fixed
date twelve months after scheduled completion, a 1.15× covenant, a six-month debt service reserve of
5,600,000, and operating `CFADS` of 14,400,000 a year accruing evenly from actual completion.
Completion slips three months. Compute the `DSCR` at the first test, the reserve consumption, and
the three breakeven slips.
*Solution.* `CFADS` at the first test `= 14,400,000 × 9/12 =` **10,800,000**;
`DSCR = 10,800,000/11,200,000 =` **0.9643**. Shortfall **400,000**, being **7.14 %** of the reserve.
Breakevens: covenant cash trigger `= 1.15 × 11,200,000 = 12,880,000`, so the covenant fails beyond
`12 × (1 − 12,880,000/14,400,000) =` **1.2667 months**; operating cash ceases to cover debt service
beyond `12 × (1 − 11,200,000/14,400,000) =` **2.6667 months**; cash plus a fully funded reserve
ceases to cover it beyond `12 × (1 − 5,600,000/14,400,000) =` **7.3333 months**. *Common error:*
computing the `DSCR` on annualised `CFADS` of 14,400,000 (giving 1.2857 and a comfortable pass) —
the covenant is tested on the period's cash, not on a run rate, which is the whole content of
KA 14.4.2.

## Practitioner's toolkit — Domain 14

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 14.T.1 — Draw request pack and certification control sheet

**Part A, the arithmetic**, one row per line with a source reference: gross certified value this
period, split into value of milestones achieved, value of assessed percentage completion, and value
of off-site materials; less retention this period, with cumulative retention against its cap; less
advance recovery this period, with cumulative recovery against the advance; plus owner-retained costs
incurred and evidenced; plus fees, taxes and premiums; plus interest and commitment fees accrued,
computed on **opening** balances with the day-count basis stated; equals the period funding
requirement, split by the documented funding order into the debt draw and the equity draw.
**Part B, the conditions**, one row per condition to drawing with the evidence reference, its expiry
date, and the named person who confirmed it — no default or potential default, representations
repeated, certifier's certificate, insurances in force and premiums paid, in balance, within the
availability period, within the commitment. **Part C, the certification-mix trend**: the three
components of Part A's gross certified value as percentages, this period and cumulatively, with the
off-site balance aged. **Rules:** the off-site line is nil unless vesting, segregation and insurance
are all in place *for the amount certified*; the certification mix is reported whether or not anyone
asks; and cumulative drawn is tied to the facility agent's statement, never to the model.

### Toolkit 14.T.2 — Restated sources and uses, the in-balance test, and the `EAC` bridge

**Sheet 1, restated statement:** every use in three columns — drawn to date, remaining to be funded,
total — against sources in the same three columns, with available commitment stated as undrawn debt
plus uncalled equity and a printed warning that contingency is **not** added. Any imbalance is
explained in cash terms with its cause named. **Sheet 2, lender's cost-to-complete:** remaining
committed contract value; approved variations not yet certified; assessed exposure on notified
claims, with the assessor named; bottom-up re-estimate of owner-retained scope; remaining
capitalised interest, fees, premiums and taxes to the forecast commercial operations date; total;
available commitment; surplus or shortfall. **Sheet 3, the bridge:** the `EAC`-based `CTC` from the
delivery organisation's cost report on each published method, then one line per reconciling item —
each labelled with the discipline that is blind to it — arriving at the lender's number. **Sheet 4,
disaggregated performance:** `EV`, `AC` and `CPI` separately for fixed-price and owner-retained
scope, with the blended index shown only to be marked "not to be used for forecasting". **Rule:** no
cost report is issued, and no draw request signed, without Sheets 2 and 3 complete.

### Toolkit 14.T.3 — Contingency coverage and change-control register

**Contingency section:** funded amount; drawn to date, itemised by cause with the register item each
draw retires; remaining unallocated. **Denominator, itemised:** approved variations not yet
certified; assessed claim exposure; bottom-up re-estimate above budget on owner-retained scope; then
the open risk register with `p`, impact, `EMV`, and the computed mean, σ and P80. **Coverage on the
remainder**, computed as remaining ÷ (known committed + P80), with the two partial ratios (against
the mean, and against open risk only) shown beside it and labelled as insufficient tests. **Shortfall
in currency.** **Change-control section**, one row per variation above the threshold: price; time
effect; funding source, named; `ΔCFADS` claimed and by whom; `Δ debt service` at the debt-funded
share; **marginal `DSCR`**; coverage-neutral maximum debt funding; headroom consumed; cumulative
headroom consumed by all approved variations against the base-case headroom. **Rule:** the draw-rate
check — contingency drawn against progress — does not appear on this form; and no variation is
approved without a funding source and a marginal `DSCR`.

## Exam preparation — Domain 14

**What is assessed.** The restated sources-and-uses statement and the in-balance test on a
commitment basis; the composition of a draw request from gross certified value to the debt and equity
draws; the funding-order comparison and its capitalised-interest, exposure and present-value
consequences; the lender's cost-to-complete and its line-by-line reconciliation to an `EAC`-based
`CTC`; why a blended `CPI` misattributes an overrun where one scope is fixed-price and
milestone-certified; the contingency coverage ratio on the remainder and the defensible choice of
denominator; the three certification bases and the exposure created by off-site certification
without vesting; advance payment and retention arithmetic, including the retention release that
falls in the first operating year; the marginal `DSCR` of a variation and the coverage-neutral debt
share; the funded versus economic cost of a month of slip and the three damages-recovery
percentages; the `DSCR` at a calendar-fixed first repayment date with its covenant, payment and
reserve breakevens; and the buy-down that restores a stated coverage level.

**The calculations to do under time pressure.** Available commitment and the in-balance result from
a five- or six-line cost-to-complete. A period funding requirement and its 70/30 split, with accrued
interest on the opening balance. Certified value on three bases, net of retention, times gearing.
Net payment after advance recovery and retention, with the cap tested. `Δ debt service = cost ÷
AF(r, n)` and the marginal `DSCR`; and the coverage-neutral maximum, `(ΔCFADS/target) × AF(r, n)`.
`CFADS × (12 − m)/12 ÷ debt service`, and the three breakeven slips by inverting it. Supportable
debt at a target ratio, and the buy-down as the difference from outstanding debt.

**The traps.** Adding remaining contingency to available commitment (Exercise 14.1, MCQ 14.1-A) ·
omitting remaining capitalised interest from a cost-to-complete because it is not work (MCQ 14.2-A) ·
applying `BAC/CPI` to a blended index across fixed-price and owner-retained scope (14.2.1,
MCQ 14.2-B, Case study A) · reading the contingency draw rate against progress as evidence of health
(14.2.2, MCQ 14.2-D) · testing contingency against the *mean* of open risk rather than the P80 plus
known commitments (MCQ 14.2-C) · computing capitalised interest on closing rather than opening
balances (Exercise 14.2, imported from Domain 8) · certifying off-site materials without vesting,
segregation and insurance (14.3.1, Case study B) · applying retention to the amount net of advance
recovery (Exercise 14.3) · assuming a retention release falling after completion is already funded
because it is inside the contract price (14.3.2, MCQ 14.3-B) · sizing a debt-funded variation on full
`ΔCFADS` without the coverage divisor (Exercise 14.4, MCQ 14.3-C) · treating an `NPV`-positive
variation as automatically fundable by debt (14.3.3, MCQ 14.3-D) · quoting one damages-recovery
percentage without naming the cost basis (14.4.1, MCQ 14.4-C) · computing the first-test `DSCR` on
annualised rather than period `CFADS` (Exercise 14.5, MCQ 14.4-B) · assuming a slip postpones a
calendar-fixed repayment date (MCQ 14.4-A) · and confusing the buy-down that restores a covenant with
the one that restores base coverage (14.4.3, MCQ 14.4-D).

**How the domain connects.** Domain 6 built the construction funding model this domain operates;
Domain 8 supplied the capitalised-interest area rule, the escalation treatment and the economic cost
of a slip during construction; Domain 5 priced a slip at the commercial operations date and
established cost-overrun support; Domain 12 fixed the contract limits — damages rates, caps, bonds
and guarantees — that this domain draws against; Domain 10 supplied the coverage machinery, the
reserve and the covenant that KA 14.4 tests; Domain 13 delivered the conditions precedent that
KA 14.1's conditions to drawing continue; and PML-AI Domain 7 supplied the `EAC` family that
KA 14.2 reconciles to. Forward, Domain 15 takes the project past completion into operation — the
waterfall, the covenant regime, refinancing and restructuring — and Domain 16 governs the automation
that this domain's reporting cycle invites.

## Domain 14 summary
Construction monitoring is the discipline of releasing other people's money against evidence, and it
reduces to four tests. The **draw request** converts certified value into a funding requirement:
Kestrel's quarter five was `9,391,718 + 245,707 + 287,138 =` **9,924,564**, split 70/30, of which
the interest line — **0.83 %** of the requirement in quarter one and **8.44 %** in quarter eight — is
money borrowed to pay interest on money already borrowed. The **funding order** that governs that
split is a single sub-clause worth **1,466,064** of capitalised interest across three orders
(equity-first **1,338,006**, pro rata **2,114,597**, debt-first **2,804,070**), **11,406,157** of
mid-build lender exposure, **183,013** of present value to the sponsor, and — at a fixed envelope —
**742,647** of additional funded contingency; it is **41.6 times** the ten-basis-point margin move
the same negotiation spends weeks on. The **in-balance test** on a commitment basis is not
`EAC − AC`: Kestrel's lender cost-to-complete of **24,656,674** exceeds an `EAC`-based `CTC` of
21,120,000 by three lines earned value cannot see — approved variations, assessed claim exposure and
remaining capitalised interest — and exceeds available commitment of **22,728,934** by
**1,927,740**, while a blended `CPI` of 0.949821 was attributing **2,535,849** of overrun to a
fixed-price scope where `CPI` is 1.000 by construction and only **190,189** to the owner-retained
scope actually running at 0.6000. The **contingency test** fails independently and earlier:
1,500,000 remaining against 3,420,000 of known committed claims plus a 1,764,289 P80 gives coverage
of **0.2893**, where the comfortable versions of the same ratio read 1.3825 and 0.8502, and where a
draw rate of 58.85 % against 61.00 % progress had read as health for two quarters. Certification is
where security is quietly converted: the same quarter's work is worth **5,760,000** on milestones,
**7,084,800** measured and **7,774,800** cost-incurred, and the cost-incurred basis advances
**1,339,842** of senior debt against work not in place — 27.91 % of the performance bond, and the
mechanism that cost Case study B **13,036,633**, or 10.35 % of its equity. Payment terms carry
prices: a 10 % advance costs **254,597** of capitalised interest, 0.53 % of the EPC price, and should
be priced rather than granted; a 600,000 retention release falling twelve months after completion
takes `CFADS` to 5,784,000, the `DSCR` to **1.1546**, and breaches a 1.20× covenant by exceeding
Domain 10's 372,438 of headroom. Variations carry four numbers, not two: a 1,850,000 train adding
240,000 of `CFADS` has an `NPV` of **+711,946** and a marginal `DSCR` of **1.0876** if fully
debt-funded, so the coverage-neutral limit is **1,578,947**, or 85.35 % of cost. And delay must be
costed twice — **415,000** funded and **947,000** economic a month, recovered by the same
20,000-a-day damages at **144.58 %** and **63.36 %** respectively — because a project whose drawdown
tests keep passing while equity drains is invisible on one basis and obvious on the other. At the
first, calendar-fixed repayment date the arithmetic is unforgiving: **three weeks** of slip
(0.7001 months) breaches the 1.20× covenant, 2.5834 months exhausts operating cash, 7.2917 months
exhausts cash plus the 2,504,818 reserve, a four-month slip gives a `DSCR` of **0.8496** and consumes
30.09 % of that reserve — and whether delay damages count as `CFADS` moves the same test by
**0.4791**, from 0.8496 to 1.3286. Domain 15 takes the plant into operation, where the waterfall,
the covenant regime and the reserve replenishment this domain has already committed begin to run.
