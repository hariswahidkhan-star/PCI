using PCI.Backend.Core;
using PCI.Backend.Data;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Free Templates Library tests. The library is downloadable working product, so the two things that would
/// embarrass us in a user's spreadsheet are pinned here: the seed must be idempotent (a second boot must not
/// duplicate a template), and every generated file must be inert when opened — no cell may begin with a
/// formula trigger, because a CSV that executes on open is indistinguishable from a CSV-injection payload
/// (SEC-2 / CWE-1236). Coverage against the Lab engines is asserted too, since the library's whole premise is
/// that a learner can practise a technique and then download the artefact that runs it.
/// </summary>
public class TemplatesLibraryTests : IClassFixture<DbFixture>
{
    readonly Db _db;
    public TemplatesLibraryTests(DbFixture f) => _db = f.Db;

    static List<Dictionary<string, object?>> Templates(Db db) =>
        db.Query("SELECT doc_group,title,description,category,filename,mime,storage_ref,status FROM public_documents WHERE category='templates'");

    [Fact]
    public void Seeding_is_idempotent_and_publishes_a_template_for_every_lab_engine()
    {
        TemplatesLibrarySeed.Ensure(_db);
        var first = Templates(_db);
        Assert.True(first.Count >= 15, $"expected a substantial template library, got {first.Count}");

        TemplatesLibrarySeed.Ensure(_db);   // a second boot must add nothing
        var second = Templates(_db);
        Assert.Equal(first.Count, second.Count);
        Assert.Equal(second.Count, second.Select(r => H.Str(r["doc_group"])).Distinct().Count());

        foreach (var r in second)
        {
            Assert.Equal("published", H.Str(r["status"]));
            Assert.Equal("text/csv", H.Str(r["mime"]));
            Assert.EndsWith(".csv", H.Str(r["filename"]) ?? "");
            Assert.False(string.IsNullOrWhiteSpace(H.Str(r["description"])), $"{H.Str(r["doc_group"])}: description");
        }
    }

    /// <summary>
    /// The library's premise is that a learner practises a technique in the Lab and then downloads the sheet
    /// that runs it, so the engine mapping is load-bearing: GET /api/me/lab/templates joins on it. Every
    /// declared engine must be a real one, no engine may claim two sheets (the Lab would have to pick), and
    /// the general artefacts — the ones with no engine behind them — must still be present.
    /// </summary>
    [Fact]
    public void Engine_mapping_is_sound_and_general_artefacts_are_included()
    {
        var byEngine = TemplatesLibrarySeed.Catalogue
            .Where(t => t.Engine.Length > 0)
            .GroupBy(t => t.Engine)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var (engine, count) in byEngine)
        {
            Assert.True(SimCalc.KnownTasks.Contains(engine), $"template maps to unknown engine '{engine}'");
            Assert.True(count == 1, $"engine '{engine}' has {count} templates; the Lab link must be unambiguous");
        }
        Assert.True(byEngine.Count >= 15, $"only {byEngine.Count} engines have a working sheet");
        Assert.True(TemplatesLibrarySeed.Catalogue.Count(t => t.Engine.Length == 0) >= 5,
            "the library should also carry general artefacts that no Lab engine grades");

        Assert.Equal("evm", TemplatesLibrarySeed.ForEngine("evm")?.Engine);
        Assert.Null(TemplatesLibrarySeed.ForEngine("no_such_engine"));
        Assert.Null(TemplatesLibrarySeed.ForEngine(null));
        Assert.Null(TemplatesLibrarySeed.ForEngine(""));
    }

    [Fact]
    public void Every_template_file_is_inert_when_opened_in_a_spreadsheet()
    {
        TemplatesLibrarySeed.Ensure(_db);
        foreach (var r in Templates(_db))
        {
            var group = H.Str(r["doc_group"]);
            var stored = Storage.Get(H.Str(r["storage_ref"]) ?? "");
            Assert.NotNull(stored);
            var bytes = stored!.Value.bytes;
            Assert.NotNull(bytes);
            var text = System.Text.Encoding.UTF8.GetString(bytes!);
            Assert.Contains(",", text);

            foreach (var line in text.Split('\n'))
            {
                if (line.Length == 0) continue;
                foreach (var cell in SplitCsv(line))
                {
                    var v = cell.Trim();
                    if (v.Length == 0) continue;
                    // A leading = + - @ is only safe when the cell is a genuine number (-5000 is data, not a formula).
                    if (v[0] is '=' or '+' or '-' or '@')
                        Assert.True(double.TryParse(v, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out _),
                            $"{group}: cell '{v}' starts with a formula trigger and is not a number");
                }
            }
        }
    }

    // Minimal RFC-4180 splitter: enough to walk the cells of our own generated files.
    static IEnumerable<string> SplitCsv(string line)
    {
        var cur = new System.Text.StringBuilder();
        var quoted = false;
        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (quoted)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"') { cur.Append('"'); i++; }
                else if (c == '"') quoted = false;
                else cur.Append(c);
            }
            else if (c == '"') quoted = true;
            else if (c == ',') { yield return cur.ToString(); cur.Clear(); }
            else cur.Append(c);
        }
        yield return cur.ToString();
    }
}
