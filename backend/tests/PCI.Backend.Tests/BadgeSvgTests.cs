using System.Reflection;
using PCI.Backend.Endpoints;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// SEC-4 — the public Open-Badges SVG image is rendered from a certification's acronym, which is
/// admin-controlled data interpolated into markup and served as <c>image/svg+xml</c>. If it were not
/// escaped, a crafted acronym could break out of the SVG text/aria-label and inject active content
/// that a validator or browser would execute. <c>Badges.BadgeSvg</c> HTML-encodes it (and clamps to
/// 10 chars); this pins that guard. BadgeSvg is private, so it's reached by reflection.
/// </summary>
public class BadgeSvgTests
{
    static string BadgeSvg(string acronym)
    {
        var m = typeof(Badges).GetMethod("BadgeSvg", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(m);
        return (string)m!.Invoke(null, new object[] { acronym })!;
    }

    [Fact]
    public void BadgeSvg_EscapesAngleBracketsSoInjectedMarkupCannotRender()
    {
        var svg = BadgeSvg("<script>alert(1)</script>");
        // The raw tag must never survive into the output…
        Assert.DoesNotContain("<script", svg);
        // …and the angle brackets must appear only as HTML entities.
        Assert.Contains("&lt;", svg);
        // The payload beyond the 10-char clamp ("alert(1)…") is dropped entirely.
        Assert.DoesNotContain("alert", svg);
    }

    [Fact]
    public void BadgeSvg_EscapesQuotesSoTheAriaLabelAttributeCannotBeBroken()
    {
        var svg = BadgeSvg("\"><x");
        // A double-quote in the value would otherwise close the aria-label="…" attribute.
        Assert.DoesNotContain("\"><x", svg);
        Assert.Contains("&quot;", svg);
    }

    [Fact]
    public void BadgeSvg_ClampsToTenCharactersAndRendersOrdinaryAcronyms()
    {
        var svg = BadgeSvg("PCL-AI");
        Assert.Contains("PCL-AI", svg);
        Assert.StartsWith("<svg", svg);

        // A 20-char acronym is truncated to its first 10 characters.
        var longSvg = BadgeSvg("ABCDEFGHIJKLMNOPQRST");
        Assert.Contains("ABCDEFGHIJ", longSvg);
        Assert.DoesNotContain("ABCDEFGHIJK", longSvg);
    }
}
