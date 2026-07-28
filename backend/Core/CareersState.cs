using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World Careers — the pure decision layer (docs/pciworld/CCP_PHASE4_DESIGN.md §2, §4, §5.2,
/// §6, §7).
///
/// PURE. No database, no clock, no I/O — every input arrives as an argument, so the rules that §2
/// says "must never happen" are unit-testable without standing anything up, and two call sites
/// cannot disagree about whether an employer may publish or whether a posting is honestly live.
///
/// THE TWO RULES THIS FILE EXISTS TO MAKE UNARGUABLE:
///
///   1. VERIFY BEFORE PUBLISH (§4.3). A fake employer is not a spammer; it is a data-harvesting
///      attack, and the window between publish and takedown is exactly the window in which CVs
///      arrive. So "may this employer make anything public" has one answer, computed here, and the
///      endpoints re-ask it at the transition AND at every read.
///
///   2. THE HONESTY RULE (§7). Structured data is a claim to a search engine, and a stale claim is
///      a lie that outlives the page. <see cref="IsLive"/> is the single predicate for markup,
///      sitemap membership and apply; <see cref="JobJsonLd"/> returns NULL — not a "closed" blob —
///      the moment it is false, computed from live state at render time rather than from a cache a
///      background sweep may not have refreshed.
///
/// Deliberately absent: any ranking, scoring or ordering of candidates. §10.7 prohibits automated
/// candidate assessment in Release 1, and the control is that there is NO code path — not a
/// disabled one. Do not add one here, even as a helper.
/// </summary>
public static class CareersState
{
    // State vocabularies come from Data/CareersSchema.cs and are matched EXACTLY (ordinal,
    // lowercase). Anything else — unknown value, different casing, whitespace — fails closed:
    // a row whose state column has been mangled must lose capabilities, never gain them.
    //
    //   employers:     draft | pending_verification | verified | suspended
    //   postings:      draft | published | closed | withdrawn
    //   applications:  draft | submitted | withdrawn | shortlisted | rejected

    /// <summary>
    /// May this employer make anything public — publish a posting, appear in the sitemap, emit
    /// Organization markup, read applicant data? Exactly one state qualifies. `verified` is an
    /// assertion PCI makes to candidates about who is behind a posting (§4.2); `suspended` is
    /// deliberately NOT grandfathered: a fake employer discovered late loses the data access,
    /// not just the listing, and this predicate being re-checked per request is what makes that
    /// cut mid-session (§4.3).
    /// </summary>
    public static bool EmployerMayPublish(string employerState) =>
        string.Equals(employerState, "verified", StringComparison.Ordinal);

    /// <summary>
    /// The employer state machine. Refusal is the default; only the transitions the design names
    /// exist, and a same-state "transition" is refused because it is not one.
    ///
    /// draft → verified is refused ON PURPOSE: verification is an attributable act performed on a
    /// submission (§4.2) — an employer nobody presented for review cannot have been reviewed, and
    /// a shortcut here would let one UPDATE bypass the entire §4.3 gate. Suspension applies only
    /// to `verified` (pre-verification there is nothing public to take down, and nothing private
    /// beyond the employer's own drafts); re-verification is suspended → verified, audited by the
    /// caller like every other state change.
    /// </summary>
    public static bool EmployerTransitionAllowed(string from, string to) => (from, to) switch
    {
        ("draft", "pending_verification") => true,          // submit for review
        ("pending_verification", "draft") => true,          // returned for more evidence
        ("pending_verification", "verified") => true,       // the attributable act (§4.2)
        ("verified", "suspended") => true,                  // takedown — unpublishes everything at the read (§4.3)
        ("suspended", "verified") => true,                  // re-verification, audited
        _ => false,
    };

    /// <summary>
    /// The posting state machine. draft → published is necessary but NOT sufficient to publish:
    /// the publish endpoint must also hold <see cref="EmployerMayPublish"/> inside the same
    /// transaction (§4.3 control 1) — this function knows nothing about the employer on purpose,
    /// so it can never be mistaken for the whole gate.
    ///
    /// closed → published exists because reopening a role (a deadline extended, a hire fallen
    /// through) is a legitimate act that goes back through the publish gate. withdrawn is
    /// TERMINAL: a takedown that could be quietly reversed is not a takedown — a withdrawn role
    /// returns as a new posting with a fresh audit trail, never by resurrecting the old row.
    /// A never-published draft has nothing to take down, so draft → withdrawn is refused too.
    /// </summary>
    public static bool PostingTransitionAllowed(string from, string to) => (from, to) switch
    {
        ("draft", "published") => true,      // + EmployerMayPublish, checked by the endpoint in-transaction
        ("published", "closed") => true,     // no longer accepting; the page stays reachable (§7)
        ("published", "withdrawn") => true,  // takedown
        ("closed", "published") => true,     // reopen — re-gated on the employer at the endpoint
        ("closed", "withdrawn") => true,     // takedown of an already-closed posting
        _ => false,
    };

    /// <summary>
    /// The application state machine. draft → submitted is the freeze moment (§5.2): answers, CV
    /// reference and consent snapshot in that transaction, and NOTHING ever transitions back to
    /// `draft` — a path back to draft would be in-place mutability of a submitted application
    /// through the back door, which §28 forbids.
    ///
    /// withdrawn → submitted is §5.2's reinstatement: the unique index means a withdrawn
    /// application still holds the slot, so reapplying reopens the row the person already owns
    /// (with a fresh freeze), never a second row. `rejected` is terminal here — un-rejecting is
    /// not a transition the design names, and refusing it costs nothing until a real need is
    /// documented (§28: never widen speculatively).
    /// </summary>
    public static bool ApplicationTransitionAllowed(string from, string to) => (from, to) switch
    {
        ("draft", "submitted") => true,          // the freeze (§5.2)
        ("submitted", "withdrawn") => true,      // the applicant's act, any time after submission
        ("submitted", "shortlisted") => true,    // employer decision — recorded, never silent
        ("submitted", "rejected") => true,
        ("shortlisted", "rejected") => true,     // a later rejection
        ("shortlisted", "withdrawn") => true,    // the applicant may still walk away
        ("withdrawn", "submitted") => true,      // reinstatement — same row, fresh freeze (§5.2)
        _ => false,
    };

    /// <summary>
    /// THE HONESTY PREDICATE (§7). True only while the posting is `published`, its employer is
    /// `verified`, and `closes_at` — when set — has not passed. One function for markup, sitemap
    /// and apply, so the three surfaces cannot drift: the page saying "closed" is courtesy, this
    /// returning false is the control.
    ///
    /// Datetimes are the platform's "YYYY-MM-DD HH:MM:SS" UTC strings compared BY INSTANT via
    /// <see cref="H.After"/>, never lexically — `nowUtc` may arrive ISO-formatted with a 'T', and
    /// ' ' (0x20) &lt; 'T' (0x54) makes a lexical compare close every job for the whole of its
    /// final day. A non-blank `closes_at` that does not parse fails CLOSED: an unreadable deadline
    /// must not advertise a job forever.
    /// </summary>
    public static bool IsLive(string postingState, string employerState, string? closesAtUtc, string nowUtc)
    {
        if (!string.Equals(postingState, "published", StringComparison.Ordinal)) return false;
        if (!EmployerMayPublish(employerState)) return false;
        if (string.IsNullOrWhiteSpace(closesAtUtc)) return true;   // open-ended
        if (!DateTimeOffset.TryParse(closesAtUtc, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal,
                out _))
            return false;                                          // unparsable deadline → fail closed
        // Past means strictly past: at the exact closing instant the job is still (just) live.
        return !H.After(nowUtc, closesAtUtc);
    }

    /// <summary>
    /// Everything the JSON-LD builder needs, carried as plain values so it needs no database — the
    /// endpoint reads the posting + employer join once and copies the columns in. Employer fields
    /// come from the employer ROW (name, recorded website), never from free text on the posting:
    /// `hiringOrganization` naming anyone other than the verified tenant would be PCI attesting to
    /// an identity nobody verified (§7).
    /// </summary>
    /// <param name="Id">Posting id — the JSON-LD `identifier` value.</param>
    /// <param name="Title">Posting title (employer-authored text; encoded on output).</param>
    /// <param name="Description">Posting description; falls back to the title when blank.</param>
    /// <param name="PostingState">draft|published|closed|withdrawn.</param>
    /// <param name="EmployerState">draft|pending_verification|verified|suspended.</param>
    /// <param name="EmployerName">The verified employer's recorded name.</param>
    /// <param name="EmployerUrl">The employer's World profile page — `hiringOrganization.url`.</param>
    /// <param name="EmployerWebsite">The employer's recorded website — `sameAs`, never free text.</param>
    /// <param name="PublishedAtUtc">"YYYY-MM-DD HH:MM:SS" UTC → `datePosted` (date part).</param>
    /// <param name="ClosesAtUtc">"YYYY-MM-DD HH:MM:SS" UTC or null → `validThrough` and liveness.</param>
    /// <param name="Location">Free-text locality; omitted from markup when blank.</param>
    /// <param name="EmploymentType">Schema vocab full_time|part_time|contract|internship.</param>
    /// <param name="SalaryMinMinor">Minor units (§5.2 — INTEGER, never floating point).</param>
    /// <param name="SalaryMaxMinor">Minor units.</param>
    /// <param name="Currency">ISO code travelling with the amounts.</param>
    public sealed record JobLd(
        long Id,
        string Title,
        string? Description,
        string PostingState,
        string EmployerState,
        string EmployerName,
        string? EmployerUrl,
        string? EmployerWebsite,
        string? PublishedAtUtc,
        string? ClosesAtUtc,
        string? Location,
        string? EmploymentType,
        long? SalaryMinMinor,
        long? SalaryMaxMinor,
        string? Currency);

    // The DEFAULT encoder, deliberately — not UnsafeRelaxedJsonEscaping. This JSON is embedded
    // inside <script type="application/ld+json">, and titles/descriptions are employer-authored.
    // The relaxed encoder leaves '<' and '>' literal, so a title containing </script> closes the
    // tag early and everything after it becomes live markup — stored XSS on a crawlable page. The
    // default encoder emits <, inert inside the script block and still valid JSON-LD to every
    // crawler. This exact bug was found and fixed in ForumRender earlier in this increment.
    static readonly JsonSerializerOptions JsonOpts = new();

    // Local employment_type vocabulary → schema.org enumeration. Anything else is OMITTED rather
    // than guessed: an employmentType the data does not carry is a claim the page cannot back.
    static readonly Dictionary<string, string> EmploymentTypes = new(StringComparer.Ordinal)
    {
        ["full_time"] = "FULL_TIME",
        ["part_time"] = "PART_TIME",
        ["contract"] = "CONTRACTOR",
        ["internship"] = "INTERN",
    };

    /// <summary>
    /// The schema.org JobPosting document for a live posting — or NULL when <see cref="IsLive"/>
    /// says no. Returning null rather than a "closed" blob is the point of §7: a closed page still
    /// renders for the human holding a saved link, but it makes no machine-readable claim at all,
    /// because a stale claim outlives the page in caches nobody at PCI controls.
    ///
    /// Only what the data carries is claimed: `validThrough` only when `closes_at` is set,
    /// `baseSalary` only when the employer recorded an amount AND its currency (an amount without
    /// its currency is not an amount — omitting beats defaulting to USD, which would fabricate an
    /// offer), no salary period `unitText` because the Phase 4 schema stores none, and
    /// `directApply: true` always, because §7 forbids external apply URLs.
    /// </summary>
    public static string? JobJsonLd(JobLd job, string nowUtc)
    {
        if (!IsLive(job.PostingState, job.EmployerState, job.ClosesAtUtc, nowUtc)) return null;

        var org = new Dictionary<string, object?>
        {
            ["@type"] = "Organization",
            ["name"] = job.EmployerName,
        };
        if (!string.IsNullOrWhiteSpace(job.EmployerUrl)) org["url"] = job.EmployerUrl;
        // sameAs is the RECORDED employer website — the row a human verified, never posting text.
        if (!string.IsNullOrWhiteSpace(job.EmployerWebsite)) org["sameAs"] = job.EmployerWebsite;

        var o = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org/",
            ["@type"] = "JobPosting",
            ["title"] = job.Title,
            // description is HTML per Google's JobPosting guidance; author text is encoded so it
            // stays text (the JSON encoder additionally escapes it for the script block).
            ["description"] = string.IsNullOrWhiteSpace(job.Description)
                ? Esc(job.Title) : "<p>" + Esc(job.Description) + "</p>",
            ["identifier"] = new Dictionary<string, object?>
            {
                ["@type"] = "PropertyValue",
                ["name"] = job.EmployerName,
                ["value"] = job.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            },
            ["hiringOrganization"] = org,
            // In-platform apply only (§7) — an external apply URL would defeat the consent,
            // snapshot and audit model and is the classic job-board phishing vector.
            ["directApply"] = true,
        };

        if (!string.IsNullOrWhiteSpace(job.PublishedAtUtc))
            o["datePosted"] = job.PublishedAtUtc[..Math.Min(10, job.PublishedAtUtc.Length)];
        if (!string.IsNullOrWhiteSpace(job.ClosesAtUtc))
            o["validThrough"] = job.ClosesAtUtc;
        if (EmploymentTypes.TryGetValue(job.EmploymentType ?? "", out var et))
            o["employmentType"] = et;

        if (!string.IsNullOrWhiteSpace(job.Location))
            o["jobLocation"] = new Dictionary<string, object?>
            {
                ["@type"] = "Place",
                ["address"] = new Dictionary<string, object?>
                {
                    ["@type"] = "PostalAddress",
                    ["addressLocality"] = job.Location,
                },
            };

        // baseSalary only when the employer recorded an amount AND its currency. Minor units →
        // major via Money.ToDecimal — the platform's one conversion, exact, no float involved.
        if ((job.SalaryMinMinor.HasValue || job.SalaryMaxMinor.HasValue)
            && !string.IsNullOrWhiteSpace(job.Currency))
        {
            var val = new Dictionary<string, object?> { ["@type"] = "QuantitativeValue" };
            if (job.SalaryMinMinor is { } mn) val["minValue"] = Money.ToDecimal(mn);
            if (job.SalaryMaxMinor is { } mx) val["maxValue"] = Money.ToDecimal(mx);
            if (job.SalaryMinMinor is { } only && !job.SalaryMaxMinor.HasValue)
                val["value"] = Money.ToDecimal(only);
            o["baseSalary"] = new Dictionary<string, object?>
            {
                ["@type"] = "MonetaryAmount",
                ["currency"] = job.Currency,
                ["value"] = val,
            };
        }

        return JsonSerializer.Serialize(o, JsonOpts);
    }

    /// <summary>CV cap, inside the platform's 6 MB Kestrel request-body limit so a maximal upload
    /// is rejected by this layer's rules rather than by a connection reset the applicant cannot
    /// interpret.</summary>
    public const int MaxCvBytes = 6_000_000;

    /// <summary>
    /// The CV intake allowlist (§6): PDF, DOC, DOCX ONLY — no images (a marketplace reviewing at
    /// volume should not normalise photographed documents), no HTML, no SVG, no plain archives.
    /// Returns the canonical MIME on success, null on rejection.
    ///
    /// The verdict comes from the BYTES, never from the filename or a client-supplied content
    /// type — a renamed .exe fails because it does not carry a document signature, whatever it is
    /// called. Beyond the magic number, container formats are checked structurally, because the
    /// magic alone is ambiguous: every OOXML file and every zip starts with PK, so DOCX must
    /// additionally contain the OOXML content-types entry and the word/ document part (which is
    /// what rejects plain archives and spreadsheets); every legacy Office binary and MSI starts
    /// with the OLE header, so DOC must additionally contain the WordDocument stream name.
    ///
    /// The filename can only VETO, never grant: when it carries an extension that contradicts the
    /// sniffed type the upload is rejected as suspicious (a "cv.exe" that is genuinely a PDF is
    /// somebody probing), but no extension — "resume" — is fine, because the content decides.
    /// </summary>
    public static string? SniffCvMime(byte[] bytes, string fileName)
    {
        if (bytes is null || bytes.Length < 8) return null;   // no real document is 7 bytes

        var mime = SniffContent(bytes);
        if (mime is null) return null;

        var ext = ExtensionOf(fileName);
        if (ext is not null && !string.Equals(ext, CanonicalExt(mime), StringComparison.Ordinal))
            return null;   // extension contradicts content — veto, never re-interpret

        return mime;
    }

    const string MimePdf = "application/pdf";
    const string MimeDoc = "application/msword";
    const string MimeDocx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    static string CanonicalExt(string mime) => mime switch
    {
        MimePdf => ".pdf",
        MimeDoc => ".doc",
        _ => ".docx",
    };

    static string? SniffContent(byte[] b)
    {
        // %PDF
        if (b[0] == 0x25 && b[1] == 0x50 && b[2] == 0x44 && b[3] == 0x46) return MimePdf;

        // OLE compound file (legacy Office, but also XLS/PPT/MSI — hence the stream-name check).
        if (b[0] == 0xD0 && b[1] == 0xCF && b[2] == 0x11 && b[3] == 0xE0
            && b[4] == 0xA1 && b[5] == 0xB1 && b[6] == 0x1A && b[7] == 0xE1)
        {
            // Directory entry names are UTF-16LE; every real .doc carries a WordDocument stream.
            // An OLE file without one (a spreadsheet, an installer) is not a Word document.
            return Contains(b, System.Text.Encoding.Unicode.GetBytes("WordDocument")) ? MimeDoc : null;
        }

        // ZIP container (OOXML, but also every ordinary archive — hence the part-name checks).
        if (b[0] == 0x50 && b[1] == 0x4B && (b[2] == 0x03 || b[2] == 0x05 || b[2] == 0x07))
        {
            // A real DOCX carries the OOXML content-types entry AND the Word main part. A plain
            // archive has neither; an XLSX/PPTX has the first but not the second. §6 says no
            // archives, so PK alone is a rejection, not a pass.
            var isDocx = Contains(b, System.Text.Encoding.ASCII.GetBytes("[Content_Types].xml"))
                      && Contains(b, System.Text.Encoding.ASCII.GetBytes("word/document.xml"));
            return isDocx ? MimeDocx : null;
        }

        return null;   // everything else — MZ executables, images, HTML, SVG, text — is refused
    }

    static bool Contains(byte[] haystack, byte[] needle) =>
        haystack.AsSpan().IndexOf(needle) >= 0;

    /// <summary>The lowercased extension (with dot) of the basename, or null when there is none.
    /// Path components are stripped first so "a/b.pdf" and "..\\b.pdf" read the same.</summary>
    static string? ExtensionOf(string? fileName)
    {
        var name = (fileName ?? "").Replace('\\', '/');
        var slash = name.LastIndexOf('/');
        if (slash >= 0) name = name[(slash + 1)..];
        var dot = name.LastIndexOf('.');
        if (dot <= 0 || dot == name.Length - 1) return null;   // no extension claimed
        return name[dot..].ToLowerInvariant();
    }

    static string Esc(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");
}
