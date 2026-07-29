# PR #158 — file-by-file classification and conflict map (2026-07-28)

Prepared as part of the emergency stabilization audit. **PR #158 must not be merged in its
current form**; this document records exactly what it contains, what is already on `main`, what
is stale, and how the still-useful material was (or should be) carried forward.

## 1. Commit record

| Ref | SHA |
|---|---|
| `main` at audit time | `cae8ff4fbc0b3a0fe50204ba365be3223f874ee9` |
| PR #158 head (`claude/pci-pml-pfl-ai-books-lxg5be`) | `8c97ee425b1c74966fe4ecdbad33e74ed742a0c0` |
| PR #158 base / merge-base with `main` | `e6f0970c28a2f785ef5c8842919fdeb367429792` |
| Preservation branch | `backup/pr-158-original` → `8c97ee4` |

26 commits on the PR branch beyond the merge-base; 205 changed files, +92,861 / −265.
GitHub reports the PR as **draft** and `mergeable_state: dirty` (conflicts with current `main`).
The PR head commit has no CI workflow run attached — there is no build/test evidence for the
final branch state.

## 2. Shape of the diff

| Area | Files | Nature |
|---|---|---|
| `docs/books/**` | **197** | PML-AI / PFL-AI manuscripts, verification harness, figures, question banks, glossaries — plus **4 built PDFs** (~500 pp each) |
| Outside `docs/books/` | **8** | The production boot-path repair (below) |

The 8 non-book files:

| File | PR change | Classification | Disposition |
|---|---|---|---|
| `backend/Program.cs` | Guard `new Db(...)` → exit 75; strict `ConfigIssues()` base-URL check | **Still required** (neither fix reached `main`) | **Re-implemented on the stabilization branch** against current `main`, with the base-URL rule extracted into one shared predicate (`IsPublicHttpsUrl`) used by both the preflight and `ConfigIssues()` so they cannot drift again |
| `backend/tests/production_config_test.py` | Adds `run()`/`expect()` + 3 exit-75 assertions; **but restructures `check()` in a way that drops `main`'s later `must_mention` support and its S3 fail-closed test** | **Conflicts with newer architecture** — must not be applied wholesale | New assertions **integrated additively** on the stabilization branch; every existing `main` test and helper preserved (19/19 pass) |
| `backend/tools/sqlite_to_mysql.py` | Balanced-paren table scanner | **Already present in `main`** (independently implemented); PR version adds only backtick tolerance in the `CREATE TABLE` matcher | Backtick tolerance adopted; dedicated parser tests added (`tests/schema_generator_test.py`, 12 cases); regeneration verified byte-identical |
| `render.yaml` | `CREDENTIAL_ENCRYPTION_KEY: generateValue: true` | **Already present in `main`** — `main`'s version is *newer* (also sets concrete `APP_BASE_URL`/`ALLOWED_ORIGIN`) | **Must not be applied** — no change needed |
| `.gitignore` | Adds `__pycache__/`, `*.pyc` | **Still required**, but PR version conflicts (predates `main`'s `backend/pci.db.migrate.lock` line) | Rules added on the stabilization branch as `__pycache__/` + `*.py[cod]`, preserving `main`'s entries |
| `backend/tools/__pycache__/sqlite_to_mysql.cpython-311.pyc` | **Deleted** (it was committed at the merge-base and still tracked on `main`) | **Prohibited artifact** | Untracked + deleted on the stabilization branch; `git ls-files` shows no tracked bytecode |
| `DEPLOY.md` | Blocker + exit-code tables | **Stale** — describes the era before `main`'s persistent-disk auto-posture and PCI World-only downgrades; its "MySQL is unconditionally required" framing now contradicts the code | **Rewritten from current `Program.cs`** on the stabilization branch (three postures, exit-code table, log prefixes, key preservation, Render first-provision) |
| `docs/DEPLOY_FAILURE_POSTMORTEM.md` | New postmortem document | **Documentation only** — historically useful; some conclusions (CI queued on main, render.yaml root cause) are already resolved | Optional salvage in a docs-only PR from `backup/pr-158-original`; not required for stabilization |

## 3. Conflict map vs current `main`

`git merge-tree` of `main` (cae8ff4) × PR head (8c97ee4):

- **CONFLICT**: `.gitignore`, `backend/tests/production_config_test.py`,
  `backend/tools/sqlite_to_mysql.py`, `render.yaml`
- Auto-merging (no textual conflict, but semantic review still required): `backend/Program.cs`,
  `DEPLOY.md`
- Everything under `docs/books/` is additive (no conflicts) — but 92k lines with no CI evidence
  and no human review.

## 4. Classification of the books corpus (197 files)

| Status | Files | Notes |
|---|---|---|
| Requires a separate pull request (PR B — PML-AI source) | `docs/books/pml-ai/**` source, checks, figures_src | Manuscript + verification harness; no backend coupling |
| Requires a separate pull request (PR C — PFL-AI source) | `docs/books/pfl-ai/**` source, checks, figures_src | Same isolation rules |
| Requires a separate pull request (PR B/C shared) | `docs/books/` toolchain (`build_book.py`, `verify_formulas.py`, `make_figures.py`, `run_checks.py`), `CORPUS_GATE_REPORT.md` | Shared toolchain — land with the first book PR |
| **Generated artifact** (PR D — decide storage) | `docs/books/*/build/*.pdf` (4 PDFs, two ~500 pp) | Decide: Git LFS / release artifacts / external storage. Do **not** mix into source PRs |
| Requires product/operator decision | The whole corpus' publication status | The PR's own text: AI-drafted end to end, **no human editorial or technical review**, ~700 pp/volume short of target. Nothing here may be presented to candidates/regulators as reviewed material |

## 5. Bottom line

Every production-relevant fix in PR #158 has now been carried onto the stabilization branch as
small reviewed changes against current `main` (see `EMERGENCY_STABILIZATION_2026-07-28.md`).
What remains in PR #158 is the books programme plus stale duplicates of deploy fixes `main`
already has. Recommended: close PR #158 after the book corpus is re-raised as PRs B/C/D from
`backup/pr-158-original`, cherry-picking `docs/books/` only.
