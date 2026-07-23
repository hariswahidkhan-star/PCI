namespace PCI.Backend.Data;

/// <summary>
/// Marketing, Ads and Search Console centre — MySQL data model + honest capability registry (Phase 1).
///
/// Design principle from the spec: nothing is shown as operational just because a screen exists. Every
/// platform feature has a capability row with a status that starts at its true, un-connected state
/// (mostly "provider_approval_required" or "not_connected"), and only a real, verified connection +
/// granted scope may move it forward. Provider SECRETS never live here — only env-var NAMES and
/// booleans. OAuth tokens are stored encrypted (Security.EncryptSecret) in mkt_connections.
///
/// All tables use the SQLite dialect that Db.cs auto-translates to MySQL in production. Idempotent:
/// CREATE TABLE IF NOT EXISTS + WHERE NOT EXISTS seeds, safe to run on every boot.
/// </summary>
public static class MarketingSchema
{
    public static void Ensure(Db db)
    {
        Tables(db);
        // Incremental, non-destructive column additions for already-deployed databases.
        AddCol(db, "mkt_connections", "oauth_verifier", "oauth_verifier VARCHAR(128)");   // PKCE code_verifier
        // P0-5: lease bookkeeping so the marketing job worker claims each job atomically (no double-run).
        AddCol(db, "mkt_jobs", "lease_owner", "lease_owner TEXT");
        AddCol(db, "mkt_jobs", "lease_expires_at", "lease_expires_at TEXT");
        SeedPlatforms(db);
        SeedCapabilities(db);
    }

    // Add a column only if the table exists and lacks it (mirrors Migrate.cs's AddCol; MySQL + SQLite safe).
    static void AddCol(Db db, string table, string col, string ddl)
    {
        try { var have = db.Columns(table); if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}"); }
        catch { /* best-effort; a fresh CREATE already includes the column */ }
    }

    static void Tables(Db db)
    {
        // ── Platform registry: the ad/search platforms PCI can connect (seeded, not hardcoded in UI) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_platforms(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            code VARCHAR(40) UNIQUE NOT NULL, name TEXT NOT NULL, family VARCHAR(20),  -- linkedin|google|meta
            official_api TEXT, docs_url TEXT, sort_order INTEGER DEFAULT 0, active INTEGER DEFAULT 1,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Capability registry (the honesty layer). One row per platform × feature. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_capabilities(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_code VARCHAR(40) NOT NULL, feature VARCHAR(80) NOT NULL,
            required_api TEXT, required_permission TEXT, required_account_type TEXT,
            status VARCHAR(40) NOT NULL DEFAULT 'provider_approval_required',
              -- available | available_with_permission | provider_approval_required | account_upgrade_required
              -- | read_only | manual_workflow_only | temporarily_unavailable | deprecated | unsupported | not_connected
            connection_id INTEGER, limitation TEXT, operator_action TEXT,
            last_tested_at TEXT, last_test_result TEXT,
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_mkt_cap ON mkt_capabilities(platform_code,feature)");

        // ── Connected accounts + OAuth connections. Tokens are ENCRYPTED; usernames/passwords never stored. ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_connections(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_code VARCHAR(40) NOT NULL, label TEXT,
            external_org_id TEXT, external_ad_account_id TEXT, external_page_id TEXT,
            external_ig_id TEXT, external_business_id TEXT, external_property TEXT,
            connected_user_ref TEXT, granted_scopes TEXT, roles TEXT,
            access_tier VARCHAR(30), api_version TEXT, account_currency VARCHAR(8), account_timezone TEXT,
            access_token_enc TEXT, refresh_token_enc TEXT, token_expires_at TEXT, oauth_verifier VARCHAR(128),
            status VARCHAR(24) DEFAULT 'disconnected',  -- disconnected|connecting|connected|expired|error|revoked
            approval_status VARCHAR(30) DEFAULT 'not_requested',
            last_success_at TEXT, last_failure_at TEXT, last_error TEXT,
            connected_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_conn_platform ON mkt_connections(platform_code,status)");

        // ── One PCI (internal) campaign that can span multiple platforms ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_campaigns(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, code VARCHAR(60) UNIQUE, owner_admin_id INTEGER,
            objective VARCHAR(40), certification_id INTEGER, route_key TEXT,
            audience_summary TEXT, geography TEXT, language VARCHAR(10),
            landing_page_id INTEGER, promotion_id INTEGER,
            start_date TEXT, end_date TEXT,
            total_budget REAL DEFAULT 0, budget_currency VARCHAR(8) DEFAULT 'USD',
            alloc_linkedin REAL DEFAULT 0, alloc_google REAL DEFAULT 0, alloc_meta REAL DEFAULT 0,
            conversion_goal TEXT, status VARCHAR(30) DEFAULT 'draft', approval_status VARCHAR(30) DEFAULT 'draft',
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_camp_status ON mkt_campaigns(status)");

        // ── Per-platform campaign variants under a PCI campaign ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_platform_campaigns(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            campaign_id INTEGER NOT NULL, platform_code VARCHAR(40) NOT NULL, connection_id INTEGER,
            provider_campaign_id TEXT, name TEXT, objective TEXT, campaign_type TEXT,
            daily_budget REAL, lifetime_budget REAL, bid_strategy TEXT,
            targeting_json TEXT, landing_page_id INTEGER, lead_form_id INTEGER, conversion_id INTEGER,
            status VARCHAR(30) DEFAULT 'draft', provider_status TEXT, approval_status VARCHAR(30) DEFAULT 'draft',
            last_synced_at TEXT, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_pcamp_campaign ON mkt_platform_campaigns(campaign_id)");

        // ── Creative library (reusable assets) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_creatives(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, format VARCHAR(30), headline TEXT, primary_text TEXT, description TEXT,
            media_url TEXT, thumbnail_url TEXT, cta VARCHAR(40), destination_url TEXT, display_url TEXT,
            utm_json TEXT, platform_scope TEXT, certification_id INTEGER, promotion_id INTEGER, language VARCHAR(10),
            approval_status VARCHAR(30) DEFAULT 'draft', provider_review_status TEXT, ai_generated INTEGER DEFAULT 0,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");

        // ── Audience definitions ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_audiences(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, platform_code VARCHAR(40), source VARCHAR(40), purpose TEXT,
            countries TEXT, languages TEXT, professional_criteria TEXT, exclusions TEXT,
            consent_basis TEXT, retention TEXT, provider_audience_id TEXT,
            status VARCHAR(24) DEFAULT 'draft', last_synced_at TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");

        // ── Promotions ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_promotions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, code VARCHAR(60), promo_type VARCHAR(40), certification_id INTEGER, route_key TEXT,
            fee_type VARCHAR(40), original_amount REAL, discount_amount REAL, discount_percent REAL, net_amount REAL,
            currency VARCHAR(8) DEFAULT 'USD', start_date TEXT, end_date TEXT,
            countries TEXT, languages TEXT, usage_limit INTEGER, per_user_limit INTEGER,
            landing_page_id INTEGER, status VARCHAR(24) DEFAULT 'draft', approval_status VARCHAR(30) DEFAULT 'draft',
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_promo_status ON mkt_promotions(status)");

        // ── Landing pages (campaign destinations) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_landing_pages(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, url TEXT, certification_id INTEGER, promotion_id INTEGER,
            headline TEXT, description TEXT, cta TEXT, application_link TEXT,
            noindex INTEGER DEFAULT 0, utm_json TEXT, conversion_id INTEGER,
            status VARCHAR(24) DEFAULT 'draft', valid_from TEXT, valid_to TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");

        // ── Organic LinkedIn Company Page posts ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_linkedin_posts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            campaign_id INTEGER, connection_id INTEGER, post_type VARCHAR(24) DEFAULT 'text',
            body TEXT, article_title TEXT, article_url TEXT, media_json TEXT, alt_text TEXT,
            hashtags TEXT, cta VARCHAR(40), certification_id INTEGER, audience_note TEXT, language VARCHAR(10),
            utm_json TEXT, scheduled_at TEXT, timezone TEXT,
            approval_status VARCHAR(30) DEFAULT 'draft', status VARCHAR(24) DEFAULT 'draft',
            linkedin_post_id TEXT, public_url TEXT, provider_response TEXT, metrics_json TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_liposts_status ON mkt_linkedin_posts(status)");

        // ── Manual LinkedIn outreach (system never auto-sends personal DMs) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_linkedin_outreach(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            lead_id INTEGER, prospect_name TEXT, profile_url TEXT, suggested_message TEXT,
            sent_manually INTEGER DEFAULT 0, sent_at TEXT, response_note TEXT, followup_at TEXT, notes TEXT,
            owner_admin_id INTEGER, created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");

        // ── Conversation Ad flows (paid advertising format, not personal DMs) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_conversation_ads(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            campaign_id INTEGER, connection_id INTEGER, name TEXT NOT NULL, objective TEXT,
            sender_identity TEXT, intro_message TEXT, steps_json TEXT, buttons_json TEXT,
            audience_id INTEGER, landing_page_id INTEGER, lead_form_id INTEGER,
            daily_budget REAL, lifetime_budget REAL, start_date TEXT, end_date TEXT, frequency_cap TEXT,
            status VARCHAR(30) DEFAULT 'draft', approval_status VARCHAR(30) DEFAULT 'draft',
            provider_campaign_id TEXT, metrics_json TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");

        // ── Lead forms (LinkedIn / Meta) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_lead_forms(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_code VARCHAR(40), connection_id INTEGER, name TEXT NOT NULL, provider_form_id TEXT,
            fields_json TEXT, consent_text TEXT, privacy_url TEXT, certification_id INTEGER,
            status VARCHAR(24) DEFAULT 'draft', created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");

        // ── Unified Lead Centre ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_leads(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT, email TEXT, phone TEXT, country VARCHAR(60), company TEXT, job_title TEXT,
            source_platform VARCHAR(40), campaign_id INTEGER, platform_campaign_id INTEGER, ad_ref TEXT, form_id INTEGER,
            certification_interest TEXT, membership_interest INTEGER DEFAULT 0, institution_interest INTEGER DEFAULT 0,
            consent INTEGER DEFAULT 0, consent_text TEXT, utm_json TEXT,
            owner_admin_id INTEGER, status VARCHAR(24) DEFAULT 'new', lead_score INTEGER DEFAULT 0,
            user_id INTEGER, application_status TEXT, payment_status TEXT,
            first_contact_at TEXT, last_contact_at TEXT, next_followup_at TEXT, conversion_status TEXT,
            dedup_key VARCHAR(160), provider_lead_id TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_mkt_lead_dedup ON mkt_leads(dedup_key)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_lead_status ON mkt_leads(status)");

        // ── Conversion actions + uploaded conversion events ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_conversions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, platform_code VARCHAR(40), provider_conversion_id TEXT,
            business_event VARCHAR(60), value REAL, currency VARCHAR(8) DEFAULT 'USD',
            enabled INTEGER DEFAULT 1, created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_conversion_events(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            conversion_id INTEGER, campaign_id INTEGER, user_ref TEXT, value REAL, currency VARCHAR(8),
            utm_json TEXT, event_at TEXT, upload_status VARCHAR(24) DEFAULT 'pending', provider_response TEXT,
            dedup_key VARCHAR(160), created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_mkt_conv_evt_dedup ON mkt_conversion_events(dedup_key)");

        // ── Google Search Console: properties, sitemaps, URL inspections, query data ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_gsc_properties(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            connection_id INTEGER, property TEXT NOT NULL, verified INTEGER DEFAULT 0,
            last_synced_at TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_gsc_sitemaps(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            property_id INTEGER, path TEXT, last_submitted_at TEXT, status TEXT, errors INTEGER DEFAULT 0, warnings INTEGER DEFAULT 0,
            provider_response TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_gsc_inspections(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            property_id INTEGER, url TEXT, index_status TEXT, coverage_state TEXT, last_crawl TEXT,
            provider_response TEXT, inspected_by INTEGER, inspected_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_gsc_query_data(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            property_id INTEGER, day TEXT, dimension VARCHAR(20), dim_value TEXT,
            clicks INTEGER DEFAULT 0, impressions INTEGER DEFAULT 0, ctr REAL DEFAULT 0, position REAL DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Google Ads keyword / negative-keyword management ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_keywords(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_campaign_id INTEGER, list_name TEXT, keyword TEXT NOT NULL, match_type VARCHAR(16),
            kind VARCHAR(12) DEFAULT 'keyword',  -- keyword | negative
            status VARCHAR(16) DEFAULT 'active', max_cpc REAL, created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");

        // ── Budget approvals ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_budget_approvals(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            campaign_id INTEGER, requested_amount REAL, currency VARCHAR(8), reason TEXT,
            tier VARCHAR(30),  -- marketing|finance|senior|two_person
            requested_by INTEGER, status VARCHAR(20) DEFAULT 'pending', decided_by INTEGER, decided_at TEXT, note TEXT,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Generic approval workflow records ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_approvals(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            entity_type VARCHAR(30), entity_id INTEGER, stage VARCHAR(40), status VARCHAR(20) DEFAULT 'pending',
            requested_by INTEGER, decided_by INTEGER, decided_at TEXT, note TEXT,
            created_at TEXT DEFAULT (datetime('now')))");

        // ── Provider job queue / outbox (idempotent provider calls) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_jobs(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            idempotency_key VARCHAR(160), job_type VARCHAR(50), platform_code VARCHAR(40),
            entity_type VARCHAR(30), entity_id INTEGER, payload_json TEXT,
            status VARCHAR(20) DEFAULT 'queued', attempts INTEGER DEFAULT 0, max_attempts INTEGER DEFAULT 5,
            last_error TEXT, provider_response TEXT, next_attempt_at TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_mkt_jobs_idem ON mkt_jobs(idempotency_key)");

        // ── Per-day platform-campaign metrics (reporting sync target) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_campaign_metrics(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_campaign_id INTEGER, platform_code VARCHAR(40), day TEXT,
            impressions INTEGER DEFAULT 0, reach INTEGER DEFAULT 0, clicks INTEGER DEFAULT 0,
            spend REAL DEFAULT 0, currency VARCHAR(8) DEFAULT 'USD',
            leads INTEGER DEFAULT 0, conversions INTEGER DEFAULT 0, conversion_value REAL DEFAULT 0,
            video_views INTEGER DEFAULT 0, dedup_key VARCHAR(160),
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_mkt_metrics_dedup ON mkt_campaign_metrics(dedup_key)");

        // ── Alerts ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS mkt_alerts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            kind VARCHAR(50), severity VARCHAR(12) DEFAULT 'info', platform_code VARCHAR(40),
            entity_type VARCHAR(30), entity_id INTEGER, message TEXT,
            status VARCHAR(16) DEFAULT 'open', acknowledged_by INTEGER, acknowledged_at TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mkt_alert_status ON mkt_alerts(status)");
    }

    // Seed the platform list (idempotent).
    static void SeedPlatforms(Db db)
    {
        void P(string code, string name, string family, string api, string docs)
            => db.Execute(@"INSERT INTO mkt_platforms(code,name,family,official_api,docs_url)
                SELECT ?,?,?,?,? WHERE NOT EXISTS(SELECT 1 FROM mkt_platforms WHERE code=?)",
                code, name, family, api, docs, code);
        P("linkedin_page", "LinkedIn Company Page", "linkedin", "LinkedIn Posts API", "https://learn.microsoft.com/linkedin/marketing/");
        P("linkedin_ads", "LinkedIn Advertising", "linkedin", "LinkedIn Advertising API", "https://learn.microsoft.com/linkedin/marketing/");
        P("linkedin_conversation", "LinkedIn Conversation Ads", "linkedin", "LinkedIn Advertising API", "https://learn.microsoft.com/linkedin/marketing/");
        P("google_search_console", "Google Search Console", "google", "Search Console API", "https://developers.google.com/webmaster-tools");
        P("google_ads", "Google Ads", "google", "Google Ads API", "https://developers.google.com/google-ads/api/docs/start");
        P("meta_ads", "Meta Ads (Facebook & Instagram)", "meta", "Meta Marketing API", "https://developers.facebook.com/docs/marketing-apis");
        P("facebook_page", "Facebook Page", "meta", "Meta Graph API", "https://developers.facebook.com/docs/graph-api");
        P("instagram", "Instagram Professional", "meta", "Meta Graph API", "https://developers.facebook.com/docs/instagram");
    }

    // Seed the capability registry with HONEST starting statuses. Everything paid/organic that requires a
    // provider-approved developer app + granted scopes starts at provider_approval_required or not_connected;
    // personal LinkedIn DMs are manual_workflow_only; the raw messaging API is explicitly unsupported until
    // LinkedIn grants written access; the deprecated Message Ads format is marked deprecated.
    static void SeedCapabilities(Db db)
    {
        void C(string platform, string feature, string api, string perm, string acct, string status, string limitation, string action)
            => db.Execute(@"INSERT INTO mkt_capabilities(platform_code,feature,required_api,required_permission,required_account_type,status,limitation,operator_action)
                SELECT ?,?,?,?,?,?,?,? WHERE NOT EXISTS(SELECT 1 FROM mkt_capabilities WHERE platform_code=? AND feature=?)",
                platform, feature, api, perm, acct, status, limitation, action, platform, feature);

        // LinkedIn — organic + advertising
        C("linkedin_page", "Organisation page posts", "Posts API", "w_organization_social + Page admin role", "Company Page + approved developer app",
            "provider_approval_required", "Community Management API access must be granted by LinkedIn before organisation posting works.",
            "Create/confirm the LinkedIn developer app, confirm Page-admin roles, and request Community Management access.");
        C("linkedin_ads", "Ad campaign management", "Advertising API", "rw_ads / r_ads_reporting", "LinkedIn ad account + approved app (Standard tier)",
            "provider_approval_required", "LinkedIn Advertising API has Development and Standard access tiers; Standard requires approval.",
            "Apply for LinkedIn Advertising API access and connect the ad account.");
        C("linkedin_conversation", "Conversation Ads", "Advertising API", "rw_ads", "LinkedIn ad account with Conversation Ads capability",
            "provider_approval_required", "Conversation Ads is a paid advertising format and requires the ad account + app to have the capability.",
            "Request Conversation Ads capability from LinkedIn after Advertising API approval.");
        C("linkedin_ads", "Lead Gen Forms", "Advertising API", "rw_ads + r_ads_reporting + lead sync", "Approved ad account",
            "provider_approval_required", "Lead sync requires the corresponding LinkedIn permission.", "Request Lead Gen Forms / lead-sync permission.");
        C("linkedin_page", "Personal direct messages", "n/a (no supported API)", "explicit LinkedIn approval", "n/a",
            "manual_workflow_only", "Ordinary personal LinkedIn DMs have no supported bulk API; they stay a manual, human action.",
            "Use the Manual LinkedIn Outreach workflow. Do not automate personal DMs.");
        C("linkedin_page", "Direct Messaging API", "LinkedIn Messaging API", "written LinkedIn approval", "explicitly approved app",
            "unsupported", "Disabled placeholder — may only be enabled after PCI receives written LinkedIn approval for the exact capability.",
            "Obtain written LinkedIn approval before this is enabled.");
        C("linkedin_ads", "Message Ads (legacy)", "Advertising API", "n/a", "n/a",
            "deprecated", "The older Message Ads format is deprecated; use Conversation Ads instead.", "Do not build on Message Ads.");

        // Google Search Console (search performance — NOT paid advertising)
        C("google_search_console", "Search analytics", "Search Console API", "webmasters.readonly", "Verified property",
            "not_connected", "Read-only search performance for a verified property.", "Verify the property and connect via OAuth.");
        C("google_search_console", "Sitemap list & submit", "Search Console API", "webmasters", "Verified property",
            "not_connected", "Sitemap submission is a discovery signal — it does NOT guarantee crawling or indexing.", "Connect the property, then submit sitemaps.");
        C("google_search_console", "URL Inspection", "Search Console API (URL Inspection)", "webmasters.readonly", "Verified property",
            "not_connected", "Reports the index/indexable status the API exposes; it is not a guaranteed 'index now' request.", "Connect the property to inspect public URLs.");

        // Google Ads
        C("google_ads", "Campaign management", "Google Ads API", "adwords", "Google Ads account + developer token",
            "account_upgrade_required", "Requires an approved Google Ads developer token (Basic/Standard access).", "Obtain a developer token and connect the account via OAuth.");
        C("google_ads", "Conversion tracking", "Google Ads API", "adwords", "Google Ads account",
            "account_upgrade_required", "Conversion actions must be configured in the connected account.", "Configure conversion actions after connecting.");

        // Meta
        C("meta_ads", "Campaign management", "Meta Marketing API", "ads_management", "Meta Business + ad account + app review",
            "provider_approval_required", "ads_management permission requires Meta App Review.", "Complete Meta App Review and connect the ad account.");
        C("meta_ads", "Lead forms", "Meta Marketing API", "leads_retrieval + pages_manage_ads", "Facebook Page + ad account",
            "provider_approval_required", "Lead retrieval requires approved permissions + Page webhook.", "Request lead permissions and register the lead webhook.");
        C("facebook_page", "Page posts", "Meta Graph API", "pages_manage_posts", "Facebook Page",
            "provider_approval_required", "Page management permissions require App Review.", "Complete App Review for Page permissions.");
        C("instagram", "Instagram ads placement", "Meta Marketing API", "instagram_basic + ads_management", "IG professional linked to Page",
            "provider_approval_required", "Requires a linked Instagram professional account and approved permissions.", "Link the IG professional account and complete App Review.");
    }
}
