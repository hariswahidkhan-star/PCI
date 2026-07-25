using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Partner sponsorship + commissions, riding on the institution portal's own auth
/// (partner_users / partner_sessions — see <see cref="PartnerPortal"/>). Two capabilities on top of
/// the code-based portal:
///
///   Bulk candidate sponsorship — an enabled institution registers candidates directly: each row
///   creates the account if new (set-password welcome email), an approved sponsored application and a
///   sponsor-funded exam entitlement for the chosen certification, then tracks per-candidate progress
///   (application → exam access → result → credential), always scoped per certification.
///
///   Commission ledger — an IMMUTABLE, transaction-level ledger (see <see cref="PartnerCommission"/>).
///   A settled payment writes one commission transaction whose rate, basis and rule are snapshotted at
///   that moment, so changing an agreement can never restate history; refunds and chargebacks add a
///   negative reversal rather than editing the original. Settlements (<see cref="PartnerSettlement"/>)
///   allocate specific transactions to a payout batch under prepare → approve → pay, with the approver
///   required to differ from the preparer. Statements and remittance advice are rendered on demand from
///   the same ledger (<see cref="PartnerStatement"/>), so they can never drift from it.
///
///   The pre-ledger derived view (attributed revenue × the partner's CURRENT commission_pct, offset by
///   partner_payouts) is still served by <c>CommissionLedger</c> below for the legacy portal screens.
///   It is a display of the old model only — nothing settles or pays from it.
/// </summary>
public static class Partners
{
    const int MaxBatch = 200;

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult? Need(HttpContext ctx, out PartnerCtx? p)
        {
            p = Auth.PartnerFromReq(ctx.Request, db);
            return p is null ? Results.Json(new { error = "unauthorized" }, statusCode: 401) : null;
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
                WHERE COALESCE(p.partner_id, dc.partner_id)=? AND p.payment_status='paid'
                ORDER BY p.payment_date DESC LIMIT 500", pid);
            var attributed = db.Scalar<double>(@"SELECT COALESCE(SUM(p.final_amount),0)
                FROM code_redemptions r
                JOIN discount_codes dc ON dc.id=r.code_id
                JOIN payments p ON p.id=r.payment_id
                WHERE COALESCE(p.partner_id, dc.partner_id)=? AND p.payment_status='paid'", pid);
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
        // entitlement / attempt / credential state, always scoped per certification. The institution
        // registered these candidates itself, so their identity is not privacy-masked here.
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

        // ── Partner portal: sponsored candidates + progress ──
        app.MapGet("/api/partner/candidates", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var partner = db.QueryOne("SELECT sponsor_enabled FROM training_partners WHERE id=?", p!.PartnerId);
            return J(new { sponsor_enabled = partner is not null && H.B(partner["sponsor_enabled"]), rows = CandidateRows(p.PartnerId) });
        });

        // ── Partner portal: bulk-sponsor candidates ──
        // For each {email, first_name, last_name, certification}: create the account if new (with a
        // set-password welcome email), record an approved sponsored application and grant a sponsor-
        // funded exam entitlement — all scoped to the requested certification. Per-row results; a
        // failure in one row never blocks the rest. Institution admin/finance roles only.
        app.MapPost("/api/partner/candidates", async (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            if (p!.Role is not ("admin" or "finance"))
                return Results.Json(new { error = "role_forbidden", message = "Only institution admin/finance users can sponsor candidates." }, statusCode: 403);
            var partner = db.QueryOne("SELECT * FROM training_partners WHERE id=?", p.PartnerId);
            if (partner is null || !H.B(partner["sponsor_enabled"]))
                return Results.Json(new { error = "sponsorship_disabled", message = "Direct candidate sponsorship is not enabled in your agreement. Contact PCI to enable it." }, statusCode: 403);
            var pid = p.PartnerId;
            var body = await H.BodyEl(ctx.Request);
            if (body.ValueKind != System.Text.Json.JsonValueKind.Object
                || !body.TryGetProperty("candidates", out var list)
                || list.ValueKind != System.Text.Json.JsonValueKind.Array)
                return Results.Json(new { error = "candidates_required" }, statusCode: 400);
            if (list.GetArrayLength() > MaxBatch)
                return Results.Json(new { error = "batch_too_large", message = $"At most {MaxBatch} candidates per request." }, statusCode: 400);

            var baseUrl = Mailer.BaseUrl(ctx.Request);
            var results = new List<object>();
            int granted = 0;
            foreach (var el in list.EnumerateArray())
            {
                var row = H.ToMap(el);
                var email = (H.GetS(row, "email") ?? "").Trim().ToLowerInvariant();
                if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                { results.Add(new { email, status = "invalid_email" }); continue; }
                var certSel = Certs.TryResolve(db, H.GetS(row, "certification") ?? H.GetS(row, "certification_id"));
                if (certSel is null) { results.Add(new { email, status = "bad_certification" }); continue; }
                var certId = certSel.Value;
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
                    uid, certId, System.Text.Json.JsonSerializer.Serialize(new { sponsored_by = H.Str(partner["name"]), partner_id = pid, registered_by = p.Email }));
                db.Execute("UPDATE certification_applications SET application_no=? WHERE id=?", $"PCI-APP-{DateTime.UtcNow.Year}-{100000 + appId}", appId);
                var entRef = Applications.GrantExamEntitlement(db, uid, certId, "sponsored", "sponsored");
                db.Execute(@"INSERT INTO partner_sponsorships(partner_id,user_id,application_id,certification_id,route_key,candidate_email,candidate_name)
                    VALUES(?,?,?,?, 'sponsored', ?, ?)", pid, uid, appId, certId, email, name);
                try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Sponsorship', 'Exam access sponsored', ?, 'Schedule exam', '/certifications')",
                    uid, $"{H.Str(partner["name"])} has sponsored your {H.Str(cert["acronym"]) ?? H.Str(cert["code"])} examination. Your exam access is open."); } catch { }
                if (entRef is not null) granted++;
                results.Add(new { email, status = entRef is not null ? "sponsored" : "sponsored_already_entitled", account = created ? "created" : "existing", certification = cert["code"] });
            }
            log(null, "partner_bulk_sponsor", $"partner {pid} sponsored {granted}/{list.GetArrayLength()} candidates (by {p.Email})");
            try { Notify.Alert(db, "partners", $"{H.Str(partner["name"])} sponsored candidates", $"<p><strong>{System.Net.WebUtility.HtmlEncode(H.Str(partner["name"]) ?? "")}</strong> registered {granted} sponsored candidate(s) through the partner portal.</p>", "partner", pid, null); } catch { }
            return J(new { ok = true, results });
        });

        // ── Partner portal: commission ledger ──
        app.MapGet("/api/partner/commissions", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var partner = db.QueryOne("SELECT * FROM training_partners WHERE id=?", p!.PartnerId);
            if (partner is null) return Results.Json(new { error = "institution_not_found" }, statusCode: 404);
            return J(CommissionLedger(partner));
        });

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

        // ================= immutable commission ledger (Phase 1) =================
        // The derived CommissionLedger above stays the response contract during the migration window;
        // these surfaces expose the real per-transaction ledger that will replace it in Phase 3.

        // Shape one ledger row for an API response. Minor units are rendered as decimals here — the only
        // place money leaves integer form — and user_id is never included.
        object TxnDto(Dictionary<string, object?> r) => new
        {
            id = H.L(r["id"]),
            reference = H.Str(r["txn_ref"]),
            status = H.Str(r["status"]),
            payment_id = r["payment_id"] is null ? (long?)null : H.L(r["payment_id"]),
            certification_id = r["certification_id"] is null ? (long?)null : H.L(r["certification_id"]),
            product_type = H.Str(r["product_type"]),
            currency = H.Str(r["currency"]),
            gross = Money.ToDecimal(H.L(r["gross_minor"])),
            discount = Money.ToDecimal(H.L(r["discount_minor"])),
            eligible_net = Money.ToDecimal(H.L(r["eligible_net_minor"])),
            commission_type = H.Str(r["commission_type"]),
            commission_percent = Money.BasisPointsToPercent(H.L(r["commission_rate_bp"])),
            commission_basis = H.Str(r["commission_basis"]),
            commission = Money.ToDecimal(H.L(r["commission_minor"])),
            earned_at = H.Str(r["earned_at"]),
            hold_until = H.Str(r["hold_until"]),
            approved_at = H.Str(r["approved_at"]),
            reversal_of = r["reversal_of_transaction_id"] is null ? (long?)null : H.L(r["reversal_of_transaction_id"]),
            requires_finance_review = H.L(r["requires_finance_review"]) == 1,
            created_at = H.Str(r["created_at"]),
        };

        object LedgerView(long partnerId, int limit, int offset)
        {
            var t = PartnerCommission.Totals(db, partnerId);
            var rows = db.Query(@"SELECT * FROM partner_commission_transactions WHERE partner_id=?
                ORDER BY id DESC LIMIT ? OFFSET ?", partnerId, limit, offset).Select(TxnDto).ToList();
            return new
            {
                totals = new
                {
                    gross = Money.ToDecimal(t["gross"]),
                    net = Money.ToDecimal(t["net"]),
                    commission = Money.ToDecimal(t["commission"]),
                    pending = Money.ToDecimal(t["pending"]),
                    due = Money.ToDecimal(t["due"]),
                    approved = Money.ToDecimal(t["approved"]),
                    paid = Money.ToDecimal(t["paid"]),
                    reversed = Money.ToDecimal(t["reversed"]),
                },
                count = db.Scalar<long>("SELECT COUNT(*) FROM partner_commission_transactions WHERE partner_id=?", partnerId),
                limit, offset, transactions = rows,
            };
        }

        static (int limit, int offset) Page(HttpRequest req)
        {
            var limit = int.TryParse(req.Query["limit"], out var l) ? Math.Clamp(l, 1, 200) : 50;
            var offset = int.TryParse(req.Query["offset"], out var o) ? Math.Max(0, o) : 0;
            return (limit, offset);
        }

        // ── Partner portal: its own ledger, paginated (no LIMIT 500 wall) ──
        app.MapGet("/api/partner/commission-ledger", (HttpContext ctx) =>
        {
            if (Need(ctx, out var p) is { } deny) return deny;
            var (limit, offset) = Page(ctx.Request);
            return J(LedgerView(p!.PartnerId, limit, offset));
        });

        // ── Admin: the same ledger ──
        app.MapGet("/api/admin/partner-finance/{id}/ledger", (HttpRequest req, long id) => gate(req, "partners", _ =>
        {
            var (limit, offset) = Page(req);
            return J(LedgerView(id, limit, offset));
        }));

        // ── Finance: effective-dated agreements and commission rules ──
        app.MapGet("/api/admin/partner-finance/{id}/agreements", (HttpRequest req, long id) => gate(req, "partners", _ =>
            J(new
            {
                agreements = db.Query("SELECT * FROM partner_agreements WHERE partner_id=? ORDER BY effective_from DESC, id DESC", id),
                rules = db.Query("SELECT * FROM partner_commission_rules WHERE partner_id=? ORDER BY priority ASC, id DESC", id),
            })));

        app.MapPost("/api/admin/partner-finance/{id}/agreements", (HttpContext ctx, long id) => gate(ctx.Request, "finance", adm =>
        {
            if (db.QueryOne("SELECT id FROM training_partners WHERE id=?", id) is null)
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var from = (H.GetS(b, "effective_from") ?? "").Trim();
            if (from.Length != 10) return Results.Json(new { error = "effective_from_required", message = "effective_from must be YYYY-MM-DD." }, statusCode: 400);
            var to = (H.GetS(b, "effective_to") ?? "").Trim();
            if (to.Length > 0 && string.CompareOrdinal(to, from) < 0)
                return Results.Json(new { error = "bad_window", message = "effective_to cannot precede effective_from." }, statusCode: 400);
            var number = (H.GetS(b, "agreement_number") ?? $"AGR-{id}-{DateTime.UtcNow:yyyyMMddHHmmss}").Trim();
            if (db.QueryOne("SELECT id FROM partner_agreements WHERE agreement_number=?", number) is not null)
                return Results.Json(new { error = "agreement_number_taken" }, statusCode: 409);
            var holdDays = (int)Math.Clamp(H.GetNum(b, "refund_hold_days") ?? 30, 0, 365);
            var termsDays = (int)Math.Clamp(H.GetNum(b, "payment_terms_days") ?? 30, 0, 365);
            var minPayout = Money.ToMinor(H.GetNum(b, "minimum_payout") ?? 0);
            // Supersede the previous open agreement so exactly one is in force at a time.
            db.Execute("UPDATE partner_agreements SET status='superseded', updated_at=datetime('now') WHERE partner_id=? AND status='active'", id);
            var aid = db.ExecuteReturningId(@"INSERT INTO partner_agreements
                (partner_id,agreement_number,effective_from,effective_to,currency,payment_terms_days,minimum_payout_minor,refund_hold_days,tax_treatment,status,note,created_by)
                VALUES(?,?,?,?,?,?,?,?,?, 'active',?,?)",
                id, number, from, to.Length == 0 ? null : to, (H.GetS(b, "currency") ?? "USD").ToUpperInvariant(),
                termsDays, minPayout, holdDays, H.GetS(b, "tax_treatment"), H.GetS(b, "note"), adm.Id);
            log(adm.Id, "partner_agreement_created", $"partner {id} agreement {number} from {from} by {adm.Id}");
            return J(new { ok = true, id = aid, agreement_number = number });
        }));

        app.MapPost("/api/admin/partner-finance/agreements/{agreementId}/rules", (HttpContext ctx, long agreementId) => gate(ctx.Request, "finance", adm =>
        {
            var agreement = db.QueryOne("SELECT id,partner_id FROM partner_agreements WHERE id=?", agreementId);
            if (agreement is null) return Results.Json(new { error = "agreement_not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var type = (H.GetS(b, "commission_type") ?? "percentage").Trim().ToLowerInvariant();
            if (type is not ("percentage" or "fixed")) return Results.Json(new { error = "bad_commission_type" }, statusCode: 400);
            var basis = (H.GetS(b, "commission_basis") ?? "net_after_discount").Trim().ToLowerInvariant();
            if (basis is not ("gross" or "net_after_discount" or "net_after_tax")) return Results.Json(new { error = "bad_basis" }, statusCode: 400);
            // Reject an out-of-range rate rather than clamping it — a silently altered commission rate is
            // exactly the class of bug this module exists to remove.
            var percent = H.GetNum(b, "commission_percent") ?? 0;
            if (type == "percentage" && (percent < 0 || percent > 100))
                return Results.Json(new { error = "bad_percent", message = "commission_percent must be between 0 and 100." }, statusCode: 422);
            var fixedMinor = Money.ToMinor(H.GetNum(b, "commission_fixed") ?? 0);
            if (type == "fixed" && fixedMinor <= 0)
                return Results.Json(new { error = "bad_fixed_amount", message = "commission_fixed must be greater than zero." }, statusCode: 422);
            var rid = db.ExecuteReturningId(@"INSERT INTO partner_commission_rules
                (agreement_id,partner_id,certification_id,route_key,product_type,country,commission_type,commission_rate_bp,commission_fixed_minor,commission_basis,effective_from,effective_to,priority,active,created_by)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,1,?)",
                agreementId, H.L(agreement["partner_id"]),
                H.GetNum(b, "certification_id") is { } c && c > 0 ? (long?)c : null,
                H.GetS(b, "route_key"), H.GetS(b, "product_type"), H.GetS(b, "country"),
                type, Money.PercentToBasisPoints(percent), fixedMinor, basis,
                H.GetS(b, "effective_from"), H.GetS(b, "effective_to"),
                (long)Math.Clamp(H.GetNum(b, "priority") ?? 100, 1, 1000), adm.Id);
            log(adm.Id, "partner_commission_rule_created", $"agreement {agreementId} rule {rid} {type} by {adm.Id}");
            return J(new { ok = true, id = rid });
        }));

        // ── Finance: historical backfill. Dry run unless dry_run=0 is passed explicitly. ──
        app.MapPost("/api/admin/partner-finance/backfill", (HttpContext ctx) => gate(ctx.Request, "finance", adm =>
        {
            var dryRun = ctx.Request.Query["dry_run"] != "0";
            var partnerId = long.TryParse(ctx.Request.Query["partner_id"], out var p) && p > 0 ? p : (long?)null;
            var report = PartnerFinanceBackfill.Run(db, dryRun, partnerId);
            // A live run moves money-relevant history; a dry run does not, so only the former is audited
            // as a mutation (the dry run is still visible as a request in the access log).
            if (!dryRun)
                log(adm.Id, "partner_finance_backfill", $"partner {(partnerId?.ToString() ?? "all")} by {adm.Id}: " +
                    $"{report.CommissionsCreated} created, {report.ReversalsCreated} reversed, {report.LegacyPayoutsImported} legacy payouts imported");
            return J(new
            {
                dry_run = report.DryRun,
                payments_examined = report.PaymentsExamined,
                commissions_created = report.CommissionsCreated,
                commissions_already_present = report.CommissionsAlreadyPresent,
                reversals_created = report.ReversalsCreated,
                requires_finance_review = report.NeedsFinanceReview,
                legacy_payouts_imported = report.LegacyPayoutsImported,
                commission_total = Money.ToDecimal(report.CommissionMinor),
                exceptions = report.Exceptions,
                note = report.DryRun
                    ? "Dry run — nothing was written. Re-send with ?dry_run=0 to apply."
                    : "Applied. Re-running is safe: creation is idempotent.",
            });
        }));

        // ── Finance: reconciliation. Reports discrepancies; never repairs them silently. ──
        app.MapGet("/api/admin/partner-finance/reconcile", (HttpRequest req) => gate(req, "finance", _ =>
        {
            var partnerId = long.TryParse(req.Query["partner_id"], out var p) && p > 0 ? p : (long?)null;
            return J(PartnerFinanceBackfill.Reconcile(db, partnerId));
        }));

        // ══ Phase 3 — settlements, statements and disputes ═══════════════════════════════════════════

        // Refusals carry the engine's own reason code so the console can explain WHY, not just that it
        // failed. 409 for a state/duty conflict (wrong status, self-approval, duplicate reference),
        // 422 for a request that is well-formed but financially invalid (over-payment, nothing payable).
        IResult Settle(PartnerSettlement.Result r)
        {
            if (r.Ok) return J(new { ok = true, id = r.Id, data = r.Data });
            var code = r.Error switch
            {
                "not_found" or "partner_not_found" => 404,
                "bad_status" or "maker_checker" or "maker_unknown" or "already_paid" or "duplicate_reference" => 409,
                _ => 422,
            };
            return Results.Json(new { error = r.Error, message = r.Message }, statusCode: code);
        }

        object SettlementSummary(long partnerId) => new
        {
            payable = Money.ToDecimal(PartnerSettlement.PayableMinor(db, partnerId)),
            recoverable = Money.ToDecimal(PartnerCommissionReversal.RecoverableMinor(db, partnerId)),
            recoverable_outstanding = Money.ToDecimal(PartnerSettlement.UnrecoveredMinor(db, partnerId)),
            settlements = db.Query(@"SELECT id, settlement_no, period_start, period_end, currency,
                    eligible_commission_minor, adjustments_minor, amount_approved_minor, amount_paid_minor,
                    closing_balance_minor, status, prepared_by, approved_by, paid_at, payment_method,
                    payment_reference, proof_storage_ref, internal_note, partner_note, created_at
                FROM partner_settlements WHERE partner_id=? ORDER BY id DESC LIMIT 200", partnerId),
        };

        app.MapGet("/api/admin/partner-finance/{id}/settlements", (HttpRequest req, long id) =>
            gate(req, "pf_view", _ => J(SettlementSummary(id))));

        app.MapGet("/api/admin/partner-finance/settlements/{sid}", (HttpRequest req, long sid) => gate(req, "pf_view", _ =>
        {
            var s = db.QueryOne("SELECT * FROM partner_settlements WHERE id=?", sid);
            if (s is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new
            {
                settlement = s,
                items = db.Query(@"SELECT i.id, i.transaction_id, i.amount_allocated_minor, t.txn_ref, t.earned_at,
                        t.product_type, t.commission_minor, t.status, dc.code
                    FROM partner_settlement_items i
                    JOIN partner_commission_transactions t ON t.id=i.transaction_id
                    LEFT JOIN discount_codes dc ON dc.id=t.discount_code_id
                    WHERE i.settlement_id=? ORDER BY t.earned_at ASC, t.id ASC", sid),
            });
        }));

        // Maker: assemble the batch. Nothing is payable until a DIFFERENT person approves it.
        app.MapPost("/api/admin/partner-finance/{id}/settlements", (HttpContext ctx, long id) => gate(ctx.Request, "pf_prepare", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var capNum = H.GetNum(b, "cap_amount", "max_amount");
            var cap = capNum is { } c && c > 0 ? (long?)Money.ToMinor(c) : null;
            var r = PartnerSettlement.Prepare(db, id, adm.Id,
                H.GetS(b, "period_start"), H.GetS(b, "period_end"), cap, H.GetS(b, "note"));
            if (r.Ok) log(adm.Id, "partner_settlement_prepared", $"partner {id} settlement {r.Id} by {adm.Id}");
            return Settle(r);
        }));

        // Checker: authorise it. The engine refuses when the approver is the preparer.
        app.MapPost("/api/admin/partner-finance/settlements/{sid}/approve", (HttpContext ctx, long sid) => gate(ctx.Request, "pf_approve", adm =>
        {
            var r = PartnerSettlement.Approve(db, sid, adm.Id);
            if (r.Ok) log(adm.Id, "partner_settlement_approved", $"settlement {sid} by {adm.Id}");
            else if (r.Error is "maker_checker" or "maker_unknown")
                // A blocked self-approval is a control working, and is worth seeing in the audit trail.
                log(adm.Id, "partner_settlement_approval_refused", $"settlement {sid} by {adm.Id}: {r.Error}");
            return Settle(r);
        }));

        // Record the money leaving, with its evidence. Partial payments are supported and re-checked.
        app.MapPost("/api/admin/partner-finance/settlements/{sid}/pay", (HttpContext ctx, long sid) => gate(ctx.Request, "pf_pay", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var s = db.QueryOne("SELECT amount_approved_minor,amount_paid_minor FROM partner_settlements WHERE id=?", sid);
            if (s is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // Omitting the amount pays the outstanding balance in full — the common case, and one where a
            // hand-typed figure is the likeliest source of an error.
            var amountMinor = H.GetNum(b, "amount") is { } a
                ? Money.ToMinor(a)
                : H.L(s["amount_approved_minor"]) - H.L(s["amount_paid_minor"]);
            var r = PartnerSettlement.Pay(db, sid, adm.Id, amountMinor,
                H.GetS(b, "method"), H.GetS(b, "reference"), H.GetS(b, "proof_ref"), H.GetS(b, "partner_note"));
            if (r.Ok) log(adm.Id, "partner_settlement_paid", $"settlement {sid} {Money.Format(amountMinor)} by {adm.Id}");
            return Settle(r);
        }));

        app.MapPost("/api/admin/partner-finance/settlements/{sid}/cancel", (HttpContext ctx, long sid) => gate(ctx.Request, "pf_prepare", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var r = PartnerSettlement.Cancel(db, sid, adm.Id, H.GetS(b, "reason") ?? "");
            if (r.Ok) log(adm.Id, "partner_settlement_cancelled", $"settlement {sid} by {adm.Id}");
            return Settle(r);
        }));

        // ── Statements ──
        // One builder serves admin and partner, so the two never disagree about a number.
        (string start, string end) PeriodOf(HttpRequest req)
        {
            var from = req.Query["from"].ToString().Trim();
            var to = req.Query["to"].ToString().Trim();
            if (from.Length == 10 && to.Length == 10) return (from, to);
            var month = req.Query["month"].ToString().Trim();             // YYYY-MM
            if (month.Length == 7 && DateTime.TryParse(month + "-01", System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var m))
                return PartnerStatement.MonthOf(m);
            return PartnerStatement.MonthOf(DateTime.UtcNow);
        }

        object StatementJson(PartnerStatement.Statement st) => new
        {
            partner_id = st.PartnerId,
            partner = st.PartnerName,
            period_start = st.PeriodStart,
            period_end = st.PeriodEnd,
            currency = st.Currency,
            opening = Money.ToDecimal(st.OpeningMinor),
            earned = Money.ToDecimal(st.EarnedMinor),
            reversed = Money.ToDecimal(st.ReversedMinor),
            paid = Money.ToDecimal(st.PaidMinor),
            closing = Money.ToDecimal(st.ClosingMinor),
            recoverable = Money.ToDecimal(st.RecoverableMinor),
            lines = st.Lines.Select(l => new
            {
                l.Id, reference = l.Ref, date = l.Date, kind = l.Kind, code = l.Code, product = l.Product,
                currency = l.Currency,
                gross = Money.ToDecimal(l.GrossMinor),
                discount = Money.ToDecimal(l.DiscountMinor),
                eligible_net = Money.ToDecimal(l.NetMinor),
                rate_percent = Money.BasisPointsToPercent(l.RateBp),
                commission = Money.ToDecimal(l.CommissionMinor),
                status = l.Status,
            }).ToArray(),
            settlements = st.Settlements,
        };

        IResult StatementResult(HttpRequest req, long partnerId, string format)
        {
            var (from, to) = PeriodOf(req);
            var st = PartnerStatement.Build(db, partnerId, from, to);
            var stem = $"statement-{partnerId}-{from}-to-{to}";
            return format switch
            {
                "csv" => Results.Text(PartnerStatement.Csv(st), "text/csv", System.Text.Encoding.UTF8),
                "pdf" => Results.File(PartnerStatement.Pdf(st, PartnerStatement.OrgName(db)), "application/pdf", $"{stem}.pdf"),
                _ => J(StatementJson(st)),
            };
        }

        app.MapGet("/api/admin/partner-finance/{id}/statement", (HttpRequest req, long id) =>
            gate(req, "pf_view", _ => StatementResult(req, id, req.Query["format"].ToString().ToLowerInvariant())));

        app.MapGet("/api/admin/partner-finance/settlements/{sid}/remittance", (HttpRequest req, long sid) => gate(req, "pf_view", _ =>
        {
            var pdf = PartnerStatement.RemittancePdf(db, sid, PartnerStatement.OrgName(db));
            return pdf is null
                ? Results.Json(new { error = "not_found" }, statusCode: 404)
                : Results.File(pdf, "application/pdf", $"remittance-{sid}.pdf");
        }));

        // ── Partner portal: read-only money views ──
        // Money is restricted to the institution's admin/finance users, matching the existing rule on code
        // creation. A reporting or support login can see performance, never the payout ledger.
        IResult? NeedFinance(HttpContext ctx, out PartnerCtx? p)
        {
            var deny = Need(ctx, out p);
            if (deny is not null) return deny;
            return p!.Role is "admin" or "finance"
                ? null
                : Results.Json(new { error = "role_forbidden", message = "Only institution admin/finance users can view settlements and statements." }, statusCode: 403);
        }

        app.MapGet("/api/partner/settlements", (HttpContext ctx) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            // internal_note and prepared_by/approved_by are ours, not the partner's — deliberately omitted.
            return J(new
            {
                payable = Money.ToDecimal(PartnerSettlement.PayableMinor(db, p!.PartnerId)),
                settlements = db.Query(@"SELECT id, settlement_no, period_start, period_end, currency,
                        amount_approved_minor, amount_paid_minor, closing_balance_minor, status, paid_at,
                        payment_method, payment_reference, partner_note, created_at
                    FROM partner_settlements
                    WHERE partner_id=? AND status IN ('approved','scheduled','paid') ORDER BY id DESC LIMIT 200", p.PartnerId),
            });
        });

        app.MapGet("/api/partner/settlements/{sid}/remittance", (HttpContext ctx, long sid) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            var owner = db.QueryOne("SELECT partner_id,status FROM partner_settlements WHERE id=?", sid);
            if (owner is null || H.L(owner["partner_id"]) != p!.PartnerId)
                return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(owner["status"]) != PartnerSettlement.StatusPaid)
                return Results.Json(new { error = "not_paid", message = "A remittance advice is available once the settlement has been paid." }, statusCode: 409);
            var pdf = PartnerStatement.RemittancePdf(db, sid, PartnerStatement.OrgName(db));
            return pdf is null
                ? Results.Json(new { error = "not_found" }, statusCode: 404)
                : Results.File(pdf, "application/pdf", $"remittance-{sid}.pdf");
        });

        app.MapGet("/api/partner/statement", (HttpContext ctx) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            return StatementResult(ctx.Request, p!.PartnerId, ctx.Request.Query["format"].ToString().ToLowerInvariant());
        });

        // ── Disputes ──
        string DisputeNo(long partnerId) => $"PD-{partnerId}-{DateTime.UtcNow:yyyyMMddHHmmss}";

        app.MapGet("/api/partner/disputes", (HttpContext ctx) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            var rows = db.Query(@"SELECT id, dispute_no, transaction_id, settlement_id, category, subject, detail,
                    claimed_amount_minor, currency, status, resolution, resolved_at, created_at, updated_at
                FROM partner_disputes WHERE partner_id=? ORDER BY id DESC LIMIT 200", p!.PartnerId);
            return J(new { disputes = rows });
        });

        app.MapPost("/api/partner/disputes", async (HttpContext ctx) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var subject = (H.GetS(b, "subject") ?? "").Trim();
            if (subject.Length < 4) return Results.Json(new { error = "subject_required", message = "Describe the dispute in a short subject line." }, statusCode: 400);
            var category = (H.GetS(b, "category") ?? "other").Trim().ToLowerInvariant();
            if (category is not ("missing_commission" or "wrong_rate" or "wrong_amount" or "not_paid" or "other"))
                return Results.Json(new { error = "bad_category" }, statusCode: 400);

            // A partner may only dispute their own rows. Silently accepting someone else's id would let one
            // partner probe another's ledger through the error responses.
            long? txnId = null, setId = null;
            if (H.GetNum(b, "transaction_id") is { } t && t > 0)
            {
                var row = db.QueryOne("SELECT partner_id FROM partner_commission_transactions WHERE id=?", (long)t);
                if (row is null || H.L(row["partner_id"]) != p!.PartnerId)
                    return Results.Json(new { error = "transaction_not_found" }, statusCode: 404);
                txnId = (long)t;
            }
            if (H.GetNum(b, "settlement_id") is { } s2 && s2 > 0)
            {
                var row = db.QueryOne("SELECT partner_id FROM partner_settlements WHERE id=?", (long)s2);
                if (row is null || H.L(row["partner_id"]) != p!.PartnerId)
                    return Results.Json(new { error = "settlement_not_found" }, statusCode: 404);
                setId = (long)s2;
            }

            var no = DisputeNo(p!.PartnerId);
            var detail = H.GetS(b, "detail");
            var id = db.ExecuteReturningId(@"INSERT INTO partner_disputes
                (dispute_no,partner_id,transaction_id,settlement_id,category,subject,detail,claimed_amount_minor,currency,raised_by_partner_user_id)
                VALUES(?,?,?,?,?,?,?,?,?,?)",
                no, p.PartnerId, txnId, setId, category, subject, detail,
                Money.ToMinor(H.GetNum(b, "claimed_amount") ?? 0),
                (H.GetS(b, "currency") ?? "USD").ToUpperInvariant(), p.Id);
            if (!string.IsNullOrWhiteSpace(detail))
                db.Execute("INSERT INTO partner_dispute_messages(dispute_id,author_type,author_id,body) VALUES(?, 'partner',?,?)", id, p.Id, detail);
            return J(new { ok = true, id, dispute_no = no });
        });

        app.MapGet("/api/partner/disputes/{did}/messages", (HttpContext ctx, long did) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            var d = db.QueryOne("SELECT partner_id FROM partner_disputes WHERE id=?", did);
            if (d is null || H.L(d["partner_id"]) != p!.PartnerId) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // internal=1 notes are the reviewer's working notes and are never returned to the partner.
            return J(new
            {
                messages = db.Query(@"SELECT id, author_type, body, created_at FROM partner_dispute_messages
                    WHERE dispute_id=? AND internal=0 ORDER BY id ASC", did),
            });
        });

        app.MapPost("/api/partner/disputes/{did}/messages", async (HttpContext ctx, long did) =>
        {
            if (NeedFinance(ctx, out var p) is { } deny) return deny;
            var d = db.QueryOne("SELECT partner_id,status FROM partner_disputes WHERE id=?", did);
            if (d is null || H.L(d["partner_id"]) != p!.PartnerId) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(d["status"]) is "resolved" or "rejected" or "withdrawn")
                return Results.Json(new { error = "closed", message = "This dispute is closed. Raise a new one if the issue persists." }, statusCode: 409);
            var b = await H.Body(ctx.Request);
            var body = (H.GetS(b, "body") ?? "").Trim();
            if (body.Length == 0) return Results.Json(new { error = "body_required" }, statusCode: 400);
            var id = db.ExecuteReturningId("INSERT INTO partner_dispute_messages(dispute_id,author_type,author_id,body) VALUES(?, 'partner',?,?)", did, p!.Id, body);
            db.Execute("UPDATE partner_disputes SET updated_at=datetime('now') WHERE id=?", did);
            return J(new { ok = true, id });
        });

        app.MapGet("/api/admin/partner-finance/disputes", (HttpRequest req) => gate(req, "pf_dispute", _ =>
        {
            var status = req.Query["status"].ToString().Trim().ToLowerInvariant();
            var partnerId = long.TryParse(req.Query["partner_id"], out var pv) && pv > 0 ? pv : (long?)null;
            return J(new
            {
                disputes = db.Query(@"SELECT d.*, tp.name partner_name FROM partner_disputes d
                    LEFT JOIN training_partners tp ON tp.id=d.partner_id
                    WHERE (? = '' OR d.status=?) AND (? IS NULL OR d.partner_id=?)
                    ORDER BY d.id DESC LIMIT 200", status, status, partnerId, partnerId),
            });
        }));

        app.MapGet("/api/admin/partner-finance/disputes/{did}", (HttpRequest req, long did) => gate(req, "pf_dispute", _ =>
        {
            var d = db.QueryOne("SELECT * FROM partner_disputes WHERE id=?", did);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new
            {
                dispute = d,
                messages = db.Query("SELECT * FROM partner_dispute_messages WHERE dispute_id=? ORDER BY id ASC", did),
            });
        }));

        app.MapPost("/api/admin/partner-finance/disputes/{did}", (HttpContext ctx, long did) => gate(ctx.Request, "pf_dispute", adm =>
        {
            var d = db.QueryOne("SELECT id,status FROM partner_disputes WHERE id=?", did);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();

            var reply = (H.GetS(b, "reply") ?? "").Trim();
            var isInternal = (H.GetS(b, "visibility") ?? "partner") == "internal";
            var status = (H.GetS(b, "status") ?? "").Trim().ToLowerInvariant();
            if (status.Length > 0 && status is not ("open" or "under_review" or "resolved" or "rejected" or "withdrawn"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var resolution = (H.GetS(b, "resolution") ?? "").Trim();
            var closing = status is "resolved" or "rejected";
            // Closing a dispute without saying why leaves the partner with an outcome and no reason for it.
            if (closing && resolution.Length == 0)
                return Results.Json(new { error = "resolution_required", message = "Record the reason before closing a dispute." }, statusCode: 422);

            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            db.Transaction(() =>
            {
                if (reply.Length > 0)
                    db.Execute("INSERT INTO partner_dispute_messages(dispute_id,author_type,author_id,body,internal) VALUES(?, 'admin',?,?,?)",
                        did, adm.Id, reply, isInternal ? 1 : 0);
                // Every "leave this alone" is a NULL parameter under COALESCE rather than a conditional SQL
                // expression, so the statement is identical on MySQL and SQLite.
                if (status.Length > 0 || resolution.Length > 0)
                    db.Execute(@"UPDATE partner_disputes SET status=COALESCE(?,status), resolution=COALESCE(?,resolution),
                            resolved_by=COALESCE(?,resolved_by), resolved_at=COALESCE(?,resolved_at), updated_at=?
                        WHERE id=?",
                        status.Length > 0 ? status : null,
                        resolution.Length > 0 ? resolution : null,
                        closing ? (long?)adm.Id : null,
                        closing ? now : null,
                        now, did);
            });
            log(adm.Id, "partner_dispute_updated", $"dispute {did} status {(status.Length > 0 ? status : "unchanged")} by {adm.Id}");
            return J(new { ok = true, id = did });
        }));
    }
}
