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
        // Egress-guarded like the dispatcher's client — the admin "test" action uses the same
        // delivery path and must obey the same private-address restrictions (Core/Egress.cs).
        var http = Egress.CreateClient(TimeSpan.FromSeconds(15));

        // Which QuickBooks secret sub-fields are set (values are NEVER returned — write-only).
        string[] QboSecretKeys = { "client_secret", "refresh_token", "access_token" };
        object Redact(Dictionary<string, object?> r)
        {
            var provider = H.Str(r["provider"]) ?? "webhook";
            var secretRaw = H.Str(r["secret"]) ?? "";
            var secretFields = new List<string>();
            if (provider == "quickbooks")
            {
                try
                {
                    var e = System.Text.Json.JsonDocument.Parse(secretRaw.Length > 0 ? secretRaw : "{}").RootElement;
                    foreach (var k in QboSecretKeys)
                        if (e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String && (v.GetString() ?? "").Length > 0) secretFields.Add(k);
                }
                catch { }
            }
            else if (secretRaw.Length > 0) secretFields.Add("secret");
            object? config = null;
            if (H.Str(r["config"]) is { Length: > 0 } cfg) { try { config = JsonSerializer.Deserialize<Dictionary<string, object?>>(cfg); } catch { } }
            return new
            {
                id = H.L(r["id"]), provider, name = H.Str(r["name"]),
                enabled = H.L(r["enabled"]), endpoint_url = H.Str(r["endpoint_url"]),
                has_secret = secretRaw.Length > 0, secret_fields = secretFields, config,
                event_filter = H.Str(r["event_filter"]), status = H.Str(r["status"]),
                last_delivery_at = H.Str(r["last_delivery_at"]), created_at = H.Str(r["created_at"]),
            };
        }

        // ---------- list connectors + catalogues (events, providers) ----------
        app.MapGet("/api/admin/integrations", (HttpRequest req) => gate(req, "integrations", _ =>
            J(new
            {
                rows = db.Query("SELECT * FROM integrations ORDER BY id").Select(Redact),
                events = Integrations.KnownEvents,
                providers = new object[]
                {
                    new { key = "webhook", label = "Generic webhook", description = "Signed JSON POST to any HTTPS endpoint or automation bridge." },
                    new { key = "quickbooks", label = "QuickBooks Online", description = "Members → Customers, payments → Sales Receipts, via the QuickBooks API (OAuth)." },
                },
            })));

        // ---------- create / update ----------
        app.MapPost("/api/admin/integrations", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var id = (long)(H.GetNum(b, "id") ?? 0);
            var provider = (H.GetS(b, "provider") ?? "webhook").Trim().ToLowerInvariant();
            if (provider is not ("webhook" or "quickbooks"))
                return Results.Json(new { error = "unsupported_provider", message = "Supported providers: webhook, quickbooks." }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? "").Trim();
            if (name.Length == 0) name = provider == "quickbooks" ? "QuickBooks Online" : "Webhook";
            var endpoint = (H.GetS(b, "endpoint_url") ?? "").Trim();
            if (provider == "webhook" && endpoint.Length > 0 && Egress.UrlProblem(endpoint) is { } prob)
                return Results.Json(new { error = "bad_endpoint", message = prob }, statusCode: 400);
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
            // QuickBooks: non-secret config (company/realm, environment, sales item, base override) as JSON.
            string? configJson = null;
            if (provider == "quickbooks")
            {
                var env = (H.GetS(b, "environment") ?? "production").Trim().ToLowerInvariant();
                if (env is not ("production" or "sandbox")) env = "production";
                var cfg = new Dictionary<string, object?>
                {
                    ["realm_id"] = (H.GetS(b, "realm_id") ?? "").Trim(),
                    ["environment"] = env,
                    ["item_ref"] = (H.GetS(b, "item_ref") is { Length: > 0 } ir ? ir.Trim() : "1"),
                    ["client_id"] = (H.GetS(b, "client_id") ?? "").Trim(),
                };
                if (H.GetS(b, "api_base") is { Length: > 0 } ab)
                {
                    // The api_base override is an outbound URL too — same egress rules as a webhook endpoint.
                    if (Egress.UrlProblem(ab.Trim()) is { } abProb)
                        return Results.Json(new { error = "bad_api_base", message = abProb }, statusCode: 400);
                    cfg["api_base"] = ab.Trim();
                }
                configJson = JsonSerializer.Serialize(cfg);
            }
            // Secret handling is write-only. Webhook: a plain signing-secret string. QuickBooks: a JSON of
            // OAuth secrets, merged with what's stored so a single field can be updated without re-entering all.
            // EXT-P1-03 — envelope-encrypt secrets at rest.
            string MergeQbo(string? existing)
            {
                var plain = Security.DecryptSecret(existing) ?? existing;
                Dictionary<string, string> cur = new();
                try { if (!string.IsNullOrEmpty(plain)) cur = JsonSerializer.Deserialize<Dictionary<string, string>>(plain) ?? new(); } catch { }
                foreach (var k in QboSecretKeys)
                    if (H.GetS(b, k) is { } v) { if (v.Length == 0) cur.Remove(k); else cur[k] = v; }
                return Security.EncryptSecret(JsonSerializer.Serialize(cur)) ?? JsonSerializer.Serialize(cur);
            }
            bool AnyQboSecretInBody() => QboSecretKeys.Any(k => H.GetEl(b, k) is not null);

            if (id > 0 && db.QueryOne("SELECT secret FROM integrations WHERE id=?", id) is { } existing)
            {
                db.Execute("UPDATE integrations SET provider=?, name=?, endpoint_url=?, config=COALESCE(?,config), event_filter=?, enabled=?, updated_at=datetime('now') WHERE id=?",
                    provider, name, endpoint, configJson, filter, enabled, id);
                if (provider == "quickbooks")
                {
                    if (AnyQboSecretInBody())
                        db.Execute("UPDATE integrations SET secret=? WHERE id=?", MergeQbo(H.Str(existing["secret"])), id);
                }
                else if (H.GetEl(b, "secret") is { ValueKind: JsonValueKind.String } sEl)
                    db.Execute("UPDATE integrations SET secret=? WHERE id=?", Security.EncryptSecret(sEl.GetString() ?? "") ?? "", id);
                log(actorId, "integration.update", $"{id} {name} ({provider})");
                return J(new { ok = true, id });
            }
            var secretVal = provider == "quickbooks" ? MergeQbo(null) : (Security.EncryptSecret(H.GetS(b, "secret") ?? "") ?? "");
            var newId = db.ExecuteReturningId("INSERT INTO integrations(provider,name,endpoint_url,secret,config,event_filter,enabled) VALUES(?,?,?,?,?,?,?)",
                provider, name, endpoint, secretVal, configJson, filter, enabled);
            log(actorId, "integration.create", $"{newId} {name} ({provider})");
            return J(new { ok = true, id = newId });
        });

        app.MapPost("/api/admin/integrations/{id}/delete", (HttpRequest req, long id) => gate(req, "integrations", adm =>
        {
            db.Execute("DELETE FROM integration_deliveries WHERE integration_id=?", id);
            db.Execute("DELETE FROM integrations WHERE id=?", id);
            log(adm.Id, "integration.delete", id.ToString());
            return J(new { ok = true });
        }));

        // ---------- test: emit a ping and deliver it immediately ----------
        app.MapPost("/api/admin/integrations/{id}/test", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var integ = db.QueryOne("SELECT * FROM integrations WHERE id=?", id);
            if (integ is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var eventId = db.ExecuteReturningId("INSERT INTO integration_events(event_type,entity_type,entity_id,payload) VALUES('ping','test',?,?)",
                id, JsonSerializer.Serialize(new { message = "PCI integration test", integration_id = id, at = H.IsoNow }));
            db.Execute("INSERT OR IGNORE INTO integration_deliveries(event_id,integration_id,status,next_attempt_at) VALUES(?,?,'pending',datetime('now'))", eventId, id);
            var del = db.QueryOne("SELECT * FROM integration_deliveries WHERE event_id=? AND integration_id=?", eventId, id);
            if (del is not null) await Integrations.DeliverOne(db, http, del);
            var after = db.QueryOne("SELECT status,response_code,last_error FROM integration_deliveries WHERE event_id=? AND integration_id=?", eventId, id);
            log(actorId, "integration.test", id.ToString());
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

        app.MapPost("/api/admin/integrations/deliveries/{id}/retry", (HttpRequest req, long id) => gate(req, "integrations", adm =>
        {
            var d = db.QueryOne("SELECT id FROM integration_deliveries WHERE id=?", id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            db.Execute("UPDATE integration_deliveries SET status='pending', next_attempt_at=datetime('now'), last_error=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "integration.retry", id.ToString());
            return J(new { ok = true });   // the dispatcher will pick it up on its next tick
        }));
    }
}
