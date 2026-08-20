---
platform:      DEV Community (dev.to)
type:          guide
title:         Precision, recall and the accrual no model can see
meta:          Precision, recall and F1 on project cost data, and why a model flag is not a control. The failure that has no row cannot be a false negative.
when_to_post:  Launch week + 2. Publish only after the pciai.org original has been live and indexed for at least a full week, and leave at least five clear days after the Medium story, because both carry the same arithmetic under a canonical and the audiences overlap at the edges. Wednesday, 08:00–09:00 US Eastern (13:00–14:00 UK), which is when the DEV feed is busiest and when a post has the longest run at the top of its tags. DEV ranks partly on early engagement, so the author must be available to answer comments for the first six hours; a technical post left unanswered for a day is a dead post here. Do not schedule it into a Friday or a US public holiday.
word_count:    1,508 prose (H1 to the end of the questions block, including table cells, excluding the two code blocks); 1,635 counting the code as well. Excludes the production front matter, the DEV front-matter block, the first-published line and the linking note.
primary_kw:    AI in project controls — inherited via the canonical, not targeted here
secondary_kw:  precision recall F1, AI governance, anomaly detection, cost performance index
pillar:        AI in project controls
credential:    PCL-AI (suite named once)
target_domain: pciai.org
canonical:     canonical -> https://pciai.org/ai-in-project-controls
schema:        Article + FAQPage
hashtags:      DEV tags, four (the maximum), lowercase, no hash — ai, datascience, machinelearning, python
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
canonical_instruction: |
  SET canonical_url IN THE FRONT MATTER BEFORE THE FIRST SAVE, NOT AFTER PUBLISHING. DEV renders
  <link rel="canonical"> straight from that field, and dev.to outranks every domain in this estate.
  Paste the front-matter block below into the DEV editor in markdown mode, publish, then view source
  on the live post and confirm the canonical carries the pciai.org URL and not the dev.to one.
  A post published without it can be edited afterwards, but the first crawl will already have
  happened and the fix is slower than the ten seconds it takes now.
notes: |
  REPUBLISHED, CANONICAL SET. DEV supports canonical_url, so this may legitimately carry a near-copy
  of the pciai.org pillar on AI in project controls. It is an adaptation, not a paste: the site page
  argues from decision rights inwards, this argues from a confusion matrix outwards, which is the
  only order a data person will read. The canonical is the pillar that targets the inherited primary
  keyword, and it is the page that exists. An earlier draft pointed at an AI-governance slug that was
  never authored, and a canonical aimed at a 404 hands the ranking straight back to dev.to.
  PRIMARY KEYWORD IS INHERITED, NOT TARGETED, and the placement rule in _BRIEF.md §4 is therefore
  not applied to it. The phrase appears once, inside the first 100 words, and is deliberately absent
  from the H1, the H2s and the description: the canonical hands ranking for this subject to the
  pciai.org original, and a republish optimised for the same term competes with the page it points
  at. Do not add the phrase to the title later "to help it rank".
  Canonical points at pciai.org, NOT the hub. AI governance is pciai.org's territory per
  _LINK_ARCHITECTURE.md §1, and pointing this at the hub pillar that the Medium story already
  canonicalises to would set two flagship assets competing for one page.
  Hook C (consequence first), which _STORY.md §2 assigns to Medium and DEV. Medium spends its hook on
  the ledger; this one spends it on the detector, and after the opening line the two pieces share
  only the locked arithmetic, the locked stake and the locked three facts. No blended hooks.
  THE CONFUSION MATRIX IS ILLUSTRATIVE ARITHMETIC AND SAYS SO IN THE TEXT. 45/155/15/9,785 describe
  no real model, no real dataset and no real project. They exist to make the formulas do work in
  front of the reader. Every figure derived from them was computed, not estimated: precision 0.225,
  recall 0.75, F1 0.346, accuracy 0.983, and the do-nothing baseline 0.994. The Python block runs as
  written. If anyone edits one cell of that table, every number downstream of it has to be recomputed.
  Numbers about PCI: 13/61, 16/61, 16/63, 113 Standards, 532 process requirements, 40/40/20 labelled
  as the Body of Knowledge's proportions and stated outright not to be the examination's, and 15,613
  inside a sentence naming PFL-AI and PML-AI, with PCL-AI's exclusion in the next breath. Nothing
  else, nothing rounded, no examination weighting anywhere. The 92 sector case studies are in the
  register and are deliberately left out of this asset: a data audience reads the calculation checks
  as evidence and a case-study count as marketing, and the paragraph is stronger carrying one claim.
  The project arithmetic carries no currency, no client, no sector, no date and no claim about how
  often it happens, because there is no researched frequency to cite.
  The SQL is the reason a developer bookmarks this rather than clapping at it. It is deliberately
  plain: the point being made is that the control which catches the error is a set difference and not
  a prediction, and dressing it up would undercut the argument.
  Three links, three domains, one each, all in the body where the sentence raises the question the
  target answers. Anchors differ from every other flagship asset pointing at those pages. No
  pciworld.org or pciglobal.ai link: this piece raises no career and no regional question.
  Tags: DEV allows four and fewer than four is wasted distribution. ai, datascience and
  machinelearning carry the reach; python is earned by the code block and pulls a different feed.
  No cover image is specified. If one is added it must be 1000 × 420, must not contain a fabricated
  chart, and its alt text must describe it.
---

Paste this block at the top of the DEV editor in markdown mode. The `canonical_url` line is not optional.

```yaml
---
title: Precision, recall and the accrual no model can see
published: true
description: A cost report can be arithmetically perfect and still wrong. Precision, recall, F1, and why a model flag is not a control.
tags: ai, datascience, machinelearning, python
canonical_url: https://pciai.org/ai-in-project-controls
---
```

---

# Precision, recall and the accrual no model can see

Fourteen CPI points went missing in an accrual, not a schedule. No anomaly detector on that ledger would have caught it, and its recall score would not have dropped by a point. The error was an entry nobody had made yet, and a model cannot score a row that does not exist.

This is the part of AI in project controls that the metrics do not cover. The expensive failures are absences, and an absence has no features.

Here is the whole of it, in round numbers that belong to no project.

## The fourteen points

Earned value is 2,200,000. Invoiced cost is 1,850,000.

**2,200,000 ÷ 1,850,000 = 1.19**

A cost performance index of 1.19 says every unit of cost bought 1.19 units of work. On the evidence in the system, that is correct. Most people would sign it.

Now the part the system has not seen. There is 240,000 of work done and not yet invoiced. Accrue it.

1,850,000 + 240,000 = 2,090,000

**2,200,000 ÷ 2,090,000 = 1.05**

Fourteen points, on one entry. Nothing had happened on site. Finance had closed to a reporting calendar, progress had closed to a data date set by the site, and between the two sat work performed and not yet billed.

The error is accounting. The damage is delivery. Neither training alone catches it.

## What precision, recall and F1 actually say

Precision is the share of the model's flags that were real: TP ÷ (TP + FP). Recall is the share of real cases the model flagged: TP ÷ (TP + FN). F1 is their harmonic mean, which stops a model buying one by giving up the other.

Take a screening model over one period of cost transactions. These figures are illustrative arithmetic. They describe no real model and no real dataset.

| | Model flags | Model silent |
|---|---|---|
| **Genuine error** | 45 | 15 |
| **No error** | 155 | 9,785 |

```python
tp, fp, fn, tn = 45, 155, 15, 9_785

precision = tp / (tp + fp)                                # 0.225
recall    = tp / (tp + fn)                                # 0.75
f1        = 2 * precision * recall / (precision + recall)  # 0.346
accuracy  = (tp + tn) / (tp + fp + fn + tn)                # 0.983

# the model that flags nothing at all
baseline  = (tn + fp) / (tp + fp + fn + tn)                # 0.994
```

More than three flags in four are wrong, and one error in four is missed. The accuracy figure, 98.3 per cent, is the one that would reach a steering group, and it is beaten by a model that does nothing: predict "no error" every time and you score 99.4 per cent.

At a prevalence of 60 in 10,000, accuracy measures the base rate rather than the model. Precision and recall are properties of a threshold rather than of a model, so both move when somebody tunes it. "The model is 98 per cent accurate" is not a sentence anyone can act on.

One question comes before all of these. Who labelled the 60? If the labels came out of the same monthly review the model is meant to replace, the score measures agreement with that review, not truth.

## Why "the AI found something" is not a control

A control and an alert are different objects, and the gap between them is where assurance work sits.

| What a flag gives you | What a control has to have |
|---|---|
| one output, on one run | a defined population it runs over every period |
| an implicit threshold | a stated exception condition |
| a score | a measured error rate in both directions, at the operating threshold |
| a notification | a named owner who has to act on it |
| an interesting finding | a defined action and a date by which it happens |
| a log line when it fires | evidence that it ran, including the runs that found nothing |

A model gives you an alert. A control gives you evidence that something ran even when it found nothing. The last row is the one an auditor asks for first and the one almost nobody builds.

## The failure mode the metrics cannot see

Recall is measured against the errors you labelled. A missing accrual is not a false negative. It is not in the denominator.

Anomaly detection operates on rows. A missing accrual is the absence of a row: no invoice, no posting, no feature vector, no residual large enough to rank. It is only visible where two systems disagree about what month it is.

So the control that catches it is not a model. It is a join.

```sql
-- not a prediction: a set difference between two calendars
select p.work_package,
       p.earned_value - coalesce(sum(i.amount), 0) as uninvoiced
from progress p
left join invoices i
       on i.work_package = p.work_package
      and i.posted_date <= :ledger_cutoff
where p.data_date > :ledger_cutoff
group by p.work_package, p.earned_value
having p.earned_value - coalesce(sum(i.amount), 0) > 0;
```

That query is not clever and it will not appear in a conference talk. It moves 1.19 to 1.05 before the report leaves the building, which is the only test it has to pass.

Models earn their place on the far side of that join, ranking the candidates the reconciliation produces so that a reviewer with time for twenty reads the right twenty. That is a scarce-attention problem with a measurable answer, and a better use of a model than asking it to notice something nobody wrote down.

## Accountancy exams rarely test float. Engineering exams rarely test cut-off.

None of this is an argument about competence, and the version that blames people is wrong. A chartered accountant knows exactly what an accrual is. A planner knows exactly what earned value counts.

The gap is in what each profession is *examined* on, and therefore in what each is assumed to have covered already. One profession produces the estimate at completion. The other consumes it, unchanged, in a statement that will be audited. Neither is examined on the handover, and the handover is where the money goes.

The Project Controls Institute exists for that handover. Three credentials, each with its own Body of Knowledge and its own examination: the PCI AI Project Controls Leader (PCL-AI), the PCI AI Project Finance Leader (PFL-AI) and the PCI Project Management Leader – AI (PML-AI).

| | PCL-AI | PFL-AI | PML-AI |
|---|---|---|---|
| Domains | 13 | 16 | 16 |
| Knowledge areas | 61 | 61 | 63 |
| Centre of gravity | measurement: schedule, progress, earned value, quantitative risk | money: recognition, forecasting, working capital, project finance | delivery: contract, scope, stakeholders, integrated execution |

Underneath them sit 113 mandatory PCI Standards carrying 532 process requirements. Those are certification requirements established by the Institute. They are not law, and nothing PCI publishes is legal, tax or accounting advice.

The Bodies of Knowledge are built in proportions of 40 per cent finance and reporting, 40 per cent project management and 20 per cent governed AI. Those proportions describe the Body of Knowledge. They do not describe the examination. No examination weighting is published, because the syllabus is settled while the exam blueprint is still an open decision, and any PCI exam weighting you see quoted did not come from us.

An article built on arithmetic should say how its own arithmetic is checked. There are 15,613 machine calculation checks running against PFL-AI and PML-AI, all passing. PCL-AI has no equivalent suite yet. Saying so costs the cleaner sentence, and it is the reason the number is worth anything.

## The part that is yours

You sign the forecast. Miss the accrual and the number you defended was wrong before you saw it. Seniority means owning both ledgers, and a credential that examines only one half leaves you accountable for a gap that nobody taught you.

Do not take anyone's word for the syllabus, this article included. Read [the PCL-AI syllabus, domain by domain](https://projectcontrolsinstitute.org/body-of-knowledge), and hold it against your own last month-end.

Then go and find the accrual nobody asked about.

## Questions

**Why not train a model on historical accruals?**
Because the only labelled accruals are the ones somebody eventually caught. The ones nobody caught are unlabelled and sit in the data looking like clean months. Train on that and you learn to predict which errors get found, which is a different target with a comforting score.

**What precision would make a screening model worth deploying?**
State the review capacity first. If a reviewer can work through 40 items a month, rank the queue and measure precision at 40, because that is the number deciding whether next month's queue gets read at all. A model tuned to a global F1 optimum and handed to a team with no capacity is a report, not a control.

**Isn't a cut-off mismatch just a data quality problem?**
Partly, and the part that is will not be fixed by a rule, because a rule needs somebody to know it should exist. Two calendars closing on two dates is structural. It recurs every period and needs a reconciliation with an owner, not a validator on a field.

**What does "governed AI" mean in an examination?**
The governed use of machine output in decisions someone signs. Reading precision, recall and F1 instead of a headline accuracy figure. Knowing the base rate before believing the score. Setting out which figures a model may originate, which it may only restate, and whose name is on the output. [The decision rights an AI policy for project controls has to set out](https://pciai.org/ai-policy-for-project-controls) fit on fewer pages than most teams expect.

**Does a PCI credential replace a chartered qualification?**
No, and it is not built to. Where statutory or chartered status is required, that requirement is met by the relevant qualification. These credentials examine the crossing between finance and delivery, which is a different question. If you already hold one, [which credential examines which half of the problem](https://credentialfinder.org/best-project-controls-certification) is more useful than another brochure.

---

*First published on pciai.org.*

---

*Internal links: three, one per domain, each sitting in a sentence that raises the question the target answers. [The decision rights an AI policy for project controls has to set out](https://pciai.org/ai-policy-for-project-controls) answers "so who decides what the model is allowed to produce", raised by the governed-AI question; note this is a different pciai.org page from the canonical target, so the estate keeps one body link per domain and the canonical does its own job. [The PCL-AI syllabus, domain by domain](https://projectcontrolsinstitute.org/body-of-knowledge) answers "what would examining both sides actually cover", raised by the ask. [Which credential examines which half of the problem](https://credentialfinder.org/best-project-controls-certification) answers "how is this different from the credential I already hold". The closing "first published" line stays unlinked because the canonical already carries that relationship. The comment thread carries no URLs either, and on DEV that matters more than it does elsewhere, because comments index with the post. A reader arguing about cut-off dates is told in plain words that the hub sets out the month-end close; a reader who wants the whole period worked through is told a worked earned value month sits there too. Pasting either address below would take one hub link to three on one indexed page.*
