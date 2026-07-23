using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// PCI AI Project Controls Simulation Lab — deterministic calculation engine (Phase 1/2 foundation).
///
/// Pure, side-effect-free functions: no database, no clock, no randomness. Given the same inputs they
/// always return the same numbers, which is exactly what the spec's non-negotiable "critical calculations
/// must be deterministic and unit-tested" requires. Every method here is exercised directly by
/// SimCalcTests (xUnit, the backend-unit CI job) and is the single source of truth the grading layer
/// (Core/SimLab) computes authoritative answers with — the AI Coach never computes numbers, it only
/// explains the ones this engine produced.
///
/// Three engines cover the Phase 1 guided labs: Earned Value (EVM), Critical Path (CPM forward/backward
/// pass), and Work Breakdown Structure roll-up + the 100% rule. Later phases extend this file (forecasting
/// variants, risk/Monte-Carlo, cash flow) rather than scattering arithmetic across endpoints.
/// </summary>
public static class SimCalc
{
    /// <summary>Round to 4 dp for stable equality in tests and JSON; callers present fewer.</summary>
    static double R(double v) => Math.Round(v, 4, MidpointRounding.AwayFromZero);

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Earned Value Management
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public readonly record struct EvmResult(
        double PV, double EV, double AC, double? BAC,
        double SV, double CV, double SPI, double CPI,
        double? EAC, double? ETC, double? VAC, double? TCPI,
        double? PercentComplete, double? PercentSpent);

    /// <summary>
    /// Core EVM measures. SPI/CPI guard division by zero (a period with zero PV/AC yields 0, the
    /// conventional "no basis" reading rather than an exception). BAC-dependent forecasts (EAC via the
    /// typical CPI method, ETC, VAC, TCPI) are computed only when a budget-at-completion is supplied.
    /// </summary>
    public static EvmResult Evm(double pv, double ev, double ac, double? bac = null)
    {
        var sv = ev - pv;
        var cv = ev - ac;
        var spi = pv == 0 ? 0 : ev / pv;
        var cpi = ac == 0 ? 0 : ev / ac;

        double? eac = null, etc = null, vac = null, tcpi = null, pctComplete = null, pctSpent = null;
        if (bac is double b)
        {
            // EAC by the typical (CPI-based) method: assumes current cost performance continues.
            eac = cpi == 0 ? (double?)null : b / cpi;
            if (eac is double e) { etc = e - ac; vac = b - e; }
            // To-Complete Performance Index: efficiency needed on remaining work to still hit the BAC.
            var remainingBudget = b - ac;
            tcpi = remainingBudget == 0 ? 0 : (b - ev) / remainingBudget;
            pctComplete = b == 0 ? 0 : ev / b;
            pctSpent = b == 0 ? 0 : ac / b;
        }

        return new EvmResult(R(pv), R(ev), R(ac), bac is double bb ? R(bb) : null,
            R(sv), R(cv), R(spi), R(cpi),
            eac is double ev2 ? R(ev2) : null, etc is double et ? R(et) : null,
            vac is double va ? R(va) : null, tcpi is double tc ? R(tc) : null,
            pctComplete is double pc ? R(pc) : null, pctSpent is double ps ? R(ps) : null);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Critical Path Method (forward/backward pass on an activity-on-node network)
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record CpmActivity(string Id, double Duration, IReadOnlyList<string> Predecessors);

    public sealed record CpmNode(string Id, double Duration, double Es, double Ef, double Ls, double Lf, double TotalFloat)
    {
        public bool Critical => Math.Abs(TotalFloat) < 1e-9;
    }

    public sealed record CpmResult(
        double ProjectDuration,
        IReadOnlyList<CpmNode> Nodes,
        IReadOnlyList<string> CriticalPath);

    /// <summary>
    /// Forward + backward pass over an activity-on-node network. Returns each activity's ES/EF/LS/LF and
    /// total float, the project duration, and the critical path (zero-float activities in topological
    /// order). Throws on an unknown predecessor or a cycle — a malformed network is a scenario-authoring
    /// error, never a silently-wrong schedule.
    /// </summary>
    public static CpmResult CriticalPath(IReadOnlyList<CpmActivity> activities)
    {
        if (activities.Count == 0) return new CpmResult(0, Array.Empty<CpmNode>(), Array.Empty<string>());
        var byId = activities.ToDictionary(a => a.Id);
        foreach (var a in activities)
            foreach (var p in a.Predecessors)
                if (!byId.ContainsKey(p))
                    throw new ArgumentException($"activity '{a.Id}' names unknown predecessor '{p}'");

        var order = TopoSort(activities, byId);   // throws on cycle

        // Forward pass: ES = max(EF of predecessors); EF = ES + duration.
        var es = new Dictionary<string, double>();
        var ef = new Dictionary<string, double>();
        foreach (var id in order)
        {
            var a = byId[id];
            var start = a.Predecessors.Count == 0 ? 0 : a.Predecessors.Max(p => ef[p]);
            es[id] = start;
            ef[id] = start + a.Duration;
        }
        var projectDuration = ef.Values.Max();

        // Backward pass: LF = min(LS of successors) or project duration for terminal activities.
        var successors = activities.ToDictionary(a => a.Id, _ => new List<string>());
        foreach (var a in activities)
            foreach (var p in a.Predecessors)
                successors[p].Add(a.Id);

        var lf = new Dictionary<string, double>();
        var ls = new Dictionary<string, double>();
        foreach (var id in order.AsEnumerable().Reverse())
        {
            var a = byId[id];
            var finish = successors[id].Count == 0 ? projectDuration : successors[id].Min(s => ls[s]);
            lf[id] = finish;
            ls[id] = finish - a.Duration;
        }

        var nodes = order.Select(id => new CpmNode(id, byId[id].Duration,
            R(es[id]), R(ef[id]), R(ls[id]), R(lf[id]), R(ls[id] - es[id]))).ToList();
        var critical = nodes.Where(n => n.Critical).Select(n => n.Id).ToList();
        return new CpmResult(R(projectDuration), nodes, critical);
    }

    static List<string> TopoSort(IReadOnlyList<CpmActivity> activities, Dictionary<string, CpmActivity> byId)
    {
        // Kahn's algorithm; deterministic tie-break by the scenario's declared activity order.
        var indeg = activities.ToDictionary(a => a.Id, a => a.Predecessors.Count);
        var succ = activities.ToDictionary(a => a.Id, _ => new List<string>());
        foreach (var a in activities)
            foreach (var p in a.Predecessors)
                succ[p].Add(a.Id);

        var ready = activities.Where(a => indeg[a.Id] == 0).Select(a => a.Id).ToList();
        var seq = new List<string>();
        var idx = activities.Select((a, i) => (a.Id, i)).ToDictionary(t => t.Id, t => t.i);
        while (ready.Count > 0)
        {
            ready.Sort((x, y) => idx[x].CompareTo(idx[y]));
            var id = ready[0];
            ready.RemoveAt(0);
            seq.Add(id);
            foreach (var s in succ[id])
                if (--indeg[s] == 0) ready.Add(s);
        }
        if (seq.Count != activities.Count)
            throw new ArgumentException("activity network contains a cycle");
        return seq;
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Work Breakdown Structure — roll-up and the 100% rule
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record WbsInputNode(string Id, string? Parent, double? Value);

    public sealed record WbsNode(string Id, string? Parent, double Value, bool IsLeaf, int Depth);

    public sealed record WbsResult(
        double RootTotal,
        IReadOnlyList<WbsNode> Nodes,
        bool HundredPercentValid,
        IReadOnlyList<string> Violations);

    /// <summary>
    /// Roll a WBS up from its leaves. Each leaf carries a value (budget or weight); each parent's value is
    /// the sum of its children (the "100% rule": a parent equals the sum of its parts and nothing is left
    /// out or double-counted). Returns the rolled-up totals, the grand total at the root, and whether the
    /// structure satisfies the 100% rule against any parent values that were pre-supplied. Throws on a
    /// missing parent or a cycle — structural errors, not scoring outcomes.
    /// </summary>
    public static WbsResult Wbs(IReadOnlyList<WbsInputNode> input, double tolerance = 0.01)
    {
        if (input.Count == 0) return new WbsResult(0, Array.Empty<WbsNode>(), true, Array.Empty<string>());
        var byId = input.ToDictionary(n => n.Id);
        var children = input.ToDictionary(n => n.Id, _ => new List<string>());
        string? root = null;
        foreach (var n in input)
        {
            if (n.Parent is null) { root ??= n.Id; continue; }
            if (!byId.ContainsKey(n.Parent)) throw new ArgumentException($"node '{n.Id}' names unknown parent '{n.Parent}'");
            children[n.Parent].Add(n.Id);
        }

        var depth = new Dictionary<string, int>();
        int Depth(string id, int guard)
        {
            if (guard > input.Count) throw new ArgumentException("WBS contains a cycle");
            if (depth.TryGetValue(id, out var d)) return d;
            var p = byId[id].Parent;
            return depth[id] = p is null ? 0 : Depth(p, guard + 1) + 1;
        }
        foreach (var n in input) Depth(n.Id, 0);

        var rolled = new Dictionary<string, double>();
        double Roll(string id)
        {
            var kids = children[id];
            if (kids.Count == 0) return rolled[id] = byId[id].Value ?? 0;   // leaf
            var sum = kids.Sum(Roll);
            return rolled[id] = sum;
        }
        foreach (var n in input) if (!rolled.ContainsKey(n.Id)) Roll(n.Id);

        // 100% rule: any node that was given an explicit value AND has children must equal its roll-up.
        var violations = new List<string>();
        foreach (var n in input)
        {
            if (children[n.Id].Count == 0 || n.Value is not double declared) continue;
            if (Math.Abs(declared - rolled[n.Id]) > tolerance)
                violations.Add(n.Id);
        }

        var nodes = input.Select(n => new WbsNode(n.Id, n.Parent, R(rolled[n.Id]),
            children[n.Id].Count == 0, depth[n.Id])).ToList();
        var rootTotal = root is null ? R(nodes.Where(n => n.Parent is null).Sum(n => n.Value)) : R(rolled[root]);
        return new WbsResult(rootTotal, nodes, violations.Count == 0, violations);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Answer resolution — the values a scenario can ask a student to compute
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Compute the authoritative value of a single named measure for a scenario's <c>given</c> inputs.
    /// The grader in Core/SimLab calls this so there is exactly ONE definition of every answer. Numeric
    /// measures return a boxed double; set-valued measures (the critical path) return a string[]. An
    /// unknown key returns null — the scenario author asked for something the engine does not compute.
    /// </summary>
    public static object? Resolve(string task, string key, JsonElement given)
    {
        double G(string name) => given.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
        double? GN(string name) => given.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : (double?)null;

        switch (task)
        {
            case "evm":
            {
                var r = Evm(G("pv"), G("ev"), G("ac"), GN("bac"));
                return key switch
                {
                    "sv" => (object?)r.SV, "cv" => r.CV, "spi" => r.SPI, "cpi" => r.CPI,
                    "eac" => r.EAC, "etc" => r.ETC, "vac" => r.VAC, "tcpi" => r.TCPI,
                    "percent_complete" => r.PercentComplete, "percent_spent" => r.PercentSpent,
                    _ => null,
                };
            }
            case "cpm":
            {
                var acts = ParseCpm(given);
                var r = CriticalPath(acts);
                if (key == "project_duration") return r.ProjectDuration;
                if (key == "critical_path") return r.CriticalPath.ToArray();
                if (key.StartsWith("float_", StringComparison.Ordinal))
                {
                    var id = key.Substring("float_".Length);
                    return r.Nodes.FirstOrDefault(n => n.Id == id)?.TotalFloat;
                }
                return null;
            }
            case "wbs":
            {
                var r = Wbs(ParseWbs(given));
                return key switch
                {
                    "root_total" => (object?)r.RootTotal,
                    "hundred_percent_valid" => r.HundredPercentValid,
                    _ => null,
                };
            }
            default:
                return null;
        }
    }

    static List<CpmActivity> ParseCpm(JsonElement given)
    {
        var list = new List<CpmActivity>();
        if (given.TryGetProperty("activities", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var a in arr.EnumerateArray())
            {
                var id = a.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                var dur = a.TryGetProperty("dur", out var d) && d.ValueKind == JsonValueKind.Number ? d.GetDouble() : 0;
                var preds = new List<string>();
                if (a.TryGetProperty("preds", out var pe) && pe.ValueKind == JsonValueKind.Array)
                    foreach (var p in pe.EnumerateArray()) if (p.GetString() is { } s) preds.Add(s);
                list.Add(new CpmActivity(id, dur, preds));
            }
        return list;
    }

    static List<WbsInputNode> ParseWbs(JsonElement given)
    {
        var list = new List<WbsInputNode>();
        if (given.TryGetProperty("nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                string? parent = n.TryGetProperty("parent", out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;
                double? val = n.TryGetProperty("value", out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : (double?)null;
                list.Add(new WbsInputNode(id, parent, val));
            }
        return list;
    }
}
