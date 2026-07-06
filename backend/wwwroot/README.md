# Project Controls Institute — static site

196 real, individually crawlable pages with per-page SEO. Upload the folder and it works.

---

## Site features added (this update)

These now appear on the live site, ahead of wiring them into the admin panel:

- **Cookie-consent banner** — shows on first visit (bottom of every page), with
  Accept all / Essential only. The choice is remembered in `localStorage` as
  `pci-cookie-consent`, so it won't show again. Links to `/privacy`. Wire your
  analytics to respect the stored value.
- **On-site search** — the search icon in the header (and **⌘K / Ctrl-K**) opens
  a search modal that filters all 195 pages live as you type. It reads
  `search-index.json` (auto-generated from every page's title and description),
  so it stays in sync whenever you regenerate. Esc closes it.
- **Newsletter signup** — a band above the footer on every page. It validates the
  email and shows a confirmation; **connect it to your email provider** (e.g.
  Mailchimp/SendGrid) where marked. Right now it's front-end only and doesn't
  send anywhere.
- **Downloadable files** — `publications` and `white-papers` now have a downloads
  section with file cards. **Add the actual PDFs** to a `/downloads/` folder; the
  links point at `/downloads/*.pdf` (placeholders until you drop the files in).

Still to come (admin-only, no public page): revision history & rollback,
scheduled publishing, editorial workflow, email-template editor, the
role-permission matrix, exam-session scheduling, and analytics. Those go into the
panel next, since they have no front-end page.

---

## Forms

Five forms are ready to wire — `method="post"`, named inputs, validation. Replace
the placeholder `action` on the live site with your endpoint, then delete the
`TODO` comment above each form.

| Page | Replace this `action` | Fields (POST) |
|---|---|---|
| `contact.html` | `REPLACE_WITH_CONTACT_ENDPOINT` | `name`, `email`, `subject`, `message` |
| `enrol.html` | `REPLACE_WITH_ENROL_ENDPOINT` | `first_name`, `last_name`, `email`, `country`, `background`, `experience` |
| `login.html` | `REPLACE_WITH_LOGIN_ENDPOINT` | `email`, `password`, `remember` |
| `forgot-password.html` | `REPLACE_WITH_FORGOT_ENDPOINT` | `email` |
| `reset-password.html` | `REPLACE_WITH_RESET_ENDPOINT` | `password`, `password_confirm`, `token` (hidden) |

---

## SEO

Per-page `title`/description/canonical/Open Graph in static HTML; CSS externalised
once to `assets/styles.css`; ~1,460 internal links on real paths; `sitemap.xml`,
`robots.txt`, per-page JSON-LD, a `noindex` 404, a homepage redirect for old
`/#…` deep links, and now `search-index.json` for on-site search.

## Deploy (clean URLs)

- **Netlify** — drag the folder in, or `netlify deploy --prod --dir .`
- **Cloudflare Pages** — `wrangler pages deploy .`
- **Vercel** — `vercel --prod`

`/certification.html` 301-redirects to `/certification` on all three.

## Before launch

- Add `assets/logo.png`, `assets/og-image.jpg`, and the `/downloads/*.pdf` files.
- Wire the newsletter to your email provider, and the five form endpoints.
- Give `sector-government.html` or `government-programs.html` a distinct title
  (they currently share one).

## Regenerating

`generate.py` builds this whole site from the original single file. Re-running
keeps the shared chrome, the forms, the page content, and all the new features
(cookie banner, search, newsletter, downloads) in sync across all pages.
