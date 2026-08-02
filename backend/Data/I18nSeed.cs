using System.Text.Json;
using PCI.Backend.Core;

namespace PCI.Backend.Data;

/// <summary>
/// Boot-time starter translations for the public website. Loads i18n_seed.json (shipped with the app:
/// navigation labels, shared site-wide elements, the complete homepage, and top pages' titles/headlines
/// in ko/ar/es/fr/zh/ru) into content_i18n so the language switcher and translated pages work out of the
/// box, with no provider key. Applied ONCE per seed version (site_settings 'i18n_seed_version'), with
/// INSERT OR IGNORE — so admin edits are never overwritten, and an admin 'Clear' is not undone on the
/// next restart. The long tail of pages is translated later via Admin → Translations (auto-translate).
/// </summary>
public static class I18nSeed
{
    const int Version = 4;   // v4: "PCI Global" → "PCI ai" brand rename (rekeyed wordmark/congress regions)

    public static void Apply(Db db)
    {
        try
        {
            var applied = db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey='i18n_seed_version'");
            if (int.TryParse(applied, out var v) && v >= Version) return;

            var path = File.Exists("i18n_seed.json") ? "i18n_seed.json"
                     : Path.Combine(AppContext.BaseDirectory, "i18n_seed.json");
            if (!File.Exists(path)) return;

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var langs = new[] { "ko", "ar", "es", "fr", "zh", "ru" };
            // Flatten to (lang,scope,slug,ckey,value) rows first, then bulk-insert in multi-row batches.
            // The full pack is ~84k rows; one INSERT per row makes boot slow enough to trip the MySQL
            // boot timeout, so batch ~300 rows per statement (~280 round-trips instead of ~84k).
            var rows = new List<(string lang, string scope, string slug, string ckey, string val)>();
            foreach (var el in doc.RootElement.EnumerateArray())
            {
                var scope = el.GetProperty("scope").GetString() ?? "";
                var slug = el.TryGetProperty("slug", out var sl) ? sl.GetString() ?? "" : "";
                var ckey = el.GetProperty("ckey").GetString() ?? "";
                if (scope.Length == 0 || ckey.Length == 0) continue;
                foreach (var lang in langs)
                {
                    if (!el.TryGetProperty(lang, out var tv)) continue;
                    var val = tv.GetString();
                    if (string.IsNullOrWhiteSpace(val)) continue;
                    rows.Add((lang, scope, slug, ckey, val));
                }
            }
            int n = rows.Count;
            const int chunk = 300;   // 300 rows * 5 params = 1500 bound params/statement — well within limits
            db.Transaction(() =>
            {
                for (int i = 0; i < rows.Count; i += chunk)
                {
                    var take = Math.Min(chunk, rows.Count - i);
                    var sb = new System.Text.StringBuilder("INSERT OR IGNORE INTO content_i18n(lang,scope,slug,ckey,cvalue) VALUES ");
                    var args = new object?[take * 5];
                    for (int j = 0; j < take; j++)
                    {
                        if (j > 0) sb.Append(',');
                        sb.Append("(?,?,?,?,?)");
                        var r = rows[i + j];
                        args[j * 5 + 0] = r.lang; args[j * 5 + 1] = r.scope; args[j * 5 + 2] = r.slug;
                        args[j * 5 + 3] = r.ckey; args[j * 5 + 4] = r.val;
                    }
                    db.Execute(sb.ToString(), args);
                }
                db.Execute("DELETE FROM site_settings WHERE skey='i18n_seed_version'");
                db.Execute("INSERT INTO site_settings(skey,svalue) VALUES('i18n_seed_version',?)", Version.ToString());
            });
            I18nContent.Bump();
            ListSections.Bump();
            Console.WriteLine($"[seed] starter translations loaded: {n} entries (6 languages)");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] i18n starter pack skipped: {e.Message}"); }
    }
}
