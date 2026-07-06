using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Reviews / testimonials (student submit → admin moderate → public publish) plus the MCQ bulk-upload
/// endpoint for the question bank. Kept in one module because both are "content contributed then curated".
/// </summary>
public static class Reviews
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, AdminCtx?> adminFromReq, Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);

        // ─────────────────────────── PUBLIC: published reviews ───────────────────────────
        // Website reviews page + the home-page strip before the footer both read this.
        app.MapGet("/api/reviews", (HttpRequest req) =>
        {
            var featuredOnly = req.Query["featured"].ToString() == "1";
            var limit = int.TryParse(req.Query["limit"].ToString(), out var l) ? Math.Clamp(l, 1, 100) : 100;
            var where = "status='published'" + (featuredOnly ? " AND featured=1" : "");
            var rows = db.Query($"SELECT id,name,designation,company,country,relationship,rating,title,body,featured,published_at FROM reviews WHERE {where} ORDER BY featured DESC, sort_order, published_at DESC, id DESC LIMIT {limit}");
            return J(new { reviews = rows });
        });

        // ─────────────────────────── STUDENT: submit + list own ───────────────────────────
        app.MapPost("/api/me/reviews", async (HttpRequest req) =>
        {
            var u = User(req); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(req);
            var body = (H.GetS(b, "body") ?? "").Trim();
            if (body.Length < 10) return Results.Json(new { error = "review_too_short" }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? ($"{u.FirstName} {u.LastName}").Trim()).Trim();
            var rating = (int)Math.Clamp(H.GetNum(b, "rating") ?? 5, 1, 5);
            var rel = H.GetS(b, "relationship") ?? "student";
            // one pending/published review per user at a time keeps the moderation queue clean
            var existing = db.QueryOne("SELECT id FROM reviews WHERE user_id=? AND status IN ('pending','published') ORDER BY id DESC", u.Id);
            if (existing is not null)
            {
                db.Execute("UPDATE reviews SET name=?, designation=?, company=?, country=?, relationship=?, rating=?, title=?, body=?, status='pending', published_at=NULL WHERE id=?",
                    name, H.GetS(b, "designation"), H.GetS(b, "company"), H.GetS(b, "country"), rel, rating, H.GetS(b, "title"), body, existing["id"]);
                log(u.Id, "review_updated", existing["id"]!.ToString());
                return J(new { ok = true, id = existing["id"], status = "pending", message = "Your review has been updated and is awaiting moderation." });
            }
            var id = db.ExecuteReturningId("INSERT INTO reviews(user_id,name,designation,company,country,relationship,rating,title,body,status) VALUES(?,?,?,?,?,?,?,?,?, 'pending')",
                u.Id, name, H.GetS(b, "designation"), H.GetS(b, "company"), H.GetS(b, "country"), rel, rating, H.GetS(b, "title"), body);
            log(u.Id, "review_submitted", id.ToString());
            return J(new { ok = true, id, status = "pending", message = "Thank you — your review has been submitted for moderation." });
        });

        app.MapGet("/api/me/reviews", (HttpRequest req) =>
        {
            var u = User(req); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var rows = db.Query("SELECT id,name,designation,company,country,relationship,rating,title,body,status,created_at,published_at FROM reviews WHERE user_id=? ORDER BY id DESC", u.Id);
            return J(new { reviews = rows });
        });

        // ─────────────────────────── ADMIN: moderate (gated: content) ───────────────────────────
        app.MapGet("/api/admin/reviews", (HttpRequest req) => gate(req, "content", _ =>
        {
            var status = req.Query["status"].ToString();
            var where = string.IsNullOrEmpty(status) ? "1" : "status=?";
            var rows = string.IsNullOrEmpty(status)
                ? db.Query("SELECT * FROM reviews ORDER BY (status='pending') DESC, id DESC")
                : db.Query("SELECT * FROM reviews WHERE status=? ORDER BY id DESC", status);
            return J(new { reviews = rows,
                counts = new {
                    pending = db.Scalar<long>("SELECT COUNT(*) FROM reviews WHERE status='pending'"),
                    published = db.Scalar<long>("SELECT COUNT(*) FROM reviews WHERE status='published'"),
                    featured = db.Scalar<long>("SELECT COUNT(*) FROM reviews WHERE status='published' AND featured=1")
                } });
        }));

        app.MapPost("/api/admin/reviews/{id}/status", (HttpRequest req, long id) => gate(req, "content", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var status = H.GetS(b, "status");
            if (status is not ("pending" or "published" or "rejected")) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            if (status == "published")
                db.Execute("UPDATE reviews SET status='published', published_at=COALESCE(published_at,datetime('now')), reviewed_by=? WHERE id=?", a.Id, id);
            else
                db.Execute("UPDATE reviews SET status=?, reviewed_by=? WHERE id=?", status, a.Id, id);
            log(a.Id, "review_" + status, id.ToString());
            return J(new { ok = true });
        }));

        // toggle home-page featuring + edit curated fields (order, light copy edits)
        app.MapPatch("/api/admin/reviews/{id}", (HttpRequest req, long id) => gate(req, "content", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            foreach (var (key, col) in new[] { ("featured", "featured"), ("sort_order", "sort_order") })
                if (b.ContainsKey(key)) db.Execute($"UPDATE reviews SET {col}=? WHERE id=?", (long)(H.GetNum(b, key) ?? 0), id);
            foreach (var col in new[] { "name", "designation", "company", "country", "title", "body" })
                if (b.ContainsKey(col)) db.Execute($"UPDATE reviews SET {col}=? WHERE id=?", H.GetS(b, col), id);
            log(a.Id, "review_edited", id.ToString());
            return J(new { ok = true });
        }));

        app.MapDelete("/api/admin/reviews/{id}", (HttpRequest req, long id) => gate(req, "content", a =>
        {
            db.Execute("DELETE FROM reviews WHERE id=?", id);
            log(a.Id, "review_deleted", id.ToString());
            return J(new { ok = true });
        }));

        // ─────────────────────────── ADMIN: MCQ bulk upload (gated: sampleq) ───────────────────────────
        // Accepts either JSON {rows:[{question,option_a..d,answer_index|answer,domain,is_practice,published}]}
        // or a CSV string in {csv:"..."} with a header row. Answer may be 0-3, 1-4, or a letter A-D.
        app.MapPost("/api/admin/sample_questions/bulk", (HttpRequest req) => gate(req, "sampleq", a =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var records = new List<Dictionary<string, string>>();
            if (b.TryGetValue("csv", out var csvEl) && csvEl.ValueKind == JsonValueKind.String)
                records = ParseCsv(csvEl.GetString() ?? "");
            else if (b.TryGetValue("rows", out var rowsEl) && rowsEl.ValueKind == JsonValueKind.Array)
                foreach (var r in rowsEl.EnumerateArray())
                {
                    var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    if (r.ValueKind == JsonValueKind.Object)
                        foreach (var p in r.EnumerateObject())
                            d[p.Name] = p.Value.ValueKind == JsonValueKind.String ? (p.Value.GetString() ?? "") : p.Value.ToString();
                    records.Add(d);
                }
            else return Results.Json(new { error = "no_rows" }, statusCode: 400);

            int inserted = 0; var errors = new List<object>(); int line = 1;
            db.Transaction(() =>
            {
                foreach (var r in records)
                {
                    line++;
                    string G(params string[] keys) { foreach (var k in keys) if (r.TryGetValue(k, out var v) && !string.IsNullOrWhiteSpace(v)) return v.Trim(); return ""; }
                    var q = G("question", "q");
                    var oa = G("option_a", "a", "optiona"); var ob = G("option_b", "b", "optionb");
                    var oc = G("option_c", "c", "optionc"); var od = G("option_d", "d", "optiond");
                    if (q == "" || oa == "" || ob == "") { errors.Add(new { line, error = "missing question or options A/B" }); continue; }
                    var ans = NormaliseAnswer(G("answer_index", "answer", "correct", "key"));
                    if (ans < 0) { errors.Add(new { line, error = "answer must be A-D or 0-3 or 1-4" }); continue; }
                    var domain = G("domain", "topic");
                    var isPractice = ParseBool(G("is_practice", "practice"));
                    var published = r.ContainsKey("published") ? ParseBool(G("published")) : 1;
                    var sort = int.TryParse(G("sort_order", "order"), out var so) ? so : 0;
                    db.Execute(@"INSERT INTO sample_questions(question,option_a,option_b,option_c,option_d,answer_index,domain,published,is_practice,sort_order)
                                 VALUES(?,?,?,?,?,?,?,?,?,?)", q, oa, ob, oc, od, ans, domain, published, isPractice, sort);
                    inserted++;
                }
            });
            log(a.Id, "questions_bulk_upload", $"{inserted} inserted, {errors.Count} skipped");
            return J(new { ok = true, inserted, skipped = errors.Count, errors });
        }));
    }

    // answer accepts A-D (case-insensitive), 0-3 (0-based), or 1-4 (1-based) → 0-based index, or -1 if invalid
    static int NormaliseAnswer(string s)
    {
        s = s.Trim();
        if (s.Length == 0) return -1;
        var up = s.ToUpperInvariant();
        if (up is "A" or "B" or "C" or "D") return up[0] - 'A';
        if (int.TryParse(s, out var n))
        {
            if (n is >= 0 and <= 3) return n;      // treat 0-3 as 0-based
            if (n is >= 1 and <= 4) return n - 1;  // treat 4 (with no 0 seen) as 1-based → but 1-3 ambiguous
        }
        return -1;
    }

    static int ParseBool(string s)
    {
        s = s.Trim().ToLowerInvariant();
        return s is "1" or "true" or "yes" or "y" or "practice" ? 1 : 0;
    }

    // Minimal RFC-4180-ish CSV: header row maps column names; supports quoted fields with commas/quotes.
    static List<Dictionary<string, string>> ParseCsv(string csv)
    {
        var rows = SplitCsvRows(csv);
        var result = new List<Dictionary<string, string>>();
        if (rows.Count == 0) return result;
        var headers = rows[0].Select(h => h.Trim().ToLowerInvariant().Replace(" ", "_")).ToList();
        for (int i = 1; i < rows.Count; i++)
        {
            if (rows[i].Count == 1 && rows[i][0].Trim() == "") continue;
            var d = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            for (int c = 0; c < headers.Count && c < rows[i].Count; c++) d[headers[c]] = rows[i][c];
            result.Add(d);
        }
        return result;
    }

    static List<List<string>> SplitCsvRows(string csv)
    {
        var rows = new List<List<string>>(); var cur = new List<string>(); var field = new System.Text.StringBuilder();
        bool inQ = false;
        for (int i = 0; i < csv.Length; i++)
        {
            char ch = csv[i];
            if (inQ)
            {
                if (ch == '"') { if (i + 1 < csv.Length && csv[i + 1] == '"') { field.Append('"'); i++; } else inQ = false; }
                else field.Append(ch);
            }
            else
            {
                if (ch == '"') inQ = true;
                else if (ch == ',') { cur.Add(field.ToString()); field.Clear(); }
                else if (ch == '\r') { }
                else if (ch == '\n') { cur.Add(field.ToString()); field.Clear(); rows.Add(cur); cur = new List<string>(); }
                else field.Append(ch);
            }
        }
        if (field.Length > 0 || cur.Count > 0) { cur.Add(field.ToString()); rows.Add(cur); }
        return rows;
    }
}
