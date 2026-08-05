# PCI marketing assets

Everything in this folder is for promoting the AI certification suite — the three Bodies of
Knowledge (PCL-AI, PFL-AI, PML-AI) and the PCI Standards that sit behind them.

## What is here

| File | What it is |
|---|---|
| `assets/PCI-Overview-Onepager.pdf` | One-page A4 overview of the suite. The leave-behind: hand it to someone who asked "what is this?" and it answers without a call. |
| `assets/carousel-*.pdf` | Six 8-slide LinkedIn document posts, 1080 pt square. Upload as a *document* post, not an image. |
| `course-outline-pcl-ai.md` | The PCL-AI syllabus: a short lead-in post for the feed, and the full outline for a LinkedIn Article. |
| `assets/PCI-PCL-AI-Course-Outline.pdf` | The same outline as a 3-page A4 document post, rendered from that file so the two cannot drift. |
| `linkedin-launch-20-posts.md` | Twenty copy-paste text posts in the Institute's voice. |
| `pci-linkedin-launch-pack.pdf` | The twenty posts, typeset for reading offline. |
| `build_assets.py` | Builds everything under `assets/`. |

### The six carousels

| Deck | Teaches |
|---|---|
| `carousel-01-the-cpi-that-lies` | A CPI can look excellent and be wrong. The accrual arithmetic that moves it fourteen points. |
| `carousel-02-what-a-compliance-test-looks-like` | Why a requirement no two reviewers can apply the same way is an aspiration, not a requirement. |
| `carousel-03-ai-what-it-may-not-decide` | The line between what AI may assist with and what it may never decide, approve or certify. |
| `carousel-04-the-covenant-that-bites` | Reading a financial covenant for the headroom it actually leaves. |
| `carousel-05-the-five-step-method` | The five-step worked-example method used throughout the suite. |
| `carousel-06-reading-the-eac-family` | There is no single EAC. Four formulas, four assumptions, four different answers. |

Each deck ends on a PCI Standards slide, so the teaching lands on the framework rather than on a
pitch.

## Claims discipline

The visual assets say only what can be shown on request:

- **Counts and calculation figures are real and reproducible.** The 15,613 independent calculation
  checks come from the golden-answer suites; the domain, worked-example and question counts come from
  the manuscripts. If a figure changes, change it here too — see "Keeping the numbers true" below.
- **No accreditation, recognition, endorsement or affiliation is claimed anywhere.** External
  frameworks are named and characterised, never reproduced, and naming one implies nothing about its
  publisher's view of this programme.
- **The Standards are described as certification requirements established by the Institute, not as
  legislation.** That wording is deliberate and should not be softened into anything that reads as
  legal obligation.
- **Review status is not asserted.** The material does not claim expert review it has not had. If
  someone asks directly, answer directly — the position is recorded in
  `docs/books/reports/SIGN_OFF_REGISTER.md`.

## Keeping the numbers true

Every figure printed in these assets is listed below with the source it was verified against. The
figures are hard-coded in `build_assets.py`, not read from the manuscripts at build time, so they can
drift — re-verify before a launch push.

| Figure | Source |
|---|---|
| 13 / 16 / 16 domains, 61 / 61 / 63 knowledge areas | volume front matter: `docs/bok/domain-*.md`, `docs/books/pfl-ai/TOC.md`, `docs/books/pml-ai/TOC.md` |
| 45 domains, 185 knowledge areas | the three rows above, summed |
| 26 / 33 / 33 = 92 sector case studies | case-study headings in `docs/bok/` and the two `manuscript/` directories |
| 15,613 machine calculation checks | `docs/books/reports/CALCULATION_ASSURANCE.md` §3 |
| 113 standards, 532 process requirements | `docs/books/laws/STANDARDS_CONCORDANCE.md` |

**The 15,613 figure covers PFL-AI and PML-AI only.** PCL-AI has no golden-answer suite; its numbers
rest on sampled independent recomputation. `CALCULATION_ASSURANCE.md` §4 states that the two
positions must never be quoted as one number, so every asset that prints 15,613 names the two volumes
it covers in the same breath. Do not drop that scoping to make the line shorter.

**No examination weighting appears anywhere.** The course outline states 40/40/20 as the *Body of
Knowledge's* proportion, which is what the volume's own part dividers say. That is not an
examination weighting, and the examination blueprint records its weighting as an open decision — so
no exam percentage is published and none should be added. The honest answer to the question is that
the syllabus is settled and the examination weighting is not yet.

**Worked-example and question counts are deliberately absent.** Earlier drafts printed per-volume
totals that could not be reproduced from the manuscripts by any consistent counting method. Rather
than publish a number that cannot be defended when someone asks, the assets describe the worked
examples qualitatively. Reinstate counts only once a script produces them reproducibly.

## Rebuilding

Needs WeasyPrint and the Libertine superfamily (the same toolchain that builds the books):

```bash
cd docs/marketing
python3 build_assets.py
```

Writes all seven PDFs into `assets/`. The design follows
`docs/books/_build/DESIGN_SYSTEM.md` — same faces, same green, same small caps and old-style
figures — so an asset and the book it points to read as one publication.
