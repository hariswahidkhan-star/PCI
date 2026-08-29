---
platform:      LinkedIn Article
type:          data-study
title:         AI skills in project controls: what employers ask for
meta:          The AI skills in project controls employers now test, with worked EAC arithmetic showing how a plausible generated forecast understated exposure by £2.79m.
primary_kw:    AI skills in project controls
secondary_kw:  AI governance for project teams, verification of AI output, estimate at completion methods, project controls hiring
pillar:        AI in project controls
credential:    suite
target_domain: pciai.org
canonical:     original
schema:        Article + FAQPage
word_count:    1,661
hashtags:      #ProjectControls #AIGovernance #CostEngineering #Scheduling #ProjectManagement
ab_id:         AB-00100
---

# AI skills in project controls: what employers ask for

Employers are not asking for prompt engineering. The AI skills in project controls that appear in serious requirements are verification skills: can you check a generated forecast against the ledger, name what the method assumed, and refuse a number you cannot source? Tool literacy is assumed. Scrutiny is what gets paid for.

Written for LinkedIn as an original. It sits under the Institute's AI in project controls pillar.

## Why this piece carries no survey percentage

Because we have not run a countable sample of job advertisements and shown you the method, and a percentage without a method is decoration.

Plenty of numbers are circulating about AI adoption in the professions. Most of them trace back to self-selected surveys with unstated sampling, and a certifying body that repeats them has spent credibility it cannot recover.

What follows instead is the structure of the requirement, read off the tasks that are actually changing, with the arithmetic that a competent candidate should be able to do. That is testable, which a percentage is not.

## Which AI skills in project controls do employers ask for?

Six clusters, and only one of them is about operating a tool.

| Skill cluster | What the employer is really testing | What satisfies it |
|---|---|---|
| Verification of generated output | Whether you can find a wrong number inside a plausible report | A named order of checks you can run inside an hour |
| Data literacy | Whether you read a distribution rather than a mean | P50 against P80, sample sizes, error rates |
| Assumption reading | Whether you know what a forecast method assumes | Naming the EAC method and its assumption without being asked |
| Governance and records | Whether AI-assisted output survives an audit | Retained source, prompt, reviewer and date for every generated figure |
| Confidentiality discipline | Whether project data ends up somewhere it should not | Knowing what may be put into a hosted tool and what may not |
| Tool operation | Assumed rather than tested | Working fluency. No certificate required |

The first cluster carries most of the value and appears in almost no training course. It is also the one that separates a candidate in an interview inside five minutes.

## Can you catch a wrong number in a generated report?

This is the test, and it is easy to set. Hand a candidate a generated forecast and ask them what it assumed.

Take a package with BAC £18.0m. At the data date, PV = £7.00m, EV = £6.30m, AC = £7.80m.

CPI = 6.30 ÷ 7.80 = **0.808**. SPI = 6.30 ÷ 7.00 = **0.90**. Remaining work, BAC − EV, is **£11.70m**.

The generated report says: *estimate at completion £19.50m, forecast overrun £1.50m, cost performance below plan but recoverable.*

The arithmetic is correct. EAC = AC + (BAC − EV) = 7.80 + 11.70 = £19.50m. That is the first of [the four standard EAC methods](https://projectcontrolsinstitute.org/four-eac-formulas).

What the report did not say is what that method assumes: that the £1.50m variance to date was a one-off, and that all remaining work runs exactly to budget. Nobody has asserted either.

| Method | Formula | Assumption | EAC | VAC |
|---|---|---|---|---|
| 1 | AC + (BAC − EV) | The overrun was a one-off event, now closed | **£19.50m** | −£1.50m |
| 2 | BAC ÷ CPI | Remaining work runs at cumulative cost performance to date | **£22.29m** | −£4.29m |
| 3 | AC + (BAC − EV) ÷ CPI | Identical to method 2 once the algebra is done | **£22.29m** | −£4.29m |
| 4 | AC + (BAC − EV) ÷ (CPI × SPI) | Schedule pressure costs money as well as time | **£23.90m** | −£5.90m |

The difference between what was reported and the index-based view is **£2.79m** on one package, and the report was not wrong. It was unlabelled.

A candidate who spots that, names the method, and asks what evidence supports the one-off assumption has demonstrated the skill that matters. A candidate who checks whether 7.80 + 11.70 equals 19.50 has demonstrated a calculator.

Methods 2 and 3 producing the same figure is not a coincidence, incidentally. BAC ÷ CPI equals BAC × AC ÷ EV, and AC + (BAC − EV) ÷ CPI collapses to the same expression.

## What evidence satisfies an employer?

Claims about AI on a CV are currently worth close to nothing, because everyone makes them. Evidence is what converts them.

| Claim | What it proves alone | What turns it into evidence |
|---|---|---|
| "Used AI to speed up reporting" | Nothing | A before-and-after with a named check that caught a specific error |
| "Prompt engineering" | Very little | A written protocol another person can follow and get the same result |
| "Familiar with AI scheduling tools" | Tool exposure | A recall figure measured against a sample you checked by hand |
| "AI governance experience" | Nothing without a document | A one-page team policy covering retention, review and escalation |
| "Data-driven forecasting" | A phrase | The four EAC methods, what each assumes, and which one you signed |

The pattern is that evidence is always something a third party could repeat. That is also the definition of a control, which is why this is a project controls skill rather than a technology skill.

## Where does governed AI sit in a body of knowledge?

Alongside the finance and the delivery content rather than bolted on to the end of it, because in practice the three interfere with each other.

The Body of Knowledge's proportions across PCI's credentials are 40% finance and reporting, 40% project management, and 20% governed AI. A model that produces a forecast is producing an accounting input, so the governance content cannot sit apart from the finance content.

| Credential | Full name | Domains | Knowledge areas |
|---|---|---|---|
| PCL-AI | PCI AI Project Controls Leader | 13 | 61 |
| PFL-AI | PCI AI Project Finance Leader | 16 | 61 |
| PML-AI | PCI Project Management Leader – AI | 16 | 63 |

Underneath the credentials sit 113 mandatory PCI Standards carrying 532 process requirements, which are certification requirements established by the Institute rather than law.

On the calculation side, 15,613 machine calculation checks have been run and all pass, and that suite covers PFL-AI and PML-AI only. PCL-AI has no equivalent suite, so the figure should never be quoted as though it covered all three.

That last paragraph is itself the skill this article is about. A number, its scope, and what it does not cover, in the same sentence.

## What would you do in the next ninety days?

Pick one report you produce monthly and write the check list for it. Five to eight checks, in order, each one a statement that can be true or false.

Then run those checks against a generated version of the same report and record what they caught. That record is the strongest single item you can take to an interview, and almost nobody has one.

Third, write down [the rule for what project data may go into a hosted tool](https://pciai.org/ai-policy-for-project-controls). One page, agreed with whoever owns information security. Teams that skip this find out about it during an incident.

The PCI AI Project Controls Leader (PCL-AI) credential examines this territory across 13 domains and 61 knowledge areas, and the finance-side equivalent sits in PFL-AI.

## Frequently asked questions

**Which AI skill has the highest return for a project controls professional?**
Verification of generated output. It is scarce, it is demonstrable in an interview, and it becomes more valuable as the tools improve, because better tools produce more plausible errors rather than fewer. Every other skill on the list depends on it, since none of the outputs can be used until somebody has checked them.

**Do I need to learn Python to work with AI in project controls?**
No, though basic data handling helps. The skills that appear in real requirements are statistical literacy and structured checking rather than programming. If you want a technical skill, learn to interrogate a dataset well enough to say what its sample is and where its errors concentrate, which is more useful on a project than writing scripts.

**Are AI certifications worth taking?**
It depends entirely on whether the certification examines judgement or tool operation. A credential that tests whether you can drive a specific product ages with the product. A credential that tests whether you can govern a forecast, name a method's assumptions and defend a number under questioning does not, because those requirements outlive any tool.

**How should a team record AI-assisted work for audit?**
Retain four things against every generated figure: the source data, the instruction given, the reviewer, and the date. That is enough for a third party to reconstruct how the number was produced. Treat a generated figure as unverified until a named person has checked it, in the same way an uncertified valuation is not a receivable.

**What should a hiring manager actually ask at interview?**
Hand over a one-page forecast with a labelled EAC and ask which method produced it and what that method assumes. Then ask what evidence would change the answer. It takes five minutes, it cannot be prepared for from a course, and it separates people who use tools from people who can be accountable for what the tools produce.

---

*PCI publishes certification requirements. Nothing here is legal, tax or accounting advice, and no claim is made about employment outcomes, salaries or the number of people holding any credential.*

*Written for LinkedIn as an original. LinkedIn supports no canonical tag, so this piece is not a copy of anything on the PCI site.*

*Internal links: two links are in the body, on two different domains. "The rule for what project data may go into a hosted tool" points to https://pciai.org/ai-policy-for-project-controls, in the ninety-day list where the reader is told to write that rule down and will want a template. "The four standard EAC methods" points to https://projectcontrolsinstitute.org/four-eac-formulas, where the generated report's method is named. The standfirst pillar link and the PCL-AI link were removed: the piece carried four links, three of them to one domain, which is the density the link-spam policy names. Reciprocal: https://pciai.org/ai-project-controls-certification could cite this piece for the interview test that separates verification from calculation.*
