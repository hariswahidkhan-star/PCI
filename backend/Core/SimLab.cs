using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI AI Project Controls Simulation Lab — access/entitlement logic (Phase 1).
///
/// Access is computed LIVE from the student's existing PCI state (no separate account, no parallel
/// membership) — mirroring CertuvoLink.Eligible. A student reaches the Lab when the operator flag is on
/// AND they qualify under the configured `simlab_requires` rule (active membership and/or a paid exam
/// entitlement), OR they hold an explicit, in-window grant in `simulation_entitlements`
/// (admin/complimentary/sponsored/institution/marketing). Nothing here touches exam_attempts,
/// entitlements or credentials.
/// </summary>
public static class SimLab
{
    public static bool Enabled(Db db) => Settings.Bool(db, "sp_simlab_enabled", true);

    /// <summary>An explicit, currently-in-window Lab grant (admin/complimentary/sponsored/institution/…).</summary>
    public static bool HasGrant(Db db, long userId) =>
        db.QueryOne(@"SELECT id FROM simulation_entitlements
            WHERE user_id=? AND status='active'
              AND (starts_at IS NULL OR starts_at<=datetime('now'))
              AND (expires_at IS NULL OR expires_at>datetime('now'))
            LIMIT 1", userId) is not null;

    /// <summary>Does the student qualify for Lab access under the configured rule (or an explicit grant)?</summary>
    public static bool Eligible(Db db, long userId)
    {
        if (HasGrant(db, userId)) return true;
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        bool HasExam() => db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        return Settings.Str(db, "simlab_requires", "membership_or_exam") switch
        {
            "open" => true,
            "membership_and_enrolment" => hasMembership && HasExam(),
            "membership_or_exam" => hasMembership || HasExam(),
            _ => hasMembership,   // "membership" (default fallback)
        };
    }

    /// <summary>Student-facing access descriptor for GET /api/me/lab/access.</summary>
    public static object AccessFor(Db db, long userId)
    {
        var enabled = Enabled(db);
        var rule = Settings.Str(db, "simlab_requires", "membership_or_exam");
        var grant = db.QueryOne(@"SELECT source,expires_at FROM simulation_entitlements
            WHERE user_id=? AND status='active'
              AND (starts_at IS NULL OR starts_at<=datetime('now'))
              AND (expires_at IS NULL OR expires_at>datetime('now'))
            ORDER BY id DESC LIMIT 1", userId);
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        var hasExam = db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        var eligible = Eligible(db, userId);
        var hasAccess = enabled && eligible;

        // Why access is (not) granted — never a raw error; friendly, student-facing.
        string source = grant is not null ? "grant" : hasMembership ? "membership" : hasExam ? "exam" : "none";
        string reason = !enabled
            ? "The Practice Lab is not currently available."
            : hasAccess
                ? "You have access to the Project Controls Practice Lab."
                : rule == "membership_and_enrolment"
                    ? "Practice Lab access requires an active membership and a certification enrolment."
                    : "Practice Lab access is included with an active PCI membership.";

        return new
        {
            enabled,
            has_access = hasAccess,
            reason,
            rule,
            source,
            member_type = hasAccess ? CertuvoLink.DetectMemberType(db, userId) : null,
            expires_at = grant is null ? null : H.Str(grant["expires_at"]),
        };
    }
}
