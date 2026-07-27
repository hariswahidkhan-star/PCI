#!/usr/bin/env python3
"""
PCI World community rooms — live HTTP suite for the guest journey.

Boots the REAL built backend against a throwaway SQLite database and drives the participant
API over real HTTP:

  • the whole surface 404s while the feature flag is off (it ships dark)
  • guest entry: name validation, stale rules version, capacity, duplicate names
  • send → moderate → publish, and ordered replay from a sequence
  • THE INVARIANT: a blocked message is invisible to a second participant
  • ejection ends the session in the same request
  • reporting is idempotent and leaks no moderation state
  • leaving releases the display name

Exit code 0 iff every assertion passes.  Run from backend/:  python3 tests/community_test.py
"""
import json, os, re, socket, sqlite3, subprocess, sys, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_community.db")
STORAGE = os.path.join(HERE, "_community_storage")
OWNER_PW = "CommunitySuiteOwner!99"

# Provider-aware, exactly like integration_test.py. This matters: the suite reaches into the
# database to enable the feature flag and seed a room, so running it against SQLite while CI
# believes it ran against MySQL would be a false green on the parity gate.
PROVIDER = (os.environ.get("TEST_DB_PROVIDER") or "sqlite").strip().lower()
if PROVIDER in ("mariadb", "mysql"): PROVIDER = "mysql"
elif PROVIDER != "sqlite": raise SystemExit(f"unknown TEST_DB_PROVIDER={PROVIDER!r}")
if os.environ.get("REQUIRE_MYSQL_TEST") == "1" and PROVIDER != "mysql":
    raise SystemExit("REQUIRE_MYSQL_TEST=1 but TEST_DB_PROVIDER is not mysql")

_base = os.environ.get("MYSQL_DATABASE", "pci")
if not re.fullmatch(r"[A-Za-z0-9_]+", _base): raise SystemExit(f"unsafe MYSQL_DATABASE={_base!r}")
MYSQL = dict(host=os.environ.get("MYSQL_HOST", "127.0.0.1"), port=int(os.environ.get("MYSQL_PORT", "3306")),
             user=os.environ.get("MYSQL_USER", "pci"), password=os.environ.get("MYSQL_PASSWORD", "pcipass"),
             database=_base if _base.endswith("_community") else _base + "_community")

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port()
BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, body=None, headers=None):
    hdr = dict(headers or {})
    data = None
    if body is not None:
        data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp:
            return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e:
        return e.code, e.read().decode()
    except urllib.error.URLError as e:
        return 0, str(e)

def js(path, method="GET", body=None, headers=None):
    code, text = req(method, path, body, headers)
    try: return code, json.loads(text) if text else {}
    except Exception: return code, {"_raw": text}

def sql(statement, args=()):
    """Run one statement against whichever provider is under test. The few statements this suite
    issues are deliberately plain — INSERT/UPDATE/SELECT with ? placeholders and no date maths — so
    the only translation needed is the placeholder form.

    Keep them free of % as well: pymysql renders via `query % args`, so a literal percent (a LIKE
    pattern, a DATE_FORMAT specifier) must be doubled or the statement dies with 'not enough
    arguments for format string' on MySQL while passing cleanly on SQLite."""
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
    for _ in range(120):
        code, _ = req("GET", "/api/health")
        if code == 200: return
        time.sleep(0.5)
    raise SystemExit("backend did not become healthy")

def shutdown():
    if proc:
        proc.terminate()
        try: proc.wait(timeout=15)
        except Exception: proc.kill()

def main():
    boot()
    try:
        # ── The feature ships dark ─────────────────────────────────────────────────────────
        code, _ = js("/api/world/community/rooms")
        chk("C01 the whole surface 404s while the flag is off", code == 404, f"got {code}")

        # Enable it, and configure the deterministic moderator so the pipeline is exercisable
        # without a vendor. Both are settings, which is the point of the adapter seam.
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_enabled','1')")
        sql("INSERT OR REPLACE INTO site_settings(skey,svalue) VALUES('world_community_moderator','deterministic')")
        sql("""INSERT INTO pciworld_community_rooms(slug,title,description,state,capacity,rules_version)
               VALUES('lobby','Lobby','General discussion','open',3,'v1')""")

        code, rooms = js("/api/world/community/rooms")
        chk("C02 the room catalogue is public", code == 200 and len(rooms.get("rooms", [])) == 1, str(rooms))
        chk("C03 the catalogue reports presence", rooms["rooms"][0]["participants"] == 0)

        code, room = js("/api/world/community/rooms/lobby")
        chk("C04 room detail is public and does not advertise images",
            code == 200 and room["accepting"] is True and room["images_allowed"] is False, str(room))

        # ── Guest entry ────────────────────────────────────────────────────────────────────
        code, r = js("/api/world/community/guest-sessions", "POST",
                     {"room": "lobby", "display_name": "admin", "rules_version": "v1"})
        chk("C05 a reserved display name is refused", code == 400 and r.get("error") == "name_reserved", str(r))

        code, r = js("/api/world/community/guest-sessions", "POST",
                     {"room": "lobby", "display_name": "Ali Hassan", "rules_version": "v0"})
        chk("C06 a stale rules version is refused", code == 409 and r.get("error") == "rules_version_stale", str(r))

        NET_A = {"X-Forwarded-For": "198.51.100.10"}
        NET_B = {"X-Forwarded-For": "198.51.100.20"}
        code, alice = js("/api/world/community/guest-sessions", "POST",
                         {"room": "lobby", "display_name": "Ali Hassan", "rules_version": "v1"}, NET_A)
        chk("C07 a valid guest joins with no account", code == 200 and alice.get("token"), str(alice))
        A = {"X-Community-Session": alice["token"], **NET_A}

        code, r = js("/api/world/community/guest-sessions", "POST",
                     {"room": "lobby", "display_name": "Ali Hassan", "rules_version": "v1"}, NET_A)
        chk("C08 a duplicate active name is refused with a suggestion",
            code == 400 and r.get("error") == "name_taken" and r.get("suggestion"), str(r))

        code, bob = js("/api/world/community/guest-sessions", "POST",
                       {"room": "lobby", "display_name": "Sara Khan", "rules_version": "v1"}, NET_B)
        chk("C09 a second guest joins", code == 200 and bob.get("token"))
        B = {"X-Community-Session": bob["token"], **NET_B}

        # ── Send and replay ────────────────────────────────────────────────────────────────
        code, m = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "Has anyone modelled SPI recovery late in a project?",
                      "client_message_id": "a1"}, A)
        chk("C10 an ordinary message publishes", code == 200 and m["published"] and m["sequence"] == 1, str(m))

        code, m2 = js("/api/world/community/rooms/lobby/messages", "POST",
                      {"body": "Has anyone modelled SPI recovery late in a project?",
                       "client_message_id": "a1"}, A)
        chk("C11 a retried send is idempotent",
            code == 200 and m2["sequence"] == 1 and m2["reason"] == "duplicate", str(m2))

        code, hist = js("/api/world/community/rooms/lobby/messages")
        chk("C12 replay is public and ordered", code == 200 and len(hist["messages"]) == 1, str(hist))
        chk("C13 replay carries the author's chosen name", hist["messages"][0]["author"] == "Ali Hassan")

        # ── THE INVARIANT: a blocked message reaches nobody ────────────────────────────────
        code, bad = js("/api/world/community/rooms/lobby/messages", "POST",
                       {"body": "you are an idiot", "client_message_id": "b1"}, B)
        chk("C14 an abusive message is not published", code == 200 and bad["published"] is False, str(bad))
        chk("C15 its author is told why", bad.get("reason") == "abuse_blocked", str(bad))

        code, hist = js("/api/world/community/rooms/lobby/messages")
        bodies = [x["body"] for x in hist["messages"]]
        chk("C16 no other participant can see it", "you are an idiot" not in bodies, str(bodies))
        chk("C17 it consumed no sequence", len(hist["messages"]) == 1)

        # ── Ejection ends the session in the same request ──────────────────────────────────
        code, ej = js("/api/world/community/rooms/lobby/messages", "POST",
                      {"body": "f u c k this", "client_message_id": "b2"}, B)
        chk("C18 a severe violation ejects", code == 200 and ej.get("ejected") is True, str(ej))
        chk("C18a the ejection hands over an appeal ticket there and then",
            ej.get("appeal", {}).get("reference") and ej.get("appeal", {}).get("credential"), str(ej.get("appeal")))
        EJECT_APPEAL = ej.get("appeal") or {}

        code, after = js("/api/world/community/rooms/lobby/messages", "POST",
                         {"body": "still here?", "client_message_id": "b3"}, B)
        chk("C19 the ejected session is dead immediately", after.get("error") == "no_session", str(after))

        code, hist = js("/api/world/community/rooms/lobby/messages")
        chk("C20 nothing the ejected guest sent was ever published", len(hist["messages"]) == 1)

        # ── Reporting ──────────────────────────────────────────────────────────────────────
        code, rep = js("/api/world/community/reports", "POST",
                       {"sequence": 1, "reason": "spam"}, A)
        chk("C21 a participant can report a message", code == 200 and rep.get("ok") is True, str(rep))
        chk("C22 the response leaks no moderation state", "status" not in rep and "action" not in rep, str(rep))

        code, rep2 = js("/api/world/community/reports", "POST",
                        {"sequence": 1, "reason": "spam"}, A)
        chk("C23 a repeat report is a harmless no-op", code == 200, str(rep2))
        n = sql("SELECT COUNT(*) FROM pciworld_community_reports")[0][0]
        chk("C24 one reporter cannot inflate the count", n == 1, f"rows={n}")

        # ── Session lifecycle ──────────────────────────────────────────────────────────────
        code, _ = js("/api/world/community/guest-sessions/current", "DELETE", None, A)
        chk("C25 a guest can leave", code == 200)

        code, again = js("/api/world/community/guest-sessions", "POST",
                         {"room": "lobby", "display_name": "Ali Hassan", "rules_version": "v1"}, NET_A)
        chk("C26 leaving releases the display name", code == 200 and again.get("token"), str(again))

        # ── Anonymous access boundaries ────────────────────────────────────────────────────
        code, r = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "hello", "client_message_id": "x1"})
        chk("C27 sending requires a session", code == 401 and r.get("error") == "no_session", str(r))

        code, r = js("/api/world/community/reports", "POST", {"sequence": 1, "reason": "spam"})
        chk("C28 reporting requires a session", code == 401, str(r))

        # ── The fail-closed default ────────────────────────────────────────────────────────
        sql("UPDATE site_settings SET svalue='none' WHERE skey='world_community_moderator'")
        code, m = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "a perfectly ordinary sentence", "client_message_id": "z1"},
                     {"X-Community-Session": again["token"], **NET_A})
        chk("C29 with no moderation provider nothing publishes",
            code == 200 and m["published"] is False and m["reason"] == "moderation_unavailable", str(m))

        code, hist = js("/api/world/community/rooms/lobby/messages")
        chk("C30 and the room gains no new visible message", len(hist["messages"]) == 1)

        # ── The realtime hub ───────────────────────────────────────────────────────────────
        # SignalR's negotiate handshake is plain HTTP, so the transport can be checked without a
        # WebSocket client. What matters here is the SHAPE of the guarantees, not the framing:
        # the hub must exist, must refuse an unauthenticated handshake, and must not become a
        # second way into a room.
        sql("UPDATE site_settings SET svalue='deterministic' WHERE skey='world_community_moderator'")

        code, _ = js("/api/world/community/hubs/community/negotiate?negotiateVersion=1", "POST")
        chk("C31 the hub is not mounted on a guessable sibling path", code == 404, f"got {code}")

        # NOTE ON WHAT THIS DOES AND DOES NOT PROVE. SignalR's negotiate handshake is unauthenticated
        # by design — authorization happens in OnConnectedAsync, which aborts a connection with no
        # valid guest session. This call carries no session and still succeeds, which is CORRECT
        # behaviour, so the assertion is only that the transport is mounted and reachable. The
        # connect-time abort needs a real WebSocket client to exercise and is NOT covered here; it
        # is recorded as a gap rather than implied by a passing negotiate.
        code, neg = js("/api/world/hubs/community/negotiate?negotiateVersion=1", "POST")
        chk("C32 the hub transport is mounted and reachable (negotiate is unauthenticated by design)",
            code == 200 and ("connectionToken" in neg or "connectionId" in neg), f"{code} {neg}")

        # A connection is only useful with a session, and the session is what scopes it to a room.
        # The hub derives the room from the session rather than from anything the client sends, so
        # there is no room parameter to tamper with — which is the point.
        code, hist_before = js("/api/world/community/rooms/lobby/messages")
        code, _ = js("/api/world/community/rooms/lobby/messages", "POST",
                     {"body": "sent over http, broadcast over the hub", "client_message_id": "h1"},
                     {"X-Community-Session": again["token"], **NET_A})
        code, hist_after = js("/api/world/community/rooms/lobby/messages")
        chk("C33 messages still arrive through the durable path, not the hub",
            len(hist_after["messages"]) == len(hist_before["messages"]) + 1,
            f"{len(hist_before['messages'])} -> {len(hist_after['messages'])}")

        # The outbox is what the hub drains. Draining is asynchronous, so the assertion is that the
        # event was ENQUEUED transactionally with the message — the delivery itself is allowed to be
        # late, duplicated or lost without affecting correctness.
        queued = sql("SELECT COUNT(*) FROM pciworld_community_outbox WHERE event_type='CommunityMessageAllowed'")[0][0]
        chk("C34 a published message enqueues exactly one broadcast event", queued >= 1, f"rows={queued}")

        # Stated as an equality rather than a LIKE: exactly as many broadcast events exist as there
        # are published messages, so nothing unpublished produced one and nothing published was
        # missed. (A LIKE would also have needed its % escaped for pymysql — see sql()'s note.)
        events = sql("SELECT COUNT(*) FROM pciworld_community_outbox WHERE event_type='CommunityMessageAllowed'")[0][0]
        allowed = sql("SELECT COUNT(*) FROM pciworld_community_messages WHERE status='allowed'")[0][0]
        chk("C35 broadcast events exactly match published messages, no more and no fewer",
            events == allowed, f"events={events} allowed={allowed}")

        # ── Moderation console ─────────────────────────────────────────────────────────────
        code, login = js("/api/world-admin/auth/login", "POST",
                         {"email": "owner@pciworld.local", "password": OWNER_PW})
        chk("C36 the world-admin realm authenticates", code == 200 and login.get("token"), str(login)[:200])
        OWNER = {"Authorization": "Bearer " + login["token"]} if login.get("token") else {}

        code, q = js("/api/world-admin/community/queue", "GET", None, OWNER)
        chk("C37 the moderation queue is reachable by an authorised admin", code == 200, str(q)[:160])

        code, anon_q = js("/api/world-admin/community/queue")
        chk("C38 the queue rejects an unauthenticated caller", anon_q.get("error") == "no_token", str(anon_q))

        # Seed a case + a restricted case directly, then prove the queue separation over HTTP.
        sql("""INSERT INTO pciworld_moderation_cases(room_id,subject_kind,subject_ref,severity,status,restricted,reason_code)
               VALUES(1,'guest_session','ordinary','normal','open',0,'abuse_blocked')""")
        sql("""INSERT INTO pciworld_moderation_cases(room_id,subject_kind,subject_ref,severity,status,restricted,reason_code)
               VALUES(1,'guest_session','restricted','critical','open',1,'grooming_severe')""")

        code, q = js("/api/world-admin/community/queue", "GET", None, OWNER)
        refs = [c["subject_kind"] for c in q.get("cases", [])]
        restricted_visible = any(c.get("restricted") for c in q.get("cases", []))
        chk("C39 restricted cases never appear in the ordinary queue", not restricted_visible, str(q)[:200])

        code, rq = js("/api/world-admin/community/queue/restricted", "GET", None, OWNER)
        chk("C40 restricted cases are reachable only on their own route", code == 200 and len(rq.get("cases", [])) == 1, str(rq)[:200])

        # A case summary is for triage — it must not carry message bodies or risk keys.
        blob = json.dumps(q)
        chk("C41 the queue leaks no evidence", "risk_key" not in blob and "body" not in blob, blob[:200])

        # ── Maker-checker over HTTP ────────────────────────────────────────────────────────
        code, s1 = js("/api/world-admin/community/sanctions", "POST",
                      {"subject_kind": "risk_key", "subject_ref": "rk-http", "type": "permanent",
                       "reason": "hate_severe", "scope": "world"}, OWNER)
        chk("C42 a permanent sanction is created awaiting approval",
            code == 200 and s1.get("awaiting_approval") is True, str(s1))

        code, self_ok = js(f"/api/world-admin/community/sanctions/{s1.get('id')}/approve", "POST", {}, OWNER)
        chk("C43 the issuer cannot approve their own permanent sanction",
            code == 403 and self_ok.get("error") == "maker_checker", f"{code} {self_ok}")

        in_effect = sql("SELECT COUNT(*) FROM pciworld_sanctions WHERE id=%s AND approved_by IS NOT NULL"
                        .replace("%s", "?"), (s1.get("id"),))[0][0]
        chk("C44 and it still has no effect", in_effect == 0, f"approved rows={in_effect}")

        code, noreason = js("/api/world-admin/community/sanctions", "POST",
                            {"subject_kind": "risk_key", "subject_ref": "rk-2", "type": "mute", "reason": ""}, OWNER)
        chk("C45 every sanction must carry a reason", code == 400 and noreason.get("error") == "reason_required", str(noreason))

        # ── Rooms are created dark ─────────────────────────────────────────────────────────
        code, room_new = js("/api/world-admin/community/rooms", "POST",
                            {"slug": "new-room", "title": "A New Room"}, OWNER)
        chk("C46 a new room is created as draft, never live", code == 200 and room_new.get("state") == "draft", str(room_new))

        code, listed = js("/api/world/community/rooms")
        slugs = [r["slug"] for r in listed.get("rooms", [])]
        chk("C47 a draft room is not publicly listed", "new-room" not in slugs, str(slugs))

        # ── The kill switch beats an open room ─────────────────────────────────────────────
        code, _ = js("/api/world-admin/community/rooms/lobby/emergency", "POST", {"mode": "locked"}, OWNER)
        code, blocked = js("/api/world/community/rooms/lobby/messages", "POST",
                           {"body": "should not land", "client_message_id": "kill1"},
                           {"X-Community-Session": again["token"], **NET_A})
        chk("C48 an emergency lock stops an open room accepting",
            blocked.get("reason") == "room_not_accepting", str(blocked))

        code, _ = js("/api/world-admin/community/rooms/lobby/emergency", "POST", {"mode": ""}, OWNER)
        code, room_after = js("/api/world/community/rooms/lobby")
        chk("C49 clearing the emergency restores the room's own state",
            room_after.get("state") == "open" and room_after.get("accepting") is True, str(room_after))

        # ── Guest appeals need the credential, not the reference ───────────────────────────
        case_id = sql("SELECT id FROM pciworld_moderation_cases WHERE subject_ref='ordinary'")[0][0]
        ref = "PCIW-TESTREF"
        # credential 'letmein' hashed the same way the service stores it
        import hashlib
        cred = "letmein-appeal-credential"
        cred_sha = hashlib.sha256(cred.encode()).hexdigest()
        sql("""INSERT INTO pciworld_appeals(case_id,public_reference,credential_sha,status)
               VALUES(?,?,?,'issued')""", (case_id, ref, cred_sha))

        code, bad = js("/api/world/community/appeals", "POST",
                       {"reference": ref, "credential": "wrong", "submission": "please"})
        chk("C50 the reference alone cannot submit an appeal", code == 404, f"{code} {bad}")

        code, good = js("/api/world/community/appeals", "POST",
                        {"reference": ref, "credential": cred, "submission": "I was quoting a policy document."})
        chk("C51 the credential submits the appeal without any account", code == 200 and good.get("ok"), str(good))

        code, st = js("/api/world/community/appeals/status", "POST", {"reference": ref, "credential": cred})
        chk("C52 a guest can retrieve a minimal status", code == 200 and st.get("status") == "submitted", str(st))
        chk("C53 the status carries no evidence",
            set(st.keys()) <= {"reference", "status", "outcome", "submitted", "decided_at"}, str(st.keys()))

        # ── The ejected guest completes an appeal with no account ──────────────────────────
        # PW-US-053/054: the whole right of appeal for a guest rests on the ticket handed over at
        # ejection, because there is no account to come back to.
        code, appealed = js("/api/world/community/appeals", "POST",
                            {"reference": EJECT_APPEAL.get("reference"),
                             "credential": EJECT_APPEAL.get("credential"),
                             "submission": "I was quoting a policy document, not directing it at anyone."})
        chk("C54 an ejected guest can appeal using only the ticket they were given",
            code == 200 and appealed.get("ok") is True, str(appealed))

        code, st = js("/api/world/community/appeals/status", "POST",
                      {"reference": EJECT_APPEAL.get("reference"), "credential": EJECT_APPEAL.get("credential")})
        chk("C55 and can retrieve their own status", code == 200 and st.get("status") == "submitted", str(st))

        # The ejection must actually have produced the case and the restriction, not just a token.
        ej_cases = sql("SELECT COUNT(*) FROM pciworld_moderation_cases WHERE reason_code='profanity_severe'")[0][0]
        chk("C56 the ejection opened a moderation case", ej_cases >= 1, f"cases={ej_cases}")

        ej_restr = sql("SELECT COUNT(*) FROM pciworld_risk_restrictions")[0][0]
        chk("C57 and a time-bounded restriction", ej_restr >= 1, f"restrictions={ej_restr}")
        chk("C58 the guest is told when the restriction lifts", bool(EJECT_APPEAL.get("restricted_until")),
            str(EJECT_APPEAL))

        # ── An overturned appeal restores access ───────────────────────────────────────────
        appeal_id = sql("SELECT id FROM pciworld_appeals WHERE public_reference=?",
                        (EJECT_APPEAL.get("reference"),))[0][0]
        code, decided = js(f"/api/world-admin/community/appeals/{appeal_id}/decide", "POST",
                           {"decision": "overturn", "outcome": "Context accepted; restriction lifted."}, OWNER)
        chk("C59 an authorised reviewer can overturn", code == 200 and decided.get("ok"), str(decided))

        live = sql("SELECT COUNT(*) FROM pciworld_risk_restrictions WHERE expires_at > ?",
                   (time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime()),))[0][0]
        chk("C60 overturning lifts the restriction rather than leaving it to expire", live == 0, f"still live={live}")

        code, st2 = js("/api/world/community/appeals/status", "POST",
                       {"reference": EJECT_APPEAL.get("reference"), "credential": EJECT_APPEAL.get("credential")})
        chk("C61 the guest sees the outcome without seeing any evidence",
            st2.get("status") == "overturned" and set(st2.keys()) <= {"reference","status","outcome","submitted","decided_at"},
            str(st2))

        # ── A report reaches the queue ─────────────────────────────────────────────────────
        rep_cases = sql("SELECT COUNT(*) FROM pciworld_moderation_cases WHERE reason_code='participant_report'")[0][0]
        chk("C62 a participant report opens a case a moderator can actually see", rep_cases >= 1, f"cases={rep_cases}")

        linked = sql("SELECT COUNT(*) FROM pciworld_community_reports WHERE case_id IS NOT NULL")[0][0]
        chk("C63 and the report is attached to it as evidence", linked >= 1, f"linked={linked}")

    finally:
        shutdown()

    print()
    print(f"  [{PROVIDER}] ══ {passed}/{passed + failed} PASSED ══" if not failed else f"  ══ {failed} FAILED, {passed} passed ══")
    sys.exit(1 if failed else 0)

if __name__ == "__main__":
    main()
