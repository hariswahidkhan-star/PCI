using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The public, crawlable careers read path (docs/pciworld/CCP_PHASE4_DESIGN.md §7; §18.3, D-009).
///
/// SERVER-RENDERED ON PURPOSE, following ForumRender: a job that only exists after JavaScript runs
/// is a job that, for practical purposes, is not in the index. The employer portal, the applicant
/// views and the verification queue are React; this is not.
///
/// THE HONESTY RULE IS THE WHOLE POINT (§7). Structured data is a claim to a search engine, and a
/// stale claim is a lie that outlives the page:
///
///   • <c>JobPosting</c> markup is emitted only while <see cref="CareersState.IsLive"/> is true —
///     computed AT RENDER TIME from the rows, so honesty never depends on a background sweep
///     having stamped `closed` yet. <see cref="CareersState.JobJsonLd"/> returns null the moment
///     the predicate fails, and null means NO script block, not a "closed" blob.
///   • A CLOSED posting's URL stays reachable — an applicant's saved link should explain itself,
///     not 404 — but it says "no longer accepting applications" in words, carries no JobPosting
///     block, and is <c>noindex</c>. The apply endpoint refuses independently; this page is the
///     courtesy, the server is the control.
///   • A DRAFT or WITHDRAWN posting, or ANY posting of a non-verified employer, is a public 404.
///     The existence of a tenant's draft is tenant-confidential, and a 403 would confirm it
///     exists — so the renderer returns null and the endpoint answers NotFound, indistinguishable
///     from a URL that never existed.
///   • Employer pages exist, and emit <c>Organization</c> markup, only once VERIFIED — `verified`
///     is an assertion PCI makes to candidates (§4.2), and this surface repeats it nowhere else.
///
/// Every listing query carries the state predicates IN THE SQL — a query that returned everything
/// and trimmed afterwards would be one refactor away from leaking drafts.
/// </summary>
public static class CareersRender
{
    /// <summary>The Phase 4 kill switch (§3). With the flag off every renderer returns null and
    /// the routes answer the standard World 404.</summary>
    public static bool Enabled(Db db) => Settings.Str(db, "pciworld_careers_enabled", "0") == "1";

    // The DEFAULT encoder, deliberately — not UnsafeRelaxedJsonEscaping. This JSON is embedded
    // inside <script type="application/ld+json"> and employer names are employer-authored: the
    // relaxed encoder leaves '<' and '>' literal, so a name containing </script> would close the
    // tag early and everything after it becomes live markup — stored XSS on a crawlable page.
    // The default encoder emits <, inert inside the script block and still valid JSON-LD to
    // every crawler. This exact bug was found and fixed in ForumRender during this increment.
    static readonly JsonSerializerOptions JsonOpts = new();

    // ── the list ──────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// <c>/world/careers</c> — every live posting, newest first. Null only when the feature is
    /// off: an empty board is still a page (the URL is stable and linked), it just says so.
    /// </summary>
    public static string? List(Db db, string nowUtc, string canonicalBase = "")
    {
        if (!Enabled(db)) return null;

        var jobs = LivePostings(db, nowUtc, employerId: null);
        var sb = new StringBuilder();
        sb.Append("<nav class=\"wf-crumbs\" aria-label=\"Breadcrumb\"><ol>");
        sb.Append("<li><a href=\"/world\">PCI World</a></li>");
        sb.Append("<li aria-current=\"page\">Careers</li>");
        sb.Append("</ol></nav>");
        sb.Append("<h1>Careers</h1>");
        sb.Append("<p class=\"wc-intro\">Roles posted by verified employers. Applications are made "
                + "here, with your consent recorded — no posting links out to an external form.</p>");

        if (jobs.Count == 0)
            sb.Append("<p class=\"wc-empty\" role=\"status\">There are no open roles right now.</p>");
        else
            AppendJobList(sb, jobs, showEmployer: true);

        return Document("Careers", "Project controls roles from verified employers on PCI World.",
                        canonicalBase + "/world/careers", "index, follow", sb.ToString());
    }

    // ── the job page ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// <c>/world/careers/{employerSlug}/{postingSlug}</c>. Null — a public 404 — for a missing
    /// posting, a draft, a withdrawn posting, any posting of a non-verified employer, or the
    /// feature being off. A closed (or deadline-passed) posting still renders, honestly.
    /// </summary>
    public static string? Job(Db db, string employerSlug, string postingSlug, string nowUtc, string canonicalBase = "")
    {
        if (!Enabled(db)) return null;

        var row = db.QueryOne(
            @"SELECT p.id, p.slug, p.title, p.description, p.location, p.employment_type,
                     p.salary_min_minor, p.salary_max_minor, p.currency,
                     p.state, p.published_at, p.closes_at,
                     e.slug AS employer_slug, e.name AS employer_name,
                     e.website AS employer_website, e.state AS employer_state
              FROM pciworld_job_postings p
              JOIN pciworld_employers e ON e.id = p.employer_id
              WHERE e.slug=? AND p.slug=?", employerSlug, postingSlug);
        if (row is null) return null;

        // A tenant's draft is tenant-confidential (§7). 404, never 403 — a 403 confirms it exists.
        // The employer gate uses the same single predicate as the publish transaction; anything
        // but 'verified' (draft, pending, suspended, a mangled value) fails closed to 404, which
        // is how one suspension UPDATE unpublishes the whole surface mid-session (§4.3).
        if (!CareersState.EmployerMayPublish(H.Str(row["employer_state"]) ?? "")) return null;
        var postingState = H.Str(row["state"]) ?? "";
        if (postingState is not ("published" or "closed")) return null;   // draft|withdrawn|unknown

        var title = H.Str(row["title"]) ?? "Role";
        var employerName = H.Str(row["employer_name"]) ?? "";
        var eSlug = H.Str(row["employer_slug"]) ?? employerSlug;
        var canonical = $"{canonicalBase}/world/careers/{Uri.EscapeDataString(eSlug)}/{Uri.EscapeDataString(H.Str(row["slug"]) ?? postingSlug)}";
        var employerPage = $"{canonicalBase}/world/employers/{Uri.EscapeDataString(eSlug)}";

        // THE HONESTY PREDICATE, at render time. `published` with a passed closes_at renders as
        // closed even though no sweep has stamped the row — the claim must die with the deadline.
        var jsonLd = CareersState.JobJsonLd(new CareersState.JobLd(
            H.L(row["id"]), title, H.Str(row["description"]),
            postingState, H.Str(row["employer_state"]) ?? "",
            employerName, employerPage, H.Str(row["employer_website"]),
            H.Str(row["published_at"]), H.Str(row["closes_at"]),
            H.Str(row["location"]), H.Str(row["employment_type"]),
            NullableL(row["salary_min_minor"]), NullableL(row["salary_max_minor"]),
            H.Str(row["currency"])), nowUtc);
        var live = jsonLd is not null;

        var sb = new StringBuilder();
        sb.Append("<article class=\"wc-job-page\">");
        sb.Append("<nav class=\"wf-crumbs\" aria-label=\"Breadcrumb\"><ol>");
        sb.Append("<li><a href=\"/world\">PCI World</a></li>");
        sb.Append("<li><a href=\"/world/careers\">Careers</a></li>");
        sb.Append($"<li aria-current=\"page\">{Esc(title)}</li>");
        sb.Append("</ol></nav>");
        sb.Append($"<h1>{Esc(title)}</h1>");
        sb.Append($"<p class=\"wc-meta\">Posted by <a href=\"/world/employers/{Uri.EscapeDataString(eSlug)}\">{Esc(employerName)}</a>");
        sb.Append(" <span class=\"wc-verified\">(verified employer)</span></p>");

        if (!live)
            // In words, not by a colour or a missing button alone — and role="status" so it is
            // announced. The apply endpoint refuses independently of what this page says.
            sb.Append("<p class=\"wc-closed\" role=\"status\">This role is no longer accepting applications.</p>");

        var facts = Facts(row);
        if (facts.Count > 0)
        {
            sb.Append("<ul class=\"wc-facts\">");
            foreach (var f in facts) sb.Append("<li>").Append(f).Append("</li>");
            sb.Append("</ul>");
        }

        var description = H.Str(row["description"]);
        if (!string.IsNullOrWhiteSpace(description))
            sb.Append("<div class=\"wc-desc\"><p>")
              .Append(Esc(description).Replace("\r\n", "\n").Replace("\n\n", "</p><p>").Replace("\n", "<br/>"))
              .Append("</p></div>");

        if (live)
            // In-platform apply only (§7): no external URL, ever — a listing that bounces
            // applicants elsewhere defeats the consent and audit model and is the classic
            // job-board phishing vector.
            sb.Append("<p class=\"wc-apply\">To apply, sign in to your PCI World account. "
                    + "Applications are made on this platform only — this posting never links to an external application form.</p>");

        sb.Append("</article>");
        if (jsonLd is not null)
            sb.Append("<script type=\"application/ld+json\">").Append(jsonLd).Append("</script>");

        var meta = string.IsNullOrWhiteSpace(description)
            ? $"{title} at {employerName} on PCI World Careers."
            : Truncate(description!, 160);
        return Document($"{title} — {employerName}", meta, canonical,
                        live ? "index, follow" : "noindex", sb.ToString());
    }

    // ── the employer page ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// <c>/world/employers/{slug}</c>. Renders — and emits <c>Organization</c> markup — only for
    /// a VERIFIED employer; everything else, including suspended, is a public 404 (§4.3, §7).
    /// </summary>
    public static string? Employer(Db db, string slug, string nowUtc, string canonicalBase = "")
    {
        if (!Enabled(db)) return null;

        var e = db.QueryOne("SELECT id, slug, name, website, state FROM pciworld_employers WHERE slug=?", slug);
        if (e is null) return null;
        if (!CareersState.EmployerMayPublish(H.Str(e["state"]) ?? "")) return null;

        var name = H.Str(e["name"]) ?? "";
        var eSlug = H.Str(e["slug"]) ?? slug;
        var website = H.Str(e["website"]);
        var canonical = $"{canonicalBase}/world/employers/{Uri.EscapeDataString(eSlug)}";

        var sb = new StringBuilder();
        sb.Append("<nav class=\"wf-crumbs\" aria-label=\"Breadcrumb\"><ol>");
        sb.Append("<li><a href=\"/world\">PCI World</a></li>");
        sb.Append("<li><a href=\"/world/careers\">Careers</a></li>");
        sb.Append($"<li aria-current=\"page\">{Esc(name)}</li>");
        sb.Append("</ol></nav>");
        sb.Append($"<h1>{Esc(name)}</h1>");
        // Stated in words: what "verified" means here, and no more than that.
        sb.Append("<p class=\"wc-verified\">Verified employer — the Project Controls Institute has "
                + "checked who is behind this organisation's postings.</p>");
        if (IsHttpUrl(website))
            sb.Append($"<p class=\"wc-meta\">Website: <a href=\"{Esc(website!)}\" rel=\"nofollow noopener\">{Esc(website!)}</a></p>");

        sb.Append("<h2>Open roles</h2>");
        var jobs = LivePostings(db, nowUtc, H.L(e["id"]));
        if (jobs.Count == 0)
            sb.Append("<p class=\"wc-empty\" role=\"status\">No open roles right now.</p>");
        else
            AppendJobList(sb, jobs, showEmployer: false);

        // Organization markup only once verified — this whole branch IS the verified branch. The
        // recorded website goes to sameAs; never free text from a posting.
        var org = new Dictionary<string, object?>
        {
            ["@context"] = "https://schema.org/",
            ["@type"] = "Organization",
            ["name"] = name,
            ["url"] = canonical,
        };
        if (IsHttpUrl(website)) org["sameAs"] = website;
        sb.Append("<script type=\"application/ld+json\">")
          .Append(JsonSerializer.Serialize(org, JsonOpts))
          .Append("</script>");

        return Document(name, $"{name} — verified employer on PCI World Careers.",
                        canonical, "index, follow", sb.ToString());
    }

    // ── shared pieces ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Live postings. The state predicates live IN THE SQL — posting `published`, employer
    /// `verified`, deadline not passed — following ForumRender's rule: a query that returned
    /// everything and trimmed afterwards is one refactor away from leaking drafts, and the SQL
    /// must stay load-bearing rather than shadowed by a second in-memory filter. (Datetimes are
    /// same-format "YYYY-MM-DD HH:MM:SS" UTC strings on both providers, so the string comparison
    /// is a sound instant comparison.)
    ///
    /// The one thing a lexical compare cannot do is fail CLOSED on an unparsable deadline — the
    /// write path stores `closes_at` as text, and garbage like "soon" sorts above any date, which
    /// would advertise the job for ever. So rows that CARRY a deadline are re-checked through the
    /// single honesty predicate, which refuses what it cannot parse. Rows without one are decided
    /// by the SQL alone.
    /// </summary>
    static List<Dictionary<string, object?>> LivePostings(Db db, string nowUtc, long? employerId)
    {
        var where = @"p.state='published' AND e.state='verified'
                      AND (p.closes_at IS NULL OR p.closes_at='' OR p.closes_at >= ?)";
        var sql = $@"SELECT p.slug, p.title, p.location, p.employment_type, p.state, p.closes_at,
                            p.published_at, e.slug AS employer_slug, e.name AS employer_name,
                            e.state AS employer_state
                     FROM pciworld_job_postings p
                     JOIN pciworld_employers e ON e.id = p.employer_id
                     WHERE {where}{(employerId is null ? "" : " AND p.employer_id=?")}
                     ORDER BY p.published_at DESC, p.id DESC";
        var rows = employerId is null ? db.Query(sql, nowUtc) : db.Query(sql, nowUtc, employerId);
        return rows.Where(r => string.IsNullOrWhiteSpace(H.Str(r["closes_at"]))
                || CareersState.IsLive(H.Str(r["state"]) ?? "", H.Str(r["employer_state"]) ?? "",
                                       H.Str(r["closes_at"]), nowUtc))
            .ToList();
    }

    /// <summary>List semantics for the listing — a UL of roles, each a link plus a plain-words
    /// meta line. Nothing here is conveyed by colour alone.</summary>
    static void AppendJobList(StringBuilder sb, List<Dictionary<string, object?>> jobs, bool showEmployer)
    {
        sb.Append("<ul class=\"wc-jobs\">");
        foreach (var j in jobs)
        {
            var href = $"/world/careers/{Uri.EscapeDataString(H.Str(j["employer_slug"]) ?? "")}/{Uri.EscapeDataString(H.Str(j["slug"]) ?? "")}";
            sb.Append("<li class=\"wc-job\">");
            sb.Append($"<h2><a href=\"{href}\">{Esc(H.Str(j["title"]))}</a></h2>");
            var bits = new List<string>();
            if (showEmployer) bits.Add(Esc(H.Str(j["employer_name"])));
            if (!string.IsNullOrWhiteSpace(H.Str(j["location"]))) bits.Add(Esc(H.Str(j["location"])));
            if (EmploymentTypeLabel(H.Str(j["employment_type"])) is { } et) bits.Add(et);
            if (H.Str(j["closes_at"]) is { Length: >= 10 } closes) bits.Add("Applications close " + Esc(closes[..10]));
            if (bits.Count > 0) sb.Append("<p class=\"wc-meta\">").Append(string.Join(" · ", bits)).Append("</p>");
            sb.Append("</li>");
        }
        sb.Append("</ul>");
    }

    /// <summary>The job page's fact list — only facts the row actually carries; nothing is
    /// defaulted or guessed, the same rule the JSON-LD builder follows.</summary>
    static List<string> Facts(Dictionary<string, object?> row)
    {
        var facts = new List<string>();
        if (!string.IsNullOrWhiteSpace(H.Str(row["location"])))
            facts.Add("Location: " + Esc(H.Str(row["location"])));
        if (EmploymentTypeLabel(H.Str(row["employment_type"])) is { } et)
            facts.Add("Type: " + et);
        var min = NullableL(row["salary_min_minor"]);
        var max = NullableL(row["salary_max_minor"]);
        var currency = H.Str(row["currency"]);
        // An amount without its currency is not an amount at all — shown only when both exist,
        // matching JobJsonLd exactly so the page never says more than the markup claims.
        if ((min.HasValue || max.HasValue) && !string.IsNullOrWhiteSpace(currency))
            facts.Add("Salary: " + Esc(currency!.Trim()) + " " + (min.HasValue && max.HasValue
                ? $"{Amount(min.Value)} to {Amount(max.Value)}"
                : Amount(min ?? max!.Value)));
        if (H.Str(row["closes_at"]) is { Length: >= 10 } closes)
            facts.Add("Applications close: " + Esc(closes[..10]));
        return facts;
    }

    /// <summary>Local vocabulary → words. Anything unknown is omitted rather than guessed.</summary>
    static string? EmploymentTypeLabel(string? t) => t switch
    {
        "full_time" => "Full time",
        "part_time" => "Part time",
        "contract" => "Contract",
        "internship" => "Internship",
        _ => null,
    };

    /// <summary>Minor units → display, via the platform's one exact conversion — no float.</summary>
    static string Amount(long minor) =>
        Money.ToDecimal(minor).ToString("#,##0.##", System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>An employer-recorded website renders as a link ONLY when it is plainly http(s).
    /// The value is tenant-supplied text; anything else (javascript:, data:, a stray note) is
    /// simply not linked — the filename-style veto, applied to hrefs.</summary>
    static bool IsHttpUrl(string? url) =>
        url is not null
        && (url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            || url.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// A COMPLETE document, per ForumRender's hard-won rule: a fragment with no title, no lang and
    /// no canonical is not "crawlable" in any sense that matters. Self-contained rather than the
    /// Institute shell because World-only deployments serve this surface too. `robots` is a
    /// parameter because the closed page must be noindex while staying reachable (§7).
    /// </summary>
    static string Document(string title, string description, string canonical, string robots, string body) =>
        "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"utf-8\"/>"
        + "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\"/>"
        + $"<title>{Esc(title)} — PCI World careers</title>"
        + $"<meta name=\"description\" content=\"{Esc(description)}\"/>"
        + $"<link rel=\"canonical\" href=\"{Esc(canonical)}\"/>"
        + $"<meta name=\"robots\" content=\"{robots}\"/>"
        + "<link rel=\"stylesheet\" href=\"/assets/world.css\"/>"
        + "</head><body>"
        // The skip link's target exists in every variant of every page — the axe suite checks.
        + "<a class=\"wf-skip\" href=\"#wc-main\">Skip to the content</a>"
        + "<main id=\"wc-main\">" + body + "</main>"
        + "</body></html>";

    static string Truncate(string s, int max)
    {
        var text = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ").Trim();
        return text.Length > max ? text[..(max - 1)].TrimEnd() + "…" : text;
    }

    static long? NullableL(object? v) => v is null ? null : H.L(v);

    static string Esc(string? s) => System.Net.WebUtility.HtmlEncode(s ?? "");
}
