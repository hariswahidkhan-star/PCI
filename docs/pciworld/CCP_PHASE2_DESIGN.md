# CCP Phase 2 — image safety pipeline (design)

**Status: design only. The feature ships behind `pciworld_community_images_enabled`, default OFF,
and the Phase 2 exit gate CANNOT be signed off from this repository.** Two prerequisites are owned
outside engineering and are still open: `CCP-P1-003` (legal/child-safety prerequisites) and
`CCP-P1-004` (moderation provider and calibrated bands). This document specifies what gets built so
that the controls already exist the day those decisions land — it does not claim they are met.

Scope: §8.5 (image pipeline), §19.3 (legal prerequisites), §31 (restricted evidence), and the
prohibitions in §28.6 / §28.7. Phase 1 (guest text rooms) is the substrate; nothing here changes a
Phase 1 guarantee.

---

## 1. The one rule this phase exists to keep

**No image byte reaches a second participant before the server has recorded an allow verdict.**

This is the image restatement of Phase 1's §28.4 rule for text, and it drives every structural
choice below: the upload endpoint returns a *pending* handle, never a URL; the media row is created
`pending` with no room sequence; and the message that carries it is not allocated a sequence — and
therefore cannot be replayed by `since()` — until the verdict is `allowed`. There is no code path
that publishes an image and then scans it.

The corollary is equally load-bearing: an image the pipeline *cannot* classify is not published.
A provider timeout, a decode failure, an unknown format and a missing provider all resolve to the
same outcome as an explicit refusal. `NullMediaScanner` — the default when nothing is configured —
returns "cannot classify" and the policy engine resolves that to no publication, exactly as
`NullModerator` does for text today. Fail-closed is the default, not a configuration.

---

## 2. What must never happen (§28.6, §28.7)

These are not aspirations; each maps to a specific structural control below.

| Prohibition | Control |
|---|---|
| Never broadcast guest images before the allow verdict | §1; no sequence until allowed (§4.3) |
| Never claim a general classifier detects every illegal image | §6.3 — the wording is in the schema and the admin UI, not only in prose |
| Never expose suspected illegal media in ordinary admin thumbnails | §5 restricted evidence store; the ordinary media table stores **no renderable derivative** for restricted items |
| Never turn a low-confidence result into an irreversible sanction | §6.2 — restricted band routes to human review, never to auto-eject |
| Never delete production data | §7 — a legal hold blocks purge; retention never overrides a hold |
| Never expose raw IPs or private evidence | Risk keys stay peppered hashes (Phase 1 §2.2); evidence access is logged per-view |

---

## 3. Feature flag and the gate it stands in for

```
pciworld_community_images_enabled   -- site setting, default '0'
```

Every image write path checks it, and so does `pciworld_community_rooms.image_allowed` (the column
already exists from Phase 1 and is currently rejected by every write path). Both must be true. The
site setting is the kill switch; the room column is the per-room grant.

The flag is **not** the legal gate. Turning it on is a decision a human makes after `CCP-P1-003`
resolves; the code cannot and must not assert that the prerequisites are met. The admin settings
screen therefore renders the outstanding prerequisites as a checklist with an explicit
"not verified by this system" note rather than a green tick.

---

## 4. Data model

All tables carry the `pciworld_` prefix (D-002) and are declared in `Data/CommunitySchema.cs` with
the same idempotent `CREATE TABLE IF NOT EXISTS` + guarded `AddCol` discipline as Phase 1, so an
existing database converges without a manual migration. SQLite dialect is the source of truth;
`schema.mysql.sql` is regenerated with `tools/sqlite_to_mysql.py`.

### 4.1 `pciworld_community_media`

```
pciworld_community_media(
  id, room_id, session_id, message_id NULL,
  client_upload_id,          -- idempotency: UNIQUE(session_id, client_upload_id)
  state,                     -- pending|scanning|allowed|withheld|restricted|expired
  declared_mime, sniffed_mime, byte_size, width, height,
  content_sha256 VARCHAR(64),
  perceptual_hash VARCHAR(64) NULL,
  storage_ref NULL,          -- ORIGINAL bytes; NULL once moved to restricted evidence
  derivative_ref NULL,       -- re-encoded, EXIF-stripped render copy; NULL unless state='allowed'
  scan_provider, scan_provider_version, scan_band, scan_confidence,
  policy_version_id, verdict_at, created_at, updated_at, version)
```

Two separate references matter. `storage_ref` is the bytes as uploaded. `derivative_ref` is the
sanitised copy that is the *only* thing ever served to a participant. A row in any state other than
`allowed` has `derivative_ref IS NULL`, which makes "served before allowed" a schema impossibility
rather than a code convention.

`content_sha256` gives exact-duplicate suppression and links a re-upload of known-bad bytes to the
prior decision without re-scanning. `perceptual_hash` supports near-duplicate matching for evasion
(a one-pixel edit); it is a *signal*, never an automatic sanction (§28.5).

### 4.2 `pciworld_media_scans`

Append-only, one row per scan attempt — including failures and timeouts, which are the rows that
matter when explaining why something was withheld.

```
pciworld_media_scans(
  id, media_id, provider, provider_version, requested_at, completed_at,
  outcome,                   -- allowed|refused|restricted|error|timeout|unsupported
  band, confidence, raw_labels_json, error_text, created_at)
```

Never edited. A re-scan writes a new row, mirroring Phase 1's append-only decision history.

### 4.3 Message linkage

An image message reuses `pciworld_community_messages` with `message_kind='image'` and
`media_id`. **Sequence allocation is unchanged and remains the publication boundary**: the message
is inserted with `sequence IS NULL` and only receives a sequence — inside the same transaction that
writes the allow verdict and the outbox row — when the media state becomes `allowed`. `since()`
already filters on `sequence IS NOT NULL`, so a pending or withheld image is invisible to every
reader without a single new query predicate. This is deliberate: the safest change to a publication
path is no change at all.

---

## 5. The restricted evidence store (§31)

Suspected illegal media leaves the ordinary pipeline entirely.

- The original bytes move to a **separate storage root** with its own prefix, never under the
  attachment or evidence categories `Core/Storage` already serves, and `storage_ref` on the media
  row is nulled so no ordinary handler can resolve it.
- **No derivative is ever generated.** There is nothing to thumbnail, because the system never
  produces a renderable copy. §28.7 is enforced by absence, not by a UI flag that a future
  refactor could drop.
- The moderation console shows a **text-only record** — hash, size, dimensions, room, time,
  decision, case id — and a disabled control explaining that access requires the escalation
  procedure. The queue count is visible so the work is not hidden; the content is not.
- Access is two-person: a `safety_lead` request plus a distinct `trust_safety` approval, recorded
  as an append-only case event. Approver ≠ requester is enforced server-side, the same maker-checker
  rule Phase 1 applies to sanctions.
- Every access attempt — granted or refused — writes an audit row.

```
pciworld_restricted_evidence(
  id, media_id, case_id, storage_ref, content_sha256,
  reason, preserved_until NULL, legal_hold INTEGER NOT NULL DEFAULT 0,
  requested_by NULL, approved_by NULL, accessed_count, last_accessed_at,
  created_at, updated_at, version)
```

**What this is not.** A general image classifier is not a specialist illegal-media detection
service, and this design does not pretend otherwise. The restricted store is the *container and
escalation path* for suspected material so that a specialist arrangement (hash-matching against an
authorised list, reporting, preservation) can be connected when `CCP-P1-003` resolves. Until then
the system routes suspicion to trained humans and preserves; it does not detect, and the admin UI
says so in those words.

---

## 6. Pipeline stages

### 6.1 Accept (synchronous, cheap, fail-closed)

In order, each rejecting outright:

1. Flag + `room.image_allowed` + room state accepts messages.
2. Session active, not ejected, not restricted; slow-mode and per-session caps apply as for text.
3. Size cap (**2 MB**, well under the 6 MB Kestrel body cap) and declared MIME in the allowlist.
4. **Content sniff** — magic bytes must agree with the declared type. Disagreement is a rejection,
   not a correction.
5. Decode to raster with hard **dimension and pixel-count caps**, which is what stops a
   decompression bomb: a 20 KB PNG that expands to 40 000 × 40 000.
6. **Re-encode** from the decoded raster. This is the single most valuable control in the list: it
   drops EXIF (including GPS — a guest room must not leak where someone is), strips trailing
   payloads, and neutralises polyglot files, because the output is bytes this server produced.

Formats: PNG, JPEG, WebP. **No SVG** — SVG is a script-bearing document, not an image, and no
sanitiser is worth the standing XSS risk in a guest surface. **No animation** in the first release;
a frame budget is a separate scanning problem and is deferred honestly rather than half-done.

### 6.2 Classify (asynchronous, ordered, resumable)

The scan runs on the existing outbox/worker substrate — `WorkerLease.TryClaim` with expiry
recovery, the same mechanism Phase 1 uses for broadcast — so a crashed worker's item is re-claimed
rather than stranded. Retries use bounded backoff via `H.StampInMinutes` (never a literal SQL
datetime modifier: `Db.Translate` only rewrites literals, and that bug has already been paid for
twice in this increment).

```csharp
public interface ICommunityMediaScanner {
    Task<MediaSignal> ScanAsync(MediaSubject subject, CancellationToken ct);
    string ProviderName { get; }
    string ProviderVersion { get; }
}
```

Bands map through the versioned policy matrix, as text does:

| Band | Outcome |
|---|---|
| `clear` | allow → derivative generated → sequence allocated → broadcast |
| `low` | allow, flagged for sampled review |
| `medium` | **quarantine** — withheld from the room, queued for human review |
| `high` | withheld + case opened; sanction only by human decision |
| `restricted` | withheld + restricted evidence + escalation; **no automatic sanction** |
| `error` / `timeout` / `unsupported` | withheld (fail-closed), queued, retried |

`NullMediaScanner` returns `unsupported`. `DeterministicMediaScanner` maps fixture hashes to bands
so every branch — including `restricted` — is testable end to end without a vendor and without any
real harmful material.

### 6.3 Uncalibrated until a provider is contracted

Per §8.4.1 and `CCP-P1-004`, a raw score from one provider does not mean the same thing as
another's. Until a provider is contracted and a PCI benchmark corpus exists, the bands are
**uncalibrated**, the admin UI must display that plainly next to any confidence figure, and no band
may drive an irreversible sanction. Auto-ejection on an image signal alone is not implemented in
this phase — that is a deliberate omission, not an oversight.

---

## 7. Retention, holds and deletion (§28.1)

- Ordinary allowed media follows the room's `retention_class`; the derivative is purged with the
  message, the original earlier.
- Withheld media is retained long enough to serve the appeal window, then purged.
- **A legal hold blocks every purge path.** `RetentionService` must consult
  `pciworld_restricted_evidence.legal_hold` and `preserved_until` and skip held rows, and the skip
  is asserted by a test that fails if the hold is ignored. "Never delete or reset production data"
  is enforced by a check, not by a convention.
- A participant deleting their own message hides it from the room; it does not destroy evidence
  attached to an open case or a hold. The distinction is recorded in the case history so a reviewer
  can see what happened.

---

## 8. Test plan — what must fail first

Written before the implementation, and none may be weakened to pass (§28.3):

**Unit** — sniff-vs-declared mismatch rejected; decompression bomb rejected by pixel budget; EXIF
(incl. GPS) absent from every derivative; SVG rejected; band→outcome mapping for every row of §6.2;
`NullMediaScanner` yields no publication.

**Repository (real MySQL, not only SQLite)** — idempotent re-`Ensure`; `UNIQUE(session_id,
client_upload_id)` upload idempotency; `derivative_ref IS NULL` for every non-`allowed` state;
restricted rows carry no `storage_ref` in the ordinary root.

**Integration / abuse** — the pipeline's whole point:
- a second client polling `since()` throughout an upload+scan **never** observes the image before
  the allow verdict (the image analogue of E2E-014);
- a worker crash mid-scan, a duplicate scan delivery and an out-of-order completion converge on
  exactly one outcome and one sequence (E2E-028);
- a restricted verdict produces **no** renderable derivative anywhere, and the admin media list
  returns no image bytes for it;
- evidence access without the second approval is refused and audited;
- retention purge with a legal hold in effect deletes nothing.

**Accessibility** — image messages keep the Phase 1 guarantees: alternative text is required before
send, the pending/withheld state is conveyed in text and not by colour, and the transcript's live
region still announces without stealing focus.

---

## 9. Honest status

- Built and testable: the pipeline, the restricted store, the escalation path, the holds.
- **Not** satisfied by this repository: minimum age and jurisdictions, grooming/enticement detection
  and trained escalation, specialist illegal-media detection and reporting arrangements, emergency
  and legal-hold runbooks, reviewer training and welfare protection (`CCP-P1-003`).
- **Not** satisfied: calibrated confidence bands and a benchmark corpus (`CCP-P1-004`).

The Phase 2 exit gate stays open until those are signed off by their owners. The flag stays off.
