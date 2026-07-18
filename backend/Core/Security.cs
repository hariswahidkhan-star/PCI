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

    /// <summary>Constant-time secret comparison (webhook shared secrets etc.).</summary>
    public static bool FixedTimeEquals(string? a, string? b)
    {
        if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));
    }

    // ---- Temporary-credential generation ----
    // Unambiguous alphabets (no 0/O, 1/l/I) so a student can retype a temp password without confusion.
    const string PwLower = "abcdefghijkmnpqrstuvwxyz";
    const string PwUpper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    const string PwDigit = "23456789";
    const string PwSymbol = "!@#$%^&*?-_+=";

    /// <summary>A cryptographically-secure temporary password with guaranteed complexity: at least one
    /// lower, upper, digit and symbol, the remainder drawn uniformly from the full set, then shuffled.
    /// Length is clamped to a sane range. Used for PCI-generated Certuvo credentials.</summary>
    public static string GenPassword(int length)
    {
        length = Math.Clamp(length, 10, 64);
        var all = PwLower + PwUpper + PwDigit + PwSymbol;
        var chars = new char[length];
        chars[0] = Pick(PwLower); chars[1] = Pick(PwUpper); chars[2] = Pick(PwDigit); chars[3] = Pick(PwSymbol);
        for (var i = 4; i < length; i++) chars[i] = Pick(all);
        // Fisher–Yates with a CSPRNG so the guaranteed positions are not always first.
        for (var i = length - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }
        return new string(chars);
        static char Pick(string set) => set[RandomNumberGenerator.GetInt32(set.Length)];
    }

    // ---- Reversible encryption for credentials that must be displayed back (AES-256-GCM) ----
    // A student must be able to READ their temporary Certuvo password, so it cannot be one-way hashed;
    // it is instead encrypted at rest with an authenticated cipher. The key comes from
    // CREDENTIAL_ENCRYPTION_KEY (base64 or hex, 32 bytes) in production; absent that a per-install key is
    // derived deterministically from available app secrets so dev/test still round-trips.
    const string EncPrefix = "enc:v1:";
    static readonly byte[] _credKey = ResolveCredKey();

    static byte[] ResolveCredKey()
    {
        var raw = Environment.GetEnvironmentVariable("CREDENTIAL_ENCRYPTION_KEY");
        if (!string.IsNullOrWhiteSpace(raw))
        {
            try { var b = Convert.FromBase64String(raw.Trim()); if (b.Length == 32) return b; } catch { }
            try { if (raw.Trim().Length == 64) return Convert.FromHexString(raw.Trim()); } catch { }
            // Any other non-empty value: derive a 32-byte key from it.
            return SHA256.HashData(Encoding.UTF8.GetBytes(raw.Trim()));
        }
        // No dedicated key: derive one from other configured secrets so the value is stable across
        // restarts on the same install. Production should set CREDENTIAL_ENCRYPTION_KEY explicitly.
        var seed = (Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET") ?? "")
                 + "|" + (Environment.GetEnvironmentVariable("DATABASE_FILE") ?? "")
                 + "|" + (Environment.GetEnvironmentVariable("MYSQL_DATABASE") ?? "")
                 + "|pci-certuvo-credential-key-v1";
        return SHA256.HashData(Encoding.UTF8.GetBytes(seed));
    }

    /// <summary>Encrypt a value that must later be read back (e.g. a temp password). Returns an
    /// "enc:v1:" tagged token; a null/empty input passes through unchanged.</summary>
    public static string? EncryptSecret(string? plaintext)
    {
        if (string.IsNullOrEmpty(plaintext)) return plaintext;
        var nonce = RandomNumberGenerator.GetBytes(12);
        var pt = Encoding.UTF8.GetBytes(plaintext);
        var ct = new byte[pt.Length];
        var tag = new byte[16];
        using var gcm = new AesGcm(_credKey, 16);
        gcm.Encrypt(nonce, pt, ct, tag);
        var blob = new byte[nonce.Length + ct.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, blob, 0, 12);
        Buffer.BlockCopy(ct, 0, blob, 12, ct.Length);
        Buffer.BlockCopy(tag, 0, blob, 12 + ct.Length, 16);
        return EncPrefix + Convert.ToBase64String(blob);
    }

    /// <summary>Decrypt an "enc:v1:" token produced by <see cref="EncryptSecret"/>. Values without the
    /// tag are returned as-is (legacy/plaintext tolerance); a tampered/undecryptable token returns null.</summary>
    public static string? DecryptSecret(string? stored)
    {
        if (string.IsNullOrEmpty(stored)) return stored;
        if (!stored.StartsWith(EncPrefix, StringComparison.Ordinal)) return stored;
        try
        {
            var blob = Convert.FromBase64String(stored[EncPrefix.Length..]);
            if (blob.Length < 12 + 16) return null;
            var nonce = blob[..12];
            var tag = blob[^16..];
            var ct = blob[12..^16];
            var pt = new byte[ct.Length];
            using var gcm = new AesGcm(_credKey, 16);
            gcm.Decrypt(nonce, ct, tag, pt);
            return Encoding.UTF8.GetString(pt);
        }
        catch { return null; }
    }

    // ---- TOTP (RFC 6238, SHA-1, 6 digits, 30 s) — optional MFA for privileged admin accounts ----
    static readonly char[] B32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

    public static string NewTotpSecret()
    {
        var bytes = RandomNumberGenerator.GetBytes(20);
        var sb = new StringBuilder();
        int bits = 0, value = 0;
        foreach (var b in bytes)
        {
            value = (value << 8) | b; bits += 8;
            while (bits >= 5) { sb.Append(B32[(value >> (bits - 5)) & 31]); bits -= 5; }
        }
        if (bits > 0) sb.Append(B32[(value << (5 - bits)) & 31]);
        return sb.ToString();
    }

    static byte[] B32Decode(string s)
    {
        var clean = s.Trim().TrimEnd('=').ToUpperInvariant();
        var bytes = new List<byte>();
        int bits = 0, value = 0;
        foreach (var c in clean)
        {
            var idx = Array.IndexOf(B32, c);
            if (idx < 0) continue;
            value = (value << 5) | idx; bits += 5;
            if (bits >= 8) { bytes.Add((byte)((value >> (bits - 8)) & 0xFF)); bits -= 8; }
        }
        return bytes.ToArray();
    }

    /// <summary>Verify a 6-digit TOTP code, accepting a ±1 window step for clock skew.</summary>
    public static bool VerifyTotp(string secret, string? code)
    {
        if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(code) || code.Trim().Length != 6) return false;
        var key = B32Decode(secret);
        if (key.Length == 0) return false;
        var step = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;
        for (var offset = -1; offset <= 1; offset++)
        {
            var counter = BitConverter.GetBytes(step + offset);
            if (BitConverter.IsLittleEndian) Array.Reverse(counter);
            using var hmac = new HMACSHA1(key);
            var hash = hmac.ComputeHash(counter);
            var pos = hash[^1] & 0x0F;
            var truncated = ((hash[pos] & 0x7F) << 24) | (hash[pos + 1] << 16) | (hash[pos + 2] << 8) | hash[pos + 3];
            if ((truncated % 1_000_000).ToString("D6") == code.Trim()) return true;
        }
        return false;
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
        ["platform"] = new[]{ "overview","reports","audit","emails","settings","team","integrations" },
        ["website"]  = new[]{ "set_web","pricing","codes","content","pages","news","faqs","bok","governance","resources","media","nav","partners","sitesettings","subscribers","submissions","inquiries" },
        ["student"]  = new[]{ "set_sp","members","enrollments","payments","credentials","tickets" },
        ["exam"]     = new[]{ "set_exam","exams","proctoring","sampleq","exam_delivery" },
        // High-privilege operator capabilities. Deliberately NOT part of any named role bundle
        // (owner excepted): they are granted individually via a custom role or a permissions
        // override, so "who can move money / act as a student / mint test accounts" is always an
        // explicit decision, never a side-effect of a job title.
        ["operations"] = new[]{ "finance","impersonate","test_users" },
        // Customer-service portal: 'inbox' = work the unified queue (chats, tickets, enquiries,
        // error references); 'support_admin' = manage templates, SLA targets and assignment rules.
        ["support"] = new[]{ "inbox","support_admin" },
    };

    public static string[] AllSections => Sections.Values.SelectMany(x => x).ToArray();

    public static readonly Dictionary<string, string[]> RoleGrants = new()
    {
        ["owner"]           = Sections.Values.SelectMany(x => x).ToArray(),
        ["website_manager"] = Sections["website"].Append("overview").ToArray(),
        ["student_manager"] = Sections["student"].Append("overview").ToArray(),
        ["exam_manager"]    = Sections["exam"].Append("overview").ToArray(),
        ["viewer"]          = new[]{ "overview","reports" },
        // Customer-service roles: agents work the queue and see member context; supervisors also
        // manage templates/SLA and can read reports. Neither gets finance/impersonate/test_users —
        // those stay explicit per-person grants.
        ["support_agent"]      = new[]{ "overview","inbox","tickets","members" },
        ["support_supervisor"] = new[]{ "overview","inbox","support_admin","tickets","members","reports" },
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
