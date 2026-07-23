using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Background worker that delivers approved content-syndication jobs. Every ~20s it claims due content_jobs
/// of type 'syndicate' (pending/retrying, past any backoff), publishes each cc_syndicated_posts row to its
/// destination via SyndicationConnectors (create, or update when an external id already exists), and records
/// the outcome. Failures retry with exponential backoff up to a cap, then mark failed — a failed syndication
/// NEVER touches the original PCI article. The job's unique idempotency key (post+destination) blocks a
/// duplicate cross-post if the same target is queued twice.
/// </summary>
public sealed class SyndicationDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);
    const int MaxAttempts = 6;
    public SyndicationDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { await DrainOnce(_db, 15); }
            catch (Exception e) { Console.Error.WriteLine($"[syndication] drain failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    static int Backoff(int attempt) => attempt switch { <= 1 => 30, 2 => 120, 3 => 300, 4 => 900, _ => 3600 };

    /// <summary>Process up to <paramref name="limit"/> due syndication jobs. Also callable by an admin retry.</summary>
    public static async Task<int> DrainOnce(Db db, int limit)
    {
        try { WorkerLease.RecoverExpired(db, "content_jobs", "retrying"); } catch { }
        var due = db.Query(@"SELECT id FROM content_jobs WHERE job_type='syndicate'
            AND status IN ('pending','retrying') AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))
            AND (lease_until IS NULL OR lease_until<=datetime('now'))
            ORDER BY id LIMIT ?", limit);
        var owner = WorkerLease.NewOwner();
        int done = 0;
        foreach (var row in due)
        {
            var jobId = H.L(row["id"]);
            if (!WorkerLease.TryClaim(db, "content_jobs", jobId, owner, "'pending','retrying'")) continue;
            var job = db.QueryOne("SELECT * FROM content_jobs WHERE id=?", jobId);
            if (job is null) continue;
            long synId = 0;
            try { using var d = System.Text.Json.JsonDocument.Parse(H.Str(job["payload"]) ?? "{}"); if (d.RootElement.TryGetProperty("syndicated_id", out var el)) synId = el.GetInt64(); } catch { }
            var syn = synId > 0 ? db.QueryOne("SELECT * FROM cc_syndicated_posts WHERE id=?", synId) : null;
            if (syn is null) { db.Execute("UPDATE content_jobs SET status='failed', last_error='row_missing', lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?", jobId); continue; }
            var dest = db.QueryOne("SELECT * FROM cc_syndication_destinations WHERE id=? AND active=1", H.L(syn["destination_id"]));
            var post = db.QueryOne("SELECT * FROM blog_posts WHERE id=?", H.L(syn["post_id"]));
            if (dest is null || post is null)
            {
                db.Execute("UPDATE content_jobs SET status='failed', last_error='dest_or_post_missing', lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?", jobId);
                db.Execute("UPDATE cc_syndicated_posts SET status='failed', last_error='destination or post unavailable', updated_at=datetime('now') WHERE id=?", synId);
                continue;
            }
            db.Execute("UPDATE cc_syndicated_posts SET status='publishing', updated_at=datetime('now') WHERE id=?", synId);
            var canonical = H.Str(syn["canonical_url"]);
            if (string.IsNullOrEmpty(canonical)) canonical = Blog.PublicUrl(db, H.Str(post["slug"]) ?? "");
            var tags = Blog.Tags(db, H.L(post["id"])).Select(t => H.Str(t["name"]) ?? "").Where(s => s.Length > 0).ToList();
            var pubStatus = H.Str(dest["default_status"]) == "published" ? "published" : "draft";
            var p = new SyndicationConnectors.Post(H.Str(post["title"]) ?? "", H.Str(post["body"]) ?? "", canonical ?? "", tags, pubStatus);
            var externalId = H.Str(syn["external_id"]);
            var res = await SyndicationConnectors.Publish(dest, p, externalId);
            if (res.Ok)
            {
                var mode = externalId is { Length: > 0 } ? "updated" : "published";
                db.Execute("UPDATE content_jobs SET status='delivered', response_code=?, result_ref=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?", res.Code, res.ExternalUrl, jobId);
                db.Execute("UPDATE cc_syndicated_posts SET status=?, external_id=COALESCE(?,external_id), external_url=?, canonical_url=?, last_error=NULL, provider_response='ok', updated_at=datetime('now') WHERE id=?",
                    mode, res.ExternalId, res.ExternalUrl, canonical, synId);
                db.Execute("UPDATE cc_syndication_destinations SET status='connected', last_error=NULL WHERE id=?", H.L(dest["id"]));
                done++;
            }
            else
            {
                var attempts = (int)H.L(job["attempts"]) + 1;
                var terminal = attempts >= MaxAttempts;
                var next = DateTime.UtcNow.AddSeconds(Backoff(attempts)).ToString("yyyy-MM-dd HH:mm:ss");
                db.Execute("UPDATE content_jobs SET status=?, attempts=?, response_code=?, last_error=?, next_attempt_at=?, lease_owner=NULL, lease_until=NULL, updated_at=datetime('now') WHERE id=?",
                    terminal ? "failed" : "retrying", attempts, res.Code, res.Error, next, jobId);
                db.Execute("UPDATE cc_syndicated_posts SET status=?, last_error=?, updated_at=datetime('now') WHERE id=?",
                    terminal ? "failed" : "retrying", res.Error, synId);
                db.Execute("UPDATE cc_syndication_destinations SET last_error=? WHERE id=?", res.Error, H.L(dest["id"]));
            }
        }
        return done;
    }
}
