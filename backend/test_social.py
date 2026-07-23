#!/usr/bin/env python3
"""LEGACY — superseded by the account-based Social Media Management API.

The flat URL map this script exercised (POST /api/admin/social with linkedin/x/…)
was replaced by Endpoints/Social.cs account CRUD + share settings. Deep coverage
now lives in:

  • backend/tests/integration_test.py  — §21 (publishing) + §21B (profiles) + §56 (marketing)
  • backend/tests/social_deep_test.py  — focused runner for those three sections

Run:  cd backend && python3 tests/social_deep_test.py
"""
import sys
print("backend/test_social.py is retired — run tests/social_deep_test.py instead.", file=sys.stderr)
sys.exit(2)
