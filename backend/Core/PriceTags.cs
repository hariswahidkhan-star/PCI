using System.Text.RegularExpressions;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;

namespace PCI.Backend.Core;

/// <summary>
/// Live price tokens for the public pages. Any element carrying data-price="key" has its text
/// replaced, server-side, with the CURRENT value from the pricing engine — the same engine the
/// checkout charges with — so a price mentioned in page copy can never drift from what a customer
/// actually pays. Change a pricing rule (or the founding certification's exam fee) in the admin
/// console and every mention across the site updates instantly.
///
/// Keys: {membership|exam|bundle|renewal|recert|retake}.{std|final|discpct}
///   std     — standard price            e.g. "USD 500"
///   final   — price after the standing discount  e.g. "USD 350"
///   discpct — the standing discount     e.g. "30%"
/// Exam and bundle keys price the FOUNDING certification (the credential the marketing copy
/// describes); each certification's own price is shown on its catalogue card.
/// </summary>
public static class PriceTags
{
    static int _version = 1;
    /// <summary>Called when pricing rules, certifications or settings change.</summary>
    public static void Bump() => Interlocked.Increment(ref _version);

    static int _cacheVer = -1;
    static Dictionary<string, string> _values = new();
    static readonly object _lock = new();

    static readonly Regex RxTag = new(
        @"(<([a-zA-Z0-9]+)([^>]*?)\sdata-price=""([^""]+)""([^>]*)>)(.*?)(</\2>)",
        RegexOptions.Singleline | RegexOptions.Compiled);

    public static string Inject(Db db, string html)
    {
        if (string.IsNullOrEmpty(html) || !html.Contains("data-price", StringComparison.Ordinal)) return html;
        var vals = Values(db);
        return RxTag.Replace(html, m =>
            vals.TryGetValue(m.Groups[4].Value, out var v)
                ? m.Groups[1].Value + v + m.Groups[7].Value
                : m.Value);                                  // unknown key → leave the static text
    }

    static Dictionary<string, string> Values(Db db)
    {
        var v = Volatile.Read(ref _version);
        lock (_lock) { if (_cacheVer == v) return _values; }
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var founding = Certs.ById(db, Certs.DefaultId);
            foreach (var product in new[] { "membership", "exam", "bundle", "renewal", "recert", "retake" })
            {
                var pr = Public.Pricing(db, product, null, founding);
                if (pr.items.Count == 0) continue;
                map[product + ".std"] = Money(pr.standard);
                map[product + ".final"] = Money(pr.final);
                var pct = pr.standard > 0 ? (pr.defaultDiscount / pr.standard) * 100.0 : 0;
                map[product + ".discpct"] = Pct(pct);
            }
        }
        catch (Exception e) { Console.Error.WriteLine($"[price-tags] compute skipped: {e.Message}"); }
        lock (_lock) { _cacheVer = v; _values = map; }
        return map;
    }

    static string Money(double n) => "USD " + (n == Math.Floor(n) ? ((long)n).ToString() : n.ToString("0.00"));
    static string Pct(double n) => (Math.Abs(n - Math.Round(n)) < 0.05 ? ((long)Math.Round(n)).ToString() : n.ToString("0.#")) + "%";
}
