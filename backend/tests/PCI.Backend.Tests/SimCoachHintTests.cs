using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class SimCoachHintTests
{
    static JsonElement Config() => JsonDocument.Parse("""
        {"task":"evm",
         "prompt":"Compute the earned-value measures.",
         "given":{"pv":100000,"ev":90000,"ac":95000,"bac":200000},
         "ask":[
           {"key":"spi","label":"Schedule Performance Index (SPI)","type":"number"},
           {"key":"cpi","label":"Cost Performance Index (CPI)","type":"number"}],
         "tolerance":0.01,"pass_pct":70,"competencies":["earned_value"]}
        """).RootElement;

    [Fact]
    public void HintLadder_LevelsOneToFive_DoNotRevealCorrectValue()
    {
        for (var level = 1; level <= 5; level++)
        {
            var msg = SimCoach.HintLadder("evm", "cpi", "Cost Performance Index (CPI)", 0.9474, level, allowReveal: false);
            Assert.DoesNotContain("0.95", msg);
            Assert.DoesNotContain("0.9474", msg);
            Assert.DoesNotContain("The correct value is", msg);
        }
    }

    [Fact]
    public void HintLadder_LevelSix_RevealsOnlyWhenAllowed()
    {
        var withheld = SimCoach.HintLadder("evm", "cpi", "CPI", 0.9474, 6, allowReveal: false);
        Assert.Contains("stays hidden", withheld);

        var revealed = SimCoach.HintLadder("evm", "cpi", "CPI", 0.9474, 6, allowReveal: true);
        Assert.Contains("0.95", revealed);
    }

    [Fact]
    public void DeterministicCoach_GuidedMode_UsesHintLadderWithoutRevealBeforeSubmit()
    {
        var config = Config();
        var grade = SimGrade.Grade(config, JsonDocument.Parse("{\"spi\":0.9,\"cpi\":2.0}").RootElement, "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null, "guided", 2, allowReveal: false);
        Assert.Contains("concept", msg.ToLowerInvariant());
        Assert.DoesNotContain("The correct value is", msg);
    }

    [Fact]
    public void Modes_AreRecognised()
    {
        Assert.Contains("socratic", SimCoach.Modes);
        Assert.Contains("language", SimCoach.Modes);
        Assert.Equal("explain", SimCoach.NormMode("nope"));
        Assert.Equal("debrief", SimCoach.NormMode("Debrief"));
    }
}
