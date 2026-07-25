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

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
