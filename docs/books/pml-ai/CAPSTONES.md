# Appendix G — Integrated capstones

A capstone is not a longer case study. Each domain's cases test whether a candidate can apply one
domain's machinery. A capstone tests what the chapters structurally cannot — whether the answers
**agree with each other**, and whether the reader can see which of them was actually governing while
everyone was watching something else. A programme is not a sequence of sixteen correct disciplines.
It is one commitment whose selection, governance, integration, scope, schedule, cost, risk, quality,
procurement, stakeholders, leadership, method, data, commitments and closeout all have to be true at
the same time, and the instructive failures live in the joints.

**Four capstones, and why the first is different.** Three of the four are new programmes, chosen for
different failure shapes: **Calderhall Metro Line 3** (G.2), a metro-rail programme where the binding
constraint is physical interface and a political clock; **Sable Plains Energy** (G.3), a grid-scale
renewables portfolio where the unit of management is a pipeline rather than a project; and the
**National Entitlement Service** (G.4), a national public-service programme where the delivery
organisation does not control the operating one. Each is worked from its own arithmetic, and none reuses
Meridian's figures, because the point of each is that a different failure shape puts a different
quantity in control. Every programme, organisation and person in this appendix is fictitious.

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

| Line | USD |
|---|---|
| Annual benefit shortfall | 293,760 |
| Capital underspend | 72,000 |
| **Ratio, in the first year alone** | **4.08** |

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

## Capstone Two — Calderhall Metro Line 3: the date nobody in the programme owns

Meridian's failure was a **conjunction reported as a mean**: information that existed, aggregated by
the wrong operation, on a dashboard nobody could read arithmetically. Calderhall's failure is not
about information at all. Every number below was known, correctly aggregated and correctly reported.
The programme still lost because of a **structural** fact: the date belonged to somebody outside the
programme, the scope was not equally fixed, and the exchange rate between the two had been set years
earlier by a decision that looked like a procurement choice and was in fact a schedule choice.

**The programme.** A metropolitan transport authority builds Line 3, a **19 km**, **12-station**
driverless extension, at a capital expenditure of **USD 2,880,000,000**. Notice to proceed is month 0.
The line is committed to open at **month 63** — the date is set by a fixed-date international event the
city is hosting, whose dates belong to the event's owner and cannot move, and which no one inside the
programme can influence. All amounts are USD; schedule is in calendar months from notice to proceed;
percentages are shown to four decimal places where a comparison depends on them; and money is computed
at full precision and rounded for display, so re-multiplying two displayed figures may differ from the
displayed product in its final digit. *(Calderhall, its authority, the line and the event are
fictitious. Interface, possession and productivity figures are illustrative and do not describe any
real railway. Whether a concession or grant condition permits an opening date to be renegotiated is a
question of the specific instrument and of local law, and belongs to qualified counsel rather than to
this arithmetic.)*

### G.2.1 The interface count, and the 0.2000 % decision

Domain 4 gives the count and the architecture choice: pairwise interfaces grow as `n(n − 1)/2` and a
layered architecture holds them to `n`. Calderhall's baseline design has **16** controlled subsystems —
track, traction power, signalling, rolling stock, platform screen doors, tunnel ventilation, fire and
life safety, drainage, communications, passenger information, fare collection, lifts and escalators,
the control centre, the depot, station civils, and the interface to the existing network.

| | Baseline (16 subsystems) | After design growth (19) |
|---|---|---|
| Possible pairwise interfaces `n(n−1)/2` | **120** | **171** |
| Interfaces the register actually requires | 80 | 126 |
| Register as a share of the bound | **66.6667 %** | **73.6842 %** |
| Interfaces under a layered architecture (`n`) | **16** | **19** |

Three subsystems arrive during detailed design: an interchange with an existing line, a third-party
utility corridor, and a second depot connection. They add **51** *possible* interfaces — **16, 17 and
18** as each in turn joins, which averages **17** and equals it for none of the three — and **46**
required ones, because each of the three connects to nearly everything. This is Domain 4's caution
honoured rather than ignored: the count that matters is the register, built from need, and the formula
is the **bound** the register is tested against.

At a fully loaded **USD 145,000** to specify, design, build, test, witness and document one bilateral
interface, and **USD 5,760,000** for a systems-integration authority with a controlled interface
baseline and an integration test facility:

| | Bilateral | Layered | Saving |
|---|---|---|---|
| Baseline scope | **11,600,000** | **8,080,000** | **3,520,000** |
| After growth | **18,270,000** | **8,515,000** | **9,755,000** |
| Cost *of the growth itself* | **6,670,000** | **435,000** | ratio **15.3333×** |

The layer remains worth building at baseline while its cost is below **USD 9,280,000**. But the third
row is the one to read. **The same three subsystems cost 6,670,000 on a bilateral architecture and
435,000 on a layered one** — the architecture does not change what the growth is, only what it costs,
and it changes that by a factor of fifteen. The 15.3333 is not an artefact of the unit cost, which
cancels: it is `46 ÷ 3`, the ratio of the interface counts the two architectures make the growth into.
One modelling choice is worth stating, because it cuts against the layer rather than for it: the
145,000 unit cost is held constant for a layer interface even though verifying one takes twice as many
witnessed test-days (G.2.2), on the reasoning that a layer interface has one target and one protocol and
is correspondingly cheaper to specify and build. Set the figures against the capital budget and the
proportions become uncomfortable: the baseline interface work is **0.4028 %** of capital expenditure and
the integration layer is **0.2000 %** — one part in five hundred. The decision that governs everything
below was, on the programme's own cost report, a rounding error.

### G.2.2 The count becomes a duration, and the duration meets the date

The cost is not the mechanism. Interface verification sits on the critical path, and its duration is
proportional to the count, because each interface must be witnessed on an energised, complete system.
Calderhall can run **5** concurrent test streams — limited by one operational control centre, one
energised test track and one authorised witnessing authority — for **20** working days a month:
**100 witnessed test-days a month**. A bilateral interface takes **10** witnessed test-days, so the
campaign verifies **10 interfaces a month**. A conformance test against a controlled interface baseline
takes longer per interface — **20** test-days — because it is a full suite rather than a point check.

| | Interfaces | Test-days | Verification campaign |
|---|---|---|---|
| Bilateral, baseline scope | 80 | 800 | **8.00 months** |
| Bilateral, after growth | 126 | 1,260 | **12.60 months** |
| Layered, after growth | 19 | 380 | **3.80 months** |

The rest of the critical path is **53 months** — civils and station boxes 32, systemwide installation
14, trial running 4, independent safety assessment and authorisation 3 — and none of it moves.

| | Critical path | Against month 63 |
|---|---|---|
| Baseline, bilateral | **61.00 months** | **2.00 months of float** |
| After growth, bilateral | **65.60 months** | **2.60 months late** |
| After growth, **layered** | **56.80 months** | **6.20 months of float** |

Read the third row twice. A layered architecture carrying the **grown** scope finishes verification
**4.20 months** earlier than a bilateral architecture carrying the **original** scope, and the same
4.20 months appears as the difference in float — 6.20 against 2.00 — which is the check that the two
readings agree. **The architecture decision was worth more than the entire scope growth.** The growth
was survivable. What made it fatal was a choice about integration structure taken years before anyone
knew the growth would happen, by people who believed they were choosing a procurement model.

### G.2.3 What the immovable date actually costs

Calderhall's cost of delay is **USD 3,150,000 a month**: deferred farebox and commercial revenue of
**1,890,000** (45,000 boardings a day × 30 days × an average fare of **USD 1.40**) plus **1,260,000** of
extended programme management, site establishment and financing carry. That is **USD 735,000 a week** on
a 30-day month, **51.4706 times** Meridian's 14,280 (G.1.3) — the same discipline at fifty-one times the
price of a week, which is why the metro's governance latency and its architecture decisions cannot be
governed with a health programme's thresholds.

If the date could move, the 2.60-month overrun would cost **USD 8,190,000** and the conversation would
end there. It cannot. So the overrun has to be repaid in **scope**, and the exchange rate is exact: at
100 test-days a month and 10 per interface, 2.60 months is **26 interfaces**, which is 2.60 months'
worth of the 4.60 the growth added, the other 2.00 having been absorbed by the float that existed. Each
deferred interface is therefore worth `0.10 months × 3,150,000 =` **USD 315,000** of avoided delay.

Deferred to what, though? After opening, the railway owns the night. Interface verification then
happens in engineering possessions yielding **2.5** productive hours against a day shift's **10.0** — a
productivity factor of **0.25**, so **4.0** possession nights replace one test-day.

| Step | Result |
|---|---|
| `26 interfaces × 10 test-days` | 260 test-day equivalents |
| `260 / 0.25` | 1,040 possession nights |
| `1,040 / 200 nights available a year` | **5.20 years** of post-opening integration |
| `26 × 145,000` delivered in the programme | 3,770,000 |
| `26 × 145,000 / 0.25` in possessions | 15,080,000 |
| **Extra cost of deferring** | **11,310,000** — 435,000 an interface |

Now put the two prices beside each other, per interface, because at that unit the count cancels:

| Price, per interface | USD |
|---|---|
| Cost of deferring one interface into possessions | 435,000 |
| Cost of the 0.10 month that same interface buys | 315,000 |
| **Ratio** | **1.3810** |

(The 435,000 here and the layered cost of the whole scope growth in G.2.1 are the same number for an
arithmetical reason rather than a related one: each is three interface-units — one because the
possession multiplier is 4.0, the other because three subsystems arrived. Unrelated quantities that
happen to coincide.)

**Holding the date costs 1.3810 times what missing it would have cost, and the ratio does not depend on
how many interfaces are deferred** — it is a property of the possession premium and the cost of delay,
not of the programme's size. On the totals the same figure appears as `11,310,000 ÷ 8,190,000`. This is
the finding a reader could not have guessed from Capstone One, and it runs against the instinct that a
committed date is defended first and paid for afterwards: **a date defended without an architecture is
defended at a loss.**

Three qualifications keep the result honest, and none of them reverses it. The 4.0 multiplier assumes
the cost is labour-dominated; possession working also carries premium rates, re-mobilisation and
protection staff, so **11,310,000 is a floor**. Both figures are one-off totals caused by the same
decision, which is the point of difference from G.1.1: there a stock was set beside a flow and only a
scale comparison was available, whereas here the two are commensurable and may legitimately be
subtracted. What they do not share is timing — the 8,190,000 falls in the 2.60 months before opening
and the 11,310,000 across the 5.20 years of possessions after it, so at any positive discount rate the
deferral premium is worth less than its undiscounted total, while the premium rates push the other way.
And a fixed political date carries consequences the 3,150,000 does not price: a committed event,
statutory undertakings, a public promise already made. Those consequences are exactly what the
arithmetic hands the decision back to, because it now has a threshold: holding the date costs
`11,310,000 − 8,190,000 =` **USD 3,120,000** more than moving it, so the date is worth defending if and
only if the unpriced consequences of moving it are worth more than 3,120,000. That is a question a
transport minister can answer and a spreadsheet cannot, and it is a question nobody was able to put
until the two costs were computed.

### G.2.4 The lever that is not available: adding people

Faced with 2.60 months, the programme proposes doubling the verification team from **60** engineers to
**120**. This buys nothing, for two independent reasons, and the second is worse than the first.

**The campaign is stream-limited, not effort-limited.** Capacity is 100 witnessed test-days a month
because each test consumes the control centre, the energised test track and the witnessing authority.
A sixth stream needs a second energised section, which the traction-power staging cannot provide inside
the remaining programme. Sixty additional engineers at **USD 1,450** a day over 20 days for 2.60 months
cost **USD 4,524,000** and shorten the critical path by **zero months**.

**And the team gets smaller as it grows.** Domain 12's coordination arithmetic, applied unchanged:

| Team size | Communication paths `n(n−1)/2` | Coordination share `c(n−1)/2h` | Net capacity `hn − c·n(n−1)/2` |
|---|---|---|---|
| 60 | **1,770** | **36.8750 %** | **1,515 hours a week** |
| 120 | **7,140** | **74.3750 %** | **1,230 hours a week** |

At Domain 12's calibrated link cost `c` = 0.5 hours a week and a 40-hour week, doubling the team
multiplies communication paths by **4.0339** and **reduces net capacity by 285 hours a week —
18.8119 %**. Net capacity peaks at **1,620 hours a week**, reached at 80 and 81 people alike — the
continuous optimum is 80.5, so the two integers tie, exactly as Domain 12 records it — and at 120 the
team forgoes **390 hours a week** against that peak. So the proposal spends 4,524,000 to remove 285
hours a week of productive capacity from the function on the critical path. Domain 12 states the rule;
a programme under date pressure is where it is disbelieved.

### G.2.5 The last 0.80 months, and who owns them

Descoping 26 interfaces is a decision the programme is not free to take either. Of the 26, only **18**
are demonstrably outside the safety-authorisation boundary — depot connections, commercial systems, the
utility corridor's non-safety functions. The remaining **8** are inside it, and no arithmetic entitles a
railway to open with an unverified fire, evacuation or signalling interface.

| Step | Result |
|---|---|
| `18 interfaces × 0.10 months` | 1.80 months bought by descoping |
| Overrun | 2.60 months |
| **Residual** | **0.80 months**, or **USD 2,520,000**, with nowhere to go |

G.2.3's exchange rate survives the narrowing, which is the test of whether it was a real finding or an
artefact of the round 26: the 18 the programme may lawfully defer cost **7,830,000** in possessions to
avoid **5,670,000** of delay, the same **1.3810**, and the last 0.80 months cannot be bought at any
price because the only thing left to sell is a safety verification.

The only remaining levers are trial running (4 months) and authorisation (3 months), and **both belong
to the independent safety assessor**, which has no accountability for the opening date and no incentive
to shorten either. Count the quantities that determine whether Line 3 opens on the committed date —
the date, the opening scope, the integration architecture, the trial-running duration, the
authorisation duration — and the programme controls **two of the five**, and one of those two, the
opening scope, only as far as the safety boundary allows: 18 of the 26 interfaces, **69.2308 %** of the
lever it appears to hold. That is the structural statement of Calderhall's failure, and it is the door
into Capstone Four.

### G.2.6 The five questions Calderhall adds to Capstone One

1. **What is the interface count, and what is it a count of?** The register, not the formula; the
   formula is the bound. Three subsystems added 51 possible interfaces and 46 required ones here,
   costing **15.3333 times** more on a bilateral architecture than on a layered one.
2. **Is any critical-path activity's duration proportional to a count that is still growing?** If so,
   the count is a schedule risk, not a cost risk. Verification here ran 8.00 months at the original
   scope, 12.60 at the grown scope and 3.80 at the grown scope on a layered architecture — and once the
   scope had grown, only the third made the date.
3. **What is the exchange rate between schedule and scope?** With the date fixed, there is one, and it
   is arithmetic: 100 test-days a month, 10 an interface, so 2.60 months = **26 interfaces**. A
   programme that cannot state this rate is descoping without knowing the price.
4. **Is holding the date cheaper than moving it?** Compute both. Here deferral cost **435,000** an
   interface against **315,000** of avoided delay — a ratio of **1.3810**, independent of the count —
   so the date was defended at a loss.
5. **How many of the quantities that decide the outcome do I control?** Two of the five at Calderhall,
   and one of those two only in part. The answer bounds every plan built on the other three, and it is
   available on day one.

---

## Capstone Three — Sable Plains Energy: the arithmetic that has no project in it

Capstones One and Two are single undertakings: one programme, one railway, one date. Sable Plains is
the capstone that changes the unit of account. Every project in its portfolio is competently appraised
and individually attractive, every business case is arithmetically correct, and the enterprise still
forgoes more than two fifths of its build capacity to one portfolio decision and **39.3276 %** of every
project's appraised value to another — because the decisions that govern it are **portfolio decisions
that appear in no project's model**, and a reader who reasons project by project cannot see any of
them. (The two losses are not additive and this capstone does not add them: they belong to different
regimes, one steady-state and one episodic, and each is separately large.) This is the one that should
break a habit.

**The developer.** Sable Plains Energy develops, builds and operates utility-scale solar with
co-located storage. Its pipeline holds **48** named prospects — **2,400 MW** nameplate at **50 MW** a
build. A build costs **USD 60,000,000** (**USD 1,200,000 a megawatt**), takes **one year** with the
resource it needs, and earns **USD 7,200,000** a year of net contribution for a **25-year** contracted
life — **USD 46.9667 a megawatt-hour** on **153,300 MWh** at a 35 % capacity factor. The discount
rate is **8 %**. Measured throughput of the grid-connection and energisation function, the binding
capability, is **4 energisations a year** (12 in the last three years). All amounts are USD; years run
from each build's investment decision; capital is modelled as drawn in equal annual instalments ending
at energisation, and that convention is load-bearing — see the caution in G.3.2. Every figure is
computed at full precision and rounded for display, so re-multiplying two displayed figures may differ
from the displayed product in its last few digits. *(Sable Plains, its pipeline and every figure below
are fictitious and illustrative. Connection-queue rules, land-option law and the consequences of
relinquishing a connection offer are jurisdiction-specific and are questions for qualified counsel.)*

### G.3.1 The pipeline is the asset, and it costs 1.9377 times what the projects report

Five development gates stand between origination and an investment decision. Each is passed or the
prospect dies, so conversion is the **product** — G.1.2's finding in a new costume, and worth meeting
twice because in a portfolio it decides the size of the whole enterprise rather than the honesty of one
dashboard.

| Gate | Stage cost | Pass rate | Cumulative survival *after* this gate | Prospects *entering* this gate, per built project |
|---|---|---|---|---|
| Screening | 40,000 | 0.72 | 0.720000 | **5.7132** |
| Land option | 120,000 | 0.65 | 0.468000 | **4.1135** |
| Grid application and offer | 260,000 | 0.55 | 0.257400 | **2.6738** |
| Consent | 640,000 | 0.80 | 0.205920 | **1.4706** |
| Offtake and investment decision | 400,000 | 0.85 | **0.175032** | **1.1765** |

Conversion is **17.5032 %** against an arithmetic mean pass rate of **71.4000 %** — a gap of **53.8968
percentage points**, wider than Meridian's 40.92 (G.1.2) on *fewer* factors, because the gap widens with
both the count and the distance of each factor below one, and these five are weak where Meridian's seven
were mostly strong. The geometric mean, **0.7057055**, is the number that actually behaves like a pass
rate: raised to the fifth power it returns 17.5032 % on the digits printed here, which the arithmetic
mean never does. To energise 4 builds a year the organisation must originate **22.8530 prospects a year**.

Now the finding, which is a cost and not a probability. The entrants column is the reciprocal of the
downstream conjunction — 1.4706 prospects reach consent per build because `1 ÷ (0.80 × 0.85)` of them
must — and multiplying it by the stage costs gives the development cost the **portfolio** actually
incurs for one built project:

| Basis | USD per built project |
|---|---|
| The portfolio: `5.7132 × 40,000 + 4.1135 × 120,000 + 2.6738 × 260,000 + 1.4706 × 640,000 + 1.1765 × 400,000` | **2,829,105.53** |
| The closing project's own path, on its charge code: `40,000 + 120,000 + 260,000 + 640,000 + 400,000` | **1,460,000** |
| **Understatement** | **1.9377×** |

The entrant counts are displayed to four decimal places and the sum is computed from the exact
reciprocals, so re-adding the displayed line will land a few dollars away. **A built project's
development cost is 1.9377 times what the project reports**, because **USD
1,369,105.53** of it — **48.3936 %** of all portfolio development spend — is spent on prospects that
never build. At four builds a year that is **USD 5,476,422.14** a year, or **USD 27,382.11 a megawatt**,
carried by no business case in the company. PFL-AI's own capstone reaches the same conclusion from a
cruder ratio — programme spend divided by closings — and the stage-resolved form here does something
the ratio cannot: it shows *where* the money goes, which is what a leader needs in order to move it.

### G.3.2 Twelve good projects and one bad decision, which was not about any of them

Twelve connection offers mature inside a two-year window. Each carries a contractual energisation
milestone set by the network operator; miss it and the capacity is lost. Every one of the twelve has
been appraised and approved on its own merits. The board starts all twelve.

Twelve builds sharing a function sized for four do not run at full speed. They run at roughly a third
of it, which is Little's Law's content (Domain 13, KA 13.2.3, applied at portfolio level in KA 15.3.2):
at a throughput of 4 a year, holding 12 in flight makes the average cycle time **3.00 years**. So
compare the same twelve builds, the same twelve contracts and the same capacity under two sequencing
policies.

| | One-year build | Three-year build |
|---|---|---|
| Capital drawn | 60,000,000 at year 1 | 20,000,000 at years 1, 2 and 3 |
| PV of capital at 8 % | **55,555,555.56** | **51,541,939.74** |
| PV of contribution | **71,165,174.59** | **61,012,666.83** |
| **NPV** | **15,609,619.04** | **9,470,727.09** |

Present value of contribution at energisation is `7,200,000 × AF(0.08, 25) = 7,200,000 × 10.674776 =`
**76,858,388.56**; the three-year column discounts it three years and prices capital on
`AF(0.08, 3) = 2.577097`. **Stretching the build from one year to three removes USD 6,138,891.95 —
39.3276 % — of every project's appraised value**, and it does so without changing one line of any
project's contract, cost estimate, resource plan or offtake.

Then the portfolio comparison, which is the capstone's central finding:

| Twelve builds, twelve offers, one capacity | Present value at the decision |
|---|---|
| All twelve started together (each takes 3 years) | 12 × the three-year NPV = **113,648,725.02** |
| Three cohorts of four, each at full speed | 4 × the one-year NPV × 2.783265 = **173,782,809.45** |
| **Released by sequencing** | **+60,134,084.43, or +52.9122 %** |

The deferral factor **2.783265** is `1 + 1.08⁻¹ + 1.08⁻²`, the second and third cohorts starting a year
and two years later. Two invariants keep this from being a conjuring trick, and both must be stated or
the result is not believable. **Capacity consumed is identical**: 4 a year for three years is twelve
builds either way. (The simultaneous case has the scarce function spread thinly across all twelve rather
than serving them in order, which is what produces both the three-year cycle time and twelve
energisations arriving together. A first-in-first-out queue on the same capacity would finish four a
year — the sequenced policy, reached by discipline instead of by decision.) **The last build energises
in year three under both policies** — sequencing does not
finish the programme earlier; it finishes eight of the twelve earlier and the last four at the same
time. Per build, the sequenced plan is worth **1.5291 times** the simultaneous one — 14,481,900.79
against 9,470,727.09 — so the *gain* is 0.5291 of a simultaneous build's value, **USD 5,011,173.70**
each, and it is bought entirely by declining to start work the organisation cannot progress.

The same fact in steady state, since Sable Plains is not a one-off: at four builds a year the policy
forgoes **USD 24,555,567.80 a year** — **10.2315 %** of the **240,000,000** of capital it deploys
annually — which capitalised at 8 % is **USD 306,944,597.50**. And the same fact on the balance sheet,
on a deliberately different convention: with capital drawn uniformly, average capital in construction
is **120,000,000** at four in flight and **360,000,000** at twelve, so the policy ties up an extra
**240,000,000** permanently — the same figure as the annual deployment two sentences earlier, because
eight extra builds half-drawn is arithmetically four builds' capital, and a different quantity
entirely — carrying **19,200,000** a year at the cost of capital. Two conventions, two figures in the
same range, neither derived from the other: **24,555,567.80** a year of value forgone on discounted cash
flow against **19,200,000** a year of carry on capital held idle. They are not a reconciliation — they
differ by **5,355,567.80** and nothing here bridges that, because they price different consequences of
one policy, which is what PFL-AI's Domain 6 means by a basis bridge and this is not one. What two
independent conventions landing in the same range do establish is narrower and worth stating exactly:
the finding is not an artefact of either. One model supporting one number could not have shown that.

Three cautions. The equal-instalment capital convention is doing real work: a build that stretches also
stretches its drawdown, which is why the three-year column's PV of capital is *lower* than the
one-year's, and a front-loaded contract would produce a larger loss while a genuinely deferred one
produces a smaller. **State the drawdown convention beside the result or the result is not
interpretable.** Second, cycle time does not stretch exactly in proportion — dedicated construction
contractors are not shared, so only the scarce functions queue, and 3.00 years is therefore an upper
bound and 60,134,084.43 with it. Third, the sequenced plan requires eight connection offers to be
re-timed or relinquished, and the value available to pay for that is **USD 7,516,760.55 an offer** —
**5.3691 times** a 1,400,000 deposit. That is the breakeven form G.1.3 argues for: not "sequencing is
worth sixty million", which a board can dismiss, but "re-timing an offer is worth doing unless it costs
more than seven and a half million", which it cannot.

### G.3.3 Two queues, and the one nobody manages

Why did twelve offers mature together in an organisation that can build four a year? Because the two
halves of the enterprise are separate queues running at different rates, and only one of them has a
manager.

Development is slow and its slowness is computable. Each stage takes elapsed time whether the prospect
passes or fails, so expected development duration per prospect originated is the survival-weighted sum:

`1.000000 × 0.25 + 0.720000 × 0.50 + 0.468000 × 1.25 + 0.257400 × 1.75 + 0.205920 × 0.75 =`
**1.79989 years in the system**.

Little's Law again, now on the development pipeline: to originate 22.8530 prospects a year and hold each
for 1.79989 years, the organisation must be carrying **41.1328 prospects in development at once**. It is
resourced for **24**. So development completes `24 ÷ 1.79989 =` **13.3341** prospects a year, which at
17.5032 % conversion feeds **2.3339 builds a year** against a build capacity of **4**.

**The binding constraint is development, and nobody is managing it.** The shortfall is **1.6661 builds a
year — 83.3049 MW, USD 26,007,145.26 of NPV a year** that the construction organisation has the capacity
to create and will never be given. Closing it needs **17.1328** more prospects in development, a
**71.3868 %** increase, which at three prospects a manager is **5.7109** development managers at a
loaded **USD 240,000** each — **USD 1,370,625.71** a year. Against a build worth 15,609,619.04, that
spend breaks even at **0.0878 additional builds a year** — one extra build every **11.3887 years**.
The function that gates the entire
enterprise pays for itself if it produces one more project every eleven years, and it is short by one
and two thirds a year.

And the queue behind it ages. Twenty-four prospects wait for a development slot; at 13.3341 completions
a year the wait is **1.7999 years**, so the average prospect spends **3.5998 years** in the pipeline all
told, queue and development together. That average is not the figure to set against an instrument,
because it mixes the survivors with the four fifths that leave, each of them sooner and more cheaply than
a survivor even though 48.3936 % of the money is still theirs. The prospects holding land
options are the ones that keep passing gates, and a prospect that passes all five spends the full
**4.50 years** of stage durations in active development, having secured its option **0.75 years** in. Its
option therefore has to hold for **3.75 years** against a life of **3** — it expires **0.75 years**, nine
months, before the decision it exists to protect, and that is before the 1.7999 years of queue in front
of it. **Every prospect that reaches an investment decision outlives its own land option**, which is why
offers, options and consents expire in clusters and then a cohort of twelve arrives at once. (That the
queue wait and the average time in development are both 1.7999 years follows from the queue being
exactly as long as the active caseload, 24 either way; it is an artefact of these figures rather than a
general identity.)

So the sequence runs: development is under-resourced, which starves the build function, which makes the
build function look idle, which invites the board to start everything that is ready the moment offers
mature — and the concurrency then destroys 39.3276 % of each project's value. **The overload of one
queue was caused by the starvation of the other.** No project's business case contains any part of that
chain, and no project manager could have found it.

### G.3.4 The five questions Sable Plains adds to Capstones One and Two

1. **What does one delivered unit cost the portfolio, as against the project?** Here 2,829,105.53
   against 1,460,000 — **1.9377×** — because **48.3936 %** of development spend buys prospects that
   never build. A charge code that shows only the survivor always concludes development is cheap.
2. **What is the measured throughput, and how many things are in flight?** If work in progress exceeds
   throughput × the technical duration, cycle time is a management decision, not an attribute of the
   work: 12 against 4 here, costing **39.3276 %** of every project's NPV.
3. **Would sequencing the same work release value without delaying its completion?** Test it against the
   two invariants — same capacity consumed, same final completion date. At Sable Plains sequencing
   released **60,134,084.43, +52.9122 %**, and the last build still energised in year three.
4. **Which queue is actually binding, and is anyone managing it?** Development feeds **2.3339** builds a
   year into a capacity of 4. The constraint was in the function with no delivery accountability,
   and its breakeven was one extra build every **11.3887 years**.
5. **Does the pipeline age faster than its own instruments?** A survivor here needs its land option to
   hold **3.75 years** against a 3-year life — it expires nine months before the decision it protects —
   which guarantees expiry clusters, and an expiry cluster is what forces the concurrency in question 2.

---

## Capstone Four — the National Entitlement Service: twenty vetoes and one accountability

Calderhall controlled two of the five quantities that decided its date (G.2.5) and that was the
structural finding hiding behind its arithmetic. The National Entitlement Service is that finding made
the whole subject. Here the delivery organisation does not control the operating one, accountability is
split across bodies that each hold a veto, and **the programme's dominant quantity is not any body's
behaviour — it is the number of bodies**. That claim is easy to assert and it is usually asserted
without arithmetic, which is why it never changes anything. This capstone computes it.

**The programme.** A national digital agency replaces a paper-based entitlement assessment with a
digital service. Policy belongs to a ministry; money to a funding committee; data-sharing and privacy
design to a national data authority; the operating model to a council representing **14** regional
operating offices; service standards to a digital standards body; and statutory accessibility sign-off
to an independent panel. The service handles **2,600,000** assessments a year at **USD 41.60** each
manually and **USD 12.30** digitally. All amounts are USD; latency is in weeks; percentages and
weeks are shown to four decimal places where a comparison depends on them, and money is computed
from the exact latencies rather than the displayed ones, so re-multiplying a displayed week count by
the cost of delay will differ from the printed money by a few tens of dollars. *(The service, the
jurisdiction and all six bodies are fictitious. Statutory approval structures, and whether decision
rights can lawfully be consolidated at all, differ entirely between jurisdictions and are questions
for qualified counsel — the arithmetic below prices a governance design, it does not prescribe one.)*

### G.4.1 Six decision rights, priced

Domain 3's governance latency is `E[wait] = M/2 + L` for a body meeting every `M` weeks with papers
closing `L` weeks ahead, and `E[wait] = M/2 + L + (1/q − 1) × M` where the body returns an item for
more information with probability `1 − q` (KA 3.1.2, KA 3.2.3). The first-pass rates below come from the
programme's own decision log over 30 months.

| Body | `M` | `L` | Base `M/2 + L` | First-pass `q` | `E[wait]` |
|---|---|---|---|---|---|
| Policy ministry | 4 | 2 | **4.0** | 0.80 | **5.0000** |
| Funding committee | 13 | 3 | **9.5** | 0.90 | **10.9444** |
| National data authority | 6 | 2 | **5.0** | 0.70 | **7.5714** |
| Regional operations council | 8 | 3 | **7.0** | 0.75 | **9.6667** |
| Digital standards body | 3 | 1 | **2.5** | 0.85 | **3.0294** |
| Accessibility and equality panel | 10 | 2 | **7.0** | 0.95 | **7.5263** |
| **Total** | | | **35.0** | product **30.5235 %** | **43.7383** |

One assumption is doing work in that total and should be visible before anything is built on it: the six
sit in **series**, because each body's paper needs the last body's decision — money follows policy, the
data authority's design follows the operating model, the accessibility panel signs off what the others
settled. Where bodies are genuinely independent the right model is the **maximum** and not the sum, and
the arithmetic then says to run them at the same time: the longest single wait is the funding committee's
10.9444 weeks, so a fully parallel path would save **32.7938 weeks**, more than either lever priced in
G.4.2. That lever is available exactly to the extent the dependencies are imaginary, which is a question
about this programme and not about arithmetic. What parallelism cannot touch is the other half of the
finding: the probability of clearing six bodies first time is **30.5235 %** whether they sit in series or
side by side, because a conjunction does not care about order.

Returns add **8.7383 weeks — 24.9665 %** — to a path that was already 35.0 weeks long before anyone
disagreed with anything. And the conjunction is the same theorem as G.1.2 and G.3.1 in a third costume:
the mean first-pass rate is **82.5000 %**, the probability of clearing all six first time is
**30.5235 %**, and the gap is **51.9765 percentage points**. The geometric mean, **0.8205513**,
returns the product at the sixth power on the digits printed here; the arithmetic mean reproduces nothing.

The programme's cost of delay is derived, not asserted: `2,600,000 × (41.60 − 12.30) =` **USD 76,180,000
a year** of operating saving, so **USD 1,465,000 a week**. (Citizen-service benefit is deliberately
excluded, because it is contested and the finding does not need it.) The single most consequential
decision the programme must take — the change to the assessment operating model — requires all six
bodies, and therefore costs `43.7383 × 1,465,000 =` **USD 64,076,561.50** of deferred benefit before a
single body says no.

### G.4.2 The finding: the count dominates the conduct

Every governance improvement programme in this situation attacks behaviour — better papers, better
pre-briefing, better secretariat, a first-pass rate target. Price that lever at its ceiling. Suppose
every one of the six bodies reaches **0.95**, the best first-pass rate any of them has ever achieved:

| Path | Latency |
|---|---|
| At present | 43.7383 weeks |
| With all six bodies at 0.95 — `35.0 + 44/19` | 37.3158 weeks |
| **Saved by every body at its best observed rate** | **6.4225 weeks**, or **USD 9,408,929.92** |

The `44/19` is exact: a 0.95 first-pass rate costs `1/0.95 − 1 = 1/19` of one meeting interval per body,
and the six intervals sum to 44 weeks, so the whole return penalty falls to **2.3158 weeks**. That
is the observed behavioural lever in full — an extraordinary improvement to ask of six independent
institutions at once — and it also raises the clean-approval probability from 30.5235 % to
**73.5092 %**, which is a real gain. Now price the other lever. Consolidate to three decision rights,
keeping policy, money and data, and folding the standards assessment into the data authority's, the
accessibility sign-off into a once-for-the-service determination, and the regional council's voice into
the ministry's board:

| Consolidated to three decision rights | Value |
|---|---|
| Latency on a three-body path | 23.5159 weeks, or USD 34,450,753.97 |
| **Saved** | **20.2224 weeks**, or **USD 29,625,807.53** |
| First-pass probability rises to | 50.4000 % |

**Halving the number of decision rights is worth 3.1487 times the best observed improvement in every
body's conduct.** And the result does not depend on where the behavioural ceiling is put: push conduct
past anything observed, to a path on which no body ever returns anything — `q = 1` at all six, which no
institution achieves — and the lever buys the entire return penalty, **8.7383 weeks**, against which the
count lever is still **2.3142 times** larger.

Now the sharper form of the same finding, which is the one to carry away. The average
decision right on this path costs `43.7383 ÷ 6 =` **7.2897 weeks**, or **USD 10,679,426.92**. Lifting
all six bodies to the best rate any of them has achieved saves 6.4225 weeks — **88.1033 %** of what one
single additional body costs, so **no attainable improvement in how these bodies behave pays for one
more of them.** The unattainable version pays for one and very little more: abolishing returns
altogether comes to **119.8712 %** of one decision right, which is the honest measure of the headroom —
one extra approver costs about what every return in the system costs. Adding a seventh body resembling
the six takes latency to **51.0280 weeks** and drops clean approval to **25.0461 %**;
deleting one — taking the accessibility determination once for the service rather than for every change,
which removes no scrutiny anyone can name — saves **7.5263 weeks, USD 11,026,052.63**, and lifts clean
approval to **32.1300 %**.

The reason is structural and generalises past this programme. Latency is **linear** in the number of
decision rights — a sum of per-body waits — while clean-approval probability decays **geometrically** in
it, as `q̄ᵏ` on the geometric mean pass rate. Conduct moves the terms; topology moves how the terms are
summed and leaves the 30.5235 % exactly where it was; count moves how many terms there are, and one of
those is an exponent. **Only the count moves both.** That is why *the list of approvers is the governance
design*, and why a review that examines each body's performance separately is examining the wrong
variable however carefully it does so.

### G.4.3 The fourteen offices the delivery agency does not run

Latency is only half of a split accountability. The other half is that the benefit is produced by the
**14 regional operating offices**, none of which reports to the delivery agency and each of which can
decline to deploy, decline to retrain, or keep the paper route open — a practical veto, in Domain 11's
terms a consent risk that is a property of position rather than disposition (KA 11.1.2).

If each region independently adopts in year one with probability **0.85**, the expected number adopting
is **11.90** and the probability that **all fourteen** do is **10.2770 %**. So a business case built on
universal adoption overstates the annual benefit by exactly the complement of the adoption probability —
on equal regional volumes, an assumption the realised case below shows to be itself optimistic:
**15.0000 %**, or **USD 11,427,000 a year**, against an honest **64,753,000**. The 10.2770 % was
computable before a line of code was written, and it is the same multiplication that G.1.2 found on
Meridian's readiness dashboard and G.3.1 found in Sable Plains' funnel — three programmes, one
operation. (The 15.0000 % is not that multiplication but its linear cousin: expected coverage is a *mean*
of independent adoptions, while it is the *product* that decides whether all fourteen arrive. Both belong
in the case, and they answer different questions.)

What actually happened: **9 of 14** regions switched in year one, handling **62 %** of national volume —
slightly less than their share of the offices, because the larger offices were the more cautious, which
is the ordinary direction of this error rather than bad luck. Realised saving **USD 47,231,600**;
shortfall **USD 28,948,400 a year**, measured against full digital conversion rather than against the
probabilistic case, so it is the whole gap and not the surprise in it. Set that against the entire
national readiness programme — an operating-model workshop and two follow-ups in each region, **180
hours** at a blended **USD 95** an hour, **USD 17,100** a region, **USD 239,400** nationally:

| Measure | Value |
|---|---|
| Shortfall ÷ national readiness programme | **120.9206×** |
| Breakeven share of national volume — `239,400 / 76,180,000` | **0.3143 %** |
| The same breakeven in volume | **8,170.6** assessments of 2,600,000 |
| Per region — `17,100 / 29.30` | **583.62** assessments of a regional average of 185,714.29 |

Domain 11's discipline is to lead with the breakeven and not the ratio, and here the two forms are
so far apart that the choice decides the meeting. **121 times** invites a sponsor to argue about
whether the regions would have adopted anyway. **0.3143 % of volume — 583.62 assessments in a region
that handles 185,714.29 —** invites nothing; it is a number no operational director will claim a
workshop cannot move. The national and per-region breakevens are the same figure computed two ways,
which is the check that the allocation is coherent.

Now count the programme, which is the finding this capstone exists for:

| | Count |
|---|---|
| Bodies holding an approval veto | **6** of the 20 parties that can stop it |
| Operating units holding a practical veto | **14** |
| **Parties able to stop or blunt the outcome** | **20** |
| Parties with the 76,180,000 in their own objectives | **1** — the policy ministry |
| Parties holding a *delivery or operating* lever **and** the outcome | **0** |

**Twenty vetoes and one accountability — and the one accountable party holds nothing but a veto.** The
last row is the whole diagnosis, and it turns on a distinction worth naming: **a veto is a negative
lever.** The ministry can refuse a policy change and cannot cause a region to adopt, cannot build the
service and cannot retrain an assessor; the delivery agency holds the build levers and is measured on
delivery rather than on benefit; the fourteen offices hold the adoption lever and are measured on
casework. So the outcome has an owner with no instrument, and every instrument has an owner with no
outcome. That is not a criticism of any of the twenty; each is discharging a duty it was given, and
several of the vetoes exist for good reasons that would survive any review. It is a statement about a
*design*, and the design's cost is now quantified twice and then confirmed after the event:
**29,625,807.53** on one decision's latency from the count of approval rights; **11,427,000 a year**
from adoption treated as certain rather than probable; and then, ex post, **28,948,400 a year** of
realised shortfall against a **239,400** intervention that was never funded — the realised 38 % of
volume standing where the probabilistic 15 % had been forecast.

Note the commensurability discipline G.1.1 insists on: **29,625,807.53 is a stock, banked once on one
decision; 11,427,000 and 28,948,400 are flows, recurring as long as the service runs** — and those two
flows are one flow, forecast and then realised, so the ledger has two entries and not three. They are not
addable, the comparison is a scale comparison, and stating that is what stops this table becoming the
reporting convenience Meridian's board was handed.

### G.4.4 The four questions the National Entitlement Service adds

1. **How many parties can stop this, and how many are measured on the outcome?** Twenty and one
   here. The ratio is the governance design; every coordination mechanism on top of it is a workaround.
2. **What does one decision right cost?** **7.2897 weeks, USD 10,679,426.92** on this programme — and
   lifting every body to the best first-pass rate any of them has achieved recovers only **88.1033 %**
   of one, while abolishing their returns altogether recovers **119.8712 %**. Compute this before
   agreeing to add a body, because afterwards the argument is about the body's merits rather than its
   price.
3. **Is the improvement programme aimed at the count or the conduct?** Halving the count was worth
   **3.1487 times** the observed behavioural ceiling and **2.3142 times** an unattainable one. A review
   that assesses each body separately cannot discover this, because the quantity is a property of the
   list.
4. **Does the benefit case assume adoption or estimate it?** At 0.85 a region, universal adoption has a
   **10.2770 %** probability and the assumption overstates the benefit by **15.0000 %**. The correction
   is a stated coverage in the case, and a readiness programme whose breakeven — **0.3143 % of
   volume** — is small enough to end the argument.
