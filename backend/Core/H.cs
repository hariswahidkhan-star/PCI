using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>Shared helpers used across ported endpoints. Coercion helpers hide SQLite's
/// object typing (INTEGER→long, REAL→double, TEXT→string) so ported code reads like the Node original.</summary>
public static class H
{
    // ---- value coercion from a row dictionary ----
    public static long L(object? v) => v is null ? 0 : Convert.ToInt64(v);
    public static long? Ln(object? v) => v is null ? null : Convert.ToInt64(v);
    public static double D(object? v) => v is null ? 0 : Convert.ToDouble(v);
    public static string? Str(object? v) => v?.ToString();
    public static bool B(object? v) => v is not null && (Convert.ToString(v) == "1" || string.Equals(Convert.ToString(v), "true", StringComparison.OrdinalIgnoreCase) || (v is long l && l != 0));

    // ---- JSON request body ----
    public static async Task<JsonElement> BodyEl(HttpRequest r)
    {
        try
        {
            using var doc = await JsonDocument.ParseAsync(r.Body);
            return doc.RootElement.Clone();
        }
        catch { return default; }
    }
    public static Dictionary<string, JsonElement> ToMap(JsonElement e)
    {
        var d = new Dictionary<string, JsonElement>(StringComparer.Ordinal);
        if (e.ValueKind == JsonValueKind.Object)
            foreach (var p in e.EnumerateObject()) d[p.Name] = p.Value;
        return d;
    }
    public static async Task<Dictionary<string, JsonElement>> Body(HttpRequest r) => ToMap(await BodyEl(r));

    public static string? GetS(Dictionary<string, JsonElement> b, params string[] keys)
    {
        foreach (var k in keys)
            if (b.TryGetValue(k, out var v) && v.ValueKind != JsonValueKind.Null)
                return v.ValueKind == JsonValueKind.String ? v.GetString() : v.GetRawText().Trim('"');
        return null;
    }
    public static double? GetNum(Dictionary<string, JsonElement> b, params string[] keys)
    {
        foreach (var k in keys)
            if (b.TryGetValue(k, out var v))
            {
                if (v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out var d)) return d;
                if (v.ValueKind == JsonValueKind.String && double.TryParse(v.GetString(), out var d2)) return d2;
            }
        return null;
    }
    public static JsonElement? GetEl(Dictionary<string, JsonElement> b, params string[] keys)
    {
        foreach (var k in keys) if (b.TryGetValue(k, out var v)) return v;
        return null;
    }

    // ---- exam config (parity with examCfg) ----
    public const int EXAM_MIN = 90, PASS = 65, OPEN_BEFORE_MIN = 15, GRACE_MIN = 30, RESCHED_LOCK_H = 24, FREE_RESCHED_H = 72, MAX_RESCHED = 3;
    public record ExamCfg(double Duration, double Pass, double OpenBefore, double Grace, bool RequireIdentity, bool RequireRoomScan);
    public static ExamCfg Cfg(Db db) => new(
        Settings.Num(db, "exam_duration_minutes", EXAM_MIN),
        Settings.Num(db, "exam_pass_mark_pct", PASS),
        Settings.Num(db, "exam_open_before_minutes", OPEN_BEFORE_MIN),
        Settings.Num(db, "exam_grace_minutes", GRACE_MIN),
        Settings.Bool(db, "exam_require_identity", true),
        Settings.Bool(db, "exam_require_room_scan", true));

    // ---- attemptForToken: resolve a launch-code string OR a numeric attempt id (parity) ----
    public static Dictionary<string, object?>? AttemptForToken(Db db, object? token, long userId)
    {
        var s = token?.ToString() ?? "";
        if (System.Text.RegularExpressions.Regex.IsMatch(s, @"^\d+$"))
            return db.QueryOne("SELECT * FROM exam_attempts WHERE id=? AND user_id=?", s, userId);
        // launch code → its attempt (create-or-resume handled in authorize); resolve most recent in-progress
        var lc = db.QueryOne("SELECT * FROM exam_launch_codes WHERE code=? AND user_id=?", s, userId);
        if (lc is null) return null;
        return db.QueryOne("SELECT * FROM exam_attempts WHERE user_id=? AND (booking_id=? OR status='in_progress') ORDER BY id DESC", userId, lc["booking_id"]);
    }

    // ---- ISO timestamp parity with JS toISOString/Date math ----
    public static long JsMillis(string? sqliteDatetime)
    {
        // SQLite datetime('now') → "YYYY-MM-DD HH:MM:SS" (UTC). Parse as UTC.
        if (string.IsNullOrWhiteSpace(sqliteDatetime)) return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        if (DateTimeOffset.TryParse(sqliteDatetime, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var dto))
            return dto.ToUnixTimeMilliseconds();
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
    public static string IsoNow => DateTime.UtcNow.ToString("o");
    public static string IsoFromMillis(long ms) => DateTimeOffset.FromUnixTimeMilliseconds(ms).UtcDateTime.ToString("o");

    public static List<string> SplitOptions(object? opts) =>
        (opts?.ToString() ?? "").Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

    /// <summary>Build the ordered MCQ options for a sample_questions row. Prefers the four discrete
    /// columns (option_a..option_d); falls back to the legacy newline-separated `options` field so
    /// pre-existing questions still render. Blank trailing options are dropped.</summary>
    public static List<string> OptionsFor(Dictionary<string, object?> row)
    {
        var four = new[] { "option_a", "option_b", "option_c", "option_d" }
            .Select(k => row.TryGetValue(k, out var v) ? Str(v) : null)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .ToList();
        if (four.Count > 0) return four;
        return SplitOptions(row.TryGetValue("options", out var o) ? o : null);
    }
}
