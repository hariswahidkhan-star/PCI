# Judging task — read this in full before scoring anything

You are judging the PCI content run at `/home/user/PCI/docs/marketing/content`. You did not write
this work. **Your job is to find what is wrong with it.** A judge who passes everything is useless.

Be hard and be specific. But do not invent faults: every REWORK verdict must quote the sentence or
name the line that is wrong. A finding you cannot point at is not a finding.

## What has already been checked mechanically — do not spend effort here

Six scripts in `_tools/` cover the countable ground, and the remediation agents verified their own
work against them. Five now report clean across all 347 pieces. **`quality_check.py` does not** — it
still holds 18 flags, most of them keyword string-matches the remediation agents judged to be false
positives and recorded as such in `reports/remediate-*.json`. Check those judgements rather than
assuming either side is right: a definition that answers the question perfectly without containing
the literal search string is correct, and so is a checker that catches a genuine miss. The scripts
cover:

- link counts, per-domain caps, all-five-domains, weak anchors, and every URL validated against the
  pages that actually exist (`link_audit.py`)
- keyword placement, title and meta lengths, heading structure, paragraph length, FAQ and table
  presence, banned phrasing (`quality_check.py`)
- `canonical` and `cta_link` URLs in front matter (`frontmatter_audit.py`)
- the four exactly-checkable claims rules (`claims_check.py`)
- anchor-text diversity per destination (`anchor_audit.py`)
- broken URLs inside the trailing notes (`note_audit.py`)

**You are here for what a script cannot decide.** That is the whole point of a judging pass.

## The five lenses, and what actually earns a low score

Score each 0–10.

### seo
Not "is the keyword present" — the script knows that. **Is this piece the best answer to its query?**
Does the H1 promise what the body delivers? Do the H2s match what a person would actually search, or
are they writerly headings that no one types? Is the length driven by the question or by a target?
Would you rank this above the page currently ranking first?

### aeo
The script checks the keyword is inside the first 60 words. **You check whether those 60 words
actually answer the question.** A sentence can contain the keyword and answer nothing. Read the
title as a question, read the first paragraph, and ask: has it been answered, or merely introduced?
Could a reader stop there and be satisfied? Is there a definition sentence that survives being
lifted out with no context around it? Are the FAQ answers real answers, or padding to fill a block?

### geo_aio
**Would a language model cite this, and would it be right to?** Does every section survive being read
alone — no "as we saw above", no dependency on a paragraph three screens up? Is there a comparison
table where the subject genuinely has axes, and is the table right? Do the statistics carry their
source? Is the entity naming consistent — "PCI AI Project Controls Leader (PCL-AI)" first, "PCL-AI"
after, never an invented variant? Is the declared schema type the one that matches the content?

### link_risk
**Scored in reverse: 10 means no scheme risk, 0 means this piece would help get the estate
devalued.** The counts are already clean, so judge the thing counting cannot reach:

- Does each link sit in a sentence that genuinely raises the question its target answers, or has one
  been dropped in to satisfy a rule? A link placed to hit a number reads as placed, and that is the
  signal the whole architecture exists to avoid. Cap at 5.
- Is the anchor text describing the destination, or describing the source's wishes?
- Does the piece link to a page on a subject its own domain owns, competing with itself? Check the
  territory table in `_LINK_ARCHITECTURE.md` section 1.
- Read the trailing note. It should now RECORD what is in the body. If it still INSTRUCTS a
  publisher to add links that would breach the caps — three to one domain, all five domains — that
  is a live defect, because someone will follow it. Cap at 4 and say exactly which instruction.

### claims
Any breach is an automatic 0 and an automatic REWORK. The script catches four patterns exactly;
**you are looking for the ones phrasing can hide**:

- A number with no source anyone could check, or a figure that sounds researched and is not.
- An implied endorsement — "trusted by", "the industry standard", "recognised across the sector" —
  that asserts standing PCI has not got.
- 15,613 quoted anywhere without PFL-AI and PML-AI named in the same sentence.
- 40/40/20 presented as anything other than the Body of Knowledge's proportions.
- Any examination weighting, pass rate, student number, salary uplift or worked-example count.
- Reproduced text from ISO, IFRS, IAS, PMI or AACE, as opposed to PCI describing them in its own
  words. Check any passage that reads like a standard's own language.
- Anything about accreditation that is not the careful position: PCI is not accredited, the scheme is
  developed with reference to ISO/IEC 17024 principles, accreditation is in progress.

Read `_BRIEF.md` section 3 for the full register before you start.

## One more thing to judge, which no script covers

**Does it read as though a person wrote it?** The banned-phrase list is checked mechanically, so the
obvious tells are gone. The subtler ones are not: every paragraph the same length, every section
opening on a rhetorical question, three-item lists used for rhythm rather than because there are
three things, a confident summary sentence that adds nothing, hedging where a practitioner would
commit. Write like someone who has run a month-end and been asked to defend a forecast — if the
piece does not, say where.

## Verdict

**REWORK** if any lens scores below 7, or if claims or link_risk is below 8. Otherwise **PASS**.

Each `must_fix` entry names the file, the line or sentence, and what it should become — specific
enough for someone to act on without reading your reasoning.

## What to write

`_tools/reports/judge-<N>.json` for your batch number N:

```json
{"batch": N,
 "files": [{"file": "...", "seo": 0, "aeo": 0, "geo_aio": 0, "link_risk": 0, "claims": 0,
            "verdict": "PASS|REWORK", "must_fix": ["..."],
            "evidence": "the sentence or line that decided the lowest score"}]}
```

Write it even if every file passes. Then reply with three lines: how many you failed, the single
worst defect you found, and anything that made you doubt your own verdict.
