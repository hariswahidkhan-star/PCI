using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Integrations (master-plan Phase 9 — ERP foundation). Manage outbound connectors,
/// inspect the event outbox and the delivery ledger, and re-drive deliveries. The generic <b>webhook</b>
/// connector pushes signed JSON to any HTTPS endpoint (or an automation bridge to an ERP); specific ERP
/// connectors arrive in Phase 10 on the same outbox + delivery machinery.
///
///   GET  /api/admin/integrations                       — connectors (secret redacted → has_secret)
///   POST /api/admin/integrations                       — create / update (secret write-only)
///   POST /api/admin/integrations/{id}/delete
///   POST /api/admin/integrations/{id}/test             — emit a ping and deliver it now
///   GET  /api/admin/integrations/events                — recent outbox events
///   GET  /api/admin/integrations/deliveries            — delivery ledger (status, attempts, errors)
///   POST /api/admin/integrations/deliveries/{id}/retry — requeue a delivery immediately
///
/// Gated by the 'integrations' permission (platform group); every mutation is audit-logged.
/// </summary>
public static class AdminIntegrations
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult? Deny(HttpRequest req)
        {
            var r = gate(req, "integrations", _ => Results.Ok());
            return r is Microsoft.AspNetCore.Http.HttpResults.Ok ? null : r;
        }
        var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

        object Redact(Dictionary<string, object?> r) => new
        {
            id = H.L(r["id"]), provider = H.Str(r["provider"]), name = H.Str(r["name"]),
            enabled = H.L(r["enabled"]), endpoint_url = H.Str(r["endpoint_url"]),
            has_secret = (H.Str(r["secret"]) ?? "").Length > 0,             // the secret itself is never returned
            event_filter = H.Str(r["event_filter"]), status = H.Str(r["status"]),
            last_delivery_at = H.Str(r["last_delivery_at"]), created_at = H.Str(r["created_at"]),
        };

        // ---------- list connectors + catalogue of emittable events ----------
        app.MapGet("/api/admin/integrations", (HttpRequest req) => gate(req, "integrations", _ =>
            J(new
            {
                rows = db.Query("SELECT * FROM integrations ORDER BY id").Select(Redact),
                events = Integrations.KnownEvents,
            })));

        // ---------- create / update ----------
        app.MapPost("/api/admin/integrations", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var id = (long)(H.GetNum(b, "id") ?? 0);
            var provider = (H.GetS(b, "provider") ?? "webhook").Trim().ToLowerInvariant();
            if (provider != "webhook") return Results.Json(new { error = "unsupported_provider", message = "Only the generic webhook connector is available in this release." }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? "").Trim(); if (name.Length == 0) name = "Webhook";
            var endpoint = (H.GetS(b, "endpoint_url") ?? "").Trim();
            if (endpoint.Length > 0 && !endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                return Results.Json(new { error = "bad_endpoint", message = "Endpoint must be an http(s) URL." }, statusCode: 400);
            var enabled = H.GetEl(b, "enabled") is { } en && (en.ValueKind == JsonValueKind.True
                || (en.ValueKind == JsonValueKind.String && en.GetString() is "1" or "true")
                || (en.ValueKind == JsonValueKind.Number && en.TryGetInt32(out var ei) && ei != 0)) ? 1 : 0;
            // event_filter: an array of event types (empty/absent = all). Only keep known event names.
            string? filter = null;
            if (H.GetEl(b, "event_filter") is { ValueKind: JsonValueKind.Array } arr)
            {
                var picked = arr.EnumerateArray().Select(e => e.GetString()).Where(s => s is not null && Integrations.KnownEvents.Contains(s)).ToList();
                if (picked.Count > 0) filter = JsonSerializer.Serialize(picked);
            }

            if (id > 0 && db.QueryOne("SELECT id FROM integrations WHERE id=?", id) is not null)
            {
                db.Execute("UPDATE integrations SET provider=?, name=?, endpoint_url=?, event_filter=?, enabled=?, updated_at=datetime('now') WHERE id=?",
                    provider, name, endpoint, filter, enabled, id);
                // Secret is only touched when the key is present: allows "set" and "clear" without leaking it.
                if (H.GetEl(b, "secret") is { } sEl && sEl.ValueKind == JsonValueKind.String)
                    db.Execute("UPDATE integrations SET secret=? WHERE id=?", sEl.GetString() ?? "", id);
                log(null, "integration.update", $"{id} {name}");
                return J(new { ok = true, id });
            }
            var secret = H.GetS(b, "secret") ?? "";
            var newId = db.ExecuteReturningId("INSERT INTO integrations(provider,name,endpoint_url,secret,event_filter,enabled) VALUES(?,?,?,?,?,?)",
                provider, name, endpoint, secret, filter, enabled);
            log(null, "integration.create", $"{newId} {name}");
            return J(new { ok = true, id = newId });
        });

        app.MapPost("/api/admin/integrations/{id}/delete", (HttpRequest req, long id) => gate(req, "integrations", _ =>
        {
            db.Execute("DELETE FROM integration_deliveries WHERE integration_id=?", id);
            db.Execute("DELETE FROM integrations WHERE id=?", id);
            log(null, "integration.delete", id.ToString());
            return J(new { ok = true });
        }));

        // ---------- test: emit a ping and deliver it immediately ----------
        app.MapPost("/api/admin/integrations/{id}/test", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var integ = db.QueryOne("SELECT * FROM integrations WHERE id=?", id);
            if (integ is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var eventId = db.ExecuteReturningId("INSERT INTO integration_events(event_type,entity_type,entity_id,payload) VALUES('ping','test',?,?)",
                id, JsonSerializer.Serialize(new { message = "PCI integration test", integration_id = id, at = H.IsoNow }));
            db.Execute("INSERT OR IGNORE INTO integration_deliveries(event_id,integration_id,status,next_attempt_at) VALUES(?,?,'pending',datetime('now'))", eventId, id);
            var del = db.QueryOne("SELECT * FROM integration_deliveries WHERE event_id=? AND integration_id=?", eventId, id);
            if (del is not null) await Integrations.DeliverOne(db, http, del);
            var after = db.QueryOne("SELECT status,response_code,last_error FROM integration_deliveries WHERE event_id=? AND integration_id=?", eventId, id);
            log(null, "integration.test", id.ToString());
            return J(new { ok = H.Str(after?["status"]) == "delivered", status = H.Str(after?["status"]),
                response_code = after?["response_code"], error = H.Str(after?["last_error"]) });
        });

        // ---------- outbox + ledger ----------
        app.MapGet("/api/admin/integrations/events", (HttpRequest req) => gate(req, "integrations", _ =>
            J(new { rows = db.Query("SELECT id,event_type,entity_type,entity_id,payload,created_at FROM integration_events ORDER BY id DESC LIMIT 100") })));

        app.MapGet("/api/admin/integrations/deliveries", (HttpRequest req) => gate(req, "integrations", _ =>
            J(new { rows = db.Query(@"SELECT d.id,d.event_id,d.integration_id,d.status,d.attempts,d.response_code,d.last_error,d.next_attempt_at,d.updated_at,
                                             e.event_type, i.name integration_name
                                      FROM integration_deliveries d
                                      LEFT JOIN integration_events e ON e.id=d.event_id
                                      LEFT JOIN integrations i ON i.id=d.integration_id
                                      ORDER BY d.id DESC LIMIT 100") })));

        app.MapPost("/api/admin/integrations/deliveries/{id}/retry", (HttpRequest req, long id) => gate(req, "integrations", _ =>
        {
            var d = db.QueryOne("SELECT id FROM integration_deliveries WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE integration_deliveries SET status='pending', next_attempt_at=datetime('now'), last_error=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(null, "integration.retry", id.ToString());
            return J(new { ok = true });   // the dispatcher will pick it up on its next tick
        }));
    }
}
