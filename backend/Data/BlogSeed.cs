namespace PCI.Backend.Data;

/// <summary>
/// First-run editorial scaffolding for the Content Centre blog: the default byline author and the content
/// pillar categories that the blog and news verticals share. Seeded idempotently by slug (INSERT OR IGNORE
/// against the UNIQUE slug column), so an operator's edits, renames or deletions are NEVER overwritten or
/// resurrected on redeploy, and nothing is ever duplicated. Blog POSTS are deliberately NOT seeded here —
/// articles enter through the editorial workflow (draft → review → publish), so nothing is auto-published.
/// </summary>
public static class BlogSeed
{
    public static void Ensure(Db db)
    {
        try
        {
            // Default institutional byline. admin_user_id is left null; an operator links it to a real admin
            // account to activate separation-of-duties for this author. The bio is factual — no fabricated
            // individuals or credentials.
            db.Execute(@"INSERT OR IGNORE INTO blog_authors(slug,name,title,bio,active,sort_order,created_at,updated_at)
                VALUES('pci-editorial-team','PCI Editorial Team','Project Controls Institute',
                'The editorial team of the Project Controls Institute publishes research, guidance and analysis on project controls, project finance and project delivery.',
                1, 0, datetime('now'), datetime('now'))");

            // Content pillar categories (blog + news share this taxonomy). Descriptions are factual and make
            // no accreditation, salary or employment claims.
            var cats = new (string slug, string name, string desc, int sort)[]
            {
                ("project-controls",       "Project Controls",       "Planning, scheduling, cost control, earned value and performance management.", 1),
                ("project-finance",        "Project Finance",        "Funding, cash flow, cost estimating, capital allocation and financial control of projects.", 2),
                ("project-delivery",       "Project Delivery",       "Delivery methods, governance, risk, change and controls assurance across the project lifecycle.", 3),
                ("ai-in-project-controls", "AI in Project Controls", "Applied and governed use of AI and analytics in project controls, finance and delivery.", 4),
                ("certification-cpd",      "Certification & CPD",    "PCI credentials, the Body of Knowledge, continuing professional development and exam guidance.", 5),
                ("industry-standards",     "Industry & Standards",   "Industry developments, standards and news relevant to project controls professionals.", 6),
            };
            foreach (var c in cats)
                db.Execute(@"INSERT OR IGNORE INTO blog_categories(slug,name,description,sort_order,active,created_at)
                    VALUES(?,?,?,?,1, datetime('now'))", c.slug, c.name, c.desc, c.sort);

            Console.WriteLine("[seed] blog authors/categories ensured");
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] blog scaffold skipped: {e.Message}"); }
    }
}
