# Per-domain figure modules

Each newly authored domain contributes **one file** here — `<book>_d<NN>.py` — so that domains
written concurrently never edit a shared file.

A module exposes exactly one function:

```python
def make(ctx):
    svg, axes, PML = ctx["svg"], ctx["axes"], ctx["PML"]
    BLUE, CRIMSON, INK, SLATE, GRID = (ctx[k] for k in ("BLUE", "CRIMSON", "INK", "SLATE", "GRID"))
    (PML / "fig_5_2_1.svg").write_text(svg(640, 400, body))
```

`ctx` carries `svg(w, h, body)`, `axes(x0, y0, x1, y1, xlab, ylab)`, the brand colours, the `FONT`
string and the `PML` / `PFL` output directories.

Rules:

- Filename must be `fig_<D>_<K>_<T>.svg` — the build injects a figure only where the manuscript
  carries a matching `> **Fig D.K.T — …` blockquote spec.
- PCI-original artwork only. No third-party diagram is reproduced or adapted.
- Output must be deterministic: no randomness, no timestamps, no locale-dependent formatting.
- Escape `&` `<` `>` in labels, and use `&#8722;` for a minus sign and `&#8217;` for an apostrophe.
