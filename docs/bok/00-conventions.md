# Conventions of This Reference

This reference is one book, not thirteen. The same symbols, the same worked-example shape, the same rounding
rules and the same citation practice run from the first page to the last, so that a formula first met in
Domain 3 means exactly the same thing when it reappears in Domain 9. This opening chapter sets out those shared
conventions — how the book is numbered, how its worked examples are laid out, what its symbols mean, how its
figures and sample questions are built, and how it treats standards, numbers and language. Read it once; it
makes every later chapter faster to read.

---

## 1. Scope and weighting

This reference covers project controls, project finance and the governed use of artificial intelligence, and
it is weighted **40 % finance / 40 % project management / 20 % AI**.

| Domain group | Share |
|---|---|
| Finance, accounting & reporting (Domains 1–4) | 40 % |
| Project management — lifecycle, agile, scheduling, cost/EVM, contracts, risk, process cycles (Domains 5–12) | 40 % |
| AI knowledge & practical approach (Domain 13) | 20 % |

The AI weighting is carried by Domain 13 together with the "AI in this domain/KA" sections embedded in every
non-AI chapter, and by KA 13.5's systematic pass across all thirteen domains — the 20 % is measured across
that whole surface, not by Domain 13's page count alone. Depth follows the material rather than a page quota:
where a topic genuinely demands more, it gets more.

This Body of Knowledge *is* the credential framework. The Institute's examination blueprint and practice
materials are aligned to these thirteen domains and to the 40/40/20 weighting; practice environments may
regroup content for drill purposes, but the domain numbering and Knowledge-Area structure defined here govern
throughout.

---

## 2. How the book is numbered

Everything in this reference sits in a three-level hierarchy:

- **Domain** — the major pillar (for example, *Foundations of Accounting for Project Controls*).
- **Knowledge Area (KA)** — a coherent body of practice within a domain (for example, *The accounting model*).
- **Topic** — the atomic, teachable and testable unit (for example, *Debit and credit rules by account type*).

Each level carries a number, written `Domain.KA.Topic`. So **1.1.2** is Domain 1, Knowledge Area 1.1,
Topic 2. Knowledge Areas are cited as **6.3**, topics as **6.3.2**.

**Cross-references are by number.** When a passage says "see 6.3.1", it is pointing at that exact topic rather
than repeating its content, so the explanation lives in one place and stays consistent. Following the numbers
is how the disciplines are meant to be read together: the accrual discipline of 5.2 is what makes the `AC` in
6.2 true, and the forecast chosen in 6.3 is what sets the revenue ratio in 2.2.6.

Two further numbering conventions appear inside chapters. Advanced material is lettered — **10.A.5** is
Domain 10's fifth advanced topic — and appears in the *Advanced topics* section after the Knowledge Areas.
Worked examples take the number of the topic they illustrate, with a letter suffix where a topic carries more
than one (**10.3.1**, then **10.3.1b**, **10.3.1c**).

Each domain follows one shape: Knowledge Areas, advanced topics, two sector case studies, an executive
perspective, calculation exercises with full solutions (in the quantitative domains), a practitioner's
toolkit, exam preparation, and a summary.

---

## 3. Terminology and the glossary

Terms are defined once and used identically everywhere; a later chapter never quietly redefines a term an
earlier one established. Each Knowledge Area closes with a **key-terms box** giving the working definition of
the terms it introduced, and **Appendix B** consolidates every one of those boxes into a single alphabetical
glossary, each entry tagged with the Knowledge Area where it is first defined. Where a term recurs across
domains, that first definition governs.

A small core of terms recurs so often that it is worth fixing before Domain 1 begins:

| Term | Definition (as used throughout this reference) |
|---|---|
| **Accrual basis** | Recognising the effects of transactions when they occur, not when cash moves. |
| **Baseline** | The approved, version-controlled plan (scope, schedule or cost) against which performance is measured. |
| **Control account (CA)** | A management-control point where scope, budget, actual cost and schedule integrate — the intersection of a WBS element and an organisational (OBS) element. |
| **Cost breakdown structure (CBS)** | The hierarchical decomposition of a project's cost by cost element/type. |
| **Provision** | A liability of uncertain timing or amount, recognised under IAS 37 when the recognition tests are met. |
| **Recognition** | Recording an item in the financial statements (as an asset, liability, income or expense). |
| **Work breakdown structure (WBS)** | The hierarchical decomposition of the total scope of work into deliverables and work packages. |

---

## 4. The master formula-symbol table

Every symbol below means the same thing in every chapter. A chapter restates a symbol's definition where it
first uses it, but never changes it. The finance and earned-value symbols are collected here even though they
are first used in depth in Domains 3–6, because the classical treatment (Domain 6) and the adaptive one
(Domain 9) share one notation. **Appendix A** carries the full formula sheet.

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

> **One notation clash to watch.** `PV` denotes **Planned Value** in earned-value contexts and **present
> value** in discounting contexts. Where discounting is meant, this reference writes **"present value"** in
> words or `PV(x)`, so the two never collide on a page.

---

## 5. Worked examples — the five-step format

Worked examples are the spine of the book, and they all follow the same five steps, so that once the format is
familiar any calculation in any domain can be followed at speed:

1. **Setup** — the scenario in a sentence or two, with the given data listed.
2. **Formula** — the formula stated, each variable named with its unit.
3. **Substitution** — the numbers substituted in, shown explicitly.
4. **Result** — the computed answer, rounded per §7 and stated with its unit.
5. **Interpretation** — what the number *means* for the decision in front of the professional.

The interpretation step is never decorative: a figure that has not been interpreted has not yet been used.
Where a calculation is inherently tabular — a network forward and backward pass, a multi-scenario comparison —
the five steps give way to a labelled table, but the interpretation still follows it. Where an example rests on
an assumption, the assumption is stated rather than glossed over, because in practice it is the assumption
that gets challenged.

**How to work them.** Attempt each example before reading its solution: cover the substitution, work it, then
compare. The **calculation exercises** at the end of each quantitative domain extend the same discipline to
multi-step problems with full solutions, and the **exam preparation** section names the errors these
calculations most often attract.

---

## 6. Figures

Figures are numbered on the same scheme as everything else: **Fig 1.2.1** is the first figure in Domain 1,
Knowledge Area 2. Each figure carries a caption, the underlying data it is drawn from — so the reader can
reproduce or re-scale it — and a description of what is plotted. Figures are drawn in a single house style:
clean, professional, brand blue `#1D4ED8`. **Appendix D** indexes every figure in the book.

Some figures also carry an **animation storyboard**, marked *digital-only*. These describe how the figure
moves in the digital and LMS editions — an S-curve building period by period, a critical-path pass sweeping
forward then backward, a sprint looping. The printed and PDF editions use the static figure rendered from the
same data; nothing needed to understand the point is confined to the animation.

---

## 7. Numbers, language and currency

- **British English** throughout (*recognise*, *organisation*, *utilisation*).
- **Currency:** primary figures in **USD**, with a parallel **SAR** figure where the regional context helps.
  Any exchange rate shown is indicative and stated at the point of use (for example, `USD 1 ≈ SAR 3.75`); no
  rate in this book should be read as a live or precise one.
- **Rounding:** money to the nearest whole currency unit unless the topic needs cents; ratios and indices to
  **two decimal places**; percentages to **one decimal place** unless the precision matters. Where rounding
  changes the answer, the book says so and carries the unrounded figure through the working.
- **Thousands separators:** comma (`1,250,000`). Negative and adverse amounts appear in parentheses —
  `(80,000)` — in statements and variance tables, and with a leading minus elsewhere.
- **Code style:** symbols, formulae and account entries are set in `monospace` so they stand apart from the
  prose that explains them.

---

## 8. Sample MCQs

Each Knowledge Area closes with **sample multiple-choice questions** written to certification standard. Every
item has exactly **four options (A–D)** with **one correct answer**, followed by a short **rationale** that
explains why the right answer is right *and* why each plausible distractor is wrong — the distractors are the
results of the errors candidates actually make, not filler. Each item is tagged with the **topic number it
tests and its cognitive level** — *Recall*, *Application* or *Analysis* — written `[1.4.2 · Application]`, so
weak areas can be traced straight back to the topic that fixes them. Where a topic is quantitative, the item
is quantitative: the candidate computes first, then selects.

**Appendix F** collects every sample item in the book into one bank, grouped by domain and numbered
`PCL-MCQ-DD-NN`, with the answer to each and a pointer back to the Knowledge Area holding its rationale.
Self-check questions — two or three at the close of each Knowledge Area, with their answers alongside for
immediate self-marking — are a lighter, faster check on the same material; **Appendix E** gathers their
answers into a single key.

These are **study items**. They are written from the same blueprint as the examination but are maintained
separately from any live examination bank, and they are not reused as live questions.

---

## 9. Standards and citation practice

Real frameworks are named and their principles explained; nothing is invented and nothing is copied.

- **Named at principle level.** IFRS 15, IAS 1, IAS 2, IAS 16, IAS 23, IAS 37 and IFRS 16; the PMBOK Guide and
  the AACE Total Cost Management framework; ISO 31000 and ISO/IEC 17024; the Agile Manifesto, the Scrum Guide,
  Kanban and Lean; and, for AI governance (13.6.6), ISO/IEC 42001, ISO/IEC 23894, the NIST AI RMF, the OECD AI
  Principles and the EU AI Act — of which only the last is legislation, and only within its own jurisdiction.
  **Appendix C** indexes every framework this book refers to and where it is used.
- **In this book's own words.** No standard's text is reproduced. A principle is summarised — "under IAS 37 a
  provision is recognised when…" — and every example, table, figure and question is original. Clause numbers,
  page references and quotations are never manufactured to look authoritative.
- **Honest about AI.** Domain 13 describes real, current capabilities together with their limits and risks —
  hallucination, data quality, bias, confidentiality, auditability — and marks fast-moving capabilities as
  such. One principle governs the whole treatment: **AI proposes; the professional verifies, decides and
  remains accountable.**

---

## 10. The thirteen domains

| # | Domain | Group |
| --- | --- | --- |
| 1 | Foundations of Accounting for Project Controls | Finance |
| 2 | Financial Reporting & the Standards (incl. IFRS 15 flagship) | Finance |
| 3 | Budgeting & Forecasting | Finance |
| 4 | Performance Management, Variance Analysis & Management Reporting | Finance |
| 5 | Cost Management & Cost Control | PM |
| 6 | Earned Value Management & Forecasting (EVM/EAC) — flagship | PM |
| 7 | Contracts, Commercial Management, BoQ, Invoicing & Revenue | PM |
| 8 | Project Management Lifecycle | PM |
| 9 | Agile, Scrum & Adaptive Delivery for Project Controls | PM |
| 10 | Project Scheduling (in depth) | PM |
| 11 | Business Process Cycles (O2C, P2P & the control environment) | PM |
| 12 | Risk Management for Project Controls | PM |
| 13 | AI for Project Controls & Project Management: Concepts, Tools & Practice | AI |
| — | Appendices — formula sheet · glossary · standards index · figure index · self-check answers · sample-MCQ bank · integrated capstone | — |

The order is cumulative rather than arbitrary. Domains 1–4 build the financial grammar the rest of the book
speaks; Domains 5–12 apply it to delivery, with Domain 9 deliberately placed after Domain 6 because adaptive
delivery reuses the `EV`/`CPI`/`EAC` machinery; and Domain 13 comes last so that its workflows can be built on
the disciplines they govern. **Appendix G** then runs a single project through all thirteen domains in one
month-end close, which is the closest this reference comes to showing the job as it is actually done.
