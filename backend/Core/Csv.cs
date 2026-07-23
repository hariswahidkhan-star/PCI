using System.Text.RegularExpressions;

namespace PCI.Backend.Core;

/// <summary>CSV cell encoding for administrator downloads. Besides RFC-4180 quoting, string cells
/// that spreadsheet software could execute as formulas are forced to text. Numeric CLR values stay
/// numeric, so legitimate negative amounts are not changed.</summary>
public static class Csv
{
    static readonly Regex Formula = new(@"^[\t\r\n ]*[=+\-@]", RegexOptions.Compiled);

    public static string Cell(object? value)
    {
        var text = H.Str(value) ?? value?.ToString() ?? "";
        if (value is string && Formula.IsMatch(text)) text = "'" + text;
        return Regex.IsMatch(text, "[\",\r\n]") ? "\"" + text.Replace("\"", "\"\"") + "\"" : text;
    }
}
