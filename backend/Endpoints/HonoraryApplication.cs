using System.Linq;
using System.Text.Json;
using System.Net;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Honorary Route public application: anyone may apply for the board's consideration to be conferred
/// "Honorary Fellow (PCI)". This is a SEPARATE flow from student registration — it collects its own
/// applicant record and supporting documents, is reviewed by the board in the admin portal, and on
/// approval mints a normal honorary_awards row via <see cref="Honorary.ConferAward"/>. It never touches
/// exam, entitlement or issued-credential data, and the recognition is always labelled honorary.
/// </summary>
public static class HonoraryApplication
{
    static readonly HashSet<string> DocKinds = new() { "resume", "academic", "certifications", "supporting" };
    static readonly System.Text.RegularExpressions.Regex EmailRx =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", System.Text.RegularExpressions.RegexOptions.Compiled);

    // Trim + hard-cap a free-text field so an over-long or padded value can never bloat a row.
    static string Clip(string? s, int max) => (s ?? "").Trim() is { } t && t.Length > max ? t[..max] : (s ?? "").Trim();

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);

        // Board/owner only — conferring recognition is a governance act (mirrors Honorary.cs).
        (AdminCtx? adm, IResult? deny) Owner(HttpRequest req)
        {
            var a = Auth.AdminFromReq(req, db);
            if (a is null) return (null, Results.Json(new { error = "unauthorised" }, statusCode: 401));
            if (!a.IsOwner) return (null, Results.Json(new { error = "forbidden" }, statusCode: 403));
            return (a, null);
        }

        // ---------------- PUBLIC: submit an application ----------------
        app.MapPost("/api/honorary-application", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            string S(string k, int max, params string[] more) => Clip(H.GetS(b, new[] { k }.Concat(more).ToArray()), max);

            var first = S("first_name", 80, "firstName");
            var last = S("last_name", 80, "lastName");
            var email = S("email", 160).ToLowerInvariant();
            var mobile = S("mobile", 40, "phone");
            var country = S("country", 80);
            var city = S("city", 80);
            var nationality = S("nationality", 80);
            var jobTitle = S("job_title", 120, "jobTitle");
            var employer = S("employer", 160, "organization", "organisation");
            var industry = S("industry", 120);
            var yearsExp = (int)Math.Clamp(H.GetNum(b, "years_experience", "yearsExperience") ?? 0, 0, 80);
            var highestQual = S("highest_qualification", 200, "highestQualification");
            var profCerts = S("professional_certifications", 4000, "professionalCertifications");
            var relevantExp = S("relevant_experience", 6000, "relevantExperience");
            var summary = S("professional_summary", 6000, "professionalSummary");
            bool Flag(string key) => H.GetEl(b, key) is { } e && (e.ValueKind == JsonValueKind.True
                || (e.ValueKind == JsonValueKind.String && e.GetString() is "1" or "true" or "yes"));
            var declaration = Flag("declaration");
            var eligibilityConfirmed = Flag("eligibility_confirmed") || Flag("eligibilityConfirmed");
            var termsAccepted = Flag("terms_accepted") || Flag("termsAccepted");
            var suitabilityNote = S("suitability_note", 2000, "suitabilityNote");

            // ---- structured history: repeatable qualification / certification / experience rows ----
            // Each row is sanitised field-by-field (clip + year clamp), capped at 10 rows per section,
            // and stored as JSON for the board. The legacy flat text columns are composed from the rows
            // when the flat field is absent, so older admin views and exports keep working.
            string ES(JsonElement e, params string[] keys)
            {
                foreach (var k in keys)
                    if (e.ValueKind == JsonValueKind.Object && e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String)
                        return Clip(v.GetString(), 200);
                return "";
            }
            int? EY(JsonElement e, params string[] keys)
            {
                foreach (var k in keys)
                    if (e.ValueKind == JsonValueKind.Object && e.TryGetProperty(k, out var v))
                    {
                        var s = v.ValueKind == JsonValueKind.Number ? v.GetRawText()
                              : v.ValueKind == JsonValueKind.String ? v.GetString() : null;
                        if (int.TryParse((s ?? "").Trim(), out var y) && y >= 1940 && y <= DateTime.UtcNow.Year + 1) return y;
                    }
                return null;
            }
            List<Dictionary<string, object?>> Rows(string key, Func<JsonElement, Dictionary<string, object?>?> shape)
            {
                var list = new List<Dictionary<string, object?>>();
                if (H.GetEl(b, key) is { ValueKind: JsonValueKind.Array } arr2)
                    foreach (var e in arr2.EnumerateArray())
                    {
                        if (list.Count >= 10) break;
                        var row = shape(e);
                        if (row is not null) list.Add(row);
                    }
                return list;
            }
            var quals = Rows("qualifications", e =>
            {
                var t = ES(e, "qualification", "title");
                if (t.Length == 0) return null;
                return new Dictionary<string, object?> { ["qualification"] = t, ["institution"] = ES(e, "institution"), ["year"] = EY(e, "year") };
            });
            var certRows = Rows("certifications", e =>
            {
                var n = ES(e, "name", "certification");
                if (n.Length == 0) return null;
                return new Dictionary<string, object?> { ["name"] = n, ["issuer"] = ES(e, "issuer", "issuing_body"), ["year"] = EY(e, "year") };
            });
            var expRows = Rows("experience", e =>
            {
                var r = ES(e, "role", "job_title", "title"); var emp = ES(e, "employer", "organisation", "organization");
                if (r.Length == 0 && emp.Length == 0) return null;
                return new Dictionary<string, object?> { ["role"] = r, ["employer"] = emp, ["from_year"] = EY(e, "from_year", "from"), ["to_year"] = EY(e, "to_year", "to") };
            });
            static string RowLine(Dictionary<string, object?> r, string mainKey, string subKey)
            {
                var s = (string?)r[mainKey] ?? "";
                if (r[subKey] is string sub && sub.Length > 0) s += ", " + sub;
                if (r["year"] is int y) s += $" ({y})";
                return s;
            }
            if (highestQual.Length == 0 && quals.Count > 0)
                highestQual = Clip(string.Join("; ", quals.Select(q => RowLine(q, "qualification", "institution"))), 200);
            if (profCerts.Length == 0 && certRows.Count > 0)
                profCerts = Clip(string.Join("; ", certRows.Select(c => RowLine(c, "name", "issuer"))), 4000);
            var qualJson = quals.Count > 0 ? JsonSerializer.Serialize(quals) : null;
            var certJson = certRows.Count > 0 ? JsonSerializer.Serialize(certRows) : null;
            var expJson = expRows.Count > 0 ? JsonSerializer.Serialize(expRows) : null;

            // Optional certification/discipline the applicant is aligned to. Honorary recognition is not
            // tied to an examination, so this is not required — but if a value is sent it must resolve to a
            // real, active certification (never silently defaulted), so the board sees an accurate record.
            var certSel = S("certification", 40, "certification_id", "certificationId", "cert");
            long? certId = null;
            if (certSel.Length > 0)
            {
                certId = Certs.TryResolve(db, certSel);
                if (certId is null || Certs.ById(db, certId.Value) is null)
                    return Results.Json(new { error = "invalid_certification", message = "The selected certification is not recognised." }, statusCode: 400);
            }

            // ---- required-field + format validation (server-side; the form also checks client-side) ----
            if (first.Length == 0 || last.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
            if (!EmailRx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            foreach (var (val, code) in new[] { (mobile, "mobile_required"), (country, "country_required"),
                (city, "city_required"), (nationality, "nationality_required"), (jobTitle, "job_title_required"),
                (employer, "employer_required"), (industry, "industry_required"),
                (highestQual, "qualification_required"), (relevantExp, "relevant_experience_required"),
                (summary, "professional_summary_required") })
                if (val.Length == 0) return Results.Json(new { error = code }, statusCode: 400);
            if (!declaration) return Results.Json(new { error = "declaration_required", message = "The applicant declaration must be accepted." }, statusCode: 400);
            if (!eligibilityConfirmed) return Results.Json(new { error = "eligibility_required", message = "Please confirm you meet the eligibility criteria." }, statusCode: 400);
            if (!termsAccepted) return Results.Json(new { error = "terms_required", message = "The terms and conditions must be accepted." }, statusCode: 400);

            // ---- validate + store documents (Storage enforces MIME allow-list + 3 MB magic-byte check) ----
            var docsEl = H.GetEl(b, "documents");
            var toStore = new List<(string kind, string name, Storage.StoredObject obj)>();
            var resumeSeen = false;
            if (docsEl is { ValueKind: JsonValueKind.Array } arr)
            {
                if (arr.GetArrayLength() > 8) return Results.Json(new { error = "too_many_documents", message = "Please attach at most 8 documents." }, statusCode: 400);
                foreach (var d in arr.EnumerateArray())
                {
                    string? P(string k) => d.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
                    var kind = (P("doc_kind") ?? P("kind") ?? "supporting").Trim().ToLowerInvariant();
                    if (!DocKinds.Contains(kind)) kind = "supporting";
                    var dataUri = P("data_uri") ?? P("dataUri") ?? P("data");
                    if (string.IsNullOrWhiteSpace(dataUri)) continue; // empty optional slot
                    var (bytes, mime, err) = Storage.DecodeDataUri(dataUri);
                    if (err is not null) return Results.Json(new { error = err, doc_kind = kind }, statusCode: 400);
                    var obj = Storage.Put(bytes!, mime, "honorary");
                    toStore.Add((kind, Clip(P("filename") ?? P("name"), 200), obj));
                    if (kind == "resume") resumeSeen = true;
                }
            }
            if (!resumeSeen) return Results.Json(new { error = "resume_required", message = "A résumé / CV (PDF, JPG or PNG, up to 3 MB) is required." }, statusCode: 400);

            // ---- insert application (unique reference PCI-HONAPP-YYYY-NNNN) + documents ----
            var yr = DateTime.UtcNow.Year; var rnd = new Random();
            long appId = 0; string? reference = null;
            for (int i = 0; i < 20 && reference is null; i++)
            {
                var cand = i < 10 ? $"PCI-HONAPP-{yr}-{1000 + rnd.Next(0, 8999)}" : $"PCI-HONAPP-{yr}-{10000 + rnd.Next(0, 89999)}";
                try
                {
                    appId = db.ExecuteReturningId(@"INSERT INTO honorary_applications
                        (reference,first_name,last_name,email,mobile,country,city,nationality,job_title,employer,years_experience,industry,highest_qualification,professional_certifications,relevant_experience,professional_summary,qualifications_json,certifications_json,experience_json,certification_id,suitability_note,declaration,eligibility_confirmed,terms_accepted,terms_accepted_at,status)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,1,1,1,datetime('now'),'pending_review')",
                        cand, first, last, email, mobile, country, city, nationality, jobTitle, employer, yearsExp, industry, highestQual, profCerts, relevantExp, summary, qualJson, certJson, expJson, certId, suitabilityNote);
                    reference = cand;
                }
                catch { /* reference collision → retry */ }
            }
            if (reference is null) return Results.Json(new { error = "reference_generation_failed" }, statusCode: 500);
            foreach (var (kind, name, obj) in toStore)
                db.Execute("INSERT INTO honorary_application_documents(application_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256) VALUES(?,?,?,?,?,?,?)",
                    appId, kind, name, obj.Mime, obj.SizeBytes, obj.Reference, obj.Sha256);
            log(null, "honorary_application_submitted", $"{reference} <{email}>");

            // ---- notifications (email now; recipients/enable are settings-driven, never hardcoded) ----
            if (Notify.Enabled(db, "honorary"))
            {
                var fullName = $"{first} {last}".Trim();
                var baseUrl = Mailer.BaseUrl(req);
                var when = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'");
                var ackSubject = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='notify_honorary_ack_subject'") is { Length: > 0 } cs
                    ? cs : "We've received your Honorary Fellow (PCI) application";
                Notify.Email(db, null, email, ackSubject,
                    $"<p>Dear {WebUtility.HtmlEncode(first)},</p>" +
                    $"<p>Thank you for applying for consideration as an <strong>Honorary Fellow (PCI)</strong>. " +
                    $"Your application has been received and is <strong>under review</strong> by the board.</p>" +
                    $"<p>Your application reference is <strong>{WebUtility.HtmlEncode(reference)}</strong> — please quote it in any correspondence.</p>" +
                    $"<p>Honorary recognition is conferred at the board's discretion. It involves no examination and is separate from PCI's examined certification credentials. We will email you once a decision has been made.</p>" +
                    $"<p>— Project Controls Institute Global</p>",
                    "honorary_application", appId);

                var adminTo = Notify.AdminEmail(db);
                var adminSubject = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='notify_honorary_admin_subject'") is { Length: > 0 } asub
                    ? asub : "New Honorary Fellow (PCI) application";
                Notify.Email(db, null, adminTo, adminSubject,
                    $"<p>A new Honorary Fellow (PCI) application has been submitted.</p>" +
                    $"<table cellpadding='4'><tr><td><strong>Applicant</strong></td><td>{WebUtility.HtmlEncode(fullName)}</td></tr>" +
                    $"<tr><td><strong>Email</strong></td><td>{WebUtility.HtmlEncode(email)}</td></tr>" +
                    $"<tr><td><strong>Country</strong></td><td>{WebUtility.HtmlEncode(country)}</td></tr>" +
                    (certId is not null && Certs.ById(db, certId.Value) is { } cRow
                        ? $"<tr><td><strong>Certification of interest</strong></td><td>{WebUtility.HtmlEncode(H.Str(cRow["name"]))}</td></tr>" : "") +
                    $"<tr><td><strong>Reference</strong></td><td>{WebUtility.HtmlEncode(reference)}</td></tr>" +
                    $"<tr><td><strong>Submitted</strong></td><td>{when}</td></tr>" +
                    $"<tr><td><strong>Documents</strong></td><td>{toStore.Count} attached</td></tr></table>" +
                    $"<p>Review it in the admin portal: <a href=\"{baseUrl}/admin/\">{baseUrl}/admin/</a> → Honorary applications. Uploaded files download securely from the application detail.</p>",
                    "honorary_application", appId);
            }

            return J(new { ok = true, reference, message = "Application received — check your email for confirmation." });
        });

        // ---------------- ADMIN (owner): list / detail / files / decide ----------------
        app.MapGet("/api/admin/honorary-applications", (HttpContext ctx) =>
        {
            var (_, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var status = ctx.Request.Query["status"].ToString();
            var where = string.IsNullOrEmpty(status) ? "" : "WHERE a.status=?";
            const string sel = @"SELECT a.*, (SELECT COUNT(*) FROM honorary_application_documents d WHERE d.application_id=a.id) AS doc_count,
                (SELECT c.name FROM certifications c WHERE c.id=a.certification_id) AS certification_name FROM honorary_applications a ";
            var rows = string.IsNullOrEmpty(status)
                ? db.Query(sel + "ORDER BY a.id DESC LIMIT 500")
                : db.Query(sel + where + " ORDER BY a.id DESC LIMIT 500", status);
            return J(new { rows });
        });

        app.MapGet("/api/admin/honorary-applications/{id}", (HttpContext ctx, long id) =>
        {
            var (_, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne(@"SELECT a.*, (SELECT c.name FROM certifications c WHERE c.id=a.certification_id) AS certification_name
                FROM honorary_applications a WHERE a.id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            // Never expose storage_ref/sha to the client — only safe metadata + the download id.
            var docs = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,created_at FROM honorary_application_documents WHERE application_id=? ORDER BY id", id);
            // Shortlist-stage identity-verification documents (owner-only surface), same safe-metadata rule.
            var idvDocs = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,created_at FROM honorary_idv_documents WHERE application_id=? ORDER BY id", id);
            return J(new { application = a, documents = docs, idv_documents = idvDocs });
        });

        app.MapGet("/api/admin/honorary-applications/{id}/documents/{docId}/file", (HttpContext ctx, long id, long docId) =>
        {
            var (_, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var d = db.QueryOne("SELECT storage_ref,mime,filename FROM honorary_application_documents WHERE id=? AND application_id=?", docId, id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var got = Storage.Get(H.Str(d["storage_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            return Results.File(got.Value.bytes!, got.Value.mime);
        });

        app.MapPost("/api/admin/honorary-applications/{id}/decide", async (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Owner(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne("SELECT * FROM honorary_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var status = (H.GetS(b, "status") ?? "").Trim().ToLowerInvariant();
            var note = Clip(H.GetS(b, "admin_note", "note"), 2000);
            var validStatuses = new[] { "under_review", "approved", "rejected", "pending_review" };
            if (status.Length > 0 && !validStatuses.Contains(status))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            if (H.Str(a["status"]) == "approved" && status is "approved" or "rejected")
                return Results.Json(new { error = "already_decided", message = "This application has already been approved." }, statusCode: 409);

            var email = H.Str(a["email"]) ?? "";
            var fullName = $"{H.Str(a["first_name"])} {H.Str(a["last_name"])}".Trim();
            string? awardNo = H.Str(a["award_no"]);

            if (status == "approved")
            {
                var u = db.QueryOne("SELECT id FROM users WHERE email=?", email);
                long? userId = u is null ? null : H.L(u["id"]);

                // Honorary members receive the SAME student experience as any other member (spec §10–12):
                // approval automatically provisions a complete PCI student account (with login credentials),
                // activates an honorary membership, and — via the normal settlement path — hands off to
                // Certuvo. Gated by a configurable rule so an operator can keep honorary purely ceremonial.
                var grantsMembership = Settings.Bool(db, "honorary_grants_membership", true);
                string? setupUrl = null;
                if (grantsMembership)
                {
                    if (userId is null)
                    {
                        var newId = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status) VALUES(?,?,?, 'student','active')",
                            email, H.Str(a["first_name"]), H.Str(a["last_name"]));
                        db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", newId);
                        // Deliver login credentials the same way a new member gets them: a single-use set-password link.
                        var token = Security.RandomHex(32);
                        db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+14 day'))", newId, Security.Sha(token));
                        var baseUrl = Mailer.BaseUrl(ctx.Request);
                        setupUrl = Mailer.SetupLink(baseUrl, token);
                        try { Mailer.SendWelcome(db, newId, email, H.Str(a["first_name"]), setupUrl, baseUrl); } catch { }
                        userId = newId;
                        log(newId, "honorary_account_created", $"{H.Str(a["reference"])} by admin {adm!.Id}");
                    }
                }

                var citation = note.Length > 0 ? note : $"Conferred following honorary application {H.Str(a["reference"])}.";
                awardNo = Honorary.ConferAward(db, fullName, userId, citation, adm!.Id);
                if (awardNo is null) return Results.Json(new { error = "award_no_generation_failed" }, statusCode: 500);
                db.Execute("UPDATE honorary_applications SET status='approved', award_no=?, decided_by=?, decided_at=datetime('now'), admin_note=?, updated_at=datetime('now') WHERE id=?",
                    awardNo, adm.Id, note, id);

                // Activate the honorary membership (a waived, non-revenue settlement) — this drives membership
                // activation AND the Certuvo hand-off through the shared downstream path, so honorary members
                // get practice access with no separate process.
                if (grantsMembership && userId is not null && db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is null)
                {
                    try { Settlement.Grant(db, userId.Value, email, "membership", 0, 0, "HON-" + awardNo, "admin_honorary"); } catch { }
                }

                if (userId is not null)
                    db.Execute("INSERT INTO notifications(user_id,category,title,body,cta_label,cta_route) VALUES(?, 'Recognition', 'Honorary Fellow (PCI)', ?, 'View membership', '/credentials')",
                        userId, $"The board has conferred on you the designation Honorary Fellow (PCI) — award number {awardNo}. Your membership and practice access are being set up.");
                log(userId, "honorary_application_approved", $"{H.Str(a["reference"])} → {awardNo} by admin {adm.Id}");
            }
            else if (status.Length > 0)
            {
                db.Execute("UPDATE honorary_applications SET status=?, decided_by=?, decided_at=datetime('now'), admin_note=?, updated_at=datetime('now') WHERE id=?",
                    status, adm!.Id, note, id);
                log(null, "honorary_application_" + status, $"{H.Str(a["reference"])} by admin {adm.Id}");
            }
            else
            {
                // Note-only update (no status change) — supports "add internal notes".
                db.Execute("UPDATE honorary_applications SET admin_note=?, updated_at=datetime('now') WHERE id=?", note, id);
            }

            // ---- applicant email on a real decision ----
            if (status.Length > 0 && status != "pending_review" && Notify.Enabled(db, "honorary"))
            {
                var baseUrl = Mailer.BaseUrl(ctx.Request);
                var (subject, body) = status switch
                {
                    "approved" => ("Congratulations — Honorary Fellow (PCI) conferred",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["first_name"]))},</p>" +
                        $"<p>The board has conferred on you the designation <strong>Honorary Fellow (PCI)</strong>. Your award number is <strong>{WebUtility.HtmlEncode(awardNo)}</strong>.</p>" +
                        // A student account is created automatically on approval; the set-password/login email is sent
                        // separately. Point the recipient at the portal, where their official certificate can be downloaded.
                        $"<p>We have set up your PCI student portal account — you will receive a separate email to choose your password and sign in. Once signed in, you can <strong>download your official Honorary Fellow (PCI) certificate</strong> from the Credentials area, along with your membership and learning resources.</p>" +
                        $"<p>You can verify your recognition any time at <a href=\"{baseUrl}/verify.html?id={WebUtility.HtmlEncode(awardNo)}\">{baseUrl}/verify.html?id={WebUtility.HtmlEncode(awardNo)}</a>. This is an honorary recognition, distinct from PCI's examined certification credentials.</p><p>— Project Controls Institute Global</p>"),
                    "rejected" => ("An update on your Honorary Fellow (PCI) application",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["first_name"]))},</p>" +
                        $"<p>Thank you for your interest in the Honorary Fellow (PCI) recognition. After careful review, the board is not taking your application forward at this time.</p>" +
                        (note.Length > 0 ? $"<p>{WebUtility.HtmlEncode(note)}</p>" : "") +
                        "<p>We are grateful for your contribution to the profession.</p><p>— Project Controls Institute Global</p>"),
                    _ => ("Your Honorary Fellow (PCI) application is under review",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["first_name"]))},</p><p>Your application (reference {WebUtility.HtmlEncode(H.Str(a["reference"]))}) is now <strong>under review</strong> by the board. We will be in touch once a decision has been made.</p><p>— Project Controls Institute Global</p>"),
                };
                Notify.Email(db, null, email, subject, body, "honorary_application", id);
            }

            return J(new { ok = true, status = status.Length > 0 ? status : H.Str(a["status"]), award_no = awardNo });
        });
    }
}
