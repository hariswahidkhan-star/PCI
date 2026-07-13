using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// ERP / integrations foundation (master-plan Phase 9). A durable, at-least-once event pipeline that
/// lets PCI push its business events (payments, memberships, new members) to external systems — an ERP,
/// an accounting package, or an automation bridge (Zapier / Make / n8n) that fans out to any of them.
///
/// Design:
///   • <see cref="Emit"/> writes a canonical event to the <c>integration_events</c> outbox, then fans
///     out one <c>integration_deliveries</c> row per enabled connector whose filter matches. Emitting
///     never throws — integration plumbing must never break a business flow.
///   • The generic <b>webhook</b> connector delivers a signed JSON POST (HMAC-SHA256), so it works
///     today against any HTTPS endpoint with no third-party credentials. Specific ERP connectors
///     (Phase 10) reuse the same outbox + delivery ledger, adding their own auth and field mapping.
///   • Delivery is retried with exponential backoff up to <see cref="MaxAttempts"/>; the
///     <c>IntegrationDispatcher</c> background service drains due deliveries, and the admin "test"/
///     "retry" endpoints call the same <see cref="DeliverOne"/> path — one delivery code path.
///
/// Secrets are stored per connector and are write-only through the API (never returned).
/// </summary>
public static class Integrations
{
    public const int MaxAttempts = 6;
    public static readonly string[] KnownEvents = { "payment.recorded", "membership.activated", "member.registered", "ping" };

    /// <summary>Record a canonical business event and queue it for every subscribed connector. Returns the
    /// event id (0 on failure). Never throws.</summary>
    public static long Emit(Db db, string eventType, string? entityType, long? entityId, object payload)
    {
        try
        {
            var json = JsonSerializer.Serialize(payload);
            var eventId = db.ExecuteReturningId(
                "INSERT INTO integration_events(event_type,entity_type,entity_id,payload) VALUES(?,?,?,?)",
                eventType, entityType, entityId, json);
            foreach (var i in db.Query("SELECT id,event_filter FROM integrations WHERE enabled=1"))
            {
                if (!Subscribes(H.Str(i["event_filter"]), eventType)) continue;
                db.Execute("INSERT OR IGNORE INTO integration_deliveries(event_id,integration_id,status,next_attempt_at) VALUES(?,?,'pending',datetime('now'))",
                    eventId, H.L(i["id"]));
            }
            return eventId;
        }
        catch (Exception e) { Console.Error.WriteLine($"[integrations] emit {eventType} failed: {e.Message}"); return 0; }
    }

    /// <summary>Empty/null filter = every event; otherwise the filter is a JSON array of subscribed event types.</summary>
    public static bool Subscribes(string? filter, string eventType)
    {
        if (string.IsNullOrWhiteSpace(filter)) return true;
        try
        {
            var arr = JsonSerializer.Deserialize<List<string>>(filter!);
            return arr is null || arr.Count == 0 || arr.Contains(eventType);
        }
        catch { return true; }   // malformed filter → fail open on config (never silently drop events)
    }

    /// <summary>HMAC-SHA256 (hex) over "{timestamp}.{body}" — the receiver recomputes it with the shared
    /// secret to verify the payload's authenticity and integrity.</summary>
    public static string Sign(string secret, string timestamp, string body)
    {
        using var h = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var mac = h.ComputeHash(Encoding.UTF8.GetBytes(timestamp + "." + body));
        return "sha256=" + Convert.ToHexString(mac).ToLowerInvariant();
    }

    static int BackoffSeconds(int attempt) => attempt switch { <= 1 => 30, 2 => 120, 3 => 300, 4 => 900, _ => 3600 };
    static string? Clip(string? s, int max = 500) => s is { Length: > 0 } && s.Length > max ? s[..max] : s;

    /// <summary>Deliver all pending deliveries whose next_attempt_at is due. Returns the count attempted.</summary>
    public static async Task<int> DeliverDue(Db db, HttpClient http, int max = 25)
    {
        var due = db.Query("SELECT * FROM integration_deliveries WHERE status='pending' AND (next_attempt_at IS NULL OR next_attempt_at<=datetime('now')) ORDER BY id LIMIT ?", max);
        foreach (var d in due) await DeliverOne(db, http, d);
        return due.Count;
    }

    /// <summary>Attempt a single delivery: build the signed envelope, POST it, and record the outcome
    /// (delivered, or pending-with-backoff, or failed after the attempt cap). Never throws.</summary>
    public static async Task DeliverOne(Db db, HttpClient http, Dictionary<string, object?> d)
    {
        var deliveryId = H.L(d["id"]);
        var integ = db.QueryOne("SELECT * FROM integrations WHERE id=?", H.L(d["integration_id"]));
        var ev = db.QueryOne("SELECT * FROM integration_events WHERE id=?", H.L(d["event_id"]));
        if (integ is null || ev is null)
        {
            db.Execute("UPDATE integration_deliveries SET status='failed', last_error='missing connector or event', updated_at=datetime('now') WHERE id=?", deliveryId);
            return;
        }
        var integId = H.L(integ["id"]);
        var attempts = (int)H.L(d["attempts"]) + 1;
        var url = (H.Str(integ["endpoint_url"]) ?? "").Trim();
        var eventType = H.Str(ev["event_type"]) ?? "";
        var payloadRaw = H.Str(ev["payload"]) ?? "{}";
        var occurredAt = H.Str(ev["created_at"]) ?? "";
        // Envelope: data is inlined as raw JSON (already serialized) so it isn't double-encoded.
        var body = $"{{\"event_id\":{H.L(ev["id"])},\"type\":{JsonSerializer.Serialize(eventType)}," +
                   $"\"occurred_at\":{JsonSerializer.Serialize(occurredAt)},\"data\":{payloadRaw}}}";
        try
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://")) throw new Exception("connector has no valid endpoint URL");
            var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            using var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new StringContent(body, Encoding.UTF8, "application/json") };
            req.Headers.TryAddWithoutValidation("User-Agent", "PCI-Integrations/1.0");
            req.Headers.TryAddWithoutValidation("X-PCI-Event", eventType);
            req.Headers.TryAddWithoutValidation("X-PCI-Delivery", deliveryId.ToString());
            req.Headers.TryAddWithoutValidation("X-PCI-Timestamp", ts);
            if (H.Str(integ["secret"]) is { Length: > 0 } secret)
                req.Headers.TryAddWithoutValidation("X-PCI-Signature", Sign(secret, ts, body));
            using var resp = await http.SendAsync(req);
            var code = (int)resp.StatusCode;
            if (resp.IsSuccessStatusCode)
            {
                db.Execute("UPDATE integration_deliveries SET status='delivered', attempts=?, response_code=?, last_error=NULL, updated_at=datetime('now') WHERE id=?", attempts, code, deliveryId);
                db.Execute("UPDATE integrations SET status='ok', last_delivery_at=datetime('now'), updated_at=datetime('now') WHERE id=?", integId);
            }
            else Fail(db, deliveryId, attempts, code, $"HTTP {code}", integId);
        }
        catch (Exception e) { Fail(db, deliveryId, attempts, null, e.Message, integId); }
    }

    static void Fail(Db db, long deliveryId, int attempts, int? code, string error, long integId)
    {
        if (attempts >= MaxAttempts)
            db.Execute("UPDATE integration_deliveries SET status='failed', attempts=?, response_code=?, last_error=?, updated_at=datetime('now') WHERE id=?",
                attempts, code, Clip(error), deliveryId);
        else
        {
            var next = DateTime.UtcNow.AddSeconds(BackoffSeconds(attempts)).ToString("yyyy-MM-dd HH:mm:ss");
            db.Execute("UPDATE integration_deliveries SET status='pending', attempts=?, response_code=?, last_error=?, next_attempt_at=?, updated_at=datetime('now') WHERE id=?",
                attempts, code, Clip(error), next, deliveryId);
        }
        db.Execute("UPDATE integrations SET status='error', updated_at=datetime('now') WHERE id=?", integId);
    }
}
