using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Self-hosted site chat: a bot answers first from the admin-managed knowledge base (chat_kb);
/// the visitor can escalate to a live person at any time; every conversation is stored as a full
/// transcript and traceable in the admin console (admin-chat.html). No third-party chat services.
/// Abuse control mirrors Forum.cs: a salted, truncated IP hash (never returned to clients) driving
/// fixed-window rate limits via the forum_actions ledger, plus a hard per-session message cap.
/// </summary>
public static class Chat
{
    const int MAX_VISITOR_MSGS_PER_SESSION = 30;   // hard cap per conversation
    const int MAX_MSGS_PER_HOUR_PER_IP = 60;       // fixed-window per ip hash
    const int MAX_STARTS_PER_HOUR_PER_IP = 10;

    const string QUEUE_MSG = "You're in the queue — a member of the team will reply here. You can also leave the page and come back with this same chat.";
    const string FALLBACK_MSG = "I'm sorry — I don't have a good answer for that yet. You could try the FAQ page, or ask to talk to a person and a member of the team will reply here in this chat.";
    const string CLOSED_MSG = "This chat has been closed by the team. Thank you for talking to us — you can start a new chat any time.";

    // Handoff intent: any of these as a whole word in a visitor message moves the session to the queue.
    static readonly string[] HandoffWords = { "human", "agent", "person", "someone", "live" };

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        IResult Err(string code, int status) => Results.Json(new { error = code }, statusCode: status);

        // Salted, truncated IP hash (same construction as Forum.cs): buckets rate limits without
        // storing a reversible identifier. NEVER selected back out to any client, including admins.
        var salt = Environment.GetEnvironmentVariable("CHAT_SALT")
                   ?? Environment.GetEnvironmentVariable("FORUM_SALT") ?? "pci-forum-salt-v1";
        string IpHash(HttpRequest req) => Security.Sha(
            H.LastHopIp(req.Headers["x-forwarded-for"].ToString(), req.HttpContext.Connection.RemoteIpAddress?.ToString())
            + "|" + salt)[..16];

        long CountSince(string ipHash, string action, string window) =>
            db.Scalar<long>($"SELECT COUNT(*) FROM forum_actions WHERE ip_hash=? AND action=? AND created_at > datetime('now','{window}')", ipHash, action);
        void RecordAction(string ipHash, string action) =>
            db.Execute("INSERT INTO forum_actions(ip_hash,action) VALUES(?,?)", ipHash, action);

        // ─────────────────────────── PUBLIC: start a conversation ───────────────────────────
        app.MapPost("/api/chat/start", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var name = Clean(H.GetS(b, "name") ?? "");
            if (name.Length > 60) name = name[..60];
            var ip = IpHash(req);
            if (CountSince(ip, "chat_start", "-1 hours") >= MAX_STARTS_PER_HOUR_PER_IP)
                return Err("rate_limited", 429);
            var token = Security.RandomHex(16);
            var greeting = (name.Length > 0 ? $"Hello {name}! " : "Hello! ")
                + "I'm the PCI Assistant. Ask me about the PCI PCL-AI, PFL-AI and PML-AI credentials, eligibility, fees, enrolment or membership — or ask to talk to a person at any time.";
            long id = 0;
            db.Transaction(() =>
            {
                id = db.ExecuteReturningId("INSERT INTO chat_sessions(token,visitor_name,ip_hash) VALUES(?,?,?)",
                    token, name.Length > 0 ? name : null, ip);
                db.Execute("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'bot', ?)", id, greeting);
                RecordAction(ip, "chat_start");
            });
            log(null, "chat_start", id.ToString());
            return J(new { ok = true, token, greeting, status = "bot" });
        });

        // ─────────────────────────── PUBLIC: send a message ───────────────────────────
        app.MapPost("/api/chat/send", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var token = (H.GetS(b, "token") ?? "").Trim();
            var body = Clean(H.GetS(b, "body") ?? "");
            var escalate = b.TryGetValue("escalate", out var e) && e.ValueKind == JsonValueKind.True;
            var s = token.Length == 0 ? null : db.QueryOne("SELECT id,status FROM chat_sessions WHERE token=?", token);
            if (s is null) return Err("not_found", 404);
            var status = s["status"] as string ?? "bot";
            if (status == "closed") return Err("closed", 409);
            if (body.Length > 2000 || (body.Length < 1 && !escalate)) return Err("bad_body", 400);
            var sid = H.L(s["id"]);
            var ip = IpHash(req);
            if (db.Scalar<long>("SELECT COUNT(*) FROM chat_messages WHERE session_id=? AND sender='visitor'", sid) >= MAX_VISITOR_MSGS_PER_SESSION)
                return Err("rate_limited", 429);
            if (CountSince(ip, "chat_msg", "-1 hours") >= MAX_MSGS_PER_HOUR_PER_IP)
                return Err("rate_limited", 429);

            var replies = new List<object>();
            db.Transaction(() =>
            {
                if (body.Length > 0)
                    db.Execute("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'visitor', ?)", sid, body);
                RecordAction(ip, "chat_msg");
                db.Execute("UPDATE chat_sessions SET last_activity_at=datetime('now') WHERE id=?", sid);

                if (status == "bot")
                {
                    if (escalate || WantsHuman(body))
                    {
                        db.Execute("UPDATE chat_sessions SET status='waiting' WHERE id=?", sid);
                        status = "waiting";
                        var mid = db.ExecuteReturningId("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'bot', ?)", sid, QUEUE_MSG);
                        replies.Add(new { id = mid, sender = "bot", body = QUEUE_MSG });
                        // Alert the notification recipients that a visitor is waiting for a live person.
                        var vName = System.Net.WebUtility.HtmlEncode(db.Scalar<string>("SELECT visitor_name FROM chat_sessions WHERE id=?", sid) ?? "A visitor");
                        var vMsg = System.Net.WebUtility.HtmlEncode(body);
                        Notify.Alert(db, "chat", $"Live chat request from {vName}",
                            $"<p><strong>{vName}</strong> has asked to speak with a person in the website chat.</p>" +
                            $"<p><strong>Their message:</strong><br/>{vMsg}</p>" +
                            "<p>Open the live-chat console to reply.</p>", "chat_session", sid);
                    }
                    else
                    {
                        // Small talk first (greetings / thanks / farewells), then the knowledge base,
                        // then a helpful fallback — so a friendly "hi" never gets the "no answer" reply.
                        var answer = SmallTalk(body) ?? MatchKb(db, body) ?? FALLBACK_MSG;
                        var mid = db.ExecuteReturningId("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'bot', ?)", sid, answer);
                        replies.Add(new { id = mid, sender = "bot", body = answer });
                    }
                }
                // status 'waiting' / 'live': just store — an agent replies from the admin console.
            });
            log(null, "chat_msg", sid.ToString());
            return J(new { ok = true, status, replies });
        });

        // ─────────────────────────── PUBLIC: poll for new messages ───────────────────────────
        app.MapGet("/api/chat/poll", (HttpRequest req) =>
        {
            var token = req.Query["token"].ToString().Trim();
            var after = long.TryParse(req.Query["after"].ToString(), out var a) ? a : 0;
            var s = token.Length == 0 ? null : db.QueryOne("SELECT id,status,visitor_name FROM chat_sessions WHERE token=?", token);
            if (s is null) return Err("not_found", 404);
            var messages = db.Query("SELECT id,sender,body,created_at FROM chat_messages WHERE session_id=? AND id>? ORDER BY id ASC", s["id"], after);
            return J(new { status = s["status"], visitor_name = s["visitor_name"], messages });
        });

        // ─────────────────────────── ADMIN: tracing (gated: content, like Forum/Reviews) ───────────────────────────
        // ip_hash (and the visitor's bearer token) are NEVER selected — not even for admins.
        app.MapGet("/api/admin/chat/sessions", (HttpRequest req) => gate(req, "inbox", _ =>
        {
            var status = req.Query["status"].ToString();
            var hasStatus = status is "bot" or "waiting" or "live" or "closed";
            var where = hasStatus ? "WHERE s.status=?" : "";
            var args = hasStatus ? new object?[] { status } : Array.Empty<object?>();
            var sessions = db.Query($@"SELECT s.id,s.visitor_name,s.status,s.created_at,s.last_activity_at,
                    (SELECT COUNT(*) FROM chat_messages m WHERE m.session_id=s.id) AS message_count,
                    (SELECT COUNT(*) FROM chat_messages m WHERE m.session_id=s.id AND m.sender='visitor') AS visitor_count,
                    (SELECT m.body   FROM chat_messages m WHERE m.session_id=s.id ORDER BY m.id DESC LIMIT 1) AS last_message,
                    (SELECT m.sender FROM chat_messages m WHERE m.session_id=s.id ORDER BY m.id DESC LIMIT 1) AS last_sender
                FROM chat_sessions s {where}
                ORDER BY (s.status='waiting') DESC, s.last_activity_at DESC, s.id DESC LIMIT 200", args);
            return J(new
            {
                sessions,
                counts = new
                {
                    bot = db.Scalar<long>("SELECT COUNT(*) FROM chat_sessions WHERE status='bot'"),
                    waiting = db.Scalar<long>("SELECT COUNT(*) FROM chat_sessions WHERE status='waiting'"),
                    live = db.Scalar<long>("SELECT COUNT(*) FROM chat_sessions WHERE status='live'"),
                    closed = db.Scalar<long>("SELECT COUNT(*) FROM chat_sessions WHERE status='closed'"),
                }
            });
        }));

        app.MapGet("/api/admin/chat/sessions/{id}", (HttpRequest req, long id) => gate(req, "inbox", _ =>
        {
            var s = db.QueryOne("SELECT id,visitor_name,status,created_at,last_activity_at FROM chat_sessions WHERE id=?", id);
            if (s is null) return Err("not_found", 404);
            var messages = db.Query("SELECT id,sender,body,created_at FROM chat_messages WHERE session_id=? ORDER BY id ASC", id);
            return J(new { session = s, messages });
        }));

        app.MapPost("/api/admin/chat/sessions/{id}/reply", (HttpRequest req, long id) => gate(req, "inbox", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var body = Clean(H.GetS(b, "body") ?? "");
            if (body.Length is < 1 or > 4000) return Err("bad_body", 400);
            var s = db.QueryOne("SELECT id,status FROM chat_sessions WHERE id=?", id);
            if (s is null) return Err("not_found", 404);
            long mid = 0;
            db.Transaction(() =>
            {
                mid = db.ExecuteReturningId("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'agent', ?)", id, body);
                db.Execute("UPDATE chat_sessions SET status='live', last_activity_at=datetime('now') WHERE id=?", id);
            });
            log(a.Id, "chat_reply", id.ToString());
            return J(new { ok = true, id = mid, status = "live" });
        }));

        app.MapPost("/api/admin/chat/sessions/{id}/close", (HttpRequest req, long id) => gate(req, "inbox", a =>
        {
            var s = db.QueryOne("SELECT id,status FROM chat_sessions WHERE id=?", id);
            if (s is null) return Err("not_found", 404);
            db.Transaction(() =>
            {
                if ((s["status"] as string) != "closed")
                    db.Execute("INSERT INTO chat_messages(session_id,sender,body) VALUES(?, 'bot', ?)", id, CLOSED_MSG);
                db.Execute("UPDATE chat_sessions SET status='closed', last_activity_at=datetime('now') WHERE id=?", id);
            });
            log(a.Id, "chat_close", id.ToString());
            return J(new { ok = true, status = "closed" });
        }));

        // ─────────────────────────── ADMIN: knowledge base CRUD (gated: content) ───────────────────────────
        app.MapGet("/api/admin/chat/kb", (HttpRequest req) => gate(req, "inbox", _ =>
            J(new { rows = db.Query("SELECT id,question,answer,keywords,enabled,sort_order FROM chat_kb ORDER BY sort_order, id") })));

        app.MapPost("/api/admin/chat/kb", (HttpRequest req) => gate(req, "inbox", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var question = Clean(H.GetS(b, "question") ?? "");
            var answer = Clean(H.GetS(b, "answer") ?? "");
            var keywords = Clean(H.GetS(b, "keywords") ?? "");
            if (question.Length is < 3 or > 300) return Err("bad_question", 400);
            if (answer.Length is < 3 or > 4000) return Err("bad_answer", 400);
            var sort = (long)(H.GetNum(b, "sort_order") ?? 0);
            var id = db.ExecuteReturningId("INSERT INTO chat_kb(question,answer,keywords,enabled,sort_order) VALUES(?,?,?,1,?)",
                question, answer, keywords, sort);
            log(a.Id, "chat_kb_add", id.ToString());
            return J(new { ok = true, id });
        }));

        app.MapPost("/api/admin/chat/kb/{id}", (HttpRequest req, long id) => gate(req, "inbox", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var action = H.GetS(b, "action");
            var row = db.QueryOne("SELECT id,enabled FROM chat_kb WHERE id=?", id);
            if (row is null) return Err("not_found", 404);
            switch (action)
            {
                case "update":
                    foreach (var col in new[] { "question", "answer", "keywords" })
                        if (b.ContainsKey(col)) db.Execute($"UPDATE chat_kb SET {col}=? WHERE id=?", Clean(H.GetS(b, col) ?? ""), id);
                    if (b.ContainsKey("sort_order")) db.Execute("UPDATE chat_kb SET sort_order=? WHERE id=?", (long)(H.GetNum(b, "sort_order") ?? 0), id);
                    if (b.ContainsKey("enabled")) db.Execute("UPDATE chat_kb SET enabled=? WHERE id=?", (long)(H.GetNum(b, "enabled") ?? 1), id);
                    break;
                case "toggle":
                    db.Execute("UPDATE chat_kb SET enabled=CASE WHEN enabled=1 THEN 0 ELSE 1 END WHERE id=?", id);
                    break;
                case "delete":
                    db.Execute("DELETE FROM chat_kb WHERE id=?", id);
                    break;
                default: return Err("bad_action", 400);
            }
            log(a.Id, "chat_kb_" + action, id.ToString());
            return J(new { ok = true });
        }));
    }

    // ─────────────────────────── bot brain ───────────────────────────
    // Case-insensitive keyword scoring over enabled chat_kb rows: single-word keywords must match a
    // whole token of the message; multi-word keywords count when the message contains the phrase;
    // a message that contains (or is contained by) the row's question gets a bonus. Best score >= 1 wins.
    const string SMALLTALK_MENU = " I can help with the PCI PCL-AI, PFL-AI and PML-AI credentials, the thirteen domains, eligibility, fees, enrolment, the exam or membership — what would you like to know? You can also ask to talk to a person any time.";

    /// <summary>Handle greetings, thanks and farewells conversationally so a friendly "hi" or "how are you"
    /// never gets the "no answer" fallback. Returns null when the message isn't small talk (so the KB runs).</summary>
    internal static string? SmallTalk(string message)
    {
        var msg = message.ToLowerInvariant().Trim().TrimEnd('!', '.', '?', ' ');
        var tokens = Tokens(msg);
        bool Has(params string[] ws) => ws.Any(w => tokens.Contains(w));
        // Thanks
        if (Has("thanks", "thank", "thankyou", "cheers", "appreciated", "ta"))
            return "You're very welcome. Is there anything else I can help you with?";
        // Farewells
        if (Has("bye", "goodbye", "cya") || msg is "see you" or "see ya")
            return "Thanks for chatting — take care, and come back any time.";
        // "how are you" / wellbeing
        if (msg.Contains("how are you") || msg.Contains("how's it going") || msg.Contains("how you doing") || msg.Contains("hows it going"))
            return "I'm doing well, thank you — ready to help." + SMALLTALK_MENU;
        // "who/what are you"
        if (msg.Contains("who are you") || msg.Contains("what are you") || msg.Contains("your name"))
            return "I'm the PCI Assistant — an automated helper for the Project Controls Institute." + SMALLTALK_MENU;
        // Bare greetings (only when the message is essentially just a greeting, so "hi, what are the fees?" still hits the KB)
        if (tokens.Count <= 3 && Has("hi", "hello", "hey", "hiya", "heya", "yo", "greetings", "morning", "afternoon", "evening", "salaam", "salam", "hola"))
            return "Hello! 👋" + SMALLTALK_MENU;
        // Open-ended help asks with no specific topic
        if (tokens.Count <= 4 && (msg.Contains("help") || msg.Contains("hi there")) && !msg.Contains("exam") && !msg.Contains("fee") && !msg.Contains("enrol"))
            return "Happy to help." + SMALLTALK_MENU;
        return null;
    }

    internal static string? MatchKb(Db db, string message)
    {
        var msg = message.ToLowerInvariant();
        var tokens = Tokens(msg);
        if (tokens.Count == 0) return null;
        string? best = null; var bestScore = 0;
        foreach (var row in db.Query("SELECT question,answer,keywords FROM chat_kb WHERE enabled=1 ORDER BY sort_order, id"))
        {
            var score = 0;
            foreach (var kwRaw in (H.Str(row["keywords"]) ?? "").Split(','))
            {
                var kw = kwRaw.Trim().ToLowerInvariant();
                if (kw.Length == 0) continue;
                if (kw.Contains(' ') ? msg.Contains(kw) : tokens.Contains(kw)) score++;
            }
            var q = (H.Str(row["question"]) ?? "").ToLowerInvariant().TrimEnd('?', '.', ' ');
            if (q.Length > 0 && (msg.Contains(q) || (msg.Length >= 8 && q.Contains(msg.TrimEnd('?', '.', ' '))))) score += 2;
            if (score > bestScore) { bestScore = score; best = H.Str(row["answer"]); }
        }
        return bestScore >= 1 ? best : null;
    }

    internal static bool WantsHuman(string body)
    {
        var tokens = Tokens(body.ToLowerInvariant());
        return HandoffWords.Any(tokens.Contains);
    }

    // Tokens keep letters/digits/hyphens so "pcp-ai" stays one word and "ai" can't match inside "waiting".
    static HashSet<string> Tokens(string lower) =>
        System.Text.RegularExpressions.Regex.Split(lower, @"[^a-z0-9\-]+")
            .Where(t => t.Length > 0).ToHashSet();

    static string Clean(string s) =>
        new string(s.Replace("\r\n", "\n").Where(c => !char.IsControl(c) || c is '\n' or '\t').ToArray()).Trim();
}
