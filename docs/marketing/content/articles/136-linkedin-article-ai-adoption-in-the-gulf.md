---
platform:      LinkedIn Article
type:          data-study
title:         AI adoption in the Gulf: what the numbers can prove
meta:          AI adoption in the Gulf is measured by self-report, not capability. What a survey headline cannot tell you, and the precision test that can.
primary_kw:    AI adoption in the Gulf
secondary_kw:  AI in project controls, precision and recall, AI skills for project professionals, Gulf capital projects
pillar:        AI in project controls
credential:    suite
target_domain: pciglobal.ai
canonical:     original
schema:        Article + FAQPage
word_count:    1632
hashtags:      #ProjectControls #AIGovernance #CostEngineering #ProjectManagement
ab_id:         AB-00302
---

# AI adoption in the Gulf: what the numbers can prove

Adoption figures measure whether people say they use AI. They do not measure whether anyone can check the output. AI adoption in the Gulf is high on the first measure and almost entirely unmeasured on the second, which is the one that decides whether a machine-produced cost report is safe to sign.

Written for LinkedIn as an original. It sits under the Institute's AI in project controls pillar.

## What does AI adoption in the Gulf actually measure?

Survey headlines of the "two-thirds of professionals now use AI at work" kind circulate constantly. PCI does not publish figures of that shape, because we cannot show you the sample frame, the definition of "use", or the year they were collected in.

The definition is the bigger problem. Someone who pasted a paragraph into a chatbot once last month and someone whose monthly cost report is coded by a trained classifier both answer yes to the same question.

Self-reported use is a measure of exposure. It says nothing about whether an output was verified before it reached a client, a lender or an auditor.

## What can be verified without running a survey?

Three classes of evidence are observable rather than reported, and all three are stronger than a percentage.

Public policy is the first. The Gulf states have published national artificial-intelligence strategies and stood up dedicated bodies to run them, which is a matter of public record. That tells you what governments intend to buy. It tells you nothing about what a planning team can do on a Tuesday.

Job advertisements are the second, and for a workforce question they are better data than any survey. What a Gulf employer names in a job advertisement is a claim it has to live with when the person arrives.

Deployment records are the third. Whether a model sits inside a reporting workflow is a fact recorded in an organisation's own procurement and process documentation. Nobody has to be asked.

## The four levels, and where most organisations really sit

| Level | What it means | How you would evidence it | What it proves about capability |
|---|---|---|---|
| 0 — Exposure | Staff have access to a general model | Licence count | Nothing beyond availability |
| 1 — Assisted drafting | Text, summaries and commentary drafted by a model, edited by a person | Version history, editing time | Writing speed, not judgement |
| 2 — Workflow integration | A model classifies, extracts or forecasts inside a reporting process | Process map with a named sign-off point | That somebody owns the output |
| 3 — Governed measurement | Output is sampled against a known-correct set and scored, with a stated threshold | Precision, recall and F1 by class, reviewed every period | Capability, and the right to rely on it |

Most reported adoption is level 0 and level 1. A headline percentage cannot distinguish between them, which is why it moves so easily and means so little.

Level 3 is rare everywhere, not only in the Gulf. It is also the only level at which the word "adoption" carries any assurance value.

## How do you measure whether an AI output is good enough?

You score it against a set of answers you already know are correct, and you use three numbers that come from classification, not from marketing.

**Precision** is true positives divided by all positives claimed. Of the lines the model said belonged in an account, how many actually did.

**Recall** is true positives divided by all the lines that genuinely belonged there. Of the lines it should have found, how many it found.

**F1** is the harmonic mean of the two: F1 = 2 × (precision × recall) ÷ (precision + recall). It punishes a model that is strong on one measure and weak on the other, which a simple average does not.

### A worked month on invoice coding

A contractor runs 1,000 invoice lines through a model that assigns cost accounts. Take one account, 4200 Mechanical, and check it by hand.

The model assigns 4200 to **200 lines**. Of those, **150 are correct** and **50 do not belong**. The true population of 4200 lines that month is **180**, so **30 were missed** and sit in other accounts.

**Precision** = 150 ÷ 200 = **0.75**.
**Recall** = 150 ÷ 180 = **0.83**.
**F1** = 2 × (0.75 × 0.83) ÷ (0.75 + 0.83) = 1.25 ÷ 1.58 = **0.79**.

Now the part that matters at month-end. The account reports 200 lines against a true 180, a **net overstatement of 20 lines, about 11%**. The gross error is 50 wrong lines in plus 30 right lines out, which is **80 lines, about 44%** of the account.

Netting hid three-quarters of the error. Worse, the 30 missing lines did not vanish: they landed in other accounts, so two forecasts are now wrong instead of one, and the second one looks clean.

### Which of the two you should optimise depends on the cost

Precision matters more when a false positive is expensive to unwind, such as a commitment posted against the wrong account and carried into an approved forecast.

Recall matters more when a miss is the expensive event, such as an unrecorded commitment or a risk trigger that nobody saw. You choose, you write the threshold down, and you review it every period.

A team that cannot state its own precision and recall is not doing AI-assisted project controls. It is doing unverified project controls with a faster keyboard.

## What does a governed standard look like in practice?

It needs a known-correct sample drawn every reporting period, a threshold agreed before the period starts, and a named person who signs the output regardless of what produced it.

That verification skill is why governed AI carries 20% of the Body of Knowledge's proportions across PCI's credentials, alongside 40% finance and reporting and 40% project management. The PCI AI Project Controls Leader (PCL-AI) examines it across 13 domains and 61 knowledge areas.

The Institute applies the same standard to itself where it can. PCI's machine calculation suite runs 15,613 checks, all passing, and it covers the PCI AI Project Finance Leader (PFL-AI) and the PCI Project Management Leader – AI (PML-AI) only; PCL-AI has no equivalent suite. A number without its scope attached is not a number.

Employers hiring into [project controls training in the UAE](https://pciglobal.ai/project-controls-training-uae) and the wider Gulf market are increasingly asking for exactly this: not tool familiarity, but the ability to test what the tool produced.

## What should a Gulf employer measure instead of adoption?

Count processes, not people. How many reporting processes have a machine step, and how many of those have a recorded sample score for the last period.

Then count owners. Every machine output that reaches a client, a lender or an auditor should have one name against it, and that person should be able to state the error rate without looking it up.

Then count skills. How many of your team could compute precision and recall on their own workflow this week. In most functions the honest answer is a small number, and it is a better baseline than any survey response.

## Frequently asked questions

**Is there a reliable figure for AI use among Gulf project professionals?**
Not one that PCI is willing to publish. The figures in circulation come from self-report surveys with different definitions of "use", different sample frames and different years, and they are not comparable with each other. Treat any single percentage as a headline rather than a measurement, and ask what the respondent had to do to count as a user.

**Why is precision more useful than accuracy?**
Accuracy counts every correct decision, including all the lines a model correctly left alone. When one class is rare, a model that never assigns it can still be 95% accurate and completely useless. Precision and recall look only at the class you care about, which is why classification work reports them as a pair rather than reporting accuracy alone.

**What is a good F1 score for cost coding?**
There is no universal answer, and anyone who quotes one is guessing. The threshold depends on what a wrong line costs to unwind and how much manual checking you can afford. Set it against your own tolerance, record it before the period starts, and review it when the mix of work changes, because a model calibrated on civils will drift on fit-out.

**Does the Gulf need different AI skills from other markets?**
The arithmetic is identical. What differs is the scale and the contracting model: programme-scale delivery with many contractors reporting in different formats creates more machine-assisted consolidation, and therefore more places for an unverified output to enter a reported position. The verification discipline matters more there, not less.

**Can a certification prove someone can govern AI output?**
An examination can test whether a candidate can compute and interpret the measures, choose a threshold and explain what a given error rate does to a forecast. That is a capability test, not an employment outcome, and PCI makes no claim beyond it. It is one documentary input a hiring manager can check rather than infer.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice, and no claim is made about employment outcomes.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: this article should link to [AI in project controls](https://pciai.org/ai-in-project-controls) as the pillar it supports, to [AI project controls certification](https://pciai.org/ai-project-controls-certification) with that anchor, and to [project controls training in the UAE](https://pciglobal.ai/project-controls-training-uae) with that anchor.*
