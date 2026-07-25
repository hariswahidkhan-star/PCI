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
except OSError:
    print("  SKIP  PCI World zero-config bridge boot (no writable /data in this environment)")

print(f"\n  == {passed}/{passed + failed} PASSED ==")
raise SystemExit(0 if failed == 0 else 1)
