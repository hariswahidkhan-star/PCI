# Exam Delivery Integrations — Operator Guide

PCI can deliver its certification examinations through third-party exam-delivery / proctoring vendors
instead of (or alongside) the in-house SecureExam runner. Five vendors are supported out of the box:

| Vendor | Model PCI uses | Scheduling | Results |
|---|---|---|---|
| **Questionmark OnDemand** | Delivery OData (REST/JSON, HTTP Basic) | PCI schedules directly | PCI pulls (real-time `Results` feed) |
| **Kryterion Webassessor** | EWS JSON-RPC (`requestType` + `securityToken`) | PCI registers directly | PCI pulls (`Get Registrations`) |
| **PSI (Atlas / PSI Bridge)** | Eligibility Service (REST/OAuth2) | Candidate self-schedules on PSI | Vendor **push** → PCI callback |
| **Pearson VUE / OnVUE** | Real-time web services (SSO-RTI) | Candidate self-schedules via SSO | Vendor **push** (RTEN) → PCI callback |
| **TestReach** | Configured push integration | Candidate books in TestReach | Vendor **push** → PCI callback |

Admin Console → **Examinations → Exam delivery vendors** (permission: `exam_delivery`).

## The lifecycle

Every routed booking runs the same canonical pipeline; each connector maps it onto that vendor's real API:

```
student books an exam  →  order created (default vendor)
   → upsert candidate      (register the person)
   → authorize             (create the eligibility / authorization for the mapped exam code)
   → schedule              (book directly, or hand the candidate off to self-schedule)
   → (candidate sits the exam at the vendor)
   → results               (PCI pulls, or the vendor pushes to the callback)
   → PASS → PCI credential is issued automatically
```

State is stored per booking in `exam_delivery_orders`; every vendor API call is logged to
`exam_delivery_log` (visible under each booking's **Log**).

## Configuring a vendor

1. **Add vendor**, pick the vendor, and choose **Sandbox** or **Production**.
2. Fill the **Configuration** fields (per vendor — see below) and the **Credentials** (write-only).
3. **Certification → vendor exam code**: map each PCI certification to that vendor's exam / assessment
   code. A booking is only routed to the vendor when its certification is mapped.
4. Set a **Callback token** for the push vendors (PSI / Pearson VUE / TestReach) — the vendor must send
   it as `X-PCI-Callback-Token:` (or `?token=`) to `POST /api/exam-delivery/callback/<vendor>`.
5. Tick **Enabled**, and **Default** for the vendor that new bookings should route to.
6. Use **Test** to verify connectivity before enabling.

### Per-vendor configuration / credentials

- **Questionmark** — config: `customer_id` (area number), `region` (`us`/`eu`), `monitoring_type_id`;
  credentials: OData service-account `username` + `password` (Admin role or "Access Server Configuration").
- **Kryterion** — config: `rpc_path`, `delivery_type` (`WRAPPER_PROCTORED` / `PROCTORED` / …);
  credential: `security_token` (from your Customer Success Manager). Base host is
  `sb01.webassessor.com` (sandbox) / `www.webassessor.com` (production).
- **PSI** — config: `account_code`, `token_url`, `eligibility_days`; credentials: `consumer_key` +
  `consumer_secret` + `oauth_username` + `oauth_password` (OAuth2 password grant), or a preset
  `access_token`. Base URL is assigned per client via PSI's API Store → set it as **API base**.
- **Pearson VUE** — config: `client_code`, `candidate_path`, `eligibility_path`, `exam_series`,
  `eligibility_days`; credentials: `username`/`password` or `access_token`. Web-services base URL is
  provisioned via connect.pearsonvue.com → set it as **API base**.
- **TestReach** — config: `candidate_path`, `enrol_path`, `auth_header`; credential: `api_key`. Base URL
  is provisioned by TestReach → set it as **API base**.

Every connector accepts an **API base override**, so the whole pipeline can be exercised against a mock
or sandbox host before pointing at production.

## Driving a booking (admin)

Each routed booking (tab **Bookings**) supports:

- **Provision** — run candidate → authorize → schedule now (idempotent; resumes where it left off).
- **Sync** — pull the latest status + results; **issue the PCI credential on a pass** (idempotent — a
  re-sync or a duplicate callback never issues a second credential).
- **Cancel** — cancel the appointment / eligibility at the vendor.
- **Log** — the full per-order API call audit trail.

Candidates see their delivery status in the portal (`GET /api/me/exam/delivery`); for the
self-scheduling vendors the status `awaiting_candidate_schedule` cues the "book your slot with <vendor>"
step.

## Important: vendor onboarding is required before go-live

Each of these vendors gates the exact wire contract (endpoint hosts, authentication, message schemas,
sandbox URLs) behind a **signed agreement + NDA + a per-client sandbox certification**. The connectors
ship with each vendor's documented model and every host/path/credential is configurable, so once you
complete that vendor's onboarding you enter the provisioned base URL + credentials, run **Test**,
validate in **Sandbox**, then switch the environment to **Production**. The connectors never fabricate a
booking or a credential — an unconfigured or unauthorized vendor returns a clear error, and a pass is
only recorded when the vendor actually reports one.
