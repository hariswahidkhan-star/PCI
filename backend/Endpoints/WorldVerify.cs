using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Phase 5 — privacy-safe PUBLIC verification of a PCI World Passport by PCI Student Number.
///
/// A recruiter holding only a Student Number can confirm whether its owner currently publishes a
/// PCI World Passport — and see exactly what the owner chose to disclose, nothing more. Three
/// rules shape everything here:
///
///   POST only    — the number NEVER appears in a URL, query string, access log or Referer. It
///                  travels in the request body and is logged only as a hash.
///
///   One neutral  — unknown, malformed, private, unpublished, expired, suspended, erased and
///   answer         retired numbers all produce THE SAME response object, built at one single
///                  site, with the same 200 status. "This number was withdrawn" is itself a
///                  disclosure, so a caller must not be able to distinguish it from "this number
///                  never existed". A uniform random delay is added to every resolution answer
///                  (hit or miss) so the depth at which resolution stopped is not measurable
///                  from response timing.
///
///   Owner's      — a successful answer reuses the SAME disclosure object the public Passport
///   disclosure     page uses (<see cref="WorldPassport.Disclosure"/>) and the SAME evidence
///                  queries (<see cref="WorldAccount.EvidenceRows"/>/<see cref="WorldAccount.EvidenceStats"/>),
///                  so this endpoint can never show a field the Passport itself would not.
///
/// Resolution follows the canonical identity chain and nothing else: exact number →
/// pci_student_number_registry (the permanent ledger; a merged number follows its recorded
/// successor owner, a retired or quarantined one resolves to nobody) → canonical users row
/// (active, non-test student) → active World participation (pciworld_participants) → the World
/// account's published, unexpired Passport. Exact equality only — parameterised '=' throughout,
/// and the input shape is pinned to the legacy_v1 format before any query runs, so wildcard,
/// range, prefix and bulk lookups are structurally impossible.
///
/// Deliberate omission: the live public Passport URL is NOT returned. The Passport token is
/// stored hashed only (passport_token_sha) — by design the raw link is known to nobody but the
/// owner — so this endpoint cannot reconstruct it, and minting a second addressing scheme for
/// published Passports would widen the attack surface the token design deliberately narrowed.
/// The response says the live record exists and points the caller at /world/verify, where the
/// owner-shared link can be checked.
/// </summary>
public static class WorldVerify
{
    /// <summary>Spoken on every verification result, verbatim (master spec §7).</summary>
    public const string Disclaimer =
        "PCI World Passport records selected professional practice and challenge evidence. " +
        "It is not, by itself, a PCI certification, examination result, accreditation, licence, " +
        "or guarantee of professional competence.";

    /// <summary>The one neutral sentence every non-disclosing state answers with.</summary>
    public const string NeutralMessage =
        "No public PCI World Passport is currently available for that number.";

    /// <summary>legacy_v1 shape — the same pin the Phase 2 backfill uses. Anything else never
    /// reaches the database.</summary>
    static readonly System.Text.RegularExpressions.Regex Shape =
        new(@"^PCI-\d{4}-\d{6,}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    /// <summary>A resolved, currently-published Passport: the World account row plus the Student
    /// Number its canonical owner currently carries (after a merge, the successor number).</summary>
    public sealed record Hit(Dictionary<string, object?> World, string StudentNumber);

    /// <summary>
    /// The whole verification predicate in one place: NULL for every state that must stay
    /// indistinguishable, a <see cref="Hit"/> only when every gate passes. Callers must map the
    /// null to the single neutral response and never branch on WHY it was null — this method
    /// deliberately does not say.
    /// </summary>
    public static Hit? Resolve(Db db, string number)
    {
        if (number.Length > 32 || !Shape.IsMatch(number)) return null;

        // 1. The registry is the authority on what a number means. issued → its current owner;
        //    merged → the recorded successor owner (the one state where answering under a
        //    different current number is the DESIGN, not drift); retired/quarantined → nobody.
        //    A number issued before the registry existed has no row: fall back to the exact
        //    projection — but only when it identifies exactly ONE account. A duplicated
        //    lazy-writer-era number is ambiguous, and verifying an ambiguous identity could
        //    disclose the wrong person's record.
        long? uid = null;
        var wasMerged = false;
        var reg = db.QueryOne(
            "SELECT state,resolves_to_user_id FROM pci_student_number_registry WHERE student_number=?", number);
        if (reg is not null)
        {
            var state = H.Str(reg["state"]);
            if (state is not ("issued" or "merged") || reg["resolves_to_user_id"] is null) return null;
            uid = H.L(reg["resolves_to_user_id"]);
            wasMerged = state == "merged";
        }
        else
        {
            var owners = db.Query("SELECT id FROM users WHERE registration_no=? LIMIT 2", number);
            if (owners.Count != 1) return null;
            uid = H.L(owners[0]["id"]);
        }

        // 2. The canonical account must be an eligible, living student identity. Test accounts
        //    never verify (same rule as the credential register).
        var u = db.QueryOne(
            "SELECT registration_no, role, status, COALESCE(is_test,0) AS is_test FROM users WHERE id=?", uid);
        if (u is null || H.Str(u["role"]) != "student" || H.Str(u["status"]) != "active"
            || H.L(u["is_test"]) != 0) return null;
        var current = H.Str(u["registration_no"]);
        if (string.IsNullOrWhiteSpace(current)) return null;
        // For an 'issued' number (and the projection fallback) the owner must still CARRY the
        // queried number — registry/projection drift is a state to repair, not to verify through.
        // Only a recorded merge may answer under a successor number.
        if (!wasMerged && !string.Equals(current, number, StringComparison.Ordinal)) return null;

        // 3. World participation must be active — a suspended or erased participation withdraws
        //    the public record with it.
        var part = db.QueryOne("SELECT status FROM pciworld_participants WHERE user_id=?", uid);
        if (part is null || H.Str(part["status"]) != "active") return null;

        // 4. The World account behind the canonical identity, and its published Passport — the
        //    exact predicate the token route (PassportByToken) enforces: active account, public,
        //    a live token, not past its owner-set expiry.
        var w = db.QueryOne(@"SELECT w.* FROM pciworld_users w
            WHERE (w.student_user_id=? OR EXISTS(
                    SELECT 1 FROM pciworld_user_map m
                    WHERE m.legacy_world_id=w.id AND m.canonical_user_id=?))
              AND w.status='active' AND w.passport_public=1 AND w.passport_token_sha IS NOT NULL
            ORDER BY w.id LIMIT 1", uid, uid);
        if (w is null || WorldPassport.Expired(w)) return null;

        return new Hit(w, current!);
    }

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        IResult Err(string key, int code) => Results.Json(new { error = key }, statusCode: code);

        // Same in-memory fixed-window limiter as WorldAccount/Program.cs, applied progressively:
        // a per-caller burst window, a per-caller sustained window, and a global ceiling that
        // bounds distributed enumeration of the (guessable, sequential) number space.
        var rl = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long start)>();
        bool Throttled(string key, int limit, long windowMs)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (rl.Count > 20_000)
                foreach (var kv in rl) if (now - kv.Value.start >= windowMs) rl.TryRemove(kv.Key, out _);
            var e = rl.AddOrUpdate(key, (1, now), (_, c) => now - c.start >= windowMs ? (1, now) : (c.count + 1, c.start));
            return e.count > limit;
        }

        app.MapPost("/api/public/world-passports/verify", async (HttpContext ctx) =>
        {
            // Privacy headers on EVERY answer: a verification result is queried about a person,
            // so no cache may keep it and no crawler may index it.
            ctx.Response.Headers["Cache-Control"] = "no-store";
            ctx.Response.Headers["X-Robots-Tag"] = "noindex";

            // Progressive throttle. Single '|' on purpose: every window counts every request,
            // so tripping the burst limit cannot be used to stop the hourly window counting.
            var ip = Security.ClientIp(ctx);
            if (Throttled("vnum-burst|" + ip, 10, 60_000)
                | Throttled("vnum-hour|" + ip, 40, 3_600_000)
                | Throttled("vnum-global", 600, 3_600_000))
                return Err("rate_limited", 429);

            var b = await H.Body(ctx.Request);
            var raw = H.GetS(b, "number") ?? "";
            // Normalisation: surrounding whitespace only, then uppercase. Nothing inside the
            // value is rewritten — an internally malformed number simply fails the shape pin.
            var number = raw.Trim().ToUpperInvariant();
            if (number.Length == 0) return Err("missing_number", 400);

            // ONE construction site for the neutral answer — every non-disclosing state flows
            // through this exact object, so the response bytes cannot vary by state.
            IResult Neutral() => Results.Json(new { found = false, message = NeutralMessage });

            // With World disabled no Passport is served anywhere, so nothing is verifiable —
            // the same neutral sentence, not a distinct shape.
            var hit = Settings.Bool(db, "world_enabled", true) ? Resolve(db, number) : null;

            // Never the raw number in a log line — a hash prefix is enough to correlate abuse.
            log(null, "world_passport_number_verify",
                $"sha:{Security.Sha(number)[..12]} {(hit is null ? "neutral" : "disclosed")}");

            // Uniform jitter on hit and miss alike: response time stops correlating with how
            // deep in the resolution chain a miss occurred.
            await Task.Delay(System.Random.Shared.Next(30, 90));
            if (hit is null) return Neutral();

            var wid = H.L(hit.World["id"]);
            var show = WorldPassport.Disclosure.From(hit.World);
            var stats = WorldAccount.EvidenceStats(db, wid, visibleOnly: true);
            var rows = WorldAccount.EvidenceRows(db, wid, visibleOnly: true, limit: 20);

            // Field-level disclosure: a switched-off field is ABSENT, not null — the shape
            // itself honours the owner's choice, exactly like the public Passport page.
            var evidence = new List<Dictionary<string, object?>>();
            foreach (var r in rows)
            {
                var item = new Dictionary<string, object?>
                {
                    ["title"] = H.Str(r["title"]),
                    ["reference"] = $"{H.Str(r["code"])} · v{H.L(r["version"])}",
                    ["industry"] = H.Str(r["industry"]),
                    ["track"] = H.Str(r["track"]),
                    ["difficulty"] = H.Str(r["difficulty"]),
                };
                if (show.Scores) item["score"] = r["score"] is null ? null : Math.Round(Convert.ToDouble(r["score"]), 1);
                if (show.Profiles) item["profile"] = H.Str(r["profile_key"])?.Replace('_', ' ');
                if (show.Dates) item["completed_on"] = (H.Str(r["completed_at"]) ?? "").Split(' ')[0];
                evidence.Add(item);
            }

            // The Passport photograph, inline. Its presence IS the owner's consent (uploading it
            // is opt-in and it is already served on the public Passport). Inlined as a data URI
            // because the only by-reference route is the token URL this endpoint deliberately
            // does not hold; bounded so one large image cannot balloon the response.
            string? photo = null;
            if (H.Str(hit.World["passport_photo_ref"]) is { Length: > 0 } pref)
            {
                var got = Storage.Get(pref);
                if (got?.bytes is { Length: > 0 and <= 600_000 } bytes)
                    photo = $"data:{H.Str(hit.World["passport_photo_mime"]) ?? got.Value.mime};base64,{Convert.ToBase64String(bytes)}";
            }

            // ONLY owner-disclosed, public-by-consent fields below. Never email, DOB, address,
            // phone, users.id or any other internal identifier.
            return Results.Json(new
            {
                found = true,
                student_number = hit.StudentNumber,
                display_name = H.Str(hit.World["display_name"]) ?? "PCI World participant",
                passport = new
                {
                    status = "published",
                    discloses = show.Summary,
                    expires_on = H.Str(hit.World["passport_expires_at"])?.Split(' ')[0],
                    completed = stats.Completed,
                    industries = stats.Industries,
                    tracks = stats.Tracks,
                },
                evidence,
                evidence_total = stats.Completed,
                photo,
                verify_page = "/world/verify",
                verified_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " UTC",
                disclaimer = Disclaimer,
            });
        });
    }
}
