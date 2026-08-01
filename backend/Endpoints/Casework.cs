using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Phase-2 casework: appeals & complaints, accommodation requests (with a real effect on exam
/// duration), support-ticket attachments, CPD evidence + admin review, and the certificate data
/// endpoint. Student routes require a session; admin routes are RBAC-gated ("tickets" for
/// support casework, "members" for CPD review).
/// </summary>
public static class Casework
{
    // Small-file uploads are stored as size-capped data URIs (the same pattern as exam_evidence).
    // ~2M chars ≈ 1.5 MB decoded. Only document/image types are accepted.
    // Validate an uploaded data URI and persist the bytes via the Storage abstraction. Returns the stored
    // reference (provider:path) that the DB records — NOT the raw bytes — plus a sanitised filename.
    // On any validation failure, returns the error code and a null reference.
    static (string? reference, string? mime, long size, string? sha, string cleanName, string? error) StoreUpload(string? name, string? dataUri, string category)
    {
        var cleanName = (name ?? "attachment").Trim();
        if (cleanName.Length > 120) cleanName = cleanName[..120];
        // Drop control characters too: CR/LF in a stored filename makes ASP.NET's Content-Disposition
        // validation throw on every later download of the file (a stored denial of the review path).
        cleanName = string.Concat(cleanName.Where(c => c >= 0x20 && !"\\/:*?\"<>|".Contains(c)));
        if (string.IsNullOrWhiteSpace(cleanName)) cleanName = "attachment";
        var (bytes, mime, err) = Storage.DecodeDataUri(dataUri);
        if (err is not null) return (null, null, 0, null, cleanName, err);
        var obj = Storage.Put(bytes!, mime, category);
        return (obj.Reference, obj.Mime, obj.SizeBytes, obj.Sha256, cleanName, null);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);

        // Serve an evidence value that is either a Storage reference (current) or a legacy inline
        // data URI (early rows). Shared by the admin review routes and the student view-back routes
        // so both sides of a case see exactly the same bytes. `inline` controls Content-Disposition
        // so the in-app viewer can render without forcing a save dialog.
        IResult ServeEvidence(string? reference, string? filename, bool inline)
        {
            if (string.IsNullOrEmpty(reference)) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (reference.StartsWith("local:") || reference.StartsWith("s3:"))
            {
                var got = Storage.Get(reference);
                if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                if (inline)
                    return Results.File(got.Value.bytes, got.Value.mime); // no filename → inline display
                return Results.Bytes(got.Value.bytes, got.Value.mime, filename);
            }
            return Results.Content(reference, "text/plain"); // legacy inline data URI
        }
        static bool Inline(HttpRequest r) => r.Query["inline"].ToString() == "1";

        // ─────────────────────────── APPEALS (student) ───────────────────────────
        app.MapPost("/api/me/appeals", async (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var type = H.GetS(b, "type") ?? "";
            if (type is not ("result_appeal" or "invalidation_appeal" or "complaint" or "ethics"))
                return Results.Json(new { error = "bad_type" }, statusCode: 400);
            var reason = (H.GetS(b, "reason") ?? "").Trim();
            if (reason.Length < 20) return Results.Json(new { error = "reason_too_short", message = "Please describe the grounds for your appeal (at least 20 characters)." }, statusCode: 400);
            if (reason.Length > 5000) reason = reason[..5000];

            // Optional attempt/credential references must belong to the caller.
            long? attemptId = null;
            var attEl = H.GetEl(b, "attempt_id");
            if (attEl is { ValueKind: JsonValueKind.Number })
            {
                attemptId = attEl.Value.GetInt64();
                var owns = db.QueryOne("SELECT id FROM exam_attempts WHERE id=? AND user_id=?", attemptId, u.Id);
                if (owns is null) return Results.Json(new { error = "attempt_not_found" }, statusCode: 404);
            }
            string? credentialId = H.GetS(b, "credential_id");
            if (credentialId is not null)
            {
                var owns = db.QueryOne("SELECT id FROM issued_credentials WHERE credential_id=? AND user_id=?", credentialId, u.Id);
                if (owns is null) return Results.Json(new { error = "credential_not_found" }, statusCode: 404);
            }

            // Storage I/O stays OUTSIDE the transaction so S3/disk writes never run under the DB lock.
            string? evName = null, evRef = null;
            var dataUri = H.GetS(b, "evidence_data");
            if (!string.IsNullOrEmpty(dataUri))
            {
                var (reference, _, _, _, clean, err) = StoreUpload(H.GetS(b, "evidence_name"), dataUri, "appeal");
                if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
                evName = clean; evRef = reference; // DB stores the storage reference, not the bytes
            }
            // One open appeal per attempt keeps the queue coherent — checked and inserted atomically so
            // two concurrent submissions can't both pass the check.
            long id = 0; object? dupId = null;
            db.Transaction(() =>
            {
                if (attemptId is not null)
                {
                    var open = db.QueryOne("SELECT id FROM appeals WHERE user_id=? AND attempt_id=? AND status IN ('submitted','under_review')", u.Id, attemptId);
                    if (open is not null) { dupId = open["id"]; return; }
                }
                id = db.ExecuteReturningId("INSERT INTO appeals(user_id,attempt_id,credential_id,type,reason,evidence_name,evidence_data) VALUES(?,?,?,?,?,?,?)",
                    u.Id, attemptId, credentialId, type, reason, evName, evRef);
            });
            if (dupId is not null) return Results.Json(new { error = "appeal_already_open", id = dupId }, statusCode: 400);
            log(u.Id, "appeal_submitted", $"{type} #{id}" + (attemptId is not null ? $" attempt {attemptId}" : ""));
            return J(new { ok = true, id, status = "submitted", message = "Your appeal has been submitted. PCI will review it and respond through your portal." });
        });

        app.MapGet("/api/me/appeals", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT id,attempt_id,credential_id,type,reason,evidence_name,status,submitted_at,decision,decided_at FROM appeals WHERE user_id=? ORDER BY id DESC", u.Id) });
        });

        // Student: view/download back the evidence attached to their OWN appeal (ownership enforced).
        app.MapGet("/api/me/appeals/{id}/evidence", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var r = db.QueryOne("SELECT evidence_name,evidence_data FROM appeals WHERE id=? AND user_id=?", id, u.Id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        });

        // ─────────────────────────── ACCOMMODATIONS (student) ───────────────────────────
        app.MapPost("/api/me/accommodations", async (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var type = H.GetS(b, "request_type") ?? "";
            if (type is not ("extra_time" or "separate_setting" or "assistive_technology" or "other"))
                return Results.Json(new { error = "bad_type" }, statusCode: 400);
            var desc = (H.GetS(b, "description") ?? "").Trim();
            if (desc.Length < 20) return Results.Json(new { error = "description_too_short", message = "Please describe the accommodation you need (at least 20 characters)." }, statusCode: 400);
            if (desc.Length > 5000) desc = desc[..5000];
            // Storage I/O stays OUTSIDE the transaction so S3/disk writes never run under the DB lock.
            string? evName = null, evRef = null;
            var dataUri = H.GetS(b, "evidence_data");
            if (!string.IsNullOrEmpty(dataUri))
            {
                var (reference, _, _, _, clean, err) = StoreUpload(H.GetS(b, "evidence_name"), dataUri, "accommodation");
                if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
                evName = clean; evRef = reference;
            }
            // One open request per user — checked and inserted atomically to close the concurrent-submit race.
            long id = 0; object? dupId = null;
            db.Transaction(() =>
            {
                var open = db.QueryOne("SELECT id FROM accommodation_requests WHERE user_id=? AND status IN ('submitted','under_review')", u.Id);
                if (open is not null) { dupId = open["id"]; return; }
                id = db.ExecuteReturningId("INSERT INTO accommodation_requests(user_id,request_type,description,evidence_name,evidence_data) VALUES(?,?,?,?,?)",
                    u.Id, type, desc, evName, evRef);
            });
            if (dupId is not null) return Results.Json(new { error = "request_already_open", id = dupId }, statusCode: 400);
            log(u.Id, "accommodation_requested", $"{type} #{id}");
            return J(new { ok = true, id, status = "submitted", message = "Your accommodation request has been submitted for review. Please allow time before booking your exam." });
        });

        app.MapGet("/api/me/accommodations", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            return J(new { rows = db.Query("SELECT id,request_type,description,evidence_name,status,approved_extra_minutes,admin_note,created_at,decided_at FROM accommodation_requests WHERE user_id=? ORDER BY id DESC", u.Id) });
        });

        // Student: view/download back the evidence attached to their OWN accommodation request.
        app.MapGet("/api/me/accommodations/{id}/evidence", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var r = db.QueryOne("SELECT evidence_name,evidence_data FROM accommodation_requests WHERE id=? AND user_id=?", id, u.Id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        });

        // ─────────────────────────── SUPPORT ATTACHMENTS ───────────────────────────
        app.MapPost("/api/me/tickets/{id}/attachments", async (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var t = db.QueryOne("SELECT id FROM tickets WHERE id=? AND user_id=?", id, u.Id);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            // Storage I/O first, outside the transaction; the cap check + INSERT run atomically so two
            // concurrent uploads can't both pass the 10-attachment cap.
            var (reference, mime, size, sha, clean, err) = StoreUpload(H.GetS(b, "filename"), H.GetS(b, "data_uri"), "support");
            if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
            long aid = 0; var overCap = false;
            db.Transaction(() =>
            {
                var count = db.Scalar<long>("SELECT COUNT(*) FROM support_attachments WHERE ticket_id=?", id);
                if (count >= 10) { overCap = true; return; }
                aid = db.ExecuteReturningId("INSERT INTO support_attachments(ticket_id,user_id,filename,mime,size_bytes,storage_ref,sha256) VALUES(?,?,?,?,?,?,?)",
                    id, u.Id, clean, mime, size, reference, sha);
                db.Execute("UPDATE tickets SET updated_at=datetime('now') WHERE id=?", id);
            });
            if (overCap) return Results.Json(new { error = "too_many_attachments" }, statusCode: 400);
            log(u.Id, "ticket_attachment", $"ticket {id} file '{clean}'");
            return J(new { ok = true, id = aid, filename = clean });
        });

        app.MapGet("/api/me/tickets/{id}/attachments", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var t = db.QueryOne("SELECT id FROM tickets WHERE id=? AND user_id=?", id, u.Id);
            if (t is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new { rows = db.Query("SELECT id,filename,mime,size_bytes,user_id,created_at, COALESCE(storage_ref, CASE WHEN data_uri IS NOT NULL THEN 'legacy' END) AS ref FROM support_attachments WHERE ticket_id=? ORDER BY id", id) });
        });

        // Stream one of the caller's own support attachments (ownership enforced via the parent ticket).
        app.MapGet("/api/me/tickets/{tid}/attachments/{aid}", (HttpContext ctx, long tid, long aid) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var a = db.QueryOne(@"SELECT sa.mime,sa.storage_ref,sa.data_uri,sa.filename FROM support_attachments sa
                JOIN tickets t ON t.id=sa.ticket_id WHERE sa.id=? AND sa.ticket_id=? AND t.user_id=?", aid, tid, u.Id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var reference = H.Str(a["storage_ref"]);
            if (!string.IsNullOrEmpty(reference))
            {
                var got = Storage.Get(reference);
                if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                return Results.Bytes(got.Value.bytes, got.Value.mime, H.Str(a["filename"]));
            }
            var legacy = H.Str(a["data_uri"]);
            if (!string.IsNullOrEmpty(legacy)) return Results.Content(legacy, "text/plain");
            return Results.Json(new { error = "not_found" }, statusCode: 404);
        });

        // Staff (support inbox): list a ticket's attachments. Students could already upload and
        // re-download their own files, but the inbox had no way to see them — this closes that gap.
        app.MapGet("/api/support/tickets/{id}/attachments", (HttpRequest req, long id) => gate(req, "inbox", _ =>
        {
            if (db.QueryOne("SELECT id FROM tickets WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new { rows = db.Query("SELECT id,filename,mime,size_bytes,user_id,created_at FROM support_attachments WHERE ticket_id=? ORDER BY id", id) });
        }));

        // Staff (support inbox): stream one attachment. Every staff read of a student file is logged.
        app.MapGet("/api/support/tickets/{tid}/attachments/{aid}", (HttpContext ctx, long tid, long aid) => gate(ctx.Request, "inbox", adm =>
        {
            var a = db.QueryOne("SELECT mime,storage_ref,data_uri,filename FROM support_attachments WHERE id=? AND ticket_id=?", aid, tid);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "ticket_attachment_view", $"ticket {tid} attachment {aid} '{H.Str(a["filename"])}'");
            var reference = H.Str(a["storage_ref"]);
            if (!string.IsNullOrEmpty(reference)) return ServeEvidence(reference, H.Str(a["filename"]), Inline(ctx.Request));
            var legacy = H.Str(a["data_uri"]);
            if (!string.IsNullOrEmpty(legacy)) return Results.Content(legacy, "text/plain");
            return Results.Json(new { error = "not_found" }, statusCode: 404);
        }));

        // ─────────────────────────── CERTIFICATE DATA (student) ───────────────────────────
        // Returns the data the portal renders into the certificate document. A certificate is only
        // "active" while the credential is active AND unexpired — same rule as the public verify.
        app.MapGet("/api/me/certificate", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var requested = ctx.Request.Query["id"].ToString().Trim();
            var c = db.QueryOne(@"SELECT ic.credential_id,ic.holder_name,ic.credential,ic.status,ic.issued_at,ic.expires_at,ic.attempt_id,ic.certificate_wording,
                       COALESCE(ic.certification_id,1) certification_id,ct.code certification_code,
                       ct.name certification_name, ct.acronym certification_acronym
                FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                WHERE ic.user_id=? AND (?='' OR ic.credential_id=?) ORDER BY ic.id DESC", u.Id, requested, requested);
            if (c is null) return J(new { found = false });
            var status = H.Str(c["status"]) ?? "active";
            var expires = H.Str(c["expires_at"]);
            var lapsed = status == "active" && H.IsPast(expires);
            var state = status == "revoked" ? "revoked" : (lapsed || status == "expired") ? "expired" : "active";
            var regNo = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", u.Id);
            return J(new
            {
                found = true, state, valid = state == "active",
                credential_id = c["credential_id"], holder_name = c["holder_name"], credential = c["credential"],
                certification_id = c["certification_id"], certification_code = c["certification_code"],
                certification_name = c["certification_name"], certification_acronym = c["certification_acronym"],
                certificate_wording = c["certificate_wording"],
                issued_at = c["issued_at"], expires_at = c["expires_at"], registration_no = regNo,
                verify_path = "/verify.html?id=" + H.Str(c["credential_id"])
            });
        });

        // ─────────────────────────── ADMIN: appeals & accommodations (gate: tickets) ───────────────────────────
        app.MapGet("/api/admin/appeals", (HttpRequest req) => gate(req, "tickets", _ =>
            J(new { rows = db.Query(@"SELECT ap.id,ap.user_id,ap.attempt_id,ap.credential_id,ap.type,ap.reason,ap.evidence_name,ap.status,ap.submitted_at,ap.decision,ap.decided_at,
                       u.email,u.first_name,u.last_name
                FROM appeals ap LEFT JOIN users u ON u.id=ap.user_id ORDER BY (ap.status IN ('submitted','under_review')) DESC, ap.id DESC") })));

        app.MapGet("/api/admin/appeals/{id}/evidence", (HttpContext ctx, long id) => gate(ctx.Request, "tickets", adm =>
        {
            var r = db.QueryOne("SELECT user_id,evidence_name,evidence_data FROM appeals WHERE id=?", id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "appeal_evidence_view", $"appeal {id} (user {H.Ln(r["user_id"])})");
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        }));

        app.MapPost("/api/admin/appeals/{id}/decide", (HttpContext ctx, long id) => gate(ctx.Request, "tickets", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status") ?? "";
            if (status is not ("under_review" or "upheld" or "dismissed"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var decision = H.GetS(b, "decision");
            long changes;
            if (status is "upheld" or "dismissed")
                (_, changes) = db.ExecuteWithChanges("UPDATE appeals SET status=?, decision=?, decided_by=?, decided_at=datetime('now') WHERE id=?", status, decision, adm.Id, id);
            else
                // Returning to under_review clears the stale decision — GET /api/me/appeals surfaces
                // decision/decided_at, and a candidate must not see a verdict for a reopened appeal.
                (_, changes) = db.ExecuteWithChanges("UPDATE appeals SET status=?, decision=NULL, decided_by=NULL, decided_at=NULL WHERE id=?", status, id);
            if (changes == 0) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "appeal_" + status, "appeal " + id + (string.IsNullOrEmpty(decision) ? "" : " — " + decision![..Math.Min(decision.Length, 120)]));
            return J(new { ok = true });
        }));

        app.MapGet("/api/admin/accommodations", (HttpRequest req) => gate(req, "tickets", _ =>
            J(new { rows = db.Query(@"SELECT ar.id,ar.user_id,ar.request_type,ar.description,ar.evidence_name,ar.status,ar.approved_extra_minutes,ar.admin_note,ar.created_at,ar.decided_at,
                       u.email,u.first_name,u.last_name
                FROM accommodation_requests ar LEFT JOIN users u ON u.id=ar.user_id ORDER BY (ar.status IN ('submitted','under_review')) DESC, ar.id DESC") })));

        app.MapGet("/api/admin/accommodations/{id}/evidence", (HttpContext ctx, long id) => gate(ctx.Request, "tickets", adm =>
        {
            var r = db.QueryOne("SELECT user_id,evidence_name,evidence_data FROM accommodation_requests WHERE id=?", id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "accommodation_evidence_view", $"request {id} (user {H.Ln(r["user_id"])})");
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        }));

        app.MapPost("/api/admin/accommodations/{id}/decide", (HttpContext ctx, long id) => gate(ctx.Request, "tickets", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status") ?? "";
            if (status is not ("under_review" or "approved" or "rejected"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var extra = (int)Math.Clamp(H.GetNum(b, "approved_extra_minutes") ?? 0, 0, 120);
            var note = H.GetS(b, "admin_note");
            long changes;
            if (status == "approved")
                (_, changes) = db.ExecuteWithChanges("UPDATE accommodation_requests SET status='approved', approved_extra_minutes=?, admin_note=?, decided_by=?, decided_at=datetime('now') WHERE id=?", extra, note, adm.Id, id);
            else if (status == "rejected")
                (_, changes) = db.ExecuteWithChanges("UPDATE accommodation_requests SET status='rejected', approved_extra_minutes=0, admin_note=?, decided_by=?, decided_at=datetime('now') WHERE id=?", note, adm.Id, id);
            else // returning to under_review clears the stale decision fields, mirroring appeals
                (_, changes) = db.ExecuteWithChanges("UPDATE accommodation_requests SET status='under_review', approved_extra_minutes=NULL, admin_note=NULL, decided_by=NULL, decided_at=NULL WHERE id=?", id);
            if (changes == 0) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "accommodation_" + status, $"request {id}" + (status == "approved" ? $" (+{extra} min)" : ""));
            return J(new { ok = true });
        }));

        // ─────────────────────────── ADMIN: CPD review (gate: members) ───────────────────────────
        app.MapGet("/api/admin/cpd", (HttpRequest req) => gate(req, "members", _ =>
        {
            var status = req.Query["status"].ToString();
            var rows = string.IsNullOrEmpty(status)
                ? db.Query(@"SELECT c.id,c.user_id,c.activity_date,c.category,c.hours,c.description,c.evidence_name,c.status,c.admin_note,c.created_at,u.email,u.first_name,u.last_name
                    FROM cpd_entries c LEFT JOIN users u ON u.id=c.user_id ORDER BY (c.status='recorded') DESC, c.id DESC LIMIT 500")
                : db.Query(@"SELECT c.id,c.user_id,c.activity_date,c.category,c.hours,c.description,c.evidence_name,c.status,c.admin_note,c.created_at,u.email,u.first_name,u.last_name
                    FROM cpd_entries c LEFT JOIN users u ON u.id=c.user_id WHERE c.status=? ORDER BY c.id DESC LIMIT 500", status);
            return J(new { rows });
        }));

        app.MapGet("/api/admin/cpd/{id}/evidence", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var r = db.QueryOne("SELECT user_id,evidence_name,evidence_data FROM cpd_entries WHERE id=?", id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "cpd_evidence_view", $"entry {id} (user {H.Ln(r["user_id"])})");
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        }));

        app.MapPost("/api/admin/cpd/{id}/review", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status") ?? "";
            if (status is not ("approved" or "rejected" or "recorded"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var (_, changes) = db.ExecuteWithChanges("UPDATE cpd_entries SET status=?, admin_note=?, reviewed_by=?, reviewed_at=datetime('now') WHERE id=?", status, H.GetS(b, "admin_note"), adm.Id, id);
            if (changes == 0) return Results.Json(new { error = "not_found" }, statusCode: 404);
            log(adm.Id, "cpd_" + status, "entry " + id);
            return J(new { ok = true });
        }));

        // ─────────────────────────── STUDENT: attach evidence to a CPD entry ───────────────────────────
        app.MapPost("/api/me/cpd/{id}/evidence", async (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (u.Impersonated) return Results.Json(new { error = "impersonation_readonly", message = "This action is disabled in support view." }, statusCode: 403);
            var e = db.QueryOne("SELECT id,status FROM cpd_entries WHERE id=? AND user_id=?", id, u.Id);
            if (e is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var (reference, _, _, _, clean, err) = StoreUpload(H.GetS(b, "filename"), H.GetS(b, "data_uri"), "cpd");
            if (err is not null) return Results.Json(new { error = err }, statusCode: 400);
            // Attaching evidence returns an approved/rejected entry to the review queue. DB stores the ref.
            db.Execute("UPDATE cpd_entries SET evidence_name=?, evidence_data=?, status='recorded' WHERE id=?", clean, reference, id);
            log(u.Id, "cpd_evidence", $"entry {id} file '{clean}'");
            return J(new { ok = true, filename = clean });
        });

        // Student: view/download back the evidence attached to their OWN CPD entry.
        app.MapGet("/api/me/cpd/{id}/evidence", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var r = db.QueryOne("SELECT evidence_name,evidence_data FROM cpd_entries WHERE id=? AND user_id=?", id, u.Id);
            if (r is null || r["evidence_data"] is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return ServeEvidence(H.Str(r["evidence_data"]), H.Str(r["evidence_name"]), Inline(ctx.Request));
        });
    }
}
