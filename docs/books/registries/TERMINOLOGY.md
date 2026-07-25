# Shared Terminology Registry — PML-AI · PFL-AI (binds to the PCP-AI seed glossary)

**Rule:** a term is defined **once** and used identically in both new books and consistently with the
PCP-AI BoK. New terms are added here (with the proposing book/domain) before first use in a chapter;
silent redefinition in a chapter is a gate defect. British English throughout.

## 1. Inherited from the PCP-AI Style Spine (unchanged meanings)

Accrual basis · Baseline · Control account (CA) · Cost breakdown structure (CBS) · Provision ·
Recognition · Work breakdown structure (WBS) — as defined in `docs/bok/00-style-spine.md` §3, plus the
full PCP-AI global glossary (`docs/bok/appendices.md`, Appendix B) which governs any term it contains.

## 2. Programme-level terms (fixed at Phase 0)

| Term | Definition (as used in both books) | Proposed by |
|---|---|---|
| **PML-AI** | PCI Project Management Leader – AI: the certification; its BoK is this programme's Book One | Charter |
| **PFL-AI** | PCI Project Finance Leader – AI: the certification; its BoK is Book Two | Charter |
| **Responsible AI principle** | "AI proposes; the professional verifies, decides and remains accountable" — the suite-wide restatement of PCP-AI's "AI proposes, the professional disposes" | Charter (D-11) |
| **Domain / Knowledge Area / Topic** | The three-level content hierarchy `D.K.T`, identical to PCP-AI | Pattern spec |
| **Sponsor** | The accountable executive owner of the business case (PML-AI); in PFL-AI project-finance contexts, an equity investor promoting the project — the books flag the context at each use | PML-AI D3 / PFL-AI D1 |
| **Special-purpose vehicle (SPV)** | The ring-fenced legal entity created to own, finance and operate a project | PFL-AI D5 |
| **Bankability** | The degree to which a project's contracts, risks and cash flows support limited-recourse financing on acceptable terms | PFL-AI D5 |
| **CFADS** | Cash flow available for debt service, as defined in the formula registry | PFL-AI D10 |
| **Cash waterfall** | The contractually agreed priority order in which project cash is applied each period | PFL-AI D6/D15 |
| **Benefits realization** | The identification, planning, measurement and sustainment of the outcomes a programme exists to deliver | PML-AI D2/D16 |
| **Decision rights** | The explicit allocation of which role may take which decision at which threshold | PML-AI D3 |
| **Psychological safety** | A shared belief that the team is safe for interpersonal risk-taking — candour without penalty | PML-AI D12 |
| **Hybrid delivery** | A deliberate combination of predictive and adaptive methods under one governance frame | PML-AI D13 |
| **Model risk** | The risk of loss from decisions based on flawed, misused or misunderstood models (financial or AI) | PFL-AI D11/D16 |

Terms proposed during drafting are appended in dated batches with the proposing chapter; the
consolidation pass merges each book's key-terms boxes back into this registry and each book's
Appendix B glossary.

## 3. Naming and branding rules

- Certification names are written exactly as the live catalogue (`backend/Data/MultiCert.cs`) states
  them — the suite is the **PCI AI Project Leadership Certification Suite**:
  **PCL-AI** — PCI AI Project Controls Leader (the previous book's credential, renamed from PCP-AI);
  **PFL-AI** — PCI AI Project Finance Leader (brief uses "PCI Project Finance Leader – AI" — open
  decision OD-1); **PML-AI** — PCI Project Management Leader – AI. Retired names (PCP-AI, PDL-AI,
  CPMD, PFIP) never appear in new content.
- **No trademark symbols (™/®) on PCI credential names** — platform policy, enforced at boot by
  `MultiCert.TrademarkStrip`. Third-party names appear unadorned in running text; marks are
  acknowledged once on the copyright page (the approved book pattern).
- The portfolio statement "Finance intelligently. Control predictively. Deliver successfully."
  (`MultiCert.PortfolioTagline`; order maps Finance = PFL-AI, Control = PCL-AI, Deliver = PML-AI)
  appears in front matter and where genuinely apt — never as a running slogan.
- Examination parameters (duration 90 min, pass mark 65 %, USD 500 list / USD 350 discounted, 3-year
  validity, 12-month scheduling window) are **platform-configurable defaults** — the books cite them
  only as "published in the current examination specification", never as hard-coded facts (D-29).
