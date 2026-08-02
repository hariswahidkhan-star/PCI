using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>Turning community/forum ON must leave a joinable room and a publishing moderator.</summary>
[Collection(DbCollection.Name)]
public class CommunityBootstrapTests
{
    static Db NewDb()
    {
        // Full migrate so site_settings (and the rest of the World stack) exist — CommunitySchema
        // Seed writes the off-by-default flag into that table.
        var db = TestEnv.NewMigratedDb();
        CommunitySchema.Ensure(db);
        ForumSchema.Ensure(db);
        return db;
    }

    [Fact]
    public void OnFeatureEnabled_opens_a_default_room_and_fills_empty_policy()
    {
        var db = NewDb();
        Assert.Equal(0, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_rooms WHERE state IN ('open','slow_mode','read_only')"));
        Assert.Empty(CommunityEligibility.Jurisdictions(db));
        Assert.Equal("none", Settings.Str(db, "world_community_moderator", "none"));

        CommunityBootstrap.OnFeatureEnabled(db);

        Assert.True(db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_rooms WHERE slug='general' AND state='open'") > 0);
        Assert.NotEmpty(CommunityEligibility.Jurisdictions(db));
        Assert.Equal("deterministic", Settings.Str(db, "world_community_moderator", "none"));
    }

    [Fact]
    public void OnFeatureEnabled_preserves_a_jurisdiction_list_the_operator_already_set()
    {
        var db = NewDb();
        Settings.Put(db, CommunityEligibility.JurisdictionsKey, "GB");
        CommunityBootstrap.OnFeatureEnabled(db);
        Assert.Equal(new HashSet<string> { "GB" }, CommunityEligibility.Jurisdictions(db).ToHashSet());
    }

    [Fact]
    public void EnsureCatalogueNotEmpty_is_a_no_op_while_the_flag_is_off()
    {
        var db = NewDb();
        Settings.Put(db, "world_community_enabled", "0");
        CommunityBootstrap.EnsureCatalogueNotEmpty(db);
        Assert.Equal(0, db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_rooms WHERE state='open'"));
    }
}
