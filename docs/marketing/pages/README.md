# Publish-ready pages

347 standalone HTML pages, one per piece, each carrying the head tags and structured data a
search engine and a social crawler actually read. Plus five sitemaps.

Regenerate any time with:

```bash
cd docs/marketing/content && python3 _tools/build_pages.py
```

## What every page carries

| | |
|---|---|
| `<title>` | from the piece's own title, 48–62 characters on the articles |
| `<meta name="description">` | the authored meta, 135–162 characters |
| `<link rel="canonical">` | the real URL, on the 134 pages that have one |
| `<meta name="robots">` | `index,follow,max-image-preview:large,max-snippet:-1` — or `noindex,follow` on the 213 copy sheets |
| Open Graph | `og:type`, `og:title`, `og:description`, `og:url`, `og:site_name`, `og:locale` |
| Twitter | `summary_large_image` card with title and description |
| JSON-LD | an `@graph` carrying `Article`, plus `FAQPage` where the piece has a real FAQ |
| Semantics | one `<h1>`, body headings from `<h2>` down, `<article>`, tables in their own scroll container |
| Links | the estate links embedded in the prose, exactly as audited |

## The structured data is built from the prose, not asserted

Declaring `schema: FAQPage` in front matter tells a crawler nothing. **222 pages carry 1,155
real `Question` and `Answer` entities**, parsed out of the question-and-answer pairs the
articles were written with — a bolded question, then the paragraph that answers it. That is
what a rich result is actually built from.

Every page also carries `Article` with the publisher, the language, the pillar as `about`,
and the primary and secondary keywords. All 347 parse as valid JSON-LD.

## Two things that are deliberate, not missing

**213 pages are `noindex`.** A Bluesky post, an Instagram caption, a journalist pitch and a
LinkedIn article are published on someone else's platform. The HTML here is a formatted copy
sheet for whoever posts it, not a page meant to live at a URL of ours — so it says so, which
keeps 200-odd near-duplicates of published social copy out of any index they get served from
by accident. The 134 pages that will live at a real URL are `index,follow` and carry a
canonical.

**28 pages have no meta description.** They are social pieces with no `meta` field, because a
caption has no meta description. They are all inside the `noindex` set.

## The sitemaps

One per domain, listing only that domain's real, indexable URLs:

| Sitemap | URLs |
|---|---:|
| `sitemap-projectcontrolsinstitute.org.xml` | 55 |
| `sitemap-pciai.org.xml` | 15 |
| `sitemap-credentialfinder.org.xml` | 10 |
| `sitemap-pciglobal.ai.xml` | 10 |
| `sitemap-pciworld.org.xml` | 10 |

They carry the 100 own-site pages. The other 34 indexable pages are republications whose
canonical points at an origin already in one of these sitemaps, so listing them again would
ask a crawler to index a copy in preference to its source.

## Before publishing

The pages are complete as markup. Three things still need a person:

1. **Add `datePublished` and `dateModified`** once each piece has a real publication date.
   They are omitted rather than invented — a fabricated date in structured data is worse than
   no date.
2. **Add an `og:image`** per piece, 1200 × 630. The card markup is there and will use it.
3. **Confirm the slugs** against what the site actually serves. The canonicals are built from
   the filename, and `_tools/link_audit.py` validates every in-body link against the pages
   that exist — but it cannot know a slug you change later.
