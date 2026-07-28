using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// The public, crawlable careers surface (Core/CareersRender.cs, the WorldSeo sitemap seam;
/// docs/pciworld/CCP_PHASE4_DESIGN.md §7).
///
/// THE HONESTY RULE IS WHAT THESE TESTS PROTECT. Structured data is a claim to a search engine,
/// and a stale claim is a lie that outlives the page — so the tests assert, row by row, that
/// JobPosting markup, sitemap membership and the visible page agree with live state computed at
/// render time: a deadline that passed a second ago must already have killed the markup, with no
/// background sweep involved. And the boundary between "closed" (reachable, honest, noindex) and
/// "draft/withdrawn/non-verified" (a public 404, because a tenant's draft is tenant-confidential)
/// must never blur in either direction.
///
/// The XSS tests at the bottom re-litigate the exact bug found in ForumRender during this
/// increment: UnsafeRelaxedJsonEscaping leaves '&lt;' literal inside the ld+json script block, so
/// a title of the shape &lt;/script&gt;&lt;svg…&gt; becomes live markup. Feeding precisely that
/// shape is what catches an encoder regression.
/// </summary>
[Collection(DbCollection.Name)]
public class CareersRenderTests
{
    static string Now => H.StampInMinutes(0);

    static Db NewDb()
    {
        var db = TestEnv.NewMigratedDb();
        WorldSchema.Ensure(db);
        Settings.Put(db, "pciworld_careers_enabled", "1");
        return db;
    }

    static long Employer(Db db, string slug, string state = "verified",
                         string name = "Acme Controls Ltd", string? website = "https://acme.example")
        => db.ExecuteReturningId(
            "INSERT INTO pciworld_employers(slug,name,website,state) VALUES(?,?,?,?)",
            slug, name, website, state);

    static long Posting(Db db, long employerId, string slug, string state = "published",
                        string title = "Senior Planner", string? closesAt = null,
                        string? description = "Own the integrated programme.",
                        string? location = "Bristol", string? employmentType = "full_time",
                        long? salaryMin = null, long? salaryMax = null, string? currency = null)
        => db.ExecuteReturningId(
            @"INSERT INTO pciworld_job_postings
                (employer_id,slug,title,description,location,employment_type,
                 salary_min_minor,salary_max_minor,currency,state,published_at,closes_at)
              VALUES(?,?,?,?,?,?,?,?,?,?,?,?)",
            employerId, slug, title, description, location, employmentType,
            salaryMin, salaryMax, currency, state,
            state == "draft" ? null : TestEnv.Stamp("-1 day"), closesAt);

    // ── the job page: live ────────────────────────────────────────────────────────────────────

    [Fact]
    public void ALiveJobPageCarriesJobPostingMarkupAndIsIndexable()
    {
        var db = NewDb();
        var e = Employer(db, "acme-live");
        Posting(db, e, "senior-planner", closesAt: TestEnv.Stamp("+30 day"),
                salaryMin: 5_500_000, salaryMax: 6_500_000, currency: "GBP");

        var html = CareersRender.Job(db, "acme-live", "senior-planner", Now, "https://world.example");
        Assert.NotNull(html);
        Assert.Contains("\"@type\":\"JobPosting\"", html);
        Assert.Contains("\"directApply\":true", html);
        Assert.Contains("\"hiringOrganization\"", html);
        Assert.Contains("Acme Controls Ltd", html);
        Assert.Contains("<meta name=\"robots\" content=\"index, follow\"/>", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://world.example/world/careers/acme-live/senior-planner\"/>", html);
        // The page states the salary the markup claims — currency beside the amounts, no float.
        Assert.Contains("GBP 55,000 to 65,000", html);
        Assert.DoesNotContain("no longer accepting applications", html);
    }

    [Fact]
    public void NoExternalApplyUrlAppearsAnywhere()
    {
        // §7: in-platform apply only. The employer website belongs on the employer line and in
        // sameAs; the apply path must never bounce an applicant to an external form.
        var db = NewDb();
        var e = Employer(db, "acme-apply", website: "https://acme.example");
        Posting(db, e, "planner");

        var html = CareersRender.Job(db, "acme-apply", "planner", Now)!;
        Assert.Contains("this platform only", html);
        Assert.DoesNotContain("applicationContact", html);
        Assert.Contains("\"directApply\":true", html);
    }

    // ── the job page: closed is honest, not gone ──────────────────────────────────────────────

    [Fact]
    public void AClosedPostingStaysReachableButMakesNoMachineClaim()
    {
        // A saved link should explain itself, not 404 — but the page must carry NO JobPosting
        // block and be noindex, because a crawler's cache outlives the page (§7).
        var db = NewDb();
        var e = Employer(db, "acme-closed");
        Posting(db, e, "cost-engineer", state: "closed", title: "Cost Engineer");

        var html = CareersRender.Job(db, "acme-closed", "cost-engineer", Now);
        Assert.NotNull(html);
        Assert.Contains("no longer accepting applications", html);
        Assert.Contains("role=\"status\"", html);
        Assert.Contains("<meta name=\"robots\" content=\"noindex\"/>", html);
        Assert.DoesNotContain("JobPosting", html);
        Assert.DoesNotContain("application/ld+json", html);
        Assert.Contains("Cost Engineer", html);   // the human still sees what the role was
    }

    [Fact]
    public void APassedDeadlineClosesThePageAtRenderTimeWithNoSweep()
    {
        // The row still says 'published' — no background job has stamped it 'closed' — and the
        // page must ALREADY be honest. This is the whole point of computing liveness at render
        // time rather than trusting a state column a sweep maintains (§7).
        var db = NewDb();
        var e = Employer(db, "acme-expired");
        Posting(db, e, "risk-lead", state: "published", closesAt: TestEnv.Stamp("-1 hour"));

        var html = CareersRender.Job(db, "acme-expired", "risk-lead", Now);
        Assert.NotNull(html);
        Assert.Contains("no longer accepting applications", html);
        Assert.Contains("<meta name=\"robots\" content=\"noindex\"/>", html);
        Assert.DoesNotContain("JobPosting", html);
    }

    // ── the job page: what must 404 ───────────────────────────────────────────────────────────

    [Fact]
    public void ADraftOrWithdrawnPostingIsAPublic404()
    {
        // Not a closed page and not a 403 — a tenant's draft is tenant-confidential, and a 403
        // confirms it exists (§7). Null from the renderer is the endpoint's NotFound.
        var db = NewDb();
        var e = Employer(db, "acme-private");
        Posting(db, e, "draft-role", state: "draft", title: "SECRET-DRAFT-TITLE");
        Posting(db, e, "pulled-role", state: "withdrawn", title: "PULLED-TITLE");

        Assert.Null(CareersRender.Job(db, "acme-private", "draft-role", Now));
        Assert.Null(CareersRender.Job(db, "acme-private", "pulled-role", Now));
    }

    [Theory]
    [InlineData("draft")]
    [InlineData("pending_verification")]
    [InlineData("suspended")]
    public void AnyPostingOfANonVerifiedEmployerIsAPublic404(string employerState)
    {
        // Even a 'published' posting: the read-side predicate is the second, independent half of
        // the §4.3 verify-before-publish control, and it is what makes one suspension UPDATE
        // unpublish a tenant's whole surface mid-session.
        var db = NewDb();
        var e = Employer(db, "shady-" + employerState, state: employerState);
        Posting(db, e, "too-good-role");

        Assert.Null(CareersRender.Job(db, "shady-" + employerState, "too-good-role", Now));
        Assert.Null(CareersRender.Employer(db, "shady-" + employerState, Now));
    }

    [Fact]
    public void NothingRendersWhileTheFlagIsOff()
    {
        var db = NewDb();
        var e = Employer(db, "acme-off");
        Posting(db, e, "role-off");
        Settings.Put(db, "pciworld_careers_enabled", "0");

        Assert.Null(CareersRender.List(db, Now));
        Assert.Null(CareersRender.Job(db, "acme-off", "role-off", Now));
        Assert.Null(CareersRender.Employer(db, "acme-off", Now));
    }

    // ── the list ──────────────────────────────────────────────────────────────────────────────

    [Fact]
    public void TheListShowsLivePostingsAndOnlyLivePostings()
    {
        // One posting of every non-live shape beside one live one; exactly one survivor. The
        // predicates are asserted through the output because the output is what gets crawled.
        var db = NewDb();
        var good = Employer(db, "good-emp");
        Posting(db, good, "live-role", title: "LIVE-ROLE-TITLE", closesAt: TestEnv.Stamp("+10 day"));
        Posting(db, good, "closed-role", state: "closed", title: "CLOSED-ROLE-TITLE");
        Posting(db, good, "draft-role", state: "draft", title: "DRAFT-ROLE-TITLE");
        Posting(db, good, "withdrawn-role", state: "withdrawn", title: "WITHDRAWN-ROLE-TITLE");
        Posting(db, good, "expired-role", state: "published", title: "EXPIRED-ROLE-TITLE",
                closesAt: TestEnv.Stamp("-1 day"));
        var bad = Employer(db, "suspended-emp", state: "suspended");
        Posting(db, bad, "phantom-role", title: "SUSPENDED-EMP-TITLE");

        var html = CareersRender.List(db, Now)!;
        Assert.Contains("LIVE-ROLE-TITLE", html);
        foreach (var leaked in new[] { "CLOSED-ROLE-TITLE", "DRAFT-ROLE-TITLE", "WITHDRAWN-ROLE-TITLE",
                                       "EXPIRED-ROLE-TITLE", "SUSPENDED-EMP-TITLE" })
            Assert.DoesNotContain(leaked, html);
        // List semantics, not a div soup — a screen reader announces "list, 1 item".
        Assert.Contains("<ul class=\"wc-jobs\">", html);
        Assert.Contains("<li class=\"wc-job\">", html);
    }

    [Fact]
    public void AnUnparsableDeadlineFailsClosedEverywhere()
    {
        // closes_at is stored as text, and garbage like "soon" sorts ABOVE any date in the lexical
        // SQL compare — without the render-time IsLive re-check this job would be advertised for
        // ever. An unreadable deadline must not advertise a job (CareersState.IsLive fails closed).
        var db = NewDb();
        var e = Employer(db, "acme-garbled");
        var p = Posting(db, e, "garbled-role", title: "GARBLED-DEADLINE-TITLE", closesAt: "soon");
        db.Execute("UPDATE pciworld_job_postings SET updated_at=? WHERE id=?", TestEnv.Stamp("+19 second"), p);

        Assert.DoesNotContain("GARBLED-DEADLINE-TITLE", CareersRender.List(db, Now)!);
        var page = CareersRender.Job(db, "acme-garbled", "garbled-role", Now)!;
        Assert.DoesNotContain("JobPosting", page);
        Assert.Contains("no longer accepting applications", page);
        Assert.DoesNotContain("garbled-role", WorldSeo.Sitemap(db));
    }

    [Fact]
    public void AnEmptyBoardIsStillAPage()
    {
        // The URL is stable and linked; an empty marketplace says so rather than 404ing.
        var db = NewDb();
        var html = CareersRender.List(db, Now);
        Assert.NotNull(html);
        Assert.Contains("no open roles", html);
        Assert.Contains("<meta name=\"robots\" content=\"index, follow\"/>", html);
    }

    // ── the employer page ─────────────────────────────────────────────────────────────────────

    [Fact]
    public void AVerifiedEmployerPageEmitsOrganizationMarkupAndItsLiveRoles()
    {
        var db = NewDb();
        var e = Employer(db, "acme-org", name: "Acme Controls Ltd", website: "https://acme.example");
        Posting(db, e, "org-live-role", title: "ORG-LIVE-TITLE");
        Posting(db, e, "org-closed-role", state: "closed", title: "ORG-CLOSED-TITLE");

        var html = CareersRender.Employer(db, "acme-org", Now, "https://world.example")!;
        Assert.Contains("\"@type\":\"Organization\"", html);
        Assert.Contains("\"sameAs\":\"https://acme.example\"", html);
        Assert.Contains("ORG-LIVE-TITLE", html);
        Assert.DoesNotContain("ORG-CLOSED-TITLE", html);
        Assert.Contains("<link rel=\"canonical\" href=\"https://world.example/world/employers/acme-org\"/>", html);
        // "Verified" is explained in words, not asserted by a badge glyph alone.
        Assert.Contains("Verified employer", html);
    }

    [Fact]
    public void AnEmployerWebsiteIsLinkedOnlyWhenItIsPlainlyHttp()
    {
        // The website column is tenant-supplied text. A javascript: value must never become an
        // href on a public page — it renders unlinked or not at all.
        var db = NewDb();
        Employer(db, "acme-jsurl", website: "javascript:alert(1)");
        var html = CareersRender.Employer(db, "acme-jsurl", Now)!;
        Assert.DoesNotContain("href=\"javascript:", html);
        Assert.DoesNotContain("javascript:alert(1)", html);
    }

    // ── the sitemap seam ──────────────────────────────────────────────────────────────────────

    [Fact]
    public void TheSitemapListsLivePostingsOfVerifiedEmployersAndNothingElse()
    {
        var db = NewDb();
        var good = Employer(db, "map-emp");
        var live = Posting(db, good, "map-live", closesAt: TestEnv.Stamp("+5 day"));
        // WorldSeo caches on a content signature; a distinct updated_at keeps this test's
        // signature unique even when suites run within the same clock second.
        db.Execute("UPDATE pciworld_job_postings SET updated_at=? WHERE id=?", TestEnv.Stamp("+11 second"), live);
        Posting(db, good, "map-closed", state: "closed");
        Posting(db, good, "map-draft", state: "draft");
        Posting(db, good, "map-expired", state: "published", closesAt: TestEnv.Stamp("-2 day"));
        var bad = Employer(db, "map-bad-emp", state: "pending_verification");
        Posting(db, bad, "map-unverified");

        var xml = WorldSeo.Sitemap(db);
        Assert.Contains("/world/careers/map-emp/map-live</loc>", xml);
        Assert.DoesNotContain("map-closed", xml);
        Assert.DoesNotContain("map-draft", xml);
        Assert.DoesNotContain("map-expired", xml);
        Assert.DoesNotContain("map-unverified", xml);
        // "and nothing else": no employer pages either (§7).
        Assert.DoesNotContain("/world/employers/", xml);
    }

    [Fact]
    public void ClosingAPostingRemovesItFromTheSitemapImmediately()
    {
        var db = NewDb();
        var e = Employer(db, "gone-emp");
        var p = Posting(db, e, "gone-role", closesAt: TestEnv.Stamp("+3 day"));
        db.Execute("UPDATE pciworld_job_postings SET updated_at=? WHERE id=?", TestEnv.Stamp("+13 second"), p);
        Assert.Contains("/world/careers/gone-emp/gone-role</loc>", WorldSeo.Sitemap(db));

        db.Execute("UPDATE pciworld_job_postings SET state='closed', updated_at=? WHERE id=?",
                   TestEnv.Stamp("+17 second"), p);
        Assert.DoesNotContain("/world/careers/gone-emp/gone-role</loc>", WorldSeo.Sitemap(db));
    }

    [Fact]
    public void TheSitemapCarriesNoCareersEntriesWhileTheFlagIsOff()
    {
        var db = NewDb();
        var e = Employer(db, "flag-emp");
        Posting(db, e, "flag-role");
        Settings.Put(db, "pciworld_careers_enabled", "0");
        Assert.DoesNotContain("/world/careers/", WorldSeo.Sitemap(db));
    }

    // ── accessibility structure (the axe suite scans the rendered pattern; these assert it) ───

    [Fact]
    public void EveryPageIsACompleteDocumentWithOneH1ABreadcrumbAndAWorkingSkipLink()
    {
        var db = NewDb();
        var e = Employer(db, "acme-a11y");
        Posting(db, e, "a11y-role");

        foreach (var html in new[]
        {
            CareersRender.List(db, Now)!,
            CareersRender.Job(db, "acme-a11y", "a11y-role", Now)!,
            CareersRender.Employer(db, "acme-a11y", Now)!,
        })
        {
            Assert.StartsWith("<!DOCTYPE html>", html);
            Assert.Contains("<html lang=\"en\">", html);
            Assert.Contains("<title>", html);
            // Exactly one h1 — role names and employer names render as h2 within lists.
            Assert.Equal(1, CountOf(html, "<h1>"));
            Assert.Contains("aria-label=\"Breadcrumb\"", html);
            Assert.Contains("aria-current=\"page\"", html);
            // The skip link's target must exist on the same page.
            Assert.Contains("href=\"#wc-main\"", html);
            Assert.Contains("id=\"wc-main\"", html);
            Assert.Contains("name=\"viewport\"", html);
        }
    }

    // ── author text stays text ────────────────────────────────────────────────────────────────

    [Fact]
    public void AHostileJobTitleCannotEscapeTheJsonLdScriptBlock()
    {
        // The exact ForumRender bug, §28: with UnsafeRelaxedJsonEscaping, '<' survives into the
        // <script type="application/ld+json"> block, so this title closes the tag early and the
        // svg becomes live markup — stored XSS on a crawlable, cacheable page. The DEFAULT
        // encoder emits \u003C, inert and still valid JSON-LD.
        var db = NewDb();
        var e = Employer(db, "acme-xss");
        Posting(db, e, "xss-role", title: "</script><svg onload=alert(1)>");

        var html = CareersRender.Job(db, "acme-xss", "xss-role", Now)!;
        Assert.Contains("application/ld+json", html);          // the markup IS emitted (live job)…
        Assert.DoesNotContain("</script><svg", html);          // …but the title cannot break out
        Assert.DoesNotContain("<svg onload", html);
        Assert.Contains("\\u003C/script\\u003E", html);        // default-encoder escaping, verbatim
    }

    [Fact]
    public void AHostileEmployerNameCannotEscapeTheOrganizationBlockEither()
    {
        // Same rule for the employer page's own serializer (CareersRender.JsonOpts), which is a
        // different JsonSerializerOptions instance from CareersState's — each must stay default.
        var db = NewDb();
        Employer(db, "evil-org", name: "</script><svg onload=alert(2)>Evil");
        var html = CareersRender.Employer(db, "evil-org", Now)!;
        Assert.DoesNotContain("</script><svg", html);
        Assert.DoesNotContain("<svg onload", html);
    }

    [Fact]
    public void EmployerAuthoredTextIsHtmlEscapedInTheVisibleMarkup()
    {
        var db = NewDb();
        var e = Employer(db, "acme-esc", name: "<b>Bold & Co</b>");
        Posting(db, e, "esc-role", title: "<script>alert(1)</script>",
                description: "Line one <img src=x onerror=alert(1)>", location: "<i>Nowhere</i>");

        var page = CareersRender.Job(db, "acme-esc", "esc-role", Now)!;
        Assert.DoesNotContain("<script>alert(1)</script>", page);
        Assert.DoesNotContain("<img src=x", page);
        Assert.DoesNotContain("<i>Nowhere</i>", page);
        Assert.Contains("&lt;script&gt;", page);

        var list = CareersRender.List(db, Now)!;
        Assert.DoesNotContain("<b>Bold & Co</b>", list);
    }

    static int CountOf(string haystack, string needle)
    {
        int count = 0, at = 0;
        while ((at = haystack.IndexOf(needle, at, StringComparison.Ordinal)) >= 0) { count++; at += needle.Length; }
        return count;
    }
}
