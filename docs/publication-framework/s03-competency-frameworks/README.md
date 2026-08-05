# S03 — Competency Frameworks

**10 documents · prefix `CMP` · [registry](../00-framework/ASSET-REGISTRY.md#s03--competency-frameworks-10--prefix-cmp)**

What competence looks like, by discipline and by level — and how it is evidenced rather than asserted.

## The binding constraint

The platform **already seeds** the competency sets: **PCL-AI has exactly 14, PFL-AI exactly 19, PML-AI
exactly 24**, listed verbatim in [`CANONICAL-FACTS.md`](../00-framework/CANONICAL-FACTS.md) §7 and defined
in `backend/Data/MultiCert.cs`.

This series **explains and operationalises those sets. It does not invent a parallel scheme, and it does
not change the counts.** `CMP-03`, `CMP-04` and `CMP-05` each take their credential's list and give every
competency a definition, what it looks like at each level, the evidence that demonstrates it, and the
common false positive — the thing that looks like the competency but is not.

## Three ladders that must never be conflated

This is the series where confusion is most likely, so the distinction is drawn explicitly in `CMP-02`:

| Ladder | What it measures | Values |
|---|---|---|
| **Competency level** | Demonstrated capability | foundation · practitioner · professional · leader |
| **Credential** | Examined attainment | PCL-AI, PFL-AI, PML-AI — all at **Leader** level |
| **Membership grade** | Standing in the body | SPCI · APCI · **MPCI** (requires an active certification) · FPCI (by nomination) |

Membership is not certification. A grade is a standing; a credential is earned by examination.

**Known source inconsistency:** the database records all three credentials at level `Leader`, while the
public `certification.html` renders "Level: Professional". Use **Leader**, treat the competency-scale word
*professional* as a separate concept clearly labelled, and record the conflict rather than silently
resolving it (`CANONICAL-FACTS.md` §9.1).

## Cautions specific to this series

- Never add, rename or drop a seeded competency to make a section balance.
- `CMP-10` is the document that gives the rest their teeth. A framework without evidence rules and
  moderation is a vocabulary, not an assessment — and self-assessment alone is worthless.
- Rubric wording must be observable. "Understands earned value" is not assessable; "reconstructs a control
  account's earned value from source records and identifies the cut-off error" is.

## Reading order

`CMP-01` (what the framework is for) → `CMP-02` (levels, and the three ladders) → `CMP-03`/`04`/`05` (the
credential sets) → `CMP-06`/`07`/`08` (clusters that cut across credentials) → `CMP-09` (the behavioural
competencies that decide whether a controls professional is listened to) → `CMP-10` (assessment).
