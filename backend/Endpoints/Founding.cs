using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Founding-Stage access — Route B of the three-route model (Standard / Founding / Honorary).
/// ONE founding route: during the window, a valid code grants free membership + study access + a
/// free exam entitlement together, layered on the NORMAL paid flow — never a parallel path. The
/// board may optionally gate a code behind an application (evidence + criteria, auto-approved by
/// default). Every grant settles through the same rows a real payment creates (payments +
/// memberships + exam_entitlements), so BookingBlockers, /api/me, invoices and the one-attempt rule
/// apply unchanged, and the credential is still EARNED by passing the real exam — no code path here
/// ever writes issued_credentials. (The separate honorary recognition lives in Honorary.cs and is
/// never a PCP-AI credential.)
/// </summary>
public static class Founding
{
    /// <summary>Windows/caps state of a founding code row. Never leaks internals to callers.
    /// A date-only end_date is INCLUSIVE of that whole day (same semantics as ordinary discount
    /// codes in Public.ValidateCode) — otherwise a window ending "2026-12-31" would close at
    /// midnight UTC and lose its final day.</summary>
    static (bool ok, string reason) Usable(Dictionary<string, object?> c)
    {
        if (H.L(c["active"]) != 1) return (false, "inactive");
        var now = H.IsoNow;
        var from = H.Str(c["start_date"]);
        var until = H.Str(c["end_date"]);
        if (!string.IsNullOrEmpty(from) && H.After(from, now)) return (false, "not_open");
        if (!string.IsNullOrEmpty(until))
        {
            var u = until!.Length == 10 ? until + " 23:59:59" : until;
            if (H.After(now, u)) return (false, "window_closed");
        }
        var max = c["max_uses"];
        if (max is not null && H.L(c["used_count"]) >= H.L(max)) return (false, "fully_redeemed");
        return (true, "ok");
    }

    /// <summary>
    /// Settle a founding grant exactly once per (code, user): a USD 0 'paid' payment row marked
    /// payment_provider='founding_waiver' plus membership and/or exam entitlement — the same rows
    /// the Stripe webhook writes, so everything downstream treats it uniformly. Idempotency uses the
    /// webhook_events ledger (unique event_id 'founding:{codeId}:u{userId}'), mirroring the webhook.
    /// </summary>
    public static (bool granted, string reason, string? reference) Grant(Db db, Action<long?, string, string?> log,
        long userId, Dictionary<string, object?> code, string grantedVia)
    {
        var codeId = H.L(code["id"]);
        (bool granted, string reason, string? reference) result = (false, "failed", null);
        // The WHOLE grant is one transaction: the idempotency ledger row, the cap take and the
        // settlement rows commit together or not at all. Without this, a failure between the ledger
        // insert and the settlement would permanently lock the user out ("already_redeemed" with
        // nothing granted) and burn a cap slot.
        db.Transaction(() =>
        {
            // ── exactly-once gate (runs before any side effect) ──
            var (_, ledger) = db.ExecuteWithChanges("INSERT OR IGNORE INTO webhook_events(provider,event_id,status) VALUES('founding',?, 'processed')",
                $"founding:{codeId}:u{userId}");
            if (ledger == 0) { result = (false, "already_redeemed", null); return; }

            // ── total-uses cap taken atomically; on failure undo the ledger row and stop ──
            var used = db.Execute("UPDATE discount_codes SET used_count=used_count+1 WHERE id=? AND (max_uses IS NULL OR used_count<max_uses)", codeId);
            if (used == 0)
            {
                db.Execute("DELETE FROM webhook_events WHERE provider='founding' AND event_id=?", $"founding:{codeId}:u{userId}");
                result = (false, "fully_redeemed", null);
                return;
            }

            bool grantsMembership = H.L(code.GetValueOrDefault("grants_membership")) == 1;
            bool grantsExam = H.L(code.GetValueOrDefault("grants_exam")) == 1;
            var product = grantsExam && grantsMembership ? "bundle" : grantsExam ? "exam" : "membership";
            var reference = "FOUND-" + Security.RandomHex(5).ToUpperInvariant();

            var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,discount_code,final_amount,currency,payment_provider,payment_status,payment_date,reference)
                VALUES(?,?,0,?,0,'USD','founding_waiver','paid',datetime('now'),?)", userId, product, H.Str(code["code"]), reference);
            db.Execute(@"INSERT OR IGNORE INTO code_redemptions(code_id,code,user_id,email,payment_id,product_type,amount_before,discount_amount)
                VALUES(?,?,?,(SELECT email FROM users WHERE id=?),?,?,0,0)", codeId, H.Str(code["code"]), userId, userId, payId, product);

            if (grantsMembership)
            {
                // expiry computed in C# (provider-agnostic), term set by the board per code
                var months = (int)Math.Max(1, H.L(code.GetValueOrDefault("membership_months") is null or "" ? 12L : code["membership_months"]));
                var expiry = DateTime.UtcNow.AddMonths(months).ToString("yyyy-MM-dd HH:mm:ss");
                var existing = db.QueryOne("SELECT id,expiry_date,status FROM memberships WHERE user_id=? ORDER BY id DESC", userId);
                if (existing is null)
                    db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,renewal_fee,renewal_cycle,amount_paid,currency) VALUES(?, 'Founding Membership','active',datetime('now'),?,99,'3 years',0,'USD')", userId, expiry);
                else
                {
                    // Never shorten an existing membership; reactivate and extend to at least the
                    // founding term. If the row was NOT live (expired/inactive), this founding grant
                    // is what makes it active — stamp it 'Founding Membership' so a later revoke can
                    // find and lapse it. A genuinely live paid membership keeps its own type (revoke
                    // must never claw back membership the person actually paid for).
                    var cur = H.Str(existing["expiry_date"]);
                    var wasLive = H.Str(existing["status"]) == "active" && !H.IsPast(cur);
                    var newExp = !string.IsNullOrEmpty(cur) && H.After(cur, expiry) ? cur : expiry;
                    if (wasLive)
                        db.Execute("UPDATE memberships SET status='active', expiry_date=? WHERE id=?", newExp, existing["id"]);
                    else
                        db.Execute("UPDATE memberships SET status='active', membership_type='Founding Membership', expiry_date=? WHERE id=?", newExp, existing["id"]);
                }
            }
            if (grantsExam)
            {
                db.Execute("UPDATE payments SET exam_schedule_deadline=datetime('now','+1 year') WHERE id=?", payId);
                db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) VALUES(?,?,?,?, 'available', datetime('now','+1 year'))",
                    userId, payId, product, Certs.DefaultId);
            }

            var what = grantsExam && grantsMembership ? "membership and exam access"
                : grantsExam ? "exam access" : "membership";
            db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Founding', 'Founding access granted', ?, 'View', ?)",
                userId, $"Your founding code {H.Str(code["code"])} has been applied — {what} granted at USD 0. Welcome to the founding cohort.",
                grantsExam ? "/certifications" : "/billing");
            result = (true, "granted", reference);
        });
        if (result.granted)
            log(userId, "founding_redeemed", $"code {H.Str(code["code"])} ({grantedVia}) ref {result.reference}");
        return result;
    }

    static bool CriteriaMet(Dictionary<string, object?> code, long expYears, string role, string qualification)
    {
        var raw = H.Str(code.GetValueOrDefault("criteria_json"));
        if (string.IsNullOrWhiteSpace(raw)) return true;   // board set no thresholds → nothing to enforce
        try
        {
            var c = JsonDocument.Parse(raw).RootElement;
            if (c.TryGetProperty("min_experience_years", out var minY) && minY.ValueKind == JsonValueKind.Number
                && expYears < minY.GetInt64()) return false;
            if (c.TryGetProperty("require_qualification", out var rq) && rq.ValueKind == JsonValueKind.True
                && string.IsNullOrWhiteSpace(qualification)) return false;
            if (c.TryGetProperty("allowed_roles", out var roles) && roles.ValueKind == JsonValueKind.Array)
            {
                var declared = role.Trim().ToLowerInvariant();
                var any = false; var listed = false;
                foreach (var r in roles.EnumerateArray())
                {
                    if (r.ValueKind != JsonValueKind.String) continue;
                    listed = true;
                    var want = (r.GetString() ?? "").Trim().ToLowerInvariant();
                    if (want.Length > 0 && (declared == want || declared.Contains(want))) { any = true; break; }
                }
                if (listed && !any) return false;
            }
            return true;
        }
        catch { return false; }   // malformed board JSON must fail CLOSED, never grant on a parse error
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);
        Dictionary<string, object?>? Code(string? code) => string.IsNullOrWhiteSpace(code) ? null
            : db.QueryOne("SELECT * FROM discount_codes WHERE code=? AND founding_route IS NOT NULL AND founding_route!=''", code.Trim().ToUpperInvariant());

        object PublicShape(Dictionary<string, object?> c, bool usable, string reason) => new
        {
            valid = usable,
            in_window = reason is not ("not_open" or "window_closed" or "inactive"),
            route = usable ? H.Str(c["founding_route"]) : null,
            grants = usable ? new { membership = H.L(c["grants_membership"]) == 1, exam = H.L(c["grants_exam"]) == 1, study = H.L(c["grants_study_access"]) == 1 } : null,
            requires_application = usable && H.L(c["requires_application"]) == 1,
            window_ends = usable ? H.Str(c["end_date"]) : null,
        };

        // ── PUBLIC: is a founding window open at all? Drives the website's Founding Programme
        // section (shown only while open, with the honest end date). No code details are leaked.
        app.MapGet("/api/founding/status", () =>
        {
            var open = db.Query("SELECT * FROM discount_codes WHERE founding_route IS NOT NULL AND founding_route!='' AND active=1")
                .Where(c => Usable(c).ok).ToList();
            if (open.Count == 0) return J(new { open = false });
            // the latest end date among open codes (null = open-ended window)
            var ends = open.Select(c => H.Str(c["end_date"])).ToList();
            var windowEnds = ends.Any(string.IsNullOrEmpty) ? null : ends.OrderByDescending(H.JsMillis).FirstOrDefault();
            return J(new { open = true, window_ends = windowEnds, requires_application = open.All(c => H.L(c["requires_application"]) == 1) });
        });

        // ── PUBLIC: validate a code (never reveals why an invalid code failed) ──
        app.MapPost("/api/founding/validate", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            var c = Code(H.GetS(b, "code"));
            if (c is null) return J(new { valid = false, in_window = false });
            var (ok, reason) = Usable(c);
            return J(PublicShape(c, ok, reason));
        });

        // ── STUDENT: redeem (Route 1 — no application required) ──
        app.MapPost("/api/founding/redeem", async (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var c = Code(H.GetS(b, "code"));
            if (c is null) return Results.Json(new { error = "invalid_code" }, statusCode: 400);
            var (ok, why) = Usable(c);
            // sold-out is reported distinctly (the caller typed a REAL code); window/inactive stay generic
            if (!ok) return Results.Json(new { error = why == "fully_redeemed" ? "fully_redeemed" : "invalid_code" }, statusCode: 400);
            if (H.L(c["requires_application"]) == 1) return J(new { needs_application = true, route = H.Str(c["founding_route"]) });
            var (granted, reason, reference) = Grant(db, log, u.Id, c, "redeem");
            if (!granted) return Results.Json(new { error = reason }, statusCode: reason == "already_redeemed" ? 409 : 400);
            return J(new { ok = true, reference, granted = new { membership = H.L(c["grants_membership"]) == 1, exam = H.L(c["grants_exam"]) == 1, study = H.L(c["grants_study_access"]) == 1 } });
        });

        // ── STUDENT: founding application (Route 2 — evidence REQUIRED) ──
        app.MapPost("/api/me/founding-application", async (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var c = Code(H.GetS(b, "code"));
            if (c is null) return Results.Json(new { error = "invalid_code" }, statusCode: 400);
            var (ok, _) = Usable(c);
            if (!ok) return Results.Json(new { error = "invalid_code" }, statusCode: 400);
            if (H.L(c["requires_application"]) != 1) return Results.Json(new { error = "no_application_needed" }, statusCode: 400);
            // one live application per user per code (a rejected one may be re-submitted)
            var prior = db.QueryOne("SELECT id,status FROM founding_applications WHERE user_id=? AND code_id=? AND status IN ('auto_approved','pending_review','approved') ORDER BY id DESC", u.Id, c["id"]);
            if (prior is not null) return Results.Json(new { error = "application_exists", status = H.Str(prior["status"]) }, statusCode: 409);

            var expYears = (long)Math.Max(0, H.GetNum(b, "experience_years", "declared_experience_years") ?? 0);
            var role = (H.GetS(b, "role", "declared_role") ?? "").Trim(); if (role.Length > 200) role = role[..200];
            var qual = (H.GetS(b, "qualification", "declared_qualification") ?? "").Trim(); if (qual.Length > 200) qual = qual[..200];
            // evidence is mandatory; validated + stored via the same Storage pipeline as every upload
            var (bytes, mime, err) = Storage.DecodeDataUri(H.GetS(b, "evidence", "data_uri", "dataUri"));
            if (err is not null) return Results.Json(new { error = err == "no_file" ? "evidence_required" : err }, statusCode: 400);
            var name = (H.GetS(b, "evidence_name", "filename") ?? "evidence").Trim();
            if (name.Length > 120) name = name[..120];
            name = string.Concat(name.Where(ch => !"\\/:*?\"<>|".Contains(ch)));
            var obj = Storage.Put(bytes!, mime, "founding");

            var auto = H.L(c.GetValueOrDefault("auto_approve") is null or "" ? 1L : c["auto_approve"]) == 1;
            var met = CriteriaMet(c, expYears, role, qual);
            var status = auto && met ? "auto_approved" : "pending_review";
            var appId = db.ExecuteReturningId(@"INSERT INTO founding_applications(user_id,code_id,route,declared_experience_years,declared_role,declared_qualification,evidence_ref,evidence_name,evidence_mime,evidence_size,status)
                VALUES(?,?,?,?,?,?,?,?,?,?,?)", u.Id, c["id"], H.Str(c["founding_route"]), expYears, role, qual, obj.Reference, name, obj.Mime, obj.SizeBytes, status);
            log(u.Id, "founding_application", $"code {H.Str(c["code"])} → {status}");

            if (status == "auto_approved")
            {
                var (granted, reason, reference) = Grant(db, log, u.Id, c, "application_auto");
                if (!granted)
                {
                    // e.g. the user already redeemed this code — keep the application consistent
                    db.Execute("UPDATE founding_applications SET status='rejected', admin_note=?, decided_at=datetime('now') WHERE id=?", "duplicate: " + reason, appId);
                    return Results.Json(new { error = reason }, statusCode: reason == "already_redeemed" ? 409 : 400);
                }
                return J(new { ok = true, status, reference, granted = new { membership = H.L(c["grants_membership"]) == 1, exam = H.L(c["grants_exam"]) == 1, study = H.L(c["grants_study_access"]) == 1 } });
            }
            return J(new { ok = true, status, message = "Your founding application has been received and is under review. We will notify you of the outcome." });
        });

        app.MapGet("/api/me/founding-application", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            // evidence_name/size are included so the applicant can see what they submitted (§6.2);
            // the file itself stays server-side (evidence_ref is never exposed to the client).
            var rows = db.Query(@"SELECT fa.id,fa.status,fa.route,fa.declared_experience_years,fa.declared_role,fa.declared_qualification,fa.evidence_name,fa.evidence_size,fa.admin_note,fa.created_at,fa.decided_at,dc.code
                FROM founding_applications fa JOIN discount_codes dc ON dc.id=fa.code_id WHERE fa.user_id=? ORDER BY fa.id DESC", u.Id);
            return J(new { rows, latest = rows.FirstOrDefault() });
        });

        // ── ADMIN: review queue (members section, like other student casework) ──
        app.MapGet("/api/admin/founding-applications", (HttpContext ctx) => gate(ctx.Request, "members", _ =>
        {
            var status = ctx.Request.Query["status"].ToString();
            var where = string.IsNullOrEmpty(status) ? "" : "WHERE fa.status=?";
            var args = string.IsNullOrEmpty(status) ? Array.Empty<object?>() : new object?[] { status };
            var rows = db.Query($@"SELECT fa.*, u.email, u.first_name, u.last_name, dc.code,
                CASE WHEN fa.evidence_ref IS NULL OR fa.evidence_ref='' THEN 0 ELSE 1 END has_evidence
                FROM founding_applications fa JOIN users u ON u.id=fa.user_id JOIN discount_codes dc ON dc.id=fa.code_id
                {where} ORDER BY fa.id DESC LIMIT 500", args);
            return J(new { rows });
        }));

        app.MapGet("/api/admin/founding-applications/{id}/evidence", (HttpContext ctx, long id) => gate(ctx.Request, "members", _ =>
        {
            var a = db.QueryOne("SELECT evidence_ref FROM founding_applications WHERE id=?", id);
            var got = a is null ? null : Storage.Get(H.Str(a["evidence_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return Results.File(got.Value.bytes!, got.Value.mime);
        }));

        app.MapPost("/api/admin/founding-applications/{id}/decide", (HttpContext ctx, long id) => gate(ctx.Request, "members", adm =>
        {
            var a = db.QueryOne("SELECT * FROM founding_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var status = H.GetS(b, "status");
            if (status is not ("approved" or "rejected" or "revoked")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var note = (H.GetS(b, "admin_note", "note") ?? "").Trim(); if (note.Length > 500) note = note[..500];
            var uid = H.L(a["user_id"]);
            var code = db.QueryOne("SELECT * FROM discount_codes WHERE id=?", a["code_id"]);

            if (status == "approved")
            {
                if (H.Str(a["status"]) is "approved" or "auto_approved") return Results.Json(new { error = "already_approved" }, statusCode: 409);
                if (code is null) return Results.Json(new { error = "code_missing" }, statusCode: 400);
                var (granted, reason, reference) = Grant(db, log, uid, code, $"manual_approve_admin{adm.Id}");
                if (!granted && reason != "already_redeemed") return Results.Json(new { error = reason }, statusCode: 400);
                db.Execute("UPDATE founding_applications SET status='approved', decided_by=?, decided_at=datetime('now'), admin_note=? WHERE id=?", adm.Id, note, id);
                log(uid, "founding_app_approved", $"app {id} by admin {adm.Id}");
                return J(new { ok = true, status = "approved", reference });
            }
            if (status == "rejected")
            {
                db.Execute("UPDATE founding_applications SET status='rejected', decided_by=?, decided_at=datetime('now'), admin_note=? WHERE id=?", adm.Id, note, id);
                db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Founding', 'Founding application update', ?)",
                    uid, $"Your founding application was not approved{(note.Length > 0 ? ": " + note : "")}. The standard enrolment route remains open to you.");
                log(uid, "founding_app_rejected", $"app {id} by admin {adm.Id}");
                return J(new { ok = true, status = "rejected" });
            }
            // revoke: kill the UNUSED grant only. An earned credential is NEVER touched — if the
            // entitlement was already consumed by a passed exam, the credential stands (spec §3).
            var pay = code is null ? null : db.QueryOne(@"SELECT id FROM payments WHERE user_id=? AND discount_code=? AND payment_provider='founding_waiver' ORDER BY id DESC", uid, H.Str(code["code"]));
            var revokedEnt = 0;
            if (pay is not null)
            {
                revokedEnt = db.Execute("UPDATE exam_entitlements SET status='revoked' WHERE payment_id=? AND status IN ('available','booked')", pay["id"]);
                if (revokedEnt > 0)
                    db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE payment_id=? AND status='scheduled'", pay["id"]);
                // mark the waiver payment reversed only when nothing was consumed, so a completed
                // sitting keeps its full paper trail intact
                if (db.QueryOne("SELECT id FROM exam_entitlements WHERE payment_id=? AND status='consumed'", pay["id"]) is null)
                    db.Execute("UPDATE payments SET payment_status='reversed' WHERE id=?", pay["id"]);
            }
            // founding-granted membership lapses per policy (only if it was founding-granted)
            db.Execute("UPDATE memberships SET status='expired', expiry_date=datetime('now') WHERE user_id=? AND membership_type='Founding Membership'", uid);
            db.Execute("UPDATE founding_applications SET status='revoked', decided_by=?, decided_at=datetime('now'), admin_note=? WHERE id=?", adm.Id, note, id);
            db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Founding', 'Founding access revoked', ?)",
                uid, $"Your founding-stage access has been revoked{(note.Length > 0 ? ": " + note : "")}. Any credential you have already earned is unaffected.");
            log(uid, "founding_app_revoked", $"app {id} by admin {adm.Id} (entitlements revoked: {revokedEnt})");
            return J(new { ok = true, status = "revoked", entitlements_revoked = revokedEnt });
        }));

        // ── ADMIN: per-code founding stats for the console dashboard ──
        app.MapGet("/api/admin/founding-stats", (HttpContext ctx) => gate(ctx.Request, "codes", _ =>
        {
            var rows = db.Query("SELECT * FROM discount_codes WHERE founding_route IS NOT NULL AND founding_route!='' ORDER BY id DESC")
                .Select(c =>
                {
                    var (ok, reason) = Usable(c);
                    var apps = db.Query("SELECT status, COUNT(*) n FROM founding_applications WHERE code_id=? GROUP BY status", c["id"])
                        .ToDictionary(r => H.Str(r["status"]) ?? "?", r => H.L(r["n"]));
                    double? daysLeft = null;
                    var end = H.Str(c["end_date"]);
                    if (!string.IsNullOrEmpty(end))
                        daysLeft = Math.Round((H.JsMillis(end) - DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) / 86400_000.0, 1);
                    return new
                    {
                        id = c["id"], code = c["code"], route = c["founding_route"],
                        grants = new { membership = H.L(c["grants_membership"]) == 1, exam = H.L(c["grants_exam"]) == 1, study = H.L(c["grants_study_access"]) == 1 },
                        auto_approve = H.L(c.GetValueOrDefault("auto_approve") is null or "" ? 1L : c["auto_approve"]) == 1,
                        window = new { from = c["start_date"], until = c["end_date"], days_left = daysLeft, open = ok || reason == "fully_redeemed" ? reason != "window_closed" && reason != "not_open" : false },
                        uses = new { redeemed = H.L(c["used_count"]), cap = c["max_uses"], remaining = c["max_uses"] is null ? null : (object)(H.L(c["max_uses"]) - H.L(c["used_count"])) },
                        applications = apps,
                        usable = ok, state = reason,
                    };
                }).ToList();
            return J(new { rows });
        }));
    }
}
