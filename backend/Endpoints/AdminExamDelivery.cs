using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Exam Delivery. Configure the third-party certification exam-delivery / proctoring
/// vendors (Pearson VUE/OnVUE, Kryterion Webassessor, PSI, TestReach, Questionmark), map PCI
/// certifications to each vendor's exam codes, drive the scheduling lifecycle for a booking, and ingest
/// results — issuing the PCI credential on a vendor-graded pass.
///
///   GET  /api/admin/exam-delivery                      — providers (secret redacted), connector catalogue, certs
///   POST /api/admin/exam-delivery                      — create / update a provider (secret write-only, merged)
///   POST /api/admin/exam-delivery/{id}/delete
///   POST /api/admin/exam-delivery/{id}/test            — TestConnection against the vendor (or mock)
///   GET  /api/admin/exam-delivery/orders               — routed bookings + their lifecycle status
///   GET  /api/admin/exam-delivery/orders/{id}/log      — the per-order API call audit trail
///   POST /api/admin/exam-delivery/orders/{id}/provision — run candidate→authorize→schedule now
///   POST /api/admin/exam-delivery/orders/{id}/sync      — pull status + results; issue credential on a pass
///   POST /api/admin/exam-delivery/orders/{id}/cancel    — cancel the appointment/eligibility at the vendor
///
/// Inbound (public, per-provider shared-secret verified) — how PSI/Pearson/TestReach push back:
///   POST /api/exam-delivery/callback/{provider}         — appointment + result callbacks (RTEN / Notification)
///
/// Admin routes are gated by the 'exam_delivery' permission; every mutation is audit-logged.
/// </summary>
public static class AdminExamDelivery
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        var http = Egress.CreateClient(TimeSpan.FromSeconds(20));   // SSRF-guarded (see ExamDelivery.Http)
        IResult? Deny(HttpRequest req)
        {
            var r = gate(req, "exam_delivery", _ => Results.Ok());
            return r is Microsoft.AspNetCore.Http.HttpResults.Ok ? null : r;
        }

        object Redact(Dictionary<string, object?> r)
        {
            var provider = H.Str(r["provider"]) ?? "";
            var connector = ExamDelivery.Get(provider);
            var secretRaw = H.Str(r["secret"]) ?? "";
            var setSecrets = new List<string>();
            if (connector is not null && secretRaw.Length > 0)
                try { var e = JsonDocument.Parse(secretRaw).RootElement;
                      foreach (var k in connector.SecretFields)
                          if (e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String && (v.GetString() ?? "").Length > 0) setSecrets.Add(k); }
                catch { }
            object? config = null;
            if (H.Str(r["config"]) is { Length: > 0 } cfg) { try { config = JsonSerializer.Deserialize<Dictionary<string, object?>>(cfg); } catch { } }
            return new
            {
                id = H.L(r["id"]), provider, name = H.Str(r["name"]), label = connector?.Label ?? provider,
                enabled = H.L(r["enabled"]), is_default = H.L(r["is_default"]), environment = H.Str(r["environment"]),
                config, secret_fields = setSecrets, status = H.Str(r["status"]),
                last_sync_at = H.Str(r["last_sync_at"]), created_at = H.Str(r["created_at"]),
            };
        }

        // ---------- list providers + connector catalogue + certifications + the delivery-mode switch ----------
        app.MapGet("/api/admin/exam-delivery", (HttpRequest req) => gate(req, "exam_delivery", _ =>
        {
            var certs = db.Query("SELECT id,code,name FROM certifications WHERE COALESCE(active,1)=1 ORDER BY id").ToList();
            var certModes = certs.ToDictionary(c => H.L(c["id"]).ToString(), c => Core.Settings.Str(db, $"exam_delivery_mode:{H.L(c["id"])}", "inherit"));
            return J(new
            {
                rows = db.Query("SELECT * FROM exam_delivery_providers ORDER BY id").Select(Redact),
                connectors = ExamDelivery.All.Select(c => new
                {
                    key = c.Slug, label = c.Label, description = c.Description,
                    config_fields = c.ConfigFields, secret_fields = c.SecretFields,
                }),
                certifications = certs,
                mode = Core.Settings.Str(db, "exam_delivery_mode", ExamDelivery.InHouse),   // global: in_house or a vendor slug
                cert_modes = certModes,                                                       // per-cert: slug | in_house | inherit
            });
        }));

        // ---------- the delivery-mode switch: choose PCI's own SecureExam or a vendor (global + per-cert) ----------
        app.MapPost("/api/admin/exam-delivery/mode", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            bool Valid(string m) => m == ExamDelivery.InHouse || m == "inherit" || ExamDelivery.Get(m) is not null;
            if (H.GetS(b, "mode") is { } gm)
            {
                var m = gm.Trim().ToLowerInvariant(); if (m == "inherit") m = ExamDelivery.InHouse;
                if (!Valid(m)) return Results.Json(new { error = "bad_mode" }, statusCode: 400);
                Core.Settings.Put(db, "exam_delivery_mode", m);
                // keep the provider is_default flag coherent with the global choice
                if (m != ExamDelivery.InHouse) { db.Execute("UPDATE exam_delivery_providers SET is_default=0"); db.Execute("UPDATE exam_delivery_providers SET is_default=1 WHERE provider=?", m); }
                log(actorId, "exam_delivery.mode", $"global={m}");
            }
            if (H.GetNum(b, "certification_id") is { } cidn && H.GetS(b, "cert_mode") is { } cmRaw)
            {
                var cid = (long)cidn; var cm = cmRaw.Trim().ToLowerInvariant();
                if (!Valid(cm)) return Results.Json(new { error = "bad_mode" }, statusCode: 400);
                Core.Settings.Put(db, $"exam_delivery_mode:{cid}", cm);   // 'inherit' clears the override
                log(actorId, "exam_delivery.mode", $"cert {cid}={cm}");
            }
            return J(new { ok = true, mode = Core.Settings.Str(db, "exam_delivery_mode", ExamDelivery.InHouse) });
        });

        // ---------- create / update a provider ----------
        app.MapPost("/api/admin/exam-delivery", async (HttpContext ctx) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var id = (long)(H.GetNum(b, "id") ?? 0);
            var provider = (H.GetS(b, "provider") ?? "").Trim().ToLowerInvariant();
            var connector = ExamDelivery.Get(provider);
            if (connector is null) return Results.Json(new { error = "unsupported_provider", supported = ExamDelivery.All.Select(c => c.Slug) }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? "").Trim(); if (name.Length == 0) name = connector.Label;
            var env = (H.GetS(b, "environment") ?? "sandbox").Trim().ToLowerInvariant(); if (env is not ("sandbox" or "production")) env = "sandbox";
            var enabled = H.GetEl(b, "enabled") is { } en && (en.ValueKind == JsonValueKind.True || (en.ValueKind == JsonValueKind.String && en.GetString() is "1" or "true") || (en.ValueKind == JsonValueKind.Number && en.TryGetInt32(out var ei) && ei != 0)) ? 1 : 0;
            var isDefault = H.GetEl(b, "is_default") is { } df && (df.ValueKind == JsonValueKind.True || (df.ValueKind == JsonValueKind.String && df.GetString() is "1" or "true") || (df.ValueKind == JsonValueKind.Number && df.TryGetInt32(out var di) && di != 0)) ? 1 : 0;

            // config = the connector's declared config fields + exam_map + callback_secret (non-secret).
            var cfg = new Dictionary<string, object?>();
            foreach (var f in connector.ConfigFields) if (H.GetS(b, f) is { } v) cfg[f] = v;
            // Reject an api_base that points at a private/loopback/metadata address on save (fast operator
            // feedback; the SSRF-guarded client is the actual runtime control).
            if (cfg.TryGetValue("api_base", out var abv) && abv is string abs && abs.Trim().Length > 0
                && Egress.UrlProblem(abs.Trim()) is { } abProb)
                return Results.Json(new { error = "bad_api_base", message = abProb }, statusCode: 400);
            if (H.GetEl(b, "exam_map") is { ValueKind: JsonValueKind.Object } em)
            {
                var map = new Dictionary<string, string>();
                foreach (var pr in em.EnumerateObject()) if (pr.Value.ValueKind == JsonValueKind.String && pr.Value.GetString() is { Length: > 0 } code) map[pr.Name] = code;
                cfg["exam_map"] = map;
            }
            if (H.GetS(b, "callback_secret") is { } csct) cfg["callback_secret"] = csct;
            var configJson = JsonSerializer.Serialize(cfg);

            // secret = write-only, merged with what's stored so one field can change without re-entering all.
            string MergeSecret(string? existing)
            {
                Dictionary<string, string> cur = new();
                try { if (!string.IsNullOrEmpty(existing)) cur = JsonSerializer.Deserialize<Dictionary<string, string>>(existing) ?? new(); } catch { }
                foreach (var k in connector.SecretFields) if (H.GetS(b, k) is { } v) { if (v.Length == 0) cur.Remove(k); else cur[k] = v; }
                return JsonSerializer.Serialize(cur);
            }
            bool AnySecretInBody() => connector.SecretFields.Any(k => H.GetEl(b, k) is not null);

            // a single default provider: setting this one default clears the others.
            void ApplyDefault(long pid) { if (isDefault == 1) db.Execute("UPDATE exam_delivery_providers SET is_default=0 WHERE id<>?", pid); }

            if (id > 0 && db.QueryOne("SELECT secret FROM exam_delivery_providers WHERE id=?", id) is { } existing)
            {
                db.Execute("UPDATE exam_delivery_providers SET provider=?, name=?, environment=?, config=?, enabled=?, is_default=?, updated_at=datetime('now') WHERE id=?",
                    provider, name, env, configJson, enabled, isDefault, id);
                if (AnySecretInBody()) db.Execute("UPDATE exam_delivery_providers SET secret=? WHERE id=?", MergeSecret(H.Str(existing["secret"])), id);
                ApplyDefault(id);
                log(actorId, "exam_delivery.update", $"{id} {name} ({provider})");
                return J(new { ok = true, id });
            }
            var newId = db.ExecuteReturningId("INSERT INTO exam_delivery_providers(provider,name,environment,config,secret,enabled,is_default) VALUES(?,?,?,?,?,?,?)",
                provider, name, env, configJson, MergeSecret(null), enabled, isDefault);
            ApplyDefault(newId);
            log(actorId, "exam_delivery.create", $"{newId} {name} ({provider})");
            return J(new { ok = true, id = newId });
        });

        app.MapPost("/api/admin/exam-delivery/{id}/delete", (HttpRequest req, long id) => gate(req, "exam_delivery", adm =>
        {
            db.Execute("DELETE FROM exam_delivery_providers WHERE id=?", id);
            log(adm.Id, "exam_delivery.delete", id.ToString());
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/exam-delivery/{id}/test", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE id=?", id);
            if (prov is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var connector = ExamDelivery.Get(H.Str(prov["provider"]));
            if (connector is null) return Results.Json(new { error = "no_connector" }, statusCode: 400);
            ConnResult r; try { r = await connector.TestConnection(http, ExamDelivery.CtxFor(prov)); } catch (Exception ex) { r = ConnResult.Fail(ex.Message); }
            db.Execute("UPDATE exam_delivery_providers SET status=? WHERE id=?", r.Ok ? "ok" : "error", id);
            log(actorId, "exam_delivery.test", $"{id} → {(r.Ok ? "ok" : "error")}");
            return J(new { ok = r.Ok, code = r.Code, detail = r.Detail });
        });

        // ---------- orders (routed bookings) ----------
        app.MapGet("/api/admin/exam-delivery/orders", (HttpRequest req) => gate(req, "exam_delivery", _ =>
            J(new { rows = db.Query(@"SELECT o.*, p.name provider_name, u.email candidate_email, u.first_name, u.last_name
                                      FROM exam_delivery_orders o
                                      LEFT JOIN exam_delivery_providers p ON p.id=o.provider_id
                                      LEFT JOIN users u ON u.id=o.user_id
                                      ORDER BY o.id DESC LIMIT 200") })));

        app.MapGet("/api/admin/exam-delivery/orders/{id}/log", (HttpRequest req, long id) => gate(req, "exam_delivery", _ =>
            J(new { rows = db.Query("SELECT id,operation,ok,response_code,detail,created_at FROM exam_delivery_log WHERE order_id=? ORDER BY id DESC LIMIT 100", id) })));

        app.MapPost("/api/admin/exam-delivery/orders/{id}/provision", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var r = await ExamDelivery.Provision(db, http, id);
            log(actorId, "exam_delivery.provision", $"order {id} → {(r.Ok ? "ok" : "fail")}");
            var row = db.QueryOne("SELECT status FROM exam_delivery_orders WHERE id=?", id);
            return J(new { ok = r.Ok, status = H.Str(row?["status"]), detail = r.Detail });
        });

        app.MapPost("/api/admin/exam-delivery/orders/{id}/sync", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            await ExamDelivery.Sync(db, http, id);
            var cred = ExamDeliveryCredits.MaybeIssue(db, id);   // issue the PCI credential if the vendor graded a pass
            var row = db.QueryOne("SELECT status,result_status,score FROM exam_delivery_orders WHERE id=?", id);
            log(actorId, "exam_delivery.sync", $"order {id} → {H.Str(row?["result_status"])}");
            return J(new { ok = true, status = H.Str(row?["status"]), result_status = H.Str(row?["result_status"]), credential = cred });
        });

        app.MapPost("/api/admin/exam-delivery/orders/{id}/cancel", async (HttpContext ctx, long id) =>
        {
            var deny = Deny(ctx.Request); if (deny is not null) return deny;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var row = db.QueryOne("SELECT * FROM exam_delivery_orders WHERE id=?", id);
            if (row is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE id=?", row["provider_id"]);
            var connector = ExamDelivery.Get(H.Str(prov?["provider"]));
            if (connector is null || prov is null) return Results.Json(new { error = "no_connector" }, statusCode: 400);
            ConnResult r; try { r = await connector.Cancel(http, ExamDelivery.CtxFor(prov), ExamDelivery.OrderFor(db, row)); } catch (Exception ex) { r = ConnResult.Fail(ex.Message); }
            if (r.Ok) db.Execute("UPDATE exam_delivery_orders SET status='cancelled', updated_at=datetime('now') WHERE id=?", id);
            log(actorId, "exam_delivery.cancel", $"order {id} → {(r.Ok ? "ok" : "fail")}");
            return J(new { ok = r.Ok, detail = r.Detail });
        });

        // ---------- inbound vendor callback (PSI Notification / Pearson RTEN / TestReach results push) ----------
        // Public endpoint, verified by the per-provider callback_secret (header X-PCI-Callback-Token or ?token=).
        // Matches the order by any external id present in the payload, records appointment/result, and issues
        // the PCI credential on a graded pass. Vendors that PULL (Kryterion/Questionmark) don't use this.
        app.MapPost("/api/exam-delivery/callback/{provider}", async (HttpContext ctx, string provider) =>
        {
            var body = await H.Body(ctx.Request);
            var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE provider=? AND enabled=1 ORDER BY id DESC", provider.ToLowerInvariant());
            if (prov is null) return Results.Json(new { error = "unknown_provider" }, statusCode: 404);
            var p = ExamDelivery.CtxFor(prov);
            var expected = p.Cfg("callback_secret");
            if (string.IsNullOrEmpty(expected)) return Results.Json(new { error = "callback_not_configured" }, statusCode: 400);
            var token = ctx.Request.Headers["X-PCI-Callback-Token"].ToString();
            if (string.IsNullOrEmpty(token)) token = ctx.Request.Query["token"].ToString();
            if (!System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
                    System.Text.Encoding.UTF8.GetBytes(token ?? ""), System.Text.Encoding.UTF8.GetBytes(expected)))
                return Results.Json(new { error = "bad_token" }, statusCode: 401);

            // Locate the order by any external id the vendor echoes back.
            string? Get(params string[] k) => H.GetS(body, k);
            var candidate = Get("candidate_id", "clientCandidateId", "candidateReference", "externalReference", "login");
            var registration = Get("client_eligibility_id", "authorizationId", "enrolmentId", "registration_id", "clientAuthorizationId");
            var appointment = Get("booking_code", "confirmationNumber", "appointmentId", "orderNumber", "registrationId");
            var order = FindOrder(db, p.Id, candidate, registration, appointment);
            if (order is null) return Results.Json(new { error = "order_not_found" }, statusCode: 404);
            var orderId = H.L(order["id"]);

            // Appointment info (schedule/reschedule/cancel events).
            if (appointment is { Length: > 0 }) db.Execute("UPDATE exam_delivery_orders SET external_appointment_id=?, confirmation_code=COALESCE(confirmation_code,?), status=CASE WHEN status IN ('completed','cancelled') THEN status ELSE 'scheduled' END, updated_at=datetime('now') WHERE id=?", appointment, appointment, orderId);
            var evStatus = Get("event", "status", "type")?.ToLowerInvariant();
            if (evStatus is not null && (evStatus.Contains("cancel"))) db.Execute("UPDATE exam_delivery_orders SET status='cancelled', updated_at=datetime('now') WHERE id=?", orderId);

            // Result (pass/fail) if present.
            var pf = Web.PassFail(Get("result", "grade", "passed", "outcome"));
            string? cred = null;
            if (pf is not null)
            {
                db.Execute("UPDATE exam_delivery_orders SET result_status=?, score=?, max_score=?, raw_result=?, status='completed', updated_at=datetime('now') WHERE id=?",
                    pf, H.GetNum(body, "score") is { } sc ? sc : (object?)null, H.GetNum(body, "max_score", "maxScore") is { } mx ? mx : (object?)null,
                    ctx.Request.ContentLength is > 0 ? "callback" : null, orderId);
                cred = ExamDeliveryCredits.MaybeIssue(db, orderId);
            }
            db.Execute("INSERT INTO exam_delivery_log(order_id,provider_id,provider,operation,ok,response_code,detail) VALUES(?,?,?,?,1,200,?)",
                orderId, p.Id, provider.ToLowerInvariant(), "callback", $"{evStatus ?? "event"} {pf ?? ""}".Trim());
            log(null, "exam_delivery.callback", $"order {orderId} {provider} {pf ?? evStatus}");
            return J(new { ok = true, order_id = orderId, result_status = pf, credential = cred });
        });
    }

    static Dictionary<string, object?>? FindOrder(Db db, long providerId, string? candidate, string? registration, string? appointment)
    {
        foreach (var (col, val) in new[] { ("external_appointment_id", appointment), ("confirmation_code", appointment), ("external_registration_id", registration), ("external_candidate_id", candidate) })
            if (val is { Length: > 0 } && db.QueryOne($"SELECT * FROM exam_delivery_orders WHERE provider_id=? AND {col}=? ORDER BY id DESC", providerId, val) is { } hit)
                return hit;
        return null;
    }
}

/// <summary>Issues the PCI credential when an exam-delivery order is graded a pass by the vendor. Idempotent
/// per order via an exam_delivery_log 'credential' marker, so a re-sync or a duplicate callback never issues
/// twice. External exams have no PCI attempt row, so the credential is issued against the user + certification
/// with the order's holder name.</summary>
public static class ExamDeliveryCredits
{
    public static string? MaybeIssue(Db db, long orderId)
    {
        var o = db.QueryOne("SELECT * FROM exam_delivery_orders WHERE id=?", orderId);
        if (o is null || H.Str(o["result_status"]) != "pass") return null;
        // idempotency: already issued for this order?
        var prior = db.QueryOne("SELECT detail FROM exam_delivery_log WHERE order_id=? AND operation='credential' AND ok=1 ORDER BY id DESC", orderId);
        if (prior is not null) return H.Str(prior["detail"]);
        var userId = H.L(o["user_id"]);
        var user = db.QueryOne("SELECT first_name,last_name FROM users WHERE id=?", userId) ?? new();
        var holder = $"{H.Str(user.GetValueOrDefault("first_name"))} {H.Str(user.GetValueOrDefault("last_name"))}".Trim();
        if (holder.Length == 0) holder = "PCI Candidate";
        var certId = H.L(o["certification_id"]); if (certId == 0) certId = Certs.DefaultId;
        var cid = Lifecycle.IssueCredential(db, userId, null, holder, certId);
        db.Execute("INSERT INTO exam_delivery_log(order_id,provider_id,provider,operation,ok,response_code,detail) VALUES(?,?,?,?,?,?,?)",
            orderId, H.L(o["provider_id"]), H.Str(o["provider"]), "credential", cid is null ? 0 : 1, 200, cid);
        return cid;
    }
}
