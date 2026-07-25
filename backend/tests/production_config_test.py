#!/usr/bin/env python3
"""Fail-closed startup checks that must run before any SQLite file is created."""
import os, subprocess, tempfile

BACKEND = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
passed = failed = 0

def run(overrides, timeout=30):
    """Boot the published app with a clean production-ish environment; return (proc, db_path)."""
    work = tempfile.mkdtemp(prefix="pci_prod_preflight_")
    db = os.path.join(work, "must-not-exist.db")
    env = dict(os.environ)
    for key in (
        "ASPNETCORE_ENVIRONMENT", "DOTNET_ENVIRONMENT", "APP_ENV", "DB_PROVIDER",
        "MYSQL_CONNECTION_STRING", "MYSQL_HOST", "MYSQL_PASSWORD",
        "ALLOW_INSECURE_PRODUCTION", "PCIWORLD_ONLY", "PCIWORLD_ALLOW_SQLITE",
    ):
        env.pop(key, None)
    env.update(DATABASE_FILE=db, PORT="0")
    env.update(overrides)
    proc = subprocess.run(
        ["dotnet", DLL], cwd=BACKEND, env=env, text=True,
        stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=timeout)
    return proc, db


def check(name, overrides):
    global passed, failed
    proc, db = run(overrides)
    ok = proc.returncode == 78 and not os.path.exists(db)
    if ok:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}: exit={proc.returncode} db_exists={os.path.exists(db)}")
        print(proc.stdout[-1200:])


def expect(name, condition, detail=""):
    global passed, failed
    if condition:
        passed += 1
        print(f"  PASS  {name}")
    else:
        failed += 1
        print(f"  FAIL  {name}{': ' + detail if detail else ''}")

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

# An unreachable database must fail as a named, documented exit — never as an unhandled exception.
# Regression: Db's connect-retry exception escaped the constructor (called outside any try/catch),
# so a wrong MySQL host/credential aborted the process with exit 134 and a stack trace instead of
# telling the operator which setting to fix. That is the single commonest production deploy failure.
proc, _ = run({
    "ASPNETCORE_ENVIRONMENT": "Production",
    "DB_PROVIDER": "mysql",
    # 127.0.0.1:1 refuses immediately, so the retry loop finishes fast.
    "MYSQL_HOST": "127.0.0.1", "MYSQL_PORT": "1",
    "MYSQL_USER": "pci", "MYSQL_PASSWORD": "secret", "MYSQL_DATABASE": "pci",
    "MYSQL_CONNECT_RETRIES": "1",
    "APP_BASE_URL": "https://example.org", "ALLOWED_ORIGIN": "https://example.org",
    "CREDENTIAL_ENCRYPTION_KEY": "x" * 32,
}, timeout=90)
expect("unreachable MySQL exits 75 (EX_TEMPFAIL), not an unhandled crash",
       proc.returncode == 75, f"exit={proc.returncode}")
expect("unreachable MySQL names the cause and the settings to check",
       "[db] refusing to start" in proc.stdout and "MYSQL_HOST" in proc.stdout)
expect("unreachable MySQL prints no unhandled-exception stack trace",
       "Unhandled exception" not in proc.stdout)

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
