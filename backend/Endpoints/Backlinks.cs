using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Backlink &amp; Outreach CRM (Phase 5) — track link prospects, log real outreach touchpoints, and record
/// earned backlinks. The whole surface is gated by the cc_backlinks permission and every mutating action is
/// audited. PCI does not scrape the web, buy links, or run automated discovery: prospects/outreach/links are
/// entered manually or by CSV. The one automated action is on-demand VERIFICATION of a single recorded
/// source URL through the SSRF-guarded egress client (see <see cref="BacklinkMonitor"/>).
/// </summary>
public static class Backlinks
{
    const string PERM = "cc_backlinks";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;
        string? Cap(string? s, int n) => s is null ? null : (s.Length <= n ? s : s[..n]);
        int? Int(Dictionary<string, System.Text.Json.JsonElement> b, string k) => H.GetNum(b, k) is double d ? (int)d : (int?)null;

        // ── overview ────────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/backlinks/overview", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var prospects = db.Query("SELECT status, COUNT(*) AS n FROM cc_link_prospects WHERE active=1 GROUP BY status");
            var links = db.Query("SELECT status, COUNT(*) AS n FROM cc_backlinks WHERE active=1 GROUP BY status");
            var due = db.Scalar<long>("SELECT COUNT(*) FROM cc_outreach WHERE follow_up_at IS NOT NULL AND follow_up_at<=datetime('now')");
            var live = db.Scalar<long>("SELECT COUNT(*) FROM cc_backlinks WHERE active=1 AND status='live'");
            var lost = db.Scalar<long>("SELECT COUNT(*) FROM cc_backlinks WHERE active=1 AND status IN ('lost','removed','redirected')");
            return J(new { prospects, links, follow_ups_due = due, live, lost });
        }));

        // ── prospects ───────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/backlinks/prospects", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var status = req.Query["status"].ToString();
            var sql = "SELECT * FROM cc_link_prospects WHERE active=1" + (status.Length > 0 ? " AND status=?" : "") + " ORDER BY id DESC LIMIT 500";
            return J(new { rows = status.Length > 0 ? db.Query(sql, status) : db.Query(sql) });
        }));

        app.MapPost("/api/admin/content/backlinks/prospects", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, PERM, adm =>
            {
                var name = (H.GetS(b, "name") ?? "").Trim();
                if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var id = db.ExecuteReturningId(@"INSERT INTO cc_link_prospects(name,domain,url,category,authority,relevance,status,owner,contact_name,contact_email,notes,next_action_at,created_by,created_at,updated_at)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?, ?, datetime('now'), datetime('now'))",
                    Cap(name, 160), Cap(H.GetS(b, "domain"), 190), Cap(H.GetS(b, "url"), 500), Cap(H.GetS(b, "category") ?? "publication", 40),
                    Int(b, "authority"), Cap(H.GetS(b, "relevance") ?? "medium", 12), Cap(H.GetS(b, "status") ?? "prospect", 20),
                    Cap(H.GetS(b, "owner"), 120), Cap(H.GetS(b, "contact_name"), 120), Cap(H.GetS(b, "contact_email"), 190),
                    H.GetS(b, "notes"), H.GetS(b, "next_action_at"), adm.Id);
                log(adm.Id, "backlink_prospect_create", name + " #" + id);
                return J(new { ok = true, id });
            });
        });

        app.MapPatch("/api/admin/content/backlinks/prospects/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, PERM, adm =>
            {
                foreach (var k in new[] { "name", "domain", "url", "category", "relevance", "status", "owner", "contact_name", "contact_email", "notes", "next_action_at" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE cc_link_prospects SET {k}=? WHERE id=?", v, id);
                if (Int(b, "authority") is int a) db.Execute("UPDATE cc_link_prospects SET authority=? WHERE id=?", a, id);
                db.Execute("UPDATE cc_link_prospects SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "backlink_prospect_update", "#" + id);
                return J(new { ok = true });
            });
        });

        app.MapPost("/api/admin/content/backlinks/prospects/{id:long}/delete", (HttpRequest req, long id) => gate(req, PERM, adm =>
        {
            db.Execute("UPDATE cc_link_prospects SET active=0, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "backlink_prospect_delete", "#" + id);
            return J(new { ok = true });
        }));

        // ── outreach touchpoints ────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/backlinks/outreach", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var pid = req.Query["prospect_id"].ToString();
            var sql = "SELECT * FROM cc_outreach" + (pid.Length > 0 ? " WHERE prospect_id=?" : "") + " ORDER BY id DESC LIMIT 300";
            return J(new { rows = pid.Length > 0 ? db.Query(sql, long.Parse(pid)) : db.Query(sql) });
        }));

        app.MapPost("/api/admin/content/backlinks/outreach", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, PERM, adm =>
            {
                var pid = H.GetNum(b, "prospect_id") is double d ? (long)d : 0;
                if (pid <= 0) return Results.Json(new { error = "prospect_id_required" }, statusCode: 400);
                var id = db.ExecuteReturningId(@"INSERT INTO cc_outreach(prospect_id,channel,direction,subject,body,outcome,occurred_at,follow_up_at,created_by,created_at)
                    VALUES(?,?,?,?,?,?, COALESCE(?, datetime('now')), ?, ?, datetime('now'))",
                    pid, Cap(H.GetS(b, "channel") ?? "email", 20), Cap(H.GetS(b, "direction") ?? "out", 8), Cap(H.GetS(b, "subject"), 200),
                    H.GetS(b, "body"), Cap(H.GetS(b, "outcome") ?? "sent", 20), H.GetS(b, "occurred_at"), H.GetS(b, "follow_up_at"), adm.Id);
                // Logging a touchpoint may advance the prospect's pipeline status and next action (operator-supplied).
                if (H.GetS(b, "prospect_status") is { Length: > 0 } ns) db.Execute("UPDATE cc_link_prospects SET status=?, updated_at=datetime('now') WHERE id=?", Cap(ns, 20), pid);
                if (H.GetS(b, "follow_up_at") is { Length: > 0 } fu) db.Execute("UPDATE cc_link_prospects SET next_action_at=?, updated_at=datetime('now') WHERE id=?", fu, pid);
                log(adm.Id, "backlink_outreach_log", $"prospect {pid} #{id}");
                return J(new { ok = true, id });
            });
        });

        // ── backlinks ───────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/backlinks/links", (HttpRequest req) => gate(req, PERM, _ =>
        {
            var status = req.Query["status"].ToString();
            var pid = req.Query["prospect_id"].ToString();
            var where = new List<string> { "active=1" }; var args = new List<object?>();
            if (status.Length > 0) { where.Add("status=?"); args.Add(status); }
            if (pid.Length > 0) { where.Add("prospect_id=?"); args.Add(long.Parse(pid)); }
            return J(new { rows = db.Query("SELECT * FROM cc_backlinks WHERE " + string.Join(" AND ", where) + " ORDER BY id DESC LIMIT 500", args.ToArray()) });
        }));

        app.MapPost("/api/admin/content/backlinks/links", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, PERM, adm =>
            {
                var (ok, id, err) = InsertLink(db, b, adm.Id, "manual");
                if (!ok) return Results.Json(new { error = err }, statusCode: 400);
                log(adm.Id, "backlink_link_create", "#" + id);
                return J(new { ok = true, id, duplicate = id == 0 });
            });
        });

        // Bulk CSV/paste import: { rows: [{source_url,target_url,anchor_text,rel,link_type,prospect_id}, ...] }.
        app.MapPost("/api/admin/content/backlinks/links/import", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, PERM, adm =>
            {
                int added = 0, dup = 0, bad = 0;
                if (H.GetEl(b, "rows") is { ValueKind: System.Text.Json.JsonValueKind.Array } rowsEl)
                    foreach (var el in rowsEl.EnumerateArray())
                    {
                        var row = H.ToMap(el);
                        var (ok, id, _) = InsertLink(db, row, adm.Id, "csv");
                        if (!ok) bad++; else if (id > 0) added++; else dup++;
                    }
                log(adm.Id, "backlink_link_import", $"+{added} / {dup} dup / {bad} bad");
                return J(new { ok = true, added, duplicate = dup, invalid = bad });
            });
        });

        app.MapPatch("/api/admin/content/backlinks/links/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, PERM, adm =>
            {
                foreach (var k in new[] { "anchor_text", "rel", "link_type", "status", "notes" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE cc_backlinks SET {k}=? WHERE id=?", v, id);
                if (H.GetNum(b, "prospect_id") is double d) db.Execute("UPDATE cc_backlinks SET prospect_id=? WHERE id=?", (long)d, id);
                db.Execute("UPDATE cc_backlinks SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "backlink_link_update", "#" + id);
                return J(new { ok = true });
            });
        });

        app.MapPost("/api/admin/content/backlinks/links/{id:long}/delete", (HttpRequest req, long id) => gate(req, PERM, adm =>
        {
            db.Execute("UPDATE cc_backlinks SET active=0, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "backlink_link_delete", "#" + id);
            return J(new { ok = true });
        }));

        // Verify a single recorded backlink NOW — fetch its source page through the SSRF-guarded client and
        // classify live/lost/redirected/removed/unreachable. On-demand only; no crawling.
        app.MapPost("/api/admin/content/backlinks/links/{id:long}/verify", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, PERM, _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            if (!Settings.Bool(db, "backlink_verify_enabled", true)) return Results.Json(new { error = "verify_disabled" }, statusCode: 400);
            var adm = adminFromReq(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var (status, code, err) = await BacklinkMonitor.VerifyLink(db, id);
            log(adm?.Id, "backlink_link_verify", $"#{id} → {status} ({code})");
            return J(new { ok = true, status, http_code = code, error = err });
        });
    }

    /// <summary>Insert one backlink with a computed dedup hash. Returns (ok, id, err); id==0 means the link
    /// was a duplicate (unique source→target) and silently ignored. A source URL is required.</summary>
    static (bool ok, long id, string? err) InsertLink(Db db, Dictionary<string, System.Text.Json.JsonElement> b, long? adminId, string via)
    {
        string? Cap(string? s, int n) => s is null ? null : (s.Length <= n ? s : s[..n]);
        var source = (H.GetS(b, "source_url") ?? "").Trim();
        if (source.Length == 0) return (false, 0, "source_url_required");
        var target = (H.GetS(b, "target_url") ?? "").Trim();
        string? domain = null;
        try { if (Uri.TryCreate(source, UriKind.Absolute, out var u)) domain = u.Host.ToLowerInvariant(); } catch { }
        var hash = BacklinkMonitor.LinkHash(source, target);
        var (rowid, changes) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO cc_backlinks(prospect_id,link_hash,source_url,source_domain,target_url,anchor_text,rel,link_type,status,discovered_via,created_by,created_at,updated_at)
            VALUES(?,?,?,?,?,?,?,?, 'candidate', ?, ?, datetime('now'), datetime('now'))",
            H.GetNum(b, "prospect_id") is double d ? (long)d : (object?)null, hash, Cap(source, 500), Cap(domain, 190), Cap(target, 500),
            Cap(H.GetS(b, "anchor_text"), 300), Cap(H.GetS(b, "rel") ?? "unknown", 16), Cap(H.GetS(b, "link_type") ?? "editorial", 20), via, adminId);
        return (true, changes > 0 ? rowid : 0, null);
    }
}
