# PCI Professional Laws — Red-Team Report

**Instrument status: none.** This is a Charter §5 **Stage 9** record. It is not a PCI Law, it creates
no obligation, and nothing in it may be cited as a requirement (Charter §3). Where it says a law was
changed, the law itself is the record; where it says a finding was referred, the referral is the
finding.

**Scope of the exercise.** The four published law sets —
[`PCI_FOUNDATIONAL_LAWS.md`](PCI_FOUNDATIONAL_LAWS.md) (15 laws),
[`PCL_AI_LAWS.md`](PCL_AI_LAWS.md) (33), [`PFL_AI_LAWS.md`](PFL_AI_LAWS.md) (33) and
[`PML_AI_LAWS.md`](PML_AI_LAWS.md) (32) — **113 laws and 532 process requirements**, read against the
[`PCI Professional Laws Charter`](PCI_PROFESSIONAL_LAWS_CHARTER.md) and the
[`PCI Law Drafting Manual`](PCI_LAW_DRAFTING_MANUAL.md). The red team did not draft these laws.

**Method.** Six attack fronts (circumvention · ambiguity · technical compliance without substance ·
conflict · impossible or disproportionate evidence · undefined judgement), plus a Stage 8 scenario
sweep over twenty-three laws against all eight case types. Every finding below is a worked case, not
an impression. Mechanical sweeps were run over all 113 laws for undefined judgement words inside the
eight normative elements, for timing words with no clock, and for the wording of every element 12 and
element 21 in the corpus; the sweeps are what located findings **A-1**, **U-1** and **T-1**.

**Validator.** `python3 check_laws.py` was run after every edit and exits 0.

**British English throughout. Nothing was committed.**

---

## 1. Summary

| | Count |
|---|---|
| Laws attacked with a specific constructed circumvention | **38** |
| Circumventions that succeeded against the law as drafted | **17** |
| Circumventions closed by amendment | **17** |
| Conflicts between laws found | **9** |
| Conflicts fixed as drafting errors | **7** |
| Conflicts referred to the Interpretation Panel as policy | **2** |
| Laws amended | **17** (2 foundational, 5 PCL, 5 PFL, 5 PML) |
| Process requirements added | **3** |
| Definitions amended or added | **8** |
| Element-21 compliance tests strengthened | **6** |
| Emergency routes added | **3** (`PCI-FND-LAW-03`, `PCI-FND-LAW-08`, `PCI-PML-LAW-16.01`) |
| Findings **referred onward, not fixed** | **17** |

**Overall judgement.** The foundational set is the strongest instrument in the corpus: its element 21
tests are already built around the two defences that matter — the reviewer selects the sample, and the
test starts from the population of *events* rather than the population of *records about events*. The
three certification sets are good, but they are where the corpus leaks, and they leak in one
characteristic way: **a certification law states a narrower version of a foundational obligation and,
because the certification law is the one the candidate is examined on and works to, the narrower
version is the one that operates.** Eight of the seventeen closed circumventions are instances of that
single pattern.

---

## 2. The single most dangerous finding

### **C-1 · A material AI-generated figure can be "verified" by the person who produced it.**
**Severity: critical.** `PCI-PCL-LAW-13.02`, `PCI-PCL-LAW-13.03`, `PCI-PFL-LAW-16.01`,
`PCI-PML-LAW-14.02`. **Fixed.**

`PCI-FND-LAW-03` element 1 requires a material calculation, model output or automated conclusion to be
verified **by a person independent (D-12) of its preparation** before any person relies on it. Each of
the four certification laws that actually governs AI output in practice said something weaker on the
same act:

- `PCI-PCL-LAW-13.02` element 10 — "The verifier must be **independent** of the configuration of the
  tool" — and its `PR-01` directs the recomputation to be performed "by the professional".
- `PCI-PCL-LAW-13.03` element 10 — independence of the tool's configuration only.
- `PCI-PFL-LAW-16.01` element 10 — "Verification under PR-02 **may be performed by the relying
  professional; it does not require independence**", with independence required only where the
  verification is relied on *by another party*.
- `PCI-PML-LAW-14.02` element 10 — independence of configuration only.

**The worked circumvention.** A cost engineer prompts an assistant for an estimate at completion,
recomputes it themselves from the components, records the recomputation with method, scope, result and
date, names themselves as verifier and their controls lead as approver, and issues it into the board
pack that supports a funding decision. Every action in `PCI-PCL-LAW-13.02` elements 5 and 7 is
performed; the element 21 test passes on re-performance; and the only person who has ever looked at
the figure is the person whose forecast it is. The purpose of element 2 — "an output that is coherent,
well-formatted, confidently expressed and wrong" — is untouched, because self-review blindness is
exactly what survives a self-recomputation.

Two features make this the most dangerous finding rather than merely the most common. First, it sits
in the AI laws, which are the newest, the most used and the least settled by professional habit.
Second, the reader who follows the certification law faithfully arrives at a *worse* outcome than the
reader who ignores it and applies `PCI-FND-LAW-03` alone — the certification law actively misleads.

**Fix.** Element 10 of each of the four now states that the check it requires is **additional to, and
does not displace**, the independent-person verification `PCI-FND-LAW-03` requires before reliance, and
that where the verifier is also the preparer the certification law is satisfied and the foundational
one is not. The amendment is recorded in each law's element 25.

---

## 3. Front 1 — Circumvention

Thirty-eight laws were attacked with a specific route by which a professional could satisfy every
stated action and every evidence requirement while wholly defeating the stated Purpose. Seventeen
routes succeeded. All seventeen are closed.

### Closed

| # | Law(s) | The route, worked | How it is now closed | Severity |
|---|---|---|---|---|
| C-1 | `PCI-PCL-LAW-13.02` · `13.03` · `PCI-PFL-LAW-16.01` · `PCI-PML-LAW-14.02` | Self-verification of one's own AI output — see §2 | Element 10 of each: the check is additional to `PCI-FND-LAW-03`, not a substitute | **Critical** |
| C-2 | `PCI-PML-LAW-03.03` | The law fixes *when* gate criteria are published but never *who sets them*. A project publishes criteria of its own drafting — dated, version-identified, assessable, and calibrated so it cannot fail — before assembling any evidence. Every process requirement is met and element 21(a)–(e) passes; the gate that "cannot fail" of element 2 is produced in full compliance | New `PCI-PML-LAW-03.03-PR-06`: criteria approved by the authority holding the gate decision before publication; the project must not approve its own criteria. Element 7 and a new element 21(f) test for the approval | **High** |
| C-3 | `PCI-PFL-LAW-01.02` | Element 21 step (b) begins "For each interest **on the register**…". An interest never entered on the register is never tested, so the whole law is defeated by omission — and the register is maintained by the person with the interest | New element 21 step (e) works from the transaction's party, adviser, contractor, offtaker and fee records against the `PCI-FND-LAW-08` relationship list. "A clean register is a pass only when step (e) confirms it is complete" | **High** |
| C-4 | `PCI-PFL-LAW-14.02` | *In balance* is defined as "committed and available funding" ≥ remaining cost-to-complete, but neither "committed" nor "available" was defined. A sponsor's letter of comfort, an uncommitted facility and an anticipated contingency release are counted, the test returns positive, and `PR-03` is satisfied by disclosing a result that is untrue. The lenders' principal protection is removed at the exact moment element 2 identifies as the exposure point | *Committed and available funding* defined at element 4, with the six excluded instruments named; element 21(c) now traces every amount counted as funding to the instrument that commits it | **High** |
| C-5 | `PCI-PCL-LAW-03.03` | Re-baseline one control account at a time. Each partial re-baseline is genuinely approved, states its accumulated variance, carries an effective date and retains the outgoing baseline — and each falls within the lowest delegation band. Across a period the whole project's adverse variance is erased and no authority ever sees the reset. `PCI-PCL-LAW-05.04-PR-05` anticipates exactly this for changes; `03.03` had no aggregation rule | New `PCI-PCL-LAW-03.03-PR-05`: partial and successive re-baselines aggregate for banding; new element 21(e) totals the variance removed across the period and tests it against the authority that approved it | **High** |
| C-6 | `PCI-PFL-LAW-13.01` | The engaging party is entitled to set the scope before the review begins. Scope the model audit away from the tax model and the liquidated-damages mechanics — the two areas the reviewer believes carry the risk — and issue a clean, genuinely independent report on which lenders rely. Independence is intact; the review is worthless | New `PCI-PFL-LAW-13.01-PR-06`: every excluded area the reviewer considers capable of a material effect is named on the face of the report, with the fact that the engaging party set the exclusion; element 21 gains step (f) | **High** |
| C-7 | `PCI-PML-LAW-12.02` | `PR-04`'s detriment review bites only inside "the documented lookback window", and the organisation documents the window. Set it to two weeks and the protection switches off while the law is complied with in full | Element 11: the law's own default (end of project or twelve months, whichever is shorter) is now also the **floor**; a shorter documented window does not satisfy the law | **High** |
| C-8 | PCL corpus-wide (27 lines across 28 element-21 tests) | Twenty-eight compliance tests closed by saying the defective condition "**is an exception**". In this corpus *exception* is a Charter §8 term meaning an **approved departure**, and Charter §8 states that an undocumented departure is a breach and not an exception. Read in the corpus's own vocabulary, twenty-eight tests said the failure condition was permitted | Every occurrence now reads "is a failure of this test". Recorded in the PCL audit table, question 6 | **High** |
| C-9 | All three certification sets | Use the narrower certification definition of *material* to escape `D-15`. PCL's and PFL's definitions turn only on whether a decision would change; `D-15` additionally catches an item bearing on safety, on a contractual, regulatory, tax or financial-reporting position, or on a party's reliance. A variance that endangers nobody's decision but changes a contractual position is immaterial under PCL and material under the foundational law | A reading rule at the head of each certification volume's Definitions: where a definition here and a foundational definition bear on the same act, **the one producing the wider obligation governs**, and the `D-15` limbs apply in addition | **High** |
| C-10 | All three certification sets | Element 21 in fifteen certification laws tests "a sample selected on a stated basis" or "a stated sample" without saying who selects it. The foundational set says "selected by the reviewer **and not by the professional**" every time, so the drafters knew it mattered. The preparer selects a favourable sample, records the basis, and the test passes | A reading rule at the head of each certification volume's Definitions: the sample is selected by the reviewer performing the test, and the reviewer records the basis | **Medium** |
| C-11 | `PCI-PCL-LAW-03.05` | Element 12 permitted an unchallenged forecast "in an emergency funding request", with the challenge to follow "within a stated period" — stated by nobody, with no outer limit. An emergency funding request is also the case in which `PCI-FND-LAW-03` element 12 makes **no** waiver available. A professional could comply with the certification law while breaching the foundational one, indefinitely | Element 12 now fixes the period (recorded by the decision owner at approval, and in any event before the next issue) and states the `PCI-FND-LAW-03` carve-out expressly | **High** |
| C-12 | `PCI-PML-LAW-13.01` | Element 12 permits a release without complete acceptance evidence where the release is reversible or the residual risk is accepted. `PCI-PML-LAW-16.01` covers "operational release … in every delivery model", permits no exception at all in respect of a mandatory precondition, and was not cited. An adaptive release into operational use could be taken under `13.01`'s exception with a safety case open | Element 12 of `13.01` now states that the exception does not reach a mandatory precondition, and cites `16.01` | **High** |
| C-13 | `PCI-PCL-LAW-*` (definitions) | PCL's *escalation threshold* is defined as "the event stated in element 13 of a law", with no clock. A matter that is material, or that endangers a person, but which no element 13 happens to enumerate, reaches no threshold — and no element 13 in PCL states a time | Definition now states that element 13 events are **additional to** `D-10`, and that the time is the organisation's published period or, failing that, `D-20` | **Medium** |
| C-14 | `PCI-PML-LAW-*` (definitions) | PML's *escalation threshold* required a stated destination and a stated time, and said that a threshold lacking either "is not an escalation threshold". An organisation that omits the time from its delegation schedule therefore has no PML escalation thresholds and no PML escalation duties — a perverse incentive to draft badly | Definition now states that the absence of a documented threshold does not remove the duty: the destination defaults to the next authority above the decision owner, and the time to `D-20` | **Medium** |
| C-15 | `PCI-FND-LAW-03` (emergency) | See §6, E-1. A professional acting on an unverified item to protect a person had no compliant route, so the honest act was a breach | Bounded allowance added at element 12 | **High** |
| C-16 | `PCI-FND-LAW-08` (emergency) | See §6, E-2. Where withdrawal was the only available safeguard and the matter bore on a person's safety, the law required the only person able to act to stand aside | Bounded allowance added at element 12 | **High** |
| C-17 | `PCI-PML-LAW-16.01` (emergency) | See §6, E-3. The credential holder was forbidden to "seek, recommend or record" a dispensation — including the owning authority's own lawful emergency derogation | Element 12 now states the route through the owning authority's own instrument, without conceding the duty | **High** |

### Attacks that failed — the laws held

Recorded because a red-team report that lists only its successes overstates the defect rate. In each
case a specific route was constructed and a specific provision defeated it.

| Law | Route attempted | What defeated it |
|---|---|---|
| `PCI-FND-LAW-01` | Acceptance records entered in bulk at period end by an administrator | Element 21(c) attributability + (d) the no-notice question to the named individual |
| `PCI-FND-LAW-02` | Attach a document that does not support the claim and cite it | Element 21(c): the reviewer must reach the same magnitude, direction and qualification from the evidence alone |
| `PCI-FND-LAW-03` | Record a re-run of the same tool as verification | `-PR-03` |
| `PCI-FND-LAW-04` | Approve every recommendation within seconds, unread | Element 21(d) substance test — a recorded factor not in the recommendation |
| `PCI-FND-LAW-05` | Bury three material assumptions among a hundred boilerplate ones | Element 21(b) two-way match against the sensitivity statement |
| `PCI-FND-LAW-07` | Narrative lineage only the preparer can follow | Reproduction **without assistance from the preparer** |
| `PCI-FND-LAW-11` | Present a clean escalation log | The test starts from the population of matters, not of escalations |
| `PCI-FND-LAW-12` | Conduct material business in ephemeral channels | `-PR-06` |
| `PCI-FND-LAW-13` | Widen a tolerance rather than record an override | `-PR-02` + element 21(e) reads the configuration change history against the override log |
| `PCI-FND-LAW-14` | Label an output "AI-verified" | Element 21(b) representation test |
| `PCI-FND-LAW-15` | Let the next routine report carry the correction | Element 6 + `-PR-05` |
| `PCI-PCL-LAW-05.04` | Split a change to stay below a band | `-PR-05` aggregation (the model for fix C-5) |
| `PCI-PCL-LAW-06.02` | Treat a supplier's own progress return as verification | Element 10 — "a supplier's own progress return is a claim, never a verification of itself" |
| `PCI-PCL-LAW-12.03` | Let contingency be absorbed into control-account performance | Element 21's register reconciliation with no residual; element 10 bars self-approval into one's own control account |
| `PCI-PFL-LAW-10.03` | Report an average coverage ratio | Element 1 requires the minimum over the tested horizon |
| `PCI-PFL-LAW-14.04` | One person prepares and authorises an urgent payment | Element 11 — no *de minimis*; element 12 closes the emergency route expressly |
| `PCI-PFL-LAW-15.01` | Obtain the waiver after the distribution | Element 12 — "a waiver obtained afterwards does not make the earlier payment compliant" |
| `PCI-PFL-LAW-16.03` | Record the sign-off after release | Element 21(a) + element 12: a breach "even where the output was correct" |
| `PCI-PML-LAW-01.03` | A permanently titled "independent assurance" function assures its own artefact | `-PR-06` name-matching against the authorship record |
| `PCI-PML-LAW-16.01` | Show a safety case at 95 per cent | `-PR-03` and element 21(b) — any numeric value on a gate-block item fails outright |

---

## 4. Front 2 — Ambiguity, and the definitions read against each other

The Definitions sections were written per file. Read against each other they diverge on **seven terms
that decide compliance**. No file said which governs. This is the corpus's largest structural defect
and the reading rules below are a patch, not the cure (see **P-1**).

| Term | `PCI_FOUNDATIONAL_LAWS.md` | PCL-AI | PFL-AI | PML-AI | Effect |
|---|---|---|---|---|---|
| **material** | `D-15`, six limbs: decision · published tolerance · contractual/regulatory/tax/reporting position · safety · reliance · organisation's criteria | Decision-change only, plus a recorded materiality rule and a cumulative test | Decision-change only, stated before work begins in the transaction's metric | Decision-change **plus** an express carve-out that safety, legality, a licence, a statutory duty or the truth of a statement is material irrespective of size | PCL and PFL are narrower than both `D-15` and PML. **A-1, closed by C-9; the underlying divergence referred as R-3** |
| **independent** | `D-12`, five limbs; line reporting to the preparer's manager does **not** by itself defeat it, appraisal on the outcome does | Four facts, including not reporting to the preparer for that work | Four limbs, including **not in the preparer's reporting line at all** | Five limbs, relative to a specified matter | Four different tests for one word. PFL's is the strictest; `D-12`'s express carve-out for line reporting is contradicted by it. **Referred as R-4** |
| **competent reviewer** | `D-04`, two limbs, **independence not required** | Independence built in | Independence **plus** written authorisation to record a conclusion | Did not prepare, direct, specify or approve | `PCI-FND-LAW-10` element 12's supervised-acquisition exception requires the supervisor to be a `D-04` competent reviewer; under the certification definitions a supervisor can never qualify, so the exception is unusable. **Closed by the `D-NN` reading rule; the divergence referred as R-5** |
| **verified** | `D-26` + `PCI-FND-LAW-03-PR-01`: eight named methods | The same eight | **Seven** — *named expert judgement* absent, and *clause-to-summary comparison* renamed *clause-to-output comparison* | Split into V&V verification and AI verification, with a nine-item method list at element 16 of `14.02` | PFL is stricter, which is not a loophole, but the divergence was silent. **Now stated as deliberate with its reason; confirm with an SME — R-6** |
| **escalation threshold** | `D-10`, six substantive triggers; reaching it starts the `D-20` clock | "The event stated in element 13 of a law" — no clock | A condition recorded before work begins | A documented value in the delegation schedule, with a time; without a time it "is not an escalation threshold" | Two live defects, **closed as C-13 and C-14** |
| **evidence** | `D-11`, with a closed list of four things that are **not** evidence | Similar, adds "an AI-generated summary of a record is not evidence of the underlying fact" | Similar | Similar, adds an unreproducible dashboard state | Compatible; no defect |
| **decision owner** | `D-08` | §A | Core terms | Terms | Compatible; all four exclude committees without a named chair |

**A-2 · `authorised tool` listed as a defined term where it is neither defined nor used.**
`PCI-PFL-LAW-16.03` element 4 lists *authorised tool*; the term is defined at `PCI-PFL-LAW-16.01`
element 4 and appears nowhere in `16.03`. Element 4 is normative-determinative, so an undefined term in
it is a Manual §4 defect. **Fixed** — element 4 now carries the pointer. Severity: low.

**A-3 · *exception* used in two incompatible senses.** Charter §8 sense (an approved departure) against
the audit sense (a finding). Twenty-eight instances of the audit sense sat inside PCL element 21s.
**Fixed as C-8.** One residual instance is left standing deliberately: `PCI-PCL-LAW-06.02` element 21
requires a verification record to state "the exceptions found", which is a field of a record rather
than a statement of consequence. Flagged for the Panel as **R-14**.

---

## 5. Front 3 — Technical compliance without substance

Sought specifically: a register that exists but is empty, a sign-off with no evidence behind it, a
"review" that is the preparer re-reading their own work, and a sample the preparer chooses. All four
patterns were found.

| # | Pattern | Where | Disposition |
|---|---|---|---|
| T-1 | **A sample the preparer chooses** — fifteen certification element-21 tests say "a sample selected on a stated basis" without naming the selector, against the foundational set's uniform "selected by the reviewer and not by the professional" | PCL `06.01` `06.02` `07.01` `07.02` `07.03` `11.01`; PML `01.01` `01.03` `09.01` `11.01` `12.01` `13.02` `15.01`; PFL `06.01` `16.03` | **Fixed as C-10** by a reading rule in each volume's Definitions. Individually rewriting fifteen element 21s was rejected as higher-risk than one interpretive rule |
| T-2 | **A review that is the preparer re-reading their own work** | The four AI laws | **Fixed as C-1** |
| T-3 | **A register that exists but is empty** | `PCI-PFL-LAW-01.02` — the compliance test read the register rather than the transaction | **Fixed as C-3.** The same defect does **not** exist in `PCI-FND-LAW-08`, whose element 21(e) already works from the participant, supplier, counterparty and vendor-selection records |
| T-4 | **A document produced instead of the thing being done** | `PCI-PML-LAW-03.03` — criteria published on time, written by the party they judge | **Fixed as C-2** |
| T-5 | **A disclosure that discloses nothing** | `PCI-PFL-LAW-14.02-PR-03` — the in-balance result is disclosed truthfully, computed on funding that does not exist | **Fixed as C-4** |
| T-6 | **A protection whose duration the protected party's employer sets** | `PCI-PML-LAW-12.02-PR-04` lookback window | **Fixed as C-7** |

---

## 6. Front 4 — Conflicts between laws

Nine conflicts found. Seven were plain drafting errors and are fixed. Two are genuine policy questions
and are referred with the question stated.

### Fixed as drafting errors

| # | Pair | The conflict | Fix |
|---|---|---|---|
| X-1 | `PCI-FND-LAW-03` ↔ `PCI-PCL-LAW-13.02` / `13.03` / `PCI-PFL-LAW-16.01` / `PCI-PML-LAW-14.02` | Different independence thresholds on the same act. Charter §4 says the foundational law governs and "a PCI Law never lowers an obligation"; the certification laws lowered it on their face | Element 10 cross-references — see C-1 |
| X-2 | `PCI-FND-LAW-03` element 12 ↔ `PCI-PCL-LAW-03.05` element 12 | The foundational law makes **no** waiver available where the item supports an irreversible commitment or a payment; the certification law offered an emergency funding request as its worked example of when the exception applies | C-11 |
| X-3 | `PCI-PML-LAW-16.01` ↔ `PCI-PML-LAW-13.01` | Absolute no-exception rule against a release exception covering the same act | C-12 |
| X-4 | `PCI-FND-LAW-11` / `D-10` ↔ PCL *escalation threshold* | The certification definition narrowed the foundational duty and supplied no clock | C-13 |
| X-5 | `PCI-FND-LAW-11` / `D-20` ↔ PML *escalation threshold* | The certification definition extinguished the duty where the organisation drafted badly | C-14 |
| X-6 | `PCI-FND-LAW-10` element 12 ↔ certification *competent reviewer* | The supervised-acquisition exception requires a supervisor who is a `D-04` competent reviewer; the certification definitions build in independence, which a supervisor cannot hold | Closed by the `D-NN` reading rule in each volume |
| X-7 | `D-15` ↔ PCL and PFL *material* | Different materiality on the same item, with safety and contractual position caught by one and not the other | Closed by the wider-obligation reading rule (C-9); the substantive divergence referred as R-3 |

### Referred — genuine policy conflicts, not drafting errors

**R-1 · `PCI-FND-LAW-08` ↔ `PCI-PFL-LAW-01.02`: who decides what must be disclosed?**
`PCI-FND-LAW-08` sets a **closed list** (`D-05`) of relationship kinds, states that "every conflict
within `D-05` must be disclosed; there is no threshold", and prohibits the professional "deciding for
oneself that a disclosed conflict needs no safeguard". `PCI-PFL-LAW-01.02` element 1 triggers
disclosure on what "a **reasonable party** to that financing would want to know", and its element 4
states that "**the test is applied by the credential holder**". The wider-obligation reading rule means
`D-05` still catches everything it catches, so the practical gap is closed — but the two laws state
genuinely different philosophies of conflict disclosure (closed list versus judgement standard), and
choosing between them is a policy decision.

> **Question for the Interpretation Panel.** Is `PCI-PFL-LAW-01.02`'s reasonable-party test intended to
> *widen* `D-05` only, or to *replace* it for project-finance work? If the former, element 1 should say
> so. If the latter, it conflicts with `PCI-FND-LAW-08` element 12's no-exception rule and with Charter
> §4, and one of the two must change.

**R-2 · `PCI-FND-LAW-03` element 12 ↔ the two-person consultancy.**
A waiver of independent verification is available only where no person meeting `D-12` exists **and** no
external reviewer can be engaged — and it is unavailable altogether where the item supports an
irreversible commitment, a payment to a third party, an external report or a safety decision. For a
sole practitioner or a two-person firm, essentially every material estimate supports a payment. The
practical effect is that the foundational law requires a paid external reviewer on almost every
material output of a very small practice.

> **Question for the Interpretation Panel.** Is that effect intended? If it is, `PCI-FND-LAW-03`
> element 11 should say so plainly, so that small practices price it. If it is not, the four no-waiver
> cases need a proportionality qualifier that a red team should not invent.

---

## 7. Front 5 — Impossible or disproportionate evidence

Each requirement was tested against a two-person consultancy, a sole practitioner and a large
organisation.

| # | Requirement | Problem | Disposition |
|---|---|---|---|
| R-7 | `PCI-FND-LAW-09` element 21(d) — the reviewer "searches the professional's outbound traffic, prompt history, shared-drive and mailbox records for the period" | The evidence may not be **producible without information the professional has no right to**: employee-monitoring, works-council and data-protection regimes restrict exactly this search in several jurisdictions, and a two-person firm has no logging to search. Element 18 cautions on personal data but the test is stated unconditionally | **Referred.** A red team should not narrow a confidentiality test; the Panel should decide whether (d) becomes "so far as the applicable monitoring and data-protection regime permits, and otherwise by a recorded attestation plus tool-inventory inspection" |
| R-8 | `PCI-PFL-LAW-14.04-PR-02` — three named individuals for prepare, authorise and reconcile, with **no *de minimis*** | Element 11's small case is "three people in the finance function". A genuine two-person borrower's finance function must buy an external administrator or a board member's time for every payment run | **Referred.** The obligation is right; whether the cost is proportionate at the very bottom of the range is a practitioner-consultation question |
| R-9 | `PCI-FND-LAW-12` element 21 — "the reviewer tests **one record known to have been amended**" | Where no in-period record was amended, the test cannot be completed, and a law that cannot be tested cannot be passed | **Referred** as an editorial correction for the Panel: "where any record was amended in the period" |
| R-10 | `PCI-PFL-LAW-13.01-PR-06` (new) | Requires the reviewer to form and record a view on which exclusions could be material — a judgement they may prefer not to record | **Accepted as proportionate.** It is one line in a document the reviewer already writes, and it is the only thing that makes a scoped-to-pass review visible |
| R-11 | `PCI-PML-LAW-12.02-PR-06` — the assurance-discovered versus team-reported indicator | Requires both populations to be classified consistently; on a small project the denominator may be two | **Held.** Element 11 already states that a register with no entries is not a breach, and the indicator is drawn from records the project already produces |

---

## 8. Front 6 — Undefined judgement

A mechanical sweep of the eight normative elements (1, 5, 6, 7, 12, 13, 15, 16) of all 113 laws for
*appropriate*, *adequate*, *reasonable*, *relevant*, *timely*, *sufficient* and *promptly* returned
**eight hits in total**, which is an unusually clean result for a corpus of this size.

| # | Occurrence | Finding |
|---|---|---|
| U-1 | `PCI-PFL-LAW-01.02` element 1 — "a **reasonable** party … would want to know" | Defined at element 4 of that law. **Not a defect**, but it is the judgement standard behind conflict **R-1** |
| U-2 | `PCI-PCL-LAW-06.01` and `06.04` element 15 — AI must not "decide that level of effort is **appropriate**" / "decide which method is **appropriate**" | Element 15 is a prohibition on an AI system, not an obligation on a person; the judgement word describes the thing the AI must not do. **Not a defect** |
| U-3 | `PCI-PFL-LAW-01.01` element 15 — AI must not conclude a project is "**adequately** funded" | As U-2. **Not a defect** |
| U-4 | `PCI-PCL-LAW-12.03` element 15 — AI must not "determine that remaining contingency is **sufficient**" | As U-2. **Not a defect** |
| U-5 | `PCI-FND-LAW-03` and `07` element 7 — working papers "**sufficient** for the method to be re-performed" / "**sufficient** for re-performance" | Carries its own test. **Not a defect** |
| U-6 | `PCI-PFL-LAW-09.02` element 5 — a description "**sufficient** to price, model and compare the structure without reference to a compliance label" | Carries its own test. **Not a defect** |
| U-7 | *promptly* | Used only in the foundational set, where `D-20` defines it with a one- and five-working-day default and a start point at knowledge or suspicion. **Zero occurrences in all three certification sets** |
| U-8 | **Clocks that were missing** | Three obligations turned on a period no one set: `PCI-PCL-LAW-03.05` element 12 ("within a stated period"), and the PCL and PML *escalation threshold* definitions. **All three fixed** — C-11, C-13, C-14. Six further element 12s use "within a stated period" where the approver plainly states it at the time of approval, which is adequate |

---

## 9. Stage 8 — Scenario testing

Twenty-three laws against all eight case types. **P** = passes as drafted · **F→fix** = failed and was
fixed in this pass · **R** = failed or strained and is referred.

| Law | Normal | Boundary | **Emergency** | Conflict | AI-assisted | Multi-jurisdiction | Small project | Megaproject |
|---|---|---|---|---|---|---|---|---|
| `PCI-FND-LAW-03` | P | P | **F→fix E-1** | P | P | P | **R-2** | P |
| `PCI-FND-LAW-04` | P | P | P (named deputy) | P | P | P | P (one recorded line) | P |
| `PCI-FND-LAW-08` | P | P | **F→fix E-2** | P | P | P | P | P |
| `PCI-FND-LAW-09` | P | P | P (safety act + notify) | P | P | P | P (`-PR-01` route) | **R-7** |
| `PCI-FND-LAW-10` | P | P | **R-12** | P | P | P | P | P |
| `PCI-FND-LAW-11` | P | P | P (escalate what is known) | P | P | P | P | P |
| `PCI-FND-LAW-12` | P | P | P | P | P | P | P | **R-9** |
| `PCI-FND-LAW-13` | P | P | P (safety allowance) | P | P | P | P | P |
| `PCI-FND-LAW-14` | P | P | P — no exception, and element 12 says why | P | P | P | P | P |
| `PCI-FND-LAW-15` | P | P | P | P (legal restriction) | P | P | P | P |
| `PCI-PCL-LAW-03.03` | P | **F→fix C-5** | P (provisional approval) | P | P | P | P | **F→fix C-5** |
| `PCI-PCL-LAW-03.05` | P | P | **F→fix C-11** | **F→fix X-2** | P | P | P | P |
| `PCI-PCL-LAW-05.04` | P | P | P (execution authorised, approval outstanding) | P | P | P | P (authority from outside) | P |
| `PCI-PCL-LAW-12.03` | P | P | P (life/works/environment) | P | P | P | P | P |
| `PCI-PCL-LAW-13.02` | P | P | P (labelled unverified, no irreversible use) | **F→fix X-1** | **F→fix C-1** | P | P | P |
| `PCI-PFL-LAW-01.02` | P | **F→fix C-3** | P | **R-1** | P | P | P | P |
| `PCI-PFL-LAW-13.01` | P | **F→fix C-6** | P | P | P | P | P | P |
| `PCI-PFL-LAW-14.02` | P | **F→fix C-4** | P (waiver before drawing) | P | P | P | P | P |
| `PCI-PFL-LAW-14.04` | P | P | P — no exception, and element 12 says why | P | P | P | **R-8** | P |
| `PCI-PFL-LAW-16.03` | P | P | P (alternate signatory) | P | P | P | P | P |
| `PCI-PML-LAW-03.03` | P | **F→fix C-2** | P (item-by-item incomplete evidence) | P | P | P | P | P |
| `PCI-PML-LAW-12.02` | P | **F→fix C-7** | P | P | P | P (defers to statute) | P | P |
| `PCI-PML-LAW-14.02` | P | P | P (sponsor exception; never for safety) | **F→fix X-1** | **F→fix C-1** | P | P | P |
| `PCI-PML-LAW-16.01` | P | P | **F→fix E-3** | **F→fix C-12** | P | P | P | P |

**184 cells tested · 12 failures · 11 fixed · 4 referred** (three cells carry a referral rather than a
fix, and `PCI-FND-LAW-03`'s small-project cell carries **R-2**).

### The emergency case, worked

The emergency case was the weakest, as expected. Three laws had no compliant route in a case that will
certainly arise, and a law with no route in that case is a law that gets broken — which teaches
contempt for the rest of the system. Each gained a **bounded route**, stating who may invoke it, what
must still be recorded, what is never waived, and how it is ratified afterwards.

**E-1 · `PCI-FND-LAW-03`.** *Case:* a temporary-works calculation must be acted on now to get people off
a structure; no independent verifier is reachable; element 12's waiver is unavailable precisely because
the item bears on a person's safety. The professional either acts and breaches, or complies and someone
is hurt.
*Route added:* reliance **only to the extent necessary to protect that person**; record the reliance,
the necessity and what was attempted within one working day; state the limitation to every recipient;
obtain and record the verification as soon as it can be obtained; communicate any resulting difference
under `PCI-FND-LAW-15`; the decision owner records their position within five working days; no reliance
for any other purpose until the verification exists. Expressly not available for commercial urgency, a
deadline, a reporting cycle or a funding date. Element 12 now also states *why* the four no-waiver cases
are what they are.

**E-2 · `PCI-FND-LAW-08`.** *Case:* the only person able to act on a safety matter holds a conflict, and
element 10 makes withdrawal the only remaining safeguard where no independent person exists inside or
outside the organisation. The law requires the only capable person to stand aside.
*Route added:* act **only to the extent necessary to protect that person**; record the conflict, the
necessity and each act at the time; disclose in writing within one working day; withdraw as soon as
another person can act; the person or body to whom the duty is owed records its position within five
working days and decides which `-PR-01` safeguard operates from that point. The disclosure duty is
untouched and the professional still never chooses their own safeguard.

**E-3 · `PCI-PML-LAW-16.01`.** *Case:* continuing without the transition is the greater danger — a
failed system that cannot be restored — and element 12 forbade the credential holder to "seek,
recommend or record" a dispensation. That prohibition also caught the owning authority's **own lawful
emergency derogation**, which is the correct professional route and which regulators genuinely operate.
*Route added:* the answer is still not a trade at the gate; it is that the **owning authority** exercises
its own emergency instrument — a regulator's derogation, a safety authority's temporary approval, an
interim certificate — and the item is then recorded under `-PR-02` as **met**, with that instrument, its
scope and its expiry, and reported at the next governance meeting. Seeking such an instrument from
anyone other than the owning authority remains prohibited; where the authority declines or cannot be
reached, the decision remains **hold** and the consequences of holding are escalated under
`PCI-FND-LAW-11`.

**Laws where no emergency exception exists, and the reason is now visible.** `PCI-FND-LAW-14` (element
12 already enumerates urgency, volume, cost pressure, staffing, client instruction and vendor assurance
and rejects each), `PCI-FND-LAW-11`, `PCI-FND-LAW-15`, `PCI-PFL-LAW-14.04` (element 12 states the
compliant urgent route is an alternative named authoriser, "never by collapsing the roles") and
`PCI-PFL-LAW-16.03`. These were tested and held: in each, the compliant act in an emergency is
available and takes no longer than the non-compliant one.

---

## 10. What was fixed

Seventeen laws amended across four files. Three new process requirements. Every amendment is recorded
in the amended law's element 25, and the file-level records point here.

**`PCI_FOUNDATIONAL_LAWS.md`**
- `PCI-FND-LAW-03` element 12 — emergency allowance (E-1); reason stated for the four no-waiver cases.
- `PCI-FND-LAW-08` element 12 — emergency allowance (E-2).
- Audit table question 18 and the unresolved-findings note updated.

**`PCL_AI_LAWS.md`**
- Definitions — foundational-collision reading rule; element-21 sampling rule; *escalation threshold*
  rebuilt on `D-10` and `D-20`.
- 27 lines / 28 element-21 tests — "is an exception" → "is a failure of this test".
- `PCI-PCL-LAW-03.03` — new `-PR-05`; elements 7 and 21.
- `PCI-PCL-LAW-03.05` — element 12.
- `PCI-PCL-LAW-13.02`, `PCI-PCL-LAW-13.03` — element 10.
- Audit table question 6 and the Charter §5 record updated. PR count 144 → 145.

**`PFL_AI_LAWS.md`**
- Definitions — reading rule; sampling rule; *verified* method-list collision stated as deliberate.
- `PCI-PFL-LAW-01.02` — element 21 step (e).
- `PCI-PFL-LAW-13.01` — new `-PR-06`; elements 7 and 21.
- `PCI-PFL-LAW-14.02` — element 4 (*committed and available funding*); element 21(c).
- `PCI-PFL-LAW-16.01` — element 10.
- `PCI-PFL-LAW-16.03` — element 4 pointer.
- Audit-table preamble updated. PR count 155 → 156.

**`PML_AI_LAWS.md`**
- Definitions — reading rule; sampling rule; *escalation threshold* fallback.
- `PCI-PML-LAW-03.03` — new `-PR-06`; elements 7 and 21.
- `PCI-PML-LAW-12.02` — element 11 lookback floor.
- `PCI-PML-LAW-13.01` — element 12 boundary with `16.01`.
- `PCI-PML-LAW-14.02` — element 10.
- `PCI-PML-LAW-16.01` — element 12 emergency route.
- Audit-table preamble updated. PR count 147 → 148.

**What was deliberately not done.** No law was restructured, no identifier renumbered, no obligation
weakened. Where a choice existed between loosening a duty and tightening a test, the test was tightened
— including in `PCI-PFL-LAW-16.01`, where the temptation was to add *named expert judgement* to the
method list and the correct answer was to explain why it is absent.

---

## 11. Referred onward — the agenda for human review

**These are the most valuable findings in this report.** Each needs a policy decision or a
subject-matter expert, and a red team that fixed them would be legislating.

### Structural

**P-1 · There is no PCI Law Definitions Register, and the corpus needs one.**
Manual §4 permits a term to be defined "in the law that uses it, **or in the PCI Law Definitions
Register**". No such register exists, so each volume built its own, and seven compliance-deciding terms
now diverge across four files. The reading rules added in this pass are a patch that resolves *which*
definition wins; they do not stop the definitions drifting further apart at the next edition, and they
place an interpretive burden on every reader of every law. **Recommendation: create the register, move
the seven terms into it, and leave per-volume definitions only for genuinely credential-specific
terms.** No file in the corpus has authority to do this.

**P-2 · Charter §4's priority order is unsettled by counsel, and every conflict finding depends on it.**
Charter §4 states this itself. Seven of the nine conflicts in §6 were resolved by applying "a PCI Law
never lowers an obligation". If counsel reaches a different view of the order, those resolutions are
reopened.

### Definitional policy

- **R-3 · Should PCL-AI and PFL-AI *material* carry PML-AI's explicit "irrespective of size" limb for
  safety, legality, a licence, a statutory duty and the truth of a statement made to a decision-maker?**
  PML has it; PCL and PFL do not; `D-15` reaches the same result by a different route. The reading rule
  makes the outcome correct today, but three credentials examining three different materiality tests is
  an examination problem as much as a drafting one.
- **R-4 · Should *independent* be one test or four?** `D-12` says line reporting does not by itself
  defeat independence; `PCI-PFL-LAW-01.02`'s and `PCI-PFL-LAW-13.01`'s four-limb test says it does. Both
  are defensible professional positions. If the divergence is deliberate it must be stated; if not, one
  must move.
- **R-5 · Should *competent reviewer* include independence?** `D-04` deliberately separates them; all
  three certification sets fold independence in. The separation matters, because
  `PCI-FND-LAW-10` element 12 needs a competent reviewer who is precisely **not** independent — a
  supervisor.
- **R-6 · PFL's *verified* omits *named expert judgement* and renames *clause-to-summary comparison*.**
  Now stated as deliberate with a reason a red team supplied. **A modelling SME should confirm the
  reason is right**, and the Panel should decide whether renaming a Manual §5.2 method is acceptable.

### Emergency and proportionality

- **R-12 · `PCI-FND-LAW-10` fails the emergency case and the fix is not PCI's to make.** Where an
  emergency requires an act nobody present is competent to perform, element 1 prohibits it and element
  12's only no-exception case is a reserved activity. The competing considerations — acting outside
  competence, versus performing an activity a jurisdiction reserves to a licence holder — are settled by
  local law, not by PCI. **Question:** should element 12 carry an allowance mirroring `PCI-FND-LAW-03`'s
  new one, expressly subject to reserved-activity law, or should it state that no allowance exists and
  that the professional's duty is to summon competence and escalate?
- **R-2 · `PCI-FND-LAW-03` and the two-person practice** — see §6.
- **R-8 · `PCI-PFL-LAW-14.04` segregation of duties at the bottom of the size range** — see §7.
- **R-7 · `PCI-FND-LAW-09` element 21(d) and employee-monitoring law** — see §7.
- **R-13 · `PCI-PML-LAW-16.01`'s new emergency route assumes the owning authority has a derogation
  process.** Many do; some do not. **Question:** should a further process requirement oblige the
  credential holder to establish, when the gate block is assembled, whether each owning authority
  operates an emergency instrument and what it requires? That would be useful and is a change of
  obligation, so it is not a red-team fix.

### Unvalidated numbers

**R-15 · Every period in the corpus is a drafting choice that no practitioner has tested.** The
foundational set already records this. The live list is: `D-20`'s one and five working days;
`PCI-FND-LAW-11-PR-02`'s ten working days; `PCI-FND-LAW-03`'s three-month waiver ceiling;
`PCI-FND-LAW-05`'s ten working days; `PCI-FND-LAW-06`'s thirty days; `PCI-FND-LAW-12`'s twelve months;
`PCI-PFL-LAW-06.01`'s fourteen days; and — **added by this pass, and therefore carrying the same
warning** — the twelve-month lookback floor now in `PCI-PML-LAW-12.02` element 11 and the one- and
five-working-day periods in the three new emergency routes. Stage 6 practitioner consultation is the
right place to test all of them.

### Editorial, for the Panel

- **R-9 · `PCI-FND-LAW-12` element 21** — "one record known to have been amended" is untestable where
  none was; suggest "where any record was amended in the period".
- **R-14 · `PCI-PCL-LAW-06.02` element 21** — "the exceptions found" retains the audit sense of
  *exception* that C-8 removed elsewhere. It is a field of a record rather than a statement of
  consequence, so it was left; the Panel should decide whether the corpus tolerates the word in that
  sense at all.

### Out of the red team's competence

**R-16 · `PCI-PFL-LAW-09.02` (Shariah compliance determination) and `PCI-PFL-LAW-09.03` (sustainability
claims) were read but not attacked substantively.** Both turn on subject matter this red team is not
competent in — Islamic finance and sustainable-finance taxonomies. `PCI-FND-LAW-10` is the reason for
saying so rather than producing confident findings about them. **Both need a named subject specialist
under Charter §5 Stage 4 before approval**, and `09.02` in particular, because it is the one law in the
corpus whose element 12 correctly records that PCI has no authority to grant an exception at all.

**R-17 · Charter §5 Stage 4 (technical review) and Stage 6 (practitioner consultation) remain
unperformed for all four files.** Nothing in this report substitutes for either. A red team can show
that a law is gameable; only a practitioner can show that it is unworkable, and only a specialist can
show that it is wrong.

---

## 12. Validator

```
$ cd /home/user/PCI/docs/books/laws && python3 check_laws.py
PCI Professional Laws — reference-integrity check
==============================================================================
Files: PCI_FOUNDATIONAL_LAWS.md, PCL_AI_LAWS.md, PFL_AI_LAWS.md, PML_AI_LAWS.md
Laws parsed: 113 (PCI 15, PCL 33, PFL 33, PML 32)

  ok    duplicate identifiers                              113 laws, 532 process requirements, all unique
  ok    twenty-five elements, in Manual §5 order           all 113 laws carry every element in order
  ok    foundational citations resolve                     725 citations, all within `PCI-FND-LAW-01`–`PCI-FND-LAW-15`
  ok    certification-law citations resolve                1469 citations against 98 published laws
  ok    process-requirement citations resolve              594 citations against 532 defined requirements
  ok    no `shall` in a law element or process requirement zero occurrences inside any of the 113 laws
  ok    anchor domains within the credential range         PCL ≤ 13, PFL ≤ 16, PML ≤ 16
  ok    LAW_CONCORDANCE.md matches the law files           subjects and citation map current

------------------------------------------------------------------------------
PASSED — 113 laws, 532 process requirements, 725 foundational citations and 1469 certification citations all resolve.
```

Exit code 0.

---

*Charter §5 Stage 9 record, prepared 4 August 2026. Not a PCI Law; creates no obligation. The
seventeen items in §11 are the agenda for human review and none of them was fixed in this pass.*

> **AI proposes; the professional verifies, decides and remains accountable.**
