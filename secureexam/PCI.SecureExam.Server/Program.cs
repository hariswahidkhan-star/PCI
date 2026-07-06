using PCI.SecureExam.Server.Hubs;

// Reference proctoring/launch server for the PCI Secure Exam client.
// In production this sits alongside (or behind the same gateway as) the existing PCI Node backend:
//   • POST /api/exam/authorize   validates a one-time launch code -> ExamAuthorization (+ items)
//   • POST /api/exam/evidence    receives webcam/room-scan snapshots (multipart)
//   • POST /api/exam/identity    receives the AI identity-check result
//   • /hubs/proctor              SignalR hub for in-exam chat + live proctor console
// The exam clock/scoring endpoints (/api/me/exam/{start,heartbeat,submit}) are already implemented in
// the Node backend; this service only adds the launch + evidence + realtime pieces, so both can run
// together. Scoring is never done client-side.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSingleton<LaunchStore>();
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "https://portal.projectcontrolsinstitute.org", "http://localhost:3000" };
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.WithOrigins(allowedOrigins)
     .AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();
app.UseCors();
app.MapControllers();
app.MapHub<ProctorHub>("/hubs/proctor");
app.MapGet("/", () => "PCI Secure Exam server. See /api/exam/authorize.");
app.Run();
