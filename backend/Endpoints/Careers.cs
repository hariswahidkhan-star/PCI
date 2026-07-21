using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Careers / job board — a fully dynamic careers page driven by the database. The public site lists
/// published, open postings and lets candidates apply in-platform (with a CV upload) or via an external
/// link/email; admins manage postings and review applicants. Nothing about the page is hardcoded: every
/// role, filter value and apply flow comes from job_postings.
/// </summary>
public static class Careers
{
    // Reuse the shared upload flow: decode a data URI, store the bytes, return a storage reference (never raw bytes).
    static (string? reference, string cleanName, string? error) StoreUpload(string? name, string? dataUri, string category)
    {
        var cleanName = (name ?? "cv").Trim();
        if (cleanName.Length > 120) cleanName = cleanName[..120];
        cleanName = string.Concat(cleanName.Where(c => !"\\/:*?\"<>|".Contains(c)));
        var (bytes, mime, err) = Storage.DecodeDataUri(dataUri);
        if (err is not null) return (null, cleanName, err);
        var obj = Storage.Put(bytes!, mime, category);
        return (obj.Reference, cleanName, null);
    }

    static readonly string[] Types = { "full_time", "part_time", "contract", "internship", "temporary" };
    static readonly string[] Remotes = { "onsite", "remote", "hybrid" };
    static readonly string[] QTypes = { "short_text", "long_text", "yesno", "single", "multi", "number", "date", "dropdown", "consent" };
    // Applicant-tracking statuses (Increment 3). The public "My applications" view maps these to
    // candidate-friendly labels; internal notes are never candidate-visible.
    static readonly string[] AppStatuses = { "new", "reviewing", "shortlisted", "interview", "assessment", "offer", "hired", "rejected", "withdrawn", "closed" };
    // Increment 4: admin-managed master data. Categories are fixed (they map to job_postings columns);
    // the values within each category are configured in the Admin Panel (career_taxonomy), never hardcoded.
    static readonly string[] TaxKinds = { "department", "sector", "experience", "location" };
    // Candidate email templates keyed by event; each is admin-editable and can be disabled.
    static readonly string[] TemplateKeys = { "application_received", "status_changed", "interview_scheduled", "message" };

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        // Only postings that are published AND (no close date OR not yet closed).
        const string OpenWhere = "status='published' AND (closes_at IS NULL OR closes_at='' OR closes_at > datetime('now'))";
        static object Row(Dictionary<string, object?> r, bool full) => new Dictionary<string, object?>(r);

        // ─────────────────────────── PUBLIC ───────────────────────────
        app.MapGet("/api/careers", (HttpRequest req) =>
        {
            var q = (req.Query["q"].ToString() ?? "").Trim();
            var type = req.Query["type"].ToString();
            var remote = req.Query["remote"].ToString();
            var sector = req.Query["sector"].ToString();
            var country = req.Query["country"].ToString();
            var department = req.Query["department"].ToString();
            var experience = req.Query["experience"].ToString();
            var sort = req.Query["sort"].ToString();
            var where = OpenWhere; var args = new List<object?>();
            if (q.Length > 0) { where += " AND (title LIKE ? OR organisation LIKE ? OR location LIKE ? OR sector LIKE ? OR department LIKE ? OR job_code LIKE ?)"; var like = "%" + q + "%"; args.AddRange(new object?[] { like, like, like, like, like, like }); }
            if (Types.Contains(type)) { where += " AND employment_type=?"; args.Add(type); }
            if (Remotes.Contains(remote)) { where += " AND remote_type=?"; args.Add(remote); }
            if (!string.IsNullOrWhiteSpace(sector)) { where += " AND sector=?"; args.Add(sector); }
            if (!string.IsNullOrWhiteSpace(country)) { where += " AND country=?"; args.Add(country); }
            if (!string.IsNullOrWhiteSpace(department)) { where += " AND department=?"; args.Add(department); }
            if (!string.IsNullOrWhiteSpace(experience)) { where += " AND experience_level=?"; args.Add(experience); }
            var order = sort switch
            {
                "closing" => "featured DESC, (closes_at IS NULL OR closes_at='') ASC, closes_at ASC, id DESC",
                "title" => "title ASC, id DESC",
                _ => "featured DESC, COALESCE(posted_at,created_at) DESC, id DESC",   // newest / relevance
            };
            var rows = db.Query($@"SELECT id,job_code,title,organisation,location,country,department,employment_type,remote_type,experience_level,sector,
                salary_min,salary_max,salary_currency,salary_period,salary_visible,featured,urgent,vacancies,posted_at,closes_at,apply_method
                FROM job_postings WHERE {where} ORDER BY {order}", args.ToArray());
            List<string?> Facet(string col) => db.Query($"SELECT DISTINCT {col} FROM job_postings WHERE {OpenWhere} AND {col} IS NOT NULL AND {col}<>'' ORDER BY {col}").Select(r => H.Str(r[col])).ToList();
            return J(new { rows, sectors = Facet("sector"), countries = Facet("country"), departments = Facet("department"), total = rows.Count });
        });

        app.MapGet("/api/careers/{id:long}", (long id) =>
        {
            var r = db.QueryOne($@"SELECT id,job_code,slug,title,organisation,location,country,department,employment_type,remote_type,experience_level,sector,
                description,requirements,responsibilities,education,languages,certifications,benefits,reporting_line,vacancies,expected_start,
                application_instructions,eo_statement,salary_min,salary_max,salary_currency,salary_period,salary_visible,urgent,featured,posted_at,closes_at,
                apply_method,apply_url,apply_email FROM job_postings WHERE id=? AND {OpenWhere}", id);
            if (r is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var questions = db.Query("SELECT id,qtype,label,options,required FROM job_questions WHERE job_id=? ORDER BY sort_order,id", id);
            return J(new { job = Row(r, true), jsonld = JobJsonLd(db, r), questions });   // jsonld = Google-for-Jobs JobPosting structured data
        });

        // Admin-managed controlled vocabulary for the careers board (departments/sectors/experience/locations),
        // so filters and the posting editor draw from configured master data rather than hardcoded lists.
        app.MapGet("/api/careers/meta", () =>
        {
            var g = new Dictionary<string, List<string?>>();
            foreach (var kind in TaxKinds)
                g[kind] = db.Query("SELECT value FROM career_taxonomy WHERE kind=? AND active=1 ORDER BY sort_order,value", kind)
                    .Select(r => H.Str(r["value"])).ToList();
            return J(g);
        });

        // Apply in-platform (only for postings whose apply_method is 'inplatform'). Honeypot + basic validation
        // + one application per email per posting (unique index enforces it too).
        app.MapPost("/api/careers/{id:long}/apply", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            if (!string.IsNullOrEmpty(H.GetS(b, "website"))) return J(new { ok = true });   // honeypot: pretend success
            var job = db.QueryOne($"SELECT id,apply_method FROM job_postings WHERE id=? AND {OpenWhere}", id);
            if (job is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            if (H.Str(job["apply_method"]) != "inplatform") return Results.Json(new { error = "external_apply", message = "This role is applied for externally." }, statusCode: 400);
            var name = (H.GetS(b, "name") ?? "").Trim();
            var email = (H.GetS(b, "email") ?? "").Trim().ToLowerInvariant();
            if (name.Length is < 2 or > 120) return Results.Json(new { error = "bad_name", message = "Please enter your full name." }, statusCode: 400);
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return Results.Json(new { error = "bad_email", message = "Please enter a valid email." }, statusCode: 400);
            var cover = (H.GetS(b, "cover_message", "message") ?? "").Trim(); if (cover.Length > 5000) cover = cover[..5000];
            if (db.QueryOne("SELECT id FROM job_applications WHERE job_id=? AND email=?", id, email) is not null)
                return Results.Json(new { error = "already_applied", message = "You have already applied for this role." }, statusCode: 409);
            string? cvRef = null, cvName = null;
            if (H.GetS(b, "cv_data", "cv") is { Length: > 0 } dataUri)
            {
                var (reference, clean, err) = StoreUpload(H.GetS(b, "cv_name", "filename"), dataUri, "cv");
                if (err is not null) return Results.Json(new { error = err, message = "That CV file could not be accepted." }, statusCode: 400);
                cvRef = reference; cvName = clean;
            }
            // Job-specific questions: validate required answers and capture them (label+value) for the reviewer.
            var questions = db.Query("SELECT id,label,required FROM job_questions WHERE job_id=? ORDER BY sort_order,id", id);
            var ansMap = new Dictionary<long, string>();
            if (H.GetEl(b, "answers") is JsonElement ae && ae.ValueKind == JsonValueKind.Object)
                foreach (var p in ae.EnumerateObject())
                    if (long.TryParse(p.Name, out var qid))
                        ansMap[qid] = p.Value.ValueKind == JsonValueKind.String ? (p.Value.GetString() ?? "")
                            : p.Value.ValueKind == JsonValueKind.Array ? string.Join(", ", p.Value.EnumerateArray().Select(x => x.GetString() ?? ""))
                            : p.Value.ToString();
            var captured = new List<Dictionary<string, object?>>();
            foreach (var qq in questions)
            {
                var lbl = H.Str(qq["label"]) ?? "";
                var val = (ansMap.TryGetValue(H.L(qq["id"]), out var vv) ? vv : "").Trim();
                if (val.Length > 4000) val = val[..4000];
                if (H.B(qq["required"]) && val.Length == 0)
                    return Results.Json(new { error = "answer_required", message = $"Please answer: {lbl}" }, statusCode: 400);
                if (val.Length > 0) captured.Add(new() { ["label"] = lbl, ["value"] = val });
            }
            var answersJson = captured.Count > 0 ? JsonSerializer.Serialize(captured) : null;
            var userId = Auth.UserFromReq(ctx.Request, db)?.Id;   // link to the member's dashboard when signed in
            var appId = db.ExecuteReturningId("INSERT INTO job_applications(job_id,name,email,phone,cover_message,cv_ref,cv_name,answers_json,user_id) VALUES(?,?,?,?,?,?,?,?,?)",
                id, name, email, H.GetS(b, "phone"), cover.Length > 0 ? cover : null, cvRef, cvName, answersJson, userId);
            var reference = $"PCI-APP-{DateTime.UtcNow:yyyy}-{appId:D6}";
            db.Execute("UPDATE job_applications SET reference=? WHERE id=?", reference, appId);
            log(userId, "job_application", $"job {id} #{appId} {reference}");
            Notify(db, appId, "application_received");   // best-effort candidate acknowledgement
            return J(new { ok = true, reference, message = $"Your application has been received. Your reference is {reference}." });
        });

        // ─────────────────────────── ADMIN (gated: content) ───────────────────────────
        app.MapGet("/api/admin/careers", (HttpRequest req) => gate(req, "content", _ =>
            J(new { rows = db.Query(@"SELECT j.*, (SELECT COUNT(*) FROM job_applications a WHERE a.job_id=j.id) applications
                FROM job_postings j ORDER BY j.featured DESC, COALESCE(j.posted_at,j.created_at) DESC, j.id DESC LIMIT 500") })));

        app.MapPost("/api/admin/careers", (HttpRequest req) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var title = (H.GetS(b, "title") ?? "").Trim();
            if (title.Length is < 3 or > 200) return Results.Json(new { error = "bad_title", message = "Title must be 3–200 characters." }, statusCode: 400);
            var type = H.GetS(b, "employment_type") ?? "full_time"; if (!Types.Contains(type)) type = "full_time";
            var remote = H.GetS(b, "remote_type") ?? "onsite"; if (!Remotes.Contains(remote)) remote = "onsite";
            var apply = H.GetS(b, "apply_method") ?? "inplatform"; if (apply is not ("inplatform" or "url" or "email")) apply = "inplatform";
            var status = H.GetS(b, "status") ?? "draft"; if (status is not ("draft" or "published" or "closed")) status = "draft";
            // Auto-stamp posted_at when a posting is first published without one.
            var postedAt = H.GetS(b, "posted_at");
            if (status == "published" && string.IsNullOrWhiteSpace(postedAt)) postedAt = H.IsoNow;
            var cols = new (string col, object? val)[]
            {
                ("title", title), ("organisation", H.GetS(b, "organisation")), ("location", H.GetS(b, "location")), ("country", H.GetS(b, "country")),
                ("employment_type", type), ("remote_type", remote), ("sector", H.GetS(b, "sector")),
                ("description", H.GetS(b, "description")), ("requirements", H.GetS(b, "requirements")), ("responsibilities", H.GetS(b, "responsibilities")),
                ("salary_min", H.GetNum(b, "salary_min")), ("salary_max", H.GetNum(b, "salary_max")),
                ("salary_currency", H.GetS(b, "salary_currency") ?? "USD"), ("salary_period", H.GetS(b, "salary_period") ?? "year"),
                ("apply_method", apply), ("apply_url", H.GetS(b, "apply_url")), ("apply_email", H.GetS(b, "apply_email")),
                ("featured", (H.GetBool(b, "featured") ?? false) ? 1 : 0), ("status", status),
                ("posted_at", postedAt), ("closes_at", H.GetS(b, "closes_at")),
                // Increment 1: richer job model (fuller detail pages + Google-for-Jobs structured data)
                ("department", H.GetS(b, "department")), ("experience_level", H.GetS(b, "experience_level")),
                ("vacancies", H.GetNum(b, "vacancies")), ("benefits", H.GetS(b, "benefits")),
                ("education", H.GetS(b, "education")), ("languages", H.GetS(b, "languages")),
                ("certifications", H.GetS(b, "certifications")), ("reporting_line", H.GetS(b, "reporting_line")),
                ("expected_start", H.GetS(b, "expected_start")), ("application_instructions", H.GetS(b, "application_instructions")),
                ("eo_statement", H.GetS(b, "eo_statement")), ("publish_at", H.GetS(b, "publish_at")),
                ("salary_visible", (H.GetBool(b, "salary_visible") ?? true) ? 1 : 0),
                ("urgent", (H.GetBool(b, "urgent") ?? false) ? 1 : 0),
            };
            // Optional custom job code; validated for uniqueness (the unique index also guards it).
            var codeIn = (H.GetS(b, "job_code") ?? "").Trim().ToUpperInvariant();
            if (codeIn.Length > 32) codeIn = codeIn[..32];
            var id = H.GetNum(b, "id");
            if (id is > 0)
            {
                if (codeIn.Length > 0 && db.QueryOne("SELECT id FROM job_postings WHERE job_code=? AND id<>?", codeIn, (long)id) is not null)
                    return Results.Json(new { error = "code_taken", message = "That job code is already in use." }, statusCode: 400);
                var set = string.Join(",", cols.Select(c => c.col + "=?")) + (codeIn.Length > 0 ? ",job_code=?" : "") + ",updated_at=datetime('now')";
                var vals = cols.Select(c => c.val).ToList(); if (codeIn.Length > 0) vals.Add(codeIn); vals.Add((long)id.Value);
                db.Execute($"UPDATE job_postings SET {set} WHERE id=?", vals.ToArray());
                log(adm.Id, "career_update", ((long)id).ToString());
                return J(new { ok = true, id = (long)id });
            }
            if (codeIn.Length > 0 && db.QueryOne("SELECT id FROM job_postings WHERE job_code=?", codeIn) is not null)
                return Results.Json(new { error = "code_taken", message = "That job code is already in use." }, statusCode: 400);
            var colNames = cols.Select(c => c.col).Append("created_by").ToArray();
            var ph = string.Join(",", colNames.Select(_ => "?"));
            var newId = db.ExecuteReturningId($"INSERT INTO job_postings({string.Join(",", colNames)}) VALUES({ph})", cols.Select(c => c.val).Append((object?)adm.Id).ToArray());
            // Auto-generate a unique job code from the new id when the operator didn't supply one.
            var code = codeIn.Length > 0 ? codeIn : $"PCI-{DateTime.UtcNow:yyyy}-{newId:D4}";
            db.Execute("UPDATE job_postings SET job_code=? WHERE id=?", code, newId);
            log(adm.Id, "career_create", $"{newId} {code}");
            return J(new { ok = true, id = newId, job_code = code });
        }));

        app.MapPost("/api/admin/careers/{id:long}/delete", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            db.Execute("DELETE FROM job_applications WHERE job_id=?", id);
            db.Execute("DELETE FROM job_postings WHERE id=?", id);
            log(adm.Id, "career_delete", id.ToString());
            return J(new { ok = true });
        }));

        app.MapGet("/api/admin/careers/{id:long}/applications", (HttpRequest req, long id) => gate(req, "content", _ =>
            J(new { rows = db.Query(@"SELECT a.id,a.job_id,a.name,a.email,a.phone,a.cover_message,a.cv_name,a.reference,a.answers_json,a.status,a.admin_note,
                a.assigned_to,au.name AS assignee_name,a.created_at
                FROM job_applications a LEFT JOIN admin_users au ON au.id=a.assigned_to WHERE a.job_id=? ORDER BY a.id DESC", id) })));

        // Per-job application questions (admin builder). POST replaces the whole set for the job.
        app.MapGet("/api/admin/careers/{id:long}/questions", (HttpRequest req, long id) => gate(req, "content", _ =>
            J(new { rows = db.Query("SELECT id,qtype,label,options,required,sort_order FROM job_questions WHERE job_id=? ORDER BY sort_order,id", id) })));

        app.MapPost("/api/admin/careers/{id:long}/questions", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            db.Execute("DELETE FROM job_questions WHERE job_id=?", id);
            if (H.GetEl(b, "questions") is JsonElement qs && qs.ValueKind == JsonValueKind.Array)
            {
                int i = 0;
                foreach (var q in qs.EnumerateArray())
                {
                    var qtype = q.TryGetProperty("qtype", out var qt) ? (qt.GetString() ?? "short_text") : "short_text";
                    if (!QTypes.Contains(qtype)) qtype = "short_text";
                    var label = (q.TryGetProperty("label", out var lb) ? (lb.GetString() ?? "") : "").Trim();
                    if (label.Length == 0) continue;
                    if (label.Length > 300) label = label[..300];
                    var options = q.TryGetProperty("options", out var op) && op.ValueKind == JsonValueKind.String ? op.GetString() : null;
                    var required = q.TryGetProperty("required", out var rq) && (rq.ValueKind == JsonValueKind.True || (rq.ValueKind == JsonValueKind.Number && rq.GetInt32() != 0));
                    db.Execute("INSERT INTO job_questions(job_id,qtype,label,options,required,sort_order) VALUES(?,?,?,?,?,?)",
                        id, qtype, label, options, required ? 1 : 0, i++);
                }
            }
            log(adm.Id, "career_questions", id.ToString());
            return J(new { ok = true });
        }));

        // ── Master data (career_taxonomy): admin-managed departments/sectors/experience/locations ──
        app.MapGet("/api/admin/careers/taxonomy", (HttpRequest req) => gate(req, "content", _ =>
            J(new { rows = db.Query("SELECT id,kind,value,sort_order,active FROM career_taxonomy ORDER BY kind,sort_order,value"), kinds = TaxKinds })));

        app.MapPost("/api/admin/careers/taxonomy", (HttpRequest req) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var kind = (H.GetS(b, "kind") ?? "").Trim().ToLowerInvariant();
            if (!TaxKinds.Contains(kind)) return Results.Json(new { error = "bad_kind" }, statusCode: 400);
            var value = (H.GetS(b, "value") ?? "").Trim();
            if (value.Length is < 1 or > 160) return Results.Json(new { error = "bad_value", message = "Enter a value (1–160 characters)." }, statusCode: 400);
            var order = (long)(H.GetNum(b, "sort_order") ?? 0);
            var id = H.GetNum(b, "id");
            if (id is > 0)
                db.Execute("UPDATE career_taxonomy SET value=?,sort_order=?,active=? WHERE id=?",
                    value, order, (H.GetBool(b, "active") ?? true) ? 1 : 0, (long)id.Value);
            else
            {
                if (db.QueryOne("SELECT id FROM career_taxonomy WHERE kind=? AND value=?", kind, value) is not null)
                    return Results.Json(new { error = "exists", message = "That value already exists." }, statusCode: 400);
                db.Execute("INSERT INTO career_taxonomy(kind,value,sort_order) VALUES(?,?,?)", kind, value, order);
            }
            log(adm.Id, "career_taxonomy", $"{kind}:{value}");
            return J(new { ok = true });
        }));

        app.MapPost("/api/admin/careers/taxonomy/{id:long}/delete", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            db.Execute("DELETE FROM career_taxonomy WHERE id=?", id);
            log(adm.Id, "career_taxonomy_delete", id.ToString());
            return J(new { ok = true });
        }));

        // ── Candidate email templates (admin-editable, rendered with {{placeholders}}) ──
        app.MapGet("/api/admin/careers/templates", (HttpRequest req) => gate(req, "content", _ =>
            J(new { rows = db.Query("SELECT id,event_key,subject,body,enabled FROM career_email_templates ORDER BY id"), keys = TemplateKeys })));

        app.MapPost("/api/admin/careers/templates", (HttpRequest req) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var key = (H.GetS(b, "event_key") ?? "").Trim();
            if (!TemplateKeys.Contains(key)) return Results.Json(new { error = "bad_event" }, statusCode: 400);
            var subject = (H.GetS(b, "subject") ?? "").Trim(); if (subject.Length > 300) subject = subject[..300];
            var body = H.GetS(b, "body") ?? "";
            var enabled = (H.GetBool(b, "enabled") ?? true) ? 1 : 0;
            if (db.QueryOne("SELECT id FROM career_email_templates WHERE event_key=?", key) is not null)
                db.Execute("UPDATE career_email_templates SET subject=?,body=?,enabled=?,updated_at=datetime('now') WHERE event_key=?", subject, body, enabled, key);
            else
                db.Execute("INSERT INTO career_email_templates(event_key,subject,body,enabled) VALUES(?,?,?,?)", key, subject, body, enabled);
            log(adm.Id, "career_template", key);
            return J(new { ok = true });
        }));

        // ── Recruiting analytics (read-only funnel + per-posting counts) ──
        app.MapGet("/api/admin/careers/reports", (HttpRequest req) => gate(req, "content", _ =>
        {
            var byStatus = db.Query("SELECT status, COUNT(*) n FROM job_applications GROUP BY status");
            var perJob = db.Query(@"SELECT j.id,j.title,j.job_code,j.status,
                (SELECT COUNT(*) FROM job_applications a WHERE a.job_id=j.id) applications
                FROM job_postings j ORDER BY applications DESC, j.id DESC LIMIT 50");
            var totals = db.QueryOne(@"SELECT
                (SELECT COUNT(*) FROM job_postings) postings,
                (SELECT COUNT(*) FROM job_postings WHERE status='published') published,
                (SELECT COUNT(*) FROM job_applications) applications,
                (SELECT COUNT(*) FROM job_applications WHERE created_at >= datetime('now','-30 days')) applications_30d,
                (SELECT COUNT(*) FROM job_applications WHERE status='hired') hired");
            return J(new { totals, byStatus, perJob });
        }));

        // ─────────────────────────── MEMBER (student session) ───────────────────────────
        // The signed-in member's own applications, for the portal "My applications" view.
        app.MapGet("/api/me/applications", (HttpContext ctx) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var rows = db.Query(@"SELECT a.id,a.reference,a.status,a.created_at,a.cv_name,j.title,j.job_code,j.organisation,j.location,j.country
                FROM job_applications a JOIN job_postings j ON j.id=a.job_id WHERE a.user_id=? ORDER BY a.id DESC", u.Id);
            return J(new { rows });
        });

        // One application's candidate-visible detail: status + messages + interviews only (never internal notes).
        app.MapGet("/api/me/applications/{id:long}", (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var a = db.QueryOne(@"SELECT a.id,a.reference,a.status,a.created_at,a.cv_name,j.title,j.job_code,j.organisation,j.location,j.country
                FROM job_applications a JOIN job_postings j ON j.id=a.job_id WHERE a.id=? AND a.user_id=?", id, u.Id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var events = db.Query("SELECT kind,to_status,body,scheduled_at,created_at FROM job_app_events WHERE application_id=? AND kind IN('message','interview','status') ORDER BY id", id);
            return J(new { application = a, events });
        });

        // Candidate withdraws their own application.
        app.MapPost("/api/me/applications/{id:long}/withdraw", (HttpContext ctx, long id) =>
        {
            var u = Auth.UserFromReq(ctx.Request, db);
            if (u is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
            var a = db.QueryOne("SELECT status FROM job_applications WHERE id=? AND user_id=?", id, u.Id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var from = H.Str(a["status"]);
            if (from is "withdrawn" or "hired") return Results.Json(new { error = "not_allowed", message = "This application can no longer be withdrawn." }, statusCode: 400);
            db.Execute("UPDATE job_applications SET status='withdrawn' WHERE id=?", id);
            db.Execute("INSERT INTO job_app_events(application_id,kind,from_status,to_status,body,actor_name) VALUES(?,?,?,?,?,?)",
                id, "status", from, "withdrawn", "Withdrawn by candidate", "Candidate");
            log(u.Id, "job_app_withdraw", id.ToString());
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/careers/applications/{id:long}/status", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var status = H.GetS(b, "status") ?? "";
            if (!AppStatuses.Contains(status)) return Results.Json(new { error = "bad_status" }, statusCode: 400);
            var cur = db.QueryOne("SELECT status FROM job_applications WHERE id=?", id);
            var from = cur is null ? null : H.Str(cur["status"]);
            db.Execute("UPDATE job_applications SET status=?, admin_note=? WHERE id=?", status, H.GetS(b, "admin_note"), id);
            Event(db, id, "status", adm, H.GetS(b, "note"), from, status, null);   // no silent status changes — every move is recorded
            if (from != status) Notify(db, id, "status_changed");   // tell the candidate their status moved
            log(adm.Id, "job_application_" + status, id.ToString());
            return J(new { ok = true });
        }));

        // Internal note (never candidate-visible).
        app.MapPost("/api/admin/careers/applications/{id:long}/note", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var body = (H.GetS(H.Body(req).GetAwaiter().GetResult(), "body") ?? "").Trim();
            if (body.Length == 0) return Results.Json(new { error = "empty" }, statusCode: 400);
            Event(db, id, "note", adm, body.Length > 5000 ? body[..5000] : body, null, null, null);
            log(adm.Id, "job_app_note", id.ToString());
            return J(new { ok = true });
        }));

        // Candidate-visible message (shown to the applicant in the member portal).
        app.MapPost("/api/admin/careers/applications/{id:long}/message", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var body = (H.GetS(H.Body(req).GetAwaiter().GetResult(), "body") ?? "").Trim();
            if (body.Length == 0) return Results.Json(new { error = "empty" }, statusCode: 400);
            var msg = body.Length > 5000 ? body[..5000] : body;
            Event(db, id, "message", adm, msg, null, null, null);
            Notify(db, id, "message", new() { ["message"] = msg });   // email the candidate the message too
            log(adm.Id, "job_app_message", id.ToString());
            return J(new { ok = true });
        }));

        // Schedule an interview (records a candidate-visible interview event with a date).
        app.MapPost("/api/admin/careers/applications/{id:long}/interview", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var when = (H.GetS(b, "scheduled_at") ?? "").Trim();
            if (when.Length == 0) return Results.Json(new { error = "no_date" }, statusCode: 400);
            Event(db, id, "interview", adm, H.GetS(b, "body"), null, null, when);
            Notify(db, id, "interview_scheduled", new() { ["interview_at"] = when, ["message"] = H.GetS(b, "body") ?? "" });
            log(adm.Id, "job_app_interview", id.ToString());
            return J(new { ok = true });
        }));

        // Assign / unassign a reviewer (assign to me, or clear).
        app.MapPost("/api/admin/careers/applications/{id:long}/assign", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            long? to = (H.GetBool(b, "unassign") == true) ? null : ((long?)H.GetNum(b, "assigned_to") ?? adm.Id);
            db.Execute("UPDATE job_applications SET assigned_to=? WHERE id=?", to, id);
            Event(db, id, "note", adm, to is null ? "Unassigned" : $"Assigned to {adm.Name ?? ("#" + to)}", null, null, null);
            log(adm.Id, "job_app_assign", id.ToString());
            return J(new { ok = true });
        }));

        // Full internal event timeline for one application.
        app.MapGet("/api/admin/careers/applications/{id:long}/events", (HttpRequest req, long id) => gate(req, "content", _ =>
            J(new { rows = db.Query("SELECT id,kind,from_status,to_status,body,scheduled_at,actor_name,created_at FROM job_app_events WHERE application_id=? ORDER BY id", id) })));

        app.MapGet("/api/admin/careers/applications/{id:long}/cv", (HttpRequest req, long id) => gate(req, "content", _ =>
        {
            var a = db.QueryOne("SELECT cv_ref,cv_name FROM job_applications WHERE id=?", id);
            var got = a is null ? null : Storage.Get(H.Str(a["cv_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return Results.Bytes(got.Value.bytes!, got.Value.mime, H.Str(a!["cv_name"]));
        }));
    }

    // Append one applicant-tracking event (status change, internal note, candidate message or interview).
    static void Event(Db db, long appId, string kind, AdminCtx adm, string? body, string? from, string? to, string? scheduledAt)
        => db.Execute("INSERT INTO job_app_events(application_id,kind,from_status,to_status,body,scheduled_at,actor_id,actor_name) VALUES(?,?,?,?,?,?,?,?)",
            appId, kind, from, to, string.IsNullOrWhiteSpace(body) ? null : body, scheduledAt, adm.Id, adm.Name);

    /// <summary>Best-effort candidate notification: render the admin-managed template for this event and
    /// enqueue it through the Communications Centre outbox (which the dispatcher then delivers, honouring
    /// suppression/consent). A missing or disabled template is skipped. This NEVER throws into the caller —
    /// a recruiting action must not fail because email is misconfigured.</summary>
    static void Notify(Db db, long appId, string eventKey, Dictionary<string, string?>? extra = null)
    {
        try
        {
            var t = db.QueryOne("SELECT subject,body,enabled FROM career_email_templates WHERE event_key=?", eventKey);
            if (t is null || !H.B(t["enabled"])) return;
            var a = db.QueryOne(@"SELECT a.email,a.name,a.reference,a.status,a.user_id,j.title,j.organisation
                FROM job_applications a JOIN job_postings j ON j.id=a.job_id WHERE a.id=?", appId);
            if (a is null) return;
            var email = H.Str(a["email"]);
            if (string.IsNullOrWhiteSpace(email)) return;
            var map = new Dictionary<string, string?>
            {
                ["name"] = H.Str(a["name"]), ["job_title"] = H.Str(a["title"]),
                ["org"] = string.IsNullOrWhiteSpace(H.Str(a["organisation"])) ? "the Project Controls Institute" : H.Str(a["organisation"]),
                ["reference"] = H.Str(a["reference"]), ["status"] = H.Str(a["status"]), ["message"] = "", ["interview_at"] = "",
            };
            if (extra is not null) foreach (var kv in extra) map[kv.Key] = kv.Value;
            string Render(string? s) { s ??= ""; foreach (var kv in map) s = s.Replace("{{" + kv.Key + "}}", kv.Value ?? ""); return s; }
            long? uid = a.TryGetValue("user_id", out var uv) && uv is not null ? H.L(uv) : null;
            Comms.Enqueue(db, "email", $"careers:{eventKey}:{appId}:{DateTime.UtcNow.Ticks}", uid, email, null,
                Render(H.Str(t["subject"])), Render(H.Str(t["body"])), category: "operational", triggerCode: "careers_" + eventKey);
        }
        catch { /* candidate notifications are best-effort — never break the recruiting action */ }
    }

    // schema.org employmentType values (Google for Jobs).
    static readonly Dictionary<string, string> EmpMap = new()
    { ["full_time"] = "FULL_TIME", ["part_time"] = "PART_TIME", ["contract"] = "CONTRACTOR", ["internship"] = "INTERN", ["temporary"] = "TEMPORARY" };

    /// <summary>Build a schema.org JobPosting JSON-LD document for a posting row, for Google for Jobs.
    /// Salary is included only when the operator chose to publish it (salary_visible). Returns a JSON string.</summary>
    static string JobJsonLd(Db db, Dictionary<string, object?> r)
    {
        string? S(string k) => H.Str(r.TryGetValue(k, out var v) ? v : null);
        var baseUrl = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", "site_base_url");
        if (string.IsNullOrWhiteSpace(baseUrl)) baseUrl = "https://www.projectcontrolsinstitute.org";
        baseUrl = baseUrl!.TrimEnd('/');

        string Part(string t, string? body) => string.IsNullOrWhiteSpace(body) ? "" : $"<h3>{Esc(t)}</h3><p>{Esc(body)}</p>";
        var desc = Part("About the role", S("description")) + Part("Responsibilities", S("responsibilities"))
                 + Part("Requirements", S("requirements")) + Part("Benefits", S("benefits"));
        if (desc.Length == 0) desc = Esc(S("title") ?? "Role at the Project Controls Institute");

        var o = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org/", ["@type"] = "JobPosting",
            ["title"] = S("title"), ["description"] = desc,
            ["identifier"] = new Dictionary<string, object?> { ["@type"] = "PropertyValue", ["name"] = "Project Controls Institute", ["value"] = S("job_code") },
            ["hiringOrganization"] = new Dictionary<string, object?>
            { ["@type"] = "Organization", ["name"] = string.IsNullOrWhiteSpace(S("organisation")) ? "Project Controls Institute" : S("organisation"), ["sameAs"] = baseUrl },
            ["directApply"] = S("apply_method") == "inplatform",
        };
        if (S("posted_at") is { Length: > 0 } pa) o["datePosted"] = pa[..Math.Min(10, pa.Length)];
        if (S("closes_at") is { Length: > 0 } ca) o["validThrough"] = ca;
        if (EmpMap.TryGetValue(S("employment_type") ?? "", out var et)) o["employmentType"] = et;

        var city = S("location"); var country = S("country");
        if (!string.IsNullOrWhiteSpace(city) || !string.IsNullOrWhiteSpace(country))
            o["jobLocation"] = new Dictionary<string, object?>
            { ["@type"] = "Place", ["address"] = new Dictionary<string, object?> { ["@type"] = "PostalAddress", ["addressLocality"] = city, ["addressCountry"] = country } };
        if (S("remote_type") == "remote")
        {
            o["jobLocationType"] = "TELECOMMUTE";
            if (!string.IsNullOrWhiteSpace(country)) o["applicantLocationRequirements"] = new Dictionary<string, object?> { ["@type"] = "Country", ["name"] = country };
        }

        var mn = H.D(r.GetValueOrDefault("salary_min")); var mx = H.D(r.GetValueOrDefault("salary_max"));
        if (H.B(r.GetValueOrDefault("salary_visible")) && (mn > 0 || mx > 0))
        {
            var val = new Dictionary<string, object?> { ["@type"] = "QuantitativeValue", ["unitText"] = (S("salary_period") ?? "year").ToUpperInvariant() };
            if (mn > 0) val["minValue"] = mn;
            if (mx > 0) val["maxValue"] = mx;
            if (mn > 0 && mx <= 0) val["value"] = mn;
            o["baseSalary"] = new Dictionary<string, object?> { ["@type"] = "MonetaryAmount", ["currency"] = S("salary_currency") ?? "USD", ["value"] = val };
        }
        return System.Text.Json.JsonSerializer.Serialize(o);
    }

    static string Esc(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");
}
