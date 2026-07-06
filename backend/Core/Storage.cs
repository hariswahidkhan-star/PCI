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
///   • "s3" — any S3-compatible object store. Requires S3_BUCKET; optional S3_ENDPOINT (for MinIO/R2/…,
///     forces path-style addressing), S3_REGION (default us-east-1). Credentials via the standard AWS env
///     vars (AWS_ACCESS_KEY_ID / AWS_SECRET_ACCESS_KEY) or the SDK's default chain. If S3 is selected but
///     S3_BUCKET is missing, the app falls back to local with a startup warning (never silently drops data).
///
/// A stored reference looks like:  local:ev/ab/abcd…ef.jpg  or  s3:ev/ab/abcd…ef.jpg
/// (provider:relativePath). Get() routes by the reference prefix, so a deployment can migrate from local
/// to s3 without breaking old references. Nothing about a reference leaks an absolute path to clients.
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

    // ---- S3 configuration (env-gated seam, now wired) ----
    static string? S3Bucket => Environment.GetEnvironmentVariable("S3_BUCKET");
    static bool S3Configured => Provider == "s3" && !string.IsNullOrEmpty(S3Bucket);
    public static bool UsingLocal => !S3Configured && (Provider == "local" || !KnownProviders.Contains(Provider) || Provider == "s3");
    static readonly HashSet<string> KnownProviders = new() { "local", "s3" };

    static Amazon.S3.IAmazonS3? _s3;
    static Amazon.S3.IAmazonS3 S3Client()
    {
        if (_s3 is not null) return _s3;
        var cfg = new Amazon.S3.AmazonS3Config
        {
            AuthenticationRegion = Environment.GetEnvironmentVariable("S3_REGION") ?? "us-east-1",
        };
        var endpoint = Environment.GetEnvironmentVariable("S3_ENDPOINT");
        if (!string.IsNullOrEmpty(endpoint)) { cfg.ServiceURL = endpoint; cfg.ForcePathStyle = true; }
        else cfg.RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("S3_REGION") ?? "us-east-1");
        _s3 = new Amazon.S3.AmazonS3Client(cfg); // credentials from the SDK default chain (env vars, profile, role)
        return _s3;
    }

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
        if (S3Configured)
        {
            var put = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = S3Bucket, Key = rel, ContentType = mime,
                InputStream = new MemoryStream(bytes),
            };
            S3Client().PutObjectAsync(put).GetAwaiter().GetResult(); // content-addressed key → re-put is a no-op overwrite
            return new StoredObject($"s3:{rel}", mime, bytes.LongLength, sha);
        }
        var full = Path.Combine(Root, rel);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        if (!File.Exists(full)) File.WriteAllBytes(full, bytes); // content-addressed → dedupe for free
        return new StoredObject($"local:{rel}", mime, bytes.LongLength, sha);
    }

    /// <summary>Read bytes back for a reference (used by the authenticated serve endpoints). Null if missing.
    /// Routes by the reference prefix (local: vs s3:) so mixed references keep working after a migration.</summary>
    public static (byte[]? bytes, string mime)? Get(string? reference)
    {
        if (string.IsNullOrEmpty(reference)) return null;
        var colon = reference.IndexOf(':');
        if (colon < 0) return null;
        var provider = reference[..colon];
        var rel = reference[(colon + 1)..];
        // hard path/key-traversal guard
        if (rel.Contains("..") || rel.StartsWith('/') || rel.Contains('\\')) return null;
        var ext = Path.GetExtension(rel).ToLowerInvariant();
        var mime = AllowedMime.FirstOrDefault(kv => kv.Value == ext).Key ?? "application/octet-stream";
        if (provider == "s3")
        {
            if (string.IsNullOrEmpty(S3Bucket)) return null;
            try
            {
                using var resp = S3Client().GetObjectAsync(S3Bucket, rel).GetAwaiter().GetResult();
                using var ms = new MemoryStream();
                resp.ResponseStream.CopyTo(ms);
                return (ms.ToArray(), mime);
            }
            catch { return null; }
        }
        var full = Path.Combine(Root, rel);
        if (!File.Exists(full)) return null;
        try { return (File.ReadAllBytes(full), mime); } catch { return null; }
    }

    /// <summary>Delete artefacts older than the retention window (days). Returns count removed.
    /// Covers whichever backend is active: local files by mtime, S3 objects by LastModified.</summary>
    public static int PurgeOlderThan(int days)
    {
        var cutoff = DateTime.UtcNow.AddDays(-days);
        int n = 0;
        if (S3Configured)
        {
            try
            {
                var s3 = S3Client();
                string? token = null;
                do
                {
                    var page = s3.ListObjectsV2Async(new Amazon.S3.Model.ListObjectsV2Request
                    { BucketName = S3Bucket, ContinuationToken = token }).GetAwaiter().GetResult();
                    foreach (var o in page.S3Objects)
                        if (o.LastModified.ToUniversalTime() < cutoff)
                        { try { s3.DeleteObjectAsync(S3Bucket, o.Key).GetAwaiter().GetResult(); n++; } catch { } }
                    token = page.IsTruncated ? page.NextContinuationToken : null;
                } while (token is not null);
            }
            catch (Exception e) { Console.Error.WriteLine($"[storage] s3 purge failed: {e.Message}"); }
            return n;
        }
        if (!Directory.Exists(Root)) return 0;
        foreach (var f in Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories))
        {
            // Only delete files this app wrote — content-addressed names are "<64-hex-sha><ext>".
            // Skipping anything else means a stray operator file dropped under STORAGE_ROOT is never
            // purged by the retention sweep (STORAGE_ROOT should still be a dedicated volume).
            if (!ArtefactName.IsMatch(Path.GetFileName(f))) continue;
            try { if (File.GetLastWriteTimeUtc(f) < cutoff) { File.Delete(f); n++; } } catch { }
        }
        return n;
    }

    static readonly System.Text.RegularExpressions.Regex ArtefactName =
        new(@"^[0-9a-f]{64}\.(jpg|png|webp|pdf|bin)$", System.Text.RegularExpressions.RegexOptions.Compiled);

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
