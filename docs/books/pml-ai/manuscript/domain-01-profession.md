# Domain 1 — The Project Leadership Profession

> **Group:** Leading projects (Domain 1 of 4 in Part One). **Target:** ~68 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This domain fixes the book's core vocabulary — accountability,
> outputs/outcomes/benefits, systems thinking, the responsible-AI principle — that every later
> domain assumes. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).

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
portfolios and explain what each optimises; contrast project leadership with operational
leadership; explain why a temporary organisation makes trust and clarity urgent; distinguish
accountability from responsibility and apply the distinction to a delegation; state the leader's
obligations to sponsor, team, users and the public, and where they conflict; explain the
professional standard of care; analyse a project as a system with feedback and delay; distinguish
outputs, outcomes and benefits and demonstrate why output-based claims overstate value; compute
the value of time in benefit terms; describe the ethical obligations of the role; and apply the
PCI responsible-AI principle to a realistic delivery decision.

**The master case.** One programme runs through this domain and returns in Domains 2 and 16:
**Meridian Care Records**, a fictional public-health programme rolling a shared clinical-records
system out to **40 clinics**. Its numbers are used in KA 1.3 to make the outputs/outcomes/benefits
distinction arithmetic rather than rhetorical.

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
nothing if clinicians do not use the system. KA 1.3 makes that arithmetic.

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

### Self-check — KA 1.1

1. *Can a project succeed while its programme fails?* — Yes: outputs delivered, outcomes not
   realised. The tests differ (1.1.1).
2. *Name the defining failure mode of temporary work.* — Late discovery followed by
   irreversibility; correction cost rises as commitments harden.
3. *State the responsible-AI principle.* — AI proposes; the professional verifies, decides and
   remains accountable.

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
exactly this) and the same problem reported at the deadline is not.

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

### Key terms — KA 1.2

| Term | Meaning |
|---|---|
| **Responsibility** | The obligation to do; shareable and delegable. |
| **Accountability** | The obligation to answer; single-holder, non-delegable. |
| **Escalation duty** | The obligation to raise, with options, a conflict beyond one's authority. |
| **Honesty asymmetry** | Bad news costs the messenger and helps the project; leaders must make it safe. |
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

### Self-check — KA 1.2

1. *What can be delegated and what cannot?* — Responsibility can; accountability cannot.
2. *Why can accountability never attach to an AI tool?* — Accountability is the obligation to
   answer; a tool cannot be asked to answer, sanctioned, or hold a duty.
3. *Name the four constituents of care in this discipline.* — Proportionate method, evidence
   matching the claim, checkable records, candour about uncertainty.

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
   right, and why "40 clinics live" is a milestone, not a benefit. Sensitivity is the leader's
   real lever: at 50 % adoption the benefit is USD 489,600; at 90 %, USD 881,280. **A programme
   of this shape creates more value by moving adoption than by moving its installation date** —
   which should change where the leader spends personal attention (Domain 11's engagement work,
   not Domain 6's schedule compression).

> **Fig 1.3.1 — The value chain, and where it leaks.** Left-to-right chain diagram with four
> linked blocks: OUTPUT "40 clinics installed" → OUTCOME "28 clinics using it (70 % adoption)" →
> BENEFIT "USD 685,440 per year released" → VALUE "benefit weighed against cost and risk". Beneath
> the output→outcome link, a crimson leak arrow annotated "30 % — USD 293,760 of claimed value
> that never existed". A caption note: each link can fail independently. Source: PCI original.
> Alt text: a four-stage chain from installed outputs through adoption and released benefit to
> value, with a leak arrow marking the thirty per cent lost between output and outcome.

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
   costs?" — and here it clearly does, provided adoption materialises. Note the dependency the
   arithmetic makes visible: every figure rests on the 70 % adoption of 1.3.2, so **spending
   60,000 to arrive sooner at an unadopted system buys nothing.** Sequencing engagement before
   acceleration is not a preference; it is what the numbers say.

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

### Self-check — KA 1.3

1. *Why is an output claimed as a benefit an overstatement by construction?* — Because the
   outcome link can fail independently; unadopted outputs produce no benefit (Meridian's 30 %).
2. *What is the leader's question on hearing "we absorbed it"?* — Where did the pressure go?
3. *What must be true before paying to accelerate Meridian?* — That adoption materialises;
   arriving sooner at an unadopted system buys nothing.

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
different places.

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

### Self-check — KA 1.4

1. *State the daylight test.* — Would every party, seeing the full picture, still regard this as
   impartial?
2. *Which AI failure mode is hardest to counter, and why?* — Over-trust through fluency: polish
   suppresses the scrutiny that rough work attracts.
3. *What does "protect the team's judgment" mean in practice?* — Do not accept AI output the team
   cannot critique; capability traded for speed is repaid during recovery.

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

### 1.A.3 The reviewer's leadership eye

Invariants an experienced reviewer tests in the first hour on any project: exactly one accountable
name per outcome; a benefits chain where each claimed benefit traces to a measured outcome with an
owner; decision records that show what was known when; escalations that arrived early enough to
matter; forecasts with ranges rather than single numbers; and AI-assisted artefacts carrying a
named verifier. Every one of these is cheap to check and expensive to have missing — the same
posture Domains 6 and 7 apply to schedules and cost.

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

**What was done.** The recovery was not technical. Adoption was made a named accountability with a
monthly measure; the benefits case was restated with an adoption term and a sensitivity range
(50/70/90 % → 489,600 / 685,440 / 881,280); clinical champions were funded in the eleven
lowest-adoption clinics; and the programme board's report changed from "clinics live" to "clinics
using, and hours released". Adoption reached 68 % within a year.

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

**What the domain teaches here.** Fluency is not evidence. Accountability did not move to the
vendor or the model — it stayed with the person who issued the plan (MCQ 1.2-B is this case in
miniature). And the durable fix is structural, not exhortative: a named verifier per artefact,
discipline leads walking their own links, and a team kept capable of critique.

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
  irreversible surprises into recoverable problems.
- **The AI accountability line.** Named verifiers, proportionate depth, and a team that remains
  able to critique what the tools produce.

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

## Exam preparation — Domain 1

**The traps.** Treating outputs as benefits (1.3.2 — the domain's central trap, and Meridian's
whole case study) · saying accountability is "shared" (1.2.1) · assigning accountability to a
vendor or a tool (MCQ 1.2-A, 1.2-B) · comparing an acceleration cost against annual rather than
saved-period benefit (Exercise 1.2) · reading a delivery success as a programme success (1.1.1) ·
accepting "we absorbed it" (1.3.1) · uniform AI verification depth (1.4.3) · judging the standard
of care by outcome alone (1.2.3).

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
being skipped. Timing decisions are priceable (USD 14,280 per week; acceleration breaking even at
4.20 weeks), and attention belongs where value actually leaks. Around all of it sits professional
ethics and the suite's governing principle — **AI proposes; the professional verifies, decides and
remains accountable** — with named verifiers, proportionate depth, and a team kept capable of
critique. Domain 2 turns strategy into selected work; Domain 3 gives the accountability of this
domain its governance machinery.
