---
platform:      DEV Community
type:          how-to
title:         Generative AI project reporting with an audit trail
meta:          Generative AI project reporting is safe when the model writes words and never sources numbers. The provenance manifest, a worked variance, a ledger check.
primary_kw:    generative AI project reporting
secondary_kw:  IFRS 15 five-step model, cost-to-cost method, cash conversion cycle, estimate at completion
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     canonical -> /generative-ai-project-reporting (own site #070)
schema:        HowTo
word_count:    1785
hashtags:      #ai #python #architecture #productivity
ab_id:         AB-00157
---

# Generative AI project reporting with an audit trail

Generative AI project reporting is safe when the model writes the words and never sources the numbers. Fix the inputs at a stated cut-off, compute every figure where the control accounts live, pass those figures into the prompt, and keep the inputs, prompt, model version, reviewer and owner with the pack.

Built that way, a monthly commentary takes an afternoon rather than a week. Built the other way, you ship a well-written report nobody can reconstruct when it is challenged.

## What is generative AI project reporting?

It is the use of a language model to draft the narrative sections of a project report — variance commentary, risk summaries, milestone narrative, executive summaries — from figures produced elsewhere and supplied to it.

The definition contains the control. The model is a writing function applied to a fixed input set, never a source of that set.

The architecture that follows is unremarkable: compute, serialise, generate with the inputs frozen, then write a manifest alongside the output.

## Which parts of a report can a model draft?

Most of the prose and none of the arithmetic. The table is the working split, and the right-hand column is the one that matters.

| Report section | Model role | Evidence required | Who signs |
|---|---|---|---|
| Cost variance commentary | Drafts from supplied EV, AC, PV, CPI, SPI and the agreed cause | Cost ledger extract at the stated cut-off | Cost manager |
| Schedule narrative | Drafts from supplied dates, float and driving path | Schedule export and recalculation log | Planning manager |
| Risk summary | Drafts from the register; flags rows unchanged for three periods | Risk register version and review minutes | Risk owner |
| Change and claims log | Summarises status from the change register | Signed instructions and correspondence | Commercial lead |
| Cash and funding note | Drafts from supplied cash forecast and drawdown position | Treasury forecast and bank position | Finance lead |
| Revenue and margin note | Drafts from supplied recognised revenue and margin | Ledger, contract, cost-to-cost workings | Finance lead |
| Executive summary | Assembles from the sections above, nothing new | The sections themselves | Project director |
| Forecast to complete | Explains a method already chosen | The EAC calculation and the method decision | Project director |

Whoever signs the figure was accountable for it before the model existed and remains accountable after. No pipeline design changes that.

## The provenance manifest

Six items, written next to the generated text at generation time. Each is cheap to capture and painful to reconstruct eight months later when a figure is challenged. The cut-off itself comes from [the close timetable the project runs to](https://projectcontrolsinstitute.org/month-end-close-for-projects), not from whenever the pack was assembled.

```json
{
  "cut_off": "2026-07-31T23:59:59Z",
  "inputs": {"bac": 86.0, "pv": 31.0, "ev": 28.2, "ac": 30.6,
             "cpi": 0.9216, "spi": 0.9097, "eac": 93.3, "tcpi": 1.04,
             "currency": "USD", "unit": "m"},
  "input_source": "cost_ledger_export_2026-07.csv (sha256: 4f1c…)",
  "prompt_template": "variance_commentary@v7",
  "model": {"provider": "…", "id": "…", "version": "2026-05-14"},
  "raw_output_ref": "artefacts/var_commentary_raw.md",
  "reviewer": "…",
  "owner": "…",
  "checks": ["inputs reconciled to ledger extract",
             "no figure recomputed by model",
             "no cause proposed by model"]
}
```

Two design choices carry the weight. The hash means the extract cannot drift after the fact, and storing the raw output separately keeps the reviewer's edits visible rather than flattened into the final text.

## Worked example: the variance paragraph

Take a contract with a budget at completion (BAC) of $86.0m. At the cut-off, planned value (PV) is $31.0m, earned value (EV) is $28.2m and actual cost (AC) is $30.6m.

Cost variance is EV − AC = 28.2 − 30.6 = **−$2.4m**. Schedule variance is EV − PV = 28.2 − 31.0 = **−$2.8m**.

The indices are CPI = 28.2 ÷ 30.6 = **0.92** and SPI = 28.2 ÷ 31.0 = **0.91**.

On the assumption that cost performance continues, the estimate at completion is BAC ÷ CPI = 86.0 ÷ 0.9216 = **$93.3m**, giving a variance at completion of 86.0 − 93.3 = **−$7.3m**.

The to-complete performance index needed to finish on budget is (BAC − EV) ÷ (BAC − AC) = 57.8 ÷ 55.4 = **1.04**. A team running at 0.92 is being asked to run at 1.04 for every remaining dollar, and the commentary has to say so.

The model receives those figures, the agreed cause, a word limit and the house style, and is told to recompute nothing and propose no cause. It returns a paragraph closing with a line naming every figure used, which the reviewer ticks against the extract.

What it does not return is an explanation of why performance slipped, because that came from the project review and was supplied rather than generated.

## Where the reported number meets the ledger

This is the boundary a drafting tool cannot see, and it is where contracting businesses lose money. An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on a driving path.

IFRS 15 sets out a five-step model, described here in PCI's own words. Identify the contract with the customer, then identify the distinct performance obligations within it.

Determine the transaction price, including variable consideration and the limit on how much of it may be recognised. Allocate that price across the obligations. Recognise revenue as each obligation is satisfied, over time where the criteria for that are met, otherwise at a point in time.

Step five is where the project report and the ledger meet. Under a cost-to-cost input method, progress is the ratio of costs incurred that genuinely depict progress to the total costs expected, and costs that do not depict progress — significant uninstalled materials, rectification of defective work — are excluded from the measure.

Use the figures above. Costs incurred are $30.6m and total expected costs are the $93.3m estimate at completion, so the measure is 30.6 ÷ 93.3 = **32.8%**.

On a transaction price of $110.0m, revenue recognised to date is 0.328 × 110.0 = **$36.1m**.

Now suppose the project report states physical progress of 40%, measured from work in place. Applying that to the same price gives 0.40 × 110.0 = **$44.0m**, a difference of **$7.9m** on identical facts.

Neither percentage is wrong. They answer different questions, and a report saying "the project is 40% complete" without saying which 40% has produced a figure that is correct for the site meeting and misleading for the ledger. A model drafting from that sentence reproduces the ambiguity faithfully and confidently, which is exactly the class of bug that never throws.

Name the measure in the field: `progress_pct_cost_to_cost` and `progress_pct_physical` are different keys, and a schema allowing only `progress_pct` is a defect waiting for a month-end.

## Why the pack needs a cash line as well as a margin line

Recognised revenue is not money received, and contracting businesses fail on the second while reporting well on the first.

The cash conversion cycle measures how long cash is tied up: days inventory outstanding, plus days sales outstanding, minus days payables outstanding. For a contractor the first term is unbilled work in progress.

Take 35 days of work in progress, 68 days to collect and 45 days taken to pay suppliers. The cycle is 35 + 68 − 45 = **58 days** of working capital funded by the business.

At an annualised $110.0m, one day of receivables is 110.0 ÷ 365 = **$0.30m**. Taking ten days out of collection releases roughly **$3.0m** of cash without changing revenue, margin or a single line of the delivery plan.

That arithmetic belongs in the pack a model helps you draft, and it is the part most often missing when the pack is written and reviewed entirely by delivery people.

## What must never be generated

**The numbers**, computed where the ledger, cost tool or schedule lives and then supplied. A model can still be pointed at a schedule export to [find structural defects a planner then verifies in the tool](https://pciai.org/llm-schedule-review), which is a search task rather than a sourcing one.

**The cause of a variance**, which is a finding from a review rather than an inference from a trend.

**Quotations**, because no output should be attributed to somebody who did not say it. **Contract interpretation**, because what an entitlement clause means is a commercial question with money attached.

**Forward-looking commitments**, because a recovery date the delivery team has not agreed becomes a promise the moment it is printed. A regex over the draft for date patterns and modal verbs catches most of these before a human does.

## How PCI examines this

The PCI AI Project Finance Leader (PFL-AI) credential has 16 domains and 61 knowledge areas, and covers the point where project reporting meets financial reporting: revenue recognition, cut-off, contract assets and liabilities, provisions, and the cash position behind the margin.

The Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Can the model read the cost ledger and write the report itself?**
It can read, but not source. Extraction with a verification step is reasonable; taking figures straight into narrative removes the point at which anyone confirms the extract matches the ledger at the cut-off. Extract, reconcile, then draft, with the reconciliation recorded as a check in the manifest.

**Should this run in CI?**
The computation and the schema validation should, because both are deterministic and both fail loudly. Generation itself is better triggered manually at cut-off, since a model call inside a build is non-deterministic, costs money on every run and produces text that still needs a named reviewer before it means anything.

**How much time does it actually save?**
The saving is in drafting and reformatting, not analysis or review. Teams that measure it usually find the first draft collapses while review stays flat or grows slightly, because reviewers now check provenance as well as prose. Measure your own before and after rather than accepting a vendor figure.

**Will an auditor object to AI-assisted commentary?**
Not to the assistance, because the questions an auditor asks are about evidence rather than tools. The objection arrives when provenance is missing and a figure in a signed pack cannot be reproduced from its inputs. Keep the manifest and an AI-assisted paragraph is no harder to support than a typed one.

**How do we stop commentary becoming generic month to month?**
Make the cause a required field and fail generation when it is empty. Generic commentary is almost always the product of a prompt that supplied figures and nothing else, leaving the model to fill the gap with management prose. If nobody can state the cause, that absence is itself the finding the report should carry.

---

*First published on pciai.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the full method including the manifest is here.*

*Linking note — the links now in the body: "the close timetable the project runs to" points at projectcontrolsinstitute.org/month-end-close-for-projects from the provenance manifest section, because fixing inputs at a cut-off raises what sets that cut-off; "find structural defects a planner then verifies in the tool" points at pciai.org/llm-schedule-review from the list of what must never be generated, because saying the schedule owns its own numbers raises what a model may legitimately do with a schedule export. Two links, one per domain — the IFRS 15 passage here is written in PCI's own words and stands without one. Reciprocal: the month-end close guide could point at this piece where it reaches the commentary drafted after the cut-off.*
