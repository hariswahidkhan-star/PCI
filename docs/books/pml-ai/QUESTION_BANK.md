# Question bank — PML-AI Body of Knowledge

> **Derived, not duplicated.** Every item is the question as it appears in its Knowledge
> Area, consolidated here by `_build/make_question_bank.py`. Answer keys and rationales are
> the chapters' own. To change an item, change it in its Knowledge Area and regenerate —
> which is why there is no second copy to fall out of step.

**289 items** across 16 domains. Every numeric option in every item is
independently recomputed by the golden-answer suite, not only the correct one, so a
distractor cannot be arithmetically impossible without the gate failing.

## Coverage by cognitive level

| Level | Items | Share |
|---|---|---|
| Recall | 10 | 3.5 % |
| Comprehension | 20 | 6.9 % |
| Application | 95 | 32.9 % |
| Analysis | 108 | 37.4 % |
| Evaluation | 56 | 19.4 % |

A bank weighted heavily to recall tests memory rather than competence; one weighted
heavily to Evaluation is unanswerable under time pressure. The distribution above is a fact
to be reviewed against the examination blueprint, not a claim that it is correctly balanced —
the blueprint weightings are an open decision (see `CORPUS_GATE_REPORT.md` §9).

## Coverage by domain

| Domain | Items | Levels represented |
|---|---|---|
| 1 | 13 | Recall, Application, Analysis |
| 2 | 13 | Recall, Application, Analysis |
| 3 | 15 | Comprehension, Application, Analysis, Evaluation |
| 4 | 17 | Comprehension, Application, Analysis, Evaluation |
| 5 | 19 | Comprehension, Application, Analysis, Evaluation |
| 6 | 22 | Recall, Application, Analysis |
| 7 | 13 | Recall, Application, Analysis |
| 8 | 15 | Recall, Application, Analysis |
| 9 | 23 | Comprehension, Application, Analysis, Evaluation |
| 10 | 21 | Comprehension, Application, Analysis, Evaluation |
| 11 | 20 | Comprehension, Application, Analysis, Evaluation |
| 12 | 16 | Application, Analysis, Evaluation |
| 13 | 21 | Comprehension, Application, Analysis, Evaluation |
| 14 | 20 | Comprehension, Application, Analysis, Evaluation |
| 15 | 21 | Comprehension, Application, Analysis, Evaluation |
| 16 | 20 | Comprehension, Application, Analysis, Evaluation |

---

## Domain 1

**1.1-A** `[1.1.1 · Analysis]` A programme's four projects each deliver on time and to budget, but the intended service change does not materialise. The most accurate statement is:

- A. the programme succeeded, since all its projects succeeded
- B. the projects succeeded on their own test while the programme failed on its own test — outcomes are not the sum of outputs ✅
- C. the projects must therefore have failed
- D. programme success cannot be assessed until every project closes

*Rationale:* Projects are judged on delivered outputs, programmes on outcomes; the two tests are different, which is exactly why the distinction exists. C rewrites history to preserve a single verdict, and D denies the measurement programmes exist to make.


**1.1-B** `[1.1.2 · Analysis]` Why does project leadership place more weight on early definition than operational leadership does?

- A. project teams are less capable
- B. project problems grow monotonically more expensive as commitments harden, so lead time is the cheapest resource available ✅
- C. operations have no need for planning
- D. projects have larger budgets

*Rationale:* Irreversibility is the structural difference (1.1.2). A and D are unfounded; C misstates operational practice, which plans continuously — just with the ability to correct cheaply next cycle.


**1.1-C** `[1.1.3 · Recall]` A team charter is written in week one primarily because:

- A. governance requires the document
- B. a temporary organisation has no accumulated norms, so what operations grow informally must here be made explicit ✅
- C. it replaces the need for a project plan
- D. it fixes the team's membership for the duration

*Rationale:* Explicit clarity substitutes for familiarity the team has had no time to build. A is a by-product, C confuses artefacts, D is false — membership usually changes.


**1.2-A** `[1.2.1 · Application]` A leader outsources the migration design to a specialist vendor under a fixed-price contract. Accountability for the migration's success now rests with:

- A. the vendor, under the contract
- B. the leader, who retains the duty to specify, verify proportionately and act on failure ✅
- C. jointly and equally with the vendor
- D. the sponsor, who approved the contract

*Rationale:* The contract transfers responsibility (and commercial risk); accountability is non-delegable. C describes the arrangement that guarantees nobody answers (1.2.1); D confuses approving a route with owning the delivery.


**1.2-B** `[1.2.1 · Analysis]` An AI planning tool generates a schedule that the leader submits unchanged; it contains an infeasible dependency that causes a two-month slip. The accountability position is:

- A. shared with the tool's vendor, who supplied a defective product
- B. the leader's: a tool cannot answer for an outcome, and submitting unverified output is a failure of the verification duty ✅
- C. nobody's — the failure was technological
- D. the team's, for not catching it

*Rationale:* Accountability requires a party who can be asked to answer, so it cannot attach to software (1.2.1); the specific lapse is verification (1.1's principle). Vendor recourse (A) is a commercial question and does not relocate the professional answer; D inverts delegation.


**1.2-C** `[1.2.3 · Analysis]` A leader's project overran despite a documented method, ranged forecasts, a decision log and escalated risks. Under the standard of care this is:

- A. professional negligence, because the project overran
- B. defensible practice — care is judged on the exercise of competent method, not on outcome alone ✅
- C. irrelevant, since standards of care apply only to regulated professions
- D. defensible only if the overrun was under 10 %

*Rationale:* The standard is conduct-based (1.2.3). A collapses care into outcome; C is false in substance — the expectation attaches to professional practice generally; D invents a threshold.


**1.3-A** `[1.3.2 · Application]` 40 clinics installed; 70 % adoption; 6 hours/week released per adopting clinic at USD 85/hour over 48 weeks. The defensible annual benefit is:

- A. USD 979,200
- B. USD 685,440 ✅
- C. USD 293,760
- D. USD 14,280

*Rationale:* `40 × 0.70 × 6 × 85 × 48 = 685,440`. A is the output-based claim that skips adoption; C is the overstatement itself; D is the weekly benefit.


**1.3-B** `[1.3.2 · Analysis]` A programme board is told "40 clinics live — benefits delivered". The soundest challenge is:

- A. ask for the installation evidence
- B. ask which measured outcome produces the benefit and who owns that measure, since installation is an output and adoption is unverified ✅
- C. accept it, since the output target was met
- D. ask for the cost variance instead

*Rationale:* The claim skips the outcome link, which is precisely where 30 % of the value leaked in 1.3.2. A verifies the wrong thing, C repeats the error, D changes the subject.


**1.3-C** `[1.3.3 · Application]` Benefit accrues at USD 14,280 per week once adopted. Compressing the rollout by 8 weeks costs USD 60,000. The decision and its breakeven are:

- A. reject — 60,000 exceeds the weekly benefit
- B. accept — net +USD 54,240, breakeven at 4.20 weeks ✅
- C. accept — net +USD 114,240
- D. indifferent — the two are equal

*Rationale:* `8 × 14,280 = 114,240`, less the 60,000 cost, nets **+54,240**; breakeven `60,000/14,280 = 4.20` weeks. A compares a total against a weekly rate; C forgets to deduct the cost; D asserts an equality the arithmetic denies.


**1.3-D** `[1.3.1 · Analysis]` A discipline lead reports having "absorbed" a two-week schedule squeeze with no impact. The systems-literate response is:

- A. record the recovery and move on
- B. ask where the pressure went — into quality, scope, people or suppliers — because pressure moves rather than vanishing ✅
- C. squeeze the remaining teams equally
- D. treat it as evidence the original estimate was padded

*Rationale:* Pressure relocates (1.3.1); the leader's job is to find where. A accepts an unverified claim, C compounds it, D leaps to a conclusion the report does not support.


**1.4-A** `[1.4.1 · Analysis]` A sponsor asks that the optimistic scenario be presented as the base case "to keep the board confident". The professional response is:

- A. comply — scenario labels are presentational
- B. decline, and offer the honest base case with sensitivities and the evidence that supports confidence ✅
- C. comply while noting the true base case privately
- D. present both without indicating which is the base case

*Rationale:* Candour about status is a duty, not a style (1.4.1). C documents the misrepresentation without preventing it; D abdicates the professional judgment the board is relying on.


**1.4-B** `[1.4.2 · Application]` Which AI failure mode most directly explains an authoritative-sounding schedule that encodes assumptions nobody made?

- A. confidentiality leakage
- B. over-trust through fluency, compounded by absent verification ✅
- C. model bias
- D. hardware limitation

*Rationale:* Polished output invites less scrutiny (1.4.2), and the missing control is verification. A concerns data egress, C concerns systematic skew (real, but not this symptom), D is irrelevant.


**1.4-C** `[1.4.3 · Analysis]` A leader requires the same verification depth for every AI-assisted artefact, from internal summaries to safety analyses. This is:

- A. best practice — consistency is the point
- B. simultaneously wasteful and negligent: proportionality means depth matched to the stakes of reliance ✅
- C. acceptable only if the team is small
- D. unnecessary, since approved tools need no verification

*Rationale:* Uniform depth over-checks the trivial and under-checks the critical (1.4.3). A mistakes uniformity for rigour; D abandons the duty entirely.


## Domain 2

**2.1-A** `[2.1.3 · Analysis]` A project is delivering to plan, but the strategic priority it served has been superseded. The governance-sound response is:

- A. continue — the project is on track and the case was approved
- B. re-test the case at the next gate and decide on current strategy and remaining cost and benefit ✅
- C. continue but reduce scope proportionately
- D. transfer the project to another portfolio

*Rationale:* Alignment decays and gates exist to re-decide (2.1.3), on remaining cost and benefit (KA 2.4.2). A treats approval as permanent; C is an arbitrary compromise that decides nothing; D relocates the question without answering it.


**2.1-B** `[2.1.2 · Application]` A case states its constraint as "must use the existing platform", which reflects an architectural preference rather than a contractual or legal requirement. The correct treatment is:

- A. accept it as a constraint, since the architects have stated it
- B. record it as a **soft** constraint, so it can be traded in option generation and its cost made visible ✅
- C. remove it from the case entirely
- D. re-classify it as an objective

*Rationale:* Only hard constraints (physics, law, contract) are untradeable; a soft one recorded as hard silently eliminates options (2.1.2). C loses real information; D confuses a limit with a goal.


**2.2-A** `[2.2.2 · Application]` Full benefit potential is 979,200 per year; steady-state adoption is 70 %; the profile ramps 40 %/60 %/70 % and is appraised over 8 years at 7 % (`AF` = 5.971299). Against a flat full-potential case, the ramped NPV is lower by:

- A. USD 979,200
- B. USD 2,114,198 ✅
- C. USD 1,332,898
- D. USD 293,760

*Rationale:* Flat PV 5,847,096 less ramped PV 3,732,898 = **2,114,198** of overstated present value. C is the honest NPV itself; A is one year's potential; D is Domain 1's single-year output-based overstatement, a different figure.


**2.2-B** `[2.2.2 · Analysis]` Both the flat and ramped Meridian cases produce a positive NPV and the same approval decision. Why does the flat case still matter?

- A. it does not — the decision is unchanged, so the error is harmless
- B. because the case becomes the promise the programme is later judged against, and benefits that were never deliverable are recorded as failure ✅
- C. because the discount rate must be recalculated
- D. because the flat case understates cost

*Rationale:* The approval is identical; the *commitment* is not (2.2.2, and Domain 1's case study where Meridian was publicly called a failure). A is the reasoning that let the error survive.


**2.2-C** `[2.2.3 · Analysis]` Integration capacity of 3 units is the binding constraint. Meridian needs 3 units for NPV 1,693,072; Beta 2 for 1,200,000; Gamma 1 for 900,000. The value-maximising selection is:

- A. Meridian alone — the highest single NPV
- B. Beta + Gamma — combined NPV 2,100,000 within the 3-unit constraint ✅
- C. Meridian + Gamma — the two highest NPVs
- D. all three, phased across two years

*Rationale:* Beta and Gamma together fit the constraint and beat Meridian's 1,693,072. C needs 4 units; D changes the premise rather than answering under it; A ignores the constraint's implications.


**2.2-D** `[2.2.1 · Recall]` The clearest test that a business case is a decision instrument rather than an advocacy document is:

- A. whether it follows the corporate template
- B. whether it could have concluded "no" ✅
- C. whether it was approved
- D. whether its NPV is positive

*Rationale:* An options set and evidence that permit a negative conclusion are what make it a decision (2.2.1). A is the compliance failure mode; C and D are outcomes, not tests.


**2.3-A** `[2.3.1 · Analysis]` A benefits map runs directly from "system installed" to "USD 685,440 released per year". Its principal defect is:

- A. the benefit figure is too precise
- B. the **enabling change** is missing — training, workflow redesign and adoption support, largely owned outside the project, are what convert the output into the outcome ✅
- C. it should be expressed in hours rather than money
- D. the map should start with the strategic objective

*Rationale:* Omitting enabling change hides the work that determines realisation (2.3.1) — exactly how Meridian stalled at 40 % adoption with installation on plan. C is a separate (also real) question of unit; D is presentational.


**2.3-B** `[2.3.2 · Application]` A programme releases 6 clinician-hours per week per clinic; headcount is unchanged and the hours are absorbed by existing demand. Reporting this as a cash saving is:

- A. correct — released time has a value
- B. incorrect: it is a non-cash-releasing (capacity) benefit unless cost is reduced or the capacity is converted to valued activity ✅
- C. correct if the hourly rate is documented
- D. acceptable provided it is discounted

*Rationale:* Value released is not cash released (2.3.2); claiming otherwise is the error that discredits benefits cases. A and C mistake a valuation rate for a cash effect; D discounts a figure that was never cash.


**2.3-C** `[2.3.4 · Analysis]` An enabling platform programme and three dependent projects each claim the same USD 4m of downstream benefit. The portfolio total is:

- A. USD 16m, since each case is individually valid
- B. overstated by double-counting — one benefit, one claimant, with the enabler credited through the dependents or vice versa but not both ✅
- C. USD 4m, and the dependent projects have no benefits
- D. indeterminate until delivery completes

*Rationale:* Double-counting is the standing audit finding (2.3.2); the fix is a single claimant per benefit in the portfolio register. C overcorrects by denying the dependents any case; D defers a question answerable now.


**2.3-D** `[2.3.3 · Application]` A carbon reduction has no priced compliance consequence for the organisation. The soundest treatment in the case is:

- A. omit it, since it cannot be valued
- B. report it in its physical unit alongside the financial case, rather than monetising it with an invented price ✅
- C. monetise it using a rate found in a published study
- D. record it as a constraint

*Rationale:* Unpriced benefits are reported honestly in their own unit (2.3.3). A discards real value; C imports a price the organisation does not face; D confuses value with obligation.


**2.4-A** `[2.4.2 · Application]` 1,800,000 is spent; completion needs a further 900,000; remaining benefit PV is 780,000. The correct decision and its basis are:

- A. continue — stopping wastes the 1,800,000 already spent
- B. stop — forward NPV is (120,000); the sunk 1,800,000 is irrelevant to the remaining choice ✅
- C. continue — total spend 2,700,000 against total benefits exceeds the original case
- D. continue at reduced pace to spread the cost

*Rationale:* Only remaining cost and benefit bear on the decision (2.4.2). A is the sunk-cost fallacy stated plainly; C reintroduces sunk cost as "total"; D changes the schedule without changing the negative economics.


**2.4-B** `[2.4.3 · Analysis]` A portfolio's gates have never stopped a project in four years. The soundest inference is:

- A. selection is excellent, so no project has needed stopping
- B. the gates are not functioning as controls — no real stop option, evidence assembled to support continuation, or deciders too close to the cases ✅
- C. the gate criteria are too lenient and should be tightened numerically
- D. stopping is unnecessary if delivery is well managed

*Rationale:* A control that never fires is not demonstrably a control (2.4.3). A is implausible across a whole portfolio; C addresses thresholds when the defect is process and authority; D mistakes delivery quality for strategic validity.


**2.4-C** `[2.4.3 · Recall]` Kill criteria derive their power from:

- A. the severity of the thresholds
- B. being agreed in advance, so the same evidence produces a decision rather than a negotiation ✅
- C. being set by the project manager
- D. being confidential until invoked

*Rationale:* Advance agreement converts assessment into decision (2.4.3); without it, whoever has most invested wins the argument. C removes independence; D prevents the behavioural effect entirely.


## Domain 3

**3.1-A** `[3.1.1 · Comprehension]` Which statement best distinguishes governance from management?

- A. governance is performed by senior people and management by junior people
- B. governance grants, bounds and withdraws the authority within which management runs the work ✅
- C. governance is concerned with reporting and management with delivery
- D. governance applies to programmes and management to projects

*Rationale:* The distinction is about authority, not seniority, reporting or scale (3.1.1). C describes governance artefacts, which an organisation can have in full while having no governance.


**3.1-B** `[3.1.2 · Analysis]` A balanced matrix has no written precedence rule for conflicts between project and functional priorities. The most likely consequence is that authority:

- A. is shared evenly, as intended
- B. transfers to whichever manager escalates harder ✅
- C. defaults to the project leader
- D. defaults to the steering committee automatically

*Rationale:* Undefined precedence does not split authority; it awards it to escalation behaviour (3.1.2). The countermeasure is a precedence rule written before the conflict arises.


**3.1-C** `[3.1.3 · Application]` A monthly steering committee governs two-week sprints. The predictable failure is that:

- A. the team will produce lower-quality increments
- B. governance latency exceeds the cycle being governed, so the team either waits or proceeds and seeks retrospective approval ✅
- C. the committee will meet too often
- D. the sprints must be lengthened to a month

*Rationale:* The mismatch is one of latency against cycle time (3.1.3); the design response is a bounded envelope and cadence matching, not slower delivery.


**3.1-D** `[3.1.2 · Analysis]` In a three-party consortium, the governance provision most likely to produce paralysis under stress is:

- A. a defined majority for defined decision classes
- B. a short reserved-matters list
- C. unanimity for all substantive decisions ✅
- D. a deadlock-breaking mechanism with a deadline

*Rationale:* Unanimity is indistinguishable from an inability to decide once interests diverge (3.1.2); the other three are the countermeasures.


**3.2-A** `[3.2.3 · Application]` A committee meets every 6 weeks and closes papers 2 weeks before each meeting. The expected wait for a decision arising at a random point in the cycle is:

- A. 3 weeks
- B. 5 weeks ✅
- C. 6 weeks
- D. 8 weeks

*Rationale:* `E[wait] = M/2 + L = 6/2 + 2 = 5` weeks (3.2.3). A counts only half the interval and omits the paper lead time; C is the interval itself; D adds the whole interval to the lead time.


**3.2-B** `[3.2.3 · Analysis]` A committee meets every 4 weeks with a 2-week paper lead time. Two options are on the table: cut the paper lead time by one week, or cut the meeting interval by one week. Which reduces expected latency more, and by how much?

- A. cutting the meeting interval, by 1.0 week
- B. cutting the paper lead time, by 1.0 week — twice the 0.5-week saving from the interval ✅
- C. both, equally, by 1.0 week
- D. both, equally, by 0.5 weeks

*Rationale:* `E[wait] = M/2 + L` is 4.0 weeks initially; cutting `L` to 1 gives 3.0 (saves 1.0), cutting `M` to 3 gives 3.5 (saves 0.5) — a one-week cut in the paper lead time always saves a full week, a one-week cut in the meeting interval only half of one (3.2.3). The administrative deadline is also the cheaper lever to move.


**3.2-C** `[3.2.3 · Evaluation]` Raising Meridian's threshold from 10,000 to 25,000 saves 171,360 a year in delay. The band delegated comprises 12 changes worth 204,000. The strongest argument for the change is that:

- A. the project leader is experienced
- B. the delegate would have to destroy 84 % of the value of every delegated decision for the escalation to break even ✅
- C. the committee is too busy
- D. 12 changes is a small number

*Rationale:* The decisive argument is the breakeven value-destruction rate, which makes the comparison explicit and quantitative (3.2.3). A and D are assertions; C is a capacity argument, which is real but secondary.


**3.2-D** `[3.2.2 · Analysis]` A steering committee's effective authority on a given decision class is best described as:

- A. the authority of its chair
- B. the highest authority among its members
- C. the lowest authority among the members whose agreement is required ✅
- D. the authority delegated to the project leader

*Rationale:* One member who must consult before agreeing blocks the class, so the binding constraint is the minimum, not the maximum (3.2.2, Fault 1).


**3.2-E** `[3.2.1 · Analysis]` The most reliable test of whether a sponsor is an *advocate* sponsor rather than an effective one is whether they:

- A. attend every steering committee
- B. can state a condition under which they would recommend stopping the project ✅
- C. defend the project's funding
- D. know the project's schedule in detail

*Rationale:* An advocate sponsor is committed to the project rather than the outcome and therefore cannot stop it (3.2.1). C is an obligation of the role; D is closer to the operational failure mode.


**3.3-A** `[3.3.1 · Application]` A gate costs 45,000 in review effort and 6 weeks of elapsed time at a delay cost of 14,280 per week. Expected remediation cost with the gate is 82,800; without the gate it is 270,000. The gate's net value is:

- A. USD 141,480
- B. USD 56,520 ✅
- C. USD 85,680
- D. USD 187,200

*Rationale:* `270,000 − (45,000 + 85,680 + 82,800) = 56,520` (3.3.1). A omits the delay cost; C is the delay cost alone; D omits the remediation cost.


**3.3-B** `[3.3.1 · Evaluation]` For the gate above, the elapsed time at which it stops adding value is closest to:

- A. 6 weeks
- B. 8 weeks
- C. 10 weeks ✅
- D. 19 weeks

*Rationale:* `(270,000 − 45,000 − 82,800)/14,280 = 9.96` weeks (3.3.1) — the arithmetic behind the complaint that assurance has become an obstacle.


**3.3-C** `[3.3.3 · Application]` Three tiers must approve a decision: 2-weekly with 1-week papers, 4-weekly with 2-week papers, and 13-weekly with 3-week papers. Total expected latency is:

- A. 9.5 weeks
- B. 15.5 weeks ✅
- C. 19.0 weeks
- D. 6.0 weeks

*Rationale:* `(2/2+1) + (4/2+2) + (13/2+3) = 2.0 + 4.0 + 9.5 = 15.5` (3.3.3). A is the top tier alone; D counts only half the intervals and omits the paper lead times.


**3.3-D** `[3.3.2 · Analysis]` A PMO drafts the delivery plan and later provides second-line assurance on it. The defect is:

- A. duplication of first-line controls
- B. assurance capture — the function cannot challenge what it produced ✅
- C. a proportionality failure
- D. an assurance gap

*Rationale:* Capture is the most damaging and least visible line failure, because the product still looks independent (3.3.2).


**3.3-E** `[3.3.4 · Comprehension]` The decision-record field most often missing and most consequential when a decision is later examined is:

- A. the date
- B. the versioned reference to the information relied on ✅
- C. the decision-maker's role
- D. the decision reference number

*Rationale:* The retrospective question is whether the decision was reasonable on what was known at the time, which only a versioned reference can answer (3.3.4).


**3.3-F** `[3.3.4 · Analysis]` In a 12-class decision-rights matrix, 2 classes carry two Accountable roles and 1 carries none. Which statement is most accurate?

- A. the defect rate is 8.3 % and only the zero-A class matters
- B. the defect rate is 25.0 %; the two-A classes will be decided late or twice, and the zero-A class will drift until it becomes an escalation ✅
- C. the matrix is acceptable because 9 of 12 classes are correct
- D. the defect rate is 25.0 % and all three classes fail identically

*Rationale:* `3/12 = 25.0 %`, and the two failure types behave differently under stress, which is why they are distinguished rather than totalled (3.3.4).


## Domain 4

**4.1-A** `[4.1.1 · Comprehension]` The element of a charter whose absence most directly disables the project leader is:

- A. the list of high-level risks
- B. the statement of the leader's authority bounds ✅
- C. the milestone schedule
- D. the communication approach

*Rationale:* Without stated bounds every decision becomes an ask and authority is discovered incident by incident (4.1.1). The others are important and none of them confer power.


**4.1-B** `[4.1.2 · Analysis]` A programme's monthly report is produced three days after its steering committee's papers close. The consequence is that:

- A. the report is slightly late
- B. the committee will systematically consider last month's position, every month ✅
- C. the reporting cadence must be made weekly
- D. the paper lead time must be abolished

*Rationale:* The defect is a plan-consistency failure between the communication plan and the governance design's paper lead time (4.1.2), and it recurs for the project's whole life until the cadences are aligned.


**4.1-C** `[4.1.3 · Evaluation]` Which is *not* legitimately tailorable?

- A. the number of subsidiary plans maintained
- B. the depth of the schedule beyond the planning horizon
- C. the traceability of a baseline change to an approving authority ✅
- D. the frequency of progress reporting

*Rationale:* Traceability of baseline change to authority, the decision record, and legal or contractual obligations are outside tailoring (4.1.3).


**4.2-A** `[4.2.3 · Application]` A programme integrates 12 components. The number of possible point-to-point interfaces is:

- A. 12
- B. 66 ✅
- C. 132
- D. 144

*Rationale:* `n(n−1)/2 = 12 × 11/2 = 66` (4.2.3). A is the layered count; C counts each pair twice; D is `n²`.


**4.2-B** `[4.2.3 · Evaluation]` In WE 4.2.3, the strongest argument for the integration layer is:

- A. it saves 652,000 against the point-to-point design
- B. adding one further component costs 216,000 on a mesh and 18,000 layered — and programmes acquire components ✅
- C. it reduces the interface count from 66 to 12
- D. point-to-point interfaces are technically inferior

*Rationale:* The marginal cost of growth is decisive because it compounds, whereas the one-off saving is a single number on today's component count (4.2.3). D is not established and would be an assertion.


**4.2-C** `[4.2.1 · Analysis]` A WBS's five level-2 elements sum to 2,332,000 against an approved 2,400,000. The correct reading is that:

- A. the project has 68,000 of contingency
- B. the decomposition is incomplete, and the 68,000 will be consumed by whatever arrives first ✅
- C. the baseline should be reduced to 2,332,000
- D. the hundred-per-cent rule permits a 3 % tolerance

*Rationale:* Unallocated budget is a symptom of incomplete decomposition, not contingency (4.2.1); the rule admits no tolerance, and reducing the baseline would lock in the omission.


**4.2-D** `[4.2.2 · Comprehension]` Configuration management applies naturally to a product breakdown rather than a WBS because:

- A. products are more important than work
- B. configuration items are products, and activities do not have versions ✅
- C. a WBS changes more often
- D. product breakdowns are more detailed

*Rationale:* Versioning is a property of things, not of activities (4.2.2); applying configuration control to activities is one of the two standard confusions.


**4.3-A** `[4.3.1 · Analysis]` A schedule change is approved and the time-phased cost baseline is not updated. The most serious consequence is that:

- A. the cost baseline is slightly inaccurate
- B. earned value indices now measure the gap between two documents rather than the state of the work ✅
- C. the schedule must be re-baselined
- D. contingency is understated

*Rationale:* `SPI` and `CPI` compare achievement against the time-phased baseline; if it no longer matches the executing schedule they produce confident numbers about nothing (4.3.1).


**4.3-B** `[4.3.2 · Evaluation]` In a 340-item configuration audit, 28 items have no version reference, 11 have two current versions and 5 have a recorded version that differs from what is deployed. Which is the most serious class, and why?

- A. the 28, because they are the most numerous
- B. the 11, because ambiguity blocks decisions
- C. the 5, because verification against the register has verified nothing ✅
- D. all three are equally serious at a 12.94 % defect rate

*Rationale:* A register that states something untrue invalidates any verification performed against it, which is the class that produces "every component passed and the system failed" (4.3.2).


**4.3-C** `[4.3.3 · Application]` 34 changes averaging 6,800 direct cost were approved over a year, 14 of them carrying 0.3 weeks of critical-path impact at a delay cost of 14,280 per week. Total baseline drift is closest to:

- A. USD 231,200
- B. USD 291,176 ✅
- C. USD 59,976
- D. USD 376,856

*Rationale:* `34 × 6,800 = 231,200` direct plus `14 × 0.3 × 14,280 = 59,976` schedule impact (4.3.3). A omits the schedule impact; C is the schedule impact alone; D applies the schedule impact to all 34 changes rather than the 14 that carried it.


**4.3-D** `[4.3.3 · Evaluation]` For the drift above, a cumulative test of "related changes above 100,000 in a rolling 90 days" would:

- A. have caught it, since total drift exceeds 100,000
- B. not have caught it, since a quarter's changes aggregate to about 57,800 ✅
- C. have caught it only if the threshold were raised
- D. be inapplicable to changes below the delegation threshold

*Rationale:* The 90-day aggregate is `(34/4) × 6,800 ≈ 57,800`, below the threshold (4.3.3) — a cumulative test set at a round number without reference to the observed change rate has the appearance of a control and none of the function.


**4.3-E** `[4.3.3 · Comprehension]` The discipline that most protects re-baselining from abuse is:

- A. re-baselining no more than once a year
- B. keeping the original baseline visible in reporting alongside the current one ✅
- C. having the project leader approve it
- D. recalculating contingency at each re-baseline

*Rationale:* Visibility of the original preserves the ability to state performance against the original commitment (4.3.3). C is precisely the wrong authority.


**4.4-A** `[4.4.2 · Application]` A change is quoted at 40,000 direct. Assessment adds 2 weeks of critical-path impact at 14,280 per week, 22,000 of rework, 3 interfaces at 6,000 each, 14,000 of regression testing and 9,000 of documentation. The assessed total is:

- A. USD 40,000
- B. USD 103,000
- C. USD 131,560 ✅
- D. USD 109,560

*Rationale:* `40,000 + 28,560 + 22,000 + 18,000 + 14,000 + 9,000 = 131,560` (4.4.2). B omits the schedule impact; D omits the rework.


**4.4-B** `[4.4.2 · Evaluation]` A programme's delegation threshold is 25,000 on quoted direct cost. A change quoted at 22,000 carries 2 weeks of critical-path impact at 14,280 per week. The structural defect is that:

- A. the threshold is too low
- B. a change with an assessed impact of at least 50,560 is decided without escalation, because the threshold reads on the quoted figure ✅
- C. the change should be split into smaller changes
- D. the cost of delay should be excluded from change assessment

*Rationale:* The threshold's *basis* is the defect, not its level (4.4.2); the remedy is that it reads on assessed total impact.


**4.4-C** `[4.4.1 · Analysis]` Classifying a defect as a change results in:

- A. the baseline correctly reflecting the work
- B. the supplier being paid twice for the same obligation ✅
- C. faster resolution at no cost
- D. an unnecessary escalation

*Rationale:* A defect is work already owed under the baseline; treating it as a change adds budget for it (4.4.1). The mirror error — a change classified as a clarification — grows scope silently.


**4.4-D** `[4.4.3 · Comprehension]` The change-log entry type most often missing and most consequential is:

- A. the approval entry
- B. the rejection entry ✅
- C. the implementation confirmation
- D. the assessed cost

*Rationale:* An untraced rejection returns as a new request, is re-assessed at full cost, and may be approved by a different body in ignorance of the first decision (4.4.3).


**4.4-E** `[4.4.2 · Analysis]` Why is impact assessment systematically under-resourced?

- A. assessors lack the skill
- B. it costs money before any decision has been taken to spend money, so it competes with delivery and loses ✅
- C. change boards prefer quoted figures
- D. assessment adds no value to rejected changes

*Rationale:* The structural cause is that assessment is unfunded work preceding a decision (4.4.1, 4.4.2); the remedy is an explicit assessment budget.


## Domain 5

**5.1-A** `[5.1.1 · Comprehension]` A scope statement reads "implement a regional records system". Its principal defect is that:

- A. it is too short
- B. it is written in the activity register, so it has no boundary that can be tested ✅
- C. it does not name the sponsor
- D. it omits the schedule

*Rationale:* Activities have no boundary of their own, so completeness cannot be assessed (5.1.1). Length is not the defect — decidability is; C and D belong to the charter and the schedule.


**5.1-B** `[5.1.3 · Application]` A rollout element of USD 520,000 covers 40 approved sites. Eleven further sites believe they are included. The boundary exposure is:

- A. USD 13,000
- B. USD 143,000 ✅
- C. USD 60,000
- D. USD 188,496

*Rationale:* `520,000/40 = 13,000` per site, `× 11 = 143,000` (5.1.3). A is the cost of one site, not of eleven; C divides the whole 2,400,000 baseline by 40 rather than the rollout element, giving a per-site 60,000 — the wrong cost base, and a per-site figure rather than an exposure; D is the annual *benefit* of the 11 sites, not their cost.


**5.1-C** `[5.1.3 · Evaluation]` The 11 sites would cost USD 143,000 to include and generate USD 188,496 a year at realistic adoption. The correct professional conclusion is that:

- A. they should be excluded, because the exposure exceeds the tolerance
- B. the boundary statement's value is that it forces the decision while it is still cheap to act on — and here the decision is probably to include them ✅
- C. they are in scope already, since the statement says "all clinics in the region"
- D. the exposure should be added to contingency

*Rationale:* A nine-month payback makes exclusion value-destroying (5.1.3); the boundary statement's function is to surface the question in time, not to say no. C is the reading that causes the dispute; D funds an unmade decision.


**5.1-D** `[5.1.2 · Analysis]` A WBS dictionary consists only of element titles. The defect becomes visible:

- A. immediately, at baseline approval
- B. at acceptance, when the acceptance basis has to be invented by whoever is present ✅
- C. during estimating
- D. only if the scope changes

*Rationale:* The dictionary carries the acceptance basis; its absence is invisible until acceptance is attempted (5.1.2). That is what makes it an expensive defect rather than a documentation nicety.


**5.2-A** `[5.2.1 · Application]` On a ladder of 400 / 1,600 / 6,400 / 25,600 / 102,400 per defect by stage, an elicitation programme moves 13 build-stage, 9 test-stage and 3 live-service defects to the definition stage. The saving is:

- A. USD 620,800
- B. USD 610,800 ✅
- C. USD 526,800
- D. USD 1,286,400

*Rationale:* The saving is the **difference** at each stage: `13 × 6,000 + 9 × 25,200 + 3 × 102,000 = 610,800` (5.2.1). A uses the gross later-stage costs and forgets the defect still costs 400 to fix at definition; C is the saving net of the 84,000 investment; D is the unimproved total correction cost.


**5.2-B** `[5.2.1 · Evaluation]` The strongest single argument for the USD 84,000 elicitation programme is that:

- A. it returns 7.27 times its cost
- B. one defect moved out of live service saves USD 102,000 and pays for the whole programme ✅
- C. average correction cost falls from 13,400 to 7,037.50 per defect
- D. early detection is recognised good practice

*Rationale:* The single-defect argument is decisive because it needs no assumption about how many defects move (5.2.1); A and C are true but depend on the whole estimated shift, and D is an appeal to practice rather than a computation.


**5.2-C** `[5.2.3 · Analysis]` In a 480-requirement audit, 34 have no design element, 21 no test case, and 9 are recorded as accepted with no verification evidence. Which finding most undermines the **release decision itself**, and why?

- A. the 34, because unmet scope is the largest group
- B. the 21, because untested requirements reach live service
- C. the 9, because the acceptance record states something untrue, so every decision that relied on it rested on nothing ✅
- D. all three equally, at a combined 13.33 % defect rate

*Rationale:* A false record invalidates the decisions taken on it, including the release decision (5.2.3) — the same structural failure as Domain 4's mis-recorded configuration items. A ranks by group size and B by future correction cost; both are real findings and both leave the release record truthful, which is why neither is the answer to *this* question. D is the reading the class breakdown exists to prevent.


**5.2-D** `[5.2.3 · Comprehension]` Seventeen design elements trace to no approved requirement out of a 425-element register. This is:

- A. a forward traceability defect of 3.54 %
- B. a reverse traceability defect of 4.00 %, on a different denominator from the forward rate ✅
- C. immaterial, since the requirements are all still met
- D. evidence that 17 requirements were lost

*Rationale:* `17/425 = 4.00 %`, and the design register is not the requirements baseline, so the two rates are not combinable (5.2.3). A puts the 17 over the 480-requirement baseline (`17/480 = 3.54 %`) and then mislabels a reverse finding as a forward one — the wrong denominator and the wrong test. C ignores unrequested effort worth 108,800 at build-stage cost. D inverts the finding: an orphan is work nobody asked for, not a requirement that went missing.


**5.2-E** `[5.2.2 · Evaluation]` A requirement reads "the system shall provide faster record retrieval and improved reporting". Its defects are:

- A. it is untestable only
- B. it is ambiguous ("faster", "improved" — no referent) and compound (two requirements in one) ✅
- C. it is infeasible
- D. it is untraceable

*Rationale:* Comparatives without a referent make it ambiguous and therefore untestable, and the conjunction makes it two requirements that will be accepted in part and disputed in part (5.2.2).


**5.3-A** `[5.3.3 · Application]` With 20 development-weeks available and bundles A (13 wk, 299,000), B (10 wk, 228,000), C (10 wk, 225,000), D (5 wk, 110,000) and E (5 wk, 95,000), greedy ranking by value per week selects:

- A. A + D, worth 409,000 a year ✅
- B. B + C, worth 453,000 a year
- C. A + B, worth 527,000 a year
- D. B + D + E, worth 433,000 a year

*Rationale:* Greedy takes A first (23,000/week), then D, stranding 2 weeks — 409,000 (5.3.3). B is the enumerated optimum, not the greedy result; C exceeds capacity at 23 weeks; D is a feasible set greedy never reaches.


**5.3-B** `[5.3.3 · Analysis]` In that selection, greedy loses to enumeration because:

- A. value per week is the wrong measure
- B. the highest-ratio bundle is lumpy and strands 2 of 20 weeks that nothing can fill ✅
- C. bundle A's benefit is overstated
- D. the capacity constraint is not binding

*Rationale:* A's ratio advantage is under 1 % and its size wastes 10 % of capacity (5.3.3). The measure is not wrong; it is a heuristic, and the constraint is precisely what makes it fail.


**5.3-C** `[5.3.3 · Evaluation]` Before the enumeration above can be relied upon, the indispensable additional input is:

- A. a probability distribution on each benefit estimate
- B. the dependency map among bundles, since an infeasible set must not be selected ✅
- C. the sponsor's ranking preference
- D. a re-test of the must-have classification

*Rationale:* If C depends on A, `B+C` cannot be built and the answer is confidently wrong (5.3.3); dependency feasibility is a precondition, not a refinement. A and D are valuable and not indispensable.


**5.3-D** `[5.3.2 · Analysis]` 374 of 480 requirements are marked "must have". The consequence for prioritisation is that:

- A. the project is unusually well specified
- B. the discretionary pool is 106 requirements — 22.08 % — so prioritisation can act on a fifth of the scope, and the classification carries almost no information ✅
- C. the must-haves should be delivered first in ranked order
- D. capacity must be increased

*Rationale:* At 77.92 % must-have the category no longer discriminates; the re-test that asks for the consequence of omission raises the actionable pool to 332, a 3.13-fold increase (5.3.2).


**5.3-E** `[5.3.1 · Comprehension]` Regulatory and safety requirements should be:

- A. scored with the highest weight in the value model
- B. removed from the value ranking and held as constraints on capacity ✅
- C. prioritised last, since they generate no benefit
- D. attributed a nominal benefit for completeness

*Rationale:* They are not prioritisable; scoring them produces a statutory obligation ranking eleventh and forces the process to override itself, discrediting the ranking (5.3.1).


**5.4-A** `[5.4.1 · Application]` A baseline of 480 requirements received 12 approved additions; the traceability matrix at acceptance-test entry carries 531. Uncontrolled requirements average 4,200 direct, and 16 of them consumed 0.25 weeks of critical path each at a delay cost of 14,280 per week. Total creep cost is:

- A. USD 163,800
- B. USD 220,920 ✅
- C. USD 214,200
- D. USD 57,120

*Rationale:* `531 − (480 + 12) = 39` unexplained; `39 × 4,200 = 163,800` plus `16 × 0.25 × 14,280 = 57,120` gives 220,920 (5.4.1). A omits the schedule impact; C prices all 51 additions including the 12 that were properly approved; D is the schedule impact alone.


**5.4-B** `[5.4.1 · Evaluation]` Domain 4 established USD 291,176 of authorised baseline drift over the programme's first year, giving cumulative movement of USD 512,096 against a USD 2,400,000 baseline. The most important consequence of the creep figure is that:

- A. total movement is 21.34 % of the baseline
- B. the change log captures only 56.86 % of the movement it exists to control, and no cumulative test on that log can see the rest ✅
- C. the average crept requirement cost 5,664.62
- D. the delegation threshold should be lowered below 4,200

*Rationale:* Creep leaves no change record, so change-log instruments — including Domain 4's cumulative test — monitor a little over half the movement (5.4.1); the remedy is the count reconciliation. A and C are true and subordinate; D cannot work, since no change is ever raised.


**5.4-C** `[5.4.2 · Application]` Remediating 98 deficient criteria costs 320 each. A deficient criterion causes a dispute 25 % of the time, at an expected cost of 11,980 per dispute. The breakeven dispute probability is:

- A. 25.00 %
- B. 2.67 % ✅
- C. 9.36 %
- D. 20.42 %

*Rationale:* `31,360 / (98 × 11,980) = 2.67 %` (5.4.2). A is the observed rate; C is the return multiple; D is the share of the baseline that is deficient.


**5.4-D** `[5.4.3 · Evaluation]` Six severity-one items cost 6,400 each to fix before acceptance, with a 1.5-week go-live delay at 14,280 per week, or 102,400 each in live service. The result and its limit:

- A. defer; the go-live delay is unaffordable
- B. fix first — it costs 59,820 against 614,400, and the breakeven delay is 40.34 weeks — but the ladder must be applied by severity class, not to all 41 open items ✅
- C. fix first, and apply the same reasoning to all 41 open items
- D. the comparison cannot be made without the benefit forgone during the delay

*Rationale:* Fix-first is cheaper by 554,580 and would remain so up to a 40.34-week delay (5.4.3); applying the live-service figure to minor items would prevent go-live altogether. D double-counts — the cost of delay already prices forgone benefit (Domain 1).


**5.4-E** `[5.4.3 · Comprehension]` A programme reports 100 % of requirements verified and its benefits are 34 % of forecast. This is:

- A. a verification failure
- B. a validation failure — the specified thing was built, and it did not produce the outcome ✅
- C. a benefits-measurement error
- D. evidence of scope creep

*Rationale:* Verification asks whether the specified thing was built and validation whether it produces the outcome; the two fail independently (5.4.3), which is Case study B.


## Domain 6

**6.1-A** `[6.1.2 · Application]` Cabling may begin one week after civil works begin. The correct dependency is:

- A. FS+1
- B. SS+1 ✅
- C. FF+1
- D. FS−1

*Rationale:* The condition binds the two *starts* with a one-week lag: start-to-start plus one. A would wait for all civils to finish; C binds the finishes; D (a lead on FS) overlaps the finish, which is not what was stated.


**6.1-B** `[6.1.3 · Analysis]` A schedule shows 14 "must finish on" constraints, all on milestones reported to the board. The most accurate reading is:

- A. the schedule is well-governed, because board dates are protected
- B. the milestones will be met, because the tool will honour the constraints
- C. the schedule may no longer model reality — pinned dates can mask logic-driven slippage that float analysis would otherwise reveal ✅
- D. constraints are neutral scheduling hygiene with no analytical effect

*Rationale:* Pins override logic: the network can be slipping while pinned milestones hold still, hiding negative float until it is unrecoverable. A and B mistake suppression for control; D is false — every pin degrades the model's predictive value.


**6.1-C** `[6.1.2 · Application]` Survey P (4 wk) feeds report Q (6 wk). Under `FS+2` the chain completes week 12. Re-linked `SS+1`, it completes week:

- A. 7 ✅
- B. 11
- C. 12
- D. 5

*Rationale:* `ES(Q) = ES(P) + 1 = 1`, `EF(Q) = 7`. B subtracts one lag week from 12; C assumes logic changes nothing; D forgets Q's own duration must still run.


**6.1-D** `[6.1.3 · Recall]` A "dangle" in a schedule network is:

- A. an activity with negative float
- B. an activity missing a predecessor or successor link ✅
- C. a milestone with zero duration
- D. a lag longer than its predecessor

*Rationale:* Dangles are unbound activity ends — the passes cannot constrain them, so their dates and float are unreliable. A describes constraint-driven lateness; C describes every milestone; D is unusual but legal when justified.


**6.1-E** `[6.1.1 · Recall]` The schedule level that carries the logic, the floats and the critical path — and sits under formal change control — is:

- A. L1, because the board owns the schedule
- B. L2, because the PMO maintains it
- C. L3, the control schedule ✅
- D. L4, because it has the most detail

*Rationale:* The L3 control schedule is the analytical model; L1/L2 summarise it and L4 elaborates near-term execution from it. Detail (D) is not the same as control — L4 churns weekly by design and is never the baselined network.


**6.2-A** `[6.2.1 · Application]` In the Auriga network, activity D (duration 7, predecessor B finishing week 8, successors E and G) has `LS` = 9 and `ES` = 8. Its total float is:

- A. 0
- B. 1 ✅
- C. 8
- D. 7

*Rationale:* `TF = LS − ES = 9 − 8 = 1`. A confuses D with the critical activities; C is G's float; D is the activity's duration, not its float.


**6.2-B** `[6.2.2 · Analysis]` An activity shows `TF` = 1 and `FF` = 0. Delaying it by one week will:

- A. delay the project by one week
- B. delay nothing, because total float absorbs the slip
- C. delay at least one successor's earliest start while leaving the project end date unchanged ✅
- D. be impossible — free float can never be below total float

*Rationale:* Positive `TF` protects the end date (not A); zero `FF` means some successor moves immediately (not B). D inverts the invariant — `FF ≤ TF` always.


**6.2-C** `[6.2.3 · Application]` Auriga's procurement activity C is crashed from 8 weeks to 6. The new project duration is:

- A. 23 weeks
- B. 24 weeks ✅
- C. 25 weeks
- D. 21 weeks

*Rationale:* After one week of crashing, path B–D–E (8+7+5+4 via D) becomes co-critical at 24; the second crashed week buys nothing — duration stays 24. A assumes both weeks convert to project weeks; C forgets the crash entirely; D subtracts from the wrong baseline. (The full economics: KA 6.4.)


**6.2-D** `[6.2.1 · Application]` Auriga's training activity G runs ES 15–EF 17 with `LF` = 25. Its total float is:

- A. 8 ✅
- B. 10
- C. 2
- D. 0

*Rationale:* `TF = LF − EF = 25 − 17 = 8` (equivalently `LS − ES = 23 − 15`). B ignores G's duration (25 − 15); C is G's duration; D confuses G with the critical path.


**6.2-E** `[6.2.3 · Analysis]` A programme's binding path shows `TF` = −3 after a constraint-honest pass. The professional reading is:

- A. the software has malfunctioned — float cannot be negative
- B. the plan, as constrained, is already three weeks late; the number sizes the recovery problem and must be reported now ✅
- C. delete the constraints so the float returns to zero
- D. the project will finish three weeks early

*Rationale:* Negative float is the gap between what logic needs and what constraints allow — information, not error (A) and not good news (D). C is the pin-and-squeeze suppression this domain's Case B dissects; it hides the problem until it is unrecoverable.


**6.2-F** `[6.2.1 · Application]` Auriga's procurement C slips from 8 weeks to 9 (all else unchanged). The new project duration is:

- A. 25 weeks
- B. 26 weeks ✅
- C. 27 weeks
- D. 24 weeks

*Rationale:* C was critical with zero float, so its extra week passes straight through: E runs 17–22, F finishes week 26. A assumes float absorbed a critical activity's slip; C adds the week twice; D subtracts it.


**6.3-A** `[6.3.1 · Application]` A planner clears a resource peak by delaying an activity with `TF` = 1, `FF` = 0 by one week. The immediate consequences are:

- A. no schedule effect of any kind
- B. project delay of one week
- C. the end date holds, the activity's path float is exhausted, and at least one successor's earliest start moves ✅
- D. the resource peak worsens

*Rationale:* One week inside `TF` protects the end date (not B), but zero `FF` moves a successor and the path's buffer is now spent (not A) — the smoothing was legal but not free.


**6.3-B** `[6.3.3 · Analysis]` A 30-month programme shows daily-level tasks throughout, including month 30. The strongest inference is:

- A. the planning team is unusually diligent
- B. far-horizon detail is manufactured precision that will churn on every update, obscuring real variance ✅
- C. the schedule will need no re-baselining
- D. rolling-wave planning has been correctly applied

*Rationale:* Detail beyond the knowable horizon creates update churn and false confidence — the opposite of diligence in effect (A) and the opposite of rolling wave (D); constant churn makes re-baselining more likely, not less (C).


**6.3-C** `[6.3.1 · Application]` A hard 3-crew cap forces either a 4-week project extension (delay cost USD 45,000/week) or a second-shift waiver at USD 20,000/week for the same 4 weeks (end date held). The value-maximising choice and its saving are:

- A. extend: saves USD 100,000
- B. second shift: saves USD 100,000 ✅
- C. second shift: saves USD 25,000
- D. they cost the same

*Rationale:* Extension costs `4 × 45,000 = 180,000`; the shift premium `4 × 20,000 = 80,000` — choosing the shift saves USD 100,000 and keeps the date. A picks the dearer plan; C compares one week only; D ignores the 65,000-per-week difference... which compounds four times.


**6.3-D** `[6.3.3 · Recall]` A legitimate planning package in a rolling-wave schedule must carry:

- A. daily-level task detail for its whole span
- B. a ranged duration consistent with its estimate class and a dated elaboration event under change control ✅
- C. zero float, to keep pressure on the team
- D. a pinned finish date agreed with the sponsor

*Rationale:* Far-wave honesty is ranged duration plus a governed elaboration point. A is the manufactured precision rolling wave exists to avoid; C fabricates criticality; D is the typed milestone anti-pattern (Case study B).


**6.3-E** `[6.3.1 · Application]` D needs 3 crews in weeks 9–15 and concurrent staging needs 2; the site cap is 4. The excess demand the histogram must clear is:

- A. 1 crew for 7 weeks ✅
- B. 5 crews for 7 weeks
- C. 2 crews for 7 weeks
- D. nothing — 3 + 2 = 5 is within a 4-crew cap across two activities

*Rationale:* Peak demand 5 against cap 4 leaves one excess crew-week in each of the seven overlap weeks — the precise quantity smoothing must relocate. B is total demand, not excess; C is the staging demand itself; D misreads the cap as per-activity when it is per-site.


**6.4-A** `[6.4.2 · Application]` Saving a week is worth USD 45,000; crashing the critical activity costs USD 30,000 per week for up to two weeks; after one week of crashing, a parallel path becomes co-critical. The value-maximising decision is:

- A. crash two weeks (net +USD 30,000)
- B. crash one week (net +USD 15,000) ✅
- C. crash nothing (avoid all cost)
- D. crash two weeks and fast-track the parallel path at no cost

*Rationale:* Week one converts to a project week: +15,000. Week two is absorbed by the co-critical path: −30,000. A prices weeks that don't materialise; C leaves +15,000 unclaimed; D invents a free fast-track — overlap always carries rework risk.


**6.4-B** `[6.4.3 · Application]` An activity is estimated o = 4, m = 5, p = 12 weeks. Its PERT expected duration is:

- A. 5.0 weeks
- B. 7.0 weeks
- C. 6.0 weeks ✅
- D. 6.5 weeks

*Rationale:* `(4 + 4×5 + 12)/6 = 36/6 = 6.0`. A is the most-likely value (the mode); B is the unweighted mean of the three points; D miscounts the weighting.


**6.4-C** `[6.4.1 · Analysis]` A hybrid programme needs a software module from an agile team for an integration milestone. The schedule-sound way to join the two worlds is:

- A. impose the CPM date on the team as a sprint deadline
- B. enter the team's velocity-based forecast as a ranged duration and read the latest acceptable delivery from the backward pass ✅
- C. exclude the module from the network since agile work cannot be scheduled
- D. convert the agile team to predictive planning for the integration period

*Rationale:* B translates in both directions — evidence-based forecast in, latest-start requirement out. A dictates without evidence; C leaves the network blind at a convergence point; D destroys the team's delivery system to decorate the network.


**6.4-D** `[6.4.3 · Application]` At week 22, a programme has earned the value its baseline planned to earn by week 20. Its time-based schedule performance index `SPI(t)` is:

- A. 1.10
- B. 0.91 ✅
- C. 0.80
- D. 20.0

*Rationale:* `SPI(t) = ES/AT = 20/22 = 0.91` — the programme delivers at 91 % of planned tempo. A inverts the ratio; C subtracts the two weeks from the wrong base; D reports earned schedule itself, not the index.


**6.4-E** `[6.4.3 · Analysis]` Compared with quoting "78 % confidence of the date", giving the board three fully re-run schedule scenarios is stronger because:

- A. three numbers always beat one
- B. each scenario is an auditable network with named assumptions, showing how the date moves and where the path migrates ✅
- C. percentages are always statistically invalid
- D. scenarios eliminate the need for risk analysis

*Rationale:* The scenario's power is auditability and mechanism — assumptions with owners, visible path migration. A is numerology; C overclaims (a calibrated percentage is legitimate, Domain 8); D reverses the relationship — scenarios are inputs to risk analysis, not substitutes.


**6.4-F** `[6.4.3 · Application]` An activity is estimated optimistic 3, most-likely 4, pessimistic 8 weeks. Its PERT expected duration and standard deviation are:

- A. tₑ = 4.5, σ = 0.83 ✅
- B. tₑ = 4.0, σ = 0.83
- C. tₑ = 5.0, σ = 1.67
- D. tₑ = 4.5, σ = 5.0

*Rationale:* `tₑ = (3 + 16 + 8)/6 = 4.5`; `σ = (8 − 3)/6 = 0.83`. B reports the mode as the mean; C is the unweighted three-point average with a doubled spread; D confuses σ with the pessimistic-minus-optimistic range.


## Domain 7

**7.1-A** `[7.1.2 · Application]` A package is estimated o = 680,000, m = 750,000, p = 1,000,000. Its PERT expected cost is:

- A. USD 750,000
- B. USD 780,000 ✅
- C. USD 810,000
- D. USD 840,000

*Rationale:* `(680,000 + 4 × 750,000 + 1,000,000)/6 = 780,000`. A is the mode; C is the unweighted three-point mean; D over-weights the pessimistic value.


**7.1-B** `[7.1.3 · Analysis]` A project reports "on budget" while having consumed 60 % of its management reserve at 40 % complete. The correct reading is:

- A. genuinely on budget — management reserve exists to be spent
- B. the baseline is intact but the project's total funding is eroding faster than progress; the trend belongs in the next report to the sponsor ✅
- C. a baseline breach requiring immediate re-baselining
- D. an accounting error, since management reserve is inside the baseline

*Rationale:* Management reserve sits *outside* the baseline (so D is wrong and A is technically true but misleading), and its release is a sponsor-level signal, not a project-level convenience. It is not yet a breach (C) — it is the early warning that precedes one.


**7.1-C** `[7.1.1 · Recall]` Which statement about a bottom-up estimate summing to USD 4,183,662 is soundest?

- A. its precision indicates high accuracy
- B. its accuracy is bounded by its assumptions and definition maturity, and it must still carry a range and class ✅
- C. rounding it would reduce its accuracy
- D. bottom-up estimates do not need accuracy classes

*Rationale:* Precision (digits) and accuracy (closeness to outturn) are independent; definition maturity governs the latter. Rounding changes no information (C), and every estimate carries a class (D).


**7.2-A** `[7.2.1 · Analysis]` A project's `CPI` has read 1.02 for four months; then one month it drops to 0.91 with no change in productivity. The likeliest explanation is:

- A. genuine sudden inefficiency
- B. accruals were not being recognised, so earlier `AC` understated cost and this month absorbed the catch-up ✅
- C. the baseline was too generous
- D. `EV` was over-claimed this month

*Rationale:* A step change in `CPI` without an operational change points at measurement, and missing accruals are the classic cause — earlier periods flattered, one period punished. D would raise, not lower, the earlier readings' credibility, and C would show as a stable, not stepped, pattern.


**7.2-B** `[7.2.3 · Recall]` Which action preserves baseline integrity?

- A. moving budget from an underspent control account to cover an overspend, without record
- B. re-baselining through change control with an audit trail when scope genuinely changes ✅
- C. adjusting a completed package's budget to match its actual cost
- D. reducing remaining budgets so the total still equals `BAC`

*Rationale:* Only governed change preserves the measurement. A is a silent transfer, C is retrospective adjustment, D is the classic "make the numbers add up" manoeuvre — each destroys comparability.


**7.3-A** `[7.3.2 · Application]` `BAC` 4,000,000; `PV` 2,080,000; `EV` 1,920,000; `AC` 2,120,000. `CPI` and `SPI` are:

- A. 0.91 and 0.92 ✅
- B. 1.10 and 1.08
- C. 0.92 and 0.91
- D. 0.91 and 1.02

*Rationale:* `CPI = 1,920,000/2,120,000 = 0.91`; `SPI = 1,920,000/2,080,000 = 0.92`. B inverts both ratios; C swaps them (dividing `EV` by the wrong denominator); D miscomputes `SPI` against `BAC`-derived progress.


**7.3-B** `[7.3.3 · Analysis]` The overrun was caused by a one-off ground-remediation event, now closed, and the remaining work is expected to run to budget. The appropriate `EAC` is:

- A. `AC + (BAC − EV)` = 4,200,000 ✅
- B. `BAC/CPI` = 4,416,667
- C. `AC + (BAC − EV)/(CPI × SPI)` = 4,608,056
- D. `BAC` = 4,000,000

*Rationale:* A discrete, closed cause makes the variance **atypical**, so remaining work is forecast at the budgeted rate. B assumes the inefficiency persists and C that it compounds with schedule pressure — both contradict the stated cause. D ignores money already spent above budget.


**7.3-C** `[7.3.4 · Application]` With `BAC` 4,000,000, `EV` 1,920,000 and `AC` 2,120,000, the `TCPI` required to complete at `BAC` is:

- A. 0.91
- B. 1.00
- C. 1.11 ✅
- D. 1.21

*Rationale:* `(4,000,000 − 1,920,000)/(4,000,000 − 2,120,000) = 2,080,000/1,880,000 = 1.11`. A is the demonstrated `CPI`; B assumes recovery needs only par performance; D uses `PV` in place of `AC` in the denominator.


**7.3-D** `[7.3.1 · Analysis]` A control account is 70 % level-of-effort by budget. Its reported `SPI` of 1.00 most likely means:

- A. the account is exactly on schedule
- B. little about schedule: level of effort earns by the calendar, so `EV ≡ PV` for most of the account regardless of progress ✅
- C. the discrete work is ahead, offsetting a delay
- D. the earning rules were misapplied

*Rationale:* LOE sets `EV` equal to `PV` by construction, so a heavily-LOE account reads 1.00 whatever happens — which is why practice segregates and caps it. C invents an offset the data cannot show; the rules may have been applied entirely correctly (D), and that is the problem.


**7.4-A** `[7.4.3 · Application]` Target cost 2,000,000; target fee 150,000; share 70/30; actual cost 2,300,000. The seller's fee is:

- A. USD 150,000
- B. USD 60,000 ✅
- C. USD 90,000
- D. USD 45,000

*Rationale:* The seller absorbs 30 % of the 300,000 overrun: `150,000 − 90,000 = 60,000`. A ignores the incentive; C states the fee reduction rather than the fee; D applies the buyer's share to the fee.


**7.4-B** `[7.4.3 · Analysis]` Target cost 2,000,000, target fee 150,000, ceiling 2,450,000, buyer share 70 %. The `PTA` is 2,428,571, and its delivery significance is that above it:

- A. the contract becomes void
- B. the buyer absorbs all further cost
- C. the seller bears 100 % of further cost, so the incentive inverts and cost growth becomes a scope-change argument ✅
- D. the fee becomes negative but risk-sharing continues unchanged

*Rationale:* Beyond the `PTA` the ceiling binds the buyer, so every further dollar is the seller's — which predictably redirects the seller's effort from efficiency to entitlement. B reverses the exposure; A is fiction; D misses that sharing has *stopped*.


**7.4-C** `[7.4.2 · Analysis]` A leader lets an FFP contract for scope that is only 30 % defined. The most likely outcome is:

- A. cost risk is genuinely eliminated
- B. a priced-in risk premium plus a claims-and-variations exposure as the scope is defined ✅
- C. the seller absorbs all scope growth at no cost to the buyer
- D. the contract converts automatically to cost-plus

*Rationale:* Fixed price transfers risk *at a price* and only for the scope actually specified; undefined scope returns as variations. A and C mistake the contractual form for the underlying uncertainty; D invents a mechanism.


**7.4-D** `[7.4.1 · Application]` A pool of 40 at USD 95/h, 25 at USD 140/h and 15 at USD 210/h has a blended rate of:

- A. USD 148.33
- B. USD 130.63 ✅
- C. USD 115.00
- D. USD 140.00

*Rationale:* `10,450/80 = 130.63`. A averages the three rates unweighted; C weights toward the cheapest grade only; D takes the middle rate as representative.


## Domain 8

**8.1-A** `[8.1.2 · Analysis]` Which register entry best supports a decision?

- A. "Supplier risk — high"
- B. "Because the controller is single-sourced with volatile lead times, delivery may slip beyond the installation window, delaying commissioning and adding preservation cost" ✅
- C. "Delay to commissioning — probability 35 %"
- D. "Supplier may cause problems — owner: procurement"

*Rationale:* Only B names a cause to attack, an event to monitor and a consequence to size (8.1.2). C states a consequence with a probability but no cause, so no response can be designed; A and D support nothing at all.


**8.1-B** `[8.1.1 · Recall]` The ground contamination has been discovered and remediation is under way. In the register this is:

- A. a risk with probability 1.0
- B. an issue — it has occurred, and is managed rather than analysed ✅
- C. an opportunity, since remediation was funded
- D. removed entirely, with no further record

*Rationale:* Occurred risks become issues (8.1.1). A is the common fudge that clogs registers; D loses the audit trail and the lesson; C is nonsense.


**8.1-C** `[8.1.3 · Analysis]` A register of 60 items contains 3 opportunities. The soundest inference is:

- A. this project genuinely has few opportunities
- B. the identification process is framed defensively; opportunities must be asked for separately and sized the same way ✅
- C. opportunities do not belong in a risk register
- D. the register is too long and should be cut

*Rationale:* A 95 % threat ratio reflects process framing rather than reality (8.1.3). C contradicts the definition of risk; D addresses a different problem.


**8.2-A** `[8.2.2 · Application]` A risk has probability 0.15 and impact USD 400,000. Its `EMV` is:

- A. USD 400,000
- B. USD 60,000 ✅
- C. USD 340,000
- D. USD 26,667

*Rationale:* `0.15 × 400,000 = 60,000`. A is the impact; C is impact less `EMV`; D divides instead of multiplying.


**8.2-B** `[8.2.2 · Analysis]` R4 has the register's largest impact (400,000) but its smallest threat `EMV` (60,000). The correct managerial reading is:

- A. R4 should be ignored, having the lowest `EMV`
- B. `EMV` sets the funding priority, while impact still governs whether the event is survivable — both readings are needed ✅
- C. impact alone should drive priority
- D. the assessment must be wrong, since impact and `EMV` disagree

*Rationale:* `EMV` is the right basis for funding a portfolio of risks; a single large impact may still be existential regardless of probability (8.2.2, 8.3.1). A and C each discard half the information; D misunderstands that the two measure different things.


**8.2-C** `[8.2.3 · Application]` Proceeding directly has an expected cost of `0.40 × 300,000`. A USD 25,000 survey reduces the mitigated cost to USD 90,000. The value of the information is:

- A. USD 25,000
- B. USD 59,000 ✅
- C. USD 84,000
- D. nil — the survey costs more than it saves

*Rationale:* `120,000 − (25,000 + 36,000) = 59,000`. A is the survey's price, not its value; C omits the survey cost from branch B; D reverses the conclusion.


**8.2-D** `[8.2.4 · Analysis]` A register's `EMV` sum is 278,000 and its worst-case sum is 1,140,000. Setting contingency at 278,000 means:

- A. an appropriately funded reserve
- B. funding the average outcome, which by construction is exceeded roughly half the time ✅
- C. a conservative reserve, since not all risks will occur
- D. the same as a P80 reserve

*Rationale:* The mean is the ~50th percentile of the aggregate (8.2.4), so it is exceeded about half the time — the reason a confidence level is chosen explicitly. C mistakes the mean for conservatism; D confuses two different statistics (490,624 here).


**8.2-E** `[8.2.1 · Analysis]` Why must ordinal probability-impact scores not be multiplied and summed as money?

- A. the matrix is only for threats
- B. ordinal bands are ranks, not quantities — a "4" is not twice a "2", so the arithmetic is meaningless ✅
- C. multiplication requires more than five bands
- D. scores may be summed provided they are weighted

*Rationale:* Ordinal scales support ordering, not arithmetic (8.2.1). Weighting (D) does not repair a scale that never carried magnitude.


**8.3-A** `[8.3.1 · Application]` A mitigation costs USD 50,000 and reduces a risk's `EMV` from 84,000 to 20,000. The decision and its basis are:

- A. reject — 50,000 is a large outlay
- B. accept — the `EMV` reduction of 64,000 exceeds the 50,000 cost ✅
- C. accept — any reduction in `EMV` justifies a response
- D. indifferent — cost and benefit are equal

*Rationale:* Responses are investments: `84,000 − 20,000 = 64,000` of reduction for 50,000 of cost is value-creating. A prices the outlay without the benefit; C would justify unlimited spend; D miscomputes.


**8.3-B** `[8.3.1 · Analysis]` A risk has probability 0.03 and an impact the project could not survive. The correct treatment is:

- A. accept it — the `EMV` is small
- B. treat it structurally: avoid, transfer, or reconsider viability, because `EMV` funds portfolios while survivability is governed by impact ✅
- C. fund its `EMV` in contingency and monitor
- D. exclude it from the register as improbable

*Rationale:* Existential exposure is not an averaging problem (8.3.1). A and C both apply portfolio logic to a single point of failure; D removes the entry that most needs governance attention.


**8.3-C** `[8.3.2 · Analysis]` Contingency freed by a retired risk is used to cover an unrelated overspend. This is:

- A. efficient reserve management
- B. a governance failure: the reserve silently becomes a slush fund and the next genuine risk is unfunded ✅
- C. acceptable if the total baseline is unchanged
- D. required, since contingency is inside the baseline

*Rationale:* Contingency is tied to identified risks; reallocating it to overspends destroys the link between reserve and exposure (8.3.2). C is the reasoning that makes the failure invisible.


**8.4-A** `[8.4.2 · Application]` A programme's estimates have run 25 % low across four comparable past projects. The most effective countermeasure is:

- A. instruct estimators to be more careful
- B. reference-class forecasting — estimate from the distribution of comparable completed projects rather than from the plan ✅
- C. add a 25 % contingency and continue the same process
- D. escalate to the sponsor for a larger budget

*Rationale:* Optimism bias is systematic, so the fix is methodological (8.4.2). A relies on exhortation against a structural effect; C treats the symptom while preserving the cause; D funds it without correcting it.


**8.4-B** `[8.4.1 · Analysis]` A project has been optimised to eliminate all float and buffers. Its risk position is:

- A. improved — waste has been removed
- B. degraded — it now has no capacity to absorb the risks not in the register, and resilience is what covers those ✅
- C. unchanged, provided contingency is funded
- D. improved, since efficiency reduces exposure time

*Rationale:* Resilience assumes register incompleteness (8.4.1); removing all absorption capacity maximises fragility. C confuses money with time and capacity — funded contingency cannot buy back a schedule with nowhere to move.


**8.4-C** `[8.4.4 · Analysis]` An AI monitor has raised 40 alerts this month, 3 of which mattered. The correct response is:

- A. disable the monitor
- B. tune thresholds and require a calibration record, because unfiltered false positives erode the attention the tool exists to direct ✅
- C. investigate all 40 equally
- D. accept the ratio as inherent to anomaly detection

*Rationale:* False positives consume the scarce resource (attention) that detection is meant to focus (8.4.4). A discards real capability, C guarantees the erosion, D abandons the calibration duty.


**8.4-D** `[8.4.3 · Recall]` In the first hour of a crisis, the leader's priority order is:

- A. communicate, then investigate, then stabilise
- B. stabilise and secure safety, establish facts, then decide with a clock and communicate early ✅
- C. establish blame, then stabilise
- D. wait for complete information before acting

*Rationale:* Stabilisation and factual grounding precede decisions, which are taken against a clock (8.4.3). D is the failure the sequence exists to prevent; C is never a first-hour activity.


## Domain 9

**9.1-A** `[9.1.2 · Application]` A regime allows 80 defects to be introduced and detects 90 % before handover; internal correction averages 1,500 and an escaped defect costs 12,000. Prevention is 96,000 and appraisal 64,000. Total cost of quality is:

- A. USD 268,000
- B. USD 364,000 ✅
- C. USD 204,000
- D. USD 460,000

*Rationale:* `96,000 + 64,000 + (72 × 1,500) + (8 × 12,000) = 364,000` (9.1.2). A omits external failure; C counts nonconformance only and drops conformance; D applies the 12,000 external unit cost to all 80 introduced defects rather than to the 8 that escape.


**9.1-B** `[9.1.2 · Evaluation]` Moving to the next-stricter regime costs 90,000 more in conformance and removes 9,000 of internal failure and 4 escaped defects. At what external-failure unit cost does the stricter regime become worth buying?

- A. USD 12,000
- B. USD 22,500
- C. USD 20,250 ✅
- D. USD 81,000

*Rationale:* The step pays when `90,000 ≤ 9,000 + 4u`, so `u ≥ 81,000/4 = 20,250` (9.1.2). B divides the conformance cost by escapes avoided and forgets the internal-failure saving; D is the numerator, not the unit cost; A is the assumed unit cost, which is the thing being tested.


**9.1-C** `[9.1.2 · Analysis]` An organisation strengthens testing and its internal failure cost rises in the first period while total cost of quality falls. The correct reading is that:

- A. the testing programme is failing and should be reversed
- B. defects have moved from the external column to the internal one at a much lower unit cost ✅
- C. internal and external failure have been misclassified
- D. prevention spending was set too low

*Rationale:* Internal failure cost is not monotone in quality — better detection finds defects that were previously escaping (9.1.2). Reading the rise as deterioration is the standard reason such programmes are cancelled in their first period.


**9.1-D** `[9.1.1 · Comprehension]` A supplier delivers a product built exactly to a deliberately basic specification. It is best described as:

- A. low grade and low quality
- B. low grade and high quality ✅
- C. high grade and low quality
- D. not assessable until the client uses it

*Rationale:* Grade is the specified level; quality is conformance to it (9.1.1). D confuses quality with fitness for purpose, which is a third and separate test.


**9.1-E** `[9.1.2 · Evaluation]` Removing the last 6 escaped defects costs 129,000 more in total cost of quality, and each escape would have cost 12,000. The strongest professional statement is that:

- A. the spend is justified because defects should be eliminated
- B. the spend buys protection at 21,500 per defect, 1.79 times the harm it prevents, so it is not justified on these figures ✅
- C. the spend is justified because 129,000 is small against a 4,000,000 budget
- D. the comparison cannot be made without a Monte Carlo simulation

*Rationale:* `129,000/6 = 21,500` against a 12,000 consequence, a ratio of 1.79 (9.1.2). A is the zero-defects fallacy; C is affordability, not value; D over-reaches — the expected-value comparison is valid on stated averages, though a safety case would also require the tail to be examined.


**9.2-A** `[9.2.2 · Application]` Three containment layers have detection rates 0.50, 0.60 and 0.50. Of 80 introduced defects, how many escape?

- A. 8 ✅
- B. 3
- C. 12
- D. 27

*Rationale:* Escape fraction `0.50 × 0.40 × 0.50 = 0.10`, so `80 × 0.10 = 8` (9.2.2). B adds the detection rates to 1.60 and treats the chain as detecting everything with a remainder; C uses two layers only; D applies the *average* detection rate (0.5333) three times as if each layer saw all 80.


**9.2-B** `[9.2.2 · Analysis]` On a chain where 80 defects are introduced, internal failure costs 108,000 and external failure 96,000. The expected containment cost per introduced defect is:

- A. USD 1,500
- B. USD 2,550 ✅
- C. USD 12,000
- D. USD 2,000

*Rationale:* `(108,000 + 96,000)/80 = 2,550` (9.2.2). A is the average cost per defect found *internally*, omitting escapes; C is the escape unit cost; D divides internal failure alone by 80 and rounds — all three understate the breakeven price of prevention.


**9.2-C** `[9.2.2 · Evaluation]` USD 40,000 will either raise the last layer's detection rate from 0.50 to 0.75, or cut defects introduced from 80 to 60. On the figures of 9.2.2, the better choice and its reason are:

- A. raising detection, because it halves escapes from 8 to 4
- B. cutting introduction, because it removes 20 defects at an expected containment cost of 2,550 each — USD 51,000 of value for USD 40,000 ✅
- C. either, since both cost the same
- D. raising detection, because internal correction is cheaper than external failure

*Rationale:* Prevention returns 51,000 against 40,000 — a net 11,000 — and beats the appraisal option by 17,000 (9.2.2b). A counts escapes rather than cost and misses that raising late-layer detection moves work to a dearer layer, pushing internal failure from 108,000 to 122,000; D states a true premise that does not reach the conclusion.


**9.2-D** `[9.2.3 · Application]` A 4-week activity with 6 engineers has 20 engineer-weeks of first-pass content. Rework runs at 30 % of capacity. The activity will take:

- A. 4.00 weeks
- B. 4.76 weeks ✅
- C. 5.20 weeks
- D. 4.29 weeks

*Rationale:* `20 ÷ (6 × 0.70) = 4.7619` weeks (9.2.3). C adds 30 % to the 4-week plan, the linear error; D mishandles the allowance by scaling the 3.33 weeks of pure content; A ignores the overrun.


**9.2-E** `[9.2.3 · Analysis]` Why does the marginal cost of rework rise as the rework share rises?

- A. because rework is charged at a premium rate
- B. because the duration multiplier `1/(1 − r)` is convex, so equal increments of `r` add increasing amounts of elapsed time ✅
- C. because defects found later cost more to fix
- D. because float is consumed first and then lost

*Rationale:* The convexity is the mechanism: on Auriga's figures 10 %→20 % adds 0.4630 weeks while 40 %→50 % adds 1.1111 (9.2.3). C is the correction-cost ladder, a different effect (9.2.2); A and D are not general.


**9.2-F** `[9.2.2 · Analysis]` An automated check and a manual review are placed as consecutive containment layers, both driven from the same design document by the same team. The escape fraction computed as the product of their detection rates will be:

- A. correct
- B. too optimistic, because the layers are not independent detectors ✅
- C. too pessimistic, because two layers always find more than one
- D. correct only if their detection rates are equal

*Rationale:* The product form assumes independence; layers sharing a source, a method or a blind spot miss the same defects, so the true escape fraction is worse than the product (9.2.2). The countermeasure is to make consecutive layers methodologically different.


**9.3-A** `[9.3.2 · Application]` A sample of 20 items is drawn and none is defective. At 95 % confidence, the largest defective fraction consistent with that result is closest to:

- A. 0 %
- B. 5 %
- C. 13.9 % ✅
- D. 2.5 %

*Rationale:* `1 − 0.05^(1/20) = 0.1391` (9.3.2). A treats a clean sample as proof of conformance; B is the fraction a 59-item sample would bound; D halves the 5 % significance level.


**9.3-B** `[9.3.2 · Analysis]` Verifying one item costs 1,400; an escaped defective item costs 12,000. Above what defective fraction does verifying the whole population beat sampling?

- A. 8.57 %
- B. 11.67 % ✅
- C. it depends on the sample size
- D. 1.4 %

*Rationale:* `p* = c/u = 1,400/12,000 = 11.67 %`, and the `(N − n)` terms cancel so the breakeven is independent of `n` (9.3.2). A inverts the ratio; C is the intuition the algebra refutes.


**9.3-C** `[9.3.2 · Evaluation]` A 20-item zero-defect acceptance plan has a 95 % bound of 13.91 % and a breakeven defective fraction of 11.67 %. The most serious criticism of the plan is that:

- A. 20 items is too few to be statistically valid
- B. its confidence bound exceeds its own breakeven, so a clean sample cannot exclude the defect rate at which full verification would have been correct ✅
- C. it should use a one-defect acceptance number instead of zero
- D. sampling is inappropriate for safety-related work

*Rationale:* Self-consistency is the precise defect, repaired by raising `n` to 25 (9.3.2). A is an unquantified assertion; C would weaken the plan further; D is a different argument these figures do not establish.


**9.3-D** `[9.3.1 · Comprehension]` A delivery team decides to accept its own nonconforming output as fit for use without correction. The defect in that process is that:

- A. rework is always preferable to concession
- B. the authority for a concession belongs to the party that will bear the consequence, not the producer ✅
- C. concessions are never permissible
- D. the disposition should have been regrade

*Rationale:* A concession transfers a consequence, so its authority sits with whoever carries it (9.3.1). A and C overstate — concession is a legitimate disposition; D presumes a lesser purpose exists.


**9.3-E** `[9.3.3 · Application]` Removing a root cause costs 18,000; each recurrence costs 12,000. The strongest way to present the case is that removal:

- A. has a payback ratio of 3.33 times on five expected recurrences
- B. breaks even at 1.5 recurrences, so needs the cause to recur only twice ✅
- C. saves 60,000
- D. reduces the cause concentration from 62.5 %

*Rationale:* The breakeven recurrence count is the robust argument because it does not depend on forecasting the recurrence rate (9.3.3). A and C are true but rest on the five-recurrence estimate; D describes a measure, not a value.


**9.3-F** `[9.3.3 · Analysis]` Which statement identifies a root cause rather than an event description?

- A. the engineer loaded the wrong parameter set
- B. the device was delivered with a superseded configuration
- C. there was no controlled baseline against which a parameter set could be checked, and no process step that would have detected the mismatch ✅
- D. the site acceptance test did not detect the fault

*Rationale:* Only C names something within a manager's authority to change whose removal prevents the whole class (9.3.3). A stops where a person can be blamed; B and D describe the occurrence and a layer's miss.


**9.4-A** `[9.4.2 · Application]` Six sequential steps have first-time-right yields 0.95, 0.92, 0.90, 0.94, 0.88 and 0.85. The rolled throughput yield is closest to:

- A. 90.7 %
- B. 55.3 % ✅
- C. 44.7 %
- D. 85.0 %

*Rationale:* `RTY` is the product, 0.55307 (9.4.2). A is the arithmetic mean of the six yields, which describes nothing anyone experiences; C is the complement of the product; D is the worst single step.


**9.4-B** `[9.4.2 · Evaluation]` On that chain, improvement effort is best directed at the step with yield 0.85 rather than the one with 0.95 because:

- A. the 0.85 step is later in the chain
- B. raising 0.85 to 0.95 multiplies `RTY` by 0.95/0.85 and gains 6.51 points, against 1.75 points for raising 0.95 to 0.98 ✅
- C. late steps are always cheaper to improve
- D. the 0.95 step is already compliant

*Rationale:* The multiplicative form makes the gain proportional to the ratio of new to old yield, so the weakest step gives the largest lift — here 3.73 times as much (9.4.2). A confuses position with leverage; C is unsupported.


**9.4-C** `[9.4.2 · Analysis]` An end-to-end first-time-right target of 80 % across six sequential steps requires a per-step yield of about:

- A. 80.0 %
- B. 96.4 % ✅
- C. 93.3 %
- D. 88.9 %

*Rationale:* `0.80^(1/6) = 0.9635` (9.4.2). A applies the end-to-end figure per step; C divides 80 % by 6 and adds it back; D treats the losses as additive.


**9.4-D** `[9.4.3 · Application]` Six data quality dimensions score 0.960, 0.930, 0.980, 0.910, 0.990 and 0.995, and a record is fit for use only if it satisfies all six. Composite fitness is closest to:

- A. 96.1 %
- B. 78.4 % ✅
- C. 91.0 %
- D. 21.6 %

*Rationale:* The product is 0.7843 (9.4.3). A is the arithmetic mean; C is the weakest dimension; D is the complement of the product.


**9.4-E** `[9.4.4 · Evaluation]` Reviewing one AI-drafted item costs 40; an escaped erroneous item costs 1,800; a clean sample of 20 has been taken from 240 items. The defensible conclusion is:

- A. the sample is clean, so the output may be accepted
- B. the breakeven error fraction is 2.22 % while a clean sample of 20 bounds the rate only at 13.91 %, and reaching the breakeven needs 149 items — so full review at 9,600 is indicated ✅
- C. the sample should be increased to 59 items, bounding the rate at 5 %
- D. the output should not be used at all

*Rationale:* The bound must lie below the breakeven for the plan to support its own decision (9.4.4, applying 9.3.2). A accepts on a sample that cannot exclude 33 erroneous items; C bounds at 5 %, still more than double the breakeven; D is not supported — the arithmetic supports full review, not abandonment.


**9.4-F** `[9.4.4 · Analysis]` Which measure most raises the breakeven error fraction and so makes sampling AI output defensible?

- A. increasing the sample size
- B. adding an independent, cheap containment layer after the AI step and before execution, which lowers the cost of an escaped error ✅
- C. asking the model to check its own output
- D. improving the prompt

*Rationale:* The breakeven is `c/u`, so lowering `u` raises it (9.4.4 with 9.2.2). A changes the bound, not the breakeven; C is not an independent layer; D may lower the error rate but does not change the economics of verification.


## Domain 10

**10.1-A** `[10.1.2 · Application]` In-house provision costs 420,000 to stand up, 3,600 per unit and 60,000 to exit; buying costs 95,000 to transition in, 5,400 per unit and 145,000 to exit. The breakeven volume is:

- A. 66.67 units
- B. 133.33 units ✅
- C. 180.56 units
- D. 227.78 units

*Rationale:* `(480,000 − 240,000)/(5,400 − 3,600) = 240,000/1,800 = 133.33` (10.1.2). A divides the fixed-cost difference by the *make* unit cost instead of the unit-cost difference (`240,000/3,600 = 66.67`). C omits both exit costs (`325,000/1,800 = 180.56`). D assigns each option's exit cost to the other option (`410,000/1,800 = 227.78`).


**10.1-B** `[10.1.2 · Analysis]` At 84 units the make option costs 782,400 and the buy option 693,600, yet the make option's unit cost is 33.33 % lower. The correct reading is that:

- A. the unit costs must have been miscalculated
- B. the fixed-cost difference of 240,000 has not been recovered at this volume, so the decision is a bet on volume exceeding 133.33 units ✅
- C. unit cost is the more reliable comparison because it excludes one-off items
- D. the two options are equivalent because the difference is under 15 %

*Rationale:* The unit-price advantage must recover a fixed-cost disadvantage, and 84 units does not (10.1.2). C inverts the principle — excluding the one-off items is the error. D substitutes a tolerance for an answer.


**10.1-C** `[10.1.2 · Evaluation]` Standing the in-house capability up takes 9 weeks longer than mobilising the supplier, and the capability is on the critical path at a cost of delay of 45,000 per week. The effect on the breakeven volume is to move it from 133.33 units to:

- A. 133.33 units — elapsed time does not affect a cost comparison
- B. 225.00 units
- C. 358.33 units ✅
- D. 491.67 units

*Rationale:* `(480,000 + 405,000 − 240,000)/1,800 = 645,000/1,800 = 358.33` (10.1.2). A omits the delay entirely. B prices the delay alone and forgets the fixed-cost difference (`405,000/1,800 = 225.00`). D omits the buy option's fixed cost from the numerator (`885,000/1,800 = 491.67`).


**10.1-D** `[10.1.3 · Application]` A procurement chain has 12 weeks of process legs, 13 weeks of manufacture and delivery, and two approvals by a body meeting every 4 weeks with a 1-week paper deadline. Total lead time is:

- A. 25 weeks
- B. 29 weeks
- C. 31 weeks ✅
- D. 33 weeks

*Rationale:* Each approval adds `E[wait] = 4/2 + 1 = 3.0` weeks, so `12 + 13 + 6 = 31` (10.1.3, Domain 3 KA 3.2.3). A omits governance latency altogether; B counts only half of each interval and omits the paper deadlines; D adds the whole meeting interval twice.


**10.1-E** `[10.1.1 · Comprehension]` Which stage of the procurement lifecycle is the last at which the option "we should not buy this at all" remains available?

- A. define the need
- B. decide make-or-buy ✅
- C. choose the route to market
- D. tender and evaluate

*Rationale:* After the make-or-buy decision, subsequent stages choose *how* to buy, not *whether* (10.1.1). The stage with the most process — tender and evaluate — has the least remaining freedom.


**10.2-A** `[10.2.2 · Application]` Bids: Alpha 2,000,000 quality 62; Beta 2,200,000 quality 78; Gamma 2,480,000 quality 92. Price is scored `lowest ÷ own × 100`. At a 70/30 price/quality weighting the winner and score are:

- A. Gamma, 84.05
- B. Beta, 87.04
- C. Alpha, 88.60 ✅
- D. Alpha, 81.00

*Rationale:* `0.70 × 100 + 0.30 × 62 = 88.60`, against Beta 87.04 and Gamma 84.05 (10.2.2). A and B name the other bidders' correct scores but not the winner at this weighting; D is Alpha's score at a 50/50 weighting.


**10.2-B** `[10.2.2 · Analysis]` For the same three bids, the price weight at which Beta overtakes Gamma is closest to:

- A. 50.00 %
- B. 57.70 % ✅
- C. 60.78 %
- D. 63.77 %

*Rationale:* `w* = (92 − 78)/[(90.909091 − 78) − (80.645161 − 92)] = 14/24.263930 = 57.70 %` (10.2.2). A is the Beta/Gamma crossover under the *linear* normalisation convention, not the ratio convention. C is the Alpha/Gamma crossover, which lies inside Beta's winning band and is therefore not a boundary at all. D is the Alpha/Beta crossover.


**10.2-C** `[10.2.2 · Evaluation]` Gamma's price premium over Alpha is 480,000. On the panel's own risk mapping, Gamma's higher quality reduces expected integration rework from 96,000 to 32,000. The strongest statement a leader can make about the premium is that it is:

- A. justified, because Gamma scores 30 quality points higher
- B. 7.5 times the expected cost it avoids, so it must be justified by something the risk assessment does not capture — named and priced ✅
- C. unjustifiable in all circumstances
- D. justified, because 480,000 is only 24 % of Alpha's price

*Rationale:* `480,000/(96,000 − 32,000) = 7.5` (10.2.2). A and D restate inputs as conclusions. C overreaches: quality may buy safety, regulatory standing or capability the mapping omits — but those must be named, not assumed.


**10.2-D** `[10.2.3 · Analysis]` A panel opens the priced envelopes, then confirms the price/quality weighting at 40/60. The defect is that:

- A. 40/60 is too low a price weighting for a construction package
- B. the weighting should have been an odd split to avoid ties
- C. with the bids known, the weighting can be selected to produce any of three winners, so a model fixed after opening is not an evaluation method ✅
- D. the panel should have used the linear normalisation convention

*Rationale:* The bid set in 10.2.2 has three different winners across the weighting range, so post-hoc weighting selects the supplier (10.2.3). A and D are matters of judgement, not defects; B is invented.


**10.2-E** `[10.2.2 · Analysis]` Alpha's 2,000,000 bid is 10.18 % below the mean of the three bids but exactly equal to the buyer's own target cost. The correct professional response is to:

- A. disqualify Alpha as abnormally low
- B. ignore the test, since the bid matches the buyer's estimate
- C. seek a structured explanation of the price build-up, satisfy the panel that scope and risk are covered, and record the answer ✅
- D. average the two benchmarks and apply the threshold to the result

*Rationale:* The two benchmarks disagree, which is information rather than a tie to be broken; the response is clarification and a record, because an under-priced bid returns as variations (10.2.2, Domain 7 KA 7.4.2). D invents a procedure.


**10.3-A** `[10.3.2 · Application]` A supplier's cost outcomes are 1,850,000 (0.20), 2,000,000 (0.40), 2,300,000 (0.30) and 2,600,000 (0.10), and it requires a 150,000 expected margin. The risk-neutral firm fixed price is:

- A. USD 2,150,000
- B. USD 2,270,000 ✅
- C. USD 2,120,000
- D. USD 2,450,000

*Rationale:* `E[cost] = 2,120,000`, so the price is `2,120,000 + 150,000 = 2,270,000` (10.3.2). A adds the margin to the *target* cost rather than the expected cost — the commonest error, and it understates by 120,000. C is the expected cost with no margin. D is the ceiling of the target-cost alternative.


**10.3-B** `[10.3.2 · Analysis]` Under a target-cost contract with a 70/30 share, above the point of total assumption the buyer's and supplier's shares of the next dollar of cost are:

- A. 0.70 and 0.30, unchanged
- B. 0.00 and 1.00 ✅
- C. 1.00 and 0.00
- D. 0.50 and 0.50

*Rationale:* Above the `PTA` the ceiling binds the buyer, so the supplier carries every further dollar (Domain 7 KA 7.4.3; 10.3.2). A misses that sharing has stopped; C reverses the exposure; D invents a split.


**10.3-C** `[10.3.2 · Evaluation]` The expected buyer outturn is 2,270,000 under both firm fixed price and cost plus fixed fee, while the outturn standard deviation is 230,434 under cost-plus and zero under fixed price. The correct conclusion is that at risk-neutral pricing, fixed price:

- A. is cheaper in expectation and therefore always preferable
- B. costs the same in expectation and buys variance reduction, whose value depends on the buyer's ability to absorb variance — and on the supplier's solvency ✅
- C. is more expensive in expectation by the risk premium
- D. eliminates the cost risk from the project

*Rationale:* The premium equals the expected cost of the risk, so nothing is gained in expectation except certainty (10.3.2). D contradicts the marginal-dollar identity — the risk is allocated, not removed, and returns if the supplier fails.


**10.3-D** `[10.3.2 · Analysis]` As specified, the target-cost structure gives the buyer an expected outturn 48,000 below the fixed price and the supplier an expected margin 48,000 below its requirement. The most useful inference is that:

- A. the target-cost structure is superior for the buyer and should be used
- B. an incentive structure reallocates value unless it changes behaviour; the supplier will raise the target cost or fee, or decline ✅
- C. the supplier has mis-stated its cost distribution
- D. the share ratio should be 50/50

*Rationale:* The buyer's gain is exactly the supplier's loss, so no value is created (10.3.2). A treats a transfer as an improvement and will not survive negotiation; C is possible but not inferable; D is an unmotivated fix.


**10.3-E** `[10.3.3 · Evaluation]` Service credits are capped at 5 % of a 2,200,000 contract. Compliance costs the supplier 180,000 a year and non-performance costs the buyer 320,000 a year. The regime:

- A. deters non-performance, because 110,000 is a material sum
- B. prices non-performance at a 70,000 discount to compliance, and recovers only 34.38 % of the buyer's loss ✅
- C. is adequate because service credits are a secondary remedy
- D. should be capped at 5 % of the buyer's loss instead

*Rationale:* `0.05 × 2,200,000 = 110,000 < 180,000`, so failing is the supplier's cheaper course; and `110,000/320,000 = 34.38 %` of the buyer's loss (10.3.3). Deterrence needs 8.18 %, compensation 14.55 %.


**10.4-A** `[10.4.1 · Application]` Disruption is claimed on 1,204 planned hours whose productivity fell to 0.86 of the measured-mile baseline, at USD 130.625 per hour. The disruption cost is:

- A. USD 22,018.15
- B. USD 25,602.50 ✅
- C. USD 29,770.35
- D. USD 157,272.50

*Rationale:* Hours required `= 1,204/0.86 = 1,400`, so extra hours `= 196` and cost `= 196 × 130.625 = 25,602.50` (10.4.1). A multiplies the planned hours by `(1 − 0.86)` instead of dividing by 0.86 — 168.56 hours rather than 196, understating by USD 3,584.35. C applies the correct division to 1,400 hours instead of 1,204 (227.91 extra hours). D prices all 1,204 hours rather than the extra ones.


**10.4-B** `[10.4.1 · Analysis]` A USD 107,914.80 claim comprises labour and materials of 49,750 and prolongation and disruption of 46,602.50, each grossed up by 12 % overhead and profit. Notice was given on day 41 against a 28-day requirement, and the prolongation and disruption heads depend on notice. The amount at risk is:

- A. USD 46,602.50
- B. USD 52,194.80 ✅
- C. USD 55,720.00
- D. USD 107,914.80

*Rationale:* `46,602.50 × 1.12 = 52,194.80`, 48.37 % of the claim (10.4.1). A omits the overhead and profit attaching to those heads; C is the surviving amount; D assumes the whole claim falls, which the direct heads do not.


**10.4-C** `[10.4.2 · Evaluation]` A 400,000 claim can be settled for 220,000 plus 15,000 of costs. Arbitration has an expected award of 274,000 and irrecoverable costs of 340,000. The strongest argument for settling is that:

- A. the expected award of 274,000 exceeds the 220,000 settlement
- B. arbitration takes 78 weeks
- C. the irrecoverable cost of 340,000 exceeds the entire negotiated settlement of 235,000, so arbitrating cannot pay even on a total win ✅
- D. settlements are always cheaper than awards

*Rationale:* A actually argues *against* settling and is the comparison usually made; it omits the cost of obtaining the award. B is real but secondary. C is decisive: a total victory saves 235,000 and spends 340,000 (10.4.2). D is an unsupported generalisation.


**10.4-D** `[10.4.4 · Application]` Single sourcing costs 846,400 with a 0.18 probability of a 900,000 disruption. A qualified alternate adds 65,000 of certain cost and cuts the consequence to 295,000. The breakeven disruption probability is:

- A. 7.22 %
- B. 10.74 % ✅
- C. 18.00 %
- D. 22.03 %

*Rationale:* `p* = 65,000/(900,000 − 295,000) = 65,000/605,000 = 10.74 %` (10.4.4). A divides by the 900,000 consequence rather than the reduction; C restates the assumed probability; D divides by 295,000.


**10.4-E** `[10.4.4 · Analysis]` Two suppliers each have a 0.18 disruption probability, but both draw a critical module from one sub-tier source, so the joint probability is 0.12 rather than 0.0324. The effect on a dual-split sourcing case is that expected disruption cost:

- A. falls, because two suppliers are still better than one
- B. is unchanged, since the marginal probabilities are unchanged
- C. rises from 86,724 to 131,400, making the dual split worse than single sourcing ✅
- D. rises, but the dual split remains the best option

*Rationale:* `P(exactly one) = 2(0.18 − 0.12) = 0.12`, so expected cost is `0.12 × 195,000 + 0.12 × 900,000 = 131,400`, taking the total to 1,071,400 against single sourcing's 1,008,400 (10.4.4). B mistakes marginals for the joint distribution, which is the error the whole example exists to expose.


**10.4-F** `[10.4.3 · Evaluation]` A 124,800 supply-chain diligence programme avoids an expected 106,400 of exposure. The correct professional conclusion is that:

- A. the programme should be cancelled, since it fails an expected-value test
- B. the programme should be risk-tiered to raise effectiveness where probability is highest, the breakevens (82.11 % effectiveness, 9.38 % probability) stated, and any legal duty met regardless of the arithmetic ✅
- C. the consequence figure should be raised until the business case works
- D. expected monetary value is the correct basis for this decision

*Rationale:* The arithmetic allocates effort; it does not decide whether a legal or values obligation applies, and the consequence distribution is fat-tailed so its mean is the wrong statistic (10.4.3, Domain 8 KA 8.2.2). C is the manipulation the honest presentation of breakevens is designed to prevent.


## Domain 11

**11.1-A** `[11.1.1 · Comprehension]` The practical difference between a stakeholder list and a stakeholder system is that only the system:

- A. records contact details and communication preferences
- B. captures the relationships between parties, and so supports prediction of positions formed in conversations the project is not part of ✅
- C. is approved by the sponsor
- D. includes external as well as internal parties

*Rationale:* The system's added content is the relationships between members (11.1.1). A is administrative; C is governance; D is a completeness property a list can also have.


**11.1-B** `[11.1.2 · Application]` An objection causes 4.0 weeks of governance latency and 5 weeks of rework on the critical path at a cost of delay of 14,280 per week, plus three interface re-verifications at 18,000 each and 42,000 of application rework. The assessed total is:

- A. USD 96,000
- B. USD 128,520
- C. USD 167,400
- D. USD 224,520 ✅

*Rationale:* `9.0 × 14,280 + 54,000 + 42,000 = 224,520` (11.1.2). A is the direct cost alone; B is the delay alone; C omits the 4.0 weeks of governance latency and counts only the rework weeks.


**11.1-C** `[11.1.2 · Evaluation]` A 30-hour pre-consultation costing 3,300 would have addressed the objection above. The most persuasive argument to a sceptical sponsor is that:

- A. the consequence is 68.0 times the avoidance cost
- B. the pre-consultation pays whenever the probability of the objection exceeds 1.47 % ✅
- C. engaging regulators early is good practice
- D. 3,300 is immaterial against a 2,400,000 budget

*Rationale:* The breakeven probability forces the sceptic to assert something indefensible, whereas a ratio invites the answer "it might not have happened" (11.1.2). D is true and irrelevant, since immateriality is not a reason to spend.


**11.1-D** `[11.1.3 · Analysis]` Allocating engagement capacity in proportion to influence × interest systematically under-serves:

- A. the most powerful stakeholders
- B. the parties with the highest interest
- C. the user group that determines benefit realisation and the quiet party holding a veto ✅
- D. the project's own delivery team

*Rationale:* Salience measures power and attention, not value at stake or consequence of refusal (11.1.3) — which is why the design adds a value-led core and consent-risk floors.


**11.1-E** `[11.1.3 · Application]` A 320-hour adoption core raises adoption by 12.5 percentage points across 40 clinics, each adopting clinic being worth 24,480 a year. The benefit per engagement hour in year one is:

- A. USD 76.50
- B. USD 382.50 ✅
- C. USD 3,060.00
- D. USD 2,142.00

*Rationale:* `40 × 0.125 × 24,480 = 122,400`; `122,400 ÷ 320 = 382.50` (11.1.3). A divides one clinic's annual value by the 320 hours (`24,480 ÷ 320`); C divides the gain by the 40 clinics rather than by hours; D divides the whole 685,440 annual benefit by 320 hours instead of the uplift.


**11.2-A** `[11.2.1 · Application]` Fourteen parties each communicate directly with every other. The number of channels is:

- A. 14
- B. 91 ✅
- C. 182
- D. 196

*Rationale:* `n(n − 1)/2 = 14 × 13/2 = 91` (11.2.1, citing Domain 4 KA 4.2.3). A is the routed count; C counts each pair twice; D is `n²`.


**11.2-B** `[11.2.1 · Analysis]` At 1.5 hours per channel per month and 40 hours of engagement capacity, the largest number of parties an unrouted design sustains is:

- A. 7 ✅
- B. 8
- C. 14
- D. 26

*Rationale:* Seven parties give 21 channels and 31.5 hours; eight give 28 channels and 42.0 hours, which exceeds capacity (11.2.1). C is the actual party count, which is why the design was infeasible; D divides capacity by the unit cost and ignores the combinatorial term.


**11.2-C** `[11.2.2 · Application]` A monthly report has a 4-week period, 1 week of consolidation and a 2-week paper lead time. The mean age of the facts at the decision meeting is:

- A. 2.0 weeks
- B. 3.0 weeks
- C. 5.0 weeks ✅
- D. 7.0 weeks

*Rationale:* `C + L + P/2 = 1 + 2 + 2 = 5.0` weeks (11.2.2). B is the newest fact only; D is maximum blindness; A counts only the period's half-life.


**11.2-D** `[11.2.2 · Evaluation]` The strongest argument for cutting data weekly rather than monthly is that it:

- A. makes the pack look more current to the committee
- B. reduces maximum blindness from 7.0 to 2.5 weeks — 4.5 weeks, or USD 64,260 at the cost of delay ✅
- C. saves consolidation effort
- D. reduces the paper lead time

*Rationale:* Maximum blindness is the decision-relevant quantity and the redesign is arguable arithmetically (11.2.2). A is presentational; C is false, since more frequent cuts add consolidation events; D is a separate governance lever (Domain 3).


**11.2-E** `[11.2.3 · Analysis]` The root cause of amber compression is that:

- A. project leaders are optimistic by disposition
- B. the status is set by the person it evaluates, against undefined thresholds ✅
- C. boards react badly to red status
- D. reporting cycles are too long

*Rationale:* The corruption is structural — discretion plus undefined thresholds — so the remedy is pre-agreed objective thresholds applied mechanically (11.2.3). A and C are contributing conditions, not the cause.


**11.3-A** `[11.3.1 · Application]` A supplier's cost to serve a contract is 520,000 and the best alternative use of the same team would earn 95,000 of contribution. Its reservation value is:

- A. USD 425,000
- B. USD 520,000
- C. USD 615,000 ✅
- D. USD 95,000

*Rationale:* `520,000 + 95,000 = 615,000` (11.3.1). B omits the forgone contribution, which is the commonest error and would put the ZOPA floor 95,000 too low; A subtracts it.


**11.3-B** `[11.3.1 · Analysis]` With a ZOPA of 615,000–845,000, a buyer that fails to price its alternative and settles at 780,000 rather than the 730,000 midpoint has:

- A. captured 71.7 % of the zone
- B. transferred USD 50,000 of value to the seller through the absence of preparation ✅
- C. made an error of USD 165,000
- D. exceeded its reservation value

*Rationale:* `780,000 − 730,000 = 50,000`; the buyer's share falls to 28.3 % (11.3.1). A is the seller's share; C is the seller's surplus; D is false, since 780,000 is inside the zone.


**11.3-C** `[11.3.1 · Evaluation]` Pre-qualifying a second supplier for 25,000 cuts the buyer's reservation value from 845,000 to 754,880 and the midpoint settlement from 730,000 to 684,940. The correct reading is that the buyer is:

- A. worse off, because its surplus falls from 115,000 to 69,940
- B. better off by USD 20,060 in cash after the spend, and carrying USD 33,000 less risk ✅
- C. unaffected, since both reservation values moved
- D. better off by USD 45,060, ignoring the pre-qualification cost

*Rationale:* Surplus is measured against a reservation value the investment deliberately moved; cash paid and risk carried govern (11.3.1). A is the trap; D omits the 25,000.


**11.3-D** `[11.3.2 · Application]` Three issues are worth 84,000, 36,000 and 40,000 to the buyer and cost the seller 30,000, 60,000 and 12,000. Conceding half of each yields a joint gain of 29,000. Trading the two value-creating issues in full yields:

- A. USD 58,000
- B. USD 82,000 ✅
- C. USD 124,000
- D. USD 160,000

*Rationale:* `(84,000 + 40,000) − (30,000 + 12,000) = 82,000` (11.3.2). A trades all three issues, including the one whose joint value is −24,000, giving `160,000 − 102,000 = 58,000`; C is the buyer's value on the traded pair with no deduction of the seller's cost; D is the buyer's value across all three issues.


**11.3-E** `[11.3.3 · Analysis]` Two workstream leads are in open conflict; each has been given an objective the other's success would prevent. Facilitation has failed twice. The diagnosis is:

- A. interpersonal conflict requiring coaching
- B. structural conflict requiring a decision about objectives and authority ✅
- C. information conflict requiring a single authoritative source
- D. interest conflict requiring negotiation

*Rationale:* Incompatible assigned objectives are structural, and no facilitation resolves a structure (11.3.3) — the repeated failure of facilitation is itself the diagnostic.


**11.4-A** `[11.4.1 · Application]` A consent failure would cost 231,360. Its probability is 0.35 without consultation and 0.10 with a consultation costing 48,000. The consultation is worth:

- A. USD 57,840
- B. USD 9,840 ✅
- C. USD 80,976
- D. USD 23,136

*Rationale:* `0.35 × 231,360 = 80,976` against `48,000 + 0.10 × 231,360 = 71,136`; the difference is 9,840 (11.4.1). A is the breakeven consultation cost; C and D are the two `EMV` terms.


**11.4-B** `[11.4.1 · Evaluation]` For the same case, the figure a leader should present to a sceptical board is:

- A. the expected saving of 9,840
- B. the required probability reduction of 20.75 percentage points ✅
- C. the consequence of 231,360
- D. the consultation cost of 48,000

*Rationale:* The point estimate is the difference of two judged probabilities and is fragile; the required reduction states exactly what must be believed and can be answered from experience (11.4.1).


**11.4-C** `[11.4.2 · Analysis]` A programme translates all its material accurately but assumes that agreement reached in a meeting is a decision. In a consensus-forming counterpart organisation the predictable consequence is:

- A. the translation will be misunderstood
- B. agreement in the room, no decision, and the difference discovered weeks later ✅
- C. escalation will be faster than designed
- D. the counterpart will refuse to meet

*Rationale:* Decision convention, not language, is the expensive variable (11.4.2); the plan's approval durations are systematically understated as a result.


**11.4-D** `[11.4.3 · Application]` Ninety-six external communications a year, verification at 0.5 hours each and USD 110 an hour, against an assessed 4 % error rate at USD 6,000 per error. The return on the verification step is:

- A. 2.18 times
- B. 4.36 times ✅
- C. 0.23 times
- D. 3.12 times

*Rationale:* Exposure `96 × 0.04 × 6,000 = 23,040` against verification `96 × 0.5 × 110 = 5,280`, a ratio of 4.36 (11.4.3). A uses 1.0 hour per item rather than 0.5 (`23,040 ÷ 10,560`); C inverts the ratio (`5,280 ÷ 23,040`); D uses the 7,392 net hourly saving as if it were the verification cost.


**11.4-E** `[11.4.3 · Analysis]` An AI-drafted bulletin states a go-live month to which the programme has not committed. This is best treated as:

- A. a copy-editing error, corrected in the next issue
- B. a governance defect — an unauthorised commitment made outside the decision system ✅
- C. a supplier performance issue
- D. an acceptable risk given the drafting time saved

*Rationale:* The defect is the creation of an expectation and possibly a representation without authority, which is Domain 3's category of a decision taken without authority (11.4.3).


## Domain 12

**12.1-A** `[12.1.4 · Application]` A leader's 45.0-hour planning week contains 37.0 hours of recurring commitments. The share of the week available as discretionary attention, before any personal planning reserve, is:

- A. 14.4 %
- B. 17.8 % ✅
- C. 20.0 %
- D. 82.2 %

*Rationale:* `(45.0 − 37.0)/45.0 = 17.8 %` (12.1.4). A is the people-work share after the 1.5-hour reserve — the answer to a different question; C is the interruption load's share of the week; D is the committed share.


**12.1-B** `[12.1.1 · Analysis]` A project reports faithfully on plan for four months, then discloses a six-month overrun in one meeting. The failure this most strongly suggests is:

- A. inadequate management, because variance control was absent
- B. inadequate leadership, because the conditions for truthful upward information were absent ✅
- C. inadequate governance, because the committee met too rarely
- D. inadequate scope definition

*Rationale:* Compliant reporting is evidence that management existed; what was missing was the condition under which people report what the reports do not ask for (12.1.1, and Domain 3's escalation-lead-time indicator at 3.3.3).


**12.1-C** `[12.1.2 · Evaluation]` A delivery organisation proposes using a personality instrument to select project leaders. The soundest professional position is that the instrument:

- A. should be used, because it is widely adopted
- B. may be useful as a conversation aid inside a team, but its predictive claims are too weak to support a selection decision ✅
- C. should be used only for senior appointments
- D. is invalid and has no legitimate use

*Rationale:* Describing an individual difference is not explaining an outcome, so the legitimate use is internal dialogue, not selection (12.1.2). D overstates in the other direction, which is also a failure of judgement.


**12.1-D** `[12.1.2 · Application]` A competent engineer is put on genuinely unfamiliar work and does not ask for help. The situational diagnosis indicates that the leader should:

- A. leave them alone, since they are competent
- B. increase direction on this task while keeping the relationship supportive ✅
- C. reduce their scope permanently
- D. treat the silence as confidence

*Rationale:* The diagnosis is per person **per task** (12.1.2); competence on familiar work does not transfer, and the absence of a request for help is not evidence of not needing it.


**12.2-A** `[12.2.2 · Application]` A 12-person team works as a single group. Each pairwise relationship consumes 0.5 hours of total team time per week; people are productive 40 hours a week. Coordination overhead as a share of team capacity is:

- A. 3.4375 %
- B. 6.875 % ✅
- C. 13.75 %
- D. 82.5 %

*Rationale:* Paths `12 × 11/2 = 66`; overhead `0.5 × 66 = 33.0` h; capacity `40 × 12 = 480` h; `33.0/480 = 6.875 %` (12.2.2). A counts only one side of each relationship, halving the cost; C omits the division by two in `n(n − 1)/2`, counting each pair twice; D divides the 33 coordination hours by a single 40-hour week, attributing all coordination to the leader.


**12.2-B** `[12.2.2 · Analysis]` Under the same parameters, the team size at which one further member adds no net capacity at all is:

- A. 41
- B. 81 ✅
- C. 160
- D. 161

*Rationale:* Marginal capacity is `h − c(n − 1) = 40 − 0.5(n − 1)`, zero at `n` = 81 (12.2.2). A is the answer if the link cost is doubled to 1.0 hours; C mistakes `n − 1` = 160 for `n`; D is the size at which coordination consumes the team's *entire* gross capacity, a different and later point.


**12.2-C** `[12.2.4 · Evaluation]` A departure costs 26,612.50 in recruitment, 18,810.00 in ramp-up, and — in lost productivity — 4,180 of leaver disengagement, 1,567.50 of handover, and a 50 % overtime premium of 15,675 to cover a six-week vacancy worth 31,350 in engineer-weeks. The defensible total is:

- A. USD 26,612.50
- B. USD 45,422.50
- C. USD 66,845.00 ✅
- D. USD 82,520.00

*Rationale:* `26,612.50 + 18,810.00 + 21,422.50 = 66,845.00` (12.2.4). A counts recruitment only; B omits lost productivity; D substitutes the vacant post's 31,350 of pay for the 15,675 premium actually incurred — the saved-salary double count.


**12.2-D** `[12.2.2 · Evaluation]` Restructuring a 40-person team from one group into five pods of eight, each with a single representative in a coordination forum, changes coordination from 780 paths to 150. At 0.5 hours per path per week, the capacity released is:

- A. 630
- B. 7.875 FTE ✅
- C. 9.75 FTE
- D. 19.6875 FTE

*Rationale:* `0.5 × (780 − 150) = 315.0` hours a week `÷ 40 = 7.875` FTE (12.2.2). A is the reduction in paths; C is the mesh design's total coordination cost, not the saving; D is the saving in percentage points of capacity, mislabelled as FTE.


**12.3-A** `[12.3.2 · Application]` A leader has 6.5 hours a week of people-work capacity at six direct reports, and each additional report adds 0.5 hours to the leader's fixed load. Against a 45-minute weekly coaching convention, the maximum sustainable span is:

- A. 6 reports
- B. 7 reports ✅
- C. 8 reports
- D. 19 reports

*Rationale:* `n ≤ (6.5 + 0.5 × 6)/(0.75 + 0.5) = 9.5/1.25 = 7.6`, so 7 (12.3.2), giving 51.43 minutes each; 8 gives 41.25 and fails. C is the answer from the naive curve `6.5/n`, which ignores the growth of the leader's fixed load and overstates the span by one; D is the span at which coaching capacity reaches zero.


**12.3-B** `[12.3.1 · Analysis]` Delegating a task costs USD 430 more in direct labour over 12 weeks and releases 34.0 hours of the leader's time. The breakeven value of a released leader hour is:

- A. USD 10.24
- B. USD 12.65 ✅
- C. USD 185.00
- D. USD 430.00

*Rationale:* `430.00/34.0 = 12.65` (12.3.1). A divides by the leader's original 42.0 hours rather than the 34.0 released; C is the leader's charge rate, which is the value the released hour *may* have, not the breakeven; D reads the incremental cost as an hourly figure.


**12.3-C** `[12.3.1 · Evaluation]` The strongest argument for delegating in MCQ 12.3-B, given that a mishandled interface would cost USD 6,000 to re-verify and the released hours are worth USD 6,290 at the leader's rate, is that:

- A. the senior engineer is capable
- B. the leader is too busy
- C. the delegation fails only if the probability of a 6,000-dollar incident exceeds 97.67 % ✅
- D. delegation develops people

*Rationale:* The decisive argument is the breakeven error probability, `(6,290 − 430)/6,000 = 97.67 %` (12.3.1). A and D are assertions, however true; B is a capacity claim, real but not the comparison.


**12.3-D** `[12.3.3 · Analysis]` A leader raises a recurring lateness issue for the first time at its third occurrence. The principal cost of the delay is that:

- A. the person will be more defensive
- B. the conversation is now about a pattern, and therefore about the person, rather than about a single event ✅
- C. the leader has lost credibility with the team
- D. the record is now incomplete

*Rationale:* The first-instance rule is about what the available evidence permits you to discuss: one event supports a short factual conversation, a pattern forces a conversation about the person, which is the one that fails (12.3.3). A is a likely symptom of B rather than the cause.


**12.4-A** `[12.4.1 · Application]` Team A at UTC+3 and team B at UTC−5 both work 09:00–17:00 local. Their daily overlap is:

- A. 0.0 hours ✅
- B. 2.0 hours
- C. 3.0 hours
- D. 8.0 hours

*Rationale:* In UTC, A works 06:00–14:00 and B works 14:00–22:00, so `min(14:00, 22:00) − max(06:00, 14:00) = 0` (12.4.1). B results from taking the offset as 6 hours rather than 8; C is the overlap after the shift redesign; D compares local clock times without converting to a common reference — the error the topic exists to prevent.


**12.4-B** `[12.4.1 · Analysis]` Twenty-six critical-path clarifications a year each need three exchanges. At zero overlap each exchange costs one working day; the cost of delay is USD 2,856 per working day. Creating a three-hour overlap lets each clarification complete in one day. The annual saving is:

- A. USD 5,712
- B. USD 74,256
- C. USD 148,512 ✅
- D. USD 222,768

*Rationale:* `26 × 3 × 2,856 = 222,768` before and `26 × 1 × 2,856 = 74,256` after, so the saving is **148,512** (12.4.1). A is the saving per clarification; B is the post-fix cost; D is the pre-fix cost.


**12.4-C** `[12.4.1 · Evaluation]` Shifting the core team's day two hours later to create the overlap above reduces its overlap with a supplier from 5.5 hours to 3.5. The correct professional response is to:

- A. abandon the redesign, since it damages an existing relationship
- B. proceed, having computed the effect on every pair and judged 3.5 hours sufficient for one daily synchronisation ✅
- C. proceed without recomputing, since the adviser problem is larger
- D. ask the supplier to shift instead, since they are the supplier

*Rationale:* A distributed design must be computed as a whole, not fixed one pair at a time, and the trade must be a priced decision rather than a side effect (12.4.1). D allocates the burden by power rather than by reason, the failure 12.4.1 and 12.4.3 both identify.


**12.4-D** `[12.4.2 · Analysis]` A leader asks a distributed team "does everyone agree?" and receives agreement, then discovers a fortnight later that two members had serious objections. The best countermeasure is to:

- A. learn the national communication norms of the members concerned
- B. ask a named person for the strongest argument against the proposal, and confirm decisions in writing ✅
- C. require objections to be raised in the meeting
- D. reduce the team's size

*Rationale:* Changing the question and confirming in writing works regardless of which norm is operating, whereas a national generalisation cannot predict an individual (12.4.2). C restates the requirement that has just failed.


## Domain 13

**13.1-A** `[13.1.3 · Application]` Four epics carry costs of delay of 6,120, 4,080, 2,720 and 1,360 per week and efforts of 14, 6, 3 and 11 team-weeks respectively. The sequence that minimises total delay cost is:

- A. 6,120 first, then 4,080, 2,720, 1,360 — largest cost of delay first
- B. 2,720 first, then 4,080, 6,120, 1,360 — decreasing cost of delay per team-week ✅
- C. 2,720 first, then 4,080, 1,360, 6,120 — shortest effort first
- D. 1,360 first, then 6,120, 4,080, 2,720 — lowest density first

*Rationale:* Densities are 906.67, 680.00, 437.14 and 123.64, so the order is 2,720 → 4,080 → 6,120 → 1,360, costing **231,880** (13.1.3). A is the intuitive largest-first error, costing **276,080** — 44,200 worse. C ranks on effort alone, which promotes the low-value 1,360 epic ahead of the 6,120 one and costs **280,160**. D reverses the density rule and is the worst available order at **386,920**.


**13.1-B** `[13.1.3 · Evaluation]` For the same four epics, if no epic can be released until all four are complete at week 34, the total delay cost is:

- A. USD 231,880, unchanged — sequencing still helps
- B. USD 485,520, and sequencing is worth nothing ✅
- C. USD 155,040
- D. USD 253,640

*Rationale:* With a single release every epic completes at week 34, so the cost is `14,280 × 34 =` **485,520** whatever the order — the whole value of sequencing comes from releasability (13.1.3). C is the best-to-worst sequencing spread; D is the value of releasability itself, `485,520 − 231,880`.


**13.1-C** `[13.1.2 · Analysis]` A named product owner must obtain a steering committee's agreement before changing the backlog order. The most accurate description is that:

- A. the arrangement strengthens governance by adding scrutiny
- B. the effective decision right, and therefore the latency, belongs to the committee ✅
- C. the product owner remains accountable and the arrangement is sound
- D. the team should escalate less often

*Rationale:* Authority is where the binding agreement sits, so the product owner is a proxy and the committee's latency applies to every ordering decision (13.1.2, and Domain 3 KA 3.1.1's decidability test). A confuses scrutiny with authority; C mistakes the title for the right.


**13.1-D** `[13.1.1 · Analysis]` A programme adopts adaptive delivery but keeps its scope fixed by contract. The predictable consequence is that:

- A. delivery becomes faster because iterations are short
- B. it retains the coordination cost of the method and forgoes its benefit, which comes from not building the wrong thing ✅
- C. quality improves because increments are tested
- D. the cost of delay falls

*Rationale:* The economic case for adaptive delivery rests on varying scope in response to feedback; fixing scope removes the mechanism and leaves the overhead (13.1.1).


**13.1-E** `[13.1.4 · Comprehension]` A team, under schedule pressure, begins counting items as done with tests deferred. The measure that detects this soonest is:

- A. velocity, which will fall
- B. the count of items completed per iteration, which will rise
- C. the rework share of capacity in later iterations, which degrades throughput at `1/(1 − r)` ✅
- D. stakeholder satisfaction

*Rationale:* A relaxed definition of done initially *raises* reported completion and later shows as rework consuming capacity, at Domain 9 KA 9.2.3's multiplier (13.1.4). A and B move the wrong way; D is a lagging and non-specific signal.


**13.2-A** `[13.2.3 · Application]` A team completes 6 items a week with 18 items in progress. Its average cycle time is:

- A. 0.33 weeks
- B. 3.00 weeks ✅
- C. 6.00 weeks
- D. 108 weeks

*Rationale:* `C = W/T = 18/6 = 3.00` weeks (13.2.3). A inverts the ratio; C reads the throughput as a duration; D multiplies instead of dividing.


**13.2-B** `[13.2.3 · Analysis]` The same team is instructed to raise work in progress from 18 to 30 items. Each concurrent item above 18 costs 2 % of capacity. Throughput and cycle time become:

- A. 6.00 items a week and 5.00 weeks
- B. 4.56 items a week and 6.58 weeks ✅
- C. 4.56 items a week and 3.00 weeks
- D. 7.20 items a week and 4.17 weeks

*Rationale:* `T = 6 × (1 − 0.24) = 4.56` and `C = 30/4.56 = 6.58` weeks (13.2.3). A is the common error of holding throughput constant, giving `30/6 = 5.00`; C forgets that cycle time depends on both; D assumes work in progress raises throughput.


**13.2-C** `[13.2.4 · Application]` A 240-item backlog, sustained rates of 5.5 to 6.5 items a week, and measured discovery arrivals of 1.5 items a week. The honest forecast range is:

- A. 36.9 to 43.6 weeks
- B. 48.0 to 60.0 weeks ✅
- C. 40.0 weeks
- D. 24.0 to 30.0 iterations, reported as weeks

*Rationale:* Net drains are `6.5 − 1.5 = 5.0` and `5.5 − 1.5 = 4.0`, giving `240/5.0 = 48.0` and `240/4.0 = 60.0` (13.2.4). A ignores arrivals — the single most common forecasting error here, and it understates the date by 11 weeks. C is the naive mean-rate point estimate. D is the correct range expressed in two-week iterations and mislabelled as weeks, which halves it.


**13.2-D** `[13.2.4 · Evaluation]` For the same team, a plan commits to 34 weeks. The most useful single statement to put in front of the sponsor is that the plan requires:

- A. more effort from the team
- B. a gross throughput of 8.56 items a week, 31.7 % above the team's best sustained four-week rate ✅
- C. a throughput of 7.06 items a week
- D. the backlog to be reduced

*Rationale:* `240/34 = 7.06` net, plus 1.5 arrivals gives **8.56** gross, which is 31.7 % above the 6.5 best observed rate (13.2.4). C omits arrivals and so understates the requirement; A and D are responses, not the diagnosis.


**13.2-E** `[13.2.2 · Analysis]` Six iterations delivered 12, 10, 13, 13, 13 and 11 items, a mean of exactly 12. A team committed to 12 items an iteration will:

- A. meet the commitment, since 12 is the mean
- B. miss it in 33.3 % of iterations while performing exactly to its own average ✅
- C. miss it in 50 % of iterations
- D. exceed it in every iteration

*Rationale:* Two of the six totals are below 12, so the commitment fails a third of the time at unchanged performance (13.2.2). C assumes a symmetric distribution the data does not have; the resolution is to plan with the mean and commit with the low end of 10.


**13.2-F** `[13.2.3 · Comprehension]` A team's cycle time is 3.00 weeks and a typical item takes 0.9 weeks of active work. Flow efficiency is:

- A. 30.0 % ✅
- B. 70.0 %
- C. 3.33 %
- D. 333 %

*Rationale:* `0.9/3.00 = 30.0 %` (13.2.3). B is the waiting share; C and D invert the ratio. The low figure is normal and locates the improvement in waiting, not in working faster.


**13.3-A** `[13.3.2 · Application]` A team runs at 6 items a week with 18 in progress; 15 % of items need a committee decision with `E[wait] = 4.0` weeks. The work in progress parked waiting for a decision is:

- A. 1.80 items
- B. 3.60 items, 20.0 % of the team's work in progress ✅
- C. 0.90 items
- D. 2.70 items

*Rationale:* Blocked arrivals are `0.15 × 6 = 0.90` a week, and by Little's Law the blocked population is `0.90 × 4.0 = 3.60` items (13.3.2). A is the blocked items per iteration (`0.15 × 12`), not the parked population; C is the weekly arrival rate; D applies the 15 % share to the work in progress of 18 instead of to the arrival rate, which omits the wait entirely.


**13.3-B** `[13.3.2 · Analysis]` For the same team, average cycle time is 3.00 weeks. The cycle times of the unblocked and blocked populations are:

- A. 3.00 and 3.00 weeks
- B. 2.40 and 6.40 weeks ✅
- C. 3.00 and 7.00 weeks
- D. 2.55 and 6.55 weeks

*Rationale:* `Cu = 3.00 − 0.15 × 4.0 = 2.40`, and blocked items add the whole 4.0-week wait, giving 6.40 — a multiple of 2.667 (13.3.2). C adds the wait to the average rather than to the unblocked figure, double-counting the 0.60 weeks the blocked items already contribute; D uses the post-remedy average.


**13.3-C** `[13.3.2 · Evaluation]` Replacing the 4.0-week committee wait with a 1.0-week written-resolution route changes cycle time to 2.550 weeks. At unchanged work in progress of 18, the throughput effect is:

- A. none — throughput depends on capacity, not on cycle time
- B. an increase from 6.00 to 7.06 items a week, +17.6 % ✅
- C. an increase of 15.0 %, matching the cycle-time reduction
- D. an increase from 6.00 to 7.20 items a week

*Rationale:* `T = W/C = 18/2.55 = 7.0588`, an increase of 17.6 % (13.3.2). A forgets that Little's Law reads both ways; C confuses the proportional fall in `C` with the proportional rise in `T`, which are not equal because the relationship is reciprocal (`1/0.85 = 1.176`, not 1.15); D rounds the cycle time to 2.50 before dividing, and rounding before the division moves the answer by 0.14 items a week.


**13.3-D** `[13.3.4 · Analysis]` 96 of 240 items are done, averaging 1.2 size units, and the 144 remaining average 2.0. Reporting 40.0 % complete:

- A. is correct, since 96/240 = 40 %
- B. overstates progress by 11.4 percentage points, because optimal sequencing delivers the tractable items first ✅
- C. understates progress
- D. is correct provided the team's velocity is stable

*Rationale:* By size the release is `115.2/403.2 =` **28.6 %** complete (13.3.4). The direction of the error is systematic, not accidental: the density rule of 13.1.3 tells teams to do the cheap high-value work first, which guarantees that a count overstates.


**13.3-E** `[13.3.1 · Application]` Nine delivery streams must integrate. The reduction in potential pairwise interfaces from adopting an integration layer is:

- A. from 36 to 9 ✅
- B. from 81 to 9
- C. from 45 to 9
- D. from 36 to 18

*Rationale:* `n(n−1)/2 = 36` against `n = 9` (Domain 4, KA 4.2.3, cited at 13.3.1). B squares `n`; C uses `n(n+1)/2`.


**13.4-A** `[13.4.1 · Application]` A firm-price contract of 1,200,000 for 240 items prices additions at 7,500 and credits omissions at 3,000. The buyer adds 30 items and drops 30. The price movement is:

- A. zero — the change is net-zero in item count
- B. an increase of USD 135,000 ✅
- C. an increase of USD 225,000
- D. a decrease of USD 90,000

*Rationale:* `30 × 7,500 − 30 × 3,000 = 135,000` (13.4.1). A is the error the item count invites; C counts only the additions; D only the credit. The asymmetry between the two rates is where the exposure lives.


**13.4-B** `[13.4.1 · Evaluation]` The same change costs 57,120 in change-board latency, against 14,280 under a capacity model; the capacity model's worst-case quantity exposure is 600,000. The number of material changes at which the capacity model becomes cheaper is:

- A. 2
- B. 3
- C. 4 ✅
- D. it never becomes cheaper

*Rationale:* The per-change difference is `192,120 − 14,280 = 177,840`, and `600,000/177,840 = 3.37`, so the fourth change tips it: `1,200,000 + 4 × 192,120 = 1,968,480` against 1,800,000 (13.4.1). A and B are below the breakeven; against the *expected* capacity cost of 1,600,000 the breakeven is 2.25, which is a different and clearly-labelled comparison.


**13.4-C** `[13.4.2 · Analysis]` A buyer adopts a capacity-based contract and keeps its existing change-control process as the principal control. The defect is that:

- A. change control is unnecessary in adaptive delivery
- B. price is no longer the binding constraint, so the control must be the continuation decision and the value-envelope reconciliation ✅
- C. the iteration price should be renegotiated
- D. the supplier will slow down

*Rationale:* A capacity contract has no per-change price to control, so retaining change control leaves the quantity exposure unmanaged (13.4.2). A overstates — the change *record* still matters (Domain 3, KA 3.3.4); the missing controls are the brake and the value test.


**13.4-D** `[13.4.2 · Application]` A team delivers 12 items an iteration at an average size of 1.2 units. Sizing is inflated by 25 %. The reported velocity and the actual throughput become:

- A. 18.0 units and 15 items
- B. 18.0 units and 12 items — a 25 % reported improvement with no change in delivery ✅
- C. 14.4 units and 12 items
- D. 14.4 units and 15 items

*Rationale:* `12 × 1.5 = 18.0` units against an unchanged 12 items (13.4.2). This is why velocity is a planning input inside a team and not a performance measure: the team being measured sets the weights, and the inflation is invisible in the metric itself.


**13.4-E** `[13.4.4 · Analysis]` A supplier's reports show size units per iteration rising steadily while items completed per iteration are flat. The most likely diagnosis is:

- A. the team is improving
- B. sizing inflation under a velocity target ✅
- C. the definition of done has been relaxed
- D. work in progress has been raised

*Rationale:* Rising units with flat item throughput is the specific signature of sizing inflation (13.4.2, 13.4.4). A relaxed definition of done would show completion rising then reopened items rising; raised work in progress would show cycle time rising.


## Domain 14

**14.1-A** `[14.1.4 · Application]` A data class holds 900 records with a 6.0 % defect rate and an assessed consequence of USD 1,200 per defect. Its consequence-weighted exposure is:

- A. USD 54.00
- B. USD 72.00
- C. USD 64,800 ✅
- D. USD 1,080,000

*Rationale:* `900 × 0.06 × 1,200 = 64,800` (14.1.4). A is the defect count; B is the exposure *per record* (`dᵢuᵢ`), which is the remediation ranking key, not the class exposure; D omits the defect rate and prices every record as defective.


**14.1-B** `[14.1.4 · Evaluation]` Six data classes with a weighted mean defect rate of 2.3546 % carry a total exposure of USD 172,820. Imposing a uniform 2 % target on all six would produce 282 defects and an exposure of USD 207,440. The correct conclusion is that the uniform target:

- A. improves quality and reduces cost, since 282 is fewer than 332
- B. reduces the defect count by 15.06 % and raises expected cost by 20.03 %, because it loosens the low-rate, high-consequence classes and tightens the cheap ones ✅
- C. is acceptable because it is simpler to administer
- D. is equivalent to the observed position, since the mean rate is close to 2 %

*Rationale:* Cost is `defects × consequence`, and a rate target is blind to consequence (14.1.4). A counts defects and ignores their price; D confuses a mean rate with an expected cost.


**14.1-C** `[14.1.4 · Analysis]` Across six classes, remediation effort should be ranked by:

- A. defect rate `dᵢ`
- B. record count `nᵢ`
- C. exposure per record `dᵢuᵢ` ✅
- D. total class exposure `nᵢdᵢuᵢ`

*Rationale:* Remediation cost scales with records touched, so the return per record touched is `dᵢuᵢ` (14.1.4). D ranks by size of prize without regard to the effort of claiming it, and would direct effort to a large cheap class ahead of a small expensive one; A puts the lowest-rate, highest-consequence class last.


**14.1-D** `[14.1.2 · Comprehension]` Two systems report different "committed cost" for the same purchase order, and both values are correct within their own system. The defect is:

- A. a data accuracy failure
- B. a definition failure — the field has more than one meaning across systems ✅
- C. an interface failure
- D. a completeness failure

*Rationale:* Nothing is wrong with either value; the field has no single agreed definition (14.1.2), which is why the definition column of the data class schedule repays attention out of proportion to its cost. It will present as a reconciliation problem indefinitely until a meaning is chosen.


**14.1-E** `[14.1.3 · Analysis]` A programme stores every document in one repository with full version history, but item status is recorded in a separate spreadsheet and changing status requires no authorisation. Against the five CDE guarantees this arrangement fails on:

- A. single instance and retained history
- B. explicit state and controlled transition ✅
- C. access by role only
- D. nothing — it is a valid CDE

*Rationale:* Status is not a property of the item and transitions are unauthorised and unrecorded (14.1.3). Single instance and retained history are satisfied; the two failures are precisely the ones that make an environment unauditable.


**14.2-A** `[14.2.1 · Application]` A programme reports monthly with a 9-day production lag from data cut-off to publication. The expected age of a fact at publication is:

- A. 9 days
- B. 15 days
- C. 24 days ✅
- D. 39 days

*Rationale:* `R/2 + G = 30/2 + 9 = 24` days (14.2.1, applying Domain 3's `E[wait]` identity). A counts only the lag; B only half the period; D adds the whole period to the lag.


**14.2-B** `[14.2.1 · Analysis]` For that report, which change reduces information age more, and by how much: cutting the production lag by 3 days, or cutting the reporting period by 3 days?

- A. the reporting period, by 3.0 days
- B. the production lag, by 3.0 days — twice the 1.5-day saving from the period ✅
- C. both equally, by 3.0 days
- D. both equally, by 1.5 days

*Rationale:* Age is `R/2 + G`, so a cut of `x` in `G` saves `x` while a cut of `x` in `R` saves `x/2` — the same asymmetry Domain 3, KA 3.2.3 found for paper lead times, and the lag is usually the cheaper lever to move.


**14.2-C** `[14.2.4 · Application]` An automation costs USD 79,600 over the horizon. Manual unit cost is USD 52.25 and automated USD 4.25; escape rates are 1.5 % manual and 2.5 % automated with an escaped error costing USD 1,800. The quality-adjusted breakeven volume is closest to:

- A. 959 units
- B. 1,659 units
- C. 2,654 units ✅
- D. 1,524 units

*Rationale:* Adjusted unit costs are `52.25 + 27.00 = 79.25` and `4.25 + 45.00 = 49.25`, so `79,600/30.00 = 2,653.33 → 2,654` (14.2.4). A uses the build cost alone with unadjusted units; B is the naive figure including maintenance; D divides the fixed cost by the manual unit cost instead of by the saving.


**14.2-D** `[14.2.4 · Evaluation]` An automation's quality-adjusted automated unit cost exceeds its quality-adjusted manual unit cost. The correct conclusion is that:

- A. the breakeven volume is very large but attainable at portfolio scale
- B. no volume justifies the automation; the breakeven volume does not exist ✅
- C. the automation is justified if the visible unit saving is positive
- D. the escape rates should be excluded as they are estimates

*Rationale:* With a negative unit saving every additional unit adds loss, so there is no crossover (14.2.4) — the position Case study B describes. C is exactly the error that produces it; D discards the term that decides the answer.


**14.2-E** `[14.2.3 · Application]` A digital twin costs USD 211,500 and would deliver USD 306,000 of benefit at perfect fidelity. Its breakeven fidelity is:

- A. 58.82 %
- B. 69.12 % ✅
- C. 85.00 %
- D. 113.71 %

*Rationale:* `211,500/306,000 = 69.12 %` (14.2.3). A divides the build cost alone by the benefit, omitting nine months of synchronisation; C is the measured fidelity, not the breakeven; D divides by one benefit stream only.


**14.3-A** `[14.3.3 · Application]` Tier 2 costs USD 44.00 per item with a detection rate of 0.70; tier 3 costs USD 121.00 with 0.90. The measured material-error rate is 0.12. The consequence at which tier 3 begins to pay is:

- A. USD 712.96
- B. USD 1,120.37
- C. USD 3,208.33 ✅
- D. USD 5,041.67

*Rationale:* `u* = Δv/(p·Δq) = 77/(0.12 × 0.20) = 3,208.33` (14.3.3). A divides the increment by the *total* detection rate; B divides the total cost by the total detection rate; D divides the total cost by the increment. Only the increment-over-increment form answers "is the next tier worth it".


**14.3-B** `[14.3.3 · Evaluation]` A tiered standard costs USD 12,650 in verification with USD 24,300 of expected escaped loss. Uniform tier 2 on the same outputs costs USD 41,404 with USD 21,771 of escaped loss. The best characterisation is that uniform tier 2:

- A. is safer, since escaped loss is lower
- B. spends 3.27 times as much on verification to reduce escaped loss by 10.41 %, and is USD 26,225 worse in total ✅
- C. is equivalent, since both are defensible policies
- D. is cheaper, because one policy is simpler to administer

*Rationale:* Compare totals: 36,950 against 63,175 (14.3.3). A looks at one term of two; the extra review lands mostly on items whose errors are cheap, which is the signature of an undifferentiated policy.


**14.3-C** `[14.3.3 · Analysis]` The measured material-error rate for a class falls from 0.12 to 0.04. Every verification threshold:

- A. falls by a factor of three
- B. rises by a factor of three ✅
- C. is unchanged, since the tiers' costs and detection rates are unchanged
- D. rises by a factor of nine

*Rationale:* `u* = Δv/(p·Δq)` is inversely proportional to `p` (14.3.3), so thresholds triple and more classes fall into lighter tiers — which is where the productivity gain of a better configuration is actually realised. C confuses the tier definitions with the threshold.


**14.3-D** `[14.3.4 · Analysis]` An obvious error occurs on 10 % of cycles, costs USD 900,000 if it escapes and is detected with probability 0.98. A plausible error occurs equally often, costs USD 270,000 and is detected with probability 0.25. Which is more dangerous, and by how much?

- A. the obvious error, by 3.33 times, because its consequence is larger
- B. the plausible error, by 11.25 times ✅
- C. the plausible error, by 37.50 times
- D. they are equally dangerous once probability is taken into account

*Rationale:* Expected escaped costs are `0.10 × 0.02 × 900,000 = 1,800` and `0.10 × 0.75 × 270,000 = 20,250` (14.3.4). C is the escape-probability ratio alone, which would be the answer only at equal consequence; A ranks by consequence and ignores detectability.


**14.3-E** `[14.3.4 · Application]` The control that most raises the detection rate for plausible numerical error in an AI-assisted forecast is:

- A. a reasonableness check against expectation
- B. reperformance from the inputs by an independent route ✅
- C. asking the model to check its own output
- D. increasing the number of reviewers reading the same document

*Rationale:* A plausible error offers nothing to notice, so a plausibility check is near-worthless against it (14.3.4). C is not an independent layer (Domain 9, KA 9.2.2); D multiplies the same ineffective check.


**14.4-A** `[14.4.2 · Application]` Of 24 urban clinics, 9 were at risk and 8 were flagged; of 16 rural clinics, 11 were at risk and 6 were flagged. Rural recall is:

- A. 37.50 %
- B. 54.55 % ✅
- C. 68.75 %
- D. 70.00 %

*Rationale:* `6/11 = 54.55 %` (14.4.2). A divides flagged by all rural clinics; C is the rural base rate `11/16`; D is the aggregate recall `14/20`, which describes neither group.


**14.4-B** `[14.4.2 · Evaluation]` Lowering the rural flagging threshold would raise rural recall from 6 of 11 to 10 of 11, adding two false positives at USD 1,900 each and avoiding four emergency remediations whose excess cost is USD 4,500 each. The correct conclusion is:

- A. it should not be done, because false positives rise
- B. it is worth USD 14,200 net, a return of 4.74 times the extra cost ✅
- C. it is worth USD 18,000, the emergency cost avoided
- D. it cannot be evaluated economically

*Rationale:* `4 × 4,500 − 2 × 1,900 = 14,200`, and `18,000/3,800 = 4.74` (14.4.2). C omits the extra support cost; the differential here was a defect, not a price paid for efficiency.


**14.4-C** `[14.4.4 · Analysis]` Control A cuts incident probability from 0.08 to 0.02 at USD 26,000 a year; control B cuts impact from USD 480,000 to USD 260,000 at USD 14,000 a year. Which option gives the lowest total position?

- A. both controls, total USD 45,200
- B. control A alone, total USD 35,600
- C. control B alone, total USD 34,800 ✅
- D. neither, total USD 38,400

*Rationale:* Totals are control cost plus residual `EAL` (14.4.4). Both controls cost more than they avoid together because their reductions multiply — the sub-additivity result — so A is the trap the example exists to expose.


**14.4-D** `[14.4.4 · Evaluation]` For those two controls, the individual avoided losses are USD 28,800 and USD 17,600. The combined avoided loss is:

- A. USD 46,400
- B. USD 33,200 ✅
- C. USD 28,800
- D. USD 38,400

*Rationale:* Combined residual `EAL` is `0.02 × 260,000 = 5,200`, so avoided is `38,400 − 5,200 = 33,200` (14.4.4). A adds the two individual figures and overstates by USD 13,200, which is the error that converts a value-destroying package into an attractive one.


**14.4-E** `[14.4.1 · Analysis]` A supplier not shortlisted asks why. The applicable explainability requirement is best met by:

- A. disclosing the model's feature importances
- B. the evaluator's recorded reasoning against the published criteria, with any AI output identified as one input ✅
- C. a statement that the process was automated and consistent
- D. re-running the model with the supplier present

*Rationale:* The explanation must be in the vocabulary of the obligation — the published criteria — and the accountable human's reasoning is what is owed (14.4.1, with Domain 10, KA 10.2). A explains a model, not a decision; C is the answer that converts a substantive objection into a legitimacy grievance.


## Domain 15

**15.1-A** `[15.1.3 · Application]` A programme milestone requires four independent predecessors, assessed at 0.95, 0.90, 0.90 and 0.85. The probability the milestone is met is closest to:

- A. 0.90
- B. 0.85
- C. 0.65 ✅
- D. 0.60

*Rationale:* `0.95 × 0.90 × 0.90 × 0.85 = 0.654075`, so 0.65 (15.1.3). A is the arithmetic mean of the four, the commonest error; B is the minimum, the second commonest; D subtracts the summed shortfalls from one (`1 − 0.40`), which treats the shortfalls as additive.


**15.1-B** `[15.1.3 · Analysis]` Meridian's Region A go-live has six independent predecessors giving a milestone probability of 52.95 %. Four structurally identical regions must go live simultaneously. The programme milestone probability is:

- A. 52.95 %, because the regions are identical
- B. 13.24 %, being 52.95 % divided by four
- C. 7.86 % ✅
- D. 28.04 %

*Rationale:* `0.52953912⁴ = 7.86 %` (15.1.3). A ignores that all four must succeed; B divides instead of taking the fourth power; D squares instead of raising to the fourth power (`0.52953912² = 28.04 %`), the error of treating four regions as two.


**15.1-C** `[15.1.3 · Evaluation]` A programme needs 80 % confidence in a milestone that depends on 24 independent predecessors. The required probability on each is closest to:

- A. 80.0 %
- B. 96.3 %
- C. 99.1 % ✅
- D. 99.9 %

*Rationale:* `0.80^(1/24) = 99.0745 %` (15.1.3). A applies the target to each component; B is the answer for six dependencies, not 24; D over-corrects. The teaching point is that C is unattainable, which is why the lever is structural.


**15.1-D** `[15.1.4 · Evaluation]` Removing the 0.85 data-migration dependency from Meridian's regional go-live milestone raises the regional probability from 52.95 % to 62.30 %. The mechanism is best described as:

- A. improving the migration team's performance
- B. multiplying the remaining product by `1/0.85` ✅
- C. adding 0.85 of buffer to the milestone
- D. averaging the remaining five probabilities

*Rationale:* Decoupling divides the product by the removed factor, which is the same as multiplying by its reciprocal — `0.52953912/0.85 = 0.6229872` (15.1.4). A describes the money route, which bought 2.69 points on the programme milestone for 240,000; C and D misstate the arithmetic.


**15.1-E** `[15.1.2 · Analysis]` The soundest test that a proposed tranche boundary is real is whether:

- A. it falls at the end of a financial quarter
- B. the organisation can genuinely operate in the intermediate state ✅
- C. a governance body is available to meet on that date
- D. it divides the components into groups of similar size

*Rationale:* A tranche exists to reach a usable intermediate state at which benefit begins and the programme could stop with value in operation (15.1.2). A, C and D are reporting conveniences that produce gates with nothing to decide.


**15.2-A** `[15.2.3 · Analysis]` A portfolio's scarce team supplies 6 units a quarter for four quarters. Three candidates demanding 24 units in total are selected on the basis that total capacity is 24 units. The most accurate criticism is that:

- A. the portfolio is under-committed
- B. aggregate feasibility does not imply period feasibility; the set demands 9 units in Q1 ✅
- C. the team's capacity should have been expressed annually
- D. net present value is the wrong selection measure

*Rationale:* Constraints bind per period; the set is aggregate-feasible and Q1-infeasible by 3 units (15.2.3). A inverts the problem; C names the cause of the error as its cure; D is a different debate.


**15.2-B** `[15.2.3 · Evaluation]` In the Meridian portfolio allocation, ranking by NPV per unit yields 2,800,000 while enumeration yields 3,810,000. The reason the ranking fails is that:

- A. the ratios were computed incorrectly
- B. enumeration uses a different objective function
- C. the highest-ratio candidate consumes five of the six units in the scarce quarter, excluding the two largest-value candidates ✅
- D. the candidates have equal ratios, so the ranking is arbitrary

*Rationale:* A ratio averages the period away and cannot see a demand profile concentrated in the binding period (15.2.3). Both methods maximise NPV, so B is wrong; three candidates do share a 170,000 ratio, but that is not what causes the 1,010,000 shortfall.


**15.2-C** `[15.2.1 · Application]` Five components claim 1,981,200 of annual benefit at full potential. Eliminations total 364,800 and portfolio adoption is 70 %. The net realistic annual benefit is:

- A. USD 1,616,400
- B. USD 1,386,840
- C. USD 1,131,480 ✅
- D. USD 792,036

*Rationale:* `(1,981,200 − 364,800) × 0.70 = 1,131,480` (15.2.1). A omits the adoption adjustment; B omits the eliminations; D applies adoption twice (`1,131,480 × 0.70`), the error of taking a component figure that is already adoption-adjusted and adjusting it again.


**15.2-D** `[15.2.1 · Analysis]` Two components claim a combined 8.4 administrative posts from a pool of 6.0, valued at 42,000 each. The elimination is:

- A. USD 352,800
- B. USD 252,000
- C. USD 100,800 ✅
- D. USD 42,000

*Rationale:* `(8.4 − 6.0) × 42,000 = 100,800` (15.2.1). A is the gross claim; B is the pool's total value, which is the retained benefit, not the elimination; D is one post.


**15.2-E** `[15.2.1 · Evaluation]` A portfolio's benefits bridge eliminates 18.41 % of gross claimed benefit, and the breakeven elimination rate for its four-year payback rule is 12.03 %. The correct conclusion is that:

- A. the portfolio clears the rule with a 6.4-point margin
- B. the portfolio fails the rule, and would still fail it at any elimination rate above 12.03 % ✅
- C. the rule should be relaxed to accommodate the eliminations
- D. the eliminations of 364,800 should be reported as a portfolio saving

*Rationale:* An observed rate above the breakeven rate means the rule is not met (15.2.1). A reads the comparison backwards; C is a decision for the investment committee, not a conclusion from the arithmetic; D commits the error the topic explicitly warns against — eliminating a double count prevents a wrong decision and creates no value.


**15.3-A** `[15.3.2 · Application]` A delivery organisation completes 5 initiatives a year and has 12 in flight. Average cycle time is:

- A. 0.42 years
- B. 1.00 year
- C. 2.40 years ✅
- D. 5.00 years

*Rationale:* `T = W/C = 12/5 = 2.40` years (Little's Law, cited from KA 13.2.3). A inverts the ratio; B is the cycle time at a work-in-progress limit of 5; D is the throughput read as a duration.


**15.3-B** `[15.3.2 · Evaluation]` Reducing portfolio work in progress from 12 to 5 at an unchanged throughput of 5 a year, with a net benefit run rate of 1,131,480, is best described as:

- A. increasing throughput by 140 %
- B. bringing the whole benefit stream forward by 1.40 years, worth 1,584,072 once ✅
- C. saving 1,584,072 every year thereafter
- D. reducing the portfolio's benefit, since fewer initiatives are in flight

*Rationale:* Throughput is capacity-set and unchanged; cycle time falls by `(12−5)/5 = 1.40` years and the whole stream shifts forward once — `1,131,480 × 1.4 = 1,584,072` (15.3.2). A misreads Little's Law; C converts a one-off shift into an annuity; D confuses starting with finishing.


**15.3-C** `[15.3.3 · Analysis]` A four-quarter plan has slack of 0, 1, 0 and 0 units. Losses of 0, 1 and 2 or more units occur with probabilities 0.70, 0.22 and 0.08. The probability the plan survives all four quarters is closest to:

- A. 70.0 %
- B. 41.5 %
- C. 31.6 % ✅
- D. 24.0 %

*Rationale:* `0.70 × 0.92 × 0.70 × 0.70 = 31.56 %` (15.3.3). A applies the single-quarter figure to the year; B uses 0.92 in two quarters rather than one; D applies 0.70 to all four quarters and ignores the slack quarter's tolerance.


**15.3-D** `[15.3.3 · Evaluation]` Reserving capacity costs 720,000 of NPV and lifts plan survival from 31.56 % to 76.31 %. A quarter's breach costs 185,640. The correct conclusion is:

- A. reserve, because the survival probability more than doubles
- B. do not reserve unless a breach would cost more than about 1.61 million ✅
- C. reserve, because the expected saving of 83,084 is positive
- D. the comparison cannot be made without a discount rate

*Rationale:* `720,000 / (0.76311424 − 0.31556) = 1,608,744` is the breach cost at which the trade breaks even (15.3.3). A argues from a ratio without a price; C is true and irrelevant, since 83,084 is far below 720,000; D is a refinement, not an obstacle.


**15.3-E** `[15.3.4 · Evaluation]` An enterprise PMO costing 1,180,000 a year claims 703,650 of recurring value and 565,740 of one-off value. The soundest assessment is that it:

- A. pays, with a year-one surplus of 89,390
- B. pays, because total claimed value exceeds cost
- C. fails from year two by 476,350 and needs a named recurring mechanism ✅
- D. should book the 364,800 of benefits eliminations to close the gap

*Rationale:* One-offs do not repeat, so the recurring test is the one that matters (15.3.4). A and B are true of year one and irrelevant thereafter; D books a prevented wrong decision as a cash benefit, which the domain rejects at KA 15.2.1.


**15.4-A** `[15.4.1 · Application]` Five tiers have `M` and `L` of (2, 1), (4, 2), (6, 2), (12, 3) and (13, 4) weeks. The expected latency of a decision that must pass all five is:

- A. 18.5 weeks
- B. 30.5 weeks ✅
- C. 37.0 weeks
- D. 20.0 weeks

*Rationale:* `(2/2+1) + (4/2+2) + (6/2+2) + (12/2+3) + (13/2+4) = 2.0+4.0+5.0+9.0+10.5 = 30.5` (15.4.1, formula from KA 3.3.3). A halves the intervals and omits the paper lead times entirely (1.0+2.0+3.0+6.0+6.5); C sums the meeting intervals alone without halving them or adding the lead times (2+4+6+12+13); D omits the executive tier.


**15.4-B** `[15.4.1 · Evaluation]` A portfolio's 85 decisions generate 468 gross latency-weeks and 145 tier traversals; 25 % sit on the critical path and delay costs 14,280 a week. Cutting one week from every tier's paper lead time saves:

- A. USD 517,650 ✅
- B. USD 1,670,760
- C. USD 303,450
- D. USD 2,070,600

*Rationale:* A one-week cut saves one week per traversal: `145 × 0.25 × 14,280 = 517,650`, which is 30.98 % of the 1,670,760 total (15.4.1). B is the whole bill; C applies the saving to the 85 decisions rather than the 145 traversals; D omits the critical-path share.


**15.4-C** `[15.4.1 · Analysis]` In that architecture, 6 of the 85 decisions carry 141 of the 468 gross latency-weeks. The strongest implication is that:

- A. those 6 decisions should be delegated downward
- B. an out-of-cycle route at the slow tiers is worth far more per decision affected than removing a fast tier ✅
- C. the executive board should meet more often
- D. the 6 decisions are the most important and their latency is justified

*Rationale:* 7.06 % of decisions carry 30.13 % of latency, so a written-resolution route for them saves 471,240 against 107,100 from removing the fast component-board tier (15.4.1). A may breach the reserved-matters rationale; C is the weaker lever, since a week off `M` saves half a week; D is a legitimate position but not an implication of the arithmetic.


**15.4-D** `[15.4.2 · Analysis]` A portfolio has `ΣEV` 2,349,900 and `ΣAC` 2,473,000. The unweighted mean of its five component `CPI`s is 1.006. The portfolio `CPI` is:

- A. 1.01, the mean of the components
- B. 0.95 ✅
- C. 0.96
- D. 1.05

*Rationale:* `CPI = ΣEV/ΣAC = 0.950222` (15.4.2). A averages ratios, the defect the topic exists to prevent; C is the portfolio `SPI` (`ΣEV/ΣPV`); D inverts the ratio.


**15.4-E** `[15.4.2 · Evaluation]` The unweighted-average `CPI` flatters this portfolio because:

- A. there are five components and five is too few to average
- B. the component with 54.67 % of cost incurred is the only one below 0.95, and four small ahead-of- budget components outvote it ✅
- C. the components have different budgets at completion
- D. `EAC = BAC/CPI` is the wrong forecasting method

*Rationale:* An unweighted mean gives each component equal voice regardless of the money behind it, so it flatters whenever the large component is the troubled one (15.4.2). A is not the mechanism; C is true but not the cause, since the weighting that matters is cost incurred; D is a separate question governed by Domain 7.


**15.4-F** `[15.4.3 · Comprehension]` A portfolio report should support exactly three decisions. They are:

- A. plan, monitor and control
- B. continue/change/stop, reallocate capacity, escalate ✅
- C. approve, reject and defer
- D. report, review and assure

*Rationale:* Those three are what a portfolio body can actually do; anything serving none of them is decoration (15.4.3).


## Domain 16

**16.1-A** `[16.1.3 · Application]` Seven readiness conditions, whose failures are assessed as independent, hold with probabilities 0.96, 0.90, 0.94, 0.98, 0.85, 0.80 and 0.92. All seven are necessary for a clean go-live. The probability of a clean go-live is closest to:

- A. 90.71 %
- B. 80.00 %
- C. 49.79 % ✅
- D. 47.83 %

*Rationale:* The conjunction is the product, `0.96 × 0.90 × 0.94 × 0.98 × 0.85 × 0.80 × 0.92 = 49.79 %` (16.1.3). A is the equal-weight average — the dashboard figure, and a different quantity. B is `min(pᵢ)`, the perfectly correlated bound, which the stem's independence assumption excludes and which is in any case the optimistic case. D rounds the dashboard reading to 0.90 and raises *that* to the seventh power, `0.90⁷ = 47.83 %` — an answer that both rounds and double-counts the averaging.


**16.1-B** `[16.1.3 · Analysis]` Seven independent readiness conditions, holding at 0.96, 0.90, 0.94, 0.98, 0.85, 0.80 and 0.92, give a conjunction of 49.79 %. The gain in the probability of a clean go-live from lifting one condition is governed by:

- A. the absolute gap to the target, with an equal gain per percentage point of gap closed
- B. the ratio `p′/p`, so the same gap closed at a lower `p` yields a larger gain ✅
- C. the condition's weight on the readiness dashboard
- D. `1/k`, equally across all seven conditions

*Rationale:* Lifting a condition from `p` to `p′` multiplies the conjunction by `p′/p`, so the gain is `∏pᵢ × (p′/p − 1)` (16.1.3). Closing two points of gap at 0.80 is worth **1.24 pp** while closing the same two points at 0.96 is worth **1.04 pp**, which is exactly what A denies — a common target ranks the conditions in the same order either way, but the magnitudes are set by the ratio. B is therefore the statement that generalises. C substitutes an artefact for the arithmetic. D confuses "a product" with "symmetric in its factors".


**16.1-C** `[16.1.3 · Evaluation]` A programme wants a 95 % probability of a clean go-live across seven independent readiness conditions. The minimum probability required on each condition is closest to:

- A. 95.00 %
- B. 99.27 % ✅
- C. 98.00 %
- D. 99.90 %

*Rationale:* `p = 0.95^(1/7) = 99.27 %` (16.1.3). A applies the target to each condition rather than to their conjunction. C gives only `0.98⁷ = 86.81 %`. D is stricter than required and would be rejected as unachievable, losing the argument the arithmetic wins.


**16.1-D** `[16.1.4 · Application]` Forty sites each face seven independent readiness conditions giving a 49.79 % probability of a clean go-live; a failed go-live costs 27,060. A three-week hold costs 42,840 in deferred benefit plus 96,000 of remediation and lifts all seven conditions to 0.98. The expected saving from holding is closest to:

- A. USD 138,840
- B. USD 261,864 ✅
- C. USD 404,605
- D. USD 543,445

*Rationale:* Going now costs `40 × (1 − 0.4979) × 27,060 = 543,445`; holding costs `42,840 + 96,000 + 40 × (1 − 0.8681) × 27,060 = 281,581`; the saving is **261,864** (16.1.4). A is the cost of holding, not the saving. C omits the residual remediation after the uplift and so credits the hold with the whole 543,445, giving `543,445 − 138,840 = 404,605` — the error that makes readiness work look better than it is. D is the cost of going now with no credit for the hold at all.


**16.1-E** `[16.1.1 · Comprehension]` Which of the following does **not** transfer at handover?

- A. custody of the asset
- B. accountability for operating the service
- C. accountability for realising the benefit ✅
- D. responsibility for paying the running costs

*Rationale:* The benefits owner sits in the receiving organisation from the outset (Domain 2, KA 2.3.1), so handover tests that appointment rather than creating it; a benefit "handed over" at closeout has no owner and no baseline (16.1.1).


**16.2-A** `[16.2.3 · Application]` A contract sum of 1,680,000 carries 96,000 of approved variations, a claim settled at 74,000 and an 18,600 recovery for defects rectified by others. The final account is:

- A. USD 1,776,000
- B. USD 1,831,400 ✅
- C. USD 1,850,000
- D. USD 1,757,400

*Rationale:* `1,680,000 + 96,000 + 74,000 − 18,600 = 1,831,400` (16.2.3). A is the certified works value, omitting both the settlement and the recovery. C omits the recovery. D omits the settlement.


**16.2-B** `[16.2.4 · Application]` Carrying an open claim costs 5,250 a month. Determination at month 14 has an expected value of 68,500; the alternative is settlement at month 2. The breakeven settlement price is:

- A. USD 68,500
- B. USD 131,500 ✅
- C. USD 142,000
- D. USD 73,500

*Rationale:* `68,500 + 14 × 5,250 − 2 × 5,250 = 131,500` (16.2.4). A ignores carrying cost entirely. C is the expected cost of fighting, without crediting the two months still carried under the settlement option. D is fourteen months of carrying cost alone.


**16.2-C** `[16.2.4 · Analysis]` A claim costing 5,250 a month to carry, with an expected determination of 68,500 at month 14 and settlement available at month 2, is assessed by the client at 62,000 against the contractor's claimed 148,000. The breakeven **premium** over the assessed value is 69,500. The most defensible reading is that:

- A. the claim should be conceded in full at 148,000
- B. up to 69,500 above the assessed value can be paid to close twelve months earlier, because carrying cost times months saved plus the gap between the expected determination and the assessment is 63,000 + 6,500 ✅
- C. the claim should be fought, since 62,000 is the defensible figure
- D. the premium equals the provision held

*Rationale:* The identity `c × Δm + (EMV − assessed)` gives 69,500 (16.2.4). A ignores the assessment altogether. C is the intuition the arithmetic overturns — winning at 62,000 after fourteen months costs 135,500. D confuses a provision with a price.


**16.2-D** `[16.2.1 · Evaluation]` A business case with no operating-cost line hands over a service whose measured run cost is 108,000 a year. Over an eight-year appraisal at 7 % (`AF = 5.971299`), the omission is worth:

- A. USD 864,000
- B. USD 644,900 ✅
- C. USD 108,000
- D. USD 540,000

*Rationale:* `108,000 × 5.971299 = 644,900` (16.2.1). A is the undiscounted eight-year total, the commonest error and an overstatement of 34.0 %. C is a single year. D applies a five-year horizon undiscounted.


**16.2-E** `[16.2.2 · Analysis]` A client holds 84,000 of retention, releases half of it at takeover, and later recovers 18,600 for defects that another supplier had to rectify. Releasing the whole retention at takeover rather than in stages would most directly have:

- A. improved the contractor's cash position at no cost to the client
- B. converted an 18,600 recovery into a debt to be pursued from a contractor with no remaining incentive ✅
- C. breached the defects-liability period
- D. reduced the final account by 18,600

*Rationale:* Retention is security, and its value is precisely that it is still held when a recovery arises (16.2.2). C confuses a payment mechanism with a liability period; D reverses the direction of the recovery.


**16.3-A** `[16.3.3 · Application]` A review costing 46,000 produces 34 lessons; an applied lesson avoids 26,000 on average. The breakeven retrieval rate is closest to:

- A. 5.20 % ✅
- B. 12.00 %
- C. 43.36 %
- D. 52.04 %

*Rationale:* `46,000/(34 × 26,000) = 46,000/884,000 = 5.20 %` (16.3.3). B is the organisation's current retrieval rate, not the breakeven. C is `46,000/106,080` — the share of the *expected recovery* consumed by the review, which is a cost-recovery ratio and not a rate. D misplaces the decimal, a common slip on a small ratio.


**16.3-B** `[16.3.3 · Evaluation]` A review costing 46,000 produces 34 lessons, an applied lesson avoids 26,000 on average, and 12 % of captured lessons are currently retrieved before a comparable decision. The highest-return improvement is to:

- A. run a longer, better-facilitated review workshop
- B. spend 18,000 indexing the lessons against the decision points where they apply, raising retrieval from 12 % to 35 % ✅
- C. capture more lessons per project
- D. shorten the review to reduce its 46,000 cost

*Rationale:* Indexing yields `34 × 0.23 × 26,000 = 203,320` for 18,000 — a ratio of 11.30 (16.3.3). A and C invest in capture, where the value is not being lost. D reduces a cost that is already recovered 2.31 times over.


**16.3-C** `[16.3.1 · Application]` Three undocumented interface configurations cost 8,400 each to reconstruct after handover, against 1,200 each to document at the time. The ratio of avoidable to incurred cost is:

- A. 7.00 ✅
- B. 3.00
- C. 21,600 to 1
- D. 2.33

*Rationale:* `25,200/3,600 = 7.00`, which is also the per-item ratio `8,400/1,200` (16.3.1). B counts the three items rather than the cost ratio. C is the absolute saving, `25,200 − 3,600 = 21,600`, mislabelled as a ratio. D divides the *per-item* reconstruction cost by the *total* documentation cost, `8,400/3,600 = 2.33`, mixing a unit figure with a total.


**16.3-D** `[16.3.2 · Analysis]` The review practice most likely to produce transferable findings rather than narrative is to:

- A. begin from the schedule variance and work backwards
- B. begin from the decision record and ask of each material decision whether it was reasonable on what was known at the time ✅
- C. invite every stakeholder to describe their experience
- D. rank the events of the project by impact

*Rationale:* Decisions are the unit at which an organisation can improve; events are consequences (16.3.2, and Domain 3 KA 3.3.4 on the retrospective question). A and D produce narrative; C produces testimony without a comparison.


**16.4-A** `[16.4.2 · Application]` Twenty-seven of 40 clinics are in daily use, each releasing 5.4 hours a week at USD 85 over 48 weeks. The measured annual benefit is:

- A. USD 685,440
- B. USD 594,864 ✅
- C. USD 881,280
- D. USD 660,960

*Rationale:* `27 × 5.4 × 85 × 48 = 594,864` (16.4.2). A is the honest business-case figure at 70 % adoption and 6.0 hours. C omits the adoption term, counting all 40 clinics at the measured 5.4 hours. D keeps the correct 27 clinics but the case's 6.0 hours, omitting the benefit-per-unit correction.


**16.4-B** `[16.4.2 · Analysis]` With `U = 163,200`, planned adoption 0.700 and 6.0 hours, and measured adoption 0.675 and 5.4 hours, the decomposition taking **adoption first** gives:

- A. adoption (24,480); hours (66,096) ✅
- B. adoption (22,032); hours (68,544)
- C. adoption (24,480); hours (68,544)
- D. adoption (66,096); hours (24,480)

*Rationale:* Adoption at planned hours is `163,200 × 6.0 × (−0.025) = (24,480)`; hours at measured adoption is `163,200 × 0.675 × (−0.6) = (66,096)`; both sum to (90,576) (16.4.2). B is the reverse order — correct arithmetic, wrong to the question asked. C mixes the two orders and over-states the total by the 2,448 interaction term. D transposes the labels.


**16.4-C** `[16.4.2 · Evaluation]` Meridian's realised NPV is (304,827). Which single line is most responsible, and what does that imply?

- A. the adoption shortfall; the rollout under-delivered
- B. the operating cost omitted from the case, at (644,900); without it the realised position is +340,073, so the whole margin turned on a line the project never controlled ✅
- C. the cost outturn variance of (114,000); the programme overspent
- D. the timing variance; benefits arrived late

*Rationale:* The operating-cost line is the largest realised-period movement and exceeds the whole negative position (16.4.2, 16.2.1). A and D are real but smaller — (465,015) and (413,809). C is the smallest line at 4.75 % of approved cost.


**16.4-D** `[16.4.3 · Analysis]` A reviewer claims a concurrent national initiative accounts for 0.6 of the 5.4 measured hours. The measurement design element that answers the claim is:

- A. a larger time-sampling exercise in the adopting clinics
- B. the comparison cohort of clinics measured at baseline and review without the change ✅
- C. a sensitivity analysis on the valuation rate
- D. the benefits owner's professional judgement

*Rationale:* Only a comparison can net out other causes, and it must exist from baseline (16.4.3); without it the assertion is unfalsifiable and, at 4.8 attributable hours, would move the realised NPV from (304,827) to (621,946). A measures the same population more precisely and answers a different question.


**16.4-E** `[16.4.4 · Application]` A 6.4 TB archive costs USD 21 per TB per month. If the probability that the evidence is needed within seven years is 0.18 and the cost of being unable to answer is 310,000, the breakeven need probability is closest to:

- A. 18.00 %
- B. 3.64 % ✅
- C. 0.52 %
- D. 36.42 %

*Rationale:* `6.4 × 21 × 12 × 7 = 11,290`; `11,290/310,000 = 3.64 %` (16.4.4). A is the assessed probability, not the breakeven. C uses one year of storage. D misplaces the decimal.


**16.4-F** `[16.4.5 · Comprehension]` The minimum retention that makes an AI-informed decision explainable two years later is:

- A. the model artefact itself, retained locally
- B. the prompt and the output
- C. model identifier and version, date, input, output, named verifier and decision-maker ✅
- D. a summary of the analysis in the decision record

*Rationale:* A third-party model version may cease to exist, so the durable evidence is the record of what it produced and who verified it (16.4.5). B omits the version and the verifier, which is exactly the omission that made 28.50 % of Meridian's AI-informed decision base unexplainable.
