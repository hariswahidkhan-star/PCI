# S10 — Free Templates

**16 documents · prefix `TPL` · [registry](../00-framework/ASSET-REGISTRY.md#s10--free-templates-16--prefix-tpl)**

Instruments a practitioner can use on Monday morning. In this series the template **is** the deliverable:
a short instructional preamble, then the instrument, then completion notes and a worked fragment.

## The relationship with the platform's existing CSV library

The platform already publishes **25 CSV templates** (`backend/Data/TemplatesLibrarySeed.cs`). These
sixteen documents do not duplicate them and must not be allowed to drift into doing so.

**The CSVs are instruments. These documents are the method.**

The CSV library deliberately ships **without embedded formulas**: every field is neutralised against
spreadsheet-formula injection, so a template carrying live formulas would be indistinguishable from an
attack payload. Each CSV therefore carries columns, example rows and a brief "how to compute" note.

That constraint is exactly the gap this series fills — the formula stated in words *and* as a spreadsheet
expression, field-by-field completion instructions, a worked fragment, the common mistakes, and the rules
for adapting the instrument safely. None of that fits in a CSV.

Nine of these sixteen pair with an existing CSV; seven are genuinely new because they are forms, plans,
narrative skeletons or assessment instruments that do not reduce to a grid. The full mapping — including
the sixteen CSVs with no counterpart here, which are a candidate future series and **not** to be
absorbed into this one — is in [`PLATFORM-INTEGRATION.md`](../00-framework/PLATFORM-INTEGRATION.md) §3.

## Cautions specific to this series

- **Every formula must be verified before it is written.** This is the most embarrassing place in the
  framework to be wrong, because the reader will actually run it. Spreadsheet expressions guard against
  division by zero.
- Templates must work both as plain Markdown and pasted into a spreadsheet.
- These are **original instruments**. Do not reproduce any third-party template, form or checklist. Where
  a threshold is convention rather than rule — `TPL-14` in particular — label it as convention to be
  agreed, and do not attribute it to a named published standard.
- `TPL-13` (claim and extension-of-time narrative) is jurisdiction-neutral and carries a plain
  "this is a structure, not legal advice" line. Entitlement depends on the contract and the governing law.
- Four templates are gated (`TPL-06`, `TPL-14`, `TPL-15`, alongside `CER-01` and `SAL-05`); the rest are
  free and ungated. Teaching is never gated.

## Pairing with the guides

Every template has a best-practice guide behind it in S09, and the calendar ships the guide first — the
method earns the instrument (`PUBLISHING-CALENDAR.md` sequencing rule 1).
