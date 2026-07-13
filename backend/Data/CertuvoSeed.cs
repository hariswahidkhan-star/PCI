using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// Boot-time starter practice pack for Certuvo (Phase 8). Loads certuvo_seed.json (scenario-based
/// MCQs across the BoK domains, each with four options, a correct answer and a teaching explanation)
/// into sample_questions as PRACTICE rows (is_practice=1) — separate from the secure examination bank —
/// so the study/practice engine works out of the box with no manual content authoring.
///
/// Applied ONCE per seed version (site_settings 'certuvo_seed_version'), INSERT OR IGNORE keyed on the
/// (UNIQUE) question text — so admin edits and deletions are never overwritten on a later restart.
/// </summary>
public static class CertuvoSeed
{
    const int Version = 1;

    public static void Apply(Db db)
    {
        try
        {
            var applied = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='certuvo_seed_version'");
            if (int.TryParse(applied, out var v) && v >= Version) return;

            var path = File.Exists("certuvo_seed.json") ? "certuvo_seed.json"
                     : Path.Combine(AppContext.BaseDirectory, "certuvo_seed.json");
            if (!File.Exists(path)) return;

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("questions", out var qs) || qs.ValueKind != JsonValueKind.Array) return;
            int n = 0, sort = 1000;   // high sort_order so seeded practice trails any hand-authored rows
            db.Transaction(() =>
            {
                foreach (var q in qs.EnumerateArray())
                {
                    string? S(string k) => q.TryGetProperty(k, out var el) && el.ValueKind == JsonValueKind.String ? el.GetString() : null;
                    var question = S("question");
                    if (string.IsNullOrWhiteSpace(question)) continue;
                    var ai = q.TryGetProperty("answer_index", out var aiEl) && aiEl.TryGetInt32(out var a) ? a : 0;
                    if (ai < 0 || ai > 3) ai = 0;
                    db.Execute(@"INSERT OR IGNORE INTO sample_questions
                        (question,option_a,option_b,option_c,option_d,answer_index,domain,explanation,difficulty,is_practice,published,sort_order,certification_id)
                        VALUES(?,?,?,?,?,?,?,?,?,1,1,?,1)",
                        question, S("option_a"), S("option_b"), S("option_c"), S("option_d"), ai,
                        S("domain"), S("explanation"), S("difficulty"), sort++);
                    n++;
                }
                db.Execute("DELETE FROM site_settings WHERE skey='certuvo_seed_version'");
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('certuvo_seed_version',?)", Version.ToString());
            });
            Console.WriteLine($"[seed] Certuvo starter practice pack loaded: {n} questions");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] Certuvo practice pack skipped: {e.Message}"); }
    }
}
