using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>PCI World-only deployment mode (PCIWORLD_ONLY=true): the service serves EXCLUSIVELY
/// the PCI World surfaces — the Institute website, portals and their APIs are unreachable on it.
/// This is the "deploy only PCI World" shape for a dedicated pciworld.org / Render service.</summary>
public static class WorldOnly
{
    /// <summary>Paths a world-only deployment serves. Everything else is redirected (HTML) or
    /// 404'd (API) by the middleware in Program.cs. Segment-boundary matching: "/world" does not
    /// leak "/worldfoo".</summary>
    public static bool Allowed(Microsoft.AspNetCore.Http.PathString p) =>
        p == "/" ||
        p.StartsWithSegments("/world") || p.StartsWithSegments("/world-admin") ||
        p.StartsWithSegments("/api/world") || p.StartsWithSegments("/api/world-admin") ||
        p.StartsWithSegments("/api/health") ||
        // The self-hosted brand fonts the world pages reference. Only the fonts directory — the
        // rest of /assets belongs to the Institute site, which this deployment does not serve.
        p.StartsWithSegments("/assets/fonts") ||
        // A host that cannot serve its own sitemap cannot be crawled properly, so these are part of
        // the world surface rather than platform extras.
        p == "/robots.txt" || p == "/world-sitemap.xml" || p == "/sitemap.xml" ||
        p == "/favicon.ico" || p == "/favicon.svg";

    public static bool Enabled =>
        string.Equals(Environment.GetEnvironmentVariable("PCIWORLD_ONLY"), "true", StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// Absolute base URL for links PCI World mails out (verify, reset) and for canonical tags.
///
/// SECURITY: never build a mailed link from the request's Host header. `Host` is attacker-supplied
/// on every request, so `https://{Host}/world/reset-password?t=…` lets anyone who can reach /forgot
/// mail a victim a VALID reset token pointing at a host they control. This resolver trusts only
/// values the operator (or the platform) set, and accepts the request host solely when it is
/// demonstrably one of ours; otherwise it falls back to the canonical base, which is a constant.
/// </summary>
public static class WorldUrl
{
    static string? Env(string k)
    {
        var v = Environment.GetEnvironmentVariable(k);
        return string.IsNullOrWhiteSpace(v) ? null : v.Trim().TrimEnd('/');
    }

    /// <summary>Hosts we will echo back from a request: the configured canonical/world hosts plus
    /// loopback for local development.</summary>
    static HashSet<string> KnownHosts()
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "localhost", "127.0.0.1", "[::1]" };
        set.Add(Redirects.CanonicalHost);
        foreach (var k in new[] { "PCIWORLD_HOSTS", "PCIWORLD_ADMIN_HOSTS", "REDIRECT_HOSTS" })
            foreach (var h in (Environment.GetEnvironmentVariable(k) ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                set.Add(h);
        return set;
    }

    public static string Base(HttpRequest? req = null)
    {
        // 1. Explicit operator configuration always wins.
        var configured = Env("PCIWORLD_BASE_URL") ?? Env("APP_BASE_URL") ?? Env("SITE_BASE_URL")
        // 2. Render exports the service's real external URL — makes zero-config previews safe too.
                      ?? Env("RENDER_EXTERNAL_URL");
        if (configured is not null) return configured;
        if (Env("RENDER_EXTERNAL_HOSTNAME") is { } rh) return "https://" + rh;
        // 3. The request host, but only when it is a host we know we answer on.
        if (req is not null && req.Host.HasValue && KnownHosts().Contains(req.Host.Host))
            return $"{req.Scheme}://{req.Host}";
        // 4. Fail closed on a constant rather than echoing an unverified Host header.
        return Redirects.CanonicalBase;
    }

    /// <summary>Absolute URL for a world path ("/world/reset-password?t=…").</summary>
    public static string Abs(HttpRequest? req, string path) =>
        Base(req) + (path.StartsWith('/') ? path : "/" + path);
}

/// <summary>PCI World admin RBAC — server-side, per action group. Hiding a button is not
/// authorization; every world-admin endpoint calls Allowed() before touching data.</summary>
public static class WorldRbac
{
    public static readonly string[] Roles = { "owner", "author", "reviewer", "publisher", "viewer" };

    /// <summary>Action groups: read | author (draft CRUD, validate, submit for review) |
    /// review (approve/reject) | publish (publish, retire/restore, calendar) | admin (user management).</summary>
    public static bool Allowed(string? role, string action) => role switch
    {
        "owner" => true,
        "author" => action is "read" or "author",
        "reviewer" => action is "read" or "review",
        "publisher" => action is "read" or "publish",
        "viewer" => action is "read",
        _ => false,
    };
}

/// <summary>
/// PCI World challenge lifecycle — the working-copy state machine plus the publish snapshot.
///
///   draft → in_review → approved → published   (independent maker-checker at approve)
///   published → draft                          (revise: edit a NEW version; the last published
///                                               snapshot keeps serving until republished)
///   retired flag                               (hides from rotation; history untouched)
///
/// Publishing is the only way content reaches pciworld_challenge_versions, and versions are
/// immutable: attempts pin (challenge_id, version) and replay only from the snapshot.
/// </summary>
public static class WorldLifecycle
{
    public static bool CanEdit(string? status) => status == "draft";

    public static WorldContent.ChallengeInput InputFor(Dictionary<string, object?> row) => new(
        H.Str(row["code"]) ?? "", H.Str(row["title"]), H.Str(row["hook"]), H.Str(row["industry"]),
        H.Str(row["track"]), H.Str(row["difficulty"]), H.L(row["est_minutes"]),
        H.L(row["synthetic_declared"]) == 1, H.Str(row["config_json"]));

    public static string? SubmitReview(Db db, long id)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='in_review', updated_at=datetime('now') WHERE id=? AND status='draft'", id);
        return n == 0 ? "not_draft" : null;
    }

    /// <summary>Approve = the maker-checker checkpoint: the approver must not be the author, and
    /// the §14-style validator must pass on the working copy.</summary>
    public static string? Approve(Db db, long id, long approverId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "in_review") return "not_in_review";
        if (row["author_id"] is not null && H.L(row["author_id"]) == approverId) return "maker_checker";
        if (!WorldContent.Publishable(WorldContent.Validate(InputFor(row)))) return "not_publishable";
        db.Execute("UPDATE pciworld_challenges SET status='approved', approved_by=?, updated_at=datetime('now') WHERE id=? AND status='in_review'",
            approverId, id);
        return null;
    }

    public static string? Reject(Db db, long id, long reviewerId, string? note)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='draft', reviewed_by=?, review_note=?, updated_at=datetime('now') WHERE id=? AND status='in_review'",
            reviewerId, note, id);
        return n == 0 ? "not_in_review" : null;
    }

    /// <summary>Publish the approved working copy as an immutable new version. Re-validates at the
    /// moment of publication — approval is necessary but never sufficient.</summary>
    public static string? Publish(Db db, long id, long publisherId)
    {
        var row = db.QueryOne("SELECT * FROM pciworld_challenges WHERE id=?", id);
        if (row is null) return "not_found";
        if (H.Str(row["status"]) != "approved") return "not_approved";
        if (!WorldContent.Publishable(WorldContent.Validate(InputFor(row)))) return "not_publishable";
        var next = H.L(row["current_version"]) + 1;
        db.Execute(@"INSERT INTO pciworld_challenge_versions
                (challenge_id,version,title,hook,industry,role,track,difficulty,est_minutes,competencies_json,config_json,published_by)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?)",
            id, next, H.Str(row["title"]), H.Str(row["hook"]), H.Str(row["industry"]), H.Str(row["role"]),
            H.Str(row["track"]), H.Str(row["difficulty"]), H.L(row["est_minutes"]),
            H.Str(row["competencies_json"]), H.Str(row["config_json"]), publisherId);
        db.Execute(@"UPDATE pciworld_challenges SET status='published', current_version=?,
                published_at=COALESCE(published_at, datetime('now')), updated_at=datetime('now') WHERE id=?", next, id);
        return null;
    }

    public static string? Revise(Db db, long id)
    {
        var n = db.Execute("UPDATE pciworld_challenges SET status='draft', approved_by=NULL, updated_at=datetime('now') WHERE id=? AND status='published'", id);
        return n == 0 ? "not_published" : null;
    }

    public static void Retire(Db db, long id) =>
        db.Execute("UPDATE pciworld_challenges SET retired=1, updated_at=datetime('now') WHERE id=?", id);

    public static void Restore(Db db, long id) =>
        db.Execute("UPDATE pciworld_challenges SET retired=0, updated_at=datetime('now') WHERE id=?", id);

    /// <summary>The latest immutable snapshot for a servable challenge (published at least once,
    /// not retired) — the ONLY content the public surface ever serves.</summary>
    public static Dictionary<string, object?>? LiveVersion(Db db, long challengeId)
    {
        var ch = db.QueryOne("SELECT id,current_version,retired FROM pciworld_challenges WHERE id=? AND current_version>=1 AND retired=0", challengeId);
        if (ch is null) return null;
        return db.QueryOne("SELECT * FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?",
            challengeId, H.L(ch["current_version"]));
    }

    /// <summary>The exact snapshot an attempt pinned — historical replay never moves.</summary>
    public static Dictionary<string, object?>? PinnedVersion(Db db, long challengeId, long version) =>
        db.QueryOne("SELECT * FROM pciworld_challenge_versions WHERE challenge_id=? AND version=?", challengeId, version);

    /// <summary>
    /// Today's featured challenge. Delegates to the rotation ledger (Core/WorldRotation.cs): the day's
    /// selection is a recorded, immutable period, so it is stable for the whole day, survives
    /// catalogue changes, and can be answered historically. The calendar override still wins — it is
    /// now recorded as the period's source rather than being re-evaluated on every request.
    /// </summary>
    public static Dictionary<string, object?>? Today(Db db, DateTime utcNow) => WorldRotation.Current(db, utcNow);
}
