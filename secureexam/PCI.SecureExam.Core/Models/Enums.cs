namespace PCI.SecureExam.Core.Models;

public enum ExamScreen { Launch, Consent, SystemCheck, IdentityCapture, RoomScan, Rules, Exam, Submitted, Terminated }

public enum ProctorSeverity { Info, Low, Medium, High, Critical }

/// <summary>Every integrity signal the client can raise. Streamed to the server, time-stamped, and compiled into the incident report.</summary>
public enum ProctorEventType
{
    SessionStarted, IdentityVerified, IdentityFailed, RoomScanCompleted,
    FocusLost, FocusRegained, ProhibitedProcess, SecondDisplayConnected, DisplayRemoved,
    VirtualMachineDetected, RemoteDesktopDetected, ClipboardBlocked, HotkeyBlocked,
    NoFaceDetected, MultipleFacesDetected, FaceAbsentTooLong, LookedAwayRepeatedly,
    LoudAudioDetected, SpeechDetected, PhoneLikeObjectDetected,
    NetworkDropped, NetworkRestored, ClientCrashedResumed, HeartbeatMissed,
    AnswerSaved, ExamSubmitted, ManualFlagByProctor, ChatMessage
}

public enum SystemCheckStatus { Pass, Warn, Fail }
public enum IdentityResult { Verified, MismatchLikely, NoFace, Inconclusive }
