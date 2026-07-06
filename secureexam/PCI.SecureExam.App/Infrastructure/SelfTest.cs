using PCI.SecureExam.App.Proctoring;
using PCI.SecureExam.App.Security;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Infrastructure;

/// <summary>
/// `--selftest` entry point. Lets a developer or proctor validate a machine (camera, mic, displays, VM,
/// prohibited apps, connectivity) without launching a live exam. Prints a readable report and returns an
/// exit code (0 = ready, 1 = one or more blocking failures) so it can gate an automated rollout.
/// </summary>
public static class SelfTest
{
    public static int Run()
    {
        var rows = new List<(string name, bool pass, bool blocking, string detail)>();

        var analyzer = new BaselineProctorAnalyzer();
        using var cam = new CameraService(analyzer);
        var camOk = cam.TryOpen(out var camDetail);
        rows.Add(("Webcam", camOk, true, camDetail));

        using var mic = new MicMonitor(analyzer);
        var micOk = mic.TryStart(out var micDetail);
        rows.Add(("Microphone", micOk, true, micDetail));

        using var display = new DisplayGuard();
        var single = !display.IsMultiMonitor();
        rows.Add(("Displays", single, true, single ? "Single display." : "More than one display detected."));

        var vm = VmDetector.Check();
        rows.Add(("Environment", vm is null, false, vm?.Detail ?? "No VM/remote session detected."));

        using var proc = new ProcessGuard();
        var blocking = proc.PreflightBlockingProcesses();
        rows.Add(("Applications", blocking.Count == 0, true,
            blocking.Count == 0 ? "No prohibited applications." : "Close: " + string.Join(", ", blocking)));

        var online = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        rows.Add(("Connectivity", online, true, online ? "Network available." : "No network."));

        Console.WriteLine();
        Console.WriteLine("PCI Secure Exam — machine self-test");
        Console.WriteLine("===================================");
        foreach (var r in rows)
            Console.WriteLine($"[{(r.pass ? "PASS" : r.blocking ? "FAIL" : "WARN")}] {r.name,-14} {r.detail}");
        var blockingFails = rows.Count(r => !r.pass && r.blocking);
        Console.WriteLine("-----------------------------------");
        Console.WriteLine(blockingFails == 0 ? "READY: this machine can run a secured exam."
                                             : $"NOT READY: {blockingFails} blocking issue(s) above.");
        Console.WriteLine();
        return blockingFails == 0 ? 0 : 1;
    }
}
