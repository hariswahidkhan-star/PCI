using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Learning-content translations (multilingual launch): exam/practice items, sim-lab scenarios and
/// books/study materials, translated per field per language in <c>item_i18n</c> and overlaid onto
/// the English source rows at serve time. The same rules as the website's <see cref="I18nContent"/>:
/// human-reviewed or provider-translated strings stored server-side, English always the fallback
/// (a missing or empty translation leaves the English value untouched), nothing machine-translated
/// at request time.
///
/// Integrity rules this class enforces (they are the reason translation lives HERE and not in ad-hoc
/// endpoint code):
///   • only display text is ever overlaid — ids, answer indexes, scores and config are untouched;
///   • an <c>options</c> translation is applied ONLY when it is a JSON array with exactly the same
///     number of entries as the English options, so a stored answer_index can never point at a
///     different choice in one language than another;
///   • unknown languages (and "en") are no-ops, so every caller can pass client input unfiltered.
/// </summary>
public static class ItemI18n
{
    /// <summary>Entities item_i18n rows may describe (also the admin translate surface).</summary>
    public static readonly string[] Entities = { "question", "scenario", "book" };

    /// <summary>Resolve a client-requested content language: explicit value → the site cookie
    /// (shared with the public website switcher) → English. Unknown values collapse to "en".</summary>
    public static string Resolve(HttpContext ctx, string? explicitLang)
    {
        var l = (explicitLang ?? "").Trim().ToLowerInvariant();
        if (l.Length == 0) l = (ctx.Request.Cookies[I18nContent.Cookie] ?? "").Trim().ToLowerInvariant();
        return l.Length > 0 && l != "en" && Translator.LangNames.ContainsKey(l) ? l : "en";
    }

    /// <summary>True when translations should be looked up at all for this language.</summary>
    public static bool Applies(string lang) => lang != "en" && Translator.LangNames.ContainsKey(lang);

    /// <summary>Fetch the stored translations for a set of items in one query:
    /// (entity_id, field) → value.</summary>
    public static Dictionary<(long id, string field), string> For(Db db, string entity, string lang, IReadOnlyCollection<long> ids)
    {
        var map = new Dictionary<(long, string), string>();
        if (!Applies(lang) || ids.Count == 0) return map;
        // ids are server-derived longs (never raw client input), so an IN(...) list is safe here and
        // keeps this a single round-trip for a whole exam paper / catalogue page.
        var inList = string.Join(",", ids.Distinct());
        foreach (var r in db.Query($"SELECT entity_id,field,value FROM item_i18n WHERE entity=? AND lang=? AND entity_id IN ({inList})", entity, lang))
        {
            var v = H.Str(r["value"]);
            if (v is { Length: > 0 }) map[(H.L(r["entity_id"]), H.Str(r["field"]) ?? "")] = v;
        }
        return map;
    }

    /// <summary>Overlay translated display fields onto DB row dictionaries in place. Rows must carry
    /// an <c>id</c> column. Fields with no stored translation keep their English value.</summary>
    public static void Overlay(Db db, string entity, string lang, IEnumerable<Dictionary<string, object?>> rows, params string[] fields)
    {
        if (!Applies(lang)) return;
        var list = rows as IList<Dictionary<string, object?>> ?? rows.ToList();
        if (list.Count == 0) return;
        var tr = For(db, entity, lang, list.Select(r => H.L(r["id"])).ToList());
        if (tr.Count == 0) return;
        foreach (var row in list)
        {
            var id = H.L(row["id"]);
            foreach (var f in fields)
                if (tr.TryGetValue((id, f), out var v))
                    row[f] = v;
        }
    }

    /// <summary>Translated options for one item, applied only when the stored value is a JSON array
    /// of exactly <paramref name="englishCount"/> strings — otherwise null (keep English), so an
    /// answer key can never drift between languages.</summary>
    public static List<string>? Options(string? stored, int englishCount)
    {
        if (string.IsNullOrWhiteSpace(stored) || englishCount <= 0) return null;
        try
        {
            var parsed = JsonSerializer.Deserialize<List<string>>(stored);
            if (parsed is null || parsed.Count != englishCount) return null;
            if (parsed.Any(string.IsNullOrWhiteSpace)) return null;
            return parsed;
        }
        catch { return null; }
    }

    /// <summary>Localized projection of question rows into the served paper shape
    /// (id, question, options, domain) — shared by the exam and Certuvo practice endpoints so both
    /// papers follow the same translation rules. Scoring never sees these values: answer keys are
    /// derived server-side from the source rows, and an options translation with the wrong entry
    /// count is ignored (English served) rather than ever re-ordering choices.</summary>
    public static List<Dictionary<string, object?>> Questions(Db db, string lang, List<Dictionary<string, object?>> rows)
    {
        var tr = For(db, "question", lang, rows.Select(r => H.L(r["id"])).ToList());
        return rows.Select(r =>
        {
            var id = H.L(r["id"]);
            var opts = H.OptionsFor(r);
            if (tr.TryGetValue((id, "options"), out var to) && Options(to, opts.Count) is { } localized) opts = localized;
            return new Dictionary<string, object?>
            {
                ["id"] = r["id"],
                ["question"] = tr.TryGetValue((id, "question"), out var tq) ? tq : H.Str(r["question"]),
                ["options"] = opts,
                ["domain"] = tr.TryGetValue((id, "domain"), out var td) ? td : H.Str(r["domain"]),
            };
        }).ToList();
    }

    /// <summary>Upsert one translation (admin hand-edit or provider output). DELETE+INSERT like
    /// I18nContent.Upsert, so it is provider-agnostic without a dialect-specific upsert.</summary>
    public static void Upsert(Db db, string entity, long id, string lang, string field, string value)
    {
        db.Execute("DELETE FROM item_i18n WHERE entity=? AND entity_id=? AND lang=? AND field=?", entity, id, lang, field);
        if (!string.IsNullOrEmpty(value))
            db.Execute("INSERT INTO item_i18n(entity,entity_id,lang,field,value) VALUES(?,?,?,?,?)", entity, id, lang, field, value);
    }
}
