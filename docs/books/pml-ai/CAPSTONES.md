# Appendix G — Integrated capstones

A capstone is not a longer case study. Each domain's cases test whether a candidate can apply one
domain's machinery. A capstone tests what the chapters structurally cannot — whether the answers
**agree with each other**, and whether the reader can see which of them was actually governing while
everyone was watching something else. A programme is not a sequence of sixteen correct disciplines.
It is one commitment whose selection, governance, integration, scope, schedule, cost, risk, quality,
procurement, stakeholders, leadership, method, data, commitments and closeout all have to be true at
the same time, and the instructive failures live in the joints.

**Four capstones, and why the first is different.** Three of the four are new programmes, chosen for
different failure shapes: a **metro-rail programme** where the binding constraint is physical
interface and the political clock, a **grid-scale renewables portfolio** where the unit of management
is a pipeline rather than a project, and a **national public-service programme** where the delivery
organisation does not control the operating one. Each is worked from its own arithmetic.

The first capstone is the **Meridian Care Records** programme, the master thread this volume has
carried since Domain 1. It is not reworked here — reworking it would be the duplication this
programme's editorial rules forbid. What it has never had is an **assembly**. Meridian's numbers are
distributed across sixteen chapters, and read in one place they tell a story no chapter tells: a
programme that delivered every commitment it made, on time and under budget, and lost more value in
its first operating year than it saved in its whole build. Every warning was computable in advance.
Several were computed. None was on the report the board read.

---

## Capstone One — Meridian Care Records: the anatomy of a green programme that failed

**The programme.** A public-health authority rolls a shared clinical-records system to 40 clinics.
Approved cost **USD 2,400,000**; eight-year appraisal life at a 7 % discount rate; cost of delay
**USD 14,280 a week**; the honest business case assumes **70 %** clinician adoption worth
**USD 685,440** a year against a theoretical maximum of **979,200**. Every figure below is a result
printed in this volume and independently recomputed by the golden-answer suite.

### G.1.1 The completion report, and the report nobody wrote

Meridian's completion report said this, and all of it was true:

| Delivery measure | Result |
|---|---|
| Clinics installed | **40 of 40** |
| Schedule | on time |
| Cost | **3 % under** the approved 2,400,000 |
| Capital variance | **+USD 72,000**, banked once |

The programme verdict was taken separately, against benefit, and was also true. Measured adoption
was **40 %**, not 70 %: **391,680** a year against **685,440**, a shortfall of
`979,200 × (0.70 − 0.40) =` **USD 293,760 a year**.

```
annual benefit shortfall  293,760
capital underspend         72,000
                          -------
ratio                        4.08   in the FIRST YEAR ALONE
```

Over Domain 2's eight-year appraisal life the shortfall is **2,350,080** undiscounted — **97.92 %**
of what the entire programme cost to build — or **1,754,128.79** discounted at 7 % on an annuity
factor of 5.971299, still **73.09 %** of the approved cost. Both statements went to the same board in
the same month, and the one printed in bold was the 72,000.

Domain 1 is careful about why: the two verdicts are **not commensurable**, one being a stock banked
once and the other a flow recurring as long as the service runs, so the 4.08 is legitimate as a scale
comparison and not as a net. That care is the point. The failure was not that anyone lied. It was
that the commensurability problem was allowed to become a reporting convenience — a single figure in
the delivery report and a flow that belonged to somebody else's business case.

### G.1.2 The same arithmetical error, twice, on the same dashboard

Meridian's board saw averages where products governed. It happened independently in two domains, and
neither instance required any new information to detect.

**Domain 16, readiness.** Seven go-live conditions, averaging **90.71 %** on the programme dashboard.
Readiness is a **conjunction** — every condition must hold — so the probability of a clean go-live is
their product: **49.79 %**. The gap between the number reported and the number that mattered is
**40.92 percentage points**. Domain 16 also settles the instinctive remedy: even all seven conditions
at 0.98 leaves **13.19 %** of go-lives failing, which is why the answer to the residual is a rehearsed
reversion plan rather than more assurance.

**Domain 15, commitments.** Region A's go-live had six independent predecessors, none worse than 0.85,
giving **52.95 %**. Four such regions committed to one date stand at `0.5295⁴ =` **7.86 %** — a
commitment **92.14 %** likely to be missed **while all twenty-four components report green**.

These are the same mistake in two costumes, and the assembly is what makes that visible. A reader who
meets the readiness conjunction in Domain 16 learns a technique; a reader who sees it beside Domain
15's commitment conjunction learns a **failure mode of reporting itself** — that a dashboard
aggregating by mean will report health at exactly the moment a conjunction is failing, and will do so
more confidently the more components it has. Both instances were available before go-live. Both are
one multiplication.

**What the remediation ranking then buys.** Domain 16's ratio `p′/p` ranks the conditions by what
fixing each is worth. Remediating **to the same target of 0.98** in each case — which is what makes the
comparison fair — lifting the clinical champion from 0.80 is worth **11.20 points** of go-live
probability, against **1.04** for lifting training from 0.96: a **10.7692×** difference on the same
remedial target. Effort spent on the condition that already looks good is nearly worthless, and a
dashboard sorted red-amber-green points a programme directly at it. The seven conditions, for the
record, are training 0.96, data migration 0.90, interfaces 0.94, network and devices 0.98, workflow
redesign 0.85, clinical champion 0.80 and rehearsed fallback 0.92 — summing to 6.35, hence the 90.71 %
mean, and multiplying to 49.79 %.

### G.1.3 Every avoidable cost had a breakeven small enough to be embarrassing

Assemble Meridian's avoidable costs and the pattern is not that the interventions were expensive. It
is that each was trivially cheap against what it prevented, and that the case for each rested on a
**breakeven probability** rather than a ratio.

| Domain | The avoidable cost | What prevention cost | Breakeven |
|---|---|---|---|
| 11 | national reporting body's week-34 objection: **224,520** (9.0 weeks of critical path at 14,280, of which 4.0 weeks are governance latency, plus **96,000** of re-verification and rework) | a pre-consultation at **3,300** | objection probability above **1.4698 %** |
| 6 | an approval modelled as a three-week label rather than against its governance calendar | **1 week** of design work | worth **4 weeks** of schedule |
| 13 | release sequencing: **386,920** of delay cost in the worst order against **231,880** ranked by delay-cost density | nothing — it is a sequencing decision | a **155,040** swing, **10.8571 weeks** of delay cost, free |
| 16 | going live unready | holding **3 weeks** and spending **96,000** | dominated going |

Domain 11 is explicit about why the breakeven and not the ratio is what wins the argument: the
prevention was 68.04 times cheaper than the consequence, and *a sponsor can dismiss a ratio and
cannot assert one and a half per cent.* Read across the four rows, the finding generalises. Every one
of these decisions was taken by someone who believed the risk was unlikely, and in every case the
question they needed was not "how unlikely?" but "unlikelier than 1.47 %?" That is a question a
professional can answer in a meeting, and it is the form the argument should always take.

Note the two governance figures in the first row. Of the nine weeks the objection cost, **four** were
Domain 3's `E[wait] = M/2 + L` — half the meeting interval plus the whole paper lead time — which is
to say that nearly half the price of a stakeholder failure was paid by the committee calendar,
computable at design time, months before anyone objected to anything.

### G.1.4 The benefit was decided before delivery began

The temptation is to read G.1.1 as a delivery failure to be fixed by better change management. The
ledger says otherwise, and Domain 2 says it first.

Meridian's parent authority scores an **alignment index** of `Σ min(declared, funded)` at
**76.6667 %** on the spend that could be mapped to a declared objective and **59.0000 %** on all of
it — the whole difference being the **40 %** of money that serves no declared objective at all. Its
**reallocation distance**, the money that would have to move to make the portfolio match its own
strategy, is **USD 4,200,000**: **1.75 Meridians**. The organisation that could not connect two
fifths of its spending to an objective is the same organisation that later could not explain why
clinicians were not using a records system.

Domain 5 supplies the mechanism, in a different public-sector programme so that the finding is not
one project's anecdote. Case study B reports **612 requirements, 100 % verified, 34.0 %** of forecast
benefit, because the **4,200** external applicants whose behaviour actually produced the benefit had
no seat in a requirements process assembled from the organisational chart: only **47** of the 612 —
**7.6797 %** — addressed their journey. The correction cost **USD 285,000** and returned
**USD 425,500** a year, a payback of **8.04 months**, for a conversation that had been available free
eighteen months earlier.

Set the two together and Meridian's adoption shortfall stops looking like an operational surprise.
**Verification asks whether we built what we specified; validation asks whether the specified thing
produces the outcome — and the two fail independently.** A programme can pass every test it wrote for
itself, which is precisely what 40 of 40 on time and 3 % under budget means, and fail the only test
that was never written down.

### G.1.5 What the delivery organisation could and could not see

Two further numbers belong in the assembly because they bound what a competent leader could have
known.

**The estate the decisions rested on.** Domain 14 finds Meridian's common data environment holding
**14,100** records in six classes with **332** defects — a weighted mean defect rate of **2.3546 %**
carrying **USD 172,820** of consequence-weighted exposure. The distribution is the finding: exposure
looks nothing like the defect rate, and the lowest-rate class in the estate (0.8 %) is the
second-highest by exposure per record at **USD 28.00**. Cleaning by defect rate would have started in
the wrong place.

**The risk register's own completeness.** Domain 8 puts a floor under what was missing rather than
assuming nothing was: two independent identification methods and their overlap imply **11.43**
unidentified risks and **78.61 %** coverage. That is the number that finally gives management reserve
a basis, and it is also an honest statement that roughly a fifth of the risk population was never
written down — which is a fact about method, not about diligence.

**And the leader's capacity, which was never baselined.** Domain 12's figure is the one most likely to
be recognised: recurring commitments of **37.0 hours** in a 45.0-hour planning week — **82.2 %** —
leaving 8.0 hours of discretionary attention, of which 1.5 went to people work. Every intervention in
G.1.3 had to come out of that 8.0 hours. A programme that plans a leader's calendar to 82.2 %
occupancy has, in effect, decided not to do the cheapest work available to it.

### G.1.6 The five questions this capstone equips a candidate to ask

1. **Is any number on this dashboard an average of things that must all be true?** If yes, compute the
   product before the meeting. Meridian's was 90.71 % reported against 49.79 % actual, and 24 green
   components against a 92.14 % probability of missing the date.
2. **Which verdict is a stock and which is a flow, and are both on the same page?** A capital variance
   banked once (+72,000) and an annual benefit shortfall (293,760) are not comparable, and reporting
   only the first is the whole failure in miniature.
3. **What is the breakeven probability, not the ratio?** 1.4698 % is an argument a sponsor cannot
   dismiss; "sixty-eight times cheaper" is one they can.
4. **Do the people whose behaviour produces the benefit have a seat in the requirements process?** If
   the process was assembled from the organisational chart, they almost certainly do not — 7.6797 % of
   requirements in Domain 5's Case study B.
5. **How much of the leader's week is uncommitted?** At 82.2 % occupancy, every preventive measure in
   this ledger competes with every other for the same eight hours.

---

## Capstones Two to Four

The three new-programme capstones — the metro-rail programme, the grid-scale renewables portfolio and
the national public-service programme — are **not yet written**. They are listed in this volume's plan
and in `CORPUS_GATE_REPORT.md` as outstanding, and this appendix does not pretend otherwise. Each
needs its own verified arithmetic on a different failure shape: physical interface and a political
clock on the metro-rail programme, a pipeline rather than a project as the unit of management on the
renewables portfolio, and a delivery organisation that does not control the operating one on the
public-service programme. None may reuse Meridian's figures, because the point of each is that a
different failure shape puts a different quantity in control — which is what G.1.2 and G.1.4 establish
for a clinical-records rollout and what cannot be assumed to transfer.

Saying so is the honest alternative to filling the space. A capstone that recycled the master thread's
numbers under a new programme name would add pages and subtract credibility.
