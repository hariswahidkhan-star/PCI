using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Membership grades (professional standing), matching the public membership pages:
///   Student → Associate (APCI) → Professional (MPCI) → Fellow (FPCI).
/// The grade is separate from membership_type (the product): it carries the post-nominal and the
/// progression. Associate is the default paid grade; Professional requires holding an active PCI
/// certification (self-service upgrade once earned); Fellow is by nomination (admin/board review).
/// </summary>
public static class MembershipGrades
{
    public record Grade(string Key, string Label, string PostNominal, int Rank, bool SelfService, string Eligibility);

    public static readonly Grade[] All =
    {
        new("student",      "Student Member",      "SPCI", 0, true,  "For those studying or exploring the profession."),
        new("associate",    "Associate Member",    "APCI", 1, true,  "Early-career professionals working toward a PCI certification."),
        new("professional", "Professional Member", "MPCI", 2, false, "Hold an active PCI certification."),
        new("fellow",       "Fellow",              "FPCI", 3, false, "By nomination — a sustained record of contribution to the profession and the institute."),
    };

    public static Grade ByKey(string? key) => All.FirstOrDefault(g => g.Key == key) ?? All[1];  // default: associate
    public static bool IsGrade(string? key) => All.Any(g => g.Key == key);

    public static string CurrentKey(Db db, long userId) =>
        H.Str(db.QueryOne("SELECT grade FROM memberships WHERE user_id=? ORDER BY id DESC", userId)?["grade"]) ?? "associate";

    public static bool HoldsActiveCredential(Db db, long userId) =>
        db.QueryOne("SELECT id FROM issued_credentials WHERE user_id=? AND status='active'", userId) is not null;

    /// <summary>The grade the member can self-upgrade to right now (Professional once they hold a credential),
    /// or null when no self-service upgrade is available. Fellow is never self-service (nomination only).</summary>
    public static Grade? SelfServiceUpgrade(Db db, long userId)
    {
        var cur = ByKey(CurrentKey(db, userId));
        if (cur.Rank < 2 && HoldsActiveCredential(db, userId)) return ByKey("professional");
        return null;
    }

    /// <summary>True if the member may apply for Fellowship (nomination): a Professional Member (MPCI) — the
    /// senior grade builds on Professional — with no pending Fellowship application.</summary>
    public static bool CanApplyForFellow(Db db, long userId)
    {
        var cur = ByKey(CurrentKey(db, userId));
        if (cur.Rank != 2) return false;                                   // must be Professional (not Student/Associate/Fellow)
        var pending = db.QueryOne("SELECT id FROM membership_upgrades WHERE user_id=? AND to_grade='fellow' AND status='pending'", userId);
        return pending is null;
    }

    public static object Snapshot(Db db, long userId)
    {
        var cur = ByKey(CurrentKey(db, userId));
        var up = SelfServiceUpgrade(db, userId);
        var pending = db.QueryOne("SELECT to_grade,status,created_at FROM membership_upgrades WHERE user_id=? AND status='pending' ORDER BY id DESC", userId);
        return new
        {
            current = cur.Key,
            label = cur.Label,
            post_nominal = cur.PostNominal,
            rank = cur.Rank,
            eligible_upgrade = up is null ? null : new { key = up.Key, label = up.Label, post_nominal = up.PostNominal },
            can_apply_fellow = CanApplyForFellow(db, userId),
            pending_application = pending is null ? null : new { to_grade = H.Str(pending["to_grade"]), created_at = H.Str(pending["created_at"]) },
        };
    }

    /// <summary>Set a member's grade on their latest membership row (creating none). Returns false if absent.</summary>
    public static bool SetGrade(Db db, long userId, string gradeKey)
    {
        var m = db.QueryOne("SELECT id FROM memberships WHERE user_id=? ORDER BY id DESC", userId);
        if (m is null || !IsGrade(gradeKey)) return false;
        db.Execute("UPDATE memberships SET grade=? WHERE id=?", gradeKey, m["id"]);
        return true;
    }
}
