using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// OPTIONAL demo LIVE-exam pack. Loads demo_exam_seed.json into sample_questions as SECURE-EXAM rows
/// (is_practice=0, published=1) so a fresh deployment can sit a full end-to-end examination — browser
/// runner (<c>/api/me/exam/start</c>) and desktop client (<c>/api/exam/authorize</c>) both draw from
/// this bank — without an admin first hand-authoring questions.
///
/// GATED on the environment variable <c>SEED_DEMO_EXAM=true</c>. It never runs by default, so a real
/// production instance is never contaminated with committed answers. The pack is generic, publicly-known
/// project-controls fundamentals — NOT the confidential live exam bank. For a genuine certification
/// launch, leave the flag unset and author the true live bank privately via Admin Console → Questions
/// (leave "Practice question" unchecked, tick "Published") so real answers stay out of source control.
///
/// Applied ONCE per seed version (site_settings 'demo_exam_seed_version'); INSERT OR IGNORE keyed on the
/// (UNIQUE) question text so admin edits/deletes are never overwritten on a later restart.
/// </summary>
public static class DemoExamSeed
{
    const int Version = 1;

    public static void Apply(Db db)
    {
        try
        {
            var enabled = string.Equals(Environment.GetEnvironmentVariable("SEED_DEMO_EXAM"), "true", StringComparison.OrdinalIgnoreCase);
            if (!enabled) return;

            var applied = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='demo_exam_seed_version'");
            if (int.TryParse(applied, out var v) && v >= Version) return;

            var path = File.Exists("demo_exam_seed.json") ? "demo_exam_seed.json"
                     : Path.Combine(AppContext.BaseDirectory, "demo_exam_seed.json");
            if (!File.Exists(path)) return;

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            if (!doc.RootElement.TryGetProperty("questions", out var qs) || qs.ValueKind != JsonValueKind.Array) return;
            int n = 0, sort = 1;   // low sort_order so the demo bank leads if an admin later adds live rows
            db.Transaction(() =>
            {
                foreach (var q in qs.EnumerateArray())
                {
                    string? S(string k) => q.TryGetProperty(k, out var el) && el.ValueKind == JsonValueKind.String ? el.GetString() : null;
                    var question = S("question");
                    if (string.IsNullOrWhiteSpace(question)) continue;
                    var ai = q.TryGetProperty("answer_index", out var aiEl) && aiEl.TryGetInt32(out var a) ? a : 0;
                    if (ai < 0 || ai > 3) ai = 0;
                    // is_practice=0 + published=1 → this is a LIVE secure-exam item for certification 1.
                    db.Execute(@"INSERT OR IGNORE INTO sample_questions
                        (question,option_a,option_b,option_c,option_d,answer_index,domain,explanation,difficulty,is_practice,published,sort_order,certification_id)
                        VALUES(?,?,?,?,?,?,?,?,?,0,1,?,1)",
                        question, S("option_a"), S("option_b"), S("option_c"), S("option_d"), ai,
                        S("domain"), S("explanation"), S("difficulty"), sort++);
                    n++;
                }
                db.Execute("DELETE FROM site_settings WHERE skey='demo_exam_seed_version'");
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('demo_exam_seed_version',?)", Version.ToString());
            });
            Console.WriteLine($"[seed] Demo LIVE-exam pack loaded (SEED_DEMO_EXAM=true): {n} questions — REMOVE before real certification launch.");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] Demo exam pack skipped: {e.Message}"); }
    }
}
