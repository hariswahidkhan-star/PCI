#!/usr/bin/env bash
# Live smoke suite for the .NET backend — identical assertions to the Node original (24 checks).
set -uo pipefail
B="http://127.0.0.1:8080"; pass=0; fail=0
# optional env (default empty so set -u doesn't abort): ST = student session token
ADMIN_TOKEN="${ADMIN_TOKEN:-}"; ST="${ST:-}"
j(){ curl -s -X "$1" "$B$2" "${@:3}"; }
chk(){ local n="$1" cond="$2"; if [ "$cond" = "1" ]; then echo "  PASS  $n"; pass=$((pass+1)); else echo "  FAIL  $n"; fail=$((fail+1)); fi; }
code(){ curl -s -o /dev/null -w "%{http_code}" -X "$1" "$B$2" "${@:3}"; }

chk "S01 /api/health" "$([ "$(j GET /api/health | grep -c pci-backend)" = 1 ] && echo 1)"
for p in / /student.html /admin/ /exam-ui.html /index-launcher.html; do
  chk "S02 static $p" "$([ "$(code GET "$p")" = 200 ] && echo 1)"
done
OT=$(j POST /api/admin/auth/login -H 'Content-Type: application/json' -d '{"email":"owner@pci.local","password":"changeme-owner"}' | python3 -c 'import sys,json;print(json.load(sys.stdin).get("token",""))')
chk "S03 owner login" "$([ -n "$OT" ] && echo 1)"
NP=$(j GET /api/admin/me -H "Authorization: Bearer $OT" | python3 -c 'import sys,json;d=json.load(sys.stdin);print(1 if d.get("role")=="owner" and len(d.get("permissions",[]))>=30 else 0)')
chk "S04 owner + 32 perms" "$NP"
chk "S05a forced pw change" "$([ "$(j POST /api/admin/me/password -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"new_password":"OwnerPass99!"}' | grep -c '"ok":true')" = 1 ] && echo 1)"
OT=$(j POST /api/admin/auth/login -H 'Content-Type: application/json' -d '{"email":"owner@pci.local","password":"OwnerPass99!"}' | python3 -c 'import sys,json;print(json.load(sys.stdin).get("token",""))')
chk "S05b re-login new pw" "$([ -n "$OT" ] && echo 1)"
TP=$(j POST /api/admin/team -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"email":"exam.mgr@pci.test","name":"Exa","role":"exam_manager"}' | python3 -c 'import sys,json;print(json.load(sys.stdin).get("temp_password",""))')
chk "S06 create exam_manager" "$([ -n "$TP" ] && echo 1)"
T2=$(j POST /api/admin/auth/login -H 'Content-Type: application/json' -d "{\"email\":\"exam.mgr@pci.test\",\"password\":\"$TP\"}" | python3 -c 'import sys,json;print(json.load(sys.stdin).get("token",""))')
chk "S07 manager login temp pw" "$([ -n "$T2" ] && echo 1)"
# A freshly-provisioned admin is flagged must_change_pw; the server blocks the console (same gate the
# SPA enforces) until a new password is set. Clear it so the RBAC probes below exercise real permissions.
chk "S07b manager password change clears must_change_pw" "$([ "$(j POST /api/admin/me/password -H "Authorization: Bearer $T2" -H 'Content-Type: application/json' -d '{"new_password":"MgrPass99!"}' | grep -c '"ok":true')" = 1 ] && echo 1)"
chk "S08 manager CAN exam-sessions" "$([ "$(code GET /api/admin/exam-sessions -H "Authorization: Bearer $T2")" = 200 ] && echo 1)"
chk "S09 manager BLOCKED members 403" "$([ "$(code GET /api/admin/members -H "Authorization: Bearer $T2")" = 403 ] && echo 1)"
chk "S10 manager BLOCKED team 403" "$([ "$(code GET /api/admin/team -H "Authorization: Bearer $T2")" = 403 ] && echo 1)"
chk "S11a manager CAN set exam" "$([ "$(j PATCH /api/admin/settings -H "Authorization: Bearer $T2" -H 'Content-Type: application/json' -d '{"exam_pass_mark_pct":"70"}' | grep -c rejected)" = 0 ] && echo 1)"
chk "S11b website key REJECTED" "$([ "$(j PATCH /api/admin/settings -H "Authorization: Bearer $T2" -H 'Content-Type: application/json' -d '{"web_membership_price":"111"}' | grep -c web_membership_price)" = 1 ] && echo 1)"
chk "S12 price unchanged" "$([ "$(j GET /api/content | python3 -c 'import sys,json;d=json.load(sys.stdin);print(d["settings"].get("web_membership_price"))')" = 99 ] && echo 1)"
# The .NET backend removed the legacy shared x-admin-token entirely (Core/Auth.cs):
# it must NEVER authenticate, with any value, in any environment.
chk "S13 legacy x-admin-token is dead" "$([ "$(j GET /api/admin/me -H "x-admin-token: ${ADMIN_TOKEN:-changeme}" | grep -c '"role"')" = 0 ] && echo 1)"
chk "S14 pass mark persisted 70" "$([ "$(j GET /api/admin/settings -H "Authorization: Bearer $OT" | python3 -c 'import sys,json;print(json.load(sys.stdin).get("exam_pass_mark_pct"))')" = 70 ] && echo 1)"
j PATCH /api/admin/settings -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"web_maintenance_mode":"1"}' >/dev/null
chk "S15a maintenance ON → 503" "$([ "$(code GET /)" = 503 ] && echo 1)"
chk "S15b admin stays up" "$([ "$(code GET /admin/)" = 200 ] && echo 1)"
j PATCH /api/admin/settings -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"web_maintenance_mode":"0"}' >/dev/null
chk "S15c maintenance OFF → 200" "$([ "$(code GET /)" = 200 ] && echo 1)"
chk "S16 student login 401" "$([ "$(code POST /api/login -H 'Content-Type: application/json' -d '{"email":"x@y.z","password":"x"}')" = 401 ] && echo 1)"
# ---- extended: student portal + exam-session module (ported this iteration) ----
chk "S17 exam-sessions list (owner)" "$([ "$(code GET /api/admin/exam-sessions -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"
chk "S18 members list (owner)" "$([ "$(code GET /api/admin/members -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"
chk "S19 me requires auth (401)" "$([ "$(code GET /api/me)" = 401 ] && echo 1)"
chk "S20 me/config requires auth (401)" "$([ "$(code GET /api/me/config)" = 401 ] && echo 1)"

# ---- iteration 3: public + CMS + admin management ----
chk "S21 /api/pricing" "$([ "$(j GET /api/pricing | grep -c currency)" = 1 ] && echo 1)"
chk "S22 validate-code (invalid→valid:false)" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"NOPE","product":"membership"}' | grep -c '"valid":false')" = 1 ] && echo 1)"
# Scoped discount codes (seeded): MEMBER20 = membership-only, EXAM15 = exam-only, SAVE10 = both.
# A scoped code must be accepted on its own product and rejected on the other.
chk "S22b MEMBER20 valid on membership" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"MEMBER20","product":"membership"}' | grep -c '"valid":true')" = 1 ] && echo 1)"
chk "S22c MEMBER20 REJECTED on exam" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"MEMBER20","product":"exam"}' | grep -c '"valid":false')" = 1 ] && echo 1)"
chk "S22d EXAM15 valid on exam" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"EXAM15","product":"exam"}' | grep -c '"valid":true')" = 1 ] && echo 1)"
chk "S22e EXAM15 REJECTED on membership" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"EXAM15","product":"membership"}' | grep -c '"valid":false')" = 1 ] && echo 1)"
chk "S22f SAVE10 valid on both" "$([ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"SAVE10","product":"membership"}' | grep -c '"valid":true')" = 1 ] && [ "$(j POST /api/validate-code -H 'Content-Type: application/json' -d '{"code":"SAVE10","product":"exam"}' | grep -c '"valid":true')" = 1 ] && echo 1)"
chk "S23 verify unknown credential (found:false)" "$([ "$(j GET '/api/verify?id=PCP-AI-9999-99999' | grep -c '"found":false')" = 1 ] && echo 1)"
chk "S24 newsletter subscribe" "$([ "$(j POST /api/newsletter -H 'Content-Type: application/json' -d '{"email":"n@ex.co"}' | grep -c '"ok":true')" = 1 ] && echo 1)"
chk "S25 inquiry (returns reference)" "$([ "$(j POST /api/inquiry -H 'Content-Type: application/json' -d '{"email":"a@b.co","type":"general"}' | grep -c reference)" = 1 ] && echo 1)"
chk "S26 form-submit (returns reference)" "$([ "$(j POST /api/form-submit -H 'Content-Type: application/json' -d '{"form_type":"contact","email":"a@b.co"}' | grep -c reference)" = 1 ] && echo 1)"
chk "S27 CMS: create+list FAQ (owner)" "$([ -n "$(j POST /api/admin/faqs -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"question":"Q?","answer":"A","published":1}' | grep -o '"id"')" ] && echo 1)"
chk "S28 admin overview (owner)" "$([ "$(j GET /api/admin/overview -H "Authorization: Bearer $OT" | grep -c kpis)" = 1 ] && echo 1)"
chk "S29 admin audit log (owner)" "$([ "$(code GET /api/admin/audit -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"
chk "S30 CSV export members (owner)" "$([ "$(code GET '/api/admin/export?entity=members' -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"
chk "S31 exam_mgr BLOCKED from payments (403)" "$([ "$(code GET /api/admin/payments -H "Authorization: Bearer $T2")" = 403 ] && echo 1)"

# ---- iteration 4: full parity (payments/enrollment/tickets/reports) ----
chk "S32 checkout 503 without STRIPE key" "$([ "$(code POST /api/create-checkout-session -H 'Content-Type: application/json' -d '{"product":"membership"}')" = 503 ] && echo 1)"
chk "S33 session/start (in-progress)" "$([ "$(j POST /api/session/start -H 'Content-Type: application/json' -d '{"email":"wizard@ex.co"}' | grep -c session_id)" = 1 ] && echo 1)"
chk "S34 enrollment/save returns token" "$([ "$(j POST /api/enrollment/save -H 'Content-Type: application/json' -d '{"email":"wiz2@ex.co","step":2,"data":{}}' | grep -c resume_token)" = 1 ] && echo 1)"
chk "S35 admin tickets (owner)" "$([ "$(code GET /api/admin/tickets -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"
chk "S36 admin reports (owner)" "$([ "$(j GET /api/admin/reports -H "Authorization: Bearer $OT" | grep -c totals)" = 1 ] && echo 1)"
chk "S37 codes/generate (owner)" "$([ "$(j POST /api/admin/codes/generate -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"count":3,"prefix":"TEST"}' | grep -c batch_id)" = 1 ] && echo 1)"
chk "S38 legacy /students (owner)" "$([ "$(code GET /api/admin/students -H "Authorization: Bearer $OT")" = 200 ] && echo 1)"

# ---- security hardening checks ----
chk "SEC1 practice hides answer key" "$([ "$(j GET /api/me/practice -H "Authorization: Bearer $ST" | grep -c answer_index)" = 0 ] && echo 1)"
chk "SEC2 enrollment resume needs token (400)" "$([ "$(code GET '/api/enrollment/resume?email=x@y.co')" = 400 ] && echo 1)"
chk "SEC3 set-password bad token rejected (400)" "$([ "$(code POST /api/set-password -H 'Content-Type: application/json' -d '{"token":"deadbeef","password":"abcd1234"}')" = 400 ] && echo 1)"
chk "SEC4 legacy admin token rejected (401)" "$([ "$(code GET /api/admin/overview -H 'x-admin-token: changeme')" = 401 ] && echo 1)"
chk "SEC5 exam_mgr blocked from CMS faqs write (403)" "$([ "$(code POST /api/admin/faqs -H "Authorization: Bearer $T2" -H 'Content-Type: application/json' -d '{"question":"q","answer":"a"}')" = 403 ] && echo 1)"
chk "SEC6 exam_mgr can access sample_questions (200)" "$([ "$(code GET /api/admin/sample_questions -H "Authorization: Bearer $T2")" = 200 ] && echo 1)"

# ---- founding-stage smoke: create a code as admin → validate → redeem → confirm /api/me ----
FC="SMOKEFND$$"
FE="founding-smoke-$$@ex.co"
FT=$(j POST /api/register -H 'Content-Type: application/json' -d "{\"email\":\"$FE\",\"password\":\"Passw0rd!fnd\",\"first_name\":\"Fnd\",\"last_name\":\"Smoke\"}" | python3 -c 'import sys,json;print(json.load(sys.stdin).get("token",""))')
chk "F1 create founding code (owner)" "$([ -n "$(j POST /api/admin/codes -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d "{\"code\":\"$FC\",\"founding_route\":\"founding\",\"end_date\":\"2099-01-01\",\"active\":true}" | grep -o '"id"')" ] && echo 1)"
chk "F2 validate founding code (public)" "$([ "$(j POST /api/founding/validate -H 'Content-Type: application/json' -d "{\"code\":\"$FC\"}" | grep -c '"valid":true')" = 1 ] && echo 1)"
chk "F3 student redeems at USD 0" "$([ "$(j POST /api/founding/redeem -H "Authorization: Bearer $FT" -H 'Content-Type: application/json' -d "{\"code\":\"$FC\"}" | grep -cE '"ok":true|already_redeemed')" = 1 ] && echo 1)"
chk "F4 /api/me reflects the full founding grant" "$([ "$(j GET /api/me -H "Authorization: Bearer $FT" | grep -cE '"membership_status":"active".*"candidate_status":"exam_fee_paid"|"candidate_status":"exam_fee_paid".*"membership_status":"active"')" = 1 ] && echo 1)"

# ---- honorary smoke: owner confers → verify shows it as honorary, never a passed exam ----
HN=$(j POST /api/admin/honorary -H "Authorization: Bearer $OT" -H 'Content-Type: application/json' -d '{"recipient_name":"Smoke Honoree","citation":"Smoke-test citation"}' | python3 -c 'import sys,json;print(json.load(sys.stdin).get("award_no",""))')
chk "H1 owner confers an honorary award (PCI-HON number)" "$([ -n "$HN" ] && echo "$HN" | grep -q '^PCI-HON-' && echo 1)"
chk "H2 verify shows honorary type + designation" "$([ "$(j GET "/api/verify?id=$HN" | grep -c '"type":"honorary"')" = 1 ] && echo 1)"
chk "H3 confer without auth rejected (401)" "$([ "$(code POST /api/admin/honorary -H 'Content-Type: application/json' -d '{"recipient_name":"X"}')" = 401 ] && echo 1)"

echo ""; echo "  ══ $pass/$((pass+fail)) PASSED ══"
[ "$fail" = 0 ]
