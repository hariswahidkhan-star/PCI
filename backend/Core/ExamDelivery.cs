using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Exam Delivery integrations — the vendor-agnostic core that lets PCI drive a third-party certification
/// exam-delivery / proctoring platform (Pearson VUE/OnVUE, Kryterion Webassessor, PSI, TestReach,
/// Questionmark) through one canonical lifecycle:
///
///   upsert candidate → authorize (eligibility) → schedule appointment → (candidate sits) →
///   get status → get results → issue PCI credential on a pass.  (+ reschedule / cancel)
///
/// Every vendor exposes these same conceptual operations; the per-vendor <see cref="IExamDeliveryConnector"/>
/// implementations map each step onto that vendor's real endpoints + authentication. Configuration and the
/// (write-only) credentials live in exam_delivery_providers; each routed booking is an exam_delivery_orders
/// row carrying the external ids + lifecycle status; every API call is appended to exam_delivery_log.
///
/// Like the QuickBooks connector, each vendor has a sandbox and production base URL, and an optional
/// `api_base` override so the whole pipeline can be exercised against a local mock vendor server.
/// </summary>
public sealed record ConnResult(
    bool Ok,
    int? Code = null,
    string? Detail = null,
    string? CandidateId = null,
    string? RegistrationId = null,
    string? AppointmentId = null,
    string? ConfirmationCode = null,
    string? DeliveryStatus = null,      // vendor-normalised: scheduled|in_progress|delivered|no_show|cancelled
    string? ResultStatus = null,        // pass|fail (null = no result yet)
    double? Score = null,
    double? MaxScore = null,
    string? Raw = null)
{
    public static ConnResult Fail(string detail, int? code = null) => new(false, code, detail);
    public static ConnResult Good(string? detail = null) => new(true, 200, detail);
}

/// <summary>Parsed, non-DB view of a configured provider row: environment, its JSON config and the
/// write-only secret, plus typed accessors and the certification→vendor-exam-code map.</summary>
public sealed class ProviderCtx
{
    public long Id;
    public string Provider = "";
    public string Environment = "sandbox";       // sandbox | production
    public JsonElement Config;
    public JsonElement Secret;

    public bool IsProduction => Environment == "production";

    static string? Str(JsonElement e, string key)
        => e.ValueKind == JsonValueKind.Object && e.TryGetProperty(key, out var v)
           && v.ValueKind == JsonValueKind.String && v.GetString() is { Length: > 0 } s ? s : null;

    public string? Cfg(string key) => Str(Config, key);
    public string? Sec(string key) => Str(Secret, key);

    /// <summary>Optional per-provider override of the vendor's own base URL — used to point the whole
    /// pipeline at a local mock vendor server during validation (mirrors QuickBooks' api_base).</summary>
    public string? ApiBase => Cfg("api_base");

    /// <summary>The vendor's exam / assessment code for a PCI certification, from config.exam_map
    /// (keyed by certification id or code). Falls back to a single configured default_exam_code.</summary>
    public string? ExamCodeFor(long certId, string? certCode)
    {
        if (Config.ValueKind == JsonValueKind.Object && Config.TryGetProperty("exam_map", out var m) && m.ValueKind == JsonValueKind.Object)
        {
            foreach (var key in new[] { certId.ToString(), certCode })
                if (key is { Length: > 0 } k && m.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String && v.GetString() is { Length: > 0 } code)
                    return code;
        }
        return Cfg("default_exam_code");
    }
}

/// <summary>The mutable per-booking order passed to the connector: candidate identity, the PCI
/// certification, the resolved vendor exam code, the requested slot, and any external ids already
/// obtained from earlier lifecycle steps (so authorize/schedule can reference them).</summary>
public sealed class ExamOrder
{
    public long Id;
    public long UserId;
    public long? BookingId;
    public long CertificationId;
    public string Email = "";
    public string FirstName = "";
    public string LastName = "";
    public string? Country;
    public string? Phone;
    public string VendorExamCode = "";
    public string DeliveryType = "online";       // online | test_centre
    public string? ScheduledAt;                   // ISO-8601 UTC
    public string? Timezone;
    public string? ExternalCandidateId;
    public string? ExternalRegistrationId;
    public string? ExternalAppointmentId;
    public string? ConfirmationCode;
}

/// <summary>The contract every exam-delivery vendor connector implements. Each method performs ONE vendor
/// API call and returns a normalised <see cref="ConnResult"/>; the orchestrator persists the ids/status
/// and logs the outcome. Implementations must never fabricate success — a missing credential or a non-2xx
/// response is a failed ConnResult with a clear message.</summary>
public interface IExamDeliveryConnector
{
    string Slug { get; }
    string Label { get; }
    string Description { get; }
    /// <summary>Non-secret config keys the admin UI collects (besides environment + exam_map + api_base).</summary>
    string[] ConfigFields { get; }
    /// <summary>Write-only secret keys (API keys, OAuth secrets, SFTP keys).</summary>
    string[] SecretFields { get; }

    Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p);
    Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o);
    Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o);
}

/// <summary>Registry + orchestrator. The registry is filled by the per-vendor connectors
/// (ExamDeliveryConnectors.cs). The orchestrator drives the canonical lifecycle over the DB rows,
/// persisting external ids + status and logging every call.</summary>
public static class ExamDelivery
{
    /// <summary>Shared client for lifecycle calls made off the student booking path (best-effort).</summary>
    public static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

    // provider slug → connector. Populated by ExamDeliveryConnectors.Register() at startup.
    static readonly Dictionary<string, IExamDeliveryConnector> _reg = new(StringComparer.OrdinalIgnoreCase);

    public static void Register(IExamDeliveryConnector c) => _reg[c.Slug] = c;
    public static IExamDeliveryConnector? Get(string? provider) =>
        provider is { Length: > 0 } p && _reg.TryGetValue(p, out var c) ? c : null;
    public static IEnumerable<IExamDeliveryConnector> All => _reg.Values;

    public static ProviderCtx CtxFor(Dictionary<string, object?> providerRow)
    {
        JsonElement Parse(string? s) { try { return JsonDocument.Parse(string.IsNullOrWhiteSpace(s) ? "{}" : s!).RootElement.Clone(); } catch { return JsonDocument.Parse("{}").RootElement.Clone(); } }
        return new ProviderCtx
        {
            Id = H.L(providerRow["id"]),
            Provider = H.Str(providerRow["provider"]) ?? "",
            Environment = H.Str(providerRow["environment"]) is { Length: > 0 } e ? e : "sandbox",
            Config = Parse(H.Str(providerRow["config"])),
            Secret = Parse(H.Str(providerRow["secret"])),
        };
    }

    /// <summary>Build the connector order view from an exam_delivery_orders row, hydrating candidate
    /// identity from the user + profile so the vendor gets a real name/email/country.</summary>
    public static ExamOrder OrderFor(Db db, Dictionary<string, object?> row)
    {
        var userId = H.L(row["user_id"]);
        var user = db.QueryOne("SELECT email,first_name,last_name FROM users WHERE id=?", userId) ?? new();
        var prof = db.QueryOne("SELECT country,mobile FROM student_profiles WHERE user_id=?", userId);
        return new ExamOrder
        {
            Id = H.L(row["id"]),
            UserId = userId,
            BookingId = row["booking_id"] is null ? null : H.L(row["booking_id"]),
            CertificationId = H.L(row["certification_id"]),
            Email = H.Str(user.GetValueOrDefault("email")) ?? "",
            FirstName = H.Str(user.GetValueOrDefault("first_name")) ?? "",
            LastName = H.Str(user.GetValueOrDefault("last_name")) ?? "",
            Country = H.Str(prof?.GetValueOrDefault("country")),
            Phone = H.Str(prof?.GetValueOrDefault("mobile")),
            VendorExamCode = H.Str(row["vendor_exam_code"]) ?? "",
            DeliveryType = H.Str(row["delivery_type"]) is { Length: > 0 } dt ? dt : "online",
            ScheduledAt = H.Str(row["scheduled_at"]),
            Timezone = H.Str(row["timezone"]),
            ExternalCandidateId = H.Str(row["external_candidate_id"]),
            ExternalRegistrationId = H.Str(row["external_registration_id"]),
            ExternalAppointmentId = H.Str(row["external_appointment_id"]),
            ConfirmationCode = H.Str(row["confirmation_code"]),
        };
    }

    /// <summary>Route a PCI exam booking to the configured default delivery vendor: create the order and
    /// run the candidate→authorize→schedule pipeline. Best-effort and idempotent — if no default provider
    /// is enabled, or the certification isn't mapped to a vendor exam code, this is a no-op and the PCI
    /// booking stands on its own. Never throws into the booking path.</summary>
    public static async Task RouteBooking(Db db, long bookingId, long userId, long certId, string? scheduledAt, string? timezone)
    {
        try
        {
            var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE enabled=1 AND is_default=1 ORDER BY id DESC LIMIT 1");
            if (prov is null) return;
            var providerId = H.L(prov["id"]);
            if (db.QueryOne("SELECT id FROM exam_delivery_orders WHERE booking_id=? AND provider_id=?", bookingId, providerId) is not null) return;
            var p = CtxFor(prov);
            var certCode = H.Str(db.QueryOne("SELECT code FROM certifications WHERE id=?", certId)?["code"]);
            var examCode = p.ExamCodeFor(certId, certCode);
            if (string.IsNullOrEmpty(examCode)) return;   // certification not mapped to this vendor → skip cleanly
            var deliveryType = p.Cfg("delivery_type") is { Length: > 0 } dt && dt.Contains("centre", StringComparison.OrdinalIgnoreCase) ? "test_centre" : "online";
            var orderId = db.ExecuteReturningId(@"INSERT INTO exam_delivery_orders(provider_id,provider,user_id,booking_id,certification_id,vendor_exam_code,delivery_type,scheduled_at,timezone,status)
                                                  VALUES(?,?,?,?,?,?,?,?,?, 'pending')",
                providerId, p.Provider, userId, bookingId, certId, examCode, deliveryType, scheduledAt, timezone);
            await Provision(db, Http, orderId);
        }
        catch { /* delivery routing is best-effort; the PCI booking must never fail because of it */ }
    }

    /// <summary>Propagate a PCI reschedule to the vendor for the booking's order (best-effort).</summary>
    public static async Task RescheduleBooking(Db db, long bookingId, string? scheduledAt, string? timezone)
    {
        try
        {
            var order = db.QueryOne("SELECT * FROM exam_delivery_orders WHERE booking_id=? AND status NOT IN ('cancelled','completed') ORDER BY id DESC", bookingId);
            if (order is null) return;
            db.Execute("UPDATE exam_delivery_orders SET scheduled_at=?, timezone=?, updated_at=datetime('now') WHERE id=?", scheduledAt, timezone, H.L(order["id"]));
            var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE id=?", order["provider_id"]);
            var connector = Get(H.Str(prov?["provider"]));
            if (connector is null || prov is null) return;
            var o = OrderFor(db, db.QueryOne("SELECT * FROM exam_delivery_orders WHERE id=?", order["id"])!);
            var r = await connector.Reschedule(Http, CtxFor(prov), o);
            Log(db, o.Id, CtxFor(prov).Id, H.Str(prov["provider"]) ?? "", "reschedule", r);
        }
        catch { }
    }

    static void Log(Db db, long orderId, long providerId, string provider, string op, ConnResult r)
    {
        try
        {
            db.Execute("INSERT INTO exam_delivery_log(order_id,provider_id,provider,operation,ok,response_code,detail) VALUES(?,?,?,?,?,?,?)",
                orderId, providerId, provider, op, r.Ok ? 1 : 0, r.Code, (r.Detail ?? "").Length > 900 ? r.Detail![..900] : r.Detail);
        }
        catch { }
    }

    // Persist any external ids a step returned back onto the order + the connector view.
    static void Absorb(Db db, ExamOrder o, ConnResult r)
    {
        if (r.CandidateId is { Length: > 0 }) { o.ExternalCandidateId = r.CandidateId; db.Execute("UPDATE exam_delivery_orders SET external_candidate_id=? WHERE id=?", r.CandidateId, o.Id); }
        if (r.RegistrationId is { Length: > 0 }) { o.ExternalRegistrationId = r.RegistrationId; db.Execute("UPDATE exam_delivery_orders SET external_registration_id=? WHERE id=?", r.RegistrationId, o.Id); }
        if (r.AppointmentId is { Length: > 0 }) { o.ExternalAppointmentId = r.AppointmentId; db.Execute("UPDATE exam_delivery_orders SET external_appointment_id=? WHERE id=?", r.AppointmentId, o.Id); }
        if (r.ConfirmationCode is { Length: > 0 }) { o.ConfirmationCode = r.ConfirmationCode; db.Execute("UPDATE exam_delivery_orders SET confirmation_code=? WHERE id=?", r.ConfirmationCode, o.Id); }
    }

    static void SetStatus(Db db, long orderId, string status, string? err = null) =>
        db.Execute("UPDATE exam_delivery_orders SET status=?, last_error=?, updated_at=datetime('now') WHERE id=?", status, err, orderId);

    /// <summary>Drive an order from wherever it is toward 'scheduled': upsert candidate → authorize →
    /// schedule, persisting ids + logging each call and stopping at the first failure. Idempotent: a step
    /// whose external id already exists is skipped, so a retry resumes rather than duplicating.</summary>
    public static async Task<ConnResult> Provision(Db db, HttpClient http, long orderId)
    {
        var row = db.QueryOne("SELECT * FROM exam_delivery_orders WHERE id=?", orderId);
        if (row is null) return ConnResult.Fail("order not found", 404);
        var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE id=?", row["provider_id"]);
        if (prov is null) return ConnResult.Fail("provider not found", 404);
        var connector = Get(H.Str(prov["provider"]));
        if (connector is null) { SetStatus(db, orderId, "failed", "no connector for provider"); return ConnResult.Fail("no connector for provider"); }
        if (H.L(prov["enabled"]) != 1) { SetStatus(db, orderId, "failed", "provider disabled"); return ConnResult.Fail("provider disabled"); }

        var p = CtxFor(prov);
        var o = OrderFor(db, row);

        async Task<ConnResult> Step(string op, string reachedStatus, bool skip, Func<Task<ConnResult>> call)
        {
            if (skip) return ConnResult.Good("already done");
            ConnResult r;
            try { r = await call(); } catch (Exception ex) { r = ConnResult.Fail(ex.Message); }
            Log(db, o.Id, p.Id, p.Provider, op, r);
            if (r.Ok)
            {
                Absorb(db, o, r);
                // Prefer the connector's own delivery status when it's a recognised lifecycle state (so a
                // candidate-driven vendor lands on 'awaiting_candidate_schedule', not a flat 'scheduled').
                var ns = r.DeliveryStatus is { Length: > 0 } d && d is "scheduled" or "awaiting_candidate_schedule" or "delivered" or "cancelled" ? d : reachedStatus;
                if (ns.Length > 0) SetStatus(db, o.Id, ns);
            }
            else SetStatus(db, o.Id, "failed", $"{op}: {r.Detail}");
            return r;
        }

        var c = await Step("candidate", "candidate_created", o.ExternalCandidateId is { Length: > 0 }, () => connector.UpsertCandidate(http, p, o));
        if (!c.Ok) return c;
        var a = await Step("authorize", "authorized", o.ExternalRegistrationId is { Length: > 0 }, () => connector.Authorize(http, p, o));
        if (!a.Ok) return a;
        var s = await Step("schedule", "scheduled", o.ExternalAppointmentId is { Length: > 0 }, () => connector.Schedule(http, p, o));
        return s;
    }

    /// <summary>Pull the latest status + results for an order. On a graded pass the caller issues the PCI
    /// credential (wired in the scheduling layer); here we persist result_status/score and advance status.</summary>
    public static async Task<ConnResult> Sync(Db db, HttpClient http, long orderId)
    {
        var row = db.QueryOne("SELECT * FROM exam_delivery_orders WHERE id=?", orderId);
        if (row is null) return ConnResult.Fail("order not found", 404);
        var prov = db.QueryOne("SELECT * FROM exam_delivery_providers WHERE id=?", row["provider_id"]);
        if (prov is null) return ConnResult.Fail("provider not found", 404);
        var connector = Get(H.Str(prov["provider"]));
        if (connector is null) return ConnResult.Fail("no connector for provider");
        var p = CtxFor(prov);
        var o = OrderFor(db, row);

        ConnResult st; try { st = await connector.GetStatus(http, p, o); } catch (Exception ex) { st = ConnResult.Fail(ex.Message); }
        Log(db, o.Id, p.Id, p.Provider, "status", st);
        if (st.Ok && st.DeliveryStatus is { Length: > 0 } ds)
            db.Execute("UPDATE exam_delivery_orders SET status=CASE WHEN status IN ('completed') THEN status ELSE ? END, updated_at=datetime('now') WHERE id=?",
                ds == "delivered" ? "delivered" : ds == "cancelled" ? "cancelled" : H.Str(row["status"]), o.Id);

        ConnResult rr; try { rr = await connector.GetResults(http, p, o); } catch (Exception ex) { rr = ConnResult.Fail(ex.Message); }
        Log(db, o.Id, p.Id, p.Provider, "results", rr);
        db.Execute("UPDATE exam_delivery_providers SET last_sync_at=datetime('now') WHERE id=?", p.Id);
        if (rr.Ok && rr.ResultStatus is { Length: > 0 } res)
        {
            db.Execute("UPDATE exam_delivery_orders SET result_status=?, score=?, max_score=?, raw_result=?, status='completed', updated_at=datetime('now') WHERE id=?",
                res, rr.Score, rr.MaxScore, rr.Raw is { Length: > 4000 } ? rr.Raw[..4000] : rr.Raw, o.Id);
        }
        return rr.Ok ? rr : st;
    }
}
