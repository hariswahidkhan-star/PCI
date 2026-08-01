using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Makes community rooms usable the moment an operator turns the feature on.
///
/// Without this, enabling <c>world_community_enabled</c> still left every participant facing an
/// empty catalogue (no seeded rooms), a join form that always refused (empty jurisdiction
/// allowlist), and a NullModerator that accepts messages then never publishes them. That is the
/// difference between "the feature exists in the codebase" and "a proper chat room people can use".
///
/// Fail-closed defaults stay in place until the flag is ON. The first enable is the operator's
/// signal that they want the surface live; we then fill the gaps that would otherwise brick it.
/// Jurisdictions and the moderator can still be tightened later from world-admin settings.
/// </summary>
public static class CommunityBootstrap
{
    /// <summary>ISO alpha-2 markets commonly served by project-controls employers. Not a legal
    /// opinion — an operator starting point so the join form has somewhere to land. Counsel can
    /// narrow the list in Settings without a code change.</summary>
    public const string DefaultJurisdictions =
        "GB,US,AE,PK,IN,CA,AU,NZ,IE,DE,FR,NL,SG,ZA,SA,QA,KW,BH,OM,EG,KE,NG,MY,ID,PH,TH,JP,KR,BR,MX,TR,PL,ES,IT,PT,SE,NO,DK,FI,CH,AT,BE";

    public static void EnsureDefaultRoom(Db db)
    {
        db.Execute(
            @"INSERT OR IGNORE INTO pciworld_community_rooms
                (slug,title,description,topic,category,state,guest_allowed,member_allowed,
                 pinned_welcome,discoverable,rules_version,capacity)
              VALUES(
                'general',
                'General project controls',
                'Open room for professional discussion of scheduling, cost, risk and controls practice.',
                'Ask questions, share approaches, and help peers — no sales pitches.',
                'practice',
                'open',
                1, 1,
                'Welcome. Be professional. No contact details, abuse or spam. Messages are checked before anyone else sees them.',
                1, 'v1', 200)");
        // A leftover draft from an earlier seed still counts — open it so the catalogue is not empty.
        db.Execute(
            @"UPDATE pciworld_community_rooms
                 SET state='open', discoverable=1, updated_at=datetime('now')
               WHERE slug='general' AND state IN ('draft','closed')");
    }

    /// <summary>Called when an operator turns community rooms (or the forum) on. Idempotent.</summary>
    public static void OnFeatureEnabled(Db db, Action<string>? log = null)
    {
        EnsureDefaultRoom(db);

        if (!CommunityEligibility.Jurisdictions(db).Any())
        {
            Settings.Put(db, CommunityEligibility.JurisdictionsKey, DefaultJurisdictions);
            log?.Invoke("community bootstrap: seeded default jurisdiction allowlist (narrow in Settings)");
        }

        var mod = (Settings.Str(db, "world_community_moderator", "none") ?? "none").Trim().ToLowerInvariant();
        if (mod is "" or "none")
        {
            // DeterministicModerator publishes ordinary professional text and holds obvious abuse
            // markers — enough for a working room without a contracted AI vendor. Operators who
            // need fail-closed NullModerator behaviour can set moderator=none again explicitly.
            Settings.Put(db, "world_community_moderator", "deterministic");
            log?.Invoke("community bootstrap: moderator set to deterministic so messages can publish");
        }
    }

    /// <summary>Soft ensure used on the public catalogue path: create the default room if the
    /// feature is on and the catalogue would otherwise be empty. Does NOT change moderator or
    /// jurisdictions — those are operator decisions made at enable time.</summary>
    public static void EnsureCatalogueNotEmpty(Db db)
    {
        if (!Settings.Bool(db, "world_community_enabled", false)) return;
        var open = db.Scalar<long>(
            "SELECT COUNT(*) FROM pciworld_community_rooms WHERE state IN ('open','slow_mode','read_only') AND discoverable=1");
        if (open == 0) EnsureDefaultRoom(db);
    }
}
