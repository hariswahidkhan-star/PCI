using System.Net.Http;           // WPF's implicit-usings profile removes System.Net.Http
using System.Net.Http.Headers;
using System.Net.Http.Json;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Api;

/// <summary>
/// Talks to the existing PCI backend. Reuses the power-off-safe exam endpoints already implemented:
/// /api/me/exam/start (resumes with saved answers + remaining time), /api/me/exam/heartbeat
/// (server-anchored clock + answer persistence), /api/me/exam/submit (server-scored). Adds evidence
/// upload + identity/authorize endpoints served by PCI.SecureExam.Server.
/// </summary>
public sealed class PciApiClient
{
    private readonly HttpClient _http;
    private static readonly System.Text.Json.JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };
    public PciApiClient(string baseUrl, string? sessionToken = null)
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"), Timeout = TimeSpan.FromSeconds(30) };
        if (!string.IsNullOrWhiteSpace(sessionToken))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);
    }

    /// <summary>Adopt the short-lived exam session token returned by /api/exam/authorize so that the
    /// session-protected endpoints (heartbeat/submit/evidence/identity) authenticate correctly. The
    /// desktop client has no prior browser session; the launch code is exchanged for this token.</summary>
    public void UseSessionToken(string sessionToken)
        => _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionToken);

    public async Task<ExamAuthorization?> AuthorizeAsync(string launchCode, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("api/exam/authorize", new { code = launchCode }, _json, ct);
        if (!res.IsSuccessStatusCode) return null;
        var auth = await res.Content.ReadFromJsonAsync<ExamAuthorization>(_json, ct);
        if (auth?.SessionToken is { Length: > 0 } tok) UseSessionToken(tok);  // switch to the exam session
        return auth;
    }

    public async Task<HeartbeatResponse?> HeartbeatAsync(HeartbeatRequest req, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("api/me/exam/heartbeat", req, _json, ct);
        return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<HeartbeatResponse>(_json, ct) : null;
    }

    public async Task<SubmitResult?> SubmitAsync(SubmitRequest req, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("api/me/exam/submit", req, _json, ct);
        return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<SubmitResult>(_json, ct) : null;
    }

    public async Task UploadEvidenceAsync(string attemptToken, string kind, byte[] jpeg, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(attemptToken), "attemptToken" },
            { new StringContent(kind), "kind" },
        };
        var img = new ByteArrayContent(jpeg);
        img.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(img, "file", $"{kind}_{DateTime.UtcNow:HHmmss}.jpg");
        await _http.PostAsync("api/exam/evidence", content, ct);
    }

    /// <summary>Post an evidence frame as base64 JSON (matches the Node backend /api/exam/evidence).</summary>
    public async Task UploadEvidenceBase64Async(string attemptToken, string kind, byte[] jpeg, CancellationToken ct = default)
    {
        var dataUri = "data:image/jpeg;base64," + Convert.ToBase64String(jpeg);
        await _http.PostAsJsonAsync("api/exam/evidence", new { attemptToken, kind, data_uri = dataUri }, _json, ct);
    }

    public async Task PostIdentityAsync(string attemptToken, IdentityCheck check, CancellationToken ct = default)
        => await _http.PostAsJsonAsync("api/exam/identity", new { attemptToken, check }, _json, ct);
}
