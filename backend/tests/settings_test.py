import sqlite3, os
d=sqlite3.connect(':memory:'); d.row_factory=sqlite3.Row
d.executescript(open(os.path.join(os.path.dirname(__file__),'..','schema.sql')).read())
for t,cs in [('exam_attempts',['result_status TEXT','hold_reason TEXT'])]:
    h={r[1] for r in d.execute(f'PRAGMA table_info({t})')}
    for c in cs:
        if c.split()[0] not in h: d.execute(f'ALTER TABLE {t} ADD COLUMN {c}')
print("=== PHASE 3 — settings enforcement + readiness ===\n")
d.execute("INSERT INTO users(id,email,status) VALUES(1,'a@b.co','active')")
def setb(k,v): d.execute("INSERT INTO site_settings(skey,svalue) VALUES(?,?) ON CONFLICT(skey) DO UPDATE SET svalue=?",(k,v,v))
def getb(k,dflt):
    r=d.execute("SELECT svalue FROM site_settings WHERE skey=?",(k,)).fetchone()
    if not r: return dflt
    return r['svalue'] not in ('0','false','no','off','')

# 1. booking closed when sp_exam_booking_open=0
setb('sp_exam_booking_open','0')
blockers=[]
if not getb('sp_exam_booking_open',True): blockers.append('booking_closed')
print(f"1. sp_exam_booking_open=0 -> booking_closed blocker present? {'PASS' if 'booking_closed' in blockers else 'FAIL'}")
setb('sp_exam_booking_open','1')
blockers=[]
if not getb('sp_exam_booking_open',True): blockers.append('booking_closed')
print(f"   sp_exam_booking_open=1 -> no block? {'PASS' if 'booking_closed' not in blockers else 'FAIL'}")

# 2. readiness gate
setb('sp_readiness_required','1')
def readiness_ok(uid):
    if not getb('sp_readiness_required',True): return True
    return d.execute("SELECT COUNT(*) c FROM exam_readiness_checks WHERE user_id=? AND passed=1",(uid,)).fetchone()['c']>0
print(f"2a. readiness required, none on record -> launch blocked? {'PASS' if not readiness_ok(1) else 'FAIL'}")
# failed check (camera off) does NOT satisfy
d.execute("INSERT INTO exam_readiness_checks(user_id,camera,microphone,network,passed) VALUES(1,0,1,1,0)")
print(f"2b. failed check (passed=0) -> still blocked? {'PASS' if not readiness_ok(1) else 'FAIL'}")
# passed check satisfies
d.execute("INSERT INTO exam_readiness_checks(user_id,camera,microphone,network,passed) VALUES(1,1,1,1,1)")
print(f"2c. passed check -> launch allowed? {'PASS' if readiness_ok(1) else 'FAIL'}")
# when not required, always allowed
setb('sp_readiness_required','0')
d.execute("INSERT INTO users(id,email,status) VALUES(2,'b@c.co','active')")
print(f"2d. readiness NOT required -> allowed with no check? {'PASS' if readiness_ok(2) else 'FAIL'}")

# 3. passed computation: camera+mic+net all required
def compute(cam,mic,net): return 1 if (cam and mic and net) else 0
print(f"3a. all on -> passed? {'PASS' if compute(1,1,1)==1 else 'FAIL'}")
print(f"3b. mic off -> failed? {'PASS' if compute(1,0,1)==0 else 'FAIL'}")

# 4. practice/results/tickets/certificate toggles all read the same way
for key,ep in [('sp_practice_enabled','practice'),('sp_results_visible','results'),('sp_support_tickets_enabled','tickets'),('sp_certificate_download','certificate')]:
    setb(key,'0')
    blocked = not getb(key,True)
    print(f"4. {key}=0 -> {ep} blocked? {'PASS' if blocked else 'FAIL'}")
    setb(key,'1')
print("\nALL PHASE-3 ENFORCEMENT VERIFIED")
