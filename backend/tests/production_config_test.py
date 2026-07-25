#!/usr/bin/env python3
"""Fail-closed startup checks that must run before any SQLite file is created."""
import os, subprocess, tempfile

BACKEND = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
passed = failed = 0

def check(name, overrides):
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
    if ok:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}: exit={proc.returncode} db_exists={os.path.exists(db)}")
        print(proc.stdout[-1200:])

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
# The whole-platform persistent-disk opt-in has the same shape as the World waiver: the flag alone
# is NOT enough — the database must live on the mounted disk, or boot still fails closed.
check("ALLOW_SQLITE_IN_PRODUCTION rejects an ephemeral (non-/data) path",
      {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite",
       "ALLOW_SQLITE_IN_PRODUCTION": "true",
       "DATABASE_FILE": os.path.join("/tmp", "platform-must-not-open.db")})

# Positive case: the opt-in with a /data path must get PAST the database gate (the deploy-recovery
# path for pre-MySQL services). Requires a writable /data, which CI runners may not have — skip
# honestly rather than fake a pass. "Past the gate" = the boot log reaches the provider line and
# never prints a refusal; the process is killed before it has to finish seeding.
def check_boots_past_gate(name, overrides):
    global passed, failed
    if not (os.path.isdir("/data") and os.access("/data", os.W_OK)):
        print(f"  SKIP  {name} (no writable /data on this runner)")
        return
    db = os.path.join("/data", "preflight-optin-test.db")
    for suffix in ("", "-wal", "-shm"):
        try: os.remove(db + suffix)
        except FileNotFoundError: pass
    env = dict(os.environ)
    for key in ("ASPNETCORE_ENVIRONMENT", "DOTNET_ENVIRONMENT", "APP_ENV", "DB_PROVIDER",
                "MYSQL_CONNECTION_STRING", "MYSQL_HOST", "MYSQL_PASSWORD",
                "ALLOW_INSECURE_PRODUCTION", "PCIWORLD_ONLY", "PCIWORLD_ALLOW_SQLITE",
                "ALLOW_SQLITE_IN_PRODUCTION"):
        env.pop(key, None)
    env.update(DATABASE_FILE=db, PORT="0")
    env.update(overrides)
    try:
        proc = subprocess.run(["dotnet", DLL], cwd=BACKEND, env=env, text=True,
                              stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=25)
        out = proc.stdout
    except subprocess.TimeoutExpired as e:  # still running = it booted past every fail-closed gate
        out = (e.stdout or b"").decode() if isinstance(e.stdout, bytes) else (e.stdout or "")
    ok = "Refusing" not in out and "database provider: Sqlite" in out and os.path.exists(db)
    for suffix in ("", "-wal", "-shm"):
        try: os.remove(db + suffix)
        except FileNotFoundError: pass
    if ok:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}")
        print(out[-1200:])

check_boots_past_gate("ALLOW_SQLITE_IN_PRODUCTION + /data path opens the database (deploy recovery)",
      {"ASPNETCORE_ENVIRONMENT": "Production", "DB_PROVIDER": "sqlite",
       "ALLOW_SQLITE_IN_PRODUCTION": "true",
       "APP_BASE_URL": "https://pci-platform.example.org",
       "ALLOWED_ORIGIN": "https://pci-platform.example.org",
       "CREDENTIAL_ENCRYPTION_KEY": "preflight-test-key-0123456789abcdef0123456789abcdef"})

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
