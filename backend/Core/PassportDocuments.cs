using System.Text;
using QRCoder;

namespace PCI.Backend.Core;

/// <summary>
/// Phase 5A (Release 1) — the printable PCI Passport documents (spec §7A.3–§7A.5): a CR80 wallet
/// card and a 4×6in portrait event badge. Verification-QR documents ONLY — the event-admission
/// system (entry passes, gates, scanners, offline validation) is a later slice and nothing here
/// stubs it.
///
/// Both documents are drawn from the SAME canonical data the existing Passport PDF
/// (<see cref="WorldPassport"/>) uses, with the same construction: QRCoder's module matrix and the
/// CertPdf-style content-stream assembly — deterministic bytes, vector Helvetica text, no native
/// dependencies, nothing installed in the container.
///
/// The QR contract (privacy-critical, tested by tests/passport_documents_test.py):
///   • the QR encodes ONLY an opaque HTTPS verification URL — never the PCI Student Number alone,
///     never an email address, never any internal id;
///   • the payload is the caller-resolved <see cref="DocData.VerifyUrl"/> and nothing else;
///   • solid black modules on a white field, the same QRCoder call and ECC level (M) the existing
///     Passport PDF uses;
///   • the 4-module quiet zone lives INSIDE the reserved square (QRCoder 1.6's module matrix
///     already carries it: ModulePlacer.AddQuietZone pads the matrix by 4 light modules per side),
///     and nothing is ever drawn inside the QR or its quiet zone.
///
/// Geometry (exact page boxes in PDF points, pinned by the spec and by the test suite):
///   wallet  CR80 (ISO/IEC 7810 ID-1, 85.60 × 53.98 mm)  →  242.65 × 153.07 pt, two pages
///   badge   4 × 6 in portrait                            →  288 × 432 pt, one page
///   wallet QR reserved square 24.0 mm (68.03 pt) ≥ the 22 mm minimum incl. quiet zone
///   badge  QR reserved square 34.0 mm (96.38 pt) ≥ the 30 mm minimum incl. quiet zone
/// At those sizes the printed module stays ≥ 0.40 mm for every payload up to QR version 8 at
/// ECC M (133 URL characters) — comfortably above the canonical and Render-host URL lengths.
///
/// This class is a pure generator: it never touches the database and never decides WHO may have a
/// document. Publication gating (published, unexpired, active — otherwise refuse) lives in
/// Endpoints/PassportDocs.cs, so the rule sits with the auth context that can enforce it.
/// </summary>
public static class PassportDocuments
{
    // ── exact page boxes, PDF points (spec §7A.3/§7A.4) ──
    public const double WalletWidthPt = 242.65;    // CR80 85.60 mm
    public const double WalletHeightPt = 153.07;   // CR80 53.98 mm (54.0 mm nominal in points)
    public const double BadgeWidthPt = 288;        // 4.0 in
    public const double BadgeHeightPt = 432;       // 6.0 in

    // ── reserved QR squares, INCLUDING the 4-module quiet zone ──
    public const double WalletQrPt = 68.03;        // 24.0 mm — spec minimum is 22 mm
    public const double BadgeQrPt = 96.38;         // 34.0 mm — spec minimum is 30 mm

    /// <summary>The opaque printed-document verification path. Keyed by the passport token's
    /// stored HASH: the raw /world/p/ token is deliberately known to nobody but the owner, and a
    /// document generator must neither reconstruct nor re-mint it. The hash is derived from the
    /// token (never the reverse), shares its exact lifecycle (publish rotates it, unpublish clears
    /// it, expiry kills it), and carries no PII — so the printed QR is a capability equivalent to,
    /// and strictly weaker than, the owner-shared link itself.</summary>
    public const string VerifyPathPrefix = "/world/pd/";
    public static string VerifyPath(string tokenSha) => VerifyPathPrefix + tokenSha;

    /// <summary>Everything a document may carry — resolved by the endpoint from the same canonical
    /// sources the Passport summary and the existing PDF use. No email, no internal ids.</summary>
    public sealed class DocData
    {
        public string Name = "";
        /// <summary>The canonical PCI Student Number — printed as text, NEVER encoded in the QR.</summary>
        public string StudentNumber = "";
        /// <summary>The ONLY string the QR encodes: the opaque HTTPS verification URL.</summary>
        public string VerifyUrl = "";
        /// <summary>Human-typeable fallback printed beside the QR (scheme stripped) — a reader who
        /// cannot scan goes here and verifies by the Student Number on the document.</summary>
        public string ShortVerifyUrl = "";
        /// <summary>Public headline (published evidence counts) — only ever derived from data the
        /// published Passport already discloses; null renders nothing.</summary>
        public string? Headline;
        public string? ExpiresOn;
    }

    // Brand colours in PDF RGB — the same constants the existing Passport PDF draws with.
    const string Noir = "0.055 0.082 0.145";
    const string Crimson = "0.757 0.200 0.161";
    const string Gilt = "0.784 0.635 0.294";
    const string GiltFaint = "0.855 0.780 0.600";

    // ───────────────────────────── wallet card (2 pages: front + back) ─────────────────────────

    /// <summary>The CR80 wallet card: noir front (identity), white back (verification QR).
    /// Deterministic for a given input.</summary>
    public static byte[] WalletCard(DocData d)
    {
        const double W = WalletWidthPt, Hc = WalletHeightPt;

        // ── front: name, PCI Student Number, Passport status, PCI World wordmark ──
        var f = new StringBuilder();
        f.Append($"{Noir} rg 0 0 {W:0.##} {Hc:0.##} re f ");
        f.Append($"{Gilt} RG 0.8 w 6 6 {W - 12:0.##} {Hc - 12:0.##} re S ");
        f.Append($"{GiltFaint} RG 0.4 w 9 9 {W - 18:0.##} {Hc - 18:0.##} re S ");
        f.Append($"{Crimson} rg 18 {Hc - 26:0.##} 24 2.2 re f ");
        Text(f, "FROM THE PROJECT CONTROLS INSTITUTE", 48, Hc - 27.6, 4.2, bold: false, 0.72, 0.78, 0.86, spacing: 0.8);
        Text(f, "PCI WORLD", 18, Hc - 50, 15, bold: true, 1, 1, 1);
        Text(f, "PASSPORT — PRACTICE EVIDENCE", 18, Hc - 61, 5.2, bold: false, 0.784, 0.635, 0.294, spacing: 1.5);
        Text(f, Clip(d.Name, 26), 18, 61, 12, bold: true, 0.97, 0.95, 0.91);
        Text(f, "PCI STUDENT NUMBER", 18, 48, 4.2, bold: true, 0.72, 0.78, 0.86, spacing: 1.2);
        Text(f, d.StudentNumber, 18, 36, 9.5, bold: true, 0.784, 0.635, 0.294);
        var status = "PASSPORT PUBLISHED" + (d.ExpiresOn is null ? "" : $" - LINK EXPIRES {d.ExpiresOn}");
        Text(f, status, 18, 16, 4.6, bold: false, 0.72, 0.78, 0.86, spacing: 0.7);
        Seal(f, W - 40, 42, 19);

        // ── back: verification QR ≥ 22 mm incl. quiet zone, instruction, disclaimer, short URL ──
        var b = new StringBuilder();
        b.Append($"1 1 1 rg 0 0 {W:0.##} {Hc:0.##} re f ");
        b.Append($"{GiltFaint} RG 0.6 w 6 6 {W - 12:0.##} {Hc - 12:0.##} re S ");
        // The QR square: nothing else is EVER drawn inside it or its quiet zone.
        DrawQr(b, d.VerifyUrl, 12, (Hc - WalletQrPt) / 2, WalletQrPt);
        const double cx = 90;   // text column, safely clear of the QR square (12 + 68.03 ≈ 80)
        Text(b, "VERIFY THIS PASSPORT", cx, 128, 6.2, bold: true, 0.055, 0.082, 0.145, spacing: 1.1);
        Text(b, "Scan the code to open the live", cx, 116, 5.6, bold: false, 0.28, 0.33, 0.41);
        Text(b, "Passport record this card refers to.", cx, 108, 5.6, bold: false, 0.28, 0.33, 0.41);
        Text(b, Clip(d.ShortVerifyUrl, 44), cx, 94, 6, bold: true, 0.11, 0.31, 0.85);
        Text(b, "or verify the PCI Student Number", cx, 82, 5.2, bold: false, 0.28, 0.33, 0.41);
        Text(b, "shown on the front of this card.", cx, 74, 5.2, bold: false, 0.28, 0.33, 0.41);
        // Live-status disclaimer — the record, not the card, is the authority.
        Text(b, "The live record is the authority. This", cx, 58, 5.2, bold: false, 0.45, 0.50, 0.57);
        Text(b, "card is a copy - its owner can withdraw", cx, 50, 5.2, bold: false, 0.45, 0.50, 0.57);
        Text(b, "or expire the verification link at any time.", cx, 42, 5.2, bold: false, 0.45, 0.50, 0.57);
        Text(b, "PCI World practice evidence - not a PCI certification, examination result or licence.",
            12, 14, 4.6, bold: false, 0.45, 0.50, 0.57);

        return Assemble(new[] { (W, Hc, f.ToString()), (W, Hc, b.ToString()) });
    }

    // ───────────────────────────── event badge (1 page, 4×6in portrait) ────────────────────────

    /// <summary>The 4×6in portrait event badge: large verification QR ≥ 30 mm, "Scan to verify".
    /// A verification document — NOT an event entry pass, and it says so on its face.</summary>
    public static byte[] EventBadge(DocData d)
    {
        const double W = BadgeWidthPt, Hc = BadgeHeightPt;
        var c = new StringBuilder();

        // header band, the brand's noir with the gilt seal
        c.Append($"{Noir} rg 0 {Hc - 95:0.##} {W:0.##} 95 re f ");
        c.Append($"{Crimson} rg 22 {Hc - 24:0.##} 26 2.6 re f ");
        Text(c, "FROM THE PROJECT CONTROLS INSTITUTE", 54, Hc - 25.8, 4.8, bold: false, 0.72, 0.78, 0.86, spacing: 1.0);
        Text(c, "PCI WORLD", 22, Hc - 57, 21, bold: true, 1, 1, 1);
        Text(c, "PASSPORT — VERIFIED PRACTICE EVIDENCE", 22, Hc - 73, 6.5, bold: false, 0.72, 0.78, 0.86, spacing: 1.8);
        Seal(c, W - 38, Hc - 47, 24);
        c.Append($"{GiltFaint} RG 0.7 w 8 8 {W - 16:0.##} {Hc - 16:0.##} re S ");

        Text(c, Clip(d.Name, 24), 22, 305, 19, bold: true, 0.06, 0.09, 0.16);
        Text(c, "PCI STUDENT NUMBER", 22, 288, 5.5, bold: true, 0.28, 0.33, 0.41, spacing: 1.4);
        Text(c, d.StudentNumber, 22, 274, 12, bold: true, 0.06, 0.09, 0.16);
        if (d.Headline is { Length: > 0 } headline)
        {
            var lines = Wrap(headline, 52);
            var hy = 252.0;
            foreach (var line in lines.Take(2)) { Text(c, line, 22, hy, 8.5, bold: false, 0.28, 0.33, 0.41); hy -= 12; }
        }

        // the verification block: caption above, QR centred, URL and disclaimer below
        Text(c, "SCAN TO VERIFY", 108, 216, 8, bold: true, 0.055, 0.082, 0.145, spacing: 2.0);
        var qx = (W - BadgeQrPt) / 2;
        c.Append($"{GiltFaint} RG 0.8 w {qx - 6:0.##} 106 {BadgeQrPt + 12:0.##} {BadgeQrPt + 12:0.##} re S ");
        DrawQr(c, d.VerifyUrl, qx, 112, BadgeQrPt);
        Text(c, Clip(d.ShortVerifyUrl, 48), 60, 92, 7, bold: true, 0.11, 0.31, 0.85);
        Text(c, "The QR opens the live public Passport verification record.", 42, 78, 6.5, bold: false, 0.28, 0.33, 0.41);
        Text(c, "The live record is the authority. This badge is a printed copy -", 36, 62, 6, bold: false, 0.45, 0.50, 0.57);
        Text(c, "its owner can withdraw or expire the verification link at any time.", 34, 53, 6, bold: false, 0.45, 0.50, 0.57);
        Text(c, "Public Passport verification only - this badge is not an event entry pass.", 30, 38, 6.2, bold: true, 0.28, 0.33, 0.41);
        var op = Wrap(WorldPages.OperatedBy, 74);
        var oy = 24.0;
        foreach (var line in op.Take(2)) { Text(c, line, 22, oy, 5.2, bold: false, 0.45, 0.50, 0.57); oy -= 8; }

        return Assemble(new[] { (W, Hc, c.ToString()) });
    }

    // ───────────────────────────── drawing primitives (WorldPassport's approach) ───────────────

    static void Text(StringBuilder cs, string s, double x, double y, double size, bool bold,
        double r, double g, double b, double spacing = 0)
    {
        cs.Append($"{r:0.###} {g:0.###} {b:0.###} rg BT /{(bold ? "F2" : "F1")} {size:0.##} Tf ")
          .Append(spacing != 0 ? $"{spacing:0.##} Tc " : "")
          .Append($"1 0 0 1 {x:0.##} {y:0.##} Tm ({Esc(s)}) Tj ET ");
        if (spacing != 0) cs.Append("BT 0 Tc ET ");
    }

    /// <summary>The verification QR. Same QRCoder call and ECC level (M) as WorldPassport. The
    /// module matrix from QRCoder 1.6 already includes the 4-module quiet zone on every side, so
    /// the printed module size is size/(matrix count) and the quiet zone sits inside the reserved
    /// square by construction. Solid black on a white field; nothing overlaps the square.</summary>
    static void DrawQr(StringBuilder cs, string payload, double x, double y, double size)
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode(string.IsNullOrWhiteSpace(payload) ? "https://" : payload, QRCodeGenerator.ECCLevel.M);
        var m = data.ModuleMatrix;
        var n = m.Count;                 // version modules + 8 (the built-in 4-module quiet zone)
        var mod = size / n;
        cs.Append($"1 1 1 rg {x:0.##} {y:0.##} {size:0.##} {size:0.##} re f ");
        cs.Append("0 0 0 rg ");
        for (var row = 0; row < n; row++)
            for (var col = 0; col < n; col++)
                if (m[row][col])
                    cs.Append($"{x + col * mod:0.###} {y + (n - 1 - row) * mod:0.###} {mod + 0.05:0.###} {mod + 0.05:0.###} re f ");
    }

    /// <summary>The gilt seal, scaled down from the A4 Passport PDF: two rings, tick marks, the
    /// monogram and the crimson underline. Decorative only.</summary>
    static void Seal(StringBuilder cs, double cx, double cy, double r)
    {
        cs.Append($"{Gilt} RG 0.9 w ");
        Circle(cs, cx, cy, r);
        cs.Append($"{Gilt} RG 0.4 w ");
        Circle(cs, cx, cy, r * 0.86);
        cs.Append($"{Gilt} RG 0.6 w ");
        for (var i = 0; i < 12; i++)
        {
            var a = Math.PI * i / 6.0;
            var (dx, dy) = (Math.Cos(a), Math.Sin(a));
            cs.Append($"{cx + dx * r * 0.90:0.##} {cy + dy * r * 0.90:0.##} m ")
              .Append($"{cx + dx * r * 0.98:0.##} {cy + dy * r * 0.98:0.##} l S ");
        }
        var ms = r * 0.42;
        Text(cs, "PW", cx - ms * 0.75, cy - ms * 0.3, ms, bold: true, 0.784, 0.635, 0.294);
        cs.Append($"{Crimson} rg {cx - r * 0.18:0.##} {cy - r * 0.3:0.##} {r * 0.36:0.##} 1.2 re f ");
    }

    static void Circle(StringBuilder cs, double cx, double cy, double r)
    {
        var k = 0.552284749831 * r;
        cs.Append($"{cx + r:0.##} {cy:0.##} m ")
          .Append($"{cx + r:0.##} {cy + k:0.##} {cx + k:0.##} {cy + r:0.##} {cx:0.##} {cy + r:0.##} c ")
          .Append($"{cx - k:0.##} {cy + r:0.##} {cx - r:0.##} {cy + k:0.##} {cx - r:0.##} {cy:0.##} c ")
          .Append($"{cx - r:0.##} {cy - k:0.##} {cx - k:0.##} {cy - r:0.##} {cx:0.##} {cy - r:0.##} c ")
          .Append($"{cx + k:0.##} {cy - r:0.##} {cx + r:0.##} {cy - k:0.##} {cx + r:0.##} {cy:0.##} c S ");
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

    /// <summary>Wrap one or more content streams into a minimal, valid PDF with a per-page
    /// MediaBox (Helvetica, no embedding). The multi-page generalisation of the WorldPassport /
    /// CertPdf assembler; pure ASCII, so string length equals byte offset and output stays
    /// byte-deterministic.</summary>
    static byte[] Assemble(IReadOnlyList<(double W, double H, string Content)> pages)
    {
        var n = pages.Count;
        // Object layout: 1 Catalog, 2 Pages, then per page i: (3+2i) Page + (4+2i) Contents,
        // then the two font objects (3+2n regular, 4+2n bold).
        var fontRegular = 3 + 2 * n;
        var fontBold = 4 + 2 * n;
        var objects = new List<string?>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            $"<< /Type /Pages /Kids [{string.Join(" ", Enumerable.Range(0, n).Select(i => $"{3 + 2 * i} 0 R"))}] /Count {n} >>",
        };
        for (var i = 0; i < n; i++)
        {
            objects.Add($"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {pages[i].W:0.##} {pages[i].H:0.##}] " +
                        $"/Resources << /Font << /F1 {fontRegular} 0 R /F2 {fontBold} 0 R >> >> /Contents {4 + 2 * i} 0 R >>");
            objects.Add(null);   // the page's content stream, written below
        }
        objects.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>");
        objects.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>");

        var body = new StringBuilder("%PDF-1.4\n");
        var offsets = new List<int>();
        for (var i = 0; i < objects.Count; i++)
        {
            offsets.Add(body.Length);
            body.Append(i + 1).Append(" 0 obj\n");
            if (objects[i] is null)
            {
                var content = pages[(i - 3) / 2].Content;
                body.Append($"<< /Length {content.Length} >>\nstream\n").Append(content).Append("\nendstream\n");
            }
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
