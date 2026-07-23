#!/usr/bin/env python3
"""Deep social-features runner — boots the real backend once and exercises:

  • §21  Content Centre social publishing (Discord/Telegram/Mastodon/Bluesky)
  • §21B Social Media Management (profiles, share buttons, footer, audit)
  • §56  Marketing / Ads / Search Console centre (OAuth secrecy, approval gates)

Run from backend/:  python3 tests/social_deep_test.py

This is the focused path for "test all social features" without waiting on the
full integration suite. The same assertions live in integration_test.py so CI
still covers them on every PR.
"""
import os, sys, time

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)

import integration_test as it  # noqa: E402


def main():
    print("== social deep test: building is assumed (Release dll) ==")
    if not os.path.exists(it.DLL):
        print(f"missing {it.DLL} — run: dotnet build -c Release", file=sys.stderr)
        sys.exit(2)
    proc = it.boot()
    try:
        admin = it.admin_login()
        # Seed a least-privileged viewer so §21B·x / §56a RBAC assertions fire.
        c, vb = it.jget("POST", "/api/admin/team", token=admin,
                        body={"email": "social-viewer@pci.test", "name": "Social Viewer", "role": "viewer"})
        if c == 200 and vb.get("temp_password"):
            c2, vl = it.jget("POST", "/api/admin/auth/login",
                             body={"email": "social-viewer@pci.test", "password": vb["temp_password"]})
            if c2 == 200 and vl.get("token"):
                it.clear_must_change(vl["token"], newpw="Viewer!Pw1")
                # Re-login after password clear
                c3, vl2 = it.jget("POST", "/api/admin/auth/login",
                                  body={"email": "social-viewer@pci.test", "password": "Viewer!Pw1"})
                if c3 == 200 and vl2.get("token"):
                    it._VIEWER_TOK = vl2["token"]
        if not getattr(it, "_VIEWER_TOK", None):
            vtok = "socialview_" + it.sha256hex("social-viewer")[:20]
            con = it.dbconn()
            row = con.execute("SELECT id FROM admin_users WHERE email=?", ("social-viewer@pci.test",)).fetchone()
            if row:
                con.execute("UPDATE admin_users SET must_change_pw=0 WHERE id=?", (row[0],))
                con.execute("INSERT INTO admin_sessions(admin_id,token,expires_at) VALUES(?,?, datetime('now','+1 day'))",
                            (row[0], it.sha256hex(vtok)))
                con.commit()
                it._VIEWER_TOK = vtok
            con.close()

        it.widen_window(admin)
        it.test_social_publishing(admin)
        it.test_social_media_management(admin)
        it.test_marketing_centre(admin)
    finally:
        proc.terminate()
        try: proc.wait(timeout=10)
        except Exception: proc.kill()

    total = it.passed + it.failed
    print(f"\n  ══ {it.passed}/{total} PASSED (social deep) ══")
    sys.exit(0 if it.failed == 0 else 1)


if __name__ == "__main__":
    main()
