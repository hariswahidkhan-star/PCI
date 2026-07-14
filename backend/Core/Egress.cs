using System.Net;
using System.Net.Sockets;

namespace PCI.Backend.Core;

/// <summary>
/// Outbound-egress guard for the admin-configured connectors (Phase 11 security-review hardening).
/// The integrations pipeline calls admin-supplied URLs by design (webhook endpoint_url, QuickBooks
/// api_base override) — an admin trust boundary — but the server should still refuse to be pointed at
/// itself or its host network: loopback, RFC1918/link-local ranges and the cloud metadata service
/// (169.254.169.254), which would turn a compromised admin account into an SSRF primitive.
///
/// Enforcement happens at connect time via <see cref="SocketsHttpHandler.ConnectCallback"/>: the guard
/// resolves the hostname itself and dials only public addresses, so an endpoint whose DNS later changes
/// to an internal IP (rebinding) is blocked at the socket, not just at config time. Redirects are not
/// followed (a public URL 302-ing to an internal one is the classic bypass). Admin-save-time validation
/// (<see cref="UrlProblem"/>) exists purely for fast operator feedback.
///
/// For local development and self-hosted deployments that legitimately deliver to a private ERP bridge,
/// set INTEGRATIONS_ALLOW_PRIVATE_EGRESS=true to disable the guard (documented in RUN.md / OPERATIONS.md).
/// </summary>
public static class Egress
{
    /// <summary>Opt-out for dev / private-network deployments. Read once at boot.</summary>
    public static readonly bool AllowPrivate =
        string.Equals(Environment.GetEnvironmentVariable("INTEGRATIONS_ALLOW_PRIVATE_EGRESS"), "true", StringComparison.OrdinalIgnoreCase);

    /// <summary>True for any address that must never be dialled by a connector: loopback, unspecified,
    /// multicast/broadcast, RFC1918 private, link-local (incl. the cloud metadata service), CGNAT,
    /// and their IPv6 equivalents (loopback, link-local, unique-local, IPv4-mapped forms).</summary>
    public static bool IsBlockedIp(IPAddress ip)
    {
        if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
        if (IPAddress.IsLoopback(ip) || ip.Equals(IPAddress.Any) || ip.Equals(IPAddress.IPv6Any)) return true;
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            var b = ip.GetAddressBytes();
            return b[0] == 10                                    // 10.0.0.0/8
                || (b[0] == 172 && b[1] >= 16 && b[1] <= 31)     // 172.16.0.0/12
                || (b[0] == 192 && b[1] == 168)                  // 192.168.0.0/16
                || (b[0] == 169 && b[1] == 254)                  // 169.254.0.0/16 (link-local + cloud metadata)
                || (b[0] == 100 && b[1] >= 64 && b[1] <= 127)    // 100.64.0.0/10 (CGNAT)
                || (b[0] == 192 && b[1] == 0 && b[2] == 0)       // 192.0.0.0/24 (IETF protocol assignments)
                || b[0] == 0                                     // 0.0.0.0/8
                || b[0] >= 224;                                  // multicast + reserved + broadcast
        }
        return ip.IsIPv6LinkLocal || ip.IsIPv6Multicast || ip.IsIPv6UniqueLocal   // fe80::/10, ff00::/8, fc00::/7
            || ip.IsIPv6SiteLocal;                                                // fec0::/10 (deprecated but reserved)
    }

    /// <summary>Admin-save-time check for a connector URL. Returns a human-readable problem, or null when
    /// the URL is acceptable. A hostname that does not resolve yet is accepted — the connect-time guard is
    /// the enforcement point; this is operator feedback only.</summary>
    public static string? UrlProblem(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var u) || (u.Scheme != "http" && u.Scheme != "https"))
            return "Endpoint must be an http(s) URL.";
        if (AllowPrivate) return null;
        try
        {
            var addrs = IPAddress.TryParse(u.Host.Trim('[', ']'), out var lit)
                ? new[] { lit }
                : Dns.GetHostAddresses(u.Host);
            if (addrs.Length > 0 && addrs.All(IsBlockedIp))
                return "Endpoint resolves to a private/internal address, which outbound connectors may not call.";
        }
        catch { /* unresolvable now — the connect-time guard still enforces at delivery */ }
        return null;
    }

    /// <summary>An HttpClient for connector deliveries. With the guard active it never follows redirects
    /// and only dials public IP addresses (DNS is resolved inside the connect callback, so the vetted
    /// address is the dialled address).</summary>
    public static HttpClient CreateClient(TimeSpan timeout)
    {
        if (AllowPrivate) return new HttpClient { Timeout = timeout };
        var handler = new SocketsHttpHandler
        {
            AllowAutoRedirect = false,
            ConnectCallback = async (ctx, ct) =>
            {
                var host = ctx.DnsEndPoint.Host;
                var addrs = IPAddress.TryParse(host.Trim('[', ']'), out var lit)
                    ? new[] { lit }
                    : await Dns.GetHostAddressesAsync(host, ct);
                var allowed = addrs.Where(a => !IsBlockedIp(a)).ToArray();
                if (allowed.Length == 0)
                    throw new HttpRequestException($"egress blocked: '{host}' resolves to a private/internal address");
                var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
                try
                {
                    await socket.ConnectAsync(allowed, ctx.DnsEndPoint.Port, ct);
                    return new NetworkStream(socket, ownsSocket: true);
                }
                catch { socket.Dispose(); throw; }
            },
        };
        return new HttpClient(handler) { Timeout = timeout };
    }
}
