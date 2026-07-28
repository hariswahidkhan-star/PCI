using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Canonical identity administration: the health of the PCI Student Number estate, and the
/// controlled backfill that fills the gaps left by the era when GET /api/me minted numbers lazily.
///
/// Three deliberate constraints run through this module:
///
///   • There is no endpoint that sets a Student Number to a value an admin chose. Issuance belongs
///     to the creating transaction or to an audited backfill; a number an operator can type is a
///     number an operator can collide, and the whole point of the registry is that it cannot happen.
///   • Reads report COUNTS, not people. An operator diagnosing the estate does not need member
///     records to do it, so the health report exposes none and the merge preview shows per-table
///     counts, never member records.
///   • Every write is gated on an 'identity' permission ('id_backfill' for the backfill,
///     'id_merge_request'/'id_merge_approve' for merges) — all outside every named role bundle, so
///     holding 'members' or even 'settings' does not get you the ability to reshape identities.
///     Merges add a second person: the maker (id_merge_request) can never approve their own
///     request, even if they also hold id_merge_approve (Core/IdentityMerge.cs).
/// </summary>
public static class AdminIdentity
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        IResult J(object o) => Results.Json(o);

        // ── health: counts only, safe to surface on a dashboard ──
        app.MapGet("/api/admin/identity/student-numbers/health", (HttpContext ctx) =>
            gate(ctx.Request, "id_read", _ => J(new { ok = true, health = StudentNumberBackfill.Health(db) })));

        // ── preview: what a run WOULD do; writes nothing ──
        // POST rather than GET because it is the deliberate first half of a two-step operation and
        // should not be something a browser can be tricked into pre-fetching; it still writes nothing.
        app.MapPost("/api/admin/identity/student-numbers/backfill/preview", async (HttpContext ctx) =>
            await Task.FromResult(gate(ctx.Request, "id_backfill", adm =>
            {
                var limit = int.TryParse(ctx.Request.Query["limit"], out var l) ? l : 100;
                var preview = StudentNumberBackfill.Preview(db, limit);
                log(adm.Id, "identity_backfill_preview", $"missing={preview["total_missing"]}");
                return J(new { ok = true, preview });
            })));

        // ── run: issue numbers for one batch ──
        app.MapPost("/api/admin/identity/student-numbers/backfill/run", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "id_backfill", adm =>
            {
                var batch = (int)(H.GetNum(b, "batch_size") ?? 200);
                // The correlation id ties every reservation this run creates to this one operator
                // action, so a later reconciliation can say which run produced which number without
                // any of them carrying an admin identity into the ledger's public-facing fields.
                var correlation = Security.RandomHex(8);
                var result = StudentNumberBackfill.Run(db, batch, correlation);
                log(adm.Id, "identity_backfill_run",
                    $"correlation={correlation} issued={result["issued"]} quarantined={result["quarantined"]} remaining={result["remaining"]}");
                return J(new { ok = true, correlation_id = correlation, result });
            });
        });

        // ── reconcile: ledger versus projection ──
        app.MapGet("/api/admin/identity/student-numbers/reconcile", (HttpContext ctx) =>
            gate(ctx.Request, "id_audit", _ => J(new { ok = true, reconciliation = StudentNumberBackfill.Reconcile(db) })));

        // ══ maker-checker duplicate-account merges (spec §4.11) ══
        // Two authorized admins, never one: id_merge_request creates, a DIFFERENT admin holding
        // id_merge_approve decides. Execution lives in Core/IdentityMerge.cs, in one transaction.

        // ── create a request (the maker) ──
        app.MapPost("/api/admin/identity/merges", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "id_merge_request", adm =>
            {
                var source = (long)(H.GetNum(b, "source_user_id") ?? 0);
                var target = (long)(H.GetNum(b, "target_user_id") ?? 0);
                if (source <= 0 || target <= 0)
                    return Results.Json(new { error = "bad_request", message = "source_user_id and target_user_id are required." }, statusCode: 400);
                var (id, err) = IdentityMerge.CreateRequest(db, source, target, H.GetS(b, "reason"), adm.Id);
                if (err is not null)
                    return Results.Json(new { error = err }, statusCode: err switch
                    {
                        "unknown_user" => 404,
                        "already_pending" => 409,
                        _ => 400,
                    });
                log(adm.Id, "identity_merge_requested", $"merge={id} source={source} target={target}");
                return J(new { ok = true, id });
            });
        });

        // ── list / inspect (id_read). The detail IS the preview: per-table counts for both
        //    accounts — the blast radius, never member records. ──
        app.MapGet("/api/admin/identity/merges", (HttpContext ctx) =>
            gate(ctx.Request, "id_read", _ => J(new { ok = true, rows = IdentityMerge.List(db) })));

        app.MapGet("/api/admin/identity/merges/{id}", (HttpContext ctx, long id) =>
            gate(ctx.Request, "id_read", _ =>
                IdentityMerge.Get(db, id) is { } detail
                    ? J(new { ok = true, merge = detail })
                    : Results.Json(new { error = "not_found" }, statusCode: 404)));

        // ── approve + execute (the checker — never the maker) ──
        app.MapPost("/api/admin/identity/merges/{id}/approve", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "id_merge_approve", adm =>
            {
                var (err, result) = IdentityMerge.Approve(db, id, adm.Id, H.GetS(b, "note"));
                if (err is not null)
                    return Results.Json(new { error = err }, statusCode: err switch
                    {
                        "not_found" => 404,
                        "maker_checker" or "already_decided" => 409,
                        _ => 400,
                    });
                log(adm.Id, "identity_merge_approved",
                    $"merge={id} source={result!["source_user_id"]} target={result["target_user_id"]} correlation={result["correlation_id"]}");
                return J(new { ok = true, result });
            });
        });

        // ── reject (terminal; a maker may withdraw their own — refusing to merge is safe) ──
        app.MapPost("/api/admin/identity/merges/{id}/reject", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "id_merge_approve", adm =>
            {
                var err = IdentityMerge.Reject(db, id, adm.Id, H.GetS(b, "note"));
                if (err is not null)
                    return Results.Json(new { error = err }, statusCode: err == "not_found" ? 404 : 409);
                log(adm.Id, "identity_merge_rejected", $"merge={id}");
                return J(new { ok = true });
            });
        });
    }
}
