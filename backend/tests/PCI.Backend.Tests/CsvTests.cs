using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// SEC-2 — regression coverage for the shared CSV field encoder (<see cref="Csv.Field"/>) that every
/// admin/partner CSV export now routes through. Pins both the RFC-4180 quoting and the formula-injection
/// (CWE-1236) neutralisation, so a crafted analytics/marketing value can never be exported as a live
/// spreadsheet formula, while genuine numbers stay numeric.
/// </summary>
public class CsvTests
{
    [Theory]
    [InlineData("=1+1")]
    [InlineData("=HYPERLINK(\"http://evil\",\"click\")")]
    [InlineData("+1+1")]
    [InlineData("@SUM(A1:A9)")]
    [InlineData("-cmd|' /c calc'!A1")]        // classic DDE payload — leading '-', not a number
    [InlineData("\tstartswithtab")]
    [InlineData("\rstartswithcr")]
    public void Field_NeutralisesFormulaTriggers_OnNonNumericText(string payload)
    {
        var outp = Csv.Field(payload);
        Assert.StartsWith("'", outp.StartsWith("\"") ? outp[1..] : outp);  // a single quote precedes the trigger
        // The neutralised value can never begin with a bare formula trigger once any RFC-4180 quote is peeled.
        var inner = outp.StartsWith("\"") ? outp[1..] : outp;
        Assert.Equal('\'', inner[0]);
    }

    [Theory]
    [InlineData("-5")]
    [InlineData("-3.14")]
    [InlineData("+42")]
    [InlineData("1000")]
    [InlineData("12.5")]
    public void Field_LeavesGenuineNumbersIntact(string number)
    {
        // Numeric columns (revenue, amounts) must not be corrupted with a leading apostrophe.
        Assert.Equal(number, Csv.Field(number));
    }

    [Theory]
    [InlineData("hello", "hello")]
    [InlineData("", "")]
    [InlineData("plain text no specials", "plain text no specials")]
    public void Field_PassesThroughOrdinaryText(string input, string expected)
        => Assert.Equal(expected, Csv.Field(input));

    [Fact]
    public void Field_QuotesCommasQuotesAndNewlines()
    {
        Assert.Equal("\"a,b\"", Csv.Field("a,b"));
        Assert.Equal("\"he said \"\"hi\"\"\"", Csv.Field("he said \"hi\""));
        Assert.Equal("\"line1\nline2\"", Csv.Field("line1\nline2"));
    }

    [Fact]
    public void Field_NeutralisesAndQuotes_WhenTriggerValueAlsoContainsAComma()
    {
        // A payload that is both a formula trigger AND contains a delimiter is guarded then quoted.
        Assert.Equal("\"'=1+1,2\"", Csv.Field("=1+1,2"));
    }

    [Fact]
    public void Field_NullBecomesEmpty()
        => Assert.Equal("", Csv.Field(null));
}
