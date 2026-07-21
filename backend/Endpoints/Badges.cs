using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Native verifiable digital badges — the Open Badges 2.0 standard that Credly and other badge networks
/// implement. PCI mints and hosts its own badges, so a credential is a shareable, machine-verifiable object
/// with no vendor lock-in:
///   • Issuer profile      GET /api/badges/issuer
///   • BadgeClass (per cert) GET /api/badges/class/{certRef}
///   • Assertion (per credential, the verifiable badge) GET /api/badges/assertion/{credentialId}
///   • Badge image (SVG)   GET /api/badges/image/{certRef}
/// The Assertion is "HostedBadge"-verified: its own URL is the id, so any Open Badges validator (or a badge
/// backpack / LinkedIn / an employer) can confirm it at source. Revocation and expiry mirror /api/verify.
/// Recipient identity is a salted SHA-256 hash of the holder's email (never the raw email), so the holder can
/// prove ownership without PCI exposing personal data.
/// </summary>
public static class Badges
{
    const string Ctx = "https://w3id.org/openbadges/v2";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult J(object o) => Results.Json(o);
        static string Issuer(string b) => $"{b}/api/badges/issuer";

        // ── Issuer profile ─────────────────────────────────────────────────────────────
        app.MapGet("/api/badges/issuer", (HttpRequest req) =>
        {
            var b = Mailer.BaseUrl(req);
            return J(new Dictionary<string, object?>
            {
                ["@context"] = Ctx, ["type"] = "Issuer", ["id"] = Issuer(b),
                ["name"] = "Project Controls Institute",
                ["url"] = b,
                ["email"] = "credentials@projectcontrolsinstitute.org",
                ["image"] = $"{b}/api/badges/issuer/image",
            });
        });
        app.MapGet("/api/badges/issuer/image", (HttpRequest req) =>
            Results.Text(IssuerSvg(), "image/svg+xml"));

        // ── BadgeClass for a certification (by code or id) ──────────────────────────────
        app.MapGet("/api/badges/class/{certRef}", (HttpRequest req, string certRef) =>
        {
            var b = Mailer.BaseUrl(req);
            var certId = Certs.TryResolve(db, certRef);
            var cert = certId is null ? null : Certs.ById(db, certId);
            if (cert is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var code = H.Str(cert["code"]) ?? "PCL-AI";
            var slug = H.Str(cert["slug"]) ?? code.ToLowerInvariant();
            var name = (H.Str(cert["public_title"]) ?? H.Str(cert["name"]) ?? code);
            var desc = H.Str(cert["short_description"]) ?? H.Str(cert["description"]) ?? $"The {name} certification issued by the Project Controls Institute.";
            var tags = new List<string> { "project controls", "certification" };
            if (H.Str(cert["acronym"]) is { Length: > 0 } ac) tags.Add(ac);
            return J(new Dictionary<string, object?>
            {
                ["@context"] = Ctx, ["type"] = "BadgeClass",
                ["id"] = $"{b}/api/badges/class/{code}",
                ["name"] = name,
                ["description"] = desc,
                ["image"] = $"{b}/api/badges/image/{code}",
                ["criteria"] = new Dictionary<string, object?> { ["id"] = $"{b}/certifications/{slug}" },
                ["issuer"] = Issuer(b),
                ["tags"] = tags,
            });
        });

        // ── Badge image (branded SVG, per certification) ────────────────────────────────
        app.MapGet("/api/badges/image/{certRef}", (HttpRequest req, string certRef) =>
        {
            var certId = Certs.TryResolve(db, certRef);
            var cert = certId is null ? null : Certs.ById(db, certId);
            var acronym = H.Str(cert?["acronym"]) ?? H.Str(cert?["code"]) ?? "PCI";
            return Results.Text(BadgeSvg(acronym), "image/svg+xml");
        });

        // ── Assertion (the verifiable badge for one issued credential) ──────────────────
        app.MapGet("/api/badges/assertion/{credentialId}", (HttpRequest req, string credentialId) =>
        {
            var b = Mailer.BaseUrl(req);
            var id = (credentialId ?? "").Trim().ToUpperInvariant();
            var assertionUrl = $"{b}/api/badges/assertion/{credentialId}";
            // Exclude test-account credentials from the public badge surface (parity with /api/verify).
            var c = db.QueryOne(@"SELECT ic.credential_id,ic.user_id,ic.status,ic.issued_at,ic.expires_at,
                       COALESCE(ic.certification_id,1) certification_id, ct.code certification_code
                FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                LEFT JOIN users tu ON tu.id=ic.user_id WHERE upper(ic.credential_id)=? AND COALESCE(tu.is_test,0)=0", id);
            if (c is null) return Results.Json(new { error = "not_found" }, statusCode: 404);

            var status = H.Str(c["status"]) ?? "active";
            var expires = H.Str(c["expires_at"]);
            var code = H.Str(c["certification_code"]) ?? "PCL-AI";
            // A revoked credential still resolves (per Open Badges) but is flagged revoked.
            if (status == "revoked")
                return J(new Dictionary<string, object?>
                {
                    ["@context"] = Ctx, ["type"] = "Assertion", ["id"] = assertionUrl,
                    ["revoked"] = true, ["revocationReason"] = "This credential has been revoked by the Project Controls Institute.",
                    ["badge"] = $"{b}/api/badges/class/{code}",
                });

            // Recipient: salted SHA-256 of the holder's email; salt = credential id (public, unique) so a
            // verifier the holder shares their email with can recompute and confirm ownership.
            var email = H.Str(db.QueryOne("SELECT email FROM users WHERE id=?", c["user_id"])?["email"]) ?? "";
            var salt = H.Str(c["credential_id"]) ?? id;
            var identity = string.IsNullOrEmpty(email) ? null : "sha256$" + Security.Sha(email.Trim().ToLowerInvariant() + salt);

            var a = new Dictionary<string, object?>
            {
                ["@context"] = Ctx, ["type"] = "Assertion", ["id"] = assertionUrl,
                ["recipient"] = new Dictionary<string, object?> { ["type"] = "email", ["hashed"] = true, ["salt"] = salt, ["identity"] = identity },
                ["badge"] = $"{b}/api/badges/class/{code}",
                ["verification"] = new Dictionary<string, object?> { ["type"] = "HostedBadge" },
                ["issuedOn"] = Iso(H.Str(c["issued_at"])),
                ["evidence"] = $"{b}/verify.html?id={Uri.EscapeDataString(H.Str(c["credential_id"]) ?? id)}",
            };
            if (!string.IsNullOrEmpty(expires)) a["expires"] = Iso(expires);
            return J(a);
        });

        // ── Public JSON summary for the badge page (image + share + status), by credential id ──
        app.MapGet("/api/badges/view/{credentialId}", (HttpRequest req, string credentialId) =>
        {
            var b = Mailer.BaseUrl(req);
            var id = (credentialId ?? "").Trim().ToUpperInvariant();
            var c = db.QueryOne(@"SELECT ic.credential_id,ic.holder_name,ic.status,ic.issued_at,ic.expires_at,
                       ct.code certification_code, ct.name certification_name, ct.acronym certification_acronym
                FROM issued_credentials ic LEFT JOIN certifications ct ON ct.id=COALESCE(ic.certification_id,1)
                LEFT JOIN users tu ON tu.id=ic.user_id WHERE upper(ic.credential_id)=? AND COALESCE(tu.is_test,0)=0", id);
            if (c is null) return J(new { found = false });
            var status = H.Str(c["status"]) ?? "active";
            var lapsed = status == "active" && H.IsPast(H.Str(c["expires_at"]));
            var state = status == "revoked" ? "revoked" : (lapsed || status == "expired") ? "expired" : "active";
            var code = H.Str(c["certification_code"]) ?? "PCL-AI";
            return J(new
            {
                found = true, state, valid = state == "active",
                credential_id = H.Str(c["credential_id"]),
                holder_name = H.Str(c["holder_name"]),
                certification_name = H.Str(c["certification_name"]),
                certification_acronym = H.Str(c["certification_acronym"]),
                issued_at = H.Str(c["issued_at"]), expires_at = H.Str(c["expires_at"]),
                image = $"{b}/api/badges/image/{code}",
                assertion_url = $"{b}/api/badges/assertion/{H.Str(c["credential_id"])}",
                verify_url = $"{b}/verify.html?id={Uri.EscapeDataString(H.Str(c["credential_id"]) ?? id)}",
            });
        });
    }

    static string Iso(string? sqlDate)
    {
        if (string.IsNullOrWhiteSpace(sqlDate)) return "";
        return DateTimeOffset.TryParse(sqlDate, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal, out var dto)
            ? dto.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") : sqlDate!;
    }

    static string Esc(string s) => System.Net.WebUtility.HtmlEncode(s);

    // A branded circular badge as inline SVG — no image pipeline, scales cleanly, Open Badges accepts SVG.
    static string BadgeSvg(string acronym)
    {
        var a = Esc(acronym.Length > 10 ? acronym[..10] : acronym);
        return $@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 240 240"" width=""240"" height=""240"" role=""img"" aria-label=""{a} digital badge"">
  <defs><linearGradient id=""g"" x1=""0"" y1=""0"" x2=""0"" y2=""1""><stop offset=""0"" stop-color=""#1D4ED8""/><stop offset=""1"" stop-color=""#1e3a8a""/></linearGradient></defs>
  <circle cx=""120"" cy=""120"" r=""112"" fill=""url(#g)"" stroke=""#0f2260"" stroke-width=""4""/>
  <circle cx=""120"" cy=""120"" r=""92"" fill=""none"" stroke=""#93c5fd"" stroke-width=""2"" opacity=""0.6""/>
  <text x=""120"" y=""70"" text-anchor=""middle"" fill=""#bfdbfe"" font-family=""Segoe UI,Arial,sans-serif"" font-size=""15"" letter-spacing=""1"">PROJECT CONTROLS</text>
  <text x=""120"" y=""90"" text-anchor=""middle"" fill=""#bfdbfe"" font-family=""Segoe UI,Arial,sans-serif"" font-size=""15"" letter-spacing=""1"">INSTITUTE</text>
  <text x=""120"" y=""148"" text-anchor=""middle"" fill=""#ffffff"" font-family=""Segoe UI,Arial,sans-serif"" font-size=""36"" font-weight=""700"">{a}</text>
  <text x=""120"" y=""182"" text-anchor=""middle"" fill=""#dbeafe"" font-family=""Segoe UI,Arial,sans-serif"" font-size=""13"" letter-spacing=""2"">CERTIFIED</text>
</svg>";
    }

    static string IssuerSvg() => BadgeSvg("PCI");
}
