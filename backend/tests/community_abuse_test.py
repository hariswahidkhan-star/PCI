#!/usr/bin/env python3
"""
PCI World community rooms — abuse and failure drills (spec §24.2: E2E-019, E2E-026, E2E-028).

Separate from community_test.py, which proves the journeys WORK. This one attacks them, and
asserts the system fails in the direction that keeps people safe rather than the direction that
keeps the room lively.

  • flood, oversized payload, malformed body, capacity, slow mode
  • forged and replayed session tokens, cross-room use
  • worker crash after commit, duplicate delivery, out-of-order events
  • moderation provider outage mid-conversation
  • ordered recovery after every one of the above

Exit code 0 iff every assertion passes.  Run from backend/:  python3 tests/community_abuse_test.py
"""
import json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_community_abuse.db")
STORAGE = os.path.join(HERE, "_community_abuse_storage")
OWNER_PW = "AbuseSuiteOwner!99"

PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")
_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_abuse") else _base + "_abuse")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, body=None, headers=None, raw=None):
    hdr = dict(headers or {}); data = None
    if raw is not None:
        data = raw; hdr.setdefault("Content-Type", "application/json")
    elif body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp: return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e: return e.code, e.read().decode()
    except urllib.error.URLError as e: return 0, str(e)

def js(path, method="GET", body=None, headers=None, raw=None):
    code, text = req(method, path, body, headers, raw)
    try: return code, json.loads(text) if text else {}
    except Exception: return code, {"_raw": text[:200]}

def sql(statement, args=()):
    if PROVIDER == "mysql":
        import pymysql
        con = pymysql.connect(**MYSQL); cur = con.cursor()
        cur.execute(statement.replace("INSERT OR REPLACE INTO", "REPLACE INTO").replace("?", "%s"), args)
        con.commit(); rows = cur.fetchall(); con.close(); return rows
    con = sqlite3.connect(DB); cur = con.cursor()
    cur.execute(statement, args); con.commit()
    rows = cur.fetchall(); con.close(); return rows

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
                   PCIWORLD_OWNER_PASSWORD=OWNER_PW, SEED_DEMO_EXAM="",
                   CANONICAL_HOST="", CANONICAL_REDIRECT="")
        env.pop("MYSQL_CONNECTION_STRING", None)
    else:
        for p in (DB, DB + "-wal", DB + "-shm"):
            if os.path.exists(p): os.remove(p)
        env = dict(os.environ, PORT=str(PORT), DATABASE_FILE=DB, STORAGE_ROOT=STORAGE,
                   ASPNETCORE_ENVIRONMENT="Development", DB_PROVIDER="sqlite",
                   PCIWORLD_OWNER_PASSWORD=OWNER_PW, SEED_DEMO_EXAM="",
                   CANONICAL_HOST="", CANONICAL_REDIRECT="")
    proc = subprocess.Popen([os.environ.get("DOTNET", "dotnet"), DLL], cwd=BACKEND, env=env,
                            stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    for _ in range(140):
        if req("GET", "/api/health")[0] == 200: return
        time.sleep(0.5)
    raise SystemExit("backend did not become healthy")

def shutdown():
    if proc:
        proc.terminate()
        try: proc.wait(timeout=15)
        except Exception: proc.kill()

def join(name, net, room="lobby"):
    code, r = js("/api/world/community/guest-sessions", "POST",
                 {"room": room, "display_name": name, "rules_version": "v1", "birth_date": "1990-05-04", "jurisdiction": "GB"}, {"X-Forwarded-For": net})
    return r.get("token"), {"X-Community-Session": r.get("token", ""), "X-Forwarded-For": net}

def main():
    boot()
    try:
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_enabled','1')")
        # CCP-P1-003: the eligibility gate admits nobody until a jurisdiction list exists, which is
        # the fail-closed posture the register calls for. A suite must configure it the way an
        # operator would rather than have the gate relaxed to let tests through.
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_community_jurisdictions','GB,AE,PK')")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('pciworld_community_min_age','18')")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_moderator','deterministic')")
        sql("""INSERT INTO pciworld_community_rooms(slug,title,state,capacity,rules_version,slow_mode_seconds)
               VALUES('lobby','Lobby','open',3,'v1',0)""")
        sql("""INSERT INTO pciworld_community_rooms(slug,title,state,capacity,rules_version,slow_mode_seconds)
               VALUES('slowroom','Slow','open',50,'v1',60)""")

        tok, A = join("Ali Hassan", "203.0.113.1")
        chk("A01 a legitimate guest can join", bool(tok))

        # ── Forged and misused session tokens (E2E-012) ────────────────────────────────────
        code, r = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "hi", "client_message_id": "f1"},
                     {"X-Community-Session": "0" * 64, "X-Forwarded-For": "203.0.113.9"})
        chk("A02 a forged session token is refused", code == 401 and r.get("error") == "no_session", str(r))

        code, r = js("/api/world/community/rooms/slowroom/messages", "POST",
                     {"body": "hi", "client_message_id": "f2"}, A)
        chk("A03 a session cannot be used in another room", code == 403 and r.get("error") == "wrong_room", str(r))

        # ── Payload bounds ─────────────────────────────────────────────────────────────────
        code, r = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "x" * 5000, "client_message_id": "big"}, A)
        chk("A04 an oversized message is refused", r.get("reason") == "message_too_long", str(r))

        code, r = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "   ", "client_message_id": "empty"}, A)
        chk("A05 an empty message is refused", r.get("reason") == "empty_message", str(r))

        code, r = js("/api/world/community/rooms/lobby/messages", "POST", None, A, raw=b"{not json")
        chk("A06 a malformed body does not 500", code in (400, 401), f"got {code}")

        code, r = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "hi", "client_message_id": "y" * 200}, A)
        chk("A07 an oversized client id is refused", code == 400 and r.get("error") == "bad_client_message_id", str(r))

        # ── Capacity ───────────────────────────────────────────────────────────────────────
        join("Sara Khan", "203.0.113.2")
        join("Omar Farouk", "203.0.113.3")
        code, r = js("/api/world/community/guest-sessions", "POST",
                     {"room": "lobby", "display_name": "One Too Many", "rules_version": "v1", "birth_date": "1990-05-04", "jurisdiction": "GB"},
                     {"X-Forwarded-For": "203.0.113.4"})
        chk("A08 a full room refuses the next guest", code == 409 and r.get("error") == "room_full", str(r))

        # ── Slow mode is enforced server-side ──────────────────────────────────────────────
        stok, S = join("Slow Poster", "203.0.113.20", room="slowroom")
        code, first = js("/api/world/community/rooms/slowroom/messages", "POST",
                         {"body": "first message", "client_message_id": "s1"}, S)
        chk("A09 the first message in a slow room publishes", first.get("published") is True, str(first))
        code, second = js("/api/world/community/rooms/slowroom/messages", "POST",
                          {"body": "immediately after", "client_message_id": "s2"}, S)
        chk("A10 slow mode blocks the next message regardless of the client",
            code == 429 and second.get("error") == "slow_mode", str(second))

        # ── Flood control ──────────────────────────────────────────────────────────────────
        blocked = 0
        for i in range(45):
            code, _ = js("/api/world/community/rooms/lobby/messages", "POST",
                         {"body": f"flood {i}", "client_message_id": f"fl{i}"}, A)
            if code == 429: blocked += 1
        chk("A11 a flood is rate-limited rather than accepted wholesale", blocked > 0, f"blocked={blocked}/45")

        # ── Ordering survives everything above ─────────────────────────────────────────────
        code, hist = js("/api/world/community/rooms/lobby/messages")
        seqs = [m["sequence"] for m in hist.get("messages", [])]
        chk("A12 every published message has a distinct sequence", len(seqs) == len(set(seqs)), str(seqs[:20]))
        chk("A13 sequences are contiguous from 1 with no gaps",
            seqs == list(range(1, len(seqs) + 1)), f"{seqs[:10]}...")
        chk("A14 nothing refused above consumed a sequence",
            all(m["body"] not in ("", "   ") and len(m["body"]) <= 2000 for m in hist.get("messages", [])))

        # ── Duplicate delivery and out-of-order events (E2E-028) ───────────────────────────
        # Re-queue every delivered broadcast: the transport is allowed to deliver twice, and the
        # durable history must be unmoved by it.
        before = len(hist.get("messages", []))
        sql("UPDATE pciworld_community_outbox SET status='queued', next_attempt_at=NULL, attempts=0")
        time.sleep(3)
        code, hist2 = js("/api/world/community/rooms/lobby/messages")
        chk("A15 re-delivering every broadcast changes no durable history",
            len(hist2.get("messages", [])) == before, f"{before} -> {len(hist2.get('messages', []))}")

        # A crashed worker leaves rows claimed mid-flight; lease expiry must recover them rather
        # than stranding the queue forever.
        sql("INSERT INTO pciworld_community_outbox(event_type,room_id,payload_json,status,lease_owner,lease_until) "
            "VALUES('CommunityMessageAllowed',1,'{}','processing','dead-worker','2000-01-01 00:00:00')")
        time.sleep(3)
        stranded = sql("SELECT COUNT(*) FROM pciworld_community_outbox WHERE status='processing' AND lease_owner='dead-worker'")[0][0]
        chk("A16 a crashed worker's claim is recovered, not stranded", stranded == 0, f"stranded={stranded}")

        # ── Moderation outage mid-conversation (E2E-026) ───────────────────────────────────
        # A fresh session, because the flood above deliberately exhausted A's per-session budget and
        # the limiter is per session by design (a shared office address must not throttle everyone).
        # Reusing A here would test the rate limiter a second time rather than the outage behaviour.
        sql("UPDATE pciworld_community_rooms SET capacity=50 WHERE slug='lobby'")
        rtok, R = join("Recovery Tester", "203.0.113.60")
        chk("A16b a fresh guest can still join after the flood", bool(rtok), "no token")

        sql("UPDATE site_settings SET svalue='none' WHERE skey='world_community_moderator'")
        code, during = js("/api/world/community/rooms/lobby/messages", "POST",
                          {"body": "a perfectly ordinary sentence", "client_message_id": "out1"}, R)
        chk("A17 a provider outage stops publication rather than passing text through",
            during.get("published") is False and during.get("reason") == "moderation_unavailable", str(during))

        code, hist3 = js("/api/world/community/rooms/lobby/messages")
        chk("A18 and the room gains nothing during the outage",
            len(hist3.get("messages", [])) == before, f"{before} -> {len(hist3.get('messages', []))}")

        # Recovery: the room works again the moment a provider returns, with no gap in sequence.
        sql("UPDATE site_settings SET svalue='deterministic' WHERE skey='world_community_moderator'")
        code, after = js("/api/world/community/rooms/lobby/messages", "POST",
                         {"body": "back online", "client_message_id": "rec1"}, R)
        chk("A19 the room recovers when the provider returns", after.get("published") is True, str(after))
        code, hist4 = js("/api/world/community/rooms/lobby/messages")
        seqs4 = [m["sequence"] for m in hist4.get("messages", [])]
        chk("A20 the outage left no hole in the sequence",
            seqs4 == list(range(1, len(seqs4) + 1)), f"{seqs4[-5:]}")

        # ── Reconnect replay ───────────────────────────────────────────────────────────────
        code, tail = js(f"/api/world/community/rooms/lobby/messages?afterSequence={len(seqs4) - 2}")
        chk("A21 a reconnect replays only what was missed",
            [m["sequence"] for m in tail.get("messages", [])] == seqs4[-2:], str(tail)[:160])

        code, none_left = js(f"/api/world/community/rooms/lobby/messages?afterSequence={len(seqs4)}")
        chk("A22 a caught-up client replays nothing", none_left.get("messages") == [], str(none_left)[:120])

        # ── Report abuse ───────────────────────────────────────────────────────────────────
        rep_blocked = 0
        for i in range(15):
            code, _ = js("/api/world/community/reports", "POST", {"sequence": 1, "reason": "spam"}, A)
            if code == 429: rep_blocked += 1
        chk("A23 report spam is rate-limited", rep_blocked > 0, f"blocked={rep_blocked}/15")
        n = sql("SELECT COUNT(*) FROM pciworld_community_reports WHERE message_id IS NOT NULL")[0][0]
        chk("A24 and one reporter still counts once per message", n <= 1, f"rows={n}")

    finally:
        shutdown()

    print()
    print(f"  [{PROVIDER}] ══ {passed}/{passed + failed} PASSED ══" if not failed
          else f"  [{PROVIDER}] ══ {failed} FAILED, {passed} passed ══")
    sys.exit(1 if failed else 0)

if __name__ == "__main__":
    main()
