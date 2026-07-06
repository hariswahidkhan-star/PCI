using Microsoft.Extensions.Logging;
using PCI.SecureExam.Core;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Providers;

/// <summary>
/// The single seam for plugging in a production AI provider. Selects the identity + monitoring
/// implementations from configuration (appsettings.json → Ai:IdentityProvider / Ai:MonitorProvider).
/// To add a provider: implement IIdentityVerifier / IProctorAnalyzer, add a case here, flip the config
/// value and set Ai:ApiKey/Endpoint. No other code changes.
/// </summary>
public static class AiProviderFactory
{
    public static IIdentityVerifier CreateIdentityVerifier(ClientConfig cfg, Func<byte[], int> faceCounter, ILogger? log = null)
        => cfg.Ai.IdentityProvider?.Trim().ToLowerInvariant() switch
        {
            "azureface" => new AzureFaceIdentityVerifier(cfg.Ai, log),
            "awsrekognition" => new AwsRekognitionIdentityVerifier(cfg.Ai, log),
            _ => new BaselineIdentityVerifier(faceCounter)   // ships; fully offline
        };

    public static IProctorAnalyzer CreateMonitor(ClientConfig cfg, ILogger? log = null)
        => cfg.Ai.MonitorProvider?.Trim().ToLowerInvariant() switch
        {
            // add "azurevision" / "custom" here when wired
            _ => new BaselineProctorAnalyzer { AbsenceSeconds = cfg.Proctoring.AbsenceSeconds, LoudRmsThreshold = cfg.Proctoring.LoudAudioRms }
        };
}

/// <summary>Template for an Azure Face provider. Fill in the REST/SDK call; the seam is done.</summary>
public sealed class AzureFaceIdentityVerifier : IIdentityVerifier
{
    private readonly AiConfig _cfg; private readonly ILogger? _log;
    public AzureFaceIdentityVerifier(AiConfig cfg, ILogger? log = null)
    {
        _cfg = cfg; _log = log;
        if (string.IsNullOrWhiteSpace(cfg.Endpoint) || string.IsNullOrWhiteSpace(cfg.ApiKey))
            _log?.LogWarning("AzureFace selected but Ai:Endpoint/Ai:ApiKey are not set — verification will fail closed.");
    }
    public Task<IdentityCheck> VerifyAsync(byte[] faceJpeg, byte[] idJpeg, CancellationToken ct = default)
    {
        // TODO(prod): POST faceJpeg + idJpeg to Azure Face 'verify'; map similarity -> IdentityResult using _cfg.IdentityMatchThreshold.
        // Failing closed (never fabricate a match) until wired:
        return Task.FromResult(new IdentityCheck(IdentityResult.Inconclusive, 0,
            null, null, "Azure Face provider not configured — set Ai:Endpoint and Ai:ApiKey."));
    }
}

/// <summary>Template for an AWS Rekognition provider.</summary>
public sealed class AwsRekognitionIdentityVerifier : IIdentityVerifier
{
    private readonly AiConfig _cfg; private readonly ILogger? _log;
    public AwsRekognitionIdentityVerifier(AiConfig cfg, ILogger? log = null) { _cfg = cfg; _log = log; }
    public Task<IdentityCheck> VerifyAsync(byte[] faceJpeg, byte[] idJpeg, CancellationToken ct = default)
        => Task.FromResult(new IdentityCheck(IdentityResult.Inconclusive, 0, null, null,
            "AWS Rekognition provider not configured — set Ai:Region and credentials."));
}
