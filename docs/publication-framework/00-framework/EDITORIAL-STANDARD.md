# Editorial Standard — PCI Publication Framework

**Binding on every one of the 100 documents.** A subset of `docs/books/EDITORIAL_CHARTER.md`, tuned for
short-form public documents that must survive being read by a hostile expert on LinkedIn.

---

## 1. Voice

Professional international **British English**. Complete sentences. Active voice where it reads better.
Confident where we have grounds, qualified where we do not.

Write for a competent practitioner who is short of time and long on scepticism. Assume they have seen
consultancy fluff before and will close the tab the moment they smell it.

**The test every document must pass:** *would a senior project controls manager learn something they could
apply this month, and would they be unable to find it stated this clearly anywhere else?*

---

## 2. Prohibited moves

These are rejection-at-gate defects, not style preferences.

| Prohibited | Why |
|---|---|
| Generic AI-shaped openings — "In today's fast-paced project environment…" | Signals nothing was thought about |
| Motivational filler — "unlock your potential", "take it to the next level" | We teach; we do not cheerlead |
| Unsupported superlatives — "the world's leading", "industry-standard" (of ourselves) | Claims we have not earned |
| Bullet lists where prose is clearer, or bullets of one word | Bullets are for genuinely parallel items |
| Unexplained acronyms on first use | EOT, CBS, QSRA and the rest are expanded once, then used |
| Constant references to "the exam" in teaching documents | Teaching stands on its own merit |
| Fabricated quotations, case studies, company names, project data | Absolute prohibition, no exceptions |
| Invented statistics, salary figures, adoption percentages | See §4 |
| Implying endorsement by a standards body, employer or vendor | Absolute prohibition |
| ™ or ® symbols in PCI branding | Current platform policy |
| Repetition of the same definition across documents in a series | Define once, cross-reference after |
| A closing paragraph that only summarises what was just said | End on the consequence, not a recap |

---

## 3. Structure and length

Every document follows `DOCUMENT-TEMPLATE.md`. Beyond that:

- **Best Practice Guides (S09)** — 2,000–3,500 words. At least one fully worked numerical example and one
  "how this goes wrong" section.
- **Guides and frameworks (S02, S03, S07)** — 1,500–3,000 words.
- **Governance documents (S04, S05, S06)** — as long as completeness requires; clauses numbered for
  citation.
- **Templates (S10)** — the template *is* the deliverable. Short instructional preamble, then the
  structure itself, then completion notes and a worked fragment.
- **Executive summaries (S01)** — 800–1,500 words. Density is the point.

Length is an output check, never a writing method. Padding is a rejection defect.

---

## 4. Evidence and claims

Four kinds of statement, and each is marked so the reader knows which they are reading:

1. **Fact** — verifiable, and where it matters, cited. Standards are named (IFRS 15, IAS 37, ISO 31000,
   PMBOK, AACE TCM, the Scrum Guide) and their principles explained **in our own words**. Never reproduce
   protected text, tables, diagrams or question banks.
2. **Recommended practice** — what the Institute advises, stated as advice.
3. **Professional judgement** — where competent practitioners legitimately differ. Say so, and give the
   grounds for choosing.
4. **PCI interpretation** — our position, labelled as ours.

**Never invent:** standards, clause numbers, laws, company facts, project data, quotations, citations,
survey results, salary bands, market sizes, adoption rates.

**Placeholders.** Any operational or market number not yet confirmed is written as
`[CONFIRM: what is needed]`. A document may ship to review with placeholders; it may not ship to
publication with them. See `README.md` §4.

**Illustrative figures.** Worked examples are the backbone of our teaching and are always permitted.
Label them: *"Illustrative figures."* A reader must never mistake a teaching example for market data or
for a real project.

---

## 5. Arithmetic discipline

Every number in a worked example is independently verified before submission.

- State units, currency, period and rounding.
- Show the substitution, not just the result: `CPI = EV ÷ AC = 2,200,000 ÷ 2,090,000 = 1.053`.
- Currency-neutral where possible. Where a currency is needed, use a generic unit or state the currency
  explicitly; never imply a jurisdiction's tax or accounting treatment is universal.
- Percentages: state the base. "12 % over" is meaningless without "of what".
- If a calculation depends on an assumption, name the assumption in the same breath as the answer.

A document containing an unverified calculation is rejected at gate.

---

## 6. Jurisdiction and neutrality

Globally applicable, cross-industry, jurisdiction-neutral. Where legal, tax or accounting treatment
varies, say that it varies and describe the principle rather than one country's rule. Where an example
needs a legal frame, state the frame as an assumption of the example.

---

## 7. Cross-referencing

Documents in this framework form a library, not a pile.

- Reference sibling documents by ID and title: *see `BPG-09 — Estimate at Completion`*.
- The `related:` front-matter field lists every document a reader should go to next.
- Where a topic is owned by another document, link rather than restate. Duplication across documents is a
  defect.
- The Body of Knowledge (`docs/bok/`) is cited by domain: *BoK Domain 6 (EVM/EAC)*.

---

## 8. Accessibility

- Tables have header rows and are readable linearly.
- Every figure or diagram carries a caption and alt text.
- No meaning is carried by colour alone.
- Headings are properly nested — never skip a level for visual effect.
- Plain-language summary at the top of every document (the template's *In one paragraph* block).

---

## 9. LinkedIn adaptation

Every document carries a `linkedin:` block in its front matter with a hook and format. The adaptation
rules — how a 3,000-word guide becomes a 200-word post without becoming a lie — are in
`LINKEDIN-PLAYBOOK.md`. Two rules bind here:

1. **The post must be true on its own.** A hook that only works because the reader has not yet read the
   caveat is a defect.
2. **The teaching goes in the post, not behind the click.** We publish; we do not tease.

---

## 10. Per-document checklist

Before a draft is submitted:

- [ ] Front matter complete and schema-valid (`FRONT-MATTER-SCHEMA.md`)
- [ ] Template sections all present, in order
- [ ] Opens on substance — no throat-clearing paragraph
- [ ] Every calculation independently verified, substitutions shown
- [ ] Every unconfirmed number is a `[CONFIRM: …]` placeholder
- [ ] Illustrative examples labelled as illustrative
- [ ] No fabricated source, quotation, company, project or statistic
- [ ] Standards named, explained in our own words, never reproduced
- [ ] Acronyms expanded on first use
- [ ] No duplication of a sibling document's owned topic
- [ ] `related:` populated with real IDs that exist in the registry
- [ ] LinkedIn hook is true standing alone
- [ ] British English throughout; no ™/® in PCI branding
- [ ] Closes on a consequence, not a summary
