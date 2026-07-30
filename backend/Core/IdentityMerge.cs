using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Maker-checker duplicate-account merges (spec §4.11): fold one canonical student account (the
/// SOURCE, the loser) into another (the TARGET, the survivor), under a two-person rule.
///
/// The control structure is the design:
///
///   • One admin holding 'id_merge_request' CREATES a request; a DIFFERENT admin holding
///     'id_merge_approve' decides it. The maker can never approve their own request even if they
///     hold both permissions — the check is on identity, not on permission overlap.
///   • The detail view is a PREVIEW: per-table counts of affected rows for BOTH accounts — counts,
///     never personal dumps — so the checker can see the blast radius before deciding.
///   • Execution is ONE transaction. Both users are locked in deterministic order (lower id first)
///     so two concurrent merges touching the same pair cannot deadlock; the decision claim is a
///     guarded UPDATE (status='pending'), so a retry or a racing second approver gets
///     'already_decided' rather than a second execution.
///   • The survivor keeps their Student Number untouched. The loser's number goes to
///     state='merged' in pci_student_number_registry, recording the survivor's number and
///     canonical user id — a private support-resolution pointer; public verification already
///     resolves merged numbers via their successor.
///   • Tables with a per-user uniqueness shape are resolved deliberately, never blanket-updated:
///     where both accounts hold a colliding row the survivor's is kept and the loser's is left
///     behind, marked, on the (now inert) merged account rather than violating a constraint.
/// </summary>
public static class IdentityMerge
{
    /// <summary>Child tables safe to re-point wholesale: nothing in them is unique per user, so
    /// moving every loser row to the survivor cannot violate a constraint.</summary>
    static readonly string[] BlanketTables =
    {
        "payments", "exam_entitlements", "exam_bookings", "exam_attempts",
        "issued_credentials", "memberships", "founding_applications",
    };

    static bool Has(Db db, string table)
    {
        try { return db.Columns(table).Count > 0; } catch { return false; }
    }

    static long N(Db db, string sql, params object?[] a)
    {
        try { return db.Scalar<long>(sql, a); } catch { return -1; }
    }

    /// <summary>
    /// Per-table affected-row counts for one account — the preview a checker decides on, and the
    /// shape stored in the before/after snapshots. Counts only, never member records: deciding a
    /// merge needs the blast radius, not the person's data.
    /// </summary>
    public static Dictionary<string, object?> Counts(Db db, long userId)
    {
        var c = new Dictionary<string, object?>
        {
            ["applications"] = N(db, "SELECT COUNT(*) FROM founding_applications WHERE user_id=?", userId),
            ["payments"] = N(db, "SELECT COUNT(*) FROM payments WHERE user_id=?", userId),
            ["exam_entitlements"] = N(db, "SELECT COUNT(*) FROM exam_entitlements WHERE user_id=?", userId),
            ["exam_bookings"] = N(db, "SELECT COUNT(*) FROM exam_bookings WHERE user_id=?", userId),
            ["exam_attempts"] = N(db, "SELECT COUNT(*) FROM exam_attempts WHERE user_id=?", userId),
            ["credentials"] = N(db, "SELECT COUNT(*) FROM issued_credentials WHERE user_id=?", userId),
            ["memberships"] = N(db, "SELECT COUNT(*) FROM memberships WHERE user_id=?", userId),
            ["cpd_entries"] = N(db, "SELECT COUNT(*) FROM cpd_entries WHERE user_id=?", userId),
            ["event_registrations"] = N(db, "SELECT COUNT(*) FROM event_registrations WHERE user_id=?", userId),
            ["student_profiles"] = N(db, "SELECT COUNT(*) FROM student_profiles WHERE user_id=?", userId),
            ["sessions"] = N(db, "SELECT COUNT(*) FROM login_tokens WHERE user_id=?", userId),
        };
        // World-side tables are installed by their module's Ensure, not schema.sql — count them
        // when present, report -1 (unknown) rather than 0 when the query itself failed.
        c["world_attempts"] = Has(db, "pciworld_attempts")
            ? N(db, "SELECT COUNT(*) FROM pciworld_attempts WHERE canonical_user_id=?", userId) : 0L;
        c["passport_evidence"] = Has(db, "pciworld_attempts")
            ? N(db, "SELECT COUNT(*) FROM pciworld_attempts WHERE canonical_user_id=? AND status='completed' AND passport_visible=1", userId) : 0L;
        c["handoff_codes"] = Has(db, "pciworld_handoff_codes")
            ? N(db, @"SELECT COUNT(*) FROM pciworld_handoff_codes WHERE world_user_id IN (
                    SELECT w.id FROM pciworld_users w WHERE w.student_user_id=?
                    UNION SELECT m.legacy_world_id FROM pciworld_user_map m WHERE m.canonical_user_id=?)",
                userId, userId) : 0L;
        return c;
    }

    /// <summary>Create a merge request (the maker's half). Returns (id, null) or (0, error key):
    /// self_merge | unknown_user | not_student | not_mergeable | already_pending.</summary>
    public static (long id, string? error) CreateRequest(Db db, long sourceId, long targetId, string? reason, long requestedBy)
    {
        if (sourceId == targetId) return (0, "self_merge");
        var src = db.QueryOne("SELECT id,role,status FROM users WHERE id=?", sourceId);
        var dst = db.QueryOne("SELECT id,role,status FROM users WHERE id=?", targetId);
        if (src is null || dst is null) return (0, "unknown_user");
        if (H.Str(src["role"]) != "student" || H.Str(dst["role"]) != "student") return (0, "not_student");
        // An erased account has no identity left to merge; an already-merged one has been absorbed
        // elsewhere — pointing a second merge at either would corrupt the audit chain.
        if (H.Str(src["status"]) is "erased" or "merged" || H.Str(dst["status"]) is "erased" or "merged")
            return (0, "not_mergeable");
        // One pending merge per person, on either side of it. Two simultaneous plans over the same
        // account cannot both be right, and the checker must see the whole picture in one request.
        var pending = db.Scalar<long>(@"SELECT COUNT(*) FROM pci_identity_merges WHERE status='pending'
            AND (source_user_id IN (?,?) OR target_user_id IN (?,?))", sourceId, targetId, sourceId, targetId);
        if (pending > 0) return (0, "already_pending");

        var id = db.ExecuteReturningId(@"INSERT INTO pci_identity_merges
                (source_user_id,target_user_id,reason,status,requested_by,correlation_id)
            VALUES(?,?,?,'pending',?,?)", sourceId, targetId, reason, requestedBy, Security.RandomHex(8));
        return (id, null);
    }

    /// <summary>The request rows for the list view — decision metadata only, no snapshots.</summary>
    public static List<Dictionary<string, object?>> List(Db db) =>
        db.Query(@"SELECT id,source_user_id,target_user_id,reason,status,requested_by,requested_at,
                decided_by,decided_at,decision_note,correlation_id
            FROM pci_identity_merges ORDER BY id DESC LIMIT 200");

    /// <summary>One request plus its live preview: per-table counts for both accounts.</summary>
    public static Dictionary<string, object?>? Get(Db db, long id)
    {
        var row = db.QueryOne("SELECT * FROM pci_identity_merges WHERE id=?", id);
        if (row is null) return null;
        return new Dictionary<string, object?>
        {
            ["request"] = row,
            ["preview"] = new Dictionary<string, object?>
            {
                ["source"] = Counts(db, H.L(row["source_user_id"])),
                ["target"] = Counts(db, H.L(row["target_user_id"])),
            },
        };
    }

    /// <summary>
    /// Approve and EXECUTE a pending merge in one transaction (the checker's half).
    /// Returns (null, summary) on success, or an error key:
    /// not_found | maker_checker | already_decided | unknown_user | not_mergeable.
    /// </summary>
    public static (string? error, Dictionary<string, object?>? result) Approve(Db db, long mergeId, long approverId, string? note)
    {
        var m = db.QueryOne("SELECT * FROM pci_identity_merges WHERE id=?", mergeId);
        if (m is null) return ("not_found", null);
        // THE two-person rule. Identity, not permission: an admin holding both id_merge_request
        // and id_merge_approve still cannot approve a request they made.
        if (H.L(m["requested_by"]) == approverId) return ("maker_checker", null);
        if (H.Str(m["status"]) != "pending") return ("already_decided", null);

        var sourceId = H.L(m["source_user_id"]);   // loser
        var targetId = H.L(m["target_user_id"]);   // survivor
        var corr = H.Str(m["correlation_id"]);
        var world = Has(db, "pciworld_users") && Has(db, "pciworld_user_map");
        Dictionary<string, object?>? summary = null;
        string? failure = null;

        try
        {
            db.Transaction(() =>
            {
                // Claim the decision first, guarded on 'pending'. This is the idempotency and race
                // gate: a retry after success, or a second approver landing a moment later, changes
                // zero rows here and the whole transaction unwinds to 'already_decided'.
                var claimed = db.Execute(@"UPDATE pci_identity_merges
                    SET status='approved', decided_by=?, decided_at=datetime('now'), decision_note=?,
                        row_version=row_version+1
                    WHERE id=? AND status='pending'", approverId, note, mergeId);
                if (claimed == 0) throw new InvalidOperationException("already_decided");

                // Lock and read both users in deterministic order — lower id first — so two merges
                // touching the same pair in opposite roles acquire row locks in the same order and
                // cannot deadlock each other on MySQL.
                var (lo, hi) = sourceId < targetId ? (sourceId, targetId) : (targetId, sourceId);
                db.Execute("UPDATE users SET updated_at=updated_at WHERE id=?", lo);
                db.Execute("UPDATE users SET updated_at=updated_at WHERE id=?", hi);
                var src = db.QueryOne("SELECT id,email,registration_no,status,role FROM users WHERE id=?", sourceId);
                var dst = db.QueryOne("SELECT id,email,registration_no,status,role FROM users WHERE id=?", targetId);
                if (src is null || dst is null) throw new InvalidOperationException("unknown_user");
                if (H.Str(src["status"]) is "erased" or "merged" || H.Str(dst["status"]) is "erased" or "merged"
                    || H.Str(src["role"]) != "student" || H.Str(dst["role"]) != "student")
                    throw new InvalidOperationException("not_mergeable");

                var loserEmail = H.Str(src["email"]) ?? "";
                var loserNumber = H.Str(src["registration_no"]);
                var survivorNumber = H.Str(dst["registration_no"]);

                var before = Snapshot(db, src, dst);

                // Outstanding cross-surface handoff codes die for BOTH accounts — a code minted
                // before the merge must not authenticate anyone into either identity afterwards.
                // Done before the world links are re-pointed, while the loser's links still resolve.
                if (world && Has(db, "pciworld_handoff_codes"))
                    db.Execute(@"DELETE FROM pciworld_handoff_codes WHERE world_user_id IN (
                            SELECT w.id FROM pciworld_users w WHERE w.student_user_id IN (?,?)
                            UNION SELECT m2.legacy_world_id FROM pciworld_user_map m2 WHERE m2.canonical_user_id IN (?,?))",
                        sourceId, targetId, sourceId, targetId);

                // The loser's Student Number: permanently 'merged' in the registry, resolving to the
                // survivor. The registry row is the durable record — support can follow it — while
                // public verification already answers for merged numbers via the successor.
                if (!string.IsNullOrEmpty(loserNumber))
                {
                    var changed = db.Execute(@"UPDATE pci_student_number_registry
                        SET state='merged', merged_into_student_number=?, resolves_to_user_id=?,
                            changed_at=datetime('now'), reason_code='duplicate_merge',
                            changed_by_admin_id=?, correlation_id=?, row_version=row_version+1
                        WHERE student_number=?", survivorNumber, targetId, approverId, corr, loserNumber);
                    // A number issued before the registry existed has no row — create its permanent
                    // record now rather than leaving the value unclaimed and reissuable.
                    if (changed == 0)
                        db.Execute(@"INSERT INTO pci_student_number_registry(student_number,format_version,
                                original_user_id,resolves_to_user_id,state,merged_into_student_number,
                                reason_code,changed_by_admin_id,correlation_id)
                            VALUES(?,?,?,?,'merged',?,'duplicate_merge',?,?)",
                            loserNumber, StudentNumbers.FormatVersion, sourceId, targetId,
                            survivorNumber, approverId, corr);
                }

                // Blanket re-points: nothing in these tables is unique per user.
                foreach (var t in BlanketTables)
                    db.Execute($"UPDATE {t} SET user_id=? WHERE user_id=?", targetId, sourceId);

                // cpd_entries: UNIQUE(source_event_id,user_id). Manual entries (NULL source) move
                // wholesale; event-credited entries move only where the survivor was NOT credited
                // for the same event — the collision keeps the survivor's credit (exactly-once per
                // person per event is the invariant) and marks the loser's row instead.
                db.Execute("UPDATE cpd_entries SET user_id=? WHERE user_id=? AND source_event_id IS NULL",
                    targetId, sourceId);
                var survivorEvents = db.Query(
                        "SELECT source_event_id AS e FROM cpd_entries WHERE user_id=? AND source_event_id IS NOT NULL", targetId)
                    .Select(r => H.L(r["e"])).ToHashSet();
                foreach (var r in db.Query(
                    "SELECT id,source_event_id FROM cpd_entries WHERE user_id=? AND source_event_id IS NOT NULL", sourceId))
                {
                    var ev = H.L(r["source_event_id"]);
                    if (survivorEvents.Add(ev))
                        db.Execute("UPDATE cpd_entries SET user_id=? WHERE id=?", targetId, r["id"]);
                    else
                        db.Execute(@"UPDATE cpd_entries SET admin_note=?, status='merged_duplicate' WHERE id=?",
                            $"duplicate of survivor's credit for event #{ev}; merge {corr}", r["id"]);
                }

                // event_registrations: UNIQUE(event_id,user_id) — same shape, same resolution. A
                // 'merged_duplicate' status also keeps the leftover row out of capacity and
                // attendance counts, which only look at 'registered'/'attended'.
                if (Has(db, "event_registrations"))
                {
                    var survivorRegs = db.Query(
                            "SELECT event_id AS e FROM event_registrations WHERE user_id=?", targetId)
                        .Select(r => H.L(r["e"])).ToHashSet();
                    foreach (var r in db.Query("SELECT id,event_id FROM event_registrations WHERE user_id=?", sourceId))
                    {
                        if (survivorRegs.Add(H.L(r["event_id"])))
                            db.Execute("UPDATE event_registrations SET user_id=? WHERE id=?", targetId, r["id"]);
                        else
                            db.Execute("UPDATE event_registrations SET status='merged_duplicate' WHERE id=?", r["id"]);
                    }
                }

                // student_profiles: one per person. Where both have one the survivor's stands and
                // the loser's is deleted; where only the loser has one, it becomes the survivor's.
                if (db.QueryOne("SELECT id FROM student_profiles WHERE user_id=?", targetId) is not null)
                    db.Execute("DELETE FROM student_profiles WHERE user_id=?", sourceId);
                else
                    db.Execute("UPDATE student_profiles SET user_id=? WHERE user_id=?", targetId, sourceId);

                if (world)
                {
                    // World evidence follows the person: the canonical ownership stamp moves. The
                    // legacy world-namespace user_id column is deliberately untouched.
                    if (Has(db, "pciworld_attempts"))
                        db.Execute("UPDATE pciworld_attempts SET canonical_user_id=? WHERE canonical_user_id=?",
                            targetId, sourceId);
                    // Participation row: UNIQUE(user_id) — survivor's kept, loser's removed.
                    if (Has(db, "pciworld_participants"))
                    {
                        if (db.QueryOne("SELECT id FROM pciworld_participants WHERE user_id=?", targetId) is not null)
                            db.Execute("DELETE FROM pciworld_participants WHERE user_id=?", sourceId);
                        else
                            db.Execute("UPDATE pciworld_participants SET user_id=? WHERE user_id=?", targetId, sourceId);
                    }
                    // World credential links re-resolve to the survivor, so a World login lands on
                    // the surviving identity, never on the inert merged shell.
                    db.Execute("UPDATE pciworld_users SET student_user_id=? WHERE student_user_id=?", targetId, sourceId);
                    db.Execute("UPDATE pciworld_user_map SET canonical_user_id=? WHERE canonical_user_id=?", targetId, sourceId);
                }

                // The loser's users row becomes an inert, auditable shell: status 'merged', the
                // email prefixed so the real address frees up for the survivor (or anyone) to use,
                // and the number projection cleared — the registry row is the permanent record.
                db.Execute(@"UPDATE users SET status='merged', email=?, registration_no=NULL,
                        updated_at=datetime('now') WHERE id=?",
                    $"merged-{sourceId}-{loserEmail}", sourceId);

                // Revoke BOTH accounts' portal sessions and outstanding links: every live token was
                // minted against a pre-merge identity, and the survivor's should re-establish too.
                db.Execute("DELETE FROM login_tokens WHERE user_id IN (?,?)", sourceId, targetId);

                var after = Snapshot(db,
                    db.QueryOne("SELECT id,email,registration_no,status,role FROM users WHERE id=?", sourceId)!,
                    db.QueryOne("SELECT id,email,registration_no,status,role FROM users WHERE id=?", targetId)!);
                db.Execute("UPDATE pci_identity_merges SET before_json=?, after_json=? WHERE id=?",
                    System.Text.Json.JsonSerializer.Serialize(before),
                    System.Text.Json.JsonSerializer.Serialize(after), mergeId);

                summary = new Dictionary<string, object?>
                {
                    ["merge_id"] = mergeId,
                    ["source_user_id"] = sourceId,
                    ["target_user_id"] = targetId,
                    ["survivor_student_number"] = survivorNumber,
                    ["merged_student_number"] = loserNumber,
                    ["correlation_id"] = corr,
                };
            });
        }
        catch (InvalidOperationException ex) when (ex.Message is "already_decided" or "unknown_user" or "not_mergeable")
        {
            failure = ex.Message;
        }
        return failure is not null ? (failure, null) : (null, summary);
    }

    /// <summary>Reject a pending request — terminal, recorded, and idempotently guarded the same
    /// way as approve. A maker MAY reject (withdraw) their own request: refusing to merge is the
    /// safe direction and needs no second person. Errors: not_found | already_decided.</summary>
    public static string? Reject(Db db, long mergeId, long adminId, string? note)
    {
        if (db.QueryOne("SELECT id FROM pci_identity_merges WHERE id=?", mergeId) is null) return "not_found";
        var changed = db.Execute(@"UPDATE pci_identity_merges
            SET status='rejected', decided_by=?, decided_at=datetime('now'), decision_note=?,
                row_version=row_version+1
            WHERE id=? AND status='pending'", adminId, note, mergeId);
        return changed == 0 ? "already_decided" : null;
    }

    /// <summary>Key fields + per-table counts for both sides — the before/after audit shape.</summary>
    static Dictionary<string, object?> Snapshot(Db db, Dictionary<string, object?> src, Dictionary<string, object?> dst) => new()
    {
        ["source"] = new Dictionary<string, object?>
        {
            ["user_id"] = H.L(src["id"]),
            ["email"] = H.Str(src["email"]),
            ["registration_no"] = H.Str(src["registration_no"]),
            ["status"] = H.Str(src["status"]),
            ["counts"] = Counts(db, H.L(src["id"])),
        },
        ["target"] = new Dictionary<string, object?>
        {
            ["user_id"] = H.L(dst["id"]),
            ["email"] = H.Str(dst["email"]),
            ["registration_no"] = H.Str(dst["registration_no"]),
            ["status"] = H.Str(dst["status"]),
            ["counts"] = Counts(db, H.L(dst["id"])),
        },
    };
}
