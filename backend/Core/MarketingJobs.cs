using Microsoft.Extensions.Hosting;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Reliable provider-call queue for the Marketing centre (Phase 2). Every provider action (publish a
/// LinkedIn post, submit a sitemap, inspect a URL) becomes a mkt_jobs row with a unique idempotency key,
/// so a retry or a double-click never produces a duplicate. A background worker drains due jobs, calls the
/// official connector, records the provider response, and retries transient failures with exponential
/// backoff up to max_attempts. Nothing throws out of the loop; a provider outage leaves jobs queued.
/// </summary>
public sealed class MarketingJobDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);
    public MarketingJobDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { DrainOnce(_db, 20); }
            catch (Exception e) { Console.Error.WriteLine($"[mkt-jobs] drain failed: {e.Message}"); }
        }
        while (await SafeWait(timer, stoppingToken));
    }
    static async Task<bool> SafeWait(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    /// <summary>Enqueue a provider job. Duplicate idempotency keys are silently ignored (returns null).</summary>
    public static long? Enqueue(Db db, string jobType, string platformCode, string entityType, long entityId,
        object? payload, string idempotencyKey, long? createdBy)
    {
        if (db.QueryOne("SELECT id FROM mkt_jobs WHERE idempotency_key=?", idempotencyKey) is not null) return null;
        try
        {
            return db.ExecuteReturningId(@"INSERT INTO mkt_jobs(idempotency_key,job_type,platform_code,entity_type,entity_id,payload_json,status,created_by)
                VALUES(?,?,?,?,?,?, 'queued',?)",
                idempotencyKey, jobType, platformCode, entityType, entityId,
                payload is null ? null : JsonSerializer.Serialize(payload), createdBy);
        }
        catch { return null; /* unique race → treat as duplicate */ }
    }

    /// <summary>Process up to <paramref name="limit"/> due jobs. Also callable directly for an admin retry.</summary>
    public static int DrainOnce(Db db, int limit)
    {
        // Atomically claim each due (or crashed-lease) job before running it, so concurrent dispatchers on
        // several instances never process the same job twice.
        var owner = Lease.NewOwner();
        var rows = db.Query(@"SELECT id FROM mkt_jobs
            WHERE (status IN('queued','retrying') AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now')))
               OR (status='processing' AND lease_expires_at IS NOT NULL AND lease_expires_at<datetime('now'))
            ORDER BY id LIMIT ?", limit);
        var n = 0;
        foreach (var idRow in rows)
        {
            var id = H.L(idRow["id"]);
            if (!Lease.Claim(db, "mkt_jobs", id, owner, new[] { "queued", "retrying" })) continue;
            var r = db.QueryOne("SELECT * FROM mkt_jobs WHERE id=?", id);
            if (r is null) continue;
            try { RunOne(db, r); n++; }
            catch (Exception e) { db.Execute("UPDATE mkt_jobs SET status='failed', last_error=?, updated_at=datetime('now') WHERE id=?", e.Message, id); }
        }
        return n;
    }

    static void RunOne(Db db, Dictionary<string, object?> job)
    {
        var id = H.L(job["id"]);
        // (status is already 'processing' — claimed atomically by the caller's lease.)
        var type = H.Str(job["job_type"]) ?? "";
        var entityId = H.L(job["entity_id"]);
        var attempt = (int)H.L(job["attempts"]) + 1;
        var maxAttempts = (int)Math.Max(1, H.L(job["max_attempts"]));

        MarketingConnectors.Result res = type switch
        {
            "linkedin_post_publish" => PublishLinkedInPost(db, entityId),
            "gsc_sitemap_submit" => SubmitSitemap(db, job),
            "gsc_url_inspect" => InspectUrl(db, job),
            "gsc_search_analytics" => SearchAnalytics(db, job),
            "meta_campaign_create" => MetaCampaignCreate(db, job, entityId),
            "meta_lead_fetch" => MetaLeadFetch(db, job),
            "google_campaign_create" => GoogleCampaignCreate(db, entityId),
            "meta_insights_sync" => InsightsSync(db, job, "meta_ads"),
            "linkedin_insights_sync" => InsightsSync(db, job, "linkedin_ads"),
            "linkedin_lead_fetch" => LinkedInLeadFetch(db, job),
            _ => new MarketingConnectors.Result(false, 0, null, "unknown_job_type:" + type),
        };

        if (res.Ok)
        {
            db.Execute("UPDATE mkt_jobs SET status='sent', attempts=?, provider_response=?, last_error=NULL, updated_at=datetime('now') WHERE id=?",
                attempt, Trunc(res.Response), id);
            OnSuccess(db, type, entityId, res);
        }
        else
        {
            // A missing connection/token/config or malformed payload is a permanent (operator-action)
            // failure — don't spin retries on it. Genuine provider/network errors still retry with backoff.
            var permanent = res.Response.StartsWith("no_connected") || res.Response.StartsWith("missing_")
                || res.Response is "no_access_token" or "no_organisation_id" or "no_ad_account_id"
                    or "no_developer_token" or "no_customer_id" or "no_provider_campaign_id"
                || res.Response.StartsWith("unknown_job_type") || res.Response.EndsWith("_not_found");
            if (permanent || attempt >= maxAttempts)
                db.Execute("UPDATE mkt_jobs SET status='failed', attempts=?, last_error=?, provider_response=?, updated_at=datetime('now') WHERE id=?",
                    attempt, res.Response, Trunc(res.Response), id);
            else
            {
                var backoff = Math.Min(3600, (int)Math.Pow(2, attempt) * 30);   // 60s,120s,240s… capped 1h
                db.Execute("UPDATE mkt_jobs SET status='retrying', attempts=?, last_error=?, next_attempt_at=datetime('now', ?), updated_at=datetime('now') WHERE id=?",
                    attempt, res.Response, $"+{backoff} seconds", id);
            }
        }
    }

    static void OnSuccess(Db db, string type, long entityId, MarketingConnectors.Result res)
    {
        if (type == "linkedin_post_publish")
            db.Execute("UPDATE mkt_linkedin_posts SET status='published', linkedin_post_id=?, provider_response=?, updated_at=datetime('now') WHERE id=?",
                res.ProviderId, Trunc(res.Response), entityId);
        else if (type == "google_campaign_create")
            db.Execute("UPDATE mkt_platform_campaigns SET provider_campaign_id=?, provider_status='PAUSED', status='created', last_synced_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                res.ProviderId, entityId);
    }

    static MarketingConnectors.Result GoogleCampaignCreate(Db db, long platformCampaignId)
    {
        var pc = db.QueryOne("SELECT * FROM mkt_platform_campaigns WHERE id=?", platformCampaignId);
        if (pc is null) return new(false, 0, null, "platform_campaign_not_found");
        return MarketingConnectors.GoogleAdsCreateCampaign(db, H.Str(pc["name"]) ?? "PCI campaign", H.D(pc["daily_budget"]));
    }

    // Pull insights for one platform campaign and upsert a daily metrics row (deduped per platform+campaign+day).
    static MarketingConnectors.Result InsightsSync(Db db, Dictionary<string, object?> job, string platform)
    {
        var pcId = H.L(job["entity_id"]);
        var pc = db.QueryOne("SELECT * FROM mkt_platform_campaigns WHERE id=?", pcId);
        if (pc is null) return new(false, 0, null, "platform_campaign_not_found");
        var provId = H.Str(pc["provider_campaign_id"]);
        if (string.IsNullOrWhiteSpace(provId)) return new(false, 0, null, "no_provider_campaign_id");
        var res = platform == "meta_ads" ? MarketingConnectors.MetaInsights(db, provId!) : MarketingConnectors.LinkedInAdAnalytics(db, provId!);
        if (res.Ok)
        {
            var (impr, clicks, spend) = ParseInsights(res.Response, platform);
            var day = DateTime.UtcNow.ToString("yyyy-MM-dd");
            try
            {
                db.Execute(@"INSERT INTO mkt_campaign_metrics(platform_campaign_id,platform_code,day,impressions,clicks,spend,dedup_key)
                    VALUES(?,?,?,?,?,?,?)", pcId, platform, day, impr, clicks, spend, $"{platform}:{pcId}:{day}");
            }
            catch { /* unique dedup: already synced today — treat as success */ }
        }
        return res;
    }

    static (long, long, double) ParseInsights(string json, string platform)
    {
        try
        {
            using var d = JsonDocument.Parse(json);
            var root = d.RootElement;
            // Meta: {data:[{impressions,clicks,spend}]}. LinkedIn: {elements:[{impressions,clicks,costInUsd}]}.
            var arr = platform == "meta_ads"
                ? (root.TryGetProperty("data", out var dt) ? dt : default)
                : (root.TryGetProperty("elements", out var el) ? el : default);
            if (arr.ValueKind == JsonValueKind.Array && arr.GetArrayLength() > 0)
            {
                var r = arr[0];
                long Num(string k) => r.TryGetProperty(k, out var v) ? (v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), out var n) ? n : (v.ValueKind == JsonValueKind.Number ? (long)v.GetDouble() : 0)) : 0;
                double Dbl(string k) => r.TryGetProperty(k, out var v) ? (v.ValueKind == JsonValueKind.String && double.TryParse(v.GetString(), out var n) ? n : (v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0)) : 0;
                return platform == "meta_ads"
                    ? (Num("impressions"), Num("clicks"), Dbl("spend"))
                    : (Num("impressions"), Num("clicks"), Dbl("costInUsd"));
            }
        }
        catch { }
        return (0, 0, 0);
    }

    static MarketingConnectors.Result LinkedInLeadFetch(Db db, Dictionary<string, object?> job)
    {
        var formId = PayloadStr(job, "form_id") ?? "";
        return MarketingConnectors.LinkedInFetchLeads(db, formId);
    }

    static MarketingConnectors.Result PublishLinkedInPost(Db db, long postId)
    {
        var post = db.QueryOne("SELECT * FROM mkt_linkedin_posts WHERE id=?", postId);
        if (post is null) return new(false, 0, null, "post_not_found");
        return MarketingConnectors.LinkedInPublishPost(db, post);
    }

    static MarketingConnectors.Result SubmitSitemap(Db db, Dictionary<string, object?> job)
    {
        var (property, path) = PayloadTwo(job, "property", "sitemap_url");
        if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(path)) return new(false, 0, null, "missing_property_or_sitemap");
        var res = MarketingConnectors.GscSubmitSitemap(db, property!, path!);
        if (res.Ok)
        {
            var propId = db.QueryOne("SELECT id FROM mkt_gsc_properties WHERE property=? ORDER BY id DESC LIMIT 1", property);
            db.Execute("INSERT INTO mkt_gsc_sitemaps(property_id,path,last_submitted_at,status,provider_response) VALUES(?,?,datetime('now'),'submitted',?)",
                propId is null ? null : H.L(propId["id"]), path, Trunc(res.Response));
        }
        return res;
    }

    static MarketingConnectors.Result InspectUrl(Db db, Dictionary<string, object?> job)
    {
        var (property, url) = PayloadTwo(job, "property", "url");
        if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(url)) return new(false, 0, null, "missing_property_or_url");
        var res = MarketingConnectors.GscInspectUrl(db, property!, url!);
        if (res.Ok)
        {
            var propId = db.QueryOne("SELECT id FROM mkt_gsc_properties WHERE property=? ORDER BY id DESC LIMIT 1", property);
            db.Execute("INSERT INTO mkt_gsc_inspections(property_id,url,provider_response) VALUES(?,?,?)",
                propId is null ? null : H.L(propId["id"]), url, Trunc(res.Response));
        }
        return res;
    }

    static MarketingConnectors.Result SearchAnalytics(Db db, Dictionary<string, object?> job)
    {
        var (property, range) = PayloadTwo(job, "property", "range");
        var dimension = PayloadStr(job, "dimension") ?? "query";
        var (start, end) = PayloadTwo(job, "start_date", "end_date");
        if (string.IsNullOrWhiteSpace(property) || string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
            return new(false, 0, null, "missing_property_or_dates");
        var res = MarketingConnectors.GscSearchAnalytics(db, property!, start!, end!, dimension);
        if (res.Ok)
        {
            var propId = db.QueryOne("SELECT id FROM mkt_gsc_properties WHERE property=? ORDER BY id DESC LIMIT 1", property);
            var pid = propId is null ? (long?)null : H.L(propId["id"]);
            try
            {
                using var doc = JsonDocument.Parse(res.Response);
                if (doc.RootElement.TryGetProperty("rows", out var rows) && rows.ValueKind == JsonValueKind.Array)
                    foreach (var row in rows.EnumerateArray())
                    {
                        var key = row.TryGetProperty("keys", out var ks) && ks.ValueKind == JsonValueKind.Array && ks.GetArrayLength() > 0 ? ks[0].GetString() : null;
                        double D(string k) => row.TryGetProperty(k, out var v) && v.TryGetDouble(out var d) ? d : 0;
                        db.Execute(@"INSERT INTO mkt_gsc_query_data(property_id,day,dimension,dim_value,clicks,impressions,ctr,position)
                            VALUES(?,?,?,?,?,?,?,?)", pid, end, dimension, key, (long)D("clicks"), (long)D("impressions"), D("ctr"), D("position"));
                    }
            }
            catch (Exception e) { Console.Error.WriteLine($"[mkt-jobs] gsc analytics parse: {e.Message}"); }
        }
        return res;
    }

    static MarketingConnectors.Result MetaCampaignCreate(Db db, Dictionary<string, object?> job, long platformCampaignId)
    {
        var pc = db.QueryOne("SELECT * FROM mkt_platform_campaigns WHERE id=?", platformCampaignId);
        if (pc is null) return new(false, 0, null, "platform_campaign_not_found");
        var res = MarketingConnectors.MetaCreateCampaign(db, H.Str(pc["name"]) ?? "PCI campaign", (H.Str(pc["objective"]) ?? "OUTCOME_LEADS").ToUpperInvariant());
        if (res.Ok)
            db.Execute("UPDATE mkt_platform_campaigns SET provider_campaign_id=?, provider_status='PAUSED', status='created', last_synced_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                res.ProviderId, platformCampaignId);
        return res;
    }

    static MarketingConnectors.Result MetaLeadFetch(Db db, Dictionary<string, object?> job)
    {
        var leadgenId = PayloadStr(job, "leadgen_id");
        var leadId = PayloadStr(job, "lead_id");
        if (string.IsNullOrWhiteSpace(leadgenId)) return new(false, 0, null, "missing_leadgen_id");
        var res = MarketingConnectors.MetaFetchLead(db, leadgenId!);
        if (res.Ok && long.TryParse(leadId, out var lid))
        {
            try
            {
                using var doc = JsonDocument.Parse(res.Response);
                string? name = null, email = null, phone = null, company = null, title = null;
                if (doc.RootElement.TryGetProperty("field_data", out var fd) && fd.ValueKind == JsonValueKind.Array)
                    foreach (var f in fd.EnumerateArray())
                    {
                        var n = f.TryGetProperty("name", out var nv) ? nv.GetString() : null;
                        var val = f.TryGetProperty("values", out var vv) && vv.ValueKind == JsonValueKind.Array && vv.GetArrayLength() > 0 ? vv[0].GetString() : null;
                        switch (n)
                        {
                            case "full_name" or "name": name = val; break;
                            case "email": email = val; break;
                            case "phone_number" or "phone": phone = val; break;
                            case "company_name" or "company": company = val; break;
                            case "job_title": title = val; break;
                        }
                    }
                db.Execute("UPDATE mkt_leads SET name=COALESCE(?,name), email=COALESCE(?,email), phone=COALESCE(?,phone), company=COALESCE(?,company), job_title=COALESCE(?,job_title), updated_at=datetime('now') WHERE id=?",
                    name, email, phone, company, title, lid);
            }
            catch (Exception e) { Console.Error.WriteLine($"[mkt-jobs] meta lead parse: {e.Message}"); }
        }
        return res;
    }

    static string? PayloadStr(Dictionary<string, object?> job, string key)
    {
        try
        {
            var raw = H.Str(job["payload_json"]); if (string.IsNullOrEmpty(raw)) return null;
            using var doc = JsonDocument.Parse(raw);
            return doc.RootElement.TryGetProperty(key, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
        }
        catch { return null; }
    }

    static (string?, string?) PayloadTwo(Dictionary<string, object?> job, string a, string b)
    {
        try
        {
            var raw = H.Str(job["payload_json"]); if (string.IsNullOrEmpty(raw)) return (null, null);
            using var doc = JsonDocument.Parse(raw); var root = doc.RootElement;
            string? G(string k) => root.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
            return (G(a), G(b));
        }
        catch { return (null, null); }
    }

    static string Trunc(string? s) => s is null ? "" : s.Length > 4000 ? s[..4000] : s;
}
