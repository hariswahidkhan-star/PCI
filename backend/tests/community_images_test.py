#!/usr/bin/env python3
"""
PCI World community images (Phase 2) — live HTTP suite.

Boots the REAL built backend and drives the image endpoints over real HTTP, so what is tested is
what a browser would actually get — including the background scan worker, which is where the
publication decision is made.

The assertion this suite exists for is the negative one:

    a second participant must never be able to load an image before the allow verdict,
    and must never be able to load one that was withheld or restricted AT ALL.

So most of what follows uploads something and then tries, from a different session, to see it —
and fails if it can.

Fixtures are ordinary generated PNGs. Where a test needs a WITHHELD or RESTRICTED verdict it paints
a marker colour into the top-left pixel, which the deterministic test scanner recognises. There is
no real harmful material in this suite and there never needs to be.

Exit code 0 iff every assertion passes.  Run from backend/:  python3 tests/community_images_test.py
"""
import base64, json, os, re, socket, sqlite3, struct, subprocess, sys, time, urllib.error, urllib.request, zlib

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_community_images.db")
STORAGE = os.path.join(HERE, "_community_images_storage")
OWNER_PW = "CommunityImagesOwner!99"

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_cimg") else _base + "_cimg")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, body=None, headers=None, raw=False):
    hdr = dict(headers or {})
    data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp:
            return resp.status, (resp.read() if raw else resp.read().decode())
    except urllib.error.HTTPError as e:
        return e.code, (e.read() if raw else e.read().decode())
    except urllib.error.URLError as e:
        return 0, (b"" if raw else str(e))

def js(path, method="GET", body=None, headers=None):
    code, text = req(method, path, body, headers)
    try: return code, json.loads(text) if text else {}
    except Exception: return code, {"_raw": text}

def sql(statement, args=()):
    if PROVIDER == "mysql":
        import pymysql
        con = pymysql.connect(**MYSQL); cur = con.cursor()
        cur.execute(statement.replace("INSERT OR REPLACE INTO", "REPLACE INTO").replace("?", "%s"), args)
        con.commit(); rows = cur.fetchall(); con.close(); return rows
    con = sqlite3.connect(DB); cur = con.cursor()
    cur.execute(statement, args); con.commit()
    rows = cur.fetchall(); con.close(); return rows

# ── fixtures ──────────────────────────────────────────────────────────────────────────────────
#
# A PNG is written by hand rather than pulled from a library so the suite has no image dependency
# and so a reader can see exactly what is being uploaded — including the marker pixel.

def png(marker=(0, 128, 200), w=8, h=8):
    """A tiny RGBA PNG whose FIRST pixel is `marker`. The deterministic test scanner reads that
    pixel to choose a verdict, which is how the withheld and restricted branches are reachable over
    HTTP: the scanner sees the server's re-encoded derivative, whose bytes a client never has, so a
    hash could not be registered from out here."""
    rows = b""
    for y in range(h):
        rows += b"\x00"                                   # filter type 0 for the scanline
        for x in range(w):
            px = marker if (x == 0 and y == 0) else (20, 30, 40)
            rows += bytes(px) + b"\xff"
    def chunk(tag, data):
        return struct.pack(">I", len(data)) + tag + data + struct.pack(">I", zlib.crc32(tag + data) & 0xFFFFFFFF)
    ihdr = struct.pack(">IIBBBBB", w, h, 8, 6, 0, 0, 0)   # 8-bit RGBA, no interlace
    return (b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", ihdr)
            + chunk(b"IDAT", zlib.compress(rows)) + chunk(b"IEND", b""))

def data_uri(payload, mime="image/png"):
    return f"data:{mime};base64," + base64.b64encode(payload).decode()

MARK_OK        = (0, 128, 200)
MARK_WITHHELD  = (255, 0, 255)
MARK_RESTRICTED = (255, 0, 254)

def _reset_mysql():
    import pymysql
    root = pymysql.connect(host=MYSQL["host"], port=MYSQL["port"], user=MYSQL["user"], password=MYSQL["password"])
    cur = root.cursor()
    cur.execute(f"DROP DATABASE IF EXISTS `{MYSQL['database']}`")
    cur.execute(f"CREATE DATABASE `{MYSQL['database']}` CHARACTER SET utf8mb4")
    root.commit(); root.close()

proc = None
def boot():
    global proc
    if PROVIDER == "mysql":
        _reset_mysql()
        env = dict(os.environ, PORT=str(PORT), STORAGE_ROOT=STORAGE, DB_PROVIDER="mysql",
                   MYSQL_DATABASE=MYSQL["database"], ASPNETCORE_ENVIRONMENT="Development",
                   PCIWORLD_OWNER_PASSWORD=OWNER_PW,
                   SEED_DEMO_EXAM="", CANONICAL_HOST="", CANONICAL_REDIRECT="")
        env.pop("MYSQL_CONNECTION_STRING", None)
    else:
        for p in (DB, DB + "-wal", DB + "-shm"):
            if os.path.exists(p): os.remove(p)
        env = dict(os.environ, PORT=str(PORT), DATABASE_FILE=DB, STORAGE_ROOT=STORAGE,
                   ASPNETCORE_ENVIRONMENT="Development", DB_PROVIDER="sqlite",
                   PCIWORLD_OWNER_PASSWORD=OWNER_PW,
                   SEED_DEMO_EXAM="", CANONICAL_HOST="", CANONICAL_REDIRECT="")
    proc = subprocess.Popen([os.environ.get("DOTNET", "dotnet"), DLL], cwd=BACKEND, env=env,
                            stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    for _ in range(240):
        code, _ = req("GET", "/api/health")
        if code == 200: return
        time.sleep(0.5)
    raise SystemExit("backend did not become healthy")

def shutdown():
    if proc:
        proc.terminate()
        try: proc.wait(timeout=15)
        except Exception: proc.kill()

def settle(media_id, timeout=25):
    """Wait for the background worker to reach a terminal state for this upload. Returns the state."""
    deadline = time.time() + timeout
    while time.time() < deadline:
        rows = sql("SELECT state FROM pciworld_community_media WHERE id=?", (media_id,))
        state = rows[0][0] if rows else None
        if state in ("allowed", "withheld", "restricted"): return state
        time.sleep(0.4)
    return state

def main():
    boot()
    try:
        # ── The feature ships dark, INDEPENDENTLY of rooms ─────────────────────────────────
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_enabled','1')")
        sql("""INSERT INTO pciworld_community_rooms(slug,title,description,state,capacity,rules_version,image_allowed)
               VALUES('gallery','Gallery','Images','open',10,'v1',1)""")

        code, r = js("/api/world/community/rooms/gallery/images", "POST", {"client_upload_id": "x"})
        chk("I01 uploads are 503 while images are off, even in a room that allows them",
            code == 503 and r.get("error") == "images_disabled", f"{code} {r}")

        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_community_images_enabled','1')")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_community_image_scanner','deterministic')")

        NET_A = {"X-Forwarded-For": "198.51.100.30"}
        NET_B = {"X-Forwarded-For": "198.51.100.40"}
        code, alice = js("/api/world/community/guest-sessions", "POST",
                         {"room": "gallery", "display_name": "Ayesha Iqbal", "rules_version": "v1"}, NET_A)
        chk("I02 a guest joins the image room", code == 200 and alice.get("token"), str(alice))
        A = {"X-Community-Session": alice["token"], **NET_A}

        code, bob = js("/api/world/community/guest-sessions", "POST",
                       {"room": "gallery", "display_name": "Omar Farooq", "rules_version": "v1"}, NET_B)
        chk("I03 a second guest joins, to watch", code == 200 and bob.get("token"))
        B = {"X-Community-Session": bob["token"], **NET_B}

        # ── Authorisation and validation ───────────────────────────────────────────────────
        code, r = js("/api/world/community/rooms/gallery/images", "POST",
                     {"client_upload_id": "anon", "data": data_uri(png()), "alt": "A chart"})
        chk("I04 an upload without a session is refused", code == 401, f"{code} {r}")

        code, r = js("/api/world/community/rooms/gallery/images", "POST",
                     {"client_upload_id": "svg1", "data": data_uri(
                         b"<svg xmlns='http://www.w3.org/2000/svg'><script/></svg>"), "alt": "Not an image"}, A)
        chk("I05 an SVG is refused outright", code == 400 and r.get("reason") == "svg_not_accepted", f"{code} {r}")

        code, r = js("/api/world/community/rooms/gallery/images", "POST",
                     {"client_upload_id": "noalt", "data": data_uri(png()), "alt": "  "}, A)
        chk("I06 alternative text is required", code == 400 and r.get("reason") == "alt_text_required", f"{code} {r}")

        code, r = js("/api/world/community/rooms/gallery/images", "POST",
                     {"client_upload_id": "junk", "data": data_uri(b"not an image at all"), "alt": "Junk"}, A)
        chk("I07 arbitrary bytes wearing an image type are refused",
            code == 400 and r.get("reason") in ("unrecognised_image", "content_mime_mismatch"), f"{code} {r}")

        # ── THE INVARIANT: nothing is visible before the verdict ───────────────────────────
        code, up = js("/api/world/community/rooms/gallery/images", "POST",
                      {"client_upload_id": "ok1", "data": data_uri(png(MARK_OK)), "alt": "A Gantt chart"}, A)
        chk("I08 a valid upload is accepted as PENDING, not published",
            code == 200 and up.get("ok") and up.get("state") == "pending" and up.get("media_id"), f"{code} {up}")
        chk("I09 the response hands back no URL — only a pending handle",
            "url" not in json.dumps(up).lower(), str(up))

        media_id = up["media_id"]
        code, body = req("GET", f"/api/world/community/media/{media_id}", headers=B, raw=True)
        chk("I10 a second participant cannot load a pending image", code == 404, f"got {code}")

        code, msgs = js("/api/world/community/rooms/gallery/messages", headers=B)
        chk("I11 a pending image is absent from the transcript", code == 200 and msgs["messages"] == [], str(msgs))

        # ── Allowed ────────────────────────────────────────────────────────────────────────
        state = settle(media_id)
        chk("I12 the background worker reaches a verdict", state == "allowed", f"state={state}")

        code, body = req("GET", f"/api/world/community/media/{media_id}", headers=B, raw=True)
        chk("I13 once allowed, the image is served to another participant",
            code == 200 and body[:8] == b"\x89PNG\r\n\x1a\n", f"{code} {body[:16]!r}")
        chk("I14 the served copy is the SERVER'S re-encode, not the uploaded bytes",
            body != png(MARK_OK), "the uploaded bytes were served verbatim")

        code, msgs = js("/api/world/community/rooms/gallery/messages", headers=B)
        got = msgs.get("messages", [])
        chk("I15 the published image appears in the transcript as an image message",
            len(got) == 1 and got[0]["kind"] == "image" and got[0]["media_id"] == media_id, str(msgs))
        chk("I16 the alternative text travels in the ordinary body field",
            got and got[0]["body"] == "A Gantt chart", str(got))

        # Idempotency is checked HERE, while Alice still has upload budget. Uploads are rate limited
        # per session at 6 per window — deliberately tighter than text, because an upload costs a
        # sanitise, a store and a classifier call — and this must be tested against the same session
        # that made the original, since idempotency is keyed on (session, client_upload_id).
        code, again = js("/api/world/community/rooms/gallery/images", "POST",
                         {"client_upload_id": "ok1", "data": data_uri(png(MARK_OK)), "alt": "A Gantt chart"}, A)
        chk("I17 a retried upload returns the original outcome rather than a second image",
            code == 200 and again.get("reason") == "duplicate" and again.get("media_id") == media_id, str(again))
        rows = sql("SELECT COUNT(*) FROM pciworld_media_scan_queue WHERE media_id=?", (media_id,))
        chk("I18 a retry queues no second scan", rows and rows[0][0] == 1, str(rows))

        # ── Withheld ───────────────────────────────────────────────────────────────────────
        # A fresh participant, because Alice's upload budget is nearly spent — and because a second
        # person uploading is what actually happens in a room.
        NET_C = {"X-Forwarded-For": "198.51.100.60"}
        code, carla = js("/api/world/community/guest-sessions", "POST",
                         {"room": "gallery", "display_name": "Nadia Rahman", "rules_version": "v1"}, NET_C)
        C = {"X-Community-Session": carla["token"], **NET_C}

        code, up2 = js("/api/world/community/rooms/gallery/images", "POST",
                       {"client_upload_id": "bad1", "data": data_uri(png(MARK_WITHHELD)), "alt": "Something"}, C)
        chk("I19 a to-be-withheld upload is still accepted as pending", code == 200 and up2.get("ok"), str(up2))
        state = settle(up2["media_id"])
        chk("I20 it settles as withheld", state == "withheld", f"state={state}")

        code, _ = req("GET", f"/api/world/community/media/{up2['media_id']}", headers=B, raw=True)
        chk("I21 a withheld image is never servable", code == 404, f"got {code}")

        code, msgs = js("/api/world/community/rooms/gallery/messages", headers=B)
        chk("I22 a withheld image never enters the transcript",
            len(msgs.get("messages", [])) == 1, str(msgs))

        # ── Restricted ─────────────────────────────────────────────────────────────────────
        NET_D = {"X-Forwarded-For": "198.51.100.70"}
        code, dara = js("/api/world/community/guest-sessions", "POST",
                        {"room": "gallery", "display_name": "Imran Sheikh", "rules_version": "v1"}, NET_D)
        D = {"X-Community-Session": dara["token"], **NET_D}

        code, up3 = js("/api/world/community/rooms/gallery/images", "POST",
                       {"client_upload_id": "esc1", "data": data_uri(png(MARK_RESTRICTED)), "alt": "Something"}, D)
        state = settle(up3["media_id"])
        chk("I23 an escalated upload settles as restricted", state == "restricted", f"state={state}")

        code, _ = req("GET", f"/api/world/community/media/{up3['media_id']}", headers=B, raw=True)
        chk("I24 a restricted image is never servable", code == 404, f"got {code}")

        rows = sql("SELECT storage_ref, derivative_ref FROM pciworld_community_media WHERE id=?", (up3["media_id"],))
        chk("I25 a restricted item keeps NO reference an ordinary handler could resolve",
            rows and rows[0][0] is None and rows[0][1] is None, str(rows))

        rows = sql("SELECT legal_hold FROM pciworld_restricted_evidence WHERE media_id=?", (up3["media_id"],))
        chk("I26 escalation preserves the evidence under a legal hold", rows and rows[0][0] in (1, True), str(rows))

        code, msgs = js("/api/world/community/rooms/gallery/messages", headers=B)
        chk("I27 a restricted image never enters the transcript",
            len(msgs.get("messages", [])) == 1, str(msgs))

        # ── The second gate ────────────────────────────────────────────────────────────────
        sql("""INSERT INTO pciworld_community_rooms(slug,title,description,state,capacity,rules_version,image_allowed)
               VALUES('textonly','Text only','No images','open',10,'v1',0)""")
        code, tguest = js("/api/world/community/guest-sessions", "POST",
                          {"room": "textonly", "display_name": "Zara Malik", "rules_version": "v1"},
                          {"X-Forwarded-For": "198.51.100.50"})
        T = {"X-Community-Session": tguest["token"], "X-Forwarded-For": "198.51.100.50"}
        code, r = js("/api/world/community/rooms/textonly/images", "POST",
                     {"client_upload_id": "t1", "data": data_uri(png()), "alt": "A chart"}, T)
        chk("I28 a room that was not granted images refuses them even with the feature on",
            code == 400 and r.get("reason") == "images_not_allowed_here", f"{code} {r}")

        # ── Fail-closed on an unknown provider name ────────────────────────────────────────
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_community_image_scanner','typo-provider')")
        NET_E = {"X-Forwarded-For": "198.51.100.80"}
        code, eman = js("/api/world/community/guest-sessions", "POST",
                        {"room": "gallery", "display_name": "Hina Baig", "rules_version": "v1"}, NET_E)
        E = {"X-Community-Session": eman["token"], **NET_E}
        code, up4 = js("/api/world/community/rooms/gallery/images", "POST",
                       {"client_upload_id": "typo1", "data": data_uri(png(MARK_OK)), "alt": "A chart"}, E)
        chk("I29 the upload is accepted for inspection", code == 200 and up4.get("media_id"), f"{code} {up4}")
        state = settle(up4["media_id"], timeout=20)
        chk("I30 a misspelled provider name fails CLOSED — nothing is published",
            state != "allowed", f"state={state}")
        code, _ = req("GET", f"/api/world/community/media/{up4['media_id']}", headers=B, raw=True)
        chk("I31 and the image is not servable", code == 404, f"got {code}")

        # ── The moderation surface ─────────────────────────────────────────────────────────
        code, login = js("/api/world-admin/auth/login", "POST",
                         {"email": "owner@pciworld.local", "password": OWNER_PW})
        OWNER = {"Authorization": "Bearer " + login["token"]} if login.get("token") else {}
        chk("I32 the world admin signs in", code == 200 and login.get("token"), f"{code} {login}")

        code, r = js("/api/world-admin/community/media")
        chk("I33 the media queue is not readable without a token", code == 401, f"got {code}")

        code, q = js("/api/world-admin/community/media", headers=OWNER)
        ids = [m["id"] for m in q.get("media", [])]
        chk("I34 the ordinary media queue lists reviewable items",
            code == 200 and media_id in ids, str(q)[:300])
        chk("I35 the ordinary queue NEVER contains the restricted item",
            up3["media_id"] not in ids, f"restricted item {up3['media_id']} was listed")
        chk("I36 a withheld item IS in the ordinary queue, so the work is not hidden",
            up2["media_id"] in ids, str(ids))

        code, r = js("/api/world-admin/community/media?state=restricted", headers=OWNER)
        chk("I37 restricted items cannot be reached by widening a query parameter",
            code == 403, f"{code} {r}")

        code, ev = js("/api/world-admin/community/media/restricted", headers=OWNER)
        chk("I38 the restricted record is text only — hash, size, room, decision",
            code == 200 and len(ev.get("evidence", [])) == 1
            and ev["evidence"][0]["media_id"] == up3["media_id"], str(ev)[:300])
        # Checked against the RECORDS, not the whole response — the response's own notice contains
        # the word "preview" while saying there isn't one, and a substring match over that would be
        # a test that fails for the opposite of the reason it claims.
        blob = json.dumps(ev["evidence"])
        chk("I39 and no record exposes a thumbnail, preview or link to bytes",
            not any(k in blob for k in ("thumbnail", "preview", "derivative", "storage_ref")), blob[:300])
        chk("I40 the count is visible even though the content is not",
            ev["evidence"][0]["legal_hold"] is True, str(ev["evidence"][0]))
        chk("I41 the identifying hash IS recorded — it is what a specialist service is given",
            bool(ev["evidence"][0]["sha256"]), str(ev["evidence"][0]))

        # ── Two-person access ──────────────────────────────────────────────────────────────
        eid = ev["evidence"][0]["id"]
        code, r = js(f"/api/world-admin/community/media/restricted/{eid}/request-access", "POST",
                     {"reason": "short"}, OWNER)
        chk("I42 an access request without a stated reason is refused",
            code == 400 and r.get("error") == "reason_required", f"{code} {r}")

        code, r = js(f"/api/world-admin/community/media/restricted/{eid}/approve-access", "POST", {}, OWNER)
        chk("I43 there is nothing to approve before a request exists",
            code == 400 and r.get("error") == "no_request_to_approve", f"{code} {r}")

        code, r = js(f"/api/world-admin/community/media/restricted/{eid}/request-access", "POST",
                     {"reason": "Referral to the specialist reporting route under the escalation procedure."}, OWNER)
        chk("I44 an access request with a reason is recorded", code == 200 and r.get("ok"), f"{code} {r}")

        code, r = js(f"/api/world-admin/community/media/restricted/{eid}/approve-access", "POST", {}, OWNER)
        chk("I45 the requester cannot approve their own access request",
            code == 403 and r.get("error") == "approver_must_differ_from_requester", f"{code} {r}")

        rows = sql("SELECT approved_by, accessed_count FROM pciworld_restricted_evidence WHERE id=?", (eid,))
        chk("I46 and the refused self-approval granted nothing",
            rows and rows[0][0] is None and rows[0][1] == 0, str(rows))

        rows = sql("SELECT COUNT(*) FROM pciworld_audit WHERE action=?",
                   ("world_community_restricted_self_approval_refused",))
        chk("I47 the refused self-approval is audited, not silently dropped",
            rows and rows[0][0] == 1, str(rows))

        rows = sql("SELECT COUNT(*) FROM pciworld_audit WHERE action=?",
                   ("world_community_restricted_media_listed",))
        chk("I48 merely listing restricted evidence is audited", rows and rows[0][0] >= 1, str(rows))

    finally:
        shutdown()

    print(f"\n  {passed} passing, {failed} failing")
    sys.exit(1 if failed else 0)

if __name__ == "__main__":
    main()
