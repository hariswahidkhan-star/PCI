# Implementation record — the 17 open completeness findings, PML-AI Body of Knowledge

**Date:** 4 August 2026
**Scope worked:** `docs/books/pml-ai/` (manuscripts and derived files), plus two `_build/` files that
derived output could not be made correct without — `_build/checks/pml_d08_ext.py` (golden checks for
one new worked example) and `_build/make_standards.py` (characterisations and patterns for the newly
named reference points, since `STANDARDS.md` is generated and a hand edit would be overwritten).
**Out of scope, untouched:** `docs/bok/`, `docs/books/pfl-ai/` manuscripts, `docs/books/laws/`.
**Not committed.**

**Verification:** `docs/books/_build/verify_formulas.py` — **`✓ all golden answers verified`**,
before and after. No existing number, worked example or golden answer was altered. The PML-AI
Domain 8 check tally moved from 535 to 543: eight new checks, all for the one new worked example.
All four derived-file generators re-run and re-run with `--check`: **0 defects each**.

---

## 1. Summary

| # | Finding | Severity | Status |
|---|---|---|---|
| PML-03 | D2 2.3.3 sustainability as a disclosed claim | major | Implemented |
| PML-04 | D3 3.3.2 three-lines model attributed and characterised | major | Implemented, one deviation (section 3.1) |
| PML-05 | D3 3.3.4 custody and retention of the decision record | major | Implemented |
| PML-06 | D6 6.4.2 non-financial ceilings on compression | major | Implemented |
| PML-07 | D7 KA 7.2 and D8 KA 8.3 "AI in this KA" sections | major | Implemented |
| PML-08 | D8 8.4.3 crisis leadership as an instrument | major | Implemented, one deviation (section 3.2) |
| PML-09 | D9 9.3.1 record retention and custody | major | Implemented |
| PML-10 | D10 (+D15, D16) external reference points | major | Implemented, one scope judgement (section 3.4) |
| PML-11 | D11 the register as a record about identified people | major | Implemented |
| PML-12 | D12 12.4.3 when the decision goes the other way | major | Implemented |
| PML-13 | D14 14.4.3 the failure route | major | Implemented |
| PML-14 | D15 15.2.1 cashable / non-cashable classification | major | Implemented, one deviation (section 3.3) |
| PML-15 | D8 8.3.1 duty-engaged rows leave the EMV comparison | moderate | Implemented |
| PML-16 | Domain-opener standards anchors | moderate | Implemented |
| PML-17 | D9 9.3.1 cumulative-effect test operationalised | moderate | Implemented |
| PML-18 | D12 Toolkit 12.T.3 the conversation record as a record | moderate | Implemented |
| PML-19 | D13 13.1.2 what the ordering right does not reach | moderate | Implemented |

Seventeen findings worked; all seventeen implemented. Three carry a deliberate departure from the
proposed correction and one carries a scope judgement, each stated in section 3 with its reasoning.

---

## 2. What was added, finding by finding

### PML-03 — D2 2.3.3, sustainability and ESG value

`manuscript/domain-02-strategy-selection.md`. A third numbered part, **"As a disclosed claim"**,
after the constraint and value parts. It states that a benefit reported outside the organisation
stops being an estimate and becomes a claim, and sets four provisions: a stated boundary and method;
a named owner; retained evidence sufficient for someone else to test it (cross-referenced to Domain
16, KA 16.4.4's retention economics and KA 16.4.1's measurement plan); and approval by whoever signs
the disclosure — expressly not the project. Adds **the professional prohibition** ("a benefit that
cannot be evidenced to the standard its intended audience requires is not reported as achieved") and
the **standing caveat** in the volume's form, including the distinction between a voluntary reporting
framework and a disclosure regime, which the finding correctly identified as routinely conflated.
**ISO 14064** named as the usual reference point for quantifying and reporting greenhouse-gas
emissions and removals, characterised as voluntary guidance and *not itself a disclosure obligation*.

Apparatus: three key terms (disclosed claim · claim boundary · voluntary framework vs disclosure
regime), two self-check items, three fields on Toolkit 2.T.2, a learning objective, and the domain
summary sentence. No arithmetic added.

### PML-04 — D3 3.3.2, assurance lines

`manuscript/domain-03-governance-decision-rights.md`. Two paragraphs before the three-line list:
the vocabulary is named as an assurance architecture published by the **Institute of Internal
Auditors**, voluntary guidance owned by a named body rather than a standard or a requirement, revised
by its owner so the current formulation is a model of roles rather than sequential lines of defence;
and it is one architecture among several, the applicable structure being set by the organisation's
own governance and, in regulated sectors, by what the regulator expects — a lens, not an obligation.
The two asserted structural claims are softened to **"typically"**, with a following paragraph on
why the qualifications are load-bearing across the organisational forms of KA 3.1.2 (including the
consortium case, where there may be two of each line answering to different parents). The domain
opener flags the second reference point. Key term rewritten; self-check item added; domain summary
sentence adjusted. Disclosed in `STANDARDS.md` under a new **Assurance and internal audit** heading.

### PML-05 — D3 3.3.4 and Toolkit 3.T.3 (with D5 5.4.3)

A closing part of 3.3.4, **"Custody and retention"**, with five provisions: a named custodian role
per record class (decision log, change log, gate packs, baseline archive, assurance opinions,
acceptance evidence); a system that preserves version and carries any amendment with the amender and
the reason; a retention period set in advance at the longest of contractual limitation, applicable
retention requirement, benefits-realisation horizon and records policy, with the standing caveat that
the requirements themselves come from the records and legal functions and qualified counsel; an
explicit handover of custody at closure; and an access list with a data-protection caution, since
declared-interest entries and dissents name identified individuals. Toolkit 3.T.3 gains a **custody
block** with two monthly counts. Three key terms, one self-check item, one learning objective, and a
summary sentence. `manuscript/domain-05-scope-requirements-value.md` 5.4.3's "the evidence is
retained" gains the three fields that make it real (class, custodian, period with its source named).

### PML-06 — D6 6.4.2

`manuscript/domain-06-planning-scheduling-flow.md`. **"What bounds the menu"** inserted before Worked
example 6.4.2b: each compression lever carries a non-financial ceiling — working time, rest and
fatigue; the safety case and permit regime; agreement terms — jurisdiction-, sector- and
site-specific, taken from the safety function, HR and counsel, with the operative rule that **a lever
whose ceiling binds is struck from the menu rather than priced into it**. Followed by the
decision-rights paragraph: a shift-pattern, night-working or simultaneous-operations move requires
the **named safety approver in addition to** the authority spending the money, and Worked example
6.4.2c's overlap is named as a simultaneous-operations decision before it is an arithmetic one.
Toolkit 6.T.4 gains "non-financial ceiling" and "the approval it needs"; Toolkit 6.T.2 and the
Recovery paragraph now record **both** authorities. Three key terms, two self-check items, a learning
objective and the domain summary. No number altered.

### PML-07 — D7 KA 7.2 and D8 KA 8.3

`manuscript/domain-07-cost-resources-commercial.md`: a full **"AI in this KA"** section for KA 7.2 on
the Domain 5 three-part pattern, immediately before "Key terms — KA 7.2". *Earns its place*:
ledger-to-cost-report reconciliation producing the list rather than the total; periods with no accrual
posted against evidence of receipt; open-PO sweeps for commitment coverage and stale lines;
control-account transfers with no change reference. *Must not go*: determining whether work has been
received, setting or adjusting an accrual, authorising a control-account transfer, re-baselining.
*Verification, concretely*: recompute `AC`-plus-accrual against the ledger; confirm the following
period's reversal (7.2.1's timing invariant); reproduce commitment coverage by hand for one period.

`manuscript/domain-08-risk-uncertainty-resilience.md`: the equivalent section for KA 8.3, with the
explicit prohibition that **no model output may authorise a reserve draw** — a draw is an attributable
decision under the published protocol of 8.3.2 — plus prohibitions on choosing the confidence level,
revising `p` or impact, retiring a risk, and determining whether a duty is engaged. A learning
objective added in D7.

### PML-08 — D8 8.4.3, crisis leadership

Expanded into three named parts. **Declaration and authority**: who may declare (named role, with an
out-of-hours alternate); what the declaration changes, stated as authorities rather than urgency — a
named stop-work authority, a stated fund with the reserve named and the draw protocol *expressly
modified rather than silently suspended*, a standing cadence; who stands it down; and what does not
change. Modelled on the emergency change route of Domain 4, KA 4.4.1, as the finding asked.
**Notification**: the one-page notification map held with the risk register, written before any
incident, recording which notifications could fall due, who determines that they do, to whom, within
what period and who may make them — with the insurer entry and a "who may speak" line — under the
standing caution. **Record**: a contemporaneous decision log from the moment of declaration with a
finer clock, and the instruction to **preserve rather than tidy**.

**New Worked example 8.4.3 — the price of a notification that drifts.** Five steps, in the shape of
Worked example 1.2.2. Early notification **USD 54,560**; at week 6 **USD 197,240**; six weeks of
drift **USD 142,680**, **2.6151 times** the whole cost of notifying on time; the avoidable cost
accrues at **USD 23,780 a week** and **1.0934 weeks** of drift pays for the entire USD 26,000
notification pack. Interpretation closes with the prohibition that the arithmetic prices drift and
never decides whether a notification is due. **Eight golden checks added to
`_build/checks/pml_d08_ext.py`**, including two invariants — that the avoidable cost is linear in the
drift, and that the pack and the hold cancel from the comparison, so the run rate is the whole
decision. Four key terms, three self-check items, a learning objective and a summary passage.

### PML-09 and PML-17 — D9 9.3.1 and Toolkit 9.T.2

`manuscript/domain-09-quality-assurance-improvement.md`. Three additions to 9.3.1:

- **External acceptance.** Where an approval regime applies, a repair, concession or regrade may
  engage a certifying body, notified assessor or regulator as well, potentially before use; the
  professional position is that the disposition is treated as unavailable until the external position
  is established, that establishing it sits outside the project's authority, and that what applies
  differs by sector and jurisdiction and is settled with the regulatory function and the body itself.
- **Making the cumulative-effect test operable** (PML-17): five decisions, all belonging in the
  quality management plan of 9.1.3 — who sets the threshold (sponsor or design authority, at
  baseline, never the delivery team); how it is derived, using the window logic of Domain 3, KA
  3.3.4c (above the base-rate aggregate of the class, below the material-difference point); the
  aggregation period; the relatedness test, defined by *what the concessions touch*; and the named
  owner of the running total. Plus the consequence: **suspension of further concessions in that class
  pending a decision by the aggregate authority**, taken through Domain 3's escalation machinery.
- **The record, and how long it lives** (PML-09): record class, named custodian, retention period and
  disposal authority, on Domain 3, KA 3.3.4's machinery, with Domain 16, KA 16.4.4 cross-referenced
  for the economics and the explicit statement that nothing in the domain states a legal minimum.

Toolkit 9.T.2 gains the three record fields, the external-acceptance field, the four cumulative-test
parameters and two further monthly counts. Five key terms, three self-check items, a learning
objective and a summary passage.

### PML-10 — D10 (and D15, D16) reference points

`manuscript/domain-10-procurement-contracts-supply.md`. In 10.3.1: **NEC, FIDIC and JCT** named as
families of *privately published model forms* with no standing of their own, mattering to a project
because the parties wrote one into their agreement — what one then means between them being a
question of that contract and the applicable law, for counsel; the professional value stated (each
embeds a default risk allocation and administration rhythm) and the professional error named (treating
"we are using a standard form" as settling an allocation the parties have amended). **ISO 44001**
named for collaborative business relationships, voluntary. In 10.4.3: the **UN Guiding Principles on
Business and Human Rights** and the **OECD Guidelines for Multinational Enterprises** with its
due-diligence guidance, named as **non-binding international instruments** that supply the method and
impose no obligation of themselves, expressly distinguished from the national due-diligence
legislation the section already discusses; **ISO 20400** named as voluntary guidance and *not a
certifiable requirements standard*. D15 opener anchors **ISO 21503/21504**; D16 opener anchors
**ISO 15489** (records management) and **ISO/IEC 20000** (service management), with the explicit note
that neither states a retention period applicable to any organisation.

### PML-11 — D11 11.1.2, 11.A.2, Toolkit 11.T.1

`manuscript/domain-11-stakeholders-communication-influence.md`. **"What the register is, as a record"**
opens 11.1.2 with five provisions: a stated owner and access list; attitude recorded as an **observed
position with its source** rather than a characterisation of the person; deletion of departed role
holders' assessments at refresh; a deletion point at closure; and the caution that data-protection
obligations, including any entitlement of the individual to see what is held, apply on their own terms
in their own territories and are settled with whoever holds that accountability and with counsel —
nothing stating any jurisdiction's position. The domain's own KA 11.3 principle is quoted and extended
to human-authored entries. 11.A.2 gains a fourth practice, **delete rather than carry forward**, tied
to the 30.6 % turnover figure already in the text. Toolkit 11.T.1 gains a **record block** (owner,
access list, refresh deletion, deletion point) and two monthly counts; the attitude column now
requires the observed act and source. Three key terms, one self-check item, a learning objective and
a summary passage.

### PML-12 and PML-18 — D12 12.4.3 and Toolkit 12.T.3

`manuscript/domain-12-leadership-teams-behaviour.md`. **"When the decision goes the other way"**
supplies the mechanism the section previously lacked: the **dissent record** (fact stated, date and
forum, to whom, decision taken, risk remaining), filed in the decision record under Domain 3, KA 3.3.4
with a copy to a party other than the decision-maker; **declining the signature** on an acceptance,
readiness certificate, completion statement, forecast or account the leader does not believe, in
writing, saying what would have to change; and the **route of last resort** for safety and harm — next
governance tier, independent assurance, then the board or its audit or risk committee, each step in
writing. Closes with the neutral note on protected-disclosure arrangements (section 3.5 below on wording).
Toolkit 12.T.3 gains a closing block — custodian; access limited to the leader, their manager and HR;
a copy to the individual; a retention period from the organisation's employment-records policy with
disposal; the working-note/file distinction; and the statement that where the record may feed a formal
process the HR and legal functions own its retention and access. Four key terms, two self-check items,
a learning objective and a summary passage.

### PML-13 — D14 14.4.3, 14.A.2, 14.A.3, Toolkit 14.T.1

`manuscript/domain-14-digital-data-responsible-ai.md`. **"When a relied-upon output turns out to be
wrong"** closes 14.4.3 with a six-step sequence: stop the use and suspend the register entry;
enumerate every recorded decision that cited the output from the register's *decision informed*
column; notify the accountable person and the decision-maker in writing the same day in Domain 11,
KA 11.2.3's escalation-grade form; reopen decisions above a **pre-set materiality threshold** with the
authority that took them, recording those not reopened as considered and confirmed; record whether an
affected person, counterparty, insurer or supervisory body must be told and **on whose determination**
(a question for the relevant authority and counsel); and re-measure `p` and `q` by the same method
under a named authority before resumption. 14.A.2 gains a third lifecycle obligation, **failure**,
distinguishing a hygiene trigger from an incident with a clock. 14.A.3 gains the invariant the finding
specified verbatim in substance. Two register columns added, in the 14.4.3 table and in Toolkit
14.T.1, plus a third monthly integrity count. One key term, one self-check item, a learning objective,
an executive-perspective bullet and the domain summary.

### PML-14 — D15 15.2.1, Toolkit 15.T.3, D16 16.4.1

`manuscript/domain-15-programmes-portfolios-enterprise.md`. The register definition now carries a
**mandatory fund-type classification** with a four-row table — cashable (budget line named),
capacity-released, cost-avoidance, non-financial — and the column stating what each may be compared
against. The **decision right** is stated: the finance owner of the affected budget, not the portfolio
board and not the delivering component, confirms cashability in writing naming the line and the
period, and **an unconfirmed cashable claim is recorded as capacity-released until it is**. Worked
example 15.2.1 gains a fourth interpretation point disclosing the composition of its own denominator
(section 3.3 on the option taken). Toolkit 15.T.3 gains the fund-type tag per line, a subtotal by fund type,
the composition of the stream the rule is applied to, and two further integrity counts. Four key
terms, two self-check items, a learning objective and a summary passage.
`manuscript/domain-16-transition-closeout-benefits.md` 16.4.1 adds the fund type to the required
elements of the measurement plan, with the reason it must be carried forward from the approval — it
decides what the realised number may be compared against — plus a key term, a self-check item and a
learning objective.

### PML-15 — D8 8.3.1 and Toolkit 8.T.1

A third governing rule after "Impact governs survivability even at low probability", worded to match
Domain 1, KA 1.4.3's swept formulation: **where a duty arising under law or regulation, an operating
licence or a safety obligation is engaged, the duty is not transferable and acceptance is not an
available response** — insurance and contract terms move money, not accountability, and only so far
as the wording and the applicable law allow. The practical consequence is stated: duty-engaged rows
are marked and **taken out of the EMV comparison before it is run**, leaving the narrower question of
*how* the duty is met. Worked example 8.3.1's "Two boundaries" becomes "Three boundaries", the third
explaining why the table is the right instrument for R1 and would not be for a duty-engaged version of
the same event. A **duty-engaged flag** added to Toolkit 8.T.1 as a mandatory field, mirroring Domain
5's non-prioritisable regulatory class and Domain 1's `mandatory — not an expected-value decision`
marking, together with a notification-map reference column. One key term.

### PML-16 — domain-opener anchors

A short **"Reference points"** paragraph added to the "Why this domain exists" section of Domains 2,
3, 4, 7, 8, 11, 15 and 16, in a single consistent form: name the document, characterise it as
voluntary guidance describing practice rather than legislation or a certifiable requirement, state
that it obliges nobody of itself unless an organisation, a contract or a regulator adopts it, state
that it is named and not reproduced, point the reader to the publisher for the current edition, and
disclaim endorsement in either direction.

| Domain | Anchored |
|---|---|
| D2 | ISO 21504 (within the ISO 21500 family) |
| D3 | ISO 21505, plus a pointer to KA 3.3.2's second reference point |
| D4 | ISO 21502 |
| D7 | AACE International's Total Cost Management framework, promoted from 7.1.1 |
| D8 | ISO 31000 |
| D11 | ISO 21502, with the note that stakeholder engagement has no standard of its own |
| D15 | ISO 21503 and ISO 21504, alongside ISO 21505 |
| D16 | ISO 15489 and ISO/IEC 20000 |

`STANDARDS.md`'s "Discussed in" column is derived and now agrees with the manuscripts: ISO 31000
D1, D8 · ISO 21505 D1, D3, D15 · ISO 21502 D1, D4, D11 · ISO 21504 D1, D2, D15 · ISO 21503 D1, D15.

### PML-19 — D13 13.1.2

`manuscript/domain-13-agile-adaptive-hybrid.md`. **"What the ordering right does not reach"** added
directly after "What the role must be able to do", where the decision right is defined rather than
1,200 lines later: the product owner orders **discretionary** value, and items carrying a safety,
regulatory, statutory, contractual or security-remediation obligation are **constraints on the order,
not candidates within it** — scheduled to their required date by the authority accountable for the
obligation, **excluded from the delay-cost-density ranking of 13.1.3**, and classified as such when
the item is created. An owner who deprioritises one has exceeded the right rather than exercised it.
Where the obligation itself admits options, the arithmetic returns — applied to the options rather
than to whether. Cross-referenced to Domain 3's decision classes, Domain 8, KA 8.3.1's duty rule and
the healthcare industry variation. Toolkit 13.T.2 gains the obligation/discretionary classification
and restricts the product-owner decision cell to discretionary items. Two key terms, one self-check
item, a learning objective and a summary passage.

---

## 3. Departures from the proposed corrections, and one scope judgement

### 3.1 PML-04 — the revision year is not printed

The finding notes that the model was "materially revised in 2020". The revision is described
("revised by its owner so that the current formulation is a model of roles rather than sequential
lines of defence") but **the year is not stated**, because the volume's register policy is that no
entry carries an edition year or revision date — a citation to a superseded edition reads as
authoritative while being wrong, and this build has no way to confirm the current position. The
substantive correction the finding sought is unaffected: the reader is told whose model it is, that
it is voluntary, that it has been revised, and that the older defensive/sequential reading is the one
its own publisher moved away from.

### 3.2 PML-08 — "statutory rather than discretionary" not used

The proposed caution said the applicable duties are "statutory rather than discretionary". The legal
sweep completed on these files deliberately replaced "statutory duty" with "duty arising under law or
regulation" throughout this volume, and describing a class of duties as statutory would reintroduce
the swept construction. The caution therefore reads: *obligations of this kind arise under law,
regulation, licence conditions or contract rather than at the project's discretion — but whether any
of them reaches this organisation, this incident, this territory and this timetable is a question for
the relevant authority and for qualified counsel*, followed by the express disclaimer that nothing
states the position anywhere or characterises any act or omission as compliant. The professional
obligation the finding wanted is stated unconditionally: build the map before the incident, name the
person who takes the question to counsel, take it promptly, record that you did.

### 3.3 PML-14 — the second option, not the first

The finding offered two ways to fix the mixed-denominator problem in Worked example 15.2.1: restate
the payback test on the cashable stream, **or** state explicitly which components' benefits are
cashable and that the rule is being applied to a mixed stream by the committee's own convention. The
second was taken, for two reasons. First, every printed number in the example is pinned by the
golden-answer suite and the instruction is that no existing number may be altered; restating the test
would require a new denominator and a new payback figure resting on a cashability split the example's
own data does not establish. Second, and more importantly, the professional point is stronger the
second way: *only the finance owner of the affected budget can say which savings are budget-removable*,
which is precisely the decision right the finding asks the register to record. The example now
discloses the composition of its own denominator component by component, names the fund type of each,
and states the obligation as disclosure rather than recomputation — with the observation that the
honest 4.3129-year figure is more defensible than the unreconciled 3.5188 and is still a comparison of
cash against something that is not all cash. Nothing in the arithmetic moved.

### 3.4 PML-10 — Domains 12 and 13 were not given standards anchors

The finding observes that Domains 11, 12, 13, 15 and 16 name no external authority, and its fix
prescribes anchors for 15 and 16. Anchors were added to 15 and 16 as directed and, additionally, to
Domain 11 — ISO 21502 genuinely addresses stakeholder engagement within project-management guidance,
and the line also carries the honest statement that stakeholder engagement has no standard of its own.

**Domains 12 and 13 were deliberately left unanchored.** There is no reference point for delivery
leadership, team behaviour or adaptive delivery that could be named without one of three failures the
volume's charter forbids: citing a certification body's or a commercial framework's publication (the
register expressly contains no bibliography of certification-body publications); naming a
trademark-bearing method in a way that reads as affiliation; or manufacturing an anchor that does not
in fact address the subject, which is worse than none because it sends a reader somewhere useless. An
anchor added for symmetry would be an over-claim of exactly the kind the finding set itself against.
This is recorded as a judgement, not an omission — if a suitable reference point is identified later
the same paragraph form is ready for it.

### 3.5 PML-12 — the protected-disclosure note

Written in the swept form rather than the proposed one: such arrangements are "understood to exist in
many jurisdictions", their scope, conditions and protections "differ materially", and the text states
expressly that **nothing here states the position in any jurisdiction, states that any such
arrangement applies to any person or organisation, or characterises any response to a disclosure as
lawful or otherwise**. The two practical consequences are stated — engage the organisation's policy
and legal function before any external step, because the order of steps can matter; and the
unconditional professional obligation is to raise the concern internally, promptly, in writing, keep a
copy, and take advice early. No employer's act is characterised anywhere in the addition.

---

## 4. Legal-sweep compliance of the new material

The sweep completed on 4 August 2026 was treated as a constraint, not a baseline. Nothing it changed
was reverted; no caution was removed; cautions were added in eleven places. Every addition was checked
against the sweep's own patterns:

- **No statement of what the law requires.** Every obligation-shaped sentence is either a professional
  obligation this book imposes, or a conditional ("may engage", "could fall due") immediately followed
  by the referral to the relevant authority and qualified counsel.
- **No act characterised as lawful or unlawful.** The only occurrence of "lawful" in the new material
  is inside an express disclaimer of characterisation (D12).
- **No statement of what an external standard requires.** Every named document is characterised at the
  level of what it addresses, in the book's own words. No clause number, article, paragraph or edition
  year appears anywhere in the manuscripts (checked by pattern).
- **No implied endorsement.** Every new reference-point paragraph disclaims endorsement in both
  directions, and `STANDARDS.md`'s volume-level non-endorsement statement is unchanged.
- **Data-protection cautions** added wherever new content directs recording information about
  identified individuals: the stakeholder register (D11 11.1.2, 11.A.2, Toolkit 11.T.1), the decision
  record's access provisions (D3 3.3.4), the crisis decision log (D8 8.4.3), and the
  difficult-conversation file (D12 Toolkit 12.T.3).
- **Employment-sensitive ground** (D12 dissent, escalation, conversation records) refers every legal
  question to the organisation's HR and legal functions and to qualified advice, and states the
  professional obligation separately so that it survives whatever the local position turns out to be.
- **Safety and regulatory approvals** (D6 compression, D9 dispositions, D8 duty rule) are framed as
  professional positions — the lever is unavailable, the disposition is treated as unavailable, the
  row leaves the comparison — with the determination of whether an obligation is engaged referred out
  in every case.

---

## 5. Build and verification

| Check | Result |
|---|---|
| `python3 _build/verify_formulas.py` | **✓ all golden answers verified** (58 modules) |
| PML-AI Domain 8 check count | 535 → **543** (8 new, one worked example) |
| Any existing number altered | **None** |
| `python3 _build/make_standards.py --check` | 0 defects; pml-ai 33 references, pfl-ai 14 |
| `python3 _build/make_glossary.py --check` | 0 defects; pml-ai 622 terms |
| `python3 _build/make_question_bank.py --check` | 0 defects; pml-ai 363 items |
| `python3 _build/make_appendices.py --check` | clean |
| Cross-references resolve | Checked: 4.3.3, 4.4.1, 9.1.1, 11.2.3, 14.4.4, 16.4.1, 16.4.2, 16.4.4 and all KA-level targets cited in new text exist |
| Repository paths / spec blocks / draft markers in reader-facing text | None (pattern-checked) |
| Retired principle wording | None; the suite principle appears only as "AI proposes; the professional verifies, decides and remains accountable." |
| British English house forms | Checked: no American spellings anywhere in the PML-AI manuscripts |
| PCI Standard identifiers cited | None cited, so none to verify against `laws/PML_AI_STANDARDS.md` |

`_build/build_book.py` could not be run in this environment (the `markdown` package is not installed);
this is a pre-existing environment limitation and unrelated to the edits.

### Files changed

**Manuscripts (13):** `domain-02` · `domain-03` · `domain-04` · `domain-05` · `domain-06` ·
`domain-07` · `domain-08` · `domain-09` · `domain-10` · `domain-11` · `domain-12` · `domain-13` ·
`domain-14` · `domain-15` · `domain-16`.
**Derived, regenerated (4):** `pml-ai/STANDARDS.md` · `pml-ai/GLOSSARY.md` ·
`pml-ai/QUESTION_BANK.md` · `pml-ai/APPENDICES.md`. (`pfl-ai`'s three derived files were regenerated
in the same run, as the generators write both volumes; their diffs contain only the OECD
characterisation extension below and one AACE row that another agent's manuscript edit had already
earned but not yet regenerated.)
**Build (2):** `_build/checks/pml_d08_ext.py` (8 checks + docstring entry);
`_build/make_standards.py` (three new families, one extended characterisation, five new patterns).

The single change to a shared file with cross-volume effect is the **OECD** characterisation, extended
to cover the Guidelines for Multinational Enterprises and the associated due-diligence guidance —
"recommendations addressed to adhering governments and, through them, to enterprises, and not
obligations of themselves". It is accurate for both volumes and was necessary because the bare `OECD`
pattern matches inside "OECD Guidelines", which would otherwise have attached D10 to a
characterisation that did not cover the use.

### Nothing left open

No finding was deferred. The four judgement calls are recorded in section 3. One item is worth a reviewer's
eye at the next pass rather than being a defect of this work: `TOC.md`'s back-matter list names the
standards register as "Appendix C" while `STANDARDS.md` generates as "Appendix F" and `APPENDICES.md`
generates A–E. The new reference-point paragraphs deliberately avoid citing an appendix letter for
that reason, so nothing in the manuscripts depends on the discrepancy being resolved either way.
