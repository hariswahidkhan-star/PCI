using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Public member directory — "find a certified professional". Privacy-first and opt-in: a member appears
/// ONLY when they have switched the listing on AND hold at least one active credential. Per-field toggles
/// decide whether their country, organisation and LinkedIn are shown; email and other PII are never exposed.
/// Test accounts and revoked/expired-only holders never appear.
/// </summary>
public static class MemberDirectory
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);

        // Active, non-test credential-holders who opted in. One row per member with their credential acronyms.
        List<Dictionary<string, object?>> Listed(string extraWhere, params object?[] args)
        {
            var rows = db.Query($@"SELECT u.id, u.first_name, u.last_name, u.created_at,
                    p.country, p.current_role, p.company, p.linkedin_url, p.directory_headline,
                    p.directory_show_country, p.directory_show_org, p.directory_show_linkedin
                FROM users u JOIN student_profiles p ON p.user_id=u.id
                WHERE p.directory_opt_in=1 AND COALESCE(u.is_test,0)=0 AND u.status='active'
                  AND EXISTS (SELECT 1 FROM issued_credentials ic WHERE ic.user_id=u.id AND ic.status='active')
                  {extraWhere}
                ORDER BY u.first_name, u.last_name", args);
            foreach (var r in rows)
            {
                var creds = db.Query(@"SELECT ic.credential_id, COALESCE(ct.acronym, ic.credential) acronym, COALESCE(ct.name, ic.credential) name
                    FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                    WHERE ic.user_id=? AND ic.status='active' ORDER BY ic.id DESC", r["id"]);
                r["credentials"] = creds.Select(c => new { credential_id = H.Str(c["credential_id"]), acronym = H.Str(c["acronym"]), name = H.Str(c["name"]) }).ToList();
                // Apply the member's per-field consent, then drop the raw fields.
                var showCountry = H.B(r["directory_show_country"]);
                var showOrg = H.B(r["directory_show_org"]);
                var showLinkedin = H.B(r["directory_show_linkedin"]);
                r["name"] = ($"{H.Str(r["first_name"])} {H.Str(r["last_name"])}").Trim();
                r["headline"] = H.Str(r["directory_headline"]) is { Length: > 0 } h ? h
                    : (showOrg && H.Str(r["current_role"]) is { Length: > 0 } cr ? cr + (H.Str(r["company"]) is { Length: > 0 } co ? " · " + co : "") : null);
                r["country"] = showCountry ? H.Str(r["country"]) : null;
                r["linkedin"] = showLinkedin ? H.Str(r["linkedin_url"]) : null;
                r["member_since"] = H.Str(r["created_at"]);
                foreach (var k in new[] { "first_name", "last_name", "current_role", "company", "linkedin_url", "directory_headline", "directory_show_country", "directory_show_org", "directory_show_linkedin", "created_at" })
                    r.Remove(k);
            }
            return rows;
        }

        // ─────────────────────────── PUBLIC ───────────────────────────
        app.MapGet("/api/directory", (HttpRequest req) =>
        {
            var q = (req.Query["q"].ToString() ?? "").Trim();
            var country = req.Query["country"].ToString();
            var cert = req.Query["cert"].ToString();
            var where = ""; var args = new List<object?>();
            if (q.Length > 0) { where += " AND (u.first_name LIKE ? OR u.last_name LIKE ? OR p.current_role LIKE ? OR p.company LIKE ? OR p.directory_headline LIKE ?)"; var like = "%" + q + "%"; args.AddRange(new object?[] { like, like, like, like, like }); }
            if (!string.IsNullOrWhiteSpace(country)) { where += " AND p.directory_show_country=1 AND p.country=?"; args.Add(country); }
            if (Certs.TryResolve(db, cert) is { } cid) { where += " AND EXISTS (SELECT 1 FROM issued_credentials ic2 WHERE ic2.user_id=u.id AND ic2.status='active' AND COALESCE(ic2.certification_id,1)=?)"; args.Add(cid); }
            var rows = Listed(where, args.ToArray());
            // Facets for the filter UI: countries (of consenting members) and the active certifications.
            var countries = db.Query(@"SELECT DISTINCT p.country FROM student_profiles p JOIN users u ON u.id=p.user_id
                WHERE p.directory_opt_in=1 AND p.directory_show_country=1 AND p.country IS NOT NULL AND p.country<>'' AND COALESCE(u.is_test,0)=0
                  AND EXISTS (SELECT 1 FROM issued_credentials ic WHERE ic.user_id=u.id AND ic.status='active') ORDER BY p.country")
                .Select(r => H.Str(r["country"])).ToList();
            var certs = db.Query("SELECT code, COALESCE(acronym,code) acronym, name FROM certifications WHERE active=1 ORDER BY sort_order, id")
                .Select(r => new { code = H.Str(r["code"]), acronym = H.Str(r["acronym"]), name = H.Str(r["name"]) }).ToList();
            return J(new { rows, countries, certifications = certs, total = rows.Count });
        });

        // ─────────────────────────── STUDENT (manage own listing) ───────────────────────────
        app.MapGet("/api/me/directory", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var p = db.QueryOne("SELECT directory_opt_in,directory_headline,directory_show_country,directory_show_org,directory_show_linkedin FROM student_profiles WHERE user_id=?", u.Id);
            var eligible = db.QueryOne("SELECT id FROM issued_credentials WHERE user_id=? AND status='active'", u.Id) is not null;
            return J(new
            {
                eligible,   // a member can opt in any time, but only appears once they hold an active credential
                opt_in = H.B(p?["directory_opt_in"]),
                headline = H.Str(p?["directory_headline"]),
                show_country = p is null || H.B(p["directory_show_country"]),
                show_org = p is null || H.B(p["directory_show_org"]),
                show_linkedin = H.B(p?["directory_show_linkedin"]),
            });
        });

        app.MapPost("/api/me/directory", async (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            if (db.QueryOne("SELECT user_id FROM student_profiles WHERE user_id=?", u.Id) is null)
                db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", u.Id);
            var headline = (H.GetS(b, "headline") ?? "").Trim(); if (headline.Length > 160) headline = headline[..160];
            db.Execute(@"UPDATE student_profiles SET directory_opt_in=?, directory_headline=?, directory_show_country=?, directory_show_org=?, directory_show_linkedin=? WHERE user_id=?",
                (H.GetBool(b, "opt_in") ?? false) ? 1 : 0, headline.Length > 0 ? headline : null,
                (H.GetBool(b, "show_country") ?? true) ? 1 : 0, (H.GetBool(b, "show_org") ?? true) ? 1 : 0, (H.GetBool(b, "show_linkedin") ?? false) ? 1 : 0, u.Id);
            log(u.Id, "directory_prefs", (H.GetBool(b, "opt_in") ?? false) ? "listed" : "unlisted");
            return J(new { ok = true });
        });

        // ─────────────────────────── ADMIN (moderation) ───────────────────────────
        app.MapGet("/api/admin/directory", (HttpRequest req) => gate(req, "members", _ =>
            J(new { rows = Listed("") })));

        app.MapPost("/api/admin/directory/{userId:long}/unlist", (HttpRequest req, long userId) => gate(req, "members", adm =>
        {
            db.Execute("UPDATE student_profiles SET directory_opt_in=0 WHERE user_id=?", userId);
            log(userId, "directory_unlisted_by_admin", adm.Id.ToString());
            return J(new { ok = true });
        }));
    }
}
