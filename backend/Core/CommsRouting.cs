using PCI.Backend.Data;
using PCI.Backend.Endpoints;

namespace PCI.Backend.Core;

/// <summary>
/// Customer-service auto-triage for the unified inbox (Comms Centre Phase 5). When an inbound message
/// arrives (email or WhatsApp) this classifies and routes the conversation, and — where safe — drafts or
/// sends an automated FAQ answer.
///
/// SAFETY (per spec §9/§15): keyword rules NEVER make adverse decisions. They only classify (category,
/// priority, assignment, SLA, tags) and route. Anything touching a sensitive/adverse topic — rejection,
/// refund, revocation, exam-result disputes, identity, criminal, legal, privacy, security, financial
/// waivers/complaints — is force-escalated to a human and is NEVER auto-answered. The FAQ engine only
/// ever replays admin-authored knowledge-base answers (chat_kb); it invents nothing.
/// </summary>
public static class CommsRouting
{
    // Topics that must always reach a human and must never be auto-answered by a bot. These are matched as
    // case-insensitive substrings/tokens against the inbound text. Admins can extend via the
    // comms_sensitive_keywords setting (comma-separated); this built-in list is always applied as a floor.
    static readonly string[] SensitiveDefaults = new[]
    {
        "reject", "rejected", "rejection", "appeal", "refund", "chargeback", "revoke", "revoked", "revocation",
        "dispute", "disputed", "complaint", "complain", "legal", "lawyer", "solicitor", "lawsuit", "sue", "court",
        "criminal", "fraud", "scam", "identity", "passport", "gdpr", "data protection", "privacy", "erasure",
        "delete my data", "right to be forgotten", "breach", "hacked", "security", "waiver", "exam result",
        "failed exam", "exam fail", "cheat", "misconduct", "discriminat", "harass", "safeguarding"
    };

    /// <summary>Read the merged sensitive-topic keyword list (built-in floor + admin additions).</summary>
    static IEnumerable<string> SensitiveKeywords(Db db)
    {
        var extra = Settings.Str(db, "comms_sensitive_keywords", "");
        foreach (var k in SensitiveDefaults) yield return k;
        foreach (var k in extra.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            yield return k.ToLowerInvariant();
    }

    static bool IsSensitive(Db db, string text)
    {
        var lower = (text ?? "").ToLowerInvariant();
        return SensitiveKeywords(db).Any(k => k.Length > 0 && lower.Contains(k));
    }

    // Does a comma-separated keyword spec match the message? Multi-word entries match as a phrase,
    // single words match a whole token, mirroring the KB matcher so behaviour is consistent.
    static bool KeywordsMatch(string? spec, string lower)
    {
        if (string.IsNullOrWhiteSpace(spec)) return false;
        var tokens = System.Text.RegularExpressions.Regex.Split(lower, @"[^a-z0-9\-]+").Where(t => t.Length > 0).ToHashSet();
        foreach (var raw in spec.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var kw = raw.ToLowerInvariant();
            if (kw.Length == 0) continue;
            if (kw.Contains(' ') ? lower.Contains(kw) : tokens.Contains(kw)) return true;
        }
        return false;
    }

    /// <summary>
    /// Triage an inbound message on a conversation: apply the first matching routing rule, enforce the
    /// sensitive-topic escalation guard, then run the FAQ engine per the configured mode. Best-effort:
    /// never throws into the webhook path.
    /// </summary>
    public static void Apply(Db db, long conversationId, string messageText)
    {
        try { ApplyCore(db, conversationId, messageText ?? ""); }
        catch (Exception e) { Console.Error.WriteLine($"[comms-routing] conv {conversationId}: {e.Message}"); }
    }

    static void ApplyCore(Db db, long convId, string text)
    {
        var c = db.QueryOne("SELECT * FROM comm_conversations WHERE id=?", convId);
        if (c is null) return;
        var channel = H.Str(c["channel"]) ?? "email";
        var received = (H.Str(c["received_address"]) ?? "").ToLowerInvariant();
        var certId = c.TryGetValue("certification_id", out var cv) ? cv as long? : null;
        var lower = text.ToLowerInvariant();

        // ── 1. Routing rules (classification only) — first match by priority, then id ──
        long? matchedRule = null;
        string? setCategory = null, setPriority = null, addTags = null;
        long? assignAdmin = null; int? slaHours = null; bool ruleEscalate = false;
        foreach (var r in db.Query("SELECT * FROM comm_routing_rules WHERE active=1 ORDER BY priority, id"))
        {
            var mChan = H.Str(r["match_channel"]);
            if (!string.IsNullOrEmpty(mChan) && !string.Equals(mChan, channel, StringComparison.OrdinalIgnoreCase)) continue;
            var mAddr = (H.Str(r["match_received_address"]) ?? "").ToLowerInvariant();
            if (mAddr.Length > 0 && !received.Contains(mAddr)) continue;
            var mCert = r.TryGetValue("match_certification_id", out var mcv) ? mcv as long? : null;
            if (mCert is not null && mCert != certId) continue;
            var mKw = H.Str(r["match_keywords"]);
            if (!string.IsNullOrWhiteSpace(mKw) && !KeywordsMatch(mKw, lower)) continue;
            // matched
            matchedRule = H.L(r["id"]);
            setCategory = H.Str(r["set_category"]);
            setPriority = H.Str(r["set_priority"]);
            assignAdmin = r.TryGetValue("assign_admin_id", out var aa) ? aa as long? : null;
            slaHours = r.TryGetValue("sla_hours", out var sh) && sh is not null ? (int?)H.L(sh) : null;
            addTags = H.Str(r["add_tags"]);
            ruleEscalate = H.L(r["escalate"]) == 1;
            break;
        }

        // ── 2. Sensitive-topic guard — always overrides toward human escalation, never auto-answers ──
        var sensitive = IsSensitive(db, text);
        var escalate = ruleEscalate || sensitive;

        // ── 3. Apply routing/classification to the conversation ──
        var newCategory = string.IsNullOrEmpty(setCategory) ? null : setCategory;
        var newPriority = escalate ? "high" : (string.IsNullOrEmpty(setPriority) ? null : setPriority);
        string? slaDue = slaHours is > 0 ? DateTime.UtcNow.AddHours(slaHours.Value).ToString("yyyy-MM-dd HH:mm:ss") : null;
        var tags = MergeTags(H.Str(c["tags"]), addTags, sensitive ? "sensitive" : null, escalate ? "escalated" : null);

        db.Execute(@"UPDATE comm_conversations SET
                category=COALESCE(?,category),
                priority=COALESCE(?,priority),
                assigned_admin_id=COALESCE(?,assigned_admin_id),
                sla_due_at=COALESCE(?,sla_due_at),
                tags=?,
                routed_rule_id=COALESCE(?,routed_rule_id),
                status=CASE WHEN ? = 1 AND status IN('open','pending') THEN 'escalated' ELSE status END,
                updated_at=datetime('now')
            WHERE id=?",
            newCategory, newPriority, assignAdmin, slaDue, tags, matchedRule, escalate ? 1 : 0, convId);

        // ── 4. FAQ auto-response engine ──
        // Modes: suggest (default) = draft for agent review; auto = send the KB answer directly;
        //        escalate_all = never auto-answer, always leave for a human.
        var mode = Settings.Str(db, "comms_faq_mode", "suggest").ToLowerInvariant();
        if (escalate || mode == "escalate_all") return;           // sensitive/escalated: humans only

        var answer = Chat.MatchKb(db, text);
        if (string.IsNullOrWhiteSpace(answer)) return;            // no confident KB match → leave for a human

        if (mode == "auto")
        {
            var reference = H.Str(c["reference"]) ?? ("PCI-CONV-" + convId);
            var subject = "Re: " + (H.Str(c["subject"]) ?? "Your enquiry") + $" [{reference}]";
            var body = channel == "email"
                ? $"<p>{System.Net.WebUtility.HtmlEncode(answer)}</p><p style='color:#666;font-size:13px'>This is an automated answer under reference {reference}. If it doesn't fully resolve your question, just reply and a member of our team will help.</p>"
                : answer + $"\n\nThis is an automated answer under reference {reference}. Reply and a member of our team will help if you need more.";
            var qid = Comms.Enqueue(db, channel == "whatsapp" ? "whatsapp" : "email", $"autofaq:{convId}",
                c["user_id"] as long?, H.Str(c["customer_email"]), H.Str(c["customer_phone"]),
                subject, body, senderKey: "support", category: "support",
                triggerCode: "support.auto_faq", conversationId: convId);
            if (qid is not null)
            {
                db.Execute("INSERT INTO comm_inbound_messages(conversation_id,direction,channel,from_addr,to_addr,body) VALUES(?, 'out', ?, 'PCI Assistant (automated)', ?, ?)",
                    convId, channel, H.Str(c["customer_email"]) ?? H.Str(c["customer_phone"]), answer);
                db.Execute("UPDATE comm_conversations SET auto_answered=1, suggested_response=NULL, last_message_at=datetime('now'), updated_at=datetime('now') WHERE id=?", convId);
                try { OutboxDispatcher.DrainOnce(db, 5); } catch { }
            }
        }
        else // suggest
        {
            db.Execute("UPDATE comm_conversations SET suggested_response=?, updated_at=datetime('now') WHERE id=?", answer, convId);
        }
    }

    // Union of existing + new tags, de-duplicated, comma-separated, order-preserving.
    static string MergeTags(string? existing, params string?[] additions)
    {
        var set = new List<string>();
        void AddAll(string? csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return;
            foreach (var t in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (!set.Any(x => x.Equals(t, StringComparison.OrdinalIgnoreCase))) set.Add(t);
        }
        AddAll(existing);
        foreach (var a in additions) AddAll(a);
        return string.Join(",", set);
    }
}
