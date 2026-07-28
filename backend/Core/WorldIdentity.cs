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
///                   works on both products. The canonical PCI Student Number is issued right here,
///                   in the same creating path a portal signup uses — one number per person, whichever
///                   product they arrived through, and never one derived from a World-specific id.
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

        // ONE-TIME onboarding backfill: accounts already practising when the onboarding state
        // machine shipped are established participants — gating them behind a welcome tour they
        // never needed would be a regression, not a journey. Runs once (flagged), so accounts
        // created afterwards go through onboarding normally even if they claimed anonymous work.
        if (Settings.Str(db, "world_onboarding_backfilled", "0") != "1")
        {
            db.Execute(@"UPDATE pciworld_participants SET onboarding_state='completed',
                    onboarded_at=COALESCE(onboarded_at, datetime('now'))
                WHERE onboarding_state='not_started' AND user_id IN (
                    SELECT m.canonical_user_id FROM pciworld_user_map m
                    WHERE m.canonical_user_id IS NOT NULL AND EXISTS(
                        SELECT 1 FROM pciworld_attempts a
                        WHERE a.user_id=m.legacy_world_id AND a.status='completed'))");
            Settings.Put(db, "world_onboarding_backfilled", "1");
        }

        // Cutover step 1: converge canonical ownership stamps on every resolvable owned attempt.
        // Idempotent (only NULL stamps are touched) and safe on every boot; conflicts stay NULL
        // and keep the cutover audit failing closed.
        try { BackfillAttemptOwnership(db); }
        catch (Exception e) { Console.Error.WriteLine($"[pciworld identity] ownership backfill failed: {e.Message}"); }

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
        // A World-first participant is a canonical PCI student, so they get the same canonical number
        // here — not later, and not a World-specific one derived from a World id.
        StudentNumbers.GetOrIssue(db, uid, "world_canonical_created");
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

    // ───────────────────────── World onboarding state machine (§7.3) ─────────────────────────
    //
    //   not_started → welcome → goal → preferences → privacy → completed
    //
    // Each step persists server-side on the participation row; order is ENFORCED here, so a
    // manipulated client route can never mark a missing required step complete. Completion is
    // idempotent and stamps onboarded_at exactly once.

    public static readonly string[] OnboardingSteps =
        { "not_started", "welcome", "goal", "preferences", "privacy", "completed" };

    public static string? OnboardingState(Db db, long worldUserId)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return null;
        EnsureParticipant(db, uid.Value);
        return H.Str(db.QueryOne("SELECT onboarding_state FROM pciworld_participants WHERE user_id=?", uid)!["onboarding_state"])
               ?? "not_started";
    }

    /// <summary>Record one step as DONE. Valid only when it is the current step's successor —
    /// re-posting an already-passed step is an idempotent no-op (resume after refresh), and
    /// jumping ahead is refused. Returns an error key or null.</summary>
    public static string? AdvanceOnboarding(Db db, long worldUserId, string step)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return "not_linked";
        EnsureParticipant(db, uid.Value);
        var target = Array.IndexOf(OnboardingSteps, step);
        if (target <= 0) return "bad_step";
        if (step == "completed") return CompleteOnboarding(db, worldUserId);
        var current = Array.IndexOf(OnboardingSteps, OnboardingState(db, worldUserId) ?? "not_started");
        if (target <= current) return null;                    // already passed — resume is a no-op
        if (target != current + 1) return "out_of_order";      // no skipping by route manipulation
        db.Execute("UPDATE pciworld_participants SET onboarding_state=?, last_activity_at=datetime('now') WHERE user_id=?",
            step, uid);
        return null;
    }

    /// <summary>Finish onboarding: every prior step must be done. Idempotent; onboarded_at is
    /// stamped once and never moves.</summary>
    public static string? CompleteOnboarding(Db db, long worldUserId)
    {
        var uid = CanonicalUserFor(db, worldUserId);
        if (uid is null) return "not_linked";
        EnsureParticipant(db, uid.Value);
        var state = OnboardingState(db, worldUserId) ?? "not_started";
        if (state == "completed") return null;                 // idempotent
        if (state != "privacy") return "out_of_order";
        db.Execute(@"UPDATE pciworld_participants SET onboarding_state='completed',
            onboarded_at=COALESCE(onboarded_at, datetime('now')), last_activity_at=datetime('now') WHERE user_id=?", uid);
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
        var p = db.QueryOne("SELECT goal, timezone, weekly_target, onboarding_state, preferences_json FROM pciworld_participants WHERE user_id=?", uid)!;
        bool remindersEnabled = false; long? reminderHour = null;
        try
        {
            if (H.Str(p["preferences_json"]) is { } raw && raw.Length > 0)
            {
                var el = System.Text.Json.JsonDocument.Parse(raw).RootElement;
                if (el.TryGetProperty("reminders_enabled", out var re)) remindersEnabled = re.ValueKind == System.Text.Json.JsonValueKind.True;
                if (el.TryGetProperty("reminder_hour", out var rh) && rh.ValueKind == System.Text.Json.JsonValueKind.Number)
                    reminderHour = rh.GetInt64();
            }
        }
        catch { }
        return new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["goal"] = H.Str(p["goal"]),
            ["timezone"] = H.Str(p["timezone"]),
            ["weekly_target"] = p["weekly_target"] is null ? null : H.L(p["weekly_target"]),
            ["onboarding_state"] = H.Str(p["onboarding_state"]),
            // Reminders are explicit OPT-IN (PW-US-030): absent means off, always.
            ["reminders_enabled"] = remindersEnabled,
            ["reminder_hour"] = reminderHour,
        };
    }

    /// <summary>Update World preferences. Absent keys leave stored values untouched (the P0-06
    /// lesson, applied everywhere). Returns an error key or null.</summary>
    public static string? UpdatePreferences(Db db, long worldUserId,
        string? goal, string? timezone, long? weeklyTarget,
        bool? remindersEnabled = null, long? reminderHour = null)
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
        // Reminder consent (PW-US-030): explicit opt-in stored in the participation record's
        // preferences JSON. No mail is wired to this yet — the CONSENT is the contract, recorded
        // first, so the future reminder job can never send to anyone who did not ask.
        if (remindersEnabled is not null || reminderHour is not null)
        {
            if (reminderHour is < 0 or > 23) return "bad_hour";
            var current = ReadPreferences(db, worldUserId)!;
            var merged = System.Text.Json.JsonSerializer.Serialize(new
            {
                reminders_enabled = remindersEnabled ?? (bool)(current["reminders_enabled"] ?? false),
                reminder_hour = reminderHour ?? (long?)current["reminder_hour"],
            });
            db.Execute("UPDATE pciworld_participants SET preferences_json=? WHERE user_id=?", merged, uid);
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

    /// <summary>The canonical owner of an attempt, or null for anonymous/unresolvable attempts.
    /// Prefers the STAMPED canonical column (cutover step 1); the legacy World-id resolution is
    /// the fallback until the backfill has converged everywhere.</summary>
    public static long? CanonicalOwnerOfAttempt(Db db, long attemptId)
    {
        var a = db.QueryOne("SELECT user_id, canonical_user_id FROM pciworld_attempts WHERE id=?", attemptId);
        if (a is null) return null;
        if (a["canonical_user_id"] is not null) return H.L(a["canonical_user_id"]);
        return a["user_id"] is null ? null : CanonicalUserFor(db, H.L(a["user_id"]));
    }

    /// <summary>Idempotent backfill (cutover step 1): stamp canonical ownership on every owned
    /// attempt whose legacy World id resolves through the map or the direct link. Quarantined
    /// conflicts stay NULL — the cutover audit fails closed on them. Returns rows stamped.</summary>
    public static int BackfillAttemptOwnership(Db db) =>
        db.Execute(@"UPDATE pciworld_attempts SET canonical_user_id=(
                SELECT COALESCE(m.canonical_user_id, w.student_user_id)
                FROM pciworld_users w
                LEFT JOIN pciworld_user_map m ON m.legacy_world_id=w.id AND m.canonical_user_id IS NOT NULL
                WHERE w.id=pciworld_attempts.user_id)
            WHERE user_id IS NOT NULL AND canonical_user_id IS NULL
              AND (EXISTS(SELECT 1 FROM pciworld_user_map m2
                          WHERE m2.legacy_world_id=pciworld_attempts.user_id AND m2.canonical_user_id IS NOT NULL)
                   OR EXISTS(SELECT 1 FROM pciworld_users w2
                             WHERE w2.id=pciworld_attempts.user_id AND w2.student_user_id IS NOT NULL))");

    // ───────────────────────── support diagnostics (PW-US-050) ─────────────────────────

    /// <summary>
    /// Safe participant diagnostics for support staff: identity references, mapping outcome,
    /// participation/onboarding state and COUNTS — enough to diagnose a stuck journey, and
    /// deliberately nothing more. Never answers, never tokens or their hashes, never disclosure
    /// choices, never photo references: support can see THAT a Passport exists, not what its
    /// owner chose to put on it.
    /// </summary>
    public static Dictionary<string, object?>? SupportDiagnostics(Db db, string lookup)
    {
        lookup = (lookup ?? "").Trim();
        if (lookup.Length == 0) return null;
        var w = long.TryParse(lookup, out var id)
            ? db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", id)
            : db.QueryOne("SELECT * FROM pciworld_users WHERE email=?", lookup.ToLowerInvariant());
        if (w is null) return null;
        var wid = H.L(w["id"]);
        var map = db.QueryOne(@"SELECT outcome, detail, canonical_user_id, created_at, resolved_at
            FROM pciworld_user_map WHERE legacy_world_id=?", wid);
        var canonical = CanonicalUserFor(db, wid);
        var participant = canonical is null ? null : db.QueryOne(
            "SELECT status, onboarding_state, goal, first_entered_at, onboarded_at FROM pciworld_participants WHERE user_id=?", canonical);
        var me = db.QueryOne(@"SELECT
                COUNT(*) AS total,
                SUM(CASE WHEN status='completed' THEN 1 ELSE 0 END) AS done,
                SUM(CASE WHEN status='in_progress' THEN 1 ELSE 0 END) AS open
            FROM pciworld_attempts WHERE user_id=?", wid)!;
        return new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["world_id"] = wid,
            ["email"] = H.Str(w["email"]),
            ["status"] = H.Str(w["status"]),
            ["email_verified"] = H.L(w["email_verified"]) == 1,
            ["created_at"] = H.Str(w["created_at"]),
            ["last_login_at"] = H.Str(w["last_login_at"]),
            ["mapping_outcome"] = map is null ? "none" : H.Str(map["outcome"]),
            ["mapping_detail"] = map is null ? null : H.Str(map["detail"]),
            ["canonical_user_id"] = canonical,
            ["participation_status"] = participant is null ? null : H.Str(participant["status"]),
            ["onboarding_state"] = participant is null ? null : H.Str(participant["onboarding_state"]),
            ["first_entered_at"] = participant is null ? null : H.Str(participant["first_entered_at"]),
            ["onboarded_at"] = participant is null ? null : H.Str(participant["onboarded_at"]),
            ["goal"] = participant is null ? null : H.Str(participant["goal"]),
            ["attempts_total"] = H.L(me["total"]),
            ["attempts_completed"] = H.L(me["done"]),
            ["attempts_in_progress"] = H.L(me["open"]),
            ["passport_public"] = H.L(w["passport_public"]) == 1,
            ["passport_expired"] = WorldPassport.Expired(w),
            ["visible_evidence"] = db.Scalar<long>(
                "SELECT COUNT(*) FROM pciworld_attempts WHERE user_id=? AND status='completed' AND passport_visible=1", wid),
            ["live_sessions"] = db.Scalar<long>(
                "SELECT COUNT(*) FROM pciworld_user_sessions WHERE user_id=? AND expires_at>datetime('now')", wid),
        };
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
