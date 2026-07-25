using System.Text;
using PCI.Backend.Core;
using PCI.Backend.Data;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World — the premium Passport and the privacy guarantees around it (Phase 2).
///
/// A Passport is only worth something if three claims hold: the reader can verify it against the
/// live record, the owner controls exactly what appears, and deleting the account really removes
/// the person rather than merely detaching a foreign key. Each is tested here.
/// </summary>
public class WorldPassportTests
{
    static Db NewWorldDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        return db;
    }

    /// <summary>A verified account with two completed, published-to-Passport attempts.</summary>
    static (long UserId, long SessionId) Participant(Db db, string email = "p@example.com")
    {
        var (err, userId, _) = WorldAccount.Register(db, email, "a-long-enough-password", "Sam Rivera", null);
        Assert.Null(err);
        db.Execute("UPDATE pciworld_users SET email_verified=1 WHERE id=?", userId);

        db.Execute("INSERT INTO pciworld_sessions(token_sha) VALUES(?)", Security.Sha("sess-" + email));
        var sessionId = db.Scalar<long>("SELECT id FROM pciworld_sessions WHERE token_sha=?", Security.Sha("sess-" + email));

        var challenges = db.Query("SELECT id,current_version FROM pciworld_challenges WHERE current_version>=1 LIMIT 2");
        foreach (var c in challenges)
            db.Execute(@"INSERT INTO pciworld_attempts(session_id,user_id,challenge_id,version,status,score,
                    profile_key,passport_visible,completed_at,answers_json)
                VALUES(?,?,?,?, 'completed', 88.5, 'evidence_led', 1, '2026-05-04 09:30:00', '{""sv"":-1}')",
                sessionId, userId, H.L(c["id"]), H.L(c["current_version"]));
        return (userId, sessionId);
    }

    // ───────────────────────── disclosure ─────────────────────────

    [Fact]
    public void Passport_shows_only_the_fields_the_owner_published()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        var rows = WorldAccount.EvidenceRows(db, userId, visibleOnly: true);
        Assert.Equal(2, rows.Count);

        // Everything on by default — the behaviour every already-published Passport had.
        var all = WorldPassport.Disclosure.From(db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId)!);
        Assert.True(all.Scores && all.Profiles && all.Dates);
        var full = WorldPages.PublicPassport(db, "Sam Rivera", rows, all);
        Assert.Contains("88.5", full);
        Assert.Contains("Evidence led", full);
        Assert.Contains("2026-05-04", full);

        // Scores off: the number must not reach the page at all — not merely be hidden by CSS,
        // which would still ship the value in the HTML.
        db.Execute("UPDATE pciworld_users SET passport_show_scores=0, passport_show_dates=0 WHERE id=?", userId);
        var reduced = WorldPassport.Disclosure.From(db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId)!);
        var partial = WorldPages.PublicPassport(db, "Sam Rivera", rows, reduced);
        Assert.DoesNotContain("88.5", partial);
        Assert.DoesNotContain("2026-05-04", partial);
        Assert.Contains("Evidence led", partial);            // still published
        Assert.Contains(H.Str(rows[0]["title"])!, partial);  // titles are always shown

        // Column headers track the disclosure, so the table never has an empty column.
        Assert.DoesNotContain("<th scope=\"col\">Score</th>", partial);
        Assert.Contains("<th scope=\"col\">Decision profile</th>", partial);
    }

    // ───────────────────────── expiry ─────────────────────────

    [Fact]
    public void An_expired_passport_link_stops_resolving()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        Assert.Null(WorldAccount.PublishPassport(db, userId).Error);

        var live = db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId)!;
        Assert.False(WorldPassport.Expired(live));                       // NULL = no expiry

        db.Execute("UPDATE pciworld_users SET passport_expires_at=datetime('now','+30 days') WHERE id=?", userId);
        Assert.False(WorldPassport.Expired(db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId)!));

        db.Execute("UPDATE pciworld_users SET passport_expires_at=datetime('now','-1 day') WHERE id=?", userId);
        Assert.True(WorldPassport.Expired(db.QueryOne("SELECT * FROM pciworld_users WHERE id=?", userId)!));
    }

    [Fact]
    public void Republishing_rotates_the_token_so_the_old_link_dies()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        var (_, first) = WorldAccount.PublishPassport(db, userId);
        var (_, second) = WorldAccount.PublishPassport(db, userId);
        Assert.NotNull(first);
        Assert.NotEqual(first, second);

        var stored = H.Str(db.QueryOne("SELECT passport_token_sha FROM pciworld_users WHERE id=?", userId)!["passport_token_sha"]);
        var secondToken = second!.Split('/')[^1];
        Assert.Equal(Security.Sha(secondToken), stored);
        // The raw token is never stored — only its hash, so a database read cannot mint links.
        Assert.NotEqual(secondToken, stored);
        Assert.NotEqual(Security.Sha(first!.Split('/')[^1]), stored);
    }

    // ───────────────────────── the artefact ─────────────────────────

    [Fact]
    public void The_passport_pdf_is_a_valid_deterministic_document_that_leaks_nothing()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        var rows = WorldAccount.EvidenceRows(db, userId, visibleOnly: true);
        var doc = new WorldPassport.PassportDoc
        {
            Name = "Sam Rivera",
            VerifyUrl = "https://pciworld.example/world/p/" + new string('a', 64),
            Completed = rows.Count, Industries = 2, Tracks = 1,
            IssuedOn = "2026-07-24",
            Show = new WorldPassport.Disclosure(true, true, true),
        };
        foreach (var r in rows)
            doc.Rows.Add((H.Str(r["title"]) ?? "", H.Str(r["industry"]) ?? "", "Professional", "88.5", "evidence led", "2026-05-04"));

        var pdf = WorldPassport.Pdf(doc);
        var text = Encoding.ASCII.GetString(pdf);
        Assert.StartsWith("%PDF-1.4", text);
        Assert.EndsWith("%%EOF", text);
        Assert.Contains("/Type /Catalog", text);
        Assert.Contains("startxref", text);

        // Deterministic for a given input: two renders of the same document are byte-identical, so
        // the artefact can be hashed and compared.
        Assert.Equal(pdf, WorldPassport.Pdf(doc));

        // It carries the verification route and the practice disclosure, and no answers.
        Assert.Contains("VERIFY THIS PASSPORT", text);
        Assert.Contains("pciworld.example", text);
        Assert.DoesNotContain("p@example.com", text);   // never the email
        Assert.DoesNotContain("sv", text.Split("stream")[0]);
    }

    [Fact]
    public void The_cover_endorsement_follows_the_crimson_rule()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        var rows = WorldAccount.EvidenceRows(db, userId, visibleOnly: true);
        var html = WorldPages.PublicPassport(db, "Sam Rivera", rows);

        // The cover lockup is ordered: the PCI World wordmark, the crimson bar, then who the
        // artefact is from — the same endorsement the site header carries. Anchor the search
        // inside .ppt-top, because the page header contains its own copy of the words.
        var top = html.IndexOf("class=\"ppt-top\"", StringComparison.Ordinal);
        Assert.True(top >= 0);
        var bar = html.IndexOf("class=\"bar\"", top, StringComparison.Ordinal);
        var from = html.IndexOf("class=\"ppt-from\">From the Project<br>Controls Institute", top, StringComparison.Ordinal);
        Assert.True(bar > top && from > bar);

        // The PDF says the same thing after its crimson rule.
        var pdf = Encoding.ASCII.GetString(WorldPassport.Pdf(new WorldPassport.PassportDoc
        { Name = "Sam Rivera", VerifyUrl = "https://pciworld.example/world/p/x" }));
        Assert.Contains("FROM THE PROJECT CONTROLS INSTITUTE", pdf);
    }

    [Fact]
    public void The_photograph_appears_only_when_provided_and_is_erased_with_the_account()
    {
        var db = NewWorldDb();
        var (userId, _) = Participant(db);
        var rows = WorldAccount.EvidenceRows(db, userId, visibleOnly: true);

        // No photo: no <img> reaches the page at all — absence, not display:none. (The stylesheet
        // always carries the class, so the assertion targets the markup.)
        Assert.DoesNotContain("<img class=\"ppt-photo\"", WorldPages.PublicPassport(db, "Sam Rivera", rows));

        // Store one through the same validation path the endpoint uses: data URI → sniff → Put.
        var png = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x70, 0x63, 0x69 };
        var (bytes, mime, err) = Storage.DecodeDataUri("data:image/png;base64," + Convert.ToBase64String(png));
        Assert.Null(err);
        var obj = Storage.Put(bytes!, mime, "world-passport");
        db.Execute("UPDATE pciworld_users SET passport_photo_ref=?, passport_photo_mime=? WHERE id=?",
            obj.Reference, mime, userId);

        var withPhoto = WorldPages.PublicPassport(db, "Sam Rivera", rows, photoUrl: "/world/p/tok/photo");
        Assert.Contains("<img class=\"ppt-photo\"", withPhoto);
        Assert.Contains("src=\"/world/p/tok/photo\"", withPhoto);
        Assert.Contains("Photograph of Sam Rivera", withPhoto);   // the image carries an accessible name

        // A PDF is a valid stored artefact elsewhere, but never a Passport photograph.
        var (_, pdfMime, pdfErr) = Storage.DecodeDataUri("data:application/pdf;base64," +
            Convert.ToBase64String(new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D }));
        Assert.Null(pdfErr);
        Assert.False(pdfMime.StartsWith("image/"));               // the endpoint's image-only gate

        // Deleting the account erases the stored image itself, not just the row pointing to it.
        Assert.NotNull(Storage.Get(obj.Reference));
        WorldAccount.DeleteAccount(db, userId);
        Assert.Null(Storage.Get(obj.Reference));
    }

    [Fact]
    public void The_verification_qr_is_self_contained_svg_with_an_accessible_name()
    {
        var svg = WorldPassport.QrSvg("https://pciworld.example/world/p/abc");
        Assert.StartsWith("<svg xmlns=\"http://www.w3.org/2000/svg\"", svg);
        Assert.EndsWith("</svg>", svg);
        Assert.Contains("role=\"img\"", svg);
        Assert.Contains("aria-label=", svg);
        Assert.Contains("<rect", svg);
        Assert.DoesNotContain("http://www.w3.org/1999/xlink", svg);  // no external references at all
        // Different payloads produce different codes; the same payload is stable.
        Assert.NotEqual(svg, WorldPassport.QrSvg("https://pciworld.example/world/p/xyz"));
        Assert.Equal(svg, WorldPassport.QrSvg("https://pciworld.example/world/p/abc"));
    }

    [Fact]
    public void The_verification_page_never_confirms_that_a_bad_link_once_existed()
    {
        var db = NewWorldDb();
        var page = WorldPages.VerifyPassport(db, "https://pciworld.example/world/p/" + new string('f', 64), null);
        Assert.Contains("does not resolve", page);
        // It must not claim the link is fake, expired or withdrawn — we cannot tell which, and
        // saying so would leak whether a Passport ever existed at that address.
        Assert.DoesNotContain("was withdrawn", page);
        Assert.DoesNotContain("has expired", page);
        Assert.Contains(WorldPages.PracticeNotice, page);
    }

    // ───────────────────────── privacy ─────────────────────────

    [Fact]
    public void Deleting_an_account_de_identifies_rather_than_merely_unlinking()
    {
        var db = NewWorldDb();
        var (userId, sessionId) = Participant(db);
        Assert.Null(WorldAccount.PublishPassport(db, userId).Error);

        // Give the account everything that could re-identify it later.
        var attemptId = db.Scalar<long>("SELECT id FROM pciworld_attempts WHERE user_id=? LIMIT 1", userId);
        db.Execute("INSERT INTO pciworld_invites(attempt_id,token_sha,inviter_name) VALUES(?,?,'Sam')",
            attemptId, Security.Sha("invite-token"));
        db.Execute("INSERT INTO pciworld_reports(challenge_id,category,message,session_id) VALUES(1,'other','I am Sam and my email is p@example.com',?)", sessionId);
        db.Execute("INSERT INTO pciworld_events(event,session_id) VALUES('home_viewed',?)", sessionId);

        WorldAccount.DeleteAccount(db, userId);

        // The account and every credential are gone.
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_users WHERE id=?", userId));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE user_id=?", userId));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_tokens WHERE user_id=?", userId));

        // Attempts survive as anonymous statistics — but carry nothing that points back.
        var attempts = db.Query("SELECT * FROM pciworld_attempts WHERE id=?", attemptId);
        Assert.Single(attempts);
        Assert.Null(attempts[0]["user_id"]);
        Assert.Null(attempts[0]["display_name"]);
        Assert.Null(attempts[0]["result_token_sha"]);
        Assert.Null(attempts[0]["answers_json"]);            // answer text is personal content
        Assert.Equal(0L, H.L(attempts[0]["session_id"]));     // the linking pseudonym is gone too
        Assert.NotNull(attempts[0]["score"]);                 // the anonymous statistic remains

        // Every public surface tied to the identity stops resolving.
        Assert.Equal(1L, db.Scalar<long>("SELECT revoked FROM pciworld_invites WHERE attempt_id=?", attemptId));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_sessions WHERE id=?", sessionId));

        // Reports and analytics keep their content but lose the linkage.
        Assert.Null(db.QueryOne("SELECT session_id FROM pciworld_reports LIMIT 1")!["session_id"]);
        Assert.Null(db.QueryOne("SELECT session_id FROM pciworld_events WHERE event='home_viewed' LIMIT 1")!["session_id"]);
    }

    [Fact]
    public void Retention_expires_continuity_data_and_never_learner_history()
    {
        var db = NewWorldDb();
        var (userId, sessionId) = Participant(db);

        db.Execute("INSERT INTO pciworld_user_sessions(user_id,token,expires_at) VALUES(?,?, datetime('now','-1 day'))", userId, "expired");
        db.Execute("INSERT INTO pciworld_user_sessions(user_id,token,expires_at) VALUES(?,?, datetime('now','+1 day'))", userId, "live");
        db.Execute("INSERT INTO pciworld_user_tokens(user_id,purpose,token_sha,expires_at) VALUES(?, 'reset', 'x', datetime('now','-1 hour'))", userId);
        db.Execute("INSERT INTO pciworld_events(event,created_at) VALUES('home_viewed', datetime('now','-500 days'))");
        db.Execute("INSERT INTO pciworld_events(event,created_at) VALUES('home_viewed', datetime('now','-10 days'))");
        // A dormant anonymous session that owns nothing, and the participant's, which owns attempts.
        db.Execute("INSERT INTO pciworld_sessions(token_sha,last_seen_at) VALUES('dormant', datetime('now','-400 days'))");
        db.Execute("UPDATE pciworld_sessions SET last_seen_at=datetime('now','-400 days') WHERE id=?", sessionId);

        var attemptsBefore = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts");
        WorldRetentionService.Sweep(db);

        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE token='expired'"));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_sessions WHERE token='live'"));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_user_tokens"));
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_events WHERE created_at<=datetime('now','-450 days')"));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_events"));
        // The dormant session goes; the one holding learner history stays, whatever its age.
        Assert.Equal(0, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_sessions WHERE token_sha='dormant'"));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_sessions WHERE id=?", sessionId));
        Assert.Equal(attemptsBefore, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_attempts"));
    }

    [Fact]
    public void Retention_windows_of_zero_disable_the_sweep()
    {
        var db = NewWorldDb();
        Settings.Put(db, "world_session_retention_days", "0");
        Settings.Put(db, "world_event_retention_days", "0");
        db.Execute("INSERT INTO pciworld_sessions(token_sha,last_seen_at) VALUES('ancient', datetime('now','-4000 days'))");
        db.Execute("INSERT INTO pciworld_events(event,created_at) VALUES('home_viewed', datetime('now','-4000 days'))");

        WorldRetentionService.Sweep(db);

        // A stray 0 must never mean "delete everything older than now" — the platform's convention.
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_sessions WHERE token_sha='ancient'"));
        Assert.Equal(1, db.Scalar<long>("SELECT COUNT(*) FROM pciworld_events"));
    }
}
