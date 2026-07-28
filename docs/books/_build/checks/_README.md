# Per-domain check modules

Each newly authored domain contributes **one file** here — `<book>_d<NN>.py`, e.g.
`pfl_d05.py`, `pml_d12.py` — so that domains written concurrently never edit a shared file.

A module exposes exactly one function:

```python
def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    check("WE 5.1.2 something", D(2) * 3, 6)
```

`ctx` carries the `check` helper, `Decimal` as `D`, and the shared helpers `af(r, n)` (annuity
factor), `ew(M, L)` (governance latency) and `mesh(n)` (pairwise interface count), plus the
master-thread constants (`KESTREL_*`, `MERIDIAN_*`, `AURIGA_*`) so that a value shared between
domains is read from one place and never re-typed.

Rules that the loader enforces or the gate depends on:

- **Every printed number in the manuscript has a check here.** A domain does not pass gate otherwise.
- A module that raises is a **failure**, not a skip — there is no silent pass.
- Reuse a master-thread constant rather than restating its literal; a value that appears in two
  domains has one golden check, not two.
- Tolerances: default 0.005. Widen only for a display-rounding boundary, and say so in the name.
