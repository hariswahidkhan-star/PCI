---
id: CER-08
series: S05
series_name: Certification Handbook
title: Recertification, CPD and the AI-currency requirement
subtitle: The three-year cycle, what counts towards it, and the component you cannot substitute your way out of
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager, employer]
level: professional
reading_time_min: 12
summary: >
  A PCI credential is valid for three years and is maintained by continuing professional development
  recorded in your PCI account, in four categories, with a mandatory AI-currency component that no other
  activity can substitute for. Only approved entries dated inside the current cycle count. This document
  explains how the cycle window is calculated, what evidence a CPD entry needs, why the widely quoted
  30-hour figure is a portal target rather than a published requirement, and what recertification and
  lapse mean for a credential's verified status.
linkedin:
  format: article
  hook: >
    Our recertification cycle carries a mandatory AI-currency component: staying current with what AI can
    and cannot do in your discipline is not optional, and no other activity substitutes for it.
  tags: [CPD, Certification, ResponsibleAI, ProjectControls]
  asset: checklist-pdf
gated: false
related: [CER-01, CER-07, AIG-12, ETH-01, CMP-08]
sources:
  - "PCI platform CPD, credential-expiry and recertification logic (backend/Core/CpdPolicy.cs; schema.sql), verified August 2026"
  - "PCI Candidate Handbook (docs/downloads/candidate-handbook.md), 2026"
  - "PCI Publication Framework — CANONICAL-FACTS.md §§4.2, 4.5, August 2026"
placeholders: 3
---

# Recertification, CPD and the AI-currency requirement

> Three years, four categories, and one component the Institute will not let you trade away.

**In one paragraph.** A PCI credential is valid for three years and is maintained by continuing
professional development recorded in your PCI account, in four categories, with a mandatory AI-currency
component that no other activity can substitute for. Only approved entries dated inside the current cycle
count. This document explains how the cycle window is calculated, what evidence a CPD entry needs, why the
widely quoted 30-hour figure is a portal target rather than a published requirement, and what
recertification and lapse mean for a credential's verified status.

**Who this is for.** Credential holders planning a cycle; managers who need to know what maintaining a
certified team actually costs in time; and employers verifying that a credential they rely on is still
active.

---

## 1. Why the credential expires at all

A credential that never expires certifies what someone knew once. That is a defensible claim about the
past and a misleading one about the present, and the gap between them widens fastest in exactly the areas
this credential covers.

The Institute's position is specific rather than general. Project controls practice moves at the ordinary
pace of a professional discipline; **the AI component of it does not**. A holder certified three years ago
was assessed against tools, failure modes and governance expectations that have since changed materially.
Recertification exists to close that gap, and the AI-currency component exists because closing it is the
part a busy professional is most likely to defer.

---

## 2. The cycle

### 2.1 Three years, ending at expiry

A credential is valid for **three years**. The **current cycle** is the three-year period ending on the
credential's expiry date. Everything you record for recertification must be dated inside that window.

The window is calculated backwards from expiry, not forwards from issue. In the ordinary case the two are
the same thing; after a reinstatement or an adjusted expiry date they are not, and the platform's
calculation — expiry minus the certification's cycle length — governs.

### 2.2 Only approved entries count

A CPD entry you create is **recorded**, which means submitted and awaiting review. It counts towards
recertification only once it is **approved**. Entries that are recorded but not yet reviewed, and entries
that have been rejected, contribute nothing.

This is the single most common surprise at the end of a cycle. A holder with a full log and an empty
approved total is not compliant, and the fix — evidence, review, approval — takes longer in month
thirty-five than in month two.

### 2.3 Where it is recorded

CPD is recorded in your PCI account, with a running total shown against the current cycle. Each entry
carries an activity date, a category, hours, a description and, where relevant, an evidence file. Credit
earned automatically from an attended PCI event is granted **once** per attendee: a retry, a crash or a
duplicate scan cannot produce a second credit for the same event.

---

## 3. The four categories

CPD is organised into four categories of activity:

| Category | What it covers |
|---|---|
| **Structured learning** | Taught or curriculum-based activity: courses, programmes, workshops, structured webinars |
| **Learning through work** | Capability genuinely developed in practice — a first quantitative schedule risk analysis, a first claim narrative, a system implementation |
| **Contribution to the profession** | Giving back: mentoring, examining, writing, standards and committee work, speaking |
| **Formal study** | Academic or formal qualification study undertaken during the cycle |

Two notes on how to use them. **Categorise by what you did, not by what it was called** — a conference is
structured learning if you attended sessions and contribution to the profession if you presented, and
recording it as both is double counting. And **a category is not a quota**: nothing requires an even
spread across four categories, and a cycle weighted towards learning through work is a legitimate cycle
for a practitioner in a demanding role.

---

## 4. The AI-currency requirement

### 4.1 What it is

The three-year cycle carries a **mandatory AI-currency component**. In your account it appears as a CPD
category named exactly that — **AI currency** — and hours recorded under it are counted separately from
your total, because the requirement is a floor in its own right and not a share of something else.

### 4.2 What it is not

It is not a general technology requirement, and it is not satisfied by using AI tools. Using a tool is
not currency in it. The component asks for evidence that you have **kept up with what AI can and cannot
safely do in your discipline**, which is a different activity from adopting it.

### 4.3 What genuinely satisfies it

*Recommended practice, not an approved list.* Activity that would satisfy a reviewer typically shows one
of three things:

1. **Capability boundaries** — structured learning about where a class of model fails, how its outputs
   degrade with poor data, and what it cannot be relied upon to do in cost, schedule, risk or finance
   work.
2. **Validation practice** — work in which you took an AI-assisted output and established, with a method
   you could describe to an auditor, whether it could be relied upon. Challenging an AI-generated forecast
   and documenting what you validated is stronger evidence than completing a course about forecasting.
3. **Governance contribution** — building, reviewing or applying controls over AI use in a project or an
   organisation: model registers, human-review gates, disclosure requirements, data-handling rules.

The governing principle is the same one the credential was awarded against: **AI proposes; the
professional disposes.** The AI-currency component asks you to demonstrate that you are still competent to
be the one who disposes.

### 4.4 It cannot be substituted

Excess hours in another category do not satisfy the AI-currency component. A cycle that is otherwise
complete but empty in this category is an incomplete cycle. The Institute treats this as the load-bearing
part of maintenance, because it is the part that makes the credential mean something current rather than
something dated.

---

## 5. How many hours

**No binding hours requirement is published today, and this document will not invent one.**

Three statements, all true and often conflated:

1. The student portal displays a **target of 30 hours per three-year cycle**. It is a target the portal
   shows. **It is not a published requirement and no holder can be held to it.**
2. The platform's required-hours setting for the certification currently **defaults to zero**, which is
   the honest configuration for a requirement that has not yet been published.
3. The binding requirement **will be published with the recertification rules**.

`[CONFIRM: the binding CPD hours requirement per three-year cycle — 30 hours is a student-portal target,
not a published requirement]`

`[CONFIRM: the mandatory AI-currency hours required within a cycle, as a subset of the total]`

**What to do in the meantime.** Record everything, dated and evidenced, from the first month of your
cycle. A holder with a complete, evidenced, approved log is compliant with any requirement that is
subsequently published at or below their total. A holder who waited for the number is not, and cannot
retrospectively acquire hours they did not do.

---

## 6. Worked example — which activities fall inside the cycle

*Illustrative figures. The dates and activities are invented to show how the cycle window and the approval
rule interact.*

**The facts.** A credential expires on **12 June 2028**. The cycle length is three years, so the current
cycle runs from **12 June 2025** to **12 June 2028**. The holder's log contains:

| Activity | Date | Category | Hours | Status | Counts? |
|---|---|---|---|---|---|
| Cost engineering short course | 3 May 2025 | Structured learning | 12.0 | Approved | **No** — dated before the cycle start |
| First quantitative schedule risk analysis | 14 Nov 2025 | Learning through work | 8.0 | Approved | Yes |
| Model-limitations workshop | 22 Feb 2026 | AI currency | 6.0 | Approved | Yes |
| Conference attendance | 9 Sep 2026 | Structured learning | 7.0 | Recorded | **No** — not yet approved |
| Mentoring two junior planners | 2027, ongoing | Contribution | 10.0 | Approved | Yes |
| Validating an AI-assisted cost forecast | 18 Jan 2028 | AI currency | 4.0 | Approved | Yes |

**The arithmetic.**

Approved hours inside the cycle = 8.0 + 6.0 + 10.0 + 4.0 = **28.0 hours**.
Of which AI currency = 6.0 + 4.0 = **10.0 hours**.

Excluded: 12.0 hours dated before the cycle start, and 7.0 hours recorded but not approved — **19.0 hours
of genuine activity that contributes nothing**, purely on dates and status.

**The assumptions the answer depends on.** That the cycle start is expiry minus three years; that every
"approved" entry above has in fact been approved rather than merely submitted; and that no entry is
double-counted across categories. Whether 28.0 hours is sufficient cannot be stated here, because the
binding requirement is not yet published (§5) — which is exactly why the conference entry sitting
unapproved for eighteen months is a risk rather than a rounding detail.

---

## 7. Evidence and audit

A CPD entry stands on four things: **a date**, **a category**, **hours you could defend**, and **a
description specific enough that a reviewer can tell what you actually did**. Attach evidence where you
have it — a certificate, an agenda, a deliverable, a confirmation.

CPD is subject to review, and evidence may be requested. Write each entry as though it will be the one
that is audited: "AI workshop, 6 hours" is a claim; "Six-hour workshop on failure modes of time-series
forecasting models in cost applications, certificate attached, applied to the Q3 forecast review" is a
record.

---

## 8. Recertifying, and what happens if you lapse

### 8.1 Recertifying

Recertification is completed within the cycle, on the basis of approved CPD in that cycle. It is a
maintenance decision, not a fresh assessment: it does not require you to re-sit the examination. A renewal
fee applies — seeded in the platform at **USD 99**, and confirmed at checkout, which is the amount that
binds you.

### 8.2 Lapse

Letting a cycle end without recertifying **suspends the credential's active status** until the
recertification requirements are met. This is not cosmetic: public verification is expiry-aware, so an
employer or client checking your credential identifier sees the change (`CER-07` §5.3).

`[CONFIRM: the grace period after expiry, if any, and the published requirements for reinstating a lapsed
credential]`

### 8.3 What a lapse does not do

It does not erase the fact that you passed the examination, and it does not restart your eligibility.
Recertification is about currency, not about re-earning entry.

---

## 9. How this goes wrong

- **Logging activity but never getting it approved.** Recorded is not approved, and only approved counts.
  §2.2.
- **Recording activity from the wrong side of the cycle boundary.** Good work, dated three weeks early,
  counts for nothing. §6.
- **Treating 30 hours as the rule.** It is a portal target. Record what you actually do and evidence it.
  §5.
- **Trying to substitute for AI currency.** Nothing else satisfies it. §4.4.
- **Recording tool use as AI currency.** Using a tool is not currency in it. §4.2.
- **Double counting one activity across two categories.** §3.
- **Discovering the requirement in month thirty-four.** The failure mode this whole document exists to
  prevent: a compliant cycle is built from month one, in twenty-minute entries, not assembled in a
  fortnight from memory.
- **Assuming a lapsed credential still verifies as active.** It does not, and the people who check are the
  ones whose opinion matters. §8.2.

---

## 10. The cycle checklist

Set this up in the week you are certified, not in the year you expire.

- [ ] Expiry date recorded in your own calendar, with a reminder at 24 months and at 30 months
- [ ] Cycle start calculated (expiry minus three years) and written down
- [ ] A single place to keep evidence as it arrives, not reconstructed later
- [ ] CPD entered within a month of each activity, with date, category, hours, specific description and
      evidence attached
- [ ] Approval status checked quarterly — anything still "recorded" chased
- [ ] At least one **AI-currency** activity planned in each year of the cycle, not saved for the last
- [ ] A note of which activities were AI currency, kept separately from the total
- [ ] Recertification started well before expiry, not in the final month

---

## Related

- `CER-01 — Certification handbook — master` — the whole journey, of which this is the standing obligation
- `CER-07 — Results, scoring, appeals and complaints` — how credential status and verification work
- `AIG-12 — The AI-literate controls professional` — what "keeping current" actually consists of
- `ETH-01 — The PCI code of ethics and professional conduct` — the accountability that maintenance supports
- `CMP-08 — Data, digital and AI competencies in depth` — the competencies the AI-currency component keeps alive

## Sources and standards

- PCI platform CPD and recertification logic, verified August 2026 — the cycle-window calculation, the
  approved-entries-only rule, the separately counted AI-currency category, the exactly-once event credit,
  and the required-hours setting that currently defaults to zero.
- PCI Candidate Handbook (`docs/downloads/candidate-handbook.md`), 2026.
- PCI Publication Framework, `CANONICAL-FACTS.md` §§4.2 and 4.5, August 2026.

## Status and version

> Founding-stage document · Version 1.0 — effective date to be confirmed · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
