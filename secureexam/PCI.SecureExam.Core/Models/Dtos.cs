namespace PCI.SecureExam.Core.Models;

public record ExamAuthorization(
    string AttemptToken, string CandidateName, string CandidateEmail,
    string ExamCode, string ExamTitle, int DurationMinutes,
    DateTimeOffset ScheduledAt, DateTimeOffset OpensAt, DateTimeOffset ClosesAt,
    bool RequiresIdentity, bool RequiresRoomScan, IReadOnlyList<ExamItem> Items,
    int? RemainingSeconds = null, Dictionary<int,int>? SavedAnswers = null, bool Resumed = false,
    string? SessionToken = null);

public record ExamItem(int Id, string Domain, string Question, IReadOnlyList<string> Options);

public record SystemCheckResult(string Name, SystemCheckStatus Status, string Detail);

public record HeartbeatRequest(string AttemptToken, Dictionary<int,int> Answers, int Violations, IReadOnlyList<ProctorEvent> PendingEvents, IReadOnlyList<string>? ChatOut = null);
public record HeartbeatResponse(bool Ok, DateTimeOffset ServerTime, DateTimeOffset Deadline, int RemainingSeconds, bool ForceSubmit, IReadOnlyList<ChatMessage>? Messages = null);

public record ProctorEvent(ProctorEventType Type, ProctorSeverity Severity, DateTimeOffset At, string? Detail = null, string? EvidenceRef = null);

public record IdentityCheck(IdentityResult Result, double Confidence, string? FacePhotoRef, string? IdPhotoRef, string? Note);

public record SubmitRequest(string AttemptToken, Dictionary<int,int> Answers, int Violations);
public record SubmitResult(bool Ok, double Percent, string Result, string? CredentialId, IReadOnlyList<DomainBand> Breakdown, bool Held = false, string? Message = null, string? ResultStatus = null);
public record DomainBand(string Domain, int Percent, string Band);

public record ChatMessage(string From, string Body, DateTimeOffset At);
