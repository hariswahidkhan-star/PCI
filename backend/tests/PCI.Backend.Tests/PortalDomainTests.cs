using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Student-panel domain separation (RES-013): the panel lives on mypci.org while the marketing site
/// stays on the institute domain.
///
/// The property that matters most is the LAST one: with nothing configured, every behaviour here is
/// inert. Domain separation is an opt-in for deployments that want it, not a change that alters a
/// single-domain install the moment it ships.
/// </summary>
[Collection("portal-domain")]
public class PortalDomainTests : IDisposable
{
    public PortalDomainTests() => Clear();
    public void Dispose() => Clear();

    static void Clear()
    {
        foreach (var k in new[] { "PORTAL_BASE_URL", "PORTAL_HOSTS", "APP_BASE_URL", "SITE_BASE_URL" })
            Environment.SetEnvironmentVariable(k, null);
    }

    static void Configure(string portal = "https://mypci.org", string? institute = "https://projectcontrolsinstitute.org")
    {
        Environment.SetEnvironmentVariable("PORTAL_BASE_URL", portal);
        Environment.SetEnvironmentVariable("APP_BASE_URL", institute);
    }

    [Fact]
    public void OffByDefault_LinksStayRelativeAndNothingIsRewritten()
    {
        Assert.False(PortalDomain.Enabled);
        Assert.Equal("/app/billing", PortalDomain.Url("/app/billing"));
        const string html = @"<a href=""/app/login"">Sign in</a>";
        Assert.Equal(html, PortalDomain.RewriteLinks(html));
    }

    [Fact]
    public void ConfiguringAPortalOriginEnablesSeparation()
    {
        Configure();
        Assert.True(PortalDomain.Enabled);
        Assert.Equal("https://mypci.org", PortalDomain.BaseUrl);
        Assert.Equal("https://projectcontrolsinstitute.org", PortalDomain.PublicBaseUrl);
    }

    [Fact]
    public void PointingBothAtTheSameHostIsANoOpNotARedirectLoop()
    {
        // Setting PORTAL_BASE_URL to the site's own origin must not start redirecting /app to itself.
        Configure(portal: "https://projectcontrolsinstitute.org");
        Assert.False(PortalDomain.Enabled);
    }

    [Fact]
    public void PortalBaseUrlIsReducedToItsOrigin()
    {
        // A trailing path in the env var would otherwise produce https://mypci.org/x/app/login.
        Configure(portal: "https://mypci.org/some/path");
        Assert.Equal("https://mypci.org", PortalDomain.BaseUrl);
        Assert.Equal("https://mypci.org/app/login", PortalDomain.Url("/app/login"));
    }

    [Fact]
    public void PortalHostsIncludeTheBaseUrlHostAndAnyExtras()
    {
        Configure();
        Environment.SetEnvironmentVariable("PORTAL_HOSTS", "www.mypci.org, portal.mypci.org");
        Assert.True(PortalDomain.IsPortalHost("mypci.org"));
        Assert.True(PortalDomain.IsPortalHost("www.mypci.org"));
        Assert.True(PortalDomain.IsPortalHost("portal.mypci.org"));
        Assert.False(PortalDomain.IsPortalHost("projectcontrolsinstitute.org"));
    }

    [Theory]
    [InlineData("/app")]
    [InlineData("/app/")]
    [InlineData("/app/billing")]
    [InlineData("/student.html")]
    public void StudentPanelPathsBelongToThePortal(string path) =>
        Assert.True(PortalDomain.IsPortalPath(path));

    [Theory]
    [InlineData("/")]
    [InlineData("/about.html")]
    [InlineData("/certifications")]
    // "/apply" and "/appeals" start with the letters "app" but are marketing pages — a prefix match
    // without a segment boundary would wrongly send them to the portal domain.
    [InlineData("/apply")]
    [InlineData("/appeals")]
    public void MarketingPathsDoNotBelongToThePortal(string path) =>
        Assert.False(PortalDomain.IsPortalPath(path));

    [Theory]
    [InlineData("/api/me")]
    [InlineData("/assets/app.js")]
    [InlineData("/admin")]
    [InlineData("/robots.txt")]
    public void SharedPathsWorkOnBothHosts(string path) =>
        Assert.True(PortalDomain.IsSharedPath(path));

    [Fact]
    public void RewriteSendsPortalLinksToThePortalDomain()
    {
        Configure();
        var html = @"<a href=""/app/login"">Sign in</a><a href='/app/register'>Join</a><form action=""/app/onboarding"">";
        var outHtml = PortalDomain.RewriteLinks(html);
        Assert.Contains(@"href=""https://mypci.org/app/login""", outHtml);
        Assert.Contains("href='https://mypci.org/app/register'", outHtml);
        Assert.Contains(@"action=""https://mypci.org/app/onboarding""", outHtml);
    }

    [Fact]
    public void RewriteLeavesMarketingAndExternalLinksAlone()
    {
        Configure();
        var html = @"<a href=""/apply"">Apply</a><a href=""/appeals"">Appeals</a><a href=""https://example.com/app/x"">ext</a>";
        Assert.Equal(html, PortalDomain.RewriteLinks(html));
    }

    [Fact]
    public void RewriteIsInertWhenSeparationIsOff()
    {
        var html = @"<a href=""/app/login"">Sign in</a>";
        Assert.Equal(html, PortalDomain.RewriteLinks(html));
    }

    [Fact]
    public void MailerPortalLinksFollowThePortalDomain()
    {
        Configure();
        Assert.Equal("https://mypci.org/app/billing", Mailer.PortalLink("/app/billing"));
        // A password link posts a credential, so it must stay on the portal origin.
        Assert.StartsWith("https://mypci.org/reset-password.html?token=", Mailer.SetupLink("https://projectcontrolsinstitute.org", "tok"));
    }

    [Fact]
    public void MailerPortalLinksFallBackToTheSiteBaseWhenSeparationIsOff()
    {
        Environment.SetEnvironmentVariable("APP_BASE_URL", "https://projectcontrolsinstitute.org");
        Assert.Equal("https://projectcontrolsinstitute.org/app/billing", Mailer.PortalLink("/app/billing"));
        Assert.StartsWith("https://projectcontrolsinstitute.org/reset-password.html?token=", Mailer.SetupLink("https://projectcontrolsinstitute.org", "tok"));
    }

    [Fact]
    public void EmailTemplatesAbsolutiseRelativePortalLinks()
    {
        Configure();
        // Comms templates pass portal_link as "/app/billing"; a relative href is dead in a mail client.
        var html = Mailer.Template("no-such-template", new Dictionary<string, string>
        {
            ["BODY"] = @"<a href=""{{PORTAL_LINK}}"">Open</a>",
            ["PORTAL_LINK"] = "/app/billing",
        });
        Assert.Contains("https://mypci.org/app/billing", html);
    }
}
