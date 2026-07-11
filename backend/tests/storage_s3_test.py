#!/usr/bin/env python3
"""
Live test of the S3 storage provider (Core/Storage.cs, STORAGE_PROVIDER=s3) against a local
moto S3 server — real HTTP, real AWS SDK signing, no AWS account.

Proves, end-to-end through the app's own API:
  1. upload lands in the BUCKET (not on local disk), DB row holds an s3:<key> reference only
  2. the authenticated attachment fetch streams the bytes back from S3
  3. PurgeOlderThan removes aged S3 objects (retention works on the S3 backend)
  4. with STORAGE_PROVIDER=s3 but no S3_BUCKET, the app falls back to local with a warning

Requires:  pip install 'moto[server]'   (skipped gracefully if moto is unavailable)
Run from backend/:  python3 tests/storage_s3_test.py
"""
import base64, json, os, socket, sqlite3, subprocess, sys, time, hashlib, urllib.request, urllib.error

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_s3.db")
BUCKET = "pci-artefacts-test"

try:
    import moto  # noqa
except ImportError:
    print("moto not installed — skipping S3 provider test (pip install 'moto[server]')")
    sys.exit(0)

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

S3PORT, APPPORT = free_port(), free_port()
S3EP = f"http://127.0.0.1:{S3PORT}"
BASE = f"http://127.0.0.1:{APPPORT}"

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def req(method, path, token=None, body=None, base=None):
    hdr = {}; data = None
    if body is not None: data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    if token: hdr["Authorization"] = "Bearer " + token
    r = urllib.request.Request((base or BASE) + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r, timeout=15) as resp: return resp.status, resp.read()
    except urllib.error.HTTPError as e:
        return e.code, e.read()

def main():
    for f in (DB, DB+"-wal", DB+"-shm"):
        try: os.remove(f)
        except OSError: pass

    # Start the mock S3 server. If it can't be launched or never becomes reachable (e.g. a CI runner
    # without `moto.server`, or a restricted sandbox), SKIP rather than fail — this exercises the optional
    # S3 provider and must never redden the pipeline for an environment reason. A genuine S3-logic
    # regression still fails the suite once the server IS up.
    try:
        moto_p = subprocess.Popen([sys.executable, "-m", "moto.server", "-p", str(S3PORT)],
                                  stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    except Exception as e:
        print(f"moto.server could not be launched ({e}) — skipping S3 provider test"); sys.exit(0)
    app_p = None
    try:
        up = False
        for _ in range(40):
            try: urllib.request.urlopen(S3EP, timeout=2); up = True; break
            except urllib.error.HTTPError: up = True; break   # moto answers 404 at / — that's up
            except Exception: time.sleep(0.5)
        if not up:
            print("moto S3 server never became reachable — skipping S3 provider test")
            moto_p.terminate(); sys.exit(0)
        # create the bucket via the raw S3 REST API (no boto3 needed)
        urllib.request.urlopen(urllib.request.Request(f"{S3EP}/{BUCKET}", method="PUT"))

        env = dict(os.environ, DATABASE_FILE=DB, PORT=str(APPPORT),
                   STORAGE_PROVIDER="s3", S3_BUCKET=BUCKET, S3_ENDPOINT=S3EP, S3_REGION="us-east-1",
                   AWS_ACCESS_KEY_ID="testing", AWS_SECRET_ACCESS_KEY="testing",
                   STORAGE_ROOT=os.path.join(HERE, "_s3_localroot"))
        app_p = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND,
                                 stdout=open(os.path.join(HERE, "_s3_server.log"), "w"), stderr=subprocess.STDOUT)
        for _ in range(60):
            try:
                if req("GET", "/api/health")[0] == 200: break
            except Exception: pass
            time.sleep(0.5)

        boot = open(os.path.join(HERE, "_s3_server.log")).read()
        chk("s3 backend announced at boot", f"s3 bucket '{BUCKET}'" in boot, boot[:200])

        # student + ticket (direct rows; the upload/fetch path is what we're testing)
        con = sqlite3.connect(DB)
        uid = con.execute("INSERT INTO users(email,role,status) VALUES('s3@ex.co','student','active') RETURNING id").fetchone()[0]
        tok = "s3tok"
        con.execute("INSERT INTO login_tokens(user_id,token,purpose,expires_at) VALUES(?,?, 'session', datetime('now','+1 day'))",
                    (uid, hashlib.sha256(tok.encode()).hexdigest()))
        tid = con.execute("INSERT INTO tickets(user_id,reference,subject,category,status) VALUES(?, 'T-S3','s','general','open') RETURNING id", (uid,)).fetchone()[0]
        con.commit(); con.close()

        pdf_bytes = b"%PDF-1.4 s3 roundtrip test"
        data_uri = "data:application/pdf;base64," + base64.b64encode(pdf_bytes).decode()
        c, up = req("POST", f"/api/me/tickets/{tid}/attachments", token=tok, body={"filename": "r.pdf", "data_uri": data_uri})
        chk("upload via API accepted", c == 200, up[:200])

        con = sqlite3.connect(DB)
        ref = con.execute("SELECT storage_ref FROM support_attachments WHERE ticket_id=?", (tid,)).fetchone()
        aid = con.execute("SELECT id FROM support_attachments WHERE ticket_id=?", (tid,)).fetchone()[0]
        con.close()
        chk("DB reference is s3:<key>", ref and ref[0].startswith("s3:"), ref)
        key = ref[0].split(":", 1)[1]

        # Object exists in the bucket: moto answers an unsigned GET with 403 for an EXISTING key and
        # 404 for a missing one (the post-purge check below relies on exactly that 404). The byte-level
        # equality is proven by the authed fetch check, which streams from S3 through the app.
        c2, s3body = req("GET", f"/{BUCKET}/{key}", base=S3EP)
        chk("object present in the S3 bucket (unsigned GET → 403 not 404)", c2 in (200, 403), c2)
        local_root = os.path.join(HERE, "_s3_localroot")
        on_disk = os.path.exists(os.path.join(local_root, key))
        chk("bytes NOT written to local disk", not on_disk)

        # authenticated fetch streams from S3 through the app
        c3, fetched = req("GET", f"/api/me/tickets/{tid}/attachments/{aid}", token=tok)
        chk("authed fetch streams S3 bytes back", c3 == 200 and pdf_bytes in fetched, (c3, fetched[:40]))

        # retention purge on the S3 backend: age the object by rewriting is not possible via REST here,
        # so purge with days=0 must remove objects modified before 'now' — moto sets LastModified at PUT,
        # a second earlier than the purge call.
        time.sleep(1.2)
        _, ob = req("POST", "/api/admin/auth/login", body={"email": "owner@pci.local", "password": "changeme-owner"})
        otok = json.loads(ob)["token"]
        # clear the forced-password-change flag so the owner token can operate the console
        req("POST", "/api/admin/me/password", token=otok, body={"new_password": "Op3rator!Pw"})
        req("PATCH", "/api/admin/settings", token=otok, body={"evidence_retention_days": "0"})
        c4, pg = req("POST", "/api/admin/storage/purge", token=otok, body={})
        pg = json.loads(pg)
        chk("purge removed the aged S3 object", c4 == 200 and pg.get("removed", 0) >= 1, pg)
        c5, _ = req("GET", f"/{BUCKET}/{key}", base=S3EP)
        chk("object gone from the bucket after purge", c5 == 404, c5)
    finally:
        if app_p: app_p.terminate()
        moto_p.terminate()
        for p in (app_p, moto_p):
            try:
                if p: p.wait(timeout=10)
            except Exception:
                p.kill()

    # fallback honesty: s3 selected but no bucket → local fallback with a warning
    env2 = dict(os.environ, DATABASE_FILE=DB, PORT=str(free_port()), STORAGE_PROVIDER="s3",
                STORAGE_ROOT=os.path.join(HERE, "_s3_localroot"))
    env2.pop("S3_BUCKET", None)
    p2 = subprocess.Popen(["dotnet", DLL], env=env2, cwd=BACKEND, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, text=True)
    line = ""
    t0 = time.time()
    while time.time() - t0 < 20:
        l = p2.stdout.readline()
        if "storage:" in l: line = l; break
    p2.terminate()
    try: p2.wait(timeout=10)
    except Exception: p2.kill()
    chk("no-bucket fallback warns and uses local", "S3_BUCKET is not set" in line and "falling back to local" in line, line.strip())

    print(f"\n  ══ {passed}/{passed+failed} PASSED ══")
    sys.exit(0 if failed == 0 else 1)

if __name__ == "__main__":
    main()
