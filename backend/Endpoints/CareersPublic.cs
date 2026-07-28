using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// The careers marketplace's public, crawlable surface (docs/pciworld/CCP_PHASE4_DESIGN.md §7;
/// §18.3, D-009) — the read-side counterpart of <see cref="WorldCareers"/>, following the
/// ForumPublic split:
///
///   <c>/world/careers…</c>, <c>/world/employers/…</c>   server-rendered HTML for readers and crawlers
///   <c>/api/world/careers/…</c>                          authenticated JSON (WorldCareers.cs)
///
/// Reading needs no account. Everything the public may NOT see — drafts, withdrawn postings, any
/// posting of a non-verified employer, the feature being off — comes back from CareersRender as
/// null and is answered 404 here, so this handler does not know the rules and cannot get them
/// subtly different from the renderer. Never 403: a 403 would confirm a tenant-confidential draft
/// exists (§7).
///
/// "Now" is taken once per request and handed down, so the liveness decision and the JSON-LD
/// emission are computed from the same instant — honesty at render time, never dependent on a
/// background sweep having stamped `closed` (§7).
/// </summary>
public static class CareersPublic
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        static IResult Html(string? html) => html is null
            ? Results.NotFound()
            : Results.Content(html, "text/html; charset=utf-8");

        app.MapGet("/world/careers", (HttpContext ctx) =>
            Html(CareersRender.List(db, H.StampInMinutes(0), WorldUrl.Base(ctx.Request))));

        app.MapGet("/world/careers/{employer}/{slug}", (HttpContext ctx, string employer, string slug) =>
            Html(CareersRender.Job(db, employer, slug, H.StampInMinutes(0), WorldUrl.Base(ctx.Request))));

        app.MapGet("/world/employers/{slug}", (HttpContext ctx, string slug) =>
            Html(CareersRender.Employer(db, slug, H.StampInMinutes(0), WorldUrl.Base(ctx.Request))));
    }
}
