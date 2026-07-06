using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;
using Xunit;

namespace PCI.SecureExam.Tests;

public class BaselineAnalyzerTests
{
    private static FrameSample Frame() => new(Array.Empty<byte>(), 640, 480, DateTimeOffset.UtcNow);

    [Fact]
    public void Multiple_faces_raises_high_event()
    {
        var a = new BaselineProctorAnalyzer { LastFaceCount = 2 };
        var events = a.AnalyzeFrame(Frame()).ToList();
        Assert.Contains(events, e => e.Type == ProctorEventType.MultipleFacesDetected && e.Severity == ProctorSeverity.High);
    }

    [Fact]
    public void Single_face_is_clean()
    {
        var a = new BaselineProctorAnalyzer { LastFaceCount = 1 };
        Assert.Empty(a.AnalyzeFrame(Frame()));
    }

    [Fact]
    public void Loud_audio_raises_event_above_threshold()
    {
        var a = new BaselineProctorAnalyzer { LoudRmsThreshold = 0.25 };
        var loud = a.AnalyzeAudio(new AudioSample(0.9, false, DateTimeOffset.UtcNow)).ToList();
        Assert.Contains(loud, e => e.Type == ProctorEventType.LoudAudioDetected);
        var quiet = a.AnalyzeAudio(new AudioSample(0.01, false, DateTimeOffset.UtcNow)).ToList();
        Assert.DoesNotContain(quiet, e => e.Type == ProctorEventType.LoudAudioDetected);
    }
}
