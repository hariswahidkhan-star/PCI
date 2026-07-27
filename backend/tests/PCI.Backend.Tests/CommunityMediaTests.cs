using PCI.Backend.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// PCI World community images — the accept-and-sanitise stage (Core/CommunityMedia.cs;
/// docs/pciworld/CCP_PHASE2_DESIGN.md §6.1, §8).
///
/// Fixtures are GENERATED here rather than committed as binaries. Three reasons, all practical: a
/// reviewer can read exactly what is being fed in; there is no opaque blob in the repository that
/// nobody re-examines; and the hostile cases (a decompression bomb, a trailing payload, a lying
/// header) are constructed byte by byte, which is the only way to be sure the test is actually
/// exercising the attack it names.
///
/// None of these may be relaxed to make an implementation pass (§28.3).
/// </summary>
public class CommunityMediaTests
{
    // ── fixture builders ──────────────────────────────────────────────────────────────────────

    static byte[] Png(int w = 32, int h = 24)
    {
        using var img = new Image<Rgba32>(w, h);
        using var ms = new MemoryStream();
        img.Save(ms, new PngEncoder());
        return ms.ToArray();
    }

    static byte[] JpegWithGps()
    {
        using var img = new Image<Rgba32>(32, 24);
        var exif = new ExifProfile();
        // A real location. The point of the test is that this must not survive to the copy that
        // other people in the room can download.
        exif.SetValue(ExifTag.GPSLatitudeRef, "N");
        exif.SetValue(ExifTag.GPSLatitude, new[]
        {
            new Rational(51, 1), new Rational(30, 1), new Rational(0, 1),
        });
        exif.SetValue(ExifTag.GPSLongitudeRef, "W");
        exif.SetValue(ExifTag.GPSLongitude, new[]
        {
            new Rational(0, 1), new Rational(7, 1), new Rational(0, 1),
        });
        exif.SetValue(ExifTag.Make, "PCI-TEST-CAMERA");
        img.Metadata.ExifProfile = exif;
        using var ms = new MemoryStream();
        img.Save(ms, new JpegEncoder { Quality = 90 });
        return ms.ToArray();
    }

    /// <summary>
    /// A PNG whose IHDR claims enormous dimensions while the file itself is a few dozen bytes —
    /// the classic decompression bomb. If the implementation decoded before checking, this test
    /// would not fail politely; it would try to allocate gigabytes.
    /// </summary>
    static byte[] BombHeader(int w, int h)
    {
        var ms = new MemoryStream();
        ms.Write(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });
        var ihdr = new List<byte>();
        ihdr.AddRange("IHDR"u8.ToArray());
        ihdr.AddRange(BeInt(w));
        ihdr.AddRange(BeInt(h));
        ihdr.AddRange(new byte[] { 8, 6, 0, 0, 0 });   // 8-bit RGBA, no interlace
        ms.Write(BeInt(13));
        ms.Write(ihdr.ToArray());
        ms.Write(BeInt(unchecked((int)Crc32(ihdr.ToArray()))));
        return ms.ToArray();
    }

    static byte[] BeInt(int v) => new[] { (byte)(v >> 24), (byte)(v >> 16), (byte)(v >> 8), (byte)v };

    static uint Crc32(byte[] data)
    {
        uint crc = 0xFFFFFFFF;
        foreach (var b in data)
        {
            crc ^= b;
            for (var i = 0; i < 8; i++) crc = (crc >> 1) ^ (0xEDB88320u & (uint)(-(crc & 1)));
        }
        return crc ^ 0xFFFFFFFF;
    }

    // ── the happy path still has to be a re-encode ────────────────────────────────────────────

    [Fact]
    public void AValidPngIsAcceptedAndProducesADerivative()
    {
        var r = CommunityMedia.Sanitise(Png(), "image/png");
        Assert.True(r.Ok, r.Error);
        Assert.NotNull(r.Derivative);
        Assert.Equal(32, r.Width);
        Assert.Equal(24, r.Height);
    }

    [Fact]
    public void TheServedCopyIsNotTheUploadedBytes()
    {
        // The whole safety argument rests on this: what other people download is something this
        // server encoded, not something a stranger supplied. If these ever became the same array
        // the EXIF, trailing-payload and polyglot guarantees would all quietly evaporate.
        var original = Png();
        var r = CommunityMedia.Sanitise(original, "image/png");
        Assert.True(r.Ok, r.Error);
        Assert.NotSame(original, r.Derivative);
    }

    [Fact]
    public void ARefusalLeavesNothingRenderable()
    {
        // There must be no path where a rejected upload still hands back something servable.
        var r = CommunityMedia.Sanitise(new byte[] { 1, 2, 3, 4 }, "image/png");
        Assert.False(r.Ok);
        Assert.Null(r.Derivative);
        Assert.Null(r.Original);
    }

    // ── privacy: location data must not survive ───────────────────────────────────────────────

    [Fact]
    public void GpsAndCameraMetadataDoNotSurviveTheReEncode()
    {
        var withGps = JpegWithGps();

        // Guard the fixture itself: if the input silently lost its EXIF, the assertion below would
        // pass while testing nothing at all.
        using (var check = Image.Load(withGps))
            Assert.NotNull(check.Metadata.ExifProfile);

        var r = CommunityMedia.Sanitise(withGps, "image/jpeg");
        Assert.True(r.Ok, r.Error);

        using var served = Image.Load(r.Derivative!);
        var exif = served.Metadata.ExifProfile;
        // Either no profile at all, or one carrying none of the uploader's values.
        if (exif is not null)
        {
            Assert.False(exif.TryGetValue(ExifTag.GPSLatitude, out _));
            Assert.False(exif.TryGetValue(ExifTag.GPSLongitude, out _));
            Assert.False(exif.TryGetValue(ExifTag.Make, out _));
        }
    }

    // ── structural refusals ───────────────────────────────────────────────────────────────────

    [Fact]
    public void SvgIsRefusedOutright()
    {
        // SVG is a script-bearing document that browsers execute. Sanitising it is an arms race;
        // refusing it is not.
        var svg = System.Text.Encoding.UTF8.GetBytes(
            "<svg xmlns=\"http://www.w3.org/2000/svg\"><script>alert(1)</script></svg>");
        var r = CommunityMedia.Sanitise(svg, "image/svg+xml");
        Assert.False(r.Ok);
        Assert.Equal("svg_not_accepted", r.Error);
    }

    [Fact]
    public void AnSvgWearingAPngContentTypeIsStillRefused()
    {
        // The declared type is the uploader's claim, not evidence. Trusting it here would make the
        // previous test trivially bypassable.
        var svg = System.Text.Encoding.UTF8.GetBytes("<svg xmlns=\"http://www.w3.org/2000/svg\"></svg>");
        var r = CommunityMedia.Sanitise(svg, "image/png");
        Assert.False(r.Ok);
        Assert.Equal("svg_not_accepted", r.Error);
    }

    [Fact]
    public void AnimationIsRefusedRatherThanFlattened()
    {
        // Publishing frame one after classifying frame one leaves every later frame unexamined.
        var gif = new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00 };
        var r = CommunityMedia.Sanitise(gif, "image/gif");
        Assert.False(r.Ok);
        Assert.Equal("animation_not_accepted", r.Error);
    }

    [Fact]
    public void ADeclaredTypeThatDisagreesWithTheContentIsRefused()
    {
        // A refusal, never a correction: a file whose uploader lied about it is the case worth
        // refusing, not the case worth quietly fixing up.
        var r = CommunityMedia.Sanitise(Png(), "image/jpeg");
        Assert.False(r.Ok);
        Assert.Equal("content_mime_mismatch", r.Error);
    }

    [Fact]
    public void AnUnsupportedTypeIsRefusedByTheAllowList()
    {
        var pdf = System.Text.Encoding.ASCII.GetBytes("%PDF-1.7\n%âãÏÓ\n");
        var r = CommunityMedia.Sanitise(pdf, "application/pdf");
        Assert.False(r.Ok);
        Assert.Equal("file_type_not_allowed", r.Error);
    }

    // ── resource bounds ───────────────────────────────────────────────────────────────────────

    [Fact]
    public void ADecompressionBombIsRefusedFromItsHeader()
    {
        // A 33-byte file claiming 25 000 x 25 000 pixels — 625 megapixels, ~2.5 GB decoded. This
        // test completing quickly IS the assertion: if the implementation decoded before checking
        // the header, it would not fail politely.
        var r = CommunityMedia.Sanitise(BombHeader(25_000, 25_000), "image/png");
        Assert.False(r.Ok);
        Assert.Equal("image_too_large", r.Error);
    }

    [Fact]
    public void ThePixelBudgetBitesBeforeTheEdgeLimitDoes()
    {
        // Both edges are inside MaxDimension; the product is not. Checking only the edges would let
        // this through, which is why the budget exists as a separate rule.
        var r = CommunityMedia.Sanitise(BombHeader(5_900, 5_900), "image/png");
        Assert.False(r.Ok);
        Assert.Equal("image_too_large", r.Error);
    }

    [Fact]
    public void AnOversizeUploadIsRefusedBeforeAnythingElseHappens()
    {
        var big = new byte[CommunityMedia.MaxBytes + 1];
        big[0] = 0x89; big[1] = 0x50; big[2] = 0x4E; big[3] = 0x47;
        var r = CommunityMedia.Sanitise(big, "image/png");
        Assert.False(r.Ok);
        Assert.Equal("file_too_large", r.Error);
    }

    [Fact]
    public void AnEmptyUploadIsRefused()
    {
        Assert.Equal("no_file", CommunityMedia.Sanitise(Array.Empty<byte>(), "image/png").Error);
        Assert.Equal("no_file", CommunityMedia.Sanitise(null, "image/png").Error);
    }

    // ── hostile input must not become a server error ──────────────────────────────────────────

    [Fact]
    public void ATruncatedImageIsARefusalNotACrash()
    {
        // A decoder crash surfacing as a 500 is itself a denial-of-service lever: one malformed
        // upload, repeated, would be enough.
        var png = Png();
        var truncated = png[..(png.Length / 2)];
        var r = CommunityMedia.Sanitise(truncated, "image/png");
        Assert.False(r.Ok);
        Assert.NotNull(r.Error);
    }

    [Fact]
    public void RandomBytesWearingAnImageContentTypeAreARefusal()
    {
        var junk = new byte[4096];
        new Random(20260727).NextBytes(junk);
        var r = CommunityMedia.Sanitise(junk, "image/png");
        Assert.False(r.Ok);
        Assert.NotNull(r.Error);
    }

    [Fact]
    public void APayloadAppendedAfterTheImageDoesNotSurvive()
    {
        // Data glued after a PNG's end marker survives every metadata-stripping pass ever written.
        // It does not survive being decoded to pixels and encoded again — which is the argument for
        // re-encoding rather than stripping.
        var payload = "<script>alert('polyglot')</script>"u8.ToArray();
        var original = Png().Concat(payload).ToArray();

        var r = CommunityMedia.Sanitise(original, "image/png");
        Assert.True(r.Ok, r.Error);

        var served = System.Text.Encoding.ASCII.GetString(r.Derivative!);
        Assert.DoesNotContain("polyglot", served, StringComparison.Ordinal);
    }

    // ── the sniffer itself ────────────────────────────────────────────────────────────────────

    [Fact]
    public void TheSnifferIdentifiesEachFormatFromItsMagicBytes()
    {
        Assert.Equal("image/png", CommunityMedia.Sniff(Png()));
        Assert.Equal("image/jpeg", CommunityMedia.Sniff(JpegWithGps()));
        Assert.Null(CommunityMedia.Sniff(new byte[] { 0x00, 0x01, 0x02, 0x03 }));
    }

    [Fact]
    public void SvgIsNotOnTheAllowList()
    {
        // Stated as a test so that "add SVG support" cannot be a one-line change nobody discusses.
        Assert.DoesNotContain("image/svg+xml", CommunityMedia.AllowedMime.Keys);
        Assert.DoesNotContain("image/gif", CommunityMedia.AllowedMime.Keys);
    }
}
