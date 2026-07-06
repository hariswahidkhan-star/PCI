using Microsoft.AspNetCore.SignalR;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.Server.Hubs;

/// <summary>
/// Realtime bridge between candidates (in the secure client) and the PCI proctor desk.
///   • Candidate calls JoinExam(attempt) -> added to a per-attempt group.
///   • Candidate calls SendToProctor(attempt, body) -> echoed to their group AND to the "proctors" group.
///   • A proctor console joins "proctors" (JoinProctorConsole) and can SendToCandidate(attempt, body).
/// Messages are ChatMessage records so both ends share one shape with the Core library.
/// </summary>
public sealed class ProctorHub : Hub
{
    public async Task JoinExam(string attempt)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Group(attempt));
        await Clients.Group("proctors").SendAsync("CandidateJoined", attempt);
    }

    public async Task JoinProctorConsole()
        => await Groups.AddToGroupAsync(Context.ConnectionId, "proctors");

    public async Task SendToProctor(string attempt, string body)
    {
        var msg = new ChatMessage("You", body, DateTimeOffset.UtcNow);           // as the candidate sees it
        await Clients.Caller.SendAsync("ReceiveMessage", msg);
        var toProctor = new ChatMessage(attempt, body, DateTimeOffset.UtcNow);   // proctor sees attempt id
        await Clients.Group("proctors").SendAsync("ReceiveMessage", toProctor);
    }

    public async Task SendToCandidate(string attempt, string body)
    {
        var msg = new ChatMessage("PCI Support", body, DateTimeOffset.UtcNow);
        await Clients.Group(Group(attempt)).SendAsync("ReceiveMessage", msg);
    }

    /// <summary>Proctor raises a manual flag against an attempt; surfaced to the candidate's event stream.</summary>
    public async Task RaiseManualFlag(string attempt, string note)
        => await Clients.Group(Group(attempt)).SendAsync("ManualFlag",
            new ProctorEvent(ProctorEventType.ManualFlagByProctor, ProctorSeverity.High, DateTimeOffset.UtcNow, note));

    private static string Group(string attempt) => $"exam:{attempt}";
}
