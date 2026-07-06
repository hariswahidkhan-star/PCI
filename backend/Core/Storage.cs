using System.Security.Cryptography;

namespace PCI.Backend.Core;

/// <summary>
/// Storage abstraction for binary artefacts (exam evidence, support/appeal/accommodation/CPD attachments).
/// Bytes are written to a configurable backend and the database stores only <b>metadata + a reference</b> —
/// this keeps SQLite small instead of embedding multi-megabyte data URIs inline.
///
/// Backends (STORAGE_PROVIDER):
///   • "local" (default) — writes under STORAGE_ROOT (default ./storage), sharded by the first 2 chars of the
///     object key. Suitable for development and single-node deployments with a persistent volume.
///   • "s3" / object storage — intentionally a documented seam: the reference format and the read/write API
///     are provider-agnostic, so a production deployment can implement PutObject/GetObject without touching
///     any call site. Until wired, selecting a non-local provider falls back to local with a startup warning.
///
/// A stored reference looks like:  local:ev/ab/abcd…ef.jpg   (provider:relativePath)
/// Nothing about the reference leaks a filesystem absolute path to clients.
/// </summary>
public static class Storage
{
    public const long MaxBytes = 3_000_000; // 3 MB per artefact (decoded)
    public static readonly Dictionary<string, string> AllowedMime = new()
    {
        ["image/jpeg"] = ".jpg", ["image/png"] = ".png", ["image/webp"] = ".webp",
        ["application/pdf"] = ".pdf",
    };

    static string Provider => (Environment.GetEnvironmentVariable("STORAGE_PROVIDER") ?? "local").ToLowerInvariant();
    static string Root => Environment.GetEnvironmentVariable("STORAGE_ROOT") ?? "./storage";

    public static bool UsingLocal => Provider == "local" || !KnownProviders.Contains(Provider);
    static readonly HashSet<string> KnownProviders = new() { "local" };

    public record StoredObject(string Reference, string Mime, long SizeBytes, string Sha256);

    /// <summary>Validate + decode a data URI. Returns null bytes and an error string when invalid.</summary>
    public static (byte[]? bytes, string mime, string? error) DecodeDataUri(string? dataUri)
    {
        if (string.IsNullOrEmpty(dataUri)) return (null, "", "no_file");
        if (!dataUri.StartsWith("data:", StringComparison.Ordinal)) return (null, "", "not_a_data_uri");
        var comma = dataUri.IndexOf(',');
        var semi = dataUri.IndexOf(';');
        if (comma < 0 || semi < 0 || semi > comma) return (null, "", "malformed_data_uri");
        var mime = dataUri[5..semi];
        if (!AllowedMime.ContainsKey(mime)) return (null, mime, "file_type_not_allowed");
        // rough decoded-size guard before allocating (base64 ≈ 4/3 of bytes)
        if ((long)(dataUri.Length - comma) * 3 / 4 > MaxBytes + 1024) return (null, mime, "file_too_large");
        byte[] bytes;
        try { bytes = Convert.FromBase64String(dataUri[(comma + 1)..]); }
        catch { return (null, mime, "invalid_base64"); }
        if (bytes.LongLength > MaxBytes) return (null, mime, "file_too_large");
        if (!SniffMatches(bytes, mime)) return (null, mime, "content_mime_mismatch");
        return (bytes, mime, null);
    }

    /// <summary>Persist bytes and return a provider-qualified reference + metadata. Throws only on IO error.</summary>
    public static StoredObject Put(byte[] bytes, string mime, string category)
    {
        var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var ext = AllowedMime.TryGetValue(mime, out var e) ? e : ".bin";
        var shard = sha[..2];
        var rel = $"{Sanitise(category)}/{shard}/{sha}{ext}";
        if (UsingLocal)
        {
            var full = Path.Combine(Root, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(full)!);
            if (!File.Exists(full)) File.WriteAllBytes(full, bytes); // content-addressed → dedupe for free
            return new StoredObject($"local:{rel}", mime, bytes.LongLength, sha);
        }
        // Non-local providers not yet wired: fall back to local so nothing breaks, but be explicit.
        var f2 = Path.Combine(Root, rel);
        Directory.CreateDirectory(Path.GetDirectoryName(f2)!);
        if (!File.Exists(f2)) File.WriteAllBytes(f2, bytes);
        return new StoredObject($"local:{rel}", mime, bytes.LongLength, sha);
    }

    /// <summary>Read bytes back for a reference (used by the authenticated serve endpoint). Null if missing.</summary>
    public static (byte[]? bytes, string mime)? Get(string? reference)
    {
        if (string.IsNullOrEmpty(reference)) return null;
        var colon = reference.IndexOf(':');
        if (colon < 0) return null;
        var rel = reference[(colon + 1)..];
        // hard path-traversal guard
        if (rel.Contains("..") || rel.StartsWith('/') || rel.Contains('\\')) return null;
        var full = Path.Combine(Root, rel);
        if (!File.Exists(full)) return null;
        var ext = Path.GetExtension(full).ToLowerInvariant();
        var mime = AllowedMime.FirstOrDefault(kv => kv.Value == ext).Key ?? "application/octet-stream";
        try { return (File.ReadAllBytes(full), mime); } catch { return null; }
    }

    /// <summary>Delete artefacts older than the retention window (days). Returns count removed.
    /// Safe no-op for non-local providers until wired.</summary>
    public static int PurgeOlderThan(int days)
    {
        if (!UsingLocal || !Directory.Exists(Root)) return 0;
        var cutoff = DateTime.UtcNow.AddDays(-days);
        int n = 0;
        foreach (var f in Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories))
        {
            try { if (File.GetLastWriteTimeUtc(f) < cutoff) { File.Delete(f); n++; } } catch { }
        }
        return n;
    }

    // Minimal magic-byte sniff so a mislabeled/renamed payload is rejected (defence in depth vs. the header).
    static bool SniffMatches(byte[] b, string mime)
    {
        if (b.Length < 4) return false;
        return mime switch
        {
            "image/jpeg" => b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF,
            "image/png" => b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47,
            "image/webp" => b.Length > 12 && b[0] == (byte)'R' && b[1] == (byte)'I' && b[2] == (byte)'F' && b[3] == (byte)'F'
                             && b[8] == (byte)'W' && b[9] == (byte)'E' && b[10] == (byte)'B' && b[11] == (byte)'P',
            "application/pdf" => b[0] == 0x25 && b[1] == 0x50 && b[2] == 0x44 && b[3] == 0x46, // %PDF
            _ => false,
        };
    }

    static string Sanitise(string s) => new string(s.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_').ToArray());
}
