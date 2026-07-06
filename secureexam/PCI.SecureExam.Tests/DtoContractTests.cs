using System.Text.Json;
using PCI.SecureExam.Core.Models;
using Xunit;

namespace PCI.SecureExam.Tests;

public class DtoContractTests
{
    [Fact]
    public void HeartbeatRequest_round_trips_with_events()
    {
        var req = new HeartbeatRequest("tok", new Dictionary<int,int>{{1,2}}, 3,
            new[]{ new ProctorEvent(ProctorEventType.FocusLost, ProctorSeverity.Medium, DateTimeOffset.UtcNow, "alt-tab") });
        var json = JsonSerializer.Serialize(req);
        var back = JsonSerializer.Deserialize<HeartbeatRequest>(json)!;
        Assert.Equal("tok", back.AttemptToken);
        Assert.Equal(2, back.Answers[1]);
        Assert.Single(back.PendingEvents);
        Assert.Equal(ProctorEventType.FocusLost, back.PendingEvents[0].Type);
    }

    [Fact]
    public void SubmitResult_deserializes_case_insensitively()
    {
        // server may send camelCase; client reads with PropertyNameCaseInsensitive
        var json = """{"ok":true,"percent":82.0,"result":"pass","credentialId":"PCP-AI-2026-1","breakdown":[]}""";
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var r = JsonSerializer.Deserialize<SubmitResult>(json, opts)!;
        Assert.True(r.Ok);
        Assert.Equal("pass", r.Result);
        Assert.Equal("PCP-AI-2026-1", r.CredentialId);
    }

    [Fact]
    public void HeartbeatResponse_delivers_proctor_messages()
    {
        var json = """{"ok":true,"serverTime":"2026-07-04T10:00:00Z","deadline":"2026-07-04T11:30:00Z","remainingSeconds":5000,"forceSubmit":false,"messages":[{"from":"PCI Support","body":"Keep your ID visible.","at":"2026-07-04T10:01:00Z"}]}""";
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var r = JsonSerializer.Deserialize<HeartbeatResponse>(json, opts)!;
        Assert.NotNull(r.Messages);
        Assert.Single(r.Messages!);
        Assert.Equal("PCI Support", r.Messages![0].From);
    }

    [Fact]
    public void HeartbeatRequest_carries_candidate_chat()
    {
        var req = new HeartbeatRequest("tok", new(), 0, Array.Empty<ProctorEvent>(), new[]{ "Hello proctor" });
        var back = JsonSerializer.Deserialize<HeartbeatRequest>(JsonSerializer.Serialize(req))!;
        Assert.NotNull(back.ChatOut);
        Assert.Equal("Hello proctor", back.ChatOut![0]);
    }

    [Fact]
    public void ExamAuthorization_resume_fields_are_optional()
    {
        var json = """{"attemptToken":"PCI-X","candidateName":"A","candidateEmail":"a@e.com","examCode":"PCP-AI","examTitle":"T","durationMinutes":90,"scheduledAt":"2026-07-04T10:00:00Z","opensAt":"2026-07-04T09:45:00Z","closesAt":"2026-07-04T10:30:00Z","requiresIdentity":true,"requiresRoomScan":true,"items":[],"remainingSeconds":4200,"savedAnswers":{"3":1},"resumed":true}""";
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var a = JsonSerializer.Deserialize<ExamAuthorization>(json, opts)!;
        Assert.Equal(4200, a.RemainingSeconds);
        Assert.True(a.Resumed);
        Assert.Equal(1, a.SavedAnswers![3]);
    }
}
