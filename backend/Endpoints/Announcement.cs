using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin-controlled site-wide announcement modal shown on the public website (rendered by
/// <c>assets/premium.js</c>). Everything the visitor sees — whether it shows at all, the launch date,
/// every line of copy and the call-to-action — is editable from the admin console, so no code change
/// is needed to update or retire the notice.
///
///   GET  /api/announcement        — public: the resolved config, or { enabled:false }
///   GET  /api/admin/announcement  — admin (content perm): every stored field + enabled
///   POST /api/admin/announcement  — admin (content perm): update fields / toggle show-hide
///
/// Values live in <c>site_settings</c> under the <c>announce_*</c> keys (same pattern as Social).
/// The literal token <c>{date}</c> in any text field is replaced server-side with the single editable
/// <c>announce_date</c> value, so the launch date is changed in exactly one place.
/// </summary>
public static class Announcement
{
    // Editable text fields (key suffix → default). Kept here so the public endpoint, the admin editor
    // and the first-run seed all agree on the field set and its defaults.
    public static readonly (string key, string def)[] Fields =
    {
        ("date",      "1 November 2026"),
        ("eyebrow",   "Project Controls Institute · Announcement"),
        ("title",     "Certifications open for examination on {date}"),
        ("subtitle",  "Applications are open now — Honorary Fellowship only"),
        ("intro",     "The PCI AI Project Leadership Certification Suite — PCI PCL-AI, PFL-AI and PML-AI — is scheduled to open for enrolment and examination on {date}. Ahead of that date, the Institute is accepting applications for Honorary Fellow (PCI) recognition only."),
        ("p1_title",  "Review times."),
        ("p1_text",   "Owing to the current volume of applications, a decision may take approximately 15 to 30 days from the date your complete application is received."),
        ("p2_title",  "If your application is successful."),
        ("p2_text",   "Accepted applicants are issued personal access credentials to the PCI student portal, together with access to the Institute’s learning resources and study materials."),
        ("p3_title",  "No fee to be recognised."),
        ("p3_text",   "PCI charges no nomination, assessment or credential fee — honorary recognition is awarded solely on merit following independent review. Optional charges may apply only for extras such as printed certificates, delivery or replacements, and never influence the decision."),
        ("note",      "Honorary Fellow (PCI) is a board-conferred recognition of distinguished contribution to the profession. It involves no examination, is awarded at the Institute’s discretion on the merits of each application, and is not an examined PCI certification. Dates, eligibility and benefits may change and do not form a contract or guarantee."),
        ("cta_label", "Apply for Honorary Fellow (PCI)"),
        ("cta_href",  "honorary-application.html"),
        ("dismiss",   "Continue browsing"),
        ("version",   "2026-11-01"),
    };

    /// <summary>Seed the defaults on first run (only fills a key that is entirely absent, so admin edits
    /// and an explicit hide are never overwritten). Called from migrations.</summary>
    public static void Seed(Db db)
    {
        void Fill(string key, string val)
        {
            var exists = db.Scalar<long>("SELECT COUNT(*) FROM site_settings WHERE skey=?", key);
            if (exists == 0) db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", key, val);
        }
        Fill("announce_enabled", "1");
        foreach (var (k, def) in Fields) Fill("announce_" + k, def);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(string suffix) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", "announce_" + suffix) ?? "";
        string Def(string suffix) => Array.Find(Fields, f => f.key == suffix).def ?? "";
        // Fall back to the built-in default when a field has never been stored, so a partial config still renders.
        string Val(string suffix) { var v = S(suffix); return string.IsNullOrEmpty(v) ? Def(suffix) : v; }
        bool Enabled() => (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='announce_enabled'") ?? "1") == "1";

        // ---------- public ----------
        app.MapGet("/api/announcement", (HttpContext ctx) =>
        {
            if (!Enabled()) return J(new { enabled = false });
            // Serve the visitor's language (cookie/?lang=, same resolution as the pages): a translated
            // field from content_i18n scope "ann" overlays the stored English; untranslated stays English.
            var lang = I18nContent.ActiveLang(ctx);
            string L(string suffix) => I18nContent.AnnField(db, lang, suffix) ?? Val(suffix);
            var date = L("date");
            string R(string suffix) => L(suffix).Replace("{date}", date);   // resolve the {date} token
            var points = new List<object>();
            foreach (var n in new[] { "1", "2", "3" })
            {
                var t = R($"p{n}_title"); var x = R($"p{n}_text");
                if (t.Length > 0 || x.Length > 0) points.Add(new { title = t, text = x });
            }
            // CTA href is admin-authored; only ever emit a relative path or an http(s) URL.
            var href = Val("cta_href").Trim();
            if (href.Contains(':') && !href.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                && !href.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                href = "honorary-application.html";
            return J(new
            {
                enabled = true,
                version = Val("version"),
                // Language-stable dismissal key (English date): dismissing in one language dismisses in all.
                key = Val("version") + "." + Val("date"),
                date,
                eyebrow = R("eyebrow"),
                title = R("title"),
                subtitle = R("subtitle"),
                intro = R("intro"),
                points,
                note = R("note"),
                dismiss = R("dismiss"),
                cta = new { label = R("cta_label"), href },
            });
        });

        // ---------- admin ----------
        app.MapGet("/api/admin/announcement", (HttpRequest req) => gate(req, "content", _ =>
        {
            var o = new Dictionary<string, object?> { ["announce_enabled"] = Enabled() };
            foreach (var (k, _) in Fields) o["announce_" + k] = S(k).Length > 0 ? S(k) : Def(k);
            return J(o);
        }));

        app.MapPost("/api/admin/announcement", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "content", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actorId = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            void Put(string key, string v) { db.Execute("DELETE FROM site_settings WHERE skey=?", key); db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", key, v); }

            foreach (var (k, _) in Fields)                            // only overwrite fields present in the body
                if (H.GetS(b, k) is { } v) Put("announce_" + k, v.Trim());
            if (H.GetEl(b, "announce_enabled") is { } en)
            {
                var on = en.ValueKind switch
                {
                    System.Text.Json.JsonValueKind.True => true,
                    System.Text.Json.JsonValueKind.False => false,
                    System.Text.Json.JsonValueKind.Number => en.GetDouble() != 0,
                    System.Text.Json.JsonValueKind.String => en.GetString() is "1" or "true" or "on",
                    _ => Enabled(),
                };
                Put("announce_enabled", on ? "1" : "0");
            }
            log(actorId, "announcement_update", string.Join(",", b.Keys));
            return J(new { ok = true });
        });
    }
}
