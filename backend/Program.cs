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
// Deterministic browser-lifecycle tests need to book a policy-valid future slot and launch it
// immediately. This override is deliberately Development-only and opt-in; production continues to
// read the operator-controlled site setting with no test back door or public mutation endpoint.
if (builder.Environment.IsDevelopment()
    && double.TryParse(Environment.GetEnvironmentVariable("E2E_EXAM_OPEN_BEFORE_MINUTES"), out var e2eOpenBefore)
    && e2eOpenBefore > 0)
    Settings.Put(db, "exam_open_before_minutes", e2eOpenBefore.ToString(System.Globalization.CultureInfo.InvariantCulture));
try { PCI.Backend.Data.CommsSeed.Ensure(db); } catch (Exception e) { Console.Error.WriteLine($"[comms seed] {e.Message}"); }
try { PCI.Backend.Data.MarketingSchema.Ensure(db); } catch (Exception e) { Console.Error.WriteLine($"[marketing schema] {e.Message}"); }
builder.Services.AddSingleton(db);
// Scheduled retention: purge stored artefacts past evidence_retention_days, daily (manual endpoint stays).
builder.Services.AddHostedService<PCI.Backend.Core.RetentionService>();
// ERP / integrations outbox delivery worker (Phase 9): drains due deliveries and posts them, signed.
builder.Services.AddHostedService<PCI.Backend.Core.IntegrationDispatcher>();
// Communications outbox worker: drains comm_outbox (email/WhatsApp/in-app) with retries + backoff.
builder.Services.AddHostedService<PCI.Backend.Core.OutboxDispatcher>();
builder.Services.AddHostedService<PCI.Backend.Core.SocialDispatcher>();   // Phase 2: social publish outbox
builder.Services.AddHostedService<PCI.Backend.Core.ScheduledPublisher>();   // Content Centre: publish scheduled blog posts when due
builder.Services.AddHostedService<PCI.Backend.Core.SyndicationDispatcher>();   // Phase 3: syndication outbox
builder.Services.AddHostedService<PCI.Backend.Core.MarketingJobDispatcher>();   // Marketing Phase 2: provider job queue
builder.Services.AddHostedService<PCI.Backend.Core.CommsReminderService>();     // Comms §13: scheduled reminder sequences

var app = builder.Build();

// Friendly, branded 404 for unknown WEBSITE pages. UseStatusCodePages fires only when a downstream
// handler produced a 404 WITHOUT writing a body, so it never touches the JSON 404s from /api/* (those
// write their own body → HasStarted) nor any real static file. A GET for an .html/extension-less path
// that matched no endpoint, static file or SPA mount gets 404.html with a proper 404 status instead of
// the browser's blank error page.
app.UseStatusCodePages(async context =>
{
    var ctx = context.HttpContext;
    if (ctx.Response.StatusCode != 404 || ctx.Response.HasStarted || ctx.Request.Method != "GET") return;
    if (ctx.Request.Path.StartsWithSegments("/api")) return;
    var ext = Path.GetExtension(ctx.Request.Path.Value ?? "");
    var wantsHtml = ext.Length == 0 || ext.Equals(".html", StringComparison.OrdinalIgnoreCase)
                    || ctx.Request.Headers.Accept.ToString().Contains("text/html", StringComparison.OrdinalIgnoreCase);
    if (!wantsHtml) return;
    var root = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
    var page = Path.Combine(root, "404.html");
    if (File.Exists(page))
    {
        ctx.Response.ContentType = "text/html; charset=utf-8";
        await ctx.Response.SendFileAsync(page);
    }
});

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
    "script-src 'self' 'unsafe-inline' https://www.googletagmanager.com https://plausible.io https://cdnjs.cloudflare.com https://accounts.google.com/gsi/client https://www.clarity.ms",
    "connect-src 'self' https://plausible.io https://www.google-analytics.com https://accounts.google.com/gsi/ https://*.clarity.ms",
});
var _cspHeader = string.Equals(Environment.GetEnvironmentVariable("CSP_REPORT_ONLY"), "true", StringComparison.OrdinalIgnoreCase)
    ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";
// Error-reference net (outermost): any unhandled API exception becomes a logged error_report with a
// PCI-YYYY-NNNNNN reference and a clean JSON body the student can quote to support — never a stack
// trace. Website pages keep the framework's default handling (branded 404/500 pages).
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex) when (ctx.Request.Path.StartsWithSegments("/api"))
    {
        var reference = PCI.Backend.Core.ErrorRefs.Capture(db, null, ctx.Request.Path.ToString(), "server",
            "We could not complete your request.", $"{ex.GetType().Name}: {ex.Message}", ctx.Request.Headers.UserAgent.ToString());
        Console.Error.WriteLine($"[error {reference}] {ctx.Request.Method} {ctx.Request.Path}: {ex}");
        if (!ctx.Response.HasStarted)
        {
            ctx.Response.StatusCode = 500;
            await ctx.Response.WriteAsJsonAsync(new { error = "server_error", reference,
                message = $"We could not complete your request. Please contact support and quote Error Reference: {reference}." });
        }
    }
});

// Canonical-domain + HTTPS enforcement (FIRST, before anything serves): 301 the www/pciglobal.ai
// hosts to https://projectcontrolsinstitute.org, page-to-page. Unknown hosts pass through so the
// service keeps working on the Render URL / localhost / staging during the DNS transition.
app.Use(async (ctx, next) =>
{
    if (PCI.Backend.Core.Redirects.Target(ctx.Request) is { } target)
    {
        ctx.Response.StatusCode = StatusCodes.Status301MovedPermanently;
        ctx.Response.Headers.Location = target;
        return;
    }
    await next();
});

app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h["X-Content-Type-Options"] = "nosniff";
    h["Referrer-Policy"] = "strict-origin-when-cross-origin";
    h["X-Frame-Options"] = "DENY";                       // legacy ally of frame-ancestors 'none'
    h["Cross-Origin-Opener-Policy"] = "same-origin";
    // Deny powerful browser features site-wide except where actually used (payment on checkout is same-origin).
    h["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), usb=(), payment=(self), interest-cohort=()";
    h[_cspHeader] = _csp;
    // Keep private/authenticated surfaces out of search indexes (defence in depth alongside
    // auth, the noindex meta tags and robots.txt).
    if (PCI.Backend.Core.Redirects.IsPrivatePath(ctx.Request.Path.Value ?? ""))
        h["X-Robots-Tag"] = "noindex, nofollow";
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
string[] _rlPaths = { "/api/login", "/api/admin/auth/login", "/api/admin/auth/forgot", "/api/admin/auth/reset", "/api/admin/auth/recover", "/api/forgot-password", "/api/validate-code", "/api/set-password", "/api/exam/authorize", "/api/register", "/api/auth/google", "/api/founding/validate", "/api/founding/redeem", "/api/me/founding-application", "/api/honorary-application", "/api/training-partner-application", "/api/errors", "/api/partner/auth/login", "/api/inquiry", "/api/newsletter", "/api/form-submit" };
// Playwright boots this process with E2E_ADMIN_PASSWORD set; raise the window so the browser suite
// (dozens of register/login/forgot posts) does not cascade 429s. Production never sets that env.
var _rlE2e = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("E2E_ADMIN_PASSWORD"));
int RL_LIMIT = _rlE2e ? 500 : 10; long RL_WINDOW_MS = 60_000;
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
{ "/api/admin/auth/login", "/api/admin/auth/logout", "/api/admin/me", "/api/admin/me/password", "/api/admin/auth/forgot", "/api/admin/auth/reset", "/api/admin/auth/recover" };
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
        // Production MUST run on MySQL — SQLite is not an approved production/staging database, and there
        // is deliberately no silent fallback: a misconfigured DB_PROVIDER fails the preflight loudly.
        var dbProvider = (E("DB_PROVIDER") ?? "sqlite").Trim().ToLowerInvariant();
        if (dbProvider is not ("mysql" or "mariadb"))
            Err("DB_PROVIDER", "production requires MySQL — set DB_PROVIDER=mysql (SQLite is not approved for production)");
        else if (string.IsNullOrEmpty(E("MYSQL_CONNECTION_STRING")) && (string.IsNullOrEmpty(E("MYSQL_HOST")) || string.IsNullOrEmpty(E("MYSQL_PASSWORD"))))
            Err("MYSQL_HOST", "MySQL is selected but connection settings are incomplete (need MYSQL_HOST + MYSQL_PASSWORD, or MYSQL_CONNECTION_STRING)");
        if (string.IsNullOrEmpty(E("ADMIN_OWNER_PASSWORD"))) Warn("ADMIN_OWNER_PASSWORD", "bootstrap owner uses the default password until changed");
        // Data-at-rest encryption (Certuvo credentials + stored ID documents/photos) must use an explicit
        // 32-byte key in production — never the derived fallback, which would be predictable if the seeding
        // secrets are known. (ISO/IEC 27001 A.8.24 / 27018 cryptographic protection of PII.)
        if (!Security.HasDedicatedEncryptionKey())
            Err("CREDENTIAL_ENCRYPTION_KEY", "set a 32-byte key (base64/hex/passphrase) — required to encrypt credentials and stored identity documents at rest in production");
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
app.MapGet("/api/health", () => Json(new { ok = true, service = "pci-backend", time = DateTime.UtcNow.ToString("o"), recovery_configured = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ADMIN_RECOVERY_CODE")) }));

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
    if (u is null) { LoginGuard.BurnTime(password); return Results.Json(new { error = "invalid_credentials" }, statusCode: 401); }
    if (LoginGuard.IsLocked(db, "users", u["id"]))
        return Results.Json(new { error = "account_locked", message = "Too many failed attempts. Please try again in a few minutes." }, statusCode: 429);
    if (!Security.VerifyPassword(password, u["password_hash"] as string))
    {
        LoginGuard.OnFail(db, "users", u["id"]);
        try { var fip = H.LastHopIp(req.Headers["x-forwarded-for"].ToString(), req.HttpContext.Connection.RemoteIpAddress?.ToString());
              db.Execute("INSERT INTO login_events(user_id,ip,user_agent,device,outcome) VALUES(?,?,?,?,?)", u["id"], fip, "", "", "failed"); } catch { }
        return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
    }
    LoginGuard.OnSuccess(db, "users", u["id"]);
    if ((u["status"] as string) != "active") return Results.Json(new { error = "inactive" }, statusCode: 403);
    // Second factor: if the candidate has enabled TOTP (secret present and not still 'pending:'), require a
    // valid 6-digit code or a one-time recovery code before a session is issued. Replay is blocked by
    // recording the last consumed timestep (a captured code can't be reused within its ±1 window).
    if ((u["two_factor_enabled"] as long? ?? 0) == 1 && u["totp_secret"] is string usecret && usecret.Length > 0 && !usecret.StartsWith("pending:"))
    {
        var code = (S(b, "totp") ?? "").Trim();
        if (code.Length == 0) return Results.Json(new { error = "totp_required", two_factor = true, message = "Enter the 6-digit code from your authenticator app." }, statusCode: 401);
        var lastStep = u.TryGetValue("totp_last_step", out var ls) && ls is not null ? Convert.ToInt64(ls) : 0L;
        var step = Security.VerifyTotpStep(usecret, code);
        var totpOk = step >= 0 && step > lastStep;
        if (totpOk) db.Execute("UPDATE users SET totp_last_step=? WHERE id=?", step, u["id"]);
        else
        {
            // Fall back to a one-time recovery code (hashed compare; consumed on use).
            var recovHash = Security.Sha(code.Replace(" ", "").Replace("-", "").ToLowerInvariant());
            var stored = (u["totp_recovery"] as string) ?? "";
            var codes = string.IsNullOrEmpty(stored) ? new List<string>() : System.Text.Json.JsonSerializer.Deserialize<List<string>>(stored) ?? new();
            if (!codes.Remove(recovHash))
            {
                LoginGuard.OnFail(db, "users", u["id"]);
                return Results.Json(new { error = "totp_invalid", two_factor = true, message = "That code was not valid. Try again, or use a recovery code." }, statusCode: 401);
            }
            db.Execute("UPDATE users SET totp_recovery=? WHERE id=?", System.Text.Json.JsonSerializer.Serialize(codes), u["id"]);
        }
    }
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
    PCI.Backend.Core.Analytics.Track(db, req.HttpContext, "login", Convert.ToInt64(u["id"]));
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
    if (a is null) { LoginGuard.BurnTime(password); return Results.Json(new { error = "invalid_credentials" }, statusCode: 401); }
    if (LoginGuard.IsLocked(db, "admin_users", a["id"]))
        return Results.Json(new { error = "account_locked", message = "Too many failed attempts. Please try again in a few minutes." }, statusCode: 429);
    if ((a["status"] as string) != "active" || !Security.VerifyPassword(password, a["password_hash"] as string))
    {
        LoginGuard.OnFail(db, "admin_users", a["id"]);
        Log(a["id"] as long? ?? 0, "admin_login_failed", email);
        return Results.Json(new { error = "invalid_credentials" }, statusCode: 401);
    }
    LoginGuard.OnSuccess(db, "admin_users", a["id"]);
    // Optional MFA for privileged accounts: once an admin has enrolled a TOTP secret, every login
    // requires the 6-digit code as a second factor. The matched timestep is recorded and must strictly
    // advance, so a captured code cannot be replayed within its ±1 validity window.
    if (a["totp_secret"] is string totp && totp.Length > 0 && !totp.StartsWith("pending:"))
    {
        var code = (S(b, "totp") ?? "").Trim();
        if (code.Length == 0) return Results.Json(new { error = "totp_required" }, statusCode: 401);
        var matchedStep = Security.VerifyTotpStep(totp, code);
        var lastStep = a.TryGetValue("totp_last_step", out var ls) && ls is not null ? Convert.ToInt64(ls) : 0L;
        if (matchedStep >= 0 && matchedStep > lastStep)
        {
            db.Execute("UPDATE admin_users SET totp_last_step=? WHERE id=?", matchedStep, a["id"]);
        }
        else
        {
            // Fall back to a one-time recovery code (hashed compare; consumed on use) so a lost
            // authenticator is recoverable without operator intervention. Same scheme as the student login.
            var recovHash = Security.Sha(code.Replace(" ", "").Replace("-", "").ToLowerInvariant());
            var storedRec = (a.TryGetValue("totp_recovery", out var rv) ? rv as string : null) ?? "";
            var codes = string.IsNullOrEmpty(storedRec) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(storedRec) ?? new();
            if (!codes.Remove(recovHash))
            {
                LoginGuard.OnFail(db, "admin_users", a["id"]);
                return Results.Json(new { error = "totp_invalid" }, statusCode: 401);
            }
            db.Execute("UPDATE admin_users SET totp_recovery=? WHERE id=?", JsonSerializer.Serialize(codes), a["id"]);
        }
    }
    var tok = Security.RandomHex(24);
    db.Execute("INSERT INTO admin_sessions(admin_id,token,expires_at) VALUES(?,?,datetime('now','+12 hours'))", a["id"], Security.Sha(tok));
    db.Execute("UPDATE admin_users SET last_login_at=datetime('now') WHERE id=?", a["id"]);
    var ctx = Auth.ToAdmin(a);
    Log(ctx.Id, "admin_login", ctx.Email);   // attribute the login to the admin, so the audit log tracks who signed in
    return Json(new { token = tok, admin = new { id = ctx.Id, email = ctx.Email, name = ctx.Name, role = ctx.Role, must_change_pw = ctx.MustChangePw }, permissions = ctx.Perms });
});

// ---- self-service admin password reset (Forgot password) ----
// Request a reset link: always answers ok so the endpoint can never be used to probe which emails are
// admins. When the email belongs to an active admin, a single-use token (hashed at rest, valid 1 hour)
// is issued and a reset link emailed. If no email provider is configured the link is written to the
// email log / console instead, so the operator can still complete the reset.
app.MapPost("/api/admin/auth/forgot", async (HttpRequest req) =>
{
    var b = await Body(req);
    var email = (S(b, "email") ?? "").Trim().ToLowerInvariant();
    if (System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        var a = db.QueryOne("SELECT id,name FROM admin_users WHERE lower(email)=? AND status='active'", email);
        if (a is not null)
        {
            var token = Security.RandomHex(32);
            db.Execute("INSERT INTO admin_reset_tokens(admin_id,token,expires_at) VALUES(?,?, datetime('now','+1 hour'))", a["id"], Security.Sha(token));
            var baseUrl = Mailer.BaseUrl(req);
            var link = $"{baseUrl}/admin/reset-password?token={token}";
            var name = (a["name"] as string) is { Length: > 0 } nm ? nm : "there";
            Mailer.Send(db, null, email, "admin_password_reset", "Reset your PCI admin password",
                $"<p>Hello {System.Net.WebUtility.HtmlEncode(name)},</p>" +
                $"<p>We received a request to reset the password for your PCI Admin Console account. " +
                $"Click the link below to choose a new password. This link is valid for one hour and can be used once.</p>" +
                $"<p><a href=\"{link}\" style=\"display:inline-block;background:#1D4ED8;color:#fff;padding:11px 20px;border-radius:8px;text-decoration:none;font-weight:600\">Reset my password</a></p>" +
                $"<p style=\"font-size:12.5px;color:#64748B\">Or paste this address into your browser:<br>{link}</p>" +
                $"<p style=\"font-size:12.5px;color:#64748B\">If you did not request this, you can ignore this email — your password will not change.</p>");
            Log(a["id"] as long? ?? 0, "admin_password_reset_requested", email);
        }
    }
    return Json(new { ok = true });   // never reveal whether an admin account exists
});

// Consume a reset token and set a new password. Clears any active sessions and the must-change flag.
app.MapPost("/api/admin/auth/reset", async (HttpRequest req) =>
{
    var b = await Body(req);
    var token = S(b, "token") ?? "";
    var newPw = S(b, "new_password") ?? "";
    if (newPw.Length < 8) return Results.Json(new { error = "weak_password", message = "Use at least 8 characters." }, statusCode: 400);
    var row = db.QueryOne("SELECT * FROM admin_reset_tokens WHERE token=? AND used_at IS NULL AND expires_at > datetime('now')", Security.Sha(token));
    if (row is null) return Results.Json(new { error = "invalid_or_expired", message = "This reset link is invalid or has expired. Request a new one." }, statusCode: 400);
    var adminId = row["admin_id"];
    db.Execute("UPDATE admin_users SET password_hash=?, must_change_pw=0, status='active' WHERE id=?", BCrypt.Net.BCrypt.HashPassword(newPw), adminId);
    db.Execute("UPDATE admin_reset_tokens SET used_at=datetime('now') WHERE id=?", row["id"]);
    db.Execute("DELETE FROM admin_reset_tokens WHERE admin_id=? AND used_at IS NULL", adminId);  // invalidate other outstanding links
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", adminId);                           // sign out everywhere
    Log(adminId as long? ?? 0, "admin_password_reset_completed", null);
    return Json(new { ok = true });
});

// ---- recovery-code reset: reset an admin password with a shared recovery code, no email needed ----
// Enabled only when the ADMIN_RECOVERY_CODE environment variable is set (a secret the operator holds).
// Anyone presenting the correct code can set a new password for the named active admin — so the code
// is a master recovery secret and must be kept private. Constant-time compared and rate-limited.
// Distinct from the email flow (no inbox required) and the ADMIN_OWNER_RESET_PASSWORD env reset (no
// redeploy required — set the code once, then reset on demand from the login page).
app.MapPost("/api/admin/auth/recover", async (HttpRequest req) =>
{
    var configured = Environment.GetEnvironmentVariable("ADMIN_RECOVERY_CODE");
    if (string.IsNullOrWhiteSpace(configured))
        return Results.Json(new { error = "recovery_disabled", message = "Recovery-code reset is not enabled for this instance." }, statusCode: 400);
    var b = await Body(req);
    var email = (S(b, "email") ?? "").Trim().ToLowerInvariant();
    var code = S(b, "code") ?? "";
    var newPw = S(b, "new_password") ?? "";
    if (newPw.Length < 8) return Results.Json(new { error = "weak_password", message = "Use at least 8 characters." }, statusCode: 400);
    // Constant-time code check first; a generic failure is returned for a bad code OR unknown email so
    // the endpoint reveals nothing about which is wrong.
    var codeOk = Security.FixedTimeEquals(code, configured);
    var a = codeOk ? db.QueryOne("SELECT id,email FROM admin_users WHERE lower(email)=? AND status='active'", email) : null;
    if (!codeOk || a is null)
    {
        Log(0, "admin_recovery_failed", email);
        return Results.Json(new { error = "recovery_invalid", message = "The recovery code or email is not valid." }, statusCode: 400);
    }
    // The master recovery code also clears any TOTP factor — otherwise an admin who lost BOTH their password
    // and their authenticator would reset the password here and still be blocked at the MFA prompt.
    db.Execute("UPDATE admin_users SET password_hash=?, must_change_pw=0, status='active', totp_secret=NULL, totp_recovery=NULL, totp_last_step=0 WHERE id=?", BCrypt.Net.BCrypt.HashPassword(newPw), a["id"]);
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", a["id"]);               // sign out everywhere
    db.Execute("DELETE FROM admin_reset_tokens WHERE admin_id=? AND used_at IS NULL", a["id"]);   // void any pending email links
    Log(a["id"] as long? ?? 0, "admin_recovery_completed", email);
    return Json(new { ok = true });
});

// ---- optional TOTP MFA for privileged accounts: enrol (pending) → verify (active) → disable ----
app.MapGet("/api/admin/me/2fa", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var row = db.QueryOne("SELECT totp_secret,totp_recovery FROM admin_users WHERE id=?", a.Id);
    var secret = row?["totp_secret"] as string ?? "";
    var recovery = row?["totp_recovery"] as string ?? "";
    var recoveryCount = 0;
    try
    {
        recoveryCount = string.IsNullOrEmpty(recovery)
            ? 0
            : (JsonSerializer.Deserialize<List<string>>(recovery) ?? new()).Count;
    }
    catch
    {
        // A malformed legacy recovery-code payload must not break the Settings page. It is treated as
        // an empty inventory; the active TOTP factor still remains enforced until explicitly disabled.
    }
    return Json(new
    {
        enabled = secret.Length > 0 && !secret.StartsWith("pending:"),
        pending = secret.StartsWith("pending:"),
        recovery_remaining = recoveryCount,
    });
});

app.MapPost("/api/admin/me/2fa/setup", (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var existing = db.Scalar<string>("SELECT totp_secret FROM admin_users WHERE id=?", a.Id) ?? "";
    // Never replace an active factor with an unverified pending secret. Doing so would make the next
    // login skip MFA and let any stolen authenticated session silently downgrade the account.
    if (existing.Length > 0 && !existing.StartsWith("pending:"))
        return Results.Json(new { error = "already_enabled", message = "Two-factor authentication is already enabled." }, statusCode: 409);
    var secret = Security.NewTotpSecret();
    // Stored as pending until the admin proves their authenticator works — enabling MFA can never lock
    // an account out on a mis-scanned QR.
    db.Execute("UPDATE admin_users SET totp_secret=? WHERE id=?", "pending:" + secret, a.Id);
    Log(a.Id, "admin_2fa_setup", a.Email);
    return Json(new { secret, otpauth = $"otpauth://totp/PCI%20Admin:{Uri.EscapeDataString(a.Email)}?secret={secret}&issuer=PCI" });
});

app.MapPost("/api/admin/me/2fa/verify", async (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var b = await Body(req);
    var stored = db.Scalar<string>("SELECT totp_secret FROM admin_users WHERE id=?", a.Id) ?? "";
    if (!stored.StartsWith("pending:")) return Results.Json(new { error = "nothing_pending" }, statusCode: 409);
    if (!Security.VerifyTotp(stored["pending:".Length..], S(b, "code"))) return Results.Json(new { error = "totp_invalid" }, statusCode: 400);
    // Issue 10 one-time recovery codes so a lost authenticator can never permanently lock the admin out;
    // store SHA-256 hashes only and show the plaintext once (mirrors the student 2FA enrolment).
    var plain = Enumerable.Range(0, 10).Select(_ => Security.RandomHex(5).ToLowerInvariant()).ToList();
    var hashes = plain.Select(c => Security.Sha(c)).ToList();
    db.Execute("UPDATE admin_users SET totp_secret=?, totp_last_step=0, totp_recovery=? WHERE id=?",
        stored["pending:".Length..], JsonSerializer.Serialize(hashes), a.Id);
    Log(a.Id, "admin_2fa_enabled", a.Email);
    return Json(new { ok = true, enabled = true, recovery_codes = plain.Select(c => c.Length >= 10 ? c[..5] + "-" + c[5..10] : c).ToArray() });
});

app.MapPost("/api/admin/me/2fa/disable", async (HttpRequest req) =>
{
    var a = Auth.AdminFromReq(req, db);
    if (a is null) return Results.Json(new { error = "unauthorized" }, statusCode: 401);
    var b = await Body(req);
    var stored = db.Scalar<string>("SELECT totp_secret FROM admin_users WHERE id=?", a.Id) ?? "";
    if (stored.Length > 0 && !stored.StartsWith("pending:"))
    {
        var code = (S(b, "code") ?? "").Trim();
        var ok = Security.VerifyTotp(stored, code);
        if (!ok)
        {
            // Accept a one-time recovery code as well (consumed), so a lost authenticator can still turn MFA off.
            var recovHash = Security.Sha(code.Replace(" ", "").Replace("-", "").ToLowerInvariant());
            var rec = db.Scalar<string>("SELECT totp_recovery FROM admin_users WHERE id=?", a.Id) ?? "";
            var codes = string.IsNullOrEmpty(rec) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(rec) ?? new();
            if (!codes.Remove(recovHash))
                return Results.Json(new { error = "totp_invalid", message = "Enter a current authenticator or recovery code to disable MFA." }, statusCode: 400);
            db.Execute("UPDATE admin_users SET totp_recovery=? WHERE id=?", JsonSerializer.Serialize(codes), a.Id);
        }
    }
    db.Execute("UPDATE admin_users SET totp_secret=NULL, totp_recovery=NULL, totp_last_step=0 WHERE id=?", a.Id);
    Log(a.Id, "admin_2fa_disabled", a.Email);
    return Json(new { ok = true, enabled = false });
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
    var rows = db.Query("SELECT id,email,name,role,permissions,status,must_change_pw,last_login_at,created_at,cert_scope FROM admin_users ORDER BY id ASC")
        .Select(a => {
            var perms = new List<string>();
            try { perms = JsonSerializer.Deserialize<List<string>>(a["permissions"] as string ?? "[]") ?? new(); } catch { }
            var scope = new List<long>();
            try { scope = JsonSerializer.Deserialize<List<long>>(a["cert_scope"] as string ?? "[]") ?? new(); } catch { }
            return new Dictionary<string, object?>(a) { ["permissions"] = perms, ["effective"] = Rbac.PermsFor((string)a["role"]!, a["permissions"] as string), ["cert_scope"] = scope };
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
    // Optional per-certification scope: an array of certification ids. Empty/absent (and any owner)
    // means unrestricted. Values are validated to longs, so the stored JSON is always well-formed.
    string? certScope = null;
    if (role != "owner" && b.TryGetValue("cert_scope", out var csv) && csv.ValueKind == JsonValueKind.Array)
    {
        var ids = csv.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.Number).Select(e => e.GetInt64()).Distinct().ToArray();
        if (ids.Length > 0) certScope = JsonSerializer.Serialize(ids);
    }
    var tempPw = S(b, "password") ?? Security.RandomHex(5);
    // Whether a new admin must change their password at first sign-in is operator-configurable
    // (Settings → 'admin_force_password_change', default on). A per-request `force_password_change`
    // still overrides it either way.
    var forcePw = (H.GetEl(b, "force_password_change") is { } fpc
        ? fpc.ValueKind is JsonValueKind.True || (fpc.ValueKind is JsonValueKind.String && fpc.GetString() is "1" or "true")
        : Settings.Bool(db, "admin_force_password_change", true)) ? 1 : 0;
    var id = db.ExecuteReturningId("INSERT INTO admin_users(email,name,password_hash,role,permissions,status,must_change_pw,created_by,cert_scope) VALUES(?,?,?,?,?, 'active',?,?,?)",
        email, S(b, "name") ?? "", BCrypt.Net.BCrypt.HashPassword(tempPw), role, perms, forcePw, admin!.Id == 0 ? (object?)null : admin.Id, certScope);
    Log(admin.Id == 0 ? null : admin.Id, "admin_created", $"{email} ({role})");
    return Json(new { ok = true, id, temp_password = tempPw });
});

app.MapPatch("/api/admin/team/{id}", async (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out var actor); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    var b = await Body(req);
    var sets = new List<string>(); var vals = new List<object?>();
    if (b.ContainsKey("name")) { sets.Add("name=?"); vals.Add(S(b, "name")); }
    if (b.ContainsKey("role")) { sets.Add("role=?"); vals.Add(S(b, "role")); }
    if (b.TryGetValue("permissions", out var pv) && pv.ValueKind == JsonValueKind.Array) { sets.Add("permissions=?"); vals.Add(pv.GetRawText()); }
    if (b.TryGetValue("cert_scope", out var csv) && csv.ValueKind is JsonValueKind.Array or JsonValueKind.Null)
    {
        // An empty array or explicit null clears the restriction (all certifications).
        string? scope = null;
        if (csv.ValueKind == JsonValueKind.Array)
        {
            var ids = csv.EnumerateArray().Where(e => e.ValueKind == JsonValueKind.Number).Select(e => e.GetInt64()).Distinct().ToArray();
            if (ids.Length > 0) scope = JsonSerializer.Serialize(ids);
        }
        sets.Add("cert_scope=?"); vals.Add(scope);
    }
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
    Log(actor?.Id, "admin_updated", a["email"] as string);
    return Json(new { ok = true });
});

app.MapPost("/api/admin/team/{id}/reset-password", (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out var actor); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    var tempPw = Security.RandomHex(5);
    var forceReset = Settings.Bool(db, "admin_force_password_change", true) ? 1 : 0;
    db.Execute("UPDATE admin_users SET password_hash=?, must_change_pw=? WHERE id=?", BCrypt.Net.BCrypt.HashPassword(tempPw), forceReset, id);
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
    Log(actor?.Id, "admin_pw_reset", $"{a["email"] as string} (admin {id})");
    return Json(new { ok = true, temp_password = tempPw });
});

// Owner reset of another admin's MFA — the recovery path for a lost authenticator. Clears the TOTP factor
// and any recovery codes so the admin can sign in with their password alone and re-enrol a fresh factor.
// Owner-gated and audited; sessions are cleared so a stolen device cannot ride an existing session.
app.MapPost("/api/admin/team/{id}/reset-2fa", (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out var actor); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    db.Execute("UPDATE admin_users SET totp_secret=NULL, totp_recovery=NULL, totp_last_step=0 WHERE id=?", id);
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
    Log(actor?.Id, "admin_2fa_reset", $"{a["email"] as string} (admin {id})");
    return Json(new { ok = true });
});

app.MapDelete("/api/admin/team/{id}", (HttpRequest req, long id) =>
{
    var gate = OwnerGate(req, out var actor); if (gate is not null) return gate;
    var a = db.QueryOne("SELECT * FROM admin_users WHERE id=?", id);
    if (a is null) return Results.Json(new { error = "not_found" }, statusCode: 404);
    if ((a["role"] as string) == "owner")
    {
        var owners = db.Scalar<long>("SELECT COUNT(*) FROM admin_users WHERE role='owner' AND status='active'");
        if (owners <= 1) return Results.Json(new { error = "cannot_delete_last_owner" }, statusCode: 400);
    }
    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
    db.Execute("DELETE FROM admin_users WHERE id=?", id);
    Log(actor?.Id, "admin_deleted", a["email"] as string);
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
    Log(a.Id, "settings_update", string.Join(",", b.Keys));
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
PCI.Backend.Endpoints.ExamExceptions.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.Public.Map(app, db, logFn);
PCI.Backend.Endpoints.Account.Map(app, db, logFn);
PCI.Backend.Endpoints.AdminMgmt.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.PublicDocuments.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.CommsCentre.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.ContentCentre.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Blog CMS + SEO + AI Studio + capability registry
PCI.Backend.Endpoints.SocialPublishing.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Phase 2: live social connectors + drafts + outbox
PCI.Backend.Endpoints.Syndication.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Phase 3: WordPress/Ghost/Forem syndication
PCI.Backend.Endpoints.ExternalImport.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Phase 4: RSS/Atom import + review queue
PCI.Backend.Endpoints.Backlinks.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Phase 5: backlink & outreach CRM + on-demand link verification
PCI.Backend.Endpoints.ContentAnalytics.Map(app, db, logFn, GateFn, r => Auth.AdminFromReq(r, db));  // Phase 6: read-only analytics connectors (GSC/Bing/GA4)
PCI.Backend.Endpoints.MarketingCentre.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Payments.Map(app, db, logFn, () => !string.IsNullOrEmpty(stripeKey));
PCI.Backend.Endpoints.AdminExtra.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Reviews.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Forum.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);   // public discussion forum
PCI.Backend.Endpoints.Chat.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);    // self-hosted bot + live chat
PCI.Backend.Endpoints.Casework.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);
PCI.Backend.Endpoints.Events.Map(app, db, logFn, GateFn);   // member events & webinars (CPD-earning)
PCI.Backend.Endpoints.Badges.Map(app, db, logFn);           // native Open Badges (verifiable digital credentials)
PCI.Backend.Endpoints.Careers.Map(app, db, logFn, GateFn);  // dynamic careers / job board
PCI.Backend.Endpoints.MemberDirectory.Map(app, db, logFn, GateFn);// opt-in public member directory
PCI.Backend.Endpoints.Founding.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.Honorary.Map(app, db, logFn);
PCI.Backend.Endpoints.HonoraryApplication.Map(app, db, logFn);
PCI.Backend.Endpoints.HonoraryIdv.Map(app, db, logFn);   // shortlist-gated identity verification (photo + gov ID + background declaration)
PCI.Backend.Endpoints.Certificates.Map(app, db, logFn, GateFn);
PCI.Backend.Endpoints.Documents.Map(app, db, logFn, r => Auth.AdminFromReq(r, db), GateFn);   // Student Documents & Resources module
PCI.Backend.Endpoints.Books.Map(app, db, logFn, GateFn);              // Books & study materials: upload + watermarked download
PCI.Backend.Endpoints.Announcement.Map(app, db, logFn, GateFn);       // Admin-controlled public announcement modal
PCI.Backend.Endpoints.TrainingPartners.Map(app, db, logFn, GateFn);   // Training Partner framework (Phase 7)
PCI.Backend.Endpoints.Partners.Map(app, db, logFn, GateFn);           // Partner dashboards: portal token, sponsorship, commissions
PCI.Backend.Endpoints.Certuvo.Map(app, db, logFn);                    // Certuvo study & practice engine (Phase 8)
PCI.Backend.Endpoints.AdminI18n.Map(app, db, logFn);
PCI.Backend.Endpoints.Social.Map(app, db, logFn, GateFn);              // footer social-media links (admin-controlled)
PCI.Backend.Endpoints.Notifications.Map(app, db, logFn, GateFn);       // owner alert recipients + per-event toggles
PCI.Backend.Endpoints.Campaigns.Map(app, db, logFn, GateFn);           // bulk / marketing email campaigns + unsubscribe

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
try { PCI.Backend.Data.PublicDocsSeed.Ensure(db, webRoot); } catch { } // Public Downloads Centre core documents (idempotent)
PCI.Backend.Endpoints.AdminSeo.Map(app, db, logFn, GateFn, webRoot);   // Admin Console → SEO (/api/admin/seo/...)
PCI.Backend.Endpoints.AdminAnalytics.Map(app, db, GateFn);             // Admin Console → Analytics (/api/admin/analytics/...)
PCI.Backend.Endpoints.AdminAiVisibility.Map(app, db, logFn, GateFn, webRoot); // Admin Console → AI Visibility (/api/admin/ai-visibility/...)
PCI.Backend.Endpoints.AdminIntegrations.Map(app, db, logFn, GateFn);   // Admin Console → Integrations / ERP (/api/admin/integrations/...)
PCI.Backend.Core.ExamDeliveryConnectors.Register();                    // register the 5 exam-delivery vendor connectors
PCI.Backend.Endpoints.AdminExamDelivery.Map(app, db, logFn, GateFn);   // Admin Console → Exam Delivery (/api/admin/exam-delivery/...) + inbound callbacks
PCI.Backend.Endpoints.AdminOps.Map(app, db, logFn, GateFn);            // operator toolkit: mark-paid, test users, student journey, Certuvo config
PCI.Backend.Endpoints.Support.Map(app, db, logFn, GateFn);             // customer-service portal: error refs, unified inbox, SLA, templates, KB
PCI.Backend.Endpoints.PartnerPortal.Map(app, db, logFn, GateFn);       // institution portal + code approvals + fraud review queue
// one-time: capture each page's current headline as an editable block so every page is editable out of the box
PCI.Backend.Core.PageContent.SeedFromFiles(db, webRoot);
PCI.Backend.Data.I18nSeed.Apply(db);   // starter translations (nav + shared + homepage + top pages, 6 languages)
PCI.Backend.Data.CertuvoSeed.Apply(db); // Certuvo starter practice pack (scenario MCQs across BoK domains)
PCI.Backend.Data.BlogSeed.Ensure(db);  // Content Centre: default byline author + content-pillar categories (idempotent by slug)
PCI.Backend.Data.BlogContentSeed.Ensure(db);  // Content Centre: 20 PCI blog articles as DRAFTS (idempotent by slug; never auto-published)
PCI.Backend.Data.NewsContentSeed.Ensure(db);  // Content Centre: source-verified news items as DRAFTS (idempotent by slug; never auto-published)
PCI.Backend.Data.DemoExamSeed.Apply(db); // optional demo LIVE-exam bank — only when SEED_DEMO_EXAM=true (fresh-deploy testing)
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Method == "GET")
    {
        // Managed path redirects (Admin → SEO → Redirects): renamed/retired pages 301 to their
        // replacement before any serving. Single-hop by construction (chains rejected at write time).
        if (PCI.Backend.Core.PathRedirects.Target(db, ctx.Request) is { } red)
        {
            ctx.Response.StatusCode = red.status;
            if (red.status != 410)
                ctx.Response.Headers.Location = PCI.Backend.Core.PathRedirects.WithQuery(red.to, ctx.Request.QueryString);
            return;
        }
        var reqPath = ctx.Request.Path.Value ?? "/";
        // The classic HTML admin pages are retired — the React admin at /admin/ is the single console.
        // Permanently forward the old URLs (and any lingering links/bookmarks) to the new console.
        if (reqPath.Equals("/admin.html", StringComparison.OrdinalIgnoreCase)
            || reqPath.Equals("/admin-students.html", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.StatusCode = 301;
            ctx.Response.Headers.Location = reqPath.Contains("students", StringComparison.OrdinalIgnoreCase) ? "/admin/students" : "/admin/";
            return;
        }
        // Public verification clean URL: /verify-certificate is the canonical alias of the verify page.
        if (reqPath.Equals("/verify-certificate", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.StatusCode = 301;
            ctx.Response.Headers.Location = "/verify.html" + ctx.Request.QueryString;
            return;
        }
        // ── Public Downloads Centre clean URLs: /downloads and /downloads/{category} both serve the
        //    dynamic centre page (a single file that reads the category from the path and pre-filters via
        //    the /api/public/documents feed). Real files under /downloads/*.pdf are left to static serving. ──
        if (reqPath.Equals("/downloads", StringComparison.OrdinalIgnoreCase) ||
            (reqPath.StartsWith("/downloads/", StringComparison.OrdinalIgnoreCase) && !reqPath.Contains('.')))
        {
            var centre = System.IO.Path.Combine(webRoot, "downloads-centre.html");
            if (System.IO.File.Exists(centre))
            {
                ctx.Response.ContentType = "text/html; charset=utf-8";
                ctx.Response.Headers.CacheControl = "no-cache";
                await ctx.Response.SendFileAsync(centre);
                return;
            }
        }
        // ── Content Centre: dynamic blog (server-side rendered) + syndication feeds + blog sitemap ──────
        //    Full article HTML is present in the initial response (crawler-safe). Feeds/sitemap are exact
        //    paths; the index, category/author/tag facets and each article are rendered from blog_posts.
        {
            const StringComparison OIC = StringComparison.OrdinalIgnoreCase;
            var bp = PCI.Backend.Core.Blog.BasePath(db);                     // configurable, default "/blog"
            var nbp = PCI.Backend.Core.Blog.NewsBasePath(db);               // configurable, default "/news"
            var blang = PCI.Backend.Core.I18nContent.ActiveLang(ctx);
            if (reqPath.Equals("/blog-sitemap.xml", OIC))
            { ctx.Response.ContentType = "application/xml; charset=utf-8"; await ctx.Response.WriteAsync(PCI.Backend.Core.BlogFeeds.Sitemap(db)); return; }
            if (reqPath.Equals("/news-sitemap.xml", OIC))
            { ctx.Response.ContentType = "application/xml; charset=utf-8"; await ctx.Response.WriteAsync(PCI.Backend.Core.BlogFeeds.NewsSitemap(db)); return; }

            // Serve one content section (blog or news): feeds, listing, category/author/tag facets, article.
            // News and blog share one store + CMS but live at separate URL spaces; a post has exactly one
            // canonical home, so a wrong-section article URL 301s to the correct section.
            async Task<bool> ServeSection(string secBase, bool secNews)
            {
                if (reqPath.Equals(secBase + "/feed.xml", OIC))
                { ctx.Response.ContentType = "application/rss+xml; charset=utf-8"; await ctx.Response.WriteAsync(PCI.Backend.Core.BlogFeeds.Rss(db, secNews)); return true; }
                if (reqPath.Equals(secBase + "/atom.xml", OIC))
                { ctx.Response.ContentType = "application/atom+xml; charset=utf-8"; await ctx.Response.WriteAsync(PCI.Backend.Core.BlogFeeds.Atom(db, secNews)); return true; }
                if (reqPath.Equals(secBase + "/feed.json", OIC))
                { ctx.Response.ContentType = "application/feed+json; charset=utf-8"; await ctx.Response.WriteAsync(PCI.Backend.Core.BlogFeeds.Json(db, secNews)); return true; }
                if (reqPath.Equals(secBase, OIC) || reqPath.Equals(secBase + "/", OIC))
                {
                    int.TryParse(ctx.Request.Query["page"], out var pg);
                    ctx.Response.ContentType = "text/html; charset=utf-8"; ctx.Response.Headers.CacheControl = "no-cache"; ctx.Response.Headers.Vary = "Cookie";
                    await ctx.Response.WriteAsync(PCI.Backend.Core.BlogRender.RenderIndex(db, webRoot, blang, pg <= 0 ? 1 : pg, null, null, null, null, null, null, secNews));
                    return true;
                }
                if (reqPath.StartsWith(secBase + "/", OIC))
                {
                    var rest = reqPath.Substring(secBase.Length + 1).Trim('/');
                    int.TryParse(ctx.Request.Query["page"], out var pg);
                    if (pg <= 0) pg = 1;
                    string? html = null;
                    if (rest.StartsWith("category/", OIC))
                    {
                        var c = db.QueryOne("SELECT id,name FROM blog_categories WHERE slug=? AND active=1", rest.Substring(9).ToLowerInvariant());
                        if (c is not null) html = PCI.Backend.Core.BlogRender.RenderIndex(db, webRoot, blang, pg, PCI.Backend.Core.H.L(c["id"]), PCI.Backend.Core.H.Str(c["name"]), null, null, null, null, secNews);
                    }
                    else if (rest.StartsWith("author/", OIC))
                    {
                        var a = db.QueryOne("SELECT id,name FROM blog_authors WHERE slug=? AND active=1", rest.Substring(7).ToLowerInvariant());
                        if (a is not null) html = PCI.Backend.Core.BlogRender.RenderIndex(db, webRoot, blang, pg, null, null, PCI.Backend.Core.H.L(a["id"]), (secNews ? "News by " : "Articles by ") + (PCI.Backend.Core.H.Str(a["name"]) ?? ""), null, null, secNews);
                    }
                    else if (rest.StartsWith("tag/", OIC))
                    {
                        var t = db.QueryOne("SELECT id,name FROM blog_tags WHERE slug=?", rest.Substring(4).ToLowerInvariant());
                        if (t is not null) html = PCI.Backend.Core.BlogRender.RenderIndex(db, webRoot, blang, pg, null, null, null, null, PCI.Backend.Core.H.L(t["id"]), "#" + (PCI.Backend.Core.H.Str(t["name"]) ?? ""), secNews);
                    }
                    else if (!rest.Contains('/') && !rest.Contains('.'))
                    {
                        var (ahtml, aredirect) = PCI.Backend.Core.BlogRender.Article(db, webRoot, rest, blang, secNews);
                        if (aredirect is not null) { ctx.Response.StatusCode = 301; ctx.Response.Headers.Location = aredirect; return true; }
                        html = ahtml;
                        // Per-article, cookieless page view (articles are served here, before the static-page
                        // PageView call) — feeds the per-article analytics rollup.
                        if (html is not null) PCI.Backend.Core.Analytics.PageView(db, ctx, (secBase + "/" + rest).TrimStart('/'));
                    }
                    if (html is not null)
                    {
                        ctx.Response.ContentType = "text/html; charset=utf-8"; ctx.Response.Headers.CacheControl = "no-cache"; ctx.Response.Headers.Vary = "Cookie";
                        await ctx.Response.WriteAsync(html); return true;
                    }
                    // no match under this section (draft, unknown slug/category/author/tag) → 404 fall-through
                }
                return false;
            }

            if (await ServeSection(bp, false)) return;
            if (await ServeSection(nbp, true)) return;
        }
        // ── Multi-certification clean URLs: /certifications (catalogue) and /certifications/{slug} (per credential) ──
        if (reqPath.StartsWith("/certifications", StringComparison.OrdinalIgnoreCase))
        {
            var rest = reqPath.Length > 15 ? reqPath.Substring(15).Trim('/') : "";  // "/certifications".Length == 15
            // The dynamic suite comparison page — rendered live from the certifications, routes and
            // pricing tables, with #compare/#routes/#fees/#exams anchors as the canonical home of
            // that content. The aliases below deep-link into it.
            if (rest.Equals("compare", StringComparison.OrdinalIgnoreCase))
            {
                var cmp = PCI.Backend.Core.CertCompare.Render(db, webRoot, PCI.Backend.Core.I18nContent.ActiveLang(ctx));
                if (cmp is not null)
                {
                    ctx.Response.ContentType = "text/html; charset=utf-8";
                    ctx.Response.Headers.CacheControl = "no-cache";
                    ctx.Response.Headers.Vary = "Cookie";
                    await ctx.Response.WriteAsync(cmp);
                    return;
                }
            }
            // Catalogue-level aliases: stable URLs for routes/fees/exams deep-linking into the live
            // comparison page's anchored sections. Query string is preserved ahead of any fragment.
            var certAlias = rest.ToLowerInvariant() switch
            {
                "routes" => "/certifications/compare#routes",
                "fees" => "/certifications/compare#fees",
                "exams" => "/certifications/compare#exams",
                _ => null,
            };
            if (certAlias is not null && certAlias != reqPath)
            {
                var hash = certAlias.IndexOf('#');
                var target = hash < 0 ? certAlias + ctx.Request.QueryString
                                      : certAlias[..hash] + ctx.Request.QueryString + certAlias[hash..];
                ctx.Response.StatusCode = 301;
                ctx.Response.Headers.Location = target;
                return;
            }
            // Permanent redirects from previous credential slugs to the final Project Leadership Suite slugs.
            var certRedirect = rest.ToLowerInvariant() switch
            {
                "pcp-ai" or "pcp" => "pcl-ai",
                "pfip" or "pfip-ai" => "pfl-ai",
                "cpmd" or "cpmd-ai" or "pdl-ai" => "pml-ai",
                _ => null,
            };
            if (certRedirect is not null)
            {
                ctx.Response.StatusCode = 301;
                ctx.Response.Headers.Location = "/certifications/" + certRedirect + ctx.Request.QueryString;
                return;
            }
            if (rest.Length > 0 && !rest.Contains('/') && !rest.Contains('.'))
            {
                var page = PCI.Backend.Core.CertPage.Render(db, webRoot, rest, PCI.Backend.Core.I18nContent.ActiveLang(ctx));
                if (page is not null)
                {
                    ctx.Response.ContentType = "text/html; charset=utf-8";
                    ctx.Response.Headers.CacheControl = "no-cache";
                    ctx.Response.Headers.Vary = "Cookie";
                    await ctx.Response.WriteAsync(page);
                    return;
                }
                // unknown certification slug → fall through to normal 404 handling
            }
            else if (rest.Length == 0)
            {
                reqPath = "/certifications.html";  // render the catalogue landing through the normal pipeline
            }
        }
        var slug = reqPath == "/" ? "index.html" : reqPath.TrimStart('/');
        var isPage = slug.EndsWith(".html", StringComparison.OrdinalIgnoreCase) && !slug.Contains("..");
        // Active public-website language (?lang= persisted to a cookie, else cookie, else English).
        // When a non-English language is active every page must be rendered (to inject translations),
        // even one that has no admin content overrides.
        var lang = PCI.Backend.Core.I18nContent.ActiveLang(ctx);
        var needI18n = isPage && PCI.Backend.Core.I18nContent.Applies(lang);
        var hasContent = isPage && PCI.Backend.Core.PageContent.HasOverrides(db, slug);
        var hasCerts = isPage && PCI.Backend.Core.CertCatalogue.Applies(slug);
        if (hasContent || hasCerts || needI18n)
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
                // Public-website translation: replace each captured region's text with its translation
                // for the active language and set <html lang dir>. No-op for English (byte-identical).
                if (needI18n) rendered = PCI.Backend.Core.I18nContent.Render(db, slug, rendered, lang);
                if (hasCerts) rendered = PCI.Backend.Core.CertCatalogue.Inject(db, rendered);
                rendered = PCI.Backend.Core.ListSections.Inject(db, rendered, lang);
                rendered = PCI.Backend.Core.PriceTags.Inject(db, rendered);
                rendered = PCI.Backend.Core.SeoTags.Inject(db, rendered, slug);   // GA4/GTM/Clarity + verification metas (admin-set, consent-gated)
                PCI.Backend.Core.Analytics.PageView(db, ctx, slug);               // first-party, cookieless page view
                ctx.Response.ContentType = "text/html; charset=utf-8";
                ctx.Response.Headers.CacheControl = "no-cache";   // content is admin-editable — always revalidate
                ctx.Response.Headers.Vary = "Cookie";             // the language cookie varies the response
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
        // Dynamic sitemap.xml from the pages table (published, indexable, canonical host) — overrides
        // the static file so it always reflects real content and excludes private/noindex pages.
        if (reqPath.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "application/xml; charset=utf-8";
            await ctx.Response.WriteAsync(PCI.Backend.Core.Sitemap.Xml(db));
            return;
        }
        // Sitemap index — unifies the page sitemap and the blog (image) sitemap under one entry point.
        if (reqPath.Equals("/sitemap-index.xml", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "application/xml; charset=utf-8";
            await ctx.Response.WriteAsync(PCI.Backend.Core.Sitemap.Index(db));
            return;
        }
        // Policy-aware robots.txt (Phase 6) — overrides the static file so blocked AI crawlers
        // (Admin → AI Visibility) are reflected live; allowed answer engines inherit the * group.
        if (reqPath.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "text/plain; charset=utf-8";
            await ctx.Response.WriteAsync(PCI.Backend.Core.AiVisibility.Robots(db));
            return;
        }
        // llms.txt (llmstxt.org) — a curated Markdown map of indexable content for language models.
        if (reqPath.Equals("/llms.txt", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "text/plain; charset=utf-8";
            await ctx.Response.WriteAsync(PCI.Backend.Core.AiVisibility.LlmsTxt(db));
            return;
        }
        // IndexNow ownership proof: serve the site key at /{key}.txt (the protocol's requirement).
        if (reqPath.Length > 5 && reqPath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
            && PCI.Backend.Core.IndexNowService.Key(db) is { Length: > 0 } inKey
            && reqPath.Equals("/" + inKey + ".txt", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Response.ContentType = "text/plain; charset=utf-8";
            await ctx.Response.WriteAsync(inKey);
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
Console.WriteLine($"│  Admin console  {u}/admin/");
Console.WriteLine($"│  Student Panel  {u}/student.html");
Console.WriteLine($"│  Exam preview   {u}/exam-ui.html");
Console.WriteLine("└──────────────────────────────────────────────────────┘");

app.Run();
