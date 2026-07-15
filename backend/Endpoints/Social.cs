using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin-controlled social-media links shown in the public footer.
///
///   GET  /api/social         — public: only enabled + non-empty entries (no admin data)
///   GET  /api/admin/social   — admin (content perm): every stored value + master toggle
///   POST /api/admin/social   — admin (content perm): update links / toggle
///
/// Values live in site_settings (skey 'social_&lt;network&gt;' + 'social_enabled'), following the
/// same key/value settings pattern as SEO integrations and notifications. Only http(s) URLs are
/// ever emitted publicly.
/// </summary>
public static class Social
{
    /// <summary>Supported networks, in display order.</summary>
    public static readonly string[] Keys = { "linkedin", "x", "facebook", "instagram", "youtube", "whatsapp" };

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
        bool Enabled() => S("social_enabled") == "1";

        // ---------- public ----------
        app.MapGet("/api/social", () =>
        {
            var links = new Dictionary<string, string>();
            if (Enabled())
                foreach (var k in Keys)
                {
                    var v = S("social_" + k).Trim();
                    if (v.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        || v.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        links[k] = v;
                }
            return J(new { links });
        });

        // ---------- admin ----------
        app.MapGet("/api/admin/social", (HttpRequest req) => gate(req, "content", _ =>
        {
            var o = new Dictionary<string, object?> { ["social_enabled"] = Enabled() };
            foreach (var k in Keys) o[k] = S("social_" + k);
            return J(o);
        }));

        app.MapPost("/api/admin/social", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "content", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var b = await H.Body(ctx.Request);
            void Set(string skey, string v)
            {
                db.Execute("DELETE FROM site_settings WHERE skey=?", skey);
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", skey, v);
            }
            foreach (var k in Keys)                                   // only overwrite keys present in the body
                if (H.GetS(b, k) is { } v) Set("social_" + k, v.Trim());
            if (H.GetEl(b, "social_enabled") is { } en)
            {
                var on = en.ValueKind switch
                {
                    System.Text.Json.JsonValueKind.True => true,
                    System.Text.Json.JsonValueKind.False => false,
                    System.Text.Json.JsonValueKind.Number => en.GetDouble() != 0,
                    System.Text.Json.JsonValueKind.String => en.GetString() is "1" or "true" or "on",
                    _ => false,
                };
                Set("social_enabled", on ? "1" : "0");
            }
            log(null, "social_update", string.Join(",", b.Keys));
            return J(new { ok = true });
        });
    }
}
