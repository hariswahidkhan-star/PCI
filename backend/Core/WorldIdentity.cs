using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — canonical-identity unification (journey repair P0-00).
///
/// TARGET MODEL: the platform's `users` table is the ONLY student credential authority, and
/// `student_profiles` (+ repeaters) the only canonical student profile. PCI World keeps a
/// product-PARTICIPATION record per canonical user (pciworld_participants) — World status,
/// onboarding state, goal, preferences and timestamps — never a second email, password hash,
/// MFA secret or copied professional profile.
///
/// LEGACY: pciworld_users is a separate credential table (own email + bcrypt hash), linked to a
/// student at best via student_user_id. This class installs the two bridge tables and runs the
/// reversible legacy→canonical mapping:
///
///   Rule LINKED     a World row with a valid student_user_id maps to exactly that users.id.
///   Rule CREATED    an unlinked World row whose email matches NO canonical user creates one
///                   canonical users + student_profiles pair, PRESERVING the bcrypt hash (the
///                   platform verifies with the same BCrypt), so the person's one password now
///                   works on both products. registration_no stays NULL — the platform assigns
///                   it lazily, exactly as it does for portal-registered students.
///   Rule CONFLICT   an unlinked World row whose email matches an EXISTING canonical user is
///                   QUARANTINED, never silently merged: linking two credential rows on a string
///                   match alone could hand one person's evidence to another. Resolution needs
///                   verified reauthentication or audited support action (a later phase).
///
/// The map is append-only and idempotent: a legacy row is evaluated once, its outcome recorded in
/// pciworld_user_map, and never re-decided — except a CONFLICT row that later gains a real
/// student_user_id link, which upgrades to LINKED (the person proved the identity in the portal).
/// Nothing here renumbers pciworld_attempts.user_id or changes the runtime auth path yet — the
/// map is the prerequisite that makes that cutover a mechanical, reversible step.
/// </summary>
public static class WorldIdentity
{
    public static void Ensure(Db db)
    {
        // One row per canonical user who participates in PCI World. Product data ONLY — the
        // absence of email/password/profile columns here is the design, not an omission.
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_participants(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,                      -- canonical users.id — the ONLY identity key
            status VARCHAR(16) NOT NULL DEFAULT 'active',  -- active | suspended | erased
            suspension_reason TEXT,
            onboarding_state VARCHAR(24) NOT NULL DEFAULT 'not_started', -- not_started|welcome|goal|preferences|privacy|completed
            goal VARCHAR(32),                              -- daily_practice|certification_prep|evidence|explore
            timezone VARCHAR(64),
            weekly_target INTEGER,
            preferences_json TEXT,                         -- reminders/interests; never profile data
            first_entered_at TEXT DEFAULT (datetime('now')),
            onboarded_at TEXT,
            last_activity_at VARCHAR(32) DEFAULT (datetime('now')),
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldpart_user ON pciworld_participants(user_id)");

        // Reversible legacy→canonical ledger. Append-only; the rollback path is "read the map".
        db.Exec(@"CREATE TABLE IF NOT EXISTS pciworld_user_map(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            legacy_world_id INTEGER NOT NULL,
            canonical_user_id INTEGER,                     -- NULL while quarantined
            outcome VARCHAR(16) NOT NULL,                  -- linked | created | conflict
            detail TEXT,
            resolved_at TEXT,                              -- set when a conflict is later resolved
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_worldmap_legacy ON pciworld_user_map(legacy_world_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_worldmap_user ON pciworld_user_map(canonical_user_id)");
    }

    public sealed record Reconciliation(int Linked, int Created, int Conflicts, int Upgraded, int AlreadyMapped)
    {
        public int Total => Linked + Created + Conflicts + Upgraded + AlreadyMapped;
    }

    /// <summary>Map every legacy World account to a canonical identity (rules above). Idempotent —
    /// safe on every boot; each legacy row is decided once and the decision is durable.</summary>
    public static Reconciliation Run(Db db)
    {
        int linked = 0, created = 0, conflicts = 0, upgraded = 0, already = 0;

        // A quarantined row that has SINCE been linked in the portal is the one legitimate
        // re-decision: the person authenticated as the student, so the link is proven, not guessed.
        foreach (var stale in db.Query(@"SELECT m.id AS map_id, w.id AS wid, w.student_user_id
                FROM pciworld_user_map m JOIN pciworld_users w ON w.id=m.legacy_world_id
                WHERE m.outcome='conflict' AND w.student_user_id IS NOT NULL"))
        {
            var canonical = H.L(stale["student_user_id"]);
            if (db.QueryOne("SELECT id FROM users WHERE id=?", canonical) is null) continue;
            db.Execute(@"UPDATE pciworld_user_map SET canonical_user_id=?, outcome='linked',
                    detail='conflict resolved by portal link', resolved_at=datetime('now') WHERE id=?",
                canonical, stale["map_id"]);
            EnsureParticipant(db, canonical);
            upgraded++;
        }

        foreach (var w in db.Query(@"SELECT w.* FROM pciworld_users w
                LEFT JOIN pciworld_user_map m ON m.legacy_world_id=w.id
                WHERE m.id IS NULL ORDER BY w.id"))
        {
            switch (Decide(db, w))
            {
                case "linked": linked++; break;
                case "created": created++; break;
                default: conflicts++; break;
            }
        }

        already = (int)db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_map") - linked - created - conflicts - upgraded;
        return new Reconciliation(linked, created, conflicts, upgraded, Math.Max(0, already));
    }

    /// <summary>Map ONE legacy/new World account right away (used at registration and at the portal
    /// bridge, so every account gains its canonical identity the moment it exists). No-op when the
    /// row is already mapped.</summary>
    public static void MapOne(Db db, long worldId)
    {
        if (db.QueryOne("SELECT id FROM pciworld_user_map WHERE legacy_world_id=?", worldId) is not null) return;
        var w = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", worldId);
        if (w is not null) Decide(db, w);
    }

    /// <summary>Apply the LINKED / CREATED / CONFLICT rules to one World row and record the outcome.</summary>
    static string Decide(Db db, Dictionary<string, object?> w)
    {
        var wid = H.L(w["id"]);
        var email = (H.Str(w["email"]) ?? "").Trim().ToLowerInvariant();

        // Rule LINKED — the portal already proved this identity.
        if (w["student_user_id"] is not null)
        {
            var sid = H.L(w["student_user_id"]);
            if (db.QueryOne("SELECT id FROM users WHERE id=?", sid) is not null)
            {
                Map(db, wid, sid, "linked", "student_user_id link");
                EnsureParticipant(db, sid);
                return "linked";
            }
            // A link pointing at a deleted user is a conflict, not a crash.
            Map(db, wid, null, "conflict", $"student_user_id #{sid} no longer exists");
            return "conflict";
        }

        // Rule CONFLICT — an email match alone must never merge two credential rows.
        if (db.QueryOne("SELECT id FROM users WHERE lower(email)=?", email) is not null)
        {
            Map(db, wid, null, "conflict",
                "email matches an existing canonical account — requires verified reauthentication to merge");
            return "conflict";
        }

        // Rule CREATED — a standalone World learner becomes a first-class canonical student.
        var (first, last) = SplitName(H.Str(w["display_name"]));
        var uid = db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
                VALUES(?,?,?,?,'student','active')",
            email, first, last, H.Str(w["password_hash"]));
        db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
        db.Execute("UPDATE pciworld_users SET student_user_id=? WHERE id=? AND student_user_id IS NULL", uid, wid);
        Map(db, wid, uid, "created", "canonical identity created from standalone World account (bcrypt hash preserved)");
        EnsureParticipant(db, uid);
        return "created";
    }

    /// <summary>The canonical users.id behind a legacy World account, when the mapping resolved one.</summary>
    public static long? CanonicalFor(Db db, long legacyWorldId)
    {
        var m = db.QueryOne(@"SELECT canonical_user_id FROM pciworld_user_map
            WHERE legacy_world_id=? AND canonical_user_id IS NOT NULL", legacyWorldId);
        return m is null ? null : H.L(m["canonical_user_id"]);
    }

    /// <summary>The canonical user for any World account: the map first, then the direct
    /// student_user_id link (older rows the boot pass has not decided yet). Null = unmapped
    /// (a quarantined conflict, or a deleted canonical row).</summary>
    public static long? CanonicalUserFor(Db db, long worldUserId)
    {
        var viaMap = CanonicalFor(db, worldUserId);
        if (viaMap is not null) return viaMap;
        var w = db.QueryOne("SELECT student_user_id FROM pciworld_users WHERE id=?", worldUserId);
        return w?["student_user_id"] is null ? null : H.L(w["student_user_id"]);
    }

    // ───────────────────────── shared canonical profile (P1-10) ─────────────────────────
    //
    // ONE student profile, both products. PCI World reads and writes the SAME canonical
    // users/student_profiles records the PCI AI portal uses — never a copy, never a sync job.
    // The Passport's disclosure layer is a separate consent: nothing read here becomes public
    // anywhere without the owner's explicit Passport choices.

    /// <summary>The writable subset — exactly the portal's PATCH /api/me/profile allow-list minus
    /// profile_photo (the Passport photograph is a separate, World-scoped consent).</summary>
    public static readonly string[] SharedProfileFields =
    {
        "mobile", "country", "city", "preferred_language", "current_role", "company",
        "industry_sector", "years_experience", "highest_qualification", "project_controls_area",
        "enrollment_purpose", "linkedin_url",
    };

    /// <summary>The canonical profile as the World surface may see it. Null when the World account
    /// has no resolved canonical identity (quarantined conflict).</summary>
    public static Dictionary<string, object?>? ReadSharedProfile(Db db, long worldUserId)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return null;
        var u = db.QueryOne("SELECT first_name, last_name FROM users WHERE id=?", uid);
        if (u is null) return null;
        var p = db.QueryOne("SELECT * FROM student_profiles WHERE user_id=?", uid);
        var outRow = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["user_id"] = uid,
            ["first_name"] = H.Str(u["first_name"]),
            ["last_name"] = H.Str(u["last_name"]),
            ["profile_completion_percentage"] = p is null ? 20L : H.L(p["profile_completion_percentage"]),
        };
        foreach (var f in SharedProfileFields) outRow[f] = p is null ? null : H.Str(p[f]);
        return outRow;
    }

    /// <summary>Write allow-listed fields through to the canonical student_profiles row — the same
    /// mutation, caps and completion recompute the portal performs, so a save made here is simply
    /// THE profile on the next portal read. Returns an error key or null.</summary>
    public static string? UpdateSharedProfile(Db db, long worldUserId, Dictionary<string, System.Text.Json.JsonElement> body)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return "not_linked";
        if (db.QueryOne("SELECT user_id FROM student_profiles WHERE user_id=?", uid) is null)
            db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
        var set = SharedProfileFields.Where(body.ContainsKey).ToList();
        if (set.Count > 0)
        {
            // Same 1000-char cap and back-quoted identifiers as the portal (`current_role` is a
            // reserved word on MySQL/MariaDB).
            var vals = set.Select(k => { var s = H.GetS(body, k) ?? ""; return (object?)(s.Length > 1000 ? s[..1000] : s); })
                .Append(uid).ToArray();
            db.Execute($"UPDATE student_profiles SET {string.Join(",", set.Select(k => $"`{k}`=?"))} WHERE user_id=?", vals);
        }
        Endpoints.Account.RecomputeCompletion(db, uid.Value);
        return null;
    }

    // ───────────────────────── World preferences on the participation row ─────────────────────────
    //
    // Product data ONLY (§10.5): the goal, timezone and weekly target live on
    // pciworld_participants keyed by canonical users.id — never on the canonical profile.

    public static readonly string[] Goals = { "daily_practice", "certification_prep", "evidence", "explore" };

    /// <summary>The account's World preferences, or null when it has no resolved canonical
    /// identity (quarantined conflict — there is no participation row to hold them).</summary>
    public static Dictionary<string, object?>? ReadPreferences(Db db, long worldUserId)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return null;
        EnsureParticipant(db, uid.Value);
        var p = db.QueryOne("SELECT goal, timezone, weekly_target, onboarding_state FROM pciworld_participants WHERE user_id=?", uid)!;
        return new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["goal"] = H.Str(p["goal"]),
            ["timezone"] = H.Str(p["timezone"]),
            ["weekly_target"] = p["weekly_target"] is null ? null : H.L(p["weekly_target"]),
            ["onboarding_state"] = H.Str(p["onboarding_state"]),
        };
    }

    /// <summary>Update World preferences. Absent keys leave stored values untouched (the P0-06
    /// lesson, applied everywhere). Returns an error key or null.</summary>
    public static string? UpdatePreferences(Db db, long worldUserId,
        string? goal, string? timezone, long? weeklyTarget)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return "not_linked";
        EnsureParticipant(db, uid.Value);
        if (goal is not null)
        {
            if (!Goals.Contains(goal)) return "bad_goal";
            db.Execute("UPDATE pciworld_participants SET goal=?, last_activity_at=datetime('now') WHERE user_id=?", goal, uid);
        }
        if (timezone is not null)
        {
            var tz = timezone.Trim();
            if (tz.Length is 0 or > 64) return "bad_timezone";
            db.Execute("UPDATE pciworld_participants SET timezone=? WHERE user_id=?", tz, uid);
        }
        if (weeklyTarget is not null)
        {
            if (weeklyTarget is < 0 or > 7) return "bad_target";
            db.Execute("UPDATE pciworld_participants SET weekly_target=? WHERE user_id=?", weeklyTarget, uid);
        }
        return null;
    }

    // ───────────────────────── attempt-namespace reconciliation (cutover groundwork) ─────────────────────────

    public sealed record OwnershipAudit(long OwnedAttempts, long Resolvable, long Orphaned)
    {
        /// <summary>True when every owned attempt's user_id (legacy World namespace) resolves to a
        /// canonical identity — the precondition for a mechanical namespace cutover.</summary>
        public bool CutoverReady => Orphaned == 0;
    }

    /// <summary>Reconciliation: pciworld_attempts.user_id values live in the LEGACY World-id
    /// namespace; this proves how many resolve to a canonical user through the map or the direct
    /// link. Orphaned &gt; 0 means a cutover would strand evidence — fix the mapping first.</summary>
    public static OwnershipAudit AuditAttemptOwnership(Db db)
    {
        var owned = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts WHERE user_id IS NOT NULL");
        var resolvable = db.Scalar<long>(@"SELECT COUNT(*) FROM pciworld_attempts a
            WHERE a.user_id IS NOT NULL AND (
              EXISTS(SELECT 1 FROM pciworld_user_map m
                     WHERE m.legacy_world_id=a.user_id AND m.canonical_user_id IS NOT NULL)
              OR EXISTS(SELECT 1 FROM pciworld_users w
                        WHERE w.id=a.user_id AND w.student_user_id IS NOT NULL))");
        return new(owned, resolvable, owned - resolvable);
    }

    /// <summary>The canonical owner of an attempt (via its legacy World user id), or null for
    /// anonymous/unresolvable attempts. The read path future product surfaces will use.</summary>
    public static long? CanonicalOwnerOfAttempt(Db db, long attemptId)
    {
        var a = db.QueryOne("SELECT user_id FROM pciworld_attempts WHERE id=?", attemptId);
        return a?["user_id"] is null ? null : CanonicalUserFor(db, H.L(a["user_id"]));
    }

    public static void EnsureParticipant(Db db, long canonicalUserId) =>
        db.Execute("INSERT OR IGNORE INTO pciworld_participants(user_id) VALUES(?)", canonicalUserId);

    static void Map(Db db, long legacyId, long? canonicalId, string outcome, string detail) =>
        db.Execute(@"INSERT OR IGNORE INTO pciworld_user_map(legacy_world_id,canonical_user_id,outcome,detail)
            VALUES(?,?,?,?)", legacyId, canonicalId, outcome, detail);

    static (string? First, string? Last) SplitName(string? display)
    {
        var s = display?.Trim();
        if (string.IsNullOrEmpty(s)) return (null, null);
        var i = s.IndexOf(' ');
        return i < 0 ? (s, null) : (s[..i], s[(i + 1)..].Trim());
    }
}
