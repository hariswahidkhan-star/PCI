using System.Diagnostics;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Enforces a process allow-list. At launch, blocks the exam from starting if prohibited
/// applications (browsers, chat, screen-share, remote-desktop, VMs, recorders) are running.
/// During the exam, polls the foreground/among-running processes and raises ProctorEvents;
/// can optionally terminate freshly launched prohibited processes.
/// HONEST LIMIT: user-mode termination is defeatable by a determined user with elevated tools;
/// kernel-level guarantees require a signed driver, which is out of scope for source delivery.
/// </summary>
public sealed class ProcessGuard : IDisposable
{
    // Denylist by process name (lowercase, no extension). Extend per policy.
    private static readonly string[] Denylist =
    {
        "chrome","msedge","firefox","opera","brave","zoom","teams","skype","discord","slack",
        "anydesk","teamviewer","rustdesk","obs64","obs32","obs","camtasia","snagit","bandicam",
        "vmware","vmwareuser","virtualbox","vboxservice","vboxtray","mstsc","quicktimeplayer",
        "screenconnect","logmein","gotomeeting","webexmta","parsec"
    };
    private readonly System.Timers.Timer _timer = new(2500);
    private readonly bool _terminate;
    public event Action<ProctorEvent>? Event;

    public ProcessGuard(bool terminateOnDetect = false) { _terminate = terminateOnDetect; _timer.Elapsed += (_, _) => Scan(); }

    /// <summary>Called before the exam starts. Returns the list of prohibited processes currently running.</summary>
    public IReadOnlyList<string> PreflightBlockingProcesses() =>
        Process.GetProcesses()
            .Select(p => Safe(() => p.ProcessName.ToLowerInvariant()))
            .Where(n => n is not null && Denylist.Contains(n))
            .Distinct().Select(n => n!).ToList();

    public void StartMonitoring() => _timer.Start();

    private void Scan()
    {
        foreach (var p in Process.GetProcesses())
        {
            var name = Safe(() => p.ProcessName.ToLowerInvariant());
            if (name is null || !Denylist.Contains(name)) continue;
            Event?.Invoke(new ProctorEvent(ProctorEventType.ProhibitedProcess, ProctorSeverity.High, DateTimeOffset.UtcNow, name));
            if (_terminate) Safe(() => { p.Kill(); return 0; });
        }
    }

    private static T? Safe<T>(Func<T> f) { try { return f(); } catch { return default; } }
    public void Dispose() => _timer.Dispose();
}
