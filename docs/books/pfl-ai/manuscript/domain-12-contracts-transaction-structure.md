# Domain 12 — Contracts and Transaction Structure

## Why this domain exists

Domain 11 established that risk allocation is a price and computed what Kestrel's transfers were
worth. It stopped one step short of the thing that actually binds anyone: **the document**. An
allocation that has been priced but not written is an intention; an allocation that has been
written badly is a liability the model does not show. This domain is where the arithmetic of
Domains 5 to 11 becomes contractual language, and where the leader discovers whether the
protections the financial model assumed are the protections the contracts deliver.

The domain's central claim is that **a contract's commercial value is not its promise but the
size, quality and reach of the money standing behind it.** Practitioners read contracts for
their obligations. The contractor *shall* achieve the guaranteed output, the offtaker *shall*
take the water. Lenders read them for their limits: the liquidated-damages rate, the cap on that
rate, the aggregate cap above it, the exclusion of consequential loss, the identity and balance
sheet of the guarantor, the expiry date of the bond, and what the termination clause pays. Those
limits are numbers, they are computable before signature, and they routinely fail to cover the
loss they were drafted against. Kestrel's contract stack looks orthodox — delay damages of USD
20,000 per day capped at 10 % of the EPC price, performance damages capped at another 10 %, an
aggregate liability cap of 20 % — and yet a 300-day delay combined with a 5 % output shortfall
produces exposure of **USD 12,255,674** against nominal cover of **USD 9,600,000** and
risk-adjusted cover of **USD 8,160,000**. The uncovered residue of **2,655,674** is 14.75 % of
the entire equity cheque, and it was knowable at signature from four numbers on two pages.

The domain works through the stack in the order a transaction is assembled. The construction and
operating contracts are where damages, caps and buy-downs are calibrated, or mis-calibrated
against a loss that is computable to the day (KA 12.1). The revenue contracts are where the
project's cash flow is manufactured, and where the single most under-tested number in project
finance lives: the **contracted volume floor**, which for Kestrel must be **95.8892 %** of
capacity to hold its own covenant, not the 90 % the commercial team negotiated (KA 12.2). The
guarantees, direct agreements and security package convert promises into recoveries, and their
worth is the guarantor's credit quality multiplied by the cap, not the cap (KA 12.3). And claims
and change are where the allocation is tested in anger, on an arithmetic most organisations
never do: on Kestrel's disputed claim, fighting costs a present value of **1,347,115** against a
disputed sum of **1,520,000**, so the rational settlement ceiling is **84.68 %** of the claim
and the disputed sum would have to exceed **6,901,234** before litigation beat settlement (KA
12.4).

**Learning objectives.** After this domain a candidate can: read an EPC, O&M, offtake, concession
and guarantee package as a set of computable financial limits rather than a set of promises;
calibrate a delay liquidated-damages rate against daily carrying cost plus forgone `CFADS`, and
identify the day on which the cap binds; compute the uncovered residue of a delay beyond the cap
and express it as a share of contract value, equity and project `NPV`; build a cap stack and show
where sub-caps exhaust an aggregate cap; calibrate performance liquidated damages on a
value basis and distinguish that figure from the coverage-restoring buy-down and from the bare
covenant-restoring prepayment, quantifying the gap between all three; explain why the clause
governing the *application* of damages proceeds can leave equity short even when the amount is
right; derive the minimum contracted volume that holds a stated `DSCR` covenant and explain why it
exceeds the commercially negotiated floor; test a termination-compensation formula against the
debt-outstanding profile and against unreturned equity; compute risk-adjusted cover from
instrument face amounts and guarantor credit quality, and the equivalent face amount of a
conditional guarantee against an unconditional bond; assess the commercial value of a claim
including time impact and the cost of the dispute itself; compute a settlement ceiling, the saving
from settling and the disputed sum at which litigation becomes rational; and govern AI use in
contract review while never delegating a legal conclusion to it.

**The master thread.** Kestrel Water SPC continues. Capital cost **USD 60,000,000** funded
**70/30** as **USD 42,000,000** of senior debt at **6.0 % over 12 years** — annual instalment
**USD 5,009,635.23**, year-one interest **2,520,000**, year-one principal **2,489,635.23**,
`AF(0.06, 12) = 8.383844` (Domain 3) — plus **USD 18,000,000** of equity. Operating life 25
years; documented `CFADS` **6,384,000** (6,984,000 before working-capital movements); `EBITDA`
**7,500,000**, `EBIT` **5,100,000**; appraisal at 8 % gives `NPV` **+16,179,360**, `IRR` 12.19
%, `MIRR` 9.73 %, `PI` 1.270; year-one `DSCR` **1.2743 = LLCR**, `PLCR` **1.9431**, with a
**1.20×** covenant (cash trigger **6,011,562**, headroom **372,438**) and a **1.15×** lock-up
(cash trigger **5,761,081**). Its contracts are a fixed-price, date-certain EPC wrap at
**48,000,000** (Domains 5, 6 and 8), delay damages of **20,000 per day** capped at **10 %** of
that price, and a 25-year water offtake coterminous with the 25-year concession. This domain
adds the rest of the stack (the performance-damages regime, the aggregate cap, the security
package and the claims machinery), and computes what each is worth.

---

## Knowledge Area 12.1 — EPC and O&M

*Topics: 12.1.1 the construction contract as a set of financial limits · 12.1.2 delay damages,
caps and the uncovered residue · 12.1.3 performance damages, buy-down and the three make-good
numbers · 12.1.4 O&M contracts and the liability asymmetry.*

### 12.1.1 The construction contract as a set of financial limits

**Definition.** An **EPC contract** is a single agreement under which one contractor engineers,
procures and constructs the whole works for a fixed lump-sum price, by a date certain, to defined
performance — the **wrap** whose bankability role Domain 5 (KA 5.4.1) established and whose
premium Domain 11 (KA 11.1.3) priced. Its obligations are familiar. Its **financial limits** are
what a financier reads, and they are these six numbers:

| Limit | What it is | Kestrel |
|---|---|---|
| Contract price | The fixed sum, with defined change mechanics | 48,000,000 |
| Delay damages rate | Payable per day of delay beyond the date certain | 20,000 / day |
| Delay damages cap | Maximum recoverable for delay, whatever its length | 10 % = 4,800,000 |
| Performance damages cap | Maximum recoverable for output or efficiency shortfall | 10 % = 4,800,000 |
| Aggregate liability cap | Maximum total contractual liability, all heads | 20 % = 9,600,000 |
| Performance security | Instruments callable against the above (KA 12.3) | bond 10 % + parent guarantee |

Three properties of that table decide whether the financing works. First, **the rate and the cap
are independent decisions**: a well-calibrated rate under a low cap protects only the early days
of a delay. Second, **the caps interact**, and the aggregate cap is the real limit: Kestrel's
two 10 % sub-caps sum to exactly the 20 % aggregate, so any third head of claim (defect
rectification after the sub-caps are exhausted, a third-party indemnity that is not carved out)
has no room left in the stack. Third, **the exclusion of consequential or indirect loss**
removes from recovery precisely the losses a project company actually suffers (forgone revenue,
financing cost, loss of profit), which is why liquidated damages exist at all: they are the
agreed, recoverable substitute for a loss the contract has excluded.

**The arm's-length rule is not only an O&M rule.** Sponsor-affiliate contracting is at least as
common on the construction side as on the operating side, and the six limits above are precisely
where an affiliate relationship shows: a price, a damages rate, two sub-caps and an aggregate
cap negotiated between related parties are terms nobody was fully adverse about. Wherever the
EPC contractor, the operator, a supplier under a material input contract, or the provider of any
instrument in the security package (KA 12.3) is an **affiliate of a sponsor**, the relationship
is **disclosed**, the terms are **tested on an arm's-length basis** by someone outside the
commercial line, and the body that approved the related-party arrangement is **recorded**,
because a lender is being asked to accept related-party terms as a bankability item (Domain 1,
KA 1.3.3; the O&M case is developed at 12.1.4, and the double-counting it creates in the
security package at 12.3.2). Whether a related-party contract also engages any approval
requirement under the SPV's constitution, the shareholders' agreement or the finance documents
is a question on those documents; it is characteristically a reserved matter (Domain 5, KA
5.2.3), and it is established rather than assumed.

Whether a particular damages provision is enforceable as agreed compensation, whether it is
vulnerable to challenge as a penalty, how "consequential loss" is construed, and whether an
aggregate cap survives particular breaches are all **jurisdiction-specific questions of law and
drafting**. This domain computes the commercial arithmetic those provisions are meant to deliver;
it does not opine on their effect. Refer each such question to qualified counsel in the governing
jurisdiction, and require counsel's confirmation before the model relies on a recovery.

### 12.1.2 Delay damages, caps and the uncovered residue

**Definition.** **Delay liquidated damages** are a pre-agreed sum per unit of time by which the
contractor misses the date certain. Their commercial purpose is to reimburse the project company
for the cost of lateness, and their correct calibration basis is therefore the **daily economic
cost of delay**: the carrying cost of debt already drawn plus the `CFADS` the project would have
earned. Domain 5 (KA 5.4.2) computed Kestrel's: **7,000 per day** of interest on the fully drawn
42,000,000 at 6.0 % on a 30/360 basis, plus **17,733.33 per day** of forgone `CFADS`, giving
**24,733.33 per day**. A rate of 20,000 recovers **80.86 %** of that; a rate calibrated on
forgone revenue alone would recover **71.70 %**, omitting the most certain component of the loss,
because interest accrues whether or not the plant would have run well.

What Domain 5 did not do, and what a financier must do before signature, is compute the whole
cap stack against a combined stress, because delay and underperformance are correlated (a
contractor in schedule trouble commissions in a hurry) and the caps are shared.

**Worked example 12.1.2 — Kestrel's cap stack against a 300-day delay and a 5 % shortfall.**

1. **Setup.** The contract stack of 12.1.1. The stress: commissioning is **300 days** late and
   the plant completes at **95 %** of guaranteed output. Daily economic cost of delay
   **24,733.33** (Domain 5, KA 5.4.2, 30/360). The value of the output shortfall is computed in
   12.1.3 below as **4,835,673.53**. Compute the total exposure, the recovery the caps permit, and
   the residue.
2. **Formula.** Delay exposure = daily economic cost × days. Delay recovery =
   min(rate × days, delay cap). Performance recovery = min(value of shortfall, performance cap).
   Total recovery is further limited by the aggregate cap. Residue = exposure − recovery.
3. **Substitution.** Delay: `24,733.33 × 300`; recovery `min(20,000 × 300, 4,800,000)`.
   Performance: `min(4,835,673.53, 4,800,000)`. Aggregate: `min(4,800,000 + 4,800,000, 9,600,000)`.
4. **Result.**

   | Head | Exposure | Contractual recovery | Uncovered |
   |---|---|---|---|
   | Delay, 300 days | 7,420,000.00 | 4,800,000.00 (capped at day 240) | 2,620,000.00 |
   | Output shortfall, 5 % | 4,835,673.53 | 4,800,000.00 (capped) | 35,673.53 |
   | **Total** | **12,255,673.53** | **9,600,000.00** (= the aggregate cap, exactly) | **2,655,673.53** |

   The uncovered residue is **5.53 %** of the EPC price, **14.75 %** of the equity contribution
   and **16.41 %** of Domain 4's entire project `NPV` of 16,179,360.
5. **Interpretation.** Four things in that table are decision-grade and none of them requires a
   model. **The cap, not the rate, is where the structure breaks.** Damages of 20,000 per day
   recover 80.86 % of the daily cost for 240 days and **nothing at all** thereafter; the 300-day
   case leaves 2,620,000 uncovered and Domain 5's 360-day case leaves 4,104,000. The negotiating
   priority is therefore not the rate (sponsors habitually spend their leverage there), but the
   **cap-binding day**, which is the cap divided by the rate and which the sponsor should
   compare with the credible worst-case delay from the schedule risk analysis. A cap that binds
   before the P80 delay is a cap that does not cover the risk it was bought for. **The aggregate
   cap is not an additional protection.** Two 10 % sub-caps summing to a 20 % aggregate means
   the aggregate binds only when a *third* head arises, and then it binds at zero: after a full
   delay and a full performance claim, a defect the contractor refuses to rectify is
   uncompensated. Structures that intend the aggregate to be meaningful set it above the sum of
   the sub-caps, or ring-fence specific heads outside it. **The residue is an equity number, and
   it belongs in the equity case.** 2,655,674 is 14.75 % of the 18,000,000 cheque; a sponsor
   that has not stated that figure to its investment committee has not described the
   transaction. **And the contractor's incentive changes at the cap.** Once cumulative damages
   reach 4,800,000 the contractor's marginal cost of a further day of delay is zero, which
   converts a financial deterrent into a pure commercial negotiation exactly when the project
   can least afford one — the reason sophisticated structures pair a cap with a
   **termination-for-delay right** at a defined long-stop date, so that something other than
   money still bites.

> **Fig 12.1.1 — The cap stack against the loss it was drafted for.** Horizontal stacked
> comparison of three bars for the combined 300-day delay and 5 % output shortfall: total exposure
> 12,255,674 (split 7,420,000 delay + 4,835,674 performance); nominal contractual cover 9,600,000
> (split 4,800,000 delay cap + 4,800,000 performance cap, annotated "the two sub-caps exhaust the
> 20 % aggregate exactly"); and risk-adjusted cover 8,160,000 (4,800,000 on-demand bond at full
> value + 3,360,000 of parent guarantee at 0.70 credit quality). Crimson brackets mark the
> uncovered residue of 2,655,674 nominal and 4,095,674 risk-adjusted, labelled as 14.75 % and
> 22.75 % of the 18,000,000 equity contribution. Source: PCI original. Alt text: three horizontal
> bars comparing project exposure with nominal contractual cover and with credit-adjusted cover,
> the exposure bar visibly longer than both, with the shortfall bracketed and expressed as a share
> of equity.

### 12.1.3 Performance damages, buy-down and the three make-good numbers

**Definition.** **Performance liquidated damages** compensate for a plant that completes but
underperforms (output, efficiency, availability or consumption below guarantee). Where the
shortfall is permanent they are commonly structured as a **buy-down**: a lump sum, usually
applied to prepay debt, calibrated so that the financing survives a smaller plant. The question
nobody asks early enough is *calibrated to restore what* — because there are three defensible
answers and they differ by an order of magnitude.

Kestrel's cash flow is linear in output, which makes the arithmetic clean. With revenue of
12,000,000 at full output, cash operating costs of 4,500,000 of which **85 % is fixed**
(3,825,000), depreciation 2,400,000, year-one interest 2,520,000, cash tax at 20 % and a
working-capital movement of 600,000 (Domain 5, KA 5.4.3 established this cost structure and the
**1.510× operating leverage** it produces), `CFADS` as a function of output share `x` is:

```
CFADS(x) = 9,060,000 x − 2,676,000
```

so **each percentage point of output is worth 90,600 of annual `CFADS`**. At `x` = 1 the line
reproduces the documented 6,384,000; at 0.97 it reproduces Domain 5's 6,112,200.

**Worked example 12.1.3 — what should a 5 % output shortfall be worth?**

1. **Setup.** Kestrel completes at **95 %** of guaranteed output, permanently. `EBITDA` falls to
   **6,933,750** and `CFADS` to **5,931,000** (an annual shortfall of **453,000**). Remaining
   operating life 25 years; the sponsors' appraisal rate is Domain 4's **8 %**; the performance
   damages cap is **4,800,000**. Compute the value of the shortfall, the damages rate it
   implies, the shortfall the cap can cover, and the two rival make-good figures.
2. **Formula.** Value of the shortfall = annual `CFADS` shortfall × `AF(0.08, 25)`. Damages rate
   per point = 90,600 × `AF(0.08, 25)`. Coverage-restoring buy-down = debt ×
   (`CFADS` shortfall ÷ base `CFADS`), first-order as in Domain 5, KA 5.4.3. Covenant-restoring
   prepayment: reduce debt until `CFADS`(new) ÷ (debt ÷ `AF(0.06, 12)`) = 1.20.
3. **Substitution.** `AF(0.08, 25) = 10.674776`; `453,000 × 10.674776`; `90,600 × 10.674776`;
   `42,000,000 × 453,000/6,384,000`; `5,931,000/1.20 = 4,942,500`, then `× 8.383844`.
4. **Result.**

   | Basis | What it restores | Amount |
   |---|---|---|
   | Bare covenant | `DSCR` back to the 1.20 covenant, nothing more | **562,851.03** (debt to 41,437,148.97) |
   | Sized coverage | `DSCR` back to the 1.2743 the debt was sized on | **2,980,263.16** (debt to 39,019,736.84) |
   | Value | The present value of 25 years of lost `CFADS` | **4,835,673.53** |

   The value basis implies a damages rate of **967,134.71 per percentage point** of shortfall, and
   the **4,800,000 cap therefore covers a shortfall of 4.9631 %** and not one basis point more.
   For reference, the covenant itself fails at a shortfall of **4.1108 %** (the point at which
   90,600 per point exhausts the 372,438 of headroom): at 5 % the `DSCR` is **1.1839**, breaching
   the 1.20 covenant by **80,562** of cash.
5. **Interpretation.** The spread between 562,851 and 4,835,674 is **8.591×**, and every party
   at the table has a favourite end of it. **Lenders are satisfied by the smallest number**,
   because their interest ends at the covenant, which is why a facility agreement that merely
   requires "performance damages sufficient to restore the covenant" protects the banks and
   abandons the equity. **Sponsors need the largest**, because their loss is 25 years of cash,
   not 12 years of coverage. **The sized-coverage basis, at 2,980,263, is the negotiated
   middle** and the one most commonly drafted, which means the standard market outcome quietly
   transfers 1,855,410 of value loss from the contractor to the equity. A sponsor that
   understands this asks for the value basis and settles for the sized basis knowing what it
   conceded; a sponsor that does not, discovers the gap in operations.

   **The application clause matters as much as the amount**, and this is the subtler and more
   expensive point. Suppose the full 4,800,000 is received and, as facility agreements usually
   require, applied to **mandatory prepayment**. Debt falls to 37,200,000, the instalment falls to
   **4,437,105.46** — a relief of **572,529.77** per year — and the `DSCR` rises to **1.3367**,
   *better* than the position the debt was sized on. Distributions actually rise, from 1,374,365
   to **1,493,895**. And yet equity is still worse off: the relief runs for the loan's remaining
   12 years and is worth `572,529.77 × AF(0.08, 12) = 7.536078` → **4,314,628.99**, while the loss
   runs for 25 years and is worth 4,835,673.53. The residual gap is **521,044.53**. *Paying a
   25-year loss with a 12-year debt prepayment under-compensates by construction*, however
   generous the headline. The remedies are to direct part of the proceeds to a distribution
   account or a maintenance reserve rather than wholly to prepayment, or to size the damages on the
   value basis in the first place — a drafting choice worth half a million dollars that costs
   nothing to make at signature and cannot be made afterwards.

   The caution: this arithmetic assumes the shortfall is permanent, the cost structure holds and
   the 8 % rate is the right measure of the sponsors' loss. Where the shortfall can be engineered
   out, the honest comparison is the rectification cost against the damages; where the offtake
   pays for availability rather than output, the loss runs through the deduction regime instead
   (KA 12.2) and this calculation must be rebuilt on that basis.

### 12.1.4 O&M contracts and the liability asymmetry

**Definition.** An **operation and maintenance agreement** engages an operator to run the asset to
defined standards for a fee, usually with a fixed element, a variable element and a performance
regime of bonuses and deductions. Structurally it is the mirror of the EPC contract, and it
carries one systematic defect a financier must look for: **the liability asymmetry**.

The asymmetry is arithmetic. An O&M contractor's annual fee on a project like Kestrel might be
1,200,000: roughly 27 % of the 4,500,000 cash operating cost, the rest being power, chemicals
and insurance. Its liability cap is customarily expressed as a share of that fee: one year's
fee, or 50 % of it, is common. But the loss an operator can cause is a *revenue* loss, measured
on the same daily basis as a construction delay. A **30-day** unplanned outage costs Kestrel
`24,733.33 × 30 = ` **742,000**, which already exceeds a cap set at 50 % of the fee (600,000) by
**142,000**; a **60-day** outage costs **1,484,000**, leaving **884,000** uncovered against that
cap and **284,000** uncovered even against a full-year-fee cap of 1,200,000. The consequence is
that **operating risk is only nominally transferred**: the O&M agreement buys competence,
mobilisation and a set of key performance indicators, not indemnity. Lenders know this, which is
why they price operating risk in the coverage ratio and the maintenance reserve (Domain 10, KA
10.3.1) rather than relying on the O&M contract, and why an availability guarantee from a thinly
capitalised operator adds less bankability than sponsors expect.

Three structural responses are worth naming. A **parent guarantee or performance bond** from the
operator's group raises the recoverable amount to something comparable with the loss (KA 12.3).
A **deduction regime that bites on the fee before the cap**, availability-linked fee at risk,
gives the operator a running incentive rather than a terminal liability. And **a step-in and
replacement right**, with a pre-qualified substitute operator and a transition plan, is often
worth more than any cap: the value at stake is continuity of revenue, and the fastest route to
continuity is a new operator, not a claim against the old one. Where the O&M contractor is an
affiliate of the sponsor, very common, the conflict must be disclosed and the agreement tested
on arm's-length terms, because a lender is being asked to accept a related-party contract as a
bankability item (Domain 1, KA 1.3.3).

### AI in this KA

**Where it earns its place.** Extracting the six financial limits of 12.1.1 from a 400-page
contract and its schedules, across a portfolio, is exactly the work machines should do: rate,
cap, cap-binding day, aggregate cap, exclusions, security instruments and their expiry dates,
assembled into a structured table with clause references. So is the consistency check between
that table and the financial model: an assistant that reads both and flags "the model recovers
6,000,000 of delay damages; the contract caps them at 4,800,000" has earned its licence fee in
one line. And so is generating the stress grid: exposure and residue across a matrix of delay
days and output shortfalls, which is 400 arithmetic cells nobody computes by hand.

**Where it must not go.** It must not conclude whether a damages provision is enforceable, whether
a cap survives a particular breach, what "consequential loss" excludes in a given jurisdiction, or
whether a set of facts triggers a termination right. Those are legal conclusions, they are
jurisdiction-specific, and they belong to qualified counsel; a model's confident answer on any of
them is a liability dressed as a summary. Nor should it draft the damages number: the calibration
of 12.1.3 rests on a judgment about whose loss is being restored, which is a commercial decision
with a named owner.

**Verification, concretely.** Sample the extracted limits back to the clause on at least five
items per contract, including every cap, and record the human verifier and date. Recompute one
cap-binding day and one residue by hand. Confirm that the exclusions list in the extraction
matches the contract's own list word for word rather than a normalised paraphrase. The
normalising is where the error enters. **AI proposes; the professional verifies, decides and
remains accountable.**

### Key terms — KA 12.1

| Term | Meaning |
|---|---|
| **EPC wrap** | Single-contractor fixed-price, date-certain, turnkey responsibility for the whole works. |
| **Delay liquidated damages** | Pre-agreed sum per unit of time of late completion; calibrated on carrying cost plus forgone `CFADS`. |
| **Cap-binding day** | Delay cap ÷ daily damages rate; the day after which further delay is uncompensated. |
| **Cap stack** | The interacting set of sub-caps and the aggregate cap; the aggregate is the real limit. |
| **Performance liquidated damages / buy-down** | Payment for permanent underperformance, usually applied to prepay debt. |
| **Uncovered residue** | Exposure less contractual recovery; an equity number, and a disclosure item. |
| **Liability asymmetry (O&M)** | Operator liability capped on fee while the loss it can cause is revenue-scaled. |

### Sample MCQs — KA 12.1

**MCQ 12.1-A `[12.1.2 · Application]`** Delay damages are 20,000 per day, capped at 10 % of a
48,000,000 EPC price, against a daily economic cost of delay of 24,733.33. For a 300-day delay the
amount borne by the project company is:
- A. USD 1,420,000
- B. USD 2,620,000 ✅
- C. USD 1,484,000
- D. nil — the damages regime covers it

*Rationale:* Economic cost `24,733.33 × 300 = 7,420,000`; recovery is capped at 4,800,000 (the cap
binds at day 240), so 2,620,000 is uncovered. A applies the 4,733.33 per day uncovered *rate* to
300 days and ignores that the cap stops recovery entirely after day 240. C is the 60-day interface
figure of 12.2.4. D assumes a cap covers any delay.

**MCQ 12.1-B `[12.1.2 · Analysis]`** An EPC contract has a 10 % delay-damages sub-cap, a 10 %
performance sub-cap and a 20 % aggregate liability cap. The correct reading is:
- A. total recoverable liability is 40 % of the contract price
- B. the aggregate cap adds protection above the sub-caps
- C. the two sub-caps can exhaust the aggregate cap exactly, leaving no room for any third head of claim ✅
- D. the aggregate cap applies only to indemnities

*Rationale:* 10 % + 10 % = 20 %, so once both sub-caps are drawn the aggregate is fully consumed
and a later defect or indemnity claim recovers nothing (12.1.1, 12.1.2). A double-counts; B is the
common misreading the arithmetic disproves; D asserts a carve-out the structure does not contain.

**MCQ 12.1-C `[12.1.3 · Application]`** Kestrel's `CFADS` falls by 453,000 per year for 25 years
on a 5 % output shortfall; the appraisal rate is 8 % (`AF(0.08, 25) = 10.674776`); debt is
42,000,000 against base `CFADS` of 6,384,000 and an instalment of 5,009,635.23. Which figure is
the **value-basis** performance damages amount?
- A. USD 562,851 (the prepayment that restores the 1.20 covenant)
- B. USD 2,980,263 (the buy-down that restores the sized 1.2743 `DSCR`)
- C. USD 4,835,674 ✅
- D. USD 453,000 (one year of lost `CFADS`)

*Rationale:* `453,000 × 10.674776 = 4,835,673.53`. A restores only the covenant, B only the
sized coverage. Both are lender-facing measures, not the sponsors' loss; D omits discounting and
the remaining 24 years.

**MCQ 12.1-D `[12.1.3 · Analysis]`** A 4,800,000 performance damages receipt is applied wholly to
mandatory prepayment, cutting debt to 37,200,000 and the instalment to 4,437,105, and lifting the
`DSCR` to 1.3367. The soundest conclusion is:
- A. equity has been over-compensated, since the `DSCR` now exceeds the sized 1.2743
- B. equity remains short by about 521,000, because a 25-year loss has been compensated with 12 years of debt-service relief ✅
- C. equity is exactly compensated, since the amount equals the cap
- D. the prepayment is irrelevant to equity

*Rationale:* Relief of 572,529.77 × `AF(0.08, 12) = 7.536078` is 4,314,629 against a loss of
4,835,674 — a residual gap of 521,045 (12.1.3). A confuses a coverage ratio with value; C confuses
the cap with the loss; D ignores that prepayment raises distributions.

**MCQ 12.1-E `[12.1.2 · Evaluation]`** Kestrel's delay damages run at 20,000 per day under a
4,800,000 cap. With limited negotiating capital left before signature, the soundest priority is to:
- A. press for a higher daily rate, since a higher rate raises recovery on every day of delay
- B. press for the 20 % aggregate cap to be raised, since the aggregate is the real limit and a higher
  aggregate extends delay recovery
- C. compare the cap-binding day (4,800,000 ÷ 20,000, day 240) with the credible worst-case
  delay from the schedule risk analysis, and pair the cap with a termination-for-delay right at
  a long-stop date, because beyond the cap the contractor's marginal cost of a further day is
  zero ✅
- D. accept the regime, since a 10 % delay cap under a 20 % aggregate is conventional

*Rationale:* The rate governs recovery only to the cap-binding day, after which further delay is
wholly uncompensated (2,620,000 on a 300-day slip), so a cap that binds before the P80 delay
does not cover the risk it was bought for (12.1.2). A spends leverage where sponsors habitually
spend it and where it buys least. B is a true point misapplied: the aggregate binds only when a
*third* head of claim arises, and raising it extends the delay sub-cap by not one day. D
substitutes market convention for the project's own schedule evidence, which is the only thing
that can calibrate a cap.

**MCQ 12.1-F `[12.1.4 · Comprehension]`** The "liability asymmetry" in an O&M agreement means that:
- A. the operator's bonuses exceed the deductions it can suffer
- B. the cap binds the project company but not the operator
- C. the operator's liability outlasts its appointment
- D. the operator's liability cap is scaled to its fee while the loss its failure causes is scaled to
  the project's revenue, so the two are measured on different bases and the cap is smaller than the
  loss by construction ✅

*Rationale:* A cap expressed as one year's fee, or half of it, stands against an outage cost measured
on the same daily basis as a construction delay, so a 30-day outage already exceeds a half-fee cap
(12.1.4). That is why lenders price operating risk in the coverage ratio and the maintenance reserve
rather than relying on the O&M contract. A describes an incentive regime, not a cap. C describes a
survival period. B misstates whom the cap protects.

**MCQ 12.1-G `[12.1.3 · Evaluation]`** Kestrel's negotiator proposes to accept performance damages on
the sized-coverage basis, **2,980,263**, describing it as the market standard. The value of the
sponsors' loss on a permanent 5 % output shortfall is **4,835,674**; the bare covenant-restoring figure
is **562,851**; the performance sub-cap is 4,800,000. The recommendation to the investment committee
should be:
- A. ask for the value basis and, if the sized basis is conceded, direct part of the proceeds
  away from mandatory prepayment, because the sized basis gives up **1,855,410** of value loss,
  and even the full 4,800,000 applied wholly to prepayment under-compensates equity by
  **521,045** ✅
- B. accept 2,980,263: it restores the 1.2743 the debt was sized on, which is the standard the
  financing was built to
- C. accept 562,851: the covenant is the only contractual test, so anything above it is a windfall
- D. insist on 4,835,674, which the sub-cap makes deliverable

*Rationale:* The three bases restore three different things, and only the value basis restores
the sponsors' loss, so the ask and the fallback should each be stated with what it concedes
(12.1.3). B is the defensible weaker course (it is the common drafting and the coverage argument
supports it), and it transfers 1,855,410 of value loss to equity without saying so. C adopts the
lenders' interest as the equity case, an understatement of 8.591×. D overstates what is
available: 4,835,674 exceeds the 4,800,000 sub-cap, so the value basis requires the cap to move
as well as the calibration, and asking for the number without the cap is asking for 4,800,000.
The application point in A is the one that costs nothing at signature and cannot be made
afterwards: a 25-year loss compensated with 12 years of debt-service relief is short by
construction, however generous the headline.

### Self-check — KA 12.1

1. *Which negotiating variable matters more, the damages rate or the cap, and why?* The cap: the
   rate governs recovery only up to the cap-binding day (day 240 for Kestrel), after which
   further delay is wholly uncompensated.
2. *Name the three make-good bases for a permanent output shortfall and Kestrel's figures.* —
   Bare covenant 562,851; sized coverage 2,980,263; value 4,835,674 — a spread of 8.591×.
3. *Why does an O&M liability cap rarely transfer operating risk?* It is scaled to the fee while
   the loss it can cause is scaled to revenue; the practical protections are guarantees, fee at
   risk and a step-in replacement right.

---

## Knowledge Area 12.2 — Offtake, concession, supply and interface agreements

*Topics: 12.2.1 the four load-bearing terms of a revenue contract · 12.2.2 the contracted volume
floor is a financing parameter · 12.2.3 concession termination compensation · 12.2.4 supply,
interface and the hole between packages.*

### 12.2.1 The four load-bearing terms of a revenue contract

**Definition.** The **offtake agreement** (a power purchase agreement, water purchase agreement,
capacity or availability agreement, or a concession's tariff regime) is the contract that
manufactures the project's cash flow. Domain 7 built its economics; this domain reads it as a
document, and four of its terms carry the entire financing:

**The volume or availability commitment.** Whether the offtaker must pay for output it does not
take (**take-or-pay**) or only for output delivered (**take-and-pay**), and at what level. This is
the term of 12.2.2, and it is systematically under-tested.

**The price and its indexation.** The tariff, its escalation formula and the pass-through of
input costs. Domain 11 (KA 11.2.3) demonstrated the cost of an **indexation mismatch** (a
revenue index that does not track the cost driver it is meant to fund), and that analysis is not
repeated here; the contractual point is that the mismatch is created by the words in the
escalation schedule, so the schedule must be read against the operating cost build-up line by
line, not accepted as "CPI-linked".

**The deduction and abatement regime.** What reduces payment: unavailability, quality failures,
delivery-point failures, metering disputes. A deduction regime is a liability cap in reverse:
uncapped, running annually, and biting on revenue before any of the project's own protections
engage. The single most useful test is to compute the **maximum annual deduction** the regime
permits and compare it with the covenant headroom: for Kestrel, a deduction exceeding 372,438 in
any year breaches the 1.20 covenant regardless of how well the plant ran in every other respect.

**Termination and compensation.** What is paid, by whom, on whose default (12.2.3). For a lender
this is the most important clause in the document, because it is the only one that speaks to the
recovery of principal.

### 12.2.2 The contracted volume floor is a financing parameter

**The claim.** Commercial teams negotiate the take-or-pay level as a commercial matter; how much
volume will the offtaker commit to, how much flexibility does it need. It is not a commercial
matter. **The minimum contracted volume is determined by the covenant**, and it is computable
from the cash-flow line and the debt service before anyone sits down.

**Worked example 12.2.2 — what floor does Kestrel's covenant actually require?**

1. **Setup.** `CFADS(x) = 9,060,000x − 2,676,000` from 12.1.3, where `x` is the share of
   guaranteed output taken and paid for. Debt service **5,009,635.23**; covenant **1.20×**;
   lock-up **1.15×**. The commercial team has agreed a take-or-pay floor of **90 %** of capacity.
   Find the `DSCR` at that floor, the floor the covenant requires, the floor the lock-up requires,
   and the floors at which coverage and cash reach unity and zero.
2. **Formula.** `DSCR(x)` = `CFADS(x)` ÷ debt service. Inverting for a target ratio `k`:
   `x = (k × 5,009,635.23 + 2,676,000) ÷ 9,060,000`.
3. **Substitution.** At `x` = 0.90: `(8,154,000 − 2,676,000)/5,009,635.23`. For `k` = 1.20:
   `(6,011,562.28 + 2,676,000)/9,060,000`. Similarly for 1.15, 1.00 and `CFADS` = 0.
4. **Result.**

   | Contracted floor | `CFADS` | `DSCR` | Status |
   |---|---|---|---|
   | 100.0000 % | 6,384,000 | **1.2743** | The sized case |
   | **95.8892 %** | 6,011,562 | **1.2000** | Covenant exactly met |
   | 95.0000 % | 5,931,000 | 1.1839 | **Covenant breached** |
   | **93.1245 %** | 5,761,080 | **1.1500** | Lock-up trigger |
   | 90.0000 % | 5,478,000 | **1.0935** | Breach and lock-up; debt still paid |
   | **84.8304 %** | 5,009,635 | **1.0000** | Cash exactly equals debt service |
   | 29.5364 % | 0 | — | `CFADS` exhausted |
5. **Interpretation.** **The negotiated 90 % floor does not support the project's own
   covenant**, and the gap is not marginal: 90 % delivers 1.0935 against a 1.20 requirement, a
   breach and an automatic distribution lock-up from the first test date, on a plant performing
   exactly to specification and an offtaker performing exactly to contract. The floor the
   financing needs is **95.8892 %**, which, expressed the way it must be expressed to a
   commercial negotiator, means the offtaker's flexibility is worth **4.11 percentage points of
   capacity, not ten**.

   The reason the intuition fails is **operating leverage compounded by fixed debt service**.
   Because 85 % of cash operating cost is fixed, a 10 % volume reduction cuts `CFADS` by 14.19 %
   (906,000 on 6,384,000), and because debt service does not move at all, the whole reduction lands
   on the ratio. The general form is worth carrying: `DSCR` is linear in contracted volume with a
   slope of `90,600 ÷ 5,009,635.23 = 0.0181` of ratio per percentage point, so **each point of
   contracted volume conceded costs 0.0181 of coverage** — and the 372,438 of headroom buys 4.11
   points and no more.

   Three professional consequences. **The floor is a financing deliverable, not a commercial
   preference**, and it should be issued to the commercial team as a constraint with its
   derivation attached, before negotiation, in the same way a lender issues a target `DSCR`. **A
   lower floor has a price, and the price is computable**: if the offtaker will only commit to
   90 %, the project needs either more equity (reducing debt service until 90 % clears 1.20 —
   debt of `5,478,000/1.20 × 8.383844 = ` **38,272,248**, so **3,727,752** of additional
   equity), or a compensating floor price, or a volume-shortfall payment that is economically
   take-or-pay under another name. **And the two thresholds must both be mapped onto volume**:
   the covenant fails at 95.8892 % and the lock-up engages **2.76 points of volume lower**, at
   93.1245 %, so between those two floors the project is in breach (with all the consequences of
   Domain 10, KA 10.4), while distributions are not yet automatically trapped. A sponsor who
   models only the lock-up will therefore believe it has more contractual room than it has,
   which is the mirror image of the error Domain 10's Case study B punished.

   The caution: this arithmetic is year-one, on a level `CFADS` assumption, and it takes the
   cost structure as fixed. A ramping or seasonal offtake requires the same test **period by
   period** against the sculpted debt-service profile (Domain 10, KA 10.1.3), and the binding
   period (almost always the first full year or the year of a major maintenance outage) is the
   one the floor must clear.

> **Fig 12.2.1 — The contracted volume floor a covenant requires.** Line chart, x-axis contracted
> volume as a share of guaranteed output from 80 % to 102 %, y-axis year-one `DSCR` from 0.90 to
> 1.32, showing the straight line `DSCR(x) = (9,060,000x − 2,676,000)/5,009,635.23` with a slope of
> 0.0181 of coverage per percentage point. Horizontal dashed references at the 1.20 covenant and
> the 1.15 lock-up. A crimson marker at 95.8892 % (covenant met exactly, `CFADS` 6,011,562), slate
> markers at 93.1245 % (lock-up) and 84.8304 % (`DSCR` 1.0000), a blue marker at 100 % (1.2743, the
> sized case), and a crimson marker at the commercially negotiated 90 % floor showing 1.0935, with a
> bracket labelled "5.8892 points of volume short of the covenant". Source: PCI
> original. Alt text: a straight rising line of debt service coverage against contracted volume,
> crossing the covenant threshold at just under ninety-six per cent, with the ninety per cent
> negotiated floor sitting well below the covenant line.

### 12.2.3 Concession termination compensation

**Definition.** A **concession agreement** grants the project company the right to build, operate
and charge for an asset for a defined term, subject to obligations and reversion. Its
**compensation-on-termination** provisions state what the grantor pays if the arrangement ends
early, and they are conventionally graded by cause: grantor default or voluntary termination
(most generous, typically debt plus equity plus a return); force majeure or extended
unavailability of the asset (intermediate, often debt-focused); and project-company default (least
generous, sometimes a market-value or debt-haircut formula).

The financier's test is arithmetic and takes ten minutes. **Compare the compensation formula, at
each year of the term, with the debt outstanding on that date.** Kestrel's 12-year amortisation
gives:

| End of year | Interest | Principal | Debt outstanding |
|---|---|---|---|
| 1 | 2,520,000.00 | 2,489,635.23 | **39,510,364.77** |
| 3 | 2,212,281.09 | 2,797,354.14 | **34,073,997.28** |
| 5 | 1,866,528.11 | 3,143,107.12 | **27,965,694.77** |
| 8 | 1,266,144.36 | 3,743,490.87 | **17,358,915.21** |
| 12 | 283,564.26 | 4,726,070.97 | **0** |

Two consequences follow. First, **a formula that pays a percentage of debt outstanding fails
worst early**: an 85 %-of-debt formula on project-company default leaves a gap of **5,926,555**
at the end of year 1 and **4,194,854** at the end of year 5, and the early years are precisely
when default risk is highest, so the haircut bites exactly when it is most likely to be needed.
Lenders respond by requiring compensation at least equal to senior debt outstanding plus
breakage in the grantor-default and force-majeure cases, and by treating the project-company
default case as a real exposure to be sized rather than a theoretical one.

Second, **a formula that covers debt in full makes the lenders whole and leaves equity at zero**,
and this is the single most important number in a concession for an equity investor. If Kestrel's
concession terminated for force majeure at the end of year 5 on a debt-outstanding formula, the
lenders recover 27,965,695 and the sponsors recover nothing against 18,000,000 contributed less
the **6,871,824** of nominal distributions received in five years (1,374,365 per year) — an
unreturned **11,128,176**. That figure, not the elegance of the clause, is what an investment
committee needs, and it is why sponsors negotiate hardest on the definition of equity base,
whether a return is payable on it, and the treatment of subordinated debt.

Whether a compensation formula is enforceable as drafted, how it interacts with statutory
compensation or public-procurement rules, whether the grantor's payment obligation ranks ahead of
other public liabilities, and what happens on the grantor's own insolvency are **matters of local
law and public policy that vary fundamentally between jurisdictions**. They must be confirmed by
qualified local counsel before the model treats a termination sum as a recovery.

### 12.2.4 Supply, interface and the hole between packages

**Definition.** **Supply agreements** secure inputs (fuel, feedstock, chemicals, grid
connection, raw water) on terms whose volume, price, indexation and interruption provisions
mirror the offtake in reverse. **Interface agreements** allocate responsibility where two or
more contractors, utilities or authorities must physically or programmatically connect. The
financier's interest in both is the same: **an input failure or an interface failure produces a
revenue loss that the offtake's deduction regime charges to the project company, and the
question is who reimburses it.**

Where the works are delivered under a single EPC wrap, interface risk sits inside the wrap;
where they are split, it sits nowhere. That is not rhetoric but arithmetic. Had Kestrel's
48,000,000 been split into an 8,000,000 marine intake package and a 40,000,000 process plant
package, a 60-day delay caused by the intake being handed over late to a specification the
process contractor disputed would cost the project company `24,733.33 × 60 = ` **1,484,000**
(**3.092 %** of the combined contract price) with **no recovery against either contractor**,
because each would demonstrate that it performed its own scope and each would claim an extension
of time and prolongation cost for the other's delay. The wrap premium Domain 11 (KA 11.1.3)
priced is, in part, the price of not owning that hole.

Splitting packages is nevertheless often right; it can be cheaper, it can be the only route to a
specialist scope, and Domain 11 established that buying a transfer is not automatically value
creating. What is never right is splitting them **without an interface regime**. The minimum
package: a single **interface matrix** identifying every physical, programme, data and
documentation handover with a named owner and a date; **back-to-back completion obligations** so
that one contractor's milestone is defined by the other's readiness; **cross-liability or
knock-for-knock provisions** stating who pays whom when a handover slips; an **interface
manager** with authority inside the project company; and **a single float owner**, because
unallocated float is the commonest cause of concurrent claims (Domain 8, KA 8.4). Case study B
below prices exactly this omission on a digital-infrastructure project at **5,302,717**.

### AI in this KA

**Where it earns its place.** Reading a long tariff and deduction schedule and reconstructing the
payment formula as an auditable calculation is high-value machine work, and so is the reverse
check: does the model's revenue line reproduce the contract's formula on a sample of historical or
test-case months? Extracting every termination and compensation provision from a concession and
tabulating it by cause, with the payment basis and the clause reference, turns a week of associate
time into an afternoon of review. And the volume-floor computation of 12.2.2 should be automated
across every revenue contract in a portfolio, because it is mechanical and nobody does it.

**Where it must not go.** It must not decide whether a deduction is contractually due in a
disputed month, whether a force-majeure definition captures a particular event, or whether a
termination notice is valid. Those are contract-interpretation questions with legal consequences
and named counsel. Nor should it be trusted to normalise a bespoke tariff formula into a standard
shape: the variations between formulae are where the money is, and a tidy summary is precisely the
wrong output.

**Verification, concretely.** Re-derive the payment for two contract months by hand from the
clause and compare with the extraction. Confirm that the extracted deduction regime's worst annual
case has been computed and compared with covenant headroom. Sample every termination row back to
its clause, and record counsel's confirmation that the compensation basis is enforceable as
modelled before the recovery appears in a lender case.

### Key terms — KA 12.2

| Term | Meaning |
|---|---|
| **Take-or-pay / take-and-pay** | Offtaker pays for contracted volume whether taken, or only for volume delivered. |
| **Contracted volume floor** | The minimum committed volume; a financing parameter set by the covenant, not a commercial preference. |
| **Deduction / abatement regime** | Contractual reductions in payment for unavailability or quality failure; an uncapped, annually recurring exposure. |
| **Compensation on termination** | What the grantor or offtaker pays on early termination, graded by cause. |
| **Interface agreement** | Allocation of responsibility at handovers between separately contracted packages. |
| **Interface hole** | Loss caused by a handover failure that neither contractor is liable for. |

### Sample MCQs — KA 12.2

**MCQ 12.2-A `[12.2.2 · Application]`** `CFADS(x) = 9,060,000x − 2,676,000`; debt service is
5,009,635.23; the covenant is 1.20×. The minimum contracted volume that holds the covenant is:
- A. 90.0000 %
- B. 84.8304 %
- C. 95.8892 % ✅
- D. 100.0000 %

*Rationale:* `(1.20 × 5,009,635.23 + 2,676,000)/9,060,000 = 0.958892`. A is the commercially
negotiated floor, which delivers only 1.0935; B is the volume at which `DSCR` = 1.00, i.e. cash
merely equals debt service; D is the sized case, which the covenant does not require.

**MCQ 12.2-B `[12.2.2 · Analysis]`** Why does a 10 % reduction in contracted volume cut `DSCR` by
far more than 10 %?
- A. because the covenant is tested on revenue, not `CFADS`
- B. because 85 % of cash operating cost is fixed, so `CFADS` falls 14.19 %, and debt service does not fall at all ✅
- C. because interest rates rise with lower volume
- D. because the tariff falls with volume

*Rationale:* Operating leverage compounded by a fixed denominator: 906,000 of `CFADS` lost on
6,384,000 is 14.19 %, and all of it lands on the ratio (12.2.2, and the 1.510× leverage of
Domain 5, KA 5.4.3). A misstates the test basis; C and D invent mechanisms.

**MCQ 12.2-C `[12.2.3 · Analysis]`** A concession pays 85 % of senior debt outstanding on
project-company default. Kestrel's debt outstanding is 39,510,365 at the end of year 1 and
27,965,695 at the end of year 5. The most important observation is:
- A. the formula is adequate, since 85 % is a high recovery
- B. the shortfall is largest early (5,926,555 at the end of year 1) precisely when default risk
  is highest ✅
- C. the shortfall is largest late, as the debt amortises
- D. the formula protects equity but not lenders

*Rationale:* 15 % of a declining balance is largest at the start (12.2.3). C inverts the
profile; D reverses the ranking: a debt-based formula protects lenders first and leaves equity
at zero.

**MCQ 12.2-D `[12.2.4 · Application]`** Two packages are let without an interface regime; a 60-day
handover dispute delays completion. The daily economic cost of delay is 24,733.33. The project
company's most likely position is:
- A. it recovers 1,484,000 from the contractor whose scope was late
- B. it bears 1,484,000, recovers nothing, and faces prolongation claims from both contractors ✅
- C. it recovers from its insurers under a delay-in-start-up policy in all cases
- D. it suffers no loss, because the offtake date moves with the works

*Rationale:* Each contractor shows performance of its own scope and claims for the other's delay
(12.2.4). A assumes an allocation the documents do not make; C assumes cover that depends entirely
on policy wording and a triggering insured peril; D assumes an offtake flexibility that
date-certain revenue contracts do not grant.

**MCQ 12.2-E `[12.2.2 · Evaluation]`** The commercial team has agreed a take-or-pay floor of 90
% of capacity (a `DSCR` of 1.0935 against a 1.20× covenant that requires 95.8892 %), and asks
the finance lead to sign it off so the bid can go in. The soundest professional response is:
- A. sign it off: 90 % is a strong commercial outcome and a covenant can be reset at close
- B. decline the transaction, since any floor below 100 % is unbankable
- C. decline to treat the floor as a commercial term, restate it as a financing constraint with
  its derivation attached, and price the alternatives: 3,727,752 of additional equity, a
  compensating floor price, or a volume-shortfall payment that is take-or-pay under another name
  ✅
- D. sign it off and rely on the 1.15× lock-up trigger, which sits at 93.1245 % of capacity

*Rationale:* The floor is a financing deliverable computable before anyone sits down: each point
of contracted volume is worth 0.0181 of coverage, so 372,438 of headroom buys 4.11 points and
not ten (12.2.2). A concedes a breach and an automatic distribution lock-up from the first test
date, on a plant performing exactly to specification and an offtaker performing exactly to
contract. B is the opposite failure of judgment; the covenant requires 95.8892 %, not the sized
case. D is self-defeating as well as misdirected: 90 % sits below the lock-up floor too, and a
covenant breach is an event of default whether or not cash is trapped.

**MCQ 12.2-F `[12.2.1 · Evaluation]`** A draft water purchase agreement has been reviewed by the
commercial team, which reports the tariff, its indexation and the take-or-pay level as agreed and the
deduction and abatement schedule as "operational detail for the O&M team". The financier's first
intervention should be to:
- A. accept the division of labour: deductions are operational, and the O&M team is closer to the
  metering and quality regime than the finance function
- B. re-open the indexation schedule instead, since an indexation mismatch is the larger structural risk
- C. require the deduction regime to be capped at the covenant headroom figure, since anything larger is
  unbankable
- D. compute the maximum annual deduction the regime permits and compare it with covenant
  headroom. A deduction regime is a liability cap in reverse, uncapped and running annually, and
  on Kestrel a deduction exceeding **372,438** in any year breaches the 1.20× covenant however
  well the plant performed in every other respect ✅

*Rationale:* Of the four load-bearing terms of a revenue contract, the deduction regime is the
one with no ceiling and the one that bites on revenue before any of the project's own
protections engage, so its worst annual case is a financing number and not an operating one
(12.2.1). A hands the covenant to a team that is not measured on it. C is the defensible weaker
course and the right thing to *ask* for second: a cap on aggregate annual deductions is a
legitimate negotiating position, and it is not achievable at the headroom figure in most
markets, so computing the exposure has to come first. B names a genuine and larger risk that
Domain 11 (KA 11.2.3) prices, and the indexation schedule is reported as agreed, while the
deduction schedule has not been read by anyone with a covenant to protect.

**MCQ 12.2-G `[12.2.3 · Comprehension]`** A grantor's adviser describes a compensation-on-termination
formula measured on senior debt outstanding as one that "makes everybody whole". The accurate
restatement is:
- A. it makes the lenders whole and pays equity nothing. It is a lender-recovery formula, and
  any return of, or return on, the equity base has to be provided for separately ✅
- B. it makes everybody whole, since equity ranks behind the debt and is paid from the same sum
- C. it makes equity whole and leaves the lenders exposed for their breakage costs
- D. it makes nobody whole, because debt outstanding is always less than the amount originally advanced

*Rationale:* The formula is measured on the debt and stops there: on a force-majeure termination
at the end of year five Kestrel's lenders recover 27,965,695 and the sponsors' unreturned
**11,128,176** is lost (12.2.3). That is why sponsors negotiate the definition of the equity
base, whether a return accrues on it, and the treatment of subordinated debt. B assumes a
residual the formula does not create. C reverses the ranking. D confuses amortisation with
impairment: the sum tracks what is owed on the date, which is exactly what a lender needs.
Whether any such formula is enforceable as drafted, and how it interacts with local
public-procurement and insolvency rules, is a matter for qualified local counsel.

### Self-check — KA 12.2

1. *What sets the minimum take-or-pay level?* The covenant: `x = (k × debt service + 2,676,000)
   ÷ 9,060,000`; for Kestrel's 1.20× that is 95.8892 %, against a negotiated 90 %.
2. *What does one point of contracted volume cost in coverage?* 0.0181 of `DSCR` (90,600 ÷
   5,009,635.23), so 372,438 of headroom buys 4.11 points.
3. *What does a debt-outstanding termination formula give equity?* Nothing: at the end of year 5
   the lenders take 27,965,695 and the sponsors' unreturned 11,128,176 is lost.

---

## Knowledge Area 12.3 — Guarantees, direct agreements and the security package

*Topics: 12.3.1 the instrument family · 12.3.2 guarantee sufficiency: cap times credit quality ·
12.3.3 direct agreements and step-in · 12.3.4 what the security package is actually worth.*

### 12.3.1 The instrument family

**Definition.** The **security package** is the set of instruments and rights that convert
contractual promises into recoveries. It has two halves that are routinely confused. The
**credit-support instruments** stand behind counterparties' obligations to the project company;
the **lenders' security** stands behind the project company's obligations to the lenders.

| Instrument | Who provides it | What it does | Commercial character |
|---|---|---|---|
| On-demand bond / standby letter of credit | A bank, on the counterparty's account | Pays on a compliant demand, without proof of breach | Bank credit; near-certain payment; costs a fee |
| Conditional (surety) bond | A surety or insurer | Pays on proof of default and loss | Slower, defensible, cheaper |
| Parent company guarantee | The counterparty's group parent | Extends the obligation to a bigger balance sheet | Worth the parent's credit, no more |
| Retention / retention bond | Withheld from payments, or bonded | Funds defect rectification | Self-funding; releases on milestones |
| Sponsor support: equity commitment, cost overrun undertaking | The sponsors | Funds the SPV's own shortfalls | Domain 5, KA 5.2.3 |
| Lenders' security: share pledge, asset security, assignment of contracts and insurances, accounts control | The SPV and its shareholders | Enables enforcement and step-in | Domain 13 perfects it |

The instruments differ along three axes a financier must price separately: **how much** (face
amount), **how certain** (the obligor's credit and the conditions on payment) and **how long**
(expiry against the exposure it covers). The third is the most frequently missed: a performance
bond that expires at provisional completion does not cover the defects-liability period, and a
guarantee that expires on a fixed date rather than on discharge of the underlying obligation is a
gap in the stack with a date on it.

The legal effectiveness of each instrument: the conditions for a compliant demand, whether a
guarantee is primary or secondary, the effect of amendments to the underlying contract on the
guarantor's liability, the perfection and priority of security interests, and their treatment in
the obligor's insolvency — is **wholly jurisdiction-specific and must be confirmed by qualified
counsel**. Nothing in the arithmetic below survives an instrument that cannot be called.

### 12.3.2 Guarantee sufficiency: cap times credit quality

**The principle.** A cap is a promise; a **recovery** is a cap multiplied by the probability that
the obligor pays it in full when called. The financier's question is therefore never "what is the
cap?" but "**what is the risk-adjusted cover, and how does it compare with the exposure?**"

**Worked example 12.3.2 — is Kestrel's security package sufficient?**

1. **Setup.** The exposure of 12.1.2: **12,255,673.53** in the combined 300-day, 5 %-shortfall
   stress. The nominal cover is the **9,600,000** aggregate cap. It is supported by an
   **on-demand bond of 4,800,000** (10 % of the EPC price) from an investment-grade bank,
   treated as payable with effective certainty, and a **parent company guarantee** up to the
   aggregate cap from a group parent whose probability of paying in full when called is assessed
   at **0.70**: an illustrative assumption standing for a mid-quality unrated corporate obligor,
   to be replaced in practice by the credit team's own assessment. Compute the risk-adjusted
   cover, the residue, and the parent-guarantee face amount that would be equivalent to the
   bond.
2. **Formula.** Risk-adjusted cover = Σ(instrument face × probability of payment), applying
   instruments in order of certainty and counting the guarantee only for the increment above the
   bond. Equivalent face = certain amount ÷ probability of payment.
3. **Substitution.** `4,800,000 × 1.00 + (9,600,000 − 4,800,000) × 0.70`;
   `12,255,673.53 − 8,160,000`; `4,800,000 ÷ 0.70`.
4. **Result.**

   | Layer | Face | Quality | Risk-adjusted |
   |---|---|---|---|
   | On-demand bond | 4,800,000.00 | 1.00 | **4,800,000.00** |
   | Parent guarantee, incremental | 4,800,000.00 | 0.70 | **3,360,000.00** |
   | **Total cover** | **9,600,000.00** | | **8,160,000.00** |

   The credit haircut is **1,440,000**, or **15.00 %** of the nominal cap. Against the exposure,
   the uncovered residue rises from **2,655,674** nominal to **4,095,674** risk-adjusted
   (**22.75 %** of the 18,000,000 equity contribution). The parent guarantee face amount
   equivalent to the 4,800,000 bond is **6,857,143**, a multiple of **1.4286×**.
5. **Interpretation.** The **1.4286× equivalence** is the sentence to carry out of this KA: at a
   0.70 assessment, **one dollar of unconditional bank cover is worth 1.4286 dollars of parent
   guarantee**, so a negotiator offered "a parent guarantee instead of the bond" is being
   offered a discount unless the face amount rises by at least 42.86 %. That converts an
   argument about instrument preference into a priced trade, which is the only form in which
   such arguments get settled honestly. The bond's cost makes the trade concrete: at 1.2 % per
   annum over a three-year construction period the 4,800,000 bond costs **172,800**, or 3.60 %
   of its face, which the contractor prices into the contract sum, and which is cheap against
   1,440,000 of credit haircut.

   Three further disciplines. **Assess the guarantor, not the counterparty.** The contracting
   entity in a large EPC is frequently a special-purpose or thinly capitalised subsidiary, so the
   whole credit question is the parent's, and the assessment must be documented, dated and owned by
   the credit function — not asserted in a bankability memorandum. **Test expiry against exposure**,
   instrument by instrument: Kestrel's delay exposure runs to the long-stop date, its performance
   exposure to the completion tests, and its defects exposure to the end of the defects-liability
   period, and any instrument expiring earlier leaves a dated hole. **And never net cover against
   exposure without stating both.** The residue of 4,095,674 is the number that belongs in the
   board paper and in the equity case; the temptation to report "fully covered up to the cap" is
   the reporting failure this KA exists to prevent.

   The caution: probabilities of payment are judgments, they are correlated with the very events
   that trigger a call (a contractor group in distress is both more likely to fail and less
   likely to pay), and a single-point 0.70 conceals that correlation. The professional treatment
   is a range, a named owner for the assessment, and a structural response, an unconditional
   instrument, rather than a more confident number.

   **And a fourth discipline, which the 0.70 conceals more completely than the correlation
   does.** Where the guarantor sits **in the sponsor's own group**, the cover and the equity
   support are not independent: they rest on one balance sheet, and the project has counted it
   more than once. Before the risk-adjusted total is reported, identify **every other obligation
   resting on that same obligor** and state the aggregate: the equity commitment and any
   commitment letter behind it (Domain 5, KA 5.2.3); the contingent equity and cost-overrun
   undertaking (Domain 9); the in-balance cash-call obligation during construction (Domain 14);
   guarantees given on the group's *other* projects, which the project team will not find
   without asking; and this guarantee. The number that matters is not the 4,800,000 in the table
   but the total the group is standing behind across every line of the structure, because the
   events that call one of them are the events that call the others. A security package whose
   parent guarantee and whose equity support are the same covenant has diversified nothing, and
   reporting the two separately without the aggregate is the same reporting failure as netting
   cover against exposure — the numbers are all correct and the picture is wrong. State the
   aggregate, name the obligor, and say plainly that the cover and the equity support are
   correlated.

### 12.3.3 Direct agreements and step-in

**Definition.** A **direct agreement** is a tripartite contract among the project company, a key
counterparty (the offtaker, grantor, EPC contractor or operator) and the lenders' security agent.
Its function is to make the project's contracts survive the project company's own default. Its
standard content: the counterparty acknowledges the assignment of the contract as security; it
agrees to give the lenders **notice** of any project-company default and a **cure period** longer
than the project company's own; and it agrees that the lenders (or a transferee they nominate) may
**step in** and assume the contract, curing arrears, rather than have it terminated.

The commercial logic is Domain 10's, in contractual form. Enforcement that destroys the revenue
contracts destroys the asset's value, leaving lenders with a plant and no offtake. Step-in
converts a liquidation into a substitution: the same asset, the same contracts, a new equity
owner. For the sponsors, the same instrument is the mechanism by which they can lose the project
without the project failing, which is precisely the discipline it is intended to impose.

Two practical points a leader should test. **Cure periods must be operationally achievable**: a
30-day cure that requires a lenders' committee decision, an intercreditor vote and a funding
call is not 30 days of cure, and Domain 10's forward-looking test discipline applies here too;
the question is whether the machinery can move at the speed the clause assumes. **And step-in
without a willing transferee is a right, not a remedy**: its value depends on there being an
operator or sponsor able to take the asset on the terms available, which is a market question to
be answered before close, not a legal question to be answered after default.

### 12.3.4 What the security package is actually worth

The valuation discipline for the whole package follows from 12.3.2 and takes one page:

- **Face amounts**, listed by instrument, with the exposure each is intended to cover.
- **Credit quality** of each obligor, assessed and dated, with the risk-adjusted amount.
- **Conditionality**: on-demand, conditional, or dependent on establishing breach and loss (the
  practical difference between money in 10 days and money in 2 years).
- **Expiry**, against the exposure period, with every gap dated.
- **Reach**: which entity in the counterparty group is actually bound, and whether the guarantee
  survives amendment of the underlying contract.
- **Residue**: exposure less risk-adjusted cover, stated in currency and as a share of equity.

The last line is the one that changes decisions. A package described as "10 % bond plus parent
guarantee to the cap" sounds complete; the same package described as "8,160,000 of risk-adjusted
cover against 12,255,674 of stress exposure, leaving 22.75 % of the equity cheque exposed"
invites the questions that improve it: a larger bond, a bank guarantee in place of the parent,
an uncapped indemnity for defined heads, or a smaller stress accepted with eyes open.

### AI in this KA

**Where it earns its place.** Instrument registers are structured data trapped in unstructured
documents: obligor, face amount, currency, trigger, conditions, expiry, governing law, notice
address. Extracting them across a portfolio and reconciling them against the exposures they are
supposed to cover, flagging every instrument that expires before its exposure ends, is machine
work with an immediate and measurable payoff. Monitoring is better still: an assistant that
watches expiry dates and rating actions against a live register does a job humans do badly.

**Where it must not go.** It must not assess credit quality as if it were a computation; the 0.70
of 12.3.2 is a judgment with an owner. It must not opine on whether a demand would be compliant,
whether a guarantee has been discharged by an amendment, or whether security is perfected — all
jurisdiction-specific legal questions for counsel. And it must not be allowed to *net* cover
against exposure in a summary, because that is exactly the reporting failure 12.3.4 warns against.

**Verification, concretely.** Sample every face amount and expiry date back to the instrument.
Require the credit function to sign the quality assessment used in any risk-adjusted number, with
a date and a range. Recompute one equivalence figure by hand. And confirm counsel's sign-off that
each instrument is callable as modelled before any recovery appears in a lender or equity case.

### Key terms — KA 12.3

| Term | Meaning |
|---|---|
| **On-demand bond** | Bank instrument payable on a compliant demand without proof of breach; near-certain, fee-bearing. |
| **Parent company guarantee** | Group parent's assumption of a subsidiary's obligation; worth the parent's credit. |
| **Risk-adjusted cover** | Σ(face × probability of payment); the number to compare with exposure. |
| **Equivalent face amount** | Certain cover ÷ probability of payment; 1.4286× at a 0.70 assessment. |
| **Direct agreement** | Tripartite notice, cure and step-in rights making contracts survive SPV default. |
| **Step-in** | Lenders or their nominee assuming a contract instead of allowing termination. |
| **Instrument expiry gap** | The dated hole where an instrument expires before the exposure it covers. |
| **Related-party instrument** | Cover provided by an entity in a sponsor's group; counted only once the arm's-length test, the disclosure and the approving body are recorded. |
| **Same-obligor aggregate** | The total of every obligation resting on one group balance sheet — guarantee, equity commitment, contingent equity, cost-overrun undertaking, cash call, other projects' guarantees; the number the risk-adjusted total conceals. |

### Sample MCQs — KA 12.3

**MCQ 12.3-A `[12.3.2 · Application]`** Cover comprises a 4,800,000 on-demand bank bond (payment
effectively certain) and a parent guarantee taking total nominal cover to 9,600,000, the parent
assessed at a 0.70 probability of paying in full. Risk-adjusted cover is:
- A. USD 9,600,000
- B. USD 8,160,000 ✅
- C. USD 6,720,000
- D. USD 3,360,000

*Rationale:* `4,800,000 + 4,800,000 × 0.70 = 8,160,000`. A ignores credit quality; C applies 0.70
to the whole 9,600,000, haircutting the bank bond as well; D counts only the guarantee increment.

**MCQ 12.3-B `[12.3.2 · Analysis]`** A contractor offers to replace a 4,800,000 on-demand bond
with a parent guarantee, the parent assessed at 0.70. The face amount that leaves the project
company no worse off is:
- A. USD 4,800,000
- B. USD 3,360,000
- C. USD 6,857,143 ✅
- D. any amount, since a guarantee from a large group is stronger than a bond

*Rationale:* `4,800,000 ÷ 0.70 = 6,857,142.86`, a 1.4286× multiple (12.3.2). A treats unequal
certainty as equal; B applies the haircut in the wrong direction; D confuses balance-sheet size
with payment certainty and ignores conditionality and timing.

**MCQ 12.3-C `[12.3.3 · Recall]`** The primary commercial purpose of a direct agreement is to:
- A. increase the liability cap of the counterparty
- B. give lenders notice, an extended cure period and the right to step in, so the project's contracts survive the project company's default ✅
- C. transfer the offtake obligation to the lenders
- D. provide additional security over the asset

*Rationale:* Direct agreements preserve the contract, not the cap (12.3.3). C misstates step-in,
which is a right to assume the contract, usually through a nominated transferee; D describes asset
security, a separate instrument.

**MCQ 12.3-D `[12.3.2 · Evaluation]`** A bankability memorandum states that Kestrel's EPC exposure is
"fully covered up to the aggregate cap of 9,600,000". Risk-adjusted cover, on a 0.70 assessment of the
parent guarantor, is 8,160,000 against a stress exposure of 12,255,674. The soundest reporting
position is:
- A. report exposure and risk-adjusted cover side by side, with the residue stated in currency
  and as a share of equity (4,095,674, or 22.75 % of the 18,000,000 cheque), and the 0.70
  recorded as a dated range owned by the credit function ✅
- B. the memorandum is right: 9,600,000 is the contractual cover, and a credit assessment is not
  contractual
- C. report the nominal residue of 2,655,674 only, since a probability of payment is speculative
- D. raise the assumed probability to 0.85 to reflect the size of the parent's balance sheet

*Rationale:* Netting cover against exposure without stating both is the reporting failure this
Knowledge Area exists to prevent, and it is the residue that invites the questions which improve the
package — a larger bond, a bank guarantee in place of the parent, an uncapped indemnity for defined
heads, or a smaller stress accepted with eyes open (12.3.2, 12.3.4). B treats a promise as a recovery.
C discards the credit dimension altogether, when even the single-point 0.70 already understates the
problem by concealing that distress and non-payment are correlated. D answers a correlated exposure
with a more confident number rather than a structural response: at 0.70 one dollar of unconditional
bank cover is worth 1.4286 dollars of parent guarantee, so the remedy is an unconditional instrument
or a larger face amount, not a kinder assumption.

**MCQ 12.3-E `[12.3.2 · Analysis]`** The contractor offers to replace the 4,800,000 on-demand bank
bond with a parent company guarantee of **5,500,000**, pointing out that the face amount is 700,000
higher and that the bond's fee of **172,800** over the construction period is priced into the contract
sum in any event. The credit function assesses the parent at a 0.70 probability of paying in full. The
response should be:
- A. accept: a larger face amount from a substantial group is better cover than a bank instrument, and
  the fee saving is real money
- B. accept, provided the guarantee is drafted in on-demand form
- C. reject as offered: at 0.70 the guarantee is worth **3,850,000**, which is **950,000** less than the
  bond, and the equivalent face is 6,857,143 — accept at that face or keep the bond ✅
- D. reject: a parent company guarantee is never acceptable in place of bank cover

*Rationale:* Cover is face amount multiplied by the probability of payment, so the comparison is
3,850,000 against 4,800,000 and the fee saving is less than a fifth of the 950,000 being given
up (12.3.2). B is the defensible weaker answer and the instructive one: demand form addresses
**conditionality and timing** (money in ten days rather than two years), and leaves the
obligor's **credit** exactly where it was, so a 5,500,000 on-demand parent guarantee is still
worth 3,850,000. A confuses balance-sheet size with payment certainty. D states a rule the
arithmetic does not support: the guarantee is acceptable at 6,857,143 or above, which is what
converts an argument about instrument preference into a priced trade. Whether an instrument is
callable as modelled, and the effect of amendments to the underlying contract on a guarantor's
liability, are questions for qualified counsel in the governing jurisdiction.

**MCQ 12.3-F `[12.3.1 · Comprehension]`** A performance bond and a parent company guarantee of the same
face amount differ in the way a financier prices them because:
- A. they differ only in cost, the bond carrying a fee and the guarantee none
- B. a guarantee covers the defects-liability period and a bond does not
- C. a bond is security over the asset while a guarantee is a contractual promise
- D. an instrument is priced on three separate attributes — how much (face amount), how certain (the
  obligor's credit and the conditions on payment) and how long (expiry against the exposure it covers) —
  and the two instruments are alike only on the first ✅

*Rationale:* Face amount, certainty and duration are three independent questions, and the third
is the most frequently missed: a bond expiring at provisional completion does not reach the
defects-liability period, and a guarantee expiring on a fixed date rather than on discharge of
the underlying obligation is a gap in the stack with a date on it (12.3.1). A reduces three
attributes to one, and the fee is the price of the certainty, not an extra. C confuses credit
support with the lenders' own asset security. B asserts as a rule what is a drafting question in
each instrument.

**MCQ 12.3-F `[12.3.2 · Analysis]`** A security package shows a parent company guarantee from the
lead sponsor's group behind the EPC contractor, assessed at a 0.70 probability of payment. The same
group has given the equity commitment, the contingent equity and the cost-overrun undertaking. The
reviewer's first requirement is:
- A. none: the 0.70 already prices the guarantor's credit, and each obligation has been assessed
  on its own terms
- B. the aggregate of every obligation resting on that one obligor, stated beside the risk-adjusted cover, because the guarantee and the equity support are not independent and the events that call one call the others ✅
- C. that the guarantee be replaced by an on-demand bond, since a 0.70 assessment is never acceptable
- D. that the probability be lowered from 0.70 to reflect the additional obligations

*Rationale:* the assessment prices the obligor's credit, not the number of claims that will
arrive at once; a package whose parent guarantee and equity support are the same covenant has
diversified nothing, and the aggregate is the disclosure (12.3.2). C converts a diligence
finding into a negotiating demand without pricing the trade the 1.4286× equivalence makes
available. D buries the finding inside a probability, which is exactly the reporting failure
this KA exists to prevent: adjust the number and the reader can no longer see what was adjusted
or why.

### Self-check — KA 12.3

1. *State the sufficiency test in one line.* Risk-adjusted cover, Σ(face × probability of
   payment), against the stress exposure; Kestrel's is 8,160,000 against 12,255,674.
2. *What is a 0.70 guarantee worth against an on-demand bond?* (0.70 of its face, so the
   equivalent face is 1.4286× the bond) 6,857,143 for a 4,800,000 bond.
3. *Which instrument attribute is most often missed?* Expiry: an instrument that expires before
   its exposure ends is a dated hole in the stack.
4. *Why is a sponsor-group guarantee not independent of the equity support?* Because both rest
   on one balance sheet, and the events that call one call the others. State the aggregate of
   every obligation on that obligor (equity commitment, contingent equity, cost-overrun
   undertaking, in-balance cash call, other projects' guarantees) beside the risk-adjusted
   cover.
5. *What must be recorded before a related-party instrument counts as cover?* The relationship
   disclosed, the terms tested on an arm's-length basis by someone outside the commercial line,
   and the body that approved the arrangement, with the date and any consent required for it.

---

## Knowledge Area 12.4 — Risk allocation, claims and change

*Topics: 12.4.1 the allocation matrix as a document map · 12.4.2 change and variation mechanics ·
12.4.3 the commercial arithmetic of a claim · 12.4.4 dispute pathways and the cost of the process.*

### 12.4.1 The allocation matrix as a document map

**Definition.** A **risk allocation matrix** in this domain is not the register Domain 11 priced;
it is the map from each priced allocation to **the clause that effects it, the instrument that
backs it and the amount recoverable under it**. One row per risk, four columns beyond the risk
name: the party bearing it, the document and clause, the financial limit, and the instrument
standing behind that limit. The matrix's value is that it makes two failures visible that no
register shows.

**The orphan risk** (priced as transferred in the register, but no clause transfers it). This is
the commonest defect in a transaction that has moved fast, and it is created by exactly the
sequence this book describes: a risk workshop allocates, a model prices the allocation, and the
drafting does not follow. Domain 11's Kestrel case is instructive in reverse. Five items were
*offered* to the contractor and declined, so the register correctly shows them retained; a
matrix that showed them transferred, with no clause reference, would be a 2,690,000 error hiding
in a spreadsheet.

**The doubly covered risk**: two documents allocate the same risk to different parties, or the
same cap is committed twice. Cross-liability between an EPC contract and an interface agreement
is the classic case, and the aggregate-cap arithmetic of 12.1.1 is the other: a matrix that
shows delay, performance and defects each backed by "the aggregate cap" has shown the reader
that the cap is committed three times over.

The matrix is also the instrument of the **reviewer's discipline** of 12.A.3, and it is the
artefact Domain 13's legal diligence stream produces and Domain 14's change control maintains.
Adding a row is how a change order enters the financing record.

### 12.4.2 Change and variation mechanics

**Definition.** A **variation** (or change order) is an instructed alteration to scope, quality,
sequence or timing, with a defined mechanism for valuing it and for adjusting time. A
**compensation event** (or relief event, or excusable delay) is an occurrence for which the
contract grants the contractor time, money, or both, without breach by either party.

The finance leader's interest is narrow and important: **change mechanics decide who funds the
change and whether the funding exists.** Four terms carry that question.

**The valuation rule.** Whether variations are valued at contract rates, at agreed lump sums, on a
cost-plus basis or by a defined dispute route. A cost-plus default is an open cheque against a
fixed funding envelope; contract rates are only protective if the schedule of rates is complete.

**The time consequence.** Whether an extension of time carries prolongation cost, and at what
rate. A pre-agreed daily prolongation rate removes the largest single area of claim quantification
argument, and it should be negotiated at the same time as the delay damages rate, on the same
evidence. Kestrel's regime is asymmetric in a way sponsors should notice: **the contractor's
liability for delay is 20,000 per day, and the project company's exposure to prolongation on an
owner-caused delay is a separately negotiated number** — on Kestrel's claim (12.4.3) it is 12,500
per day of site overhead before disruption and additional plant are added, which is why it should be
pre-agreed on evidence rather than left to be proved after the event.

**The funding link.** Whether the variation is funded from contingency, from the sponsors' cost
overrun undertaking, or from a further drawing, and whether the facility agreement permits it.
Domain 14 (KA 14.3) operates this link at drawdown; the contractual point here is that a change
mechanism that produces obligations the funding documents cannot fund is a structural defect,
not an administrative one.

**The threshold and approval architecture.** Which changes the project company may agree alone,
which require lender consent (usually any change to a material contract, per Domain 10's negative
covenants), and which require the offtaker's or grantor's consent. Serial small changes below a
consent threshold are how a fixed-price contract stops being fixed, which is why cumulative as
well as individual thresholds belong in the drafting.

### 12.4.3 The commercial arithmetic of a claim

**Definition.** A **claim** is an assertion of entitlement to time, money or both under the
contract. Its commercial value to either party is not the sum claimed. It is the **expected
recovery, net of the cost of obtaining it, discounted for the time it takes**; and because
disputes are slow and expensive, that arithmetic frequently inverts the negotiating intuition.

**Notice, records and the back-to-back check, before any of the arithmetic.** The
expected-recovery model below operates on merits and process economics. It assumes something it
never states: that the claim exists as an entitlement at all. Under most standard forms it does
not, unless notice was given. Five things are established as **dated facts** before a single
probability is assigned.

- **The notice position, first and as a fact rather than an argument.** Service of a valid
  notice within a stated period after the event, in the stated form, to the stated recipient, is
  commonly a **condition precedent to entitlement**, which means a claim not notified in time
  can be worth nothing regardless of its merits. The professional consequence is procedural:
  **compute nothing until the notice position is established**, because a probability set
  applied to an entitlement that was never preserved produces a defensible-looking expected
  recovery for a claim that has none, and that number then travels into a board paper, a lender
  report and a provision.
- **The back-to-back check, which runs upstream.** Where the project company receives a claim
  from its contractor and the same event gives it a claim against the grantor or the offtaker,
  it must pass the claim up within **that** contract's own notice period, which is frequently
  **shorter** than the one it has just been given, and starts from the event rather than from
  the day the contractor's notice arrived. Any gap between the two periods is an **uninsured
  exposure**, and it is identified at contract stage rather than at claim stage, alongside the
  threshold and approval architecture of §12.4.2. A project company with a 28-day upstream
  period and a 42-day downstream one has bought a fortnight of exposure on every compensation
  event for the life of the contract.
- **Contemporaneous records are the evidence quantum is proved on.** Site records,
  correspondence, programme updates, resource returns and cost records must be maintained **from
  the date of the event**, not assembled at claim stage: a record created afterwards is an
  argument about what happened, while a record created at the time is evidence of it. The gap
  between the two is routinely the difference between the 1,050,000 assessment below and the
  1,870,000 claimed. Records are retained on the standing basis in the toolkit preamble, for at
  least as long as a claim on the event can still be brought.
- **A named owner and an internal deadline.** The **project company's contract manager** owns
  identification of a potentially notifiable event, and the internal deadline for escalating one
  to that owner and to counsel is set **well inside** the contractual period, because the
  contractual clock runs from the event, not from the day somebody realised it mattered. An
  organisation with no internal deadline has delegated its notice position to whoever happened
  to notice.
- **The standing caution.** Whether a time bar operates as a strict condition precedent, whether it
  can be relieved and on what basis, what a valid notice must contain and to whom it must go, are
  **jurisdiction- and form-specific questions for qualified counsel** on the executed contract.
  Nothing here states the position under any form or any governing law, and nothing here
  characterises any notice as valid or invalid. What the professional owes is that the question is
  asked, dated and answered *before* the claim is valued.

Only once those five are settled does the arithmetic below mean anything.

**Worked example 12.4.3 — Kestrel's contractor claims 90 days and 1,870,000.**

1. **Setup.** The contractor claims a **90-day** extension of time and prolongation cost,
   arising from a late owner-supplied approval that the contract treats as a compensation event.
   Quantum: site overhead **12,500 per day** for 90 days (**1,125,000**), disruption and lost
   productivity **480,000**, additional preliminaries and plant **265,000** (**1,870,000** in
   total). The project company's own assessment, on its expert's analysis, is **55 days** and
   **1,050,000**. Delay damages are 20,000 per day. If the matter goes to arbitration: each
   side's legal and expert costs **620,000**, internal management time **180,000**, an award
   expected in **26 months**, and assessed outcome probabilities of **0.35** that the
   contractor's full case succeeds, **0.25** of a split at the midpoint and **0.40** that the
   project company's assessment is upheld. Discount at Domain 4's **8 %**; costs are incurred
   evenly, so treat them as falling at the **13-month** midpoint. Each party bears its own costs
   (cost-shifting rules vary by jurisdiction and by arbitral rules — a matter for counsel, and
   one that changes this arithmetic materially).
2. **Formula.** Time impact = extension days × delay damages rate (the damages the project company
   forgoes by granting the extension). Disputed sum = quantum gap + time-impact gap. Expected
   award = Σ(probability × outcome). Present value of fighting = expected award ÷ (1.08)^2.1667 +
   costs ÷ (1.08)^1.0833. Settlement ceiling = present value of fighting − negotiation cost.
   Breakeven disputed sum: solve `k·D + PV(costs) = 0.5D + negotiation cost`, where
   `k = (0.35 + 0.25/2) ÷ (1.08)^2.1667`.
3. **Substitution.** Time impact `90 × 20,000 = 1,800,000`; owner's economic cost of the 90 days
   `24,733.33 × 90 = 2,226,000`. Gaps: `1,870,000 − 1,050,000 = 820,000` of money and
   `35 × 20,000 = 700,000` of forgone damages, so `D = 1,520,000`. Expected award
   `0.35 × 1,520,000 + 0.25 × 760,000 = 722,000`. Discount factors `(1.08)^2.1667 = 1.181458` and
   `(1.08)^1.0833 = 1.086949`.
4. **Result.**

   | Item | Amount |
   |---|---|
   | Contractor's quantum claimed | 1,870,000.00 |
   | Value of the extension of time to the contractor (forgone delay damages) | 1,800,000.00 |
   | Project company's own economic cost of the 90 days | 2,226,000.00 |
   | **Total exposure of the event to the project company** | **4,096,000.00** |
   | Disputed sum after the owner's assessment | **1,520,000.00** |
   | Expected award | 722,000.00 |
   | Present value of the expected award (26 months) | **611,109.54** |
   | Present value of own costs (13 months) | **736,005.26** |
   | **Present value of fighting** | **1,347,114.80** |
   | Settling at the midpoint (760,000 + 60,000 of negotiation cost) | 820,000.00 |
   | **Saving from settling at the midpoint** | **527,114.80** |
   | **Settlement ceiling** (indifference point) | **1,287,114.80** = **84.68 %** of the disputed sum |
   | **Disputed sum at which fighting becomes rational** | **6,901,234.43** |
5. **Interpretation.** **The time impact is the bigger prize, and it is usually argued about
   least.** The contractor's money claim is 1,870,000; the extension of time is worth 1,800,000
   to it in delay damages it will not pay, and the two are close to equal. Meanwhile the project
   company's own economic cost of the 90 days (2,226,000) exceeds both. The negotiation is
   therefore not about 1,870,000; the event is worth 4,096,000 to the project company, and any
   settlement discussion that treats the extension of time as a procedural concession has given
   away the largest number on the page.

   **Fighting is expensive in a way the win probability conceals.** A 0.40 chance of complete
   victory sounds like a strong position. It is: the expected award is only 722,000 against a
   disputed 1,520,000. And yet the present value of fighting is **1,347,115**, because own costs
   of 800,000 are certain and immediate while the award is contingent and 26 months away. The
   settlement ceiling is therefore **84.68 % of the disputed sum**: meaning the project company
   should rationally settle at anything up to 1,287,115 of a 1,520,000 dispute, and settling at
   the midpoint saves **527,115**. Practitioners who "never pay more than their assessment" are
   choosing to spend 527,115 to defend a principle, which is a legitimate choice only if it is
   made explicitly and for a stated reason: precedent across a portfolio of similar claims is
   the usual one, and it should be quantified as such.

   **The threshold generalises, and it is the number to carry.** On these cost and probability
   assumptions, fighting only beats settling at the midpoint once the disputed sum exceeds
   **6,901,234**: roughly 4.5 times the disputed sum in front of them, and 14.4 % of the EPC
   price. That single figure explains the observed behaviour of the market better than any
   principle: small and medium claims settle because the process costs more than the gap, and
   only large ones are litigated. An organisation that computes its own threshold once, from its
   own cost base and its own historical outcome distribution, converts claims handling from
   temperament into policy.

   The cautions are three and they are not decoration. The probabilities are judgments and
   should be elicited from the party's own legal advisers as a documented range, not asserted;
   the arithmetic is only as good as they are. Cost recovery (whether the loser pays some or all
   of the winner's costs) varies fundamentally by jurisdiction and by the applicable arbitral
   rules, and it can move this calculation by more than the disputed sum; it is a question for
   qualified counsel and must be answered before the threshold is relied upon. And the model
   omits every non-financial consequence: management distraction, the effect on a continuing
   working relationship, the disclosure obligation to lenders under the information covenants,
   and the precedent set for the remaining claims on the same project. Those belong beside the
   number, not inside it.

### 12.4.4 Dispute pathways and the cost of the process

**Definition.** Contracts specify an escalating sequence for resolving disagreement, typically
engineer's or employer's determination, then senior executive negotiation, then mediation or a
dispute board, then arbitration or litigation, with interim binding decisions in some standard
forms. Each step has a cost, a duration and a probability of resolving the matter, and 12.4.3's
arithmetic applies at every one.

The finance leader's contribution is to **price the pathway before it is used**. Three questions
do most of the work. *How long is the whole ladder?* A pathway whose steps sum to 30 months has
built a 30-month funding and covenant problem into every serious disagreement, and Domain 10's
information covenants will require disclosure at month one. *Is there an interim binding
mechanism?* A dispute board or interim determination that keeps cash moving while the merits are
argued is worth a great deal to a project company whose covenants are tested quarterly; a
pathway whose first binding outcome is a final award is not. *And who funds the process?* Legal
and expert cost is an operating outflow the base case does not contain, and on Kestrel's
arithmetic 800,000 of it is **2.148 times** the project's entire annual covenant headroom of
372,438: a dispute budget large enough to breach the covenant on its own.

Two standing cautions. **A claim disclosed late is a covenant problem and a relationship problem
at the same time**: Domain 10, KA 10.4.4's honesty asymmetry applies with full force, because a
lender who learns of a 4,096,000 exposure from a compliance certificate will price the discovery
into every future waiver. **And settlement is a financing event.** A settlement that changes the
contract price, the completion date, the performance guarantees or the security package touches
the negative covenants and the conditions precedent, so lender consent is usually required and
should be sought early, not presented as a fait accompli.

The selection of forum, seat, governing law, arbitral rules, enforceability of awards and the
availability of interim relief are **jurisdiction-specific matters with material commercial
consequences**, and they are decided at drafting. They must be settled by qualified counsel; the
financial contribution is to state what each option costs in time and money so that the legal
choice is made with the commercial arithmetic in view.

### AI in this KA

**Where it earns its place.** Claims work is document-intensive and that is where machines are
strongest: assembling a chronology from correspondence, programmes and site records; identifying
which notices were served within contractual time bars and which were not; mapping each claimed
head to the clause relied on; and computing the 12.4.3 grid across a range of probability and cost
assumptions so that the sensitivity of the settlement ceiling is visible rather than assumed.
Maintaining the allocation matrix of 12.4.1 as changes are executed is another natural fit.

**Where it must not go.** It must not assess the merits of a claim, opine on whether a time bar
has been missed with the consequence the contract states, characterise concurrent delay, or draft
a settlement position. Those are legal and expert judgments; a claim consultant's and counsel's
work is not a summarisation task. Nor should the probability distribution in 12.4.3 come from a
model: it must come from named advisers, on the record, as a range.

**Verification, concretely.** Test the chronology against the primary documents on a sample of at
least ten events, including every alleged notice. Recompute the expected award and one discount
factor by hand. Require the probability assumptions to be attributed to a named adviser with a
date. And require counsel's confirmation of the cost-shifting and limitation position before the
settlement ceiling is used in a negotiation mandate.

### Key terms — KA 12.4

| Term | Meaning |
|---|---|
| **Allocation matrix** | Risk → bearing party → clause → financial limit → instrument; reveals orphan and doubly covered risks. |
| **Variation / compensation event** | Instructed change; or an occurrence granting time and/or money without breach. |
| **Prolongation cost** | Time-related cost of delay, ideally at a pre-agreed daily rate. |
| **Time impact of a claim** | Extension days × delay damages rate — the damages forgone by granting time. |
| **Settlement ceiling** | Present value of fighting less negotiation cost; the rational maximum to settle at. |
| **Breakeven disputed sum** | The dispute size above which litigating beats settling at the midpoint. |
| **Notice as a condition precedent** | The common position under standard forms that valid, timely notice is a precondition of entitlement, so an unnotified claim can be worth nothing regardless of merit; whether and how it operates is for counsel on the executed contract. |
| **Back-to-back notice check** | The comparison of the upstream notice period the project company must meet with the downstream period it has been given; the gap is an uninsured exposure identified at contract stage. |
| **Contemporaneous records** | Site records, correspondence, programme updates and cost records maintained from the date of the event; the evidence quantum is proved on, as distinct from an argument assembled at claim stage. |

### Sample MCQs — KA 12.4

**MCQ 12.4-A `[12.4.3 · Application]`** A contractor claims 90 days and 1,870,000; delay damages
are 20,000 per day; the project company's daily economic cost of delay is 24,733.33. The total
exposure of the event to the project company is closest to:
- A. USD 1,870,000
- B. USD 3,670,000
- C. USD 4,096,000 ✅
- D. USD 2,226,000

*Rationale:* Quantum 1,870,000 + own economic cost `24,733.33 × 90 = 2,226,000` gives 4,096,000
(12.4.3). A counts only the money claim; B adds the forgone damages of 1,800,000 to the quantum but
omits the project company's own cost; D counts only the economic cost.

**MCQ 12.4-B `[12.4.3 · Analysis]`** The disputed sum is 1,520,000; the present value of the
expected award is 611,110 and of own costs 736,005. The rational settlement ceiling, allowing
60,000 of negotiation cost, is:
- A. USD 760,000, the midpoint
- B. USD 1,287,115 ✅
- C. USD 611,110
- D. USD 1,520,000, the full claim

*Rationale:* Fighting costs a present value of 1,347,115; deducting the 60,000 that settling itself
costs gives an indifference point of 1,287,115, or 84.68 % of the disputed sum (12.4.3). A is one
possible settlement, not the ceiling; C omits own costs, which are the larger component; D assumes
no defence has value.

**MCQ 12.4-C `[12.4.3 · Analysis]`** Why does a 0.40 probability of complete victory still leave a
settlement ceiling at 84.68 % of the disputed sum?
- A. because the expected award is larger than the disputed sum
- B. because own costs are certain and immediate while the award is contingent and 26 months away ✅
- C. because the discount rate is too high
- D. because liquidated damages are excluded from the calculation

*Rationale:* PV of own costs (736,005) exceeds the PV of the expected award (611,110); certainty
and timing dominate probability (12.4.3). A is arithmetically false; C confuses a parameter with
the mechanism; D is untrue. The forgone damages are the largest part of the disputed sum.

**MCQ 12.4-D `[12.4.1 · Analysis]`** A risk register shows five construction risks as transferred;
the allocation matrix shows no clause reference for any of them. The correct conclusion is:
- A. the matrix is incomplete but the allocation stands
- B. these are orphan risks: priced as transferred, retained in fact, and the register overstates protection ✅
- C. the risks are doubly covered
- D. the contractor's aggregate cap covers them

*Rationale:* An allocation without a clause is not an allocation (12.4.1); the register and the
model are both wrong until the drafting follows. C is the opposite defect; D assumes a cap can
cover a liability the contract never created.

**MCQ 12.4-E `[12.4.3 · Evaluation]`** A claims policy states that the organisation "never
settles above its own assessment": 1,050,000 on a disputed sum of 1,520,000, where the present
value of fighting is 1,347,115 and the settlement ceiling 84.68 % of the disputed sum. The
soundest professional position is:
- A. keep the policy: paying more than the merits justify rewards an inflated claim
- B. keep it only as a stated and quantified choice (it costs 527,115 against a midpoint
  settlement), and confirm the cost-shifting position with counsel first, because it can move
  the arithmetic by more than the disputed sum ✅
- C. settle at the ceiling of 1,287,115, since the arithmetic identifies the rational price
- D. arbitrate: a 0.40 probability that the project company's own assessment is upheld is a strong
  position

*Rationale:* Own costs of 800,000 are certain and immediate while the award is contingent and 26
months away, so the process rather than the merits dominates the answer; holding to the
assessment is a legitimate choice, usually for precedent across a portfolio of similar claims,
provided its price is computed and stated (12.4.3). A defends a principle without pricing it. C
mistakes an indifference point for an opening position: the ceiling is a maximum, and settling
at the midpoint saves 527,115. D reads a probability as a position (on these assumptions
fighting beats a midpoint settlement only once the disputed sum exceeds 6,901,234).

**MCQ 12.4-F `[12.4.1 · Comprehension]`** A risk allocation matrix differs from the priced risk
register in that:
- A. it is the same document at a coarser level of aggregation
- B. the register records each risk's probability, impact and owner, while the matrix maps each
  allocation to the clause that effects it, the financial limit and the instrument standing
  behind that limit, which is why only the matrix reveals an orphan or a doubly covered risk ✅
- C. the register is the legal document and the matrix the commercial one
- D. the matrix replaces the register once the contracts are signed

*Rationale:* An allocation with no clause is an intention, and one cap committed to three heads of
claim is one cap and not three; both defects are invisible in a register whose only allocation column
is an owner's name (12.4.1). A misses that the matrix adds columns the register does not have. C
reverses the character of both documents. D discards the quantification the register carries and its
continuing role in sizing contingency and reserves.

**MCQ 12.4-G `[12.4.2 · Evaluation]`** Kestrel's EPC contract fixes the contractor's delay liability at
**20,000 per day** and leaves the project company's exposure to prolongation on an owner-caused delay to
be proved after the event; on the claim of 12.4.3 it is asserted at **12,500 per day** of site overhead
before disruption and additional plant. Variations are valued on a cost-plus basis where the schedule of
rates is silent, and changes below a consent threshold need no lender approval. With the change
mechanics still open, the priority is to:
- A. pre-agree the daily prolongation rate on the same evidence as the delay damages rate, and
  add a cumulative consent threshold alongside the individual one. The asymmetry is **7,500 per
  day** of quantification argument, and serial changes below a threshold are how a fixed-price
  contract stops being fixed ✅
- B. reduce the contractor's daily damages rate to 12,500, so that the regime is symmetrical
- C. remove the cost-plus fallback and require all variations to be valued at contract rates
- D. accept the mechanics: prolongation is proved on actual cost, which is the fairest measure available

*Rationale:* Change mechanics decide who funds a change and whether the funding exists, and a
pre-agreed prolongation rate removes the largest single area of claim-quantification argument,
which is why it belongs beside the delay damages rate and on the same evidence (12.4.2). B
achieves symmetry by weakening the project's own recovery, which is the wrong direction from a
rate that already recovers only 80.86 % of the daily economic cost of delay. C is the defensible
weaker course and only protective if the schedule of rates is complete: where it is not,
contract rates simply relocate the argument, so the ask is a complete schedule with a defined
route for genuinely new work. D mistakes a measurement principle for a mechanism: proving actual
cost after the event is precisely the exercise the pre-agreed rate exists to avoid, and it is
conducted while the covenant is being tested. Whether a pre-agreed daily rate survives challenge
as a penalty, and how a consent threshold interacts with the facility's negative covenants, are
governing-law questions for qualified counsel; the commercial task is to price the asymmetry and
ask for the term.

**MCQ 12.4-H `[12.4.3 · Evaluation]`** A contractor's claim arrives with strong merits and detailed
quantum. The project team asks the commercial manager for an expected-recovery assessment for the
month-end report. The first thing the manager should do is:
- A. build the probability set and the settlement ceiling, since merits and quantum are what determine value
- B. establish the notice position as a dated fact (whether a valid notice was served within the
  contractual period), and compute nothing until it is settled, because under many forms notice
  is a condition precedent to entitlement and an unnotified claim can be worth nothing
  regardless of merit ✅
- C. reject the claim on the assumption that notice was late, and reserve the position
- D. report the claimed sum as the exposure until the arbitration outcome is known

*Rationale:* the arithmetic operates on merits and process economics and silently assumes the
entitlement exists (12.4.3); running it first produces a defensible-looking expected recovery for a
claim that may have none, and that figure then travels into a report and a provision. C asserts a
conclusion in place of a fact and is the mirror error. D reports the claim rather than the exposure
and ignores the time impact, which is usually the larger number. Whether a time bar operates
strictly, and whether it can be relieved, are questions for counsel on the executed contract and its
governing law.

### Self-check — KA 12.4

1. *What is the largest number in a typical extension-of-time negotiation?* Usually the time
   impact: 90 days at 20,000 is 1,800,000 of forgone damages, against a 1,870,000 money claim
   and the project company's own 2,226,000 of economic cost.
2. *State the settlement ceiling rule.* Settle at up to the present value of fighting less the
   cost of settling: 1,287,115 on a disputed 1,520,000, saving 527,115 against a midpoint
   settlement.
3. *Why do most claims settle?* Because process cost and delay dominate: on these assumptions a
   dispute must exceed 6,901,234 before litigating beats settling at the midpoint.
4. *What is established before any of that arithmetic is run?* The notice position, as a dated
   fact. Valid, timely notice is commonly a condition precedent to entitlement, so a probability
   set applied to an unnotified claim produces a defensible-looking expected recovery for
   something worth nothing.
5. *What is the back-to-back check, and when is it done?* Comparing the project company's own
   upstream notice period against the downstream period it has received. It is done at contract
   stage, because any gap is an uninsured exposure on every compensation event for the life of
   the contract, not at claim stage, when it is a discovery rather than a decision.
6. *Who owns identification of a notifiable event?* The project company's contract manager, on
   an internal deadline set well inside the contractual period, because the contractual clock
   runs from the event and not from the day somebody realised it mattered.

---

## Advanced topics — Domain 12

### 12.A.1 What sits outside the cap

An aggregate liability cap is routinely read as the maximum a counterparty can owe. It is not.
Contracts customarily carve out defined heads from the cap — abandonment, fraud or wilful
misconduct, breach of confidentiality or intellectual property, third-party death and personal
injury, environmental indemnities, and sometimes the proceeds of insurance — and the carve-outs are
where unbounded exposure lives, in both directions. Two consequences for a finance leader.

**On the recovery side**, an uncapped indemnity is worth only the obligor's balance sheet, so a
carve-out that looks like unlimited protection is limited in practice by exactly the credit
arithmetic of 12.3.2, and unlike the capped heads, it has no instrument sized against it. **On
the liability side**, the project company's own uncapped indemnities are the exposures most
likely to exceed its equity, and they are the ones sponsors most often fail to model because
they sit outside every number in the contract summary. The discipline is to list the carve-outs
explicitly in the allocation matrix with the word "uncapped" in the limit column, so that they
cannot be read as absent, and to test the largest of them against the insurance programme rather
than the cap.

The scope, validity and effect of any carve-out, and whether a cap is displaced entirely by
particular conduct, are questions of the governing law and the drafting, and they vary
fundamentally between jurisdictions. Qualified counsel must confirm them; the commercial task is
to ensure that each one appears in the matrix with an owner and a sizing.

### 12.A.2 Termination compensation against the amortisation profile

12.2.3 compared a compensation formula with the debt-outstanding profile at points in time. The
advanced form is to plot both across the whole term, because the **shape** of the gap is what
determines whether the structure is robust. Three shapes recur. A **debt-outstanding formula**
tracks the amortisation exactly and produces no gap by construction: the lenders' preferred
outcome, and the one that leaves equity at zero (11,128,176 unreturned at the end of year 5 for
Kestrel). A **fixed-schedule formula** (a depreciated capital value, or a table agreed at
signature) diverges from an annuity amortisation because principal repayment accelerates while
straight-line depreciation does not, so the gap opens or closes depending on the tenor
relationship. And a **percentage-of-debt formula** produces a gap proportional to the
outstanding balance, therefore **largest in the earliest years** (5,926,555 at the end of
Kestrel's year 1 against 4,194,854 at the end of year 5 on an 85 % formula), which is the worst
possible profile, because early-life default risk is the highest and the tail of the loan is the
part a refinancing would have addressed anyway.

Two refinements matter in a real negotiation. **Hedge breakage** is a real cost that
debt-outstanding formulae frequently exclude: a fixed-to-floating swap terminated at an
unfavourable point produces a payment that is neither principal nor interest, and if the
compensation clause does not name it, the lenders are not whole even under a "full debt" formula.
**And the reference date matters**: compensation computed at the termination date, at the date of
a valuation, or at the date of payment can differ by a full period's interest and by any accrued
default interest, all of which should be specified rather than left to construction. Both are
drafting points with a computable price, and both are for counsel to render enforceable and for
the finance leader to size.

### 12.A.3 The reviewer's contract eye

The invariants to test on any contract stack before a financing relies on it:

- **Every risk shown as transferred has a clause reference**, and every clause reference resolves
  to language that actually transfers it (no orphan risks — 12.4.1).
- **Delay damages are calibrated on carrying cost plus forgone `CFADS`**, not revenue alone, and
  the calibration basis is documented (Kestrel: 24,733.33 per day, not 17,733.33 — 12.1.2).
- **The cap-binding day is computed and compared with the schedule risk analysis' credible
  worst-case delay** (Kestrel: day 240).
- **Sub-caps are summed against the aggregate cap**, and the aggregate exceeds the sum wherever it
  is intended to add protection (Kestrel's do not — 12.1.1).
- **Performance damages state which loss they restore** (bare covenant, sized coverage or
  value), and the model uses the same basis the contract does (562,851 / 2,980,263 / 4,835,674 —
  12.1.3).
- **The application of damages proceeds is tested for tenor mismatch**: a multi-decade loss paid by
  a shorter-dated debt prepayment under-compensates (521,045 — 12.1.3).
- **The contracted volume floor clears the covenant**, computed as
  `x = (k × debt service + intercept) ÷ slope`, in the binding period, not on an annual average
  (95.8892 % against a negotiated 90 % — 12.2.2).
- **The maximum annual deduction under the revenue contract is computed and compared with covenant
  headroom** (372,438 for Kestrel — 12.2.1).
- **Termination compensation is plotted against debt outstanding across the term**, and hedge
  breakage is named in the formula (12.A.2).
- **Every instrument's face amount is multiplied by an owned credit assessment**, and every expiry
  date is tested against the exposure period it covers (8,160,000 against 9,600,000 nominal —
  12.3.2).
- **Uncovered residue is reported in currency and as a share of equity**, never netted silently
  (2,655,674 nominal, 4,095,674 risk-adjusted, 14.75 % and 22.75 % of equity — 12.1.2, 12.3.2).
- **Carve-outs from the cap are listed as uncapped**, sized, and tested against insurance
  (12.A.1).
- **Every recovery the model relies on carries counsel's confirmation** that the instrument is
  callable and the provision enforceable in the governing jurisdiction.

---

## Industry variations — Domain 12

- **Contracted power and availability PPPs.** The document set is the most standardised, and the
  binding constraint is usually the **availability and deduction regime** rather than volume:
  the test of 12.2.1, maximum annual deduction against covenant headroom, replaces the volume
  floor of 12.2.2, and performance damages attach to heat rate or capacity rather than output.
- **Transport concessions.** Patronage risk is rarely contracted away, so there is no volume floor
  to compute; the load-bearing clauses instead are **compensation on termination** (12.2.3, because
  the grantor's covenant is the credit) and the **change-in-law and competing-infrastructure
  protections**, whose absence is a revenue risk no cap covers.
- **Water and desalination.** The Kestrel case: take-or-pay water purchase, output and consumption
  guarantees, and a heavy dependence on a single input contract (power) whose interruption
  provisions must mirror the offtake's force-majeure relief exactly, or a gap opens between the
  cost the project incurs and the revenue it may abate.
- **Digital infrastructure.** Multi-package delivery is normal (shell, mechanical and electrical
  fit-out, network), so **interface architecture is the dominant contractual question** (12.2.4
  and Case study B), tenant contracts are shorter than the debt so termination and renewal
  provisions carry the refinancing case, and service-level agreements rather than output
  guarantees define performance.
- **Oil, gas and process industries.** Performance guarantees are multi-dimensional (throughput,
  yield, specification, consumption), so the buy-down arithmetic of 12.1.3 must be run per
  guarantee and aggregated, and licensor and process-technology agreements add a party whose
  liability is characteristically capped very low relative to the loss it can cause.
- **Social infrastructure and accommodation PPPs.** Payment is availability-based with detailed
  performance deductions and a defined **hand-back regime**: condition surveys, a hand-back reserve
  and rectification obligations at the end of the concession, which is a contractual exposure that
  falls after the loan has been repaid and is therefore invisible in every coverage ratio.

---

## Case study — Domain 12: the cap that covered 4.96 per cent (water / desalination)

**Situation.** Ten weeks before financial close, Kestrel's lenders' legal adviser circulated a
contract-limits table of the kind set out in 12.1.1. It showed the six numbers: a 48,000,000 fixed
price, delay damages of 20,000 per day capped at 10 %, performance damages capped at 10 %, an
aggregate cap of 20 %, and security comprising a 10 % on-demand bond and a parent guarantee to the
aggregate cap. The sponsors' commercial team regarded the stack as market standard and the
technical adviser had confirmed the plant design. Nobody had multiplied anything.

**What happened.** The financial adviser ran three computations in an afternoon. **The
cap-binding day**: 4,800,000 ÷ 20,000 = **day 240**, against a schedule risk analysis whose P80
delay was 210 days and whose P95 was 320, so the cap covered the P80 case and failed the P95
one, leaving **2,620,000** uncovered at 300 days and **4,104,000** at 360 (Domain 5, KA 5.4.2
computed the 360-day figure). **The performance cap's reach**: on the value basis of 12.1.3, one
percentage point of permanent output shortfall is worth `90,600 × AF(0.08, 25) = `
**967,134.71**, so the 4,800,000 cap covers a shortfall of **4.9631 %** — while the 1.20
covenant fails at a shortfall of **4.1108 %**. The regime was adequate by a margin of 0.85
percentage points, which nobody had designed and nobody had checked. **And the aggregate cap**:
10 % + 10 % = 20 %, so the two sub-caps consumed the aggregate exactly and a defects claim after
both would recover nothing.

The combined stress (300 days late at 95 % output) produced exposure of **12,255,673.53**
against nominal cover of **9,600,000**, a residue of **2,655,673.53**. Applying the credit
arithmetic of 12.3.2, with the bond at full value and the parent guarantee assessed by the
lenders' credit function at 0.70, risk-adjusted cover was **8,160,000** and the residue
**4,095,673.53**, **22.75 %** of the 18,000,000 equity contribution, and **16.41 %** of Domain
4's project `NPV` of 16,179,360 on the nominal measure.

**How it resolved.** Four changes were negotiated, each cheap because the arithmetic was
explicit. The **aggregate cap moved from 20 % to 30 %** (14,400,000), so the sub-caps no longer
exhausted it and a defects head had 4,800,000 of room. The **on-demand bond rose from 10 % to 15
%** (7,200,000), converting 2,400,000 of parent-guarantee cover at 0.70 into bank cover at full
value and lifting risk-adjusted cover by 720,000; the contractor priced the additional bonding
at about 1.2 % per annum over three years (some 86,400 on the incremental 2,400,000), and added
it to the contract sum, which the sponsors accepted as the cheapest credit enhancement
available. **A termination-for-delay right at a 300-day long-stop** was added, so that the
contractor's incentive did not fall to zero at the cap. And **the performance damages were
re-based on the value measure** at 967,135 per percentage point, with the facility agreement
amended to direct 60 % of any receipt to prepayment and 40 % to a blocked distribution account:
the drafting response to the 521,044.53 tenor mismatch of 12.1.3, which cost nothing and removed
a permanent equity leak.

**What the domain teaches here.** Every one of those four defects was visible from numbers
already in the documents; none required a model, a negotiation or a new adviser. The failure was
not analytical but procedural: no one had been given the job of multiplying the contract's
limits against the project's stresses. That job (the six-number table, the cap-binding day, the
coverage reach of each cap, and cover multiplied by credit quality) is the whole of this
domain's professional content, and it takes an afternoon.

## Case study B — Domain 12: the interface nobody owned (digital infrastructure)

**Situation.** A 24 MW hyperscale data centre in a competitive market was let as two packages to
save cost and time: **shell and core at 62,000,000** and **mechanical, electrical and network
fit-out at 48,000,000**, a combined **110,000,000**. The single-wrap alternative had been
offered by the shell contractor at a **3.5 %** premium (**3,850,000**), and declined, correctly
on Domain 11's reasoning: the shell contractor could not price the fit-out interfaces it did not
control, and its loading reflected that ignorance. What the project company did not do was
replace the wrap with an interface regime. There was no interface matrix, no back-to-back
completion definition, no cross-liability provision, no interface manager and no single owner of
programme float. Financing was **77,000,000** of senior debt at **6.5 %** against forecast
operating `CFADS` of **14,600,000**.

**What happened.** The fit-out contractor's containment and cable-tray installation required
structural penetrations at coordinates the shell contractor built to an earlier revision of the
model. Each party demonstrated compliance with its own contract documents. The dispute over which
revision governed the handover took eleven weeks to resolve commercially, and the practical
completion date moved **74 days**.

The arithmetic was unforgiving. Daily carrying cost on the drawn debt was `77,000,000 × 0.065 ÷
360 = ` **13,902.78**; forgone `CFADS` was `14,600,000 ÷ 360 = ` **40,555.56**; the daily
economic cost of delay was therefore **54,458.33** and the 74 days cost **4,029,916.67**. On top
of that, **both** contractors claimed extensions of time and prolongation (at 9,800 and 7,400
per day respectively, **1,272,800** in total), and both were entitled to something, because
neither was in breach of its own scope. Total cost to the project company: **5,302,716.67**, or
**4.821 %** of the combined contract price, with **no delay damages recoverable from anyone**.

**How it resolved.** The claims were settled at a discount on the 12.4.3 arithmetic; both were
below the project company's own litigation threshold, and settling promptly was worth more than
either defence. The interface responsibility was retrofitted mid-project: an interface manager
with authority over both contracts, a matrix of 340 handovers with named owners and dates, a
single model revision under change control, and back-to-back milestone definitions for the
remaining handovers. The retrofit cost about **640,000** and no comparable event recurred.

**What the domain teaches here.** The declined wrap was not the error, and the arithmetic proves
it: the wrap premium of 3,850,000 was certain, the interface loss of 5,302,717 was not, so
buying the wrap would only have paid if the probability of an event of this size exceeded
`3,850,000 ÷ 5,302,717 = ` **72.60 %** — which no one could have argued. The error was to treat
"do not buy the wrap" as a complete decision. The interface regime that would have prevented the
event cost **640,000**, a breakeven probability of only **12.07 %**, and it was the obviously
correct purchase on any plausible view of the risk. **Declining a transfer creates a management
obligation**, and this domain's contribution is to name the instrument that discharges it (an
interface matrix with owners, dates, back-to-back completion definitions, cross-liability and
one owner of float), and to price it against the exposure it removes.

---

## Executive perspective — Domain 12

What a project finance director cannot delegate in this domain:

- **The six numbers.** Contract price, damages rate, delay cap, performance cap, aggregate cap and
  security. Read them off the documents personally, multiply them, and know the cap-binding day
  (Kestrel: day 240) and the shortfall each cap reaches (4.9631 %) before signature.
- **The calibration basis of every damages provision.** Whether damages restore the covenant,
  the sized coverage or the value of the loss, 562,851, 2,980,263 or 4,835,674 for the same 5 %
  shortfall. Whoever chooses that basis chooses who bears the loss, and it is a sponsor
  decision, not a drafting detail.
- **The contracted volume floor.** Issue it to the commercial team as a financing constraint with
  its derivation attached, before negotiation: 95.8892 % for Kestrel's 1.20× covenant, not the 90 %
  a commercial team will otherwise agree.
- **Cover multiplied by credit quality, and the residue.** 8,160,000 against 12,255,674 of stress
  exposure leaves 22.75 % of the equity cheque exposed. That number belongs in the investment
  committee paper in those words; "covered up to the cap" does not.
- **The claims threshold, computed once.** Know the settlement ceiling rule and the disputed sum
  above which the organisation litigates (6,901,234 on Kestrel's assumptions). Claims policy set in
  advance beats claims temperament in the moment, and it removes a systematic value leak.
- **The boundary with counsel, in both directions.** Never let a financial model rely on a recovery
  counsel has not confirmed is available in the governing jurisdiction; and never let a legal
  negotiation trade a limit whose financial consequence has not been computed. The director owns
  the interface between the two, and it is the only place in the transaction where both errors are
  visible at once.

## Calculation exercises — Domain 12

**Exercise 12.1** A project has 96,000,000 of debt drawn at 7.2 %, forecast operating `CFADS` of
15,300,000 per year, and an EPC price of 120,000,000. Delay damages are 45,000 per day, capped at
8 % of the price; 30/360 applies. Compute the daily economic cost of delay, the share of it the
damages recover, the cap-binding day, and the amount uncovered on a 300-day delay.

*Solution.* Daily interest `96,000,000 × 0.072/360 = ` **19,200**; daily forgone `CFADS`
`15,300,000/360 = ` **42,500**; daily economic cost **61,700**. Damages recover `45,000/61,700 =
` **72.93 %**. Cap `120,000,000 × 0.08 = ` **9,600,000**, binding at `9,600,000/45,000 = ` **day
213.33**. At 300 days the economic cost is `61,700 × 300 = ` **18,510,000** against a capped
recovery of 9,600,000, leaving **8,910,000** uncovered, 7.425 % of the contract price. *Common
error:* calibrating on forgone `CFADS` alone, which would set the rate at 42,500 and recover
only **68.88 %** of the daily cost, omitting the carrying cost of drawn debt — the most certain
component, because it accrues whether or not the plant would have performed.

**Exercise 12.2** The same project's performance damages are capped at 8 % of the 120,000,000
price. Each percentage point of permanent output shortfall reduces annual `CFADS` by 210,000. The
remaining operating life is 20 years and the sponsors' appraisal rate is 8 %. On a value basis,
what output shortfall does the cap actually cover?

*Solution.* `AF(0.08, 20) = 9.818147`; the value of one percentage point is
`210,000 × 9.818147 = ` **2,061,810.87**. The cap of **9,600,000** therefore covers
`9,600,000/2,061,810.87 = ` **4.6561 %** of output and no more. *Common error:* dividing the cap by
the *annual* `CFADS` shortfall (9,600,000/210,000 = 45.7 "points"), which values a permanent loss
as though it lasted one year and overstates the cap's reach roughly tenfold.

**Exercise 12.3** The same project is funded with 96,000,000 of senior debt at 7.2 % over 15 years.
Its `CFADS` as a function of the share `x` of contracted output taken is
`CFADS(x) = 16,800,000x − 3,072,000`. The covenant is **1.25×**. Compute the instalment, the
`DSCR` at full contracted volume, and the minimum contracted volume floor the covenant requires.

*Solution.* `AF(0.072, 15) = 8.993967`; instalment `96,000,000/8.993967 = ` **10,673,821.69**.
At `x` = 1, `CFADS` = **13,728,000** and `DSCR` = **1.2861**. The covenant requires `CFADS ≥
1.25 × 10,673,821.69 = ` **13,342,277.11**, so `x = (13,342,277.11 + 3,072,000)/16,800,000 = `
**97.7040 %**. *Common error:* concluding that because the sized `DSCR` of 1.2861 comfortably
exceeds the 1.25 covenant, a 90 % volume floor is safe, at 90 % the `DSCR` is **1.1287**, a
breach. Thin headroom converts almost any volume concession into a covenant problem, and the
floor must be computed, not inferred from the base-case ratio.

**Exercise 12.4** Stress exposure on a contract is 14,200,000. Cover comprises a 6,000,000
on-demand bank bond (payment treated as certain) and a parent company guarantee taking nominal
cover to 12,000,000, the parent assessed at a 0.65 probability of paying in full when called.
Compute risk-adjusted cover, the uncovered residue on both measures, and the parent-guarantee face
amount equivalent to the bond.

*Solution.* Risk-adjusted cover `6,000,000 + (12,000,000 − 6,000,000) × 0.65 = ` **9,900,000**.
Uncovered: **2,200,000** on the nominal measure, **4,300,000** risk-adjusted, nearly double.
Equivalent face for the bond `6,000,000/0.65 = ` **9,230,769.23**, a 1.5385× multiple. *Common
error:* applying the credit haircut to the whole 12,000,000 (giving 7,800,000), which wrongly
discounts the bank bond; the haircut applies only to the layer the weaker obligor supports.

**Exercise 12.5** A disputed sum is 2,400,000. Assessed outcomes: 0.30 that the claimant's full
case succeeds, 0.30 of a split at the midpoint, 0.40 that it fails. Own legal, expert and
management costs are 700,000, incurred at a 14-month midpoint; an award is expected at 30 months;
the discount rate is 8 %; settling costs 50,000 of negotiation effort. Compute the present value of
fighting, the saving from settling at the midpoint, the settlement ceiling, and the disputed sum
above which fighting becomes rational.

*Solution.* Expected award `0.30 × 2,400,000 + 0.30 × 1,200,000 = ` **1,080,000**. Discount
factors `(1.08)^2.5 = 1.212158` and `(1.08)^(14/12) = 1.093942`. Present value of the award
`1,080,000/1.212158 = ` **890,972.64**; of costs `700,000/1.093942 = ` **639,887.55**; present
value of fighting **1,530,860.19**. Settling at the midpoint costs `1,200,000 + 50,000 = `
**1,250,000**, so settling saves **280,860.19**. The settlement ceiling is `1,530,860.19 −
50,000 = ` **1,480,860.19**, or **61.70 %** of the disputed sum. With `k = (0.30 +
0.15)/1.212158 = 0.371239`, fighting beats settling at the midpoint only above `(639,887.55 −
50,000)/(0.5 − 0.371239) = ` **4,581,245.18**. *Common error:* comparing the expected award of
1,080,000 with the midpoint settlement of 1,200,000 and concluding that fighting is cheaper,
which ignores both own costs, the larger term, and the 30-month delay before any recovery.

## Practitioner's toolkit — Domain 12

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable, and set a
retention period against each. These registers are the evidence that a decision was taken
properly, so each is retained at least as long as the obligation it supports, in a form that
opens without the tool that created it, with a named custodian who holds it once the engagement
ends. The applicable minimum periods are set by the organisation's own policy and by
jurisdiction-specific statutory, tax and limitation requirements, which this book does not
state. Where a register holds information about identified individuals, the retention period and
any minimisation or deletion obligation that cuts across it are settled with the organisation's
data-protection adviser before the register is adopted.*

### Toolkit 12.T.1 — The contract limits and calibration sheet (one per contract)

One page, filled before the contract is signed and re-filled at every amendment. Rows: contract
price and change mechanics · damages rate, with the daily economic cost it was calibrated against
and the share it recovers · delay cap, and **the cap-binding day** · the credible worst-case delay
from the schedule risk analysis, beside it · performance damages rate and cap, with **the shortfall
percentage the cap reaches** on the stated basis · the basis itself (bare covenant / sized coverage
/ value) named and owned · aggregate cap, with the **sum of the sub-caps** beside it · the carve-outs
from the cap, each marked "uncapped" and sized · the exclusion of consequential loss, quoted · the
combined stress exposure and the residue in currency and as a share of equity. Rule: no contract
goes to signature until the residue line is filled and initialled by the accountable sponsor
executive. Two further rows where the counterparty is a sponsor affiliate: **related party (Y/N) and
the relationship**, and **the disclosure made and the body that approved it**, with the date and any
consent required (12.1.1). *Retention:* held by a named custodian with the executed contract, for
the longest of the contract's life including its defects and warranty periods, the applicable
limitation period and any statutory requirement the organisation has established.

### Toolkit 12.T.2 — Security package sufficiency register (one per transaction)

One row per instrument: instrument type · obligor, and which entity in the group is actually
bound · face amount and currency · conditionality (on demand / conditional / requires proof of
loss) · credit assessment, with the assessor's name and date · **risk-adjusted amount** · the
exposure it covers and that exposure's end date · **the instrument's expiry, and the gap in days
if it is earlier** · counsel's confirmation that it is callable as modelled, with date ·
**related party (Y/N) and the relationship**, which sponsor, and how · **other group obligations
resting on the same obligor**, listed and totalled (equity commitment, contingent equity,
cost-overrun undertaking, in-balance cash call, guarantees on the group's other projects) ·
**the disclosure made and the body that approved the related-party arrangement**, with the date,
and whether any shareholder or lender consent was required for it. Footer lines: total face,
total risk-adjusted, stress exposure, **residue in currency and as a percentage of equity**,
and, separately, **the aggregate exposure of each group obligor across the whole structure**.
Rule: no lender or equity case reports cover without the risk-adjusted total beside the face
total; and no related-party instrument is counted as cover until the arm's-length test, the
disclosure and the approval are recorded (12.1.1, 12.3.2). *Retention:* held by a named
custodian with the instruments themselves and the credit assessments behind them, for the
longest of the exposure period each instrument covers, the applicable limitation period and any
statutory requirement the organisation has established.

### Toolkit 12.T.3 — Claim assessment and settlement-zone worksheet (one per claim)

Header: claim reference, event, notice served and whether within the contractual time bar (fact,
with the date), clause relied on. Section 1 — **quantum**: each head claimed, the party's own
assessment, and the gap. Section 2 — **time**: days claimed, days assessed, and the gap valued
at the delay damages rate; plus the party's own economic cost of the days at issue. Section 3 —
**process**: own legal, expert and management cost, its timing midpoint, expected date of a
binding outcome, outcome probabilities attributed to a named adviser as a range and dated, and
the applicable cost-shifting position confirmed by counsel. Section 4 — **arithmetic**: present
value of the expected award, present value of own costs, present value of fighting, the
settlement ceiling, the saving against a midpoint settlement, and the organisation's standing
breakeven disputed sum. Section 5 — **non-financial**: relationship, precedent across open
claims, lender disclosure obligation and date, and any consent required for a settlement that
touches a material contract. Rule: the mandate given to a negotiator states the ceiling, not the
aspiration, and **Section 4 is not started until the header's notice line is a dated fact**
(12.4.3), with the upstream back-to-back position recorded beside it: the period the project
company has to pass the claim on, the date it expires, and who owns serving it. *Retention:* the
worksheet and the contemporaneous records it rests on (site records, correspondence, programme
updates, cost records) are held by a named custodian for at least as long as a claim on the
event can still be brought, in a form that opens without the systems that created them; the
period is jurisdiction-specific and is established with counsel rather than assumed.

## Exam preparation — Domain 12

**What is assessed.** Reading a contract stack as a set of computable limits; calibrating and
testing liquidated damages against the loss they cover; the interaction of sub-caps and aggregate
caps; the three make-good bases for underperformance and the consequences of choosing among them;
deriving a contracted volume floor from a covenant; testing termination compensation against the
debt-outstanding profile; converting instrument face amounts into risk-adjusted cover and
computing equivalence between instruments of different certainty; the commercial valuation of a
claim including time impact and process cost; and the governance boundary between commercial
arithmetic and legal conclusion.

**The calculations to do under time pressure.** Daily economic cost of delay = drawn debt × rate ÷
day-count basis + annual `CFADS` ÷ basis. Cap-binding day = cap ÷ daily rate. Uncovered residue =
daily cost × days − min(rate × days, cap). Value of a permanent shortfall = annual `CFADS`
shortfall × `AF(r, n)`, and the shortfall a cap reaches = cap ÷ value per point.
Coverage-restoring buy-down = debt × (`CFADS` shortfall ÷ base `CFADS`). Contracted volume floor
= (target ratio × debt service + intercept) ÷ slope. Risk-adjusted cover = Σ(face × probability of
payment), and equivalent face = certain cover ÷ probability. Expected award = Σ(probability ×
outcome), discounted to today; settlement ceiling = present value of fighting − cost of settling.

**The traps.**
- Negotiating the damages **rate** and ignoring the **cap-binding day**. The cap is where the
  structure breaks (12.1.2, Exercise 12.1).
- Calibrating delay damages on forgone revenue alone, omitting carrying cost — 71.70 % coverage for
  Kestrel, 68.88 % in Exercise 12.1 (12.1.2).
- Reading an aggregate cap as protection above sub-caps that already exhaust it (12.1.1, MCQ 12.1-B).
- Valuing a permanent output shortfall as one year's loss rather than `× AF(r, n)` (Exercise 12.2).
- Confusing the three make-good bases and quoting a lender-facing number as the sponsors' loss —
  562,851 / 2,980,263 / 4,835,674 (12.1.3, MCQ 12.1-C).
- Assuming that damages of the right **amount** compensate, regardless of how their proceeds are
  **applied** — the 521,045 tenor mismatch (12.1.3, MCQ 12.1-D).
- Treating the contracted volume floor as a commercial preference rather than deriving it from the
  covenant — 95.8892 % against 90 % (12.2.2, MCQ 12.2-A, Exercise 12.3).
- Inferring that a comfortable base-case `DSCR` makes a volume concession safe (Exercise 12.3).
- Reading a percentage-of-debt termination formula as adequate without plotting it against the
  amortisation profile, where the gap is largest early (12.2.3, 12.A.2, MCQ 12.2-C).
- Splitting packages without an interface regime, and assuming a loss will be recoverable from one
  of the contractors (12.2.4, MCQ 12.2-D, Case study B).
- Reporting cap face amounts as cover, without multiplying by credit quality — 9,600,000 against
  8,160,000 (12.3.2, MCQ 12.3-A, Exercise 12.4).
- Accepting a parent guarantee for a bond at equal face value. The equivalence is 1.4286× at
  0.70 (12.3.2, MCQ 12.3-B).
- Ignoring instrument expiry against the exposure period (12.3.1, 12.3.4).
- Treating an extension of time as a procedural concession rather than the largest number in the
  negotiation — 1,800,000 of forgone damages (12.4.3, MCQ 12.4-A).
- Comparing an expected award with a settlement figure while ignoring own costs and the delay to
  recovery (12.4.3, MCQ 12.4-C, Exercise 12.5).
- Recording a risk as transferred in the register with no clause reference — the orphan risk
  (12.4.1, MCQ 12.4-D).
- Allowing an AI-produced contract summary, or any summary, to substitute for counsel on
  enforceability, and letting a model rely on a recovery counsel has not confirmed (every KA).

**How the domain connects.** Domain 5 established the bankability role of the EPC wrap and computed
the cost of a COD slip; Domain 8 sized the contingency that a fixed-price wrap made defensible;
Domain 10 supplied the coverage machinery every limit in this domain is tested against, and the
covenant and lock-up triggers those tests read; Domain 11 priced the allocations this domain
documents and quantified the register residue that no clause covers. Forward: Domain 13's legal,
insurance and tax diligence streams verify this stack and its conditions precedent, and its model
audit checks that the model's recoveries match the caps; Domain 14 operates the change and
certification machinery of KA 12.4 at drawdown; Domain 15 lives with the deduction regimes,
guarantees and termination provisions in operation, and reaches for them in restructuring; and
Domain 16 governs the AI-assisted contract review that every KA here has bounded.

## Domain 12 summary
Contracts are where Domain 11's priced allocations become enforceable limits, and those limits
are numbers that can be multiplied before signature. Kestrel's stack — a 48,000,000 fixed price,
delay damages of 20,000 per day against a daily economic cost of 24,733.33, a 10 % delay cap
binding at **day 240**, a 10 % performance cap, a 20 % aggregate cap that the two sub-caps
**exhaust exactly**, a 10 % on-demand bond and a parent guarantee — produces, against a 300-day
delay and a 5 % output shortfall, exposure of **12,255,674** against nominal cover of
**9,600,000** and risk-adjusted cover of **8,160,000**: a residue of **2,655,674** nominal and
**4,095,674** credit-adjusted, being 14.75 % and **22.75 %** of the 18,000,000 equity cheque and
16.41 % of Domain 4's `NPV` of 16,179,360. Performance damages must state which loss they
restore, because the same 5 % shortfall is worth **562,851** on a bare-covenant basis,
**2,980,263** on the sized-coverage basis and **4,835,673.53** on the value basis (a spread of
**8.591×**), and the 10 % cap reaches a shortfall of only **4.9631 %** against a covenant that
fails at **4.1108 %**; even a correctly sized receipt under-compensates by **521,045** when a
25-year loss is paid by a 12-year debt prepayment, so the application clause is worth as much as
the amount. On the revenue side the contracted volume floor is a **financing parameter**, not a
commercial preference: Kestrel's 1.20× covenant requires **95.8892 %** of guaranteed output
where the commercial team agreed 90 % (a `DSCR` of **1.0935**), each point of volume being worth
0.0181 of coverage; and a termination formula paying a percentage of debt outstanding fails
worst early (**5,926,555** short at the end of year 1 on an 85 % formula), while a formula that
covers debt in full leaves equity's **11,128,176** of unreturned capital at zero. Guarantees are
worth cap × credit quality, making one dollar of on-demand bank cover equal to **1.4286**
dollars of parent guarantee at a 0.70 assessment, and instrument expiry against exposure is the
gap most often missed. Claims are valued on expected recovery net of process cost and time: on a
disputed **1,520,000**, fighting costs a present value of **1,347,115**, the settlement ceiling
is **1,287,115** (**84.68 %** of the claim) settling at the midpoint saves **527,115**, and the
disputed sum must exceed **6,901,234** before litigation is rational, which is why an
organisation should compute that threshold once and make it policy. Case study B prices the
other half of Domain 11's lesson: declining a **3,850,000** wrap premium was defensible at a
**72.60 %** breakeven probability, but failing to buy the **640,000** interface regime that
would have prevented a **5,302,717** loss at a **12.07 %** breakeven was not — **declining a
transfer creates a management obligation**. Throughout, the commercial arithmetic is the finance
leader's and the legal conclusion is counsel's: no model may rely on a recovery that qualified
counsel has not confirmed is available in the governing jurisdiction. Domain 13 verifies this
stack in diligence and at financial close; Domain 14 runs its change machinery through
construction; Domain 15 lives inside its deduction, guarantee and termination regimes in
operation.
