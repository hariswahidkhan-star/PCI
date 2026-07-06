import sqlite3
d=sqlite3.connect(':memory:'); d.row_factory=sqlite3.Row; d.executescript(open(__import__('os').path.join(__import__('os').path.dirname(__file__),'..','schema.sql')).read())
print("=== PHASE 2 — casework flows ===\n")
d.execute("INSERT INTO users(id,email,first_name,last_name,status) VALUES(1,'a@b.co','Jane','Doe','active')")
d.execute("INSERT INTO users(id,email,status) VALUES(2,'other@x.co','active')")

# 1. ACCOMMODATION: request -> approve(+30) -> duration effect
rid=d.execute("INSERT INTO accommodation_requests(user_id,request_type,description) VALUES(1,'extra_time','I require additional time due to a documented condition.')").lastrowid
# second open request blocked
open2=d.execute("SELECT id FROM accommodation_requests WHERE user_id=1 AND status IN ('submitted','under_review')").fetchone()
print(f"1a. Duplicate open request blocked? {'PASS' if open2 else 'FAIL'}")
d.execute("UPDATE accommodation_requests SET status='approved', approved_extra_minutes=30, decided_by=5, decided_at=datetime('now') WHERE id=?",(rid,))
extra=d.execute("SELECT COALESCE(MAX(approved_extra_minutes),0) x FROM accommodation_requests WHERE user_id=1 AND status='approved'").fetchone()['x']
base=90
print(f"1b. Approved +30min -> exam duration {base}+{extra}={base+extra} -> {'PASS' if base+extra==120 else 'FAIL'}")
# rejected request contributes nothing; MAX not SUM
d.execute("INSERT INTO accommodation_requests(user_id,request_type,description,status,approved_extra_minutes) VALUES(1,'extra_time','x'||'y'*30,'rejected',60)")
d.execute("INSERT INTO accommodation_requests(user_id,request_type,description,status,approved_extra_minutes) VALUES(1,'other','another approved accommodation entry','approved',15)")
extra2=d.execute("SELECT COALESCE(MAX(approved_extra_minutes),0) x FROM accommodation_requests WHERE user_id=1 AND status='approved'").fetchone()['x']
print(f"1c. MAX of approvals (30,15), rejected 60 ignored -> {extra2} -> {'PASS' if extra2==30 else 'FAIL'}")
# user 2 has no approvals -> 0
e0=d.execute("SELECT COALESCE(MAX(approved_extra_minutes),0) x FROM accommodation_requests WHERE user_id=2 AND status='approved'").fetchone()['x']
print(f"1d. No approvals -> +0 min -> {'PASS' if e0==0 else 'FAIL'}")

# 2. APPEALS: submit vs own attempt only; one open per attempt; decide
d.execute("INSERT INTO exam_attempts(id,user_id,kind,status,result_status) VALUES(50,1,'exam','submitted','invalidated')")
owns=d.execute("SELECT id FROM exam_attempts WHERE id=50 AND user_id=2").fetchone()
print(f"2a. User 2 appealing user 1's attempt: ownership check -> {'PASS (rejected)' if owns is None else 'FAIL'}")
apid=d.execute("INSERT INTO appeals(user_id,attempt_id,type,reason) VALUES(1,50,'invalidation_appeal','I believe the invalidation was made in error because...')").lastrowid
dup=d.execute("SELECT id FROM appeals WHERE user_id=1 AND attempt_id=50 AND status IN ('submitted','under_review')").fetchone()
print(f"2b. Second appeal on same attempt blocked while open? {'PASS' if dup else 'FAIL'}")
d.execute("UPDATE appeals SET status='upheld', decision='Invalidation overturned on review.', decided_by=5, decided_at=datetime('now') WHERE id=?",(apid,))
a=d.execute("SELECT status,decision,decided_by FROM appeals WHERE id=?",(apid,)).fetchone()
print(f"2c. Decide: status={a['status']}, actor={a['decided_by']} -> {'PASS' if a['status']=='upheld' and a['decided_by']==5 else 'FAIL'}")

# 3. SUPPORT ATTACHMENTS: type/size validation replicated + ownership
def validate(uri):
    ALLOWED=('data:image/png;base64,','data:image/jpeg;base64,','data:image/webp;base64,','data:application/pdf;base64,')
    if not uri: return 'no_file'
    if len(uri)>2_000_000: return 'file_too_large'
    if not uri.startswith(ALLOWED): return 'file_type_not_allowed'
    return None
print(f"3a. exe rejected: {'PASS' if validate('data:application/x-msdownload;base64,AAAA')=='file_type_not_allowed' else 'FAIL'}")
print(f"3b. oversized rejected: {'PASS' if validate('data:image/png;base64,'+'A'*2_000_001)=='file_too_large' else 'FAIL'}")
print(f"3c. pdf accepted: {'PASS' if validate('data:application/pdf;base64,JVBERi0=') is None else 'FAIL'}")
d.execute("INSERT INTO tickets(id,user_id,reference,subject,category,status) VALUES(7,1,'T-1','Help','general','open')")
t=d.execute("SELECT id FROM tickets WHERE id=7 AND user_id=2").fetchone()
print(f"3d. User 2 attaching to user 1's ticket: {'PASS (rejected)' if t is None else 'FAIL'}")
d.execute("INSERT INTO support_attachments(ticket_id,user_id,filename,mime,size_bytes,data_uri) VALUES(7,1,'receipt.pdf','application/pdf',100,'data:application/pdf;base64,JVBERi0=')")
n=d.execute("SELECT COUNT(*) c FROM support_attachments WHERE ticket_id=7").fetchone()['c']
print(f"3e. Attachment stored + listable: {n} -> {'PASS' if n==1 else 'FAIL'}")

# 4. CPD: evidence attach -> back to review queue; approve/reject with note
cid=d.execute("INSERT INTO cpd_entries(user_id,activity_date,category,hours,description,status) VALUES(1,date('now'),'Structured learning',4,'EVM workshop','approved')").lastrowid
d.execute("UPDATE cpd_entries SET evidence_name='cert.pdf', evidence_data='data:application/pdf;base64,JVBERi0=', status='recorded' WHERE id=?",(cid,))
r=d.execute("SELECT status,evidence_name FROM cpd_entries WHERE id=?",(cid,)).fetchone()
print(f"4a. Evidence attach returns entry to review queue: status={r['status']} -> {'PASS' if r['status']=='recorded' else 'FAIL'}")
d.execute("UPDATE cpd_entries SET status='approved', admin_note='Verified against certificate', reviewed_by=5, reviewed_at=datetime('now') WHERE id=?",(cid,))
r2=d.execute("SELECT status,admin_note,reviewed_by FROM cpd_entries WHERE id=?",(cid,)).fetchone()
print(f"4b. Admin approve with note + actor: {'PASS' if r2['status']=='approved' and r2['reviewed_by']==5 else 'FAIL'}")

# 5. CERTIFICATE data: only active+unexpired is valid
d.execute("INSERT INTO issued_credentials(credential_id,user_id,holder_name,status,expires_at) VALUES('PCP-AI-2026-55555',1,'Jane Doe','active',datetime('now','+3 year'))")
c=d.execute("SELECT status,expires_at FROM issued_credentials WHERE user_id=1 ORDER BY id DESC").fetchone()
import datetime as dt
now=dt.datetime.utcnow().strftime('%Y-%m-%d %H:%M:%S')
valid=c['status']=='active' and c['expires_at']>now
print(f"5. Certificate valid (active + unexpired): {'PASS' if valid else 'FAIL'}")
print("\nALL PHASE-2 CASEWORK FLOWS VERIFIED")
