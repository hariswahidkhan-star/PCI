using Microsoft.Win32;

namespace PCI.SecureExam.App.Infrastructure;

/// <summary>
/// Registers the pciexam:// scheme for the current user so the portal's "Launch examination" button
/// opens this client. Runs on startup (idempotent) — no installer or admin rights required. An MSI/MSIX
/// installer would additionally register it machine-wide.
/// </summary>
public static class UriSchemeRegistrar
{
    private const string Scheme = "pciexam";

    public static void EnsureRegistered()
    {
        if (!OperatingSystem.IsWindows()) return;
        try
        {
            var exe = Environment.ProcessPath ?? System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
            if (string.IsNullOrEmpty(exe)) return;

            using var root = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{Scheme}");
            var existing = root.GetValue("URL Protocol");
            root.SetValue("", "URL:PCI Secure Exam");
            root.SetValue("URL Protocol", "");
            using var cmd = root.CreateSubKey(@"shell\open\command");
            cmd.SetValue("", $"\"{exe}\" \"%1\"");
        }
        catch { /* non-fatal: the launch link simply won't auto-open until the installer registers it */ }
    }
}
