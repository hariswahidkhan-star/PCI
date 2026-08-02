namespace PCI.SecureExam.Core;

/// <summary>
/// Strongly-typed client configuration. Loaded from appsettings.json at startup and overridable by the
/// pciexam:// launch URI (which carries the per-session api + token + code). Pure POCO so it is unit-tested
/// in the Core test project and shared by any host.
/// </summary>
public sealed class ClientConfig
{
    public string ApiBaseUrl { get; set; } = "https://exam.projectcontrolsinstitute.org";

    /// <summary>Host suffixes the client is permitted to talk to. Any ApiBaseUrl (from config or a
    /// launch URI) must match one of these or it is rejected — a malicious launch link cannot point
    /// the locked-down client at an untrusted server. localhost is allowed only for development.</summary>
    public string[] AllowedApiHosts { get; set; } =
    {
        "projectcontrolsinstitute.org",
        "localhost"
    };

    public ProctoringConfig Proctoring { get; set; } = new();
    public AiConfig Ai { get; set; } = new();
    public FeatureConfig Features { get; set; } = new();
    public ChatConfig Chat { get; set; } = new();

    /// <summary>True if <paramref name="url"/> is an absolute HTTPS (or localhost) URL whose host is,
    /// or is a sub-domain of, one of <see cref="AllowedApiHosts"/>.</summary>
    public bool IsTrustedApi(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;
        var host = uri.Host.ToLowerInvariant();
        var isLocal = host is "localhost" or "127.0.0.1";
        // Enforce HTTPS for any non-local host (no plaintext exam traffic).
        if (!isLocal && uri.Scheme != Uri.UriSchemeHttps) return false;
        foreach (var allowed in AllowedApiHosts)
        {
            var a = allowed.Trim().ToLowerInvariant();
            if (a.Length == 0) continue;
            if (host == a || host.EndsWith("." + a)) return true;
        }
        return false;
    }

    /// <summary>Overlay per-session values parsed from the launch URI onto the file config.
    /// The launch-supplied ApiBaseUrl is accepted ONLY if it passes domain pinning; otherwise it is
    /// ignored and the trusted configured value stands.</summary>
    public ClientConfig WithLaunch(LaunchParameters launch)
    {
        if (!string.IsNullOrWhiteSpace(launch.ApiBaseUrl) && IsTrustedApi(launch.ApiBaseUrl))
            ApiBaseUrl = launch.ApiBaseUrl!;
        return this;
    }

    /// <summary>Call after loading config + launch. Throws if the effective endpoint is not trusted,
    /// so the client refuses to start against an untrusted server.</summary>
    public void EnsureTrustedOrThrow()
    {
        if (!IsTrustedApi(ApiBaseUrl))
            throw new System.InvalidOperationException(
                $"Refusing to start: API endpoint '{ApiBaseUrl}' is not an approved PCI domain.");
    }
}

public sealed class ProctoringConfig
{
    public int EvidenceIntervalSeconds { get; set; } = 15;
    public double FaceCheckFps { get; set; } = 1.25;
    public int AbsenceSeconds { get; set; } = 8;
    public double LoudAudioRms { get; set; } = 0.28;
}

public sealed class AiConfig
{
    /// <summary>Baseline | AzureFace | AwsRekognition — selects the IIdentityVerifier implementation.</summary>
    public string IdentityProvider { get; set; } = "Baseline";
    /// <summary>Baseline | AzureVision | Custom — selects the IProctorAnalyzer implementation.</summary>
    public string MonitorProvider { get; set; } = "Baseline";
    public string? Endpoint { get; set; }
    public string? ApiKey { get; set; }
    public string? Region { get; set; }
    /// <summary>Confidence at/above which a provider match is treated as Verified.</summary>
    public double IdentityMatchThreshold { get; set; } = 0.80;
}

public sealed class FeatureConfig
{
    public bool RequireIdentity { get; set; } = true;
    public bool RequireRoomScan { get; set; } = true;
    public bool BlockOnSecondMonitor { get; set; } = true;
    public bool TerminateOnProhibitedProcess { get; set; } = false;
    public bool RegisterUriScheme { get; set; } = true;
}

public sealed class ChatConfig
{
    public string HubPath { get; set; } = "/hubs/proctor";
    public bool Enabled { get; set; } = true;
}
