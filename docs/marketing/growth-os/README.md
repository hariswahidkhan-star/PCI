# PCI AI Growth OS — build and verification chain

`docs/marketing/PCI_AI_Growth_OS.xlsx` is the shipped workbook: 42 sheets covering the
whole marketing operation — daily logging, LinkedIn outreach, partnerships, content and
scheduling, off-page SEO and link building, job postings, 133 platform accounts with a
step-by-step usage guide for each, a 5,683-brief article bank, and the management
reporting that reads off all of it.

Everything in this folder exists so the file can be rebuilt, changed and re-proved rather
than hand-edited. Nothing here is needed to *use* the workbook.

## Rebuilding

```bash
cd docs/marketing/growth-os
cp _base_v6.xlsx growth_os_v6.xlsx
export PCI_XLSX_PASSWORD=...   # the owner's sheet-protection password
python3 build_v7.py      # base  → PCI_AI_Growth_OS_V7.xlsx
python3 build_v8.py      # V7    → PCI_AI_Growth_OS_V9.xlsx
python3 finish_v8.py     # recalculate, repair, then 73 structural checks
```

The protection password is a guard rail against accidental formula edits, not a security
control — the XLSX hash is trivially reversible either way. It is still the owner's, so
it is passed in at build time and is not stored in this repository. Ask the owner for it,
or pick a new one: the build applies whatever `PCI_XLSX_PASSWORD` holds.

`finish_v8.py` recalculates through LibreOffice and then repairs the four things that
save strips (tab colours, hidden helper columns, sheet-protection passwords, and the
workbook-protection element) at zip/XML level, so the computed caches survive intact.

## Proving it

Six independent layers, all of which must pass before the file ships:

| Script | What it proves | Assertions |
|---|---|---|
| `finish_v8.py` | structure, protection, wiring, print setup | 73 |
| `verify_v8.py` | a three-person team's month, recalculated and re-derived in Python | 51 |
| `verify_oracle.py` | the numerical audit's findings cannot return | 47 |
| `verify_robust.py` | the reliability audit's attacks cannot return | 42 |
| `audit_all.py` | eight lenses over formulas, validations, protection, content, print, navigation | — |
| `oracle_static.py` + `excel_integrity.py` | silent-zero criteria, range drift, and anything that would make Excel offer to repair the file | — |

The two regression suites are the important ones. Each reproduces the exact adversarial
input that exposed a defect — a lead marked accepted without a logged request, a date
typed as text, a wrong-year typo, a meeting that progressed past "Meeting Booked", a
schedule whose end date precedes its start, minutes logged under a misspelled name — and
asserts the corrected behaviour. A change that reintroduces any of them fails here
instead of in a manager's hands.

## Layout

| File | What it holds |
|---|---|
| `canonical_lov.py` | the single source of truth: 133 platforms with area, priority, geography, logging rule and value rank; objectives; brands; domains; activity types |
| `keywords_data.py` | 76 researched keywords with difficulty graded from live SERP sampling |
| `usage_guides.py` | the weekly play, KPI and time budget for every platform |
| `article_bank*.py`, `grammar.py` | the article-brief generator and its seed data; `grammar.py` keeps generated titles grammatical |
| `glossary_data.py` | every term the workbook uses without stopping to explain it |
| `build_v7.py` / `build_v8.py` | the two build stages |

## Conventions worth keeping

- **Never reference a row by number across sheets.** Dashboard tiles look up Weekly Pulse
  KPIs by name; the verification suites look up rows by label. A row inserted above a
  block silently repointed a headline tile once — that is why.
- **Report ranges start at the first grid row.** The logs ship empty. There are no example
  rows to type over; the worked examples live on TEAM GUIDE.
- **Write fill colours as eight hex digits.** Six digits are stored with a zero alpha
  channel, which Excel ignores and LibreOffice paints as transparent.
- **Every dropdown carries an input message.** The answer belongs at the cell where the
  decision is made, not two sheets away.

The workbook is protected with the owner's password; formulas are locked, genuine inputs
are not. No password is stored anywhere in the file or in this folder.
