using System.Globalization;

namespace PCI.Backend.Core;

/// <summary>
/// RFC-4180 CSV field encoding that is also safe against spreadsheet formula injection
/// (CWE-1236 / "CSV injection"). A NON-NUMERIC value whose first character is a formula trigger
/// (<c>= + - @</c>) or a control character (TAB / CR / LF) is prefixed with a single quote so Excel,
/// Google Sheets and LibreOffice render it as literal text and never evaluate it as a formula.
/// Genuine numbers (including negatives like <c>-5</c> and signed decimals like <c>+3.5</c>) are left
/// intact, so numeric columns are not corrupted. The result is then quoted when it contains a comma,
/// double-quote, or newline.
///
/// SEC-2: every CSV export must route user-influenced fields through this helper. Attacker-controlled
/// analytics/marketing fields (utm_source, referrer, landing, …) and partner-portal fields flow into
/// exports that admins open in a spreadsheet; without neutralisation a crafted value such as
/// <c>=HYPERLINK("http://evil","click")</c> or <c>=cmd|'/c calc'!A1</c> would execute on open.
/// </summary>
public static class Csv
{
    static readonly char[] Triggers = { '=', '+', '-', '@', '\t', '\r', '\n' };
    static readonly char[] MustQuote = { ',', '"', '\n', '\r' };

    public static string Field(object? value)
    {
        var s = H.Str(value) ?? value?.ToString() ?? "";
        // Formula-injection guard: neutralise a leading formula trigger, but only on text that is not a
        // genuine number (so -5 / +3.5 stay numeric while =cmd / @foo / -cmd|... become inert text).
        if (s.Length > 0 && System.Array.IndexOf(Triggers, s[0]) >= 0
            && !double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out _))
            s = "'" + s;
        return s.IndexOfAny(MustQuote) >= 0 ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }
}
