# Domain 14 — Digital Delivery, Data and Responsible AI

> **Group:** Enterprise delivery and the digital future (Domain 14 of 3 in Part Four). **Target:**
> ~76 pages.
> **Binds to:** the PCI Book Pattern Specification and the shared registries
> (`docs/books/registries/`). This is the volume's **systematic** treatment of digital delivery, data
> and artificial intelligence. Domains 1 to 13 each carried an *AI in this KA* section covering the
> use and misuse of AI inside their own subject matter; this domain **consolidates** those positions
> into one governable system and supplies the arithmetic the earlier domains asserted but did not
> derive. It runs **Meridian Care Records** at programme scale and **Project Auriga** at
> single-project scale. British English; USD (+SAR where useful, indicative `USD 1 ≈ SAR 3.75`).
>
> **Registry note.** This domain uses the registered **cost of delay** (PML-AI D1/D3) and the
> registered **governance latency** identity `E[wait] = M/2 + L` (PML-AI D3, KA 3.2.3) unchanged,
> applying the second to *information* rather than to decisions — the same two operations on a
> reporting period and a production lag, cited rather than re-derived. It cites Domain 9's sampling
> arithmetic (`p* = c/u` and the clean-sample bound, KA 9.3.2 and 9.4.4) and Domain 9's composite
> data fitness (KA 9.4.3) rather than restating them. It submits four candidate registry rows:
> **consequence-weighted defect exposure** `Σ nᵢdᵢuᵢ` with the per-record exposure `dᵢuᵢ` that ranks
> remediation; **quality-adjusted automation breakeven volume**
> `n* = F / [(m + eₘu) − (a + eₐu)]`; the **verification tier threshold**
> `u*ₖ = Δvₖ / (p · Δqₖ)` — the derived form of the *verification standard proportional to
> consequence* that Domain 3, KA 3.A.2 asserted; and the **escape-cost asymmetry**
> `(1 − q_p)u_p ÷ (1 − q_o)u_o`. Each carries a verified golden example below. Every error rate,
> detection rate, defect rate and unit cost in them is a **locally calibrated planning or measured
> figure, not a constant** — the domain says so at each use, and says what must be measured to
> replace it.

## Why this domain exists

Thirteen domains have now used AI in the way a professional actually uses it: as an assistant to a
specific task, bounded by a specific prohibition. Domain 3 refused it a governance decision;
Domain 4 refused it a baseline; Domain 5 refused it the authorship of a requirement; Domain 9 refused
it a test result and an inspection signature; Domain 10 refused it a quality score and a liability
cap; Domains 11 and 12 refused it inference about people. Each refusal was correct and each was
local. What none of them could supply, standing alone, is the thing an organisation actually needs:
**one system in which those refusals are consistent, resourced, auditable and priced.**

That is what is missing in practice, and its absence has a characteristic shape. Organisations adopt
digital tools and AI assistance tool by tool, each justified on a productivity claim, each verified
by whoever happens to be nervous, none priced. The result is an estate in which the verification
effort is simultaneously excessive on low-consequence outputs and absent on high-consequence ones;
in which a data defect rate is reported as a single percentage that no decision can use; in which an
automation is celebrated for the labour it removed and never charged for the errors it lets through;
and in which the most dangerous artefact in the whole system — a fluent, plausible, wrong number —
passes every review that was designed to catch an obviously wrong one.

Hence the domain's central claim: **digital delivery, data quality and AI assistance are all
economic quantities with computable breakevens, and the professional obligation is to compute them
rather than to adopt or resist on principle.** A leader who can state the consequence at which
verification becomes worth its cost, the volume at which an automation starts paying, the exposure
that each class of data defect carries and the ratio by which a plausible error out-damages an
obvious one has a governable digital estate. A leader who cannot will oscillate between two equally
indefensible postures — blanket prohibition, which forfeits real value, and blanket adoption, which
transfers an unmeasured cost to whoever inherits the output.

This is therefore the most rigorous domain in the volume rather than the softest, which is the
opposite of how responsible-AI material is usually written. KA 14.1 builds the substrate — the
digital environment, the governance of data, the common data environment, and the defect rate by
data class that determines whether anything built on top of it is usable. KA 14.2 covers what
organisations build on that substrate: dashboards, analytics, digital twins and automation, each
tested against the decision it claims to serve and the volume at which it pays. KA 14.3 is the
domain's core: where AI earns its place across the lifecycle, how a prompt becomes professional
practice, and the derivation of the verification standard proportional to consequence — followed by
the arithmetic of why the plausible wrong number is the one to fear. KA 14.4 closes with the four
obligations that cannot be computed away: explainability that supports a decision, differential
error, human accountability, and the security and privacy of a delivery estate that now holds
everything.

**Learning objectives.** After this domain a candidate can: describe a digital delivery environment
as a set of systems with a countable interface problem and apply Domain 4's interface arithmetic to
it; specify data governance as ownership, definition, lineage and access rather than as a policy
document; state what a common data environment must guarantee and test whether a claimed one does;
**compute a defect rate by data class, convert it into consequence-weighted exposure, rank
remediation by exposure per record, and demonstrate arithmetically that a uniform data-quality
target both reduces defect counts and increases expected cost**; test a dashboard against the
decision it claims to inform and **compute the expected age of a fact at the moment of decision
using the registered `M/2 + L` identity, priced at the cost of delay**; place an analytics
initiative on the descriptive-to-prescriptive ladder and state what each rung requires and returns;
define what makes a digital twin a twin and **compute the fidelity at which it breaks even**;
**compute an automation breakeven volume, adjust it for the differential error-escape rate, and
identify the cases in which no volume justifies the automation**; state where AI earns its place at
each stage of the lifecycle and cite the domain that governs each; write and govern a prompt as a
professional artefact with grounding, provenance and a recorded version; **derive a tiered
verification standard from the marginal cost of a review tier, its marginal detection rate and the
consequence of an escaped error, and price the tiered standard against uniform alternatives**;
**compute why a plausible wrong number costs an order of magnitude more than an obviously wrong one,
and price the reperformance step that closes the gap**; specify explainability as a decision
requirement rather than a model property; **compute differential error rates across groups, price
the remediation, and state where expected-value reasoning stops applying**; maintain an AI use
register that satisfies Domain 1's accountability test; and **compute the economics of security
controls, distinguishing probability reduction from impact reduction and demonstrating why control
benefits are sub-additive**.

**The master threads.** Meridian Care Records supplies the programme case: the clinical-records
rollout to **40 clinics**, approved cost **USD 2,400,000**, full-potential benefit
**USD 979,200** a year, realistic benefit **USD 685,440** a year at 70 % adoption, **cost of delay
USD 14,280 per week** (Domain 1) — **USD 2,040 per day** — and a steering committee whose latency
`E[wait] = 4/2 + 2` is **4.0 weeks** (Domain 3, KA 3.2.3). Internal effort is valued at the blended
programme rate of **USD 110 per hour** established in Domain 11. This domain audits Meridian's
common data environment across **14,100** records in six classes, derives its verification standard
across **941** AI-assisted outputs a month, and prices its security controls. Project Auriga
supplies the project case: the 25-week control-systems upgrade, `BAC` **USD 4,000,000**, cost of
delay **USD 45,000 per week**, a blended engineering rate of **USD 130.625 per hour** (Domain 7,
KA 7.4.1), and at week 13 `PV` **2,080,000** / `EV` **1,920,000** / `AC` **2,120,000**, giving
`CPI` **0.91** and `SPI` **0.92**. This domain prices Auriga's data-check automation, its control
system's digital twin, and the forecast error that its week-13 numbers make possible.

---

## Knowledge Area 14.1 — Digital project environments, data governance and the common data environment

*Topics: 14.1.1 the digital delivery environment · 14.1.2 data governance · 14.1.3 the common data
environment · 14.1.4 defect rate by data class and the exposure it carries.*

### 14.1.1 The digital delivery environment

**Definition.** A project's **digital delivery environment** is the set of systems that hold its
authoritative information and mediate its work: the schedule tool, the cost ledger, the requirements
and change registers, the document and model repository, the test and defect systems, the
procurement and contract records, the risk register, and the reporting layer assembled over them.

The environment is not a technology question, and treating it as one is the first error. It is an
**information architecture** question with a shape Domain 4 already priced. Each system holds facts
that other systems need, so each pair of systems is a candidate interface, and the count of possible
pairwise interfaces over `n` systems is the registered `n(n − 1)/2` (Domain 4, KA 4.2.3). Meridian's
environment has eleven systems of record; the mesh count is `11 × 10 / 2 =` **55** possible
interfaces against **11** connections to a single integration layer, and the practical consequence
is the one Domain 4 states: the mesh is cheaper for the first two or three systems and
catastrophically more expensive by the eleventh, because each interface is a thing to build, test,
version, secure and re-verify after every change on either side.

Three properties determine whether the environment helps or hinders, and all three are testable
before anything is procured. **Authority** — for each material fact, exactly one system is the
source of record, and the others hold copies marked as copies. An environment in which two systems
each believe they own the baseline dates has not integrated anything; it has arranged for a
disagreement. **Lineage** — for any number on a report, the path from source record to displayed
value is reconstructable. **Latency** — the age of the information at the point of use, which
14.2.1 computes.

**What the leader owns here.** Not the tool selection, which is usually an enterprise decision, but
four things that are never in a procurement document: which system is authoritative for which fact;
who may change each class of record and under what authority (which is Domain 3's delegation
schedule expressed in system permissions); what the environment must produce for the governance
body and by when; and what happens to the information at closeout, which Domain 16, KA 16.4 governs
and which is decided far too late in almost every programme.

### 14.1.2 Data governance

**Definition.** **Data governance** is the allocation of ownership, definition, quality
accountability and access rights over an organisation's data — the same four questions Domain 3
asked about decisions, asked about facts.

It is routinely written as a policy and therefore routinely ineffective. The workable form is a
**data class schedule** with one row per class of data the project relies on and five columns that
are each individually testable:

| Column | The test it must pass |
|---|---|
| **Owner** | A named person, not a function, accountable for the class being fit for its stated use. |
| **Definition** | One written definition per field, with units and permitted values, agreed across every system that holds it. |
| **Source of record** | The one system whose value prevails; every other holding is a copy. |
| **Quality standard** | The conformance rate required, stated **per class and per use**, with the consequence that sets it (14.1.4). |
| **Access and retention** | Who may read, who may change, how long it is kept, and on what basis it is destroyed. |

The **definition** column is the one that repays attention out of all proportion to its cost. A
field whose definition differs between two systems produces a reconciliation problem that presents
as a data quality problem and is actually a governance problem: nothing is wrong with either
value. PFL-AI's treatment of `CFADS` makes the same point in a financial register — a defined term
whose striking point changes every ratio built on it. In delivery the classic instances are
"forecast completion date" (contractual? current best estimate? with or without float
consumption?), "committed cost" (purchase order raised? goods received? invoiced?), and
"percentage complete" (Domain 7's `PoC` conventions). Each has more than one defensible meaning and
exactly one permissible meaning per organisation, and an organisation that has not chosen will
reconcile the difference for the life of the programme.

International reference points may be named accurately: the ISO 8000 series on data quality and
ISO/IEC 25012's data quality model give vocabulary and assessment structure for dimensions and
characteristics; ISO 19650's information-management series is the widely used frame in construction
and infrastructure; and ISO/IEC 38507 addresses the governance implications for an organisation of
using AI. None of them supplies a target number. The target is a project decision, and 14.1.4
derives it.

### 14.1.3 The common data environment

**Definition.** A **common data environment (CDE)** is a single agreed information store, with
defined states and transitions, through which project information is shared — such that every party
draws on the same version of the same fact, and the state of each item (work in progress, shared,
published, archived) is explicit.

The concept comes from information management in construction and infrastructure, where ISO 19650
frames it, and it generalises cleanly to any multi-party delivery. Its value is not storage; it is
the elimination of a specific failure that costs real money and is otherwise almost undetectable:
**two parties working correctly from different versions.** Domain 3's decision record made the same
move for decisions with its versioned information reference; the CDE makes it for the information
itself.

A claimed CDE is worth testing against five guarantees, and most fail at least one. **Single
instance** — there is one copy, referenced, not distributed as attachments. **Explicit state** — an
item's status is a property of the item, so nobody has to ask whether a drawing is issued.
**Controlled transition** — moving an item between states is an authorised act with a record, which
is what makes the environment auditable. **Access by role** — read and change rights follow the
data class schedule of 14.1.2, not convenience. **Retained history** — superseded versions remain
retrievable, because the question asked afterwards is always what was current on a given date.

Two cautions belong here because they are the ones that bite. A CDE is where a **project's** shared
information lives, and it should not become a repository for information that has a stricter home:
Meridian deliberately holds no patient clinical data in its delivery CDE, keeping the delivery
estate and the clinical estate separate, which is both a privacy decision (14.4.4) and a
simplification of the security problem. And a CDE **enforces nothing about quality** — it enforces
consistency of version. A single authoritative copy of a defective record is still a defective
record, propagated more efficiently, which is the subject of the rest of this KA.

### 14.1.4 Defect rate by data class and the exposure it carries

Domain 9, KA 9.4.3 established that a record satisfying six quality dimensions independently is
less conforming than any dimension suggests, because the dimensions compound — Auriga's asset
register scored **96.08 %** on the arithmetic mean of its dimensions and **78.43 %** on the product,
so roughly **690** of **3,200** records were unfit for use. That result is about one class of
record. The question this KA answers is the next one, and it is the question a leader actually
faces: **across many classes of data, where should remediation effort go, and what quality target
should each class carry?**

The wrong answer, and the one almost universally adopted, is a **single organisational target** —
"98 % data quality" — applied uniformly. It is wrong for a reason that is arithmetic rather than
philosophical, and the reason is worth stating precisely before the example demonstrates it: a
uniform rate target treats a defect as a defect, while the organisation's actual loss is
`defects × consequence`, and consequence varies across classes by two or three orders of magnitude.

**The two quantities.** For each data class `i` holding `nᵢ` records with observed defect rate `dᵢ`
and a consequence per defect `uᵢ` (the internally assessed cost of one defect reaching use):

```
class defects        = nᵢ · dᵢ
class exposure       = nᵢ · dᵢ · uᵢ
total exposure       = Σ nᵢ · dᵢ · uᵢ
exposure per record  = dᵢ · uᵢ          ← the quantity that ranks remediation
```

`uᵢ` is an internally assessed remediation-and-consequence cost, calibrated locally and stated as an
assumption; it is not a market price and it is not comparable between organisations.

**Worked example 14.1.4 — Meridian's common data environment, by class.**

1. **Setup.** Meridian's delivery CDE holds **14,100** records in six classes. Sampling gives the
   defect rate for each, and the programme has assessed the cost of one defect of each class
   reaching use:

   | Data class | Records `nᵢ` | Defect rate `dᵢ` | Cost per defect `uᵢ` |
   |---|---:|---:|---:|
   | Clinician accounts and role assignments | 4,800 | 1.5 % | USD 250 |
   | Device and network asset records | 1,600 | 4.0 % | USD 180 |
   | Interface field-mapping records | 900 | 6.0 % | USD 1,200 |
   | Training and competency records | 3,600 | 2.5 % | USD 90 |
   | Clinic readiness checklist records | 1,200 | 3.0 % | USD 400 |
   | Migration reconciliation records | 2,000 | 0.8 % | USD 3,500 |

2. **Formula.** Class defects `nᵢdᵢ`; class exposure `nᵢdᵢuᵢ`; total exposure `Σ nᵢdᵢuᵢ`; weighted
   mean defect rate `Σ nᵢdᵢ ÷ Σ nᵢ`; exposure per record `dᵢuᵢ`. For the uniform-target comparison,
   exposure at a common rate `d̄` is `d̄ · Σ nᵢuᵢ`.
3. **Substitution.** Defects `72, 64, 54, 90, 36, 16`. Exposures `18,000 · 11,520 · 64,800 · 8,100 ·
   14,400 · 56,000`. Uniform 2 % exposure `0.02 × 10,372,000`.
4. **Result.** **332** defective records — a weighted mean defect rate of **2.3546 %** — carrying a
   total exposure of **USD 172,820** (indicatively SAR 648,075). Exposure per record, which is the
   ranking that matters: interface mappings **USD 72.00**, migration reconciliation **USD 28.00**,
   readiness checklists **USD 12.00**, device records **USD 7.20**, accounts **USD 3.75**, training
   records **USD 2.25**. A uniform **2 %** target across all six classes produces **282** defects —
   **15.06 % fewer** — and an exposure of **USD 207,440**, which is **USD 34,620** or **20.03 %
   worse**.
5. **Interpretation.** The last line of the result is the whole KA, and it should be read twice: the
   uniform target **reduces the defect count by 15 % and increases the expected cost by 20 %**. It
   does so by mechanism, not by accident. It loosens the two classes whose defects are expensive —
   migration reconciliation from 0.8 % to 2 %, mappings implicitly permitted at 2 % — and tightens
   the classes whose defects are cheap, spending effort on training records at USD 2.25 of exposure
   per record while permitting reconciliation defects at USD 3,500 each. Any target expressed as a
   rate, with no consequence attached, has this property.

   **The remediation ranking inverts the rate ranking, and that is the practical payoff.** Ranked by
   defect rate the priority order is mappings (6.0 %), devices (4.0 %), readiness (3.0 %), training
   (2.5 %), accounts (1.5 %), reconciliation (0.8 %) — which puts the most expensive class last.
   Ranked by exposure per record it is mappings, reconciliation, readiness, devices, accounts,
   training. Test the two candidates that the two rankings disagree about most. Re-verifying all
   **900** interface mappings at **USD 22** per record costs **USD 19,800** and cuts the rate from
   6.0 % to 1.0 %, removing 45 defects worth **USD 54,000** — a net **USD 34,200**, and it would
   remain worthwhile up to a remediation cost of **USD 60.00** per record. Re-reconciling the
   **2,000** migration records at **USD 12** each costs **USD 24,000** and cuts 0.8 % to 0.2 %,
   removing 12 defects worth **USD 42,000** — a net **USD 18,000**, breakeven at **USD 21.00** per
   record, from the class with the *lowest* defect rate in the estate. Now the class the rate ranking
   would have reached fourth: re-checking all **3,600** training records at **USD 8** each costs
   **USD 28,800** to remove 72 defects worth **USD 6,480** — a net **loss of USD 22,320**, and it
   only becomes worthwhile at a remediation cost below **USD 1.80** per record. Three interventions,
   two of them strongly positive and one strongly negative, and nothing in the defect rates alone
   distinguishes them.

   Four professional cautions, each of which changes the answer if ignored. The consequence figures
   `uᵢ` are **assessed, not observed**, and the whole ranking is proportional to them — so they
   belong in the data class schedule with a named owner and a basis, and a class whose `uᵢ` nobody
   will own is a class whose quality target is arbitrary. This model treats a defect as **one
   discrete event with one cost**, which understates classes where a single defect propagates: a
   wrong interface mapping corrupts every record that passes through it, so its true `uᵢ` should be
   assessed per *mapping*, not per record that suffers, and Meridian's USD 1,200 is exactly such a
   propagated figure. Where a consequence is not cost-compensable — patient safety, a licence
   condition, a statutory notification — **expected value is the wrong test entirely**, the class
   carries an absolute standard, and the arithmetic here is used only to size the effort of meeting
   it, never to trade it away (the position Domain 9, KA 9.1 takes on safety-related requirements).
   And a defect rate is only meaningful against a **defined pass criterion** for the class, which is
   Domain 5, KA 5.4.2's testability requirement arriving as a prerequisite: rates measured against
   an undefined standard record the sampler's judgement.

> **Fig 14.1.1 — Defect rate against exposure: why a uniform data-quality target costs more.**
> Paired horizontal bars for Meridian's six CDE data classes. For each class, the upper bar shows the
> observed defect rate on a 0–7 % scale (mappings 6.0, devices 4.0, readiness 3.0, training 2.5,
> accounts 1.5, reconciliation 0.8) and the lower bar shows consequence-weighted exposure on a
> 0–70,000 scale (mappings 64,800 · reconciliation 56,000 · accounts 18,000 · readiness 14,400 ·
> devices 11,520 · training 8,100), with exposure per record printed at the right of each pair
> (72.00 · 28.00 · 3.75 · 12.00 · 7.20 · 2.25). Connecting lines make the rank inversion visible:
> reconciliation is last by rate and second by exposure, devices second by rate and fifth by
> exposure. A footer panel compares three regimes — observed **332 defects / USD 172,820**, a uniform
> 2 % target **282 defects / USD 207,440 (15.06 % fewer defects, 20.03 % more cost)**, and the
> consequence-proportional schedule of the case study **275 defects / USD 76,820 at a weighted mean
> rate of 1.9504 %**. Source: PCI original. Alt text: six pairs of horizontal bars in which the
> ordering by defect rate is visibly different from the ordering by expected cost, with a footer
> showing that a uniform two per cent target reduces defect counts while increasing total expected
> cost.

### AI in this KA

**Where it earns its place.** Profiling a data class against its defined pass criteria at full
population scale, which is mechanical, high-volume and exactly checkable — Domain 9, KA 9.4's
strongest AI application, and the input this KA's arithmetic consumes. Reconciling field definitions
across systems and producing the list of fields whose definitions differ, which is a
document-comparison task humans do badly because it is tedious and enormous. Proposing the
remediation rule that would fix the largest number of records in a class, for human ruling.
Detecting duplicate and near-duplicate records, where fuzzy matching genuinely outperforms exact
rules. Assembling the lineage path for a reported number across systems, so that a challenged figure
can be traced in minutes rather than days.

**Where it must not go.** It must not set a class's quality target or its consequence figure `uᵢ` —
those are risk-appetite judgements owned by the data owner and the sponsor, and a model asked for
them returns confident numbers with no provenance that then drive real remediation spending, the
defect Domain 3, KA 3.2.3 and Domain 10, KA 10.1 both identify. It must not **remediate silently**:
an inferred value that fills a completeness gap improves the metric and can degrade the register,
which is the worst available combination, and Domain 9, KA 9.4's prohibition applies unchanged here.
It must not be the source of record for anything — a generated value is a copy with no provenance.
And it must not decide which system is authoritative for a fact, which is a governance allocation.

**Verification, concretely.** Any machine profiling result is confirmed on a human-checked sample
with the sample size and its basis stated (Domain 9, KA 9.3.2). Where records have been
machine-remediated, an untouched control sample is held and compared, because a plausible wrong fill
is detectable only against ground truth — the general principle 14.3.4 prices. Definition
discrepancies are ruled on by the named field owner, not accepted as found. And the exposure
arithmetic is reproduced by hand: every figure in this KA is a multiplication of three numbers.

### Key terms — KA 14.1

| Term | Meaning |
|---|---|
| **Digital delivery environment** | The set of systems holding a project's authoritative information and mediating its work. |
| **Source of record** | The one system whose value for a fact prevails; all other holdings are copies. |
| **Lineage** | The reconstructable path from source record to a displayed value. |
| **Data class schedule** | One row per data class: owner, definition, source of record, quality standard, access and retention. |
| **Common data environment (CDE)** | A single agreed information store with explicit item states and authorised, recorded transitions. |
| **Defect rate by class (`dᵢ`)** | The share of records in a class failing that class's defined pass criteria. |
| **Consequence per defect (`uᵢ`)** | The internally assessed cost of one defect of a class reaching use; assessed, owned and stated. |
| **Consequence-weighted exposure** | `Σ nᵢdᵢuᵢ` — the expected cost of a data estate's defects. |
| **Exposure per record** | `dᵢuᵢ` — the quantity that ranks remediation effort across classes. |
| **Uniform-target defect** | Setting one rate target across classes of unequal consequence, which can cut defect counts while raising expected cost. |

### Sample MCQs — KA 14.1

**MCQ 14.1-A `[14.1.4 · Application]`** A data class holds 900 records with a 6.0 % defect rate and
an assessed consequence of USD 1,200 per defect. Its consequence-weighted exposure is:
- A. USD 54.00
- B. USD 72.00
- C. USD 64,800 ✅
- D. USD 1,080,000

*Rationale:* `900 × 0.06 × 1,200 = 64,800` (14.1.4). A is the defect count; B is the exposure *per
record* (`dᵢuᵢ`), which is the remediation ranking key, not the class exposure; D omits the defect
rate and prices every record as defective.

**MCQ 14.1-B `[14.1.4 · Evaluation]`** Six data classes with a weighted mean defect rate of 2.3546 %
carry a total exposure of USD 172,820. Imposing a uniform 2 % target on all six would produce 282
defects and an exposure of USD 207,440. The correct conclusion is that the uniform target:
- A. improves quality and reduces cost, since 282 is fewer than 332
- B. reduces the defect count by 15.06 % and raises expected cost by 20.03 %, because it loosens the
  low-rate, high-consequence classes and tightens the cheap ones ✅
- C. is acceptable because it is simpler to administer
- D. is equivalent to the observed position, since the mean rate is close to 2 %

*Rationale:* Cost is `defects × consequence`, and a rate target is blind to consequence (14.1.4). A
counts defects and ignores their price; D confuses a mean rate with an expected cost.

**MCQ 14.1-C `[14.1.4 · Analysis]`** Across six classes, remediation effort should be ranked by:
- A. defect rate `dᵢ`
- B. record count `nᵢ`
- C. exposure per record `dᵢuᵢ` ✅
- D. total class exposure `nᵢdᵢuᵢ`

*Rationale:* Remediation cost scales with records touched, so the return per record touched is
`dᵢuᵢ` (14.1.4). D ranks by size of prize without regard to the effort of claiming it, and would
direct effort to a large cheap class ahead of a small expensive one; A puts the lowest-rate,
highest-consequence class last.

**MCQ 14.1-D `[14.1.2 · Comprehension]`** Two systems report different "committed cost" for the same
purchase order, and both values are correct within their own system. The defect is:
- A. a data accuracy failure
- B. a definition failure — the field has more than one meaning across systems ✅
- C. an interface failure
- D. a completeness failure

*Rationale:* Nothing is wrong with either value; the field has no single agreed definition (14.1.2),
which is why the definition column of the data class schedule repays attention out of proportion to
its cost. It will present as a reconciliation problem indefinitely until a meaning is chosen.

**MCQ 14.1-E `[14.1.3 · Analysis]`** A programme stores every document in one repository with full
version history, but item status is recorded in a separate spreadsheet and changing status requires
no authorisation. Against the five CDE guarantees this arrangement fails on:
- A. single instance and retained history
- B. explicit state and controlled transition ✅
- C. access by role only
- D. nothing — it is a valid CDE

*Rationale:* Status is not a property of the item and transitions are unauthorised and unrecorded
(14.1.3). Single instance and retained history are satisfied; the two failures are precisely the
ones that make an environment unauditable.

### Self-check — KA 14.1

1. *Why can a uniform data-quality target reduce defect counts and increase expected cost?* —
   Because loss is `defects × consequence` and a rate target is blind to consequence: it loosens
   low-rate, high-consequence classes and tightens cheap ones. Meridian's case: 15.06 % fewer
   defects, 20.03 % more cost.
2. *What ranks remediation across classes, and why is it not the defect rate?* — Exposure per
   record, `dᵢuᵢ`, because remediation cost scales with records touched. Meridian's lowest-rate class
   (0.8 %) is its second-highest by exposure per record (USD 28.00).
3. *What does a common data environment guarantee, and what does it not?* — Consistency of version
   through explicit states and authorised, recorded transitions; it guarantees nothing about quality,
   so a defective record is simply propagated more efficiently.

---

## Knowledge Area 14.2 — Dashboards, analytics, digital twins and automation

*Topics: 14.2.1 dashboards and the age of a fact · 14.2.2 the analytics ladder · 14.2.3 digital
twins and the fidelity that pays · 14.2.4 automation economics.*

### 14.2.1 Dashboards and the age of a fact

**Definition.** A **dashboard** is a presentation of measures selected to support a defined set of
decisions at a defined cadence. Every word in that constrains it, and the standard failure is to
build a presentation of the measures that are *available* to an audience with no decision to make.

Domain 11, KA 11.2 governs what an executive report must contain and prohibits the smoothing of its
warnings. This topic addresses the property Domain 11 did not price and that determines whether a
dashboard can inform anything at all: **the age of the information at the moment of decision.**

The arithmetic is already in the registry. Domain 3, KA 3.2.3 established that a decision arising at
a uniformly random point in a cycle of length `M`, with a paper lead time `L`, waits
`E[wait] = M/2 + L`. A *fact* arising at a uniformly random point in a reporting cycle of period `R`,
where production takes a lag `G` from data cut-off to publication, is by the identical argument
expected to be **`R/2 + G`** old when it is published. The derivation is Domain 3's and is not
repeated; only the interpretation changes — the quantity is now the staleness of information rather
than the wait for a decision, and its two levers behave in the same asymmetric way.

**Worked example 14.2.1 — how old is the fact on Meridian's report?**

1. **Setup.** Meridian's rollout status is reported monthly: data cut-off at month end, then
   extraction, validation, review and commentary before publication, a production lag of **9 days**.
   The programme is considering weekly reporting with a compressed production lag of **4 days**. Cost
   of delay **USD 14,280 per week**, i.e. **USD 2,040 per day**.
2. **Formula.** Expected age of a fact at publication `= R/2 + G`, the registered `E[wait]` identity
   (Domain 3, KA 3.2.3) applied to information. Cost of acting that late on a detectable slip
   `= age × cost of delay per day`.
3. **Substitution.** Monthly `30/2 + 9`. Weekly `7/2 + 4`. Costs `24 × 2,040` and `7.5 × 2,040`.
4. **Result.** On the monthly cycle a fact is on average **24 days** old when the report is
   published — **3.4286 weeks** — worth **USD 48,960** at the programme's cost of delay. On the
   weekly cycle with a 4-day lag it is **7.5 days** old, worth **USD 15,300**. The change is worth
   **USD 33,660** per detectable slip.
5. **Interpretation.** Three things follow, and the third is the one that changes behaviour. First,
   the *monthly* report that everybody treats as current describes a world nearly three and a half
   weeks gone, which is why a slip is so often reported as new when the people doing the work have
   known about it for a month — the report is not concealing it, the cycle is. Second, the lever
   asymmetry carries over exactly from Domain 3: a one-day cut in the **production lag `G`** saves a
   full day of staleness, while a one-day cut in the **reporting period `R`** saves only half a day.
   Compressing the production process is twice as effective per day as reporting more often, and it
   is the cheaper of the two to change — the same result Domain 3 obtained for paper lead times, and
   for the same reason. Third, and decisively: **a dashboard whose information age exceeds the
   interval within which the decision must change cannot inform that decision, at any level of
   presentation quality.** Meridian's clinic readiness interventions must be triggered at least two
   weeks before a scheduled go-live to be effective; a 24-day-old fact arrives after the window has
   closed, so the monthly report is, for that decision, decoration. The professional response is not
   a better chart. It is either a faster cycle for that specific measure, or an exception trigger
   that fires on the source system rather than waiting for the report — and the honest test to apply
   to any dashboard is to name the decision, name the window, compute `R/2 + G`, and compare.

   Two cautions. `R/2 + G` assumes facts arise uniformly through the cycle, which is a reasonable
   default and wrong where events cluster at period boundaries (invoicing, sprint ends, gate
   submissions) — for those, model the actual arrival pattern rather than the average. And a shorter
   cycle imposes real production cost: at the blended rate of USD 110 per hour, a report taking 14
   person-hours to produce costs **USD 1,540** each time, so moving from monthly to weekly adds
   **3.2857** productions a month (`30/7 − 1`) and **USD 5,060** of monthly effort. That is worth paying
   against USD 33,660 of avoided lateness on a single slip, and it must still be paid rather than
   assumed away.

### 14.2.2 The analytics ladder

Analytics initiatives are usually described by their technology and should be described by the
question they answer, because the four questions differ in what they require and what they are
worth. **Descriptive** — what happened? Requires only clean data and correct aggregation, and is
where the great majority of realised value in delivery analytics sits, unglamorously. **Diagnostic**
— why did it happen? Requires the ability to decompose a measure into contributions, which in turn
requires the data class schedule of 14.1.2 to be coherent. **Predictive** — what is likely to
happen? Requires history that is genuinely comparable to the future being predicted, which is a
strong assumption on projects and a fatal one on novel work. **Prescriptive** — what should be done?
Requires a model of the levers *and* of their costs, and it is where the accountability question
becomes acute, because a recommendation presented in a system's voice is very hard for a governance
body to interrogate (Domain 3, KA 3.A.2).

**The test each rung must pass, and the ordering rule.** For each rung: *is there a named decision
that would change if this output changed?* An analytics product with no such decision is a cost.
And the ordering rule is unglamorous and reliable: **the rungs must be climbed in order, because
each depends on the one below being right.** A predictive model built on a register with a 6 %
mapping defect rate predicts the defects. The standard sequencing error in digital delivery, stated
plainly, is to buy the analytics before fixing the data — which is why 14.1 precedes 14.2 in this
domain and should precede it in a programme's plan.

### 14.2.3 Digital twins and the fidelity that pays

**Definition.** A **digital twin** is a model of a physical or process asset that is kept
synchronised with the real asset through a data connection, such that questions can be asked of the
model instead of the asset. The synchronisation is what distinguishes a twin from a design model or
a simulation; a model that is not updated from the asset is a drawing, whatever it is called, and
much of what is marketed as a twin is a drawing.

Twins earn their place in delivery for three uses: **rehearsal** (testing a sequence off-line
before committing an irreversible operation), **handover** (a validated as-built information set the
receiving organisation can operate from, which is Domain 16, KA 16.1's readiness problem), and
**training** (operators competent before go-live rather than during it). Their value in all three
depends on one property, and it is the property least often measured: **fidelity**, the share of
questions on which the twin's answer matches the asset's behaviour within a stated tolerance.

**Auriga's control-system twin, priced.** The twin costs **USD 180,000** to build and **USD 3,500**
a month to keep synchronised over the **9** months to handover — **USD 211,500** all in. Its two
benefit streams, both assessed locally: avoiding **3** of the 8 live outage windows required for
sequence testing at **USD 62,000** each, worth **USD 186,000**; and operator training plus the
as-built information handover, assessed at **USD 120,000**. Gross benefit at perfect fidelity is
therefore **USD 306,000**, and the **breakeven fidelity** is `211,500 / 306,000 =` **69.12 %**.
Measured against the commissioned plant on **40** test cases, the twin agreed within tolerance on
**34** — a measured fidelity of **85.00 %**, giving an expected net value of
`0.85 × 306,000 − 211,500 =` **USD 48,600**.

That is a positive answer with a thin margin, and the thinness is the lesson. Measured fidelity sits
only **15.88 percentage points** above breakeven, and it was measured on 40 cases. If the next 40
cases produce 10 failures, pooled fidelity is `64/80 =` **80.00 %** and expected net value falls to
**USD 33,300**; a pooled fidelity of 69.12 % takes it to zero. Two structural cautions must
accompany any such calculation. Scaling benefit linearly with fidelity assumes the twin's failures
are **distributed across decisions in proportion to their value**, and they are not: a twin
typically fails on the novel, coupled, poorly instrumented cases, which are exactly the cases worth
rehearsing, so linear scaling is optimistic and the failures should be examined individually rather
than counted. And a twin's fidelity **decays** — the asset is modified, the model is not, and the
synchronisation that defines a twin is the first budget line cut after handover, which is why
14.A.2's revalidation cadence applies to twins as much as to models.

### 14.2.4 Automation economics

**Definition.** **Automation** here means replacing a repeated human task with a machine process —
rule-based scripting, robotic process automation, or an AI-assisted step. The decision is an
economic one with a well-known form and a routinely omitted term.

The familiar form is a breakeven volume. With a fixed build-and-maintain cost `F` over the horizon
considered, a manual unit cost `m` and an automated unit cost `a`:

```
n* = F / (m − a)
```

The omitted term is quality. An automated step and a manual step do not have the same **error escape
rate**, and errors that escape cost money. Writing `eₘ` and `eₐ` for the share of units on which each
route lets a material error through, and `u` for the cost of one escaped error, the honest unit costs
are `m + eₘu` and `a + eₐu`, and the **quality-adjusted breakeven volume** is:

```
n* = F / [ (m + eₘu) − (a + eₐu) ]
```

Three consequences follow immediately and are worth stating before the example. If automation
**raises** the escape rate, the breakeven volume rises and may exceed any volume the organisation
will ever process. If automation **lowers** it — which deterministic rule-checking often does, since
machines do not tire — the breakeven volume falls below the naive figure, and the naive calculation
*understates* the case. And if `(a + eₐu) > (m + eₘu)`, **no volume justifies the automation**: the
breakeven volume does not exist, and a business case expressed in visible unit costs will
nonetheless show a healthy one, which is the arithmetic behind Case study B.

**Worked example 14.2.4 — automating Auriga's handover data checks.**

1. **Setup.** Auriga's **60** handover packages each require **12** data-consistency checks against
   the asset register — **720** checks on this project. A check takes an engineer **24 minutes** at
   the blended engineering rate of **USD 130.625** per hour. An automated checker costs
   **USD 46,000** to build and **USD 1,400** a month to maintain, and processes a check for
   **USD 4.25** of exception-handling time. Consider a **24-month** horizon. Measured on a held-back
   set, the manual route lets a material error through on **1.5 %** of checks and the automated route
   on **2.5 %**; an escaped error costs **USD 1,800** (a re-issued package, a repeated site visit,
   a corrected handover record). The utility runs **6** comparable projects a year.
2. **Formula.** Naive `n* = F/(m − a)`; quality-adjusted `n* = F/[(m + eₘu) − (a + eₐu)]`, with
   `F` = build + maintenance over the horizon.
3. **Substitution.** `m = 130.625 × 24/60 = 52.25`; `a = 4.25`. `F = 46,000 + 1,400 × 24 = 79,600`.
   Naive `79,600/48.00`. Adjusted `m + 0.015 × 1,800 = 79.25`; `a + 0.025 × 1,800 = 49.25`;
   `79,600/30.00`.
4. **Result.** Build cost alone breaks even at **958.33 → 959** checks. Including 24 months of
   maintenance the naive breakeven is **1,658.33 → 1,659** checks. The **quality-adjusted** breakeven
   is **2,653.33 → 2,654** checks — exactly **1.60 times** the naive figure, since the unit saving
   falls from USD 48.00 to USD 30.00. Auriga alone generates **720** checks: below every one of those
   thresholds. Across the portfolio the volume is **4,320** a year, **8,640** over 24 months, giving
   a quality-adjusted net of **USD 179,600** against a naive claim of **USD 335,120**.
5. **Interpretation.** The decision reverses depending on the boundary drawn, and both answers are
   correct within their boundary — which is why the boundary is the first thing to state. **For
   Auriga alone the automation is indefensible**: 720 checks against a build-only breakeven of 959,
   so the project would spend USD 46,000 to save USD 34,560 of visible effort even before quality
   is considered. **For the portfolio it is strongly positive**, and the case must therefore be made,
   funded and owned at portfolio level (Domain 15), with the tool treated as an asset with a
   maintainer rather than as a project deliverable. A project-funded automation with no portfolio
   owner is the standard way an organisation acquires an unmaintained tool that quietly stops being
   verified.

   **The quality term is the professional content of this example.** A 1.0-percentage-point increase
   in the escape rate — from 1.5 % to 2.5 %, a difference no business case would notice — raises the
   breakeven volume by 60 % and removes **USD 155,520** from the 24-month net. Two design responses
   follow. If the automated route can be made *better* than the human one rather than worse, the
   arithmetic transforms: at an automated escape rate of **0.5 %** the adjusted unit cost is
   **USD 13.25**, the unit saving is USD 66.00, and the breakeven falls to **1,206.06 → 1,207**
   checks, below the naive figure. Machines are genuinely better than tired humans at exhaustive
   rule-checking, so this is often achievable and almost never measured. Alternatively, place a cheap
   containment layer after the automated step so that `u` itself falls — Domain 9, KA 9.4.4's result
   that the economics reward **building a containment chain around machine output** rather than
   reviewing harder. Three cautions: the escape rates must be **measured against a held-back set**,
   never asserted, and re-measured on any change of rule set, model or artefact type (14.A.2);
   maintenance is a real recurring cost that is systematically omitted, and omitting it here would
   have made the breakeven 959 instead of 1,659; and the automated route's exception-handling cost
   `a` must include the human time actually spent on exceptions, which in practice is where an
   automation's promised saving most often disappears.

### AI in this KA

**Where it earns its place.** Generating the exception logic for a dashboard — the rules that decide
what is worth surfacing — for human ruling, which is a genuine strength because the rules are
testable against history. Producing the first draft of an aggregation or transformation, with the
lineage stated, for review. Detecting anomalies in high-volume operational data where the anomaly
class is defined and the model's output is a *prompt to investigate* rather than a finding.
Reconciling a twin's predictions against measured behaviour across a large test set and clustering
the disagreements by type, which is how a fidelity measure becomes diagnostic rather than a
percentage. Modelling breakeven volumes across dozens of automation candidates, which is
deterministic and verifiable.

**Where it must not go.** It must not select the measures on a governance dashboard: that is a
statement about what the organisation will manage, and it belongs to the accountable body (Domain 11,
KA 11.2). It must not supply the escape rates, defect rates, consequence figures or fidelity
tolerances that these calculations consume — asked for any of them a model will return a confident,
well-formatted number with no provenance, and it will then anchor a real capital decision. It must
not write the commentary that accompanies a status measure, for the smoothing reason Domain 11
established: a model asked to make a report clearer reliably produces a calmer one, and the
qualifier carrying the warning is the first casualty. And it must not be the **only** step between a
generated output and an irreversible operation, which is what a twin-based rehearsal exists to
prevent.

**Verification, concretely.** Every automation carries a **measured** escape rate against a
held-back set, re-measured on change, and a named owner for the measurement. Every dashboard measure
carries its `R/2 + G` information age on the specification, so the age is a published property
rather than a discovery. A twin carries its fidelity, the test-case count it was measured on, the
date of measurement and the tolerance used. And the breakeven arithmetic in any automation business
case is reproduced by hand — it is one division — with the maintenance term and the quality term
both visible, since a case presenting neither has not been reviewed.

### Key terms — KA 14.2

| Term | Meaning |
|---|---|
| **Information age** | `R/2 + G` — the expected staleness of a fact at publication, for reporting period `R` and production lag `G`; the registered `E[wait]` identity applied to information. |
| **Production lag (`G`)** | Elapsed time from data cut-off to publication; the more powerful of the two staleness levers, day for day. |
| **Analytics ladder** | Descriptive → diagnostic → predictive → prescriptive; each rung depends on the one below being right. |
| **Digital twin** | A model kept synchronised with a real asset through a data connection; without synchronisation it is a drawing. |
| **Twin fidelity** | The share of test questions on which the twin's answer matches the asset within a stated tolerance. |
| **Breakeven fidelity** | Twin cost ÷ gross benefit at perfect fidelity — the fidelity below which the twin destroys value. |
| **Error escape rate (`e`)** | The share of units on which a route lets a material error through; differs between manual and automated routes and must be measured. |
| **Quality-adjusted breakeven volume** | `F / [(m + eₘu) − (a + eₐu)]` — the volume at which an automation pays once escaped errors are priced. |

### Sample MCQs — KA 14.2

**MCQ 14.2-A `[14.2.1 · Application]`** A programme reports monthly with a 9-day production lag from
data cut-off to publication. The expected age of a fact at publication is:
- A. 9 days
- B. 15 days
- C. 24 days ✅
- D. 39 days

*Rationale:* `R/2 + G = 30/2 + 9 = 24` days (14.2.1, applying Domain 3's `E[wait]` identity). A
counts only the lag; B only half the period; D adds the whole period to the lag.

**MCQ 14.2-B `[14.2.1 · Analysis]`** For that report, which change reduces information age more, and
by how much: cutting the production lag by 3 days, or cutting the reporting period by 3 days?
- A. the reporting period, by 3.0 days
- B. the production lag, by 3.0 days — twice the 1.5-day saving from the period ✅
- C. both equally, by 3.0 days
- D. both equally, by 1.5 days

*Rationale:* Age is `R/2 + G`, so a cut of `x` in `G` saves `x` while a cut of `x` in `R` saves
`x/2` — the same asymmetry Domain 3, KA 3.2.3 found for paper lead times, and the lag is usually the
cheaper lever to move.

**MCQ 14.2-C `[14.2.4 · Application]`** An automation costs USD 79,600 over the horizon. Manual unit
cost is USD 52.25 and automated USD 4.25; escape rates are 1.5 % manual and 2.5 % automated with an
escaped error costing USD 1,800. The quality-adjusted breakeven volume is closest to:
- A. 959 units
- B. 1,659 units
- C. 2,654 units ✅
- D. 1,524 units

*Rationale:* Adjusted unit costs are `52.25 + 27.00 = 79.25` and `4.25 + 45.00 = 49.25`, so
`79,600/30.00 = 2,653.33 → 2,654` (14.2.4). A uses the build cost alone with unadjusted units; B is
the naive figure including maintenance; D divides the fixed cost by the manual unit cost instead of
by the saving.

**MCQ 14.2-D `[14.2.4 · Evaluation]`** An automation's quality-adjusted automated unit cost exceeds
its quality-adjusted manual unit cost. The correct conclusion is that:
- A. the breakeven volume is very large but attainable at portfolio scale
- B. no volume justifies the automation; the breakeven volume does not exist ✅
- C. the automation is justified if the visible unit saving is positive
- D. the escape rates should be excluded as they are estimates

*Rationale:* With a negative unit saving every additional unit adds loss, so there is no crossover
(14.2.4) — the position Case study B describes. C is exactly the error that produces it; D discards
the term that decides the answer.

**MCQ 14.2-E `[14.2.3 · Application]`** A digital twin costs USD 211,500 and would deliver
USD 306,000 of benefit at perfect fidelity. Its breakeven fidelity is:
- A. 58.82 %
- B. 69.12 % ✅
- C. 85.00 %
- D. 113.71 %

*Rationale:* `211,500/306,000 = 69.12 %` (14.2.3). A divides the build cost alone by the benefit,
omitting nine months of synchronisation; C is the measured fidelity, not the breakeven; D divides by
one benefit stream only.

### Self-check — KA 14.2

1. *How old is a fact on a monthly report with a 9-day production lag, and what is it worth at
   Meridian's cost of delay?* — 24 days (`30/2 + 9`), worth USD 48,960 at USD 2,040 a day; a weekly
   cycle with a 4-day lag gives 7.5 days and USD 15,300.
2. *What distinguishes a digital twin from a design model, and what determines its value?* —
   Synchronisation with the real asset; and fidelity, measured against the asset on a stated number
   of test cases with a stated tolerance, against a computed breakeven — 69.12 % for Auriga's twin.
3. *Which term is systematically omitted from automation business cases, and what does it do?* — The
   differential error escape rate. One percentage point of extra escape raised Auriga's breakeven
   volume by 60 %, and a negative quality-adjusted saving removes the breakeven altogether.

---

## Knowledge Area 14.3 — AI use across the lifecycle, prompting and verification

*Topics: 14.3.1 where AI earns its place across the lifecycle · 14.3.2 prompting, grounding and
provenance · 14.3.3 verification-effort economics and the standard it derives · 14.3.4 the plausible
wrong number.*

### 14.3.1 Where AI earns its place across the lifecycle

The preceding thirteen domains have each stated, in their own subject matter, what AI may and may
not do. Restating those positions here would be a defect. What this topic contributes instead is the
**pattern** they collectively describe, because once the pattern is visible it can be applied to a
use case nobody has yet written a rule for — which is the situation a leader is actually in.

Across every domain in this volume, the permitted uses fall into five classes and the prohibited
uses into four. **The five permitted classes:** *extraction* (pulling structure out of unstructured
material — a decision register from minutes, Domain 3, KA 3.3; requirements candidates from
interview notes, Domain 5, KA 5.1); *comparison at scale* (reading many documents against each other
for inconsistency — decision rights across a contract and a terms of reference, Domain 3, KA 3.1;
field definitions across systems, 14.1.2); *enumeration* (generating candidates a human will rule
on — risks, test cases, failure modes, improvement actions, Domains 8 and 9); *deterministic
modelling* (running arithmetic a human specified across many combinations — latency and threshold
models in Domain 3, breakeven volumes in 14.2.4); and *drafting for review* (producing a first
version of a document whose author remains the named human, Domains 4, 5, 10, 11). Each is
characterised by a verifiable output and a human ruling.

**The four prohibited classes**, and this is the more useful list because it generalises:
*attributable acts* — anything that confers authority or discharges an obligation: approving a
change, accepting a deliverable, signing a test record, granting a concession, authoring a decision
record (Domains 3, 4, 5, 9). *Judgements of consequence and appetite* — thresholds, liability caps,
target costs, quality scores, risk appetites, kill criteria; these are risk-appetite decisions
belonging to accountable people (Domains 2, 3, 9, 10). *Unprovenanced parameters* — supplying the
probability, detection rate, critical-path share, escape rate, reservation value or benefit
attribution that a calculation then consumes; the model will supply a confident number and it will
enter a real decision as though it were data (Domains 3, 5, 8, 10, 11 and every worked example in
this domain). *Inference about people* — emotional state, engagement, personality, likely departure,
cultural disposition, performance or potential, whether from telemetry, text or images; Domain 12
states the prohibition in its strongest form in this volume, and this domain does not soften it.

The single test that generates all four prohibitions, and the one to apply to a novel case: **could a
named human be held to answer for this output as their own judgement, on evidence they can produce?**
If yes, AI assistance is a drafting aid and the human remains the author. If no — because the output
*is* the exercise of an authority, or because it rests on a parameter nobody can source, or because
the subject is a person's interior state — the use is not permitted at any level of verification,
because no amount of checking creates an accountable author.

**Across the lifecycle**, the same pattern maps to stages. In *definition and selection*, extraction
and enumeration are strong and benefit attribution is prohibited (Domain 2; Domain 5, KA 5.3). In
*planning*, deterministic modelling and scenario enumeration are strong while estimates and durations
are not (Domains 6 and 7). In *execution*, extraction, anomaly detection and drafting are strong
while approvals, acceptances and classifications are prohibited (Domains 4, 5, 9, 10). In *closeout
and benefits*, clustering and summarisation are strong while the benefit judgement and the archive
decision remain human (Domain 16). The prohibitions do not weaken as a project matures; the volume
of permitted use grows, which is why the verification standard of 14.3.3 has to be a standing
instrument rather than a mobilisation exercise.

### 14.3.2 Prompting, grounding and provenance

A prompt is not a conversational convenience; where its output informs a professional judgement, it
is a **method**, and it acquires the obligations of a method: it is written down, versioned,
reviewed and reproducible. That single reframing removes most of the practical problems
organisations report with AI assistance, and it is resisted mainly because prompts feel like typing.

**The four properties of a professional prompt.** *Grounding* — the material the output must be
derived from is supplied and identified, and the instruction states that nothing outside it may be
used. An ungrounded prompt asks for recall, and recall is where fabrication lives. *Constraint* —
the output's form is specified (fields, units, allowed values, "state 'not stated' where the source
is silent"), because an unconstrained output cannot be checked mechanically. *Refusal permission* —
the instruction explicitly permits, and expects, an answer of "the source does not say", which is
the single most valuable line in a professional prompt because its absence guarantees that gaps are
filled with plausible content. *Provenance* — each material assertion in the output cites the
supplied source location it came from, which is what converts verification from re-reading
everything to checking citations.

**What must be recorded.** For any AI-assisted output that will be relied on: the prompt version,
the model and version, the grounding material and its version, the date, the named human who
reviewed it, and the verification tier applied (14.3.3). This is not bureaucracy; it is the minimum
that lets the question asked afterwards be answered. Domain 3, KA 3.3.4's finding applies unchanged:
the retrospective question is never "was the output right?" but "was the decision reasonable on what
was known at the time?", and only a versioned record answers it.

**Two failure modes specific to prompting**, both common and both cheap to prevent. **Instruction
drift**: a prompt refined over many sessions in a chat window is not the prompt anyone thinks it is,
and its output is not reproducible; the fix is that the operative prompt lives in a file with a
version, not in a history. **Context contamination**: material from an unrelated matter remains in a
session and influences an output, which is both a correctness problem and, where the material is
confidential or personal, a disclosure problem — the boundary Domain 3, KA 3.A.2 requires to be
stated before use rather than after an incident.

### 14.3.3 Verification-effort economics and the standard it derives

Domain 3, KA 3.A.2 required a **verification standard proportional to consequence** and asserted it
without deriving it. Domain 9, KA 9.4.4 supplied one axis of the derivation: given a population of
machine-produced items, how large a sample must be checked, from the breakeven error fraction
`p* = c/u` and the bound a clean sample supports. This topic supplies the other axis, and it is the
one an organisation must answer first: **for a given class of output, how deep should the check be?**

**The structure.** Verification is available at increasing depths. Each depth `k` has a cost per
item `vₖ` and a **detection rate** `qₖ` — the share of material errors present that it finds. For an
output class whose items contain a material error with probability `p`, and where an escaped error
costs `u`:

```
expected cost at tier k  =  vₖ + p (1 − qₖ) u
```

Moving from tier `k − 1` to tier `k` is worth doing when the extra cost is less than the extra
expected loss avoided, `Δvₖ < p · Δqₖ · u`, which rearranges into the quantity a standard is written
from — **the consequence at which the next tier begins to pay**:

```
u*ₖ  =  Δvₖ / ( p · Δqₖ )
```

Three properties of that expression should be read before any numbers. The thresholds depend on
`Δv` and `Δq`, the **increments**, not on the totals — so the natural but wrong comparison of a
tier's whole cost against the whole consequence over-verifies, because every tier looks cheap
against a large consequence. The thresholds are **inversely proportional to `p`**, so a class's
required depth falls as the producing process improves, which is where the productivity gain of AI
assistance actually lives. And `p` and `q` are **measured quantities**: a standard built on asserted
ones is a standard built on nothing, which is why 14.A.2 makes their measurement a standing
obligation.

**Worked example 14.3.3 — deriving Meridian's verification standard, and pricing it.**

1. **Setup.** Meridian's AI-assisted outputs are reviewed at one of five depths, costed at the
   blended programme rate of **USD 110 per hour**, with detection rates measured against a held-back
   set of seeded errors: tier 0 accept unchecked (`v` = 0, `q` = 0); tier 1 author scan, 6 minutes
   (**USD 11.00**, `q` = **0.35**); tier 2 independent review, 24 minutes (**USD 44.00**,
   `q` = **0.70**); tier 3 independent review with source trace, 66 minutes (**USD 121.00**,
   `q` = **0.90**); tier 4 two independent reviewers with reperformance, 150 minutes
   (**USD 275.00**, `q` = **0.97**). The measured material-error rate of the current
   model-and-prompt configuration is `p` = **0.12**. Monthly output volumes and assessed consequences
   per escaped error: internal meeting summaries **620** at **USD 150**; clinic readiness
   assessments **180** at **USD 480**; configuration field-mapping proposals **95** at
   **USD 1,200**; migration reconciliation scripts **40** at **USD 3,500**; baseline change impact
   assessments **6** at **USD 28,560** (two weeks of programme delay at the cost of delay of
   USD 14,280 per week).
2. **Formula.** `u*ₖ = Δvₖ / (p · Δqₖ)` for each tier step; then, for any assignment of tiers to
   classes, total cost `= Σ nᵢvₖ(ᵢ) + Σ nᵢ p (1 − qₖ(ᵢ)) uᵢ`.
3. **Substitution.** `11/(0.12 × 0.35)`; `33/(0.12 × 0.35)`; `77/(0.12 × 0.20)`;
   `154/(0.12 × 0.07)`.
4. **Result — the standard.**

   | Step | `Δv` | `Δq` | Consequence at which it pays, `u*` | Applies from |
   |---|---:|---:|---:|---|
   | 0 → 1 author scan | USD 11.00 | 0.35 | **USD 261.90** | any output whose escaped error costs more than 261.90 |
   | 1 → 2 independent review | USD 33.00 | 0.35 | **USD 785.71** | " more than 785.71 |
   | 2 → 3 review with source trace | USD 77.00 | 0.20 | **USD 3,208.33** | " more than 3,208.33 |
   | 3 → 4 two reviewers, reperformance | USD 154.00 | 0.07 | **USD 18,333.33** | " more than 18,333.33 |

   Applying it: summaries (150) → **tier 0**; readiness assessments (480) → **tier 1**; mapping
   proposals (1,200) → **tier 2**; reconciliation scripts (3,500) → **tier 3**; change impact
   assessments (28,560) → **tier 4**. Monthly verification cost **USD 12,650**; expected escaped
   loss **USD 24,300.10**; **total USD 36,950.10**.

   The three alternatives, on the same 941 outputs. **No verification:** USD 0 + **USD 72,571.20** =
   **USD 72,571.20**. **Uniform tier 2 on everything** — the intuitively "consistent" policy:
   USD 41,404 + USD 21,771.36 = **USD 63,175.36**. **Uniform tier 4 on everything** — the
   intuitively "safe" policy: USD 258,775 + USD 2,177.14 = **USD 260,952.14**. The tiered standard
   is the cheapest of the four, beating uniform tier 2 by **USD 26,225.26 (41.51 %)** and uniform
   tier 4 by a factor of **7.06**.
5. **Interpretation.** Start with the comparison that changes minds. Uniform tier 2 spends
   **3.27 times** as much on verification as the tiered standard — USD 41,404 against USD 12,650 —
   and buys a reduction in escaped loss of **USD 2,528.74**, or **10.41 %**. That is the arithmetic
   signature of an undifferentiated review policy: most of the money goes to items whose errors are
   cheap, because that is where most of the items are. The tiered standard is not a relaxation of
   uniform tier 2; it is a **reallocation** — it removes review from 620 low-consequence items and
   adds review depth to 46 high-consequence ones, and it is better on both cost and loss because the
   consequences differ by a factor of **190.4** (28,560 against 150) while the review costs
   differ by a factor of **25.0** (275.00 against 11.00).

   **The uncomfortable feature, stated plainly, because a governance body will find it.** Under the
   tiered standard, **45.93 %** of all expected escaped loss — USD 11,160 of USD 24,300.10 — sits in
   the one class deliberately left unverified. That looks like negligence and it is not: putting the
   620 summaries on tier 1 costs **USD 6,820** and saves **USD 3,906** of escaped loss, a **net loss
   of USD 2,914**, which is exactly what a consequence of USD 150 sitting below the USD 261.90
   threshold means. The correct response to a large aggregate of cheap errors is not to check them;
   it is to **reduce `p` or `u`** — a better prompt, a template, a structural constraint, a cheap
   containment layer downstream (Domain 9, KA 9.4.4). Checking is the most expensive of the available
   remedies and the only one organisations reliably reach for.

   **The sensitivity that matters most is `p`, and it points somewhere unexpected.** If the
   configuration improves so that the measured error rate falls from 0.12 to **0.04**, every
   threshold triples — to **785.71 · 2,357.14 · 9,625.00 · 55,000.00** — and the assignment shifts
   down: summaries and readiness assessments to tier 0, mappings to tier 1, scripts to tier 2, impact
   assessments to tier 3. Total cost falls from USD 36,950.10 to **USD 16,036.44**, a **56.60 %**
   reduction, of which the **verification bill** falls by **72.09 %** (USD 12,650 → USD 3,531). This
   is the honest form of the AI productivity claim: **the return on a better model is mostly a
   reduction in checking, not a reduction in drafting time** — and it is realisable only by an
   organisation that measures `p`, because an unmeasured error rate leaves the verification standard
   frozen at whatever it was set to on the day someone was nervous.

   Four cautions. Every threshold scales with `1/p`, so `p` must be **measured per output class and
   per configuration**, and a single organisational `p` is as wrong as a single organisational
   quality target (14.1.4). Detection rates `q` must be measured against **seeded or held-back
   errors**, and Domain 9, KA 9.2.2's independence rule binds: two passes by the same model are one
   pass, and a model reviewing its own output is not a tier. Where the consequence is not
   cost-compensable — a safety-related output, a regulatory submission, a clinical decision support
   artefact — the class carries an **absolute** standard and this arithmetic sizes the effort rather
   than choosing the depth. And the standard must be **published and owned**, because its value lies
   in being applied consistently by people who are not doing the calculation each time; an
   unpublished standard collapses into individual nervousness, which is the condition it exists to
   replace.

> **Fig 14.3.1 — The verification standard proportional to consequence.** Step chart. Horizontal
> axis: consequence of one escaped error, `u`, on a logarithmic scale from USD 100 to USD 100,000.
> Vertical axis: required verification tier, 0 to 4, drawn as a rising staircase with risers at the
> derived thresholds **USD 261.90 · 785.71 · 3,208.33 · 18,333.33**, each riser labelled with its
> `Δv/(p·Δq)` derivation and the tier's cost and detection rate (tier 1 USD 11.00 / q 0.35; tier 2
> USD 44.00 / 0.70; tier 3 USD 121.00 / 0.90; tier 4 USD 275.00 / 0.97). Meridian's five output
> classes are plotted as crimson markers at their consequences with monthly volumes — summaries
> 150 (n 620), readiness 480 (n 180), mappings 1,200 (n 95), scripts 3,500 (n 40), impact assessments
> 28,560 (n 6) — sitting on the step each falls into. A dashed grey staircase shows the same standard
> at a measured error rate of `p` = 0.04, shifted right by a factor of three (thresholds 785.71 ·
> 2,357.14 · 9,625.00 · 55,000.00). A side panel compares four regimes on the same 941 monthly
> outputs: no verification **72,571.20**, tiered **36,950.10**, uniform tier 2 **63,175.36**,
> uniform tier 4 **260,952.14**. Source: PCI original. Alt text: a rising staircase of required
> review depth against the logarithm of consequence, with five output classes marked on their steps
> and a second, right-shifted staircase showing that a lower measured error rate moves every
> threshold outward by a factor of three.

### 14.3.4 The plausible wrong number

Everything in 14.3.3 assumed a detection rate `q`. This topic is about what determines it, because
the answer is counter-intuitive and it changes how review effort should be aimed: **`q` depends far
more on the plausibility of the error than on its size.**

An obviously wrong number — a misplaced decimal, a units confusion, a total that fails to add — is
caught by almost anyone who looks, because human review of quantitative material is largely a
plausibility check. A plausible wrong number is caught by almost nobody, because there is nothing to
notice. And machine-produced output is characteristically plausible: fluent regardless of
correctness, internally consistent, correctly formatted, and derived by a process that produces
well-shaped answers rather than hesitant ones. Domain 9, KA 9.4.4 made the point qualitatively —
an unsure human writes hesitantly and a wrong model does not. Here it is priced.

**Worked example 14.3.4 — two wrong forecasts on Auriga, one of them dangerous.**

1. **Setup.** At week 13 Auriga reports `PV` **2,080,000**, `EV` **1,920,000**, `AC` **2,120,000**
   against a `BAC` of **4,000,000**, giving `CPI` **0.91** and `SPI` **0.92** (Domain 7). The
   CPI-based forecast is `EAC = BAC/CPI =` **USD 4,416,666.67**, a `VAC` of
   **(USD 416,666.67)**. Governance rule: a forecast overrun above **USD 350,000** obliges the
   sponsor to request supplementary funding. An AI-assisted forecasting step can fail in two ways.
   *Obvious:* a decimal slip reporting `EAC` **USD 43,200,000**, whose consequence if it escaped is
   assessed at **USD 900,000** of misdirected commitment. *Plausible:* a mis-weighted index producing
   `EAC` **USD 4,320,000** — an overrun of **USD 320,000**, below the threshold, so no funding
   request is made and the request is instead made **6 weeks** later at a cost of delay of
   **USD 45,000** per week, i.e. **USD 270,000**. Measured on a held-back set, each failure type
   occurs on **10 %** of forecast cycles; review detects the obvious error with probability
   **0.98** and the plausible one with probability **0.25**.
2. **Formula.** Expected escaped cost `= p (1 − q) u` per failure type. The asymmetry ratio is
   `[(1 − q_p) u_p] ÷ [(1 − q_o) u_o]`.
3. **Substitution.** Obvious `0.10 × 0.02 × 900,000`. Plausible `0.10 × 0.75 × 270,000`.
4. **Result.** The obvious error carries an expected escaped cost of **USD 1,800**. The plausible
   error carries **USD 20,250** — **11.25 times** as much, from a consequence **3.33 times
   smaller**. At *equal* consequence the ratio would be the ratio of escape probabilities,
   `0.75/0.02 =` **37.50**.
5. **Interpretation.** The generalisable statement is worth memorising in this form: **the expected
   damage of an error is its consequence multiplied by its probability of *not* being caught, and
   plausibility drives the second term across two orders of magnitude while magnitude drives only
   the first.** A 10× decimal slip is 3.33 times more consequential and 37.5 times more likely to be
   caught, so it is 11.25 times *less* dangerous. Everything a reviewer's instinct rewards —
   noticing the number that looks wrong — is aimed at the cheap failure mode.

   Three practical consequences follow, and they are what this topic is for. **Review must be aimed
   at plausibility, not at magnitude.** A reasonableness check is a tier-1 control against the
   obvious class and close to worthless against the plausible one; the control that works is
   **reperformance** — recomputing the number from its inputs by an independent route. On Auriga, a
   reperformance step that raises `q_p` from **0.25** to **0.85** takes about **40 minutes** at the
   blended engineering rate, **USD 87.08** per forecast cycle, and cuts the expected escaped cost
   from USD 20,250 to **USD 4,050** — a saving of **USD 16,200**, a return of **186 times** its
   cost. Its breakeven is a detection improvement of **0.3225 percentage points**, so on this class
   of output reperformance pays if it works at all. **Presentation determines reperformability.** A
   forecast published as a single number cannot be reperformed; the same forecast published with
   `EV`, `AC`, `BAC` and the stated method can be reperformed in two minutes, so *how a number is
   presented is a control*, and Domain 11, KA 11.2's reporting standard should be read as carrying
   this requirement. **And the small, plausible error near a threshold is the most dangerous object
   in a reporting pack**, because its consequence is not proportional to its size: Auriga's plausible
   error is USD 96,666.67 of forecast — **2.42 %** of `BAC` — and it costs USD 270,000 because it lands
   on the wrong side of a governance trigger. Any measure with a threshold attached should be
   reperformed within a band around that threshold regardless of the tier its consequence would
   otherwise attract.

   A related trap worth flagging, since it manufactures plausible errors from nothing: **rounding
   before dividing.** Computing `EAC` from the *reported* `CPI` of 0.91 rather than the underlying
   0.905660 gives **USD 4,395,604.40** instead of USD 4,416,666.67, understating the overrun by
   **USD 21,062.27** — **5.05 %** of the variance — from an operation that looks like tidiness. Full
   precision internally, rounding only at display, is not pedantry; it is the difference between a
   reperformable number and a plausible one.

### AI in this KA

**Where it earns its place.** Building the tier table itself across many output classes, which is
one division repeated and exactly checkable. Maintaining the measured error-rate and detection-rate
tables from a seeded-error programme, and flagging classes whose measurements have gone stale
(14.A.2). Producing a reperformance of a computed figure by an **independent route** — recomputing
`EAC` from `EV`, `AC` and `BAC` where the original came from a different path — which is a legitimate
and valuable second pair of eyes precisely because the arithmetic is deterministic. Checking that
every material assertion in an AI-assisted document carries a provenance citation to the supplied
grounding material, and listing those that do not, which is the mechanical part of verification and
the part humans skip. Auditing prompt and model versions against the record.

**Where it must not go.** It must not set the verification tier for its own output — the decision
whose independence the entire argument of 14.3.3 rests on, and Domain 9, KA 9.4's prohibition
verbatim. It must not supply `p`, `q` or `u`; those are measurements and assessments with owners, and
a model asked for them returns numbers that look exactly like data. It must not be the reviewer of
record at any tier: a tier is a human act with a name attached, and a model-performed check is a
*tool used within* a tier, whose contribution is included in the measured `q` of that tier and never
credited separately. And a model's own expression of confidence must not be read as a detection rate
or an error rate — it is neither, and treating it as either silently replaces a measurement with a
sentiment.

**Verification, concretely.** Error rates and detection rates come from a **seeded-error programme**
with a stated method, sample size and date, re-run on any change of model, prompt, grounding
material or output class, and owned by a named person. Every tier-3 and tier-4 review records the
reviewer, the date, the prompt and model versions, and what was reperformed. Reperformance is by an
**independent route**, not a re-reading. And the tier table is recomputed by hand whenever `p`
changes materially, since a standard nobody can reproduce will not survive its first challenge.

### Key terms — KA 14.3

| Term | Meaning |
|---|---|
| **Grounding** | Supplying and identifying the material an output must be derived from, with nothing outside it permitted. |
| **Refusal permission** | An explicit instruction that "the source does not say" is an acceptable and expected answer. |
| **Provenance citation** | Each material assertion in an output referenced to the supplied source location it came from. |
| **Detection rate (`q`)** | The share of material errors present that a given review depth finds; a measured quantity, never asserted. |
| **Material error rate (`p`)** | The share of items of a class containing a material error before verification; measured per class and per configuration. |
| **Verification tier** | A defined review depth with a cost per item `v` and a measured detection rate `q`, performed by a named human. |
| **Verification tier threshold** | `u*ₖ = Δvₖ/(p·Δqₖ)` — the escaped-error consequence at which the next tier begins to pay. |
| **Reperformance** | Recomputing a result from its inputs by an independent route; the only effective control against plausible error. |
| **Escape-cost asymmetry** | `(1 − q_p)u_p ÷ (1 − q_o)u_o` — why a plausible wrong number out-damages an obviously wrong one. |
| **Attributable act** | An output that confers authority or discharges an obligation, and therefore cannot be produced by a tool at any verification depth. |

### Sample MCQs — KA 14.3

**MCQ 14.3-A `[14.3.3 · Application]`** Tier 2 costs USD 44.00 per item with a detection rate of
0.70; tier 3 costs USD 121.00 with 0.90. The measured material-error rate is 0.12. The consequence
at which tier 3 begins to pay is:
- A. USD 712.96
- B. USD 1,120.37
- C. USD 3,208.33 ✅
- D. USD 5,041.67

*Rationale:* `u* = Δv/(p·Δq) = 77/(0.12 × 0.20) = 3,208.33` (14.3.3). A divides the increment by the
*total* detection rate; B divides the total cost by the total detection rate; D divides the total
cost by the increment. Only the increment-over-increment form answers "is the next tier worth it".

**MCQ 14.3-B `[14.3.3 · Evaluation]`** A tiered standard costs USD 12,650 in verification with
USD 24,300 of expected escaped loss. Uniform tier 2 on the same outputs costs USD 41,404 with
USD 21,771 of escaped loss. The best characterisation is that uniform tier 2:
- A. is safer, since escaped loss is lower
- B. spends 3.27 times as much on verification to reduce escaped loss by 10.41 %, and is USD 26,225
  worse in total ✅
- C. is equivalent, since both are defensible policies
- D. is cheaper, because one policy is simpler to administer

*Rationale:* Compare totals: 36,950 against 63,175 (14.3.3). A looks at one term of two; the extra
review lands mostly on items whose errors are cheap, which is the signature of an undifferentiated
policy.

**MCQ 14.3-C `[14.3.3 · Analysis]`** The measured material-error rate for a class falls from 0.12 to
0.04. Every verification threshold:
- A. falls by a factor of three
- B. rises by a factor of three ✅
- C. is unchanged, since the tiers' costs and detection rates are unchanged
- D. rises by a factor of nine

*Rationale:* `u* = Δv/(p·Δq)` is inversely proportional to `p` (14.3.3), so thresholds triple and
more classes fall into lighter tiers — which is where the productivity gain of a better
configuration is actually realised. C confuses the tier definitions with the threshold.

**MCQ 14.3-D `[14.3.4 · Analysis]`** An obvious error occurs on 10 % of cycles, costs USD 900,000 if
it escapes and is detected with probability 0.98. A plausible error occurs equally often, costs
USD 270,000 and is detected with probability 0.25. Which is more dangerous, and by how much?
- A. the obvious error, by 3.33 times, because its consequence is larger
- B. the plausible error, by 11.25 times ✅
- C. the plausible error, by 37.50 times
- D. they are equally dangerous once probability is taken into account

*Rationale:* Expected escaped costs are `0.10 × 0.02 × 900,000 = 1,800` and
`0.10 × 0.75 × 270,000 = 20,250` (14.3.4). C is the escape-probability ratio alone, which would be
the answer only at equal consequence; A ranks by consequence and ignores detectability.

**MCQ 14.3-E `[14.3.4 · Application]`** The control that most raises the detection rate for
plausible numerical error in an AI-assisted forecast is:
- A. a reasonableness check against expectation
- B. reperformance from the inputs by an independent route ✅
- C. asking the model to check its own output
- D. increasing the number of reviewers reading the same document

*Rationale:* A plausible error offers nothing to notice, so a plausibility check is near-worthless
against it (14.3.4). C is not an independent layer (Domain 9, KA 9.2.2); D multiplies the same
ineffective check.

### Self-check — KA 14.3

1. *State the verification tier threshold and what it is proportional to.* —
   `u*ₖ = Δvₖ/(p·Δqₖ)`: the increment of cost over the increment of detection and the error rate. It
   is inversely proportional to `p`, so a better configuration raises every threshold.
2. *Why is a plausible wrong number more dangerous than an obviously wrong one?* — Expected damage
   is consequence × probability of not being caught, and plausibility moves the second term by two
   orders of magnitude. On Auriga the plausible error costs 11.25 times the obvious one despite a
   consequence 3.33 times smaller.
3. *What single test generates every prohibition on AI use in this volume?* — Whether a named human
   could be held to answer for the output as their own judgement on evidence they can produce. If
   not — an attributable act, an appetite judgement, an unprovenanced parameter, or inference about a
   person — no depth of verification makes it permissible.

---

## Knowledge Area 14.4 — Explainability, bias, human accountability, cybersecurity and privacy

*Topics: 14.4.1 explainability as a decision requirement · 14.4.2 bias and differential error ·
14.4.3 human accountability and the AI use register · 14.4.4 cybersecurity and privacy in a digital
delivery estate.*

### 14.4.1 Explainability as a decision requirement

**Definition.** **Explainability**, for the purposes of delivery decisions, is the property that a
named human can state *why* an output says what it says, in terms the affected party can engage
with, sufficiently to defend the decision and to permit it to be contested.

The definition deliberately locates explainability in the **decision**, not in the model. Technical
interpretability — which features drove an output, how sensitive it is to inputs — is useful and is
not the requirement. The requirement is set by what the decision must survive: an appeal, a debrief,
an audit, a regulator's question, a court. Domain 10, KA 10.2 already applies the strict form of
this to tender evaluation, where an AI-generated quality score is prohibited precisely because
nobody can explain it in the terms the criterion was written in. That is the general rule: **the
explanation must be in the vocabulary of the obligation, not of the model.**

**The three levels a leader should distinguish**, because they have different costs and different
triggers. *No explanation required* — the output informs an internal, reversible, low-consequence
choice; most permitted AI use sits here, and demanding explainability of all of it is how
organisations make responsible AI unaffordable. *Explanation required* — a person affected by the
decision, or a body reviewing it, is entitled to know its basis: a supplier not shortlisted, a
change not approved, a clinic prioritised for support ahead of another. Here the requirement is
satisfied by the *human's* reasoning, evidenced, with the AI output as one input — which is the
Domain 1 accountability position expressed as a documentation duty. *Contestability required* — the
affected party must be able to challenge the decision and have the challenge examined by a human
with authority to change it. This is the level at which model-derived scoring becomes untenable,
because a challenge that cannot be answered in the criterion's own terms cannot be examined.

Explainability obligations in law and regulation are **jurisdiction-specific and moving**, and
nothing in this book should be read as stating them. Regimes may be named accurately as reference
points — the European Union's AI Act as a risk-tiered regulatory approach, data-protection regimes
such as the General Data Protection Regulation as sources of rights concerning automated decisions,
ISO/IEC 42001 as a management-system standard for AI, ISO/IEC 23894 as guidance on AI risk
management, and the NIST AI Risk Management Framework as a voluntary framework — each applying on
its own terms, in its own territory, to its own defined scope. The professional discipline is to
establish the applicable obligations for the specific decision in the specific place with qualified
advice, and to design to the **strictest** applicable level where a programme spans jurisdictions,
because the alternative is designing twice.

### 14.4.2 Bias and differential error

**Definition.** In this context **bias** means a **systematic difference in error rates between
identifiable groups**, such that the burden of the model's mistakes falls unevenly. It is a
measurable property of a deployed system, and it is measurable only if someone measures error rates
*by group* rather than in aggregate.

That last clause is the whole practical difficulty. An aggregate accuracy figure cannot reveal
differential error, and it is the figure that is always reported.

**Worked example 14.4.2 — Meridian's readiness prioritisation, by clinic type.**

1. **Setup.** Meridian scores its **40** clinics for risk of failing go-live readiness and sends a
   support team to those flagged. Of **24 urban** clinics, **9** turned out to be genuinely at risk
   and the model flagged **8** of them. Of **16 rural** clinics, **11** were at risk and the model
   flagged **6**. Planned support costs **USD 1,900** per clinic; a clinic that is at risk and *not*
   flagged needs emergency remediation at **USD 6,400**.
2. **Formula.** Recall by group = flagged at-risk ÷ actual at-risk. Excess cost per missed clinic =
   emergency − planned. Group burden = missed × excess, and per clinic of the group.
3. **Substitution.** Urban `8/9`; rural `6/11`; overall `14/20`. Excess `6,400 − 1,900 = 4,500`.
   Missed: urban `1`, rural `5`.
4. **Result.** Overall recall **70.00 %**. Urban recall **88.89 %**; rural recall **54.55 %** — a
   gap of **34.34 percentage points**. Total excess cost **USD 27,000**, of which rural clinics bear
   **83.33 %** while being **40.00 %** of the estate. Per clinic of each group the excess is
   **USD 187.50** urban and **USD 1,406.25** rural — a ratio of **7.50**.
5. **Interpretation.** The aggregate figure — 70 % recall — is the only number that would normally
   have been reported, and it describes neither group. It is the arithmetic equivalent of Domain 9,
   KA 9.4.2's mean-of-yields problem: a summary statistic that no member of the population
   experiences. The measurement that matters is the **disaggregated** one, and the only way to obtain
   it is to define the groups in advance and measure by group as a standing practice, because the
   comparison cannot be reconstructed from an aggregate afterwards.

   **Correcting it is also, here, economically obvious**, which is worth showing because the debate
   is usually conducted as though fairness and cost were opposed. Lowering the flagging threshold for
   rural clinics raises rural recall to **10 of 11 (90.91 %)** at the cost of two additional false
   positives — two clinics receiving planned support they did not need. Four fewer missed clinics
   avoids `4 × 4,500 =` **USD 18,000** of emergency remediation for **USD 3,800** of extra planned
   support: a net **USD 14,200** and a return of **4.74 times** the cost. The differential was not a
   price the programme was paying for efficiency; it was simply a defect nobody had measured.

   Four cautions, and the first is the most important in the book. **These units are clinics.** Where
   the units are *people*, differential error rates raise legal and ethical questions that are not
   expected-value questions at all, the applicable obligations differ by jurisdiction and by the
   characteristic concerned, and Domain 12's prohibition on individual inference applies in full —
   nothing in this arithmetic licenses building such a model about people, and this volume does not
   endorse it. Second, the **base rates differ** between the groups here — 37.50 % of urban clinics
   were at risk against 68.75 % of rural ones — and where base rates differ it is mathematically
   impossible to equalise every error measure at once: equalising recall and equalising the share of
   flags that prove correct are different targets that cannot both be met, so the choice of which
   measure to equalise is a **value judgement that must be made explicitly by an accountable person**
   and recorded, not settled by whichever metric the tool happens to display. Third, the groups must
   be **defined in advance**: choosing the grouping after seeing the results permits any conclusion,
   and the honest practice is to name the groupings that matter — geography, size, deprivation,
   language, service type — before deployment. And fourth, **the cause is usually the data, not the
   model**: rural clinics were under-represented in the historical records the model learned from,
   which is a data-coverage defect (14.1.4) presenting as a model defect, and re-tuning a threshold
   treats the symptom while leaving the coverage gap to reappear in the next model built on the same
   register.

### 14.4.3 Human accountability and the AI use register

Domain 1 established the principle and Domain 3, KA 3.A.2 required four governance instruments. This
topic makes the first of them operational, because the register is what turns a principle into
something an auditor can test.

**The AI use register**, one row per use, with the columns that make it testable rather than
descriptive:

| Column | Why it is there |
|---|---|
| **Use and output class** | What the tool produces, specifically enough to attach a consequence to. |
| **Decision informed** | The named decision the output feeds. A use that informs no decision is a cost. |
| **Accountable person** | The named human answerable for the output — Domain 1's test, applied to tools. |
| **Consequence per escaped error `u`** | Assessed, owned; the input that sets the verification tier. |
| **Measured `p` and `q`, with date and method** | The measurements that make the tier defensible, and that go stale. |
| **Verification tier applied** | From 14.3.3's standard, with the reviewer recorded per item at tiers 3–4. |
| **Model, prompt and grounding versions** | Reproducibility, and the retrospective question of 14.3.2. |
| **Data and confidentiality boundary** | What may be supplied to the tool, stated before use, not after an incident. |
| **Prohibited adjacent uses** | The uses that must not creep in — the scope creep of AI adoption, and it is real. |

**Three accountability failures the register exists to prevent.** The **unattributed output**: a
document, register or analysis in circulation with no author who can answer for it, which is Domain
1's accountability-without-a-holder defect and which Domain 3 prohibits in anything relied upon.
**Scope creep of a permitted use**: a tool approved to extract requirements candidates that begins
drafting requirements, or one approved to summarise minutes that begins drafting the decision
record — a change of class, not of degree, and the register's last column is the control. And
**accountability laundering**, the most corrosive: presenting a model's output as a neutral input in
order to avoid owning the judgement it embodies. Its diagnostic is simple and worth asking in a
governance meeting: *if this output were wrong, whose judgement was wrong?* If no one can answer,
the output is not governed, and Domain 3's duty on the receiving body applies — ask what the input
was, who verified it, and what would change the conclusion.

**Two obligations that sit above the register.** Where a person is materially affected by a decision
an AI output informed, the affected person's route of challenge must reach a human with authority to
change it (14.4.1). And the **workforce obligation**: staff must be told what tools are in use on
their work, must not be evaluated by inference from telemetry (Domain 12), and must be able to
decline to sign for an output they have not been given the time or the information to verify — the
last being the point at which a verification standard either is real or is a document, because a
standard that cannot be met in the time allowed simply relocates accountability onto whoever signs.

### 14.4.4 Cybersecurity and privacy in a digital delivery estate

A programme's digital estate now holds its commercial position, its supplier terms, its
vulnerabilities, its people's details and, increasingly, whatever has been supplied to third-party
tools. Domain 8 treats risk generally and Domain 10 supplier risk; the specific obligations here are
the ones a delivery leader owns rather than delegates to a security function.

**Four that are non-delegable.** *Data minimisation by design* — the estate holds what the project
needs, not what happened to be available; Meridian's decision to keep patient clinical data out of
the delivery CDE (14.1.3) is the model, and it reduces the security problem rather than defending
it. *Access on the data class schedule* — 14.1.2's access column implemented as permissions,
reviewed on movers and leavers, which is where real breaches begin. *A stated boundary for
third-party tools* — what may be supplied to which tool, decided before use; the commonest breach in
AI adoption is not an attack but a disclosure by a well-meaning person into a service whose terms
nobody read. *Continuity of the environment itself* — the delivery estate is now a single point of
failure for the delivery, and its loss is a delivery risk with a cost of delay attached, not merely
an IT incident.

Reference points may be named: ISO/IEC 27001 for information security management, ISO/IEC 27701 for
privacy information management, IEC 62443 for industrial automation and control system security
(directly relevant to Auriga's operational technology), and data-protection regimes that apply on
their own terms in their own territories. None of them decides the spend. The arithmetic below does
part of it, and its limits matter as much as its results.

**Worked example 14.4.4 — sizing two controls on Meridian's CDE.**

1. **Setup.** Meridian assesses the annual probability of a credential-compromise incident affecting
   its CDE at **0.08**, with an assessed impact of **USD 480,000** (incident response, remediation,
   delivery disruption, notification effort). Two controls are proposed. **A — identity hardening**
   (single sign-on, multi-factor authentication, privileged access review): **USD 26,000** a year,
   assessed to reduce the probability to **0.02**. **B — segmentation and field-level
   minimisation**: **USD 14,000** a year, assessed to reduce the *impact* to **USD 260,000** with the
   probability unchanged.
2. **Formula.** Expected annual loss `EAL = P × impact`. Total position `=` control cost `+` residual
   `EAL`. For two controls together, the reductions apply to their respective terms and therefore
   **multiply** rather than add.
3. **Substitution.** None `0.08 × 480,000`. A `26,000 + 0.02 × 480,000`. B
   `14,000 + 0.08 × 260,000`. Both `40,000 + 0.02 × 260,000`.
4. **Result.**

   | Option | Control cost | Residual `EAL` | **Total** | Loss avoided |
   |---|---:|---:|---:|---:|
   | Do nothing | 0 | 38,400 | **38,400** | — |
   | A identity hardening | 26,000 | 9,600 | **35,600** | 28,800 |
   | B segmentation and minimisation | 14,000 | 20,800 | **34,800** | 17,600 |
   | A and B together | 40,000 | 5,200 | **45,200** | 33,200 |

   **B alone is the lowest total position**, at USD 34,800. Buying both is **worse than buying
   either**, and worse than doing nothing.
5. **Interpretation.** Three results, each of which is routinely got wrong. First, **control benefits
   are sub-additive.** Summing the two avoided losses gives USD 46,400; the true combined avoided
   loss is **USD 33,200**, because the second control acts on a residual the first has already
   reduced. A business case that adds them overstates the combined benefit by USD 13,200 and, here,
   converts a value-destroying package into an apparently attractive one. Second, **the
   impact-reducing control is better value than the probability-reducing one** at these parameters —
   USD 1.26 of avoided loss per dollar spent against USD 1.11 — and this is the general tendency
   worth carrying: probability reduction is bought against an uncertain frequency, while impact
   reduction (segmentation, minimisation, tested recovery) works on every incident that does occur,
   including the ones the probability estimate did not anticipate. A programme that buys only
   probability reduction has bought the more speculative half.

   Third, and this is the professional caution rather than the result: **the answer is fragile in
   exactly the place the arithmetic is weakest.** Setting the two totals equal gives a crossover at
   an incident probability of **8.57 %** — above it, identity hardening wins; below it, segmentation
   wins. The assessed probability is 8.00 %, which sits **0.57 percentage points** below a crossover
   that no organisation can estimate to that precision. The honest conclusion is therefore *not*
   "buy B"; it is that **the two controls are economically indistinguishable at any probability
   anyone can defend**, and the choice should be made on grounds the arithmetic cannot see:
   implementation risk, effect on the ability to detect an incident at all, obligations under
   applicable regimes, and the fact that minimisation reduces the *scope* of a future incident rather
   than its price. Doing nothing, by contrast, is beaten by segmentation above an incident
   probability of **6.36 %** and by identity hardening above **7.22 %**, so at the assessed 8.00 %
   both dominate it and inaction is not a serious option — which is the robust part of this
   analysis, and the part worth taking to the board. And the frame itself has a boundary: expected annual loss
   is a legitimate test where consequences are cost-compensable, and it is the **wrong test** where
   the consequence is a statutory penalty, a licence condition, a duty of confidence or harm to a
   person — there the control is a requirement whose cost is sized, not an investment whose return
   is computed, and presenting such a control as failing a cost test is a category error a leader
   should be able to name in the room.

### AI in this KA

**Where it earns its place.** Measuring error rates by group across a large deployment, once the
groups are defined by a human, and flagging where a differential exceeds a stated tolerance —
mechanical, high-volume, exactly checkable, and the measurement nobody has time to do. Classifying
data holdings against the data class schedule to find minimisation opportunities and misplaced
records. Detecting anomalous access patterns as a **prompt to investigate**, never as a finding
about a person. Drafting the AI use register's first pass from a tool inventory, for human
completion of the accountability and consequence columns. Testing a set of decisions for whether the
recorded reasoning would satisfy 14.4.1's explanation level.

**Where it must not go.** It must not generate the explanation for a decision after the fact — a
model asked why a decision was made will produce a fluent, plausible rationalisation, and a
retrospective rationalisation presented as the decision's basis is worse than no explanation because
it is evidence of a reasoning process that did not occur. It must not choose which fairness measure
to equalise, which is a value judgement with an accountable owner. It must not make an access,
security or privacy decision, and it must not be given the data whose exposure is the risk being
managed — the boundary must be stated before use. It must not perform inference about individuals in
any of the forms Domain 12 prohibits, and monitoring staff activity as a productivity proxy remains
prohibited here as there. And it must not be the route through which an affected person's challenge
is answered: contestability requires a human with authority to change the decision.

**Verification, concretely.** Group definitions and the fairness measure chosen are recorded with the
accountable person's name and date, before deployment. Differential error measurements state the
group sizes, because a 16-clinic group supports a much weaker inference than a 24-clinic one and the
gap must be read with that in mind. Explanation levels are assigned per decision class and tested by
asking a reviewer to reconstruct the reasoning from the record alone. Security and privacy
assessments are performed by qualified people against the applicable regime, with the arithmetic here
used to size effort rather than to authorise an exception. And the register is audited on a stated
cadence against the actual tool estate, since the failure it exists to catch — a permitted use
quietly becoming a prohibited one — is invisible from inside the team using it.

### Key terms — KA 14.4

| Term | Meaning |
|---|---|
| **Explainability (decision sense)** | The property that a named human can state why an output says what it says, in the vocabulary of the obligation. |
| **Contestability** | The affected party's ability to challenge a decision and have it examined by a human with authority to change it. |
| **Differential error** | A systematic difference in error rates between identifiable groups; measurable only by disaggregated measurement. |
| **Recall (by group)** | Flagged at-risk cases ÷ actual at-risk cases within a group. |
| **Base-rate difference** | Unequal prevalence between groups, which makes simultaneous equalisation of all error measures impossible. |
| **AI use register** | The row-per-use record of use, decision informed, accountable person, consequence, measurements, tier, versions and boundary. |
| **Accountability laundering** | Presenting a model's output as a neutral input to avoid owning the judgement it embodies. |
| **Expected annual loss (`EAL`)** | Incident probability × assessed impact; the residual after a control is the basis of comparison. |
| **Sub-additivity of controls** | Combined avoided loss is less than the sum of individual avoided losses, because reductions multiply. |
| **Data minimisation** | Holding only what the project needs, which reduces the security and privacy problem rather than defending it. |

### Sample MCQs — KA 14.4

**MCQ 14.4-A `[14.4.2 · Application]`** Of 24 urban clinics, 9 were at risk and 8 were flagged; of
16 rural clinics, 11 were at risk and 6 were flagged. Rural recall is:
- A. 37.50 %
- B. 54.55 % ✅
- C. 68.75 %
- D. 70.00 %

*Rationale:* `6/11 = 54.55 %` (14.4.2). A divides flagged by all rural clinics; C is the rural base
rate `11/16`; D is the aggregate recall `14/20`, which describes neither group.

**MCQ 14.4-B `[14.4.2 · Evaluation]`** Lowering the rural flagging threshold would raise rural recall
from 6 of 11 to 10 of 11, adding two false positives at USD 1,900 each and avoiding four emergency
remediations whose excess cost is USD 4,500 each. The correct conclusion is:
- A. it should not be done, because false positives rise
- B. it is worth USD 14,200 net, a return of 4.74 times the extra cost ✅
- C. it is worth USD 18,000, the emergency cost avoided
- D. it cannot be evaluated economically

*Rationale:* `4 × 4,500 − 2 × 1,900 = 14,200`, and `18,000/3,800 = 4.74` (14.4.2). C omits the extra
support cost; the differential here was a defect, not a price paid for efficiency.

**MCQ 14.4-C `[14.4.4 · Analysis]`** Control A cuts incident probability from 0.08 to 0.02 at
USD 26,000 a year; control B cuts impact from USD 480,000 to USD 260,000 at USD 14,000 a year. Which
option gives the lowest total position?
- A. both controls, total USD 45,200
- B. control A alone, total USD 35,600
- C. control B alone, total USD 34,800 ✅
- D. neither, total USD 38,400

*Rationale:* Totals are control cost plus residual `EAL` (14.4.4). Both controls cost more than they
avoid together because their reductions multiply — the sub-additivity result — so A is the trap the
example exists to expose.

**MCQ 14.4-D `[14.4.4 · Evaluation]`** For those two controls, the individual avoided losses are
USD 28,800 and USD 17,600. The combined avoided loss is:
- A. USD 46,400
- B. USD 33,200 ✅
- C. USD 28,800
- D. USD 38,400

*Rationale:* Combined residual `EAL` is `0.02 × 260,000 = 5,200`, so avoided is
`38,400 − 5,200 = 33,200` (14.4.4). A adds the two individual figures and overstates by USD 13,200,
which is the error that converts a value-destroying package into an attractive one.

**MCQ 14.4-E `[14.4.1 · Analysis]`** A supplier not shortlisted asks why. The applicable
explainability requirement is best met by:
- A. disclosing the model's feature importances
- B. the evaluator's recorded reasoning against the published criteria, with any AI output identified
  as one input ✅
- C. a statement that the process was automated and consistent
- D. re-running the model with the supplier present

*Rationale:* The explanation must be in the vocabulary of the obligation — the published criteria —
and the accountable human's reasoning is what is owed (14.4.1, with Domain 10, KA 10.2). A explains
a model, not a decision; C is the answer that converts a substantive objection into a legitimacy
grievance.

### Self-check — KA 14.4

1. *Why can an aggregate accuracy figure not establish that a model is unbiased?* — Because
   differential error is invisible in aggregate: Meridian's 70.00 % recall concealed 88.89 % urban
   and 54.55 % rural, a 34.34-point gap, with rural clinics bearing 83.33 % of the excess cost while
   being 40.00 % of the estate.
2. *Why is buying two security controls sometimes worse than buying one?* — Because their reductions
   multiply rather than add: Meridian's combined avoided loss is USD 33,200 against a naive sum of
   USD 46,400, and the two together total USD 45,200 against USD 34,800 for the cheaper control
   alone.
3. *What does the AI use register have to contain to satisfy Domain 1's accountability test?* — A
   named accountable human per use, the decision informed, the assessed consequence, the measured
   error and detection rates with their dates, the verification tier applied, the model, prompt and
   grounding versions, the data boundary, and the adjacent uses that are prohibited.

---

## Advanced topics — Domain 14

### 14.A.1 The consolidated position across Domains 1 to 13

The volume's per-domain AI treatments are not thirteen policies; they are one policy applied
thirteen times, and consolidating them makes both the consistency and the two genuine tensions
visible. The table below is the map, by KA, and it is the artefact to put in front of a governance
body that asks "what is our position on AI?"

| Domain | Permitted class, and where | Prohibited, and why |
|---|---|---|
| 1 The profession | — (establishes the principle) | Any delegation of the obligation to answer (KA 1.2, 1.4) |
| 2 Strategy and selection | Enumeration of options; scoring arithmetic | Benefit attribution; kill criteria (KA 2.3, 2.4) |
| 3 Governance | Comparison of authority documents; latency modelling (KA 3.1, 3.2) | Decisions; thresholds; authorship of the decision record (KA 3.1, 3.3) |
| 4 Integration | Interface enumeration; register reconciliation (KA 4.2, 4.3) | Tailoring approval; baseline entry; schedule impact estimates (KA 4.1, 4.4) |
| 5 Scope and requirements | Extraction of requirement candidates; traceability gap detection (KA 5.1, 5.3) | Authoring requirements; boundary decisions; acceptance (KA 5.2, 5.4) |
| 6 Planning | Scenario enumeration; deterministic network arithmetic | Durations and logic without a planner's ruling |
| 7 Cost and commercial | Blended-rate and forecast arithmetic | Estimates; contract terms |
| 8 Risk | Risk candidate enumeration; sensitivity modelling | Probabilities and impacts from plausibility (KA 8.2) |
| 9 Quality | Data profiling; test-case drafting; yield arithmetic (KA 9.4) | Test results; inspection records; concessions; root-cause conclusions; its own sample size (KA 9.2, 9.3, 9.4) |
| 10 Procurement | Bid comparison mechanics; clause comparison (KA 10.1, 10.2) | Quality scores; liability caps; entitlement opinions; post-opening reweighting (KA 10.2, 10.3, 10.4) |
| 11 Stakeholders | Register structuring; document comparison (KA 11.1) | Attitude and influence inference; smoothing a report; synthetic voice or likeness (KA 11.2, 11.4) |
| 12 Leadership and teams | Aggregate structural analysis only (KA 12.2) | Any individual inference, assessment or people decision (KA 12.1–12.4) |
| 13 Adaptive delivery | Backlog and flow analytics; metric computation (KA 13.2, 13.4) | Prioritisation as a decision; velocity as a performance judgement (KA 13.4) |

Two tensions are real and should be named rather than smoothed. The first is between **the value of
enumeration and the risk of anchoring**: a model's list of candidate risks, requirements or options
genuinely improves coverage, and it also anchors the human list that follows, so the professional
practice is to have the human enumerate first and the model second, which costs nothing and is
almost never done in that order. The second is between **verification cost and the productivity
claim**: 14.3.3's arithmetic shows the gain is real but lands mostly as reduced checking on
low-consequence classes, so an organisation whose AI-assisted work is concentrated in
high-consequence classes should expect a modest gain and should say so, rather than promising the
gain observed in low-consequence work.

### 14.A.2 Drift, revalidation and the lifecycle of a model in a delivery organisation

Every quantity in this domain is measured, and measurements go stale. Three things drift, and each
has a different trigger for re-measurement. **The configuration** drifts when the model, its version,
the prompt, or the grounding material changes — an event, not a period, and any of those changes
invalidates `p` and `q` for every class using it. **The population** drifts when the work changes:
the same prompt applied to a new clinic type, a new contract form or a new plant area is a new
output class, and its error profile is unknown until measured. **The environment** drifts when the
downstream containment changes — remove a review step somewhere else and `u` rises, which moves every
threshold in 14.3.3 without anything about the model having changed.

A workable revalidation régime is therefore **event-triggered with a periodic floor**: re-measure on
any configuration change, on any new output class, and on any change to downstream containment; and
in the absence of events, re-measure each class on a stated cadence with the cadence set by
consequence — the tier-4 classes most often, the tier-0 classes least. The seeded-error method should
be specified once and reused, because the comparability of measurements over time is what makes drift
detectable at all; a re-measurement by a different method produces a different number and no
information.

Two lifecycle obligations close the loop. **Retirement**: a use that is no longer needed is removed
from the register and from the estate, because an unmaintained tool whose measurements have expired is
worse than no tool — it produces output nobody is verifying to a standard nobody has revisited.
And **retention**: the model version, prompt, grounding material and verification record for anything
relied on must survive the project, which is Domain 16, KA 16.4's responsible archive and
model/data retention obligation, and which must be designed at mobilisation because it cannot be
reconstructed at closeout.

### 14.A.3 The reviewer's digital and AI eye

Invariants to test on any digital or AI-assisted delivery arrangement. Each is cheap, and each is
diagnostic.

Every material fact has **exactly one source of record**, and every other holding is marked as a
copy. Every data class has a **named owner, a written definition, and a quality standard expressed
with its consequence** — never a single organisational rate target across classes of unequal
consequence (14.1.4). Remediation is ranked by **exposure per record** `dᵢuᵢ`, not by defect rate.
Every dashboard measure carries its **information age** `R/2 + G` on its specification, and the age is
compared against the window of the decision it serves (14.2.1). Every claimed digital twin has a
**measured fidelity**, a test-case count, a date and a tolerance, and a computed breakeven (14.2.3).
Every automation business case shows **maintenance** and a **measured differential escape rate**, and
its breakeven volume is compared against the volume at the *stated organisational boundary* (14.2.4).
Every AI use appears in the **register** with a named accountable human, an assessed consequence and a
verification tier (14.4.3). `p` and `q` are **measured, dated and owned**, with a re-measurement
trigger (14.A.2). The verification standard is **published**, and the thresholds are reproducible by
hand as `Δv/(p·Δq)` (14.3.3). Outputs whose consequences sit near a **governance threshold** are
reperformed regardless of tier (14.3.4). Error rates for anything that allocates support, priority or
resource are **measured by group**, with the groups defined in advance and the fairness measure chosen
by a named person (14.4.2). Security control cases **do not add** avoided losses (14.4.4). And the
question that subsumes several of the others: for any AI-informed output in circulation, *if this were
wrong, whose judgement was wrong?* — if nobody can answer, the output is not governed.

---

## Industry variations — Domain 14

- **Public sector and government.** Data residency, procurement rules for algorithmic systems and
  transparency expectations are set externally and vary sharply by jurisdiction; the leader's design
  freedom is mostly in *where* the human decision sits, not in whether it does. Publication duties
  can make the explanation level (14.4.1) higher than the internal consequence would justify, and the
  verification standard must be set to the disclosure obligation rather than to the arithmetic.
- **Regulated life sciences and medical devices.** Computerised systems supporting regulated activity
  carry validation expectations, and a change to a model or a prompt is a change to a validated
  system — so 14.A.2's event-triggered revalidation is not a good practice but a condition of use.
  The practical consequence is that configuration stability has a value here that it does not have
  elsewhere, and frequent model updates are a cost rather than a benefit.
- **Construction and infrastructure.** The common data environment is the mature instance of 14.1.3,
  framed by ISO 19650's information-management series and usually contractual; information
  obligations sit in the contract, so a data class schedule inconsistent with the contract loses to
  the contract (the general rule Domain 3 states for governance). Digital twins are furthest advanced
  here and fidelity decay after handover is the characteristic failure.
- **Energy, utilities and process industries.** Operational technology converges with information
  technology, and the security frame is different: IEC 62443's industrial control-system security
  applies alongside enterprise standards, availability and safety usually outrank confidentiality,
  and a control that reduces impact may be unavailable because the plant cannot be segmented as
  freely as an office network. Auriga's twin and its handover data are the domain's own example.
- **Financial services.** Model risk management is an established discipline with model inventories,
  independent validation and periodic review — much of 14.4.3's register already exists under another
  name, and the leader's task is to bring delivery-side AI use into it rather than to invent a
  parallel regime. Explainability obligations attached to decisions about customers are stricter than
  anything internal delivery requires.
- **Healthcare.** Clinical safety obligations run in parallel with project governance and are not
  delegable to a delivery body (the position Domain 3 takes for Meridian's clinical sign-off).
  Minimisation is the dominant design move — Meridian's exclusion of clinical data from the delivery
  CDE — and any output touching clinical decision support crosses from delivery governance into
  clinical governance, with a different accountable authority and a different standard.

---

## Case study — Domain 14: the quality target that cost 34,620 (health, Meridian)

**Situation.** Eighteen weeks into the clinic rollout, Meridian's assurance function reported that
the programme's data quality was "below standard at 97.6 % conformance against a 98 % target" and
recommended a remediation programme across all six data classes of the delivery CDE. The programme
board was minded to approve it. The estate held **14,100** records: **332** were defective, a
weighted mean defect rate of **2.3546 %**.

**What the arithmetic showed.** The project leader computed the consequence-weighted exposure by
class before the next board. Total exposure **USD 172,820**, and it was distributed nothing like the
defect rates: interface field-mapping records, **900** records at a 6.0 % defect rate, carried
**USD 64,800**; migration reconciliation records, at the *lowest* defect rate in the estate
(**0.8 %**), carried **USD 56,000**. Together, two classes holding **20.57 %** of the records
(2,900 of 14,100) carried **69.90 %** of the exposure. Meanwhile training and competency records, **3,600** of them at 2.5 %,
carried **USD 8,100** — an exposure of **USD 2.25** per record.

Then the leader priced the recommendation itself. Bringing every class to the **2 %** target would
produce **282** defects — **15.06 % fewer** than the observed 332 — at an exposure of
**USD 207,440**, which is **USD 34,620** or **20.03 %** *worse* than the position it was correcting.
The target achieved its own metric by loosening the two classes whose defects were expensive and
tightening the four whose defects were cheap.

**What changed.** The board approved a **consequence-proportional schedule** instead. Interface
mappings were re-verified at **USD 22** per record — **USD 19,800** — cutting the rate to 1.0 % and
removing **USD 54,000** of exposure. Migration reconciliation records were re-reconciled at
**USD 12** each — **USD 24,000** — cutting the rate to 0.2 % and removing **USD 42,000**. The other
four classes were left where they were, and the proposed re-check of all 3,600 training records was
declined on the arithmetic: it would have cost **USD 28,800** to remove **USD 6,480** of exposure, a
net loss of **USD 22,320**, and would only have paid at a remediation cost below **USD 1.80** per
record.

**The outcome, and the part that mattered.** Total remediation spend **USD 43,800** against
**USD 96,000** of exposure removed — a net **USD 52,200** — leaving the estate at **275** defects and
**USD 76,820** of exposure. The number the assurance function found most uncomfortable is the one
worth dwelling on: the new weighted mean defect rate was **1.9504 %**, which *passes* the original
2 % target — the schedule reached a better rate and a **62.97 %** lower expected cost than the
uniform target would have produced, for a fraction of the effort, by ignoring the target as a design
instruction and using it only as an outcome check.

**What the domain teaches here.** A quality target expressed as a rate, with no consequence
attached, is not a weak standard — it is a standard that can be met while making the organisation
worse off, and Meridian's recommendation would have spent money to increase expected cost by
USD 34,620. Compute exposure by class, rank by exposure per record, and be willing to leave a
high-rate, low-consequence class alone. The uncomfortable corollary, which a leader must be prepared
to defend, is that the resulting schedule deliberately tolerates the *most* defects where they are
cheapest — and that defending it requires the arithmetic, because without it the position is
indistinguishable from complacency.

## Case study B — Domain 14: the automation that saved 79,200 a month and lost 86,688 (financial services)

**Situation.** A payments-modernisation programme automated the triage of reconciliation exceptions.
The manual process took an analyst **5 minutes** per exception at a blended rate of **USD 112.80** an
hour — **USD 9.40** each — across **9,600** exceptions a month. The automation cost **USD 128,000**
to build and **USD 2,600** a month to maintain, and handled an exception for **USD 1.15** of
residual human exception-handling time. On the visible unit costs the case was overwhelming: a saving
of **USD 8.25** per exception, **USD 79,200** a month, a naive breakeven of **21,188** units reached
in **2.21 months** against fixed costs of **USD 174,800** over an 18-month horizon. It was approved
in one meeting and reported as a success for two quarters.

**What had happened.** A quarterly control review measured the two routes' **error escape rates** for
the first time — against a held-back sample of exceptions with known correct dispositions. The manual
route mis-triaged **0.8 %**; the automated route mis-triaged **3.5 %**. A mis-triaged exception cost
an assessed **USD 640** in customer remediation and rework. Quality-adjusted, the manual unit cost
was `9.40 + 0.008 × 640 =` **USD 14.52** and the automated unit cost was
`1.15 + 0.035 × 640 =` **USD 23.55**. The automation was **USD 9.03 per exception more expensive than
the process it replaced** — **USD 86,688** a month — so no volume justified it and none ever would:
the quality-adjusted breakeven volume did not exist. Against a claimed saving of USD 79,200 a month,
the true position was a loss of USD 86,688 a month, a swing of **USD 165,888** a month that had gone
unmeasured for eighteen months and cost **USD 1,560,384** in escaped errors on top of the
**USD 174,800** of build and maintenance.

**How it resolved.** The programme did not abandon the automation, which would have been the
intuitive response and the wrong one. The escape analysis showed the automated errors were
**systematic and concentrated**: they clustered on the **12 %** of exceptions above a value
threshold, where the exception patterns were most varied — the characteristic shape of machine error
that Domain 9, KA 9.4.4 describes. A rule-based containment check was placed on that 12 % only, at
**USD 2.10** per checked item, i.e. **USD 0.25** per exception averaged across the population, and it
cut the automated escape rate to **0.9 %**. The automated unit cost became
`1.15 + 0.252 + 0.009 × 640 =` **USD 7.16**, against the manual **USD 14.52** — a genuine saving of
**USD 7.36** per exception, **USD 70,636.80** a month, with a quality-adjusted breakeven of
**23,756** units reached in **2.47 months** and an 18-month net of **USD 1,096,662.40**.

**What the domain teaches here.** Three things, in order of how often they are got wrong. An
automation business case computed on **visible unit costs alone** is not a conservative estimate of a
good decision; it is a different calculation that can have the opposite sign, and the missing term —
the differential escape rate — is measurable in a day against a held-back sample and is almost never
measured before approval. Where the quality-adjusted unit cost of the automated route exceeds the
manual one, **no volume rescues it**, and scale makes it worse rather than better, which is the
opposite of the intuition scale usually supports. And the remedy for machine error is rarely to
review the output harder — it is to **place a cheap, targeted containment layer where the errors
actually concentrate**, which here converted a value-destroying automation into a strongly positive
one for USD 0.25 per exception, because machine errors are systematic and therefore locatable in a
way human errors are not.

---

## Executive perspective — Domain 14

What a programme director cannot delegate in this domain:

- **The consequence figures.** Every calculation here — remediation ranking, verification tiers,
  automation breakevens, control sizing — is proportional to an assessed cost per escaped error. Own
  the assessments and their owners, or accept that the whole standard is arbitrary (14.1.4, 14.3.3).
- **The verification standard, published and owned.** A tier table derived from measured `p` and `q`,
  applied consistently by people who are not recomputing it. Unpublished, it collapses into
  individual nervousness — which is expensive in both directions at once (14.3.3).
- **That `p` and `q` are measured, dated and re-triggered.** A seeded-error programme with a named
  owner. An unmeasured error rate freezes the standard and forfeits the entire productivity gain,
  which arrives as reduced checking rather than faster drafting (14.3.3, 14.A.2).
- **The AI use register, and the question it answers.** For every AI-informed output in circulation:
  *if this were wrong, whose judgement was wrong?* No answer means no governance, and it is Domain
  1's accountability-without-a-holder defect in a new costume (14.4.3).
- **Disaggregated measurement wherever an output allocates support, priority or resource.** Groups
  defined in advance, error rates measured by group, the fairness measure chosen explicitly and
  recorded. Aggregate accuracy conceals exactly the failure that will be raised publicly (14.4.2).
- **The refusal to accept a productivity claim without its verification cost.** A saving on
  generation is a saving only if verification is cheaper than producing the output correctly first
  time; and an automation's business case that shows neither maintenance nor a measured escape rate
  has not been reviewed (14.2.4, Case study B).

---

## Calculation exercises — Domain 14

**Exercise 14.1** A common data environment holds four classes: **A** 2,500 records at a 5.0 %
defect rate, consequence USD 260 per defect; **B** 900 records at 1.0 %, USD 4,800; **C** 1,800
records at 3.0 %, USD 150; **D** 600 records at 8.0 %, USD 90. Compute total defects, weighted mean
defect rate and total exposure; rank the classes for remediation; and compute the exposure that a
uniform 2 % target would produce.
*Solution.* Defects `125 · 9 · 54 · 48 =` **236** of **5,800** records — a weighted mean rate of
**4.0690 %**. Exposures `32,500 · 43,200 · 8,100 · 4,320 =` **USD 88,120**, of which class **B**
alone is **49.02 %** despite having the lowest defect rate and the fewest records. Exposure per
record — the remediation ranking — is **B USD 48.00 · A USD 13.00 · D USD 7.20 · C USD 4.50**. A
uniform 2 % target gives `0.02 × 5,800 =` **116** defects, a 50.8 % reduction in count, at an
exposure of `0.02 × 5,294,000 =` **USD 105,880** — **USD 17,760** or **20.15 % worse** than the
observed position. *Common error:* ranking remediation by defect rate, which puts class D first —
the class carrying **USD 4,320** of total exposure, less than a tenth of class B's — and leaves the
1.0 % class, which carries half the estate's exposure, until last.

**Exercise 14.2** An automation costs USD 62,400 to build and USD 900 a month to maintain over a
24-month horizon. The manual unit cost is USD 31.20 and the automated unit cost USD 3.60. Measured
escape rates are 1.0 % manual and 4.0 % automated; an escaped error costs USD 720. Volume is 500
units a month. Compute the naive and quality-adjusted breakeven volumes, decide, then re-decide with
a containment layer costing USD 1.80 per unit that cuts the automated escape rate to 1.0 %.
*Solution.* `F = 62,400 + 900 × 24 =` **USD 84,000**. Naive `84,000/(31.20 − 3.60) = 84,000/27.60 =`
**3,043.48 → 3,044** units. Quality-adjusted: manual `31.20 + 7.20 =` **38.40**; automated
`3.60 + 28.80 =` **32.40**; `84,000/6.00 =` **14,000** units — **4.60 times** the naive figure.
Volume over the horizon is `500 × 24 =` **12,000**, which is below 14,000, so **as designed the
automation should be rejected**, despite a naive breakeven it passes four times over. With
containment: automated `3.60 + 1.80 + 7.20 =` **USD 12.60**, unit saving **USD 25.80**, breakeven
`84,000/25.80 =` **3,255.81 → 3,256** units, and a 24-month net of
`12,000 × 25.80 − 84,000 =` **USD 225,600**. *Common error:* computing the breakeven on visible unit
costs only. Here it produces a breakeven the volume clears comfortably for a proposal that in fact
destroys value, and it is the error Case study B cost a programme USD 1,560,384 to discover.

**Exercise 14.3** Verification tiers for an output class cost USD 14.00 (detection 0.40), USD 42.00
(0.75) and USD 126.00 (0.92) per item, over tier 0 (accept, detection 0). The measured material-error
rate is 0.08. Derive the consequence thresholds; assign tiers to outputs whose escaped errors cost
USD 900, USD 4,000 and USD 20,000; and state what happens to the thresholds if the error rate doubles
to 0.16.
*Solution.* `u*ₖ = Δvₖ/(p·Δqₖ)`. Tier 0→1: `14/(0.08 × 0.40) =` **USD 437.50**. Tier 1→2:
`28/(0.08 × 0.35) =` **USD 1,000.00**. Tier 2→3: `84/(0.08 × 0.17) =` **USD 6,176.47**. Assignments:
USD 900 → **tier 1**; USD 4,000 → **tier 2**; USD 20,000 → **tier 3**. If `p` doubles to 0.16 every
threshold halves — **218.75 · 500.00 · 3,088.24** — so the USD 900 output moves up to tier 2 and the
USD 4,000 output to tier 3: a worse configuration requires deeper checking of the same outputs, which
is the same result as 14.3.3's improvement case read backwards. *Common error:* comparing a tier's
**total** cost against the **total** consequence rather than the increment of cost against the
increment of detection. Doing so here would justify tier 3 for the USD 900 output (126 < 900), which
over-verifies: the step from tier 2 to tier 3 buys 0.17 of detection for USD 84 and is worth only
`0.08 × 0.17 × 900 =` **USD 12.24**.

**Exercise 14.4** Two error types occur on 15 % of cycles each. An obvious error costs USD 620,000 if
it escapes and is detected with probability 0.99; a plausible error costs USD 180,000 and is detected
with probability 0.30. Compute each expected escaped cost and the asymmetry ratio, then evaluate a
reperformance step costing USD 145 per output that raises plausible-error detection to 0.88.
*Solution.* Obvious `0.15 × 0.01 × 620,000 =` **USD 930**. Plausible
`0.15 × 0.70 × 180,000 =` **USD 18,900** — a ratio of **20.32**, which decomposes as an
escape-probability ratio of `0.70/0.01 =` **70.00** divided by a consequence ratio of
`620,000/180,000 =` **3.4444**. Reperformance saves
`0.15 × (0.88 − 0.30) × 180,000 =` **USD 15,660** for **USD 145** — a return of **108 times** — and
its breakeven detection improvement is `145/(0.15 × 180,000) =` **0.5370 percentage points**.
*Common error:* prioritising review by the size of the number. The obvious error is 3.44 times more
consequential and 20.32 times less dangerous, so review effort aimed at magnitude is aimed at the
cheap failure mode — and the effective control is reperformance, not scrutiny.

**Exercise 14.5** An incident has an assessed annual probability of 0.12 and an impact of
USD 750,000. Control **X** costs USD 34,000 a year and cuts the probability to 0.03; control **Y**
costs USD 21,000 a year and cuts the impact to USD 400,000. Evaluate all four options, compute the
combined avoided loss, and find the probability at which X and Y are equally attractive.
*Solution.* Baseline `EAL = 0.12 × 750,000 =` **USD 90,000**. **Nothing** USD 90,000. **X**
`34,000 + 0.03 × 750,000 = 34,000 + 22,500 =` **USD 56,500**. **Y**
`21,000 + 0.12 × 400,000 = 21,000 + 48,000 =` **USD 69,000**. **X and Y**
`55,000 + 0.03 × 400,000 = 55,000 + 12,000 =` **USD 67,000**. X alone is best; buying both is
**USD 10,500 worse** than X alone. Combined avoided loss is `90,000 − 12,000 =` **USD 78,000**, not
the naive `67,500 + 42,000 =` **USD 109,500** — an overstatement of USD 31,500. X and Y are equal at
`(34,000 − 21,000)/(400,000 − 187,500) =` **6.1176 %**, so at the assessed 12 % X is robustly
preferred, unlike the marginal case in 14.4.4. *Common error:* adding the two controls' avoided
losses. Reductions compound rather than accumulate, and the naive sum here overstates the combined
benefit by 40 % and makes a package that is worse than one control alone look like the best option
available.

---

## Practitioner's toolkit — Domain 14

*Adoption-ready artefacts; adapt headings to your organisation, then keep them stable.*

### Toolkit 14.T.1 — AI use register with its verification standard

Two linked tables on two pages, owned by one named person and reviewed at every gate. **The
register**, one row per use: use and output class · decision informed · accountable human · assessed
consequence per escaped error `u` · measured `p` and `q` with method, sample size and date · verification
tier applied · model, prompt and grounding versions · data and confidentiality boundary · prohibited
adjacent uses · re-measurement trigger and next review date. **The standard**, one row per tier:
tier · description · cost per item `v` · measured detection rate `q` · the derived threshold
`u* = Δv/(p·Δq)` · who may perform it. Two integrity checks run monthly, each a count: uses whose
measurements are past their re-measurement trigger, and outputs in circulation with no register row.
The register's value is that it makes the question *whose judgement was wrong?* answerable in advance
rather than after an incident.

### Toolkit 14.T.2 — Data class schedule and quality exposure sheet

One row per data class: class · records `nᵢ` · owner (a named person) · written definition with units
and permitted values · source of record · defined pass criteria · measured defect rate `dᵢ` with
sample size and date · assessed consequence per defect `uᵢ` with its basis · **exposure**
`nᵢdᵢuᵢ` · **exposure per record** `dᵢuᵢ` · target rate for *this* class · remediation cost per
record and its breakeven. Sort by exposure per record, not by defect rate. A footer carries the three
totals a board needs: records, defects, total exposure — plus the exposure a uniform target would
produce, which is the number that stops a uniform target being adopted by default. Where a class
carries a non-compensable consequence, the target cell records an absolute standard and the
arithmetic is used only to size the effort.

### Toolkit 14.T.3 — Quality-adjusted digital business case sheet

One page per automation, analytics product, dashboard or twin, and it refuses to be completed
dishonestly. *Scope:* the decision or task, and the **organisational boundary** at which volume is
counted (project, portfolio, enterprise). *Costs:* build · maintenance per period × horizon ·
residual human handling per unit `a`. *Manual comparison:* unit time and rate `m`. *Quality:* measured
escape rate manual `eₘ` and automated `eₐ`, with method, sample size and date · assessed cost per
escaped error `u`. *Results:* naive breakeven `F/(m − a)` · **quality-adjusted breakeven**
`F/[(m + eₘu) − (a + eₐu)]` · volume at the stated boundary · net over the horizon · and, for
dashboards, the information age `R/2 + G` against the decision window. *Decision:* recommendation,
named owner of the tool as an asset, re-measurement trigger, and retirement condition. A sheet
returning "no breakeven exists" is a valid and valuable output, and the sheet is designed so that
this answer is reached before approval rather than after eighteen months.

---

## Exam preparation — Domain 14

**What is assessed.** The digital delivery environment and its interface arithmetic; data governance
as ownership, definition, source of record, quality standard and access; the common data environment
and its five guarantees; **defect rate by class, consequence-weighted exposure, exposure per record
and the uniform-target defect**; dashboards tested against a decision, with **information age
`R/2 + G`**; the analytics ladder and its ordering rule; digital twins and **breakeven fidelity**;
**automation breakeven volume, naive and quality-adjusted**; the five permitted and four prohibited
classes of AI use and the single test that generates the prohibitions; prompting as a method with
grounding, constraint, refusal permission and provenance; **the derivation of the verification
standard `u*ₖ = Δvₖ/(p·Δqₖ)`** and the pricing of tiered against uniform regimes; **the escape-cost
asymmetry between plausible and obvious error** and reperformance as its control; explainability as a
decision requirement at three levels; **differential error by group** and the base-rate constraint;
the AI use register; and **security control economics with sub-additivity**.

**The calculations to be able to do under time pressure.** Class exposure `nᵢdᵢuᵢ`, total exposure,
weighted mean defect rate, exposure per record, and the exposure a uniform rate target would produce.
Remediation net value and its breakeven cost per record. Information age `R/2 + G` priced at a cost of
delay per day, and the two levers' asymmetry. Breakeven fidelity for a twin. Naive and
quality-adjusted automation breakeven volumes, and the recognition that no breakeven exists.
Verification thresholds `Δv/(p·Δq)`, tier assignment from a consequence, total cost of a regime as
verification plus expected escaped loss, and the effect on thresholds of a change in `p`. Expected
escaped cost `p(1 − q)u` and the asymmetry ratio. Recall by group and the economics of correcting a
differential. `EAL`, residual `EAL` after a control, combined `EAL` for two controls, and a crossover
probability.

**The traps.** Applying a uniform data-quality target across classes of unequal consequence, which
can cut defect counts while raising expected cost (Exercise 14.1, Case study A) · ranking remediation
by defect rate rather than by exposure per record (14.1.4) · computing an automation breakeven on
visible unit costs, omitting maintenance and the differential escape rate (Exercise 14.2, Case
study B) · assuming a breakeven volume exists when the quality-adjusted saving is negative (14.2.4) ·
taking information age as the production lag alone or as half the period alone (MCQ 14.2-A) ·
comparing a verification tier's total cost against the total consequence instead of increment against
increment (Exercise 14.3) · forgetting that verification thresholds scale with `1/p`, so a better
configuration raises them (MCQ 14.3-C) · prioritising review by the magnitude of a number rather than
by its plausibility (Exercise 14.4) · treating a model's self-reported confidence as an error or
detection rate (14.3.3) · reading an aggregate accuracy figure as evidence of no differential error
(14.4.2) · adding two security controls' avoided losses (Exercise 14.5) · and applying
expected-value reasoning to a consequence that is not cost-compensable (14.1.4, 14.3.3, 14.4.4).

**How the domain connects.** Domain 1 supplies the accountability principle every prohibition here
rests on, and the cost of delay at which staleness, verification and automation are all priced.
Domain 3 supplies the `E[wait]` identity this domain applies to information, the delegation logic that
becomes system permissions, and the four AI governance instruments this domain makes operational.
Domain 4 supplies the interface arithmetic for the systems estate. Domain 5 supplies the testability
requirement that data pass criteria depend on. Domain 7 supplies the blended rates and the `CPI`-based
forecast that 14.3.4 corrupts two ways. Domain 9 supplies the sampling arithmetic, the composite data
fitness, the independence rule for layers and the containment-chain result this domain builds on
rather than repeats. Domain 10 supplies the strictest explainability case in the volume. Domain 11
supplies the reporting standard the information-age calculation belongs to. Domain 12 supplies the
prohibitions on inference about people, which this domain does not soften. Domain 13 supplies the
delivery cadence that a dashboard's information age must keep up with. Domain 15 owns the portfolio
boundary at which most automation cases actually pay. And Domain 16 owns the retention of models,
prompts, data and verification records after the project that created them has gone.

---

## Domain 14 summary
Thirteen domains each drew a local line around AI use. This domain makes those lines one system, and
its instrument is arithmetic rather than principle — because a principle cannot tell you how deeply
to check, and arithmetic can.

The substrate comes first. Meridian's common data environment holds **14,100** records in six
classes with **332** defects — a weighted mean rate of **2.3546 %** — carrying
**USD 172,820** of consequence-weighted exposure, distributed nothing like the defect rates: the
lowest-rate class in the estate (0.8 %) is the second-highest by exposure per record at **USD 28.00**,
behind interface mappings at **USD 72.00**. A uniform **2 %** target produces **282** defects,
**15.06 % fewer**, at **USD 207,440** — **20.03 % worse** — which is the whole case against rate
targets in one comparison. The consequence-proportional schedule that replaced it spent **USD 43,800**
to remove **USD 96,000** of exposure and landed at a **1.9504 %** mean rate: it passed the original
target while costing **62.97 %** less than the target's own remedy.

What is built on the substrate must be tested against a decision and a volume. A fact on a monthly
report with a 9-day production lag is **24 days** old — `R/2 + G`, Domain 3's identity applied to
information — worth **USD 48,960** at Meridian's cost of delay, against **7.5 days** and
**USD 15,300** on a weekly cycle with a 4-day lag; and the production lag is twice the lever the
reporting period is, day for day. Auriga's control-system twin costs **USD 211,500** against
**USD 306,000** of benefit at perfect fidelity, so it needs **69.12 %** fidelity and has
**85.00 %** — a positive **USD 48,600** on a margin of under sixteen points measured on forty cases.
Auriga's data-check automation breaks even at **959** checks on build cost, **1,659** with
maintenance and **2,654** once a one-point difference in escape rates is priced — **1.60 times** the
naive figure — which rejects it for a 720-check project and approves it for an 8,640-check portfolio.
Case study B's payments programme shows the other end: an automation reporting **USD 79,200** a month
of saving and destroying **USD 86,688** a month, with no breakeven volume at any scale, rescued by a
containment layer costing **USD 0.25** per exception.

The core is the verification standard Domain 3 asserted and this domain derives.
`u*ₖ = Δvₖ/(p·Δqₖ)` gives Meridian's thresholds as **USD 261.90 · 785.71 · 3,208.33 · 18,333.33**,
assigning its five output classes to tiers 0 to 4 at a monthly total of **USD 36,950.10** —
verification USD 12,650 plus expected escaped loss USD 24,300.10. Uniform tier 2, the intuitively
consistent policy, spends **3.27 times** as much on verification to cut escaped loss by **10.41 %**
and costs **USD 63,175.36**; uniform tier 4, the intuitively safe policy, costs **USD 260,952.14**,
**7.06 times** the tiered standard; doing nothing costs **USD 72,571.20**. **45.93 %** of the tiered
standard's escaped loss sits in the class deliberately left unverified, and checking that class would
lose **USD 2,914** — the correct remedy for a large aggregate of cheap errors is to reduce `p` or `u`,
not to check. And when the measured error rate falls from 0.12 to 0.04, thresholds triple and the
total falls **56.60 %** while the verification bill falls **72.09 %**: **the return on a better
configuration arrives as less checking**, which is the honest form of the productivity claim and is
unavailable to anyone who does not measure `p`.

Then the asymmetry that determines where checking should point. On Auriga's week-13 numbers a
plausible wrong forecast — **USD 4,320,000** against the correct **USD 4,416,666.67**, landing on the
wrong side of a **USD 350,000** funding trigger — carries an expected escaped cost of **USD 20,250**,
while a 10× decimal slip with **3.33 times** the consequence carries **USD 1,800**: the plausible
error is **11.25 times** more dangerous, and would be **37.50 times** at equal consequence, because
expected damage is consequence times the probability of *not* being caught. Reperformance from the
inputs raises detection from 0.25 to 0.85 for **USD 87.08** and returns **186 times** its cost, with
a breakeven of **0.3225** percentage points of detection. Rounding `CPI` to 0.91 before dividing
manufactures a plausible error of **USD 21,062.27** — 5.05 % of the variance — from an operation that
looks like tidiness.

The obligations that arithmetic cannot discharge close the domain. Explainability is a property of
the decision, in the vocabulary of the obligation, at one of three levels. Differential error is
invisible in aggregate: Meridian's **70.00 %** recall concealed **88.89 %** urban and **54.55 %**
rural, a **34.34**-point gap, with rural clinics carrying **83.33 %** of the excess cost while being
**40.00 %** of the estate — and correcting it returned **4.74 times** its cost, so the differential
was a defect and not a price. Because the base rates differ (**37.50 %** against **68.75 %**), not
every error measure can be equalised at once, and choosing which is a named person's judgement.
Security controls are sub-additive: Meridian's two options total **USD 35,600** and **USD 34,800**
alone and **USD 45,200** together, so buying both is worse than buying either, the naive sum of
avoided losses overstates by **USD 13,200**, and the choice between them flips at an incident
probability of **8.57 %** that nobody can estimate to that precision — which is the honest answer,
not a preference. And where a consequence is not cost-compensable, every expected-value test in this
domain stops applying and the control becomes a requirement to be sized rather than an investment to
be justified.

The through-line: **AI proposes; the professional verifies, decides and remains accountable — and
this domain is what makes the middle verb affordable.** Verification without arithmetic is either
theatre or negligence, and usually both at once in the same organisation: excessive on the outputs
that do not matter, absent on the ones that do. Compute the consequence, derive the tier, measure the
rates, reperform anything near a threshold, and keep a register that can answer the only question
that finally matters — *if this were wrong, whose judgement was wrong?*
