using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The one and only issuer of the canonical PCI Student Number.
///
/// A person has exactly one number, for life, whether their account was first created through
/// PCI Global/MyPCI, PCI World, an admin, a payment, an honorary conversion or a partner import.
/// Every one of those paths must call <see cref="GetOrIssue"/> inside the transaction that creates
/// the canonical <c>users</c> row — nothing else may write <c>users.registration_no</c>.
///
/// Format (legacy_v1, the currently deployed shape — preserved exactly so every historic number
/// stays valid):  PCI-{account-created UTC year}-{canonical users.id padded to at least 6 digits}
///
/// Two properties matter more than the format:
///
///   • The year comes from the canonical account's own created_at, not from "now". The lazy writer
///     this service replaces used DateTime.UtcNow at the moment of an /api/me read, so an account
///     created in 2025 whose owner first signed in during 2026 was stamped PCI-2026-… . That is a
///     wrong number, not a cosmetic one, and it is why issuance belongs in the creating transaction.
///
///   • The number is reserved in pci_student_number_registry before it is projected onto the user.
///     The registry row is the permanent claim: after a merge, retirement or erasure the identity
///     goes away but the reservation stays, so the number can never be handed to a different person.
/// </summary>
public static class StudentNumbers
{
    public const string FormatVersion = "legacy_v1";

    /// <summary>The legacy_v1 value for a canonical user. Pure — no database access, no clock —
    /// so the format can be asserted directly in tests and reused by the Phase 2 backfill.</summary>
    public static string Format(long userId, string? createdAtUtc)
    {
        // created_at is the repo-wide 'YYYY-MM-DD HH:MM:SS' UTC string. Take the year textually:
        // parsing to DateTime and back risks a local-time conversion moving a 1 January account
        // into the previous year. A row with no usable created_at (only possible for a hand-built
        // fixture) falls back to the current UTC year rather than producing 'PCI--000123'.
        var year = createdAtUtc is { Length: >= 4 } s && s[..4].All(char.IsAsciiDigit)
            ? s[..4]
            : DateTime.UtcNow.Year.ToString("D4");
        return $"PCI-{year}-{userId:D6}";
    }

    /// <summary>
    /// Returns the canonical user's Student Number, issuing and reserving it if they have none.
    ///
    /// Idempotent: a user who already has a number gets it back unchanged, so a retried signup, a
    /// double-clicked button or a replayed request all resolve to the same value. Callers are
    /// expected to be inside <see cref="Db.Transaction"/> already — this method deliberately does
    /// not open its own, so issuance commits atomically with the account and profile it belongs to.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The number could not be issued. Thrown, not swallowed: a registration that cannot produce an
    /// identity must fail loudly and roll back rather than report success on a half-built account.
    /// </exception>
    public static string GetOrIssue(Db db, long userId, string reasonCode, string? correlationId = null)
    {
        var row = db.QueryOne("SELECT registration_no,created_at FROM users WHERE id=?", userId)
            ?? throw new InvalidOperationException($"cannot issue a Student Number for unknown user #{userId}");

        // Already issued — the common case on every path after the first. Never re-derive, never
        // overwrite: the stored string is authoritative even if it predates the current format.
        if (H.Str(row["registration_no"]) is { Length: > 0 } existing) return existing;

        var number = Format(userId, H.Str(row["created_at"]));

        // Reserve first. The unique index on student_number makes this the atomic claim, so two
        // concurrent callers racing for the same user cannot both believe they issued it.
        try
        {
            db.Execute(@"INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,
                    resolves_to_user_id,state,reason_code,correlation_id)
                VALUES(?,?,?,?, 'issued',?,?)",
                number, FormatVersion, userId, userId, reasonCode, correlationId);
        }
        catch (Exception ex)
        {
            // The only conflict we tolerate is this user's own reservation already existing — a
            // partially applied earlier attempt, or the losing side of a race that has since
            // committed. Anything else means the number belongs to somebody else, and issuing it
            // would be a collision: fail the transaction instead.
            var held = db.QueryOne("SELECT resolves_to_user_id,state FROM pci_student_number_registry WHERE student_number=?", number);
            if (held is null || H.L(held["resolves_to_user_id"]) != userId)
                throw new InvalidOperationException(
                    $"Student Number {number} is already reserved for another identity — refusing to reissue it", ex);
        }

        // Project onto the user. The guard makes the write a no-op if a concurrent transaction set a
        // number first, and the re-read below returns whichever value actually won.
        db.Execute(@"UPDATE users SET registration_no=?, registration_no_issued_at=datetime('now')
            WHERE id=? AND (registration_no IS NULL OR registration_no='')", number, userId);

        var settled = db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", userId);
        if (string.IsNullOrWhiteSpace(settled))
            throw new InvalidOperationException($"Student Number issuance for user #{userId} did not persist");
        return settled;
    }

    /// <summary>
    /// The number for a user who is expected to already have one — a read-side helper for the many
    /// surfaces that display it. Returns null rather than issuing, because a read endpoint must
    /// never mutate identity data (the defect this service exists to remove from GET /api/me).
    /// </summary>
    public static string? Read(Db db, long userId) =>
        db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", userId) is { Length: > 0 } n ? n : null;
}
