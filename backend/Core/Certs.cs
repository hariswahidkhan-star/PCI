using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Certification resolution + per-certification exam configuration. The platform supports any
/// number of credentials; PCP-AI is certification id 1. A certification row's columns override
/// the global exam_* settings (NULL column → global setting → hard default), so existing
/// single-certification deployments keep behaving identically.
/// </summary>
public static class Certs
{
    public const long DefaultId = 1;

    public static Dictionary<string, object?>? ById(Db db, object? id) =>
        db.QueryOne("SELECT * FROM certifications WHERE id=?", id ?? DefaultId);

    public static Dictionary<string, object?>? ByCode(Db db, string? code) =>
        string.IsNullOrWhiteSpace(code) ? null
        : db.QueryOne("SELECT * FROM certifications WHERE upper(code)=?", code.Trim().ToUpperInvariant());

    /// <summary>Resolve a certification id from a request value that may be an id or a code;
    /// falls back to the founding certification so single-cert flows keep working unchanged.</summary>
    public static long Resolve(Db db, string? idOrCode)
    {
        if (string.IsNullOrWhiteSpace(idOrCode)) return DefaultId;
        if (long.TryParse(idOrCode, out var n) && ById(db, n) is not null) return n;
        var byCode = ByCode(db, idOrCode);
        return byCode is not null ? H.L(byCode["id"]) : DefaultId;
    }

    /// <summary>Per-certification exam configuration: certification column when set, else the
    /// global exam_* setting, else the built-in default. Timing windows (open-before/grace) and
    /// proctoring requirements stay global — they are operational policy, not credential design.</summary>
    public static H.ExamCfg Cfg(Db db, long certId)
    {
        var g = H.Cfg(db);
        var c = ById(db, certId);
        if (c is null) return g;
        var dur = c["duration_minutes"] is null ? g.Duration : H.D(c["duration_minutes"]);
        var pass = c["pass_mark_pct"] is null ? g.Pass : H.D(c["pass_mark_pct"]);
        return g with { Duration = dur, Pass = pass };
    }

    public static string Prefix(Dictionary<string, object?> cert) =>
        H.Str(cert["credential_prefix"]) is { Length: > 0 } p ? p : (H.Str(cert["code"]) ?? "PCP-AI");

    public static int ExpiryYears(Dictionary<string, object?> cert) =>
        cert["expiry_years"] is null ? 3 : (int)H.L(cert["expiry_years"]);
}
