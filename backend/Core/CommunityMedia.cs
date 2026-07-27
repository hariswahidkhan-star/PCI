using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using System.Security.Cryptography;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community images (Phase 2) — the accept-and-sanitise stage
/// (docs/pciworld/CCP_PHASE2_DESIGN.md §6.1).
///
/// This runs BEFORE any classifier and is independent of one. Its job is not to decide whether an
/// image is acceptable — that is the scanner's, and the scanner is fail-closed — but to make sure
/// that whatever reaches the scanner, the store and eventually a participant's browser is a plain
/// raster image this server produced, of a bounded size, carrying none of the uploader's metadata.
///
/// THE SINGLE MOST VALUABLE CONTROL HERE IS THE RE-ENCODE.
///
/// The bytes that get served are never the bytes that were uploaded. They are the output of
/// encoding a decoded pixel buffer, which means:
///
///   • EXIF is gone — including GPS. A guest room must not leak where someone was standing, and
///     "we stripped the tags we know about" is a weaker promise than "the output has no tag section
///     because we wrote it ourselves".
///   • Trailing payloads are gone. Data appended after an image's end marker survives every
///     metadata-stripping pass and does not survive a re-encode.
///   • Polyglot files stop being polyglots. A file that is simultaneously a valid GIF and a valid
///     archive, or a valid PNG and a valid HTML document, decodes to pixels and re-encodes to one
///     unambiguous format.
///
/// TWO REJECTIONS ARE DELIBERATE AND ARE NOT OVERSIGHTS.
///
/// SVG is refused outright. SVG is a script-bearing XML document that browsers execute, not an
/// image; sanitising it is an arms race that has been lost repeatedly across the industry, and a
/// standing XSS surface in an anonymous guest room is not worth the feature. Animation is refused
/// in this release because a frame budget is a different scanning problem — every frame needs
/// classifying — and shipping it half-done would mean classifying frame one and publishing frames
/// two onward unexamined.
///
/// ORDER MATTERS: the dimension and pixel budget is enforced from the HEADER, before any full
/// decode. That is what stops a decompression bomb — a 20 KB PNG that expands to 40 000 x 40 000
/// pixels and 6 GB of RAM. Checking after decoding would mean the attack has already succeeded.
/// </summary>
public static class CommunityMedia
{
    /// <summary>Upload cap, comfortably inside Kestrel's 6 MB request body limit.</summary>
    public const long MaxBytes = 2_000_000;
    /// <summary>Longest permitted edge. A room photo or a screenshot fits; a bomb does not.</summary>
    public const int MaxDimension = 6_000;
    /// <summary>Total pixel budget — the real bomb guard, since 6000 x 6000 is already 36 MP.</summary>
    public const long MaxPixels = 30_000_000;

    /// <summary>The only formats accepted. SVG is absent on purpose (see the class comment).</summary>
    public static readonly IReadOnlyDictionary<string, string> AllowedMime = new Dictionary<string, string>
    {
        ["image/png"] = ".png",
        ["image/jpeg"] = ".jpg",
        ["image/webp"] = ".webp",
    };

    /// <summary>
    /// The outcome of the accept stage. <paramref name="Derivative"/> is the ONLY artefact that may
    /// ever be served to a participant, and it exists only when <c>Ok</c> — which is why a failed
    /// sanitise cannot leave anything renderable behind.
    /// </summary>
    public record Accepted(
        bool Ok, string? Error,
        byte[]? Original, byte[]? Derivative,
        string Mime, int Width, int Height, string Sha256);

    static Accepted Refuse(string error, string mime = "") =>
        new(false, error, null, null, mime, 0, 0, "");

    /// <summary>
    /// Identify a format from its magic bytes. Deliberately independent of what the client claimed:
    /// the comparison between the two is the check, so deriving one from the other would be
    /// circular.
    /// </summary>
    public static string? Sniff(byte[] b)
    {
        if (b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47
            && b[4] == 0x0D && b[5] == 0x0A && b[6] == 0x1A && b[7] == 0x0A) return "image/png";
        if (b.Length >= 3 && b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF) return "image/jpeg";
        if (b.Length >= 12 && b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46
            && b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50) return "image/webp";
        if (b.Length >= 6 && b[0] == 0x47 && b[1] == 0x49 && b[2] == 0x46) return "image/gif";
        // Enough of an SVG probe to name the refusal accurately rather than reporting a generic
        // "unrecognised format" — an operator reading the log should see WHY it was refused.
        var head = System.Text.Encoding.ASCII.GetString(b, 0, Math.Min(b.Length, 256)).TrimStart();
        if (head.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase)
            || head.StartsWith("<svg", StringComparison.OrdinalIgnoreCase)) return "image/svg+xml";
        return null;
    }

    /// <summary>
    /// Validate, bound and re-encode. Never throws for bad input — a malformed or hostile upload is
    /// an ordinary refusal with a reason code, not a 500, because a decoder crash that surfaces as a
    /// server error is itself a denial-of-service lever.
    /// </summary>
    public static Accepted Sanitise(byte[]? bytes, string? declaredMime)
    {
        if (bytes is null || bytes.Length == 0) return Refuse("no_file");
        if (bytes.LongLength > MaxBytes) return Refuse("file_too_large");

        var declared = (declaredMime ?? "").Trim().ToLowerInvariant();
        var sniffed = Sniff(bytes);

        // Name the two structural refusals precisely, before the generic allow-list check, so the
        // participant is told what is actually wrong and the audit trail records which rule fired.
        if (sniffed == "image/svg+xml") return Refuse("svg_not_accepted", "image/svg+xml");
        if (sniffed == "image/gif") return Refuse("animation_not_accepted", "image/gif");

        if (!AllowedMime.ContainsKey(declared)) return Refuse("file_type_not_allowed", declared);
        if (sniffed is null) return Refuse("unrecognised_image", declared);
        // Disagreement is a refusal, never a correction. Silently trusting the sniff would accept a
        // file whose uploader was lying about it, which is exactly the case worth refusing.
        if (sniffed != declared) return Refuse("content_mime_mismatch", declared);

        // ── The bomb guard: bounds from the HEADER, before any full decode. ──
        ImageInfo info;
        try { info = Image.Identify(bytes); }
        catch { return Refuse("unrecognised_image", declared); }

        if (info.Width <= 0 || info.Height <= 0) return Refuse("unrecognised_image", declared);
        if (info.Width > MaxDimension || info.Height > MaxDimension) return Refuse("image_too_large", declared);
        if ((long)info.Width * info.Height > MaxPixels) return Refuse("image_too_large", declared);

        try
        {
            using var image = Image.Load<Rgba32>(bytes);

            // Animation is refused rather than flattened: publishing frame one after classifying
            // frame one would leave every later frame unexamined.
            if (image.Frames.Count > 1) return Refuse("animation_not_accepted", declared);

            // Belt and braces with the header check above — a decoder that disagreed with its own
            // header is precisely the situation not to trust.
            if (image.Width > MaxDimension || image.Height > MaxDimension
                || (long)image.Width * image.Height > MaxPixels) return Refuse("image_too_large", declared);

            StripMetadata(image);

            using var ms = new MemoryStream();
            switch (declared)
            {
                case "image/png":
                    image.Save(ms, new PngEncoder { CompressionLevel = PngCompressionLevel.BestCompression });
                    break;
                case "image/jpeg":
                    image.Save(ms, new JpegEncoder { Quality = 85 });
                    break;
                default:
                    image.Save(ms, new WebpEncoder());
                    break;
            }

            var derivative = ms.ToArray();
            // A derivative that re-encoded LARGER than the cap would sail past the upload limit on
            // the way back out. Refuse rather than serve something we would have rejected inbound.
            if (derivative.LongLength > MaxBytes) return Refuse("image_too_large", declared);

            var sha = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
            return new Accepted(true, null, bytes, derivative, declared, image.Width, image.Height, sha);
        }
        catch
        {
            // Any decode failure — truncated, corrupt, or deliberately malformed — is a refusal.
            return Refuse("unrecognised_image", declared);
        }
    }

    /// <summary>
    /// Drop every metadata profile, on the image and on each frame. The re-encode already means the
    /// output carries no tag section the uploader wrote, but ImageSharp will faithfully COPY a
    /// profile it parsed into the encoder's output if one is still attached — so clearing them is
    /// what makes the guarantee hold rather than merely usually hold.
    /// </summary>
    static void StripMetadata(Image<Rgba32> image)
    {
        image.Metadata.ExifProfile = null;
        image.Metadata.IptcProfile = null;
        image.Metadata.XmpProfile = null;
        image.Metadata.IccProfile = null;
        foreach (var frame in image.Frames)
        {
            frame.Metadata.ExifProfile = null;
            frame.Metadata.IptcProfile = null;
            frame.Metadata.XmpProfile = null;
            frame.Metadata.IccProfile = null;
        }
    }
}
