#!/usr/bin/env python3
"""
P0-2 regression suite — Stripe fulfillment is correct and replay-safe.

Boots the REAL built backend with TEST-mode Stripe keys and drives the webhook over real HTTP,
signing events exactly as Stripe does (SDK pins api_version 2024-06-20), so no Stripe CLI is needed.

Proves:
  • checkout.session.completed with payment_status != paid is NOT fulfilled (delayed-payment safety);
  • the later checkout.session.async_payment_succeeded fulfils exactly once;
  • replaying the async success event is a no-op (no duplicate payment / membership);
  • a genuine $0 'no_payment_required' session IS fulfilled;
  • invoice.paid (subscription_cycle) extends the term, a REPLAY of the same event does NOT extend
    again (the historical "add another year on replay" bug), and a genuinely-new cycle event does;
  • a FAILED partner-commission write or reversal is never swallowed behind a 200: the webhook
    refuses to ack (non-2xx), the settlement/refund transaction rolls back whole, and Stripe's
    redelivery of the same event completes everything exactly once.

Exit code 0 iff every assertion passes.  Run from backend/:
    python3 tests/payments_replay_test.py
"""
import hashlib, hmac, json, os, socket, sqlite3, subprocess, time, urllib.error, urllib.request

HERE = os.path.dirname(os.path.abspath(__file__))
BACKEND = os.path.dirname(HERE)
DLL = os.path.join(BACKEND, "bin", "Release", "net8.0", "PCI.Backend.dll")
DB = os.path.join(HERE, "_payments_replay.db")
STORAGE = os.path.join(HERE, "_payments_replay_storage")
WEBHOOK_SECRET = "whsec_payments_replay_secret"
STRIPE_KEY = "sk_test_payments_replay"

passed = failed = 0
def chk(name, cond, extra=""):
    global passed, failed
    if cond: passed += 1; print(f"  PASS  {name}")
    else: failed += 1; print(f"  FAIL  {name}  {extra}")

def free_port():
    s = socket.socket(); s.bind(("127.0.0.1", 0)); p = s.getsockname()[1]; s.close(); return p

PORT = free_port(); BASE = f"http://127.0.0.1:{PORT}"

def req(method, path, body=None, raw=None, headers=None):
    hdr = dict(headers or {}); data = None
    if raw is not None: data = raw
    elif body is not None: data = json.dumps(body).encode(); hdr["Content-Type"] = "application/json"
    r = urllib.request.Request(BASE + path, data=data, method=method, headers=hdr)
    try:
        with urllib.request.urlopen(r) as resp: return resp.status, resp.read().decode()
    except urllib.error.HTTPError as e: return e.code, e.read().decode()

def sha256hex(s): return hashlib.sha256(s.encode()).hexdigest()
def dbconn(): c = sqlite3.connect(DB, timeout=5); c.row_factory = sqlite3.Row; return c

def send_event(etype, obj, event_id):
    evt = {"id": event_id, "object": "event", "api_version": "2024-06-20", "created": int(time.time()),
           "livemode": False, "pending_webhooks": 1, "request": {"id": None, "idempotency_key": None},
           "type": etype, "data": {"object": obj, "previous_attributes": None}}
    payload = json.dumps(evt); ts = int(time.time())
    sig = hmac.new(WEBHOOK_SECRET.encode(), f"{ts}.{payload}".encode(), hashlib.sha256).hexdigest()
    return req("POST", "/api/webhook", raw=payload.encode(),
               headers={"Content-Type": "application/json", "Stripe-Signature": f"t={ts},v1={sig}"})

def checkout_session(session_id, email, pi_id, payment_status, product="membership", amount=9900):
    meta = {"product": product, "first_name": "Pay", "last_name": "Test", "country": "GB",
            "final_amount": str(amount/100), "code_amount": "0", "standard_amount": str(amount/100), "default_discount": "0"}
    return {"id": session_id, "object": "checkout.session", "amount_total": amount, "mode": "payment",
            "customer_email": email, "customer_details": {"email": email},
            "payment_intent": pi_id, "metadata": meta, "payment_status": payment_status}

def invoice(cust, period_end_unix, inv_id):
    return {"id": inv_id, "object": "invoice", "billing_reason": "subscription_cycle",
            "customer": cust, "period_end": period_end_unix}

def boot():
    for f in (DB, DB + "-wal", DB + "-shm"):
        try: os.remove(f)
        except OSError: pass
    env = dict(os.environ, DATABASE_FILE=DB, PORT=str(PORT), STORAGE_ROOT=STORAGE,
               STRIPE_SECRET_KEY=STRIPE_KEY, STRIPE_WEBHOOK_SECRET=WEBHOOK_SECRET,
               ASPNETCORE_ENVIRONMENT="Development")
    log = open(os.path.join(HERE, "_payments_replay_boot.log"), "wb")
    proc = subprocess.Popen(["dotnet", DLL], env=env, cwd=BACKEND, stdout=log, stderr=subprocess.STDOUT)
    deadline = time.time() + 120
    while time.time() < deadline:
        if proc.poll() is not None: raise SystemExit(f"server exited during boot (exit {proc.returncode})")
        try:
            if req("GET", "/api/health")[0] == 200: return proc
        except Exception: pass
        time.sleep(0.5)
    proc.terminate(); raise SystemExit("server did not boot within 120s")

def count(sql, args=()):
    con = dbconn(); n = con.execute(sql, args).fetchone()[0]; con.close(); return n

def main():
    proc = boot()
    try:
        # ===== A. paid-status gate + delayed-payment success + replay =====
        e1 = "delayed.payer@example.com"; pi1 = "pi_delayed_1"; cs1 = "cs_delayed_1"
        c, _ = send_event("checkout.session.completed", checkout_session(cs1, e1, pi1, "unpaid"), "evt_cs1_completed")
        chk("A1 unpaid completed accepted (200) but not fulfilled",
            c == 200 and count("SELECT COUNT(*) FROM users WHERE email=?", (e1,)) == 0
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id=?", (pi1,)) == 0, c)

        c, _ = send_event("checkout.session.async_payment_succeeded", checkout_session(cs1, e1, pi1, "paid"), "evt_cs1_async_ok")
        paid1 = count("SELECT COUNT(*) FROM payments WHERE provider_payment_id=? AND payment_status='paid'", (pi1,))
        user1 = count("SELECT COUNT(*) FROM users WHERE email=?", (e1,))
        chk("A2 async_payment_succeeded fulfils exactly once", c == 200 and paid1 == 1 and user1 == 1, (c, paid1, user1))
        con = dbconn(); uid1 = con.execute("SELECT id FROM users WHERE email=?", (e1,)).fetchone(); con.close()
        mem1 = count("SELECT COUNT(*) FROM memberships WHERE user_id=?", (uid1["id"],)) if uid1 else -1
        chk("A3 membership granted once by the settled payment", mem1 == 1, mem1)

        # replay the SAME async success event → no duplicate payment / membership
        send_event("checkout.session.async_payment_succeeded", checkout_session(cs1, e1, pi1, "paid"), "evt_cs1_async_ok")
        chk("A4 async replay is a no-op (payments still 1)",
            count("SELECT COUNT(*) FROM payments WHERE provider_payment_id=?", (pi1,)) == 1
            and count("SELECT COUNT(*) FROM memberships WHERE user_id=?", (uid1["id"],)) == 1)

        # ===== B. $0 'no_payment_required' is a genuine settlement =====
        e2 = "free.comp@example.com"; pi2 = "pi_free_2"; cs2 = "cs_free_2"
        send_event("checkout.session.completed", checkout_session(cs2, e2, pi2, "no_payment_required", amount=0), "evt_cs2_free")
        chk("B1 no_payment_required ($0) is fulfilled",
            count("SELECT COUNT(*) FROM users WHERE email=?", (e2,)) == 1
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id=?", (pi2,)) == 1)

        # ===== C. invoice.paid extends once; replay does NOT double-extend; new cycle does =====
        con = dbconn()
        uid = con.execute("INSERT INTO users(email,first_name,last_name,role,status) VALUES('subber@example.com','Sub','Ber','student','active')", ).lastrowid
        con.execute("INSERT INTO memberships(user_id,membership_type,status,start_date,expiry_date,stripe_customer_id,subscription_status) "
                    "VALUES(?, 'Annual','active', datetime('now'), datetime('now'), 'cus_sub_1','active')", (uid,))
        con.commit(); con.close()
        def expiry():
            con = dbconn(); r = con.execute("SELECT expiry_date FROM memberships WHERE stripe_customer_id='cus_sub_1'").fetchone(); con.close(); return r["expiry_date"]
        e_before = expiry()
        pe1 = int(time.time()) + 365*86400
        send_event("invoice.paid", invoice("cus_sub_1", pe1, "in_cycle_1"), "evt_inv_1")
        e_after1 = expiry()
        chk("C1 invoice.paid extends the term to the billing-period end", e_after1 != e_before and e_after1 > e_before, (e_before, e_after1))

        # REPLAY the same event id → must NOT extend again (the P0 replay bug)
        send_event("invoice.paid", invoice("cus_sub_1", pe1, "in_cycle_1"), "evt_inv_1")
        e_after_replay = expiry()
        chk("C2 replay of the same invoice.paid does NOT double-extend", e_after_replay == e_after1, (e_after1, e_after_replay))

        # a genuinely-new cycle (new event id, later period end) DOES extend
        pe2 = int(time.time()) + 730*86400
        send_event("invoice.paid", invoice("cus_sub_1", pe2, "in_cycle_2"), "evt_inv_2")
        e_after2 = expiry()
        chk("C3 a new billing cycle extends again", e_after2 > e_after_replay, (e_after_replay, e_after2))

        # ===== D. commission writes are never silently lost behind a 200 (finance P0) =====
        # A commission/reversal failure used to be swallowed while the webhook acked — Stripe never
        # redelivered and the money record was gone for good. Break the ledger table for real, prove the
        # webhook refuses to ack and settles NOTHING, then repair and redeliver the same event.
        con = dbconn()
        pid = con.execute("INSERT INTO training_partners(name,tier,status,commission_pct,partner_type) "
                          "VALUES('Webhook Partner','registered','active',10,'marketing')").lastrowid
        aid = con.execute("INSERT INTO partner_agreements(partner_id,agreement_number,effective_from,status,refund_hold_days) "
                          "VALUES(?,?, '2026-01-01','active',0)", (pid, f"AGR-WH-{pid}")).lastrowid
        con.execute("INSERT INTO partner_commission_rules(agreement_id,partner_id,commission_type,commission_rate_bp,commission_basis,priority,active) "
                    "VALUES(?,?, 'percentage',1000,'net_after_discount',100,1)", (aid, pid))
        con.execute("INSERT INTO discount_codes(code,discount_type,discount_value,applies_to,active,status,code_type,partner_id) "
                    "VALUES('WH-CODE','percentage',0,'exam',1,'active','campaign',?)", (pid,))
        con.commit(); con.close()

        def coded_session(session_id, email, pi_id, amount=50000):
            s = checkout_session(session_id, email, pi_id, "paid", product="exam", amount=amount)
            s["metadata"]["discount_code"] = "WH-CODE"
            return s

        def break_ledger():
            con = dbconn(); con.execute("ALTER TABLE partner_commission_transactions RENAME TO pct_broken"); con.commit(); con.close()
        def repair_ledger():
            con = dbconn(); con.execute("ALTER TABLE pct_broken RENAME TO partner_commission_transactions"); con.commit(); con.close()

        c, _ = send_event("checkout.session.completed", coded_session("cs_wh_1", "wh.one@example.com", "pi_wh_1"), "evt_wh_1")
        chk("D1 a partner-coded sale records its commission",
            c == 200 and count("SELECT COUNT(*) FROM partner_commission_transactions") == 1, c)

        break_ledger()
        c, _ = send_event("checkout.session.completed", coded_session("cs_wh_2", "wh.two@example.com", "pi_wh_2"), "evt_wh_2")
        rolled_back = (count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_2'") == 0
                       and count("SELECT COUNT(*) FROM users WHERE email='wh.two@example.com'") == 0
                       and count("SELECT COUNT(*) FROM webhook_events WHERE event_id='evt_wh_2'") == 0)
        chk("D2 a failed commission write is NOT acked and the settlement rolls back whole",
            c >= 500 and rolled_back, (c, rolled_back))

        repair_ledger()
        c, _ = send_event("checkout.session.completed", coded_session("cs_wh_2", "wh.two@example.com", "pi_wh_2"), "evt_wh_2")
        chk("D3 redelivery after repair settles once, commission included",
            c == 200
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_2' AND payment_status='paid'") == 1
            and count("SELECT COUNT(*) FROM partner_commission_transactions t JOIN payments p ON p.id=t.payment_id "
                      "WHERE p.provider_payment_id='pi_wh_2'") == 1, c)

        charge = {"id": "ch_wh_1", "object": "charge", "payment_intent": "pi_wh_1",
                  "refunded": True, "amount_refunded": 50000}
        break_ledger()
        c, _ = send_event("charge.refunded", charge, "evt_wh_refund_1")
        chk("D4 a failed commission reversal is NOT acked and the refund flip rolls back",
            c >= 500
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_1' AND payment_status='paid'") == 1, c)

        repair_ledger()
        c, _ = send_event("charge.refunded", charge, "evt_wh_refund_1")
        chk("D5 the redelivered refund flips the payment and reverses the commission",
            c == 200
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_1' AND payment_status='refunded'") == 1
            and count("SELECT COUNT(*) FROM partner_commission_transactions WHERE reversal_of_transaction_id>0") == 1, c)

        # A partial refund takes the other code path (no revocation transaction) — same contract.
        part = {"id": "ch_wh_2", "object": "charge", "payment_intent": "pi_wh_2",
                "refunded": False, "amount_refunded": 12500}
        break_ledger()
        c, _ = send_event("charge.refunded", part, "evt_wh_refund_2")
        chk("D6 a failed PARTIAL-refund reversal is NOT acked and the flip rolls back",
            c >= 500
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_2' AND payment_status='paid'") == 1, c)
        repair_ledger()
        c, _ = send_event("charge.refunded", part, "evt_wh_refund_2")
        chk("D7 the redelivered partial refund records and reverses proportionally",
            c == 200
            and count("SELECT COUNT(*) FROM payments WHERE provider_payment_id='pi_wh_2' AND payment_status='partially_refunded'") == 1
            and count("SELECT COUNT(*) FROM partner_commission_transactions WHERE reversal_of_transaction_id>0") == 2, c)

        print(f"\n  == {passed}/{passed+failed} PASSED ==")
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()
    raise SystemExit(1 if failed else 0)

if __name__ == "__main__":
    main()
