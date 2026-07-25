using System.Text;
using PCI.Backend.Data;
using QRCoder;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — the Passport as a verifiable artefact.
///
/// A Passport is practice evidence its owner chose to publish. Three properties make it worth
/// something to the person reading it, and all three are implemented here:
///
///   Verifiable  — the QR code and the verification page resolve to the live record on this site,
///                 so a recruiter never has to trust a document handed to them. A revoked or
///                 expired link stops resolving, and the PDF says where to check.
///   Consented   — the owner controls what appears, per item AND per field. Nothing is published
///                 that they did not switch on.
///   Honest      — it says, on every surface, that this is practice and not certification. There
///                 are no ranks, no percentiles and no comparisons with other people, because we
///                 do not have an honest basis for any of them.
///
/// The QR and PDF are drawn with no native dependencies (QRCoder's module matrix + the CertPdf
/// approach), so output is deterministic and the container needs nothing installed.
/// </summary>
public static class WorldPassport
{
    /// <summary>What a viewer of a published Passport is allowed to see.</summary>
    public sealed record Disclosure(bool Scores, bool Profiles, bool Dates)
    {
        public static Disclosure From(Dictionary<string, object?> user) => new(
            H.L(user["passport_show_scores"]) == 1,
            H.L(user["passport_show_profiles"]) == 1,
            H.L(user["passport_show_dates"]) == 1);

        /// <summary>The shape used for the owner's own view and the PDF footer.</summary>
        public string Summary =>
            (Scores ? "scores" : null, Profiles ? "decision profiles" : null, Dates ? "dates" : null) switch
            {
                (null, null, null) => "challenge titles only",
                _ => string.Join(", ", new[] { Scores ? "scores" : null, Profiles ? "decision profiles" : null, Dates ? "dates" : null }
                        .Where(s => s is not null)),
            };
    }

    /// <summary>True when a published Passport link has passed its owner-set expiry.</summary>
    public static bool Expired(Dictionary<string, object?> user)
    {
        var s = H.Str(user["passport_expires_at"]);
        if (string.IsNullOrWhiteSpace(s)) return false;               // NULL = no expiry
        return DateTime.TryParse(s, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var when)
               && when <= DateTime.UtcNow;
    }

    // ───────────────────────────── QR ─────────────────────────────

    /// <summary>
    /// The verification QR as inline SVG. Drawn from QRCoder's module matrix rather than its PNG
    /// renderer so there is no native image dependency, it scales without blurring, and it costs
    /// no extra request. Decorative-by-role: the same URL is always printed as text beside it, so
    /// a reader who cannot scan is never stuck.
    /// </summary>
    public static string QrSvg(string payload, int size = 148)
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode(string.IsNullOrWhiteSpace(payload) ? "https://" : payload, QRCodeGenerator.ECCLevel.M);
        var m = data.ModuleMatrix;
        var n = m.Count;
        var sb = new StringBuilder();
        sb.Append($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{size}\" height=\"{size}\" viewBox=\"0 0 {n} {n}\" ")
          .Append("role=\"img\" aria-label=\"QR code linking to this Passport's verification page\" ")
          .Append("style=\"background:#fff;display:block\">");
        for (var row = 0; row < n; row++)
        {
            // Emit horizontal runs rather than one rect per module: the same image, a fraction of
            // the bytes on a page that already inlines its CSS.
            var col = 0;
            while (col < n)
            {
                if (!m[row][col]) { col++; continue; }
                var start = col;
                while (col < n && m[row][col]) col++;
                sb.Append($"<rect x=\"{start}\" y=\"{row}\" width=\"{col - start}\" height=\"1\" fill=\"#0E1525\"/>");
            }
        }
        return sb.Append("</svg>").ToString();
    }

    // ───────────────────────────── PDF ─────────────────────────────

    public sealed class PassportDoc
    {
        public string Name = "";
        public string VerifyUrl = "";
        public int Completed;
        public int Industries;
        public int Tracks;
        public string? IssuedOn;
        public string? ExpiresOn;
        public Disclosure Show = new(true, true, true);
        public List<(string Title, string Industry, string Difficulty, string Score, string Profile, string Date)> Rows = new();
    }

    const double W = 595, H2 = 842;   // A4 portrait, points

    // Brand colours in PDF RGB. Gilt is the passport-cover accent the web artefact uses; the
    // muted variant strokes hairlines that must sit behind, not compete with, the content.
    const string Noir = "0.055 0.082 0.145";
    const string Crimson = "0.757 0.200 0.161";
    const string Gilt = "0.784 0.635 0.294";
    const string GiltFaint = "0.855 0.780 0.600";

    /// <summary>
    /// A one-page Passport PDF: same content, same disclosure rules and same practice notice as the
    /// web page — a document that claims more than the page it came from would be a forgery of our
    /// own making. Deterministic for a given input, with no font embedding and no native libraries.
    /// Drawn as the artefact, not a printout: noir cover band with the gilt seal, a hairline page
    /// frame, ruled stat plinths and separated evidence rows.
    /// </summary>
    public static byte[] Pdf(PassportDoc d)
    {
        var cs = new StringBuilder();

        // ── hairline page frame, the way a security document rules its edge ──
        cs.Append($"{GiltFaint} RG 0.9 w ").Append($"20 20 {W - 40:0.##} {H2 - 40:0.##} re S ");
        cs.Append($"{GiltFaint} RG 0.5 w ").Append($"24 24 {W - 48:0.##} {H2 - 48:0.##} re S ");

        // ── header band, in the brand's noir, with the gilt seal ──
        cs.Append($"{Noir} rg ").Append($"20 {H2 - 130:0.##} {W - 40:0.##} 110 re f ");
        cs.Append($"{Crimson} rg ").Append($"44 {H2 - 116:0.##} 62 4 re f ");           // crimson rule
        // The endorsement follows the crimson rule, exactly as the web lockup draws it.
        Text(cs, "FROM THE PROJECT CONTROLS INSTITUTE", 114, H2 - 115.5, 7.5, bold: false, 0.72, 0.78, 0.86, spacing: 1.6);
        Text(cs, "PCI WORLD", 44, H2 - 62, 26, bold: true, 1, 1, 1);
        Text(cs, "PASSPORT — VERIFIED PRACTICE EVIDENCE", 44, H2 - 84, 9.5, bold: false, 0.72, 0.78, 0.86, spacing: 2.2);
        Seal(cs, W - 88, H2 - 75, 33);

        double y = H2 - 178;
        Text(cs, d.Name, 44, y, 25, bold: true, 0.06, 0.09, 0.16);
        y -= 24;
        Text(cs, "Project Controls Institute · PCI World", 44, y, 11, bold: false, 0.28, 0.33, 0.41);

        // ── the three counts, on ruled plinths ──
        y -= 48;
        cs.Append("0.89 0.91 0.94 RG 1 w ").Append($"44 {y + 16:0.##} m {W - 44:0.##} {y + 16:0.##} l S ");
        void Stat(double x, string label, string value)
        {
            Text(cs, label.ToUpperInvariant(), x, y, 8.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.6);
            Text(cs, value, x, y - 30, 27, bold: true, 0.06, 0.09, 0.16);
        }
        Stat(44, "Challenges completed", d.Completed.ToString());
        Stat(230, "Industries", d.Industries.ToString());
        Stat(350, "Tracks", d.Tracks.ToString());
        cs.Append($"{GiltFaint} RG 0.8 w ")
          .Append($"212 {y - 38:0.##} m 212 {y + 10:0.##} l S ")
          .Append($"332 {y - 38:0.##} m 332 {y + 10:0.##} l S ");
        y -= 58;

        // ── evidence table, honouring the owner's field-level disclosure ──
        y -= 34;
        cs.Append($"{Gilt} RG 1.1 w ").Append($"44 {y + 16:0.##} m {W - 44:0.##} {y + 16:0.##} l S ");
        Text(cs, "CHALLENGE", 44, y, 8.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.4);
        if (d.Show.Scores) Text(cs, "SCORE", 400, y, 8.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.4);
        if (d.Show.Dates) Text(cs, "COMPLETED", 462, y, 8.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.4);
        y -= 8;

        foreach (var r in d.Rows)
        {
            // One page, by design — and the rows stop above the verification band, so the
            // evidence can never collide with the QR frame however much of it there is.
            if (y < 252) break;
            y -= 22;
            Text(cs, Clip(r.Title, 52), 44, y, 10.5, bold: true, 0.06, 0.09, 0.16);
            var sub = string.Join("  ·  ", new[] { r.Industry, r.Difficulty, d.Show.Profiles ? r.Profile : null }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            if (sub.Length > 0) { y -= 12; Text(cs, Clip(sub, 62), 44, y, 8.8, bold: false, 0.35, 0.40, 0.48); y += 12; }
            if (d.Show.Scores) Text(cs, r.Score, 400, y, 10.5, bold: true, 0.06, 0.09, 0.16);
            if (d.Show.Dates) Text(cs, r.Date, 462, y, 10.5, bold: false, 0.28, 0.33, 0.41);
            y -= 12;
            cs.Append("0.93 0.94 0.96 RG 0.7 w ").Append($"44 {y - 6:0.##} m {W - 44:0.##} {y - 6:0.##} l S ");
        }

        // ── verification block: the QR and the URL in text, so neither is the only route ──
        cs.Append($"{GiltFaint} RG 0.9 w ").Append($"{W - 158:0.##} 118 124 124 re S ");
        DrawQr(cs, d.VerifyUrl, W - 148, 128, 104);
        Text(cs, "VERIFY THIS PASSPORT", 44, 214, 8.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.6);
        Text(cs, "Scan the code or open the link below. The live record is the authority —", 44, 196, 9.5, bold: false, 0.28, 0.33, 0.41);
        Text(cs, "this document is a copy and can be withdrawn by its owner at any time.", 44, 184, 9.5, bold: false, 0.28, 0.33, 0.41);
        Text(cs, Clip(d.VerifyUrl, 68), 44, 166, 9.5, bold: true, 0.11, 0.31, 0.85);
        var meta = string.Join("   ", new[]
        {
            d.IssuedOn is null ? null : "Issued " + d.IssuedOn,
            d.ExpiresOn is null ? null : "Link expires " + d.ExpiresOn,
            "Shows: " + d.Show.Summary,
        }.Where(s => s is not null));
        Text(cs, meta, 44, 150, 8.5, bold: false, 0.45, 0.50, 0.57);

        // ── the practice notice, verbatim from the product constant ──
        cs.Append($"{Crimson} rg ").Append("44 62 3 46 re f ");
        var notice = Wrap(WorldPages.PracticeNotice, 92);
        var ny = 96.0;
        foreach (var line in notice.Take(4)) { Text(cs, line, 56, ny, 8.8, bold: false, 0.35, 0.40, 0.48); ny -= 11; }
        Text(cs, WorldPages.OperatedBy, 44, 42, 8, bold: false, 0.45, 0.50, 0.57);

        return Assemble(cs.ToString());
    }

    /// <summary>The gilt seal from the web cover, in PDF strokes: two Bézier-drawn rings, tick
    /// marks, the monogram and the crimson underline. Purely decorative; every claim it decorates
    /// is stated in text.</summary>
    static void Seal(StringBuilder cs, double cx, double cy, double r)
    {
        cs.Append($"{Gilt} RG 1.1 w ");
        Circle(cs, cx, cy, r);
        cs.Append($"{Gilt} RG 0.5 w ");
        Circle(cs, cx, cy, r * 0.86);
        cs.Append($"{Gilt} RG 0.8 w ");
        for (var i = 0; i < 12; i++)
        {
            var a = Math.PI * i / 6.0;
            var (dx, dy) = (Math.Cos(a), Math.Sin(a));
            cs.Append($"{cx + dx * r * 0.90:0.##} {cy + dy * r * 0.90:0.##} m ")
              .Append($"{cx + dx * r * 0.98:0.##} {cy + dy * r * 0.98:0.##} l S ");
        }
        Text(cs, "PW", cx - 10.5, cy - 4, 14, bold: true, 0.784, 0.635, 0.294);
        cs.Append($"{Crimson} rg ").Append($"{cx - 6:0.##} {cy - 10:0.##} 12 1.6 re f ");
    }

    /// <summary>A circle as four Bézier arcs (k = 0.5523·r), stroked with the current settings —
    /// PDF content streams have no circle primitive.</summary>
    static void Circle(StringBuilder cs, double cx, double cy, double r)
    {
        var k = 0.552284749831 * r;
        cs.Append($"{cx + r:0.##} {cy:0.##} m ")
          .Append($"{cx + r:0.##} {cy + k:0.##} {cx + k:0.##} {cy + r:0.##} {cx:0.##} {cy + r:0.##} c ")
          .Append($"{cx - k:0.##} {cy + r:0.##} {cx - r:0.##} {cy + k:0.##} {cx - r:0.##} {cy:0.##} c ")
          .Append($"{cx - r:0.##} {cy - k:0.##} {cx - k:0.##} {cy - r:0.##} {cx:0.##} {cy - r:0.##} c ")
          .Append($"{cx + k:0.##} {cy - r:0.##} {cx + r:0.##} {cy - k:0.##} {cx + r:0.##} {cy:0.##} c S ");
    }

    // ───────────────────────── PDF primitives (CertPdf's approach) ─────────────────────────

    static void Text(StringBuilder cs, string s, double x, double y, double size, bool bold,
        double r, double g, double b, double spacing = 0)
    {
        cs.Append($"{r:0.###} {g:0.###} {b:0.###} rg BT /{(bold ? "F2" : "F1")} {size:0.##} Tf ")
          .Append(spacing != 0 ? $"{spacing:0.##} Tc " : "")
          .Append($"1 0 0 1 {x:0.##} {y:0.##} Tm ({Esc(s)}) Tj ET ");
        if (spacing != 0) cs.Append("BT 0 Tc ET ");
    }

    static void DrawQr(StringBuilder cs, string payload, double x, double y, double size)
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode(string.IsNullOrWhiteSpace(payload) ? "https://" : payload, QRCodeGenerator.ECCLevel.M);
        var m = data.ModuleMatrix;
        var n = m.Count;
        var mod = size / n;
        cs.Append("1 1 1 rg ").Append($"{x - 5:0.##} {y - 5:0.##} {size + 10:0.##} {size + 10:0.##} re f ");
        cs.Append("0.055 0.082 0.145 rg ");
        for (var row = 0; row < n; row++)
            for (var col = 0; col < n; col++)
                if (m[row][col])
                    cs.Append($"{x + col * mod:0.###} {y + (n - 1 - row) * mod:0.###} {mod + 0.2:0.###} {mod + 0.2:0.###} re f ");
    }

    static string Clip(string? s, int n)
    {
        s = (s ?? "").Trim();
        return s.Length <= n ? s : s[..(n - 1)] + "…";
    }

    static List<string> Wrap(string s, int width)
    {
        var lines = new List<string>();
        var line = new StringBuilder();
        foreach (var word in s.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Length > 0 && line.Length + 1 + word.Length > width) { lines.Add(line.ToString()); line.Clear(); }
            if (line.Length > 0) line.Append(' ');
            line.Append(word);
        }
        if (line.Length > 0) lines.Add(line.ToString());
        return lines;
    }

    static string Esc(string s)
    {
        var sb = new StringBuilder(s.Length + 8);
        foreach (var ch in s)
        {
            if (ch is '(' or ')' or '\\') sb.Append('\\').Append(ch);
            else if (ch < 32 || ch > 126) sb.Append(ch switch
            {
                '’' => "'", '‘' => "'", '“' => "\"", '”' => "\"",
                '–' or '—' => "-", '…' => "...", '·' => "-",
                _ => "?",
            });
            else sb.Append(ch);
        }
        return sb.ToString();
    }

    /// <summary>Wrap a content stream into a minimal, valid single-page PDF (Helvetica, no
    /// embedding). Same construction as CertPdf so both stay byte-deterministic.</summary>
    static byte[] Assemble(string content)
    {
        var objects = new List<string>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {W:0.##} {H2:0.##}] /Resources << /Font << /F1 5 0 R /F2 6 0 R >> >> /Contents 4 0 R >>",
            null!,   // 4: the content stream, written below
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>",
        };

        var body = new StringBuilder("%PDF-1.4\n");
        var offsets = new List<int>();
        for (var i = 0; i < objects.Count; i++)
        {
            offsets.Add(body.Length);
            body.Append(i + 1).Append(" 0 obj\n");
            if (i == 3) body.Append($"<< /Length {content.Length} >>\nstream\n").Append(content).Append("\nendstream\n");
            else body.Append(objects[i]).Append('\n');
            body.Append("endobj\n");
        }
        var xref = body.Length;
        body.Append("xref\n0 ").Append(objects.Count + 1).Append("\n0000000000 65535 f \n");
        foreach (var off in offsets) body.Append(off.ToString("D10")).Append(" 00000 n \n");
        body.Append("trailer\n<< /Size ").Append(objects.Count + 1).Append(" /Root 1 0 R >>\nstartxref\n")
            .Append(xref).Append("\n%%EOF");
        return Encoding.ASCII.GetBytes(body.ToString());
    }
}
