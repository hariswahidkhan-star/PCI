---
platform:      Pinterest (standard image Pin, 1000 × 1500)
type:          pin
title:         Earned Value Cheat Sheet: The Accrual That Moves CPI 1.19 to 1.05
when_to_post:  Launch week, any weekday, 20:00–22:00 UK. Pinterest is a search index before it is a feed, so the hour barely matters and the keyword match does. Claim the domain and publish the destination page's Open Graph tags before the Pin goes up, not after — a Pin published ahead of its Rich Pin data keeps the plain version for its whole life. Judge it at ninety days, not at seventy-two hours.
char_count:    65 (title) · 484 (description, of the 500 field) · 404 (alt text)
word_count:    76 on the graphic
hashtags:      None. See notes — Pinterest retired hashtag search and they now cost a Pin description space that keywords should be holding.
cta_link:      https://projectcontrolsinstitute.org/body-of-knowledge
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
notes: |
  Pinterest indexes the title, the description, the alt text, the board name and the board
  description, and it reads the image itself. All six carry "earned value" or "cost performance
  index" in a sentence a person would actually write. None of them carries a keyword twice.

  Hashtags: zero, deliberately. Pinterest removed hashtag search, so a hashtag is now plain text
  occupying characters in a 500-character field that keywords should be holding. If a board owner
  insists, one at the very end and never more.

  The graphic is the whole asset. A Pin is judged on whether a stranger saves it to a board they
  will come back to, and nobody saves a slogan. They save arithmetic they can run on their own
  numbers, which is why the card carries the formula, the four inputs and both results with the
  division shown, and why the copy stops at 76 words.

  The full credential name is spelled out in the description even though it costs characters in a
  500-character field. A Pin is the one asset here that arrives with no post around it: it is saved,
  re-boarded and re-described by strangers years later, so "PCL-AI" alone would reach a reader with
  nothing to resolve it against.

  The 15,613 machine calculation checks that run against PFL-AI and PML-AI are absent on purpose:
  the figure is only true alongside that scope, and Pinterest images get cropped, re-uploaded and
  re-described by strangers. A number that a crop can separate from its scope does not belong on a
  Pin.

  The Body of Knowledge's 40/40/20 proportions are absent for the same reason. They describe the
  Body of Knowledge and never the examination, and those digits sitting beside a credential code in
  a cropped image would read as an exam weighting to exactly the senior reader we want. No
  examination weighting appears anywhere, in the image, the description, the alt text or any reply.

  The figures are illustrative arithmetic. Never attach a currency symbol, a client, a sector, a
  country or a date, and never answer "how often does this happen" with a number, because no
  researched frequency exists to cite.

  Comments and reshares: answer in arithmetic. If someone says the accrual belongs in the ledger and
  not in earned value, they have made the Pin's point and should be told so.
---

# Pinterest flagship — the CPI accrual card

## 1. Pin title

As entered, 65 characters:

> Earned Value Cheat Sheet: The Accrual That Moves CPI 1.19 to 1.05

The grid truncates at roughly 30 to 40 characters on a phone, which leaves **"Earned Value Cheat Sheet: The A…"** doing the work of the whole title. That is why the keyword sits at character zero and the intrigue sits second. Do not reverse them to make the line read better in this document.

## 2. Pin description

As entered, 484 characters of the 500 the field allows:

> Earned value 2,200,000. Invoiced cost 1,850,000. Cost performance index 1.19, and a cost report that says the job is under budget. Then accrue the 240,000 of work done and not yet invoiced: cost becomes 2,090,000 and CPI reads 1.05. Fourteen points on one missing accrual. The error is accounting. The damage is delivery. Save it as an earned value check for your next month-end, then read the PCI AI Project Controls Leader (PCL-AI) Body of Knowledge: 13 domains, 61 knowledge areas.

Only the first line or so shows before the description collapses, so the arithmetic opens it. The description is not clickable on any Pinterest surface; the destination is the Pin's own link field, set to the URL in `cta_link`.

## 3. Alt text

As entered, 404 characters. Written for a reader using a screen reader, and read by Pinterest as signal:

> A dark reference card headed "One missing accrual. Fourteen CPI points." Below it, a cost performance index worked example: earned value 2,200,000, invoiced cost 1,850,000, work done and not yet invoiced 240,000, cost once accrued 2,090,000. Two large figures compare the results: 2,200,000 divided by 1,850,000 gives a CPI of 1.19 as reported, and 2,200,000 divided by 2,090,000 gives 1.05 once accrued.

---

# 4. The graphic — 1000 × 1500 build spec

Every coordinate below is in pixels on the 1000 × 1500 canvas, measured from the top-left. Type positions are **baselines**, not box tops. Build it at 1×, then export the 2× for archive; never build small and scale up.

## 4.1 Canvas and export

| Property | Value |
|---|---|
| Artboard | 1000 × 1500 px, 2:3, the ratio Pinterest sizes its grid to |
| Colour space | sRGB, no transparency, no ICC profile beyond sRGB |
| Delivery file | PNG-24, `pci-cpi-accrual-card-1000x1500.png`, target under 1.5 MB (the platform ceiling is 20 MB; a heavy file only slows the grid) |
| Archive file | 2000 × 3000 PNG, same name with `@2x` |
| Never | animated, transparent, bordered by a fake device frame, or padded to 1000 × 1000 |

## 4.2 Grid and safe area

- Left and right margins 72 px. Content column 856 px wide, x = 72 to x = 928.
- Top margin 84 px, bottom margin 68 px.
- **Keep the top-right corner clear of type above y = 170.** Pinterest floats its save control there on close-up, and a covered word is a covered word.
- One column throughout. No content bleeds to the canvas edge; a Pin that ends in a hard edge of colour looks like an advert, and adverts do not get saved.

## 4.3 Palette

| Role | Hex | Where | Contrast |
|---|---|---|---|
| Ground | `#0E1113` | full canvas | — |
| Card | `#15191C` | the arithmetic panel | — |
| Card border | `#232A2F` | 1 px inset on the panel | — |
| Primary type | `#F5F3EF` | headline, figures, labels | 17.1:1 on ground |
| Secondary type | `#B6BCC1` | closing line 2 | 9.9:1 on ground |
| Muted type | `#8B9298` | eyebrow, captions, footer | 6.0:1 on ground |
| Rules | `#262C31` | dividers | — |
| Accent | `#E4B95F` | the true CPI, and the check line | 9.6:1 on card |

One accent, used twice. A near-black Pin is the highest-contrast thing in a white grid, which is the entire reason for the ground colour.

## 4.4 Typefaces

| Use | Face | Fallbacks |
|---|---|---|
| Headline, closing | Inter SemiBold | Helvetica Neue Bold, Arial Bold |
| Labels, eyebrow, footer | Inter Medium | Helvetica Neue Medium, Arial |
| All figures | IBM Plex Mono Medium | SF Mono, Roboto Mono, Menlo |

Set `tnum` (tabular figures) on every numeral so the value column aligns on the comma. Set the eyebrow and the stat labels in uppercase with 0.16 em tracking; everything else at 0 to −0.01 em.

## 4.5 The build, band by band

**Eyebrow** — baseline y = 148, x = 72, 22 px Inter Medium, `#8B9298`, uppercase, 0.16 em tracking.

> EARNED VALUE · A WORKED EXAMPLE

**Headline** — two lines, 72 px Inter SemiBold, `#F5F3EF`, 82 px leading, x = 72. Baselines y = 272 and y = 354.

> One missing accrual.
> Fourteen CPI points.

**Rule** — y = 420, x = 72 to 928, 2 px, `#262C31`.

**The card** — x = 72 to 928, y = 452 to 1204. Fill `#15191C`, 1 px inset border `#232A2F`, corner radius 8 px. Inner padding 44 px, so text runs x = 116 to x = 884.

- **Formula caption** — baseline y = 532, x = 116, 24 px Inter Medium, `#8B9298`.

  > CPI = earned value ÷ actual cost

- **Hairline** — y = 560, x = 116 to 884, 1 px, `#262C31`.

- **Four input rows.** Label left at x = 116, 28 px Inter Medium, `#B6BCC1`. Value right-aligned to x = 884, 46 px IBM Plex Mono Medium, `#F5F3EF`. Baselines y = 626, 700, 774, 848.

  | Label | Value |
  |---|---|
  | Earned value | 2,200,000 |
  | Invoiced cost | 1,850,000 |
  | Work done, not yet invoiced | + 240,000 |
  | Cost once the accrual lands | 2,090,000 |

- **Divider** — y = 896, x = 116 to 884, 1 px, `#262C31`.

- **The stat pair** — the band y = 928 to 1160, split by a 1 px vertical rule at x = 500 running y = 936 to 1152, colour `#262C31`. Left column centred on x = 302, right column centred on x = 698. Both columns centre-aligned.

  | | Left column | Right column |
  |---|---|---|
  | Eyebrow, baseline y = 962, 22 px Inter Medium, `#8B9298`, uppercase, 0.16 em | AS REPORTED | ONCE ACCRUED |
  | Division, baseline y = 1006, 26 px IBM Plex Mono Medium, `#8B9298` | 2,200,000 ÷ 1,850,000 | 2,200,000 ÷ 2,090,000 |
  | Result, baseline y = 1128, 120 px IBM Plex Mono Medium | 1.19 in `#F5F3EF` | 1.05 in `#E4B95F` |

  The accent goes on 1.05 and nowhere else in this band. The false number is the plain one; the true number is the one that is lit. Reverse that and the card teaches the wrong thing.

**Closing** — x = 72, left-aligned, three lines with their own weights and colours.

- y = 1258, 34 px Inter SemiBold, `#F5F3EF`: **Nobody was careless.**
- y = 1302, 30 px Inter Medium, `#B6BCC1`: **The error is accounting. The damage is delivery.**
- y = 1346, 30 px Inter Medium, `#E4B95F`: **The check: what is done and not yet invoiced?**

**Footer** — rule at y = 1384, x = 72 to 928, 1 px, `#262C31`. Text baseline y = 1432, 22 px Inter Medium, `#8B9298`. Left-aligned at x = 72 and right-aligned to x = 928 on the same line.

> PCL-AI · 13 domains · 61 knowledge areas          projectcontrolsinstitute.org

The wordmark belongs here and only here. Pinterest is the one platform where a discreet source line at the foot helps rather than hurts, because a saved Pin travels for years and arrives at strangers with no caption attached. It does not belong across the arithmetic.

## 4.6 The thumbnail test

Export, scale the file to 236 px wide, and look at it from a normal viewing distance. Three things must survive: **1.19**, **1.05**, and the two-line headline. If the 120 px figures have gone soft, the leading in the stat band is wrong, not the size. If nothing else survives, that is correct and intended.

Second test: crop the bottom third off and check that what remains still states nothing false. It does, because the only claims in the top two-thirds are arithmetic.

---

# 5. Destination, board and Rich Pin setup

**Destination.** One link, the PCL-AI Body of Knowledge page on `projectcontrolsinstitute.org`. The hub owns earned value and the credentials, so a Pin about a CPI calculation has no honest reason to point anywhere else in the estate.

**Rich Pins.** Claim `projectcontrolsinstitute.org` in Pinterest business settings, then confirm the destination page serves `og:title`, `og:description`, `og:image` and `og:site_name`. With those present the Pin carries the site name and the live page title under the image, which is the difference between an image somebody saved and a source somebody trusts. Publish the tags first: Pinterest reads them at pin time.

**Board.** Pin it to a board named for the search, not for the brand. "Earned Value & Project Controls" with a board description that says in plain sentences what the board collects, including "earned value management", "cost performance index" and "project controls". Board names and descriptions are indexed; a board called "Our Content" is a board nobody finds.

**Measurement.** Saves and outbound clicks, at ninety days. Impressions in the first week are noise, because a new Pin from an unclaimed-yesterday domain is still being classified.

---

# 6. Fresh Pins, not repins

Pinterest ranks fresh images and discounts the same image posted again. So when this needs a second run, build a **new** card against the same URL rather than repinning this one. Two that would work, and neither needs new claims:

- **The two-boxes card.** Earned value in one box, the ledger in the other, and 240,000 sitting in the gap between them, touching neither. Same palette, same footer.
- **The four EAC methods card.** A comparison table of the four EAC methods against the assumption each one makes. It is the discipline's own material, it is genuinely the most saveable thing in cost engineering, and no good version of it exists on Pinterest.

Do not build a variant that changes the arithmetic, the credential names or the ask. The format changes; the numbers never do.

---

*Links: one, and it is the Pin's own destination field pointing at [the PCL-AI Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) on projectcontrolsinstitute.org, because the closing line asks a reader what is done and not yet invoiced, and the 13 domains and 61 knowledge areas that examine both sides of that question live on that page. A second link on a Pin is not possible and should not be simulated by putting a URL in the description.*
