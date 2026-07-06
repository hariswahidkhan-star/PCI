using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PCI.SecureExam.App.Exam;

/// <summary>
/// Encrypted local fallback for answers + violation counts, so a hard crash or power loss before the
/// next server heartbeat never loses work. Uses DPAPI (per-user) so the cache is unreadable by other
/// users/processes. The server remains the source of truth for the clock and final scoring.
/// </summary>
public sealed class SecureStore
{
    private readonly string _path;
    public SecureStore(string attemptToken)
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PCISecureExam");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, $"attempt_{Sha(attemptToken)}.bin");
    }

    public void Save(object state)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(state);
        var enc = ProtectedData.Protect(json, null, DataProtectionScope.CurrentUser);
        File.WriteAllBytes(_path, enc);
    }

    public T? Load<T>()
    {
        try
        {
            if (!File.Exists(_path)) return default;
            var dec = ProtectedData.Unprotect(File.ReadAllBytes(_path), null, DataProtectionScope.CurrentUser);
            return JsonSerializer.Deserialize<T>(dec);
        }
        catch { return default; }
    }

    public void Clear() { try { if (File.Exists(_path)) File.Delete(_path); } catch { } }
    private static string Sha(string s) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s)))[..16];
}
