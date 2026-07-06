using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Shapes;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using PCI.SecureExam.App.Exam;
using PCI.SecureExam.Core.Models;
using PCI.SecureExam.Core.Proctoring;

namespace PCI.SecureExam.App.Views;

/// <summary>
/// Kiosk host. Renders each ExamScreen, drives the runner UI, shows the live camera thumbnail + mic
/// meter + in-exam chat, and reflects connectivity/violations. All exam scoring and the clock are
/// owned by ExamFlow/HeartbeatService (server-authoritative); this class is presentation + input only.
/// </summary>
public partial class MainWindow : Window
{
    private ExamFlow _flow = null!;
    private readonly Brush _ink = new SolidColorBrush(Color.FromRgb(0x0B,0x13,0x22));
    private readonly Brush _card = new SolidColorBrush(Color.FromRgb(0x11,0x1C,0x33));
    private readonly Brush _line = new SolidColorBrush(Color.FromRgb(0x22,0x30,0x4A));
    private readonly Brush _text = new SolidColorBrush(Color.FromRgb(0xE7,0xEE,0xFB));
    private readonly Brush _muted = new SolidColorBrush(Color.FromRgb(0x8A,0xA1,0xCC));
    private readonly Brush _brand = new SolidColorBrush(Color.FromRgb(0x1D,0x4E,0xD8));

    // runner state
    private int _idx;
    private readonly Dictionary<int,int> _selected = new();
    private readonly HashSet<int> _flagged = new();
    private Image? _camThumb;
    private ProgressBar? _micBar;
    private StackPanel? _chatLog;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += async (_, _) => await Boot();
    }

    private async System.Threading.Tasks.Task Boot()
    {
        // Launch parameters arrive via the pciexam:// URI (OS scheme handler passes it as an argument),
        // or PCI_LAUNCH for local dev. The file config (appsettings.json) supplies the default API URL;
        // the launch URI overrides it per session.
        var args = Environment.GetCommandLineArgs();
        var raw = args.FirstOrDefault(a => a.StartsWith("pciexam://", StringComparison.OrdinalIgnoreCase))
                  ?? Environment.GetEnvironmentVariable("PCI_LAUNCH") ?? "";
        var launch = PCI.SecureExam.Core.LaunchParameters.Parse(raw);
        var cfg = App.Config.WithLaunch(launch);
        var baseUrl = cfg.ApiBaseUrl;
        var token = launch.Token ?? "";
        var code = launch.Code ?? "";

        if (string.IsNullOrEmpty(code))
        {
            ShowMessage("Waiting for launch", "Start your exam from the PCI Student Portal — \u201CLaunch examination\u201D. This client opens automatically with your secure launch code.");
            return;
        }

        _flow = new ExamFlow(baseUrl, token, code, cfg);
        _flow.ScreenChanged += s => Dispatcher.Invoke(() => Render(s));
        _flow.Tick += s => Dispatcher.Invoke(() => UpdateTimer(s));
        _flow.ConnectivityChanged += on => Dispatcher.Invoke(() => SetConn(on));
        _flow.EventRaised += ev => Dispatcher.Invoke(() => OnEvent(ev));
        _flow.ChatReceived += m => Dispatcher.Invoke(() => AppendChat(m));

        ShowMessage("Authorizing", "Validating your secure launch code…");
        var ok = await _flow.AuthorizeAsync();
        if (!ok) { ShowMessage("Launch code invalid", "This exam link has expired or was already used. Return to the portal and launch again, or contact support."); return; }
    }

    // ---------------- screen router ----------------
    private void Render(ExamScreen s)
    {
        ScreenLabel.Text = s switch
        {
            ExamScreen.Consent => "Consent", ExamScreen.SystemCheck => "System check",
            ExamScreen.IdentityCapture => "Identity verification", ExamScreen.RoomScan => "Room scan",
            ExamScreen.Rules => "Examination rules", ExamScreen.Exam => "Examination in progress",
            ExamScreen.Submitted => "Submitted", ExamScreen.Terminated => "Session ended", _ => ""
        };
        TimerPill.Visibility = s == ExamScreen.Exam ? Visibility.Visible : Visibility.Collapsed;
        switch (s)
        {
            case ExamScreen.Consent: RenderConsent(); break;
            case ExamScreen.SystemCheck: RenderSystemCheck(); break;
            case ExamScreen.IdentityCapture: RenderIdentity(); break;
            case ExamScreen.RoomScan: RenderRoomScan(); break;
            case ExamScreen.Rules: RenderRules(); break;
            case ExamScreen.Exam: RenderExam(); break;
            case ExamScreen.Submitted: RenderSubmitted(); break;
            case ExamScreen.Terminated: RenderTerminated(); break;
        }
    }

    // ---------------- consent ----------------
    private void RenderConsent()
    {
        var a = _flow.Auth!;
        var body = Panel(24);
        body.Children.Add(H1($"Welcome, {a.CandidateName.Split(' ').FirstOrDefault()}"));
        body.Children.Add(P($"You are about to begin {a.ExamTitle} ({a.ExamCode}). This session is monitored by AI proctoring."));
        body.Children.Add(Bullet("Your webcam and microphone are recorded for the duration of the exam."));
        body.Children.Add(Bullet("Your screen is locked to this application. Other applications, tabs, and hotkeys are blocked."));
        body.Children.Add(Bullet("AI checks run continuously (face presence, multiple people, audio). Flags are reviewed by PCI."));
        body.Children.Add(Bullet("Your answers save to the PCI server every few seconds — a crash or outage will not lose work or add time."));
        body.Children.Add(Note("You can withdraw by closing the client before you accept. Proceeding constitutes consent to monitoring for exam-integrity purposes only."));
        var btn = Primary("I consent — continue", async () => { _flow.AcceptConsent(); await System.Threading.Tasks.Task.CompletedTask; });
        body.Children.Add(btn);
        Host.Content = Scroll(Center(body, 720));
    }

    // ---------------- system check ----------------
    private async void RenderSystemCheck()
    {
        var body = Panel(20);
        body.Children.Add(H1("System check"));
        body.Children.Add(P("Verifying your equipment and environment. All items must pass before you can continue."));
        var list = new StackPanel { Margin = new Thickness(0,8,0,0) };
        body.Children.Add(list);
        var cont = Primary("Continue", () => _flow.AfterSystemCheck());
        cont.IsEnabled = false; cont.Opacity = 0.5;
        body.Children.Add(cont);
        Host.Content = Scroll(Center(body, 720));

        await _flow.RunSystemChecksAsync();
        list.Children.Clear();
        foreach (var c in _flow.SystemChecks) list.Children.Add(CheckRow(c));
        var passed = _flow.SystemChecksPassed;
        cont.IsEnabled = passed; cont.Opacity = passed ? 1 : 0.5;
        if (!passed) list.Children.Add(Note("Resolve the failed items above, then reopen the client. Need help? Use live chat once the exam begins, or contact the portal support desk."));
    }

    // ---------------- identity ----------------
    private byte[]? _faceShot, _idShot;
    private void RenderIdentity()
    {
        var body = Panel(18);
        body.Children.Add(H1("Identity verification"));
        body.Children.Add(P("Center your face in the frame and capture a photo, then hold your government photo ID to the camera and capture it. AI confirms a face is present; PCI confirms the match."));

        _camThumb = new Image { Height = 260, Stretch = Stretch.Uniform, Margin = new Thickness(0,4,0,12) };
        var border = new Border { Background = _card, CornerRadius = new CornerRadius(14), Padding = new Thickness(10), Child = _camThumb };
        body.Children.Add(border);
        _flow.Camera!.PreviewFrame += OnPreview;
        if (_flow.Camera.TryOpen(out _)) _flow.Camera.Start();

        var row = new StackPanel { Orientation = Orientation.Horizontal };
        var status = P("");
        row.Children.Add(GhostBtn("Capture face", () => { _faceShot = _flow.Camera!.Snapshot(); status.Text = _faceShot!=null ? "Face captured." : "Camera not ready."; }));
        row.Children.Add(Spacer());
        row.Children.Add(GhostBtn("Capture ID", () => { _idShot = _flow.Camera!.Snapshot(); status.Text = _idShot!=null ? "ID captured." : "Camera not ready."; }));
        body.Children.Add(row);
        body.Children.Add(status);

        body.Children.Add(Primary("Verify identity", async () =>
        {
            if (_faceShot is null || _idShot is null) { status.Text = "Capture both your face and your ID first."; return; }
            status.Text = "Verifying…";
            var res = await _flow.VerifyIdentityAsync(_faceShot, _idShot);
            status.Text = res.Note ?? res.Result.ToString();
            if (res.Result == IdentityResult.Verified) { _flow.Camera!.PreviewFrame -= OnPreview; _flow.AfterIdentity(); }
        }));
        Host.Content = Scroll(Center(body, 640));
    }

    private void OnPreview(Mat frame)
    {
        Dispatcher.BeginInvoke(() =>
        {
            try { if (_camThumb != null) _camThumb.Source = frame.ToBitmapSource(); } catch { }
            finally { frame.Dispose(); }
        }, DispatcherPriority.Background);
    }

    private void RenderRoomScan()
    {
        var body = Panel(18);
        body.Children.Add(H1("Room scan"));
        body.Children.Add(P("Slowly pan your camera around your workspace so the environment is on record. When done, capture and continue."));
        _camThumb = new Image { Height = 300, Stretch = Stretch.Uniform };
        body.Children.Add(new Border { Background = _card, CornerRadius = new CornerRadius(14), Padding = new Thickness(10), Child = _camThumb });
        _flow.Camera!.PreviewFrame += OnPreview;
        body.Children.Add(Primary("Capture & continue", async () => { _flow.Camera!.PreviewFrame -= OnPreview; await _flow.CompleteRoomScanAsync(); }));
        Host.Content = Scroll(Center(body, 640));
    }

    private void RenderRules()
    {
        var body = Panel(16);
        body.Children.Add(H1("Examination rules"));
        foreach (var r in new[]{
            "Remain visible and alone in frame for the entire exam.",
            "No phones, notes, additional screens, headphones, or other applications.",
            "Do not leave the exam window — it is locked and focus loss is recorded.",
            "The timer is server-controlled; if you disconnect it keeps running per policy, and you resume where you left off.",
            "Live chat with the proctor desk is available throughout." })
            body.Children.Add(Bullet(r));
        body.Children.Add(Note("Breaching these rules may invalidate your result. Integrity events are logged with timestamps and reviewed by PCI."));
        body.Children.Add(Primary("I understand — start exam", async () => { await _flow.BeginExamAsync(this); _flow.AcceptRules(); }));
        Host.Content = Scroll(Center(body, 720));
    }

    // ---------------- exam runner ----------------
    private ItemsControl? _palette;
    private ContentControl? _qhost;
    private void RenderExam()
    {
        var grid = new Grid { Margin = new Thickness(0) };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });

        // left: question host + nav
        var left = new DockPanel { Margin = new Thickness(24,20,12,20) };
        _qhost = new ContentControl();
        DockPanel.SetDock(_qhost, Dock.Top);
        left.Children.Add(_qhost);
        grid.Children.Add(left);

        // right: proctor rail (camera, mic, chat, palette)
        var rail = new StackPanel { Margin = new Thickness(12,20,24,20) };
        _camThumb = new Image { Height = 150, Stretch = Stretch.UniformToFill };
        rail.Children.Add(RailCard("Camera", new Border { CornerRadius = new CornerRadius(10), ClipToBounds = true, Child = _camThumb }));
        _flow.Camera!.PreviewFrame += OnPreview;

        _micBar = new ProgressBar { Height = 6, Minimum = 0, Maximum = 1, Foreground = _brand, Background = _line, BorderThickness = new Thickness(0) };
        _flow.Mic!.Level += v => Dispatcher.BeginInvoke(() => { if (_micBar!=null) _micBar.Value = v; });
        rail.Children.Add(RailCard("Microphone", _micBar));

        _chatLog = new StackPanel();
        var chatScroll = new ScrollViewer { Height = 150, VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = _chatLog };
        var chatBox = new TextBox { Margin = new Thickness(0,8,0,0), Padding = new Thickness(8), Background = _ink, Foreground = _text, BorderBrush = _line };
        chatBox.KeyDown += async (s, e) => { if (e.Key == System.Windows.Input.Key.Enter && chatBox.Text.Trim().Length>0) { await _flow.SendChatAsync(chatBox.Text.Trim()); chatBox.Clear(); } };
        var chatWrap = new StackPanel();
        chatWrap.Children.Add(chatScroll);
        chatWrap.Children.Add(chatBox);
        rail.Children.Add(RailCard("Support chat", chatWrap));

        _palette = new ItemsControl();
        _palette.ItemsPanel = WrapPanelTemplate();
        rail.Children.Add(RailCard("Questions", _palette));

        Grid.SetColumn(rail, 1);
        grid.Children.Add(rail);

        Host.Content = grid;
        _idx = 0;
        if (_flow.Heartbeat is not null)                       // resumed answers appear immediately
            foreach (var kv in _flow.Heartbeat.Answers) _selected[kv.Key] = kv.Value;
        DrawQuestion();
        DrawPalette();
    }

    private void DrawQuestion()
    {
        var items = _flow.Auth!.Items;
        var it = items[_idx];
        var card = new Border { Background = new SolidColorBrush(Color.FromRgb(0xFF,0xFF,0xFF)), CornerRadius = new CornerRadius(16), Padding = new Thickness(28) };
        var sp = new StackPanel();
        sp.Children.Add(new TextBlock { Text = $"QUESTION {_idx+1} OF {items.Count}", Foreground = _brand, FontWeight = FontWeights.Bold, FontSize = 11.5 });
        sp.Children.Add(new TextBlock { Text = it.Domain, Foreground = new SolidColorBrush(Color.FromRgb(0x94,0xA3,0xB8)), FontSize = 12, Margin = new Thickness(0,2,0,14) });
        sp.Children.Add(new TextBlock { Text = it.Question, Foreground = new SolidColorBrush(Color.FromRgb(0x0F,0x17,0x2A)), FontSize = 17, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,0,0,18) });

        for (int i = 0; i < it.Options.Count; i++)
        {
            int oi = i;
            bool sel = _selected.TryGetValue(it.Id, out var v) && v == oi;
            var ob = new Border { CornerRadius = new CornerRadius(12), BorderThickness = new Thickness(1.5), Margin = new Thickness(0,0,0,10),
                BorderBrush = sel ? _brand : new SolidColorBrush(Color.FromRgb(0xE7,0xEB,0xF1)),
                Background = sel ? new SolidColorBrush(Color.FromRgb(0xEF,0xF4,0xFE)) : Brushes.White,
                Padding = new Thickness(14,12,14,12), Cursor = System.Windows.Input.Cursors.Hand };
            var row = new StackPanel { Orientation = Orientation.Horizontal };
            row.Children.Add(new Border { Width = 26, Height = 26, CornerRadius = new CornerRadius(13), Margin = new Thickness(0,0,12,0),
                Background = sel ? _brand : new SolidColorBrush(Color.FromRgb(0xEE,0xF1,0xF6)),
                Child = new TextBlock { Text = ((char)('A'+oi)).ToString(), Foreground = sel?Brushes.White:new SolidColorBrush(Color.FromRgb(0x47,0x54,0x69)), FontWeight=FontWeights.Bold, HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center, FontSize=12.5 } });
            row.Children.Add(new TextBlock { Text = it.Options[oi], Foreground = new SolidColorBrush(Color.FromRgb(0x0F,0x17,0x2A)), FontSize = 15, TextWrapping = TextWrapping.Wrap, VerticalAlignment = VerticalAlignment.Center, MaxWidth = 620 });
            ob.Child = row;
            ob.MouseLeftButtonUp += (s, e) => { _selected[it.Id] = oi; _flow.SetAnswer(it.Id, oi); DrawQuestion(); DrawPalette(); };
            sp.Children.Add(ob);
        }

        var nav = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0,8,0,0) };
        if (_idx > 0) nav.Children.Add(GhostBtn("← Previous", () => { _idx--; DrawQuestion(); DrawPalette(); }));
        if (_idx > 0) nav.Children.Add(Spacer());
        bool flagged = _flagged.Contains(it.Id);
        nav.Children.Add(GhostBtn(flagged ? "⚑ Unflag" : "⚑ Flag for review",
            () => { if (!_flagged.Add(it.Id)) _flagged.Remove(it.Id); DrawQuestion(); DrawPalette(); }));
        nav.Children.Add(Spacer());
        if (_idx < items.Count-1) nav.Children.Add(Primary("Next →", () => { _idx++; DrawQuestion(); DrawPalette(); }));
        else nav.Children.Add(Primary("Submit examination", async () => await ConfirmSubmit()));
        sp.Children.Add(nav);

        card.Child = sp;
        if (_qhost != null) _qhost.Content = card;
    }

    private void DrawPalette()
    {
        if (_palette is null) return;
        _palette.Items.Clear();
        var items = _flow.Auth!.Items;
        for (int i = 0; i < items.Count; i++)
        {
            int qi = i;
            bool answered = _selected.ContainsKey(items[i].Id);
            bool current = i == _idx;
            bool flag = _flagged.Contains(items[i].Id);
            var b = new Border { Width = 40, Height = 40, Margin = new Thickness(3), CornerRadius = new CornerRadius(9),
                Background = current ? _brand : answered ? new SolidColorBrush(Color.FromRgb(0x1A,0x2B,0x4D)) : new SolidColorBrush(Color.FromRgb(0x14,0x1F,0x38)),
                BorderBrush = flag ? new SolidColorBrush(Color.FromRgb(0xF5,0x9E,0x0B)) : Brushes.Transparent,
                BorderThickness = new Thickness(flag ? 2 : 0),
                Cursor = System.Windows.Input.Cursors.Hand,
                Child = new TextBlock { Text = (i+1).ToString(), Foreground = current||answered ? Brushes.White : _muted, FontWeight = FontWeights.Bold, FontSize=12.5, HorizontalAlignment=HorizontalAlignment.Center, VerticalAlignment=VerticalAlignment.Center } };
            b.MouseLeftButtonUp += (s, e) => { _idx = qi; DrawQuestion(); DrawPalette(); };
            _palette.Items.Add(b);
        }
    }

    private async System.Threading.Tasks.Task ConfirmSubmit()
    {
        int unanswered = _flow.Auth!.Items.Count(it => !_selected.ContainsKey(it.Id));
        int flaggedN = _flagged.Count;
        var parts = new List<string>();
        if (unanswered > 0) parts.Add($"{unanswered} unanswered (will score zero)");
        if (flaggedN > 0) parts.Add($"{flaggedN} flagged for review");
        var msg = parts.Count > 0 ? string.Join(" and ", parts) + ". Submit anyway?" : "Submit your examination? This is final.";
        if (MessageBox.Show(this, msg, "Submit examination", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
        var res = await _flow.SubmitAsync(auto: false);
    }

    private void RenderSubmitted()
    {
        _flow.Teardown(this);
        var r = _flow.Result;
        var body = Panel(16);
        if (r is null) { body.Children.Add(H1("Submitted")); body.Children.Add(P("Your answers were submitted. If your result isn’t shown, it will appear in the PCI Student Portal shortly.")); }
        else
        {
            body.Children.Add(H1(r.Result?.ToUpper() == "PASS" ? "Congratulations — you passed" : "Examination submitted"));
            body.Children.Add(P($"Your score: {r.Percent:0.#}% (pass mark 65%). A full score report and any credential are available in your PCI Student Portal."));
            if (!string.IsNullOrEmpty(r.CredentialId)) body.Children.Add(Note($"Credential issued: {r.CredentialId} — verifiable from the portal."));
        }
        body.Children.Add(Primary("Close", () => Close()));
        Host.Content = Scroll(Center(body, 620));
    }

    private void RenderTerminated()
    {
        var body = Panel(16);
        body.Children.Add(H1("Session ended"));
        body.Children.Add(P("Your exam session was ended. Any recorded work has been saved to the PCI server. Contact the support desk from the portal if this was unexpected."));
        body.Children.Add(Primary("Close", () => Close()));
        Host.Content = Scroll(Center(body, 560));
    }

    // ---------------- status + events ----------------
    private void UpdateTimer(int remaining)
    {
        var m = remaining / 60; var s = remaining % 60;
        TimerText.Text = $"{m:00}:{s:00}";
        TimerText.Foreground = remaining < 300 ? new SolidColorBrush(Color.FromRgb(0xFC,0xA5,0xA5)) : new SolidColorBrush(Color.FromRgb(0x9F,0xC0,0xFF));
    }

    private void SetConn(bool online)
    {
        ConnText.Text = online ? "Online" : "Reconnecting…";
        ConnText.Foreground = online ? new SolidColorBrush(Color.FromRgb(0x7D,0xEB,0xA5)) : new SolidColorBrush(Color.FromRgb(0xFC,0xD3,0x4D));
        ConnPill.Background = online ? new SolidColorBrush(Color.FromRgb(0x12,0x30,0x21)) : new SolidColorBrush(Color.FromRgb(0x2E,0x24,0x0E));
        ((Ellipse)((StackPanel)ConnPill.Child).Children[0]).Fill = online ? new SolidColorBrush(Color.FromRgb(0x4A,0xDE,0x80)) : new SolidColorBrush(Color.FromRgb(0xFB,0xBF,0x24));
    }

    private void OnEvent(ProctorEvent ev)
    {
        if (ev.Severity is ProctorSeverity.High or ProctorSeverity.Critical)
            FlashToast(ev.Type switch {
                ProctorEventType.MultipleFacesDetected => "More than one person detected in frame.",
                ProctorEventType.FaceAbsentTooLong => "Your face left the camera view.",
                ProctorEventType.ProhibitedProcess => "A prohibited application was detected.",
                ProctorEventType.SecondDisplayConnected => "A second display was connected.",
                _ => "Integrity event recorded." });
    }

    private DispatcherTimer? _toastTimer;
    private void FlashToast(string text)
    {
        ToastText.Text = text; Toast.Visibility = Visibility.Visible;
        _toastTimer?.Stop();
        _toastTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3.2) };
        _toastTimer.Tick += (s, e) => { Toast.Visibility = Visibility.Collapsed; _toastTimer!.Stop(); };
        _toastTimer.Start();
    }

    private void AppendChat(ChatMessage m) => AppendChatRow(m);
    private void AppendChatRow(ChatMessage m)
    {
        if (_chatLog is null) return;
        bool mine = m.From == "You";
        _chatLog.Children.Add(new Border {
            Background = mine ? _brand : new SolidColorBrush(Color.FromRgb(0x1A,0x24,0x3D)),
            CornerRadius = new CornerRadius(10), Padding = new Thickness(10,7,10,7), Margin = new Thickness(mine?30:0,3,mine?0:30,3),
            HorizontalAlignment = mine ? HorizontalAlignment.Right : HorizontalAlignment.Left,
            Child = new StackPanel { Children = {
                new TextBlock { Text = m.Body, Foreground = _text, TextWrapping = TextWrapping.Wrap, FontSize = 12.5 },
                new TextBlock { Text = $"{m.From} · {m.At.ToLocalTime():HH:mm}", Foreground = _muted, FontSize = 9.5, Margin = new Thickness(0,3,0,0) }
            }}});
    }

    // ---------------- tiny UI helpers ----------------
    private StackPanel Panel(double gap) => new() { Margin = new Thickness(0) };
    private TextBlock H1(string t) => new() { Text = t, Foreground = _text, FontSize = 26, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,10), TextWrapping = TextWrapping.Wrap };
    private TextBlock P(string t) => new() { Text = t, Foreground = _muted, FontSize = 14.5, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,0,0,12), LineHeight = 22 };
    private FrameworkElement Bullet(string t)
    {
        var d = new DockPanel { Margin = new Thickness(0,0,0,9) };
        var dot = new Ellipse { Width = 6, Height = 6, Fill = _brand, Margin = new Thickness(0,7,10,0), VerticalAlignment = VerticalAlignment.Top };
        DockPanel.SetDock(dot, Dock.Left);
        d.Children.Add(dot);
        d.Children.Add(new TextBlock { Text = t, Foreground = new SolidColorBrush(Color.FromRgb(0xC7,0xD2,0xE4)), FontSize = 14, TextWrapping = TextWrapping.Wrap });
        return d;
    }
    private Border Note(string t) => new() { Background = new SolidColorBrush(Color.FromRgb(0x14,0x22,0x40)), CornerRadius = new CornerRadius(12), Padding = new Thickness(14), Margin = new Thickness(0,6,0,14),
        Child = new TextBlock { Text = t, Foreground = new SolidColorBrush(Color.FromRgb(0x9F,0xB3,0xDA)), FontSize = 12.5, TextWrapping = TextWrapping.Wrap, LineHeight = 19 } };
    private Button Primary(string t, Action onClick) { var b = new Button { Content = t, Style = (Style)FindResource("Btn"), Margin = new Thickness(0,10,0,0), HorizontalAlignment = HorizontalAlignment.Left }; b.Click += (s,e)=>onClick(); return b; }
    private Button Primary(string t, Func<System.Threading.Tasks.Task> onClick) { var b = new Button { Content = t, Style = (Style)FindResource("Btn"), Margin = new Thickness(0,10,0,0), HorizontalAlignment = HorizontalAlignment.Left }; b.Click += async (s,e)=>await onClick(); return b; }
    private Button GhostBtn(string t, Action onClick) { var b = new Button { Content = t, Style = (Style)FindResource("Ghost") }; b.Click += (s,e)=>onClick(); return b; }
    private FrameworkElement Spacer() => new Border { Width = 12 };
    private Border RailCard(string title, UIElement child)
    {
        var sp = new StackPanel();
        sp.Children.Add(new TextBlock { Text = title.ToUpper(), Foreground = _muted, FontSize = 10.5, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,8) });
        sp.Children.Add(child);
        return new Border { Background = _card, CornerRadius = new CornerRadius(14), Padding = new Thickness(14), Margin = new Thickness(0,0,0,12), BorderBrush = _line, BorderThickness = new Thickness(1), Child = sp };
    }
    private FrameworkElement CheckRow(SystemCheckResult c)
    {
        var (bg, fg, glyph) = c.Status switch {
            SystemCheckStatus.Pass => (Color.FromRgb(0x12,0x30,0x21), Color.FromRgb(0x7D,0xEB,0xA5), "\u2713"),
            SystemCheckStatus.Warn => (Color.FromRgb(0x2E,0x24,0x0E), Color.FromRgb(0xFC,0xD3,0x4D), "!"),
            _ => (Color.FromRgb(0x2A,0x15,0x12), Color.FromRgb(0xFC,0xA5,0xA5), "\u2715") };
        var d = new DockPanel { Margin = new Thickness(0,0,0,8) };
        var badge = new Border { Width = 26, Height = 26, CornerRadius = new CornerRadius(13), Background = new SolidColorBrush(bg), Margin = new Thickness(0,0,12,0),
            Child = new TextBlock { Text = glyph, Foreground = new SolidColorBrush(fg), FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center } };
        DockPanel.SetDock(badge, Dock.Left);
        d.Children.Add(badge);
        var col = new StackPanel();
        col.Children.Add(new TextBlock { Text = c.Name, Foreground = _text, FontWeight = FontWeights.SemiBold, FontSize = 14 });
        col.Children.Add(new TextBlock { Text = c.Detail, Foreground = _muted, FontSize = 12, TextWrapping = TextWrapping.Wrap });
        d.Children.Add(col);
        return new Border { Background = _card, CornerRadius = new CornerRadius(12), Padding = new Thickness(14), Margin = new Thickness(0,0,0,8), Child = d };
    }
    private ScrollViewer Scroll(UIElement e) => new() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, Content = e };
    private FrameworkElement Center(FrameworkElement e, double width) { e.MaxWidth = width; e.Margin = new Thickness(24,32,24,40); e.HorizontalAlignment = HorizontalAlignment.Center; return e; }
    private void ShowMessage(string title, string body) { var p = Panel(12); p.Children.Add(H1(title)); p.Children.Add(P(body)); Host.Content = Scroll(Center(p, 560)); }
    private ItemsPanelTemplate WrapPanelTemplate() { var t = new ItemsPanelTemplate(); var f = new FrameworkElementFactory(typeof(WrapPanel)); t.VisualTree = f; return t; }
}
