# PCI World — Data Model & Content Model (Phase 0/1)

All tables are MySQL 8-compatible via the platform's `Db` dialect translation and are installed
idempotently by `WorldSchema.Ensure` on both providers (same pattern as the rest of the
platform, so provider-parity gates apply automatically). UTC timestamps throughout. No foreign
keys into exam/credential/user tables — PCI World is a separate realm by construction.

## Tables (this slice)

| Table | Purpose / key columns |
|---|---|
| `pciworld_challenges` | Authoring row: `code` UNIQUE, metadata (title, hook, industry, role, track, difficulty, minutes, competencies_json), `config_json` (draft working copy), `status` draft→in_review→approved→published→retired, `author_id`, `reviewed_by`, `approved_by`, `current_version`, timestamps |
| `pciworld_challenge_versions` | **Immutable** published snapshots: `(challenge_id, version)` UNIQUE, full `config_json` + display metadata frozen at publish; never updated, never deleted |
| `pciworld_calendar` | `day_utc` UNIQUE → `challenge_id`; overrides the deterministic day-of-year rotation |
| `pciworld_sessions` | Anonymous participant continuity: `token_sha` UNIQUE, created/last-seen |
| `pciworld_attempts` | `session_id`, `challenge_id`, `version` (pinned), `status` in_progress→completed, `answers_json`, `score`, `dimensions_json`, `profile_key`, `display_name` (participant-chosen, nullable), `result_token_sha` (nullable until shared), `result_revoked`, timestamps |
| `pciworld_invites` | `attempt_id` (inviter), `token_sha` UNIQUE, `inviter_name` (nullable = anonymous), `revoked`, `created_at` |
| `pciworld_admin_users` | Separate realm: email UNIQUE, name, `role` (owner/author/reviewer/publisher/viewer), bcrypt `password_hash`, status, `failed_logins`, `lockout_until` |
| `pciworld_admin_sessions` | `admin_id`, `token` (sha), `expires_at` |
| `pciworld_audit` | Append-only: `admin_id`, `action`, `detail`, `created_at` |
| `pciworld_events` | Privacy-aware analytics: `event`, optional `challenge_id`/`session_id`, `created_at` (no IP, no UA, no email) |

Deferred to later phases (designed, not yet created): participant accounts + passports
(`pciworld_users`, `pciworld_passports`, `pciworld_passport_evidence`), localizations, share
assets, universities/cohorts, employers/missions, ranking snapshots, content reports,
communications, feature-flag table (slice uses `site_settings` `world_*` keys), idempotency keys
(slice endpoints are idempotent by state-guarded SQL instead).

## Replay invariant

Attempts pin `(challenge_id, version)` at start and replay/grade **only** from
`pciworld_challenge_versions`. The live authoring row is never the replay authority. Revision
publishes a new version; old attempts are untouched. This is the Simulation Lab P0 lesson applied
from day one.

## Challenge content model (`config_json`)

```jsonc
{
  "context":  "...",                  // project situation shown before start (no answers)
  "evidence": [ {"label": "...", "value": "..."} ],   // the evidence pack shown in the workspace
  "task": "evm",                      // SimCalc engine family for numeric tasks
  "given": { "pv": 100000, "...": 0 },// solver inputs (synthetic data only)
  "ask": [ {"key":"cpi","label":"Cost Performance Index","type":"number"} ],
  "tolerance": 0.01,
  "decisions": [                       // judgement tasks scored by authored rubric
    { "key": "d1", "prompt": "...",
      "options": [
        {"key":"a","label":"...","quality":100,"consequence":"...","principle":"..."},
        {"key":"b","label":"...","quality":40,"consequence":"...","principle":"..."}
      ] }
  ],
  "profile_map": { "calculation": "Cost Guardian", "decision": "Recovery Leader",
                   "balanced": "Evidence-Based Decision Maker" },
  "share_line": "…one factual sentence for the share card…",
  "reveal": "after_submit"            // reference values shown only after grading
}
```

Scoring dimensions (deterministic):

- **calculation** — numeric asks vs `SimCalc.Resolve(task, key, given)` within `tolerance`
  (relative for |ref| > 1, absolute otherwise; same convention as the Simulation Lab);
- **decision** — mean authored `quality` of chosen options;
- **overall** — weighted mean (asks and decisions weighted by count), 0–100.

Decision profile: highest dimension wins its `profile_map` entry; within 10 points → `balanced`.
Every profile page explains which dimension produced it and names one improvement area (the
weakest dimension), so the identity is defensible, never random.

## Validator (`WorldContent.Validate`) — publication gate

Errors (block publish): missing/duplicate code, bad difficulty/track enums, no tasks at all,
unknown engine task, ask key that does not resolve through `SimCalc`, non-positive tolerance,
decision with <2 options or non-distinct qualities or missing consequence, quality outside
0–100, missing context/evidence, missing synthetic-data declaration, missing share line,
answer leakage (a numeric reference value appearing verbatim in context/hook/evidence/prompt).
Warnings (allowed): missing role/region, missing profile_map (falls back to generic profiles).
