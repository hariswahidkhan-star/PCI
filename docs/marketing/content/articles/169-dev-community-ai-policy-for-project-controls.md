---
platform:      DEV Community
type:          template
title:         AI policy for project controls: a two-page template
meta:          An AI policy for project controls in nine clauses, with the tool register as YAML, a data classification table, and an evaluation gate with real numbers.
primary_kw:    AI policy for project controls
secondary_kw:  governed AI, AI governance, golden set evaluation, data classification
pillar:        AI in project controls
credential:    PCL-AI
target_domain: pciai.org
canonical:     canonical -> /ai-policy-for-project-controls (own site #069)
schema:        HowTo + FAQPage
word_count:    1,860
hashtags:      #ai #devops #security #testing
ab_id:         AB-00154
---

# AI policy for project controls: a two-page template

An AI policy for project controls fits on two pages. It names the approved tools, classifies data by what may be sent to them, gives every AI-assisted number a named human owner, sets an evaluation threshold before a tool goes live, and says what happens when the tool is wrong.

Anything longer will not be read, and a policy nobody reads exists to be produced during an audit rather than to change what people do. The parts that can be machine-readable should be.

## What is an AI policy for project controls?

It is the written rule set deciding three questions in advance: which tools may be used, what data may go into them, and who answers for the output.

The distinguishing feature of a controls team's version is the third question. Cost engineers and planners produce figures other people rely on financially, so accountability attaches to a person, not a process.

## What are the nine clauses?

| # | Clause | The question it settles |
|---|---|---|
| 1 | Scope | Who is bound, and which work it covers |
| 2 | Approved tools | Which systems may be used, and who adds one |
| 3 | Data classification | What may be sent, and to what |
| 4 | Human accountability | Who owns each AI-assisted output |
| 5 | Provenance | What is kept, and for how long |
| 6 | Evaluation | What a tool must demonstrate before live use |
| 7 | Disclosure | When a client or auditor is told |
| 8 | Incidents | What happens when the tool is wrong |
| 9 | Training and review | How the policy stays current |

Each clause below carries drafting text you can adapt. Replace the bracketed items and delete anything your organisation genuinely does not do.

## Clauses 1 and 2: scope and approved tools

> This policy applies to all staff and contractors producing cost, schedule, risk or reporting outputs for [organisation]. It covers any system that generates, classifies, extracts or summarises content, whether purchased, embedded in an existing tool, or accessed through a personal account.

The last phrase matters more than the rest of the clause. Most unapproved use is a competent person quietly using a free account because the approved route was slow.

> Only tools on the approved register may be used for work covered by this policy. [Named role] maintains the register, which records for each tool the supplier, deployment type, permitted data classes, date of last evaluation and named owner.

Keep the register in the repository, not in a procurement system nobody in the team can open.

```yaml
# ai-tools.yaml — reviewed at each quarterly controls governance meeting
- id: schedule-checker
  supplier: "[vendor]"
  deployment: vendor-hosted, no-training agreement signed 2026-02-11
  data_classes: [open, internal, restricted]
  owner: "[named person]"
  last_evaluation: 2026-06-30
  scores: {precision: 0.65, recall: 0.81, f1: 0.72, set: "3 closed updates"}
- id: commentary-drafter
  supplier: "[vendor]"
  deployment: vendor-hosted
  data_classes: [open, internal]
  owner: "[named person]"
  last_evaluation: 2026-05-02
  scores: {note: "drafting only; no figures sourced by the model"}
```

A register in this shape can be linted in CI: every entry needs an owner, an in-window evaluation date, and a data-class list within what the deployment permits.

## Clause 3: data classification

The single clause preventing most real incidents. Classify once, publish the table, and make it the answer to every "can I paste this?" question.

| Class | Examples | Permitted use |
|---|---|---|
| Open | Published standards, public tender notices, your own marketing material | Any approved tool |
| Internal | Method statements, generic templates, anonymised worked examples | Approved tools with a no-training agreement in place |
| Restricted | Schedule exports, cost reports, supplier pricing, claim positions | Only tools cleared for restricted data; redact client and site identifiers first |
| Personal | Names, individual rates, health, disciplinary or performance data | Not permitted without a recorded lawful basis and an assessment |
| Prohibited | Credentials, access tokens, information under a specific confidentiality undertaking | Never, in any tool, including inside a pasted log or configuration file |

> Where classification is unclear, the test is whether the content could be shown to the counterparty. If it could not, treat it as Restricted.

That test resolves grey cases faster than any decision tree, and people remember it. Enforce the Prohibited row mechanically too: a secret scanner catches tokens a policy sentence never will.

## Clauses 4 and 5: accountability and provenance

> Every AI-assisted output used in a report, forecast, claim or decision has one named owner. The owner reviews the output, understands how it was produced, and is accountable for it exactly as if they had produced it by hand. Accountability is not transferred to the supplier of the tool.

Auditors ask who. A function name is not an answer and neither is a tool name.

> For each AI-assisted output retained in a report pack, the record includes the inputs supplied, the prompt or configuration used, the tool and model version, the date, the reviewer and the owner. Records are retained for [period], matching the retention applied to the report they support.

Model versions change without the name changing. Recording the version lets you reproduce a figure challenged eight months later; pinning it in the client stops the figure moving meanwhile.

Clause 5 is easier to adopt with an example in front of it: [a worked provenance manifest for AI-drafted commentary](https://pciai.org/generative-ai-project-reporting) shows those fields filled in around a variance paragraph.

## Clause 6: evaluation before live use

> No tool is used on live work until it has been measured on a golden set of at least [30] items drawn from closed work, with precision, recall and F1 recorded, and until the review effort each threshold implies has been estimated and budgeted.

Here is what that measurement looks like. A cost-coding checker runs over a golden set of 400 historical transactions containing 50 known miscodings. It flags 70 items, of which 40 are genuine.

- Precision = 40 ÷ 70 = **0.57**
- Recall = 40 ÷ 50 = **0.80**
- F1 = 2 × (0.57 × 0.80) ÷ (0.57 + 0.80) = 0.914 ÷ 1.371 = **0.67**

Read it plainly. Thirty of the seventy flags waste a reviewer's time, ten real miscodings still get through, and at five minutes a flag the check costs roughly six hours per 400 transactions.

Now decide. If a missed miscoding costs more than six hours of review, the tool earns its place; if not, a sample check is cheaper. That is the whole argument, and it needs measured numbers rather than a vendor's claim.

> Thresholds are set per use case by the tool owner and recorded in the register. Evaluation is repeated when the model version changes, when the contract mix changes materially, or every [12] months, whichever comes first.

Teams miss the first trigger, because a silent provider update produces no event in any of your systems. Poll the version string and alert on change.

## Clause 7: disclosure

> Where an AI-assisted output forms part of a deliverable to a client, a lender or an auditor, and the contract requires disclosure of methods or processing arrangements, the reporting lead confirms the position before issue. No AI-assisted content is represented as unassisted work.

Check the contract before writing this clause. Some engineering and professional services agreements restrict processing by third parties or require named subprocessors, and a policy contradicting a live contract is worse than no policy.

## Clauses 8 and 9: incidents, training and review

> Where an AI-assisted output is found to be materially wrong, the owner reports it to [named role] within [two working days], affected outputs are identified and corrected, and the cause is recorded in the AI incident log. The log is reviewed at each [quarterly] controls governance meeting.

Confident wrongness is the characteristic failure mode of a language model and it does not announce itself. An incident log is how a team learns the shape of its own failures, and it belongs in the same tracker as everything else.

> All staff covered by this policy complete [named training] before using approved tools, including the data classification table and the evaluation method. This policy is reviewed every [12] months by [named role].

## What the policy must not say

Do not promise accuracy. A policy stating outputs "will be accurate" creates an expectation you cannot meet and undermines the review step you are mandating.

Do not describe the policy as legal compliance. It is an internal control document, and where data protection, contract or professional obligations apply, the policy points to them and your advisers interpret them.

Do not claim external endorsement the organisation does not hold. Alignment with a standard is a choice you made; certification against it is a fact somebody else establishes.

## Why a controls team needs its own version

The outputs this policy governs turn into money on somebody else's balance sheet. An engineer is examined on float and progress measurement, and almost never on cut-off or a contract asset; an accountant is examined on when revenue may be recognised, and almost never on a driving path.

A miscoded cost or an unflagged constraint crosses that boundary silently, which is why the accountability clause names a person rather than a function.

## How PCI examines this

Governance sits inside the governed AI portion of the Body of Knowledge, proportioned 40/40/20 across finance and reporting, project management, and governed AI.

The PCI AI Project Controls Leader (PCL-AI) credential has 13 domains and 61 knowledge areas, and [how the PCL-AI credential is structured and examined](https://projectcontrolsinstitute.org/pcl-ai-certification) is set out on the Institute's site. Behind the syllabus sit 113 mandatory PCI Standards carrying 532 process requirements.

PCI is an independent certifying body. This template is guidance, not legal, tax or accounting advice, and the PCI Standards are certification requirements set by the Institute rather than law.

## Frequently asked questions

**Do we need this separately from the organisation's IT policy?**
Usually a short controls-specific annex is enough rather than a competing document. The general policy handles tooling, security and acceptable use; the annex handles what a controls team does that other functions do not, which is publish figures other people rely on financially. Where the two conflict, say which wins.

**Who should own it?**
The head of project controls, rather than IT or legal. Both are consulted and both hold a veto over their own subject matter, but whoever is accountable for the numbers should be accountable for the rules governing how they are produced. Ownership outside the function produces a policy the function works around.

**How do we handle AI features inside tools we already use?**
The same way as any other tool. An embedded feature in a scheduling or finance system is in scope, needs a register entry and needs its own evaluation, because being bundled with software you already trust is not evidence of accuracy.

**Can any of this be automated?**
The register lint, the retention of the provenance record, the secret scan and the model-version alert can all run without a human. The classification decision, the evaluation threshold and the accountability assignment cannot, because each is a judgement about what an error costs you.

---

*First published on pciai.org; the `canonical_url` on this post points there. DEV prohibits stub posts, so the full template including the register schema is here.*

*Linking note — the links now in the body: "a worked provenance manifest for AI-drafted commentary" points at pciai.org/generative-ai-project-reporting from clause 5, because a provenance clause raises what a filled-in record actually looks like; "how the PCL-AI credential is structured and examined" points at projectcontrolsinstitute.org/pcl-ai-certification from the section on how PCI examines governance, because naming the credential raises what it covers and how it is taken. Two links, one per domain — the earlier note proposed three on a single host, which is the pattern to avoid. Reciprocal: the generative AI reporting how-to could point back here for the policy clauses its manifest is evidence against.*
