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
        var waived = provider == "admin_waiver" && amount <= 0;
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
                try { Integrations.Emit(db, "membership.activated", "user", userId, new { user_id = userId, email, membership_type = "Student Membership", occurred_at = H.IsoNow }); } catch { }
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
        }
        if (isMembership && CertuvoLink.Enabled(db))
        {
            var before = H.Str(db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId)?["status"]);
            if (before != "active")
            {
                try { CertuvoLink.Provision(db, CertuvoLink.Http, userId).GetAwaiter().GetResult(); } catch { }
                var after = H.Str(db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId)?["status"]);
                if (after == "active" && before != "active") did.Add("certuvo_provisioned");
            }
        }
        return new { ok = true, payment_id = payId, product, status, ensured = did, already_complete = did.Count == 0 };
    }

    /// <summary>Grant an entitlement with a chosen certification (used by exam waivers/settlements that
    /// target a specific credential — the generic path defaults to certification 1).</summary>
    public static void RetargetEntitlement(Db db, long payId, long certId)
    {
        if (certId > 0) db.Execute("UPDATE exam_entitlements SET certification_id=? WHERE payment_id=?", certId, payId);
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
        try { db.Execute("INSERT INTO notifications(user_id,category,title,body) VALUES(?, 'Account', 'A payment record was reversed', ?)", userId,
            $"A {product} settlement on your account was reversed by our team. If you believe this is an error, contact support."); } catch { }
        return new { ok = true, payment_id = payId, previous_status = status, new_status = "refunded", membership_lapsed = membershipLapsed };
    }
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
    public static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

    public static bool Enabled(Db db) => Settings.Bool(db, "certuvo_enabled", false);

    /// <summary>Configurable business rule: does Certuvo access require only an active membership
    /// (default), or membership plus a certification enrolment (exam entitlement)?</summary>
    public static bool Eligible(Db db, long userId)
    {
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        if (!hasMembership) return false;
        if (Settings.Str(db, "certuvo_requires", "membership") == "membership_and_enrolment")
            return db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        return true;
    }

    static int RetryMax(Db db) => (int)Settings.Num(db, "certuvo_retry_max", 8);

    /// <summary>Provision (or refresh) a member's Certuvo account. No-op if disabled, already active,
    /// suspended/revoked (unless <paramref name="reactivate"/>), or not eligible under the configured rule.</summary>
    public static async Task Provision(Db db, HttpClient http, long userId, bool reactivate = false)
    {
        if (!Enabled(db)) return;
        if (!Eligible(db, userId)) return;
        db.Execute("INSERT OR IGNORE INTO certuvo_accounts(user_id,status) VALUES(?, 'pending')", userId);
        var cur = db.QueryOne("SELECT status,idempotency_key FROM certuvo_accounts WHERE user_id=?", userId);
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
        if (apiBase.Length == 0)
        {
            db.Execute("UPDATE certuvo_accounts SET status='pending', last_error=?, updated_at=datetime('now') WHERE user_id=?",
                "Certuvo API base not configured — set it in Admin → Integrations → Certuvo, then retry.", userId);
            return;
        }
        try
        {
            var path = Settings.Str(db, "certuvo_provision_path", "/api/accounts");
            var profile = db.QueryOne("SELECT country FROM student_profiles WHERE user_id=?", userId);
            var membership = db.QueryOne("SELECT start_date,expiry_date FROM memberships WHERE user_id=? AND status='active'", userId);
            // Documented data mapping — only what Certuvo needs to open the account. Never passwords,
            // card data, support messages, or admin notes.
            var body = new
            {
                external_ref = userId.ToString(),
                email = H.Str(user["email"]),
                first_name = H.Str(user["first_name"]),
                last_name = H.Str(user["last_name"]),
                country = H.Str(profile?["country"]),
                membership_start = H.Str(membership?["start_date"]),
                membership_expiry = H.Str(membership?["expiry_date"]),
                is_test = H.L(user["is_test"]) == 1,
            };
            var req = new HttpRequestMessage(HttpMethod.Post, apiBase + "/" + path.TrimStart('/'))
            { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Headers.TryAddWithoutValidation("Idempotency-Key", idemKey);
            if (Settings.Str(db, "certuvo_api_key", "") is { Length: > 0 } key)
            {
                var header = Settings.Str(db, "certuvo_auth_header", "Authorization");
                if (string.Equals(header, "Authorization", StringComparison.OrdinalIgnoreCase)) req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                else req.Headers.TryAddWithoutValidation(header, key);
            }
            using var resp = await http.SendAsync(req);
            var txt = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
            {
                RecordFailure(db, userId, $"HTTP {(int)resp.StatusCode}: {(txt.Length > 200 ? txt[..200] : txt)}");
                return;
            }
            string? Str(params string[] names) { try { foreach (var n in names) if (JsonDocument.Parse(txt).RootElement.TryGetProperty(n, out var v) && v.ValueKind == JsonValueKind.String) return v.GetString(); } catch { } return null; }
            var username = Str("username", "login", "user") ?? H.Str(user["email"]);
            var password = Str("password", "temp_password", "secret");
            var extId = Str("id", "account_id", "external_id");
            var url = Str("login_url", "url") ?? loginUrl;
            db.Execute(@"UPDATE certuvo_accounts SET external_id=?, username=?, secret=?, login_url=?, status='active', last_error=NULL,
                retry_count=0, next_retry_at=NULL, suspended_at=NULL, revoked_at=NULL,
                provisioned_at=datetime('now'), credentials_sent_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?",
                extId, username, password, url, userId);
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

    /// <summary>Drain due retries — called from the background dispatcher loop.</summary>
    public static async Task RetryDue(Db db, HttpClient http, int limit = 5)
    {
        if (!Enabled(db)) return;
        var due = db.Query("SELECT user_id FROM certuvo_accounts WHERE status='error' AND next_retry_at IS NOT NULL AND next_retry_at<=datetime('now') ORDER BY next_retry_at LIMIT " + Math.Clamp(limit, 1, 25));
        foreach (var r in due) await Provision(db, http, H.L(r["user_id"]));
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

    /// <summary>Suspend or revoke a member's Certuvo access. Best-effort remote deactivation via the
    /// configured endpoint; the local status is authoritative for what the student sees.</summary>
    public static async Task<object> Deactivate(Db db, HttpClient http, long userId, bool revoke)
    {
        var a = db.QueryOne("SELECT external_id,status FROM certuvo_accounts WHERE user_id=?", userId);
        if (a is null) return new { ok = false, error = "no_account" };
        string? remote = null;
        var deactPath = Settings.Str(db, "certuvo_deactivate_path", "");
        var apiBase = Settings.Str(db, "certuvo_api_base", "").TrimEnd('/');
        if (deactPath.Length > 0 && apiBase.Length > 0 && H.Str(a["external_id"]) is { Length: > 0 } ext)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Post, apiBase + "/" + deactPath.TrimStart('/'))
                { Content = new StringContent(JsonSerializer.Serialize(new { external_id = ext, action = revoke ? "revoke" : "suspend" }), Encoding.UTF8, "application/json") };
                if (Settings.Str(db, "certuvo_api_key", "") is { Length: > 0 } key)
                {
                    var header = Settings.Str(db, "certuvo_auth_header", "Authorization");
                    if (string.Equals(header, "Authorization", StringComparison.OrdinalIgnoreCase)) req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                    else req.Headers.TryAddWithoutValidation(header, key);
                }
                using var resp = await http.SendAsync(req);
                remote = $"HTTP {(int)resp.StatusCode}";
            }
            catch (Exception ex) { remote = "error: " + ex.Message; }
        }
        db.Execute(revoke
            ? "UPDATE certuvo_accounts SET status='revoked', revoked_at=datetime('now'), next_retry_at=NULL, updated_at=datetime('now') WHERE user_id=?"
            : "UPDATE certuvo_accounts SET status='suspended', suspended_at=datetime('now'), next_retry_at=NULL, updated_at=datetime('now') WHERE user_id=?", userId);
        return new { ok = true, status = revoke ? "revoked" : "suspended", remote };
    }

    /// <summary>The member-facing view of their Certuvo access. Credentials only while active; failures are
    /// summarised in plain language — raw integration errors are for admins, never students.</summary>
    public static object AccessFor(Db db, long userId)
    {
        if (!Enabled(db)) return new { enabled = false };
        var a = db.QueryOne("SELECT external_id,username,secret,login_url,status,provisioned_at,activated_at,credentials_sent_at FROM certuvo_accounts WHERE user_id=?", userId);
        var expiry = H.Str(db.QueryOne("SELECT expiry_date FROM memberships WHERE user_id=? AND status='active'", userId)?["expiry_date"]);
        if (a is null) return new { enabled = true, status = "not_provisioned", expires = expiry };
        var status = H.Str(a["status"]);
        var active = status == "active";
        return new
        {
            enabled = true,
            status,
            // "We're still setting it up" — the student never sees an API error.
            message = status switch
            {
                "active" => (string?)null,
                "suspended" => "Your Certuvo access is currently suspended. Contact support if you believe this is an error.",
                "revoked" => "Your Certuvo access has ended.",
                _ => "Your PCI membership is active. We are still setting up your Certuvo practice access — you will be notified once it is ready.",
            },
            username = active ? H.Str(a["username"]) : null,
            password = active ? H.Str(a["secret"]) : null,
            login_url = active ? H.Str(a["login_url"]) : null,
            provisioned_at = H.Str(a["provisioned_at"]),
            activated_at = H.Str(a["activated_at"]),
            credentials_sent_at = H.Str(a["credentials_sent_at"]),
            expires = expiry,
        };
    }
}
