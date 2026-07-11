using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://0.0.0.0:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}");
// Global request-body cap: bounds memory and rejects oversized uploads BEFORE the handler buffers them.
// A 3 MB artefact (Storage.MaxBytes) is ~4 MB as base64 inside a JSON data URI, so 6 MB leaves headroom
// for one legitimate upload while still refusing anything larger up front (Kestrel → 413).
builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 6_000_000);

// ---- Zero-config persistence: adopt a mounted disk at /data automatically ----
// Render (and the documented Docker run) mount the persistent disk at /data. When it exists and is
// writable, and no explicit paths were configured, the database and uploaded files go there — so
// attaching the disk in the dashboard is the ONLY step needed for durable data. Explicit
// DATABASE_FILE / STORAGE_ROOT always win, and a missing or read-only /data changes nothing.
try
{
    if (Directory.Exists("/data"))
    {
        var probe = Path.Combine("/data", ".pci-write-probe");
        File.WriteAllText(probe, "ok"); File.Delete(probe);
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_FILE")))
        {
            Environment.SetEnvironmentVariable("DATABASE_FILE", "/data/pci.db");
            Console.WriteLine("[boot] persistent disk detected at /data → DATABASE_FILE=/data/pci.db");
        }
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("STORAGE_ROOT")))
        {
            Environment.SetEnvironmentVariable("STORAGE_ROOT", "/data/storage");
            Console.WriteLine("[boot] persistent disk detected at /data → STORAGE_ROOT=/data/storage");
        }
    }
}
catch { /* /data exists but is not ours to write — keep the configured defaults */ }

// ---- DB: open + auto-migrate (BEFORE Build so the retention hosted service can depend on it) ----
var dbPath = Environment.GetEnvironmentVariable("DATABASE_FILE") ?? "./pci.db";
var db = new Db(dbPath);
// the base schema is dialect-specific: schema.mysql.sql (generated from schema.sql) for MySQL.
var schemaFile = db.Provider == Db.Kind.MySql ? "schema.mysql.sql" : "schema.sql";
var schemaPath = Path.Combine(AppContext.BaseDirectory, schemaFile);
if (!File.Exists(schemaPath)) schemaPath = schemaFile;
Console.WriteLine($"[boot] database provider: {db.Provider} (schema: {schemaFile})");
Migrate.Run(db, schemaPath);
builder.Services.AddSingleton(db);
// Scheduled retention: purge stored artefacts past evidence_retention_days, daily (manual endpoint stays).
builder.Services.AddHostedService<PCI.Backend.Core.RetentionService>();

var app = builder.Build();

// ================= security response headers + CORS (OUTERMOST middleware) =================
// Registered first so EVERY response — including the rate-limiter 429, the maintenance 503 and the
// CORS 204 preflight — carries these headers. Scoped for single-file apps with inline <script>/<style>
// ('unsafe-inline') plus the few external origins the site genuinely uses (Google Fonts, the two
// analytics hosts, cdnjs for pdf-lib). CSP enforces by default; CSP_REPORT_ONLY=true runs it report-only.
var _csp = string.Join("; ", new[]
{
    "default-src 'self'",
    "base-uri 'self'",
    "object-src 'none'",
    "frame-ancestors 'none'",
    "frame-src 'self' blob: https://accounts.google.com/gsi/",   // blob: = admin evidence viewer; gsi = Google sign-in button iframe
    "form-action 'self'",
    "img-src 'self' data: blob: https://www.googletagmanager.com",
    "media-src 'self' blob:",
    "font-src 'self' data: https://fonts.gstatic.com",
    "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://accounts.google.com/gsi/style",
    "script-src 'self' 'unsafe-inline' https://www.googletagmanager.com https://plausible.io https://cdnjs.cloudflare.com https://accounts.google.com/gsi/client",
    "connect-src 'self' https://plausible.io https://www.google-analytics.com https://accounts.google.com/gsi/",
});
var _cspHeader = string.Equals(Environment.GetEnvironmentVariable("CSP_REPORT_ONLY"), "true", StringComparison.OrdinalIgnoreCase)
    ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";
app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["X-Frame-Options"] = "DENY";                       // legacy ally of frame-ancestors 'none'
    h["Cross-Origin-Opener-Policy"] = "same-origin";
    h[_cspHeader] = _csp;
    // Emit HSTS whenever the TLS-terminating proxy reports https. Chained proxies send a comma-list
    // ("https, http"), so match the FIRST hop, not the whole header — an exact-equals check silently
    // dropped HSTS behind a second proxy.
    var xfProto = ctx.Request.Headers["X-Forwarded-Proto"].ToString().Split(',')[0].Trim();
    if (string.Equals(xfProto, "https", StringComparison.OrdinalIgnoreCase))
        h["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    await next();
});

// CORS: responses honour ALLOWED_ORIGIN only; the boot validator rejects wildcard/empty in production,
// so this reflects the single approved origin there and falls back to '*' only in development.
var _allowedOrigin = Environment.GetEnvironmentVariable("ALLOWED_ORIGIN");
app.Use(async (ctx, next) =>
{
    var res = ctx.Response;
    res.Headers["Access-Control-Allow-Origin"] = string.IsNullOrEmpty(_allowedOrigin) ? "*" : _allowedOrigin;
    if (!string.IsNullOrEmpty(_allowedOrigin)) res.Headers["Vary"] = "Origin";
    res.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
    res.Headers["Access-Control-Allow-Methods"] = "GET, POST, PATCH, DELETE, OPTIONS";
    if (ctx.Request.Method == "OPTIONS") { res.StatusCode = 204; return; }
    await next();
});

// Trusted client IP: the LAST X-Forwarded-For hop (appended by our own TLS-terminating proxy),
// falling back to the socket address. Never the first hop — that value is client-controlled and
// forgeable, which would let an attacker rotate the rate-limit bucket and spoof the audit IP.
static string ClientIp(HttpContext ctx)
{
    var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
    if (!string.IsNullOrEmpty(xff))
    {
        var parts = xff.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length > 0) return parts[^1];
    }
    return ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}

// Rate limiting: throttle brute-forceable endpoints (login, password reset, code validation, exam authorize)
// per client IP with a fixed-window in-memory counter. Applied by path prefix via middleware so it is
// robust to route-handler shape (no per-endpoint chaining required).
var _rlHits = new System.Collections.Concurrent.ConcurrentDictionary<string, (int count, long windowStart)>();
string[] _rlPaths = { "/api/login", "/api/admin/auth/login", "/api/forgot-password", "/api/validate-code", "/api/set-password", "/api/exam/authorize", "/api/register", "/api/auth/google", "/api/founding/validate", "/api/founding/redeem", "/api/me/founding-application" };
const int RL_LIMIT = 10; const long RL_WINDOW_MS = 60_000;
app.Use(async (ctx, next) =>
{
    // ASP.NET routing is trailing-slash-insensitive, so POST /api/login/ still reaches the handler.
    // Normalize the trailing slash before matching or the throttle is trivially bypassable with a "/".
    var path = (ctx.Request.Path.Value ?? "").TrimEnd('/');
    if (path.Length == 0) path = "/";
    if (ctx.Request.Method == "POST" && _rlPaths.Any(p => path.Equals(p, StringComparison.OrdinalIgnoreCase)))
    {
        // Key on the trusted proxy-appended IP (ClientIp), not the forgeable first X-Forwarded-For hop,
        // so an attacker can't rotate the rate-limit bucket per request.
        var ip = ClientIp(ctx);
        var key = ip + "|" + path;
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Evict expired windows so the map can't grow unbounded (only sweeps when it gets large).
        if (_rlHits.Count > 10_000)
            foreach (var kv in _rlHits)
                if (now - kv.Value.windowStart >= RL_WINDOW_MS) _rlHits.TryRemove(kv.Key, out _);
        var entry = _rlHits.AddOrUpdate(key, (1, now), (_, cur) =>
            now - cur.windowStart >= RL_WINDOW_MS ? (1, now) : (cur.count + 1, cur.windowStart));
        if (entry.count > RL_LIMIT)
        {
            ctx.Response.StatusCode = 429;
            ctx.Response.Headers["Retry-After"] = "60";
            await ctx.Response.WriteAsJsonAsync(new { error = "rate_limited", message = "Too many attempts. Please wait a minute and try again." });
            return;
        }
    }
    await next();
});

// Enforce the forced-password-change server-side, not just in the UI. An admin still flagged
// must_change_pw (e.g. logged in with the default bootstrap password) can reach ONLY logout,
// their own profile, and the change-password endpoint until they set a new password — so a
// direct API caller can't bypass the change the way the SPA gate can't. Read paths and the auth
// endpoints stay open so the change flow itself works.
var _mcpAllow = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{ "/api/admin/auth/login", "/api/admin/auth/logout", "/api/admin/me", "/api/admin/me/password" };
app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? "";
    if (path.StartsWith("/api/admin/", StringComparison.OrdinalIgnoreCase) && !_mcpAllow.Contains(path))
    {
        var a = Auth.AdminFromReq(ctx.Request, db);
        if (a is not null && a.MustChangePw)
        {
            ctx.Response.StatusCode = 403;
            await ctx.Response.WriteAsJsonAsync(new { error = "must_change_password", message = "Set a new password before using the console." });
            return;
        }
    }
    await next();
});

var stripeKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
if(!string.IsNullOrEmpty(stripeKey)) Stripe.StripeConfiguration.ApiKey = stripeKey;
if (string.IsNullOrEmpty(stripeKey)) Console.WriteLine("[boot] STRIPE_SECRET_KEY not set — payment endpoints will answer 503 until configured.");
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SMTP_HOST")) && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RESEND_API_KEY")))
    Console.WriteLine("[boot] no email provider configured (RESEND_API_KEY or SMTP_HOST) — emails will print to the console instead of sending.");
{
    var sp = (Environment.GetEnvironmentVariable("STORAGE_PROVIDER") ?? "local").ToLowerInvariant();
    var sr = Environment.GetEnvironmentVariable("STORAGE_ROOT") ?? "./storage";
    if (sp == "local") Console.WriteLine($"[boot] storage: local at '{sr}' (evidence/attachments stored as files; DB holds references).");
    else if (!PCI.Backend.Core.Storage.UsingLocal) Console.WriteLine($"[boot] storage: s3 bucket '{Environment.GetEnvironmentVariable("S3_BUCKET")}'" + (Environment.GetEnvironmentVariable("S3_ENDPOINT") is { } ep ? $" via endpoint '{ep}'" : "") + " (DB holds references).");
    else if (sp == "s3") Console.WriteLine($"[boot] storage: STORAGE_PROVIDER=s3 but S3_BUCKET is not set — falling back to local at '{sr}'.");
    else Console.WriteLine($"[boot] storage: unknown provider '{sp}' — falling back to local at '{sr}'. Set STORAGE_PROVIDER=local or s3.");
}

// ── Production configuration validation (Section 21): warn loudly on unsafe production config ──
// Reports every issue; in production it refuses to boot when a hard blocker is present (unless the operator
// explicitly sets ALLOW_INSECURE_PRODUCTION=true), so the app never silently runs in an unsafe state.
static List<(string sev, string key, string msg)> ConfigIssues()
{
    string? E(string k) => Environment.GetEnvironmentVariable(k);
    bool prod = string.Equals(E("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase)
             || string.Equals(E("APP_ENV"), "production", StringComparison.OrdinalIgnoreCase);
    var issues = new List<(string, string, string)>();
    void Err(string k, string m) => issues.Add(("error", k, m));
    void Warn(string k, string m) => issues.Add(("warn", k, m));
    if (prod)
    {
        var baseUrl = E("APP_BASE_URL") ?? E("SITE_BASE_URL");
        if (string.IsNullOrEmpty(baseUrl) || baseUrl.Contains("localhost") || baseUrl.Contains("127.0.0.1")) Err("APP_BASE_URL", "must be a public HTTPS URL in production");
        if (string.IsNullOrEmpty(E("STRIPE_SECRET_KEY"))) Warn("STRIPE_SECRET_KEY", "payments will be disabled (503) until set");
        else if (string.IsNullOrEmpty(E("STRIPE_WEBHOOK_SECRET"))) Err("STRIPE_WEBHOOK_SECRET", "required to verify webhooks when Stripe is enabled");
        if (string.IsNullOrEmpty(E("SMTP_HOST"))) Warn("SMTP_HOST", "emails will not send");
        if (string.Equals(E("ENABLE_LEGACY_ADMIN_TOKEN"), "true", StringComparison.OrdinalIgnoreCase)) Err("ENABLE_LEGACY_ADMIN_TOKEN", "legacy admin token must be disabled in production");
        var origin = E("ALLOWED_ORIGIN");
        if (string.IsNullOrEmpty(origin) || origin == "*") Err("ALLOWED_ORIGIN", "CORS must not be wildcard in production; set an explicit origin");
        var dbFile = E("DATABASE_FILE") ?? "./pci.db";
        if (dbFile.StartsWith("/tmp") || dbFile.Contains("Temp")) Err("DATABASE_FILE", "database path is temporary; use a persistent volume");
        if (string.IsNullOrEmpty(E("ADMIN_OWNER_PASSWORD"))) Warn("ADMIN_OWNER_PASSWORD", "bootstrap owner uses the default password until changed");
    }
    return issues;
}
{
    var issues = ConfigIssues();
    foreach (var (sev, key, msg) in issues) Console.WriteLine($"[config:{sev}] {key} — {msg}");
    var hardErrors = issues.Where(i => i.sev == "error").ToList();
    var allowInsecure = string.Equals(Environment.GetEnvironmentVariable("ALLOW_INSECURE_PRODUCTION"), "true", StringComparison.OrdinalIgnoreCase);
    if (hardErrors.Count > 0 && !allowInsecure)
    {
        Console.WriteLine($"[config] Refusing to start: {hardErrors.Count} production configuration error(s). " +
            "Fix them, or set ALLOW_INSECURE_PRODUCTION=true to override (not recommended).");
        Environment.Exit(78); // EX_CONFIG
    }
}

// ---- helpers ----
IResult Json(object o) => Results.Json(o);
void Log(long? uid, string action, string? details) =>
    db.Execute("INSERT INTO audit_logs(user_id,action,details) VALUES(?,?,?)", uid, action, details ?? "");
static async Task<Dictionary<string, JsonElement>> Body(HttpRequest r)
{
    try { return await r.ReadFromJsonAsync<Dictionary<string, JsonElement>>() ?? new(); }
    catch { return new(); }
}
static string? S(Dictionary<string, JsonElement> b, params string[] keys)
{
    foreach (var k in keys) if (b.TryGetValue(k, out var v) && v.ValueKind != JsonValueKind.Null)
        return v.ValueKind == JsonValueKind.String ? v.GetString() : v.ToString();
    return null;
}

// (security response headers + CORS are registered as the OUTERMOST middleware, right after Build,
//  so every response — including the rate-limiter 429, maintenance 503 and CORS 204 — carries them.)

// ================= maintenance mode (parity: 503 holding page; admin + APIs stay up) =================
const string MAINT_HTML = "<!doctype html><html><head><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><title>Project Controls Institute \u2014 momentarily offline</title><style>body{margin:0;min-height:100vh;display:flex;align-items:center;justify-content:center;background:#F6F8FC;font-family:Inter,system-ui,sans-serif;color:#0F172A}.c{text-align:center;padding:32px}.lp{font-weight:800;font-size:34px;color:#1D4ED8;letter-spacing:-.04em}.m{margin-top:14px;color:#64748B;max-width:420px;line-height:1.6}</style></head><body><div class=\"c\"><div class=\"lp\">PCI</div><div class=\"m\"><b>We\u2019ll be right back.</b><br/>The Project Controls Institute website is briefly offline for maintenance. Thank you for your patience.</div></div></body></html>";
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method == "GET")
    {
        var p = ctx.Request.Path.Value ?? "/";
        // The React student portal (/app, /app/*) is now a primary surface but is extension-less, so it
        // must be treated as a page for maintenance mode. The React admin (/admin, /admin/*) and the APIs
        // stay up so staff can still operate during maintenance (parity with the classic /admin.html).
        var isPage = p == "/" || p.EndsWith(".html") || p == "/app" || p.StartsWith("/app/");
        var allowed = p.StartsWith("/api") || p == "/admin.html" || p == "/admin" || p.StartsWith("/admin/");
        if (isPage && !allowed && Settings.Bool(db, "web_maintenance_mode", false))
        {
            ctx.Response.StatusCode = 503;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync(MAINT_HTML);
            return;
        }
    }
    await next();
});

// ================= health =================
app.MapGet("/api/health", () => Json(new { ok = true, service = "pci-backend", time = DateTime.UtcNow.ToString("o") }));

// Operational readiness for admins (owner-only). Reports booleans/severities only — never the secret values.
app.MapGet("/api/admin/system-check", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorised" }, statusCode: 401);
    if (!a.IsOwner) return Results.Json(new { error = "forbidden" }, statusCode: 403);
    string? E(string k) => Environment.GetEnvironmentVariable(k);
    var issues = ConfigIssues();
    var checks = new
    {
        stripe_configured = !string.IsNullOrEmpty(E("STRIPE_SECRET_KEY")),
        stripe_webhook_configured = !string.IsNullOrEmpty(E("STRIPE_WEBHOOK_SECRET")),
        smtp_configured = !string.IsNullOrEmpty(E("SMTP_HOST")),
        base_url_set = !string.IsNullOrEmpty(E("APP_BASE_URL") ?? E("SITE_BASE_URL")),
        cors_locked = !string.IsNullOrEmpty(E("ALLOWED_ORIGIN")) && E("ALLOWED_ORIGIN") != "*",
        legacy_admin_token_disabled = !string.Equals(E("ENABLE_LEGACY_ADMIN_TOKEN"), "true", StringComparison.OrdinalIgnoreCase),
        // Durable only when the DB lives on the mounted disk (/data, set by the boot autodetect) or an
        // operator-configured absolute, non-temp path. The relative default "./pci.db" sits on the
        // ephemeral container filesystem on Render and is wiped on redeploy — it must NOT report durable.
        persistent_db = (E("DATABASE_FILE") ?? "./pci.db") is { } _dbf
            && (_dbf.StartsWith("/data") || (System.IO.Path.IsPathRooted(_dbf) && !_dbf.StartsWith("/tmp") && !_dbf.Contains("Temp"))),
        owner_password_changed = db.Scalar<long>("SELECT COUNT(*) FROM admin_users WHERE email=? AND must_change_pw=0", (E("ADMIN_OWNER_EMAIL") ?? "owner@pci.local").ToLowerInvariant()) > 0,
        migrations_applied = db.Columns("exam_score_snapshots").Count > 0,   // provider-agnostic (sqlite_master is SQLite-only)
        storage_local = PCI.Backend.Core.Storage.UsingLocal,
        security_headers = true,                         // CSP + nosniff + frame-ancestors + referrer-policy
        csp_enforced = !string.Equals(E("CSP_REPORT_ONLY"), "true", StringComparison.OrdinalIgnoreCase),
        request_body_capped = true,                      // Kestrel MaxRequestBodySize = 6 MB
        retention_scheduled = true,                      // daily RetentionService hosted service
    };
    return Json(new { ok = issues.All(i => i.sev != "error"), environment = E("ASPNETCORE_ENVIRONMENT") ?? "Development", checks, issues = issues.Select(i => new { i.sev, i.key, i.msg }) });
});

// ================= public content =================
app.MapGet("/api/content", () =>
{
    var content = new Dictionary<string, object?>();
    foreach (var r in db.Query("SELECT ckey,cvalue FROM site_content")) content[(string)r["ckey"]!] = r["cvalue"];
    // Server-side result-publication policy must never be public: a candidate could otherwise read which
    // integrity blocks are disabled. These keys are used only by the backend (no client reads them), so
    // redacting them here changes no page behaviour.
    var settings = new Dictionary<string, object?>();
    foreach (var r in db.Query("SELECT skey,svalue FROM site_settings"))
    {
        var k = (string)r["skey"]!;
        if (k.StartsWith("auto_block_result", StringComparison.OrdinalIgnoreCase) || k == "critical_violation_threshold"
            || k.Contains("secret", StringComparison.OrdinalIgnoreCase) || k.Contains("smtp", StringComparison.OrdinalIgnoreCase)) continue;
        settings[k] = r["svalue"];
    }
    return Json(new
    {
        content, settings,
        faqs = db.Query("SELECT question,answer,category FROM faqs WHERE published=1 ORDER BY sort_order,id"),
        news = db.Query("SELECT title,body,published_date FROM news WHERE published=1 ORDER BY published_date DESC,id DESC"),
        resources = db.Query("SELECT title,category,doc_type,url FROM resources WHERE published=1 ORDER BY sort_order,id"),
        bok = db.Query("SELECT code,name,weight,description FROM bok_domains ORDER BY sort_order,id")
    });
});

// ================= student login =================
app.MapPost("/api/login", async (HttpRequest req) =>
{
    var b = await Body(req);
    var email = (S(b, "email") ?? "").ToLowerInvariant();
    var password = S(b, "password") ?? "";
    var u = db.QueryOne("SELECT * FROM users WHERE email=?", email);
    if (u is null || !Security.VerifyPassword(password, u["password_hash"] as string))
        return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
    if ((u["status"] as string) != "active") return Results.Json(new { error = "inactive" }, statusCode: 403);
    var session = Security.RandomHex(32);
    db.Execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+30 day'))",
        u["id"], Security.Sha(session));
    try {
        var ua = req.Headers.UserAgent.ToString();
        var dev = System.Text.RegularExpressions.Regex.IsMatch(ua, "Mobile|iPhone|Android") ? "Mobile"
                : System.Text.RegularExpressions.Regex.IsMatch(ua, "iPad|Tablet") ? "Tablet" : "Desktop";
        var ip = H.LastHopIp(req.Headers["x-forwarded-for"].ToString(), req.HttpContext.Connection.RemoteIpAddress?.ToString());
        db.Execute("INSERT INTO login_events(user_id,ip,user_agent,device,outcome) VALUES(?,?,?,?,?)", u["id"], ip, ua.Length>300?ua[..300]:ua, dev, "success");
    } catch { }
    return Json(new { ok = true, token = session, user = new { id = u["id"], email = u["email"], firstName = u["first_name"], lastName = u["last_name"] } });
});

// Student sign-out: revoke the presented session token server-side (mirrors the admin logout, so a
// captured token stops working immediately rather than staying valid for its 30-day lifetime).
app.MapPost("/api/logout", (HttpRequest req) =>
{
    var h = req.Headers.Authorization.ToString();
    if (h.StartsWith("Bearer ")) db.Execute("DELETE FROM login_tokens WHERE token=? AND purpose='session'", Security.Sha(h.Substring(7)));
    return Json(new { ok = true });
});

// ================= admin auth (login/logout/me/password) =================
app.MapPost("/api/admin/auth/login", async (HttpRequest req) =>
{
    var b = await Body(req);
    var email = (S(b, "email") ?? "").Trim().ToLowerInvariant();
    var password = S(b, "password") ?? "";
    var a = db.QueryOne("SELECT * FROM admin_users WHERE lower(email)=?", email);
    if (a is null || (a["status"] as string) != "active" || !Security.VerifyPassword(password, a["password_hash"] as string))
        return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
    var tok = Security.RandomHex(24);
    db.Execute("INSERT INTO admin_sessions(admin_id,token,expires_at) VALUES(?,?,datetime('now','+12 hours'))", a["id"], Security.Sha(tok));
    db.Execute("UPDATE admin_users SET last_login_at=datetime('now') WHERE id=?", a["id"]);
    var ctx = Auth.ToAdmin(a);
    Log(0, "admin_login", ctx.Email);
    return Json(new { token = tok, admin = new { id = ctx.Id, email = ctx.Email, name = ctx.Name, role = ctx.Role, must_change_pw = ctx.MustChangePw }, permissions = ctx.Perms });
});

app.MapPost("/api/admin/auth/logout", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var h = req.Headers.Authorization.ToString();
    if (h.StartsWith("Bearer ")) db.Execute("DELETE FROM admin_sessions WHERE token=?", Security.Sha(h.Substring(7)));
    return Json(new { ok = true });
});

app.MapGet("/api/admin/me", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    return Json(new { id = a.Id, email = a.Email, name = a.Name, role = a.Role, is_owner = a.IsOwner, must_change_pw = a.MustChangePw, permissions = a.Perms });
});

app.MapPost("/api/admin/me/password", async (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    if (a.Id == 0) return Results.Json(new { error = "shared_token_cannot_set_pw" }, statusCode: 400);
    var b = await Body(req);
    var np = S(b, "new_password") ?? "";
    if (np.Length < 8) return Results.Json(new { error = "weak_password" }, statusCode: 400);
    db.Execute("UPDATE admin_users SET password_hash=?, must_change_pw=0 WHERE id=?", BCrypt.Net.BCrypt.HashPassword(np), a.Id);
    // Changing the password revokes every OTHER admin session for this account (a stolen session must
    // not survive a password change), while preserving the caller's current session so the console
    // stays usable. The current bearer is kept by its hash.
    var authHeader = req.Headers.Authorization.ToString();
    var curTok = authHeader.StartsWith("Bearer ") ? Security.Sha(authHeader.Substring(7)) : "";
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=? AND token<>?", a.Id, curTok);
    return Json(new { ok = true });
});

// ================= Team & Access (owner only) =================
IResult? OwnerGate(HttpRequest req, out AdminCtx? a)
{
    a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    if (!a.IsOwner) return Results.Json(new { error = "owner_only" }, statusCode: 403);
    return null;
}

app.MapGet("/api/admin/team", (HttpRequest req) =>
{
    var gate = OwnerGate(req, out _); if (gate is not null) return gate;
    var rows = db.Query("SELECT id,email,name,role,permissions,status,must_change_pw,last_login_at,created_at FROM admin_users ORDER BY id ASC")
        .Select(a => {
            var perms = new List<string>();
            try { perms = JsonSerializer.Deserialize<List<string>>(a["permissions"] as string ?? "[]") ?? new(); } catch { }
            return new Dictionary<string, object?>(a) { ["permissions"] = perms, ["effective"] = Rbac.PermsFor((string)a["role"]!, a["permissions"] as string) };
        }).ToList();
    // sections is a FLAT list of every permission key (the admin Team permission-picker maps over it).
    // Rbac.Sections is a grouped dictionary; returning it here serialised to an object and crashed the
    // picker's sections.map(). AllSections is the flattened string[] the client expects.
    return Json(new { rows, roles = Rbac.RoleGrants.Keys.Append("custom").ToArray(), sections = Rbac.AllSections, role_grants = Rbac.RoleGrants });
});

app.MapPost("/api/admin/team", async (HttpRequest req) =>
{
    var gate = OwnerGate(req, out var admin); if (gate is not null) return gate;
    var b = await Body(req);
    var email = (S(b, "email") ?? "").Trim().ToLowerInvariant();
    if (email.Length == 0 || !System.Text.RegularExpressions.Regex.IsMatch(email, ".+@.+\\..+"))
        return Results.Json(new { error = "valid_email_required" }, statusCode: 400);
    if (db.QueryOne("SELECT 1 FROM admin_users WHERE lower(email)=?", email) is not null)
        return Results.Json(new { error = "email_exists" }, statusCode: 400);
    var role = S(b, "role") ?? "viewer";
    var perms = "[]";
    if (b.TryGetValue("permissions", out var pv) && pv.ValueKind == JsonValueKind.Array) perms = pv.GetRawText();
    var tempPw = S(b, "password") ?? Security.RandomHex(5);
    var id = db.ExecuteReturningId("INSERT INTO admin_users(email,name,password_hash,role,permissions,status,must_change_pw,created_by) VALUES(?,?,?,?,?, 'active',1,?)",
        email, S(b, "name") ?? "", BCrypt.Net.BCrypt.HashPassword(tempPw), role, perms, admin!.Id == 0 ? (object?)null : admin.Id);
    Log(0, "admin_created", $"{email} ({role})");
    return Json(new { ok = true, id, temp_password = tempPw });
});

app.MapPatch("/api/admin/team/{id}", async (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out _); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    var b = await Body(req);
    var sets = new List<string>(); var vals = new List<object?>();
    if (b.ContainsKey("name")) { sets.Add("name=?"); vals.Add(S(b, "name")); }
    if (b.ContainsKey("role")) { sets.Add("role=?"); vals.Add(S(b, "role")); }
    if (b.TryGetValue("permissions", out var pv) && pv.ValueKind == JsonValueKind.Array) { sets.Add("permissions=?"); vals.Add(pv.GetRawText()); }
    if (b.TryGetValue("status", out var sv))
    {
        var status = sv.GetString();
        if (status == "suspended" && (a["role"] as string) == "owner")
        {
            var owners = db.Scalar<long>("SELECT COUNT(*) FROM admin_users WHERE role='owner' AND status='active'");
            if (owners <= 1) return Results.Json(new { error = "cannot_suspend_last_owner" }, statusCode: 400);
        }
        sets.Add("status=?"); vals.Add(status);
    }
    var newRole = S(b, "role");
    if (newRole is not null && newRole != "owner" && (a["role"] as string) == "owner")
    {
        var owners = db.Scalar<long>("SELECT COUNT(*) FROM admin_users WHERE role='owner' AND status='active'");
        if (owners <= 1) return Results.Json(new { error = "cannot_demote_last_owner" }, statusCode: 400);
    }
    if (sets.Count > 0) { vals.Add(id); db.Execute($"UPDATE admin_users SET {string.Join(", ", sets)} WHERE id=?", vals.ToArray()); }
    Log(0, "admin_updated", a["email"] as string);
    return Json(new { ok = true });
});

app.MapPost("/api/admin/team/{id}/reset-password", (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out _); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    var tempPw = Security.RandomHex(5);
    db.Execute("UPDATE admin_users SET password_hash=?, must_change_pw=1 WHERE id=?", BCrypt.Net.BCrypt.HashPassword(tempPw), id);
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
    Log(0, "admin_pw_reset", a["email"] as string);
    return Json(new { ok = true, temp_password = tempPw });
});

app.MapDelete("/api/admin/team/{id}", (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out _); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    if ((a["role"] as string) == "owner")
    {
        var owners = db.Scalar<long>("SELECT COUNT(*) FROM admin_users WHERE role='owner' AND status='active'");
        if (owners <= 1) return Results.Json(new { error = "cannot_delete_last_owner" }, statusCode: 400);
    }
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
    db.Execute("DELETE FROM admin_users WHERE id=?", id);
    Log(0, "admin_deleted", a["email"] as string);
    return Json(new { ok = true });
});

// ================= settings GET + gated PATCH =================
app.MapGet("/api/admin/settings", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    // Read is scoped to the SAME per-key permissions the PATCH enforces: web_* → set_web, sp_* → set_sp,
    // exam_* → set_exam, everything else (platform settings) → the owner-only 'settings' perm. Previously
    // any authenticated admin (even a viewer) could read every section's config, asymmetric with the
    // deny-by-default write.
    var perms = a.Perms; var owner = a.IsOwner;
    bool KeyReadable(string k) => owner
        || (k.StartsWith("web_") ? perms.Contains("set_web")
            : k.StartsWith("sp_") ? perms.Contains("set_sp")
            : k.StartsWith("exam_") ? perms.Contains("set_exam") : perms.Contains("settings"));
    var o = new Dictionary<string, object?>();
    foreach (var r in db.Query("SELECT skey,svalue FROM site_settings"))
    { var k = (string)r["skey"]!; if (KeyReadable(k)) o[k] = r["svalue"]; }
    return Json(o);
});

app.MapPatch("/api/admin/settings", async (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var perms = a.Perms; var owner = a.IsOwner;
    // Deny-by-default: prefixed keys map to their section permission; every OTHER key is a platform-level
    // setting (auto_block_result_on_*, critical_violation_threshold, evidence_retention_days, …) that now
    // drives automated result-holding and artefact deletion, so it requires the owner-only 'settings'
    // permission. Previously the fallthrough was ': true', letting ANY admin (even a viewer) write them.
    bool KeyAllowed(string k) => owner
        || (k.StartsWith("web_") ? perms.Contains("set_web")
            : k.StartsWith("sp_") ? perms.Contains("set_sp")
            : k.StartsWith("exam_") ? perms.Contains("set_exam") : perms.Contains("settings"));
    var b = await Body(req);
    var rejected = new List<string>();
    foreach (var kv in b)
    {
        if (KeyAllowed(kv.Key))
            db.Execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?) ON CONFLICT(skey) DO UPDATE SET svalue=excluded.svalue",
                kv.Key, kv.Value.ValueKind == JsonValueKind.String ? kv.Value.GetString() : kv.Value.ToString());
        else rejected.Add(kv.Key);
    }
    PCI.Backend.Core.PageContent.Bump();   // announcement banner + any content-affecting settings
    PCI.Backend.Core.CertCatalogue.Bump(); // exam_* settings feed the public catalogue's effective prices
    PCI.Backend.Core.PriceTags.Bump();     // pricing-affecting settings flow into page price tokens
    Log(null, "settings_update", string.Join(",", b.Keys));
    return rejected.Count > 0 ? Json(new { ok = true, rejected }) : Json(new { ok = true });
});

// ---- shared gate delegate for endpoint modules + per-section gated admin reads ----
IResult GateFn(HttpRequest req, string section, Func<AdminCtx, IResult> ok)
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    if (!a.IsOwner && !a.Perms.Contains(section)) return Results.Json(new { error = "forbidden", section }, statusCode: 403);
    return ok(a);
}
Action<long?, string, string?> logFn = (uid, action, details) =>
    db.Execute("INSERT INTO audit_logs(user_id,action,details) VALUES(?,?,?)", uid, action, details ?? "");

// ---- register ported endpoint modules ----
PCI.Backend.Endpoints.StudentExam.InitScorer(db);
PCI.Backend.Endpoints.StudentExam.Map(app, db, logFn);
PCI.Backend.Endpoints.ExamClient.Map(app, db, logFn);
PCI.Backend.Endpoints.AdminProctoring.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.AdminStudents.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.Public.Map(app, db, logFn);
PCI.Backend.Endpoints.Account.Map(app, db, logFn);
PCI.Backend.Endpoints.AdminMgmt.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Payments.Map(app, db, logFn, () => !string.IsNullOrEmpty(stripeKey));
PCI.Backend.Endpoints.AdminExtra.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Reviews.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Casework.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Founding.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.Honorary.Map(app, db, logFn);

// Purge stored artefacts older than the configured retention window (owner-only). Metadata rows are kept
// for audit; only the binary artefacts are removed once past retention.
app.MapPost("/api/admin/storage/purge", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorised" }, statusCode: 401);
    if (!a.IsOwner) return Results.Json(new { error = "forbidden" }, statusCode: 403);
    var days = (int)Settings.Num(db, "evidence_retention_days", 365);
    var removed = PCI.Backend.Core.Storage.PurgeOlderThan(days);
    logFn(a.Id, "storage_purge", $"{removed} artefacts older than {days}d");
    return Json(new { ok = true, removed, retention_days = days });
});

// ================= admin overview (needed for the panel landing) =================

// ============ dynamic content injection (Stage 2) — before static files ============
// For a content page with DB overrides (editable title / meta description / data-cms regions), serve
// the HTML with those values injected server-side (SEO-safe, works with JS off). Pages with no
// overrides fall straight through to the static-file middleware, so untouched pages pay nothing.
var webRoot = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
// one-time: capture each page's current headline as an editable block so every page is editable out of the box
PCI.Backend.Core.PageContent.SeedFromFiles(db, webRoot);
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method == "GET")
    {
        var reqPath = ctx.Request.Path.Value ?? "/";
        var slug = reqPath == "/" ? "index.html" : reqPath.TrimStart('/');
        var isPage = slug.EndsWith(".html", StringComparison.OrdinalIgnoreCase) && !slug.Contains("..");
        var hasContent = isPage && PCI.Backend.Core.PageContent.HasOverrides(db, slug);
        var hasCerts = isPage && PCI.Backend.Core.CertCatalogue.Applies(slug);
        if (hasContent || hasCerts)
        {
            var file = Path.Combine(webRoot, slug.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(file))
            {
                // Content injection (universal text blocks/title/meta/data-cms/_h1) is cached per
                // (slug, content version); the certification catalogue and the table-backed sections
                // (nav, FAQs, BoK, governance, resources, news) are then filled from the DB — each
                // cached per its own version, so steady-state serving stays cheap.
                var rendered = hasContent
                    ? PCI.Backend.Core.PageContent.Render(db, slug, () => File.ReadAllText(file))
                    : File.ReadAllText(file);
                if (hasCerts) rendered = PCI.Backend.Core.CertCatalogue.Inject(db, rendered);
                rendered = PCI.Backend.Core.ListSections.Inject(db, rendered);
                rendered = PCI.Backend.Core.PriceTags.Inject(db, rendered);
                ctx.Response.ContentType = "text/html; charset=utf-8";
                ctx.Response.Headers.CacheControl = "no-cache";   // content is admin-editable — always revalidate
                await ctx.Response.WriteAsync(rendered);
                return;
            }
        }
        // the site-search index follows content edits: titles/descriptions come from the pages table
        if (reqPath.Equals("/search-index.json", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "application/json; charset=utf-8";
            await ctx.Response.WriteAsync(PCI.Backend.Core.SearchIndex.Json(db, webRoot));
            return;
        }
    }
    await next();
});

// ================= static site (all four apps) — LAST so /api wins =================
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
// Explicit cache policy — without it, mobile browsers heuristically cache the SPA shells, and after
// a deploy the cached HTML points at content-hashed CSS/JS that no longer exists → a completely
// unstyled portal until the user hard-refreshes. HTML always revalidates (ETag → cheap 304s);
// hashed SPA assets are immutable; unhashed site css/js revalidate; images cache for a day.
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = sf =>
    {
        var path = sf.Context.Request.Path.Value ?? "";
        var h = sf.Context.Response.Headers;
        if (path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            h.CacheControl = "no-cache";
        else if (path.StartsWith("/app/assets/", StringComparison.OrdinalIgnoreCase) ||
                 path.StartsWith("/admin/assets/", StringComparison.OrdinalIgnoreCase))
            h.CacheControl = "public, max-age=31536000, immutable";
        else if (path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
                 path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
            h.CacheControl = "no-cache";
        else
            h.CacheControl = "public, max-age=86400";
    }
});

// ================= React SPAs (Stage 3) — client-side routing fallback =================
// The built React apps live under wwwroot/app (student portal) and wwwroot/admin (admin console).
// This is TERMINAL MIDDLEWARE placed AFTER UseStaticFiles on purpose: real asset requests
// (/app/assets/*.js, /admin/assets/*.css, …) are served — and short-circuited — by static files above,
// so they never reach here. Only an extension-less GET under a mount (a client-side route like
// /app/cpd or /admin/students) falls through, and we return that app's shell for React Router to
// resolve. A missing asset still 404s (extension guard). /api/* (incl. /api/admin/*) starts with
// /api, matches no mount, and reaches its endpoint. The classic .html portals keep their extension
// and are served by static files, so /admin.html and /student.html are untouched.
var spaMounts = new[]
{
    ("/app", Path.Combine(webRoot, "app", "index.html")),
    ("/admin", Path.Combine(webRoot, "admin", "index.html")),
};
app.Use(async (ctx, next) =>
{
    var p = ctx.Request.Path.Value ?? "";
    if (ctx.Request.Method == "GET" && !Path.HasExtension(p))
    {
        foreach (var (prefix, index) in spaMounts)
        {
            if ((p == prefix || p.StartsWith(prefix + "/", StringComparison.OrdinalIgnoreCase)) && File.Exists(index))
            {
                ctx.Response.ContentType = "text/html; charset=utf-8";
                ctx.Response.Headers.CacheControl = "no-cache";   // the shell must revalidate every deploy
                await ctx.Response.SendFileAsync(index);
                return;
            }
        }
    }
    await next();
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var u = $"http://localhost:{port}";
Console.WriteLine("┌──────────────────────────────────────────────────────┐");
Console.WriteLine("│  Project Controls Institute — platform is running    │");
Console.WriteLine("├──────────────────────────────────────────────────────┤");
Console.WriteLine($"│  Website        {u}/");
Console.WriteLine($"│  Student (React){u}/app/");
Console.WriteLine($"│  Admin (React)  {u}/admin/");
Console.WriteLine($"│  Student Panel  {u}/student.html");
Console.WriteLine($"│  Admin Panel    {u}/admin.html");
Console.WriteLine($"│  Exam preview   {u}/exam-ui.html");
Console.WriteLine("└──────────────────────────────────────────────────────┘");

app.Run();
