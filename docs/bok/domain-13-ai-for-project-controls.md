# Domain 13 — AI for Project Controls & Project Management: Concepts, Tools & Practice

> **Group:** AI knowledge & practical approach (the 20 % AI domain). **Target:** ~300 pages — a major,
> practical domain, not an appendix. **Binds to:** [`00-style-spine.md`](00-style-spine.md). British English;
> USD (+SAR where useful). Tools are named as **categories and representative examples**, vendor-neutrally,
> with **no fabricated features**; where a capability is evolving it is flagged as such (Spine §9).

## Why this domain exists

Artificial intelligence is changing how project controls and project management are done — but the credential's
governing principle never changes: **"AI proposes, the professional disposes."** AI can draft, extract,
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
reporting, with the professional's verification shown; govern and assure AI use ("AI proposes, the professional
disposes"); and build and mature an AI-augmented controls capability.

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
- A. AI ⊂ ML ⊂ GenAI
- B. GenAI ⊂ ML ⊂ AI ✅
- C. ML ⊂ GenAI ⊂ AI
- D. They are unrelated fields.

*Rationale:* GenAI is a subset of ML, which is a subset of AI. The others invert the nesting.

**MCQ 13.1-B `[13.1.6 · Analysis]`** Flagging invoices whose PO price and invoice price differ is best done with:
- A. Generative AI.
- B. Rules/automation (deterministic logic). ✅
- C. Reinforcement learning.
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
- B. The model learns and reproduces the mis-coding, giving misleading forecasts. ✅
- C. No effect — models are robust to bad data.
- D. Better accuracy.

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

### 13.3.3 Iterative refinement and verification

**The principle.** GenAI is used **iteratively**: prompt, review, refine. And — the non-negotiable step — every
output is **verified** before use. For a controls professional this means: check figures against source
(a model can miscalculate or fabricate), check extracted data against the document, check a narrative's causal
claims against the actual variance analysis. The verification step is where "AI proposes, the professional
disposes" becomes concrete.

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
clauses. **AI proposes, the professional disposes.**

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
- B. Verify the figures and causal claims against source before use. ✅
- C. Increase the temperature.
- D. Delete the source data.

*Rationale:* Verification is the step that makes "AI proposes, the professional disposes" real — a model can
miscalculate or over-claim. Publishing unverified output, changing temperature, or deleting source all fail the
principle.

**MCQ 13.3-B `[13.3.1 · Recall]`** Which most improves a professional GenAI prompt?
- A. Making it as short and vague as possible.
- B. Supplying role/context, a clear task, the data, the desired format and constraints. ✅
- C. Omitting the audience.
- D. Requesting maximum creativity for factual tasks.

*Rationale:* Context-rich, specific prompts yield useful output; vagueness yields generic output. For factual
tasks, low creativity (temperature) is preferred.

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
| **General LLM assistants** | Draft, extract, summarise, analyse text | Claude, ChatGPT, Gemini, Copilot-class | Confidentiality; verify outputs |
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
disposes.

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
- B. Document / RAG grounded in the contract set. ✅
- C. A meeting assistant.
- D. RPA.

*Rationale:* The question is a grounded document query — RAG over the contracts gives cited, source-based
answers. A general LLM without the documents risks hallucination; the others do not fit.

**MCQ 13.4-B `[13.4.3 · Recall]`** A stated reason to note that "features change" when naming AI tools is:
- A. Tools never improve.
- B. Capabilities evolve rapidly, so a professional validates current features rather than assuming claims. ✅
- C. All tools are identical.
- D. To avoid using AI.

*Rationale:* AI capabilities change quickly; responsible use validates current capability. The other options
are false or contrary to the domain's stance.

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

**The workflow shape.** Every application below follows one shape — the operational form of "AI proposes, the
professional disposes":

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
   **AI proposes, the professional disposes.**

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

### 13.5.5 Scheduling (Domain 10)

- **Workflow.** Input: WBS/activities/logic → AI step: propose a schedule, check logic (missing links, dangling
  activities, excess constraints/lags), predict delays from progress → Verification: the professional validates
  the logic and durations and re-identifies the critical path (Domain 10) → Output: a sound, progressed
  schedule.
- **Value & limit.** Strong for logic-checking and delay prediction; an AI schedule can embed hidden
  constraints or unrealistic durations — validate before trusting.

### 13.5.6 Agile delivery (Domain 9)

- **Workflow.** Input: backlog + Sprint history → AI step: draft/split stories, forecast velocity and release
  completion, detect flow anomalies, draft agile reports → Verification: the professional owns estimates,
  commitments and the scope-change/rebaselining narrative (Domain 9, KA 9.5) → Output: forecasts and reports
  with human accountability.
- **Value & limit.** Useful for backlog and forecasting; a model that treats story points as absolute or
  ignores rebaselining misleads.

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

### Key terms — KA 13.5

| Term | Meaning |
|---|---|
| **Propose → verify → own** | The universal AI-in-controls workflow shape. |
| **Driver analysis** | AI explanation of *why* a metric is moving (e.g. an EAC). |
| **Auto-coding / reconciliation** | AI coding cost and matching it to the ledger. |
| **AI-assisted disclosure/forecast** | AI-drafted output the professional verifies and signs off. |

### Sample MCQs — KA 13.5

**MCQ 13.5-A `[13.5.1 · Recall]`** The universal shape of an AI-in-controls workflow is:
- A. AI decides → professional observes.
- B. Input → AI step → professional verification/decision → owned output. ✅
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
- B. Reproducing a cut-off error at scale (Domain 1, KA 1.3.5). ✅
- C. Improving cut-off accuracy.
- D. Violating IFRS 15 only.

*Rationale:* Accrual follows the *service* date; keying off the invoice date reproduces a classic cut-off error
across every accrual — the professional must own the accrual logic.

### Self-check — KA 13.5

1. State the three-step AI workflow and what each step contributes. *(Input → AI step (accelerate) →
   verification (assure) → owned output.)*
2. Give one high-value, lower-risk AI application and one higher-judgement one. *(Lower-risk: cost coding/
   reconciliation; higher-judgement: provisioning/revenue recognition, contingency.)*

---

## Knowledge Area 13.6 — Governance, ethics, risk & assurance of AI

*Topics: 13.6.1 "AI proposes, the professional disposes" · 13.6.2 accountability, sign-off and auditability ·
13.6.3 hallucination, bias and confidentiality · 13.6.4 when not to use AI · 13.6.5 an AI-use policy and
verification checklist.*

### 13.6.1 "AI proposes, the professional disposes"

**The governing principle.** The credential's principle is not a slogan — it is a governance rule: AI may
**propose** (draft, extract, forecast, detect), but a **qualified professional disposes** (decides, verifies,
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

### Key terms — KA 13.6

| Term | Meaning |
|---|---|
| **AI proposes, professional disposes** | AI drafts/predicts; a qualified professional decides and is accountable. |
| **Auditability / sign-off** | Keeping the trail of what AI produced, who approved it, what changed and why. |
| **Hallucination / bias / confidentiality** | The three principal AI risks and their mitigations. |
| **AI-use policy / verification checklist** | The governance document and the operational assurance step. |

### Sample MCQs — KA 13.6

**MCQ 13.6-A `[13.6.2 · Analysis]`** When an AI-assisted forecast is later challenged, an acceptable defence is:
- A. "It was the model's output."
- B. The documented verification and named sign-off showing how it was checked and owned. ✅
- C. "The model is very advanced."
- D. Deleting the audit trail.

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
- A. Trust the model more.
- B. Verify every figure/citation against source, use RAG grounding and low temperature. ✅
- C. Increase temperature.
- D. Paste more confidential data.

*Rationale:* Hallucination is mitigated by verification, grounding and low temperature. The other options
increase risk.

### Self-check — KA 13.6

1. State the governing principle and why a model cannot satisfy accountability. *(AI proposes, the professional
   disposes; a model cannot be accountable — a named person is.)*
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
accurate, every workflow reproducible, and the professional's judgement central throughout. **AI proposes, the
professional disposes.**

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
- A. Saves the most time safely.
- B. Invites the risks of ungoverned AI (hallucination, confidentiality, no audit trail). ✅
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

### Self-check — KA 13.7

1. Name the stages of the AI-maturity model. *(Ad-hoc → piloting → standardised → integrated → governed/
   optimised.)*
2. Why does advancing AI capability *increase* the need for governance? *(A more capable model that is wrong is
   more convincingly wrong; the stakes of verification rise.)*

---

## Domain 13 summary

AI is a major, practical part of modern project controls — and the governing principle, **"AI proposes, the
professional disposes,"** never bends. The domain builds from **foundations** (AI ⊃ ML ⊃ GenAI; supervised/
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
— exactly what the PCP-AI certifies.

**Cross-references.** This domain draws on all others: estimating → 3.2; forecasting/EVM → 3.4, 6.3;
cost coding/control → 1.5, 5.2; scheduling → 10; agile/AgileEVM → 9.5; contracts → 7; reporting → 4;
risk → 12; financial reporting/IFRS 15/IAS 37 → 1–2. The "AI in this domain" boxes throughout the book are the
per-chapter instances of the workflows collected here.

*Domain 13 is a first authored draft pending SME (finance, agile and AI) review before it feeds the exam
blueprint. This completes the AI group (Domain 13, ~20 % of the book).*
