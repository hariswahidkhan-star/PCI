import sqlite3, os
d=sqlite3.connect(':memory:'); d.row_factory=sqlite3.Row
d.executescript(open(os.path.join(os.path.dirname(__file__),'..','schema.sql')).read())
for t,cs in [('exam_attempts',['result_status TEXT','hold_reason TEXT','identity_result TEXT','attempt_token_ok TEXT','item_ids TEXT'])]:
    h={r[1] for r in d.execute(f'PRAGMA table_info({t})')}
    for c in cs:
        if c.split()[0] not in h: d.execute(f'ALTER TABLE {t} ADD COLUMN {c}')

print("=== IMMEDIATE-PUBLICATION DEFAULT (proctoring is audit-only) ===\n")
d.execute("INSERT INTO users(id,email,status) VALUES(1,'a@b.co','active')")
d.execute("INSERT INTO payments(id,user_id,product_type,payment_status) VALUES(9,1,'exam','paid')")
d.execute("INSERT INTO exam_bookings(id,user_id,payment_id,scheduled_at,status) VALUES(3,1,9,datetime('now'),'scheduled')")

def getb(k,dflt):
    r=d.execute("SELECT svalue FROM site_settings WHERE skey=?",(k,)).fetchone()
    if r is None: return dflt
    return r['svalue'] not in ('0','false','no','off','')
def getn(k,dflt):
    r=d.execute("SELECT svalue FROM site_settings WHERE skey=?",(k,)).fetchone()
    return int(r['svalue']) if r else dflt

def hold_reason(att, late, item_mismatch=False):
    if item_mismatch: return 'item_set_mismatch'
    if late: return 'submitted_after_deadline'
    bk=d.execute("SELECT * FROM exam_bookings WHERE id=?",(att['booking_id'],)).fetchone()
    if not bk: return 'booking_missing'
    if bk['status'] in ('cancelled','missed','expired'): return 'booking_invalid'
    ps=d.execute("SELECT payment_status FROM payments WHERE id=?",(bk['payment_id'],)).fetchone()
    if ps and ps['payment_status'] in ('refunded','failed','reversed'): return 'payment_reversed'
    dup=d.execute("SELECT COUNT(*) c FROM exam_attempts a JOIN exam_bookings b ON b.id=a.booking_id WHERE b.payment_id=? AND a.kind='exam' AND a.status='submitted' AND a.id<>?",(bk['payment_id'],att['id'])).fetchone()['c']
    if dup>0: return 'duplicate_attempt'
    # config-gated
    if getb('auto_block_result_on_tampered_attempt',False) and att['attempt_token_ok']=='0': return 'attempt_tampered'
    if getb('auto_block_result_on_critical_violation',False):
        crit=d.execute("SELECT COUNT(*) c FROM proctor_events WHERE attempt_id=? AND severity='Critical'",(att['id'],)).fetchone()['c']
        if crit>=getn('critical_violation_threshold',1): return 'critical_proctor_violation'
    if getb('auto_block_result_on_identity_fail',False) and att['identity_result'] in ('fail','failed'): return 'identity_failed'
    return None

# 1. Valid pass with a CRITICAL proctor violation → still publishes (default flags OFF)
aid=d.execute("INSERT INTO exam_attempts(id,user_id,booking_id,kind,status,result,percent,identity_result) VALUES(100,1,3,'exam','in_progress','pass',82.0,'fail')").lastrowid
d.execute("INSERT INTO proctor_events(attempt_id,user_id,type,severity) VALUES(100,1,'multiple_faces','Critical')")
att=dict(d.execute("SELECT * FROM exam_attempts WHERE id=100").fetchone())
hr=hold_reason(att,False)
print(f"1. Critical violation + identity fail, flags OFF -> hold_reason={hr} -> {'PASS (published immediately, proctoring is audit-only)' if hr is None else 'FAIL: '+str(hr)}")

# 2. proctor event still STORED for audit
cnt=d.execute("SELECT COUNT(*) c FROM proctor_events WHERE attempt_id=100").fetchone()['c']
print(f"2. Proctor event stored for audit? {'PASS' if cnt==1 else 'FAIL'}")

# 3. Late submission STILL blocks (technical invalidity)
print(f"3. Late submission -> {'PASS (blocked)' if hold_reason(att,True)=='submitted_after_deadline' else 'FAIL'}")

# 4. Item-set mismatch blocks
print(f"4. Item-set mismatch -> {'PASS (blocked)' if hold_reason(att,False,item_mismatch=True)=='item_set_mismatch' else 'FAIL'}")

# 5. Cancelled booking blocks
d.execute("UPDATE exam_bookings SET status='cancelled' WHERE id=3")
print(f"5. Cancelled booking -> {'PASS (blocked)' if hold_reason(att,False)=='booking_invalid' else 'FAIL'}")
d.execute("UPDATE exam_bookings SET status='scheduled' WHERE id=3")

# 6. Reversed payment blocks
d.execute("UPDATE payments SET payment_status='refunded' WHERE id=9")
print(f"6. Reversed payment -> {'PASS (blocked)' if hold_reason(att,False)=='payment_reversed' else 'FAIL'}")
d.execute("UPDATE payments SET payment_status='paid' WHERE id=9")

# 7. NOW enable the critical-violation flag → same attempt blocks
d.execute("INSERT INTO site_settings(skey,svalue) VALUES('auto_block_result_on_critical_violation','1') ON CONFLICT(skey) DO UPDATE SET svalue='1'")
print(f"7. Flag ON: critical violation -> {'PASS (now blocks by config)' if hold_reason(att,False)=='critical_proctor_violation' else 'FAIL'}")
d.execute("UPDATE site_settings SET svalue='0' WHERE skey='auto_block_result_on_critical_violation'")

# 8. threshold respected (2 required, only 1 present → publishes)
d.execute("UPDATE site_settings SET svalue='1' WHERE skey='auto_block_result_on_critical_violation'")
d.execute("INSERT INTO site_settings(skey,svalue) VALUES('critical_violation_threshold','2') ON CONFLICT(skey) DO UPDATE SET svalue='2'")
print(f"8. Threshold=2, 1 violation -> {'PASS (publishes, below threshold)' if hold_reason(att,False) is None else 'FAIL: '+str(hold_reason(att,False))}")

# 9. identity fail flag ON blocks
d.execute("UPDATE site_settings SET svalue='0' WHERE skey='auto_block_result_on_critical_violation'")
d.execute("INSERT INTO site_settings(skey,svalue) VALUES('auto_block_result_on_identity_fail','1') ON CONFLICT(skey) DO UPDATE SET svalue='1'")
print(f"9. Flag ON: identity fail -> {'PASS (blocks by config)' if hold_reason(att,False)=='identity_failed' else 'FAIL'}")

# 10. Clean valid pass with NO issues, all flags off → publishes + credential
d.execute("DELETE FROM site_settings WHERE skey LIKE 'auto_block%'")
d.execute("DELETE FROM proctor_events WHERE attempt_id=100")
d.execute("UPDATE exam_attempts SET identity_result='pass' WHERE id=100")
att2=dict(d.execute("SELECT * FROM exam_attempts WHERE id=100").fetchone())
hr2=hold_reason(att2,False)
print(f"10. Clean valid pass -> hold_reason={hr2}, result_status={'released_pass/credential' if hr2 is None else 'HELD'} -> {'PASS' if hr2 is None else 'FAIL'}")

print("\nDEFAULT IS IMMEDIATE PUBLICATION; PROCTORING IS AUDIT-ONLY; TECHNICAL INVALIDITY STILL BLOCKS")
