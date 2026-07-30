using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// The PCI World launch board — one owner-only screen that turns each World feature on.
///
/// WHY THIS EXISTS. Every World feature ships behind a settings flag that seeds '0', and the
/// generic Settings screen renders those flags as free-text boxes, alphabetically, among a hundred
/// unrelated platform keys, labelled "Pciworld Community Images Enabled". Two consequences, both
/// bad. An operator who wants to launch the product cannot find the switches. And an operator who
/// does find them can type '1' into the image flag — the one flag that must not move until a
/// moderation provider is contracted — with exactly the same keystroke that turns on the forum.
/// A control that makes the safe and the unsafe action identical is not a control.
///
/// THE ACKNOWLEDGEMENT IS THE POINT, NOT THE FLAG. Three of these features carry an obligation to
/// somebody who is not the operator: guests whose images are broadcast, candidates whose CVs are
/// stored, contributors whose words are published. For those, the flag will not move until the
/// corresponding prerequisite is recorded — by name, with who recorded it and when, in the
/// append-only audit. That is one deliberate extra act, not a refusal: the decision stays the
/// operator's, but it stops being a keystroke and starts being a signature.
///
/// The refusal lives in the SERVER, on the POST. Anyone can curl this API; a UI that merely greys
/// out a switch protects nobody. The client below renders what the server reports and nothing more.
///
/// AN ACKNOWLEDGEMENT IS NOT EVIDENCE. Recording one asserts that the outside-engineering work is
/// done; the software cannot verify that a provider is contracted or a notice published, and this
/// module never claims to. What it guarantees is that the assertion was made explicitly, by a named
/// owner, before the flag moved — and that it is still there to read afterwards.
/// </summary>
public static class WorldLaunch
{
    /// <summary>A prerequisite that must be recorded before its feature can be switched on.</summary>
    private record Ack(string Key, string Label, string Detail);

    /// <param name="WhenOff">What the URL actually does while the feature is off. This is per-feature
    /// because it is NOT uniform: /world/forum and /world/careers are server-rendered routes that
    /// really do 404, while /world-app/* is a static React shell that always returns 200 and gates
    /// itself on the API. A board that told an operator "returns 404" for all five would be wrong
    /// three times out of five, and wrong in the direction that makes a working deployment look
    /// broken.</param>
    private record Feature(
        string Flag,
        string Title,
        string TurnsOn,
        string Url,
        string WhenOff,
        string? DependsOn,
        Ack? Requires,
        string? Advisory);

    // The order here is the order the board renders in: the free-standing features first, then the
    // one that only makes sense once rooms exist.
    private static readonly Feature[] Features =
    {
        new("world_community_enabled",
            "Community rooms",
            "Text rooms with the moderation pipeline, reporting and the case queue. Members and invited guests can post; every message passes the allow verdict before it is broadcast.",
            "/world-app/community",
            "The page loads, but there are no rooms and nothing can be posted — the room API answers 404.",
            null,
            null,
            "Rooms generate a moderation queue from the first day. Turning this on without somebody rostered to read that queue means reports go unanswered, which is worse than the rooms being closed."),

        new("pciworld_forum_enabled",
            "Professional forum",
            "The public forum at /world/forum — threads, the trust ladder, member flags and the shared moderation queue. Threads are server-rendered, so they are crawlable and readable with JavaScript off.",
            "/world/forum",
            "That address returns 404 — the forum is a server-rendered route and does not exist while this is off.",
            null,
            null,
            "Forum posts are public and indexed. The trust ladder governs who can post without review; check its thresholds under Community before opening the doors."),

        new("pciworld_careers_enabled",
            "Careers",
            "The job board at /world/careers, the employer portal, applications and CV upload. Only employers in the 'verified' state can post or read applications.",
            "/world/careers",
            "That address returns 404 — the job board is a server-rendered route and does not exist while this is off.",
            null,
            new Ack("pciworld_ack_careers_privacy",
                "Candidate privacy notice is published",
                "Careers stores CVs and discloses them to employers. Record this once the published notice states what is stored, how long it is kept, and who it is shown to."),
            null),

        new("pciworld_contributors_enabled",
            "Contributor desk",
            "Applications to contribute, the contributor desk, and the editorial queue with its two-person rule — a contributor can never approve or publish their own article.",
            "/world-app/",
            "The World app loads as usual; the contributor desk is simply not among its sections, and the application form is unreachable.",
            null,
            new Ack("pciworld_ack_contributor_terms",
                "Editorial policy and contributor terms are published",
                "Contributors are told what they are agreeing to before they apply. Record this once the policy and terms are live at a URL the application form can link to."),
            null),

        new("pciworld_community_images_enabled",
            "Images in community rooms",
            "Lets members and guests attach images to room messages. Every image is held and scanned before any other person sees it — nothing is broadcast on upload.",
            "/world-app/community",
            "Rooms work as usual; the composer offers no attachment control and the upload endpoint answers 404.",
            "world_community_enabled",
            new Ack("pciworld_ack_image_moderation",
                "Image moderation provider is contracted, and the minimum age and jurisdictions are settled",
                "This is the one flag that can expose a third party to harm. The scanners in this codebase are a first pass, not a guarantee — no general classifier detects every illegal image. Record this only once a moderation provider is live under contract and counsel has settled the minimum age and the jurisdictions served."),
            null),
    };

    public static void Map(WebApplication app, Db db, Func<HttpRequest, AdminCtx?> owner, Action<long?, string, string?> log)
    {
        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); }
            catch { /* auditing must never break the action it records */ }
        }

        bool On(string flag) => Settings.Bool(db, flag, false);

        // An acknowledgement is stored as the settings value; who and when live alongside it so the
        // board can show them without a join, and the audit row is the copy that cannot be edited.
        Dictionary<string, object?>? AckState(Ack a)
        {
            var raw = Settings.Str(db, a.Key, "");
            if (raw.Length == 0) return null;
            try
            {
                var el = JsonDocument.Parse(raw).RootElement;
                return new Dictionary<string, object?>
                {
                    ["by"] = el.TryGetProperty("by", out var b) ? b.GetString() : null,
                    ["at"] = el.TryGetProperty("at", out var t) ? t.GetString() : null,
                    ["note"] = el.TryGetProperty("note", out var n) ? n.GetString() : null,
                };
            }
            catch
            {
                // A value set by hand through the generic Settings screen is still an acknowledgement;
                // it just has no attribution to show. Treat it as recorded rather than pretending it
                // is absent, which would let the board contradict the gate below.
                return new Dictionary<string, object?> { ["by"] = null, ["at"] = null, ["note"] = raw };
            }
        }

        Feature? Find(string flag) => Features.FirstOrDefault(f => f.Flag == flag);

        app.MapGet("/api/admin/world-launch", (HttpRequest req) =>
        {
            if (owner(req) is null) return Results.Json(new { error = "owner_only" }, statusCode: 403);

            var rows = Features.Select(f =>
            {
                var ack = f.Requires is null ? null : AckState(f.Requires);
                var depOn = f.DependsOn is null || On(f.DependsOn);
                return new Dictionary<string, object?>
                {
                    ["flag"] = f.Flag,
                    ["title"] = f.Title,
                    ["turns_on"] = f.TurnsOn,
                    ["url"] = f.Url,
                    ["when_off"] = f.WhenOff,
                    ["on"] = On(f.Flag),
                    ["advisory"] = f.Advisory,
                    ["depends_on"] = f.DependsOn,
                    ["depends_on_met"] = depOn,
                    ["depends_on_title"] = f.DependsOn is null ? null : Find(f.DependsOn)?.Title,
                    ["requires"] = f.Requires is null ? null : new Dictionary<string, object?>
                    {
                        ["key"] = f.Requires.Key,
                        ["label"] = f.Requires.Label,
                        ["detail"] = f.Requires.Detail,
                        ["recorded"] = ack,
                    },
                    // The single field the client should trust for "can this be switched on now".
                    ["can_enable"] = depOn && (f.Requires is null || ack is not null),
                };
            }).ToList();

            return Results.Json(new { rows });
        });

        app.MapPost("/api/admin/world-launch", async (HttpRequest req) =>
        {
            var adm = owner(req);
            if (adm is null) return Results.Json(new { error = "owner_only" }, statusCode: 403);

            var b = await H.Body(req);
            var flag = (H.GetS(b, "flag") ?? "").Trim();
            var on = H.GetBool(b, "on");
            if (on is null) return Results.Json(new { error = "on_required" }, statusCode: 400);

            // Only the flags on this board, by exact name. Without this the endpoint is an
            // owner-only write to any settings key at all, bypassing the per-prefix permission
            // rules the generic settings PATCH enforces.
            var f = Find(flag);
            if (f is null) return Results.Json(new { error = "unknown_flag" }, statusCode: 400);

            if (on == true)
            {
                if (f.DependsOn is not null && !On(f.DependsOn))
                    return Results.Json(new
                    {
                        error = "dependency_off",
                        requires_flag = f.DependsOn,
                        message = $"Turn on \"{Find(f.DependsOn)?.Title ?? f.DependsOn}\" first — this feature has nothing to attach to without it.",
                    }, statusCode: 409);

                if (f.Requires is not null && AckState(f.Requires) is null)
                    return Results.Json(new
                    {
                        error = "prerequisite_not_recorded",
                        requires_key = f.Requires.Key,
                        label = f.Requires.Label,
                        message = f.Requires.Detail,
                    }, statusCode: 409);
            }

            Settings.Put(db, f.Flag, on == true ? "1" : "0");

            // Turning a feature OFF leaves anything that depends on it enabled-but-inert. Say so
            // rather than silently changing a flag the operator did not ask about: an image flag
            // that reads '1' while rooms are closed is a lie waiting to be believed on the day the
            // rooms reopen.
            var orphaned = on == false
                ? Features.Where(x => x.DependsOn == f.Flag && On(x.Flag)).Select(x => x.Title).ToArray()
                : Array.Empty<string>();

            Audit(null, on == true ? "world_launch_enable" : "world_launch_disable", $"{f.Flag} by admin#{adm.Id}");
            log(adm.Id, on == true ? "world_launch_enable" : "world_launch_disable", f.Flag);

            return Results.Json(new { ok = true, flag = f.Flag, on = on == true, orphaned });
        });

        app.MapPost("/api/admin/world-launch/ack", async (HttpRequest req) =>
        {
            var adm = owner(req);
            if (adm is null) return Results.Json(new { error = "owner_only" }, statusCode: 403);

            var b = await H.Body(req);
            var key = (H.GetS(b, "key") ?? "").Trim();
            var note = (H.GetS(b, "note") ?? "").Trim();

            var f = Features.FirstOrDefault(x => x.Requires is not null && x.Requires.Key == key);
            if (f?.Requires is null) return Results.Json(new { error = "unknown_prerequisite" }, statusCode: 400);

            // Withdrawing is allowed and is not a delete: it clears the value so the flag locks
            // again, and the audit keeps both the recording and the withdrawal.
            var withdraw = H.GetBool(b, "withdraw") == true;
            if (withdraw)
            {
                Settings.Put(db, key, "");
                Audit(null, "world_launch_ack_withdraw", $"{key} by admin#{adm.Id}");
                log(adm.Id, "world_launch_ack_withdraw", key);
                return Results.Json(new { ok = true, key, recorded = (object?)null });
            }

            if (note.Length < 8)
                return Results.Json(new
                {
                    error = "note_required",
                    message = "Say what was done and where it can be checked — a name, a contract reference or a URL. This is the record somebody will read later.",
                }, statusCode: 400);

            var payload = JsonSerializer.Serialize(new
            {
                by = adm.Email,
                at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                note = note.Length > 500 ? note.Substring(0, 500) : note,
            });
            Settings.Put(db, key, payload);
            Audit(null, "world_launch_ack", $"{key} by admin#{adm.Id}: {note}");
            log(adm.Id, "world_launch_ack", $"{key}: {note}");

            return Results.Json(new { ok = true, key, recorded = JsonDocument.Parse(payload).RootElement });
        });
    }
}
