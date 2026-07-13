using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Backend-owned control of public-website translations. Everything about the multilingual site — which
/// languages, which strings, machine-translate vs hand-edit, the provider and its key — is managed here
/// from the admin panel and stored in <c>content_i18n</c>; nothing is a shipped static file. Owner-only:
/// translations are site-wide public content.
///
///   GET  /api/admin/i18n/coverage            — per-language progress + provider status
///   GET  /api/admin/i18n/regions?slug=&lang=  — a page's translatable regions + current translations
///   POST /api/admin/i18n/set                 — hand-edit one translation
///   POST /api/admin/i18n/translate           — auto-translate (one language, optional page) via provider
///   POST /api/admin/i18n/clear               — remove translations (to redo)
///   POST /api/admin/i18n/config              — set provider / key / model / endpoint
/// </summary>
public static class AdminI18n
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        (AdminCtx? adm, IResult? deny) Owner(HttpRequest req)
        {
            var a = Auth.AdminFromReq(req, db);
            if (a is null) return (null, Results.Json(new { error = "unauthorised" }, statusCode: 401));
            if (!a.IsOwner) return (null, Results.Json(new { error = "forbidden" }, statusCode: 403));
            return (a, null);
        }
        bool ValidLang(string? l) => l is not null && Translator.LangNames.ContainsKey(l);

        // ---- coverage + provider status ----
        app.MapGet("/api/admin/i18n/coverage", (HttpRequest req) =>
        {
            var (_, deny) = Owner(req); if (deny is not null) return deny;
            var cfg = Translator.Load(db);
            var pages = db.Query("SELECT DISTINCT slug FROM page_blocks ORDER BY slug").Select(r => H.Str(r["slug"])).Where(s => s is { Length: > 0 }).ToList();
            return J(new
            {
                coverage = I18nContent.Coverage(db),
                languages = Translator.LangNames.Select(kv => new { code = kv.Key, name = kv.Value }),
                provider = new { provider = cfg.Provider, model = cfg.Model, endpoint = cfg.Endpoint, has_key = cfg.ApiKey.Length > 0, configured = Translator.IsConfigured(db) },
                pages,
            });
        });

        // ---- one page's regions + current translations for a language ----
        app.MapGet("/api/admin/i18n/regions", (HttpRequest req) =>
        {
            var (_, deny) = Owner(req); if (deny is not null) return deny;
            var slug = (req.Query["slug"].ToString() ?? "").Trim();
            var lang = (req.Query["lang"].ToString() ?? "").Trim();
            if (!ValidLang(lang)) return Results.Json(new { error = "bad_lang" }, statusCode: 400);
            if (slug.Length == 0 || slug.Contains("..")) return Results.Json(new { error = "bad_slug" }, statusCode: 400);
            var existing = I18nContent.Existing(db, lang, slug);
            var rows = I18nContent.Sources(db, slug).Select(s => new
            {
                scope = s.Scope, slug = s.Slug, ckey = s.Key, ctype = s.CType, english = s.English,
                translation = existing.TryGetValue(s.Scope + "" + s.Slug + "" + s.Key, out var v) ? v : null,
            });
            return J(new { slug, lang, rows });
        });

        // ---- hand-edit one translation ----
        app.MapPost("/api/admin/i18n/set", async (HttpContext ctx) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var lang = H.GetS(b, "lang"); var scope = H.GetS(b, "scope"); var slug = H.GetS(b, "slug") ?? "";
            var ckey = H.GetS(b, "ckey"); var cvalue = H.GetS(b, "cvalue");
            if (!ValidLang(lang)) return Results.Json(new { error = "bad_lang" }, statusCode: 400);
            if (scope is not ("p" or "g" or "nav" or "meta") || string.IsNullOrEmpty(ckey))
                return Results.Json(new { error = "bad_key" }, statusCode: 400);
            I18nContent.Upsert(db, lang!, scope, slug, ckey!, cvalue);
            log(adm!.Id, "i18n.set", $"{lang}:{scope}:{slug}:{ckey}");
            return J(new { ok = true });
        });

        // ---- auto-translate one language (optionally one page) via the configured provider ----
        app.MapPost("/api/admin/i18n/translate", async (HttpContext ctx) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var lang = H.GetS(b, "lang");
            if (!ValidLang(lang)) return Results.Json(new { error = "bad_lang" }, statusCode: 400);
            if (!Translator.IsConfigured(db))
                return Results.Json(new { error = "provider_not_configured", message = "Set a translation provider and API key first (in this section's provider settings)." }, statusCode: 400);
            var slug = H.GetS(b, "slug");
            if (slug is not null && (slug.Length == 0 || slug.Contains(".."))) slug = null;
            var overwrite = H.GetEl(b, "overwrite") is { ValueKind: System.Text.Json.JsonValueKind.True };
            var limit = (int)(H.GetNum(b, "limit") ?? 250); limit = Math.Clamp(limit, 1, 1000);

            var existing = I18nContent.Existing(db, lang!, slug);
            var srcs = I18nContent.Sources(db, slug);
            string K(I18nContent.Src s) => s.Scope + "" + s.Slug + "" + s.Key;
            var pending = srcs.Where(s => overwrite || !existing.ContainsKey(K(s))).ToList();
            var remainingBefore = pending.Count;
            var batch = pending.Take(limit).ToList();

            int done = 0;
            for (int i = 0; i < batch.Count; i += 40)
            {
                var chunk = batch.GetRange(i, Math.Min(40, batch.Count - i));
                var outs = Translator.Translate(db, chunk.Select(s => s.English).ToList(), lang!);
                for (int j = 0; j < chunk.Count; j++)
                {
                    var tr = outs[j];
                    if (string.IsNullOrWhiteSpace(tr)) continue;
                    I18nContent.Upsert(db, lang!, chunk[j].Scope, chunk[j].Slug, chunk[j].Key, tr);
                    done++;
                }
            }
            log(adm!.Id, "i18n.translate", $"{lang}{(slug is null ? "" : ":" + slug)} +{done}");
            return J(new { ok = true, lang, translated = done, remaining = Math.Max(0, remainingBefore - done) });
        });

        // ---- clear translations (whole language or one page) to redo ----
        app.MapPost("/api/admin/i18n/clear", async (HttpContext ctx) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var lang = H.GetS(b, "lang");
            if (!ValidLang(lang)) return Results.Json(new { error = "bad_lang" }, statusCode: 400);
            var slug = H.GetS(b, "slug");
            if (slug is { Length: > 0 } && !slug.Contains(".."))
                db.Execute("DELETE FROM content_i18n WHERE lang=? AND scope='p' AND slug=?", lang, slug);
            else
                db.Execute("DELETE FROM content_i18n WHERE lang=?", lang);
            I18nContent.Bump();
            log(adm!.Id, "i18n.clear", $"{lang}{(slug is { Length: > 0 } ? ":" + slug : " (all)")}");
            return J(new { ok = true });
        });

        // ---- provider config (key write-only; never returned) ----
        app.MapPost("/api/admin/i18n/config", async (HttpContext ctx) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            void Set(string k, string? v) { db.Execute("DELETE FROM site_settings WHERE skey=?", k); db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?)", k, v ?? ""); }
            var provider = (H.GetS(b, "provider") ?? "").Trim().ToLowerInvariant();
            if (provider is not ("" or "anthropic" or "openai" or "custom"))
                return Results.Json(new { error = "bad_provider" }, statusCode: 400);
            Set("translate_provider", provider);
            Set("translate_model", H.GetS(b, "model"));
            Set("translate_endpoint", H.GetS(b, "endpoint"));
            var key = H.GetS(b, "api_key");                       // only overwrite when a new key is supplied
            if (key is { Length: > 0 }) Set("translate_api_key", key);
            log(adm!.Id, "i18n.config", provider);
            return J(new { ok = true, configured = Translator.IsConfigured(db) });
        });
    }
}
