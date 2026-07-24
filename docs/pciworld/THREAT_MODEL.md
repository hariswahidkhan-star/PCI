# PCI World — Threat Model (Phase 0/1 scope)

Assets: challenge content (pre-publication answers/rubrics), attempt data, anonymous session
continuity, public result integrity, admin credentials, the Institute's reputation.

| Threat | Vector | Mitigation in this slice |
|---|---|---|
| Answer leakage before submit | Public API/HTML exposing solver outputs or rubrics | Public payloads are built from an explicit allow-list of fields (`WorldContent.PublicView`); reference values are computed only at grade time and never serialized to the workspace; tests assert the workspace payload contains no `quality`/reference values |
| IDOR on attempts | Guessing attempt ids | Every attempt read/write requires the owning session token (sha match); ids alone grant nothing; tests cover cross-session access |
| Result-token enumeration | Scanning `/world/r/{token}` | 128-bit `RandomHex(32)` tokens, stored as SHA-256 only, revocable, rate-limited lookups, 404 on miss (no distinction between absent/revoked) |
| Result manipulation | Re-submitting, tampering with score | First-submit-wins idempotent transition (`status='in_progress'` guard); score computed server-side from stored answers; public page renders from stored result only |
| Admin privilege escalation | Author self-approving/publishing | Server-side RBAC per endpoint; maker-checker enforced in SQL (`approved_by` must differ from `author_id`); publish requires `approved` state and a passing validator run |
| Cross-realm escalation | PCI admin token used on world-admin (or vice versa) | Separate `pciworld_admin_sessions` table; tokens verified only against their own realm; tests assert PCI admin tokens are rejected |
| Brute force on admin login | Credential stuffing | bcrypt, per-account lockout (`LoginGuard`), timing-equalised unknown-account path, sha-stored session tokens with expiry |
| Injection (SQL/XSS) | Participant answers, admin-authored content | Parameterized queries throughout; all participant/author text is HTML-encoded at render (`WebUtility.HtmlEncode`); no raw HTML from content fields |
| Invitation spam/abuse | Mass invite minting | Per-session rate limit on invite/share minting; invitations revocable; invitation pages never expose the inviter's answers |
| Bot completion / fake virality | Scripted submits | Per-session and per-IP throttles on start/submit; no public leaderboards in this slice, so no ranking incentive exists yet; ranking thresholds + anti-fraud are a documented gate before any ranking ships |
| Prompt injection via Coach | Malicious scenario text | Coach is **out of this slice**; the gate list for its later inclusion (grounding, leakage, injection, Arabic, cost) is in PLAN.md |
| Fake certification claims | Copy drift | Fixed disclosure strings rendered from one constant; result/verification pages carry the practice-not-certification notice; no PCI World table references credential tables |
| Secrets/config leakage | Hard-coded URLs/keys | Institute URL + flags are `site_settings`; no new secrets introduced |
| Mass assignment | Over-posting on admin edit | Admin edit maps named fields explicitly; lifecycle fields (status, review_state, approved_by, version) are only writable via their dedicated endpoints |
| DoS on public pages | Hot loops on HTML/API | Cheap queries (indexed lookups), per-IP throttle on write endpoints, no unbounded catalogue dumps (paged archive) |

Residual risks (accepted for this slice, tracked in PLAN.md): no MFA yet on world-admin (backlog
before production), no CAPTCHA (introduce only on abuse signals), share-card PNG generation
deferred (OG tags only), email flows deferred (no email attack surface yet).
