# PCI Backend logic tests
Run each against real SQLite from the repo root (they load `schema.sql` directly):

    python3 tests/lifecycle_test.py   # Section-A result lifecycle: consents gate, auto-hold, release, snapshot, entitlement, webhook ledger
    python3 tests/release_test.py     # Admin result management: release-held→credential, idempotent re-release, invalidate→revoke, configured pass mark, expiry-aware verify
    python3 tests/casework_test.py    # Phase-2 casework: accommodations (+duration effect), appeals, support attachments, CPD evidence/review, certificate validity

All assertions must print PASS/✓. These replicate the exact production SQL and rules; they are the same
suites used during development and are suitable for wiring into CI.
