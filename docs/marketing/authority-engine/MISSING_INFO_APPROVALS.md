# Stage A — missing-information and approval list

Deliverable 3 of the Master Prompt's Stage A. Stage B may proceed on the repo-verified dossier,
but **no article that touches a flagged item may pass the Judge gate until its owner resolves it.**

## Blocking inputs that were not available

| # | Item | Impact | Owner action |
|---|---|---|---|
| M1 | **`PCI_AI_Growth_OS.xlsx` was not attached.** The dossier substitutes the platform repository (seeded content + public pages + policy docs) as source of truth. | Any workbook-only facts (audience research, channel plans, prior keyword work) are absent. | Supply the workbook; reconcile it against `FACT_DOSSIER.md` — where they disagree, the repo (the live product) wins until PCI rules otherwise. |
| M2 | **Live-site verification is blocked** by this environment's network egress policy (fetches to the PCI domains are refused). All facts are `Verified (repo)`. | Ledger rows carry `[LIVE-SITE VERIFICATION PENDING]` / `[PRICE UNVERIFIED]`. | Run the verification pass from a network-enabled session: fetch each dossier fact's public page, stamp `verified on` dates, promote statuses. |
| M3 | **`pciai.org` and `pciglobal.ai` have zero evidence in the platform** (repo-wide search; `pci-global.org` likewise). The master prompt asserts five owned domains; only three are real in the code. | The "all five domains" ecosystem block cannot ship — linking to unconfirmed domains violates the prompt's own §2 rule. | PCI confirms ownership + purpose + target URL for both, or the ecosystem block ships with the three verified domains. |

## Contradictions requiring a PCI decision

| # | Item | Detail | Owner action |
|---|---|---|---|
| M4 | **Certuvo access trigger** | Master prompt §4: access follows "the applicable PCI **exam fee**". Platform: provisioning follows a **settled/active membership** (`docs/CERTUVO_INTEGRATION.md:18-21`, `Certuvo.tsx:84`), with a configurable `certuvo_requires` rule. | PCI states the current commercial rule; copy uses the membership version (repo-verified) until then. |
| M5 | **Nonprofit wording inconsistency** | Footer: "intends to seek recognition … not yet granted" (safe). JSON-LD org block: "a **registered nonprofit organisation** pursuing 501(c)(3)…" — a stronger claim. | Legal confirms one formulation; content uses the footer version meanwhile. The JSON-LD should be corrected in the product (candidate platform fix). |
| M6 | **Retired "PCP-AI" naming persists** in `docs/HONORARY_ROUTE_AND_REGISTRATION.md:30-33,65` and seeded page metas (`schema.sql:469,662`). | Copy lifted from those sources would resurrect a retired brand. | Editorial rule: never lift copy containing "PCP-AI"; candidate platform cleanup ticket. |
| M7 | **`certification.html` markets only PCL-AI pricing** while the platform sells three certifications at the same price. | Articles must not imply only PCL-AI is purchasable. | Candidate platform content update; articles describe suite-wide pricing per the dossier. |

## Facts that simply do not exist yet

| # | Item | Rule until resolved |
|---|---|---|
| M8 | Exam **retake fee** (seeded inactive/0; page says "to be confirmed and displayed before any exam retake booking is completed") | Publish only that sentence. |
| M9 | **Certuvo access duration** (no fixed term seeded) | `[VERIFY CERTUVO ACCESS TERM]` placeholder; publish no duration. |
| M10 | **Public CPD hours figure** (internal setting `sp_cpd_target_hours=30`; the public page deliberately says "a minimum amount of qualifying activity") | Use the public formulation; no number. |
| M11 | **Taxes on fees** | No repo copy addresses taxes; state "in US dollars" only. |
| M12 | `org_legal_name` site-setting is read but never seeded | Operator may seed it; statements fall back to "Project Controls Institute Global, Inc." |
| M13 | Competitor facts (all bodies) | `COMPETITOR_RESEARCH_PLAN.md` fetch queue; no comparison article drafts until on file. |

## Standing approval gates (from the master prompt, restated as owners)

- Legal/board: every policy draft ships `DRAFT — LEGAL/BOARD APPROVAL REQUIRED`; PCI legal-status
  lines only per dossier §1.
- PCI subject-matter owner: pricing, routes, Certuvo terms, honorary wording — approves each
  Stage C pilot before Stage E production.
- Technical owner: robots/crawler policy, sitemaps, Search Console/IndexNow submissions.
