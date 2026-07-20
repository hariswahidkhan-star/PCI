using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The Integration Capability Registry (spec §1.2). It classifies every publishing / distribution /
/// discovery destination HONESTLY so the admin console never implies "everything is connected". There is
/// no single API that publishes to every website, and many platforms require an account type, app review,
/// audit, OAuth or a written agreement before automated publishing is allowed — the registry records that
/// reality per destination.
///
/// Capability vocabulary (exactly the spec's list):
///   Direct Publishing Available | Direct Publishing Requires Approval | Draft Export Available |
///   RSS Distribution Available | Share Link Available | Manual Submission Required | Import Only |
///   Read Only | Temporarily Unavailable | Unsupported | Integration Under Review
///
/// Seeding is idempotent (INSERT OR IGNORE) so operator edits to a row are never overwritten on redeploy.
/// The `connected` flag is computed live (env keys present) by the admin endpoint, not stored here.
/// </summary>
public static class CapabilityRegistry
{
    public record Cap(string Key, string Platform, string Kind, string Capability, string PublishMode,
        bool RequiresApproval, bool OfficialApi, string DocUrl, string Notes);

    // kinds: search | ai_discovery | ai_provider | social | syndication | import | analytics | comms
    public static readonly Cap[] Catalogue =
    {
        // ── Search engines & indexing ───────────────────────────────────────────────────────────────
        new("indexnow", "IndexNow (Bing/Yandex/Seznam)", "search", "Direct Publishing Available", "url_ping",
            false, true, "https://www.indexnow.org/documentation",
            "Open protocol, no approval. Notifies participating engines when a canonical URL is created/updated/deleted. Live in this platform."),
        new("google_search_console", "Google Search Console", "search", "Read Only", "oauth_pull",
            true, true, "https://developers.google.com/webmaster-tools",
            "Search analytics, sitemap status & URL inspection. Requires operator to verify the property and connect OAuth. Not for pushing content."),
        new("google_indexing_api", "Google Indexing API", "search", "Unsupported", "none",
            false, true, "https://developers.google.com/search/apis/indexing-api/v3/quickstart",
            "Restricted to pages with JobPosting or livestream BroadcastEvent data. MUST NOT be used for ordinary blog posts — those use sitemaps + IndexNow + normal crawling."),
        new("bing_webmaster", "Bing Webmaster Tools", "search", "Read Only", "api_key_pull",
            true, true, "https://www.bing.com/webmasters/url-submission-api",
            "Sitemap + URL submission & reporting. Requires operator site verification + API key."),
        new("google_news", "Google News", "search", "Integration Under Review", "auto_eligible",
            false, true, "https://support.google.com/news/publisher-center",
            "News sitemap is emitted; eligibility is automatic when policies are met. Publisher Center no longer accepts manual RSS/location submission — there is nothing to 'connect'."),
        new("google_discover", "Google Discover", "search", "Integration Under Review", "auto_eligible",
            false, true, "https://developers.google.com/search/docs/appearance/google-discover",
            "Automatic for indexed, policy-compliant content with strong images and non-clickbait titles. Appearance is never guaranteed."),

        // ── AI answer engines (crawler policy is managed in AI Visibility → robots) ───────────────────
        new("openai_search", "OpenAI (ChatGPT search)", "ai_discovery", "Read Only", "crawler_policy",
            false, true, "https://platform.openai.com/docs/bots",
            "Discovery controlled via robots.txt (OAI-SearchBot / ChatGPT-User / GPTBot) in AI Visibility. No publishing API; citation is never guaranteed."),
        new("anthropic_search", "Anthropic (Claude search)", "ai_discovery", "Read Only", "crawler_policy",
            false, true, "https://support.anthropic.com/en/articles/8896518",
            "Discovery controlled via robots.txt (Claude-SearchBot / Claude-User / ClaudeBot) in AI Visibility. No publishing API."),
        new("perplexity_search", "Perplexity", "ai_discovery", "Read Only", "crawler_policy",
            false, true, "https://docs.perplexity.ai/guides/bots",
            "Discovery controlled via robots.txt (PerplexityBot / Perplexity-User) in AI Visibility. No publishing API."),

        // ── AI content-generation providers (assist only; never autonomous publishing) ───────────────
        new("openai_api", "OpenAI API", "ai_provider", "Integration Under Review", "assist",
            false, true, "https://platform.openai.com/docs/api-reference",
            "Assist-only drafting/editing/translation. Set OPENAI_API_KEY as a secure environment variable to activate; AI never publishes on its own."),
        new("anthropic_api", "Anthropic Claude API", "ai_provider", "Integration Under Review", "assist",
            false, true, "https://docs.anthropic.com/en/api",
            "Assist-only drafting/editing/citation-checking. Set ANTHROPIC_API_KEY as a secure environment variable to activate; AI never publishes on its own."),

        // ── Social publishing ────────────────────────────────────────────────────────────────────────
        new("discord", "Discord channel", "social", "Direct Publishing Available", "webhook",
            false, true, "https://discord.com/developers/docs/resources/webhook",
            "Official incoming webhook — no platform review. Set a channel webhook URL to publish announcements."),
        new("telegram", "Telegram channel", "social", "Direct Publishing Available", "bot_api",
            false, true, "https://core.telegram.org/bots/api",
            "Official Bot API — no platform review. Bot must be a channel admin. Set TELEGRAM_BOT_TOKEN + channel id."),
        new("mastodon", "Mastodon / Fediverse", "social", "Direct Publishing Available", "rest_token",
            false, true, "https://docs.joinmastodon.org/methods/statuses/",
            "Official API with a per-instance access token — no central review. Configurable instance base URL."),
        new("bluesky", "Bluesky", "social", "Direct Publishing Available", "atproto",
            false, true, "https://docs.bsky.app/docs/api/com-atproto-repo-create-record",
            "Official AT Protocol with an app password — no vetting. Posts, links, images and threads supported."),
        new("linkedin_org", "LinkedIn organisation page", "social", "Direct Publishing Requires Approval", "oauth_posts_api",
            true, true, "https://learn.microsoft.com/en-us/linkedin/marketing/community-management/shares/posts-api",
            "Official Posts API. Requires an approved app, a Page admin role and (for broader access) LinkedIn Community Management review before organisation publishing."),
        new("facebook_page", "Facebook Page", "social", "Direct Publishing Requires Approval", "graph_pages_api",
            true, true, "https://developers.facebook.com/docs/pages-api",
            "Official Pages API. Requires Meta business verification, app review and Page permissions before publishing."),
        new("instagram", "Instagram Professional", "social", "Direct Publishing Requires Approval", "graph_content_publishing",
            true, true, "https://developers.facebook.com/docs/instagram-platform/content-publishing",
            "Official Content Publishing API for Professional accounts. Requires a connected account, permissions and app review; formats limited to what the account/app is approved for."),
        new("threads", "Threads", "social", "Direct Publishing Requires Approval", "threads_api",
            true, true, "https://developers.facebook.com/docs/threads",
            "Official Threads API. Requires a Meta app with Threads permissions and review before publishing."),
        new("x_twitter", "X (Twitter)", "social", "Direct Publishing Requires Approval", "x_api_v2",
            true, true, "https://docs.x.com/x-api",
            "Official X API. Requires an approved app on a current paid/access tier; rate limits apply."),
        new("pinterest", "Pinterest", "social", "Direct Publishing Requires Approval", "pinterest_v5",
            true, true, "https://developers.pinterest.com/docs/api/v5/",
            "Official API v5 create-Pin endpoint. Requires a developer app (sandbox → production approval) linking Pins back to the canonical article."),
        new("tiktok", "TikTok", "social", "Direct Publishing Requires Approval", "content_posting_api",
            true, true, "https://developers.tiktok.com/doc/content-posting-api-get-started",
            "Video/image platform, not a text-blog platform. Official Content Posting API Direct-Post requires an authorised account and an app audit before normal public visibility."),
        new("reddit", "Reddit", "social", "Direct Publishing Requires Approval", "reddit_api",
            true, true, "https://www.reddit.com/dev/api",
            "Only after approved API access, community selection, moderator-rule review and human approval. Never mass-post identical promotional links."),

        // ── Content syndication (outbound) ────────────────────────────────────────────────────────────
        new("wordpress_selfhosted", "WordPress (self-hosted)", "syndication", "Direct Publishing Available", "rest_app_password",
            false, true, "https://developer.wordpress.org/rest-api/reference/posts/",
            "Official REST API. Needs the destination site's application-password credentials (no platform review). Set a cross-domain canonical back to the original."),
        new("wordpress_com", "WordPress.com", "syndication", "Direct Publishing Requires Approval", "oauth",
            true, true, "https://developer.wordpress.com/docs/api/",
            "Official API requires an OAuth application authorised on the destination account."),
        new("ghost", "Ghost", "syndication", "Direct Publishing Available", "admin_api_key",
            false, true, "https://ghost.org/docs/admin-api/",
            "Official Admin API with a destination Admin API key (no platform review)."),
        new("blogger", "Blogger", "syndication", "Direct Publishing Requires Approval", "oauth",
            true, true, "https://developers.google.com/blogger",
            "Official Blogger API requires Google OAuth authorised on the destination blog."),
        new("forem_dev", "Forem / DEV", "syndication", "Direct Publishing Available", "api_key",
            false, true, "https://developers.forem.com/api",
            "Official Forem API with a destination API key. Supports canonical_url for cross-posting."),
        new("hashnode", "Hashnode", "syndication", "Integration Under Review", "graphql",
            true, true, "https://apidocs.hashnode.com/",
            "GraphQL publication API exists but entitlement is plan-dependent; verify live availability on the target account before activating."),
        new("medium", "Medium", "syndication", "Temporarily Unavailable", "none",
            true, true, "https://github.com/Medium/medium-api-docs",
            "Medium no longer issues integration tokens to new applications. Enable only if PCI retains valid official API capability."),
        new("substack", "Substack", "syndication", "Manual Submission Required", "rss_manual",
            false, false, "https://support.substack.com/",
            "No official publishing API. Use RSS import/export, manual publication, or an approved cross-post feature; do not use browser automation."),
        new("custom_webhook", "Custom webhook / partner API", "syndication", "Direct Publishing Available", "webhook",
            false, true, "",
            "Signed webhook to an approved partner endpoint (SSRF-guarded egress). For partner sites with a custom or headless CMS."),

        // ── Inbound external content sources (import) ─────────────────────────────────────────────────
        new("rss_import", "RSS / Atom / JSON feed import", "import", "Import Only", "feed_pull",
            false, true, "",
            "Pull an authorised source feed into the review queue. Full third-party articles are never republished without recorded permission or a valid licence."),

        // ── Backlink / SEO / analytics providers ─────────────────────────────────────────────────────
        new("semrush", "Semrush", "analytics", "Read Only", "api_key_pull",
            true, true, "https://developer.semrush.com/api/",
            "Domain / keyword / backlink / position data. Requires a paid Semrush plan with API units."),
        new("ahrefs", "Ahrefs", "analytics", "Read Only", "api_key_pull",
            true, true, "https://docs.ahrefs.com/docs/api",
            "Backlink / keyword / rank-tracking data (API v3). Requires a paid Ahrefs plan with API access."),
        new("google_analytics", "Google Analytics 4", "analytics", "Read Only", "oauth_pull",
            true, true, "https://developers.google.com/analytics/devguides/reporting/data/v1",
            "Page-view / user / engagement / conversion reporting. Requires operator OAuth or a service account."),
        new("pagespeed", "PageSpeed Insights", "analytics", "Read Only", "api_key_pull",
            false, true, "https://developers.google.com/speed/docs/insights/v5/get-started",
            "Lighthouse performance / accessibility / SEO scores. Live in this platform (optional PSI API key raises quota)."),

        // ── Newsletter / messaging (reuse the Communications Centre) ──────────────────────────────────
        new("email_newsletter", "Email newsletter", "comms", "Direct Publishing Available", "comms_centre",
            false, true, "",
            "Delivered through the Communications Centre (Resend/SMTP) with consent, dedup and unsubscribe handling."),
        new("whatsapp_broadcast", "WhatsApp broadcast", "comms", "Direct Publishing Requires Approval", "comms_centre",
            true, true, "https://developers.facebook.com/docs/whatsapp/cloud-api",
            "Delivered through the Communications Centre (Meta WhatsApp Cloud API). Requires an approved WhatsApp Business account, verified templates and opt-in."),
    };

    /// <summary>Idempotently seed the registry. Only fills absent rows — operator edits persist across redeploys.</summary>
    public static void Seed(Db db)
    {
        int i = 0;
        foreach (var c in Catalogue)
        {
            db.Execute(@"INSERT OR IGNORE INTO content_capabilities
                (platform_key, platform, kind, capability, publish_mode, requires_approval, official_api, doc_url, notes, sort_order, active)
                VALUES(?,?,?,?,?,?,?,?,?,?,1)",
                c.Key, c.Platform, c.Kind, c.Capability, c.PublishMode, c.RequiresApproval ? 1 : 0,
                c.OfficialApi ? 1 : 0, c.DocUrl, c.Notes, i);
            i++;
        }
    }
}
