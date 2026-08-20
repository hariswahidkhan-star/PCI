---
platform:      Own site — pciai.org
type:          how-to
title:         Generative AI project reporting: keep the audit trail
meta:          Generative AI project reporting works when the numbers are computed outside the model. The six-item audit trail, a worked variance paragraph, a ledger check.
primary_kw:    generative AI project reporting
secondary_kw:  IFRS 15 five-step model, cost-to-cost method, cash conversion cycle, estimate at completion
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     original
schema:        HowTo
word_count:    1804
hashtags:      n/a (own site)
ab_id:         AB-00157
---

# Generative AI project reporting: keep the audit trail

Generative AI project reporting is safe when the model writes the words and never sources the numbers. Fix the inputs at a stated cut-off, compute every figure where the control accounts live, supply those figures in the prompt, and keep the inputs, prompt, model version, reviewer and owner with the pack.

Done that way, a monthly commentary takes an afternoon rather than a week. Done the other way, you get a well-written report nobody can reconstruct when it is challenged.

## What is generative AI project reporting?

It is the use of a language model to draft the narrative sections of a project report — variance commentary, risk summaries, milestone narrative, executive summaries — from figures produced elsewhere and supplied to it.

The definition contains the control. The model is a writing tool applied to a fixed input set, never a source of it.

## Which parts of a report can a model draft?

Most of the prose and none of the arithmetic. The table is the working split, and it follows the same rule that decides [which project controls tasks AI is good for](https://pciai.org/ai-in-project-controls).

| Report section | Model role | Evidence required | Who signs |
|---|---|---|---|
| Cost variance commentary | Drafts from supplied EV, AC, PV, CPI, SPI and the agreed cause | Cost ledger extract at the stated cut-off | Cost manager |
| Schedule narrative | Drafts from supplied dates, float and driving path | Schedule export and recalculation log | Planning manager |
| Risk summary | Drafts from the register; flags rows unchanged for three periods | Risk register version and review minutes | Risk owner |
| Change and claims log | Summarises status from the change register | Signed instructions and correspondence | Commercial lead |
| Cash and funding note | Drafts from supplied cash forecast and drawdown position | Treasury forecast and bank position | Finance lead |
| Revenue and margin note | Drafts from supplied recognised revenue and margin | Ledger, contract, cost-to-cost workings | Finance lead |
| Executive summary | Assembles from the sections above, nothing new | The sections themselves | Project director |
| Forecast to complete | Drafts the explanation of a method already chosen | The EAC calculation and the method decision | Project director |

The rule running through the right-hand column: whoever signs the figure was accountable for it before the model existed and remains accountable after.

The schedule narrative row has an exercise upstream of it. Run [the structural checks a model can make over a programme](https://pciai.org/llm-schedule-review) before those dates reach a report, because a narrative drafted over a defective network is fluent and wrong.

## What must the audit trail contain?

Six items, kept with the pack. Each is cheap to capture at the time and painful to reconstruct later.

The **input set** exactly as supplied, with the cut-off date and time. The **prompt or template**, in the version used. The **tool and model version**, because providers update models without changing the name.

The **output as first generated**, before editing, so the reviewer's changes are visible. The **reviewer and the owner**, by name. The **checks performed**, listed rather than implied.

That is the same evidence an auditor has always wanted from a spreadsheet-derived number. Nothing about a language model changes the question, and [the provenance clause of a controls AI policy](https://pciai.org/ai-policy-for-project-controls) is where the requirement gets written down.

## Worked example: the variance paragraph

Take a contract with a budget at completion (BAC) of $86.0m. At the cut-off, planned value (PV) is $31.0m, earned value (EV) is $28.2m and actual cost (AC) is $30.6m.

Cost variance is EV − AC = 28.2 − 30.6 = **−$2.4m**. Schedule variance is EV − PV = 28.2 − 31.0 = **−$2.8m**.

The indices are CPI = 28.2 ÷ 30.6 = **0.92** and SPI = 28.2 ÷ 31.0 = **0.91**.

On the assumption that cost performance continues, the estimate at completion is BAC ÷ CPI = 86.0 ÷ 0.9216 = **$93.3m**, giving a variance at completion of 86.0 − 93.3 = **−$7.3m**.

The to-complete performance index needed to finish on budget is (BAC − EV) ÷ (BAC − AC) = 57.8 ÷ 55.4 = **1.04**. A team running at 0.92 is being asked to run at 1.04 for every remaining dollar, and the commentary has to say so.

The model then receives those nine figures, the agreed cause, a word limit and a house style, and is told to recompute nothing and to propose no cause. It returns a paragraph with a closing line naming every figure it used, which the reviewer ticks against the extract.

What it does not return is an explanation of why performance slipped, because that came from the project review and was supplied rather than generated.

## Where does the reported number meet the ledger?

This is where projects lose money, and it is the boundary a drafting tool cannot see. An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on a driving path.

IFRS 15 sets out a five-step model, described here in PCI's own words; [how the five-step model applies to a construction contract](https://projectcontrolsinstitute.org/ifrs-15-for-construction) is worked through in full separately. Identify the contract with the customer. Identify the distinct performance obligations within it.

Determine the transaction price, including variable consideration and the limit on how much of it may be recognised. Allocate that price across the obligations. Recognise revenue as each obligation is satisfied, over time where the criteria for that are met, otherwise at a point in time.

Step five is where the project report and the ledger meet. Under a cost-to-cost input method, progress is the ratio of costs incurred that genuinely depict progress to the total costs expected, and costs that do not depict progress — significant uninstalled materials, rectification of defective work — are excluded from the measure.

Use the figures above. Costs incurred are $30.6m and total expected costs are the $93.3m estimate at completion, so the measure is 30.6 ÷ 93.3 = **32.8%**.

On a transaction price of $110.0m, revenue recognised to date is 0.328 × 110.0 = **$36.1m**.

Now suppose the project report states physical progress of 40%, measured from work in place. Applying that percentage to the same price would give 0.40 × 110.0 = **$44.0m**, a difference of **$7.9m** on identical facts.

Neither percentage is wrong. They answer different questions, and a report that says "the project is 40% complete" without saying which 40% has produced a figure that is correct for the site meeting and misleading for the ledger. A language model drafting from that sentence will reproduce the ambiguity faithfully and confidently.

## Why the pack needs a cash line as well as a margin line

Recognised revenue is not money received, and contracting businesses fail on the second while reporting well on the first.

The cash conversion cycle measures how long cash is tied up: days inventory outstanding, plus days sales outstanding, minus days payables outstanding. For a contractor, the first term is unbilled work in progress.

Take 35 days of work in progress, 68 days to collect, and 45 days taken to pay suppliers. The cycle is 35 + 68 − 45 = **58 days** of working capital funded by the business.

At an annualised $110.0m, one day of receivables is 110.0 ÷ 365 = **$0.30m**. Taking ten days out of collection releases roughly **$3.0m** of cash without changing revenue, margin or a single line of the delivery plan.

That arithmetic belongs in the pack a model helps you draft, and it is the part most often missing when the pack is written and reviewed entirely by delivery people.

## What must never be generated?

Five things, each of them a figure or a claim that has to originate with a person.

**The numbers**, which are computed where the ledger, the cost tool or the schedule lives and then supplied. **The cause of a variance**, which is a finding from a review rather than an inference from a trend.

**Quotations**, because no model output should be attributed to somebody who did not say it. **Contract interpretation**, because what an entitlement clause means is a commercial question with money attached.

**Forward-looking commitments**, because a recovery date the delivery team has not agreed becomes a promise the moment it is printed.

## How does PCI examine this?

The PCI AI Project Finance Leader (PFL-AI) credential has 16 domains and 61 knowledge areas, and covers the point where project reporting meets financial reporting: revenue recognition, cut-off, contract assets and liabilities, provisions, and the cash position behind the margin.

The Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Can I let a model read the cost ledger and write the report itself?**
You can let it read, but not source. Extraction is a reasonable use with a verification step; taking the figures straight into narrative removes the point at which anyone confirms the extract matches the ledger at the cut-off. Extract, reconcile, then draft.

**How much time does this actually save?**
The saving is in drafting and reformatting, not in analysis or review. Teams that measure it usually find the first draft collapses and the review stays the same or grows slightly, because reviewers now check provenance as well as prose. Measure your own before and after rather than accepting a vendor figure.

**Will an auditor object to AI-assisted commentary?**
Not to the assistance itself, because the questions an auditor asks are about evidence rather than tools. The objection arrives when provenance is missing and a figure in a signed pack cannot be reproduced from its inputs. Keep the six-item trail and an AI-assisted paragraph is no harder to support than a typed one.

**Does the model need to know our accounting policy?**
It needs the outputs of the policy rather than the policy itself. Supply the recognised revenue, the measure used, the transaction price and the cut-off, then forbid recalculation. A model reasoning from a summary of your policy will produce something plausible, unaudited and impossible to trace back to a ledger entry.

**How do we stop the commentary becoming generic month to month?**
Supply the cause and the decision every month, and reject any draft containing neither. Generic commentary is almost always the product of a prompt that supplied figures and nothing else, leaving the model to fill the gap with management prose. If nobody can state the cause, that absence is the finding the report should carry.

---

*Internal links: placed in the body. Three on pciai.org — the AI in project controls pillar, where the draft-versus-compute split is stated; the LLM schedule review protocol, beside the schedule narrative row, because the network should be checked before its dates are narrated; and the AI policy template, at the audit trail, where provenance becomes a written rule. One cross-estate link, to the hub's IFRS 15 for construction page, at the sentence that describes the five-step model, since that is where a reader asks how progress is measured on a construction contract. The second hub link the note originally proposed, to month-end close for projects, was dropped to hold the one-link-per-domain cap; the cut-off is named in the body without it. Reciprocal: the hub's month-end close piece has the stronger claim to link here, from its reporting-pack step, and the Qatar course guide links here from the AI question in its FAQ.*
