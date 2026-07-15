#!/usr/bin/env python3
"""End-to-end tests for admin-managed social media links (Endpoints/Social.cs + wwwroot/admin.html).

Boots the real ASP.NET Core server against a THROWAWAY sqlite database (DATABASE_FILE env —
the real ./pci.db is never touched), waits for /api/health, then exercises the public
GET /api/social and the admin GET/POST /api/admin/social endpoints via urllib.

Server-boot + admin-bootstrap pattern is copied from test_forum.py.
"""
import json, os, shutil, subprocess, sys, tempfile, time, urllib.request, urllib.error
from html.parser import HTMLParser

BACKEND = os.path.dirname(os.path.abspath(__file__))
PORT = int(os.environ.get("SOCIAL_TEST_PORT", "18097"))
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
    scratch = tempfile.mkdtemp(prefix="pci-social-test-")
    env = dict(os.environ,
               PORT=str(PORT),
               DATABASE_FILE=os.path.join(scratch, "social-test.db"),
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
        print(f"server up on {BASE} (db: {env['DATABASE_FILE']})\n== social API tests ==")

        ADMIN_FIELDS = {"social_enabled", "linkedin", "x", "facebook", "instagram", "youtube", "whatsapp"}
        URLS = {
            "linkedin": "https://www.linkedin.com/company/pci",
            "x": "https://x.com/pci",
            "facebook": "https://www.facebook.com/pci",
            "instagram": "https://www.instagram.com/pci",
            "youtube": "https://www.youtube.com/@pci",
            "whatsapp": "https://wa.me/1234567890",
        }

        # (1) public GET /api/social empty by default
        s, j = req("GET", "/api/social")
        links = j.get("links", {}) if isinstance(j, dict) else {}
        check("1. GET /api/social returns no entries by default",
              s == 200 and isinstance(links, dict) and len(links) == 0, f"status={s} body={j}")

        # (2) admin login + clear forced password change
        print("== admin tests ==")
        s, j = req("POST", "/api/admin/auth/login", {"email": ADMIN_EMAIL, "password": ADMIN_PW})
        tok = j.get("token")
        check("2. seeded owner admin can log in", s == 200 and tok, f"status={s} body={j}")
        s, j = req("POST", "/api/admin/me/password", {"new_password": "social-test-password-1"}, token=tok)
        check("2b. clear must_change_pw", s == 200 and j.get("ok"), f"status={s} body={j}")

        # unauthenticated admin GET rejected
        s, j = req("GET", "/api/admin/social")
        check("2c. GET /api/admin/social without token -> 401", s == 401, f"status={s}")

        # (3) POST all six URLs + enable=1
        s, j = req("POST", "/api/admin/social", dict(URLS, social_enabled=1), token=tok)
        check("3. POST all six URLs + enable=1", s == 200 and j.get("ok"), f"status={s} body={j}")

        # admin GET reflects stored values + toggle
        s, ja = req("GET", "/api/admin/social", token=tok)
        check("3b. admin GET reflects stored values + enabled toggle",
              s == 200 and ja.get("social_enabled") is True
              and all(ja.get(k) == v for k, v in URLS.items()), f"status={s} body={ja}")

        # (4) public GET returns exactly the enabled non-empty entries, no admin-only fields
        s, j = req("GET", "/api/social")
        links = j.get("links", {}) if isinstance(j, dict) else {}
        no_admin = "social_enabled" not in j and (not isinstance(j, dict) or "social_enabled" not in links)
        check("4. GET /api/social returns exactly the 6 URLs, correct values, no admin fields",
              s == 200 and links == URLS and no_admin, f"status={s} body={j}")

        # (5) non-http value in one field -> excluded from public /api/social
        bad = dict(URLS, facebook="ftp://not-a-web-link.example", social_enabled=1)
        s, j = req("POST", "/api/admin/social", bad, token=tok)
        s2, j2 = req("GET", "/api/social")
        links2 = j2.get("links", {}) if isinstance(j2, dict) else {}
        expected = {k: v for k, v in URLS.items() if k != "facebook"}
        check("5. non-http value excluded from public /api/social",
              s == 200 and s2 == 200 and "facebook" not in links2 and links2 == expected,
              f"post={s} get={s2} links={links2}")
        # admin GET still stores the raw (invalid) value
        s, ja = req("GET", "/api/admin/social", token=tok)
        check("5b. admin GET still stores the raw invalid value",
              s == 200 and ja.get("facebook") == "ftp://not-a-web-link.example", f"status={s} facebook={ja.get('facebook')}")

        # (6) enable=0 -> public GET empty again
        s, j = req("POST", "/api/admin/social", {"social_enabled": 0}, token=tok)
        s2, j2 = req("GET", "/api/social")
        links3 = j2.get("links", {}) if isinstance(j2, dict) else {}
        check("6. POST enable=0 -> GET /api/social empty again",
              s == 200 and s2 == 200 and len(links3) == 0, f"post={s} get={s2} links={links3}")

        # ---- frontend checks ----
        print("== frontend checks ==")
        html = open(os.path.join(BACKEND, "wwwroot", "admin.html"), encoding="utf-8").read()

        class P(HTMLParser):
            def __init__(self):
                super().__init__(); self.tags = []
            def handle_starttag(self, tag, attrs): self.tags.append(tag)
        errors = None
        p = P()
        try: p.feed(html); p.close()
        except Exception as e: errors = e
        check("F1. admin.html parses (html.parser)", errors is None, str(errors))
        check("F2. Social media section wired into nav (renderSocial + label)",
              "renderSocial" in html and "Social media" in html and "id:'social'" in html)
        check("F3. six networks in NETS + soc-input class + master toggle present",
              all(("'" + k + "'") in html for k in ("linkedin", "x", "facebook", "instagram", "youtube", "whatsapp"))
              and "soc-input" in html and "socEnabled" in html)
        check("F4. Save posts to /api/admin/social", "/api/admin/social" in html and "saveSocial" in html)
        check("F5. status line uses textContent (no innerHTML with server status)",
              "socStatus" in html and "st.textContent" in html)

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
    print("ALL SOCIAL TESTS PASSED")

if __name__ == "__main__":
    main()
