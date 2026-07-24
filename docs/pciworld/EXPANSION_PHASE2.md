# PCI World Expansion — Phase 2 delivery report

_Baseline: `main` @ `ae85eab` (Phase 1). Scope: the Phase 2 row of EXPANSION_PHASE0.md §10 — the
premium Passport, the §8 accessibility fix pack with real axe coverage, and the Medium/Low privacy
findings from §7 that Phase 1 deferred to the phase that touches these code paths._

## 1. Premium Passport

A Passport is only worth something if three claims hold. Each is now implemented rather than
asserted:

**Verifiable.** Every published Passport carries a QR code and its own address, and there is a
verification entry point at `/world/verify` where someone handed a document can paste the link or
code and land on the live record. A one-page PDF (`/world/p/{token}.pdf`) states plainly that the
live record — not the file — is the authority and that its owner can withdraw it. QR and PDF are
drawn with no native dependencies (QRCoder's module matrix, the `CertPdf` construction), so output
is byte-deterministic and the container needs nothing installed.

**Consented, per field.** Publishing *what* you have practised no longer forces you to publish your
scores. `passport_show_scores`, `passport_show_profiles` and `passport_show_dates` gate the public
page, the table headers and the PDF alike — a value the owner did not publish never reaches the
rendered output at all, rather than being hidden in CSS. Titles, industry and difficulty are always
shown, because without them a Passport says nothing. Defaults preserve the behaviour of every
Passport published before these columns existed.

**Withdrawable.** Links can now expire (90 days / 6 months / 12 months / never), and expiry
collapses into the same 404 as "never existed" — a viewer must not be able to distinguish
*withdrawn* from *never was*, which would leak that a Passport once lived at that address. The
verification page follows the same rule: it says the link does not resolve and lists the ordinary
explanations without asserting which applies. Since the token is stored only as a SHA-256, the
server genuinely cannot re-display a link; the browser that minted it remembers it, and from
anywhere else the honest answer is "generate a new link", which rotates and retires the old one.

## 2. Accessibility

The §8 ranked gaps, closed:

| # | Gap | Fix |
|---|---|---|
| 1 | Skip link stayed invisible when focused (2.4.7 fail) | `a.visually-hidden:focus` restores it as the first visible control; `#main` takes `tabindex="-1"` so activating it really moves focus |
| 2 | Focus destroyed on every state transition | Start, submit and both auth toggles move focus into the panel that just appeared, in the public app and the admin |
| 3 | `.dim .kicker` 4.42:1 on white (AA fail) | `--slate` (7.59:1) — these labels carry the meaning of the numbers beside them |
| 4 | Focus ring 2.76:1 on noir surfaces (1.4.11 fail) | light ring (`#93C5FD`) on dark surfaces, in both apps |
| 5 | Form-field borders 1.23:1 (1.4.11 fail) | new `--field` token at 3.03:1 for anything a person types into; `--line` stays for decorative edges |
| 6 | `#result` was a whole-page live region; errors went to a polite region | live region removed and focus moved there instead; submit failures go to a `role="alert"`; the form no longer describes itself with a live region |
| 7 | `window.prompt()` collected a password in clear text | a labelled, masked in-page confirmation panel |
| 8 | Admin: no `h1`, no skip link, broken ARIA tabs | `h1`, skip link, and the complete tabs pattern — `aria-controls`, `role="tabpanel"`, `aria-labelledby`, roving `tabindex`, arrow/Home/End keys |
| 9 | Smooth scroll ignored `prefers-reduced-motion` | gated on `matchMedia`; the admin gained the reduced-motion block it lacked entirely |
| 10 | Missing table captions | added on the archive and Passport tables |

**Why these survived the original build:** the axe scan covered `/world` only, and every one of
the contrast failures renders on the result, Passport and account pages. The E2E now scans six
surfaces plus a genuinely completed result view, and adds two keyboard tests — the skip link, and
focus retention across start/submit — that no automated scanner can catch.

## 3. Privacy findings closed

- **Deletion is now de-identification, not unlinking.** Detaching `user_id` left `session_id` in
  place — a durable pseudonym still linking every attempt to each other, to the browser holding the
  raw token, and to any content report filed from it. Deletion now also nulls answer text, clears
  the session linkage, deletes the session rows, revokes every invitation minted from those
  attempts, and unlinks reports and analytics events. Scores survive, unlinked, as the anonymous
  statistics the content-quality gates depend on.
- **A retention sweep exists.** Nothing in the world realm expired before: anonymous sessions had
  no expiry, used tokens were never collected, and `pciworld_events` grew unbounded. The new
  `WorldRetentionService` removes expired sessions and tokens, dormant anonymous sessions that own
  no attempts, and old analytics rows. Windows are operator-settable and `0` disables a sweep, per
  the platform convention — a stray zero must never mean "delete everything".
- **Invitations are actually revocable.** `revoked` was only ever read; nothing could set it. There
  is now an owner-authenticated revoke endpoint, enforced in SQL.
- **The data export works and is complete.** It was rendered as a plain link, which sends no auth
  header and so always answered 401. It is now fetched with credentials and saved as a file, and
  it includes the answers given and any content reports filed — a copy of their data, not a summary.

One thing worth recording: adding answers to the shared evidence query made an existing test fail,
correctly. That query feeds the public Passport and the PDF, so anything selected into it is one
careless interpolation from publication. The export got its own query instead.

## 4. Test evidence

- .NET: **700 passed / 0 failed** — 54 PCI World, including 9 new Passport/privacy tests.
- Python integration: **1124 / 1124**.
- Playwright PCI World: **17 / 17** — 7 original, plus 6 axe scans, 2 keyboard tests, the Passport
  disclosure journey and the verification-page privacy check.

## 5. Not in this phase

- **Share-asset images** (OG/square/story cards). Deferred deliberately rather than half-built:
  every platform that matters rejects SVG for `og:image`, and rasterising server-side needs either
  a native dependency added to the container (SkiaSharp + fonts) or a hand-written PNG encoder with
  an embedded bitmap font. That is a real technical decision with a deployment cost, and it belongs
  next to the SEO work in Phase 7 where `og:image` plumbing lands anyway.
- **TOTP for world admins** — still open, carried to the admin-hardening slice.
- **Referral analytics** beyond the existing invite events.
- No content was added: the bank is still the 30 pilot challenges.

## 6. Open decisions (unchanged)

1. **Managed MySQL 8 provider + credentials** — still the launch gate.
2. Institute URL mapping for contextual links.
3. Named editorial authors/reviewers for the blog/news programme.
4. Company-logo permissions (default: none).
5. Arabic review capacity before the localization phase exits.
