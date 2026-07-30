# CCP Phase 5 — External ATS integration (contracts only)

Scope: §10.8. Outbound integration between PCI World Careers and an employer's own applicant
tracking system, so a verified employer that already runs Greenhouse, Workday, Lever or similar can
see World applications where its recruiters actually work.

**This phase ships no integration, and that is the correct outcome rather than a shortfall.** The
baseline settled it before Phase 4 was written (`CCP_PHASE0_BASELINE.md:307`): *"External ATS
vendors — deferred to Phase 5, contracts only until sandbox credentials exist"*, and §28 forbids
marking an integration complete on mocks. An adapter tested only against a fixture asserts that our
idea of a vendor's API is self-consistent. It cannot discover that the real endpoint paginates
differently, rejects a field we always send, rate-limits at a tenth of the documented figure, or
returns 200 with an error body — which is the entire class of defect an integration has.

So what this document defines is the **shape** the integration must take, chosen now while it costs
nothing, so that the day credentials arrive the work is wiring rather than design. What it does not
do is pretend that shape has been validated against anything.

---

## 1. What must never happen

| Prohibition | Control |
|---|---|
| An applicant's data reaches a third-party system they did not consent to | §3 — a per-destination consent, distinct from the employer consent Phase 4 already records; no export without one |
| An export runs for an employer that is not verified, or is suspended | §4 — the connector re-reads the employer state per run, the same predicate as every other careers read path |
| A vendor credential is readable from the admin UI, an export, or a log | §5 — write-only credential fields, redacted everywhere, the pattern `system-check` already follows |
| A failed export silently drops an application | §6 — the outbox is durable and retried; a permanently failed row is visible, never deleted |
| A retry duplicates a candidate in the employer's ATS | §6 — an idempotency key per (application, destination), and the vendor's own dedupe key where one exists |
| We claim an integration works because a mock said so | §7 — the certification gate: no destination may be marked `live` without a recorded run against the vendor's sandbox |
| An inbound vendor callback is trusted without verification | §5 — signed callbacks only, verified against the exact bytes received |

---

## 2. Why this is an outbox and not a webhook call in the request

Phase 4's application submit is already a transaction that freezes answers, the CV reference and
consent. Calling a vendor API inside that transaction would put a third party's availability on the
critical path of a candidate pressing Apply: a slow vendor becomes a slow application, and a vendor
outage becomes an apply outage. It would also hold the database lock across network I/O, which this
codebase has an explicit regression test against (`tests/waiver_vendor_stall_test.py`).

The integration is therefore an **outbox** — the pattern Phase 1 already established for community
broadcast and the platform uses for `Comms.Enqueue`. The apply transaction writes a row saying "this
application should reach that destination"; a worker delivers it afterwards, with retries and a
lease, and publication of the candidate to the vendor happens strictly after our own commit.

The worker claims rows through `WorkerLease` — atomic conditional UPDATE, crash recovery via
`RecoverExpired` — because that is the mechanism the repo already proved under test
(`tests/worker_leasing_test.py`), and a second claiming mechanism would be a second thing to get
wrong.

---

## 3. Consent is per destination, not per employer

Phase 4 records consent to disclose an application **to a named employer, for a named purpose,
under a named policy version** (`CCP_PHASE4_DESIGN.md` §5.3). Sending that application onward to a
third-party ATS is a **different disclosure to a different processor**, and the existing consent
does not cover it. Treating it as covered would be the exact move the consent record exists to
prevent.

So a destination requires its own grant, written the same way — purpose-scoped, policy-versioned,
withdrawable, and withdrawn by timestamp rather than deletion so "was there consent the day this was
exported?" stays answerable. An applicant who declines still applies; the employer reviews them in
PCI World rather than in its own ATS. **Declining must not be a hidden penalty**, and the interface
has to say so in words.

An applicant withdrawing this consent stops future exports. It cannot un-send what has already
reached the vendor, and the interface must say that plainly rather than implying a recall we cannot
perform.

---

## 4. Destinations are per employer, and inherit the verification gate

```
pciworld_ats_destinations(
  id, employer_id, vendor,          -- greenhouse|workday|lever|smartrecruiters|generic_webhook
  state,                            -- draft|sandbox|live|suspended|failed
  config_json,                      -- non-secret settings only
  credential_ref,                   -- opaque handle; the secret itself is never in this row
  certified_at, certified_by_admin_id,
  last_ok_at, last_error, consecutive_failures,
  created_at, updated_at, version)
```

Every export re-reads the employer through the Phase 4 predicate: an unverified or suspended
employer exports nothing. This is not a courtesy — a suspended employer is one we have stopped
trusting with applicant data, and an export path that kept running would hand that data to a system
we have even less control over than our own.

`version` is the optimistic-concurrency column, per the increment-wide rule. Datetimes are
`VARCHAR(32)`.

---

## 5. Credentials, and the callback direction

Vendor credentials are **write-only from every interface**: settable, never readable back, redacted
in `system-check`, in `/api/content`, in exports and in logs — the discipline the platform already
applies to SMTP and Stripe keys. `credential_ref` is a handle; the secret lives in the configured
secret store or an environment variable, never in a row that an admin list query could return.

Inbound callbacks (status changes coming back from the vendor) are **signed and verified against the
exact bytes received**, using the raw-body read the Stripe webhook already needs
(`H.RawString`) — parsing before verifying is how signature checks get quietly defeated. An
unverified callback is refused, logged, and changes nothing.

**No inbound callback may move an application to a terminal state on its own.** A vendor saying
"rejected" is information from a third party about their process, recorded as an event; the decision
of record still belongs to a person acting in PCI World, because the audit trail a candidate could
one day ask about has to be ours.

---

## 6. Delivery: durable, idempotent, and honest about failure

```
pciworld_ats_outbox(
  id, destination_id, application_id,
  idempotency_key,                  -- UNIQUE(destination_id, application_id)
  status,                           -- pending|claimed|delivered|failed_permanent
  attempts, next_attempt_at,
  lease_owner, lease_until,
  last_error, vendor_ref,
  created_at, updated_at)
```

`UNIQUE(destination_id, application_id)` — no `WHERE` clause, because `Db.Translate` strips
predicates from partial unique indexes on MySQL and a partial index would leave the two providers
agreeing only by accident. A retry after an ambiguous timeout re-sends the same idempotency key, so
the duplicate is the vendor's to collapse and ours not to create.

Backoff is exponential and bounded. A row that exhausts its attempts becomes `failed_permanent` and
**stays visible** — it is never deleted, because a silently dropped application is indistinguishable
from a candidate who never applied, and the employer would be reviewing an incomplete pile without
knowing it. The employer sees "3 applications could not be delivered to your ATS; they are here in
PCI World" rather than nothing.

---

## 7. The certification gate — what makes this phase honest

A destination moves `sandbox → live` only when a **recorded run against the vendor's own sandbox**
exists: a stored request/response pair, a timestamp, and the admin who reviewed it. No mock, fixture
or replay satisfies it, and the state machine has no path from `draft` or `sandbox` to `live` that
skips it.

This is the same shape as Phase 4's employer verification and Phase 1's moderation calibration: the
code enforces a gate whose evidence standard is defined outside the code. It is also the reason this
phase is registered as blocked rather than built — **there is nothing to certify against until
somebody holds sandbox credentials for a named vendor.**

---

## 8. Test plan — what must fail first

Written now so it constrains the implementation later. No test may weaken a threshold, an
authorization check or the certification gate to pass.

**Unit** — the destination state machine including every refused transition, and specifically that
no input reaches `live` without a certification record; idempotency-key derivation is stable across
retries and distinct across destinations; backoff is bounded; redaction of every credential-shaped
field in every serialiser that could emit one.

**Repository (both providers)** — idempotent `Ensure`; `UNIQUE(destination_id, application_id)` under
concurrency; lease claim/expiry/recovery under two workers; a `failed_permanent` row survives a purge.

**Integration / abuse** — an unverified or suspended employer exports nothing, checked on the export
path and not only at enqueue; an application whose destination consent is absent or withdrawn is
never enqueued; a vendor returning 500, then a timeout, then 200 results in **exactly one** delivered
row; an unsigned or wrongly-signed callback is refused and changes no state; a callback claiming
"hired" does not move the application's state of record; a credential never appears in any response
body, log line or export.

**What cannot be tested until credentials exist, stated rather than faked** — that our request shape
is one the vendor accepts, that its pagination and rate limits are what its documentation says, and
that its error bodies mean what we think. That is precisely the gap §28 refuses to let us paper over.

---

## 9. Blocked on decisions and access outside engineering

| # | Needed | Owner |
|---|---|---|
| CCP-P5-017 | **Sandbox credentials for at least one named vendor**, and the commercial/contractual relationship behind them. Nothing in this phase can be certified — or honestly called built — without this. | Commercial owner + Engineering |
| CCP-P5-018 | The data-protection position on onward transfer: PCI's role when application data moves to an employer's own processor, the wording of the per-destination consent, and whether any vendor's region is disallowed. | Data Protection Officer + legal counsel |
| CCP-P5-019 | Which vendors are in scope for release 1, and in what order. Each is separate validation surface; "all of them" is not a plan. | Commercial owner + Product |

---

## 10. What this phase does not do

- **No adapter is written against a mock and called done.** §28 forbids it, and the certification
  gate in §7 makes it structurally impossible to mark such a destination `live`.
- **No inbound candidate import.** Pulling candidates *from* a vendor into PCI World is a different
  consent problem — those people never agreed to anything with us — and needs its own design.
- **No two-way state synchronisation.** Callbacks are recorded as events; the decision of record
  stays in PCI World (§5).
- **No CV transformation or parsing** on the way out. The employer's ATS receives the document the
  applicant consented to disclose, not our paraphrase of it — the same rule as Phase 4 §6.
- **No change to the Phase 4 apply path.** Enqueueing an export is additive; if this whole phase is
  disabled, applications behave exactly as they do today.
