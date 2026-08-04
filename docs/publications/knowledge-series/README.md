# PCI AI Knowledge Series

Premium executive publications from **PCI AI · Project Controls Institute Global, Inc.**

These are institutional knowledge publications, not marketing material. Each one is written to the
standard expected of a professional body — original frameworks, worked arithmetic, named sources, and a
practical checklist a reader can apply to their own projects on the day they read it.

Each publication ships as four assets so it can move across every channel without rewriting:

| Asset | File | Purpose |
|---|---|---|
| Publication | `01-publication.md` | The full document, built to a print-ready PDF |
| Carousel | `02-linkedin-carousel.md` | 10–15 slides, one idea per slide, ready to design |
| Post | `03-linkedin-post.md` | 500–1,000 words, educational, no hard sell |
| Kit | `04-design-and-distribution-kit.md` | Cover, infographic, diagrams, SEO, hashtags, CTA, derivatives |

## Published

| # | Title | Status | PDF |
|---|---|---|---|
| 01 | [Knowing Early — An Executive Summary of the Project Controls Body of Knowledge](01-project-controls-body-of-knowledge/01-publication.md) | First edition · 22 pp | `/downloads/pci-knowledge-01-project-controls-body-of-knowledge.pdf` |

## Planned

Project Controls Competency Framework · Project Finance Competency Framework · Project Management
Competency Framework · AI in Project Controls (Executive Guide) · AI in Project Finance · AI in Project
Management · Code of Ethics · Certification Handbook · Examination Blueprint · Certification Learning
Outcomes · Project Controls Career Roadmap · Best Practices Guide · Professional Templates Collection.

Publications are authored one at a time and reviewed before the next begins.

## Building the PDF

```bash
pip install weasyprint markdown
cd docs/publications/knowledge-series/build
python3 build_publication.py                                  # build every publication
python3 build_publication.py 01-project-controls-body-of-knowledge   # build one
```

Output goes to `backend/wwwroot/downloads/pci-knowledge-<slug>.pdf`, served by the site at
`/downloads/<file>.pdf`. The house stylesheet is `build/publication.css` — navy title page, clean body
pages, one gold accent, A4.

**Source format.** Line 1 is `# Title`, then a blank line, then `> Subtitle` as a blockquote. Everything
after is the body. The builder generates the title page and appends the standard notices block, so
neither belongs in the body.

**Blockquote conventions.** A blockquote beginning `**Figure N — suggested diagram.**` renders as a
grey figure-spec box; one containing `E = F × T × A` renders as a centred equation panel; any other
blockquote renders as a gold callout.

## Editorial standards

These are hard gates, applied to every asset before release. They exist because the Institute's
credibility is the product.

- **No accreditation claims.** PCI is not accredited by ANAB, IAS or any ISO/IEC 17024 body. Never imply
  otherwise. Never claim government recognition.
- **No outcome claims.** Nothing about employment, salary, promotion, immigration or licensing.
- **Every statistic carries a named source and year.** No unattributed figures, ever.
- **Every worked number is recomputed and verified** before it enters a draft.
- **Case studies are labelled as illustrative composites.** No client, project or organisation is
  identifiable.
- **Standards are described, never reproduced.** Named at awareness level, explained in our own words.
- **British English. No emojis. No marketing language.**
- **The notices block appears on every PDF**, unaltered.

## Voice

Executive and plain. Short paragraphs. Simple words for difficult ideas. Show the arithmetic. Land one
takeaway per section. Educate before mentioning the credential — and on most assets, do not mention it
at all.

The governing principle across everything the Institute publishes: **AI proposes; the professional
disposes.**
