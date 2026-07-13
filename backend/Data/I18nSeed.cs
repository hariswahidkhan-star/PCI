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
    const int Version = 1;

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
            int n = 0;
            db.Transaction(() =>
            {
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
                        db.Execute("INSERT OR IGNORE INTO content_i18n(lang,scope,slug,ckey,cvalue) VALUES(?,?,?,?,?)",
                            lang, scope, slug, ckey, val);
                        n++;
                    }
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
