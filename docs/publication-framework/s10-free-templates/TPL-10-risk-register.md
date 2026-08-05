---
id: TPL-10
series: S10
series_name: Free Templates
title: Risk register
subtitle: A register that separates cause, event and effect — and connects the score to the money
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [practitioner, manager]
level: practitioner
reading_time_min: 20
summary: >
  A risk register built so that each entry can be acted on rather than admired. Cause, event and effect are
  three separate fields, not one sentence. Probability and impact are scored against a scale the project
  agrees and writes down, cost and schedule impact are assessed separately, and every risk carries a named
  owner, dated actions, a post-response assessment and an explicit link to contingency. The scoring scale is
  supplied as a convention to be agreed, not as a universal truth.
linkedin:
  format: document
  hook: >
    Most risk registers fail at the first field. "Bad weather" is not a risk — it is a cause. Split cause,
    event and effect into three columns and half the register turns out to be duplicates, issues or wishes.
  tags: [ProjectControls, RiskManagement, ProjectRisk, Contingency, CostEngineering]
  asset: one-pager
gated: false
related: [BPG-16, BPG-10, BPG-17, TPL-11, TPL-15]
bok_domains: [5, 12]
sources: []
placeholders: 0
---

# Risk register

> A register that separates cause, event and effect — and connects the score to the money.

**In one paragraph.** A risk register built so that each entry can be acted on rather than admired. Cause,
event and effect are three separate fields, not one sentence. Probability and impact are scored against a
scale the project agrees and writes down, cost and schedule impact are assessed separately, and every risk
carries a named owner, dated actions, a post-response assessment and an explicit link to contingency. The
scoring scale is supplied as a convention to be agreed, not as a universal truth.

**Who this is for.** Risk managers, project controls managers, cost engineers and planners who maintain the
register; the risk owners named in it; and the project managers and sponsors who have to decide what to
fund.

---

## 1. When to use this

Open it at the point the project scope is stable enough to be described and keep it open until closeout.
Three uses justify the effort, and a register that serves none of them is administration.

**To decide where to spend money and attention.** A register earns its place when it changes what somebody
does this week. That is why response actions, owners and dates are in the same row as the score.

**To feed the quantification.** The cost risk model and the quantitative schedule risk analysis both draw
their discrete events from here. If the register is loose, the model is precise nonsense. The mapping to the
schedule analysis is `TPL-11 — Quantitative schedule risk analysis input sheet`.

**To justify contingency.** Contingency held without a stated basis is a number that will be taken away by
the first person who asks where it came from. Columns AL to AN make the link explicit, risk by risk.

It is not an issues log. An issue has happened; a risk has not. Mixing them is the fastest way to make a
register unreadable, because the two need different fields and different meetings. Keep a separate issues
log and cross-reference it in column AR when a risk is realised.

## 2. How to complete it

### 2.1 Write the risk statement in three fields, not one

The cause, the event and the effect are separate columns on purpose. Written as one sentence they collapse,
and what collapses is always the part that would have told you what to do.

- **Cause** (column H) is a condition that exists now, or will exist. It is a fact, not a possibility. *The
  ground investigation covered only the northern half of the site.*
- **Event** (column I) is the uncertain occurrence. It is the only part of the row that carries a
  probability. *Obstructions are found in the southern foundations zone.*
- **Effect** (column J) is the consequence on a project objective, described so that it can be measured.
  *Additional excavation and disposal, and delay to the foundations completion milestone.*

The discipline pays for itself immediately. "Bad weather" is a cause and cannot be scored. "Delay to
handover" is an effect and will appear against a dozen unrelated events. If you cannot fill all three
fields, you do not yet have a risk — you have a worry, and the honest thing is to log it as one and go and
find out which of the three parts is missing.

Two tests are worth applying to every new entry. If the cause is already certain and the event has already
occurred, it is an issue. If two rows share the same event, they are one risk with two effects, and merging
them will change the score.

### 2.2 Agree the scoring scale before the first row

**The scale below is a convention, not a universal truth.** There is no correct probability–impact scale.
Different scales are defensible, they produce different rankings of the same risks, and none of them is more
right than another in the abstract. What matters is that the project agrees one scale, writes it down, uses
it consistently, and states it on the face of every report that quotes a score. A register scored against
two different scales — the usual result of a change of risk manager — is worse than no register, because it
looks comparable and is not.

Agree the scale at the same meeting that appoints the risk owners, record it on a `Scales` sheet, and do not
change it mid-project without rescoring every open risk.

**Probability bands.** Lower bound inclusive, so that a value falling exactly on a boundary has one
unambiguous home.

| Score | Band | Midpoint used for expected value |
|---|---|---|
| 1 | 0 % to under 10 % | 5 % |
| 2 | 10 % to under 30 % | 20 % |
| 3 | 30 % to under 50 % | 40 % |
| 4 | 50 % to under 70 % | 60 % |
| 5 | 70 % and above | 85 % |

Record the probability as a percentage in column N and let the score calculate. Where an analyst has a
considered percentage, use it directly for expected value rather than the band midpoint — the midpoints
exist for rows where only a band was ever elicited.

**Cost impact bands.** *Illustrative calibration.* Expressed as a proportion of the approved project budget
so that the scale is currency-neutral. Replace with the thresholds your project agrees.

| Score | Band |
|---|---|
| 1 | Under 0.1 % of the approved budget |
| 2 | 0.1 % to under 0.5 % |
| 3 | 0.5 % to under 2 % |
| 4 | 2 % to under 5 % |
| 5 | 5 % and above |

Calibrating to the budget makes registers comparable across projects of different sizes. It also means that
a score of 5 on a small project is not the same amount of money as a score of 5 on a large one, which
matters the moment a portfolio function aggregates registers. If that aggregation is going to happen, agree
absolute currency thresholds instead and say so.

**Schedule impact bands.** *Illustrative calibration.* Working days of delay to the affected contractual
milestone, not to the activity.

| Score | Band |
|---|---|
| 1 | Under 5 working days |
| 2 | 5 to under 10 |
| 3 | 10 to under 20 |
| 4 | 20 to under 40 |
| 5 | 40 and above |

**Severity** is the probability score multiplied by the higher of the two impact scores, giving a range of 1
to 25. Taking the higher rather than the sum is a deliberate convention: a risk that is severe on either
axis is severe, and adding cost and schedule scores flatters risks that are mildly bad at everything over
risks that are catastrophic at one thing. Other conventions are defensible — separate cost and schedule
severities, or a weighted combination. Pick one, state it, and never run two in one register.

### 2.3 Score twice: before the response and after it

The pre-response assessment is what the project faces if nothing further is done. The post-response
assessment is what it faces once the actions in columns W to Z have been completed — not once they have been
proposed. Until an action is complete, the post-response score is a forecast, and the register should be
read that way.

The difference between the two, priced in column AJ and tested against the cost of the response in column
AK, is the argument for spending the money. It is a prompt for a conversation and not a decision rule: a
response can be worth buying at a ratio well below 1 when the risk threatens something the project cannot
absorb, such as a milestone with a monetary consequence or a consent that cannot be re-applied for.

### 2.4 Set the review cadence when you open the row

Column AQ is a date, not a frequency. A risk with a proximity three months away and a next review in six
months is not being managed. Set the next review inside the proximity window, and make the risk owner —
not the risk manager — accountable for the review.

## 3. The template

Header row in row 1; data from row 2. Formulas below are written for row 2 and fill down. The scoring
thresholds live on a sheet named `Scales`: column B holds the five probability lower bounds, column C the
five cost lower bounds and column D the five schedule lower bounds, each starting at 0 in row 2 and
ascending to row 6.

### 3.1 Input columns

| Col | Field | What goes in it |
|---|---|---|
| A | Risk ID | Permanent, never reused, e.g. `RSK-014` |
| B | Date raised | The date the entry was created |
| C | Raised by | Named individual |
| D | Status | Open · In treatment · Closed · Realised · Retired |
| E | Category | From the project's agreed taxonomy — design, ground, consents, supply chain, resource, interface, commercial, weather, security, digital and data |
| F | WBS or control account reference | Where the effect lands in the cost and schedule structures |
| G | Threat or opportunity | Threat · Opportunity. Opportunities use the same fields with the sign reversed |
| H | Cause | The condition that exists. A fact, stated without hedging |
| I | Event | The uncertain occurrence. The only part carrying a probability |
| J | Effect | The consequence on an objective, described so it can be measured |
| K | Objective affected | Cost · Schedule · Scope · Quality · Safety · Environment · Consent · Reputation |
| L | Risk owner | The named individual who can actually influence the event, not the person who wrote the row |
| M | Proximity | The date or window in which the event could occur |
| N | Pre-response probability | Percentage, as a decimal |
| P | Pre-response cost impact | Currency, the most likely cost consequence if the event occurs |
| R | Pre-response schedule impact | Working days of delay to the affected milestone if the event occurs |
| V | Response strategy | Threats: Avoid · Reduce · Transfer · Accept. Opportunities: Exploit · Enhance · Share · Accept |
| W | Response actions | What will be done, specifically enough to be verifiable |
| X | Action owner | Named individual, one per action; split the row if there are several |
| Y | Action due date | A date, not a month |
| Z | Action status | Not started · In progress · Complete · Abandoned |
| AA | Cost of response | The money and resource the actions consume |
| AB | Post-response probability | Percentage, as a decimal, once the actions are complete |
| AD | Post-response cost impact | Currency |
| AF | Post-response schedule impact | Working days |
| AL | In the quantitative model? | Yes · No, with the model reference. Says whether this risk is represented in the cost or schedule simulation |
| AM | Contingency treatment | Held in contingency · Included in the cost forecast · Not provided · Transferred |
| AN | Amount provided | Currency amount actually carried, in contingency or in the forecast |
| AO | Residual accepted by | The named person who has accepted the post-response position, and the date |
| AP | Date last reviewed | |
| AQ | Date of next review | Inside the proximity window |
| AR | Closure or realisation outcome | What actually happened, recorded at closure. This is what makes the register useful to the next project |

### 3.2 Calculated columns

| Col | Field | Formula in words | Spreadsheet expression |
|---|---|---|---|
| O | Pre-response probability score | The band the probability falls into, using lower-bound-inclusive thresholds | `=IF($N2="","",IFERROR(MATCH($N2,Scales!$B$2:$B$6,1),""))` |
| Q | Pre-response cost impact score | The band the cost impact falls into | `=IF($P2="","",IFERROR(MATCH($P2,Scales!$C$2:$C$6,1),""))` |
| S | Pre-response schedule impact score | The band the schedule impact falls into | `=IF($R2="","",IFERROR(MATCH($R2,Scales!$D$2:$D$6,1),""))` |
| T | Pre-response severity | Probability score multiplied by the higher of the two impact scores | `=IF(OR($O2="",$Q2="",$S2=""),"",$O2*MAX($Q2,$S2))` |
| U | Pre-response expected cost | Probability multiplied by cost impact | `=IF(OR($N2="",$P2=""),"",$N2*$P2)` |
| AC | Post-response probability score | As column O, on the post-response probability | `=IF($AB2="","",IFERROR(MATCH($AB2,Scales!$B$2:$B$6,1),""))` |
| AE | Post-response cost impact score | As column Q, on the post-response cost impact | `=IF($AD2="","",IFERROR(MATCH($AD2,Scales!$C$2:$C$6,1),""))` |
| AG | Post-response schedule impact score | As column S, on the post-response schedule impact | `=IF($AF2="","",IFERROR(MATCH($AF2,Scales!$D$2:$D$6,1),""))` |
| AH | Post-response severity | Post-response probability score multiplied by the higher of the two post-response impact scores | `=IF(OR($AC2="",$AE2="",$AG2=""),"",$AC2*MAX($AE2,$AG2))` |
| AI | Post-response expected cost | Post-response probability multiplied by post-response cost impact | `=IF(OR($AB2="",$AD2=""),"",$AB2*$AD2)` |
| AJ | Expected cost avoided | Pre-response expected cost less post-response expected cost | `=IF(OR($U2="",$AI2=""),"",$U2-$AI2)` |
| AK | Expected value ratio of the response | Expected cost avoided divided by the cost of the response; reported as text where no response cost has been recorded | `=IF($AJ2="","",IF(N($AA2)=0,"No response cost recorded",$AJ2/$AA2))` |

Two summary cells:

| Field | Formula in words | Spreadsheet expression |
|---|---|---|
| Sum of pre-response expected cost | Total expected cost across all open threats | `=SUMIFS($U:$U,$D:$D,"Open",$G:$G,"Threat")` |
| Proportion of severity 12 or above with a complete response | Of the risks scoring 12 or more, the share whose actions are complete; blank if there are none | `=IF(COUNTIF($T:$T,">=12")=0,"",COUNTIFS($T:$T,">=12",$Z:$Z,"Complete")/COUNTIF($T:$T,">=12"))` |

**The expected cost figures are not contingency.** Summing expected cost across a register produces a mean,
and a mean is not a number you can hold. It ignores correlation, it ignores that a discrete risk either
happens in full or does not happen at all, and it says nothing about the tail you are actually funding
against. Use the register to feed a simulation and set contingency from the distribution — see
`BPG-10 — Contingency and management reserve`.

### 3.3 Pasting it into a spreadsheet

Copy the header line into cell A1 and split on the pipe character. Format columns N, AB as percentages and
B, M, Y, AP, AQ as dates, then apply the §3.2 formulas to row 2 and fill down.

```
Risk ID|Date raised|Raised by|Status|Category|WBS or control account|Threat or opportunity|Cause|Event|Effect|Objective affected|Risk owner|Proximity|Pre probability|Pre probability score|Pre cost impact|Pre cost score|Pre schedule impact (wd)|Pre schedule score|Pre severity|Pre expected cost|Response strategy|Response actions|Action owner|Action due|Action status|Cost of response|Post probability|Post probability score|Post cost impact|Post cost score|Post schedule impact (wd)|Post schedule score|Post severity|Post expected cost|Expected cost avoided|Response value ratio|In quantitative model?|Contingency treatment|Amount provided|Residual accepted by|Last reviewed|Next review|Closure outcome
```

In Markdown, split it into three blocks that share the risk ID: the statement block (A to M), the assessment
block (N to AK) and the treatment block (AL to AR). Three readable tables beat one unreadable one.

## 4. Worked fragment

*Illustrative figures.* Currency-neutral units. Schedule impacts in working days against the affected
contractual milestone. This project has agreed absolute cost bands with lower bounds of 50,000 · 150,000 ·
400,000 · 1,000,000, and the illustrative schedule bands from §2.2.

**Statement block**

| Risk ID | Cause | Event | Effect | Objective | Owner | Proximity |
|---|---|---|---|---|---|---|
| RSK-014 | The ground investigation covered only the northern half of the site | Obstructions are found in the southern foundations zone | Additional excavation and disposal, and delay to the foundations completion milestone | Cost, Schedule | Construction manager | Foundations window, periods 4 to 7 |
| RSK-021 | The permit authority's published determination period is longer than the period assumed in the baseline | The discharge consent is granted later than the baseline date | Commissioning cannot start, delaying the takeover milestone | Schedule | Consents lead | Periods 9 to 12 |

**Assessment block**

| Risk ID | Pre prob | Score | Pre cost | Score | Pre sched | Score | Severity | Expected cost | Post prob | Score | Post cost | Score | Post sched | Score | Severity | Expected cost | Avoided | Response cost | Ratio |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| RSK-014 | 40 % | 3 | 180,000 | 3 | 15 | 3 | 9 | 72,000 | 30 % | 3 | 90,000 | 2 | 6 | 2 | 6 | 27,000 | 45,000 | 25,000 | 1.8 |
| RSK-021 | 50 % | 4 | 60,000 | 2 | 30 | 4 | 16 | 30,000 | 50 % | 4 | 60,000 | 2 | 8 | 2 | 8 | 30,000 | 0 | 12,000 | 0.0 |

**The substitutions.**

RSK-014 pre-response severity: probability 40 % falls in the band from 30 % to under 50 %, so the score is
3. Cost impact 180,000 is at or above 150,000 and below 400,000, so the score is 3. Schedule impact 15
working days is at or above 10 and below 20, so the score is 3. Severity is `3 × MAX(3, 3) = 9`. Expected
cost is `0.40 × 180,000 = 72,000`.

RSK-014 post-response: the response is to reduce — commission a supplementary ground investigation across
the southern zone before foundation works begin, and pre-agree a rate for obstruction removal. Note what
each part of that does. The investigation does not change the ground; it changes what is known, which allows
the design and the sequence to be adjusted, which is what moves the probability of the *effect* from 40 % to
30 %. The pre-agreed rate is what moves the cost impact, from 180,000 to 90,000. Post severity is
`3 × MAX(2, 2) = 6` and post expected cost is `0.30 × 90,000 = 27,000`. Expected cost avoided is
`72,000 − 27,000 = 45,000` against a response cost of 25,000, giving a ratio of `45,000 ÷ 25,000 = 1.8`.

Notice that the probability *score* did not move even though the probability did: a ten-point reduction
sits entirely inside one band. That is a property of every banded scale and it is worth knowing about before
somebody asks why the response achieved nothing. Bands are for sorting; the underlying percentage is for
arithmetic.

RSK-021 shows why severity takes the higher of the two impact scores rather than the sum. Probability 50 %
scores 4. Cost impact 60,000 scores 2. Schedule impact 30 working days scores 4. Severity is
`4 × MAX(2, 4) = 16`, second only to nothing else on this fragment — and correctly so, because a delayed
takeover milestone is the damage, not the extended preliminaries.

It also shows the limits of expected cost as a test. The response — escalate the application, agree a
commissioning sequence that does not depend on the consent, and re-plan so the consent-dependent work sits
off the critical path — leaves the cost impact untouched at 60,000 and the probability untouched at 50 %,
because none of it changes how long the authority takes. Expected cost avoided is `30,000 − 30,000 = 0` and
the ratio is 0.0. On an expected-cost test the response is worthless. On the schedule it halves severity
from 16 to 8. Any register that scores only cost will systematically underprice schedule-driven risk, and
this is what that looks like in a single row.

## 5. Common mistakes

**One sentence instead of three fields.** "Risk of delay due to late design" contains a cause, an event and
an effect in a form where none of them can be scored, owned or acted on. The three-column discipline is the
whole reason this register produces different behaviour from the last one.

**Issues logged as risks.** If the event has already happened, the probability field is meaningless and the
row will sit at severity 25 for the rest of the project, crowding out everything that can still be
influenced. Move it to the issues log and record the realisation outcome in column AR.

**Effects logged as risks.** "Cost overrun" and "delay to handover" are the effects of dozens of events. A
register organised by effect cannot tell anyone what to do on Monday.

**The register owner as the risk owner.** The risk manager owns the process. The risk owner must be the
person with the authority to change the outcome. When column L fills up with the same name, the register has
become somebody's private document.

**Post-response scores recorded as though the actions were done.** This is the most damaging error in the
template, because it makes the register report a position the project does not hold. Column Z exists to
prevent it: read the post-response score only in the light of the action status beside it.

**Scores that never move.** A register in which last month's scores are this month's scores has not been
reviewed; it has been reopened. Column AP dates the review and column AQ commits to the next one, and both
should be audited.

**Changing the scale mid-project.** Rescoring against a new scale without rescoring the closed and dormant
rows produces a register in which a 12 from March and a 12 from September mean different things. If the
scale must change, rescore everything open and annotate the change.

**Summing expected cost and calling it contingency.** Covered in §3.2, and worth stating twice because it is
the most common quantitative error in the discipline. The sum of expected values is a mean. Contingency is
set from a distribution at a confidence level the project has chosen and can defend.

**Opportunities absent.** A register with no opportunities in it is a statement about the culture, not about
the project. The same fields work with the sign reversed, and the strategies are Exploit, Enhance, Share and
Accept.

## 6. Adapting it

**Safe to change.** The category taxonomy, the band thresholds, the severity convention, the number of
scoring levels, and the currency. Add columns for anything your governance requires — a portfolio reference,
a contract package, an insurance position, a regulatory flag. Add a second schedule impact against a
different milestone if the project has more than one date that carries a consequence.

**Safe to simplify.** On a small project, columns AL to AO can collapse into a single note field, and the
post-response block can be omitted until responses are actually being planned. Nothing is lost as long as
the three statement fields survive.

**Do not change.** The separation of cause, event and effect. The requirement that the scale is agreed,
written down and quoted alongside every score. The rule that the risk owner is a named individual. And the
distinction between the pre-response and post-response assessment — a register with one assessment cannot
show what the response bought, which means it cannot justify the response.

### 6.1 Before the risk review meeting

- Every open row has all three statement fields populated, and none of them contains two of the others.
- Every open row has a named risk owner who is in the room or has sent a position.
- The scoring scale is printed on the agenda, in the version currently in force.
- Every risk with a proximity inside the next reporting period has been reviewed since the last meeting.
- Every action past its due date has either a completion date or an explanation, and no action has been
  silently re-dated.
- The top ten by pre-response severity and the top ten by post-response severity have both been produced —
  they are usually different lists, and the difference is the agenda.
- Contingency treatment is stated for every risk scoring 12 or above, and the amounts reconcile to the
  contingency actually held.
- Any risk realised since the last meeting has been closed with an outcome recorded in column AR and, where
  it changed the scope or the cost, a corresponding entry in the change order log.

---

## Related

- `BPG-16 — Risk registers that work` — the method behind this instrument, including how to run an
  identification workshop that produces events rather than worries
- `BPG-10 — Contingency and management reserve` — how a register becomes a defensible contingency figure,
  and why the sum of expected values is not it
- `BPG-17 — Quantitative schedule risk analysis` — what happens to the schedule-impact columns once they
  reach a simulation
- `TPL-11 — Quantitative schedule risk analysis input sheet` — the sheet the discrete events in this
  register feed into, and the double-counting check between the two
- `TPL-15 — Project controls health check` — the risk dimension of the health check assesses whether this
  register is being used or merely maintained

## Sources and standards

This is an original instrument developed by the Institute. It reproduces no third-party template, form,
matrix or scoring scale. The three-part risk statement, the probability–impact convention and the four
response strategies are general practice in the discipline, described here in the Institute's own words. The
principle that risk is the effect of uncertainty on objectives is the organising idea of ISO 31000, which is
named here as a framework and not quoted; no edition of it was consulted for this template and nothing in it
is reproduced. The scales in §2.2 are conventions offered for adaptation, not standards, and are labelled as
such wherever they appear.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
