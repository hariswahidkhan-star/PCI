# Stage A — PCI fact dossier (source-controlled)

Deliverables 1–2 of the Master Prompt's Stage A. **Source of truth used:** this repository (the
platform's seeded content, public pages and policy docs), cited as `file:line`. The referenced
workbook `PCI_AI_Growth_OS.xlsx` was **not supplied**, and live-site fetches are blocked by this
environment's network egress policy — every fact below therefore carries status
`Verified (repo)`; promotion to `Verified (live)` requires the operator pass described in
`MISSING_INFO_APPROVALS.md`. Nothing here may be published while its status line says otherwise.

## 1. PCI entity facts and approved wording

| Fact | Approved wording (verbatim) | Source | Status |
|---|---|---|---|
| Legal name | Project Controls Institute Global, Inc. | `backend/Core/PartnerStatement.cs:136-138` (fallback; `org_legal_name` setting exists but is never seeded) | Verified (repo) |
| Legal form | "a Delaware Non-Stock Corporation" | site footer, e.g. `backend/wwwroot/certuvo.html:10` | Verified (repo) |
| Tax status | "It intends to seek recognition as a tax-exempt nonprofit organisation under Section 501(c)(3) of the U.S. Internal Revenue Code; tax-exempt status has not yet been granted, and contributions are not currently represented as tax-deductible." | footer, `certuvo.html:10` | Verified (repo) — **use this wording, not the JSON-LD variant; see risk F5** |
| Accreditation | "PCI is not currently accredited by ANAB, IAS, or any ISO/IEC 17024 accreditation body — its certification framework is being developed with reference to ISO/IEC 17024 personnel-certification principles." | footer, `certuvo.html:10` | Verified (repo) — mandatory in trust content |
| No-guarantee clause | "PCI does not guarantee employment, promotion, salary improvement, immigration benefits, licensing eligibility or acceptance by any third party." | footer, `certuvo.html:10` | Verified (repo) — mandatory near career claims |
| Slogan | "AI proposes. The professional disposes." | JSON-LD org block, `backend/wwwroot/route-honorary.html:3` | Verified (repo) |
| Founding date / country | 2025 / US | JSON-LD org block | Verified (repo) |
| Contact | hello@projectcontrolsinstitute.org | `backend/schema.sql:930` | Verified (repo) |
| Social profiles | linkedin.com/company/project-control-institute · x.com/projectcontrolinstitute | JSON-LD `sameAs` | Verified (repo) |

## 2. Domain / URL inventory

| Domain (per master prompt) | Repo evidence | Status |
|---|---|---|
| `https://projectcontrolsinstitute.org/` | Canonical: `site_base_url = https://www.projectcontrolsinstitute.org` (`backend/schema.sql:791`); JSON-LD on every page; secure-exam client host allowlist (`secureexam/PCI.SecureExam.Core/ClientConfig.cs:10-19`) | **Verified (repo)** — live check pending |
| `https://mypci.org/` | Student-portal domain: `PortalDomain.cs:8-13` (`PORTAL_BASE_URL = https://mypci.org`); brand is **"MyPCI"**, one word (`wwwroot/platform-preview.html:39`; "My PCI" with a space appears nowhere) | **Verified (repo)** — live check pending |
| `https://pciworld.org/` | PCI World domain: `backend/Program.cs:576` (host mapping `pciworld.org` / `admin.pciworld.org`), `Core/WorldLifecycle.cs:7` | **Verified (repo)** — live check pending |
| `https://pciai.org/` | **Zero matches repo-wide** | **Unverified — do not link until confirmed** (see MISSING M3) |
| `https://pciglobal.ai/` | **Zero matches repo-wide** (nor `pci-global.org`) | **Unverified — do not link until confirmed** (see MISSING M3) |

Deep-URL rules: verified deep paths so far are `/certifications/pcl-ai`, `/certifications/pfl-ai`,
`/certifications/pml-ai`, `/certifications` (`backend/Data/SeedContent.cs:60`), `/verify.html`
(`wwwroot/verify.html`), `/world/verify` (`Core/WorldPages.cs:1889`). Everything else in the
ledger stays `[DEEP URL PENDING]` until the live crawl (Master Prompt §2: never invent deep URLs).

## 3. Certification matrix

| | PCI PCL-AI | PCI PFL-AI | PCI PML-AI |
|---|---|---|---|
| **Official full title** | PCI AI Project Controls Leader | PCI AI Project Finance Leader | **PCI Project Management Leader – AI** (suffix form — deliberate asymmetry, enforced by `MultiCert.cs:566-590`; never write "PCI AI Project Management Leader") |
| Source | `MultiCert.cs:124` | `MultiCert.cs:133` | `MultiCert.cs:103,79,110,142` |
| Scope (verbatim) | "unites project-controls governance, cost, planning, earned value, forecasting and risk with AI-enabled project controls" | "covers project finance, financial modelling, capital structure, bankability, coverage ratios, PPP structures and financial close" | "a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management" |
| Suite name | "PCI AI Project Leadership Certification Suite" (`route-honorary.html:3`, `CertCompare.cs:24`) | | |
| Entry requirement | "three years of professional experience in any field — the only prerequisite" (`wwwroot/candidate-journey.html:7`) | same | same |
| Assessment | "scenario-based and sat fully online under remote proctoring" (`candidate-journey.html:7`) | same | same |
| Validity | 3 years, then recertification (`schema.sql:425` `expiry_years DEFAULT 3`; `CertCompare.cs:127`) | same | same |
| Retired names | PCP-AI → PCL-AI; PFIP → PFL-AI; CPMD → PML-AI (`SeedContent.cs:64-66`); "PCI AI Project Delivery Leader" retired (`MultiCert.cs:168`) | | |

## 4. Price matrix (all USD; single one-time payment, no instalments — `wwwroot/certification.html:20`)

| Component | Standard | Current discount | Payable now | Source | Status |
|---|---|---|---|---|---|
| Certification exam (each of the three) | 500 | 30% | **350** | `schema.sql:286,928`; `MultiCert.cs:100-113` (no per-cert override seeded) | Verified (repo) — `[PRICE UNVERIFIED]` for live |
| Membership | 99/year | 50% | **49.50** | `schema.sql:284,926` | Verified (repo) |
| Membership + Exam bundle | — | — | **399.50** ("membership USD 49.50 + exam USD 350") | `certification.html:20` | Verified (repo) |
| Renewal / recertification | 99 per three-year cycle | — | 99 | `schema.sql:788,790`; `certification.html:20` | Verified (repo) |
| Exam retake | **Unpublished by design** — "to be confirmed and displayed before any exam retake booking is completed." | — | — | `schema.sql:288` (seeded 0, inactive); `certification.html:20` | **Do not state a retake price** |
| Application fee | "Included" when null | — | — | `CertCompare.cs:121-127` | Verified (repo) |
| Taxes | Not addressed in repo copy | — | — | — | Needs PCI confirmation |
| Sanctioned footnote | "Fees are live values from PCI's pricing configuration. Discount codes, waivers and sponsorships are applied at checkout." | | | `CertCompare.cs:129` | Verified (repo) |

## 5. Route matrix

**The master prompt's "standard/experience/recognition/honorary" vocabulary does not match the
platform.** Public framing is **three routes** ("There are three ways to be recognised" —
`wwwroot/membership.html:9`); the system carries 8 route keys (`MultiCert.cs:502-512`):

| Route (public) | Exam? | Fee | Decision | Approved wording |
|---|---|---|---|---|
| Standard | Yes | Standard fees | Application + approval | "For candidates meeting the education and professional-experience requirements." (`MultiCert.cs:504`) |
| Founding | Yes | Free | **By invitation** — "A limited founding cohort, joined by invitation during the founding stage." (`membership.html:9`) | earned credential |
| Honorary | **No exam** | Free | Board discretion — "**Anyone may apply for the board's consideration; conferral is at the board's discretion.**" (`route-honorary.html:11`) | Confers "Honorary Fellow (PCI) — labelled honorary, never the examined PCL-AI" (`membership.html:9`) |

Internal-only route keys (never marketed): sponsored, complimentary, waived_full,
waived_partial, test. Honorary invariant (binding, `docs/HONORARY_ROUTE_AND_REGISTRATION.md:65`):
approving an honorary application **never** creates a credential/entitlement/attempt row.
**Prohibited:** the phrase "invitation to be considered" (belongs to no route — "invitation" is
Founding-only vocabulary); any implication that honorary conferral is likely or guaranteed.

## 6. Certuvo relationship and access terms

| Fact | Approved wording (verbatim) | Source |
|---|---|---|
| Relationship | "Preparation and training are provided separately by Certuvo, our official partner." / footer: "Exam practice & training by Certuvo, PCI's official platform for exam preparation and study" | `SeedContent.cs:124`; footer `certuvo.html:10` |
| Separation | "PCI sets the standard; Certuvo is where you meet it." … "PCI owns the standard, the body of knowledge and the examination." | `certuvo.html:7` |
| Not mandatory | "Do I have to use Certuvo to sit the exam? No. Certuvo is the official preparation, but candidates may prepare however they wish." | `certuvo.html:7` |
| No pass guarantee | "Preparation through Certuvo or any other provider does not guarantee exam success or certification." | `candidate-journey.html:7` |
| **Access trigger** | "Your Certuvo practice account is set up automatically once your **membership** is active. It will appear here shortly after payment." | `frontend/src/pages/Certuvo.tsx:84`; mechanics `docs/CERTUVO_INTEGRATION.md:18-21` (settled membership → `CertuvoLink.Provision`; eligibility rule `certuvo_requires` configurable) |
| Access duration | **No fixed term seeded** — UI renders the record's own `expires` (`Certuvo.tsx:74`) | `[VERIFY CERTUVO ACCESS TERM]` before publishing any duration |
| Mandated notice | "Certuvo is an external practice platform. All practice questions, mock examinations, study tools, AI coaching, progress tracking and learning activities are available directly within Certuvo." | `backend/Core/Provisioning.cs:632` |

> **Correction to the master prompt (§4):** its proposition ties access to "the applicable PCI
> **exam fee**". The platform ties provisioning to an **active membership** (default
> `certuvo_requires` rule). Do not publish the exam-fee version until PCI confirms which is
> current — see MISSING M4.

## 7. Guided labs and simulation labs

- Official product name: **"PCI AI Project Controls Simulation Lab"** (`SimLabSchema.cs:4`,
  `Core/SimLab.cs:7`). "Guided lab" is one of five artifact kinds — `guided_lab | skill_drill |
  scenario | capstone | team` (`SimLabSchema.cs:35`); difficulty bands foundation→expert.
- Seeded catalogue: 10+ published guided labs (WBS, EVM, scheduling, CBS, progress, Pareto,
  change, cash-flow…) and skill drills (`SimLabSchema.cs:185-292`), typically ~15 minutes each.
- Grading is deterministic; "the answer key is never stored, it is derived … at grade time"
  (`SimLabSchema.cs:187-188`); the AI Coach "never computes numbers" (`SimCalc.cs:12`).
- Access: `simlab_requires = membership_or_exam` (`SimLabSchema.cs:179-183`).
- **"simulation lap" typo: zero occurrences repo-wide** — the master prompt's correction task is
  already satisfied in the platform; keep the check in the editorial QA list for new copy.

## 8. PCI World Passport fact sheet

- Definition: "a page of verified practice evidence that its owner controls entirely … Answers
  are never published, to anyone, under any setting." (`Core/WorldPages.cs:1447`)
- Cost: "Free, and it always will be." (`WorldPages.cs:1945`)
- Owner controls: rotate/expire/withdraw the share link; "a withdrawn link simply stops
  resolving" (`WorldPages.cs:1448`).
- Relationship to certification: "The Institute is the certification authority; PCI World is its
  open practice ground … formal credentials are earned only through the Institute's own
  examinations." (`WorldPages.cs:1457`)
- **Mandatory disclaimer** (verbatim on every verification result — `WorldVerify.cs:47-51`):
  "PCI World Passport records selected professional practice and challenge evidence. It is not,
  by itself, a PCI certification, examination result, accreditation, licence, or guarantee of
  professional competence."
- Verification: POST-only by PCI Student Number (`^PCI-\d{4}-\d{6,}$`), uniform answer for
  unknown/withdrawn/expired (`WorldVerify.cs:7-60`).

## 9. Competitor matrix

No competitor fact is verified. The fetch queue, per-body source list, fairness contract and the
rule governing PCI's defensible-distinction claim live in `COMPETITOR_RESEARCH_PLAN.md`.

## 10. Claims register

| # | Claim | Status |
|---|---|---|
| C1 | Titles, scope, suite name as §3 | **Verified (repo)** — live check pending |
| C2 | Prices as §4 (exam 500→350, membership 99→49.50, bundle 399.50, renewal 99/3yr) | **Verified (repo)** — `[PRICE UNVERIFIED]` until live check |
| C3 | Retake price | **Do not publish** any figure; only the sanctioned "to be confirmed…" line |
| C4 | Route story = Standard / Founding / Honorary; honorary = "apply for the board's consideration" | **Verified (repo)** |
| C5 | Certuvo = official preparation partner; access on active membership; no duration claim | **Verified (repo)** with `[VERIFY CERTUVO ACCESS TERM]`; exam-fee-trigger version **Do not publish** pending M4 |
| C6 | 3-year validity/CPD cycle; renewal USD 99; mandatory AI-currency CPD component | **Verified (repo)**; numeric CPD hours (30) is an internal setting — **Do not publish** a public figure until PCI confirms |
| C7 | Legal/tax/accreditation wording as §1 footer text | **Verified (repo)**; JSON-LD "registered nonprofit" variant **Do not publish** (see F5) |
| C8 | Passport facts + disclaimer as §8 | **Verified (repo)** |
| C9 | pciai.org / pciglobal.ai are PCI-owned | **Needs PCI confirmation** — zero repo evidence |
| C10 | Any competitor fact | **Needs verification** (live fetch) — none may appear in drafts |
| C11 | "PCI is the only credential combining PM/controls + finance + applied AI" | **Do not publish** until the dated competitor sweep is on file (COMPETITOR_RESEARCH_PLAN.md) |
| C12 | Salary uplift, pass rates, student counts, employer acceptance | **Do not publish** — no evidence exists; master prompt §1.3 prohibition |
