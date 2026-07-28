using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World contributor publishing — the standing grant and the cross-namespace maker-checker
/// (docs/pciworld/CCP_PHASE6_DESIGN.md §4, §5).
///
/// TWO OBLIGATIONS LIVE HERE, and they are the two the phase is judged on.
///
/// 1. A CONTRIBUTOR MAY NEVER APPROVE OR PUBLISH THEIR OWN ARTICLE. The editorial engine already
///    refuses when the acting admin IS the article's <c>author_id</c> — correct for house content,
///    where both are <c>pciworld_admin_users</c> ids. A contributor is a <c>pciworld_users</c> row.
///    The two id spaces are disjoint, so for a contributor's manuscript that comparison could never
///    fire: id 7 in one table and id 7 in the other are unrelated people, and a staff editor who
///    was also the contributor would sail straight through. <see cref="IsSelfReview"/> is the
///    comparison made in the namespace where it means something, via the explicit
///    <c>pciworld_admin_users.world_user_id</c> link.
///
///    What this CANNOT do, said plainly rather than implied away: an admin whose link is unset and
///    who quietly holds a second Passport account defeats it. Code cannot prove two accounts are
///    one person when nobody has said so. That residue belongs to the conflict-of-interest limb of
///    the editorial policy (CCP-P6-017). The declared link makes the honest case easy; the check
///    makes the recorded case impossible.
///
/// 2. THE GRANT IS A HUMAN ACT, NOT A THRESHOLD. Forum standing is evidence shown to the editor,
///    never the decision. An earned-only gate is a ratchet an abuser plays for — post innocuously
///    until it opens, then publish under PCI's byline — and unlike a forum post, an article is
///    indexed under PCI's name in caches nobody at PCI controls. Revocation is likewise a recorded
///    human decision with a reason, never an automatic consequence of a report count or a
///    classifier score.
/// </summary>
public static class WorldContributors
{
    public static bool Enabled(Db db) => Settings.Bool(db, "pciworld_contributors_enabled", false);

    /// The terms the applicant accepted, pinned at application. Blank means no terms have been
    /// authored, and the application endpoint refuses rather than recording acceptance of nothing.
    public static string TermsVersion(Db db) => Settings.Str(db, "pciworld_contributor_terms_version", "") ?? "";

    // ── The grant ──────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Apply to contribute. Returns null on success, otherwise a reason code.
    ///
    /// Re-applying after a decline updates the one standing row rather than writing a second: "may
    /// this person publish, and on whose authority" must have exactly one answer. A REVOKED person
    /// cannot re-apply their way back in — restoring standing is a staff decision, because
    /// otherwise revocation lasts exactly as long as it takes to press the button again.
    /// </summary>
    public static string? Apply(Db db, long userId, string? statement)
    {
        if (!Enabled(db)) return "disabled";
        var terms = TermsVersion(db);
        if (string.IsNullOrWhiteSpace(terms)) return "terms_unavailable";

        var u = db.QueryOne("SELECT email_verified FROM pciworld_users WHERE id=?", userId);
        if (u is null) return "not_found";
        // A verified email is the floor for applying: it is the cheapest evidence that an account
        // belongs to a reachable person, and every editorial decision below needs somewhere to send
        // its outcome.
        if (!H.B(u["email_verified"])) return "email_not_verified";

        var existing = db.QueryOne("SELECT state,version FROM pciworld_contributors WHERE user_id=?", userId);
        if (existing is not null)
        {
            var state = H.Str(existing["state"]) ?? "";
            if (state is "granted") return "already_granted";
            if (state is "applied") return "already_applied";
            if (state is "revoked") return "revoked";     // only a person can undo a person's decision
            return db.Execute(
                @"UPDATE pciworld_contributors SET state='applied', statement=?, terms_version=?,
                         updated_at=datetime('now'), version=version+1
                   WHERE id=(SELECT id FROM (SELECT id FROM pciworld_contributors WHERE user_id=?) t)
                     AND version=?",
                Trim(statement, 4000), terms, userId, H.L(existing["version"])) == 0 ? "conflict" : null;
        }

        db.Execute(
            "INSERT INTO pciworld_contributors(user_id,state,statement,terms_version) VALUES(?,'applied',?,?)",
            userId, Trim(statement, 4000), terms);
        return null;
    }

    /// <summary>
    /// Grant, decline or revoke contributor standing. Admin-only in its entirety: there is no
    /// argument value by which an applicant reaches their own decision.
    ///
    /// `adminId` and `when` are recorded for the same reason Phase 4 records
    /// <c>verified_by_admin_id</c>: a bare state nobody can later explain is an unattributable
    /// claim, and "who let this person publish under our name" is exactly the question an editorial
    /// incident asks.
    /// </summary>
    public static string? Decide(Db db, long userId, string decision, long adminId, string? reason, int expectedVersion)
    {
        if (decision is not ("granted" or "declined" or "revoked")) return "bad_decision";

        var admin = db.QueryOne("SELECT role,status FROM pciworld_admin_users WHERE id=?", adminId);
        if (admin is null || H.Str(admin["status"]) != "active" ||
            !WorldRbac.Allowed(H.Str(admin["role"]), "editorial.contributors"))
            return "forbidden";

        var row = db.QueryOne("SELECT state,version FROM pciworld_contributors WHERE user_id=?", userId);
        if (row is null) return "not_found";
        if (H.L(row["version"]) != expectedVersion) return "conflict";
        var from = H.Str(row["state"]) ?? "";
        if (!TransitionAllowed(from, decision)) return "invalid_transition";
        // Revocation is a human act and must carry its reason. A revocation nobody can explain is
        // indistinguishable from a mistake, and the person it happened to cannot appeal it.
        if (decision == "revoked" && string.IsNullOrWhiteSpace(reason)) return "reason_required";

        string? result = null;
        db.Transaction(() =>
        {
            var n = decision switch
            {
                "granted" => db.Execute(
                    @"UPDATE pciworld_contributors SET state='granted', granted_at=datetime('now'),
                             granted_by_admin_id=?, revoked_at=NULL, revoke_reason=NULL,
                             updated_at=datetime('now'), version=version+1
                       WHERE user_id=? AND version=?", adminId, userId, expectedVersion),
                "revoked" => db.Execute(
                    @"UPDATE pciworld_contributors SET state='revoked', revoked_at=datetime('now'),
                             revoked_by_admin_id=?, revoke_reason=?, updated_at=datetime('now'), version=version+1
                       WHERE user_id=? AND version=?", adminId, Trim(reason, 2000), userId, expectedVersion),
                _ => db.Execute(
                    @"UPDATE pciworld_contributors SET state='declined', updated_at=datetime('now'), version=version+1
                       WHERE user_id=? AND version=?", userId, expectedVersion),
            };
            if (n == 0) { result = "conflict"; return; }
            db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)",
                adminId, $"contributor.{decision}", $"user:{userId} {from}->{decision}");
        });
        return result;
    }

    /// draft/none → applied is Apply's business; this is the staff side only.
    public static bool TransitionAllowed(string from, string to) => (from, to) switch
    {
        ("applied", "granted") => true,
        ("applied", "declined") => true,
        ("granted", "revoked") => true,
        ("revoked", "granted") => true,     // restoring standing is a staff decision, and allowed
        ("declined", "granted") => true,    // an editor may change their mind on the same application
        _ => false,
    };

    /// <summary>
    /// May this account publish under PCI's byline right now? Re-read on every call and never
    /// cached — a revocation has to bite on the next publish attempt, not at the next sign-in.
    /// </summary>
    public static bool MayPublish(Db db, long userId) =>
        db.QueryOne(
            @"SELECT c.id FROM pciworld_contributors c
              JOIN pciworld_users u ON u.id = c.user_id
              WHERE c.user_id=? AND c.state='granted' AND (u.status IS NULL OR u.status='active')",
            userId) is not null;

    // ── The maker-checker, made in a namespace where it means something ────────────────────────

    /// <summary>
    /// True when the admin acting on this article IS its contributor.
    ///
    /// The comparison is <c>pciworld_admin_users.world_user_id</c> against
    /// <c>pciworld_articles.contributor_user_id</c> — the only pair of columns in the two systems
    /// that refer to the same namespace. A NULL link means the admin has not declared a World
    /// account: that is not evidence of innocence, it is absence of evidence, and it is why §5.2 of
    /// the design routes the residue to the conflict-of-interest policy instead of claiming the
    /// code settles it.
    /// </summary>
    public static bool IsSelfReview(Db db, long articleId, long actingAdminId)
    {
        // Read through QueryOne + H.Ln rather than Scalar<long?>: Db.Scalar goes through
        // Convert.ChangeType, which cannot produce a Nullable<long> and throws — surfacing as a 500
        // on a security check, which is the worst possible way for one to fail.
        var art = db.QueryOne("SELECT contributor_user_id FROM pciworld_articles WHERE id=?", articleId);
        var contributor = art is null ? null : H.Ln(art["contributor_user_id"]);
        if (contributor is null or 0) return false;          // house content: the existing check covers it
        var adm = db.QueryOne("SELECT world_user_id FROM pciworld_admin_users WHERE id=?", actingAdminId);
        var linked = adm is null ? null : H.Ln(adm["world_user_id"]);
        return linked is not null and not 0 && linked == contributor;
    }

    /// <summary>
    /// The publish-time eligibility gate. Approval is necessary and never sufficient — the
    /// editorial engine already re-validates at publication because sources can change between the
    /// two, and standing can change in exactly the same window.
    ///
    /// Note what this deliberately does NOT do: it never touches an ALREADY published article. A
    /// sanction is about conduct and unpublishing is about content; conflating them would turn
    /// every sanction into retroactive content destruction. Archiving a published piece is a
    /// separate, audited, human act.
    /// </summary>
    public static string? PublishGate(Db db, long articleId)
    {
        var art = db.QueryOne("SELECT contributor_user_id FROM pciworld_articles WHERE id=?", articleId);
        var contributor = art is null ? null : H.Ln(art["contributor_user_id"]);
        if (contributor is null or 0) return null;
        return MayPublish(db, contributor.Value) ? null : "contributor_not_eligible";
    }

    static string? Trim(string? s, int max) =>
        string.IsNullOrWhiteSpace(s) ? null : (s.Trim().Length > max ? s.Trim()[..max] : s.Trim());
}
