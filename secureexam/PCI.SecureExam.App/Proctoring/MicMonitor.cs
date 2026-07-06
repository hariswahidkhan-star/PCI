using NAudio.Wave;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Proctoring;

/// <summary>
/// Owns the microphone. Uses NAudio to capture the default input, computes rolling RMS energy and a
/// crude voice-likelihood (energy + zero-crossing) and feeds the analyzer. Full speech recognition /
/// speaker-diarisation is a production upgrade behind the same IProctorAnalyzer seam.
/// </summary>
public sealed class MicMonitor : IDisposable
{
    private readonly IProctorAnalyzer _analyzer;
    private WaveInEvent? _wave;
    public event Action<ProctorEvent>? Event;
    public event Action<double>? Level; // 0..1 for a live meter

    public MicMonitor(IProctorAnalyzer analyzer) => _analyzer = analyzer;

    public bool TryStart(out string detail)
    {
        try
        {
            if (WaveInEvent.DeviceCount == 0) { detail = "No microphone detected."; return false; }
            _wave = new WaveInEvent { WaveFormat = new WaveFormat(16000, 16, 1), BufferMilliseconds = 200 };
            _wave.DataAvailable += OnData;
            _wave.StartRecording();
            detail = "Microphone ready.";
            return true;
        }
        catch (Exception ex) { detail = "Microphone error: " + ex.Message; return false; }
    }

    private void OnData(object? sender, WaveInEventArgs e)
    {
        long sumSq = 0; int samples = e.BytesRecorded / 2; int zc = 0; short prev = 0;
        for (int i = 0; i < e.BytesRecorded; i += 2)
        {
            short s = BitConverter.ToInt16(e.Buffer, i);
            sumSq += (long)s * s;
            if ((s >= 0) != (prev >= 0)) zc++;
            prev = s;
        }
        double rms = samples > 0 ? Math.Sqrt(sumSq / (double)samples) / 32768.0 : 0;
        bool voice = rms > 0.06 && zc > samples * 0.05 && zc < samples * 0.35;
        Level?.Invoke(Math.Min(1, rms * 3));

        var sample = new AudioSample(rms, voice, DateTimeOffset.UtcNow);
        foreach (var ev in _analyzer.AnalyzeAudio(sample)) Event?.Invoke(ev);
    }

    public void Dispose() { try { _wave?.StopRecording(); } catch { } _wave?.Dispose(); }
}
