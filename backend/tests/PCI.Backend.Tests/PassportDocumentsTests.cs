using System.Text;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Phase 5A (Release 1) — printable Passport documents (Core/PassportDocuments.cs +
/// Endpoints/PassportDocs.cs, spec §7A.3–§7A.5).
///
/// The contract under test: the wallet card is two CR80 pages (242.65 × 153.07 pt) and the event
/// badge one 4×6in portrait page (288 × 432 pt); both carry the name and PCI Student Number as
/// vector text; the QR payload is ONLY the opaque /world/pd/{token-hash} verification URL; and
/// only a PUBLISHED, unexpired Passport on an active, linked, numbered account resolves to a
/// document — every other state collapses to the same null refusal. The printed key is the stored
/// token hash, so it shares the owner link's exact lifecycle: unpublishing kills it instantly.
/// </summary>
public class PassportDocumentsTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long MkStudent(Db db, string email)
    {
        var uid = db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES(?,?,?,?,'student','active')", email, "Jordan", "Q", BCrypt.Net.BCrypt.HashPassword("portal-pw-1"));
        StudentNumbers.GetOrIssue(db, uid, "test_fixture");
        return uid;
    }

    static long LinkWorld(Db db, long uid, string email)
    {
        var (err, wid) = WorldAccount.LinkStudent(db, uid, email, "Jordan Q.");
        Assert.Null(err);
        return wid;
    }

    static void MkEvidence(Db db, long wid, long uid, string code, bool visible = true,
        string completed = "2026-06-01 09:00:00", double score = 82.5)
    {
        var cid = db.QueryOne("SELECT id FROM pciworld_challenges WHERE code=?", code) is { } ch
            ? H.L(ch["id"])
            : db.ExecuteReturningId("INSERT INTO pciworld_challenges(code,title) VALUES(?,?)", code, "T " + code);
        if (db.QueryOne("SELECT id FROM pciworld_challenge_versions WHERE challenge_id=? AND version=1", cid) is null)
            db.Execute(@"INSERT INTO pciworld_challenge_versions(challenge_id,version,title,industry,track,difficulty,config_json)
                VALUES(?,?,?,?,?,?,'{}')", cid, 1, "T " + code, "Energy", "project_controls", "professional");
        db.Execute(@"INSERT INTO pciworld_attempts(session_id,challenge_id,version,status,score,profile_key,
                completed_at,user_id,passport_visible,canonical_user_id)
            VALUES(0,?,1,'completed',?,'steady_navigator',?,?,?,?)",
            cid, score, completed, wid, visible ? 1 : 0, uid);
    }

    /// <summary>A published fixture: linked, verified, one visible evidence row, published.</summary>
    static (long Uid, long Wid, string TokenSha) Published(Db db, string email)
    {
        var uid = MkStudent(db, email);
        var wid = LinkWorld(db, uid, email);
        MkEvidence(db, wid, uid, "WC-EVM-001");
        var (err, url) = WorldAccount.PublishPassport(db, wid);
        Assert.Null(err);
        var token = url!.Split('/').Last();
        return (uid, wid, Security.Sha(token));
    }

    static PassportDocuments.DocData Doc() => new()
    {
        Name = "Jordan Q.",
        StudentNumber = "PCI-2026-000042",
        VerifyUrl = "https://projectcontrolsinstitute.org/world/pd/" + new string('a', 64),
        ShortVerifyUrl = "projectcontrolsinstitute.org/world/verify",
        Headline = "7 completed PCI World challenges across 3 industries",
        ExpiresOn = "2026-12-01",
    };

    static int CountOf(string s, string sub)
    {
        var count = 0;
        var i = 0;
        while ((i = s.IndexOf(sub, i, StringComparison.Ordinal)) >= 0) { count++; i += sub.Length; }
        return count;
    }

    // ── geometry ──────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Wallet_is_two_CR80_pages_and_badge_one_4x6_page()
    {
        var wallet = Encoding.ASCII.GetString(PassportDocuments.WalletCard(Doc()));
        var badge = Encoding.ASCII.GetString(PassportDocuments.EventBadge(Doc()));

        Assert.StartsWith("%PDF-1.4", wallet);
        Assert.StartsWith("%PDF-1.4", badge);
        Assert.Equal(2, CountOf(wallet, "/MediaBox [0 0 242.65 153.07]"));
        Assert.Contains("/Count 2", wallet);
        Assert.Equal(1, CountOf(badge, "/MediaBox [0 0 288 432]"));
        Assert.Contains("/Count 1", badge);
        Assert.EndsWith("%%EOF", badge);
    }

    [Fact]
    public void Documents_carry_name_and_student_number_as_vector_text()
    {
        foreach (var pdf in new[] { PassportDocuments.WalletCard(Doc()), PassportDocuments.EventBadge(Doc()) })
        {
            var s = Encoding.ASCII.GetString(pdf);
            Assert.Contains("(PCI-2026-000042) Tj", s);      // uncompressed content stream = real text
            Assert.Contains("(Jordan Q.) Tj", s);
            Assert.Contains("/BaseFont /Helvetica", s);      // vector Type1 text, nothing rasterised
        }
    }

    [Fact]
    public void Output_is_deterministic_for_the_same_input()
    {
        Assert.Equal(PassportDocuments.WalletCard(Doc()), PassportDocuments.WalletCard(Doc()));
        Assert.Equal(PassportDocuments.EventBadge(Doc()), PassportDocuments.EventBadge(Doc()));
    }

    [Fact]
    public void Verify_path_is_the_opaque_prefix_plus_hash_and_nothing_else()
    {
        var sha = Security.Sha("some-token");
        Assert.Equal("/world/pd/" + sha, PassportDocuments.VerifyPath(sha));
        Assert.DoesNotContain("PCI-", PassportDocuments.VerifyPath(sha));
        Assert.DoesNotContain("@", PassportDocuments.VerifyPath(sha));
    }

    // ── the publication gate ──────────────────────────────────────────────────────────────────

    [Fact]
    public void A_published_unexpired_passport_resolves_with_the_stored_hash_and_number()
    {
        var db = NewWorldDb();
        var (uid, wid, tokenSha) = Published(db, "doc-live@x.test");

        var r = PassportDocs.ResolvePublished(db, uid);

        Assert.NotNull(r);
        Assert.Equal(wid, r!.WorldId);
        Assert.Equal(tokenSha, r.TokenSha);                       // the key IS the stored hash
        Assert.Equal(StudentNumbers.Read(db, uid), r.StudentNumber);
        Assert.Matches("^[0-9a-f]{64}$", r.TokenSha);             // opaque, no PII
    }

    [Fact]
    public void Unpublishing_refuses_immediately()
    {
        var db = NewWorldDb();
        var (uid, wid, _) = Published(db, "doc-unpub@x.test");

        WorldAccount.UnpublishPassport(db, wid);

        Assert.Null(PassportDocs.ResolvePublished(db, uid));
    }

    [Fact]
    public void An_expired_link_refuses_and_a_future_expiry_still_resolves()
    {
        var db = NewWorldDb();
        var (uid, wid, _) = Published(db, "doc-exp@x.test");

        db.Execute("UPDATE pciworld_users SET passport_expires_at=? WHERE id=?",
            DateTime.UtcNow.AddDays(90).ToString("yyyy-MM-dd HH:mm:ss"), wid);
        Assert.NotNull(PassportDocs.ResolvePublished(db, uid));

        db.Execute("UPDATE pciworld_users SET passport_expires_at=? WHERE id=?",
            DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"), wid);
        Assert.Null(PassportDocs.ResolvePublished(db, uid));
    }

    [Fact]
    public void Suspension_on_either_side_refuses()
    {
        var db = NewWorldDb();
        var (uid, wid, _) = Published(db, "doc-susp@x.test");

        db.Execute("UPDATE pciworld_users SET status='suspended' WHERE id=?", wid);
        Assert.Null(PassportDocs.ResolvePublished(db, uid));
        db.Execute("UPDATE pciworld_users SET status='active' WHERE id=?", wid);
        Assert.NotNull(PassportDocs.ResolvePublished(db, uid));

        db.Execute("UPDATE pciworld_participants SET status='suspended' WHERE user_id=?", uid);
        Assert.Null(PassportDocs.ResolvePublished(db, uid));
    }

    [Fact]
    public void A_private_or_unlinked_or_numberless_state_refuses()
    {
        var db = NewWorldDb();

        // linked but never published
        var uid = MkStudent(db, "doc-priv@x.test");
        var wid = LinkWorld(db, uid, "doc-priv@x.test");
        MkEvidence(db, wid, uid, "WC-RSK-002");
        Assert.Null(PassportDocs.ResolvePublished(db, uid));

        // no World account at all
        var solo = MkStudent(db, "doc-solo@x.test");
        Assert.Null(PassportDocs.ResolvePublished(db, solo));

        // published but the canonical number is missing — the document prints the number, so it
        // must exist; the refusal is the same null as every other gate
        var (uid2, _, _) = Published(db, "doc-nonum@x.test");
        db.Execute("UPDATE users SET registration_no=NULL WHERE id=?", uid2);
        Assert.Null(PassportDocs.ResolvePublished(db, uid2));
    }
}
