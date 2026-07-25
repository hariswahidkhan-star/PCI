using PCI.Backend.Core;
using PdfSharp.Pdf;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// SEC-4 — hostile-file robustness for the per-recipient watermarker. <see cref="PdfWatermark.Apply"/>
/// is best-effort by contract: any unparseable / non-PDF / encrypted input must return <c>null</c>
/// (the download endpoints then fall back to the original bytes and audit the download as
/// unwatermarked) — it must NEVER throw, so a crafted "document" can't crash a download handler.
/// </summary>
public class PdfWatermarkTests
{
    // A genuinely valid single-page PDF, built by the same PDFsharp the watermarker uses.
    static byte[] ValidPdf()
    {
        var doc = new PdfDocument();
        doc.AddPage();
        using var ms = new MemoryStream();
        doc.Save(ms);
        return ms.ToArray();
    }

    [Fact]
    public void Apply_WatermarksAValidPdf_AndKeepsThePdfHeader()
    {
        var outp = PdfWatermark.Apply(ValidPdf(), "PCL-AI — Sam Rivera", "Issued to sam@example.com");
        Assert.NotNull(outp);
        // The result is still a PDF (the paranoia guard requires the %P… header).
        Assert.True(outp!.Length > 4);
        Assert.Equal((byte)'%', outp[0]);
        Assert.Equal((byte)'P', outp[1]);
    }

    [Theory]
    [InlineData("")]                                   // empty
    [InlineData("hello, not a pdf at all")]            // plain text
    [InlineData("%PDF-1.4")]                           // header only, no body/xref — unparseable
    [InlineData("<svg xmlns='http://www.w3.org/2000/svg'><script>x</script></svg>")] // hostile SVG masquerading
    [InlineData("%PDF-1.4\n1 0 obj<<>>endobj\n")]      // truncated object, no xref/trailer
    public void Apply_ReturnsNull_OnMalformedOrHostileInput_WithoutThrowing(string payload)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(payload);
        // The contract is "null, never an exception" — assert both.
        var outp = Record.Exception(() => PdfWatermark.Apply(bytes, "m", "f"));
        Assert.Null(outp); // no exception escaped
        Assert.Null(PdfWatermark.Apply(bytes, "m", "f"));
    }

    [Fact]
    public void Apply_ReturnsNull_OnRandomBinaryGarbage()
    {
        var garbage = new byte[512];
        for (int i = 0; i < garbage.Length; i++) garbage[i] = (byte)((i * 37 + 11) & 0xFF);
        Assert.Null(PdfWatermark.Apply(garbage, "m", "f"));
    }
}
