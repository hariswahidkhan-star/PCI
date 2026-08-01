# CI merge-blocker audit — 1 August 2026

## Verdict

**Current open PRs are green and mergeable.** The wave of red `frontend` checks on 31 July
was a single calendar-scheduling unit-test flake that #215 already mitigated. This follow-up
pins the test clock and hardens deadline parsing so the failure cannot return at month-end.

| Open PR | Title | CI | Mergeable |
|--------:|-------|----|-----------|
| #219 | Appendix F / books rebuild | Success | CLEAN |
| #220 | PCIOnboarding.md (detailed) | Success | CLEAN |
| #221 | PCIOnboarding.md (onboarding guide) | Success | CLEAN |

Latest `main` CI (`30658074765`, after books merge) — **all 12 jobs success**.

---

## What was failing (31 July ~17:22–17:51 UTC)

| Run | Branch | Failed job | Root cause |
|-----|--------|------------|------------|
| [30650902268](https://github.com/hariswahidkhan-star/PCI/actions/runs/30650902268) | `main` | `frontend` | `Certifications.test.tsx` booking form |
| [30652417549](https://github.com/hariswahidkhan-star/PCI/actions/runs/30652417549) | coming-soon | `frontend` | same |
| [30652442391](https://github.com/hariswahidkhan-star/PCI/actions/runs/30652442391) | books | `frontend` | same |
| [30652608843](https://github.com/hariswahidkhan-star/PCI/actions/runs/30652608843) | marketing | `frontend` | same |

**Assertion:** `expected 0 to be greater than 0` on `.schedm-day:not([disabled])`, and
`Unable to find … name "14:00"`.

**Why:** On the last day of a month, the 2-hour booking floor can leave **zero** clickable days
in the opening calendar month. The pre-#215 tests picked a day from that month and hard-coded
slot `14:00`. Secondary CI noise: `upload-artifact` then failed with “no coverage” because the
test run never produced `frontend/coverage`.

**Mitigation shipped in #215:** tests click **Next month** before picking a day.

**This cycle:** fake timers pinned to `2026-06-15T12:00:00Z`, plus an explicit late-month-end
regression case, plus safer deadline `Date.parse` (avoid `…Z` + `Z` → `NaN`).

---

## What is *not* blocking merges right now

- No queued / stuck Actions runs.
- No open PR with failing required checks.
- Branch-protection details are not readable with this token (403); operationally, recent merges
  succeeded once `frontend` went green.

**Note:** #220 and #221 both add onboarding docs — review for duplication before merging both.

---

## How to merge when you still see a red X

1. Open the PR → **Checks** → confirm you are looking at the **latest** run, not an older failure
   from before #215.
2. If the latest run is green and Mergeable = CLEAN, merge (or ask an agent with write access).
3. If an older run is red only, **Re-run failed jobs** is optional — GitHub merge uses the latest
   commit’s checks.

---

## Residual CI risk (P2)

| Risk | Severity | Status |
|------|----------|--------|
| Wall-clock / month-end scheduling unit tests | P1 → mitigated | Hardened this cycle |
| Coverage upload `if-no-files-found: error` after a failed test | P2 | Noise only; consider `warn` |
| Node 20 deprecation warnings on Actions | P3 | Informational |
| Duplicate onboarding PRs #220 / #221 | Process | Choose one |
