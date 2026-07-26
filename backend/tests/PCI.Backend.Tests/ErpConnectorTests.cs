using System.Text.Json;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// Zoho Books (RES-008) and Odoo (RES-009) connector contracts.
///
/// These pin the parts that are pure and therefore testable without a vendor: regional routing, the
/// canonical→vendor object mapping, the JSON-RPC envelope shape, and — most importantly — the fault
/// detection that stops an Odoo error being recorded as a successful delivery. The live request paths
/// (OAuth refresh, partner/contact resolution) need an approved sandbox and are covered by the
/// end-to-end integration suite's mock vendors.
/// </summary>
public class ErpConnectorTests
{
    // ---------------------------------------------------------------- Zoho: regional routing

    [Theory]
    [InlineData("com", "com")]
    [InlineData("us", "com")]
    [InlineData("eu", "eu")]
    [InlineData("in", "in")]
    [InlineData("au", "com.au")]
    [InlineData("com.au", "com.au")]
    [InlineData("jp", "jp")]
    [InlineData("ca", "ca")]
    public void Zoho_RegionNormalisesToItsDataCentre(string input, string expected) =>
        Assert.Equal(expected, ZohoConnector.NormaliseRegion(input));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-region")]
    public void Zoho_UnknownRegionFallsBackToCom(string? input) =>
        Assert.Equal("com", ZohoConnector.NormaliseRegion(input));

    [Fact]
    public void Zoho_ApiAndAccountsHostsFollowTheRegion()
    {
        // A token minted in one Zoho data centre is invalid in another, so both hosts must move together.
        Assert.Equal("https://www.zohoapis.eu", ZohoConnector.ApiBase("eu", null));
        Assert.Equal("https://accounts.zoho.eu", ZohoConnector.AccountsBase("eu"));
        Assert.Equal("https://www.zohoapis.com.au", ZohoConnector.ApiBase("com.au", null));
        Assert.Equal("https://accounts.zoho.com.au", ZohoConnector.AccountsBase("com.au"));
    }

    [Fact]
    public void Zoho_ApiBaseOverrideWinsButAccountsHostDoesNot()
    {
        // The override exists to point deliveries at a test receiver; tokens still come from Zoho itself,
        // so redirecting the accounts host would send real OAuth credentials to that receiver.
        Assert.Equal("https://receiver.test", ZohoConnector.ApiBase("com", "https://receiver.test/"));
        Assert.Equal("https://accounts.zoho.com", ZohoConnector.AccountsBase("com"));
    }

    // ---------------------------------------------------------------- Zoho: mapping

    [Fact]
    public void Zoho_MapsOnlyTheEventsBooksCanRepresent()
    {
        Assert.Equal("contacts", ZohoConnector.ResourceFor("member.registered"));
        Assert.Equal("invoices", ZohoConnector.ResourceFor("payment.recorded"));
        // A membership activation is a CRM state, not an accounting entry — skipped, never a failure.
        Assert.Null(ZohoConnector.ResourceFor("membership.activated"));
        Assert.Null(ZohoConnector.ResourceFor("something.unknown"));
    }

    [Fact]
    public void Zoho_ContactCarriesDisplayNameEmailAndPrimaryPerson()
    {
        var e = JsonDocument.Parse(ZohoConnector.ContactBody("Ada", "Lovelace", "ada@pci.test")).RootElement;
        Assert.Equal("Ada Lovelace", e.GetProperty("contact_name").GetString());
        Assert.Equal("customer", e.GetProperty("contact_type").GetString());
        Assert.Equal("ada@pci.test", e.GetProperty("email").GetString());
        var person = e.GetProperty("contact_persons")[0];
        Assert.Equal("Ada", person.GetProperty("first_name").GetString());
        Assert.True(person.GetProperty("is_primary_contact").GetBoolean());
    }

    [Fact]
    public void Zoho_ContactFallsBackToEmailThenAPlaceholderWhenUnnamed()
    {
        var byEmail = JsonDocument.Parse(ZohoConnector.ContactBody("", "", "noname@pci.test")).RootElement;
        Assert.Equal("noname@pci.test", byEmail.GetProperty("contact_name").GetString());
        // Books rejects a contact with no name, so an anonymous event still gets a usable one.
        var placeholder = JsonDocument.Parse(ZohoConnector.ContactBody("", "", "")).RootElement;
        Assert.Equal("PCI Member", placeholder.GetProperty("contact_name").GetString());
    }

    [Fact]
    public void Zoho_InvoiceBindsToTheContactAndRoundsMoneyToCents()
    {
        var e = JsonDocument.Parse(ZohoConnector.InvoiceBody("461000000012345", 349.999, "Exam fee", "WAIVE-AB12")).RootElement;
        Assert.Equal("461000000012345", e.GetProperty("customer_id").GetString());
        Assert.Equal("WAIVE-AB12", e.GetProperty("reference_number").GetString());
        var line = e.GetProperty("line_items")[0];
        Assert.Equal("Exam fee", line.GetProperty("name").GetString());
        Assert.Equal(350.00, line.GetProperty("rate").GetDouble(), 2);
        Assert.Equal(1, line.GetProperty("quantity").GetInt32());
    }

    [Fact]
    public void Zoho_InvoiceOmitsReferenceWhenThereIsNone()
    {
        var e = JsonDocument.Parse(ZohoConnector.InvoiceBody("42", 99, "Membership", null)).RootElement;
        Assert.False(e.TryGetProperty("reference_number", out _));
        Assert.Equal("Membership", e.GetProperty("line_items")[0].GetProperty("name").GetString());
    }

    // ---------------------------------------------------------------- Odoo: mapping

    [Fact]
    public void Odoo_MapsOnlyTheEventsOdooCanRepresent()
    {
        Assert.Equal("res.partner", OdooConnector.ModelFor("member.registered"));
        Assert.Equal("account.move", OdooConnector.ModelFor("payment.recorded"));
        Assert.Null(OdooConnector.ModelFor("membership.activated"));
    }

    [Fact]
    public void Odoo_PartnerIsMarkedAsACustomer()
    {
        var vals = OdooConnector.PartnerValues("Grace", "Hopper", "grace@pci.test");
        Assert.Equal("Grace Hopper", vals["name"]);
        Assert.Equal("grace@pci.test", vals["email"]);
        // customer_rank drives Odoo's customer filters; without it the partner is invisible in sales.
        Assert.Equal(1, vals["customer_rank"]);
    }

    [Fact]
    public void Odoo_PartnerFallsBackToEmailThenAPlaceholder()
    {
        Assert.Equal("solo@pci.test", OdooConnector.PartnerValues("", "", "solo@pci.test")["name"]);
        Assert.Equal("PCI Member", OdooConnector.PartnerValues("", "", "")["name"]);
    }

    [Fact]
    public void Odoo_InvoiceUsesTheOne2ManyCreateTriple()
    {
        var vals = OdooConnector.InvoiceValues(77, 12.3456, "Retake fee", "REF-9");
        Assert.Equal("out_invoice", vals["move_type"]);
        Assert.Equal(77L, vals["partner_id"]);
        Assert.Equal("REF-9", vals["ref"]);
        // Odoo writes inline lines as (0, 0, values); anything else silently creates no line.
        var lines = Assert.IsType<object[]>(vals["invoice_line_ids"]);
        var triple = Assert.IsType<object[]>(lines[0]);
        Assert.Equal(0, triple[0]);
        Assert.Equal(0, triple[1]);
        var line = Assert.IsType<Dictionary<string, object?>>(triple[2]);
        Assert.Equal("Retake fee", line["name"]);
        Assert.Equal(12.35, (double)line["price_unit"]!, 2);
    }

    [Fact]
    public void ErpMoney_UsesDotNetMidpointRoundingLikeTheQuickBooksConnector()
    {
        // Documented, not accidental: Math.Round(x, 2) is banker's rounding (to-even), so an exact
        // half-cent goes to the even cent — 12.345 → 12.34, not 12.35. Both new connectors match the
        // existing QuickBooks connector rather than introducing a second money convention. In practice
        // the case is unreachable: payments.final_amount is already cent-precision (DECIMAL(12,2)), so
        // a half-cent never reaches a connector. This test exists so the next reader knows which it is.
        var odoo = OdooConnector.InvoiceValues(1, 12.345, "x", null);
        var line = (Dictionary<string, object?>)((object[])((object[])odoo["invoice_line_ids"]!)[0])[2]!;
        Assert.Equal(12.34, (double)line["price_unit"]!, 3);

        var zoho = JsonDocument.Parse(ZohoConnector.InvoiceBody("1", 12.345, "x", null)).RootElement;
        Assert.Equal(12.34, zoho.GetProperty("line_items")[0].GetProperty("rate").GetDouble(), 3);
    }

    [Fact]
    public void Odoo_ExecuteKwEnvelopeMatchesTheJsonRpcContract()
    {
        var body = OdooConnector.ExecuteKwBody("pcidb", 7, "key-123", "res.partner", "create",
            new object[] { new Dictionary<string, object?> { ["name"] = "X" } });
        var e = JsonDocument.Parse(body).RootElement;
        Assert.Equal("2.0", e.GetProperty("jsonrpc").GetString());
        Assert.Equal("call", e.GetProperty("method").GetString());
        var p = e.GetProperty("params");
        Assert.Equal("object", p.GetProperty("service").GetString());
        Assert.Equal("execute_kw", p.GetProperty("method").GetString());
        var args = p.GetProperty("args");
        Assert.Equal("pcidb", args[0].GetString());
        Assert.Equal(7, args[1].GetInt32());
        Assert.Equal("key-123", args[2].GetString());
        Assert.Equal("res.partner", args[3].GetString());
        Assert.Equal("create", args[4].GetString());
    }

    // ---------------------------------------------------------------- Odoo: fault detection

    [Fact]
    public void Odoo_SuccessfulReplyIsNotAProblem()
    {
        Assert.Null(OdooConnector.ResponseProblem("{\"jsonrpc\":\"2.0\",\"id\":1,\"result\":42}"));
        Assert.Null(OdooConnector.ResponseProblem("{\"jsonrpc\":\"2.0\",\"id\":1,\"result\":[7]}"));
    }

    [Fact]
    public void Odoo_FaultIsDetectedEvenThoughItArrivesAsHttp200()
    {
        // This is the whole point: Odoo answers failures with HTTP 200 and an "error" member, so a
        // status-code-only check would record a delivery that never happened.
        var problem = OdooConnector.ResponseProblem(
            "{\"jsonrpc\":\"2.0\",\"id\":1,\"error\":{\"code\":200,\"message\":\"Odoo Server Error\"," +
            "\"data\":{\"message\":\"Invalid field 'nope' on model 'res.partner'\"}}}");
        Assert.NotNull(problem);
        Assert.Contains("Invalid field", problem);
    }

    [Fact]
    public void Odoo_FaultWithoutNestedDataStillReportsTheGenericMessage()
    {
        var problem = OdooConnector.ResponseProblem("{\"error\":{\"message\":\"Access Denied\"}}");
        Assert.Equal("Access Denied", problem);
    }

    [Fact]
    public void Odoo_FailedAuthenticationReturnsResultFalseAndIsTreatedAsAFault()
    {
        // authenticate answers result:false for bad credentials — a fault in everything but name.
        var problem = OdooConnector.ResponseProblem("{\"jsonrpc\":\"2.0\",\"id\":1,\"result\":false}");
        Assert.NotNull(problem);
        Assert.Contains("credentials", problem);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Odoo_EmptyBodyIsAProblemNotASuccess(string? body) =>
        Assert.NotNull(OdooConnector.ResponseProblem(body));

    [Fact]
    public void Odoo_NonJsonBodyIsAProblemNotASuccess() =>
        Assert.NotNull(OdooConnector.ResponseProblem("<html>502 Bad Gateway</html>"));
}
