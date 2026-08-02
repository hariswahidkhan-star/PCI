using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI AI Project Controls Simulation Lab — access/entitlement logic (Phase 1).
///
/// Access is computed LIVE from the student's existing PCI state (no separate account, no parallel
/// membership) — mirroring CertuvoLink.Eligible. A student reaches the Lab when the operator flag is on
/// AND they qualify under the configured `simlab_requires` rule (active membership and/or a paid exam
/// entitlement), OR they hold an explicit, in-window grant in `simulation_entitlements`
/// (admin/complimentary/sponsored/institution/marketing). Nothing here touches exam_attempts,
/// entitlements or credentials.
/// </summary>
public static class SimLab
{
    public static bool Enabled(Db db) => Settings.Bool(db, "sp_simlab_enabled", true);

    /// <summary>An explicit, currently-in-window Lab grant (admin/complimentary/sponsored/institution/…).</summary>
    public static bool HasGrant(Db db, long userId) =>
        db.QueryOne(@"SELECT id FROM simulation_entitlements
            WHERE user_id=? AND status='active'
              AND (starts_at IS NULL OR starts_at<=datetime('now'))
              AND (expires_at IS NULL OR expires_at>datetime('now'))
            LIMIT 1", userId) is not null;

    /// <summary>Does the student qualify for Lab access under the configured rule (or an explicit grant)?</summary>
    public static bool Eligible(Db db, long userId)
    {
        if (HasGrant(db, userId)) return true;
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        bool HasExam() => db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        return Settings.Str(db, "simlab_requires", "membership_or_exam") switch
        {
            "open" => true,
            "membership_and_enrolment" => hasMembership && HasExam(),
            "membership_or_exam" => hasMembership || HasExam(),
            _ => hasMembership,   // "membership" (default fallback)
        };
    }

    /// <summary>Student-facing access descriptor for GET /api/me/lab/access.</summary>
    public static object AccessFor(Db db, long userId)
    {
        var enabled = Enabled(db);
        var rule = Settings.Str(db, "simlab_requires", "membership_or_exam");
        var grant = db.QueryOne(@"SELECT source,expires_at FROM simulation_entitlements
            WHERE user_id=? AND status='active'
              AND (starts_at IS NULL OR starts_at<=datetime('now'))
              AND (expires_at IS NULL OR expires_at>datetime('now'))
            ORDER BY id DESC LIMIT 1", userId);
        var hasMembership = db.QueryOne("SELECT id FROM memberships WHERE user_id=? AND status='active'", userId) is not null;
        var hasExam = db.QueryOne("SELECT id FROM exam_entitlements WHERE user_id=? AND status IN ('available','booked','consumed')", userId) is not null;
        var eligible = Eligible(db, userId);
        var hasAccess = enabled && eligible;

        // Why access is (not) granted — never a raw error; friendly, student-facing.
        string source = grant is not null ? "grant" : hasMembership ? "membership" : hasExam ? "exam" : "none";
        string reason = !enabled
            ? "The Practice Lab is not currently available."
            : hasAccess
                ? "You have access to the Project Controls Practice Lab."
                : rule == "membership_and_enrolment"
                    ? "Practice Lab access requires an active membership and a certification enrolment."
                    : "Practice Lab access is included with an active PCI membership.";

        return new
        {
            enabled,
            has_access = hasAccess,
            reason,
            rule,
            source,
            member_type = hasAccess ? CertuvoLink.DetectMemberType(db, userId) : null,
            expires_at = grant is null ? null : H.Str(grant["expires_at"]),
        };
    }

    /// <summary>
    /// The student catalogue with SERVER-side search + faceted filtering (the capacity path). Facets and
    /// the whole-catalogue summary/resume are computed over the FULL published set — before the filter
    /// predicate — so a filtered view keeps stable filter controls and whole-catalogue KPIs while `rows`
    /// carries only the matches. Single pass over the published scenarios; the shape is the wire contract
    /// GET /api/me/lab/catalogue returns verbatim (do not rename keys — the SPA and tests read them).
    /// </summary>
    public static object Catalogue(Db db, long userId,
        string? q, string? difficulty, string? kind, string? industry, string? competency,
        string lang = "en")
    {
        var fQ = (q ?? "").Trim().ToLowerInvariant();
        var fDifficulty = (difficulty ?? "").Trim().ToLowerInvariant();
        var fKind = (kind ?? "").Trim().ToLowerInvariant();
        var fIndustry = (industry ?? "").Trim().ToLowerInvariant();
        var fCompetency = (competency ?? "").Trim().ToLowerInvariant();

        // The student's latest attempt per scenario (for status/score badges), keyed by scenario_id.
        var latest = new Dictionary<long, (string status, object? score, long attemptId)>();
        foreach (var a in db.Query(@"SELECT id,scenario_id,status,score FROM simulation_attempts
            WHERE user_id=? ORDER BY id ASC", userId))
            latest[H.L(a["scenario_id"])] = (H.Str(a["status"]) ?? "in_progress", a["score"], H.L(a["id"]));

        var rows = new List<object>();
        var difficulties = new SortedSet<string>();
        var kinds = new SortedSet<string>();
        var industries = new SortedSet<string>();
        var competencySet = new SortedSet<string>();
        var total = 0;
        // Full-set student-progress aggregates (the catalogue KPIs) and the resume list — computed over
        // EVERY published scenario, before the filter predicate, so a filtered view still shows
        // whole-catalogue progress and can carry its own "continue where you left off" strip.
        var completedCount = 0;
        var inProgressCount = 0;
        long scoreSum = 0;
        var scoredCount = 0;
        var resume = new List<object>();
        var done = new HashSet<string>(StringComparer.Ordinal) { "passed", "completed" };
        var scenarios = db.Query(@"SELECT id,scenario_code,title,kind,industry,project_type,difficulty,
            est_minutes,competencies_json,certification_id,summary,version,config_json FROM simulation_scenarios
            WHERE status='published' ORDER BY sort_order ASC, id ASC");
        // Multilingual catalogue: overlay translated display text BEFORE the loop so the search
        // predicate matches what the student actually sees. Codes, facets and grading config are
        // language-invariant.
        ItemI18n.Overlay(db, "scenario", lang, scenarios, "title", "summary");
        foreach (var s in scenarios)
        {
            total++;
            var scenDifficulty = H.Str(s["difficulty"]) ?? "";
            var scenKind = H.Str(s["kind"]) ?? "";
            var scenIndustry = H.Str(s["industry"]) ?? "";
            var comps = ParseArray(H.Str(s["competencies_json"]));
            if (scenDifficulty.Length > 0) difficulties.Add(scenDifficulty);
            if (scenKind.Length > 0) kinds.Add(scenKind);
            if (scenIndustry.Length > 0) industries.Add(scenIndustry);
            foreach (var c in comps) competencySet.Add(c);

            var sid = H.L(s["id"]);
            var has = latest.TryGetValue(sid, out var at);
            if (has)
            {
                if (done.Contains(at.status)) completedCount++;
                else if (at.status == "in_progress")
                {
                    inProgressCount++;
                    if (resume.Count < 12)
                        resume.Add(new
                        {
                            scenario_code = H.Str(s["scenario_code"]),
                            title = H.Str(s["title"]),
                            kind = scenKind,
                            difficulty = scenDifficulty,
                            industry = scenIndustry,
                            est_minutes = H.L(s["est_minutes"]),
                            certification_id = s["certification_id"] is null ? (long?)null : H.L(s["certification_id"]),
                        });
                }
                if (at.score is not null) { scoreSum += H.L(at.score); scoredCount++; }
            }

            // Filter predicate: exact (case-insensitive) facet matches + a substring search over
            // title / summary / code / industry.
            if (fDifficulty.Length > 0 && !scenDifficulty.Equals(fDifficulty, StringComparison.OrdinalIgnoreCase)) continue;
            if (fKind.Length > 0 && !scenKind.Equals(fKind, StringComparison.OrdinalIgnoreCase)) continue;
            if (fIndustry.Length > 0 && !scenIndustry.Equals(fIndustry, StringComparison.OrdinalIgnoreCase)) continue;
            if (fCompetency.Length > 0 && !comps.Any(c => c.Equals(fCompetency, StringComparison.OrdinalIgnoreCase))) continue;
            if (fQ.Length > 0)
            {
                var hay = $"{H.Str(s["title"])} {H.Str(s["summary"])} {H.Str(s["scenario_code"])} {scenIndustry}".ToLowerInvariant();
                if (!hay.Contains(fQ)) continue;
            }

            rows.Add(new
            {
                id = sid,
                scenario_code = H.Str(s["scenario_code"]),
                title = H.Str(s["title"]),
                kind = scenKind,
                industry = scenIndustry,
                project_type = H.Str(s["project_type"]),
                difficulty = scenDifficulty,
                est_minutes = H.L(s["est_minutes"]),
                competencies = comps,
                certification_id = s["certification_id"] is null ? (long?)null : H.L(s["certification_id"]),
                summary = H.Str(s["summary"]),
                version = H.L(s["version"]),
                interactive = !string.IsNullOrWhiteSpace(H.Str(s["config_json"])),   // has a runnable task
                attempt_status = has ? at.status : (string?)null,
                attempt_id = has ? at.attemptId : (long?)null,
                score = has ? at.score : null,
            });
        }
        return new
        {
            rows,
            total,                 // total published scenarios (before filtering)
            matched = rows.Count,  // after filtering
            facets = new { difficulties, kinds, industries, competencies = competencySet },
            // Whole-catalogue KPIs + resume list (unaffected by the active filter).
            summary = new
            {
                published = total,
                completed = completedCount,
                in_progress = inProgressCount,
                avg_score = scoredCount > 0 ? (double?)Math.Round((double)scoreSum / scoredCount) : null,
            },
            resume,
        };
    }

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
