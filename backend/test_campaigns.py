#!/usr/bin/env python3
"""End-to-end tests for the bulk / marketing email campaign system (Endpoints/Campaigns.cs +
Data/Migrate.cs tables). Boots the real server against a THROWAWAY sqlite db with a KNOWN
NEWSLETTER_SALT (so python can compute the same one-click unsubscribe token), seeds subscribers
via the public /api/newsletter endpoint, then exercises the full admin flow: draft -> validation ->
audience preview -> test send -> unsubscribe (wrong + correct token) -> bulk send -> double-send
guard -> manual suppression. With no SMTP configured the Mailer uses its console sink, so delivery
is recorded but nothing leaves the box. Nothing touches the real ./pci.db.
"""
import hashlib, hmac, json, os, shutil, subprocess, sys, tempfile, time, urllib.parse, urllib.request, urllib.error

BACKEND = os.path.dirname(os.path.abspath(__file__))
PORT = int(os.environ.get("CAMPAIGN_TEST_PORT", "18097"))
BASE = f"http://127.0.0.1:{PORT}"
ADMIN_EMAIL = "owner@test.local"
ADMIN_PW = "changeme-owner-test"
NEWSLETTER_SALT = "campaign-test-salt-123"
SUBS = ["alice@example.com", "bob@example.com", "carol@example.com"]

results = []
def check(name, ok, detail=""):
    results.append((name, bool(ok), detail))
    print(f"  [{'PASS' if ok else 'FAIL'}] {name}" + (f"  — {detail}" if detail and not ok else ""))

def unsub_token(email):
    mac = hmac.new(NEWSLETTER_SALT.encode(), email.strip().lower().encode(), hashlib.sha256).hexdigest()
    return mac[:24]

def find_dotnet():
    for c in (shutil.which("dotnet"), "/usr/bin/dotnet", "/usr/lib/dotnet/dotnet"):
        if c and os.path.exists(c):
            return c
    sys.exit("dotnet not found")

def req(method, path, body=None, token=None, raw=False):
    data = json.dumps(body).encode() if body is not None else None
    r = urllib.request.Request(BASE + path, data=data, method=method)
    r.add_header("Content-Type", "application/json")
    if token: r.add_header("Authorization", "Bearer " + token)
    try:
        with urllib.request.urlopen(r, timeout=20) as resp:
            text = resp.read().decode()
            return resp.status, (text if raw else (json.loads(text or "{}")))
    except urllib.error.HTTPError as e:
        text = e.read().decode()
        try: return e.code, (text if raw else json.loads(text or "{}"))
        except Exception: return e.code, ({} if not raw else text)

def main():
    dotnet = find_dotnet()
    print("== dotnet build ==")
    b = subprocess.run([dotnet, "build"], cwd=BACKEND, capture_output=True, text=True)
    print("\n".join(b.stdout.splitlines()[-4:]))
    if b.returncode != 0:
        print(b.stdout[-4000:]); print(b.stderr[-2000:]); sys.exit("BUILD FAILED")
    dll = os.path.join(BACKEND, "bin", "Debug", "net8.0", "PCI.Backend.dll")
    if not os.path.exists(dll): sys.exit(f"built dll not found: {dll}")

    scratch = tempfile.mkdtemp(prefix="pci-campaign-test-")
    env = dict(os.environ, PORT=str(PORT),
               DATABASE_FILE=os.path.join(scratch, "campaign-test.db"),
               STORAGE_ROOT=os.path.join(scratch, "storage"),
               ADMIN_OWNER_EMAIL=ADMIN_EMAIL, ADMIN_OWNER_PASSWORD=ADMIN_PW,
               NEWSLETTER_SALT=NEWSLETTER_SALT)
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
        print(f"server up on {BASE}\n== campaign tests ==")

        # (1) seed audience: public newsletter subscribers
        seeded = 0
        for e in SUBS:
            s, j = req("POST", "/api/newsletter", {"email": e})
            if s == 200 and j.get("ok"): seeded += 1
        check("1. seed subscribers (public /api/newsletter)", seeded == len(SUBS), f"seeded={seeded}")

        # (2) admin login (+ clear forced password change)
        s, j = req("POST", "/api/admin/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PW})
        tok = j.get("token")
        if tok:
            req("POST", "/api/admin/me/password", {"new_password": "campaign-test-password-1"}, token=tok)
        check("2. admin login", bool(tok), f"status={s}")

        # (3) create a draft campaign (audience=subscribers) + validation
        body_html = "Hi {{first_name}}, welcome to the Project Controls Institute. This is a proper campaign body."
        s, j = req("POST", "/api/admin/campaigns",
                   {"name": "July Update", "subject": "PCI July Update for {{first_name}}",
                    "body": body_html, "audience": "subscribers"}, token=tok)
        cid = j.get("id")
        check("3a. create draft -> id", s == 200 and isinstance(cid, int), f"status={s} body={j}")
        s, j = req("POST", "/api/admin/campaigns",
                   {"name": "Bad", "subject": "", "body": body_html, "audience": "subscribers"}, token=tok)
        check("3b. empty subject rejected (400)", s == 400, f"status={s} body={j}")

        # (4) audience preview matches the subscribed count
        s, j = req("POST", "/api/admin/campaigns/audience-preview", {"audience": "subscribers"}, token=tok)
        pre = j.get("count")
        check("4. audience-preview count == subscribed count", s == 200 and pre == len(SUBS),
              f"count={pre} expected={len(SUBS)}")

        # (5) send a test -> ok + notification_history row created
        s0, j0 = req("GET", "/api/admin/notifications", token=tok)
        before_hist = len(j0.get("recent", []))
        s, j = req("POST", f"/api/admin/campaigns/{cid}/test", {"to": "reviewer@example.com"}, token=tok)
        ok_test = s == 200 and j.get("ok")
        s1, j1 = req("GET", "/api/admin/notifications", token=tok)
        hist_rows = [r for r in j1.get("recent", []) if r.get("related_type") == "campaign"]
        check("5. test send ok + notification_history row", ok_test and len(hist_rows) >= 1,
              f"status={s} body={j} hist={len(hist_rows)}")

        # (6) WRONG unsubscribe token -> NOT suppressed (still subscribed)
        victim = SUBS[0]
        s, txt = req("GET", f"/api/unsubscribe?e={urllib.parse.quote(victim)}&t=deadbeefdeadbeefdeadbeef", raw=True)
        s2, j2 = req("POST", "/api/admin/campaigns/audience-preview", {"audience": "subscribers"}, token=tok)
        still = j2.get("count")
        check("6. wrong token does NOT suppress", s == 200 and still == len(SUBS),
              f"status={s} count_after_bad={still}")

        # (7) CORRECT token (computed in python with the known salt) -> suppressed + status unsubscribed
        good = unsub_token(victim)
        s, txt = req("GET", f"/api/unsubscribe?e={urllib.parse.quote(victim)}&t={good}", raw=True)
        html_ok = s == 200 and "unsubscrib" in (txt or "").lower()
        s3, j3 = req("GET", "/api/admin/suppression", token=tok)
        supp_emails = {r.get("email") for r in j3.get("rows", [])}
        s4, j4 = req("GET", "/api/admin/subscribers", token=tok)
        vrow = next((r for r in j4.get("rows", []) if r.get("email") == victim), None)
        check("7. correct token suppresses + marks unsubscribed",
              html_ok and victim in supp_emails and vrow and vrow.get("status") == "unsubscribed",
              f"html_ok={html_ok} in_supp={victim in supp_emails} sub_status={vrow and vrow.get('status')}")

        # (8) audience preview dropped by one after the unsubscribe
        s, j = req("POST", "/api/admin/campaigns/audience-preview", {"audience": "subscribers"}, token=tok)
        check("8. preview count dropped by one", s == 200 and j.get("count") == len(SUBS) - 1,
              f"count={j.get('count')} expected={len(SUBS)-1}")

        # (9) send the campaign -> ok + total; poll until 'sent'
        s, j = req("POST", f"/api/admin/campaigns/{cid}/send", token=tok)
        sent_ok = s == 200 and j.get("ok") and isinstance(j.get("total"), int)
        total = j.get("total")
        final = None
        deadline = time.time() + 15
        while time.time() < deadline:
            s5, det = req("GET", f"/api/admin/campaigns/{cid}", token=tok)
            camp = det.get("campaign", {})
            if camp.get("status") == "sent":
                final = det; break
            time.sleep(0.5)
        camp = (final or {}).get("campaign", {})
        # unsubscribed victim must NOT appear as a 'sent' recipient
        recips = (final or {}).get("recipients", [])
        victim_sent = any(r.get("email") == victim and r.get("status") == "sent" for r in recips)
        check("9. bulk send completes; sent>=1; unsubscribed addr not sent",
              sent_ok and final is not None and camp.get("status") == "sent"
              and (camp.get("sent") or 0) >= 1 and not victim_sent,
              f"sent_ok={sent_ok} total={total} status={camp.get('status')} sent={camp.get('sent')} victim_sent={victim_sent}")

        # (10) double-send is rejected
        s, j = req("POST", f"/api/admin/campaigns/{cid}/send", token=tok)
        check("10. double-send rejected", s != 200 or not j.get("ok"), f"status={s} body={j}")

        # (11) manual suppression add/remove
        manual = "manual@example.com"
        req("POST", "/api/admin/suppression", {"email": manual, "action": "add"}, token=tok)
        s, j = req("GET", "/api/admin/suppression", token=tok)
        added = manual in {r.get("email") for r in j.get("rows", [])}
        req("POST", "/api/admin/suppression", {"email": manual, "action": "remove"}, token=tok)
        s, j = req("GET", "/api/admin/suppression", token=tok)
        removed = manual not in {r.get("email") for r in j.get("rows", [])}
        check("11. manual suppression add + remove", added and removed, f"added={added} removed={removed}")

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
    print("ALL CAMPAIGN TESTS PASSED")

if __name__ == "__main__":
    main()
