# PCI Publication Design System

The design of a professional reference is not decoration. A candidate reads these volumes for hours
at a time, hunts a number under pressure, and returns to a page they half-remember. Everything below
serves reading, finding and returning. Where a choice is between elegant and legible, legible wins —
but the two are rarely in conflict, and most of what looks cheap in a technical book is a legibility
failure wearing a badge.

This system governs all four publications: the three Bodies of Knowledge, and the PCI Standards.

---

## 1. Type

Set in the **Libertine superfamily** — a designed set with a text face, a display cut, a sans
companion and a monospace, sharing one skeleton. Using one family across four roles is what gives a
book its voice; mixing unrelated faces is the commonest tell of a document nobody designed.

| Role | Face | Why |
|---|---|---|
| Running text | **Linux Libertine O** | A book text face with true small caps, old-style figures and a full ligature set. Robust at 10pt across 1,200 pages of dense technical matter — where a delicate Garamond would break down. |
| Chapter and part titles | **Linux Libertine Display O** | The display optical size: tighter fitting, finer hairlines, drawn to be set large. |
| Running heads, labels, captions, table headers | **Linux Biolinum O** | The family's humanist sans. Carries the small apparatus without competing with the text. |
| Identifiers, code, formulas | **Linux Libertine Mono O** | Same skeleton, fixed width. |

**Never** use a UI or screen face — Inter, system-ui, Georgia, Arial, Helvetica. Georgia in
particular was drawn for low-resolution screens; its large x-height and heavy colour look coarse in
print and it is the single strongest signal that a document was styled by default rather than
designed.

### Figures

**Old-style figures in running prose** (`onum`): they have ascenders and descenders, so they sit in a
line of text the way lowercase letters do instead of shouting. **Lining, tabular figures in tables
and formulas** (`lnum`, `tnum`): equal width, so columns of numbers align on the digit.

This distinction matters more in this corpus than in most, because these books are full of both
narrative numbers and columnar ones.

### Small caps

Real small caps (`smcp`), never faux — never `text-transform: uppercase` at a reduced size, which
produces letters too heavy for their width. Used for running heads and call-out labels only.

### Ligatures

Standard ligatures on (`liga`). Discretionary ligatures off — they are a display effect and become
noise in a long text.

## 2. The page

A4, with **asymmetric margins**. The inner margin is smaller than the outer because facing pages
share the gutter, and the bottom is deeper than the top because the optical centre of a page sits
above its geometric centre. Symmetric margins are the second tell of an undesigned document.

| | |
|---|---|
| Inner | 22 mm |
| Outer | 30 mm |
| Top | 24 mm |
| Bottom | 28 mm |
| Measure | ~66 characters — the range in which the eye finds the next line without effort |

**Folio** in the outer margin, old-style figures, Biolinum. **Running head** in small caps with a
hairline beneath: verso carries the volume, recto the current part or domain.

Hyphenation on. Widows and orphans controlled at 2 lines. No heading may end a page alone.

## 3. Colour

One accent, used sparingly. A professional body's reference is not a brochure.

| Token | Value | Use |
|---|---|---|
| Ink | `#1A1A1A` | Body text. Soft black — pure black on white is harsh in print and vibrates against the page. |
| PCI Green | `#14432E` | Headings, rules, the Standards accent. A deep forest green: authoritative, calm, and legible at small sizes where a bright green would fluoresce. |
| Green mid | `#2D6A4F` | Secondary marks, identifiers. |
| Green tint | `#F2F6F3` | Call-out grounds. Barely there by design — a tint you notice is a tint that is too strong. |
| Rule | `#C8CFC9` | Hairlines. |
| Muted | `#5A6560` | Folios, captions, apparatus. |

Amber `#8A5A00` is retained for cautions only, because a caution that shares the accent colour stops
being a caution.

## 4. Tables

Premium print tables are made of **space and hairlines**, not fills. Specifically:

- **No zebra striping.** It is a screen convention. On paper it stripes the page and fights the text.
- **No filled header.** A hairline above and below the header row, with the header set in Biolinum
  small caps, does the same work without the weight.
- **No vertical rules.** Columns are separated by space; if they need a line, they are too tight.
- One closing rule at the foot of the table.
- Generous cell padding — at least 1.6 mm vertical.
- Numeric columns right-aligned on tabular figures.

## 5. Call-outs

The four call-out classes keep their written label, their icon-free identifier and their distinct
border treatment, so they remain distinguishable in greyscale, on screen readers and in monochrome
print. Colour is never the only distinction.

| Call-out | Label | Rule | Ground |
|---|---|---|---|
| PCI Standard | `PCI STANDARD` + identifier | 2 pt solid green, left | Green tint |
| External reference | `EXTERNAL STANDARD OR FRAMEWORK` | 1 pt double, left | none |
| Practice guidance | `PCI RECOMMENDED PRACTICE` | 1 pt dashed, left | none |
| Caution | `CAUTION` | 1 pt dotted, left | amber tint |

**No `§` and no pictographic icons.** A section sign in front of an identifier that is already
displayed adds nothing, and glyph availability across fonts and readers is a liability — a marker
that renders as a blank box in one environment is worse than no marker. The label and the identifier
carry the meaning.

## 6. Openers

A chapter opener earns its page with space, not ornament. Roughly the top third stays empty; the
domain number sits large in the display cut, muted; the title follows in display; a hairline closes
the block. No rules above, no boxes, no reversed panels.

Part dividers get a full page: number, title, one line of orientation, nothing else.

## 7. What this system forbids

- Emoji and pictographic icons anywhere in a published page.
- Heavy fills behind text.
- More than one accent colour.
- Faux small caps and faux italics.
- Centred body text.
- Underlining for emphasis.
- Any font not in §1.
