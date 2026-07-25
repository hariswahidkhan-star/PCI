# Simulation Lab — UI audit and premium redesign notes

Audit of the Simulation Lab surfaces (student catalogue `/app/lab`, workspace
`/app/lab/:code`, Admin Studio `/admin/lab`) taken before the premium UI pass,
with the design decisions the pass implements. Baseline: 211 unit tests green;
student bundle 467.25 kB JS / 23.80 kB CSS (gz 157.01 / 6.21).

## What exists and is reused

- **Design language.** `frontend/src/styles.css` — navy plate chrome, blue-tinted
  depth, semantic-ish palette (`--brand/--ink/--ok/--warn/--err`), 12 px radius,
  `fade-stagger` motion, global `prefers-reduced-motion` kill-switch, RTL rules.
  All reused; the SimLab layer adds `--sl-*` tokens on top rather than replacing.
- **Primitives.** `components/ui.tsx` (`Card`, `Badge`, `StatusBadge`, `Spinner`,
  `Empty`, `ErrorNote`, `Stat`) — reused everywhere; not duplicated.
- **Charts.** Hand-authored accessible SVGs (`SCurve`, `Gantt`, `Histogram`) with
  `role="img"` + descriptive `aria-label` — upgraded in place, no chart library.
- **Data/API.** Access gate, catalogue, mastery, attempts, autosave, coach and
  admin governance endpoints — untouched. All data stays dynamic.

## Defects and friction found

**Content hierarchy**
- Catalogue: every block is an equal-weight `Card`; the page reads as a stack of
  identical boxes. No at-a-glance progress (labs done / in progress / avg score).
- No "continue where you left off" surface even though `attempt_status`
  (`in_progress`) is already in the catalogue payload.
- Runner: Brief, Answers and Coach all render with equal visual weight; the
  coach is buried inside the answers card; the grade is a plain table with the
  score hidden in a card title.
- Admin Studio: stats, create form, validation and table compete visually; row
  actions are a wall of identical small buttons.

**Interaction friction**
- Mode switch (Training ↔ Assessment) is a bare `<select>` that silently
  restarts the attempt — work-loss risk with no confirmation.
- Autosave state is a transient text fragment, easy to miss; no persistent
  save indicator.
- Catalogue filters: seven controls, no active-filter summary, no clear-all,
  result count not announced to screen readers.
- Admin: no search/filter over the scenario table (96+ rows); governance dates
  edited via `window.prompt`.

**Accessibility**
- Result count and coach responses not announced (`aria-live` missing; save
  notes had `role="status"` but coach/grade did not).
- Charts have good `aria-label`s but no data-table alternative.
- Series in `SCurve` differ by colour only (colour-blind unsafe).
- Hint-level select gives no sense of progression; disabled "Open lab" button
  relies on `title` alone for its explanation.

**Responsive**
- Runner is one column at all widths; on wide screens the answer inputs float
  in space and the coach pushes the grade below the fold.
- Filter toolbar wraps raggedly at tablet widths.

## Redesign implemented (this pass)

- **Tokens** (`simlab.css`): semantic `--sl-*` layer — surface, elevated surface,
  border, text, muted, accent, success, warning, danger, info, focus, plus chart
  tokens (baseline / earned / actual, critical path, float, grid) aligned with
  the existing PCI palette. Logical properties for RTL; 4/8-based spacing;
  tabular numerals for all figures.
- **Primitives** (`components/simlab.tsx`, shared student + admin, tested):
  `LabHeader`, `KpiRow`, `SegmentedControl` (native radios), `Chip`,
  `SaveIndicator`, `ConfirmDialog` (focus-managed, Escape closes),
  `InsightCallout`, `SkeletonCards`, `ScoreSummary`, `LevelDots`, `MetaLine`.
- **Catalogue**: compact editorial header + live KPI strip; "Continue where you
  left off"; recommendations kept; refined filter toolbar with clear-all and an
  `aria-live` result count; scenario cards with disciplined hierarchy (kind
  eyebrow, title, summary, tabular meta, competency chips, status + action);
  skeleton loading matching final layout; calm empty/error states.
- **Workspace**: workspace header (breadcrumb, title, meta, mode segmented
  control with confirm-on-mode-change when unsaved work exists, persistent save
  indicator); two-column analytical canvas ≥1100 px (brief + evidence + answers
  left, Coach rail right) collapsing to one column; premium debrief (score
  hero, measures table, schedule chart, competency evidence, coach debrief,
  distinct retry).
- **Charts**: token colours, dash/pattern differentiation in addition to colour,
  collapsed "View the data as a table" alternatives, unchanged aria-labels.
- **Admin Studio**: same header/KPI language, client-side search + review-state
  filter with announced result count, contained sticky-header table, grouped row
  actions, confirmation before Retire, refined create/validate panels.

## Explicitly out of scope (unchanged behaviour)

- Grading, entitlements, exam-record isolation, review workflow, all endpoints.
- The Free Templates Library.
- Full i18n of Lab copy (the current surfaces are English-only; the shell nav
  is translated). New CSS is RTL-safe so Arabic keeps the mirrored shell.
- Theme switch: the platform has a single light theme and no theme
  infrastructure; one high-quality light theme is kept rather than adding a
  fragile switch.
