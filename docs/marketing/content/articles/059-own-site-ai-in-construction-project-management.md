---
platform:      Own site — pciai.org
type:          guide
title:         AI in construction project management: an honest guide
meta:          Where AI in construction project management helps, where it quietly breaks the monthly numbers, and the progress-percentage error that costs real margin.
primary_kw:    AI in construction project management
secondary_kw:  IFRS 15 five-step model, cost-to-cost input method, progress measurement, PML-AI
pillar:        AI in project controls
credential:    PML-AI
target_domain: pciai.org
canonical:     original
schema:        Article + FAQPage
word_count:    1569
hashtags:      n/a (own site)
ab_id:         AB-00041
---

# AI in construction project management: an honest guide

AI in construction project management is now routine in four places: reading and routing documents, estimating from historical data, measuring physical progress from images and sensors, and drafting reports. Each saves real time. Each also produces a number that can be carried into the monthly accounts incorrectly, and that is where the money goes.

This guide covers both halves — the applications that work, and the one arithmetic error that turns a useful tool into an overstated margin.

## Where AI in construction project management is actually used

The table below reflects functions where deployment is common on large projects, and what each application still requires from a person.

| Function | What the tool does | What it cannot settle |
|---|---|---|
| Estimating and preconstruction | Prices from historical cost data, flags scope items missing against comparable jobs | Whether this job's conditions resemble the ones in the history |
| Document control | Classifies and routes RFIs, submittals and correspondence; extracts dates and obligations | The commercial meaning of a clause under dispute |
| Progress measurement | Reads site imagery, scans and sensor data into a physical percentage complete | Which definition of "complete" the number is answering |
| Safety | Detects unsafe conditions in images and near-miss patterns in reports | The decision to stop work, which is a person's to make |
| Commercial | Compares variation orders against contract terms and prior instructions | Entitlement, which is a legal and commercial judgement |
| Cost and forecasting | Runs every forecasting method and tests each against project history | Which method to publish, because that is a claim about cause |
| Reporting | Drafts commentary in the house format, in minutes | Ownership of the number in front of the client |

Nothing in the right-hand column becomes automatable by buying a better product. Those are decisions with consequences attached, and consequences need an owner. The same split runs through the rest of the discipline, and [where AI helps and where it misleads across project controls](https://pciai.org/ai-in-project-controls) follows the same line between structure and judgement.

## The percentage problem

A construction project has at least three different percentages complete, and AI progress tools are very good at producing exactly one of them.

**Physical progress** is how much of the work exists on site. **Earned progress** is budgeted value earned against an agreed earning rule. **Cost-to-cost progress** is costs incurred as a share of total expected costs, and it is the one that commonly drives revenue under the input method.

They are not interchangeable, and on a job with front-loaded costs — piling, temporary works, off-site fabrication — they can differ by ten points or more at the same data date.

## The five-step model, in PCI's own words

Revenue on a construction contract is not recognised because a valuation was certified. It follows a five-step sequence, described here in plain terms rather than reproduced from the standard, and worked through at length in [the five-step model applied to a construction contract](https://projectcontrolsinstitute.org/ifrs-15-for-construction).

1. **Identify the contract.** A contract with enforceable rights and obligations, where consideration is probable.

2. **Identify the performance obligations.** The distinct promises in it. On many construction contracts the whole works is one obligation; on some it is not.

3. **Determine the transaction price.** Including variable consideration such as incentives, claims and liquidated damages, constrained so that a significant reversal is not likely.

4. **Allocate the price** across the identified obligations.

5. **Recognise revenue as each obligation is satisfied**, over time where the criteria are met, using a method that faithfully depicts progress.

Step five is where the AI-generated percentage arrives, and where it can go wrong. This is a description of the mechanism, not accounting advice; nothing PCI publishes is legal, tax or accounting advice.

## The arithmetic

Take a contract with a transaction price of **£40m** and expected total costs of **£32m**. Costs incurred to date are **£12m**.

Straightforward cost-to-cost: progress = 12 ÷ 32 = **37.5%**. Revenue = 37.5% × 40 = **£15.0m**. Against £12m of cost, that is **£3.0m** of margin.

Now the detail. Of the £12m incurred, **£2m** is structural steel delivered to site and not yet installed. Costs that do not depict progress are excluded from the measure, and such materials are commonly recognised at cost with no margin.

Recompute: progress = (12 − 2) ÷ (32 − 2) = 10 ÷ 30 = **33.3%**. Revenue on progress = 33.3% × (40 − 2) = **£12.67m**, plus the £2m of materials at cost, giving **£14.67m**. Margin is 14.67 − 12 = **£2.67m**.

The two answers differ by **£0.33m of margin in a single month**, on the same site, the same costs and the same contract.

Now add the tool. A progress engine reading drone imagery reports the structure as **45% complete**, which may be perfectly accurate as a physical measure. Carried into revenue as though it were the progress measure, it gives 45% × 40 = **£18.0m** and a margin of £6.0m.

That is **£3.33m of margin above** the properly measured figure, produced by a tool doing exactly what it was sold to do. Nobody lied. The number simply answered a different question from the one the ledger asked.

## The test to run before you buy

Ask the vendor which of the three percentages their output is, and what it excludes. A clear answer — "physical completion of installed permanent works, excluding materials on site" — is a good sign.

Ask who maps their output to the earning rules in your control accounts, and whether that mapping is documented. If the answer is that the finance team will "work it out", you have bought a dispute.

Ask what happens at cut-off. A progress reading taken three days after the accounting cut-off is not wrong, it is just unusable, and the difference is not visible in the report.

Ask for the evaluation. Precision and recall on your own data, against outcomes you already know, with the review time priced in. A tool nobody has scored is an assumption you are carrying at full value.

## What this means for the project manager's job

The tasks that compress are the ones a project manager was probably doing at eleven at night: chasing status, reconciling versions, drafting a pack. That is a genuine gain and it should be taken.

The tasks that grow are the ones the tools create demand for. More flags need dispositioning, more model outputs need challenging, and more numbers arrive with a plausible face and no provenance.

The skill that becomes decisive is the ability to read a number and know which question it answers. That has always been a controls skill; AI has made it a delivery skill too, because the outputs now arrive directly on the project manager's desk without passing through anyone who was trained to check them. Whether that adds up to a shrinking role is a fair question, and [what stays with the project manager once the assembly work goes](https://pciai.org/will-ai-replace-project-managers) is the longer answer to it.

## How PCI examines this

PCI certifies delivery leadership through the PCI Project Management Leader – AI (PML-AI), which holds 16 domains and 63 knowledge areas. The controls-side credential, the PCI AI Project Controls Leader (PCL-AI), holds 13 domains and 61 knowledge areas, with a Body of Knowledge proportioned 40/40/20 across project accounting and finance, project management principles and governed AI.

Both examine the crossing described above: the point where a delivery measurement becomes a reported number. The calculation content of the PFL-AI and PML-AI volumes is verified by 15,613 machine calculation checks, all passing; PCL-AI has no equivalent suite. Across the three volumes there are 92 sector case studies (26 + 33 + 33).

PCI is an independent certifying body and claims no accreditation, endorsement, affiliation or equivalence with any other organisation.

## Frequently asked questions

**Is AI reliable enough to measure progress for payment applications?**
For physical progress on repetitive, visible work it is often more consistent than manual assessment. Whether it is reliable enough for a payment application depends on the contract's measurement provisions, which usually specify how quantities are agreed. The technology does not change what the contract says.

**Can AI read our contract and tell us our entitlement?**
It can find the clauses, the notice periods and the prior correspondence quickly, which is a large part of the work. Entitlement itself is a legal and commercial judgement about facts, and a model's confident summary of a disputed clause is a liability rather than an opinion.

**Where does AI fail most expensively on construction projects?**
In the handover between measurement and reporting. A tool that produces a defensible physical percentage, mapped without thought onto a revenue measure, moves margin in the accounts without anyone deciding to move it. That failure is silent, monthly and cumulative.

**Do we need new controls to use these tools?**
Not new in kind. Provenance for every AI-assisted number, a measured baseline on your own data, a review step that is funded, and a named owner. Those are the same four controls you should already have for a spreadsheet nobody understands, and they fit on one page — [an AI policy for project controls you can adapt](https://pciai.org/ai-policy-for-project-controls) sets out the wording.

**Should a project manager learn the accounting side?**
Enough of it to know which percentage the report is using and what the earning rule is. A delivery lead who can ask "is this cost-to-cost or physical?" catches the error above in one question, and that question takes ten seconds.

---

*Internal links: now placed in the body. Same-domain: "where AI helps and where it misleads across project controls" follows the application table, where the split between structure and judgement is stated; "what stays with the project manager once the assembly work goes" closes the section on the changing role, which raises exactly that question; "an AI policy for project controls you can adapt" answers the FAQ asking whether new controls are needed. One cross-estate link only, to the hub: "the five-step model applied to a construction contract" where the five steps are introduced in PCI's own words and a reader wants the full treatment. Reciprocal: the project-manager piece should point back here for the progress-percentage arithmetic.*
