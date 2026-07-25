using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class CommsTests
{
    [Fact]
    public void Render_SubstitutesVariablesAndBlanksUnknownTokens()
    {
        var vars = new Dictionary<string, string?> { ["first_name"] = "Aisha", ["cert"] = "PCI-CP" };
        Assert.Equal("Hello Aisha, your PCI-CP awaits.",
            Comms.Render("Hello {{first_name}}, your {{cert}} awaits.", vars));
        Assert.Equal("Hello Aisha!", Comms.Render("Hello {{ first_name }}!", vars));   // inner whitespace ok
        Assert.Equal("Hi , welcome", Comms.Render("Hi {{unknown_var}}, welcome", vars)); // never leaked as literal
        Assert.Equal("A  B", Comms.Render("A {{nulled}} B", new Dictionary<string, string?> { ["nulled"] = null }));
        Assert.Equal("no vars", Comms.Render("no vars", null));
        Assert.Equal("", Comms.Render(null, vars));
    }

    [Fact]
    public void UnsubToken_RoundTripsPerUser()
    {
        var t1 = Comms.UnsubToken(42);
        var t2 = Comms.UnsubToken(43);
        Assert.Equal(42L, Comms.VerifyUnsub(t1));
        Assert.Equal(43L, Comms.VerifyUnsub(t2));
        Assert.NotEqual(t1, t2);
        Assert.StartsWith("42.", t1);
    }

    [Fact]
    public void VerifyUnsub_RejectsTamperAndMalformedTokens()
    {
        var good = Comms.UnsubToken(7);
        var parts = good.Split('.');
        Assert.Null(Comms.VerifyUnsub($"8.{parts[1]}"));                 // signature is bound to the uid
        Assert.Null(Comms.VerifyUnsub($"{parts[0]}.{new string('a', parts[1].Length)}"));
        Assert.Null(Comms.VerifyUnsub(good + ".extra"));
        Assert.Null(Comms.VerifyUnsub("no-dot-here"));
        Assert.Null(Comms.VerifyUnsub("abc.def"));                       // non-numeric uid
        Assert.Null(Comms.VerifyUnsub(""));
        Assert.Null(Comms.VerifyUnsub(null));
    }
}

public class MarketingOAuthTests
{
    const long Now = 1_750_000_000;   // fixed epoch — the API takes 'now' explicitly, so no clock coupling

    [Fact]
    public void State_RoundTripsWithinTheHour()
    {
        var state = MarketingOAuth.State(123, Now);
        Assert.Equal(123L, MarketingOAuth.VerifyState(state, Now));
        Assert.Equal(123L, MarketingOAuth.VerifyState(state, Now + 3599));   // still inside the window
        Assert.Equal(3, state.Split('.').Length);
    }

    [Fact]
    public void VerifyState_RejectsExpiredState()
    {
        var state = MarketingOAuth.State(5, Now);
        Assert.Null(MarketingOAuth.VerifyState(state, Now + 3601));
    }

    [Fact]
    public void VerifyState_RejectsTamperedOrMalformedState()
    {
        var state = MarketingOAuth.State(9, Now);
        var p = state.Split('.');
        Assert.Null(MarketingOAuth.VerifyState($"10.{p[1]}.{p[2]}", Now));           // conn id swap
        Assert.Null(MarketingOAuth.VerifyState($"{p[0]}.{long.Parse(p[1]) + 60}.{p[2]}", Now)); // expiry extension
        Assert.Null(MarketingOAuth.VerifyState($"{p[0]}.{p[1]}.{new string('0', p[2].Length)}", Now));
        Assert.Null(MarketingOAuth.VerifyState("just-one-part", Now));
        Assert.Null(MarketingOAuth.VerifyState("a.b.c", Now));
        Assert.Null(MarketingOAuth.VerifyState("", Now));
        Assert.Null(MarketingOAuth.VerifyState(null, Now));
    }
}
