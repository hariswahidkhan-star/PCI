using System.Runtime.InteropServices;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Best-effort screen-capture resistance. SetWindowDisplayAffinity(WDA_EXCLUDEFROMCAPTURE) asks the
/// compositor to exclude the exam window from screenshots and screen recordings on Windows 10 2004+.
/// HONEST LIMIT: this is a request the OS honours for cooperative capture paths; it is not an absolute
/// guarantee against a camera pointed at the screen or a capture card. Stated plainly to candidates.
/// </summary>
public static class CaptureShield
{
    private const uint WDA_NONE = 0x0, WDA_EXCLUDEFROMCAPTURE = 0x11;
    [DllImport("user32.dll", SetLastError = true)] private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

    public static bool Protect(IntPtr hwnd) => SetWindowDisplayAffinity(hwnd, WDA_EXCLUDEFROMCAPTURE);
    public static void Release(IntPtr hwnd) => SetWindowDisplayAffinity(hwnd, WDA_NONE);
}
