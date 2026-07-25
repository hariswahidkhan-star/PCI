using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Simulation Lab — authoring-template tests. The Admin Studio offers these skeletons as the starting point
/// for a new scenario, so a template that does not itself pass the §14 publication gate would hand every
/// author a broken draft. Each one is run through the real <see cref="SimContent.Validate"/> — the same
/// reference-solver check the publish gate applies — and every engine in
/// <see cref="SimCalc.KnownTasks"/> must have a starter, so adding an engine without authoring guidance
/// fails here rather than silently leaving a blank textarea in the Studio.
/// </summary>
public class SimTemplatesTests
{
    [Fact]
    public void Every_engine_task_has_exactly_one_authoring_template()
    {
        var byTask = SimTemplates.All.GroupBy(t => t.Task).ToDictionary(g => g.Key, g => g.Count());
        foreach (var task in SimCalc.KnownTasks)
            Assert.True(byTask.ContainsKey(task), $"engine '{task}' has no authoring template in SimTemplates");
        foreach (var (task, count) in byTask)
        {
            Assert.True(SimCalc.KnownTasks.Contains(task), $"template names unknown engine '{task}'");
            Assert.True(count == 1, $"engine '{task}' has {count} templates; expected exactly one");
        }
    }

    [Fact]
    public void Every_template_passes_the_publication_validator()
    {
        foreach (var t in SimTemplates.All)
        {
            var issues = SimContent.Validate(new SimContent.ScenarioInput(
                $"TPL-{t.Task}", t.Label, t.Hint, "foundation", 1,
                "[\"earned_value\"]", t.Config, SyntheticDeclared: true));
            var errors = issues.Where(i => i.Severity == SimContent.Severity.Error).ToList();
            Assert.True(errors.Count == 0,
                $"template '{t.Task}' is not publishable: {string.Join("; ", errors.Select(e => $"{e.Code}: {e.Message}"))}");
        }
    }

    [Fact]
    public void Every_template_declares_the_engine_it_is_filed_under()
    {
        foreach (var t in SimTemplates.All)
        {
            using var doc = System.Text.Json.JsonDocument.Parse(t.Config);
            Assert.True(doc.RootElement.TryGetProperty("task", out var task), $"{t.Task}: config has no task");
            Assert.Equal(t.Task, task.GetString());
            Assert.False(string.IsNullOrWhiteSpace(t.Label), $"{t.Task}: label");
            Assert.False(string.IsNullOrWhiteSpace(t.Hint), $"{t.Task}: hint");
        }
    }

    [Fact]
    public void Lookup_finds_a_known_engine_and_rejects_an_unknown_one()
    {
        Assert.NotNull(SimTemplates.For("evm"));
        Assert.Equal("evm", SimTemplates.For("evm")!.Task);
        Assert.Null(SimTemplates.For("no_such_engine"));
    }
}
