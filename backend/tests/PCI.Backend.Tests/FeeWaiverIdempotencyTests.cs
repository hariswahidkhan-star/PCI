using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// RES-026 — partial and reschedule-only fee-waiver ledger rows must carry a client idempotency key
/// and a schema-level uniqueness guard so retries cannot duplicate ledger/discount state.
/// </summary>
public class FeeWaiverIdempotencyTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    static long SeedUser(Db db, string email = "waiver-idem@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", email, "Waiver", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    [Fact]
    public void Schema_exposes_unique_idempotency_key_on_fee_waivers()
    {
        var db = Fresh();
        Assert.Contains(db.Columns("fee_waivers"), c => string.Equals(c, "idempotency_key", StringComparison.OrdinalIgnoreCase));
        // Two rows with the same key must be rejected by the unique index.
        var uid = SeedUser(db);
        var (id1, created1) = FeeWaiverLedger.TryInsert(db, "idem-schema-1", uid, "exam", 1, "partial",
            "exam", "partial", 100, 50, 50, 50, "reason", null, 1);
        Assert.True(created1);
        Assert.True(id1 > 0);
        var (id2, created2) = FeeWaiverLedger.TryInsert(db, "idem-schema-1", uid, "exam", 1, "partial",
            "exam", "partial", 100, 50, 50, 50, "reason", null, 1);
        Assert.False(created2);
        Assert.Equal(id1, id2);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM fee_waivers WHERE idempotency_key=?", "idem-schema-1"));
    }

    [Fact]
    public void Partial_waiver_replay_returns_same_row_without_second_insert()
    {
        var db = Fresh();
        var uid = SeedUser(db, "partial-replay@test.io");
        var (id1, c1) = FeeWaiverLedger.TryInsert(db, "partial-key-a", uid, "membership", 1, "partial",
            "membership", "partial", 200, 100, 100, 100, "sponsorship", "n1", 9);
        Assert.True(c1);
        var (id2, c2) = FeeWaiverLedger.TryInsert(db, "partial-key-a", uid, "membership", 1, "partial",
            "membership", "partial", 200, 100, 100, 100, "sponsorship", "n1", 9);
        Assert.False(c2);
        Assert.Equal(id1, id2);
        var replay = FeeWaiverLedger.ReplayResponse(db, FeeWaiverLedger.Find(db, "partial-key-a")!);
        var json = System.Text.Json.JsonSerializer.Serialize(replay);
        Assert.Contains("\"replayed\":true", json);
        Assert.Contains("\"kind\":\"partial\"", json);
    }

    [Fact]
    public void Reschedule_only_waiver_is_idempotent_under_same_key()
    {
        var db = Fresh();
        var uid = SeedUser(db, "resched-replay@test.io");
        var key = "resched-key-b";
        var (id1, c1) = FeeWaiverLedger.TryInsert(db, key, uid, "exam", 1, "full",
            "reschedule", "full", 80, 80, 0, 0, "weather", null, 2);
        Assert.True(c1);
        // Concurrent-style second insert under the same key must not create another ledger line.
        var (id2, c2) = FeeWaiverLedger.TryInsert(db, key, uid, "exam", 1, "full",
            "reschedule", "full", 80, 80, 0, 0, "weather", null, 2);
        Assert.False(c2);
        Assert.Equal(id1, id2);
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM fee_waivers WHERE fee_type='reschedule' AND user_id=?", uid));
    }

    [Fact]
    public void Null_idempotency_keys_still_allow_distinct_rows()
    {
        var db = Fresh();
        var uid = SeedUser(db, "null-keys@test.io");
        var (a, ca) = FeeWaiverLedger.TryInsert(db, null, uid, "exam", 1, "full",
            "exam", "full", 100, 100, 0, 0, "comp-a", null, 1);
        var (b, cb) = FeeWaiverLedger.TryInsert(db, null, uid, "exam", 1, "full",
            "exam", "full", 100, 100, 0, 0, "comp-b", null, 1);
        Assert.True(ca);
        Assert.True(cb);
        Assert.NotEqual(a, b);
    }

    [Fact]
    public void NormalizeKey_trims_and_bounds_length()
    {
        Assert.Null(FeeWaiverLedger.NormalizeKey("   "));
        Assert.Equal("abc", FeeWaiverLedger.NormalizeKey("  abc  "));
        var longKey = new string('k', FeeWaiverLedger.MaxKeyLength + 40);
        Assert.Equal(FeeWaiverLedger.MaxKeyLength, FeeWaiverLedger.NormalizeKey(longKey)!.Length);
    }
}
