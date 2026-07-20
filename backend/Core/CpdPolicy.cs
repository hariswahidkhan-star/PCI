using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Continuing Professional Development (CPD) recertification policy. A certification may require a number
/// of APPROVED CPD hours per recertification cycle (certifications.cpd_required_hours; 0 = no requirement).
/// The counting window is the credential's current cycle — the cert's expiry_years ending at the
/// credential's current expiry — so only CPD earned in this term counts toward the next recert.
///
/// Only admin-approved entries (cpd_entries.status='approved') count; 'recorded' (pending review) and
/// 'rejected' entries never do. This is the single source of truth for both the recert checkout gate and
/// the status shown to the student, so the two can never disagree.
/// </summary>
public static class CpdPolicy
{
    /// <summary>Required approved CPD hours per cycle for a certification (0 = no requirement).</summary>
    public static double RequiredHours(Db db, long certId)
    {
        var cert = Certs.ById(db, certId);
        return cert is not null && cert.TryGetValue("cpd_required_hours", out var v) && v is not null ? H.D(v) : 0;
    }

    /// <summary>ISO start of a credential's current recert cycle: expiry minus the cert's cycle length.</summary>
    public static string CycleStart(Db db, Dictionary<string, object?> cred)
    {
        var certId = cred["certification_id"] is null ? Certs.DefaultId : H.L(cred["certification_id"]);
        var cert = Certs.ById(db, certId);
        var years = cert is null ? 3 : Certs.ExpiryYears(cert);
        var expires = H.Str(cred["expires_at"]);
        var baseMs = string.IsNullOrEmpty(expires) ? H.NowMillis : H.JsMillis(expires);
        return H.IsoFromMillis(baseMs - years * 365L * 86400_000L);
    }

    /// <summary>Sum of APPROVED CPD hours for a user dated on/after the cycle start (undated entries count).</summary>
    public static double ApprovedHours(Db db, long userId, string cycleStart)
    {
        var rows = db.Query(
            "SELECT hours FROM cpd_entries WHERE user_id=? AND status='approved' AND (activity_date IS NULL OR activity_date='' OR activity_date>=?)",
            userId, cycleStart);
        return rows.Sum(r => H.D(r["hours"]));
    }

    public record Status(bool HasRequirement, double Required, double Approved, bool Met);

    /// <summary>Recert CPD status for one credential. When the cert has no requirement, Met is always true.</summary>
    public static Status ForCredential(Db db, long userId, Dictionary<string, object?> cred)
    {
        var certId = cred["certification_id"] is null ? Certs.DefaultId : H.L(cred["certification_id"]);
        var required = RequiredHours(db, certId);
        if (required <= 0) return new Status(false, 0, 0, true);
        var approved = ApprovedHours(db, userId, CycleStart(db, cred));
        return new Status(true, required, approved, approved + 1e-9 >= required);
    }

    /// <summary>Recert CPD status for the credential a student would recertify for a given certification
    /// (the most recent non-revoked credential on that cert). Null when there is no such credential.</summary>
    public static Status? ForUserCert(Db db, long userId, long certId)
    {
        var cred = db.QueryOne(
            "SELECT id,certification_id,expires_at FROM issued_credentials WHERE user_id=? AND status!='revoked' AND COALESCE(certification_id,1)=? ORDER BY id DESC",
            userId, certId);
        return cred is null ? null : ForCredential(db, userId, cred);
    }
}
