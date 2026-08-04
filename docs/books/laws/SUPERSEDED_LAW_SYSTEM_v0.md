# The PCI Standards System

**Status: SUPERSEDED — retained for history only. Do not draft to this document.**

This was the first drafting specification for the PCI Standards. It has been replaced by two
documents, and where this file and either of those disagree, **those govern**:

- [`PCI_STANDARDS_CHARTER.md`](PCI_STANDARDS_CHARTER.md) — status, hierarchy,
  priority, due process, interpretation, amendment, exceptions, consequences.
- [`PCI_STANDARDS_DRAFTING_MANUAL.md`](PCI_STANDARDS_DRAFTING_MANUAL.md) — normative language, the
  twenty-five-element law structure, identifiers, external-reference classification, prohibited
  patterns, the visual system, and the twenty-five audit questions.

**The specific contradiction that made supersession necessary:** §3 below permits `must`, `must not`,
`shall` and `shall not` interchangeably for mandatory rules. PCI has since adopted **modern
must-drafting exclusively**, and `shall` is now barred from every law in every field. §3 below is
therefore wrong and must not be followed. The same section treats `should` as a recommendation while
requiring a recorded reason for departure, which is not a recommendation at all — the Drafting Manual
§1.3 splits that into a recommendation plus a separate recording requirement.

The eighteen-field template in §4 below is likewise superseded by the twenty-five-element structure,
which adds — among others — the compliance test, breach indicators, defined terms, independence,
threshold and exception elements that this version lacked.

---

## 1. What a PCI Standard is — and is not

> **PCI Standard** means a mandatory professional rule established by Project Controls
> Institute Global for the ethical, competent, verifiable and accountable performance of work within
> a PCI certification scope.

**Legal-status disclaimer (must appear wherever laws are published):**

> PCI Standards are professional certification rules and standards of conduct established by
> PCI Global. They are not legislation, regulatory requirements or substitutes for applicable law,
> contractual obligations or authoritative professional standards. Where any applicable law,
> regulation, contract or authoritative professional standard imposes a stricter requirement, that
> requirement governs.

Laws are never presented as enacted by any government. They bind candidates and credential holders
within PCI's own authority: examination scope, certification conditions, and PCI's quality and
conduct processes.

## 2. Hierarchy

1. **PCI Foundational Standards** — apply to all three credentials. IDs `PCI-FND-STD-NN` (`PCI-FND-STD-01`
   to `PCI-FND-STD-15`). The superseded form `PCI-LAW-F-NN` is withdrawn and is recorded only in
   [`STANDARDS_CONCORDANCE.md`](STANDARDS_CONCORDANCE.md).
2. **Certification Standards** — apply to one credential. IDs `PCI-PCL-STD-DD.NN`, `PCI-PFL-STD-DD.NN`,
   `PCI-PML-STD-DD.NN`, where `DD` is the two-digit domain of primary anchorage and `NN` a two-digit
   sequence within that domain. A certification law that spans domains anchors to the domain that
   teaches it and lists the others under *Related book content*.
3. **Process Rules** — operational requirements subordinate to a law (numbered `…-R1`, `…-R2` under
   their parent law).
4. **Practice Guidance** — recommended, not mandatory. Never labelled "law".
5. **Examples and commentary** — illustrative only. Never normative.

Distinguish in text, every time: **Mandatory PCI Law** · **Mandatory PCI Rule** · **Recommended PCI
Practice** · **External requirement** · **Illustrative example** · **Jurisdictional requirement**.

## 3. Normative language

- `must` / `must not` / `shall` / `shall not` — only for mandatory professional rules PCI intends
  to hold candidates and credential holders to.
- `should` — recommended practice; departures need a recorded reason.
- `may` — discretionary.
- Every law's Rule section is a direct mandatory statement. No hedging inside a Rule; qualifications
  belong in Scope or Jurisdictional caution.

## 4. The mandatory drafting format

Every law uses exactly this structure, in this order:

```
### PCI STANDARD <ID> — <Official title>

**Rule.** <direct mandatory statement>

**Purpose.** <why the law exists>

**Scope.** <who and what it applies to>

**Minimum professional requirement.** <minimum actions necessary for compliance>

**Required evidence.** <documents, data, calculations, approvals, records that prove compliance>

**Decision owner.** <the role accountable for the decision>

**Independent review.** <when independent review is required>

**Escalation trigger.** <the event or threshold requiring escalation>

**Prohibited practice.** <specific conduct that violates the law>

**AI application.** <what AI may assist with>

**AI restriction.** <what AI must not decide, approve or certify>

**Verification requirement.** <how machine-assisted and human-produced work must be checked>

**External references.** <applicable IFRS, IAS, ISO, IEC, FIDIC, AACE, PMI, OECD, IFC, Equator
Principles or other verified authorities — named and characterised, never reproduced. State that
the official publication governs.>

**Jurisdictional caution.** <what requires local legal, tax, regulatory or accounting advice>

**Related PCI laws.** <IDs>

**Related book content.** <Domain / Knowledge Area / topic references in the book's numbering>

**Examination relevance.** <how the law may be assessed — scenario-based; no live examination
content is exposed>

**Consequences of breach.** <professional, examination, certification, quality or escalation
consequences within PCI's authority only>
```

No field may be omitted. Where a field is genuinely inapplicable, write "None." and say why in one
clause. Official titles are concise and memorable.

## 5. External references inside laws

- Name the authority and instrument (e.g. "IFRS 15 *Revenue from Contracts with Customers*,
  IFRS Foundation"); characterise its relevance in PCI's own words; never reproduce its text.
- Never invent clause numbers, editions, or requirements. If the precise provision is not verified,
  cite the instrument by name only.
- Tag each reference per the External-Reference Register categories (see
  `../registries/EXTERNAL_AUTHORITIES.md` and each book's `STANDARDS.md`): authoritative accounting
  standard · international standard · **national standard** · contract framework · professional
  guidance · voluntary framework · industry practice · **supervisory guidance** · illustrative
  reference. A voluntary framework is never described as legislation. Where currency matters, add
  "verify current requirements".
- Two categories exist because the earlier vocabulary forced awkward workarounds. A **national
  standard** (e.g. an ANSI-accredited US standard) is a real published standard that binds only where
  a contract or procurement regime imports it — it is not an international standard and not industry
  practice. **Supervisory guidance** (e.g. a banking supervisor's model-risk expectations, or an
  internationally agreed supervisory framework) has no legal force of its own and applies only as a
  national authority transposes it or a supervised firm is subject to it; it must never be tagged as
  regulation.
- Some authoritative material is **not a standard even though its publisher issues standards**. The
  IFRS *Conceptual Framework* is the case that matters here: the IASB states expressly that it is not
  a Standard and that nothing in it overrides any Standard. Never source a requirement to it, and
  never tag it as an authoritative accounting standard.

## 6. Visual system (applies at typesetting)

Colour is never the only distinction: every box carries a written label, an icon, a border, and a
number, and must survive grayscale print and screen reading.

| Box | Label | Colour (heading/border) | Tint | Icon |
|---|---|---|---|---|
| PCI Law | `PCI STANDARD <ID>` | PCI Law Red `#C62828` (dark variant `#9B1C1C`) | `#FDECEC` | § |
| External standards reference | `EXTERNAL REFERENCE` | Standards Blue `#1D4ED8` | `#EEF4FF` | ⬢ |
| Guidance (non-mandatory) | `PCI PRACTICE GUIDANCE` | Guidance Teal `#0F766E` | `#ECFDF5` | ✦ |
| Jurisdictional / legal / tax caution, high-stakes AI limitation | `CAUTION` | Amber `#B45309` | `#FEF3C7` | ⚠ |

Body copy inside boxes is black on the light tint; the colour carries the heading, left border,
label and index marker only. Large blocks of body text are never set in bright red.

## 7. Stable identifiers

Laws, formulas, figures, tables and worked examples cite by stable ID (e.g. `PCL-LAW-06-03`,
`PFL-WE-10-04`), never by page number, because pagination changes. Page numbers may appear in
indexes only.

## 8. Suite principle

One approved formulation, everywhere, in all three books and every law publication:

> **AI proposes; the professional verifies, decides and remains accountable.**

The legacy wording "AI proposes, the professional disposes" is retired; it may appear only in
edition-history notes marked as the former wording.
