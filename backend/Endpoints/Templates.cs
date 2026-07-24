using System.Text;
using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI Free Templates Library (§6A–6C). A public, free library of ready-to-use project-controls templates
/// (WBS, EVM tracker, risk register, cash-flow forecast, RACI, …), each a small CSV whose content is stored
/// inline in the row. Two public, no-login endpoints serve the catalogue and the file download; the admin
/// side (gated on the existing 'content' permission) manages the library. Every admin mutation bumps the
/// server-rendered public section cache (ListSections) so the website reflects a change immediately.
///
/// Only PUBLISHED templates are ever served publicly — a draft is admin-only. Content is synthetic; there is
/// no student data and no exam answer key here.
/// </summary>
public static class Templates
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        const string SECTION = "content";

        // ---- public catalogue (no login) ----
        app.MapGet("/api/public/templates", () =>
        {
            var rows = new List<object>();
            var categories = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var t in db.Query(@"SELECT slug,title,category,certification_id,summary,format,download_count,
                    LENGTH(body) AS bytes
                FROM templates WHERE published=1 ORDER BY sort_order ASC, id ASC"))
            {
                var cat = H.Str(t["category"]) ?? "general";
                categories.Add(cat);
                rows.Add(new
                {
                    slug = H.Str(t["slug"]),
                    title = H.Str(t["title"]),
                    category = cat,
                    certification_id = t["certification_id"] is null ? (long?)null : H.L(t["certification_id"]),
                    summary = H.Str(t["summary"]),
                    format = H.Str(t["format"]) ?? "csv",
                    bytes = H.L(t["bytes"]),
                    download_count = H.L(t["download_count"]),
                    download_url = $"/api/public/templates/{H.Str(t["slug"])}/file",
                });
            }
            return Results.Json(new { rows, total = rows.Count, categories });
        });

        // ---- public single template (metadata + body preview); free content, so the body is not secret ----
        app.MapGet("/api/public/templates/{slug}", (string slug) =>
        {
            var t = db.QueryOne("SELECT * FROM templates WHERE slug=? AND published=1", slug);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return Results.Json(new
            {
                slug = H.Str(t["slug"]),
                title = H.Str(t["title"]),
                category = H.Str(t["category"]),
                certification_id = t["certification_id"] is null ? (long?)null : H.L(t["certification_id"]),
                summary = H.Str(t["summary"]),
                format = H.Str(t["format"]) ?? "csv",
                body = H.Str(t["body"]),
                download_count = H.L(t["download_count"]),
                download_url = $"/api/public/templates/{H.Str(t["slug"])}/file",
            });
        });

        // ---- public file download (no login). Streams the CSV as an attachment and counts the download. ----
        app.MapGet("/api/public/templates/{slug}/file", (string slug) =>
        {
            var t = db.QueryOne("SELECT id,slug,title,format,body FROM templates WHERE slug=? AND published=1", slug);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var tid = H.L(t["id"]);
            db.Execute("UPDATE templates SET download_count=download_count+1 WHERE id=?", tid);
            // §6C — also bump today's per-template daily aggregate (INSERT-OR-IGNORE the row, then increment).
            var day = DateTime.UtcNow.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            db.Execute("INSERT OR IGNORE INTO template_download_daily(template_id,day,count) VALUES(?,?,0)", tid, day);
            db.Execute("UPDATE template_download_daily SET count=count+1 WHERE template_id=? AND day=?", tid, day);
            var ext = (H.Str(t["format"]) ?? "csv").Trim();
            var name = $"{H.Str(t["slug"])}.{(ext.Length == 0 ? "csv" : ext)}";
            var mime = ext == "csv" ? "text/csv" : "text/plain";
            return Results.File(Encoding.UTF8.GetBytes(H.Str(t["body"]) ?? ""), mime, name);
        });

        // ---- admin: list every template (incl. drafts), with body + counts (gated 'content') ----
        app.MapGet("/api/admin/templates", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            var rows = new List<object>();
            var published = 0;
            foreach (var t in db.Query("SELECT * FROM templates ORDER BY sort_order ASC, id ASC"))
            {
                if (H.L(t["published"]) == 1) published++;
                rows.Add(new
                {
                    id = H.L(t["id"]),
                    slug = H.Str(t["slug"]),
                    title = H.Str(t["title"]),
                    category = H.Str(t["category"]),
                    certification_id = t["certification_id"] is null ? (long?)null : H.L(t["certification_id"]),
                    summary = H.Str(t["summary"]),
                    format = H.Str(t["format"]) ?? "csv",
                    body = H.Str(t["body"]),
                    published = H.L(t["published"]) == 1,
                    sort_order = H.L(t["sort_order"]),
                    download_count = H.L(t["download_count"]),
                    updated_at = H.Str(t["updated_at"]),
                });
            }
            return Results.Json(new { rows, total = rows.Count, published });
        }));

        // ---- admin: download analytics (gated 'content') — totals, a 30-day daily trend, and the top titles ----
        app.MapGet("/api/admin/templates/analytics", (HttpRequest req) => gate(req, SECTION, _ =>
        {
            // Grand total is the canonical per-template counter (covers downloads recorded before the daily
            // table existed); the daily table drives the trend only.
            var totals = db.QueryOne(@"SELECT COUNT(*) AS n, COALESCE(SUM(download_count),0) AS dl,
                    COALESCE(SUM(CASE WHEN published=1 THEN 1 ELSE 0 END),0) AS pub FROM templates");
            var totalDownloads = totals is null ? 0 : H.L(totals["dl"]);

            // 30-day window ending today (UTC). Build the day keys in C#, then fold the grouped sums in so
            // missing days render as zero — the SVG trend needs a dense, ordered series.
            const int Days = 30;
            var today = DateTime.UtcNow.Date;
            var keys = Enumerable.Range(0, Days).Select(i => today.AddDays(i - (Days - 1))
                .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)).ToList();
            var byDay = keys.ToDictionary(k => k, _ => 0L);
            foreach (var r in db.Query("SELECT day, SUM(count) AS n FROM template_download_daily WHERE day>=? GROUP BY day", keys[0]))
            {
                var d = H.Str(r["day"]) ?? "";
                if (byDay.ContainsKey(d)) byDay[d] = H.L(r["n"]);
            }
            var series = keys.Select(k => new { day = k, count = byDay[k] }).ToList();
            var windowTotal = series.Sum(s => s.count);

            var top = new List<object>();
            foreach (var r in db.Query("SELECT slug,title,download_count,published FROM templates ORDER BY download_count DESC, id ASC LIMIT 8"))
                top.Add(new
                {
                    slug = H.Str(r["slug"]),
                    title = H.Str(r["title"]),
                    download_count = H.L(r["download_count"]),
                    published = H.L(r["published"]) == 1,
                });

            return Results.Json(new
            {
                total_downloads = totalDownloads,
                templates = totals is null ? 0 : H.L(totals["n"]),
                published = totals is null ? 0 : H.L(totals["pub"]),
                window_days = Days,
                window_downloads = windowTotal,
                series,
                top,
            });
        }));

        // ---- admin: create a template ----
        app.MapPost("/api/admin/templates", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var slug = Slugify(H.GetS(b, "slug") ?? "");
                var title = (H.GetS(b, "title") ?? "").Trim();
                var body = H.GetS(b, "body") ?? "";
                if (slug.Length == 0 || title.Length == 0 || body.Trim().Length == 0)
                    return Results.Json(new { error = "bad_input", message = "slug, title and body are required." }, statusCode: 400);
                if (db.QueryOne("SELECT id FROM templates WHERE slug=?", slug) is not null)
                    return Results.Json(new { error = "duplicate_slug" }, statusCode: 409);

                long? cert = H.GetNum(b, "certification_id") is { } cn ? (long)cn : null;
                var id = db.ExecuteReturningId(@"INSERT INTO templates
                    (slug,title,category,certification_id,summary,format,body,published,sort_order,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?)",
                    slug, title, (H.GetS(b, "category") ?? "general").Trim(), cert, H.GetS(b, "summary"),
                    (H.GetS(b, "format") ?? "csv").Trim(), body, Truthy(H.GetEl(b, "published"), dflt: true) ? 1 : 0,
                    (int)(H.GetNum(b, "sort_order") ?? 0), adm.Id);
                ListSections.Bump();
                log(adm.Id, "template_create", $"{slug} · #{id}");
                return Results.Json(new { id, slug });
            });
        });

        // ---- admin: edit a template (dynamic partial update) ----
        app.MapPatch("/api/admin/templates/{id:long}", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, SECTION, adm =>
            {
                var t = db.QueryOne("SELECT id,slug FROM templates WHERE id=?", id);
                if (t is null) return Results.NotFound(new { error = "not_found" });

                var sets = new List<string>(); var vals = new List<object?>();
                void SetIf(string key, string col, Func<JsonElement, object?> map)
                {
                    if (H.GetEl(b, key) is { } el) { sets.Add($"{col}=?"); vals.Add(map(el)); }
                }
                SetIf("title", "title", e => e.GetString());
                SetIf("category", "category", e => e.GetString());
                SetIf("certification_id", "certification_id", e => e.ValueKind == JsonValueKind.Number ? (object?)e.GetInt64() : null);
                SetIf("summary", "summary", e => e.GetString());
                SetIf("format", "format", e => e.GetString());
                SetIf("body", "body", e => e.GetString());
                SetIf("published", "published", e => Truthy(el: e) ? 1 : 0);
                SetIf("sort_order", "sort_order", e => e.ValueKind == JsonValueKind.Number ? e.GetInt32() : 0);
                if (sets.Count == 0) return Results.Json(new { error = "no_fields" }, statusCode: 400);

                vals.Add(id);
                db.Execute($"UPDATE templates SET {string.Join(",", sets)}, updated_at=datetime('now') WHERE id=?", vals.ToArray());
                ListSections.Bump();
                log(adm.Id, "template_edit", $"{H.Str(t["slug"])} · {sets.Count} field(s)");
                return Results.Json(new { id, updated = sets.Count });
            });
        });

        // ---- admin: delete a template ----
        app.MapDelete("/api/admin/templates/{id:long}", (HttpRequest req, long id) => gate(req, SECTION, adm =>
        {
            var t = db.QueryOne("SELECT slug FROM templates WHERE id=?", id);
            if (t is null) return Results.NotFound(new { error = "not_found" });
            db.Execute("DELETE FROM templates WHERE id=?", id);
            // §6C — clear the template's analytics too, so no orphan daily rows survive the delete.
            db.Execute("DELETE FROM template_download_daily WHERE template_id=?", id);
            ListSections.Bump();
            log(adm.Id, "template_delete", H.Str(t["slug"]));
            return Results.Json(new { id, deleted = true });
        }));
    }

    // Conservative slug: lowercase, alphanumerics and hyphens only (safe in a URL path segment and filename).
    static string Slugify(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var ch in s.Trim().ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(ch) && ch < 128) sb.Append(ch);
            else if (ch is ' ' or '-' or '_' or '.') { if (sb.Length > 0 && sb[^1] != '-') sb.Append('-'); }
        }
        return sb.ToString().Trim('-');
    }

    static bool Truthy(JsonElement? el, bool dflt = false)
    {
        if (el is not { } e) return dflt;
        return e.ValueKind == JsonValueKind.True
            || (e.ValueKind == JsonValueKind.String && e.GetString() is "1" or "true")
            || (e.ValueKind == JsonValueKind.Number && e.TryGetInt32(out var i) && i != 0);
    }
}
