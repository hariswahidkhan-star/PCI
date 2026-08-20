---
platform:      Own site — pciai.org
type:          how-to
title:         Prompt engineering for project professionals explained
meta:          Prompt engineering for project professionals: the five parts of a usable prompt, four patterns that work, and how to test one on a golden set.
primary_kw:    prompt engineering for project professionals
secondary_kw:  governed AI, golden set evaluation, variance commentary, AI in project controls
pillar:        AI in project controls
credential:    suite
target_domain: pciai.org
canonical:     original
schema:        HowTo
word_count:    1658
hashtags:      n/a (own site)
ab_id:         AB-00150
---

# Prompt engineering for project professionals explained

Prompt engineering for project professionals means writing an instruction that carries five things: the role and standard to apply, the data itself, the task with its boundary, the output format, and the check that proves the answer right. A prompt missing the fifth is not a control, it is a guess with good grammar.

The rest of this page is the five parts, four patterns worth keeping, and how to test a prompt before it touches a report.

## What is prompt engineering for project professionals?

Prompt engineering for project professionals is the practice of specifying a task for a language model precisely enough that a competent stranger could carry out the same task and produce a comparable answer.

That test is the useful one. If you would have to explain three more things to a new starter before they could attempt the job, the prompt is short by three things.

## What does a usable project prompt contain?

Five parts. Drop any of them and the failure is predictable rather than random.

| Part | What it does | What goes wrong without it |
|---|---|---|
| Role and standard | Names the discipline and the rule set to apply | The model answers in generic management prose and invents a convention |
| Data | Supplies the actual figures, export or text in the prompt | The model fills the gap from training data and produces plausible fiction |
| Task and boundary | States the one job and what is out of scope | Scope creep: you asked for a check and received a rewrite |
| Output format | Fixes the shape: table, columns, field names, units | Unusable output that has to be retyped, which erases the time saved |
| Verification hook | Requires the model to show inputs, formulas and its own uncertainty | You cannot tell a correct answer from a confident one |

The verification hook is the part most people skip. It costs one sentence and it converts an opaque answer into something a reviewer can mark.

## Which prompt patterns are worth keeping?

Four cover almost all project work. Each has a distinct failure mode, and knowing the failure mode is most of the skill.

| Pattern | Use it for | Characteristic failure | Control |
|---|---|---|---|
| Extraction | Pulling dates, values, obligations and quantities out of contracts, RFIs and diaries | Quiet omission: it returns eight of the twelve clauses and says nothing | Ask for a count, then verify the count against the source |
| Classification | Sorting transactions, risks or correspondence into named categories | Category drift: it invents a class you did not define | Give a closed list and require "unclassified" as an allowed answer |
| Calculation check | Confirming that a stated figure follows from stated inputs | Arithmetic that looks right and is not | Compute in the tool; ask the model to reconcile, not to calculate |
| Drafting | Turning your numbers into house-format commentary | Confident causal claims nobody made | Supply the cause; forbid the model from proposing one |

Adversarial review is a variant of drafting worth naming separately. Give the model the finished paragraph and ask what a sceptical auditor would challenge first. It is good at that, because it is a language task rather than a numerical one. Pointed at a programme file instead of a paragraph, the same four patterns become [a protocol for reviewing a schedule export with a language model](https://pciai.org/llm-schedule-review).

## A worked prompt: variance commentary that survives review

Start with numbers you computed yourself. Budget at completion (BAC) £12.0m, planned value (PV) £4.20m, earned value (EV) £3.78m, actual cost (AC) £4.20m at the data date.

Cost variance is EV − AC = 3.78 − 4.20 = **−£0.42m**. Schedule variance is EV − PV = 3.78 − 4.20 = **−£0.42m**. Both indices land at 3.78 ÷ 4.20 = **0.90**.

The independent estimate at completion on the assumption that performance continues is BAC ÷ CPI = 12.0 ÷ 0.90 = **£13.33m**, a variance at completion of −£1.33m against budget. Anyone unsure where those indices come from will find [the earned value method behind these figures](https://projectcontrolsinstitute.org/earned-value-management) set out in full.

The prompt then reads, in substance:

> You are a project controls reporter drafting the cost variance paragraph for a monthly board pack. Use only the figures given: BAC 12.00, PV 4.20, EV 3.78, AC 4.20, CPI 0.90, SPI 0.90, EAC 13.33, all £m. The cause agreed at the review is rework on the eastern façade following a design change in month four. Write 90 to 120 words in plain British English. State the variance, the index, the forecast and the agreed cause. Do not propose any cause not given. Do not recompute the figures. End with a line listing every figure you used.

Each clause earns its place. The figures are supplied so nothing is inferred. The cause is supplied so nothing is invented. The final line is the verification hook, and it is what a reviewer checks first.

What comes back is a paragraph that took ninety seconds instead of forty minutes, with an input list you can tick against the source. What does not come back is a claim about why the façade needed rework, because you forbade it. Scaling that from one paragraph to a whole monthly pack is [drafting project reporting without losing the audit trail](https://pciai.org/generative-ai-project-reporting).

## Why should the model not do the arithmetic?

Because the arithmetic already lives somewhere with an audit trail, and moving it into a prompt removes that trail without adding anything.

Compute CPI, SPI and EAC in the cost tool or the spreadsheet that already holds the control accounts. Then use the model for what it is good at: checking that the stated figure follows from the stated inputs, and writing the words around it.

There is one honest exception. Asking a model to reconcile a number you already have — "given EV 3.78 and AC 4.20, is a stated CPI of 0.94 consistent?" — is a genuine check, because you are testing a claim rather than sourcing one.

## How do you test a prompt before you trust it?

Build a golden set: thirty to a hundred items where you already know the right answer, drawn from work that has closed. Run the prompt over the set and count.

Here is a worked example. A prompt is asked to pull payment-terms clauses from 200 contract extracts. The set contains 80 genuine payment-terms clauses. The prompt returns 75 items, of which 60 are correct.

- Precision = 60 ÷ 75 = **0.80**
- Recall = 60 ÷ 80 = **0.75**
- F1 = 2 × (0.80 × 0.75) ÷ (0.80 + 0.75) = 1.20 ÷ 1.55 = **0.77**

That tells you fifteen of the returned clauses were wrong and twenty real ones were missed. Whether 0.77 is acceptable depends entirely on what a missed payment clause costs on your contracts.

Change one line of the prompt and re-run the same set. A prompt improvement you cannot measure on a fixed set is a preference, not an improvement.

Re-run the set when the model version changes. Providers update models without changing the name, and a prompt tuned to one version is not guaranteed on the next.

## What must never go into a prompt?

Four categories, and they are worth writing into a team policy rather than leaving to judgement.

**Client-confidential commercial data** where the contract restricts disclosure to third parties, which a hosted model usually is unless your agreement says otherwise.

**Personal data** — names, rates tied to individuals, health or disciplinary information — unless the processing has been assessed and the lawful basis recorded.

**Credentials and access tokens**, ever, including inside a pasted configuration file or log.

**Anything you could not show to the counterparty**, as a working test for the grey cases. Draft claim positions and internal reserve figures fail it.

## How does PCI examine prompting?

Prompting sits inside the governed AI portion of the Body of Knowledge, which is proportioned 40/40/20 across finance and reporting, project management, and governed AI across the three credentials. It is one skill inside [the wider governed-AI picture in project controls](https://pciai.org/ai-in-project-controls), not a discipline of its own.

The AI domain covers concepts, data, prompting, tooling, applied workflows, governance and capability. The examinable skill is not clever wording. It is whether a candidate can specify a task precisely, verify the output and say who owns the result.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Do I need to learn a special prompt syntax?**
No. Plain, precise British English outperforms formatting tricks on current models, and the tricks that circulate tend to be tuned to a model version that has since been replaced. The gains come from supplying the data, naming the boundary and demanding a verification line, none of which is syntax and all of which survive a model update.

**How long should a prompt be?**
Long enough to remove ambiguity, which for project work is usually 80 to 250 words plus the data. Prompts fail from missing context far more often than from length, though a prompt carrying three unrelated tasks will do all three poorly.

**Should prompts be shared across a team?**
Yes, and versioned like any other control document, with the golden-set score recorded beside each version. A shared prompt with a measured score is reusable and can be improved by anyone; a personal prompt living in one person's chat history is a single point of failure the week they leave.

**Can I ask a model to check my earned value figures?**
You can ask it to reconcile figures you supply, which tests consistency. Do not ask it to source the figures. Compute them where the control accounts live, then use the model to confirm that the stated index follows from the stated inputs.

**What is the single most common prompting mistake?**
Asking for an answer instead of asking for a checkable answer. Adding "list every figure and formula you used" to the end of a prompt costs nothing and turns an opaque output into something a reviewer can mark in a minute.

---

*Internal links: now placed in the body. Same-domain: "a protocol for reviewing a schedule export with a language model" follows the pattern table, where a reader asks what the patterns look like on a programme; "drafting project reporting without losing the audit trail" follows the worked prompt, which raises what happens when one paragraph becomes a pack; "the wider governed-AI picture in project controls" sits in the examination section, which raises where prompting fits in the syllabus. One cross-estate link only, to the hub: "the earned value method behind these figures" beside the CPI, SPI and EAC arithmetic the prompt is built on. Reciprocal: the LLM schedule review how-to should point back here for the five parts of a usable prompt.*
