# Building the PCP-AI Body of Knowledge PDF

The BoK is authored as Markdown under `docs/bok/<NN-domain-slug>/<KA>.md` (61 Knowledge Areas
across 13 domains). To compile the downloadable book:

```bash
# 1) assemble ordered, styled HTML (cover + contents + 61 chapters + appendices)
python3 docs/bok/_build/bok_build.py /tmp/bok.html          # needs: pip install markdown

# 2) render to A4 PDF with page numbers (headless Chromium via Playwright)
node docs/bok/_build/bok_pdf.js /tmp/bok.html docs/bok/PCP-AI-Body-of-Knowledge-v1.pdf
```

`bok_build.py` orders chapters by domain then KA number, converts Markdown (tables/code/lists),
wraps them in a print stylesheet (A4, brand blue `#1D4ED8`), and appends three auto-generated
appendices: the master formula & symbol sheet, the standards-referenced list, and the study-apparatus
/ SME-reconciliation note. The compiled `PCP-AI-Body-of-Knowledge-v1.pdf` is an **SME-verifiable first
draft** — every worked number, standard reference and AI claim must be confirmed by a qualified
subject-matter expert before it becomes certified text.
