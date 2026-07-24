using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — scenario manifest export (§5B.4). The manifest exists to be archived, diffed and
/// checksum-compared, so these tests pin the three properties that make it worth trusting: it is
/// deterministic, its checksum tracks graded content and nothing else, and it carries no identities.
/// </summary>
public class SimManifestTests
{
    // A representative published row, including every governance column the exporter reads.
    static Dictionary<string, object?> Row(params (string Col, object? Val)[] overrides)
    {
        var r = new Dictionary<string, object?>
        {
            ["id"] = 42L,
            ["scenario_code"] = "GL-EVM-010",
            ["title"] = "Calculate the core EVM measures",
            ["kind"] = "guided_lab",
            ["industry"] = "construction",
            ["project_type"] = "capital",
            ["difficulty"] = "foundation",
            ["est_minutes"] = 15L,
            ["certification_id"] = 1L,
            ["competencies_json"] = "[\"earned_value\",\"forecasting\"]",
            ["summary"] = "Work the standard EVM set from a synthetic monthly cut.",
            ["brief"] = "You are the project controls lead.",
            ["objectives_json"] = "[\"Compute CPI and SPI\"]",
            ["provenance"] = "Authored from synthetic data.",
            ["disclaimers"] = "Practice only — not a formal exam record.",
            ["worked_solution"] = "CPI = EV / AC.",
            ["synthetic_declared"] = 1L,
            ["config_json"] = "{\"task\":\"evm\",\"given\":{\"pv\":100000,\"ev\":90000,\"ac\":95000},\"ask\":[{\"key\":\"cpi\"}]}",
            ["status"] = "published",
            ["review_state"] = "published",
            ["version"] = 3L,
            ["authored_by"] = 987654321L,
            ["calc_reviewed_by"] = 987654322L,
            ["learning_reviewed_by"] = 987654323L,
            ["safety_reviewed_by"] = null,
            ["approved_by"] = 987654324L,
            ["approved_at"] = "2026-03-01 10:00:00",
            ["published_at"] = "2026-03-02 09:30:00",
            ["review_due"] = "2027-03-01",
            ["expires_at"] = "2028-03-01",
        };
        foreach (var (col, val) in overrides) r[col] = val;
        return r;
    }

    static readonly List<SimContent.Issue> NoIssues = new();

    [Fact]
    public void Build_IsDeterministic_AcrossRepeatedExports()
    {
        // Two exports of the same content must be byte-identical — otherwise the file cannot be diffed
        // or compared between environments, which is the only reason to have a manifest.
        Assert.Equal(SimManifest.Build(Row(), NoIssues), SimManifest.Build(Row(), NoIssues));
    }

    [Fact]
    public void Build_EmitsWellFormedManifestEnvelope()
    {
        using var doc = JsonDocument.Parse(SimManifest.Build(Row(), NoIssues));
        var root = doc.RootElement;
        Assert.Equal(SimManifest.ManifestVersion, root.GetProperty("manifest_version").GetInt32());
        Assert.Equal(SimManifest.Kind, root.GetProperty("kind").GetString());
        Assert.Equal(SimManifest.Checksum(Row()), root.GetProperty("checksum").GetString());

        var scenario = root.GetProperty("scenario");
        Assert.Equal("GL-EVM-010", scenario.GetProperty("scenario_code").GetString());
        Assert.Equal(3, scenario.GetProperty("version").GetInt32());
        Assert.True(scenario.GetProperty("synthetic_declared").GetBoolean());
        // Stored JSON columns are re-parsed, so the manifest carries real JSON — not a quoted blob.
        Assert.Equal(JsonValueKind.Object, scenario.GetProperty("config").ValueKind);
        Assert.Equal(JsonValueKind.Array, scenario.GetProperty("competencies").ValueKind);
        Assert.Equal("evm", scenario.GetProperty("config").GetProperty("task").GetString());

        var gov = root.GetProperty("governance");
        Assert.Equal("published", gov.GetProperty("review_state").GetString());
        Assert.Equal("2027-03-01", gov.GetProperty("review_due").GetString());
        var signed = gov.GetProperty("signed_off");
        Assert.True(signed.GetProperty("calc_review").GetBoolean());
        Assert.True(signed.GetProperty("approved").GetBoolean());
        Assert.False(signed.GetProperty("safety_review").GetBoolean());   // the only unsigned stage
    }

    [Fact]
    public void Checksum_TracksGradedContentOnly()
    {
        var baseline = SimManifest.Checksum(Row());

        // Governance dates may be re-set on a published scenario (metadata, not content — §18 does not
        // freeze them), so they must not move the fingerprint of a frozen version.
        Assert.Equal(baseline, SimManifest.Checksum(Row(("review_due", "2030-01-01"), ("expires_at", "2031-01-01"))));
        Assert.Equal(baseline, SimManifest.Checksum(Row(("approved_by", 5L), ("published_at", "2026-12-25 00:00:00"))));

        // Anything the engine grades against must move it.
        Assert.NotEqual(baseline, SimManifest.Checksum(Row(("title", "A different title"))));
        Assert.NotEqual(baseline, SimManifest.Checksum(Row(("version", 4L))));
        Assert.NotEqual(baseline, SimManifest.Checksum(Row(("config_json", "{\"task\":\"cpm\"}"))));
        Assert.NotEqual(baseline, SimManifest.Checksum(Row(("competencies_json", "[\"scheduling\"]"))));
    }

    [Fact]
    public void Checksum_IgnoresInsignificantWhitespaceInStoredJson()
    {
        // The config column is re-serialised through the parser, so reformatting the stored text alone
        // must not look like a content change.
        var pretty = "{\n  \"task\": \"evm\",\n  \"given\": {\"pv\": 100000, \"ev\": 90000, \"ac\": 95000},\n  \"ask\": [{\"key\": \"cpi\"}]\n}";
        Assert.Equal(SimManifest.Checksum(Row()), SimManifest.Checksum(Row(("config_json", pretty))));
    }

    [Fact]
    public void Build_NeverCarriesAdminIdentities()
    {
        var json = SimManifest.Build(Row(), NoIssues);
        foreach (var col in new[] { "authored_by", "created_by", "calc_reviewed_by", "learning_reviewed_by", "safety_reviewed_by", "approved_by" })
            Assert.DoesNotContain(col, json);
        // …nor the id values behind them, under any key.
        foreach (var id in new[] { "987654321", "987654322", "987654323", "987654324" })
            Assert.DoesNotContain(id, json);
    }

    [Fact]
    public void Build_OmitsStudentUsage_SoTheExportStaysStableWhileStudentsPractise()
    {
        // Attempt aggregates are deliberately absent: they would leak usage and would change the file on
        // every practice run, defeating checksum comparison.
        var json = SimManifest.Build(Row(), NoIssues);
        Assert.DoesNotContain("attempts", json);
        Assert.DoesNotContain("completed", json);
    }

    [Fact]
    public void Build_CarriesTheValidationVerdict()
    {
        var issues = new List<SimContent.Issue>
        {
            new(SimContent.Severity.Error, "missing_summary", "Add a summary."),
            new(SimContent.Severity.Warning, "short_brief", "The brief is thin."),
        };
        using var doc = JsonDocument.Parse(SimManifest.Build(Row(), issues));
        var v = doc.RootElement.GetProperty("validation");
        Assert.False(v.GetProperty("publishable").GetBoolean());
        Assert.Equal(1, v.GetProperty("errors").GetInt32());
        Assert.Equal(1, v.GetProperty("warnings").GetInt32());
        Assert.Equal("error", v.GetProperty("issues")[0].GetProperty("severity").GetString());
        Assert.Equal("missing_summary", v.GetProperty("issues")[0].GetProperty("code").GetString());

        using var clean = JsonDocument.Parse(SimManifest.Build(Row(), NoIssues));
        Assert.True(clean.RootElement.GetProperty("validation").GetProperty("publishable").GetBoolean());
    }

    [Fact]
    public void Build_ToleratesSparseRowsAndUnparseableJson()
    {
        // A freshly created draft has almost nothing filled in — export must still produce a valid document.
        var draft = new Dictionary<string, object?>
        {
            ["scenario_code"] = "GL-NEW-001",
            ["title"] = "Untitled draft",
            ["version"] = 1L,
        };
        using var doc = JsonDocument.Parse(SimManifest.Build(draft, NoIssues));
        var scenario = doc.RootElement.GetProperty("scenario");
        Assert.Equal(JsonValueKind.Null, scenario.GetProperty("config").ValueKind);
        Assert.Equal(JsonValueKind.Array, scenario.GetProperty("competencies").ValueKind);
        Assert.False(scenario.GetProperty("synthetic_declared").GetBoolean());
        Assert.Equal("draft", doc.RootElement.GetProperty("governance").GetProperty("review_state").GetString());

        // Corrupt JSON in a column is preserved as text rather than dropped — an export must not lose content.
        using var bad = JsonDocument.Parse(SimManifest.Build(Row(("config_json", "{not json")), NoIssues));
        var cfg = bad.RootElement.GetProperty("scenario").GetProperty("config");
        Assert.Equal(JsonValueKind.String, cfg.ValueKind);
        Assert.Equal("{not json", cfg.GetString());
    }

    [Theory]
    [InlineData("GL-EVM-010", 3, "GL-EVM-010-v3.pcisim.json")]
    [InlineData("gl_evm.010", 1, "gl_evm.010-v1.pcisim.json")]
    [InlineData("bad\"code", 2, "badcode-v2.pcisim.json")]              // quote stripped — no header forgery
    [InlineData("bad\r\nX-Injected: 1", 2, "badX-Injected1-v2.pcisim.json")]
    [InlineData("../../etc/passwd", 1, "....etcpasswd-v1.pcisim.json")] // no path separators survive
    [InlineData("", 1, "scenario-v1.pcisim.json")]
    [InlineData(null, 1, "scenario-v1.pcisim.json")]
    [InlineData("日本語", 1, "scenario-v1.pcisim.json")]                 // non-ASCII → safe fallback
    public void FileName_ReducesOperatorInputToASafeAlphabet(string? code, long version, string expected)
    {
        var name = SimManifest.FileName(code, version);
        Assert.Equal(expected, name);
        Assert.DoesNotContain('"', name);
        Assert.DoesNotContain('/', name);
        Assert.All(name, c => Assert.True(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.'));
    }

    [Fact]
    public void FileName_ClampsAnAbsurdlyLongCode()
    {
        var name = SimManifest.FileName(new string('A', 400), 9);
        Assert.Equal(new string('A', 64) + "-v9.pcisim.json", name);
    }
}
