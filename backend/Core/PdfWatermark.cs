using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PCI.Backend.Core;

/// <summary>
/// Stamps a per-recipient watermark onto every page of an EXISTING PDF (admin-uploaded documents whose
/// <c>watermark</c> flag is set). The master file in storage is never modified — each download renders a
/// fresh copy carrying the recipient's identity, so a leaked file is traceable to who received it.
///
/// Implementation notes:
///  • PDFsharp (pure managed, no native libs) opens/saves the document; the watermark itself is written
///    as RAW content-stream operators with the standard Helvetica font (same approach as CertPdf) — this
///    avoids PDFsharp's font-resolver requirement (no TTF files needed on the server).
///  • A `q` stream is PREPENDED and the watermark stream (starting with `Q`) APPENDED, so an original
///    content stream that leaves the graphics state unbalanced can never skew the watermark placement.
///  • Page /Resources may be inherited from the page tree; we copy the inherited entries into an own
///    dictionary before adding ours, so the original page keeps every font/image it referenced.
///  • Semi-transparent (ExtGState CA/ca) diagonal text across the page + a small footer line.
///  • Best-effort by contract: any unparseable/encrypted PDF returns null and the caller decides
///    (the download endpoints fall back to the original and audit the result as unwatermarked).
/// </summary>
public static class PdfWatermark
{
    /// <summary>Apply a diagonal watermark + footer to every page. Returns null if the PDF cannot be
    /// processed (caller falls back to the original bytes).</summary>
    public static byte[]? Apply(byte[] pdf, string mainLine, string footerLine)
    {
        try
        {
            using var input = new MemoryStream(pdf);
            var doc = PdfReader.Open(input, PdfDocumentOpenMode.Modify);
            if (doc.PageCount == 0) return null;

            foreach (var page in doc.Pages)
            {
                var res = OwnResources(doc, page);
                AddFont(doc, res);
                AddAlphaState(doc, res);

                var w = page.Width.Point;
                var h = page.Height.Point;

                // Balance any unbalanced state left by the original content: prepend `q`, start ours with `Q`.
                Prepend(doc, page, "q\n");
                Append(doc, page, BuildOps(w, h, mainLine, footerLine));
            }

            using var outMs = new MemoryStream();
            doc.Save(outMs);
            var bytes = outMs.ToArray();
            // paranoia: a save that somehow produced a non-PDF is treated as a failure
            return bytes.Length > 4 && bytes[0] == (byte)'%' && bytes[1] == (byte)'P' ? bytes : null;
        }
        catch { return null; }
    }

    // ---- page plumbing ----

    /// <summary>The page's OWN /Resources dictionary, creating one if needed. If resources are inherited
    /// from the /Pages tree, the inherited entries are copied in first so nothing the page uses is lost.</summary>
    static PdfDictionary OwnResources(PdfDocument doc, PdfPage page)
    {
        var own = page.Elements.GetDictionary("/Resources");
        if (own is not null) return own;
        own = new PdfDictionary(doc);
        // walk up the page tree for inherited resources and copy their entries (references, not clones)
        var node = page.Elements.GetDictionary("/Parent");
        while (node is not null)
        {
            var inh = node.Elements.GetDictionary("/Resources");
            if (inh is not null)
            {
                foreach (var key in inh.Elements.Keys) own.Elements[key] = inh.Elements[key];
                break;
            }
            node = node.Elements.GetDictionary("/Parent");
        }
        page.Elements["/Resources"] = own;
        return own;
    }

    static void AddFont(PdfDocument doc, PdfDictionary res)
    {
        var fonts = res.Elements.GetDictionary("/Font");
        if (fonts is null) { fonts = new PdfDictionary(doc); res.Elements["/Font"] = fonts; }
        if (fonts.Elements.ContainsKey("/PCIWMF")) return;
        var f = new PdfDictionary(doc);
        f.Elements["/Type"] = new PdfName("/Font");
        f.Elements["/Subtype"] = new PdfName("/Type1");
        f.Elements["/BaseFont"] = new PdfName("/Helvetica-Bold");
        f.Elements["/Encoding"] = new PdfName("/WinAnsiEncoding");
        fonts.Elements["/PCIWMF"] = f;
    }

    static void AddAlphaState(PdfDocument doc, PdfDictionary res)
    {
        var gs = res.Elements.GetDictionary("/ExtGState");
        if (gs is null) { gs = new PdfDictionary(doc); res.Elements["/ExtGState"] = gs; }
        if (gs.Elements.ContainsKey("/PCIWMG")) return;
        var g = new PdfDictionary(doc);
        g.Elements["/Type"] = new PdfName("/ExtGState");
        g.Elements.SetReal("/CA", 0.32);
        g.Elements.SetReal("/ca", 0.32);
        gs.Elements["/PCIWMG"] = g;
    }

    static void Prepend(PdfDocument doc, PdfPage page, string ops)
    {
        var stream = page.Contents.PrependContent();
        stream.CreateStream(Encoding.Latin1.GetBytes(ops));
    }

    static void Append(PdfDocument doc, PdfPage page, string ops)
    {
        var stream = page.Contents.AppendContent();
        stream.CreateStream(Encoding.Latin1.GetBytes(ops));
    }

    // ---- watermark drawing (raw operators, Helvetica-Bold, WinAnsi) ----

    static string BuildOps(double w, double h, string mainLine, string footerLine)
    {
        var main = Latin1(mainLine);
        var footer = Latin1(footerLine);

        // Diagonal along the page's own diagonal; font size scaled so the text spans most of it.
        var angle = Math.Atan2(h, w);
        var cos = Math.Cos(angle); var sin = Math.Sin(angle);
        var diag = Math.Sqrt(w * w + h * h);
        var size = Math.Clamp(diag / Math.Max(main.Length * 0.62, 10), 12, 42);
        var textW = EstWidth(main, size);
        var cx = w / 2; var cy = h / 2;

        var sb = new StringBuilder();
        sb.Append("Q\nq\n");                                     // close the prepended q, open ours
        sb.Append("/PCIWMG gs 0.35 0.42 0.55 rg\n");             // soft slate, ~32% opacity
        sb.Append(F(cos)).Append(' ').Append(F(sin)).Append(' ').Append(F(-sin)).Append(' ')
          .Append(F(cos)).Append(' ').Append(F(cx)).Append(' ').Append(F(cy)).Append(" cm\n");
        sb.Append("BT /PCIWMF ").Append(F(size)).Append(" Tf ")
          .Append(F(-textW / 2)).Append(' ').Append(F(-size / 3)).Append(" Td (").Append(Esc(main)).Append(") Tj ET\n");
        sb.Append("Q\nq\n");
        // small footer line, bottom-left, upright
        sb.Append("/PCIWMG gs 0.25 0.30 0.40 rg\n");
        sb.Append("BT /PCIWMF 7.5 Tf 18 10 Td (").Append(Esc(footer)).Append(") Tj ET\n");
        sb.Append("Q\n");
        return sb.ToString();
    }

    static string F(double v) => v.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>Coerce to the WinAnsi byte range the embedded font declares. CP1252 punctuation
    /// above Latin-1 maps to its WinAnsi byte — ™ (in every certification designation) most
    /// importantly — and anything else non-representable becomes '?', same policy as CertPdf.</summary>
    static string Latin1(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var c in s)
            sb.Append(c switch
            {
                '™' => (char)0x99,
                '’' => (char)0x92,
                '‘' => (char)0x91,
                '“' => (char)0x93,
                '”' => (char)0x94,
                '–' => (char)0x96,
                '—' => (char)0x97,
                '…' => (char)0x85,
                '€' => (char)0x80,
                _ => c <= 0xFF ? c : '?',
            });
        return sb.ToString();
    }

    static string Esc(string s) => s.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");

    /// <summary>Rough Helvetica-Bold width estimate (per-char average ≈0.56em; caps/digits wider,
    /// narrow punctuation thinner). A watermark only needs centring, not typography-exact metrics.</summary>
    static double EstWidth(string s, double size)
    {
        double units = 0;
        foreach (var c in s)
            units += char.IsUpper(c) || char.IsDigit(c) ? 0.68 : c is '.' or ',' or ':' or ';' or 'i' or 'l' or 'j' or '\'' or ' ' ? 0.30 : 0.56;
        return units * size;
    }
}
