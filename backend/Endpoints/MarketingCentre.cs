using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Marketing, Ads and Search Console centre — admin API (Phase 1 foundation).
///
/// Honesty first: capabilities report their true access status (mostly provider-approval-required /
/// not-connected) and gated actions (publish a LinkedIn post, submit a sitemap, launch a paid campaign)
/// are BLOCKED with a clear operator action until a real, approved connection exists. Provider SECRETS
/// are never returned — the providers endpoint reports booleans only, and OAuth tokens live encrypted
/// and never leave the backend. Permissions are enforced here in .NET (owner bypasses), not just in React.
/// </summary>
public static class MarketingCentre
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        string S(Dictionary<string, JsonElement> b, params string[] k) => (H.GetS(b, k) ?? "").Trim();
        double? N(Dictionary<string, JsonElement> b, string k) => H.GetNum(b, k);
        int I(Dictionary<string, JsonElement> b, string k) => (int)(H.GetNum(b, k) ?? 0);
        static string? Nz(string s) => string.IsNullOrWhiteSpace(s) ? null : s;

        // ───────── dashboard + honest registry ─────────
        app.MapGet("/api/admin/marketing/overview", (HttpRequest req) => gate(req, "mkt_view", _ => J(new
        {
            counts = new
            {
                connections = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE status='connected'"),
                campaigns = db.Scalar<long>("SELECT COUNT(*) FROM mkt_campaigns"),
                posts = db.Scalar<long>("SELECT COUNT(*) FROM mkt_linkedin_posts"),
                promotions = db.Scalar<long>("SELECT COUNT(*) FROM mkt_promotions WHERE status='active'"),
                leads = db.Scalar<long>("SELECT COUNT(*) FROM mkt_leads"),
                open_alerts = db.Scalar<long>("SELECT COUNT(*) FROM mkt_alerts WHERE status='open'"),
            },
            capability_summary = db.Query("SELECT status, COUNT(*) n FROM mkt_capabilities GROUP BY status"),
            alerts = db.Query("SELECT * FROM mkt_alerts WHERE status='open' ORDER BY id DESC LIMIT 20"),
        })));

        // Capability registry with a live effective status (never inflates an approval-gated feature).
        app.MapGet("/api/admin/marketing/capabilities", (HttpRequest req) => gate(req, "mkt_view", _ =>
        {
            var live = db.Query("SELECT DISTINCT platform_code FROM mkt_connections WHERE status='connected'")
                .Select(r => H.Str(r["platform_code"]) ?? "").ToHashSet();
            var rows = db.Query(@"SELECT c.*, p.name platform_name, p.family, p.official_api, p.docs_url
                FROM mkt_capabilities c LEFT JOIN mkt_platforms p ON p.code=c.platform_code
                ORDER BY p.sort_order, p.family, c.platform_code, c.feature")
                .Select(r =>
                {
                    var declared = H.Str(r["status"]) ?? "not_connected";
                    var hasConn = live.Contains(H.Str(r["platform_code"]) ?? "");
                    r["effective_status"] = Marketing.EffectiveStatus(declared, hasConn);
                    r["connected"] = hasConn ? 1 : 0;
                    return r;
                }).ToList();
            return J(new { rows });
        }));

        // Provider configuration — BOOLEANS ONLY. Never returns a secret value.
        app.MapGet("/api/admin/marketing/providers", (HttpRequest req) => gate(req, "mkt_view", _ => J(Marketing.ProviderConfig())));

        app.MapGet("/api/admin/marketing/platforms", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_platforms WHERE active=1 ORDER BY sort_order, family, name") })));

        // ───────── connected accounts (metadata only — tokens never returned) ─────────
        const string CONN_COLS = "id,platform_code,label,external_org_id,external_ad_account_id,external_page_id,external_ig_id,external_business_id,external_property,connected_user_ref,granted_scopes,roles,access_tier,api_version,account_currency,account_timezone,token_expires_at,status,approval_status,last_success_at,last_failure_at,last_error,connected_by,created_at,updated_at";
        app.MapGet("/api/admin/marketing/connections", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query($"SELECT {CONN_COLS} FROM mkt_connections ORDER BY platform_code, id") })));

        app.MapPost("/api/admin/marketing/connections", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_connect", adm =>
            {
                var platform = S(b, "platform_code");
                var pf = db.QueryOne("SELECT * FROM mkt_platforms WHERE code=?", platform);
                if (pf is null) return Results.Json(new { error = "unknown_platform" }, statusCode: 400);
                var id = db.ExecuteReturningId(@"INSERT INTO mkt_connections
                    (platform_code,label,external_org_id,external_ad_account_id,external_page_id,external_ig_id,external_business_id,external_property,status,approval_status,connected_by)
                    VALUES(?,?,?,?,?,?,?,?, 'disconnected','not_requested',?)",
                    platform, Nz(S(b, "label")), Nz(S(b, "external_org_id")), Nz(S(b, "external_ad_account_id")),
                    Nz(S(b, "external_page_id")), Nz(S(b, "external_ig_id")), Nz(S(b, "external_business_id")), Nz(S(b, "external_property")), adm.Id);
                log(adm.Id, "mkt_connection_registered", $"{platform} #{id}");
                return J(new { ok = true, id, family_configured = Marketing.FamilyConfigured(H.Str(pf["family"]) ?? "") });
            });
        });

        // Begin OAuth — honest: reports whether the provider app is even configured in Render env.
        app.MapPost("/api/admin/marketing/connections/{id:long}/oauth-url", (HttpRequest req, long id) => gate(req, "mkt_connect", adm =>
        {
            var c = db.QueryOne("SELECT * FROM mkt_connections WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var family = H.Str(db.QueryOne("SELECT family FROM mkt_platforms WHERE code=?", H.Str(c["platform_code"]))?["family"]) ?? "";
            if (!Marketing.FamilyConfigured(family))
                return J(new { ok = false, reason = "provider_not_configured", operator_action = $"Set the {family} OAuth client id and secret in Render environment variables before connecting." });
            // Live OAuth redirect construction is Phase 2 (needs the approved developer app + redirect URL).
            return J(new { ok = false, reason = "oauth_pending_setup", operator_action = "Provider app configured. Live OAuth authorisation flow is enabled once the redirect URL is registered with the provider (Phase 2)." });
        }));

        app.MapPost("/api/admin/marketing/connections/{id:long}/disconnect", (HttpRequest req, long id) => gate(req, "mkt_connect", adm =>
        {
            db.Execute("UPDATE mkt_connections SET status='disconnected', access_token_enc=NULL, refresh_token_enc=NULL, token_expires_at=NULL, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "mkt_connection_disconnected", $"#{id}");
            return J(new { ok = true });
        }));

        // ───────── promotions ─────────
        app.MapGet("/api/admin/marketing/promotions", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_promotions ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/promotions", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_promos", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var id = I(b, "id");
                if (id > 0)
                {
                    db.Execute(@"UPDATE mkt_promotions SET name=?,code=?,promo_type=?,fee_type=?,original_amount=?,discount_amount=?,discount_percent=?,net_amount=?,currency=?,start_date=?,end_date=?,countries=?,languages=?,usage_limit=?,per_user_limit=?,status=?,updated_at=datetime('now') WHERE id=?",
                        name, Nz(S(b, "code")), Nz(S(b, "promo_type")), Nz(S(b, "fee_type")), N(b, "original_amount"), N(b, "discount_amount"), N(b, "discount_percent"), N(b, "net_amount"),
                        Nz(S(b, "currency")) ?? "USD", Nz(S(b, "start_date")), Nz(S(b, "end_date")), Nz(S(b, "countries")), Nz(S(b, "languages")),
                        b.ContainsKey("usage_limit") ? (long?)I(b, "usage_limit") : null, b.ContainsKey("per_user_limit") ? (long?)I(b, "per_user_limit") : null,
                        Nz(S(b, "status")) ?? "draft", id);
                    log(adm.Id, "mkt_promotion_update", $"#{id}");
                    return J(new { ok = true, id });
                }
                var nid = db.ExecuteReturningId(@"INSERT INTO mkt_promotions(name,code,promo_type,fee_type,original_amount,discount_amount,discount_percent,net_amount,currency,start_date,end_date,countries,languages,usage_limit,per_user_limit,status,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                    name, Nz(S(b, "code")), Nz(S(b, "promo_type")), Nz(S(b, "fee_type")), N(b, "original_amount"), N(b, "discount_amount"), N(b, "discount_percent"), N(b, "net_amount"),
                    Nz(S(b, "currency")) ?? "USD", Nz(S(b, "start_date")), Nz(S(b, "end_date")), Nz(S(b, "countries")), Nz(S(b, "languages")),
                    b.ContainsKey("usage_limit") ? (long?)I(b, "usage_limit") : null, b.ContainsKey("per_user_limit") ? (long?)I(b, "per_user_limit") : null,
                    Nz(S(b, "status")) ?? "draft", adm.Id);
                log(adm.Id, "mkt_promotion_create", $"#{nid} {name}");
                return J(new { ok = true, id = nid });
            });
        });

        // ───────── LinkedIn organic posts (draft → approve → publish, publish honestly gated) ─────────
        app.MapGet("/api/admin/marketing/posts", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_linkedin_posts ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/posts", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_posts", adm =>
            {
                var id = I(b, "id");
                if (id > 0)
                {
                    db.Execute(@"UPDATE mkt_linkedin_posts SET post_type=?,body=?,article_title=?,article_url=?,alt_text=?,hashtags=?,cta=?,audience_note=?,language=?,scheduled_at=?,timezone=?,updated_at=datetime('now') WHERE id=?",
                        Nz(S(b, "post_type")) ?? "text", Nz(S(b, "body")), Nz(S(b, "article_title")), Nz(S(b, "article_url")), Nz(S(b, "alt_text")),
                        Nz(S(b, "hashtags")), Nz(S(b, "cta")), Nz(S(b, "audience_note")), Nz(S(b, "language")), Nz(S(b, "scheduled_at")), Nz(S(b, "timezone")), id);
                    log(adm.Id, "mkt_post_update", $"#{id}");
                    return J(new { ok = true, id });
                }
                var nid = db.ExecuteReturningId(@"INSERT INTO mkt_linkedin_posts(post_type,body,article_title,article_url,alt_text,hashtags,cta,audience_note,language,scheduled_at,timezone,status,approval_status,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?, 'draft','draft',?)",
                    Nz(S(b, "post_type")) ?? "text", Nz(S(b, "body")), Nz(S(b, "article_title")), Nz(S(b, "article_url")), Nz(S(b, "alt_text")),
                    Nz(S(b, "hashtags")), Nz(S(b, "cta")), Nz(S(b, "audience_note")), Nz(S(b, "language")), Nz(S(b, "scheduled_at")), Nz(S(b, "timezone")), adm.Id);
                log(adm.Id, "mkt_post_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });
        app.MapPost("/api/admin/marketing/posts/{id:long}/approve", (HttpRequest req, long id) => gate(req, "mkt_approve", adm =>
        {
            db.Execute("UPDATE mkt_linkedin_posts SET approval_status='approved', status=CASE WHEN status='draft' THEN 'approved' ELSE status END, updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "mkt_post_approved", $"#{id}");
            return J(new { ok = true });
        }));
        app.MapPost("/api/admin/marketing/posts/{id:long}/publish", (HttpRequest req, long id) => gate(req, "mkt_publish", adm =>
        {
            var p = db.QueryOne("SELECT * FROM mkt_linkedin_posts WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(p["approval_status"]) != "approved") return Results.Json(new { error = "not_approved", detail = "Post must be approved before publishing." }, statusCode: 400);
            // HONEST GATE: only publish when the organisation-posting capability is actually available on a
            // live connection. Until LinkedIn grants Community Management access, this returns a clear action.
            var cap = db.QueryOne("SELECT status FROM mkt_capabilities WHERE platform_code='linkedin_page' AND feature='Organisation page posts'");
            var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code='linkedin_page' AND status='connected'") > 0;
            var eff = Marketing.EffectiveStatus(H.Str(cap?["status"]) ?? "provider_approval_required", hasConn);
            if (eff is not ("available" or "available_with_permission"))
            {
                db.Execute("UPDATE mkt_linkedin_posts SET status='scheduled', updated_at=datetime('now') WHERE id=?", id);
                return J(new { ok = false, queued = true, reason = eff, operator_action = "LinkedIn organisation posting is not yet available. Post marked scheduled; it will publish once the LinkedIn Company Page connection and Community Management access are approved (Phase 2)." });
            }
            // (Phase 2: enqueue a mkt_jobs row for the LinkedIn Posts API call with an idempotency key.)
            db.Execute("UPDATE mkt_linkedin_posts SET status='publishing', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "mkt_post_publish_requested", $"#{id}");
            return J(new { ok = true, status = "publishing" });
        }));

        // ───────── manual LinkedIn outreach (system NEVER auto-sends a personal DM) ─────────
        app.MapGet("/api/admin/marketing/outreach", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_linkedin_outreach ORDER BY id DESC LIMIT 300") })));
        app.MapPost("/api/admin/marketing/outreach", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_posts", adm =>
            {
                var id = I(b, "id");
                if (id > 0)
                {
                    var sentManually = I(b, "sent_manually") == 1;
                    var sentAt = sentManually ? (Nz(S(b, "sent_at")) ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")) : Nz(S(b, "sent_at"));
                    db.Execute("UPDATE mkt_linkedin_outreach SET prospect_name=?,profile_url=?,suggested_message=?,sent_manually=?,sent_at=?,response_note=?,followup_at=?,notes=?,updated_at=datetime('now') WHERE id=?",
                        Nz(S(b, "prospect_name")), Nz(S(b, "profile_url")), Nz(S(b, "suggested_message")),
                        sentManually ? 1 : 0, sentAt, Nz(S(b, "response_note")), Nz(S(b, "followup_at")), Nz(S(b, "notes")), id);
                    log(adm.Id, "mkt_outreach_update", $"#{id}");
                    return J(new { ok = true, id });
                }
                var nid = db.ExecuteReturningId("INSERT INTO mkt_linkedin_outreach(prospect_name,profile_url,suggested_message,notes,owner_admin_id,created_by) VALUES(?,?,?,?,?,?)",
                    Nz(S(b, "prospect_name")), Nz(S(b, "profile_url")), Nz(S(b, "suggested_message")), Nz(S(b, "notes")), adm.Id, adm.Id);
                log(adm.Id, "mkt_outreach_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });

        // ───────── PCI unified campaigns (internal records; launch is honestly gated) ─────────
        app.MapGet("/api/admin/marketing/campaigns", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_campaigns ORDER BY id DESC") })));
        app.MapGet("/api/admin/marketing/campaigns/{id:long}", (HttpRequest req, long id) => gate(req, "mkt_view", _ =>
        {
            var c = db.QueryOne("SELECT * FROM mkt_campaigns WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return J(new { campaign = c, variants = db.Query("SELECT * FROM mkt_platform_campaigns WHERE campaign_id=?", id) });
        }));
        app.MapPost("/api/admin/marketing/campaigns", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var id = I(b, "id");
                if (id > 0)
                {
                    db.Execute(@"UPDATE mkt_campaigns SET name=?,code=?,objective=?,route_key=?,audience_summary=?,geography=?,language=?,start_date=?,end_date=?,total_budget=?,budget_currency=?,alloc_linkedin=?,alloc_google=?,alloc_meta=?,conversion_goal=?,status=?,updated_at=datetime('now') WHERE id=?",
                        name, Nz(S(b, "code")), Nz(S(b, "objective")), Nz(S(b, "route_key")), Nz(S(b, "audience_summary")), Nz(S(b, "geography")), Nz(S(b, "language")),
                        Nz(S(b, "start_date")), Nz(S(b, "end_date")), N(b, "total_budget") ?? 0, Nz(S(b, "budget_currency")) ?? "USD",
                        N(b, "alloc_linkedin") ?? 0, N(b, "alloc_google") ?? 0, N(b, "alloc_meta") ?? 0, Nz(S(b, "conversion_goal")), Nz(S(b, "status")) ?? "draft", id);
                    log(adm.Id, "mkt_campaign_update", $"#{id}");
                    return J(new { ok = true, id });
                }
                var nid = db.ExecuteReturningId(@"INSERT INTO mkt_campaigns(name,code,owner_admin_id,objective,route_key,audience_summary,geography,language,start_date,end_date,total_budget,budget_currency,alloc_linkedin,alloc_google,alloc_meta,conversion_goal,status,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?, 'draft',?)",
                    name, Nz(S(b, "code")), adm.Id, Nz(S(b, "objective")), Nz(S(b, "route_key")), Nz(S(b, "audience_summary")), Nz(S(b, "geography")), Nz(S(b, "language")),
                    Nz(S(b, "start_date")), Nz(S(b, "end_date")), N(b, "total_budget") ?? 0, Nz(S(b, "budget_currency")) ?? "USD",
                    N(b, "alloc_linkedin") ?? 0, N(b, "alloc_google") ?? 0, N(b, "alloc_meta") ?? 0, Nz(S(b, "conversion_goal")), adm.Id);
                log(adm.Id, "mkt_campaign_create", $"#{nid} {name}");
                return J(new { ok = true, id = nid });
            });
        });
        app.MapPost("/api/admin/marketing/campaigns/{id:long}/approve", (HttpRequest req, long id) => gate(req, "mkt_approve", adm =>
        {
            db.Execute("UPDATE mkt_campaigns SET approval_status='approved', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "mkt_campaign_approved", $"#{id}");
            return J(new { ok = true });
        }));

        // ───────── audiences / creatives / landing pages / conversions (read + simple create) ─────────
        app.MapGet("/api/admin/marketing/audiences", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_audiences ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/audiences", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var nid = db.ExecuteReturningId("INSERT INTO mkt_audiences(name,platform_code,source,purpose,countries,languages,professional_criteria,exclusions,consent_basis,status,created_by) VALUES(?,?,?,?,?,?,?,?,?, 'draft',?)",
                    name, Nz(S(b, "platform_code")), Nz(S(b, "source")), Nz(S(b, "purpose")), Nz(S(b, "countries")), Nz(S(b, "languages")), Nz(S(b, "professional_criteria")), Nz(S(b, "exclusions")), Nz(S(b, "consent_basis")), adm.Id);
                log(adm.Id, "mkt_audience_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });
        app.MapGet("/api/admin/marketing/creatives", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_creatives ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/creatives", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var nid = db.ExecuteReturningId("INSERT INTO mkt_creatives(name,format,headline,primary_text,description,cta,destination_url,platform_scope,language,approval_status,ai_generated,created_by) VALUES(?,?,?,?,?,?,?,?,?, 'draft',?,?)",
                    name, Nz(S(b, "format")), Nz(S(b, "headline")), Nz(S(b, "primary_text")), Nz(S(b, "description")), Nz(S(b, "cta")), Nz(S(b, "destination_url")), Nz(S(b, "platform_scope")), Nz(S(b, "language")), I(b, "ai_generated") == 1 ? 1 : 0, adm.Id);
                log(adm.Id, "mkt_creative_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });
        app.MapGet("/api/admin/marketing/landing-pages", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_landing_pages ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/landing-pages", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var nid = db.ExecuteReturningId("INSERT INTO mkt_landing_pages(name,url,headline,description,cta,application_link,noindex,status,created_by) VALUES(?,?,?,?,?,?,?, 'draft',?)",
                    name, Nz(S(b, "url")), Nz(S(b, "headline")), Nz(S(b, "description")), Nz(S(b, "cta")), Nz(S(b, "application_link")), I(b, "noindex") == 1 ? 1 : 0, adm.Id);
                log(adm.Id, "mkt_landing_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });
        app.MapGet("/api/admin/marketing/conversions", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_conversions ORDER BY id DESC") })));
        app.MapPost("/api/admin/marketing/conversions", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                var name = S(b, "name"); if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
                var nid = db.ExecuteReturningId("INSERT INTO mkt_conversions(name,platform_code,business_event,value,currency,enabled,created_by) VALUES(?,?,?,?,?,1,?)",
                    name, Nz(S(b, "platform_code")), Nz(S(b, "business_event")), N(b, "value"), Nz(S(b, "currency")) ?? "USD", adm.Id);
                log(adm.Id, "mkt_conversion_create", $"#{nid}");
                return J(new { ok = true, id = nid });
            });
        });

        // ───────── Lead Centre ─────────
        app.MapGet("/api/admin/marketing/leads", (HttpRequest req) => gate(req, "mkt_leads", _ =>
        {
            var status = req.Query["status"].ToString();
            var rows = string.IsNullOrEmpty(status)
                ? db.Query("SELECT * FROM mkt_leads ORDER BY id DESC LIMIT 500")
                : db.Query("SELECT * FROM mkt_leads WHERE status=? ORDER BY id DESC LIMIT 500", status);
            return J(new { rows });
        }));
        app.MapPost("/api/admin/marketing/leads/{id:long}/status", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_leads", adm =>
            {
                db.Execute("UPDATE mkt_leads SET status=?, owner_admin_id=COALESCE(?,owner_admin_id), next_followup_at=?, last_contact_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                    Nz(S(b, "status")) ?? "new", b.ContainsKey("assign_self") && I(b, "assign_self") == 1 ? (long?)adm.Id : null, Nz(S(b, "next_followup_at")), id);
                log(adm.Id, "mkt_lead_status", $"#{id} -> {S(b, "status")}");
                return J(new { ok = true });
            });
        });

        // ───────── Google Search Console (read + honest sitemap/inspect gating) ─────────
        app.MapGet("/api/admin/marketing/gsc/properties", (HttpRequest req) => gate(req, "mkt_gsc", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_gsc_properties ORDER BY id DESC"), sitemaps = db.Query("SELECT * FROM mkt_gsc_sitemaps ORDER BY id DESC LIMIT 200") })));
        app.MapPost("/api/admin/marketing/gsc/sitemaps/submit", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_gsc", adm =>
            {
                var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code='google_search_console' AND status='connected'") > 0;
                if (!hasConn)
                    return J(new { ok = false, reason = "not_connected", operator_action = "Connect a verified Search Console property via Google OAuth before submitting sitemaps." });
                // Phase 2: call the Search Console API sitemaps.submit; record the provider response.
                return J(new { ok = false, reason = "pending_setup", operator_action = "Property connection pending. Note: sitemap submission is a discovery signal and does not guarantee indexing." });
            });
        });

        // ───────── alerts + audit ─────────
        app.MapGet("/api/admin/marketing/alerts", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT * FROM mkt_alerts ORDER BY id DESC LIMIT 200") })));
        app.MapPost("/api/admin/marketing/alerts/{id:long}/ack", (HttpRequest req, long id) => gate(req, "mkt_view", adm =>
        {
            db.Execute("UPDATE mkt_alerts SET status='acknowledged', acknowledged_by=?, acknowledged_at=datetime('now') WHERE id=?", adm.Id, id);
            return J(new { ok = true });
        }));
        app.MapGet("/api/admin/marketing/audit", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT id,user_id,action,details,created_at FROM audit_logs WHERE action LIKE 'mkt_%' ORDER BY id DESC LIMIT 300") })));
    }
}
