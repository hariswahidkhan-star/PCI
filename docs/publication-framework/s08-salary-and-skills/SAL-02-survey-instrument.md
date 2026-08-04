---
id: SAL-02
series: S08
series_name: Salary and Skills Report
title: The survey instrument
subtitle: The questionnaire, question by question, ready to field
version: 1.0
status: draft
date: 2026-08-04
authors: [PCI Editorial]
audience: [manager, employer, academic, practitioner]
level: professional
reading_time_min: 28
summary: >
  The complete questionnaire for the Institute's salary and skills survey: consent and privacy wording,
  screening, role and level, employment context, contract type, experience and qualifications, the
  decomposed remuneration block, skills held against skills required, AI tool use, satisfaction and
  mobility. Every question carries its wording, its answer options, its routing and a note on why it is
  worded that way. No figure, band or example value appears anywhere in the instrument, because showing
  one would move the answers.
linkedin:
  format: document
  hook: >
    Most salary surveys ask one question about pay. This one asks eight, because base, allowances, bonus,
    overtime and day rate are different quantities and averaging them together produces a number that
    describes nobody.
  tags: [SalarySurvey, ProjectControls, SurveyDesign, ResearchMethod]
  asset: one-pager
gated: false
related: [SAL-01, SAL-03, SAL-04, SAL-05]
bok_domains: [13]
sources: []
placeholders: 9
---

# The survey instrument

> The questionnaire the Institute will field, with wording, options, routing and design notes for every question.

**In one paragraph.** This is the complete questionnaire for the Institute's salary and skills survey:
consent and privacy wording, screening, role and level, employment context, contract type, experience and
qualifications, the decomposed remuneration block, skills held against skills required, AI tool use,
satisfaction and mobility. Every question carries its wording, its answer options, its routing and a note
on why it is worded that way. No figure, band or example value appears anywhere in the instrument, because
showing one would move the answers.

**Who this is for.** The research lead and reviewers who will pilot, translate and field this survey;
subject-matter experts asked to critique it before it goes live; and anyone building a comparable
instrument who would rather start from something argued than from a blank page.

---

## 1. How to use this instrument

Field it as written, or change it and record the change. The questions are not interchangeable phrasings
of a general idea: the wording, the option sets and the order have been chosen against the biases named in
`SAL-01` §5, and a well-meaning edit can undo a control silently. Where a change is needed, it goes in the
instrument change log carried in `SAL-05` §12, because a question that changed between cycles cannot be
compared across them.

**Notation used below.**

| Convention | Meaning |
|---|---|
| `Q00` | Question number, stable across cycles. Retired numbers are never reused |
| *Type* | `single` (one option), `multi` (several), `numeric`, `text`, `scale`, `matrix` |
| *Routing* | The condition under which the question is shown. Unconditional unless stated |
| *Required* | Whether the respondent may continue without answering |
| *Note* | Why the question is worded this way. Not shown to the respondent |
| A bracketed `CONFIRM` marker | Wording or a parameter that cannot be settled without a decision or a legal review |

**Three rules bind every question.** No figure, band, average or "typical" value is ever shown to a
respondent before they answer — anchoring is the fastest way to ruin a pay survey. Every remuneration
question offers *Prefer not to say*, so a respondent who will not disclose one component does not abandon
the whole response. And no question is mandatory except the screening and consent items, because a forced
answer is a guessed answer.

## 2. Block A — consent, privacy and data handling

This block is shown first, on its own screen. The respondent cannot proceed without an affirmative action.
The wording below is a drafting starting point and is **not cleared for use**: every bracketed item
requires legal review in the jurisdictions where the survey is fielded, and the Institute must not present
one jurisdiction's data-protection regime as the general case.

> **Before you start**
>
> This survey is run by the Project Controls Institute. It asks about your role, your skills and how you
> are paid. It takes about fifteen minutes.
>
> **What we collect.** Your answers, and technical information your browser sends. We do not ask for your
> name, your employer's name, or your address. If you choose to give us an email address at the end, it is
> stored separately from your answers.
>
> **Why we collect it.** To produce an annual public report on reward and skills in project controls. The
> report contains statistics only. No individual and no employer is identified in anything we publish, and
> no figure is published for any group smaller than the threshold stated in our published methodology.
>
> **Who controls the data.** `[CONFIRM: named data controller entity, registered address and contact
> point for data-protection queries — pending legal review]`
>
> **Our lawful basis.** `[CONFIRM: lawful basis for processing in each jurisdiction where the survey is
> fielded, and the wording used to describe it — pending legal review]`
>
> **How long we keep it.** `[CONFIRM: retention period for individual responses and for the analysis
> dataset, and the deletion process at the end of it — pending legal review]`
>
> **Where it is processed.** `[CONFIRM: survey platform and processor, hosting jurisdiction, and the
> mechanism relied on for any cross-border transfer — pending legal review]`
>
> **Withdrawing.** You can stop at any point and nothing is saved. After you submit, you can ask us to
> delete your response using the reference shown on the final screen
> `[CONFIRM: withdrawal and erasure procedure, the window during which it is possible, and the point after
> which responses are irreversibly aggregated — pending legal review]`.
>
> **What you get.** Nothing is required of you and nothing is sold to you. If you want the report when it
> is published, you can leave an email address at the end.
> `[CONFIRM: whether any incentive or prize draw is offered, and if so its full terms, eligibility and the
> jurisdictions in which it may lawfully be run — pending legal review]`

**Q01 — Consent.** *Type: single. Required.*
"I have read the above and I am willing to take part."
Options: `Yes, continue` · `No, exit`.
Routing: `No` ends the survey with a thank-you screen and no data retained.
*Note:* an explicit affirmative action, not a pre-ticked box and not implied consent from proceeding. It
is also the last screen on which a respondent can leave costlessly, which is why the commitment (about
fifteen minutes) is stated above it rather than discovered halfway through.

## 3. Block B — screening

**Q02 — Current involvement.** *Type: single. Required.*
"Which of these best describes your work in the twelve months to the reference date shown above?"
Options: `I worked mainly in a project controls role` · `I worked in a role that included project controls
among other duties` · `I manage or lead people who do project controls work` · `I work in project
controls software, recruitment, training or consulting to the field, but not in a controls role myself` ·
`None of these`.
Routing: `None of these` and the software/recruitment/training option exit to a thank-you screen.
*Note:* the fourth option exists to catch, and politely remove, the group most likely to be interested in
a pay survey and least appropriate for it. Without it, those respondents pick the nearest fitting option
and contaminate the sample.

**Q03 — Country of work.** *Type: single, searchable list. Required.*
"In which country did you do most of your work during the period?"
*Note:* country of *work*, not of residence, citizenship or employer registration. Rotational and remote
cross-border working makes these four different answers, and pay attaches to the work location far more
often than to the other three.

**Q04 — Cross-border working pattern.** *Type: single.*
"Did you work mainly in that country, or on a rotation or assignment away from your home country?"
Options: `Mainly in that country, living there` · `Rotational or fly-in fly-out` · `On international
assignment or secondment` · `Fully remote from another country` · `Prefer not to say`.
*Note:* rotation and expatriate assignment packages are structurally different from local pay and must be
separable in analysis. Merging them produces cells that look like extreme local pay and are not.

**Q05 — Reference period check.** *Type: single. Required.*
"Were you in paid work in this field for at least six months of the period?"
Options: `Yes` · `No`.
Routing: `No` exits. *Note:* keeps the sample to people with a full enough period for the remuneration
questions to mean something, and prevents part-period pay being reported as annual pay.

## 4. Block C — role and level

**Q06 — Job title as written.** *Type: text. Required.*
"What is your job title, exactly as your employer writes it?"
*Note:* free text captured verbatim and coded later against `SAL-03`. It is asked *before* the taxonomy
question so that the taxonomy options do not reshape the respondent's memory of their own title. It is
never used as the classification variable — see `SAL-01` §13, "the title trap".

**Q07 — Primary role.** *Type: single. Required.*
"Which of these best describes what you actually spend most of your time doing? Full definitions are in
the linked role guide."
Options: the ten canonical roles in `SAL-03` §3 — `Planner / scheduler` · `Cost engineer` · `Cost
controller` · `Estimator` · `Risk analyst` · `Change / commercial controller` · `Reporting and data
analyst` · `Project controls lead` · `Project controls manager` · `Head of project controls` — plus
`Other project controls role (please describe)`.
*Note:* "what you actually spend most of your time doing" rather than "your role", because in small teams
the title and the work diverge sharply. The definitions are linked rather than embedded so the question
stays readable on a phone.

**Q08 — Second role.** *Type: single, optional.*
"If your work is genuinely split between two of those, which is the second?"
*Note:* combined planner-and-cost roles are common and must not be forced into one bucket. In analysis a
respondent contributes to their primary role only; the second role is used to characterise the sample and
to flag cells where combined roles dominate.

**Q09 — Level, by behaviour.** *Type: single. Required.*
"Which of these describes your work best? Choose on what you are accountable for, not on your title."
Options: the four levelling descriptors from `SAL-03` §4, presented in full sentences describing autonomy,
the complexity of the work owned, and what the respondent's judgement is trusted with — labelled
`Foundation` · `Practitioner` · `Professional` · `Leader`.
*Note:* the descriptors are behavioural because self-rated seniority is otherwise a measure of confidence.
The labels match the Institute's competency levels (`CMP-02`), which is what allows the report to be read
alongside the competency framework. Note that "professional" here is a level on a competency scale and is
a different concept from the level at which the Institute's credentials sit.

**Q10 — Reporting line.** *Type: single.*
"Who do you report to?"
Options: `A project controls specialist` · `A project or programme manager` · `A commercial or finance
manager` · `A functional or department head` · `A director or executive` · `A client representative` ·
`Other` · `Prefer not to say`.
*Note:* a levelling cross-check that respondents cannot easily inflate, and a genuinely interesting
structural variable: whether controls reports into delivery or into finance changes the job.

**Q11 — People responsibility.** *Type: single.*
"How many people report to you directly?"
Options: `None` · `1–2` · `3–5` · `6–10` · `11–25` · `More than 25` · `Prefer not to say`.
*Note:* bands rather than a number, because exact headcount is identifying in small organisations and adds
nothing.

**Q12 — Scope owned.** *Type: single.*
"What do you have controls accountability for?"
Options: `Part of one project` · `One whole project` · `Several projects` · `A programme` · `A portfolio
or business unit` · `A functional capability across the organisation` · `Prefer not to say`.
*Note:* the strongest single discriminator between levels, and far more comparable across employers than
either title or headcount.

## 5. Block D — employment and project context

**Q13 — Employer type.** *Type: single. Required.*
Options: `Owner, operator or client organisation` · `Main contractor` · `Subcontractor or specialist
contractor` · `Engineering or design consultancy` · `Project management or controls consultancy` ·
`Government body or public authority` · `Agency or umbrella company` · `Self-employed / own company` ·
`Other`.
*Note:* the side of the table a person sits on is one of the largest structural influences on how a
controls role is paid, and it is knowable without asking for pay.

**Q14 — Employer identifier.** *Type: text, optional.*
"Optionally, your employer's name. It is used only to check that no published figure is dominated by one
organisation, and is deleted before analysis. It is never published and never reported back."
*Note:* this exists solely to make the employer-dominance test in `SAL-01` §6 rule 3 possible. It is
optional, its single purpose is stated, and the field is hashed on receipt and the plain text discarded.
A survey that cannot run the dominance test cannot detect its worst failure mode. Where a respondent
declines, the record counts towards the cell size but is treated as unknown-employer in the test.

**Q15 — Sector.** *Type: single. Required.*
Options: `Oil, gas and petrochemical` · `Power and utilities` · `Renewables` · `Nuclear` · `Mining and
metals` · `Transport and rail infrastructure` · `Water` · `Buildings and property` · `Manufacturing and
industrial` · `Pharmaceutical and life sciences` · `Technology and digital` · `Defence and aerospace` ·
`Public sector programmes` · `Financial services change` · `Other`.
*Note:* one list, cross-industry, deliberately not weighted towards the sectors project controls
historically came from. `Financial services change` and `Technology and digital` are included because
controls work exists there and is systematically missing from surveys built around heavy industry.

**Q16 — Typical project or portfolio size.** *Type: single.*
"What is the approximate capital value of the project or portfolio you work on?"
Options: `[CONFIRM: capital value bands — to be defined once, in a stated currency with a stated
conversion basis, and held constant across cycles so the variable is comparable]` plus `Don't know` ·
`Prefer not to say`.
*Note:* deliberately left unset here. Bands must be defined once, published, and never adjusted quietly,
because changing them breaks comparability while looking like a cosmetic edit.

**Q17 — Contract environment.** *Type: multi.*
Options: `Lump sum / fixed price` · `Reimbursable` · `Target cost with pain-gain` · `Framework or call-off`
· `Alliance or integrated delivery` · `Public-private partnership or concession` · `Don't know`.
*Note:* multi-select because most people work across several. It is asked because commercial model changes
what a controls role has to do, which is context the skills block needs.

## 6. Block E — contract type and working pattern

**Q18 — Engagement type.** *Type: single. Required.*
Options: `Permanent employee` · `Fixed-term employee` · `Day-rate contractor through my own company` ·
`Day-rate contractor through an umbrella company` · `Agency worker` · `Self-employed / sole trader` ·
`Secondee from another organisation` · `Other`.
Routing: drives the remuneration block. Employees route to Q26–Q31; day-rate and self-employed
respondents route to Q32–Q35.
*Note:* this is the most consequential routing question in the instrument. Asking a contractor for an
annual salary, or an employee for a day rate, produces answers that are converted by the respondent using
assumptions we never see. Each is asked in its own terms and reported separately, never merged.

**Q19 — Working time.** *Type: single. Required.*
Options: `Full time` · `Part time` · `Rotational cycle` · `Varies`.
Routing: `Part time` shows Q20.

**Q20 — Part-time proportion.** *Type: single. Routing: Q19 = Part time.*
Options: `Up to a quarter of full time` · `About half` · `About three-quarters` · `Other` ·
`Prefer not to say`.
*Note:* proportions, not hours, because contractual full-time hours differ by country and industry.
Part-time pay is reported only on a stated full-time-equivalent (FTE) basis, and the basis is disclosed.

**Q21 — Work location pattern.** *Type: single.*
Options: `Fully on site or in the office` · `Mostly on site, some remote` · `Balanced hybrid` · `Mostly
remote` · `Fully remote` · `Rotational camp or offshore`.
*Note:* a structural characteristic, asked without any implication that one pattern should pay more.

## 7. Block F — experience, qualifications and certification

**Q22 — Total experience.** *Type: single. Required.*
"How long have you worked in project controls or a closely related discipline?"
Options: `Less than 2 years` · `2–4 years` · `5–9 years` · `10–14 years` · `15–19 years` · `20–29 years` ·
`30 years or more`.
*Note:* bands, because recalled exact tenure is spuriously precise. The bands are uneven on purpose:
they are narrow where careers change fastest and wide where they do not.

**Q23 — Time in current role.** *Type: single.* Options: `Less than 1 year` · `1–2 years` · `3–5 years` ·
`6–10 years` · `More than 10 years`.

**Q24 — Highest completed qualification.** *Type: single.*
Options: `Secondary education` · `Vocational or technical qualification` · `Apprenticeship` ·
`Undergraduate degree` · `Postgraduate degree` · `Doctorate` · `Prefer not to say`.
*Note:* one neutral ladder, with no implication that any level is expected. The Institute's own
eligibility rule requires three years of professional experience in any field and no degree, and the
instrument must not contradict that in its framing.

**Q25 — Certifications held.** *Type: multi.*
"Which professional certifications do you currently hold?"
Options: `PCI PCL-AI` · `PCI PFL-AI` · `PCI PML-AI` · `PCI AIPC (AI in Project Controls — Specialist
Certificate)` · `A certification from another professional body (please name)` · `A vendor or software
certification (please name)` · `None` · `Prefer not to say`.
*Note:* two things matter here. Other bodies' certifications are captured by free text rather than a
curated list, so the instrument neither endorses nor omits anyone. And this question is about
*certification*; membership is asked separately at Q26, because a membership grade and a credential are
different ladders and conflating them is a category error the Institute is careful never to make.

**Q26 — Membership.** *Type: multi.*
"Are you a member of any professional body? Membership is separate from certification."
Options: `Yes, of PCI` · `Yes, of another body (please name)` · `No` · `Prefer not to say`.

## 8. Block G — remuneration, asked component by component

The whole block is preceded by one instruction screen:

> The next questions ask about pay. Please answer for the twelve months to the reference date, in the
> currency you are paid in, **before tax and any deductions**, and answer each component separately —
> we will not add them up for you incorrectly. Every question here can be skipped.

**Q27 — Currency.** *Type: single, searchable. Required for the block.*
"Which currency are you paid in?"
*Note:* asked before any amount, so the respondent enters a number in a field already labelled with their
own currency. Nothing is converted at entry; conversion happens once, centrally, at a single reference
date (`SAL-01` §9).

**Q28 — Basis.** *Type: single. Required for the block.*
"Are the amounts you are about to give gross (before tax) or net (after tax)?"
Options: `Gross` · `Net` · `Not sure`.
*Note:* a survey that assumes gross will silently mix in net answers from countries where net is the
number people know. Analysis uses gross only; net answers are reported as a separate count and excluded
from pay statistics rather than converted, because converting requires modelling a jurisdiction's tax and
we do not do that.

### For employees (routed from Q18)

**Q29 — Base pay.** *Type: numeric, with a period selector. Optional.*
"What is your annual base salary, excluding all allowances, bonus and overtime?"
Period selector: `per year` · `per month` · `per week` · `per hour`.
*Note:* the period selector is essential. Forcing annualisation makes the respondent do arithmetic we
cannot inspect, and it is the largest single source of unit errors (`SAL-01` §8). The exclusions are
listed inside the question rather than in a footnote, because footnotes are not read.

**Q30 — Fixed allowances.** *Type: multi with amounts, optional.*
"Do you receive any fixed allowances on top of base pay? Give the annual value of each you receive."
Categories: `Site or location allowance` · `Rotation or offshore allowance` · `Housing` · `Transport or
car` · `Travel or subsistence paid regardless of travel` · `Cost-of-living or hardship allowance` ·
`Other fixed allowance`.
*Note:* allowances are where international and rotational pay actually lives, and where they are lumped
into "salary" the resulting comparison between a site-based and an office-based respondent is nonsense.
Each is captured separately so the report can state exactly which components a figure includes.

**Q31 — Variable pay.** *Type: numeric ×2, optional.*
"If you have a bonus or incentive scheme: what is your target or on-target amount, and what were you
actually paid for the last completed period?"
Two fields: `Target` · `Actually paid`. Plus `I have no bonus scheme` · `I have one but received nothing`
· `Prefer not to say`.
*Note:* target and actual are asked separately because they diverge, and because a survey that asks only
one gets whichever one flatters the respondent. "Received nothing" is a distinct answer from "no scheme"
and from a skip; collapsing the three is how bonus data becomes uninterpretable.

**Q32 — Overtime and additional hours.** *Type: single plus optional numeric.*
"Are you paid for hours beyond your contracted hours?"
Options: `No, my pay covers all hours worked` · `Yes, at plain time` · `Yes, at an enhanced rate` · `Yes,
through time off in lieu only` · `Prefer not to say`; if paid, an optional annual amount.
*Note:* in several sectors overtime is a large and routine part of controls pay, and in others it does not
exist. A total-cash figure that silently includes it in one cell and excludes it in another is not a
comparison.

**Q33 — Employer retirement contribution.** *Type: single.*
Options: `Yes, and I know the rate` (with an optional rate field) · `Yes, but I don't know the rate` ·
`No employer contribution` · `Not applicable in my country` · `Prefer not to say`.
*Note:* `Not applicable in my country` is present because mandatory and voluntary retirement provision
differ fundamentally between jurisdictions, and an instrument that assumes one arrangement produces
missing data that looks like an absence of benefit.

**Q34 — Non-cash benefits.** *Type: multi, no amounts.*
Options: `Private medical cover` · `Life or income protection insurance` · `Company car or allowance` ·
`Additional paid leave above the statutory or contractual minimum` · `Paid professional membership or
certification fees` · `Funded training or study` · `Share or equity scheme` · `Relocation support` ·
`Childcare or family support` · `None of these` · `Prefer not to say`.
*Note:* deliberately unvalued. Valuing benefits requires assumptions about tax and local market cost that
would be invented, and an invented valuation folded into a total-reward figure is exactly the practice
this series exists to avoid. Benefits are reported as prevalence within a cell, never as money.

### For day-rate and self-employed respondents (routed from Q18)

**Q35 — Day rate.** *Type: numeric with a basis selector. Optional.*
"What is your current charge-out rate to your client or agency?"
Basis selector: `per day` · `per hour`. Plus: `Is that the rate you receive, or the rate the agency charges
the client?` — `What I receive` · `What the client is charged` · `Not sure`.
*Note:* the receive-versus-charged distinction is asked because both numbers circulate as "the rate" and
they are not the same quantity. Without it, contractor rate data is the sum of two different measurements.

**Q36 — Days worked.** *Type: single.*
"Roughly how many days did you work in the period?"
Options: banded, plus `Prefer not to say`.
*Note:* a rate is not an income. Utilisation, unpaid leave, unpaid sickness and gaps between assignments
are the difference, and without days worked no honest comparison with an employee's salary is possible.
The report presents rates and salaries as separate series and never converts between them — see
`SAL-06` §6.

**Q37 — Costs borne by the respondent.** *Type: multi.*
Options: `Own insurance` · `Own retirement provision` · `Own training` · `Own equipment or software` ·
`Accountancy or umbrella fees` · `Unpaid leave` · `None of these` · `Prefer not to say`.
*Note:* the costs an employee does not carry. Recorded so that any commentary comparing engagement types
has to acknowledge them, rather than comparing a gross rate with a gross salary and calling it a finding.

**Q38 — Rate movement.** *Type: single.*
"Compared with your previous engagement, is your current rate higher, the same, or lower?"
Options: `Higher` · `About the same` · `Lower` · `This is my first engagement` · `Prefer not to say`.
*Note:* direction only, with no magnitude. A directional item is robust to recall error; a remembered
percentage is not, and would be a change statistic the sample cannot support.

**Q39 — Pay change in the period (all respondents).** *Type: single.*
"Did your pay change during the period, and if so why?"
Options: `No change` · `Annual review or inflation-related increase` · `Promotion` · `Change of employer`
· `Change of contract or engagement type` · `Reduction` · `Prefer not to say`.
*Note:* reasons, not amounts. It supports honest commentary on how pay moves without producing a
year-on-year figure the cross-sectional design cannot support.

## 9. Block H — skills held and skills required

Two matrices over the same skill list, taken from the taxonomy in `SAL-04`. The list is presented facet by
facet — technical, tool category, domain, data and AI, behavioural — with roughly ten items per screen.

**Q40 — Skills held.** *Type: matrix.*
"For each of these, how would you describe your own capability?"
Scale: `I don't use this` · `I can do it with support` · `I can do it independently` · `I set how it is
done and others come to me for it`.
*Note:* behavioural anchors rather than `Beginner / Intermediate / Expert`. Adjectival scales measure
self-confidence, which varies systematically by group; "others come to me for it" is a claim about
observable behaviour, which is harder to inflate without noticing.

**Q41 — Skills required of you.** *Type: matrix over the same list.*
"In the last twelve months, which of these has your employer, client or a recruiter actually asked you for
— in a job specification, an objective, an appraisal or an interview?"
Scale: `Not asked` · `Mentioned` · `Explicitly required`.
*Note:* the wording is tightly bounded on purpose. It asks about a documented request in a stated window,
not about the respondent's impression of "what the market wants". This is the difference between a
measurable construct and a vibe, and `SAL-04` §6 sets out exactly what the resulting variable may and may
not be called. It is never reported as "market demand".

**Q42 — Tools used.** *Type: multi, by category, with an optional free-text field per category.*
Categories: `Planning and scheduling software` · `Cost management or control system` · `Enterprise
resource planning (ERP) system` · `Risk analysis or simulation tool` · `Business intelligence (BI) or
dashboarding tool` · `Spreadsheet as the primary control tool` · `Document control system` ·
`Programming or scripting language` · `AI assistant or large language model tool`.
*Note:* categories are the published unit; product names are collected as free text and used only for
coding. Publishing a ranked list of named products would function as a vendor endorsement, would date
within a year, and would draw attention away from the finding that matters — which is which *categories*
of capability a role is expected to have.

## 10. Block I — AI in the work

**Q43 — Use.** *Type: multi.*
"In your controls work, have you used AI tools for any of these in the last twelve months?"
Options: drawn from the data-and-AI facet of `SAL-04`: `Drafting or summarising documents` · `Analysing
or cleaning data` · `Forecasting or trend analysis` · `Schedule analysis or scenario testing` · `Risk
identification` · `Writing code, formulas or queries` · `Preparing reporting narrative` · `None of these`.

**Q44 — Sanction.** *Type: single.*
"Is that use permitted by your employer or client?"
Options: `Yes, with a policy that covers it` · `Yes, informally` · `No policy exists` · `It is prohibited`
· `Don't know`.
*Note:* the governance question that makes the usage question meaningful. Usage without permission status
is a number without a control environment attached — and the presence or absence of a policy is a finding
about employers, which is legitimate, rather than about individuals, which would not be.

**Q45 — Validation practice.** *Type: single.*
"When you use an AI tool on work that goes to a client or into a report, what happens to the output?"
Options: `I check every part of it before it is used` · `I check the parts I judge material` · `A
colleague or reviewer checks it` · `It goes through the normal review the work would have anyway` · `It
is generally used as produced` · `Not applicable`.
*Note:* this is the Institute's position — AI proposes; the professional disposes — turned into a measured
behaviour rather than an assertion. It is asked neutrally, with the least defensible option phrased
without judgement, or nobody selects it.

**Q46 — Training.** *Type: single.*
Options: `Employer-provided training on AI tools` · `Self-taught` · `Formal course or certification` ·
`No training` · `Prefer not to say`.

## 11. Block J — satisfaction, mobility and transparency

**Q47 — Satisfaction with reward.** *Type: scale, five points from `Very dissatisfied` to `Very
satisfied`, plus `Prefer not to say`.*
"How satisfied are you with your total reward for the work you do?"

**Q48 — Satisfaction with the role.** *Type: scale, same five points.*
*Note:* asked separately from Q47 because they move independently, and a single "job satisfaction" item
would conflate them.

**Q49 — Mobility.** *Type: single.*
"How likely are you to change employer or engagement in the next twelve months?"
Options: five points from `Very unlikely` to `Very likely`, plus `Already in the process` ·
`Prefer not to say`.

**Q50 — Reasons.** *Type: multi. Routing: shown if Q49 is `Likely`, `Very likely` or `Already in the
process`.*
Options: `Pay` · `Progression` · `Work content` · `Management` · `Job security or project ending` ·
`Location or travel` · `Flexibility` · `Culture` · `Relocation or personal reasons` · `Other`.
*Note:* multi-select without a forced ranking. Forcing a single primary reason produces "pay" from people
whose real reason is their manager.

**Q51 — Pay transparency at your employer.** *Type: single.*
Options: `Pay ranges are published internally` · `Ranges exist but are not shared` · `Pay is individually
negotiated with no published structure` · `Don't know` · `Prefer not to say`.
*Note:* a structural fact about the employer, asked without implying that any arrangement is required.
Pay-transparency law differs sharply between jurisdictions and the instrument takes no position on any of
them.

## 12. Block K — free text and close

**Q52 — Open question.** *Type: text, optional.*
"Is there anything about how project controls work is paid or valued that this survey has not asked
about?"
*Note:* one open question, at the end, deliberately broad. It is the instrument's own error-detection
mechanism: recurring themes here become questions in the next cycle. Responses are coded under `SAL-04`
§5 and screened for identifying detail before coding, and identifying detail is removed rather than
paraphrased.

**Q53 — Report notification.** *Type: single plus optional email field.*
"Would you like us to email you when the report is published?"
*Note:* the email address is stored separately from the response and is not joined back to it. If it were
joined, the promise of non-identification in Block A would be false.

**Q54 — Follow-up.** *Type: single.*
`[CONFIRM: whether respondents may opt in to a follow-up panel or to longitudinal linkage across cycles,
and the separate consent wording and identifier scheme that would require — pending legal review]`

**Closing screen.** Thanks, the reference for withdrawal, a link to the published methodology (`SAL-01`),
and a link to the role taxonomy (`SAL-03`) for anyone who wants to see how their answers will be
classified. No offer, no upsell, no certification marketing on the closing screen of a research
instrument.

## 13. Routing map

| From | Condition | To |
|---|---|---|
| Q01 | `No` | Exit, nothing retained |
| Q02 | `None of these`, or supplier-to-the-field option | Exit with thanks |
| Q05 | `No` | Exit with thanks |
| Q07 | Any role | Q08 |
| Q18 | Employee, fixed-term, agency or secondee | Q29–Q34, then Q39 |
| Q18 | Day rate (own company or umbrella), self-employed | Q35–Q38, then Q39 |
| Q19 | `Part time` | Q20 |
| Q49 | `Likely`, `Very likely`, `Already in the process` | Q50 |

Everything else runs in order. There are no display conditions based on a previous *pay* answer, because
adaptive pay routing lets a respondent infer what the instrument expects.

## 14. Fielding notes

**Length.** Target fifteen minutes. The remuneration and skills blocks are the ones that matter, so they
are protected; anything added in a future cycle displaces something rather than extending the survey.

**Order.** Screening, then role, then context, then pay, then skills, then attitudes. Pay sits in the
middle: early enough that the fatigued respondents have not yet dropped out, late enough that the
respondent has already invested effort and has been reminded, by the context questions, of the specifics
of their own situation.

**Mobile and save-and-resume.** A meaningful share of respondents will start on a phone. Matrices are
paged rather than scrolled sideways, and a partially completed response can be resumed from the same
device.

**Translation.** `[CONFIRM: which languages the instrument is translated into for the first cycle, and
who performs the reconciliation review of the back-translation]` — the platform's examination interface
supports several languages, but a survey translation is a separate exercise requiring back-translation
and reconciliation, because a mistranslated pay question produces confidently wrong data.

**Accessibility.** Every question is answerable with a keyboard and readable by a screen reader; no
meaning is carried by colour; matrices carry row and column headers; no question relies on a
drag-and-drop interaction.

**Piloting.** The instrument is piloted with a small group across at least three sectors and three
countries before fielding, timed, and debriefed on comprehension — particularly the level descriptors at
Q09 and the receive-versus-charged distinction at Q35. Pilot responses are excluded from the analysis
dataset.

## 15. How this goes wrong

**One pay question.** The single most common failure: "What is your salary?" and a box. The answers are a
mixture of base, total cash, and something the respondent computed in their head from a day rate. No
amount of analysis recovers the components afterwards.

**Showing a band to "help" the respondent.** A well-meaning designer adds an example or a slider with a
default position. Every answer thereafter is measured from that anchor.

**Mandatory pay questions.** Forcing an answer converts non-response into fabrication, and fabrication is
undetectable where refusal would have been visible.

**Titles as the classification variable.** Coding by Q06 instead of Q07 and Q09 would be quicker and would
produce cells that are not comparable across employers.

**Silent improvement between cycles.** A question is reworded for clarity and the series is compared
anyway. Every change goes in the change log, and a changed question breaks its own series.

**Marketing on the closing screen.** A research instrument that ends in a sales pitch teaches respondents
what the research was for, and they answer accordingly next year.

## 16. Checklist — before this instrument is fielded

- [ ] Every bracketed `CONFIRM` marker in Block A resolved by legal review in every jurisdiction of fielding
- [ ] No figure, band, default or example value appears anywhere before a respondent's own answer
- [ ] Every remuneration question offers *Prefer not to say* and none is mandatory
- [ ] Employee and day-rate routing verified end to end, in both directions
- [ ] Currency and gross/net basis asked before any amount field
- [ ] Period selectors present on every amount field
- [ ] Employer identifier hashed on receipt, plain text discarded, purpose stated at the point of asking
- [ ] Email addresses stored separately from responses, with no join key
- [ ] Role and level definitions linked and matching `SAL-03` exactly
- [ ] Skill lists match the current version of the `SAL-04` codebook
- [ ] Translations back-translated and reconciled
- [ ] Pilot completed, timed and debriefed; pilot data excluded
- [ ] Question numbers unchanged from the previous cycle wherever the wording is unchanged
- [ ] Change log updated for every wording change, however small

---

## Related

- `SAL-01 — Salary and skills report — framework and methodology` — the sampling, suppression and disclosure rules this instrument serves
- `SAL-03 — Role taxonomy and levelling` — the definitions behind Q07, Q08 and Q09
- `SAL-04 — The skills demand taxonomy` — the skill list and the coding rules for Q40, Q41, Q42 and Q52
- `SAL-05 — Report template and data tables` — where each question's output lands, and the instrument change log

## Sources and standards

No external source is cited. The design decisions above rest on the Institute's own editorial standard
(`00-framework/EDITORIAL-STANDARD.md` §4) and on the canonical facts register for credential names,
membership grades and eligibility (`00-framework/CANONICAL-FACTS.md` §1, §4.4 and §6). Where the
instrument relies on established survey-methodology practice — behavioural anchors, back-translation with
reconciliation, separating target from actual variable pay — the practice is described in our own words;
the methodology annex of the published report will cite the specific texts the research lead relies on.
The consent and privacy wording in §2 is a drafting starting point only and carries no legal review.

## Status and version

> Founding-stage document · Version 1.0 · draft · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
