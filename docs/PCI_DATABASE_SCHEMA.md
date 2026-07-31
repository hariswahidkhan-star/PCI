# PCI Platform — Complete Database Schema

**Generated from a freshly migrated database**, not written by hand — re-run
`gen_schema.py` to regenerate. Counts and types are therefore what the code actually
produces, including every runtime installer, not what a design document intended.

- **Tables:** 290
- **Columns:** 3,701
- **Indexes:** 336
- **Declared foreign keys:** 19

> **Read the foreign-key number before you read the ER diagrams.** Nineteen constraints
> across two hundred and ninety tables means referential integrity is, with a handful of
> exceptions, **maintained by application code rather than enforced by the database**.
> Relationships marked *inferred* in the ER diagrams are naming conventions this generator
> detected — they are real relationships in the code, but nothing stops a bad write.
> Delete a `users` row directly with SQL and you will orphan rows across dozens of tables.

Types are as declared in SQLite. On MySQL, `Db.Translate` maps them at runtime and
**datetimes are strings** in `YYYY-MM-DD HH:MM:SS` on both providers — see the developer
guide's data-access section.

---

## Contents

- [PCI World — community & moderation](#pci-world--community--moderation) — 15 tables
- [PCI World — forum](#pci-world--forum) — 9 tables
- [Forum (platform)](#forum-platform) — 3 tables
- [PCI World — careers](#pci-world--careers) — 8 tables
- [PCI World — editorial & contributors](#pci-world--editorial--contributors) — 9 tables
- [PCI World — challenges, rotation & intelligence](#pci-world--challenges-rotation--intelligence) — 7 tables
- [PCI World — identity, passport & admin](#pci-world--identity-passport--admin) — 23 tables
- [Students & identity](#students--identity) — 22 tables
- [Examinations & credentials](#examinations--credentials) — 31 tables
- [Payments, finance & partners](#payments-finance--partners) — 25 tables
- [Simulation Lab](#simulation-lab) — 6 tables
- [Content, website & SEO](#content-website--seo) — 40 tables
- [Marketing, social & syndication](#marketing-social--syndication) — 34 tables
- [Communications & notifications](#communications--notifications) — 23 tables
- [Support, casework & documents](#support-casework--documents) — 16 tables
- [Events](#events) — 2 tables
- [Integrations & operations](#integrations--operations) — 17 tables

---

## PCI World — community & moderation

*15 tables*

### `pciworld_community_eligibility_log`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `guest_session_id` | INTEGER | yes |  | → `pciworld_guest_sessions` *(inferred)* |
| `jurisdiction` | VARCHAR(8) | yes |  |  |
| `min_age_required` | INTEGER | no | `0` |  |
| `declared_age_band` | VARCHAR(16) | yes |  |  |
| `outcome` | VARCHAR(32) | no |  |  |
| `policy_version` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcelig_outcome`

### `pciworld_community_media`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `room_id` | INTEGER | no |  |  |
| `guest_session_id` | INTEGER | yes |  | → `pciworld_guest_sessions` *(inferred)* |
| `world_user_id` | INTEGER | yes |  |  |
| `message_id` | INTEGER | yes |  |  |
| `client_upload_id` | VARCHAR(64) | no |  |  |
| `state` | VARCHAR(16) | no | `'pending'` |  |
| `declared_mime` | VARCHAR(64) | yes |  |  |
| `sniffed_mime` | VARCHAR(64) | yes |  |  |
| `byte_size` | INTEGER | no | `0` |  |
| `width` | INTEGER | no | `0` |  |
| `height` | INTEGER | no | `0` |  |
| `content_sha256` | VARCHAR(64) | yes |  |  |
| `perceptual_hash` | VARCHAR(64) | yes |  |  |
| `alt_text` | TEXT | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `derivative_ref` | TEXT | yes |  |  |
| `scan_provider` | VARCHAR(48) | yes |  |  |
| `scan_provider_version` | VARCHAR(48) | yes |  |  |
| `scan_band` | VARCHAR(16) | yes |  |  |
| `scan_confidence` | REAL | yes |  |  |
| `policy_version_id` | INTEGER | yes |  | → `pciworld_policy_versions` *(inferred)* |
| `withheld_reason` | VARCHAR(48) | yes |  |  |
| `verdict_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcmedia_hash`, `ix_wcmedia_queue`, `ix_wcmedia_room`, `ux_wcmedia_upload` (unique)

### `pciworld_community_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `room_id` | INTEGER | no |  |  |
| `sequence` | INTEGER | yes |  |  |
| `guest_session_id` | INTEGER | yes |  | → `pciworld_guest_sessions` *(inferred)* |
| `world_user_id` | INTEGER | yes |  |  |
| `client_message_id` | VARCHAR(64) | no |  |  |
| `body` | TEXT | no |  |  |
| `body_normalized` | TEXT | yes |  |  |
| `reply_to_message_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(16) | no | `'pending'` |  |
| `decision_id` | INTEGER | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `published_at` | VARCHAR(32) | yes |  |  |
| `message_kind` | VARCHAR(16) | yes | `'text'` |  |
| `media_id` | INTEGER | yes |  |  |

**Indexes:** `ix_wcmsg_room`, `ix_wcmsg_session`, `ux_wcmsg_client` (unique), `ux_wcmsg_sequence` (unique)

### `pciworld_community_outbox`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event_type` | VARCHAR(48) | no |  |  |
| `room_id` | INTEGER | yes |  |  |
| `payload_json` | TEXT | no |  |  |
| `status` | VARCHAR(16) | no | `'queued'` |  |
| `attempts` | INTEGER | no | `0` |  |
| `next_attempt_at` | VARCHAR(32) | yes |  |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | VARCHAR(32) | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `sent_at` | VARCHAR(32) | yes |  |  |

**Indexes:** `ix_wcoutbox_due`

### `pciworld_community_reports`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `room_id` | INTEGER | no |  |  |
| `message_id` | INTEGER | yes |  |  |
| `reported_session_id` | INTEGER | yes |  |  |
| `reporter_session_id` | INTEGER | yes |  |  |
| `reporter_user_id` | INTEGER | yes |  |  |
| `reason_code` | VARCHAR(48) | no |  |  |
| `note` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | no | `'open'` |  |
| `case_id` | INTEGER | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcreport_status`, `ux_wcreport_once` (unique)

### `pciworld_community_rooms`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(120) | no |  |  |
| `title` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `topic` | TEXT | yes |  |  |
| `category` | VARCHAR(60) | yes |  |  |
| `locale` | VARCHAR(16) | yes | `'en'` |  |
| `room_type` | VARCHAR(24) | no | `'community'` |  |
| `state` | VARCHAR(16) | no | `'draft'` |  |
| `emergency_state` | VARCHAR(24) | yes |  |  |
| `capacity` | INTEGER | no | `200` |  |
| `guest_allowed` | INTEGER | no | `1` |  |
| `member_allowed` | INTEGER | no | `1` |  |
| `text_allowed` | INTEGER | no | `1` |  |
| `image_allowed` | INTEGER | no | `0` |  |
| `slow_mode_seconds` | INTEGER | no | `0` |  |
| `message_cooldown_ms` | INTEGER | no | `750` |  |
| `max_messages_per_session` | INTEGER | no | `200` |  |
| `opens_at` | VARCHAR(32) | yes |  |  |
| `closes_at` | VARCHAR(32) | yes |  |  |
| `timezone` | VARCHAR(64) | yes | `'UTC'` |  |
| `pinned_welcome` | TEXT | yes |  |  |
| `learning_prompt` | TEXT | yes |  |  |
| `resources_json` | TEXT | yes |  |  |
| `retention_class` | VARCHAR(32) | no | `'standard'` |  |
| `discoverable` | INTEGER | no | `1` |  |
| `rules_version` | VARCHAR(32) | no | `'v1'` |  |
| `policy_version_id` | INTEGER | yes |  | → `pciworld_policy_versions` *(inferred)* |
| `next_sequence` | INTEGER | no | `1` |  |
| `version` | INTEGER | no | `1` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcroom_state`

### `pciworld_guest_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `token_sha` | VARCHAR(64) | no |  |  |
| `room_id` | INTEGER | no |  |  |
| `display_name` | TEXT | no |  |  |
| `display_name_folded` | VARCHAR(64) | no |  |  |
| `active_name_key` | VARCHAR(64) | yes |  |  |
| `locale` | VARCHAR(16) | yes | `'en'` |  |
| `rules_version_accepted` | VARCHAR(32) | no |  |  |
| `risk_key` | VARCHAR(64) | yes |  |  |
| `status` | VARCHAR(16) | no | `'active'` |  |
| `message_count` | INTEGER | no | `0` |  |
| `last_message_at` | VARCHAR(32) | yes |  |  |
| `last_seen_sequence` | INTEGER | no | `0` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `expires_at` | VARCHAR(32) | yes |  |  |
| `ended_at` | VARCHAR(32) | yes |  |  |
| `ended_reason` | VARCHAR(48) | yes |  |  |

**Indexes:** `ix_wcguest_risk`, `ix_wcguest_room`, `ux_wcguest_name` (unique)

### `pciworld_media_scan_queue`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `media_id` | INTEGER | no |  |  |
| `status` | VARCHAR(16) | no | `'queued'` |  |
| `attempts` | INTEGER | no | `0` |  |
| `next_attempt_at` | VARCHAR(32) | yes |  |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | VARCHAR(32) | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `completed_at` | VARCHAR(32) | yes |  |  |

**Indexes:** `ix_wcscanq_due`, `ux_wcscanq_media` (unique)

### `pciworld_media_scans`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `media_id` | INTEGER | no |  |  |
| `provider` | VARCHAR(48) | no |  |  |
| `provider_version` | VARCHAR(48) | yes |  |  |
| `requested_at` | VARCHAR(32) | yes |  |  |
| `completed_at` | VARCHAR(32) | yes |  |  |
| `outcome` | VARCHAR(16) | no |  |  |
| `band` | VARCHAR(16) | yes |  |  |
| `confidence` | REAL | yes |  |  |
| `raw_labels_json` | TEXT | yes |  |  |
| `error_text` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcscan_media`

### `pciworld_moderation_case_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `case_id` | INTEGER | no |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `actor_kind` | VARCHAR(24) | no | `'system'` |  |
| `event` | VARCHAR(48) | no |  |  |
| `detail` | TEXT | yes |  |  |
| `previous_state` | VARCHAR(24) | yes |  |  |
| `new_state` | VARCHAR(24) | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wccaseev_case`

### `pciworld_moderation_cases`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `room_id` | INTEGER | yes |  |  |
| `subject_kind` | VARCHAR(24) | no |  |  |
| `subject_ref` | VARCHAR(64) | no |  |  |
| `severity` | VARCHAR(16) | no | `'normal'` |  |
| `status` | VARCHAR(24) | no | `'open'` |  |
| `restricted` | INTEGER | no | `0` |  |
| `assigned_to` | INTEGER | yes |  |  |
| `reason_code` | VARCHAR(48) | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `closed_at` | VARCHAR(32) | yes |  |  |

**Indexes:** `ix_wccase_queue`

### `pciworld_moderation_decisions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `scope` | VARCHAR(24) | no |  |  |
| `subject_ref` | VARCHAR(64) | yes |  |  |
| `content_hash` | VARCHAR(64) | yes |  |  |
| `policy_version_id` | INTEGER | yes |  | → `pciworld_policy_versions` *(inferred)* |
| `rule_id` | INTEGER | yes |  |  |
| `provider` | VARCHAR(48) | yes |  |  |
| `provider_version` | VARCHAR(48) | yes |  |  |
| `category` | VARCHAR(48) | yes |  |  |
| `severity` | VARCHAR(16) | yes |  |  |
| `confidence_band` | VARCHAR(16) | yes |  |  |
| `context_rule` | VARCHAR(48) | yes |  |  |
| `repetition_count` | INTEGER | no | `0` |  |
| `outcome` | VARCHAR(16) | no |  |  |
| `reason_code` | VARCHAR(48) | no |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcdecision_outcome`, `ix_wcdecision_subject`

### `pciworld_policy_rules`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `policy_version_id` | INTEGER | no |  | → `pciworld_policy_versions` *(inferred)* |
| `content_type` | VARCHAR(24) | no | `'text'` |  |
| `category` | VARCHAR(48) | no |  |  |
| `severity` | VARCHAR(16) | no | `'medium'` |  |
| `confidence_band` | VARCHAR(16) | no |  |  |
| `context_rule` | VARCHAR(48) | yes |  |  |
| `repetition_min` | INTEGER | no | `0` |  |
| `outcome` | VARCHAR(16) | no |  |  |
| `reason_code` | VARCHAR(48) | no |  |  |
| `sort` | INTEGER | no | `100` |  |

**Indexes:** `ix_wcrule_lookup`

### `pciworld_policy_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `version_label` | VARCHAR(48) | no |  |  |
| `status` | VARCHAR(24) | no | `'draft'` |  |
| `notes` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `activated_at` | VARCHAR(32) | yes |  |  |
| `retired_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

### `pciworld_reports`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `challenge_id` | INTEGER | yes |  | → `pciworld_challenges` *(inferred)* |
| `category` | VARCHAR(24) | no |  |  |
| `message` | TEXT | no |  |  |
| `session_id` | INTEGER | yes |  | → `pciworld_sessions` *(inferred)* |
| `status` | VARCHAR(16) | no | `'open'` |  |
| `resolution` | TEXT | yes |  |  |
| `resolved_by` | INTEGER | yes |  |  |
| `resolved_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldreports_ch`, `ix_worldreports_status`

## PCI World — forum

*9 tables*

### `pciworld_forum_categories`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(120) | no |  |  |
| `title` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `sort` | INTEGER | no | `100` |  |
| `min_trust_to_post` | VARCHAR(16) | no | `'new'` |  |
| `state` | VARCHAR(16) | no | `'open'` |  |
| `locale` | VARCHAR(16) | no | `'en'` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wforumcat_state`

### `pciworld_forum_flags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `flagged_by_user_id` | INTEGER | no |  |  |
| `weight` | INTEGER | no | `0` |  |
| `reason` | VARCHAR(48) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ux_wforumflag_once` (unique)

### `pciworld_forum_post_revisions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `revision_no` | INTEGER | no |  |  |
| `body` | TEXT | no |  |  |
| `body_rendered` | TEXT | yes |  |  |
| `body_hash` | VARCHAR(64) | yes |  |  |
| `edited_by_user_id` | INTEGER | yes |  |  |
| `edit_reason` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ux_wforumrev_no` (unique)

### `pciworld_forum_posts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `thread_id` | INTEGER | no |  |  |
| `author_user_id` | INTEGER | yes |  |  |
| `current_revision_id` | INTEGER | yes |  |  |
| `state` | VARCHAR(16) | no | `'pending'` |  |
| `kind` | VARCHAR(16) | no | `'reply'` |  |
| `reply_to_post_id` | INTEGER | yes |  |  |
| `flag_weight` | INTEGER | no | `0` |  |
| `decision_id` | INTEGER | yes |  |  |
| `published_at` | VARCHAR(32) | yes |  |  |
| `edited_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wforumpost_author`, `ix_wforumpost_queue`, `ix_wforumpost_thread`

### `pciworld_forum_standing`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `user_id` | INTEGER | yes |  | PK · → `users` *(inferred)* |
| `level` | VARCHAR(16) | no | `'new'` |  |
| `accepted_posts` | INTEGER | no | `0` |  |
| `upheld_reports` | INTEGER | no | `0` |  |
| `accurate_flags` | INTEGER | no | `0` |  |
| `first_post_at` | VARCHAR(32) | yes |  |  |
| `staff_granted` | INTEGER | no | `0` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

### `pciworld_forum_standing_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `from_level` | VARCHAR(16) | yes |  |  |
| `to_level` | VARCHAR(16) | no |  |  |
| `reason` | VARCHAR(64) | no |  |  |
| `actor_admin_id` | INTEGER | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wforumstanding_user`

### `pciworld_forum_tags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(80) | no |  |  |
| `label` | TEXT | no |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

### `pciworld_forum_thread_tags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `thread_id` | INTEGER | no |  |  |
| `tag_id` | INTEGER | no |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wforumthreadtag_tag`, `ux_wforumthreadtag` (unique)

### `pciworld_forum_threads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `category_id` | INTEGER | no |  |  |
| `slug` | VARCHAR(160) | no |  |  |
| `title` | TEXT | no |  |  |
| `author_user_id` | INTEGER | yes |  |  |
| `state` | VARCHAR(16) | no | `'open'` |  |
| `is_pinned` | INTEGER | no | `0` |  |
| `reply_count` | INTEGER | no | `0` |  |
| `last_post_at` | VARCHAR(32) | yes |  |  |
| `last_post_user_id` | INTEGER | yes |  |  |
| `view_count` | INTEGER | no | `0` |  |
| `solved_post_id` | INTEGER | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wforumthread_author`, `ix_wforumthread_recent`, `ux_wforumthread_slug` (unique)

## Forum (platform)

*3 tables*

### `forum_actions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `ip_hash` | VARCHAR(64) | no |  |  |
| `action` | VARCHAR(32) | no |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_forum_actions_lookup`

### `forum_posts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `thread_id` | INTEGER | no |  |  |
| `author_name` | TEXT | no |  |  |
| `body` | TEXT | no |  |  |
| `status` | VARCHAR(24) | yes | `'live'` |  |
| `flags` | INTEGER | yes | `0` |  |
| `ip_hash` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_forum_posts_thread`

### `forum_threads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `category` | VARCHAR(64) | no |  |  |
| `title` | TEXT | no |  |  |
| `author_name` | TEXT | no |  |  |
| `status` | VARCHAR(24) | yes | `'live'` |  |
| `reply_count` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `last_post_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_forum_threads_cat`, `ix_forum_threads_list`

## PCI World — careers

*8 tables*

### `pciworld_application_answers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `question_id` | INTEGER | yes |  |  |
| `prompt_snapshot` | TEXT | no |  |  |
| `answer_snapshot` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarans_app`

### `pciworld_application_consents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `employer_id` | INTEGER | no |  | → `pciworld_employers` *(inferred)* |
| `purpose` | VARCHAR(48) | no |  |  |
| `granted_at` | VARCHAR(32) | yes |  |  |
| `withdrawn_at` | VARCHAR(32) | yes |  |  |
| `policy_version` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarcons_app`

### `pciworld_application_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `event` | VARCHAR(32) | no |  |  |
| `from_state` | VARCHAR(16) | yes |  |  |
| `to_state` | VARCHAR(16) | yes |  |  |
| `actor_kind` | VARCHAR(16) | no |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `note` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarev_app`

### `pciworld_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `posting_id` | INTEGER | no |  |  |
| `applicant_user_id` | INTEGER | no |  |  |
| `state` | VARCHAR(16) | no | `'draft'` |  |
| `cv_ref` | VARCHAR(255) | yes |  |  |
| `cv_sha256` | VARCHAR(64) | yes |  |  |
| `cv_name` | VARCHAR(255) | yes |  |  |
| `cv_mime` | VARCHAR(96) | yes |  |  |
| `submitted_at` | VARCHAR(32) | yes |  |  |
| `withdrawn_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcarapp_posting`, `ix_wcarapp_user`, `ux_wcarapp_once` (unique)

### `pciworld_employer_members`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `employer_id` | INTEGER | no |  | → `pciworld_employers` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `role` | VARCHAR(16) | no | `'member'` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarmem_user`, `ux_wcarmem_once` (unique)

### `pciworld_employers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(160) | no |  |  |
| `name` | TEXT | no |  |  |
| `website` | VARCHAR(255) | yes |  |  |
| `state` | VARCHAR(24) | no | `'draft'` |  |
| `verified_at` | VARCHAR(32) | yes |  |  |
| `verified_by_admin_id` | INTEGER | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcaremp_state`

### `pciworld_job_postings`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `employer_id` | INTEGER | no |  | → `pciworld_employers` *(inferred)* |
| `slug` | VARCHAR(160) | no |  |  |
| `title` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `location` | TEXT | yes |  |  |
| `employment_type` | VARCHAR(24) | yes |  |  |
| `salary_min_minor` | INTEGER | yes |  |  |
| `salary_max_minor` | INTEGER | yes |  |  |
| `currency` | VARCHAR(8) | yes |  |  |
| `state` | VARCHAR(16) | no | `'draft'` |  |
| `published_at` | VARCHAR(32) | yes |  |  |
| `closes_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcarpost_employer`, `ix_wcarpost_public`, `ux_wcarpost_slug` (unique)

### `pciworld_job_questions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `posting_id` | INTEGER | no |  |  |
| `sort` | INTEGER | no | `100` |  |
| `kind` | VARCHAR(16) | no | `'text'` |  |
| `prompt` | TEXT | no |  |  |
| `required` | INTEGER | no | `0` |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarq_posting`

## PCI World — editorial & contributors

*9 tables*

### `pciworld_article_reviews`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `kind` | VARCHAR(16) | no |  |  |
| `reviewer_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `outcome` | VARCHAR(8) | no |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldartrev`

### `pciworld_article_sources`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `source_id` | INTEGER | no |  | → `pciworld_sources` *(inferred)* |
| `claim` | TEXT | yes |  |  |
| `confidence` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldartsrc`

### `pciworld_article_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `version` | INTEGER | no |  |  |
| `title` | TEXT | no |  |  |
| `dek` | TEXT | yes |  |  |
| `body_md` | TEXT | yes |  |  |
| `author_name` | TEXT | yes |  |  |
| `seo_title` | TEXT | yes |  |  |
| `seo_desc` | TEXT | yes |  |  |
| `tags_json` | TEXT | yes |  |  |
| `published_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_worldartver` (unique)

### `pciworld_articles`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(160) | no |  |  |
| `kind` | VARCHAR(8) | no | `'blog'` |  |
| `title` | TEXT | no |  |  |
| `dek` | TEXT | yes |  |  |
| `body_md` | TEXT | yes |  |  |
| `author_name` | TEXT | yes |  |  |
| `tags_json` | TEXT | yes |  |  |
| `seo_title` | TEXT | yes |  |  |
| `seo_desc` | TEXT | yes |  |  |
| `corrections_json` | TEXT | yes |  |  |
| `status` | VARCHAR(20) | no | `'idea'` |  |
| `current_version` | INTEGER | yes | `0` |  |
| `author_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `approved_by` | INTEGER | yes |  |  |
| `review_note` | TEXT | yes |  |  |
| `published_at` | VARCHAR(32) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `contributor_user_id` | INTEGER | yes |  |  |
| `contributor_terms_version` | VARCHAR(32) | yes |  |  |
| `declarations_json` | TEXT | yes |  |  |

**Indexes:** `ix_worldart_contrib`, `ix_worldart_kind`

### `pciworld_contributor_assignments`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `editor_admin_id` | INTEGER | no |  |  |
| `assigned_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `unassigned_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcontrasg_article`

### `pciworld_contributor_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `event` | VARCHAR(32) | no |  |  |
| `from_status` | VARCHAR(24) | yes |  |  |
| `to_status` | VARCHAR(24) | yes |  |  |
| `actor_kind` | VARCHAR(16) | no |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `note` | TEXT | yes |  |  |
| `declarations_json` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcontrev_article`

### `pciworld_contributor_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `sender_kind` | VARCHAR(16) | no |  |  |
| `sender_id` | INTEGER | yes |  |  |
| `body` | TEXT | no |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcontrmsg_article`

### `pciworld_contributors`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `state` | VARCHAR(16) | no | `'applied'` |  |
| `statement` | TEXT | yes |  |  |
| `terms_version` | VARCHAR(32) | yes |  |  |
| `granted_at` | VARCHAR(32) | yes |  |  |
| `granted_by_admin_id` | INTEGER | yes |  |  |
| `revoked_at` | VARCHAR(32) | yes |  |  |
| `revoked_by_admin_id` | INTEGER | yes |  |  |
| `revoke_reason` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcontrib_state`, `ux_wcontrib_user` (unique)

### `pciworld_sources`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `url` | VARCHAR(512) | no |  |  |
| `publisher` | TEXT | yes |  |  |
| `title` | TEXT | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `retrieved_at` | TEXT | yes | `datetime('now')` |  |
| `tier` | VARCHAR(24) | yes |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldsrc_url`

## PCI World — challenges, rotation & intelligence

*7 tables*

### `pciworld_attempts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `session_id` | INTEGER | no |  | → `pciworld_sessions` *(inferred)* |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `version` | INTEGER | no |  |  |
| `status` | VARCHAR(16) | no | `'in_progress'` |  |
| `answers_json` | TEXT | yes |  |  |
| `score` | REAL | yes |  |  |
| `dimensions_json` | TEXT | yes |  |  |
| `profile_key` | TEXT | yes |  |  |
| `display_name` | TEXT | yes |  |  |
| `result_token_sha` | VARCHAR(64) | yes |  |  |
| `result_revoked` | INTEGER | yes | `0` |  |
| `invite_id` | INTEGER | yes |  | → `pciworld_invites` *(inferred)* |
| `parent_attempt_id` | INTEGER | yes |  |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `completed_at` | VARCHAR(32) | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `passport_visible` | INTEGER | yes | `0` |  |
| `hints_used` | INTEGER | yes | `0` |  |
| `rotation_period_id` | INTEGER | yes |  | → `pciworld_rotation_periods` *(inferred)* |
| `canonical_user_id` | INTEGER | yes |  |  |

**Indexes:** `ix_worldatt_canonical`, `ix_worldatt_challenge`, `ix_worldatt_completed`, `ix_worldatt_period`, `ix_worldatt_resume`, `ix_worldatt_session`, `ix_worldatt_token`, `ix_worldatt_user`

### `pciworld_challenge_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `version` | INTEGER | no |  |  |
| `title` | TEXT | no |  |  |
| `hook` | TEXT | yes |  |  |
| `industry` | VARCHAR(64) | yes |  |  |
| `role` | TEXT | yes |  |  |
| `track` | VARCHAR(32) | yes |  |  |
| `difficulty` | VARCHAR(16) | yes |  |  |
| `est_minutes` | INTEGER | yes |  |  |
| `competencies_json` | TEXT | yes |  |  |
| `config_json` | TEXT | no |  |  |
| `published_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_worldver` (unique)

### `pciworld_challenges`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | VARCHAR(64) | no |  |  |
| `title` | TEXT | no |  |  |
| `hook` | TEXT | yes |  |  |
| `industry` | VARCHAR(64) | yes |  |  |
| `role` | TEXT | yes |  |  |
| `track` | VARCHAR(32) | yes | `'project_controls'` |  |
| `difficulty` | VARCHAR(16) | yes | `'foundation'` |  |
| `est_minutes` | INTEGER | yes | `8` |  |
| `competencies_json` | TEXT | yes |  |  |
| `synthetic_declared` | INTEGER | yes | `0` |  |
| `config_json` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | no | `'draft'` |  |
| `retired` | INTEGER | yes | `0` |  |
| `current_version` | INTEGER | yes | `0` |  |
| `author_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `reviewed_by` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `review_note` | TEXT | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `pi_type` | VARCHAR(24) | yes |  |  |
| `pi_domain` | VARCHAR(32) | yes |  |  |
| `pi_lifecycle` | VARCHAR(32) | yes |  |  |
| `pi_sector` | VARCHAR(32) | yes |  |  |
| `pi_interaction` | VARCHAR(32) | yes |  |  |

**Indexes:** `ix_worldch_facets`, `ix_worldch_pi`, `ix_worldch_servable`, `ix_worldch_status`

### `pciworld_rotation_lock`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `status` | VARCHAR(16) | no | `'queued'` |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `pciworld_rotation_order`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `cycle_no` | INTEGER | no |  |  |
| `seq_no` | INTEGER | no |  |  |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_worldrotord` (unique)

### `pciworld_rotation_periods`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `day_key` | VARCHAR(10) | no |  |  |
| `revision` | INTEGER | no | `1` |  |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `version` | INTEGER | no |  |  |
| `cycle_no` | INTEGER | no | `1` |  |
| `seq_no` | INTEGER | no | `0` |  |
| `source` | VARCHAR(16) | no | `'auto'` |  |
| `reason` | TEXT | yes |  |  |
| `superseded_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `opened_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldrotper_ch`, `ux_worldrotper` (unique)

### `pciworld_rotation_runs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `day_key` | VARCHAR(10) | yes |  |  |
| `outcome` | VARCHAR(32) | no |  |  |
| `periods_created` | INTEGER | yes | `0` |  |
| `detail` | TEXT | yes |  |  |
| `owner` | VARCHAR(64) | yes |  |  |
| `duration_ms` | INTEGER | yes |  |  |
| `ran_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_worldrotrun_at`

## PCI World — identity, passport & admin

*23 tables*

### `pciworld_admin_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `admin_id` | INTEGER | no |  |  |
| `token` | VARCHAR(64) | no |  |  |
| `expires_at` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldadm_token`

### `pciworld_admin_users`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | VARCHAR(190) | no |  |  |
| `name` | TEXT | yes |  |  |
| `role` | VARCHAR(16) | no | `'viewer'` |  |
| `password_hash` | TEXT | no |  |  |
| `status` | VARCHAR(16) | no | `'active'` |  |
| `failed_logins` | INTEGER | yes | `0` |  |
| `lockout_until` | TEXT | yes |  |  |
| `last_login_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `world_user_id` | INTEGER | yes |  |  |

### `pciworld_appeals`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `case_id` | INTEGER | no |  |  |
| `public_reference` | VARCHAR(32) | no |  |  |
| `credential_sha` | VARCHAR(64) | no |  |  |
| `credential_expires_at` | VARCHAR(32) | yes |  |  |
| `attempts` | INTEGER | no | `0` |  |
| `status` | VARCHAR(24) | no | `'issued'` |  |
| `submission` | TEXT | yes |  |  |
| `outcome` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `reviewed_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcappeal_case`

### `pciworld_audit`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `admin_id` | INTEGER | yes |  |  |
| `action` | VARCHAR(64) | no |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_worldaudit_at`

### `pciworld_calendar`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `day_utc` | VARCHAR(10) | no |  |  |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `pciworld_cv_access_log`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `employer_id` | INTEGER | yes |  | → `pciworld_employers` *(inferred)* |
| `actor_kind` | VARCHAR(16) | no |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `allowed` | INTEGER | no | `0` |  |
| `refused_reason` | VARCHAR(48) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcarcv_actor`, `ix_wcarcv_app`

### `pciworld_entities`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `legal_name` | TEXT | no |  |  |
| `trademark_spelling` | TEXT | yes |  |  |
| `aliases_json` | TEXT | yes |  |  |
| `risk_note` | TEXT | yes |  |  |
| `logo_permission` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `pciworld_entity_mentions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `article_id` | INTEGER | no |  | → `pciworld_articles` *(inferred)* |
| `entity_id` | INTEGER | no |  |  |
| `context` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldentmention`

### `pciworld_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event` | VARCHAR(48) | no |  |  |
| `challenge_id` | INTEGER | yes |  | → `pciworld_challenges` *(inferred)* |
| `session_id` | INTEGER | yes |  | → `pciworld_sessions` *(inferred)* |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_worldev_at`, `ix_worldev_event`

### `pciworld_handoff_codes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code_sha` | VARCHAR(64) | no |  |  |
| `world_user_id` | INTEGER | no |  |  |
| `return_to` | VARCHAR(128) | yes |  |  |
| `expires_at` | VARCHAR(32) | no |  |  |
| `consumed_at` | VARCHAR(32) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldhandoff_user`

### `pciworld_invites`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `token_sha` | VARCHAR(64) | no |  |  |
| `inviter_name` | TEXT | yes |  |  |
| `revoked` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldinv_attempt`

### `pciworld_oauth_clients`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `client_id` | VARCHAR(64) | yes |  | PK |
| `name` | VARCHAR(120) | no |  |  |
| `redirect_uris` | TEXT | no |  |  |
| `first_party` | INTEGER | no | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `pciworld_oauth_codes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code_sha` | VARCHAR(64) | no |  |  |
| `client_id` | VARCHAR(64) | no |  |  |
| `world_user_id` | INTEGER | no |  |  |
| `redirect_uri` | VARCHAR(400) | no |  |  |
| `code_challenge` | VARCHAR(128) | no |  |  |
| `minted_token_sha` | VARCHAR(64) | yes |  |  |
| `expires_at` | VARCHAR(32) | no |  |  |
| `consumed_at` | VARCHAR(32) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `pciworld_participants`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `status` | VARCHAR(16) | no | `'active'` |  |
| `suspension_reason` | TEXT | yes |  |  |
| `onboarding_state` | VARCHAR(24) | no | `'not_started'` |  |
| `goal` | VARCHAR(32) | yes |  |  |
| `timezone` | VARCHAR(64) | yes |  |  |
| `weekly_target` | INTEGER | yes |  |  |
| `preferences_json` | TEXT | yes |  |  |
| `first_entered_at` | TEXT | yes | `datetime('now')` |  |
| `onboarded_at` | TEXT | yes |  |  |
| `last_activity_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_worldpart_user` (unique)

### `pciworld_referrals`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `share_ref` | VARCHAR(64) | no |  |  |
| `anonymous_world_session_id` | INTEGER | no |  |  |
| `referred_user_id` | INTEGER | yes |  |  |
| `challenge_id` | INTEGER | no |  | → `pciworld_challenges` *(inferred)* |
| `challenge_version` | INTEGER | no |  |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `completed_at` | VARCHAR(32) | yes |  |  |
| `conversion_state` | VARCHAR(16) | no | `'started'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldref_session`, `ux_worldref` (unique)

### `pciworld_restricted_evidence`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `media_id` | INTEGER | no |  |  |
| `case_id` | INTEGER | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `content_sha256` | VARCHAR(64) | yes |  |  |
| `reason` | VARCHAR(64) | no |  |  |
| `preserved_until` | VARCHAR(32) | yes |  |  |
| `legal_hold` | INTEGER | no | `0` |  |
| `requested_by` | INTEGER | yes |  |  |
| `requested_at` | VARCHAR(32) | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | VARCHAR(32) | yes |  |  |
| `access_expires_at` | VARCHAR(32) | yes |  |  |
| `accessed_count` | INTEGER | no | `0` |  |
| `last_accessed_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `updated_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `version` | INTEGER | no | `1` |  |

**Indexes:** `ix_wcevid_hold`, `ux_wcevid_media` (unique)

### `pciworld_risk_restrictions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `risk_key` | VARCHAR(64) | no |  |  |
| `scope` | VARCHAR(24) | no | `'room'` |  |
| `room_id` | INTEGER | yes |  |  |
| `reason_code` | VARCHAR(48) | no |  |  |
| `case_id` | INTEGER | yes |  |  |
| `expires_at` | VARCHAR(32) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcrisk_key`

### `pciworld_sanctions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `subject_kind` | VARCHAR(24) | no |  |  |
| `subject_ref` | VARCHAR(64) | no |  |  |
| `room_id` | INTEGER | yes |  |  |
| `sanction_type` | VARCHAR(32) | no |  |  |
| `reason_code` | VARCHAR(48) | no |  |  |
| `policy_version_id` | INTEGER | yes |  | → `pciworld_policy_versions` *(inferred)* |
| `scope` | VARCHAR(24) | no | `'room'` |  |
| `issued_by` | INTEGER | yes |  |  |
| `issued_by_kind` | VARCHAR(24) | no | `'system'` |  |
| `approved_by` | INTEGER | yes |  |  |
| `starts_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `expires_at` | VARCHAR(32) | yes |  |  |
| `revoked_at` | VARCHAR(32) | yes |  |  |
| `revoked_by` | INTEGER | yes |  |  |
| `case_id` | INTEGER | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_wcsanction_subject`

### `pciworld_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `token_sha` | VARCHAR(64) | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `last_seen_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_worldsess_seen`

### `pciworld_user_map`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `legacy_world_id` | INTEGER | no |  |  |
| `canonical_user_id` | INTEGER | yes |  |  |
| `outcome` | VARCHAR(16) | no |  |  |
| `detail` | TEXT | yes |  |  |
| `resolved_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldmap_user`, `ux_worldmap_legacy` (unique)

### `pciworld_user_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `token` | VARCHAR(64) | no |  |  |
| `expires_at` | VARCHAR(32) | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldusess_exp`, `ix_worldusess_token`

### `pciworld_user_tokens`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `purpose` | VARCHAR(16) | no |  |  |
| `token_sha` | VARCHAR(64) | no |  |  |
| `expires_at` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldutok_user`

### `pciworld_users`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | VARCHAR(190) | no |  |  |
| `password_hash` | TEXT | no |  |  |
| `display_name` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | no | `'active'` |  |
| `email_verified` | INTEGER | yes | `0` |  |
| `passport_public` | INTEGER | yes | `0` |  |
| `passport_token_sha` | VARCHAR(64) | yes |  |  |
| `passport_show_scores` | INTEGER | yes | `1` |  |
| `passport_show_profiles` | INTEGER | yes | `1` |  |
| `passport_show_dates` | INTEGER | yes | `1` |  |
| `passport_expires_at` | TEXT | yes |  |  |
| `passport_photo_ref` | VARCHAR(255) | yes |  |  |
| `passport_photo_mime` | VARCHAR(32) | yes |  |  |
| `student_user_id` | INTEGER | yes |  |  |
| `failed_logins` | INTEGER | yes | `0` |  |
| `lockout_until` | TEXT | yes |  |  |
| `last_login_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_worldusers_passport`, `ux_worldusers_student` (unique)

## Students & identity

*22 tables*

### `account_requests`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `kind` | TEXT | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'received'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `admin_reset_tokens`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `admin_id` | INTEGER | no |  |  |
| `token` | VARCHAR(128) | no |  |  |
| `expires_at` | TEXT | no |  |  |
| `used_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_admin_reset_token`

### `admin_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `admin_id` | INTEGER | no |  | FK → `admin_users.id` |
| `token` | TEXT | no |  |  |
| `expires_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_admin_sess`

### `admin_users`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | TEXT | no |  |  |
| `name` | TEXT | yes |  |  |
| `password_hash` | TEXT | yes |  |  |
| `role` | TEXT | no | `'viewer'` |  |
| `permissions` | TEXT | yes | `'[]'` |  |
| `status` | TEXT | no | `'active'` |  |
| `must_change_pw` | INTEGER | yes | `1` |  |
| `last_login_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `totp_secret` | TEXT | yes |  |  |
| `totp_last_step` | INTEGER | yes |  |  |
| `totp_recovery` | TEXT | yes |  |  |
| `failed_logins` | INTEGER | yes | `0` |  |
| `lockout_until` | TEXT | yes |  |  |
| `cert_scope` | TEXT | yes |  |  |

### `candidate_consents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `consent_type` | TEXT | no |  |  |
| `policy_version` | TEXT | no |  |  |
| `accepted_at` | TEXT | yes | `datetime('now')` |  |
| `ip_address` | TEXT | yes |  |  |
| `user_agent` | TEXT | yes |  |  |
| `metadata_json` | TEXT | yes |  |  |

**Indexes:** `ix_consents_user`

### `enrollment_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | TEXT | no |  |  |
| `user_id` | INTEGER | yes |  | FK → `users.id` |
| `current_step` | INTEGER | yes | `1` |  |
| `session_status` | TEXT | yes | `'in_progress'` |  |
| `resume_token_hash` | TEXT | yes |  |  |
| `resume_token_expiry` | TEXT | yes |  |  |
| `selected_product` | TEXT | yes |  |  |
| `selected_membership` | TEXT | yes |  |  |
| `pricing_snapshot` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `last_activity_at` | TEXT | yes | `datetime('now')` |  |
| `reminders_sent` | INTEGER | yes | `0` |  |
| `last_reminder_at` | TEXT | yes |  |  |

### `fraud_flags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `kind` | VARCHAR(40) | no |  |  |
| `code_id` | INTEGER | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `email` | TEXT | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | yes | `'open'` |  |
| `actioned_by` | INTEGER | yes |  |  |
| `actioned_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_fraud_flags_status`

### `identity_checks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `result` | TEXT | yes |  |  |
| `confidence` | REAL | yes |  |  |
| `note` | TEXT | yes |  |  |
| `face_ref` | TEXT | yes |  |  |
| `id_ref` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `identity_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `doc_kind` | TEXT | yes | `'passport'` |  |
| `filename` | TEXT | yes |  |  |
| `mime` | TEXT | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `sha256` | TEXT | yes |  |  |
| `status` | TEXT | no | `'submitted'` |  |
| `review_note` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `reviewed_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_iddoc_user`

### `impersonation_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `session_id` | INTEGER | no |  | → `pciworld_sessions` *(inferred)* |
| `method` | VARCHAR(8) | yes |  |  |
| `path` | TEXT | yes |  |  |
| `at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_imp_events_session`

### `impersonation_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `token_sha` | VARCHAR(64) | no |  |  |
| `admin_id` | INTEGER | no |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `reason` | TEXT | yes |  |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `ended_at` | TEXT | yes |  |  |
| `last_seen_at` | TEXT | yes |  |  |

### `login_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `ip` | TEXT | yes |  |  |
| `user_agent` | TEXT | yes |  |  |
| `device` | TEXT | yes |  |  |
| `outcome` | TEXT | yes | `'success'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_login_events_user`

### `login_tokens`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | FK → `users.id` |
| `token` | TEXT | no |  |  |
| `purpose` | TEXT | yes | `'set_password'` |  |
| `expires_at` | TEXT | yes |  |  |
| `used_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `membership_upgrades`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `from_grade` | TEXT | yes |  |  |
| `to_grade` | TEXT | no |  |  |
| `statement` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'pending'` |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `reviewed_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `decided_at` | TEXT | yes |  |  |

**Indexes:** `ix_mupgrade_status`, `ix_mupgrade_user`

### `memberships`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | FK → `users.id` |
| `membership_type` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'inactive'` |  |
| `start_date` | TEXT | yes |  |  |
| `expiry_date` | TEXT | yes |  |  |
| `renewal_fee` | REAL | yes | `99` |  |
| `renewal_cycle` | TEXT | yes | `'3 years'` |  |
| `amount_paid` | REAL | yes |  |  |
| `currency` | TEXT | yes | `'USD'` |  |
| `grade` | TEXT | yes | `'associate'` |  |
| `stripe_customer_id` | VARCHAR(64) | yes |  |  |
| `stripe_subscription_id` | VARCHAR(64) | yes |  |  |
| `subscription_status` | VARCHAR(24) | yes |  |  |
| `cancel_at_period_end` | INTEGER | yes | `0` |  |

**Indexes:** `ix_memberships_user`

### `pci_identity_merges`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `source_user_id` | INTEGER | no |  |  |
| `target_user_id` | INTEGER | no |  |  |
| `reason` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | no | `'pending'` |  |
| `requested_by` | INTEGER | no |  |  |
| `requested_at` | TEXT | yes | `datetime('now')` |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `decision_note` | TEXT | yes |  |  |
| `before_json` | TEXT | yes |  |  |
| `after_json` | TEXT | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `row_version` | INTEGER | no | `1` |  |

**Indexes:** `ix_identity_merges_source`, `ix_identity_merges_status`, `ix_identity_merges_target`

### `pci_student_number_registry`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `student_number` | VARCHAR(32) | no |  |  |
| `format_version` | VARCHAR(16) | no | `'legacy_v1'` |  |
| `original_user_id` | INTEGER | no |  |  |
| `resolves_to_user_id` | INTEGER | yes |  |  |
| `state` | VARCHAR(16) | no | `'issued'` |  |
| `merged_into_student_number` | VARCHAR(32) | yes |  |  |
| `issued_at` | TEXT | yes | `datetime('now')` |  |
| `changed_at` | TEXT | yes | `datetime('now')` |  |
| `reason_code` | VARCHAR(48) | yes |  |  |
| `changed_by_admin_id` | INTEGER | yes |  |  |
| `correlation_id` | VARCHAR(64) | yes |  |  |
| `row_version` | INTEGER | no | `1` |  |

**Indexes:** `ix_student_number_registry_user`, `ux_student_number_registry` (unique)

### `qualifications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `institution` | TEXT | no |  |  |
| `degree` | TEXT | no |  |  |
| `field` | TEXT | yes |  |  |
| `year_completed` | TEXT | yes |  |  |
| `country` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_qual_user`

### `security_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `kind` | TEXT | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `ip` | TEXT | yes |  |  |
| `user_agent` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `student_profiles`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | FK → `users.id` |
| `mobile` | TEXT | yes |  |  |
| `country` | TEXT | yes |  |  |
| `city` | TEXT | yes |  |  |
| `preferred_language` | TEXT | yes |  |  |
| `current_role` | TEXT | yes |  |  |
| `company` | TEXT | yes |  |  |
| `industry_sector` | TEXT | yes |  |  |
| `years_experience` | TEXT | yes |  |  |
| `highest_qualification` | TEXT | yes |  |  |
| `project_controls_area` | TEXT | yes |  |  |
| `enrollment_purpose` | TEXT | yes |  |  |
| `profile_completion_percentage` | INTEGER | yes | `20` |  |
| `linkedin_url` | TEXT | yes |  |  |
| `profile_photo` | TEXT | yes |  |  |
| `directory_opt_in` | INTEGER | yes | `0` |  |
| `directory_headline` | TEXT | yes |  |  |
| `directory_show_country` | INTEGER | yes | `1` |  |
| `directory_show_org` | INTEGER | yes | `1` |  |
| `directory_show_linkedin` | INTEGER | yes | `0` |  |

**Indexes:** `ux_student_profiles_user` (unique)

### `users`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | TEXT | no |  |  |
| `first_name` | TEXT | yes |  |  |
| `last_name` | TEXT | yes |  |  |
| `registration_no` | VARCHAR(32) | yes |  |  |
| `registration_no_issued_at` | TEXT | yes |  |  |
| `password_hash` | TEXT | yes |  |  |
| `role` | TEXT | no | `'student'` |  |
| `status` | TEXT | no | `'pending'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `is_test` | INTEGER | yes | `0` |  |
| `failed_logins` | INTEGER | yes | `0` |  |
| `lockout_until` | TEXT | yes |  |  |
| `two_factor_enabled` | INTEGER | yes | `0` |  |
| `totp_secret` | TEXT | yes |  |  |
| `totp_last_step` | INTEGER | yes |  |  |
| `totp_recovery` | TEXT | yes |  |  |

**Indexes:** `ux_users_registration_no` (unique)

### `work_experiences`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `company` | TEXT | no |  |  |
| `title` | TEXT | no |  |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `is_current` | INTEGER | yes | `0` |  |
| `country` | TEXT | yes |  |  |
| `industry` | TEXT | yes |  |  |
| `hours_per_week` | TEXT | yes |  |  |
| `summary` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_workexp_user`

## Examinations & credentials

*31 tables*

### `bok_domains`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | TEXT | yes |  |  |
| `name` | TEXT | no |  |  |
| `weight` | INTEGER | yes | `0` |  |
| `description` | TEXT | yes |  |  |
| `bullets` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |

### `cert_document_downloads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `cert_document_id` | INTEGER | no |  | → `cert_documents` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `copy_id` | VARCHAR(16) | yes |  |  |
| `result` | VARCHAR(30) | yes |  |  |
| `ip` | VARCHAR(64) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_certdocdl_doc`

### `cert_document_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `cert_document_id` | INTEGER | no |  | → `cert_documents` *(inferred)* |
| `version` | INTEGER | no |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `filename` | VARCHAR(255) | yes |  |  |
| `mime` | VARCHAR(80) | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | VARCHAR(64) | yes |  |  |
| `replaced_by` | INTEGER | yes |  |  |
| `replace_reason` | TEXT | yes |  |  |
| `restored_from_id` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_certdocver_doc`

### `cert_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `kind` | TEXT | yes | `'general'` |  |
| `title` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `url` | TEXT | yes |  |  |
| `route_key` | TEXT | yes |  |  |
| `watermark` | INTEGER | yes | `0` |  |
| `published` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `storage_ref` | TEXT | yes |  |  |
| `filename` | TEXT | yes |  |  |
| `mime` | VARCHAR(80) | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | VARCHAR(64) | yes |  |  |

**Indexes:** `ix_cert_documents_cert`

### `certificate_downloads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `credential_id` | VARCHAR(64) | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `actor` | TEXT | yes |  |  |
| `role` | TEXT | yes |  |  |
| `ip` | TEXT | yes |  |  |
| `kind` | TEXT | yes |  |  |
| `result` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `certification_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_no` | VARCHAR(40) | yes |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | no |  | → `certifications` *(inferred)* |
| `route_key` | TEXT | no |  |  |
| `status` | VARCHAR(24) | no | `'submitted'` |  |
| `workflow_stage` | TEXT | yes | `'application_submitted'` |  |
| `data_json` | TEXT | yes |  |  |
| `blocker` | TEXT | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cert_apps_cert`, `ix_cert_apps_user`

### `certification_routes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `certification_id` | INTEGER | no | `1` | → `certifications` *(inferred)* |
| `route_key` | TEXT | no |  |  |
| `label` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `enabled` | INTEGER | yes | `1` |  |
| `public` | INTEGER | yes | `1` |  |
| `exam_required` | INTEGER | yes | `1` |  |
| `requires_approval` | INTEGER | yes | `1` |  |
| `fee_mode` | TEXT | yes | `'standard'` |  |
| `fee_amount` | DECIMAL(12,2) | yes |  |  |
| `discount_pct` | REAL | yes |  |  |
| `opens_at` | TEXT | yes |  |  |
| `closes_at` | TEXT | yes |  |  |
| `max_applications` | INTEGER | yes |  |  |
| `max_approvals` | INTEGER | yes |  |  |
| `certificate_wording` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_cert_route` (unique)

### `certifications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | TEXT | no |  |  |
| `name` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `credential_prefix` | TEXT | yes |  |  |
| `pass_mark_pct` | REAL | yes |  |  |
| `duration_minutes` | INTEGER | yes |  |  |
| `expiry_years` | INTEGER | yes | `3` |  |
| `exam_price` | REAL | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `acronym` | TEXT | yes |  |  |
| `short_name` | TEXT | yes |  |  |
| `public_title` | TEXT | yes |  |  |
| `tagline` | TEXT | yes |  |  |
| `short_description` | TEXT | yes |  |  |
| `category` | TEXT | yes |  |  |
| `level` | TEXT | yes |  |  |
| `status` | TEXT | yes |  |  |
| `slug` | TEXT | yes |  |  |
| `audience` | TEXT | yes |  |  |
| `overview` | TEXT | yes |  |  |
| `application_fee` | DECIMAL(12,2) | yes |  |  |
| `membership_required` | INTEGER | yes | `0` |  |
| `next_exam_note` | TEXT | yes |  |  |
| `meta_title` | TEXT | yes |  |  |
| `meta_description` | TEXT | yes |  |  |
| `keywords` | TEXT | yes |  |  |
| `og_title` | TEXT | yes |  |  |
| `og_description` | TEXT | yes |  |  |
| `social_image` | TEXT | yes |  |  |
| `canonical_url` | TEXT | yes |  |  |
| `content_json` | TEXT | yes |  |  |
| `certuvo_enabled` | INTEGER | yes | `1` |  |
| `certuvo_product` | TEXT | yes |  |  |
| `cpd_required_hours` | REAL | yes | `0` |  |
| `cpd_ai_hours_required` | REAL | yes | `0` |  |
| `credly_template_id` | VARCHAR(64) | yes |  |  |

### `exam_attempt_grants`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `authorization_id` | INTEGER | yes |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | → `certifications` *(inferred)* |
| `grant_type` | TEXT | yes | `'additional'` |  |
| `counts_as_attempt` | INTEGER | yes | `1` |  |
| `reason` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `incident_id` | INTEGER | yes |  |  |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `fee_applies` | INTEGER | yes | `0` |  |
| `fee_waived` | INTEGER | yes | `1` |  |
| `status` | TEXT | yes | `'granted'` |  |
| `consumed_attempt_id` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examgrant_auth`, `ix_examgrant_incident`, `ix_examgrant_user`

### `exam_attempts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `booking_id` | INTEGER | yes |  |  |
| `certification_id` | INTEGER | yes | `1` | FK → `certifications.id` |
| `kind` | TEXT | yes | `'exam'` |  |
| `violations` | INTEGER | yes | `0` |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `submitted_at` | TEXT | yes |  |  |
| `duration_minutes` | INTEGER | yes | `90` |  |
| `item_ids` | TEXT | yes |  |  |
| `answers` | TEXT | yes |  |  |
| `score` | REAL | yes |  |  |
| `max_score` | REAL | yes |  |  |
| `percent` | REAL | yes |  |  |
| `result` | TEXT | yes |  |  |
| `domain_breakdown` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'in_progress'` |  |
| `result_status` | TEXT | yes | `'not_started'` |  |
| `hold_reason` | TEXT | yes |  |  |
| `released_at` | TEXT | yes |  |  |
| `answer_key_version` | TEXT | yes |  |  |
| `bank_version` | TEXT | yes |  |  |
| `attempt_class` | TEXT | yes | `'normal'` |  |
| `authorization_id` | INTEGER | yes |  |  |
| `grant_id` | INTEGER | yes |  |  |
| `replaces_attempt_id` | INTEGER | yes |  |  |
| `counts_as_attempt` | INTEGER | yes | `1` |  |
| `invalidation_reason` | TEXT | yes |  |  |
| `invalidated_by` | INTEGER | yes |  |  |
| `identity_result` | TEXT | yes |  |  |
| `identity_confidence` | REAL | yes |  |  |
| `evidence_count` | INTEGER | yes | `0` |  |
| `review_status` | TEXT | yes | `'unreviewed'` |  |
| `review_note` | TEXT | yes |  |  |
| `reviewed_at` | TEXT | yes |  |  |
| `client_kind` | TEXT | yes | `'browser'` |  |
| `last_heartbeat_at` | TEXT | yes |  |  |

**Indexes:** `ix_attempts_user`

### `exam_authorizations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | → `certifications` *(inferred)* |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `entitlement_id` | INTEGER | yes |  |  |
| `eligibility_start` | TEXT | yes |  |  |
| `original_deadline` | TEXT | yes |  |  |
| `current_deadline` | TEXT | yes |  |  |
| `access_expiry` | TEXT | yes |  |  |
| `attempts_permitted` | INTEGER | yes | `1` |  |
| `attempts_used` | INTEGER | yes | `0` |  |
| `retake_wait_until` | TEXT | yes |  |  |
| `window_days` | INTEGER | yes |  |  |
| `window_source` | TEXT | yes |  |  |
| `route_key` | TEXT | yes |  |  |
| `campaign` | TEXT | yes |  |  |
| `institution_id` | INTEGER | yes |  |  |
| `country` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'active'` |  |
| `notes` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examauth_cert`, `ix_examauth_user`, `ux_examauth_payment` (unique)

### `exam_bookings`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | FK → `certifications.id` |
| `scheduled_at` | TEXT | no |  |  |
| `timezone` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'scheduled'` |  |
| `reschedule_count` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `authorization_id` | INTEGER | yes |  |  |
| `delivery_status` | VARCHAR(32) | yes |  |  |

**Indexes:** `ix_bookings_payment`, `ix_bookings_user`

### `exam_delivery_log`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `order_id` | INTEGER | yes |  |  |
| `provider_id` | INTEGER | yes |  |  |
| `provider` | TEXT | yes |  |  |
| `operation` | TEXT | no |  |  |
| `ok` | INTEGER | yes | `0` |  |
| `response_code` | INTEGER | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_exdellog_order`

### `exam_delivery_orders`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider_id` | INTEGER | no |  |  |
| `provider` | TEXT | no |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `booking_id` | INTEGER | yes |  |  |
| `attempt_id` | INTEGER | yes |  | → `pciworld_attempts` *(inferred)* |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `vendor_exam_code` | TEXT | yes |  |  |
| `delivery_type` | TEXT | yes | `'online'` |  |
| `status` | VARCHAR(32) | no | `'pending'` |  |
| `external_candidate_id` | TEXT | yes |  |  |
| `external_registration_id` | TEXT | yes |  |  |
| `external_appointment_id` | TEXT | yes |  |  |
| `confirmation_code` | TEXT | yes |  |  |
| `scheduled_at` | TEXT | yes |  |  |
| `timezone` | TEXT | yes |  |  |
| `result_status` | TEXT | yes |  |  |
| `score` | REAL | yes |  |  |
| `max_score` | REAL | yes |  |  |
| `raw_result` | TEXT | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |

**Indexes:** `ix_exdelorder_booking`, `ix_exdelorder_status`, `ux_exdelorder_booking_prov` (unique)

### `exam_delivery_providers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | VARCHAR(40) | no |  |  |
| `name` | TEXT | yes |  |  |
| `enabled` | INTEGER | yes | `0` |  |
| `is_default` | INTEGER | yes | `0` |  |
| `environment` | TEXT | yes | `'sandbox'` |  |
| `config` | TEXT | yes |  |  |
| `secret` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'idle'` |  |
| `last_sync_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_exdelprov_provider`

### `exam_entitlements`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `payment_id` | INTEGER | yes |  | FK → `payments.id` |
| `product_type` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes | `1` | FK → `certifications.id` |
| `status` | TEXT | yes | `'available'` |  |
| `valid_until` | TEXT | yes |  |  |
| `booking_id` | INTEGER | yes |  |  |
| `attempt_id` | INTEGER | yes |  | → `pciworld_attempts` *(inferred)* |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `authorization_id` | INTEGER | yes |  |  |
| `route_key` | TEXT | yes |  |  |

**Indexes:** `ix_entitlements_booking`, `ix_entitlements_user`, `ux_entitlement_payment` (unique)

### `exam_evidence`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `kind` | TEXT | yes |  |  |
| `mime` | TEXT | yes | `'image/jpeg'` |  |
| `data_uri` | TEXT | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_evidence_attempt`

### `exam_extension_history`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `authorization_id` | INTEGER | yes |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | → `certifications` *(inferred)* |
| `previous_deadline` | TEXT | yes |  |  |
| `new_deadline` | TEXT | yes |  |  |
| `previous_expiry` | TEXT | yes |  |  |
| `new_expiry` | TEXT | yes |  |  |
| `added_days` | INTEGER | yes |  |  |
| `reason` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `fee_applies` | INTEGER | yes | `0` |  |
| `is_free` | INTEGER | yes | `1` |  |
| `evidence_ref` | TEXT | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examext_auth`, `ix_examext_user`

### `exam_incidents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | → `certifications` *(inferred)* |
| `attempt_id` | INTEGER | yes |  | → `pciworld_attempts` *(inferred)* |
| `booking_id` | INTEGER | yes |  |  |
| `authorization_id` | INTEGER | yes |  |  |
| `category` | TEXT | yes |  |  |
| `occurred_at` | TEXT | yes |  |  |
| `student_explanation` | TEXT | yes |  |  |
| `proctor_report` | TEXT | yes |  |  |
| `tech_logs` | TEXT | yes |  |  |
| `evidence_ref` | TEXT | yes |  |  |
| `evidence_name` | TEXT | yes |  |  |
| `severity` | TEXT | yes | `'medium'` |  |
| `status` | VARCHAR(24) | yes | `'received'` |  |
| `investigation_result` | TEXT | yes |  |  |
| `decision` | TEXT | yes |  |  |
| `remedy` | TEXT | yes |  |  |
| `reported_by` | TEXT | yes | `'student'` |  |
| `created_by` | INTEGER | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examincident_attempt`, `ix_examincident_status`, `ix_examincident_user`

### `exam_launch_codes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | TEXT | yes |  |  |
| `code_hash` | TEXT | no |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `booking_id` | INTEGER | yes |  |  |
| `attempt_id` | INTEGER | yes |  | → `pciworld_attempts` *(inferred)* |
| `expires_at` | TEXT | yes |  |  |
| `redeemed_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_launch_code`, `ux_launch_code_hash` (unique)

### `exam_readiness_checks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `booking_id` | INTEGER | yes |  |  |
| `camera` | INTEGER | yes | `0` |  |
| `microphone` | INTEGER | yes | `0` |  |
| `network` | INTEGER | yes | `0` |  |
| `fullscreen` | INTEGER | yes | `0` |  |
| `environment` | INTEGER | yes | `0` |  |
| `browser` | TEXT | yes |  |  |
| `screen` | TEXT | yes |  |  |
| `passed` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_readiness_user`

### `exam_reschedule_history`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `booking_id` | INTEGER | yes |  |  |
| `authorization_id` | INTEGER | yes |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes | `1` | → `certifications` *(inferred)* |
| `previous_scheduled_at` | TEXT | yes |  |  |
| `previous_timezone` | TEXT | yes |  |  |
| `previous_status` | TEXT | yes |  |  |
| `new_scheduled_at` | TEXT | yes |  |  |
| `new_timezone` | TEXT | yes |  |  |
| `delivery_change` | TEXT | yes |  |  |
| `provider_change` | TEXT | yes |  |  |
| `reason` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `fee_applies` | INTEGER | yes | `0` |  |
| `fee_waived` | INTEGER | yes | `0` |  |
| `changed_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examresched_booking`, `ix_examresched_user`

### `exam_score_snapshots`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | FK → `exam_attempts.id` |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `score` | INTEGER | yes |  |  |
| `max_score` | INTEGER | yes |  |  |
| `percent` | REAL | yes |  |  |
| `result` | TEXT | yes |  |  |
| `domain_breakdown` | TEXT | yes |  |  |
| `unanswered` | INTEGER | yes |  |  |
| `flagged_events` | INTEGER | yes |  |  |
| `duration_seconds` | INTEGER | yes |  |  |
| `submitted_at` | TEXT | yes |  |  |
| `answer_key_version` | TEXT | yes |  |  |
| `bank_version` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_score_snapshot_attempt` (unique)

### `exam_window_rules`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `scope_type` | VARCHAR(40) | no |  |  |
| `scope_value` | VARCHAR(190) | yes |  |  |
| `window_days` | INTEGER | yes |  |  |
| `access_expiry_days` | INTEGER | yes |  |  |
| `attempts_permitted` | INTEGER | yes |  |  |
| `retake_wait_days` | INTEGER | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `note` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_examwindow_scope`

### `governance_roles`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `role` | TEXT | no |  |  |
| `holder` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'open'` |  |
| `remit` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |

### `held_certifications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `name` | TEXT | no |  |  |
| `issuer` | TEXT | yes |  |  |
| `credential_ref` | TEXT | yes |  |  |
| `issued_year` | TEXT | yes |  |  |
| `expires_year` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_heldcert_user`

### `issued_credentials`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `credential_id` | TEXT | no |  |  |
| `user_id` | INTEGER | yes |  | FK → `users.id` |
| `attempt_id` | INTEGER | yes |  | FK → `exam_attempts.id` |
| `holder_name` | TEXT | yes |  |  |
| `credential` | TEXT | yes | `'PCP-AI'` |  |
| `certification_id` | INTEGER | yes | `1` | FK → `certifications.id` |
| `status` | TEXT | yes | `'active'` |  |
| `issued_at` | TEXT | yes | `datetime('now')` |  |
| `expires_at` | TEXT | yes |  |  |
| `pdf_ref` | TEXT | yes |  |  |
| `pdf_sha256` | TEXT | yes |  |  |
| `verify_token` | TEXT | yes |  |  |
| `pdf_generated_at` | TEXT | yes |  |  |
| `route_key` | TEXT | yes |  |  |
| `certificate_wording` | TEXT | yes |  |  |
| `credly_badge_id` | VARCHAR(64) | yes |  |  |
| `credly_state` | VARCHAR(16) | yes |  |  |
| `credly_error` | TEXT | yes |  |  |

**Indexes:** `ix_credentials_user`, `ux_credential_attempt` (unique)

### `practice_attempts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `mode` | TEXT | no | `'quiz'` |  |
| `domain` | TEXT | yes |  |  |
| `question_ids` | TEXT | yes |  |  |
| `answers` | TEXT | yes |  |  |
| `score` | INTEGER | yes |  |  |
| `total` | INTEGER | yes |  |  |
| `domain_breakdown` | TEXT | yes |  |  |
| `status` | TEXT | no | `'in_progress'` |  |
| `duration_seconds` | INTEGER | yes |  |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `completed_at` | TEXT | yes |  |  |

**Indexes:** `ix_practice_user`

### `proctor_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `type` | TEXT | no |  |  |
| `severity` | TEXT | yes | `'Info'` |  |
| `detail` | TEXT | yes |  |  |
| `evidence_ref` | TEXT | yes |  |  |
| `at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_proctor_attempt`

### `proctor_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `sender` | TEXT | no | `'proctor'` |  |
| `body` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `delivered_at` | TEXT | yes |  |  |

**Indexes:** `ix_pm_attempt`

### `sample_questions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `question` | TEXT | no |  |  |
| `options` | TEXT | yes |  |  |
| `option_a` | TEXT | yes |  |  |
| `option_b` | TEXT | yes |  |  |
| `option_c` | TEXT | yes |  |  |
| `option_d` | TEXT | yes |  |  |
| `answer_index` | INTEGER | yes |  |  |
| `domain` | TEXT | yes |  |  |
| `published` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes |  |  |
| `is_practice` | INTEGER | yes | `0` |  |
| `explanation` | TEXT | yes |  |  |
| `difficulty` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes | `1` | FK → `certifications.id` |

## Payments, finance & partners

*25 tables*

### `checkout_reservations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `idempotency_key` | VARCHAR(120) | no |  |  |
| `email` | VARCHAR(255) | no |  |  |
| `product_type` | VARCHAR(32) | no |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `discount_code_id` | INTEGER | yes |  | → `discount_codes` *(inferred)* |
| `partner_id` | INTEGER | yes |  |  |
| `amount_minor` | INTEGER | no | `0` |  |
| `currency` | VARCHAR(8) | no | `'USD'` |  |
| `stripe_session_id` | VARCHAR(128) | yes |  |  |
| `status` | VARCHAR(16) | no | `'reserved'` |  |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `expires_at` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_checkout_res_code`, `ix_checkout_res_session`, `ux_checkout_res_idem` (unique)

### `code_redemptions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code_id` | INTEGER | yes |  |  |
| `code` | TEXT | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `email` | TEXT | yes |  |  |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `product_type` | TEXT | yes |  |  |
| `amount_before` | REAL | yes |  |  |
| `discount_amount` | REAL | yes |  |  |
| `redeemed_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_redemptions_code`, `ix_redemptions_email`

### `discount_codes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | TEXT | no |  |  |
| `discount_type` | TEXT | yes |  |  |
| `discount_value` | REAL | yes |  |  |
| `applies_to` | TEXT | yes | `'all'` |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `max_uses` | INTEGER | yes |  |  |
| `used_count` | INTEGER | yes | `0` |  |
| `single_use_per_email` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `code_type` | TEXT | yes | `'general'` |  |
| `org_name` | TEXT | yes |  |  |
| `owner_user_id` | INTEGER | yes |  |  |
| `batch_id` | TEXT | yes |  |  |
| `per_user_limit` | INTEGER | yes |  |  |
| `notes` | TEXT | yes |  |  |
| `founding_route` | TEXT | yes |  |  |
| `grants_membership` | INTEGER | yes | `1` |  |
| `grants_exam` | INTEGER | yes | `1` |  |
| `grants_study_access` | INTEGER | yes | `1` |  |
| `requires_application` | INTEGER | yes | `0` |  |
| `auto_approve` | INTEGER | yes | `1` |  |
| `membership_months` | INTEGER | yes | `12` |  |
| `criteria_json` | TEXT | yes |  |  |
| `partner_id` | INTEGER | yes |  |  |
| `reserved_count` | INTEGER | yes | `0` |  |
| `status` | VARCHAR(20) | yes |  |  |
| `created_by_partner_user` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `rejection_reason` | TEXT | yes |  |  |
| `campaign_name` | TEXT | yes |  |  |
| `min_payable` | DECIMAL(12,2) | yes |  |  |
| `eligible_countries` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | TEXT | yes |  |  |
| `min_transaction` | DECIMAL(12,2) | yes |  |  |
| `max_discount` | DECIMAL(12,2) | yes |  |  |

**Indexes:** `ix_codes_type`

### `fee_waivers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `product_type` | VARCHAR(24) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `kind` | VARCHAR(16) | yes | `'full'` |  |
| `original_amount` | DECIMAL(12,2) | yes |  |  |
| `waived_amount` | DECIMAL(12,2) | yes |  |  |
| `final_amount` | DECIMAL(12,2) | yes |  |  |
| `currency` | VARCHAR(8) | yes | `'USD'` |  |
| `reason` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `code_id` | INTEGER | yes |  |  |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `expires_at` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | yes | `'granted'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `fee_type` | TEXT | yes | `'exam'` |  |
| `waiver_type` | TEXT | yes |  |  |
| `sponsor` | TEXT | yes |  |  |
| `institution_id` | INTEGER | yes |  |  |
| `incident_id` | INTEGER | yes |  |  |
| `appeal_id` | INTEGER | yes |  | → `appeals` *(inferred)* |
| `evidence_ref` | TEXT | yes |  |  |
| `payable_amount` | DECIMAL(12,2) | yes |  |  |
| `idempotency_key` | VARCHAR(120) | yes |  |  |

**Indexes:** `ix_fee_waivers_user`, `ux_fee_waivers_idem` (unique)

### `partner_agreements`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_id` | INTEGER | no |  |  |
| `agreement_number` | VARCHAR(40) | no |  |  |
| `effective_from` | VARCHAR(10) | no |  |  |
| `effective_to` | VARCHAR(10) | yes |  |  |
| `currency` | VARCHAR(3) | no | `'USD'` |  |
| `payment_terms_days` | INTEGER | no | `30` |  |
| `minimum_payout_minor` | INTEGER | no | `0` |  |
| `refund_hold_days` | INTEGER | no | `30` |  |
| `tax_treatment` | VARCHAR(32) | yes |  |  |
| `status` | VARCHAR(16) | no | `'draft'` |  |
| `note` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_partner_agreements_partner`, `ux_partner_agreement_no` (unique)

### `partner_campaign_links`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `token` | VARCHAR(24) | no |  |  |
| `partner_id` | INTEGER | no |  |  |
| `code_id` | INTEGER | yes |  |  |
| `name` | VARCHAR(120) | no |  |  |
| `destination` | VARCHAR(255) | yes |  |  |
| `utm_source` | VARCHAR(80) | yes |  |  |
| `utm_medium` | VARCHAR(80) | yes |  |  |
| `utm_campaign` | VARCHAR(120) | yes |  |  |
| `active` | INTEGER | no | `1` |  |
| `click_count` | INTEGER | no | `0` |  |
| `created_by_partner_user` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pcl_partner`, `ux_pcl_token` (unique)

### `partner_commission_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `transaction_id` | INTEGER | no |  |  |
| `from_status` | VARCHAR(28) | yes |  |  |
| `to_status` | VARCHAR(28) | no |  |  |
| `actor_type` | VARCHAR(16) | no | `'system'` |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `reason` | TEXT | yes |  |  |
| `reference` | VARCHAR(64) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pce_txn`

### `partner_commission_rules`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `agreement_id` | INTEGER | no |  |  |
| `partner_id` | INTEGER | no |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | VARCHAR(40) | yes |  |  |
| `product_type` | VARCHAR(32) | yes |  |  |
| `country` | VARCHAR(64) | yes |  |  |
| `commission_type` | VARCHAR(12) | no | `'percentage'` |  |
| `commission_rate_bp` | INTEGER | no | `0` |  |
| `commission_fixed_minor` | INTEGER | no | `0` |  |
| `commission_basis` | VARCHAR(24) | no | `'net_after_discount'` |  |
| `effective_from` | VARCHAR(10) | yes |  |  |
| `effective_to` | VARCHAR(10) | yes |  |  |
| `priority` | INTEGER | no | `100` |  |
| `active` | INTEGER | no | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pcr_agreement`, `ix_pcr_partner`

### `partner_commission_transactions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `txn_ref` | VARCHAR(32) | yes |  |  |
| `dedupe_key` | VARCHAR(120) | no |  |  |
| `partner_id` | INTEGER | no |  |  |
| `agreement_id` | INTEGER | yes |  |  |
| `commission_rule_id` | INTEGER | yes |  |  |
| `discount_code_id` | INTEGER | yes |  | → `discount_codes` *(inferred)* |
| `code_redemption_id` | INTEGER | yes |  | → `code_redemptions` *(inferred)* |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | VARCHAR(40) | yes |  |  |
| `product_type` | VARCHAR(32) | yes |  |  |
| `currency` | VARCHAR(3) | no | `'USD'` |  |
| `gross_minor` | INTEGER | no | `0` |  |
| `discount_minor` | INTEGER | no | `0` |  |
| `eligible_net_minor` | INTEGER | no | `0` |  |
| `commission_type` | VARCHAR(12) | yes |  |  |
| `commission_rate_bp` | INTEGER | yes |  |  |
| `commission_basis` | VARCHAR(24) | yes |  |  |
| `commission_minor` | INTEGER | no | `0` |  |
| `status` | VARCHAR(28) | no | `'payment_received'` |  |
| `earned_at` | TEXT | yes |  |  |
| `due_at` | TEXT | yes |  |  |
| `hold_until` | TEXT | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `reversal_of_transaction_id` | INTEGER | yes |  |  |
| `reason` | TEXT | yes |  |  |
| `requires_finance_review` | INTEGER | no | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pct_partner`, `ix_pct_payment`, `ix_pct_status`, `ux_pct_dedupe` (unique)

### `partner_dispute_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `dispute_id` | INTEGER | no |  |  |
| `author_type` | VARCHAR(12) | no |  |  |
| `author_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `body` | TEXT | no |  |  |
| `internal` | INTEGER | no | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_dispute_msgs`

### `partner_disputes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `dispute_no` | VARCHAR(32) | no |  |  |
| `partner_id` | INTEGER | no |  |  |
| `transaction_id` | INTEGER | yes |  |  |
| `settlement_id` | INTEGER | yes |  |  |
| `category` | VARCHAR(32) | no | `'other'` |  |
| `subject` | VARCHAR(200) | no |  |  |
| `detail` | TEXT | yes |  |  |
| `claimed_amount_minor` | INTEGER | no | `0` |  |
| `currency` | VARCHAR(3) | no | `'USD'` |  |
| `status` | VARCHAR(20) | no | `'open'` |  |
| `resolution` | TEXT | yes |  |  |
| `adjustment_transaction_id` | INTEGER | yes |  |  |
| `raised_by_partner_user_id` | INTEGER | yes |  |  |
| `assigned_to` | INTEGER | yes |  |  |
| `resolved_by` | INTEGER | yes |  |  |
| `resolved_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_disputes_partner`, `ux_dispute_no` (unique)

### `partner_link_clicks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `link_id` | INTEGER | no |  |  |
| `visitor` | VARCHAR(32) | yes |  |  |
| `country` | VARCHAR(80) | yes |  |  |
| `device` | VARCHAR(24) | yes |  |  |
| `browser` | VARCHAR(32) | yes |  |  |
| `referrer` | VARCHAR(255) | yes |  |  |
| `created_at` | VARCHAR(32) | yes | `datetime('now')` |  |

**Indexes:** `ix_plc_link`

### `partner_notices`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_id` | INTEGER | no |  |  |
| `title` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `read_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_partner_notices_partner`

### `partner_payouts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_id` | INTEGER | no |  |  |
| `amount` | DECIMAL(12,2) | no |  |  |
| `currency` | TEXT | yes | `'USD'` |  |
| `note` | TEXT | yes |  |  |
| `paid_by` | INTEGER | yes |  |  |
| `paid_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_payouts_partner`

### `partner_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_user_id` | INTEGER | no |  | → `partner_users` *(inferred)* |
| `token` | VARCHAR(64) | no |  |  |
| `expires_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `partner_settlement_items`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `settlement_id` | INTEGER | no |  |  |
| `transaction_id` | INTEGER | no |  |  |
| `amount_allocated_minor` | INTEGER | no | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_psi_txn`, `ux_psi_pair` (unique)

### `partner_settlements`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `settlement_no` | VARCHAR(32) | no |  |  |
| `partner_id` | INTEGER | no |  |  |
| `period_start` | VARCHAR(10) | yes |  |  |
| `period_end` | VARCHAR(10) | yes |  |  |
| `currency` | VARCHAR(3) | no | `'USD'` |  |
| `opening_balance_minor` | INTEGER | no | `0` |  |
| `eligible_commission_minor` | INTEGER | no | `0` |  |
| `adjustments_minor` | INTEGER | no | `0` |  |
| `amount_approved_minor` | INTEGER | no | `0` |  |
| `amount_paid_minor` | INTEGER | no | `0` |  |
| `closing_balance_minor` | INTEGER | no | `0` |  |
| `status` | VARCHAR(20) | no | `'draft'` |  |
| `prepared_by` | INTEGER | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `scheduled_date` | VARCHAR(10) | yes |  |  |
| `paid_at` | TEXT | yes |  |  |
| `payment_method` | VARCHAR(32) | yes |  |  |
| `payment_reference` | VARCHAR(120) | yes |  |  |
| `proof_storage_ref` | VARCHAR(255) | yes |  |  |
| `internal_note` | TEXT | yes |  |  |
| `partner_note` | TEXT | yes |  |  |
| `legacy_payout_id` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_settlements_partner`, `ux_settlement_no` (unique)

### `partner_sponsorships`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_id` | INTEGER | no |  |  |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `application_id` | INTEGER | yes |  | → `pciworld_applications` *(inferred)* |
| `certification_id` | INTEGER | no | `1` | → `certifications` *(inferred)* |
| `route_key` | TEXT | yes | `'sponsored'` |  |
| `candidate_email` | TEXT | yes |  |  |
| `candidate_name` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'registered'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_sponsorships_partner`, `ux_sponsorship_candidate` (unique)

### `partner_users`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `partner_id` | INTEGER | no |  |  |
| `email` | VARCHAR(255) | no |  |  |
| `name` | TEXT | yes |  |  |
| `role` | VARCHAR(24) | yes | `'admin'` |  |
| `password_hash` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | yes | `'active'` |  |
| `must_change_pw` | INTEGER | yes | `1` |  |
| `last_login_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `failed_logins` | INTEGER | yes | `0` |  |
| `lockout_until` | TEXT | yes |  |  |

**Indexes:** `ix_partner_users_partner`

### `payments`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | FK → `users.id` |
| `enrollment_session_id` | INTEGER | yes |  | FK → `enrollment_sessions.id` |
| `product_type` | TEXT | yes |  |  |
| `standard_amount` | REAL | yes |  |  |
| `default_discount_amount` | REAL | yes |  |  |
| `discount_code` | TEXT | yes |  |  |
| `discount_code_amount` | REAL | yes |  |  |
| `final_amount` | REAL | yes |  |  |
| `currency` | TEXT | yes | `'USD'` |  |
| `payment_provider` | TEXT | yes |  |  |
| `provider_payment_id` | TEXT | yes |  |  |
| `payment_status` | TEXT | yes |  |  |
| `payment_date` | TEXT | yes |  |  |
| `invoice_url` | TEXT | yes |  |  |
| `receipt_url` | TEXT | yes |  |  |
| `reference` | TEXT | yes |  |  |
| `exam_schedule_deadline` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `method` | TEXT | yes |  |  |
| `bank_reference` | TEXT | yes |  |  |
| `gateway_reference` | TEXT | yes |  |  |
| `receipt_no` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `recorded_by` | INTEGER | yes |  |  |
| `waived_amount` | DECIMAL(12,2) | yes |  |  |
| `reversed_at` | TEXT | yes |  |  |
| `reversed_by` | INTEGER | yes |  |  |
| `reversal_reason` | TEXT | yes |  |  |
| `discount_code_id` | INTEGER | yes |  | → `discount_codes` *(inferred)* |
| `partner_id` | INTEGER | yes |  |  |
| `amount_refunded` | DECIMAL(12,2) | yes | `0` |  |
| `refunded_at` | TEXT | yes |  |  |

**Indexes:** `ix_payments_date`, `ix_payments_partner`, `ix_payments_status`, `ix_payments_user`, `ux_payments_provider` (unique)

### `pricing_rules`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `currency` | TEXT | yes | `'USD'` |  |
| `product_type` | TEXT | yes |  |  |
| `standard_price` | REAL | yes |  |  |
| `default_discount_percentage` | REAL | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `training_partner_application_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `doc_kind` | TEXT | yes | `'supporting'` |  |
| `filename` | TEXT | yes |  |  |
| `mime` | TEXT | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `sha256` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_tpappdoc_app`

### `training_partner_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `reference` | TEXT | no |  |  |
| `org_name` | TEXT | yes |  |  |
| `website` | TEXT | yes |  |  |
| `contact_name` | TEXT | yes |  |  |
| `contact_email` | TEXT | yes |  |  |
| `contact_phone` | TEXT | yes |  |  |
| `country` | TEXT | yes |  |  |
| `city` | TEXT | yes |  |  |
| `region` | TEXT | yes |  |  |
| `delivery_modes` | TEXT | yes |  |  |
| `specialties` | TEXT | yes |  |  |
| `learners_per_year` | INTEGER | yes |  |  |
| `description` | TEXT | yes |  |  |
| `declaration` | INTEGER | yes | `0` |  |
| `status` | TEXT | no | `'pending_review'` |  |
| `proposed_tier` | TEXT | yes |  |  |
| `partner_id` | INTEGER | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_tpapp_status`

### `training_partners`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `slug` | TEXT | yes |  |  |
| `tier` | TEXT | no | `'registered'` |  |
| `country` | TEXT | yes |  |  |
| `region` | TEXT | yes |  |  |
| `city` | TEXT | yes |  |  |
| `website` | TEXT | yes |  |  |
| `logo_url` | TEXT | yes |  |  |
| `summary` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `specialties` | TEXT | yes |  |  |
| `contact_email` | TEXT | yes |  |  |
| `listed` | INTEGER | yes | `0` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `source_application_id` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `partner_type` | TEXT | yes | `'training'` |  |
| `contact_name` | TEXT | yes |  |  |
| `commission_pct` | REAL | yes | `0` |  |
| `sponsor_enabled` | INTEGER | yes | `0` |  |
| `max_discount_percent` | REAL | yes |  |  |
| `max_codes` | INTEGER | yes |  |  |
| `max_uses_per_code` | INTEGER | yes |  |  |
| `total_allocation` | INTEGER | yes |  |  |
| `allow_full_sponsorship` | INTEGER | yes | `0` |  |
| `status` | VARCHAR(16) | yes | `'active'` |  |
| `institution_type` | TEXT | yes |  |  |
| `agreement_start` | TEXT | yes |  |  |
| `agreement_end` | TEXT | yes |  |  |
| `auto_approve_codes` | INTEGER | yes | `0` |  |
| `privacy_fields` | TEXT | yes |  |  |
| `eligible_countries` | TEXT | yes |  |  |
| `is_test` | INTEGER | yes | `0` |  |

**Indexes:** `ix_partners_listed`, `ix_partners_tier`

### `webhook_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | TEXT | yes | `'stripe'` |  |
| `event_id` | TEXT | yes |  | → `events` *(inferred)* |
| `processed_at` | TEXT | yes | `datetime('now')` |  |
| `status` | TEXT | yes |  |  |
| `error` | TEXT | yes |  |  |

**Indexes:** `ux_webhook_event` (unique)

## Simulation Lab

*6 tables*

### `simulation_attempt_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `event_type` | VARCHAR(32) | no |  |  |
| `period` | INTEGER | yes | `0` |  |
| `payload_json` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_simevents_attempt`, `ix_simevents_user`

### `simulation_attempts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `scenario_id` | INTEGER | no |  |  |
| `scenario_version` | INTEGER | yes | `1` |  |
| `mode` | VARCHAR(16) | no | `'training'` |  |
| `status` | VARCHAR(24) | no | `'in_progress'` |  |
| `seed` | INTEGER | yes | `0` |  |
| `period` | INTEGER | yes | `0` |  |
| `score` | REAL | yes |  |  |
| `hints_used` | INTEGER | yes | `0` |  |
| `state_json` | TEXT | yes |  |  |
| `started_at` | TEXT | yes | `datetime('now')` |  |
| `submitted_at` | TEXT | yes |  |  |
| `completed_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_simattempts_scenario`, `ix_simattempts_user`

### `simulation_competency`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `attempt_id` | INTEGER | no |  | → `pciworld_attempts` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `competency` | VARCHAR(48) | no |  |  |
| `score` | REAL | yes |  |  |
| `level` | VARCHAR(16) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_simcompetency_attempt`, `ix_simcompetency_user`

### `simulation_entitlements`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `source` | VARCHAR(24) | no | `'admin'` |  |
| `status` | VARCHAR(16) | no | `'active'` |  |
| `starts_at` | TEXT | yes |  |  |
| `expires_at` | TEXT | yes |  |  |
| `granted_by` | INTEGER | yes |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_simentitlements_user`

### `simulation_scenario_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `scenario_id` | INTEGER | no |  |  |
| `version` | INTEGER | no |  |  |
| `config_json` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_simversions_scenario` (unique)

### `simulation_scenarios`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `scenario_code` | VARCHAR(64) | no |  |  |
| `title` | TEXT | no |  |  |
| `kind` | VARCHAR(24) | no | `'guided_lab'` |  |
| `industry` | VARCHAR(64) | yes |  |  |
| `project_type` | VARCHAR(64) | yes |  |  |
| `difficulty` | VARCHAR(16) | yes | `'foundation'` |  |
| `est_minutes` | INTEGER | yes | `15` |  |
| `competencies_json` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `summary` | TEXT | yes |  |  |
| `brief` | TEXT | yes |  |  |
| `config_json` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | no | `'draft'` |  |
| `version` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `review_state` | VARCHAR(24) | no | `'draft'` |  |
| `synthetic_declared` | INTEGER | yes | `0` |  |
| `objectives_json` | TEXT | yes |  |  |
| `provenance` | TEXT | yes |  |  |
| `disclaimers` | TEXT | yes |  |  |
| `worked_solution` | TEXT | yes |  |  |
| `authored_by` | INTEGER | yes |  |  |
| `calc_reviewed_by` | INTEGER | yes |  |  |
| `learning_reviewed_by` | INTEGER | yes |  |  |
| `safety_reviewed_by` | INTEGER | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `review_due` | TEXT | yes |  |  |
| `expires_at` | TEXT | yes |  |  |
| `changelog_json` | TEXT | yes |  |  |
| `pilot_json` | TEXT | yes |  |  |

**Indexes:** `ix_simscenarios_kind`, `ix_simscenarios_review`, `ix_simscenarios_status`

## Content, website & SEO

*40 tables*

### `ai_content_generations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | VARCHAR(24) | yes |  |  |
| `model` | VARCHAR(80) | yes |  |  |
| `use_case` | VARCHAR(40) | yes |  |  |
| `post_id` | INTEGER | yes |  |  |
| `prompt` | TEXT | yes |  |  |
| `output` | TEXT | yes |  |  |
| `sources` | TEXT | yes |  |  |
| `tokens_in` | INTEGER | yes |  |  |
| `tokens_out` | INTEGER | yes |  |  |
| `status` | VARCHAR(16) | yes | `'generated'` |  |
| `human_approved` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_ai_gen_creator`, `ix_ai_gen_post`

### `ai_content_providers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | VARCHAR(24) | no |  |  |
| `label` | VARCHAR(120) | yes |  |  |
| `model` | VARCHAR(80) | yes |  |  |
| `use_case` | VARCHAR(40) | yes |  |  |
| `system_prompt` | TEXT | yes |  |  |
| `prompt_template` | TEXT | yes |  |  |
| `max_tokens` | INTEGER | yes | `1200` |  |
| `temperature` | REAL | yes | `0.7` |  |
| `key_env` | VARCHAR(64) | yes |  |  |
| `daily_quota` | INTEGER | yes |  |  |
| `require_citations` | INTEGER | yes | `0` |  |
| `require_review` | INTEGER | yes | `1` |  |
| `active` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_ai_providers_use`

### `blog_authors`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(160) | no |  |  |
| `name` | VARCHAR(200) | no |  |  |
| `title` | TEXT | yes |  |  |
| `bio` | TEXT | yes |  |  |
| `avatar_ref` | TEXT | yes |  |  |
| `credentials` | TEXT | yes |  |  |
| `email` | VARCHAR(255) | yes |  |  |
| `links` | TEXT | yes |  |  |
| `admin_user_id` | INTEGER | yes |  | → `admin_users` *(inferred)* |
| `active` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_blog_authors_active`

### `blog_categories`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(160) | no |  |  |
| `name` | VARCHAR(200) | no |  |  |
| `description` | TEXT | yes |  |  |
| `parent_id` | INTEGER | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `sort_order` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `seo_title` | TEXT | yes |  |  |
| `meta_description` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_blog_categories_active`

### `blog_post_tags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `post_id` | INTEGER | no |  | PK |
| `tag_id` | INTEGER | no |  | PK |

**Indexes:** `ix_blog_post_tags_tag`

### `blog_post_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `version` | INTEGER | no |  |  |
| `status_at` | VARCHAR(24) | yes |  |  |
| `snapshot_json` | TEXT | yes |  |  |
| `change_reason` | TEXT | yes |  |  |
| `editor_id` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_blog_versions_post`

### `blog_posts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(200) | no |  |  |
| `title` | TEXT | no |  |  |
| `seo_title` | TEXT | yes |  |  |
| `subtitle` | TEXT | yes |  |  |
| `summary` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `body_format` | VARCHAR(12) | yes | `'html'` |  |
| `blocks_json` | TEXT | yes |  |  |
| `featured_image` | TEXT | yes |  |  |
| `featured_image_alt` | TEXT | yes |  |  |
| `social_image` | TEXT | yes |  |  |
| `author_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `reviewer_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `editor_id` | INTEGER | yes |  |  |
| `category_id` | INTEGER | yes |  |  |
| `topic_cluster` | TEXT | yes |  |  |
| `primary_keyword` | TEXT | yes |  |  |
| `secondary_keywords` | TEXT | yes |  |  |
| `search_intent` | VARCHAR(24) | yes |  |  |
| `target_audience` | TEXT | yes |  |  |
| `language` | VARCHAR(8) | yes | `'en'` |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | TEXT | yes |  |  |
| `industry` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `published` | INTEGER | yes | `0` |  |
| `published_at` | TEXT | yes |  |  |
| `scheduled_at` | TEXT | yes |  |  |
| `original_published_at` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `reading_time` | INTEGER | yes |  |  |
| `meta_description` | TEXT | yes |  |  |
| `canonical_url` | TEXT | yes |  |  |
| `original_source_url` | TEXT | yes |  |  |
| `og_title` | TEXT | yes |  |  |
| `og_description` | TEXT | yes |  |  |
| `og_image` | TEXT | yes |  |  |
| `robots_noindex` | INTEGER | yes | `0` |  |
| `structured_type` | VARCHAR(24) | yes | `'BlogPosting'` |  |
| `content_ownership` | VARCHAR(24) | yes | `'original'` |  |
| `copyright_status` | TEXT | yes |  |  |
| `license` | TEXT | yes |  |  |
| `attribution` | TEXT | yes |  |  |
| `syndication_status` | VARCHAR(24) | yes | `'none'` |  |
| `social_distribution` | INTEGER | yes | `1` |  |
| `newsletter` | INTEGER | yes | `0` |  |
| `ai_assisted` | INTEGER | yes | `0` |  |
| `ai_disclosure` | VARCHAR(32) | yes |  |  |
| `internal_notes` | TEXT | yes |  |  |
| `extra_json` | TEXT | yes |  |  |
| `version` | INTEGER | yes | `1` |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_blog_posts_author`, `ix_blog_posts_cat`, `ix_blog_posts_pub`, `ix_blog_posts_status`

### `blog_reviews`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `stage` | VARCHAR(24) | no |  |  |
| `decision` | VARCHAR(16) | yes |  |  |
| `reviewer_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_blog_reviews_post`

### `blog_tags`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(160) | no |  |  |
| `name` | VARCHAR(200) | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `cc_analytics_metrics`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `source_id` | INTEGER | no |  | → `pciworld_sources` *(inferred)* |
| `dimension` | VARCHAR(16) | no |  |  |
| `dim_value` | VARCHAR(400) | yes |  |  |
| `metric_date` | VARCHAR(12) | yes |  |  |
| `clicks` | INTEGER | yes |  |  |
| `impressions` | INTEGER | yes |  |  |
| `ctr` | REAL | yes |  |  |
| `position` | REAL | yes |  |  |
| `sessions` | INTEGER | yes |  |  |
| `users` | INTEGER | yes |  |  |
| `pageviews` | INTEGER | yes |  |  |
| `fetched_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_anmetric_source`

### `cc_analytics_sources`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | VARCHAR(16) | no |  |  |
| `label` | VARCHAR(160) | yes |  |  |
| `property` | VARCHAR(300) | yes |  |  |
| `api_base` | VARCHAR(300) | yes |  |  |
| `auth_kind` | VARCHAR(16) | yes | `'bearer'` |  |
| `secret_enc` | TEXT | yes |  |  |
| `range_days` | INTEGER | yes | `28` |  |
| `status` | VARCHAR(16) | yes | `'not_connected'` |  |
| `active` | INTEGER | yes | `1` |  |
| `last_synced_at` | TEXT | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_ansrc_provider`

### `cc_backlinks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `prospect_id` | INTEGER | yes |  |  |
| `link_hash` | VARCHAR(64) | yes |  |  |
| `source_url` | VARCHAR(500) | no |  |  |
| `source_domain` | VARCHAR(190) | yes |  |  |
| `target_url` | VARCHAR(500) | yes |  |  |
| `anchor_text` | VARCHAR(300) | yes |  |  |
| `rel` | VARCHAR(16) | yes | `'unknown'` |  |
| `link_type` | VARCHAR(20) | yes | `'editorial'` |  |
| `status` | VARCHAR(16) | yes | `'candidate'` |  |
| `discovered_via` | VARCHAR(24) | yes | `'manual'` |  |
| `first_seen_at` | TEXT | yes |  |  |
| `last_checked_at` | TEXT | yes |  |  |
| `last_status_code` | INTEGER | yes |  |  |
| `notes` | TEXT | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_backlink_status`, `ux_cc_backlink_hash` (unique)

### `cc_content_links`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `url` | VARCHAR(500) | no |  |  |
| `url_norm` | VARCHAR(500) | yes |  |  |
| `kind` | VARCHAR(12) | yes | `'external'` |  |
| `anchor_text` | VARCHAR(300) | yes |  |  |
| `rel` | VARCHAR(24) | yes | `'auto'` |  |
| `is_citation` | INTEGER | yes | `0` |  |
| `approved` | INTEGER | yes | `0` |  |
| `status` | VARCHAR(16) | yes | `'unchecked'` |  |
| `http_code` | INTEGER | yes |  |  |
| `last_checked_at` | TEXT | yes |  |  |
| `clicks` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_links_post`, `ux_cc_links_post_url` (unique)

### `cc_external_items`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `source_id` | INTEGER | no |  | → `pciworld_sources` *(inferred)* |
| `guid` | VARCHAR(400) | yes |  |  |
| `source_url` | VARCHAR(500) | yes |  |  |
| `title` | TEXT | yes |  |  |
| `author` | VARCHAR(190) | yes |  |  |
| `summary` | TEXT | yes |  |  |
| `image_url` | VARCHAR(500) | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `content_hash` | VARCHAR(64) | yes |  |  |
| `status` | VARCHAR(28) | yes | `'retrieved'` |  |
| `pci_post_id` | INTEGER | yes |  |  |
| `review_note` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_extitem_source`, `ux_cc_extitem_guid` (unique)

### `cc_external_sources`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | VARCHAR(160) | no |  |  |
| `domain` | VARCHAR(190) | yes |  |  |
| `feed_url` | VARCHAR(400) | yes |  |  |
| `source_type` | VARCHAR(16) | yes | `'rss'` |  |
| `owner_contact` | VARCHAR(190) | yes |  |  |
| `license` | VARCHAR(40) | yes | `'all_rights_reserved'` |  |
| `permission_ref` | TEXT | yes |  |  |
| `allowed_use` | VARCHAR(24) | yes | `'curated_link'` |  |
| `attribution_required` | INTEGER | yes | `1` |  |
| `canonical_required` | INTEGER | yes | `1` |  |
| `auto_publish` | INTEGER | yes | `0` |  |
| `language` | VARCHAR(8) | yes |  |  |
| `category_id` | INTEGER | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `last_fetched_at` | TEXT | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_extsrc_active`

### `cc_link_prospects`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | VARCHAR(160) | no |  |  |
| `domain` | VARCHAR(190) | yes |  |  |
| `url` | VARCHAR(500) | yes |  |  |
| `category` | VARCHAR(40) | yes | `'publication'` |  |
| `authority` | INTEGER | yes |  |  |
| `relevance` | VARCHAR(12) | yes | `'medium'` |  |
| `status` | VARCHAR(20) | yes | `'prospect'` |  |
| `owner` | VARCHAR(120) | yes |  |  |
| `contact_name` | VARCHAR(120) | yes |  |  |
| `contact_email` | VARCHAR(190) | yes |  |  |
| `notes` | TEXT | yes |  |  |
| `next_action_at` | TEXT | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_prospect_status`

### `cc_outreach`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `prospect_id` | INTEGER | no |  |  |
| `channel` | VARCHAR(20) | yes | `'email'` |  |
| `direction` | VARCHAR(8) | yes | `'out'` |  |
| `subject` | VARCHAR(200) | yes |  |  |
| `body` | TEXT | yes |  |  |
| `outcome` | VARCHAR(20) | yes | `'sent'` |  |
| `occurred_at` | TEXT | yes | `datetime('now')` |  |
| `follow_up_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_outreach_prospect`

### `cc_syndicated_posts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `destination_id` | INTEGER | no |  |  |
| `external_id` | VARCHAR(190) | yes |  |  |
| `external_url` | TEXT | yes |  |  |
| `canonical_url` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'pending'` |  |
| `mode` | VARCHAR(24) | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `job_id` | INTEGER | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_synpost_post`, `ux_cc_synpost_dest` (unique)

### `cc_syndication_destinations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_key` | VARCHAR(48) | no |  |  |
| `label` | VARCHAR(160) | yes |  |  |
| `base_url` | VARCHAR(300) | yes |  |  |
| `config` | TEXT | yes |  |  |
| `secret_enc` | TEXT | yes |  |  |
| `mode` | VARCHAR(24) | yes | `'create'` |  |
| `default_status` | VARCHAR(16) | yes | `'draft'` |  |
| `status` | VARCHAR(20) | yes | `'connected'` |  |
| `last_error` | TEXT | yes |  |  |
| `connected_by` | INTEGER | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cc_syndest_platform`

### `content_capabilities`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_key` | VARCHAR(48) | no |  |  |
| `platform` | VARCHAR(120) | no |  |  |
| `kind` | VARCHAR(24) | no |  |  |
| `capability` | VARCHAR(40) | no |  |  |
| `publish_mode` | VARCHAR(32) | yes |  |  |
| `requires_approval` | INTEGER | yes | `0` |  |
| `official_api` | INTEGER | yes | `1` |  |
| `doc_url` | TEXT | yes |  |  |
| `notes` | TEXT | yes |  |  |
| `connected` | INTEGER | yes | `0` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_content_caps_kind`

### `content_i18n`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `lang` | TEXT | no |  |  |
| `scope` | TEXT | no |  |  |
| `slug` | TEXT | no | `''` |  |
| `ckey` | TEXT | no |  |  |
| `cvalue` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_content_i18n_lang`, `ux_content_i18n` (unique)

### `content_jobs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `job_type` | VARCHAR(32) | no |  |  |
| `idempotency_key` | VARCHAR(120) | yes |  |  |
| `post_id` | INTEGER | yes |  |  |
| `target` | VARCHAR(48) | yes |  |  |
| `payload` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | yes | `'pending'` |  |
| `attempts` | INTEGER | yes | `0` |  |
| `response_code` | INTEGER | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `result_ref` | TEXT | yes |  |  |
| `next_attempt_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |

**Indexes:** `ix_content_jobs_post`, `ix_content_jobs_status`

### `faqs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `question` | TEXT | no |  |  |
| `answer` | TEXT | yes |  |  |
| `category` | TEXT | yes | `'General'` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `published` | INTEGER | yes | `1` |  |

### `media_assets`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `filename` | TEXT | yes |  |  |
| `alt` | TEXT | yes |  |  |
| `width` | INTEGER | yes |  |  |
| `height` | INTEGER | yes |  |  |
| `usage` | TEXT | yes |  |  |

### `nav_items`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `label` | TEXT | no |  |  |
| `url` | TEXT | yes |  |  |
| `nav_group` | TEXT | yes | `'Footer'` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `visible` | INTEGER | yes | `1` |  |

### `news`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | TEXT | no |  |  |
| `body` | TEXT | yes |  |  |
| `url` | TEXT | yes |  |  |
| `published_date` | TEXT | yes |  |  |
| `published` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |

### `newsletter_subscribers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | TEXT | no |  |  |
| `status` | TEXT | yes | `'subscribed'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `page_blocks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | TEXT | no |  |  |
| `block_key` | TEXT | no |  |  |
| `label` | TEXT | yes |  |  |
| `ctype` | TEXT | yes | `'text'` |  |
| `cvalue` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_page_block_slug`, `ux_page_block` (unique)

### `pages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | TEXT | no |  |  |
| `title` | TEXT | yes |  |  |
| `meta_description` | TEXT | yes |  |  |
| `nav_group` | TEXT | yes |  |  |
| `noindex` | INTEGER | yes | `0` |  |
| `published` | INTEGER | yes | `1` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `canonical_url` | TEXT | yes |  |  |
| `og_image` | TEXT | yes |  |  |

### `public_document_downloads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `document_id` | INTEGER | no |  | → `documents` *(inferred)* |
| `doc_group` | VARCHAR(64) | yes |  |  |
| `ip` | VARCHAR(64) | yes |  |  |
| `ua` | VARCHAR(255) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pubdocdl_doc`

### `public_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `doc_group` | VARCHAR(64) | no |  |  |
| `title` | TEXT | no |  |  |
| `description` | TEXT | yes |  |  |
| `category` | VARCHAR(60) | yes | `'general'` |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | VARCHAR(40) | yes |  |  |
| `language` | VARCHAR(10) | yes | `'en'` |  |
| `version` | VARCHAR(20) | yes | `'1.0'` |  |
| `status` | VARCHAR(30) | no | `'draft'` |  |
| `legal_review_status` | VARCHAR(40) | yes | `'draft'` |  |
| `visibility` | VARCHAR(12) | yes | `'public'` |  |
| `owner` | TEXT | yes |  |  |
| `approver` | TEXT | yes |  |  |
| `effective_date` | TEXT | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `scheduled_at` | TEXT | yes |  |  |
| `next_review_date` | TEXT | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `filename` | TEXT | yes |  |  |
| `mime` | VARCHAR(80) | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | VARCHAR(64) | yes |  |  |
| `is_current` | INTEGER | yes | `0` |  |
| `supersedes_id` | INTEGER | yes |  |  |
| `related_groups` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `download_count` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_pubdoc_cat`, `ix_pubdoc_group`, `ix_pubdoc_status`

### `resources`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | TEXT | no |  |  |
| `category` | TEXT | yes |  |  |
| `doc_type` | TEXT | yes | `'PDF'` |  |
| `url` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `published` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |

### `reviews`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | FK → `users.id` |
| `name` | TEXT | no |  |  |
| `designation` | TEXT | yes |  |  |
| `company` | TEXT | yes |  |  |
| `country` | TEXT | yes |  |  |
| `relationship` | TEXT | yes |  |  |
| `rating` | INTEGER | yes |  |  |
| `title` | TEXT | yes |  |  |
| `body` | TEXT | no |  |  |
| `status` | TEXT | yes | `'pending'` |  |
| `featured` | INTEGER | yes | `0` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `published_at` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |

### `seo_redirects`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `from_path` | TEXT | no |  |  |
| `to_url` | TEXT | no |  |  |
| `status` | INTEGER | yes | `301` |  |
| `active` | INTEGER | yes | `1` |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_seo_redirect_from` (unique)

### `seo_submissions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `engine` | TEXT | no |  |  |
| `url_count` | INTEGER | yes | `0` |  |
| `status` | TEXT | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `site_content`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `ckey` | TEXT | no |  |  |
| `cgroup` | TEXT | yes |  |  |
| `label` | TEXT | yes |  |  |
| `ctype` | TEXT | yes | `'text'` |  |
| `cvalue` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `site_settings`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `skey` | TEXT | yes |  | PK |
| `svalue` | TEXT | yes |  |  |

### `template_download_daily`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `template_id` | INTEGER | no |  | PK · → `templates` *(inferred)* |
| `day` | VARCHAR(10) | no |  | PK |
| `count` | INTEGER | no | `0` |  |

**Indexes:** `ix_tpl_dl_day`

### `template_user_downloads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `user_id` | INTEGER | no |  | PK · → `users` *(inferred)* |
| `template_id` | INTEGER | no |  | PK · → `templates` *(inferred)* |
| `count` | INTEGER | no | `0` |  |
| `first_at` | TEXT | yes | `datetime('now')` |  |
| `last_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_tpl_user_dl`

### `templates`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `slug` | VARCHAR(80) | no |  |  |
| `title` | TEXT | no |  |  |
| `category` | VARCHAR(40) | no | `'general'` |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `summary` | TEXT | yes |  |  |
| `format` | VARCHAR(12) | no | `'csv'` |  |
| `body` | TEXT | no |  |  |
| `published` | INTEGER | no | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `download_count` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_templates_cat`, `ix_templates_pub`

## Marketing, social & syndication

*34 tables*

### `analytics_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event` | TEXT | no |  |  |
| `path` | TEXT | yes |  |  |
| `visitor` | TEXT | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `country` | TEXT | yes |  |  |
| `device` | TEXT | yes |  |  |
| `browser` | TEXT | yes |  |  |
| `utm_source` | TEXT | yes |  |  |
| `utm_medium` | TEXT | yes |  |  |
| `utm_campaign` | TEXT | yes |  |  |
| `referrer` | TEXT | yes |  |  |
| `landing` | TEXT | yes |  |  |
| `value` | DECIMAL(12,2) | yes |  |  |
| `currency` | TEXT | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_analytics_event_time`, `ix_analytics_time`

### `campaign_recipients`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `campaign_id` | INTEGER | yes |  |  |
| `email` | TEXT | yes |  |  |
| `first_name` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'pending'` |  |
| `error` | TEXT | yes |  |  |
| `sent_at` | TEXT | yes |  |  |

**Indexes:** `ix_campaign_recipients_campaign`

### `mkt_alerts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `kind` | VARCHAR(50) | yes |  |  |
| `severity` | VARCHAR(12) | yes | `'info'` |  |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `entity_type` | VARCHAR(30) | yes |  |  |
| `entity_id` | INTEGER | yes |  |  |
| `message` | TEXT | yes |  |  |
| `status` | VARCHAR(16) | yes | `'open'` |  |
| `acknowledged_by` | INTEGER | yes |  |  |
| `acknowledged_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_alert_status`

### `mkt_approvals`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `entity_type` | VARCHAR(30) | yes |  |  |
| `entity_id` | INTEGER | yes |  |  |
| `stage` | VARCHAR(40) | yes |  |  |
| `status` | VARCHAR(20) | yes | `'pending'` |  |
| `requested_by` | INTEGER | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_audiences`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `source` | VARCHAR(40) | yes |  |  |
| `purpose` | TEXT | yes |  |  |
| `countries` | TEXT | yes |  |  |
| `languages` | TEXT | yes |  |  |
| `professional_criteria` | TEXT | yes |  |  |
| `exclusions` | TEXT | yes |  |  |
| `consent_basis` | TEXT | yes |  |  |
| `retention` | TEXT | yes |  |  |
| `provider_audience_id` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `last_synced_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_budget_approvals`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `campaign_id` | INTEGER | yes |  |  |
| `requested_amount` | DECIMAL(12,2) | yes |  |  |
| `currency` | VARCHAR(8) | yes |  |  |
| `reason` | TEXT | yes |  |  |
| `tier` | VARCHAR(30) | yes |  |  |
| `requested_by` | INTEGER | yes |  |  |
| `status` | VARCHAR(20) | yes | `'pending'` |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_campaign_metrics`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_campaign_id` | INTEGER | yes |  |  |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `day` | TEXT | yes |  |  |
| `impressions` | INTEGER | yes | `0` |  |
| `reach` | INTEGER | yes | `0` |  |
| `clicks` | INTEGER | yes | `0` |  |
| `spend` | DECIMAL(18,6) | yes | `0` |  |
| `currency` | VARCHAR(8) | yes | `'USD'` |  |
| `leads` | INTEGER | yes | `0` |  |
| `conversions` | INTEGER | yes | `0` |  |
| `conversion_value` | DECIMAL(18,6) | yes | `0` |  |
| `video_views` | INTEGER | yes | `0` |  |
| `dedup_key` | VARCHAR(160) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_mkt_metrics_dedup` (unique)

### `mkt_campaigns`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `code` | VARCHAR(60) | yes |  |  |
| `owner_admin_id` | INTEGER | yes |  |  |
| `objective` | VARCHAR(40) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | TEXT | yes |  |  |
| `audience_summary` | TEXT | yes |  |  |
| `geography` | TEXT | yes |  |  |
| `language` | VARCHAR(10) | yes |  |  |
| `landing_page_id` | INTEGER | yes |  |  |
| `promotion_id` | INTEGER | yes |  |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `total_budget` | DECIMAL(12,2) | yes | `0` |  |
| `budget_currency` | VARCHAR(8) | yes | `'USD'` |  |
| `alloc_linkedin` | DECIMAL(12,2) | yes | `0` |  |
| `alloc_google` | DECIMAL(12,2) | yes | `0` |  |
| `alloc_meta` | DECIMAL(12,2) | yes | `0` |  |
| `conversion_goal` | TEXT | yes |  |  |
| `status` | VARCHAR(30) | yes | `'draft'` |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_camp_status`

### `mkt_capabilities`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_code` | VARCHAR(40) | no |  |  |
| `feature` | VARCHAR(80) | no |  |  |
| `required_api` | TEXT | yes |  |  |
| `required_permission` | TEXT | yes |  |  |
| `required_account_type` | TEXT | yes |  |  |
| `status` | VARCHAR(40) | no | `'provider_approval_required'` |  |
| `connection_id` | INTEGER | yes |  |  |
| `limitation` | TEXT | yes |  |  |
| `operator_action` | TEXT | yes |  |  |
| `last_tested_at` | TEXT | yes |  |  |
| `last_test_result` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_mkt_cap` (unique)

### `mkt_connections`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_code` | VARCHAR(40) | no |  |  |
| `label` | TEXT | yes |  |  |
| `external_org_id` | TEXT | yes |  |  |
| `external_ad_account_id` | TEXT | yes |  |  |
| `external_page_id` | TEXT | yes |  |  |
| `external_ig_id` | TEXT | yes |  |  |
| `external_business_id` | TEXT | yes |  |  |
| `external_property` | TEXT | yes |  |  |
| `connected_user_ref` | TEXT | yes |  |  |
| `granted_scopes` | TEXT | yes |  |  |
| `roles` | TEXT | yes |  |  |
| `access_tier` | VARCHAR(30) | yes |  |  |
| `api_version` | TEXT | yes |  |  |
| `account_currency` | VARCHAR(8) | yes |  |  |
| `account_timezone` | TEXT | yes |  |  |
| `access_token_enc` | TEXT | yes |  |  |
| `refresh_token_enc` | TEXT | yes |  |  |
| `token_expires_at` | TEXT | yes |  |  |
| `oauth_verifier` | VARCHAR(128) | yes |  |  |
| `status` | VARCHAR(24) | yes | `'disconnected'` |  |
| `approval_status` | VARCHAR(30) | yes | `'not_requested'` |  |
| `last_success_at` | TEXT | yes |  |  |
| `last_failure_at` | TEXT | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `connected_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_conn_platform`

### `mkt_conversation_ads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `campaign_id` | INTEGER | yes |  |  |
| `connection_id` | INTEGER | yes |  |  |
| `name` | TEXT | no |  |  |
| `objective` | TEXT | yes |  |  |
| `sender_identity` | TEXT | yes |  |  |
| `intro_message` | TEXT | yes |  |  |
| `steps_json` | TEXT | yes |  |  |
| `buttons_json` | TEXT | yes |  |  |
| `audience_id` | INTEGER | yes |  |  |
| `landing_page_id` | INTEGER | yes |  |  |
| `lead_form_id` | INTEGER | yes |  |  |
| `daily_budget` | DECIMAL(12,2) | yes |  |  |
| `lifetime_budget` | DECIMAL(12,2) | yes |  |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `frequency_cap` | TEXT | yes |  |  |
| `status` | VARCHAR(30) | yes | `'draft'` |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `provider_campaign_id` | TEXT | yes |  |  |
| `metrics_json` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_conversion_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `conversion_id` | INTEGER | yes |  |  |
| `campaign_id` | INTEGER | yes |  |  |
| `user_ref` | TEXT | yes |  |  |
| `value` | DECIMAL(12,2) | yes |  |  |
| `currency` | VARCHAR(8) | yes |  |  |
| `utm_json` | TEXT | yes |  |  |
| `event_at` | TEXT | yes |  |  |
| `upload_status` | VARCHAR(24) | yes | `'pending'` |  |
| `provider_response` | TEXT | yes |  |  |
| `dedup_key` | VARCHAR(160) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_mkt_conv_evt_dedup` (unique)

### `mkt_conversions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `provider_conversion_id` | TEXT | yes |  |  |
| `business_event` | VARCHAR(60) | yes |  |  |
| `value` | DECIMAL(12,2) | yes |  |  |
| `currency` | VARCHAR(8) | yes | `'USD'` |  |
| `enabled` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_creatives`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `format` | VARCHAR(30) | yes |  |  |
| `headline` | TEXT | yes |  |  |
| `primary_text` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `media_url` | TEXT | yes |  |  |
| `thumbnail_url` | TEXT | yes |  |  |
| `cta` | VARCHAR(40) | yes |  |  |
| `destination_url` | TEXT | yes |  |  |
| `display_url` | TEXT | yes |  |  |
| `utm_json` | TEXT | yes |  |  |
| `platform_scope` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `promotion_id` | INTEGER | yes |  |  |
| `language` | VARCHAR(10) | yes |  |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `provider_review_status` | TEXT | yes |  |  |
| `ai_generated` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_gsc_inspections`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `property_id` | INTEGER | yes |  |  |
| `url` | TEXT | yes |  |  |
| `index_status` | TEXT | yes |  |  |
| `coverage_state` | TEXT | yes |  |  |
| `last_crawl` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `inspected_by` | INTEGER | yes |  |  |
| `inspected_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_gsc_properties`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `connection_id` | INTEGER | yes |  |  |
| `property` | TEXT | no |  |  |
| `verified` | INTEGER | yes | `0` |  |
| `last_synced_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_gsc_query_data`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `property_id` | INTEGER | yes |  |  |
| `day` | TEXT | yes |  |  |
| `dimension` | VARCHAR(20) | yes |  |  |
| `dim_value` | TEXT | yes |  |  |
| `clicks` | INTEGER | yes | `0` |  |
| `impressions` | INTEGER | yes | `0` |  |
| `ctr` | REAL | yes | `0` |  |
| `position` | REAL | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_gsc_sitemaps`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `property_id` | INTEGER | yes |  |  |
| `path` | TEXT | yes |  |  |
| `last_submitted_at` | TEXT | yes |  |  |
| `status` | TEXT | yes |  |  |
| `errors` | INTEGER | yes | `0` |  |
| `warnings` | INTEGER | yes | `0` |  |
| `provider_response` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_jobs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `idempotency_key` | VARCHAR(160) | yes |  |  |
| `job_type` | VARCHAR(50) | yes |  |  |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `entity_type` | VARCHAR(30) | yes |  |  |
| `entity_id` | INTEGER | yes |  |  |
| `payload_json` | TEXT | yes |  |  |
| `status` | VARCHAR(20) | yes | `'queued'` |  |
| `attempts` | INTEGER | yes | `0` |  |
| `max_attempts` | INTEGER | yes | `5` |  |
| `last_error` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `next_attempt_at` | TEXT | yes |  |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_mkt_jobs_idem` (unique)

### `mkt_keywords`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_campaign_id` | INTEGER | yes |  |  |
| `list_name` | TEXT | yes |  |  |
| `keyword` | TEXT | no |  |  |
| `match_type` | VARCHAR(16) | yes |  |  |
| `kind` | VARCHAR(12) | yes | `'keyword'` |  |
| `status` | VARCHAR(16) | yes | `'active'` |  |
| `max_cpc` | DECIMAL(18,6) | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_landing_pages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `url` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `promotion_id` | INTEGER | yes |  |  |
| `headline` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `cta` | TEXT | yes |  |  |
| `application_link` | TEXT | yes |  |  |
| `noindex` | INTEGER | yes | `0` |  |
| `utm_json` | TEXT | yes |  |  |
| `conversion_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `valid_from` | TEXT | yes |  |  |
| `valid_to` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_lead_forms`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_code` | VARCHAR(40) | yes |  |  |
| `connection_id` | INTEGER | yes |  |  |
| `name` | TEXT | no |  |  |
| `provider_form_id` | TEXT | yes |  |  |
| `fields_json` | TEXT | yes |  |  |
| `consent_text` | TEXT | yes |  |  |
| `privacy_url` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_leads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | yes |  |  |
| `email` | TEXT | yes |  |  |
| `phone` | TEXT | yes |  |  |
| `country` | VARCHAR(60) | yes |  |  |
| `company` | TEXT | yes |  |  |
| `job_title` | TEXT | yes |  |  |
| `source_platform` | VARCHAR(40) | yes |  |  |
| `campaign_id` | INTEGER | yes |  |  |
| `platform_campaign_id` | INTEGER | yes |  |  |
| `ad_ref` | TEXT | yes |  |  |
| `form_id` | INTEGER | yes |  |  |
| `certification_interest` | TEXT | yes |  |  |
| `membership_interest` | INTEGER | yes | `0` |  |
| `institution_interest` | INTEGER | yes | `0` |  |
| `consent` | INTEGER | yes | `0` |  |
| `consent_text` | TEXT | yes |  |  |
| `utm_json` | TEXT | yes |  |  |
| `owner_admin_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(24) | yes | `'new'` |  |
| `lead_score` | INTEGER | yes | `0` |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `application_status` | TEXT | yes |  |  |
| `payment_status` | TEXT | yes |  |  |
| `first_contact_at` | TEXT | yes |  |  |
| `last_contact_at` | TEXT | yes |  |  |
| `next_followup_at` | TEXT | yes |  |  |
| `conversion_status` | TEXT | yes |  |  |
| `dedup_key` | VARCHAR(160) | yes |  |  |
| `provider_lead_id` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_lead_status`, `ux_mkt_lead_dedup` (unique)

### `mkt_linkedin_outreach`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `lead_id` | INTEGER | yes |  |  |
| `prospect_name` | TEXT | yes |  |  |
| `profile_url` | TEXT | yes |  |  |
| `suggested_message` | TEXT | yes |  |  |
| `sent_manually` | INTEGER | yes | `0` |  |
| `sent_at` | TEXT | yes |  |  |
| `response_note` | TEXT | yes |  |  |
| `followup_at` | TEXT | yes |  |  |
| `notes` | TEXT | yes |  |  |
| `owner_admin_id` | INTEGER | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_linkedin_posts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `campaign_id` | INTEGER | yes |  |  |
| `connection_id` | INTEGER | yes |  |  |
| `post_type` | VARCHAR(24) | yes | `'text'` |  |
| `body` | TEXT | yes |  |  |
| `article_title` | TEXT | yes |  |  |
| `article_url` | TEXT | yes |  |  |
| `media_json` | TEXT | yes |  |  |
| `alt_text` | TEXT | yes |  |  |
| `hashtags` | TEXT | yes |  |  |
| `cta` | VARCHAR(40) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `audience_note` | TEXT | yes |  |  |
| `language` | VARCHAR(10) | yes |  |  |
| `utm_json` | TEXT | yes |  |  |
| `scheduled_at` | TEXT | yes |  |  |
| `timezone` | TEXT | yes |  |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `linkedin_post_id` | TEXT | yes |  |  |
| `public_url` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `metrics_json` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_liposts_status`

### `mkt_platform_campaigns`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `campaign_id` | INTEGER | no |  |  |
| `platform_code` | VARCHAR(40) | no |  |  |
| `connection_id` | INTEGER | yes |  |  |
| `provider_campaign_id` | TEXT | yes |  |  |
| `name` | TEXT | yes |  |  |
| `objective` | TEXT | yes |  |  |
| `campaign_type` | TEXT | yes |  |  |
| `daily_budget` | DECIMAL(12,2) | yes |  |  |
| `lifetime_budget` | DECIMAL(12,2) | yes |  |  |
| `bid_strategy` | TEXT | yes |  |  |
| `targeting_json` | TEXT | yes |  |  |
| `landing_page_id` | INTEGER | yes |  |  |
| `lead_form_id` | INTEGER | yes |  |  |
| `conversion_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(30) | yes | `'draft'` |  |
| `provider_status` | TEXT | yes |  |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `last_synced_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_pcamp_campaign`

### `mkt_platforms`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | VARCHAR(40) | no |  |  |
| `name` | TEXT | no |  |  |
| `family` | VARCHAR(20) | yes |  |  |
| `official_api` | TEXT | yes |  |  |
| `docs_url` | TEXT | yes |  |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `mkt_promotions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `code` | VARCHAR(60) | yes |  |  |
| `promo_type` | VARCHAR(40) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | TEXT | yes |  |  |
| `fee_type` | VARCHAR(40) | yes |  |  |
| `original_amount` | DECIMAL(12,2) | yes |  |  |
| `discount_amount` | DECIMAL(12,2) | yes |  |  |
| `discount_percent` | REAL | yes |  |  |
| `net_amount` | DECIMAL(12,2) | yes |  |  |
| `currency` | VARCHAR(8) | yes | `'USD'` |  |
| `start_date` | TEXT | yes |  |  |
| `end_date` | TEXT | yes |  |  |
| `countries` | TEXT | yes |  |  |
| `languages` | TEXT | yes |  |  |
| `usage_limit` | INTEGER | yes |  |  |
| `per_user_limit` | INTEGER | yes |  |  |
| `landing_page_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `approval_status` | VARCHAR(30) | yes | `'draft'` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_mkt_promo_status`

### `social_accounts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform` | VARCHAR(40) | no |  |  |
| `display_name` | TEXT | yes |  |  |
| `handle` | TEXT | yes |  |  |
| `url` | TEXT | no |  |  |
| `icon_type` | VARCHAR(16) | yes | `'builtin'` |  |
| `custom_icon` | TEXT | yes |  |  |
| `aria_label` | TEXT | yes |  |  |
| `tooltip` | TEXT | yes |  |  |
| `locations` | TEXT | yes | `'footer'` |  |
| `display_order` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `is_official` | INTEGER | yes | `1` |  |
| `open_new_tab` | INTEGER | yes | `1` |  |
| `rel_nofollow` | INTEGER | yes | `1` |  |
| `language` | VARCHAR(8) | yes |  |  |
| `country` | VARCHAR(8) | yes |  |  |
| `effective_date` | TEXT | yes |  |  |
| `expiry_date` | TEXT | yes |  |  |
| `approval_status` | VARCHAR(16) | yes | `'approved'` |  |
| `link_status` | VARCHAR(16) | yes | `'not_checked'` |  |
| `link_checked_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `updated_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_social_accounts_order`

### `social_audit`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `account_id` | INTEGER | yes |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `action` | VARCHAR(40) | no |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_social_audit_account`

### `social_drafts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `post_id` | INTEGER | no |  |  |
| `platform_key` | VARCHAR(48) | no |  |  |
| `account_id` | INTEGER | yes |  |  |
| `text` | TEXT | yes |  |  |
| `link` | TEXT | yes |  |  |
| `hashtags` | TEXT | yes |  |  |
| `image` | TEXT | yes |  |  |
| `first_comment` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `scheduled_at` | TEXT | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `public_url` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `job_id` | INTEGER | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_social_drafts_post`, `ix_social_drafts_status`

### `social_link_checks`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `account_id` | INTEGER | no |  |  |
| `status` | VARCHAR(16) | yes |  |  |
| `http_code` | INTEGER | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `checked_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_social_link_checks_account`

### `social_pub_accounts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `platform_key` | VARCHAR(48) | no |  |  |
| `label` | VARCHAR(160) | yes |  |  |
| `external_id` | VARCHAR(190) | yes |  |  |
| `config` | TEXT | yes |  |  |
| `secret_enc` | TEXT | yes |  |  |
| `status` | VARCHAR(20) | yes | `'connected'` |  |
| `last_error` | TEXT | yes |  |  |
| `connected_by` | INTEGER | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_social_pub_accounts_platform`

### `social_share_settings`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `content_type` | VARCHAR(40) | no |  |  |
| `buttons` | TEXT | yes | `'linkedin,x,facebook,whatsapp,email,copy'` |  |
| `enabled` | INTEGER | yes | `0` |  |

**Indexes:** `ux_social_share_type` (unique)

## Communications & notifications

*23 tables*

### `chat_kb`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `question` | TEXT | no |  |  |
| `answer` | TEXT | no |  |  |
| `keywords` | TEXT | yes |  |  |
| `enabled` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |

### `chat_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `session_id` | INTEGER | no |  | → `pciworld_sessions` *(inferred)* |
| `sender` | TEXT | no |  |  |
| `body` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_chat_messages_session`

### `chat_sessions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `token` | VARCHAR(64) | no |  |  |
| `visitor_name` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'bot'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `last_activity_at` | VARCHAR(32) | yes | `datetime('now')` |  |
| `ip_hash` | TEXT | yes |  |  |
| `assigned_to` | INTEGER | yes |  |  |
| `linked_user_id` | INTEGER | yes |  |  |

**Indexes:** `ix_chat_sessions_status`

### `comm_campaigns`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `channel` | VARCHAR(16) | yes | `'email'` |  |
| `category` | VARCHAR(24) | yes | `'operational'` |  |
| `subject` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `template_key` | VARCHAR(80) | yes |  |  |
| `sender_profile_key` | VARCHAR(60) | yes |  |  |
| `whatsapp_account_key` | VARCHAR(60) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `filters` | TEXT | yes |  |  |
| `status` | VARCHAR(20) | yes | `'draft'` |  |
| `scheduled_at` | TEXT | yes |  |  |
| `recurring` | VARCHAR(16) | yes |  |  |
| `approval_required` | INTEGER | yes | `1` |  |
| `approved_by` | INTEGER | yes |  |  |
| `approved_at` | TEXT | yes |  |  |
| `total` | INTEGER | yes | `0` |  |
| `queued` | INTEGER | yes | `0` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_campaign_status`

### `comm_conversations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `reference` | VARCHAR(40) | yes |  |  |
| `channel` | VARCHAR(16) | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `customer_name` | TEXT | yes |  |  |
| `customer_email` | TEXT | yes |  |  |
| `customer_phone` | TEXT | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `application_id` | INTEGER | yes |  | → `pciworld_applications` *(inferred)* |
| `payment_id` | INTEGER | yes |  | → `payments` *(inferred)* |
| `received_address` | TEXT | yes |  |  |
| `assigned_admin_id` | INTEGER | yes |  |  |
| `priority` | VARCHAR(12) | yes | `'normal'` |  |
| `status` | VARCHAR(20) | yes | `'open'` |  |
| `sla_due_at` | TEXT | yes |  |  |
| `category` | VARCHAR(40) | yes |  |  |
| `last_message_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `suggested_response` | TEXT | yes |  |  |
| `tags` | TEXT | yes |  |  |
| `auto_answered` | INTEGER | yes | `0` |  |
| `routed_rule_id` | INTEGER | yes |  |  |

**Indexes:** `ix_comm_conv_status`

### `comm_delivery_attempts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `outbox_id` | INTEGER | no |  |  |
| `attempt` | INTEGER | yes |  |  |
| `status` | VARCHAR(20) | yes |  |  |
| `detail` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_attempt_outbox`

### `comm_inbound_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `conversation_id` | INTEGER | no |  |  |
| `direction` | VARCHAR(8) | yes | `'in'` |  |
| `channel` | VARCHAR(16) | yes |  |  |
| `from_addr` | TEXT | yes |  |  |
| `to_addr` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `provider_message_id` | TEXT | yes |  |  |
| `is_internal_note` | INTEGER | yes | `0` |  |
| `author_admin_id` | INTEGER | yes |  |  |
| `attachments` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_inbound_conv`

### `comm_outbox`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `dedup_key` | VARCHAR(160) | yes |  |  |
| `channel` | VARCHAR(16) | no |  |  |
| `trigger_code` | VARCHAR(80) | yes |  |  |
| `category` | VARCHAR(24) | yes | `'operational'` |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `conversation_id` | INTEGER | yes |  |  |
| `campaign_id` | INTEGER | yes |  |  |
| `to_email` | TEXT | yes |  |  |
| `to_phone` | TEXT | yes |  |  |
| `sender_profile_key` | VARCHAR(60) | yes |  |  |
| `whatsapp_account_key` | VARCHAR(60) | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `template_key` | VARCHAR(80) | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `status` | VARCHAR(20) | yes | `'queued'` |  |
| `scheduled_at` | VARCHAR(40) | yes |  |  |
| `attempts` | INTEGER | yes | `0` |  |
| `max_attempts` | INTEGER | yes | `5` |  |
| `last_error` | TEXT | yes |  |  |
| `provider` | VARCHAR(30) | yes |  |  |
| `provider_message_id` | TEXT | yes |  |  |
| `provider_response` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `next_attempt_at` | TEXT | yes |  |  |
| `sent_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |

**Indexes:** `ix_comm_outbox_status`, `ix_comm_outbox_user`, `ux_comm_outbox_dedup` (unique)

### `comm_preferences`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `email_marketing` | INTEGER | yes | `0` |  |
| `whatsapp_marketing` | INTEGER | yes | `0` |  |
| `whatsapp_optin` | INTEGER | yes | `0` |  |
| `newsletter` | INTEGER | yes | `0` |  |
| `events` | INTEGER | yes | `0` |  |
| `surveys` | INTEGER | yes | `0` |  |
| `consent_source` | TEXT | yes |  |  |
| `consent_version` | TEXT | yes |  |  |
| `consent_at` | TEXT | yes |  |  |
| `withdrawn_at` | TEXT | yes |  |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `comm_routing_rules`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | no |  |  |
| `priority` | INTEGER | yes | `100` |  |
| `match_channel` | VARCHAR(16) | yes |  |  |
| `match_received_address` | TEXT | yes |  |  |
| `match_keywords` | TEXT | yes |  |  |
| `match_certification_id` | INTEGER | yes |  |  |
| `set_category` | VARCHAR(40) | yes |  |  |
| `set_priority` | VARCHAR(12) | yes |  |  |
| `assign_admin_id` | INTEGER | yes |  |  |
| `sla_hours` | INTEGER | yes |  |  |
| `add_tags` | TEXT | yes |  |  |
| `escalate` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_routing_active`

### `comm_sender_profiles`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `key` | VARCHAR(60) | no |  |  |
| `name` | TEXT | no |  |  |
| `display_name` | TEXT | yes |  |  |
| `from_email` | TEXT | no |  |  |
| `reply_to` | TEXT | yes |  |  |
| `purpose` | TEXT | yes |  |  |
| `category` | VARCHAR(40) | yes | `'operational'` |  |
| `provider` | VARCHAR(30) | yes | `'resend'` |  |
| `domain_verified` | INTEGER | yes | `0` |  |
| `permitted_roles` | TEXT | yes |  |  |
| `is_default` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `approval_status` | VARCHAR(20) | yes | `'approved'` |  |
| `owner` | TEXT | yes |  |  |
| `effective_date` | TEXT | yes |  |  |
| `expiry_date` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `comm_suppression`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `channel` | VARCHAR(16) | yes |  |  |
| `address` | VARCHAR(190) | yes |  |  |
| `reason` | VARCHAR(40) | yes |  |  |
| `category` | VARCHAR(24) | yes |  |  |
| `source` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_suppress_addr`

### `comm_template_versions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `template_id` | INTEGER | no |  | → `templates` *(inferred)* |
| `version` | INTEGER | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `wa_template_name` | TEXT | yes |  |  |
| `saved_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `comm_templates`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `key` | VARCHAR(80) | no |  |  |
| `name` | TEXT | no |  |  |
| `kind` | VARCHAR(24) | yes | `'email'` |  |
| `category` | VARCHAR(40) | yes | `'operational'` |  |
| `subject` | TEXT | yes |  |  |
| `body` | TEXT | yes |  |  |
| `wa_template_name` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `route_key` | VARCHAR(40) | yes |  |  |
| `language` | VARCHAR(10) | yes | `'en'` |  |
| `version` | INTEGER | yes | `1` |  |
| `status` | VARCHAR(20) | yes | `'draft'` |  |
| `required_vars` | TEXT | yes |  |  |
| `approved_by` | INTEGER | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_comm_tpl_key`

### `comm_triggers`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `code` | VARCHAR(80) | no |  |  |
| `name` | TEXT | no |  |  |
| `event_group` | VARCHAR(40) | yes |  |  |
| `description` | TEXT | yes |  |  |
| `backend_wired` | INTEGER | yes | `0` |  |
| `email_enabled` | INTEGER | yes | `1` |  |
| `whatsapp_enabled` | INTEGER | yes | `0` |  |
| `inapp_enabled` | INTEGER | yes | `1` |  |
| `email_template_key` | VARCHAR(80) | yes |  |  |
| `whatsapp_template_key` | VARCHAR(80) | yes |  |  |
| `sender_profile_key` | VARCHAR(60) | yes |  |  |
| `whatsapp_account_key` | VARCHAR(60) | yes |  |  |
| `consent_category` | VARCHAR(24) | yes | `'transactional'` |  |
| `certification_scope` | TEXT | yes |  |  |
| `route_scope` | TEXT | yes |  |  |
| `delay_minutes` | INTEGER | yes | `0` |  |
| `reminder_sequence` | TEXT | yes |  |  |
| `conditions` | TEXT | yes |  |  |
| `dedup_window_minutes` | INTEGER | yes | `0` |  |
| `approval_required` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `effective_date` | TEXT | yes |  |  |
| `expiry_date` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `comm_whatsapp_accounts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `key` | VARCHAR(60) | no |  |  |
| `name` | TEXT | no |  |  |
| `display_name` | TEXT | yes |  |  |
| `phone_number` | TEXT | no |  |  |
| `provider` | VARCHAR(30) | yes | `'meta_cloud'` |  |
| `provider_account_id` | TEXT | yes |  |  |
| `token_env` | VARCHAR(80) | yes | `'WHATSAPP_ACCESS_TOKEN'` |  |
| `purpose` | TEXT | yes |  |  |
| `country` | VARCHAR(4) | yes |  |  |
| `permitted_categories` | TEXT | yes |  |  |
| `permitted_roles` | TEXT | yes |  |  |
| `business_hours` | TEXT | yes |  |  |
| `escalation_rule` | TEXT | yes |  |  |
| `verification_status` | VARCHAR(20) | yes | `'unverified'` |  |
| `is_default` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `owner` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `email_campaigns`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | TEXT | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `body_html` | TEXT | yes |  |  |
| `audience` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'draft'` |  |
| `total` | INTEGER | yes | `0` |  |
| `sent` | INTEGER | yes | `0` |  |
| `failed` | INTEGER | yes | `0` |  |
| `suppressed` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `sent_at` | TEXT | yes |  |  |

### `email_logs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `email` | TEXT | yes |  |  |
| `email_type` | TEXT | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `status` | TEXT | yes |  |  |
| `sent_at` | TEXT | yes | `datetime('now')` |  |

### `email_suppression`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `email` | VARCHAR(255) | no |  |  |
| `reason` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `form_submissions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `form_type` | TEXT | yes |  |  |
| `name` | TEXT | yes |  |  |
| `email` | TEXT | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `message` | TEXT | yes |  |  |
| `reference` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'new'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `inquiries`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `type` | TEXT | yes | `'general'` |  |
| `email` | TEXT | no |  |  |
| `first_name` | TEXT | yes |  |  |
| `topic` | TEXT | yes |  |  |
| `seats` | TEXT | yes |  |  |
| `org` | TEXT | yes |  |  |
| `message` | TEXT | yes |  |  |
| `reference` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'new'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `notification_history`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `channel` | TEXT | no | `'email'` |  |
| `recipient` | TEXT | yes |  |  |
| `subject` | TEXT | yes |  |  |
| `status` | TEXT | yes |  |  |
| `related_type` | TEXT | yes |  |  |
| `related_id` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `notifications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `category` | TEXT | yes | `'General'` |  |
| `title` | TEXT | no |  |  |
| `body` | TEXT | yes |  |  |
| `cta_label` | TEXT | yes |  |  |
| `cta_route` | TEXT | yes |  |  |
| `dedupe_key` | TEXT | yes |  |  |
| `read_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_notif_user`

## Support, casework & documents

*16 tables*

### `accommodation_requests`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `request_type` | TEXT | no |  |  |
| `description` | TEXT | no |  |  |
| `evidence_name` | TEXT | yes |  |  |
| `evidence_data` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'submitted'` |  |
| `approved_extra_minutes` | INTEGER | yes | `0` |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `decided_at` | TEXT | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |

**Indexes:** `ix_accom_user`

### `appeals`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `attempt_id` | INTEGER | yes |  | → `pciworld_attempts` *(inferred)* |
| `credential_id` | TEXT | yes |  |  |
| `type` | TEXT | no |  |  |
| `reason` | TEXT | no |  |  |
| `evidence_name` | TEXT | yes |  |  |
| `evidence_data` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'submitted'` |  |
| `submitted_at` | TEXT | yes | `datetime('now')` |  |
| `decision` | TEXT | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |

**Indexes:** `ix_appeals_user`

### `cpd_declarations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `cycle_year` | INTEGER | no |  |  |
| `position` | TEXT | no |  |  |
| `statement` | TEXT | yes |  |  |
| `hours_snapshot` | REAL | yes | `0` |  |
| `ai_hours_snapshot` | REAL | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_cpd_decl_user`

### `cpd_entries`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `activity_date` | TEXT | yes |  |  |
| `category` | TEXT | yes |  |  |
| `hours` | REAL | yes | `0` |  |
| `description` | TEXT | yes |  |  |
| `evidence_name` | TEXT | yes |  |  |
| `evidence_data` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `reviewed_at` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'recorded'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `source_event_id` | INTEGER | yes |  |  |

**Indexes:** `ix_cpd_user`, `ux_cpd_event_user` (unique)

### `document_acknowledgements`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `document_id` | INTEGER | no |  | → `documents` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `ip` | VARCHAR(64) | yes |  |  |
| `acknowledged_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_docack` (unique)

### `document_assignments`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `document_id` | INTEGER | no |  | → `documents` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `assignment_type` | VARCHAR(40) | yes |  |  |
| `source` | VARCHAR(20) | yes | `'auto'` |  |
| `status` | VARCHAR(20) | yes | `'active'` |  |
| `assigned_by` | INTEGER | yes |  |  |
| `assigned_at` | TEXT | yes | `datetime('now')` |  |
| `revoked_by` | INTEGER | yes |  |  |
| `revoked_at` | TEXT | yes |  |  |
| `revoke_reason` | TEXT | yes |  |  |

**Indexes:** `ix_docassign_user`, `ux_docassign` (unique)

### `document_categories`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `name` | VARCHAR(120) | no |  |  |
| `slug` | VARCHAR(120) | yes |  |  |
| `description` | TEXT | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `document_downloads`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `document_id` | INTEGER | yes |  | → `documents` *(inferred)* |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `actor` | TEXT | yes |  |  |
| `role` | VARCHAR(20) | yes |  |  |
| `ip` | VARCHAR(64) | yes |  |  |
| `action` | VARCHAR(20) | yes |  |  |
| `result` | VARCHAR(30) | yes |  |  |
| `version` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_docdl_doc`

### `documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | VARCHAR(255) | no |  |  |
| `description` | TEXT | yes |  |  |
| `category` | VARCHAR(120) | yes |  |  |
| `doc_type` | VARCHAR(60) | yes | `'general'` |  |
| `status` | VARCHAR(20) | no | `'draft'` |  |
| `storage_ref` | TEXT | yes |  |  |
| `filename` | VARCHAR(255) | yes |  |  |
| `mime` | VARCHAR(120) | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | VARCHAR(64) | yes |  |  |
| `version` | INTEGER | yes | `1` |  |
| `root_id` | INTEGER | yes |  |  |
| `supersedes_id` | INTEGER | yes |  |  |
| `superseded_by` | INTEGER | yes |  |  |
| `assignment_type` | VARCHAR(40) | yes | `'all'` |  |
| `assignment_config` | TEXT | yes |  |  |
| `view_only` | INTEGER | yes | `0` |  |
| `restricted_until` | TEXT | yes |  |  |
| `ack_required` | INTEGER | yes | `0` |  |
| `watermark` | INTEGER | yes | `0` |  |
| `include_test` | INTEGER | yes | `0` |  |
| `publish_at` | TEXT | yes |  |  |
| `expires_at` | TEXT | yes |  |  |
| `published_at` | TEXT | yes |  |  |
| `archived_at` | TEXT | yes |  |  |
| `visible_to_cs` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `updated_by` | INTEGER | yes |  |  |
| `reject_reason` | TEXT | yes |  |  |
| `is_test` | INTEGER | yes | `0` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `replace_reason` | TEXT | yes |  |  |
| `restored_from_id` | INTEGER | yes |  |  |

**Indexes:** `ix_documents_root`, `ix_documents_status`

### `erasure_requests`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `email` | TEXT | yes |  |  |
| `reason` | TEXT | yes |  |  |
| `status` | VARCHAR(24) | yes | `'pending'` |  |
| `requested_at` | TEXT | yes | `datetime('now')` |  |
| `due_at` | TEXT | yes |  |  |
| `acknowledged_at` | TEXT | yes |  |  |
| `reviewed_by` | INTEGER | yes |  |  |
| `reviewed_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `completed_at` | TEXT | yes |  |  |

**Indexes:** `ix_erasure_status`, `ix_erasure_user`

### `error_reports`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `reference` | VARCHAR(24) | no |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `page` | TEXT | yes |  |  |
| `category` | VARCHAR(40) | yes | `'general'` |  |
| `user_message` | TEXT | yes |  |  |
| `tech_summary` | TEXT | yes |  |  |
| `browser` | TEXT | yes |  |  |
| `os` | TEXT | yes |  |  |
| `app_version` | TEXT | yes |  |  |
| `related_type` | VARCHAR(32) | yes |  |  |
| `related_id` | INTEGER | yes |  |  |
| `status` | VARCHAR(16) | yes | `'open'` |  |
| `resolution_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_error_reports_user`

### `support_attachments`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `ticket_id` | INTEGER | no |  | FK → `tickets.id` |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `filename` | TEXT | no |  |  |
| `mime` | TEXT | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `sha256` | TEXT | yes |  |  |
| `data_uri` | TEXT | yes |  |  |
| `storage_ref` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_attach_ticket`

### `support_templates`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | TEXT | no |  |  |
| `body` | TEXT | no |  |  |
| `category` | VARCHAR(40) | yes |  |  |
| `active` | INTEGER | yes | `1` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `ticket_messages`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `ticket_id` | INTEGER | no |  | FK → `tickets.id` |
| `sender` | TEXT | no |  |  |
| `body` | TEXT | no |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_tmsg_ticket`

### `ticket_notes`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `ticket_id` | INTEGER | no |  | → `tickets` *(inferred)* |
| `admin_id` | INTEGER | no |  |  |
| `body` | TEXT | no |  |  |
| `mentions` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_ticket_notes_ticket`

### `tickets`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `reference` | TEXT | yes |  |  |
| `subject` | TEXT | no |  |  |
| `category` | TEXT | yes | `'General'` |  |
| `status` | TEXT | yes | `'open'` |  |
| `priority` | TEXT | yes | `'normal'` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `assigned_to` | INTEGER | yes |  |  |
| `tags` | TEXT | yes |  |  |
| `escalated` | INTEGER | yes | `0` |  |
| `first_response_at` | TEXT | yes |  |  |
| `resolved_at` | TEXT | yes |  |  |
| `rating` | INTEGER | yes |  |  |
| `followup_at` | TEXT | yes |  |  |

## Events

*2 tables*

### `event_registrations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event_id` | INTEGER | no |  | → `events` *(inferred)* |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `status` | TEXT | yes | `'registered'` |  |
| `registered_at` | TEXT | yes | `datetime('now')` |  |
| `attended_at` | TEXT | yes |  |  |
| `cpd_entry_id` | INTEGER | yes |  | → `cpd_entries` *(inferred)* |

**Indexes:** `ix_event_reg_user`, `ux_event_reg` (unique)

### `events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | TEXT | no |  |  |
| `summary` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `event_type` | TEXT | yes | `'webinar'` |  |
| `starts_at` | TEXT | yes |  |  |
| `ends_at` | TEXT | yes |  |  |
| `timezone` | TEXT | yes |  |  |
| `location` | TEXT | yes |  |  |
| `join_url` | TEXT | yes |  |  |
| `capacity` | INTEGER | yes | `0` |  |
| `cpd_hours` | REAL | yes | `0` |  |
| `cpd_category` | TEXT | yes | `'Events & webinars'` |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_events_status`

## Integrations & operations

*17 tables*

### `audit_logs`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `action` | TEXT | yes |  |  |
| `details` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

### `career_email_templates`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event_key` | VARCHAR(40) | no |  |  |
| `subject` | VARCHAR(300) | yes |  |  |
| `body` | TEXT | yes |  |  |
| `enabled` | INTEGER | yes | `1` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ux_career_tmpl_event` (unique)

### `career_taxonomy`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `kind` | VARCHAR(24) | no |  |  |
| `value` | VARCHAR(160) | no |  |  |
| `sort_order` | INTEGER | yes | `0` |  |
| `active` | INTEGER | yes | `1` |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_career_tax_kind`

### `certuvo_accounts`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `external_id` | TEXT | yes |  |  |
| `username` | TEXT | yes |  |  |
| `secret` | TEXT | yes |  |  |
| `login_url` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'pending'` |  |
| `last_error` | TEXT | yes |  |  |
| `provisioned_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `retry_count` | INTEGER | yes | `0` |  |
| `next_retry_at` | TEXT | yes |  |  |
| `suspended_at` | TEXT | yes |  |  |
| `revoked_at` | TEXT | yes |  |  |
| `activated_at` | TEXT | yes |  |  |
| `credentials_sent_at` | TEXT | yes |  |  |
| `idempotency_key` | TEXT | yes |  |  |
| `must_change_password` | INTEGER | yes | `1` |  |
| `email_conflict` | INTEGER | yes | `0` |  |
| `eligible_reason` | TEXT | yes |  |  |
| `member_type` | TEXT | yes |  |  |
| `username_regenerated_at` | TEXT | yes |  |  |
| `password_reset_at` | TEXT | yes |  |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |

### `founding_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `user_id` | INTEGER | no |  | → `users` *(inferred)* |
| `code_id` | INTEGER | no |  |  |
| `route` | TEXT | yes |  |  |
| `declared_experience_years` | INTEGER | yes |  |  |
| `declared_role` | TEXT | yes |  |  |
| `declared_qualification` | TEXT | yes |  |  |
| `evidence_ref` | TEXT | yes |  |  |
| `evidence_name` | TEXT | yes |  |  |
| `evidence_mime` | TEXT | yes |  |  |
| `evidence_size` | INTEGER | yes |  |  |
| `status` | TEXT | no | `'pending_review'` |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_founding_app_code`, `ix_founding_app_user`

### `honorary_application_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `doc_kind` | TEXT | yes | `'supporting'` |  |
| `filename` | TEXT | yes |  |  |
| `mime` | TEXT | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `sha256` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_honappdoc_app`

### `honorary_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `reference` | TEXT | no |  |  |
| `first_name` | TEXT | yes |  |  |
| `last_name` | TEXT | yes |  |  |
| `email` | TEXT | yes |  |  |
| `mobile` | TEXT | yes |  |  |
| `country` | TEXT | yes |  |  |
| `city` | TEXT | yes |  |  |
| `nationality` | TEXT | yes |  |  |
| `job_title` | TEXT | yes |  |  |
| `employer` | TEXT | yes |  |  |
| `years_experience` | INTEGER | yes |  |  |
| `industry` | TEXT | yes |  |  |
| `highest_qualification` | TEXT | yes |  |  |
| `professional_certifications` | TEXT | yes |  |  |
| `relevant_experience` | TEXT | yes |  |  |
| `professional_summary` | TEXT | yes |  |  |
| `declaration` | INTEGER | yes | `0` |  |
| `status` | TEXT | no | `'pending_review'` |  |
| `award_no` | TEXT | yes |  |  |
| `decided_by` | INTEGER | yes |  |  |
| `decided_at` | TEXT | yes |  |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `eligibility_confirmed` | INTEGER | yes | `0` |  |
| `terms_accepted` | INTEGER | yes | `0` |  |
| `terms_accepted_at` | TEXT | yes |  |  |
| `qualifications_json` | TEXT | yes |  |  |
| `certifications_json` | TEXT | yes |  |  |
| `experience_json` | TEXT | yes |  |  |
| `shortlisted` | INTEGER | yes | `0` |  |
| `shortlisted_at` | TEXT | yes |  |  |
| `idv_token` | VARCHAR(64) | yes |  |  |
| `idv_token_expires` | TEXT | yes |  |  |
| `idv_status` | VARCHAR(16) | yes | `'none'` |  |
| `idv_submitted_at` | TEXT | yes |  |  |
| `background_declaration` | INTEGER | yes | `0` |  |
| `background_declared_at` | TEXT | yes |  |  |
| `idv_deleted_at` | TEXT | yes |  |  |
| `certification_id` | INTEGER | yes |  | → `certifications` *(inferred)* |
| `suitability_note` | TEXT | yes |  |  |

**Indexes:** `ix_honapp_status`

### `honorary_awards`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `award_no` | TEXT | no |  |  |
| `recipient_name` | TEXT | no |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `citation` | TEXT | yes |  |  |
| `designation` | TEXT | yes | `'Honorary Fellow (PCI)'` |  |
| `status` | TEXT | yes | `'active'` |  |
| `conferred_by` | INTEGER | no |  |  |
| `conferred_at` | TEXT | yes | `datetime('now')` |  |
| `revoked_by` | INTEGER | yes |  |  |
| `revoked_at` | TEXT | yes |  |  |
| `revoke_reason` | TEXT | yes |  |  |
| `pdf_ref` | TEXT | yes |  |  |
| `pdf_sha256` | TEXT | yes |  |  |
| `pdf_generated_at` | TEXT | yes |  |  |

**Indexes:** `ix_honorary_user`

### `honorary_idv_documents`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `doc_kind` | VARCHAR(20) | yes |  |  |
| `filename` | TEXT | yes |  |  |
| `mime` | VARCHAR(80) | yes |  |  |
| `size_bytes` | INTEGER | yes |  |  |
| `storage_ref` | TEXT | yes |  |  |
| `sha256` | VARCHAR(64) | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_honidv_app`

### `integration_deliveries`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event_id` | INTEGER | no |  | → `events` *(inferred)* |
| `integration_id` | INTEGER | no |  | → `integrations` *(inferred)* |
| `status` | TEXT | no | `'pending'` |  |
| `attempts` | INTEGER | yes | `0` |  |
| `response_code` | INTEGER | yes |  |  |
| `last_error` | TEXT | yes |  |  |
| `next_attempt_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `lease_owner` | VARCHAR(64) | yes |  |  |
| `lease_until` | TEXT | yes |  |  |

**Indexes:** `ix_intdel_pending`, `ux_intdel_event_int` (unique)

### `integration_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `event_type` | TEXT | no |  |  |
| `entity_type` | TEXT | yes |  |  |
| `entity_id` | INTEGER | yes |  |  |
| `payload` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_intevent_created`

### `integrations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `provider` | TEXT | no | `'webhook'` |  |
| `name` | TEXT | yes |  |  |
| `enabled` | INTEGER | yes | `0` |  |
| `endpoint_url` | TEXT | yes |  |  |
| `secret` | TEXT | yes |  |  |
| `event_filter` | TEXT | yes |  |  |
| `config` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'idle'` |  |
| `last_delivery_at` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |

### `job_app_events`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `application_id` | INTEGER | no |  | → `pciworld_applications` *(inferred)* |
| `kind` | VARCHAR(16) | yes | `'note'` |  |
| `from_status` | VARCHAR(24) | yes |  |  |
| `to_status` | VARCHAR(24) | yes |  |  |
| `body` | TEXT | yes |  |  |
| `scheduled_at` | VARCHAR(40) | yes |  |  |
| `actor_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `actor_name` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |

**Indexes:** `ix_jobappev_app`

### `job_applications`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `job_id` | INTEGER | no |  |  |
| `name` | TEXT | yes |  |  |
| `email` | VARCHAR(190) | yes |  |  |
| `phone` | TEXT | yes |  |  |
| `cover_message` | TEXT | yes |  |  |
| `cv_ref` | TEXT | yes |  |  |
| `cv_name` | TEXT | yes |  |  |
| `status` | TEXT | yes | `'new'` |  |
| `admin_note` | TEXT | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `answers_json` | TEXT | yes |  |  |
| `reference` | VARCHAR(24) | yes |  |  |
| `user_id` | INTEGER | yes |  | → `users` *(inferred)* |
| `assigned_to` | INTEGER | yes |  |  |

**Indexes:** `ix_jobapp_job`, `ux_jobapp_email` (unique)

### `job_postings`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `title` | TEXT | no |  |  |
| `organisation` | TEXT | yes |  |  |
| `location` | TEXT | yes |  |  |
| `employment_type` | TEXT | yes | `'full_time'` |  |
| `remote_type` | TEXT | yes | `'onsite'` |  |
| `sector` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `requirements` | TEXT | yes |  |  |
| `responsibilities` | TEXT | yes |  |  |
| `salary_min` | DECIMAL(12,2) | yes |  |  |
| `salary_max` | DECIMAL(12,2) | yes |  |  |
| `salary_currency` | TEXT | yes | `'USD'` |  |
| `salary_period` | TEXT | yes | `'year'` |  |
| `apply_method` | TEXT | yes | `'inplatform'` |  |
| `apply_url` | TEXT | yes |  |  |
| `apply_email` | TEXT | yes |  |  |
| `featured` | INTEGER | yes | `0` |  |
| `status` | VARCHAR(24) | yes | `'draft'` |  |
| `posted_at` | TEXT | yes |  |  |
| `closes_at` | TEXT | yes |  |  |
| `created_by` | INTEGER | yes |  |  |
| `created_at` | TEXT | yes | `datetime('now')` |  |
| `updated_at` | TEXT | yes | `datetime('now')` |  |
| `job_code` | VARCHAR(32) | yes |  |  |
| `country` | VARCHAR(64) | yes |  |  |
| `department` | VARCHAR(120) | yes |  |  |
| `experience_level` | VARCHAR(40) | yes |  |  |
| `vacancies` | INTEGER | yes | `1` |  |
| `benefits` | TEXT | yes |  |  |
| `education` | TEXT | yes |  |  |
| `languages` | TEXT | yes |  |  |
| `certifications` | TEXT | yes |  |  |
| `reporting_line` | VARCHAR(160) | yes |  |  |
| `expected_start` | VARCHAR(40) | yes |  |  |
| `application_instructions` | TEXT | yes |  |  |
| `eo_statement` | TEXT | yes |  |  |
| `salary_visible` | INTEGER | yes | `1` |  |
| `urgent` | INTEGER | yes | `0` |  |
| `publish_at` | VARCHAR(40) | yes |  |  |
| `slug` | VARCHAR(200) | yes |  |  |

**Indexes:** `ix_jobs_status`, `ux_jobs_code` (unique)

### `job_questions`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `id` | INTEGER | yes |  | PK |
| `job_id` | INTEGER | no |  |  |
| `qtype` | VARCHAR(24) | yes | `'short_text'` |  |
| `label` | TEXT | no |  |  |
| `options` | TEXT | yes |  |  |
| `required` | INTEGER | yes | `0` |  |
| `sort_order` | INTEGER | yes | `0` |  |

**Indexes:** `ix_jobq_job`

### `schema_migrations`

| Column | Type | Null | Default | Key |
|---|---|---|---|---|
| `version` | INTEGER | yes |  | PK |
| `checksum` | TEXT | yes |  |  |
| `description` | TEXT | yes |  |  |
| `applied_at` | TEXT | yes | `datetime('now')` |  |
