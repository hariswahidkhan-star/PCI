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
            return J(new { job = Row(r, true), jsonld = JobJsonLd(db, r) });   // jsonld = Google-for-Jobs JobPosting structured data
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
            var appId = db.ExecuteReturningId("INSERT INTO job_applications(job_id,name,email,phone,cover_message,cv_ref,cv_name) VALUES(?,?,?,?,?,?,?)",
                id, name, email, H.GetS(b, "phone"), cover.Length > 0 ? cover : null, cvRef, cvName);
            log(null, "job_application", $"job {id} #{appId}");
            return J(new { ok = true, message = "Your application has been received. Thank you." });
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
            J(new { rows = db.Query("SELECT id,job_id,name,email,phone,cover_message,cv_name,status,admin_note,created_at FROM job_applications WHERE job_id=? ORDER BY id DESC", id) })));

        app.MapPost("/api/admin/careers/applications/{id:long}/status", (HttpRequest req, long id) => gate(req, "content", adm =>
        {
            var b = H.Body(req).GetAwaiter().GetResult();
            var status = H.GetS(b, "status") ?? "";
            if (status is not ("new" or "reviewing" or "shortlisted" or "rejected" or "hired"))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            db.Execute("UPDATE job_applications SET status=?, admin_note=? WHERE id=?", status, H.GetS(b, "admin_note"), id);
            log(adm.Id, "job_application_" + status, id.ToString());
            return J(new { ok = true });
        }));

        app.MapGet("/api/admin/careers/applications/{id:long}/cv", (HttpRequest req, long id) => gate(req, "content", _ =>
        {
            var a = db.QueryOne("SELECT cv_ref,cv_name FROM job_applications WHERE id=?", id);
            var got = a is null ? null : Storage.Get(H.Str(a["cv_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            return Results.Bytes(got.Value.bytes!, got.Value.mime, H.Str(a!["cv_name"]));
        }));
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
