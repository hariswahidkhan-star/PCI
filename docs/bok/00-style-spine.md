# Batch 0 — Style Spine (the shared authoring standard)

> **Status:** foundation document. Every domain of the PCP-AI Body of Knowledge (BoK) is authored against
> this spine so that terminology, notation, worked-example format, figures and MCQs stay consistent across
> the whole ~1,500-page volume. Author nothing before reading this; restate any symbol you use.

This spine fixes the seven things that make parallel/serial authoring cohere instead of drifting into
"thirteen different books": (1) terminology & seed glossary, (2) the master formula-symbol table,
(3) worked-example format, (4) figure & animation spec format, (5) MCQ format, (6) citation rules, and
(7) language & currency conventions. It closes with the numbering scheme and the per–Knowledge-Area
checklist.

---

## 1. Scope, weighting & page budget (v1)

The BoK is weighted **40 % finance / 40 % project management / 20 % AI**, with an "AI in this domain"
section additionally embedded in every non-AI chapter. The full volume targets **~1,500 pages** of genuine
content — breadth of real material, never padding.

| Domain group | Share | ~Pages (of ~1,500) |
|---|---|---|
| Finance, accounting & reporting (Domains 1–4) | 40 % | ~600 |
| Project management — lifecycle, agile, scheduling, cost/EVM, contracts, risk, process cycles (Domains 5–12) | 40 % | ~600 |
| AI knowledge & practical approach (Domain 13) | 20 % | ~300 |

Per-section budgets in the outline may flex ±15 % as content genuinely demands, provided the 40/40/20 split
and the ~1,500-page target hold.

> **Relationship to the live credential framework.** This BoK is authored as a **standalone study
> reference**. It is deliberately *not* reconciled line-by-line with the eight-domain competency framework
> published on the Institute's website or the Certuvo practice domains; those serve exam-blueprint and
> self-assessment purposes. Keeping them separate lets this reference go far deeper (e.g. full IFRS 15 and
> AgileEVM treatments) without being constrained to the exam's coarser groupings.

---

## 2. Three-level structure (use everywhere)

- **DOMAIN** — the major pillar (e.g. *Foundations of Accounting for Project Controls*).
- **KNOWLEDGE AREA (KA)** — a coherent body within a domain (e.g. *The accounting model*).
- **TOPIC** — the atomic, teachable/testable unit (e.g. *Debit and credit rules by account type*).

Every page sits under Domain → KA → Topic and is **numbered** `Domain.KA.Topic` (e.g. `1.1.2`) for
cross-referencing and exam-blueprint mapping. Cross-reference by number ("see 6.3.1"), never by repeating
content.

---

## 3. Seed glossary (extend, never redefine)

Terms are defined **once** here and used identically everywhere. Where a domain introduces a new term, it
is added to this glossary in the consolidation pass; it must not be silently redefined in a later chapter.

| Term | Definition (as used throughout the BoK) |
|---|---|
| **Accrual basis** | Recognising the effects of transactions when they occur, not when cash moves. |
| **Baseline** | The approved, version-controlled plan (scope, schedule or cost) against which performance is measured. |
| **Control account (CA)** | A management-control point where scope, budget, actual cost and schedule integrate — the intersection of a WBS element and an organisational (OBS) element. |
| **Cost breakdown structure (CBS)** | The hierarchical decomposition of a project's cost by cost element/type. |
| **Provision** | A liability of uncertain timing or amount, recognised under IAS 37 when the recognition tests are met. |
| **Recognition** | Recording an item in the financial statements (as an asset, liability, income or expense). |
| **Work breakdown structure (WBS)** | The hierarchical decomposition of the total scope of work into deliverables and work packages. |

*(The consolidated global glossary in the appendices is assembled from every domain's key-terms boxes.)*

---

## 4. Master formula-symbol table (defined once; restate on use)

Every symbol below means the same thing in every chapter. When a chapter uses a symbol, it restates the
definition inline, but never changes it. Finance/EVM symbols are seeded here even though they are first
used deeply in Domains 3–6, so the classical and agile treatments (Domain 9) share one notation.

| Symbol | Meaning | Unit |
|---|---|---|
| `A`, `L`, `E` | Assets, Liabilities, Equity | currency |
| `Rev`, `Exp` | Income/Revenue, Expenses | currency |
| `PV` (BCWS) | Planned Value / Budgeted Cost of Work Scheduled | currency |
| `EV` (BCWP) | Earned Value / Budgeted Cost of Work Performed | currency |
| `AC` (ACWP) | Actual Cost / Actual Cost of Work Performed | currency |
| `BAC` | Budget at Completion | currency |
| `CV`, `SV` | Cost Variance (`EV − AC`), Schedule Variance (`EV − PV`) | currency |
| `CPI`, `SPI` | Cost Performance Index (`EV/AC`), Schedule Performance Index (`EV/PV`) | ratio |
| `EAC`, `ETC` | Estimate at Completion, Estimate to Complete | currency |
| `VAC` | Variance at Completion (`BAC − EAC`) | currency |
| `TCPI` | To-Complete Performance Index | ratio |
| `PoC` | Percentage of completion | % |
| `r`, `n` | Discount rate per period; number of periods | ratio; count |
| `PV(x)` | Present value of amount `x` (context-flagged to avoid clash with Planned Value) | currency |

> **Notation clash rule.** `PV` denotes **Planned Value** in EVM contexts and **present value** in
> discounting contexts. Always write **"present value"** in words, or `PV(x)`, when discounting, so the two
> never collide on a page.

---

## 5. Worked-example format (mandatory shape)

Every worked example uses this five-line skeleton so a reader can follow any calculation identically across
the book:

1. **Setup** — the scenario in one or two sentences, with the given data listed.
2. **Formula** — the formula stated, each variable named with its unit.
3. **Substitution** — the numbers substituted in, shown explicitly.
4. **Result** — the computed answer, rounded per §7 and stated with its unit.
5. **Interpretation** — one or two sentences on what the number *means* for the professional's decision.

Numbers **must actually add up** — re-check every calculation. Where an assumption is needed, state it
explicitly rather than glossing over it.

---

## 6. Figure & animation spec format

- **Figures (print + digital).** Every KA specifies at least one figure where a diagram/chart/table aids
  understanding. Each figure has: a **number** (`Fig 1.2.1` = Domain 1, KA 2, figure 1), a **caption**, the
  **underlying data** (so it is reproducible), and a **render-ready description** (axes, series, labels,
  sample values). Illustration style: clean, professional, brand blue `#1D4ED8`, Plus Jakarta Sans labels.
- **Animations (digital/LMS edition only).** Where motion aids understanding (an S-curve building, a CPM
  forward/backward pass, a sprint looping), specify an **animation storyboard** — frames/steps, what
  changes each step, and the narration cue — clearly marked **"digital-only."** The print/PDF uses the
  static figure rendered from the same spec.

---

## 7. Numbers, language & currency conventions

- **British English** throughout (e.g. *recognise*, *organisation*, *utilisation*).
- **Currency:** primary examples in **USD**; where the sponsor's context helps, a parallel **SAR** figure
  (indicative rate stated at point of use, e.g. `USD 1 ≈ SAR 3.75`). Never imply a live/precise FX rate.
- **Rounding:** money to the nearest whole currency unit unless the topic needs cents; ratios/indices to
  **two decimal places**; percentages to **one decimal place** unless precision matters. State the rounding
  where it affects the answer.
- **Thousands separators:** comma (`1,250,000`). Negative/adverse amounts in parentheses `(80,000)` in
  statements; a leading minus elsewhere.

---

## 8. MCQ format (every Knowledge Area)

Each KA ends with **3–6 sample MCQs** to certification standard:

- Exactly **four options** (A–D), **one correct**, with the correct option **marked** and a **1–3 sentence
  rationale** explaining why it is right *and* why the plausible distractors are wrong.
- A **mix of cognitive levels** — *Recall*, *Application*, *Analysis* — tagged on each item, plus the
  **topic number** it maps to (e.g. `[1.4.2 · Application]`).
- **Numerical items** wherever the topic is quantitative (the candidate computes, then selects).
  Distractors must be *plausible* — the results of common errors — never filler.
- These are **study/sample items** drawn from the same blueprint as, but kept **separate from**, the live
  exam bank; do not reuse verbatim as live questions.

---

## 9. Citation rules (non-negotiable)

- **Name real frameworks; never fabricate.** Reference standards by name and principle — IFRS 15, IAS 1,
  IAS 2, IAS 16, IAS 23, IAS 37, IFRS 16; PMBOK, AACE TCM Framework; ISO 31000, ISO/IEC 17024; the Agile
  Manifesto, the Scrum Guide, Kanban, Lean. **Do not invent citations, clause numbers, page numbers or
  quotes.**
- **Never reproduce copyrighted text.** Summarise a standard's principle in your **own words** (e.g. "under
  IAS 37 a provision is recognised when…"); do **not** paste Manifesto/Scrum-Guide/standard wording
  verbatim. Examples, tables, diagrams and MCQs must be **original**.
- **Honesty about AI.** Describe real, current AI capabilities *and their limits and risks* (hallucination,
  data quality, bias, confidentiality, auditability) — never hype. Mark evolving capabilities as such. The
  governing principle throughout: **"AI proposes, the professional disposes."**

---

## 10. Per–Knowledge-Area authoring checklist

Author each KA to this checklist; a KA is complete only when every applicable line is satisfied.

- [ ] **Definition & purpose** for each topic — precise, professional, with the real standard named.
- [ ] **Underlying principle / the "why."**
- [ ] **Formulae** stated, every variable and unit defined (restating spine symbols).
- [ ] **≥ 1 fully worked example** in the §5 five-line format, numbers re-checked, realistic USD (+SAR where useful).
- [ ] **A second example or mini-case** for any non-trivial topic.
- [ ] **≥ 1 numbered figure spec** (§6) and, where motion helps, a digital-only animation storyboard.
- [ ] **Common pitfalls / misconceptions.**
- [ ] **"AI in this topic"** — how AI assists and its governance limits ("AI proposes, the professional disposes").
- [ ] **Key-terms box.**
- [ ] **3–6 sample MCQs** (§8) with answers, rationales and topic/level tags.
- [ ] **2–4 self-check questions** with answers.
- [ ] **Cross-references** by number to related topics.

---

## 11. Domain map (v1 outline — the thirteen domains)

| # | Domain | Group | ~Pages |
|---|---|---|---|
| 1 | Foundations of Accounting for Project Controls | Finance | ~110 |
| 2 | Financial Reporting & the Standards (incl. IFRS 15 flagship) | Finance | ~150 |
| 3 | Budgeting & Forecasting | Finance | ~125 |
| 4 | Performance Management, Variance Analysis & Management Reporting | Finance | ~115 |
| 5 | Cost Management & Cost Control | PM | ~90 |
| 6 | Earned Value Management & Forecasting (EVM/EAC) — flagship | PM | ~90 |
| 7 | Contracts, Commercial Management, BoQ, Invoicing & Revenue | PM | ~135 |
| 8 | Project Management Lifecycle | PM | ~120 |
| 9 | Agile, Scrum & Adaptive Delivery for Project Controls | PM | ~90 |
| 10 | Project Scheduling (in depth) | PM | ~65 |
| 11 | Business Process Cycles (O2C, P2P & the control environment) | PM | ~45 |
| 12 | Risk Management for Project Controls | PM | ~50 |
| 13 | AI for Project Controls & PM: Concepts, Tools & Practice | AI | ~300 |
| — | Appendices (glossary · master formula sheet · self-check answers · standards index · figure/animation index · MCQ bank) | — | ~55 |

Totals to ~**1,585 pages** before front/back-matter trimming — comfortably meeting the ~1,500-page target
through genuine content. Author finance domains (1–4) first to lock the notation and worked-example style,
then the PM domains, authoring the Agile domain (9) after EVM (6) since it reuses the EV/CPI/EAC machinery,
then the AI domain (13).
