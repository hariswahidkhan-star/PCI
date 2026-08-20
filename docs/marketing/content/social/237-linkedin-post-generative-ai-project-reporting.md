---
platform:      LinkedIn post
type:          linkedin-post
title:         Generative AI wrote the report. Who signs the audit trail?
meta:          Two EAC methods on the same data sit £6.7m apart. A model asked to summarise the forecast picks one and will not say which. Three controls that fix that.
primary_kw:    generative AI project reporting
secondary_kw:  EAC methods, audit trail, provenance, governed AI
pillar:        AI in project controls
credential:    PFL-AI
target_domain: pciai.org
canonical:     original
schema:        Article
word_count:    323
hashtags:      #ProjectControls #AIGovernance #ProjectFinance #CostEngineering
ab_id:         AB-00157
---

# Generative AI wrote the report. Who signs the audit trail?

**Post body (1,844 characters):**

The narrative took four minutes instead of two hours. Nobody at the review asked how fast it was written. They asked who stands behind the forecast inside it.

Generative AI in project reporting has to be split in two, because the halves carry completely different risk. A model may draft prose. It must never produce a number.

Watch what happens when it does. Budget at completion £48.0m, earned value £21.0m, actual cost £26.2m.

CPI = 21.0 ÷ 26.2 = 0.802
EAC by the CPI method = 48.0 ÷ 0.802 = £59.9m
EAC assuming the rest goes to plan = 26.2 + (48.0 − 21.0) = £53.2m

Both are legitimate. They are £6.7m apart, and the difference is entirely an assumption about whether past cost performance continues. Ask a model to "summarise the forecast" and it will pick one and not tell you which, because nothing in the prompt made the assumption a decision.

So make it one. The template names the method and the model fills a slot. Method selection is a human judgement, recorded in the forecast log with the reason.

Three controls make the rest of it auditable.

The model reads a versioned, read-only extract, never a live connection. Store the extract's hash with the output so the report can be reproduced exactly a year later.

Store the prompt and the model version alongside the pack. "We used AI" is not an audit trail. "This paragraph came from this prompt, this model version and this extract" is.

A named person signs the pack. That signature is what an auditor tests. The model is a drafting tool and cannot hold accountability, which is not a limitation to engineer around but the point.

One reason PCI is strict about this: 15,613 machine calculation checks run across the PFL-AI and PML-AI material, all passing. Arithmetic that will be examined has to be checkable by machine. Arithmetic that goes to a board deserves the same.

#ProjectControls #AIGovernance #ProjectFinance #CostEngineering

**First comment:** How to keep the audit trail intact when the report is drafted by a model, with the provenance fields to store: https://pciai.org/generative-ai-project-reporting

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [generative AI project reporting](https://pciai.org/generative-ai-project-reporting) with the anchor "keeping the audit trail when a model drafts the pack", and [the four EAC formulas](https://projectcontrolsinstitute.org/four-eac-formulas) with the anchor "what each EAC method assumes".*
