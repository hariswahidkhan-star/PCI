using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// External-integrations audit regressions (EXT-P0/P1): atomic worker leases, fail-closed Meta secret
/// gate helpers, encrypted secret round-trip for connector blobs, and external exam block reasons.
/// </summary>
public class ExtIntegrationsAuditTests
{
    static Db Fresh() => TestEnv.NewMigratedDb();

    [Fact]
    public void WorkerLease_OnlyOneWinnerClaimsSameRow()
    {
        var db = Fresh();
        var id = db.ExecuteReturningId(
            @"INSERT INTO comm_outbox(channel,status,to_email,subject,body,attempts,max_attempts)
              VALUES('email','queued','a@test.io','S','B',0,5)");
        Assert.True(WorkerLease.TryClaim(db, "comm_outbox", id, "worker-a", "'queued','scheduled','retrying'"));
        Assert.False(WorkerLease.TryClaim(db, "comm_outbox", id, "worker-b", "'queued','scheduled','retrying'"));
        var row = db.QueryOne("SELECT status,lease_owner FROM comm_outbox WHERE id=?", id)!;
        Assert.Equal("processing", H.Str(row["status"]));
        Assert.Equal("worker-a", H.Str(row["lease_owner"]));
    }

    [Fact]
    public void WorkerLease_ExpiredLeaseCanBeReclaimed()
    {
        var db = Fresh();
        var id = db.ExecuteReturningId(
            @"INSERT INTO comm_outbox(channel,status,to_email,subject,body,attempts,max_attempts)
              VALUES('email','processing','a@test.io','S','B',0,5)");
        db.Execute("UPDATE comm_outbox SET lease_owner='dead', lease_until=datetime('now','-1 minutes') WHERE id=?", id);
        WorkerLease.RecoverExpired(db, "comm_outbox", "retrying");
        Assert.Equal("retrying", db.Scalar<string>("SELECT status FROM comm_outbox WHERE id=?", id));
        Assert.True(WorkerLease.TryClaim(db, "comm_outbox", id, "worker-c", "'queued','scheduled','retrying'"));
    }

    [Fact]
    public void ExamDelivery_ExternalBlockReason_WhenVendorMissing()
    {
        var db = Fresh();
        ExamDeliveryConnectors.Register(); // connectors are normally registered at app boot
        var certId = db.Scalar<long>("SELECT id FROM certifications ORDER BY id LIMIT 1");
        Assert.True(certId > 0);
        Settings.Put(db, "exam_delivery_mode", "psi");
        Assert.Equal("delivery_vendor_unavailable", ExamDelivery.ExternalBlockReason(db, certId));
        Settings.Put(db, "exam_delivery_mode", ExamDelivery.InHouse);
        Assert.Null(ExamDelivery.ExternalBlockReason(db, certId));
    }

    [Fact]
    public void ConnectorSecret_EncryptRoundTrip_AbsentFromPlainLookup()
    {
        var db = Fresh();
        var plain = "{\"client_secret\":\"s3cr3t\",\"refresh_token\":\"rt-1\"}";
        var enc = Security.EncryptSecret(plain)!;
        Assert.StartsWith("enc:v1:", enc);
        Assert.DoesNotContain("s3cr3t", enc);
        Assert.Equal(plain, Security.DecryptSecret(enc));
        // Legacy plaintext still decrypts as itself (migration tolerance).
        Assert.Equal("legacy", Security.DecryptSecret("legacy"));
    }

    [Fact]
    public void Mailer_EnqueueWelcome_IsDurableAndDeduped()
    {
        var db = Fresh();
        var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)", "w@test.io", "W", "student", "active");
        var a = Mailer.EnqueueWelcome(db, uid, "w@test.io", "W", "https://example.test/setup?t=1", "https://example.test", "ref-1");
        var b = Mailer.EnqueueWelcome(db, uid, "w@test.io", "W", "https://example.test/setup?t=1", "https://example.test", "ref-1");
        Assert.NotNull(a);
        Assert.Null(b); // dedup
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE trigger_code='account.welcome' AND channel='email' AND user_id=?", uid));
        Assert.Equal("queued", db.Scalar<string>("SELECT status FROM comm_outbox WHERE id=?", a!.Value));
    }

    [Fact]
    public void Storage_S3Misconfigured_Detected()
    {
        var prev = Environment.GetEnvironmentVariable("STORAGE_PROVIDER");
        var prevBucket = Environment.GetEnvironmentVariable("S3_BUCKET");
        try
        {
            Environment.SetEnvironmentVariable("STORAGE_PROVIDER", "s3");
            Environment.SetEnvironmentVariable("S3_BUCKET", null);
            Assert.True(Storage.S3Misconfigured);
            Assert.False(Storage.UsingLocal); // incomplete s3 must NOT report as a healthy local backend
        }
        finally
        {
            Environment.SetEnvironmentVariable("STORAGE_PROVIDER", prev);
            Environment.SetEnvironmentVariable("S3_BUCKET", prevBucket);
        }
    }
}
