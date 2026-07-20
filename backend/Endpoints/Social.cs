using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Social Media Management — the database-backed configuration surface for every public social-media
/// profile and every page-sharing button. The DB (social_accounts, social_share_settings,
/// social_audit, social_link_checks) is the source of truth; nothing is hardcoded in React.
///
///   Public
///     GET  /api/social?loc=footer   — qualifying profile links for a location (icons + a11y metadata)
///     GET  /api/social/share?type=  — enabled page-sharing buttons for a content type
///   Admin (permission 'social')
///     GET  /api/admin/social                      — accounts, platform registry, locations, share config, audit
///     POST /api/admin/social                      — master toggle + analytics toggle
///     POST /api/admin/social/account              — create / update (validated, audited)
///     POST /api/admin/social/account/{id}/delete  — archive (soft) or hard delete
///     POST /api/admin/social/account/{id}/duplicate
///     POST /api/admin/social/account/{id}/activate
///     POST /api/admin/social/account/{id}/check   — run a broken-link validation
///     POST /api/admin/social/share                — save page-sharing buttons per content type
///     GET  /api/admin/social/audit                — change history
///
/// Every write validates the URL (scheme + platform domain) via Core/SocialIcons, records a
/// social_audit row, and bumps the ListSections render cache so the public footer updates at once.
/// </summary>
public static class Social
{
    static readonly HttpClient Http = Egress.CreateClient(TimeSpan.FromSeconds(8));

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
        void SetSetting(string k, string v) { db.Execute("DELETE FROM site_settings WHERE skey=?", k); db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", k, v); }
        void Audit(long? actor, long? accountId, string action, string? detail)
        {
            db.Execute("INSERT INTO social_audit(account_id,actor_id,action,detail) VALUES(?,?,?,?)", accountId, actor, action, detail ?? "");
            log(actor, "social_" + action, detail);
        }

        // ─────────────────────────── public ───────────────────────────
        app.MapGet("/api/social", (HttpRequest req) =>
        {
            var loc = req.Query["loc"].ToString();
            if (string.IsNullOrWhiteSpace(loc)) loc = "footer";
            var links = SocialLinks.ForLocation(db, loc).Select(l => new
            {
                platform = l.Platform, url = l.Url, label = l.Label, tooltip = l.Tooltip,
                svg = l.Svg, rel = l.Rel, target = l.Target, official = l.Official,
                handle = l.Handle, color = l.Color,
            });
            return J(new { enabled = SocialLinks.Enabled(db), analytics = SocialLinks.AnalyticsEnabled(db), links });
        });

        app.MapGet("/api/social/share", (HttpRequest req) =>
        {
            var type = req.Query["type"].ToString();
            if (string.IsNullOrWhiteSpace(type)) type = "blog";
            var row = db.QueryOne("SELECT buttons,enabled FROM social_share_settings WHERE content_type=?", type);
            var enabled = row is not null && H.B(row["enabled"]);
            var buttons = enabled ? (H.Str(row!["buttons"]) ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) : Array.Empty<string>();
            return J(new { enabled, buttons });
        });

        // ─────────────────────────── admin: read ───────────────────────────
        app.MapGet("/api/admin/social", (HttpRequest req) => gate(req, "social", _ =>
        {
            var accounts = db.Query("SELECT * FROM social_accounts ORDER BY display_order, id");
            var share = db.Query("SELECT content_type,buttons,enabled FROM social_share_settings ORDER BY content_type");
            var audit = db.Query(@"SELECT sa.*, au.name AS actor_name FROM social_audit sa
                LEFT JOIN admin_users au ON au.id=sa.actor_id ORDER BY sa.id DESC LIMIT 100");
            return J(new
            {
                enabled = S("social_enabled") == "1",
                analytics = S("social_analytics_enabled") != "0",
                accounts,
                platforms = SocialIcons.All.Select(p => new { key = p.Key, label = p.Label, color = p.Color, domains = p.Domains, svg = SocialIcons.Svg(p.Key, "builtin", null) }),
                locations = SocialIcons.Locations.Select(l => new { key = l.Key, label = l.Label }),
                share_targets = SocialIcons.ShareTargets.Select(l => new { key = l.Key, label = l.Label }),
                share,
                audit,
            });
        }));

        app.MapGet("/api/admin/social/audit", (HttpRequest req) => gate(req, "social", _ =>
            J(new { rows = db.Query(@"SELECT sa.*, au.name AS actor_name FROM social_audit sa
                LEFT JOIN admin_users au ON au.id=sa.actor_id ORDER BY sa.id DESC LIMIT 300") })));

        // ─────────────────────────── admin: settings ───────────────────────────
        app.MapPost("/api/admin/social", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "social", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actor = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            if (H.GetBool(b, "social_enabled", "enabled") is { } en) SetSetting("social_enabled", en ? "1" : "0");
            if (H.GetBool(b, "social_analytics_enabled", "analytics") is { } an) SetSetting("social_analytics_enabled", an ? "1" : "0");
            Audit(actor, null, "settings", string.Join(",", b.Keys));
            ListSections.Bump();
            return J(new { ok = true });
        });

        // ─────────────────────────── admin: create / update ───────────────────────────
        app.MapPost("/api/admin/social/account", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "social", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actor = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);

            var id = (long?)H.GetNum(b, "id");
            var platform = (H.GetS(b, "platform") ?? "custom").Trim().ToLowerInvariant();
            var url = (H.GetS(b, "url") ?? "").Trim();
            var overrideDomain = H.GetBool(b, "override_domain") == true;

            var check = SocialIcons.ValidateUrl(platform, url);
            if (!check.Ok) return J(new { error = check.Error });
            // A domain-mismatch warning blocks publication unless an authorised override is supplied.
            if (check.Warning is { } w && w.Contains("does not look like") && !overrideDomain)
                return Results.Json(new { error = w, warning = w, needs_override = true }, statusCode: 422);

            var iconType = (H.GetS(b, "icon_type") ?? "builtin").Trim().ToLowerInvariant();
            var customIcon = iconType == "custom" ? (SocialIcons.SafeCustomIcon(H.GetS(b, "custom_icon"))) : null;
            if (iconType == "custom" && customIcon is null) return J(new { error = "The custom icon must be a safe inline SVG (or a path definition)." });

            // locations may arrive as an array or a CSV string.
            var locations = ReadCsv(b, "locations");
            if (string.IsNullOrWhiteSpace(locations)) locations = "footer";

            string? Sv(string k) => H.GetS(b, k);
            int I(string k, int def) => (int)(H.GetNum(b, k) ?? def);
            int Bit(string k, bool def) => (H.GetBool(b, k) ?? def) ? 1 : 0;
            var approval = (H.GetS(b, "approval_status") ?? "approved").Trim().ToLowerInvariant();
            if (approval is not ("draft" or "approved" or "archived")) approval = "approved";

            if (id is > 0)
            {
                db.Execute(@"UPDATE social_accounts SET platform=?,display_name=?,handle=?,url=?,icon_type=?,custom_icon=?,
                    aria_label=?,tooltip=?,locations=?,display_order=?,active=?,is_official=?,open_new_tab=?,rel_nofollow=?,
                    language=?,country=?,effective_date=?,expiry_date=?,approval_status=?,updated_by=?,updated_at=datetime('now')
                    WHERE id=?",
                    platform, Sv("display_name"), Sv("handle"), check.Url, iconType, customIcon,
                    Sv("aria_label"), Sv("tooltip"), locations, I("display_order", 0), Bit("active", true), Bit("is_official", true),
                    Bit("open_new_tab", true), Bit("rel_nofollow", true), Sv("language"), Sv("country"),
                    Sv("effective_date"), Sv("expiry_date"), approval, actor, id);
                Audit(actor, id, "update", $"{platform} {check.Url}" + (overrideDomain ? " [domain override]" : ""));
            }
            else
            {
                var maxOrd = db.Scalar<long>("SELECT COALESCE(MAX(display_order),0) FROM social_accounts");
                id = db.ExecuteReturningId(@"INSERT INTO social_accounts(platform,display_name,handle,url,icon_type,custom_icon,
                    aria_label,tooltip,locations,display_order,active,is_official,open_new_tab,rel_nofollow,language,country,
                    effective_date,expiry_date,approval_status,created_by,updated_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    platform, Sv("display_name"), Sv("handle"), check.Url, iconType, customIcon,
                    Sv("aria_label"), Sv("tooltip"), locations, I("display_order", (int)maxOrd + 1), Bit("active", true), Bit("is_official", true),
                    Bit("open_new_tab", true), Bit("rel_nofollow", true), Sv("language"), Sv("country"),
                    Sv("effective_date"), Sv("expiry_date"), approval, actor, actor);
                Audit(actor, id, "create", $"{platform} {check.Url}" + (overrideDomain ? " [domain override]" : ""));
            }
            ListSections.Bump();
            return J(new { ok = true, id, warning = check.Warning });
        });

        // ─────────────────────────── admin: lifecycle ───────────────────────────
        app.MapPost("/api/admin/social/account/{id:long}/delete", async (long id, HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "social", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actor = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var hard = H.GetBool(b, "hard") == true;
            if (hard) { db.Execute("DELETE FROM social_accounts WHERE id=?", id); Audit(actor, id, "delete", "hard"); }
            else { db.Execute("UPDATE social_accounts SET approval_status='archived',active=0,updated_by=?,updated_at=datetime('now') WHERE id=?", actor, id); Audit(actor, id, "archive", null); }
            ListSections.Bump();
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/social/account/{id:long}/duplicate", (long id, HttpRequest req) => gate(req, "social", a =>
        {
            var src = db.QueryOne("SELECT * FROM social_accounts WHERE id=?", id);
            if (src is null) return J(new { error = "not found" });
            var maxOrd = db.Scalar<long>("SELECT COALESCE(MAX(display_order),0) FROM social_accounts");
            var newId = db.ExecuteReturningId(@"INSERT INTO social_accounts(platform,display_name,handle,url,icon_type,custom_icon,
                aria_label,tooltip,locations,display_order,active,is_official,open_new_tab,rel_nofollow,language,country,
                effective_date,expiry_date,approval_status,created_by,updated_by)
                SELECT platform,display_name,handle,url,icon_type,custom_icon,aria_label,tooltip,locations,?,0,is_official,
                open_new_tab,rel_nofollow,language,country,effective_date,expiry_date,'draft',?,? FROM social_accounts WHERE id=?",
                (int)maxOrd + 1, a.Id, a.Id, id);
            Audit(a.Id, newId, "duplicate", $"from #{id}");
            ListSections.Bump();
            return J(new { ok = true, id = newId });
        }));

        app.MapPost("/api/admin/social/account/{id:long}/activate", async (long id, HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "social", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actor = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            var active = H.GetBool(b, "active") ?? true;
            db.Execute("UPDATE social_accounts SET active=?,updated_by=?,updated_at=datetime('now') WHERE id=?", active ? 1 : 0, actor, id);
            Audit(actor, id, active ? "activate" : "deactivate", null);
            ListSections.Bump();
            return J(new { ok = true });
        });

        // ─────────────────────────── admin: link validation ───────────────────────────
        app.MapPost("/api/admin/social/account/{id:long}/check", (long id, HttpRequest req) => gate(req, "social", a =>
        {
            var row = db.QueryOne("SELECT platform,url FROM social_accounts WHERE id=?", id);
            if (row is null) return J(new { error = "not found" });
            var (status, code, detail) = CheckLink(H.Str(row["platform"]) ?? "custom", H.Str(row["url"]) ?? "");
            db.Execute("INSERT INTO social_link_checks(account_id,status,http_code,detail) VALUES(?,?,?,?)", id, status, code, detail);
            db.Execute("UPDATE social_accounts SET link_status=?,link_checked_at=datetime('now') WHERE id=?", status, id);
            Audit(a.Id, id, "check", $"{status} ({code}) {detail}");
            ListSections.Bump();                                             // a now-invalid link must drop from the footer
            return J(new { ok = true, status, http_code = code, detail });
        }));

        // ─────────────────────────── admin: sharing config ───────────────────────────
        app.MapPost("/api/admin/social/share", async (HttpContext ctx) =>
        {
            var denied = gate(ctx.Request, "social", _ => Results.Ok());
            if (denied is not Microsoft.AspNetCore.Http.HttpResults.Ok) return denied;
            var actor = Auth.AdminFromReq(ctx.Request, db)?.Id;
            var b = await H.Body(ctx.Request);
            if (H.GetEl(b, "rows") is JsonElement arr && arr.ValueKind == JsonValueKind.Array)
            {
                var allowed = SocialIcons.ShareTargets.Select(t => t.Key).ToHashSet(StringComparer.OrdinalIgnoreCase);
                foreach (var row in arr.EnumerateArray())
                {
                    var ct = row.TryGetProperty("content_type", out var c) ? c.GetString() : null;
                    if (string.IsNullOrWhiteSpace(ct)) continue;
                    var buttons = row.TryGetProperty("buttons", out var bt) && bt.ValueKind == JsonValueKind.Array
                        ? string.Join(",", bt.EnumerateArray().Select(x => x.GetString() ?? "").Where(x => allowed.Contains(x)))
                        : "";
                    var enabled = row.TryGetProperty("enabled", out var e) && (e.ValueKind == JsonValueKind.True || (e.ValueKind == JsonValueKind.Number && e.GetInt32() != 0));
                    db.Execute("DELETE FROM social_share_settings WHERE content_type=?", ct);
                    db.Execute("INSERT INTO social_share_settings(content_type,buttons,enabled) VALUES(?,?,?)", ct, buttons, enabled ? 1 : 0);
                }
            }
            Audit(actor, null, "share", null);
            return J(new { ok = true });
        });
    }

    /// <summary>Read a body field that may be a JSON array of strings or a CSV string → normalised CSV.</summary>
    static string ReadCsv(Dictionary<string, JsonElement> b, string key)
    {
        if (!b.TryGetValue(key, out var v)) return "";
        if (v.ValueKind == JsonValueKind.Array)
            return string.Join(",", v.EnumerateArray().Select(x => (x.GetString() ?? "").Trim()).Where(x => x.Length > 0));
        var s = v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        return string.Join(",", s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    /// <summary>Broken-link validation. Returns a status from the account model's vocabulary. A
    /// third-party platform that merely rate-limits/blocks bots is reported as 'warning', never
    /// 'invalid', so we don't flag a healthy profile as broken.</summary>
    static (string status, int code, string detail) CheckLink(string platform, string url)
    {
        var check = SocialIcons.ValidateUrl(platform, url);
        if (!check.Ok) return ("invalid", 0, check.Error ?? "malformed URL");
        try
        {
            var req = new HttpRequestMessage(HttpMethod.Get, check.Url);
            req.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (compatible; PCI-LinkCheck/1.0)");
            using var resp = Http.Send(req, HttpCompletionOption.ResponseHeadersRead);
            var code = (int)resp.StatusCode;
            if (code >= 200 && code < 400) return (check.Warning is null ? "valid" : "warning", code, check.Warning ?? "OK");
            if (code is 401 or 403 or 429 or 405) return ("warning", code, "Reachable but the platform blocked automated checking.");
            if (code == 404 || code == 410) return ("invalid", code, "Page not found.");
            return ("warning", code, $"Unexpected response {code}.");
        }
        catch (Exception e) { return ("unreachable", 0, e.Message.Length > 200 ? e.Message[..200] : e.Message); }
    }
}
