namespace PCI.SecureExam.Core;

/// <summary>Values carried by a pciexam://start?code=..&api=..&token=.. launch link.</summary>
public sealed record LaunchParameters(string? Code, string? ApiBaseUrl, string? Token)
{
    public bool IsValid => !string.IsNullOrWhiteSpace(Code);

    /// <summary>
    /// Parse a pciexam:// launch URI. Tolerant of missing parts and of the value being passed either as a
    /// full URI (from the OS scheme handler) or already-decoded. Never throws — returns an empty result on
    /// anything unparseable, so the host can show a friendly "waiting for launch" screen.
    /// </summary>
    public static LaunchParameters Parse(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return new LaunchParameters(null, null, null);
        try
        {
            var s = raw.Trim().Trim('"');
            // Normalise custom scheme so System.Uri can parse the query.
            var normalised = s.Replace("pciexam://start", "https://pciexam.local/start", StringComparison.OrdinalIgnoreCase)
                              .Replace("pciexam://", "https://pciexam.local/", StringComparison.OrdinalIgnoreCase);
            if (!Uri.TryCreate(normalised, UriKind.Absolute, out var uri))
                return new LaunchParameters(null, null, null);
            var q = ParseQuery(uri.Query);
            string? Get(params string[] keys) => keys.Select(k => q.TryGetValue(k, out var v) ? v : null).FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
            var code = Get("code", "c");
            var api = Get("api", "apiBaseUrl", "baseUrl");
            var token = Get("token", "t", "session");
            if (!string.IsNullOrWhiteSpace(api)) api = Uri.UnescapeDataString(api!);
            return new LaunchParameters(code, api, token);
        }
        catch { return new LaunchParameters(null, null, null); }
    }

    /// <summary>Dependency-free query-string parser (avoids System.Web).</summary>
    private static Dictionary<string, string> ParseQuery(string query)
    {
        var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var i = pair.IndexOf('=');
            if (i < 0) { d[Uri.UnescapeDataString(pair)] = ""; continue; }
            var k = Uri.UnescapeDataString(pair[..i]);
            var v = Uri.UnescapeDataString(pair[(i + 1)..]);
            d[k] = v;
        }
        return d;
    }
}
