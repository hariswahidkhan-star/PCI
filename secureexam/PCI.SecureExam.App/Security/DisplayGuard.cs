using System.Runtime.InteropServices;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Detects additional displays connecting mid-exam (a common cheat vector) and virtual-machine /
/// remote-desktop sessions at launch. Raises ProctorEvents; the exam window voids to a warning
/// overlay while a second display is attached.
/// </summary>
public sealed class DisplayGuard : IDisposable
{
    private readonly System.Timers.Timer _timer = new(3000);
    private int _lastCount;
    public event Action<ProctorEvent>? Event;
    public event Action<bool>? MultiMonitorChanged; // true = violation active

    public DisplayGuard() { _lastCount = MonitorCount(); _timer.Elapsed += (_, _) => Scan(); }
    public void Start() => _timer.Start();

    private void Scan()
    {
        int n = MonitorCount();
        if (n == _lastCount) return;
        if (n > _lastCount)
        {
            Event?.Invoke(new ProctorEvent(ProctorEventType.SecondDisplayConnected, ProctorSeverity.High, DateTimeOffset.UtcNow, $"{n} displays"));
            MultiMonitorChanged?.Invoke(true);
        }
        else
        {
            Event?.Invoke(new ProctorEvent(ProctorEventType.DisplayRemoved, ProctorSeverity.Low, DateTimeOffset.UtcNow, $"{n} displays"));
            if (n <= 1) MultiMonitorChanged?.Invoke(false);
        }
        _lastCount = n;
    }

    public static int MonitorCount() => GetSystemMetrics(80); // SM_CMONITORS
    public bool IsMultiMonitor() => MonitorCount() > 1;

    [DllImport("user32.dll")] private static extern int GetSystemMetrics(int nIndex);
    public void Dispose() => _timer.Dispose();
}
