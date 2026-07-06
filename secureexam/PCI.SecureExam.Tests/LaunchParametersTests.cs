using PCI.SecureExam.Core;
using Xunit;

namespace PCI.SecureExam.Tests;

public class LaunchParametersTests
{
    [Fact]
    public void Parses_full_launch_uri()
    {
        var p = LaunchParameters.Parse("pciexam://start?code=PCI-ABC123&api=https%3A%2F%2Fapi.pci.org&token=sess_9");
        Assert.True(p.IsValid);
        Assert.Equal("PCI-ABC123", p.Code);
        Assert.Equal("https://api.pci.org", p.ApiBaseUrl);   // url-decoded
        Assert.Equal("sess_9", p.Token);
    }

    [Fact]
    public void Missing_code_is_invalid_but_does_not_throw()
    {
        var p = LaunchParameters.Parse("pciexam://start?api=https://api.pci.org");
        Assert.False(p.IsValid);
        Assert.Null(p.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("not a uri at all")]
    [InlineData("pciexam://")]
    public void Garbage_input_returns_empty(string? raw)
    {
        var p = LaunchParameters.Parse(raw);
        Assert.False(p.IsValid);
    }

    // WithLaunch only honours the launch-URI api= override when it passes domain pinning
    // (ClientConfig.IsTrustedApi). The old expectation that ANY override wins predates the
    // pinning hardening and contradicted the RunnableChecks attack suite.
    [Fact]
    public void Launch_override_ignored_for_untrusted_host()
    {
        var cfg = new ClientConfig { ApiBaseUrl = "https://exam.projectcontrolsinstitute.org" };
        cfg.WithLaunch(LaunchParameters.Parse("pciexam://start?code=X&api=https://evil.example"));
        Assert.Equal("https://exam.projectcontrolsinstitute.org", cfg.ApiBaseUrl);
    }

    [Fact]
    public void Launch_override_accepted_for_allowlisted_host()
    {
        var cfg = new ClientConfig { ApiBaseUrl = "https://exam.projectcontrolsinstitute.org" };
        cfg.WithLaunch(LaunchParameters.Parse("pciexam://start?code=X&api=https%3A%2F%2Feu.pci-global.org"));
        Assert.Equal("https://eu.pci-global.org", cfg.ApiBaseUrl);
    }
}
