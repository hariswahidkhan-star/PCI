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
///     records to do it, so the health report exposes none.
///   • Every write is gated on 'id_backfill', which sits outside every named role bundle — holding
///     'members' or even 'settings' does not get you the ability to reshape identities.
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
    }
}
