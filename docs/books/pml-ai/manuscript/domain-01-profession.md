# Domain 1 — The Project Leadership Profession

## Why this domain exists

Every technique in this book — the networks of Domain 6, the earned value of Domain 7, the risk
quantification of Domain 8 — is instrumentation. This domain is about the person holding the
instruments: what a project leader is *for*, what they are answerable for, and how they think.
It establishes the delivery landscape and how leading temporary work differs from running a
steady-state function (KA 1.1); defines accountability precisely, because the word is used
loosely and the distinction decides who answers when things go wrong (KA 1.2); builds the two
habits of mind that separate a leader from a coordinator — systems thinking, and the discipline
of outputs versus outcomes versus benefits (KA 1.3); and grounds professional ethics and the
governed use of AI (KA 1.4). A reader who finishes only this domain should already reason like the
profession: *know what success actually means, know who owns it, and stay answerable for the
judgment even when a machine produced the analysis.*

**Learning objectives.** After this domain a candidate can: distinguish projects, programmes and
portfolios and explain what each optimises; **price the delivery verdict and the programme verdict
on the same programme and state why the two are not directly commensurable**; contrast project
leadership with operational leadership; explain why a temporary organisation makes trust and clarity
urgent; distinguish accountability from responsibility and apply the distinction to a delegation;
state the leader's obligations to sponsor, team, users and the public, and where they conflict;
**compute the value of escalating a problem early and the number of weeks of earliness that funds
the whole remedy**; explain the professional standard of care and show what exercising it looked
like on a specific priced decision; analyse a project as a system with feedback and delay;
**compute the transient trough that follows reinforcing a late team, derive its crossover horizon
and state the condition under which no trough exists**; distinguish outputs, outcomes and benefits
and demonstrate why output-based claims overstate value; **convert a benefit stream into a simple
payback, show that payback scales as the reciprocal of adoption, and invert a payback rule into the
adoption commitment it silently makes**; compute the value of time in benefit terms and the adoption
level below which paying for speed destroys value; describe the ethical obligations of the role;
apply the PCI responsible-AI principle to a realistic delivery decision; and **derive the breakeven
error probability that makes a verification proportionate, and name the class of artefact for which
that arithmetic must not be the deciding test.**

**The master case.** One programme runs through this domain and returns in Domains 2, 5, 15 and 16:
**Meridian Care Records**, a fictional public-health programme rolling a shared clinical-records
system out to **40 clinics** at an approved cost of **USD 2,400,000**. Its full-potential benefit is
**USD 979,200** a year, its realistic benefit **USD 685,440** a year at 70 % adoption, and its cost
of delay **USD 14,280 a week** — three figures derived in KA 1.3 and used, unchanged, by a dozen
later domains. This domain's job is to make them arithmetic rather than rhetorical, and to be
explicit about what each rests on.

---

## Knowledge Area 1.1 — Projects, programmes and portfolios

*Topics: 1.1.1 the delivery landscape · 1.1.2 project versus operational leadership · 1.1.3 the
temporary organisation.*

### 1.1.1 The delivery landscape

**Definitions.** A **project** is a temporary endeavour undertaken to produce a defined result. A
**programme** is a group of related projects and change activities managed together to achieve
**outcomes** no single project could deliver alone. A **portfolio** is the set of projects and
programmes an organisation chooses to fund, selected and balanced against strategy and capacity.
The three are not sizes; they are different *objects of management*:

| Level | Optimises | Primary success test | This book |
|---|---|---|---|
| **Project** | Delivery of a defined output to time, cost, quality | Was the thing delivered, fit for purpose? | Domains 4–13 |
| **Programme** | Coherent outcomes and benefits across components | Did the change actually land? | Domain 15 |
| **Portfolio** | Value and balance of the whole investment set | Are we doing the right things at all? | Domains 2, 15 |

The practical consequence is that **the same event can be a project success and a programme
failure**: Meridian delivering all 40 clinic installations on time is a project triumph and worth
nothing if clinicians do not use the system. The example below prices both verdicts, because a
leader who can only assert the distinction will lose the argument to whoever has a number.

**Worked example 1.1.1 — the two verdicts on one programme, priced.**

1. **Setup.** Meridian's completion report states the delivery verdict: **40 of 40** clinics
   installed, on time, **3 % under** the approved cost of **USD 2,400,000**. The programme verdict is
   taken separately, against benefit: the honest case assumed **70 %** adoption and **USD 685,440** a
   year (derived in 1.3.2); measured adoption is **40 %**, worth **USD 391,680** a year. Both
   statements are true and they are reported to the same board in the same month.
2. **Formula.** Delivery verdict = approved cost × underspend rate (a one-off capital variance).
   Programme verdict = potential benefit × (planned adoption − actual adoption), an **annual flow**.
   The comparison ratio = annual benefit shortfall ÷ capital underspend.
3. **Substitution.** Underspend `2,400,000 × 0.03`; shortfall `685,440 − 391,680`, equivalently
   `979,200 × (0.70 − 0.40)`; ratio `293,760 / 72,000`.
4. **Result.** Delivery verdict **+USD 72,000**, once. Programme verdict **(USD 293,760)** a year.
   The shortfall is **4.08 times** the underspend **in the first year alone**. Undiscounted over
   Domain 2's eight-year appraisal life the shortfall totals **USD 2,350,080**, or **97.92 %** of
   what the whole programme cost to build; discounted at Domain 2's 7 % (annuity factor
   **5.971299**) it is **USD 1,754,128.79**, or **73.09 %** of the approved cost.
5. **Interpretation.** Four readings, and the fourth is the one that keeps a leader honest.

   **The verdicts are not commensurable, and saying so is part of reporting them.** One is a
   stock (a capital variance, banked once); the other is a flow (annual benefit, recurring for as
   long as the service runs). Dividing one by the other is legitimate only as a *scale* comparison,
   which is what the 4.08 is for; converting the flow into a single figure requires a horizon and a
   discount rate, and those belong to the business case (Domain 2, KA 2.2.2), not to the delivery
   report. A leader who quotes the undiscounted 2,350,080 without saying it is undiscounted has
   overstated by **USD 595,951.21** against the discounted figure.

   **The delivery lever is small because it is bounded and the benefit lever is not.** To offset the
   first year of shortfall through cost alone, the programme would have had to underspend by
   **12.24 %** — four times what it achieved, on a rollout whose costs are mostly contracted. The
   asymmetry is structural, not particular to Meridian: on benefit-generating work the accessible
   range of cost variance is a few per cent of a one-off, while the accessible range of adoption is
   tens of per cent of a perpetuity. That is the quantitative reason KA 1.3 spends its attention on
   the outcome link.

   **The weekly form is the one that changes behaviour.** The 293,760 a year is
   **USD 6,120 a week** — which is exactly **12 clinics × 6 hours × USD 85**, the cost of delay of
   1.3.3 applied to the twelve clinics that are not using the system. A board that hears "we are
   293,760 light on the annual case" adjourns; a board that hears "the gap is costing 6,120 a week
   and has been for nine months" acts. Same number, different unit, different decision.

   **The delivery verdict was provisional and was later reversed.** Domain 16's closing account
   records a final capital outturn of **USD 2,514,000**, against the **USD 2,328,000** implied by the
   3 % underspend at installation — a swing of **USD 186,000**, most of it the recovery this domain's
   case study describes. The professional caution: an underspend reported before the outcome is
   measured is an *interim* figure, and a leader who banks it rhetorically ("we came in under") has
   made a claim the programme may still take back. A reviewer should ask, of any celebrated
   underspend, what remains to be spent to make the output work.

**How the discipline arrived at that distinction.** The vocabulary above is recent, and knowing why
it exists prevents a candidate from treating it as bureaucratic layering. Four shifts, each of which
added an obligation that had not previously been anyone's:

| Shift | What was codified | What it added to the leader's duty |
|---|---|---|
| **Technique** | Network scheduling, cost control and configuration control, developed on large capital, defence and aerospace programmes and generalised from there | Produce a plan that can be checked by someone else |
| **Process and profession** | Bodies of knowledge, competence frameworks and certification; a common lifecycle vocabulary | Practise to a stated standard, not to personal habit |
| **Governance and outcomes** | Sponsorship, decision rights, gates, benefits management; internationally, the ISO 21500 family — context and concepts (ISO 21500), project management guidance (ISO 21502), programme (ISO 21503), portfolio (ISO 21504) and governance (ISO 21505), alongside ISO 31000 for risk and ISO 10006 for quality in projects | Answer for whether the change landed, not only for whether the thing was built |
| **Adaptive and digital delivery** | Iterative and flow-based methods generalised out of software; data-centred delivery environments; and management-system standards for artificial intelligence itself, including ISO/IEC 42001 for AI management systems and ISO/IEC 23894 for AI risk | Choose a delivery approach and defend the choice; govern the tools that now draft the work |

Those standards are named here, not reproduced: each is described in this book's own words, and a
reader who needs the requirements themselves must obtain the standard. The relevant point for
Domain 1 is directional. **The discipline has moved its test of success outward** — from "was the
plan followed?" to "was the thing delivered?" to "did the change land, and can you show how you
knew?" Every one of those moves increased what the leader must be able to evidence, and none of
them removed an earlier obligation. That is why a modern project leader is answerable for a benefits
chain (1.3.2) that a scheduler of the technique era was never asked about, and why the fourth shift
puts a governed AI tool inside the same accountability frame rather than outside it (KA 1.4).

### 1.1.2 Project versus operational leadership

Operations optimise a repeating process; projects deliver a novel result once. The differences
that change a leader's behaviour:

| | Operational leadership | Project leadership |
|---|---|---|
| Work | Repeating, refined over time | Novel, done once |
| Team | Stable, known, yours | Assembled, borrowed, temporary |
| Authority | Usually line authority | Often influence without authority |
| Time | Continuous | Bounded, with an end that must be planned for |
| Learning curve | Amortised over many cycles | Paid once, mid-delivery |
| Failure mode | Drift, inefficiency | Late discovery, then irreversibility |

The last row is the one that shapes everything downstream. Operational problems are usually
recoverable at similar cost whenever they are found; project problems get monotonically more
expensive as commitments harden. That is why this book spends so much on early definition
(Domain 5), honest schedules (Domain 6) and early warning (Domains 7–8) — **in temporary work,
lead time is the cheapest resource a leader has, and it is spent whether used or not.**

That claim is quantified elsewhere in this volume rather than asserted twice, and a candidate should
know where: Domain 5 (KA 5.2.1) prices the rising cost of correcting a requirement defect by the
stage at which it is found; Domain 9 (KA 9.2.2) computes the containment economics that make a later
layer of checking an order of magnitude dearer than an earlier one; and Domain 3 (KA 3.3.1) values a
gate by netting the cost of the delay it imposes against the failure cost it prevents. Domain 1's
contribution is the leadership consequence, which is behavioural: because correction cost rises with
commitment, **the profession's most valuable act is moving information earlier**, and KA 1.2 prices
one instance of exactly that.

### 1.1.3 The temporary organisation

A project team is a temporary organisation: people arrive with divided loyalties, no shared
history, and no accumulated trust to draw on. Three consequences a leader must design for.
**Trust has no time to accrue naturally**, so it must be built deliberately and early — the team
charter (Domain 12) exists because the informal norms that operations grow over years must here be
stated in week one. **Clarity substitutes for familiarity**: roles, decision rights and
done-definitions have to be explicit precisely because nobody can infer them (Domains 3–5).
**Endings must be engineered**: a temporary organisation dissolves, and the knowledge, the
relationships and the operational readiness either transfer deliberately or evaporate (Domain 16).

### AI in this domain — first statement of the principle

AI now sits inside the delivery environment itself: drafting plans, summarising status, generating
risk lists, answering questions the team used to bring to a senior colleague. The suite's
governing principle, stated once here and applied in every later domain:

> **AI proposes; the professional verifies, decides and remains accountable.**

Three things it means concretely for a leader, before any technique in this book is applied.
**Verification is a duty, not a preference** — machine output entering a plan, report or decision
is checked by a named human against evidence. **Accountability is not transferable** — "the model
said so" is never a defence, because accountability cannot be delegated to software (KA 1.2 makes
the reason precise). **Disclosure is part of honesty** — material AI use in a deliverable is
stated, so colleagues can weigh it. Domain 14 builds the full governance architecture; this domain
establishes that the obligation attaches to the *person*, not the tool.

### Key terms — KA 1.1

| Term | Meaning |
|---|---|
| **Project** | Temporary endeavour producing a defined result. |
| **Programme** | Related projects managed together for outcomes no one project delivers. |
| **Portfolio** | The funded, balanced set of projects and programmes. |
| **Temporary organisation** | A team with no accumulated trust or shared norms; both must be designed. |
| **Irreversibility** | The rising cost of correction as commitments harden — projects' defining failure mode. |
| **Delivery verdict** | The project-level judgment: was the output delivered to time, cost and quality? A one-off variance. |
| **Programme verdict** | The programme-level judgment: did the outcome and its benefit materialise? An annual flow. |
| **Commensurability** | Whether two figures may be compared directly; a capital variance and an annual benefit may not, without a horizon and a rate. |
| **Responsible-AI principle** | AI proposes; the professional verifies, decides and remains accountable. |

### Sample MCQs — KA 1.1

**MCQ 1.1-A `[1.1.1 · Analysis]`** A programme's four projects each deliver on time and to
budget, but the intended service change does not materialise. The most accurate statement is:
- A. the programme succeeded, since all its projects succeeded
- B. the projects succeeded on their own test while the programme failed on its own test — outcomes are not the sum of outputs ✅
- C. the projects must therefore have failed
- D. programme success cannot be assessed until every project closes

*Rationale:* Projects are judged on delivered outputs, programmes on outcomes; the two tests are
different, which is exactly why the distinction exists. C rewrites history to preserve a single
verdict, and D denies the measurement programmes exist to make.

**MCQ 1.1-B `[1.1.2 · Analysis]`** Why does project leadership place more weight on early
definition than operational leadership does?
- A. project teams are less capable
- B. project problems grow monotonically more expensive as commitments harden, so lead time is the cheapest resource available ✅
- C. operations have no need for planning
- D. projects have larger budgets

*Rationale:* Irreversibility is the structural difference (1.1.2). A and D are unfounded; C
misstates operational practice, which plans continuously — just with the ability to correct
cheaply next cycle.

**MCQ 1.1-C `[1.1.3 · Recall]`** A team charter is written in week one primarily because:
- A. governance requires the document
- B. a temporary organisation has no accumulated norms, so what operations grow informally must here be made explicit ✅
- C. it replaces the need for a project plan
- D. it fixes the team's membership for the duration

*Rationale:* Explicit clarity substitutes for familiarity the team has had no time to build. A is
a by-product, C confuses artefacts, D is false — membership usually changes.

**MCQ 1.1-D `[1.1.1 · Evaluation]`** A programme delivers 3 % under an approved cost of 2,400,000
while adoption reaches 40 % against a planned 70 % on a full-potential benefit of 979,200 a year. The
soundest single sentence for the board is:
- A. the programme is 72,000 ahead, so the net position is positive
- B. the delivery result is +72,000 once, the benefit result is (293,760) a year — 4.08 times the underspend in year one alone — and the two are a stock and a flow, so no net figure should be quoted without a horizon and a rate ✅
- C. the programme has lost 221,760 net in its first year
- D. no comparison is possible, since the two figures measure different things

*Rationale:* Both verdicts must be reported, scaled against each other, and flagged as
non-commensurable (1.1.1). A nets a one-off against a perpetuity and keeps only the flattering half;
C performs the same illegitimate subtraction (`293,760 − 72,000`) and presents the remainder as a
result; D uses non-commensurability as an excuse not to compare at all, when the ratio is precisely
what makes the scale visible.

**MCQ 1.1-E `[1.1.1 · Comprehension]`** Which best describes what the governance-and-outcomes shift
added to the project leader's duty?
- A. a requirement to follow a certified process
- B. an obligation to answer for whether the intended change landed, in addition to whether the output was built ✅
- C. the replacement of scheduling technique by benefits management
- D. a transfer of delivery accountability to the sponsor

*Rationale:* Each shift added an obligation without removing an earlier one (1.1.1). A describes the
preceding process-and-profession shift; C is the common misreading — technique was retained, not
displaced; D inverts KA 1.2, where accountability for delivery stays with the leader.

### Self-check — KA 1.1

1. *Can a project succeed while its programme fails?* — Yes: outputs delivered, outcomes not
   realised. The tests differ (1.1.1).
2. *Name the defining failure mode of temporary work.* — Late discovery followed by
   irreversibility; correction cost rises as commitments harden.
3. *State the responsible-AI principle.* — AI proposes; the professional verifies, decides and
   remains accountable.
4. *Why may a 72,000 underspend not simply be netted against a 293,760 annual benefit shortfall?* —
   One is a stock and the other a flow; netting them needs a horizon and a discount rate, which
   belong to the business case (1.1.1, Domain 2 KA 2.2.2).
5. *Name the four shifts in the discipline and the obligation each added.* — Technique (a checkable
   plan), process and profession (practice to a stated standard), governance and outcomes (answer for
   the change landing), adaptive and digital delivery (defend the approach; govern the tools).

---

## Knowledge Area 1.2 — The project leader's accountability

*Topics: 1.2.1 accountability and responsibility · 1.2.2 the obligation set · 1.2.3 the
professional standard of care.*

### 1.2.1 Accountability and responsibility

**Definitions.** **Responsibility** is the obligation to *do* the work — it can be shared and
delegated. **Accountability** is the obligation to *answer* for the outcome — it sits with exactly
one named person and **cannot be delegated**, only reassigned by whoever holds the authority to do
so. A leader may hand a task to a specialist, an outsourcer or an AI tool; the answer for the
result stays where it was.

Two working rules follow.

**Delegation transfers responsibility, never accountability.** The leader who delegates keeps the
duty to specify, to verify at a proportionate depth, and to notice when the work is failing.
"I gave it to the vendor" is a description of responsibility, not a defence of accountability.

**One accountable person per outcome.** Where two people are accountable for the same thing,
nobody is: each defers, and the gap is discovered late. This is why Domain 3 builds explicit
decision rights and why RACI-style matrices insist on a single "A" per row.

The mechanism deserves stating precisely, because it is not merely untidy — it has a cost and a
signature. Two named holders of one outcome each face the same private calculation: acting costs
effort and political capital now, while the consequence of not acting is shared, deferred and
attributable to the other. Each therefore waits for evidence that the other will not act, and the
evidence takes the form of the outcome deteriorating. **Dual accountability does not slow the
response by a little; it converts the trigger from a measure into an incident.** Its signature in a
report pack is an outcome measure that is reported by both parties, owned by neither, and never
accompanied by a proposed action. On Meridian, the two holders were the delivery leader (installations)
and the clinical directorate (practice change), the un-owned outcome was adoption, and the incident
that finally triggered action was an external audit — twenty-six weeks after the measure first showed
it. Worked example 1.2.2 prices those twenty-six weeks.

**Accountability is not the same as legal liability.** This domain uses accountability in the
professional sense defined above. Legal liability — who must compensate whom, on what basis, and
subject to what limitation or exclusion — is set by the contract and by the law of the jurisdiction, and it can sit with a party who is not
professionally accountable, or be capped for a party who is. The two are routinely conflated, in both
directions: a contractual cap is offered as a reason not to answer, or a professional duty is treated
as if it created a payment obligation. Keep them separate in your own reasoning, and where a decision
turns on which is engaged — an indemnity, a professional-indemnity notification, a duty to warn a
third party — take the position from counsel for the applicable jurisdiction rather than from this
book (Domain 10, KA 10.4 for the contractual machinery).

The AI corollary is now immediate rather than novel. A tool has no standing to be accountable — it
cannot be asked to answer, cannot be sanctioned, cannot hold a duty. So *every* use of AI in
delivery leaves accountability exactly where it was: with the professional who used the output.
That is not a policy choice PCI made; it follows from what accountability is.

### 1.2.2 The obligation set

A project leader owes duties in four directions, and the skill is holding them together honestly
rather than pretending they never conflict:

| To | Owes | Typical conflict |
|---|---|---|
| **Sponsor / organisation** | Honest forecasts, stewardship of funds, no surprises | Pressure to report reassurance rather than status |
| **Team** | Clarity, safety, achievable asks, fair credit | Absorbing schedule pressure by quietly overloading people |
| **Users / customers** | A result that is genuinely fit for purpose | Delivering on time by descoping what users needed most |
| **Public / third parties** | Safety, legality, honest claims, environmental care | Cost pressure meeting a safety or compliance margin |

**The escalation duty.** When these conflict beyond the leader's authority to resolve, the
professional act is to escalate with options — not to absorb the conflict silently and hope. An
unescalated conflict becomes a late surprise, and late surprises are the failure mode of §1.1.2.

**The honesty asymmetry.** Bad news travels badly: it costs the messenger and helps the project.
A leader's most consequential cultural act is making early bad news *safe* — because a schedule
or cost problem reported five weeks early is usually recoverable (Domain 6's case study turns on
exactly this) and the same problem reported at the deadline is not. The asymmetry is worth making
arithmetic, because "escalate early" is advice everyone agrees with and almost nobody prices.

**Worked example 1.2.2 — what the timing of one escalation was worth.**

1. **Setup.** Meridian measures adoption monthly from go-live. At **week 13** the clinical lead's
   data shows **16** of 40 clinics in daily use — 40 % against the planned **70 %**, a shortfall of
   **12 clinics**, each worth `6 × 85 =` **USD 510** a week (1.3.2). Nobody escalates: the measure is
   reported by two parties and owned by neither (1.2.1). The shortfall surfaces at **week 39**, in an
   external audit. The remedy, once authorised, is clinical champions in the **11** lowest-adoption
   clinics at **USD 12,000** each; it lifts adoption to **68 %** — **27.2** clinics — closing
   **11.2** of the 12-clinic gap, and it costs the same whenever it is authorised. (Domain 16's
   registered clinic-count measure reads **67.50 %** and reconciles the two definitions at
   KA 16.4.1; the 11.2 clinics used here is therefore the more generous of the two readings.) What
   did the 26-week silence cost?
2. **Formula.** Shortfall run rate = gap clinics × weekly value per clinic. Recovered run rate =
   closed clinics × weekly value. Timing value of the escalation = weeks of earliness × recovered run
   rate. Weeks of earliness that fund the whole remedy = remedy cost ÷ recovered run rate.
3. **Substitution.** Run rate `12 × 510`; recovered `11.2 × 510`; timing value `26 × 5,712`; remedy
   `11 × 12,000`; funding earliness `132,000 / 5,712`.
4. **Result.** The shortfall ran at **USD 6,120 a week**; the remedy recovers **USD 5,712 a week**.
   Escalating at week 13 instead of week 39 was worth **USD 148,512** — against a remedy costing
   **USD 132,000**, so the *timing* of the escalation was worth **1.13 times** the entire remedy it
   would have paid for. **23.11 weeks** of earliness funds the remedy outright, so the 26 weeks
   available carried **2.89 weeks** of margin.
5. **Interpretation.** Five readings, and the last is the one a leader must not skip.

   **The most valuable act in the sequence cost nothing.** The remedy required a business case,
   funding and eleven appointments; the escalation required one person to send one message. On these
   figures the message was worth more than the programme it triggered. That is the honesty asymmetry
   in numbers, and it generalises: wherever a problem has a run rate, the value of early notice rises
   linearly with earliness while its cost stays fixed and small, so **the benefit-to-cost ratio of
   escalation is bounded only by how early the measure could have shown** — which is why the
   behaviour is worth engineering rather than exhorting.

   **The recovered run rate, not the shortfall run rate, is the honest multiplier.** The gap was 12
   clinics but the remedy closes 11.2, so valuing the earliness at 6,120 rather than 5,712 would
   overstate it by **USD 10,608** across the 26 weeks. The general error is pricing early warning at
   the size of the problem rather than at the size of the *fix that early warning brings forward*.
   Note the identity that falls out: 5,712 is exactly **40.00 %** of Meridian's USD 14,280 cost of
   delay, because 11.2 of 28 adopting clinics is 40 %. Early warning about part of a programme is
   worth the corresponding fraction of that programme's cost of delay, and no more.

   **Everything rests on the remedy working.** Had clinical champions failed to move adoption, the
   148,512 would be zero: earliness is worth exactly the value of the action it enables. The same
   dependency governs 1.3.3, and a leader who cannot name the action an escalation is meant to
   trigger does not yet have an escalation — only a complaint.

   **The sensitivity runs the reassuring way.** The calculation assumes a constant 12-clinic gap. In
   practice an unaddressed adoption gap tends to widen as early enthusiasm decays and paper
   workarounds harden, so a constant-gap model **understates** the timing value and 148,512 should be
   read as a floor. If instead the gap had been closing unaided, the timing value would fall — and the
   test of which world you are in is whether the measure has a trend, which is the reason a single
   monthly number without a trend line is not yet a measure.

   **The professional caution: this arithmetic must not be turned on the analyst.** The temptation on
   seeing 148,512 is to ask who failed to escalate. But the analyst reported the data on time; the
   failures were that no name owned the measure (1.2.1) and no route made raising it safe (1.2.2).
   The 148,512 is therefore the *leader's* number. Used to discipline a junior it destroys the next
   escalation and buys a second incident; used to fund an owned measure and a standing route to the
   board, it is the strongest business case available for the cultural work this KA describes.

> **Fig 1.2.1 — The cost of a silent measure.** Line chart. X-axis weeks from go-live, 0–52; y-axis
> cumulative benefit forgone, USD 0–180,000. A crimson line, "escalated at the audit", is flat to week
> 13 — marked with a crimson dot and "measure first shows 16 of 40 clinics" — then rises at
> **USD 6,120 a week** to **USD 159,120** at week 39, where a vertical ink rule is annotated "external
> audit — week 39"; beyond it the slope falls to **USD 408 a week**, the residual 0.8-clinic gap the
> remedy does not close, ending at **USD 164,424** at week 52. A brand-blue line, "escalated when the
> measure showed", rises at 408 a week from week 13 to **USD 15,912** at week 52. The vertical gap
> between them at week 52 is labelled **"USD 148,512 — the value of the timing alone"**. A horizontal
> dashed ink line at **USD 132,000**, labelled "cost of the remedy", is met by the growing gap between
> the two lines at **week 36.11** — **23.11** weeks after the measure first showed — annotated
> "earliness that funds the whole remedy: 23.11 weeks, of the 26 available". Source: PCI original.
> Alt text: a crimson line of cumulative benefit forgone climbing steeply from week thirteen to a week
> thirty-nine audit and then flattening, against a low blue line for the same remedy authorised
> twenty-six weeks earlier, the gap between them marked as the value of the timing.

### 1.2.3 The professional standard of care

Professionals are judged not on outcomes alone but on whether they exercised the **care a
competent practitioner would** in the circumstances. Practically, four things constitute care in
this discipline: **method appropriate to the stakes** (a two-week internal task and a safety-
critical programme do not warrant the same rigour); **evidence proportionate to the claim** (a
forecast presented to a board carries its basis — Domain 7's estimate basis sheet); **records
that let others check the work** (decision logs, assumption registers, audit trails); and
**candour about uncertainty** (ranges, not false single numbers). A leader who did all four and
still missed can defend the work. One who hit the date by luck and cannot show any of them has no
defence available when the next project does not get lucky.

**What care looks like on one real decision.** Abstract lists of this kind are easy to agree with and
hard to apply, so take the acceleration decision worked in 1.3.3 — spend USD 60,000 to arrive eight
weeks sooner — and ask what each constituent required of the leader who took it.

| Constituent | What it required here | What its absence would have looked like |
|---|---|---|
| **Method proportionate to the stakes** | A 60,000 decision does not warrant a simulation; it warrants a run rate, a breakeven and one sensitivity — the three lines of 1.3.3 | "The team thinks we can pull it in and it seems worth it" |
| **Evidence proportionate to the claim** | The USD 14,280 a week traced to 28 adopting clinics, 6 hours, USD 85 — every factor with a source, and adoption identified as the one factor not yet observed | A weekly benefit figure quoted with no derivation, which cannot be challenged and therefore cannot be trusted |
| **Records others can check** | The decision recorded with its inputs, its breakeven of 4.20 weeks, and the adoption assumption named as the thing that would invalidate it | A minute reading "acceleration approved", which tells a later reviewer nothing about what was known |
| **Candour about uncertainty** | The 50/70/90 % adoption range carried into the paper, and the statement that below **36.76 %** adoption the spend destroys value | A single-point benefit and a confident recommendation |

The leader who did those four things and then watched adoption stall at 40 % has a defensible file:
the decision was right on the information available, the invalidating condition was named in advance,
and the record shows it. That is the whole practical content of the standard of care — **it is not a
promise about outcomes, it is a discipline about evidence, and it is built before the outcome is
known, because it cannot be built afterwards.**

Two cautions. First, care is judged **in the circumstances**, which cuts both ways: the same
one-page analysis that constitutes care on a 60,000 decision would be negligent on a safety-related
one, and a leader who applies a uniform standard is wrong in one direction or the other (the same
proportionality argument as KA 1.4.3). Second, where care is **codified** — regulated construction,
pharmaceuticals, aviation, nuclear, financial services — the required records, competencies and
sign-offs are set by that regime and by the relevant professional-body rules, and what counts as
adequate there is a question for the regime and, where liability is engaged, for counsel in the
applicable jurisdiction. This book states the professional discipline; it does not state anyone's
legal duty.

### Key terms — KA 1.2

| Term | Meaning |
|---|---|
| **Responsibility** | The obligation to do; shareable and delegable. |
| **Accountability** | The obligation to answer; single-holder, non-delegable. |
| **Escalation duty** | The obligation to raise, with options, a conflict beyond one's authority. |
| **Honesty asymmetry** | Bad news costs the messenger and helps the project; leaders must make it safe. |
| **Timing value of an escalation** | Weeks of earliness × the run rate the remedy recovers — what the *timing* alone is worth, separate from the remedy. |
| **Silent measure** | An outcome measure reported by two parties, owned by neither, and never accompanied by a proposed action. |
| **Legal liability** | Who must compensate whom, set by contract and jurisdiction; distinct from professional accountability and capable of sitting elsewhere. |
| **Standard of care** | The care a competent practitioner would exercise in the circumstances. |

### Sample MCQs — KA 1.2

**MCQ 1.2-A `[1.2.1 · Application]`** A leader outsources the migration design to a specialist
vendor under a fixed-price contract. Accountability for the migration's success now rests with:
- A. the vendor, under the contract
- B. the leader, who retains the duty to specify, verify proportionately and act on failure ✅
- C. jointly and equally with the vendor
- D. the sponsor, who approved the contract

*Rationale:* The contract transfers responsibility (and commercial risk); accountability is
non-delegable. C describes the arrangement that guarantees nobody answers (1.2.1); D confuses
approving a route with owning the delivery.

**MCQ 1.2-B `[1.2.1 · Analysis]`** An AI planning tool generates a schedule that the leader
submits unchanged; it contains an infeasible dependency that causes a two-month slip. The
accountability position is:
- A. shared with the tool's vendor, who supplied a defective product
- B. the leader's: a tool cannot answer for an outcome, and submitting unverified output is a failure of the verification duty ✅
- C. nobody's — the failure was technological
- D. the team's, for not catching it

*Rationale:* Accountability requires a party who can be asked to answer, so it cannot attach to
software (1.2.1); the specific lapse is verification (1.1's principle). Vendor recourse (A) is a
commercial question and does not relocate the professional answer; D inverts delegation.

**MCQ 1.2-C `[1.2.3 · Analysis]`** A leader's project overran despite a documented method,
ranged forecasts, a decision log and escalated risks. Under the standard of care this is:
- A. professional negligence, because the project overran
- B. defensible practice — care is judged on the exercise of competent method, not on outcome alone ✅
- C. irrelevant, since standards of care apply only to regulated professions
- D. defensible only if the overrun was under 10 %

*Rationale:* The standard is conduct-based (1.2.3). A collapses care into outcome; C is false in
substance — the expectation attaches to professional practice generally; D invents a threshold.

**MCQ 1.2-D `[1.2.2 · Application]`** An adoption shortfall of 12 clinics, each worth USD 510 a
week, is visible at week 13 but escalated at week 39. The remedy costs USD 132,000 and recovers 11.2
of the 12 clinics. The value of the 26 weeks of earliness that were not taken is:
- A. USD 159,120
- B. USD 148,512 ✅
- C. USD 16,512
- D. USD 132,000

*Rationale:* Earliness is worth the run rate the remedy *recovers*: `26 × 11.2 × 510 = 148,512`
(1.2.2). A values it at the full 12-clinic gap (`26 × 6,120`), crediting the remedy with a recovery it
does not achieve; C deducts the remedy cost, which is incurred either way and so does not belong in a
timing comparison; D is the remedy cost itself.

**MCQ 1.2-E `[1.2.1 · Analysis]`** A programme's adoption measure appears in both the delivery
report and the clinical directorate's report, with no proposed action in either. The most accurate
diagnosis is:
- A. duplicated reporting, which should be rationalised for efficiency
- B. two accountable names and therefore none — the measure has no owner, and its trigger has become an incident rather than a threshold ✅
- C. adequate governance, since two parties are monitoring the measure
- D. a data-quality problem in the reporting pipeline

*Rationale:* Dual accountability converts the trigger from a measure into an incident, and the
signature is exactly this: reported twice, owned once by nobody, never with an action (1.2.1). A
treats the symptom as an administrative annoyance; C mistakes visibility for ownership; D relocates a
governance defect to the tooling.

### Self-check — KA 1.2

1. *What can be delegated and what cannot?* — Responsibility can; accountability cannot.
2. *Why can accountability never attach to an AI tool?* — Accountability is the obligation to
   answer; a tool cannot be asked to answer, sanctioned, or hold a duty.
3. *Name the four constituents of care in this discipline.* — Proportionate method, evidence
   matching the claim, checkable records, candour about uncertainty.
4. *What is the signature of dual accountability in a report pack?* — An outcome measure reported by
   both parties, owned by neither, never accompanied by a proposed action (1.2.1).
5. *At what run rate should the value of early warning be priced?* — The run rate the remedy
   **recovers**, not the size of the problem: 5,712 a week, not 6,120 (1.2.2).
6. *Is professional accountability the same as legal liability?* — No: liability is set by contract
   and jurisdiction and may sit elsewhere or be capped; take that position from counsel (1.2.1).

---

## Knowledge Area 1.3 — Systems thinking and value

*Topics: 1.3.1 projects as systems · 1.3.2 outputs, outcomes, benefits, value · 1.3.3 leading
under uncertainty.*

### 1.3.1 Projects as systems

A project is a system: parts that interact, with **feedback** and **delay** between cause and
visible effect. Three systems behaviours a leader must recognise, because each defeats
common-sense management.

**Delay hides consequence.** Adding people to a late project consumes the productivity of those
already on it (onboarding, communication paths) before adding any, so the intervention looks
harmful before it looks helpful — and leaders often reverse it in the interval.

**Local optimisation degrades the whole.** A discipline lead maximising their own efficiency
levels their team's workload smoothly and delivers the interface documents late, stalling three
other teams. Every function can be efficient while the project is slow — the constraint logic of
Domain 6 (6.A.1) is the schedule expression of this.

**Pressure moves, it does not vanish.** Squeeze the schedule and the pressure emerges as quality
defects (Domain 9), scope disputes (Domain 5), team attrition (Domain 12) or supplier claims
(Domain 10). A leader's question on receiving any "we absorbed it" report is *where did it go?*

The first of those three is the one that most often causes a leader to undo their own correct
decision, and it is priced in KA 1.3.3 (Worked example 1.3.3b), where the cost of delay it needs has
been derived: adding people to late work produces a measurable **transient** in which progress is
worse than it would have been, and the arithmetic gives the week at which the intervention overtakes
doing nothing. Domain 12 (KA 12.2.2) treats the *steady-state* counterpart — the coordination cost of
team size, which has a hard capacity ceiling — and the two are different mechanisms with different
remedies, so neither substitutes for the other: shortening a ramp does nothing about a mesh, and
structuring a team does nothing about a ramp.

### 1.3.2 Outputs, outcomes, benefits and value

**Definitions.** An **output** is what the project delivers (a system installed, a road built).
An **outcome** is the change in how things work as a result (clinicians actually using the
records; journeys actually faster). A **benefit** is the measurable improvement that the outcome
produces (hours released, cost avoided, harm reduced). **Value** is benefit weighed against cost
and risk, for a stated stakeholder. The chain has a hard property: **each link can fail
independently**, so an output claimed as a benefit is an overstatement by construction.

**Worked example 1.3.2 — what Meridian actually delivers.**

1. **Setup.** Meridian Care Records installs the shared records system in **40 clinics**
   (the output). Realistic adoption at 12 months is **70 %** of clinics using it in daily
   practice. In an adopting clinic the system releases **6 clinician-hours per week**, valued at
   **USD 85 per hour**, over a **48-week** operating year.
2. **Formula.** Annual benefit = clinics × adoption × hours/week × rate × weeks. Contrast with
   the output-based claim, which counts all 40 clinics.
3. **Substitution.** Adopting clinics `40 × 0.70 = 28`; benefit
   `28 × 6 × 85 × 48`. Output-based claim `40 × 6 × 85 × 48`.
4. **Result.** Benefit **USD 685,440 per year** (≈ SAR 2.57 million indicatively). The
   output-based claim gives **USD 979,200** — an overstatement of **USD 293,760**, exactly
   **30.0 %**, which is the non-adoption rate reappearing as fictitious value.
5. **Interpretation.** The 30 % gap is not an estimating error; it is the *outcome* link being
   skipped. This is why a benefits register (Domain 16) tracks adoption as a measure in its own
   right, and why "40 clinics live" is a milestone, not a benefit. Five things follow.

   **The overstatement is exactly the non-adoption rate, whatever the other numbers are.** Because
   both figures share the hours, the rate and the weeks, the ratio of the honest benefit to the
   output-based claim is the adoption term alone: `685,440 / 979,200 = 0.70`. So an output-based
   claim overstates by `1 − a` — here 30.0 % — **irrespective of how many hours are released, what
   they are worth, or how long the operating year is.** That is a useful thing to know in a meeting:
   you do not need the model to say how wrong a claim is, only the adoption assumption it omitted.
   The corollary is a reviewer's shortcut — if a benefits case contains no adoption term, its
   overstatement is already known to be the non-adoption rate, and the discussion can move straight
   to what that rate is.

   **Sensitivity is the leader's real lever.** At 50 % adoption the benefit is USD 489,600; at 90 %,
   USD 881,280. **A programme of this shape creates more value by moving adoption than by moving its
   installation date** — which should change where the leader spends personal attention (Domain 11's
   engagement work, not Domain 6's schedule compression). Per clinic the same arithmetic gives
   `979,200 / 40 =` **USD 24,480** of full potential and **USD 17,136** realistic (the per-clinic
   figures Domain 5 uses to price scope decisions at KA 5.1.3), so a single clinic won or lost is
   worth more than most of the change requests a rollout of this kind argues about.

   **The definition of adoption is where the number is actually decided.** "Using it in daily
   practice" has to be operationalised — a proportion of encounters recorded in the system, a
   threshold of active clinicians, a fortnight without reversion to paper — and reasonable
   definitions differ. On these figures a **5-percentage-point** difference in where the threshold
   is drawn is worth `0.05 × 979,200 =` **USD 48,960** a year. So the definition belongs in the
   benefits register with the measure, agreed before the first report, or the programme will spend
   its second year arguing about the denominator (Domain 16, KA 16.4.1).

   **Released hours are capacity, not cash — and the distinction is a professional obligation, not a
   quibble.** Valuing 6 clinician-hours at USD 85 states what those hours are *worth if they are
   used for something*. They become a cash saving only if headcount or agency spend falls, and a
   service improvement only if the hours go to patients; if neither happens they are absorbed and the
   benefit is real to nobody. A benefits case must therefore say which of the two it is claiming and
   who is accountable for making it happen — the same distinction Domain 2 draws between cost
   *removed* and cost *moved* (KA 2.1.2). A leader who lets a released-hours figure be read as cash
   has authorised a claim the finance function will later, correctly, refuse to recognise.

   **What would falsify the whole chain.** Three things, in order of likelihood: adoption measured on
   a definition nobody agreed (above); hours released estimated rather than sampled — Domain 16's
   measurement found 5.4 hours, not 6.0, which alone removes **USD 66,096** a year at that domain's
   measured steady adoption of **67.50 %** (`163,200 × 0.675 × 0.6`); and an attribution problem,
   where hours would have been released anyway by an unrelated change, which is why a comparison
   cohort matters (Domain 16, KA 16.4.3). None of these is a reason
   to omit the arithmetic. They are the reasons to publish it with its assumptions attached, so that
   the argument is about the assumptions rather than about the conclusion.

> **Fig 1.3.1 — The value chain, and where it leaks.** Left-to-right chain diagram with four
> linked blocks: OUTPUT "40 clinics installed" → OUTCOME "28 clinics using it (70 % adoption)" →
> BENEFIT "USD 685,440 per year released" → VALUE "benefit weighed against cost and risk". Beneath
> the output→outcome link, a crimson leak arrow annotated "30 % — USD 293,760 of claimed value
> that never existed". A caption note: each link can fail independently. Source: PCI original.
> Alt text: a four-stage chain from installed outputs through adoption and released benefit to
> value, with a leak arrow marking the thirty per cent lost between output and outcome.

**The fourth link is the one leaders skip.** Benefit is not value: value is benefit weighed against
cost and risk, for a stated stakeholder. A programme with a genuine benefit and a bad ratio of
benefit to cost is a bad investment, and the leader who reports only the benefit has not yet said
anything about value. The simplest instrument that closes the link is **simple payback** — the number
of periods of benefit needed to repay the cost — and it repays study here because it is the instrument
boards actually use, it makes the adoption dependency arithmetic, and its limitations are instructive.

**Worked example 1.3.2b — payback, and the adoption commitment a payback rule silently makes.**

1. **Setup.** Meridian's approved cost is **USD 2,400,000**. Full potential benefit is
   **USD 979,200** a year; the honest benefit at 70 % adoption is **USD 685,440**. The sponsoring
   organisation applies a **three-year simple payback** rule to discretionary investment. Compute the
   payback on both benefit figures, and find the adoption the rule actually requires.
2. **Formula.** Simple payback = cost ÷ annual benefit, where annual benefit = potential × adoption.
   Required adoption for a payback target `T` = cost ÷ (`T` × potential).
3. **Substitution.** Output-based `2,400,000 / 979,200`; honest `2,400,000 / 685,440`; required
   annual benefit `2,400,000 / 3 = 800,000`; required adoption `800,000 / 979,200`.
4. **Result.** Payback on the output-based claim **2.4510 years**; on the honest benefit
   **3.5014 years**. The ratio of the two is **1.428571**, which is exactly `1 / 0.70`. A three-year
   payback rule requires **USD 800,000** a year, and therefore **81.70 %** adoption — **11.70
   percentage points**, or **4.68 clinics**, beyond what the case assumed.
5. **Interpretation.** Five readings, and the fourth is the one that changes how a leader reads a
   board's own rules.

   **Payback scales as the reciprocal of adoption, exactly.** `payback = cost / (potential × a)`, so
   halving adoption doubles payback and the 1.428571 ratio above is `1/0.70` with no residue. This is
   worth holding as an identity because it converts a governance question into an arithmetic one:
   any output-based payback is understated by the factor `1/a`, and a leader who knows the adoption
   assumption can correct a payback in their head.

   **A payback rule is an adoption commitment in disguise.** Nobody in the approval meeting said "we
   undertake to reach 81.70 % adoption"; the three-year rule said it for them, and nobody noticed
   because the rule was expressed in years and the commitment is in percentage points. **Inverting a
   hurdle into the operational condition it implies is one of the highest-value habits in this
   book** — it is the same move Domain 2 makes when it reports breakeven adoption instead of NPV
   (KA 2.2.2), and Domain 3 makes when it converts a delegation threshold into an escalation volume
   (KA 3.2.3). The professional act on seeing a 3.5014-year payback against a three-year rule is not
   to argue about the rule but to say what the rule demands and ask who owns delivering it.

   **The marginal value of adoption is largest where adoption is worst, by a factor of 3.60.**
   Because payback is hyperbolic in adoption, moving from 40 % to 50 % cuts payback by **1.2255
   years** while moving from 80 % to 90 % cuts it by **0.3404 years** — the first ten points are worth
   **3.60 times** the last ten. This contradicts the instinct to chase the final laggards for
   completeness, and it is the arithmetic behind the recovery in this domain's case study, which
   funded champions in the **eleven lowest-adoption** clinics rather than pushing the leaders higher.
   The same reasoning appears in Domain 11's engagement prioritisation: effort goes where the curve is
   steep.

   **Simple payback is the wrong instrument for the decision and the right one for the
   conversation.** It ignores the time profile of benefits, the cost of capital, everything after the
   payback point, and — on Meridian — the operating cost the case omitted. Including Domain 16's
   measured run cost of **USD 108,000** a year, net benefit falls to **USD 577,440** and payback
   lengthens to **4.1563 years**, an addition of **0.6549 years** from a line that was simply absent.
   Domain 2 does the decision properly, discounted over eight years at 7 %, and reports a breakeven
   sustained adoption of **41.05 %**. Note what the two breakevens are for: **81.70 % is what a
   three-year payback rule demands; 41.05 % is what creating value demands.** The **40.65-point** gap
   between them is the price of using a payback rule as a proxy for value — and a programme killed at
   70 % adoption for failing a payback rule would have been killed while creating value, which is a
   governance failure and not an arithmetic one.

   **The caution.** Every figure here is undiscounted, in nominal money, and takes the approved cost
   as complete. Do not present a simple payback to a board without saying which of those three
   simplifications is doing the most work; on Meridian it was the third. Where the investment is
   material, the discounted appraisal is not an optional refinement: Domain 2 is where it belongs.

> **Fig 1.3.2 — Payback is hyperbolic in adoption, so the first points are worth the most.** Line
> chart. X-axis adoption 30–100 %; y-axis simple payback in years, 0–9. A brand-blue curve plots
> `2,400,000 / (979,200 × a)`, falling from **8.1699** years at 30 % through **6.1275** at 40 %,
> **4.9020** at 50 %, **3.5014** at 70 %, **3.0637** at 80 % and **2.7233** at 90 % to **2.4510** at
> 100 %. A horizontal dashed crimson line at 3 years is labelled "three-year payback rule"; it meets
> the curve at **81.70 %**, marked with a crimson dot and annotated "the adoption the rule silently
> requires". A vertical slate rule at 70 % is annotated "the case's assumption — payback 3.5014
> years", and a second vertical rule at **41.05 %** is annotated "Domain 2's breakeven adoption for
> value creation", with the span between 41.05 % and 81.70 % bracketed and labelled "40.65 points —
> the cost of using a payback rule as a proxy for value". Two marginal-slope notes sit beside the
> curve: "40 → 50 %: **−1.2255 yr**" at the steep end and "80 → 90 %: **−0.3404 yr** (3.60× flatter)"
> at the shallow end. Source: PCI original. Alt text: a falling hyperbolic curve of payback against
> adoption, steep at low adoption and nearly flat at high adoption, crossing a three-year rule line at
> eighty-two per cent.

### 1.3.3 Leading under uncertainty

Uncertainty is the condition, not an exception to be eliminated before acting. Three disciplines
constitute leading well inside it.

**Decide at the right moment.** Deciding early buys momentum and forecloses options; deciding late
preserves options and costs time. The professional question is not "do we have enough
information?" but *what is the last responsible moment, and what does waiting cost?*

**Price the time.** In benefit-generating work, delay has an arithmetic cost.

**Worked example 1.3.3 — what a week of Meridian delay costs.**

1. **Setup.** Meridian's benefit runs at the KA 1.3.2 rate once adopted. The team can compress
   the rollout by **8 weeks** at an incremental cost of **USD 60,000**. Should the leader buy it?
2. **Formula.** Benefit per week = adopting clinics × hours × rate. Net = (weeks × benefit per
   week) − cost. Breakeven weeks = cost ÷ benefit per week.
3. **Substitution.** Per week `28 × 6 × 85 = 14,280`; eight weeks `114,240`; net
   `114,240 − 60,000`.
4. **Result.** Benefit per week **USD 14,280**; eight weeks **USD 114,240**; **net +USD 54,240**.
   Breakeven at **4.20 weeks** — beyond which acceleration pays.
5. **Interpretation.** The decision is not "is 60,000 a lot?" but "does it buy more than it
   costs?" — and here it clearly does, provided adoption materialises. Five further readings, and the
   second is the one that separates a priced decision from a lucky one.

   **The breakeven is the sentence to take to the sponsor, not the net.** A net of +54,240 invites an
   argument about the inputs; a breakeven of 4.20 weeks invites a question that can be answered —
   *can we really pull in more than 4.20 weeks?* The identity is `breakeven weeks = cost ÷ cost of
   delay`, which is scale-free: it does not care about the size of the programme, only about the price
   of a week. The same identity governs Domain 3's gate durations and Domain 15's escalation latency,
   which is why the cost of delay is registered once and reused rather than re-derived.

   **Every figure rests on the 70 % adoption of 1.3.2, and the sensitivity is severe.** Spending
   60,000 to arrive sooner at an unadopted system buys nothing. Precisely: the acceleration pays for
   itself over eight weeks at any adoption above **36.76 %** — because 60,000 over 8 weeks needs
   **USD 7,500** a week, which is **14.7059** clinics at USD 510 each. That is comfortably below the
   planned 70 %, so the decision is robust *in direction*. But it is fragile in *magnitude*: at the
   adoption Meridian actually achieved, 40 %, the cost of delay is `16 × 510 =` **USD 8,160** a week,
   the breakeven stretches to **7.3529 weeks** of the 8 available, and the net collapses from
   **USD 54,240** to **USD 5,280** — a **90.3 %** reduction, from a variable nobody in the meeting was
   watching. **A decision can be correct and still have almost no margin, and the leader is the person
   who has to know which.**

   **The maximum justified spend follows immediately and is worth stating in the paper.** Eight weeks
   at 14,280 is **USD 114,240**, so any compression package priced above that destroys value however
   attractive it looks; at the measured 40 % adoption the ceiling falls to `8 × 8,160 =` **USD 65,280**.
   Quoting the ceiling rather than only the recommendation is what stops a 60,000 proposal becoming a
   120,000 proposal during procurement without anyone re-testing it.

   **Sequencing engagement before acceleration is not a preference; it is what the numbers say.**
   Adoption work raises both the benefit *and* the value of every subsequent timing decision, because
   the cost of delay is proportional to adoption. Acceleration bought before adoption is secured is
   leveraged on an unverified assumption; adoption secured first makes the acceleration worth more.

   **What breaks it.** The eight weeks must be genuinely available on the critical path — compression
   applied to non-critical work buys nothing at all, which Domain 6 (KA 6.4) computes properly, and a
   compression estimate produced by the same team that owns the date should be treated as an advocacy
   figure until tested. The 60,000 must be the *whole* incremental cost, including any overtime
   premium, additional supervision and the rework that rushed installation tends to generate
   (Domain 9, KA 9.2.3 prices the rework-capacity effect). And the benefit must actually begin when
   the output arrives: if go-live is gated by a training window or a licence date, arriving early buys
   nothing but idle readiness. A reviewer's single best question on any acceleration case is *what
   starts earning on the earlier date, and who has confirmed it can?*

**The other way to buy time is people, and it does not price the same way.** Acceleration bought with
money answered cleanly in the example above. The same objective pursued by adding staff behaves
differently, because capacity added mid-delivery arrives with a transient attached — which is 1.3.1's
"delay hides consequence" property in its most expensive everyday form. The two decisions look alike
to a sponsor and reach opposite verdicts, which is the reason to work both.

**Worked example 1.3.3b — the reinforcement trough, and when buying time with people is a mistake.**

1. **Setup.** Eight clinics remain in Meridian's rollout. The crew of **6** deployment specialists
   completes **0.1** clinics each per week, so **0.6** a week in total. The programme is late, and the
   leader can add **3** more specialists at **USD 4,200** per specialist-week. Newcomers take a
   **4-week** ramp, during which each works at **25 %** of full productivity and absorbs **50 %** of
   an existing specialist's time in supervision. The cost of delay is the **USD 14,280** a week
   derived above.
   Both the ramp length and the supervision load are **locally calibrated planning figures, not
   constants** — the same discipline Domain 12 applies to its coordination parameter.
2. **Formula.** Ramp-period rate = base rate − (newcomers × supervision × per-head rate) + (newcomers
   × ramp productivity × per-head rate). Post-ramp rate = base rate + newcomers × per-head rate.
   Duration = ramp weeks + (remaining work − ramp-period output) ÷ post-ramp rate. Crossover horizon =
   ramp weeks × (1 + deficit rate ÷ gain rate). Net = weeks saved × cost of delay − incremental
   specialist-weeks × rate.
3. **Substitution.** Ramp rate `0.6 − (3 × 0.5 × 0.1) + (3 × 0.25 × 0.1) = 0.6 − 0.15 + 0.075`;
   post-ramp `0.6 + 0.3`; baseline duration `8 / 0.6`; new duration `4 + (8 − 4 × 0.525) / 0.9`;
   specialist-weeks `9 × 10.5556` against `6 × 13.3333`.
4. **Result.**

   | | Crew of 6 | Crew of 9 |
   |---|---|---|
   | Delivery rate, weeks 1–4 | 0.6 clinics/week | **0.525** clinics/week |
   | Delivery rate thereafter | 0.6 | **0.9** |
   | Cumulative at week 4 | 2.4 clinics | **2.1** clinics |
   | Duration for the last 8 clinics | **13.3333** weeks | **10.5556** weeks |
   | Specialist-weeks consumed | 80.0 | **95.0** |

   Weeks saved **2.7778**, worth **USD 39,666.67** at the cost of delay. Incremental effort **15.0**
   specialist-weeks, costing **USD 63,000**. **Net (USD 23,333.33)** — the reinforcement loses money.
   The crossover horizon is `4 × (1 + 0.075/0.3) =` **5.00 weeks**, and at week 4 the reinforced crew
   is **0.3 clinics — 12.5 % — behind** where the original crew would have been.
5. **Interpretation.** Four results, of which the third is the one that decides the case and the
   fourth is the one that keeps the model honest.

   **The trough is real, measurable and misread.** For four weeks every progress metric says the
   decision was wrong, by 12.5 % at its worst. A leader who reverses in the trough pays the whole
   supervision cost, receives none of the added capacity, and ends later than either pure option —
   the single most expensive available move. The counter-measure is procedural, not stoical: **state
   the crossover week when the decision is taken**, and put it in the record, so the trough is a
   forecast being met rather than a surprise being explained.

   **The crossover horizon is computable in advance, and it is the decision rule.** `R × (1 + d/g)`
   needs only the ramp length, the net deficit rate during the ramp and the gain rate after it — three
   figures any team can state. Here it is 5.00 weeks: with more than five weeks of work left,
   reinforcement finishes earlier; with less, it finishes later, and no amount of commitment changes
   that. The identity also tells you which lever matters. Halving the ramp to two weeks moves the
   crossover to **2.50** weeks — a gain of 2.50; halving the supervision load instead removes the
   deficit altogether (`d = 0`) and moves the crossover to **4.00** weeks, the ramp length itself — a
   gain of 1.00. **Shortening the ramp is worth two and a half times as much as easing supervision**,
   which is an argument for prepared onboarding material rather than for asking mentors to give
   newcomers less of their time.

   **Being earlier is not the same as being better off.** The 2.7778 weeks saved are worth 39,666.67;
   the 15 extra specialist-weeks cost 63,000; the reinforcement therefore destroys **USD 23,333.33**
   of value while genuinely delivering sooner. It becomes worthwhile only above a cost of delay of
   **USD 22,680 a week** — and Meridian's maximum conceivable cost of delay, at 100 % adoption, is
   `40 × 6 × 85 =` **USD 20,400**, which is **11.18 % short** of the breakeven. **No adoption level
   makes this reinforcement pay on delay grounds.** So if the crew is added anyway, it must be for a
   reason that is stated and different — a statutory date, a contractual liquidated sum, the crew being
   needed for the next tranche regardless — and that reason belongs in the decision record (1.2.3).
   The alternative lever is the one that is cheap: the same 2.7778 weeks bought by shortening the
   ramp costs nothing like 63,000. The breakeven specialist-week rate is **USD 2,644.44**; at
   Meridian's 4,200 the arithmetic simply does not clear.

   **What breaks the model.** It assumes the remaining work is divisible and rate-limited by
   specialist capacity — not by a fixed clinic-by-clinic sequence, a licence window or a single
   scarce approver, any of which makes added capacity worth nothing at all (Domain 6, KA 6.A.1 on the
   constraint). It assumes the newcomers are competent in the work and merely unfamiliar with this
   programme; recruiting into an unfamiliar *discipline* has a longer ramp and a larger supervision
   load, both of which push the crossover out. And it prices only duration and effort: quality
   consequences of a rushed ramp sit in Domain 9's containment economics, and morale consequences in
   Domain 12. The invariant worth remembering, because it is parameter-free: **a trough exists
   precisely when the supervision load a newcomer imposes exceeds the newcomer's own ramp
   productivity.** At 50 % supervision against 25 % productivity there is a trough; at 25 %
   supervision against 50 % productivity the reinforced rate is 0.675 from day one and there is none.
   A leader who knows only that inequality already knows whether to expect a dip.

> **Fig 1.3.3 — The reinforcement trough and its crossover.** Line chart. X-axis weeks from the
> decision, 0–14; y-axis cumulative clinics completed, 0–9. A slate line for the crew of 6 rises
> linearly at 0.6 a week, reaching 8 clinics at week **13.3333**. A brand-blue line for the crew of 9
> rises at 0.525 a week to week 4 (**2.1** clinics) then at 0.9 a week, reaching 8 at week
> **10.5556**. The blue line sits **below** the slate line until they cross at week **5.00**, and the
> region between them over weeks 0–5 is shaded pale crimson and labelled "the trough — reinforcement
> looks wrong here; 0.3 clinics, 12.5 % behind at week 4". A crimson dot marks the crossover, annotated
> `R × (1 + d/g) = 4 × 1.25 = 5.00 weeks`. A bracket between the two finishing points is labelled
> "2.7778 weeks saved = USD 39,666.67; incremental effort 15.0 specialist-weeks = USD 63,000; net
> (USD 23,333.33)". Source: PCI original. Alt text: two cumulative-progress lines, the reinforced crew
> starting below the original crew and crossing it at week five before finishing nearly three weeks
> earlier, with the early shortfall shaded as a trough.

**Act reversibly where you can.** Prefer decisions that can be unwound, pilots over commitments,
and options kept open at low cost — then commit hard once uncertainty has genuinely reduced. This
is the leadership behaviour behind rolling-wave planning (Domain 6, KA 6.3.3) and staged
investment (Domain 2).

### AI in this KA

Systems and value reasoning is where AI assistance is *weakest* and most confidently wrong: a
model will produce a fluent benefits case that silently treats outputs as benefits, because that
is what most documents in its training data do. The governed use is narrow and real — stress-test
a benefits chain by asking what would have to be true, generate adoption scenarios, surface
second-order effects a team has not considered — followed by human judgment on which link is
actually fragile. The check that catches the common failure is one question: *for each claimed
benefit, which measured outcome produces it, and who owns that measure?* (Domain 16 turns that
into the benefits register.)

### Key terms — KA 1.3

| Term | Meaning |
|---|---|
| **Output / outcome / benefit / value** | Delivered thing · change in how things work · measurable improvement · benefit against cost and risk. |
| **Adoption** | The outcome measure linking an output to any benefit at all. |
| **Feedback and delay** | Systems properties that make interventions look wrong before they look right. |
| **Local optimisation** | Function-level efficiency that degrades whole-system performance. |
| **Last responsible moment** | The latest point a decision can be taken without foreclosing a needed option. |
| **Cost of delay** | The benefit forgone per period of lateness. |
| **Simple payback** | Cost ÷ annual benefit, in periods; scales as the reciprocal of adoption. |
| **Reinforcement trough** | The transient period after adding people in which progress is worse than it would have been; exists when supervision load per newcomer exceeds their ramp productivity. |
| **Crossover horizon** | `R × (1 + d/g)` — the remaining work below which reinforcement finishes later, not earlier. |
| **Released hours** | Capacity freed, not cash; a benefit only once redeployed to a stated use. |
| **Decision breakeven adoption** | The adoption at which one priced decision stops paying — 36.76 % for Meridian's acceleration. Distinct from Domain 2's **breakeven adoption**, which is the level at which the whole investment's NPV is zero (41.05 %). |

### Sample MCQs — KA 1.3

**MCQ 1.3-A `[1.3.2 · Application]`** 40 clinics installed; 70 % adoption; 6 hours/week released
per adopting clinic at USD 85/hour over 48 weeks. The defensible annual benefit is:
- A. USD 979,200
- B. USD 685,440 ✅
- C. USD 293,760
- D. USD 14,280

*Rationale:* `40 × 0.70 × 6 × 85 × 48 = 685,440`. A is the output-based claim that skips
adoption; C is the overstatement itself; D is the weekly benefit.

**MCQ 1.3-B `[1.3.2 · Analysis]`** A programme board is told "40 clinics live — benefits
delivered". The soundest challenge is:
- A. ask for the installation evidence
- B. ask which measured outcome produces the benefit and who owns that measure, since installation is an output and adoption is unverified ✅
- C. accept it, since the output target was met
- D. ask for the cost variance instead

*Rationale:* The claim skips the outcome link, which is precisely where 30 % of the value leaked
in 1.3.2. A verifies the wrong thing, C repeats the error, D changes the subject.

**MCQ 1.3-C `[1.3.3 · Application]`** Benefit accrues at USD 14,280 per week once adopted.
Compressing the rollout by 8 weeks costs USD 60,000. The decision and its breakeven are:
- A. reject — 60,000 exceeds the weekly benefit
- B. accept — net +USD 54,240, breakeven at 4.20 weeks ✅
- C. accept — net +USD 114,240
- D. indifferent — the two are equal

*Rationale:* `8 × 14,280 = 114,240`, less the 60,000 cost, nets **+54,240**; breakeven
`60,000/14,280 = 4.20` weeks. A compares a total against a weekly rate; C forgets to deduct the
cost; D asserts an equality the arithmetic denies.

**MCQ 1.3-D `[1.3.1 · Analysis]`** A discipline lead reports having "absorbed" a two-week
schedule squeeze with no impact. The systems-literate response is:
- A. record the recovery and move on
- B. ask where the pressure went — into quality, scope, people or suppliers — because pressure moves rather than vanishing ✅
- C. squeeze the remaining teams equally
- D. treat it as evidence the original estimate was padded

*Rationale:* Pressure relocates (1.3.1); the leader's job is to find where. A accepts an
unverified claim, C compounds it, D leaps to a conclusion the report does not support.

**MCQ 1.3-E `[1.3.3 · Evaluation]`** Reinforcing a crew saves 2.7778 weeks at a cost of delay of
USD 14,280 a week and consumes 15.0 extra specialist-weeks at USD 4,200 each. The programme's cost of
delay could not exceed USD 20,400 even at 100 % adoption. The strongest professional conclusion is:
- A. approve — the work finishes 2.7778 weeks earlier, which is the objective
- B. reject on delay grounds: the breakeven cost of delay is USD 22,680 a week, above the programme's maximum conceivable 20,400, so no adoption level makes it pay and any approval needs a different stated reason ✅
- C. approve — 39,666.67 of delay value exceeds the 23,333.33 net loss
- D. reject — adding people always makes a late project later

*Rationale:* `63,000 / 2.7778 = 22,680`, which exceeds the 20,400 ceiling (1.3.3b). A optimises
earliness rather than value; C compares the gross benefit against the net result, double-counting the
benefit; D over-applies the coordination result of Domain 12 as a law — here reinforcement genuinely
does finish earlier, it simply costs more than it saves.

**MCQ 1.3-F `[1.3.2 · Application]`** A cost of USD 2,400,000 against a full-potential benefit of
USD 979,200 a year is assessed under a three-year simple payback rule. The adoption the rule requires
is:
- A. 70.00 %
- B. 81.70 % ✅
- C. 245.10 %
- D. 41.05 %

*Rationale:* The rule needs `2,400,000/3 = 800,000` a year, and `800,000/979,200 = 81.70 %` (1.3.2b).
A is the case's assumption, which is the thing being tested; C divides the cost by the potential and
reads the payback in years as a percentage; D is Domain 2's breakeven adoption for value creation — a
different question, and 40.65 points lower.

**MCQ 1.3-G `[1.3.3 · Evaluation]`** Compressing the rollout by 8 weeks costs USD 60,000. Which
statement best assesses the decision's robustness?
- A. it is robust: the net is +54,240, comfortably positive
- B. it pays at any adoption above 36.76 %, so the direction is robust, but at the 40 % adoption actually achieved the net falls to 5,280 — robust in direction, fragile in magnitude ✅
- C. it is not robust, because it depends on an adoption assumption
- D. robustness cannot be assessed without a discounted appraisal

*Rationale:* `60,000/8 = 7,500` a week is 14.7059 clinics, or 36.76 % adoption; at 40 % the weekly
benefit is 8,160 and the net is `8 × 8,160 − 60,000 = 5,280` (1.3.3). A quotes the point estimate as
though it were the range; C treats any dependency as fatal and so never approves anything; D imports
Domain 2's instrument into a decision whose horizon is eight weeks.

### Self-check — KA 1.3

1. *Why is an output claimed as a benefit an overstatement by construction?* — Because the
   outcome link can fail independently; unadopted outputs produce no benefit (Meridian's 30 %).
2. *What is the leader's question on hearing "we absorbed it"?* — Where did the pressure go?
3. *What must be true before paying to accelerate Meridian?* — That adoption materialises;
   arriving sooner at an unadopted system buys nothing.
4. *By how much does an output-based benefit claim overstate, and what does the answer depend on?* —
   By exactly the non-adoption rate, `1 − a`; it depends on nothing else — not the hours, the rate or
   the length of the operating year (1.3.2).
5. *When does adding people to late work finish later rather than earlier?* — Below the crossover
   horizon `R × (1 + d/g)`; and a trough exists at all only when supervision load per newcomer exceeds
   their ramp productivity (1.3.3b).
6. *What does a three-year payback rule commit an organisation to, on Meridian's figures?* — 81.70 %
   adoption — a commitment expressed in years and never discussed in percentage points (1.3.2b).
7. *Are released clinician-hours a benefit?* — Not yet: they are capacity, and become a cash saving or
   a service improvement only when redeployed to a stated use with an owner (1.3.2).

---

## Knowledge Area 1.4 — Ethics and the responsible use of AI

*Topics: 1.4.1 professional ethics in delivery · 1.4.2 the responsible-AI principle applied ·
1.4.3 the leader's AI accountability.*

### 1.4.1 Professional ethics in delivery

Ethics in this discipline is rarely dramatic; it is a series of small reporting and allocation
decisions taken under pressure. Four recurring tests:

- **Report what is, not what is wanted.** Presenting an optimistic case as the base case is a
  misrepresentation whatever the spreadsheet says (the same rule PFL-AI applies to forecasts).
- **Do not spend other people's margins silently.** Absorbing pressure into team overtime, a
  supplier's contingency or a safety margin is a decision someone is entitled to be told about.
- **Claim only what is true.** Milestone and benefit claims are statements of fact to people
  relying on them (1.3.2).
- **Compete and procure honestly.** Conflicts declared, evaluations run as published, suppliers
  not used as free consultancies (Domain 10's ethical sourcing).

**Conflicts of interest** are handled by disclosure and separation: declared before engagement,
managed or declined — and tested by daylight, *would every party, seeing the full picture, still
regard this as impartial?* Where the answer wavers, impartiality has already failed.

### 1.4.2 The responsible-AI principle applied

The principle from KA 1.1 becomes operational through four obligations that recur in every later
domain:

| Obligation | What it requires of a leader |
|---|---|
| **Appropriate use** | Match the tool to the task; prohibit it where the stakes or data forbid (Domain 14) |
| **Verification** | Machine output checked against evidence by a named human before reliance |
| **Transparency** | Material AI use disclosed in deliverables and decisions |
| **Human decision** | The judgment, and its record, belongs to a person |

The failure modes to expect, stated honestly: **hallucination** (fluent, false specifics — the
invented precedent, the plausible citation); **silent staleness** (confident answers from
out-of-date data); **bias** inherited from training data, especially in anything touching people;
**confidentiality leakage** (project data entering a tool is a disclosure); and
**over-trust through fluency** — the deepest one, because polished output suppresses the scrutiny
that rough output invites. None of these is a reason to refuse the technology; each is a reason
the verification duty is not optional.

### 1.4.3 The leader's AI accountability

Three concrete leadership acts, which Domain 14 then systematises:

**Name the owner.** Every AI-assisted artefact that informs a decision has a human owner who
verified it. Anonymous machine output has no place in a governed decision record (Domain 3,
KA 3.3.4).

**Make verification proportionate and real.** A summary for internal reading needs a glance; a
forecast in a board paper needs recomputation and a source trace; a safety-relevant analysis needs
independent review. Uniform verification is either wasteful or negligent — usually both, in
different places. "Proportionate" is only a defensible word if it can be derived, so the example
below derives it.

**Worked example 1.4.3 — how deep is proportionate, and what actually decides it.**

1. **Setup.** Three AI-assisted artefacts on Meridian. A weekly internal **status summary**: a glance
   costs 0.5 hours at USD 85 (**USD 42.50**); a downstream control — the delivery meeting, where four
   people who know the work read it aloud — catches a material error with probability **0.90**; an
   uncaught error costs about **USD 5,000** in a week of misdirected attention. A **board benefits
   figure**: recomputation plus a source trace costs 6 hours at USD 85 (**USD 510**); once a paper is
   tabled the chance of anyone catching the error is **0.25**; an uncaught error carries the
   output-based overstatement of 1.3.2, **USD 293,760** a year. A **clinical-safety impact
   assessment** for the prescribing module: independent clinical review costs **USD 12,000**; the
   chance of another control catching the error is **0.05**; the modelled consequence is
   **USD 4,000,000**.
2. **Formula.** Verification is worth performing when its cost is below the expected loss it removes:
   `C_v ≤ P(error) × (1 − q) × L`, where `q` is the probability a downstream control catches the error
   anyway and `L` the loss if it flows through. Rearranged, the **breakeven error probability** is
   `P* = C_v ÷ [(1 − q) × L]` — the error rate above which the check pays.
3. **Substitution.** Summary `42.50 / (0.10 × 5,000)`; board figure `510 / (0.75 × 293,760)`; safety
   assessment `12,000 / (0.95 × 4,000,000)`.
4. **Result.**

   | Artefact | Verification `C_v` | `q` | Loss `L` | Exposed loss `(1−q)L` | **Breakeven `P*`** |
   |---|---|---|---|---|---|
   | Weekly status summary | 42.50 | 0.90 | 5,000 | 500.00 | **8.5000 %** |
   | Board benefits figure | 510 | 0.25 | 293,760 | 220,320 | **0.2315 %** |
   | Clinical-safety assessment | 12,000 | 0.05 | 4,000,000 | 3,800,000 | **0.3158 %** |

5. **Interpretation.** Five readings. The third is the counter-intuitive one and the fifth is the
   limit of the method.

   **The breakevens differ by a factor of 36.72, and that is what "proportionate" means.** A glance at
   the summary is justified only if such summaries are wrong more than **8.5 %** of the time — which,
   for AI-drafted narrative over live data, they plainly are, so a glance is warranted and a glance is
   all that is warranted. The board figure must be verified if the error rate exceeds **0.2315 %**,
   which is to say: always. Proportionality is not a sliding scale of diligence applied by feel; it is
   the observation that the same duty produces very different depths once the exposure is written down.

   **Uniform depth is arithmetically absurd, not merely wasteful.** Apply the board paper's six-hour
   recompute to the weekly summary and the breakeven becomes `510 / 500 =` **1.02** — a required error
   probability **above 1**, meaning **no error rate whatever could justify it**. That is a sharper
   statement than "wasteful": it is a check that cannot be justified on any assumption. Meanwhile the
   same six hours applied to the safety assessment would not merely be too shallow, it would be the
   wrong *kind* of check — recomputation cannot find a clinical misjudgement. Depth and *kind* are
   separate choices, and conflating them is how organisations produce thick verification files that
   miss the thing that matters.

   **The cost of checking barely matters; the exposed loss decides everything.** The safety review
   costs **23.53 times** the board recompute, yet its breakeven is only **1.36 times** higher — the two
   are effectively in the same band, while the trivial artefact sits two orders of magnitude away. The
   discriminating variable is `(1 − q) × L`, not `C_v`. The practical consequence is that the familiar
   objection — "we cannot afford to check everything properly" — is answering the wrong question: on
   anything with material downstream reliance, the check is cheap relative to the exposure, and the
   real constraint is attention, not money.

   **`q` is where this arithmetic is most often abused.** A high `q` makes any check unnecessary, so a
   `q` is asserted rather than evidenced far more often than an `L` is. A reviewer's question is
   therefore not "what is your verification policy?" but *which named control would have caught this,
   and when has it caught anything?* On the summary, `q = 0.90` is credible only because the delivery
   meeting genuinely reads it; remove that meeting and the breakeven falls to `42.50/5,000 =`
   **0.8500 %**, a tenfold increase in the case for checking, with no change to the artefact at all.

   **The limit, stated plainly: row three should not be decided this way.** Where a statutory duty,
   an operating licence or patient safety is engaged, the obligation to review is not contingent on an
   expected-value test, and computing `P*` there has exactly one legitimate use — to show that the
   review is *also* cheap, which forecloses the cost argument. It must never be used to conclude that
   a safety review is not worth performing. What constitutes an adequate assessment, who is competent
   to sign it, and what must be retained are set by the applicable regulatory regime and the relevant
   professional body in the jurisdiction; take that from the regulator and from counsel, not from this
   table (Domain 14, KA 14.4 for the governance architecture, and Domain 9 for the assurance regime).
   Two further limits: expected-value reasoning is the wrong frame for a single catastrophic tail even
   where safety is not engaged, and this model prices one artefact at a time, whereas AI-assisted work
   arrives in volume — sizing a *sample* across many artefacts is Domain 14's problem (KA 14.3), and it
   starts where this example stops.

> **Fig 1.4.1 — Proportionate verification: the breakeven error probability.** Horizontal bar chart on
> a logarithmic x-axis, breakeven error probability from 0.1 % to 200 %. Four bars in ascending order of
> breakeven, each labelled with its verification cost over its exposed loss: brand-blue **board benefits
> figure** at **0.2315 %** (`510 / 220,320`), **clinical-safety assessment** at **0.3158 %**
> (`12,000 / 3,800,000`), **weekly status summary** at **8.5000 %** (`42.50 / 500`), then a crimson bar
> for the six-hour recompute misapplied to the weekly summary at **102.00 %** (`510 / 500`), extending
> past a vertical crimson rule at 100 % annotated "no error rate can justify the check →". A slate note
> beneath reads **"36.72× between the summary and the board figure — while the cost of checking differs
> 23.53×"**, and the axis caption states the formula `P* = C_v / [(1 − q) × L]`. Source: PCI original.
> Alt text: three bars of breakeven error probability on a log scale, the trivial artefact two orders of
> magnitude above the two consequential ones, and a fourth bar beyond one hundred per cent showing a
> check that no error rate could justify.

**Protect the team's judgment.** The subtler risk is not a wrong output but the atrophy of the
skills needed to notice one. A leader who accepts AI-drafted plans no one on the team can
critique has traded capability for speed, and will discover the price during the first
recovery (Domain 6, KA 6.4).

### Key terms — KA 1.4

| Term | Meaning |
|---|---|
| **Daylight test** | Would every party, seeing the full picture, still regard this as impartial? |
| **Hallucination** | Fluent, confidently-stated content that is false. |
| **Over-trust through fluency** | Polished output suppressing the scrutiny rough output would attract. |
| **Verification proportionality** | Depth of checking matched to the stakes of reliance. |
| **Breakeven error probability (`P*`)** | `C_v ÷ [(1 − q) × L]` — the error rate above which a verification pays for itself. |
| **Exposed loss** | `(1 − q) × L` — the loss a verification is actually protecting against, after any downstream control. |
| **Depth and kind** | Two separate verification choices: how hard to look, and what sort of looking finds this class of error. |
| **Named owner** | The human who verified an AI-assisted artefact and answers for it. |

### Sample MCQs — KA 1.4

**MCQ 1.4-A `[1.4.1 · Analysis]`** A sponsor asks that the optimistic scenario be presented as
the base case "to keep the board confident". The professional response is:
- A. comply — scenario labels are presentational
- B. decline, and offer the honest base case with sensitivities and the evidence that supports confidence ✅
- C. comply while noting the true base case privately
- D. present both without indicating which is the base case

*Rationale:* Candour about status is a duty, not a style (1.4.1). C documents the
misrepresentation without preventing it; D abdicates the professional judgment the board is
relying on.

**MCQ 1.4-B `[1.4.2 · Application]`** Which AI failure mode most directly explains an
authoritative-sounding schedule that encodes assumptions nobody made?
- A. confidentiality leakage
- B. over-trust through fluency, compounded by absent verification ✅
- C. model bias
- D. hardware limitation

*Rationale:* Polished output invites less scrutiny (1.4.2), and the missing control is
verification. A concerns data egress, C concerns systematic skew (real, but not this symptom),
D is irrelevant.

**MCQ 1.4-C `[1.4.3 · Analysis]`** A leader requires the same verification depth for every
AI-assisted artefact, from internal summaries to safety analyses. This is:
- A. best practice — consistency is the point
- B. simultaneously wasteful and negligent: proportionality means depth matched to the stakes of reliance ✅
- C. acceptable only if the team is small
- D. unnecessary, since approved tools need no verification

*Rationale:* Uniform depth over-checks the trivial and under-checks the critical (1.4.3). A
mistakes uniformity for rigour; D abandons the duty entirely.

**MCQ 1.4-D `[1.4.3 · Application]`** Recomputing an AI-drafted board benefits figure costs USD 510.
An error surviving into the paper would carry an overstatement of USD 293,760, and the chance of
anyone catching it after tabling is 0.25. The breakeven error probability above which the recompute
pays is:
- A. 0.1736 %
- B. 0.2315 % ✅
- C. 0.6944 %
- D. 0.1302 %

*Rationale:* `510 / (0.75 × 293,760) = 0.2315 %` (1.4.3). A divides by the full loss and forgets that
only 75 % of it is exposed; C multiplies by 0.25 instead of (1 − 0.25), using the catch probability as
if it were the escape probability; D applies the escape fraction to the verification cost
(`510 × 0.75`) rather than to the loss, which is the same slip in the opposite direction.

**MCQ 1.4-E `[1.4.3 · Evaluation]`** A safety review costing USD 12,000 has a breakeven error
probability of 0.3158 %, while a USD 510 recompute has one of 0.2315 %. The most useful professional
inference is:
- A. the cheaper check is better value and should be preferred
- B. the cost of checking is nearly irrelevant to whether to check — a 23.53-fold cost difference produces only a 1.36-fold difference in breakeven, because the exposed loss dominates ✅
- C. both checks are unnecessary, since the breakevens are below 1 %
- D. the safety review should be dropped, as its expected value is the weaker of the two

*Rationale:* `P* = C_v/[(1 − q)L]`, and the exposed losses differ by more than the costs do (1.4.3). A
compares the wrong quantity — the checks protect different exposures; C inverts the meaning of a
breakeven, which is a threshold to exceed, not a hurdle to fail; D applies an expected-value test to a
safety decision, which the example expressly rules out.

### Self-check — KA 1.4

1. *State the daylight test.* — Would every party, seeing the full picture, still regard this as
   impartial?
2. *Which AI failure mode is hardest to counter, and why?* — Over-trust through fluency: polish
   suppresses the scrutiny that rough work attracts.
3. *What does "protect the team's judgment" mean in practice?* — Do not accept AI output the team
   cannot critique; capability traded for speed is repaid during recovery.
4. *State the breakeven error probability and say which of its inputs decides the answer.* —
   `P* = C_v / [(1 − q) × L]`; the exposed loss `(1 − q)L` dominates, and the cost of checking barely
   moves it (1.4.3).
5. *What does a breakeven above 100 % tell you?* — That no error rate could justify that check on that
   artefact: it is unjustifiable rather than merely uneconomic (1.4.3).
6. *Where must this arithmetic not be the deciding test?* — Where a statutory duty, licence or safety
   is engaged; there its only legitimate use is to show the review is also cheap (1.4.3).

---

## Advanced topics — Domain 1

### 1.A.1 Authority, influence and the borrowed team

Most project leaders hold less authority than accountability — the defining asymmetry of the role.
The sources of influence that actually work in temporary organisations: **competence** (visible
command of the work), **reciprocity** (having helped others deliver), **information** (being the
person with a true picture), **coalition** (sponsor and peer support secured before it is needed),
and **fairness** (a record of allocating credit and blame accurately). Positional authority is the
weakest of the six in borrowed teams, because the borrowed people's careers are decided elsewhere.
The practical implication: influence is *built in advance*, and a leader who begins building it
during a crisis is too late.

### 1.A.2 Success criteria and the multiple-verdict problem

Projects are judged simultaneously against delivery (time, cost, quality), outcome and benefit
realisation, and stakeholder satisfaction — which is why the same project is often described as a
success and a failure by different parties, honestly. The professional response is not to pick a
flattering verdict but to **agree the criteria in advance, with owners and measures** (Domain 2's
benefits mapping, Domain 5's acceptance criteria), and to report against all of them. Unagreed
success criteria are settled retrospectively by whoever is most disappointed.

### 1.A.3 Dissent, compliance and the record that protects everyone

The obligation set of 1.2.2 has a case it does not cover: the leader escalates with options, the
accountable authority chooses the option the leader advised against, and the leader must now deliver
it. This is common, it is not misconduct by anyone, and handling it badly damages both the project and
the leader.

Three positions are available and only one of them is professional. **Silent compliance** protects
the relationship and destroys the record: when the risk materialises there is no evidence the
question was asked, and the leader who warned privately is indistinguishable from the leader who did
not notice. **Obstruction** — delivering the decision unenthusiastically, slowly, or with visible
reservation to the team — is a breach of the duty to the sponsor and, because a team reads its leader
accurately, converts a difficult decision into a failing one. **Dissent and comply** is the
professional position: the disagreement is stated once, in writing, to the person who holds the
authority; the reasoning and the condition that would change the answer are recorded in the decision
log (Domain 3, KA 3.3.4); the leader then implements the decision as if it were their own, including
to the team; and the recorded condition is monitored, so that if it occurs the matter returns
automatically rather than as an accusation.

What makes this work is that the record is **neutral in tone and specific in content**: the option
chosen, the option advised, the reason, and the observable that would reopen it — for example, on
Meridian, "proceed to full rollout without funded adoption support; the writer's advice was to fund
champions first; reopen if adoption at week 13 is below 55 %." That sentence would have converted the
26-week silence of 1.2.2 into an automatic trigger, and it costs one line. A record written to
apportion blame in advance ("for the avoidance of doubt, I do not accept responsibility for…")
achieves the opposite: it invites the authority to stop asking for advice, and it is usually worth
nothing anyway, because accountability is not reallocated by assertion (1.2.1).

Two boundaries. First, dissent and comply applies to decisions within the range of legitimate
judgment. It does **not** apply where the instruction is unlawful, where safety or a regulatory
obligation would be breached, or where the leader is being asked to make a statement they know to be
untrue (1.4.1) — those are refusal-and-escalate situations, and they are the reason a profession has
ethics rather than only a service ethic. Second, the protections available to someone who raises such
a matter, the routes that count as a protected disclosure, and any obligation to report externally
are entirely jurisdiction- and sector-specific, and can turn on details of how and to whom the
disclosure was made. Before acting on that class of concern, take advice — from counsel, from the
relevant professional body, or from the organisation's designated route — rather than from any
general account, including this one.

### 1.A.4 The reviewer's leadership eye

Invariants an experienced reviewer tests in the first hour on any project: exactly one accountable
name per outcome; a benefits chain where each claimed benefit traces to a measured outcome with an
owner; decision records that show what was known when; escalations that arrived early enough to
matter; forecasts with ranges rather than single numbers; and AI-assisted artefacts carrying a
named verifier. Every one of these is cheap to check and expensive to have missing — the same
posture Domains 6 and 7 apply to schedules and cost.

Four questions convert that list into arithmetic in the same hour, and each has a one-line answer or
a finding. *Does the business case contain an adoption term?* — if not, the overstatement is already
known to be the non-adoption rate (1.3.2). *What operational condition does the approval hurdle
imply?* — a payback rule restated as an adoption percentage often turns out to be a commitment nobody
made (1.3.2b). *What is the cost of delay per week, and who computed it?* — a project that cannot
state it cannot price a single timing decision, including its own escalations (1.3.3). *For the last
AI-assisted artefact that mattered, what was the exposed loss and which named control was relied on?*
— the answer distinguishes a verification regime from a verification policy (1.4.3). A reviewer who
asks those four learns more in ten minutes than a document review yields in a day, because each of
them is answerable only by someone who has actually done the arithmetic.

---

## Industry variations — Domain 1

- **Public programmes.** Accountability is doubled — professional and political — and the public
  obligation of 1.2.2 is enforceable through scrutiny bodies; benefits (not outputs) are the
  currency of accountability, and announced dates often precede plans (Domain 6's Case B).
- **Regulated industries (pharma, aviation, nuclear).** The standard of care is codified and
  auditable; the "care exercised" defence of 1.2.3 is a documented reality rather than an
  argument, and AI use faces explicit validation expectations.
- **Construction and engineering.** Authority is contractual as much as organisational; influence
  runs through commercial relationships, and the leader's obligation set includes site safety as a
  first-order duty.
- **Technology and product.** Outcome ownership frequently sits with a product role rather than the
  delivery leader, making the 1.3.2 chain a shared accountability that must be explicitly split —
  the commonest source of "who owns adoption?" disputes.
- **Professional services and consulting.** Duties to the client organisation and to the
  end-users can diverge, and independence (1.4.1) becomes a standing rather than occasional test.

## Case study — Domain 1: Meridian under scrutiny (public health)

**Situation.** Eighteen months in, Meridian Care Records reports 40 of 40 clinics installed, on
time, 3 % under budget. The programme is publicly praised. Nine months later a health-service
audit finds clinician hours released are running at roughly a third of the business case, and the
programme is publicly described as a failure. Both accounts cite true facts.

**Analysis.** The delivery verdict was sound: the outputs landed. The benefits case had been
written against **outputs** — all 40 clinics, no adoption term — so it claimed USD 979,200 per
year where the honest figure at 70 % adoption was USD 685,440, and actual adoption reached only
about 40 %, giving roughly USD 391,680. Two failures compounded: an arithmetic one (skipping the
outcome link, 1.3.2) and an accountability one (**nobody owned adoption** — the delivery leader
owned installations, the clinical directorate owned practice change, and no single name owned the
measure that connected them, 1.2.1).

**The account in numbers.** Both public verdicts were arithmetically defensible, which is the whole
difficulty.

| Line | Figure | Where it comes from |
|---|---|---|
| Approved cost | USD 2,400,000 | The business case |
| Delivery verdict — 3 % underspend at installation | **+USD 72,000**, once | WE 1.1.1 |
| Benefit claimed in the case (output basis, no adoption term) | USD 979,200 a year | 1.3.2 |
| Honest benefit at the case's own 70 % adoption | USD 685,440 a year | 1.3.2 |
| Benefit at the 40 % adoption actually measured | USD 391,680 a year | WE 1.1.1 |
| Programme verdict — annual shortfall against the honest case | **(USD 293,760) a year** | WE 1.1.1 |
| Scale of the two verdicts | shortfall = **4.08 ×** the underspend, in year one alone | WE 1.1.1 |
| Weekly form of the shortfall | **USD 6,120 a week** = 12 clinics × 6 h × USD 85 | WE 1.2.2 |
| Simple payback, honest benefit against approved cost | **3.5014 years** | WE 1.3.2b |
| Simple payback at the measured 40 % adoption | **6.1275 years** | WE 1.3.2b |

Two figures in that table explain the collapse of the public narrative. The programme was praised on
a line worth 72,000 once and condemned on a line worth 293,760 a year, and **nobody had put the two
in the same table** — which is a reporting failure, not a delivery one. And the payback the board
believed it had approved, 2.4510 years on the output-based claim, became 6.1275 years on measured
adoption: the same programme, the same installations, an investment case two and a half times longer.

**What was done.** The recovery was not technical. Adoption was made a named accountability with a
monthly measure; the benefits case was restated with an adoption term and a sensitivity range
(50/70/90 % → 489,600 / 685,440 / 881,280); clinical champions were funded in the eleven
lowest-adoption clinics at USD 12,000 each, **USD 132,000** in total; and the programme board's report
changed from "clinics live" to "clinics using, and hours released". Adoption reached 68 % within a
year, recovering **11.2** of the 12 missing clinics — **USD 5,712 a week**, or **USD 274,176** a year.

**What the timing cost.** The monthly measure had shown 16 of 40 clinics in daily use at **week 13**;
the matter reached the board at **week 39**, through the audit. On WE 1.2.2's arithmetic those 26 weeks
were worth **USD 148,512** — **1.13 times** the entire remedy they would have funded, and more than
twice the 72,000 underspend the programme had been congratulated for. The remedy needed only
**23.11 weeks** of earliness to pay for itself outright.

**And the underspend did not survive.** Domain 16's closing account records a final capital outturn of
**USD 2,514,000** against the **USD 2,328,000** implied by the 3 % underspend — a **USD 186,000** swing,
most of it the recovery described above. The celebrated cost result was an interim figure that the
programme later took back, which is the general lesson: **an underspend booked before the outcome is
measured is a loan against the recovery, not a saving.**

**What the domain teaches here.** Every element of this domain appears: outputs are not benefits
(1.3.2); one accountable name per outcome (1.2.1); success criteria agreed in advance or settled
by the disappointed (1.A.2); and the leader's attention belongs where the value actually
leaks — which the arithmetic, not instinct, identified.

## Case study B — Domain 1: the plan nobody could critique (financial services)

**Situation.** A platform-migration leader under time pressure used an AI planning assistant to
generate the full delivery schedule and dependency network, reviewed it briefly, and issued it.
It was detailed, internally consistent and professionally formatted. It also assumed the legacy
data extract could run concurrently with the schema freeze — an assumption no one on the team had
made, and which the two engineers who would have spotted it never saw.

**What happened.** The conflict surfaced eleven weeks later during rehearsal, costing a
seven-week slip and a weekend of reputational damage with the regulator's supervisory team. The
post-incident review found no fault in the tool: the schedule was a reasonable inference from an
under-specified prompt. The failures were the leader's — no verification against the discipline
leads (the 1.1 principle and 1.4.2 obligation), and no named verifier on the artefact (1.4.3).
The compounding finding was cultural: the polish of the output had actively discouraged
challenge, and two team members later said they had assumed the network "must have been checked".

**What it cost, and what the check would have cost.** This programme's cost of delay was
**USD 38,000 a week** — dual-running licence and support on the legacy platform at **USD 12,000**, plus
**USD 26,000** of deferred decommissioning saving — so the seven-week slip cost **USD 266,000** before
any regulatory consequence. The verification that would have found the conflict was two discipline
leads reading the dependency network for four hours each: `2 × 4 × 145 =` **USD 1,160**. The loss was
therefore **229.31 times** the check. Put through KA 1.4.3's test, the breakeven error probability was
`1,160 / 266,000 =` **0.4361 %**: the review paid for itself if an AI-generated dependency network
carried one material error more than about four times in a thousand. It is not necessary to know the
true rate to make that decision — only to observe that no honest estimate of it is that low.

Two details make this the general case rather than an unlucky one. First, the breakeven sits in the
same band as the board-paper and safety-review rows of 1.4.3 (**0.2315 %** and **0.3158 %**), which is
what one should expect: wherever an artefact carries material downstream reliance, the check is
trivially cheap against the exposure, and the argument that verification is unaffordable does not
survive being written down. Second, the eleven weeks between issue and discovery are 1.3.1's delay
property doing its work — the seven-week loss was locked in from the day the plan was issued, and the
rehearsal that finally revealed it was the only control in the chain, with the schedule freeze already
committed by the time it fired.

**What the domain teaches here.** Fluency is not evidence. Accountability did not move to the
vendor or the model — it stayed with the person who issued the plan (MCQ 1.2-B is this case in
miniature). And the durable fix is structural, not exhortative: a named verifier per artefact,
discipline leads walking their own links, a team kept capable of critique — and a rule that no artefact
whose exposed loss exceeds a stated figure is issued without a recorded check, because at 229 to 1 the
decision is not close enough to require judgment.

---

## Executive perspective — Domain 1

What a project leader cannot delegate in this domain:

- **The single accountable name.** For every outcome, including the awkward ones like adoption.
  Where two names appear, the leader's job is to reduce it to one.
- **The benefits chain's honesty.** Each claimed benefit traced to a measured outcome with an
  owner. A leader who lets outputs be reported as benefits has authorised the failure of
  Meridian's first eighteen months.
- **The obligation set, held openly.** Sponsor, team, users, public — with conflicts escalated
  with options rather than absorbed in silence.
- **Bad news made safe.** The single highest-leverage cultural act available, because it converts
  irreversible surprises into recoverable problems — and on Meridian it was worth **USD 148,512**,
  1.13 times the remedy it would have funded, for the price of one message (1.2.2).
- **The inversion of every hurdle the organisation imposes.** A three-year payback rule is an
  **81.70 %** adoption commitment; a delegation threshold is an escalation volume; a gate is a priced
  delay. The director's contribution is insisting that hurdles be restated as the operational
  conditions they imply, because that is the form in which someone can own them (1.3.2b).
- **The AI accountability line.** Named verifiers, proportionate depth, and a team that remains
  able to critique what the tools produce. Proportionate is a derivable word: the exposed loss
  `(1 − q) × L` decides the depth, and a check whose breakeven error probability exceeds 100 % is one
  the organisation should stop performing (1.4.3).

## Calculation exercises — Domain 1

**Exercise 1.1** A programme installs a system in 60 sites; adoption is 65 %; each adopting site
releases 5 hours/week at USD 90/hour over 46 weeks. Compute the defensible annual benefit and the
overstatement in an output-based claim.
*Solution.* Adopting sites `60 × 0.65 = 39`; benefit `39 × 5 × 90 × 46 =` **USD 807,300**.
Output-based `60 × 5 × 90 × 46 =` **USD 1,242,000**; overstatement **USD 434,700** = **35.0 %**,
exactly the non-adoption rate. Common error: applying adoption to the *rate* rather than the site
count — the same product here, but it conceals which link failed and misdirects the fix.

**Exercise 1.2** Using Meridian's figures (28 adopting clinics, 6 h/week, USD 85/h), compute the
cost of a 5-week delay and the maximum justified acceleration spend for 5 weeks.
*Solution.* Per week `28 × 6 × 85 =` **USD 14,280**; five weeks **USD 71,400** — which is also the
maximum justified spend, since above it acceleration destroys value. Common error: comparing the
spend against annual benefit rather than the benefit for the weeks actually saved.

**Exercise 1.3** Meridian's benefit at 50 %, 70 % and 90 % adoption (40 clinics, 6 h/week,
USD 85/h, 48 weeks). What does the spread imply for where the leader spends attention?
*Solution.* **USD 489,600 · 685,440 · 881,280**. The 40-point adoption swing moves annual benefit
by **USD 391,680** — far more than plausible schedule compression could add (Exercise 1.2 values
5 weeks at 71,400). Attention belongs on adoption (Domain 11), not acceleration.

**Exercise 1.4** A programme is approved at USD 1,800,000 and delivers 2 % under budget. Its
full-potential benefit is USD 540,000 a year; the case assumed 75 % adoption and measurement finds
45 %. Report both verdicts and their scale, and state what may not be done with them.
*Solution.* Delivery verdict `1,800,000 × 0.02 =` **+USD 36,000**, once. Benefit at plan
`540,000 × 0.75 =` **USD 405,000**; at outturn `540,000 × 0.45 =` **USD 243,000**; programme verdict
**(USD 162,000) a year**, equivalently `540,000 × (0.75 − 0.45)`. The shortfall is **4.50 times** the
underspend in the first year alone. What may not be done: netting them into a single figure — one is a
stock, the other a flow, and a net requires a horizon and a discount rate from the business case.
Common error: reporting "the programme is 126,000 down" (`162,000 − 36,000`), which subtracts an
annual flow from a one-off variance and produces a number that means nothing.

**Exercise 1.5** An investment of USD 3,150,000 has a full-potential benefit of USD 1,050,000 a year
and assumes 60 % adoption. The sponsor applies a four-year simple payback rule. Compute the payback as
assumed, and the adoption the rule requires.
*Solution.* Benefit `1,050,000 × 0.60 =` **USD 630,000**; payback `3,150,000 / 630,000 =` **5.00
years**, which fails the rule. The rule needs `3,150,000 / 4 =` **USD 787,500** a year, so adoption of
`787,500 / 1,050,000 =` **75.00 %** — fifteen percentage points above the assumption, and the sentence
the approval meeting should have heard. Common error: dividing cost by *potential* benefit
(`3,150,000/1,050,000 =` 3.00 years) and reporting a pass; that is the output-based payback, understated
by the factor `1/0.60 = 1.6667`.

**Exercise 1.6** A crew of 5 completes 0.08 units a week each. Two people are added; they take a
3-week ramp at 20 % productivity and each absorbs 60 % of an existing member's time. Compute the
crossover horizon and say whether a trough occurs.
*Solution.* Supervision loss `2 × 0.6 × 0.08 = 0.096`; newcomer output `2 × 0.2 × 0.08 = 0.032`;
deficit `d = 0.064` a week; post-ramp gain `g = 2 × 0.08 = 0.16` a week. Crossover
`R × (1 + d/g) = 3 × (1 + 0.4) =` **4.20 weeks**. A trough does occur, because supervision load per
newcomer (60 %) exceeds ramp productivity (20 %); with fewer than 4.20 weeks of work remaining the
reinforcement finishes later. Common error: comparing capacity before and after (0.40 against 0.56 a
week) and concluding the addition helps whenever work remains — which never computes the crossover and
so cannot distinguish the two cases.

**Exercise 1.7** Verifying an AI-drafted forecast costs USD 720. An error surviving into the decision
would cost USD 250,000, and an existing control catches such errors with probability 0.40. Compute the
breakeven error probability, and state what would change it most.
*Solution.* Exposed loss `(1 − 0.40) × 250,000 =` **USD 150,000**; breakeven
`720 / 150,000 =` **0.48 %**. What changes it most is the control: removing it raises the exposed loss
to 250,000 and lowers the breakeven to **0.288 %**, while halving the verification cost only moves it to
0.24 % — so evidence for `q` matters about as much as the price of checking. Common error: dividing by
the full loss and reporting 0.288 %, which is arithmetically the same as assuming no control exists;
the error understates the breakeven and so overstates the case for checking, and it is the mirror of
the commoner abuse in the other direction — asserting a high `q` for a control nobody has named.

## Practitioner's toolkit — Domain 1

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 1.T.1 — Leader's accountability map (one page)

Per outcome: the outcome stated in outcome language (not output language) · **the single
accountable name** · responsible parties · the measure that evidences it and its owner · the
decision authority and threshold (Domain 3) · review cadence. Rule: if any row has two accountable
names or an output in the outcome column, it is not finished.

### Toolkit 1.T.2 — Benefits-chain test (run on any benefits claim)

- [ ] Each claimed benefit names the measured **outcome** that produces it.
- [ ] Each outcome measure has an owner and a reporting cadence.
- [ ] Adoption (or the equivalent behavioural change) appears explicitly, with a value.
- [ ] The claim carries a sensitivity range, not a single number.
- [ ] Output milestones are labelled as milestones, never as benefits.
- [ ] Cost of delay per period is stated, so timing decisions can be priced (1.3.3).

### Toolkit 1.T.3 — AI-use and verification record

Per AI-assisted artefact: tool and approved environment · data classification cleared ·
prompt/intent summary · **verification performed and its depth** · **named verifier** ·
disclosure status in the deliverable · date. Standing agenda item, not an archive: the register's
purpose is that the next artefact is verified, not that the last one was.

### Toolkit 1.T.4 — Verification-depth card (one per artefact class, not per artefact)

Six fields, filled once for a *class* of artefact and reused: the class · what relies on it · the
**loss `L`** if a material error flows through · the **named downstream control** and its assumed
catch rate `q`, with the evidence for that rate · the verification's **cost `C_v`** and **kind**
(glance · recompute and source trace · independent review of a different discipline) · the resulting
**breakeven `P* = C_v/[(1−q)L]`** · the decision, and who verifies.

Three rules that make it usable rather than decorative. **A `q` with no named control is recorded as
zero** — this is the single field that determines whether the card is honest. **A class whose `P*`
exceeds 100 % has its check removed, not reduced**, and the removal is recorded, because a check no
error rate can justify is consuming attention that a real exposure needs. And **any class where a
statutory duty, licence or safety is engaged is marked `mandatory — not an expected-value decision`**,
with the `P*` retained only as evidence that the review is cheap (1.4.3). Review the card when the
tooling changes, not when the artefact changes.

## Exam preparation — Domain 1

**The traps.** Treating outputs as benefits (1.3.2 — the domain's central trap, and Meridian's
whole case study) · saying accountability is "shared" (1.2.1) · assigning accountability to a
vendor or a tool (MCQ 1.2-A, 1.2-B) · comparing an acceleration cost against annual rather than
saved-period benefit (Exercise 1.2) · reading a delivery success as a programme success (1.1.1) ·
accepting "we absorbed it" (1.3.1) · uniform AI verification depth (1.4.3) · judging the standard
of care by outcome alone (1.2.3) · **netting a one-off capital variance against an annual benefit
flow** (WE 1.1.1, Exercise 1.4) · **valuing early warning at the size of the problem rather than at
the run rate the remedy recovers** (WE 1.2.2 — 6,120 instead of 5,712) · **dividing cost by potential
rather than realistic benefit when computing payback**, which understates it by exactly `1/a`
(WE 1.3.2b, Exercise 1.5) · **approving a reinforcement because it finishes earlier, without testing
it against the cost of delay** (WE 1.3.3b — earlier and worse off are compatible) · **omitting the
downstream control from a breakeven error probability**, or asserting one that has no name
(WE 1.4.3, Exercise 1.7).

**The calculations to be able to do under time pressure.** Annual benefit `= sites × adoption ×
hours × rate × weeks`, and the overstatement of an output-based claim as `1 − a`. Cost of delay
`= adopting sites × hours × rate`, and breakeven weeks `= cost ÷ cost of delay`. Simple payback
`= cost ÷ (potential × adoption)`, and the adoption a payback target `T` requires
`= cost ÷ (T × potential)`. Crossover horizon `= R × (1 + d/g)`. Breakeven error probability
`= C_v ÷ [(1 − q) × L]`. Five formulae, all of them one line, and between them they price every
decision this domain contains.

**Reflection questions.**
1. Take your current project: write the single accountable name for its principal outcome. If you
   hesitated, or wrote two, what does that predict?
2. For each benefit in your business case, which measured outcome produces it and who owns that
   measure? What percentage of your claimed value survives the question?
3. Which AI-assisted artefact in your last month of work had no named verifier — and what would
   have caught it if it had been wrong?

## Domain 1 summary

Project leadership is a distinct profession because temporary work fails distinctively: problems
discovered late become irreversible, teams have no accumulated trust to draw on, and lead time is
the cheapest resource available and is spent whether used or not. The discipline rests on a
precise view of accountability — responsibility is delegable, the obligation to answer is not, one
name per outcome — held across four directions at once (sponsor, team, users, public) with
conflicts escalated rather than absorbed, and judged by the care a competent practitioner would
exercise rather than by outcome alone. Two habits of mind make a leader rather than a
coordinator: systems thinking, which expects feedback, delay, local optimisation and pressure that
relocates instead of vanishing; and the outputs–outcomes–benefits–value chain, which Meridian
turns into arithmetic — 40 clinics installed produce USD 685,440 of annual benefit at 70 %
adoption, not the USD 979,200 an output-based claim asserts, and the 30 % gap is the outcome link
being skipped, a proportion that is exactly the non-adoption rate whatever the hours or the rate.
Against the approved cost of USD 2,400,000 that benefit repays in 3.5014 years, and a sponsor's
three-year payback rule is therefore an undisclosed commitment to 81.70 % adoption — the habit of
inverting a hurdle into the condition it implies being one of the most useful in the book. Timing
decisions are priceable (USD 14,280 per week; acceleration breaking even at 4.20 weeks and paying at
any adoption above 36.76 %), delay itself is priceable (a measure left silent for 26 weeks cost
USD 148,512, more than the remedy it would have funded), and reinforcement is priceable in both
directions — 2.7778 weeks earlier and USD 23,333.33 worse off, with a crossover horizon of 5.00 weeks
computable before the decision. Attention belongs where value actually leaks. Around all of it sits
professional ethics and the suite's governing principle — **AI proposes; the professional verifies, decides and
remains accountable** — with named verifiers, a team kept capable of critique, and a depth of checking
that is derived rather than felt: the breakeven error probability `C_v/[(1−q)L]` puts a weekly summary
at 8.50 % and a board benefits figure at 0.2315 %, a difference of 36.72 times that is the whole
content of the word "proportionate". Domain 2 turns strategy into selected work; Domain 3 gives the
accountability of this domain its governance machinery.
