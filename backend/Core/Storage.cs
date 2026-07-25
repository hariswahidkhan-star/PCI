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
///     S3_BUCKET is missing, production refuses to boot (EXT-P1-09); non-production logs an error and
///     does not silently treat incomplete S3 as a working local backend when STORAGE_PROVIDER=s3.
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
    /// <summary>True when the effective backend is local disk. In production, selecting S3 without a
    /// complete config is a hard boot failure (EXT-P1-09) — never silently fall back.</summary>
    public static bool UsingLocal => !S3Configured && (Provider == "local" || (!KnownProviders.Contains(Provider) && Provider != "s3"));
    /// <summary>True when STORAGE_PROVIDER=s3 but required settings are incomplete.</summary>
    public static bool S3Misconfigured => Provider == "s3" && !S3Configured;
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
        // Content-address on the PLAINTEXT (dedup + integrity) but persist ciphertext: every artefact is
        // envelope-encrypted at rest (AES-256-GCM) so a stolen disk/bucket/backup exposes no raw PII.
        var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var ext = AllowedMime.TryGetValue(mime, out var e) ? e : ".bin";
        var shard = sha[..2];
        var rel = $"{Sanitise(category)}/{shard}/{sha}{ext}";
        var enc = Security.EncryptBytes(bytes);
        if (S3Misconfigured)
            throw new InvalidOperationException("STORAGE_PROVIDER=s3 but S3_BUCKET is not set — refusing local-disk fallback (EXT-P1-09).");
        if (S3Configured)
        {
            var put = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = S3Bucket, Key = rel, ContentType = "application/octet-stream",
                InputStream = new MemoryStream(enc),
                // App-level envelope encryption is the portable guarantee; SSE adds provider-side cover too.
                ServerSideEncryptionMethod = Amazon.S3.ServerSideEncryptionMethod.AES256,
            };
            S3Client().PutObjectAsync(put).GetAwaiter().GetResult(); // content-addressed key → re-put is a no-op overwrite
            return new StoredObject($"s3:{rel}", mime, bytes.LongLength, sha);
        }
        var full = Path.Combine(Root, rel);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        // Content addressing makes an existing file a dedupe hit — but ONLY if we can still read it
        // back. Artefacts are persisted as ciphertext, and the key can legitimately change: an
        // explicit CREDENTIAL_ENCRYPTION_KEY set for the first time, a key rotation, or a move
        // between database providers (the derived fallback key mixes in DATABASE_FILE and
        // MYSQL_DATABASE). After any of those, every existing file is undecryptable.
        //
        // A blind "it exists, skip the write" made that damage PERMANENT: re-uploading byte-identical
        // content would take the same content-addressed path, skip the write again, and the store
        // would serve file_missing for ever with no signal and no way to repair it short of deleting
        // files by hand. Verifying instead of assuming costs one read on the dedupe path — which was
        // skipping a write anyway — and makes the store self-healing.
        if (!File.Exists(full) || !Readable(full)) File.WriteAllBytes(full, enc);
        return new StoredObject($"local:{rel}", mime, bytes.LongLength, sha);
    }

    /// <summary>True when a stored artefact can actually be decrypted with the CURRENT key. Used to
    /// decide whether an existing content-addressed file is a genuine dedupe hit or a stale one left
    /// behind by a key change.</summary>
    static bool Readable(string full)
    {
        try { return Security.DecryptBytes(File.ReadAllBytes(full)).Length > 0; }
        catch { return false; }
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
                return (Security.DecryptBytes(ms.ToArray()), mime);   // envelope-decrypt (passes plaintext through)
            }
            catch { return null; }
        }
        var full = Path.Combine(Root, rel);
        if (!File.Exists(full)) return null;
        try { return (Security.DecryptBytes(File.ReadAllBytes(full)), mime); } catch { return null; }
    }

    /// <summary>Delete a single stored object by its reference (provider:rel). Used for immediate erasure
    /// of a specific artefact (e.g. an identity document under a GDPR erasure request), independent of the
    /// age-based retention purge. Best-effort; returns true if the object was removed or already gone.</summary>
    public static bool DeleteRef(string? reference)
    {
        if (string.IsNullOrEmpty(reference)) return false;
        var colon = reference.IndexOf(':');
        if (colon < 0) return false;
        var provider = reference[..colon];
        var rel = reference[(colon + 1)..];
        if (rel.Contains("..") || rel.StartsWith('/') || rel.Contains('\\')) return false;   // path-traversal guard
        try
        {
            if (provider == "s3")
            {
                if (string.IsNullOrEmpty(S3Bucket)) return false;
                S3Client().DeleteObjectAsync(S3Bucket, rel).GetAwaiter().GetResult();
                return true;
            }
            var full = Path.Combine(Root, rel);
            if (File.Exists(full)) File.Delete(full);
            return true;
        }
        catch { return false; }
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
                    {
                        if (!ArtefactName.IsMatch(Path.GetFileName(o.Key))) continue;
                        if (ProtectedCategories.Contains(o.Key.Split('/', 2)[0])) continue;
                        if (o.LastModified.ToUniversalTime() < cutoff)
                        { try { s3.DeleteObjectAsync(S3Bucket, o.Key).GetAwaiter().GetResult(); n++; } catch { } }
                    }
                    token = page.IsTruncated ? page.NextContinuationToken : null;
                } while (token is not null);
            }
            catch (Exception e) { Console.Error.WriteLine($"[storage] s3 purge failed: {e.Message}"); }
            return n;
        }
        if (!Directory.Exists(Root)) return 0;
        var rootFull = Path.GetFullPath(Root);
        foreach (var f in Directory.EnumerateFiles(Root, "*", SearchOption.AllDirectories))
        {
            // Only delete files this app wrote — content-addressed names are "<64-hex-sha><ext>".
            // Skipping anything else means a stray operator file dropped under STORAGE_ROOT is never
            // purged by the retention sweep (STORAGE_ROOT should still be a dedicated volume).
            if (!ArtefactName.IsMatch(Path.GetFileName(f))) continue;
            // Retention is for transient EVIDENCE/ATTACHMENTS. Never sweep protected categories:
            // these are permanent artefacts (books/documents cannot be regenerated; certificates are
            // authoritative; partner/founding/honorary uploads are business records, not case evidence).
            // Each lives under its own top-level category folder.
            if (InProtectedCategory(rootFull, f)) continue;
            try { if (File.GetLastWriteTimeUtc(f) < cutoff) { File.Delete(f); n++; } } catch { }
        }
        return n;
    }

    static readonly System.Text.RegularExpressions.Regex ArtefactName =
        new(@"^[0-9a-f]{64}\.(jpg|png|webp|pdf|bin|docx|doc|xlsx|xls|pptx|ppt|csv|txt|zip)$", System.Text.RegularExpressions.RegexOptions.Compiled);

    // Top-level category folders whose contents the retention sweep must never delete (both backends).
    // Purgeable categories stay implicit: evidence, idd, appeal, accommodation, support, cpd,
    // incidents — all candidate-submitted case/identity evidence, which is what
    // evidence_retention_days governs. public-docs holds PUBLISHED governance/legal PDFs served to
    // the anonymous public and cv holds job-applicant CVs under an open posting — neither is case
    // evidence, so neither may silently age out.
    static readonly HashSet<string> ProtectedCategories = new(StringComparer.OrdinalIgnoreCase)
        { "documents", "certificates", "books", "founding", "honorary", "honorary-idv", "partners", "public-docs", "cv" };

    /// <summary>Delete a SINGLE stored object by reference (used for targeted, policy-driven deletion of
    /// sensitive artefacts such as honorary identity documents — not the blanket retention sweep). Returns
    /// true if the object was removed (or already absent).</summary>
    public static bool DeleteOne(string? reference)
    {
        if (string.IsNullOrEmpty(reference)) return false;
        var colon = reference.IndexOf(':');
        if (colon < 0) return false;
        var provider = reference[..colon];
        var rel = reference[(colon + 1)..];
        if (rel.Contains("..") || rel.StartsWith('/') || rel.Contains('\\')) return false;
        if (provider == "s3")
        {
            if (string.IsNullOrEmpty(S3Bucket)) return false;
            try { S3Client().DeleteObjectAsync(S3Bucket, rel).GetAwaiter().GetResult(); return true; } catch { return false; }
        }
        var full = Path.Combine(Root, rel);
        try { if (File.Exists(full)) File.Delete(full); return true; } catch { return false; }
    }
    static bool InProtectedCategory(string rootFull, string file)
    {
        try
        {
            var rel = Path.GetRelativePath(rootFull, Path.GetFullPath(file));
            var top = rel.Replace('\\', '/').Split('/', 2)[0];
            return ProtectedCategories.Contains(top);
        }
        catch { return false; }
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
