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

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
