using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — moderation cases, sanctions and appeals (spec §8.6/§8.7;
/// CCP_PHASE1_DESIGN §2.4/§3).
///
/// FOUR RULES THIS FILE EXISTS TO ENFORCE, each written because the obvious implementation gets it
/// wrong:
///
///   1. **History is append-only.** Overturning an appeal does NOT edit the decision that imposed
///      the sanction. It revokes the sanction and writes a NEW decision recording the reversal, so
///      "what did we decide, and when did we change our mind?" stays answerable. A system that
///      rewrites its own record cannot be audited (§8.7).
///
///   2. **A permanent sanction needs two people.** The issuer and the approver must differ. Without
///      this, one compromised or mistaken account can permanently exclude someone with no check.
///
///   3. **A public case reference reveals nothing on its own.** It is safe to quote in an email.
///      Retrieving status needs a SEPARATE high-entropy credential, shown once, stored only as a
///      hash, expiring and rate-limited — so a guest can appeal without an account (§8.6) while a
///      reference leaked in a screenshot grants nobody anything.
///
///   4. **Restricted cases stay out of the ordinary queue.** Suspected child-safety material must
///      not appear in a routine moderator's list at all (§8.7, §28.7). The queue query excludes
///      them structurally rather than relying on a UI filter.
/// </summary>
public static class CommunityCases
{
    // ── Cases ─────────────────────────────────────────────────────────────────────────────────

    /// <summary>Open a case, or return the open one that already covers this subject. Idempotent by
    /// subject so a burst of reports about one participant produces one case to work, not twenty.</summary>
    public static long OpenOrGet(Db db, long? roomId, string subjectKind, string subjectRef,
                                 string severity, string reasonCode, bool restricted = false,
                                 string? correlationId = null)
    {
        var existing = db.QueryOne(
            @"SELECT id FROM pciworld_moderation_cases
              WHERE subject_kind=? AND subject_ref=? AND status IN ('open','in_review')
              ORDER BY id DESC", subjectKind, subjectRef);
        if (existing is not null) return H.L(existing["id"]);

        var id = db.ExecuteReturningId(
            @"INSERT INTO pciworld_moderation_cases
                  (room_id,subject_kind,subject_ref,severity,status,restricted,reason_code,correlation_id)
              VALUES(?,?,?,?,'open',?,?,?)",
            roomId, subjectKind, subjectRef, severity, restricted ? 1 : 0, reasonCode, correlationId);
        Event(db, id, null, "system", "case_opened", reasonCode, null, "open", correlationId);
        return id;
    }

    /// <summary>Append to a case's immutable trail. Every state change goes through here so the
    /// history is complete by construction rather than by discipline.</summary>
    public static void Event(Db db, long caseId, long? actorId, string actorKind, string eventName,
                             string? detail, string? previousState, string? newState, string? correlationId = null)
        => db.Execute(
            @"INSERT INTO pciworld_moderation_case_events
                  (case_id,actor_id,actor_kind,event,detail,previous_state,new_state,correlation_id)
              VALUES(?,?,?,?,?,?,?,?)",
            caseId, actorId, actorKind, eventName, detail, previousState, newState, correlationId);

    /// <summary>
    /// The ordinary moderation queue: severity first, then age, so the worst and the most-neglected
    /// surface together.
    ///
    /// Restricted cases are EXCLUDED here and reachable only through <see cref="RestrictedQueue"/>,
    /// which requires a separate permission. That is a structural exclusion, not a UI filter — a
    /// routine moderator cannot reach the evidence by changing a query parameter.
    /// </summary>
    public static List<Dictionary<string, object?>> Queue(Db db, string? status = null, int limit = 100)
        => db.Query(
            @"SELECT c.*, (SELECT COUNT(*) FROM pciworld_community_reports r WHERE r.case_id=c.id) AS report_count
              FROM pciworld_moderation_cases c
              WHERE c.restricted=0 AND (? IS NULL OR c.status=?)
              ORDER BY CASE c.severity WHEN 'critical' THEN 0 WHEN 'high' THEN 1 WHEN 'normal' THEN 2 ELSE 3 END,
                       c.created_at
              LIMIT ?", status, status, Math.Clamp(limit, 1, 500));

    /// <summary>Restricted cases — child-safety and legal escalations. Gated on
    /// <c>community.restricted</c>, never merged into the ordinary queue.</summary>
    public static List<Dictionary<string, object?>> RestrictedQueue(Db db, int limit = 100)
        => db.Query(
            @"SELECT * FROM pciworld_moderation_cases
              WHERE restricted=1 AND status IN ('open','in_review','escalated')
              ORDER BY created_at LIMIT ?", Math.Clamp(limit, 1, 500));

    public static bool Assign(Db db, long caseId, long adminId, string? correlationId = null)
    {
        var changed = db.Execute(
            "UPDATE pciworld_moderation_cases SET assigned_to=?, status='in_review', updated_at=datetime('now') WHERE id=? AND status='open'",
            adminId, caseId);
        if (changed == 1) Event(db, caseId, adminId, "world_admin", "assigned", null, "open", "in_review", correlationId);
        return changed == 1;
    }

    public static bool Close(Db db, long caseId, long adminId, string outcome, string reason, string? correlationId = null)
    {
        var previous = H.Str(db.QueryOne("SELECT status FROM pciworld_moderation_cases WHERE id=?", caseId)?["status"]);
        var changed = db.Execute(
            @"UPDATE pciworld_moderation_cases SET status=?, closed_at=datetime('now'), updated_at=datetime('now')
              WHERE id=? AND status IN ('open','in_review')", outcome, caseId);
        if (changed == 1) Event(db, caseId, adminId, "world_admin", "case_closed", reason, previous, outcome, correlationId);
        return changed == 1;
    }

    // ── Sanctions ─────────────────────────────────────────────────────────────────────────────

    /// <summary>Sanction types whose effect is permanent, and which therefore require a second
    /// authorised approver before they bite.</summary>
    public static bool RequiresApproval(string sanctionType) => sanctionType is "permanent";

    public readonly record struct SanctionResult(bool Ok, string? ErrorCode, long SanctionId)
    {
        public static SanctionResult Fail(string code) => new(false, code, 0);
    }

    /// <summary>
    /// Issue a sanction. A permanent one is created PENDING — it carries no effect until a
    /// different admin approves it (§7.4 maker-checker). Everything else takes effect immediately,
    /// because a live room cannot wait for a second reviewer to eject someone abusive.
    /// </summary>
    public static SanctionResult Issue(Db db, string subjectKind, string subjectRef, long? roomId,
                                       string sanctionType, string reasonCode, long issuedBy,
                                       long? caseId, int? durationMinutes, long? policyVersionId = null,
                                       string scope = "room", string? correlationId = null)
    {
        if (string.IsNullOrWhiteSpace(reasonCode)) return SanctionResult.Fail("reason_required");

        // Computed here and bound as a value: Db.Translate's rewrite is textual, so a modifier
        // arriving through a parameter reaches MySQL as SQLite syntax it cannot evaluate.
        var expiresAt = durationMinutes is { } m and > 0 ? H.StampInMinutes(m) : null;
        if (RequiresApproval(sanctionType) && expiresAt is not null)
            return SanctionResult.Fail("permanent_cannot_expire");

        var id = db.ExecuteReturningId(
            @"INSERT INTO pciworld_sanctions
                  (subject_kind,subject_ref,room_id,sanction_type,reason_code,policy_version_id,scope,
                   issued_by,issued_by_kind,approved_by,expires_at,case_id,correlation_id)
              VALUES(?,?,?,?,?,?,?,?,'world_admin',NULL,?,?,?)",
            subjectKind, subjectRef, roomId, sanctionType, reasonCode, policyVersionId, scope,
            issuedBy, expiresAt, caseId, correlationId);

        if (caseId is { } cid)
            Event(db, cid, issuedBy, "world_admin", "sanction_issued", $"{sanctionType}:{reasonCode}",
                  null, RequiresApproval(sanctionType) ? "pending_approval" : "active", correlationId);
        return new SanctionResult(true, null, id);
    }

    /// <summary>
    /// The checker half of maker-checker. Refuses when the approver is the issuer — the whole point
    /// of the control — and refuses to re-approve, so an already-approved sanction cannot be
    /// laundered through a second signature.
    /// </summary>
    public static SanctionResult Approve(Db db, long sanctionId, long approverId, string? correlationId = null)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_sanctions WHERE id=?", sanctionId);
        if (row is null) return SanctionResult.Fail("not_found");
        if (row["approved_by"] is not null) return SanctionResult.Fail("already_approved");
        if (H.L(row["issued_by"]) == approverId) return SanctionResult.Fail("maker_checker");
        if (row["revoked_at"] is not null) return SanctionResult.Fail("revoked");

        db.Execute("UPDATE pciworld_sanctions SET approved_by=? WHERE id=?", approverId, sanctionId);
        if (row["case_id"] is not null)
            Event(db, H.L(row["case_id"]), approverId, "world_admin", "sanction_approved",
                  H.Str(row["sanction_type"]), "pending_approval", "active", correlationId);
        return new SanctionResult(true, null, sanctionId);
    }

    /// <summary>Whether a sanction is actually biting right now: not revoked, not expired, and — if
    /// it is the kind that needs a second signature — actually approved. A pending permanent
    /// sanction has NO effect, which is what makes maker-checker real rather than advisory.</summary>
    public static bool IsInEffect(Dictionary<string, object?> sanction)
    {
        if (sanction["revoked_at"] is not null) return false;
        if (RequiresApproval(H.Str(sanction["sanction_type"]) ?? "") && sanction["approved_by"] is null) return false;
        var expires = H.Str(sanction["expires_at"]);
        return string.IsNullOrEmpty(expires) || !H.IsPast(expires);
    }

    public static List<Dictionary<string, object?>> ActiveFor(Db db, string subjectKind, string subjectRef)
        => db.Query(
            @"SELECT * FROM pciworld_sanctions
              WHERE subject_kind=? AND subject_ref=? AND revoked_at IS NULL
                AND (expires_at IS NULL OR expires_at > datetime('now'))",
            subjectKind, subjectRef).Where(IsInEffect).ToList();

    public static bool Revoke(Db db, long sanctionId, long adminId, string reason, string? correlationId = null)
    {
        var row = db.QueryOne("SELECT case_id FROM pciworld_sanctions WHERE id=? AND revoked_at IS NULL", sanctionId);
        if (row is null) return false;
        db.Execute("UPDATE pciworld_sanctions SET revoked_at=datetime('now'), revoked_by=? WHERE id=?", adminId, sanctionId);
        if (row["case_id"] is not null)
            Event(db, H.L(row["case_id"]), adminId, "world_admin", "sanction_revoked", reason, "active", "revoked", correlationId);
        return true;
    }

    // ── Appeals ───────────────────────────────────────────────────────────────────────────────

    /// <summary>What a guest is given when they are sanctioned. The reference is safe to quote; the
    /// credential is shown ONCE and never stored in the clear.</summary>
    public readonly record struct AppealTicket(string PublicReference, string Credential);

    /// <summary>
    /// Mint an appeal ticket for a case. The public reference is deliberately short and
    /// non-sensitive — it identifies a case for support correspondence and grants nothing. The
    /// credential is 128-bit, stored only as SHA-256, and expires, so a guest can appeal without an
    /// account while a leaked reference remains useless.
    /// </summary>
    public static AppealTicket IssueAppeal(Db db, long caseId, int validDays = 30)
    {
        var reference = "PCIW-" + Security.RandomHex(4).ToUpperInvariant();
        var credential = Security.RandomHex(32);
        db.Execute(
            @"INSERT INTO pciworld_appeals(case_id,public_reference,credential_sha,credential_expires_at,status)
              VALUES(?,?,?,?,'issued')",
            caseId, reference, Security.Sha(credential), H.StampInMinutes(validDays * 24 * 60));
        Event(db, caseId, null, "system", "appeal_issued", reference, null, "issued");
        return new AppealTicket(reference, credential);
    }

    /// <summary>
    /// Resolve an appeal from its credential. Requires BOTH the public reference and the
    /// credential: the reference alone reveals nothing, and the credential is checked by hash.
    /// Attempts are counted so a credential cannot be brute-forced, and an exhausted or expired
    /// credential resolves to null exactly like a wrong one — no oracle.
    /// </summary>
    public static Dictionary<string, object?>? ByCredential(Db db, string? reference, string? credential)
    {
        if (string.IsNullOrWhiteSpace(reference) || string.IsNullOrWhiteSpace(credential)) return null;
        var row = db.QueryOne("SELECT * FROM pciworld_appeals WHERE public_reference=?", reference);
        if (row is null) return null;

        // Count the attempt before judging it, so a failed guess is recorded even if the caller
        // abandons the request.
        db.Execute("UPDATE pciworld_appeals SET attempts=attempts+1 WHERE id=?", H.L(row["id"]));
        if (H.L(row["attempts"]) >= 20) return null;

        var expires = H.Str(row["credential_expires_at"]);
        if (!string.IsNullOrEmpty(expires) && H.IsPast(expires)) return null;
        if (H.Str(row["credential_sha"]) != Security.Sha(credential)) return null;
        return row;
    }

    public static bool SubmitAppeal(Db db, long appealId, string submission)
    {
        if (string.IsNullOrWhiteSpace(submission) || submission.Length > 4000) return false;
        var changed = db.Execute(
            "UPDATE pciworld_appeals SET submission=?, status='submitted' WHERE id=? AND status IN ('issued','changes_requested')",
            submission, appealId);
        return changed == 1;
    }

    /// <summary>
    /// Decide an appeal.
    ///
    /// An OVERTURN revokes the sanction and writes a NEW decision recording the reversal. It never
    /// edits the original decision: the record of what was decided, and of the change of mind, must
    /// both survive (§8.7). Derived state — the sanction, the case — is corrected; history is added
    /// to, never rewritten.
    /// </summary>
    public static bool Decide(Db db, long appealId, long reviewerId, bool upheld, string outcome,
                              string? correlationId = null)
    {
        var appeal = db.QueryOne("SELECT * FROM pciworld_appeals WHERE id=? AND status IN ('submitted','under_review')", appealId);
        if (appeal is null) return false;
        var caseId = H.L(appeal["case_id"]);

        db.Transaction(() =>
        {
            db.Execute(
                @"UPDATE pciworld_appeals SET status=?, outcome=?, reviewed_by=?, reviewed_at=datetime('now')
                  WHERE id=?", upheld ? "upheld" : "overturned", outcome, reviewerId, appealId);

            if (!upheld)
            {
                // Revoke every sanction the case imposed, and record WHY in the sanction row itself
                // so a later reader does not have to reconstruct it from the case trail.
                foreach (var s in db.Query("SELECT id FROM pciworld_sanctions WHERE case_id=? AND revoked_at IS NULL", caseId))
                    db.Execute("UPDATE pciworld_sanctions SET revoked_at=datetime('now'), revoked_by=? WHERE id=?",
                               reviewerId, H.L(s["id"]));

                // Lift the risk restriction too, or the person stays locked out of the room by a
                // side-effect of a sanction that has just been overturned.
                db.Execute("UPDATE pciworld_risk_restrictions SET expires_at=datetime('now') WHERE case_id=?", caseId);

                // A NEW decision row, not an edit of the old one.
                db.Execute(
                    @"INSERT INTO pciworld_moderation_decisions
                          (scope,subject_ref,outcome,reason_code,category,confidence_band,correlation_id)
                      VALUES('appeal',?,'allow','appeal_overturned','review','high',?)",
                    caseId.ToString(), correlationId);
            }

            db.Execute("UPDATE pciworld_moderation_cases SET status=?, closed_at=datetime('now'), updated_at=datetime('now') WHERE id=?",
                       upheld ? "actioned" : "dismissed", caseId);
            Event(db, caseId, reviewerId, "world_admin", upheld ? "appeal_upheld" : "appeal_overturned",
                  outcome, "in_review", upheld ? "actioned" : "dismissed", correlationId);
        });
        return true;
    }

    /// <summary>The minimal status a guest may retrieve with their credential. Deliberately narrow:
    /// no evidence, no other participants, no classifier internals, no network data (§8.6).</summary>
    public static Dictionary<string, object?> PublicView(Dictionary<string, object?> appeal) => new()
    {
        ["reference"] = H.Str(appeal["public_reference"]),
        ["status"] = H.Str(appeal["status"]),
        ["outcome"] = H.Str(appeal["outcome"]),
        ["submitted"] = !string.IsNullOrEmpty(H.Str(appeal["submission"])),
        ["decided_at"] = H.Str(appeal["reviewed_at"]),
    };
}
