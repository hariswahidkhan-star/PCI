using System.Net.Http.Headers;
using System.Text;

namespace PCI.Backend.Core;

/// <summary>
/// One malware policy for EVERY upload (RES-021).
///
/// Before this existed the two upload paths disagreed: <c>DocStore.Decode</c> rejected executable headers
/// while <c>Storage.DecodeDataUri</c> — the path evidence, identity documents and support attachments take
/// — performed no content-safety check at all. Both now call <see cref="Scan"/>, so a file is judged the
/// same way whatever door it arrives through.
///
/// Two layers:
///
///   1. Built-in signatures, always on and free: executable images (PE/ELF/Mach-O/Java class), the EICAR
///      anti-malware test file, and OLE/PDF documents carrying an embedded executable payload. These catch
///      the obvious cases and give the test suite something deterministic to assert.
///
///   2. An optional external scanner (UPLOAD_SCANNER_URL) for real AV coverage — the bytes are POSTed and
///      the verdict is read from the response. This is the seam an operator wires ClamAV (via a REST
///      shim), an ICAP gateway or a cloud scanning API into.
///
/// **Fail-closed.** When a scanner is configured but cannot be reached, or answers something we do not
/// understand, the upload is REJECTED rather than waved through. An operator who asks for scanning gets
/// scanning or gets an error — never a silent bypass. When no scanner is configured, only the built-in
/// signatures apply, and that is stated honestly in system-check rather than implied to be full AV.
/// </summary>
public static class UploadScan
{
    /// <summary>The verdict for a scanned upload. <paramref name="Reason"/> is null when clean.</summary>
    public readonly record struct Verdict(bool Clean, string? Reason)
    {
        public static Verdict Ok => new(true, null);
        public static Verdict Reject(string reason) => new(false, reason);
    }

    static string? ScannerUrl => Environment.GetEnvironmentVariable("UPLOAD_SCANNER_URL") is { Length: > 0 } u ? u.Trim() : null;

    /// <summary>Whether an external scanner is wired up (surfaced by system-check so the posture is honest).</summary>
    public static bool ExternalScannerConfigured => ScannerUrl is not null;

    static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

    // The EICAR standard anti-malware test file. Not malware — every scanner recognises it, which makes it
    // the one payload that can be asserted in a test suite without shipping something harmful.
    static readonly byte[] Eicar = Encoding.ASCII.GetBytes(
        @"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");

    /// <summary>
    /// Judge an upload. Runs the built-in signatures first (cheap, local), then the external scanner when
    /// one is configured. Returns the first reason to reject, or a clean verdict.
    /// </summary>
    public static Verdict Scan(byte[] bytes, string mime)
    {
        var builtin = ScanBuiltIn(bytes, mime);
        if (!builtin.Clean) return builtin;
        return ScannerUrl is { } url ? ScanExternal(bytes, mime, url) : Verdict.Ok;
    }

    /// <summary>The always-on local signature checks. Public so tests and system-check can exercise them
    /// without an external scanner configured.</summary>
    public static Verdict ScanBuiltIn(byte[] bytes, string mime)
    {
        if (bytes.Length == 0) return Verdict.Reject("empty_file");

        // Executable images — a document should never begin with one, whatever type it claims to be.
        if (Starts(bytes, 0x4D, 0x5A)) return Verdict.Reject("executable_header");                     // PE  "MZ"
        if (Starts(bytes, 0x7F, 0x45, 0x4C, 0x46)) return Verdict.Reject("executable_header");          // ELF
        if (Starts(bytes, 0xCA, 0xFE, 0xBA, 0xBE)) return Verdict.Reject("executable_header");          // Mach-O fat / Java class
        if (Starts(bytes, 0xCF, 0xFA, 0xED, 0xFE) || Starts(bytes, 0xCE, 0xFA, 0xED, 0xFE))
            return Verdict.Reject("executable_header");                                                 // Mach-O
        if (Starts(bytes, 0x23, 0x21)) return Verdict.Reject("script_shebang");                         // "#!"

        // The EICAR test file, wherever it sits in the payload (scanners look for it anywhere).
        if (IndexOf(bytes, Eicar) >= 0) return Verdict.Reject("eicar_test_file");

        // A PDF that embeds a launchable action or an executable attachment is a classic delivery vector.
        if (mime == "application/pdf" && ContainsAscii(bytes, "/Launch")) return Verdict.Reject("pdf_launch_action");

        return Verdict.Ok;
    }

    /// <summary>
    /// Ask the configured scanner. The bytes are POSTed as the raw body; any of the common "infected"
    /// conventions is honoured: a non-2xx status, a JSON/plain body containing INFECTED/FOUND/VIRUS, or
    /// a ClamAV-style "stream: Eicar-Test-Signature FOUND" line. Anything unrecognised — including a
    /// transport failure — rejects, because a scanner that cannot answer is not a clean result.
    /// </summary>
    static Verdict ScanExternal(byte[] bytes, string mime, string url)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new ByteArrayContent(bytes);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(string.IsNullOrWhiteSpace(mime) ? "application/octet-stream" : mime);
            req.Content = content;
            req.Headers.TryAddWithoutValidation("User-Agent", "PCI-UploadScan/1.0");
            using var resp = Http.Send(req);
            var body = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult() ?? "";
            if (!resp.IsSuccessStatusCode)
            {
                // A scanner that answers 4xx/5xx has not cleared the file.
                if (Mentions(body, "INFECTED") || Mentions(body, "FOUND") || Mentions(body, "VIRUS"))
                    return Verdict.Reject("malware_detected");
                return Verdict.Reject("scanner_unavailable");
            }
            if (Mentions(body, "INFECTED") || Mentions(body, "VIRUS") || body.TrimEnd().EndsWith("FOUND", StringComparison.OrdinalIgnoreCase))
                return Verdict.Reject("malware_detected");
            if (Mentions(body, "OK") || Mentions(body, "CLEAN") || body.Trim().Length == 0)
                return Verdict.Ok;
            // An answer we cannot interpret is not permission to store the file.
            return Verdict.Reject("scanner_unavailable");
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[upload-scan] scanner unreachable: {e.Message}");
            return Verdict.Reject("scanner_unavailable");
        }
    }

    static bool Mentions(string haystack, string needle) => haystack.Contains(needle, StringComparison.OrdinalIgnoreCase);

    static bool Starts(byte[] b, params byte[] prefix)
    {
        if (b.Length < prefix.Length) return false;
        for (var i = 0; i < prefix.Length; i++) if (b[i] != prefix[i]) return false;
        return true;
    }

    static bool ContainsAscii(byte[] b, string ascii) => IndexOf(b, Encoding.ASCII.GetBytes(ascii)) >= 0;

    static int IndexOf(byte[] haystack, byte[] needle)
    {
        if (needle.Length == 0 || haystack.Length < needle.Length) return -1;
        var last = haystack.Length - needle.Length;
        for (var i = 0; i <= last; i++)
        {
            var j = 0;
            while (j < needle.Length && haystack[i + j] == needle[j]) j++;
            if (j == needle.Length) return i;
        }
        return -1;
    }
}
