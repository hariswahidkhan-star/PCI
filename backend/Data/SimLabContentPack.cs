using System.Globalization;
using System.Text;
using System.Text.Json;

namespace PCI.Backend.Data;

/// <summary>
/// House-authored PCI Simulation Lab content pack. The pack is intentionally data-driven: scenario answers
/// are never stored here; every asked measure resolves through Core/SimCalc at grade time.
/// </summary>
public static class SimLabContentPack
{
    sealed record Measure(string key, string label, string type);

    sealed record Scenario(
        string Code,
        string Title,
        string Kind,
        string Industry,
        string Difficulty,
        int Minutes,
        long CertificationId,
        string[] Competencies,
        string Summary,
        string Task,
        string Prompt,
        string GivenJson,
        Measure[] Ask,
        double Tolerance,
        int PassPct,
        string? VariantJson = null)
    {
        public string CompetenciesJson => JsonSerializer.Serialize(Competencies);

        public string ConfigJson
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append("{\"task\":").Append(JsonSerializer.Serialize(Task));
                sb.Append(",\"prompt\":").Append(JsonSerializer.Serialize(Prompt));
                sb.Append(",\"given\":").Append(GivenJson);
                sb.Append(",\"ask\":").Append(JsonSerializer.Serialize(Ask));
                sb.Append(",\"tolerance\":").Append(Tolerance.ToString(CultureInfo.InvariantCulture));
                sb.Append(",\"pass_pct\":").Append(PassPct.ToString(CultureInfo.InvariantCulture));
                sb.Append(",\"competencies\":").Append(JsonSerializer.Serialize(Competencies));
                if (!string.IsNullOrWhiteSpace(VariantJson))
                    sb.Append(",\"variant\":").Append(VariantJson);
                sb.Append('}');
                return sb.ToString();
            }
        }
    }

    static Measure A(string key, string label, string type = "number") => new(key, label, type);

    static readonly Measure[] EvmAsk =
    {
        A("sv", "Schedule Variance (SV)"),
        A("cv", "Cost Variance (CV)"),
        A("spi", "Schedule Performance Index (SPI)"),
        A("cpi", "Cost Performance Index (CPI)"),
        A("eac", "Estimate at Completion (CPI method)"),
        A("etc", "Estimate to Complete (ETC)"),
        A("vac", "Variance at Completion (VAC)"),
        A("tcpi", "To-Complete Performance Index (TCPI to BAC)"),
        A("percent_complete", "Percent complete (EV/BAC)"),
        A("percent_spent", "Percent spent (AC/BAC)"),
        A("eac_cpi", "EAC - CPI method"),
        A("eac_composite", "EAC - composite CPI x SPI method"),
        A("eac_budget", "EAC - budget-rate method"),
    };

    static readonly Measure[] WbsAsk =
    {
        A("root_total", "Root WBS roll-up total"),
        A("hundred_percent_valid", "Does the WBS satisfy the 100 percent rule?", "bool"),
    };

    static Measure[] CpmAsk(params string[] ids) =>
        new[] { A("project_duration", "Project duration"), A("critical_path", "Critical path activity IDs", "set") }
            .Concat(ids.Select(id => A($"float_{id}", $"Total float for activity {id}")))
            .ToArray();

    static Measure[] CbsAsk(params string[] ids) =>
        new[] { A("root_budget", "Root budget"), A("root_actual", "Root actual cost"), A("root_variance", "Root variance (budget minus actual)") }
            .Concat(ids.Select(id => A($"variance_{id}", $"Variance for cost account {id}")))
            .ToArray();

    static readonly Measure[] ProgressAsk =
    {
        A("overall_percent", "Overall weighted percent complete"),
        A("total_weight", "Total progress weight"),
    };

    static Measure[] RiskAsk(params string[] ids) =>
        new[] { A("emv", "Total risk-register EMV") }
            .Concat(ids.Select(id => A($"emv_{id}", $"EMV for risk {id}")))
            .ToArray();

    static readonly Measure[] PertAsk =
    {
        A("expected_duration", "PERT expected duration"),
        A("std_dev", "PERT path standard deviation"),
        A("variance", "PERT path variance"),
        A("prob_on_time", "Probability of finishing by the deadline (%)"),
    };

    static readonly Measure[] ChangeAsk =
    {
        A("revised_bac", "Revised BAC after approved changes"),
        A("revised_duration", "Revised duration after approved changes"),
        A("approved_cost_delta", "Approved cost delta"),
        A("approved_schedule_delta", "Approved schedule delta"),
        A("approved_count", "Approved change count"),
    };

    static Measure[] CashAsk(params int[] periods) =>
        new[] { A("final_position", "Final cumulative cash position"), A("peak_funding", "Peak funding requirement") }
            .Concat(periods.Select(p => A($"cumulative_{p}", $"Cumulative cash position after period {p}")))
            .ToArray();

    static readonly Measure[] TimelineAsk =
    {
        A("final_ev", "Final cumulative EV"),
        A("final_ac", "Final cumulative AC"),
        A("final_spi", "Final cumulative SPI"),
        A("final_cpi", "Final cumulative CPI"),
        A("final_eac", "Final EAC"),
        A("vac", "Variance at Completion"),
        A("worst_spi", "Worst period SPI"),
        A("worst_spi_period", "Period with worst SPI"),
        A("worst_cpi", "Worst period CPI"),
        A("worst_cpi_period", "Period with worst CPI"),
    };

    static readonly Measure[] EarnedScheduleAsk =
    {
        A("es", "Earned Schedule"),
        A("spi_time", "Schedule Performance Index in time"),
        A("sv_time", "Schedule Variance in time"),
        A("eac_time", "Independent time forecast"),
    };

    static readonly Measure[] ProductivityAsk =
    {
        A("planned_productivity", "Planned productivity"),
        A("actual_productivity", "Actual productivity"),
        A("factor", "Productivity factor"),
        A("hours_variance", "Hours variance"),
        A("qty_variance", "Quantity variance"),
    };

    static readonly Measure[] BoqAsk =
    {
        A("total", "Bill total"),
        A("line_count", "Line count"),
        A("average_rate", "Average rate"),
    };

    static readonly Measure[] ResourceAsk =
    {
        A("peak_demand", "Peak resource demand"),
        A("peak_capacity", "Peak resource capacity"),
        A("peak_overload", "Peak overload"),
        A("total_overdemand", "Total overdemand"),
        A("overloaded_periods", "Number of overloaded periods"),
    };

    static readonly Measure[] ProcurementAsk =
    {
        A("critical_delay", "Supplier delay beyond remaining float"),
        A("new_project_duration", "New project duration"),
        A("float_consumed", "Float consumed by supplier delay"),
    };

    static Measure[] PortfolioAsk(params string[] ids) =>
        new[] { A("top_score", "Highest portfolio score"), A("total_npv", "Total portfolio NPV") }
            .Concat(ids.Select(id => A($"score_{id}", $"Score for project {id}")))
            .ToArray();

    static Measure[] DecisionAsk(params string[] ids) =>
        new[] { A("best_score", "Best weighted impact score") }
            .Concat(ids.Select(id => A($"score_{id}", $"Weighted impact score for option {id}")))
            .ToArray();

    static readonly Measure[] DataQualityAsk =
    {
        A("anomaly_count", "Anomaly count"),
        A("completeness_pct", "Completeness percentage"),
        A("mean_abs_error", "Mean absolute error"),
        A("record_count", "Record count"),
    };

    static readonly Scenario[] Scenarios =
    {
        new("GL-WBS-001", "Structure a clinic expansion WBS", "guided_lab", "Construction", "foundation", 15, 1,
            new[] { "scope_structuring" },
            "Roll up a clinic expansion WBS from leaf budgets and test the 100 percent rule.",
            "wbs",
            "A regional clinic is expanding treatment rooms while staying open. Roll the work breakdown from leaf budgets to the root and confirm whether the declared structure satisfies the 100 percent rule.",
            """{"nodes":[{"id":"1","parent":null,"name":"Clinic expansion"},{"id":"1.1","parent":"1","name":"Design","value":65000},{"id":"1.2","parent":"1","name":"Enabling works"},{"id":"1.2.1","parent":"1.2","name":"Temporary services","value":42000},{"id":"1.2.2","parent":"1.2","name":"Decant rooms","value":38000},{"id":"1.3","parent":"1","name":"Build"},{"id":"1.3.1","parent":"1.3","name":"Structure","value":155000},{"id":"1.3.2","parent":"1.3","name":"MEP fit-out","value":117000},{"id":"1.4","parent":"1","name":"Commissioning","value":28000}]}""",
            WbsAsk, 0.01, 70),

        new("GL-EVM-001", "Calculate EVM for a battery substation", "guided_lab", "Energy", "foundation", 18, 1,
            new[] { "earned_value", "forecasting" },
            "Compute the full EVM and forecast set for a battery-storage substation package.",
            "evm",
            "At month 4, a battery-storage substation reports PV 320000, EV 296000 and AC 340000 against a BAC of 820000. Compute the full earned-value and forecast measures.",
            """{"pv":320000,"ev":296000,"ac":340000,"bac":820000}""",
            EvmAsk, 0.01, 72,
            """{"vary":{"pv":{"min":300000,"max":340000,"step":5000},"ev":{"min":280000,"max":315000,"step":5000},"ac":{"min":320000,"max":360000,"step":5000}}}"""),

        new("SD-EVM-001", "EVM drill for rail signalling", "skill_drill", "Rail", "intermediate", 12, 1,
            new[] { "earned_value", "forecasting" },
            "A timed drill covering all EVM snapshot and forecast measures for a signalling work package.",
            "evm",
            "A rail signalling package has PV 540000, EV 486000, AC 510000 and BAC 1200000. Produce the full EVM bank, including completion percentages and all EAC methods.",
            """{"pv":540000,"ev":486000,"ac":510000,"bac":1200000}""",
            EvmAsk, 0.01, 80),

        new("GL-SCH-001", "Identify the critical path for a bridge outage", "guided_lab", "Infrastructure", "intermediate", 22, 3,
            new[] { "schedule_analysis", "risk_management" },
            "Run a forward/backward pass on a bridge outage network and report floats for every activity.",
            "cpm",
            "A bridge bearing replacement must fit inside a possession window. Calculate the project duration, identify the critical path, and report total float for each activity.",
            """{"activities":[{"id":"A","dur":2,"preds":[]},{"id":"B","dur":5,"preds":["A"]},{"id":"C","dur":3,"preds":["A"]},{"id":"D","dur":4,"preds":["B"]},{"id":"E","dur":6,"preds":["B","C"]},{"id":"F","dur":2,"preds":["D","E"]}]}""",
            CpmAsk("A", "B", "C", "D", "E", "F"), 0.001, 75),

        new("SD-FCT-001", "Forecast final cost for offshore turbines", "skill_drill", "Renewables", "advanced", 14, 1,
            new[] { "forecasting", "earned_value", "cost_control" },
            "Compare CPI, composite, and budget-rate forecasts for an offshore wind package.",
            "evm",
            "An offshore turbine installation package reports PV 1180000, EV 1035000, AC 1120000 and BAC 2400000. Forecast the final cost using the full EVM bank.",
            """{"pv":1180000,"ev":1035000,"ac":1120000,"bac":2400000}""",
            EvmAsk, 0.01, 78),

        new("GL-CBS-001", "Roll up a hospital fit-out CBS", "guided_lab", "Healthcare", "intermediate", 18, 1,
            new[] { "cost_control", "earned_value" },
            "Roll budget and actual cost through a cost breakdown and interpret account variances.",
            "cbs",
            "A hospital fit-out has actuals reported at work-package level. Roll the CBS to the root and compute variances at every account shown.",
            """{"nodes":[{"id":"1","parent":null,"name":"Hospital fit-out"},{"id":"1.1","parent":"1","name":"Partitions","budget":145000,"actual":151000},{"id":"1.2","parent":"1","name":"Medical gases","budget":210000,"actual":198000},{"id":"1.3","parent":"1","name":"Clean power","budget":175000,"actual":188500},{"id":"1.4","parent":"1","name":"Validation","budget":45000,"actual":42000}]}""",
            CbsAsk("1", "1.1", "1.2", "1.3", "1.4"), 0.01, 75),

        new("SC-EVM-001", "Read the data-centre S-curve", "scenario", "Technology", "intermediate", 20, 1,
            new[] { "earned_value", "forecasting", "cost_control" },
            "Use current cumulative S-curve values to compute the complete EVM measure bank.",
            "evm",
            "A data-centre white-space fit-out reports PV 950000, EV 874000, AC 920000 and BAC 1900000 at the reporting cut-off. Compute the complete earned-value and forecast set.",
            """{"pv":950000,"ev":874000,"ac":920000,"bac":1900000,"series":[{"period":1,"pv":180000,"ev":165000,"ac":170000},{"period":2,"pv":390000,"ev":360000,"ac":382000},{"period":3,"pv":650000,"ev":596000,"ac":628000},{"period":4,"pv":950000,"ev":874000,"ac":920000}]}""",
            EvmAsk, 0.01, 75),

        new("GL-PRG-001", "Measure weighted progress on an assembly line", "guided_lab", "Manufacturing", "foundation", 12, 3,
            new[] { "progress_measurement" },
            "Compute overall physical progress and confirm the total reporting weight.",
            "progress",
            "A new assembly line is reporting physical progress by weighted work package. Calculate the overall percent complete and the total measurement weight.",
            """{"nodes":[{"id":"P1","name":"Cell foundations","weight":250000,"percent":100},{"id":"P2","name":"Robot installation","weight":420000,"percent":65},{"id":"P3","name":"Conveyors","weight":180000,"percent":40},{"id":"P4","name":"Commissioning","weight":150000,"percent":10}]}""",
            ProgressAsk, 0.01, 70),

        new("SD-RSK-001", "Value a refinery risk register", "skill_drill", "Oil & Gas", "intermediate", 12, 3,
            new[] { "risk_management", "cost_control" },
            "Calculate total and per-risk EMV for a mixed threat and opportunity register.",
            "risk",
            "A refinery turnaround register includes quantified threats and opportunities. Compute the EMV for the whole register and for each line item.",
            """{"risks":[{"id":"R1","name":"Crane availability","probability":0.28,"impact":-85000},{"id":"R2","name":"Hot-work permit delay","probability":0.35,"impact":-42000},{"id":"R3","name":"Vendor rebate","probability":0.2,"impact":30000},{"id":"R4","name":"Weather window","probability":0.18,"impact":-56000},{"id":"R5","name":"Productivity gain","probability":0.25,"impact":45000}]}""",
            RiskAsk("R1", "R2", "R3", "R4", "R5"), 0.01, 75),

        new("GL-PRT-001", "PERT estimate for a satellite payload", "guided_lab", "Aerospace", "advanced", 22, 3,
            new[] { "schedule_analysis", "risk_management" },
            "Compute PERT duration, spread, variance, and deadline confidence for a payload chain.",
            "pert",
            "Three payload integration activities sit on the controlling chain. Use the optimistic, most-likely and pessimistic estimates to calculate the PERT statistics and the chance of finishing by day 31.",
            """{"activities":[{"id":"A","o":5,"m":7,"p":11},{"id":"B","o":8,"m":10,"p":18},{"id":"C","o":4,"m":6,"p":12},{"id":"D","o":3,"m":5,"p":9}],"deadline":31}""",
            PertAsk, 0.02, 75),

        new("SC-MC-001", "Compare PERT risk for a clinical batch", "scenario", "Pharmaceutical", "advanced", 24, 3,
            new[] { "risk_management", "schedule_analysis", "data_quality" },
            "Use PERT normal approximation on a clinical batch chain and compare with a seeded simulation payload.",
            "pert",
            "A clinical batch-release chain has three-point estimates and a day-38 commitment. Compute the PERT measures before comparing them with the provided simulation distribution.",
            """{"activities":[{"id":"A","o":6,"m":8,"p":14,"preds":[]},{"id":"B","o":9,"m":12,"p":21,"preds":["A"]},{"id":"C","o":5,"m":7,"p":13,"preds":["B"]},{"id":"D","o":4,"m":5,"p":11,"preds":["C"]}],"deadline":38,"seed":9917,"iterations":5000}""",
            PertAsk, 0.02, 75),

        new("GL-CHG-001", "Apply approved defence changes", "guided_lab", "Defence", "intermediate", 16, 3,
            new[] { "change_control", "cost_control", "schedule_analysis" },
            "Roll only approved changes onto the baseline and separate pending or rejected items.",
            "change",
            "A defence test-rig baseline is BAC 3750000 over 180 days. Apply only approved change requests to determine the revised baseline, approved deltas, and count.",
            """{"baseline_bac":3750000,"baseline_duration":180,"changes":[{"id":"C1","title":"Cyber-hardening tests","status":"approved","cost_delta":185000,"schedule_delta":6},{"id":"C2","title":"Paint specification change","status":"rejected","cost_delta":42000,"schedule_delta":2},{"id":"C3","title":"Telemetry interface","status":"approved","cost_delta":96000,"schedule_delta":4},{"id":"C4","title":"Optional viewing gallery","status":"pending","cost_delta":125000,"schedule_delta":8},{"id":"C5","title":"Remove duplicate trial","status":"approved","cost_delta":-58000,"schedule_delta":-3}]}""",
            ChangeAsk, 0.01, 75),

        new("GL-CASH-001", "Forecast utility project funding", "guided_lab", "Utilities", "intermediate", 18, 2,
            new[] { "cash_flow", "financial_analysis" },
            "Roll monthly net cash to final position and peak funding requirement.",
            "cashflow",
            "A grid connection project has phased customer receipts and contractor payments. Compute each cumulative cash position, the final position, and peak funding need.",
            """{"periods":[{"period":1,"inflow":0,"outflow":180000},{"period":2,"inflow":75000,"outflow":220000},{"period":3,"inflow":150000,"outflow":260000},{"period":4,"inflow":420000,"outflow":210000},{"period":5,"inflow":360000,"outflow":190000},{"period":6,"inflow":260000,"outflow":120000}]}""",
            CashAsk(1, 2, 3, 4, 5, 6), 0.01, 75),

        new("SC-EVT-001", "Track a tunnel package month by month", "scenario", "Infrastructure", "advanced", 22, 1,
            new[] { "earned_value", "forecasting", "schedule_analysis" },
            "Read an EVM timeline to identify final status and the worst performance periods.",
            "timeline",
            "A tunnel systems package has cumulative PV, EV and AC by reporting month. Read the whole trajectory to compute final status and the worst schedule and cost periods.",
            """{"bac":5100000,"series":[{"period":1,"pv":420000,"ev":390000,"ac":410000},{"period":2,"pv":920000,"ev":850000,"ac":910000},{"period":3,"pv":1580000,"ev":1440000,"ac":1600000},{"period":4,"pv":2340000,"ev":2110000,"ac":2440000},{"period":5,"pv":3230000,"ev":2980000,"ac":3410000},{"period":6,"pv":4050000,"ev":3810000,"ac":4290000},{"period":7,"pv":4720000,"ev":4500000,"ac":4970000}]}""",
            TimelineAsk, 0.01, 78),

        new("SD-ESC-001", "Measure earned schedule for a software release", "scenario", "Software", "advanced", 20, 1,
            new[] { "schedule_analysis", "forecasting", "earned_value" },
            "Use the planned-value curve to convert schedule performance into time-based measures.",
            "earned_schedule",
            "A seven-sprint release has cumulative planned value and an actual time of sprint 5. Use earned schedule to compute time variance, SPI(t), and the independent time forecast.",
            """{"planned_duration":7,"at":5,"ev":760,"plan":[{"period":1,"pv":120},{"period":2,"pv":260},{"period":3,"pv":430},{"period":4,"pv":640},{"period":5,"pv":830},{"period":6,"pv":960},{"period":7,"pv":1100}]}""",
            EarnedScheduleAsk, 0.01, 78),

        new("GL-PROD-001", "Check pipe crew productivity", "guided_lab", "Water", "foundation", 12, 1,
            new[] { "productivity_analysis" },
            "Compare planned and actual installation productivity for a water-main crew.",
            "productivity",
            "A water-main crew planned to install 480 metres in 960 labour-hours. The earned quantity is 445 metres after booking 1040 labour-hours. Compute productivity and variance measures.",
            """{"planned_qty":480,"planned_hours":960,"earned_qty":445,"actual_hours":1040}""",
            ProductivityAsk, 0.01, 70),

        new("GL-BOQ-001", "Price a mining camp BOQ", "guided_lab", "Mining", "foundation", 14, 2,
            new[] { "quantity_surveying", "cost_control" },
            "Calculate a measured bill total, line count, and average rate.",
            "boq",
            "A temporary mining camp package has measured quantities and agreed rates. Calculate the bill total and rate statistics.",
            """{"lines":[{"id":"L1","qty":1200,"rate":18.5},{"id":"L2","qty":45,"rate":760},{"id":"L3","qty":320,"rate":42},{"id":"L4","qty":18,"rate":1350},{"id":"L5","qty":96,"rate":115}]}""",
            BoqAsk, 0.01, 70),

        new("GL-RES-001", "Level runway paving crews", "guided_lab", "Aviation", "foundation", 15, 3,
            new[] { "resource_management", "schedule_analysis" },
            "Read weekly labour demand against capacity and quantify overload.",
            "resource",
            "A runway paving plan has crew demand and available capacity by week. Identify the peak demand, peak capacity, overload magnitude, and overload duration.",
            """{"periods":[{"period":1,"demand":18,"capacity":20},{"period":2,"demand":24,"capacity":22},{"period":3,"demand":31,"capacity":25},{"period":4,"demand":28,"capacity":25},{"period":5,"demand":19,"capacity":24},{"period":6,"demand":16,"capacity":22}]}""",
            ResourceAsk, 0.01, 70),

        new("GL-PROC-001", "Assess a port crane supplier delay", "guided_lab", "Ports", "foundation", 12, 3,
            new[] { "procurement_management", "schedule_analysis" },
            "Convert supplier delay and remaining float into critical delay and revised project duration.",
            "procurement",
            "A ship-to-shore crane control panel is delayed by the supplier. The project has 9 days of remaining float on the path and a current duration of 214 days. Calculate the critical impact.",
            """{"project_duration":214,"remaining_float":9,"supplier_delay_days":16}""",
            ProcurementAsk, 0.01, 70),

        new("GL-DQ-001", "Audit public-sector progress data", "guided_lab", "Public Sector", "foundation", 14, 1,
            new[] { "data_quality", "progress_measurement" },
            "Count missing and anomalous progress records before reporting a dashboard.",
            "data_quality",
            "A public-sector dashboard import contains expected percent-complete values and reported values. Treat deviations above the threshold as anomalies and quantify completeness.",
            """{"threshold":5,"rows":[{"value":42,"expected":40},{"value":null,"expected":55},{"value":63,"expected":61},{"value":74,"expected":62},{"value":81,"expected":80},{"value":46,"expected":58},{"value":90,"expected":88},{"value":null,"expected":70}]}""",
            DataQualityAsk, 0.01, 70),

        new("GL-PORT-001", "Score school-modernisation options", "guided_lab", "Education", "foundation", 16, 2,
            new[] { "portfolio_selection", "financial_analysis" },
            "Apply weighted portfolio scoring to five small capital projects.",
            "portfolio",
            "A school board is ranking five modernisation projects using NPV, delivery risk, and strategic fit. Compute the total NPV and each weighted score.",
            """{"w_npv":0.45,"w_risk":0.25,"w_fit":0.30,"projects":[{"id":"P1","npv":420000,"risk":0.28,"fit":0.70},{"id":"P2","npv":360000,"risk":0.18,"fit":0.82},{"id":"P3","npv":510000,"risk":0.42,"fit":0.66},{"id":"P4","npv":275000,"risk":0.12,"fit":0.74},{"id":"P5","npv":390000,"risk":0.25,"fit":0.88}]}""",
            PortfolioAsk("P1", "P2", "P3", "P4", "P5"), 0.0001, 70),

        new("SD-DEC-001", "Choose a telecoms recovery option", "skill_drill", "Telecoms", "foundation", 14, 3,
            new[] { "decision_analysis", "risk_management" },
            "Use weighted cost, schedule, and risk impacts to compare recovery options.",
            "decision",
            "A fibre rollout is behind plan. Compare five recovery options using weighted cost, schedule, and risk impacts; lower score is better.",
            """{"w_cost":0.00002,"w_sched":1.8,"w_risk":35,"options":[{"id":"O1","cost":90000,"schedule":-8,"risk":0.42},{"id":"O2","cost":45000,"schedule":-4,"risk":0.25},{"id":"O3","cost":120000,"schedule":-11,"risk":0.55},{"id":"O4","cost":25000,"schedule":-2,"risk":0.18},{"id":"O5","cost":70000,"schedule":-7,"risk":0.30}]}""",
            DecisionAsk("O1", "O2", "O3", "O4", "O5"), 0.0001, 70),

        new("SC-CBS-002", "Investigate biotech cleanroom cost drift", "scenario", "Pharmaceutical", "intermediate", 22, 2,
            new[] { "cost_control", "data_quality", "forecasting" },
            "Roll a multi-account CBS and identify where budget variance is emerging.",
            "cbs",
            "A biotech cleanroom project has budget and actual cost at twelve work accounts. Roll the structure and compute variance at every account.",
            """{"nodes":[{"id":"1","parent":null,"name":"Cleanroom project"},{"id":"1.1","parent":"1","name":"Shell"},{"id":"1.1.1","parent":"1.1","name":"Envelope","budget":520000,"actual":548000},{"id":"1.1.2","parent":"1.1","name":"Floors","budget":210000,"actual":198000},{"id":"1.1.3","parent":"1.1","name":"Ceilings","budget":185000,"actual":191000},{"id":"1.2","parent":"1","name":"Services"},{"id":"1.2.1","parent":"1.2","name":"HVAC","budget":780000,"actual":842000},{"id":"1.2.2","parent":"1.2","name":"Process gases","budget":335000,"actual":329000},{"id":"1.2.3","parent":"1.2","name":"Controls","budget":245000,"actual":263000},{"id":"1.3","parent":"1","name":"Validation"},{"id":"1.3.1","parent":"1.3","name":"Commissioning","budget":160000,"actual":151000},{"id":"1.3.2","parent":"1.3","name":"Qualification","budget":190000,"actual":207000},{"id":"1.4","parent":"1","name":"Handover"},{"id":"1.4.1","parent":"1.4","name":"Training","budget":55000,"actual":47000},{"id":"1.4.2","parent":"1.4","name":"Documentation","budget":72000,"actual":76000}]}""",
            CbsAsk("1", "1.1", "1.1.1", "1.1.2", "1.1.3", "1.2", "1.2.1", "1.2.2", "1.2.3", "1.3", "1.3.1", "1.3.2", "1.4", "1.4.1", "1.4.2"), 0.01, 75),

        new("SC-CASH-002", "Model stadium cash exposure", "scenario", "Sports & Venues", "intermediate", 22, 2,
            new[] { "cash_flow", "financial_analysis", "cost_control" },
            "Trace a twelve-period cash-flow curve to quantify peak funding exposure.",
            "cashflow",
            "A stadium concourse upgrade has milestone receipts and contractor outflows across twelve periods. Compute the cumulative position for every period and the resulting funding exposure.",
            """{"periods":[{"period":1,"inflow":0,"outflow":210000},{"period":2,"inflow":0,"outflow":260000},{"period":3,"inflow":150000,"outflow":310000},{"period":4,"inflow":280000,"outflow":340000},{"period":5,"inflow":320000,"outflow":360000},{"period":6,"inflow":450000,"outflow":390000},{"period":7,"inflow":520000,"outflow":410000},{"period":8,"inflow":480000,"outflow":330000},{"period":9,"inflow":350000,"outflow":260000},{"period":10,"inflow":250000,"outflow":180000},{"period":11,"inflow":180000,"outflow":120000},{"period":12,"inflow":140000,"outflow":90000}]}""",
            CashAsk(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), 0.01, 75),

        new("SC-PRG-002", "Assess logistics-hub progress weights", "scenario", "Logistics", "intermediate", 18, 1,
            new[] { "progress_measurement", "earned_value" },
            "Calculate overall physical progress from uneven work-package weights.",
            "progress",
            "A logistics-hub programme reports physical progress across civil, automation, IT, and operations readiness packages. Compute the weighted overall progress and total weight.",
            """{"nodes":[{"id":"CIV","weight":1850000,"percent":72},{"id":"AUTO","weight":2460000,"percent":48},{"id":"IT","weight":720000,"percent":65},{"id":"OPS","weight":430000,"percent":35},{"id":"QA","weight":260000,"percent":28},{"id":"MIG","weight":380000,"percent":15}]}""",
            ProgressAsk, 0.01, 75),

        new("SC-PORT-002", "Rank a renewable-energy investment portfolio", "scenario", "Financial Services", "advanced", 24, 2,
            new[] { "portfolio_selection", "financial_analysis", "risk_management" },
            "Score six renewable-energy investments using NPV, fit, and risk weights.",
            "portfolio",
            "An infrastructure fund is screening six renewable-energy investments. Use the stated weights to compute each score and the aggregate NPV for the opportunity set.",
            """{"w_npv":0.50,"w_risk":0.30,"w_fit":0.20,"projects":[{"id":"A","npv":18500000,"risk":0.38,"fit":0.82},{"id":"B","npv":12200000,"risk":0.22,"fit":0.76},{"id":"C","npv":24700000,"risk":0.55,"fit":0.88},{"id":"D","npv":9600000,"risk":0.18,"fit":0.68},{"id":"E","npv":17100000,"risk":0.31,"fit":0.91},{"id":"F","npv":14300000,"risk":0.27,"fit":0.79}]}""",
            PortfolioAsk("A", "B", "C", "D", "E", "F"), 0.0001, 78),

        new("SC-RSK-002", "Quantify metro extension risk exposure", "scenario", "Rail", "advanced", 24, 3,
            new[] { "risk_management", "cost_control", "schedule_analysis" },
            "Compute total and line-item EMV for a dense metro extension register.",
            "risk",
            "A metro extension board wants quantified exposure by risk line before approving contingency. Compute total EMV and every individual EMV.",
            """{"risks":[{"id":"R1","probability":0.22,"impact":-1200000},{"id":"R2","probability":0.18,"impact":-760000},{"id":"R3","probability":0.31,"impact":-480000},{"id":"R4","probability":0.12,"impact":900000},{"id":"R5","probability":0.27,"impact":-350000},{"id":"R6","probability":0.16,"impact":-640000},{"id":"R7","probability":0.25,"impact":280000},{"id":"R8","probability":0.09,"impact":-1500000},{"id":"R9","probability":0.34,"impact":-220000},{"id":"R10","probability":0.2,"impact":410000},{"id":"R11","probability":0.14,"impact":-830000},{"id":"R12","probability":0.3,"impact":-190000}]}""",
            RiskAsk("R1", "R2", "R3", "R4", "R5", "R6", "R7", "R8", "R9", "R10", "R11", "R12"), 0.01, 78),

        new("CP-EVM-900", "Capstone EVM recovery for a semiconductor fab", "capstone", "Semiconductors", "expert", 32, 1,
            new[] { "earned_value", "forecasting", "cost_control", "decision_analysis" },
            "Use the complete EVM measure bank to brief a recovery decision on a fab utility module.",
            "evm",
            "A semiconductor fab utility module is late and over budget at the integrated baseline review. PV is 14800000, EV is 12950000, AC is 15320000, and BAC is 31200000. Compute every EVM and forecast measure before recommending recovery focus.",
            """{"pv":14800000,"ev":12950000,"ac":15320000,"bac":31200000}""",
            EvmAsk, 0.01, 82),

        new("CP-SCH-900", "Capstone schedule analysis for a data-centre cutover", "capstone", "Data Centres", "expert", 35, 3,
            new[] { "schedule_analysis", "resource_management", "risk_management" },
            "Perform a full critical-path analysis with float for every cutover activity.",
            "cpm",
            "A data-centre migration cutover has parallel technical, network, and commissioning paths. Determine the duration, critical path, and float for each activity.",
            """{"activities":[{"id":"A","dur":2,"preds":[]},{"id":"B","dur":4,"preds":["A"]},{"id":"C","dur":3,"preds":["A"]},{"id":"D","dur":6,"preds":["B"]},{"id":"E","dur":5,"preds":["B","C"]},{"id":"F","dur":4,"preds":["C"]},{"id":"G","dur":7,"preds":["D","E"]},{"id":"H","dur":3,"preds":["E"]},{"id":"I","dur":6,"preds":["F","H"]},{"id":"J","dur":2,"preds":["G","I"]},{"id":"K","dur":4,"preds":["J"]},{"id":"L","dur":1,"preds":["K"]},{"id":"M","dur":3,"preds":["H"]},{"id":"N","dur":2,"preds":["M","I"]},{"id":"O","dur":2,"preds":["N"]},{"id":"P","dur":1,"preds":["L","O"]}]}""",
            CpmAsk("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P"), 0.001, 82),

        new("CP-INT-900", "Capstone integrated EVM trend for a hospital programme", "capstone", "Healthcare", "expert", 35, 1,
            new[] { "earned_value", "forecasting", "cash_flow", "governance" },
            "Interpret a multi-period EVM trajectory and identify final and worst-period performance.",
            "timeline",
            "A hospital redevelopment board pack includes cumulative PV, EV and AC for nine periods. Compute final EVM status, forecast, and the periods where schedule and cost performance were weakest.",
            """{"bac":68000000,"series":[{"period":1,"pv":4200000,"ev":3900000,"ac":4100000},{"period":2,"pv":8600000,"ev":7900000,"ac":8400000},{"period":3,"pv":14500000,"ev":13200000,"ac":14800000},{"period":4,"pv":21800000,"ev":19800000,"ac":22900000},{"period":5,"pv":30400000,"ev":28100000,"ac":32600000},{"period":6,"pv":39100000,"ev":36500000,"ac":41800000},{"period":7,"pv":48200000,"ev":45600000,"ac":51400000},{"period":8,"pv":57400000,"ev":54800000,"ac":61200000},{"period":9,"pv":64000000,"ev":61800000,"ac":68700000}]}""",
            TimelineAsk, 0.01, 82),
    };

    public static int ScenarioCount => Scenarios.Length;

    public static int TotalAskCount => Scenarios.Sum(s => s.Ask.Length);

    /// <summary>Codes owned by this pack, so validators can scope queries to pack rows only
    /// (other house seeds in <see cref="SimLabSchema"/> are also synthetic_declared).</summary>
    public static IReadOnlyList<string> ScenarioCodes => Scenarios.Select(s => s.Code).ToArray();

    public static void Seed(Db db)
    {
        foreach (var s in Scenarios)
            Upsert(db, s);
    }

    static void Upsert(Db db, Scenario s)
    {
        var configJson = s.ConfigJson;
        db.Execute(@"INSERT OR IGNORE INTO simulation_scenarios
            (scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,summary,config_json,certification_id,review_state,synthetic_declared,status,version)
            VALUES(?,?,?,?,?,?,?,?,?,?, 'published', 1, 'published', 1)",
            s.Code, s.Title, s.Kind, s.Industry, s.Difficulty, s.Minutes, s.CompetenciesJson, s.Summary, configJson, s.CertificationId);

        // A config change is a NEW version, never an in-place rewrite of the old one: bumping `version`
        // keeps (scenario, version) naming exactly one config, so attempts pinned to the previous version
        // (simulation_scenario_versions) replay identically after this deploy. Metadata-only changes
        // (title, summary, minutes…) don't affect grading and keep the version. A row just inserted above
        // already carries this config, so it never bumps.
        var cur = db.QueryOne("SELECT config_json FROM simulation_scenarios WHERE scenario_code=? AND authored_by IS NULL", s.Code);
        var bump = cur is not null && (Convert.ToString(cur["config_json"]) ?? "") != configJson ? 1 : 0;

        db.Execute(@"UPDATE simulation_scenarios
            SET title=?, kind=?, industry=?, difficulty=?, est_minutes=?, competencies_json=?,
                summary=?, config_json=?, certification_id=?, review_state='published',
                synthetic_declared=1, status='published', version=version+?, updated_at=datetime('now'),
                published_at=COALESCE(published_at, datetime('now'))
            WHERE scenario_code=? AND authored_by IS NULL",
            s.Title, s.Kind, s.Industry, s.Difficulty, s.Minutes, s.CompetenciesJson,
            s.Summary, configJson, s.CertificationId, bump, s.Code);
    }
}
