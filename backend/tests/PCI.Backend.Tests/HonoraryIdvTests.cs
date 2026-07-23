using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The timer-driven honorary-IDV retention sweep (RetentionService → HonoraryIdv.PurgeExpired), which
/// the live-HTTP integration suite cannot trigger: it only fires on a 24 h tick with an operator-set
/// retention window. Exercised here against a real migrated SQLite file + the real local Storage.
/// </summary>
public class HonoraryIdvPurgeTests : IClassFixture<DbFixture>
{
    readonly Db _db;
    public HonoraryIdvPurgeTests(DbFixture f) => _db = f.Db;

    long SeedApp(string reference, string idvStatus, string? submittedAgo)
    {
        var submitted = submittedAgo is null ? null : $"datetime('now','-{submittedAgo}')";
        return _db.ExecuteReturningId(
            "INSERT INTO honorary_applications(reference,first_name,email,status,idv_status,idv_submitted_at) " +
            $"VALUES(?,?,?,?,?,{submitted ?? "NULL"})",
            reference, "Test", "t@example.com", "pending_review", idvStatus);
    }

    string SeedDoc(long appId, string kind)
    {
        var obj = Storage.Put(StorageTests.PngBytes(), "image/png", "honorary-idv");
        _db.Execute("INSERT INTO honorary_idv_documents(application_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256) VALUES(?,?,?,?,?,?,?)",
            appId, kind, kind + ".png", obj.Mime, obj.SizeBytes, obj.Reference, obj.Sha256);
        return Path.Combine(TestEnv.StorageRoot, obj.Reference["local:".Length..]);
    }

    // the sweep walks every qualifying row, so each test pins the cohort it owns
    void NeutraliseOtherApps(string keepPrefix) =>
        _db.Execute("UPDATE honorary_applications SET idv_status='none' WHERE reference NOT LIKE ?", keepPrefix + "%");

    [Fact]
    public void PurgeExpired_DisabledWindowIsANoOp()
    {
        var id = SeedApp("UT-NOOP-1", "submitted", "90 day");
        var file = SeedDoc(id, "photo");
        Assert.Equal(0, HonoraryIdv.PurgeExpired(_db, 0));
        Assert.Equal(0, HonoraryIdv.PurgeExpired(_db, -7));
        Assert.Equal("submitted", H.Str(_db.QueryOne("SELECT idv_status FROM honorary_applications WHERE id=?", id)!["idv_status"]));
        Assert.True(File.Exists(file));
        // leave nothing behind for the sweep tests in this shared fixture
        _db.Execute("UPDATE honorary_applications SET idv_status='none' WHERE id=?", id);
    }

    [Fact]
    public void PurgeExpired_RemovesOnlyExpiredSubmissions()
    {
        NeutraliseOtherApps("UT-SWEEP-");
        var oldApp = SeedApp("UT-SWEEP-OLD", "submitted", "40 day");
        var oldPhoto = SeedDoc(oldApp, "photo");
        var oldGovId = SeedDoc(oldApp, "government_id");
        var recentApp = SeedApp("UT-SWEEP-RECENT", "submitted", "1 day");
        var recentPhoto = SeedDoc(recentApp, "photo");
        var invitedApp = SeedApp("UT-SWEEP-INVITED", "invited", null);       // never submitted

        Assert.Equal(2, HonoraryIdv.PurgeExpired(_db, 30));

        var purged = _db.QueryOne("SELECT idv_status,idv_deleted_at FROM honorary_applications WHERE id=?", oldApp)!;
        Assert.Equal("deleted", H.Str(purged["idv_status"]));
        Assert.NotNull(purged["idv_deleted_at"]);
        Assert.Equal(0L, _db.Scalar<long>("SELECT COUNT(*) FROM honorary_idv_documents WHERE application_id=?", oldApp));
        Assert.False(File.Exists(oldPhoto));
        Assert.False(File.Exists(oldGovId));

        // inside the window / never submitted → untouched
        Assert.Equal("submitted", H.Str(_db.QueryOne("SELECT idv_status FROM honorary_applications WHERE id=?", recentApp)!["idv_status"]));
        Assert.Equal(1L, _db.Scalar<long>("SELECT COUNT(*) FROM honorary_idv_documents WHERE application_id=?", recentApp));
        Assert.True(File.Exists(recentPhoto));
        Assert.Equal("invited", H.Str(_db.QueryOne("SELECT idv_status FROM honorary_applications WHERE id=?", invitedApp)!["idv_status"]));

        Assert.Equal(0, HonoraryIdv.PurgeExpired(_db, 30));   // idempotent — nothing left to purge
    }

    [Fact]
    public void PurgeExpired_IgnoresAlreadyDeletedRows()
    {
        NeutraliseOtherApps("UT-DEL-");
        var id = SeedApp("UT-DEL-1", "deleted", "60 day");   // decided + already erased
        Assert.Equal(0, HonoraryIdv.PurgeExpired(_db, 30));
        Assert.Equal("deleted", H.Str(_db.QueryOne("SELECT idv_status FROM honorary_applications WHERE id=?", id)!["idv_status"]));
    }
}

public class RetentionServiceTests : IClassFixture<DbFixture>
{
    readonly Db _db;
    public RetentionServiceTests(DbFixture f) => _db = f.Db;

    [Fact]
    public async Task Service_StartsAndStopsCleanly_NeverPurgesAtBoot()
    {
        var artefact = Storage.Put(StorageTests.PngBytes(), "image/png", "evidence");
        var full = Path.Combine(TestEnv.StorageRoot, artefact.Reference["local:".Length..]);
        // an artefact already past an aggressive 1-day window must survive service start-up:
        // the first sweep is deliberately one whole interval away (boot never deletes data)
        File.SetLastWriteTimeUtc(full, DateTime.UtcNow.AddDays(-2));
        _db.Execute("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('evidence_retention_days','1')");

        var svc = new RetentionService(_db);
        await svc.StartAsync(CancellationToken.None);
        await Task.Delay(100);
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await svc.StopAsync(cts.Token);

        Assert.NotNull(svc.ExecuteTask);
        Assert.True(svc.ExecuteTask!.IsCompleted);   // OperationCanceledException translated to a clean stop
        Assert.True(File.Exists(full));
        _db.Execute("DELETE FROM site_settings WHERE skey='evidence_retention_days'");
    }
}
