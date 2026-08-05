# S09 — Best Practice Guides

**20 documents · prefix `BPG` · [registry](../00-framework/ASSET-REGISTRY.md#s09--best-practice-guides-20--prefix-bpg)**

The working core of the programme and the weekly engine of the publishing calendar. These are the
documents a practitioner keeps.

## The standard every guide meets

Each guide carries three things that are not optional:

1. **A fully worked numerical example**, arithmetic independently recomputed, substitutions shown, units
   and rounding and assumptions stated, labelled *Illustrative figures.*
2. **A `How this goes wrong` section** — concrete failure modes. This is the section practitioners
   screenshot, and the one that proves the document was written by someone who has seen the work done
   badly.
3. **A usable checklist** — the thing a reader takes into a meeting, not a summary of the document in
   bullet form.

## Why the arithmetic matters more here than anywhere else

This series is where the Institute's technical credibility is either established or lost. A guide with a
wrong CPI is not a typo; it is evidence that the standard behind the credential was not checked. Every
figure is recomputed before it is written, and numerical content gets two reviewers
(`GOVERNANCE-AND-REVIEW.md` §3 gate 4).

`BPG-09` deserves particular care: all four estimate-at-completion methods are computed from **one shared
data set**, so the reader sees the spread rather than four disconnected examples. The point of that
document is that the formula was never the hard part — choosing and defending the assumption is.

## Cautions specific to this series

- Naming a standard (IFRS 15, IAS 37, ISO 31000, PMBOK, AACE TCM) and explaining its principle **in our
  own words** is correct. Reproducing its text, tables or clause numbering is prohibited.
- **`BPG-12` (claims and extension of time) is the highest legal-risk document in the framework.**
  Entitlement depends on the contract and the governing law. Describe competing approaches to concurrency
  without asserting one jurisdiction's position as settled; carry a plain "not legal advice" line. The
  same applies to `BPG-11`.
- Where a threshold is convention rather than rule — schedule-quality metrics in `BPG-05`, distribution
  and correlation choices in `BPG-17` — say so rather than attributing a number to a named standard.
- No salary or market figures. Those belong to S08 alone.

## Pairing with the templates

Eleven of these guides ship alongside an S10 template, and the guide always ships first: the method earns
the instrument (`PUBLISHING-CALENDAR.md` sequencing rule 1).
