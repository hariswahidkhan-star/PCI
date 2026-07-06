using System.Collections.ObjectModel;
using PCI.SecureExam.App.Api;
using PCI.SecureExam.App.Proctoring;
using PCI.SecureExam.App.Security;
using PCI.SecureExam.App.Support;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Exam;

/// <summary>
/// The orchestrator. Owns the whole exam lifecycle and wires the subsystems together:
///   Launch → Consent → SystemCheck → IdentityCapture → RoomScan → Rules → Exam → Submitted / Terminated.
///
/// It is deliberately UI-agnostic: MainWindow subscribes to its events and calls its Advance* methods.
/// Every proctor signal (from keyboard hook, process/display guards, camera, mic) is funnelled through
/// RaiseEvent → the HeartbeatService queue, so evidence reaches the server on the next beat and is
/// reconciled even across a crash or outage.
/// </summary>
public sealed class ExamFlow : IDisposable
{
    private readonly PciApiClient _api;
    private readonly string _baseUrl, _sessionToken, _launchCode;

    // Subsystems (constructed once authorized)
    public ExamAuthorization? Auth { get; private set; }
    public HeartbeatService? Heartbeat { get; private set; }
    public ChatClient? Chat { get; private set; }
    public CameraService? Camera { get; private set; }
    public MicMonitor? Mic { get; private set; }
    private KeyboardHook? _keys;
    private ProcessGuard? _procGuard;
    private DisplayGuard? _display;
    private SecureStore? _store;
    private IIdentityVerifier? _identity;

    // Observable state for the UI
    public ExamScreen Screen { get; private set; } = ExamScreen.Launch;
    public ObservableCollection<ProctorEvent> Events { get; } = new();
    public ObservableCollection<SystemCheckResult> SystemChecks { get; } = new();
    public ObservableCollection<ChatMessage> ChatLog { get; } = new();
    public int Violations { get; private set; }
    public IdentityCheck? Identity { get; private set; }
    public SubmitResult? Result { get; private set; }
    /// <summary>True when the submission was accepted but the result is on integrity hold — the UI must
    /// show the hold message and MUST NOT display a pass/fail outcome or a 0% score.</summary>
    public bool ResultHeld => Result?.Held ?? false;

    public event Action<ExamScreen>? ScreenChanged;
    public event Action<ProctorEvent>? EventRaised;
    public event Action<int>? Tick;
    public event Action<bool>? ConnectivityChanged;
    public event Action<ChatMessage>? ChatReceived;
    public event Action<string>? Terminated;

    private readonly PCI.SecureExam.Core.ClientConfig _cfg;

    public ExamFlow(string baseUrl, string sessionToken, string launchCode, PCI.SecureExam.Core.ClientConfig? config = null)
    {
        _baseUrl = baseUrl; _sessionToken = sessionToken; _launchCode = launchCode;
        _cfg = config ?? new PCI.SecureExam.Core.ClientConfig();
        _api = new PciApiClient(baseUrl, sessionToken);
    }

    // ---- 1. Launch: validate the launch code with the server, receive the authorization + items ----
    public async Task<bool> AuthorizeAsync()
    {
        Auth = await _api.AuthorizeAsync(_launchCode);
        if (Auth is null) return false;
        // The launch code was exchanged for a short-lived exam session token; use it for the chat hub too.
        var effectiveToken = !string.IsNullOrWhiteSpace(Auth.SessionToken) ? Auth.SessionToken! : _sessionToken;

        _store = new SecureStore(Auth.AttemptToken);
        var log = App.Services?.GetService(typeof(Microsoft.Extensions.Logging.ILogger<ExamFlow>)) as Microsoft.Extensions.Logging.ILogger;
        var monitor = PCI.SecureExam.App.Providers.AiProviderFactory.CreateMonitor(_cfg, log);
        Camera = new CameraService(monitor);
        _identity = PCI.SecureExam.App.Providers.AiProviderFactory.CreateIdentityVerifier(_cfg, jpg => Camera!.CountFacesInJpeg(jpg), log);
        Mic = new MicMonitor(monitor);
        Chat = new ChatClient(_baseUrl, effectiveToken, Auth.AttemptToken, _cfg.Chat.HubPath);

        Camera.Event += RaiseEvent;
        Camera.EvidenceJpeg += async jpeg =>
        {
            try { await _api.UploadEvidenceBase64Async(Auth!.AttemptToken, "frame", jpeg); } catch { }
        };
        Mic.Event += RaiseEvent;
        Chat.MessageReceived += m => { ChatLog.Add(m); ChatReceived?.Invoke(m); };

        Go(ExamScreen.Consent);
        return true;
    }

    public void AcceptConsent() => Go(ExamScreen.SystemCheck);

    // ---- 2. System check: browser/OS is native here; verify camera, mic, display, VM, connectivity ----
    public async Task RunSystemChecksAsync()
    {
        SystemChecks.Clear();

        var camOk = Camera!.TryOpen(out var camDetail);
        Add("Webcam", camOk ? SystemCheckStatus.Pass : SystemCheckStatus.Fail, camDetail);

        var micOk = Mic!.TryStart(out var micDetail);
        Add("Microphone", micOk ? SystemCheckStatus.Pass : SystemCheckStatus.Fail, micDetail);

        _display = new DisplayGuard();
        Add("Displays", _display.IsMultiMonitor() ? SystemCheckStatus.Fail : SystemCheckStatus.Pass,
            _display.IsMultiMonitor() ? "More than one display detected. Disconnect additional monitors." : "Single display.");

        var vm = VmDetector.Check();
        Add("Environment", vm is null ? SystemCheckStatus.Pass : SystemCheckStatus.Warn,
            vm?.Detail ?? "No virtual machine or remote session detected.");

        _procGuard = new ProcessGuard();
        var blocking = _procGuard.PreflightBlockingProcesses();
        Add("Applications", blocking.Count == 0 ? SystemCheckStatus.Pass : SystemCheckStatus.Fail,
            blocking.Count == 0 ? "No prohibited applications running."
                                : "Close before continuing: " + string.Join(", ", blocking));

        bool online = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        Add("Connectivity", online ? SystemCheckStatus.Pass : SystemCheckStatus.Fail,
            online ? "Connected to the exam service." : "No network connection.");

        await Task.CompletedTask;
    }

    public bool SystemChecksPassed => SystemChecks.All(c => c.Status != SystemCheckStatus.Fail);
    public void AfterSystemCheck() => Go(Auth!.RequiresIdentity ? ExamScreen.IdentityCapture : ExamScreen.Rules);

    // ---- 3. AI identity verification ----
    public async Task<IdentityCheck> VerifyIdentityAsync(byte[] faceJpeg, byte[] idJpeg)
    {
        Identity = await _identity!.VerifyAsync(faceJpeg, idJpeg);
        try { await _api.PostIdentityAsync(Auth!.AttemptToken, Identity); } catch { }
        RaiseEvent(new ProctorEvent(
            Identity.Result == IdentityResult.Verified ? ProctorEventType.IdentityVerified : ProctorEventType.IdentityFailed,
            Identity.Result == IdentityResult.Verified ? ProctorSeverity.Info : ProctorSeverity.High,
            DateTimeOffset.UtcNow, Identity.Note));
        return Identity;
    }

    public void AfterIdentity() => Go(Auth!.RequiresRoomScan ? ExamScreen.RoomScan : ExamScreen.Rules);

    public async Task CompleteRoomScanAsync()
    {
        var shot = Camera!.Snapshot();
        if (shot is not null) { try { await _api.UploadEvidenceBase64Async(Auth!.AttemptToken, "roomscan", shot); } catch { } }
        RaiseEvent(new ProctorEvent(ProctorEventType.RoomScanCompleted, ProctorSeverity.Info, DateTimeOffset.UtcNow));
        Go(ExamScreen.Rules);
    }

    public void AcceptRules() => Go(ExamScreen.Exam);

    // ---- 4. Begin the secured exam: lock down, start capture + heartbeat + chat ----
    public async Task BeginExamAsync(System.Windows.Window host)
    {
        // Full application lockdown
        KioskWindow.Enter(host);
        _keys = new KeyboardHook();
        _keys.HotkeyBlocked += name => RaiseEvent(new ProctorEvent(ProctorEventType.HotkeyBlocked, ProctorSeverity.Low, DateTimeOffset.UtcNow, name));
        _keys.Install();
        _procGuard ??= new ProcessGuard();
        _procGuard.Event += RaiseEvent;
        _procGuard.StartMonitoring();
        _display ??= new DisplayGuard();
        _display.Event += RaiseEvent;
        _display.Start();

        // Prevent this window from being screen-captured/streamed (best-effort, documented)
        var hwnd = new System.Windows.Interop.WindowInteropHelper(host).Handle;
        CaptureShield.Protect(hwnd);

        Camera!.Start();

        // Server-anchored clock + resume. The server is authoritative (its heartbeat corrects the clock);
        // we seed from an encrypted local cache if a prior attempt was interrupted, so the countdown is
        // sane for the few seconds before the first beat lands.
        // Resume seeding, in trust order: (1) the server's authoritative RemainingSeconds/SavedAnswers
        // from authorize, (2) the encrypted local cache overlaid on top (it may hold answers saved in the
        // seconds between the last beat and a crash). The first heartbeat re-anchors the clock regardless.
        int remaining = Auth!.RemainingSeconds ?? Auth.DurationMinutes * 60;
        var seeded = new Dictionary<int,int>();
        if (Auth.SavedAnswers is not null) foreach (var kv in Auth.SavedAnswers) seeded[kv.Key] = kv.Value;
        var cached = _store!.Load<HeartbeatService.LocalState>();
        if (cached is not null)
        {
            if (Auth.RemainingSeconds is null && cached.RemainingSeconds > 0) remaining = cached.RemainingSeconds;
            if (cached.Answers is not null) foreach (var kv in cached.Answers) seeded[kv.Key] = kv.Value;
        }
        Heartbeat = new HeartbeatService(_api, _store!, Auth.AttemptToken, remaining);
        foreach (var kv in seeded) Heartbeat.Answers[kv.Key] = kv.Value;
        Heartbeat.MessageReceived += m => { ChatLog.Add(m); ChatReceived?.Invoke(m); };
        Heartbeat.Tick += s => Tick?.Invoke(s);
        Heartbeat.ConnectivityChanged += on => ConnectivityChanged?.Invoke(on);
        Heartbeat.ForceSubmit += async () => await SubmitAsync(auto: true);
        Heartbeat.Start();

        if (_cfg.Chat.Enabled) { try { await Chat!.ConnectAsync(); } catch { } }
        RaiseEvent(new ProctorEvent(ProctorEventType.SessionStarted, ProctorSeverity.Info, DateTimeOffset.UtcNow));
    }

    public void SetAnswer(int itemId, int optionIndex)
    {
        Heartbeat?.SetAnswer(itemId, optionIndex);
        RaiseEvent(new ProctorEvent(ProctorEventType.AnswerSaved, ProctorSeverity.Info, DateTimeOffset.UtcNow, $"Q{itemId}"));
    }

    public async Task SendChatAsync(string body)
    {
        var mine = new ChatMessage("You", body, DateTimeOffset.UtcNow);
        ChatLog.Add(mine);
        Heartbeat?.EnqueueChat(body);                       // primary: rides the unified backend
        if (Chat is not null) { try { await Chat.SendAsync(body); } catch { } }  // optional SignalR hub
    }

    // ---- 5. Submit: server scores; then tear down lockdown ----
    public async Task<SubmitResult?> SubmitAsync(bool auto)
    {
        if (Result is not null) return Result; // idempotent
        var answers = Heartbeat?.Answers ?? new Dictionary<int,int>();
        Result = await _api.SubmitAsync(new SubmitRequest(Auth!.AttemptToken, answers, Violations));
        RaiseEvent(new ProctorEvent(ProctorEventType.ExamSubmitted, ProctorSeverity.Info, DateTimeOffset.UtcNow, auto ? "auto/deadline" : "candidate"));
        _store?.Clear();
        Go(ExamScreen.Submitted);
        return Result;
    }

    public void Terminate(System.Windows.Window host, string reason)
    {
        Go(ExamScreen.Terminated);
        Teardown(host);
        Terminated?.Invoke(reason);
    }

    // ---- shared plumbing ----
    private void RaiseEvent(ProctorEvent ev)
    {
        Events.Add(ev);
        if (ev.Severity is ProctorSeverity.High or ProctorSeverity.Critical) Violations++;
        Heartbeat?.Enqueue(ev);
        if (Heartbeat is not null) Heartbeat.Violations = Violations;
        EventRaised?.Invoke(ev);
    }

    private void Add(string name, SystemCheckStatus st, string detail) => SystemChecks.Add(new SystemCheckResult(name, st, detail));
    private void Go(ExamScreen s) { Screen = s; ScreenChanged?.Invoke(s); }

    public void Teardown(System.Windows.Window host)
    {
        try { _keys?.Dispose(); } catch { }
        try { _procGuard?.Dispose(); } catch { }
        try { _display?.Dispose(); } catch { }
        try { Camera?.Dispose(); } catch { }
        try { Mic?.Dispose(); } catch { }
        try { Heartbeat?.Dispose(); } catch { }
        try { var h = new System.Windows.Interop.WindowInteropHelper(host).Handle; CaptureShield.Release(h); } catch { }
        try { KioskWindow.Exit(host); } catch { }
    }

    public void Dispose() { try { Chat?.DisposeAsync().AsTask().Wait(300); } catch { } }
}
