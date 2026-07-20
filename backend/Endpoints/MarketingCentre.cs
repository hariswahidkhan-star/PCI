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

        // Begin OAuth — returns a real authorize URL when the provider app is configured (env), else an
        // honest operator action. The signed state binds the callback to this connection for one hour.
        app.MapPost("/api/admin/marketing/connections/{id:long}/oauth-url", (HttpRequest req, long id) => gate(req, "mkt_connect", adm =>
        {
            var c = db.QueryOne("SELECT * FROM mkt_connections WHERE id=?", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var family = H.Str(db.QueryOne("SELECT family FROM mkt_platforms WHERE code=?", H.Str(c["platform_code"]))?["family"]) ?? "";
            if (!Marketing.FamilyConfigured(family))
                return J(new { ok = false, reason = "provider_not_configured", operator_action = $"Set the {family} OAuth client id and secret in Render environment variables, then register the redirect URL {MarketingOAuth.RedirectUri(req)} with the provider." });
            var url = MarketingOAuth.AuthorizeUrl(db, id, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), req);
            if (url is null) return J(new { ok = false, reason = "provider_not_configured", operator_action = "Provider client id is not set in the environment." });
            db.Execute("UPDATE mkt_connections SET status='connecting', updated_at=datetime('now') WHERE id=?", id);
            log(adm.Id, "mkt_oauth_started", $"#{id} {family}");
            return J(new { ok = true, authorize_url = url, redirect_uri = MarketingOAuth.RedirectUri(req) });
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
                return J(new { ok = false, queued = true, reason = eff, operator_action = "LinkedIn organisation posting is not yet available. Post marked scheduled; it will publish once the LinkedIn Company Page connection and Community Management access are approved and the account is connected." });
            }
            // Enqueue an idempotent provider job; the dispatcher calls the LinkedIn Posts API. A re-click
            // reuses the same key, so a post can never be published twice.
            db.Execute("UPDATE mkt_linkedin_posts SET status='publishing', updated_at=datetime('now') WHERE id=?", id);
            var jid = MarketingJobDispatcher.Enqueue(db, "linkedin_post_publish", "linkedin_page", "linkedin_post", id, null, $"linkedin_post:{id}", adm.Id);
            try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
            log(adm.Id, "mkt_post_publish_requested", $"#{id} job {jid}");
            return J(new { ok = true, status = "publishing", job_id = jid });
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
        // Per-platform campaign variant under a PCI campaign.
        app.MapPost("/api/admin/marketing/campaigns/{id:long}/platforms", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_ads", adm =>
            {
                if (db.QueryOne("SELECT id FROM mkt_campaigns WHERE id=?", id) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
                var platform = S(b, "platform_code"); if (platform.Length == 0) return Results.Json(new { error = "platform_required" }, statusCode: 400);
                var nid = db.ExecuteReturningId(@"INSERT INTO mkt_platform_campaigns(campaign_id,platform_code,name,objective,campaign_type,daily_budget,lifetime_budget,status,approval_status)
                    VALUES(?,?,?,?,?,?,?, 'draft','draft')",
                    id, platform, Nz(S(b, "name")), Nz(S(b, "objective")), Nz(S(b, "campaign_type")), N(b, "daily_budget"), N(b, "lifetime_budget"));
                log(adm.Id, "mkt_platform_campaign_create", $"campaign #{id} {platform} #{nid}");
                return J(new { ok = true, id = nid });
            });
        });
        // Launch a platform variant: requires PCI approval + an approved budget + a connected account; then
        // enqueues an idempotent provider create job (created PAUSED at the provider so nothing spends yet).
        app.MapPost("/api/admin/marketing/platform-campaigns/{id:long}/launch", (HttpRequest req, long id) => gate(req, "mkt_approve", adm =>
        {
            var pc = db.QueryOne("SELECT * FROM mkt_platform_campaigns WHERE id=?", id);
            if (pc is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var camp = db.QueryOne("SELECT * FROM mkt_campaigns WHERE id=?", H.L(pc["campaign_id"]));
            if (H.Str(camp?["approval_status"]) != "approved") return Results.Json(new { error = "campaign_not_approved", detail = "Approve the PCI campaign before launching a platform." }, statusCode: 400);
            var platform = H.Str(pc["platform_code"]) ?? "";
            var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code=? AND status='connected'", platform) > 0;
            if (!hasConn) return J(new { ok = false, reason = "not_connected", operator_action = $"Connect an approved {platform} account before launching. The variant stays draft." });
            var jobType = platform switch { "meta_ads" or "facebook_page" or "instagram" => "meta_campaign_create", "google_ads" => "google_campaign_create", _ => null };
            if (jobType is null) return J(new { ok = false, reason = "connector_pending", operator_action = $"A live campaign-create connector for {platform} is not enabled yet; the variant is saved and can be launched once available." });
            db.Execute("UPDATE mkt_platform_campaigns SET status='launching', updated_at=datetime('now') WHERE id=?", id);
            var jid = MarketingJobDispatcher.Enqueue(db, jobType, platform, "platform_campaign", id, null, $"{jobType}:{id}", adm.Id);
            try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
            log(adm.Id, "mkt_platform_campaign_launch", $"#{id} {platform} job {jid}");
            return J(new { ok = true, status = "launching", job_id = jid, note = "The provider campaign is created PAUSED; activate it in the connected account after review." });
        }));

        // ───────── unified reporting (spend/metrics + lead funnel + PCI outcomes) ─────────
        app.MapGet("/api/admin/marketing/reporting", (HttpRequest req) => gate(req, "mkt_view", _ => J(new
        {
            spend_by_platform = db.Query("SELECT platform_code, SUM(spend) spend, SUM(impressions) impressions, SUM(clicks) clicks, SUM(leads) leads, SUM(conversions) conversions FROM mkt_campaign_metrics GROUP BY platform_code"),
            leads_by_status = db.Query("SELECT status, COUNT(*) n FROM mkt_leads GROUP BY status"),
            leads_by_source = db.Query("SELECT source_platform, COUNT(*) n FROM mkt_leads GROUP BY source_platform"),
            outcomes = new
            {
                leads = db.Scalar<long>("SELECT COUNT(*) FROM mkt_leads"),
                converted = db.Scalar<long>("SELECT COUNT(*) FROM mkt_leads WHERE status='converted'"),
                application_started = db.Scalar<long>("SELECT COUNT(*) FROM mkt_leads WHERE status='application_started'"),
                application_submitted = db.Scalar<long>("SELECT COUNT(*) FROM mkt_leads WHERE status='application_submitted'"),
            },
        })));
        // Sync provider insights (spend/impressions/clicks) into mkt_campaign_metrics for every launched
        // Meta/LinkedIn variant that has a provider campaign id. Token-gated; honest when not connected.
        app.MapPost("/api/admin/marketing/reporting/sync", (HttpRequest req) => gate(req, "mkt_view", adm =>
        {
            var variants = db.Query(@"SELECT id, platform_code FROM mkt_platform_campaigns
                WHERE provider_campaign_id IS NOT NULL AND provider_campaign_id<>'' AND platform_code IN('meta_ads','linkedin_ads')");
            int queued = 0;
            foreach (var v in variants)
            {
                var plat = H.Str(v["platform_code"]) ?? "";
                var jt = plat == "meta_ads" ? "meta_insights_sync" : "linkedin_insights_sync";
                var jid = MarketingJobDispatcher.Enqueue(db, jt, plat, "platform_campaign", H.L(v["id"]), null,
                    $"{jt}:{H.L(v["id"])}:{DateTime.UtcNow:yyyyMMddHH}", adm.Id);
                if (jid is not null) queued++;
            }
            try { MarketingJobDispatcher.DrainOnce(db, 20); } catch { }
            log(adm.Id, "mkt_reporting_sync", $"queued {queued}");
            return J(new { ok = true, queued, variants = variants.Count });
        }));
        // Fetch LinkedIn Lead Gen form responses into the unified Lead Centre.
        app.MapPost("/api/admin/marketing/linkedin/fetch-leads", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_leads", adm =>
            {
                var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code='linkedin_ads' AND status='connected'") > 0;
                if (!hasConn) return J(new { ok = false, reason = "not_connected", operator_action = "Connect an approved LinkedIn advertising account before fetching lead-gen responses." });
                var formId = S(b, "form_id");
                var jid = MarketingJobDispatcher.Enqueue(db, "linkedin_lead_fetch", "linkedin_ads", "lead_form", 0,
                    new { form_id = formId }, $"linkedin_lead_fetch:{formId}:{DateTime.UtcNow:yyyyMMddHH}", adm.Id);
                try { MarketingJobDispatcher.DrainOnce(db, 5); } catch { }
                log(adm.Id, "mkt_linkedin_fetch_leads", $"form {formId} job {jid}");
                return J(new { ok = true, job_id = jid });
            });
        });

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
                var property = S(b, "property"); var sitemap = S(b, "sitemap_url");
                if (property.Length == 0) property = H.Str(db.QueryOne("SELECT property FROM mkt_gsc_properties ORDER BY id DESC LIMIT 1")?["property"]) ?? "";
                if (property.Length == 0 || sitemap.Length == 0) return Results.Json(new { error = "property_and_sitemap_required" }, statusCode: 400);
                var jid = MarketingJobDispatcher.Enqueue(db, "gsc_sitemap_submit", "google_search_console", "sitemap", 0,
                    new { property, sitemap_url = sitemap }, $"gsc_sitemap:{property}:{sitemap}:{DateTime.UtcNow:yyyyMMddHH}", adm.Id);
                try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
                log(adm.Id, "mkt_sitemap_submit", $"{property} {sitemap} job {jid}");
                // Honest note carried through regardless of provider outcome.
                return J(new { ok = true, job_id = jid, note = "Sitemap submission is a discovery signal and does not guarantee crawling or indexing." });
            });
        });
        // URL Inspection — reports the index status the API exposes; not a guaranteed 'index now' request.
        app.MapPost("/api/admin/marketing/gsc/inspect", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_gsc", adm =>
            {
                var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code='google_search_console' AND status='connected'") > 0;
                if (!hasConn) return J(new { ok = false, reason = "not_connected", operator_action = "Connect a verified Search Console property before inspecting URLs." });
                var property = S(b, "property"); var url = S(b, "url");
                if (property.Length == 0) property = H.Str(db.QueryOne("SELECT property FROM mkt_gsc_properties ORDER BY id DESC LIMIT 1")?["property"]) ?? "";
                if (property.Length == 0 || url.Length == 0) return Results.Json(new { error = "property_and_url_required" }, statusCode: 400);
                var jid = MarketingJobDispatcher.Enqueue(db, "gsc_url_inspect", "google_search_console", "url", 0,
                    new { property, url }, $"gsc_inspect:{url}:{DateTime.UtcNow:yyyyMMddHHmm}", adm.Id);
                try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
                log(adm.Id, "mkt_url_inspect", $"{url} job {jid}");
                return J(new { ok = true, job_id = jid, note = "URL Inspection reports the index status the Search Console API exposes; it is not a guaranteed request for immediate indexing." });
            });
        });
        // Pull Search Analytics (clicks/impressions/CTR/position) for a date range + dimension.
        app.MapPost("/api/admin/marketing/gsc/search-analytics", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "mkt_gsc", adm =>
            {
                var hasConn = db.Scalar<long>("SELECT COUNT(*) FROM mkt_connections WHERE platform_code='google_search_console' AND status='connected'") > 0;
                if (!hasConn) return J(new { ok = false, reason = "not_connected", operator_action = "Connect a verified Search Console property before pulling search analytics." });
                var property = S(b, "property");
                if (property.Length == 0) property = H.Str(db.QueryOne("SELECT property FROM mkt_gsc_properties ORDER BY id DESC LIMIT 1")?["property"]) ?? "";
                var start = S(b, "start_date"); var end = S(b, "end_date"); var dim = S(b, "dimension");
                if (dim.Length == 0) dim = "query";
                if (property.Length == 0 || start.Length == 0 || end.Length == 0) return Results.Json(new { error = "property_start_end_required" }, statusCode: 400);
                var jid = MarketingJobDispatcher.Enqueue(db, "gsc_search_analytics", "google_search_console", "property", 0,
                    new { property, start_date = start, end_date = end, dimension = dim }, $"gsc_sa:{property}:{start}:{end}:{dim}", adm.Id);
                try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
                log(adm.Id, "mkt_gsc_analytics", $"{property} {start}..{end} {dim} job {jid}");
                return J(new { ok = true, job_id = jid });
            });
        });
        app.MapGet("/api/admin/marketing/gsc/query-data", (HttpRequest req) => gate(req, "mkt_gsc", _ =>
        {
            var dim = req.Query["dimension"].ToString();
            var rows = string.IsNullOrEmpty(dim)
                ? db.Query("SELECT * FROM mkt_gsc_query_data ORDER BY id DESC LIMIT 250")
                : db.Query("SELECT * FROM mkt_gsc_query_data WHERE dimension=? ORDER BY clicks DESC, id DESC LIMIT 250", dim);
            return J(new { rows });
        }));

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

        // ───────── provider job queue (visibility + manual drain/retry) ─────────
        app.MapGet("/api/admin/marketing/jobs", (HttpRequest req) => gate(req, "mkt_view", _ =>
            J(new { rows = db.Query("SELECT id,idempotency_key,job_type,platform_code,entity_type,entity_id,status,attempts,max_attempts,last_error,next_attempt_at,created_at,updated_at FROM mkt_jobs ORDER BY id DESC LIMIT 300") })));
        app.MapPost("/api/admin/marketing/jobs/{id:long}/retry", (HttpRequest req, long id) => gate(req, "mkt_ads", adm =>
        {
            db.Execute("UPDATE mkt_jobs SET status='queued', next_attempt_at=NULL, updated_at=datetime('now') WHERE id=? AND status='failed'", id);
            try { MarketingJobDispatcher.DrainOnce(db, 3); } catch { }
            log(adm.Id, "mkt_job_retry", $"#{id}");
            return J(new { ok = true });
        }));
        app.MapPost("/api/admin/marketing/jobs/drain", (HttpRequest req) => gate(req, "mkt_ads", adm =>
        {
            var n = MarketingJobDispatcher.DrainOnce(db, 25);
            return J(new { ok = true, processed = n });
        }));

        // ───────── public OAuth callback (browser redirect target — verified by signed state, no bearer) ─────────
        app.MapGet("/api/marketing/oauth/callback", async (HttpContext ctx) =>
        {
            IResult Page(string title, string msg) => Results.Content(
                $"<html><body style='font-family:sans-serif;max-width:560px;margin:60px auto;text-align:center'><h2>{title}</h2><p>{msg}</p><p><a href='/admin/marketing-ads'>Return to the Marketing centre</a></p></body></html>", "text/html");
            var err = ctx.Request.Query["error"].ToString();
            if (!string.IsNullOrEmpty(err)) return Page("Connection cancelled", "The provider reported: " + System.Net.WebUtility.HtmlEncode(err));
            var code = ctx.Request.Query["code"].ToString();
            var state = ctx.Request.Query["state"].ToString();
            var connId = MarketingOAuth.VerifyState(state, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            if (connId is null) return Page("Link invalid or expired", "Please start the connection again from the Marketing centre.");
            var c = db.QueryOne("SELECT * FROM mkt_connections WHERE id=?", connId.Value);
            if (c is null) return Page("Connection not found", "Please register the account again.");
            var platform = H.Str(c["platform_code"]) ?? "";
            var family = H.Str(db.QueryOne("SELECT family FROM mkt_platforms WHERE code=?", platform)?["family"]) ?? "";
            if (string.IsNullOrEmpty(code)) return Page("No authorisation code", "The provider did not return an authorisation code.");
            var tok = await MarketingOAuth.Exchange(family, code, MarketingOAuth.RedirectUri(ctx.Request));
            if (tok is null || string.IsNullOrEmpty(tok.AccessToken))
            {
                db.Execute("UPDATE mkt_connections SET status='error', last_failure_at=datetime('now'), last_error='token_exchange_failed', updated_at=datetime('now') WHERE id=?", connId.Value);
                return Page("Could not complete the connection", "The token exchange failed. Check the provider app configuration and try again.");
            }
            var expiresAt = tok.ExpiresInSeconds is > 0 ? DateTime.UtcNow.AddSeconds(tok.ExpiresInSeconds.Value).ToString("yyyy-MM-dd HH:mm:ss") : null;
            Marketing.StoreTokens(db, connId.Value, tok.AccessToken, tok.RefreshToken, expiresAt);
            db.Execute("UPDATE mkt_connections SET status='connected', granted_scopes=COALESCE(?,?), last_success_at=datetime('now'), last_error=NULL, updated_at=datetime('now') WHERE id=?",
                string.IsNullOrEmpty(tok.Scope) ? null : tok.Scope, MarketingOAuth.Scopes(platform), connId.Value);
            // For a Search Console connection, register the property record so the GSC tab can use it.
            if (platform == "google_search_console")
            {
                var property = H.Str(c["external_property"]);
                if (!string.IsNullOrEmpty(property) && db.QueryOne("SELECT id FROM mkt_gsc_properties WHERE property=?", property) is null)
                    db.Execute("INSERT INTO mkt_gsc_properties(connection_id,property,verified) VALUES(?,?,1)", connId.Value, property);
            }
            log(null, "mkt_oauth_connected", $"#{connId} {platform}");
            return Page("Account connected", "The account is now connected. You can close this window and return to the Marketing centre.");
        });

        // ───────── lead intake webhooks (public, secret-verified) → unified Lead Centre ─────────
        // Insert-or-ignore a lead keyed by a caller-supplied dedup key (or platform+provider id). Returns the
        // lead id. Never creates a student account — an advertising lead is a prospect, not a member (spec §7).
        long UpsertLead(string source, string dedup, string? name, string? email, string? phone, string? country,
            string? company, string? title, string? certInterest, bool consent, string? consentText, string? utmJson, string? providerLeadId, long? campaignId)
        {
            var existing = db.QueryOne("SELECT id FROM mkt_leads WHERE dedup_key=?", dedup);
            if (existing is not null) return H.L(existing["id"]);
            try
            {
                return db.ExecuteReturningId(@"INSERT INTO mkt_leads(name,email,phone,country,company,job_title,source_platform,campaign_id,certification_interest,consent,consent_text,utm_json,provider_lead_id,dedup_key,status)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?, 'new')",
                    name, email, phone, country, company, title, source, campaignId, certInterest, consent ? 1 : 0, consentText, utmJson, providerLeadId, dedup);
            }
            catch { return H.L(db.QueryOne("SELECT id FROM mkt_leads WHERE dedup_key=?", dedup)?["id"] ?? 0L); /* unique race */ }
        }

        // Generic secret-verified intake for website/partner/webinar lead forms (full fields inline).
        app.MapPost("/api/webhooks/lead-intake", async (HttpContext ctx) =>
        {
            var secret = Environment.GetEnvironmentVariable("MARKETING_LEAD_WEBHOOK_SECRET") ?? Environment.GetEnvironmentVariable("EMAIL_WEBHOOK_SECRET");
            var given = ctx.Request.Query["secret"].ToString();
            if (string.IsNullOrEmpty(given)) given = ctx.Request.Headers["X-Webhook-Secret"].ToString();
            if (string.IsNullOrEmpty(secret) || !Security.FixedTimeEquals(given, secret)) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var email = (H.GetS(b, "email") ?? "").Trim();
            var phone = (H.GetS(b, "phone") ?? "").Trim();
            if (email.Length == 0 && phone.Length == 0) return Results.Json(new { error = "email_or_phone_required" }, statusCode: 400);
            var source = H.GetS(b, "source", "source_platform") ?? "website";
            var dedup = H.GetS(b, "dedup_key") ?? $"intake:{source}:{(email.Length > 0 ? email.ToLowerInvariant() : phone)}";
            var lid = UpsertLead(source, dedup, H.GetS(b, "name"), email.Length > 0 ? email : null, phone.Length > 0 ? phone : null,
                H.GetS(b, "country"), H.GetS(b, "company"), H.GetS(b, "job_title", "title"), H.GetS(b, "certification_interest"),
                (H.GetNum(b, "consent") ?? 0) == 1, H.GetS(b, "consent_text"), H.GetS(b, "utm_json"), H.GetS(b, "provider_lead_id"), null);
            return J(new { ok = true, lead_id = lid });
        });

        // Meta Lead Ads webhook: GET verification challenge + POST leadgen notifications.
        app.MapGet("/api/webhooks/meta-leads", (HttpRequest req) =>
        {
            var verify = Environment.GetEnvironmentVariable("META_LEADS_VERIFY_TOKEN") ?? Environment.GetEnvironmentVariable("META_WEBHOOK_SECRET");
            var mode = req.Query["hub.mode"].ToString();
            var token = req.Query["hub.verify_token"].ToString();
            if (mode == "subscribe" && !string.IsNullOrEmpty(verify) && Security.FixedTimeEquals(token, verify))
                return Results.Text(req.Query["hub.challenge"].ToString());
            return Results.StatusCode(403);
        });
        app.MapPost("/api/webhooks/meta-leads", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            try
            {
                if (H.GetEl(b, "entry") is { ValueKind: JsonValueKind.Array } entries)
                    foreach (var entry in entries.EnumerateArray())
                        if (entry.TryGetProperty("changes", out var changes) && changes.ValueKind == JsonValueKind.Array)
                            foreach (var ch in changes.EnumerateArray())
                                if (ch.TryGetProperty("field", out var fld) && fld.GetString() == "leadgen"
                                    && ch.TryGetProperty("value", out var val))
                                {
                                    var leadgenId = val.TryGetProperty("leadgen_id", out var lg) ? lg.GetString() : null;
                                    if (string.IsNullOrEmpty(leadgenId)) continue;
                                    var formId = val.TryGetProperty("form_id", out var fm) ? fm.GetString() : null;
                                    // Create a stub keyed by the leadgen id (dedup), then fetch details via the API.
                                    var lid = UpsertLead("meta", $"meta:leadgen:{leadgenId}", null, null, null, null, null, null, null, false, null,
                                        formId is null ? null : $"{{\"form_id\":\"{formId}\"}}", leadgenId, null);
                                    MarketingJobDispatcher.Enqueue(db, "meta_lead_fetch", "meta_ads", "lead", lid,
                                        new { leadgen_id = leadgenId, lead_id = lid.ToString() }, $"meta_lead_fetch:{leadgenId}", null);
                                }
                try { MarketingJobDispatcher.DrainOnce(db, 5); } catch { }
            }
            catch (Exception e) { Console.Error.WriteLine($"[mkt] meta-leads webhook: {e.Message}"); }
            return Results.Ok(new { ok = true });   // always 200 so the provider doesn't retry-storm
        });
    }
}
