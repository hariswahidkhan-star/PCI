#!/usr/bin/env python3
"""
Exhaustive 500-sweep: calls EVERY mapped route under three auth contexts (anonymous, student,
owner admin) with benign bodies, and fails if any response is a 5xx. This is a crash-net, not a
behaviour test — 4xx responses are fine (auth/validation working); 5xx means an unhandled path
(null-ref, serialization collision, bad SQL) that only real execution can catch.

Boots its own server on a throwaway DB (same pattern as integration_test.py).
Run from backend/:  python3 tests/sweep_500_test.py
"""
import json, os, re, socket, sqlite3, subprocess, sys, time, hashlib, urllib.request, urllib.error

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_sweep.db")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p
PORT = free_port(); BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, token=None, body=None):
    hdr = {}
    data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r, timeout=15) as resp: return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()
    except Exception as e:
        return -1, str(e)[:200]

def discover_routes():
    routes = []
    pat = re.compile(r'Map(Get|Post|Patch|Put|Delete)\("(/api/[^"]+)"')
    for root in ("Endpoints", "."):
        d = os.path.join(BACKEND, root)
        for f in os.listdir(d):
            if f.endswith(".cs"):
                for m in pat.finditer(open(os.path.join(d, f), encoding="utf-8").read()):
                    routes.append((m.group(1).upper().replace("PATCH","PATCH"), m.group(2)))
    return sorted(set(routes))

def main():
    for f in (DB, DB+"-wal", DB+"-shm"):
        try: os.remove(f)
        except OSError: pass
    # No Stripe keys: payment endpoints answer a clean 503 (the Stripe-enabled path is covered by
    # integration_test.py); Development so unhandled exceptions carry diagnostics in the response.
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT),
               ASPNETCORE_ENVIRONMENT="Development",
               STORAGE_ROOT=os.path.join(HERE, "_sweep_storage"))
    logf = open(os.path.join(HERE, "_sweep_server.log"), "w")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND,
                            stdout=logf, stderr=subprocess.STDOUT)
    try:
        for _ in range(60):
            if req("GET", "/api/health")[0] == 200: break
            time.sleep(0.5)

        # owner token
        _, ob = req("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        otok = json.loads(ob)["token"]
        # student with data: create directly (this sweep tests handlers, not signup flow)
        con = sqlite3.connect(DB)
        uid = con.execute("INSERT INTO users(email,first_name,last_name,role,status,password_hash) VALUES('sweep@ex.co','S','W','student','active','x') RETURNING id").fetchone()[0]
        stok = "sweeptok"
        con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                    (uid, hashlib.sha256(stok.encode()).hexdigest()))
        con.execute("INSERT INTO student_profiles(user_id,country) VALUES(?, 'PK')", (uid,))
        con.execute("INSERT INTO payments(user_id,product_type,final_amount,currency,payment_provider,provider_payment_id,payment_status,payment_date,reference) VALUES(?, 'exam',299,'USD','stripe','pi_sweep','paid',datetime('now'),'PCI-SWEEP1')", (uid,))
        con.execute("INSERT INTO tickets(user_id,reference,subject,category,status) VALUES(?, 'T-SW','s','general','open')", (uid,))
        con.commit(); con.close()

        routes = discover_routes()
        # substitutions for parameterized segments
        def concretize(p):
            return re.sub(r"\{[^}]+\}", "1", p)

        # benign generic body: covers most handlers' optional fields harmlessly
        generic = {"email": "sweep2@ex.co", "subject": "s", "body": "b", "note": "n", "action": "noop",
                   "code": "X", "product": "membership", "reason": "r", "type": "general",
                   "question": "q", "answer": "a", "name": "n", "role": "viewer", "status": "open",
                   "scheduled_at": "2030-01-01T00:00:00Z", "hours": 1, "description": "d",
                   "category": "General", "form_type": "contact", "consent_type": "exam_rules",
                   "new_password": "Xx12345678!", "password": "Xx12345678!", "token": "deadbeef",
                   "count": 1, "prefix": "SW", "user_id": 1, "attempt_id": 1, "step": 1, "data": {}}

        fails = []
        skip = {("POST", "/api/webhook")}  # needs a signed payload; covered by integration suite
        n = 0
        for method, route in routes:
            if (method, route) in skip: continue
            path = concretize(route)
            for label, tok in (("anon", None), ("student", stok), ("owner", otok)):
                body = generic if method in ("POST", "PATCH", "PUT") else None
                code, txt = req(method, path, token=tok, body=body)
                n += 1
                # 503 payments_not_configured is the documented no-Stripe-key response, not a crash
                if code == 503 and "payments_not_configured" in txt: continue
                if code >= 500 or code == -1:
                    fails.append((method, path, label, code, txt[:300]))
        print(f"swept {n} calls across {len(routes)} routes x 3 auth contexts")
        if fails:
            print(f"\n{len(fails)} SERVER ERRORS:")
            for m, p, l, c, t in fails:
                print(f"  [{c}] {m} {p} as {l} :: {t[:180]}")
            sys.exit(1)
        print("0 server errors — PASS")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()

if __name__ == "__main__":
    main()
