using System.Text;
using System.Text.Json;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Odoo connector (RES-009). Plugs into the same integration pipeline as the generic webhook, QuickBooks
/// and Zoho connectors — outbox, delivery ledger, retry/backoff, dispatcher and egress guard unchanged;
/// this class only adds Odoo's JSON-RPC transport, authentication and canonical→Odoo mapping.
///
///   member.registered    → res.partner  create (name, email, customer_rank)
///   payment.recorded     → account.move create (out_invoice with one invoice line, against the partner)
///   membership.activated → (no accounting object — skipped, as for QuickBooks and Zoho)
///
/// Transport: every call is a POST to {url}/jsonrpc. Authentication exchanges the database name, login and
/// API key for a uid (`common.authenticate`); subsequent writes are `object.execute_kw` calls carrying that
/// uid and the same API key. An invoice needs its partner, so a payment delivery resolves the partner by
/// email (search_read) and creates one when absent — the same steps an operator would take by hand.
///
/// IMPORTANT — JSON-RPC faults are HTTP 200. Odoo answers a failed call with status 200 and an "error"
/// member in the body, so a transport-level success check alone would record a delivery that never
/// happened. <see cref="ResponseProblem"/> inspects the body and the delivery path fails the delivery when
/// a fault is present; that is why this connector cannot rely on the generic status-code handling.
/// </summary>
public static class OdooConnector
{
    public const string PartnerModel = "res.partner";
    public const string InvoiceModel = "account.move";

    sealed record Config(
        string url, string database, string login, string? partnerModel, string? invoiceModel,
        // ---- optional accounting mapping (all omitted from the write when unset, so Odoo applies
        // the company's own defaults rather than being overridden with a blank) ----
        long? companyId,           // res.company for a multi-company database
        long? journalId,           // account.journal the invoice is posted to
        long? productId,           // product.product the invoice line bills
        long? accountId,           // account.account override for the line
        long? taxId,               // account.tax applied to the line
        long? currencyId,          // res.currency when it differs from the company default
        long? paymentTermId,       // account.payment.term
        long? teamId,              // crm.team for revenue attribution
        bool partnerReuse);        // reuse an existing partner by email (default) vs always create
    //
    // NOTE — there is deliberately no "post the invoice" option. Posting is a SECOND RPC
    // (account.move.action_post) after the create returns an id, and a delivery here is exactly one
    // request whose status is the delivery's outcome. A setting that silently left every invoice in
    // draft would be worse than not offering it, so invoices are created as drafts and an accountant
    // posts them in Odoo — which is also the safer accounting default.

    static Config ParseConfig(string? json)
    {
        string G(JsonElement e, string k) => e.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        static long? Id(JsonElement e, string k)
        {
            if (!e.TryGetProperty(k, out var v)) return null;
            if (v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var n) && n > 0) return n;
            if (v.ValueKind == JsonValueKind.String && long.TryParse(v.GetString(), out var s) && s > 0) return s;
            return null;
        }
        try
        {
            var e = JsonDocument.Parse(json ?? "{}").RootElement;
            var reuse = !(e.TryGetProperty("partner_reuse", out var pr) && pr.ValueKind == JsonValueKind.False);
            return new Config(G(e, "url").TrimEnd('/'), G(e, "database"), G(e, "login"),
                G(e, "partner_model") is { Length: > 0 } pm ? pm : null,
                G(e, "invoice_model") is { Length: > 0 } im ? im : null,
                Id(e, "company_id"), Id(e, "journal_id"), Id(e, "product_id"), Id(e, "account_id"),
                Id(e, "tax_id"), Id(e, "currency_id"), Id(e, "payment_term_id"), Id(e, "team_id"),
                reuse);
        }
        catch { return new Config("", "", "", null, null, null, null, null, null, null, null, null, null, true); }
    }

    static string? ParseApiKey(string? json)
    {
        var plain = Security.DecryptSecret(json) ?? json;
        if (string.IsNullOrWhiteSpace(plain)) return null;
        try
        {
            var e = JsonDocument.Parse(plain).RootElement;
            if (e.ValueKind == JsonValueKind.Object && e.TryGetProperty("api_key", out var v)
                && v.ValueKind == JsonValueKind.String && v.GetString() is { Length: > 0 } s) return s;
        }
        catch { /* a bare (non-JSON) key is also accepted */ }
        return plain.Trim() is { Length: > 0 } bare && !bare.StartsWith("{") ? bare : null;
    }

    /// <summary>A JSON-RPC envelope for `object.execute_kw`.</summary>
    public static string ExecuteKwBody(string database, long uid, string apiKey, string model, string method, object args, object? kwargs = null) =>
        JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["jsonrpc"] = "2.0",
            ["method"] = "call",
            ["params"] = new Dictionary<string, object?>
            {
                ["service"] = "object",
                ["method"] = "execute_kw",
                ["args"] = new object[] { database, uid, apiKey, model, method, args, kwargs ?? new Dictionary<string, object?>() },
            },
            ["id"] = 1,
        });

    /// <summary>The res.partner values for a member.</summary>
    public static Dictionary<string, object?> PartnerValues(string firstName, string lastName, string email)
    {
        var name = $"{firstName} {lastName}".Trim();
        if (name.Length == 0) name = email.Length > 0 ? email : "PCI Member";
        var vals = new Dictionary<string, object?> { ["name"] = name, ["customer_rank"] = 1 };
        if (email.Length > 0) vals["email"] = email;
        return vals;
    }

    /// <summary>Accounting mapping an operator can set, all optional.</summary>
    public sealed record InvoiceOptions(
        long? CompanyId = null, long? JournalId = null, long? ProductId = null, long? AccountId = null,
        long? TaxId = null, long? CurrencyId = null, long? PaymentTermId = null, long? TeamId = null);

    /// <summary>The account.move values for a settled payment, against an already-resolved partner.
    /// Odoo one2many writes use the (0, 0, values) command triple to create a line inline; a many2many
    /// like tax_ids uses (6, 0, [ids]) to REPLACE the set, which is what "apply exactly this tax" means.
    /// Unset options are omitted entirely so Odoo's own company defaults apply.</summary>
    public static Dictionary<string, object?> InvoiceValues(long partnerId, double amount, string product, string? reference,
        InvoiceOptions? options = null)
    {
        var o = options ?? new InvoiceOptions();
        var line = new Dictionary<string, object?>
        {
            ["name"] = product.Length > 0 ? product : "PCI fee",
            ["quantity"] = 1,
            ["price_unit"] = Math.Round(amount, 2),
        };
        if (o.ProductId is { } pid) line["product_id"] = pid;
        if (o.AccountId is { } aid) line["account_id"] = aid;
        if (o.TaxId is { } tid) line["tax_ids"] = new object[] { new object[] { 6, 0, new[] { tid } } };

        var vals = new Dictionary<string, object?>
        {
            ["move_type"] = "out_invoice",
            ["partner_id"] = partnerId,
            ["invoice_line_ids"] = new object[] { new object[] { 0, 0, line } },
        };
        if (!string.IsNullOrWhiteSpace(reference)) vals["ref"] = reference;
        if (o.CompanyId is { } cid) vals["company_id"] = cid;
        if (o.JournalId is { } jid) vals["journal_id"] = jid;
        if (o.CurrencyId is { } curr) vals["currency_id"] = curr;
        if (o.PaymentTermId is { } ptid) vals["invoice_payment_term_id"] = ptid;
        if (o.TeamId is { } team) vals["team_id"] = team;
        return vals;
    }

    /// <summary>Which Odoo model a canonical event writes to, honouring any configured overrides.
    /// Null = no representation (skip).</summary>
    static string? ModelFor(string eventType, Config cfg) => eventType switch
    {
        "member.registered" => cfg.partnerModel ?? PartnerModel,
        "payment.recorded" => cfg.invoiceModel ?? InvoiceModel,
        _ => null,
    };

    /// <summary>Which Odoo model a canonical event writes to with stock models (no config overrides).</summary>
    public static string? ModelFor(string eventType) => eventType switch
    {
        "member.registered" => PartnerModel,
        "payment.recorded" => InvoiceModel,
        _ => null,
    };

    /// <summary>
    /// Inspect a JSON-RPC reply. Returns a human-readable problem when the body carries a fault, or null
    /// when the call genuinely succeeded. Odoo answers faults with HTTP 200, so this — not the status code
    /// — decides whether a delivery is recorded as delivered.
    /// </summary>
    public static string? ResponseProblem(string? body)
    {
        if (string.IsNullOrWhiteSpace(body)) return "Odoo returned an empty response";
        try
        {
            var root = JsonDocument.Parse(body).RootElement;
            if (root.ValueKind != JsonValueKind.Object) return null;
            if (root.TryGetProperty("error", out var err))
            {
                // Prefer Odoo's specific fault text, then its generic message.
                if (err.TryGetProperty("data", out var d) && d.TryGetProperty("message", out var dm)
                    && dm.ValueKind == JsonValueKind.String && dm.GetString() is { Length: > 0 } dmsg) return dmsg;
                if (err.TryGetProperty("message", out var m) && m.ValueKind == JsonValueKind.String
                    && m.GetString() is { Length: > 0 } msg) return msg;
                return "Odoo JSON-RPC fault";
            }
            // authenticate answers result:false for bad credentials — a fault in everything but name.
            if (root.TryGetProperty("result", out var res) && res.ValueKind == JsonValueKind.False)
                return "Odoo rejected the credentials (authenticate returned false)";
            return null;
        }
        catch { return "Odoo returned a non-JSON response"; }
    }

    /// <summary>Build the Odoo JSON-RPC request for a delivery, or null when the event is unmapped (skip).
    /// Throws with a clear message when the connector is misconfigured / not authorized.</summary>
    public static async Task<HttpRequestMessage?> BuildRequest(Db db, HttpClient http, Dictionary<string, object?> integ, string eventType, string payloadRaw)
    {
        var config = ParseConfig(H.Str(integ["config"]));
        if (config.url.Length == 0) throw new Exception("Odoo not configured — set the instance URL.");
        if (config.database.Length == 0) throw new Exception("Odoo not configured — set the database name.");
        if (config.login.Length == 0) throw new Exception("Odoo not configured — set the login (user email).");
        var model = ModelFor(eventType, config);
        if (model is null) return null;

        var apiKey = ParseApiKey(H.Str(integ["secret"]))
                     ?? throw new Exception("Odoo not authorized — provide the user's API key.");
        var data = JsonDocument.Parse(payloadRaw).RootElement;
        string S(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
        double N(string k) => data.TryGetProperty(k, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDouble(out var d) ? d : 0;

        var endpoint = config.url + "/jsonrpc";
        var uid = await Authenticate(http, endpoint, config.database, config.login, apiKey);

        object values;
        if (eventType == "member.registered")
        {
            values = PartnerValues(S("first_name"), S("last_name"), S("email"));
        }
        else
        {
            var email = S("email");
            if (email.Length == 0) throw new Exception("Odoo invoice needs the payer's email to resolve the partner.");
            var partnerModel = config.partnerModel ?? PartnerModel;
            // partner_reuse (default on) looks the payer up by email first; off makes every payment
            // create a new partner, which duplicates customers in Odoo.
            var partnerId = (config.partnerReuse
                                ? await ResolvePartnerId(http, endpoint, config.database, uid, apiKey, partnerModel, email)
                                : null)
                            ?? await CreatePartnerId(http, endpoint, config.database, uid, apiKey, partnerModel,
                                   PartnerValues(S("first_name"), S("last_name"), email));
            values = InvoiceValues(partnerId, N("amount"), S("product"), S("reference") is { Length: > 0 } r ? r : null,
                new InvoiceOptions(config.companyId, config.journalId, config.productId, config.accountId,
                    config.taxId, config.currencyId, config.paymentTermId, config.teamId));
        }

        var body = ExecuteKwBody(config.database, uid, apiKey, model, "create", new object[] { values });
        var req = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        req.Headers.TryAddWithoutValidation("User-Agent", "PCI-Integrations/1.0");
        return req;
    }

    static async Task<string> PostRpc(HttpClient http, string endpoint, string body)
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        req.Headers.TryAddWithoutValidation("User-Agent", "PCI-Integrations/1.0");
        using var resp = await http.SendAsync(req);
        var txt = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception($"Odoo call failed (HTTP {(int)resp.StatusCode})");
        if (ResponseProblem(txt) is { } prob) throw new Exception(prob);
        return txt;
    }

    static async Task<long> Authenticate(HttpClient http, string endpoint, string database, string login, string apiKey)
    {
        var body = JsonSerializer.Serialize(new Dictionary<string, object?>
        {
            ["jsonrpc"] = "2.0",
            ["method"] = "call",
            ["params"] = new Dictionary<string, object?>
            {
                ["service"] = "common",
                ["method"] = "authenticate",
                ["args"] = new object[] { database, login, apiKey, new Dictionary<string, object?>() },
            },
            ["id"] = 1,
        });
        var txt = await PostRpc(http, endpoint, body);
        using var doc = JsonDocument.Parse(txt);
        if (doc.RootElement.TryGetProperty("result", out var res) && res.ValueKind == JsonValueKind.Number && res.TryGetInt64(out var uid) && uid > 0)
            return uid;
        throw new Exception("Odoo authentication did not return a user id — check the database, login and API key.");
    }

    static async Task<long?> ResolvePartnerId(HttpClient http, string endpoint, string database, long uid, string apiKey, string partnerModel, string email)
    {
        var domain = new object[] { new object[] { new object[] { "email", "=", email } } };
        var body = ExecuteKwBody(database, uid, apiKey, partnerModel, "search", domain,
            new Dictionary<string, object?> { ["limit"] = 1 });
        var txt = await PostRpc(http, endpoint, body);
        using var doc = JsonDocument.Parse(txt);
        if (doc.RootElement.TryGetProperty("result", out var res) && res.ValueKind == JsonValueKind.Array)
            foreach (var item in res.EnumerateArray())
                if (item.ValueKind == JsonValueKind.Number && item.TryGetInt64(out var id)) return id;
        return null;
    }

    static async Task<long> CreatePartnerId(HttpClient http, string endpoint, string database, long uid, string apiKey, string partnerModel, Dictionary<string, object?> values)
    {
        var body = ExecuteKwBody(database, uid, apiKey, partnerModel, "create", new object[] { values });
        var txt = await PostRpc(http, endpoint, body);
        using var doc = JsonDocument.Parse(txt);
        if (doc.RootElement.TryGetProperty("result", out var res) && res.ValueKind == JsonValueKind.Number && res.TryGetInt64(out var id) && id > 0)
            return id;
        throw new Exception("Odoo partner create returned no id");
    }
}
