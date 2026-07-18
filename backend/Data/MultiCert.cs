using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// The PCI AI Project Leadership Certification Suite — the three co-launching credentials.
///
/// Naming rules (Master Naming Update): identifiers are stored CLEAN (no ™) — code, slug and
/// credential_prefix are PCL-AI / PFL-AI / PDL-AI and pcl-ai / pfl-ai / pdl-ai. Trademark symbols
/// live only in DISPLAY fields (name, public_title, acronym designation, short_name).
///
/// Migration is in-place and id-stable: id 1 was PCP-AI → PCL-AI, id 2 was PFIP → PFL-AI, id 3 was
/// CPMD → PDL-AI. Renames are keyed on the prior code, so they run once and never re-clobber later
/// admin edits, and no duplicate rows are created.
/// </summary>
public static class MultiCert
{
    static string Json(object o) => JsonSerializer.Serialize(o);

    public const string PortfolioName = "PCI AI Project Leadership Certification Suite";
    public const string PortfolioTagline = "Finance intelligently. Control predictively. Deliver successfully.";

    public static void Seed(Db db)
    {
        try { SeedInner(db); }
        catch (Exception e) { Console.Error.WriteLine($"[multicert] seed skipped: {e.Message}"); }
    }

    static void SeedInner(Db db)
    {
        var controls = Json(new
        {
            audience = "Project controls, cost, planning and PMO leaders who integrate schedule, cost, risk, forecasting and AI-enabled performance control.",
            competencies = new[]
            {
                "Project controls governance", "Cost management", "Planning and scheduling", "Earned value management",
                "Forecasting and EAC", "Performance measurement", "Project risk", "Commercial and contract controls",
                "Predictive analytics", "AI-enabled project controls", "Digital reporting", "Automation",
                "Responsible AI", "Human validation",
            },
            faqs = new[]
            {
                new { q = "Who is PCI PCL-AI™ for?", a = "Project controls, cost and planning leaders responsible for governing schedule, cost, risk, forecasting and performance — with the governed use of AI." },
                new { q = "Is an examination required?", a = "Yes — the PCI AI Project Controls Leader™ is examination-based. Founding and honorary routes have their own defined criteria." },
            },
        });
        var finance = Json(new
        {
            audience = "Project and infrastructure finance leaders — analysts, modellers, lenders, advisers and developers structuring, financing and de-risking capital projects and PPPs.",
            competencies = new[]
            {
                "Project finance", "Infrastructure finance", "Financial modelling", "Investment appraisal",
                "Capital structure", "Debt and equity", "Project bankability", "DSCR, LLCR and PLCR",
                "PPPs and concessions", "Project contracts", "Financial close", "Commercial and financial risk",
                "Sustainable finance", "Islamic project finance", "AI-enabled financial modelling",
                "Predictive cash-flow analysis", "Digital due diligence", "Responsible AI", "Human validation",
            },
            faqs = new[]
            {
                new { q = "Who is PCI PFL-AI™ for?", a = "Professionals structuring, modelling, financing or advising on infrastructure and capital projects — analysts, lenders, developers and advisers." },
                new { q = "What does PCI PFL-AI™ cover?", a = "Investment appraisal, financial modelling, capital structure, coverage ratios (DSCR/LLCR/PLCR), bankability, PPP and concession structures, financial close and AI-enabled analysis." },
            },
        });
        var delivery = Json(new
        {
            audience = "Project managers, delivery and programme leaders responsible for initiating, planning, executing and closing projects across predictive, agile and hybrid delivery.",
            competencies = new[]
            {
                "Project management", "Project leadership", "Project initiation", "Business cases", "Governance",
                "Scope and requirements", "Planning and execution", "Stakeholder leadership", "Procurement",
                "Contracts and commercial delivery", "Cost, schedule and risk integration", "Predictive delivery",
                "Agile delivery", "Hybrid delivery", "Quality and change management", "Benefits realization",
                "Sustainability", "Project closure", "AI-enabled project management", "Decision intelligence",
                "Digital delivery", "Automation", "Responsible AI", "Human accountability",
            },
            faqs = new[]
            {
                new { q = "Who is PCI PDL-AI™ for?", a = "Project and delivery leaders who take projects end to end — initiation, governance, planning, execution and closure across predictive, agile and hybrid approaches." },
                new { q = "Is PCI PDL-AI™ a project-management credential?", a = "Yes. PCI PDL-AI™ is a comprehensive project management, leadership and delivery credential; its official title is PCI AI Project Delivery Leader™." },
            },
        });

        // Ensure the Finance + Delivery rows exist on fresh installs (final names directly).
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(2,'PFL-AI','PCI AI Project Finance Leader™',
               'Project and infrastructure finance leadership: financial modelling, investment appraisal, capital structure, bankability, coverage ratios, PPPs and financial close — with AI-enabled analysis and human validation.',
               'PFL-AI',3,1,2,
               'PCI PFL-AI™','PFL-AI™','PCI AI Project Finance Leader™',
               'Project finance, financial modelling, investment appraisal, capital structure, bankability, DSCR/LLCR/PLCR, PPPs and financial close — with AI-enabled analysis.',
               'Project Finance','Leader','Active','pfl-ai',
               'Project and infrastructure finance leaders — analysts, modellers, lenders, advisers and developers.',
               0,'PCI PFL-AI™ | PCI AI Project Finance Leader™',
               'The PCI AI Project Finance Leader™ (PCI PFL-AI™) credential covers project finance, financial modelling, capital structure, bankability, coverage ratios, PPP structures and financial close.',
               'pfl-ai, pci pfl-ai, project finance leader, project finance certification, financial modelling, dscr, ppp, bankability',
               ?)", finance);
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(3,'PDL-AI','PCI AI Project Delivery Leader™',
               'Comprehensive project management, leadership and delivery: initiation, governance, planning, execution, integrated cost/schedule/risk, agile and hybrid delivery, benefits realization and AI-enabled project management with human accountability.',
               'PDL-AI',3,1,3,
               'PCI PDL-AI™','PDL-AI™','PCI AI Project Delivery Leader™',
               'Comprehensive project management, leadership and delivery — initiation, governance, planning, execution, integrated cost/schedule/risk, agile/hybrid delivery and AI-enabled project management.',
               'Project Delivery','Leader','Active','pdl-ai',
               'Project managers, delivery and programme leaders.',
               0,'PCI PDL-AI™ | PCI AI Project Delivery Leader™',
               'The PCI AI Project Delivery Leader™ (PCI PDL-AI™) credential is a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management.',
               'pdl-ai, pci pdl-ai, project delivery leader, project management certification, project leadership, agile, hybrid delivery, benefits realization',
               ?)", delivery);

        // ── Migrate-once renames to the final Project Leadership Suite names (keyed on the prior code,
        //    id-stable, no duplicates, admin edits preserved once renamed). Per-cert taglines are cleared:
        //    the Suite uses a single portfolio tagline unless an admin sets one later. ──
        MigrateCert(db, 1, new[] { "PCP-AI" }, "PCL-AI", "PCI AI Project Controls Leader™", "PCI PCL-AI™", "PCL-AI™",
            "pcl-ai", "PCL-AI", "Project Controls",
            "Project controls governance, cost, planning, EVM, forecasting, risk and predictive analytics — with AI-enabled project controls and human validation.",
            "The integrated project-controls leadership credential: governance, cost, planning, earned value, forecasting, risk and performance — with AI-enabled project controls and human validation.",
            "Project controls, cost, planning and PMO leaders.",
            "PCI PCL-AI™ | PCI AI Project Controls Leader™",
            "The PCI AI Project Controls Leader™ (PCI PCL-AI™) credential unites project-controls governance, cost, planning, earned value, forecasting and risk with AI-enabled project controls.",
            "pcl-ai, pci pcl-ai, project controls leader, project controls certification, earned value, forecasting, ai project controls",
            controls);
        MigrateCert(db, 2, new[] { "PFIP", "PFIP-AI" }, "PFL-AI", "PCI AI Project Finance Leader™", "PCI PFL-AI™", "PFL-AI™",
            "pfl-ai", "PFL-AI", "Project Finance",
            "Project finance, financial modelling, investment appraisal, capital structure, bankability, DSCR/LLCR/PLCR, PPPs and financial close — with AI-enabled analysis.",
            "Project and infrastructure finance leadership: financial modelling, investment appraisal, capital structure, bankability, coverage ratios, PPPs and financial close — with AI-enabled analysis and human validation.",
            "Project and infrastructure finance leaders — analysts, modellers, lenders, advisers and developers.",
            "PCI PFL-AI™ | PCI AI Project Finance Leader™",
            "The PCI AI Project Finance Leader™ (PCI PFL-AI™) credential covers project finance, financial modelling, capital structure, bankability, coverage ratios, PPP structures and financial close.",
            "pfl-ai, pci pfl-ai, project finance leader, project finance certification, financial modelling, dscr, ppp, bankability",
            finance);
        MigrateCert(db, 3, new[] { "CPMD", "CPMD-AI", "PML-AI" }, "PDL-AI", "PCI AI Project Delivery Leader™", "PCI PDL-AI™", "PDL-AI™",
            "pdl-ai", "PDL-AI", "Project Delivery",
            "Comprehensive project management, leadership and delivery — initiation, governance, planning, execution, integrated cost/schedule/risk, agile/hybrid delivery and AI-enabled project management.",
            "Comprehensive project management, leadership and delivery: initiation, governance, planning, execution, integrated cost/schedule/risk, agile and hybrid delivery, benefits realization and AI-enabled project management with human accountability.",
            "Project managers, delivery and programme leaders.",
            "PCI PDL-AI™ | PCI AI Project Delivery Leader™",
            "The PCI AI Project Delivery Leader™ (PCI PDL-AI™) credential is a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management.",
            "pdl-ai, pci pdl-ai, project delivery leader, project management certification, project leadership, agile, hybrid delivery",
            delivery);
    }

    static void MigrateCert(Db db, long id, string[] oldCodes, string code, string name, string designation, string shortName,
        string slug, string prefix, string category, string shortDesc, string description, string audience,
        string metaTitle, string metaDesc, string keywords, string contentJson)
    {
        var inClause = string.Join(",", oldCodes.Select(_ => "?"));
        var args = new List<object?> { code, name, name, designation, shortName, slug, prefix, category, "Leader",
            shortDesc, description, audience, metaTitle, metaDesc, keywords, contentJson, id };
        args.AddRange(oldCodes);
        db.Execute($@"UPDATE certifications SET
              code=?, name=?, public_title=?, acronym=?, short_name=?, slug=?, credential_prefix=?, category=?, level=?,
              short_description=?, description=?, audience=?, tagline=NULL, status='Active',
              meta_title=?, meta_description=?, keywords=?, content_json=?, updated_at=datetime('now')
            WHERE id=? AND code IN ({inClause})", args.ToArray());
    }
}
