using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

public static class AdminProctoring
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        app.MapGet("/api/admin/exam-sessions", (HttpContext ctx) => gate(ctx.Request, "proctoring", _ =>
            J(new { rows = db.Query(@"
                SELECT a.id, a.user_id, a.kind, a.started_at, a.submitted_at, a.percent, a.result, a.status,
                       a.violations, a.identity_result, a.identity_confidence, a.evidence_count, a.review_status, a.client_kind,
                       a.result_status, a.hold_reason, a.released_at,
                       u.email, u.first_name, u.last_name,
                       (SELECT COUNT(*) FROM proctor_events pe WHERE pe.attempt_id=a.id AND pe.severity IN ('High','Critical')) high_events
                FROM exam_attempts a LEFT JOIN users u ON u.id=a.user_id
                WHERE a.kind='exam' ORDER BY a.id DESC") })));

        app.MapGet("/api/admin/exam-sessions/live", (HttpContext ctx) => gate(ctx.Request, "proctoring", _ =>
            J(new { rows = db.Query(@"
                SELECT a.id, a.user_id, a.started_at, a.duration_minutes, a.violations, a.identity_result,
                       a.evidence_count, a.client_kind, a.last_heartbeat_at,
                       u.email, u.first_name, u.last_name,
                       CAST(MAX(0,(julianday(a.started_at)+a.duration_minutes/1440.0-julianday('now'))*86400) AS INT) remaining_s,
                       CAST((julianday('now')-julianday(COALESCE(a.last_heartbeat_at,a.started_at)))*86400 AS INT) seconds_since_beat,
                       (SELECT COUNT(*) FROM proctor_events pe WHERE pe.attempt_id=a.id AND pe.severity IN ('High','Critical')) high_events,
                       (SELECT COUNT(*) FROM proctor_messages pm WHERE pm.attempt_id=a.id AND pm.sender='candidate') candidate_msgs
                FROM exam_attempts a LEFT JOIN users u ON u.id=a.user_id
                WHERE a.kind='exam' AND a.status='in_progress' ORDER BY a.id DESC"), now = H.IsoNow })));

        app.MapPost("/api/admin/exam-sessions/{id}/message", (HttpContext ctx, long id) => gate(ctx.Request, "proctoring", _ =>
        {
            var a = db.QueryOne("SELECT * FROM exam_attempts WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var body = (H.GetS(b, "body") ?? "").Trim();
            if (body.Length > 1000) body = body[..1000];
            if (body.Length == 0) return Results.Json(new { error = "body_required" }, statusCode: 400);
            db.Execute("INSERT INTO proctor_messages(attempt_id,user_id,sender,body) VALUES(?,?,?,?)", a["id"], a["user_id"], "proctor", body);
            log(0, "proctor_message", "attempt " + id);
            return J(new { ok = true });
        }));

        // Stream a single evidence artefact by id (proctoring-gated). Serves from blob storage via the
        // stored reference; falls back to the legacy inline data URI for pre-migration rows.
        app.MapGet("/api/admin/evidence/{id}", (HttpContext ctx, long id) => gate(ctx.Request, "proctoring", _ =>
        {
            var e = db.QueryOne("SELECT mime,storage_ref,data_uri FROM exam_evidence WHERE id=?", id);
            if (e is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var reference = H.Str(e["storage_ref"]);
            if (!string.IsNullOrEmpty(reference))
            {
                var got = Storage.Get(reference);
                if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                return Results.Bytes(got.Value.bytes, got.Value.mime);
            }
            var legacy = H.Str(e["data_uri"]); // pre-migration inline artefact
            if (!string.IsNullOrEmpty(legacy)) return Results.Content(legacy, "text/plain");
            return Results.Json(new { error = "not_found" }, statusCode: 404);
        }));

        app.MapGet("/api/admin/exam-sessions/{id}", (HttpContext ctx, long id) => gate(ctx.Request, "proctoring", _ =>
        {
            var a = db.QueryOne("SELECT * FROM exam_attempts WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var user = db.QueryOne("SELECT id,email,first_name,last_name FROM users WHERE id=?", a["user_id"]) ?? new();
            var booking = a["booking_id"] is not null ? db.QueryOne("SELECT * FROM exam_bookings WHERE id=?", a["booking_id"]) : null;
            var events = db.Query("SELECT type,severity,detail,evidence_ref,at FROM proctor_events WHERE attempt_id=? ORDER BY id ASC", id);
            var evidence = db.Query("SELECT id,kind,mime,size_bytes,storage_ref,created_at, (data_uri IS NOT NULL) AS legacy_inline FROM exam_evidence WHERE attempt_id=? ORDER BY id DESC", id);
            var identity = db.Query("SELECT result,confidence,note,created_at FROM identity_checks WHERE attempt_id=? ORDER BY id DESC", id);
            var messages = db.Query("SELECT sender,body,created_at,delivered_at FROM proctor_messages WHERE attempt_id=? ORDER BY id ASC", id);
            var credential = db.QueryOne("SELECT credential_id,status,issued_at,expires_at FROM issued_credentials WHERE attempt_id=?", id)
                ?? db.QueryOne("SELECT credential_id,status,issued_at,expires_at FROM issued_credentials WHERE user_id=? ORDER BY id DESC", a["user_id"]);
            var sev = new Dictionary<string, int>();
            foreach (var e in events) { var s = H.Str(e["severity"]) ?? "Info"; sev[s] = sev.GetValueOrDefault(s) + 1; }
            int answered = 0; try { answered = H.ToMap(JsonDocument.Parse(H.Str(a["answers"]) ?? "{}").RootElement).Count; } catch { }
            return J(new { attempt = a, user, booking, events, evidence, identity, credential, messages,
                summary = new { total_events = events.Count, by_severity = sev, answered } });
        }));

        // ── Result management (Section A rule 12): release-held / invalidate / reinstate / clear ──
        // All actions are lifecycle-aware (result_status / hold_reason / released_at), audited with the
        // acting admin's id, use the CONFIGURED pass mark (not a hardcoded 65), and operate on the
        // credential linked to THIS attempt (falling back to the user's latest for pre-lifecycle data).
        app.MapPost("/api/admin/exam-sessions/{id}/review", (HttpContext ctx, long id) => gate(ctx.Request, "proctoring", adm =>
        {
            var a = db.QueryOne("SELECT * FROM exam_attempts WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var action = H.GetS(b, "action") ?? "";
            var note = H.GetS(b, "note") ?? ""; if (note.Length > 1000) note = note[..1000];
            // Configured pass mark for the attempt's CERTIFICATION (PCP-AI is simply cert 1).
            var attCertId = H.L(a.GetValueOrDefault("certification_id") ?? 1L);
            var passMark = Certs.Cfg(db, attCertId).Pass;
            var pct = H.D(a["percent"]);
            var holder = db.QueryOne("SELECT first_name,last_name FROM users WHERE id=?", a["user_id"]);
            var holderName = ($"{H.Str(holder?["first_name"])} {H.Str(holder?["last_name"])}").Trim();
            Dictionary<string, object?>? AttemptCred() =>
                db.QueryOne("SELECT * FROM issued_credentials WHERE attempt_id=?", id)
                ?? db.QueryOne("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", a["user_id"]);

            if (action == "release")
            {
                // Release a held result: only meaningful for auto_held attempts. The stored score stands
                // (from the immutable snapshot pipeline); we publish it and, on a pass, issue the credential.
                if (H.Str(a["result_status"]) != "auto_held")
                    return Results.Json(new { error = "not_held" }, statusCode: 400);
                var isPass = H.Str(a["result"]) == "pass";
                string? cred = null;
                if (isPass) cred = Lifecycle.IssueCredential(db, H.L(a["user_id"]!), a["id"], holderName, attCertId);
                var newStatus = isPass ? (cred is not null ? "credential_issued" : "released_pass") : "released_fail";
                db.Execute("UPDATE exam_attempts SET result_status=?, released_at=datetime('now'), review_status='cleared', review_note=?, reviewed_at=datetime('now') WHERE id=?", newStatus, note, id);
                log(adm.Id, "result_released", $"attempt {id} → {newStatus}" + (note.Length > 0 ? " — " + note : ""));
                if (cred is not null) log(adm.Id, "credential_issued", cred + $" (release of attempt {id})");
                return J(new { ok = true, result_status = newStatus, credential = cred });
            }
            if (action == "invalidate")
            {
                db.Execute("UPDATE exam_attempts SET result_status='invalidated', result='invalidated', review_status='invalidated', review_note=?, reviewed_at=datetime('now') WHERE id=?", note, id);
                var cred = AttemptCred();
                if (cred is not null && (cred["status"] as string) == "active")
                {
                    db.Execute("UPDATE issued_credentials SET status='revoked' WHERE id=?", cred["id"]);
                    db.Execute("UPDATE exam_attempts SET result_status='credential_revoked' WHERE id=?", id);
                    log(adm.Id, "credential_revoked", H.Str(cred["credential_id"]) + $" (invalidation of attempt {id})");
                }
                log(adm.Id, "exam_invalidated", "attempt " + id + (note.Length > 0 ? " — " + note : ""));
                return J(new { ok = true, result_status = cred is not null ? "credential_revoked" : "invalidated" });
            }
            if (action == "reinstate")
            {
                var isPass = pct >= passMark;
                string? cred = null;
                if (isPass)
                {
                    var existing = AttemptCred();
                    if (existing is not null && (existing["status"] as string) == "revoked")
                    { db.Execute("UPDATE issued_credentials SET status='active' WHERE id=?", existing["id"]); cred = H.Str(existing["credential_id"]); }
                    else cred = Lifecycle.IssueCredential(db, H.L(a["user_id"]!), a["id"], holderName, attCertId);
                }
                var newStatus = isPass ? (cred is not null ? "credential_issued" : "released_pass") : "released_fail";
                db.Execute("UPDATE exam_attempts SET result_status=?, result=?, released_at=COALESCE(released_at,datetime('now')), review_status='cleared', review_note=?, reviewed_at=datetime('now') WHERE id=?",
                    newStatus, isPass ? "pass" : "fail", note, id);
                log(adm.Id, "exam_reinstated", $"attempt {id} → {newStatus} (pass mark {passMark}%)");
                return J(new { ok = true, result_status = newStatus, credential = cred });
            }
            db.Execute("UPDATE exam_attempts SET review_status='cleared', review_note=?, reviewed_at=datetime('now') WHERE id=?", note, id);
            log(adm.Id, "exam_reviewed", "attempt " + id);
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/exam-sessions/launch-code", (HttpContext ctx) => gate(ctx.Request, "proctoring", _ =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var uid = (long)(H.GetNum(b, "user_id") ?? 0);
            if (uid == 0) return Results.Json(new { error = "user_id_required" }, statusCode: 400);
            var bk = db.QueryOne("SELECT * FROM exam_bookings WHERE user_id=? AND status='scheduled' ORDER BY id DESC", uid);
            if (bk is null) return Results.Json(new { error = "no_booking" }, statusCode: 400);
            var code = "PCI-" + Security.RandomHex(20).ToUpperInvariant();
            db.Execute("INSERT INTO exam_launch_codes(code_hash,user_id,booking_id,expires_at) VALUES(?,?,?,datetime('now','+15 minutes'))", Security.Sha(code), uid, bk["id"]);
            return J(new { code, uri = "pciexam://start?code=" + code, expires_in_seconds = 900 });
        }));
    }
}
