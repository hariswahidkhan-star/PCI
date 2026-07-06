using Microsoft.Win32;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Heuristic virtual-machine and remote-desktop detection at launch. Real proctoring vendors run
/// a continuously updated ruleset; this is an honest baseline covering the common signals.
/// </summary>
public static class VmDetector
{
    public static ProctorEvent? Check()
    {
        if (IsRemoteSession())
            return new ProctorEvent(ProctorEventType.RemoteDesktopDetected, ProctorSeverity.Critical, DateTimeOffset.UtcNow, "Remote session flag set");

        var hint = VmHint();
        if (hint is not null)
            return new ProctorEvent(ProctorEventType.VirtualMachineDetected, ProctorSeverity.Critical, DateTimeOffset.UtcNow, hint);

        return null;
    }

    private static bool IsRemoteSession()
    {
        try { return System.Windows.Forms.SystemInformation.TerminalServerSession; }
        catch { return false; }
    }

    private static string? VmHint()
    {
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            var vendor = (key?.GetValue("SystemManufacturer") as string ?? "") + " " +
                         (key?.GetValue("SystemProductName") as string ?? "");
            vendor = vendor.ToLowerInvariant();
            string[] flags = { "vmware", "virtualbox", "vbox", "qemu", "kvm", "xen", "hyper-v", "parallels", "bochs" };
            foreach (var f in flags) if (vendor.Contains(f)) return $"BIOS: {f}";
        }
        catch { }
        return null;
    }
}
