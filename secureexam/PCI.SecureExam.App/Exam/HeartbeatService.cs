using PCI.SecureExam.App.Api;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Exam;

/// <summary>
/// The resilience core. Every few seconds it flushes answers + queued proctor events to the server and
/// receives the AUTHORITATIVE remaining time. The UI clock is driven by this response, so an internet
/// outage, sleep, or restart can never add exam time. On transient failure it keeps a local encrypted
/// copy (SecureStore) and retries; on reconnect the server reconciles. If the server says ForceSubmit
/// (hard deadline passed) the client submits immediately.
/// </summary>
public sealed class HeartbeatService : IDisposable
{
    private readonly PciApiClient _api;
    private readonly SecureStore _store;
    private readonly string _token;
    private readonly System.Timers.Timer _timer = new(5000);
    private readonly object _lock = new();
    private readonly Queue<ProctorEvent> _pending = new();
    private readonly Queue<string> _chatOut = new();
    private int _beating, _forced;

    public Dictionary<int,int> Answers { get; } = new();
    public int Violations { get; set; }
    public int RemainingSeconds { get; private set; }
    public bool Online { get; private set; } = true;

    public event Action<int>? Tick;                 // remaining seconds for the UI
    public event Action<bool>? ConnectivityChanged; // true=online
    public event Action? ForceSubmit;
    public event Action<ChatMessage>? MessageReceived;   // proctor → candidate, over the heartbeat

    public HeartbeatService(PciApiClient api, SecureStore store, string attemptToken, int remainingSeconds)
    {
        _api = api; _store = store; _token = attemptToken; RemainingSeconds = remainingSeconds;
        _timer.Elapsed += async (_, _) => await Beat();
    }

    public void Start() { _timer.Start(); LocalTickLoop(); }
    public void Enqueue(ProctorEvent ev) { lock (_lock) _pending.Enqueue(ev); }
    public void EnqueueChat(string body) { if (!string.IsNullOrWhiteSpace(body)) lock (_lock) _chatOut.Enqueue(body.Trim()); }
    public void SetAnswer(int itemId, int optionIndex) { lock (_lock) { Answers[itemId] = optionIndex; } PersistLocal(); }

    private async void LocalTickLoop()
    {
        // Smooth 1s countdown between server beats using the last authoritative value.
        while (!_disposed)
        {
            await Task.Delay(1000);
            if (RemainingSeconds > 0) RemainingSeconds--;
            Tick?.Invoke(RemainingSeconds);
            if (RemainingSeconds <= 0) { FireForceSubmit(); break; }
        }
    }

    private async Task Beat()
    {
        // A slow beat (HttpClient timeout is 30s, timer fires every 5s) must never overlap itself.
        if (Interlocked.Exchange(ref _beating, 1) == 1) return;
        List<ProctorEvent> batch; Dictionary<int,int> answers; List<string> chat;
        lock (_lock)
        {
            batch = _pending.ToList(); _pending.Clear();
            answers = new(Answers);
            chat = _chatOut.ToList(); _chatOut.Clear();
        }
        try
        {
            var resp = await _api.HeartbeatAsync(new HeartbeatRequest(_token, answers, Violations, batch, chat.Count > 0 ? chat : null));
            if (resp is { Ok: true })
            {
                RemainingSeconds = resp.RemainingSeconds;         // authoritative clock
                if (!Online) { Online = true; ConnectivityChanged?.Invoke(true); }
                if (resp.Messages is { Count: > 0 } msgs)
                    foreach (var m in msgs) MessageReceived?.Invoke(m);
                if (resp.ForceSubmit) FireForceSubmit();
            }
            else
            {
                lock (_lock) { foreach (var e in batch) _pending.Enqueue(e); foreach (var c in chat) _chatOut.Enqueue(c); }
            }
        }
        catch
        {
            if (Online) { Online = false; ConnectivityChanged?.Invoke(false); }
            lock (_lock) { foreach (var e in batch) _pending.Enqueue(e); foreach (var c in chat) _chatOut.Enqueue(c); } // requeue
            PersistLocal();
        }
        finally { Interlocked.Exchange(ref _beating, 0); }
    }

    private void FireForceSubmit()
    {
        if (Interlocked.Exchange(ref _forced, 1) == 0) ForceSubmit?.Invoke();
    }

    private void PersistLocal()
    {
        lock (_lock) _store.Save(new LocalState(_token, Answers, Violations, RemainingSeconds, DateTimeOffset.UtcNow));
    }

    private bool _disposed;
    public void Dispose() { _disposed = true; _timer.Dispose(); }

    public record LocalState(string Token, Dictionary<int,int> Answers, int Violations, int RemainingSeconds, DateTimeOffset At);
}
