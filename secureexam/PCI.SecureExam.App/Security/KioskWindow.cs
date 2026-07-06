using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Helpers to lock a WPF Window into kiosk mode: borderless, top-most, covering the full virtual
/// screen, with the system menu and close affordances disabled. Applied to the exam window while
/// a sitting is in progress; released on submission.
/// </summary>
public static class KioskWindow
{
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x00080000, WS_MAXIMIZEBOX = 0x00010000, WS_MINIMIZEBOX = 0x00020000;

    public static void Enter(Window w)
    {
        w.WindowStyle = WindowStyle.None;
        w.ResizeMode = ResizeMode.NoResize;
        w.Topmost = true;
        w.WindowState = WindowState.Normal;
        w.Left = SystemParameters.VirtualScreenLeft;
        w.Top = SystemParameters.VirtualScreenTop;
        w.Width = SystemParameters.VirtualScreenWidth;
        w.Height = SystemParameters.VirtualScreenHeight;

        var hwnd = new WindowInteropHelper(w).Handle;
        int style = GetWindowLong(hwnd, GWL_STYLE);
        SetWindowLong(hwnd, GWL_STYLE, style & ~WS_SYSMENU & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX);
        CaptureShield.Protect(hwnd);
    }

    public static void Exit(Window w)
    {
        var hwnd = new WindowInteropHelper(w).Handle;
        CaptureShield.Release(hwnd);
        w.Topmost = false;
        w.WindowStyle = WindowStyle.SingleBorderWindow;
        w.ResizeMode = ResizeMode.CanResize;
    }

    [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
}
