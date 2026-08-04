# Suite Terminology Register and Collision Audit — PCL-AI · PML-AI · PFL-AI

**Status:** Audit of how 28 load-bearing terms are actually defined and used across the three books
and the PCI Standard set, checked against the shared registry `TERMINOLOGY.md`.

**Identifier note.** The instruments this audit originally called *laws* are now **PCI Standards**, and
their identifiers migrated twice — `PCL-LAW-13-04` → `PCI-PCL-LAW-13.04` → `PCI-PCL-STD-13.04`. Every
identifier cited below has been re-pointed **by subject** to the standard that carries the meaning the
sentence relies on, not by matching numbers: two of the three did not keep their number or their title.
The mapping is recorded in [`../laws/STANDARDS_CONCORDANCE.md`](../laws/STANDARDS_CONCORDANCE.md) §3.

**Method.** Every row below was built by reading the definitions where they live — the
`### Key terms — KA N.K` tables in `pml-ai/manuscript/` and `pfl-ai/manuscript/`, the derived
`GLOSSARY.md` of each book, PCL-AI's Appendix B (`docs/bok/appendices.md`), the binding
`docs/bok/00-style-spine.md` §3, and the PCI Standard files — and then counting actual usage across the
corpus. Nothing here is inferred from a term's ordinary meaning. Counts are from the corpus as it
stood on **2026-08-03**.

---

## 1. How to read this register

**The point of this audit is not to make every word mean one thing.** Several of these terms carry
genuinely different, equally legitimate professional meanings, and flattening them would make the
books wrong. Where that is so, the **Collision note** column states each context explicitly and the
Proposed suite definition says "no single definition — context flag required".

Where a term is *accidentally* inconsistent — the same concept described two ways, or a shared
registry rule quietly breached — that is a defect, and it is listed in **§4 Issues found** with a
file, a line and a proposed resolution.

**Three tiers of severity are used in §4:**

- **Defect** — a rule the programme has already written down is being broken. Fix required.
- **Drift** — two definitions that ought to match and do not. Fix or flag.
- **Gap** — a term the corpus leans on heavily and has never defined. Fill.

**A note on the glossaries.** Both new books' glossaries are *derived* — generated from the
manuscripts by `_build/make_glossary.py`, which already classifies multiply-defined terms as
RESOLVED, CROSS-REFERENCED, LAYERED, REDUNDANT or COLLISION and fails the build on the last two.
Running it on 2026-08-03 reports **0 open defects** (581 PML-AI terms, 446 PFL-AI terms). That
checker is good, and this audit does not duplicate it. What it cannot see is the ground this audit
covers: **PCL-AI, which has no derived glossary of that kind; the PCI Standard files, which are outside every
book build; the symbol registry in `FORMULAS.md`; and cross-book agreement.** Every issue in §4 is
invisible to the in-book checker for one of those reasons.

---

## 2. The register

Books are abbreviated **PCL** (PCL-AI, `docs/bok/`), **PML** (`docs/books/pml-ai/`), **PFL**
(`docs/books/pfl-ai/`), **Standards** (the PCI Standard set, `docs/books/laws/`). The Book(s) column
below reads **Standards** where earlier revisions read *Laws*; it is the same body of content under its
current name.

### 2.1 Structural terms

| Term | Book(s) | First definition | Later definitions | Inconsistent usage found | Proposed suite definition | Collision note |
|---|---|---|---|---|---|---|
| **project** | PML, PCL, PFL | PML KA 1.1: "Temporary endeavour producing a defined result." | PML KA 1.1 also frames it by *accountability lens*: "Delivery of a defined output to time, cost, quality — was the thing delivered, fit for purpose?" (line 61) | None. PCL and PFL use the term constantly but define only compounds (*project charter*, *project return*) | **A temporary endeavour undertaken to produce a defined result.** | None. The lens table at PML line 61 is a complementary framing, not a rival definition. |
| **programme** | PML | PML KA 1.1 (line 200): "Related projects managed together for outcomes no one project delivers." | PML KA 15.1 (line 284): "A temporary organisation delivering a coherent outcome through components that cannot deliver it individually." | **Yes — two definitions.** The derived glossary makes KA 15.1 canonical and demotes KA 1.1 to an "Also read at" line. Classified LAYERED by the checker, so it does not fail the build | **A temporary organisation delivering a coherent outcome through components that cannot deliver it individually** (the KA 15.1 form: it says what a programme *is*, not merely what it contains). | Not a collision — layered depth. But the suite registry records neither form. See §4 Issue 10. |
| **portfolio** | PML | PML KA 1.1 (line 201): "The funded, balanced set of projects and programmes." | PML KA 15.1 (line 285): "The set of programmes and projects an organisation chooses to run within a finite capacity and a strategy." | **Yes — two definitions**, same LAYERED classification. "Funded, balanced" and "within a finite capacity and a strategy" emphasise different constraints | **The set of programmes and projects an organisation chooses to fund and run within a finite capacity and a stated strategy** (merges both emphases). | Not a collision — layered. See §4 Issue 10. |
| **control account** | PCL (binding), PML | **`00-style-spine.md` §3 (line 64)** — inherited, binding: "A management-control point where scope, budget, actual cost and schedule integrate — the intersection of a WBS element and an organisational (OBS) element." | PML KA 4.2: "The level at which performance is measured and reported (Domain 7's earned value attaches here)." | **Yes.** `TERMINOLOGY.md` §1 lists *Control account* as inherited **unchanged**. PML's form drops the WBS×OBS intersection — the part that makes it a *control* account rather than a reporting level | Keep the **style-spine definition verbatim**; PML's sentence is a useful gloss and should be marked as one. | Not a collision — an unannounced narrowing of an inherited definition. See §4 Issue 4. |

### 2.2 Accountability and governance

| Term | Book(s) | First definition | Later definitions | Inconsistent usage found | Proposed suite definition | Collision note |
|---|---|---|---|---|---|---|
| **sponsor** | PML, PFL | PML KA 3.2: "The individual accountable for the project's business outcome and mandate." | PFL KA 1.1: "Equity investor promoting the project (**this book's project-finance sense**)." PFL KA 5.2 expands: "brings capital, capability, access and credit." | **None — this is handled correctly.** `TERMINOLOGY.md` already flags it, and PFL's glossary entry carries the context flag in its own text | **No single definition — context flag required.** | **Genuine collision, correctly resolved.** *Delivery/governance sense:* the accountable executive owner of the business case. *Project-finance sense:* an equity investor promoting the project. Both are standard in their own professions; neither may displace the other. This is the model the other collisions below should follow. |
| **accountability** | PML, Standards | PML KA 1.2: "The obligation to answer; single-holder, non-delegable." | The Standard set uses it throughout as the organising idea (`PCI-FND-STD-01` *Professional Accountability*, and `PCI-PCL-STD-13.04`, which cites it rather than restating it) without redefining — correct, they cite the concept. **Re-pointed by subject:** the PCL-AI standard formerly titled *Professional Accountability* (`PCL-LAW-13-04`) is now `PCI-PCL-STD-13.04` *Disclosure of AI Assistance in a Controls Deliverable* — same identifier position, different title | None. But **PCL and PFL never define it** despite 137 uses corpus-wide | **The obligation to answer for an outcome; held by one person and not delegable.** | None. Cleanly distinguished from *responsibility* by PML — the sharpest pair in the corpus. |
| **responsibility** | PML | PML KA 1.2: "The obligation to do; shareable and delegable." | None | None | **The obligation to perform work; shareable and delegable.** | None. The accountability/responsibility pair is defined once, correctly, and used consistently. |
| **governance** | PML, PCL | PML KA 3.1: "The decision rights, accountabilities and information flows through which an organisation directs and controls a project." | PCL Appendix B (line 211) defines only **"Governance / lineage"** in the *data* sense: "Ownership/definitions/access / traceability of a data point to source" | **Yes, mild.** PCL's only glossary entry for the word is data governance; project governance is used ~588 times corpus-wide and never defined in PCL | **The decision rights, accountabilities and information flows through which an organisation directs and controls a project, programme or portfolio.** Data governance to be written as *data governance*, always. | **Genuine collision between two established senses** — enterprise/project governance and data governance. Resolution is naming discipline, not one definition. |
| **assurance** | PML | **Never defined as a base term.** PML KA 3.3 defines only *assurance map* and *assurance capture* | None | **Gap.** 162 uses corpus-wide, no definition anywhere | **Independent examination providing confidence that a control, process or deliverable is what it is claimed to be.** | Must be distinguished from *verification* — see below. |
| **verification** | PML, PCL, PFL, Standards | PML KA 5.4: "Did we build what we specified?" (paired with *validation*: "Does what we built produce the outcome we needed?") | PCL KA 13.3: "Checking every AI output against source before use." PFL KA 1.3 *verification duty*: "The named human's obligation to check machine output before reliance." PML KA 1.4 *verification proportionality*: "Depth of checking matched to the stakes of reliance" | **Yes — two different concepts share the word**, and the suite principle uses the second sense while PML D5 teaches the first | **No single definition — context flag required.** | **Genuine collision.** *Engineering V&V sense:* did we build what we specified (vs *validation*: does it produce the outcome). *AI-assurance sense:* checking machine output against source before reliance. The suite principle "AI proposes; **the professional verifies**, decides and remains accountable" is the second sense. Both must survive; write *V&V verification* or *AI verification* where a chapter risks ambiguity. |
| **responsible AI** | PML, PFL, Standards | **Never defined as a base term.** `TERMINOLOGY.md` defines "**Responsible AI principle**" (the suite formulation) but not the phrase itself | Two domains are *named* for it (PML D14, PFL D16) | Only 6 bare uses of "responsible AI"; the corpus overwhelmingly uses the principle instead | Keep **"Responsible AI principle"** as the defined term and continue to prefer it. Add: *responsible AI* = the practice of applying that principle. | None. Low-traffic term; the discipline of using the *principle* rather than the adjective is working. |
| **professional judgement** | PCL, PML, PFL, Standards | Never given a glossary definition in any book | — | **Spelling is inconsistent**: "professional judgment" (PFL D3, PFL D5, PML D1) and "professional judgement" (PFL D5, PML D12/D14/D16, PCL D4/D6/D13, and the Standard set) both appear — twice inside PFL D5 alone | **The reasoned exercise of expertise where the evidence does not determine the answer; it is the thing a professional is accountable for and the thing AI may not perform.** | None conceptually. The problem is orthographic — see §4 Issue 7. |

### 2.3 Planning, control and measurement

| Term | Book(s) | First definition | Later definitions | Inconsistent usage found | Proposed suite definition | Collision note |
|---|---|---|---|---|---|---|
| **baseline** | PCL (binding), PML | **`00-style-spine.md` §3 (line 63)** — inherited, binding: "The approved, version-controlled plan (scope, schedule or cost) against which performance is measured." | PML KA 2.3: "The measured pre-change position; must be measured before, not reconstructed after." PCL Appendix B: "**Baselines (scope/schedule/cost)** — The approved, integrated plans control measures against" | **Yes, material.** `TERMINOLOGY.md` §1 lists *Baseline* as inherited **unchanged**, but PML's glossary carries the benefits-measurement sense as canonical. PML then uses the *plan* sense in D4 (*baseline drift*, *baseline maintenance*) — both senses live in one book | **No single definition — context flag required.** | **Genuine collision, currently undeclared.** *Control sense (inherited, binding):* the approved, version-controlled plan performance is measured against. *Measurement sense:* the pre-change position a benefit is measured from, captured before the change. Both are legitimate; PML needs both. Write *benefits baseline* for the second. See §4 Issue 3. |
| **contingency** | PCL, PML, PFL | PCL D3 (line 128): "**Contingency reserve** — For identified risks; inside the baseline; PM-controlled." | PML KA 7.1: "Inside the baseline; funds identified risks; PM-controlled." PFL KA 8.3: "**Contingency** — Funded provision for identified risks within agreed scope; drawn on certification." | None. PCL and PML agree word for word in substance; PFL adds the financing mechanic (*drawn on certification*) without contradicting them | **A provision for identified risks, held inside the baseline and released under a stated authority.** | None. A clean example of layered depth done right. |
| **management reserve** | PCL, PML, PFL | PCL D3 (line 129): "For unforeseen scope/risk; outside the baseline; **management-controlled**." PCL D12 (line 405) repeats it | PML KA 7.1: "Outside the baseline; unknown-unknowns; **sponsor-controlled**." PML KA 8.3 table: "Sponsor / change authority, via change control." PFL KA 8.3: "in a financing, largely replaced by contingent support" | **Yes.** The controlling role differs: *management* (PCL) versus *sponsor* (PML). Both are defensible practice; the registry claims consistency | **A provision for unidentified risk and scope change, held outside the baseline and released only by the authority named in the governance plan** — which names the role rather than guessing it. | Not a collision — an unreconciled detail. See §4 Issue 5. |
| **forecast** | PCL, PML, PFL, Standards | **Never defined as a base term in any book** | Compounds only (*forecast honesty*, the subject of the withdrawn `PCL-LAW-03-03`, now `PCI-PCL-STD-03.04` *Completeness of the Estimate at Completion*; EAC methods in PCL D6) | **Gap.** 1,173 uses corpus-wide; two whole PCI Standards govern it (`PCI-PCL-STD-03.04` and `PCI-PCL-STD-03.05` *Independent Challenge and Approval of the Forecast*) | **A current best estimate of a future outcome, stated with its method and its assumptions.** | None. Pure gap. |
| **EAC** | PCL, PML, PFL | PCL Appendix B (line 104): "**`EAC` / `ETC`** — Estimate at completion / to complete; `EAC = AC + ETC`." Registered in `FORMULAS.md` §1 in this sense only | PML KA 7.3: "Estimate at / to complete; `EAC = AC + ETC`" — identical. PFL KA 8.4 uses the same sense (`CTC` = `EAC − AC`). **PFL KA 4.2.3 (line 671) defines "`EAV` / EAC" as "NPV (cost PV) converted to a level annual equivalent"**, with the formula `EAC = PV / AF(r, n)` at lines 590–593 | **Yes — the sharpest inconsistency in the corpus.** One symbol, two different formulas, inside one book | **`EAC` = Estimate at Completion, suite-wide, without exception.** The annuity sense is already registered as **`EAV`** in `FORMULAS.md` §3 and must use it. | **Genuine professional collision** — *equivalent annual cost* is standard finance usage and *estimate at completion* is standard controls usage. But unlike *sponsor* or *PV*, this one is **undeclared and unregistered**, and it collides with an inherited symbol. See §4 Issue 1. |
| **PV** | PCL, PML, PFL | `FORMULAS.md` §1: **`PV` (BCWS) = Planned Value (EVM context)**; separately **`PV(x)` = present value of amount `x`** | `FORMULAS.md` lines 26–28 carry an explicit **notation clash rule**: "`PV` = Planned Value in EVM contexts; discounting always writes 'present value' in words or `PV(x)`. PFL-AI … use `PV(x)`/`FV(x)` forms throughout and reserve bare `PV` for EVM material only." | **Yes — the rule is breached in both books.** PFL uses bare `PV` for present value **93 times across 12 domains** (D3: 32, D4: 27). PML uses bare `PV` for present value **33 times in D2** while using bare `PV` for Planned Value **80 times in D4/D7/D15** | **No single definition — context flag required**, exactly as `FORMULAS.md` already says. The *rule* is right; compliance is the problem. | **Genuine collision, correctly anticipated and then not enforced.** *EVM sense:* Planned Value (BCWS). *Discounting sense:* present value. See §4 Issue 2. |

### 2.4 Risk and uncertainty

| Term | Book(s) | First definition | Later definitions | Inconsistent usage found | Proposed suite definition | Collision note |
|---|---|---|---|---|---|---|
| **risk** | PCL, PML, PFL | PCL D12 (line 69): "An uncertain event/condition affecting objectives — threat or opportunity." | PML KA 8.1: "**Risk / opportunity** — Uncertain event affecting an objective, adversely / favourably." PFL KA 11.1 defines only *risk register (financing)*, adding "**and mechanism**" | None. PCL and PML are the same statement; PFL layers the financing requirement on top | **An uncertain event or condition that, if it occurs, affects an objective — adversely (threat) or favourably (opportunity).** | None. Consistent across all three books. |
| **issue** | PCL, PML | PCL D12 (line 70): "A risk that has already occurred." | PML KA 8.1: "A risk that has occurred; managed, not analysed." | None. PML adds the consequence; the definition is identical | **A risk that has occurred; it is managed, not analysed.** | None. |
| **coverage** | PML, PFL | PML KA 8.1: "Distinct risks identified as a share of the estimated population; an optimistic figure, since shared blind spots deflate the population and so inflate it." | PFL D2 (line 1193): "Can earnings or cash service the debt? — Interest cover, `DSCR`" — i.e. the debt-service coverage family. PFL KA 14.2 uses *contingency coverage on the remainder* | **None functionally** — the two senses never meet in one chapter. But no context flag exists in either book or in `TERMINOLOGY.md` | **No single definition — context flag required.** | **Genuine collision.** *Risk-identification sense:* the share of the estimated risk population actually identified. *Credit sense:* the ratio family (`DSCR`, `LLCR`, interest cover) measuring whether cash can service debt. Both are standard; neither may be renamed. |
| **value** | PCL, PML, PFL | **Never defined as a base term in any book.** Only compounds: *value envelope*, *value of information*, *value per unit of constraint* (PML), *value measurement* (PCL D13) | — | **Gap, and the largest one: 2,601 uses corpus-wide.** The word also carries the EVM sense inside `PV`/`EV` and the finance sense in *present value* | **No single definition — context flag required**, plus a base entry for the economic sense. | **Genuine collision across three professional senses.** *Economic/benefit sense:* the worth of an outcome to the organisation. *EVM sense:* a budgeted money amount (Planned Value, Earned Value) — a measure of work, not of worth. *Finance sense:* present/future value, a discounted quantity. Forcing one definition would be an error; the books must never let *earned value* be read as *value delivered*. |
| **benefit** | PML, PFL | **Never defined as a base term.** PML defines *benefits owner*, *benefits profile*, *benefits map*, *benefits bridge*, *benefits measurement plan* | `TERMINOLOGY.md` §2 defines "**Benefits realization**" | **Gap.** 890 uses corpus-wide. `FORMULAS.md` registers `EVA(benefit)` and notes it is "named in words to avoid EV clash" — the collision was foreseen at symbol level but not at word level | **A measurable improvement resulting from an outcome, owned by a named person outside the delivery team.** | Adjacent to the *value* collision: a benefit is a *realised* improvement; value is the worth attached to it. |

### 2.5 Project-finance terms (PFL-AI only)

These four are single-book terms and were checked for agreement with `TERMINOLOGY.md` §2 rather than
for cross-book collision.

| Term | Book(s) | First definition | Later definitions | Inconsistent usage found | Proposed suite definition | Collision note |
|---|---|---|---|---|---|---|
| **CFADS** | PFL | PFL KA 2.3: "Cash flow available for debt service — a *defined* term whose definition changes the ratio." | PFL KA 6.2 (*CFADS tie*) and KA 7.3 (*CFADS elasticity*); the facility's operative definition arrives in D10 | None. This is an **accepted multi-definition**, whitelisted in `make_glossary.py` with the reason "the progression is the teaching point" | **Cash flow available for debt service — a contractually defined term; the definition governs every ratio built on it.** | None. Best-practice handling of a deliberately progressive definition. |
| **bankability** | PFL | PFL KA 5.3: "The degree to which contracts, risks and cash flows support limited-recourse financing on acceptable terms." | PFL KA 1.2 (*bankability triangle*) | None. Matches `TERMINOLOGY.md` §2 word for word | As defined. | None. |
| **SPV** | PFL | PFL KA 1.1: "Ring-fenced single-purpose project company; the borrower and contract hub." | PFL KA 9.2 (*SPV vs HoldCo mezzanine*) | **Minor wording drift**: `TERMINOLOGY.md` §2 says "The ring-fenced legal entity created to own, finance and operate a project"; the book says "single-purpose project company; the borrower and contract hub". Same concept | **The ring-fenced, single-purpose legal entity created to own, finance and operate a project; the borrower and the hub of the contract structure** (merges both). | None. |
| **DSCR** | PFL | PFL KA 10.2: "`CFADS` ÷ debt service; the period test lenders covenant on." | PFL KA 10.2 (*DSCR ÷ LLCR*) | None. Matches `FORMULAS.md` §3 | As defined. | None. |
| **headroom** | PFL | PFL KA 10.2: "Cash that can be lost before a covenant threshold is crossed." | None | None | As defined. | None — though note it is a *distance to breach*, not a reserve. Worth a cross-reference to *contingency* so the two are not conflated. |

---

## 3. Terms checked and found clean

For the record, so a later reviewer does not re-audit them: **project, contingency, risk, issue,
accountability, responsibility, CFADS, bankability, SPV, DSCR, headroom** are used consistently
wherever they are defined. The accountability/responsibility pair (PML KA 1.2) is the sharpest
definitional work in the corpus and should be the model for the gaps in §4.

---

## 4. Issues found

**Ten issues: 6 defects, 2 drifts, 2 gaps.** Each has a file, a line and a proposed resolution.
The eight *legitimate* collisions catalogued in §2 are **not** listed here — they need context flags,
which Issue 9 provides, not resolution.

### Issue 1 — `EAC` carries two different formulas inside one book *(defect — most significant)*

**Files:**
- `docs/books/pfl-ai/manuscript/domain-04-investment-appraisal.md` **lines 589–593, 627, 642–643, 653, 671**
- against `docs/books/pfl-ai/manuscript/domain-08-cost-schedule-contingency.md` **line 1220**
- and `docs/books/registries/FORMULAS.md` **lines 18 and 57**

PFL D4 line 590 states `EAC = PV / AF(r, n)` — **equivalent annual cost**. PFL D8 line 1220 states
`CTC = EAC − AC` — **estimate at completion**. Same symbol, same book, two unrelated formulas.

`FORMULAS.md` §1 registers `EAC` **only** as Estimate at Completion (inherited from PCL-AI D6) and
§3 already provides **`EAV` — equivalent annual value** for precisely the D4 quantity. D4's key-terms
row at line 671 even writes the pair as "**`EAV` / EAC**", showing the author knew the two were the
same thing and kept both labels.

This is worse than the `PV` clash because `PV` at least has a written rule (Issue 2) and because the
two `EAC` formulas produce quantities in different units of meaning — a per-year cost against a
whole-project cost. A candidate meeting both in one examination has no way to disambiguate.

**Proposed resolution.** Use **`EAV`** throughout PFL D4 and delete `EAC` from that domain entirely,
including the line 671 key-terms row (make it "**`EAV`**" alone). Reserve `EAC` for Estimate at
Completion suite-wide. Then add a line to `FORMULAS.md` §1 under the notation-clash rule:
"`EAC` = Estimate at Completion **only**; the equivalent-annual quantity is `EAV`, never `EAC`."
Zero substantive content changes — every worked example's arithmetic stands.

### Issue 2 — the `PV` notation-clash rule is breached in both books *(defect)*

**Files:**
- `docs/books/registries/FORMULAS.md` **lines 26–28** (the rule)
- `docs/books/pfl-ai/manuscript/` — bare `PV` for present value **93 times across 12 domains**
  (domain-03: 32, domain-04: 27, domain-01: 6, domain-08: 6, domain-09: 5, domain-11: 4,
  domain-15: 4, domain-10: 3, domain-06: 2, domain-12: 2, domain-13: 1, domain-14: 1)
- `docs/books/pml-ai/manuscript/domain-02-strategy-selection.md` — bare `PV` for present value
  **33 times** (e.g. lines 461–462, 487–488, 546, 865, 948)
- against `docs/books/pml-ai/manuscript/domain-07-cost-resources-commercial.md` (44),
  `domain-04-integration-delivery-architecture.md` (31) and
  `domain-15-programmes-portfolios-enterprise.md` (5) — bare `PV` for **Planned Value**

The rule says discounting "always writes 'present value' in words or `PV(x)`" and that PFL "reserve[s]
bare `PV` for EVM material only". PFL uses `PV(` only 40 times against 93 bare uses. **PML-AI is the
acute case**: `PV` means present value in D2 and Planned Value in D4, D7 and D15 — and D4 line 673
writes "`PV`, Domain 7's planned value" barely two hundred lines from D2's discounting `PV`.

**Proposed resolution.** Two options, and the second is better:

1. Enforce the rule as written — rewrite all 126 bare present-value uses to `PV(x)`.
2. **Recommended:** amend the rule to what the corpus actually needs, then enforce *that*. Bare `PV`
   is standard notation in both professions and the fight is not worth winning globally. Require
   instead that **any domain using `PV` in the non-native sense declares it in the domain's opening
   conventions note** — so PML D2 says "in this domain `PV` denotes present value; Domain 7's Planned
   Value is written `PV(EVM)` where the two could meet". Then add `PV` to the
   `_build/verify_formulas.py` checks so a domain using both senses without a declaration fails.

Either way, `FORMULAS.md` lines 26–28 must stop describing a state of affairs that is not true.

### Issue 3 — `baseline` is redefined against an inherited binding definition *(defect)*

**Files:**
- `docs/bok/00-style-spine.md` **line 63** (binding): "The approved, version-controlled plan (scope,
  schedule or cost) against which performance is measured."
- `docs/books/registries/TERMINOLOGY.md` **§1 line 9**, which lists *Baseline* among terms inherited
  **unchanged**
- `docs/books/pml-ai/manuscript/domain-02-strategy-selection.md` **line 1256**: "The measured
  pre-change position; must be measured before, not reconstructed after."
- `docs/books/pml-ai/GLOSSARY.md` **line 103**, where the D2 form is canonical

`TERMINOLOGY.md` asserts the term is inherited unchanged. It is not: PML's canonical glossary entry
is the benefits-measurement sense. PML then uses the *plan* sense in D4 (*baseline drift*,
*baseline maintenance*, lines 105–107 of the glossary), so both live in one book with no flag.

Both meanings are correct professional usage — this is a real collision, not an error of fact. What
is wrong is the registry's claim that there is only one.

**Proposed resolution.** Rename the D2 term to **"benefits baseline"** in
`domain-02-strategy-selection.md` line 1256 and at its uses, keeping the definition text unchanged.
That leaves *baseline* alone meaning the style-spine's approved plan, everywhere, and costs one word.
Then record *benefits baseline* in `TERMINOLOGY.md` §2 as a PML-proposed term.

### Issue 4 — `control account` is narrowed against its inherited definition *(drift)*

**Files:**
- `docs/bok/00-style-spine.md` **line 64** (binding, inherited unchanged per `TERMINOLOGY.md` §1)
- `docs/books/pml-ai/manuscript/domain-04-integration-delivery-architecture.md` → glossary line 282:
  "The level at which performance is measured and reported (Domain 7's earned value attaches here)."

The style spine defines a control account as **the intersection of a WBS element and an OBS
element** — the property that makes it a point of *control* (a named owner) rather than merely a
level of *reporting*. PML's gloss drops the OBS half.

**Proposed resolution.** Restore the intersection in the PML KA 4.2 key-terms row: "The WBS×OBS
intersection at which scope, budget, cost and schedule integrate and performance is measured —
Domain 7's earned value attaches here." Regenerate the glossary.

### Issue 5 — `management reserve` is controlled by different roles in different books *(drift)*

**Files:**
- `docs/bok/domain-03-budgeting-forecasting.md` **line 129** and
  `docs/bok/domain-12-risk-management.md` **line 405**: "**management-controlled**"
- `docs/books/pml-ai/manuscript/domain-07-cost-resources-commercial.md` **line 334**:
  "**sponsor-controlled**"
- `docs/books/pml-ai/manuscript/domain-08-risk-uncertainty-resilience.md` **line 1052**:
  "Sponsor / change authority, via change control"

Both are defensible; organisations differ. But a candidate reading both books gets two answers to a
question an examination could ask, and PML is internally looser than PCL is.

**Proposed resolution.** Adopt PML D8's formulation, which is the most accurate and covers both:
"outside the baseline; unidentified risk and scope change; released by the **sponsor or change
authority named in the governance plan**, via change control." Apply to all four locations. This
also removes the implication that any one role is universally correct.

### Issue 6 — the suite principle appears in three wordings *(defect)*

**Files:**
- `docs/books/laws/PCI_STANDARDS_DRAFTING_MANUAL.md` **§10** and `docs/books/laws/PCI_STANDARDS_CHARTER.md`
  **§11** — *Governing principle*, both carrying the approved formulation verbatim. (The rule was first
  stated in the superseded `SUPERSEDED_LAW_SYSTEM_v0.md` §8: "One approved formulation, everywhere, in
  all three books and every law publication.")
- `docs/books/pfl-ai/manuscript/domain-01-foundations.md` **line 1106**: "AI proposes; the
  professional verifies, decides**,** remains accountable." — the "and" is missing
- `docs/bok/domain-13-ai-for-project-controls.md` **line 1241**: "AI proposes; the professional
  **verifies and owns**." — a different sentence entirely, and it is an MCQ rationale, where
  precision matters most

The approved form appears correctly **77 times**. These are the only two deviations.

**Proposed resolution.** Replace both with the approved wording: "AI proposes; the professional
verifies, decides and remains accountable." Then add a corpus check to `_build/run_checks.py`
matching `AI proposes` and failing on any string that is not the approved formulation — this is
exactly the kind of rule a build gate should hold, and it is currently held by nothing.

### Issue 7 — British English is not maintained in PFL-AI *(defect)*

**Files:** `docs/books/pfl-ai/manuscript/` — **85 uses of American "judgment" against 9 of British
"judgement"**, across at least 11 of the 16 domains.

The rest of the corpus is consistent: PCL-AI 117 British / 1 American; PML-AI 107 / 29; the PCI
Standard files 22 / 0. `TERMINOLOGY.md` §1 requires "British English throughout", and the editorial charter is
suite-wide.

The mixing occurs **inside a single file**: `domain-05-development-bankability.md` uses "professional
judgment" at line 690 and "professional judgement" elsewhere in the same domain.

**Proposed resolution.** Replace "judgment" with "judgement" throughout `pfl-ai/manuscript/` and the
29 instances in `pml-ai/manuscript/`, with one exception preserved: **"judgment" is correct British
English for a court's decision**, so check each occurrence in legal-forum passages (the delay-analysis
material of `PCI-PCL-STD-10.02` *Critical-Path Verification Before Reliance* and PFL D12 contract
chapters) before replacing. Add a spelling check to
`_build/run_checks.py`.

### Issue 8 — `TERMINOLOGY.md` has been corrupted by a global rename *(defect)*

**File:** `docs/books/registries/TERMINOLOGY.md` — **lines 19, 40 and 43** (line numbers as at
2026-08-03; this file is being actively edited, so locate by text rather than by number)

A find-and-replace of the retired credential name across the repository has overwritten text where
the *old* name was the point of the sentence, leaving three statements that are now self-referential
or self-contradicting:

- **Line 19** defines the Responsible AI principle as "the suite-wide restatement of PCL-AI's *'AI
  proposes; the professional verifies, decides and remains accountable'*" — the sentence exists to
  contrast the new wording with the retired one, and now quotes the new wording as the thing being
  restated. It is circular.
- **Line 40** reads "**PCL-AI** — PCI AI Project Controls Leader (the previous book's credential,
  **renamed from PCL-AI**)" — renamed from itself.
- **Line 43** reads "Retired names (**PCL-AI**, PDL-AI, CPMD, PFIP) never appear in new content" —
  listing the suite's *current* credential as retired, which, read literally, forbids the name the
  rest of the registry mandates.

Line 43 is the dangerous one: it is a rule, and as written it is unfollowable.

The retired name is **PCP-AI** (137 occurrences survive in `docs/PCI_DEVELOPER_GUIDE.md`,
`docs/publications/` and `docs/lectures/`), and the retired principle wording is "AI proposes, the
professional disposes" (still present in `docs/lectures/domain-01/`). Both are recoverable.

**Proposed resolution.** Restore **PCP-AI** at all three sites: line 19 "…the suite-wide restatement
of PCP-AI's *'AI proposes, the professional disposes'*"; line 40 "(the previous book's credential,
renamed from PCP-AI)"; line 43 "Retired names (PCP-AI, PDL-AI, CPMD, PFIP)". Then exclude these
three sites from future bulk renames — a registry that records naming *history* is precisely the file
a naming find-and-replace must not touch.

**Checked and found sound:** `SOURCES.md` took the same rename at lines 4, 14, 18 and 38 ("the
PCL-AI convention", "seeded from PCL-AI usage", "Active (PCL-AI precedent)", "(PCL-AI pattern)").
Those are **correct** as rewritten — they refer to the previous book, which genuinely now carries
that name, and the old name was not load-bearing in them. Only the three sites above, where the
retired name was the point of the sentence, are broken.

### Issue 9 — eight genuine collisions have no context flags *(gap)*

**Files:** `docs/books/registries/TERMINOLOGY.md` §2, which flags **only** *sponsor*.

§2 of this audit identifies eight terms with legitimately different professional meanings:
**sponsor · value · baseline · PV · EAC · coverage · verification · governance**. Only *sponsor*
carries a context flag in the shared registry. `make_glossary.py` recognises the pattern — it treats
"Context flag:" in a definition as closing a collision finding — but the flags were never written.

**Proposed resolution.** Add a **"§4 Declared collisions"** section to `TERMINOLOGY.md` holding all
eight, each stating both senses and the disambiguation convention, drawn from the Collision note
column above. Then add "Context flag: …" to the corresponding key-terms rows in the manuscripts so
the in-book checker sees them and future editors cannot silently collapse one sense into the other.

### Issue 10 — high-traffic terms have no base definition *(gap)*

**Files:** all three books' glossaries and `docs/bok/appendices.md`.

Five terms the corpus leans on constantly are defined only as compounds:

| Term | Uses corpus-wide | Defined as | Base definition |
|---|---|---|---|
| **value** | 2,601 | *value envelope*, *value of information*, *value per unit of constraint*, *value measurement* | **none** |
| **forecast** | 1,173 | *forecast honesty* (the subject of `PCI-PCL-STD-03.04`), EAC methods | **none** |
| **benefit** | 890 | *benefits owner/profile/map/bridge/measurement plan*, *benefits realization* | **none** |
| **assurance** | 162 | *assurance map*, *assurance capture* | **none** |
| **governance** | 588 | PML KA 3.1 defines it; **PCL defines only "governance / lineage"** (data sense) | partial |

Also in this class: PML defines **programme** and **portfolio** twice each (KA 1.1 and KA 15.1 —
see §2.1), and neither form reaches `TERMINOLOGY.md`; and `TERMINOLOGY.md` §2 records **none** of
*project*, *programme*, *portfolio*, *governance*, *accountability* or *responsibility*, though it
governs terminology for all three books.

**Proposed resolution.** Add base entries using the Proposed suite definition column of §2, placing
each in the KA that already owns the concept — *value* and *benefit* in PML KA 2.1, *forecast* in
PCL D3 with a PML KA 7.3 cross-reference, *assurance* in PML KA 3.3, *governance* in PML KA 3.1 with
a PCL Appendix B pointer, and the structural terms in PML KA 1.1. Resolve the *programme*/*portfolio*
duplication by keeping the KA 15.1 forms and converting the KA 1.1 rows to cross-references, which is
the corpus's own documented "cite rather than re-derive" rule. Then promote all of them into
`TERMINOLOGY.md` §2, which is where the suite's shared vocabulary is supposed to live.

---

*Audit compiled 2026-08-03. **28 terms registered** across 5 sections; **10 issues found** —
6 defects, 2 drifts, 2 gaps; **8 legitimate collisions** catalogued and deliberately preserved.
The most significant issue is **Issue 1**: the symbol `EAC` carries two unrelated formulas inside
PFL-AI — `EAC = PV / AF(r, n)` (equivalent annual cost, D4) and `EAC = AC + ETC` (estimate at
completion, D8) — where the registry provides `EAV` for the first and reserves `EAC` for the second.
British English throughout.*
