using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Student-facing error references. Every important failure — server exception or client-reported —
/// becomes an error_reports row with a quotable reference (PCI-YYYY-NNNNNN). Support searches the
/// reference and sees exactly what the student saw plus a technical summary. Never stores passwords,
/// tokens, or card data: the capture surface only accepts page/category/messages/user-agent.
/// </summary>
public static class ErrorRefs
{
    public static string Capture(Db db, long? userId, string? page, string category, string userMessage, string techSummary, string? userAgent, string? appVersion = null, string? relatedType = null, long? relatedId = null)
    {
        var reference = $"PCI-{DateTime.UtcNow:yyyy}-{Security.RandomHex(3).ToUpperInvariant()}";
        var (browser, os) = ParseAgent(userAgent);
        try
        {
            db.Execute(@"INSERT INTO error_reports(reference,user_id,page,category,user_message,tech_summary,browser,os,app_version,related_type,related_id)
                VALUES(?,?,?,?,?,?,?,?,?,?,?)",
                reference, userId, Clip(page, 300), Clip(category, 40) ?? "general", Clip(userMessage, 500), Clip(techSummary, 1000),
                browser, os, Clip(appVersion, 40), Clip(relatedType, 32), relatedId);
        }
        catch { /* error capture must never take the request down with it */ }
        return reference;
    }

    static string? Clip(string? s, int max) => string.IsNullOrWhiteSpace(s) ? null : (s.Length > max ? s[..max] : s);

    static (string? browser, string? os) ParseAgent(string? ua)
    {
        if (string.IsNullOrWhiteSpace(ua)) return (null, null);
        string? browser = ua.Contains("Edg/") ? "Edge" : ua.Contains("OPR/") ? "Opera" : ua.Contains("Chrome/") ? "Chrome"
            : ua.Contains("Firefox/") ? "Firefox" : ua.Contains("Safari/") ? "Safari" : "Other";
        string? os = ua.Contains("Windows") ? "Windows" : ua.Contains("Mac OS") ? "macOS" : ua.Contains("Android") ? "Android"
            : ua.Contains("iPhone") || ua.Contains("iPad") ? "iOS" : ua.Contains("Linux") ? "Linux" : "Other";
        return (browser, os);
    }
}
