using Microsoft.AspNetCore.SignalR;
using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — the realtime transport (spec §8.3; CCP_PHASE1_DESIGN §4).
///
/// Mounted at <c>/api/world/hubs/community</c>, which already falls inside WorldOnly.Allowed(), so
/// this adds no deployment-boundary surface.
///
/// THE HUB IS NOT THE SOURCE OF TRUTH, AND THAT IS THE WHOLE DESIGN. Accepted messages live in the
/// database with a per-room sequence; this only pushes notifications about them. A dropped
/// connection, a duplicated delivery, an out-of-order arrival or a node restart are all recoverable
/// because a client reconnects and replays from its last acknowledged sequence over plain HTTP.
/// Nothing here is load-bearing for correctness, which is precisely why the scale-out choice
/// (sticky sessions vs Redis backplane vs Azure SignalR, CCP-P1-005) can still be made later
/// without touching the durability model.
///
/// AUTHORIZATION IS RE-CHECKED ON EVERY INVOCATION, not just at connect. A connection can outlive
/// the session that opened it: a guest can be ejected, their session can expire, or a room can be
/// locked mid-conversation. Trusting connect-time state would leave an ejected participant
/// receiving messages until they chose to disconnect.
///
/// SENDING IS DELIBERATELY NOT A HUB METHOD. Messages are posted over HTTP so that one accept path
/// — with its moderation, transaction and sequence allocation — remains the only way text enters a
/// room. A hub-side send would be a second door to the same room, and the safety guarantee is only
/// as good as its narrowest entry point.
/// </summary>
public sealed class CommunityHub : Hub
{
    readonly Db _db;
    public CommunityHub(Db db) => _db = db;

    public const string Path = "/api/world/hubs/community";

    static string Group(long roomId) => $"world-room-{roomId}";

    /// <summary>Resolve the caller's guest session from the ticket supplied at connect. Returns null
    /// when the session is absent, expired or ended — including after an ejection.</summary>
    Dictionary<string, object?>? Session()
    {
        var http = Context.GetHttpContext();
        if (http is null) return null;
        // The ticket arrives as a query parameter because browsers cannot set headers on a
        // WebSocket handshake. It is the same opaque token the HTTP API issues, stored hashed, and
        // it is never logged.
        var token = http.Request.Query["access_token"].ToString();
        if (string.IsNullOrWhiteSpace(token)) token = http.Request.Cookies["pciworld_room"] ?? "";
        return CommunityRooms.SessionByToken(_db, token);
    }

    public override async Task OnConnectedAsync()
    {
        if (!CommunityRooms.Enabled(_db)) { Context.Abort(); return; }
        var session = Session();
        if (session is null) { Context.Abort(); return; }

        // A session belongs to exactly one room, so the group is derived from the session rather
        // than from anything the client sends. A client cannot ask to join a room it has no session
        // for, because it never names the room at all.
        await Groups.AddToGroupAsync(Context.ConnectionId, Group(H.L(session["room_id"])));
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Acknowledge receipt up to a sequence. Lets the server record how far a client has caught up
    /// so a reconnect replays only the gap. Advisory: a client that never acknowledges simply
    /// replays more, which is wasteful but never wrong.
    /// </summary>
    public Task Acknowledge(long sequence)
    {
        var session = Session();
        if (session is null) { Context.Abort(); return Task.CompletedTask; }
        // Monotonic: a stale or replayed acknowledgement must not rewind a client's position and
        // cause the same messages to be delivered again.
        _db.Execute(
            "UPDATE pciworld_guest_sessions SET last_seen_sequence=? WHERE id=? AND last_seen_sequence < ?",
            sequence, H.L(session["id"]), sequence);
        return Task.CompletedTask;
    }

    /// <summary>The catch-up a client performs itself after reconnecting. Reads from the durable
    /// history, so it is correct regardless of what the transport did or failed to do.</summary>
    public async Task Replay(long afterSequence)
    {
        var session = Session();
        if (session is null) { Context.Abort(); return; }
        var roomId = H.L(session["room_id"]);
        foreach (var m in CommunityRooms.Since(_db, roomId, afterSequence))
            await Clients.Caller.SendAsync("message", new
            {
                sequence = H.L(m["sequence"]),
                body = H.Str(m["body"]),
                author = H.Str(m["author_name"]),
                at = H.Str(m["published_at"]),
            });
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Deliberately does NOT end the guest session. A dropped connection is usually a tunnel or
        // a sleeping phone, and ending the session would free the participant's display name for
        // someone else to take mid-conversation. Sessions end by leaving, expiry or ejection.
        return base.OnDisconnectedAsync(exception);
    }
}

/// <summary>
/// Pushes outbox events to the room's SignalR group. The body is fetched from durable history at
/// delivery time rather than carried in the event, so a queued or logged event never holds message
/// content (§8.7).
/// </summary>
public sealed class SignalRBroadcastSink : CommunityOutbox.IBroadcastSink
{
    readonly IHubContext<CommunityHub> _hub;
    readonly Db _db;
    public SignalRBroadcastSink(IHubContext<CommunityHub> hub, Db db) { _hub = hub; _db = db; }

    public bool Deliver(long roomId, string eventType, string payloadJson)
    {
        try
        {
            var payload = System.Text.Json.JsonDocument.Parse(payloadJson).RootElement;
            if (eventType != "CommunityMessageAllowed") return true;   // unknown types drain, not block

            var sequence = payload.TryGetProperty("sequence", out var s) && s.TryGetInt64(out var v) ? v : 0;
            if (sequence <= 0) return true;

            // One row, by sequence — the durable record is the authority for what gets sent.
            var rows = CommunityRooms.Since(_db, roomId, sequence - 1, 1);
            if (rows.Count == 0) return true;   // withdrawn or purged: nothing to broadcast, not a failure
            var m = rows[0];

            _hub.Clients.Group($"world-room-{roomId}").SendAsync("message", new
            {
                sequence = H.L(m["sequence"]),
                body = H.Str(m["body"]),
                author = H.Str(m["author_name"]),
                at = H.Str(m["published_at"]),
            }).GetAwaiter().GetResult();
            return true;
        }
        catch
        {
            // A transport failure retries with backoff; it never surfaces to the accept path, and
            // the message stays recoverable by sequence regardless.
            return false;
        }
    }
}

/// <summary>
/// Drains the community outbox on a short interval. Separate from the platform's comms
/// OutboxDispatcher because the cadence is different — a chat message is stale in seconds, an email
/// is not — and because a slow email provider must never delay a room's broadcasts.
/// </summary>
public sealed class CommunityBroadcastService : Microsoft.Extensions.Hosting.BackgroundService
{
    readonly Db _db;
    readonly IHubContext<CommunityHub> _hub;
    static readonly TimeSpan Interval = TimeSpan.FromSeconds(1);

    public CommunityBroadcastService(Db db, IHubContext<CommunityHub> hub) { _db = db; _hub = hub; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var sink = new SignalRBroadcastSink(_hub, _db);
        using var timer = new PeriodicTimer(Interval);
        do
        {
            // Only work when the feature is on, so a deployment with rooms disabled does no polling.
            try { if (CommunityRooms.Enabled(_db)) CommunityOutbox.DrainOnce(_db, sink, 100); }
            catch (Exception e) { Console.Error.WriteLine($"[world community] broadcast drain failed: {e.Message}"); }

            // The image scan queue rides the same loop, gated on ITS own flag so a deployment with
            // images off does no image work at all. Recovery runs first: an item whose worker died
            // mid-scan is stranded until its expired lease is returned to the queue, and nothing was
            // published on the way, because publication only happens in the committing transaction.
            //
            // A bounded batch per tick, deliberately. Draining the whole queue in one pass would let
            // a backlog monopolise this loop and stall broadcast delivery for everybody — the two
            // jobs share a thread, so the image queue must not be able to starve the room.
            try
            {
                if (CommunityRooms.Enabled(_db) && CommunityMediaPipeline.ImagesEnabled(_db))
                {
                    CommunityMediaPipeline.RecoverStranded(_db);
                    var scanner = CommunityMediaPipeline.ConfiguredScanner(_db);
                    var policy = CommunityModeration.DefaultImageMatrix();
                    for (var i = 0; i < 5; i++)
                        if (!CommunityMediaPipeline.ScanOnce(_db, scanner, policy).Claimed) break;
                }
            }
            catch (Exception e) { Console.Error.WriteLine($"[world community] image scan failed: {e.Message}"); }
        }
        while (await WaitAsync(timer, stoppingToken));
    }

    static async Task<bool> WaitAsync(PeriodicTimer t, CancellationToken ct)
    {
        try { return await t.WaitForNextTickAsync(ct); }
        catch (OperationCanceledException) { return false; }
    }
}
