# Governance and Review

How a document moves from draft to published, what it must survive, and what happens when it is wrong.

---

## 1. Roles

| Role | Owns |
|---|---|
| Editorial coordinator | The registry, the calendar, IDs, cross-references, publication gates |
| Series lead | Coherence across a series; no duplication; consistent depth |
| Author | The draft, its sources, its arithmetic, its assumptions |
| Technical reviewer | Correctness of method and calculation; a reviewer never reviews their own draft |
| Governance reviewer | Claims, legal risk, accreditation language, jurisdiction neutrality |
| Copy editor | British English, house voice, acronyms, accessibility |

One document, one owner, one file. Two authors never edit the same manuscript concurrently.

---

## 2. Status lifecycle

```
draft ──▶ in-review ──▶ approved ──▶ published ──▶ retired
             │
             └──▶ draft   (revise-and-resubmit)
```

| Status | Means |
|---|---|
| `draft` | Authored, not yet reviewed. May contain `[CONFIRM: …]` placeholders. |
| `in-review` | Submitted with sources, assumptions and open questions attached. |
| `approved` | Passed every gate in §3. **`placeholders: 0` is mandatory.** |
| `published` | Live, and listed in `PUBLISHING-CALENDAR.md` with a date. |
| `retired` | Superseded or withdrawn. Never silently deleted — see §6. |

---

## 3. Quality gates

Every document passes all eleven before `approved`. A failure at any gate returns it to `draft`.

1. **Template conformance** — every mandatory section present, in order.
2. **Front-matter validity** — schema-valid; `related` IDs resolve; reading time within ±20 %.
3. **Factual conformance** — every name, code, fee, duration and legal statement matches
   `CANONICAL-FACTS.md`. Retired credential codes are a hard fail.
4. **Calculation verification** — every figure independently recomputed by someone other than the author.
   Numerical examples get two reviewers.
5. **Source verification** — every citation resolves to something real, with a date. A plausible-looking
   citation that was not consulted is a hard fail and a conduct matter.
6. **Originality** — no reproduction of protected standards text, tables, diagrams or question banks.
7. **Claims and legal risk** — no accreditation implied; no endorsement implied; no guaranteed outcomes;
   no jurisdiction presented as universal; no named company or project without a reliable public source.
8. **Non-duplication** — does not restate a topic another document owns.
9. **Style** — house voice; none of the prohibited moves in `EDITORIAL-STANDARD.md` §2.
10. **Accessibility** — heading nesting, table headers, figure alt text, no meaning by colour alone.
11. **LinkedIn truthfulness** — the hook is true standing alone, with no caveat withheld.

A document is never approved because it is long, or because a deadline is close.

---

## 4. What an author submits

Not just the manuscript:

- the draft;
- the sources consulted, with dates;
- the assumptions behind every worked example;
- unresolved questions for a subject-matter expert;
- a list of every `[CONFIRM: …]` placeholder and what would resolve it.

A draft submitted without its assumptions is returned unread.

---

## 5. The release gate

Before any batch is marked `published`:

```bash
# no unresolved placeholders in approved or published documents
grep -rn 'CONFIRM:' docs/publication-framework --include='*.md'

# no retired credential codes outside the canonical-facts file that documents them
grep -rn 'PCP-AI\|PFIP\|CPMD\|PDL-AI' docs/publication-framework --include='*.md'

# every document has front matter
grep -Lr '^---$' docs/publication-framework --include='*.md'
```

Each must return nothing (allowing for the documented exceptions in `CANONICAL-FACTS.md` and this file).

---

## 6. Corrections, versioning and retirement

**Versioning.** `MINOR` for a correction, clarification or added example that does not change guidance.
`MAJOR` for anything that changes what a reader should do. A major version resets the review gates.

**Corrections.** Published errors are corrected openly. The document carries a dated correction note; if
the error was material and the document was promoted on LinkedIn, the correction is posted to the same
channel. We do not quietly re-upload a fixed file and say nothing — the correction is the credibility.

**Retirement.** A retired document keeps its ID for ever. IDs are citation keys and are never reused. The
file stays in place with `status: retired` and a line naming its successor.

---

## 7. The standing risks in this programme

Named here so no reviewer has to rediscover them.

| Risk | Control |
|---|---|
| Inventing market or salary data because the document format expects a number | S08 ships as methodology and instrument; every market figure needs a named, dated source (`README.md` §4) |
| Drifting into accreditation-adjacent language | §3 gate 7; the exact permitted wording is in `CANONICAL-FACTS.md` §2.1 |
| Propagating the retired `PCP-AI` code from legacy source material | §3 gate 3; §5 grep |
| Stating an examination item count that does not exist | `CANONICAL-FACTS.md` §4.2 — no count exists; a job-task analysis will set it |
| Presenting the 30-hour CPD target as a binding requirement | `CANONICAL-FACTS.md` §4.2 |
| Conflating membership grades with certification | `CANONICAL-FACTS.md` §6 |
| A LinkedIn hook that is true only until you read the document | §3 gate 11 |
| Duplication across 100 documents by 14 hands | §3 gate 8; series leads own this |
