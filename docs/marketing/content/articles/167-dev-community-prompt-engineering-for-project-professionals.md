---
platform:      DEV Community
type:          how-to
title:         Prompt engineering for project professionals, tested
meta:          Prompt engineering for project professionals: five parts of a usable prompt, four patterns with their failure modes, and a harness that scores them.
primary_kw:    prompt engineering for project professionals
secondary_kw:  governed AI, golden set evaluation, precision recall F1, variance commentary
pillar:        AI in project controls
credential:    suite
target_domain: pciai.org
canonical:     canonical -> /prompt-engineering-for-project-professionals (own site #067)
schema:        HowTo
word_count:    1629
hashtags:      #ai #python #testing #productivity
ab_id:         AB-00150
---

# Prompt engineering for project professionals, tested

Prompt engineering for project professionals means writing an instruction carrying five things: the role and standard to apply, the data itself, the task with its boundary, the output format, and the check that proves the answer right. A prompt missing the fifth is not a control. It is a guess with good grammar.

What follows is the five parts, four patterns with their failure modes, and a small harness that scores a prompt before it goes anywhere near a report.

## What is prompt engineering for project professionals?

Prompt engineering for project professionals is specifying a task precisely enough that a competent stranger could carry out the same task and produce a comparable answer.

That test is the useful one. If you would have to explain three more things to a new starter before they could attempt the job, the prompt is short by three things.

The audience here is people producing cost, schedule and reporting outputs that other people rely on financially. That changes the engineering problem: the output is not judged on fluency, it is judged on whether a reviewer can mark it.

## What does a usable project prompt contain?

Five parts. Drop any of them and the failure is predictable rather than random.

| Part | What it does | What goes wrong without it |
|---|---|---|
| Role and standard | Names the discipline and the rule set to apply | Generic management prose, with an invented convention |
| Data | Supplies the actual figures, export or text | The gap is filled from training data as plausible fiction |
| Task and boundary | States the one job and what is out of scope | Scope creep: you asked for a check and received a rewrite |
| Output format | Fixes the shape: table, columns, field names, units | Output that has to be retyped, which erases the time saved |
| Verification hook | Requires inputs, formulas and stated uncertainty | You cannot tell a correct answer from a confident one |

The verification hook is the part most people skip. It costs one sentence and converts an opaque answer into something a reviewer can mark in a minute.

## Which patterns are worth keeping?

Four cover almost all project work. Knowing each pattern's failure mode is most of the skill.

| Pattern | Use it for | Characteristic failure | Control |
|---|---|---|---|
| Extraction | Pulling dates, values, obligations and quantities from contracts, RFIs and diaries | Quiet omission: eight of twelve clauses returned, silently | Require a count, verify it against the source |
| Classification | Sorting transactions, risks or correspondence into named categories | Category drift: a class you never defined | Closed list, with "unclassified" as an allowed answer |
| Calculation check | Confirming a stated figure follows from stated inputs | Arithmetic that looks right and is not | Compute in the tool; ask the model to reconcile, not calculate |
| Drafting | Turning your numbers into house-format commentary | Confident causal claims nobody made | Supply the cause; forbid the model from proposing one |

Adversarial review is a variant of drafting worth naming separately. Hand the model a finished paragraph and ask what a sceptical auditor challenges first, which is a language task rather than a numerical one.

## Build the prompt as a template, not a message

Anything reused belongs in version control with its inputs typed. This is the shape that survives a team.

```python
from string import Template

VARIANCE_COMMENTARY = Template("""
Role: project controls reporter drafting the cost variance paragraph for a
monthly board pack, in plain British English.

Data (use only these figures, all $currency m):
  BAC $bac, PV $pv, EV $ev, AC $ac, CPI $cpi, SPI $spi, EAC $eac

Agreed cause (from the project review): $cause

Task: write $lo to $hi words stating the variance, the index, the forecast
and the agreed cause.

Boundary: do not recompute any figure. Do not propose any cause not given
above. Do not state a recovery date.

Output: one paragraph, then a final line listing every figure you used.
""")
```

Every clause earns its place. The figures are supplied so nothing is inferred, the cause is supplied so nothing is invented, and the final line is the verification hook.

The `Boundary` block is the part that stops a drafting tool from becoming an analysis tool nobody authorised.

## The arithmetic goes in code, not in the prompt

Compute the indices where the control accounts live, then pass the results in. Budget at completion (BAC) £12.0m, planned value (PV) £4.20m, earned value (EV) £3.78m, actual cost (AC) £4.20m at the data date.

Cost variance is EV − AC = 3.78 − 4.20 = **−£0.42m**. Schedule variance is EV − PV = 3.78 − 4.20 = **−£0.42m**. Both indices land at 3.78 ÷ 4.20 = **0.90**.

The independent estimate at completion, on the assumption that cost performance continues, is BAC ÷ CPI = 12.0 ÷ 0.90 = **£13.33m**, a variance at completion of −£1.33m against budget.

```python
def evm(bac, pv, ev, ac):
    cpi, spi = ev / ac, ev / pv
    return {"cv": ev - ac, "sv": ev - pv, "cpi": cpi, "spi": spi,
            "eac": bac / cpi, "vac": bac - bac / cpi}
```

Six lines, deterministic, testable, and the audit trail already exists in your repository. Moving that into a prompt removes the trail and adds nothing.

There is one honest exception. Asking a model to reconcile a figure you already hold — "given EV 3.78 and AC 4.20, is a stated CPI of 0.94 consistent?" — tests a claim rather than sourcing one, and that is a genuine check.

## How do you test a prompt before you trust it?

Build a golden set: thirty to a hundred items from closed work where you already know the right answer. Run the prompt over the set and count.

Worked example. A prompt pulls payment-terms clauses from 200 contract extracts.

The set contains 80 genuine payment-terms clauses, and the prompt returns 75 items, of which 60 are correct.

- Precision = 60 ÷ 75 = **0.80**
- Recall = 60 ÷ 80 = **0.75**
- F1 = 2 × (0.80 × 0.75) ÷ (0.80 + 0.75) = 1.20 ÷ 1.55 = **0.77**

Fifteen returned clauses were wrong and twenty real ones were missed. Whether 0.77 is acceptable depends entirely on what a missed payment clause costs on your contracts, which is a commercial question rather than a technical one.

```python
def score(returned, truth):
    tp = len(returned & truth)
    precision = tp / len(returned) if returned else 0.0
    recall = tp / len(truth) if truth else 0.0
    f1 = 2 * precision * recall / (precision + recall) if tp else 0.0
    return round(precision, 2), round(recall, 2), round(f1, 2)
```

Change one line of the prompt and re-run the same set. A prompt improvement you cannot measure on a fixed set is a preference, not an improvement.

Re-run the whole set when the model version changes. Providers update models without changing the name, and a prompt tuned to one version is not guaranteed on the next. Pinning the version in your client and recording it beside the score is the cheapest defence there is.

## What must never go into a prompt

Four categories, worth writing into a team policy rather than leaving to judgement.

**Client-confidential commercial data** where the contract restricts disclosure to third parties, which a hosted model usually is unless your agreement says otherwise.

**Personal data** — names, individual rates, health or disciplinary information — unless the processing has been assessed and the lawful basis recorded.

**Credentials and access tokens**, ever, including inside a pasted configuration file, stack trace or log.

**Anything you could not show to the counterparty**, as the working test for grey cases. Draft claim positions and internal reserve figures both fail it.

## Where prompting meets the money

A variance paragraph is read by people who decide whether a milestone payment is still likely, so the words carry a financial consequence the model cannot see.

An engineer is examined on float and progress measurement, and almost never on cut-off or a contract asset. An accountant is examined on when revenue may be recognised, and almost never on a driving path. A prompt that lets a model infer a cause is a prompt that lets it write across that boundary unsupervised.

## How PCI examines prompting

Prompting sits inside the governed AI portion of the Body of Knowledge, proportioned 40/40/20 across finance and reporting, project management, and governed AI across the three credentials: the PCI AI Project Controls Leader (PCL-AI) with 13 domains and 61 knowledge areas, the PCI AI Project Finance Leader (PFL-AI) with 16 domains and 61 knowledge areas, and the PCI Project Management Leader – AI (PML-AI) with 16 domains and 63 knowledge areas.

The examinable skill is not clever wording. It is whether a candidate can specify a task precisely, verify the output and say who owns the result.

PCI is an independent certifying body. Nothing here is legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Do I need a special prompt syntax?**
No. Plain, precise British English outperforms formatting tricks on current models, and the tricks that circulate tend to be tuned to a version since replaced. The gains come from supplying the data, naming the boundary and demanding a verification line, none of which is syntax and all of which survive a model update.

**How long should a prompt be?**
Long enough to remove ambiguity, which for project work is usually 80 to 250 words plus the data. Prompts fail from missing context far more often than from length, though one prompt carrying three unrelated tasks will do all three poorly. Split it and score the parts separately.

**Should the model return JSON or prose?**
JSON for extraction, classification and any check you intend to validate in code, because a schema failure is then a caught error rather than a silent one. Prose only for the final drafting step, and even then require a trailing line listing the inputs used so a reviewer can tick it against the source.

**How do we share prompts across a team?**
Version them like any other control document, with the golden-set score recorded beside each version. A shared prompt with a measured score can be improved by anyone; a personal prompt living in one chat history is a single point of failure the week its author leaves.

**What is the most common mistake?**
Asking for an answer instead of a checkable answer. Adding "list every figure and formula you used" costs nothing and turns an opaque output into something a reviewer can mark, which is the whole difference between an assistant and a control.

---

*First published on pciai.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the full text including the harness lives here.*

*Internal links: this how-to should link to [AI in project controls](https://pciai.org/ai-in-project-controls) with the anchor "the governed-AI controls pillar it supports", to [using large language models to review schedules](https://pciai.org/llm-schedule-review) with the anchor "the same discipline applied to a schedule export", and to [earned value management](https://projectcontrolsinstitute.org/earned-value-management) with the anchor "where the CPI and SPI in the worked prompt come from".*
