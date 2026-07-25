namespace PCI.Backend.Core;

/// <summary>
/// Simulation Lab — scenario authoring templates (Admin Studio).
///
/// A blank <c>config_json</c> textarea is the hardest part of authoring a scenario: every engine reads a
/// different <c>given</c> shape and grades a different set of <c>ask</c> measures, and getting either wrong
/// produces a draft the §14 publication gate rejects. This class ships ONE known-good starter per engine
/// task, so an author begins from a working, publishable skeleton and edits the story and the numbers
/// instead of reverse-engineering the contract.
///
/// Every template is a real scenario the engine can grade: SimTemplatesTests re-runs each one through
/// <see cref="SimContent.Validate"/>, so a template can never drift from its engine, and asserts that every
/// member of <see cref="SimCalc.KnownTasks"/> has one — a newly added engine fails the suite until it is
/// given a starter here. Templates are deliberately foundation-difficulty with round numbers: they are a
/// starting point to edit, not content to publish as-is.
/// </summary>
public static class SimTemplates
{
    /// <summary>One authoring starter. <c>Task</c> is the engine selector, <c>Label</c> and <c>Hint</c> are
    /// operator-facing, and <c>Config</c> is a complete, gradable <c>config_json</c> document.</summary>
    public sealed record Template(string Task, string Label, string Hint, string Config);

    public static IReadOnlyList<Template> All => Items;

    /// <summary>The starter for one engine task, or null when the task is unknown.</summary>
    public static Template? For(string task) =>
        Items.FirstOrDefault(t => string.Equals(t.Task, task, StringComparison.Ordinal));

    static readonly Template[] Items =
    {
        new("evm", "Earned value — core measures",
            "PV/EV/AC against a BAC. The classic snapshot: variances, indices and the CPI forecast.",
            """
            {"task":"evm",
             "prompt":"At the end of month 3 the package reports Planned Value 100000, Earned Value 90000 and Actual Cost 95000 against a Budget at Completion of 200000. Compute the core earned-value measures, giving indices to 2 decimal places.",
             "given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
             "ask":[
               {"key":"sv","label":"Schedule Variance (SV)","type":"number"},
               {"key":"cv","label":"Cost Variance (CV)","type":"number"},
               {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
               {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"},
               {"key":"eac","label":"Estimate at Completion (EAC, CPI method)","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
            """),

        new("cpm", "Critical path — forward and backward pass",
            "Activity-on-node network. Ask for the duration, the critical path (type \"set\") and float on a non-critical activity.",
            """
            {"task":"cpm",
             "prompt":"Run the forward and backward pass on the activity network below (durations in days). Give the project duration, the critical path, and the total float of activity C.",
             "given":{"activities":[
               {"id":"A","dur":3,"preds":[]},
               {"id":"B","dur":4,"preds":["A"]},
               {"id":"C","dur":2,"preds":["A"]},
               {"id":"D","dur":5,"preds":["B","C"]},
               {"id":"E","dur":1,"preds":["D"]}]},
             "ask":[
               {"key":"project_duration","label":"Project duration (days)","type":"number"},
               {"key":"critical_path","label":"Critical path (activity IDs)","type":"set"},
               {"key":"float_C","label":"Total float of activity C (days)","type":"number"}],
             "tolerance":0.001,"pass_pct":70,"competencies":["schedule_analysis"]}
            """),

        new("wbs", "Work breakdown — roll-up and the 100% rule",
            "Leaf values roll to the root. hundred_percent_valid is a \"bool\" ask.",
            """
            {"task":"wbs",
             "prompt":"Roll the work breakdown below up from its leaf budgets to the root, then state whether the structure satisfies the 100% rule.",
             "given":{"nodes":[
               {"id":"1","parent":null,"name":"Office fit-out"},
               {"id":"1.1","parent":"1","name":"Design","value":40000},
               {"id":"1.2","parent":"1","name":"Build"},
               {"id":"1.2.1","parent":"1.2","name":"Structure","value":30000},
               {"id":"1.2.2","parent":"1.2","name":"Services","value":20000},
               {"id":"1.3","parent":"1","name":"Handover","value":10000}]},
             "ask":[
               {"key":"root_total","label":"Total project budget (root roll-up)","type":"number"},
               {"key":"hundred_percent_valid","label":"Does the WBS satisfy the 100% rule?","type":"bool"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["scope_structuring"]}
            """),

        new("cbs", "Cost breakdown — budget vs actual",
            "Leaf accounts carry budget and actual; variance is budget − actual at any node.",
            """
            {"task":"cbs",
             "prompt":"Roll the cost accounts below up to the root and report the total budget, the total actual, and the variance (budget minus actual) at the root.",
             "given":{"nodes":[
               {"id":"1","parent":null,"name":"Refurbishment"},
               {"id":"1.1","parent":"1","name":"Demolition","budget":50000,"actual":55000},
               {"id":"1.2","parent":"1","name":"Structure","budget":30000,"actual":25000},
               {"id":"1.3","parent":"1","name":"Finishes","budget":20000,"actual":22000}]},
             "ask":[
               {"key":"root_budget","label":"Total budget (root roll-up)","type":"number"},
               {"key":"root_actual","label":"Total actual (root roll-up)","type":"number"},
               {"key":"root_variance","label":"Variance at the root (budget - actual)","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["cost_control"]}
            """),

        new("progress", "Physical progress — weighted percent complete",
            "Budget-weighted average of each work package's own percent complete.",
            """
            {"task":"progress",
             "prompt":"Each work package below carries a budget weight and its own percent complete. Compute the project's overall physical progress as the budget-weighted average.",
             "given":{"nodes":[
               {"id":"1.1","name":"Tooling","weight":40000,"percent":100},
               {"id":"1.2","name":"Assembly","weight":30000,"percent":50},
               {"id":"1.3","name":"Commissioning","weight":30000,"percent":0}]},
             "ask":[
               {"key":"overall_percent","label":"Overall percent complete (budget-weighted)","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["progress_measurement"]}
            """),

        new("risk", "Risk register — expected monetary value",
            "Signed impacts: negative is a threat, positive an opportunity. They net off.",
            """
            {"task":"risk",
             "prompt":"The quantified risk register below lists three risks. Negative impacts are threats and positive impacts are opportunities. Compute the register's Expected Monetary Value.",
             "given":{"risks":[
               {"id":"R1","name":"Supplier delay","probability":0.3,"impact":-20000},
               {"id":"R2","name":"Scope creep","probability":0.5,"impact":-10000},
               {"id":"R3","name":"Early-handover bonus","probability":0.2,"impact":15000}]},
             "ask":[{"key":"emv","label":"Expected Monetary Value of the register","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["risk_management"]}
            """),

        new("pert", "Three-point estimate — PERT and confidence",
            "o/m/p per activity plus a deadline. Pick the deadline so prob_on_time is neither 0 nor 100.",
            """
            {"task":"pert",
             "prompt":"Three activities on the critical path carry optimistic, most-likely and pessimistic estimates in days. Compute the path's PERT expected duration, its standard deviation, and the probability of finishing by day 14.",
             "given":{"activities":[
               {"id":"A","o":2,"m":4,"p":6},
               {"id":"B","o":3,"m":5,"p":13},
               {"id":"C","o":1,"m":2,"p":3}],"deadline":14},
             "ask":[
               {"key":"expected_duration","label":"PERT expected duration (days)","type":"number"},
               {"key":"std_dev","label":"Path standard deviation (days)","type":"number"},
               {"key":"prob_on_time","label":"Probability of finishing by day 14 (%)","type":"number"}],
             "tolerance":0.02,"pass_pct":70,"competencies":["schedule_analysis","risk_management"]}
            """),

        new("change", "Change control — approved changes only",
            "Only 'approved' rows move the baseline. Include a tempting pending or rejected one.",
            """
            {"task":"change",
             "prompt":"The baseline is 500000 over 100 days. Roll ONLY the approved changes in the register below onto that baseline: give the revised budget, the revised duration, and how many changes were approved.",
             "given":{"baseline_bac":500000,"baseline_duration":100,"changes":[
               {"id":"C1","title":"Extra test rig","status":"approved","cost_delta":30000,"schedule_delta":5},
               {"id":"C2","title":"Gold-plating request","status":"rejected","cost_delta":50000,"schedule_delta":10},
               {"id":"C3","title":"Descope legacy module","status":"approved","cost_delta":-10000,"schedule_delta":-2},
               {"id":"C4","title":"Client-requested finish","status":"pending","cost_delta":20000,"schedule_delta":3}]},
             "ask":[
               {"key":"revised_bac","label":"Revised BAC","type":"number"},
               {"key":"revised_duration","label":"Revised duration (days)","type":"number"},
               {"key":"approved_count","label":"Number of changes approved","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["change_control"]}
            """),

        new("cashflow", "Cash flow — cumulative position and peak funding",
            "Run the early periods cash-negative so peak_funding is a meaningful number.",
            """
            {"task":"cashflow",
             "prompt":"Roll the monthly cash flow below into its cumulative net position. Give the closing position and the peak funding requirement (the deepest cumulative deficit, as a positive amount).",
             "given":{"periods":[
               {"period":1,"inflow":0,"outflow":50000},
               {"period":2,"inflow":20000,"outflow":60000},
               {"period":3,"inflow":80000,"outflow":40000},
               {"period":4,"inflow":120000,"outflow":30000}]},
             "ask":[
               {"key":"final_position","label":"Final (closing) cash position","type":"number"},
               {"key":"peak_funding","label":"Peak funding requirement","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["cash_flow"]}
            """),

        new("timeline", "EVM trend — read the trajectory period by period",
            "Cumulative PV/EV/AC per period. Engineer a single clearly-worst SPI month.",
            """
            {"task":"timeline",
             "prompt":"The package reports cumulative Planned Value, Earned Value and Actual Cost each month against a 600000 budget. Identify the month of worst schedule performance, then give the cumulative CPI at the finish, the CPI-method forecast (EAC = BAC / CPI) and the resulting Variance at Completion.",
             "given":{"bac":600000,
               "series":[
                 {"period":1,"pv":100000,"ev":90000,"ac":100000},
                 {"period":2,"pv":220000,"ev":200000,"ac":230000},
                 {"period":3,"pv":350000,"ev":300000,"ac":360000},
                 {"period":4,"pv":470000,"ev":420000,"ac":500000},
                 {"period":5,"pv":560000,"ev":520000,"ac":610000},
                 {"period":6,"pv":600000,"ev":580000,"ac":680000}]},
             "ask":[
               {"key":"worst_spi_period","label":"Month of worst schedule performance","type":"number"},
               {"key":"final_cpi","label":"Cumulative CPI at completion","type":"number"},
               {"key":"final_eac","label":"Estimate at Completion (BAC / CPI)","type":"number"},
               {"key":"vac","label":"Variance at Completion (BAC - EAC)","type":"number"}],
             "tolerance":0.01,"pass_pct":75,"competencies":["earned_value","forecasting"]}
            """),

        new("earned_schedule", "Earned schedule — schedule performance in time",
            "Choose ev so ES lands BETWEEN two plan periods; interpolation is the lesson.",
            """
            {"task":"earned_schedule",
             "prompt":"The six-month plan below gives cumulative Planned Value against a 1000 budget. At the end of month 4 the team has earned 500. Read Earned Schedule off the plan, then give the schedule variance in time, the time-based index, and the independent time forecast.",
             "given":{"planned_duration":6,"at":4,"ev":500,
               "plan":[
                 {"period":1,"pv":100},
                 {"period":2,"pv":250},
                 {"period":3,"pv":450},
                 {"period":4,"pv":650},
                 {"period":5,"pv":830},
                 {"period":6,"pv":1000}]},
             "ask":[
               {"key":"es","label":"Earned Schedule (months)","type":"number"},
               {"key":"sv_time","label":"Schedule variance in time (ES - AT)","type":"number"},
               {"key":"spi_time","label":"Time-based schedule index (ES / AT)","type":"number"},
               {"key":"eac_time","label":"Independent time forecast (PD / SPI(t))","type":"number"}],
             "tolerance":0.01,"pass_pct":75,"competencies":["schedule_analysis","forecasting"]}
            """),

        new("productivity", "Labour productivity — unit rate and factor",
            "Quantity per labour hour, planned vs actual. Keep the factor within a believable band.",
            """
            {"task":"productivity",
             "prompt":"The estimate allowed 480 cubic metres of concrete for 600 direct labour hours. The crew has placed 420 surveyed cubic metres for 700 charged hours. Compute the planned and actual unit rates and the productivity factor, to three decimal places.",
             "given":{"planned_qty":480,"planned_hours":600,"earned_qty":420,"actual_hours":700},
             "ask":[
               {"key":"planned_productivity","label":"Planned productivity (units per hour)","type":"number"},
               {"key":"actual_productivity","label":"Actual productivity (units per hour)","type":"number"},
               {"key":"factor","label":"Productivity factor (actual / planned)","type":"number"},
               {"key":"hours_variance","label":"Hours variance (actual - planned)","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["productivity_analysis"]}
            """),

        new("boq", "Bill of quantities — priced total",
            "Note average_rate is the PLAIN mean of the rates, not quantity-weighted — a useful teaching trap.",
            """
            {"task":"boq",
             "prompt":"Price the bill of quantities below. Give the priced total, the number of lines, and the average rate across the lines.",
             "given":{"lines":[
               {"id":"B1","description":"Excavation to reduced level","qty":1200,"rate":18},
               {"id":"B2","description":"Mass concrete fill","qty":340,"rate":95},
               {"id":"B3","description":"Reinforcement, high yield","qty":26,"rate":1450},
               {"id":"B4","description":"Formwork to sides","qty":880,"rate":42}]},
             "ask":[
               {"key":"total","label":"Priced total","type":"number"},
               {"key":"line_count","label":"Number of bill lines","type":"number"},
               {"key":"average_rate","label":"Average rate across the lines","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["quantity_surveying"]}
            """),

        new("resource", "Resource loading — peak demand and overload",
            "Demand MUST exceed capacity in at least one period or the overload measures are all zero.",
            """
            {"task":"resource",
             "prompt":"The histogram below shows crew demand against available capacity for each week. Give the peak demand, the largest single-week shortfall, and the number of weeks that are overloaded.",
             "given":{"periods":[
               {"period":1,"demand":8,"capacity":12},
               {"period":2,"demand":14,"capacity":12},
               {"period":3,"demand":19,"capacity":12},
               {"period":4,"demand":11,"capacity":12},
               {"period":5,"demand":15,"capacity":12}]},
             "ask":[
               {"key":"peak_demand","label":"Peak demand (crew)","type":"number"},
               {"key":"peak_overload","label":"Largest single-period shortfall","type":"number"},
               {"key":"total_overdemand","label":"Total over-demand across the plan","type":"number"},
               {"key":"overloaded_periods","label":"Number of overloaded periods","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["resource_management"]}
            """),

        new("procurement", "Procurement delay — float absorption",
            "Make the supplier delay exceed the remaining float, or critical_delay is zero and nothing is taught.",
            """
            {"task":"procurement",
             "prompt":"A long-lead package is 12 days late against a delivery activity holding 5 days of remaining float, on a project baselined at 180 days. Give the delay that reaches the critical path, the revised project duration, and how much float the delay consumes.",
             "given":{"project_duration":180,"remaining_float":5,"supplier_delay_days":12},
             "ask":[
               {"key":"critical_delay","label":"Delay reaching the critical path (days)","type":"number"},
               {"key":"new_project_duration","label":"Revised project duration (days)","type":"number"},
               {"key":"float_consumed","label":"Float consumed (days)","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["procurement_management"]}
            """),

        new("portfolio", "Portfolio selection — weighted scoring",
            "Ask top_score or score_<id>. NEVER ask top_id: it returns a bare string the grader cannot score.",
            """
            {"task":"portfolio",
             "prompt":"Four candidate projects are scored on net present value, delivery risk and strategic fit, weighted 0.5 / 0.3 / 0.2. Give the total NPV of the portfolio and the score of the highest-ranked candidate, to three decimal places.",
             "given":{"w_npv":0.5,"w_risk":0.3,"w_fit":0.2,
               "projects":[
                 {"id":"P1","name":"Depot automation","npv":4200,"risk":0.4,"fit":0.8},
                 {"id":"P2","name":"Fleet renewal","npv":6800,"risk":0.7,"fit":0.5},
                 {"id":"P3","name":"Control-room upgrade","npv":2500,"risk":0.2,"fit":0.9},
                 {"id":"P4","name":"Legacy migration","npv":-800,"risk":0.6,"fit":0.3}]},
             "ask":[
               {"key":"total_npv","label":"Total portfolio NPV","type":"number"},
               {"key":"top_score","label":"Score of the highest-ranked project","type":"number"},
               {"key":"score_P3","label":"Weighted score of P3","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["portfolio_selection","financial_analysis"]}
            """),

        new("decision", "Decision analysis — weighted penalty score",
            "LOWEST score wins (it is a penalty). Ask best_score or score_<id>, never best_option.",
            """
            {"task":"decision",
             "prompt":"Three delivery options are scored on cost (millions), schedule (months) and risk (0-10), each weighted equally. The lowest weighted penalty score wins. Give the winning score and the score of option O2, to two decimal places.",
             "given":{"w_cost":1,"w_sched":1,"w_risk":1,
               "options":[
                 {"id":"O1","name":"In-house build","cost":12,"schedule":18,"risk":7},
                 {"id":"O2","name":"Managed service","cost":15,"schedule":11,"risk":4},
                 {"id":"O3","name":"Hybrid delivery","cost":13,"schedule":14,"risk":6}]},
             "ask":[
               {"key":"best_score","label":"Winning (lowest) weighted score","type":"number"},
               {"key":"score_O2","label":"Weighted score of option O2","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["decision_analysis"]}
            """),

        new("data_quality", "Data quality — completeness and anomalies",
            "OMIT the \"value\" key on at least one row (that is a missing reading) and breach the threshold on at least one.",
            """
            {"task":"data_quality",
             "prompt":"The progress feed below reports a measured value against the expected value for each work package, with a tolerance of 5. Some readings failed to arrive. Give the record count, the completeness percentage, the mean absolute error across the readings that did arrive, and how many of those breach the tolerance.",
             "given":{"threshold":5,
               "rows":[
                 {"id":"WP-01","value":102,"expected":100},
                 {"id":"WP-02","value":88,"expected":100},
                 {"id":"WP-03","expected":100},
                 {"id":"WP-04","value":99,"expected":100},
                 {"id":"WP-05","value":140,"expected":120},
                 {"id":"WP-06","expected":120},
                 {"id":"WP-07","value":118,"expected":120},
                 {"id":"WP-08","value":121,"expected":120}]},
             "ask":[
               {"key":"record_count","label":"Total records in the feed","type":"number"},
               {"key":"completeness_pct","label":"Completeness (%)","type":"number"},
               {"key":"mean_abs_error","label":"Mean absolute error (reported rows)","type":"number"},
               {"key":"anomaly_count","label":"Readings breaching the tolerance","type":"number"}],
             "tolerance":0.01,"pass_pct":70,"competencies":["data_quality"]}
            """),
    };
}
