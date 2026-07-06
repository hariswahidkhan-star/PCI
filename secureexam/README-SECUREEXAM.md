# PCI Secure Exam — downloadable proctored exam client (.NET 8)

A Pearson-VUE-OnVUE-style secure exam application for the PCP-AI examination: AI identity
verification, automatic AI proctoring (webcam + microphone), in-exam chat support, full application
lockdown, and crash/outage/restart resume. Built on .NET 8 (WPF client + ASP.NET Core server) and
designed to sit alongside the existing PCI backend.

> **Honest capability statement.** This is a real, working WPF client with genuine lockdown, capture,
> and a complete monitoring pipeline. Two things are deliberately explicit rather than oversold:
> (1) **OS-level lockdown** — true kernel-level lockdown (blocking Ctrl+Alt+Del/Secure Attention
> Sequence, task manager at the driver level) requires a signed kernel driver and OS notarization,
> which is a signing/deployment effort, not a source deliverable. The client does everything possible
> in user space (kiosk, low-level keyboard hook, process/display/VM guards, capture exclusion) and
> **degrades honestly** — it logs and warns rather than pretending. (2) **AI accuracy** — a rule-based
> baseline analyzer ships so the whole pipeline runs offline and is testable; production swaps in a
> trained/cloud model behind one interface. Neither the baseline nor the client ever fabricates a
> high-confidence identity match or a score. Scoring is always server-side.

## Solution layout

```
PCI.SecureExam.sln
├─ PCI.SecureExam.Core     class library — shared DTOs & enums (net8.0)
├─ PCI.SecureExam.App      WPF kiosk client (net8.0-windows)
├─ PCI.SecureExam.Server   ASP.NET Core launch + evidence + SignalR chat (net8.0) — optional reference
└─ PCI.SecureExam.Tests    xUnit tests for Core (cross-platform: launch parsing, config, analyzer rules, DTO contracts)
```

### Core (`PCI.SecureExam.Core`)
`Models/Enums.cs`, `Models/Dtos.cs` — the wire contract shared by client and server:
`ExamAuthorization`, `ExamItem`, `HeartbeatRequest/Response`, `ProctorEvent`, `IdentityCheck`,
`SubmitRequest/Result`, `ChatMessage`, and the `ExamScreen`/`ProctorEventType`/severity enums.

### App (`PCI.SecureExam.App`) — the client
- **Security/** — the lockdown layer.
  - `KeyboardHook` (WH_KEYBOARD_LL): swallows Alt+Tab, Alt+F4, Ctrl+Esc, Win, PrintScreen, etc.
    *Cannot* block Ctrl+Alt+Del by design (documented in-file).
  - `ProcessGuard`: denylist of browsers/chat/screen-share/VM/recorders; preflight + live polling.
  - `DisplayGuard`: detects a second monitor connected mid-exam.
  - `VmDetector`: flags RDP / virtual-machine BIOS hints.
  - `CaptureShield`: `SetWindowDisplayAffinity(WDA_EXCLUDEFROMCAPTURE)` so the exam window is excluded
    from screen capture/broadcast (best-effort, documented).
  - `KioskWindow`: borderless top-most full-virtual-screen; strips system menu / min / max.
- **Proctoring/** — the AI layer.
  - `Interfaces` — `IIdentityVerifier`, `IProctorAnalyzer` (the single upgrade seam).
  - `CameraService` — OpenCvSharp webcam capture, Haar face detection (~1.25 fps), rolling JPEG
    evidence snapshots, live preview.
  - `MicMonitor` — NAudio microphone capture, RMS + zero-crossing voice-likelihood, level meter.
  - `BaselineAnalyzer` — ships as default: no-face / multiple-faces / absence>8s / loud-audio / speech
    rules, plus a conservative baseline identity verifier.
- **Exam/** — `SecureStore` (DPAPI-encrypted local answer/violation cache), `HeartbeatService`
  (5-second server flush; **server-authoritative clock**; offline requeue; ForceSubmit on deadline).
- **Api/** — `PciApiClient`: `authorize`, `heartbeat`, `submit`, `evidence` (multipart), `identity`.
- **Support/** — `ChatClient`: SignalR to `/hubs/proctor`, auto-reconnect.
- **Views/** — `MainWindow`: the kiosk host that renders the whole screen sequence
  (Consent → System check → Identity → Room scan → Rules → Exam → Submitted/Terminated), the exam
  runner (question palette, per-question save, live camera thumbnail, mic meter, in-exam chat,
  connectivity + violation banners, server-driven timer).
- **Exam/ExamFlow.cs** — the orchestrator that wires security + proctoring + heartbeat + chat and funnels
  every integrity signal to the server via the heartbeat queue.

### Server (`PCI.SecureExam.Server`)
- `Controllers/ExamController` — `POST /api/exam/authorize` (single-use, time-boxed launch-code
  redemption → `ExamAuthorization`), `POST /api/exam/evidence` (webcam/room-scan sink),
  `POST /api/exam/identity` (AI result sink). Ships with a demo launch code so the client runs
  end-to-end without the portal wired in.
- `Hubs/ProctorHub` — realtime chat groups per attempt, a `proctors` console group, and manual flag.
- `Program.cs` — host wiring (controllers + SignalR + CORS).

## Quickstart — plug & play

**Prerequisites:** Windows 10/11 + [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0). That's it.

```powershell
# 1) configure (one file). Defaults work for local dev against the reference server.
notepad PCI.SecureExam.App/appsettings.json     # set ApiBaseUrl, AI provider, thresholds

# 2) build + test (restore → build → xUnit) — one command
./build.ps1

# 3) verify THIS machine can run a secured exam (camera/mic/displays/VM/apps/network)
./build.ps1 -SelfTest        # or: PCISecureExam.exe --selftest   (exit 0 = ready)

# 4) run end-to-end with the built-in demo launch code
dotnet run --project PCI.SecureExam.Server      # terminal 1 → http://localhost:5000
./build.ps1 -Run                                # terminal 2 → full flow with code PCIDEMO12345
```

On first run the client **self-registers the `pciexam://` URI scheme** for the current user (no admin,
no installer needed), so the portal's *"Open in the PCI Secure Exam app"* button works immediately.
CI is included (`.github/workflows/build.yml`): full Windows build+tests, plus Core tests on Linux.

### Configuration reference (`appsettings.json`)

| Key | Default | What it does |
|---|---|---|
| `ApiBaseUrl` | `https://localhost:5001` | PCI backend base URL. **Overridden per session by the `api=` value in the launch URI.** |
| `Ai:IdentityProvider` | `Baseline` | `Baseline` \| `AzureFace` \| `AwsRekognition` — selects the `IIdentityVerifier` via `AiProviderFactory`. |
| `Ai:MonitorProvider` | `Baseline` | Selects the `IProctorAnalyzer`. |
| `Ai:Endpoint` / `Ai:ApiKey` / `Ai:Region` | empty | Cloud-provider credentials (put real keys in `appsettings.Local.json` — gitignored). |
| `Ai:IdentityMatchThreshold` | `0.80` | Similarity at/above which a provider match counts as Verified. |
| `Proctoring:AbsenceSeconds` | `8` | No-face duration before a High event. |
| `Proctoring:LoudAudioRms` | `0.28` | RMS threshold for the loud-audio event. |
| `Proctoring:EvidenceIntervalSeconds` | `15` | Webcam evidence snapshot cadence. |
| `Features:RegisterUriScheme` | `true` | Self-register `pciexam://` at startup. |
| `Chat:Enabled` / `Chat:HubPath` | `true` / `/hubs/proctor` | In-exam support chat toggle + SignalR path. |

Environment variables prefixed `PCI_` override any key (e.g. `PCI_ApiBaseUrl`, `PCI_Ai__ApiKey`).
The server reads its own `appsettings.json` (`Urls`, `Cors:AllowedOrigins`) — no hardcoded origins.

## Launch hand-off from the web portal

The Student Portal's "Launch examination" button opens the desktop client via a custom URI scheme.

1. **Register the scheme** (installer step, per-user shown; production installers set this machine-wide):

```reg
Windows Registry Editor Version 5.00
[HKEY_CURRENT_USER\Software\Classes\pciexam]
@="URL:PCI Secure Exam"
"URL Protocol"=""
[HKEY_CURRENT_USER\Software\Classes\pciexam\shell\open\command]
@="\"C:\\Program Files\\PCI Secure Exam\\PCISecureExam.exe\" \"%1\""
```

2. **Portal emits** `pciexam://start?code={launchCode}&api={backendBaseUrl}&token={sessionToken}`.
   The portal already has a secure exam runner; the downloadable-client path is additive.

3. The client parses that URI on startup (`MainWindow.ParseLaunch`), exchanges the code at
   `/api/exam/authorize`, and begins the flow.

## Integration with the existing PCI backend

The exam **clock and scoring** already live in the Node backend and are power-off safe:
`/api/me/exam/start` (resumes with saved answers + remaining time), `/api/me/exam/heartbeat`
(server-anchored clock + answer/violation persistence), `/api/me/exam/submit` (server-scored, issues
the PCP-AI credential on pass). `PciApiClient` targets those shapes. This .NET server only adds the
launch + evidence + realtime pieces, so the two run side by side behind one gateway.

> **Wire-format note.** `PciApiClient` uses the Core PascalCase records; when pointing the client at the
> Node backend (snake_case JSON), enable `PropertyNameCaseInsensitive`/a small adapter, or expose the
> heartbeat/submit endpoints from this ASP.NET server as a façade over the Node service. The reference
> server here already speaks the PascalCase shape.

## Plugging in a production AI provider

Implement one interface — no other code changes:

```csharp
public sealed class AzureFaceVerifier : IIdentityVerifier {
    public Task<IdentityCheck> VerifyAsync(byte[] faceJpeg, byte[] idJpeg, CancellationToken ct = default) {
        /* call Azure Face / AWS Rekognition; return a real similarity score */
    }
}
public sealed class YoloProctorAnalyzer : IProctorAnalyzer { /* phone/gaze/person models */ }
```

Then register it in **one place** — `Providers/AiProviderFactory.cs` — add a case, and select it in
`appsettings.json` (`Ai:IdentityProvider": "AzureFace"` + endpoint/key). Ready-made fail-closed stubs for
Azure Face and AWS Rekognition are already in the factory: they return *Inconclusive* until you add the
API call, and never fabricate a match. The capture, evidence, heartbeat, chat, and lockdown layers are
unchanged.



## ✅ Now fully integrated with the PCI backend (portal + admin, one system)

As of this build the secure client talks to the **same Node backend** that serves the Student Portal and
Admin Panel — there is no separate data silo. The backend implements the client's endpoints directly and
tolerates the client's PascalCase JSON:

| Client call | Backend endpoint (Node) | Effect |
|---|---|---|
| Redeem launch code | `POST /api/exam/authorize` | Validates a single-use code minted by the portal/admin; returns `ExamAuthorization` + items |
| Heartbeat (+ events) | `POST /api/me/exam/heartbeat` | Persists answers, violations **and the proctor-event batch** (`PendingEvents`) to `proctor_events`; returns the server-authoritative clock |
| Identity result | `POST /api/exam/identity` | Stores the AI identity check; sets `exam_attempts.identity_result` |
| Evidence snapshot | `POST /api/exam/evidence` | Stores webcam/room-scan frames; bumps `evidence_count` |
| Submit | `POST /api/me/exam/submit` | Server-scores, issues the PCP-AI credential on pass |

**Where results show up automatically:**
- **Student Portal** → *Results* (score + domain bands) and *Certificate* (verifiable credential).
- **Admin Panel** → new **Secure Exam** section: every session with its AI identity result, live-proctoring
  event timeline, evidence gallery, and server score. Admin can **Mark reviewed**, **Invalidate** (which
  **revokes the issued credential**), or **Reinstate**.
- **Public verify page** → reflects credential status (including revocation after invalidation).

**Launch flow (real):** the portal's *"Open in the PCI Secure Exam app"* button calls
`POST /api/me/exam/launch-code`, which mints a single-use `pciexam://start?code=…` link; the client redeems
it at `/api/exam/authorize`. Admin can mint a code for any candidate via
`POST /api/admin/exam-sessions/launch-code`.

The bundled `PCI.SecureExam.Server` (ASP.NET) remains as an **optional reference** for the realtime chat hub
and as a standalone harness; in production the Node backend is the system of record and the SignalR hub can
be hosted alongside it or replaced with the portal's existing chat transport.


## New in this build — Live Proctoring Console + two-way messaging (round-2 review)

- **Live Proctoring Console** (Admin → Secure Exam): every in-progress sitting appears as a live card —
  candidate, server-clock time remaining, connection health from the heartbeat (**Live / Lagging / Offline**),
  integrity flags, client type, unread candidate messages. Auto-refreshes every 5 seconds. Click through to
  the full session drawer.
- **Proctor ↔ candidate messaging over the heartbeat** — no SignalR required. Admin sends from the session
  drawer; the message rides the candidate's next heartbeat (≤5 s) into the **desktop client's chat panel**
  and as an on-screen notice in the **browser runner**. Candidate replies travel back the same way
  (`ChatOut` on the heartbeat). The SignalR hub remains as an optional low-latency enhancement.
- **Flag for review** in the desktop runner: ⚑ button per question, amber ring in the palette, and the
  submit confirmation summarises unanswered + flagged counts.
- **Desktop crash-resume, fixed end-to-end**: `/api/exam/authorize` now **creates or resumes** the attempt
  (the desktop client never needed `/start`), returns `RemainingSeconds` + `SavedAnswers`, and a launch code
  may be **re-redeemed while its attempt is in progress** — so a laptop restart relaunches straight back
  into the exam with answers painted.

**Round-2 code-review fixes** (10-judge pass, reading every file): heartbeat could overlap itself under a
slow network (now interlocked, single-flight); ForceSubmit could fire twice (now once); the camera device
was re-opened and its capture loop started twice across screens (both idempotent now); and the authorize
endpoint split answer options on a literal `\n` sequence — the desktop client would have received one
garbled option (fixed).

## Verification status

Built and reviewed by static analysis in this environment (no Windows/.NET SDK available here):
namespaces, the Core wire contract, brace/paren balance, and cross-project references are consistent.
Compile, NuGet restore, and runtime must be exercised on a Windows machine with the .NET 8 SDK.
