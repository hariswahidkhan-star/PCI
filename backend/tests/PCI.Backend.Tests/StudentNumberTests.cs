using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The canonical PCI Student Number — one per person, for life, whichever product created them.
///
/// These assertions guard the two defects that made the number unreliable before Core/StudentNumbers.cs
/// existed: it was minted lazily by a *read* endpoint (GET /api/me), and it was stamped with the year
/// of that read rather than the year the account was created. Both produced wrong-but-plausible
/// numbers that nothing downstream could tell apart from correct ones.
/// </summary>
public class StudentNumberTests
{
    static long NewUser(Db db, string email, string? createdAt = null)
    {
        var id = db.ExecuteReturningId(
            "INSERT INTO users(email,first_name,last_name,role,status) VALUES(?, 'S','N','student','active')", email);
        if (createdAt is not null) db.Execute("UPDATE users SET created_at=? WHERE id=?", createdAt, id);
        return id;
    }

    [Fact]
    public void Format_uses_the_account_year_and_pads_to_six_digits()
    {
        Assert.Equal("PCI-2026-000123", StudentNumbers.Format(123, "2026-07-28 10:00:00"));
        // Past six digits the id is not truncated — a seven-digit member keeps a unique number.
        Assert.Equal("PCI-2026-1234567", StudentNumbers.Format(1234567, "2026-01-01 00:00:00"));
    }

    [Fact]
    public void Issuance_takes_the_year_from_the_account_not_from_the_clock()
    {
        var db = TestEnv.NewMigratedDb();
        var uid = NewUser(db, "backdated@x.test", "2024-03-04 09:00:00");

        var number = StudentNumbers.GetOrIssue(db, uid, "test");

        // The lazy writer this replaced would have produced PCI-{this year}-… here.
        Assert.StartsWith("PCI-2024-", number);
        Assert.Equal(number, db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", uid));
        Assert.False(string.IsNullOrWhiteSpace(
            db.Scalar<string>("SELECT registration_no_issued_at FROM users WHERE id=?", uid)));
    }

    [Fact]
    public void Issuance_is_idempotent_so_a_retried_signup_never_mints_a_second_identity()
    {
        var db = TestEnv.NewMigratedDb();
        var uid = NewUser(db, "retry@x.test");

        var first = StudentNumbers.GetOrIssue(db, uid, "test");
        var second = StudentNumbers.GetOrIssue(db, uid, "test");

        Assert.Equal(first, second);
        Assert.Equal(1, db.Scalar<long>(
            "SELECT COUNT(*) FROM pci_student_number_registry WHERE resolves_to_user_id=?", uid));
    }

    [Fact]
    public void An_existing_number_is_authoritative_and_never_re_derived()
    {
        var db = TestEnv.NewMigratedDb();
        var uid = NewUser(db, "historic@x.test", "2019-01-01 00:00:00");
        db.Execute("UPDATE users SET registration_no='PCI-OLD-42' WHERE id=?", uid);

        // A historic value that predates the current format must survive untouched.
        Assert.Equal("PCI-OLD-42", StudentNumbers.GetOrIssue(db, uid, "test"));
    }

    [Fact]
    public void A_number_reserved_to_another_identity_is_refused_rather_than_reissued()
    {
        var db = TestEnv.NewMigratedDb();
        var uid = NewUser(db, "clash@x.test", "2026-01-01 00:00:00");
        var wanted = StudentNumbers.Format(uid, "2026-01-01 00:00:00");
        db.Execute(@"INSERT INTO pci_student_number_registry(student_number,format_version,original_user_id,
            resolves_to_user_id,state) VALUES(?, 'legacy_v1',9999,9999,'issued')", wanted);

        Assert.Throws<InvalidOperationException>(() => StudentNumbers.GetOrIssue(db, uid, "test"));
        // Critically: the user is left with NO number rather than somebody else's.
        Assert.True(string.IsNullOrEmpty(db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", uid)));
    }

    [Fact]
    public void Read_never_issues_so_a_read_endpoint_cannot_mutate_identity()
    {
        var db = TestEnv.NewMigratedDb();
        var uid = NewUser(db, "readonly@x.test");
        var before = db.Scalar<long>("SELECT COUNT(*) FROM pci_student_number_registry");

        Assert.Null(StudentNumbers.Read(db, uid));

        Assert.Equal(before, db.Scalar<long>("SELECT COUNT(*) FROM pci_student_number_registry"));
        Assert.True(string.IsNullOrEmpty(db.Scalar<string>("SELECT registration_no FROM users WHERE id=?", uid)));
    }

    [Fact]
    public void An_unknown_user_fails_loudly_rather_than_returning_a_number()
    {
        var db = TestEnv.NewMigratedDb();
        Assert.Throws<InvalidOperationException>(() => StudentNumbers.GetOrIssue(db, 987654, "test"));
    }
}
