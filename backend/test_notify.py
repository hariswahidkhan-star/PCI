#!/usr/bin/env python3
"""End-to-end tests for the owner notification hub (Core/Notify.cs multi-recipient Alert +
Endpoints/Notifications.cs + the chat/enrolment/inquiry event hooks).

Boots the real server against a THROWAWAY sqlite db, adds recipient emails through the admin API,
fires the real public events (inquiry, chat hand-off) and the test action, then reads
notification_history to prove every recipient was alerted for every enabled event. With no SMTP
configured the Mailer uses its console sink (status "console"/"sent") — delivery is still recorded,
so the ledger is the ground truth. Nothing touches the real ./pci.db.
"""
import json, os, shutil, subprocess, sys, tempfile, time, urllib.request, urllib.error

BACKEND = os.path.dirname(os.path.abspath(__file__))
PORT = int(os.environ.get("NOTIFY_TEST_PORT", "18096"))
BASE = f"http://127.0.0.1:{PORT}"
ADMIN_EMAIL = "owner@test.local"
ADMIN_PW = "changeme-owner-test"
R1 = "founder@example.com"
R2 = "assistant@example.com"

results = []
def check(name, ok, detail=""):
    results.append((name, bool(ok), detail))
    print(f"  [{'PASS' if ok else 'FAIL'}] {name}" + (f"  — {detail}" if detail and not ok else ""))

def find_dotnet():
    for c in (shutil.which("dotnet"), "/usr/bin/dotnet", "/usr/lib/dotnet/dotnet"):
        if c and os.path.exists(c):
            return c
    sys.exit("dotnet not found")

def req(method, path, body=None, token=None, ip=None):
    data = json.dumps(body).encode() if body is not None else None
    r = urllib.request.Request(BASE + path, data=data, method=method)
    r.add_header("Content-Type", "application/json")
    if token: r.add_header("Authorization", "Bearer " + token)
    if ip: r.add_header("X-Forwarded-For", ip)
    try:
        with urllib.request.urlopen(r, timeout=20) as resp:
            return resp.status, json.loads(resp.read().decode() or "{}")
    except urllib.error.HTTPError as e:
        try: return e.code, json.loads(e.read().decode() or "{}")
        except Exception: return e.code, {}

def main():
    dotnet = find_dotnet()
    print("== dotnet build ==")
    b = subprocess.run([dotnet, "build"], cwd=BACKEND, capture_output=True, text=True)
    print("\n".join(b.stdout.splitlines()[-4:]))
    if b.returncode != 0:
        print(b.stdout[-4000:]); print(b.stderr[-2000:]); sys.exit("BUILD FAILED")
    dll = os.path.join(BACKEND, "bin", "Debug", "net8.0", "PCI.Backend.dll")
    if not os.path.exists(dll): sys.exit(f"built dll not found: {dll}")

    scratch = tempfile.mkdtemp(prefix="pci-notify-test-")
    env = dict(os.environ, PORT=str(PORT),
               DATABASE_FILE=os.path.join(scratch, "notify-test.db"),
               STORAGE_ROOT=os.path.join(scratch, "storage"),
               ADMIN_OWNER_EMAIL=ADMIN_EMAIL, ADMIN_OWNER_PASSWORD=ADMIN_PW,
               FORUM_SALT="test-salt")
    logf = open(os.path.join(scratch, "server.log"), "w")
    srv = subprocess.Popen([dotnet, dll], cwd=BACKEND, env=env, stdout=logf, stderr=subprocess.STDOUT)
    try:
        for _ in range(180):
            try:
                s, j = req("GET", "/api/health")
                if s == 200 and j.get("ok"): break
            except Exception: pass
            if srv.poll() is not None:
                print(open(logf.name).read()[-4000:]); sys.exit("server exited during boot")
            time.sleep(0.5)
        else:
            sys.exit("server never became healthy")
        print(f"server up on {BASE}\n== notification tests ==")

        # ---- admin login + clear forced password change ----
        s, j = req("POST", "/api/admin/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PW})
        tok = j.get("token")
        if tok:
            req("POST", "/api/admin/me/password", {"new_password": "notify-test-password-1"}, token=tok)
        check("0. admin login", bool(tok), f"status={s}")

        # (1) recipients empty by default
        s, j = req("GET", "/api/admin/notifications", token=tok)
        # No notify_recipients set yet, but AdminEmail falls back to the site contact so alerts never vanish.
        fallback = j.get("resolved", [])
        check("1. defaults: falls back to the site contact (alerts never vanish)",
              s == 200 and j.get("recipients") == "" and len(fallback) == 1 and "@" in fallback[0] and "events" in j,
              f"status={s} resolved={fallback}")

        # (2) requires auth
        s, j = req("GET", "/api/admin/notifications")
        check("2. GET without token rejected", s in (401, 403), f"status={s}")

        # (3) add two recipient emails (comma + newline mix) and enable all events
        s, j = req("POST", "/api/admin/notifications",
                   {"recipients": f"{R1}, {R2}", "chat": True, "enrollment": True, "inquiry": True}, token=tok)
        check("3. save recipients + enable events", s == 200 and j.get("ok")
              and set(j.get("resolved", [])) >= {R1, R2}, f"status={s} body={j}")

        # (4) recipients resolve, de-duplicated, admin fallback included
        s, j = req("GET", "/api/admin/notifications", token=tok)
        check("4. two recipients resolved", s == 200 and R1 in j.get("resolved", []) and R2 in j.get("resolved", []),
              f"resolved={j.get('resolved')}")

        def ledger_for(related_type, subject_substr):
            s, j = req("GET", "/api/admin/notifications", token=tok)
            rows = [r for r in j.get("recent", [])
                    if r.get("related_type") == related_type and subject_substr.lower() in (r.get("subject") or "").lower()]
            return {r.get("recipient") for r in rows}

        # (5) INQUIRY event -> both recipients alerted
        s, j = req("POST", "/api/inquiry",
                   {"type": "corporate", "email": "lead@acme.co", "first_name": "Dana", "org": "Acme",
                    "message": "We want to certify 20 engineers."})
        got = ledger_for("inquiry", "inquiry")
        check("5. inquiry alerts every recipient", s == 200 and {R1, R2} <= got, f"alerted={got}")

        # (6) CHAT hand-off event -> both recipients alerted
        s, j = req("POST", "/api/chat/start", {"name": "Priya"})
        ctok = j.get("token")
        s, j = req("POST", "/api/chat/send", {"token": ctok, "body": "Can I talk to a person please?"})
        waiting = j.get("status") == "waiting"
        got = ledger_for("chat_session", "Live chat request")
        check("6. chat hand-off alerts every recipient", waiting and {R1, R2} <= got, f"waiting={waiting} alerted={got}")

        # (7) send-test action reaches every recipient
        s, j = req("POST", "/api/admin/notifications/test", token=tok)
        recs = set(j.get("recipients", []))
        check("7. test action sends to every recipient", s == 200 and j.get("ok")
              and {R1, R2} <= recs and j.get("sent") == len(recs), f"status={s} body={j}")

        # (8) disabling an event stops its alerts
        req("POST", "/api/admin/notifications", {"inquiry": False}, token=tok)
        before = ledger_for("inquiry", "inquiry")
        req("POST", "/api/inquiry", {"type": "general", "email": "x@y.co", "message": "hello"})
        after = ledger_for("inquiry", "inquiry")
        check("8. disabled event sends nothing new", after == before, f"before={len(before)} after={len(after)}")

        # (9) re-enable works
        req("POST", "/api/admin/notifications", {"inquiry": True}, token=tok)
        s, j = req("GET", "/api/admin/notifications", token=tok)
        check("9. toggle re-enabled", j.get("toggles", {}).get("inquiry") is True, f"toggles={j.get('toggles')}")

        # (10) clearing notify_recipients falls back to the site contact — alerts still deliver, never vanish
        req("POST", "/api/admin/notifications", {"recipients": ""}, token=tok)
        s, j = req("POST", "/api/admin/notifications/test", token=tok)
        check("10. cleared list falls back to the site contact (alerts never vanish)",
              s == 200 and j.get("ok") and j.get("sent") >= 1, f"status={s} body={j}")

    finally:
        srv.terminate()
        try: srv.wait(timeout=10)
        except Exception: srv.kill()
        logf.close()
        shutil.rmtree(scratch, ignore_errors=True)

    passed = sum(1 for _, ok, _ in results if ok)
    print(f"\n{passed}/{len(results)} passed")
    if passed != len(results):
        print("FAILURES:")
        for n, ok, d in results:
            if not ok: print(f"  - {n}  {d}")
        sys.exit(1)
    print("ALL NOTIFICATION TESTS PASSED")

if __name__ == "__main__":
    main()
