# PCI Publication Framework — 100 Professional Documents

**Status:** framework v1.0 · **Owner:** Institute editorial coordinator · **Language:** British English

The Project Controls Institute's public publication programme: **one hundred professional documents**
organised into ten series, each written to a single editorial standard, each with a defined route to
LinkedIn and to the platform's Downloads Centre.

This directory is the **framework and the manuscripts**. It answers four questions in one place: what we
publish, to what standard, in what order, and how each document becomes a piece of public teaching.

> **Positioning.** These documents establish the Institute as a *teaching* authority. The rule inherited
> from `docs/books/EDITORIAL_CHARTER.md` is absolute: teach something concrete, show the working, claim
> nothing we have not earned. We never imply accreditation, recognition, endorsement or outcomes we do
> not hold.

---

## 1. The ten series

| # | Series | Docs | Prefix | What it is | Primary LinkedIn use |
|---|---|---|---|---|---|
| S01 | [Body of Knowledge — Executive Summary](s01-body-of-knowledge/) | 6 | `PCB` | The executive-summary layer over the 13-domain BoK. **The BoK itself is not republished** — only its summary. | Flagship article + carousel |
| S02 | [AI in Project Controls Guide](s02-ai-in-project-controls/) | 12 | `AIG` | The Institute's governed position on AI in a controls function | Newsletter series |
| S03 | [Competency Frameworks](s03-competency-frameworks/) | 10 | `CMP` | What competence looks like, by discipline and by level | Carousels + employer PDF |
| S04 | [Code of Ethics](s04-code-of-ethics/) | 6 | `ETH` | Professional conduct, and the casebook that makes it real | Posts + governance PDF |
| S05 | [Certification Handbook](s05-certification-handbook/) | 8 | `CER` | Everything a candidate must know before they book | Gated candidate pack |
| S06 | [Exam Blueprint](s06-exam-blueprints/) | 8 | `EXB` | The published syllabus, item design and standard setting | Transparency posts |
| S07 | [Career Roadmap](s07-career-roadmap/) | 8 | `CAR` | Routes in, routes up, and what each rung actually requires | High-reach carousels |
| S08 | [Salary and Skills Report](s08-salary-and-skills/) | 6 | `SAL` | The instrument and methodology for an honest annual market report | Annual flagship |
| S09 | [Best Practice Guides](s09-best-practice-guides/) | 20 | `BPG` | The working core — how the job is actually done, correctly | Weekly teaching posts |
| S10 | [Free Templates](s10-free-templates/) | 16 | `TPL` | Tools a practitioner can use on Monday morning | Lead magnets |
| | **Total** | **100** | | | |

Full per-document register with IDs, titles, status and LinkedIn mapping:
**[`00-framework/ASSET-REGISTRY.md`](00-framework/ASSET-REGISTRY.md)**.

---

## 2. Framework documents (the standard, not the content)

| File | What it governs |
|---|---|
| [`00-framework/CANONICAL-FACTS.md`](00-framework/CANONICAL-FACTS.md) | **Read first.** Every verified name, code, number and legal statement — and the ones that are *not* verified |
| [`00-framework/EDITORIAL-STANDARD.md`](00-framework/EDITORIAL-STANDARD.md) | Voice, evidence rules, arithmetic discipline, prohibited moves, claims policy |
| [`00-framework/DOCUMENT-TEMPLATE.md`](00-framework/DOCUMENT-TEMPLATE.md) | The canonical skeleton every one of the 100 documents follows |
| [`00-framework/FRONT-MATTER-SCHEMA.md`](00-framework/FRONT-MATTER-SCHEMA.md) | The YAML contract at the top of every manuscript |
| [`00-framework/ASSET-REGISTRY.md`](00-framework/ASSET-REGISTRY.md) | The register of all 100 documents — the single source of truth |
| [`00-framework/LINKEDIN-PLAYBOOK.md`](00-framework/LINKEDIN-PLAYBOOK.md) | How a document becomes a post, carousel, newsletter or lead magnet |
| [`00-framework/PUBLISHING-CALENDAR.md`](00-framework/PUBLISHING-CALENDAR.md) | The 52-week sequence that ships all 100 |
| [`00-framework/GOVERNANCE-AND-REVIEW.md`](00-framework/GOVERNANCE-AND-REVIEW.md) | Review gates, versioning, corrections, legal risk, retirement |

---

## 3. How this relates to what already exists

This framework **sits above** existing repository assets and does not duplicate them.

| Existing asset | Relationship |
|---|---|
| `docs/bok/` — the 13-domain Body of Knowledge (~1 MB of manuscript + PDF), authored under the **retired** `PCP-AI` code and now belonging to **PCL-AI** | **Source of truth.** S01 summarises it; S06 derives weightings from it; S09 must not contradict it. Never republished in full. Its internal `PCP-AI` naming is legacy — see `00-framework/CANONICAL-FACTS.md` §1. |
| `docs/downloads/` — candidate handbook, code of conduct, exam blueprint, glossary, formula sheet | **Predecessors.** S04/S05/S06 supersede and expand these; the originals stay as the platform's live downloads until the new versions are approved. |
| `docs/publications/` — PCP-AI candidate information pack (01–04 + PDF) | **Sibling.** The candidate-facing pack; S05/S06 are the public, LinkedIn-facing treatment of the same facts. Facts must agree. |
| `docs/books/` — PML-AI and PFL-AI book programme + `EDITORIAL_CHARTER.md` | **Binding standard.** Our editorial standard is a subset of that charter, tuned for short-form public documents. |
| `docs/marketing/linkedin-launch-20-posts.md` | **Proven voice.** The 20 launch posts are the tonal reference; the calendar in §2 continues from them rather than restarting. |

**Rule:** where a fact appears in both `docs/bok/` and here, the BoK wins. Where a fact concerns the live
platform (fees, durations, pass marks, CPD hours), the platform's configured settings win — see
§4 below.

---

## 4. The unverified-fact rule (read this before publishing anything)

Several documents in this framework naturally want to state operational specifics — examination fees,
question counts, time limits, pass marks, CPD hours, validity periods, salary figures, market statistics.

**Any such number is written as a bracketed placeholder unless it is confirmed from a cited source.**

```
The examination comprises [CONFIRM: item count] items over [CONFIRM: duration].
```

Two categories, two rules:

1. **Platform-configured facts** (fees, item counts, duration, pass mark, validity, CPD): confirm against
   the Institute's live examination settings and the candidate pack in `docs/publications/` before
   publication. Do not infer them from a draft document.
2. **Market facts** (salaries, adoption rates, demand statistics): may only be published with a named,
   dated, verifiable source — or as output from the Institute's own survey run under
   [`s08-salary-and-skills/`](s08-salary-and-skills/). **Series S08 therefore ships as a methodology and
   instrument, not as invented numbers.** This is deliberate; see `SAL-01`.

Illustrative figures used to teach a method are permitted everywhere, and are always labelled
*illustrative* so no reader mistakes a worked example for market data.

---

## 5. Reading order for a new contributor

1. `00-framework/EDITORIAL-STANDARD.md` — how we write.
2. `00-framework/DOCUMENT-TEMPLATE.md` — the shape of a document.
3. `00-framework/ASSET-REGISTRY.md` — find your assigned ID.
4. The two or three neighbouring documents in your series, so yours joins a conversation already in
   progress rather than restating it.
5. `00-framework/GOVERNANCE-AND-REVIEW.md` — what your draft must survive.

---

## 6. Status

Every document in this framework is a **first authored draft** pending subject-matter-expert review.
Nothing here is approved for publication until it has passed the gates in
`00-framework/GOVERNANCE-AND-REVIEW.md` and every `[CONFIRM: …]` placeholder has been resolved or
removed. The registry tracks per-document status.
