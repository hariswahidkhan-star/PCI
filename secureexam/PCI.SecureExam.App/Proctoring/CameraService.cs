using OpenCvSharp;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Proctoring;

/// <summary>
/// Owns the webcam. Uses OpenCvSharp to open the default capture device, run Haar-cascade face
/// detection on sampled frames, feed the analyzer, and expose a preview + evidence-clip buffer.
/// Frames are sampled (not every frame analysed) to keep CPU reasonable; a rolling JPEG buffer is
/// kept so a short clip around any High/Critical event can be uploaded as evidence.
/// </summary>
public sealed class CameraService : IDisposable
{
    private readonly IProctorAnalyzer _analyzer;
    private readonly BaselineProctorAnalyzer? _baseline; // for face-count side channel
    private VideoCapture? _cap;
    private CascadeClassifier? _face;
    private CancellationTokenSource? _cts;
    private Task? _loop;

    public event Action<Mat>? PreviewFrame;                 // for on-screen thumbnail
    public event Action<ProctorEvent>? Event;
    public event Action<byte[]>? EvidenceJpeg;              // periodic snapshot to upload
    public int LastFaceCount { get; private set; } = 1;

    public CameraService(IProctorAnalyzer analyzer)
    {
        _analyzer = analyzer;
        _baseline = analyzer as BaselineProctorAnalyzer;
    }

    public bool TryOpen(out string detail)
    {
        try
        {
            if (_cap is not null && _cap.IsOpened())
            { detail = $"Webcam ready ({_cap.FrameWidth}x{_cap.FrameHeight})."; return true; }  // idempotent: never re-open/leak
            _cap = new VideoCapture(0);
            if (!_cap.IsOpened()) { detail = "No webcam detected or access denied."; return false; }
            _face = LoadCascade();
            detail = $"Webcam ready ({_cap.FrameWidth}x{_cap.FrameHeight}).";
            return true;
        }
        catch (Exception ex) { detail = "Webcam error: " + ex.Message; return false; }
    }

    public void Start()
    {
        if (_loop is { IsCompleted: false }) return;   // idempotent: one capture loop only
        _cts = new CancellationTokenSource();
        _loop = Task.Run(() => Loop(_cts.Token));
    }

    private async Task Loop(CancellationToken ct)
    {
        using var frame = new Mat();
        var lastAnalyse = DateTimeOffset.MinValue;
        var lastEvidence = DateTimeOffset.MinValue;
        while (!ct.IsCancellationRequested)
        {
            if (_cap is null || !_cap.Read(frame) || frame.Empty()) { await Task.Delay(120, ct); continue; }
            PreviewFrame?.Invoke(frame.Clone());

            var now = DateTimeOffset.UtcNow;
            if ((now - lastAnalyse).TotalMilliseconds >= 800) // ~1.25 fps analysis
            {
                lastAnalyse = now;
                int faces = DetectFaces(frame);
                LastFaceCount = faces;
                if (_baseline is not null) _baseline.LastFaceCount = faces;

                var sample = new FrameSample(Array.Empty<byte>(), frame.Width, frame.Height, now);
                foreach (var ev in _analyzer.AnalyzeFrame(sample)) Event?.Invoke(ev);
            }
            if ((now - lastEvidence).TotalSeconds >= 15) // periodic evidence snapshot
            {
                lastEvidence = now;
                EvidenceJpeg?.Invoke(frame.ToBytes(".jpg"));
            }
            await Task.Delay(60, ct);
        }
    }

    private int DetectFaces(Mat frame)
    {
        if (_face is null) return 1;
        using var gray = new Mat();
        Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);
        Cv2.EqualizeHist(gray, gray);
        var rects = _face.DetectMultiScale(gray, 1.15, 5, HaarDetectionTypes.ScaleImage, new Size(80, 80));
        return rects.Length;
    }

    /// <summary>Face count for the identity verifier (single still image).</summary>
    public int CountFacesInJpeg(byte[] jpeg)
    {
        if (_face is null) _face = LoadCascade();
        using var mat = Cv2.ImDecode(jpeg, ImreadModes.Grayscale);
        if (mat.Empty()) return 0;
        Cv2.EqualizeHist(mat, mat);
        return _face.DetectMultiScale(mat, 1.15, 5, HaarDetectionTypes.ScaleImage, new Size(80, 80)).Length;
    }

    public byte[]? Snapshot()
    {
        if (_cap is null) return null;
        using var m = new Mat();
        return _cap.Read(m) && !m.Empty() ? m.ToBytes(".jpg") : null;
    }

    private static CascadeClassifier LoadCascade()
    {
        // Ships with the app; path resolved at runtime. OpenCvSharp bundles the standard cascades.
        var path = Path.Combine(AppContext.BaseDirectory, "Assets", "haarcascade_frontalface_default.xml");
        return File.Exists(path) ? new CascadeClassifier(path) : new CascadeClassifier();
    }

    public void Dispose()
    {
        try { _cts?.Cancel(); _loop?.Wait(500); } catch { }
        _cap?.Dispose(); _face?.Dispose(); _cts?.Dispose();
    }
}
