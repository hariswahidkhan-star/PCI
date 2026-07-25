using PCI.Backend.Core;

namespace PCI.Backend.Data;

/// <summary>
/// Free Templates Library — the working project-controls artefacts behind the Simulation Lab.
///
/// The Lab teaches a technique on synthetic data; this library hands the practitioner the real spreadsheet
/// to run it on their own project. Each template mirrors a Lab engine (risk register ↔ risk, bill of
/// quantities ↔ boq, and so on), so practice and practice-at-work share a column vocabulary. Seventeen
/// templates span the eighteen engines: the EVM Tracker is period-by-period, so it serves both the
/// earned-value snapshot and the timeline-trend engine rather than shipping two near-identical files.
///
/// Format is CSV on purpose: it opens unchanged in Excel, Google Sheets, LibreOffice and Numbers, needs no
/// macro trust prompt, carries no vendor lock-in, and is small enough to keep in the repository. Formulas are
/// deliberately NOT embedded — every field is routed through <see cref="Csv.Field"/>, which neutralises the
/// spreadsheet-formula characters (SEC-2 / CWE-1236), and a template that shipped live formulas would be
/// indistinguishable from a CSV-injection payload. Each file therefore carries its inputs, worked example
/// rows, and a "how to compute" note naming the formula for the reader to enter.
///
/// Idempotent by <c>doc_group</c> (the PublicDocsSeed pattern): a template is created once and never
/// duplicated, and an operator who edits or withdraws one is never overwritten on the next boot.
/// </summary>
public static class TemplatesLibrarySeed
{
    const string Category = "templates";
    const string EffectiveDate = "25 July 2026";

    /// <summary>One template: a stable group id, catalogue metadata, the engine it mirrors, the column
    /// header, worked example rows, and the notes appended under the table.</summary>
    record Tpl(string Group, string Title, string Engine, string Description, string[] Header, string[][] Rows, string[] Notes);

    public static void Ensure(Db db)
    {
        foreach (var t in Items())
        {
            try
            {
                if (db.QueryOne("SELECT id FROM public_documents WHERE doc_group=?", t.Group) is not null) continue;
                var bytes = System.Text.Encoding.UTF8.GetBytes(Render(t));
                var obj = Storage.Put(bytes, "text/csv", "templates");
                var filename = t.Group + ".csv";
                db.Execute(@"INSERT INTO public_documents
                    (doc_group,title,description,category,language,version,status,legal_review_status,visibility,
                     owner,effective_date,published_at,storage_ref,filename,mime,size_bytes,sha256,is_current,created_at)
                    VALUES(?,?,?,?, 'en','1.0','published','approved_for_publication','public',
                     'PCI Simulation Lab',?,datetime('now'),?,?,?,?,?,1,datetime('now'))",
                    t.Group, t.Title, t.Description, Category, EffectiveDate,
                    obj.Reference, filename, obj.Mime, obj.SizeBytes, obj.Sha256);
            }
            catch { /* one bad template must never stop the rest, or block boot */ }
        }
    }

    /// <summary>Render a template to CSV. Every cell goes through <see cref="Csv.Field"/> so the file is
    /// RFC-4180 correct and inert when opened in a spreadsheet.</summary>
    static string Render(Tpl t)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append(string.Join(",", t.Header.Select(Csv.Field))).Append('\n');
        foreach (var row in t.Rows)
            sb.Append(string.Join(",", row.Select(Csv.Field))).Append('\n');
        // Blank rows so the practitioner can extend the table without touching the header.
        for (var i = 0; i < 5; i++)
            sb.Append(string.Join(",", t.Header.Select(_ => ""))).Append('\n');
        sb.Append('\n');
        foreach (var n in t.Notes)
            sb.Append(Csv.Field("NOTE: " + n)).Append('\n');
        sb.Append(Csv.Field($"PCI Project Controls Institute · {t.Title} · v1.0 · {EffectiveDate} · synthetic example rows, replace with your own data")).Append('\n');
        return sb.ToString();
    }

    static Tpl[] Items() => new[]
    {
        new Tpl("template-evm-tracker", "Earned Value Tracker (CSV)", "evm",
            "Period-by-period earned value: planned value, earned value and actual cost with the variances, indices and the CPI-method forecast. Mirrors the Lab's earned-value engine.",
            new[] { "period", "pv_cumulative", "ev_cumulative", "ac_cumulative", "bac", "sv", "cv", "spi", "cpi", "eac", "vac" },
            new[] {
                new[] { "1", "100000", "90000", "100000", "600000", "-10000", "-10000", "0.90", "0.90", "666667", "-66667" },
                new[] { "2", "220000", "200000", "230000", "600000", "-20000", "-30000", "0.91", "0.87", "690000", "-90000" },
            },
            new[] {
                "SV = EV - PV. CV = EV - AC. Positive is favourable.",
                "SPI = EV / PV. CPI = EV / AC. Below 1.00 means behind schedule / over cost.",
                "EAC (CPI method) = BAC / CPI. VAC = BAC - EAC.",
                "Enter PV, EV, AC and BAC; compute the remaining columns in your spreadsheet.",
            }),

        new Tpl("template-risk-register", "Quantified Risk Register (CSV)", "risk",
            "A quantified register with probability, signed impact and expected monetary value. Threats are negative, opportunities positive, and they net off — the same convention the Lab's risk engine grades.",
            new[] { "risk_id", "description", "category", "owner", "probability", "impact", "emv", "response_strategy", "status" },
            new[] {
                new[] { "R1", "Long-lead switchgear slips", "schedule", "Procurement lead", "0.3", "-20000", "-6000", "mitigate", "open" },
                new[] { "R2", "Scope growth in fit-out", "cost", "Project manager", "0.5", "-10000", "-5000", "mitigate", "open" },
                new[] { "R3", "Early handover bonus", "opportunity", "Commercial manager", "0.2", "15000", "3000", "exploit", "open" },
            },
            new[] {
                "EMV = probability x impact. Probability is a decimal between 0 and 1.",
                "Impact sign carries the meaning: negative = threat, positive = opportunity.",
                "Register EMV is the sum of the emv column; threats and opportunities net off.",
                "Response strategy: avoid / mitigate / transfer / accept for threats; exploit / enhance / share / accept for opportunities.",
            }),

        new Tpl("template-wbs-dictionary", "WBS Dictionary and Roll-up (CSV)", "wbs",
            "A work breakdown structure with parent links and leaf budgets, so the roll-up and the 100% rule can be checked. Mirrors the Lab's WBS engine.",
            new[] { "wbs_id", "parent_id", "element_name", "deliverable", "responsible", "budget", "is_leaf" },
            new[] {
                new[] { "1", "", "Office fit-out", "Completed fit-out", "Project manager", "", "no" },
                new[] { "1.1", "1", "Design", "Approved design pack", "Design lead", "40000", "yes" },
                new[] { "1.2", "1", "Build", "Constructed works", "Construction lead", "", "no" },
                new[] { "1.2.1", "1.2", "Structure", "Structural frame", "Structures engineer", "30000", "yes" },
                new[] { "1.2.2", "1.2", "Services", "MEP services", "Services engineer", "20000", "yes" },
                new[] { "1.3", "1", "Handover", "Handover pack", "Commissioning lead", "10000", "yes" },
            },
            new[] {
                "Leave parent_id blank for the single root element.",
                "Only leaf elements carry a budget; a parent's budget is the sum of its children.",
                "100% rule: every parent must equal the sum of its children, and no scope may sit outside the structure.",
            }),

        new Tpl("template-cost-breakdown", "Cost Breakdown Structure — Budget vs Actual (CSV)", "cbs",
            "Cost accounts with budget, commitment and actual, rolling to a variance at every level. Mirrors the Lab's cost-control engine.",
            new[] { "account_id", "parent_id", "account_name", "budget", "committed", "actual", "variance" },
            new[] {
                new[] { "1", "", "Refurbishment", "100000", "96000", "102000", "-2000" },
                new[] { "1.1", "1", "Demolition", "50000", "52000", "55000", "-5000" },
                new[] { "1.2", "1", "Structure", "30000", "24000", "25000", "5000" },
                new[] { "1.3", "1", "Finishes", "20000", "20000", "22000", "-2000" },
            },
            new[] {
                "Variance = budget - actual. Negative means an overspend.",
                "Committed captures purchase orders raised but not yet invoiced — watch it as an early warning.",
                "Roll leaf accounts up to each parent; the root is the project total.",
            }),

        new Tpl("template-change-log", "Change Control Log (CSV)", "change",
            "A change register that separates approved changes from pending and rejected ones, so only approved change moves the baseline — the discipline the Lab's change engine grades.",
            new[] { "change_id", "title", "raised_by", "date_raised", "status", "cost_delta", "schedule_delta_days", "decision_date", "decision_by" },
            new[] {
                new[] { "C1", "Additional test rig", "Commissioning lead", "2026-03-02", "approved", "30000", "5", "2026-03-11", "Change board" },
                new[] { "C2", "Enhanced cladding finish", "Client", "2026-03-14", "rejected", "50000", "10", "2026-03-21", "Change board" },
                new[] { "C3", "Descope legacy interface", "Project manager", "2026-04-01", "approved", "-10000", "-2", "2026-04-08", "Change board" },
                new[] { "C4", "Accelerate handover", "Client", "2026-04-19", "pending", "20000", "3", "", "" },
            },
            new[] {
                "Status must be one of approved / rejected / pending.",
                "Revised baseline = original baseline + the sum of APPROVED deltas only.",
                "Pending and rejected changes never move the baseline, however likely approval looks.",
                "Deltas are signed: a descope is a negative cost and may pull the schedule in.",
            }),

        new Tpl("template-cash-flow", "Project Cash Flow and Peak Funding (CSV)", "cashflow",
            "Time-phased inflows and outflows rolled to a cumulative position, exposing the peak funding requirement. Mirrors the Lab's cash-flow engine.",
            new[] { "period", "period_label", "inflow", "outflow", "net", "cumulative" },
            new[] {
                new[] { "1", "Month 1", "0", "50000", "-50000", "-50000" },
                new[] { "2", "Month 2", "20000", "60000", "-40000", "-90000" },
                new[] { "3", "Month 3", "80000", "40000", "40000", "-50000" },
                new[] { "4", "Month 4", "120000", "30000", "90000", "40000" },
            },
            new[] {
                "Net = inflow - outflow. Cumulative is the running total of net.",
                "Peak funding requirement = the deepest (most negative) cumulative position, quoted as a positive amount.",
                "The peak, not the closing position, is the cash the project must be able to finance.",
            }),

        new Tpl("template-resource-histogram", "Resource Loading and Levelling (CSV)", "resource",
            "Period demand against available capacity, exposing peak demand, over-demand and the periods that need levelling. Mirrors the Lab's resource engine.",
            new[] { "period", "resource", "demand", "capacity", "overload", "levelling_action" },
            new[] {
                new[] { "1", "Steel erectors", "8", "12", "0", "" },
                new[] { "2", "Steel erectors", "14", "12", "2", "shift non-critical work right" },
                new[] { "3", "Steel erectors", "19", "12", "7", "add a second crew or re-sequence" },
                new[] { "4", "Steel erectors", "11", "12", "0", "" },
            },
            new[] {
                "Overload = max(0, demand - capacity) for each period.",
                "Total over-demand is the sum of the overload column; peak overload is its largest single value.",
                "Level by moving float-bearing work before adding capacity — it is usually cheaper.",
            }),

        new Tpl("template-bill-of-quantities", "Bill of Quantities (CSV)", "boq",
            "A priced bill with quantity, rate and line amount. Mirrors the Lab's quantity-surveying engine.",
            new[] { "item_ref", "description", "unit", "quantity", "rate", "amount" },
            new[] {
                new[] { "B1", "Excavation to reduced level", "m3", "1200", "18", "21600" },
                new[] { "B2", "Mass concrete fill", "m3", "340", "95", "32300" },
                new[] { "B3", "Reinforcement, high yield", "t", "26", "1450", "37700" },
                new[] { "B4", "Formwork to sides", "m2", "880", "42", "36960" },
            },
            new[] {
                "Amount = quantity x rate. The priced total is the sum of the amount column.",
                "The average of the rate column is NOT the effective rate — a high-rate, low-quantity item distorts it. Divide the total by the total quantity only when the units are the same.",
                "State the unit for every line; mixed units cannot be summed.",
            }),

        new Tpl("template-progress-measurement", "Weighted Progress Measurement (CSV)", "progress",
            "Work packages with budget weights and their own percent complete, rolled to an overall physical progress. Mirrors the Lab's progress engine.",
            new[] { "package_id", "package_name", "weight_budget", "percent_complete", "weighted_progress", "measurement_method" },
            new[] {
                new[] { "1.1", "Tooling", "40000", "100", "40000", "milestone" },
                new[] { "1.2", "Assembly", "30000", "50", "15000", "units completed" },
                new[] { "1.3", "Commissioning", "30000", "0", "0", "milestone" },
            },
            new[] {
                "Weighted progress = weight x percent complete. Overall percent = sum(weighted) / sum(weight).",
                "Weight by budget or labour hours — never by package count, which flatters small packages.",
                "Record the measurement method per package so progress claims stay auditable.",
            }),

        new Tpl("template-productivity-log", "Labour Productivity Log (CSV)", "productivity",
            "Planned versus achieved unit rates and the resulting productivity factor. Mirrors the Lab's productivity engine.",
            new[] { "week", "activity", "unit", "planned_qty", "planned_hours", "earned_qty", "actual_hours", "planned_rate", "actual_rate", "factor" },
            new[] {
                new[] { "1", "Concrete placement", "m3", "480", "600", "420", "700", "0.800", "0.600", "0.750" },
                new[] { "2", "Concrete placement", "m3", "480", "600", "500", "620", "0.800", "0.806", "1.008" },
            },
            new[] {
                "Planned rate = planned_qty / planned_hours. Actual rate = earned_qty / actual_hours.",
                "Productivity factor = actual rate / planned rate. Below 1.00 means the crew needs more hours per unit than estimated.",
                "Use EARNED quantity (surveyed and accepted), never claimed quantity, or the factor flatters itself.",
                "Rework hours belong in actual_hours — they are real cost that produced no new quantity.",
            }),

        new Tpl("template-schedule-network", "Schedule Network and Float (CSV)", "cpm",
            "An activity-on-node network with durations and predecessors, ready for a forward and backward pass. Mirrors the Lab's critical-path engine.",
            new[] { "activity_id", "activity_name", "duration_days", "predecessors", "early_start", "early_finish", "late_start", "late_finish", "total_float", "critical" },
            new[] {
                new[] { "A", "Site setup", "3", "", "0", "3", "0", "3", "0", "yes" },
                new[] { "B", "Foundations", "4", "A", "3", "7", "3", "7", "0", "yes" },
                new[] { "C", "Temporary services", "2", "A", "3", "5", "5", "7", "2", "no" },
                new[] { "D", "Frame erection", "5", "B;C", "7", "12", "7", "12", "0", "yes" },
                new[] { "E", "Handover", "1", "D", "12", "13", "12", "13", "0", "yes" },
            },
            new[] {
                "Separate multiple predecessors with a semicolon, so the field survives CSV without quoting.",
                "Forward pass: early start = max(early finish of predecessors); early finish = early start + duration.",
                "Backward pass: late finish = min(late start of successors); late start = late finish - duration.",
                "Total float = late start - early start. Zero float means the activity is critical.",
            }),

        new Tpl("template-three-point-estimate", "Three-Point (PERT) Estimate (CSV)", "pert",
            "Optimistic, most-likely and pessimistic estimates with the PERT expected duration and variance. Mirrors the Lab's PERT engine.",
            new[] { "activity_id", "activity_name", "optimistic", "most_likely", "pessimistic", "expected", "std_dev", "variance" },
            new[] {
                new[] { "A", "Detailed design", "2", "4", "6", "4.00", "0.667", "0.444" },
                new[] { "B", "Long-lead procurement", "3", "5", "13", "6.00", "1.667", "2.778" },
                new[] { "C", "Installation", "1", "2", "3", "2.00", "0.333", "0.111" },
            },
            new[] {
                "Expected = (optimistic + 4 x most likely + pessimistic) / 6.",
                "Standard deviation = (pessimistic - optimistic) / 6. Variance is its square.",
                "For activities in series, add the expected durations and add the VARIANCES — never add standard deviations.",
                "Path standard deviation = square root of the summed variance.",
            }),

        new Tpl("template-earned-schedule", "Earned Schedule Tracker (CSV)", "earned_schedule",
            "The planned-value curve with earned schedule read in time units, so schedule performance stays meaningful late in a project. Mirrors the Lab's earned-schedule engine.",
            new[] { "period", "pv_cumulative", "ev_cumulative", "earned_schedule", "actual_time", "sv_time", "spi_time" },
            new[] {
                new[] { "1", "100", "90", "0.90", "1", "-0.10", "0.90" },
                new[] { "2", "250", "210", "1.73", "2", "-0.27", "0.87" },
                new[] { "3", "450", "360", "2.55", "3", "-0.45", "0.85" },
                new[] { "4", "650", "500", "3.25", "4", "-0.75", "0.81" },
            },
            new[] {
                "Earned schedule = the time at which the PLAN intended to have earned today's EV, interpolated between periods.",
                "SV(t) = ES - AT. SPI(t) = ES / AT. Both are in time units, not currency.",
                "Classic SPI collapses to 1.00 as a late project finishes; SPI(t) does not, which is why it is worth tracking.",
                "Independent time forecast IEAC(t) = planned duration / SPI(t).",
            }),

        new Tpl("template-procurement-log", "Procurement and Long-Lead Log (CSV)", "procurement",
            "Long-lead packages with required-on-site dates and float, showing when a supplier slip reaches the critical path. Mirrors the Lab's procurement engine.",
            new[] { "package_id", "description", "supplier", "po_date", "promised_delivery", "required_on_site", "delay_days", "remaining_float", "critical_delay" },
            new[] {
                new[] { "PR-01", "HV switchgear", "Supplier A", "2026-02-10", "2026-08-14", "2026-08-02", "12", "5", "7" },
                new[] { "PR-02", "Chillers", "Supplier B", "2026-03-04", "2026-07-20", "2026-08-01", "0", "9", "0" },
            },
            new[] {
                "Delay days = promised delivery - required on site, counted only when positive.",
                "Critical delay = max(0, delay days - remaining float). This is what actually moves the completion date.",
                "Float consumed = min(remaining float, delay days). Float spent here is no longer available to anyone else.",
                "Track remaining float per package: two packages can each be 'within float' and still sink the same milestone.",
            }),

        new Tpl("template-decision-matrix", "Weighted Decision Matrix (CSV)", "decision",
            "Options scored against weighted criteria to produce a defensible recommendation. Mirrors the Lab's decision-analysis engine.",
            new[] { "option_id", "option_name", "cost_score", "schedule_score", "risk_score", "weight_cost", "weight_schedule", "weight_risk", "weighted_total" },
            new[] {
                new[] { "O1", "In-house build", "12", "18", "7", "1", "1", "1", "37" },
                new[] { "O2", "Managed service", "15", "11", "4", "1", "1", "1", "30" },
                new[] { "O3", "Hybrid delivery", "13", "14", "6", "1", "1", "1", "33" },
            },
            new[] {
                "Weighted total = sum of (score x weight) across the criteria.",
                "These are PENALTY scores, so the LOWEST weighted total wins. Invert any benefit criterion before scoring.",
                "Normalise units before weighting (cost in millions, schedule in months, risk 0-10) or the largest-numbered criterion silently dominates.",
                "Record who set the weights and when — the weighting is the judgement, and it must be auditable.",
            }),

        new Tpl("template-portfolio-scorecard", "Portfolio Selection Scorecard (CSV)", "portfolio",
            "Candidate investments scored on value, risk and strategic fit for a ranked portfolio. Mirrors the Lab's portfolio engine.",
            new[] { "project_id", "project_name", "npv", "risk_0_1", "strategic_fit_0_1", "weight_npv", "weight_risk", "weight_fit", "score" },
            new[] {
                new[] { "P1", "Depot automation", "4200", "0.4", "0.8", "0.5", "0.3", "0.2", "0.469" },
                new[] { "P2", "Fleet renewal", "6800", "0.7", "0.5", "0.5", "0.3", "0.2", "0.390" },
                new[] { "P3", "Control-room upgrade", "2500", "0.2", "0.9", "0.5", "0.3", "0.2", "0.504" },
                new[] { "P4", "Legacy migration", "-800", "0.6", "0.3", "0.5", "0.3", "0.2", "-0.199" },
            },
            new[] {
                "Normalise NPV against the largest absolute NPV in the list before weighting, so one big project cannot swamp the score.",
                "Score = weight_npv x normalised NPV + weight_fit x fit - weight_risk x risk. Risk SUBTRACTS.",
                "Risk and strategic fit are decimals between 0 and 1.",
                "Rank on score, but sanity-check the top of the list against affordability and delivery capacity before committing.",
            }),

        new Tpl("template-data-quality-check", "Progress Data Quality Check (CSV)", "data_quality",
            "A reconciliation sheet for reported versus expected values, exposing missing readings and anomalies before they reach a report. Mirrors the Lab's data-quality engine.",
            new[] { "record_id", "source", "reported_value", "expected_value", "absolute_error", "within_tolerance", "note" },
            new[] {
                new[] { "WP-01", "Site return", "102", "100", "2", "yes", "" },
                new[] { "WP-02", "Site return", "88", "100", "12", "no", "investigate: under-reported" },
                new[] { "WP-03", "Site return", "", "100", "", "", "MISSING reading" },
                new[] { "WP-04", "Supplier feed", "99", "100", "1", "yes", "" },
                new[] { "WP-05", "Supplier feed", "140", "120", "20", "no", "investigate: over-reported" },
            },
            new[] {
                "Absolute error = |reported - expected|. Within tolerance compares it against your agreed threshold.",
                "Completeness = records with a reported value / total records, as a percentage.",
                "Compute the mean absolute error over REPORTED rows only — averaging blanks as zero hides the gap and flatters the data.",
                "A missing reading is a finding, not a blank cell: chase it before the reporting cut-off.",
            }),
    };
}
