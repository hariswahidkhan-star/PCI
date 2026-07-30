using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI Project Intelligence — versioned learner API + world-admin coverage report
/// (docs/pciworld/PROJECT_INTELLIGENCE.md).
///
///   GET /api/world/v1/project-intelligence/home        programme summary + today + next actions
///   GET /api/world/v1/project-intelligence/categories  the 12 competency domains with live counts
///   GET /api/world/v1/project-intelligence/catalog     taxonomy-filtered, paginated practice library
///   GET /api/world/v1/project-intelligence/daily       today's Intelligence Brief (metadata only)
///   GET /api/world-admin/intelligence/coverage         Year-1 plan vs published bank (role-gated)
///
/// This is a thin, read-only projection over the existing World engines: challenges stay served
/// and attempted through Endpoints/World.cs (session, attempts, save, submit, share), so there is
/// exactly one attempt pipeline and one scoring authority. Every payload here is catalogue
/// metadata — no config_json, no reference values, no option qualities can appear, because the
/// queries never select them.
/// </summary>
public static class WorldIntelligenceApi
{
    const string Base = "/api/world/v1/project-intelligence";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        bool Enabled() => Settings.Bool(db, "world_enabled", true);
        IResult Disabled() => Results.Json(new { error = "world_disabled", message = "PCI World is not currently open." }, statusCode: 403);

        object Facets(Dictionary<string, object?> r) => new
        {
            code = H.Str(r["code"]),
            title = H.Str(r["title"]),
            hook = H.Str(r["hook"]),
            experience_type = H.Str(r["pi_type"]),
            experience_type_label = Label(WorldIntelligence.ExperienceTypes, H.Str(r["pi_type"])),
            primary_domain = H.Str(r["pi_domain"]),
            primary_domain_label = Label(WorldIntelligence.Domains, H.Str(r["pi_domain"])),
            lifecycle_stage = H.Str(r["pi_lifecycle"]),
            sector = H.Str(r["pi_sector"]),
            interaction = H.Str(r["pi_interaction"]),
            difficulty = H.Str(r["difficulty"]),
            difficulty_band = WorldIntelligence.DifficultyBand(H.Str(r["difficulty"])),
            duration_band = WorldIntelligence.DurationBand(H.L(r["est_minutes"])),
            est_minutes = H.L(r["est_minutes"]),
        };

        // ── home: what is this, what is today, where do I go next ──
        app.MapGet($"{Base}/home", () =>
        {
            if (!Enabled()) return Disabled();
            var today = WorldLifecycle.Today(db, DateTime.UtcNow);
            var version = today is null ? null : WorldLifecycle.LiveVersion(db, H.L(today["id"]));
            var counts = db.Query(@"SELECT pi_type, COUNT(*) n FROM pciworld_challenges
                    WHERE current_version>=1 AND retired=0 AND pi_type IS NOT NULL GROUP BY pi_type")
                .ToDictionary(r => H.Str(r["pi_type"]) ?? "", r => H.L(r["n"]));
            return Results.Json(new
            {
                product = "PCI Project Intelligence",
                tagline = "Think. Decide. Deliver.",
                today = today is null || version is null ? null : new
                {
                    code = H.Str(today["code"]),
                    title = H.Str(version["title"]),
                    hook = H.Str(version["hook"]),
                    difficulty_band = WorldIntelligence.DifficultyBand(H.Str(version["difficulty"])),
                    duration_band = WorldIntelligence.DurationBand(H.L(version["est_minutes"])),
                    est_minutes = H.L(version["est_minutes"]),
                },
                changes_at = WorldRotation.NextBoundaryUtc(WorldRotation.Zone(db), DateTime.UtcNow).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                library = new
                {
                    total = counts.Values.Sum(),
                    by_experience_type = WorldIntelligence.ExperienceTypes.Select(t => new
                    { key = t.Key, label = t.Value, count = counts.TryGetValue(t.Key, out var n) ? n : 0 }),
                },
            });
        });

        // ── categories: the 12 competency domains with live servable counts ──
        app.MapGet($"{Base}/categories", () =>
        {
            if (!Enabled()) return Disabled();
            var counts = db.Query(@"SELECT pi_domain, COUNT(*) n FROM pciworld_challenges
                    WHERE current_version>=1 AND retired=0 AND pi_domain IS NOT NULL GROUP BY pi_domain")
                .ToDictionary(r => H.Str(r["pi_domain"]) ?? "", r => H.L(r["n"]));
            return Results.Json(new
            {
                categories = WorldIntelligence.Domains.Select(d => new
                { key = d.Key, label = d.Value, count = counts.TryGetValue(d.Key, out var n) ? n : 0 }),
            });
        });

        // ── catalog: the practice library, filtered on the governed taxonomy ──
        app.MapGet($"{Base}/catalog", (HttpContext ctx) =>
        {
            if (!Enabled()) return Disabled();
            string Q(string k) => ctx.Request.Query[k].ToString().Trim();

            var where = " WHERE current_version>=1 AND retired=0";
            var args = new List<object?>();
            void Facet(string param, string col, IReadOnlyDictionary<string, string> vocab)
            {
                var v = Q(param);
                if (v.Length > 0 && vocab.ContainsKey(v)) { where += $" AND {col}=?"; args.Add(v); }
            }
            Facet("type", "pi_type", WorldIntelligence.ExperienceTypes);
            Facet("domain", "pi_domain", WorldIntelligence.Domains);
            Facet("sector", "pi_sector", WorldIntelligence.Sectors);
            Facet("lifecycle", "pi_lifecycle", WorldIntelligence.LifecycleStages);
            Facet("interaction", "pi_interaction", WorldIntelligence.Interactions);
            var diff = Q("difficulty");
            if (WorldIntelligence.DifficultyBands.ContainsKey(diff))
            {
                // The band is the public filter; it expands to the engine's stored difficulties.
                var raw = diff switch
                {
                    "foundation" => new[] { "foundation", "developing" },
                    "practitioner" => new[] { "professional" },
                    "advanced" => new[] { "advanced" },
                    _ => new[] { "expert" },
                };
                where += $" AND difficulty IN ({string.Join(",", raw.Select(_ => "?"))})";
                args.AddRange(raw);
            }
            var band = Q("duration");
            if (WorldIntelligence.DurationBands.ContainsKey(band))
            {
                var (lo, hi) = band switch
                {
                    "quick" => (0, 7), "standard" => (8, 12), "deep" => (13, 20), _ => (21, 999),
                };
                where += " AND est_minutes>=? AND est_minutes<=?";
                args.Add(lo); args.Add(hi);
            }

            const int perPage = 30;
            var page = Math.Max(1, (int)(H.QL(ctx, "page") ?? 1));
            var total = db.Scalar<long>("SELECT COUNT(*) FROM pciworld_challenges" + where, args.ToArray());
            var pages = (int)Math.Max(1, (total + perPage - 1) / perPage);
            if (page > pages) page = pages;
            var rows = db.Query(@"SELECT code,title,hook,difficulty,est_minutes,pi_type,pi_domain,pi_lifecycle,pi_sector,pi_interaction
                    FROM pciworld_challenges" + where +
                $" ORDER BY id ASC LIMIT {perPage} OFFSET {(page - 1) * perPage}", args.ToArray());
            return Results.Json(new { total, page, pages, items = rows.Select(Facets) });
        });

        // ── daily: today's Intelligence Brief — metadata only; the attempt pipeline is World.cs ──
        app.MapGet($"{Base}/daily", () =>
        {
            if (!Enabled()) return Disabled();
            var today = WorldLifecycle.Today(db, DateTime.UtcNow);
            var ch = today is null ? null : db.QueryOne(
                @"SELECT code,title,hook,difficulty,est_minutes,pi_type,pi_domain,pi_lifecycle,pi_sector,pi_interaction
                    FROM pciworld_challenges WHERE id=? AND current_version>=1 AND retired=0", H.L(today["id"]));
            if (ch is null) return Results.Json(new { available = false });
            return Results.Json(new
            {
                available = true,
                brief = Facets(ch),
                changes_at = WorldRotation.NextBoundaryUtc(WorldRotation.Zone(db), DateTime.UtcNow).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                timezone = Settings.Str(db, "world_rotation_timezone", "UTC"),
            });
        });

        // ── world-admin: Year-1 coverage + runway (read group; separate admin realm) ──
        app.MapGet("/api/world-admin/intelligence/coverage", (HttpContext ctx) =>
        {
            var adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, "read")) return Results.Json(new { error = "forbidden" }, statusCode: 403);
            var plan = WorldIntelligence.LoadYearOne();
            if (plan is null)
                return Results.Json(new { plan_available = false,
                    message = "Year-1 plan files not found — the programme serves the live bank only." });
            return Results.Json(new { plan_available = true, coverage = WorldIntelligence.Coverage(db, plan) });
        });
    }

    static string? Label(IReadOnlyDictionary<string, string> vocab, string? key) =>
        key is not null && vocab.TryGetValue(key, out var v) ? v : null;
}
