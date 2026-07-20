using Microsoft.Extensions.Hosting;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Background worker that delivers approved social posts. Every ~20s it claims due content_jobs of type
/// 'social_publish' (pending/retrying, past any backoff), publishes each draft through SocialConnectors,
/// and records the outcome on both the job and the draft. Failures are retried with exponential backoff up
/// to a cap, then marked failed — a failed social delivery NEVER touches the underlying blog article.
/// The idempotency_key on the job (draft id) blocks duplicate posts if a draft is queued twice.
/// </summary>
public sealed class SocialDispatcher : BackgroundService
{
    private readonly Db _db;
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(20);
    const int MaxAttempts = 6;
    public SocialDispatcher(Db db) => _db = db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);
        do
        {
            try { await DrainOnce(_db, 15); }
            catch (Exception e) { Console.Error.WriteLine($"[social] drain failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    { try { return await t.WaitForNextTickAsync(ct); } catch (OperationCanceledException) { return false; } }

    static int Backoff(int attempt) => attempt switch { <= 1 => 30, 2 => 120, 3 => 300, 4 => 900, _ => 3600 };

    /// <summary>Process up to <paramref name="limit"/> due social jobs. Also callable by an admin retry.</summary>
    public static async Task<int> DrainOnce(Db db, int limit)
    {
        var due = db.Query(@"SELECT * FROM content_jobs WHERE job_type='social_publish'
            AND status IN ('pending','retrying') AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now'))
            ORDER BY id LIMIT ?", limit);
        int done = 0;
        foreach (var job in due)
        {
            var jobId = H.L(job["id"]);
            long draftId = 0;
            try { using var d = System.Text.Json.JsonDocument.Parse(H.Str(job["payload"]) ?? "{}"); if (d.RootElement.TryGetProperty("draft_id", out var el)) draftId = el.GetInt64(); } catch { }
            var draft = draftId > 0 ? db.QueryOne("SELECT * FROM cc_social_drafts WHERE id=?", draftId) : null;
            if (draft is null) { db.Execute("UPDATE content_jobs SET status='failed', last_error='draft_missing', updated_at=datetime('now') WHERE id=?", jobId); continue; }
            var account = db.QueryOne("SELECT * FROM cc_social_accounts WHERE id=? AND active=1", H.L(draft["account_id"]));
            if (account is null)
            {
                db.Execute("UPDATE content_jobs SET status='failed', last_error='account_missing', updated_at=datetime('now') WHERE id=?", jobId);
                db.Execute("UPDATE cc_social_drafts SET status='failed', provider_response='account not connected', updated_at=datetime('now') WHERE id=?", draftId);
                continue;
            }
            db.Execute("UPDATE cc_social_drafts SET status='publishing', updated_at=datetime('now') WHERE id=?", draftId);
            var res = await SocialConnectors.Publish(db, account, H.Str(draft["text"]) ?? "", H.Str(draft["link"]));
            if (res.Ok)
            {
                db.Execute("UPDATE content_jobs SET status='delivered', response_code=?, result_ref=?, updated_at=datetime('now') WHERE id=?", res.Code, res.PublicUrl, jobId);
                db.Execute("UPDATE cc_social_drafts SET status='published', public_url=?, provider_response='ok', updated_at=datetime('now') WHERE id=?", res.PublicUrl, draftId);
                db.Execute("UPDATE cc_social_accounts SET status='connected', last_error=NULL WHERE id=?", H.L(account["id"]));
                done++;
            }
            else
            {
                var attempts = (int)H.L(job["attempts"]) + 1;
                var terminal = attempts >= MaxAttempts;
                var next = DateTime.UtcNow.AddSeconds(Backoff(attempts)).ToString("yyyy-MM-dd HH:mm:ss");
                db.Execute("UPDATE content_jobs SET status=?, attempts=?, response_code=?, last_error=?, next_attempt_at=?, updated_at=datetime('now') WHERE id=?",
                    terminal ? "failed" : "retrying", attempts, res.Code, res.Error, next, jobId);
                db.Execute("UPDATE cc_social_drafts SET status=?, provider_response=?, updated_at=datetime('now') WHERE id=?",
                    terminal ? "failed" : "retrying", res.Error, draftId);
                db.Execute("UPDATE cc_social_accounts SET last_error=? WHERE id=?", res.Error, H.L(account["id"]));
            }
        }
        return done;
    }
}
