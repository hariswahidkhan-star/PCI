# PCP‑AI Body of Knowledge v1 — Batch 0: Style Spine

*This is the governing style spine for the Project Controls Institute (PCI) **Certified Project Controls
Professional — AI (PCP‑AI) Body of Knowledge, v1**. Every chapter binds to it so the parallel‑authored
batches read as one book, not thirteen. Author teams and authoring agents must ingest this file first and
follow it exactly. This is an **SME‑verifiable draft**: an AI first draft is a strong starting point, not
the final certified text — every worked number, standard reference and AI claim must be checked by a
qualified subject‑matter expert before the content is finalised into the exam blueprint.*

---

## 0. What the book is

The PCP‑AI BoK is the authoritative reference defining what a Certified Project Controls Professional (AI)
must know. It reads like a professional certification handbook (the depth of PMI's PMBOK, AACE's Total
Cost Management framework, or an ACCA study text) — **not a blog or a summary**. It is the source from
which exam items are written and against which candidates study.

**Weighting (v1):** 40% financial reporting & accounting · 40% project management (lifecycle, agile/scrum,
scheduling, cost/EVM, contracts, risk, process cycles) · 20% AI knowledge & practical approach — with AI
also woven in as an *"AI in this domain / AI in this topic"* section in every chapter.

**Governing principle, everywhere AI appears:** **"AI proposes; the professional disposes."** The human
professional remains accountable for every estimate, commitment, forecast and decision.

## 1. Three‑level structure (use everywhere)

- **DOMAIN** — the major pillar (e.g. *Financial Reporting & the Standards*).
- **KNOWLEDGE AREA (KA)** — a coherent body within a domain (e.g. *IFRS 15 Revenue from Contracts with
  Customers*).
- **TOPIC** — the atomic, teachable/testable unit (e.g. *the five‑step model applied to a construction
  contract with variable consideration*).

Every page of content sits under **Domain → Knowledge Area → Topic** and is numbered `N.M.k`
(Domain.KnowledgeArea.Topic — e.g. `6.3.2`) for cross‑referencing and exam‑blueprint mapping. Number
figures `Fig N.M.k`, MCQs `MCQ N.M.k`, and worked examples `Example N.M.k`.

## 2. Per‑topic depth standard (apply to EVERY topic, where relevant)

1. **Definition & purpose** — precise, professional, with the standard/framework named.
2. **Underlying principle / the "why."**
3. **Formula(e)** — stated, defined variable‑by‑variable, with units (see §5 for canonical symbols).
4. **At least one fully worked numerical example** with realistic figures, shown step by step to the
   answer. The numbers **must actually add up** — re‑check every calculation.
5. **A second example or mini‑case** for non‑trivial topics (scenario → analysis → conclusion).
6. **At least one figure spec** where a diagram/chart/table aids understanding (numbered, captioned, with
   the underlying data and a render‑ready description). Add an **animation storyboard** flagged *digital‑
   only* where motion helps.
7. **Common pitfalls / misconceptions.**
8. **"AI in this topic"** — how AI assists this specific task *and its governance limits*, consistent with
   *"AI proposes; the professional disposes."*
9. **Key‑terms box**, **sample MCQs** (§6), and **2–4 self‑check questions** (answers in the appendix) per
   Knowledge Area.
10. **Cross‑references** to related topics by number (do not repeat content — point to it).

## 3. Non‑negotiable authoring rules

- **Accuracy over volume. Never pad.** If a topic needs four pages, write four good pages, not ten thin
  ones. The page target is met by *breadth of genuine content*, not filler.
- **Cite the standard, not a fabricated source.** Reference real frameworks **by name** — IFRS 15, IAS 37,
  IAS 1, IAS 2, IAS 16, IFRS 16, IAS 23, PMBOK, AACE TCM, ISO 31000, ISO/IEC 17024, the Agile Manifesto,
  the Scrum Guide, Kanban, Lean, SAFe/LeSS (awareness level). **Do NOT invent citations, clause numbers,
  page numbers or quotes.** Where you summarise a standard, describe the principle *in your own words*
  (e.g. "under the Scrum Guide, the Sprint is time‑boxed to…"); never reproduce copyrighted text (the
  Manifesto / Scrum Guide wording included) verbatim.
- **British English throughout**; consistent notation and terminology across all chapters (maintain the
  running glossary and formula‑symbol table so, e.g., EAC is defined identically everywhere).
- **Examples must be internally correct** — the numbers must add up; re‑check every calculation.
- **AI content must be honest** — describe real, current AI capabilities and their limits and risks
  (hallucination, data quality, bias, governance, auditability), not hype. Name tool **categories and
  representative tools** without fabricating features. Mark anything genuinely uncertain or evolving as
  such rather than overstating it.
- **Flag for SME sign‑off.** End each Knowledge Area with any items that a finance / agile / AI SME must
  verify before the content is finalised.

## 4. Currency, number and date conventions

- Primary currency **USD ($)**; where the sponsor's context helps, also give **SAR** (Saudi Riyal) at an
  illustrative rate stated in the example (e.g. "at an illustrative SAR 3.75 = USD 1"). Never imply a live
  FX rate.
- Thousands separator with commas ($1,250,000). Percentages to one decimal unless precision demands more
  (CPI 0.92). Negative variances in parentheses where a table convention needs it, with the sign stated in
  prose.
- Dates in British form (31 March 2026). Periods as "Month N" for schedule examples.

## 5. Canonical formula symbols (define once — use identically everywhere)

Earned Value & forecasting (Domains 3, 6, 9):

| Symbol | Term | Definition |
|---|---|---|
| PV (BCWS) | Planned Value | Budgeted cost of work scheduled to date |
| EV (BCWP) | Earned Value | Budgeted cost of work performed to date |
| AC (ACWP) | Actual Cost | Actual cost of work performed to date |
| BAC | Budget at Completion | Total budgeted cost of the project |
| CV | Cost Variance | `CV = EV − AC` |
| SV | Schedule Variance | `SV = EV − PV` |
| CPI | Cost Performance Index | `CPI = EV / AC` |
| SPI | Schedule Performance Index | `SPI = EV / PV` |
| EAC | Estimate at Completion | Forecast total cost (several formulae — see KA 6.3) |
| ETC | Estimate to Complete | `ETC = EAC − AC` |
| VAC | Variance at Completion | `VAC = BAC − EAC` |
| TCPI | To‑Complete Performance Index | `TCPI = (BAC − EV) / (BAC − AC)` (or `/(EAC − AC)`) |

Common EAC formulae (state assumptions each time):
`EAC = AC + (BAC − EV)` (future work at budget) · `EAC = BAC / CPI` (past efficiency continues) ·
`EAC = AC + (BAC − EV) / (CPI × SPI)` (cost and schedule pressure continue).

Accounting & finance (Domains 1–7): the accounting equation `Assets = Liabilities + Equity`; POC (input
method) `% complete = costs incurred to date / total estimated costs`; contract asset/liability = revenue
recognised − amounts billed (sign convention stated in KA 7.5). Agile (Domain 9): velocity = story points
completed per sprint; AgileEVM reuses EV/CPI/SPI/EAC with scope‑variable assumptions stated (KA 9.5).

New symbols introduced in a chapter must be added to the glossary and this table during consolidation.

## 6. Worked‑example, figure, animation and MCQ formats

**Worked example** — `Example N.M.k — <title>`: *Scenario* (given data as a small table) → *Required* →
*Working* (each step shown, formula → substitution → result, units carried) → *Answer* (boxed) →
*Interpretation* (one or two sentences on what it means for the professional).

**Figure spec** — `Fig N.M.k — <caption>`: chart/diagram type; axes/series/labels; the **underlying data**
(a small table) so it is reproducible; a render‑ready description (an illustrator or tool can draw it).
Illustration style: brand blue `#1D4ED8`, clean professional diagrams, sans‑serif labels.

**Animation storyboard (digital/LMS only)** — `Animation N.M.k — <caption> [digital‑only]`: frames/steps,
what changes each frame, and a narration cue. The print/PDF edition uses the corresponding static figure.

**MCQ** — `MCQ N.M.k [level: recall|application|analysis]`: a stem, **four options (A–D)**, the **correct
answer marked**, and a **1–3 sentence rationale** explaining why it is right and why the distractors are
wrong. Distractors must be plausible (common errors), not filler. Include **numerical MCQs** for
finance/EVM/agile‑metrics topics (candidate computes, then selects). Tag each with its topic number and
cognitive level. These are **study/sample items** — drawn from the same blueprint as, but kept **separate
from**, the live exam bank (never reused verbatim as live questions).

## 7. Chapter file skeleton (every KA file follows this)

```
# Domain N — <Domain title>
## KA N.M — <Knowledge Area title>
<one‑paragraph orientation: what this KA covers and why it matters to a project‑controls professional>

### N.M.1 <Topic> … (definition · principle · formula · worked example(s) · figure/animation ·
                      pitfalls · AI in this topic · cross‑refs)
### N.M.2 <Topic> …
…
### Key terms
### AI in this domain / topic
### Sample MCQs
### Self‑check questions (answers in Appendix C)
### For SME verification
```

## 8. Domain map & page budget (targets — meet by genuine content, ±15% per section)

1. Foundations of Accounting for Project Controls (~90 pp) · 2. Financial Reporting & the Standards
(~120 pp) · 3. Budgeting & Forecasting (~100 pp) · 4. Performance Management, Variance Analysis &
Management Reporting (~90 pp) · 5. Cost Management & Cost Control (~70 pp) · 6. **Earned Value Management &
Forecasting** *(flagship, ~70 pp)* · 7. Contracts, Commercial Management, BoQ, Invoicing & Revenue
(~110 pp) · 8. Project Management Lifecycle (~95 pp) · 9. Agile, Scrum & Adaptive Delivery *(new, ~70 pp)* ·
10. Project Scheduling (~50 pp) · 11. Business Process Cycles — O2C/P2P & controls (~35 pp) · 12. Risk
Management (~40 pp) · 13. **AI for Project Controls & PM** *(major, ~240 pp)* · Appendices (~40 pp:
glossary · master formula sheet · self‑check answers · standards referenced · figure/animation index ·
worked‑example index).

Finance domains (1–7) are authored first so the notation and worked‑example style lock there; the Agile
domain (9) is authored after EVM (6) because AgileEVM reuses EV/CPI/SPI/EAC and IFRS 15 — the notation must
stay identical across the classical and agile treatments.
