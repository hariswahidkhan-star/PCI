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
        ["exam"] = "PCL-AI Certification Exam",
        ["bundle"] = "Membership + Exam Bundle",
        ["renewal"] = "Annual Membership Renewal",
        ["recert"] = "PCL-AI Recertification (3-year cycle)"
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
                // Exam-only purchases require an ACTIVE membership on the account first. A student without
                // one is told to pay the membership fee first — or to choose the membership+exam bundle,
                // which pays both together. Membership / bundle / renewal / recert are exempt (the bundle is
                // itself the "pay both together" path). Enforced here (not just in the browser) so it also
                // covers the public checkout page.
                if (product == "exam")
                {
                    var em = (email ?? "").Trim().ToLowerInvariant();
                    var urow = em.Length > 0 ? db.QueryOne("SELECT id FROM users WHERE email=?", em) : null;
                    var memberActive = urow is not null && db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", H.L(urow["id"])) is not null;
                    if (!memberActive)
                        return Results.Json(new { error = "membership_required", message = "Please pay your membership fee first, or choose the membership + exam bundle to pay both together." }, statusCode: 400);
                }
                // Recertification requires the credential's CPD to be met first (per-certification requirement).
                // Enforced at checkout so a student can never PAY for a recert they aren't yet eligible for —
                // the fulfilment path only extends an already-eligible credential.
                if (product == "recert")
                {
                    var em = (email ?? "").Trim().ToLowerInvariant();
                    var urow = em.Length > 0 ? db.QueryOne("SELECT id FROM users WHERE email=?", em) : null;
                    if (urow is not null && Certs.TryResolve(db, H.GetS(d, "certification_id", "certification", "cert")) is { } rcId
                        && CpdPolicy.ForUserCert(db, H.L(urow["id"]), rcId) is { HasRequirement: true, Met: false } cpd)
                    {
                        var aiPart = cpd.AiRequired > 0 && !cpd.AiMet
                            ? $" This includes {cpd.AiRequired:0.#} AI-currency hours ({cpd.AiApproved:0.#} approved)."
                            : "";
                        return Results.Json(new
                        {
                            error = "cpd_required",
                            message = $"Recertification requires {cpd.Required:0.#} approved CPD hours for this certification cycle. You have {cpd.Approved:0.#} approved so far.{aiPart} Log and get the remaining hours approved, then recertify.",
                            required = cpd.Required, approved = cpd.Approved, ai_required = cpd.AiRequired, ai_approved = cpd.AiApproved,
                        }, statusCode: 400);
                    }
                }
                // Which certification the exam seat is for (id or code; the founding certification when
                // unspecified). An EXPLICIT but unknown value is rejected — never silently converted.
                var certSelIn = Certs.TryResolve(db, H.GetS(d, "certification_id", "certification", "cert"));
                if (certSelIn is null) return Results.Json(new { error = "bad_certification", message = "Unknown certification." }, statusCode: 400);
                var certRow = Certs.ById(db, certSelIn.Value);
                if (certRow is not null && !H.B(certRow["active"])) return Results.Json(new { error = "certification_inactive" }, statusCode: 400);
                // A supplied code is re-validated for THIS product server-side (the browser check is only
                // advisory). If it's invalid or scoped to the other product, reject the checkout rather than
                // silently dropping the discount and charging full price.
                var codeStr = H.GetS(d, "code");
                var codeVal = !string.IsNullOrWhiteSpace(codeStr) ? Public.ValidateCode(db, codeStr, product, email, certSelIn) : new Public.CodeValidation(null, null);
                if (codeVal.Error is not null) return Results.Json(new { error = "code_invalid", message = codeVal.Error }, statusCode: 400);
                var pr = Public.Pricing(db, product, codeVal.Code, certRow);
                // A code with a configured floor cannot push the payable amount below it.
                if (codeVal.Code is not null && Public.MinPayableViolation(codeVal.Code, pr.final) is { } floorMsg)
                    return Results.Json(new { error = "code_invalid", message = floorMsg }, statusCode: 400);
                db.Execute("UPDATE enrollment_sessions SET selected_product=?, pricing_snapshot=?, last_activity_at=datetime('now') WHERE email=? AND session_status='in_progress'",
                    product, JsonSerializer.Serialize(pr), (email ?? "").ToLowerInvariant());

                var label = PRODUCT_LABEL[product];
                if (product is "exam" or "bundle" or "recert" && certRow is not null)
                    label = label.Replace("PCL-AI", H.Str(certRow["code"]) ?? "PCL-AI");
                var options = new SessionCreateOptions
                {
                    Mode = "payment",
                    LineItems = new List<SessionLineItemOptions> {
                        new() { Quantity = 1, PriceData = new SessionLineItemPriceDataOptions {
                            Currency = "usd", UnitAmount = (long)Math.Round(pr.final * 100),
                            ProductData = new SessionLineItemPriceDataProductDataOptions {
                                Name = label, Description = "Access only. Certification is awarded separately under PCI policies." } } } },
                    CustomerEmail = email,
                    Metadata = new Dictionary<string, string> {
                        ["product"] = product, ["plan_label"] = label,
                        ["certification"] = certRow is null ? "PCL-AI" : (H.Str(certRow["code"]) ?? "PCL-AI"),
                        ["first_name"] = H.GetS(d, "first") ?? "", ["last_name"] = H.GetS(d, "last") ?? "",
                        ["country"] = H.GetS(d, "country") ?? "", ["discount_code"] = codeVal.Code is null ? "" : (H.Str(codeVal.Code["code"]) ?? ""),
                        ["final_amount"] = pr.final.ToString(), ["code_amount"] = pr.codeAmount.ToString(),
                        ["standard_amount"] = pr.standard.ToString(), ["default_discount"] = pr.defaultDiscount.ToString() },
                    // Purchases started inside the student portal return to the portal's Billing page,
                    // where the webhook-applied membership/entitlement shows up on reload.
                    SuccessUrl = (H.GetS(d, "portal") is "1" or "true")
                        ? $"{Base}/app/billing?paid={{CHECKOUT_SESSION_ID}}"
                        : $"{Base}/payment-success.html?ref={{CHECKOUT_SESSION_ID}}&product={Uri.EscapeDataString(label)}",
                    CancelUrl = (H.GetS(d, "portal") is "1" or "true")
                        ? $"{Base}/app/billing?cancelled=1"
                        : $"{Base}/payment-failed.html"
                };
                var session = await new SessionService().CreateAsync(options);
                return J(new { url = session.Url });
            }
            catch (Exception e) { Console.Error.WriteLine(e); return Results.Json(new { error = "session_failed" }, statusCode: 500); }
        });

        // ---------- recurring annual dues (Stripe subscription) ----------
        // Activates only when the operator sets STRIPE_MEMBERSHIP_PRICE_ID (a recurring Price). Until then the
        // endpoint reports an honest "not configured" — nothing is faked, matching the platform's capability model.
        static string? DuesPrice() => Environment.GetEnvironmentVariable("STRIPE_MEMBERSHIP_PRICE_ID");

        app.MapPost("/api/me/membership/subscribe", async (HttpRequest req) =>
        {
            if (!stripeReady()) return Stripe503();
            var u = Auth.UserFromReq(req, db); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var price = DuesPrice();
            if (string.IsNullOrWhiteSpace(price))
                return Results.Json(new { error = "dues_not_configured", message = "Recurring annual dues are not enabled yet." }, statusCode: 503);
            // Reuse the member's Stripe customer if we already have one (avoids duplicate customers on re-subscribe).
            var existingCust = H.Str(db.QueryOne("SELECT stripe_customer_id FROM memberships WHERE user_id=? ORDER BY id DESC", u.Id)?["stripe_customer_id"]);
            try
            {
                var opts = new SessionCreateOptions
                {
                    Mode = "subscription",
                    LineItems = new() { new() { Price = price, Quantity = 1 } },
                    ClientReferenceId = u.Id.ToString(),
                    SubscriptionData = new() { Metadata = new() { ["user_id"] = u.Id.ToString() } },
                    SuccessUrl = $"{Base}/app/billing?subscribed=1",
                    CancelUrl = $"{Base}/app/billing?cancelled=1",
                };
                if (!string.IsNullOrEmpty(existingCust)) opts.Customer = existingCust; else opts.CustomerEmail = u.Email;
                var session = await new SessionService().CreateAsync(opts);
                return J(new { url = session.Url });
            }
            catch (Exception e) { Console.Error.WriteLine(e); return Results.Json(new { error = "session_failed" }, statusCode: 500); }
        });

        // Stripe Billing Portal — the member manages/cancels their own subscription and payment method.
        app.MapPost("/api/me/membership/portal", async (HttpRequest req) =>
        {
            if (!stripeReady()) return Stripe503();
            var u = Auth.UserFromReq(req, db); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var cust = H.Str(db.QueryOne("SELECT stripe_customer_id FROM memberships WHERE user_id=? ORDER BY id DESC", u.Id)?["stripe_customer_id"]);
            if (string.IsNullOrEmpty(cust)) return Results.Json(new { error = "no_subscription", message = "You do not have an active subscription to manage." }, statusCode: 400);
            try
            {
                var ps = await new Stripe.BillingPortal.SessionService().CreateAsync(new Stripe.BillingPortal.SessionCreateOptions { Customer = cust, ReturnUrl = $"{Base}/app/billing" });
                return J(new { url = ps.Url });
            }
            catch (Exception e) { Console.Error.WriteLine(e); return Results.Json(new { error = "portal_failed" }, statusCode: 500); }
        });

        // ---------- webhook: the ONLY place access is granted ----------
        app.MapPost("/api/webhook", async (HttpRequest req) =>
        {
            if (!stripeReady()) return Stripe503();
            // Fail CLOSED if the signing secret is unset: ConstructEvent with an empty secret would verify
            // the HMAC against "" — trivially forgeable — so refuse rather than trust the payload. (The
            // production boot check also errors on a missing secret; this is defence in depth for dev.)
            var webhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");
            if (string.IsNullOrEmpty(webhookSecret)) return Results.Json(new { error = "webhook_not_configured" }, statusCode: 503);
            string payload;
            using (var reader = new StreamReader(req.Body)) payload = await reader.ReadToEndAsync();
            Event ev;
            try { ev = EventUtility.ConstructEvent(payload, req.Headers["stripe-signature"].ToString(), webhookSecret); }
            catch { return Results.Text("Webhook signature verification failed", "text/plain", statusCode: 400); }

            // Recurring annual-dues subscription set-up: store the Stripe customer + subscription on the
            // member and activate their membership. Kept separate from the one-off settlement below.
            if (ev.Type == "checkout.session.completed" && (ev.Data.Object as Session)?.Mode == "subscription")
            {
                var s = ev.Data.Object as Session;
                var uid = long.TryParse(s?.ClientReferenceId, out var cid) ? cid
                    : long.TryParse(s?.Metadata?.GetValueOrDefault("user_id", ""), out var mid) ? mid : 0;
                var email = (s?.CustomerDetails?.Email ?? s?.CustomerEmail ?? "").ToLowerInvariant();
                if (uid == 0 && !string.IsNullOrEmpty(email)) uid = H.L(db.QueryOne("SELECT id FROM users WHERE email=?", email)?["id"]);
                if (uid != 0)
                {
                    var m2 = db.QueryOne("SELECT id FROM memberships WHERE user_id=? ORDER BY id DESC", uid);
                    if (m2 is null)
                        db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,stripe_customer_id,stripe_subscription_id,subscription_status,grade) VALUES(?, 'Annual Membership','active',datetime('now'),datetime('now','+1 year'),?,?, 'active','associate')",
                            uid, s!.CustomerId, s.SubscriptionId);
                    else
                        db.Execute("UPDATE memberships SET status='active', stripe_customer_id=?, stripe_subscription_id=?, subscription_status='active', cancel_at_period_end=0, expiry_date=CASE WHEN expiry_date IS NULL OR expiry_date<datetime('now','+1 year') THEN datetime('now','+1 year') ELSE expiry_date END WHERE id=?",
                            s!.CustomerId, s.SubscriptionId, m2["id"]);
                    log(uid, "dues_subscribed", s.SubscriptionId);
                }
                return Results.Ok();
            }

            if (ev.Type == "checkout.session.completed")
            {
                var s = ev.Data.Object as Session;
                var m = s?.Metadata ?? new();
                var email = (s?.CustomerDetails?.Email ?? s?.CustomerEmail ?? "").ToLowerInvariant();
                if (!string.IsNullOrEmpty(email))
                {
                  // Fully atomic settlement: user, membership, payment, redemption, tokens all commit
                  // together or roll back together. Combined with INSERT OR IGNORE this is idempotent.
                  long? welcomeUserId = null; string? welcomeEmail = null, welcomeFirst = null, welcomeLink = null, welcomeBase = null, welcomeRef = null;
                  long? provisionCertuvoUserId = null;
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

                    // First-party analytics: revenue + activation events (server-to-server — no visitor context).
                    var finalAmount = MetaNum("final_amount") != 0 ? MetaNum("final_amount") : amountTotal;
                    PCI.Backend.Core.Analytics.Track(db, null, "purchase_completed", userId, finalAmount, "USD", product);
                    // ERP / integrations outbox (Phase 9): a canonical revenue event for accounting/ERP sync.
                    PCI.Backend.Core.Integrations.Emit(db, "payment.recorded", "payment", payId, new
                    {
                        payment_id = payId, user_id = userId, email, amount = finalAmount, currency = "USD",
                        product, product_type = product, occurred_at = H.IsoNow,
                    });
                    if (product == "membership" || product == "bundle")
                    {
                        db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,renewal_fee,renewal_cycle,amount_paid,currency) VALUES(?,?, 'active', datetime('now'), datetime('now','+3 year'), 99, '3 years', ?, 'USD')",
                            userId, "Student Membership", MetaNum("final_amount"));
                        PCI.Backend.Core.Analytics.Track(db, null, "membership_activated", userId, null, null, product);
                        PCI.Backend.Core.Integrations.Emit(db, "membership.activated", "user", userId, new
                        {
                            user_id = userId, email, membership_type = "Student Membership", occurred_at = H.IsoNow,
                        });
                        // Auto-provision the student's Certuvo external practice account AFTER the transaction
                        // commits (see the setup-email note below). Provision() is a sync-over-async blocking
                        // HTTP call whose continuation re-enters the shared DB connection on a threadpool thread;
                        // running it here — while db.Transaction holds the connection's single reentrant lock —
                        // deadlocks (the continuation waits for a lock the blocked webhook thread owns), wedging
                        // every DB call app-wide. Defer it to post-commit where no lock is held.
                        provisionCertuvoUserId = userId;
                    }
                    {
                        {
                        if (product == "renewal")
                        {
                            var mrow = db.QueryOne("SELECT * FROM memberships WHERE user_id=?", userId);
                            var nowIso = H.IsoNow;
                            var baseD = mrow is not null && H.Str(mrow["expiry_date"]) is { } ex && H.After(ex, nowIso) ? ex : nowIso;
                            // Membership is a 3-YEAR term (matches the initial grant above and renewal_cycle
                            // = '3 years'); a renewal must extend by 3 years, not 1, or it silently under-grants.
                            var newExp = H.IsoFromMillis(H.JsMillis(baseD) + 3L * 365L * 86400_000);
                            if (mrow is not null) db.Execute("UPDATE memberships SET expiry_date=?, status='active' WHERE user_id=?", newExp, userId);
                            else db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date) VALUES(?, 'Student','active',datetime('now'),?)", userId, newExp);
                            log(userId, "membership_renewed", newExp);
                        }
                        if (product == "recert")
                        {
                            // Extend the credential for the certification that was actually paid to recertify
                            // (falls back to the most recent credential when the checkout carried no cert).
                            var recertSel = m.GetValueOrDefault("certification");
                            // Never resurrect a REVOKED credential: recertification may extend an active or
                            // lapsed credential, but a credential an admin revoked for misconduct must stay
                            // revoked — paying for recert must not reactivate it.
                            var cred = string.IsNullOrWhiteSpace(recertSel)
                                ? db.QueryOne("SELECT * FROM issued_credentials WHERE user_id=? AND status!='revoked' ORDER BY id DESC", userId)
                                : db.QueryOne("SELECT * FROM issued_credentials WHERE user_id=? AND status!='revoked' AND COALESCE(certification_id,1)=? ORDER BY id DESC", userId, Certs.Resolve(db, recertSel));
                            if (cred is not null)
                            {
                                var nowIso = H.IsoNow;
                                var baseD = H.Str(cred["expires_at"]) is { } ex && H.After(ex, nowIso) ? ex : nowIso;
                                // Extend by the CERTIFICATION's own cycle (expiry_years), matching issuance —
                                // a 2-year credential must not silently gain a 3-year recert extension.
                                var recertCert = Certs.ById(db, cred["certification_id"] is null ? Certs.DefaultId : H.L(cred["certification_id"]));
                                var years = recertCert is null ? 3 : Certs.ExpiryYears(recertCert);
                                var newExp = H.IsoFromMillis(H.JsMillis(baseD) + years * 365L * 86400_000);
                                db.Execute("UPDATE issued_credentials SET expires_at=?, status='active' WHERE id=?", newExp, cred["id"]);
                                log(userId, "recertified", newExp);
                            }
                        }
                        var discountCode = m.GetValueOrDefault("discount_code");
                        if (!string.IsNullOrEmpty(discountCode))
                        {
                            var codeRow = db.QueryOne("SELECT id,max_uses,partner_id FROM discount_codes WHERE code=?", discountCode);
                            if (codeRow is not null)
                            {
                                db.Execute("UPDATE discount_codes SET used_count=used_count+1 WHERE id=? AND (max_uses IS NULL OR used_count<max_uses)", codeRow["id"]);
                                db.Execute(@"INSERT OR IGNORE INTO code_redemptions(code_id,code,user_id,email,payment_id,product_type,amount_before,discount_amount) VALUES(?,?,?,?,?,?,?,?)",
                                    codeRow["id"], discountCode, userId, email, payId, product, MetaNum("standard_amount"), MetaNum("code_amount"));
                                try { FraudChecks.OnRedemption(db, H.L(codeRow["id"]), discountCode!, email, codeRow["partner_id"] is null ? null : H.L(codeRow["partner_id"])); } catch { }
                            }
                        }
                        if (sess is not null) db.Execute("UPDATE enrollment_sessions SET session_status='paid', last_activity_at=datetime('now') WHERE id=?", sess["id"]);
                        var token = Security.RandomHex(32);
                        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+7 day'))", userId, Security.Sha(token));
                        // Deliver the setup link — without this email the customer has paid but can
                        // never set a password or reach the student panel.
                        // Defer the email until AFTER the transaction commits. SMTP can block for the .NET
                        // default 100s and the single shared DB connection holds its lock for the whole
                        // transaction body, so sending in-transaction would freeze every DB call app-wide.
                        var mailBase = Mailer.BaseUrl(req);
                        welcomeUserId = userId; welcomeEmail = email; welcomeFirst = m.GetValueOrDefault("first_name"); welcomeRef = reference;
                        welcomeLink = Mailer.SetupLink(mailBase, token); welcomeBase = mailBase;

                        if (product == "exam" || product == "bundle")
                        {
                            db.Execute("UPDATE payments SET exam_schedule_deadline=datetime('now','+1 year') WHERE reference=?", reference);
                            // Formal one-attempt entitlement tied to this payment (idempotent via unique
                            // index) and to the PURCHASED certification (metadata code; PCP-AI default).
                            var entCertId = Certs.Resolve(db, m.GetValueOrDefault("certification"));
                            db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) VALUES(?,?,?,?, 'available', datetime('now','+1 year'))", userId, payId, product, entCertId);
                            // Create the Exam Authorization (configurable window + attempt policy). Recomputes
                            // the deadline from the resolved window and writes it through, so the effective
                            // period is operator-configured, not the hardcoded +1 year above. Best-effort.
                            try { ExamAuthorization.EnsureForPayment(db, payId); } catch { }
                        }
                        log(userId, "account_activated", reference);
                        }
                    }
                  });
                  // Deliver the setup email outside the transaction so a slow SMTP server never holds the DB lock.
                  if (welcomeEmail is not null)
                      try { Mailer.SendWelcome(db, welcomeUserId!.Value, welcomeEmail, welcomeFirst, welcomeLink!, welcomeBase!); }
                      catch (Exception mex) { log(welcomeUserId, "welcome_email_failed", mex.Message); }
                  // Provision Certuvo outside the transaction: this is a blocking outbound HTTP call whose async
                  // continuation re-enters the shared DB connection, so running it here (no lock held) avoids the
                  // deadlock that an in-transaction call would cause. Best-effort — the retry worker recovers failures.
                  if (provisionCertuvoUserId is { } certuvoUid)
                      try { PCI.Backend.Core.CertuvoLink.Provision(db, PCI.Backend.Core.CertuvoLink.Http, certuvoUid).GetAwaiter().GetResult(); }
                      catch (Exception cex) { log(provisionCertuvoUserId, "certuvo_provision_failed", cex.Message); }
                  // Alert the notification recipients about the completed enrolment/payment.
                  try {
                      var eName = System.Net.WebUtility.HtmlEncode(welcomeFirst ?? "");
                      var eMail = System.Net.WebUtility.HtmlEncode(welcomeEmail ?? "");
                      Notify.Alert(db, "enrollment", $"New PCI enrolment — {welcomeRef}",
                          $"<p>A payment has completed and an enrolment is confirmed.</p>" +
                          $"<p><strong>Reference:</strong> {System.Net.WebUtility.HtmlEncode(welcomeRef ?? "")}<br/>" +
                          $"<strong>Name:</strong> {eName}<br/><strong>Email:</strong> {eMail}</p>", "payment", null);
                  } catch { /* alerts never break the webhook */ }
                }
            }
            else if (ev.Type is "checkout.session.async_payment_failed" or "payment_intent.payment_failed")
            {
                var failObj = ev.Data.Object as IHasId;
                var id = failObj?.Id;
                // Store failed attempts under a distinct key so a later SUCCESS on the same Stripe PaymentIntent
                // (card retry) still inserts its paid row and activates the account, and so a repeated failure
                // event can't violate the provider_payment_id unique index (INSERT OR IGNORE swallows the dup).
                db.Execute("INSERT OR IGNORE INTO payments(product_type,payment_provider,provider_payment_id,payment_status,payment_date) VALUES('unknown','stripe',?, 'failed', datetime('now'))", "failed:" + id);
                log(null, "payment_failed", ev.Id);
            }
            else if (ev.Type is "charge.refunded" or "charge.dispute.created" or "charge.dispute.funds_withdrawn")
            {
                // Money returned → revoke the access it bought. Resolve the payment by its Stripe
                // PaymentIntent, flip it to refunded/reversed, and deactivate any UNUSED entitlement or
                // membership it granted. A credential already earned by a passed exam, and a consumed
                // entitlement, are left intact — the sitting genuinely happened. Idempotent.
                // A dispute event carries a Stripe.Dispute, not a Charge — cast to the right concrete
                // type or the PaymentIntent is null and the whole revocation is silently skipped.
                var ch = ev.Data.Object as Charge;
                var pi = ch?.PaymentIntentId ?? (ev.Data.Object as Dispute)?.PaymentIntentId;
                var reversed = ev.Type != "charge.refunded" ? "reversed" : "refunded";
                // A PARTIAL refund (amount_refunded < amount) leaves the purchase substantially paid —
                // don't revoke access. Full refunds (ch.Refunded) and disputes/chargebacks do revoke.
                if (ev.Type == "charge.refunded" && ch is not null && ch.Refunded != true)
                {
                    log(0, "partial_refund_ignored", $"{ev.Id} pi={pi}");
                    pi = null;
                }
                if (!string.IsNullOrEmpty(pi))
                    db.Transaction(() =>
                    {
                        var pay = db.QueryOne("SELECT id,user_id,product_type FROM payments WHERE provider_payment_id=? AND payment_status='paid'", pi);
                        if (pay is null) return;
                        var payId = pay["id"]; var uid = pay["user_id"];
                        db.Execute("UPDATE payments SET payment_status=? WHERE id=?", reversed, payId);
                        db.Execute("UPDATE exam_entitlements SET status='revoked' WHERE payment_id=? AND status IN ('available','booked')", payId);
                        db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE payment_id=? AND status='scheduled'", payId);
                        // membership/bundle purchases: lapse the membership this payment activated
                        if (H.Str(pay["product_type"]) is "membership" or "bundle" or "renewal")
                            db.Execute("UPDATE memberships SET status='expired', expiry_date=datetime('now') WHERE user_id=? AND status='active'", uid);
                        log(H.Ln(uid), "payment_" + reversed, $"{ev.Id} pi={pi}");
                    });
            }
            // Recurring dues renewal: a successful RECURRING invoice extends the term by a year. The initial
            // invoice (subscription_create) is skipped — that term is set at checkout.session.completed.
            else if (ev.Type == "invoice.paid")
            {
                var inv = ev.Data.Object as Stripe.Invoice;
                if (inv is not null && inv.BillingReason == "subscription_cycle" && !string.IsNullOrEmpty(inv.CustomerId))
                {
                    var mem = db.QueryOne("SELECT id,user_id,expiry_date FROM memberships WHERE stripe_customer_id=? ORDER BY id DESC", inv.CustomerId);
                    if (mem is not null)
                    {
                        var nowIso = H.IsoNow;
                        var baseD = H.Str(mem["expiry_date"]) is { } ex && H.After(ex, nowIso) ? ex : nowIso;
                        var newExp = H.IsoFromMillis(H.JsMillis(baseD) + 365L * 86400_000L);
                        db.Execute("UPDATE memberships SET status='active', subscription_status='active', expiry_date=? WHERE id=?", newExp, mem["id"]);
                        log(H.Ln(mem["user_id"]), "dues_renewed", inv.CustomerId);
                    }
                }
            }
            // Subscription state changes (cancellation scheduled, past_due, canceled) mirror into the member's row.
            else if (ev.Type is "customer.subscription.updated" or "customer.subscription.deleted")
            {
                var sub = ev.Data.Object as Stripe.Subscription;
                if (sub is not null && !string.IsNullOrEmpty(sub.CustomerId))
                {
                    var mem = db.QueryOne("SELECT id,user_id FROM memberships WHERE stripe_customer_id=? ORDER BY id DESC", sub.CustomerId);
                    if (mem is not null)
                    {
                        var status = ev.Type == "customer.subscription.deleted" ? "canceled" : sub.Status;
                        db.Execute("UPDATE memberships SET subscription_status=?, cancel_at_period_end=? WHERE id=?", status, sub.CancelAtPeriodEnd ? 1 : 0, mem["id"]);
                        log(H.Ln(mem["user_id"]), "dues_sub_" + (ev.Type == "customer.subscription.deleted" ? "canceled" : "updated"), status);
                    }
                }
            }
            return J(new { received = true });
        });
    }
}
