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
            // Below the minimum transaction the code simply does not apply.
            if (codeRow.TryGetValue("min_transaction", out var mt) && mt is not null && baseAmt < H.D(mt))
                baseAmt = 0;
            codeAmount = (H.Str(codeRow["discount_type"]) == "fixed")
                ? Math.Min(H.D(codeRow["discount_value"]), baseAmt)
                : baseAmt * (H.D(codeRow["discount_value"]) / 100.0);
            // Cap the discount when a maximum is configured.
            if (codeRow.TryGetValue("max_discount", out var md) && md is not null)
                codeAmount = Math.Min(codeAmount, H.D(md));
            codeAmount = Math.Round(codeAmount * 100) / 100;
        }
        var final = Math.Max(0, standard - defDisc - codeAmount);
        return new PriceResult("USD", items, standard, defDisc, codeAmount, final);
    }

    public record CodeValidation(string? Error, Dictionary<string, object?>? Code);
    public static CodeValidation ValidateCode(Db db, string? code, string product, string? email, long? certId = null)
    {
        var c = db.QueryOne("SELECT * FROM discount_codes WHERE code=?", (code ?? "").ToUpperInvariant());
        if (c is null) return new("This discount code is not valid or has expired.", null);
        // Discount-engine lifecycle: engine-managed rows carry a status; legacy rows (status NULL) are
        // governed by the active flag alone.
        switch (H.Str(c["status"]))
        {
            case "draft": case "pending_approval": return new("This code is not active yet.", null);
            case "suspended": return new("This code is currently suspended. Contact support if you believe this is an error.", null);
            case "rejected": case "cancelled": return new("This discount code is no longer available.", null);
        }
        if (!H.B(c["active"])) return new("This discount code is not valid or has expired.", null);
        // Per-certification scope: a code tied to one credential is rejected for any other. NULL = all certs.
        if (c["certification_id"] is not null && certId is not null && H.L(c["certification_id"]) != certId.Value)
        {
            var scoped = Certs.ById(db, c["certification_id"]);
            var label = scoped is null ? "another certification" : (H.Str(scoped["acronym"]) ?? H.Str(scoped["code"]) ?? "another certification");
            return new($"This code is only valid for {label}, not the certification you selected.", null);
        }
        // A founding code is not a discount — it opens the free founding route and is redeemed in the
        // portal's Founding access card. If one is pasted into the discount field, say so plainly rather
        // than silently accepting it as a 0% code and charging full price.
        if (!string.IsNullOrEmpty(H.Str(c["founding_route"])))
            return new("That's a founding code — redeem it in the Founding access card, not as a discount.", null);
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
        if (appliesTo != "all" && !CatsFor(product).Contains(appliesTo))
            return new($"This code only applies to {(appliesTo == "exam" ? "the exam fee" : "membership")}, not this purchase.", null);
        // A partial-waiver code is issued to one named student: it only validates for that email.
        if (H.Str(c["code_type"]) == "waiver" && H.Str(c["criteria_json"]) is { Length: > 0 } cj)
        {
            try
            {
                var doc = System.Text.Json.JsonDocument.Parse(cj);
                if (doc.RootElement.TryGetProperty("email", out var em) && em.GetString() is { Length: > 0 } lockEmail
                    && !string.Equals(lockEmail, email ?? "", StringComparison.OrdinalIgnoreCase))
                    return new("This code was issued to a specific student account and cannot be used here.", null);
            }
            catch { }
        }
        // An institution (training-partner) code stops honouring redemptions once the partner's total
        // allocation is spent, even if this individual code still has headroom — and it only works at
        // all while the institution itself is active and within its agreement dates.
        if (c["partner_id"] is not null)
        {
            var pid = H.L(c["partner_id"]);
            var partner = db.QueryOne("SELECT status,agreement_end,total_allocation FROM training_partners WHERE id=?", pid);
            if (partner is null || (H.Str(partner["status"]) ?? "active") != "active")
                return new("This institution's codes are not currently available.", null);
            if (H.Str(partner["agreement_end"]) is { Length: > 0 } ae && string.Compare(ae, DateTime.UtcNow.ToString("yyyy-MM-dd"), StringComparison.Ordinal) < 0)
                return new("This institution's agreement has ended; the code can no longer be used.", null);
            if (partner["total_allocation"] is not null)
            {
                var usedTotal = db.Scalar<long>("SELECT COALESCE(SUM(dc.used_count),0) FROM discount_codes dc WHERE dc.partner_id=?", pid);
                if (usedTotal >= H.L(partner["total_allocation"]))
                    return new("This institution's sponsorship allocation has been fully used.", null);
            }
        }
        // Country eligibility: when a code names eligible countries, the student's profile country must
        // match (checked by email so it also works at public checkout).
        if (H.Str(c["eligible_countries"]) is { Length: > 0 } ec && !string.IsNullOrEmpty(email))
        {
            var country = db.Scalar<string>(@"SELECT sp.country FROM users u2 JOIN student_profiles sp ON sp.user_id=u2.id WHERE u2.email=?", email!.ToLowerInvariant());
            var allowed = ec.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (country is { Length: > 0 } && !allowed.Contains(country, StringComparer.OrdinalIgnoreCase))
                return new("This code is not available in your country.", null);
        }
        return new(null, c);
    }

    /// <summary>Enforce the code's minimum-payable floor against a computed price. Call after Pricing().</summary>
    public static string? MinPayableViolation(Dictionary<string, object?> code, double finalAmount)
    {
        if (code["min_payable"] is null) return null;
        var min = H.D(code["min_payable"]);
        return finalAmount < min ? $"This code cannot reduce the amount payable below ${min:0.##}." : null;
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
                // Renewal (3-year membership extension) and recertification (3-year credential cycle) are
                // real purchase products with their own price book rules; the portal surfaces a pay button
                // for each once a member/credential is inside its renewal window.
                renewal = Pricing(db, "renewal", null),
                recert = Pricing(db, "recert", null),
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
            rows = db.Query("SELECT id,code,name,acronym,short_description,slug,category,status,description,expiry_years,sort_order FROM certifications WHERE active=1 ORDER BY sort_order,id")
                .Select(c => {
                    var cert = Certs.ById(db, c["id"]);
                    var cfg = Certs.Cfg(db, H.L(c["id"]));
                    return new { id = c["id"], code = c["code"], name = c["name"],
                        acronym = c["acronym"], short_description = c["short_description"], slug = c["slug"],
                        category = c["category"], status = c["status"], description = c["description"],
                        expiry_years = c["expiry_years"], duration_minutes = (int)cfg.Duration, pass_mark_pct = cfg.Pass,
                        exam_price = Pricing(db, "exam", null, cert).final };
                }).ToList()
        }));

        // Public application routes for a certification (used by the student apply form). Internal routes
        // (e.g. Test User) are filtered out; each entry says whether the route requires review or an exam.
        app.MapGet("/api/certifications/{id}/routes", (string id) =>
        {
            var certSel = Certs.TryResolve(db, id);
            if (certSel is null || Certs.ById(db, certSel.Value) is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var certId = certSel.Value;
            var rows = Routes.For(db, certId, publicOnly: true).Select(r => new
            {
                route_key = r["route_key"], label = r["label"], description = r["description"],
                requires_approval = H.B(r["requires_approval"]), exam_required = H.B(r["exam_required"]),
                fee_mode = r["fee_mode"],
            }).ToList();
            return J(new { certification_id = certId, rows });
        });

        app.MapPost("/api/validate-code", async (HttpRequest req) =>
        {
            var b = await H.Body(req);
            var code = H.GetS(b, "code"); var product = H.GetS(b, "product") ?? "membership"; var email = H.GetS(b, "email");
            // The selected certification scopes both the code validity and the exam price.
            var certSel = H.GetS(b, "cert");
            var certResolved = string.IsNullOrWhiteSpace(certSel) ? null : Certs.TryResolve(db, certSel);
            if (!string.IsNullOrWhiteSpace(certSel) && certResolved is null)
                return J(new { valid = false, message = "Unknown certification." });
            var certRow = certResolved is null ? null : Certs.ById(db, certResolved.Value);
            var certId = certRow is null ? (long?)null : H.L(certRow["id"]);
            var v = ValidateCode(db, code, product, email, certId);
            if (v.Error is not null) return J(new { valid = false, message = v.Error });
            var pr = Pricing(db, product, v.Code, certRow);
            var scope = H.Str(v.Code!["applies_to"]) ?? "all";
            var scopeLabel = scope == "membership" ? "membership" : scope == "exam" ? "the exam fee" : "membership and exam fees";
            return J(new { valid = true, code = v.Code["code"], applies_to = scope, discount_type = v.Code["discount_type"], discount_value = v.Code["discount_value"], code_amount = pr.codeAmount, final_amount = pr.final, message = $"Code {v.Code["code"]} applies to {scopeLabel}." });
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
                var hAward = db.QueryOne("SELECT award_no,recipient_name,citation,designation,status,conferred_at,pdf_sha256 FROM honorary_awards WHERE upper(award_no)=?", id);
                if (hAward is null) return J(new { found = false });
                var hHash = H.Str(hAward["pdf_sha256"]);
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
                    document_hash = hHash,
                    has_pdf = !string.IsNullOrEmpty(hHash),
                    note = "Honorary recognition conferred by the board — not an examined certification credential.",
                });
            }
            // Test-account credentials are workflow artefacts, never real certifications: the public
            // register reports them as not found so a test run can never mint a verifiable credential.
            var c = db.QueryOne(@"SELECT ic.credential_id,ic.holder_name,ic.credential,ic.status,ic.issued_at,ic.expires_at,ic.pdf_sha256,ic.certificate_wording,
                       ct.code certification_code, ct.name certification_name, ct.acronym certification_acronym
                FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                LEFT JOIN users tu ON tu.id=ic.user_id
                WHERE upper(ic.credential_id)=? AND COALESCE(tu.is_test,0)=0", id);
            if (c is null) return J(new { found = false });
            // Compute the real verification state: a credential whose expiry has passed is NOT valid even
            // if the stored status column still says 'active' (statuses are not batch-updated on expiry).
            var status = H.Str(c["status"]) ?? "active";
            var expires = H.Str(c["expires_at"]);
            var lapsed = status == "active" && H.IsPast(expires);
            var state = status == "revoked" ? "revoked" : (lapsed || status == "expired") ? "expired" : "active";
            // document_hash lets anyone independently confirm a downloaded PDF is the exact one PCI issued
            // (recompute SHA-256 of the file and compare) — tamper-evidence without trusting the visual.
            var docHash = H.Str(c["pdf_sha256"]);
            c.Remove("pdf_sha256");
            var copy = new Dictionary<string, object?>(c) { ["found"] = true, ["state"] = state, ["valid"] = state == "active", ["document_hash"] = docHash, ["has_pdf"] = !string.IsNullOrEmpty(docHash) };
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
            // Security confirmation across enabled channels ("your password was changed"). Deduped per
            // token so a retried request can't double-notify. Essential/transactional — always sent.
            try
            {
                var pu = db.QueryOne("SELECT email,first_name FROM users WHERE id=?", row["user_id"]);
                Comms.Fire(db, "account.password_changed", H.Ln(row["user_id"]), H.Str(pu?["email"]), null,
                    new Dictionary<string, string?> { ["student_name"] = H.Str(pu?["first_name"]) ?? "there", ["portal_link"] = "/app/" },
                    "Your PCI password was changed",
                    "<p>Your PCI account password was just set or changed. If this wasn't you, contact support immediately — your other sessions have already been signed out.</p>",
                    dedupSuffix: $"pwset:{H.Str(row["id"])}");
            }
            catch { }
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
            // Alert the notification recipients (owner + any assignees) about the new inquiry.
            var iType = System.Net.WebUtility.HtmlEncode(H.GetS(b, "type") ?? "general");
            var iMsg = System.Net.WebUtility.HtmlEncode(H.GetS(b, "message") ?? "");
            var iName = System.Net.WebUtility.HtmlEncode(H.GetS(b, "first_name") ?? "");
            var iOrg = System.Net.WebUtility.HtmlEncode(H.GetS(b, "org") ?? "");
            Notify.Alert(db, "inquiry", $"New PCI inquiry ({iType}) — {reference}",
                $"<p>A new inquiry has been submitted.</p><p><strong>Reference:</strong> {reference}<br/>" +
                $"<strong>Type:</strong> {iType}<br/><strong>From:</strong> {iName} &lt;{System.Net.WebUtility.HtmlEncode(email)}&gt;<br/>" +
                $"<strong>Organisation:</strong> {iOrg}</p><p><strong>Message:</strong><br/>{iMsg}</p>",
                "inquiry", null);
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
            // Build a short form-type token (first 4 letters), then prefix it with PCI- so the reference
            // reads e.g. "PCI-CONT-19F7…" — previously the whole "PCI-<type>" was sliced to 4 chars,
            // which dropped the type entirely and produced "PCI--<hex>".
            var tok = System.Text.RegularExpressions.Regex.Replace(ft.ToUpperInvariant(), "[^A-Z]", "");
            tok = tok.Length == 0 ? "GEN" : tok.Length > 4 ? tok[..4] : tok;
            var reff = "PCI-" + tok + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString("X");
            try { db.Execute("INSERT INTO form_submissions(form_type,name,email,subject,message,reference) VALUES(?,?,?,?,?,?)",
                ft, H.GetS(b, "name"), H.GetS(b, "email"), H.GetS(b, "subject"), H.GetS(b, "message"), reff); } catch { }
            return J(new { ok = true, reference = reff });
        });
    }
}
