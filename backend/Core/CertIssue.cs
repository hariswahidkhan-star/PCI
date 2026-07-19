using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Turns an authoritative credential record into a verifiable PDF certificate: it renders the PDF with
/// <see cref="CertPdf"/>, stores it in private object storage, and stamps the record with the storage
/// reference, a SHA-256 tamper hash (which the public verifier can recompute), a non-guessable verify
/// token and a timestamp. Generation is idempotent and best-effort — the record is the source of truth, so
/// a render failure never blocks issuance, and a missing PDF is produced on first download.
/// </summary>
public static class CertIssue
{
    static string VerifyUrl(Db db, string credId)
    {
        // The QR must encode an ABSOLUTE URL to be scannable. Prefer the deployment's configured base
        // (APP_BASE_URL / SITE_BASE_URL, or the public_base_url setting); fall back to the canonical public
        // domain so the code always resolves even before an operator sets the base explicitly.
        var b = Mailer.BaseUrl();
        if (string.IsNullOrEmpty(b)) b = Settings.Str(db, "public_base_url", "");
        if (string.IsNullOrEmpty(b)) b = "https://projectcontrolsinstitute.org";
        return b.TrimEnd('/') + "/verify.html?id=" + credId;
    }

    /// <summary>Ensure the examination/credential PDF exists; returns (bytes, sha256, filename) or null.</summary>
    public static (byte[] bytes, string sha, string name)? EnsureCredentialPdf(Db db, string credentialId, bool force = false)
    {
        try
        {
            var c = db.QueryOne("SELECT id,credential_id,user_id,holder_name,certification_id,status,issued_at,expires_at,pdf_ref,pdf_sha256,verify_token,certificate_wording FROM issued_credentials WHERE credential_id=?", credentialId);
            if (c is null) return null;
            var name = "certificate-" + credentialId + ".pdf";
            if (!force && H.Str(c["pdf_ref"]) is { Length: > 0 } exRef && Storage.Get(exRef) is { } got && got.bytes is not null)
                return (got.bytes, H.Str(c["pdf_sha256"]) ?? "", name);

            var cert = Certs.ById(db, c["certification_id"]);
            var title = H.Str(cert?["name"]) ?? "PCI AI Project Controls Leader";
            var designation = H.Str(cert?["acronym"]) is { Length: > 0 } acr ? acr : H.Str(cert?["code"]) ?? "";
            var isTest = H.L(db.QueryOne("SELECT is_test FROM users WHERE id=?", c["user_id"])?["is_test"]) == 1;
            var token = H.Str(c["verify_token"]); if (string.IsNullOrEmpty(token)) token = Security.RandomHex(16);

            // Route-specific wording (founding/sponsored/…) was snapshotted onto the credential at
            // issuance; ordinary examination credentials use the standard sentence with the official
            // certification name and designation.
            var body = H.Str(c["certificate_wording"]) is { Length: > 0 } w ? w :
                $"has successfully fulfilled the eligibility, assessment and professional requirements for the {title} credential" +
                (designation.Length > 0 ? $" and is authorised to use the designation {designation}." : ".");
            var pdf = CertPdf.Render(new CertDoc
            {
                Title = title,
                Subtitle = designation,
                LeadIn = "This is to certify that",
                Recipient = H.Str(c["holder_name"]) ?? "",
                Body = body,
                CertificateId = credentialId,
                IssueDate = H.Str(c["issued_at"]),
                ValidUntil = H.Str(c["expires_at"]),
                VerifyUrl = VerifyUrl(db, credentialId),
                IsTest = isTest,
            });
            var stored = Storage.Put(pdf, "application/pdf", "certificates");
            db.Execute("UPDATE issued_credentials SET pdf_ref=?, pdf_sha256=?, verify_token=?, pdf_generated_at=datetime('now') WHERE credential_id=?",
                stored.Reference, stored.Sha256, token, credentialId);
            return (pdf, stored.Sha256, name);
        }
        catch { return null; }
    }

    /// <summary>Ensure the honorary certificate PDF exists. It is ALWAYS marked as an honorary certificate
    /// and never claims a passed examination.</summary>
    public static (byte[] bytes, string sha, string name)? EnsureHonoraryPdf(Db db, string awardNo, bool force = false)
    {
        try
        {
            var a = db.QueryOne("SELECT award_no,recipient_name,citation,designation,status,conferred_at,user_id,pdf_ref,pdf_sha256 FROM honorary_awards WHERE award_no=?", awardNo);
            if (a is null) return null;
            var name = "honorary-certificate-" + awardNo + ".pdf";
            if (!force && H.Str(a["pdf_ref"]) is { Length: > 0 } exRef && Storage.Get(exRef) is { } got && got.bytes is not null)
                return (got.bytes, H.Str(a["pdf_sha256"]) ?? "", name);

            var isTest = a["user_id"] is not null && H.L(db.QueryOne("SELECT is_test FROM users WHERE id=?", a["user_id"])?["is_test"]) == 1;
            var designation = H.Str(a["designation"]) ?? "Honorary Fellow (PCI)";
            var citation = H.Str(a["citation"]);
            var pdf = CertPdf.Render(new CertDoc
            {
                Honorary = true,
                Title = "Honorary Certificate",
                Subtitle = designation,
                LeadIn = "The board of Project Controls Institute Global hereby confers upon",
                Recipient = H.Str(a["recipient_name"]) ?? "",
                Body = string.IsNullOrWhiteSpace(citation)
                    ? "this honorary recognition in acknowledgement of a distinguished contribution to the project controls profession."
                    : citation,
                CertificateId = awardNo,
                IssueDate = H.Str(a["conferred_at"]),
                VerifyUrl = VerifyUrl(db, awardNo),
                IsTest = isTest,
            });
            var stored = Storage.Put(pdf, "application/pdf", "certificates");
            db.Execute("UPDATE honorary_awards SET pdf_ref=?, pdf_sha256=?, pdf_generated_at=datetime('now') WHERE award_no=?",
                stored.Reference, stored.Sha256, awardNo);
            return (pdf, stored.Sha256, name);
        }
        catch { return null; }
    }
}
