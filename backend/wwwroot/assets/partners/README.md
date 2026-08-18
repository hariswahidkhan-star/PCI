# Partner marks

Marks belonging to organisations other than PCI, used in co-branded material.

| File | What it is |
|---|---|
| `certuvo-mark.png` | Certuvo wordmark, `#3662DF`, transparent, trimmed to the glyphs |
| `certuvo-mark-white.png` | The same coverage rendered white, for dark grounds |

## How these were made

Supplied as a flat blue-on-white JPEG. A flat two-colour source recovers cleanly, so
coverage was taken from the red channel rather than keyed on white — a pixel is
`white·(1−a) + mark·a`, and red runs 255 → 0x36 across that range, giving true
antialiasing instead of a hard matte. Values under 12 were floored to kill JPEG mottle
in the ground, then the alpha was trimmed to its bounding box and used for both the blue
and the white build, so the two variants are pixel-identical in coverage.

```python
span = 255 - 0x36
a = (255 - pixel_red) * 255 // span
```

If a vector original ever arrives, replace both files with it — these are raster and will
soften if scaled much beyond the 964 × 320 they were trimmed to.

## Using them

Do not recolour, distort or set the name in our own typeface: a partner's name belongs in
its mark. The carousel kit (`docs/formula-sheets/social/carousel_kit.py`) carries the
`.cobrand` lockup and the `.pmark` / `.ctamark` rules, and ships both variants on every
slide so the right one shows on whichever ground the slide uses.

Certuvo's blue (`#3662DF`) sits close to PCI's primary (`#1D4ED8`) — near enough that the
two marks share a lockup without clashing, far enough that they stay distinguishable. Do
not "correct" either one toward the other.
