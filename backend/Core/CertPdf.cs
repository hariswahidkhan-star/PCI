using System.Text;
using QRCoder;

namespace PCI.Backend.Core;

/// <summary>Everything needed to render one certificate. Values are already the FINAL approved strings —
/// this renderer never sources names, dates or titles itself.</summary>
public sealed class CertDoc
{
    public string Title = "Certificate";            // e.g. "Certified Project Controls Professional" / "Honorary Certificate"
    public string? Subtitle;                          // e.g. the honorary recognition line
    public string Recipient = "";
    public string LeadIn = "This is to certify that"; // honorary uses a different lead-in
    public string? Body;                              // one line under the name (what was achieved / recognised)
    public string CertificateId = "";
    public string? IssueDate;
    public string? ValidUntil;
    public string VerifyUrl = "";
    public bool IsTest;
    public bool Honorary;
}

/// <summary>
/// A dependency-free certificate PDF generator. It writes a minimal, standards-compliant PDF (landscape A4,
/// the 14 standard Helvetica fonts, no font embedding) and draws the QR code as vector modules straight into
/// the content stream — so there are no native libraries (SkiaSharp/System.Drawing) to install and output is
/// byte-for-byte deterministic for a given input, which lets us hash it for tamper-evidence.
/// Non-Latin scripts fall back to the standard font's coverage; such names are flagged for manual review.
/// </summary>
public static class CertPdf
{
    const double W = 842, H = 595; // A4 landscape, points

    public static byte[] Render(CertDoc d)
    {
        var cs = new StringBuilder();
        // ── frame ──
        cs.Append("0.06 0.20 0.38 RG 3 w ")
          .Append(Rect(24, 24, W - 48, H - 48, stroke: true))
          .Append("0.80 0.68 0.36 RG 1 w ")
          .Append(Rect(34, 34, W - 68, H - 68, stroke: true));

        // ── header ──
        double y = H - 92;
        Centered(cs, "PROJECT CONTROLS INSTITUTE GLOBAL", 15, bold: true, y, 0.06, 0.20, 0.38, spacing: 2);
        y -= 20;
        Centered(cs, d.Honorary ? "HONORARY RECOGNITION" : "CERTIFICATE OF ACHIEVEMENT", 10, bold: false, y, 0.45, 0.45, 0.45, spacing: 3);

        // ── title ──
        y -= 58;
        Centered(cs, d.Title.ToUpperInvariant(), TitleSize(d.Title), bold: true, y, 0.06, 0.20, 0.38);
        if (!string.IsNullOrWhiteSpace(d.Subtitle)) { y -= 26; Centered(cs, d.Subtitle!, 14, bold: false, y, 0.30, 0.30, 0.30); }

        // ── recipient ──
        y -= 52;
        Centered(cs, d.LeadIn, 12, bold: false, y, 0.35, 0.35, 0.35);
        y -= 44;
        Centered(cs, d.Recipient, NameSize(d.Recipient), bold: true, y, 0.10, 0.10, 0.12);
        // gold rule under the name
        cs.Append("0.80 0.68 0.36 RG 1.5 w ").Append(Rect(W / 2 - 160, y - 12, 320, 0, stroke: true));

        if (!string.IsNullOrWhiteSpace(d.Body))
        {
            // Word-wrap the certification sentence to the frame's content width (long official
            // names + designation don't fit one line), centred, up to four lines.
            y -= 40;
            foreach (var line in Wrap(d.Body!, 12.5, W - 160, maxLines: 4))
            {
                Centered(cs, line, 12.5, bold: false, y, 0.30, 0.30, 0.30);
                y -= 18;
            }
        }

        // ── footer: id + dates (left), QR (right) ──
        double fy = 120;
        Text(cs, 70, fy, "Certificate ID", 9, bold: false, 0.5, 0.5, 0.5);
        Text(cs, 70, fy - 15, d.CertificateId, 12, bold: true, 0.06, 0.20, 0.38);
        if (!string.IsNullOrWhiteSpace(d.IssueDate)) { Text(cs, 70, fy - 38, "Issued", 9, false, 0.5, 0.5, 0.5); Text(cs, 70, fy - 52, DateOnly(d.IssueDate!), 11, false, 0.2, 0.2, 0.2); }
        if (!string.IsNullOrWhiteSpace(d.ValidUntil)) { Text(cs, 240, fy - 38, "Valid until", 9, false, 0.5, 0.5, 0.5); Text(cs, 240, fy - 52, DateOnly(d.ValidUntil!), 11, false, 0.2, 0.2, 0.2); }

        // signature line (right-of-centre)
        cs.Append("0.4 0.4 0.4 RG 0.8 w ").Append(Rect(430, fy - 8, 180, 0, stroke: true));
        Text(cs, 430, fy - 24, "Registrar, PCI AI", 9, false, 0.45, 0.45, 0.45);

        // ── QR (bottom-right) ──
        DrawQr(cs, d.VerifyUrl, x: W - 70 - 96, y: 56, size: 96);
        Text(cs, W - 70 - 96, 44, "Scan to verify", 8, false, 0.5, 0.5, 0.5);
        // verify URL centred at the very bottom
        Centered(cs, "Verify this certificate at  " + StripScheme(d.VerifyUrl), 8.5, false, 40, 0.45, 0.45, 0.45);

        if (d.IsTest)
        {
            // Big diagonal TEST watermark so a test certificate can never be mistaken for a real one.
            cs.Append("q 0.90 0.55 0.55 rg BT /F2 120 Tf 1 0 0 1 0 0 Tm ")
              .Append("0.86602540 0.5 -0.5 0.86602540 150 210 Tm ")
              .Append("(TEST CERTIFICATE) Tj ET Q ");
        }

        var content = cs.ToString();
        return Assemble(content);
    }

    // ───────────────────────── PDF assembly ─────────────────────────
    static byte[] Assemble(string content)
    {
        var objs = new List<string>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {W:0} {H:0}] /Resources << /Font << /F1 5 0 R /F2 6 0 R >> >> /Contents 4 0 R >>",
            $"<< /Length {Encoding.Latin1.GetByteCount(content)} >>\nstream\n{content}\nendstream",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>",
        };
        var sb = new StringBuilder();
        sb.Append("%PDF-1.4\n%âãÏÓ\n");
        var offsets = new int[objs.Count + 1];
        for (var i = 0; i < objs.Count; i++)
        {
            offsets[i + 1] = Encoding.Latin1.GetByteCount(sb.ToString());
            sb.Append(i + 1).Append(" 0 obj\n").Append(objs[i]).Append("\nendobj\n");
        }
        var xref = Encoding.Latin1.GetByteCount(sb.ToString());
        sb.Append("xref\n0 ").Append(objs.Count + 1).Append('\n');
        sb.Append("0000000000 65535 f \n");
        for (var i = 1; i <= objs.Count; i++) sb.Append(offsets[i].ToString("D10")).Append(" 00000 n \n");
        sb.Append("trailer\n<< /Size ").Append(objs.Count + 1).Append(" /Root 1 0 R >>\nstartxref\n").Append(xref).Append("\n%%EOF");
        return Encoding.Latin1.GetBytes(sb.ToString());
    }

    // ───────────────────────── drawing helpers ─────────────────────────
    static string Rect(double x, double y, double w, double h, bool stroke)
    {
        if (h == 0) return $"{x:0.##} {y:0.##} m {x + w:0.##} {y:0.##} l S ";   // horizontal rule
        return $"{x:0.##} {y:0.##} {w:0.##} {h:0.##} re {(stroke ? "S" : "f")} ";
    }

    static void Text(StringBuilder cs, double x, double y, string s, double size, bool bold, double r, double g, double b)
    {
        cs.Append($"{r:0.##} {g:0.##} {b:0.##} rg BT /{(bold ? "F2" : "F1")} {size:0.##} Tf 1 0 0 1 {x:0.##} {y:0.##} Tm ({Esc(s)}) Tj ET ");
    }

    static void Centered(StringBuilder cs, string s, double size, bool bold, double y, double r, double g, double b, double spacing = 0)
    {
        var width = TextWidth(s, size, bold) + spacing * Math.Max(0, s.Length - 1);
        var x = (W - width) / 2;
        cs.Append($"{r:0.##} {g:0.##} {b:0.##} rg BT /{(bold ? "F2" : "F1")} {size:0.##} Tf {(spacing != 0 ? $"{spacing:0.##} Tc " : "")}1 0 0 1 {x:0.##} {y:0.##} Tm ({Esc(s)}) Tj ET ");
        if (spacing != 0) cs.Append("BT 0 Tc ET ");
    }

    static void DrawQr(StringBuilder cs, string payload, double x, double y, double size)
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode(string.IsNullOrEmpty(payload) ? "https://" : payload, QRCodeGenerator.ECCLevel.M);
        var m = data.ModuleMatrix;
        var n = m.Count;
        var mod = size / n;
        // white quiet background behind the code
        cs.Append("1 1 1 rg ").Append($"{x - 4:0.##} {y - 4:0.##} {size + 8:0.##} {size + 8:0.##} re f ");
        cs.Append("0 0 0 rg ");
        for (var row = 0; row < n; row++)
            for (var col = 0; col < n; col++)
                if (m[row][col])
                {
                    // PDF y grows upward; matrix row 0 is the top.
                    var px = x + col * mod;
                    var py = y + (n - 1 - row) * mod;
                    cs.Append($"{px:0.###} {py:0.###} {mod + 0.2:0.###} {mod + 0.2:0.###} re f ");
                }
    }

    // ───────────────────────── text metrics ─────────────────────────
    static double TitleSize(string s) => s.Length > 42 ? 20 : s.Length > 30 ? 24 : 30;
    static double NameSize(string s) => s.Length > 40 ? 22 : s.Length > 28 ? 28 : 34;

    static double TextWidth(string s, double size, bool bold)
    {
        var w = bold ? HB : HV;
        double total = 0;
        foreach (var ch in s)
        {
            var c = ch;
            total += (c >= 32 && c < 127) ? w[c - 32] : 556; // fallback average advance
        }
        return total / 1000.0 * size;
    }

    /// <summary>Greedy word-wrap against real Helvetica advance widths; last permitted line is
    /// hard-truncated rather than overflowing the frame.</summary>
    static List<string> Wrap(string s, double size, double maxWidth, int maxLines)
    {
        var lines = new List<string>();
        var cur = "";
        foreach (var word in s.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var probe = cur.Length == 0 ? word : cur + " " + word;
            if (TextWidth(probe, size, bold: false) <= maxWidth || cur.Length == 0) { cur = probe; continue; }
            lines.Add(cur); cur = word;
            if (lines.Count == maxLines - 1) break;
        }
        if (cur.Length > 0)
        {
            while (TextWidth(cur, size, bold: false) > maxWidth && cur.Length > 1) cur = cur[..^1];
            lines.Add(cur);
        }
        return lines;
    }

    static string Esc(string s)
    {
        var sb = new StringBuilder(s.Length + 8);
        foreach (var ch in s)
        {
            if (ch is '(' or ')' or '\\') sb.Append('\\').Append(ch);
            // The fonts are declared /WinAnsiEncoding, so common CP1252 punctuation outside
            // Latin-1 maps to its WinAnsi byte instead of being lost (™ is the important one —
            // every certification title carries it).
            else if (ch == '™') sb.Append((char)0x99);      // ™
            else if (ch == '’') sb.Append((char)0x92);      // ’
            else if (ch == '‘') sb.Append((char)0x91);      // ‘
            else if (ch == '“') sb.Append((char)0x93);      // “
            else if (ch == '”') sb.Append((char)0x94);      // ”
            else if (ch == '–') sb.Append((char)0x96);      // –
            else if (ch == '—') sb.Append((char)0x97);      // —
            else if (ch == '…') sb.Append((char)0x85);      // …
            else if (ch == '€') sb.Append((char)0x80);      // €
            else if (ch < 32 || ch > 255) sb.Append('?');   // outside WinAnsi → placeholder (name flagged elsewhere)
            else sb.Append(ch);
        }
        return sb.ToString();
    }

    static string DateOnly(string s) => s.Length >= 10 ? s[..10] : s;
    static string StripScheme(string s) => s.Replace("https://", "").Replace("http://", "");

    // Adobe standard AFM advance widths (1/1000 em) for ASCII 32..126.
    static readonly int[] HV = {
        278,278,355,556,556,889,667,191,333,333,389,584,278,333,278,278,556,556,556,556,556,556,556,556,556,556,278,278,584,584,584,556,
        1015,667,667,722,722,667,611,778,722,278,500,667,556,833,722,778,667,778,722,667,611,722,667,944,667,667,611,278,278,278,469,556,
        333,556,556,500,556,556,278,556,556,222,222,500,222,833,556,556,556,556,333,500,278,556,500,722,500,500,500,334,260,334,584 };
    static readonly int[] HB = {
        278,333,474,556,556,889,722,238,333,333,389,584,278,333,278,278,556,556,556,556,556,556,556,556,556,556,333,333,584,584,584,611,
        975,722,722,722,722,667,611,778,722,278,556,722,611,833,722,778,667,778,722,667,611,722,667,944,667,667,611,333,278,333,584,556,
        333,556,611,556,611,556,333,611,611,278,278,556,278,889,611,611,611,611,389,556,333,611,556,778,556,556,500,389,280,389,584 };
}
