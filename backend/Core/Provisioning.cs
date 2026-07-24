using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Offline settlement — grant a member exactly what a real (Stripe) payment would, without a card charge.
/// Used by the admin "mark as paid / waive fee" controls and by one-click test users. Mirrors the webhook
/// settlement: a payment row, plus the membership and/or exam entitlement, plus the Certuvo hand-off on
/// a membership — so every downstream gate (booking, launch, one-attempt-per-entitlement) applies unchanged.
///
/// A full waiver is recorded with payment_status='waived' and final_amount 0 — it is never disguised as a
/// paid transaction. Waived rows grant access exactly like paid rows but are excluded from revenue.
/// </summary>
public static class Settlement
{
    /// <summary>Structured evidence for a manual settlement (all optional).</summary>
    public sealed class Meta
    {
        public string? Method;            // bank_transfer | cheque | invoice | gateway | other
        public string? BankReference;
        public string? GatewayReference;
        public string? ReceiptNo;
        public string? Note;
        public long? RecordedBy;          // admin_users.id
        public double? OriginalAmount;    // list price at the time (defaults to the pricing rules)
        public string? PaidAt;            // ISO date the money actually arrived (defaults to now)
    }

    /// <summary>The current list price for a product (pricing_rules), used to record what a waiver forgave.</summary>
    public static double ListPrice(Db db, string product)
    {
        try { return PCI.Backend.Endpoints.Public.Pricing(db, product, null).final; } catch { return 0; }
    }

    /// <summary>Grant a product to a user. <paramref name="product"/> is membership | exam | bundle.
    /// <paramref name="amount"/> is the money actually received; 0 with provider 'admin_waiver' records a
    /// waived (not paid) settlement. Returns the created payment id.</summary>
    public static long Grant(Db db, long userId, string? email, string product, long certId, double amount, string reference, string provider, Meta? meta = null)
    {
        product = (product ?? "membership").Trim().ToLowerInvariant();
        var isExam = product is "exam" or "bundle";
        // Honorary grants are recorded as waived (non-revenue) settlements, exactly like fee waivers — an
        // honorary member's access is genuine but no money changed hands, so it is never disguised as paid.
        var waived = amount <= 0 && provider is "admin_waiver" or "admin_honorary";
        var status = waived ? "waived" : "paid";
        var original = meta?.OriginalAmount ?? (waived ? ListPrice(db, product) : amount);
        var waivedAmount = waived ? original : (original > amount ? original - amount : 0);

        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference,exam_schedule_deadline,method,bank_reference,gateway_reference,receipt_no,note,recorded_by,waived_amount)
            VALUES(?,?,?,?, 'USD', ?, ?, COALESCE(?, datetime('now')), ?, CASE WHEN ?=1 THEN datetime('now','+1 year') ELSE NULL END,?,?,?,?,?,?,?)",
            userId, product, original, amount, provider, status, meta?.PaidAt, reference, isExam ? 1 : 0,
            meta?.Method, meta?.BankReference, meta?.GatewayReference, meta?.ReceiptNo, meta?.Note, meta?.RecordedBy,
            waivedAmount > 0 ? waivedAmount : null);

        EnsureDownstream(db, payId);

        try { Integrations.Emit(db, "payment.recorded", "payment", payId, new { payment_id = payId, user_id = userId, email, amount, currency = "USD", product, product_type = product, status, occurred_at = H.IsoNow }); } catch { }

        // Emailed payment receipt to the payer — paid settlements only (a waiver is not a receipt). Fires
        // for every settlement path (Stripe webhook, admin mark-paid). Best-effort; admin-toggleable via
        // notify_payment_receipt_enabled. The receipt is also downloadable in-portal (/api/me/invoices).
        if (status == "paid" && Notify.Enabled(db, "payment_receipt"))
            try
            {
                var payer = db.QueryOne("SELECT email,first_name FROM users WHERE id=?", userId);
                var toEmail = !string.IsNullOrWhiteSpace(email) ? email : H.Str(payer?["email"]);
                if (!string.IsNullOrWhiteSpace(toEmail))
                {
                    var first = H.Str(payer?["first_name"]);
                    var html = Mailer.Template("payment-confirmation", new()
                    {
                        ["FIRST_NAME"] = string.IsNullOrWhiteSpace(first) ? "there" : first!,
                        ["AMOUNT"] = "USD " + amount.ToString("0.##"),
                        ["DATE"] = meta?.PaidAt ?? H.IsoNow,
                        ["PRODUCT"] = char.ToUpperInvariant(product[0]) + product[1..],
                        ["REFERENCE"] = reference,
                    });
                    Mailer.Send(db, userId, toEmail!, "payment_receipt", "Your PCI payment receipt", html);
                }
            }
            catch { }
        // Additional channels (WhatsApp + in-app) via the Communications Centre, governed by the
        // payment.successful trigger's per-channel toggles. Email is handled above (and mirrored to
        // history), so skip email here; dedup on the payment id prevents a duplicate provider callback
        // from double-firing.
        if (status == "paid")
            try
            {
                var payer2 = db.QueryOne("SELECT email,first_name FROM users WHERE id=?", userId);
                Comms.Fire(db, "payment.successful", userId, H.Str(payer2?["email"]) ?? email, null,
                    new Dictionary<string, string?> { ["student_name"] = H.Str(payer2?["first_name"]) ?? "there", ["payment_amount"] = "USD " + amount.ToString("0.##"), ["invoice_number"] = reference, ["portal_link"] = "/app/billing" },
                    "Payment received", $"<p>We've received your payment of USD {amount:0.##} (reference {reference}). Thank you.</p>",
                    dedupSuffix: payId.ToString(), skipEmail: true);
            }
            catch { }
        return payId;
    }

    /// <summary>Idempotently (re)apply every downstream effect a settled payment should have produced:
    /// membership activation, exam entitlement, schedule deadline, Certuvo provisioning. Safe to call any
    /// number of times — used by the reconciliation "reprocess" action to recover missed/failed callbacks
    /// without ever double-granting. Returns what was ensured vs already present.</summary>
    public static object EnsureDownstream(Db db, long payId)
    {
        var p = db.QueryOne("SELECT * FROM payments WHERE id=?", payId);
        if (p is null) return new { ok = false, error = "payment_not_found" };
        var status = H.Str(p["payment_status"]);
        if (status is not ("paid" or "waived")) return new { ok = false, error = "payment_not_settled", status };
        var userId = H.L(p["user_id"]);
        if (userId == 0) return new { ok = false, error = "payment_has_no_user" };
        var product = (H.Str(p["product_type"]) ?? "").ToLowerInvariant();
        var isExam = product is "exam" or "bundle";
        var isMembership = product is "membership" or "bundle";
        var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", userId)?["email"]);
        var did = new List<string>();

        if (isMembership)
        {
            var m = db.QueryOne("SELECT id,status FROM memberships WHERE user_id=?", userId);
            if (m is null)
            {
                db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,renewal_fee,renewal_cycle,amount_paid,currency) VALUES(?, 'Student Membership','active',datetime('now'),datetime('now','+3 year'),99,'3 years',?, 'USD')", userId, H.D(p["final_amount"]));
                did.Add("membership_created");
            }
            else if (H.Str(m["status"]) != "active")
            {
                db.Execute("UPDATE memberships SET status='active', expiry_date=datetime('now','+3 year') WHERE user_id=?", userId);
                did.Add("membership_activated");
            }
            if (did.Count > 0)
            {
                try { Integrations.Emit(db, "membership.activated", "user", userId, new { user_id = userId, email, membership_type = "Student Membership", occurred_at = H.IsoNow }); } catch { }
                // Multi-channel member welcome via the Communications Centre; deduped per payment so the
                // idempotent reprocess path can't re-notify.
                try
                {
                    var mu = db.QueryOne("SELECT email,first_name FROM users WHERE id=?", userId);
                    Comms.Fire(db, "membership.activated", userId, H.Str(mu?["email"]) ?? email, null,
                        new Dictionary<string, string?> { ["student_name"] = H.Str(mu?["first_name"]) ?? "there", ["portal_link"] = "/app/" },
                        "Your PCI membership is active", "<p>Your PCI membership is now active — welcome. Sign in to your portal to make the most of it.</p>",
                        dedupSuffix: $"member:{payId}");
                }
                catch { }
            }
        }
        if (isExam)
        {
            var had = db.QueryOne("SELECT id FROM exam_entitlements WHERE payment_id=?", payId) is not null;
            db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) VALUES(?,?, 'exam', COALESCE((SELECT certification_id FROM exam_entitlements WHERE payment_id=?),1), 'available', datetime('now','+1 year'))", userId, payId, payId);
            if (!had && db.QueryOne("SELECT id FROM exam_entitlements WHERE payment_id=?", payId) is not null) did.Add("entitlement_created");
            if (p["exam_schedule_deadline"] is null)
            {
                db.Execute("UPDATE payments SET exam_schedule_deadline=datetime('now','+1 year') WHERE id=?", payId);
                did.Add("schedule_deadline_set");
            }
            // Create the Exam Authorization (configurable scheduling window + attempt policy + status). This
            // recomputes the deadline from the resolved window and writes it through, so the effective period
            // is operator-configured, not the hardcoded +1 year above. Idempotent; best-effort.
            try { if (ExamAuthorization.EnsureForPayment(db, payId) > 0) did.Add("authorization_ensured"); } catch { }
        }
        // Certuvo provisioning trigger. Always on a membership/bundle payment; also on an exam payment when
        // the operator has set the eligibility rule to "membership_or_exam" (so paying the exam fee alone
        // shares Certuvo access). The Provision call re-checks Eligible(), so this only ever fires when the
        // student actually qualifies under the configured rule.
        var certuvoOnExam = Settings.Str(db, "certuvo_requires", "membership") == "membership_or_exam";
        if ((isMembership || (isExam && certuvoOnExam)) && CertuvoLink.Enabled(db))
        {
            var before = H.Str(db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId)?["status"]);
            if (before != "active")
            {
                try { CertuvoLink.Provision(db, CertuvoLink.Http, userId).GetAwaiter().GetResult(); } catch { }
                var after = H.Str(db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId)?["status"]);
                if (after == "active" && before != "active") did.Add("certuvo_provisioned");
            }
        }
        // Partner commission (Phase 1): a settled, partner-attributed payment earns exactly one immutable
        // commission transaction with the rate snapshotted now. Idempotent via a UNIQUE dedupe_key, so the
        // Stripe webhook, an admin reprocess and the backfill can all reach here safely. Best-effort — a
        // ledger hiccup must never block the student's entitlement.
        try { if (PartnerCommission.EnsureForPayment(db, payId) > 0) did.Add("partner_commission_recorded"); } catch { }
        return new { ok = true, payment_id = payId, product, status, ensured = did, already_complete = did.Count == 0 };
    }

    /// <summary>Grant an entitlement with a chosen certification (used by exam waivers/settlements that
    /// target a specific credential — the generic path defaults to certification 1).</summary>
    public static void RetargetEntitlement(Db db, long payId, long certId)
    {
        if (certId > 0)
        {
            db.Execute("UPDATE exam_entitlements SET certification_id=? WHERE payment_id=?", certId, payId);
            try { ExamAuthorization.SyncCert(db, payId, certId); } catch { }   // keep the authorization's cert + window in step
        }
    }

    /// <summary>Reverse a manual settlement (offline payment or waiver): the payment is marked refunded with
    /// a mandatory reason, the unconsumed entitlement is revoked, scheduled bookings cancelled, and the
    /// membership is lapsed unless another live settlement still supports it. Stripe money is refunded at
    /// the gateway (its webhook drives that path); this handles admin-recorded rows only.</summary>
    public static object Reverse(Db db, long payId, string reason, long adminId)
    {
        var p = db.QueryOne("SELECT * FROM payments WHERE id=?", payId);
        if (p is null) return new { ok = false, error = "payment_not_found" };
        var provider = H.Str(p["payment_provider"]) ?? "";
        var status = H.Str(p["payment_status"]);
        if (provider == "stripe") return new { ok = false, error = "stripe_payment", detail = "Refund card payments from the Stripe dashboard — the webhook applies the reversal here." };
        if (status is not ("paid" or "waived")) return new { ok = false, error = "not_reversible", status };

        var userId = H.L(p["user_id"]);
        var product = (H.Str(p["product_type"]) ?? "").ToLowerInvariant();
        db.Execute("UPDATE payments SET payment_status='refunded', reversed_at=datetime('now'), reversed_by=?, reversal_reason=? WHERE id=?", adminId, reason, payId);
        db.Execute("UPDATE exam_entitlements SET status='revoked' WHERE payment_id=? AND status IN ('available','booked')", payId);
        db.Execute("UPDATE exam_bookings SET status='cancelled', updated_at=datetime('now') WHERE payment_id=? AND status='scheduled'", payId);
        db.Execute("UPDATE fee_waivers SET status='revoked' WHERE payment_id=?", payId);

        var membershipLapsed = false;
        if (product is "membership" or "bundle")
        {
            var other = db.QueryOne("SELECT id FROM payments WHERE user_id=? AND id<>? AND payment_status IN ('paid','waived') AND product_type IN ('membership','bundle')", userId, payId);
            if (other is null)
            {
                db.Execute("UPDATE memberships SET status='expired' WHERE user_id=? AND status='active'", userId);
                membershipLapsed = true;
            }
        }
        // Any partner commission this settlement earned is clawed back as a linked reversal — the original
        // transaction is never edited, so the ledger keeps showing what was earned and what was returned.
        try { PartnerCommissionReversal.EnsureForPayment(db, payId, "reversal"); } catch { }
        try { db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Account', 'A payment record was reversed', ?)", userId,
            $"A {product} settlement on your account was reversed by our team. If you believe this is an error, contact support."); } catch { }
        return new { ok = true, payment_id = payId, previous_status = status, new_status = "refunded", membership_lapsed = membershipLapsed };
    }

    /// <summary>Time-based membership expiry: flip lapsed active memberships to 'expired'. The status column
    /// stays the single source every gate reads; this sweep is what advances it once expiry_date passes —
    /// run daily by RetentionService and on demand via POST /api/admin/ops/sweeps/run. Returns rows flipped.</summary>
    public static int ExpireDueMemberships(Db db) =>
        db.Execute("UPDATE memberships SET status='expired' WHERE status='active' AND expiry_date IS NOT NULL AND expiry_date<=datetime('now')");
}

/// <summary>
/// Certuvo external practice-platform integration. Certuvo hosts the practice experience on its own platform;
/// when a student's membership is settled (paid, marked paid, waived, or sponsored), PCI provisions them a
/// Certuvo account through Certuvo's API and surfaces the credentials + login link in the student panel.
/// Fully configurable (Admin → Integrations → Certuvo). Idempotent (one account per member, stable
/// idempotency key sent on every attempt), retried automatically with backoff on failure, and never blocks
/// membership activation. Accounts can be suspended/revoked/re-sent by authorised admins.
/// </summary>
public static class CertuvoLink
{
    public static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(20));

    public static bool Enabled(Db db) => Settings.Bool(db, "certuvo_enabled", false);

    /// <summary>Decrypt the Certuvo API key (envelope-encrypted at rest; plaintext legacy tolerated).</summary>
    public static string ApiKey(Db db)
    {
        var raw = Settings.Str(db, "certuvo_api_key", "");
        return Security.DecryptSecret(raw) ?? raw;
    }

    /// <summary>Decrypt the Certuvo webhook secret (envelope-encrypted at rest).</summary>
    public static string WebhookSecret(Db db)
    {
        var raw = Settings.Str(db, "certuvo_webhook_secret", "");
        return Security.DecryptSecret(raw) ?? raw;
    }

    /// <summary>Configurable business rule: does Certuvo access require only an active membership
    /// (default), or membership plus a certification enrolment (exam entitlement)?</summary>
    public static bool Eligible(Db db, long userId)
    {
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        bool HasExam() => db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        // Eligibility rule (operator-configured):
        //   membership              → active membership required (default)
        //   membership_and_enrolment→ active membership AND a paid exam entitlement
        //   membership_or_exam      → EITHER an active membership OR a paid exam entitlement qualifies
        return Settings.Str(db, "certuvo_requires", "membership") switch
        {
            "membership_and_enrolment" => hasMembership && HasExam(),
            "membership_or_exam" => hasMembership || HasExam(),
            _ => hasMembership,
        };
    }

    static int RetryMax(Db db) => (int)Settings.Num(db, "certuvo_retry_max", 8);

    /// <summary>Generate the next globally-unique, PCI-identifiable Certuvo username. Format is configurable
    /// via `certuvo_username_prefix` (default "PCI") and a monotonic counter `certuvo_username_seq`, yielding
    /// e.g. PCI-2026-000001. Independent of the student's email and immutable once stored. Collisions (from a
    /// reused DB, or a manual regenerate) bump the counter until a free value is found.</summary>
    public static string NextUsername(Db db, long userId)
    {
        var prefix = Settings.Str(db, "certuvo_username_prefix", "PCI").Trim();
        if (prefix.Length == 0) prefix = "PCI";
        var year = H.IsoNow.Length >= 4 ? H.IsoNow[..4] : "2026";
        for (var attempt = 0; attempt < 50; attempt++)
        {
            long seq;
            lock (_seqGate)
            {
                seq = (long)Settings.Num(db, "certuvo_username_seq", 0) + 1;
                Settings.Put(db, "certuvo_username_seq", seq.ToString());
            }
            var candidate = $"{prefix}-{year}-{seq:D6}";
            if (db.QueryOne("SELECT id FROM certuvo_accounts WHERE username=?", candidate) is null) return candidate;
        }
        // Extremely unlikely fallback: guarantee uniqueness off the immutable user id.
        return $"{prefix}-{year}-U{userId:D6}";
    }
    static readonly object _seqGate = new();

    /// <summary>Coarse member category for admin visibility (paid / waived / sponsored / complimentary /
    /// honorary / test). Every category is eligible for Certuvo the moment its membership is active — this is
    /// only a label, never a gate.</summary>
    public static string DetectMemberType(Db db, long userId)
    {
        if (H.L(db.QueryOne("SELECT is_test FROM users WHERE id=?", userId)?["is_test"]) == 1) return "test";
        if (db.QueryOne("SELECT id FROM honorary_awards WHERE user_id=? AND status='active'", userId) is not null) return "honorary";
        var pay = db.QueryOne(@"SELECT payment_provider,final_amount,payment_status,discount_code FROM payments
            WHERE user_id=? AND product_type IN ('membership','bundle','renewal') ORDER BY id DESC LIMIT 1", userId);
        if (pay is null) return "member";
        var provider = H.Str(pay["payment_provider"]) ?? "";
        var status = H.Str(pay["payment_status"]) ?? "";
        var amount = H.D(pay["final_amount"]);
        if (status == "waived" || provider == "admin_waiver") return "waived";
        if (!string.IsNullOrEmpty(H.Str(pay["discount_code"])) && db.QueryOne("SELECT dc.partner_id FROM code_redemptions cr JOIN discount_codes dc ON dc.id=cr.code_id WHERE cr.user_id=? AND dc.partner_id IS NOT NULL LIMIT 1", userId) is not null) return "sponsored";
        if (amount <= 0) return "complimentary";
        return "paid";
    }

    /// <summary>Admin action: assign a brand-new PCI username and re-push it to Certuvo. The old username is
    /// discarded (regeneration is the only way it ever changes). Returns the new username.</summary>
    public static async Task<string> RegenerateUsername(Db db, HttpClient http, long userId)
    {
        var fresh = NextUsername(db, userId);
        db.Execute("UPDATE certuvo_accounts SET username=?, external_id=NULL, status='pending', last_error=NULL, retry_count=0, next_retry_at=NULL, username_regenerated_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?", fresh, userId);
        await Provision(db, http, userId, reactivate: true);
        return fresh;
    }

    /// <summary>Admin/self action: mint a fresh temporary password, re-push it to Certuvo, and re-send the
    /// access instructions. The new secret is stored encrypted; the old one is replaced.</summary>
    public static async Task<bool> NewTempPassword(Db db, HttpClient http, long userId)
    {
        var pw = Security.GenPassword((int)Settings.Num(db, "certuvo_password_length", 14));
        db.Execute("UPDATE certuvo_accounts SET secret=?, must_change_password=1, password_reset_at=datetime('now'), status='pending', last_error=NULL, retry_count=0, next_retry_at=NULL, updated_at=datetime('now') WHERE user_id=?",
            Security.EncryptSecret(pw), userId);
        await Provision(db, http, userId, reactivate: true);
        var a = db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId);
        return H.Str(a?["status"]) == "active";
    }

    /// <summary>Provision (or refresh) a member's Certuvo account. No-op if disabled, already active,
    /// suspended/revoked (unless <paramref name="reactivate"/>), or not eligible under the configured rule.</summary>
    public static async Task Provision(Db db, HttpClient http, long userId, bool reactivate = false)
    {
        if (!Enabled(db)) return;
        if (!Eligible(db, userId)) return;
        db.Execute("INSERT OR IGNORE INTO certuvo_accounts(user_id,status) VALUES(?, 'pending')", userId);
        var cur = db.QueryOne("SELECT status,idempotency_key,username FROM certuvo_accounts WHERE user_id=?", userId);
        var curStatus = H.Str(cur?["status"]);
        if (curStatus == "active") return;
        if (curStatus is "suspended" or "revoked" && !reactivate) return;

        // A stable per-account idempotency key: Certuvo can safely dedupe replays of the same request
        // (manual payment updates, callback retries, repeated webhooks can never double-create).
        var idemKey = H.Str(cur?["idempotency_key"]);
        if (string.IsNullOrEmpty(idemKey))
        {
            idemKey = Guid.NewGuid().ToString("N");
            db.Execute("UPDATE certuvo_accounts SET idempotency_key=? WHERE user_id=?", idemKey, userId);
        }

        var user = db.QueryOne("SELECT email,first_name,last_name,is_test FROM users WHERE id=?", userId);
        if (user is null) return;
        var apiBase = Settings.Str(db, "certuvo_api_base", "").TrimEnd('/');
        var loginUrl = Settings.Str(db, "certuvo_login_url", "");

        // ── PCI-controlled credentials ──────────────────────────────────────────────────────────────
        // PCI generates and owns the Certuvo login. The username is NEVER the student's email (they may
        // already hold a Certuvo account under that email for another institution) and is immutable once
        // assigned. The temporary password is a fresh cryptographically-secure secret, stored encrypted.
        // Both are generated ONCE and reused across retries so what the student sees always matches what
        // was pushed to Certuvo.
        var pciUsername = H.Str(cur?["username"]);
        if (string.IsNullOrEmpty(pciUsername))
        {
            pciUsername = NextUsername(db, userId);
            db.Execute("UPDATE certuvo_accounts SET username=?, member_type=?, eligible_reason=?, updated_at=datetime('now') WHERE user_id=?",
                pciUsername, DetectMemberType(db, userId), Settings.Str(db, "certuvo_requires", "membership"), userId);
        }
        var storedSecret = H.Str(db.QueryOne("SELECT secret FROM certuvo_accounts WHERE user_id=?", userId)?["secret"]);
        var tempPassword = Security.DecryptSecret(storedSecret);
        if (string.IsNullOrEmpty(tempPassword))
        {
            tempPassword = Security.GenPassword((int)Settings.Num(db, "certuvo_password_length", 14));
            db.Execute("UPDATE certuvo_accounts SET secret=?, must_change_password=1, updated_at=datetime('now') WHERE user_id=?",
                Security.EncryptSecret(tempPassword), userId);
        }

        if (apiBase.Length == 0)
        {
            db.Execute("UPDATE certuvo_accounts SET status='pending', last_error=?, updated_at=datetime('now') WHERE user_id=?",
                "Certuvo API base not configured — set it in Admin → Integrations → Certuvo, then retry.", userId);
            return;
        }
        // EXT-P1-01 — refuse private/loopback/metadata targets at request time (in addition to save-time).
        if (Egress.UrlProblem(apiBase) is { } urlProb)
        {
            db.Execute("UPDATE certuvo_accounts SET status='error', last_error=?, updated_at=datetime('now') WHERE user_id=?",
                "Blocked Certuvo API base: " + urlProb, userId);
            return;
        }
        if (!apiBase.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            && string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase))
        {
            db.Execute("UPDATE certuvo_accounts SET status='error', last_error=?, updated_at=datetime('now') WHERE user_id=?",
                "Certuvo API base must be HTTPS in production.", userId);
            return;
        }
        try
        {
            var path = Settings.Str(db, "certuvo_provision_path", "/api/accounts");
            var profile = db.QueryOne("SELECT country FROM student_profiles WHERE user_id=?", userId);
            var membership = db.QueryOne("SELECT membership_type,status,start_date,expiry_date FROM memberships WHERE user_id=? AND status='active'", userId);
            // Documented data mapping — only what Certuvo needs to open a PCI-controlled account. PCI pushes
            // the username + temporary password it generated; never the student's own password, card data,
            // support messages, or admin notes. `membership_number` is the PCI username (the PCI-side member
            // identifier), keeping Certuvo's login independent of email.
            var body = new
            {
                external_ref = userId.ToString(),
                username = pciUsername,
                temp_password = tempPassword,
                must_change_password = true,
                membership_number = pciUsername,
                email = H.Str(user["email"]),
                first_name = H.Str(user["first_name"]),
                last_name = H.Str(user["last_name"]),
                country = H.Str(profile?["country"]),
                membership_status = H.Str(membership?["status"]),
                membership_start = H.Str(membership?["start_date"]),
                membership_expiry = H.Str(membership?["expiry_date"]),
                member_type = DetectMemberType(db, userId),
                is_test = H.L(user["is_test"]) == 1,
            };
            var req = new HttpRequestMessage(HttpMethod.Post, apiBase + "/" + path.TrimStart('/'))
            { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Headers.TryAddWithoutValidation("Idempotency-Key", idemKey);
            if (ApiKey(db) is { Length: > 0 } key)
            {
                var header = Settings.Str(db, "certuvo_auth_header", "Authorization");
                if (string.Equals(header, "Authorization", StringComparison.OrdinalIgnoreCase)) req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                else req.Headers.TryAddWithoutValidation(header, key);
            }
            using var resp = await http.SendAsync(req);
            var txt = await resp.Content.ReadAsStringAsync();
            string? Str(params string[] names) { try { var root = JsonDocument.Parse(txt).RootElement; foreach (var n in names) if (root.TryGetProperty(n, out var v) && v.ValueKind == JsonValueKind.String) return v.GetString(); } catch { } return null; }
            bool Flag(params string[] names) { try { var root = JsonDocument.Parse(txt).RootElement; foreach (var n in names) if (root.TryGetProperty(n, out var v) && (v.ValueKind == JsonValueKind.True || (v.ValueKind == JsonValueKind.String && v.GetString() is "1" or "true"))) return true; } catch { } return false; }

            // ── Email-conflict handling ──────────────────────────────────────────────────────────────
            // If Certuvo reports the email already belongs to an account (HTTP 409, or an explicit conflict
            // flag), PCI NEVER overwrites or merges it. Because PCI already sends a unique PCI username, the
            // configured rule decides what happens: "dedicated" (default) proceeds on the PCI username so the
            // student still gets a PCI-linked account; "manual" parks it for an administrator. Either way the
            // conflict is recorded and support is alerted.
            var conflict = resp.StatusCode == System.Net.HttpStatusCode.Conflict || Flag("email_exists", "conflict", "duplicate_email");
            if (conflict && Settings.Str(db, "certuvo_email_conflict", "dedicated") != "dedicated")
            {
                db.Execute("UPDATE certuvo_accounts SET status='conflict', email_conflict=1, last_error=?, next_retry_at=NULL, updated_at=datetime('now') WHERE user_id=?",
                    "Email already exists in Certuvo — parked for manual review (never overwritten).", userId);
                try { Notify.Alert(db, "certuvo", "Certuvo email conflict needs review",
                    $"<p>Member #{userId} ({System.Net.WebUtility.HtmlEncode(H.Str(user["email"]) ?? "")}) already has a Certuvo account under this email. PCI did not overwrite it. Review it in Admin → Integrations → Certuvo.</p>",
                    "certuvo_account", userId); } catch { }
                return;
            }
            if (conflict) db.Execute("UPDATE certuvo_accounts SET email_conflict=1 WHERE user_id=?", userId);

            if (!resp.IsSuccessStatusCode)
            {
                RecordFailure(db, userId, $"HTTP {(int)resp.StatusCode}: {(txt.Length > 200 ? txt[..200] : txt)}");
                return;
            }
            // Keep the PCI username + PCI temp password authoritative; only take Certuvo's opaque account id
            // and (optionally) a login URL from the response. If Certuvo echoes a different username we still
            // honour ours — PCI owns the login identity.
            var extId = Str("id", "account_id", "external_id");
            var url = Str("login_url", "url") ?? loginUrl;
            db.Execute(@"UPDATE certuvo_accounts SET external_id=?, login_url=?, status='active', last_error=NULL,
                retry_count=0, next_retry_at=NULL, suspended_at=NULL, revoked_at=NULL,
                provisioned_at=COALESCE(provisioned_at, datetime('now')), credentials_sent_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?",
                extId, url, userId);
            SendAccessInstructions(db, userId, first: true);
        }
        catch (Exception ex)
        {
            RecordFailure(db, userId, ex.Message);
        }
    }

    /// <summary>A failed attempt schedules the next automatic retry with exponential backoff
    /// (5 min doubling, capped at 6 h). After the configured maximum the account stays failed for
    /// manual retry only and support is alerted — membership activation is never blocked.</summary>
    static void RecordFailure(Db db, long userId, string error)
    {
        var retries = (int)H.L(db.QueryOne("SELECT retry_count FROM certuvo_accounts WHERE user_id=?", userId)?["retry_count"]);
        var next = retries + 1;
        if (next <= RetryMax(db))
        {
            var mins = Math.Min(360, 5 * (1 << Math.Min(next, 10)));
            db.Execute($"UPDATE certuvo_accounts SET status='error', last_error=?, retry_count=?, next_retry_at=datetime('now','+{mins} minutes'), updated_at=datetime('now') WHERE user_id=?", error, next, userId);
        }
        else
        {
            db.Execute("UPDATE certuvo_accounts SET status='error', last_error=?, retry_count=?, next_retry_at=NULL, updated_at=datetime('now') WHERE user_id=?", error, next, userId);
            try { Notify.Alert(db, "certuvo", "Certuvo provisioning failed repeatedly",
                $"<p>Provisioning the Certuvo practice account for member #{userId} has failed {next} times and automatic retries have stopped.</p><p>Last error: {System.Net.WebUtility.HtmlEncode(error)}</p><p>Retry it from Admin → Integrations → Certuvo.</p>",
                "certuvo_account", userId); } catch { }
        }
    }

    /// <summary>Drain due retries — called from the background dispatcher loop. Atomic leases prevent
    /// multi-instance double-provisioning (EXT-P0-05).</summary>
    public static async Task RetryDue(Db db, HttpClient http, int limit = 5)
    {
        if (!Enabled(db)) return;
        try
        {
            db.Execute("UPDATE certuvo_accounts SET status='error', lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE status='processing' AND lease_until IS NOT NULL AND lease_until<=datetime('now')");
        }
        catch { }
        var due = db.Query("SELECT id,user_id FROM certuvo_accounts WHERE status='error' AND next_retry_at IS NOT NULL AND next_retry_at<=datetime('now') AND (lease_until IS NULL OR lease_until<=datetime('now')) ORDER BY next_retry_at LIMIT " + Math.Clamp(limit, 1, 25));
        var owner = WorkerLease.NewOwner();
        foreach (var r in due)
        {
            var id = H.L(r["id"]);
            if (!WorkerLease.TryClaim(db, "certuvo_accounts", id, owner, "'error'")) continue;
            try { await Provision(db, http, H.L(r["user_id"])); }
            finally { WorkerLease.Clear(db, "certuvo_accounts", id); }
        }
    }

    /// <summary>Re-send the access instructions to the student (in-app + email).</summary>
    public static bool SendAccessInstructions(Db db, long userId, bool first = false)
    {
        var a = db.QueryOne("SELECT username,login_url,status FROM certuvo_accounts WHERE user_id=?", userId);
        if (a is null || H.Str(a["status"]) != "active") return false;
        var title = first ? "Your Certuvo practice access is ready" : "Your Certuvo access instructions (resent)";
        try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Practice', ?, ?, 'Open Certuvo', '/certuvo')",
            userId, title, "Your Certuvo practice account is set up. Find your login details on the Certuvo page in your portal."); } catch { }
        try
        {
            var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", userId)?["email"]);
            if (email is { Length: > 0 })
                Notify.Email(db, userId, email, "Your Certuvo practice access",
                    "<p>Your Certuvo practice account is ready. Sign in to your PCI portal and open the <b>Certuvo</b> page to find your login details.</p>",
                    "certuvo_account", userId);
        }
        catch { }
        db.Execute("UPDATE certuvo_accounts SET credentials_sent_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?", userId);
        return true;
    }

    /// <summary>Suspend or revoke a member's Certuvo access. When a remote deactivate endpoint is
    /// configured, a remote failure does NOT flip local status to revoked/suspended (EXT-P1-06) —
    /// the row is marked for retry with the desired action so observed state can catch up.</summary>
    public static async Task<object> Deactivate(Db db, HttpClient http, long userId, bool revoke)
    {
        var a = db.QueryOne("SELECT external_id,status FROM certuvo_accounts WHERE user_id=?", userId);
        if (a is null) return new { ok = false, error = "no_account" };
        string? remote = null;
        var remoteOk = true;
        var deactPath = Settings.Str(db, "certuvo_deactivate_path", "");
        var apiBase = Settings.Str(db, "certuvo_api_base", "").TrimEnd('/');
        if (deactPath.Length > 0 && apiBase.Length > 0 && H.Str(a["external_id"]) is { Length: > 0 } ext)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, apiBase + "/" + deactPath.TrimStart('/'))
                { Content = new StringContent(JsonSerializer.Serialize(new { external_id = ext, action = revoke ? "revoke" : "suspend" }), Encoding.UTF8, "application/json") };
                if (ApiKey(db) is { Length: > 0 } key)
                {
                    var header = Settings.Str(db, "certuvo_auth_header", "Authorization");
                    if (string.Equals(header, "Authorization", StringComparison.OrdinalIgnoreCase)) req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                    else req.Headers.TryAddWithoutValidation(header, key);
                }
                using var resp = await http.SendAsync(req);
                remote = $"HTTP {(int)resp.StatusCode}";
                remoteOk = resp.IsSuccessStatusCode;
            }
            catch (Exception ex) { remote = "error: " + ex.Message; remoteOk = false; }
            if (!remoteOk)
            {
                db.Execute("UPDATE certuvo_accounts SET last_error=?, updated_at=datetime('now') WHERE user_id=?",
                    $"deactivate_pending:{revoke}:{remote}", userId);
                return new { ok = false, error = "remote_deactivate_failed", status = H.Str(a["status"]), remote, desired = revoke ? "revoked" : "suspended" };
            }
        }
        db.Execute(revoke
            ? "UPDATE certuvo_accounts SET status='revoked', revoked_at=datetime('now'), next_retry_at=NULL, last_error=NULL, updated_at=datetime('now') WHERE user_id=?"
            : "UPDATE certuvo_accounts SET status='suspended', suspended_at=datetime('now'), next_retry_at=NULL, last_error=NULL, updated_at=datetime('now') WHERE user_id=?", userId);
        return new { ok = true, status = revoke ? "revoked" : "suspended", remote };
    }

    /// <summary>The member-facing view of their Certuvo access. Credentials only while active; failures are
    /// summarised in plain language — raw integration errors are for admins, never students.</summary>
    public static object AccessFor(Db db, long userId)
    {
        if (!Enabled(db)) return new { enabled = false };
        var a = db.QueryOne("SELECT external_id,username,secret,login_url,status,must_change_password,provisioned_at,activated_at,credentials_sent_at FROM certuvo_accounts WHERE user_id=?", userId);
        // Access validity: the active membership's expiry, or (for exam-only access under the
        // membership_or_exam rule) the latest paid exam entitlement's valid_until.
        var expiry = H.Str(db.QueryOne("SELECT expiry_date FROM memberships WHERE user_id=? AND status='active'", userId)?["expiry_date"])
            ?? H.Str(db.QueryOne("SELECT valid_until FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed') ORDER BY id DESC", userId)?["valid_until"]);
        // The mandated notice: PCI shows only the access card; everything practice-related lives in Certuvo.
        const string notice = "Certuvo is an external practice platform. All practice questions, mock examinations, study tools, AI coaching, progress tracking and learning activities are available directly within Certuvo.";
        // The platform sign-in URL (operator-configured). Shown as the "Open Certuvo" link even before the
        // student's own account is active, so they always know where to go — the per-account login_url below
        // supersedes it once provisioning completes.
        var portalUrl = Settings.Str(db, "certuvo_login_url", "").Trim();
        string? portal = string.IsNullOrWhiteSpace(portalUrl) ? null : portalUrl;
        if (a is null) return new { enabled = true, status = "not_provisioned", expires = expiry, notice, portal_url = portal };
        var status = H.Str(a["status"]);
        var active = status == "active";
        return new
        {
            enabled = true,
            status,
            notice,
            // "We're still setting it up" — the student never sees an API error.
            message = status switch
            {
                "active" => (string?)null,
                "suspended" => "Your Certuvo access is currently suspended. Contact support if you believe this is an error.",
                "revoked" => "Your Certuvo access has ended.",
                "conflict" => "We are finalising your Certuvo practice access. Our team has been notified and will confirm shortly.",
                _ => "Your PCI membership is active. We are still setting up your Certuvo practice access — you will be notified once it is ready.",
            },
            // The temporary password is decrypted for the student's own view only, and only while active.
            username = active ? H.Str(a["username"]) : null,
            password = active ? Security.DecryptSecret(H.Str(a["secret"])) : null,
            must_change_password = active && H.L(a["must_change_password"]) == 1,
            login_url = active ? H.Str(a["login_url"]) : null,
            // Platform sign-in link, always present when configured (independent of the per-account credentials).
            portal_url = portal,
            provisioned_at = H.Str(a["provisioned_at"]),
            activated_at = H.Str(a["activated_at"]),
            credentials_sent_at = H.Str(a["credentials_sent_at"]),
            expires = expiry,
        };
    }
}
