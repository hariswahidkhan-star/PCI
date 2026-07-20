using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The platform's machine-translation provider seam — the same "configurable, never hardcoded, degrades
/// safely" shape as <see cref="Mailer"/>. A provider and its key/model live in site_settings (owner-set
/// in Admin → Translations), so the institute controls translation from the backend. With no provider
/// configured, translation is manual-only: admins type or paste translations, which are stored and served
/// exactly the same way. Nothing here ever throws — a provider failure leaves the region untranslated
/// (it falls back to English) rather than breaking the request.
///
///   translate_provider : "" (none) | "anthropic" | "openai" | "custom" (OpenAI-compatible endpoint)
///   translate_api_key  : provider API key
///   translate_model    : model id (sensible per-provider default if blank)
///   translate_endpoint : override base URL (for "custom"/self-hosted OpenAI-compatible gateways)
/// </summary>
public static class Translator
{
    static readonly HttpClient _http = Egress.CreateClient(TimeSpan.FromSeconds(60));   // SSRF-guarded custom endpoint

    public static readonly Dictionary<string, string> LangNames = new(StringComparer.Ordinal)
    {
        ["ko"] = "Korean", ["ar"] = "Arabic", ["es"] = "Spanish",
        ["fr"] = "French", ["zh"] = "Simplified Chinese", ["ru"] = "Russian",
    };

    public sealed record Config(string Provider, string ApiKey, string Model, string Endpoint);

    public static Config Load(Db db)
    {
        string S(string k) => db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", k) ?? "";
        return new Config(S("translate_provider").Trim().ToLowerInvariant(), S("translate_api_key").Trim(),
                          S("translate_model").Trim(), S("translate_endpoint").Trim());
    }

    /// <summary>True when a usable provider + key are configured (so auto-translate is available).</summary>
    public static bool IsConfigured(Db db)
    {
        var c = Load(db);
        return c.Provider is "anthropic" or "openai" or "custom" && c.ApiKey.Length > 0;
    }

    /// <summary>Translate a batch of English strings into one target language. Returns an array the same
    /// length as <paramref name="texts"/>; any element may be null if the provider failed for it (kept as
    /// English by the caller). Never throws. HTML strings must keep their tags — the prompt enforces it.</summary>
    public static string?[] Translate(Db db, IReadOnlyList<string> texts, string targetLang)
    {
        var result = new string?[texts.Count];
        if (texts.Count == 0) return result;
        var c = Load(db);
        if (!(c.Provider is "anthropic" or "openai" or "custom") || c.ApiKey.Length == 0) return result;
        if (!LangNames.TryGetValue(targetLang, out var langName)) return result;

        var prompt = BuildPrompt(texts, langName);
        try
        {
            var raw = c.Provider == "anthropic" ? CallAnthropic(c, prompt) : CallOpenAi(c, prompt);
            if (raw is null) return result;
            var arr = ParseArray(raw);
            if (arr is null) return result;
            for (int i = 0; i < texts.Count && i < arr.Count; i++)
                result[i] = string.IsNullOrWhiteSpace(arr[i]) ? null : arr[i];
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[translate] {c.Provider} → {targetLang} failed: {ex.Message}");
        }
        return result;
    }

    static string BuildPrompt(IReadOnlyList<string> texts, string langName)
    {
        var input = JsonSerializer.Serialize(texts);
        return
$@"You are a professional translator for the Project Controls Institute (PCI), a global certification body. Translate the following JSON array of English website strings into {langName}.

Return ONLY a JSON array of strings, the SAME length and order as the input — nothing else, no markdown, no commentary.

Rules:
- Natural, fluent, professional human-quality translation.
- Do NOT translate brand/proper nouns: PCI, PCL-AI, PFL-AI, PDL-AI, Project Controls Institute, Project Controls Institute Global, Inc., and standard/product codes (ISO/IEC, PMP, CCP, PSP, PRINCE2, EVM). Keep URLs, emails and numbers verbatim.
- Keep symbols such as ·, —, →, %, © and any {{placeholder}} tokens exactly as they appear.
- If a string contains HTML tags, translate ONLY the human-visible text and keep every tag, attribute, href/src and entity EXACTLY as in the source.

Input:
{input}";
    }

    static string? CallAnthropic(Config c, string prompt)
    {
        var url = c.Endpoint.Length > 0 ? c.Endpoint.TrimEnd('/') + "/v1/messages" : "https://api.anthropic.com/v1/messages";
        var model = c.Model.Length > 0 ? c.Model : "claude-sonnet-5";
        var body = JsonSerializer.Serialize(new
        {
            model,
            max_tokens = 8192,
            messages = new[] { new { role = "user", content = prompt } },
        });
        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Add("x-api-key", c.ApiKey);
        req.Headers.Add("anthropic-version", "2023-06-01");
        req.Content = new StringContent(body, Encoding.UTF8, "application/json");
        using var resp = _http.Send(req);
        var txt = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
        if (!resp.IsSuccessStatusCode) { Console.Error.WriteLine($"[translate] anthropic {(int)resp.StatusCode}: {Trunc(txt)}"); return null; }
        using var doc = JsonDocument.Parse(txt);
        return doc.RootElement.GetProperty("content")[0].GetProperty("text").GetString();
    }

    static string? CallOpenAi(Config c, string prompt)
    {
        var url = c.Endpoint.Length > 0 ? c.Endpoint.TrimEnd('/') + "/chat/completions" : "https://api.openai.com/v1/chat/completions";
        var model = c.Model.Length > 0 ? c.Model : "gpt-4o-mini";
        var body = JsonSerializer.Serialize(new
        {
            model,
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.2,
        });
        using var req = new HttpRequestMessage(HttpMethod.Post, url);
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", c.ApiKey);
        req.Content = new StringContent(body, Encoding.UTF8, "application/json");
        using var resp = _http.Send(req);
        var txt = new StreamReader(resp.Content.ReadAsStream()).ReadToEnd();
        if (!resp.IsSuccessStatusCode) { Console.Error.WriteLine($"[translate] openai {(int)resp.StatusCode}: {Trunc(txt)}"); return null; }
        using var doc = JsonDocument.Parse(txt);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }

    /// <summary>Parse the model's reply into a string array, tolerating markdown fences / surrounding prose
    /// by extracting the first top-level JSON array.</summary>
    static List<string>? ParseArray(string raw)
    {
        var s = raw.Trim();
        int lb = s.IndexOf('['), rb = s.LastIndexOf(']');
        if (lb < 0 || rb <= lb) return null;
        s = s.Substring(lb, rb - lb + 1);
        try
        {
            using var doc = JsonDocument.Parse(s);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return null;
            var list = new List<string>();
            foreach (var el in doc.RootElement.EnumerateArray())
                list.Add(el.ValueKind == JsonValueKind.String ? el.GetString() ?? "" : el.ToString());
            return list;
        }
        catch { return null; }
    }

    static string Trunc(string s) => s.Length <= 300 ? s : s[..300];
}
