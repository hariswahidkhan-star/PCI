#!/usr/bin/env python3
"""
Availability regression — a stalled vendor must not freeze the whole backend.

`Db.Transaction` holds `_gate`, the single global lock guarding EVERY database operation, for the
entire body of the transaction. So any outbound network call made inside a transaction blocks every
other request in the process for as long as the remote host takes to answer — and forever if it
never answers.

The fee-waiver settlement path is the one that made this reachable from a normal admin action:
`Settlement.Grant` performs Certuvo provisioning, integration webhooks and the receipt email, so
wrapping it in `db.Transaction(...)` turned "one slow vendor" into "the service is down". It is now
claimed-then-granted: the idempotency claim is its own atomic insert, and the grant runs outside any
transaction.

This test pins that contract. It points Certuvo at a black-hole listener (accepts the connection,
never replies), issues a full waiver, and — while that request is still in flight — asks for an
ordinary DB-backed read:

  • the unrelated read MUST still be served promptly (the stall is contained to its own request);
  • the waiver itself MUST still complete once its own vendor timeout elapses.

A regression (grant back inside the transaction) fails the first assertion, because the read cannot
take `_gate` until the vendor answers.

Exit code 0 iff every assertion passes.  Run from backend/ (needs bin/Release/net8.0):
    python3 tests/waiver_vendor_stall_test.py
"""
import json, os, socket, sqlite3, subprocess, threading, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_waiver_stall.db")
STORAGE = os.path.join(HERE, "_waiver_stall_storage")

# The unrelated read must be served while the waiver is stalled. Generous enough that a loaded CI
# box cannot fail it by being slow, tight enough that holding the global lock cannot pass it.
READ_BUDGET_S = 20

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port(); BASE = f"http://127.0.0.1:{PORT}"
BH_PORT = free_port()

def blackhole():
    """A vendor that hangs rather than failing fast: accept the connection, never write a reply."""
    srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    srv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    srv.bind(("127.0.0.1", BH_PORT)); srv.listen(16)
    held = []                      # keep every accepted socket open and silent
    while True:
        try: held.append(srv.accept()[0])
        except OSError: return

def req(method, path, token=None, body=None, timeout=None):
    hdr = {}; data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    with urllib.request.urlopen(r, timeout=timeout) as resp:
        return resp.status, resp.read().decode()

def jreq(method, path, **kw):
    try: code, txt = req(method, path, **kw)
    except urllib.error.HTTPError as e: code, txt = e.code, e.read().decode()
    try: return code, json.loads(txt)
    except Exception: return code, txt

def boot():
    for suffix in ("", "-wal", "-shm", ".migrate.lock"):
        try: os.remove(DB + suffix)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE,
               ASPNETCORE_ENVIRONMENT="Development",
               INTEGRATIONS_ALLOW_PRIVATE_EGRESS="true")   # the black hole is on loopback
    log = open(os.path.join(HERE, "_waiver_stall_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=log, stderr=subprocess.STDOUT)
    deadline = time.time() + 180
    while time.time() < deadline:
        if proc.poll() is not None: raise SystemExit(f"server exited during boot (exit {proc.returncode})")
        try:
            if req("GET", "/api/health", timeout=2)[0] == 200: return proc
        except Exception: time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot within 180s")

def main():
    threading.Thread(target=blackhole, daemon=True).start()
    proc = boot()
    try:
        _, b = jreq("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        admin = b["token"]
        jreq("POST", "/api/admin/me/password", token=admin, body={"new_password": "OwnerPass99!"})

        # Certuvo points at the black hole, so Settlement.Grant's provisioning call stalls.
        c, _ = jreq("POST", "/api/admin/certuvo", token=admin, body={
            "enabled": True, "api_base": f"http://127.0.0.1:{BH_PORT}",
            "provision_path": "/certuvo/accounts", "api_key": "cv_key",
            "login_url": "https://certuvo.example"})
        chk("setup: Certuvo configured against the stalled vendor", c == 200, c)

        jreq("POST", "/api/register", body={"firstName": "S", "lastName": "T", "email": "stall@ex.co",
             "password": "Passw0rd!23", "confirmPassword": "Passw0rd!23", "country": "United Kingdom"})
        con = sqlite3.connect(DB); row = con.execute("SELECT id FROM users WHERE email='stall@ex.co'").fetchone(); con.close()
        chk("setup: student created", row is not None)
        uid = row[0]

        t0 = time.time(); jreq("GET", "/api/certifications", timeout=15); baseline = time.time() - t0
        chk("baseline: an ordinary DB-backed read is fast", baseline < 5, f"{baseline:.2f}s")

        outcome = {}
        def do_waive():
            t = time.time()
            try:
                code, _ = jreq("POST", f"/api/admin/students/{uid}/waive", token=admin,
                               body={"product": "membership", "percent": 100, "reason": "vendor stall probe"},
                               timeout=180)
                outcome["code"] = code; outcome["secs"] = time.time() - t
            except Exception as e:
                outcome["error"] = f"{type(e).__name__} after {time.time()-t:.1f}s"
        w = threading.Thread(target=do_waive, daemon=True); w.start()

        time.sleep(4)          # let the waiver reach its stalled vendor call
        chk("the waiver is genuinely still in flight (the probe is meaningful)", not outcome, outcome)

        t0 = time.time()
        try:
            code, _ = jreq("GET", "/api/certifications", timeout=READ_BUDGET_S)
            elapsed = time.time() - t0
            chk(f"an unrelated DB read is still served while a vendor stalls (<{READ_BUDGET_S}s)",
                code == 200 and elapsed < READ_BUDGET_S, f"{code} in {elapsed:.1f}s")
        except Exception as e:
            chk(f"an unrelated DB read is still served while a vendor stalls (<{READ_BUDGET_S}s)",
                False, f"{type(e).__name__} after {time.time()-t0:.1f}s — the global DB lock is held across network I/O")

        w.join(timeout=200)
        chk("the waiver itself still completes once its own vendor timeout elapses",
            outcome.get("code") == 200, outcome)

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
