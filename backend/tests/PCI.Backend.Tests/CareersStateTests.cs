using System.IO.Compression;
using System.Text;
using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The pure careers decision layer (Core/CareersState.cs; docs/pciworld/CCP_PHASE4_DESIGN.md §2,
/// §4, §5.2, §6, §7). No database — these are the rules §2 says must never be broken, asserted
/// directly against the functions every endpoint will call.
///
/// The state-machine tests are EXHAUSTIVE on purpose: they enumerate every (from, to) pair and
/// compare against the full designed allow-set, so ADDING a transition fails a test just as
/// surely as removing one. A test that only checks the happy path would pass against a machine
/// that also allows draft → verified — which is the §4.3 bypass this phase exists to prevent.
/// </summary>
public class CareersStateTests
{
    static readonly string[] EmployerStates = { "draft", "pending_verification", "verified", "suspended" };
    static readonly string[] PostingStates = { "draft", "published", "closed", "withdrawn" };
    static readonly string[] ApplicationStates = { "draft", "submitted", "withdrawn", "shortlisted", "rejected" };

    /// <summary>A fixed "now" so every liveness assertion is deterministic.</summary>
    const string Now = "2026-07-28 12:00:00";

    // ── employer gate and machine (§4) ────────────────────────────────────────────────────────

    [Theory]
    [InlineData("verified", true)]
    [InlineData("draft", false)]
    [InlineData("pending_verification", false)]
    [InlineData("suspended", false)]        // suspension cuts the public surface AND data access
    [InlineData("VERIFIED", false)]          // stored vocab is exact lowercase; a mangled row fails closed
    [InlineData(" verified", false)]
    [InlineData("", false)]
    [InlineData("banana", false)]
    public void OnlyAVerifiedEmployerMayPublish(string state, bool expected) =>
        Assert.Equal(expected, CareersState.EmployerMayPublish(state));

    [Fact]
    public void EmployerMachineAllowsExactlyTheDesignedTransitions()
    {
        var allowed = new HashSet<(string, string)>
        {
            ("draft", "pending_verification"),
            ("pending_verification", "draft"),
            ("pending_verification", "verified"),
            ("verified", "suspended"),
            ("suspended", "verified"),
        };
        foreach (var from in EmployerStates.Append("banana"))
            foreach (var to in EmployerStates.Append("banana"))
                Assert.Equal(allowed.Contains((from, to)), CareersState.EmployerTransitionAllowed(from, to));
    }

    [Fact]
    public void AnEmployerCannotSkipTheVerificationQueue()
    {
        // Verification is an attributable act on a SUBMISSION (§4.2). draft → verified in one hop
        // would let a single UPDATE bypass the whole §4.3 gate — the data-harvesting scenario.
        Assert.False(CareersState.EmployerTransitionAllowed("draft", "verified"));
        Assert.False(CareersState.EmployerTransitionAllowed("draft", "suspended"));
        Assert.False(CareersState.EmployerTransitionAllowed("suspended", "draft"));
    }

    // ── posting machine (§5.2, §7) ────────────────────────────────────────────────────────────

    [Fact]
    public void PostingMachineAllowsExactlyTheDesignedTransitions()
    {
        var allowed = new HashSet<(string, string)>
        {
            ("draft", "published"),        // + EmployerMayPublish at the endpoint, in-transaction
            ("published", "closed"),
            ("published", "withdrawn"),
            ("closed", "published"),       // reopening goes back through the publish gate
            ("closed", "withdrawn"),
        };
        foreach (var from in PostingStates.Append("banana"))
            foreach (var to in PostingStates.Append("banana"))
                Assert.Equal(allowed.Contains((from, to)), CareersState.PostingTransitionAllowed(from, to));
    }

    [Fact]
    public void AWithdrawnPostingIsTerminal()
    {
        // A takedown that could be quietly reversed is not a takedown. A withdrawn role returns
        // as a NEW posting with a fresh audit trail, never by resurrecting the old row.
        foreach (var to in PostingStates)
            Assert.False(CareersState.PostingTransitionAllowed("withdrawn", to));
    }

    // ── application machine (§5.2) ────────────────────────────────────────────────────────────

    [Fact]
    public void ApplicationMachineAllowsExactlyTheDesignedTransitions()
    {
        var allowed = new HashSet<(string, string)>
        {
            ("draft", "submitted"),            // the freeze moment
            ("submitted", "withdrawn"),
            ("submitted", "shortlisted"),
            ("submitted", "rejected"),
            ("shortlisted", "rejected"),
            ("shortlisted", "withdrawn"),
            ("withdrawn", "submitted"),        // reinstatement — same row, fresh freeze
        };
        foreach (var from in ApplicationStates.Append("banana"))
            foreach (var to in ApplicationStates.Append("banana"))
                Assert.Equal(allowed.Contains((from, to)), CareersState.ApplicationTransitionAllowed(from, to));
    }

    [Fact]
    public void NothingEverTransitionsBackToDraft()
    {
        // A path back to draft would be in-place mutability of a submitted application through
        // the back door — the thing §28 forbids and the snapshot design exists to prevent.
        foreach (var from in ApplicationStates)
            Assert.False(CareersState.ApplicationTransitionAllowed(from, "draft"));
    }

    // ── the honesty predicate (§7) ────────────────────────────────────────────────────────────

    [Theory]
    // published + verified: closes_at decides
    [InlineData("published", "verified", null, true)]
    [InlineData("published", "verified", "", true)]
    [InlineData("published", "verified", "  ", true)]
    [InlineData("published", "verified", "2026-07-29 12:00:00", true)]    // future
    [InlineData("published", "verified", "2026-07-28 12:00:00", true)]    // the closing instant — not yet past
    [InlineData("published", "verified", "2026-07-28 11:59:59", false)]   // one second past
    [InlineData("published", "verified", "2026-07-27 12:00:00", false)]   // clearly past
    [InlineData("published", "verified", "not a datetime", false)]        // unreadable deadline fails closed
    // every non-published posting state is dead regardless of everything else
    [InlineData("draft", "verified", null, false)]
    [InlineData("closed", "verified", null, false)]
    [InlineData("withdrawn", "verified", null, false)]
    // every non-verified employer state kills a published posting — suspension unpublishes (§4.3)
    [InlineData("published", "draft", null, false)]
    [InlineData("published", "pending_verification", null, false)]
    [InlineData("published", "suspended", null, false)]
    [InlineData("published", "suspended", "2026-07-29 12:00:00", false)]
    public void IsLiveRowByRow(string posting, string employer, string? closesAt, bool expected) =>
        Assert.Equal(expected, CareersState.IsLive(posting, employer, closesAt, Now));

    [Fact]
    public void ClosingLaterTodayIsComparedAsAnInstantNotAsAString()
    {
        // 'now' can arrive ISO-formatted with a 'T' at index 10 while closes_at uses a space, and
        // ' ' (0x20) < 'T' (0x54). A lexical compare would rank this job closed for the whole of
        // its final day — the exact off-by-a-day bug H.After exists to prevent.
        Assert.True(CareersState.IsLive("published", "verified",
            closesAtUtc: "2026-07-28 17:00:00", nowUtc: "2026-07-28T09:00:00.000Z"));
    }

    // ── the JSON-LD builder (§7) ──────────────────────────────────────────────────────────────

    static CareersState.JobLd Job(
        string postingState = "published", string employerState = "verified",
        string? closesAt = null, long? min = null, long? max = null, string? currency = null,
        string title = "Senior Planner", string? description = "Own the schedule risk register.",
        string? location = "London", string? employmentType = "full_time",
        string? website = "https://acme-controls.example", string? empUrl = "/world/employers/acme-controls")
        => new(
            Id: 42, Title: title, Description: description,
            PostingState: postingState, EmployerState: employerState,
            EmployerName: "Acme Controls", EmployerUrl: empUrl, EmployerWebsite: website,
            PublishedAtUtc: "2026-07-01 09:00:00", ClosesAtUtc: closesAt,
            Location: location, EmploymentType: employmentType,
            SalaryMinMinor: min, SalaryMaxMinor: max, Currency: currency);

    static JsonElement Parse(string? json)
    {
        Assert.NotNull(json);
        return JsonDocument.Parse(json!).RootElement.Clone();
    }

    [Fact]
    public void ALiveJobEmitsTheCoreClaims()
    {
        var el = Parse(CareersState.JobJsonLd(Job(), Now));

        Assert.Equal("JobPosting", el.GetProperty("@type").GetString());
        Assert.Equal("Senior Planner", el.GetProperty("title").GetString());
        Assert.Equal("42", el.GetProperty("identifier").GetProperty("value").GetString());
        Assert.Equal("2026-07-01", el.GetProperty("datePosted").GetString());
        Assert.Equal("FULL_TIME", el.GetProperty("employmentType").GetString());
        Assert.True(el.GetProperty("directApply").GetBoolean());   // §7: in-platform apply only

        var org = el.GetProperty("hiringOrganization");
        Assert.Equal("Organization", org.GetProperty("@type").GetString());
        Assert.Equal("Acme Controls", org.GetProperty("name").GetString());
        Assert.Equal("/world/employers/acme-controls", org.GetProperty("url").GetString());
        // sameAs is the employer row's RECORDED website, never free text off the posting
        Assert.Equal("https://acme-controls.example", org.GetProperty("sameAs").GetString());

        Assert.Equal("London", el.GetProperty("jobLocation").GetProperty("address")
            .GetProperty("addressLocality").GetString());
    }

    [Theory]
    [InlineData("draft", "verified")]
    [InlineData("closed", "verified")]       // a closed page renders for humans; it claims NOTHING to machines
    [InlineData("withdrawn", "verified")]
    [InlineData("published", "draft")]
    [InlineData("published", "pending_verification")]
    [InlineData("published", "suspended")]   // suspension silences markup in the same instant (§4.3)
    public void AnythingNotLiveEmitsNullNotAClosedBlob(string posting, string employer) =>
        Assert.Null(CareersState.JobJsonLd(Job(postingState: posting, employerState: employer), Now));

    [Fact]
    public void APostingPastItsCloseEmitsNothingEvenWhileStillMarkedPublished()
    {
        // Honesty is computed at render time from closes_at — it cannot depend on a background
        // sweep having stamped state='closed' yet.
        Assert.Null(CareersState.JobJsonLd(Job(closesAt: "2026-07-27 09:00:00"), Now));
    }

    [Fact]
    public void ValidThroughAppearsExactlyWhenClosesAtIsSet()
    {
        var open = Parse(CareersState.JobJsonLd(Job(), Now));
        Assert.False(open.TryGetProperty("validThrough", out _));   // no deadline → no claim

        var closing = Parse(CareersState.JobJsonLd(Job(closesAt: "2026-08-31 23:59:59"), Now));
        Assert.Equal("2026-08-31 23:59:59", closing.GetProperty("validThrough").GetString());
    }

    [Fact]
    public void SalaryIsOmittedUnlessTheEmployerRecordedIt()
    {
        var none = Parse(CareersState.JobJsonLd(Job(), Now));
        Assert.False(none.TryGetProperty("baseSalary", out _));

        // An amount without its currency is not an amount — omit, never default to USD.
        var noCurrency = Parse(CareersState.JobJsonLd(Job(min: 4_500_000), Now));
        Assert.False(noCurrency.TryGetProperty("baseSalary", out _));
    }

    [Fact]
    public void SalaryTravelsAsMajorUnitsWithItsCurrency()
    {
        var el = Parse(CareersState.JobJsonLd(Job(min: 4_500_000, max: 6_000_000, currency: "GBP"), Now));
        var sal = el.GetProperty("baseSalary");
        Assert.Equal("GBP", sal.GetProperty("currency").GetString());
        Assert.Equal(45000m, sal.GetProperty("value").GetProperty("minValue").GetDecimal());
        Assert.Equal(60000m, sal.GetProperty("value").GetProperty("maxValue").GetDecimal());
    }

    [Fact]
    public void AnUnknownEmploymentTypeIsOmittedNotGuessed()
    {
        var el = Parse(CareersState.JobJsonLd(Job(employmentType: "gig"), Now));
        Assert.False(el.TryGetProperty("employmentType", out _));
    }

    [Fact]
    public void ATitleContainingScriptCloseCannotBreakOutOfTheLdBlock()
    {
        // This JSON is emitted inside <script type="application/ld+json">. With the relaxed
        // encoder, a title of exactly this shape closes the tag early and the payload becomes
        // live markup — the stored-XSS bug found and fixed in ForumRender this increment. The
        // default encoder must leave no literal '<' anywhere in the serialised output.
        var json = CareersState.JobJsonLd(
            Job(title: "Great role</script><script>alert(1)</script>",
                description: "<img src=x onerror=alert(1)>"), Now);

        Assert.NotNull(json);
        Assert.DoesNotContain("</script>", json!, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<", json!, StringComparison.Ordinal);
        Assert.Contains("\\u003C", json!, StringComparison.OrdinalIgnoreCase);  // escaped, not stripped

        // and it is still valid JSON carrying the title intact for a crawler
        Assert.Contains("</script>", Parse(json).GetProperty("title").GetString()!, StringComparison.Ordinal);
    }

    // ── CV intake sniffing (§6) ───────────────────────────────────────────────────────────────

    const string Pdf = "application/pdf";
    const string Doc = "application/msword";
    const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    static byte[] PdfBytes() => Encoding.ASCII.GetBytes("%PDF-1.7\n1 0 obj\n<< /Type /Catalog >>\nendobj\ntrailer\n%%EOF");

    static byte[] ExeBytes()
    {
        var b = new byte[256];               // MZ header — a Windows executable, whatever its name
        b[0] = 0x4D; b[1] = 0x5A; b[2] = 0x90;
        return b;
    }

    static byte[] OleBytes(bool withWordStream)
    {
        var b = new byte[1024];
        new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }.CopyTo(b, 0);
        if (withWordStream)
            Encoding.Unicode.GetBytes("WordDocument").CopyTo(b, 512);   // directory entry name, UTF-16LE
        return b;
    }

    static byte[] ZipBytes(params (string name, string content)[] entries)
    {
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
            foreach (var (name, content) in entries)
            {
                using var s = zip.CreateEntry(name).Open();
                var bytes = Encoding.UTF8.GetBytes(content);
                s.Write(bytes, 0, bytes.Length);
            }
        return ms.ToArray();
    }

    static byte[] DocxBytes() => ZipBytes(
        ("[Content_Types].xml", "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\"/>"),
        ("word/document.xml", "<w:document/>"));

    [Theory]
    [InlineData("cv.pdf")]
    [InlineData("CV.PDF")]                  // extension casing is not a rejection
    [InlineData("resume")]                  // no extension claimed — the content decides
    public void ARealPdfPasses(string name) =>
        Assert.Equal(Pdf, CareersState.SniffCvMime(PdfBytes(), name));

    [Fact]
    public void ARealDocxPasses() =>
        Assert.Equal(Docx, CareersState.SniffCvMime(DocxBytes(), "cv.docx"));

    [Fact]
    public void ARealLegacyDocPasses() =>
        Assert.Equal(Doc, CareersState.SniffCvMime(OleBytes(withWordStream: true), "cv.doc"));

    [Theory]
    [InlineData("cv.pdf")]
    [InlineData("cv.doc")]
    [InlineData("cv.docx")]
    public void ARenamedExecutableFailsWhateverItIsCalled(string name) =>
        // THE test of §6: the verdict comes from the bytes. An MZ header carries no document
        // signature, so no filename can launder it through intake.
        Assert.Null(CareersState.SniffCvMime(ExeBytes(), name));

    [Fact]
    public void AFilenameAloneGrantsNothing()
    {
        // Plain prose named cv.pdf: a client-supplied name (or content type) is an assertion, not
        // evidence. An implementation that trusts the extension passes this garbage — and fails here.
        var prose = Encoding.UTF8.GetBytes("Dear hiring manager, please find my CV attached.");
        Assert.Null(CareersState.SniffCvMime(prose, "cv.pdf"));
        Assert.Null(CareersState.SniffCvMime(prose, "cv.docx"));
    }

    [Fact]
    public void APlainArchiveIsNotADocxHoweverItIsNamed()
    {
        // §6: no archives. PK magic alone is every zip ever made; only the OOXML structure —
        // content-types entry plus the word/ main part — makes it a Word document.
        var zip = ZipBytes(("readme.txt", "not a cv"));
        Assert.Null(CareersState.SniffCvMime(zip, "cv.docx"));
        Assert.Null(CareersState.SniffCvMime(zip, "cv.zip"));
    }

    [Fact]
    public void ASpreadsheetShapedZipIsNotADocx()
    {
        var xlsx = ZipBytes(
            ("[Content_Types].xml", "<Types/>"),
            ("xl/workbook.xml", "<workbook/>"));
        Assert.Null(CareersState.SniffCvMime(xlsx, "cv.docx"));
    }

    [Fact]
    public void AnOleFileWithoutAWordStreamIsNotADoc() =>
        // The OLE header is shared by XLS, PPT and MSI installers; only the WordDocument stream
        // name makes it a Word document.
        Assert.Null(CareersState.SniffCvMime(OleBytes(withWordStream: false), "cv.doc"));

    [Fact]
    public void ImagesHtmlAndSvgAreRefused()
    {
        var png = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0, 0 };
        Assert.Null(CareersState.SniffCvMime(png, "cv.png"));
        Assert.Null(CareersState.SniffCvMime(Encoding.UTF8.GetBytes("<html><body>cv</body></html>"), "cv.html"));
        Assert.Null(CareersState.SniffCvMime(Encoding.UTF8.GetBytes("<svg xmlns='http://www.w3.org/2000/svg'/>"), "cv.svg"));
        Assert.Null(CareersState.SniffCvMime(new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0 }, "cv.jpg"));
    }

    [Fact]
    public void AContradictoryExtensionVetoesGenuineContent()
    {
        // Real PDF bytes named cv.exe is somebody probing the intake — the extension can veto,
        // it just can never grant.
        Assert.Null(CareersState.SniffCvMime(PdfBytes(), "cv.exe"));
        Assert.Null(CareersState.SniffCvMime(DocxBytes(), "cv.zip"));
        Assert.Null(CareersState.SniffCvMime(PdfBytes(), "cv.docx"));
    }

    [Fact]
    public void EmptyOrTinyPayloadsAreRefused()
    {
        Assert.Null(CareersState.SniffCvMime(Array.Empty<byte>(), "cv.pdf"));
        Assert.Null(CareersState.SniffCvMime(new byte[] { 0x25, 0x50, 0x44 }, "cv.pdf"));
    }

    [Fact]
    public void TheCvCapFitsInsideTheKestrelBodyCap()
    {
        // The platform caps request bodies at 6 MB; the CV cap must sit inside it so a maximal
        // upload is rejected by policy, not by a connection reset the applicant cannot interpret.
        Assert.Equal(6_000_000, CareersState.MaxCvBytes);
        Assert.True(CareersState.MaxCvBytes <= 6 * 1024 * 1024);
    }
}
