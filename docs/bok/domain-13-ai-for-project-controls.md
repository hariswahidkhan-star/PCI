# Domain 13 — AI for Project Controls & Project Management: Concepts, Tools & Practice

## Why this domain exists

Artificial intelligence is changing how project controls and project management are done — but the credential's
governing principle never changes: **"AI proposes; the professional verifies, decides and remains accountable."** AI can draft, extract,
forecast, detect and summarise; it cannot be *accountable*. This domain teaches both the **concepts** (what AI
is, how it works, where it fails) and the **hands-on practice** (which tool categories, which workflows, how to
apply AI to real controls tasks) so a certified professional can use AI **competently and responsibly**. It
covers: AI foundations for professionals (KA 13.1); **data**, the fuel that governs every AI outcome (KA 13.2);
**prompting** and working with generative AI (KA 13.3); the **tool categories** for controls and PM (KA 13.4);
**AI applied across the controls lifecycle** — the heart of the domain, with hands-on workflows cross-referencing
every earlier domain (KA 13.5); **governance, ethics, risk and assurance** (KA 13.6); and **building an
AI-augmented capability** (KA 13.7). Throughout, claims are kept **honest and current**: real capabilities,
real limits — hallucination, data quality, bias, confidentiality, auditability — and real governance.

**Learning objectives.** After this domain a candidate can: explain AI/ML/GenAI concepts at a working level
and classify a task as rules vs ML vs GenAI; assess and prepare project-controls data for AI; prompt
generative AI effectively and verify its output; select the right AI tool category for a task; apply AI to
estimating, forecasting/EVM, cost control, scheduling, agile, contracts, reporting, risk and financial
reporting, with the professional's verification shown; govern and assure AI use ("AI proposes; the professional verifies, decides and remains accountable"); and build and mature an AI-augmented controls capability.

---

## Knowledge Area 13.1 — AI foundations for professionals

*Topics: 13.1.1 AI, ML and GenAI · 13.1.2 how machine learning learns · 13.1.3 large language models · 13.1.4
training, inference, fine-tuning and RAG · 13.1.5 strengths and hard limits · 13.1.6 rules vs ML vs GenAI.*

### 13.1.1 AI, ML and GenAI

**Definitions.** **Artificial intelligence (AI)** is the broad field of systems that perform tasks associated
with human intelligence. **Machine learning (ML)** is the subset that **learns patterns from data** rather than
following hand-written rules. **Generative AI (GenAI)** is the subset of ML that **generates new content**
(text, images, code) — the large language models behind assistants are GenAI. These are nested: **GenAI ⊂ ML ⊂
AI**. Precision matters because the categories have different data needs, failure modes and governance (13.6).

> **Fig 13.1.1 — The AI landscape.** *Caption:* nested fields and where controls tasks sit. *Underlying data:*
> AI ⊃ ML ⊃ GenAI, with examples. *Render-ready description:* three concentric brand-blue rings — outer "AI",
> middle "Machine learning", inner "Generative AI" — each annotated with a controls example (AI: rules-based
> validation; ML: cost-forecast model; GenAI: drafting a variance narrative). *Animation storyboard
> (digital-only):* a set of controls tasks flies in and sorts itself into the correct ring.

### 13.1.2 How machine learning learns

**The principle.** ML learns a mapping from inputs to outputs by fitting to **data**:

- **Supervised learning** — learns from **labelled** examples (inputs with known answers) to predict labels for
  new inputs (e.g. predicting final cost from features of past projects). Most controls ML is supervised.
- **Unsupervised learning** — finds **structure** in unlabelled data (clustering similar cost items, detecting
  anomalies without predefined "bad" examples).
- **Reinforcement learning** — learns by **trial and reward** in an environment (less common in controls;
  relevant to some scheduling/optimisation).

The universal truth: an ML model is **only as good as its data** (13.2). A supervised model trained on
unrepresentative history will confidently mislead.

### 13.1.3 Large language models

**The principle (conceptual).** A **large language model (LLM)** generates text by predicting the next
**token** (a word-piece) given the preceding text, having learned statistical patterns from vast text. Key
working concepts:

- **Tokens** — the units LLMs read and generate; cost and length limits are measured in tokens.
- **Context window** — the amount of text (tokens) the model can consider at once; everything the model "knows"
  for a task must fit in it (or be retrieved into it, 13.1.4).
- **Temperature** — a setting controlling randomness/creativity of output; **lower** for deterministic,
  factual tasks (extraction, calculation-checking), **higher** for ideation. For controls work, **low
  temperature and verification** are the norm.

Crucially, an LLM generates **plausible** text, not **verified** text: it has no inherent notion of truth, only
of likelihood. This is the root of **hallucination** (13.1.5, 13.6).

### 13.1.4 Training, inference, fine-tuning and RAG

**The terms.**

- **Training** — the (expensive, one-off) process of learning the model's parameters from data.
- **Inference** — using the trained model to produce an output (what happens each time you prompt it).
- **Fine-tuning** — further training a base model on domain-specific data to specialise it.
- **Retrieval-augmented generation (RAG)** — retrieving relevant documents (your contracts, your standards,
  your project data) and supplying them to the model *at inference*, so its answer is grounded in **your**
  content rather than only its training. RAG is the dominant pattern for applying GenAI to an organisation's
  own knowledge safely, because it grounds answers in cited source material.

### 13.1.5 Strengths and hard limits

**Honest capabilities.** Current AI is genuinely strong at: **extraction** (pulling structured data from
documents), **drafting** (first-cut narratives, plans, summaries), **classification** (coding, categorising),
**pattern detection** (anomalies, trends), **forecasting** (from data), and **summarisation**. These are real,
high-value capabilities across controls.

**Hard limits.** Equally real: **hallucination** (confidently producing false content — fabricated figures,
citations, clauses); **data dependence** ("garbage in, garbage out"); **bias** (reproducing biases in training
data); **no true reasoning guarantee** (plausible ≠ correct, especially in multi-step calculation);
**no accountability** (a model cannot own a decision); **confidentiality risk** (pasting sensitive data into
ungoverned tools); and a **knowledge cutoff** (a base model does not know events after its training unless given
them). Every workflow in this domain is designed around these limits, not in denial of them.

### 13.1.6 Rules vs ML vs GenAI

**The professional judgement.** Not every task needs AI — and choosing the wrong kind wastes effort or adds
risk:

- Use **rules/automation** when the logic is **known and deterministic** (a three-way match tolerance, a
  validation check). Rules are transparent and auditable; do not use ML where a rule suffices.
- Use **ML** when there is a **pattern in data** worth learning that is hard to specify by rule (predicting
  cost overrun likelihood, detecting anomalies).
- Use **GenAI** when the task is **generating or transforming natural language/content** (drafting, extracting,
  summarising) — with verification.

**Worked example 13.1.6 — classify controls tasks.**

| Task | Best fit | Why |
|---|---|---|
| Flag invoices where PO ≠ invoice price | **Rules** | Deterministic, known logic (three-way match) |
| Predict final cost from project features | **ML (supervised)** | Learn a pattern from labelled history |
| Group similar anomalous cost postings | **ML (unsupervised)** | Find structure without labels |
| Draft a variance narrative from the numbers | **GenAI** | Generate language — then verify |
| Extract payment terms from a 200-page contract | **GenAI (RAG)** | Language extraction grounded in the document |

### Key terms — KA 13.1

| Term | Meaning |
|---|---|
| **AI / ML / GenAI** | The field / learning-from-data subset / content-generating subset (nested). |
| **Supervised / unsupervised / reinforcement** | Learn from labels / structure / trial-and-reward. |
| **Token / context window / temperature** | Text unit / how much the model considers / randomness setting. |
| **Training / inference / fine-tuning / RAG** | Learn parameters / use the model / specialise it / ground it in your documents. |
| **Hallucination** | Confidently producing false content. |

### Sample MCQs — KA 13.1

**MCQ 13.1-A `[13.1.1 · Recall]`** Which relationship is correct?
- A. GenAI ⊂ ML ⊂ AI ✅
- B. AI ⊂ ML ⊂ GenAI
- C. ML ⊂ GenAI ⊂ AI
- D. They are unrelated fields.

*Rationale:* GenAI is a subset of ML, which is a subset of AI. The others invert the nesting.

**MCQ 13.1-B `[13.1.6 · Analysis]`** Flagging invoices whose PO price and invoice price differ is best done with:
- A. Generative AI.
- B. Reinforcement learning.
- C. Rules/automation (deterministic logic). ✅
- D. A large language model.

*Rationale:* The logic is known and deterministic (a three-way match) — a transparent, auditable rule. GenAI/
LLM/RL add opacity and risk where a rule suffices.

**MCQ 13.1-C `[13.1.4 · Recall]`** Retrieval-augmented generation (RAG) primarily:
- A. Retrains the model on your data.
- B. Supplies relevant documents to the model at inference so answers are grounded in your content. ✅
- C. Removes the need for verification.
- D. Increases temperature.

*Rationale:* RAG grounds generation in retrieved source material at inference without retraining. It does not
retrain (that is fine-tuning), does not remove the need to verify, and is unrelated to temperature.

**MCQ 13.1-D `[13.1.3 · Recall]`** For a factual controls task such as extracting figures from a document, the
temperature setting should be:
- A. High, to maximise creativity.
- B. Set equal to the context-window size.
- C. Irrelevant — temperature only affects cost.
- D. Low, to reduce randomness in the output. ✅

*Rationale:* Low temperature suits deterministic, factual tasks; high temperature is for ideation. Temperature
is a randomness setting, not a cost control, and is unrelated to the context window.

**MCQ 13.1-E `[13.1.5 · Analysis]`** An LLM returns a fluent, confident multi-step cost calculation. The
professional must still recompute it because:
- A. An LLM generates plausible text, not verified text — plausible ≠ correct, especially in multi-step
  calculation. ✅
- B. LLMs always round figures incorrectly.
- C. Recomputation is only needed when temperature is high.
- D. The context window truncates all calculations.

*Rationale:* An LLM has no inherent notion of truth, only of likelihood — fluency and confidence do not warrant
correctness, and there is no true reasoning guarantee. B overstates a specific failure; C and D misapply the
concepts — verification is required at any temperature.

**MCQ 13.1-F `[13.1.2 · Application]`** A controls team wants to group thousands of anomalous cost postings
into families of similar cases, with no predefined categories or labelled examples. The best-fit approach is:
- A. Rules/automation.
- B. Supervised ML.
- C. Unsupervised ML — finding structure in unlabelled data. ✅
- D. Reinforcement learning.

*Rationale:* With no labels and no known logic, the task is finding structure in unlabelled data — unsupervised
learning (13.1.2, and the classification table of 13.1.6). A rule needs the logic to be known and
deterministic; supervised ML needs labelled examples; reinforcement learning learns by trial and reward in an
environment, which this task does not offer.

**MCQ 13.1-G `[13.1.4 · Application]`** A commercial team wants an assistant that answers questions from a
contract set that changes weekly, with each answer citing its source clause. Between fine-tuning and RAG, the
better fit is:
- A. Fine-tuning, because it permanently teaches the model the contracts.
- B. RAG — the current documents are retrieved and supplied at inference, so answers are grounded in this
  week's contract set and cited to source. ✅
- C. Fine-tuning, because it removes hallucination.
- D. Neither — LLMs cannot work over documents.

*Rationale:* RAG supplies the documents at inference, so a weekly-changing corpus stays current without
retraining and answers can cite retrieved sources (13.1.4). A would bake in a snapshot that goes stale with
every change; C is false — no technique removes hallucination or the need to verify; D contradicts 13.2.3 —
unstructured documents are exactly the domain of GenAI/RAG.

**MCQ 13.1-H `[13.1.3 · Recall]`** The context window of an LLM is:
- A. The amount of text (in tokens) the model can consider at once — everything it "knows" for a task must fit
  in it or be retrieved into it. ✅
- B. The setting that controls randomness in the output.
- C. The period after which a model's training data goes stale.
- D. The screen area of the assistant's interface.

*Rationale:* The context window is the model's working span in tokens (13.1.3). B describes temperature; C
describes the knowledge cutoff; D has nothing to do with the concept.

### Self-check — KA 13.1

1. Define token, context window and temperature. *(Text unit the model reads/writes; how much text it
   considers at once; a randomness setting — low for factual work.)*
2. Give one task each for rules, ML and GenAI. *(Rules — deterministic check; ML — pattern from data; GenAI —
   generate/transform language, verified.)*

---

## Knowledge Area 13.2 — Data: the fuel

*Topics: 13.2.1 garbage in, garbage out · 13.2.2 data quality dimensions · 13.2.3 structure, governance and
lineage · 13.2.4 project-controls data sources · 13.2.5 privacy, confidentiality and preparing data.*

### 13.2.1 Garbage in, garbage out

**The principle.** Every AI outcome is **dominated by the data behind it**. An estimating model trained on
mis-coded cost (Domain 1, KA 1.5) learns the mis-coding; a forecast built on incomplete accruals (Domain 5, KA
5.2) forecasts the wrong number; a RAG assistant over an out-of-date contract set answers from stale terms.
"Garbage in, garbage out" is not a cliché here — it is the single most important determinant of whether AI
helps or harms in project controls. Data quality is therefore a *prerequisite*, not an afterthought.

### 13.2.2 Data quality dimensions

**The dimensions.** Data quality is assessed along recognised dimensions:

| Dimension | Question |
|---|---|
| **Accuracy** | Does it reflect reality? |
| **Completeness** | Is anything missing (e.g. omitted accruals)? |
| **Consistency** | Does it agree across systems (controls vs ledger)? |
| **Timeliness** | Is it current enough for the decision? |
| **Validity** | Does it conform to the rules/format (valid codes)? |
| **Uniqueness** | Free of duplicates? |

A controls professional already manages several of these through reconciliation and cut-off (Domains 1, 5) —
which is exactly why controls data, well-governed, is a strong foundation for AI.

**Worked example 13.2.2 — a data-quality assessment before an AI initiative.**

1. **Setup.** Before training a cost-forecast model, a controls team profiles its **12,000-row** historical cost
   dataset against the quality dimensions above.
2. **Method.** Automated checks per dimension — validity (cost codes tested against the chart of accounts),
   uniqueness (duplicate detection), completeness (accrual flags present).
3. **Substitution.** Invalid codes **3 % = 360 rows**; duplicates **1.5 % = 180 rows**; missing accrual flags
   **6 % = 720 rows** — up to **1,260 rows (10.5 %)** failing at least one check (assuming no overlap).
4. **Result.** The dataset is **not yet model-ready**; a remediation pass (re-coding, de-duplication, accrual
   back-fill) precedes any training.
5. **Interpretation.** Profiling *before* building is the discipline — a model trained on the raw set would
   learn the 10.5 % of noise as if it were signal (13.2.1). The remediation work is classic controls hygiene
   (Domains 1, 5), which is why the controls professional is well-placed to lead data preparation.

### 13.2.3 Structure, governance and lineage

**The principle.** **Structured** data (tables, coded fields) is directly usable by ML; **unstructured** data
(contracts, emails, reports) is the domain of GenAI/RAG. **Data governance** defines ownership, definitions,
quality standards and access. **Data lineage** traces where a data point came from and how it was transformed —
essential for **auditability** (13.6): if an AI-influenced number is challenged, you must be able to trace it
back to source.

> **Fig 13.2.1 — Data quality and lineage for a controls AI workflow.** *Caption:* from source to AI output,
> with quality gates. *Underlying data:* ERP cost → coding → reconciliation → model input → AI forecast.
> *Render-ready description:* a left-to-right pipeline — "Source (ERP/schedule/contracts)" → "Quality gate
> (accuracy/completeness/validity)" → "Governed dataset (lineage tracked)" → "AI model/assistant" → "Output
> (verified)"; each stage a brand-blue node, quality gates as check icons, a lineage line running end-to-end.
> *Animation storyboard (digital-only):* a data record travels the pipeline; at each quality gate it is checked
> (pass = green); its lineage line is drawn behind it so the final output can be traced to source.

### 13.2.4 Project-controls data sources

**The sources.** The data a controls professional feeds to AI comes from: the **ERP/cost ledger** (actuals,
commitments, accruals — Domains 1, 5); the **schedule** (activities, dates, progress — Domain 10); **contracts
and BoQs** (terms, rates, quantities — Domain 7); **earned-value data** (`PV`/`EV`/`AC` — Domain 6); the **risk
register** (Domain 12); and external data (market rates, weather, benchmarks). Integrating these into a
governed dataset is what enables cross-domain AI workflows (13.5).

### 13.2.5 Privacy, confidentiality and preparing data

**The principle.** Before data touches an AI tool, two questions matter: **is it fit** (quality, 13.2.2) and
**is it safe** (confidentiality). **Never paste confidential or personal data into an ungoverned public tool** —
commercially sensitive contract terms, personal data, security information — because you lose control of it.
Use **governed** enterprise tools with appropriate data handling, and **anonymise/aggregate** where possible.
Preparing data — cleaning, coding, de-duplicating, structuring — is often the majority of the effort in any AI
initiative, and it is work a controls professional is well-placed to lead.

### Key terms — KA 13.2

| Term | Meaning |
|---|---|
| **Garbage in, garbage out** | AI outcomes are dominated by input-data quality. |
| **Data quality dimensions** | Accuracy, completeness, consistency, timeliness, validity, uniqueness. |
| **Structured / unstructured** | Tabular (ML) vs free-form (GenAI/RAG) data. |
| **Governance / lineage** | Ownership/definitions/access / traceability of a data point to source. |

### Sample MCQs — KA 13.2

**MCQ 13.2-A `[13.2.1 · Analysis]`** An ML cost-forecast model is trained on historically mis-coded project
cost. The most likely outcome is:
- A. The model corrects the mis-coding.
- B. Better accuracy.
- C. No effect — models are robust to bad data.
- D. The model learns and reproduces the mis-coding, giving misleading forecasts. ✅

*Rationale:* Models learn the patterns in their data, including errors — garbage in, garbage out. They do not
self-correct source errors, and bad data degrades (not improves) accuracy.

**MCQ 13.2-B `[13.2.5 · Recall]`** Before using an external AI tool on project data, the professional must
ensure the data is:
- A. As large as possible.
- B. Fit (quality) and safe (confidentiality — no sensitive data in ungoverned tools). ✅
- C. Unstructured.
- D. Public.

*Rationale:* Fitness (quality) and safety (confidentiality) are the two gates. Size, structure and publicness
are not the governing tests.

**MCQ 13.2-C `[13.2.2 · Application]`** A 20,000-row cost dataset is profiled before an AI initiative: **4 %**
of rows have invalid codes, **2 %** are duplicates and **5 %** are missing accrual flags. Assuming no overlap,
the number of rows failing at least one check is:
- A. 800
- B. 1,000
- C. 2,200 ✅
- D. 4,000

*Rationale:* With no overlap the failures add: 4 % + 2 % + 5 % = 11 %; `20,000 × 0.11 = 2,200` rows
(800 + 400 + 1,000). A counts only the invalid codes; B counts only the missing accrual flags; D (20 %) has no
basis in the data. At 11 % failing, the dataset is not yet model-ready — remediation precedes training (13.2.2).

**MCQ 13.2-D `[13.2.3 · Recall]`** Contracts, correspondence and free-form reports — unstructured data — are
primarily the domain of:
- A. GenAI / RAG. ✅
- B. Supervised ML over tabular features.
- C. Rules-based validation only.
- D. No AI category.

*Rationale:* Structured (tabular, coded) data is directly usable by ML; unstructured documents are the domain
of GenAI/RAG (13.2.3). Rules validate coded fields, and unstructured data is very much within AI's reach.

**MCQ 13.2-E `[13.2.2 · Application]`** A 15,000-row cost dataset is profiled before an AI initiative: **2 %**
of rows have invalid codes, **3 %** are duplicates and **4 %** are missing accrual flags. Assuming no overlap,
the number of rows failing at least one check is:
- A. 300
- B. 600
- C. 900
- D. 1,350 ✅

*Rationale:* With no overlap the failure rates add: 2 % + 3 % + 4 % = 9 %; `15,000 × 0.09 = 1,350` rows
(300 + 450 + 600). A counts only the invalid codes; B counts only the missing accrual flags; C adds the
invalid and missing-flag rates but omits the duplicates. At 9 % failing, remediation precedes any training
(13.2.2).

**MCQ 13.2-F `[13.2.2 · Application]`** Profiling finds that (i) the controls system and the ledger disagree
on several cost totals, and (ii) a number of postings appear twice. The data-quality dimensions failing are:
- A. Accuracy and timeliness.
- B. Validity and completeness.
- C. Consistency and uniqueness. ✅
- D. Timeliness and validity.

*Rationale:* Agreement across systems is **consistency**; freedom from duplicates is **uniqueness** (13.2.2).
Accuracy asks whether data reflects reality, timeliness whether it is current, validity whether it conforms to
rules/format, completeness whether anything is missing — none of which is what these two findings describe.

**MCQ 13.2-G `[13.2.3 · Analysis]`** An auditor challenges a figure in an AI-assisted forecast. The discipline
that lets the team trace that number back through its transformations to its source is:
- A. Temperature control.
- B. Data lineage. ✅
- C. Fine-tuning.
- D. Prompt patterns.

*Rationale:* Lineage traces where a data point came from and how it was transformed — essential for
auditability when an AI-influenced number is challenged (13.2.3). Temperature is a randomness setting;
fine-tuning specialises a model; prompt patterns shape instructions — none provides traceability to source.

### Self-check — KA 13.2

1. Name four data-quality dimensions. *(Accuracy, completeness, consistency, timeliness, validity,
   uniqueness.)*
2. Why is data lineage essential for AI in controls? *(Auditability — an AI-influenced number must be traceable
   to source when challenged.)*

---

## Knowledge Area 13.3 — Prompting and working with generative AI

*Topics: 13.3.1 what a good prompt contains · 13.3.2 prompt patterns · 13.3.3 iterative refinement and
verification · 13.3.4 guardrails.*

### 13.3.1 What a good prompt contains

**The principle.** Working with GenAI is a skill: the quality of the output depends heavily on the **prompt**.
An effective professional prompt typically supplies: **role/context** (who the model is acting as, and the
situation), a **clear task**, the **input/data** (or a RAG reference), the **desired format**, and any
**constraints** (tone, length, what to exclude). Vague prompts get vague, generic answers; specific,
context-rich prompts get useful ones.

### 13.3.2 Prompt patterns

**The patterns.** A few reusable patterns cover most controls tasks:

- **Extraction** — "From the attached contract, extract the payment terms, retention %, and LD rate as a
  table."
- **Analysis** — "Given this cost data, identify the three largest adverse variances and their likely drivers."
- **Drafting** — "Draft a variance narrative for this control account: numbers below; tone factual;
  ≤150 words."
- **Summarisation** — "Summarise this 40-page report into a one-page exception summary for a project board."
- **Transformation** — "Convert this raw cost extract into the standard monthly report format."

Giving **examples** (a sample of the desired output) and **context** (definitions, the audience) sharpens each.

### 13.3.2b A worked prompt-pattern library

**Extraction.** *The prompt:*

> "Acting as a quantity surveyor, extract from the attached subcontract (ref SC-014) the payment terms,
> retention %, defects liability period and LD rate, as a four-row table with the clause reference for each.
> If a term is absent, return 'not found' — do not infer."

*Good output:* a four-row table in which every value carries a real clause reference or an explicit "not
found". *Verification:* open each cited clause and check each extracted value against the source; reject any
value without a grounded reference.

**Analysis.** *The prompt:*

> "Acting as a project cost engineer, analyse the attached month-end extract for control account CA-210.
> Identify the three largest adverse cost variances, quantify each in USD and as % of budget, and state a
> likely driver only where the data supports it. Format: a table plus ≤80 words of commentary; flag anything
> you cannot substantiate."

*Good output:* a ranked variance table whose figures tie to the extract, with drivers tied to evidence and
unsupported points flagged. *Verification:* recompute the figures — the variances and percentages — from the
source extract before accepting any driver claim.

**Drafting.** *The prompt:*

> "Acting as a cost engineer reporting to a project board, draft a ≤120-word variance narrative for control
> account CA-210 from the data below. State the CV and SV, attribute the cost variance between rate and usage,
> and note the recovery action. Factual tone; no speculation beyond the data. Data: PV 480,000; EV 455,000;
> AC 492,000; rate variance −22,000; usage variance −15,000; recovery action: revised lift sequence."

*Good output:* a tight, factual narrative whose every figure and cause traces to the supplied data.
*Verification:* verify the causal claims against the variance analysis itself (and recompute the CV/SV) before
the narrative is used.

**Summarisation.** *The prompt:*

> "Acting as a PMO analyst, summarise the attached 40-page monthly project report into a one-page exception
> summary for the project board: out-of-tolerance items only, each with its figure, cause and action. Carry
> forward every caveat and stated assumption. Audience: non-specialist board members; no new claims."

*Good output:* a one-page exception summary in which every figure, action and caveat traces to the source
report. *Verification:* confirm nothing material was dropped — especially the caveats and qualifying
assumptions — by checking the summary against the source's exceptions and caveats.

**Transformation.** *The prompt:*

> "Acting as a cost controller, convert the attached raw ERP cost extract into the standard monthly report
> format (columns: WBS, cost element, budget, actuals, commitments, variance). Preserve every row; map codes
> using the attached lookup; and report the row counts and column totals before and after conversion."

*Good output:* the standard-format table with an unchanged row count and totals that match the raw extract.
*Verification:* reconcile the totals (and row counts) before and after the transformation, and investigate any
difference before the output is used.

These five entries are starting points, not scripts — the professional adapts the role, data reference, format
and constraints to the task, the audience and the organisation's standards. However adapted, every pattern
ends at the same gate: verification against source before the output is used (13.3.3).

**Six further worked patterns.** The five shapes above cover the everyday tasks; the entries below extend the
library into the judgement-heavy corners of the discipline — challenging a forecast, auditing an artefact
against a standard, and turning a completed control sheet into decision-ready prose. Each prompt carries the
full 13.3.1 anatomy — role, inputs, constraints, output format — plus an explicit **verification instruction**
written into the prompt itself, and each ends where every pattern ends: at the professional's check (13.3.3).

**Red-team challenge — "attack my EAC".** Used *before* a forecast is tabled, not after: an `EAC` is only as
good as its assumptions (KA 6.3.2), and the cheapest place to find a weak one is in private, before the board
does (KA 6.3.3; 13.5.3). This is a critique task, not a forecasting one — the model is asked to argue against
the forecast, and it returns questions, never a number.

```
Acting as an independent cost-assurance reviewer, challenge my forecast for control account CA-310. Inputs:
my EAC, the method used (EAC = AC + (BAC − EV) ÷ CPI), and my stated assumptions — steel escalation settles
by Q3; productivity recovers to plan; no further design change. For each assumption, state the strongest
reason it may not hold, what evidence in the cost data would falsify it, and which EAC method (KA 6.3.2) its
failure would point to instead. Do not propose a new EAC figure. Output: a table — assumption | challenge |
falsifying evidence | alternative method. Close by listing any data you would have needed but were not given.
```

*What to verify before use:* every challenge is a question to investigate, not a finding — test each against
the actual cost data, and the `EAC` that survives is selected and owned by the professional (KA 6.3.3): AI proposes; the professional verifies, decides and remains accountable.

**Schema-bound extraction — contract clauses into a fixed schema.** Used when the destination is a governed
artefact with fixed columns — a contract register or obligations tracker — where the freeform extraction
pattern above would return whatever table shape the model chose. The schema is dictated, every value must cite
its clause, and the task is RAG-grounded (13.1.4) so the model answers from the contract, not its training.

```
Acting as a commercial manager, populate the fixed schema below from the attached subcontract (ref SC-021)
only — use no outside knowledge. One row per field: field | value | clause reference. Fields, in this order:
payment terms; retention %; defects liability period; LD rate and cap (KA 7.2.3); variation-valuation rule
(KA 7.2.2); bond and guarantee requirements (KA 7.2.4). Rules: a value you cannot tie to a clause number is
recorded as 'not found' — never infer; do not add, rename or reorder fields. Output as a pipe-delimited
table ready for the contract records (KA 7.2.1), and state which fields returned 'not found'.
```

*What to verify before use:* open every cited clause and check the value against the source; reject any row
without a grounded reference; and route entitlement-bearing terms (KA 7.2.2) through legal review before the
register is updated.

**Critique against a standard — risk-register quality check.** Registers drift: statements decay into vague
labels, responses lose their costs, review dates go stale. The Toolkit 12.T.1 column standard is the yardstick,
and the model's job is critique only — flagging defects entry by entry, never rewriting a risk it does not own.

```
Acting as a risk-management reviewer, assess the attached risk register against the Toolkit 12.T.1 column
standard. For each entry, flag: statements not in cause–event–effect form (KA 12.2.1); missing or
non-numeric probability, impact or EMV values; responses with no stated cost (KA 12.2.4); absent residual
P/I; review dates missing or more than a quarter old (KA 12.2.5); entries without a live owner. Do not
rewrite any entry; do not add or delete risks. Output: one finding per line — risk ID | column | defect |
12.T.1 rule breached — then a three-line conformance summary. State the number of entries checked.
```

*What to verify before use:* confirm each flag against the register itself — a model can miss defects as well
as invent them — and the corrections, like the risks, belong to the risk owners (KA 12.2.1), not the reviewer
and not the model.

**Sheet-to-narrative — schedule health for a decision-maker.** Used when a completed 10.T.1 health-check sheet
must become prose for the baseline-acceptance decision (Advanced 10.A.1): the classic numbers-to-narrative
task (KA 4.3.3; 13.5.5). The model drafts the story the sheet already tells; the acceptance recommendation is
not delegated with it.

```
Acting as a planning engineer reporting to a project manager, draft a ≤150-word narrative from the completed
schedule health-check sheet below (format: Toolkit 10.T.1). State which checks passed and which failed, what
each failed line means for the schedule's ability to recalculate (KA 10.2), and close with the decisions the
sheet could support — accept, accept with conditions, return for repair — presented as options, not a
recommendation. Use only the sheet; do not soften a failed line or omit one. Sheet: [paste 10.T.1 rows].
```

*What to verify before use:* read the narrative against the sheet line by line — no failed check dropped or
softened, no pass invented — and make the acceptance decision yourself: the sheet informs it, the model drafts
it, the planner owns it (Advanced 10.A.1).

**Minutes-to-actions extraction.** Used after any progress or risk review: actions buried in prose minutes die
quietly, and the monitoring cycle (KA 8.4) runs on a tracked list with owners and dates. This pairs naturally
with a governed transcription tool (13.4.2b) — and the confidentiality guardrail (13.2.5) applies to a meeting
record as much as to any contract.

```
Acting as a PMO analyst, extract every action from the attached progress-meeting minutes (Northwind monthly
review, 12 June) into a table: action (original wording) | owner | due date | source paragraph. Rules: an
owner or date the minutes do not state is recorded as 'not stated' — never guess a name or infer a date;
decisions and discussion points are not actions unless someone is to do something. Close with a count of
actions found and a list of any passages that may contain an action but are ambiguous.
```

*What to verify before use:* check the table against the minutes, resolve every 'not stated' with the meeting
chair, and circulate only once each owner has confirmed the action — an unconfirmed action list controls
nothing (KA 8.4).

**Gap-flagged drafting — a claim-substantiation skeleton.** A claim must be notified and substantiated —
cause, effect, quantum — from contemporaneous records (KAs 7.2.1, 7.2.2). The model can organise the records
into that structure and expose the holes; it cannot create evidence, and a gap found before submission is
curable in a way a gap found by the other side is not.

```
Acting as a quantity surveyor preparing a claim file for internal review, assemble a claim-substantiation
skeleton from the attached event records only (site diaries, instructions and correspondence for the June
flooding event). Structure: three sections — cause, effect, quantum (KA 7.2.2) — with each record reference
placed under the section it supports. Where a section lacks support, write 'GAP:' and state what evidence is
missing. Do not draft entitlement arguments, estimate quantum figures, or infer facts not in the records.
Output: the three-section skeleton, then a numbered gap list.
```

*What to verify before use:* confirm every record reference against the file, treat the gap list as the work
plan, and remember the skeleton is not the claim — entitlement is argued, quantified and owned by the
professional, with legal review before any notice is issued (KA 7.2.2).

Six patterns, one gate: the model challenges, extracts, critiques and drafts, but the forecast, the register,
the baseline decision, the action list and the claim remain the professional's — verified against source
before use (13.3.3).

### 13.3.3 Iterative refinement and verification

**The principle.** GenAI is used **iteratively**: prompt, review, refine. And — the non-negotiable step — every
output is **verified** before use. For a controls professional this means: check figures against source
(a model can miscalculate or fabricate), check extracted data against the document, check a narrative's causal
claims against the actual variance analysis. The verification step is where "AI proposes; the professional verifies, decides and remains accountable" becomes concrete.

**Worked workflow 13.3.3 — raw cost data → verified variance commentary.**

1. **Input.** A control account's monthly figures (`PV`, `EV`, `AC`, variances).
2. **Prompt.** "Acting as a project cost engineer, draft a ≤120-word variance narrative for this control
   account. State the CV and SV, attribute the cost variance between rate and usage using the data below, and
   note the recovery action. Factual tone; no speculation beyond the data. Data: …"
3. **AI step.** The model returns a first-draft narrative with the variances and an attribution.
4. **Verification (the professional's step).** Recompute `CV`/`SV` and the rate/usage split (Domain 4, KA 4.2)
   against source; correct any figure the model got wrong; confirm the attributed cause matches the actual
   variance analysis; remove any claim the data does not support.
5. **Output.** A verified narrative — drafted in seconds, **owned** by the professional who checked it. The
   time saved is real; the accountability is undiminished.

**Worked workflow 13.3.3b — extracting contract terms, then verifying.**

1. **Input.** A 180-page construction subcontract — a document, so the task is RAG-grounded (13.1.4): the
   model answers from the contract itself, not from its training.
2. **Prompt.** "Acting as a commercial manager, from the attached subcontract extract, as a table: payment
   terms, retention %, liquidated-damages rate, and any variation-pricing clause — with the clause reference
   for each. If a term is absent, say 'not found'; do not infer."
3. **AI step.** The RAG-grounded model returns a table of the four terms, each with its clause reference.
4. **Verification (the professional's step).** Open each cited clause and confirm the extracted value against
   the document; **reject** any term the model could not ground to a clause; and have legal review any
   entitlement-bearing term (Domain 7). A model can misread or fabricate a clause — the extraction is a
   draft, not a fact.
5. **Output.** A verified terms table, owned by the commercial manager, feeding the contract register
   (Domain 7).

This is why grounding (RAG) and citation-to-source matter — an ungrounded extraction invites hallucinated
clauses. **AI proposes; the professional verifies, decides and remains accountable.**

### 13.3.4 Guardrails

**The principle.** **Guardrails** are the rules of safe use: **never** paste confidential/personal data into
ungoverned tools (13.2.5); **always** verify figures and citations; **never** present AI output as fact without
checking; **disclose** AI assistance where required; and **keep the audit trail** of what AI produced and who
approved it (13.6). Guardrails are what let an organisation get AI's benefits without its risks.

### Key terms — KA 13.3

| Term | Meaning |
|---|---|
| **Prompt** | The instruction/context/data given to a GenAI model. |
| **Prompt patterns** | Reusable shapes: extraction, analysis, drafting, summarisation, transformation. |
| **Iterative refinement** | Prompt → review → refine. |
| **Verification** | Checking every AI output against source before use. |
| **Guardrails** | Rules of safe use (confidentiality, verification, disclosure, audit trail). |

### Sample MCQs — KA 13.3

**MCQ 13.3-A `[13.3.3 · Analysis]`** The single non-negotiable step after a GenAI model drafts a variance
narrative is to:
- A. Publish it immediately to save time.
- B. Increase the temperature.
- C. Verify the figures and causal claims against source before use. ✅
- D. Delete the source data.

*Rationale:* Verification is the step that makes "AI proposes; the professional verifies, decides and remains accountable" real — a model can
miscalculate or over-claim. Publishing unverified output, changing temperature, or deleting source all fail the
principle.

**MCQ 13.3-B `[13.3.1 · Recall]`** Which most improves a professional GenAI prompt?
- A. Making it as short and vague as possible.
- B. Supplying role/context, a clear task, the data, the desired format and constraints. ✅
- C. Omitting the audience.
- D. Requesting maximum creativity for factual tasks.

*Rationale:* Context-rich, specific prompts yield useful output; vagueness yields generic output. For factual
tasks, low creativity (temperature) is preferred.

**MCQ 13.3-C `[13.3.2 · Application]`** "Convert this raw cost extract into the standard monthly report
format" is an instance of which prompt pattern?
- A. Extraction.
- B. Summarisation.
- C. Transformation. ✅
- D. Analysis.

*Rationale:* Converting content from one format to another is the transformation pattern. Extraction pulls
specified values out of a source; summarisation condenses; analysis explains variances and drivers — none of
them reformats a dataset wholesale.

**MCQ 13.3-D `[13.3.4 · Analysis]`** To meet a deadline, an analyst pastes a confidential subcontract into a
public AI tool to extract its terms. The primary guardrail breached is:
- A. Iterative refinement.
- B. Desired-format specification.
- C. Temperature control.
- D. Confidentiality — sensitive data must never enter ungoverned tools. ✅

*Rationale:* The confidentiality guardrail (13.2.5, 13.3.4) is absolute: pasting sensitive data into an
ungoverned public tool loses control of it, whatever the time pressure — a governed tool is the remedy.
Refinement, temperature and format are prompt-craft matters, not the governance breach at issue.

**MCQ 13.3-E `[13.3.1 · Application]`** A commercial manager must extract retention % and the LD rate from a
subcontract for the contract register. Which prompt best follows the domain's prompt discipline?
- A. "Acting as a commercial manager, extract from the attached subcontract the retention % and LD rate as a
  two-row table with the clause reference for each; if a term is absent, return 'not found' — do not infer." ✅
- B. "Tell me everything important about this contract."
- C. "Extract the retention % and LD rate; if either is missing, estimate a typical market value."
- D. "Be as creative as possible and summarise the contract's vibe."

*Rationale:* A supplies the full anatomy of 13.3.1 — role, task, data reference, format, constraints — plus the
grounding rule of 13.3.2b: cite the clause, and return 'not found' rather than infer. B is the vague prompt
that gets a vague answer; C invites the model to fabricate a value for an entitlement-bearing term; D applies
creativity to a factual extraction, the opposite of the low-temperature, verified norm.

**MCQ 13.3-F `[13.3.2b · Application]`** In the red-team "attack my EAC" pattern, the model's output must
**not** contain:
- A. The strongest reason each assumption may not hold.
- B. The evidence that would falsify each assumption.
- C. A list of data it needed but was not given.
- D. A proposed new EAC figure. ✅

*Rationale:* The red-team challenge is a critique task, not a forecasting one — the model returns questions,
never a number, and the `EAC` that survives is selected and owned by the professional (13.3.2b). A, B and C
are exactly what the pattern asks the model to produce.

**MCQ 13.3-G `[13.3.3 · Application]`** A minutes-to-actions extraction returns an action table in which one
action carries an owner's name that appears nowhere in the minutes. The correct handling is to:
- A. Keep the name — the model probably knows the team.
- B. Circulate the list immediately to save time.
- C. Replace it with 'not stated' and resolve the owner with the meeting chair before circulating. ✅
- D. Delete the action from the list.

*Rationale:* An owner the minutes do not state is recorded as 'not stated' — never a guessed name — and every
'not stated' is resolved with the chair before the list circulates (13.3.2b): an invented owner is a
hallucination entering the action register. A trusts an ungrounded claim; B circulates unverified output; D
throws away a real action instead of verifying it.

### Self-check — KA 13.3

1. List the components of a good professional prompt. *(Role/context, task, data, format, constraints.)*
2. State two guardrails for GenAI in controls. *(No confidential data in ungoverned tools; always verify
   figures/citations; disclose; keep the audit trail.)*

---

## Knowledge Area 13.4 — AI tool categories for project controls & PM

*Topics: 13.4.1 the category map · 13.4.2 assistants, RAG and analysis tools · 13.4.3 domain and platform AI ·
13.4.4 choosing a category.* Vendor-neutral; representative tools named without fabricated features; note that
features change.

### 13.4.1 The category map

**The principle.** AI reaches project controls through several **tool categories**, each with typical inputs/
outputs and governance notes. The categories matter more than any specific product, because products change
rapidly; a professional chooses the **category** that fits the task, then a governed tool within it.

| Category | What it does | Representative tools (illustrative) | Governance note |
|---|---|---|---|
| **General LLM assistants** | Draft, extract, summarise, analyse text | Assistant-class tools (e.g. Claude, ChatGPT, Gemini, Microsoft Copilot) | Confidentiality; verify outputs |
| **Document / RAG & knowledge** | Answer over your documents, grounded | Enterprise RAG/search platforms | Source access control; citations |
| **Spreadsheet / data-analysis AI** | Analyse/transform tabular data, formulas | AI features in spreadsheet/analysis tools | Check computations |
| **BI & analytics AI** | Dashboards, NL queries over data | AI features in BI/analytics platforms | Definition/consistency of metrics |
| **Scheduling & PM-suite AI** | Schedule assist, logic checks, risk | AI features in PM/planning platforms | Hidden constraints; validate logic |
| **Risk & forecasting / ML platforms** | Predict, simulate, score | ML/forecasting/simulation platforms | Model data quality; explainability |
| **RPA / process mining** | Automate flows; reconstruct process | Process-mining/RPA platforms | Control-breach detection scope |
| **Contract analytics / CLM AI** | Extract/analyse contract terms/claims | Contract-analytics/CLM tools | Legal review of extractions |
| **Transcription / meeting assistants** | Notes, actions from meetings | Meeting-assistant tools | Confidentiality; accuracy of actions |
| **AI coding / automation** | Scripts/automation for controls tasks | AI coding assistants | Test/verify generated code |

> **Fig 13.4.1 — Capability-vs-category matrix.** *Caption:* which category fits which controls need.
> *Underlying data:* the table above mapped to controls tasks. *Render-ready description:* a matrix, rows =
> controls needs (extract, draft, forecast, schedule, reconcile, analyse), columns = tool categories; cells
> shaded where a category fits a need (strong = brand blue, partial = light). A footnote: "capabilities evolve
> — verify current features." *Animation storyboard (digital-only):* selecting a need highlights the fitting
> categories.

### 13.4.2 Assistants, RAG and analysis tools

**The core three for controls.** Most day-to-day AI value comes from three categories: **general LLM
assistants** (drafting, extraction, analysis of text), **document/RAG** (grounded answers over the
organisation's contracts, standards and project records), and **spreadsheet/data-analysis AI** (working over
the tabular cost/schedule data controls lives in). A professional who is fluent in these three, with
verification and guardrails, covers a large share of practical use.

### 13.4.2b The ten categories in depth

**General LLM assistants.** *What it does:* drafts, summarises, extracts, classifies and analyses text
conversationally, applying the prompt patterns of 13.3.2 to whatever the professional supplies. *Typical
inputs → outputs:* a prompt plus pasted or attached text/data → a drafted narrative, summary, extracted table
or first-cut analysis. *Where it earns its keep in controls:* first-draft variance narratives and exception
commentary (Domain 4), drafting estimate bases and assumptions (Domain 3), and condensing long reports for
boards. *Limits & governance:* hallucination is the defining risk — a fluent draft can contain fabricated
figures, causes or citations — so every figure and claim is verified against source (13.3.3), and no
confidential data enters an ungoverned tool (13.2.5).

**Document / RAG & knowledge.** *What it does:* answers questions over the organisation's own documents —
contracts, standards, procedures, project records — grounding each answer in retrieved, cited source material
(13.1.4). *Typical inputs → outputs:* a question plus a governed document set → a cited answer or extracted
terms table. *Where it earns its keep in controls:* contract-term and claims queries across a portfolio
(Domain 7) and "what does the policy or standard require?" checks (Domains 1–2). *Limits & governance:*
source-access control is the category-specific risk — the retrieval layer must respect document permissions,
or the assistant answers from material a user should not see; a stale corpus produces confidently outdated
answers, so sources are curated and citations opened.

**Spreadsheet / data-analysis AI.** *What it does:* works over tabular data — generating formulas, cleaning
and transforming extracts, running analyses and explaining results — inside the tools controls already lives
in. *Typical inputs → outputs:* a cost/schedule/quantity table plus an instruction → a transformed table,
formula, chart or analytical summary. *Where it earns its keep in controls:* preparing month-end extracts and
reconciliations (Domain 5), variance breakdowns and EVM working (Domains 4, 6), and normalising estimating
data (Domain 3). *Limits & governance:* generated formulas and computations can be plausibly wrong — a
mis-ranged formula or silent unit error propagates downstream — so computations are checked against
hand-worked cases before the output is trusted.

**BI & analytics AI.** *What it does:* builds and augments dashboards, answers natural-language queries over
governed datasets, and surfaces trends and anomalies in performance data. *Typical inputs → outputs:* a
governed dataset plus a natural-language question → a chart, dashboard element or narrative insight. *Where it
earns its keep in controls:* performance dashboards, out-of-tolerance detection and NL querying for
decision-ready reporting (Domain 4), drawing on cost and earned-value data (Domains 5, 6). *Limits &
governance:* metric-definition drift is the category's quiet failure — an NL query answered from a subtly
different definition of "cost" or "% complete" than the report's — so metric definitions are governed centrally
(13.2.3) and AI-generated figures reconciled to the controlled ones.

**Scheduling & PM-suite AI.** *What it does:* assists schedule development, checks network logic (missing
links, dangling activities, excess constraints and lags) and predicts slippage from progress data within
PM/planning platforms. *Typical inputs → outputs:* WBS, activities, logic and progress → a proposed or
health-checked schedule and delay warnings. *Where it earns its keep in controls:* schedule quality and logic
checks, and progress-based delay prediction (Domain 10), feeding time-cost integration (Domain 6). *Limits &
governance:* an AI-assisted schedule can embed hidden constraints or unrealistic durations that make it look
achievable while quietly fixing dates — so the professional inspects constraints, validates durations and
re-identifies the critical path before relying on it.

**Risk & forecasting / ML platforms.** *What it does:* learns from historical and current project data to
predict outcomes (cost, delay, overrun likelihood), score and rank risks, and run or interpret simulations.
*Typical inputs → outputs:* governed historical/current data and a quantified register → forecasts, risk
scores, simulation distributions and driver analyses. *Where it earns its keep in controls:* predictive EAC
and early warning (Domain 6), estimate ranges (Domain 3), and Monte Carlo-based contingency (Domain 12).
*Limits & governance:* explainability is the category-specific test — a prediction that cannot be explained
cannot be defended to a board or an auditor — so the professional demands driver visibility, checks training-
data representativeness (13.2), and owns the number.

**RPA / process mining.** *What it does:* robotic process automation executes defined, repetitive workflows
across systems; process mining reconstructs how a process actually ran from system event logs. *Typical
inputs → outputs:* transaction and event data plus process rules → automated matching/postings and a map of
actual process flows with deviations flagged. *Where it earns its keep in controls:* three-way match
automation (Domain 11), month-end reconciliation flows (Domain 5), and detecting control breaches such as
approvals bypassed. *Limits & governance:* over-wide matching tolerances are the classic failure — set loosely
to reduce exceptions, they wave through the very mismatches the control exists to catch — so tolerances are
owned, justified and periodically re-tested.

**Contract analytics / CLM AI.** *What it does:* extracts and analyses terms, obligations and dates across
contract sets, and supports variation and claims review within contract-lifecycle-management workflows.
*Typical inputs → outputs:* contracts, variations and correspondence → extracted terms tables, obligation and
date registers, and flagged risk language. *Where it earns its keep in controls:* portfolio-wide term
extraction, notification-window tracking and claims analysis (Domain 7), feeding commercial valuation and
revenue treatment (Domains 1–2). *Limits & governance:* extraction is a draft, not a legal position —
entitlement-bearing terms require legal review before they move a commercial position, and every extracted
term is confirmed against its cited clause (13.3.3b).

**Transcription / meeting assistants.** *What it does:* transcribes meetings and drafts minutes, summaries,
decisions and action lists from the discussion. *Typical inputs → outputs:* a recorded or live meeting → a
transcript, summary, decision log and proposed action register. *Where it earns its keep in controls:*
capturing progress-review actions and decisions that feed reporting (Domain 4), and surfacing risks and issues
raised in discussion for the register (Domain 12). *Limits & governance:* accuracy of the captured actions is
the specific risk — a misheard owner, date or commitment propagates into the action register as if agreed — so
actions are confirmed with owners before circulation, and recording/confidentiality rules are respected before
any meeting is captured.

**AI coding / automation.** *What it does:* generates and explains scripts, queries and small automations —
data transformations, report assembly, recurring checks — from natural-language descriptions. *Typical
inputs → outputs:* a task description plus sample data → working code or query, with an explanation. *Where it
earns its keep in controls:* automating repetitive data preparation and reconciliation steps (Domain 5),
recurring EVM and variance calculations (Domains 4, 6), and one-off analyses that would otherwise be manual.
*Limits & governance:* generated code can be subtly wrong while looking correct — so it is tested against
known cases before use, reviewed like any other code, and never run against production data untested; the
professional owns what the automation does.

Across all ten, capabilities evolve quickly: the professional validates a tool's **current** features rather
than assuming vendor claims or last year's experience. The category-to-task fit (13.4.4) and the verification
discipline (13.3.3, 13.6) apply unchanged across every category — whatever the tool proposes, the professional
verifies, decides and remains accountable.

### 13.4.3 Domain and platform AI

**The principle.** Beyond general tools, AI is increasingly **embedded** in the platforms controls already use
— PM/planning suites (schedule assistance, logic checks), BI tools (natural-language querying), ERP (anomaly
detection), and contract/CLM systems (clause extraction). These bring AI to the data *in place*, which helps
governance (data stays in the platform) but requires the same verification discipline. **Features change
rapidly** — a professional validates current capability rather than assuming a claimed feature.

### 13.4.4 Choosing a category

**The judgement.** Choosing well means matching the **task** to the **category** and the **governance need**:
a language task → an assistant; a "what do our documents say?" task → RAG; a tabular-analysis task →
data-analysis AI; a prediction → an ML/forecasting platform; a process-integrity question → process mining.
Over-reaching (using a general LLM to do precise arithmetic better done in a spreadsheet, or to answer a
document question without RAG grounding) is a common error that invites hallucination.

### Key terms — KA 13.4

| Term | Meaning |
|---|---|
| **Tool category** | A class of AI tool (assistant, RAG, analysis, BI, PM-suite, ML, RPA, CLM, meeting, coding). |
| **Embedded AI** | AI features within the platforms controls already uses. |
| **Category-to-task fit** | Matching the task and governance need to the right category. |

### Sample MCQs — KA 13.4

**MCQ 13.4-A `[13.4.4 · Analysis]`** To answer "what retention and LD terms do our current contracts contain?"
the best-fitting category is:
- A. A general LLM with no documents.
- B. A meeting assistant.
- C. Document / RAG grounded in the contract set. ✅
- D. RPA.

*Rationale:* The question is a grounded document query — RAG over the contracts gives cited, source-based
answers. A general LLM without the documents risks hallucination; the others do not fit.

**MCQ 13.4-B `[13.4.3 · Recall]`** A stated reason to note that "features change" when naming AI tools is:
- A. Tools never improve.
- B. To avoid using AI.
- C. All tools are identical.
- D. Capabilities evolve rapidly, so a professional validates current features rather than assuming claims. ✅

*Rationale:* AI capabilities change quickly; responsible use validates current capability. The other options
are false or contrary to the domain's stance.

**MCQ 13.4-C `[13.4.4 · Analysis]`** Asking a general LLM assistant to perform precise multi-step arithmetic
over a large cost table, rather than using spreadsheet/data-analysis AI, is best described as:
- A. Good practice — one tool for everything.
- B. Over-reaching: a category-to-task mismatch that invites plausible but wrong computation. ✅
- C. A governance requirement.
- D. RAG grounding.

*Rationale:* Matching the task to the category is the judgement of 13.4.4; using a general LLM for precise
tabular arithmetic is the named over-reach error. It is neither good practice nor a governance requirement,
and it is unrelated to RAG grounding.

**MCQ 13.4-D `[13.4.2b · Recall]`** The category-specific governance risk of document/RAG tools is that:
- A. The retrieval layer may not respect document permissions, and a stale corpus produces confidently
  outdated answers. ✅
- B. They cannot cite sources.
- C. They work only on tabular data.
- D. They eliminate hallucination entirely.

*Rationale:* Source-access control and corpus currency are the RAG-specific risks (13.4.2b) — sources must be
curated and citations opened. RAG tools do cite sources, work over documents (not tables), and reduce rather
than eliminate hallucination.

**MCQ 13.4-E `[13.4.4 · Application]`** A portfolio office wants to predict, from its governed historical
data, which of its live projects are most likely to overrun. The best-fitting tool category is:
- A. A general LLM assistant.
- B. A risk & forecasting / ML platform. ✅
- C. A transcription / meeting assistant.
- D. Document / RAG.

*Rationale:* Predicting outcomes from historical data is the risk & forecasting / ML category's job (13.4.1,
13.4.2b) — with explainability and training-data representativeness as its governance tests. A general LLM
over-reaches on a prediction-from-data task; a meeting assistant captures discussions; RAG answers document
questions.

**MCQ 13.4-F `[13.4.2b · Application]`** A director's natural-language query to the BI assistant returns a
"% complete" figure that differs from the controlled monthly report. The category-specific failure most likely
at work is:
- A. Metric-definition drift — the query was answered from a subtly different definition than the report's. ✅
- B. A missing goods-receipt note.
- C. Too low a temperature setting.
- D. An expired tool licence.

*Rationale:* Metric-definition drift is the named quiet failure of BI & analytics AI (13.4.2b): the remedy is
centrally governed metric definitions (13.2.3) and reconciling AI-generated figures to the controlled ones. B
belongs to the P2P cycle, not BI; C affects randomness, not metric definitions; D would stop the tool, not
skew its answer.

**MCQ 13.4-G `[13.A.6 · Application]`** A controls team assembles a **monthly** cost pack for the board. Of
the three integration patterns, the proportionate choice is:
- A. Manual export/import — spreadsheets and email are simplest.
- B. API integration — the freshest data is always best.
- C. No integration — retype the figures each month.
- D. Batch ETL/file transfer — a scheduled, auditable extract matches the monthly decision cadence. ✅

*Rationale:* The right pattern is set by the decision cadence the data serves (13.A.6): a monthly pack does
not need a real-time feed, and batch ETL is robust and auditable. A is fragile, unlogged and where version
chaos lives; B pays real engineering and governance cost for freshness the decision cannot use; C is the
manual failure mode, not a pattern.

### Self-check — KA 13.4

1. Name the three tool categories that cover most day-to-day controls value. *(General LLM assistants;
   document/RAG; spreadsheet/data-analysis AI.)*
2. Why match task to category rather than reach for one tool? *(Over-reaching — e.g. an LLM for precise
   arithmetic or an ungrounded document question — invites hallucination.)*

---

## Knowledge Area 13.5 — AI applied across the project-controls lifecycle *(the heart of the domain)*

*Topics: 13.5.1 the pattern · 13.5.2 estimating & budgeting · 13.5.3 forecasting & EVM/EAC · 13.5.4 cost
control & extraction · 13.5.5 scheduling · 13.5.6 agile delivery · 13.5.7 contracts & commercial · 13.5.8
reporting & performance · 13.5.9 risk · 13.5.10 financial reporting & standards.* Each is a hands-on workflow:
**input → AI step → the professional's verification/decision → output**.

### 13.5.1 The pattern

**The workflow shape.** Every application below follows one shape — the operational form of "AI proposes; the professional verifies, decides and remains accountable":

```
Input (governed data) → AI step (draft/extract/forecast/detect) → Professional verification/decision → Output (owned)
```

The AI step **accelerates**; the verification step **assures**; the professional **owns** the result. The
sub-sections apply this shape to each earlier domain, going deeper than the per-chapter "AI in this domain"
boxes.

### 13.5.2 Estimating & budgeting (Domain 3)

- **Workflow.** Input: historical project data + the new project's parameters → AI step: a parametric/ML model
  proposes an estimate and a range, and generates scenarios → Verification: the estimator checks the analogues'
  representativeness, the driver logic, and the class/range (Domain 3, KA 3.2), adjusting for known differences
  → Output: an estimate with a documented basis (BoE) the professional owns.
- **Value & limit.** Speeds estimating and improves consistency; limited by data representativeness — a model
  trained on dissimilar projects misleads.

**Worked workflow 13.5.2b — a parametric check estimate from history.**

1. **Input.** A governed history of 12 analogous completed buildings with normalised cost/m² (13.2.3); the new
   project is 4,800 m².
2. **AI step.** The model derives a rate of USD 2,150/m² — the median of the analogues, adjusted for location
   factors — giving a check estimate `2,150 × 4,800 = USD 10,320,000`, with a model range of ±12 % →
   ~USD 9.08m–11.56m.
3. **Verification (the professional's step).** The estimator confirms the analogues are genuinely comparable
   (sector, spec level, year basis), checks the location adjustment, and reconciles the parametric figure
   against the bottom-up estimate before presenting both with their class and range (Domain 3, KA 3.2).
4. **Output.** A check estimate the estimator owns — used to **challenge**, not replace, the bottom-up figure.
   **AI proposes; the professional verifies, decides and remains accountable.**

### 13.5.3 Forecasting & EVM/EAC (Domains 3, 6)

- **Workflow.** Input: `PV`/`EV`/`AC` trends + leading indicators → AI step: a model projects `EAC` and an
  early-warning signal, with driver analysis → Verification: the professional checks the assumption against the
  variance's cause (Domain 6, KA 6.3.3), runs the `TCPI` reality check, and selects/defends the `EAC` → Output:
  a decision-ready forecast (Domain 4, KA 4.3.3) the professional defends.
- **Value & limit.** Predictive `EAC` and early warning are among AI's strongest controls uses; the model
  cannot see the critical path unless given the schedule, and can be confidently wrong — the professional owns
  the number.

**Worked workflow 13.5.3b — an early-warning trigger from a CPI trend.**

1. **Input.** Monthly `CPI` readings 0.97, 0.95, 0.92 on a control account with `BAC` USD 2,000,000, plus
   productivity and supply-chain leading indicators.
2. **AI step.** The model flags a sustained three-period decline and attributes ~70 % of the drift to a falling
   installed-quantity productivity driver; if the trend holds, projected
   `EAC = BAC/CPI = 2,000,000/0.92 = USD 2,173,913`.
3. **Verification (the professional's step).** The professional confirms the driver against site data, checks
   whether the cause is closed or persisting (Domain 6, KA 6.3.3), and decides which `EAC` assumption to
   defend.
4. **Output.** An early escalation with a quantified forecast — raised on the **trend**, months before a single
   bad period would have forced it.

### 13.5.4 Cost control & extraction (Domains 1, 5)

- **Workflow.** Input: invoices/POs/ledger feeds → AI step: auto-code cost to project/WBS/cost element, match
  to the ledger, flag anomalies/duplicates, propose accruals from goods-received-not-invoiced → Verification:
  the professional reviews the coding rules and exceptions, and checks accrual service-dates (Domain 5, KA
  5.2) → Output: coded, reconciled cost with a true cost-to-date.
- **Value & limit.** High value, relatively low risk (Domain 1, KA 1.5); an auto-accrual from a document date
  rather than a service date reproduces a real cut-off error at scale.

**Worked workflow 13.5.4b — month-end auto-coding at scale.**

1. **Input.** 4,200 invoice/PO lines from the ERP month-end feed.
2. **AI step.** The classifier codes 3,780 lines (90 %) to project/WBS/cost element with high confidence,
   routes 420 to an exception queue, and flags 37 probable duplicates totalling USD 214,000.
3. **Verification (the professional's step).** The cost engineer reviews the exception queue and the flagged
   duplicates — not the 3,780 high-confidence lines, which are sampled periodically instead — and confirms
   that accrual proposals use service dates, not invoice dates (Domain 1, KA 1.3.5).
4. **Output.** A coded, reconciled month-end in hours instead of days, with human attention concentrated on
   the 10 % that needs it.

**Worked example 13.5.4c — evaluating an invoice-coding model honestly.**

1. **Setup.** A controls function pilots an ML classifier that proposes cost codes for **1,000 invoices a
   month**. Above a confidence threshold the model auto-codes **800**; an audit of those 800 finds **780
   correct**. The remaining **200** low-confidence invoices route to humans as before. Manual coding takes
   **3 minutes** per invoice; reviewing an auto-coded line takes **0.5 minutes**.
2. **Formula.** `precision = correct auto-codes ÷ total auto-codes`; time saved = baseline minutes − minutes
   with the model.
3. **Substitution.** Precision `780 ÷ 800 = 97.5 %`. Baseline `1,000 × 3 = 3,000` minutes; with the model
   `(800 × 0.5) + (200 × 3) = 400 + 600 = 1,000` minutes — a saving of `3,000 − 1,000 = 2,000` minutes
   (**≈ 33 hours**) a month, a **66.7 %** reduction.
4. **Result.** Adopt, with the human review step retained.
5. **Interpretation.** The honest evaluation names all three numbers — precision at the threshold (97.5 %),
   the residual **20 miscodes** a month that the review step and the reconciliation discipline of KA 1.5.2
   must catch, and the measured (not vendor-claimed) time saving; "AI proposes; the professional verifies, decides and remains accountable"
   is operationalised here as a threshold, a review step and an audit sample, not a slogan.

### 13.5.5 Scheduling (Domain 10)

- **Workflow.** Input: WBS/activities/logic → AI step: propose a schedule, check logic (missing links, dangling
  activities, excess constraints/lags), predict delays from progress → Verification: the professional validates
  the logic and durations and re-identifies the critical path (Domain 10) → Output: a sound, progressed
  schedule.
- **Value & limit.** Strong for logic-checking and delay prediction; an AI schedule can embed hidden
  constraints or unrealistic durations — validate before trusting.

**Worked workflow 13.5.5b — a logic-check sweep before baselining.**

1. **Input.** A 1,240-activity contractor schedule submitted for baseline acceptance.
2. **AI step.** The checker flags 37 activities with missing predecessors/successors (dangles), 12 hard
   constraints, and 5 lags longer than 10 days — Advanced 10.A.1's health checks, run by machine.
3. **Verification (the professional's step).** The planner works the exception list — re-logics the dangles,
   justifies or removes each constraint, and replaces the long lags with real activities. On the re-run the
   critical path **moves** and the finish slips 6 days — a slip a hard constraint had been hiding.
4. **Output.** A schedule fit to baseline — the AI found the defects in minutes; the planner decided what each
   one meant (Domain 10, KA 10.4.1).

### 13.5.6 Agile delivery (Domain 9)

- **Workflow.** Input: backlog + Sprint history → AI step: draft/split stories, forecast velocity and release
  completion, detect flow anomalies, draft agile reports → Verification: the professional owns estimates,
  commitments and the scope-change/rebaselining narrative (Domain 9, KA 9.5) → Output: forecasts and reports
  with human accountability.
- **Value & limit.** Useful for backlog and forecasting; a model that treats story points as absolute or
  ignores rebaselining misleads.

**Worked workflow 13.5.6b — a velocity forecast the professional corrects.**

1. **Input.** Five Sprint velocities — 30, 32, 28, 34, 26 points (mean 30) — and a 240-point remaining
   backlog.
2. **AI step.** The model forecasts `240/30 = 8` Sprints, but flags the last two Sprints' decline (34 → 26)
   and widens its range to 7–10 Sprints.
3. **Verification (the professional's step).** The delivery lead checks the cause — the dip was two
   public-holiday Sprints, not a trend — and accepts the central 8 with the range reported, rather than
   letting the model's trend-widening stand unexamined (Domain 9, KA 9.3.3).
4. **Output.** An owned range forecast whose assumptions are known, not merely computed.

### 13.5.7 Contracts & commercial (Domain 7)

- **Workflow.** Input: contracts, variations, claims, valuations → AI step: extract terms, analyse claims/
  variations, flag billing anomalies, reconcile billing to `EV`/IFRS 15 → Verification: the professional (and
  legal, where needed) confirms extractions and entitlement judgements (Domain 7) → Output: verified commercial
  analysis.
- **Value & limit.** Contract analytics saves large amounts of reading; entitlement and recognition judgements
  remain human and legally reviewable.

**Worked workflow 13.5.7b — a claims-exposure sweep across a portfolio.**

1. **Input.** 60 live subcontracts and their correspondence/RFI logs, RAG-grounded (13.1.4).
2. **AI step.** The model surfaces 9 subcontracts with delay-notice language and expiring notification
   windows, and drafts a summary of each potential claim's cause and window date.
3. **Verification (the professional's step).** The commercial manager reads each cited clause and notice,
   confirms the window dates against the contract, and involves legal on the two with material exposure
   (Domain 7).
4. **Output.** No notification window silently missed — the model's value is **coverage** (it reads
   everything); the professional's value is judgement on what matters.

### 13.5.8 Reporting & performance (Domain 4)

- **Workflow.** Input: cost/schedule/risk data → AI step: assemble the dashboard, detect out-of-tolerance
  items, draft exception narratives, answer natural-language queries → Verification: the professional checks
  attribution, framing and caveats (Domain 4, KA 4.3) → Output: an accurate, decision-ready report.
- **Value & limit.** Automated commentary and NL querying speed reporting; a drafted narrative can misattribute
  cause or bury a caveat — the professional signs off.

**Worked workflow 13.5.8b — a natural-language query, decomposed and checked.**

1. **Input.** A director asks the controls assistant, "Why did project 1420's EAC move this month?"
2. **AI step.** The model decomposes the +USD 100,000 movement: +80,000 rate escalation on steel, +40,000
   rework in containment, −20,000 scope removed by an approved variation
   (`80,000 + 40,000 − 20,000 = 100,000`).
3. **Verification (the professional's step).** The professional ties each element to source — the escalation
   to the procurement record, the rework to the NCR log, the variation to the change log (Domain 5,
   KA 5.4.3) — before the answer leaves the room.
4. **Output.** A decision-ready decomposition in seconds, with every number traceable.

### 13.5.9 Risk (Domain 12)

- **Workflow.** Input: project data + analogous histories → AI step: identify candidate risks, score
  probability/impact, run/interpret Monte Carlo, track leading indicators → Verification: the professional
  judges risk reality, response adequacy and the contingency the organisation's appetite requires (Domain 12)
  → Output: a defensible register and contingency.
- **Value & limit.** Speeds identification and simulation; a model under-scoring a tail risk, or contingency set
  by an unexamined algorithm, can leave a project exposed.

**Worked workflow 13.5.9b — simulation-assisted contingency at P80.**

1. **Input.** The quantified risk register, with an EMV sum of USD 185,000 (Domain 12, KA 12.2.3).
2. **AI step.** A Monte Carlo engine runs the register with correlations, returning P50 USD 205,000 and P80
   USD 260,000, and a tornado ranking showing two risks drive most of the spread.
3. **Verification (the professional's step).** The professional sanity-checks the correlation assumptions,
   confirms the two driver risks' probabilities and impacts with their owners, and recommends the P80
   (USD 260,000) as the contingency consistent with the organisation's appetite (Domain 12, KA 12.3.1).
4. **Output.** A defensible, documented contingency — the model did the simulation; the professional owns the
   number.

### 13.5.10 Financial reporting & standards (Domains 1, 2)

- **Workflow.** Input: trial balance, contracts, policies → AI step: draft disclosures, run consistency checks,
  map transactions to standards, propose entries/accruals → Verification: the professional confirms against the
  policy and the standard, and signs off (Domains 1, 2) → Output: verified reporting.
- **Value & limit.** Speeds drafting and consistency-checking; recognition/measurement judgements (IFRS 15
  over-time vs point, IAS 37 provisioning) are human and auditable.

> **Fig 13.5.1 — AI across the controls lifecycle.** *Caption:* the "propose → verify → own" pattern applied to
> every domain. *Underlying data:* the ten workflows above. *Render-ready description:* a wheel with the
> controls lifecycle around it (estimate, budget, forecast, cost-control, schedule, agile, contracts, report,
> risk, financial reporting); each spoke shows the three-step pattern (AI step → verification → owned output)
> in brand blue. *Animation storyboard (digital-only):* selecting a lifecycle stage expands its workflow into
> the three-step pattern with its domain cross-reference.

**Worked assurance example 13.5.10a — an AI-drafted EAC, verified.** An AI model proposes `EAC` = USD
1,180,000 for the master project (Domain 6). The professional's verification: recompute from `AC`/`EV`/`BAC`
and `CPI`/`SPI`; confirm which method the model used and whether its assumption matches the variance cause
(Domain 6, KA 6.3.3); run the `TCPI` reality check; and confirm the figure against the schedule's critical
path. Only then is the `EAC` reported — with its method, assumption and the note that it is AI-assisted and
verified (13.6). The AI saved the assembly; the professional owns the forecast.

**Worked workflow 13.5.10b — a pre-publication consistency sweep.**

1. **Input.** The draft annual-report disclosures and the controls order book.
2. **AI step.** The checker cross-references the remaining-performance-obligation disclosure
   (USD 107,000,000) against the internal order book (USD 109,000,000) and flags the USD 2,000,000 gap.
3. **Verification (the professional's step).** The professional traces it — an agreed-but-unsigned variation
   is in the order book but does not yet qualify for the RPO disclosure (Domain 2, KA 2.2.9) — and documents
   the reconciling item rather than forcing the numbers to agree.
4. **Output.** A disclosure that survives audit because the difference is explained, not hidden.

### Key terms — KA 13.5

| Term | Meaning |
|---|---|
| **Propose → verify → own** | The universal AI-in-controls workflow shape. |
| **Driver analysis** | AI explanation of *why* a metric is moving (e.g. an EAC). |
| **Auto-coding / reconciliation** | AI coding cost and matching it to the ledger. |
| **AI-assisted disclosure/forecast** | AI-drafted output the professional verifies and signs off. |

### Sample MCQs — KA 13.5

**MCQ 13.5-A `[13.5.1 · Recall]`** The universal shape of an AI-in-controls workflow is:
- A. Input → AI step → professional verification/decision → owned output. ✅
- B. AI decides → professional observes.
- C. Professional drafts → AI approves.
- D. AI both drafts and signs off.

*Rationale:* AI proposes; the professional verifies and owns. The other options remove human accountability or
invert the roles.

**MCQ 13.5-B `[13.5.3 · Analysis]`** An AI model outputs an `EAC`. Before reporting it, the professional should
**not**:
- A. Recompute it from `AC`/`EV`/`BAC` and the indices.
- B. Confirm the method's assumption matches the variance cause.
- C. Report it unchecked because the model is advanced. ✅
- D. Run the `TCPI` reality check.

*Rationale:* Reporting an AI figure unchecked violates the governing principle; A, B and D are exactly the
verification steps required.

**MCQ 13.5-C `[13.5.4 · Analysis]`** An AI accrual tool accrues from the invoice date rather than the service
date. This risks:
- A. Nothing.
- B. Violating IFRS 15 only.
- C. Improving cut-off accuracy.
- D. Reproducing a cut-off error at scale (Domain 1, KA 1.3.5). ✅

*Rationale:* Accrual follows the *service* date; keying off the invoice date reproduces a classic cut-off error
across every accrual — the professional must own the accrual logic.

**MCQ 13.5-D `[13.5.7 · Analysis]`** In an AI claims-exposure sweep across 60 subcontracts, the model's
distinctive contribution is:
- A. Deciding entitlement on each claim.
- B. Coverage — it reads everything, surfacing candidates for the professional's judgement. ✅
- C. Replacing legal review of material exposures.
- D. Setting the portfolio contingency.

*Rationale:* The model's value is coverage; judgement on what matters — and entitlement and legal review —
remain human (13.5.7). A and C delegate judgements the workflow reserves to professionals; D belongs to the
risk workflow (13.5.9), not a claims sweep.

**MCQ 13.5-E `[13.5.3 · Application]`** A control account has `BAC` USD 1,500,000 and a sustained `CPI` of
0.96. If the trend holds, the model's projected `EAC = BAC/CPI` is:
- A. USD 1,440,000
- B. USD 1,500,000
- C. USD 1,562,500 ✅
- D. USD 1,687,500

*Rationale:* `EAC = BAC/CPI = 1,500,000/0.96 = USD 1,562,500` (check: `1,562,500 × 0.96 = 1,500,000`). A
multiplies by the `CPI` instead of dividing; B assumes the drift away; D has no basis in the data. The
professional then verifies the assumption against the variance cause and runs the `TCPI` reality check before
reporting (13.5.3).

**MCQ 13.5-F `[13.5.4 · Application]`** An ML classifier auto-codes **900** of a month's **1,200** invoices
above its confidence threshold; an audit of the 900 finds **855** correct. The model's precision at the
threshold is:
- A. 71.25 %
- B. 75 %
- C. 95 % ✅
- D. 5 %

*Rationale:* `Precision = correct auto-codes ÷ total auto-codes = 855 ÷ 900 = 95 %` (13.5.4c). A divides the
correct codes by all 1,200 invoices, including the 300 the model never coded; B is the automation rate
(900 ÷ 1,200), not precision; D is the error rate at the threshold (45 ÷ 900), the complement of the answer.
The 45 residual miscodes are what the review step and reconciliation discipline must catch.

**MCQ 13.5-G `[13.A.7 · Application]`** A duplicate-invoice detector is scored on a golden set of **300**
invoices containing **60** known duplicates. It flags **50** invoices, of which **36** are genuine duplicates.
Its precision and recall are:
- A. Precision 60 %, recall 72 %.
- B. Precision 72 %, recall 60 %. ✅
- C. Precision 83.3 %, recall 83.3 %.
- D. Precision 12 %, recall 16.7 %.

*Rationale:* `Precision = true hits ÷ total flags = 36 ÷ 50 = 72 %`; `recall = true hits ÷ total true cases =
36 ÷ 60 = 60 %` (13.A.7). A swaps the two denominators; C divides flags by true cases (50 ÷ 60), a ratio that
measures neither; D divides both counts by the whole 300-invoice set. At 60 % recall the detector misses 40 %
of duplicates, so it screens but cannot replace the month-end duplicate review.

**MCQ 13.5-H `[13.5.5 · Application]`** An AI logic-check on a contractor schedule flags dangling activities
and a hard constraint. After the planner re-logics the dangles and removes the constraint, the recalculated
finish slips 6 days. The best reading is:
- A. The constraint had been hiding a genuine slip — the repaired schedule is the honest one to take
  forward. ✅
- B. The repair introduced the slip, so the constrained version should be restored to protect the date.
- C. The AI fabricated the defects, since the original schedule showed the earlier date.
- D. Dangling activities are cosmetic and the exercise was unnecessary.

*Rationale:* A hard constraint can fix a date the network no longer supports — removing it reveals, not
creates, the slip (13.5.5b; Domain 10). B restores the concealment; C mistakes a date the constraint
manufactured for evidence against the checker; D ignores that dangles break the schedule's ability to
recalculate. The AI found the defects; the planner decided what each meant.

### Self-check — KA 13.5

1. State the three-step AI workflow and what each step contributes. *(Input → AI step (accelerate) →
   verification (assure) → owned output.)*
2. Give one high-value, lower-risk AI application and one higher-judgement one. *(Lower-risk: cost coding/
   reconciliation; higher-judgement: provisioning/revenue recognition, contingency.)*

---

## Knowledge Area 13.6 — Governance, ethics, risk & assurance of AI

*Topics: 13.6.1 "AI proposes; the professional verifies, decides and remains accountable" · 13.6.2 accountability, sign-off and auditability ·
13.6.3 hallucination, bias and confidentiality · 13.6.4 when not to use AI · 13.6.5 an AI-use policy and
verification checklist.*

### 13.6.1 "AI proposes; the professional verifies, decides and remains accountable"

**The governing principle.** The credential's principle is not a slogan — it is a governance rule: AI may
**propose** (draft, extract, forecast, detect), but a **qualified professional decides** (verifies,
signs off, and is **accountable**). No output is correct because a model produced it; it is correct because a
professional has verified it against source, policy and judgement, and put their name to it. This principle
runs through every workflow in 13.5 and every governance control below.

### 13.6.2 Accountability, sign-off and auditability

**The principle.** Because a model cannot be accountable, **a named person is** — for every AI-influenced
decision, estimate, forecast or disclosure. **Auditability** requires keeping the trail: **what** the AI
produced, **who** reviewed and approved it, **what** was changed, and **why** — so an AI-assisted number can be
defended later (linking to data lineage, 13.2.3). "It was the model's output" is never a defence; the sign-off
is.

### 13.6.3 Hallucination, bias and confidentiality

**The three principal risks.**

- **Hallucination** — AI can produce confident, false content: fabricated figures, invented citations,
  non-existent clauses. **Mitigation:** verify every figure/citation against source; use RAG grounding; low
  temperature; treat unverifiable claims as unverified.
- **Bias & fairness** — models reproduce biases in their training data. **Mitigation:** be alert where AI
  influences decisions about people or resources; test for skew; keep a human decision-maker.
- **Confidentiality / IP / data residency** — sensitive data pasted into ungoverned tools can be exposed or
  used to train models; data may cross jurisdictions. **Mitigation:** governed enterprise tools; no confidential
  data in public tools; know where data is processed and stored.

### 13.6.4 When not to use AI

**The judgement.** Responsible practice includes **not** using AI where it is inappropriate: for
**deterministic** tasks a rule handles better and more transparently (13.1.6); where **confidentiality** cannot
be assured; where the **stakes** demand certainty the model cannot give without heavy verification that negates
the time saving; where **data** is inadequate (garbage in); or where **accountability** cannot be maintained.
Knowing when *not* to reach for AI is as professional as knowing how to use it.

### 13.6.5 An AI-use policy and verification checklist

**The principle.** A controls function should operate an **AI-use policy**: which tools are approved for which
data; the verification and sign-off requirements; confidentiality rules; disclosure expectations; and the audit-
trail standard. Operationalised as a **verification checklist** applied to AI-assisted outputs:

> **AI-output assurance checklist (worked, applied to an AI-drafted EAC).**
> - [ ] **Source-checked** — figures recomputed from source (`AC`/`EV`/`BAC`, indices). ✔
> - [ ] **Method/assumption sound** — the `EAC` method matches the variance cause; `TCPI` reality-checked. ✔
> - [ ] **No hallucination** — no fabricated figures/citations; claims trace to data. ✔
> - [ ] **Confidentiality** — produced in a governed tool; no sensitive data exposed. ✔
> - [ ] **Cross-checked** — consistent with the schedule/critical path and prior period. ✔
> - [ ] **Signed off** — named professional approves; AI assistance and verification recorded. ✔

An output that fails any line is not released until fixed. The checklist is the practical embodiment of the
governing principle.

**Worked example 13.6.5b — the checklist applied to an AI-extracted contract term.** The same assurance
discipline applies to a different kind of output — an AI-extracted liquidated-damages rate (13.3.3b):

> - [ ] **Source-checked** — the LD rate is confirmed against the cited clause. ✔
> - [ ] **Grounded** — the extraction cites a real clause (no hallucinated reference). ✔
> - [ ] **Confidentiality** — the contract was processed in a governed tool, not a public one. ✔
> - [ ] **Legal-reviewed** — an entitlement-bearing term reviewed by a qualified person (Domain 7). ✔
> - [ ] **Signed off** — a named professional approves; AI assistance and verification recorded. ✔

The same "propose → verify → own" discipline applies to *extraction* as to *forecasting* — the risk (a
hallucinated or misread clause moving a commercial position) is just as real, and the accountability just as
human.

> **Fig 13.6.1 — AI-governance decision flow.** *Caption:* should this task use AI, and how is it assured?
> *Underlying data:* the governance tests above. *Render-ready description:* a decision tree — "Deterministic
> task?" → Yes → *use a rule*; No → "Data adequate & non-confidential (or governed)?" → No → *do not use / use
> governed tool*; Yes → "AI proposes" → "Verify (checklist)" → "Professional signs off & records trail" →
> *release*. Brand-blue diamonds; the sign-off node emphasised. *Animation storyboard (digital-only):* the
> EAC task flows down the tree, hitting each gate, and only reaches "release" after the sign-off node.

### 13.6.5c A model AI-use policy for a controls function

The following is a model policy a controls function can adopt and adapt. It is written as policy text — the
operational form of everything in this knowledge area.

**1. Purpose & scope.** This policy governs the use of artificial-intelligence tools by all staff, contractors
and secondees working within or on behalf of the project-controls function. It applies to any AI-assisted
work product that informs a controls output — estimates, forecasts, schedules, reports, reconciliations,
commercial analyses and disclosures — whether the tool is stand-alone or embedded in a platform. Nothing in
this policy transfers accountability to a tool: accountability for every output rests with a named
professional.

**2. Approved tools & data rules.** Staff may use only the tools on the approved register, which records each
tool, its permitted data classifications and its permitted uses. Confidential, commercially sensitive or
personal data must not be entered into any tool outside the approved register, and no data may be entered
into a tool above the data classification for which that tool is approved. Where a governed enterprise
alternative exists, it must be used in preference to a public tool. Requests to add a tool to the register are
made to the controls director and assessed for data handling, residency and auditability before approval.

**3. Verification & sign-off.** Every AI-assisted output must be verified against source before it is used or
circulated: figures recomputed, extractions checked against the cited document, and causal claims confirmed
against the underlying analysis. A named professional signs off each AI-assisted output and is accountable for
it; "the model produced it" is not an acceptable basis for release. The AI-output assurance checklist (13.6.5)
must be applied to material outputs — forecasts, disclosures, commercial positions and board reporting — and
an output that fails any checklist line is not released until the failure is fixed.

**4. Disclosure & audit trail.** AI assistance must be disclosed wherever the receiving forum, client,
regulator or contract requires it, and in all board-level and external reporting. For each material
AI-assisted output, staff must record what the AI produced, who reviewed and approved it, and what was changed
in review. These records form part of the function's audit trail and must be retained so that any
AI-influenced number can be traced to source and defended later.

**5. Prohibited uses.** AI tools must not be used to make deterministic control decisions for which an
approved rule exists — such decisions are made by transparent, auditable rules. No unverified AI-generated
figure, citation or clause may appear in any report or register. Entitlement, revenue-recognition,
provisioning and similar professional judgements must not be delegated to a model: AI may assemble and
summarise the material, but the judgement is made, and owned, by a qualified person with legal or specialist
review where required.

**6. Incidents & near-misses.** Any AI-related incident or near-miss — a hallucinated figure or clause, a
confidentiality breach, an unverified output circulated — must be reported to the controls director without
delay. Each incident is logged, the lesson is shared openly across the function rather than buried, and the
policy and working practices are updated where the incident shows a gap. Open reporting of near-misses is
treated as professional conduct, not failure.

**7. Review cadence.** The approved-tool register is re-validated quarterly, because tool capabilities and
data-handling terms change. This policy is reviewed annually, or immediately after any material incident. All
staff complete training in prompting, verification and this policy before using AI tools, with refresher
training at least annually.

*A template to adapt — the policy's force comes from the sign-off discipline it encodes, not the paper.*

**AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 13.6

| Term | Meaning |
|---|---|
| **AI proposes; the professional verifies, decides and remains accountable** | AI drafts/predicts; a qualified professional decides and is accountable. |
| **Auditability / sign-off** | Keeping the trail of what AI produced, who approved it, what changed and why. |
| **Hallucination / bias / confidentiality** | The three principal AI risks and their mitigations. |
| **AI-use policy / verification checklist** | The governance document and the operational assurance step. |

### Sample MCQs — KA 13.6

**MCQ 13.6-A `[13.6.2 · Analysis]`** When an AI-assisted forecast is later challenged, an acceptable defence is:
- A. "It was the model's output."
- B. Deleting the audit trail.
- C. "The model is very advanced."
- D. The documented verification and named sign-off showing how it was checked and owned. ✅

*Rationale:* Accountability rests with a named professional; the defence is the documented verification and
sign-off. Blaming the model, appealing to its sophistication, or destroying the trail all fail governance.

**MCQ 13.6-B `[13.6.4 · Analysis]`** Which is a legitimate reason **not** to use AI for a task?
- A. The logic is deterministic and a transparent rule handles it better. ✅
- B. AI would save time.
- C. Colleagues use AI.
- D. The output looks impressive.

*Rationale:* For deterministic tasks a rule is more transparent and auditable — a valid reason to avoid AI.
Time-saving, peer use and impressive output are not reasons to override the appropriateness test.

**MCQ 13.6-C `[13.6.3 · Recall]`** The mitigation for hallucination in a controls context is to:
- A. Verify every figure/citation against source, use RAG grounding and low temperature. ✅
- B. Trust the model more.
- C. Increase temperature.
- D. Paste more confidential data.

*Rationale:* Hallucination is mitigated by verification, grounding and low temperature. The other options
increase risk.

**MCQ 13.6-D `[13.6.5 · Application]`** An AI-drafted forecast passes every line of the assurance checklist
except "cross-checked" — it is inconsistent with the schedule's critical path. The correct action is to:
- A. Release it with a footnote noting the inconsistency.
- B. Withhold it until the failure is fixed. ✅
- C. Release it because most lines passed.
- D. Remove the cross-check line from the checklist.

*Rationale:* An output that fails **any** checklist line is not released until fixed (13.6.5). Footnoting a
known inconsistency, releasing on a majority pass, or weakening the checklist all defeat the assurance the
checklist exists to provide.

**MCQ 13.6-E `[13.6.3 · Recall]`** Bias arises in AI systems primarily because:
- A. Models are deliberately unfair.
- B. Temperature is set too low.
- C. Models reproduce the biases present in their training data. ✅
- D. Verification introduces skew.

*Rationale:* Models learn — and therefore reproduce — the patterns in their data, including its biases; the
mitigations are alertness where AI influences decisions about people or resources, testing for skew, and
keeping a human decision-maker. The other options misstate the mechanism.

**MCQ 13.6-F `[13.6.5 · Application]`** An AI extraction reports an LD rate citing clause 14.3. The reviewer
opens clause 14.3: the clause exists, but states a different rate. Applying the assurance checklist, the
correct conclusion is:
- A. The output passes — the citation is real, so the grounding line is satisfied.
- B. Release it with a footnote recording the difference.
- C. Skip the checklist — legal review will catch it later.
- D. The source-check line fails — the value does not match the cited clause — so the output is withheld
  until fixed. ✅

*Rationale:* Source-checking means confirming the extracted **value** against the cited clause, not merely
that the clause exists (13.6.5b, 13.3.3b) — a real citation with a wrong value is still a failed check, and an
output that fails any line is not released until fixed. A confuses citation existence with verification; B
footnotes a known failure; C substitutes a later control for the one that just failed.

**MCQ 13.6-G `[13.A.1 · Application]`** A controls function deploys an agentic system that retrieves the
month-end extract, computes the variances, drafts commentary and assembles the exception pack. The
verification discipline should:
- A. Move from per-output to per-workflow — assure the chain's design and insert checkpoints where
  consequential intermediate outputs are inspected before the chain proceeds. ✅
- B. Apply only to the final pack, since that is all anyone reads.
- C. Be dropped — an agent that checks its own work needs no reviewer.
- D. Be replaced by an annual audit of the vendor.

*Rationale:* The governance need scales with autonomy: an agent's early error is compounded and laundered
through every later step, so the professional assures the chain design and checkpoints, and the audit trail
records the chain, not just the answer (13.A.1). B lets a step-two error arrive polished in the final pack; C
removes the accountable human; D is far too infrequent and aimed at the wrong object.

**MCQ 13.6-H `[13.6.4 · Application]`** A one-off, high-stakes external disclosure would take longer to
verify line-by-line than to draft manually, and the drafting data is highly confidential. Under the "when not
to use AI" tests, the professional should:
- A. Use AI anyway — it is the modern approach.
- B. Not use AI for this task — the verification burden negates the time saving and the stakes demand
  certainty the model cannot give. ✅
- C. Use AI and skip verification to preserve the saving.
- D. Use a public tool, since the task is a one-off.

*Rationale:* Responsible practice includes not using AI where the stakes demand certainty that only heavy
verification could give, negating the saving (13.6.4) — knowing when *not* to reach for AI is as professional
as knowing how. A is fashion, not judgement; C abandons the non-negotiable step; D adds a confidentiality
breach (13.2.5) to the wrong call.

### Self-check — KA 13.6

1. State the governing principle and why a model cannot satisfy accountability. *(AI proposes; the professional verifies, decides and remains accountable; a model cannot be accountable — a named person is.)*
2. Give three lines of an AI-output assurance checklist. *(Source-checked; method/assumption sound; no
   hallucination; confidentiality; cross-checked; signed off.)*

---

## Knowledge Area 13.7 — Building an AI-augmented project-controls capability

*Topics: 13.7.1 the maturity model · 13.7.2 integration and upskilling · 13.7.3 measuring value · 13.7.4
pitfalls and change management · 13.7.5 the near-future outlook (honestly).*

### 13.7.1 The maturity model

**The principle.** Organisations mature through stages, from **ad-hoc** (individuals experimenting, no
governance) → **piloting** (defined use cases, some guardrails) → **standardised** (approved tools, policy,
verification embedded) → **integrated** (AI embedded in the controls workflow and tooling) → **governed &
optimised** (measured value, continuous improvement, full audit trail). Knowing the current stage sets the
realistic next step — jumping to "integrated" without governance invites the risks of 13.6.

> **Fig 13.7.1 — The AI-maturity ladder.** *Caption:* stages of an AI-augmented controls capability.
> *Underlying data:* ad-hoc → piloting → standardised → integrated → governed/optimised. *Render-ready
> description:* a five-rung ascending ladder in brand blue, each rung labelled with its stage and one marker of
> that stage (governance, tooling, value measurement). *Animation storyboard (digital-only):* a marker climbs
> the ladder; at each rung the governance/tooling/value indicators fill in.*

### 13.7.2 Integration and upskilling

**The principle.** Value comes from **integrating** AI into the actual controls workflow (not bolting it on)
and from **upskilling** people — data literacy (13.2), prompting (13.3), verification (13.3.3), and governance
(13.6). The controls professional's role shifts from *producing* every number to *directing and assuring*
AI-assisted production — a higher-judgement role, not a diminished one. The professionals who thrive are those
who pair domain mastery (Domains 1–12) with AI fluency.

### 13.7.3 Measuring value

**The principle.** AI initiatives must **measure value** — time saved, error reduction, earlier warning,
forecast accuracy improvement — against cost and risk, honestly. Vanity metrics ("we use AI") are not value;
a faster, more accurate month-end, or an earlier-warned overrun, is. Measuring value is what separates a
sustainable capability from hype.

**Worked example 13.7.3 — a value case that is honest about cost.**

1. **Setup.** AI-assisted coding and reconciliation (13.5.4) cuts a four-person month-end close from **5 days
   to 2**. Loaded cost **USD 90/hour**, 8-hour days.
2. **Formula.** `annual saving = days saved × staff × hours × rate × 12`; compare with the tooling + governance
   cost.
3. **Substitution.** `3 × 4 × 8 × 90 = USD 8,640` per month; `× 12 = USD 103,680` per year. Tool licences plus
   governance/verification effort: **USD 60,000** per year.
4. **Result.** **Net value ≈ USD 43,680 per year**, before the harder-to-price benefits (earlier reporting,
   earlier warnings).
5. **Interpretation.** An honest value case nets the *real* costs — licences *and* the human verification the
   governance model requires (13.6) — against measured time savings, not vendor claims. "We use AI" is not
   value; a faster, verified close is (13.7.3).

**Per-use economics.** Licence fees are only one AI cost shape; API-metered use is priced per token (13.1.3),
and at volume the per-use arithmetic decides deployability. Three numbers govern it: tokens in, tokens out,
and the price per million of each — multiplied by volume. Two honest observations follow. First, per-use
compute cost is usually small against the human review labour it sits beside (Advanced 13.A.5 prices that
side). Second, it scales linearly with volume while a licence is a step function — so the crossover volume,
not fashion, should pick the commercial shape (Advanced 13.A.3's procurement lens).

**Worked example 13.7.3b — pricing a document-extraction workflow.**

1. **Setup.** A controls function extracts key terms from **60,000 documents a year**. Each run averages
   **3,000 tokens in** and **500 tokens out**; the model is priced at **USD 3.00 per million input tokens**
   and **USD 15.00 per million output tokens** (illustrative rates — check current pricing).
2. **Formula.** `annual cost = volume × [(in ÷ 1M) × price_in + (out ÷ 1M) × price_out]`.
3. **Substitution.** Per document: `(3,000 ÷ 1M) × 3.00 + (500 ÷ 1M) × 15.00 = 0.009 + 0.0075 ≈ USD 0.0165`.
   Annual: `60,000 × 0.0165 ≈ 990`.
4. **Result.** About **USD 1,000 a year** of compute — while the half-day-per-month review labour beside it
   costs tens of times more (13.A.5), and a fixed per-seat licence for the same workflow might cost seventy
   times more.
5. **Interpretation.** The number that matters is rarely the compute; it is the review labour and the error
   cost (13.A.5) — but the per-use arithmetic still earns its keep, because it exposes the crossover where
   metered beats licensed (and vice versa), keeps vendor quotes honest (13.A.3), and scales the value case of
   13.7.3 from pilot to fleet without a step of faith.

### 13.7.4 Pitfalls and change management

**The pitfalls.** Common failure modes: **skipping data quality** (13.2) so outputs mislead; **skipping
governance** (13.6) so risk accumulates; **over-trusting** AI (dropping verification); **confidentiality
breaches**; **solving the wrong problem** (automating a task that should be eliminated); and **change
resistance** or, conversely, **uncritical hype**. Change management — bringing people with you, being honest
about limits — is as important as the technology.

### 13.7.5 The near-future outlook (honestly)

**The principle.** Stated honestly, not hyped: AI capability is advancing quickly, and more of the controls
workflow will be AI-assisted — but the **governing principle endures**. Greater capability raises the stakes of
verification and governance, not lowers them: a more capable model that is wrong is more *convincingly* wrong.
The durable professional skill is not any specific tool but the **judgement to direct, verify and own**
AI-assisted work — which is exactly what this credential certifies. Where a capability is uncertain or evolving,
a professional says so rather than overstating it.

**AI in this domain.** This domain *is* the AI domain — its own governing note is that every claim in it is kept
accurate, every workflow reproducible, and the professional's judgement central throughout. **AI proposes; the professional verifies, decides and remains accountable.**

### Key terms — KA 13.7

| Term | Meaning |
|---|---|
| **AI-maturity model** | Ad-hoc → piloting → standardised → integrated → governed/optimised. |
| **Integration / upskilling** | Embedding AI in the workflow / building data, prompting, verification, governance skills. |
| **Value measurement** | Honest measurement of time/error/warning/accuracy gains vs cost and risk. |
| **Change management** | Bringing people with you; honest about limits, resistant to hype. |

### Sample MCQs — KA 13.7

**MCQ 13.7-A `[13.7.1 · Analysis]`** Jumping straight to "AI integrated in the workflow" without governance
primarily:
- A. Invites the risks of ungoverned AI (hallucination, confidentiality, no audit trail). ✅
- B. Saves the most time safely.
- C. Is required by the maturity model.
- D. Has no downside.

*Rationale:* Integration without governance accumulates the very risks of 13.6. The maturity model advises
building governance *with* integration, not skipping it.

**MCQ 13.7-B `[13.7.5 · Analysis]`** As AI capability advances, the need for verification and governance:
- A. Disappears.
- B. Increases — a more capable model that is wrong is more convincingly wrong. ✅
- C. Stays irrelevant.
- D. Is replaced by the model.

*Rationale:* Greater capability raises the stakes of assurance; convincing errors are harder to catch. The
governing principle endures; the model cannot replace accountability.

**MCQ 13.7-C `[13.7.3 · Application]`** AI-assisted reconciliation cuts a three-person month-end close from
**4 days to 2** (8-hour days, loaded cost **USD 100/hour**). Annual tooling and governance cost is
**USD 30,000**. The honest **net** annual value is:
- A. USD 4,800
- B. USD 27,600 ✅
- C. USD 57,600
- D. USD 87,600

*Rationale:* Monthly saving `= 2 days × 3 staff × 8 hours × USD 100 = USD 4,800`; annual
`= 4,800 × 12 = USD 57,600`; net `= 57,600 − 30,000 = USD 27,600`. A is the monthly saving; C is the gross
annual figure with the cost not netted; D wrongly adds the cost instead of subtracting it. An honest value
case nets the full tooling *and* governance cost (13.7.3).

**MCQ 13.7-D `[13.7.2 · Recall]`** As AI is integrated into the controls workflow, the professional's role
shifts toward:
- A. Being replaced by the model.
- B. Needing less domain knowledge.
- C. Producing every number manually to be safe.
- D. Directing and assuring AI-assisted production — a higher-judgement role. ✅

*Rationale:* The role moves from *producing* every number to *directing and assuring* AI-assisted production;
the professionals who thrive pair domain mastery with AI fluency (13.7.2). The role is neither replaced nor
diminished, and domain knowledge matters more, not less.

**MCQ 13.7-E `[13.A.5 · Application]`** Reviewing one auto-coded line costs **USD 2**; an uncaught miscode
costs **USD 400** downstream. Measured precision is **99.0 %**. Pricing the review step:
- A. Per-item review still pays — expected uncaught-error cost is `1 % × 400 = USD 4` per line, above the
  USD 2 review cost; break-even sits at a precision of 99.5 %. ✅
- B. Per-item review no longer pays — a 1 % error rate is negligible.
- C. Per-item review no longer pays — 99.0 % precision exceeds the USD 2 review cost.
- D. Per-item review always pays, whatever the precision.

*Rationale:* Per-item review pays while `error rate × error cost > review cost`: `0.01 × 400 = 4 > 2`
(13.A.5). Break-even is an error rate of `2 ÷ 400 = 0.5 %` — precision 99.5 % — above which assurance moves
to sampling. B ignores the error cost that makes 1 % expensive; C compares a percentage to a dollar figure,
a category error; D makes the review a permanent fixture rather than a priced control.

**MCQ 13.7-F `[13.7.3 · Application]`** An extraction workflow runs **20,000 documents a year**, averaging
**2,000 tokens in** and **500 tokens out**, priced at **USD 3.00 per million input tokens** and **USD 15.00
per million output tokens**. The annual compute cost is approximately:
- A. USD 120
- B. USD 150
- C. USD 2,700
- D. USD 270 ✅

*Rationale:* Per document: `(2,000 ÷ 1M) × 3.00 + (500 ÷ 1M) × 15.00 = 0.006 + 0.0075 = USD 0.0135`; annual:
`20,000 × 0.0135 = USD 270` (13.7.3b). A prices the input tokens only; B prices the output tokens only; C is
a tenfold decimal slip. The compute is small against the review labour beside it — but the arithmetic is what
exposes the metered-vs-licensed crossover.

**MCQ 13.7-G `[13.A.4 · Analysis]`** A function whose AI now drafts most narratives and codes most cost lines
still requires analysts to work problems by hand on a regular rotation, with AI switched off. The primary
purpose is to:
- A. Punish over-reliance on the tools.
- B. Reduce licence costs during the rotation.
- C. Maintain the first-principles judgement that verification of AI output depends on. ✅
- D. Comply with a data-residency requirement.

*Rationale:* Verification presupposes a verifier who can still do the work — recompute the forecast, spot the
wrong assumption — and that judgement is built by doing, so the function now produces it on purpose (13.A.4).
A misreads deliberate practice as sanction; B and D are unrelated to the deskilling risk the rotation
mitigates.

### Self-check — KA 13.7

1. Name the stages of the AI-maturity model. *(Ad-hoc → piloting → standardised → integrated → governed/
   optimised.)*
2. Why does advancing AI capability *increase* the need for governance? *(A more capable model that is wrong is
   more convincingly wrong; the stakes of verification rise.)*

---

## Advanced topics — Domain 13

*These topics extend the domain for practitioners who lead the function; the examination samples them
lightly, practice does not.*

### Advanced 13.A.1 — Agentic AI at awareness level

The workflows of KA 13.5 treat AI as a **single step**: one prompt or model run, one output, one
verification. **Agentic AI** describes systems that instead **plan and execute a chain of actions** toward a
goal — retrieve the relevant documents, run a computation, draft an output, check it, decide what to do next
— rather than answer one prompt. In controls terms: not "draft a variance narrative from the data below,"
but "produce the month-end exception report" — with the system itself retrieving the extract, computing the
variances, drafting the commentary and assembling the pack.

The framing here is deliberately honest (KA 13.7.5): this capability is **evolving fast**, and any specific
claim about what agents reliably can or cannot do would date quickly — the professional validates current
capability rather than assuming it (KA 13.4.3). What can be stated durably is the governance consequence:
**the governance need scales with autonomy** (KA 13.6). A single-step tool that errs produces one wrong
output, caught by one verification. An agent that errs at step two carries the error into every later step,
each of which builds on it plausibly — the mistake is compounded and *laundered* through the chain, arriving
in a polished final output whose intermediate workings nobody saw.

Verification therefore moves **from per-output to per-workflow**. The professional assures the *design* of
the chain — which steps are permitted, which data each step may touch, where the chain must stop — and
inserts **checkpoints** at which consequential intermediate outputs are inspected before the chain proceeds,
the workflow analogue of the quality gates in a data pipeline (KA 13.2.3). The sign-off discipline is
unchanged: a named person is accountable for what the agent produced (KA 13.6.2), and the audit trail must
now record the **chain**, not just the answer. Autonomy is something the professional grants, in measured
amounts, with the verification designed in — however many steps the system takes, the governing principle
holds without amendment.

### Advanced 13.A.2 — Model risk management

Once ML models influence real decisions — a predictive `EAC` (KA 13.5.3), a risk score (KA 13.5.9) — ad-hoc
checking is not enough; the function needs **model risk management**, a discipline long practised in
regulated industries and described here generically.

- **Validation before use.** The model is tested on data held back from training, its assumptions and
  training-data representativeness challenged (KA 13.2), and its limitations documented. Evidence like the
  case study's 2-of-3 flag precision *is* validation evidence — gathered before reliance, not after.
- **Monitoring for drift.** The world the model learned changes — portfolio mix, market conditions, coding
  structures — so live performance is monitored and degradation investigated. A model that was right last
  year and is silently wrong this year is the hallmark failure this control exists to catch.
- **Champion–challenger.** A candidate replacement runs alongside the incumbent on the same live data, and is
  promoted only when it demonstrably outperforms — the model-world analogue of parallel running.
- **Periodic revalidation.** On a calendar, and on triggers (a data-structure change, a poor quarter) — the
  quarterly re-validation cadence of KA 13.7.1 extended from tools to the models themselves.

The **model inventory** binds this together: a register of every model influencing a controls output — its
purpose, data, validation date, known limitations and owner. Like the approved-tool register (KA 13.6.5c) it
is an **audit artefact**: when an AI-influenced number is challenged, the inventory shows which model touched
it and who assured it. And ownership is personal: **a named person owns each model's performance** (KA
13.6.2) — not the vendor, not "the data team" in the abstract — and can answer, at any time, "how do you
know it still works?" The scale of all this is proportionate to the stakes; for models feeding decisions it
is never zero.

### Advanced 13.A.3 — Procuring AI capability

A practitioner who leads the function buys AI capability as often as they use it, and procurement is where
several of this domain's disciplines meet.

**Claims versus capability.** Features change rapidly (KA 13.4.3), so a vendor claim is a hypothesis, not a
fact: the evaluation tests the tool against **verifiable capability on your own data** — safely prepared
(KA 13.2.5) — scoring outputs against known correct answers. A glossy demonstration on the vendor's data is
not evidence; the same discipline that verifies a model's output (KA 13.3.3) verifies a vendor's claim.

**The terms that matter.** Applied contract discipline (Domain 7), turned on the function's own purchases:
**data residency and handling** — where your data is processed and stored, and whether it is used to train
the vendor's models (KA 13.6.3); **intellectual property** — who owns the outputs, and any artefact
fine-tuned or configured on your data; and **exit terms** — the right to extract your data, mappings and
configuration in usable form. Lock-in through un-exportable data is a risk priced at signing, not discovered
at exit.

**Pilot before commit.** A bounded pilot with success metrics defined in advance — the agency case study's
sequencing — precedes any enterprise commitment, and a pilot that fails its metrics is allowed to fail.

**Total cost, honestly netted.** The value case follows KA 13.7.3: licences *plus* integration, data
preparation (KA 13.2), training, and the **ongoing governance and verification effort** the operating model
requires (KA 13.6). A vendor's business case typically prices the licence alone; omitting the human
verification cost is the most common way an AI procurement case flatters itself. The professional evaluates
the netted figure — and is prepared to find it modest.

### Advanced 13.A.4 — The deskilling risk

If AI drafts every narrative, codes every cost line and proposes every `EAC`, where does the next
generation's judgement come from? The whole governance model of this domain rests on verification (KA
13.3.3), and verification presupposes a verifier who **can still do the work**: recompute the forecast, spot
the wrong `EAC` assumption, recognise an unrealistic duration. Those instincts have always been built by
*doing* the work — historically by juniors doing precisely the tasks AI now does first. Today's verifiers
trained before AI; the open question is where tomorrow's come from. Stated honestly: this is an evolving
concern with no settled best practice, but the direction of the risk is clear, and a function that waits for
the evidence to arrive in its own error rates has waited too long.

Three mitigations are within any function's reach.

- **Deliberate practice** — regular working of problems *without* AI. That is the intent of this book's
  calculation exercises: worked by hand, before the solution is read, because the point is not the answer but
  the fluency that verification later depends on.
- **Rotation through first-principles work** — periods in which developing professionals build an estimate,
  a schedule or a reconciliation from source themselves, with AI switched off, so the judgement is formed on
  the task and not on reviewing a draft of it.
- **Verification as a skill in itself** — taught and assessed, not assumed: how to recompute a figure, ground
  an extraction, challenge a causal claim. A reviewer who only rubber-stamps is already deskilled — the
  "over-trusting" failure of KA 13.7.4 in slow motion.

The optimistic reading of KA 13.7.2 — that the professional's role rises to directing and assuring — is true
only while the judgement it presumes is maintained. The daily workflow no longer produces that judgement as a
by-product, so the profession must now produce it on purpose. **AI proposes; the professional verifies, decides and remains accountable.**

### Advanced 13.A.5 — Pricing the error: expected-value thinking for AI controls

"AI proposes; the professional verifies, decides and remains accountable" (KA 13.6.1) states *who* decides; this topic prices *when* the
disposing step earns its cost. Every AI-in-the-loop design carries two costs: the cost of **checking** an
output, and the expected cost of an **uncaught error** — the error's downstream cost weighted by its
probability. Per-item review is worth mandating while `error rate × error cost > review cost`; below that
line, per-item review destroys value, and assurance should move to sampling and monitoring — the
audit-sample logic of KA 13.6.5.

**Worked example 13.A.5 — pricing the review step.**

1. **Setup.** The invoice-coding deployment of KA 13.5.4c. Reviewing one auto-coded line costs about
   **USD 1**; an uncaught miscode costs about **USD 150** to find and fix downstream (reconciliation
   time, restated reports). Measured precision: **97.5 %**.
2. **Formula.** `expected uncaught-error cost per unreviewed line = error rate × error cost`; per-item
   review pays while that figure exceeds the review cost.
3. **Substitution.** At 97.5 % precision: `2.5 % × 150 = 3.75` per line — nearly four times the USD 1
   review cost, so per-item review pays clearly. Were precision to reach **99.5 %**: `0.5 % × 150 =
   0.75` — below the review cost, and the per-item mandate stops paying.
4. **Result.** Break-even sits at an error rate of `1 ÷ 150 ≈ 0.67 %` — a precision of about **99.3 %**.
5. **Interpretation.** The review step is not a permanent fixture but a **priced control**: as measured
   precision improves, the honest response is to re-price the loop — per-item review giving way to
   risk-based sampling — with the change logged like any control change (KA 13.6.5c).

The honesty belongs in the inputs. Both figures are estimates and should be **measured, not assumed** —
and error cost varies wildly by use: a miscoded invoice is USD 150; a miscited contract clause in a
dispute is not. **Asymmetric, fat-tailed error costs** — rare but catastrophic — break the simple
per-item arithmetic and justify review far past the naive break-even, which is why high-stakes uses keep
human sign-off regardless of the expected-value sums (KA 13.6.4, when not to use AI). Used honestly, the
arithmetic disciplines both directions: it blocks premature automation *and* retires review theatre. And
the break-even itself moves — precision drift shifts the error rate, so the model-risk monitoring of
13.A.2 feeds the pricing here.

### Advanced 13.A.6 — APIs, integration and the automated dashboard

Every AI ambition in this domain stands on a plumbing decision: how data moves from the systems where the
work happens — ERP, scheduling tool, timesheets, document control (KA 13.2.4) — to the place where it is
analysed and reported. At awareness level, three patterns cover the field. **Manual export/import** —
spreadsheets and email — is fragile, unlogged, and where version chaos lives. **Batch ETL/file transfer** —
scheduled extracts on a defined cadence — is robust and auditable, but the data is hours to days old. **API
integration** — systems queried programmatically — is the freshest and the foundation of live dashboards, at
the price of real engineering and governance. The professional's rule: the right pattern is set by the
**decision cadence** the data serves (Domain 4, KA 4.3.4) — a monthly cost pack does not need a real-time
feed, and a daily site dashboard cannot run on a monthly extract.

The **automated dashboard** is KA 4.3.2 made live, and its value case is worth working through.

**Worked example 13.A.6 — the month-end pack, automated.**

1. **Setup.** A controls team assembles its monthly pack by manual export and reconciliation: **4 working
   days** of one controller each month.
2. **Formula.** `annual effort = days per cycle × cycles per year`.
3. **Substitution.** Manual: `4 × 12 = 48` controller-days a year. An API-fed model with automated refresh
   and validation checks cuts the human step to a **half-day review** of exceptions and narrative:
   `0.5 × 12 = 6` days a year.
4. **Result.** `48 − 6 = 42` controller-days a year returned to analysis.
5. **Interpretation.** The saving is real, but the **quality** change is bigger: the controller's days move
   from assembling numbers to interrogating them. And the review half-day is not optional — an automated
   pipeline fails silently where a human assembler would have noticed, so the deskilling caution of 13.A.4
   applies to pipelines too.

For the integration to be trustworthy, four things must be governed. A **data contract** per feed — fields,
definitions, units, update cadence, owner — is the lineage discipline of KA 13.2.3 applied at the interface.
**Validation at the boundary** — row counts, control totals, referential checks — because a dashboard that
renders stale or partial data confidently is worse than a late pack. **Change management on schemas** — the
silent upstream field change is the classic failure mode (the master-data governance of Domain 11, Advanced
11.A.3). And **one source of truth per number** — two dashboards disagreeing on actual cost costs more
credibility than either earns. Access control matters doubly once AI agents consume the APIs (13.A.1).

AI can draft the integration mappings, flag anomalies at the boundary and even narrate the dashboard (KA
13.5.8) — but the data contract, the validation thresholds and the single-source decision are governance
choices a named professional owns. AI proposes; the professional verifies, decides and remains accountable.

### Advanced 13.A.7 — Evaluating AI outputs: golden sets, precision and recall, and drift

"Measure, don't trust" (KA 13.3.3) is a principle; this topic is its machinery. The test instrument is the
**golden set** — a sample of inputs whose correct answers were established by professionals and are kept
under version control — against which any AI step in the controls workflow is scored, before reliance and on
a cadence thereafter. Two complementary error rates come out of every such run. **Precision** asks: of what
the model flagged or produced, how much was right? — it is the cost of **false alarms**. **Recall** asks: of
what was truly there, how much did the model find? — it is the cost of **misses**. Which matters more is not
a technical question but an economic one, priced by the error arithmetic of 13.A.5: a fraud monitor lives on
recall, because a miss is expensive; an auto-coder lives on precision, because a false code pollutes the
ledger.

**Worked example 13.A.7 — a golden-set evaluation, run and re-run.**

1. **Setup.** A duplicate-invoice detector (KA 13.5.4) is evaluated on a golden set of **200 invoices**
   containing **50 known duplicates**. The model flags **40** invoices, of which **30** are genuine
   duplicates.
2. **Formula.** `precision = true hits ÷ total flags`; `recall = true hits ÷ total true cases`; balanced
   summary `F1 = 2 × (precision × recall) ÷ (precision + recall)`.
3. **Substitution.** Precision `= 30 ÷ 40 = 75 %`; recall `= 30 ÷ 50 = 60 %`;
   `F1 = 2 × (0.75 × 0.60) ÷ (0.75 + 0.60) = 0.90 ÷ 1.35 ≈ 0.67`.
4. **Result.** Fit for use as a *screening* aid — 75 % of its alerts are real — but not as the only line of
   defence: it misses 40 % of duplicates, so the month-end duplicate review of Toolkit 11.T.2 stays.
5. **Interpretation.** The evaluation is re-run quarterly on the versioned set. Next quarter the same **30**
   genuine hits need **48 flags** — precision `30 ÷ 48 = 62.5 %` — **drift**, investigated per 13.A.2 before
   anyone re-tunes thresholds. One number is never enough: precision without recall rewards a model that
   flags almost nothing; recall without precision rewards one that flags everything; and any score without a
   dated golden set and a re-run cadence is a claim, not a measurement.

The governance is the point. The golden set is **versioned**; its provenance is documented — who judged the
answers, and when; and it is **never used to train or tune the model it tests** — the exam-integrity
principle applied to machines. Its results feed the model inventory (13.A.2) and the review-step pricing
(13.A.5). Stated honestly: building and refreshing golden sets is unglamorous professional work — and it is
exactly the work that separates governed AI from vibes.

---

## Case study — Domain 13: building an AI-augmented controls function at a transport agency (government)

### Background

A government transport agency runs a portfolio of road and rail projects — corridor upgrades, structure
renewals, a light-rail extension — through a central project-controls function of around thirty staff. Over
**18 months**, that function moved from ad-hoc AI experimentation to a governed, measured, AI-augmented
capability: the maturity ladder of KA 13.7.1 walked for real, rung by rung. This case study follows the walk —
the unglamorous data work, two pilots with honest numbers, one instructive failure, and the governance that
turned a near-miss into the strongest adoption argument the function ever had. The public-sector context
sharpens everything: the agency's numbers feed ministerial reporting and audit, its contracts carry public
money, and "the model said so" is not a defence anyone in the chain can offer (13.6.2). Every stage below is
the operational form of the credential's governing principle: **AI proposes; the professional verifies, decides and remains accountable.**

### Where they started (13.7.1)

The starting point was the bottom rung of the KA 13.7.1 ladder, and it looked exactly as the model predicts:
**ad-hoc**. Individual analysts, entirely well-intentioned, were pasting cost extracts, schedule narratives and
— in at least one case — draft contract correspondence into **public AI tools** to speed their work. Nobody had
assessed the confidentiality exposure (13.2.5, 13.6.3): commercially sensitive rates and claim positions were
leaving the agency's control with every paste. There was no policy, no approved-tool list, and no verification
norm — outputs were being reused on the strength of looking plausible, which is precisely what an LLM
guarantees and precisely what it does not warrant (13.1.3).

The function's first act was therefore **not a pilot but a stop**. The controls director issued an **interim
AI-use policy** (13.6.5): governed, approved tools only; **no sensitive data in public tools**; and **every
AI-assisted output verified against source before use**, with the verification recorded. Some momentum was
lost and a few enthusiasts grumbled — but the sequencing was deliberate. Jumping to integration without
governance invites the risks of 13.6 (it is the failure mode KA 13.7.1 warns against by name); the stop
created the safe floor on which everything after it was built.

### The data reckoning (13.2)

Before any model was trained or any assistant deployed, the team profiled the portfolio's cost data against
the quality dimensions of 13.2.2 — and the profile, not a vendor demo, set the programme's real agenda. The
portfolio generates about **18,000 cost lines a month** across the ERP feeds. A trial of AI-assisted
auto-coding (13.5.4) showed first-pass coding confidence holding for **85 %** of lines — leaving **15 %** as
exceptions needing human handling every month.

1. **Setup.** 18,000 cost lines a month; first-pass auto-coding confidence 85 %, so 15 % fall to exceptions.
2. **Formula.** `exceptions per month = lines × exception rate`.
3. **Substitution.** `18,000 × 15 % = 18,000 × 0.15`.
4. **Result.** **2,700 exceptions a month.**
5. **Interpretation.** At 2,700 exceptions a month the "automation" would have manufactured a new manual
   workload. Tracing the exceptions showed the cause was not the classifier but the data: **inconsistent legacy
   coding** — old cost-code structures from predecessor projects, free-text descriptions, codes valid in one
   system and not another (13.2.2's validity and consistency dimensions failing at once).

The response was six months of remediation nobody would call glamorous: mapping rules from legacy codes to the
current structure, cleaning and retiring dead codes, tightening validation at the point of entry — classic
controls hygiene (Domains 1, 5) led by the controls team itself, because it is the team that understands the
coding (13.2.1). The result:

1. **Setup.** Post-remediation first-pass confidence **93 %**, so 7 % fall to exceptions.
2. **Formula.** `exceptions per month = lines × exception rate`.
3. **Substitution.** `18,000 × 7 % = 18,000 × 0.07`.
4. **Result.** **1,260 exceptions a month** — down from 2,700.
5. **Interpretation.** The remediation more than halved the exception load, and it did so before any further AI
   spend. The lesson the team internalised is the lesson of 13.2.1: **the unglamorous data work *was* the AI
   programme.** Garbage in, garbage out is not a caveat to the business case; it is the business case's first
   line item.

### The first governed pilot — month-end (13.5.4)

With coding data remediated, an approved tool in place and the verification norm operating, the function ran
its first formal pilot on the highest-value, lowest-risk workflow in the map: **month-end cost control and
extraction** (13.5.4). The AI step auto-coded the 18,000 monthly lines, matched them to the ledger, flagged
duplicates and proposed accruals; the professionals worked the 1,260-line exception queue, sampled the
high-confidence population periodically, and checked that accrual proposals used **service dates, not invoice
dates** (Domain 1, KA 1.3.5 — the cut-off error an ungoverned tool would reproduce at scale). The close for
the portfolio's central reporting shortened from **6 days to 3** across the five-person month-end team. The
value case was built the honest way (13.7.3) — measured time, loaded rates, and the *full* cost of tooling
**and** governance netted against it:

1. **Setup.** Close shortened from 6 days to 3 (3 days saved) across a five-person team; 8-hour days; loaded
   cost **USD 80/hour**. Tooling plus governance and verification effort: **USD 70,000 a year**.
2. **Formula.** `monthly saving = days saved × staff × hours × rate`; `annual saving = monthly × 12`;
   `net = annual saving − annual cost`.
3. **Substitution.** `3 × 5 × 8 × 80 = 9,600` per month; `9,600 × 12 = 115,200`; `115,200 − 70,000`.
4. **Result.** **USD 115,200 a year** gross; **net ≈ USD 45,200 a year**.
5. **Interpretation.** A real but modest net figure — deliberately stated without inflation. The team resisted
   the temptation to claim the gross number and hide the governance cost: the USD 70,000 includes the licences
   *and* the human verification the governance model requires, because a saving that evaporates when you price
   the checking is not a saving (13.7.3). The unpriced benefit sat alongside, honestly labelled as unpriced:
   reporting landing **three days earlier** every month, which moved decision meetings forward and gave every
   downstream forecast an extra three days of currency. "We use AI" was never the metric; a faster, *verified*
   close was.

### The second pilot — predictive EAC (13.5.3)

The second pilot moved up the value chain and up the risk chain: a **predictive EAC model** (13.5.3) run over
the **12 live projects** in the portfolio, trained on the now-remediated cost history with schedule progress
and commitment data alongside. In its first full quarter the model flagged **3 projects** as likely to
overrun beyond their current EACs.

The professionals then did what the workflow shape of 13.5.1 requires — verification before action. Reviewing
each flag against the variance's cause (Domain 6, KA 6.3.3) and the `TCPI` reality check, they **confirmed 2**
of the three: both had genuine, persisting cost drivers the monthly cycle had not yet escalated, caught early
enough to act — one produced a funded **recovery plan**, the other an honest **re-baseline** taken to the
project board before the position hardened. The third flag was **rejected as a false positive**: a data
artefact from a **re-phased schedule**, where the re-timing of planned work made spend look anomalous against
a baseline that no longer applied. The model could not know the re-phasing was approved; the reviewer did.

The function reported the result exactly as it happened: **2 of 3 flags useful** — no rounding up, no
suppression of the miss. That honesty mattered twice over. First, precision of 2-in-3 on early overrun
warning is *genuinely valuable* — two interventions months earlier than the reporting cycle would have forced
them — and stating it plainly kept expectations calibrated for the quarters ahead. Second, the false positive
was caught **because** the verification norm existed: the model widened the professionals' attention; the
professionals decided. Had the ad-hoc culture of month zero still prevailed, the third flag might have
travelled upward as fact and burned the model's credibility on its first outing.

### The incident that proved the governance (13.6)

Mid-programme, the governance was tested by the failure it was designed for. A drafted claims summary,
produced with AI assistance for a commercial review, **cited a contract clause that did not exist** — a
hallucinated reference from an ungrounded prompt, exactly the failure mode 13.6.3 names and 13.3.3b guards
against. The verification checklist (13.6.5) caught it: the reviewer opened the cited clause to confirm it,
found no such clause, and stopped the summary **before it left the team**. The response followed the policy
rather than improvisation: **RAG grounding was made mandatory** for all contract work (13.3.3b — extraction
must cite a real clause or return "not found"); the incident was **logged and shared** across the function,
not buried; and the policy was updated and re-briefed. One near-miss, openly handled, did more for adoption
discipline than any training deck — and the audit trail it generated (**what** the AI produced, **who** caught
it, **what** changed as a result) is precisely the record KA 13.6.2 requires a function to be able to show.

### Where they landed (13.7)

Eighteen months in, an honest reading of the KA 13.7.1 ladder puts the function at **standardised, moving into
integrated**: an approved tool list and AI-use policy in force; verification checklists **embedded in the
month-end and reporting workflows** rather than bolted on; value measured **quarterly** against the 13.7.3
discipline (time, exceptions, warning lead-time — netted against full cost); and **upskilling** — prompting
(13.3) and verification norms (13.3.3, 13.6.5) — written into the controls competency framework, so the
capability lives in the function rather than in a few enthusiasts (13.7.2). The claim stops there: the top
rung is not claimed, because parts of the workflow remain manual and the value evidence is only quarters deep.

| Ladder stage (13.7.1) | The agency's evidence at month 18 |
|---|---|
| Ad-hoc → stopped | Interim policy ended ungoverned public-tool use (month 1) |
| Piloting | Month-end (13.5.4) and predictive EAC (13.5.3) pilots, measured honestly |
| **Standardised** ✔ | Approved tools, policy, embedded verification checklists, incident process |
| **Integrated** (partial) | AI embedded in month-end and reporting; contracts RAG-grounded by mandate |
| Governed & optimised | Not yet claimed — quarterly value measurement and re-validation building toward it |

The outlook is stated the way 13.7.5 demands — honestly. AI capability keeps evolving, so the agency
re-validates its approved tools **quarterly** and expects the workflow map to change. What does not change is
the centre of the design: the professional's sign-off on every AI-influenced number. **AI proposes; the professional verifies, decides and remains accountable.**

### What the credential expects

A candidate should be able to trace each turn of this case to its knowledge area, because the case is the
domain in miniature. The **maturity ladder** (13.7.1) supplied the sequencing — and the discipline to stop
before piloting, and to claim only the rung the evidence supports. **Data as the fuel** (13.2) explains why
six months of coding remediation, not a model, was the programme's real first investment: 2,700 exceptions a
month became 1,260 only through data work. The **governed workflows** (13.3, 13.5.3–13.5.4) show the one
pattern — input → AI step → professional verification → owned output — applied to month-end and to predictive
EAC, with the false positive caught by the verification step working as designed. **Governance** (13.6)
turned a hallucinated clause into a logged incident, a mandatory grounding rule and an audit trail — assurance
functioning, not failing. **Honest value measurement** (13.7.3) netted USD 115,200 of measured saving against
USD 70,000 of full cost and reported 2-of-3 precision without varnish. And through all of it runs the enduring
principle the credential certifies: **AI proposes; the professional verifies, decides and remains accountable.**

---

## Case study B — Domain 13: two pilots and one honest failure at an engineering consultancy (professional services)

### Background

An engineering consultancy of around six hundred staff — civil, structural and environmental practices —
runs some **70 live commissions** at any time, supported by a fourteen-person project-controls and PMO team.
Its delivery raw material is documents and hours: specifications, national standards, terms of engagement,
and roughly **9,600 timesheet lines a week** whose free-text narratives drive client billing. Fee margins
are thin enough that a mis-coded week or a two-day hunt through a specification library is real money. The
firm's AI adoption year produced three results worth teaching: a tool-category decision made the right way
round (13.4), a pilot that **failed** and was reported as a failure (13.2, 13.7.3), and a second pilot that
worked for reasons the failure had just taught. Professional services sharpens the governance stakes: the
firm's outputs are advice under professional indemnity, its data is *clients'* data under confidentiality
agreements, and a hallucinated clause in a deliverable is a liability event, not an embarrassment (13.6.3).

### Choosing the category before the tool (KA 13.4)

The programme's first decision was not which product to buy but **which category fits the practice**
(13.4.4). The workflow map showed where the hours actually went: engineers and bid teams answering
"what does the standard require?" and "what did we commit to on the comparable job?" — retrieval questions
over the firm's own document estate, asked hundreds of times a month and answered by whoever could remember
which project the answer lived in. That profile points at one
category on the 13.4.1 map: **document/RAG and knowledge tools** — grounded answers over a governed corpus,
with citations — not a predictive platform, not a fleet of ungoverned assistants. The category-specific
governance (13.4.2b) was designed in before rollout: the retrieval layer enforces document permissions so a
commission's confidential material answers only its own team; the corpus is curated, with superseded
standards flagged; and two rules are mandatory — every answer **cites the clause it came from**, and where
the corpus does not contain the answer the assistant says **"not found"** rather than improvising (13.3.3b).
The value case, measured over a quarter and stated the honest 13.7.3 way:

1. **Setup.** **450** retrieval queries a month; average time to a verified answer falls from 22 to 6
   minutes — **16 minutes saved** per query; loaded rate **USD 95/hour**. Tooling plus curation and
   governance effort: **USD 60,000 a year**.
2. **Formula.** `annual saving = queries × minutes saved ÷ 60 × rate × 12`; `net = saving − full cost`.
3. **Substitution.** `450 × 16 = 7,200` minutes `= 120` hours a month; `120 × 95 = 11,400`;
   `11,400 × 12 = 136,800`; `136,800 − 60,000`.
4. **Result.** **USD 136,800 a year** gross; **net ≈ USD 76,800 a year**.
5. **Interpretation.** Real, modest, and honestly netted against the curation the category demands — a stale
   corpus produces confidently outdated answers, so keeping it current is part of the price of the saving,
   not an overhead to hide.

### The pilot that failed — and said so (KAs 13.2, 13.5.3, 13.7.3)

The second initiative was the ambitious one: a supervised **ML forecaster** (13.5.3) to predict
margin-at-completion for live commissions from features of past ones — sector, service line, client type,
fee basis, early burn rate. The training set was every completed commission with clean enough records:
**34 projects**. The model looked plausible in development — with a holdout of five projects, almost
anything does — and was run in shadow mode for two quarters. Live, it flagged **9 commissions** as
margin-erosion risks; commission reviews confirmed **2** and dismissed **7**: precision of `2 ÷ 9 ≈ ` **22 %**,
worse than the review meeting it was meant to sharpen, and expensive in the partner attention each false
alarm consumed.

The post-mortem found nothing exotic — it found 13.2. The failure was **representativeness**, the limit
KA 13.1.2 states as the universal truth: a supervised model trained on unrepresentative history will
confidently mislead.

1. **Setup.** **34** completed commissions spanning **6** service lines; the history covers boom years only,
   and one sector (water) supplies nearly half the examples.
2. **Formula.** `examples per service line = projects ÷ service lines`.
3. **Substitution.** `34 ÷ 6 ≈ 5.7`.
4. **Result.** Fewer than **six examples per service line** — and zero examples of a down-cycle.
5. **Interpretation.** No supervised learner generalises from six examples of anything (13.1.2); the model
   had memorised the water sector's good years and projected them onto everything else. The data-quality
   dimensions of 13.2.2 were individually fine — accurate, complete, valid — and the dataset still could not
   carry the use case, because representativeness is a property of coverage, not cleanliness.

What the firm did next is the teachable part. The pilot was **retired**, not quietly extended; the
post-mortem was published internally with the 22 % figure in it; the assembled dataset — the first clean,
consistent commission history the firm had ever built — was **kept** and maintained as the asset it will
become; and margin-risk flagging reverted to rules and ratios (burn rate against stage, unbilled WIP
ageing) that a 34-project firm can actually support — the honest answer to 13.1.6's rules-versus-ML
question at this data scale. A failed pilot honestly measured and cleanly killed is 13.7.3's value
discipline working, and it bought the programme credibility a buried failure would have spent.

### The pilot that worked — timesheet narratives (KAs 13.5.4, 13.1.6)

The failure re-aimed the programme at a problem with the opposite data profile. Every week, **9,600
timesheet lines** of free-text narrative must be coded to commission, task and billable status before
billing runs — at roughly half a minute a line, **80 hours a week** of coding effort spread across team
leads. As a classification task it is everything the forecaster was not: a
labelled example arrives with every line ever coded — about **480,000 a year** `(9,600 × 50)` — the
categories are stable, and an error is cheap to catch and correct. The deployment followed the 13.5.1
pattern — AI step inside a governed workflow, professional verification on the output. A first trial
auto-coded **88 %** of lines confidently; tracing the exceptions showed dead and duplicated task codes
confusing the classifier, so the code list was cleaned *first* (the 13.2.1 lesson re-learned at small
scale), lifting first-pass confidence to **94 %**.

1. **Setup.** 9,600 lines a week. Before: manual coding at 0.5 min/line. After: machine codes; humans work
   the **6 %** exception queue at 2 min/line plus **4.8 hours** a week of sampling the high-confidence
   population. Loaded rate **USD 75/hour**; tooling plus verification cost **USD 85,000 a year**;
   50 working weeks.
2. **Formula.** `hours before = lines × 0.5 ÷ 60`; `hours after = (lines × 6 % × 2 ÷ 60) + sampling`;
   `net = (hours saved × weeks × rate) − full cost`.
3. **Substitution.** Before: `9,600 × 0.5 ÷ 60 = 80` hours. After: `9,600 × 6 % = 576` lines;
   `576 × 2 ÷ 60 = 19.2` hours; `19.2 + 4.8 = 24` hours. Saved: `80 − 24 = 56` hours a week;
   `56 × 50 = 2,800` hours; `2,800 × 75 = 210,000`; `210,000 − 85,000`.
4. **Result.** **USD 210,000 a year** gross; **net ≈ USD 125,000 a year** — the programme's largest verified
   saving.
5. **Interpretation.** The sampling line in the cost is not optional: coded time drives **client invoices**,
   so the high-confidence population is audited on a cycle, and every code stream that bills a client gets
   periodic human eyes (13.6.2 — a named professional stands behind the invoice, not the classifier). The
   unpriced benefit is stated as unpriced: narratives coded the same day they are written, so WIP and
   billing cut-off improve in ways the team saw but did not monetise.

Set side by side, the two pilots taught the firm the domain's data lesson better than any course: the same
technique family failed at 34 examples and succeeded at 480,000. **The data decided; the tool never had a
vote** (13.2.1).

### Scaling on the ladder — and keeping the judgement (KAs 13.7.1–13.7.4)

Against the 13.7.1 maturity ladder, the firm ended the year at **standardised**: an approved-tool register
and AI-use policy (13.6.5) covering client-confidentiality classifications; the RAG assistant and the
timesheet coder embedded in daily work; the forecaster formally retired with its post-mortem on record; and
value measured quarterly, gross and net. **Integrated** is claimed only for the two working deployments —
nothing above it is claimed, and the failed pilot is cited internally as evidence the claims can be
trusted. The scaling plan runs category by category along the 13.4 map — contract-analytics for terms of
engagement next — each with the same governance floor.

One design choice guards the long game. Because auto-coding removes the drudgery that once taught juniors
what the codes *mean*, every controls analyst still hand-codes **one week per quarter**, and exception-queue
duty rotates — deliberate friction, kept on purpose, so the judgement that verification depends on keeps
being produced (13.7.4). A team that can no longer code a timesheet cannot audit a classifier that codes
half a million of them.

### What the credential expects

A candidate should be able to defend every decision in this case from the syllabus. **Category before
tool** (13.4.4): a document-heavy practice maps to document/RAG on the 13.4.1 grid, with the
category-specific governance — permission-aware retrieval, curated corpus, citations, "not found" — designed
in, not bolted on. **Representativeness as a hard limit** (13.2, 13.1.2): 34 projects across six service
lines is not a training set, and clean data does not rescue thin coverage; the professional response is
13.1.6's — let rules beat ML where the history is short.
**Honest failure as programme capital** (13.7.3): the 22 % precision was published, the pilot retired, the
dataset kept — value measurement that permits failure is what makes its successes believable. **The
governed-workflow pattern** (13.5.1, 13.5.4) carried the winning pilot: data cleaned first, machine coding
inside tolerances, an owned exception queue, sampling wherever the output touches a client invoice
(13.6.2). And **maturity claimed on evidence** (13.7.1) with **judgement deliberately maintained** (13.7.4)
closes the loop: the ladder is climbed rung by rung, and the humans stay skilled enough to check the
machine. Through both pilots, the failure and the scaling, one line governed every sign-off: **AI proposes; the professional verifies, decides and remains accountable.**

---

## Executive perspective — Domain 13

**What the executive must hold onto.** AI changes the **economics** of controls work — coverage no team of
readers can match, month-ends in hours rather than days, warnings months before the reporting cycle would force
them — but it does not change the **accountability**: a model cannot sign off, so a named professional must,
and "the model said so" is not a defence anyone in the chain can offer (KA 13.6.2). **Capability without
governance is risk**, and the sequencing matters — policy and verification first, pilots second, integration
only on that floor. And read incidents correctly: the near-miss you hear about is the governance working; the
one you never hear about is the risk.

**Six questions to ask from the chair.**

1. What did our verification checklists catch last quarter — and what changed as a result?
2. Which tools are on the approved register, for which data classifications — and where is our confidential
   data actually going?
3. What is the net value of the AI programme, with tooling *and* governance/verification costs netted against
   measured savings?
4. Who signed off this AI-assisted forecast, and can we produce the trail — what the model proposed, who
   reviewed it, what changed?
5. Which rung of the maturity ladder are we actually on, and what evidence supports the claim?
6. Where have we decided *not* to use AI, and why?

**The traps at board level.**

- **Value claimed gross.** A saving that evaporates when you price the checking is not a saving; an honest case
  nets the licences and the human verification the governance model requires (KA 13.7.3). "We use AI" is not
  value — a faster, verified close is.
- **Precision mistaken for truth.** A model's output is plausible, not verified — and a more capable model that
  is wrong is more *convincingly* wrong, so advancing capability raises the stakes of assurance rather than
  lowering them (KA 13.7.5).
- **The data work skipped.** The unglamorous remediation of coding and quality *is* the AI programme; garbage
  in, garbage out is the business case's first line item, not a footnote (KA 13.2).
- **Silence read as safety.** A function reporting no AI incidents is more likely ungoverned than infallible —
  in an ad-hoc culture the hallucinated clause travels upward as fact instead of landing in an incident log.

**What good looks like.** An approved-tool register and AI-use policy are in force and re-validated as
capabilities change; verification checklists are embedded in the month-end, reporting and commercial workflows
rather than bolted on; and every material AI-assisted output carries a named sign-off with its audit trail.
Value is measured quarterly and reported without varnish — two useful flags in three is stated as exactly that
— and near-misses are logged, shared and turned into policy rather than buried. The professionals have moved
up, not out: directing and assuring AI-assisted production, pairing domain mastery with AI fluency. **AI proposes; the professional verifies, decides and remains accountable.**

---

## Practitioner's toolkit — Domain 13

*Adoption-ready artefacts; adapt the column headings and thresholds to your organisation, then keep them
stable.*

### Toolkit 13.T.1 — AI tool register template

The approved-tool register the model policy requires (13.6.5c, §2): each tool, its permitted data classes and
uses, and its assurance standard. Three illustrative rows — an assistant, a RAG system, an ML forecaster.

| Tool | Category (13.4) | Approved data classes | Grounding required? | Verification standard | Owner | Last validated | Status |
|---|---|---|---|---|---|---|---|
| Enterprise LLM assistant (governed) | General LLM assistant | Internal; no personal or client-confidential data | N/A — no document claims accepted without a source | 13.6.5 checklist on material outputs; every figure recomputed against source | Controls director | Q2 2026 | Approved |
| Contract knowledge base | Document / RAG | Confidential (permission-aware corpus) | Yes — every answer cited to a clause | Citations opened and confirmed (13.3.3b); legal review of entitlement-bearing terms | Commercial manager | Q2 2026 | Approved |
| Cost-forecast model | Risk & forecasting / ML | Internal (governed cost history, 13.2.3) | N/A — driver visibility required (explainability) | `EAC` recomputed; method vs variance cause; `TCPI` reality check (13.5.3) | Head of estimating | Q1 2026 | Revalidation due |

**Usage note.** The register is the operational form of the policy's approved-tools rule: no data may enter a
tool above the data classification for which that tool is approved, and a governed enterprise alternative is
used in preference to a public tool (13.2.5, 13.6.5c §2). The category column ties each tool to its
category-specific governance risk from 13.4.2b — hallucination for assistants, source-access control and
corpus currency for RAG, explainability and training-data representativeness for ML. The last-validated
column enforces the quarterly re-validation cadence (13.6.5c §7), because capabilities and data-handling
terms change: the forecaster's "Revalidation due" status means its outputs carry extra scrutiny until the
owner re-validates. Keep the columns stable so the register can be audited period over period.

### Toolkit 13.T.2 — AI-output verification record

The 13.6.5 assurance checklist as a reusable form — one row per material AI-assisted output, filed as part of
the audit trail (13.6.2). The example row is the AI-drafted `EAC` of worked example 13.5.10a.

| Output | Source-checked | Method/assumption sound | Grounded (no hallucination) | Confidentiality | Cross-checked | Signed off by | Date |
|---|---|---|---|---|---|---|---|
| AI-drafted `EAC` USD 1,180,000, master project (13.5.10a) | ✔ recomputed from `AC`/`EV`/`BAC` and indices | ✔ method matches the variance cause; `TCPI` reality-checked | ✔ every claim traces to data; no fabricated figures | ✔ produced in a governed, registered tool | ✔ consistent with the critical path and prior period | Lead cost engineer (named) | 30 Jun 2026 |

**Usage note.** The form turns the verification checklist from a habit into a record: for each material
output — forecasts, disclosures, commercial positions, board reporting — it captures what the AI produced,
who verified it, and on what basis, which is exactly the audit trail the policy requires and the defence a
challenged number needs (13.6.2, 13.6.5c §4). An output that fails any column is not released until the
failure is fixed — footnoting a known inconsistency defeats the assurance the record exists to provide
(MCQ 13.6-D). The columns flex slightly by output type (an extracted contract term adds a legal-review
column, 13.6.5b) but the shape — propose, verify line by line, sign off by name — never does. **AI proposes; the professional verifies, decides and remains accountable.**

---

## Exam preparation — Domain 13

**How this domain is examined.** As the 20 % AI domain, Domain 13 is examined more by **recall and analysis**
than by arithmetic: the nesting (GenAI ⊂ ML ⊂ AI), the LLM vocabulary, RAG vs fine-tuning, category-to-task
fit, and — above all — the governance reasoning of KA 13.6. Where numbers appear they concentrate in three
places: **data-quality percentages** (KA 13.2), **value cases** (KA 13.7.3) and **`EAC` arithmetic embedded in
verification scenarios** (KA 13.5.3). The scenario items almost always hinge on identifying the professional's
verification step, not the AI's capability.

**Calculation traps.**

- **Data-quality percentages mis-combined.** With no overlap stated, failure rates **add** across dimensions:
  4 % + 2 % + 5 % of 20,000 rows is 2,200 — counting a single dimension gives the distractors (MCQ 13.2-C).
- **Value claimed gross.** An honest value case **nets** tooling *and* governance/verification cost from the
  measured saving — the gross annual figure, and the figure with the cost *added*, both sit among the options
  (MCQ 13.7-C, worked example 13.7.3).
- **`EAC` arithmetic inside AI items.** `EAC = BAC/CPI` — dividing, not multiplying: at a `CPI` of 0.96 the
  forecast *rises* above `BAC` (MCQ 13.5-E). The AI framing does not change the Domain 6 arithmetic.
- **"The model said so" as a defence.** It never is — the credited answer is always the documented
  verification and named sign-off (MCQs 13.5-B, 13.6-A); any option that releases an unchecked output is
  wrong.
- **Majority-pass reasoning on the checklist.** An output that fails **any** assurance-checklist line is
  withheld until fixed — footnoting the failure, or releasing because most lines passed, are distractors (MCQ
  13.6-D).
- **RAG confused with fine-tuning.** RAG supplies documents **at inference**; fine-tuning **retrains**.
  Neither removes the need to verify (MCQ 13.1-C).

**Time management.** Definitional items — tokens, temperature, the maturity ladder — are quick: clear them
early. For scenario items, ask one question before reading the options: *where is the professional's
verification step?* The option containing it is nearly always the key, and the options that skip it eliminate
themselves.

**Reflection questions.**

1. Which AI tools touch your controls outputs today, and would each survive the approved-register tests —
   data classification, grounding, auditability?
2. Where in your own workflow is the unglamorous data work — coding, de-duplication, lineage — on which any AI
   initiative would actually depend?
3. If an AI-assisted number you signed off were challenged a year from now, could you produce the trail —
   what the model proposed, who reviewed it, what changed?
4. When you last accepted an AI-drafted figure, extraction or narrative, what did you actually recompute or
   open to source — and would your own verification discipline survive the scrutiny you apply to others'?

**AI proposes; the professional verifies, decides and remains accountable.**

---

## Domain 13 summary

AI is a major, practical part of modern project controls — and the governing principle, **"AI proposes; the professional verifies, decides and remains accountable,"** never bends. The domain builds from **foundations** (AI ⊃ ML ⊃ GenAI; supervised/
unsupervised/reinforcement learning; LLM concepts — tokens, context, temperature; training/inference/fine-
tuning/RAG; honest strengths and hard limits; rules vs ML vs GenAI) through **data** (garbage in, garbage out;
quality dimensions; governance and lineage; controls data sources; confidentiality) and **prompting**
(components, patterns, iterative refinement, and the non-negotiable verification step, with guardrails) to the
**tool categories** (assistants, RAG, data-analysis, BI, PM-suite, ML, RPA/process-mining, CLM, meeting,
coding — matched to task and governance). Its heart is **AI applied across the lifecycle** — ten hands-on
workflows, each *input → AI step → professional verification → owned output*, cross-referencing every earlier
domain. It closes with **governance, ethics and assurance** (accountability, sign-off, auditability;
hallucination/bias/confidentiality; when not to use AI; an AI-use policy and verification checklist) and
**building the capability** (maturity model, integration, upskilling, value measurement, pitfalls, and an
honest outlook). The durable professional skill is the judgement to **direct, verify and own** AI-assisted work
— exactly what the PCL-AI certifies.

**Cross-references.** This domain draws on all others: estimating → 3.2; forecasting/EVM → 3.4, 6.3;
cost coding/control → 1.5, 5.2; scheduling → 10; agile/AgileEVM → 9.5; contracts → 7; reporting → 4;
risk → 12; financial reporting/IFRS 15/IAS 37 → 1–2. The "AI in this domain" boxes throughout the book are the
per-chapter instances of the workflows collected here.

