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

        // Which structured secret sub-fields are set (values are NEVER returned — write-only).
        // QuickBooks and Zoho are both OAuth (client id/secret + refresh token); Odoo authenticates with a
        // single user API key. A webhook keeps one opaque signing secret.
        string[] OAuthSecretKeys = { "client_secret", "refresh_token", "access_token" };
        string[] OdooSecretKeys = { "api_key" };
        string[] SecretKeysFor(string provider) => provider switch
        {
            "quickbooks" or "zoho" => OAuthSecretKeys,
            "odoo" => OdooSecretKeys,
            _ => Array.Empty<string>(),
        };
        object Redact(Dictionary<string, object?> r)
        {
            var provider = H.Str(r["provider"]) ?? "webhook";
            var secretRaw = H.Str(r["secret"]) ?? "";
            var secretFields = new List<string>();
            var structuredKeys = SecretKeysFor(provider);
            if (structuredKeys.Length > 0)
            {
                try
                {
                    // Stored structured secrets are envelope-encrypted; decrypt before reporting which
                    // fields are populated (the values themselves are never returned).
                    var plain = Security.DecryptSecret(secretRaw) ?? secretRaw;
                    var e = System.Text.Json.JsonDocument.Parse(plain.Length > 0 ? plain : "{}").RootElement;
                    foreach (var k in structuredKeys)
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
                    new { key = "zoho", label = "Zoho Books", description = "Members → Contacts, payments → Invoices, via the Zoho Books API (OAuth, region-aware)." },
                    new { key = "odoo", label = "Odoo", description = "Members → res.partner, payments → customer invoices, via Odoo JSON-RPC (database + API key)." },
                },
                // Field metadata so the console renders each ERP's setup form (and its help text) from
                // the server rather than hardcoding a form per vendor: adding a connector stays a
                // backend change. `secret: true` fields are write-only and never read back.
                provider_fields = new
                {
                    zoho = new
                    {
                        setup = "Zoho API console → Self Client → generate a refresh token with the scopes below, then copy your Organization ID from Zoho Books → Settings → Organizations.",
                        scopes = ZohoConnector.RequiredScopes,
                        rate_limit = $"{ZohoConnector.RateLimitPerMinute} API calls/minute per organization (HTTP 429 beyond). A payment costs up to 3 calls.",
                        fields = new object[]
                        {
                            new { key = "organization_id", label = "Organization ID", required = true, help = "Zoho Books → Settings → Organizations. Sent on every request; without it Zoho answers 'Organization not found' whatever the token." },
                            new { key = "region", label = "Data centre", required = true, type = "select", options = new[] { "com", "eu", "in", "com.au", "jp", "ca" }, help = "The data centre your Zoho account lives in. A token minted in one region is INVALID in another, so this must match your account." },
                            new { key = "client_id", label = "Client ID", required = false, help = "From the self-client you created. Needed only if you supply a refresh token rather than a direct access token." },
                            new { key = "client_secret", label = "Client secret", required = false, secret = true },
                            new { key = "refresh_token", label = "Refresh token", required = false, secret = true, help = "Preferred: the connector mints short-lived access tokens from it automatically." },
                            new { key = "access_token", label = "Access token", required = false, secret = true, help = "Optional shortcut for testing. Zoho access tokens expire in ~1 hour, so a refresh token is the durable choice." },
                            new { key = "invoice_status", label = "Invoice status", required = false, type = "select", options = new[] { "draft", "sent" }, help = "draft (default) creates the invoice without notifying the customer. 'sent' makes Zoho mail it — turn on deliberately." },
                            new { key = "item_id", label = "Books item ID", required = false, help = "Bill against an existing Books item so invoices land on your chart of accounts. Without it an ad-hoc line is used." },
                            new { key = "tax_id", label = "Tax ID", required = false },
                            new { key = "currency", label = "Currency code", required = false, help = "e.g. USD. Leave blank to use the organisation default." },
                            new { key = "place_of_supply", label = "Place of supply", required = false, help = "Required by GST/VAT jurisdictions." },
                            new { key = "payment_terms", label = "Payment terms (days)", required = false },
                            new { key = "salesperson", label = "Salesperson name", required = false },
                            new { key = "notes", label = "Invoice notes", required = false },
                            new { key = "contact_reuse", label = "Reuse existing contact by email", required = false, type = "bool", @default = true, help = "On (default): a returning payer reuses their contact. Off: every payment mints a new contact, duplicating customers." },
                            new { key = "api_base", label = "API base override", required = false, help = "Point deliveries at a test receiver. The OAuth accounts host is deliberately NOT redirected, so real credentials are never sent to it." },
                        },
                    },
                    odoo = new
                    {
                        setup = "Odoo → Preferences → Account Security → New API Key. Use your login email and that key; the database name is the one in your instance URL.",
                        notes = "Invoices are created as DRAFTS. Posting is a separate step in Odoo, deliberately left to an accountant.",
                        fields = new object[]
                        {
                            new { key = "url", label = "Instance URL", required = true, help = "https://your-instance.odoo.com — the connector posts JSON-RPC to {url}/jsonrpc." },
                            new { key = "database", label = "Database name", required = true },
                            new { key = "login", label = "Login (user email)", required = true },
                            new { key = "api_key", label = "API key", required = true, secret = true, help = "Used in place of the password. Generated per user in Odoo." },
                            new { key = "company_id", label = "Company ID", required = false, help = "Numeric res.company id — needed on multi-company databases." },
                            new { key = "journal_id", label = "Journal ID", required = false, help = "Numeric account.journal id the invoice posts to." },
                            new { key = "product_id", label = "Product ID", required = false, help = "Numeric product.product id the invoice line bills." },
                            new { key = "account_id", label = "Account ID", required = false },
                            new { key = "tax_id", label = "Tax ID", required = false, help = "Numeric account.tax id, applied as the line's only tax." },
                            new { key = "currency_id", label = "Currency ID", required = false },
                            new { key = "payment_term_id", label = "Payment term ID", required = false },
                            new { key = "team_id", label = "Sales team ID", required = false },
                            new { key = "partner_reuse", label = "Reuse existing partner by email", required = false, type = "bool", @default = true },
                            new { key = "partner_model", label = "Partner model override", required = false, @default = OdooConnector.PartnerModel },
                            new { key = "invoice_model", label = "Invoice model override", required = false, @default = OdooConnector.InvoiceModel },
                        },
                    },
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
            if (provider is not ("webhook" or "quickbooks" or "zoho" or "odoo"))
                return Results.Json(new { error = "unsupported_provider", message = "Supported providers: webhook, quickbooks, zoho, odoo." }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? "").Trim();
            if (name.Length == 0) name = provider switch
            {
                "quickbooks" => "QuickBooks Online",
                "zoho" => "Zoho Books",
                "odoo" => "Odoo",
                _ => "Webhook",
            };
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
            // Zoho Books: organization + data-centre region (a token is only valid in the region that
            // minted it), plus an optional API-host override for pointing at a test receiver.
            else if (provider == "zoho")
            {
                var cfg = new Dictionary<string, object?>
                {
                    ["organization_id"] = (H.GetS(b, "organization_id") ?? "").Trim(),
                    ["region"] = ZohoConnector.NormaliseRegion(H.GetS(b, "region")),
                    ["client_id"] = (H.GetS(b, "client_id") ?? "").Trim(),
                };
                // Optional accounting mapping — each is omitted from the invoice when unset so Books
                // applies the organisation's own default rather than being overridden with a blank.
                foreach (var k in new[] { "currency", "place_of_supply", "tax_id", "item_id", "salesperson", "payment_terms", "notes" })
                    if (H.GetS(b, k) is { Length: > 0 } v) cfg[k] = v.Trim();
                if (H.GetS(b, "invoice_status") is { Length: > 0 } inv)
                {
                    var st = inv.Trim().ToLowerInvariant();
                    if (st is not ("draft" or "sent")) return Results.Json(new { error = "bad_invoice_status", message = "invoice_status must be draft or sent." }, statusCode: 400);
                    cfg["invoice_status"] = st;
                }
                if (H.GetEl(b, "contact_reuse") is { ValueKind: JsonValueKind.False }) cfg["contact_reuse"] = false;
                if (H.GetS(b, "api_base") is { Length: > 0 } zab)
                {
                    if (Egress.UrlProblem(zab.Trim()) is { } zabProb)
                        return Results.Json(new { error = "bad_api_base", message = zabProb }, statusCode: 400);
                    cfg["api_base"] = zab.Trim();
                }
                configJson = JsonSerializer.Serialize(cfg);
            }
            // Odoo: the instance URL is an outbound destination like any other, so it obeys the same
            // egress rules; database + login identify the account the API key belongs to.
            else if (provider == "odoo")
            {
                var url = (H.GetS(b, "url") ?? "").Trim();
                if (url.Length == 0) return Results.Json(new { error = "url_required", message = "Odoo needs the instance URL (https://your-instance.odoo.com)." }, statusCode: 400);
                if (Egress.UrlProblem(url) is { } urlProb)
                    return Results.Json(new { error = "bad_url", message = urlProb }, statusCode: 400);
                var cfg = new Dictionary<string, object?>
                {
                    ["url"] = url.TrimEnd('/'),
                    ["database"] = (H.GetS(b, "database") ?? "").Trim(),
                    ["login"] = (H.GetS(b, "login") ?? "").Trim(),
                };
                if (H.GetS(b, "partner_model") is { Length: > 0 } pm) cfg["partner_model"] = pm.Trim();
                if (H.GetS(b, "invoice_model") is { Length: > 0 } im) cfg["invoice_model"] = im.Trim();
                // Optional accounting mapping. These are Odoo record IDs: a non-numeric value would be
                // silently ignored by the connector, so it is rejected here where the operator can see it.
                foreach (var k in new[] { "company_id", "journal_id", "product_id", "account_id", "tax_id", "currency_id", "payment_term_id", "team_id" })
                    if (H.GetS(b, k) is { Length: > 0 } raw)
                    {
                        if (!long.TryParse(raw.Trim(), out var idVal) || idVal <= 0)
                            return Results.Json(new { error = "bad_" + k, message = $"{k} must be a positive Odoo record id." }, statusCode: 400);
                        cfg[k] = idVal;
                    }
                if (H.GetEl(b, "partner_reuse") is { ValueKind: JsonValueKind.False }) cfg["partner_reuse"] = false;
                configJson = JsonSerializer.Serialize(cfg);
            }
            // Secret handling is write-only. Webhook: a plain signing-secret string. QuickBooks/Zoho: a JSON
            // of OAuth secrets; Odoo: a JSON holding the user API key. Structured secrets are merged with
            // what's stored so a single field can be updated without re-entering all of them.
            // EXT-P1-03 — envelope-encrypt secrets at rest.
            var structuredKeys = SecretKeysFor(provider);
            string MergeStructured(string? existing)
            {
                var plain = Security.DecryptSecret(existing) ?? existing;
                Dictionary<string, string> cur = new();
                try { if (!string.IsNullOrEmpty(plain)) cur = JsonSerializer.Deserialize<Dictionary<string, string>>(plain) ?? new(); } catch { }
                foreach (var k in structuredKeys)
                    if (H.GetS(b, k) is { } v) { if (v.Length == 0) cur.Remove(k); else cur[k] = v; }
                return Security.EncryptSecret(JsonSerializer.Serialize(cur)) ?? JsonSerializer.Serialize(cur);
            }
            bool AnyStructuredSecretInBody() => structuredKeys.Any(k => H.GetEl(b, k) is not null);

            if (id > 0 && db.QueryOne("SELECT secret FROM integrations WHERE id=?", id) is { } existing)
            {
                db.Execute("UPDATE integrations SET provider=?, name=?, endpoint_url=?, config=COALESCE(?,config), event_filter=?, enabled=?, updated_at=datetime('now') WHERE id=?",
                    provider, name, endpoint, configJson, filter, enabled, id);
                if (structuredKeys.Length > 0)
                {
                    if (AnyStructuredSecretInBody())
                        db.Execute("UPDATE integrations SET secret=? WHERE id=?", MergeStructured(H.Str(existing["secret"])), id);
                }
                else if (H.GetEl(b, "secret") is { ValueKind: JsonValueKind.String } sEl)
                    db.Execute("UPDATE integrations SET secret=? WHERE id=?", Security.EncryptSecret(sEl.GetString() ?? "") ?? "", id);
                log(actorId, "integration.update", $"{id} {name} ({provider})");
                return J(new { ok = true, id });
            }
            var secretVal = structuredKeys.Length > 0 ? MergeStructured(null) : (Security.EncryptSecret(H.GetS(b, "secret") ?? "") ?? "");
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
