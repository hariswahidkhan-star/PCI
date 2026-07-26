using System.Security.Cryptography;

namespace PCI.Backend.Core;

/// <summary>
/// Secure intake + private storage for admin-uploaded student documents. This is a document-aware layer
/// ON TOP of <see cref="Storage"/> (which keeps its deliberately-narrow evidence/image allow-list): it has
/// its own broader document allow-list, a larger size cap, real magic-byte signature verification, a safe
/// display-name sanitiser, a content hash, and a malware-scan seam. Bytes land in the same content-addressed
/// PRIVATE object store under the "documents" category — which the retention sweep never purges — so a
/// document is never publicly reachable and never silently deleted.
/// </summary>
public static class DocStore
{
    public const long MaxBytes = 26_214_400; // 25 MiB per document (decoded)

    /// <summary>Declared MIME → canonical extension. The eighteen practical document types the module
    /// accepts. Anything not on this list is rejected at intake (never stored).</summary>
    public static readonly Dictionary<string, string> AllowedMime = new(StringComparer.OrdinalIgnoreCase)
    {
        ["application/pdf"] = ".pdf",
        ["application/msword"] = ".doc",
        ["application/vnd.openxmlformats-officedocument.wordprocessingml.document"] = ".docx",
        ["application/vnd.ms-excel"] = ".xls",
        ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"] = ".xlsx",
        ["application/vnd.ms-powerpoint"] = ".ppt",
        ["application/vnd.openxmlformats-officedocument.presentationml.presentation"] = ".pptx",
        ["text/csv"] = ".csv",
        ["text/plain"] = ".txt",
        ["image/png"] = ".png",
        ["image/jpeg"] = ".jpg",
        ["image/webp"] = ".webp",
        ["application/zip"] = ".zip",
    };

    public record Decoded(byte[] Bytes, string Mime, string Ext, string Sha256);

    /// <summary>Validate + decode a document data URI. Enforces the allow-list, the size cap, a magic-byte
    /// signature match, and a malware-scan seam. Returns (null, error) on any rejection — nothing is stored.</summary>
    public static (Decoded? file, string? error) Decode(string? dataUri)
    {
        if (string.IsNullOrEmpty(dataUri)) return (null, "no_file");
        if (!dataUri.StartsWith("data:", StringComparison.Ordinal)) return (null, "not_a_data_uri");
        var comma = dataUri.IndexOf(',');
        var semi = dataUri.IndexOf(';');
        if (comma < 0 || semi < 0 || semi > comma) return (null, "malformed_data_uri");
        var mime = dataUri[5..semi].Trim().ToLowerInvariant();
        if (!AllowedMime.TryGetValue(mime, out var ext)) return (null, "file_type_not_allowed");
        // rough decoded-size guard before allocating (base64 ≈ 4/3 of bytes)
        if ((long)(dataUri.Length - comma) * 3 / 4 > MaxBytes + 1024) return (null, "file_too_large");
        byte[] bytes;
        try { bytes = Convert.FromBase64String(dataUri[(comma + 1)..]); }
        catch { return (null, "invalid_base64"); }
        if (bytes.LongLength == 0) return (null, "empty_file");
        if (bytes.LongLength > MaxBytes) return (null, "file_too_large");
        if (!SniffMatches(bytes, mime)) return (null, "content_mime_mismatch");
        if (!ScanClean(bytes, mime, out var scanReason)) return (null, scanReason ?? "malware_suspected");
        var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        return (new Decoded(bytes, mime, ext, sha), null);
    }

    /// <summary>Persist decoded document bytes privately and return the storage reference.</summary>
    public static Storage.StoredObject Put(Decoded d) => Storage.Put(d.Bytes, d.Mime, "documents");

    /// <summary>Read a stored document back (authenticated serve paths only). Null if missing.</summary>
    public static byte[]? Get(string? reference) => Storage.Get(reference)?.bytes;

    // ---- magic-byte signature verification (defence-in-depth vs. a forged Content-Type header) ----
    static bool SniffMatches(byte[] b, string mime)
    {
        if (b.Length < 4) return false;
        bool Pk() => b.Length > 4 && b[0] == 0x50 && b[1] == 0x4B && (b[2] == 0x03 || b[2] == 0x05 || b[2] == 0x07);
        bool Ole() => b.Length >= 8 && b[0] == 0xD0 && b[1] == 0xCF && b[2] == 0x11 && b[3] == 0xE0 && b[4] == 0xA1 && b[5] == 0xB1 && b[6] == 0x1A && b[7] == 0xE1;
        return mime switch
        {
            "application/pdf" => b[0] == 0x25 && b[1] == 0x50 && b[2] == 0x44 && b[3] == 0x46, // %PDF
            // OOXML formats and plain zips are all ZIP containers (PK). We can confirm "it is a real zip
            // container", which is the honest strongest signature check for these — a renamed .exe fails.
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => Pk(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => Pk(),
            "application/vnd.openxmlformats-officedocument.presentationml.presentation" => Pk(),
            "application/zip" => Pk(),
            // Legacy Office binary formats share the OLE compound-file header.
            "application/msword" => Ole(),
            "application/vnd.ms-excel" => Ole(),
            "application/vnd.ms-powerpoint" => Ole(),
            "image/png" => b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47,
            "image/jpeg" => b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF,
            "image/webp" => b.Length > 12 && b[0] == (byte)'R' && b[1] == (byte)'I' && b[2] == (byte)'F' && b[3] == (byte)'F'
                             && b[8] == (byte)'W' && b[9] == (byte)'E' && b[10] == (byte)'B' && b[11] == (byte)'P',
            // Text formats have no signature; reject if the sampled prefix looks binary (contains a NUL).
            "text/csv" or "text/plain" => LooksTextual(b),
            _ => false,
        };
    }

    static bool LooksTextual(byte[] b)
    {
        var n = Math.Min(b.Length, 4096);
        for (var i = 0; i < n; i++) if (b[i] == 0x00) return false;
        return true;
    }

    /// <summary>Malware-scan seam. Today it applies cheap structural heuristics (no MZ/ELF executable
    /// header masquerading as a document). Point this at ClamAV / a cloud AV / VirusTotal for production
    /// by replacing the body — every upload already flows through here, so no caller changes are needed.</summary>
    public static bool ScanClean(byte[] b, string mime, out string? reason)
    {
        // Delegates to the shared policy (RES-021) so documents and evidence/attachments are judged
        // identically — and so wiring an external scanner covers every upload path at once, rather than
        // leaving this one on its own executable-header heuristic.
        var verdict = UploadScan.Scan(b, mime);
        reason = verdict.Reason;
        return verdict.Clean;
    }

    static readonly System.Text.RegularExpressions.Regex Unsafe =
        new(@"[^A-Za-z0-9 ._()\-]", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>A safe, human-friendly download filename derived from the admin-supplied title/name. Strips
    /// any path component and unsafe characters, collapses whitespace, caps the length, and guarantees the
    /// correct extension. The stored object key is content-addressed and unrelated to this display name.</summary>
    public static string SafeName(string? raw, string ext)
    {
        var name = (raw ?? "").Replace('\\', '/');
        var slash = name.LastIndexOf('/');
        if (slash >= 0) name = name[(slash + 1)..];
        // drop an existing extension so we always append the canonical one
        var dot = name.LastIndexOf('.');
        if (dot > 0) name = name[..dot];
        name = Unsafe.Replace(name, "_").Trim();
        name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", " ").Trim();
        if (name.Length == 0) name = "document";
        if (name.Length > 120) name = name[..120].Trim();
        return name + ext;
    }
}
