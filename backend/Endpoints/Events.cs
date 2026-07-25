using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Member events &amp; webinars (CPD-earning). Members browse published upcoming events and register; admins
/// create/manage events and mark attendance. Marking a registrant "attended" credits their CPD with an
/// APPROVED entry for the event's cpd_hours in its cpd_category — so events feed straight into the CPD
/// framework and recertification. Idempotent: a second attendance mark never double-credits.
/// </summary>
public static class Events
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);
        UserCtx? User(HttpRequest r) => Auth.UserFromReq(r, db);

        // ─────────────────────────── STUDENT ───────────────────────────
        // Published events (upcoming first), each annotated with the caller's registration state + seats left.
        app.MapGet("/api/me/events", (HttpContext ctx) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var rows = db.Query(@"SELECT id,title,summary,event_type,starts_at,ends_at,timezone,location,join_url,capacity,cpd_hours,cpd_category,status
                FROM events WHERE status='published' ORDER BY (starts_at IS NULL), starts_at ASC, id DESC");
            var outRows = rows.Select(e =>
            {
                var eid = H.L(e["id"]);
                var reg = db.QueryOne("SELECT status,attended_at FROM event_registrations WHERE event_id=? AND user_id=?", eid, u.Id);
                var taken = db.Scalar<long>("SELECT COUNT(*) FROM event_registrations WHERE event_id=? AND status IN ('registered','attended')", eid);
                var cap = H.L(e["capacity"]);
                var o = new Dictionary<string, object?>(e)
                {
                    ["registered"] = reg is not null && H.Str(reg["status"]) is "registered" or "attended",
                    ["attended"] = reg is not null && H.Str(reg["status"]) == "attended",
                    ["seats_left"] = cap > 0 ? Math.Max(0, cap - taken) : (long?)null,
                    // Only surface the join link to a registered member.
                    ["join_url"] = reg is not null && H.Str(reg["status"]) is "registered" or "attended" ? H.Str(e["join_url"]) : null,
                };
                return o;
            }).ToList();
            return J(new { rows = outRows });
        });

        app.MapPost("/api/me/events/{id}/register", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var e = db.QueryOne("SELECT id,capacity,status FROM events WHERE id=?", id);
            if (e is null || H.Str(e["status"]) != "published") return Results.Json(new { error = "not_found" }, statusCode: 404);
            var existing = db.QueryOne("SELECT id,status FROM event_registrations WHERE event_id=? AND user_id=?", id, u.Id);
            if (existing is not null && H.Str(existing["status"]) is "registered" or "attended")
                return J(new { ok = true, already = true });
            var cap = H.L(e["capacity"]);
            if (cap > 0)
            {
                var taken = db.Scalar<long>("SELECT COUNT(*) FROM event_registrations WHERE event_id=? AND status IN ('registered','attended')", id);
                if (taken >= cap) return Results.Json(new { error = "full", message = "This event is fully booked." }, statusCode: 409);
            }
            if (existing is not null) db.Execute("UPDATE event_registrations SET status='registered', registered_at=datetime('now') WHERE id=?", existing["id"]);
            else db.Execute("INSERT INTO event_registrations(event_id,user_id,status) VALUES(?,?, 'registered')", id, u.Id);
            log(u.Id, "event_register", id.ToString());
            return J(new { ok = true });
        });

        app.MapPost("/api/me/events/{id}/cancel", (HttpContext ctx, long id) =>
        {
            var u = User(ctx.Request); if (u is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            var reg = db.QueryOne("SELECT id,status FROM event_registrations WHERE event_id=? AND user_id=?", id, u.Id);
            if (reg is null) return Results.Json(new { error = "not_registered" }, statusCode: 404);
            if (H.Str(reg["status"]) == "attended") return Results.Json(new { error = "already_attended", message = "You have already attended this event." }, statusCode: 400);
            db.Execute("UPDATE event_registrations SET status='cancelled' WHERE id=?", reg["id"]);
            log(u.Id, "event_cancel", id.ToString());
            return J(new { ok = true });
        });

        // ─────────────────────────── ADMIN (gated: content) ───────────────────────────
        app.MapGet("/api/admin/events", (HttpContext ctx) => gate(ctx.Request, "content", _ =>
            J(new { rows = db.Query(@"SELECT e.*, (SELECT COUNT(*) FROM event_registrations r WHERE r.event_id=e.id AND r.status IN ('registered','attended')) registrations,
                (SELECT COUNT(*) FROM event_registrations r WHERE r.event_id=e.id AND r.status='attended') attended
                FROM events e ORDER BY (e.starts_at IS NULL), e.starts_at DESC, e.id DESC LIMIT 300") })));

        // Create or update (id present → update). Whitelisted fields only.
        app.MapPost("/api/admin/events", (HttpContext ctx) => gate(ctx.Request, "content", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var title = (H.GetS(b, "title") ?? "").Trim();
            if (title.Length is < 3 or > 200) return Results.Json(new { error = "bad_title", message = "Title must be 3–200 characters." }, statusCode: 400);
            var type = H.GetS(b, "event_type") ?? "webinar";
            if (type is not ("webinar" or "workshop" or "conference" or "course")) type = "webinar";
            var status = H.GetS(b, "status") ?? "draft";
            if (status is not ("draft" or "published" or "cancelled")) status = "draft";
            var vals = new object?[]
            {
                title, H.GetS(b, "summary"), H.GetS(b, "description"), type,
                H.GetS(b, "starts_at"), H.GetS(b, "ends_at"), H.GetS(b, "timezone"), H.GetS(b, "location"), H.GetS(b, "join_url"),
                (int)Math.Max(0, H.GetNum(b, "capacity") ?? 0), Math.Max(0, H.GetNum(b, "cpd_hours") ?? 0),
                H.GetS(b, "cpd_category") ?? "Events & webinars", status,
            };
            var id = H.GetNum(b, "id");
            if (id is > 0)
            {
                db.Execute(@"UPDATE events SET title=?,summary=?,description=?,event_type=?,starts_at=?,ends_at=?,timezone=?,location=?,join_url=?,
                    capacity=?,cpd_hours=?,cpd_category=?,status=?,updated_at=datetime('now') WHERE id=?", vals.Append((object?)(long)id.Value).ToArray());
                log(adm.Id, "event_update", ((long)id).ToString());
                return J(new { ok = true, id = (long)id });
            }
            var newId = db.ExecuteReturningId(@"INSERT INTO events(title,summary,description,event_type,starts_at,ends_at,timezone,location,join_url,capacity,cpd_hours,cpd_category,status,created_by)
                VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", vals.Append((object?)adm.Id).ToArray());
            log(adm.Id, "event_create", newId.ToString());
            return J(new { ok = true, id = newId });
        }));

        app.MapGet("/api/admin/events/{id}/registrations", (HttpContext ctx, long id) => gate(ctx.Request, "content", _ =>
            J(new { rows = db.Query(@"SELECT r.id,r.user_id,r.status,r.registered_at,r.attended_at,u.email,u.first_name,u.last_name
                FROM event_registrations r LEFT JOIN users u ON u.id=r.user_id WHERE r.event_id=? ORDER BY r.id DESC", id) })));

        // Mark (or unmark) attendance. Marking attended credits an APPROVED CPD entry for the event's hours,
        // once — a second call is idempotent (the linked cpd_entry_id guards against double-crediting).
        app.MapPost("/api/admin/events/{id}/attendance", (HttpContext ctx, long id) => gate(ctx.Request, "content", adm =>
        {
            var b = H.Body(ctx.Request).GetAwaiter().GetResult();
            var uid = (long)(H.GetNum(b, "user_id") ?? 0);
            var attended = H.GetBool(b, "attended") ?? true;
            var ev = db.QueryOne("SELECT id,title,cpd_hours,cpd_category,starts_at FROM events WHERE id=?", id);
            if (ev is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
            var reg = db.QueryOne("SELECT id,status,cpd_entry_id FROM event_registrations WHERE event_id=? AND user_id=?", id, uid);
            if (reg is null) return Results.Json(new { error = "not_registered" }, statusCode: 404);

            if (attended)
            {
                if (H.Str(reg["status"]) == "attended") return J(new { ok = true, already = true });   // idempotent
                long? cpdId = H.Ln(reg["cpd_entry_id"]);
                var hours = H.D(ev["cpd_hours"]);
                if (cpdId is null && hours > 0)
                {
                    var date = (H.Str(ev["starts_at"]) ?? DateTime.UtcNow.ToString("yyyy-MM-dd"));
                    if (date.Length >= 10) date = date[..10];
                    cpdId = db.ExecuteReturningId(@"INSERT INTO cpd_entries(user_id,activity_date,category,hours,description,status,reviewed_by,reviewed_at)
                        VALUES(?,?,?,?,?, 'approved', ?, datetime('now'))",
                        uid, date, H.Str(ev["cpd_category"]) ?? "Events & webinars", hours,
                        "Attended: " + (H.Str(ev["title"]) ?? "PCI event"), adm.Id);
                }
                db.Execute("UPDATE event_registrations SET status='attended', attended_at=datetime('now'), cpd_entry_id=? WHERE id=?", cpdId, reg["id"]);
                log(adm.Id, "event_attended", $"event {id} (+{H.D(ev["cpd_hours"])}h CPD) (subject {uid})");
                return J(new { ok = true, cpd_hours = H.D(ev["cpd_hours"]) });
            }
            else
            {
                // Un-mark attendance: revert to registered and remove the auto-credited CPD entry (if any).
                if (H.Ln(reg["cpd_entry_id"]) is { } cid) db.Execute("DELETE FROM cpd_entries WHERE id=?", cid);
                db.Execute("UPDATE event_registrations SET status='registered', attended_at=NULL, cpd_entry_id=NULL WHERE id=?", reg["id"]);
                log(adm.Id, "event_attendance_removed", $"event {id} (subject {uid})");
                return J(new { ok = true });
            }
        }));
    }
}
