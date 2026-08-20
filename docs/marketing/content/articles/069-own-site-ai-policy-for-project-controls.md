---
platform:      Own site — pciai.org
type:          template
title:         AI policy for project controls: a template you can use
meta:          An AI policy for project controls in nine clauses: approved tools, data classes, named owners, evaluation thresholds and the incident route.
primary_kw:    AI policy for project controls
secondary_kw:  governed AI, AI governance, golden set evaluation, data classification
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     original
schema:        HowTo
word_count:    1692
hashtags:      n/a (own site)
ab_id:         AB-00154
---

# AI policy for project controls: a template you can use

An AI policy for project controls fits on two pages. It names the approved tools, classifies the data by what may be pasted into them, gives every AI-assisted number a named human owner, sets an evaluation threshold before a tool goes live, and says what happens when the tool is wrong.

Anything longer than two pages will not be read, and a policy nobody reads is a document that exists to be produced during an audit rather than to change what people do.

## What is an AI policy for project controls?

It is the written rule set that decides three questions in advance: which tools may be used, what data may go into them, and who answers for the output.

The distinguishing feature of a controls team's version is the third question. Cost engineers and planners produce figures that other people rely on financially, so accountability has to attach to a person, not to a process.

Which tools are worth approving at all is the prior question, and it is answered by [what AI actually does well in project controls](https://pciai.org/ai-in-project-controls) rather than by a policy clause.

## What are the nine clauses?

| # | Clause | The question it settles |
|---|---|---|
| 1 | Scope | Who is bound, and which work it covers |
| 2 | Approved tools | Which systems may be used, and who adds one |
| 3 | Data classification | What may be pasted, and into what |
| 4 | Human accountability | Who owns each AI-assisted output |
| 5 | Provenance | What is kept, and for how long |
| 6 | Evaluation | What a tool must demonstrate before live use |
| 7 | Disclosure | When a client or auditor is told |
| 8 | Incidents | What happens when the tool is wrong |
| 9 | Training and review | How the policy stays current |

Each clause below carries drafting text you can adapt. Replace the bracketed items and delete anything your organisation genuinely does not do.

## Clause 1 and 2: scope and approved tools

> This policy applies to all staff and contractors producing cost, schedule, risk or reporting outputs for [organisation]. It covers any system that generates, classifies, extracts or summarises content, whether purchased, embedded in an existing tool, or accessed through a personal account.

The last phrase matters more than the rest of the clause. Most unapproved use is a competent person quietly using a free account because the approved route was slow.

> Only tools on the approved register may be used for work covered by this policy. [Named role] maintains the register, which records for each tool the supplier, the deployment type, the data classes permitted, the date of the last evaluation and the named owner.

Keep the register to one table. A register in a procurement system nobody in the team can open is not a register.

## Clause 3: data classification

The single clause that prevents most real incidents. Classify once, publish the table, and make it the answer to every "can I paste this?" question.

| Class | Examples | Permitted use |
|---|---|---|
| Open | Published standards, public tender notices, your own marketing material | Any approved tool |
| Internal | Method statements, generic templates, anonymised worked examples | Approved tools with a no-training agreement in place |
| Restricted | Schedule exports, cost reports, supplier pricing, claim positions | Only tools cleared for restricted data; redact client and site identifiers first |
| Personal | Names, individual rates, health, disciplinary or performance data | Not permitted without a recorded lawful basis and an assessment |
| Prohibited | Credentials, access tokens, information under a specific confidentiality undertaking | Never, in any tool, including inside a pasted log or configuration file |

> Where classification is unclear, the test is whether the content could be shown to the counterparty. If it could not, treat it as Restricted.

That test resolves the grey cases faster than any decision tree, and people remember it.

## Clause 4 and 5: accountability and provenance

> Every AI-assisted output used in a report, forecast, claim or decision has one named owner. The owner reviews the output, understands how it was produced, and is accountable for it exactly as if they had produced it by hand. Accountability is not transferred to the supplier of the tool.

Auditors ask who. A function name is not an answer, and neither is a tool name, and some decisions do not move to a tool at all: [the Institute's position on which decisions a model may not make](https://projectcontrolsinstitute.org/ai-decision-policy) is published separately.

> For each AI-assisted output retained in a report pack, the record includes: the inputs supplied, the prompt or configuration used, the tool and model version, the date, the reviewer and the owner. Records are retained for [period], matching the retention applied to the report they support.

Model versions change without the name changing, and so does an unversioned prompt, which is why [how to specify and test a prompt](https://pciai.org/prompt-engineering-for-project-professionals) belongs beside this clause. Recording both is what lets you reproduce a figure that is challenged eight months later.

## Clause 6: evaluation before live use

> No tool is used on live work until it has been measured on a golden set of at least [30] items drawn from closed work, with precision, recall and F1 recorded, and until the review effort each threshold implies has been estimated and budgeted.

Here is what that measurement looks like. A cost-coding checker is run over a golden set of 400 historical transactions containing 50 known miscodings. It flags 70 items, of which 40 are genuine.

- Precision = 40 ÷ 70 = **0.57**
- Recall = 40 ÷ 50 = **0.80**
- F1 = 2 × (0.57 × 0.80) ÷ (0.57 + 0.80) = 0.914 ÷ 1.371 = **0.67**

Read it plainly. Thirty of the seventy flags waste a reviewer's time, ten real miscodings still get through, and at five minutes a flag the check costs roughly six hours per 400 transactions.

Now decide. If a missed miscoding costs more than six hours of review, the tool earns its place; if it does not, a sample check is cheaper. That is the whole argument, and it needs measured numbers rather than a vendor's claim.

> Thresholds are set per use case by the tool owner and recorded in the register. Evaluation is repeated when the model version changes, when the contract mix changes materially, or every [12] months, whichever comes first.

## Clause 7: disclosure

> Where an AI-assisted output forms part of a deliverable to a client, a lender or an auditor, and the contract requires disclosure of methods or processing arrangements, the reporting lead confirms the position before issue. No AI-assisted content is represented as unassisted work.

Check the contract before writing this clause. Some engineering and professional services agreements restrict processing by third parties or require named subprocessors, and a policy that contradicts a live contract is worse than no policy.

## Clause 8 and 9: incidents, training and review

> Where an AI-assisted output is found to be materially wrong, the owner reports it to [named role] within [two working days], the affected outputs are identified and corrected, and the cause is recorded in the AI incident log. The log is reviewed at each [quarterly] controls governance meeting.

Confident wrongness is the characteristic failure mode of a language model, and it does not announce itself. An incident log is how a team learns the shape of its own failures.

> All staff covered by this policy complete [named training] before using approved tools, including the data classification table and the evaluation method. This policy is reviewed every [12] months by [named role].

## What the policy must not say

Three things get written into AI policies that should not be there.

Do not promise accuracy. A policy that states outputs "will be accurate" creates an expectation you cannot meet and undermines the review step you are trying to mandate.

Do not describe the policy as legal compliance. It is an internal control document. Where data protection, contract or professional obligations apply, the policy points to them and your advisers interpret them.

Do not claim external endorsement the organisation does not hold. Alignment with a standard is a choice you have made; certification against it is a fact somebody else has to establish.

## How does PCI examine this?

Governance sits inside the governed AI portion of the Body of Knowledge, which is proportioned 40/40/20 across finance and reporting, project management, and governed AI. What a candidate is asked about it is set out in [how governed AI is examined](https://pciai.org/ai-project-controls-certification).

The PCI AI Project Controls Leader (PCL-AI) credential has 13 domains and 61 knowledge areas, and behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements.

PCI is an independent certifying body. This template is guidance, not legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**How long should an AI policy be?**
Two pages for the policy itself, plus the tools register and the data classification table as annexes people can consult without reading the policy again. Long policies get summarised into a slide, and the slide is what the team actually follows.

**Do we need a separate policy from the organisation's IT policy?**
Usually a short controls-specific annex is enough rather than a competing document. The general policy handles tooling, security and acceptable use; the annex handles what a controls team does that other functions do not, which is publish figures that clients, lenders and auditors rely on financially. Where the two conflict, say in the annex which one wins.

**Who should own the policy?**
The head of project controls, rather than IT or legal. Both are consulted and both hold a veto over their own subject matter, but the person accountable for the numbers should be accountable for the rules governing how those numbers are produced. Ownership that sits outside the function tends to produce a policy the function works around.

**What if someone breaches it?**
Treat a first breach as a process failure and find out why the approved route was too slow, because that is usually the answer. Repeated or deliberate breaches involving Restricted or Prohibited data are a disciplinary matter, and the policy should say so plainly.

**How do we handle AI features inside tools we already use?**
The same way as any other tool. An embedded feature in a scheduling or finance system is in scope, needs an entry in the register, and needs its own evaluation, because being bundled with software you already trust is not evidence of accuracy.

---

*Internal links: placed in the body. Three on pciai.org — the AI in project controls pillar, at the point the definition raises which tools are worth approving; prompt engineering, inside the provenance clause, because an unversioned prompt is as unreproducible as an unversioned model; and AI project controls certification, where the piece says how governance is examined. One cross-estate link, to the hub's AI decision policy, in the accountability clause, since a reader asking "who owns this output" next asks which decisions cannot be delegated at all. No second hub link was added: the accountability clause is the only sentence here that genuinely raises a hub question. Reciprocal: the LLM schedule review how-to links here from its confidentiality and overreach section, which is where a reader is sent to write the rule down.*
