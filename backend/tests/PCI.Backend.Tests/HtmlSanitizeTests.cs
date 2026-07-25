using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Security — <see cref="HtmlSanitize.Clean"/> is the whitelist sanitizer for operator-edited rich-text
/// blocks that are served to every visitor. It is the primary stored-XSS defence on the dynamic
/// page-block / blog surface, and had no unit coverage. These pin the guarantees the doc-comment
/// promises: scriptable subtrees are dropped whole, only whitelisted tags/attributes survive, URL
/// attributes must carry a safe scheme, style can't smuggle script, and the markup is re-balanced.
/// </summary>
public class HtmlSanitizeTests
{
    [Theory]
    [InlineData("<script>alert(1)</script>hi", "alert")]
    [InlineData("<style>body{background:url(x)}</style>ok", "background")]
    [InlineData("<iframe src=\"//evil\"></iframe>text", "evil")]
    [InlineData("<svg onload=alert(1)></svg>keep", "onload")]
    [InlineData("<object data=\"x\"></object>y", "object")]
    public void Clean_DropsScriptableSubtreesEntirely(string input, string forbidden)
    {
        var outp = HtmlSanitize.Clean(input);
        Assert.DoesNotContain(forbidden, outp, System.StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<script", outp, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Clean_KeepsTheTextAfterADroppedScript()
    {
        Assert.Equal("hi", HtmlSanitize.Clean("<script>alert(1)</script>hi"));
    }

    [Theory]
    [InlineData("<a href=\"javascript:alert(1)\">x</a>", "javascript:")]
    [InlineData("<a href=\"vbscript:msgbox(1)\">x</a>", "vbscript:")]
    [InlineData("<a href=\"data:text/html,<script>\">x</a>", "data:")]
    [InlineData("<img src=\"javascript:alert(1)\">", "javascript:")]
    public void Clean_StripsUnsafeUrlSchemesButKeepsTheTag(string input, string forbiddenScheme)
    {
        var outp = HtmlSanitize.Clean(input);
        Assert.DoesNotContain(forbiddenScheme, outp, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Clean_KeepsASafeHttpLink()
    {
        var outp = HtmlSanitize.Clean("<a href=\"https://pci.example/x\">link</a>");
        Assert.Contains("href=\"https://pci.example/x\"", outp);
        Assert.Contains(">link</a>", outp);
    }

    [Theory]
    [InlineData("<p onclick=\"steal()\">t</p>", "onclick")]
    [InlineData("<img src=\"ok.png\" onerror=\"steal()\">", "onerror")]
    [InlineData("<div onmouseover=\"x\">t</div>", "onmouseover")]
    public void Clean_StripsEventHandlerAttributes(string input, string handler)
    {
        Assert.DoesNotContain(handler, HtmlSanitize.Clean(input), System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Clean_DropsScriptableStyleButKeepsPlainStyle()
    {
        // A style that smuggles a script sink is dropped…
        Assert.DoesNotContain("javascript", HtmlSanitize.Clean("<p style=\"width:expression(alert(1))\">t</p>"), System.StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("expression", HtmlSanitize.Clean("<p style=\"width:expression(alert(1))\">t</p>"), System.StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("url(", HtmlSanitize.Clean("<p style=\"background:url(javascript:alert(1))\">t</p>"), System.StringComparison.OrdinalIgnoreCase);
        // …but an ordinary style survives.
        Assert.Contains("style=\"color:red\"", HtmlSanitize.Clean("<p style=\"color:red\">t</p>"));
    }

    [Fact]
    public void Clean_AddsRelNoopener_WhenOpeningANewTab()
    {
        var outp = HtmlSanitize.Clean("<a href=\"https://x.co\" target=\"_blank\">x</a>");
        Assert.Contains("rel=\"noopener\"", outp);
    }

    [Fact]
    public void Clean_KeepsWhitelistedFormatting_AndUnwrapsUnknownTags()
    {
        Assert.Equal("<p>hi <strong>there</strong></p>", HtmlSanitize.Clean("<p>hi <strong>there</strong></p>"));
        // An unknown structural tag is unwrapped — its text is kept, the tag is gone.
        Assert.Equal("keep <b>me</b>", HtmlSanitize.Clean("<section>keep <b>me</b></section>"));
    }

    [Fact]
    public void Clean_RebalancesMarkupAndEscapesStrayText()
    {
        // An unclosed tag is auto-closed so a half-finished edit can't break the page around it.
        Assert.Equal("<p>unclosed</p>", HtmlSanitize.Clean("<p>unclosed"));
        // A stray close tag is dropped.
        Assert.Equal("orphan", HtmlSanitize.Clean("orphan</p>"));
        // A bare '<' in text is escaped, never treated as a tag.
        Assert.Equal("<p>a &lt; b</p>", HtmlSanitize.Clean("<p>a < b</p>"));
    }

    [Fact]
    public void Clean_RemovesCommentsAndHandlesEmpty()
    {
        Assert.Equal("keep", HtmlSanitize.Clean("<!-- secret note -->keep"));
        Assert.Equal("", HtmlSanitize.Clean(""));
        Assert.Equal("", HtmlSanitize.Clean(null!));
    }

    [Fact]
    public void Clean_KeepsThePriceTokenBindingAttribute()
    {
        // data-price survives so live price tokens keep binding inside edited blocks.
        var outp = HtmlSanitize.Clean("<span data-price=\"membership\">$0</span>");
        Assert.Contains("data-price=\"membership\"", outp);
    }
}
