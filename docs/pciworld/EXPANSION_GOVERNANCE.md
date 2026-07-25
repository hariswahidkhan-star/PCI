# PCI World Expansion — Content Governance Model (Phase 0 deliverable)

_Companion to EXPANSION_PHASE0.md. Governs the 10,000-challenge bank, the 100-blog programme,
the 100-news programme and every agent that touches content. Nothing publishes without passing
this model._

## 1. Challenge taxonomy (controlled vocabularies)

Extends the shipped `pciworld_challenges` metadata. All vocabularies are closed lists owned by
the platform-operations role; authors pick, never invent.

| Facet | Values (initial) | Storage |
|---|---|---|
| Domain | planning_scheduling, cost_engineering, estimating, earned_value, risk, change_control, contracts_claims, commercial, project_finance, governance_assurance, reporting_communication, data_analytics, leadership_stakeholders, digital_delivery, ai_governance, sustainability | `domain` (new col) |
| Subdomain | free text under domain, registry-validated | `subdomain` |
| Industry | existing 20+ list (oil & gas … PMO) | `industry` (exists) |
| Lifecycle phase | initiate, plan, execute, monitor_control, close, operate | `lifecycle_phase` (new) |
| Difficulty | foundation, developing (=moderate), professional, advanced, expert | `difficulty` (exists; "moderate" alias documented) |
| Format | diagnostic_mcq, schedule_recovery, cost_investigation, risk_workshop, change_evaluation, contract_claim, executive_briefing, data_interpretation, root_cause, roleplay, document_review, prioritisation, branching, timed_incident, ai_verification, team | `format` (new; current engine covers the deterministic subset — formats requiring branching/roleplay land with the engine work in later phases and CANNOT be selected until the engine supports them: the validator enforces engine-supported formats) |
| Competency, prerequisites, objectives, standards refs, skills, geography | JSON arrays | existing `competencies_json` + new `meta_json` |

Base vs variant: variants (deterministic parameter draws) are stored as attempt-seed derivations,
never counted as base challenges — the shipped Simulation Lab discipline applies unchanged.

## 2. Challenge quality gates (all automated ones exist or extend `WorldContent.Validate`)

Automated (blocking): schema/required fields; engine/ask resolvability; ask-type ↔ solver match;
tolerance sanity; answer-leakage scan; duplicate/near-duplicate detection (new: normalized
config + title shingling against the bank); broken internal refs; scoring-total validity;
reading-level bound; taxonomy membership; localization completeness when AR is claimed;
PII scan; trademark spelling against the entity registry.

Human (blocking): technical review (calculations independently re-derived), editorial review,
accessibility review for exhibits, independent approver ≠ author (already enforced in SQL).

## 3. Delivery gates for the bank

Gate A 50 flagship → B 250 → C 500 → D 1,000 → E multi-thousand. A gate opens only when the
previous gate's content shows: zero critical content defects, completion-rate and report-rate
within thresholds set at Gate A, moderation queue current (<7 days), and rotation/search/admin
p95 unchanged. **Counts are never claimed before review; variants never count.**

## 4. Editorial roles (extends `WorldRbac`)

| Role | Adds |
|---|---|
| owner / platform_ops | everything; vocabulary + registry ownership |
| challenge_author / technical_reviewer | existing author/reviewer split (maker-checker in SQL) |
| blog_author, news_researcher, fact_checker, editor, legal_reviewer, seo_editor, localization_editor, publisher, analyst | new roles on the same `WorldRbac.Allowed` matrix; author of a piece can never be its approver; publication of news/blog additionally requires fact_check + legal pass recorded on the row |

## 5. Editorial workflow states (blogs, news; challenges keep their shipped lifecycle)

`idea → assigned → researching → drafting → technical_review → fact_check → seo_review →
legal_review → approved → scheduled → published → updated → archived`, plus `rejected`
(retains feedback + history). Published rows are versioned; corrections append a visible
correction record — never silent edits.

## 6. Source standards (news)

Hierarchy: official project sites → ministries/agencies/regulators → company pressrooms &
filings → exchanges → multilaterals → reputable journalism (context only; never cite an
aggregator over the reachable original). Every material claim stores: source URL, publisher,
source title, source publication date, event date, retrieval date, claim text, confidence note.
Freshness: ≤90 days preferred; older only if materially active and clearly dated. **No model
memory as a source. No fabrication. No copied headlines or paragraphs. Quotes brief and
attributed.** Every item carries a labelled "Why it matters for project controls" analysis
section owned by PCI World.

## 7. Company-mention policy

Central `entity registry` (legal name, trademark spelling, aliases, risk notes). Rules: mention
only when editorially relevant and sourced; never imply sponsorship/partnership/endorsement/
customer relationships; no performance/safety/financial/legal claims without a primary source;
no logos without written permission; correction route published. Legal_reviewer sign-off is a
required state for any article mentioning a registry entity.

## 8. SEO policy (summary; full technical list in EXPANSION_PHASE0.md backlog)

People-first. Prohibited: scaled thin pages, doorway pages, keyword stuffing, fake authors,
fake freshness, duplicating Institute content, auto-pages per tag combination, unreviewed AI
output, forced company names. Structured data only where the visible page supports it
(Organization, WebSite, BreadcrumbList, BlogPosting, NewsArticle). Institute links: an
admin-controlled topic→URL mapping, descriptive anchors, varied text, link only where it helps
the reader.

## 9. Multi-agent production rules (applies to every future content batch)

- Central manifests (challenges/blogs/news/sources/entities) with stable IDs and per-item
  leases: an item is assigned to exactly one agent; leases expire and are reassigned explicitly.
- Research agents return: sources with URLs + access dates, claims list, uncertainty notes.
- Coding agents return: changed files, migrations, test results, unresolved risks.
- Code overlap requires isolated branches/worktrees; two agents never edit one file.
- Validators (duplicate, citation, quality, legal, schema) run before ANY item is accepted;
  the coordinating lead reviews and integrates — never concatenates.
- Publication pauses whenever human review capacity falls behind generation.

## 10. Learner-data invariants (restating product law for the expansion)

Daily rotation and resets may change the featured challenge and daily participation state ONLY.
Historical attempts, scores, Passport evidence, streak evidence and analytics are never deleted
or rewritten by rotation, cycle restart, content retirement or migration. Attempts stay pinned
to their immutable challenge version (already structural). Badges/points/Passport entries are
never described as accredited certificates without formal Institute approval.
