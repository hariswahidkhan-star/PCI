using System.IO.Compression;
using System.Text;

namespace PCI.Backend.Core;

/// <summary>
/// Minimal, dependency-free XLSX writer for the branded template downloads (Templates Library).
/// Renders a CSV body as a styled institutional workbook: a navy brand band carrying the PCI
/// emblem and wordmark, the template title + summary + issue metadata, then the data itself with
/// a blue header row, zebra rows, sized columns and frozen panes. Everything is written as OOXML
/// inline strings (never formulas), so admin-authored cell text can't become executable spreadsheet
/// content — the same CSV-injection stance the raw CSV path takes (SEC-2 / CWE-1236).
///
/// Deliberately simple, like SimplePdf: one sheet, no shared strings, no theme part. The package
/// is plain ZIP + XML from the BCL — no OpenXML SDK dependency.
/// </summary>
public static class SimpleXlsx
{
    /// <summary>Render one branded worksheet from CSV text. <paramref name="logoPng"/> may be null
    /// (the band simply renders without the emblem — fail-open, never fail the download).</summary>
    public static byte[] BuildBranded(string title, string? summary, string? meta, string csvBody, byte[]? logoPng)
    {
        var rows = ParseCsv(csvBody);
        var colCount = Math.Max(1, rows.Count == 0 ? 1 : rows.Max(r => r.Count));

        // Column widths from content (Excel width units ≈ characters), clamped to stay tidy.
        var widths = new double[colCount];
        for (var c = 0; c < colCount; c++)
        {
            var w = 10.0;
            foreach (var r in rows)
                if (c < r.Count) w = Math.Max(w, Math.Min(46, r[c].Length + 3));
            widths[c] = w;
        }

        var lastCol = ColRef(colCount - 1);
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
        sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
        sb.Append("<sheetViews><sheetView workbookViewId=\"0\" showGridLines=\"0\">");
        sb.Append("<pane ySplit=\"6\" topLeftCell=\"A7\" activePane=\"bottomLeft\" state=\"frozen\"/>");
        sb.Append("</sheetView></sheetViews>");
        sb.Append("<sheetFormatPr defaultRowHeight=\"15\"/>");
        sb.Append("<cols>");
        for (var c = 0; c < colCount; c++)
            sb.Append($"<col min=\"{c + 1}\" max=\"{c + 1}\" width=\"{widths[c].ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)}\" customWidth=\"1\"/>");
        sb.Append("</cols><sheetData>");

        // Row 1 — brand band (indent clears the anchored emblem), rows 2–4 — title / summary / meta.
        BandRow(sb, 1, "PROJECT CONTROLS INSTITUTE", 1, colCount, height: 34);
        BandRow(sb, 2, title, 2, colCount, height: 24);
        BandRow(sb, 3, summary ?? "", 3, colCount, height: 16);
        BandRow(sb, 4, meta ?? "", 3, colCount, height: 16);
        sb.Append("<row r=\"5\" ht=\"6\" customHeight=\"1\"/>");

        // Row 6 — column headers; rows 7+ — the data, zebra-striped.
        var header = rows.Count > 0 ? rows[0] : new List<string>();
        sb.Append("<row r=\"6\" ht=\"20\" customHeight=\"1\">");
        for (var c = 0; c < colCount; c++)
            Cell(sb, 6, c, c < header.Count ? header[c] : "", 4);
        sb.Append("</row>");
        for (var i = 1; i < rows.Count; i++)
        {
            var rn = 6 + i;
            var style = i % 2 == 0 ? 6 : 5;
            sb.Append($"<row r=\"{rn}\">");
            for (var c = 0; c < colCount; c++)
                Cell(sb, rn, c, c < rows[i].Count ? rows[i][c] : "", style);
            sb.Append("</row>");
        }
        sb.Append("</sheetData>");
        sb.Append($"<mergeCells count=\"4\"><mergeCell ref=\"A1:{lastCol}1\"/><mergeCell ref=\"A2:{lastCol}2\"/><mergeCell ref=\"A3:{lastCol}3\"/><mergeCell ref=\"A4:{lastCol}4\"/></mergeCells>");
        if (logoPng is not null) sb.Append("<drawing r:id=\"rId1\"/>");
        sb.Append("</worksheet>");
        var sheetXml = sb.ToString();

        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            var hasLogo = logoPng is not null;
            Add(zip, "[Content_Types].xml",
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                (hasLogo ? "<Default Extension=\"png\" ContentType=\"image/png\"/>" : "") +
                "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
                (hasLogo ? "<Override PartName=\"/xl/drawings/drawing1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.drawing+xml\"/>" : "") +
                "<Override PartName=\"/docProps/core.xml\" ContentType=\"application/vnd.openxmlformats-package.core-properties+xml\"/>" +
                "</Types>");
            Add(zip, "_rels/.rels",
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties\" Target=\"docProps/core.xml\"/>" +
                "</Relationships>");
            Add(zip, "docProps/core.xml",
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<cp:coreProperties xmlns:cp=\"http://schemas.openxmlformats.org/package/2006/metadata/core-properties\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\">" +
                $"<dc:title>{Esc(title)}</dc:title><dc:creator>Project Controls Institute</dc:creator>" +
                "</cp:coreProperties>");
            Add(zip, "xl/workbook.xml",
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                "<sheets><sheet name=\"Template\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>");
            Add(zip, "xl/_rels/workbook.xml.rels",
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
                "</Relationships>");
            Add(zip, "xl/styles.xml", StylesXml);
            Add(zip, "xl/worksheets/sheet1.xml", sheetXml);
            if (logoPng is not null)
            {
                Add(zip, "xl/worksheets/_rels/sheet1.xml.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/drawing\" Target=\"../drawings/drawing1.xml\"/>" +
                    "</Relationships>");
                // 34 px emblem anchored inside the 34 pt brand band (EMU: 9525 per px).
                Add(zip, "xl/drawings/drawing1.xml",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<xdr:wsDr xmlns:xdr=\"http://schemas.openxmlformats.org/drawingml/2006/spreadsheetDrawing\" xmlns:a=\"http://schemas.openxmlformats.org/drawingml/2006/main\">" +
                    "<xdr:oneCellAnchor>" +
                    "<xdr:from><xdr:col>0</xdr:col><xdr:colOff>57150</xdr:colOff><xdr:row>0</xdr:row><xdr:rowOff>47625</xdr:rowOff></xdr:from>" +
                    "<xdr:ext cx=\"323850\" cy=\"323850\"/>" +
                    "<xdr:pic><xdr:nvPicPr><xdr:cNvPr id=\"1\" name=\"PCI emblem\"/><xdr:cNvPicPr/></xdr:nvPicPr>" +
                    "<xdr:blipFill><a:blip xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\" r:embed=\"rId1\"/><a:stretch><a:fillRect/></a:stretch></xdr:blipFill>" +
                    "<xdr:spPr><a:xfrm><a:off x=\"0\" y=\"0\"/><a:ext cx=\"323850\" cy=\"323850\"/></a:xfrm><a:prstGeom prst=\"rect\"><a:avLst/></a:prstGeom></xdr:spPr>" +
                    "</xdr:pic><xdr:clientData/></xdr:oneCellAnchor></xdr:wsDr>");
                Add(zip, "xl/drawings/_rels/drawing1.xml.rels",
                    "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/image\" Target=\"../media/image1.png\"/>" +
                    "</Relationships>");
                var entry = zip.CreateEntry("xl/media/image1.png", CompressionLevel.Fastest);
                using var s = entry.Open();
                s.Write(logoPng, 0, logoPng.Length);
            }
        }
        return ms.ToArray();
    }

    static void Add(ZipArchive zip, string path, string xml)
    {
        var entry = zip.CreateEntry(path, CompressionLevel.Fastest);
        using var s = entry.Open();
        var bytes = Encoding.UTF8.GetBytes(xml);
        s.Write(bytes, 0, bytes.Length);
    }

    // Palette matches assets/styles.css tokens: ink #0F172A, blue #1D4ED8, line #E3E8EF, canvas #F4F7FB.
    const string StylesXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
        "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
        "<fonts count=\"5\">" +
        "<font><sz val=\"11\"/><color rgb=\"FF1F2937\"/><name val=\"Calibri\"/></font>" +
        "<font><b/><sz val=\"12\"/><color rgb=\"FFFFFFFF\"/><name val=\"Calibri\"/></font>" +
        "<font><b/><sz val=\"15\"/><color rgb=\"FF0F172A\"/><name val=\"Calibri\"/></font>" +
        "<font><sz val=\"10\"/><color rgb=\"FF64748B\"/><name val=\"Calibri\"/></font>" +
        "<font><b/><sz val=\"11\"/><color rgb=\"FFFFFFFF\"/><name val=\"Calibri\"/></font>" +
        "</fonts>" +
        "<fills count=\"5\">" +
        "<fill><patternFill patternType=\"none\"/></fill>" +
        "<fill><patternFill patternType=\"gray125\"/></fill>" +
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF0F172A\"/></patternFill></fill>" +
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF1D4ED8\"/></patternFill></fill>" +
        "<fill><patternFill patternType=\"solid\"><fgColor rgb=\"FFF4F7FB\"/></patternFill></fill>" +
        "</fills>" +
        "<borders count=\"2\">" +
        "<border><left/><right/><top/><bottom/><diagonal/></border>" +
        "<border><left/><right/><top/><bottom style=\"thin\"><color rgb=\"FFE3E8EF\"/></bottom><diagonal/></border>" +
        "</borders>" +
        "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
        "<cellXfs count=\"7\">" +
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +                                                        // 0 default
        "<xf numFmtId=\"0\" fontId=\"1\" fillId=\"2\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyAlignment=\"1\"><alignment horizontal=\"left\" vertical=\"center\" indent=\"3\"/></xf>" + // 1 brand band
        "<xf numFmtId=\"0\" fontId=\"2\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>" +   // 2 title
        "<xf numFmtId=\"0\" fontId=\"3\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyFont=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>" +   // 3 meta
        "<xf numFmtId=\"0\" fontId=\"4\" fillId=\"3\" borderId=\"1\" xfId=\"0\" applyFont=\"1\" applyFill=\"1\" applyBorder=\"1\" applyAlignment=\"1\"><alignment vertical=\"center\"/></xf>" + // 4 header
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/>" +                                                        // 5 body
        "<xf numFmtId=\"0\" fontId=\"0\" fillId=\"4\" borderId=\"0\" xfId=\"0\" applyFill=\"1\"/>" +                                        // 6 body zebra
        "</cellXfs>" +
        "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
        "</styleSheet>";

    /// <summary>A full-width single-text row (the style is written on every cell of the merge so the
    /// band fill paints edge to edge in every viewer).</summary>
    static void BandRow(StringBuilder sb, int rowNum, string text, int style, int colCount, int height)
    {
        sb.Append($"<row r=\"{rowNum}\" ht=\"{height}\" customHeight=\"1\">");
        Cell(sb, rowNum, 0, text, style);
        for (var c = 1; c < colCount; c++)
            sb.Append($"<c r=\"{ColRef(c)}{rowNum}\" s=\"{style}\"/>");
        sb.Append("</row>");
    }

    static void Cell(StringBuilder sb, int rowNum, int colIdx, string text, int style)
    {
        if (text.Length == 0) { sb.Append($"<c r=\"{ColRef(colIdx)}{rowNum}\" s=\"{style}\"/>"); return; }
        sb.Append($"<c r=\"{ColRef(colIdx)}{rowNum}\" s=\"{style}\" t=\"inlineStr\"><is><t xml:space=\"preserve\">{Esc(text)}</t></is></c>");
    }

    static string ColRef(int idx)
    {
        var s = "";
        idx++;
        while (idx > 0) { idx--; s = (char)('A' + idx % 26) + s; idx /= 26; }
        return s;
    }

    static string Esc(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var ch in s)
        {
            switch (ch)
            {
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                case '"': sb.Append("&quot;"); break;
                // Strip control chars that are illegal in XML 1.0 (keeps tab/newline out of cells too —
                // a template cell is a single line by construction).
                default: if (ch >= 0x20 || ch == '\t') sb.Append(ch); break;
            }
        }
        return sb.ToString();
    }

    /// <summary>Small RFC-4180-ish parser: quoted fields, doubled quotes, CRLF/LF rows.</summary>
    static List<List<string>> ParseCsv(string text)
    {
        var rows = new List<List<string>>();
        var row = new List<string>();
        var field = new StringBuilder();
        var inQuotes = false;
        for (var i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            if (inQuotes)
            {
                if (ch == '"')
                {
                    if (i + 1 < text.Length && text[i + 1] == '"') { field.Append('"'); i++; }
                    else inQuotes = false;
                }
                else field.Append(ch);
            }
            else if (ch == '"') inQuotes = true;
            else if (ch == ',') { row.Add(field.ToString()); field.Clear(); }
            else if (ch is '\n' or '\r')
            {
                if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n') i++;
                row.Add(field.ToString()); field.Clear();
                if (row.Count > 1 || row[0].Length > 0) rows.Add(row);
                row = new List<string>();
            }
            else field.Append(ch);
        }
        row.Add(field.ToString());
        if (row.Count > 1 || row[0].Length > 0) rows.Add(row);
        return rows;
    }
}
