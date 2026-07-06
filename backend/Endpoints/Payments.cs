using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;
using Stripe;
using Stripe.Checkout;

namespace PCI.Backend.Endpoints;

/// <summary>Stripe checkout + webhook — the ONLY place access is granted (after verified payment).</summary>
public static class Payments
{
    static readonly Dictionary<string, string> PRODUCT_LABEL = new()
    {
        ["membership"] = "Student Membership / Registration",
        ["exam"] = "PCP-AI Certification Exam",
        ["bundle"] = "Membership + Exam Bundle",
        ["renewal"] = "Annual Membership Renewal",
        ["recert"] = "PCP-AI Recertification (3-year cycle)"
    };
    static string Base => Environment.GetEnvironmentVariable("APP_BASE_URL") ?? "";
    static string Fmt(double n) => "USD " + (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.00"));

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log, Func<bool> stripeReady)
    {
        IResult J(object o) => Results.Json(o);
        IResult Stripe503() => Results.Json(new { error = "payments_not_configured", note = "Set STRIPE_SECRET_KEY in .env to enable checkout." }, statusCode: 503);

        // ---------- create Checkout Session ----------
        app.MapPost("/api/create-checkout-session", async (HttpRequest req) =>
        {
            if (!stripeReady()) return Stripe503();
            try
            {
                var d = await H.Body(req);
                var product = PRODUCT_LABEL.ContainsKey(H.GetS(d, "product") ?? "") ? H.GetS(d, "product")! : "membership";
                var email = H.GetS(d, "email");
                var codeVal = H.GetS(d, "code") is { } code ? Public.ValidateCode(db, code, product, email) : new Public.CodeValidation(null, null);
                var pr = Public.Pricing(db, product, codeVal.Code);
                db.Execute("UPDATE enrollment_sessions SET selected_product=?, pricing_snapshot=?, last_activity_at=datetime('now') WHERE email=? AND session_status='in_progress'",
                    product, JsonSerializer.Serialize(pr), (email ?? "").ToLowerInvariant());

                var options = new SessionCreateOptions
                {
                    Mode = "payment",
                    LineItems = new List<SessionLineItemOptions> {
                        new() { Quantity = 1, PriceData = new SessionLineItemPriceDataOptions {
                            Currency = "usd", UnitAmount = (long)Math.Round(pr.final * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions {
                                Name = PRODUCT_LABEL[product], Description = "Access only. Certification is awarded separately under PCI policies." } } } },
                    CustomerEmail = email,
                    Metadata = new Dictionary<string, string> {
                        ["product"] = product, ["plan_label"] = PRODUCT_LABEL[product],
                        ["first_name"] = H.GetS(d, "first") ?? "", ["last_name"] = H.GetS(d, "last") ?? "",
                        ["country"] = H.GetS(d, "country") ?? "", ["discount_code"] = codeVal.Code is null ? "" : (H.Str(codeVal.Code["code"]) ?? ""),
                        ["final_amount"] = pr.final.ToString(), ["code_amount"] = pr.codeAmount.ToString(),
                        ["standard_amount"] = pr.standard.ToString(), ["default_discount"] = pr.defaultDiscount.ToString() },
                    SuccessUrl = $"{Base}/payment-success.html?ref={{CHECKOUT_SESSION_ID}}&product={Uri.EscapeDataString(PRODUCT_LABEL[product])}",
                    CancelUrl = $"{Base}/payment-failed.html"
                };
                var session = await new SessionService().CreateAsync(options);
                return J(new { url = session.Url });
            }
            catch (Exception e) { Console.Error.WriteLine(e); return Results.Json(new { error = "session_failed" }, statusCode: 500); }
        });

        // ---------- webhook: the ONLY place access is granted ----------
        app.MapPost("/api/webhook", async (HttpRequest req) =>
        {
            if (!stripeReady()) return Stripe503();
            string payload;
            using (var reader = new StreamReader(req.Body)) payload = await reader.ReadToEndAsync();
            Event ev;
            try { ev = EventUtility.ConstructEvent(payload, req.Headers["stripe-signature"].ToString(), Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET") ?? ""); }
            catch (Exception err) { return Results.Text($"Webhook signature error: {err.Message}", "text/plain", statusCode: 400); }

            if (ev.Type == "checkout.session.completed")
            {
                var s = ev.Data.Object as Session;
                var m = s?.Metadata ?? new();
                var email = (s?.CustomerDetails?.Email ?? s?.CustomerEmail ?? "").ToLowerInvariant();
                if (!string.IsNullOrEmpty(email))
                {
                  // Fully atomic settlement: user, membership, payment, redemption, tokens all commit
                  // together or roll back together. Combined with INSERT OR IGNORE this is idempotent.
                  db.Transaction(() =>
                  {
                    var product = m.GetValueOrDefault("product", "membership");
                    double MetaNum(string k) => double.TryParse(m.GetValueOrDefault(k, ""), out var v) ? v : 0;
                    var reference = "PCI-" + (s!.Id.Length >= 8 ? s.Id[^8..] : s.Id).ToUpperInvariant();
                    var amountTotal = (s.AmountTotal ?? 0) / 100.0;

                    // ── IDEMPOTENCY GATE (runs BEFORE any side effect, incl. user creation) ──
                    // Resolve the existing user id if any (do NOT create yet). The payment is keyed on the
                    // Stripe PaymentIntent via a UNIQUE index; a replayed delivery inserts 0 rows → we stop,
                    // having created nothing. Only a genuinely-new payment proceeds to create/activate.
                    var existing = db.QueryOne("SELECT id FROM users WHERE email=?", email);
                    long? existingId = existing is null ? (long?)null : H.L(existing["id"]);
                    var sess = db.QueryOne("SELECT id FROM enrollment_sessions WHERE email=? ORDER BY id DESC LIMIT 1", email);
                    var (payId, payChanges) = db.ExecuteWithChanges(@"INSERT OR IGNORE INTO payments(user_id,enrollment_session_id,product_type,standard_amount,default_discount_amount,discount_code,discount_code_amount,final_amount,currency,payment_provider,provider_payment_id,payment_status,payment_date,reference)
                        VALUES(?,?,?,?,?,?,?,?,'USD','stripe',?,'paid',datetime('now'),?)",
                        existingId, sess?["id"], product, MetaNum("standard_amount"), MetaNum("default_discount"),
                        m.GetValueOrDefault("discount_code"), MetaNum("code_amount"), MetaNum("final_amount") != 0 ? MetaNum("final_amount") : amountTotal, s.PaymentIntentId, reference);

                    // Record the webhook event in the idempotency ledger (belt-and-braces alongside the
                    // payment unique index). A replayed event id inserts 0 rows.
                    var (_, evChanges) = db.ExecuteWithChanges("INSERT OR IGNORE INTO webhook_events(provider,event_id,status) VALUES('stripe',?,'processed')", ev.Id);
                    if (evChanges == 0) { log(existingId, "webhook_event_replay_ignored", ev.Id); return; }

                    if (payChanges == 0) { log(existingId, "webhook_replay_ignored", s.PaymentIntentId); return; } // already settled → no side effects

                    // ── SIDE EFFECTS (run exactly once, only after the payment is newly recorded) ──
                    long userId;
                    if (existingId is null)
                    {
                        userId = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?, 'student','active')", email, m.GetValueOrDefault("first_name"), m.GetValueOrDefault("last_name"));
                        db.Execute("INSERT INTO student_profiles(user_id,country) VALUES(?,?)", userId, m.GetValueOrDefault("country"));
                        db.Execute("UPDATE payments SET user_id=? WHERE id=?", userId, payId); // backfill the FK now that the user exists
                    }
                    else { userId = existingId.Value; db.Execute("UPDATE users SET status='active', updated_at=datetime('now') WHERE id=?", userId); }

                    if (product == "membership" || product == "bundle")
                        db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,renewal_fee,renewal_cycle,amount_paid,currency) VALUES(?,?, 'active', datetime('now'), datetime('now','+3 year'), 99, '3 years', ?, 'USD')",
                            userId, "Student Membership", MetaNum("final_amount"));
                    {
                        {
                        if (product == "renewal")
                        {
                            var mrow = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", userId);
                            var nowIso = H.IsoNow;
                            var baseD = mrow is not null && H.Str(mrow["expiry_date"]) is { } ex && string.Compare(ex, nowIso, StringComparison.Ordinal) > 0 ? ex : nowIso;
                            var newExp = H.IsoFromMillis(H.JsMillis(baseD) + 365L * 86400_000);
                            if (mrow is not null) db.Execute("UPDATE memberships SET expiry_date=?, status='active' WHERE user_id=?", newExp, userId);
                            else db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date) VALUES(?, 'Student','active',datetime('now'),?)", userId, newExp);
                            log(userId, "membership_renewed", newExp);
                        }
                        if (product == "recert")
                        {
                            var cred = db.QueryOne("SELECT * FROM issued_credentials WHERE user_id=? ORDER BY id DESC", userId);
                            if (cred is not null)
                            {
                                var nowIso = H.IsoNow;
                                var baseD = H.Str(cred["expires_at"]) is { } ex && string.Compare(ex, nowIso, StringComparison.Ordinal) > 0 ? ex : nowIso;
                                var newExp = H.IsoFromMillis(H.JsMillis(baseD) + 3L * 365 * 86400_000);
                                db.Execute("UPDATE issued_credentials SET expires_at=?, status='active' WHERE id=?", newExp, cred["id"]);
                                log(userId, "recertified", newExp);
                            }
                        }
                        var discountCode = m.GetValueOrDefault("discount_code");
                        if (!string.IsNullOrEmpty(discountCode))
                        {
                            var codeRow = db.QueryOne("SELECT id,max_uses FROM discount_codes WHERE code=?", discountCode);
                            if (codeRow is not null)
                            {
                                db.Execute("UPDATE discount_codes SET used_count=used_count+1 WHERE id=? AND (max_uses IS NULL OR used_count<max_uses)", codeRow["id"]);
                                db.Execute(@"INSERT OR IGNORE INTO code_redemptions(code_id,code,user_id,email,payment_id,product_type,amount_before,discount_amount) VALUES(?,?,?,?,?,?,?,?)",
                                    codeRow["id"], discountCode, userId, email, payId, product, MetaNum("standard_amount"), MetaNum("code_amount"));
                            }
                        }
                        if (sess is not null) db.Execute("UPDATE enrollment_sessions SET session_status='paid', last_activity_at=datetime('now') WHERE id=?", sess["id"]);
                        var token = Security.RandomHex(32);
                        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", userId, Security.Sha(token));

                        if (product == "exam" || product == "bundle")
                        {
                            db.Execute("UPDATE payments SET exam_schedule_deadline=datetime('now','+1 year') WHERE reference=?", reference);
                            // Formal one-attempt entitlement tied to this payment (idempotent via unique index).
                            db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,status,valid_until) VALUES(?,?,?, 'available', datetime('now','+1 year'))", userId, payId, product);
                        }
                        log(userId, "account_activated", reference);
                        }
                    }
                  });
                }
            }
            else if (ev.Type is "checkout.session.async_payment_failed" or "payment_intent.payment_failed")
            {
                var failObj = ev.Data.Object as IHasId;
                var id = failObj?.Id;
                db.Execute("INSERT INTO payments(product_type,payment_provider,provider_payment_id,payment_status,payment_date) VALUES('unknown','stripe',?, 'failed', datetime('now'))", id);
                log(null, "payment_failed", ev.Id);
            }
            return J(new { received = true });
        });
    }
}
