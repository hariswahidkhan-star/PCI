using System.Text.Json;
using System.Net;
using System.Text.RegularExpressions;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Training Partner framework (master-plan Phase 7). Third-party training providers apply to be
/// recognised as PCI Training Partners; the board/admin reviews each application and, on approval,
/// a directory entry is created. Once an entry is published (listed=1) it renders on the public
/// website via <see cref="ListSections"/> (PCI-PARTNERS marker) — server-side, SEO-visible.
///
/// Certification stays independent of training: a partner delivers exam PREPARATION, never the
/// examination or the certification decision. This mirrors the honorary-application flow for the
/// public submit + document handling + admin review, and the generic CRUD pattern for the directory.
///
///   PUBLIC   POST /api/training-partner-application
///   ADMIN ('partners' permission)
///            GET   /api/admin/training-partner-applications[?status=]
///            GET   /api/admin/training-partner-applications/{id}
///            GET   /api/admin/training-partner-applications/{id}/documents/{docId}/file
///            POST  /api/admin/training-partner-applications/{id}/decide
///            GET   /api/admin/training-partners
///            POST  /api/admin/training-partners            (create)
///            PATCH /api/admin/training-partners/{id}        (update / publish)
///            POST  /api/admin/training-partners/{id}/delete
/// </summary>
public static class TrainingPartners
{
    static readonly HashSet<string> DocKinds = new() { "accreditation", "company_profile", "curriculum", "supporting" };
    static readonly string[] Tiers = { "registered", "authorized", "premier" };
    static readonly Regex EmailRx = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    static string Clip(string? s, int max) => (s ?? "").Trim() is { } t && t.Length > max ? t[..max] : (s ?? "").Trim();
    static string Tier(string? t) => Tiers.Contains((t ?? "").Trim().ToLowerInvariant()) ? (t ?? "").Trim().ToLowerInvariant() : "registered";

    // Truthy JSON flag: true / "1" / "true" / "yes" / non-zero number → true.
    static bool Flag(Dictionary<string, JsonElement> b, string key) =>
        H.GetEl(b, key) is { } v && (v.ValueKind == JsonValueKind.True
            || (v.ValueKind == JsonValueKind.String && v.GetString() is "1" or "true" or "yes")
            || (v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out var n) && n != 0));

    static string Slugify(Db db, string name)
    {
        var baseSlug = Regex.Replace(name.ToLowerInvariant(), @"[^a-z0-9]+", "-").Trim('-');
        if (baseSlug.Length == 0) baseSlug = "partner";
        if (baseSlug.Length > 80) baseSlug = baseSlug[..80].Trim('-');
        var slug = baseSlug;
        for (int i = 2; db.QueryOne("SELECT id FROM training_partners WHERE slug=?", slug) is not null; i++)
            slug = baseSlug + "-" + i;
        return slug;
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        // Authorise a mutating admin call (needs the AdminCtx for decided_by/audit) before reading the body.
        (AdminCtx? adm, IResult? deny) Admin(HttpRequest req)
        {
            AdminCtx? adm = null;
            var r = gate(req, "partners", a => { adm = a; return Results.Ok(); });
            return r is Microsoft.AspNetCore.Http.HttpResults.Ok ? (adm, null) : (null, r);
        }

        // ==================== PUBLIC: submit an application ====================
        app.MapPost("/api/training-partner-application", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            string S(string k, int max, params string[] more) => Clip(H.GetS(b, new[] { k }.Concat(more).ToArray()), max);

            var org = S("org_name", 160, "organisation", "organization", "company");
            var website = S("website", 200);
            var contactName = S("contact_name", 120, "contactName");
            var email = S("contact_email", 160, "email").ToLowerInvariant();
            var phone = S("contact_phone", 40, "phone", "mobile");
            var country = S("country", 80);
            var city = S("city", 80);
            var region = S("region", 80);
            var deliveryModes = S("delivery_modes", 200, "deliveryModes");
            var specialties = S("specialties", 2000, "specialisms");
            var learners = (int)Math.Clamp(H.GetNum(b, "learners_per_year", "learnersPerYear") ?? 0, 0, 1_000_000);
            var description = S("description", 6000);
            var declaration = H.GetEl(b, "declaration") is { } de && (de.ValueKind == JsonValueKind.True
                || (de.ValueKind == JsonValueKind.String && de.GetString() is "1" or "true" or "yes"));

            // ---- required-field + format validation (server-side; the form also checks client-side) ----
            if (org.Length == 0) return Results.Json(new { error = "org_name_required" }, statusCode: 400);
            if (contactName.Length == 0) return Results.Json(new { error = "contact_name_required" }, statusCode: 400);
            if (!EmailRx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            foreach (var (val, code) in new[] { (website, "website_required"), (phone, "phone_required"),
                (country, "country_required"), (city, "city_required"), (specialties, "specialties_required"),
                (description, "description_required") })
                if (val.Length == 0) return Results.Json(new { error = code }, statusCode: 400);
            if (!declaration) return Results.Json(new { error = "declaration_required", message = "The partner declaration must be accepted." }, statusCode: 400);

            // ---- validate + store documents (Storage enforces MIME allow-list + 3 MB magic-byte check) ----
            var docsEl = H.GetEl(b, "documents");
            var toStore = new List<(string kind, string name, Storage.StoredObject obj)>();
            if (docsEl is { ValueKind: JsonValueKind.Array } arr)
            {
                if (arr.GetArrayLength() > 8) return Results.Json(new { error = "too_many_documents", message = "Please attach at most 8 documents." }, statusCode: 400);
                foreach (var d in arr.EnumerateArray())
                {
                    string? P(string k) => d.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;
                    var kind = (P("doc_kind") ?? P("kind") ?? "supporting").Trim().ToLowerInvariant();
                    if (!DocKinds.Contains(kind)) kind = "supporting";
                    var dataUri = P("data_uri") ?? P("dataUri") ?? P("data");
                    if (string.IsNullOrWhiteSpace(dataUri)) continue;      // empty optional slot
                    var (bytes, mime, err) = Storage.DecodeDataUri(dataUri);
                    if (err is not null) return Results.Json(new { error = err, doc_kind = kind }, statusCode: 400);
                    var obj = Storage.Put(bytes!, mime, "partners");
                    toStore.Add((kind, Clip(P("filename") ?? P("name"), 200), obj));
                }
            }

            // ---- insert application (unique reference PCI-TPA-YYYY-NNNN) + documents ----
            var yr = DateTime.UtcNow.Year; var rnd = new Random();
            long appId = 0; string? reference = null;
            for (int i = 0; i < 20 && reference is null; i++)
            {
                var cand = i < 10 ? $"PCI-TPA-{yr}-{1000 + rnd.Next(0, 8999)}" : $"PCI-TPA-{yr}-{10000 + rnd.Next(0, 89999)}";
                try
                {
                    appId = db.ExecuteReturningId(@"INSERT INTO training_partner_applications
                        (reference,org_name,website,contact_name,contact_email,contact_phone,country,city,region,delivery_modes,specialties,learners_per_year,description,declaration,status)
                        VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,1,'pending_review')",
                        cand, org, website, contactName, email, phone, country, city, region, deliveryModes, specialties, learners, description);
                    reference = cand;
                }
                catch { /* reference collision → retry */ }
            }
            if (reference is null) return Results.Json(new { error = "reference_generation_failed" }, statusCode: 500);
            foreach (var (kind, name, obj) in toStore)
                db.Execute("INSERT INTO training_partner_application_documents(application_id,doc_kind,filename,mime,size_bytes,storage_ref,sha256) VALUES(?,?,?,?,?,?,?)",
                    appId, kind, name, obj.Mime, obj.SizeBytes, obj.Reference, obj.Sha256);
            log(null, "training_partner_application_submitted", $"{reference} <{email}> {org}");

            // ---- notifications (email now; recipients/enable are settings-driven, never hardcoded) ----
            if (Notify.Enabled(db, "partners"))
            {
                var baseUrl = Mailer.BaseUrl(req);
                var when = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'");
                Notify.Email(db, null, email, "We've received your PCI Training Partner application",
                    $"<p>Dear {WebUtility.HtmlEncode(contactName)},</p>" +
                    $"<p>Thank you for applying for <strong>{WebUtility.HtmlEncode(org)}</strong> to become a recognised <strong>PCI Training Partner</strong>. " +
                    $"Your application has been received and is <strong>under review</strong>.</p>" +
                    $"<p>Your application reference is <strong>{WebUtility.HtmlEncode(reference)}</strong> — please quote it in any correspondence.</p>" +
                    $"<p>PCI Training Partners deliver examination preparation; the examination and the certification decision remain independent of any training provider. We will email you once a decision has been made.</p>" +
                    $"<p>— Project Controls Institute Global</p>",
                    "training_partner_application", appId);

                var adminTo = Notify.AdminEmail(db);
                Notify.Email(db, null, adminTo, "New PCI Training Partner application",
                    $"<p>A new Training Partner application has been submitted.</p>" +
                    $"<table cellpadding='4'><tr><td><strong>Organisation</strong></td><td>{WebUtility.HtmlEncode(org)}</td></tr>" +
                    $"<tr><td><strong>Contact</strong></td><td>{WebUtility.HtmlEncode(contactName)} &lt;{WebUtility.HtmlEncode(email)}&gt;</td></tr>" +
                    $"<tr><td><strong>Country</strong></td><td>{WebUtility.HtmlEncode(country)}</td></tr>" +
                    $"<tr><td><strong>Reference</strong></td><td>{WebUtility.HtmlEncode(reference)}</td></tr>" +
                    $"<tr><td><strong>Submitted</strong></td><td>{when}</td></tr>" +
                    $"<tr><td><strong>Documents</strong></td><td>{toStore.Count} attached</td></tr></table>" +
                    $"<p>Review it in the admin portal: <a href=\"{baseUrl}/admin/\">{baseUrl}/admin/</a> → Training Partners → Applications.</p>",
                    "training_partner_application", appId);
            }

            return J(new { ok = true, reference, message = "Application received — check your email for confirmation." });
        });

        // ==================== ADMIN: applications ====================
        app.MapGet("/api/admin/training-partner-applications", (HttpRequest req) => gate(req, "partners", _ =>
        {
            var status = req.Query["status"].ToString();
            var rows = string.IsNullOrEmpty(status)
                ? db.Query(@"SELECT a.*, (SELECT COUNT(*) FROM training_partner_application_documents d WHERE d.application_id=a.id) AS doc_count FROM training_partner_applications a ORDER BY a.id DESC LIMIT 500")
                : db.Query(@"SELECT a.*, (SELECT COUNT(*) FROM training_partner_application_documents d WHERE d.application_id=a.id) AS doc_count FROM training_partner_applications a WHERE a.status=? ORDER BY a.id DESC LIMIT 500", status);
            return J(new { rows });
        }));

        app.MapGet("/api/admin/training-partner-applications/{id}", (HttpRequest req, long id) => gate(req, "partners", _ =>
        {
            var a = db.QueryOne("SELECT * FROM training_partner_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var docs = db.Query("SELECT id,doc_kind,filename,mime,size_bytes,created_at FROM training_partner_application_documents WHERE application_id=? ORDER BY id", id);
            return J(new { application = a, documents = docs });
        }));

        app.MapGet("/api/admin/training-partner-applications/{id}/documents/{docId}/file", (HttpRequest req, long id, long docId) => gate(req, "partners", _ =>
        {
            var d = db.QueryOne("SELECT storage_ref,mime,filename FROM training_partner_application_documents WHERE id=? AND application_id=?", docId, id);
            if (d is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var got = Storage.Get(H.Str(d["storage_ref"]));
            if (got is null || got.Value.bytes is null) return Results.Json(new { error = "file_unavailable" }, statusCode: 404);
            return Results.File(got.Value.bytes!, got.Value.mime);
        }));

        app.MapPost("/api/admin/training-partner-applications/{id}/decide", async (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Admin(ctx.Request); if (deny is not null) return deny;
            var a = db.QueryOne("SELECT * FROM training_partner_applications WHERE id=?", id);
            if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            var status = (H.GetS(b, "status") ?? "").Trim().ToLowerInvariant();
            var note = Clip(H.GetS(b, "admin_note", "note"), 2000);
            var tier = Tier(H.GetS(b, "tier", "proposed_tier"));
            var validStatuses = new[] { "under_review", "approved", "rejected", "pending_review" };
            if (status.Length > 0 && !validStatuses.Contains(status))
                return Results.Json(new { error = "bad_status" }, statusCode: 400);
            if (H.Str(a["status"]) == "approved" && status is "approved" or "rejected")
                return Results.Json(new { error = "already_decided", message = "This application has already been approved." }, statusCode: 409);

            long? partnerId = a["partner_id"] is null ? null : H.L(a["partner_id"]);
            var org = H.Str(a["org_name"]) ?? "";
            var email = H.Str(a["contact_email"]) ?? "";

            if (status == "approved")
            {
                // Create the directory entry (unlisted — the admin publishes it after a final review).
                var slug = Slugify(db, org);
                partnerId = db.ExecuteReturningId(@"INSERT INTO training_partners
                    (name,slug,tier,country,region,city,website,summary,description,specialties,contact_email,listed,source_application_id,approved_at)
                    VALUES(?,?,?,?,?,?,?,?,?,?,?,0,?,datetime('now'))",
                    org, slug, tier, H.Str(a["country"]), H.Str(a["region"]), H.Str(a["city"]), H.Str(a["website"]),
                    Clip(H.Str(a["description"]), 300), H.Str(a["description"]), H.Str(a["specialties"]), email, id);
                db.Execute("UPDATE training_partner_applications SET status='approved', proposed_tier=?, partner_id=?, decided_by=?, decided_at=datetime('now'), admin_note=?, updated_at=datetime('now') WHERE id=?",
                    tier, partnerId, adm!.Id, note, id);
                ListSections.Bump();
                log(null, "training_partner_application_approved", $"{H.Str(a["reference"])} → partner {partnerId} ({tier}) by admin {adm.Id}");
            }
            else if (status.Length > 0)
            {
                db.Execute("UPDATE training_partner_applications SET status=?, decided_by=?, decided_at=datetime('now'), admin_note=?, updated_at=datetime('now') WHERE id=?",
                    status, adm!.Id, note, id);
                log(null, "training_partner_application_" + status, $"{H.Str(a["reference"])} by admin {adm.Id}");
            }
            else
            {
                db.Execute("UPDATE training_partner_applications SET admin_note=?, updated_at=datetime('now') WHERE id=?", note, id);
            }

            // ---- applicant email on a real decision ----
            if (status.Length > 0 && status != "pending_review" && Notify.Enabled(db, "partners"))
            {
                var (subject, body) = status switch
                {
                    "approved" => ("Congratulations — your organisation is now a PCI Training Partner",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["contact_name"]))},</p>" +
                        $"<p>We are pleased to confirm that <strong>{WebUtility.HtmlEncode(org)}</strong> has been approved as a <strong>PCI Training Partner</strong> at the <strong>{WebUtility.HtmlEncode(tier)}</strong> tier.</p>" +
                        (note.Length > 0 ? $"<p>{WebUtility.HtmlEncode(note)}</p>" : "") +
                        "<p>Your directory listing will appear on the PCI website once our team completes final publication. As a reminder, the examination and certification decision remain independent of training delivery.</p><p>— Project Controls Institute Global</p>"),
                    "rejected" => ("An update on your PCI Training Partner application",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["contact_name"]))},</p>" +
                        $"<p>Thank you for your interest in the PCI Training Partner programme. After review, we are not taking your application forward at this time.</p>" +
                        (note.Length > 0 ? $"<p>{WebUtility.HtmlEncode(note)}</p>" : "") +
                        "<p>You are welcome to apply again in the future.</p><p>— Project Controls Institute Global</p>"),
                    _ => ("Your PCI Training Partner application is under review",
                        $"<p>Dear {WebUtility.HtmlEncode(H.Str(a["contact_name"]))},</p><p>Your application (reference {WebUtility.HtmlEncode(H.Str(a["reference"]))}) is now <strong>under review</strong>. We will be in touch once a decision has been made.</p><p>— Project Controls Institute Global</p>"),
                };
                Notify.Email(db, null, email, subject, body, "training_partner_application", id);
            }

            return J(new { ok = true, status = status.Length > 0 ? status : H.Str(a["status"]), partner_id = partnerId });
        });

        // ==================== ADMIN: partner directory (CRUD) ====================
        app.MapGet("/api/admin/training-partners", (HttpRequest req) => gate(req, "partners", _ =>
            J(new { rows = db.Query("SELECT * FROM training_partners ORDER BY sort_order, name") })));

        app.MapPost("/api/admin/training-partners", async (HttpContext ctx) =>
        {
            var (adm, deny) = Admin(ctx.Request); if (deny is not null) return deny;
            var b = await H.Body(ctx.Request);
            var name = Clip(H.GetS(b, "name"), 160);
            if (name.Length == 0) return Results.Json(new { error = "name_required" }, statusCode: 400);
            var slug = Slugify(db, name);
            var id = db.ExecuteReturningId(@"INSERT INTO training_partners
                (name,slug,tier,country,region,city,website,logo_url,summary,description,specialties,contact_email,listed,sort_order)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)",
                name, slug, Tier(H.GetS(b, "tier")), Clip(H.GetS(b, "country"), 80), Clip(H.GetS(b, "region"), 80),
                Clip(H.GetS(b, "city"), 80), Clip(H.GetS(b, "website"), 200), Clip(H.GetS(b, "logo_url"), 300),
                Clip(H.GetS(b, "summary"), 300), Clip(H.GetS(b, "description"), 6000), Clip(H.GetS(b, "specialties"), 2000),
                Clip(H.GetS(b, "contact_email"), 160), Flag(b, "listed") ? 1 : 0, (int)(H.GetNum(b, "sort_order") ?? 0));
            ListSections.Bump();
            log(adm!.Id, "training_partner_create", $"{id} {name}");
            return J(new { ok = true, id, slug });
        });

        app.MapPatch("/api/admin/training-partners/{id}", async (HttpContext ctx, long id) =>
        {
            var (adm, deny) = Admin(ctx.Request); if (deny is not null) return deny;
            var p = db.QueryOne("SELECT * FROM training_partners WHERE id=?", id);
            if (p is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var b = await H.Body(ctx.Request);
            // Only the fields present in the body are updated (partial update).
            var sets = new List<string>(); var args = new List<object?>();
            void Str(string col, string key, int max) { if (H.GetS(b, key) is { } v) { sets.Add(col + "=?"); args.Add(Clip(v, max)); } }
            if (H.GetS(b, "name") is { } nm && Clip(nm, 160).Length > 0) { sets.Add("name=?"); args.Add(Clip(nm, 160)); }
            if (H.GetS(b, "tier") is { } _) { sets.Add("tier=?"); args.Add(Tier(H.GetS(b, "tier"))); }
            Str("country", "country", 80); Str("region", "region", 80); Str("city", "city", 80);
            Str("website", "website", 200); Str("logo_url", "logo_url", 300); Str("summary", "summary", 300);
            Str("description", "description", 6000); Str("specialties", "specialties", 2000); Str("contact_email", "contact_email", 160);
            if (H.GetEl(b, "listed") is not null) { sets.Add("listed=?"); args.Add(Flag(b, "listed") ? 1 : 0); }
            if (H.GetEl(b, "sort_order") is not null) { sets.Add("sort_order=?"); args.Add((int)(H.GetNum(b, "sort_order") ?? 0)); }
            if (sets.Count == 0) return J(new { ok = true, unchanged = true });
            sets.Add("updated_at=datetime('now')");
            args.Add(id);
            db.Execute($"UPDATE training_partners SET {string.Join(",", sets)} WHERE id=?", args.ToArray());
            ListSections.Bump();
            log(adm!.Id, "training_partner_update", id.ToString());
            return J(new { ok = true });
        });

        app.MapPost("/api/admin/training-partners/{id}/delete", (HttpRequest req, long id) => gate(req, "partners", adm =>
        {
            db.Execute("DELETE FROM training_partners WHERE id=?", id);
            ListSections.Bump();
            log(adm.Id, "training_partner_delete", id.ToString());
            return J(new { ok = true });
        }));
    }
}
