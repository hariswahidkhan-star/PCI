namespace PCI.Backend.Core;

/// <summary>
/// Minor-unit money arithmetic for the partner finance ledger.
///
/// Every amount inside the ledger is a whole number of minor units (cents) held in a long. There is no
/// floating-point arithmetic anywhere in the commission path: floats enter only at the boundary, where a
/// legacy REAL/DECIMAL column (payments.final_amount) or a Stripe amount is converted ONCE by
/// <see cref="ToMinor"/>, and leave only when a response is rendered by <see cref="ToDecimal"/>.
///
/// Why minor units rather than DECIMAL columns: DECIMAL is exact on MySQL but only NUMERIC *affinity* on
/// SQLite (values are stored as REAL and not rounded to the scale), and Microsoft.Data.Sqlite binds a C#
/// decimal as TEXT — which would silently break SUM() and comparisons on the SQLite side of the CI matrix.
/// Integers are exact and identically summable on both providers, and match how Stripe reports amounts.
/// </summary>
public static class Money
{
    /// <summary>Convert a major-unit amount (e.g. 149.5) to minor units (14950), half away from zero.</summary>
    public static long ToMinor(double major) => (long)Math.Round(major * 100.0, MidpointRounding.AwayFromZero);

    /// <summary>Convert a nullable DB value (REAL/DECIMAL/NULL) to minor units. NULL → 0.</summary>
    public static long ToMinor(object? dbValue) => dbValue is null ? 0L : ToMinor(Convert.ToDouble(dbValue));

    /// <summary>Render minor units as a major-unit decimal for API/display. Exact — no float involved.</summary>
    public static decimal ToDecimal(long minor) => minor / 100m;

    /// <summary>Format minor units for a statement/CSV line, e.g. 14950 → "149.50".</summary>
    public static string Format(long minor) =>
        ToDecimal(minor).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Apply a basis-point rate to a minor-unit amount using exact integer arithmetic, rounding half away
    /// from zero. 10_000 bp = 100%. Sign is preserved, so a reversal (negative base) yields negative
    /// commission with the same magnitude as the original — reversals cancel exactly.
    /// </summary>
    public static long ApplyRate(long minor, long basisPoints)
    {
        if (minor == 0 || basisPoints == 0) return 0L;
        var negative = (minor < 0) ^ (basisPoints < 0);
        var a = Math.Abs(minor);
        var b = Math.Abs(basisPoints);
        var scaled = a * b;                       // cents × bp; far inside long range for real money
        var quotient = (scaled + 5_000L) / 10_000L;  // + half of 10_000 → round half up on the magnitude
        return negative ? -quotient : quotient;
    }

    /// <summary>
    /// Proportion of an amount, used for partial refunds: part/whole of the original, exact integer maths,
    /// half away from zero. Returns 0 when the whole is 0 (nothing to apportion).
    /// </summary>
    public static long Apportion(long minor, long part, long whole)
    {
        if (minor == 0 || part == 0 || whole == 0) return 0L;
        var negative = (minor < 0) ^ (part < 0) ^ (whole < 0);
        var a = Math.Abs(minor);
        var p = Math.Abs(part);
        var w = Math.Abs(whole);
        if (p >= w) return negative ? -a : a;     // a full (or over-) refund apportions the whole amount
        var quotient = (a * p + w / 2) / w;
        return negative ? -quotient : quotient;
    }

    /// <summary>Percentage (e.g. 7.5) expressed as basis points (750), rounded half away from zero.</summary>
    public static long PercentToBasisPoints(double percent) =>
        (long)Math.Round(percent * 100.0, MidpointRounding.AwayFromZero);

    /// <summary>Basis points back to a display percentage (750 → 7.5m).</summary>
    public static decimal BasisPointsToPercent(long basisPoints) => basisPoints / 100m;
}
