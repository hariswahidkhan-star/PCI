using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Offline settlement — grant a member exactly what a real (Stripe) payment would, without a card charge.
/// Used by the admin "mark as paid / waive fee" controls and by one-click test users. Mirrors the webhook
/// settlement: a paid payment row, plus the membership and/or exam entitlement, plus the Certuvo hand-off on
/// a membership — so every downstream gate (booking, launch, one-attempt-per-entitlement) applies unchanged.
/// </summary>
public static class Settlement
{
    /// <summary>Grant a paid product to a user. <paramref name="product"/> is membership | exam | bundle.
    /// Returns the created payment id.</summary>
    public static long Grant(Db db, long userId, string? email, string product, long certId, double amount, string reference, string provider)
    {
        product = (product ?? "membership").Trim().ToLowerInvariant();
        var isExam = product is "exam" or "bundle";
        var isMembership = product is "membership" or "bundle";

        var payId = db.ExecuteReturningId(@"INSERT INTO payments(user_id,product_type,standard_amount,final_amount,currency,payment_provider,payment_status,payment_date,reference,exam_schedule_deadline)
            VALUES(?,?,?,?, 'USD', ?, 'paid', datetime('now'), ?, CASE WHEN ?=1 THEN datetime('now','+1 year') ELSE NULL END)",
            userId, product, amount, amount, provider, reference, isExam ? 1 : 0);

        if (isMembership)
        {
            if (db.QueryOne("SELECT id FROM memberships WHERE user_id=?", userId) is not null)
                db.Execute("UPDATE memberships SET status='active', expiry_date=datetime('now','+3 year') WHERE user_id=?", userId);
            else
                db.Execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,renewal_fee,renewal_cycle,amount_paid,currency) VALUES(?, 'Student Membership','active',datetime('now'),datetime('now','+3 year'),99,'3 years',?, 'USD')", userId, amount);
            try { Integrations.Emit(db, "membership.activated", "user", userId, new { user_id = userId, email, membership_type = "Student Membership", occurred_at = H.IsoNow }); } catch { }
            try { CertuvoLink.Provision(db, CertuvoLink.Http, userId).GetAwaiter().GetResult(); } catch { }   // auto Certuvo hand-off
        }
        if (isExam)
            db.Execute("INSERT OR IGNORE INTO exam_entitlements(user_id,payment_id,product_type,certification_id,status,valid_until) VALUES(?,?, 'exam', ?, 'available', datetime('now','+1 year'))", userId, payId, certId);

        try { Integrations.Emit(db, "payment.recorded", "payment", payId, new { payment_id = payId, user_id = userId, email, amount, currency = "USD", product, product_type = product, occurred_at = H.IsoNow }); } catch { }
        return payId;
    }
}

/// <summary>
/// Certuvo external practice-platform integration. Certuvo hosts the practice experience on its own platform;
/// when a student's membership is settled, PCI provisions them a Certuvo account through Certuvo's API and
/// surfaces the credentials + login link in the student panel. Fully configurable (Admin → Certuvo): the base
/// URL, provisioning path, auth header and API key are all set at onboarding, with an `api_base` that can point
/// at a mock for validation. Best-effort and idempotent — a member is provisioned at most once; a failure is
/// recorded and retryable, never fabricated.
/// </summary>
public static class CertuvoLink
{
    public static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

    public static bool Enabled(Db db) => Settings.Bool(db, "certuvo_enabled", false);

    /// <summary>Provision (or refresh) a member's Certuvo account. No-op if disabled or already active.</summary>
    public static async Task Provision(Db db, HttpClient http, long userId)
    {
        if (!Enabled(db)) return;
        db.Execute("INSERT OR IGNORE INTO certuvo_accounts(user_id,status) VALUES(?, 'pending')", userId);
        if (db.QueryOne("SELECT status FROM certuvo_accounts WHERE user_id=?", userId) is { } cur && H.Str(cur["status"]) == "active") return;

        var user = db.QueryOne("SELECT email,first_name,last_name FROM users WHERE id=?", userId);
        if (user is null) return;
        var apiBase = Settings.Str(db, "certuvo_api_base", "").TrimEnd('/');
        var loginUrl = Settings.Str(db, "certuvo_login_url", "");
        if (apiBase.Length == 0)
        {
            db.Execute("UPDATE certuvo_accounts SET status='pending', last_error=?, updated_at=datetime('now') WHERE user_id=?",
                "Certuvo API base not configured — set it in Admin → Certuvo, then retry.", userId);
            return;
        }
        try
        {
            var path = Settings.Str(db, "certuvo_provision_path", "/api/accounts");
            var body = new { external_ref = userId.ToString(), email = H.Str(user["email"]), first_name = H.Str(user["first_name"]), last_name = H.Str(user["last_name"]) };
            var req = new HttpRequestMessage(HttpMethod.Post, apiBase + "/" + path.TrimStart('/'))
            { Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") };
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
                db.Execute("UPDATE certuvo_accounts SET status='error', last_error=?, updated_at=datetime('now') WHERE user_id=?", $"HTTP {(int)resp.StatusCode}: {(txt.Length > 200 ? txt[..200] : txt)}", userId);
                return;
            }
            string? Str(params string[] names) { try { foreach (var n in names) if (JsonDocument.Parse(txt).RootElement.TryGetProperty(n, out var v) && v.ValueKind == JsonValueKind.String) return v.GetString(); } catch { } return null; }
            var username = Str("username", "login", "user") ?? H.Str(user["email"]);
            var password = Str("password", "temp_password", "secret");
            var extId = Str("id", "account_id", "external_id");
            var url = Str("login_url", "url") ?? loginUrl;
            db.Execute("UPDATE certuvo_accounts SET external_id=?, username=?, secret=?, login_url=?, status='active', last_error=NULL, provisioned_at=datetime('now'), updated_at=datetime('now') WHERE user_id=?",
                extId, username, password, url, userId);
            try { db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Practice', 'Your Certuvo practice access is ready', ?, 'Open Certuvo', '/certuvo')",
                userId, "Your Certuvo practice account has been created. Find your login details on the Certuvo page in your portal."); } catch { }
        }
        catch (Exception ex)
        {
            db.Execute("UPDATE certuvo_accounts SET status='error', last_error=?, updated_at=datetime('now') WHERE user_id=?", ex.Message, userId);
        }
    }

    /// <summary>The member-facing view of their Certuvo access (credentials included — it's their own account).</summary>
    public static object AccessFor(Db db, long userId)
    {
        if (!Enabled(db)) return new { enabled = false };
        var a = db.QueryOne("SELECT external_id,username,secret,login_url,status,last_error,provisioned_at FROM certuvo_accounts WHERE user_id=?", userId);
        if (a is null) return new { enabled = true, status = "not_provisioned" };
        return new
        {
            enabled = true,
            status = H.Str(a["status"]),
            username = H.Str(a["username"]),
            password = H.Str(a["secret"]),
            login_url = H.Str(a["login_url"]),
            provisioned_at = H.Str(a["provisioned_at"]),
            error = H.Str(a["last_error"]),
        };
    }
}
