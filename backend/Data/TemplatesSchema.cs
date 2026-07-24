namespace PCI.Backend.Data;

/// <summary>
/// PCI Free Templates Library (§6A–6C) — data model + house content installer.
///
/// A public, free, lead-generating library of ready-to-use project-controls templates (WBS, EVM tracker,
/// risk register, cash-flow forecast, RACI, …). Each template is a small text file (CSV) whose content is
/// stored inline in the row — so there is no binary-file/storage surface to secure, the whole library is
/// seeded deterministically, and an operator can edit a template's body in the admin console like any other
/// content. All content is SYNTHETIC and illustrative; nothing here is student data or an exam answer key.
///
/// Follows the MarketingSchema / SimLabSchema pattern: a self-contained, idempotent Ensure(db) written in the
/// SQLite dialect that Db.cs auto-translates to MySQL, run on every boot on BOTH providers so table-set
/// parity holds. Seeds are guarded WHERE NOT EXISTS (by slug) so operator edits are never clobbered.
/// </summary>
public static class TemplatesSchema
{
    public static void Ensure(Db db)
    {
        db.Exec(@"CREATE TABLE IF NOT EXISTS templates(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(80) UNIQUE NOT NULL,
            title TEXT NOT NULL,
            category VARCHAR(40) NOT NULL DEFAULT 'general',   -- scope|schedule|cost|evm|risk|change|cashflow|finance|delivery|quality
            certification_id INTEGER,                          -- 1=PCL-AI, 2=PFL-AI, 3=PML-AI; NULL = any
            summary TEXT,
            format VARCHAR(12) NOT NULL DEFAULT 'csv',         -- file extension / mime family (csv today; extensible)
            body TEXT NOT NULL,                                -- the template content, served verbatim as the download
            published INTEGER NOT NULL DEFAULT 1,              -- 0 = draft (admin-only), 1 = live on the public library
            sort_order INTEGER DEFAULT 0,
            download_count INTEGER DEFAULT 0,
            created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')),
            updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_templates_pub ON templates(published)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_templates_cat ON templates(category)");

        // §6C — per-template download analytics. A daily aggregate (one row per template per UTC day) rather
        // than a per-event log, so the table stays bounded and a 30-day trend is a cheap range scan. The unique
        // (template_id, day) pair lets the download path do an INSERT-OR-IGNORE + UPDATE upsert on both providers.
        db.Exec(@"CREATE TABLE IF NOT EXISTS template_download_daily(
            template_id INTEGER NOT NULL,
            day VARCHAR(10) NOT NULL,                          -- UTC calendar day, YYYY-MM-DD
            count INTEGER NOT NULL DEFAULT 0,
            PRIMARY KEY(template_id, day))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_tpl_dl_day ON template_download_daily(day)");

        Seed(db);

        // Make the library discoverable: a footer nav link under the existing "Resources" group. Idempotent
        // (WHERE NOT EXISTS by url) so an operator who renames/moves it in the admin console keeps their edit.
        db.Execute(@"INSERT INTO nav_items(label,url,nav_group,sort_order,visible)
            SELECT 'Free templates','/free-templates.html','Resources', 2, 1
            WHERE NOT EXISTS(SELECT 1 FROM nav_items WHERE url='/free-templates.html')");
    }

    static void Seed(Db db)
    {
        var order = 0;
        foreach (var (slug, title, category, cert, summary, body) in House)
            SeedTemplate(db, slug, title, category, cert, summary, body, order++);
    }

    static void SeedTemplate(Db db, string slug, string title, string category, long cert, string summary, string body, int sort)
    {
        // Idempotent by slug — an operator who edits a template's body/title keeps their changes on the next boot.
        db.Execute(@"INSERT INTO templates(slug,title,category,certification_id,summary,format,body,published,sort_order)
            SELECT ?,?,?,?,?, 'csv', ?, 1, ?
            WHERE NOT EXISTS(SELECT 1 FROM templates WHERE slug=?)",
            slug, title, category, cert, summary, body, sort, slug);
    }

    // ── House content pack (synthetic). Each body is a valid CSV: a clear header row plus a few illustrative
    //    rows so the download is immediately usable. Cert mapping: 1 = PCL-AI (project controls),
    //    2 = PFL-AI (project finance), 3 = PML-AI (project delivery / management). ──
    static readonly (string slug, string title, string category, long cert, string summary, string body)[] House =
    {
        ("wbs-template", "Work Breakdown Structure (WBS) template", "scope", 1,
            "Roll project scope down to work packages with leaf budgets; each parent equals the sum of its children (the 100% rule).",
            """
            WBS ID,Parent ID,Element Name,Description,Responsible,Leaf Budget,Notes
            1,,Office Fit-Out,Whole project,PM,,Parent = sum of children
            1.1,1,Design,Concept + detailed design,Design Lead,40000,
            1.2,1,Build,Construction works,Site Manager,,Parent of 1.2.x
            1.2.1,1.2,Structure,Frame and envelope,Structural,30000,
            1.2.2,1.2,Services,MEP install,Services,20000,
            1.3,1,Handover,Commissioning + closeout,PM,10000,
            """),

        ("evm-tracker", "Earned Value (EVM) monthly tracker", "evm", 1,
            "Record PV, EV and AC each period; the formula columns show the standard earned-value measures to compute.",
            """
            Period,PV (Planned Value),EV (Earned Value),AC (Actual Cost),BAC,SV = EV-PV,CV = EV-AC,SPI = EV/PV,CPI = EV/AC,EAC = BAC/CPI
            1,100000,90000,95000,600000,,,,,
            2,220000,200000,230000,600000,,,,,
            3,350000,300000,360000,600000,,,,,
            """),

        ("cbs-cost-plan", "Cost Breakdown Structure & control-account plan", "cost", 1,
            "Track budget vs committed vs actual vs forecast per control account; variance = budget minus estimate at completion.",
            """
            Account Code,Account Name,Budget,Committed,Actual to Date,Forecast to Complete,Estimate at Completion,Variance = Budget-EAC
            CA-01,Site preliminaries,120000,80000,60000,55000,115000,
            CA-02,Civil works,300000,210000,150000,160000,310000,
            CA-03,Mechanical & electrical,180000,90000,40000,140000,180000,
            Total,,600000,,,,,
            """),

        ("risk-register", "Quantitative risk register (EMV)", "risk", 1,
            "Score probability x impact for each risk; Expected Monetary Value nets threats (negative) against opportunities (positive).",
            """
            Risk ID,Description,Category,Probability (0-1),Impact (+opp / -threat),EMV = Prob x Impact,Response Strategy,Owner,Status
            R1,Supplier delivery delay,Schedule,0.3,-20000,,Mitigate,Procurement,Open
            R2,Scope creep on interfaces,Scope,0.5,-10000,,Avoid,PM,Open
            R3,Early-handover incentive,Commercial,0.2,15000,,Enhance,Commercial,Open
            """),

        ("schedule-milestones", "Schedule activity & milestone tracker", "schedule", 1,
            "An activity list with dependencies, durations and progress; flag the activities on the critical path.",
            """
            Activity ID,Activity Name,Predecessors,Duration (days),Planned Start,Planned Finish,% Complete,On Critical Path (Y/N)
            A,Mobilise,,3,,,0,Y
            B,Detailed design,A,10,,,0,Y
            C,Procurement,A,8,,,0,N
            D,Construction,B;C,20,,,0,Y
            E,Commissioning,D,5,,,0,Y
            """),

        ("progress-measurement", "Weighted physical progress", "evm", 1,
            "Compute overall percent-complete as the budget-weighted average of the work packages.",
            """
            Work Package,Budget Weight,% Complete,Weighted Contribution = Weight x %,Notes
            Tooling,40000,100,,
            Assembly,30000,50,,
            Commissioning,30000,0,,
            Total,100000,,,Overall % = sum(weighted) / sum(weight)
            """),

        ("pert-estimate", "Three-point (PERT) estimate", "schedule", 1,
            "From optimistic / most-likely / pessimistic estimates, derive the expected duration and its standard deviation.",
            """
            Activity,Optimistic (O),Most Likely (M),Pessimistic (P),Expected = (O+4M+P)/6,Std Dev = (P-O)/6,Variance = StdDev^2
            A,2,4,6,,,
            B,3,5,13,,,
            C,1,2,3,,,
            """),

        ("cashflow-forecast", "Project cash-flow & funding forecast", "cashflow", 2,
            "Roll monthly inflows and outflows into a cumulative position; the deepest deficit is the peak funding requirement.",
            """
            Period,Inflow,Outflow,Net = Inflow-Outflow,Cumulative Position,Notes
            1,0,120000,,,Drawdown
            2,0,150000,,,
            3,100000,90000,,,
            4,250000,80000,,,
            5,300000,60000,,,Peak funding = deepest cumulative deficit
            """),

        ("investment-appraisal", "Investment appraisal (NPV / IRR / payback)", "finance", 2,
            "Discount each period's cash flow at the hurdle rate; NPV is the sum of present values, IRR the rate where NPV is zero.",
            """
            Period,Cash Flow,Discount Rate,Discount Factor = 1/(1+r)^t,Present Value = CF x DF,Cumulative PV
            0,-500000,0.10,1.000,,
            1,120000,0.10,,,
            2,160000,0.10,,,
            3,180000,0.10,,,
            4,200000,0.10,,,
            5,220000,0.10,,,NPV = sum(PV); IRR = rate where NPV = 0
            """),

        ("dscr-tracker", "Debt-service coverage (DSCR) tracker", "finance", 2,
            "Divide cash available for debt service by the debt service each period; lenders test DSCR against a minimum covenant.",
            """
            Period,CFADS (Cash Available for Debt Service),Debt Service (Principal+Interest),DSCR = CFADS/Debt Service,Covenant (min),Pass (Y/N)
            1,300000,250000,,1.20,
            2,340000,250000,,1.20,
            3,360000,250000,,1.20,
            """),

        ("raci-matrix", "RACI responsibility matrix", "delivery", 3,
            "For each deliverable, mark exactly one A (Accountable) and at least one R (Responsible) across the roles.",
            """
            Task / Deliverable,Sponsor,Project Manager,Team Lead,Subject Matter Expert,PMO
            Charter approval,A,R,C,I,I
            Schedule baseline,I,A,R,C,C
            Risk review,I,A,R,R,C
            Stage-gate sign-off,A,R,C,I,C
            """),

        ("change-register", "Change control register", "change", 3,
            "Log every change request; only APPROVED changes move the cost and schedule baseline.",
            """
            Change ID,Title,Type,Cost Impact,Schedule Impact (days),Status,Decision Date,Approved By
            C1,Additional test rig,Addition,30000,5,Approved,,Change Board
            C2,Gold-plating request,Addition,50000,10,Rejected,,Change Board
            C3,Descope legacy module,Reduction,-10000,-2,Approved,,Change Board
            C4,Client acceleration,Addition,20000,3,Pending,,
            """),

        ("stakeholder-register", "Stakeholder register & engagement plan", "delivery", 3,
            "Map each stakeholder's interest against their influence and set an engagement strategy and owner.",
            """
            Stakeholder,Role / Organisation,Interest (L/M/H),Influence (L/M/H),Current Engagement,Target Engagement,Strategy,Owner
            Client sponsor,Executive,H,H,Supportive,Leading,Manage closely,PM
            Regulator,Authority,M,H,Neutral,Supportive,Keep satisfied,Compliance
            End users,Operations,H,L,Unaware,Supportive,Keep informed,Change Lead
            """),

        ("status-report", "Project status report", "delivery", 3,
            "A one-page periodic status: overall RAG, schedule and cost position, top risks and the decisions needed.",
            """
            Section,Entry
            Reporting Period,
            Overall Status (R/A/G),
            Schedule Status (SPI / variance),
            Cost Status (CPI / variance),
            Key Achievements This Period,
            Planned for Next Period,
            Top 3 Risks / Issues,
            Decisions / Escalations Needed,
            """),

        ("lessons-learned", "Lessons learned log", "quality", 3,
            "Capture what worked and what didn't, with a recommendation to carry into the next project.",
            """
            ID,Category,What Happened,Impact,Root Cause,Recommendation,Owner
            L1,Procurement,Long-lead item ordered late,Two-week slip,Late design freeze,Freeze design before procurement,PM
            L2,Stakeholder,Regulator engaged early,Faster approval,Proactive plan,Repeat the early-engagement playbook,Compliance
            """),
    };
}
