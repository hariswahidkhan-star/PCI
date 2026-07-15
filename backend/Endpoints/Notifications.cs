using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Owner-facing notification settings: the address(es) that receive operational alerts (new chat
/// hand-offs, enrolments, inquiries), plus per-event on/off switches and a "send a test" action so the
/// owner can prove delivery works. Recipients are a multi-address list (comma / semicolon / newline
/// separated) read by <see cref="Notify.Recipients"/>; nothing here is hardcoded — the owner just adds
/// their email(s) and starts receiving everything.
///
///   GET  /api/admin/notifications        — recipients, toggles, and the recent delivery log
///   POST /api/admin/notifications        — save recipients + toggles
///   POST /api/admin/notifications/test   — send a test alert to every configured recipient
/// All three require the "content" admin permission, matching the other settings endpoints.
/// </summary>
public static class Notifications
{
    // The events an owner can be alerted about. Key -> label. The toggle for each is notify_<key>_enabled.
    static readonly (string key, string label)[] Events =
    {
        ("chat", "Live chat requests"),
        ("enrollment", "Enrolments & payments"),
        ("inquiry", "Website inquiries"),
    };

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
        void Set(string skey, string v)
        {
            db.Execute("DELETE FROM site_settings WHERE skey=?", skey);
            db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", skey, v);
        }

        // ---------- read current settings + recent deliveries ----------
        app.MapGet("/api/admin/notifications", (HttpRequest req) => gate(req, "content", _ =>
        {
            var toggles = new Dictionary<string, object?>();
            foreach (var (key, _) in Events) toggles[key] = Notify.Enabled(db, key);
            var recips = Notify.Recipients(db);           // the resolved, de-duplicated list actually used
            var recent = db.Query("SELECT channel,recipient,subject,status,related_type,created_at FROM notification_history ORDER BY id DESC LIMIT 15");
            return J(new
            {
                recipients = S("notify_recipients"),
                admin_email = Notify.AdminEmail(db),
                resolved = recips,                        // what a real alert would fan out to right now
                events = Events.Select(e => new { e.key, e.label }),
                toggles,
                recent,
            });
        }));

        // ---------- save recipients + toggles ----------
        app.MapPost("/api/admin/notifications", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "content", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var b = await H.Body(ctx.Request);
            if (H.GetS(b, "recipients") is { } r) Set("notify_recipients", r.Trim());
            foreach (var (key, _) in Events)
                if (H.GetEl(b, key) is { } en)
                {
                    var on = en.ValueKind switch
                    {
                        System.Text.Json.JsonValueKind.True => true,
                        System.Text.Json.JsonValueKind.False => false,
                        System.Text.Json.JsonValueKind.Number => en.GetDouble() != 0,
                        System.Text.Json.JsonValueKind.String => en.GetString() is "1" or "true" or "on",
                        _ => true,
                    };
                    Set("notify_" + key + "_enabled", on ? "1" : "0");
                }
            log(null, "notifications_update", string.Join(",", b.Keys));
            return J(new { ok = true, resolved = Notify.Recipients(db) });
        });

        // ---------- send a test alert (prove delivery end-to-end) ----------
        app.MapPost("/api/admin/notifications/test", (HttpRequest req) => gate(req, "content", _ =>
        {
            var recips = Notify.Recipients(db);
            if (recips.Count == 0)
                return J(new { ok = false, error = "no_recipients", message = "Add at least one email address first." });
            var sent = 0;
            foreach (var to in recips)
            {
                try { Mailer.Send(db, null, to, "notification", "PCI test notification",
                        "<p>This is a test notification from your PCI admin panel.</p>" +
                        "<p>If you received this, alerts for chats, enrolments and inquiries will reach this address.</p>");
                      Notify.Record(db, "email", to, "PCI test notification", "sent", "test", null); sent++; }
                catch { Notify.Record(db, "email", to, "PCI test notification", "failed", "test", null); }
            }
            log(null, "notifications_test", sent.ToString());
            return J(new { ok = true, sent, recipients = recips });
        }));
    }
}
