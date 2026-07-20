using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// External content import (Phase 4) — register APPROVED RSS/Atom sources with a licence/permission record,
/// fetch their feeds into a review queue, and turn queued items into PCI blog DRAFTS. The default is
/// curated-link mode (a PCI-written lead + a link back to the original); "excerpt" needs the source to permit
/// it; "full" republication requires a recorded licence AND elevated (cc_legal) approval. Nothing is
/// published automatically. Managing sources + reviewing items is gated by cc_review; full republication by
/// cc_legal. Every action is audited.
/// </summary>
public static class ExternalImport
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate, Func<HttpRequest, AdminCtx?> adminFromReq)
    {
        IResult J(object o) => Results.Json(o);
        bool IsOk(IResult r) => r is Microsoft.AspNetCore.Http.HttpResults.Ok;
        int B(Dictionary<string, System.Text.Json.JsonElement> b, string k, int def) => H.GetBool(b, k) is bool v ? (v ? 1 : 0) : def;

        // ── sources ───────────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/import/sources", (HttpRequest req) => gate(req, "cc_review", _ =>
            J(new { rows = db.Query("SELECT * FROM cc_external_sources ORDER BY id DESC") })));

        app.MapPost("/api/admin/content/import/sources", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "cc_review", adm =>
            {
                var name = (H.GetS(b, "name") ?? "").Trim();
                var feed = (H.GetS(b, "feed_url") ?? "").Trim();
                if (name.Length == 0 || feed.Length == 0) return Results.Json(new { error = "name_and_feed_required" }, statusCode: 400);
                var allowed = H.GetS(b, "allowed_use") is "excerpt" or "full" ? H.GetS(b, "allowed_use")! : "curated_link";
                var id = db.ExecuteReturningId(@"INSERT INTO cc_external_sources(name,domain,feed_url,source_type,owner_contact,license,permission_ref,allowed_use,attribution_required,canonical_required,auto_publish,language,created_by,created_at,updated_at)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?, ?, datetime('now'), datetime('now'))",
                    name, H.GetS(b, "domain"), feed, H.GetS(b, "source_type") ?? "rss", H.GetS(b, "owner_contact"),
                    H.GetS(b, "license") ?? "all_rights_reserved", H.GetS(b, "permission_ref"), allowed,
                    B(b, "attribution_required", 1), B(b, "canonical_required", 1), B(b, "auto_publish", 0), H.GetS(b, "language"), adm.Id);
                log(adm.Id, "import_source_create", name + " #" + id);
                return J(new { ok = true, id });
            });
        });

        app.MapPatch("/api/admin/content/import/sources/{id:long}", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_review", adm =>
            {
                foreach (var k in new[] { "name", "domain", "feed_url", "owner_contact", "license", "permission_ref", "allowed_use", "language" })
                    if (H.GetS(b, k) is { } v) db.Execute($"UPDATE cc_external_sources SET {k}=? WHERE id=?", v, id);
                foreach (var k in new[] { "attribution_required", "canonical_required", "auto_publish", "active" })
                    if (H.GetBool(b, k) is bool v) db.Execute($"UPDATE cc_external_sources SET {k}=? WHERE id=?", v ? 1 : 0, id);
                db.Execute("UPDATE cc_external_sources SET updated_at=datetime('now') WHERE id=?", id);
                log(adm.Id, "import_source_update", "#" + id);
                return J(new { ok = true });
            });
        });

        app.MapPost("/api/admin/content/import/sources/{id:long}/delete", (HttpRequest req, long id) => gate(req, "cc_review", adm =>
        {
            db.Execute("UPDATE cc_external_sources SET active=0, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "import_source_delete", "#" + id);
            return J(new { ok = true });
        }));

        // Fetch a source's feed now → ingest into the review queue.
        app.MapPost("/api/admin/content/import/sources/{id:long}/fetch", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "cc_review", _ => Results.Ok());
            if (!IsOk(denied)) return denied;
            var adm = adminFromReq(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var (added, dup, total, err) = await ExternalImport_Fetch(db, id);
            log(adm?.Id, "import_fetch", $"source {id}: +{added} / {dup} dup / {total}");
            return err is null ? J(new { ok = true, added, duplicate = dup, total }) : Results.Json(new { error = "fetch_failed", message = err }, statusCode: 400);
        });

        // ── review queue ──────────────────────────────────────────────────────────────────────────────
        app.MapGet("/api/admin/content/import/items", (HttpRequest req) => gate(req, "cc_review", _ =>
        {
            var sid = req.Query["source_id"].ToString();
            var status = req.Query["status"].ToString();
            var where = new List<string>(); var args = new List<object?>();
            if (sid.Length > 0) { where.Add("i.source_id=?"); args.Add(long.Parse(sid)); }
            if (status.Length > 0) { where.Add("i.status=?"); args.Add(status); }
            var sql = "SELECT i.*, s.name AS source_name, s.domain AS source_domain, s.allowed_use FROM cc_external_items i LEFT JOIN cc_external_sources s ON s.id=i.source_id"
                    + (where.Count > 0 ? " WHERE " + string.Join(" AND ", where) : "") + " ORDER BY i.id DESC LIMIT 200";
            return J(new { rows = db.Query(sql, args.ToArray()) });
        }));

        // Approve a queued item → a PCI blog DRAFT. Full republication needs cc_legal + a recorded licence.
        app.MapPost("/api/admin/content/import/items/{id:long}/approve", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            var mode = H.GetS(b, "mode") is "excerpt" or "full" ? H.GetS(b, "mode")! : "link";
            var perm = mode == "full" ? "cc_legal" : "cc_review";     // elevated permission for full republication
            return gate(ctx.Request, perm, adm =>
            {
                var (ok, postId, err) = Core.ExternalImport.ApproveItem(db, id, mode, adm.Id);
                if (!ok) return Results.Json(new { error = err }, statusCode: 400);
                if (H.GetS(b, "note") is { Length: > 0 } n) db.Execute("UPDATE cc_external_items SET review_note=? WHERE id=?", n, id);
                log(adm.Id, "import_item_approve", $"item {id} as {mode} → post {postId}");
                return J(new { ok = true, mode, post_id = postId });
            });
        });

        app.MapPost("/api/admin/content/import/items/{id:long}/reject", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var id = long.Parse((string)ctx.Request.RouteValues["id"]!);
            return gate(ctx.Request, "cc_review", adm =>
            {
                db.Execute("UPDATE cc_external_items SET status='rejected', review_note=?, reviewed_by=?, updated_at=datetime('now') WHERE id=?", H.GetS(b, "note"), adm.Id, id);
                log(adm.Id, "import_item_reject", "item " + id);
                return J(new { ok = true });
            });
        });
    }

    // async wrapper so the endpoint lambda stays synchronous inside gate().
    static Task<(int, int, int, string?)> ExternalImport_Fetch(Db db, long id) => Core.ExternalImport.FetchSource(db, id);
}
