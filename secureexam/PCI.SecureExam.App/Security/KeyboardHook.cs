using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace PCI.SecureExam.App.Security;

/// <summary>
/// Low-level keyboard hook (WH_KEYBOARD_LL) that suppresses task-switching and escape hotkeys
/// during the exam: Alt+Tab, Alt+Esc, Ctrl+Esc, the Windows keys, Alt+F4 and PrintScreen.
/// HONEST LIMIT: Ctrl+Alt+Del (the Secure Attention Sequence) is handled by Winlogon in kernel
/// space and CANNOT be intercepted from a user-mode application by design. That is enforced by
/// Windows itself; any product claiming to block it in user space is misrepresenting what it does.
/// </summary>
public sealed class KeyboardHook : IDisposable
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100, WM_SYSKEYDOWN = 0x0104;
    private readonly LowLevelKeyboardProc _proc;
    private IntPtr _hook = IntPtr.Zero;
    public event Action<string>? HotkeyBlocked;

    public KeyboardHook() => _proc = HookCallback;

    public void Install()
    {
        using var cur = Process.GetCurrentProcess();
        using var mod = cur.MainModule!;
        _hook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(mod.ModuleName), 0);
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
        {
            int vk = Marshal.ReadInt32(lParam);
            var key = KeyInterop.KeyFromVirtualKey(vk);
            bool alt = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;
            bool ctrl = (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            bool block =
                key is Key.LWin or Key.RWin ||
                (alt && key == Key.Tab) ||
                (alt && key == Key.Escape) ||
                (ctrl && key == Key.Escape) ||
                (alt && key == Key.F4) ||
                key == Key.Snapshot ||               // PrintScreen
                key is Key.BrowserBack or Key.BrowserForward;

            if (block)
            {
                HotkeyBlocked?.Invoke($"{(ctrl ? "Ctrl+" : "")}{(alt ? "Alt+" : "")}{key}");
                return (IntPtr)1; // swallow
            }
        }
        return CallNextHookEx(_hook, nCode, wParam, lParam);
    }

    public void Dispose() { if (_hook != IntPtr.Zero) { UnhookWindowsHookEx(_hook); _hook = IntPtr.Zero; } }

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)] private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
    [DllImport("user32.dll", SetLastError = true)] private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    [DllImport("user32.dll", SetLastError = true)] private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr GetModuleHandle(string lpModuleName);
}
