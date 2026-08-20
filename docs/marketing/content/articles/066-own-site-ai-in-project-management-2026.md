---
platform:      Own site — pciai.org
type:          pillar
title:         AI in project management in 2026: the state of play
meta:          What AI in project management in 2026 actually does, where it fails, and how to test any tool on your own data before its numbers reach a board pack.
primary_kw:    AI in project management in 2026
secondary_kw:  governed AI, estimate at completion, precision and recall, project reporting automation
pillar:        AI in project controls
credential:    PML-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    2481
hashtags:      n/a (own site)
ab_id:         AB-00099
---

# AI in project management in 2026: the state of play

AI in project management in 2026 is reliable at reading, drafting and checking, useful under supervision at forecasting, and no use at all at accountability. It compresses the assembly work that used to fill the first week of every month. It still cannot choose which forecast is true, and it cannot stand behind one.

That division has held for three years while the tooling around it changed completely. The rest of this page is about where the line sits and how to prove a tool falls on the correct side of it.

This page carries no adoption statistics. The ones in circulation are vendor surveys with unstated samples, and a number you cannot source is worth less than no number at all.

## What does AI in project management in 2026 actually do?

A delivery function does four things. It records what happened, forecasts what will happen, explains the gap, and puts a decision in front of somebody. Models are strong on the first and third, supervised on the second, and absent from the fourth.

The distinction is commercial, not philosophical. A tool sold as an AI forecaster that in practice does very fast data assembly is still worth buying. It should not be priced or governed as though it produced the forecast.

| Delivery task | What a model does well in 2026 | What a person still owns |
|---|---|---|
| Status collection | Reads diaries, emails, minutes and progress claims; assembles one dated set and names the gaps | Whether the cut-off is clean and the claim is supportable |
| Schedule structure | Finds open ends, dangling logic, hard constraints, long lags, out-of-sequence progress | Whether the sequence can be built with the crews available |
| Risk intake | Drafts cause-event-effect rows from correspondence and flags duplicates | Whether the row is a risk, an issue or a wish |
| Forecasting | Computes every method instantly and tests each against your own history | Choosing the method, which is a claim about cause |
| Variance commentary | Writes the paragraph in the house format from the numbers you supply | Owning the number in front of a client, a board or an auditor |
| Change and claims | Extracts dates, values and obligations from variation orders and RFIs | The commercial meaning of a disputed clause |
| Meeting output | Turns a two-hour call into actions with owners and dates | Deciding which action gets funded and which gets dropped |

Read that as a division of labour rather than a ranking. The left column is worth money precisely because it buys time for the right column.

## What has actually changed since the first wave of tools?

Three shifts are visible in practice. None of them is the shift the marketing describes.

**The reporting cycle compressed, and the released time went somewhere.** Collecting, reconciling and drafting used to occupy the opening days of a month. That work now takes hours, and whether the saved days go into analysis or simply disappear is a management decision made before the tool is bought.

**The junior task mix moved fastest.** The apprenticeship in this discipline was built on doing the assembly by hand, which is the exact part that automated first. Building judgement without that apprenticeship is an unsolved problem, and pretending otherwise does new entrants no favours.

**Assurance moved upstream.** When a pack can be produced in an afternoon, checking the finished pack stops being a control. The control has to sit on the inputs, the method and the review step, which is a different skill from marking up a spreadsheet.

## How do you tell whether an AI tool is good enough?

You measure it on your own data, against outcomes you already know. Three numbers carry most of the argument: precision, recall and F1.

Precision is the share of the tool's alerts that turned out to be real. Recall is the share of real events it caught. F1 is the harmonic mean of the two, used when you want one figure that penalises a lopsided model.

Here is a worked example, not a case study. A delivery assistant reads 1,800 site diary entries a month and flags possible delay events. It raises 240 flags. Review confirms 96 as genuine. A retrospective check on the unflagged entries finds 24 genuine events it missed.

- Precision = 96 ÷ 240 = **0.40**
- Recall = 96 ÷ (96 + 24) = 96 ÷ 120 = **0.80**
- F1 = 2 × (0.40 × 0.80) ÷ (0.40 + 0.80) = 0.64 ÷ 1.20 = **0.53**

Now price it. At four minutes of review per flag, 240 flags is 16 hours a month of a delivery manager's time, spent to surface 96 real events while 24 still slipped past.

Raise the confidence threshold and the trade moves. Suppose the tool now raises 120 flags, 72 of which are genuine, leaving 48 real events missed.

Precision rises to 72 ÷ 120 = **0.60**. Recall falls to 72 ÷ 120 = **0.60**. F1 improves to 2 × 0.36 ÷ 1.20 = **0.60**, and review load halves to 8 hours.

The second setting scores better and lets twice as many events through unnoticed. Which one you want depends on what a missed delay event costs you, and that is a commercial judgement rather than a modelling one.

A vendor who will not run this test on data you control has told you something useful. Ask for precision and recall on a golden set you own, with review time priced in.

## Can a model produce a forecast you can defend?

It can produce every candidate forecast in milliseconds. Choosing between them is the part that carries the accountability, and the arithmetic shows why.

Take a programme with a budget at completion (BAC) of £48m. At the data date, planned value (PV) is £20m, earned value (EV) is £17m and actual cost (AC) is £19m.

Cost variance is EV − AC = 17 − 19 = **−£2m**. Schedule variance is EV − PV = 17 − 20 = **−£3m**. The indices are CPI = 17 ÷ 19 = **0.89** and SPI = 17 ÷ 20 = **0.85**.

Four estimate at completion methods, four answers, identical inputs, and [when each of the four EAC formulas applies](https://projectcontrolsinstitute.org/four-eac-formulas) is the judgement the table cannot make for you.

| EAC method | Formula | What it assumes | Answer |
|---|---|---|---|
| Remaining work at budget | AC + (BAC − EV) | The overrun was a one-off that will not repeat | £50.0m |
| Performance continues | BAC ÷ CPI | Cost performance to date is the best predictor available | £53.6m |
| Cost and schedule both bite | AC + (BAC − EV) ÷ (CPI × SPI) | Pulling the schedule back will itself cost money | £59.8m |
| Bottom-up re-estimate | AC + a fresh estimate to complete | History says nothing useful about the work that remains | £53.5m |

Worked through: method one is 19 + (48 − 17) = 19 + 31 = £50.0m. Method two is 48 ÷ 0.8947 = £53.6m.

Method three uses CPI × SPI = 0.8947 × 0.85 = 0.7605, so 19 + (31 ÷ 0.7605) = 19 + 40.8 = £59.8m. Method four takes a re-priced remaining scope of £34.5m, giving 19 + 34.5 = £53.5m.

The spread is **£9.8m on the same four inputs**. A model can compute all four and tell you which has been closest on your last thirty jobs. It cannot tell you whether the cause of this overrun has passed.

One further figure frames the recovery conversation. The to-complete performance index needed to land on budget is (BAC − EV) ÷ (BAC − AC) = 31 ÷ 29 = **1.07**.

A team running at 0.89 is being asked to run at 1.07 for every remaining pound. Any recovery plan that does not explain that jump is a wish with a Gantt chart attached.

## What can a model tell you about the schedule?

It reads structure well and reality badly. Structural defects are pattern matching, and pattern matching is what these systems do.

A model will find activities with no predecessor or no successor, hard date constraints that override logic, negative lags, and progress recorded on activities whose predecessors are incomplete. Those findings are cheap and worth having every week.

What it cannot judge is whether the sequence reflects how the work will be built. Two activities can be logically sound and physically impossible in the same fortnight, because the same crane serves both.

Float is the clearest example of the boundary. Total float is the time an activity can slip before the completion date moves; free float is the time it can slip before the next activity is pushed.

A model can quote both from an export and can explain the difference clearly. It has no view on whether the contract lets you consume that float without a claim.

## Where do AI-assisted projects still lose money?

Not inside delivery, and not inside finance. The losses sit in the handover between them, and a model trained on one side does not know the other side exists.

An engineer is examined on float, earning rules and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised and what a provision must satisfy, and almost never on a critical path. The same percentage crosses that boundary every month, unchanged, and nobody in the chain has been examined on the crossing.

Here is the crossing in one step. Physical progress read from site is a statement about work in place. Revenue measured by a cost-to-cost input method is a ratio of costs incurred to total expected costs.

On a job where the expensive work is front-loaded, those two percentages diverge sharply, and using the first to drive the second overstates margin. A model that reports "the project is 40% complete" without saying which 40% has produced a figure that is right for the site meeting and wrong for the ledger.

That is a scoping defect rather than a modelling defect, and it is the most common one in the market.

## What does governed AI require in practice?

Governed AI is the principle PCI certifies against: AI proposes, the professional disposes. A tool may generate the schedule, the forecast or the risk analysis. A competent person validates it, understands how it was produced, and owns the decision that follows. Worked out across cost, schedule, risk and reporting, that principle becomes [governed AI across the controls discipline](https://pciai.org/ai-in-project-controls).

That principle becomes five requirements, none of them difficult.

**Provenance.** Every AI-assisted figure can be traced to its inputs and to the model version that produced it. A number you cannot reproduce is a number you cannot defend.

**A measured baseline.** Precision and recall on your own golden set, refreshed as the contract mix changes. Accuracy demonstrated on last year's data is an assumption, not a control.

**A priced review step.** Somebody checks the output, the check takes hours, and those hours are in the budget. Unbudgeted review is the same as no review.

**Named accountability.** A person, not a function, owns each AI-assisted output. Auditors ask who, and "the system" is not an answer.

**A failure route.** What happens when the tool is wrong, who finds out, and how quickly. Confident wrongness is the characteristic failure mode of a language model and it does not announce itself.

## How does PCI examine this?

PCI certifies three AI-era credentials, each with its own Body of Knowledge and its own examination. Telling those apart from the rest of the market is a separate exercise, because [four different things are sold as an AI project management certification](https://pciai.org/ai-project-management-certification) and only one of them is independently assessed.

| Credential | Full name | Shape | Centre of gravity |
|---|---|---|---|
| **PCL-AI** | PCI AI Project Controls Leader | 13 domains, 61 knowledge areas | The integrated controls discipline, cost through schedule to reported number |
| **PFL-AI** | PCI AI Project Finance Leader | 16 domains, 61 knowledge areas | Project finance, funding and financial reporting |
| **PML-AI** | PCI Project Management Leader – AI | 16 domains, 63 knowledge areas | Delivery leadership with genuine controls literacy |

The Body of Knowledge is proportioned 40/40/20 across finance and reporting, project management, and governed AI. The AI portion covers concepts, data, prompting, tooling, applied workflows, governance and capability, including the golden-set evaluation arithmetic worked above.

Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements, and 92 sector case studies across the three volumes (26 + 33 + 33). The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite.

PCI is an independent certifying body. Nothing on this page is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Will AI replace project managers?**
No, but it changes what the job consists of. The collection, reconciliation and drafting compress hard, while choosing methods, defending forecasts and carrying accountability do not compress at all. The individual risk is not redundancy. It is spending a decade becoming excellent at the part that automated, and [which parts of the project manager's week survive](https://pciai.org/will-ai-replace-project-managers) is worked through task by task.

**Can I put an AI-generated estimate at completion in a board pack?**
Only with the method named and the reasoning stated. The model can produce all four methods and rank them against your history, which is genuinely useful. The choice between them is a claim about what caused the variance and whether that cause persists, and that claim is yours.

**What accuracy should I demand from a delivery AI tool?**
There is no universal threshold. Ask for precision and recall measured on your own data, then price the review hours each threshold implies against the cost of the events each threshold misses. A tool at 0.60 precision that saves eight hours a month can beat one at 0.90 that flags almost nothing.

**Is our project data good enough for this?**
Usually the cost data is adequate, the schedule data is patchy, and the join between them is the weak point. Before buying anything, check whether your control accounts reconcile to your work breakdown structure and whether progress is measured by a rule somebody can state out loud.

**Does using AI create an audit problem?**
Only where provenance is missing. An AI-assisted figure is no different from a spreadsheet-derived one: the auditor asks what the inputs were, what method applied, who reviewed it and who owns it. Keep the model version and the input set with the output and every one of those is answerable.

**Where should a project manager start?**
Start with one task that is high volume, low judgement and easy to check, such as drafting variance commentary from figures you computed yourself. Run it for a quarter, record how long the review actually takes, then extend to a second task only if the first still saves time under review. Starting with forecasting inverts the risk, because the hardest output to check is the first thing you would be trusting.

---

*Internal links: now placed in the body. Same-domain: "governed AI across the controls discipline" sits where the AI-proposes-professional-disposes principle is stated and a reader asks how far it reaches; "four different things are sold as an AI project management certification" opens the credentials section, which raises how PCI's differ from the rest of the market; "which parts of the project manager's week survive" answers the first FAQ, which asks the question directly. One cross-estate link only, to the hub: "when each of the four EAC formulas applies" beside the four-method table, where the £9.8m spread raises the choice. Reciprocal: the AI project management certification comparison and the project manager piece should both point back here for the precision-and-recall worked example.*
