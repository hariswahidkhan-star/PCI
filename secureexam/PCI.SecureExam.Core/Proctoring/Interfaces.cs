using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.Core.Proctoring;

/// <summary>A single analysed camera frame (grayscale/edges already reduced by the capture layer).</summary>
public record FrameSample(byte[] Jpeg, int Width, int Height, DateTimeOffset At);
public record AudioSample(double Rms, bool VoiceLikely, DateTimeOffset At);

/// <summary>
/// AI identity verification. The BaselineIdentityVerifier ships and does face-presence + capture so the
/// full flow works offline; a production deployment swaps in Azure Face / AWS Rekognition / an on-device
/// model by implementing this interface — no other code changes.
/// </summary>
public interface IIdentityVerifier
{
    Task<IdentityCheck> VerifyAsync(byte[] faceJpeg, byte[] idJpeg, CancellationToken ct = default);
}

/// <summary>
/// AI live-monitoring analyzer. The BaselineProctorAnalyzer ships with rule-based heuristics
/// (face count, absence, audio energy). Production swaps in trained vision/audio models here.
/// Returns zero or more ProctorEvents per sample.
/// </summary>
public interface IProctorAnalyzer
{
    IEnumerable<ProctorEvent> AnalyzeFrame(FrameSample frame);
    IEnumerable<ProctorEvent> AnalyzeAudio(AudioSample audio);
}
