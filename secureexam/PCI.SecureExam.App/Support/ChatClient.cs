using Microsoft.AspNetCore.SignalR.Client;
using PCI.SecureExam.Core.Models;

namespace PCI.SecureExam.App.Support;

/// <summary>
/// Real-time in-exam support chat to the PCI proctor desk, over SignalR. Available throughout the
/// sitting without leaving the kiosk. Auto-reconnects; queued messages flush on reconnect.
/// </summary>
public sealed class ChatClient : IAsyncDisposable
{
    private readonly HubConnection _hub;
    private readonly string _attempt;
    public event Action<ChatMessage>? MessageReceived;
    public event Action<bool>? ConnectionChanged;

    public ChatClient(string baseUrl, string sessionToken, string attemptToken, string hubPath = "/hubs/proctor")
    {
        _attempt = attemptToken;
        var path = string.IsNullOrWhiteSpace(hubPath) ? "/hubs/proctor" : (hubPath.StartsWith('/') ? hubPath : "/" + hubPath);
        _hub = new HubConnectionBuilder()
            .WithUrl($"{baseUrl.TrimEnd('/')}{path}?access_token={sessionToken}")
            .WithAutomaticReconnect()
            .Build();

        _hub.On<ChatMessage>("ReceiveMessage", m => MessageReceived?.Invoke(m));
        _hub.Reconnected += _ => { ConnectionChanged?.Invoke(true); return Task.CompletedTask; };
        _hub.Closed += _ => { ConnectionChanged?.Invoke(false); return Task.CompletedTask; };
    }

    public async Task ConnectAsync()
    {
        await _hub.StartAsync();
        await _hub.InvokeAsync("JoinExam", _attempt);
        ConnectionChanged?.Invoke(true);
    }

    public async Task SendAsync(string body)
        => await _hub.InvokeAsync("SendToProctor", _attempt, body);

    public async ValueTask DisposeAsync() => await _hub.DisposeAsync();
}
