# Domain 11 — Risk Identification and Allocation

> **Group:** Executing the transaction (Domain 2 of 4 in Part Three). **Target:** ~75 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain prices the allocation decisions that Domain 12 then
> documents. It uses `EMV` on the PML-AI Domain 8 definition, and it reads its consequences in the
> coverage machinery Domain 10 built (`CFADS`, `DSCR`, covenant and lock-up triggers) — neither is
> re-derived here. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

## Why this domain exists

Domain 10 established what a project's cash flow can carry and on what conditions. It stress-tested
that cash flow without ever asking the prior question: **who bears each of the things that could
reduce it, and what did that allocation cost?** Domains 5 to 9 named the risks; Domain 10 measured the consequence
of them materialising. Neither decided the allocation, and the allocation is where most of a project
financing's value is created or quietly destroyed.

The domain's central claim is that **risk allocation is a price, not a preference.** Practitioners
talk as though risk were transferred by assertion — "the contractor takes ground risk", "the offtaker
takes volume risk" — but no counterparty absorbs an exposure for nothing. It charges, and its charge
is its own expected cost plus a loading for capital, uncertainty and margin. The value of a transfer
is therefore the transferor's expected cost *minus* the price paid, and that difference can be
negative. It is reliably negative in one identifiable circumstance: when the party being asked to
take the risk can neither influence its probability nor its consequence. Such a party prices its
ignorance, and a project that buys the transfer anyway pays more than the risk costs — sometimes
enough to break its own coverage. The five items of Kestrel's construction register that were
*offered* to the contractor and declined would have cost **USD 4,620,000** in premium against an
owner's expected cost of **USD 2,840,000**, and buying them at 70/30 gearing would have pushed the
project's year-one `DSCR` to **1.1832** — below its own covenant, before the plant produced a litre
of water.

The domain then works outwards through the four risk families. Construction and completion risk is
where the pricing test is sharpest, because counterparty and price are both explicit (KA 11.1).
Market and operating risk is allocated through indexation and pass-through formulae rather than
liability clauses, and a **mismatch** between the index on revenue and the driver of cost is a slow
covenant breach nobody tests for (KA 11.2). The financial and event risks are where the arithmetic is
most tractable and most often left undone: 74 basis points of reference rate, a 5.06 % devaluation and
a 21-day outage each sit between Kestrel and a breach, all three computable in advance (KA 11.3). And
the modern families — environmental and social, technology, cybersecurity, AI model risk — are where
the register is newest, thinnest and most aggressively re-cut by lenders (KA 11.4).

**Learning objectives.** After this domain a candidate can: build a risk register at the level of
granularity an allocation decision requires; state the two legitimate grounds for transferring a
risk and distinguish them from bargaining power; compute the net value of a transfer as expected cost
less loaded premium, and the breakeven premium and breakeven loading at which a transfer stops paying;
demonstrate arithmetically why transferring an uncontrollable risk destroys value even at a zero
margin; translate an input-cost pass-through share into a coverage tolerance multiplier; detect and
quantify an indexation mismatch between revenue escalation and cost drivers; compute unhedged,
partially hedged and fully hedged debt service and the `DSCR` range each produces; derive the minimum
hedge ratio that survives a specified rate shock; quantify the coverage consequence of a currency
mismatch and compute the exchange-rate-indexed revenue share that neutralises debt service; test an
insurance waiting period against covenant headroom in days; aggregate a risk register to a mean, a
standard deviation and a percentile on the PML-AI Domain 8 method, explain why a lender re-cuts it and
what correlation does to it; convert a register into a debt-capacity number; and govern AI use in
risk identification while treating AI itself as a register line.

**The master thread.** Kestrel Water SPC continues. Capital cost **USD 60,000,000** funded **70/30**
as **USD 42,000,000** of senior debt at **6.0 % over 12 years** — annual instalment
**USD 5,009,635.23**, year-one interest **2,520,000**, year-one principal **2,489,635.23**,
`AF(0.06, 12) = 8.383844` (Domain 3) — plus **USD 18,000,000** of equity. Operating life 25 years;
documented first-year **`CFADS` USD 6,384,000** (6,984,000 before working-capital movements) on
revenue of 12,000,000 and cash operating costs of 4,500,000. Coverage at close (Domain 10):
`DSCR` **1.2743** = `LLCR` **1.2743**, `PLCR` **1.9431**, covenant cash trigger **6,011,562.28**,
lock-up trigger **5,761,080.51**, annual headroom **372,437.72**. Domain 8 provisioned against a
retained construction register with a mean of **2,690,000** and a P80 of **4,246,095**. This domain
supplies the fact those chapters took as given: **where that retained register came from**, and what
the allocation that produced it cost.

---

## Knowledge Area 11.1 — Construction and completion risk

*Topics: 11.1.1 the register at allocation granularity · 11.1.2 the two grounds for transfer ·
11.1.3 pricing the transfer · 11.1.4 completion risk and the residue.*

### 11.1.1 The register at allocation granularity

**Definition.** A **risk register** for financing purposes is a list of identified events, each with
a probability, a monetary impact, an owner and — the line most registers omit — **the mechanism by
which that ownership is effected.** Registers written for project-management purposes stop at the
owner's name; a financing register must name the clause, the guarantee, the reserve or the insurance
policy that makes the ownership real, because an unmechanised allocation is an intention, not a
transfer.

Granularity is the practical craft. A register line reading "construction risk — contractor" is
useless for allocation because construction risk is not one thing: it decomposes into workmanship and
plant performance, programme and productivity, procurement and logistics, ground conditions,
third-party interfaces, input prices and permit conditions, and those seven items belong to different
parties on different grounds. The test of adequate granularity is operational: **a register is
granular enough when every line could in principle be allocated separately.** Aggregating two risks
with different natural owners into one line guarantees that one of them is mis-allocated, and the
mis-allocation is invisible because the line has only one owner column.

Two consequences follow. Identification must run through the **cash-flow model**, not around it: a
risk that cannot be expressed as a change to a modelled line — revenue, an operating cost, capex, a
timing — cannot be quantified, priced or allocated on any basis but instinct. And identification must
be **adversarial**, because a sponsor's register supports a case while a lender's supports a doubt, so
the sponsor's is systematically the shorter of the two. A leader who has not written the lender's
register before the lender does (Domain 13) will negotiate from a document already discredited.

### 11.1.2 The two grounds for transfer

**The principle.** There are exactly two defensible reasons to move a risk from one party to another,
and one common indefensible one.

**Control.** The transferee can change the probability or the impact. A contractor can supervise
welding, sequence a programme and expedite a supplier; when it holds the consequence, it does those
things better. Transfer on control grounds creates value because it **changes the underlying
distribution**, not merely who observes it.

**Capacity.** The transferee cannot change the distribution but has a lower cost of bearing it —
because it is diversified across many such exposures (an insurer), because the exposure is small
relative to its balance sheet (a state offtaker facing a devaluation that would destroy an SPV), or
because it can hedge in a market the transferor cannot access. Transfer on capacity grounds creates
value without touching the distribution, by moving the exposure to a cheaper holder of it.

**Bargaining power** is the indefensible ground. A party with a weak negotiating position can be made
to accept a risk it neither controls nor can cheaply bear. The transfer looks free because it is not
priced in the agreement, but it is priced somewhere: in a claims strategy, in a contingency the
counterparty holds privately, in a bid that fails at the diligence stage, or — most expensively — in a
counterparty that fails when the risk crystallises and hands the exposure back at the worst possible
moment (11.3.3). **A risk allocated to a party that can neither control nor absorb it has not been
transferred; it has been hidden.**

### 11.1.3 Pricing the transfer

**The arrangement.** Nothing new is needed beyond `EMV` (PML-AI Domain 8, KA 8.2.2: `EMV` = probability
× impact), arranged three ways:

```
Expected cost if retained          = EMV(transferor's own p and impact)
Loaded premium the transferee wants = EMV(transferee's own p and impact) × (1 + loading)
Net value of transfer               = expected cost if retained − loaded premium
Breakeven premium                   = expected cost if retained
Breakeven loading                   = (transferor's EMV ÷ transferee's EMV) − 1
```

The **breakeven loading** is the number that carries the professional insight, because it separates
the two grounds for transfer arithmetically. Where the transferee controls the risk, its own `EMV` is
much lower than the transferor's, so the breakeven loading is large and a generous margin still leaves
value on the table. Where it does not control the risk, its `EMV` equals or exceeds the transferor's,
the breakeven loading is zero or negative, and **no negotiation over margin can rescue the transfer.**

**Worked example 11.1.3 — Kestrel's construction register, priced item by item.**

1. **Setup.** Before award, Kestrel's sponsors hold eight quantified construction threats plus one
   opportunity. The preferred bidder is asked to price each item into a fixed-price wrap. It quotes
   each on **its own** probability and impact estimates, with a **40 % loading** for capital,
   uncertainty and margin (a stated assumption, not a market constant: the loading reflects the
   transferee's cost of capital and its own uncertainty, and is itself negotiable). Determine which
   items should transfer.
2. **Formula.** As above, per item, then aggregated by decision.
3. **Substitution and result.**

   | ID | Item | Owner `p` | Owner impact | Owner `EMV` | Bidder `p` | Bidder impact | Bidder `EMV` | Premium ×1.40 | Net value |
   |---|---|---|---|---|---|---|---|---|---|
   | A1 | Workmanship, plant defects, performance shortfall | 0.30 | 6,000,000 | 1,800,000 | 0.12 | 4,000,000 | 480,000 | 672,000 | **+1,128,000** |
   | A2 | Construction programme and labour productivity | 0.35 | 5,200,000 | 1,820,000 | 0.20 | 3,400,000 | 680,000 | 952,000 | **+868,000** |
   | A3 | Equipment procurement and logistics | 0.25 | 3,200,000 | 800,000 | 0.15 | 2,400,000 | 360,000 | 504,000 | **+296,000** |
   | A4 | Ground conditions beyond the disclosed geotechnical baseline | 0.40 | 2,400,000 | 960,000 | 0.45 | 2,900,000 | 1,305,000 | 1,827,000 | **(867,000)** |
   | A5 | Utility diversion and third-party interface scope growth | 0.30 | 1,800,000 | 540,000 | 0.31 | 2,000,000 | 620,000 | 868,000 | **(328,000)** |
   | A6 | Membrane supply price above the contract indexation formula | 0.35 | 1,400,000 | 490,000 | 0.35 | 1,500,000 | 525,000 | 735,000 | **(245,000)** |
   | A7 | Marine intake weather standby beyond the allowance | 0.50 | 900,000 | 450,000 | 0.50 | 900,000 | 450,000 | 630,000 | **(180,000)** |
   | A8 | Permit condition requiring additional monitoring works | 0.20 | 2,000,000 | 400,000 | 0.20 | 2,000,000 | 400,000 | 560,000 | **(160,000)** |
   | A9 | Early-completion rebate *(opportunity, retained)* | 0.25 | (600,000) | **(150,000)** | — | — | — | — | — |

   **A1–A3 transfer:** owner `EMV` 4,420,000 against premiums of **2,128,000** — value created
   **+2,292,000**. **A4–A8 do not:** owner `EMV` 2,840,000 against premiums of **4,620,000** — value
   destroyed **(1,780,000)**.
4. **Result.** The wrap is drawn round A1–A3 for a priced risk premium of **USD 2,128,000** inside the
   48,000,000 EPC price (an un-risked scope of **45,872,000** plus that premium). The retained residue
   is **2,840,000 − 150,000 = USD 2,690,000** — precisely the register Domain 8 sized contingency
   against. Gross expected cost of the risk position falls from **7,110,000** wholly retained to
   **4,818,000** after allocation.
5. **Interpretation.** Four things in that table are worth more than the arithmetic that produced
   them. **The premium is not the cost; the net is.** Every dollar of the 2,128,000 premium bought
   2.0771 dollars of expected-cost reduction, and an organisation that judges a wrap by its price
   rather than by its net value will reject the transfers that pay for themselves twice over.
   **Where control exists, price barely matters.** The breakeven loading on A1–A3 is
   **190.79 %** — the bidder could charge nearly three times its own expected cost and the transfer
   would still create value, because the transfer *changes the distribution*: 6,000,000 of impact at
   0.30 becomes 4,000,000 at 0.12 when the party doing the welding carries the consequence. **Where
   control does not exist, no price works.** The breakeven loading on A4–A8 is **−13.94 %**: the bidder
   would have to accept the bundle at 86.06 % of its own expected cost before the transfer merely broke
   even. Stripping the margin out entirely still destroys **460,000**, because the bidder's own expected
   cost on those items (3,300,000) *exceeds* the owner's (2,840,000) — it must assume the worst about a
   ground investigation it did not commission, a utility corridor it does not own and a permit
   condition it cannot influence. **And the asymmetry has a shape.** A4 is the extreme case and the
   most instructive: the owner commissioned the geotechnical survey, holds the data and set the
   baseline, so the owner is the *better-informed* party. Transferring a risk to the less-informed party
   converts an information advantage into a price disadvantage. The standing professional caution is
   that the arithmetic depends entirely on the honesty of the two `EMV` columns: a sponsor who
   understates its own retained probabilities will "prove" that every transfer destroys value, and a
   bidder who overstates its own will justify any premium. **The register's probabilities are the
   negotiation**, which is why they must be evidenced (Domain 8's estimate basis, PML-AI Domain 8's
   elicitation discipline) before they are argued.

> **Fig 11.1.1 — The allocation price test.** Diverging horizontal bar chart, x-axis the net value of
> transferring each item (owner's expected cost if retained minus the contractor's loaded premium),
> −1.2m to +1.2m USD, zero rule in ink. Bars in descending net-value order: A1 **+1,128,000**,
> A2 **+868,000**, A3 **+296,000** in brand blue; A8 **−160,000**, A7 **−180,000**, A6 **−245,000**,
> A5 **−328,000**, A4 **−867,000** in crimson. Each row carries "retain <owner EMV> · premium <loaded
> premium>" beneath its label. Header: three transfers create 2,292,000, five destroy 1,780,000; the
> crimson five destroy 460,000 even at a zero margin because the contractor's own expected cost
> (3,300,000) exceeds the owner's (2,840,000); the blue three still create value at a premium 190.79 %
> above the contractor's expected cost. Footer: retained residue 2,690,000 — the register Domain 8
> provisions against. Source: PCI original. Alt text: a diverging bar chart with three positive blue
> bars for risks the contractor controls and five negative crimson bars for risks it does not, the
> largest negative bar being ground conditions.

### 11.1.4 Completion risk and the residue

**Definition.** **Completion risk** is the risk that the project does not reach commercial operation
on time, on budget, or to the performance the financing was sized against. It is the one construction
risk that cannot be wholly transferred, because its consequences fall on parties the EPC contract
does not reach: the lenders lose their repayment source, and the equity loses its return.

Three residues survive even a well-priced wrap, and Domain 5 measured each of them, so they are cited
rather than re-derived. **Timing residue:** a slip costs 24,733.33 per day in extra interest and
forgone `CFADS`, against delay damages of 20,000 per day capped at day 240 — so damages recover
80.86 % of the cost and nothing at all beyond the cap (Domain 5, KA 5.4.2). **Performance residue:**
completion at 97 % of guaranteed output requires a buy-down that restores lenders' coverage, not merely
compensation for lost revenue (Domain 5, KA 5.4.3). **Credit residue:** the whole wrap is worth the
contractor's balance sheet, priced in 11.3.3.

The professional discipline is to state the residue in the same units as the covenant. Kestrel's
retained register mean of 2,690,000, spread across the two-year construction period, is not a coverage
number; **capitalised into debt it becomes one.** Domain 8 demonstrated the mechanism precisely: the
600,692 by which the funded contingency fell short of the register P80, capitalised, moved the `DSCR`
from 1.2743 to 1.2564 and cut annual headroom by 23.1 % for the whole twelve-year loan life. That is
the sentence a construction risk register should end with: not "the exposure is 2.69 million" but
"the exposure, if funded by debt, costs us this much covenant headroom for twelve years."

### AI in this KA

Risk identification is one of the strongest genuine applications of language models in project
finance, and one of the most dangerous places to accept an output unedited. **Where it earns its
place:** extracting risk allocation from a long contract suite into a matrix of item, clause,
mechanism and cap; cross-checking a sponsor's register against the risk taxonomies implied by
comparable transactions and the lender's diligence scopes, and reporting what is *missing* — the
omission is the failure mode registers have, and a machine that has read a thousand registers is good
at noticing an absent line; and normalising register wording so that two advisers' items can be
compared at all. **Where it must not go:** the probability and impact columns. Those are elicited
judgments with an evidentiary basis, and a model asked to supply them will produce plausible central
values with no basis, which then propagate through `EMV`, the P80, the contingency and the debt
sizing. A model may *challenge* a probability ("this is inconsistent with the ground investigation
extent you described"); it may not *supply* one. **Verification, concretely:** for an extracted
allocation matrix, sample at least ten lines back to the clause text and confirm the cap and the
mechanism, because the recurring error is a correct owner attached to a wrong cap; for a
machine-suggested register addition, require that it be expressed as a change to a named model line
before it is accepted, which filters the plausible-but-unquantifiable; and never let a model perform
the arithmetic of 11.1.3 without an independent recomputation, since a single transposed probability
flips a transfer decision. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 11.1

| Term | Meaning |
|---|---|
| **Risk register (financing)** | Identified events with probability, impact, owner **and mechanism**; granular enough for each line to be allocated separately. |
| **Grounds for transfer** | Control (changes the distribution) and capacity (cheaper holder); bargaining power is neither. |
| **Loaded premium** | Transferee's own `EMV` × (1 + loading) — what the transfer actually costs. |
| **Net value of transfer** | Transferor's `EMV` − loaded premium; negative means retain. |
| **Breakeven loading** | (transferor `EMV` ÷ transferee `EMV`) − 1; negative means no price works. |
| **Retained residue** | The register left after allocation — the thing contingency and reserves are sized against. |
| **Completion risk** | Risk of not reaching commercial operation on time, on budget or to guaranteed performance; never wholly transferable. |

### Sample MCQs — KA 11.1

**MCQ 11.1-A `[11.1.3 · Application]`** An owner's `EMV` on an item is 960,000. The contractor
assesses the same item at `p` 0.45 and impact 2,900,000 and applies a 40 % loading. The net value of
transferring is:
- A. +573,000
- B. −345,000
- C. −867,000 ✅
- D. −1,827,000

*Rationale:* Premium = 0.45 × 2,900,000 × 1.40 = 1,827,000; net = 960,000 − 1,827,000 = −867,000.
B is the net at a **zero** loading (960,000 − 1,305,000) and understates the destruction; D is the
premium alone, ignoring the retained cost avoided; A compares the owner's gross **impact**
(2,400,000) with the premium, omitting probability from the retained side.

**MCQ 11.1-B `[11.1.3 · Analysis]`** A bundle of items has an owner `EMV` of 2,840,000 and a
contractor `EMV` of 3,300,000. The correct conclusion is:
- A. transfer if the loading can be negotiated below 40 %
- B. transfer, because the contractor is better placed to manage construction
- C. do not transfer: the breakeven loading is −13.94 %, so even at a zero margin the transfer destroys 460,000 ✅
- D. transfer, and recover the premium through the contingency

*Rationale:* The transferee's own expected cost exceeds the transferor's, so the loading is not the
problem — the distribution is (11.1.3). A treats a structural result as a pricing negotiation; B
asserts control where the items are ground, interface, index and permit risks the contractor does not
control; D funds a value destruction twice.

**MCQ 11.1-C `[11.1.2 · Analysis]`** An SPV with weak negotiating position accepts a risk it neither
controls nor can absorb. The most accurate description of what has happened is:
- A. an efficient transfer, since the price was zero
- B. the risk has not been transferred but hidden — it will reappear as a claim, a private contingency, a failed bid or a counterparty default ✅
- C. a transfer on capacity grounds
- D. a transfer on control grounds

*Rationale:* Neither ground applies, and an unpriced allocation is not a costless one (11.1.2). C and
D name grounds that are absent by the stem's own terms.

**MCQ 11.1-D `[11.1.3 · Evaluation]`** The allocation arithmetic shows the five uncontrollable items
destroying 1,780,000 of value at a 40 % loading. A colleague proposes reducing the owner's retained
probabilities so that the full wrap can be recommended to the board on price grounds. The soundest
professional position is:
- A. adjust them: the register is a negotiating instrument and the full wrap has strategic value
- B. hold the retained probabilities to their evidence base and negotiate item by item on that
  evidence, because the register's probabilities *are* the negotiation and an input adjusted to reach
  a conclusion has inverted the analysis ✅
- C. adopt the bidder's probabilities throughout, since the bidder is the party pricing the risk
- D. abandon the arithmetic, since probabilities are subjective and cannot support a decision

*Rationale:* The whole result rests on the honesty of the two `EMV` columns: a sponsor who understates
its retained probabilities will "prove" that every transfer destroys value, and a bidder who
overstates its own will justify any premium (11.1.3). A produces a recommendation the lender's
diligence will reverse, at the cost of the register's credibility everywhere else. C imports the
less-informed party's assumptions about a ground investigation the owner commissioned, which is how an
information advantage becomes a price disadvantage. D discards a rule that is transparent and
challengeable in favour of instinct, which is neither.

**MCQ 11.1-E `[11.1.3 · Evaluation]`** The preferred bidder declines to price Kestrel's register items
separately and offers a single wrap covering all eight threats for a premium of **6,748,000** — the sum
of the 2,128,000 it quoted on A1–A3 and the 4,620,000 it quoted on A4–A8. Retaining the whole register
has an expected cost of 7,110,000; transferring A1–A3 alone and retaining the rest costs 4,818,000. The
recommendation is:
- A. accept the bundle: at an expected cost of **6,598,000** it beats full retention by **512,000**, and
  a single wrap removes every argument about which item a loss belongs to
- B. refuse the bundle and require the items to be priced line by line — A1–A3 transferred and A4–A8
  retained costs 4,818,000, which is **1,780,000** better than the bundle ✅
- C. refuse all transfer: the bidder's own expected cost on A4–A8 exceeds the owner's, so no wrap
  creates value
- D. accept the bundle and negotiate the 40 % loading down, since the loading is where the value
  destruction sits

*Rationale:* The bundle genuinely beats full retention, which is exactly what makes A the trap — it is
defensible on its own comparison and leaves 1,780,000 on the table, because it buys five items the
bidder cannot influence at the same time as three it can (11.1.3). C generalises the A4–A8 result across
the register and forgoes the 2,292,000 the control-based transfers create. D misplaces the defect:
stripping the loading out entirely still destroys 460,000 on A4–A8, because the bidder's own expected
cost there (3,300,000) exceeds the owner's (2,840,000). The negotiating point that follows from B is
that unbundling is itself the ask — a bidder that will not price line by line is charging for the items
it would rather not discuss.

**MCQ 11.1-F `[11.1.2 · Comprehension]`** An insurer accepts a risk it cannot influence in any way.
Expressed in this domain's terms, that transfer rests on:
- A. control, since the insurer's loss-prevention requirements change the project's behaviour
- B. capacity: the insurer cannot change the distribution but holds it more cheaply, being diversified
  across many such exposures — so value is created by moving the exposure to a cheaper holder rather
  than by improving it ✅
- C. bargaining power, since the project has no alternative
- D. no recognised ground, which is why insurance is a cost rather than a transfer

*Rationale:* The two defensible grounds are control, which changes the underlying distribution, and
capacity, which changes only who holds it; diversification is the classic capacity case (11.1.2). A
describes a real secondary effect and names the wrong ground — the insurer's requirements do not put it
in charge of the welding. C describes the indefensible ground and does not apply: the insurer is a
willing party pricing an exposure it can bear. D denies a transfer that the pricing test of 11.1.3
values in the ordinary way, as expected cost retained less loaded premium.

### Self-check — KA 11.1

1. *State the test for adequate register granularity.* — Every line could in principle be allocated
   separately; if two risks in one line have different natural owners, one is mis-allocated invisibly.
2. *Why does the breakeven loading separate the two grounds for transfer?* — Control lowers the
   transferee's `EMV` below the transferor's, making the breakeven loading large; without control it
   is zero or negative, and no margin negotiation helps.
3. *State Kestrel's allocation outcome in one line.* — 2,128,000 of premium bought 4,420,000 of
   expected-cost reduction (net +2,292,000); 4,620,000 of offered premium against 2,840,000 of
   retained `EMV` was declined; the residue is 2,690,000.

---

## Knowledge Area 11.2 — Market risks and operations

*Topics: 11.2.1 why market risk resists transfer · 11.2.2 pass-through as a coverage multiplier ·
11.2.3 the indexation mismatch · 11.2.4 operating risk and the O&M interface.*

### 11.2.1 Why market risk resists transfer

Demand, price and supply risks are the hardest to allocate because **no party to the transaction
controls them.** The pricing test of 11.1.3 therefore predicts what practice confirms: transfers of
market risk are either expensive or illusory. Domain 7 built the machinery — contracted versus
merchant structures, tariff architecture, minimum revenue guarantees, offtaker credit — and this KA
adds only the allocation view of it, which reduces to three findings.

**Transfer to the offtaker is a capacity transfer, not a control transfer.** An availability-based
structure moves volume risk to a payer that is diversified or sovereign-adjacent where the SPV is
neither, and it is bought with a lower tariff — Domain 7 priced that trade at 10,679,727 of debt
capacity relative to a volume tariff.

**Transfer to a guarantor is a capacity transfer with a credit tail.** A minimum revenue guarantee is
worth the guarantor's balance sheet in the state of the world where it is called — by construction,
the state in which the guarantor's other exposures are also stressed. Domain 7 valued Kestrel's
guarantee at 2,625,026 of released equity; the discipline is to value it for the coverage it unlocks
and then discount it for that correlation.

**Transfer to the contractor or operator is usually a mis-allocation.** Asking an O&M contractor to
carry input-price risk it cannot hedge produces exactly the A6 result of 11.1.3: a premium above the
retained cost, because a counterparty that can only insure itself by assuming the worst case prices
the worst case.

### 11.2.2 Pass-through as a coverage multiplier

**Definition.** A **pass-through** is a contractual term under which a defined movement in a defined
input cost is recovered in revenue, in whole or in a stated share. Its financial effect is not to
remove the exposure but to **divide** it, and the arithmetic of that division is more powerful than
practitioners expect.

If an input has a base annual cost `C` and a share `φ` of its price movement is passed through, the
residual cash effect of a 1 % rise in that input's price is `C × 0.01 × (1 − φ)`. Because covenant
tolerance is fixed headroom divided by residual exposure per unit, **the tolerance is multiplied by
1/(1 − φ)**.

**Worked example 11.2.2 — what a 70 % power pass-through is worth to Kestrel.**

1. **Setup.** Of Kestrel's 4,500,000 of cash operating costs, **1,800,000 is electricity** — the
   dominant input in reverse-osmosis desalination. The water-purchase agreement passes through
   **70 %** of movements in the reference power tariff. Annual covenant headroom is
   **372,437.72** (Domain 10). Compute the tolerance to a power-price rise with and without the
   pass-through, and the position after a 25 % rise.
2. **Formula.** Residual per 1 % rise = `1,800,000 × 0.01 × (1 − φ)`; tolerance = headroom ÷ residual
   per 1 %; multiplier = `1/(1 − φ)`.
3. **Substitution.** With `φ` = 0: `1,800,000 × 0.01 = 18,000`; `372,437.72 / 18,000`. With
   `φ` = 0.70: `1,800,000 × 0.01 × 0.30 = 5,400`; `372,437.72 / 5,400`. At a 25 % rise:
   residual `1,800,000 × 0.25 × 0.30 = 135,000`.
4. **Result.** Unprotected, the covenant fails on a **20.6910 %** power-price rise. With 70 %
   pass-through it fails on a **68.9699 %** rise — a tolerance multiplier of **3.3333×**. A 25 % rise
   costs **135,000** of `CFADS`, leaving 6,249,000 and a `DSCR` of **1.2474**; unprotected the same
   rise costs 450,000, leaving 5,934,000 and a `DSCR` of **1.1845** — a breach.
5. **Interpretation.** The teachable content is the leverage. Negotiating the pass-through share from
   zero to 70 % triples the project's tolerance to its single largest cost driver, and the marginal
   value of each increment is *increasing*: moving from 0 to 50 % doubles tolerance, but moving from
   70 % to 85 % doubles it again from a much higher base. The professional consequence is that a
   pass-through share is a **coverage term, not a commercial detail**, and it belongs in the same
   negotiating tranche as the covenant level. Two cautions bound the result. First, a pass-through is
   only as good as its **reference index and its reset frequency**: a share of 70 % reset annually
   leaves eleven months of exposure inside each year, and a share indexed to a published tariff the
   plant does not actually pay leaves a basis risk that no share removes. Second, the tolerance
   figures assume the rise is permanent and everything else holds; combined with any other adverse
   movement the tolerances share the same 372,437.72 of headroom and are **not additive** — which is
   the single most common misuse of a sensitivity table (Domain 7, KA 7.4.3 built the joint matrix
   for exactly this reason).

### 11.2.3 The indexation mismatch

**Definition.** An **indexation mismatch** exists when the index that escalates revenue is not the
driver that escalates cost. It is the most under-detected structural risk in project finance because
it is invisible at close — year one is unaffected by construction — and because it does not appear in
any single-year sensitivity. It appears as a **trend in coverage**, and a model that reports only the
base-case minimum `DSCR` over the loan life will report it, while a model that reports the year-one
ratio will not.

**Worked example 11.2.3 — Kestrel's CPI revenue against power-driven cost.**

1. **Setup.** Domain 7 established that **80 % of Kestrel's tariff is indexed to a consumer price
   index**, assumed at 2.5 % a year. On the cost side, 2,700,000 of operating cost tracks that same
   index, but the **1,800,000 of power escalates at 8.0 %** on the sponsors' own forecast. Cash tax
   (516,000) and the working-capital movement (600,000) are held constant, a simplifying assumption
   stated so the mismatch is isolated. Trace `CFADS` and `DSCR` across the loan life against the
   1.20× covenant.
2. **Formula.** Revenue(t) = `12,000,000 × (1 + 0.80 × 0.025)^(t−1)`; CPI cost(t) =
   `2,700,000 × 1.025^(t−1)`; power(t) = `1,800,000 × 1.08^(t−1)`;
   `CFADS(t)` = revenue − CPI cost − power − 1,116,000.
3. **Substitution and result.**

   | Year | Revenue | CPI-linked cost | Power | `CFADS` | `DSCR` |
   |---|---|---|---|---|---|
   | 1 | 12,000,000 | 2,700,000 | 1,800,000 | **6,384,000** | **1.2743** |
   | 3 | 12,484,800 | 2,836,688 | 2,099,520 | 6,432,593 | 1.2840 |
   | 5 | 12,989,186 | 2,980,295 | 2,448,880 | **6,444,011** | **1.2863** |
   | 8 | 13,784,228 | 3,209,452 | 3,084,884 | 6,373,893 | 1.2723 |
   | 10 | 14,341,111 | 3,371,930 | 3,598,208 | 6,254,972 | 1.2486 |
   | 12 | 14,920,492 | 3,542,634 | 4,196,950 | **6,064,908** | **1.2106** |
   | 15 | 15,833,745 | 3,815,029 | 5,286,949 | 5,615,767 | 1.1210 |

   Coverage **improves** to year five, then deteriorates continuously. The first covenant breach falls
   in **year 13** — one year after the loan matures. The **breakeven power escalation** that would put
   year twelve exactly on the covenant is **8.1241 %**.
4. **Result.** The structure survives its own indexation mismatch across the loan life, but by
   **12.41 basis points of assumed power escalation** and with **85.68 %** of its covenant headroom
   consumed: year-twelve headroom is **53,345.25** against 372,437.72 at close.
5. **Interpretation.** This is the most important arithmetic in the KA and the least likely to be
   found in a base-case pack. **Coverage improving early conceals a deteriorating structure.** For the
   first five years the 2 % effective revenue escalation outruns a power cost that is still small in
   absolute terms; by year twelve power has grown from 1,800,000 to 4,196,950 while the same input
   escalated at CPI would have reached only 2,361,756 — a wedge of **1,835,194** a year, which is 4.93
   times the entire original headroom. **The structure is not robust; it is lucky.** A power forecast
   of 8.1241 % rather than 8.0 % breaches in year twelve, and no one in the transaction would claim
   twelve basis points of precision on a twelve-year commodity forecast. That is the honest
   characterisation to put in front of a credit committee, and it points to three interventions, each
   priceable: raise the power pass-through share (11.2.2 shows the leverage), index part of the tariff
   to the power reference rather than to CPI, or size a reserve that funds the late-life wedge. **And
   the diagnostic is cheap.** Any level-escalation model can be tested in one line: compute the
   revenue-weighted escalation rate and the cost-weighted escalation rate, and if the cost rate is
   higher the structure has a mismatch whose only question is when it bites. Kestrel's are 2.00 % and
   4.70 % respectively — a 270-basis-point gap that a single-year sensitivity cannot see. The standing
   caution is that the arithmetic is only as good as the differential forecast: the *level* of power
   prices matters far less here than the **spread** between power escalation and the revenue index, and
   a model that stresses both at the same rate will show no exposure at all while destroying none of it.

### 11.2.4 Operating risk and the O&M interface

Operating risk allocation is a smaller-scale replay of 11.1.3 with one structural difference: the O&M
contractor is a **long-lived counterparty with a thin balance sheet relative to the exposure**, so
transfers to it are capped early and often. The practical allocation set is: availability and
performance to the operator, within a liability cap and against a fee at risk; input volumes to the
operator, which controls consumption; input **prices** to the offtaker or retained, since nobody in the
structure controls them (11.2.2); major maintenance to a reserve rather than to a party (Domain 10's
MRA); lifecycle replacement to whichever party the handback standard makes accountable.

The interface that produces the most disputes is the boundary between **availability** and **force
majeure**, because it decides whether a lost month is the operator's liability or nobody's. Domain 7
computed the coverage sensitivity that makes this boundary financially material: Kestrel breaches its
1.20× covenant at **92.086 %** availability against a 95 % guarantee, so 2.9 percentage points of
availability separate compliance from breach, and the definition of what counts as an excusable outage
is worth more than the damages rate attached to it.

### AI in this KA

**Where it earns its place:** monitoring. Pass-through and indexation terms are formulae with
published inputs, and an automated monthly recomputation of the escalation wedge (cost-weighted
escalation less revenue-weighted escalation, and the projected year of covenant breach at current
observed rates) is exactly the sort of persistent arithmetic that humans stop doing after financial
close. Anomaly detection on input consumption against a plant model is similarly strong, and it is
where a model earns operational trust before it is trusted with anything consequential.
**Where it must not go:** forecasting the differential. The whole exposure in 11.2.3 turns on the
spread between power escalation and a consumer price index over twelve years, and a model trained on
recent history will extrapolate a regime rather than a relationship. Use scenarios owned by named
people, not a machine point forecast, and note that a plausible-looking generated forecast is more
dangerous than an obviously crude one because it survives review. **Verification, concretely:** require
any automated escalation monitor to reproduce the year-one figures of the closing model exactly before
its later years are believed — Kestrel's 6,384,000 and 1.2743 are the check — and require that any
machine-produced sensitivity table state whether its cases are independent or joint, because
non-additive tolerances presented additively is the error the tool will not flag.

### Key terms — KA 11.2

| Term | Meaning |
|---|---|
| **Pass-through share `φ`** | Contractual share of an input-price movement recovered in revenue; multiplies coverage tolerance by 1/(1 − `φ`). |
| **Basis risk (pass-through)** | Exposure remaining because the reference index differs from the price actually paid. |
| **Indexation mismatch** | Revenue index differs from the driver of cost escalation; appears as a trend in coverage, not in a single-year sensitivity. |
| **Escalation wedge** | Cumulative cash difference between a cost escalating at its own driver and the same cost escalating at the revenue index. |
| **Fee at risk** | The portion of an operator's fee forfeited on performance failure — its economic stake in the allocation. |
| **Excusable outage** | An unavailability event that does not count against the availability guarantee; the availability/force-majeure boundary. |

### Sample MCQs — KA 11.2

**MCQ 11.2-A `[11.2.2 · Application]`** An input costs 1,800,000 a year; 70 % of its price movement
passes through to revenue; covenant headroom is 372,437.72. The input-price rise that breaches the
covenant is closest to:
- A. 20.7 %
- B. 69.0 % ✅
- C. 29.5 %
- D. 100.0 %

*Rationale:* Residual per 1 % = 1,800,000 × 0.01 × 0.30 = 5,400; 372,437.72 / 5,400 = 68.97 %.
A ignores the pass-through (the `φ` = 0 answer); C uses the **passed-through** share 0.70 in place of the
retained share 0.30 (372,437.72 / 12,600 = 29.56 %) — the commonest sign error here; D assumes
only a doubling of the input can breach, which no calculation supports.

**MCQ 11.2-B `[11.2.3 · Analysis]`** A project's `DSCR` is 1.2743 in year one, rises to 1.2863 in
year five and falls to 1.2106 in year twelve. The soundest reading is:
- A. the structure is robust — coverage never breaches
- B. an indexation mismatch is consuming headroom; 85.7 % of it is gone by year twelve and the breach falls just outside the loan life ✅
- C. the model contains an error, since coverage cannot both rise and fall
- D. the improvement to year five shows revenue growth exceeding costs

*Rationale:* The shape is diagnostic of a cost driver escalating faster than the revenue index
(11.2.3). A reads compliance as robustness and ignores that twelve basis points of forecast
separate the two; C mistakes a normal profile for a defect; D is true only for the first five years
and misses the trend.

**MCQ 11.2-C `[11.2.1 · Analysis]`** Why is asking an O&M contractor to bear input-price risk usually
a mis-allocation?
- A. O&M contractors are not creditworthy
- B. it neither controls the price nor can hedge it, so it prices the worst case and the premium exceeds the retained expected cost ✅
- C. input prices are always passed through by law
- D. the risk is immaterial

*Rationale:* This is the 11.1.3 result applied to operations — neither ground for transfer is present.
A is a separate (and secondary) objection; C asserts a universal legal position that does not exist;
D is contradicted by 11.2.2's arithmetic.

**MCQ 11.2-D `[11.2.3 · Evaluation]`** Asked by a credit committee to test escalation, a model adds a
percentage point to both the consumer price index and the power escalation rate, and reports that
year-twelve `DSCR` rises from 1.2106 to 1.3088. The soundest reading is:
- A. the structure is insensitive to escalation: both drivers were stressed and coverage improved
- B. the test is not evidence — the exposure is the differential, which widens only from 2.70 to 2.90
  percentage points under this stress while every escalating line simply grows, so the case must be
  re-run on the spread between the cost driver and the revenue index ✅
- C. the model contains an error, since escalation must reduce coverage
- D. the test is adequate once a volume stress is added alongside it

*Rationale:* Kestrel's revenue-weighted escalation of 2.00 % against a cost-weighted 4.70 % *is* the
exposure; a stress that lifts both leaves that gap almost unchanged and makes the reported ratio look
better, so it invites the opposite of the correct conclusion (11.2.3). A accepts a favourable output
without asking what was varied — the level of power prices matters far less here than the spread. C
mistakes a modelling artefact for an arithmetic defect. D adds a second variable without repairing
the first, and the joint table it implies would still test the wrong thing.

**MCQ 11.2-E `[11.2.2 · Comprehension]`** A pass-through of 70 % of movements in an input price
differs from a fixed-price supply contract for the same input in that the pass-through:
- A. removes the exposure entirely, as the fixed price does
- B. divides the exposure rather than removing it — the project keeps 30 % of every movement, and what
  it keeps also depends on the reference index and the reset frequency — whereas a fixed price
  replaces the price exposure with the supplier's willingness and ability to hold the price ✅
- C. removes the exposure while a fixed price merely defers it
- D. has no effect on coverage, since the cost is incurred either way

*Rationale:* A pass-through divides an exposure and multiplies coverage tolerance by 1/(1 − `φ`); it
eliminates nothing, and a share indexed to a published tariff the plant does not actually pay leaves
basis risk inside the protected portion (11.2.2). A and C misstate what each instrument does — and a
fixed price substitutes a counterparty credit question for a market one. D ignores that the retained
residual falls straight through to `CFADS`, which is the quantity the covenant divides.

**MCQ 11.2-F `[11.2.4 · Evaluation]`** Kestrel's O&M agreement carries a 95 % availability guarantee
with damages attached, and the project breaches its 1.20× covenant at **92.086 %** availability — so
2.9 percentage points of availability separate compliance from breach. With one negotiating session
left on the O&M agreement, the finance leader should spend it on:
- A. the damages rate for missed availability, which is the operator's financial incentive to perform
- B. the definition of an excusable outage — the availability/force-majeure boundary decides whether a
  lost month counts against the guarantee at all, and it is worth more than the rate attached to it
  because force majeure suspends performance obligations and never suspends debt service ✅
- C. the liability cap, which is scaled to the fee and therefore too small whatever the rate
- D. the fee at risk, which gives the operator a running stake rather than a terminal liability

*Rationale:* With 2.9 points of availability between compliance and breach, the question that decides
the covenant is which lost days are counted, not what is paid for the days that are — and an outage
reclassified as excusable lands squarely on coverage with no recovery at all (11.2.4, 11.3.4). A funds
the consequence rather than preventing it, and an operator's damages are capped on its fee in any case.
C and D are both sound and both weaker: the cap is genuinely too small — a 30-day outage costs 742,000
against a half-fee cap of 600,000 — and fee at risk is genuinely the better incentive design, but each
allocates money after the event, while the boundary definition determines whether there is a claim to
make. The wider lesson is the one 11.3.4 states: insurance waiting periods, cure periods and
availability carve-outs are calibrated in time while covenants are calibrated in cash, and somebody
has to perform the translation.

### Self-check — KA 11.2

1. *State the pass-through multiplier and Kestrel's two tolerances.* — Tolerance is multiplied by
   1/(1 − `φ`); 20.6910 % unprotected, 68.9699 % at `φ` = 0.70, a 3.3333× multiplier.
2. *What single test detects an indexation mismatch?* — Compare the revenue-weighted escalation rate
   with the cost-weighted rate (2.00 % against 4.70 % for Kestrel); a higher cost rate means the only
   open question is when it bites.
3. *Why are separate tolerance percentages not additive?* — They all consume the same 372,437.72 of
   headroom; joint movements require a joint matrix.

---

## Knowledge Area 11.3 — Counterparty, political, currency, interest-rate and force-majeure risk

*Topics: 11.3.1 interest-rate exposure and the hedge ratio · 11.3.2 currency mismatch · 11.3.3 the
counterparty risk inside the allocation · 11.3.4 political, regulatory and force majeure.*

### 11.3.1 Interest-rate exposure and the hedge ratio

**The structure.** Kestrel's senior facility is priced as a floating reference rate plus a **200 basis
point margin**. At close the reference stood at **4.00 %**, giving the 6.00 % all-in rate Domains 3
and 10 used. The amortisation schedule fixes **principal** — year-one principal is 2,489,635.23 — so
only the interest leg floats, and debt service in any period is `principal + outstanding × all-in
rate`. An interest-rate swap is available that fixes the reference at **4.20 %**, above the spot rate
because the curve slopes upward; the all-in fixed cost is therefore **6.20 %**.

**Worked example 11.3.1 — what an unhedged Kestrel is actually exposed to.**

1. **Setup.** Debt 42,000,000; year-one principal 2,489,635.23; `CFADS` 6,384,000; covenant 1.20×,
   lock-up 1.15×. Compute year-one `DSCR` across a ±200 basis point range in the reference rate,
   unhedged, 75 % hedged and fully hedged; find the rate at which each threshold is crossed; and find
   the minimum hedge ratio that survives a 200 basis point shock.
2. **Formula.** Blended rate = `swap all-in × h + floating all-in × (1 − h)`; debt service =
   `2,489,635.23 + 42,000,000 × blended rate`; `DSCR` = `6,384,000 ÷ debt service`. Breakeven rate:
   `((CFADS ÷ target DSCR) − principal) ÷ debt`. Minimum hedge ratio:
   `(shocked rate − breakeven rate) ÷ (shocked rate − swap rate)`.
3. **Substitution and result.**

   | Reference shift | All-in rate | Interest | Debt service | `DSCR` unhedged | `DSCR` at h = 75 % | `DSCR` at h = 100 % |
   |---|---|---|---|---|---|---|
   | −200 bp | 4.00 % | 1,680,000 | 4,169,635.23 | **1.5311** | 1.3129 | 1.2533 |
   | −100 bp | 5.00 % | 2,100,000 | 4,589,635.23 | 1.3910 | 1.2851 | 1.2533 |
   | base | 6.00 % | 2,520,000 | 5,009,635.23 | **1.2743** | 1.2585 | 1.2533 |
   | +50 bp | 6.50 % | 2,730,000 | 5,219,635.23 | 1.2231 | 1.2456 | 1.2533 |
   | +100 bp | 7.00 % | 2,940,000 | 5,429,635.23 | **1.1758** | 1.2330 | 1.2533 |
   | +200 bp | 8.00 % | 3,360,000 | 5,849,635.23 | **1.0914** | **1.2085** | 1.2533 |

   Unhedged breakeven rates: **6.7390 % (+73.9 bp)** for the 1.20× covenant, **7.2897 % (+129.0 bp)**
   for the 1.15× lock-up, **9.2723 % (+327.2 bp)** before scheduled debt service cannot be paid at all.
   Full hedge: interest 2,604,000, debt service 5,093,635.23, `DSCR` **1.2533** at any reference rate,
   at a year-one cash cost of **84,000**. Minimum hedge ratio surviving +200 bp at the covenant:
   **70.0576 %**.
4. **Result.** Unhedged, **74 basis points** of reference rate stand between this project and a
   covenant breach. The swap costs **0.0210 of `DSCR`** and removes **0.4397 of `DSCR` range** —
   20.92 units of range eliminated per unit of coverage given up.
5. **Interpretation.** Three professional conclusions, in order of how often they are missed.
   **Interest-rate exposure is a covenant exposure long before it is a payment exposure.** The
   distance to a breach is 73.9 basis points; the distance to a payment failure is 327.2. A treasury
   discussion framed around "can we afford the interest?" is answering the wrong question by a factor
   of four and a half, and the right question — *at what reference rate does our first covenant
   fail?* — has a single computable answer that belongs on the same dashboard as Domain 10's
   6,011,562.28 cash trigger. **The hedge is bought with coverage, and the exchange rate is
   extraordinarily favourable.** Giving up 0.0210 of ratio to remove 0.4397 of range is not a close
   call, and the reason it is nonetheless argued is that the 84,000 is certain and visible while the
   range is contingent and invisible. That asymmetry of salience, not the economics, is what produces
   under-hedged project financings. **And the right answer is a ratio, not a binary.** A 75 % hedge
   leaves 10,500,000 floating, holds the covenant at +200 bp with **1.2085**, and costs only
   **63,000** a year — three-quarters of the full hedge cost for the protection that matters, while
   retaining a quarter of the benefit of falling rates. The **70.0576 %** minimum is why facilities
   covenant a hedging ratio rather than a full hedge: below it the structure fails the lenders' own
   rate stress, and above it the incremental protection is bought at a diminishing rate. Two cautions
   belong with the arithmetic. A swap is not free of risk — it introduces **mark-to-market exposure**
   (a break cost on prepayment or refinancing, which can be large when rates have fallen) and a
   **hedge counterparty** whose own credit is now inside the structure (11.3.3). And the figures above
   are year one; the exposure declines with the outstanding balance, so a hedge profile should amortise
   with the loan rather than sit flat, or the project will be over-hedged in its later years and paying
   for protection it no longer needs.

> **Fig 11.3.1 — Coverage against the reference rate, at three hedge ratios.** Line chart, x-axis
> shift in the reference rate from its 4.00 % level at close, −200 to +200 basis points; y-axis
> year-one `DSCR`, 1.04–1.56. Three series: unhedged in crimson (1.5311 at −200 bp, **1.2743** at
> base, 1.0914 at +200 bp), 75 % hedged in brand blue (**1.2085** at +200 bp), fully hedged as a
> dashed ink horizontal at **1.2533**. Dashed ink threshold at the **1.20 covenant** (labelled with
> the 6,011,562 cash trigger) and a lighter dashed line at the **1.15 lock-up**. Crimson marker where
> the unhedged line crosses the covenant, annotated **+73.9 bp — unhedged breach**. Header: a swap
> costs 0.0210 of coverage and removes 0.4397 of range — 20.92 units of range per unit given up; and
> 70.06 % is the minimum hedge ratio that survives a 200 bp shock. Source: PCI original. Alt text:
> three coverage curves falling as the reference rate rises, the unhedged curve crossing the covenant
> threshold at about seventy-four basis points while the hedged lines stay above it throughout.

### 11.3.2 Currency mismatch

**Definition.** A **currency mismatch** exists when a project's revenue and its debt service are
denominated in different currencies. It is the most destructive of the financial risks because,
unlike an interest-rate movement, a devaluation is **unbounded and correlated with everything else
that goes wrong in a host economy** — and because it cannot be hedged for a twelve-year tenor in most
markets at any price a project can pay.

Kestrel's water-purchase agreement is denominated in USD, which is why Domains 2 to 10 could treat its
revenue as a USD figure. That term was not free, and this KA prices the alternative the offtaker
pressed for: a tariff denominated wholly in the host currency (`HC`), with the USD debt unchanged.

**Worked example 11.3.2 — the tariff term that was worth more than the tariff.**

1. **Setup.** Assume the exchange rate at close is **`HC` 4.00 = USD 1** (an illustrative rate for a
   fictitious currency; no real currency or jurisdiction is implied). Under the `HC`-tariff variant,
   revenue is `HC` 48,000,000; of the 4,500,000 of cash operating costs, **1,350,000 is USD-denominated**
   (imported membranes, chemicals, specialist spares) and the balance of 3,150,000 is local
   (`HC` 12,600,000); cash tax `HC` 2,064,000 and the working-capital movement `HC` 2,400,000 are
   local. Debt service remains USD 5,009,635.23. Compute the coverage consequence of devaluation and
   the exchange-rate-indexed revenue share that protects it.
2. **Formula.** `CFADS` in USD = `(HC revenue − HC costs − HC tax − HC working capital) ÷ x − USD costs`,
   where `x` is `HC` per USD. Breakeven `x` for a target ratio:
   `local numerator ÷ (target × debt service + USD costs)`. Debt-service-matching indexed share =
   `(debt service + USD operating costs) ÷ revenue`.
3. **Substitution.** Local numerator = `48,000,000 − 12,600,000 − 2,064,000 − 2,400,000 = HC 30,936,000`.
   At `x` = 4.00: `30,936,000/4.00 − 1,350,000 = 6,384,000` — the master thread reproduced, which is
   the check that the decomposition is right.
4. **Result.**

   | `HC` per USD | Devaluation | `CFADS` (USD) | `DSCR` |
   |---|---|---|---|
   | 4.000000 | — | 6,384,000 | **1.2743** |
   | 4.202369 | **+5.06 %** | 6,011,562 | **1.2000** — covenant |
   | 4.350394 | +8.76 % | 5,761,081 | 1.1500 — lock-up |
   | 4.400000 | +10.00 % | 5,680,909 | 1.1340 |
   | 4.864430 | +21.61 % | 5,009,635 | **1.0000** — cannot pay |
   | 5.000000 | +25.00 % | 4,837,200 | **0.9656** |

   The **debt-service-matching indexed share** is `6,359,635.23 / 12,000,000 =` **52.997 %**. The
   **minimum indexed share** that holds the covenant through a 25 % devaluation is **48.9318 %**. At
   the matching 52.997 % share, a 25 % devaluation leaves `CFADS` of **6,109,128** and a `DSCR` of
   **1.2195**, and the covenant survives devaluation up to **+37.17 %**.
5. **Interpretation.** The headline is stark enough to be worth stating plainly: **a 5.06 % currency
   movement breaches this project's covenant, and a 21.61 % movement stops it paying its debt.** For
   comparison, the interest-rate exposure of 11.3.1 required 73.9 basis points — a rare event in a
   quiet market — while a 5 % currency move is a routine month in many host economies. Unhedged
   currency mismatch is therefore not one risk among many; on these numbers it is the **binding**
   exposure, larger than rates, availability (Domain 7's 2.9 availability points) and input prices
   combined. **Note also what the arithmetic quietly reveals about "natural hedges".** Local costs do
   fall in USD terms as the currency weakens, which is why the exposure is 30,936,000 of local
   numerator rather than the full 48,000,000 of revenue — but the offset is far too small, because
   debt service (5,009,635.23) and USD operating costs (1,350,000) are **52.997 %** of base-case revenue and
   do not devalue at all. A project whose *costs* are mostly local and whose *debt* is entirely foreign
   has a natural hedge on the wrong side of the balance sheet. **The remedy is indexation, and it has a
   number.** Indexing 52.997 % of the tariff to the exchange rate — exactly the share that funds USD
   outflows — is the structurally clean ask, and it lifts the tolerable devaluation from 5.06 % to
   37.17 %. The covenant-preserving minimum against a specified 25 % stress is 48.93 %, only 4.07
   points lower, which is a useful negotiating fact: the clean structural ask is barely more expensive
   than the minimum defensible one, so there is little to be gained by conceding to a partial share.
   **And two limits must be stated honestly.** Indexation protects **debt service, not equity return**:
   full `DSCR` neutrality is impossible while any local surplus exists, because that surplus keeps
   shrinking in USD terms — at the 52.997 % share a 25 % devaluation still costs 274,872 of `CFADS`.
   And indexation is a **transfer to the offtaker**, so the pricing test of 11.1.3 applies: the
   offtaker does not control the exchange rate, so the transfer must be justified on **capacity**
   grounds — a public or utility payer with local-currency revenue and sovereign-adjacent standing bears
   a devaluation that would destroy the SPV — and it will be paid for in the tariff. That is a
   legitimate, well-grounded transfer; the illegitimate version is a sponsor accepting an unindexed
   `HC` tariff because the offtaker insisted, which is 11.1.2's bargaining-power allocation with a
   five-per-cent trigger attached.

### 11.3.3 The counterparty risk inside the allocation

**The principle.** Every risk successfully transferred becomes a **credit exposure** to the party that
took it. The allocation table of 11.1.3 removed 4,420,000 of expected cost from Kestrel's balance
sheet and replaced it with a claim on the EPC contractor — and that claim is worth the contractor's
willingness and ability to pay.

**The arithmetic that matters is conditional, not unconditional.** Take the transferred bundle at
4,420,000 of avoided expected cost, an assumed recovery of 30 % on the contractor's obligations, and
a two-year probability of default of 1.5 % for a contractor of the assumed standing. Expected credit
loss on the transfer is `0.015 × 0.70 × 4,420,000 =` **46,410** — 12.46 % of annual covenant headroom,
and easy to dismiss. But the unconditional probability is the wrong one. **The contractor most likely
to default is the one that has already incurred the losses it was allocated**, so the relevant figure
is the probability of default *conditional on a claim of the size the allocation contemplates*. At a
conditional probability of 12 %, expected credit loss becomes `0.12 × 0.70 × 4,420,000 =`
**371,280** — **99.69 %** of the entire annual covenant headroom of 372,437.72.

What matters is not the coincidence of the two magnitudes but the **eightfold difference between the
two calculations**, and the fact that the conditional one is almost never done. Risk transfer converts
a diversifiable operational exposure into a concentrated, correlated credit exposure, and the
correlation is structural: the state of the world in which the guarantee is needed is the state in
which the guarantor is weakest. Three consequences follow. **Test the guarantor, not the guarantee** —
Domain 5's distinction between joint and several liability, and the standing of the entity behind a
commitment, decides what a cap is worth. **Require the credit support to sit above the operating
entity**, so that a trading counterparty's promise becomes a claim on a balance sheet or a bank. And
**count the counterparties**: after allocation Kestrel's coverage depends on the EPC contractor, the
operator, the offtaker (Domain 7 priced its credit at 0.0400 of `DSCR`, 53.7 % of its total
expected-loss exposure), the hedge counterparty of 11.3.1, the insurers of 11.3.4 and the sponsors
behind Domain 5's 24,000,000 of committed capital. Six credit dependencies are six ways to lose the
protection the project paid for, and a **counterparty concentration schedule** — exposure, mechanism,
cap, credit standing, correlation of the call with that standing — makes them visible (11.T.2).

### 11.3.4 Political, regulatory and force-majeure risk

**Political and regulatory risk** covers expropriation, currency inconvertibility and transfer
restriction, breach of undertaking by a state counterparty, and adverse change in law, tax or
regulated tariff. Its allocation is unusual in that the natural transferee is often not a commercial
party at all: political risk insurance and the guarantee products of export credit agencies and
multilateral development institutions exist precisely because private parties cannot bear these
exposures (Domain 9 covers the instruments; the allocation view here is that they are **capacity
transfers**, and the pricing test applies unchanged). Change-in-law risk is the one commonly split
rather than allocated: discriminatory change (aimed at the project or its sector) to the state,
general change to the project, is a widely used convention — but conventions are not law, and the line
between the two categories is where the disputes live.

**Force majeure** is the allocation of last resort: it allocates a risk to **nobody**, suspending
obligations rather than shifting cost. That structure is right for genuinely uncontrollable,
uninsurable events, and it has a specific financial consequence that projects consistently
under-model: relief from performance obligations does not relieve **debt service**. The lenders'
schedule continues, so a force-majeure event that suspends the operator's availability obligation
lands squarely on coverage.

**Worked example 11.3.4 — the insurance waiting period nobody checks against the covenant.**

1. **Setup.** Kestrel carries business-interruption cover with a **60-day waiting period** — the
   insurer indemnifies lost margin only for the portion of an outage beyond 60 days. Daily `CFADS` is
   `6,384,000/360 =` **17,733.33** (Domain 5's 30/360 basis). Covenant headroom is 372,437.72.
   Determine the maximum outage the covenant survives, and the effect of outages of 21, 30 and 60 days.
2. **Formula.** Uninsured days = `min(outage days, waiting period)`; lost `CFADS` = uninsured days ×
   daily `CFADS`; maximum survivable days = headroom ÷ daily `CFADS`.
3. **Substitution.** `372,437.72 / 17,733.33`; then `CFADS − days × 17,733.33`, divided by 5,009,635.23.
4. **Result.**

   | Outage | Uninsured days | Lost `CFADS` | `CFADS` | `DSCR` |
   |---|---|---|---|---|
   | 21 days | 21 | 372,400 | 6,011,600 | **1.2000** — exactly at covenant |
   | 30 days | 30 | 532,000 | 5,852,000 | **1.1681** — breach |
   | 60 days | 60 | 1,064,000 | 5,320,000 | **1.0620** — breach, below lock-up |
   | 90 days | 60 (capped by the waiting period) | 1,064,000 | 5,320,000 | 1.0620 |

   The maximum survivable outage is **21.0021 days**, so the maximum tolerable waiting period is
   **21 days**.
5. **Interpretation.** The insurance programme and the finance documents were negotiated by different
   teams against different units, and the result is a structural gap nobody owns: **the waiting period
   is 60 days and the covenant survives 21.** Any outage longer than three weeks breaches, and the
   insurance — which is doing its job perfectly — pays nothing until the covenant has already failed by
   a wide margin. The insight generalises beyond insurance: **deductibles, waiting periods, cure
   periods, grace periods, notice periods and reporting lags are all calibrated in time, while
   covenants are calibrated in cash, and the translation between the two is almost never performed.**
   Three responses are available and should be priced against one another: buy down the waiting period
   towards 21 days (the cheapest fix if the market offers it); size the debt-service reserve to bridge
   the gap — Domain 10's six-month DSRA of 2,504,818 comfortably funds the 1,064,000 maximum uninsured
   loss, so payment continuity is not at issue and only *compliance* is; or negotiate a covenant
   carve-out for insured events within the waiting period, which is the cleanest answer and the one
   most often overlooked because it must be asked for before close. Note finally the shape of the
   exposure: it is **capped** at 1,064,000 by the waiting period itself, so this is a high-probability,
   bounded exposure — the opposite profile from the currency mismatch of 11.3.2 — and it should be
   managed with a reserve or a carve-out rather than with capital.

### AI in this KA

**Where it earns its place:** the translation layer. Every number in this KA is a mechanical
consequence of terms written in documents — a margin, a waiting period, a hedging covenant, an
indexation formula — and the failure mode is that the translation is done once at close and never
again. A monitored model that recomputes, every month, the reference rate at which the covenant fails,
the exchange rate at which it fails, the outage length at which it fails and the hedge ratio in force
against the ratio covenanted is high-value automation with a low error surface, because each output is
a closed-form calculation against a stated document term. Extraction of hedging covenants, waiting
periods and caps from a document suite is the same strong application named in 11.1.
**Where it must not go:** rate, currency and event forecasting. A model asked what the reference rate
or the exchange rate will be in year seven will answer, and the answer will carry no information; the
governed use is to compute the **breakeven** — the level at which something breaks — because a
breakeven is a fact about the structure rather than a prediction about the world. Equally, the
question of whether a set of facts constitutes force majeure, or a discriminatory change in law, is a
legal determination for qualified counsel and not a model output. **Verification, concretely:** require
each monitored breakeven to be reproducible by hand from the closing model on one period — the check
values are 6.7390 %, `HC` 4.202369 and 21 days — and re-verify after any amendment, because the
recurring failure is a monitor that keeps computing correctly against superseded terms.

### Key terms — KA 11.3

| Term | Meaning |
|---|---|
| **Hedge ratio** | Share of floating debt fixed by swap or cap; lenders covenant a minimum rather than a full hedge. |
| **Breakeven rate / breakeven exchange rate** | The level at which a named covenant fails; a fact about the structure, not a forecast. |
| **Mark-to-market (swap)** | The break cost of terminating a hedge early — an exposure a swap creates rather than removes. |
| **Currency mismatch** | Revenue and debt service in different currencies; unbounded and correlated with host-economy stress. |
| **Exchange-rate-indexed share** | The portion of tariff linked to the exchange rate; the debt-service-matching share funds USD outflows. |
| **Conditional probability of default** | Default probability given a claim of the size the allocation contemplates — the relevant figure for transferred risk. |
| **Waiting period / deductible** | Time or amount an insured bears before indemnity; must be tested against covenant headroom in days. |
| **Force majeure** | Allocation to nobody: obligations suspend, debt service does not. |

### Sample MCQs — KA 11.3

**MCQ 11.3-A `[11.3.1 · Application]`** Debt 42,000,000; fixed year-one principal 2,489,635.23;
`CFADS` 6,384,000; covenant 1.20×. The all-in interest rate at which the covenant fails is closest to:
- A. 6.00 %
- B. 6.74 % ✅
- C. 8.00 %
- D. 9.27 %

*Rationale:* Maximum debt service = 6,384,000/1.20 = 5,320,000; maximum interest = 2,830,364.77;
÷ 42,000,000 = 6.7390 %. A is the rate at close; C is a +200 bp shock, at which the ratio is already
1.0914; D is the rate at which debt service cannot be paid at all — a different and much later
threshold.

**MCQ 11.3-B `[11.3.1 · Analysis]`** A swap moves year-one `DSCR` from 1.2743 to 1.2533 and replaces a
1.0914–1.5311 range with a single value. The correct characterisation is:
- A. the swap is uneconomic, since coverage falls
- B. 0.0210 of coverage is given up to remove 0.4397 of coverage range — 20.92 units of range per unit surrendered ✅
- C. the swap eliminates all interest-rate exposure and creates no new exposure
- D. the swap is unnecessary because the project can pay debt service up to 9.27 %

*Rationale:* The trade is certainty for a small, certain cost (11.3.1). A counts the cost and not the
benefit; C ignores mark-to-market and hedge-counterparty exposure; D confuses the payment threshold
with the covenant threshold.

**MCQ 11.3-C `[11.3.2 · Application]`** Local numerator `HC` 30,936,000, USD operating costs
1,350,000, debt service 5,009,635.23, rate at close `HC` 4.00 = USD 1, covenant 1.20×. The
devaluation at which the covenant fails is closest to:
- A. 5.1 % ✅
- B. 15.9 %
- C. 21.6 %
- D. 25.0 %

*Rationale:* Covenant `CFADS` = 5,009,635.23 × 1.20 = 6,011,562.28; `x` = 30,936,000 ÷
(6,011,562.28 + 1,350,000) = 4.202369, i.e. +5.06 %. B divides `CFADS` by the covenant instead of
multiplying debt service by it — the commonest covenant-trigger error; C is the point at which the
`DSCR` reaches 1.00; D is the illustrative stress case, at which the ratio is already 0.9656.

**MCQ 11.3-D `[11.3.4 · Analysis]`** Business-interruption cover has a 60-day waiting period; daily
`CFADS` is 17,733.33 and covenant headroom is 372,437.72. The most useful statement for the finance
committee is:
- A. the cover is adequate, since the maximum uninsured loss of 1,064,000 is within the DSRA
- B. the covenant survives 21 days of outage while the waiting period is 60 — so any outage beyond three weeks breaches, and a carve-out or a bought-down waiting period is required ✅
- C. the waiting period should be extended to reduce premium
- D. force majeure relief will suspend debt service during the outage

*Rationale:* The gap between a time-calibrated insurance term and a cash-calibrated covenant is the
finding (11.3.4). A is true about *payment* and silent about *compliance* — the distinction of Domain
10, KA 10.2.1; C widens the gap; D is false, since force majeure suspends performance obligations and
not debt service.

**MCQ 11.3-E `[11.3.2 · Evaluation]`** The offtaker will accept a host-currency tariff with 40 % of it
indexed to the exchange rate. The debt-service-matching share is 52.997 % and the covenant-preserving
minimum against a 25 % devaluation is 48.9318 %. The bid team argues that partial indexation is better
than none. The soundest position is:
- A. accept 40 %: partial protection is better than none, and the offtaker has moved once already
- B. hold for a share at or above 48.9318 %: at 40 % the covenant fails on a **14.54 %** devaluation
  and a 25 % devaluation leaves a `DSCR` of **1.1572**, while the clean structural ask of 52.997 %
  sits only 4.07 points above the minimum defensible one ✅
- C. refuse any host-currency tariff, since a twelve-year currency mismatch cannot be managed
- D. accept 40 % and hedge the residual exposure in the swap market

*Rationale:* The tolerable devaluation runs 5.06 % unindexed, 14.54 % at a 40 % share and 37.17 % at
the matching share, and because the matching share is barely more expensive than the minimum there is
little to be gained by conceding to a partial one (11.3.2). A treats any movement as progress without
testing it against the covenant. C forgoes a transfer that is well grounded on **capacity** — a payer
with local-currency revenue and sovereign-adjacent standing bears a devaluation that would destroy the
SPV — and it will be paid for in the tariff. D assumes a market that does not exist for a twelve-year
tenor in most host economies at any price a project can pay.

**MCQ 11.3-F `[11.3.1 · Comprehension]`** Saying "the covenant fails at an all-in rate of 6.7390 %" is
a different kind of statement from forecasting the reference rate because a breakeven:
- A. is a more accurate forecast, being derived from the schedule rather than from the market
- B. is a fact about the structure — the level at which a named test fails, given the schedule, the
  `CFADS` definition and the covenant — while a forecast is a claim about the world ✅
- C. rests on no assumptions at all
- D. is the same statement expressed in different units

*Rationale:* This is why the governed use of a model here is to compute breakevens rather than to
predict rates: a breakeven can be monitored against a document, whereas a prediction can only be owned
(11.3.1). A collapses the two categories into one. C overstates — the 6.7390 % still depends on the
`CFADS` figure and the fixed-principal schedule it is computed from, which is why it must be
recomputed after any amendment. D ignores that one statement is conditional and the other predictive.

**MCQ 11.3-G `[11.3.1 · Evaluation]`** Treasury proposes to leave Kestrel's floating facility unhedged,
on the ground that scheduled debt service can be paid up to an all-in rate of **9.2723 %** and the
reference rate stands at 4.00 %. A full hedge fixes coverage at **1.2533** at a year-one cash cost of
**84,000**; a 75 % hedge holds **1.2085** at +200 basis points for **63,000**; and the minimum hedge
ratio surviving that shock at the covenant is **70.0576 %**. The recommendation should be:
- A. leave it unhedged: **327 basis points** of reference-rate headroom to payment failure is ample, and
  the 84,000 is a certain cost against a contingent exposure
- B. hedge at not less than 70.06 %, and in practice at 75 %: the exposure that binds is the covenant at
  **+73.9 basis points**, not payment at +327.2, and 0.0210 of coverage buys the removal of 0.4397 of
  coverage range — 20.92 units of range per unit surrendered ✅
- C. hedge fully: a single covenanted coverage figure at any reference rate is the only defensible
  position for a project financing
- D. leave it unhedged and rely on the debt service reserve, which covers a rate shock as readily as a
  cash shortfall

*Rationale:* Interest-rate exposure is a covenant exposure long before it is a payment exposure, and
treasury has answered the question four and a half times too generously (11.3.1). A is the
under-hedging error in its usual form — the certain 84,000 is visible and the contingent range is not.
C is the defensible weaker course and a common covenanted outcome: it does remove the whole range, and
it pays 84,000 rather than 63,000, forgoes every benefit of falling rates, and enlarges the
mark-to-market break cost that a later refinancing must pay. D misreads what a reserve does: it buys
payment continuity and time, not compliance, and the breach at +73.9 basis points happens with the
reserve fully funded. Two disciplines belong with B — the hedge profile should amortise with the
outstanding balance rather than sit flat, and the hedge counterparty's own credit is now inside the
structure.

### Self-check — KA 11.3

1. *State Kestrel's three unhedged interest-rate breakevens.* — Covenant at 6.7390 % (+73.9 bp),
   lock-up at 7.2897 % (+129.0 bp), payment failure at 9.2723 % (+327.2 bp).
2. *Why is the unconditional probability of default the wrong input for transferred risk?* — The
   counterparty most likely to default is the one that has already incurred the allocated loss;
   Kestrel's expected credit loss moves from 46,410 to 371,280 on that correction.
3. *State the maximum tolerable insurance waiting period for Kestrel and why.* — 21 days: covenant
   headroom of 372,437.72 divided by daily `CFADS` of 17,733.33.

---

## Knowledge Area 11.4 — Environmental and social, technology, cybersecurity and AI model risk

*Topics: 11.4.1 the operating-phase register · 11.4.2 why a lender re-cuts it · 11.4.3 AI model risk
as a register line · 11.4.4 from register to debt capacity.*

### 11.4.1 The operating-phase register

The construction register of KA 11.1 has a counterparty and therefore a price. The operating register
usually has neither: there is no single party to whom environmental compliance, technology
underperformance, a cyber intrusion or a model error can be transferred, so these risks are managed,
insured in part, reserved against, and otherwise **retained**. That makes their quantification more
important, not less, because retained risk is the only kind that shows up directly in coverage.

**Definitions worth fixing.** **Environmental and social risk** is the risk of loss from
non-compliance with environmental consents, from social-licence failure, or from the conditions
attached to lender environmental and social standards — which are contractual obligations in a
project financing, not aspirations, and whose breach is an event of default in many facilities.
**Technology risk** is the risk that plant performs below its warranted characteristics over life
rather than at completion: degradation faster than warranted, consumables consumed faster than
modelled, obsolescence of a control system. **Cybersecurity risk**, in an industrial project, is
primarily an **availability** risk on the operational-technology network rather than a data risk, and
it is therefore a `CFADS` risk with the same shape as any other outage (11.3.4's arithmetic applies).
**Model risk** is, on the shared registry's definition, the risk of loss from decisions based on
flawed, misused or misunderstood models — financial or AI.

**Worked example 11.4.1 — Kestrel's operating register, aggregated on the Domain 8 method.**

1. **Setup.** Six retained operating-phase items, each quantified once over the loan life. The
   aggregation machinery is PML-AI Domain 8's (KA 8.2.2 and 8.2.4) and is not re-derived:
   `EMV` = `p` × impact; for independent items mean = Σ`EMV` and variance = Σ`p(1 − p) × impact²`;
   a P80 ≈ mean + 0.8416σ.
2. **Formula.** As above; then the same aggregation with a uniform pairwise correlation `ρ`, for which
   variance = `(1 − ρ)Σσᵢ² + ρ(Σσᵢ)²`.
3. **Substitution and result.**

   | ID | Retained operating risk | `p` | Impact (USD) | `EMV` (USD) |
   |---|---|---|---|---|
   | O1 | Discharge-consent breach requiring plant modification | 0.15 | 3,200,000 | 480,000 |
   | O2 | Community objection interrupting intake maintenance access | 0.10 | 1,500,000 | 150,000 |
   | O3 | Membrane flux decline faster than warranted (technology) | 0.20 | 2,600,000 | 520,000 |
   | O4 | Cyber intrusion on the control network: outage plus remediation | 0.12 | 4,500,000 | 540,000 |
   | O5 | AI dosing-optimisation model error producing off-spec output | 0.25 | 900,000 | 225,000 |
   | O6 | Asset-model drift causing deferred-maintenance catch-up | 0.30 | 700,000 | 210,000 |
   | | **Mean exposure** | | | **2,125,000** |

   Variance (independent) **4,982,875,000,000**; σ **2,232,235**; **P80 4,003,649**. At `ρ` = 0.30,
   σ rises to **3,227,338** and the **P80 to 4,841,128** — an uplift of **837,478**, or **20.92 %**.
4. **Result.** The register's mean is **USD 2,125,000** and its P80 **USD 4,003,649** on an
   independence assumption that raises it to **USD 4,841,128** once a modest correlation is admitted.
5. **Interpretation.** Two observations do most of the work. **The modern risk classes dominate the
   register.** Cyber (540,000) and the two model-risk lines (435,000 combined) contribute 975,000 of
   the 2,125,000 mean — 45.9 % — and the cyber item, with the largest single impact, contributes the
   largest share of the variance and therefore of the tail. A register written five years earlier
   would have carried none of these lines, and a project whose register still does not is not less
   exposed, merely less informed. **The independence assumption is expensive.** The 837,478 uplift at
   `ρ` = 0.30 is more than twice Kestrel's entire annual covenant headroom, and correlation here is not
   hypothetical: O1 and O2 are both consequences of the same community and regulatory environment, and
   O5 and O6 are both consequences of the same data pipeline and the same modelling culture. Where
   items share a cause, independence is a choice to under-provide, and the choice should be stated in
   the paper rather than embedded in the arithmetic. PML-AI Domain 8's two standing cautions carry over
   unchanged: the normal approximation is a convenience for a handful of Bernoulli items, and a
   simulation over the register is the proper instrument where the answer is decision-critical.

### 11.4.2 Why a lender re-cuts the register

A lender does not accept a sponsor's register, and the reason is structural rather than adversarial:
**the two parties have different loss functions.** Equity holds the upside and the downside, so its
rational provision is near the middle of the distribution. A lender holds a fixed claim: it captures
none of the upside, so every dollar of distribution above its expectation is irrelevant to it while
every dollar of shortfall is loss. A rational lender therefore prices the **tail**, not the mean, and
it does so by re-cutting the register's inputs rather than by arguing about the output.

**Worked example 11.4.2 — the same register, on the lender's basis.**

1. **Setup.** The lender's technical and E&S advisers re-cut the register of 11.4.1: probabilities at
   **1.5×** the sponsor's (the upper end of the plausible range rather than the central estimate),
   impacts at **1.4×** (a P80 within each item's own impact range rather than its mode), and a uniform
   pairwise correlation of **0.30** in place of independence. These multipliers are illustrative of the
   *direction and order of magnitude* advisers apply; the actual re-cut is item-specific and evidenced.
2. **Formula.** As 11.4.1, on the re-cut inputs.
3. **Result.** Lender `EMV`s: O1 1,008,000 · O2 315,000 · O3 1,092,000 · O4 1,134,000 · O5 472,500 ·
   O6 441,000. **Lender mean 4,462,500**; σ **5,253,726**; **lender P80 8,884,036**.
4. **Interpretation.** The single most useful comparison in this KA: **the lender's *expected* case
   (4,462,500) exceeds the sponsor's *80th percentile* (4,003,649) by 458,851 — a ratio of 1.1146 — and
   the lender's P80 is 4.1807 times the sponsor's mean.** A sponsor who arrives at a credit committee
   expecting to negotiate a percentile has misread the disagreement: the parties are not arguing about
   confidence levels, they are working from different input sets, and the only productive negotiation is
   **item by item on the evidence** — this probability, on this ground investigation; this impact, on
   this remediation quotation. Two further consequences deserve stating. **The gap is a financeable
   quantity, not a debating point**: it will be closed by a reserve, a sponsor commitment, a
   contingent equity undertaking or less debt, and the sponsor's choice among those is a real
   commercial decision (Domain 8's distinction between funded cash and a posted commitment applies
   directly). And **the re-cut is where AI-assisted analysis is most tempting and least appropriate**:
   a model asked to "produce the bank case" will multiply, which is precisely the intellectually empty
   version of the exercise the arithmetic above is designed to expose. Multipliers illustrate; evidence
   decides.

### 11.4.3 AI model risk as a register line

**Definition.** In a project financing, **AI model risk** is the risk of loss arising from a decision
taken, or an action executed, on the output of a machine-learning model — whether because the model
was wrong, because it was used outside the conditions it was validated for, or because its output was
misunderstood by the person who acted on it. Kestrel's register carries two such lines: **O5**, an
optimisation model that sets chemical dosing and can drive output off specification, and **O6**, an
asset model whose drift defers maintenance that later has to be caught up.

Three properties distinguish AI model risk from the other register lines. **The impact distribution is
fat-tailed relative to the probability**: most model errors are small and self-correcting, but a model
with authority over a physical process can produce a large excursion quickly, and a single
`p` × impact pair compresses that shape. **It is correlated with everything the model touches**, so one
model feeding dosing, energy optimisation and maintenance planning creates a common cause across three
otherwise independent register lines — a concrete reason the `ρ` = 0.30 case of 11.4.1 is the honest
one. And **its probability is a function of governance rather than of nature**: unlike a membrane's
degradation rate, `p` here is set by whether validation, monitoring, drift detection, human approval
and rollback exist. That last property makes it the one register line whose probability the project can
genuinely change — which by 11.1.2's logic makes the project its own best holder of the risk, and
controls worth more than transfer.

For allocation purposes the practical settlement is: the **vendor** takes defect and availability risk
on the software within a liability cap that will be small relative to the exposure; the **operator**
takes the risk of using the model outside its validated envelope, backed by procedure and training;
and the **project retains the consequence**, provisions for it in the register, and manages it with the
control set Domain 16 specifies. Two negotiating points are worth naming because they are so often
missed: the right to the **model's validation evidence and monitoring outputs** (without which the
project cannot demonstrate its own `p`), and the right to **operate without the model** — a documented
manual fallback, which converts a dependency into a preference and is the cheapest single mitigation
available.

### 11.4.4 From register to debt capacity

A register is not a document; it is a **debt-capacity input**. The translation is short: convert the
register's present-value exposure into an annual equivalent over the loan life using the loan-rate
annuity factor, deduct it from `CFADS`, and read the coverage.

**Worked example 11.4.4 — what Kestrel's register does to its coverage and its debt.**

1. **Setup.** `AF(0.06, 12) = 8.383844`; `CFADS` 6,384,000; debt service 5,009,635.23; covenant 1.20×.
   Test the five register measures of 11.4.1 and 11.4.2.
2. **Formula.** Annual equivalent = PV exposure ÷ `AF(0.06, 12)`; stressed `CFADS` = 6,384,000 −
   annual equivalent; `DSCR` = stressed `CFADS` ÷ 5,009,635.23. Covenant-preserving maximum PV
   exposure = headroom × `AF(0.06, 12)`.
3. **Result.**

   | Register measure | PV exposure | Annual equivalent | Stressed `CFADS` | `DSCR` |
   |---|---|---|---|---|
   | Sponsor mean | 2,125,000 | 253,464 | 6,130,536 | **1.2237** — holds |
   | Sponsor P80, independent | 4,003,649 | 477,543 | 5,906,457 | **1.1790** — breach |
   | Sponsor P80, `ρ` = 0.30 | 4,841,128 | 577,435 | 5,806,565 | **1.1591** — breach, near lock-up |
   | Lender mean | 4,462,500 | 532,274 | 5,851,726 | **1.1681** — breach |
   | Lender P80 | 8,884,036 | 1,059,661 | 5,324,339 | **1.0628** — breach |

   The **covenant-preserving maximum PV exposure** is `372,437.72 × 8.383844 =` **USD 3,122,460**. On
   the lender's P80 the sustainable debt at 1.20× falls to `(5,324,339/1.20) × 8.383844 =`
   **USD 37,198,687** — **4,801,313** less than the 42,000,000 drawn.
4. **Interpretation.** This table is the domain's closing argument. **Kestrel's covenant survives its
   own operating risk register only at the mean.** At its own P80 — the confidence level its own
   contingency policy uses for construction (Domain 8) — the covenant fails, and it fails on every one
   of the lender's measures. The number to carry away is the **3,122,460 covenant-preserving ceiling**:
   that, and not a percentage of capex or an industry norm, is how much present-value operating risk
   this structure can absorb, and it is directly comparable with the register's own 4,003,649. The gap
   of 881,189 is the honest size of the problem, and it has exactly the same four resolutions as any
   coverage shortfall (Domain 10, KA 10.1.2): more cash, lower required coverage, longer tenor, or less
   debt — with the fifth, more equity, as the residual. **The lender's arithmetic is the same
   arithmetic.** Its re-cut register removes 4,801,313 of debt capacity, which is why a diligence
   process that appears to be about risk management is in fact a negotiation about the debt quantum,
   and why a sponsor who cannot reproduce the calculation above will not understand what it is
   conceding. Two cautions bound the method. The **annuity-equivalent conversion is a simplification**:
   register events are lumpy and dated, and a proper treatment models each in the year it is assumed
   to fall, which will produce a worse minimum `DSCR` than the levelised figure shown here because
   coverage is tested in periods. And the **register and the estimate range must not be added** —
   Domain 8's demonstration on the construction side applies unchanged to operations.

### AI in this KA

**Where it earns its place:** three specific, high-value uses. Extracting environmental and social
obligations from consents and lender standards into a monitored obligation register, which is
document work at a volume humans do badly. Anomaly detection on operational-technology telemetry,
where a model that flags a deviation for a human to investigate is doing what machines are for.
And **register completeness challenge** — asking what a comparable project's register contains that
this one does not, which is where the model's breadth is a genuine advantage over an individual's
experience. **Where it must not go:** into the probability and impact columns (11.1's rule, and here
it bites harder because there is no counterparty pricing the risk to provide an external check); into
autonomous action on a physical process without a human approval step and a tested rollback; and into
producing the bank case by multiplication (11.4.2). **Verification, concretely:** treat every model in
the operating envelope as a register line with a named owner, a validation date, a monitored drift
metric and a documented manual fallback — a model without those four attributes is an unquantified
register line, which is the definition of an unmanaged risk. Recompute the register's mean by hand and
require it to equal Σ`EMV` before any percentile is quoted. And require any automated register
aggregation to state its correlation assumption explicitly on the face of its output, because
independence is the assumption tools default to and the assumption that is worth 837,478 here.
**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 11.4

| Term | Meaning |
|---|---|
| **Environmental and social risk** | Loss from consent breach, social-licence failure or breach of lender E&S standards — contractual, not aspirational. |
| **Technology risk** | Under-performance over life (degradation, consumption, obsolescence) rather than at completion. |
| **Cybersecurity risk (project)** | Primarily an availability risk on the operational-technology network; a `CFADS` risk. |
| **AI model risk** | Loss from decisions or actions taken on model output — wrong model, wrong envelope, or misunderstood output. |
| **Bank case** | The lender's re-cut of the register: higher probabilities, higher impacts, correlation admitted. |
| **Correlation uplift** | The increase in a percentile from admitting `ρ` > 0; 837,478 at `ρ` = 0.30 on Kestrel's register. |
| **Covenant-preserving exposure ceiling** | Headroom × `AF(r, n)` — the present-value risk a structure can absorb (3,122,460 for Kestrel). |

### Sample MCQs — KA 11.4

**MCQ 11.4-A `[11.4.1 · Application]`** A register of independent items has a mean of 2,125,000 and
a variance of 4,982,875,000,000. Its P80 is closest to:
- A. 2,125,000
- B. 4,003,649 ✅
- C. 4,357,235
- D. 4,841,128

*Rationale:* σ = 2,232,235; P80 = 2,125,000 + 0.8416 × 2,232,235 = 4,003,649. A is the mean, which is
exceeded roughly half the time; C applies a 1.0-σ (P84) factor instead of 0.8416; D is the P80 once a
0.30 correlation is admitted — right method, different stated assumption.

**MCQ 11.4-B `[11.4.2 · Analysis]`** A sponsor's register P80 is 4,003,649 and its lender's re-cut
*mean* is 4,462,500. The correct inference is:
- A. the lender is applying a higher confidence level than the sponsor
- B. the parties are working from different input sets, not different percentiles — the lender's expected case already exceeds the sponsor's 80th ✅
- C. the sponsor's arithmetic is wrong
- D. the difference is immaterial at 458,851

*Rationale:* The lender re-cuts probabilities, impacts and correlation; the disagreement is
evidential and must be negotiated item by item (11.4.2). A misdiagnoses the disagreement as a
percentile choice; D ignores that 458,851 exceeds the whole annual covenant headroom of 372,437.72.

**MCQ 11.4-C `[11.4.4 · Application]`** Annual covenant headroom is 372,437.72 and `AF(0.06, 12)` is
8.383844. The present-value operating-risk exposure the structure can absorb before its covenant
fails is:
- A. USD 372,438
- B. USD 3,122,460 ✅
- C. USD 4,003,649
- D. USD 44,423

*Rationale:* Headroom × `AF(0.06, 12)` = 3,122,460 — the present value of losing that much cash every
year of the loan life. A is the annual figure, not its present value; C is the register's own P80,
which is the exposure being tested against the ceiling, not the ceiling; D divides (44,423) rather than
multiplies by the annuity factor.

**MCQ 11.4-D `[11.4.3 · Analysis]`** Which property makes AI model risk different in kind from
membrane degradation risk?
- A. its impact is always larger
- B. its probability is a function of governance — validation, monitoring, approval and rollback — so the project can genuinely change it ✅
- C. it can be transferred to the software vendor in full
- D. it is uninsurable

*Rationale:* 11.4.3: `p` is set by the control set rather than by nature, which by 11.1.2's logic makes
the project the right holder and controls worth more than transfer. A is unsupported; C is false —
vendor liability caps are small relative to the exposure; D overstates a market position.

**MCQ 11.4-E `[11.4.2 · Evaluation]`** A sponsor prepares "the bank case" by multiplying its own
register probabilities by 1.5 and its impacts by 1.4 and admitting a 0.30 correlation, reproducing the
order of magnitude the lender's advisers apply. The soundest professional position is:
- A. the case is adequate, since it reproduces the multipliers the lender's advisers use
- B. multipliers illustrate direction and order of magnitude only: the re-cut must be item-specific and
  evidenced, because the disagreement is evidential and will be settled line by line — this
  probability on this ground investigation, this impact on this remediation quotation ✅
- C. present only the sponsor's own mean, since the lender will produce its own case regardless
- D. adopt the lender's P80 of 8,884,036 as the sponsor's base case, to remove the argument

*Rationale:* A bank case produced by multiplication is the intellectually empty version of the
exercise: it concedes the arithmetic without contesting a single input, and multipliers illustrate
while evidence decides (11.4.2). A mistakes agreement on a multiplier for agreement on evidence. C
arrives at a credit committee unable to reproduce the calculation that will set the debt quantum — the
lender's re-cut removes 4,801,313 of capacity. D over-concedes: adopting a tail as a central case
mis-sizes contingency, reserves and distributions, when the gap between the two cases is a financeable
quantity to be closed by a reserve, a sponsor commitment or less debt.

**MCQ 11.4-F `[11.4.4 · Evaluation]`** Kestrel's operating register has a mean of **2,125,000** and a
P80 of **4,003,649** on an independence assumption, against a covenant-preserving exposure ceiling of
**3,122,460**. The team proposes to provision the mean, on the ground that it is the expected case. The
soundest position is:
- A. agree: the mean is the unbiased estimate, and the P80 is a construction-side convention that does
  not belong in an operating provision
- B. the **881,189** gap between the register's own P80 and the ceiling is the size of the problem, and
  it is closed by one of the four coverage levers or by equity — with the correlation assumption stated
  on the face of the output, since admitting ρ = 0.30 raises the P80 by a further **837,478** ✅
- C. adopt the lender's P80 of 8,884,036 and resize the facility to 37,198,687
- D. the two figures are not comparable, since the register is a present value and the ceiling an annual
  headroom figure

*Rationale:* Both quantities are present values — the ceiling is headroom × `AF(0.06, 12)` precisely so
that the comparison can be made — so the gap is a financeable quantity rather than a choice of
percentile (11.4.4). A selects the measure that makes the problem disappear, against the project's own
contingency policy on the construction side and against a covenant that is a fixed claim rather than an
expectation. C is defensible and over-conservative: the lender's re-cut is a negotiating position to be
argued item by item on the evidence (11.4.2), and adopting it unexamined surrenders 4,801,313 of debt
capacity before the argument has been had. D is simply wrong about the units, and it is the reason the
comparison is so often not performed.

**MCQ 11.4-G `[11.4.3 · Comprehension]`** On the shared registry's definition, **model risk** is:
- A. the risk that a model contains a coding error
- B. the risk of loss from decisions or actions taken on the output of a model — because the model was
  flawed, because it was used outside the conditions it was validated for, or because its output was
  misunderstood by whoever acted on it ✅
- C. the risk that a model's forecast differs from the outcome, which is inherent in any forecast
- D. the risk that a model or its training data is compromised by an intruder

*Rationale:* The definition turns on the decision taken and covers three distinct failures — a wrong
model, a sound model used in the wrong envelope, and a correct output misread (11.4.3). A is one cause
of the first failure only. C describes forecast uncertainty, which is a property of the future rather
than a defect of the model, and treating the two as the same makes the register line unmanageable —
part of why `p` here is a function of governance rather than of nature. D is cybersecurity risk, which
this domain treats primarily as an availability exposure on the operational-technology network.

### Self-check — KA 11.4

1. *State Kestrel's register on four bases.* — Sponsor mean 2,125,000; sponsor P80 independent
   4,003,649; sponsor P80 at `ρ` = 0.30 4,841,128; lender P80 8,884,036.
2. *What is the covenant-preserving exposure ceiling and how is it computed?* — 3,122,460 = annual
   headroom 372,437.72 × `AF(0.06, 12)` 8.383844.
3. *Why is a lender's mean higher than a sponsor's P80?* — Different loss functions produce different
   input sets: a fixed claim prices the tail, so probabilities, impacts and correlation are all
   re-cut upwards.

---

## Advanced topics — Domain 11

### 11.A.1 Bargaining power, priced

The illegitimate ground for transfer (11.1.2) can be quantified, and doing so is the fastest way to
stop an organisation using it. When a party is compelled to accept a risk it neither controls nor can
absorb, one of four things happens and each has a cost the transferor eventually pays. It prices the
risk **privately**, and the premium is paid unseen — the A4 case, where 1,827,000 sits inside a lump
sum against 960,000 of retained expected cost. It **adopts a claims strategy**, converting a priced
allocation into an unpriced dispute costing legal spend, management time and programme. It
**withdraws from the bid**, reducing competitive tension — the most expensive outcome and the hardest
to attribute. Or it **accepts, fails and hands the risk back** at the conditional default
probabilities of 11.3.3, the 371,280 case. Hence one question in every negotiation: *if this
counterparty cannot influence this outcome, what is it charging us, and where is that charge?*

### 11.A.2 Correlation, and the two places it hides

Correlation enters a risk position twice, and practitioners usually model neither. **Within the
register**, admitting `ρ` = 0.30 on Kestrel's operating items raises σ from 2,232,235 to 3,227,338 and
the P80 by **837,478** — 20.92 %, and more than twice the annual covenant headroom. **Between the
register and the counterparties**, the correlation is between the call on a protection and the
condition of the party providing it: the eightfold move from 46,410 to 371,280 in 11.3.3 is this
second correlation priced. The two compound in the state of the world that matters, and the
structural implication is the one to carry: **a risk position is not the sum of its lines**, and every
mitigation that depends on a counterparty is weakest exactly when it is needed. The practical
mitigations are diversification of counterparties, credit support that sits above the trading entity,
and funded reserves in place of commitments where the correlation is highest — each of which is a real
cost, honestly incurred, rather than an assumption quietly avoided.

### 11.A.3 The reviewer's allocation eye

Invariants to test on any risk allocation and its arithmetic. Every register line names a **mechanism**
(clause, guarantee, insurance, reserve), not merely an owner. Every transferred line names the
**counterparty, the cap and the credit standing behind it**, and the sum of caps is compared with the
sum of transferred exposures. Every transfer has a **priced net value**: transferor `EMV` less loaded
premium, with the breakeven loading stated; any transfer of a risk the transferee cannot control or
cheaply bear is flagged. The register's mean equals **Σ`EMV`**, and its correlation assumption is
stated on the face of the output. Register and estimate-range provisions are **not added** (Domain 8).
Every exposure is expressed in the **covenant's units**: the `CFADS` it consumes, and the basis
points, percentage, days or exchange-rate move that consumes the headroom. Time-calibrated terms
(waiting periods, deductibles, grace and cure periods) are **tested against headroom in days**. The
hedge ratio in force is compared with the ratio covenanted, and the hedge profile amortises with the
loan. Interest-rate, currency and input-price tolerances are reported as **non-additive** unless a
joint matrix is provided. The `PLCR`/tail is not used to justify retaining an exposure the lenders
have no claim on. And the register is reconciled to the **debt quantum**: the covenant-preserving
exposure ceiling (headroom × `AF(r, n)`) is stated alongside the register's own percentile, because a
register that exceeds the ceiling has already re-sized the debt whether or not anyone has said so.

---

## Industry variations — Domain 11

- **Water and desalination (Kestrel's sector).** Allocation turns on the availability/force-majeure
  boundary and on input-cost pass-through, because power is the dominant operating cost; ground and
  marine-interface risks are typically retained by the owner, which commissions the investigations,
  and E&S risk concentrates on discharge consents.
- **Contracted power and availability PPPs.** The cleanest allocation set in the market: volume to the
  payer, availability to the operator, fuel or input price passed through by formula. The residual
  risks are therefore financial — rate, currency, refinancing — which is why hedging covenants are
  tightest in this sector.
- **Merchant power and commodities.** Market risk cannot be allocated at all, only hedged for a
  fraction of the tenor. Lenders respond with higher required coverage and stressed price decks rather
  than with allocation, and the register's price lines dominate its mean.
- **Transport concessions.** Patronage risk is the defining exposure and is genuinely uncontrollable
  by any party, which is why minimum revenue guarantees, availability structures and traffic-band
  mechanisms exist; ramp-up correlation makes the independence assumption of 11.4.1 least defensible
  here.
- **Digital infrastructure.** Technology obsolescence and cybersecurity move from the tail to the
  centre of the register, tenant credit substitutes for offtaker credit, and power price and
  availability become the dominant operating exposures; short asset lives compress the tail that would
  otherwise absorb a late-life register.
- **Mining and heavy industry.** Environmental and social risk carries the largest single impacts —
  closure, rehabilitation, community and permitting — and is least transferable, so the allocation
  question becomes one of funded provision, bonding and completion support rather than of contractual
  ownership.

---

## Case study — Domain 11: the wrap that could not be afforded (water / desalination)

**Situation.** Two weeks before Kestrel's EPC award, the preferred bidder offered to convert its
partial wrap into a **full wrap**, taking the five items the pricing exercise of 11.1.3 had left with
the owner — ground conditions beyond the disclosed baseline, utility diversion and third-party
interfaces, membrane price above the contract index, marine weather standby, and permit-driven
monitoring works. The price was an additional **USD 4,620,000** on the EPC contract, taking it from
48,000,000 to **52,620,000**. Two of the three sponsors were in favour: the wrap removed 2,840,000 of
retained expected cost and, in the words of the meeting, "made the project a single-point-of-
responsibility financing".

**What happened.** The finance director ran the arithmetic of 11.1.3 and then carried it one step
further, into coverage. On expectations the transfer destroyed **1,780,000** of value, and it destroyed
**460,000** even if the bidder's 40 % loading were negotiated to zero, because the bidder's own
expected cost on the bundle (3,300,000) exceeded the owner's (2,840,000) — it had priced a
geotechnical baseline it had not set, a utility corridor it did not control and a permit condition it
could not influence. The breakeven loading was **−13.94 %**. But the decisive number was the
financing consequence. Capital cost rises to **64,620,000**. Held at 70/30 gearing, senior debt
becomes **45,234,000**, the annual instalment `45,234,000/8.383844 =` **5,395,377.11**, and the
year-one `DSCR` falls from 1.2743 to **1.1832** — below the facility's own 1.20× covenant at
financial close and only **0.0332** above its 1.15× lock-up threshold. (The figure is
generous: it ignores the additional interest during construction the larger facility would accrue.)
Funded with equity instead, the project is financeable but equity rises from 18,000,000 to
**22,620,000**, gearing moves from 70.0/30.0 to **65.0/35.0** (`D/E` **1.8568**), and the same
distributable cash stream is spread over **25.67 %** more equity.

**How it resolved.** The wrap was declined and the partial allocation of 11.1.3 was executed: A1–A3
transferred for **2,128,000** of priced risk premium inside the 48,000,000 price, A4–A8 retained,
leaving the **2,690,000** register that Domain 8 provisioned against. Two of the five retained items
were then attacked on their own terms rather than by transfer: the owner extended the geotechnical
investigation before award (reducing its own A4 probability and, more importantly, narrowing the
baseline the bidder had been pricing blind), and the utility diversion scope was fixed by a
tripartite interface agreement with the utility before contract signature — an allocation to the party
that actually controlled the corridor, which is 11.1.2's control ground applied to the right
counterparty rather than the convenient one.

**What the domain teaches here.** A wrap is not a good or a bad thing; it is a price, and the price is
comparable with a number the project already knows. Kestrel's full wrap cost **1,780,000** of expected
value and **0.0911 of `DSCR`** — more coverage than its entire 372,437.72 of annual headroom
represents — and the sponsors who favoured it were not being imprudent, they were reasoning about
responsibility rather than about arithmetic. **Single-point responsibility is worth paying for only
where the single point can actually influence the outcome.** And the resolution shows the third option
that allocation debates routinely omit: where transfer destroys value, the answer is often not to
retain the risk passively but to **reduce it at source**, or to allocate it to a party outside the EPC
relationship who genuinely controls it.

## Case study B — Domain 11: the currency the model held constant (transport / aviation)

**Situation.** Larkspur Regional Airport Concession Company (a fictitious concessionaire) financed a
terminal expansion with **USD 180,000,000** of senior debt at **6.5 % over 14 years** —
`AF(0.065, 14) = 9.013842`, annual instalment **USD 19,969,286.50** — against a 20-year concession.
Revenue of **USD 60,000,000** equivalent came entirely from regulated charges denominated in the host
currency. Cash operating costs of **24,200,000** split **17,600,000 local** and
**6,600,000 USD-linked** (imported spares, systems maintenance and expatriate technical services);
cash tax of 4,000,000 was local. Base-case `CFADS` was **31,800,000**, a `DSCR` of **1.5924** against a
**1.30× covenant** and a 1.20× lock-up — a structure everyone involved described as comfortable. The
financial model held the exchange rate constant across all fourteen years, and the sensitivity pack
contained traffic, tariff, opex and interest-rate cases but no currency case.

**What happened.** In the concession's fourth operating year the host currency depreciated such that
the cost of a dollar rose **30 %**. Traffic was on forecast; the airport was operating exactly as
planned. `CFADS` in USD fell to **22,938,462** — revenue down to 46,153,846, partly offset by local
operating costs falling to 13,538,462 and tax to 3,076,923 — and the `DSCR` fell to **1.1487**, breaching both
the covenant and the lock-up while debt service continued to be paid in full. The retrospective
arithmetic was unforgiving: the covenant had a breakeven at a devaluation of only **17.94 %**, and
payment failure at **44.53 %**. Neither number had ever been computed. The natural hedge everyone had
relied on — "our costs are local too" — was worth exactly the 21,600,000 of local costs and tax
against 60,000,000 of local revenue, leaving a local numerator of **38,400,000** exposed against
6,600,000 of USD costs and 19,969,286.50 of USD debt service that did not move at all.

**How it resolved.** The lenders waived the breach for four quarters against a fee, a distribution
lock-up that was in any case automatic, and a binding obligation on the concessionaire to seek an
exchange-rate indexation amendment from the concession grantor. The amendment obtained indexed
**40 %** of regulated charges to the exchange rate. At that share, a 30 % devaluation leaves `CFADS`
of **28,476,923** and a `DSCR` of **1.4260**; a 60 % devaluation — the lenders' stress case — leaves
**26,400,000** and **1.3220**, still above the covenant; and the covenant breakeven moves from a
17.94 % devaluation to **68.22 %**, a **3.804-fold** improvement in tolerance. The 40 % share was
chosen, deliberately, above the **38.04 %** minimum that survives the 60 % stress, so that the
structure was not calibrated to the last basis point of its own stress case. The grantor accepted the
indexation because the analysis demonstrated the alternative: a concessionaire in default on a
strategic asset, at a moment when the state's own capacity to intervene was weakest.

**What the domain teaches here.** Three things. **A model that holds a variable constant has made a
forecast, not an assumption** — and the variable held constant in this financing was the largest single
exposure in it. **A natural hedge must be measured, not asserted:** Larkspur's was real but covered
only 36 % of its local revenue, and the untouched USD debt service was 33.3 % of base-case revenue.
And **the remedy is a number, obtainable before it is needed.** The 40 % indexed share, the 38.04 %
minimum, the 17.94 % and 68.22 % breakevens are all computable at close from the closing model in an
afternoon; obtained at close they cost a tariff negotiation, and obtained in year four they cost a
waiver fee, a locked-up distribution and a year of reporting under breach.

---

## Executive perspective — Domain 11

What a project finance director cannot delegate in this domain:

- **The price of every allocation.** Not who owns each risk but what the ownership cost: transferor
  `EMV` less loaded premium, with the breakeven loading stated. A transfer nobody has priced is a
  transfer nobody has evaluated (11.1.3, Case study A).
- **The refusal to use bargaining power as a ground for transfer.** Where a counterparty can neither
  control nor cheaply bear a risk, the director's job is to say so in the room, because the charge for
  that transfer is real and will be paid somewhere less visible (11.A.1).
- **The four breakevens.** The reference rate (6.7390 %), the exchange rate (`HC` 4.202369, +5.06 %), the
  outage length (21 days) and the input-price rise (68.9699 %) at which the first covenant fails.
  These are facts about the structure, computable at close, and they belong on one page beside Domain
  10's 6,011,562.28 cash trigger.
- **The hedging and indexation policy.** The hedge ratio, its amortisation profile, the pass-through
  shares and the exchange-rate-indexed share are coverage terms, not treasury detail, and the director
  owns the decision that trades 0.0210 of ratio for 0.4397 of range.
- **The counterparty concentration after allocation.** Six credit dependencies are six ways to lose
  the protection the project paid for, and the relevant default probability is the conditional one
  (11.3.3).
- **The register's correlation and confidence assumptions, stated in the open.** Independence is worth
  837,478 of understatement on Kestrel's register, and the covenant-preserving ceiling of 3,122,460 is
  the number against which any register must be read (11.4.4).

## Calculation exercises — Domain 11

**Exercise 11.1** Three risks. X1: owner `p` 0.25, impact 4,000,000; transferee `p` 0.10, impact
3,000,000. X2: owner `p` 0.40, impact 1,500,000; transferee `p` 0.40, impact 1,600,000. X3: owner
`p` 0.30, impact 2,000,000; transferee `p` 0.15, impact 1,800,000. Loading 35 %. Compute each net
value of transfer, each breakeven loading, and the bundle decision.
*Solution.* X1: own `EMV` **1,000,000**, transferee `EMV` 300,000, premium 405,000, net **+595,000**,
breakeven loading **233.33 %**. X2: own 600,000, transferee 640,000, premium 864,000, net
**(264,000)**, breakeven loading **−6.25 %** — no price works. X3: own 600,000, transferee 270,000,
premium 364,500, net **+235,500**, breakeven loading **122.22 %**. Transfer X1 and X3 for 769,500 of
premium against 1,600,000 of retained expected cost (net **+830,500**); retain X2. *Common error:*
applying the loading to the transferor's `EMV` rather than the transferee's, which inflates every
premium and rejects transfers that create value.

**Exercise 11.2** Debt 120,000,000; fixed period principal 6,000,000; `CFADS` 17,500,000; covenant
1.25×. The facility floats at an all-in 5.50 %; a swap fixes the all-in rate at 5.75 %. Compute the
`DSCR` at 5.50 % and at a 150 basis point shock, the breakeven all-in rate, and the minimum hedge
ratio that survives the shock.
*Solution.* At 5.50 %: interest 6,600,000, debt service 12,600,000, `DSCR` **1.3889**. At 7.00 %:
interest 8,400,000, debt service 14,400,000, `DSCR` **1.2153** — breach. Breakeven: maximum debt
service `17,500,000/1.25 =` 14,000,000; maximum interest 8,000,000; ÷ 120,000,000 = **6.6667 %**
(+116.7 bp). Minimum hedge ratio `(0.07 − 0.066667)/(0.07 − 0.0575) =` **26.6667 %**. Fully swapped:
debt service 12,900,000, `DSCR` **1.3566**, at a cost of **300,000** a period. *Common error:*
computing the breakeven on total debt service rather than netting the fixed principal first, which
understates the tolerable rate and over-buys the hedge.

**Exercise 11.3** A project earns 30,000,000 of revenue, 75 % in local currency and 25 % in USD; its
operating costs are 11,000,000, 60 % local; debt service is 12,400,000 in USD; the covenant is 1.30×.
Compute base `CFADS` and `DSCR`, the position after 20 % and 35 % devaluations, and the covenant
breakeven.
*Solution.* Local numerator `30,000,000 × 0.75 − 11,000,000 × 0.60 =` **15,900,000**; USD numerator
`30,000,000 × 0.25 − 11,000,000 × 0.40 =` **3,100,000**; base `CFADS` **19,000,000**, `DSCR`
**1.5323**. At +20 %: `15,900,000/1.20 + 3,100,000 =` **16,350,000**, `DSCR` **1.3185** — holds. At
+35 %: **14,877,778**, `DSCR` **1.1998** — breach. Breakeven: covenant `CFADS` `12,400,000 × 1.30 =`
16,120,000; devaluation multiple `15,900,000/(16,120,000 − 3,100,000) =` 1.2212, i.e.
**+22.12 %**. *Common error:* applying the devaluation to gross revenue and ignoring that local costs
devalue too, which overstates the exposure — here by treating 22,500,000 rather than 15,900,000 as
exposed.

**Exercise 11.4** Four independent register items: (`p` 0.20, 2,500,000), (0.10, 4,000,000),
(0.30, 1,200,000), (0.15, 2,000,000). Compute the mean, σ and P80; recompute the P80 at `ρ` = 0.30;
and state the covenant-preserving exposure ceiling for a project with 900,000 of annual headroom and
`AF(0.06, 10) = 7.360087`.
*Solution.* Mean `500,000 + 400,000 + 360,000 + 300,000 =` **1,560,000**. Variance
`0.16 × 2,500,000² + 0.09 × 4,000,000² + 0.21 × 1,200,000² + 0.1275 × 2,000,000² =`
**3,252,400,000,000**; σ **1,803,441**; P80 `1,560,000 + 0.8416 × 1,803,441 =` **3,077,776**. With
`ρ` = 0.30: the item standard deviations are 1,000,000 · 1,200,000 · 549,909 · 714,143, so
Σσᵢ = **3,464,052** and variance `0.7 × 3,252,400,000,000 + 0.3 × 3,464,052² =`
**5,876,576,724,325**, giving σ **2,424,165** and P80 **3,600,177** — an uplift of **522,401**. Ceiling
`900,000 × AF(0.06, 10) = 900,000 × 7.360087 =` **6,624,078**, comfortably above the correlated P80. *Common error:* summing
the σᵢ rather than the variances for the independent case, which produces 3,464,052 in place of
1,803,441 and nearly doubles the provision.

**Exercise 11.5** A project has `CFADS` of 9,600,000, debt service of 7,100,000 and a 1.20× covenant.
Its business-interruption cover carries a 45-day waiting period; use a 30/360 basis. Compute annual
headroom, the maximum survivable outage, and the `DSCR` after outages of 45, 30 and 15 days. State
the maximum tolerable waiting period.
*Solution.* Covenant trigger `7,100,000 × 1.20 =` 8,520,000; headroom **1,080,000**. Daily `CFADS`
`9,600,000/360 =` **26,666.67**; maximum survivable outage `1,080,000/26,666.67 =` **40.50 days**.
At 45 days uninsured: `CFADS` 8,400,000, `DSCR` **1.1831** — breach. At 30 days: 8,800,000,
**1.2394**. At 15 days: 9,200,000, **1.2958**. Maximum tolerable waiting period **40 days** (40.50
rounded down to a whole day), so the 45-day period leaves a **4.5-day gap** to be closed by a
carve-out, a buy-down or a reserve. *Common error:* computing headroom as `CFADS` ÷ covenant rather
than debt service × covenant — the error that turns a 5 % currency tolerance into a 16 % one in
MCQ 11.3-C.

## Practitioner's toolkit — Domain 11

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 11.T.1 — Risk allocation price sheet (one row per register line)

Columns: ID · risk statement · phase · **our `p` and impact, with the evidence reference** ·
our `EMV` · proposed transferee · **their `p` and impact as priced** · their `EMV` · loading applied ·
loaded premium · **net value of transfer** · **breakeven premium** · **breakeven loading** ·
ground for transfer (control / capacity / neither) · mechanism (clause, guarantee, insurance,
reserve) · liability cap · credit standing behind the cap · decision and decision owner. Rules: a
line with "neither" in the grounds column may not be transferred without a written reason from the
decision owner; a negative breakeven loading is a stop, not a negotiation; and the sheet totals must
reconcile to the retained register the contingency provision is sized against.

### Toolkit 11.T.2 — The breakeven page (one side of paper, per facility)

Every material exposure, expressed in the covenant's own units. Rows: covenant and lock-up **cash
triggers** and the annual headroom (Domain 10) · the **reference rate** at which the covenant fails,
the hedge ratio in force and the ratio covenanted, and the hedge profile against the amortisation
profile · the **exchange rate** at which the covenant fails, the exchange-rate-indexed revenue share
in force and the debt-service-matching share · the **input-price rise** at which the covenant fails,
per material input, with its pass-through share and reset frequency · the **availability or volume**
level at which it fails (Domain 7) · the **outage length in days** at which it fails, beside every
insurance waiting period and deductible, every grace period and every cure period · the
**counterparty concentration schedule**: exposure, mechanism, cap, credit standing, and the
correlation of the call with that standing. A footer states, in bold, which single exposure is nearest
its breakeven — and the answer is rarely the one the risk register ranks first.

### Toolkit 11.T.3 — Register aggregation and re-cut worksheet

Three panels. **Sponsor case:** items with `p`, impact, evidence reference, `EMV`, σᵢ; mean = Σ`EMV`
(hand-checked); variance and σ; the required percentile; and the **correlation assumption stated on
the face of the output** with the uplift it suppresses. **Bank case:** the same items with re-cut `p`
and impact, each change carrying its evidential basis — not a multiplier — plus the correlation
assumption; the resulting mean and percentile; and the gap to the sponsor case, item by item.
**Coverage translation:** annual equivalent of each measure over the loan life at `AF(r, n)`, the
stressed `CFADS` and `DSCR`, the **covenant-preserving exposure ceiling** (headroom × `AF(r, n)`), and
the debt quantum each measure supports at the covenant ratio. A model register in the operating
envelope appears in the item list with an owner, a validation date, a monitored drift metric and a
documented manual fallback, or it is recorded as unquantified.

## Exam preparation — Domain 11

**What is assessed.** The pricing of allocation rather than its description: net value of transfer,
breakeven premium, breakeven loading, and the identification of transfers that destroy value
regardless of margin. The translation of every risk into coverage units — basis points, exchange-rate
moves, days, input-price percentages — and the level at which a named covenant fails. Hedge-ratio
arithmetic, including the minimum ratio surviving a stated shock. Currency-mismatch decomposition and
the indexed share that funds foreign outflows. Register aggregation on the PML-AI Domain 8 method, the
effect of correlation, why a lender's inputs differ from a sponsor's, and the conversion of a register
into a debt-capacity number.

**The calculations to do under time pressure.** Net value of transfer and breakeven loading for a
three-item bundle. Debt service and `DSCR` at a shocked floating rate with fixed principal, and the
breakeven all-in rate. Minimum hedge ratio for a stated shock and covenant. `CFADS` and `DSCR` after
a devaluation, and the covenant-breakeven exchange rate. Register mean, σ and P80, independent and
correlated. Covenant-preserving exposure ceiling. Maximum survivable outage in days against a waiting
period. Pass-through tolerance and its 1/(1 − `φ`) multiplier.

**The traps.** Computing a covenant trigger as `CFADS` ÷ ratio rather than debt service × ratio, which
turns Kestrel's 5.06 % currency tolerance into 15.9 % (MCQ 11.3-C, Exercise 11.5) · applying the
loading to the transferor's `EMV` instead of the transferee's (Exercise 11.1) · treating a negative
breakeven loading as a pricing negotiation (MCQ 11.1-B) · computing an interest-rate breakeven on
total debt service without netting fixed principal first (Exercise 11.2) · confusing the covenant
threshold with the payment threshold — 73.9 bp against 327.2 bp (MCQ 11.3-A) · applying a devaluation
to gross revenue and ignoring that local costs devalue too (Exercise 11.3) · summing standard
deviations instead of variances (Exercise 11.4) · presenting single-variable tolerances as additive
when they share one pool of headroom (11.2.2) · adding a register provision to an estimate-range
provision (Domain 8, KA 8.3.2) · using an unconditional default probability for transferred risk
(11.3.3) · reporting a year-one ratio for a structure with an indexation mismatch (11.2.3) · and
quoting a P80 without stating the correlation assumption underneath it (11.4.1).

**How the domain connects.** Domain 5 identified the bankability conditions each of these risks
threatens; Domain 7 built the revenue and credit machinery this domain allocates; Domain 8 supplied
the `EMV` and confidence method and provisioned against the residue this domain derives; Domain 10
supplied the coverage units every exposure is expressed in. Forward, Domain 12 documents the
allocations priced here — and a clause that does not implement a priced allocation is the defect that
domain exists to catch; Domain 13's diligence streams are where the lender's re-cut register is
actually produced; Domain 14 monitors the construction risks A1–A8 as they crystallise or expire; and
Domain 15 lives with whatever was retained, including the indexation wedge that only appears in the
loan's later years.

## Domain 11 summary
Risk allocation is a price, not a preference, and the price is computable. The net value of a transfer
is the transferor's expected cost less the transferee's loaded premium, and it is reliably negative
where the transferee can neither control the risk nor cheaply bear it: Kestrel's five declined items
carried an owner's `EMV` of **2,840,000** against a premium of **4,620,000**, and destroyed **460,000**
even at a zero margin because the bidder's own expected cost (**3,300,000**) exceeded the owner's — a
breakeven loading of **−13.94 %**, against **+190.79 %** on the three items the bidder genuinely
controlled. Those three transferred for **2,128,000** of premium against **4,420,000** of retained
expected cost — **2.0771** dollars of expected-cost reduction per dollar of premium — leaving the
**2,690,000** register Domain 8 provisions against. Buying the full wrap would have raised capex to
**64,620,000** and, at 70/30, pushed the year-one `DSCR` to **1.1832**, below the project's own
covenant. Market risk is allocated by formula rather than by clause: a **70 %** power pass-through
multiplies Kestrel's tolerance to its largest cost driver by **3.3333×**, from a **20.6910 %** to a
**68.9699 %** price rise, while an indexation mismatch — revenue escalating at an effective **2.00 %**
against costs at **4.70 %** — consumes **85.68 %** of the covenant headroom by year twelve and breaches
in year thirteen, surviving the loan life by **12.41 basis points** of assumed power escalation. The
financial exposures are the most tractable and the most neglected: unhedged, **73.9 basis points** of
reference rate stand between Kestrel and a covenant breach against **327.2** to a payment failure, and
a swap trades **0.0210** of coverage for **0.4397** of coverage range with **70.0576 %** the minimum
hedge ratio that survives a 200 basis point shock; an unindexed host-currency tariff would breach the
covenant on a **5.06 %** devaluation and stop payment at **21.61 %**, against a debt-service-matching
indexed share of **52.997 %** that lifts tolerance to **37.17 %**; every transferred risk becomes a
credit exposure whose relevant default probability is conditional, moving Kestrel's expected credit
loss from **46,410** to **371,280**; and a **60-day** insurance waiting period sits against a covenant
that survives **21 days** of outage. The operating register — where environmental and social,
technology, cybersecurity and AI model risk now sit — has a mean of **2,125,000** and a P80 of
**4,003,649**, rising **837,478** once a 0.30 correlation is admitted, against a lender's re-cut
**mean** of **4,462,500** that already exceeds the sponsor's P80 and a lender P80 of **8,884,036**.
Read through the coverage machinery, Kestrel's covenant survives its register only at the mean: the
covenant-preserving ceiling is **372,437.72 × 8.383844 = 3,122,460** of present-value exposure, and the
lender's P80 removes **4,801,313** of debt capacity. That is the domain's whole argument in one number
— a risk register is not a document, it is a debt-capacity input — and it is the argument Domain 12
must now write into contracts.
