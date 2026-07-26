# PCI World CCP — Phase 1 Design: Text-Only Guest Community Rooms

_Specification for the Phase 1 vertical slice. Written before implementation, per §26 step 5._

Prerequisite: `CCP_PHASE0_BASELINE.md` (audit), `CCP_DECISION_LOG.md` (D-001…D-010).

**Slice boundary (§33):** public room list → validated guest entry → server-issued guest session →
SignalR join → pre-broadcast text moderation → durable MySQL message + outbox → ordered broadcast →
report / eject / appeal → World Admin moderation evidence → automated E2E and abuse tests.

**Out of this slice:** images (Phase 2), forum (Phase 3), employers/jobs (Phase 4), contributor
publishing (Phase 6). No guest DMs, voice, video, links, or contact exchange in any release-1 phase.

---

## 1. Launch gating — build now, launch later

Phase 1 ships behind `world_community_enabled`, **default off**, because two external decisions are
unresolved (`CCP-P1-003` legal, `CCP-P1-004` moderation provider). The flag is not a placeholder:

- With the flag off, `/world/community*` returns the standard World 404 and the hub refuses upgrade.
- The moderation **provider** is an adapter (`ICommunityTextModerator`); the **policy** is versioned
  data. When a provider is contracted and counsel's decisions land, both are configuration, not code.
- `Settings.Bool(db, "world_community_enabled", false)` follows the existing `world_enabled` pattern.

**Fail-closed is structural, not conditional.** If no moderator is configured, the effective policy
is "no guest text may be published" — the room opens read-only. There is no code path where an
absent or erroring moderator results in a published message (§15, §28.4).

---

## 2. Schema — 11 tables, all `pciworld_`-prefixed (D-004)

Authored in SQLite dialect per repo convention; `Db.Translate` rewrites for MySQL. Installed by an
idempotent `CommunitySchema.Ensure(db)` called from `WorldSchema.Ensure`, matching the
`MarketingSchema`/`SimLabSchema` pattern. Datetimes are `YYYY-MM-DD HH:MM:SS` UTC strings (§3.3 of
the platform guide) so `H.JsMillis`/`H.IsPast`/`H.After` behave identically on both providers.

| Spec name (§13.1) | Implemented as |
|---|---|
| `world_community_rooms` | `pciworld_community_rooms` |
| `world_guest_sessions` | `pciworld_guest_sessions` |
| `world_community_messages` | `pciworld_community_messages` |
| `world_moderation_decisions` | `pciworld_moderation_decisions` |
| `world_moderation_cases` | `pciworld_moderation_cases` |
| `world_moderation_case_events` | `pciworld_moderation_case_events` |
| `world_reports` | `pciworld_community_reports` |
| `world_sanctions` | `pciworld_sanctions` |
| `world_appeals` | `pciworld_appeals` |
| `world_policy_versions` + `world_policy_rules` | `pciworld_policy_versions`, `pciworld_policy_rules` |
| `world_community_outbox` | `pciworld_community_outbox` |
| `world_risk_restrictions` | `pciworld_risk_restrictions` |

Deferred to later phases: `world_community_room_memberships` (no membership concept in a guest
text room), `world_community_media` + `world_community_message_revisions` (Phase 2).

### 2.1 Rooms

```
pciworld_community_rooms(
  id, slug UNIQUE, title, description, topic, category, locale,
  room_type,           -- community | peer_support | official_support
  state,               -- draft|scheduled|open|slow_mode|read_only|closed|archived
  emergency_state,     -- NULL|degraded|locked|globally_disabled
  capacity, guest_allowed, member_allowed, text_allowed, image_allowed,
  slow_mode_seconds, message_cooldown_ms, max_messages_per_session,
  opens_at, closes_at, timezone,
  pinned_welcome, learning_prompt, resources_json,
  retention_class, discoverable, rules_version, policy_version_id,
  next_sequence,       -- monotonic per-room counter; see §4.2
  created_at, updated_at, version)
```

`version` is the optimistic-concurrency column §13.6 requires for mutable admin records.
`image_allowed` exists now but every write path rejects it while Phase 2 is ungated — the column
avoids a later migration, it does not enable the feature.

### 2.2 Guest sessions

```
pciworld_guest_sessions(
  id, token_sha VARCHAR(64) UNIQUE, room_id,
  display_name, display_name_folded,   -- folded = normalized form, for duplicate/confusable checks
  locale, rules_version_accepted,
  risk_key VARCHAR(64),                -- rotating peppered hash; never a raw IP
  status,                              -- active|left|expired|ejected
  message_count, last_message_at, last_seen_sequence,
  created_at, expires_at, ended_at, ended_reason)
```

`UNIQUE(room_id, display_name_folded) WHERE status='active'` — a partial unique index, which
`Db.Translate` already handles for MySQL. This enforces PW-US-018's "no duplicate active
participant name" in the database rather than in a racy read-then-write.

**Privacy (§7.1, §19.2).** `risk_key = SHA256(pepper || client_ip || rotation_period)`. The pepper
comes from config, never source. Raw IP is never stored, never logged, never selected back — the
existing `Forum.cs`/`Chat.cs` salted-hash precedent, with rotation added so the identifier expires.
Guest-ban evasion via cookie deletion or a proxy change remains possible; §28 forbids claiming
otherwise, so the admin UI labels this "layered deterrent", not "ban enforcement".

### 2.3 Messages, decisions, outbox

```
pciworld_community_messages(
  id, room_id, sequence, guest_session_id, world_user_id,
  client_message_id,                   -- idempotency key from the client
  body, body_normalized,
  reply_to_message_id,
  status,                              -- pending|allowed|blocked|quarantined
  decision_id, created_at, published_at)
```

`UNIQUE(room_id, sequence)` and `UNIQUE(room_id, client_message_id)` — the first guarantees ordered
recovery, the second makes double-click and network retry idempotent (§13.6).

```
pciworld_moderation_decisions(
  id, scope,                           -- message|display_name|report|image(P2)
  subject_ref, content_hash,
  policy_version_id, rule_id, provider, provider_version,
  category, severity, confidence_band, -- low|medium|high, calibrated per D-008
  context_rule, repetition_count,
  outcome,                             -- allow|block|quarantine|eject|escalate
  reason_code, correlation_id, created_at)
```

**Decisions are append-only and carry no raw content** — only `content_hash` plus the metadata
§8.7 lists. Blocked and quarantined bodies live in the restricted evidence store, never in audit
rows or telemetry (§8.7, §28.2). An overturned appeal writes a *new* decision and updates derived
sanction state; it never rewrites decision history (§8.7 final paragraph).

```
pciworld_community_outbox(
  id, event_type, room_id, payload_json,
  status,                              -- queued|processing|sent|failed
  attempts, next_attempt_at, lease_owner, lease_until,
  last_error, correlation_id, created_at, sent_at)
```

Column shape deliberately mirrors `comm_outbox` so `Core/WorkerLease.cs` claims rows unchanged and
`OutboxDispatcher`'s drain loop is the template (§2.8 of the baseline).

### 2.4 Reports, sanctions, appeals, policy, risk

```
pciworld_community_reports(id, room_id, message_id, reported_session_id,
  reporter_session_id, reporter_user_id, reason_code, note, status,
  case_id, created_at)
  -- UNIQUE(message_id, reporter_session_id): one report per reporter per message (PW-US-021)

pciworld_sanctions(id, subject_kind, subject_ref, room_id, sanction_type,
  reason_code, policy_version_id, scope, issued_by, issued_by_kind,
  approved_by, starts_at, expires_at, revoked_at, revoked_by, case_id,
  correlation_id, created_at)

pciworld_appeals(id, case_id, public_reference UNIQUE, credential_sha VARCHAR(64),
  credential_expires_at, attempts, status, submission, outcome,
  reviewed_by, reviewed_at, created_at)

pciworld_policy_versions(id, version_label UNIQUE, status, notes,
  created_by, approved_by, activated_at, retired_at, created_at)
pciworld_policy_rules(id, policy_version_id, content_type, category, severity,
  confidence_band, context_rule, repetition_min, outcome, reason_code, sort)

pciworld_risk_restrictions(id, risk_key, scope, room_id, reason_code,
  case_id, expires_at, created_at)
```

**Appeal credentials (§8.6).** `public_reference` is non-sensitive and safe to quote in support
mail; on its own it reveals nothing — no status, no content, no personal data. The appeal
*credential* is a separate high-entropy value shown once, stored only as SHA-256, scoped to that
case, expiring, rotatable, and rate-limited via `attempts`. This is the same opaque-token discipline
`pciworld_users.passport_token_sha` already uses.

---

## 3. State machines

**Room.** `draft → scheduled → open → slow_mode|read_only → closed → archived`, with the orthogonal
emergency axis `degraded → locked → globally_disabled`. Emergency state is separate from `state` so
a kill switch never destroys the room's real schedule.

**Guest session.** `active → left | expired | ejected`. Ejection is terminal for that session; it
also writes a `pciworld_risk_restrictions` row so a fresh session from the same risk key is refused
for the sanction's duration.

**Message.** `received → normalized → schema/size check → policy rules → classifier → allowed |
blocked | quarantined | ejected | escalated` (§8.4). Only `allowed` ever reaches `published_at` and
the outbox. **A row is inserted `pending` and transitions in the same transaction as its decision**
— there is no window where a message exists without a verdict.

**Appeal.** `submitted → under_review → upheld | overturned | expired`. An overturned appeal revokes
the sanction, writes a new decision, restores access, and corrects derived metrics.

---

## 4. Real-time transport

### 4.1 Hub

`/api/world/hubs/community` — already inside `WorldOnly.Allowed()` (baseline §2.1), so **no
allowlist change**. Requires `AddSignalR()`, which is net-new to `backend/` (baseline §2.2); the
in-house reference is `secureexam/PCI.SecureExam.Server/Hubs/ProctorHub.cs`.

Server-authoritative checks on **every** invocation, not just at connect: guest ticket validity,
room membership, room state, sanction state, slow-mode cooldown. Origin is validated against the
explicit CORS allowlist. Payloads, buffers, history and connection lifetime are all bounded. Hub
exceptions are never surfaced in detail. Tokens, guest cookies, raw IPs, and message bodies are
never logged (§8.3).

### 4.2 Ordering and recovery — MySQL is authoritative, not the connection

Per room, `next_sequence` is allocated **inside the accept transaction** by a conditional
`UPDATE … SET next_sequence = next_sequence + 1` returning the prior value, using the same
rows-affected discipline as `WorkerLease.TryClaim`. Two concurrent accepts cannot receive the same
sequence, and a sequence is never allocated for a message that was not accepted.

One transaction commits: the message row (`allowed`), its decision row, and the outbox event.
Broadcast happens **from the outbox**, after commit — never inside the transaction (§28.18). A
client reconnects with `lastSeenSequence` and replays the gap from MySQL, so a dropped broadcast,
a worker crash after commit, a duplicate delivery, or a node restart all converge on one ordered
history (§15, PW-US-036, PW-US-044).

Scale-out remains undecided (`CCP-P1-005`), and this design is deliberately indifferent to it:
because MySQL plus the outbox are authoritative, sticky sessions, a Redis backplane, or Azure
SignalR are all viable later without touching the durability model.

---

## 5. Guest entry and display-name validation

One friction-light screen (§7.1, PW-US-017): display name, concise age/jurisdiction notice, rules
acceptance. No account, email, password, PCI login, or Passport. Passive risk assessment by default;
a visible challenge appears only when risk or provider policy demands it.

Name validation order — each step is a pure function, unit-testable without a database:

1. **Unicode normalize** to NFKC, strip zero-width (`U+200B`–`U+200F`, `U+2060`, `U+FEFF`) and
   bidi-control characters.
2. **Fold** for comparison: casefold, collapse whitespace, map confusable homoglyphs (Cyrillic
   `а`→`a`, Greek `ο`→`o`, fullwidth forms) to a skeleton.
3. **Preserve legitimate scripts.** Arabic and Urdu names must survive normalization intact —
   PW-US-019 is explicit that folding must not destroy them. The *fold* is used only for comparison;
   the *display* value keeps the participant's original characters.
4. **Reject**: reserved staff/PCI/moderator/admin terms and their confusable skeletons; impersonation
   patterns; profanity; contact details (phone, email, social handles, URLs); misleading badge words
   ("verified", "official", "staff").
5. **Duplicate check** against `display_name_folded` for active sessions in that room, enforced by
   the partial unique index so a race cannot produce two.
6. On rejection, return a **specific, actionable** reason code with a suggested alternative
   (PW-US-020) — never a bare "invalid".

---

## 6. Moderation policy as data (§8.4.1)

The §8.4.1 matrix is seeded into `pciworld_policy_rules`, **not** scattered through `if` statements.
A decision resolves `content type × category × severity × confidence band × context × repetition →
outcome`, and records which rule row produced it.

```csharp
public interface ICommunityTextModerator {
    Task<ModerationSignal> ClassifyAsync(string normalizedText, string locale, CancellationToken ct);
    string ProviderName { get; }
    string ProviderVersion { get; }
}
```

Two implementations ship in Phase 1:

- **`NullModerator`** — the default when nothing is configured. Returns "cannot classify", which the
  policy engine resolves to *no publication*. This is the fail-closed default, not a bypass.
- **`DeterministicModerator`** — rule/pattern based, used for unit and E2E fixtures so the state
  machine, ejection, quarantine and appeal paths are all testable without a vendor.

A real provider is a third implementation added in Phase 2/3 once `CCP-P1-004` resolves. **Confidence
bands stay uncalibrated until then**, and the admin UI must say so — §8.4.1 warns that a raw score
from one provider does not equal another's, and §28.5 forbids turning a low-confidence result into
an irreversible sanction.

---

## 7. Test plan (§23) — what must fail first

Unit (xUnit, no DB): name normalization incl. Arabic/Urdu preservation, confusable folding,
zero-width stripping, reserved-name and contact-detail rejection; policy matrix resolution for every
row of the §8.4.1 table; sequence allocation under concurrency; appeal-credential hashing and expiry.

Repository/migration (real MySQL): idempotent re-`Ensure`, partial unique indexes, `UNIQUE(room_id,
sequence)` and `UNIQUE(room_id, client_message_id)` enforcement, optimistic-concurrency conflicts.

API + hub integration: guest entry validation; forged/expired/replayed guest tickets; cross-origin
upgrade attempts; unauthorized room join; message flood, oversized and malformed payloads; slow-mode
and capacity limits.

The named abuse journeys this slice must satisfy: **E2E-009 … E2E-022, E2E-027, E2E-028**. The two
that matter most, because they are the whole point of the slice:

- **E2E-014** — no second client can observe a message before the final allow verdict.
- **E2E-028** — worker crash after commit, duplicate outbox delivery, out-of-order event, and node
  restart still converge on exactly one ordered message history.

**No test may weaken a threshold, an authorization check, or a validation rule to pass (§28.3).**

---

## 8. Known limits stated honestly

- Guest bans are evadable by cookie deletion and proxy change. Layered controls only (§7.1).
- Confidence bands are uncalibrated until a provider is contracted (`CCP-P1-004`).
- Rooms cannot launch publicly until counsel's decisions land (`CCP-P1-003`).
- Scale-out topology is undecided (`CCP-P1-005`); durability does not depend on it.
- No specialist illegal-media control exists in this slice — it is a Phase 2 prerequisite, and
  §28.6 forbids implying a text classifier or a general image classifier substitutes for one.
