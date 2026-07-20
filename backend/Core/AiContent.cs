using System.Text;
using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// AI Content Studio provider client — ASSIST ONLY. It calls the official OpenAI and Anthropic APIs to help
/// draft, edit, translate, summarise, generate SEO/social variants and check citations. It NEVER publishes:
/// every generation returns text for a human to review, and the caller persists an audit row. API keys are
/// read from environment variables (OPENAI_API_KEY / ANTHROPIC_API_KEY) — never from the database, source or
/// the React app. If a key is absent the provider is simply "not configured" and the endpoint says so.
/// </summary>
public static class AiContent
{
    // A single shared client to the public model APIs. These are first-party trusted vendor endpoints
    // (api.openai.com / api.anthropic.com), not admin-supplied URLs, so they are not egress-restricted.
    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(90) };

    public static bool OpenAiReady => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    public static bool AnthropicReady => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY"));

    public static bool Ready(string provider) => provider?.ToLowerInvariant() switch
    {
        "openai" => OpenAiReady,
        "anthropic" or "claude" => AnthropicReady,
        _ => false,
    };

    public record Result(bool Ok, string Text, string? Error, int TokensIn, int TokensOut);

    /// <summary>Run one assist generation. Returns the model's text plus token usage, or an error. Throws never.</summary>
    public static async Task<Result> Generate(string provider, string? model, string? system, string prompt, int maxTokens, double temperature)
    {
        try
        {
            return provider?.ToLowerInvariant() switch
            {
                "openai" => await OpenAi(model, system, prompt, maxTokens, temperature),
                "anthropic" or "claude" => await Anthropic(model, system, prompt, maxTokens, temperature),
                _ => new Result(false, "", "unknown_provider", 0, 0),
            };
        }
        catch (Exception e) { return new Result(false, "", e.Message, 0, 0); }
    }

    static async Task<Result> OpenAi(string? model, string? system, string prompt, int maxTokens, double temperature)
    {
        var key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(key)) return new Result(false, "", "openai_not_configured", 0, 0);
        var msgs = new List<object>();
        if (!string.IsNullOrWhiteSpace(system)) msgs.Add(new { role = "system", content = system });
        msgs.Add(new { role = "user", content = prompt });
        var body = JsonSerializer.Serialize(new { model = string.IsNullOrWhiteSpace(model) ? "gpt-4o-mini" : model, messages = msgs, max_tokens = maxTokens, temperature });
        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.TryAddWithoutValidation("Authorization", "Bearer " + key);
        req.Content = new StringContent(body, Encoding.UTF8, "application/json");
        using var resp = await Http.SendAsync(req);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) return new Result(false, "", $"openai_{(int)resp.StatusCode}: {Trim(txt)}", 0, 0);
        using var doc = JsonDocument.Parse(txt);
        var root = doc.RootElement;
        var content = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "";
        int tin = 0, tout = 0;
        if (root.TryGetProperty("usage", out var u))
        { if (u.TryGetProperty("prompt_tokens", out var a)) tin = a.GetInt32(); if (u.TryGetProperty("completion_tokens", out var b)) tout = b.GetInt32(); }
        return new Result(true, content.Trim(), null, tin, tout);
    }

    static async Task<Result> Anthropic(string? model, string? system, string prompt, int maxTokens, double temperature)
    {
        var key = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
        if (string.IsNullOrWhiteSpace(key)) return new Result(false, "", "anthropic_not_configured", 0, 0);
        var payload = new Dictionary<string, object?>
        {
            ["model"] = string.IsNullOrWhiteSpace(model) ? "claude-3-5-sonnet-latest" : model,
            ["max_tokens"] = maxTokens,
            ["temperature"] = temperature,
            ["messages"] = new[] { new { role = "user", content = prompt } },
        };
        if (!string.IsNullOrWhiteSpace(system)) payload["system"] = system;
        using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
        req.Headers.TryAddWithoutValidation("x-api-key", key);
        req.Headers.TryAddWithoutValidation("anthropic-version", "2023-06-01");
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await Http.SendAsync(req);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) return new Result(false, "", $"anthropic_{(int)resp.StatusCode}: {Trim(txt)}", 0, 0);
        using var doc = JsonDocument.Parse(txt);
        var root = doc.RootElement;
        var sb = new StringBuilder();
        if (root.TryGetProperty("content", out var blocks) && blocks.ValueKind == JsonValueKind.Array)
            foreach (var b in blocks.EnumerateArray())
                if (b.TryGetProperty("type", out var t) && t.GetString() == "text" && b.TryGetProperty("text", out var tx)) sb.Append(tx.GetString());
        int tin = 0, tout = 0;
        if (root.TryGetProperty("usage", out var u))
        { if (u.TryGetProperty("input_tokens", out var a)) tin = a.GetInt32(); if (u.TryGetProperty("output_tokens", out var b2)) tout = b2.GetInt32(); }
        return new Result(true, sb.ToString().Trim(), null, tin, tout);
    }

    static string Trim(string s) => s.Length > 300 ? s[..300] : s;
}
