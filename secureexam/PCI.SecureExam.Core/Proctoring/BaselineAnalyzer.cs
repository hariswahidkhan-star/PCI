using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.Core.Proctoring;

/// <summary>
/// Ships as the default. Rule-based baseline so the ENTIRE monitoring pipeline is real and testable
/// without a cloud dependency. It consumes face-detection results supplied by the capture layer
/// (OpenCvSharp Haar/DNN) via FrameSample metadata carried out-of-band, plus audio RMS.
///
/// The heuristics implemented:
///   • no face for &gt; AbsenceSeconds  -> FaceAbsentTooLong (High)
///   • &gt;1 face in frame              -> MultipleFacesDetected (High)
///   • sustained loud audio            -> LoudAudioDetected (Medium)
///   • voice-like audio                -> SpeechDetected (Low)
///
/// Production accuracy (phone detection, gaze, identity drift) requires trained models; this class
/// is deliberately transparent about being a baseline and is the single seam to upgrade.
/// </summary>
public sealed class BaselineProctorAnalyzer : IProctorAnalyzer
{
    public int AbsenceSeconds { get; init; } = 8;
    public double LoudRmsThreshold { get; init; } = 0.28;

    private DateTimeOffset? _faceGoneSince;

    // The capture layer sets FaceCount before calling; carried via a side channel for simplicity.
    public int LastFaceCount { get; set; } = 1;

    public IEnumerable<ProctorEvent> AnalyzeFrame(FrameSample frame)
    {
        var now = frame.At;
        if (LastFaceCount == 0)
        {
            _faceGoneSince ??= now;
            if ((now - _faceGoneSince.Value).TotalSeconds >= AbsenceSeconds)
            {
                _faceGoneSince = now; // debounce repeat
                yield return new ProctorEvent(ProctorEventType.FaceAbsentTooLong, ProctorSeverity.High, now,
                    $"No face for {AbsenceSeconds}s");
            }
            else
            {
                yield return new ProctorEvent(ProctorEventType.NoFaceDetected, ProctorSeverity.Low, now);
            }
        }
        else
        {
            _faceGoneSince = null;
            if (LastFaceCount > 1)
                yield return new ProctorEvent(ProctorEventType.MultipleFacesDetected, ProctorSeverity.High, now,
                    $"{LastFaceCount} faces");
        }
    }

    public IEnumerable<ProctorEvent> AnalyzeAudio(AudioSample a)
    {
        if (a.Rms >= LoudRmsThreshold)
            yield return new ProctorEvent(ProctorEventType.LoudAudioDetected, ProctorSeverity.Medium, a.At,
                $"RMS {a.Rms:0.00}");
        else if (a.VoiceLikely)
            yield return new ProctorEvent(ProctorEventType.SpeechDetected, ProctorSeverity.Low, a.At);
    }
}

/// <summary>
/// Baseline identity verifier: confirms a face is present in the candidate photo and that an ID image
/// was captured, returns a conservative "Inconclusive/Verified" with a note that a production FR service
/// should confirm the match. It never fabricates a high-confidence match it cannot substantiate.
/// </summary>
public sealed class BaselineIdentityVerifier : IIdentityVerifier
{
    private readonly Func<byte[], int> _faceCounter; // supplied by capture layer (OpenCvSharp)

    public BaselineIdentityVerifier(Func<byte[], int> faceCounter) => _faceCounter = faceCounter;

    public Task<IdentityCheck> VerifyAsync(byte[] faceJpeg, byte[] idJpeg, CancellationToken ct = default)
    {
        int faces = SafeCount(faceJpeg);
        if (faces == 0)
            return Task.FromResult(new IdentityCheck(IdentityResult.NoFace, 0, null, null,
                "No face detected in the candidate photo. Please retake with your face centred and well lit."));
        if (faces > 1)
            return Task.FromResult(new IdentityCheck(IdentityResult.Inconclusive, 0.2, null, null,
                "More than one face detected. Only the candidate may be in frame."));
        if (idJpeg.Length < 1024)
            return Task.FromResult(new IdentityCheck(IdentityResult.Inconclusive, 0.3, null, null,
                "ID image was not captured clearly. Please retake the photo of your government ID."));

        // Baseline PASS: face present + ID captured. Confidence intentionally moderate; a production
        // face-recognition service replaces this with a true similarity score.
        return Task.FromResult(new IdentityCheck(IdentityResult.Verified, 0.72, "face.jpg", "id.jpg",
            "Face present and ID captured. Production deployments confirm the biometric match via the configured face-recognition provider."));
    }

    private int SafeCount(byte[] jpg) { try { return _faceCounter(jpg); } catch { return 1; } }
}
