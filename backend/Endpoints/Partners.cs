using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Partner / institution / sponsor dashboards. A training_partners row doubles as the portal account:
/// the partner authenticates with a bearer access token (stored hashed, shown once at generation) and
/// can sponsor candidates in bulk (creating the account, an approved sponsored application and a
/// sponsor-funded exam entitlement per certification), track their candidates' progress, and view a
/// commission ledger. Commissions are DERIVED from paid redemptions of discount codes attributed to
/// the partner (discount_codes.partner_id) at the partner's commission_pct — nothing hooks into the
/// payment path. Payouts are the only materialized rows; balance = accrued − paid out.
/// </summary>
public static class Partners
{
    const int MaxBatch = 200;

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        Dictionary<string, object?>? PartnerFromReq(HttpRequest req)
        {
            var h = req.Headers.Authorization.ToString();
            if (!h.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return null;
            var tok = h["Bearer ".Length..].Trim();
            if (tok.Length < 16) return null;
            return db.QueryOne("SELECT * FROM training_partners WHERE access_token_hash=?", Security.Sha(tok));
        }

        // Shared commission ledger builder (partner portal + admin see identical numbers).
        object CommissionLedger(Dictionary<string, object?> p)
        {
            var pid = H.L(p["id"]);
            var pct = H.D(p["commission_pct"]);
            var rows = db.Query(@"SELECT p.reference, p.product_type, p.final_amount, p.currency, p.payment_date, dc.code
                FROM code_redemptions r
                JOIN discount_codes dc ON dc.id=r.code_id
                JOIN payments p ON p.id=r.payment_id
                WHERE dc.partner_id=? AND p.payment_status='paid'
                ORDER BY p.payment_date DESC LIMIT 500", pid);
            var attributed = db.Scalar<double>(@"SELECT COALESCE(SUM(p.final_amount),0)
                FROM code_redemptions r
                JOIN discount_codes dc ON dc.id=r.code_id
                JOIN payments p ON p.id=r.payment_id
                WHERE dc.partner_id=? AND p.payment_status='paid'", pid);
            var accrued = Math.Round(attributed * pct / 100.0, 2);
            var paidOut = db.Scalar<double>("SELECT COALESCE(SUM(amount),0) FROM partner_payouts WHERE partner_id=?", pid);
            var payouts = db.Query("SELECT amount,currency,note,paid_at FROM partner_payouts WHERE partner_id=? ORDER BY id DESC", pid);
            return new
            {
                commission_pct = pct,
                attributed_revenue = attributed,
                accrued,
                paid_out = paidOut,
                balance = Math.Round(accrued - paidOut, 2),
                payments = rows,
                payouts,
            };
        }

        // Candidate progress for a partner: sponsorship rows enriched with the live application /
        // entitlement / attempt / credential state, always scoped per certification.
        object CandidateRows(long pid) => db.Query(@"SELECT s.id, s.candidate_email, s.candidate_name, s.route_key, s.created_at,
                c.acronym cert_acronym, c.code cert_code,
                a.status application_status,
                (SELECT COALESCE(e.status,'') FROM exam_entitlements e WHERE e.user_id=s.user_id AND e.certification_id=s.certification_id ORDER BY e.id DESC LIMIT 1) entitlement_status,
                (SELECT at.result FROM exam_attempts at WHERE at.user_id=s.user_id AND at.certification_id=s.certification_id AND at.result_status IN ('credential_issued','released_pass','released_fail') ORDER BY at.id DESC LIMIT 1) exam_result,
                (SELECT ic.credential_id FROM issued_credentials ic WHERE ic.user_id=s.user_id AND ic.certification_id=s.certification_id AND ic.status='active' ORDER BY ic.id DESC LIMIT 1) credential_id
            FROM partner_sponsorships s
            LEFT JOIN certifications c ON c.id=s.certification_id
            LEFT JOIN certification_applications a ON a.id=s.application_id
            WHERE s.partner_id=? ORDER BY s.id DESC LIMIT 1000", pid);

        // ── Partner portal: profile + headline stats ──
        app.MapGet("/api/partner/me", (HttpRequest req) =>
        {
            var p = PartnerFromReq(req);
            if (p is null) return Results.Json(new { error = "bad_token" }, statusCode: 401);
            var pid = H.L(p["id"]);
            var candidates = db.Scalar<long>("SELECT COUNT(*) FROM partner_sponsorships WHERE partner_id=?", pid);
            var certified = db.Scalar<long>(@"SELECT COUNT(*) FROM partner_sponsorships s
                WHERE s.partner_id=? AND EXISTS(SELECT 1 FROM issued_credentials ic WHERE ic.user_id=s.user_id AND ic.certification_id=s.certification_id AND ic.status='active')", pid);
            return J(new
            {
                partner = new
                {
                    id = p["id"], name = p["name"], tier = p["tier"], partner_type = p["partner_type"],
                    contact_name = p["contact_name"], contact_email = p["contact_email"],
                    country = p["country"], website = p["website"],
                    sponsor_enabled = H.B(p["sponsor_enabled"]), commission_pct = p["commission_pct"],
                },
                stats = new { candidates, certified },
                commissions = CommissionLedger(p),
            });
        });

        // ── Partner portal: sponsored candidates + progress ──
        app.MapGet("/api/partner/candidates", (HttpRequest req) =>
        {
            var p = PartnerFromReq(req);
            if (p is null) return Results.Json(new { error = "bad_token" }, statusCode: 401);
            return J(new { rows = CandidateRows(H.L(p["id"])) });
        });

        // ── Partner portal: bulk-sponsor candidates ──
        // For each {email, first_name, last_name, certification}: create the account if new (with a
        // set-password welcome email), record an approved sponsored application and grant a sponsor-
        // funded exam entitlement — all scoped to the requested certification. Per-row results; a
        // failure in one row never blocks the rest.
        app.MapPost("/api/partner/candidates", async (HttpRequest req) =>
        {
            var p = PartnerFromReq(req);
            if (p is null) return Results.Json(new { error = "bad_token" }, statusCode: 401);
            if (!H.B(p["sponsor_enabled"]))
                return Results.Json(new { error = "sponsorship_disabled", message = "Candidate sponsorship is not enabled for this partner account. Contact PCI to enable it." }, statusCode: 403);
            var pid = H.L(p["id"]);
            var body = await H.BodyEl(req);
            if (body.ValueKind != System.Text.Json.JsonValueKind.Object
                || !body.TryGetProperty("candidates", out var list)
                || list.ValueKind != System.Text.Json.JsonValueKind.Array)
                return Results.Json(new { error = "candidates_required" }, statusCode: 400);
            if (list.GetArrayLength() > MaxBatch)
                return Results.Json(new { error = "batch_too_large", message = $"At most {MaxBatch} candidates per request." }, statusCode: 400);

            var baseUrl = Mailer.BaseUrl(req);
            var results = new List<object>();
            int granted = 0;
            foreach (var el in list.EnumerateArray())
            {
                var row = H.ToMap(el);
                var email = (H.GetS(row, "email") ?? "").Trim().ToLowerInvariant();
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                { results.Add(new { email, status = "invalid_email" }); continue; }
                var certId = Certs.Resolve(db, H.GetS(row, "certification") ?? H.GetS(row, "certification_id"));
                var cert = Certs.ById(db, certId);
                if (cert is null) { results.Add(new { email, status = "bad_certification" }); continue; }
                var route = Routes.Get(db, certId, "sponsored");
                if (route is null || !H.B(route["enabled"]))
                { results.Add(new { email, status = "route_unavailable" }); continue; }

                var first = (H.GetS(row, "first_name", "first") ?? "").Trim();
                var last = (H.GetS(row, "last_name", "last") ?? "").Trim();

                // Find or create the candidate account. New accounts get the standard set-password
                // welcome email (best effort — in dev it prints to the console).
                var user = db.QueryOne("SELECT id,first_name,last_name FROM users WHERE email=?", email);
                long uid; bool created = false;
                if (user is null)
                {
                    uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?, 'student','active')", email, first, last);
                    db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
                    var token = Security.RandomHex(32);
                    db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", uid, Security.Sha(token));
                    try { Mailer.SendWelcome(db, uid, email, first, Mailer.SetupLink(baseUrl, token), baseUrl); } catch { }
                    created = true;
                }
                else uid = H.L(user["id"]);

                // One sponsorship per (partner, candidate, certification).
                if (db.QueryOne("SELECT id FROM partner_sponsorships WHERE partner_id=? AND user_id=? AND certification_id=?", pid, uid, certId) is not null)
                { results.Add(new { email, status = "already_sponsored" }); continue; }

                var name = ($"{first} {last}").Trim();
                if (name.Length == 0) name = H.Str(user?["first_name"]) is { Length: > 0 } uf ? ($"{uf} {H.Str(user?["last_name"])}").Trim() : email;

                // Approved sponsored application + sponsor-funded entitlement (skipped, not failed,
                // when the candidate already holds an open entitlement for this certification).
                var appId = db.ExecuteReturningId(@"INSERT INTO certification_applications(user_id,certification_id,route_key,status,workflow_stage,data_json)
                    VALUES(?,?, 'sponsored', 'approved', 'exam_access_granted', ?)",
                    uid, certId, System.Text.Json.JsonSerializer.Serialize(new { sponsored_by = H.Str(p["name"]), partner_id = pid }));
                db.Execute("UPDATE certification_applications SET application_no=? WHERE id=?", $"PCI-APP-{DateTime.UtcNow.Year}-{100000 + appId}", appId);
                var entRef = Applications.GrantExamEntitlement(db, uid, certId, "sponsored", "sponsored");
                db.Execute(@"INSERT INTO partner_sponsorships(partner_id,user_id,application_id,certification_id,route_key,candidate_email,candidate_name)
                    VALUES(?,?,?,?, 'sponsored', ?, ?)", pid, uid, appId, certId, email, name);
                try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Sponsorship', 'Exam access sponsored', ?, 'Schedule exam', '/certifications')",
                    uid, $"{H.Str(p["name"])} has sponsored your {H.Str(cert["acronym"]) ?? H.Str(cert["code"])} examination. Your exam access is open."); } catch { }
                if (entRef is not null) granted++;
                results.Add(new { email, status = entRef is not null ? "sponsored" : "sponsored_already_entitled", account = created ? "created" : "existing", certification = cert["code"] });
            }
            log(null, "partner_bulk_sponsor", $"partner {pid} sponsored {granted}/{list.GetArrayLength()} candidates");
            try { Notify.Alert(db, "partner", $"{H.Str(p["name"])} sponsored candidates", $"<p><strong>{H.Str(p["name"])}</strong> registered {granted} sponsored candidate(s) through the partner portal.</p>", "partner", pid, null); } catch { }
            return J(new { ok = true, results });
        });

        // ── Partner portal: commission ledger ──
        app.MapGet("/api/partner/commissions", (HttpRequest req) =>
        {
            var p = PartnerFromReq(req);
            if (p is null) return Results.Json(new { error = "bad_token" }, statusCode: 401);
            return J(CommissionLedger(p));
        });

        // ── Admin: issue / revoke a partner's portal token ──
        app.MapPost("/api/admin/training-partners/{id}/token", (HttpRequest req, long id) => gate(req, "partners", adm =>
        {
            if (db.QueryOne("SELECT id FROM training_partners WHERE id=?", id) is null)
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            var token = "pp_" + Security.RandomHex(24);
            db.Execute("UPDATE training_partners SET access_token_hash=?, updated_at=datetime('now') WHERE id=?", Security.Sha(token), id);
            log(adm.Id, "partner_token_issued", id.ToString());
            // Plaintext returned exactly once; only the hash is stored.
            return J(new { ok = true, token });
        }));

        app.MapPost("/api/admin/training-partners/{id}/token/revoke", (HttpRequest req, long id) => gate(req, "partners", adm =>
        {
            db.Execute("UPDATE training_partners SET access_token_hash=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "partner_token_revoked", id.ToString());
            return J(new { ok = true });
        }));

        // ── Admin: the same ledgers the partner sees, plus payouts ──
        app.MapGet("/api/admin/training-partners/{id}/commissions", (HttpRequest req, long id) => gate(req, "partners", _ =>
        {
            var p = db.QueryOne("SELECT * FROM training_partners WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(CommissionLedger(p));
        }));

        app.MapGet("/api/admin/training-partners/{id}/candidates", (HttpRequest req, long id) => gate(req, "partners", _ =>
            J(new { rows = CandidateRows(id) })));

        app.MapPost("/api/admin/training-partners/{id}/payouts", (HttpRequest req, long id) => gate(req, "partners", adm =>
        {
            var p = db.QueryOne("SELECT * FROM training_partners WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(req).GetAwaiter().GetResult();
            var amount = Math.Round(H.GetNum(b, "amount") ?? 0, 2);
            if (amount <= 0) return Results.Json(new { error = "bad_amount" }, statusCode: 400);
            db.Execute("INSERT INTO partner_payouts(partner_id,amount,currency,note,paid_by) VALUES(?,?, 'USD', ?, ?)",
                id, amount, H.GetS(b, "note"), adm.Id);
            log(adm.Id, "partner_payout_recorded", $"partner {id} USD {amount}");
            return J(new { ok = true, ledger = CommissionLedger(db.QueryOne("SELECT * FROM training_partners WHERE id=?", id)!) });
        }));
    }
}
