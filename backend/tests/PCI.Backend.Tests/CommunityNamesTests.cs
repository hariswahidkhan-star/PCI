using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community — guest display-name rules (Core/CommunityNames.cs; PW-US-018 … PW-US-020).
///
/// Test data convention: characters that are VISIBLE (Arabic, Urdu, Cyrillic, Greek, fullwidth,
/// CJK) are written literally, because seeing the real string is the point of the assertion.
/// Characters that are INVISIBLE or indistinguishable — zero-width spaces, bidi controls,
/// private-use and astral-plane code points — are written as \uXXXX escapes, because literal ones
/// are unreviewable in a diff and turned the implementation file into a "binary file" for grep on
/// the first attempt.
///
/// NOTE ON THE ENVIRONMENT: this test project does NOT set InvariantGlobalization, while the app
/// project does. Any rule that leaned on String.Normalize would therefore pass here and fail in
/// production. CommunityNames folds explicitly and never calls Normalize, so these results hold in
/// both modes — which is the property these tests exist to protect.
/// </summary>
public class CommunityNamesTests
{
    // ── Display preserves the participant's own script ────────────────────────────────────────

    [Theory]
    [InlineData("محمد")]                       // Arabic: Muhammad
    [InlineData("علي حسن")]          // Arabic: Ali Hasan
    [InlineData("ایاز")]                       // Urdu: Ayaz
    [InlineData("李明")]                                    // Chinese
    [InlineData("Наталья")]     // Cyrillic given name
    public void Display_PreservesLegitimateScriptsIntact(string name)
    {
        // PW-US-019: normalization must not destroy legitimate Arabic or Urdu text.
        Assert.Equal(name, CommunityNames.Display(name));
        Assert.True(CommunityNames.Validate(name).Ok, $"'{name}' should be an acceptable display name");
    }

    [Fact]
    public void Display_StripsZeroWidthAndBidiControls()
    {
        var sneaky = "Ali\u200BHassan\u202E";     // ZWSP inside, RLO appended
        Assert.Equal("AliHassan", CommunityNames.Display(sneaky));
    }

    [Fact]
    public void Display_CollapsesWhitespaceButDoesNotDeleteIt()
    {
        // "ad min" must NOT become "admin" — otherwise the reserved list is bypassed by a space.
        Assert.Equal("ad min", CommunityNames.Display("  ad   min  "));
        Assert.Equal("ad min", CommunityNames.Display("ad\tmin"));
        Assert.Equal("ad min", CommunityNames.Display("ad\nmin"));
    }

    // ── Folding: the evasions that matter ─────────────────────────────────────────────────────

    // These assert that a disguise collides with the REAL word, never that the skeleton equals a
    // particular literal. The skeleton's spelling is an internal detail of the fold table — it
    // currently renders "admin" as "admln", because i/l/1 collapse to one symbol — and pinning that
    // spelling in a test would break every time the table is tuned while proving nothing. What must
    // hold is the security property: every disguise lands on the same skeleton as the genuine word.

    [Fact]
    public void Fold_MapsFullwidthOntoAscii_TheCaseInvariantNormalizeSilentlyMisses()
    {
        // Regression guard for the real defect this module was written around: under
        // InvariantGlobalization, String.Normalize(FormKC) leaves fullwidth text unchanged, so
        // "ａｂ" would sail straight past a Normalize-based check.
        Assert.Equal(CommunityNames.Fold("ab"), CommunityNames.Fold("ａｂ"));
        Assert.Equal(CommunityNames.Fold("admin"), CommunityNames.Fold("ａｄｍｉｎ"));
        Assert.Equal(CommunityNames.Fold("admin"), CommunityNames.Fold("ＡＤＭＩＮ"));   // fullwidth UPPER
    }

    [Theory]
    [InlineData("аdmin")]              // Cyrillic a
    [InlineData("АDMIN")]              // Cyrillic A
    [InlineData("admіn")]              // Cyrillic i
    [InlineData("αdmin")]              // Greek alpha
    public void Fold_MapsHomoglyphsOntoTheLatinSkeleton(string spoofed)
    {
        Assert.Equal(CommunityNames.Fold("admin"), CommunityNames.Fold(spoofed));
    }

    [Theory]
    [InlineData("adm1n")]
    [InlineData("@dmin")]
    [InlineData("4dm1n")]
    [InlineData("a.d.m.i.n")]
    [InlineData("a d m i n")]
    public void Fold_DefeatsLeetspeakAndSeparatorPadding(string spoofed)
    {
        Assert.Equal(CommunityNames.Fold("admin"), CommunityNames.Fold(spoofed));
    }

    [Fact]
    public void Fold_CollapsesTheAmbiguousIandLandOneFamily()
    {
        // "1" is a credible homoglyph for BOTH "l" and "i", so no one-to-one mapping catches both
        // "adm1n" and "wa1l". The family is collapsed onto a single symbol instead. The documented
        // cost is a false collision between "user1" and "userl" — cheap next to a missed staff
        // impersonation, but pinned here so it stays a decision rather than an accident.
        Assert.Equal(CommunityNames.Fold("userl"), CommunityNames.Fold("user1"));
        Assert.Equal(CommunityNames.Fold("wall"), CommunityNames.Fold("wa1l"));
        Assert.Equal(CommunityNames.Fold("admin"), CommunityNames.Fold("adm1n"));
    }

    // ── Rejections ────────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("admin", "name_reserved")]
    [InlineData("PCI", "name_reserved")]
    [InlineData("Moderator", "name_reserved")]
    [InlineData("ｐｃｉ", "name_reserved")]      // fullwidth p c i
    [InlineData("аdmin", "name_reserved")]              // Cyrillic-a admin
    [InlineData("adm1n", "name_reserved")]
    public void Validate_RejectsReservedNamesThroughEveryDisguise(string name, string expected)
    {
        var v = CommunityNames.Validate(name);
        Assert.False(v.Ok, $"'{name}' must be rejected");
        Assert.Equal(expected, v.ReasonCode);
    }

    [Theory]
    [InlineData("PCI Admin Team")]
    [InlineData("pci-support")]
    [InlineData("Official PCI")]
    public void Validate_RejectsImpersonationOfPciAuthority(string name)
    {
        var v = CommunityNames.Validate(name);
        Assert.False(v.Ok, $"'{name}' must be rejected");
        Assert.Contains(v.ReasonCode, new[] { "name_impersonation", "name_reserved" });
    }

    [Theory]
    [InlineData("ali@example.com")]
    [InlineData("call 07700900123")]
    [InlineData("https://evil.example")]
    [InlineData("www.example.com")]
    public void Validate_RejectsContactDetailsAndLinks(string name)
    {
        var v = CommunityNames.Validate(name);
        Assert.False(v.Ok, $"'{name}' must be rejected");
        Assert.Equal("name_contact_details", v.ReasonCode);
    }

    [Theory]
    [InlineData("Verified")]
    [InlineData("Official Support")]
    public void Validate_RejectsBareBadgeClaims(string name)
    {
        var v = CommunityNames.Validate(name);
        Assert.False(v.Ok, $"'{name}' must be rejected");
        Assert.Contains(v.ReasonCode, new[] { "name_misleading_badge", "name_reserved", "name_impersonation" });
    }

    [Fact]
    public void Validate_RejectsProfanityIncludingObfuscated()
    {
        Assert.Equal("name_offensive", CommunityNames.Validate("sh1t head").ReasonCode);
        Assert.Equal("name_offensive", CommunityNames.Validate("FｕCK").ReasonCode);
    }

    [Theory]
    [InlineData("", "name_required")]
    [InlineData("a", "name_too_short")]
    [InlineData("...", "name_not_readable")]
    [InlineData("\u200B\u200B", "name_required")]
    public void Validate_RejectsUnusableNames(string name, string expected)
    {
        Assert.Equal(expected, CommunityNames.Validate(name).ReasonCode);
    }

    [Fact]
    public void Validate_RejectsStyledLatinAndPrivateUseCharacters()
    {
        // Mathematical bold capital A (U+1D400) renders as "A" but is a surrogate pair.
        Assert.Equal("name_unsupported_characters", CommunityNames.Validate("\uD835\uDC00dmin").ReasonCode);
        Assert.Equal("name_unsupported_characters", CommunityNames.Validate("ab\uE000cd").ReasonCode);
    }

    [Fact]
    public void Validate_TooLongIsRejectedWithATruncatedSuggestion()
    {
        var v = CommunityNames.Validate(new string('a', CommunityNames.MaxLength + 10));
        Assert.False(v.Ok);
        Assert.Equal("name_too_long", v.ReasonCode);
        Assert.NotNull(v.Suggestion);
        Assert.True(v.Suggestion!.Length <= CommunityNames.MaxLength);
    }

    // ── Acceptances: the rules must not be so blunt they block real people ────────────────────

    [Theory]
    [InlineData("Ali Hassan")]
    [InlineData("Sarah O'Neill")]
    [InlineData("Jean-Luc")]
    [InlineData("Ayaz K")]
    [InlineData("Ali Verified Ltd")]        // a company name, not a bare badge claim
    [InlineData("Project Lead 42")]         // digits are fine below the phone-number threshold
    public void Validate_AcceptsOrdinaryProfessionalNames(string name)
    {
        var v = CommunityNames.Validate(name);
        Assert.True(v.Ok, $"'{name}' should be accepted, got {v.ReasonCode}");
        Assert.Equal(name, v.Display);
    }

    // ── Duplicate suggestion ──────────────────────────────────────────────────────────────────

    [Fact]
    public void SuggestAlternative_ProducesAValidNonCollidingName()
    {
        var s = CommunityNames.SuggestAlternative("Ali Hassan", 2);
        Assert.NotNull(s);
        Assert.Equal("Ali Hassan 2", s);
        Assert.True(CommunityNames.Validate(s!).Ok);
        Assert.NotEqual(CommunityNames.Fold("Ali Hassan"), CommunityNames.Fold(s));
    }

    [Fact]
    public void SuggestAlternative_StaysWithinTheLengthLimit()
    {
        var s = CommunityNames.SuggestAlternative(new string('b', CommunityNames.MaxLength), 17);
        Assert.NotNull(s);
        Assert.True(s!.Length <= CommunityNames.MaxLength, $"suggestion '{s}' is {s.Length} chars");
        Assert.True(CommunityNames.Validate(s).Ok);
    }

    // ── The folded skeleton is never the stored display value ─────────────────────────────────

    [Fact]
    public void Validate_ReturnsBothTheOriginalDisplayAndTheFoldedSkeleton()
    {
        var arabic = "علي حسن";
        var v = CommunityNames.Validate(arabic);
        Assert.True(v.Ok);
        Assert.Equal(arabic, v.Display);                 // shown to people, unchanged
        Assert.NotEqual(arabic, v.Folded);               // compared against, lossy
        Assert.DoesNotContain(" ", v.Folded);
    }
}
