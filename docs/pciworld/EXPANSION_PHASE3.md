# PCI World Expansion — Phase 3 delivery report (Gate A)

_Baseline: `main` @ `f05b4ba` (Phase 2). Scope: the Phase 3 row of EXPANSION_PHASE0.md §10 —
Gate A of the challenge-bank programme (EXPANSION_GOVERNANCE §3)._

## 1. What Gate A means, and what was actually done

Gate A is **50 flagship challenges, every one reviewed and reference-solved**. The bank went from
30 to 50: twenty new challenges, codes `WC-EVM-031` through `WC-CPM-050`.

The governance rule is that counts are never claimed before review, so the count is asserted in
CI rather than stated in prose: `Assert.Equal(50, rows.Count)` sits next to a loop that runs the
publication validator and a full reference solve over every published row. A challenge that does
not solve cannot ship, and the number cannot drift from the claim.

## 2. Coverage

| Facet | Before | After |
|---|---|---|
| Challenges | 30 | **50** |
| Deterministic engines used | 18 (9 of them once) | 18, **every one at least twice** |
| Industries | 20 | **35+** |
| Difficulty spread | thin at the ends | foundation 12 · developing 13 · professional 12 · advanced 8 · expert 5 |
| Tracks | 5 | 5 (all populated) |

The engine-coverage rule is new and enforced: no solver ships with a single worked example behind
it. That was the practical risk at 30 — nine engines had exactly one challenge, so a defect in any
of them would have been invisible to everyone except the one person who happened to play it.

New sectors include data centres, pharmaceuticals, subsea, rail, tunnelling, refining, healthcare,
space systems, water utilities, semiconductors, automotive, government, mining, logistics,
shipbuilding, education estates, telecommunications, nuclear and airports.

## 3. What makes these "flagship" rather than filler

Each challenge is a decision someone actually has to make, not a formula with a story attached:

- **The calculation is the evidence, not the point.** WC-ESC-041 asks for earned schedule because
  the classic SPI drifts toward 1.0 as a plan tails off — the arithmetic exists to show why the
  reassuring number was structurally reassuring.
- **The decisions have real trade-offs, and the wrong answers are the plausible ones.** The low-
  quality options are the things people genuinely do: smoothing a trend, netting an underspend
  against an overrun, accelerating off the critical path, dropping outliers to make a chart
  readable, re-weighting a model until the preferred project wins.
- **Every option carries a consequence and a principle.** The debrief tells you what happens next
  and what the general rule was, so a participant takes away something transferable.
- **No fabricated authority.** All data is synthetic and declared; no real project, company or
  person appears; nothing claims certification value.

## 4. Test evidence

- .NET: **741 passed / 0 failed** — every one of the 50 challenges passes `WorldContent.Validate`
  (schema, engine resolvability, ask-type ↔ solver match, tolerance sanity, answer-leakage scan,
  synthetic declaration) and a full reference solve, plus the new engine-coverage gate.
- Python integration: **1135 / 1135**.
- Playwright PCI World: **17 / 17** (archive, admin and difficulty-filter counts updated to the
  new bank).

One authoring defect was caught by the validator and fixed rather than worked around: a challenge
declared a track outside the controlled vocabulary. That is the gate doing its job — the taxonomy
in EXPANSION_GOVERNANCE §1 is a closed list, and authors pick from it rather than inventing.

## 5. Next gate

Gate B (250) opens only when Gate A's content shows zero critical content defects, completion and
report rates within the thresholds set at Gate A, a moderation queue under seven days, and
unchanged rotation/search/admin p95. Those are operational measurements over live use, not
something a code change can assert — so Gate B waits for the data, which is the point of gating.

## 6. Open decisions (unchanged)

1. **Managed MySQL 8 provider + credentials** — the launch gate.
2. Institute URL mapping for contextual links.
3. Named editorial authors/reviewers for the blog/news programme.
4. Company-logo permissions (default: none).
5. Arabic review capacity before the localization phase exits.
