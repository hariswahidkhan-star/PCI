using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-12 — the per-channel fan-out decision core of the Communications Centre (<see cref="Comms.Fire"/>
/// + <see cref="Comms.Enqueue"/>): the code that turns ONE platform event into 0..N deduped rows in
/// comm_outbox, honouring each trigger's email / WhatsApp / in-app toggles, the WhatsApp opt-in + number
/// gate, the skipEmail / skipInApp overrides, marketing consent + suppression, duplicate prevention, and
/// template-vs-fallback content resolution.
///
/// CommsTests already covers Render + the unsubscribe tokens; this suite is disjoint from it and drives the
/// queue decisions directly against a fresh migrated temp-SQLite Db. NOTE the test schema starts with an
/// EMPTY comm_triggers catalogue — CommsSeed.Ensure runs only at app startup (Program.cs), never in
/// Migrate.Run — so every trigger the decision reads is seeded explicitly here (no migration defaults to
/// fight). Assertions read comm_outbox rows (channel / to_phone / subject), not return values, because
/// Fire is void. The email leg uses the outbox path (not Mailer), so nothing dials the network. Tests use
/// ONE-BLOCKER ISOLATION: seed a legend that would queue the leg, break exactly one precondition, and
/// assert precisely that leg is (or is not) enqueued. No application code is changed.
/// </summary>
public class CommsDecisionTests
{
    const string Code = "evt.test";
    const string Email = "comms@test.io";

    static Db Fresh() => TestEnv.NewMigratedDb();

    // Gotcha 1: FK ON — a real user must exist before anything references user_id.
    static long SeedUser(Db db, string email = Email)
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)",
                   email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    /// <summary>Seed a single trigger row with the channel toggles / templates / consent-category under test.
    /// The catalogue is empty by default, so this is the only trigger Fire can resolve.</summary>
    static void SeedTrigger(Db db, int email = 0, int whatsapp = 0, int inapp = 0,
        string? emailTpl = null, string? waTpl = null, string category = "transactional",
        int active = 1, string code = Code)
    {
        db.Execute(@"INSERT INTO comm_triggers
            (code,name,email_enabled,whatsapp_enabled,inapp_enabled,
             email_template_key,whatsapp_template_key,consent_category,active)
            VALUES(?,?,?,?,?,?,?,?,?)",
            code, code, email, whatsapp, inapp, emailTpl, waTpl, category, active);
    }

    static Dictionary<string, string?> Vars() => new();

    static long CountChan(Db db, string channel)
        => db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE channel=?", channel);
    static long CountAll(Db db)
        => db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox");

    // ================================================================================================
    //  Trigger toggles — which channels fan out
    // ================================================================================================

    [Fact]
    public void Fire_AllChannelsEnabled_QueuesEmailWhatsappAndInApp()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, whatsapp: 1, inapp: 1);
        // An explicit phone satisfies the WhatsApp gate directly (no profile / opt-in lookup needed).
        Comms.Fire(db, Code, uid, Email, "+441234567890", Vars(), "Subject", "Body");
        Assert.Equal(1L, CountChan(db, "email"));
        Assert.Equal(1L, CountChan(db, "whatsapp"));
        Assert.Equal(1L, CountChan(db, "inapp"));
    }

    [Fact]
    public void Fire_OnlyEmailToggleOn_QueuesEmailOnly()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, whatsapp: 0, inapp: 0);
        // Email address AND phone are both supplied; only the enabled leg queues.
        Comms.Fire(db, Code, uid, Email, "+441234567890", Vars(), "Subject", "Body");
        Assert.Equal(1L, CountChan(db, "email"));
        Assert.Equal(0L, CountChan(db, "whatsapp"));
        Assert.Equal(0L, CountChan(db, "inapp"));
    }

    [Fact]
    public void Fire_AllTogglesOff_QueuesNothing()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 0, whatsapp: 0, inapp: 0);
        Comms.Fire(db, Code, uid, Email, "+441234567890", Vars(), "Subject", "Body");
        Assert.Equal(0L, CountAll(db));
    }

    [Fact]
    public void Fire_InactiveTrigger_IsNoOp()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // active=0 — Fire's lookup filters on active=1, so an inactive trigger never fans out.
        SeedTrigger(db, email: 1, inapp: 1, active: 0);
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body");
        Assert.Equal(0L, CountAll(db));
    }

    [Fact]
    public void Fire_UnknownTrigger_IsNoOp()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // No trigger row at all → Fire returns before touching the outbox.
        Comms.Fire(db, "no.such.trigger", uid, Email, "+441234567890", Vars(), "Subject", "Body");
        Assert.Equal(0L, CountAll(db));
    }

    // ================================================================================================
    //  WhatsApp gating — opt-in flag x number present/absent
    // ================================================================================================

    [Fact]
    public void Fire_WhatsApp_OptedInWithProfileNumber_Queues()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, whatsapp: 1);
        // No explicit phone → the number is resolved from the profile, but ONLY when the channel is opted in.
        db.Execute("INSERT INTO comm_preferences(user_id,whatsapp_optin) VALUES(?,1)", uid);
        db.Execute("INSERT INTO student_profiles(user_id,mobile) VALUES(?,?)", uid, "+441234567890");
        Comms.Fire(db, Code, uid, null, null, Vars(), "Subject", "Body");
        Assert.Equal(1L, CountChan(db, "whatsapp"));
        Assert.Equal("+441234567890",
            H.Str(db.QueryOne("SELECT to_phone FROM comm_outbox WHERE channel='whatsapp'")!["to_phone"]));
    }

    [Fact]
    public void Fire_WhatsApp_OptedInButNoNumber_DoesNotQueue()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, whatsapp: 1);
        // Opt-in present, but there is no profile number to send to → no WhatsApp row.
        db.Execute("INSERT INTO comm_preferences(user_id,whatsapp_optin) VALUES(?,1)", uid);
        Comms.Fire(db, Code, uid, null, null, Vars(), "Subject", "Body");
        Assert.Equal(0L, CountChan(db, "whatsapp"));
    }

    [Fact]
    public void Fire_WhatsApp_ProfileNumberButNotOptedIn_DoesNotQueue()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, whatsapp: 1);
        // A number exists, but without the opt-in (WhatsApp Business policy) the number is never read.
        db.Execute("INSERT INTO student_profiles(user_id,mobile) VALUES(?,?)", uid, "+441234567890");
        Comms.Fire(db, Code, uid, null, null, Vars(), "Subject", "Body");
        Assert.Equal(0L, CountChan(db, "whatsapp"));
    }

    // ================================================================================================
    //  skipEmail / skipInApp — caller-side leg suppression
    // ================================================================================================

    [Fact]
    public void Fire_SkipEmail_SuppressesEmailButKeepsInApp()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, inapp: 1);
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", skipEmail: true);
        Assert.Equal(0L, CountChan(db, "email"));
        Assert.Equal(1L, CountChan(db, "inapp"));
    }

    [Fact]
    public void Fire_SkipInApp_SuppressesInAppButKeepsEmail()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, inapp: 1);
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", skipInApp: true);
        Assert.Equal(1L, CountChan(db, "email"));
        Assert.Equal(0L, CountChan(db, "inapp"));
    }

    // ================================================================================================
    //  Marketing consent + suppression — a suppressed / unconsented recipient gets no marketing send
    // ================================================================================================

    [Fact]
    public void Fire_MarketingEmail_WithoutConsent_IsDropped()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // consent_category=marketing → Enqueue requires marketing consent when a user is attached.
        SeedTrigger(db, email: 1, category: "marketing");
        // No comm_preferences row → no consent → dropped (recorded as a non-send).
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body");
        Assert.Equal(0L, CountChan(db, "email"));
    }

    [Fact]
    public void Fire_MarketingEmail_WithConsent_IsQueued()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, category: "marketing");
        // Explicit marketing consent unblocks the send.
        db.Execute("INSERT INTO comm_preferences(user_id,email_marketing) VALUES(?,1)", uid);
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body");
        Assert.Equal(1L, CountChan(db, "email"));
    }

    [Fact]
    public void Fire_MarketingEmail_SuppressedAddress_IsDropped()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1, category: "marketing");
        // Consent is granted, so suppression is the SOLE blocker under test.
        db.Execute("INSERT INTO comm_preferences(user_id,email_marketing) VALUES(?,1)", uid);
        db.Execute("INSERT INTO comm_suppression(channel,address,reason) VALUES('email',?,?)", Email, "bounce");
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body");
        Assert.Equal(0L, CountChan(db, "email"));
    }

    // ================================================================================================
    //  Dedup — a repeat Fire never queues a duplicate; a distinct suffix is a distinct event
    // ================================================================================================

    [Fact]
    public void Fire_RepeatFire_DoesNotDuplicate()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1);
        // Same trigger + recipient + dedupSuffix => identical dedup_key => second Fire is suppressed.
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", dedupSuffix: "cert-42");
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", dedupSuffix: "cert-42");
        Assert.Equal(1L, CountChan(db, "email"));
    }

    [Fact]
    public void Fire_DifferentDedupSuffix_QueuesSeparately()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        SeedTrigger(db, email: 1);
        // A different dedupSuffix composes a different dedup_key → a genuinely distinct queued row.
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", dedupSuffix: "cert-42");
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Subject", "Body", dedupSuffix: "cert-43");
        Assert.Equal(2L, CountChan(db, "email"));
    }

    // ================================================================================================
    //  Template resolution — fall back to the caller-supplied subject/body when no template is assigned
    // ================================================================================================

    [Fact]
    public void Fire_NoTemplate_FallsBackToProvidedSubjectAndBody()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        // email_template_key is null → ResolveEmail returns the rendered fallback content verbatim.
        SeedTrigger(db, email: 1, emailTpl: null);
        Comms.Fire(db, Code, uid, Email, null, Vars(), "Welcome aboard", "Plain body text");
        var row = db.QueryOne("SELECT subject,body FROM comm_outbox WHERE channel='email'")!;
        Assert.Equal("Welcome aboard", H.Str(row["subject"]));
        Assert.Equal("Plain body text", H.Str(row["body"]));
    }
}
