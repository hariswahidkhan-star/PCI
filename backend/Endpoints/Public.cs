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

    public static PriceResult Pricing(Db db, string product, Dictionary<string, object?>? codeRow)
    {
        var items = new List<PriceItem>(); double standard = 0, defDisc = 0;
        foreach (var cat in CatsFor(product))
        {
            var r = db.QueryOne("SELECT * FROM pricing_rules WHERE product_type=? AND active=1", cat);
            if (r is null) continue;
            var disc = H.D(r["default_discount_percentage"]) / 100.0;
            var stdp = H.D(r["standard_price"]);
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

        app.MapGet("/api/pricing", () => J(new { currency = "USD", membership = Pricing(db, "membership", null), exam = Pricing(db, "exam", null), bundle = Pricing(db, "bundle", null) }));

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
            var c = db.QueryOne("SELECT credential_id,holder_name,credential,status,issued_at,expires_at FROM issued_credentials WHERE upper(credential_id)=?", id);
            if (c is null) return J(new { found = false });
            // Compute the real verification state: a credential whose expiry has passed is NOT valid even
            // if the stored status column still says 'active' (statuses are not batch-updated on expiry).
            var status = H.Str(c["status"]) ?? "active";
            var expires = H.Str(c["expires_at"]);
            var lapsed = status == "active" && !string.IsNullOrEmpty(expires) && string.Compare(expires, H.IsoNow, StringComparison.Ordinal) < 0;
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
