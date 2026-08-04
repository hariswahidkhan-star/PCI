# Document Template

The canonical skeleton. Every one of the 100 documents uses it. Section order is fixed; a section may be
omitted only where the notes below say it may.

---

## The skeleton

````markdown
---
(front matter per FRONT-MATTER-SCHEMA.md)
---

# {Title}

> {Subtitle — one line that says what this is, not why it matters.}

**In one paragraph.** Plain-language statement of what the reader gets and what they will be able to do
afterwards. Written so that someone who reads only this paragraph is not misled. No marketing.

**Who this is for.** One or two sentences naming the actual roles.

---

## 1. {The substantive opening}

Open on the thing itself. No throat-clearing, no "in today's environment", no restating the title.
The strongest opening is usually the problem the reader already has, stated more precisely than they
would state it themselves.

## 2. …{body sections}…

Numbered, so clauses can be cited: *"see §4.2"*.

## N. How this goes wrong

**Required in every S02, S03, S07 and S09 document.** The failure modes, stated concretely. This is
the section practitioners screenshot, and it is the one that proves the document was written by someone
who has seen the work done badly.

## N+1. Worked example

**Required in every S09 document, and wherever a method involves arithmetic.**

Label it *Illustrative figures.* Show the substitution, not just the result. State units, period,
currency basis, rounding, and every assumption the answer depends on.

## N+2. Checklist

A genuinely usable list — the thing a reader can take into a meeting. Not a summary of the document
in bullet form.

---

## Related

- `ID — Title` — one clause on why they should go there next
- `ID — Title` — …

## Sources and standards

Named, dated, verifiable. Standards are named and explained in our own words, never reproduced.
An empty list is honest; a fabricated entry is a rejection defect.

## Status and version

> Founding-stage document · Version {x.y} · {status} · Reviewed under PCI governance.
> PCI makes no claims of accreditation or recognition beyond what is true today.
````

---

## Section notes

| Section | Rule |
|---|---|
| **In one paragraph** | Mandatory everywhere. Reused verbatim as `summary:` in front matter, the Downloads Centre description and the LinkedIn blurb. Write it last, once you know what the document actually says. |
| **Who this is for** | Mandatory. Name roles — "cost engineers and control account managers" — not segments. |
| **Numbered sections** | Mandatory. Governance documents (S04, S05, S06) number to clause depth so they can be cited in a decision. |
| **How this goes wrong** | Mandatory in S02, S03, S07, S09. Optional but encouraged elsewhere. |
| **Worked example** | Mandatory in S09 and anywhere arithmetic appears. Every figure independently verified. |
| **Checklist** | Mandatory in S09 and S10. Optional elsewhere. |
| **Related** | Mandatory, minimum two entries, IDs must exist in the registry. |
| **Sources and standards** | Mandatory heading; may legitimately be empty. |
| **Status and version** | Mandatory, verbatim footer stamp. |

---

## Template-series variant (S10)

Templates invert the shape: the deliverable is the instrument, not the essay.

````markdown
---
(front matter)
---

# {Template name}

> {What it produces.}

**In one paragraph.** …

**Who this is for.** …

## 1. When to use this
## 2. How to complete it
Field-by-field instructions, in the order the user meets them.
## 3. The template
The instrument itself — table, form or structure, ready to copy. Every column defined.
## 4. Worked fragment
A partially completed example. *Illustrative figures.*
## 5. Common mistakes
## 6. Adapting it
What you may safely change, and what you must not.

## Related / Sources and standards / Status and version
````

Templates must be usable as plain Markdown or pasted into a spreadsheet. Where a column is calculated,
state the formula in words **and** as a spreadsheet expression, and verify it.

---

## What "detailed" means here

Detail is *specificity*, not length. A document is detailed when it:

- names the artefact, the field, the frequency and the owner rather than "the relevant documentation";
- gives the number and where it comes from rather than "appropriate contingency";
- says what to do when the ideal is unavailable, because it usually is;
- distinguishes the rule from the judgement, and says which is which;
- survives a reader asking "yes, but how?" three times in a row.

Padding a document to look thorough is a rejection defect. So is a document that answers "what" and
never "how".
