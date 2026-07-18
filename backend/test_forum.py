#!/usr/bin/env python3
"""End-to-end tests for the public discussion forum (Endpoints/Forum.cs + wwwroot/forum.html).

Boots the real ASP.NET Core server against a THROWAWAY sqlite database (DATABASE_FILE env —
the real ./pci.db is never touched), waits for /api/health, then exercises every public and
admin endpoint via urllib. Client IPs are varied with X-Forwarded-For: the app trusts the LAST
XFF hop (H.LastHopIp), so with no fronting proxy the header directly sets the rate-limit bucket.
"""
import json, os, shutil, subprocess, sys, tempfile, time, urllib.request, urllib.error
from html.parser import HTMLParser

BACKEND = os.path.dirname(os.path.abspath(__file__))
PORT = int(os.environ.get("FORUM_TEST_PORT", "18093"))
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
    scratch = tempfile.mkdtemp(prefix="pci-forum-test-")
    env = dict(os.environ,
               PORT=str(PORT),
               DATABASE_FILE=os.path.join(scratch, "forum-test.db"),
               STORAGE_ROOT=os.path.join(scratch, "storage"),
               ADMIN_OWNER_EMAIL=ADMIN_EMAIL,
               ADMIN_OWNER_PASSWORD=ADMIN_PW,
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
                logf.flush(); print(open(logf.name).read()[-4000:]); sys.exit("server exited during boot")
            time.sleep(0.5)
        else:
            sys.exit("server never became healthy")
        print(f"server up on {BASE} (db: {env['DATABASE_FILE']})\n== forum API tests ==")

        # (1) categories
        s, j = req("GET", "/api/forum/categories")
        keys = [c.get("key") for c in j.get("categories", [])]
        check("1. categories list (5 fixed categories with labels + counts)",
              s == 200 and keys == ["general", "exam-prep", "domains", "ai-controls", "careers"]
              and all("label" in c and "threads" in c for c in j["categories"]), f"status={s} keys={keys}")

        # (2) create thread OK
        s, j = req("POST", "/api/forum/threads", {
            "name": "Jane Practitioner", "title": "How do you baseline EVM on hybrid projects?",
            "body": "We run hybrid delivery and I am unsure how to set the PMB when half the scope is agile. What do you do?",
            "category": "general", "website": ""}, ip="10.9.0.1")
        tid = j.get("id")
        check("2. create thread OK", s == 200 and j.get("ok") and tid, f"status={s} body={j}")

        s, j = req("GET", "/api/forum/threads?category=general")
        total_before = j.get("total")
        check("2b. thread appears in listing with fields", s == 200 and total_before == 1
              and j["threads"][0]["title"].startswith("How do you baseline")
              and j["threads"][0]["reply_count"] == 0
              and "ip_hash" not in j["threads"][0], f"status={s} body={j}")

        # (3) honeypot filled -> ok:true but nothing stored
        s, j = req("POST", "/api/forum/threads", {
            "name": "Bot Bot", "title": "Totally legitimate discussion title",
            "body": "This body is long enough to pass validation but comes from a bot filling the trap.",
            "category": "general", "website": "https://spam.example"}, ip="10.9.0.2")
        s2, j2 = req("GET", "/api/forum/threads")
        check("3. honeypot filled -> ok:true, thread count unchanged",
              s == 200 and j.get("ok") and "id" not in j and j2.get("total") == total_before,
              f"status={s} body={j} total={j2.get('total')}")

        # (4) title too short -> 400
        s, j = req("POST", "/api/forum/threads", {
            "name": "Jane", "title": "Short", "category": "general", "website": "",
            "body": "A perfectly reasonable body of more than twenty characters."}, ip="10.9.0.1")
        check("4. title too short -> 400", s == 400 and j.get("error") == "bad_title", f"status={s} body={j}")

        # (5) too many links -> 400 too_many_links
        s, j = req("POST", "/api/forum/threads", {
            "name": "Jane", "title": "A valid title for a spammy post", "category": "general", "website": "",
            "body": "See http://a.example and https://b.example and HTTP://c.example for details."}, ip="10.9.0.1")
        check("5. too many links -> 400 too_many_links", s == 400 and j.get("error") == "too_many_links", f"status={s} body={j}")

        # (6) immediate second (valid) thread from same ip -> 429
        s, j = req("POST", "/api/forum/threads", {
            "name": "Jane Practitioner", "title": "Another perfectly valid discussion",
            "body": "Posting again straight away should hit the one-thread-per-two-minutes limit.",
            "category": "exam-prep", "website": ""}, ip="10.9.0.1")
        check("6. immediate second thread -> 429 rate_limited", s == 429 and j.get("error") == "rate_limited", f"status={s} body={j}")

        # (7) reply OK, reply_count increments
        s, j = req("POST", f"/api/forum/threads/{tid}/posts", {
            "name": "Sam Scheduler", "body": "We baseline the predictive scope only and track the agile scope with AgileEVM.",
            "website": ""}, ip="10.9.0.3")
        reply_id = j.get("id")
        s2, j2 = req("GET", f"/api/forum/threads/{tid}")
        check("7. reply OK -> reply_count 1, thread shows 2 live posts",
              s == 200 and j.get("ok") and reply_id
              and s2 == 200 and j2["thread"]["reply_count"] == 1 and len(j2["posts"]) == 2
              and all("ip_hash" not in p for p in j2["posts"]), f"status={s},{s2} posts={len(j2.get('posts', []))}")

        # (8) reply again within 30s -> 429
        s, j = req("POST", f"/api/forum/threads/{tid}/posts", {
            "name": "Sam Scheduler", "body": "Adding one more thought immediately.", "website": ""}, ip="10.9.0.3")
        check("8. reply again within 30s -> 429 rate_limited", s == 429 and j.get("error") == "rate_limited", f"status={s} body={j}")

        # (9) three reports from three different IPs -> post auto-hidden
        ok_reports = True
        for ip in ("10.9.0.4", "10.9.0.5", "10.9.0.6"):
            s, j = req("POST", f"/api/forum/posts/{reply_id}/report", {}, ip=ip)
            ok_reports = ok_reports and s == 200 and j.get("ok")
        s2, j2 = req("GET", f"/api/forum/threads/{tid}")
        check("9. 3 reports (3 IPs) -> post hidden, no longer in thread",
              ok_reports and s2 == 200 and len(j2["posts"]) == 1
              and j2["posts"][0]["author_name"] == "Jane Practitioner", f"reports_ok={ok_reports} posts={len(j2.get('posts', []))}")

        # ---- admin: login seeded owner, clear forced password change ----
        print("== admin moderation tests ==")
        s, j = req("POST", "/api/admin/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PW})
        tok = j.get("token")
        check("A1. seeded owner admin can log in", s == 200 and tok, f"status={s} body={j}")
        s, j = req("POST", "/api/admin/me/password", {"new_password": "forum-test-password-1"}, token=tok)
        check("A2. clear must_change_pw", s == 200 and j.get("ok"), f"status={s} body={j}")

        s, j = req("GET", "/api/admin/forum/queue")
        check("A3. queue without token -> 401", s == 401, f"status={s}")

        s, j = req("GET", "/api/admin/forum/queue", token=tok)
        qids = [p["id"] for p in j.get("posts", [])]
        check("A4. queue lists the auto-hidden flagged post (no ip_hash)",
              s == 200 and reply_id in qids
              and all("ip_hash" not in p for p in j.get("posts", [])), f"status={s} qids={qids}")

        s, j = req("POST", f"/api/admin/forum/posts/{reply_id}", {"action": "restore"}, token=tok)
        s2, j2 = req("GET", f"/api/forum/threads/{tid}")
        check("A5. restore post -> visible again, flags reset", s == 200 and j.get("ok")
              and len(j2["posts"]) == 2, f"status={s} posts={len(j2.get('posts', []))}")

        s, j = req("POST", f"/api/admin/forum/threads/{tid}", {"action": "lock"}, token=tok)
        s2, j2 = req("POST", f"/api/forum/threads/{tid}/posts", {
            "name": "Late Arrival", "body": "Trying to reply to a locked thread.", "website": ""}, ip="10.9.0.7")
        check("A6. lock thread -> reply rejected 409 locked", s == 200 and s2 == 409
              and j2.get("error") == "locked", f"status={s},{s2} body={j2}")

        s, j = req("POST", f"/api/admin/forum/threads/{tid}", {"action": "unlock"}, token=tok)
        s2, j2 = req("POST", f"/api/forum/threads/{tid}/posts", {
            "name": "Late Arrival", "body": "And now the thread is unlocked again.", "website": ""}, ip="10.9.0.7")
        check("A7. unlock thread -> reply accepted", s == 200 and s2 == 200 and j2.get("ok"), f"status={s},{s2} body={j2}")

        # (10) hidden thread -> 404 for readers and repliers
        s, j = req("POST", f"/api/admin/forum/threads/{tid}", {"action": "hide"}, token=tok)
        s2, j2 = req("GET", f"/api/forum/threads/{tid}")
        s3, j3 = req("POST", f"/api/forum/threads/{tid}/posts", {
            "name": "Reader", "body": "Replying to a hidden thread.", "website": ""}, ip="10.9.0.8")
        s4, j4 = req("GET", "/api/forum/threads")
        check("10. hidden thread -> GET 404, reply 404, absent from listing",
              s == 200 and s2 == 404 and s3 == 404 and j4.get("total") == 0,
              f"status={s},{s2},{s3} total={j4.get('total')}")

        s, j = req("POST", f"/api/admin/forum/threads/{tid}", {"action": "restore"}, token=tok)
        s2, j2 = req("GET", f"/api/forum/threads/{tid}")
        check("A8. restore thread -> live again", s == 200 and s2 == 200
              and j2["thread"]["status"] == "live", f"status={s},{s2}")

        s, j = req("POST", f"/api/admin/forum/threads/{tid}", {"action": "delete"}, token=tok)
        s2, _ = req("GET", f"/api/forum/threads/{tid}")
        s3, j3 = req("GET", "/api/admin/forum/queue", token=tok)
        gone = all(p.get("thread_id") != tid for p in j3.get("posts", []))
        check("A9. delete thread -> 404 and posts removed", s == 200 and s2 == 404 and gone, f"status={s},{s2}")

        # ---- frontend checks ----
        print("== frontend checks ==")
        html = open(os.path.join(BACKEND, "wwwroot", "forum.html"), encoding="utf-8").read()

        class P(HTMLParser):
            def __init__(self):
                super().__init__(); self.tags = []
            def handle_starttag(self, tag, attrs): self.tags.append(tag)
        p = P(); errors = None
        try: p.feed(html); p.close()
        except Exception as e: errors = e
        check("F1. forum.html parses (html.parser)", errors is None and "footer" in p.tags and "main" in p.tags, str(errors))

        forum_js = html.split("PCI Community Forum")[1].split("</script>")[0]
        check("F2. forum JS never uses innerHTML with user data",
              "innerHTML" not in forum_js and "textContent" in forum_js and "createElement" in forum_js)
        check("F3. honeypot 'website' field present and display:none",
              'name="website"' in html and html.count('style="display:none"') >= 2)
        check("F4. Community Forum link in footer Resources column",
              '<li><a href="forum.html">Community Forum</a></li>' in html)
        check("F5. title + hero copy", "<title>Community Forum | PCI</title>" in html
              and "The PCI discussion forum" in html and ">Community<" in html)
        check("F6. guidelines box present", "Community guidelines" in html
              and "examination content" in html and "employer-confidential" in html)

        # served by the static host too (wwwroot copied to bin on build)
        try:
            with urllib.request.urlopen(BASE + "/forum.html", timeout=10) as r:
                check("F7. GET /forum.html serves 200", r.status == 200)
        except Exception as e:
            check("F7. GET /forum.html serves 200", False, str(e))

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
    print("ALL FORUM TESTS PASSED")

if __name__ == "__main__":
    main()
