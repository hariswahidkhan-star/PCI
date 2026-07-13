using System.Security.Cryptography;
using System.Text;

namespace PCI.Backend.Core;

public static class Security
{
    public static string Sha(string s)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(s));
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    public static string RandomHex(int nbytes)
    {
        var buf = RandomNumberGenerator.GetBytes(nbytes);
        var sb = new StringBuilder(buf.Length * 2);
        foreach (var b in buf) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }

    /// <summary>Constant-behaviour password check: false for null/malformed stored hashes instead of
    /// throwing. BCrypt.Verify raises SaltParseException on a non-bcrypt hash value, which turned the
    /// public login endpoints into a 500 for any row with a legacy/corrupted hash — an auth endpoint
    /// must answer 401, never crash, on bad stored data.</summary>
    public static bool VerifyPassword(string password, string? storedHash)
    {
        if (string.IsNullOrEmpty(storedHash)) return false;
        try { return BCrypt.Net.BCrypt.Verify(password, storedHash); }
        catch { return false; }
    }
}

/// <summary>RBAC: section map, role grants, permsFor — ported verbatim from the Node backend.</summary>
public static class Rbac
{
    public static readonly Dictionary<string, string[]> Sections = new()
    {
        ["platform"] = new[]{ "overview","reports","audit","emails","settings","team" },
        ["website"]  = new[]{ "set_web","pricing","codes","content","pages","news","faqs","bok","governance","resources","media","nav","partners","sitesettings","subscribers","submissions","inquiries" },
        ["student"]  = new[]{ "set_sp","members","enrollments","payments","credentials","tickets" },
        ["exam"]     = new[]{ "set_exam","exams","proctoring","sampleq" },
    };

    public static string[] AllSections => Sections.Values.SelectMany(x => x).ToArray();

    public static readonly Dictionary<string, string[]> RoleGrants = new()
    {
        ["owner"]           = Sections.Values.SelectMany(x => x).ToArray(),
        ["website_manager"] = Sections["website"].Append("overview").ToArray(),
        ["student_manager"] = Sections["student"].Append("overview").ToArray(),
        ["exam_manager"]    = Sections["exam"].Append("overview").ToArray(),
        ["viewer"]          = new[]{ "overview","reports" },
    };

    public static List<string> PermsFor(string role, string? permissionsJson)
    {
        if (role == "owner") return RoleGrants["owner"].ToList();
        var baseGrants = RoleGrants.TryGetValue(role, out var g) ? g : Array.Empty<string>();
        var extra = new List<string>();
        try { extra = System.Text.Json.JsonSerializer.Deserialize<List<string>>(permissionsJson ?? "[]") ?? new(); }
        catch { }
        var set = new HashSet<string>();
        if (role != "custom") foreach (var s in baseGrants) set.Add(s);
        foreach (var s in extra) set.Add(s);
        return set.ToList();
    }
}
