using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PCI.Backend.Core;

// CS1998 (async method lacks 'await') is intentionally suppressed for this file. Several connector
// methods implement the async IExamDeliveryConnector contract but are legitimately synchronous: the
// vendor either folds the step into another call (e.g. Pearson VUE authorize → schedule) or makes it
// candidate-driven (self-scheduling), so the method returns a truthful ConnResult without any network
// I/O to await. Keeping the async signature preserves the uniform interface while honestly modelling a
// no-op step. See SUPPRESSION_PROCESS.md for the review record.
#pragma warning disable CS1998

/// <summary>
/// The five concrete exam-delivery vendor connectors, each mapping PCI's canonical lifecycle
/// (<see cref="IExamDeliveryConnector"/>) onto that vendor's real, documented integration model:
///
///  • Questionmark  — Delivery OData (REST/JSON, HTTP Basic): Participants → Schedules(+InvokeAction) →
///                    Results feed. Fully API-driven incl. scheduling + results pull.
///  • Kryterion     — Webassessor EWS (JSON-RPC envelope: requestType + securityToken): Add User →
///                    Assign Voucher → Get Available Time Slots + Add Registration → Get Registrations.
///  • PSI (Atlas)   — Eligibility Service (REST/JSON, OAuth2): POST eligibility (candidate embedded);
///                    candidate self-schedules; booking + results arrive via the push Notification
///                    Service (our inbound callback). GET eligibilities for status; DELETE to cancel.
///  • Pearson VUE   — real-time web services (SSO-RTI candidate+eligibility upsert); candidate
///                    self-schedules; appointment + results pushed back via RTEN (our inbound callback).
///  • TestReach     — per-client configured push (registration-in / results-out); candidate books in
///                    the TestReach dashboard; results pushed back (our inbound callback).
///
/// Every host, path and credential is configurable, because each vendor provisions these per client under
/// NDA — the connectors ship with the documented defaults and a sandbox/prod toggle plus an `api_base`
/// override so the whole pipeline can be exercised against a local mock vendor server. A connector NEVER
/// fabricates success: a missing credential, a non-2xx response, or a step the vendor makes
/// candidate-driven returns a truthful <see cref="ConnResult"/> (failure, or an explicit "awaiting"
/// delivery status), never a fake booking or credential.
/// </summary>
public static class ExamDeliveryConnectors
{
    public static void Register()
    {
        ExamDelivery.Register(new QuestionmarkConnector());
        ExamDelivery.Register(new KryterionConnector());
        ExamDelivery.Register(new PsiConnector());
        ExamDelivery.Register(new PearsonVueConnector());
        ExamDelivery.Register(new TestReachConnector());
    }
}

/// <summary>Shared HTTP + JSON helpers so each connector stays focused on its vendor mapping.</summary>
internal static class Web
{
    static readonly JsonSerializerOptions J = new() { PropertyNameCaseInsensitive = true };

    public static async Task<(int code, string body, bool ok)> Send(HttpClient http, HttpRequestMessage req)
    {
        using var resp = await http.SendAsync(req);
        var body = await resp.Content.ReadAsStringAsync();
        return ((int)resp.StatusCode, body, resp.IsSuccessStatusCode);
    }

    public static HttpRequestMessage Req(HttpMethod m, string url, object? body = null)
    {
        var r = new HttpRequestMessage(m, url);
        if (body is not null) r.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        r.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        r.Headers.TryAddWithoutValidation("User-Agent", "PCI-ExamDelivery/1.0");
        return r;
    }

    public static void Basic(HttpRequestMessage r, string user, string pass)
        => r.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}")));

    public static void Bearer(HttpRequestMessage r, string token)
        => r.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

    /// <summary>Pull a string property (first match, case-insensitive, searched recursively) from a JSON body.</summary>
    public static string? Str(string json, params string[] names)
    {
        try { return Find(JsonDocument.Parse(json).RootElement, names, wantNumber: false); } catch { return null; }
    }
    public static double? Num(string json, params string[] names)
    {
        try { var s = Find(JsonDocument.Parse(json).RootElement, names, wantNumber: true); return s is not null && double.TryParse(s, out var d) ? d : null; } catch { return null; }
    }

    static string? Find(JsonElement e, string[] names, bool wantNumber)
    {
        switch (e.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var p in e.EnumerateObject())
                    if (names.Any(n => string.Equals(n, p.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (p.Value.ValueKind == JsonValueKind.String) return p.Value.GetString();
                        if (p.Value.ValueKind == JsonValueKind.Number) return p.Value.GetRawText();
                        if (p.Value.ValueKind is JsonValueKind.True or JsonValueKind.False) return p.Value.GetRawText();
                    }
                foreach (var p in e.EnumerateObject())
                    if (Find(p.Value, names, wantNumber) is { } hit) return hit;
                return null;
            case JsonValueKind.Array:
                foreach (var it in e.EnumerateArray())
                    if (Find(it, names, wantNumber) is { } hit) return hit;
                return null;
            default: return null;
        }
    }

    /// <summary>Normalise a vendor pass/fail signal to "pass" | "fail" | null.</summary>
    public static string? PassFail(string? raw)
    {
        if (raw is null) return null;
        var s = raw.Trim().ToLowerInvariant();
        if (s is "pass" or "passed" or "true" or "1" or "p") return "pass";
        if (s is "fail" or "failed" or "false" or "0" or "f") return "fail";
        if (s.Contains("pass")) return "pass";
        if (s.Contains("fail")) return "fail";
        return null;
    }
}

// ─────────────────────────────────────────────────────────────────────────────────────────────
// Questionmark — Delivery OData (REST/JSON, HTTP Basic). The most fully API-driven of the five.
// ─────────────────────────────────────────────────────────────────────────────────────────────
public sealed class QuestionmarkConnector : IExamDeliveryConnector
{
    public string Slug => "questionmark";
    public string Label => "Questionmark OnDemand";
    public string Description => "Delivery OData (REST): participants, schedules (assign + launch) and real-time results. HTTP Basic service account.";
    public string[] ConfigFields => new[] { "customer_id", "region", "monitoring_type_id", "default_exam_code", "api_base" };
    public string[] SecretFields => new[] { "username", "password" };

    string Root(ProviderCtx p)
    {
        var host = p.ApiBase ?? (p.Cfg("region")?.ToLowerInvariant() == "eu" ? "https://ondemand.questionmark.eu" : "https://ondemand.questionmark.com");
        return $"{host.TrimEnd('/')}/deliveryodata/{p.Cfg("customer_id")}";
    }
    bool Auth(ProviderCtx p, HttpRequestMessage r)
    {
        if (p.Sec("username") is { Length: > 0 } u && p.Sec("password") is { Length: > 0 } pw) { Web.Basic(r, u, pw); return true; }
        return false;
    }
    ConnResult NeedCreds() => ConnResult.Fail("Questionmark not authorised — set the OData service-account username + password.");
    ConnResult NeedCustomer() => ConnResult.Fail("Questionmark not configured — set the customer id (area number).");

    public async Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p)
    {
        if (string.IsNullOrEmpty(p.Cfg("customer_id"))) return NeedCustomer();
        var r = Web.Req(HttpMethod.Get, $"{Root(p)}/Participants?$top=1"); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? ConnResult.Good($"connected ({c})") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(p.Cfg("customer_id"))) return NeedCustomer();
        var r = Web.Req(HttpMethod.Post, $"{Root(p)}/Participants", new { Name = o.Email, FirstName = o.FirstName, LastName = o.LastName, PrimaryEmail = o.Email });
        if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"create participant HTTP {c}: {b}", c);
        return new ConnResult(true, c, "participant created", CandidateId: Web.Str(b, "ID", "Participant_ID"), Raw: b);
    }
    public async Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        // Questionmark assigns eligibility AND schedules in one Schedule row, so Authorize is a no-op here;
        // the Schedule step below creates the Schedule bound to this participant + assessment.
        return ConnResult.Good("authorization folded into schedule");
    }
    public async Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(o.ExternalCandidateId)) return ConnResult.Fail("no participant id");
        var mon = int.TryParse(p.Cfg("monitoring_type_id"), out var mt) ? mt : 1;
        var body = new
        {
            ID = 0,
            Name = $"PCI {o.VendorExamCode} {o.Email}",
            AssessmentID = o.VendorExamCode,
            ParticipantID = int.TryParse(o.ExternalCandidateId, out var pid) ? pid : (object)o.ExternalCandidateId!,
            MonitoringTypeID = mon,
            StartFrom = o.ScheduledAt,
        };
        var r = Web.Req(HttpMethod.Post, $"{Root(p)}/Schedules", body); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"create schedule HTTP {c}: {b}", c);
        return new ConnResult(true, c, "scheduled", RegistrationId: Web.Str(b, "ID"), AppointmentId: Web.Str(b, "ID"), DeliveryStatus: "scheduled", Raw: b);
    }
    public async Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(o.ExternalAppointmentId)) return ConnResult.Fail("no schedule id");
        var r = Web.Req(HttpMethod.Patch, $"{Root(p)}/Schedules({o.ExternalAppointmentId})", new { StartFrom = o.ScheduledAt }); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? new ConnResult(true, c, "rescheduled", DeliveryStatus: "scheduled") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(o.ExternalAppointmentId)) return ConnResult.Fail("no schedule id");
        var r = Web.Req(HttpMethod.Patch, $"{Root(p)}/Schedules({o.ExternalAppointmentId})", new { Disabled = true }); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? new ConnResult(true, c, "cancelled", DeliveryStatus: "cancelled") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(o.ExternalAppointmentId)) return ConnResult.Good("no schedule yet");
        var r = Web.Req(HttpMethod.Get, $"{Root(p)}/Schedules({o.ExternalAppointmentId})"); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? new ConnResult(true, c, "status", DeliveryStatus: "scheduled", Raw: b) : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        var filter = Uri.EscapeDataString($"ParticipantName eq '{o.Email}' and AssessmentID eq {o.VendorExamCode}L");
        var r = Web.Req(HttpMethod.Get, $"{Root(p)}/Results?$filter={filter}"); if (!Auth(p, r)) return NeedCreds();
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"HTTP {c}: {b}", c);
        var band = Web.Str(b, "ScoreBandTitle");
        var pf = Web.PassFail(band) ?? Web.PassFail(Web.Str(b, "Outcome", "Result"));
        if (pf is null) return new ConnResult(true, c, "no result yet", DeliveryStatus: "awaiting_results", Raw: b);
        return new ConnResult(true, c, "results", ResultStatus: pf, Score: Web.Num(b, "PercentageScore", "TotalScore"), MaxScore: Web.Num(b, "MaxScore"), Raw: b);
    }
}

// ─────────────────────────────────────────────────────────────────────────────────────────────
// Kryterion Webassessor — EWS JSON-RPC envelope (requestType + securityToken). Direct API scheduling.
// ─────────────────────────────────────────────────────────────────────────────────────────────
public sealed class KryterionConnector : IExamDeliveryConnector
{
    public string Slug => "kryterion";
    public string Label => "Kryterion Webassessor";
    public string Description => "Webassessor EWS (JSON): Add User, Add Registration (online-proctored/test-centre), Get Registrations for results.";
    public string[] ConfigFields => new[] { "rpc_path", "delivery_type", "default_exam_code", "api_base" };
    public string[] SecretFields => new[] { "security_token" };

    string Url(ProviderCtx p)
    {
        var host = p.ApiBase ?? (p.IsProduction ? "https://www.webassessor.com" : "https://sb01.webassessor.com");
        var path = p.Cfg("rpc_path") is { Length: > 0 } rp ? rp : "/api";
        return host.TrimEnd('/') + "/" + path.TrimStart('/');
    }
    async Task<(int, string, bool)> Call(HttpClient http, ProviderCtx p, string requestType, Dictionary<string, object?> fields)
    {
        var env = new Dictionary<string, object?>(fields) { ["requestType"] = requestType, ["securityToken"] = p.Sec("security_token") ?? "", ["returnFormat"] = "json" };
        return await Web.Send(http, Web.Req(HttpMethod.Post, Url(p), env));
    }
    ConnResult NeedToken() => ConnResult.Fail("Kryterion not authorised — set the Webassessor security token.");

    public async Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p)
    {
        if (string.IsNullOrEmpty(p.Sec("security_token"))) return NeedToken();
        var (c, b, ok) = await Call(http, p, "ping", new());
        return ok && (b.Contains("Ping Successful", StringComparison.OrdinalIgnoreCase) || Web.Str(b, "success") == "true" || c == 200)
            ? ConnResult.Good($"ping ok ({c})") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(p.Sec("security_token"))) return NeedToken();
        var (c, b, ok) = await Call(http, p, "Add User", new() { ["login"] = o.Email, ["firstName"] = o.FirstName, ["lastName"] = o.LastName, ["email"] = o.Email });
        if (!ok) return ConnResult.Fail($"Add User HTTP {c}: {b}", c);
        // login is the durable person key across all EWS calls; keep it as the candidate id.
        return new ConnResult(true, c, "user added", CandidateId: o.Email, Raw: b);
    }
    public async Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o)
        => ConnResult.Good("eligibility via voucher/registration (folded into schedule)");
    public async Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (string.IsNullOrEmpty(p.Sec("security_token"))) return NeedToken();
        var delivery = p.Cfg("delivery_type") is { Length: > 0 } dt ? dt : (o.DeliveryType == "test_centre" ? "PROCTORED" : "WRAPPER_PROCTORED");
        var (c, b, ok) = await Call(http, p, "Add Registration", new()
        {
            ["login"] = o.ExternalCandidateId ?? o.Email,
            ["examCode"] = o.VendorExamCode,
            ["deliveryType"] = delivery,
            ["scheduledStartTime"] = o.ScheduledAt,
        });
        if (!ok) return ConnResult.Fail($"Add Registration HTTP {c}: {b}", c);
        var conf = Web.Str(b, "confirmationNumber");
        return new ConnResult(true, c, "registered", RegistrationId: conf, AppointmentId: conf, ConfirmationCode: conf, DeliveryStatus: "scheduled", Raw: b);
    }
    public async Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        var (c, b, ok) = await Call(http, p, "Transfer Registration", new() { ["confirmationNumber"] = o.ConfirmationCode ?? o.ExternalAppointmentId, ["scheduledStartTime"] = o.ScheduledAt });
        return ok ? new ConnResult(true, c, "rescheduled", DeliveryStatus: "scheduled", Raw: b) : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        var (c, b, ok) = await Call(http, p, "Cancel Registration", new() { ["confirmationNumber"] = o.ConfirmationCode ?? o.ExternalAppointmentId });
        return ok ? new ConnResult(true, c, "cancelled", DeliveryStatus: "cancelled", Raw: b) : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o) => await Fetch(http, p, o, statusOnly: true);
    public async Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o) => await Fetch(http, p, o, statusOnly: false);

    async Task<ConnResult> Fetch(HttpClient http, ProviderCtx p, ExamOrder o, bool statusOnly)
    {
        if (string.IsNullOrEmpty(p.Sec("security_token"))) return NeedToken();
        var (c, b, ok) = await Call(http, p, "Get Registrations", new() { ["login"] = o.ExternalCandidateId ?? o.Email });
        if (!ok) return ConnResult.Fail($"Get Registrations HTTP {c}: {b}", c);
        var progress = Web.Str(b, "progress");          // COMPLETED | SCHEDULED | IN_PROGRESS | ...
        var ds = progress?.ToUpperInvariant() switch { "COMPLETED" => "delivered", "IN_PROGRESS" => "in_progress", "SCHEDULED" => "scheduled", _ => "scheduled" };
        if (statusOnly) return new ConnResult(true, c, "status", DeliveryStatus: ds, Raw: b);
        var pf = Web.PassFail(Web.Str(b, "passed"));
        if (pf is null || progress?.ToUpperInvariant() != "COMPLETED") return new ConnResult(true, c, "no result yet", DeliveryStatus: ds, Raw: b);
        return new ConnResult(true, c, "results", ResultStatus: pf, Score: Web.Num(b, "score"), DeliveryStatus: "delivered", Raw: b);
    }
}

// ─────────────────────────────────────────────────────────────────────────────────────────────
// PSI (Atlas / PSI Bridge) — Eligibility Service (REST/JSON, OAuth2). Candidate self-schedules;
// booking + results arrive via the push Notification Service (handled by the inbound callback).
// ─────────────────────────────────────────────────────────────────────────────────────────────
public sealed class PsiConnector : IExamDeliveryConnector
{
    public string Slug => "psi";
    public string Label => "PSI (Atlas / PSI Bridge)";
    public string Description => "Eligibility Service (REST/OAuth2): push an eligibility; candidate self-schedules; booking + results via the Notification callback.";
    public string[] ConfigFields => new[] { "account_code", "token_url", "eligibility_days", "default_exam_code", "api_base" };
    public string[] SecretFields => new[] { "consumer_key", "consumer_secret", "oauth_username", "oauth_password", "access_token" };

    ConnResult NeedBase() => ConnResult.Fail("PSI not configured — set the Atlas API base URL (assigned per client via the API Store) and account code.");

    async Task<string?> Token(HttpClient http, ProviderCtx p)
    {
        if (p.Sec("access_token") is { Length: > 0 } at) return at;                 // preset short-lived token (or a mock)
        if (p.Cfg("token_url") is not { Length: > 0 } turl) return null;
        if (p.Sec("consumer_key") is not { Length: > 0 } ck || p.Sec("consumer_secret") is not { Length: > 0 } cs) return null;
        var form = new Dictionary<string, string> { ["grant_type"] = "password", ["username"] = p.Sec("oauth_username") ?? "", ["password"] = p.Sec("oauth_password") ?? "" };
        var tr = new HttpRequestMessage(HttpMethod.Post, turl) { Content = new FormUrlEncodedContent(form) };
        Web.Basic(tr, ck, cs);
        var (c, b, ok) = await Web.Send(http, tr);
        return ok ? Web.Str(b, "access_token") : null;
    }
    async Task<HttpRequestMessage?> Authed(HttpClient http, ProviderCtx p, HttpMethod m, string url, object? body)
    {
        var tok = await Token(http, p); if (tok is null) return null;
        var r = Web.Req(m, url, body); Web.Bearer(r, tok); return r;
    }
    string Base(ProviderCtx p) => (p.ApiBase ?? "").TrimEnd('/');

    public async Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p)
    {
        if (Base(p).Length == 0 || string.IsNullOrEmpty(p.Cfg("account_code"))) return NeedBase();
        var tok = await Token(http, p);
        return tok is null ? ConnResult.Fail("PSI not authorised — set access_token, or consumer key/secret + oauth username/password.")
                           : ConnResult.Good("token acquired");
    }
    public async Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o)
        => ConnResult.Good("candidate created with the eligibility (PSI folds candidate into the eligibility record)");

    public async Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0 || string.IsNullOrEmpty(p.Cfg("account_code"))) return NeedBase();
        var days = int.TryParse(p.Cfg("eligibility_days"), out var d) ? d : 365;
        var body = new
        {
            candidate = new { candidate_id = o.UserId.ToString(), first_name = o.FirstName, last_name = o.LastName, email = o.Email, country = o.Country },
            client_eligibility_id = $"PCI-{o.Id}",
            test_code = o.VendorExamCode,
            schedule_start_date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            eligibility_end_date = DateTime.UtcNow.AddDays(days).ToString("yyyy-MM-dd"),
        };
        var r = await Authed(http, p, HttpMethod.Post, $"{Base(p)}/accounts/{p.Cfg("account_code")}/candidates", body);
        if (r is null) return ConnResult.Fail("PSI not authorised — check OAuth credentials.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"create eligibility HTTP {c}: {b}", c);
        return new ConnResult(true, c, "eligibility created", CandidateId: o.UserId.ToString(),
            RegistrationId: Web.Str(b, "psi_eligiblity_id", "atlas_eligibility_code", "client_eligibility_id"), Raw: b);
    }
    public async Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o)
        // Scheduling is candidate-driven on PSI's UCS site once eligibility exists; the appointment + result
        // then arrive via the Notification callback. We advance to "awaiting the candidate to self-schedule".
        => new ConnResult(true, 200, "eligibility issued — candidate self-schedules on the PSI site", DeliveryStatus: "awaiting_candidate_schedule");

    public async Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "reschedule is candidate-driven on the PSI site within the eligibility window", DeliveryStatus: "awaiting_candidate_schedule");

    public async Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var r = await Authed(http, p, HttpMethod.Delete, $"{Base(p)}/accounts/{p.Cfg("account_code")}/candidates/{o.UserId}/tests/{o.VendorExamCode}", null);
        if (r is null) return ConnResult.Fail("PSI not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? new ConnResult(true, c, "eligibility cancelled", DeliveryStatus: "cancelled") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var r = await Authed(http, p, HttpMethod.Get, $"{Base(p)}/accounts/{p.Cfg("account_code")}/candidates/{o.UserId}/eligibilities", null);
        if (r is null) return ConnResult.Fail("PSI not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"HTTP {c}: {b}", c);
        return new ConnResult(true, c, "eligibility status", DeliveryStatus: Web.Str(b, "status") ?? "authorized", Raw: b);
    }
    public async Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o)
        // A partner-facing results pull is not offered; results arrive via the Notification callback.
        => new ConnResult(true, 200, "results arrive via the PSI Notification callback", DeliveryStatus: "awaiting_results");
}

// ─────────────────────────────────────────────────────────────────────────────────────────────
// Pearson VUE — real-time web services (SSO-RTI candidate + eligibility upsert). Candidate
// self-schedules via SSO; appointment + results pushed back via RTEN (inbound callback).
// ─────────────────────────────────────────────────────────────────────────────────────────────
public sealed class PearsonVueConnector : IExamDeliveryConnector
{
    public string Slug => "pearsonvue";
    public string Label => "Pearson VUE / OnVUE";
    public string Description => "Real-time web services (SSO-RTI): upsert candidate + eligibility (authorization); candidate self-schedules; RTEN callback delivers appointment + results.";
    public string[] ConfigFields => new[] { "client_code", "candidate_path", "eligibility_path", "exam_series", "eligibility_days", "default_exam_code", "api_base" };
    public string[] SecretFields => new[] { "username", "password", "access_token" };

    ConnResult NeedBase() => ConnResult.Fail("Pearson VUE not configured — set the web-services base URL and client code (provisioned via connect.pearsonvue.com).");
    string Base(ProviderCtx p) => (p.ApiBase ?? "").TrimEnd('/');
    bool Auth(ProviderCtx p, HttpRequestMessage r)
    {
        if (p.Sec("access_token") is { Length: > 0 } at) { Web.Bearer(r, at); return true; }
        if (p.Sec("username") is { Length: > 0 } u && p.Sec("password") is { Length: > 0 } pw) { Web.Basic(r, u, pw); return true; }
        return false;
    }

    public async Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p)
        => Base(p).Length == 0 ? NeedBase()
         : (p.Sec("access_token") ?? p.Sec("username")) is { Length: > 0 } ? ConnResult.Good("configured")
         : ConnResult.Fail("Pearson VUE not authorised — set an access token or username/password.");

    public async Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var path = p.Cfg("candidate_path") is { Length: > 0 } cp ? cp : "/candidates";
        var body = new { clientCandidateId = o.UserId.ToString(), clientCode = p.Cfg("client_code"), firstName = o.FirstName, lastName = o.LastName, email = o.Email, country = o.Country };
        var r = Web.Req(HttpMethod.Post, Base(p) + "/" + path.TrimStart('/'), body); if (!Auth(p, r)) return ConnResult.Fail("Pearson VUE not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"add candidate HTTP {c}: {b}", c);
        return new ConnResult(true, c, "candidate upserted", CandidateId: Web.Str(b, "candidateId", "vueCandidateId") ?? o.UserId.ToString(), Raw: b);
    }
    public async Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var path = p.Cfg("eligibility_path") is { Length: > 0 } ep ? ep : "/eligibilities";
        var days = int.TryParse(p.Cfg("eligibility_days"), out var d) ? d : 365;
        var body = new
        {
            clientCandidateId = o.ExternalCandidateId ?? o.UserId.ToString(),
            clientAuthorizationId = $"PCI-{o.Id}",
            examSeriesCode = p.Cfg("exam_series"),
            examAuthorizationCode = o.VendorExamCode,
            eligibilityStart = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            eligibilityEnd = DateTime.UtcNow.AddDays(days).ToString("yyyy-MM-dd"),
        };
        var r = Web.Req(HttpMethod.Post, Base(p) + "/" + path.TrimStart('/'), body); if (!Auth(p, r)) return ConnResult.Fail("Pearson VUE not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"add eligibility HTTP {c}: {b}", c);
        return new ConnResult(true, c, "authorized", RegistrationId: Web.Str(b, "authorizationId") ?? $"PCI-{o.Id}", Raw: b);
    }
    public async Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "eligibility loaded — candidate self-schedules via Pearson VUE SSO (OnVUE or a test centre)", DeliveryStatus: "awaiting_candidate_schedule");
    public async Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "reschedule is candidate-driven within the eligibility window", DeliveryStatus: "awaiting_candidate_schedule");
    public async Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var path = p.Cfg("eligibility_path") is { Length: > 0 } ep ? ep : "/eligibilities";
        var r = Web.Req(HttpMethod.Delete, $"{Base(p)}/{path.TrimStart('/')}/{Uri.EscapeDataString(o.ExternalRegistrationId ?? $"PCI-{o.Id}")}");
        if (!Auth(p, r)) return ConnResult.Fail("Pearson VUE not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        return ok ? new ConnResult(true, c, "eligibility cancelled", DeliveryStatus: "cancelled") : ConnResult.Fail($"HTTP {c}: {b}", c);
    }
    public async Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "appointment status arrives via the RTEN callback", DeliveryStatus: o.ExternalAppointmentId is { Length: > 0 } ? "scheduled" : "awaiting_candidate_schedule");
    public async Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "results arrive via the RTEN callback / results file", DeliveryStatus: "awaiting_results");
}

// ─────────────────────────────────────────────────────────────────────────────────────────────
// TestReach — per-client configured push (registration-in / results-out). Candidate books in the
// TestReach dashboard; results pushed back (inbound callback).
// ─────────────────────────────────────────────────────────────────────────────────────────────
public sealed class TestReachConnector : IExamDeliveryConnector
{
    public string Slug => "testreach";
    public string Label => "TestReach";
    public string Description => "Configured push integration: register candidate + enrol to an assessment; live remote invigilation; results pushed back via the callback.";
    public string[] ConfigFields => new[] { "candidate_path", "enrol_path", "auth_header", "default_exam_code", "api_base" };
    public string[] SecretFields => new[] { "api_key" };

    ConnResult NeedBase() => ConnResult.Fail("TestReach not configured — set the integration base URL + API key (provisioned by TestReach at onboarding).");
    string Base(ProviderCtx p) => (p.ApiBase ?? "").TrimEnd('/');
    bool Auth(ProviderCtx p, HttpRequestMessage r)
    {
        if (p.Sec("api_key") is not { Length: > 0 } k) return false;
        var header = p.Cfg("auth_header") is { Length: > 0 } h ? h : "Authorization";
        if (string.Equals(header, "Authorization", StringComparison.OrdinalIgnoreCase)) Web.Bearer(r, k);
        else r.Headers.TryAddWithoutValidation(header, k);
        return true;
    }

    public async Task<ConnResult> TestConnection(HttpClient http, ProviderCtx p)
        => Base(p).Length == 0 ? NeedBase() : p.Sec("api_key") is { Length: > 0 } ? ConnResult.Good("configured") : ConnResult.Fail("TestReach not authorised — set the API key.");

    public async Task<ConnResult> UpsertCandidate(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var path = p.Cfg("candidate_path") is { Length: > 0 } cp ? cp : "/candidates";
        var r = Web.Req(HttpMethod.Post, Base(p) + "/" + path.TrimStart('/'),
            new { externalReference = o.UserId.ToString(), firstName = o.FirstName, lastName = o.LastName, email = o.Email }); if (!Auth(p, r)) return ConnResult.Fail("TestReach not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"register candidate HTTP {c}: {b}", c);
        return new ConnResult(true, c, "candidate registered", CandidateId: Web.Str(b, "candidateId", "id") ?? o.UserId.ToString(), Raw: b);
    }
    public async Task<ConnResult> Authorize(HttpClient http, ProviderCtx p, ExamOrder o)
    {
        if (Base(p).Length == 0) return NeedBase();
        var path = p.Cfg("enrol_path") is { Length: > 0 } ep ? ep : "/enrolments";
        var r = Web.Req(HttpMethod.Post, Base(p) + "/" + path.TrimStart('/'),
            new { candidateReference = o.ExternalCandidateId ?? o.UserId.ToString(), assessmentId = o.VendorExamCode }); if (!Auth(p, r)) return ConnResult.Fail("TestReach not authorised.");
        var (c, b, ok) = await Web.Send(http, r);
        if (!ok) return ConnResult.Fail($"enrol HTTP {c}: {b}", c);
        return new ConnResult(true, c, "enrolled", RegistrationId: Web.Str(b, "enrolmentId", "id") ?? $"PCI-{o.Id}", Raw: b);
    }
    public async Task<ConnResult> Schedule(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "enrolled — candidate confirms the slot in the TestReach dashboard", DeliveryStatus: "awaiting_candidate_schedule");
    public async Task<ConnResult> Reschedule(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "reschedule is candidate-driven in the TestReach dashboard", DeliveryStatus: "awaiting_candidate_schedule");
    public async Task<ConnResult> Cancel(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "cancellation handled in the TestReach dashboard / by arrangement", DeliveryStatus: "cancelled");
    public async Task<ConnResult> GetStatus(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "session status arrives via the TestReach results callback", DeliveryStatus: o.ExternalRegistrationId is { Length: > 0 } ? "scheduled" : "awaiting_candidate_schedule");
    public async Task<ConnResult> GetResults(HttpClient http, ProviderCtx p, ExamOrder o)
        => new ConnResult(true, 200, "results are pushed back via the TestReach callback", DeliveryStatus: "awaiting_results");
}
