using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Resolves which students a document targets and materialises the concrete per-student grants
/// (<c>document_assignments</c>). Assignment is always resolved to an explicit list of user_ids and
/// written as rows, so a student's "My Documents" panel is a simple, fast lookup and every grant is
/// individually revocable and auditable. The document's <c>status</c> plus an <b>active</b> assignment
/// row are the two independent gates every download must pass.
/// </summary>
public static class DocAccess
{
    // Statuses in which a document is downloadable by an assigned student.
    public static readonly HashSet<string> LiveStatuses = new(StringComparer.OrdinalIgnoreCase) { "published", "active" };
    // Statuses in which an assigned student may at least SEE the document listed (locked/pending states).
    public static readonly HashSet<string> VisibleStatuses = new(StringComparer.OrdinalIgnoreCase) { "published", "active", "scheduled", "expired", "test" };

    /// <summary>Every audience the module can target, for validation + the admin picker.</summary>
    public static readonly string[] Types =
    {
        "all", "student", "students", "membership", "certification", "exam", "course",
        "passed", "honorary", "institution", "discount_code", "country", "custom_group", "test_users",
    };

    public static bool IsValidType(string? t) => t is not null && Array.IndexOf(Types, t) >= 0;

    // ---- config parsing ----
    static JsonElement Cfg(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return default;
        try { using var d = JsonDocument.Parse(json); return d.RootElement.Clone(); } catch { return default; }
    }
    static long? CfgLong(JsonElement c, string key)
    {
        if (c.ValueKind != JsonValueKind.Object || !c.TryGetProperty(key, out var v)) return null;
        if (v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var n)) return n;
        if (v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), out var n2)) return n2;
        return null;
    }
    static string? CfgStr(JsonElement c, string key)
    {
        if (c.ValueKind != JsonValueKind.Object || !c.TryGetProperty(key, out var v)) return null;
        return v.ValueKind == JsonValueKind.String ? v.GetString() : v.ValueKind == JsonValueKind.Number ? v.ToString() : null;
    }
    static List<long> CfgLongList(JsonElement c, string key)
    {
        var outp = new List<long>();
        if (c.ValueKind != JsonValueKind.Object || !c.TryGetProperty(key, out var v)) return outp;
        if (v.ValueKind == JsonValueKind.Array)
            foreach (var e in v.EnumerateArray())
            {
                if (e.ValueKind == JsonValueKind.Number && e.TryGetInt64(out var n)) outp.Add(n);
                else if (e.ValueKind == JsonValueKind.String && long.TryParse(e.GetString(), out var n2)) outp.Add(n2);
            }
        else if (v.ValueKind == JsonValueKind.String)
            foreach (var part in (v.GetString() ?? "").Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries))
                if (long.TryParse(part.Trim(), out var n)) outp.Add(n);
        return outp;
    }

    /// <summary>Resolve the target student set for a document as a de-duplicated list of user_ids.
    /// Broad group audiences exclude test accounts unless <paramref name="includeTest"/> is set; explicit
    /// student/list/test-user audiences are honoured as chosen. Only active accounts are returned.</summary>
    public static List<long> ResolveTargets(Db db, string type, string? configJson, bool includeTest)
    {
        var c = Cfg(configJson);
        var ids = new HashSet<long>();
        void AddRows(List<Dictionary<string, object?>> rows) { foreach (var r in rows) { var id = H.Ln(r.Values.First()); if (id is > 0) ids.Add(id.Value); } }
        // active-only, optional test-account filter — applied by the broad group queries below.
        var testFilter = includeTest ? "" : " AND COALESCE(is_test,0)=0";

        switch ((type ?? "all").ToLowerInvariant())
        {
            case "student":
                if (CfgLong(c, "user_id") is { } uid) ids.Add(uid);
                break;
            case "students":
            case "multiple":
            case "custom_group":
            case "group":
                foreach (var id in CfgLongList(c, "user_ids")) ids.Add(id);
                if (CfgLong(c, "user_id") is { } single) ids.Add(single);
                break;
            case "test_users":
                AddRows(db.Query("SELECT id FROM users WHERE status='active' AND is_test=1"));
                break;
            case "membership":
            {
                var mt = CfgStr(c, "membership_type");
                AddRows(string.IsNullOrWhiteSpace(mt)
                    ? db.Query($"SELECT DISTINCT u.id FROM users u JOIN memberships m ON m.user_id=u.id WHERE u.status='active' AND m.status='active'{testFilter}")
                    : db.Query($"SELECT DISTINCT u.id FROM users u JOIN memberships m ON m.user_id=u.id WHERE u.status='active' AND m.status='active' AND m.membership_type=?{testFilter}", mt));
                break;
            }
            case "certification":
            case "exam":
            case "course":
            {
                var certId = CfgLong(c, "certification_id");
                AddRows(certId is null
                    ? db.Query($"SELECT DISTINCT u.id FROM users u JOIN issued_credentials ic ON ic.user_id=u.id WHERE u.status='active' AND ic.status='active'{testFilter}")
                    : db.Query($"SELECT DISTINCT u.id FROM users u JOIN issued_credentials ic ON ic.user_id=u.id WHERE u.status='active' AND ic.status='active' AND COALESCE(ic.certification_id,1)=?{testFilter}", certId));
                break;
            }
            case "passed":
            case "passed_students":
                AddRows(db.Query($"SELECT DISTINCT u.id FROM users u JOIN issued_credentials ic ON ic.user_id=u.id WHERE u.status='active' AND ic.status='active'{testFilter}"));
                break;
            case "honorary":
                AddRows(db.Query($"SELECT DISTINCT u.id FROM users u JOIN honorary_awards h ON h.user_id=u.id WHERE u.status='active' AND h.status='active'{testFilter}"));
                break;
            case "institution":
            {
                var pid = CfgLong(c, "partner_id");
                if (pid is not null)
                    AddRows(db.Query($"SELECT DISTINCT u.id FROM users u JOIN code_redemptions r ON r.user_id=u.id JOIN discount_codes dc ON dc.id=r.code_id WHERE u.status='active' AND dc.partner_id=?{testFilter}", pid));
                break;
            }
            case "discount_code":
            case "code":
            {
                var code = CfgStr(c, "code");
                if (!string.IsNullOrWhiteSpace(code))
                    AddRows(db.Query($"SELECT DISTINCT u.id FROM users u JOIN code_redemptions r ON r.user_id=u.id WHERE u.status='active' AND UPPER(r.code)=?{testFilter}", code.Trim().ToUpperInvariant()));
                break;
            }
            case "country":
            {
                var country = CfgStr(c, "country");
                if (!string.IsNullOrWhiteSpace(country))
                    AddRows(db.Query($"SELECT DISTINCT u.id FROM users u JOIN student_profiles sp ON sp.user_id=u.id WHERE u.status='active' AND sp.country=?{testFilter}", country.Trim()));
                break;
            }
            case "all":
            default:
                AddRows(db.Query($"SELECT id FROM users WHERE status='active'{testFilter}"));
                break;
        }
        return ids.ToList();
    }

    /// <summary>Preview an audience without persisting anything: the resolved count and a small sample of
    /// names/emails so an admin can confirm "who will receive this" before publishing.</summary>
    public static (int count, List<object> sample) Preview(Db db, string type, string? configJson, bool includeTest, int sampleSize = 8)
    {
        var ids = ResolveTargets(db, type, configJson, includeTest);
        var sample = new List<object>();
        foreach (var id in ids.Take(sampleSize))
        {
            var u = db.QueryOne("SELECT id,first_name,last_name,email FROM users WHERE id=?", id);
            if (u is not null) sample.Add(new { id, name = ((H.Str(u["first_name"]) ?? "") + " " + (H.Str(u["last_name"]) ?? "")).Trim(), email = H.Str(u["email"]) });
        }
        return (ids.Count, sample);
    }

    /// <summary>Materialise the per-student grants for a document. Inserts an ACTIVE assignment for every
    /// currently-resolved target that does not already have a row (a previously revoked grant is left
    /// revoked — revocation is deliberate and persists). Returns the number of new grants written.</summary>
    public static int Materialize(Db db, long documentId, string type, string? configJson, bool includeTest, long? actorId)
    {
        var targets = ResolveTargets(db, type, configJson, includeTest);
        var added = 0;
        foreach (var uid in targets)
        {
            // Unique index ux_docassign(document_id,user_id) makes this idempotent: re-publishing never
            // duplicates a grant and never resurrects a revoked one.
            var (_, changes) = db.ExecuteWithChanges(
                "INSERT OR IGNORE INTO document_assignments(document_id,user_id,assignment_type,source,status,assigned_by) VALUES(?,?,?, 'auto', 'active', ?)",
                documentId, uid, type, actorId);
            if (changes > 0) added++;
        }
        return added;
    }

    /// <summary>Add a single explicit (manual) grant — e.g. a student-specific document created from the
    /// admin's student profile. Idempotent; if a revoked row exists it is re-activated (an explicit admin
    /// re-grant is an intentional override, unlike group re-resolution).</summary>
    public static void GrantOne(Db db, long documentId, long userId, long? actorId)
    {
        var (_, changes) = db.ExecuteWithChanges(
            "INSERT OR IGNORE INTO document_assignments(document_id,user_id,assignment_type,source,status,assigned_by) VALUES(?,?, 'manual', 'manual', 'active', ?)",
            documentId, userId, actorId);
        if (changes == 0)
            db.Execute("UPDATE document_assignments SET status='active', revoked_at=NULL, revoked_by=NULL, revoke_reason=NULL WHERE document_id=? AND user_id=?", documentId, userId);
    }

    /// <summary>The active assignment row for (document, user), or null. This is the per-student access
    /// gate — the download endpoints combine it with the document status/restriction/expiry checks.</summary>
    public static Dictionary<string, object?>? ActiveAssignment(Db db, long documentId, long userId) =>
        db.QueryOne("SELECT * FROM document_assignments WHERE document_id=? AND user_id=? AND status='active'", documentId, userId);
}
