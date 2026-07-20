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
        var rows = db.Query(@"SELECT * FROM mkt_jobs
            WHERE status IN('queued','retrying')
              AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))
            ORDER BY id LIMIT ?", limit);
        var n = 0;
        foreach (var r in rows)
        {
            var id = H.L(r["id"]);
            try { RunOne(db, r); n++; }
            catch (Exception e) { db.Execute("UPDATE mkt_jobs SET status='failed', last_error=?, updated_at=datetime('now') WHERE id=?", e.Message, id); }
        }
        return n;
    }

    static void RunOne(Db db, Dictionary<string, object?> job)
    {
        var id = H.L(job["id"]);
        db.Execute("UPDATE mkt_jobs SET status='processing', updated_at=datetime('now') WHERE id=?", id);
        var type = H.Str(job["job_type"]) ?? "";
        var entityId = H.L(job["entity_id"]);
        var attempt = (int)H.L(job["attempts"]) + 1;
        var maxAttempts = (int)Math.Max(1, H.L(job["max_attempts"]));

        MarketingConnectors.Result res = type switch
        {
            "linkedin_post_publish" => PublishLinkedInPost(db, entityId),
            "gsc_sitemap_submit" => SubmitSitemap(db, job),
            "gsc_url_inspect" => InspectUrl(db, job),
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
            // A missing connection/token is a permanent (operator-action) failure — don't spin retries on it.
            var permanent = res.Response.StartsWith("no_connected") || res.Response is "no_access_token" or "no_organisation_id" || res.Response.StartsWith("unknown_job_type");
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
