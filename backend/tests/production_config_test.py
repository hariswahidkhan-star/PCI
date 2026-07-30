#!/usr/bin/env python3
"""Fail-closed startup checks that must run before any SQLite file is created."""
import os, subprocess, tempfile

BACKEND = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
passed = failed = 0

def check(name, overrides, must_mention=None):
    global passed, failed
    work = tempfile.mkdtemp(prefix="pci_prod_preflight_")
    db = os.path.join(work, "must-not-exist.db")
    env = dict(os.environ)
    for key in (
        "ASPNETCORE_ENVIRONMENT", "DOTNET_ENVIRONMENT", "APP_ENV", "DB_PROVIDER",
        "MYSQL_CONNECTION_STRING", "MYSQL_HOST", "MYSQL_PASSWORD",
        "ALLOW_INSECURE_PRODUCTION", "PCIWORLD_ONLY", "PCIWORLD_ALLOW_SQLITE",
        "ALLOW_SQLITE_IN_PRODUCTION",
    ):
        env.pop(key, None)
    env.update(DATABASE_FILE=db, PORT="0")
    env.update(overrides)
    proc = subprocess.run(
        ["dotnet", DLL], cwd=BACKEND, env=env, text=True,
        stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=30)
    ok = proc.returncode == 78 and not os.path.exists(db)
    if ok and must_mention:
        ok = must_mention in proc.stdout
    if ok:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}: exit={proc.returncode} db_exists={os.path.exists(db)}")
        print(proc.stdout[-1200:])

def run(overrides, timeout=90):
    """Boot the published app with a clean production-ish environment; return (proc, db_path).
    For assertions that need to inspect the exit code / output directly (expect below), rather
    than the fixed exit-78 contract check() asserts."""
    work = tempfile.mkdtemp(prefix="pci_prod_preflight_")
    db = os.path.join(work, "must-not-exist.db")
    env = dict(os.environ)
    for key in (
        "ASPNETCORE_ENVIRONMENT", "DOTNET_ENVIRONMENT", "APP_ENV", "DB_PROVIDER",
        "MYSQL_CONNECTION_STRING", "MYSQL_HOST", "MYSQL_PASSWORD",
        "ALLOW_INSECURE_PRODUCTION", "PCIWORLD_ONLY", "PCIWORLD_ALLOW_SQLITE",
        "ALLOW_SQLITE_IN_PRODUCTION",
    ):
        env.pop(key, None)
    env.update(DATABASE_FILE=db, PORT="0")
    env.update(overrides)
    proc = subprocess.run(
        ["dotnet", DLL], cwd=BACKEND, env=env, text=True,
        stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=timeout)
    return proc, db

def expect(name, condition, detail=""):
    global passed, failed
    if condition:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}{': ' + detail if detail else ''}")

def check_boots(name, overrides, db_file):
    """The inverse contract: this configuration must get PAST the fail-closed preflight and open
    its database. Passing the preflight is observable as the DB file appearing; the process is
    then terminated — a full HTTP boot is the E2E suite's job, not this one's."""
    global passed, failed
    env = dict(os.environ)
    for key in (
        "ASPNETCORE_ENVIRONMENT", "DOTNET_ENVIRONMENT", "APP_ENV", "DB_PROVIDER",
        "MYSQL_CONNECTION_STRING", "MYSQL_HOST", "MYSQL_PASSWORD",
        "ALLOW_INSECURE_PRODUCTION", "PCIWORLD_ONLY", "PCIWORLD_ALLOW_SQLITE",
        "ALLOW_SQLITE_IN_PRODUCTION",
        "APP_BASE_URL", "SITE_BASE_URL", "ALLOWED_ORIGIN", "CREDENTIAL_ENCRYPTION_KEY",
    ):
        env.pop(key, None)
    env.update(DATABASE_FILE=db_file, PORT="0")
    env.update(overrides)
    proc = subprocess.Popen(
        ["dotnet", DLL], cwd=BACKEND, env=env, text=True,
        stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
    try:
        out = proc.communicate(timeout=45)[0]
    except subprocess.TimeoutExpired:
        proc.terminate()          # still running at 45s = it booted and is serving
        out = proc.communicate(timeout=15)[0]
    ok = proc.returncode != 78 and os.path.exists(db_file)
    if ok:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}: exit={proc.returncode} db_exists={os.path.exists(db_file)}")
        print(out[-1200:])

check("framework-default Production rejects SQLite before open", {"DB_PROVIDER": "sqlite"})
check("DOTNET_ENVIRONMENT=Production rejects SQLite before open",
      {"DOTNET_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite"})
check("Staging rejects SQLite before open",
      {"ASPNETCORE_ENVIRONMENT": "Staging", "DB_PROVIDER": "sqlite"})
check("unknown provider never falls back to SQLite",
      {"ASPNETCORE_ENVIRONMENT": "Development", "DB_PROVIDER": "mysqll"})
check("PCI World SQLite waiver rejects ephemeral /tmp path",
      {"ASPNETCORE_ENVIRONMENT": "Production", "PCIWORLD_ONLY": "true",
       "PCIWORLD_ALLOW_SQLITE": "true", "DB_PROVIDER": "sqlite",
       "DATABASE_FILE": os.path.join("/tmp", "pciworld-must-not-open.db")})
check("PCI World WITHOUT the explicit bridge still rejects SQLite",
      {"ASPNETCORE_ENVIRONMENT": "Production", "PCIWORLD_ONLY": "true", "DB_PROVIDER": "sqlite"})
# The whole-platform persistent-disk opt-in has the same shape as the World waiver: the flag alone
# is NOT enough — the database must live on the mounted disk, or boot still fails closed.
check("ALLOW_SQLITE_IN_PRODUCTION rejects an ephemeral (non-/data) path",
      {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite",
       "ALLOW_SQLITE_IN_PRODUCTION": "true",
       "DATABASE_FILE": os.path.join("/tmp", "platform-must-not-open.db")})
# EXT-P1-09 — STORAGE_PROVIDER=s3 with no S3_BUCKET must be a named hard blocker, never a silent
# local-disk fallback (the config error must call out S3_BUCKET by name).
check("STORAGE_PROVIDER=s3 without S3_BUCKET refuses production boot",
      {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "mysql",
       "MYSQL_HOST": "127.0.0.1", "MYSQL_PASSWORD": "wrong-on-purpose",
       "STORAGE_PROVIDER": "s3"},
      must_mention="S3_BUCKET")

# The PCIWorld image's zero-config contract (PCIWorld/README.md): world-only + the explicit
# bridge + a /data database must BOOT — with the base-URL/CORS/at-rest-key blockers downgraded
# to warnings, because the surfaces they guard are not served on a world-only deployment. This
# is exactly the posture the PCIWorld/Dockerfile entrypoint produces with no variables set.
# Needs a writable /data (the container always has one); skip where the sandbox does not.
data_dir = "/data"
try:
    os.makedirs(data_dir, exist_ok=True)
    probe = os.path.join(data_dir, ".pci-test-probe")
    open(probe, "w").write("ok"); os.remove(probe)
    world_db = os.path.join(data_dir, "pciworld-boot-test.db")
    if os.path.exists(world_db): os.remove(world_db)
    check_boots("PCI World zero-config bridge boots on /data (blockers downgrade to warnings)",
                {"ASPNETCORE_ENVIRONMENT": "Production", "PCIWORLD_ONLY": "true",
                 "PCIWORLD_ALLOW_SQLITE": "true", "DB_PROVIDER": "sqlite",
                 "RENDER_EXTERNAL_URL": "https://pciworld.example.onrender.com"},
                world_db)
    if os.path.exists(world_db): os.remove(world_db)

    # Whole-platform deploy recovery: ALLOW_SQLITE_IN_PRODUCTION + a /data database must get past
    # the fail-closed gate too — but unlike world-only, the base-URL/CORS/at-rest-key blockers stay
    # ACTIVE, so the overrides must satisfy them for boot to proceed.
    platform_db = os.path.join(data_dir, "platform-optin-boot-test.db")
    if os.path.exists(platform_db): os.remove(platform_db)
    check_boots("ALLOW_SQLITE_IN_PRODUCTION + /data path opens the database (deploy recovery)",
                {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite",
                 "ALLOW_SQLITE_IN_PRODUCTION": "true",
                 "APP_BASE_URL": "https://pci-platform.example.org",
                 "ALLOWED_ORIGIN": "https://pci-platform.example.org",
                 "CREDENTIAL_ENCRYPTION_KEY": "preflight-test-key-0123456789abcdef0123456789abcdef"},
                platform_db)
    if os.path.exists(platform_db): os.remove(platform_db)

    # Zero-config persistent-disk auto-posture — the exact shape of a legacy hand-created Render
    # service: a /data disk, RENDER_EXTERNAL_URL (which Render always exports), and NOTHING else —
    # no flags, no APP_BASE_URL, no ALLOWED_ORIGIN, no CREDENTIAL_ENCRYPTION_KEY. It must boot:
    # the base URL is adopted from RENDER_EXTERNAL_URL, CORS defaults to same-origin, and the
    # remaining gaps downgrade to loud warnings. The ephemeral-path refusals above prove the
    # auto-posture never extends beyond /data.
    auto_db = os.path.join(data_dir, "platform-auto-boot-test.db")
    if os.path.exists(auto_db): os.remove(auto_db)
    check_boots("bare legacy service (/data + RENDER_EXTERNAL_URL only) boots with zero config",
                {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite",
                 "RENDER_EXTERNAL_URL": "https://pci-legacy.example.onrender.com"},
                auto_db)
    if os.path.exists(auto_db): os.remove(auto_db)
except OSError:
    print("  SKIP  /data boot-through checks (no writable /data in this environment)")

# Base-URL rule consistency: the pre-DB preflight and ConfigIssues() now share one predicate
# (IsPublicHttpsUrl — absolute, https, not loopback). Previously ConfigIssues() only searched the
# string for "localhost"/"127.0.0.1", so system-check called an http:// or malformed base URL
# healthy while the preflight refused to boot on it. Each bad shape must be refused BY NAME.
_valid_but_for_url = {
    "ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "mysql",
    "MYSQL_HOST": "127.0.0.1", "MYSQL_PASSWORD": "wrong-on-purpose",
    "ALLOWED_ORIGIN": "https://pci.example.org",
    "CREDENTIAL_ENCRYPTION_KEY": "preflight-test-key-0123456789abcdef0123456789abcdef",
}
check("malformed (non-absolute) APP_BASE_URL is refused by name",
      {**_valid_but_for_url, "APP_BASE_URL": "pci.example.org"}, must_mention="APP_BASE_URL")
check("http:// APP_BASE_URL is refused by name",
      {**_valid_but_for_url, "APP_BASE_URL": "http://pci.example.org"}, must_mention="APP_BASE_URL")
check("loopback https://localhost APP_BASE_URL is refused by name",
      {**_valid_but_for_url, "APP_BASE_URL": "https://localhost:8443"}, must_mention="APP_BASE_URL")

# An unreachable database must fail as a named, documented exit — never as an unhandled exception.
# Regression: Db's connect-retry exception escaped the constructor (called outside any try/catch),
# so a wrong MySQL host/credential aborted the process with exit 134 and a stack trace instead of
# telling the operator which setting to fix. That is the single commonest production deploy failure.
proc, db = run({
    "ASPNETCORE_ENVIRONMENT": "Production",
    "DB_PROVIDER": "mysql",
    # 127.0.0.1:1 refuses immediately, so the retry loop finishes fast.
    "MYSQL_HOST": "127.0.0.1", "MYSQL_PORT": "1",
    "MYSQL_USER": "pci", "MYSQL_PASSWORD": "secret", "MYSQL_DATABASE": "pci",
    "MYSQL_CONNECT_RETRIES": "1",
    "APP_BASE_URL": "https://example.org", "ALLOWED_ORIGIN": "https://example.org",
    "CREDENTIAL_ENCRYPTION_KEY": "preflight-test-key-0123456789abcdef0123456789abcdef",
})
expect("unreachable MySQL exits 75 (EX_TEMPFAIL), not an unhandled crash",
       proc.returncode == 75, f"exit={proc.returncode}")
expect("unreachable MySQL names the cause and the settings to check",
       "[db] refusing to start" in proc.stdout and "MYSQL_HOST" in proc.stdout)
expect("unreachable MySQL does not print the password",
       "secret" not in proc.stdout)
expect("unreachable MySQL prints no unhandled-exception stack trace",
       "Unhandled exception" not in proc.stdout)
expect("unreachable MySQL never creates a fallback SQLite database",
       not os.path.exists(db))

# ---------------------------------------------------------------------------------------------
# The DEPLOYED configuration itself. render.yaml is the only production environment nothing else
# in CI reads, and it is exactly where a fail-closed regression hides: the app can be perfectly
# correct while the blueprint hands it a combination that exits 78 on every deploy, and the only
# symptom is that the live site silently keeps serving the previous build.
#
# So: parse the blueprint, take the environment IT declares, and boot the real binary with it.
# This is the config the platform actually deploys with, asserted against the real preflight
# rather than against a copy of it that can drift.
try:
    import yaml
except ImportError:
    print("  SKIP  render.yaml blueprint checks (PyYAML not installed)")
else:
    blueprint = os.path.join(os.path.dirname(BACKEND), "render.yaml")
    with open(blueprint) as fh:
        service = yaml.safe_load(fh)["services"][0]

    declared = {}
    for entry in service.get("envVars", []):
        if "value" in entry:
            declared[entry["key"]] = str(entry["value"])
        elif entry.get("generateValue"):
            # Render mints this once at service creation; any 32-byte value models it faithfully.
            declared[entry["key"]] = "blueprint-preflight-key-0123456789abcdef0123456789abcdef"
        # sync:false keys are deliberately blank on a fresh service — leave them unset, because
        # "boots before the operator has filled anything in" is precisely the property under test.

    expect("blueprint mounts a persistent disk at /data",
           service.get("disk", {}).get("mountPath") == "/data")
    expect("blueprint keeps the health check on /api/health",
           service.get("healthCheckPath") == "/api/health")
    # A SQLite database anywhere but the mount is erased on every redeploy; the app refuses to open
    # one there, so a blueprint that declared it would fail-closed on arrival.
    if declared.get("DB_PROVIDER", "").lower() not in ("mysql", "mariadb"):
        expect("blueprint puts the SQLite database on the persistent disk",
               declared.get("DATABASE_FILE", "").startswith("/data/"))
    expect("blueprint declares DB_PROVIDER explicitly (a blueprint cannot clear a key it omits)",
           "DB_PROVIDER" in declared)

    # BOTH providers are supported, so the blueprint has to keep the other one switchable. The
    # non-secret MySQL settings must be declared with working values, or "switch to MySQL" quietly
    # becomes "edit this file, open a PR and redeploy" — which is how a second provider decays into
    # an unmaintained one. The three credentials stay sync:false: they are secrets.
    mysql_keys = {e["key"] for e in service.get("envVars", [])}
    for key in ("MYSQL_HOST", "MYSQL_USER", "MYSQL_PASSWORD", "MYSQL_PORT", "MYSQL_DATABASE", "MYSQL_SSL"):
        expect(f"blueprint keeps {key} declared so the provider stays switchable from the dashboard",
               key in mysql_keys)
    for key in ("MYSQL_HOST", "MYSQL_USER", "MYSQL_PASSWORD"):
        expect(f"{key} is a dashboard secret, never committed",
               key not in declared)
    expect("blueprint ships a working MySQL port/database/TLS default",
           declared.get("MYSQL_PORT") == "3306" and bool(declared.get("MYSQL_DATABASE"))
           and declared.get("MYSQL_SSL") in ("required", "true", "false"))

    if os.path.isdir(data_dir) and os.access(data_dir, os.W_OK):
        bp_db = os.path.join(data_dir, "blueprint-boot-test.db")
        if os.path.exists(bp_db): os.remove(bp_db)
        env = dict(declared)
        env["DATABASE_FILE"] = bp_db      # same mount, a name that cannot collide with real data
        check_boots("render.yaml as written boots a fresh service with NO dashboard values set",
                    env, bp_db)
        if os.path.exists(bp_db): os.remove(bp_db)

        # The inert MySQL block must stay inert. Blank credentials sitting beside a SQLite provider
        # are only safe because the MySQL preflight is gated on DB_PROVIDER; if that gate ever
        # widened, every SQLite deploy would start failing on credentials it does not need.
        bp_db2 = os.path.join(data_dir, "blueprint-inert-mysql-test.db")
        if os.path.exists(bp_db2): os.remove(bp_db2)
        env2 = dict(declared)
        env2["DATABASE_FILE"] = bp_db2
        env2["MYSQL_PORT"] = declared.get("MYSQL_PORT", "3306")
        check_boots("declared-but-blank MySQL settings cannot fail a SQLite boot", env2, bp_db2)
        if os.path.exists(bp_db2): os.remove(bp_db2)
    else:
        print("  SKIP  render.yaml boot-through (no writable /data in this environment)")

    # And the other direction: selecting MySQL without credentials must still fail closed. This is
    # the pairing that bricked the deploy — DB_PROVIDER=mysql with blank MYSQL_* — and the reason
    # ALLOW_SQLITE_IN_PRODUCTION does not rescue it is easy to lose in a refactor of the preflight.
    check("selecting MySQL with blank credentials still fails closed, waiver or not",
          {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "mysql",
           "ALLOW_SQLITE_IN_PRODUCTION": "true",
           "APP_BASE_URL": declared.get("APP_BASE_URL", "https://example.org"),
           "ALLOWED_ORIGIN": declared.get("ALLOWED_ORIGIN", "https://example.org")},
          must_mention="MySQL is selected but connection settings are incomplete")

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
