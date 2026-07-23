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
    //  Forecasting — the three standard Estimate-at-Completion methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public readonly record struct ForecastResult(
        double CPI, double SPI,
        double EacCpi,        // BAC / CPI            — current cost performance continues
        double EacComposite,  // AC + (BAC−EV)/(CPI·SPI) — cost AND schedule performance continue
        double EacBudget,     // AC + (BAC−EV)        — remaining work at the budgeted rate
        double Etc, double Vac, double Tcpi);

    /// <summary>
    /// The three conventional EAC forecasts, from the same PV/EV/AC/BAC. EacCpi assumes current cost
    /// efficiency holds; EacComposite assumes both cost and schedule efficiency hold; EacBudget assumes the
    /// remaining work runs at the original budgeted rate. Divisions guard zero (a degenerate index yields
    /// the budget-rate estimate rather than an exception). ETC/VAC/TCPI use the CPI forecast.
    /// </summary>
    public static ForecastResult Forecast(double pv, double ev, double ac, double bac)
    {
        var cpi = ac == 0 ? 0 : ev / ac;
        var spi = pv == 0 ? 0 : ev / pv;
        var remaining = bac - ev;
        var eacCpi = cpi == 0 ? ac + remaining : bac / cpi;
        var eacComposite = (cpi * spi) == 0 ? ac + remaining : ac + remaining / (cpi * spi);
        var eacBudget = ac + remaining;
        var etc = eacCpi - ac;
        var vac = bac - eacCpi;
        var tcpi = (bac - ac) == 0 ? 0 : remaining / (bac - ac);
        return new ForecastResult(R(cpi), R(spi), R(eacCpi), R(eacComposite), R(eacBudget), R(etc), R(vac), R(tcpi));
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Cost Breakdown Structure — budget vs actual roll-up and variance
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record CbsInputNode(string Id, string? Parent, double? Budget, double? Actual);

    public sealed record CbsNode(string Id, string? Parent, double Budget, double Actual, double Variance, bool IsLeaf);

    public sealed record CbsResult(double TotalBudget, double TotalActual, double Variance, IReadOnlyList<CbsNode> Nodes);

    /// <summary>
    /// Roll budget and actual cost up a cost breakdown structure from its leaf accounts, and compute the
    /// variance (budget − actual; positive is under budget) at every node and at the root. Throws on a
    /// missing parent or a cycle — structural errors, not scoring outcomes.
    /// </summary>
    public static CbsResult Cbs(IReadOnlyList<CbsInputNode> input)
    {
        if (input.Count == 0) return new CbsResult(0, 0, 0, Array.Empty<CbsNode>());
        var byId = input.ToDictionary(n => n.Id);
        var children = input.ToDictionary(n => n.Id, _ => new List<string>());
        string? root = null;
        foreach (var n in input)
        {
            if (n.Parent is null) { root ??= n.Id; continue; }
            if (!byId.ContainsKey(n.Parent)) throw new ArgumentException($"cost account '{n.Id}' names unknown parent '{n.Parent}'");
            children[n.Parent].Add(n.Id);
        }

        var budget = new Dictionary<string, double>();
        var actual = new Dictionary<string, double>();
        var guard = 0;
        (double b, double a) Roll(string id)
        {
            if (++guard > input.Count * input.Count) throw new ArgumentException("CBS contains a cycle");
            var kids = children[id];
            if (kids.Count == 0) { budget[id] = byId[id].Budget ?? 0; actual[id] = byId[id].Actual ?? 0; return (budget[id], actual[id]); }
            double b = 0, a = 0;
            foreach (var k in kids) { var (kb, ka) = Roll(k); b += kb; a += ka; }
            budget[id] = b; actual[id] = a; return (b, a);
        }
        foreach (var n in input) if (!budget.ContainsKey(n.Id)) Roll(n.Id);

        var nodes = input.Select(n => new CbsNode(n.Id, n.Parent, R(budget[n.Id]), R(actual[n.Id]),
            R(budget[n.Id] - actual[n.Id]), children[n.Id].Count == 0)).ToList();
        var tb = root is null ? nodes.Where(n => n.Parent is null).Sum(n => n.Budget) : budget[root];
        var ta = root is null ? nodes.Where(n => n.Parent is null).Sum(n => n.Actual) : actual[root];
        return new CbsResult(R(tb), R(ta), R(tb - ta), nodes);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Physical progress — weighted percent-complete roll-up
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record ProgressInputNode(string Id, double? Weight, double? Percent);

    public sealed record ProgressResult(double OverallPercent, double TotalWeight);

    /// <summary>
    /// Overall physical percent-complete as a weight-weighted average of each work package's own
    /// percent-complete: Σ(weight·percent) ÷ Σ(weight). Packages missing a weight or a percent are ignored
    /// (a package with no weight contributes nothing to the roll-up). Zero total weight yields 0.
    /// </summary>
    public static ProgressResult Progress(IReadOnlyList<ProgressInputNode> input)
    {
        double wsum = 0, wp = 0;
        foreach (var n in input)
        {
            if (n.Weight is not double w || n.Percent is not double p) continue;
            wsum += w; wp += w * p;
        }
        return new ProgressResult(wsum == 0 ? 0 : R(wp / wsum), R(wsum));
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Risk — Expected Monetary Value and three-point (PERT) analysis
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record RiskItem(string Id, double Probability, double Impact);

    public sealed record EmvResult(double Total, IReadOnlyList<(string id, double emv)> Items);

    /// <summary>
    /// Expected Monetary Value of a risk register: Σ(probability · impact). Signed impacts let a threat
    /// (negative) and an opportunity (positive) net off. Per-risk EMVs are returned for the breakdown.
    /// </summary>
    public static EmvResult Emv(IReadOnlyList<RiskItem> risks)
    {
        double total = 0;
        var items = new List<(string, double)>();
        foreach (var r in risks) { var e = r.Probability * r.Impact; total += e; items.Add((r.Id, R(e))); }
        return new EmvResult(R(total), items);
    }

    public readonly record struct PertResult(double Expected, double StdDev, double Variance);

    /// <summary>Three-point (PERT/beta) estimate for one activity: expected = (O + 4M + P)/6, standard
    /// deviation = (P − O)/6, variance = ((P − O)/6)².</summary>
    public static PertResult Pert(double o, double m, double p)
    {
        var e = (o + 4 * m + p) / 6.0;
        var sd = (p - o) / 6.0;
        return new PertResult(R(e), R(sd), R(sd * sd));
    }

    public sealed record PertPathResult(double Expected, double StdDev, double Variance, double? ProbOnTimePercent);

    /// <summary>
    /// PERT for a chain of activities in series (e.g. the critical path): the path's expected duration is
    /// the sum of the activities' expected durations, its variance the sum of their variances. If a
    /// deadline is supplied, the probability of finishing on time is Φ((deadline − expected) / σ) as a
    /// percentage (the normal approximation), where σ is the path standard deviation.
    /// </summary>
    public static PertPathResult PertPath(IReadOnlyList<(double o, double m, double p)> acts, double? deadline)
    {
        double e = 0, var = 0;
        foreach (var a in acts) { var r = Pert(a.o, a.m, a.p); e += r.Expected; var += r.Variance; }
        var sd = Math.Sqrt(var);
        double? prob = null;
        if (deadline is double d)
            prob = sd == 0 ? (d >= e ? 100 : 0) : 100.0 * NormalCdf((d - e) / sd);
        return new PertPathResult(R(e), R(sd), R(var), prob is double pr ? R(pr) : null);
    }

    /// <summary>Standard-normal CDF Φ(z) via the Abramowitz &amp; Stegun 7.1.26 erf approximation
    /// (max error ≈ 1.5e-7) — deterministic and dependency-free.</summary>
    public static double NormalCdf(double z)
    {
        var sign = z < 0 ? -1.0 : 1.0;
        var x = Math.Abs(z) / Math.Sqrt(2.0);
        var t = 1.0 / (1.0 + 0.3275911 * x);
        var y = 1.0 - (((((1.061405429 * t - 1.453152027) * t) + 1.421413741) * t - 0.284496736) * t + 0.254829592) * t * Math.Exp(-x * x);
        return 0.5 * (1.0 + sign * y);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Monte-Carlo schedule simulation — deterministic (seeded), portable PRNG
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record McActivity(string Id, double O, double M, double P, IReadOnlyList<string> Preds);

    public sealed record McBucket(double Lo, double Hi, int Count);

    public sealed record McResult(
        int Iterations, double Mean, double Min, double Max,
        double P10, double P50, double P80, double P90,
        IReadOnlyList<McBucket> Histogram);

    /// <summary>
    /// Monte-Carlo of project duration: each iteration samples every activity's duration from a triangular
    /// distribution (O/M/P) and runs the critical-path method over the network, giving the distribution of
    /// finish dates. Uses a self-contained xorshift* PRNG seeded from <paramref name="seed"/>, so results
    /// are fully deterministic and reproducible on any platform (unlike System.Random). Returns percentiles
    /// and a fixed-bucket histogram for the dashboard.
    /// </summary>
    public static McResult MonteCarlo(IReadOnlyList<McActivity> acts, int seed, int iterations)
    {
        iterations = Math.Clamp(iterations, 1, 100_000);
        var rng = new DetRng((ulong)seed + 0x9E3779B97F4A7C15UL);
        var results = new double[iterations];
        for (var i = 0; i < iterations; i++)
        {
            var sampled = acts.Select(a => new CpmActivity(a.Id, Triangular(rng.NextDouble(), a.O, a.M, a.P), a.Preds)).ToList();
            results[i] = sampled.Count == 0 ? 0 : CriticalPath(sampled).ProjectDuration;
        }
        Array.Sort(results);
        var mean = results.Average();
        var min = results[0];
        var max = results[^1];

        const int buckets = 10;
        var width = max > min ? (max - min) / buckets : 1;
        var hist = new List<McBucket>();
        for (var b = 0; b < buckets; b++)
        {
            var lo = min + b * width;
            var hi = b == buckets - 1 ? max : lo + width;
            var count = results.Count(v => v >= lo && (b == buckets - 1 ? v <= hi : v < hi));
            hist.Add(new McBucket(R(lo), R(hi), count));
        }
        return new McResult(iterations, R(mean), R(min), R(max),
            R(Percentile(results, 10)), R(Percentile(results, 50)), R(Percentile(results, 80)), R(Percentile(results, 90)), hist);
    }

    static double Triangular(double u, double o, double m, double p)
    {
        if (p <= o) return o;
        var f = (m - o) / (p - o);
        return u < f ? o + Math.Sqrt(u * (p - o) * (m - o)) : p - Math.Sqrt((1 - u) * (p - o) * (p - m));
    }

    static double Percentile(double[] sorted, double pct)
    {
        if (sorted.Length == 1) return sorted[0];
        var rank = (pct / 100.0) * (sorted.Length - 1);
        var lo = (int)Math.Floor(rank);
        var hi = (int)Math.Ceiling(rank);
        if (lo == hi) return sorted[lo];
        return sorted[lo] + (rank - lo) * (sorted[hi] - sorted[lo]);
    }

    /// <summary>A tiny, self-contained deterministic PRNG (xorshift64*) so Monte-Carlo results are identical
    /// on every machine — the "deterministic critical calculations" guarantee extends to the seeded sim.</summary>
    sealed class DetRng
    {
        ulong _s;
        public DetRng(ulong seed) => _s = seed == 0 ? 0xD1B54A32D192ED03UL : seed;
        public double NextDouble()
        {
            _s ^= _s >> 12; _s ^= _s << 25; _s ^= _s >> 27;
            var r = _s * 0x2545F4914F6CDD1DUL;
            return (r >> 11) * (1.0 / (1UL << 53));
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Change control — approved-change roll-up onto the baseline
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record ChangeItem(string Id, string Status, double CostDelta, double ScheduleDelta);

    public sealed record ChangeResult(double RevisedBac, double RevisedDuration, double ApprovedCostDelta, double ApprovedScheduleDelta, int ApprovedCount);

    /// <summary>
    /// Apply only the APPROVED changes in a change register to the baseline: revised BAC = baseline +
    /// Σ(approved cost deltas), revised duration = baseline + Σ(approved schedule deltas). Pending and
    /// rejected changes never move the baseline — that is the discipline the lab teaches.
    /// </summary>
    public static ChangeResult ChangeControl(double baseBac, double baseDuration, IReadOnlyList<ChangeItem> changes)
    {
        double cost = 0, sched = 0; var n = 0;
        foreach (var c in changes)
            if (string.Equals(c.Status, "approved", StringComparison.OrdinalIgnoreCase)) { cost += c.CostDelta; sched += c.ScheduleDelta; n++; }
        return new ChangeResult(R(baseBac + cost), R(baseDuration + sched), R(cost), R(sched), n);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Cash flow — cumulative net position and peak funding requirement
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record CashPeriod(int Period, double Inflow, double Outflow);

    public sealed record CashPoint(int Period, double Net, double Cumulative);

    public sealed record CashResult(double FinalPosition, double PeakFunding, IReadOnlyList<CashPoint> Series);

    /// <summary>
    /// Roll a time-phased cash flow into its cumulative net position. The final position is the closing
    /// cumulative; the peak funding requirement is the deepest cumulative deficit as a positive magnitude
    /// (0 if the project is never cash-negative). Periods are processed in order.
    /// </summary>
    public static CashResult CashFlow(IReadOnlyList<CashPeriod> periods)
    {
        double cum = 0, minCum = 0;
        var series = new List<CashPoint>();
        foreach (var p in periods.OrderBy(x => x.Period))
        {
            var net = p.Inflow - p.Outflow;
            cum += net;
            if (cum < minCum) minCum = cum;
            series.Add(new CashPoint(p.Period, R(net), R(cum)));
        }
        return new CashResult(R(cum), R(minCum < 0 ? -minCum : 0), series);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Time-driven EVM timeline — replay a project period by period
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record TimelineInput(int Period, double Pv, double Ev, double Ac);

    public sealed record TimelinePoint(
        int Period, double Pv, double Ev, double Ac,
        double Sv, double Cv, double Spi, double Cpi, double Eac);

    public sealed record TimelineResult(
        IReadOnlyList<TimelinePoint> Series,
        double FinalEv, double FinalAc, double FinalSpi, double FinalCpi, double FinalEac, double Vac,
        double WorstSpi, int WorstSpiPeriod, double WorstCpi, int WorstCpiPeriod);

    /// <summary>
    /// Step a project through its reporting periods, computing the earned-value snapshot at each one from
    /// the cumulative Planned Value, Earned Value and Actual Cost recorded to date. This is the "time-driven"
    /// view the spec's dynamic simulation calls for: rather than one snapshot, the student reads the whole
    /// trajectory — where schedule and cost performance were worst, and where the CPI-method forecast (EAC =
    /// BAC ÷ CPI) is heading. Periods are processed in order; SPI/CPI guard division by zero (0 = "no basis").
    /// The worst-period selection keeps the FIRST period at the minimum, so ties resolve deterministically.
    /// </summary>
    public static TimelineResult EvmTimeline(IReadOnlyList<TimelineInput> periods, double bac)
    {
        var series = new List<TimelinePoint>();
        double worstSpi = double.PositiveInfinity, worstCpi = double.PositiveInfinity;
        int worstSpiP = 0, worstCpiP = 0;
        TimelineInput? last = null;
        foreach (var p in periods.OrderBy(x => x.Period))
        {
            var spi = p.Pv == 0 ? 0 : p.Ev / p.Pv;
            var cpi = p.Ac == 0 ? 0 : p.Ev / p.Ac;
            var eac = cpi == 0 ? 0 : bac / cpi;
            series.Add(new TimelinePoint(p.Period, R(p.Pv), R(p.Ev), R(p.Ac),
                R(p.Ev - p.Pv), R(p.Ev - p.Ac), R(spi), R(cpi), R(eac)));
            if (spi < worstSpi) { worstSpi = spi; worstSpiP = p.Period; }
            if (cpi < worstCpi) { worstCpi = cpi; worstCpiP = p.Period; }
            last = p;
        }
        var lp = last ?? new TimelineInput(0, 0, 0, 0);
        var fSpi = lp.Pv == 0 ? 0 : lp.Ev / lp.Pv;
        var fCpi = lp.Ac == 0 ? 0 : lp.Ev / lp.Ac;
        var fEac = fCpi == 0 ? 0 : bac / fCpi;
        return new TimelineResult(series, R(lp.Ev), R(lp.Ac), R(fSpi), R(fCpi), R(fEac), R(bac - fEac),
            R(double.IsPositiveInfinity(worstSpi) ? 0 : worstSpi), worstSpiP,
            R(double.IsPositiveInfinity(worstCpi) ? 0 : worstCpi), worstCpiP);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Earned Schedule — schedule performance measured in time, not cost
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    public sealed record EarnedScheduleResult(double Es, double SvTime, double SpiTime, double? EacTime);

    /// <summary>
    /// Earned Schedule (Lipke): the time at which the plan intended to have earned what the project has
    /// actually earned so far. Where the classic earned-value SPI collapses to 1.0 as a late project
    /// finishes (EV → PV → BAC), ES stays meaningful because it works in the time dimension. ES is read off
    /// the planned-value curve — the last period whose cumulative PV does not exceed the current EV, plus the
    /// linearly-interpolated fraction into the next period. From it: SV(t) = ES − AT, SPI(t) = ES ÷ AT, and
    /// an independent time forecast IEAC(t) = PD ÷ SPI(t). Inputs are cumulative; AT is the elapsed time.
    /// </summary>
    public static EarnedScheduleResult EarnedSchedule(IReadOnlyList<(int period, double pv)> plan, double evNow, double atNow, double? plannedDuration)
    {
        var pts = plan.OrderBy(p => p.period).ToList();
        double es;
        if (pts.Count == 0) es = 0;
        else if (evNow <= pts[0].pv)
            es = pts[0].pv <= 0 ? 0 : pts[0].period * (evNow / pts[0].pv);   // fractional first period
        else
        {
            var i = 0;
            while (i + 1 < pts.Count && pts[i + 1].pv <= evNow) i++;
            es = pts[i].period;
            if (i + 1 < pts.Count && pts[i + 1].pv > pts[i].pv)
                es += (evNow - pts[i].pv) / (pts[i + 1].pv - pts[i].pv) * (pts[i + 1].period - pts[i].period);
            // else the plan is fully earned — ES rests at the final planned period (project at/ahead of plan)
        }
        var spiT = atNow == 0 ? 0 : es / atNow;
        double? eacT = plannedDuration is double pd && spiT != 0 ? R(pd / spiT) : (double?)null;
        return new EarnedScheduleResult(R(es), R(es - atNow), R(spiT), eacT);
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────
    //  Answer resolution — the values a scenario can ask a student to compute
    // ─────────────────────────────────────────────────────────────────────────────────────────────

    /// <summary>Every task type the engine can compute — the single source of truth shared by the
    /// dispatcher below and the content validator (Core/SimContent). A scenario whose <c>task</c> is not in
    /// this set can never be graded, so publication must reject it. Keep in lockstep with the switch in
    /// <see cref="Resolve"/>.</summary>
    public static readonly IReadOnlySet<string> KnownTasks = new HashSet<string>(StringComparer.Ordinal)
    {
        "evm", "cbs", "progress", "risk", "pert", "change", "cashflow", "timeline", "earned_schedule", "cpm", "wbs",
    };

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
                switch (key)
                {
                    case "sv": return r.SV; case "cv": return r.CV; case "spi": return r.SPI; case "cpi": return r.CPI;
                    case "eac": return r.EAC; case "etc": return r.ETC; case "vac": return r.VAC; case "tcpi": return r.TCPI;
                    case "percent_complete": return r.PercentComplete; case "percent_spent": return r.PercentSpent;
                }
                // Forecasting keys share the EVM inputs; the three EAC methods come from Forecast().
                if (key is "eac_cpi" or "eac_composite" or "eac_budget")
                {
                    var f = Forecast(G("pv"), G("ev"), G("ac"), G("bac"));
                    return key switch { "eac_cpi" => (object?)f.EacCpi, "eac_composite" => f.EacComposite, "eac_budget" => f.EacBudget, _ => null };
                }
                return null;
            }
            case "cbs":
            {
                var r = Cbs(ParseCbs(given));
                if (key == "root_budget") return r.TotalBudget;
                if (key == "root_actual") return r.TotalActual;
                if (key == "root_variance") return r.Variance;
                if (key.StartsWith("variance_", StringComparison.Ordinal))
                {
                    var id = key.Substring("variance_".Length);
                    return r.Nodes.FirstOrDefault(n => n.Id == id)?.Variance;
                }
                return null;
            }
            case "progress":
            {
                var r = Progress(ParseProgress(given));
                return key switch { "overall_percent" => (object?)r.OverallPercent, "total_weight" => r.TotalWeight, _ => null };
            }
            case "risk":
            {
                var r = Emv(ParseRisks(given));
                if (key == "emv") return r.Total;
                if (key.StartsWith("emv_", StringComparison.Ordinal))
                {
                    var id = key.Substring("emv_".Length);
                    var hit = r.Items.FirstOrDefault(i => i.id == id);
                    return hit.id == id ? hit.emv : (object?)null;
                }
                return null;
            }
            case "pert":
            {
                var acts = ParsePert(given);
                var deadline = GN("deadline");
                var r = PertPath(acts, deadline);
                return key switch
                {
                    "expected_duration" => (object?)r.Expected,
                    "std_dev" => r.StdDev,
                    "variance" => r.Variance,
                    "prob_on_time" => r.ProbOnTimePercent,
                    _ => null,
                };
            }
            case "change":
            {
                var r = ChangeControl(G("baseline_bac"), G("baseline_duration"), ParseChanges(given));
                return key switch
                {
                    "revised_bac" => (object?)r.RevisedBac,
                    "revised_duration" => r.RevisedDuration,
                    "approved_cost_delta" => r.ApprovedCostDelta,
                    "approved_schedule_delta" => r.ApprovedScheduleDelta,
                    "approved_count" => (double)r.ApprovedCount,
                    _ => null,
                };
            }
            case "cashflow":
            {
                var r = CashFlow(ParseCash(given));
                if (key == "final_position") return r.FinalPosition;
                if (key == "peak_funding") return r.PeakFunding;
                if (key.StartsWith("cumulative_", StringComparison.Ordinal) && int.TryParse(key.Substring("cumulative_".Length), out var per))
                    return r.Series.FirstOrDefault(s => s.Period == per)?.Cumulative;
                return null;
            }
            case "timeline":
            {
                var (pts, bac) = ParseTimeline(given);
                var r = EvmTimeline(pts, bac);
                return key switch
                {
                    "final_ev" => (object?)r.FinalEv,
                    "final_ac" => r.FinalAc,
                    "final_spi" => r.FinalSpi,
                    "final_cpi" => r.FinalCpi,
                    "final_eac" => r.FinalEac,
                    "vac" => r.Vac,
                    "worst_spi" => r.WorstSpi,
                    "worst_spi_period" => (double)r.WorstSpiPeriod,
                    "worst_cpi" => r.WorstCpi,
                    "worst_cpi_period" => (double)r.WorstCpiPeriod,
                    _ => null,
                };
            }
            case "earned_schedule":
            {
                var plan = ParsePlan(given);
                var r = EarnedSchedule(plan, G("ev"), G("at"), GN("planned_duration"));
                return key switch
                {
                    "es" => (object?)r.Es,
                    "spi_time" => r.SpiTime,
                    "sv_time" => r.SvTime,
                    "eac_time" => r.EacTime,
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

    /// <summary>Run the schedule (forward/backward pass) directly from a scenario's <c>given</c> block —
    /// used to render the Gantt in the result view once an attempt is graded (never before).</summary>
    public static CpmResult ScheduleFrom(JsonElement given) => CriticalPath(ParseCpm(given));

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

    static List<ChangeItem> ParseChanges(JsonElement given)
    {
        var list = new List<ChangeItem>();
        if (given.TryGetProperty("changes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                var status = n.TryGetProperty("status", out var s) ? s.GetString() ?? "" : "";
                double G(string k) => n.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
                list.Add(new ChangeItem(id, status, G("cost_delta"), G("schedule_delta")));
            }
        return list;
    }

    static List<CashPeriod> ParseCash(JsonElement given)
    {
        var list = new List<CashPeriod>();
        if (given.TryGetProperty("periods", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var per = n.TryGetProperty("period", out var pv) && pv.ValueKind == JsonValueKind.Number ? pv.GetInt32() : 0;
                double G(string k) => n.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
                list.Add(new CashPeriod(per, G("inflow"), G("outflow")));
            }
        return list;
    }

    static List<(int period, double pv)> ParsePlan(JsonElement given)
    {
        var list = new List<(int, double)>();
        if (given.TryGetProperty("plan", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var per = n.TryGetProperty("period", out var pd) && pd.ValueKind == JsonValueKind.Number ? pd.GetInt32() : 0;
                var pv = n.TryGetProperty("pv", out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
                list.Add((per, pv));
            }
        return list;
    }

    static (List<TimelineInput> points, double bac) ParseTimeline(JsonElement given)
    {
        var list = new List<TimelineInput>();
        if (given.TryGetProperty("series", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var per = n.TryGetProperty("period", out var pd) && pd.ValueKind == JsonValueKind.Number ? pd.GetInt32() : 0;
                double G(string k) => n.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
                list.Add(new TimelineInput(per, G("pv"), G("ev"), G("ac")));
            }
        double bac = given.TryGetProperty("bac", out var b) && b.ValueKind == JsonValueKind.Number ? b.GetDouble() : 0;
        return (list, bac);
    }

    static List<RiskItem> ParseRisks(JsonElement given)
    {
        var list = new List<RiskItem>();
        if (given.TryGetProperty("risks", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                var prob = n.TryGetProperty("probability", out var p) && p.ValueKind == JsonValueKind.Number ? p.GetDouble() : 0;
                var impact = n.TryGetProperty("impact", out var im) && im.ValueKind == JsonValueKind.Number ? im.GetDouble() : 0;
                list.Add(new RiskItem(id, prob, impact));
            }
        return list;
    }

    /// <summary>Run the Monte-Carlo directly from a scenario's <c>given</c> (activities with O/M/P/preds,
    /// plus <c>seed</c> and <c>iterations</c>) — used to attach the distribution to a schedule-risk task.
    /// Returns null when the block is absent so callers can omit the dashboard.</summary>
    public static McResult? MonteCarloFrom(JsonElement given)
    {
        if (!given.TryGetProperty("activities", out var arr) || arr.ValueKind != JsonValueKind.Array) return null;
        if (!given.TryGetProperty("seed", out var sd) || sd.ValueKind != JsonValueKind.Number) return null;
        var seed = sd.GetInt32();
        var iters = given.TryGetProperty("iterations", out var it) && it.ValueKind == JsonValueKind.Number ? it.GetInt32() : 2000;
        var acts = new List<McActivity>();
        foreach (var n in arr.EnumerateArray())
        {
            var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
            double G(string k) => n.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
            var preds = new List<string>();
            if (n.TryGetProperty("preds", out var pe) && pe.ValueKind == JsonValueKind.Array)
                foreach (var p in pe.EnumerateArray()) if (p.GetString() is { } s) preds.Add(s);
            acts.Add(new McActivity(id, G("o"), G("m"), G("p"), preds));
        }
        return acts.Count == 0 ? null : MonteCarlo(acts, seed, iters);
    }

    static List<(double o, double m, double p)> ParsePert(JsonElement given)
    {
        var list = new List<(double, double, double)>();
        if (given.TryGetProperty("activities", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                double G(string k) => n.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;
                list.Add((G("o"), G("m"), G("p")));
            }
        return list;
    }

    static List<ProgressInputNode> ParseProgress(JsonElement given)
    {
        var list = new List<ProgressInputNode>();
        if (given.TryGetProperty("nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                double? weight = n.TryGetProperty("weight", out var w) && w.ValueKind == JsonValueKind.Number ? w.GetDouble() : (double?)null;
                double? pct = n.TryGetProperty("percent", out var p) && p.ValueKind == JsonValueKind.Number ? p.GetDouble() : (double?)null;
                list.Add(new ProgressInputNode(id, weight, pct));
            }
        return list;
    }

    static List<CbsInputNode> ParseCbs(JsonElement given)
    {
        var list = new List<CbsInputNode>();
        if (given.TryGetProperty("nodes", out var arr) && arr.ValueKind == JsonValueKind.Array)
            foreach (var n in arr.EnumerateArray())
            {
                var id = n.TryGetProperty("id", out var i) ? i.GetString() ?? "" : "";
                string? parent = n.TryGetProperty("parent", out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;
                double? budget = n.TryGetProperty("budget", out var b) && b.ValueKind == JsonValueKind.Number ? b.GetDouble() : (double?)null;
                double? actual = n.TryGetProperty("actual", out var a) && a.ValueKind == JsonValueKind.Number ? a.GetDouble() : (double?)null;
                list.Add(new CbsInputNode(id, parent, budget, actual));
            }
        return list;
    }
}
