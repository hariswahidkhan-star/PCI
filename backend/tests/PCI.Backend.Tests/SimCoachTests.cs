using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Unit coverage of the Simulation Lab AI Coach's deterministic (no-provider) path and its grounding
/// contract. The coach must (a) explain a wrong measure with its concept and correct value in Training
/// Mode, (b) only ever cite numbers the deterministic engine produced, and (c) never fabricate. These
/// tests exercise the pure helpers directly; the Assessment-Mode refusal is covered by the live suite
/// (it needs a Db for the settings flag).
/// </summary>
public class SimCoachTests
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

    static JsonElement Answers(string json) => JsonDocument.Parse(json).RootElement;

    [Fact]
    public void DeterministicCoach_ExplainsAWrongMeasureWithItsCorrectValue()
    {
        var config = Config();
        // SPI right (0.9), CPI wrong.
        var grade = SimGrade.Grade(config, Answers("{\"spi\":0.9,\"cpi\":2.0}"), "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null);

        Assert.Contains("Cost Performance Index", msg);       // names the missed measure
        Assert.Contains("0.95", msg);                         // cites the ENGINE's correct CPI (0.9474→0.95)
        Assert.Contains("50%", msg);                          // 1 of 2 correct
    }

    [Fact]
    public void DeterministicCoach_CongratulatesAndInterpretsAPerfectScore()
    {
        var config = Config();
        var grade = SimGrade.Grade(config, Answers("{\"spi\":0.9,\"cpi\":0.9474}"), "training");
        var msg = SimCoach.DeterministicCoach(config, grade, null);

        Assert.Contains("100%", msg);
        // Both indices below 1 → behind schedule + over budget interpretation.
        Assert.Contains("behind schedule", msg);
        Assert.Contains("over budget", msg);
    }

    [Fact]
    public void BuildContext_GroundsOnlyInProvidedFiguresAndFlagsWrongAnswers()
    {
        var config = Config();
        var grade = SimGrade.Grade(config, Answers("{\"spi\":0.9,\"cpi\":2.0}"), "training");
        var ctx = SimCoach.BuildContext(config, grade, "why is my CPI off?");

        Assert.Contains("do not recompute", ctx);             // instruction to the model to stay grounded
        Assert.Contains("WRONG", ctx);                        // the missed measure is flagged
        Assert.Contains("why is my CPI off?", ctx);           // the student's question is passed through
        Assert.Contains("synthetic", ctx);                    // given data labelled synthetic
    }
}
