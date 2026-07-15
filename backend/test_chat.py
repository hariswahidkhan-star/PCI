#!/usr/bin/env python3
"""End-to-end tests for the self-hosted website chat (Endpoints/Chat.cs + wwwroot/assets/chat.js
and wwwroot/admin-chat.html).

Boots the real ASP.NET Core server against a THROWAWAY sqlite database (DATABASE_FILE env — the
real ./pci.db is never touched), waits for /api/health, then exercises the public chat API and the
gated admin console API via urllib. Client IPs are varied with X-Forwarded-For (the app trusts the
LAST XFF hop) so independent tests get independent rate-limit buckets.

Server-boot + admin bootstrap are copied from test_forum.py.
"""
import json, os, shutil, subprocess, sys, tempfile, time, urllib.request, urllib.error
from html.parser import HTMLParser

BACKEND = os.path.dirname(os.path.abspath(__file__))
PORT = int(os.environ.get("CHAT_TEST_PORT", "18094"))
BASE = f"http://127.0.0.1:{PORT}"
ADMIN_EMAIL = "owner@test.local"
ADMIN_PW = "changeme-owner-test"

results = []
def check(name, ok, detail=""):
    results.append((name, bool(ok), detail))
    print(f"  [{'PASS' if ok else 'FAIL'}] {name}" + (f"  — {detail}" if detail and not ok else ""))

def find_dotnet():
    for c in (shutil.which("dotnet"), "/usr/bin/dotnet", "/usr/lib/dotnet/dotnet"):
        if c and os.path.exists(c):
            return c
    sys.exit("dotnet not found")

def req(method, path, body=None, ip=None, token=None):
    """Returns (status, parsed-json)."""
    data = json.dumps(body).encode() if body is not None else None
    r = urllib.request.Request(BASE + path, data=data, method=method)
    r.add_header("Content-Type", "application/json")
    if ip: r.add_header("X-Forwarded-For", ip)
    if token: r.add_header("Authorization", "Bearer " + token)
    try:
        with urllib.request.urlopen(r, timeout=15) as resp:
            return resp.status, json.loads(resp.read().decode() or "{}")
    except urllib.error.HTTPError as e:
        try: return e.code, json.loads(e.read().decode() or "{}")
        except Exception: return e.code, {}

# ---- chat helpers ----
def start(name=None, ip=None):
    body = {"name": name} if name is not None else {}
    return req("POST", "/api/chat/start", body, ip=ip)

def send(token, body, ip=None, escalate=None):
    b = {"token": token, "body": body}
    if escalate is not None: b["escalate"] = escalate
    return req("POST", "/api/chat/send", b, ip=ip)

def poll(token, after=0):
    return req("GET", f"/api/chat/poll?token={token}&after={after}")

def main():
    dotnet = find_dotnet()

    # ---- build (must be clean) ----
    print("== dotnet build ==")
    b = subprocess.run([dotnet, "build"], cwd=BACKEND, capture_output=True, text=True)
    print("\n".join(b.stdout.splitlines()[-4:]))
    if b.returncode != 0:
        print(b.stdout[-4000:]); print(b.stderr[-2000:]); sys.exit("BUILD FAILED")

    dll = os.path.join(BACKEND, "bin", "Debug", "net8.0", "PCI.Backend.dll")
    if not os.path.exists(dll): sys.exit(f"built dll not found: {dll}")

    # ---- boot server on a scratch db ----
    scratch = tempfile.mkdtemp(prefix="pci-chat-test-")
    env = dict(os.environ,
               PORT=str(PORT),
               DATABASE_FILE=os.path.join(scratch, "chat-test.db"),
               STORAGE_ROOT=os.path.join(scratch, "storage"),
               ADMIN_OWNER_EMAIL=ADMIN_EMAIL,
               ADMIN_OWNER_PASSWORD=ADMIN_PW,
               FORUM_SALT="test-salt", CHAT_SALT="test-chat-salt")
    logf = open(os.path.join(scratch, "server.log"), "w")
    srv = subprocess.Popen([dotnet, dll], cwd=BACKEND, env=env, stdout=logf, stderr=subprocess.STDOUT)
    try:
        for _ in range(180):
            try:
                s, j = req("GET", "/api/health")
                if s == 200 and j.get("ok"): break
            except Exception: pass
            if srv.poll() is not None:
                logf.flush(); print(open(logf.name).read()[-4000:]); sys.exit("server exited during boot")
            time.sleep(0.5)
        else:
            sys.exit("server never became healthy")
        print(f"server up on {BASE} (db: {env['DATABASE_FILE']})\n== public chat tests ==")

        # (1) start -> token + greeting
        s, j = start("QA Primary Session", ip="10.20.0.1")
        primary = j.get("token")
        check("1. start -> token + greeting",
              s == 200 and primary and isinstance(j.get("greeting"), str) and len(j["greeting"]) > 20
              and j.get("status") == "bot", f"status={s} body={j}")

        # (2) KB-matching question -> bot answer with substance
        # seeded row: "How much does it cost?" keywords cost,price,fees,how much,...
        s, j = send(primary, "How much does it cost to sit the exam?", ip="10.20.0.1")
        reply2 = (j.get("replies") or [{}])[0].get("body", "")
        low2 = reply2.lower()
        check("2. KB-matching question -> substantive bot answer",
              s == 200 and j.get("status") == "bot" and len(reply2) > 40
              and ("fee" in low2 or "cost" in low2 or "enrol" in low2),
              f"status={s} reply={reply2!r}")

        # (3) gibberish -> fallback mentions a person / team
        s, j = send(primary, "qwx zzptfk blorf vibblewock", ip="10.20.0.1")
        reply3 = (j.get("replies") or [{}])[0].get("body", "").lower()
        check("3. gibberish -> fallback offers a person/team",
              s == 200 and ("person" in reply3 or "team" in reply3),
              f"status={s} reply={reply3!r}")

        # (4) ask for a person -> session moves to 'waiting'
        s, j = start("Handoff Tester", ip="10.20.0.2")
        htok = j.get("token")
        s, j = send(htok, "Please can I talk to a person?", ip="10.20.0.2")
        sp, jp = poll(htok, 0)
        check("4. 'talk to a person' -> status waiting",
              s == 200 and j.get("status") == "waiting"
              and sp == 200 and jp.get("status") == "waiting",
              f"send_status={j.get('status')} poll_status={jp.get('status')}")

        # 4b) escalate=true flag also queues (independent session/ip)
        s, j = start("Escalate Flag", ip="10.20.0.9")
        etok = j.get("token")
        s, j = send(etok, "", ip="10.20.0.9", escalate=True)
        check("4b. escalate=true -> status waiting", s == 200 and j.get("status") == "waiting", f"status={s} body={j}")

        # (5) poll after= returns only newer messages, in ascending id order
        s, all_j = poll(primary, 0)
        msgs = all_j.get("messages", [])
        ids = [m["id"] for m in msgs]
        cutoff = ids[len(ids)//2] if len(ids) > 2 else ids[0]
        s2, j2 = poll(primary, cutoff)
        newer = j2.get("messages", [])
        newer_ids = [m["id"] for m in newer]
        check("5. poll after= filters + orders",
              s == 200 and s2 == 200 and len(ids) >= 3 and ids == sorted(ids)
              and all(i > cutoff for i in newer_ids) and newer_ids == sorted(newer_ids)
              and newer_ids and max(ids) in newer_ids,
              f"all_ids={ids} cutoff={cutoff} newer={newer_ids}")

        # (6) exceed the per-session visitor message cap -> 429 eventually
        s, j = start("Flood Tester", ip="10.20.0.3")
        ftok = j.get("token")
        got_429 = False
        for i in range(40):
            s, j = send(ftok, f"message number {i}", ip="10.20.0.3")
            if s == 429 and j.get("error") == "rate_limited":
                got_429 = True; break
        check("6. message cap / rate limit -> 429", got_429, f"last_status={s} body={j}")

        # ---- admin console API ----
        print("== admin chat console tests ==")
        s, j = req("POST", "/api/admin/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PW})
        tok = j.get("token")
        check("A0. seeded owner admin can log in", s == 200 and tok, f"status={s} body={j}")
        # clear forced password change (same bootstrap as test_forum), token stays valid
        req("POST", "/api/admin/me/password", {"new_password": "chat-test-password-1"}, token=tok)

        s, j = req("GET", "/api/admin/chat/sessions")
        check("A1. sessions without token -> 401", s == 401, f"status={s}")

        # (7) admin lists sessions incl. ours; transcript returns full history
        s, j = req("GET", "/api/admin/chat/sessions", token=tok)
        sess = j.get("sessions", [])
        mine = next((x for x in sess if x.get("visitor_name") == "QA Primary Session"), None)
        no_secrets = all(("ip_hash" not in x and "token" not in x) for x in sess)
        check("7. admin sessions list includes our session (no secrets)",
              s == 200 and mine is not None and mine.get("message_count", 0) >= 3 and no_secrets,
              f"status={s} found={mine is not None} count={len(sess)}")
        pid = (mine or {}).get("id")
        s, j = req("GET", f"/api/admin/chat/sessions/{pid}", token=tok)
        tmsgs = j.get("messages", [])
        senders = set(m["sender"] for m in tmsgs)
        check("7b. transcript returns full history (tracing)",
              s == 200 and j.get("session", {}).get("id") == pid and len(tmsgs) >= 4
              and "visitor" in senders and "bot" in senders
              and all("ip_hash" not in m for m in tmsgs),
              f"status={s} n={len(tmsgs)} senders={senders}")

        # (8) admin reply -> visitor poll receives 'agent' message + status 'live'
        s, j = req("POST", f"/api/admin/chat/sessions/{pid}/reply",
                   {"body": "Hello from the PCI team — happy to help further."}, token=tok)
        sp, jp = poll(primary, 0)
        pmsgs = jp.get("messages", [])
        has_agent = any(m["sender"] == "agent" and "PCI team" in m["body"] for m in pmsgs)
        check("8. admin reply -> visitor sees agent msg + status live",
              s == 200 and j.get("status") == "live"
              and sp == 200 and jp.get("status") == "live" and has_agent,
              f"reply_status={s} poll_status={jp.get('status')} has_agent={has_agent}")

        # (9) admin close -> status 'closed'
        s, j = req("POST", f"/api/admin/chat/sessions/{pid}/close", {}, token=tok)
        sp, jp = poll(primary, 0)
        s2, j2 = send(primary, "one more?", ip="10.20.0.1")
        check("9. admin close -> status closed (visitor send 409)",
              s == 200 and j.get("status") == "closed"
              and sp == 200 and jp.get("status") == "closed" and s2 == 409,
              f"close_status={s} poll_status={jp.get('status')} send_status={s2}")

        # (10) KB: add a row with a unique keyword, ask -> served; toggle off -> not served
        marker = "zephyrquux"
        answer = "Bespoke QA answer: the zephyrquux marker confirms a fresh knowledge-base entry."
        s, j = req("POST", "/api/admin/chat/kb",
                   {"question": "What is the zephyrquux marker?", "answer": answer,
                    "keywords": "zephyrquux,marker-test"}, token=tok)
        kb_id = j.get("id")
        check("10a. KB add row", s == 200 and j.get("ok") and kb_id, f"status={s} body={j}")

        s, j = start("KB Tester", ip="10.20.0.4")
        ktok = j.get("token")
        s, j = send(ktok, "please tell me about the zephyrquux thing", ip="10.20.0.4")
        r10 = (j.get("replies") or [{}])[0].get("body", "")
        check("10b. new KB answer is served", s == 200 and marker in r10, f"reply={r10!r}")

        s, j = req("POST", f"/api/admin/chat/kb/{kb_id}", {"action": "toggle"}, token=tok)
        check("10c. KB toggle off", s == 200 and j.get("ok"), f"status={s} body={j}")

        s, j = start("KB Tester 2", ip="10.20.0.5")
        ktok2 = j.get("token")
        s, j = send(ktok2, "again about the zephyrquux thing please", ip="10.20.0.5")
        r10d = (j.get("replies") or [{}])[0].get("body", "")
        check("10d. disabled KB answer no longer served",
              s == 200 and marker not in r10d and ("person" in r10d.lower() or "team" in r10d.lower() or "faq" in r10d.lower()),
              f"reply={r10d!r}")

        # ---- frontend checks ----
        print("== frontend checks ==")
        chat_js = open(os.path.join(BACKEND, "wwwroot", "assets", "chat.js"), encoding="utf-8").read()
        # innerHTML only ever used with static developer SVG/markup — never with message/user/bot data.
        import re
        bad = re.findall(r"innerHTML\s*=\s*([^;]+);", chat_js)
        dynamic_innerhtml = [x for x in bad if any(t in x for t in ("body", "greeting", "message", "res.", "m.", "state.", "name"))]
        check("F1. chat.js: no innerHTML with dynamic content",
              not dynamic_innerhtml and "textContent" in chat_js and "createElement" in chat_js,
              f"offending={dynamic_innerhtml}")

        admin_chat = open(os.path.join(BACKEND, "wwwroot", "admin-chat.html"), encoding="utf-8").read()
        class P(HTMLParser):
            def __init__(self): super().__init__(); self.tags = []
            def handle_starttag(self, tag, attrs): self.tags.append(tag)
        p = P(); perr = None
        try: p.feed(admin_chat); p.close()
        except Exception as e: perr = e
        check("F2. admin-chat.html parses (html.parser)",
              perr is None and "script" in p.tags and "textarea" in p.tags, str(perr))
        # admin-chat dynamic JS section should not build message bubbles via innerHTML
        acbad = re.findall(r"innerHTML\s*=\s*([^;]+);", admin_chat)
        ac_dynamic = [x for x in acbad if any(t in x for t in ("body", "message", "m.", "visitor", "r.answer", "r.question"))]
        check("F3. admin-chat.html: no innerHTML with dynamic content",
              not ac_dynamic and "textContent" in admin_chat and "createElement" in admin_chat,
              f"offending={ac_dynamic}")

        # served by the static host
        try:
            with urllib.request.urlopen(BASE + "/admin-chat.html", timeout=10) as r:
                check("F4. GET /admin-chat.html serves 200", r.status == 200)
        except Exception as e:
            check("F4. GET /admin-chat.html serves 200", False, str(e))
        try:
            with urllib.request.urlopen(BASE + "/assets/chat.js", timeout=10) as r:
                check("F5. GET /assets/chat.js serves 200", r.status == 200)
        except Exception as e:
            check("F5. GET /assets/chat.js serves 200", False, str(e))

        # premium.js injector block present and gated
        prem = open(os.path.join(BACKEND, "wwwroot", "assets", "premium.js"), encoding="utf-8").read()
        check("F6. premium.js chat injector present + gated",
              "PCI-CHAT-INJECT" in prem and "assets/chat.js" in prem
              and "exam-ui" in prem and "student" in prem and "admin" in prem
              and "PCI-LANGBAR" in prem and "PCI-SOCIAL" in prem,
              "injector or existing blocks missing")

    finally:
        srv.terminate()
        try: srv.wait(timeout=10)
        except Exception: srv.kill()
        logf.close()

    # ---- summary ----
    failed = [n for n, ok, _ in results if not ok]
    print("\n==================== SUMMARY ====================")
    for n, ok, _ in results:
        print(f"  {'PASS' if ok else 'FAIL'}  {n}")
    print(f"\n{len(results) - len(failed)}/{len(results)} passed")
    if failed:
        print("FAILED:", ", ".join(failed))
        sys.exit(1)
    print("ALL CHAT TESTS PASSED")

if __name__ == "__main__":
    main()
