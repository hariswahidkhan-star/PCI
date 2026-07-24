using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>PCI World admin RBAC — server-side, per action group. Hiding a button is not
/// authorization; every world-admin endpoint calls Allowed() before touching data.</summary>
public static class WorldRbac
{
    public static readonly string[] Roles = { "owner", "author", "reviewer", "publisher", "viewer" };

    /// <summary>Action groups: read | author (draft CRUD, validate, submit for review) |
    /// review (approve/reject) | publish (publish, retire/restore, calendar) | admin (user management).</summary>
    public static bool Allowed(string? role, string action) => role switch
    {
        "owner" => true,
        "author" => action is "read" or "author",
        "reviewer" => action is "read" or "review",
        "publisher" => action is "read" or "publish",
        "viewer" => action is "read",
        _ => false,
    };
}

/// <summary>
/// PCI World challenge lifecycle — the working-copy state machine plus the publish snapshot.
///
///   draft → in_review → approved → published   (independent maker-checker at approve)
///   published → draft                          (revise: edit a NEW version; the last published
///                                               snapshot keeps serving until republished)
///   retired flag                               (hides from rotation; history untouched)
///
/// Publishing is the only way content reaches pciworld_challenge_versions, and versions are
/// immutable: attempts pin (challenge_id, version) and replay only from the snapshot.
/// </summary>
public static class WorldLifecycle
{
    public static bool CanEdit(string? status) => status == "draft";

    public static WorldContent.ChallengeInput InputFor(Dictionary<string, object?> row) => new(
        H.Str(row["code"]) ?? "", H.Str(row["title"]), H.Str(row["hook"]), H.Str(row["industry"]),
        H.Str(row["track"]), H.Str(row["difficulty"]), H.L(row["est_minutes"]),
        H.L(row["synthetic_declared"]) == 1, H.Str(row["config_json"]));

    public static string? SubmitReview(Db db, long id)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='in_review', updated_at=datetime('now') WHERE id=? AND status='draft'", id);
        return n == 0 ? "not_draft" : null;
    }

    /// <summary>Approve = the maker-checker checkpoint: the approver must not be the author, and
    /// the §14-style validator must pass on the working copy.</summary>
    public static string? Approve(Db db, long id, long approverId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "in_review") return "not_in_review";
        if (row["author_id"] is not null && H.L(row["author_id"]) == approverId) return "maker_checker";
        if (!WorldContent.Publishable(WorldContent.Validate(InputFor(row)))) return "not_publishable";
        db.Execute("UPDATE pciworld_challenges SET status='approved', approved_by=?, updated_at=datetime('now') WHERE id=? AND status='in_review'",
            approverId, id);
        return null;
    }

    public static string? Reject(Db db, long id, long reviewerId, string? note)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='draft', reviewed_by=?, review_note=?, updated_at=datetime('now') WHERE id=? AND status='in_review'",
            reviewerId, note, id);
        return n == 0 ? "not_in_review" : null;
    }

    /// <summary>Publish the approved working copy as an immutable new version. Re-validates at the
    /// moment of publication — approval is necessary but never sufficient.</summary>
    public static string? Publish(Db db, long id, long publisherId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "approved") return "not_approved";
        if (!WorldContent.Publishable(WorldContent.Validate(InputFor(row)))) return "not_publishable";
        var next = H.L(row["current_version"]) + 1;
        db.Execute(@"INSERT INTO pciworld_challenge_versions
                (challenge_id,version,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,config_json,published_by)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?)",
            id, next, H.Str(row["title"]), H.Str(row["hook"]), H.Str(row["industry"]), H.Str(row["role"]),
            H.Str(row["track"]), H.Str(row["difficulty"]), H.L(row["est_minutes"]),
            H.Str(row["competencies_json"]), H.Str(row["config_json"]), publisherId);
        db.Execute(@"UPDATE pciworld_challenges SET status='published', current_version=?,
                published_at=COALESCE(published_at, datetime('now')), updated_at=datetime('now') WHERE id=?", next, id);
        return null;
    }

    public static string? Revise(Db db, long id)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='draft', approved_by=NULL, updated_at=datetime('now') WHERE id=? AND status='published'", id);
        return n == 0 ? "not_published" : null;
    }

    public static void Retire(Db db, long id) =>
        db.Execute("UPDATE pciworld_challenges SET retired=1, updated_at=datetime('now') WHERE id=?", id);

    public static void Restore(Db db, long id) =>
        db.Execute("UPDATE pciworld_challenges SET retired=0, updated_at=datetime('now') WHERE id=?", id);

    /// <summary>The latest immutable snapshot for a servable challenge (published at least once,
    /// not retired) — the ONLY content the public surface ever serves.</summary>
    public static Dictionary<string, object?>? LiveVersion(Db db, long challengeId)
    {
        var ch = db.QueryOne("SELECT id,current_version,retired FROM pciworld_challenges WHERE id=? AND current_version>=1 AND retired=0", challengeId);
        if (ch is null) return null;
        return db.QueryOne("SELECT * FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?",
            challengeId, H.L(ch["current_version"]));
    }

    /// <summary>The exact snapshot an attempt pinned — historical replay never moves.</summary>
    public static Dictionary<string, object?>? PinnedVersion(Db db, long challengeId, long version) =>
        db.QueryOne("SELECT * FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?", challengeId, version);

    /// <summary>Today's challenge (UTC day): the calendar override when set and servable, else the
    /// deterministic day-of-year rotation over servable challenges. Returns the challenge row.</summary>
    public static Dictionary<string, object?>? Today(Db db, DateTime utcNow)
    {
        var day = utcNow.ToString("yyyy-MM-dd");
        var cal = db.QueryOne("SELECT challenge_id FROM pciworld_calendar WHERE day_utc=?", day);
        if (cal is not null)
        {
            var pick = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=? AND current_version>=1 AND retired=0", cal["challenge_id"]);
            if (pick is not null) return pick;
        }
        var servable = db.Query("SELECT * FROM pciworld_challenges WHERE current_version>=1 AND retired=0 ORDER BY id ASC");
        if (servable.Count == 0) return null;
        return servable[(int)(utcNow.DayOfYear % servable.Count)];
    }
}
