using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Server.Hubs;

namespace PCI.SecureExam.Server.Controllers;

/// <summary>
/// In-memory launch registry for the reference server. A launch code is minted by the portal when a
/// candidate clicks "Launch examination" (server-to-server), handed to the client via the pciexam://
/// URI, then exchanged here for the full authorization. Codes are single-use and time-boxed.
/// Production would back this with the same database the Node backend uses.
/// </summary>
public sealed class LaunchStore
{
    private readonly ConcurrentDictionary<string, ExamAuthorization> _codes = new();

    public LaunchStore() => Seed();

    public ExamAuthorization? Redeem(string code)
        => _codes.TryRemove(code, out var auth) && auth.ClosesAt > DateTimeOffset.UtcNow ? auth : null;

    public string Mint(ExamAuthorization auth)
    {
        var code = Guid.NewGuid().ToString("N")[..12];
        _codes[code] = auth;
        return code;
    }

    // A demo code so the client is runnable end-to-end without the portal wired in.
    private void Seed()
    {
        var items = new List<ExamItem>
        {
            new(1,"Earned Value & Performance","Which metric best indicates cost efficiency in EVM?", new[]{"SPI","CPI","BAC","ETC"}),
            new(2,"Governed AI","When AI proposes a schedule change, the professional's duty is to:", new[]{"Accept it as final","Oversee, validate and remain accountable","Automate the decision","Hide the method"}),
            new(3,"Planning & Scheduling","An SPI of 0.85 means the project is:", new[]{"Ahead","On schedule","Behind schedule","Under budget"}),
            new(4,"Risk & Contingency","Contingency is best sized by:", new[]{"A flat 10%","Quantitative risk analysis","Client preference","Last year's average"}),
        };
        _codes["PCIDEMO12345"] = new ExamAuthorization(
            "demo-attempt-token", "Amelia Hartley", "amelia.hartley@example.com",
            "PCP-AI", "PCP-AI Examination", 90,
            DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(-5), DateTimeOffset.UtcNow.AddHours(3),
            true, true, items);
    }
}

[ApiController]
[Route("api/exam")]
public sealed class ExamController : ControllerBase
{
    private readonly LaunchStore _launch;
    private readonly IHubContext<ProctorHub> _hub;
    private readonly IWebHostEnvironment _env;
    public ExamController(LaunchStore launch, IHubContext<ProctorHub> hub, IWebHostEnvironment env)
    { _launch = launch; _hub = hub; _env = env; }

    public sealed record AuthorizeBody(string Code);

    [HttpPost("authorize")]
    public ActionResult<ExamAuthorization> Authorize([FromBody] AuthorizeBody body)
    {
        var auth = _launch.Redeem(body.Code);
        return auth is null ? Unauthorized(new { error = "invalid_or_expired_code" }) : Ok(auth);
    }

    /// <summary>Receives webcam frames / room-scan snapshots. Stored under an evidence folder keyed by attempt.</summary>
    [HttpPost("evidence")]
    [RequestSizeLimit(20_000_000)]
    public async Task<IActionResult> Evidence()
    {
        var form = await Request.ReadFormAsync();
        var attempt = form["attemptToken"].ToString();
        var kind = form["kind"].ToString();
        var file = form.Files.GetFile("file");
        if (string.IsNullOrEmpty(attempt) || file is null) return BadRequest();

        var dir = Path.Combine(_env.ContentRootPath, "evidence", Sanitize(attempt));
        Directory.CreateDirectory(dir);
        var name = $"{Sanitize(kind)}_{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}.jpg";
        await using (var fs = System.IO.File.Create(Path.Combine(dir, name)))
            await file.CopyToAsync(fs);

        // Notify any attached proctor console that new evidence arrived.
        await _hub.Clients.Group("proctors").SendAsync("Evidence", new { attempt, kind, name });
        return Ok(new { stored = name });
    }

    public sealed record IdentityBody(string AttemptToken, IdentityCheck Check);

    [HttpPost("identity")]
    public async Task<IActionResult> Identity([FromBody] IdentityBody body)
    {
        await _hub.Clients.Group("proctors").SendAsync("Identity", body);
        return Ok(new { ok = true });
    }

    private static string Sanitize(string s) => new(s.Where(c => char.IsLetterOrDigit(c) || c is '-' or '_').ToArray());
}
