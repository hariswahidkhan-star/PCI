# S08 — Salary and Skills Report

**6 documents · prefix `SAL` · [registry](../00-framework/ASSET-REGISTRY.md#s08--salary-and-skills-report-6--prefix-sal)**

## This series contains no salary figures, and that is the point

The Institute has not run the survey. Publishing invented numbers — or numbers scraped from job adverts
and dressed as research — would destroy the credibility the other ninety-four documents exist to build.

So S08 ships as **the instrument and the methodology**: everything needed to run an honest annual report,
plus a report template whose data tables are structurally complete and deliberately empty, awaiting field
data. Every cell reads `[DATA]`, and every table is labelled *structure only — no data collected*.

A professional body that publishes its methodology before it publishes a number is demonstrating exactly
the discipline it certifies. `SAL-01` makes that argument openly rather than apologising for it.

## Cautions specific to this series

- **Absolutely prohibited anywhere in S08:** any salary figure, range, median, percentile, currency
  amount, year-on-year percentage, demand statistic, vacancy count, adoption rate or "typical" pay — not
  even as an illustrative example. `00-framework/validate.sh` gate 11 enforces this.
- The same prohibition binds series S07 (career roadmap), which is where the temptation actually shows up.
  Career documents cross-reference `SAL-06` instead of estimating.
- `SAL-05` must never ship before `SAL-01`. A report template released without its methodology invites
  precisely the misreading this series exists to prevent — see `PUBLISHING-CALENDAR.md` sequencing rule 3.
- Consent, privacy and data-handling wording carries a `[CONFIRM: …]` until a legal review resolves it.

## The methodological commitments worth keeping

A minimum cell size below which no figure is ever published. Response counts disclosed alongside every
number. Self-selection bias named rather than buried. Currency normalisation with an explicit
purchasing-power caveat. And a stated list of what the report will never claim.

## Reading order

`SAL-01` (why and how) → `SAL-03` and `SAL-04` (the taxonomies that make answers comparable) → `SAL-02`
(the instrument itself) → `SAL-05` (the shell the data lands in) → `SAL-06` (how a reader should use the
result without misrepresenting it).
