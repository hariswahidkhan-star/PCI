using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// The PCI AI Project Leadership Certification Suite — the three co-launching credentials.
///
/// Naming rules (Master Naming Update): identifiers are stored CLEAN (no ) — code, slug and
/// credential_prefix are PCL-AI / PFL-AI / PML-AI and pcl-ai / pfl-ai / pml-ai. Trademark symbols
/// live only in DISPLAY fields (name, public_title, acronym designation, short_name).
///
/// Migration is in-place and id-stable: id 1 was PCP-AI → PCL-AI, id 2 was PFIP → PFL-AI, id 3 was
/// CPMD → PML-AI. Renames are keyed on the prior code, so they run once and never re-clobber later
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
                new { q = "Who is PCI PCL-AI for?", a = "Project controls, cost and planning leaders responsible for governing schedule, cost, risk, forecasting and performance — with the governed use of AI." },
                new { q = "Is an examination required?", a = "Yes — the PCI AI Project Controls Leader is examination-based. Founding and honorary routes have their own defined criteria." },
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
                new { q = "Who is PCI PFL-AI for?", a = "Professionals structuring, modelling, financing or advising on infrastructure and capital projects — analysts, lenders, developers and advisers." },
                new { q = "What does PCI PFL-AI cover?", a = "Investment appraisal, financial modelling, capital structure, coverage ratios (DSCR/LLCR/PLCR), bankability, PPP and concession structures, financial close and AI-enabled analysis." },
            },
        });
        var management = Json(new
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
                new { q = "Who is PCI PML-AI for?", a = "Project and delivery leaders who take projects end to end — initiation, governance, planning, execution and closure across predictive, agile and hybrid approaches." },
                new { q = "Is PCI PML-AI a project-management credential?", a = "Yes. PCI PML-AI is a comprehensive project management, leadership and delivery credential; its official title is PCI Project Management Leader – AI." },
            },
        });

        // Ensure the Finance + Delivery rows exist on fresh installs (final names directly).
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(2,'PFL-AI','PCI AI Project Finance Leader',
               'Project and infrastructure finance leadership: financial modelling, investment appraisal, capital structure, bankability, coverage ratios, PPPs and financial close — with AI-enabled analysis and human validation.',
               'PFL-AI',3,1,2,
               'PCI PFL-AI','PFL-AI','PCI AI Project Finance Leader',
               'Project finance, financial modelling, investment appraisal, capital structure, bankability, DSCR/LLCR/PLCR, PPPs and financial close — with AI-enabled analysis.',
               'Project Finance','Leader','Active','pfl-ai',
               'Project and infrastructure finance leaders — analysts, modellers, lenders, advisers and developers.',
               0,'PCI PFL-AI | PCI AI Project Finance Leader',
               'The PCI AI Project Finance Leader (PCI PFL-AI) credential covers project finance, financial modelling, capital structure, bankability, coverage ratios, PPP structures and financial close.',
               'pfl-ai, pci pfl-ai, project finance leader, project finance certification, financial modelling, dscr, ppp, bankability',
               ?)", finance);
        db.Execute(@"INSERT OR IGNORE INTO certifications
              (id,code,name,description,credential_prefix,expiry_years,active,sort_order,
               acronym,short_name,public_title,short_description,category,level,status,slug,audience,
               membership_required,meta_title,meta_description,keywords,content_json)
            VALUES(3,'PML-AI','PCI Project Management Leader – AI',
               'Comprehensive project management, leadership and delivery: initiation, governance, planning, execution, integrated cost/schedule/risk, agile and hybrid delivery, benefits realization and AI-enabled project management with human accountability.',
               'PML-AI',3,1,3,
               'PCI PML-AI','PML-AI','PCI Project Management Leader – AI',
               'Comprehensive project management, leadership and delivery — initiation, governance, planning, execution, integrated cost/schedule/risk, agile/hybrid delivery and AI-enabled project management.',
               'Project Management','Leader','Active','pml-ai',
               'Project managers, delivery and programme leaders.',
               0,'PCI PML-AI | PCI Project Management Leader – AI',
               'The PCI Project Management Leader – AI (PCI PML-AI) credential is a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management.',
               'pml-ai, pci pml-ai, project delivery leader, project management certification, project leadership, agile, hybrid delivery, benefits realization',
               ?)", management);

        // ── Migrate-once renames to the final Project Leadership Suite names (keyed on the prior code,
        //    id-stable, no duplicates, admin edits preserved once renamed). Per-cert taglines are cleared:
        //    the Suite uses a single portfolio tagline unless an admin sets one later. ──
        MigrateCert(db, 1, new[] { "PCP-AI", "PCL-AI" }, "PCL-AI", "PCI AI Project Controls Leader", "PCI PCL-AI", "PCL-AI",
            "pcl-ai", "PCL-AI", "Project Controls",
            "Project controls governance, cost, planning, EVM, forecasting, risk and predictive analytics — with AI-enabled project controls and human validation.",
            "The integrated project-controls leadership credential: governance, cost, planning, earned value, forecasting, risk and performance — with AI-enabled project controls and human validation.",
            "Project controls, cost, planning and PMO leaders.",
            "PCI PCL-AI | PCI AI Project Controls Leader",
            "The PCI AI Project Controls Leader (PCI PCL-AI) credential unites project-controls governance, cost, planning, earned value, forecasting and risk with AI-enabled project controls.",
            "pcl-ai, pci pcl-ai, project controls leader, project controls certification, earned value, forecasting, ai project controls",
            controls);
        MigrateCert(db, 2, new[] { "PFIP", "PFIP-AI" }, "PFL-AI", "PCI AI Project Finance Leader", "PCI PFL-AI", "PFL-AI",
            "pfl-ai", "PFL-AI", "Project Finance",
            "Project finance, financial modelling, investment appraisal, capital structure, bankability, DSCR/LLCR/PLCR, PPPs and financial close — with AI-enabled analysis.",
            "Project and infrastructure finance leadership: financial modelling, investment appraisal, capital structure, bankability, coverage ratios, PPPs and financial close — with AI-enabled analysis and human validation.",
            "Project and infrastructure finance leaders — analysts, modellers, lenders, advisers and developers.",
            "PCI PFL-AI | PCI AI Project Finance Leader",
            "The PCI AI Project Finance Leader (PCI PFL-AI) credential covers project finance, financial modelling, capital structure, bankability, coverage ratios, PPP structures and financial close.",
            "pfl-ai, pci pfl-ai, project finance leader, project finance certification, financial modelling, dscr, ppp, bankability",
            finance);
        MigrateCert(db, 3, new[] { "CPMD", "CPMD-AI", "PDL-AI" }, "PML-AI", "PCI Project Management Leader – AI", "PCI PML-AI", "PML-AI",
            "pml-ai", "PML-AI", "Project Management",
            "Comprehensive project management, leadership and delivery — initiation, governance, planning, execution, integrated cost/schedule/risk, agile/hybrid delivery and AI-enabled project management.",
            "Comprehensive project management, leadership and delivery: initiation, governance, planning, execution, integrated cost/schedule/risk, agile and hybrid delivery, benefits realization and AI-enabled project management with human accountability.",
            "Project managers, delivery and programme leaders.",
            "PCI PML-AI | PCI Project Management Leader – AI",
            "The PCI Project Management Leader – AI (PCI PML-AI) credential is a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management.",
            "pml-ai, pci pml-ai, project delivery leader, project management certification, project leadership, agile, hybrid delivery",
            management);
        RepairPrePdlPmlAlias(db, management);

        EnsureRoutes(db);
        EnsureDocuments(db);
        NameSweep(db);
    }

    /// <summary>Replace the retired project-controls credential names in stored content so no old name is
    /// served from the database (parallel to the static-HTML sweep). Case-sensitive REPLACE leaves lowercase
    /// slugs/hrefs (pcp-ai) intact; idempotent — once swept, the LIKE-guarded updates are no-ops.</summary>
    public static void NameSweep(Db db)
    {
        // (old, new) — longest/most-specific first so the acronym pass doesn't pre-empt the full title.
        var maps = new (string a, string b)[]
        {
            ("Certified Project Controls Professional — AI (PCP-AI)", "PCI AI Project Controls Leader (PCI PCL-AI)"),
            ("Certified Project Controls Professional – AI (PCP-AI)", "PCI AI Project Controls Leader (PCI PCL-AI)"),
            ("Certified Project Controls Professional — AI", "PCI AI Project Controls Leader"),
            ("Certified Project Controls Professional – AI", "PCI AI Project Controls Leader"),
            ("Certified Project Controls Professional", "PCI AI Project Controls Leader"),
            ("Certified Project Finance & Infrastructure Professional – AI", "PCI AI Project Finance Leader"),
            ("Certified Project Finance Professional – AI", "PCI AI Project Finance Leader"),
            ("Project Finance Leader – AI", "PCI AI Project Finance Leader"),
            ("PCI AI Project Delivery Leader", "PCI Project Management Leader – AI"),
            ("PCI Project Delivery Leader – AI", "PCI Project Management Leader – AI"),
            ("Project Delivery Leader – AI", "Project Management Leader – AI"),
            ("Project Delivery Leader", "Project Management Leader – AI"),
            ("PCP-AI", "PCL-AI"), ("PFIP-AI", "PFL-AI"), ("PFIP", "PFL-AI"),
            ("CPMD-AI", "PML-AI"), ("CPMD", "PML-AI"), ("PDL-AI", "PML-AI"),
            // Bare retired acronym LAST: by this point every "PCP-AI" is already "PCL-AI", so any
            // remaining "PCP" is a standalone use — "(PCP)", "the PCP…" (truncated card copy) — that the
            // hyphenated map could never match.
            ("PCP", "PCL-AI"),
            // Lowercase variants: SQL REPLACE is case-sensitive, and translated content, hrefs
            // (pmp-vs-aace-vs-pcp-ai.html) and chat keywords store the retired names in lowercase.
            ("pcp-ai", "pcl-ai"), ("pfip-ai", "pfl-ai"), ("pfip", "pfl-ai"),
            ("cpmd-ai", "pml-ai"), ("cpmd", "pml-ai"), ("pdl-ai", "pml-ai"),
            ("pcp", "pcl-ai"),
            ("certified project controls professional", "pci ai project controls leader"),
        };
        (string table, string col)[] targets =
        {
            ("page_blocks", "cvalue"), ("pages", "title"), ("pages", "meta_description"),
            ("faqs", "question"), ("faqs", "answer"),
            ("bok_domains", "name"), ("bok_domains", "description"), ("bok_domains", "bullets"),
            ("news", "title"), ("news", "body"), ("resources", "title"), ("resources", "description"),
            ("site_content", "cvalue"), ("cert_documents", "title"), ("cert_documents", "description"),
            // Content surfaces that predate the sweep and can also carry retired credential names:
            // chat knowledge base, support reply templates, student-document titles, document categories,
            // stored translations, public download-centre documents and nav labels.
            ("chat_kb", "question"), ("chat_kb", "answer"), ("chat_kb", "keywords"),
            ("support_templates", "title"), ("support_templates", "body"),
            ("documents", "title"), ("documents", "description"),
            ("document_categories", "name"),
            ("content_i18n", "cvalue"),
            ("public_documents", "title"), ("public_documents", "description"),
            ("nav_items", "label"),
        };
        foreach (var (table, col) in targets)
        {
            try
            {
                foreach (var (a, b) in maps)
                    db.Execute($"UPDATE {table} SET {col}=REPLACE({col},?,?) WHERE {col} LIKE ?", a, b, "%" + a + "%");
                // Second stage — suite phrasing: the original site was written around ONE credential, so
                // even after the acronym rename the stored copy still reads "the PCL-AI …" (singular).
                // Rewrite those phrasings to present the three-certification suite. Longest-first ordering
                // matters; the list mirrors the static-page sweep so DB-injected content matches the files.
                foreach (var (a, b) in SingularMaps)
                    db.Execute($"UPDATE {table} SET {col}=REPLACE({col},?,?) WHERE {col} LIKE ?", a, b, "%" + a + "%");
            }
            catch { /* table/column may not exist on an older DB — skip */ }
        }
        TrademarkStrip(db);
    }

    /// <summary>Remove every trademark symbol (™ and its HTML entities, plus stray ®) from stored,
    /// user-visible content. PCI has not instructed the platform to display registered or unregistered
    /// trademark symbols, so no PCI credential name may carry one. This runs unconditionally on every boot
    /// (idempotent — once stripped, the LIKE-guarded REPLACE is a no-op) so it heals production rows that a
    /// prior deployment seeded WITH the symbol, independent of whether MigrateCert re-runs for that cert.
    /// Internal codes (PCL-AI/PFL-AI/PML-AI) never contain a symbol, so they are untouched.</summary>
    static void TrademarkStrip(Db db)
    {
        // (table, column) covering every surface that can hold a rendered credential name.
        (string table, string col)[] cols =
        {
            ("certifications", "name"), ("certifications", "public_title"), ("certifications", "acronym"),
            ("certifications", "short_name"), ("certifications", "designation"), ("certifications", "short_description"),
            ("certifications", "description"), ("certifications", "audience"), ("certifications", "meta_title"),
            ("certifications", "meta_description"), ("certifications", "keywords"), ("certifications", "content_json"),
            ("certifications", "tagline"),
            ("page_blocks", "cvalue"), ("pages", "title"), ("pages", "meta_description"),
            ("faqs", "question"), ("faqs", "answer"),
            ("bok_domains", "name"), ("bok_domains", "description"), ("bok_domains", "bullets"),
            ("news", "title"), ("news", "body"), ("resources", "title"), ("resources", "description"),
            ("site_content", "cvalue"), ("cert_documents", "title"), ("cert_documents", "description"),
            ("nav_items", "label"), ("public_documents", "title"), ("public_documents", "description"),
        };
        // Each symbol/entity to erase. Case-sensitive REPLACE; the ™ glyph is U+2122.
        string[] marks = { "™", "&trade;", "&#8482;", "&#x2122;" };
        foreach (var (table, col) in cols)
            foreach (var m in marks)
            {
                try { db.Execute($"UPDATE {table} SET {col}=REPLACE({col},?,'') WHERE {col} LIKE ?", m, "%" + m + "%"); }
                catch { /* table/column may not exist on this DB — skip */ }
            }
    }

    static readonly (string a, string b)[] SingularMaps =
    {
            (@"Enrolment for the PCL-AI is now open.", @"Enrolment for the PCI certifications — PCL-AI, PFL-AI and PML-AI — is now open."),
            (@"The step-by-step roadmap to the PCL-AI: eligibility", @"The step-by-step roadmap to a PCI certification — PCL-AI, PFL-AI or PML-AI: eligibility"),
            (@"Your step-by-step path to the PCL-AI — from eligibility to credential.", @"Your step-by-step path to a PCI certification — PCL-AI, PFL-AI or PML-AI — from eligibility to credential."),
            (@"Earning the PCL-AI follows a clear path", @"Earning a PCI certification — PCL-AI, PFL-AI or PML-AI — follows a clear path"),
            (@"separate from the examined PCL-AI credential", @"separate from PCI's examined certifications — PCL-AI, PFL-AI and PML-AI"),
            (@"never the examined PCL-AI credential", @"never an examined PCI credential"),
            (@"You do not need to hold the PCL-AI credential", @"You do not need to hold a PCI credential"),
            (@"the PCL-AI rests on an independent", @"every PCI certification rests on an independent"),
            (@"the integrated judgement the PCL-AI certifies", @"the integrated judgement PCI certifications assess"),
            (@"Is this part of the PCL-AI exam", @"Is this part of the PCI examinations"),
            (@"assessed in the PCL-AI examination", @"assessed in the PCI examinations"),
            (@"body of knowledge and the PCL-AI credential", @"body of knowledge and the PCI certifications"),
            (@"events and the PCL-AI credential", @"events and the PCI certifications"),
            (@"Questions about the PCL-AI credential, membership", @"Questions about the PCI certifications, membership"),
            (@"Do I need the PCL-AI to be a member?", @"Do I need a PCI certification to be a member?"),
            (@"actively working toward the PCL-AI", @"actively working toward a PCI certification"),
            (@"professionals who hold the PCL-AI", @"professionals who hold a PCI certification"),
            (@"Why the PCL-AI matters here", @"Why PCI certification matters here"),
            (@"The PCL-AI proves a professional can", @"A PCI credential proves a professional can"),
            (@"so the PCL-AI is not yet a government-", @"so PCI certifications are not yet government-"),
            (@"pay the exam fee and sit the PCL-AI examination", @"pay the exam fee and sit your PCI examination"),
            (@"Is the PCL-AI accredited?", @"Are PCI certifications accredited?"),
            (@"team enrolment toward the PCL-AI", @"team enrolment toward the PCI certifications"),
            (@"Assess and certify competence through the PCL-AI.", @"Assess and certify competence through the PCI certifications."),
            (@"Is the PCL-AI relevant to", @"Are PCI certifications relevant to"),
            (@"keeps the PCL-AI credential impartial", @"keeps PCI credentials impartial"),
            (@"training aligned to the PCL-AI scope", @"training aligned to PCI certification scopes"),
            (@"within the PCL-AI, and the principle", @"within the PCI certifications, and the principle"),
            (@"The PCL-AI credential, and how to earn", @"The PCI certifications, and how to earn"),
            (@"What the PCL-AI credential covers", @"What PCI certifications cover"),
            (@"Explore the PCL-AI credential, Knowledge Centre", @"Explore the PCI certifications, Knowledge Centre"),
            (@"A single, rigorous credential", @"A rigorous credential"),
            (@"careers, events and the PCL-AI credential", @"careers, events and the PCI certifications"),
            (@"careers and the PCL-AI credential", @"careers and the PCI certifications"),
            (@"careers and the PCL-AI", @"careers and the PCI certifications"),
            (@"the PCL-AI programme", @"the PCI certification programmes"),
            (@"Can I sit the PCL-AI examination", @"Can I sit a PCI examination"),
            (@"A clear route toward the PCL-AI credential", @"A clear route toward the PCI certifications"),
            (@"what the PCL-AI tells an employer", @"what a PCI credential tells an employer"),
            (@"how the PCL-AI maps to the local market", @"how the PCI certifications map to the local market"),
            (@"the PCL-AI does not replace any statutory registration", @"the PCI certifications do not replace any statutory registration"),
            (@"an integrated standard such as the PCL-AI", @"an integrated standard such as the PCI certifications"),
            (@"How does the PCL-AI relate to German engineering qualifications", @"How do the PCI certifications relate to German engineering qualifications"),
            (@"Answers on the PCL-AI credential, the examination", @"Answers on the PCI certifications, the examinations"),
            (@"What is the PCL-AI credential?", @"What are the PCI certifications?"),
            (@"Who is the PCL-AI for?", @"Who are the PCI certifications for?"),
            (@"What the PCL-AI is, who it’s for and how it works", @"What the PCI certifications are, who they’re for and how they work"),
            (@"the integrated, AI-assisted discipline the PCL-AI assesses", @"the integrated, AI-assisted discipline the PCI certifications assess"),
            (@"Recertification keeps the PCL-AI a credible", @"Recertification keeps every PCI credential a credible"),
            (@"toward the PCL-AI — eligibility", @"toward a PCI certification — eligibility"),
            (@"preparing for the PCL-AI if that is the case", @"preparing for a PCI certification if that is the case"),
            (@"Sit the PCL-AI examination", @"Sit your PCI examination"),
            (@"Available once you hold the PCL-AI credential", @"Available once you hold a PCI credential"),
            (@"Hold the PCL-AI credential", @"Hold a PCI credential"),
            (@"and the PCL-AI competency map", @"and the PCI competency map"),
            (@"has earned the PCL-AI from the Project Controls Institute", @"has earned a PCI credential from the Project Controls Institute"),
            (@"Schedule and sit the PCL-AI exam", @"Schedule and sit your PCI exam"),
            (@"Sitting and passing the PCL-AI examination is required", @"Sitting and passing your PCI examination is required"),
            (@"booking or sitting the PCL-AI examination", @"booking or sitting your PCI examination"),
            (@"Recertification of the PCL-AI credential is", @"Recertification of a PCI credential is"),
            (@"whether or not you hold the PCL-AI", @"whether or not you hold a PCI credential"),
            (@"the PCL-AI credential is earned only through independent assessme", @"a PCI credential is earned only through independent assessme"),
            (@"access member rates and use the PCL-AI credential", @"access member rates and use your PCI credential"),
            (@"Passing the PCL-AI is the trigger", @"Passing a PCI examination is the trigger"),
            (@"start using the PCL-AI designation", @"start using your PCI designation"),
            (@"practitioners preparing for the PCL-AI join", @"practitioners preparing for a PCI certification join"),
            (@"Earn the PCL-AI credential", @"Earn a PCI credential"),
            (@"professional membership without the PCL-AI", @"professional membership without a PCI credential"),
            (@"core grade once the PCL-AI is held", @"core grade once a PCI credential is held"),
            (@"distinct from — the PCL-AI credential itself", @"distinct from — the PCI credentials themselves"),
            (@"Use of the PCL-AI post-nominal", @"Use of your PCI designation post-nominal"),
            (@"the PCL-AI is where your competence is assessed", @"the PCI examinations are where your competence is assessed"),
            (@"About the PCL-AI", @"About the PCI certifications"),
            (@"Your platform for the PCL-AI", @"Your platform for the PCI certifications"),
            (@"official platform for the PCL-AI", @"official platform for the PCI certifications"),
            (@"aligned to the PCL-AI body of knowledge", @"aligned to each certification's body of knowledge"),
            (@"everything for the PCL-AI into one place", @"everything for your PCI certification into one place"),
            (@"Make the PCL-AI your standard", @"Make PCI certification your standard"),
            (@"Set the PCL-AI as the bar", @"Set PCI certification as the bar"),
            (@"Does the PCL-AI replace", @"Do PCI certifications replace"),
            (@"value the PCL-AI —", @"value PCI certifications —"),
            (@"care about the PCL-AI", @"care about PCI certifications"),
            (@"Why employers value the PCL-AI", @"Why employers value PCI certifications"),
            (@"certifies professionals through the PCL-AI credential", @"certifies professionals through the PCI certifications"),
            (@"how the PCL-AI's integrated scope differs", @"how the PCI certifications' integrated scope differs"),
            (@"awards the PCL-AI — one", @"awards the PCI certifications —"),
            (@"Who should pursue the PCL-AI", @"Who should pursue a PCI certification"),
            (@"how the PCL-AI relates to established credentials", @"how the PCI certifications relate to established credentials"),
            (@"add the PCL-AI to demonstrate", @"add a PCI credential to demonstrate"),
            (@"Does the PCL-AI cover", @"Do the PCI certifications cover"),
            (@"with governed AI and the PCL-AI credential", @"with governed AI and the PCI certifications"),
            (@"examinable ground in the PCL-AI —", @"examinable ground in the PCI certifications —"),
            (@"counts toward the PCL-AI", @"counts toward the PCI certifications"),
            (@"View the PCL-AI", @"View the PCI certifications"),
            (@"If I later sit the PCL-AI", @"If I later sit a PCI examination"),
            (@"sit the PCL-AI examination", @"sit your PCI examination"),
            (@"staging ground for the PCL-AI", @"staging ground for the PCI certifications"),
            (@"shorten the route to the PCL-AI", @"shorten the route to a PCI certification"),
            (@"They map to the PCL-AI body of knowledge", @"They map to the PCI bodies of knowledge"),
            (@"and the PCL-AI requires only three years of experience", @"and the PCI certifications require only three years of experience"),
            (@"the role the PCL-AI is designed to play", @"the role the PCI certifications are designed to play"),
            (@"cohort development toward the PCL-AI", @"cohort development toward the PCI certifications"),
            (@"Governed AI is not an appendix to the PCL-AI", @"Governed AI is not an appendix to the PCI certifications"),
            (@"records who holds the PCL-AI and their current status", @"records who holds a PCI credential and their current status"),
            (@"records who holds the PCL-AI and what their status is", @"records who holds a PCI credential and what their status is"),
            (@"This is the discipline the PCL-AI examines", @"This is the discipline the PCI certifications examine"),
            (@"This is why the PCL-AI examines the transferable core", @"This is why the PCI certifications examine the transferable core"),
            (@"the global body of knowledge the PCL-AI standard draws on", @"the global body of knowledge the PCI standards draw on"),
            (@"Is the PCL-AI relevant alongside qualifications", @"Are the PCI certifications relevant alongside qualifications"),
            (@"adopt and recognise the PCL-AI as the benchmark", @"adopt and recognise the PCI certifications as the benchmark"),
            (@"Certification — the PCL-AI credential, eligibility", @"Certification — the PCI credentials, eligibility"),
            (@"Ask for the PCL-AI syllabus, fees or certification roadmap", @"Ask for a certification syllabus, fees or the certification roadmap"),
            (@"ask for the PCL-AI syllabus", @"ask for a certification syllabus"),
            (@"Is the PCL-AI recognised in", @"Are PCI certifications recognised in"),
            (@"Preparing for the PCL-AI from", @"Preparing for a PCI certification from"),
            (@"Choose the PCL-AI or the AIPC", @"Choose a PCI certification or the AIPC"),
            (@"Use the PCL-AI post-nominal", @"Use your PCI post-nominal"),
            (@"who has been awarded the PCL-AI and keeps it current", @"who has been awarded a PCI credential and keeps it current"),
            (@"How the PCL-AI examination is organised", @"How the PCI examinations are organised"),
            (@"sample items in the PCL-AI style", @"sample items in the PCI examination style"),
            (@"weighing the PCL-AI", @"weighing a PCI certification"),
            (@"then to the PCL-AI and Professional grade", @"then to a PCI certification and Professional grade"),
            (@"and the PCL-AI marks the point", @"and a PCI credential marks the point"),
            (@"managed development toward the PCL-AI", @"managed development toward the PCI certifications"),
            (@"controls leads toward the PCL-AI", @"controls leads toward the PCI certifications"),
            (@"developing staff toward the PCL-AI", @"developing staff toward the PCI certifications"),
            (@"it is what the PCL-AI credential certifies", @"it is what the PCI credentials certify"),
            (@"statement of what the PCL-AI examines", @"statement of what each PCI examination assesses"),
            (@"When you pay the PCL-AI exam fee", @"When you pay your PCI exam fee"),
            (@"weighting of the PCL-AI exam", @"weighting of the PCI exams"),
            (@"one team or programme toward the PCL-AI", @"one team or programme toward the PCI certifications"),
            (@"A fellow who has not earned the PCL-AI holds no certification", @"A fellow who has not earned a PCI credential holds no certification"),
            (@"project controls, the PCL-AI and governed AI", @"project controls, the PCI certifications and governed AI"),
            (@"Pursuing the PCL-AI from", @"Pursuing a PCI certification from"),
            (@"Working towards the PCL-AI from", @"Working towards a PCI certification from"),
            (@"Working toward the PCL-AI from", @"Working toward a PCI certification from"),
            (@"assessed across PCI and the PCL-AI", @"assessed across PCI and its certifications"),
            (@"How does the PCL-AI help", @"How do the PCI certifications help"),
            (@"exactly what the PCL-AI is designed to recognise", @"exactly what the PCI certifications are designed to recognise"),
            (@"AI-ready capability with the PCL-AI", @"AI-ready capability with the PCI certifications"),
            (@"who may sit the PCL-AI and how eligibility", @"who may sit a PCI examination and how eligibility"),
            (@"someone with the PCL-AI, you are relying", @"someone with a PCI credential, you are relying"),
            (@"List the PCL-AI as desirable", @"List a PCI credential as desirable"),
            (@"How the PCL-AI standard is governed", @"How the PCI standards are governed"),
            (@"the things that make the PCL-AI mean something", @"the things that make the PCI credentials mean something"),
            (@"the same one the PCL-AI teaches", @"the same one the PCI certifications teach"),
            (@"competence expectations the PCL-AI examination assesses", @"competence expectations the PCI examinations assess"),
            (@"part of the competence the PCL-AI assesses", @"part of the competence the PCI certifications assess"),
            (@"controls capability with the PCL-AI", @"controls capability with the PCI certifications"),
            (@"how the PCL-AI is earned", @"how a PCI credential is earned"),
            (@"who wants to earn the PCL-AI credential", @"who wants to earn a PCI credential"),
            (@"After you pass the PCL-AI examination", @"After you pass your PCI examination"),
            (@"requires passing the PCL-AI examination", @"requires passing a PCI examination"),
            (@"When you earn the PCL-AI, you receive", @"When you earn a PCI credential, you receive"),
            (@"representation of the PCL-AI", @"representation of the PCI credentials"),
            (@"what was certified: the PCL-AI covers", @"what was certified: each PCI credential covers"),
            (@"Earn the PCL-AI through independent assessment", @"Earn a PCI credential through independent assessment"),
            (@"experience maps to the PCL-AI", @"experience maps to the PCI certifications"),
            (@"How would I sit the PCL-AI exam", @"How would I sit a PCI exam"),
            (@"master document defining the PCL-AI certification scheme", @"master document defining the PCI certification scheme"),
            (@"the global body of knowledge the PCL-AI", @"the global body of knowledge the PCI standards"),
            (@"Is the PCL-AI still valuable now", @"Are the PCI certifications still valuable now"),
            (@"the named person holds the PCL-AI", @"the named person holds the stated PCI credential"),
            (@"protect the impartiality of the PCL-AI", @"protect the impartiality of the PCI certifications"),
            (@"You do not need to travel to pursue the PCL-AI", @"You do not need to travel to pursue a PCI certification"),
            (@"can resit the PCL-AI", @"can resit their PCI examination"),
            (@"protecting what the PCL-AI signals to employers", @"protecting what a PCI credential signals to employers"),
            (@"the PCL-AI examines them as applied judgement", @"the PCI certifications examine them as applied judgement"),
            (@"a student pathway to the PCL-AI and research collaboration", @"a student pathway to the PCI certifications and research collaboration"),
            (@"Awareness of the PCL-AI pathway", @"Awareness of the PCI certification pathways"),
            (@"No — the PCL-AI requires three years of experience", @"No — the PCI certifications require three years of experience"),
            (@"Sitting the PCL-AI from the Emirates", @"Sitting a PCI examination from the Emirates"),
            (@"Can I sit the PCL-AI exam from", @"Can I sit a PCI exam from"),
            (@"That is the line the PCL-AI is built around", @"That is the line the PCI certifications are built around"),
            (@"as a competency in its own right within the PCL-AI, not bolted on", @"as a competency in its own right within the PCI certifications, not bolted on"),
            (@"the working behaviour the PCL-AI examination is designed to test", @"the working behaviour the PCI examinations are designed to test"),
            (@"does the PCL-AI fit a generalist", @"do the PCI certifications fit a generalist"),
            (@"Explore the PCL-AI and how", @"Explore the PCI certifications and how"),
            (@"The examined PCL-AI credential is always earned by passing", @"An examined PCI credential is always earned by passing"),
            (@"The PCL-AI assesses the applied, governed use of AI", @"The PCI certifications assess the applied, governed use of AI"),
            (@"Explore the PCL-AI", @"Explore the PCI certifications"),
            (@"pay the exam fee and earn the PCL-AI credential", @"pay the exam fee and earn your PCI credential"),
            (@"The draft competency framework the PCL-AI credential assesses", @"The draft competency framework the PCI PCL-AI credential assesses"),
            (@"Sit the PCL-AI or AIPC online", @"Sit a PCI examination or the AIPC online"),
            (@"student membership, the PCL-AI exam, or both", @"student membership, your PCI certification exam, or both"),
            (@"the style and format of the PCL-AI examination", @"the style and format of the PCI examinations"),
            (@"the PCL-AI credential, eligibility, examinations", @"the PCI credentials, eligibility, examinations"),
            (@"the PCL-AI exam at USD 350", @"a PCI certification exam at USD 350"),
            (@"including where the PCL-AI fits and why AI", @"including where the PCI certifications fit and why AI"),
            (@"with governed AI and the PCL-AI.", @"with governed AI and the PCI certifications."),
    };

    /// <summary>Default Certuvo product mapping (per credential) and a starter document set (candidate
    /// handbook + Body of Knowledge) for each certification. Idempotent: never overwrites admin edits.</summary>
    public static void EnsureDocuments(Db db)
    {
        // Certuvo product defaults to the certification code; admin can remap per credential.
        db.Execute("UPDATE certifications SET certuvo_product=code WHERE certuvo_product IS NULL OR certuvo_product=''");
        foreach (var cert in db.Query("SELECT id,code,acronym FROM certifications"))
        {
            var cid = Convert.ToInt64(cert["id"]);
            var have = db.Scalar<long>("SELECT COUNT(*) FROM cert_documents WHERE certification_id=?", cid);
            if (have > 0) continue;
            var acr = (cert["acronym"] as string) ?? (cert["code"] as string) ?? "";
            db.Execute("INSERT INTO cert_documents(certification_id,kind,title,description,published,sort_order) VALUES(?,?,?,?,1,10)",
                cid, "handbook", $"{acr} Candidate Handbook", "Eligibility, application, examination rules and conduct for this credential.");
            db.Execute("INSERT INTO cert_documents(certification_id,kind,title,description,watermark,published,sort_order) VALUES(?,?,?,?,1,1,20)",
                cid, "bok", $"{acr} Body of Knowledge", "The competency framework and study syllabus — delivered as a personalised watermarked copy.");
        }
        EnsureBookFiles(db);
    }

    /// <summary>Attach the authored Body of Knowledge PDFs shipped with the app (books/&lt;code&gt;-bok.pdf)
    /// to their seeded cert_documents rows. Runs on every boot but only fills rows whose file is still
    /// missing, so an admin who replaces a book through the console is never clobbered. The bytes go
    /// through content-addressed private storage — the file itself is never web-served from disk.</summary>
    public static void EnsureBookFiles(Db db)
    {
        try
        {
            foreach (var cert in db.Query("SELECT id,code FROM certifications"))
            {
                var code = (cert["code"] as string) ?? "";
                if (code.Length == 0) continue;
                var path = Path.Combine("books", code.ToLowerInvariant() + "-bok.pdf");
                if (!File.Exists(path)) continue;
                var row = db.QueryOne("SELECT id,filename,storage_ref FROM cert_documents WHERE certification_id=? AND kind='bok' ORDER BY id LIMIT 1", cert["id"]);
                if (row is null) continue;
                var missing = string.IsNullOrWhiteSpace(row["storage_ref"] as string);
                var legacyPdlMaster = code == "PML-AI"
                    && string.Equals(row["filename"] as string, "pdl-ai-bok.pdf", StringComparison.OrdinalIgnoreCase);
                // Replace only the bundled legacy PDL-AI master. Any admin-uploaded replacement keeps its
                // own filename and storage reference, so it is never overwritten by a deployment.
                if (!missing && !legacyPdlMaster) continue;
                var bytes = File.ReadAllBytes(path);
                var stored = Core.Storage.Put(bytes, "application/pdf", "books");
                db.Execute("UPDATE cert_documents SET storage_ref=?, filename=?, mime='application/pdf', size_bytes=?, sha256=?, watermark=1, updated_at=datetime('now') WHERE id=?",
                    stored.Reference, code.ToLowerInvariant() + "-bok.pdf", bytes.LongLength, stored.Sha256, row["id"]);
                Console.WriteLine($"[seed] book attached: {code} Body of Knowledge ({bytes.LongLength / 1024} KB)");
            }
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] book files skipped: {e.Message}"); }
    }

    // The default application-route set (Phase 4). Every certification offers these; the admin enables,
    // disables, prices and dates each independently. Identifiers (route_key) are clean and stable.
    static readonly (string key, string label, string desc, int exam, int approval, int pub, string feeMode)[] DefaultRoutes =
    {
        ("standard","Standard Route","For candidates meeting the education and professional-experience requirements.",1,1,1,"standard"),
        ("founding","Founding Route","For early applicants approved under PCI's founding-professional criteria.",1,1,1,"custom"),
        ("honorary","Honorary Route","Recognising distinguished contribution to the profession — assessed, not examination-based.",0,1,1,"free"),
        ("sponsored","Sponsored Route","Employer-, institution-, government- or scholarship-funded applications.",1,0,1,"sponsored"),
        ("complimentary","Complimentary Route","A complimentary application approved by PCI.",1,1,1,"free"),
        ("waived_full","Fully Waived Route","A full fee waiver approved by PCI.",1,1,1,"free"),
        ("waived_partial","Partially Waived Route","A partial fee waiver approved by PCI.",1,1,1,"waived_partial"),
        ("test","Test User Route","Internal test applications — not shown on the public website.",1,0,0,"free"),
    };

    /// <summary>Create the per-certification routes table and ensure every active certification carries the
    /// full default route set (idempotent; existing/edited routes are never overwritten).</summary>
    public static void EnsureRoutes(Db db)
    {
        db.Exec(@"CREATE TABLE IF NOT EXISTS certification_routes(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            certification_id INTEGER NOT NULL DEFAULT 1,
            route_key TEXT NOT NULL,
            label TEXT, description TEXT,
            enabled INTEGER DEFAULT 1,
            public INTEGER DEFAULT 1,
            exam_required INTEGER DEFAULT 1,
            requires_approval INTEGER DEFAULT 1,
            fee_mode TEXT DEFAULT 'standard',
            fee_amount DECIMAL(12,2),
            discount_pct REAL,
            opens_at TEXT, closes_at TEXT,
            max_applications INTEGER, max_approvals INTEGER,
            certificate_wording TEXT,
            sort_order INTEGER DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_cert_route ON certification_routes(certification_id, route_key)");
        foreach (var cert in db.Query("SELECT id FROM certifications"))
        {
            var cid = Convert.ToInt64(cert["id"]);
            int so = 0;
            foreach (var r in DefaultRoutes)
            {
                so += 10;
                db.Execute(@"INSERT OR IGNORE INTO certification_routes
                    (certification_id,route_key,label,description,enabled,public,exam_required,requires_approval,fee_mode,sort_order)
                    VALUES(?,?,?,?,1,?,?,?,?,?)",
                    cid, r.key, r.label, r.desc, r.pub, r.exam, r.approval, r.feeMode, so);
            }
        }
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

    /// <summary>Some installations briefly used PML-AI before the PDL-AI release. Because the final
    /// machine code is also PML-AI, repair that older identity only when one of its identity fields is
    /// non-canonical. Once repaired, later boots leave descriptive/admin-editable content untouched.</summary>
    static void RepairPrePdlPmlAlias(Db db, string contentJson)
    {
        db.Execute(@"UPDATE certifications SET
              name='PCI Project Management Leader – AI', public_title='PCI Project Management Leader – AI',
              acronym='PCI PML-AI', short_name='PML-AI', slug='pml-ai', credential_prefix='PML-AI',
              category='Project Management', level='Leader',
              short_description='Comprehensive project management, leadership and delivery — initiation, governance, planning, execution, integrated cost/schedule/risk, agile/hybrid delivery and AI-enabled project management.',
              description='Comprehensive project management, leadership and delivery: initiation, governance, planning, execution, integrated cost/schedule/risk, agile and hybrid delivery, benefits realization and AI-enabled project management with human accountability.',
              audience='Project managers, delivery and programme leaders.', tagline=NULL, status='Active',
              meta_title='PCI PML-AI | PCI Project Management Leader – AI',
              meta_description='The PCI Project Management Leader – AI (PCI PML-AI) credential is a comprehensive project management, leadership and delivery credential covering governance, planning, execution, agile/hybrid delivery and AI-enabled project management.',
              keywords='pml-ai, pci pml-ai, project delivery leader, project management certification, project leadership, agile, hybrid delivery',
              content_json=?, updated_at=datetime('now')
            WHERE id=3 AND code='PML-AI' AND (
              COALESCE(name,'')<>'PCI Project Management Leader – AI'
              OR COALESCE(public_title,'')<>'PCI Project Management Leader – AI'
              OR COALESCE(acronym,'')<>'PCI PML-AI'
              OR COALESCE(short_name,'')<>'PML-AI'
              OR COALESCE(slug,'')<>'pml-ai'
              OR COALESCE(credential_prefix,'')<>'PML-AI'
              OR COALESCE(category,'')<>'Project Management')", contentJson);
    }
}
