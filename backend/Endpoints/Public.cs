using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>Public endpoints: pricing, code validation, verify, set-password, forgot, inquiry, newsletter, form-submit.</summary>
public static class Public
{
    static string[] CatsFor(string product) => product == "bundle" ? new[]{ "membership","exam" } : new[]{ product };

    public record PriceItem(string cat, double std, double disc, double payable);
    public record PriceResult(string currency, List<PriceItem> items, double standard, double defaultDiscount, double codeAmount, double final);

    public static PriceResult Pricing(Db db, string product, Dictionary<string, object?>? codeRow, Dictionary<string, object?>? cert = null)
    {
        var items = new List<PriceItem>(); double standard = 0, defDisc = 0;
        foreach (var cat in CatsFor(product))
        {
            var r = db.QueryOne("SELECT * FROM pricing_rules WHERE product_type=? AND active=1", cat);
            if (r is null) continue;
            var disc = H.D(r["default_discount_percentage"]) / 100.0;
            var stdp = H.D(r["standard_price"]);
            // A certification's own exam price (when set) overrides the generic exam pricing rule,
            // so each credential can be priced independently.
            if (cat == "exam" && cert is not null && cert["exam_price"] is not null) stdp = H.D(cert["exam_price"]);
            var payable = stdp * (1 - disc);
            standard += stdp; defDisc += stdp * disc;
            items.Add(new PriceItem(cat, stdp, disc, payable));
        }
        double codeAmount = 0;
        if (codeRow is not null)
        {
            var appliesTo = H.Str(codeRow["applies_to"]);
            var applicable = items.Where(it => appliesTo == "all" || appliesTo == it.cat).ToList();
            var baseAmt = applicable.Sum(it => it.payable);
            codeAmount = (H.Str(codeRow["discount_type"]) == "fixed")
                ? Math.Min(H.D(codeRow["discount_value"]), baseAmt)
                : baseAmt * (H.D(codeRow["discount_value"]) / 100.0);
            codeAmount = Math.Round(codeAmount * 100) / 100;
        }
        var final = Math.Max(0, standard - defDisc - codeAmount);
        return new PriceResult("USD", items, standard, defDisc, codeAmount, final);
    }

    public record CodeValidation(string? Error, Dictionary<string, object?>? Code);
    public static CodeValidation ValidateCode(Db db, string? code, string product, string? email)
    {
        var c = db.QueryOne("SELECT * FROM discount_codes WHERE code=?", (code ?? "").ToUpperInvariant());
        if (c is null || !H.B(c["active"])) return new("This discount code is not valid or has expired.", null);
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
        if (H.Str(c["end_date"]) is { } ed && string.Compare(ed, today, StringComparison.Ordinal) < 0) return new("This discount code has expired.", null);
        if (H.Str(c["start_date"]) is { } sd && string.Compare(sd, today, StringComparison.Ordinal) > 0) return new("This discount code is not yet active.", null);
        if (c["max_uses"] is not null && H.L(c["used_count"]) >= H.L(c["max_uses"])) return new("This discount code has reached its usage limit.", null);
        if (!string.IsNullOrEmpty(email) && (c["per_user_limit"] is not null || H.B(c["single_use_per_email"])))
        {
            var lim = c["per_user_limit"] is not null ? H.L(c["per_user_limit"]) : 1;
            var used = db.Scalar<long>("SELECT COUNT(*) FROM code_redemptions WHERE code_id=? AND email=?", c["id"], email!.ToLowerInvariant());
            if (used >= lim) return new("This code has already been used with this email address.", null);
        }
        var appliesTo = H.Str(c["applies_to"]);
        if (appliesTo != "all" && !CatsFor(product).Contains(appliesTo)) return new("This code does not apply to the selected membership or exam fee.", null);
        return new(null, c);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        var rx = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        // Effective pricing. An optional ?cert=CODE prices the exam (and bundle) for that specific
        // certification, so the checkout shows the real price of whichever credential was chosen — no
        // certification detail is hardcoded on the page. Omitting cert keeps the founding-cert behaviour.
        app.MapGet("/api/pricing", (HttpRequest req) =>
        {
            var certSel = req.Query["cert"].ToString();
            var cert = string.IsNullOrWhiteSpace(certSel) ? null : Certs.ById(db, Certs.Resolve(db, certSel));
            return J(new
            {
                currency = "USD",
                membership = Pricing(db, "membership", null),
                exam = Pricing(db, "exam", null, cert),
                bundle = Pricing(db, "bundle", null, cert),
                cert = cert is null ? null : new { code = cert["code"], name = cert["name"] },
            });
        });

        // Structured content overrides for one page (title, meta description, editable blocks). Used by
        // the client CMS loader and the admin live preview; the same values are also injected server-side.
        app.MapGet("/api/page-content", (HttpRequest req) =>
        {
            var slug = req.Query["slug"].ToString();
            if (string.IsNullOrEmpty(slug)) slug = "index.html";
            if (slug.Contains("..")) return Results.Json(new { error = "bad_slug" }, statusCode: 400);
            return J(PageContent.ForApi(db, slug));
        });

        // Public catalogue of ACTIVE certifications (safe fields only — never the bank or keys).
        // Each entry carries its effective exam price and headline exam parameters.
        app.MapGet("/api/certifications", () => J(new
        {
            rows = db.Query("SELECT id,code,name,description,expiry_years,sort_order FROM certifications WHERE active=1 ORDER BY sort_order,id")
                .Select(c => {
                    var cert = Certs.ById(db, c["id"]);
                    var cfg = Certs.Cfg(db, H.L(c["id"]));
                    return new { id = c["id"], code = c["code"], name = c["name"], description = c["description"],
                        expiry_years = c["expiry_years"], duration_minutes = (int)cfg.Duration, pass_mark_pct = cfg.Pass,
                        exam_price = Pricing(db, "exam", null, cert).final };
                }).ToList()
        }));

        app.MapPost("/api/validate-code", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var code = H.GetS(b, "code"); var product = H.GetS(b, "product") ?? "membership"; var email = H.GetS(b, "email");
            var v = ValidateCode(db, code, product, email);
            if (v.Error is not null) return J(new { valid = false, message = v.Error });
            var pr = Pricing(db, product, v.Code);
            return J(new { valid = true, code = v.Code!["code"], discount_type = v.Code["discount_type"], discount_value = v.Code["discount_value"], code_amount = pr.codeAmount, final_amount = pr.final, message = $"Discount applied: {v.Code["code"]}" });
        });

        app.MapGet("/api/verify", (HttpRequest req) =>
        {
            var id = (req.Query["id"].ToString() ?? "").Trim().ToUpperInvariant();
            if (id.Length == 0) return Results.Json(new { error = "missing_id" }, statusCode: 400);
            // Honorary awards live in their own registry with the PCI-HON prefix — a distinct number
            // space from every certification prefix, so the two record types can never collide. An
            // honorary result is explicitly typed and NEVER represented as a passed examination.
            if (id.StartsWith(Honorary.AwardPrefix + "-", StringComparison.Ordinal))
            {
                var hAward = db.QueryOne("SELECT award_no,recipient_name,citation,designation,status,conferred_at FROM honorary_awards WHERE upper(award_no)=?", id);
                if (hAward is null) return J(new { found = false });
                return J(new
                {
                    found = true,
                    type = "honorary",
                    designation = H.Str(hAward["designation"]) ?? "Honorary Fellow (PCI)",
                    recipient = hAward["recipient_name"],
                    citation = hAward["citation"],
                    award_no = hAward["award_no"],
                    state = H.Str(hAward["status"]) == "revoked" ? "revoked" : "active",
                    valid = H.Str(hAward["status"]) != "revoked",
                    conferred_at = hAward["conferred_at"],
                    note = "Honorary recognition conferred by the board — not an examined PCP-AI credential.",
                });
            }
            var c = db.QueryOne(@"SELECT ic.credential_id,ic.holder_name,ic.credential,ic.status,ic.issued_at,ic.expires_at,
                       ct.code certification_code, ct.name certification_name
                FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                WHERE upper(ic.credential_id)=?", id);
            if (c is null) return J(new { found = false });
            // Compute the real verification state: a credential whose expiry has passed is NOT valid even
            // if the stored status column still says 'active' (statuses are not batch-updated on expiry).
            var status = H.Str(c["status"]) ?? "active";
            var expires = H.Str(c["expires_at"]);
            var lapsed = status == "active" && H.IsPast(expires);
            var state = status == "revoked" ? "revoked" : (lapsed || status == "expired") ? "expired" : "active";
            var copy = new Dictionary<string, object?>(c) { ["found"] = true, ["state"] = state, ["valid"] = state == "active" };
            return J(copy);
        });

        app.MapPost("/api/set-password", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var token = H.GetS(b, "token"); var password = H.GetS(b, "password") ?? "";
            if (password.Length < 8) return Results.Json(new { error = "weak_password" }, statusCode: 400);
            var row = db.QueryOne("SELECT * FROM login_tokens WHERE token=? AND purpose='set_password' AND used_at IS NULL AND expires_at > datetime('now')", Security.Sha(token ?? ""));
            if (row is null) return Results.Json(new { error = "invalid_or_expired_token" }, statusCode: 400);
            db.Execute("UPDATE users SET password_hash=?, updated_at=datetime('now') WHERE id=?", BCrypt.Net.BCrypt.HashPassword(password), row["user_id"]);
            db.Execute("UPDATE login_tokens SET used_at=datetime('now') WHERE id=?", row["id"]);
            // Setting a new password revokes every existing session and burns any other outstanding
            // set-password/reset link for this user — a password reset means "log me out everywhere",
            // and a leaked older link must not remain usable after the account is recovered.
            db.Execute("DELETE FROM login_tokens WHERE user_id=? AND purpose='session'", row["user_id"]);
            db.Execute("UPDATE login_tokens SET used_at=datetime('now') WHERE user_id=? AND purpose='set_password' AND used_at IS NULL", row["user_id"]);
            log(H.Ln(row["user_id"]), "password_set", "via secure link");
            return J(new { ok = true });
        });

        app.MapPost("/api/forgot-password", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant().Trim();
            if (rx.IsMatch(email))
            {
                var u = db.QueryOne("SELECT * FROM users WHERE email=?", email);
                if (u is not null)
                {
                    var token = Security.RandomHex(32);
                    db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'set_password', datetime('now','+2 day'))", u["id"], Security.Sha(token));
                    var baseUrl = Mailer.BaseUrl(req);
                    Mailer.Send(db, H.Ln(u["id"]), email, "password_reset", "Reset your PCI password",
                        Mailer.Template("welcome", new() { ["FIRST_NAME"] = H.Str(u["first_name"]) ?? "there",
                            ["LOGIN_URL"] = Mailer.SetupLink(baseUrl, token), ["DOWNLOADS_URL"] = baseUrl + "/downloads.html" }));
                }
            }
            return J(new { ok = true }); // never reveal whether an account exists
        });

        app.MapPost("/api/inquiry", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = H.GetS(b, "email") ?? "";
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            var reference = "PCI-INQ-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("X");
            try { db.Execute("INSERT INTO inquiries(type,email,first_name,topic,seats,org,message,reference) VALUES(?,?,?,?,?,?,?,?)",
                H.GetS(b, "type") ?? "general", email, H.GetS(b, "first_name"), H.GetS(b, "topic"), H.GetS(b, "seats"), H.GetS(b, "org"), H.GetS(b, "message"), reference); } catch { }
            return J(new { ok = true, reference });
        });

        app.MapPost("/api/newsletter", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var email = (H.GetS(b, "email") ?? "").ToLowerInvariant().Trim();
            if (!rx.IsMatch(email)) return Results.Json(new { error = "invalid_email" }, statusCode: 400);
            try { db.Execute("INSERT OR IGNORE INTO newsletter_subscribers(email) VALUES(?)", email); } catch { }
            return J(new { ok = true });
        });

        app.MapPost("/api/form-submit", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var ft = H.GetS(b, "form_type") ?? "general";
            var reff = "PCI-" + System.Text.RegularExpressions.Regex.Replace(ft.ToUpperInvariant(), "[^A-Z]", "");
            reff = (reff.Length > 4 ? reff[..4] : reff) + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("X");
            try { db.Execute("INSERT INTO form_submissions(form_type,name,email,subject,message,reference) VALUES(?,?,?,?,?,?)",
                ft, H.GetS(b, "name"), H.GetS(b, "email"), H.GetS(b, "subject"), H.GetS(b, "message"), reff); } catch { }
            return J(new { ok = true, reference = reff });
        });
    }
}
