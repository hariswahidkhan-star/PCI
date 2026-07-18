using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// Phase-1 seed for the multi-certification catalogue. Enriches the founding PCP-AI row (filling only
/// empty catalogue fields, so admin edits are never clobbered) and inserts PFIP and CPMD as first-class
/// certifications. Idempotent on both providers: INSERT OR IGNORE means an existing row (edited in the
/// admin console) is left exactly as-is.
/// </summary>
public static class MultiCert
{
    static string Json(object o) => JsonSerializer.Serialize(o);

    public static void Seed(Db db)
    {
        try { SeedInner(db); }
        catch (Exception e) { Console.Error.WriteLine($"[multicert] seed skipped: {e.Message}"); }
    }

    static void SeedInner(Db db)
    {
        // 1) Enrich the founding PCP-AI row (id 1). COALESCE keeps any admin-edited value; only NULLs fill.
        var pcpContent = Json(new
        {
            audience = "Project controls, cost, planning, scheduling and PMO professionals who integrate schedule, cost, risk, forecasting and governed AI.",
            competencies = new[]
            {
                "Project controls fundamentals", "Planning and scheduling", "Cost engineering and estimating",
                "Earned value management", "Forecasting and analytics", "Project risk", "Project finance foundations",
                "Contract and commercial awareness", "Governance and assurance", "Governed use of AI in project controls",
            },
            faqs = new[]
            {
                new { q = "Who is PCP-AI for?", a = "Project controls, cost and planning professionals who want one credential covering the integrated discipline, including the governed use of AI." },
                new { q = "Is an exam required?", a = "Yes — PCP-AI is examination-based. Founding and honorary routes have their own defined criteria." },
            },
        });
        db.Execute(@"UPDATE certifications SET
              acronym=COALESCE(acronym,'PCP-AI'),
              short_name=COALESCE(short_name,'PCP-AI'),
              public_title=COALESCE(public_title,'Certified Project Controls Professional — AI'),
              tagline=COALESCE(tagline,'The credential for the people who control projects.'),
              short_description=COALESCE(short_description,'Planning, cost engineering, project finance and governed AI — united in one rigorous credential.'),
              category=COALESCE(category,'Project Controls'),
              level=COALESCE(level,'Professional'),
              status=COALESCE(status,'Active'),
              slug=COALESCE(slug,'pcp-ai'),
              audience=COALESCE(audience,'Project controls, cost, planning and PMO professionals.'),
              membership_required=COALESCE(membership_required,0),
              meta_title=COALESCE(meta_title,'PCP-AI — Certified Project Controls Professional (AI) | PCI'),
              meta_description=COALESCE(meta_description,'The PCP-AI credential unites project controls, cost engineering and project finance with governed AI. Eligibility, routes, fees and exam structure.'),
              keywords=COALESCE(keywords,'pcp-ai, project controls certification, cost engineering, project finance, governed ai'),
              content_json=COALESCE(content_json,?)
            WHERE id=1", pcpContent);

        // 2) PFIP — Project Finance & Infrastructure Professional (id 2)
        var pfipContent = Json(new
        {
            audience = "Project finance analysts, infrastructure investment professionals, financial modellers, lenders, advisers and developers working on capital projects, PPPs and energy assets.",
            competencies = new[]
            {
                "Project finance fundamentals", "Investment appraisal", "Financial modelling", "Capital structure",
                "Debt and equity", "DSCR, LLCR and PLCR", "Project risk allocation", "EPC, O&M and offtake contracts",
                "PPP and concession models", "Financial close", "Infrastructure finance", "Energy finance",
                "Islamic and sustainable finance", "AI and analytics in project finance",
            },
            faqs = new[]
            {
                new { q = "Who is PFIP for?", a = "Professionals structuring, modelling, financing or advising on infrastructure and capital projects — analysts, lenders, developers and advisers." },
                new { q = "What does PFIP cover?", a = "Investment appraisal, financial modelling, capital structure, coverage ratios (DSCR/LLCR/PLCR), risk allocation, PPP and concession structures, financial close, and AI-assisted analytics." },
            },
        });
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,tagline,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(2,'PFIP','Project Finance & Infrastructure Professional',
               'A professional credential in project finance and infrastructure investment: investment appraisal, financial modelling, capital structure, coverage ratios, risk allocation, PPP and concession structures, financial close and AI-assisted analytics.',
               'PFIP',3,1,2,
               'PFIP','PFIP','Project Finance & Infrastructure Professional',
               'Structure, model and finance the infrastructure the world builds.',
               'Investment appraisal, financial modelling, capital structure, DSCR/LLCR/PLCR, PPP and project risk — with AI-assisted analytics.',
               'Project Finance','Professional','Active','pfip',
               'Project finance, infrastructure investment and financial-modelling professionals.',
               0,
               'PFIP — Project Finance & Infrastructure Professional | PCI',
               'The PFIP credential covers project finance, financial modelling, capital structure, coverage ratios, PPP structures and infrastructure investment. Eligibility, routes, fees and exam information.',
               'pfip, project finance certification, infrastructure finance, financial modelling, dscr, ppp, project finance professional',
               ?)", pfipContent);

        // 3) CPMD — Certified Project Management & Delivery Professional (id 3)
        var cpmdContent = Json(new
        {
            audience = "Project managers, delivery leads, programme managers and PMO professionals responsible for initiating, planning, executing and closing projects across predictive, agile and hybrid delivery.",
            competencies = new[]
            {
                "Project initiation", "Business case", "Governance", "Scope", "Planning", "Execution",
                "Cost, schedule and risk integration", "Leadership", "Stakeholder management", "Procurement",
                "Commercial management", "Agile, predictive and hybrid delivery", "Quality and change",
                "Benefits realization", "Sustainability", "AI-assisted project management", "Project closure",
            },
            faqs = new[]
            {
                new { q = "Who is CPMD for?", a = "Project and delivery professionals who lead projects end to end — initiation, governance, planning, execution, delivery and closure across predictive, agile and hybrid approaches." },
                new { q = "How is CPMD different from PCP-AI?", a = "CPMD centres on managing and delivering the project; PCP-AI centres on the integrated project-controls discipline (schedule, cost, EVM, forecasting and finance)." },
            },
        });
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,tagline,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(3,'CPMD','Certified Project Management & Delivery Professional',
               'A professional credential in project management and delivery: initiation, business case, governance, scope, planning, execution, integrated cost/schedule/risk control, leadership, procurement, commercial management, agile/predictive/hybrid delivery, benefits realization and AI-assisted project management.',
               'CPMD',3,1,3,
               'CPMD','CPMD','Certified Project Management & Delivery Professional',
               'Lead and deliver projects end to end.',
               'Initiation, governance, planning, execution, integrated cost/schedule/risk, leadership, procurement, agile/hybrid delivery and AI-assisted PM.',
               'Project Management','Professional','Active','cpmd',
               'Project managers, delivery leads and PMO professionals.',
               0,
               'CPMD — Certified Project Management & Delivery Professional | PCI',
               'The CPMD credential covers project initiation, governance, planning, execution, integrated cost/schedule/risk control, agile and hybrid delivery, benefits realization and AI-assisted project management.',
               'cpmd, project management certification, project delivery, agile, hybrid delivery, pmo, benefits realization',
               ?)", cpmdContent);

        // All three credentials launch together — no "Coming Soon". Flip any row still seeded as
        // Coming Soon to Active (idempotent; leaves any other admin-chosen status untouched).
        db.Execute("UPDATE certifications SET status='Active' WHERE id IN (2,3) AND status='Coming Soon'");
    }
}
