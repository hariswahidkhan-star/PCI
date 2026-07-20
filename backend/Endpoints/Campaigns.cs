using System.Security.Cryptography;
using System.Text;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Bulk / marketing email campaigns. A campaign is authored as a draft, previewed against its audience,
/// test-sent, then dispatched in the background to every deliverable recipient.
///
/// The anti-spam / CAN-SPAM / GDPR guarantees are enforced HERE, not left to the author:
///   • every email — test or bulk — gets an auto-appended footer with the org name, the reason the
///     recipient is hearing from us, an optional postal address, and a working ONE-CLICK unsubscribe link;
///   • the suppression list (unsubscribes + manual entries) is ALWAYS honoured before any send, and for
///     the "subscribers" audience an address must still be status='subscribed';
///   • {{first_name}} / {{email}} are personalised per recipient (blank name ⇒ "there").
///
/// Public (no auth):
///   GET/POST /api/unsubscribe            — one-click unsubscribe (token-verified, neutral page on failure)
/// Admin (gate "subscribers", same as the subscribers view):
///   GET  /api/admin/campaigns            — list, newest first
///   POST /api/admin/campaigns            — create a draft
///   GET  /api/admin/campaigns/{id}       — campaign + recipient sample + live counts
///   POST /api/admin/campaigns/audience-preview — deliverable/suppressed sizes before sending
///   POST /api/admin/campaigns/{id}/test  — send one test with the full footer + "[TEST]" prefix
///   POST /api/admin/campaigns/{id}/send  — resolve audience, then send in the background in batches
///   GET  /api/admin/suppression          — list the suppression list
///   POST /api/admin/suppression          — add / remove an address manually
/// </summary>
public static class Campaigns
{
    static readonly System.Text.RegularExpressions.Regex EmailRx =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);

    static readonly string[] Audiences = { "students", "subscribers", "all" };

    /// <summary>One-click unsubscribe token: the first 24 hex chars of HMAC-SHA256(lower(email), secret).
    /// Secret precedence: NEWSLETTER_SALT ⇒ FORUM_SALT ⇒ a built-in default. Shared by send + verify so
    /// the two can never drift.</summary>
    public static string UnsubToken(string email)
    {
        var secret = Environment.GetEnvironmentVariable("NEWSLETTER_SALT")
                  ?? Environment.GetEnvironmentVariable("FORUM_SALT")
                  ?? "pci-unsub-secret";
        using var h = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var bytes = h.ComputeHash(Encoding.UTF8.GetBytes((email ?? "").Trim().ToLowerInvariant()));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString().Substring(0, 24);
    }

    static bool TokenOk(string email, string? supplied)
    {
        if (string.IsNullOrEmpty(supplied)) return false;
        var expected = UnsubToken(email);
        var a = Encoding.ASCII.GetBytes(expected);
        var b = Encoding.ASCII.GetBytes(supplied);
        if (a.Length != b.Length) return false;              // FixedTimeEquals requires equal lengths
        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";

        // ---------- footer + personalisation (mandatory on every send) ----------
        string Footer(string baseUrl, string email)
        {
            var addr = S("org_postal_address").Trim();
            var link = $"{baseUrl}/api/unsubscribe?e={Uri.EscapeDataString(email)}&t={UnsubToken(email)}";
            var enc = System.Net.WebUtility.HtmlEncode(email);
            var sb = new StringBuilder();
            sb.Append("<hr style=\"border:none;border-top:1px solid #e2e2e2;margin:32px 0 16px\">");
            sb.Append("<div style=\"font-size:12px;line-height:1.6;color:#777;font-family:Arial,Helvetica,sans-serif\">");
            sb.Append("<p style=\"margin:0 0 6px\"><strong>Project Controls Institute</strong></p>");
            sb.Append("<p style=\"margin:0 0 6px\">You're receiving this because you subscribed or hold a PCI account.</p>");
            if (!string.IsNullOrEmpty(addr))
                sb.Append($"<p style=\"margin:0 0 6px\">{System.Net.WebUtility.HtmlEncode(addr)}</p>");
            sb.Append($"<p style=\"margin:0\">Sent to {enc}. ");
            sb.Append($"<a href=\"{link}\" style=\"color:#777\">Unsubscribe with one click</a>.</p>");
            sb.Append("</div>");
            return sb.ToString();
        }

        string Personalise(string s, string email, string? firstName)
        {
            var fn = string.IsNullOrWhiteSpace(firstName) ? "there" : firstName!.Trim();
            return (s ?? "").Replace("{{first_name}}", fn).Replace("{{email}}", email);
        }

        // ---------- audience resolution (deliverable list + suppressed count) ----------
        // Candidates come from users (students, active) and/or newsletter_subscribers. Suppression is
        // always applied; subscriber-sourced rows must additionally be status='subscribed'. De-duplicated
        // case-insensitively so an address that is both a student and a subscriber is only mailed once.
        (List<(string Email, string? First)> Deliver, int Suppressed) Resolve(string audience)
        {
            var suppressed = 0;
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var deliver = new List<(string, string?)>();
            var supp = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in db.Query("SELECT email FROM email_suppression"))
                supp.Add((H.Str(r["email"]) ?? "").Trim().ToLowerInvariant());

            void Consider(string? emailRaw, string? first, bool subscriberSource, string? subStatus)
            {
                var email = (emailRaw ?? "").Trim();
                if (email.Length == 0 || !EmailRx.IsMatch(email)) return;
                var key = email.ToLowerInvariant();
                if (!seen.Add(key)) return;                                   // de-dup across sources
                if (supp.Contains(key)) { suppressed++; return; }             // globally suppressed
                if (subscriberSource && subStatus != "subscribed") { suppressed++; return; }
                deliver.Add((email, first));
            }

            if (audience is "students" or "all")
                foreach (var u in db.Query("SELECT email,first_name FROM users WHERE role='student' AND status='active'"))
                    Consider(H.Str(u["email"]), H.Str(u["first_name"]), false, null);
            if (audience is "subscribers" or "all")
                foreach (var s in db.Query("SELECT email,status FROM newsletter_subscribers"))
                    Consider(H.Str(s["email"]), null, true, H.Str(s["status"]));

            return (deliver, suppressed);
        }

        // ================= PUBLIC: one-click unsubscribe =================
        // Idempotent. Valid token ⇒ suppress + mark the subscriber unsubscribed. Invalid ⇒ a neutral page
        // that neither confirms nor suppresses (don't leak whether an address / token was real).
        void DoUnsubscribe(string email)
        {
            var e = (email ?? "").Trim().ToLowerInvariant();
            if (e.Length == 0) return;
            try { db.Execute("INSERT OR IGNORE INTO email_suppression(email,reason) VALUES(?, 'unsubscribe')", e); } catch { }
            try { db.Execute("UPDATE newsletter_subscribers SET status='unsubscribed' WHERE email=?", e); } catch { }
        }

        IResult Page(string title, string body)
        {
            var html = "<!doctype html><html lang=\"en\"><head><meta charset=\"utf-8\">" +
                       "<meta name=\"viewport\" content=\"width=device-width,initial-scale=1\">" +
                       $"<title>{title}</title></head>" +
                       "<body style=\"margin:0;background:#f6f7f9;font-family:Arial,Helvetica,sans-serif;color:#222\">" +
                       "<div style=\"max-width:520px;margin:64px auto;background:#fff;border-radius:12px;" +
                       "padding:40px 36px;box-shadow:0 1px 4px rgba(0,0,0,.06);text-align:center\">" +
                       "<div style=\"font-weight:700;font-size:15px;letter-spacing:.02em;color:#0b3d67;margin-bottom:20px\">PROJECT CONTROLS INSTITUTE</div>" +
                       $"<h1 style=\"font-size:20px;margin:0 0 12px\">{title}</h1>" +
                       $"<p style=\"font-size:14px;line-height:1.7;color:#555;margin:0\">{body}</p>" +
                       "</div></body></html>";
            return Results.Content(html, "text/html; charset=utf-8");
        }

        IResult Unsubscribe(string? email, string? token)
        {
            var e = (email ?? "").Trim();
            if (TokenOk(e, token))
            {
                DoUnsubscribe(e);
                log(null, "campaign_unsubscribe", e.ToLowerInvariant());
                return Page("You've been unsubscribed",
                    "You will no longer receive marketing emails from the Project Controls Institute. " +
                    "Operational emails tied to your account (such as exam or payment notices) are unaffected.");
            }
            // Neutral page — never reveal that the token/address was invalid, never suppress on a bad token.
            return Page("Unsubscribe",
                "If your request was valid, your preferences have been updated. " +
                "You can safely close this page.");
        }

        app.MapGet("/api/unsubscribe", (HttpRequest req) =>
            Unsubscribe(req.Query["e"].ToString(), req.Query["t"].ToString()));

        app.MapPost("/api/unsubscribe", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return Unsubscribe(H.GetS(b, "email", "e"), H.GetS(b, "token", "t"));
        });

        // ================= ADMIN =================
        // ---------- list ----------
        app.MapGet("/api/admin/campaigns", (HttpRequest req) => gate(req, "subscribers", _ =>
            J(new { rows = db.Query(
                "SELECT id,name,subject,audience,status,total,sent,failed,suppressed,created_at,sent_at " +
                "FROM email_campaigns ORDER BY id DESC") })));

        // ---------- create draft ----------
        app.MapPost("/api/admin/campaigns", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "subscribers", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var name = (H.GetS(b, "name") ?? "").Trim();
            var subject = (H.GetS(b, "subject") ?? "").Trim();
            var body = (H.GetS(b, "body", "body_html") ?? "").Trim();
            var audience = (H.GetS(b, "audience") ?? "").Trim().ToLowerInvariant();
            if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
            if (subject.Length == 0) return Results.Json(new { error = "subject_required" }, statusCode: 400);
            if (body.Length < 10) return Results.Json(new { error = "body_too_short" }, statusCode: 400);
            if (!Audiences.Contains(audience)) return Results.Json(new { error = "invalid_audience" }, statusCode: 400);
            var id = db.ExecuteReturningId(
                "INSERT INTO email_campaigns(name,subject,body_html,audience,status) VALUES(?,?,?,?, 'draft')",
                name, subject, body, audience);
            log(actorId, "campaign_create", id.ToString());
            return J(new { id });
        });

        // ---------- detail: campaign + recipient sample + live counts ----------
        app.MapGet("/api/admin/campaigns/{id:long}", (HttpRequest req, long id) => gate(req, "subscribers", _ =>
        {
            var c = db.QueryOne("SELECT * FROM email_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var recipients = db.Query(
                "SELECT email,first_name,status,error,sent_at FROM campaign_recipients WHERE campaign_id=? ORDER BY id LIMIT 20", id);
            var counts = new
            {
                total = H.L(db.Scalar<long>("SELECT COUNT(*) FROM campaign_recipients WHERE campaign_id=?", id)),
                sent = H.L(db.Scalar<long>("SELECT COUNT(*) FROM campaign_recipients WHERE campaign_id=? AND status='sent'", id)),
                failed = H.L(db.Scalar<long>("SELECT COUNT(*) FROM campaign_recipients WHERE campaign_id=? AND status='failed'", id)),
                pending = H.L(db.Scalar<long>("SELECT COUNT(*) FROM campaign_recipients WHERE campaign_id=? AND status='pending'", id)),
            };
            return J(new { campaign = c, recipients, counts });
        }));

        // ---------- audience preview (size before sending) ----------
        app.MapPost("/api/admin/campaigns/audience-preview", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "subscribers", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var b = await H.Body(ctx.Request);
            var audience = (H.GetS(b, "audience") ?? "").Trim().ToLowerInvariant();
            if (!Audiences.Contains(audience)) return Results.Json(new { error = "invalid_audience" }, statusCode: 400);
            var (deliver, suppressed) = Resolve(audience);
            return J(new { count = deliver.Count, suppressed });
        });

        // ---------- send a single test (full footer + [TEST] prefix; does not touch the audience) ----------
        app.MapPost("/api/admin/campaigns/{id:long}/test", async (HttpContext ctx, long id) =>
        {
            var denied = gate(ctx.Request, "subscribers", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var to = (H.GetS(b, "to") ?? "").Trim();
            if (!EmailRx.IsMatch(to)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            var c = db.QueryOne("SELECT * FROM email_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var baseUrl = Mailer.BaseUrl(ctx.Request);
            var subject = "[TEST] " + Personalise(H.Str(c["subject"]) ?? "", to, null);
            var html = Personalise(H.Str(c["body_html"]) ?? "", to, null) + Footer(baseUrl, to);
            // Notify.Email sends AND records the attempt in notification_history (never throws).
            var status = Notify.Email(db, null, to, subject, html, "campaign", id);
            log(actorId, "campaign_test", id.ToString());
            return J(new { ok = true, status });
        });

        // ---------- send (background, batched) ----------
        app.MapPost("/api/admin/campaigns/{id:long}/send", (HttpRequest req, long id) => gate(req, "subscribers", admin =>
        {
            var c = db.QueryOne("SELECT * FROM email_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if ((H.Str(c["status"]) ?? "") != "draft")
                return Results.Json(new { ok = false, error = "not_draft" }, statusCode: 409);   // double-send guard

            var audience = (H.Str(c["audience"]) ?? "").ToLowerInvariant();
            var (deliver, suppressed) = Resolve(audience);

            // Write the recipient ledger + flip to 'sending' synchronously, so a concurrent /send sees a
            // non-draft status and is rejected above before any second dispatch can start.
            foreach (var (email, first) in deliver)
                db.Execute("INSERT INTO campaign_recipients(campaign_id,email,first_name,status) VALUES(?,?,?, 'pending')",
                    id, email, first);
            db.Execute("UPDATE email_campaigns SET status='sending', total=?, sent=0, failed=0, suppressed=? WHERE id=?",
                deliver.Count, suppressed, id);
            log(admin.Id, "campaign_send", id.ToString());

            var baseUrl = Mailer.BaseUrl(req);
            var subjectTpl = H.Str(c["subject"]) ?? "";
            var bodyTpl = H.Str(c["body_html"]) ?? "";

            // Background dispatch: batches of 25 with a 300ms pause, so a large list neither blocks this
            // request nor hammers the provider. Each send is isolated — one failure never aborts the run.
            _ = Task.Run(async () =>
            {
                try
                {
                    var rows = db.Query("SELECT id,email,first_name FROM campaign_recipients WHERE campaign_id=? AND status='pending' ORDER BY id", id);
                    var n = 0;
                    foreach (var r in rows)
                    {
                        var rid = H.L(r["id"]);
                        var email = H.Str(r["email"]) ?? "";
                        var first = H.Str(r["first_name"]);
                        try
                        {
                            var subject = Personalise(subjectTpl, email, first);
                            var html = Personalise(bodyTpl, email, first) + Footer(baseUrl, email);
                            Mailer.Send(db, null, email, "campaign", subject, html);
                            db.Execute("UPDATE campaign_recipients SET status='sent', sent_at=datetime('now') WHERE id=?", rid);
                            db.Execute("UPDATE email_campaigns SET sent=sent+1 WHERE id=?", id);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                db.Execute("UPDATE campaign_recipients SET status='failed', error=? WHERE id=?",
                                    ex.Message.Length > 300 ? ex.Message.Substring(0, 300) : ex.Message, rid);
                                db.Execute("UPDATE email_campaigns SET failed=failed+1 WHERE id=?", id);
                            }
                            catch { }
                        }
                        if (++n % 25 == 0) await Task.Delay(300);
                    }
                    db.Execute("UPDATE email_campaigns SET status='sent', sent_at=datetime('now') WHERE id=?", id);
                }
                catch
                {
                    try { db.Execute("UPDATE email_campaigns SET status='failed' WHERE id=?", id); } catch { }
                }
            });

            return J(new { ok = true, total = deliver.Count, suppressed });
        }));

        // ---------- suppression list ----------
        app.MapGet("/api/admin/suppression", (HttpRequest req) => gate(req, "subscribers", _ =>
            J(new { rows = db.Query("SELECT id,email,reason,created_at FROM email_suppression ORDER BY id DESC") })));

        app.MapPost("/api/admin/suppression", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "subscribers", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            var action = (H.GetS(b, "action") ?? "add").Trim().ToLowerInvariant();
            if (!EmailRx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            if (action == "remove")
                db.Execute("DELETE FROM email_suppression WHERE email=?", email);
            else
                db.Execute("INSERT OR IGNORE INTO email_suppression(email,reason) VALUES(?, 'manual')", email);
            log(actorId, "campaign_suppression", $"{action}:{email}");
            return J(new { ok = true });
        });
    }
}
