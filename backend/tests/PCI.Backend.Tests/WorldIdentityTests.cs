using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — canonical-identity unification (journey repair P0-00).
///
/// Proves the legacy pciworld_users → canonical users mapping: portal-linked rows map to exactly
/// their student; standalone rows become first-class canonical students with the SAME bcrypt hash
/// (one password, both products); an email collision with an existing canonical account is
/// quarantined and never silently merged; the whole pass is idempotent and append-only; and the
/// participation aggregate is keyed one-to-one by canonical users.id with product data only.
/// </summary>
public class WorldIdentityTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    static long CanonicalUser(Db db, string email) =>
        db.ExecuteReturningId(@"INSERT INTO users(email,first_name,last_name,password_hash,role,status)
            VALUES(?,?,?,?,'student','active')", email, "Cano", "Nical", BCrypt.Net.BCrypt.HashPassword("portal-password-1"));

    static long LegacyWorld(Db db, string email, string? displayName = null, long? studentId = null) =>
        db.ExecuteReturningId(@"INSERT INTO pciworld_users(email,password_hash,display_name,student_user_id)
            VALUES(?,?,?,?)", email, BCrypt.Net.BCrypt.HashPassword("world-password-1"), displayName, studentId);

    [Fact]
    public void Linked_rows_map_to_exactly_their_student_and_gain_a_participation_row()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "linked@x.test");
        var wid = LegacyWorld(db, "linked@x.test", "L Inked", student);

        var r = WorldIdentity.Run(db);
        Assert.Equal(1, r.Linked);
        Assert.Equal(0, r.Created);
        Assert.Equal(0, r.Conflicts);
        Assert.Equal(student, WorldIdentity.CanonicalFor(db, wid));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", student));
        // No second canonical identity was created.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='linked@x.test'"));
    }

    [Fact]
    public void Standalone_rows_become_canonical_students_with_the_same_password()
    {
        var db = NewWorldDb();
        var wid = LegacyWorld(db, "solo@x.test", "Solo Learner");

        var r = WorldIdentity.Run(db);
        Assert.Equal(1, r.Created);
        var uid = WorldIdentity.CanonicalFor(db, wid);
        Assert.NotNull(uid);

        var u = db.QueryOne("SELECT * FROM users WHERE id=?", uid)!;
        Assert.Equal("solo@x.test", H.Str(u["email"]));
        Assert.Equal("Solo", H.Str(u["first_name"]));
        Assert.Equal("Learner", H.Str(u["last_name"]));
        Assert.Equal("active", H.Str(u["status"]));
        // THE guarantee of P0-00: the one bcrypt hash now verifies on the canonical side too —
        // same email, same password, both products, zero copied/synchronized hashes.
        Assert.True(Security.VerifyPassword("world-password-1", H.Str(u["password_hash"])));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=?", uid));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", uid));
        // The world row is now linked, so future passes see it as decided.
        Assert.Equal(Convert.ToInt64(uid), Convert.ToInt64(
            db.QueryOne("SELECT student_user_id FROM pciworld_users WHERE id=?", wid)!["student_user_id"]));
    }

    [Fact]
    public void Email_collisions_are_quarantined_never_silently_merged()
    {
        var db = NewWorldDb();
        var portal = CanonicalUser(db, "both@x.test");
        var wid = LegacyWorld(db, "both@x.test");   // same email, NOT linked — could be someone else

        var r = WorldIdentity.Run(db);
        Assert.Equal(1, r.Conflicts);
        Assert.Null(WorldIdentity.CanonicalFor(db, wid));
        var m = db.QueryOne("SELECT * FROM pciworld_user_map WHERE legacy_world_id=?", wid)!;
        Assert.Equal("conflict", H.Str(m["outcome"]));
        Assert.Null(m["canonical_user_id"]);
        // Neither side was touched: one canonical user, its hash intact, no participation row.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='both@x.test'"));
        Assert.True(Security.VerifyPassword("portal-password-1",
            H.Str(db.QueryOne("SELECT password_hash FROM users WHERE id=?", portal)!["password_hash"])));
        Assert.Equal(0L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", portal));
    }

    [Fact]
    public void A_conflict_resolved_by_a_later_portal_link_upgrades_to_linked()
    {
        var db = NewWorldDb();
        var portal = CanonicalUser(db, "later@x.test");
        var wid = LegacyWorld(db, "later@x.test");
        Assert.Equal(1, WorldIdentity.Run(db).Conflicts);

        // The person proves the identity in the portal (LinkStudent adopts the row)…
        db.Execute("UPDATE pciworld_users SET student_user_id=? WHERE id=?", portal, wid);
        var r2 = WorldIdentity.Run(db);
        Assert.Equal(1, r2.Upgraded);
        Assert.Equal(portal, WorldIdentity.CanonicalFor(db, wid));
        Assert.Equal("linked", H.Str(db.QueryOne("SELECT outcome FROM pciworld_user_map WHERE legacy_world_id=?", wid)!["outcome"]));
        Assert.NotNull(db.QueryOne("SELECT resolved_at FROM pciworld_user_map WHERE legacy_world_id=?", wid)!["resolved_at"]);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", portal));
    }

    [Fact]
    public void The_pass_is_idempotent_and_append_only()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "idem-linked@x.test");
        LegacyWorld(db, "idem-linked@x.test", null, student);
        LegacyWorld(db, "idem-solo@x.test", "Ida Solo");
        CanonicalUser(db, "idem-clash@x.test");
        LegacyWorld(db, "idem-clash@x.test");

        var first = WorldIdentity.Run(db);
        Assert.Equal((1, 1, 1), (first.Linked, first.Created, first.Conflicts));

        var users = db.Scalar<long>("SELECT COUNT(*) FROM users");
        var maps = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_map");
        var parts = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants");
        for (var i = 0; i < 3; i++)
        {
            var again = WorldIdentity.Run(db);
            Assert.Equal(0, again.Linked + again.Created + again.Upgraded);
        }
        Assert.Equal(users, db.Scalar<long>("SELECT COUNT(*) FROM users"));
        Assert.Equal(maps, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_map"));
        Assert.Equal(parts, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants"));
    }

    [Fact]
    public void New_world_registrations_gain_a_canonical_identity_immediately()
    {
        var db = NewWorldDb();
        var (err, wid, _) = WorldAccount.Register(db, "fresh@x.test", "long-password-1", "Fresh Start", null);
        Assert.Null(err);

        var uid = WorldIdentity.CanonicalFor(db, wid);
        Assert.NotNull(uid);
        var u = db.QueryOne("SELECT * FROM users WHERE id=?", uid)!;
        Assert.Equal("fresh@x.test", H.Str(u["email"]));
        Assert.True(Security.VerifyPassword("long-password-1", H.Str(u["password_hash"])));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", uid));
        // Exactly one canonical identity and one student profile — no duplicates anywhere.
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='fresh@x.test'"));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM student_profiles WHERE user_id=?", uid));
    }

    [Fact]
    public void The_portal_bridge_records_the_mapping_for_the_signed_in_student()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "bridge@x.test");
        var (err, wid) = WorldAccount.LinkStudent(db, student, "bridge@x.test", "B Ridge");
        Assert.Null(err);
        Assert.Equal(student, WorldIdentity.CanonicalFor(db, wid));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", student));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='bridge@x.test'"));
    }

    [Fact]
    public void World_login_accepts_the_same_canonical_credentials_the_portal_uses()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "same-creds@x.test");   // portal password: portal-password-1
        var sess = db.ExecuteReturningId("INSERT INTO pciworld_sessions(token_sha) VALUES('s-cred')");

        // No World account exists — the canonical credentials alone sign the student in.
        var (err, wid, token) = WorldAccount.Login(db, "same-creds@x.test", "portal-password-1", sess);
        Assert.Null(err);
        Assert.NotEqual("", token);
        // The linked World account was created through the guarded bridge: verified, linked to the
        // student, and mapped — never a second canonical identity.
        var w = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", wid)!;
        Assert.Equal(student, Convert.ToInt64(w["student_user_id"]));
        Assert.Equal(1L, Convert.ToInt64(w["email_verified"]));
        Assert.Equal(student, WorldIdentity.CanonicalFor(db, wid));
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM users WHERE email='same-creds@x.test'"));

        // A second login resolves the SAME account (no duplicates), and a wrong password fails
        // with the generic error on either path.
        var again = WorldAccount.Login(db, "same-creds@x.test", "portal-password-1", null);
        Assert.Equal(wid, again.UserId);
        Assert.Equal("invalid_credentials", WorldAccount.Login(db, "same-creds@x.test", "wrong-password-x", null).Error);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE email='same-creds@x.test'"));
    }

    [Fact]
    public void A_portal_password_change_signs_in_on_world_the_canonical_hash_wins()
    {
        var db = NewWorldDb();
        var student = CanonicalUser(db, "rotated@x.test");
        // A linked World row whose own (legacy) password the user never knew — the old bridge shape.
        var wid = LegacyWorld(db, "rotated@x.test", "R Otated", student);
        WorldIdentity.Run(db);

        // The student changes their password on the portal; the canonical hash is updated.
        db.Execute("UPDATE users SET password_hash=? WHERE id=?",
            BCrypt.Net.BCrypt.HashPassword("new-portal-password-2"), student);

        // The NEW canonical password signs in on World, resolving the same linked account.
        var (err, got, _) = WorldAccount.Login(db, "rotated@x.test", "new-portal-password-2", null);
        Assert.Null(err);
        Assert.Equal(wid, got);
    }

    [Fact]
    public void A_world_email_linked_to_a_different_student_is_refused_never_hijacked()
    {
        var db = NewWorldDb();
        // The World row carries email "victim@x.test" but is LINKED to student A (whose canonical
        // email differs — the World email is stale). Canonical user B genuinely owns
        // "victim@x.test" on the platform.
        var studentA = CanonicalUser(db, "a-real@x.test");
        var wid = LegacyWorld(db, "victim@x.test", "V Ictim", studentA);
        CanonicalUser(db, "victim@x.test");   // student B — portal password: portal-password-1
        WorldIdentity.Run(db);

        // B signs in on World with their own valid canonical credentials: the bridge must refuse
        // to adopt A's linked World account rather than hand over its attempts and Passport.
        var hijack = WorldAccount.Login(db, "victim@x.test", "portal-password-1", null);
        Assert.Equal("email_in_use", hijack.Error);
        Assert.Equal(studentA, Convert.ToInt64(
            db.QueryOne("SELECT student_user_id FROM pciworld_users WHERE id=?", wid)!["student_user_id"]));
    }

    [Fact]
    public void Participation_rows_hold_product_data_only_and_are_unique_per_user()
    {
        var db = NewWorldDb();
        var uid = CanonicalUser(db, "part@x.test");
        WorldIdentity.EnsureParticipant(db, uid);
        WorldIdentity.EnsureParticipant(db, uid);
        Assert.Equal(1L, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_participants WHERE user_id=?", uid));
        // The aggregate must never grow credential or profile columns — that is the boundary.
        var cols = db.Columns("pciworld_participants");
        foreach (var forbidden in new[] { "email", "password_hash", "mfa_secret", "first_name", "last_name",
                                          "mobile", "company", "current_role" })
            Assert.DoesNotContain(forbidden, cols);
    }
}
