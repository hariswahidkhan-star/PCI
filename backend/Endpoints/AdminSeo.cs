using System.Text.RegularExpressions;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → SEO (master-plan Phase 3). Groups the SEO controls under /api/admin/seo/... :
///
///   GET  /api/admin/seo/overview   — headline counts + issue summary (sitemap, noindex, redirects)
///   GET  /api/admin/seo/pages      — every page's SEO fields + per-page issue flags
///   GET  /api/admin/seo/redirects  — managed path redirects
///   POST /api/admin/seo/redirects  — create (rejects self-redirects and chains at write time)
///   POST /api/admin/seo/redirects/{id}/delete
///   GET  /api/admin/seo/audit      — practical site audit: missing/duplicate metadata, missing H1,
///                                    missing alt text, missing canonical/JSON-LD, broken internal
///                                    links, redirect problems. Page count is detected dynamically.
///
/// Page SEO edits reuse the existing PATCH /api/admin/pages/{id} (extended with canonical_url and
/// og_image) — one write path, no duplicate module. Gated by the existing 'pages' permission; every
/// mutation is audit-logged.
/// </summary>
public static class AdminSeo
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, string webRoot)
    {
        IResult J(object o) => Results.Json(o);
        IResult? Deny(HttpRequest req)
        {
            var r = gate(req, "pages", _ => Results.Ok());
            return r is Microsoft.AspNetCore.Http.HttpResults.Ok ? null : r;
        }

        // ---------- overview ----------
        app.MapGet("/api/admin/seo/overview", (HttpRequest req) => gate(req, "pages", _ =>
        {
            long C(string sql) => db.Scalar<long>(sql);
            var pages = C("SELECT COUNT(*) FROM pages");
            var published = C("SELECT COUNT(*) FROM pages WHERE published=1");
            var noindex = C("SELECT COUNT(*) FROM pages WHERE noindex=1");
            var missingTitle = C("SELECT COUNT(*) FROM pages WHERE published=1 AND (title IS NULL OR title='')");
            var missingMeta = C("SELECT COUNT(*) FROM pages WHERE published=1 AND (meta_description IS NULL OR meta_description='')");
            var dupTitles = C("SELECT COUNT(*) FROM (SELECT title FROM pages WHERE published=1 AND title IS NOT NULL AND title<>'' GROUP BY title HAVING COUNT(*)>1) d");
            var dupMetas = C("SELECT COUNT(*) FROM (SELECT meta_description FROM pages WHERE published=1 AND meta_description IS NOT NULL AND meta_description<>'' GROUP BY meta_description HAVING COUNT(*)>1) d");
            var redirects = C("SELECT COUNT(*) FROM seo_redirects WHERE active=1");
            var sitemapUrls = published - noindex;
            return J(new
            {
                canonical_host = Redirects.CanonicalBase,
                pages, published, noindex, sitemap_urls = sitemapUrls, redirects,
                issues = new { missing_title = missingTitle, missing_meta = missingMeta, duplicate_titles = dupTitles, duplicate_descriptions = dupMetas },
            });
        }));

        // ---------- per-page SEO listing with issue flags ----------
        app.MapGet("/api/admin/seo/pages", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var rows = db.Query("SELECT id,slug,title,meta_description,noindex,published,canonical_url,og_image,updated_at FROM pages ORDER BY slug");
            var titleCount = new Dictionary<string, int>(); var metaCount = new Dictionary<string, int>();
            foreach (var r in rows)
            {
                if (H.Str(r["title"]) is { Length: > 0 } t) titleCount[t] = titleCount.GetValueOrDefault(t) + 1;
                if (H.Str(r["meta_description"]) is { Length: > 0 } m) metaCount[m] = metaCount.GetValueOrDefault(m) + 1;
            }
            var outRows = rows.Select(r => new
            {
                id = H.L(r["id"]), slug = H.Str(r["slug"]), title = H.Str(r["title"]),
                meta_description = H.Str(r["meta_description"]), noindex = H.L(r["noindex"]),
                published = H.L(r["published"]), canonical_url = H.Str(r["canonical_url"]),
                og_image = H.Str(r["og_image"]), updated_at = H.Str(r["updated_at"]),
                issues = new
                {
                    missing_title = string.IsNullOrEmpty(H.Str(r["title"])),
                    missing_meta = string.IsNullOrEmpty(H.Str(r["meta_description"])),
                    duplicate_title = H.Str(r["title"]) is { Length: > 0 } tt && titleCount[tt] > 1,
                    duplicate_meta = H.Str(r["meta_description"]) is { Length: > 0 } mm && metaCount[mm] > 1,
                },
            });
            return J(new { rows = outRows });
        }));

        // ---------- redirect manager ----------
        app.MapGet("/api/admin/seo/redirects", (HttpRequest req) => gate(req, "pages", _ =>
            J(new { rows = db.Query("SELECT * FROM seo_redirects ORDER BY from_path") })));

        app.MapPost("/api/admin/seo/redirects", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var from = PathRedirects.Norm(H.GetS(b, "from_path"));
            var to = (H.GetS(b, "to_url") ?? "").Trim();
            var status = (int)(H.GetNum(b, "status") ?? 301); if (status is not (301 or 302 or 308)) status = 301;
            var note = (H.GetS(b, "note") ?? "").Trim(); if (note.Length > 500) note = note[..500];
            if (from.Length < 2) return Results.Json(new { error = "bad_from", message = "from_path must be a site path like /old-page.html" }, statusCode: 400);
            if (to.Length == 0) return Results.Json(new { error = "bad_to" }, statusCode: 400);
            if (Redirects.IsPrivatePath(from)) return Results.Json(new { error = "private_path", message = "private/app paths cannot be redirected" }, statusCode: 400);
            // single-hop guarantee: no self-redirects, and the target must not itself be redirected (chain)
            if (string.Equals(PathRedirects.Norm(to), from, StringComparison.OrdinalIgnoreCase))
                return Results.Json(new { error = "self_redirect" }, statusCode: 400);
            if (to.StartsWith('/') && db.QueryOne("SELECT id FROM seo_redirects WHERE active=1 AND from_path=?", PathRedirects.Norm(to)) is not null)
                return Results.Json(new { error = "chain", message = "the target is itself redirected — point directly at the final URL" }, statusCode: 400);
            // and creating this rule must not turn an existing rule's target into a hop
            if (db.QueryOne("SELECT id FROM seo_redirects WHERE active=1 AND to_url=?", from) is not null)
                return Results.Json(new { error = "chain", message = "an existing redirect points at this path — update it to the final URL first" }, statusCode: 400);
            db.Execute("DELETE FROM seo_redirects WHERE from_path=?", from);
            db.Execute("INSERT INTO seo_redirects(from_path,to_url,status,active,note) VALUES(?,?,?,1,?)", from, to, status, note);
            PathRedirects.Bump();
            log(actorId, "seo.redirect_set", $"{from} -> {to} ({status})");
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/seo/redirects/{id}/delete", (HttpRequest req, long id) => gate(req, "pages", adm =>
        {
            db.Execute("DELETE FROM seo_redirects WHERE id=?", id);
            PathRedirects.Bump();
            log(adm.Id, "seo.redirect_delete", id.ToString());
            return J(new { ok = true });
        }));

        // ---------- search-engine integrations (Phase 4) ----------
        // Public-facing IDs/tokens (GA4/GTM/Clarity/verification) are returned in full — they end up
        // in every page's HTML anyway. The PSI API key is a real secret: write-only, has_key status.
        app.MapGet("/api/admin/seo/integrations", (HttpRequest req) => gate(req, "pages", _ =>
        {
            string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
            var inKey = IndexNowService.Key(db);
            return J(new
            {
                ga4_measurement_id = S("ga4_measurement_id"),
                gtm_container_id = S("gtm_container_id"),
                clarity_project_id = S("clarity_project_id"),
                google_site_verification = S("google_site_verification"),
                bing_site_verification = S("bing_site_verification"),
                psi_has_key = S("psi_api_key").Length > 0,
                indexnow_key = inKey,
                indexnow_key_url = $"{Redirects.CanonicalBase}/{inKey}.txt",
                submissions = db.Query("SELECT engine,url_count,status,detail,created_at FROM seo_submissions ORDER BY id DESC LIMIT 20"),
            });
        }));

        app.MapPost("/api/admin/seo/integrations", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            void Set(string k, string? v)
            {
                if (v is null) return;                                  // only overwrite keys present in the body
                db.Execute("DELETE FROM site_settings WHERE skey=?", k);
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", k, v.Trim());
            }
            foreach (var k in new[] { "ga4_measurement_id", "gtm_container_id", "clarity_project_id", "google_site_verification", "bing_site_verification", "psi_api_key" })
                Set(k, H.GetS(b, k));
            SeoTags.Bump();
            log(actorId, "seo.integrations_update", null);
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/seo/indexnow/submit", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            List<string> urls;
            if (H.GetEl(b, "urls") is { ValueKind: System.Text.Json.JsonValueKind.Array } arr)
                urls = arr.EnumerateArray().Select(e => e.GetString() ?? "").Where(u => u.StartsWith(Redirects.CanonicalBase, StringComparison.OrdinalIgnoreCase)).ToList();
            else
                urls = IndexNowService.AllPublicUrls(db);
            var (ok, detail) = IndexNowService.Submit(db, urls);
            log(actorId, "seo.indexnow_submit", $"{urls.Count} urls: {detail}");
            return J(new { ok, submitted = urls.Count, detail });
        });

        // Google PageSpeed Insights adapter: measures a PUBLIC URL (the live site, not this process).
        // Works keyless at low volume; psi_api_key raises the quota. Failures return a clear error.
        var psiHttp = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        app.MapPost("/api/admin/seo/pagespeed", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var url = (H.GetS(b, "url") ?? Redirects.CanonicalBase + "/").Trim();
            if (!url.StartsWith("https://") && !url.StartsWith("http://"))
                return Results.Json(new { error = "bad_url" }, statusCode: 400);
            var key = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='psi_api_key'") ?? "";
            var api = "https://www.googleapis.com/pagespeedonline/v5/runPagespeed?url=" + Uri.EscapeDataString(url)
                    + "&category=performance&category=seo&strategy=mobile" + (key.Length > 0 ? "&key=" + Uri.EscapeDataString(key) : "");
            try
            {
                using var resp = await psiHttp.GetAsync(api);
                var txt = await resp.Content.ReadAsStringAsync();
                if (!resp.IsSuccessStatusCode)
                    return Results.Json(new { error = "psi_failed", status = (int)resp.StatusCode, detail = txt.Length > 300 ? txt[..300] : txt }, statusCode: 502);
                using var doc = System.Text.Json.JsonDocument.Parse(txt);
                var cats = doc.RootElement.GetProperty("lighthouseResult").GetProperty("categories");
                double Score(string c) => cats.TryGetProperty(c, out var el) && el.TryGetProperty("score", out var s) ? Math.Round(s.GetDouble() * 100) : -1;
                log(actorId, "seo.pagespeed", url);
                return J(new { ok = true, url, performance = Score("performance"), seo = Score("seo") });
            }
            catch (Exception e)
            {
                return Results.Json(new { error = "psi_failed", detail = e.Message }, statusCode: 502);
            }
        });

        // ---------- practical audit ----------
        var rxH1 = new Regex(@"<h1[\s>]", RegexOptions.IgnoreCase);
        var rxImg = new Regex(@"<img\b[^>]*>", RegexOptions.IgnoreCase);
        var rxAlt = new Regex(@"\balt\s*=", RegexOptions.IgnoreCase);
        var rxCanonical = new Regex(@"rel=[""']canonical[""']", RegexOptions.IgnoreCase);
        var rxJsonLd = new Regex(@"application/ld\+json", RegexOptions.IgnoreCase);
        var rxHref = new Regex(@"href=[""']([a-zA-Z0-9_\-]+\.html)(#[^""']*)?[""']");

        app.MapGet("/api/admin/seo/audit", (HttpRequest req) => gate(req, "pages", _ =>
        {
            var shells = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "student.html", "admin.html", "exam-ui.html" };
            var files = Directory.Exists(webRoot)
                ? Directory.EnumerateFiles(webRoot, "*.html").Select(Path.GetFileName).Where(f => f is not null && !shells.Contains(f)).Cast<string>().ToHashSet(StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>();
            var missingH1 = new List<string>(); var missingCanonical = new List<string>(); var missingJsonLd = new List<string>();
            var imagesNoAlt = new List<object>(); var brokenLinks = new List<object>();
            foreach (var f in files.OrderBy(x => x))
            {
                string html;
                try { html = File.ReadAllText(Path.Combine(webRoot, f)); } catch { continue; }
                if (!rxH1.IsMatch(html)) missingH1.Add(f);
                if (!rxCanonical.IsMatch(html)) missingCanonical.Add(f);
                if (!rxJsonLd.IsMatch(html)) missingJsonLd.Add(f);
                var noAlt = rxImg.Matches(html).Count(m => !rxAlt.IsMatch(m.Value));
                if (noAlt > 0) imagesNoAlt.Add(new { page = f, images = noAlt });
                var bad = rxHref.Matches(html).Select(m => m.Groups[1].Value).Distinct()
                    .Where(t => !files.Contains(t) && !shells.Contains(t)).ToList();
                if (bad.Count > 0) brokenLinks.Add(new { page = f, targets = bad });
            }
            // db-side metadata issues
            var missingTitle = db.Query("SELECT slug FROM pages WHERE published=1 AND (title IS NULL OR title='')").Select(r => H.Str(r["slug"])).ToList();
            var missingMeta = db.Query("SELECT slug FROM pages WHERE published=1 AND (meta_description IS NULL OR meta_description='')").Select(r => H.Str(r["slug"])).ToList();
            var dupTitles = db.Query("SELECT title, COUNT(*) n FROM pages WHERE published=1 AND title IS NOT NULL AND title<>'' GROUP BY title HAVING COUNT(*)>1").Select(r => new { title = H.Str(r["title"]), count = H.L(r["n"]) }).ToList();
            // redirect problems (defensive — write-time validation should keep these empty)
            var reds = db.Query("SELECT from_path,to_url FROM seo_redirects WHERE active=1");
            var fromSet = reds.Select(r => H.Str(r["from_path"]) ?? "").ToHashSet(StringComparer.OrdinalIgnoreCase);
            var chains = reds.Where(r => (H.Str(r["to_url"]) ?? "").StartsWith('/') && fromSet.Contains(PathRedirects.Norm(H.Str(r["to_url"]))))
                             .Select(r => new { from = H.Str(r["from_path"]), to = H.Str(r["to_url"]) }).ToList();
            return J(new
            {
                page_count = files.Count,
                missing_title = missingTitle, missing_meta = missingMeta, duplicate_titles = dupTitles,
                missing_h1 = missingH1, missing_canonical = missingCanonical, missing_structured_data = missingJsonLd,
                images_missing_alt = imagesNoAlt, broken_internal_links = brokenLinks, redirect_chains = chains,
            });
        }));
    }
}
