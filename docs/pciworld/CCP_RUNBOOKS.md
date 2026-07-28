# PCI World — emergency, legal-hold and escalation runbooks

**Status: DRAFT authored by engineering. Not yet approved for use.**
These close the *drafting* limb of `CCP-P1-003`. They do not close the item: a runbook is only a
control once a named person has approved it, the named roles are filled by trained people, and the
external arrangements they reference exist. Every place that depends on a decision engineering
cannot make is marked **[COUNSEL]** or **[T&S]** rather than filled with a plausible guess.

Scope: §19.3, §31, §15. Companion to `CCP_PHASE1_DESIGN.md` and `CCP_PHASE2_DESIGN.md`.

---

## 0. Roles

| Role | System permission | Filled by |
|---|---|---|
| Live moderator | `community.moderate` | **[T&S]** |
| Trust & Safety officer | `community.sanction` | **[T&S]** |
| Appeals reviewer | `community.appeal` | **[T&S]** — must not be the person who sanctioned |
| Safety lead | `community.restricted`, `community.restricted.approve` | **[T&S]** |
| Duty counsel | — (no system access) | **[COUNSEL]** |

Two-person rules are enforced in software, not by convention: the approver of a permanent sanction
cannot be its issuer, and the approver of restricted-evidence access cannot be its requester. Both
refusals are audited.

---

## RB-1 — Emergency: a room is being abused right now

**Trigger.** Coordinated abuse, a credible threat, or any situation where continuing to accept
messages is worse than stopping.

**Do this, in this order.**

1. **Lock the room** — World Admin → Community → the room → Emergency → `locked`.
   Use the *emergency* control, not the room state. Emergency state is a separate axis precisely so
   an incident does not overwrite the room's real schedule; the room you get back afterwards is the
   room you had.
2. **Do not delete anything.** Messages, sessions and decisions are the evidence. Locking stops the
   harm; deletion destroys the record of it.
3. **Open a case** if one is not already open. Severity `critical` for threats to a person, child
   safety, or suspected illegal material.
4. **If a person may be in danger** — contact the emergency service for their stated jurisdiction.
   **[COUNSEL]** must supply the per-jurisdiction contact list; this system holds no such list and
   must not pretend to. The participant's *declared* jurisdiction is on the session; it is a
   declaration, not a location.
5. **If it is child safety or suspected illegal media** — stop and go to **RB-3**. Do not view, copy,
   download, forward or screenshot the material.
6. **Record what you did** in the case timeline as you go, not afterwards.

**Global kill switch.** If more than one room is affected, or the moderation provider is failing:
World Admin → Community → Settings → disable. Rooms stop accepting; nothing is destroyed. Prefer
this to leaving rooms open while you work out what is happening — the system already fails toward
silence rather than toward publication, and the kill switch is the same instinct at product scale.

**Do not** turn off the moderation provider to "let messages through" while investigating. With no
provider the system publishes nothing, which is correct; a room that is quiet is a room that is safe.

---

## RB-2 — Legal hold

**Trigger.** Any of: a preservation request from law enforcement or a regulator; notice of actual or
anticipated litigation; a safeguarding referral; a data-protection complaint that names specific
content. **[COUNSEL]** decides what counts — this list is a prompt, not a definition.

**On receipt.**

1. **Do not reply substantively.** Acknowledge receipt and route to **[COUNSEL]** the same working
   day. Do not confirm or deny the existence of any account or content to the requester.
2. **Apply the hold before anything else.** Restricted evidence carries `legal_hold` from the moment
   it is created; for ordinary content the safety lead records the hold against the case.
3. **Verify the hold is real, not assumed.** The retention purge skips held material, and there is a
   test that fails if it does not — but confirm the specific items are covered rather than trusting
   the general rule.
4. **Suspend related deletions.** A participant's own delete hides content; it does not erase it, and
   that distinction is what makes a hold survivable. Do not "tidy up" a case under hold.
5. **Record** who applied the hold, when, on what, and on whose instruction.

**Releasing a hold** is **[COUNSEL]**'s decision and nobody else's. It is a deliberate, recorded act.
Nothing about a hold expires on a schedule; if `preserved_until` is set, it is a *floor*, not an
expiry.

**Never** delete or reset production data to resolve an incident. If you believe deletion is
required, that is a **[COUNSEL]** decision, taken in writing, recorded on the case.

---

## RB-3 — Suspected illegal material (child sexual abuse material or equivalent)

**This runbook is the one that must not be improvised.** It is deliberately restrictive.

**Do not, under any circumstances:** view the material to "check"; download, copy, forward, print or
screenshot it; attach it to an email, ticket or chat; describe it in a case note in any detail
beyond what is needed to route it; or discuss it outside the escalation path.

**What the system already does, without anyone acting.** A restricted verdict moves the original out
of the ordinary store, nulls the reference on the media record, produces **no renderable copy at
all**, and applies a legal hold. There is nothing to preview in any admin screen because nothing
exists to preview. The record you can see is text only: hash, size, dimensions, room, time, decision.

**What a human does.**

1. **Safety lead only.** Nobody else touches it. If you are not the safety lead, escalate and stop.
2. **Preserve** — already automatic; confirm the evidence record exists and carries `legal_hold`.
3. **Report to the specialist body** for the relevant jurisdiction, using the arrangement
   **[COUNSEL]** has concluded. **This arrangement does not yet exist and this system cannot
   substitute for it.** Until it does, escalate to **[COUNSEL]** directly and immediately.
4. **Access to the material, if the specialist body requires it**, is two-person: request with a
   stated reason, then approval by a *different* safety lead. The system refuses self-approval and
   audits the attempt. Approval records that access was authorised — retrieval itself happens out of
   band, under the specialist procedure. There is deliberately no download route in the product.
5. **Do not sanction the account on the classifier's say-so.** No image signal ends anybody's
   session automatically, by design. Any action against a person is a human decision, recorded.
6. **Support the reviewer.** See RB-4. This is not optional and not a formality.

**A general classifier does not detect illegal material.** It produces a signal that routes suspicion
to trained people. Nothing in this platform may be described, in a report or a summary or a sales
conversation, as detecting illegal images. That prohibition is §28.6 and it is absolute.

---

## RB-4 — Reviewer welfare

Moderation review is psychologically hazardous work, and a system that quietly relies on people
absorbing that harm is not a safe system. These are obligations on the organisation, not advice to
the reviewer.

**Built into the product**

- Restricted material has no preview anywhere, so no reviewer encounters it by accident while doing
  ordinary work.
- The ordinary queue is text-first; a reviewer chooses to open content rather than having it pushed
  at them.
- Restricted access requires a deliberate request and a second person's approval, so nobody is alone
  with the decision or the material.

**Obligations on the organisation — [T&S] to implement and evidence**

| Obligation | Why |
|---|---|
| A per-shift cap on restricted-material exposure | Cumulative exposure is the harm, not any single item |
| Rotation off the restricted queue | Nobody should be the permanent recipient of the worst content |
| Access to counselling, arranged before it is needed and not on request | Asking for help is the hardest step; remove it |
| A named person a reviewer can stop with, no reason required | A reviewer must be able to stop mid-task without justifying it |
| Onboarding before first restricted access, including what they will see | Consent to this work requires knowing what it is |
| No performance metric that rewards volume of restricted review | Rewarding throughput on this work is how people get hurt |

**Engineering note.** Exposure limits and rotation are enforceable in software and are **not yet
built**. They are recorded here as an obligation rather than implied to exist. Building them needs
**[T&S]** to set the numbers first — a cap invented by engineering would be a guess wearing the
authority of a control.

---

## RB-5 — Moderation provider outage

**Symptom.** Messages accepted, none published. Images stuck pending. This is the system working.

1. **Do not disable moderation to restore throughput.** With no usable verdict nothing publishes, and
   that is the correct behaviour, not the fault. Publishing unclassified content to "unblock" a room
   is the single worst action available during this incident.
2. Check World Admin → Community → Settings. `publishes_messages: false` means no provider is
   configured — a different problem from a provider that is failing.
3. If the outage is the vendor's, put the affected rooms in `read_only`. People can still read; the
   queue does not grow; nothing is lost.
4. Image scans retry with bounded backoff and settle to *withheld* rather than refusing permanently.
   Nothing was published at any point during the outage.
5. When the provider recovers, held items are re-processed. Do not bulk-approve to clear a backlog.

---

## RB-6 — Rolling back a moderation policy change

1. Policy versions are data. Re-activate the previous version; do not edit rows in place.
2. Decisions taken under the old version stay attributed to it. That is why every decision records
   `policy_version_id` — a decision nobody can interpret afterwards is not an audit trail.
3. An overturned decision writes a **new** decision. History is never rewritten.
4. If the change affected sanctions, notify appeals: someone may have been sanctioned under a rule
   that has since been withdrawn, and they should not have to discover that themselves.

---

## What these runbooks do not and cannot cover

- The per-jurisdiction emergency and specialist-reporting contacts. **[COUNSEL]**
- The threshold for what constitutes a preservation request. **[COUNSEL]**
- Exposure caps and rotation intervals. **[T&S]**
- Whether PCI is a mandated reporter in each jurisdiction it serves, and what that obliges. **[COUNSEL]**

Each of these is a decision with legal consequences. Engineering has written the procedure around
them and left the values blank on purpose: a plausible-looking number in a runbook is worse than an
obvious gap, because it stops anybody asking.
