using System.Net;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class EgressTests
{
    [Theory]
    // loopback / unspecified
    [InlineData("127.0.0.1")]
    [InlineData("127.8.9.10")]
    [InlineData("0.0.0.0")]
    [InlineData("::1")]
    [InlineData("::")]
    // RFC1918 private
    [InlineData("10.0.0.1")]
    [InlineData("172.16.0.1")]
    [InlineData("172.31.255.254")]
    [InlineData("192.168.1.50")]
    // link-local incl. the cloud metadata service
    [InlineData("169.254.169.254")]
    [InlineData("169.254.0.1")]
    // CGNAT 100.64.0.0/10
    [InlineData("100.64.0.1")]
    [InlineData("100.127.255.254")]
    // IETF protocol assignments, multicast, broadcast
    [InlineData("192.0.0.8")]
    [InlineData("224.0.0.251")]
    [InlineData("255.255.255.255")]
    // IPv6 link-local / unique-local / site-local / multicast / v4-mapped private
    [InlineData("fe80::1")]
    [InlineData("fc00::1")]
    [InlineData("fd12:3456::1")]
    [InlineData("fec0::1")]
    [InlineData("ff02::1")]
    [InlineData("::ffff:192.168.0.10")]
    [InlineData("::ffff:169.254.169.254")]
    public void IsBlockedIp_RefusesInternalAddresses(string ip) =>
        Assert.True(Egress.IsBlockedIp(IPAddress.Parse(ip)));

    [Theory]
    [InlineData("8.8.8.8")]
    [InlineData("1.1.1.1")]
    [InlineData("172.15.255.255")]     // just below 172.16.0.0/12
    [InlineData("172.32.0.1")]         // just above 172.16.0.0/12
    [InlineData("100.63.255.255")]     // just below CGNAT
    [InlineData("100.128.0.1")]        // just above CGNAT
    [InlineData("2001:4860:4860::8888")]
    [InlineData("::ffff:8.8.4.4")]     // v4-mapped public
    public void IsBlockedIp_AllowsPublicAddresses(string ip) =>
        Assert.False(Egress.IsBlockedIp(IPAddress.Parse(ip)));

    [Fact]
    public void UrlProblem_RejectsNonHttpSchemes()
    {
        Assert.NotNull(Egress.UrlProblem("ftp://example.com/x"));
        Assert.NotNull(Egress.UrlProblem("not a url"));
        Assert.NotNull(Egress.UrlProblem("file:///etc/passwd"));
    }

    [Fact]
    public void UrlProblem_FlagsLiteralPrivateHosts()
    {
        // literal IPs need no DNS, so this stays hermetic
        Assert.NotNull(Egress.UrlProblem("http://127.0.0.1:8080/hook"));
        Assert.NotNull(Egress.UrlProblem("https://192.168.1.10/webhook"));
        Assert.NotNull(Egress.UrlProblem("http://[::1]/x"));
        Assert.Null(Egress.UrlProblem("https://8.8.8.8/webhook"));
    }
}
