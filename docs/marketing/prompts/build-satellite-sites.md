# Prompt — build two static satellite sites with a blog admin

Copy everything below the line into Claude Code, in a fresh repository.

---

Build two small, fast, static websites — one per domain — plus one minimal admin for writing
blog posts. No application backend serves the public pages: every public URL is a static file.

## The two sites

**SET THESE TWO VALUES FIRST and use them throughout. Everything else in this prompt is
written to work for either.**

| | Site A | Site B |
|---|---|---|
| Domain | `pciworld.org` | `pciglobal.ai` |
| Subject it owns | Careers and community in project controls — career paths, role guides, salary questions, interview preparation, what the work actually involves | Regional and market-specific guidance — country and region guides for the Gulf, India, the UK, the US, and local market context |
| Audience | Practitioners deciding what to do next in their career | Practitioners in a specific market working out what applies where they are |

Both are operated by **Project Controls Institute Global**, whose main site is
`https://projectcontrolsinstitute.org` ("the hub"). These two are not competitors to the hub
and must never duplicate it.

## Do NOT redirect the home page, and this matters more than it sounds

A brief for this work asked for the home page to 301 to the hub. **Do not do that.** A domain
whose home page redirects away while the rest of it hosts blog posts is the textbook doorway-site
pattern: the domain has no identity of its own, exists only to funnel, and search engines treat
it accordingly. It would destroy the visibility the sites are being built to gain.

Build instead a **real home page** that:

- says in one sentence what this site is and what subject it covers;
- says plainly, above the fold, that it is operated by Project Controls Institute Global, with a
  prominent link to `https://projectcontrolsinstitute.org`;
- lists the most recent and the most useful posts;
- carries the site's own navigation.

That gives the domain a genuine identity, sends the visitor to the hub anyway, and is safe.

## Stack — keep it boring

- **Public output: plain static HTML and CSS.** No React, no Vue, no Tailwind CDN, no build
  framework, no client-side rendering. A visitor with JavaScript disabled must see every word.
- **One small static site generator**, written in Python 3 with no dependencies beyond the
  standard library (write the Markdown-to-HTML conversion yourself; the content uses only
  headings, paragraphs, bold, italic, links, lists, block quotes, tables and fenced code).
- **Source content is Markdown files** with YAML-style front matter in `content/posts/`.
- **Output is a `public/` directory** of finished HTML, ready to serve from any static host —
  Netlify, Cloudflare Pages, GitHub Pages, S3, or nginx. No server required to view it.
- Total page weight under 100 KB, no web fonts loaded from a third party unless self-hosted,
  no analytics scripts, no cookie banner needed because there are no cookies.

## The blog admin — the only piece with a server

A single small Python application (Flask or the standard library's `http.server`, your choice,
but keep it under ~400 lines) that runs **locally or on a private host, never on the public
domain**. It does exactly four things:

1. Lists existing posts.
2. Creates a new post: a form for the front-matter fields plus a Markdown body textarea, with a
   live character count on the title and meta description showing whether each is inside its
   target range.
3. Edits an existing post.
4. Runs the generator and reports what it wrote.

Requirements:

- **Authentication:** a single password read from the `BLOG_ADMIN_PASSWORD` environment
  variable. Never hard-code a password, never commit one, never accept one as a command-line
  argument. If the variable is unset, refuse to start and say so.
- Bind to `127.0.0.1` by default.
- It writes Markdown files and nothing else. It never touches `public/` except by invoking the
  generator.
- No database. The Markdown files are the source of truth and belong in version control.

## Front matter every post carries

```yaml
title:        # 50-60 characters
meta:         # 140-158 characters
slug:         # lowercase, hyphenated, no dates in the URL
published:    # YYYY-MM-DD
updated:      # YYYY-MM-DD
summary:      # 1-2 sentences, used on listing pages
tags:         # 2-5, lowercase
schema:       # Article | FAQPage
```

Validate all of it at build time. Fail the build loudly on a missing field, a duplicate slug, a
title or meta outside its range, or a link to a URL not on the allowed list below. A silent
failure here ships a broken page.

## SEO — this is the whole point of the sites, so do it properly

Every generated page carries, in the `<head>`:

- `<title>` from the front matter, and one `<h1>` in the body that is not a duplicate of it.
- `<meta name="description">`.
- `<link rel="canonical">` — absolute, self-referencing, on this site's own domain.
- `<meta name="robots" content="index,follow,max-image-preview:large,max-snippet:-1">`.
- Open Graph: `og:type`, `og:title`, `og:description`, `og:url`, `og:site_name`, `og:locale`.
- Twitter: `summary_large_image` with title and description.
- JSON-LD in an `@graph`, containing:
  - `Article` with `headline`, `description`, `datePublished`, `dateModified`, `inLanguage:
    en-GB`, `mainEntityOfPage`, and `publisher` as an `Organization` named
    "Project Controls Institute Global" with `url` pointing at the hub.
  - `BreadcrumbList` for the path.
  - `FAQPage` **only** where the post genuinely contains question-and-answer pairs — parse them
    out of the body (a bolded question followed by its answer paragraph) and emit real
    `Question` and `Answer` entities. Do not emit an empty or invented `FAQPage`; declaring one
    a page does not have is worse than declaring none.
  - `Organization` with a `sameAs` array listing the Institute's real, verifiable public
    profiles — this is how the two domains are associated with the hub as one entity.

Also generate, at the site root:

- `sitemap.xml` listing every page with `lastmod` from the front matter.
- `robots.txt` allowing everything, naming the sitemap, and carrying explicit stanzas for the
  AI crawlers — `OAI-SearchBot`, `ChatGPT-User`, `PerplexityBot`, `ClaudeBot`,
  `Google-Extended`, `Bingbot`, `GPTBot`, `CCBot`. **A named user-agent group does not inherit
  the rules from `User-agent: *`**, so any path you disallow must be repeated inside each named
  group or you will have opened it to that crawler alone.
- `llms.txt` following the llmstxt.org convention: what this site is, who operates it, and a
  linked list of the main posts.
- An RSS feed at `/feed.xml`.
- A `404.html`.

Body requirements, because these decide whether a page is ever cited:

- The post's own question answered in the **first 40–60 words**, before any preamble.
- Paragraphs of two to three sentences. Nothing over 90 words.
- One `<h1>`; `<h2>` and `<h3>` beneath it with no skipped levels.
- A comparison table wherever the subject genuinely has axes.
- Images with real alt text describing the image, and explicit `width`/`height` to stop layout
  shift.
- Tables inside a container with `overflow-x: auto`, so a wide table never makes the page scroll
  sideways on a phone.

## Linking rules — read these carefully, they are not optional

The Institute operates five domains: `projectcontrolsinstitute.org`, `pciai.org`,
`pciglobal.ai`, `pciworld.org`, `credentialfinder.org`.

1. **A post may link to at most one other Institute domain, once.** Never two links to the same
   external domain, never links to three or more of them from one post.
2. **Never link to all five.** Five commonly-owned domains each carrying a block of links to the
   other four is the private-blog-network pattern; the cost of it is all five losing standing at
   the same time, not one page underperforming.
3. **Every post carries two to three internal links** to other posts on its own domain. These
   are what actually build the site's topical authority and they carry no risk.
4. **No "our other sites" block, no reciprocal footer, no link list.** Links sit inside
   sentences, where the sentence raises a question the target answers.
5. Anchor text describes the destination, varies between posts, and is never a bare domain or
   "click here".
6. **Never link to any other certifying body, awarding organisation or training provider.**
7. Validate every link at build time against an allow-list of the five domains plus the site's
   own real slugs. **Fail the build on a link to a page that does not exist.**

## Content rules — absolute, because this is a certifying body

- **Never claim or imply accreditation, recognition, endorsement, affiliation or partnership.**
  The Institute holds none. Where the subject comes up, state plainly: "PCI is a new,
  independent certifying body. It is not accredited by ANAB, UKAS, IAS or any other ISO/IEC
  17024 accreditation body, and does not claim to be."
- **Never name another certifying body, credential or qualification** — not in a title, heading,
  table, sentence, meta description, tag or link. Write about categories described by what they
  examine: "cost and scheduling credentials", "accountancy and finance qualifications", "audit
  credentials", "project management certifications".
- **Never publish** a pass rate, a student number, a holder number, a salary figure, an
  examination weighting, or any fee or exam specific belonging to another organisation.
- **Never invent** a statistic, a testimonial, a case study or a source. If a number cannot be
  pointed at, the sentence goes.
- Every page states, in the footer, that the site is operated by Project Controls Institute
  Global, and that nothing published is legal, tax or accounting advice.

## Design

Take the visual identity from the Institute so a visitor moving between the sites and the hub
does not feel they have left:

- Navy `#1D3C92` to `#13245A`, gold accent `#B8923E`, a crimson rule `#C13329`.
- Neutrals biased slightly toward the navy rather than pure grey.
- A display face and a reading face, self-hosted or from Google Fonts with a real fallback
  stack. Reading column capped around 68 characters.
- Light and dark themes driven by CSS custom properties: define the full light palette on bare
  `:root`, redefine only the tokens under `@media (prefers-color-scheme: dark)`. Never declare a
  colour only inside a media block. Give `body` an explicit background.
- Responsive by default, keyboard focus visible, `prefers-reduced-motion` respected.

## What to deliver

```
content/posts/*.md         the source posts
build.py                   the generator: markdown -> public/
admin/app.py               the blog admin (password from BLOG_ADMIN_PASSWORD)
templates/                 the HTML templates
static/                    css, any images
public/                    generated output, gitignored
README.md                  how to write a post, build, and deploy
```

The README must cover: writing a post, running the admin, building, previewing locally, and
deploying to a static host, plus a short section on the linking and claims rules above so
whoever writes the next post is not guessing.

## Finally

Seed each site with **three real posts** on its own subject, written to every rule above, so the
build is exercised end to end and the site is not empty on first deploy. Then run the build,
confirm it passes its own validation, and show me the generated `sitemap.xml`, one full page of
generated HTML, and the output of the link validator.
