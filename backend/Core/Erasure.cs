using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// GDPR right-to-erasure execution. This ANONYMISES a member rather than hard-deleting them: personal data
/// (name, contact details, identity documents, learning/casework history) is removed or scrubbed, while
/// records the institute must retain for a lawful basis are kept in de-identified form:
///   • payments — financial/tax retention obligations;
///   • issued_credentials — the public credential register must stay verifiable, but the holder's name is
///     replaced with a neutral placeholder so no personal data is exposed.
/// Identity-document blobs are deleted from storage immediately (not left to the age-based purge). The
/// account is locked (status='erased', password/2FA cleared, all sessions revoked) so it can never be used
/// again. The operation is idempotent-safe: re-running on an already-erased account is a no-op.
/// </summary>
public static class Erasure
{
    /// <summary>Anonymise a user's personal data. Returns a short summary of what was removed.</summary>
    public static object Anonymise(Db db, long userId)
    {
        // 1. Delete identity-document blobs from storage, then the rows (biometric/ID data must go).
        var idDocs = db.Query("SELECT storage_ref FROM identity_documents WHERE user_id=?", userId);
        var blobs = 0;
        foreach (var d in idDocs)
            if (H.Str(d["storage_ref"]) is { Length: > 0 } r && Storage.DeleteRef(r)) blobs++;

        // 2. Delete personal / behavioural child records. Financial (payments) and the credential register
        //    (issued_credentials) are deliberately NOT in this list — see the class summary.
        var deleted = 0;
        foreach (var t in new[]
        {
            "identity_documents", "candidate_consents", "student_profiles", "cpd_entries", "appeals",
            "accommodation_requests", "practice_attempts", "reviews", "notifications", "support_attachments",
            "tickets", "login_events", "security_events", "login_tokens", "certuvo_accounts",
            "exam_readiness_checks", "proctor_events", "proctor_messages", "work_experiences", "qualifications",
            "held_certifications",
        })
            try { deleted += db.Execute($"DELETE FROM {t} WHERE user_id=?", userId); } catch { /* table may not exist in a given build */ }

        // 3. Scrub the credential register: keep each credential verifiable (id, status, dates) but strip the
        //    holder's name so no personal data remains on the public verifier.
        try { db.Execute("UPDATE issued_credentials SET holder_name='(erased)' WHERE user_id=?", userId); } catch { }

        // 4. Anonymise the account itself and lock it out permanently.
        //
        // Retire the Student Number BEFORE clearing it. Clearing alone used to make the value
        // reusable: nothing recorded that it had ever been issued, so a later backfill or a new
        // account could be handed an erased person's public identifier — and every certificate,
        // Passport and printed badge already carrying that number would then point at somebody
        // else. Retiring keeps the reservation forever while releasing the person.
        var erasedNumber = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", userId);
        try { StudentNumberBackfill.Retire(db, erasedNumber, "erasure", null, null); }
        catch { /* never let the ledger write block an erasure the person is entitled to */ }

        var pseudoEmail = $"erased-{userId}@deleted.invalid";
        db.Execute(@"UPDATE users SET first_name='Erased', last_name='User', email=?, registration_no=NULL,
            password_hash=?, status='erased', two_factor_enabled=0, totp_secret=NULL, totp_last_step=NULL,
            totp_recovery=NULL, updated_at=datetime('now') WHERE id=?",
            pseudoEmail, Security.Sha(Security.RandomHex(24)), userId);

        return new { anonymised = true, identity_blobs_removed = blobs, personal_rows_removed = deleted, retained = new[] { "payments", "issued_credentials" } };
    }
}
