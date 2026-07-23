using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// BD-14 — the inbound customer-service auto-triage decision core of the Communications Centre
/// (<see cref="CommsRouting.Apply"/>): the code that, for one inbound message on a conversation, applies the
/// first matching routing rule (classification only), enforces the sensitive-topic escalation FLOOR, and then
/// runs the FAQ auto-responder per the configured mode. Per spec keyword rules NEVER make an adverse
/// decision — anything sensitive is force-escalated to a human and is NEVER auto-answered.
///
/// Apply is void, so every assertion reads state the call mutates: the comm_conversations row
/// (status / priority / category / tags / routed_rule_id / auto_answered / suggested_response), the
/// comm_inbound_messages 'out' transcript row, and the comm_outbox queue — never a return value. Like
/// CommsDecisionTests this drives a fresh migrated temp-SQLite Db directly; NOTE the test schema starts with an
/// EMPTY comm_routing_rules catalogue (CommsSeed.Ensure runs only at app startup in Program.cs, never in
/// Migrate.Run), so every rule a decision reads is seeded explicitly here — there are no migration defaults to
/// fight. chat_kb IS migration-seeded, so KB tests insert a distinctive private answer of their own for a
/// deterministic match. No application code is changed.
/// </summary>
public class CommsRoutingTests
{
    // A nonsense KB keyword that appears in no migration-seeded answer and contains no sensitive substring,
    // so a message carrying it deterministically resolves to OUR answer and nothing else.
    const string KbKeyword = "widgetzz";
    const string KbAnswer = "Our automated knowledge-base reply for widgetzz.";

    static Db Fresh() => TestEnv.NewMigratedDb();

    // Gotcha 1: FK ON — a real user must exist before a row references user_id.
    static long SeedUser(Db db, string email = "route@test.io")
    {
        db.Execute("INSERT INTO users(email,first_name,role,status) VALUES(?,?,?,?)",
                   email, "Test", "student", "active");
        return db.Scalar<long>("SELECT id FROM users WHERE email=?", email);
    }

    /// <summary>Seed one open conversation and return its id. reference is UNIQUE, so it is always distinct.</summary>
    static long SeedConversation(Db db, string channel = "email", string? received = null, string? tags = null,
        long? userId = null, long? certId = null, string? customerEmail = "cust@test.io", string subject = "Enquiry")
    {
        var reference = "REF-" + Guid.NewGuid().ToString("n").Substring(0, 10);
        return db.ExecuteReturningId(@"INSERT INTO comm_conversations
            (reference,channel,subject,customer_email,received_address,tags,user_id,certification_id,status,priority)
            VALUES(?,?,?,?,?,?,?,?, 'open','normal')",
            reference, channel, subject, customerEmail, received, tags, userId, certId);
    }

    /// <summary>Seed one routing rule; unset match_* fields (null) make that dimension a wildcard, so a rule with
    /// every match field null is a catch-all. Returns the new rule id (used to pin routed_rule_id).</summary>
    static long SeedRule(Db db, string name, int priority = 100, string? matchChannel = null, string? matchAddr = null,
        string? matchKeywords = null, long? matchCertId = null, string? setCategory = null, string? setPriority = null,
        long? assignAdmin = null, int? slaHours = null, string? addTags = null, int escalate = 0, int active = 1)
        => db.ExecuteReturningId(@"INSERT INTO comm_routing_rules
            (name,priority,match_channel,match_received_address,match_keywords,match_certification_id,
             set_category,set_priority,assign_admin_id,sla_hours,add_tags,escalate,active)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?)",
            name, priority, matchChannel, matchAddr, matchKeywords, matchCertId,
            setCategory, setPriority, assignAdmin, slaHours, addTags, escalate, active);

    static void SeedKb(Db db, string keyword = KbKeyword, string answer = KbAnswer)
        => db.Execute("INSERT INTO chat_kb(question,answer,keywords,enabled,sort_order) VALUES(?,?,?,1,0)",
                      "What is " + keyword + "?", answer, keyword);

    static Dictionary<string, object?> Conv(Db db, long id)
        => db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", id)!;

    static long OutMessages(Db db, long convId)
        => db.Scalar<long>("SELECT COUNT(*) FROM comm_inbound_messages WHERE conversation_id=? AND direction='out'", convId);

    static long OutboxRows(Db db, long convId)
        => db.Scalar<long>("SELECT COUNT(*) FROM comm_outbox WHERE conversation_id=?", convId);

    // ================================================================================================
    //  Sensitive-topic floor — always overrides toward a human, and NEVER auto-answers
    // ================================================================================================

    [Fact]
    public void Sensitive_KeywordForcesEscalation_RegardlessOfAnyRule()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // No routing rule at all; the built-in floor ("refund") alone must escalate the conversation.
        CommsRouting.Apply(db, cid, "Hello, I would like a refund for my order.");
        var c = Conv(db, cid);
        Assert.Equal("escalated", H.Str(c["status"]));
        Assert.Equal("high", H.Str(c["priority"]));
        Assert.Contains("sensitive", H.Str(c["tags"]));
        Assert.Contains("escalated", H.Str(c["tags"]));
    }

    [Fact]
    public void Sensitive_IsNeverAutoAnswered_EvenInAutoModeWithAKbMatch()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        SeedKb(db);
        Settings.Put(db, "comms_faq_mode", "auto");
        // The message hits the KB keyword AND a sensitive keyword ("appeal"): the floor wins, so despite a
        // confident KB answer and auto mode, nothing is sent and nothing is even drafted.
        CommsRouting.Apply(db, cid, "widgetzz — and I want to appeal this decision.");
        var c = Conv(db, cid);
        Assert.Equal("escalated", H.Str(c["status"]));
        Assert.Equal(0L, H.L(c["auto_answered"]));
        Assert.Null(H.Str(c["suggested_response"]));
        Assert.Equal(0L, OutMessages(db, cid));
    }

    [Fact]
    public void Sensitive_AdminAddedKeyword_AlsoForcesEscalation()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // The built-in list is a floor; admins extend it via the comma-separated setting. "unicorn" is not a
        // built-in sensitive term, so escalation here proves the admin addition is honoured.
        Settings.Put(db, "comms_sensitive_keywords", "unicorn");
        CommsRouting.Apply(db, cid, "A quick unicorn matter to raise.");
        var c = Conv(db, cid);
        Assert.Equal("escalated", H.Str(c["status"]));
        Assert.Contains("sensitive", H.Str(c["tags"]));
    }

    [Fact]
    public void NonSensitive_PlainMessage_IsNotEscalated()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // No rule, no sensitive term → the conversation keeps its open/normal classification and no guard tags.
        CommsRouting.Apply(db, cid, "Just checking in on my application progress.");
        var c = Conv(db, cid);
        Assert.Equal("open", H.Str(c["status"]));
        Assert.Equal("normal", H.Str(c["priority"]));
        Assert.DoesNotContain("escalated", H.Str(c["tags"]) ?? "");
        Assert.DoesNotContain("sensitive", H.Str(c["tags"]) ?? "");
    }

    // ================================================================================================
    //  First-match rule precedence — ORDER BY priority, id
    // ================================================================================================

    [Fact]
    public void FirstMatch_LowestPriorityNumberWins()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // Both rules match the keyword; priority 10 sorts ahead of 50, so its classification is the one applied.
        var win = SeedRule(db, "A", priority: 10, matchKeywords: "billing", setCategory: "cat_a");
        SeedRule(db, "B", priority: 50, matchKeywords: "billing", setCategory: "cat_b");
        CommsRouting.Apply(db, cid, "I have a billing query.");
        var c = Conv(db, cid);
        Assert.Equal("cat_a", H.Str(c["category"]));
        Assert.Equal(win, H.L(c["routed_rule_id"]));
    }

    [Fact]
    public void FirstMatch_PriorityTie_LowestIdWins()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // Equal priority → the id tiebreaker (ORDER BY priority, id) picks the first-inserted rule.
        var first = SeedRule(db, "first", priority: 10, matchKeywords: "billing", setCategory: "first");
        SeedRule(db, "second", priority: 10, matchKeywords: "billing", setCategory: "second");
        CommsRouting.Apply(db, cid, "A billing question please.");
        var c = Conv(db, cid);
        Assert.Equal("first", H.Str(c["category"]));
        Assert.Equal(first, H.L(c["routed_rule_id"]));
    }

    [Fact]
    public void NoRuleMatches_LeavesClassificationAndRuleIdUnset()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        // The only rule requires a keyword the message does not contain → no rule applies.
        SeedRule(db, "invoices", matchKeywords: "invoice", setCategory: "finance");
        CommsRouting.Apply(db, cid, "Totally unrelated wording here.");
        var c = Conv(db, cid);
        Assert.Null(H.Str(c["category"]));
        Assert.Null(c["routed_rule_id"]);
    }

    // ================================================================================================
    //  Match dimensions — channel and received-address gating
    // ================================================================================================

    [Fact]
    public void Rule_ChannelMismatch_DoesNotApply()
    {
        var db = Fresh();
        var cid = SeedConversation(db, channel: "email");
        // Rule is scoped to WhatsApp; an email conversation must skip it even though the keyword matches.
        SeedRule(db, "wa-only", matchChannel: "whatsapp", matchKeywords: "billing", setCategory: "wa");
        CommsRouting.Apply(db, cid, "a billing note");
        var c = Conv(db, cid);
        Assert.Null(c["routed_rule_id"]);
        Assert.Null(H.Str(c["category"]));
    }

    [Fact]
    public void Rule_ReceivedAddressSubstring_Matches()
    {
        var db = Fresh();
        var cid = SeedConversation(db, received: "exams@pci.io");
        // match_received_address is a case-insensitive substring of the received/to address.
        var rid = SeedRule(db, "exam-inbox", matchAddr: "exams@", setCategory: "exam-support");
        CommsRouting.Apply(db, cid, "hello there");
        var c = Conv(db, cid);
        Assert.Equal("exam-support", H.Str(c["category"]));
        Assert.Equal(rid, H.L(c["routed_rule_id"]));
    }

    // ================================================================================================
    //  FAQ modes — suggest / auto / escalate_all produce the corresponding action
    // ================================================================================================

    [Fact]
    public void SuggestMode_Default_StoresSuggestedResponse_ButSendsNothing()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        SeedKb(db);
        // Default mode (comms_faq_mode unset → "suggest"): the KB answer is drafted for an agent, never sent.
        CommsRouting.Apply(db, cid, "widgetzz");
        var c = Conv(db, cid);
        Assert.Equal(KbAnswer, H.Str(c["suggested_response"]));
        Assert.Equal(0L, H.L(c["auto_answered"]));
        Assert.Equal(0L, OutMessages(db, cid));
    }

    [Fact]
    public void AutoMode_KbMatch_SendsTheAutomatedAnswer()
    {
        var db = Fresh();
        var uid = SeedUser(db);
        var cid = SeedConversation(db, userId: uid, customerEmail: "route@test.io");
        SeedKb(db);
        Settings.Put(db, "comms_faq_mode", "auto");
        // auto mode with a confident, non-sensitive KB match: an outbox row is queued, the 'out' transcript row
        // is written, auto_answered flips, and the draft is cleared.
        CommsRouting.Apply(db, cid, "widgetzz");
        var c = Conv(db, cid);
        Assert.Equal(1L, H.L(c["auto_answered"]));
        Assert.Null(H.Str(c["suggested_response"]));
        Assert.Equal(1L, OutMessages(db, cid));
        Assert.Equal(1L, OutboxRows(db, cid));
    }

    [Fact]
    public void EscalateAllMode_SuppressesTheFaq_ButDoesNotEscalateTheConversation()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        SeedKb(db);
        Settings.Put(db, "comms_faq_mode", "escalate_all");
        // escalate_all leaves every conversation for a human: no draft, no send. It is a FAQ policy, not a
        // sensitive-topic hit, so the conversation itself is not force-escalated (status stays open).
        CommsRouting.Apply(db, cid, "widgetzz");
        var c = Conv(db, cid);
        Assert.Equal(0L, H.L(c["auto_answered"]));
        Assert.Null(H.Str(c["suggested_response"]));
        Assert.Equal(0L, OutMessages(db, cid));
        Assert.Equal("open", H.Str(c["status"]));
    }

    [Fact]
    public void RuleEscalate_NonSensitive_EscalatesAndBlocksAutoAnswer()
    {
        var db = Fresh();
        var cid = SeedConversation(db);
        SeedKb(db);
        Settings.Put(db, "comms_faq_mode", "auto");
        // A rule with escalate=1 forces a human even without a sensitive keyword: high priority, escalated
        // status and tag, and the FAQ engine short-circuits — but "escalated" here is not "sensitive".
        SeedRule(db, "vip-escalate", matchKeywords: "billing", escalate: 1);
        CommsRouting.Apply(db, cid, "billing widgetzz");
        var c = Conv(db, cid);
        Assert.Equal("escalated", H.Str(c["status"]));
        Assert.Equal("high", H.Str(c["priority"]));
        Assert.Equal("escalated", H.Str(c["tags"]));
        Assert.DoesNotContain("sensitive", H.Str(c["tags"]) ?? "");
        Assert.Equal(0L, H.L(c["auto_answered"]));
        Assert.Equal(0L, OutMessages(db, cid));
    }

    // ================================================================================================
    //  Tag merge — union of existing + rule + guard tags, de-duplicated, order-preserving
    // ================================================================================================

    [Fact]
    public void TagMerge_RuleTagsAreDedupedAgainstExistingTags()
    {
        var db = Fresh();
        var cid = SeedConversation(db, tags: "vip");
        // The rule adds "vip,gold"; "vip" already exists, so the merged set keeps a single "vip" and appends
        // "gold" in order — no duplicate, no escalation tags (nothing sensitive, escalate=0).
        SeedRule(db, "tagger", matchKeywords: "billing", addTags: "vip,gold");
        CommsRouting.Apply(db, cid, "a billing note");
        Assert.Equal("vip,gold", H.Str(Conv(db, cid)["tags"]));
    }

    [Fact]
    public void TagMerge_AutoAddedSensitiveAndEscalatedTagsAreDeduped()
    {
        var db = Fresh();
        var cid = SeedConversation(db, tags: "sensitive");
        // Pre-existing "sensitive" + a rule that adds "escalated" + the guard-added "sensitive"/"escalated"
        // (the message is sensitive) must collapse to exactly one of each, order-preserving.
        SeedRule(db, "dup-tags", matchKeywords: "refund", addTags: "escalated");
        CommsRouting.Apply(db, cid, "please process my refund");
        Assert.Equal("sensitive,escalated", H.Str(Conv(db, cid)["tags"]));
    }
}
