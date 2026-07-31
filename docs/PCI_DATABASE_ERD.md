# PCI Platform — Entity-Relationship Diagrams

One diagram per domain, because a single diagram of 290 tables is a picture of nothing.
Generated from the live schema alongside the full column reference in
[`PCI_DATABASE_SCHEMA.md`](PCI_DATABASE_SCHEMA.md).

**How to read these**

- Solid `||--o{` — a **declared** foreign key. There are only 19 in the whole schema.
- Dotted `}o..o{` — an **inferred** relationship: a `<thing>_id` column pointing at a
  plausible table. Real in the code, **not enforced by the database**.
- Only join-bearing columns are drawn. Every column of every table is in the schema
  reference; repeating them here would make the diagrams unreadable.

> Because integrity is enforced in application code, the arrows tell you what the code
> intends — not what the database guarantees. Both facts matter when you write a migration
> or a manual fix.

---

## PCI World — community & moderation

*15 tables in this domain*

```mermaid
erDiagram
    pciworld_challenges {
        INTEGER id
        VARCHAR code
        VARCHAR status
        INTEGER author_id
        TEXT created_at
    }
    pciworld_community_eligibility_log {
        INTEGER id
        INTEGER guest_session_id
        VARCHAR created_at
    }
    pciworld_community_media {
        INTEGER id
        INTEGER room_id
        INTEGER guest_session_id
        INTEGER world_user_id
        INTEGER message_id
        VARCHAR client_upload_id
        VARCHAR state
    }
    pciworld_community_messages {
        INTEGER id
        INTEGER room_id
        INTEGER guest_session_id
        INTEGER world_user_id
        VARCHAR client_message_id
        INTEGER reply_to_message_id
        VARCHAR status
    }
    pciworld_community_rooms {
        INTEGER id
        VARCHAR slug
        VARCHAR state
        INTEGER policy_version_id
        VARCHAR created_at
    }
    pciworld_guest_sessions {
        INTEGER id
        INTEGER room_id
        VARCHAR status
        VARCHAR created_at
    }
    pciworld_moderation_case_events {
        INTEGER id
        INTEGER case_id
        INTEGER actor_id
        VARCHAR correlation_id
        VARCHAR created_at
    }
    pciworld_moderation_decisions {
        INTEGER id
        INTEGER policy_version_id
        INTEGER rule_id
        VARCHAR correlation_id
        VARCHAR created_at
    }
    pciworld_policy_rules {
        INTEGER id
        INTEGER policy_version_id
    }
    pciworld_policy_versions {
        INTEGER id
        VARCHAR status
        VARCHAR created_at
    }
    pciworld_reports {
        INTEGER id
        INTEGER challenge_id
        INTEGER session_id
        VARCHAR status
        TEXT created_at
    }
    pciworld_sessions {
        INTEGER id
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    pciworld_guest_sessions }o..o{ pciworld_community_eligibility_log : "guest_session_id"
    pciworld_guest_sessions }o..o{ pciworld_community_media : "guest_session_id"
    pciworld_policy_versions }o..o{ pciworld_community_media : "policy_version_id"
    pciworld_guest_sessions }o..o{ pciworld_community_messages : "guest_session_id"
    pciworld_policy_versions }o..o{ pciworld_community_rooms : "policy_version_id"
    users }o..o{ pciworld_moderation_case_events : "actor_id"
    pciworld_policy_versions }o..o{ pciworld_moderation_decisions : "policy_version_id"
    pciworld_policy_versions }o..o{ pciworld_policy_rules : "policy_version_id"
    pciworld_challenges }o..o{ pciworld_reports : "challenge_id"
    pciworld_sessions }o..o{ pciworld_reports : "session_id"
```

**Drawn from other domains as anchors:** `pciworld_challenges` *(PCI World — challenges, rotation & intelligence)*, `pciworld_sessions` *(PCI World — identity, passport & admin)*, `users` *(Students & identity)*

**Tables in this domain:** `pciworld_community_eligibility_log`, `pciworld_community_media`, `pciworld_community_messages`, `pciworld_community_outbox`, `pciworld_community_reports`, `pciworld_community_rooms`, `pciworld_guest_sessions`, `pciworld_media_scan_queue`, `pciworld_media_scans`, `pciworld_moderation_case_events`, `pciworld_moderation_cases`, `pciworld_moderation_decisions`, `pciworld_policy_rules`, `pciworld_policy_versions`, `pciworld_reports`

---

## PCI World — forum

*9 tables in this domain*

```mermaid
erDiagram
    pciworld_forum_standing {
        INTEGER user_id
        VARCHAR created_at
    }
    pciworld_forum_standing_events {
        INTEGER id
        INTEGER user_id
        INTEGER actor_admin_id
        VARCHAR created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    users }o..o{ pciworld_forum_standing : "user_id"
    users }o..o{ pciworld_forum_standing_events : "user_id"
```

**Drawn from other domains as anchors:** `users` *(Students & identity)*

**Tables in this domain:** `pciworld_forum_categories`, `pciworld_forum_flags`, `pciworld_forum_post_revisions`, `pciworld_forum_posts`, `pciworld_forum_standing`, `pciworld_forum_standing_events`, `pciworld_forum_tags`, `pciworld_forum_thread_tags`, `pciworld_forum_threads`

---

## Forum (platform)

*3 tables in this domain*

```mermaid
erDiagram
    forum_posts {
        INTEGER id
        INTEGER thread_id
        VARCHAR status
        TEXT created_at
    }
    forum_threads {
        INTEGER id
        VARCHAR status
        TEXT created_at
    }
    forum_threads }o..o{ forum_posts : "thread_id"
```

**Tables in this domain:** `forum_actions`, `forum_posts`, `forum_threads`

---

## PCI World — careers

*8 tables in this domain*

```mermaid
erDiagram
    pciworld_application_answers {
        INTEGER id
        INTEGER application_id
        INTEGER question_id
        VARCHAR created_at
    }
    pciworld_application_consents {
        INTEGER id
        INTEGER application_id
        INTEGER employer_id
        VARCHAR created_at
    }
    pciworld_application_events {
        INTEGER id
        INTEGER application_id
        INTEGER actor_id
        VARCHAR created_at
    }
    pciworld_applications {
        INTEGER id
        INTEGER posting_id
        INTEGER applicant_user_id
        VARCHAR state
        VARCHAR created_at
    }
    pciworld_employer_members {
        INTEGER id
        INTEGER employer_id
        INTEGER user_id
        VARCHAR created_at
    }
    pciworld_employers {
        INTEGER id
        VARCHAR slug
        VARCHAR state
        INTEGER verified_by_admin_id
        VARCHAR created_at
    }
    pciworld_job_postings {
        INTEGER id
        INTEGER employer_id
        VARCHAR slug
        VARCHAR state
        VARCHAR created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    pciworld_applications }o..o{ pciworld_application_answers : "application_id"
    pciworld_applications }o..o{ pciworld_application_consents : "application_id"
    pciworld_employers }o..o{ pciworld_application_consents : "employer_id"
    pciworld_applications }o..o{ pciworld_application_events : "application_id"
    users }o..o{ pciworld_application_events : "actor_id"
    pciworld_employers }o..o{ pciworld_employer_members : "employer_id"
    users }o..o{ pciworld_employer_members : "user_id"
    pciworld_employers }o..o{ pciworld_job_postings : "employer_id"
```

**Drawn from other domains as anchors:** `users` *(Students & identity)*

**Tables in this domain:** `pciworld_application_answers`, `pciworld_application_consents`, `pciworld_application_events`, `pciworld_applications`, `pciworld_employer_members`, `pciworld_employers`, `pciworld_job_postings`, `pciworld_job_questions`

---

## PCI World — editorial & contributors

*9 tables in this domain*

```mermaid
erDiagram
    pciworld_article_reviews {
        INTEGER id
        INTEGER article_id
        INTEGER reviewer_id
        TEXT created_at
    }
    pciworld_article_sources {
        INTEGER id
        INTEGER article_id
        INTEGER source_id
        TEXT created_at
    }
    pciworld_article_versions {
        INTEGER id
        INTEGER article_id
        TEXT created_at
    }
    pciworld_articles {
        INTEGER id
        VARCHAR slug
        VARCHAR status
        INTEGER author_id
        TEXT created_at
        INTEGER contributor_user_id
    }
    pciworld_contributor_assignments {
        INTEGER id
        INTEGER article_id
        INTEGER editor_admin_id
        VARCHAR created_at
    }
    pciworld_contributor_events {
        INTEGER id
        INTEGER article_id
        INTEGER actor_id
        VARCHAR created_at
    }
    pciworld_contributor_messages {
        INTEGER id
        INTEGER article_id
        INTEGER sender_id
        VARCHAR created_at
    }
    pciworld_contributors {
        INTEGER id
        INTEGER user_id
        VARCHAR state
        INTEGER granted_by_admin_id
        INTEGER revoked_by_admin_id
        VARCHAR created_at
    }
    pciworld_sources {
        INTEGER id
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    pciworld_articles }o..o{ pciworld_article_reviews : "article_id"
    users }o..o{ pciworld_article_reviews : "reviewer_id"
    pciworld_articles }o..o{ pciworld_article_sources : "article_id"
    pciworld_sources }o..o{ pciworld_article_sources : "source_id"
    pciworld_articles }o..o{ pciworld_article_versions : "article_id"
    users }o..o{ pciworld_articles : "author_id"
    pciworld_articles }o..o{ pciworld_contributor_assignments : "article_id"
    pciworld_articles }o..o{ pciworld_contributor_events : "article_id"
    users }o..o{ pciworld_contributor_events : "actor_id"
    pciworld_articles }o..o{ pciworld_contributor_messages : "article_id"
    users }o..o{ pciworld_contributors : "user_id"
```

**Drawn from other domains as anchors:** `users` *(Students & identity)*

**Tables in this domain:** `pciworld_article_reviews`, `pciworld_article_sources`, `pciworld_article_versions`, `pciworld_articles`, `pciworld_contributor_assignments`, `pciworld_contributor_events`, `pciworld_contributor_messages`, `pciworld_contributors`, `pciworld_sources`

---

## PCI World — challenges, rotation & intelligence

*7 tables in this domain*

```mermaid
erDiagram
    pciworld_attempts {
        INTEGER id
        INTEGER session_id
        INTEGER challenge_id
        VARCHAR status
        INTEGER invite_id
        INTEGER parent_attempt_id
        INTEGER user_id
    }
    pciworld_challenge_versions {
        INTEGER id
        INTEGER challenge_id
        TEXT created_at
    }
    pciworld_challenges {
        INTEGER id
        VARCHAR code
        VARCHAR status
        INTEGER author_id
        TEXT created_at
    }
    pciworld_invites {
        INTEGER id
        INTEGER attempt_id
        TEXT created_at
    }
    pciworld_rotation_order {
        INTEGER id
        INTEGER challenge_id
        TEXT created_at
    }
    pciworld_rotation_periods {
        INTEGER id
        INTEGER challenge_id
    }
    pciworld_sessions {
        INTEGER id
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    pciworld_sessions }o..o{ pciworld_attempts : "session_id"
    pciworld_challenges }o..o{ pciworld_attempts : "challenge_id"
    pciworld_invites }o..o{ pciworld_attempts : "invite_id"
    users }o..o{ pciworld_attempts : "user_id"
    pciworld_rotation_periods }o..o{ pciworld_attempts : "rotation_period_id"
    pciworld_challenges }o..o{ pciworld_challenge_versions : "challenge_id"
    users }o..o{ pciworld_challenges : "author_id"
    pciworld_challenges }o..o{ pciworld_rotation_order : "challenge_id"
    pciworld_challenges }o..o{ pciworld_rotation_periods : "challenge_id"
```

**Drawn from other domains as anchors:** `pciworld_invites` *(PCI World — identity, passport & admin)*, `pciworld_sessions` *(PCI World — identity, passport & admin)*, `users` *(Students & identity)*

**Tables in this domain:** `pciworld_attempts`, `pciworld_challenge_versions`, `pciworld_challenges`, `pciworld_rotation_lock`, `pciworld_rotation_order`, `pciworld_rotation_periods`, `pciworld_rotation_runs`

---

## PCI World — identity, passport & admin

*23 tables in this domain*

```mermaid
erDiagram
    pciworld_applications {
        INTEGER id
        INTEGER posting_id
        INTEGER applicant_user_id
        VARCHAR state
        VARCHAR created_at
    }
    pciworld_calendar {
        INTEGER id
        INTEGER challenge_id
        TEXT created_at
    }
    pciworld_challenges {
        INTEGER id
        VARCHAR code
        VARCHAR status
        INTEGER author_id
        TEXT created_at
    }
    pciworld_cv_access_log {
        INTEGER id
        INTEGER application_id
        INTEGER employer_id
        INTEGER actor_id
        VARCHAR created_at
    }
    pciworld_employers {
        INTEGER id
        VARCHAR slug
        VARCHAR state
        INTEGER verified_by_admin_id
        VARCHAR created_at
    }
    pciworld_events {
        INTEGER id
        INTEGER challenge_id
        INTEGER session_id
        VARCHAR created_at
    }
    pciworld_participants {
        INTEGER id
        INTEGER user_id
        VARCHAR status
        TEXT created_at
    }
    pciworld_referrals {
        INTEGER id
        INTEGER anonymous_world_session_id
        INTEGER referred_user_id
        INTEGER challenge_id
        TEXT created_at
    }
    pciworld_sessions {
        INTEGER id
        TEXT created_at
    }
    pciworld_user_sessions {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    pciworld_user_tokens {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    pciworld_challenges }o..o{ pciworld_calendar : "challenge_id"
    pciworld_applications }o..o{ pciworld_cv_access_log : "application_id"
    pciworld_employers }o..o{ pciworld_cv_access_log : "employer_id"
    users }o..o{ pciworld_cv_access_log : "actor_id"
    pciworld_challenges }o..o{ pciworld_events : "challenge_id"
    pciworld_sessions }o..o{ pciworld_events : "session_id"
    users }o..o{ pciworld_participants : "user_id"
    pciworld_challenges }o..o{ pciworld_referrals : "challenge_id"
    users }o..o{ pciworld_user_sessions : "user_id"
    users }o..o{ pciworld_user_tokens : "user_id"
```

**Drawn from other domains as anchors:** `pciworld_applications` *(PCI World — careers)*, `pciworld_challenges` *(PCI World — challenges, rotation & intelligence)*, `pciworld_employers` *(PCI World — careers)*, `users` *(Students & identity)*

**Tables in this domain:** `pciworld_admin_sessions`, `pciworld_admin_users`, `pciworld_appeals`, `pciworld_audit`, `pciworld_calendar`, `pciworld_cv_access_log`, `pciworld_entities`, `pciworld_entity_mentions`, `pciworld_events`, `pciworld_handoff_codes`, `pciworld_invites`, `pciworld_oauth_clients`, `pciworld_oauth_codes`, `pciworld_participants`, `pciworld_referrals`, `pciworld_restricted_evidence`, `pciworld_risk_restrictions`, `pciworld_sanctions`, `pciworld_sessions`, `pciworld_user_map`, `pciworld_user_sessions`, `pciworld_user_tokens`, `pciworld_users`

---

## Students & identity

*22 tables in this domain*

```mermaid
erDiagram
    account_requests {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
    }
    admin_sessions {
        INTEGER id
        INTEGER admin_id
        TEXT created_at
    }
    admin_users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    candidate_consents {
        INTEGER id
        INTEGER user_id
    }
    enrollment_sessions {
        INTEGER id
        TEXT email
        INTEGER user_id
        TEXT created_at
    }
    fraud_flags {
        INTEGER id
        INTEGER code_id
        INTEGER user_id
        TEXT email
        VARCHAR status
        TEXT created_at
    }
    identity_checks {
        INTEGER id
        INTEGER attempt_id
        INTEGER user_id
        TEXT created_at
    }
    identity_documents {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
    }
    impersonation_events {
        INTEGER id
        INTEGER session_id
    }
    impersonation_sessions {
        INTEGER id
        INTEGER admin_id
        INTEGER user_id
    }
    login_events {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    login_tokens {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    membership_upgrades {
        INTEGER id
        INTEGER user_id
        VARCHAR status
        TEXT created_at
    }
    memberships {
        INTEGER id
        INTEGER user_id
        TEXT status
        VARCHAR stripe_customer_id
        VARCHAR stripe_subscription_id
    }
    pciworld_attempts {
        INTEGER id
        INTEGER session_id
        INTEGER challenge_id
        VARCHAR status
        INTEGER invite_id
        INTEGER parent_attempt_id
        INTEGER user_id
    }
    qualifications {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    security_events {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    student_profiles {
        INTEGER id
        INTEGER user_id
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    work_experiences {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    users }o..o{ account_requests : "user_id"
    admin_users ||--o{ admin_sessions : "admin_id"
    users }o..o{ candidate_consents : "user_id"
    users ||--o{ enrollment_sessions : "user_id"
    users }o..o{ fraud_flags : "user_id"
    pciworld_attempts }o..o{ identity_checks : "attempt_id"
    users }o..o{ identity_checks : "user_id"
    users }o..o{ identity_documents : "user_id"
    impersonation_sessions }o..o{ impersonation_events : "session_id"
    users }o..o{ impersonation_sessions : "user_id"
    users }o..o{ login_events : "user_id"
    users ||--o{ login_tokens : "user_id"
    users }o..o{ membership_upgrades : "user_id"
    users ||--o{ memberships : "user_id"
    users }o..o{ qualifications : "user_id"
    users }o..o{ security_events : "user_id"
    users ||--o{ student_profiles : "user_id"
    users }o..o{ work_experiences : "user_id"
```

**Drawn from other domains as anchors:** `pciworld_attempts` *(PCI World — challenges, rotation & intelligence)*

**Tables in this domain:** `account_requests`, `admin_reset_tokens`, `admin_sessions`, `admin_users`, `candidate_consents`, `enrollment_sessions`, `fraud_flags`, `identity_checks`, `identity_documents`, `impersonation_events`, `impersonation_sessions`, `login_events`, `login_tokens`, `membership_upgrades`, `memberships`, `pci_identity_merges`, `pci_student_number_registry`, `qualifications`, `security_events`, `student_profiles`, `users`, `work_experiences`

---

## Examinations & credentials

*31 tables in this domain*

```mermaid
erDiagram
    cert_document_downloads {
        INTEGER id
        INTEGER cert_document_id
        INTEGER user_id
        VARCHAR copy_id
        TEXT created_at
    }
    cert_document_versions {
        INTEGER id
        INTEGER cert_document_id
        INTEGER restored_from_id
        TEXT created_at
    }
    cert_documents {
        INTEGER id
        INTEGER certification_id
        TEXT created_at
    }
    certificate_downloads {
        INTEGER id
        VARCHAR credential_id
        INTEGER user_id
        TEXT created_at
    }
    certification_applications {
        INTEGER id
        INTEGER user_id
        INTEGER certification_id
        VARCHAR status
        TEXT created_at
    }
    certification_routes {
        INTEGER id
        INTEGER certification_id
        TEXT created_at
    }
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    exam_attempt_grants {
        INTEGER id
        INTEGER authorization_id
        INTEGER user_id
        INTEGER certification_id
        INTEGER incident_id
        INTEGER payment_id
        TEXT status
    }
    exam_attempts {
        INTEGER id
        INTEGER user_id
        INTEGER booking_id
        INTEGER certification_id
        TEXT status
        INTEGER authorization_id
        INTEGER grant_id
    }
    exam_authorizations {
        INTEGER id
        INTEGER user_id
        INTEGER certification_id
        INTEGER payment_id
        INTEGER entitlement_id
        INTEGER institution_id
        TEXT status
    }
    exam_bookings {
        INTEGER id
        INTEGER user_id
        INTEGER payment_id
        INTEGER certification_id
        TEXT status
        TEXT created_at
        INTEGER authorization_id
    }
    exam_delivery_orders {
        INTEGER id
        INTEGER provider_id
        INTEGER user_id
        INTEGER booking_id
        INTEGER attempt_id
        INTEGER certification_id
        VARCHAR status
    }
    exam_entitlements {
        INTEGER id
        INTEGER user_id
        INTEGER payment_id
        INTEGER certification_id
        TEXT status
        INTEGER booking_id
        INTEGER attempt_id
    }
    exam_evidence {
        INTEGER id
        INTEGER attempt_id
        INTEGER user_id
        TEXT created_at
    }
    exam_extension_history {
        INTEGER id
        INTEGER authorization_id
        INTEGER user_id
        INTEGER certification_id
    }
    exam_incidents {
        INTEGER id
        INTEGER user_id
        INTEGER certification_id
        INTEGER attempt_id
        INTEGER booking_id
        INTEGER authorization_id
        VARCHAR status
    }
    payments {
        INTEGER id
        INTEGER user_id
        INTEGER enrollment_session_id
        TEXT provider_payment_id
        TEXT created_at
        INTEGER discount_code_id
        INTEGER partner_id
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    cert_documents }o..o{ cert_document_downloads : "cert_document_id"
    users }o..o{ cert_document_downloads : "user_id"
    cert_documents }o..o{ cert_document_versions : "cert_document_id"
    certifications }o..o{ cert_documents : "certification_id"
    users }o..o{ certificate_downloads : "user_id"
    users }o..o{ certification_applications : "user_id"
    certifications }o..o{ certification_applications : "certification_id"
    certifications }o..o{ certification_routes : "certification_id"
    exam_authorizations }o..o{ exam_attempt_grants : "authorization_id"
    users }o..o{ exam_attempt_grants : "user_id"
    certifications }o..o{ exam_attempt_grants : "certification_id"
    exam_incidents }o..o{ exam_attempt_grants : "incident_id"
    payments }o..o{ exam_attempt_grants : "payment_id"
    certifications ||--o{ exam_attempts : "certification_id"
    users }o..o{ exam_attempts : "user_id"
    exam_bookings }o..o{ exam_attempts : "booking_id"
    exam_authorizations }o..o{ exam_attempts : "authorization_id"
    users }o..o{ exam_authorizations : "user_id"
    certifications }o..o{ exam_authorizations : "certification_id"
    payments }o..o{ exam_authorizations : "payment_id"
    exam_entitlements }o..o{ exam_authorizations : "entitlement_id"
    certifications ||--o{ exam_bookings : "certification_id"
    users }o..o{ exam_bookings : "user_id"
    payments }o..o{ exam_bookings : "payment_id"
    exam_authorizations }o..o{ exam_bookings : "authorization_id"
    users }o..o{ exam_delivery_orders : "user_id"
    exam_bookings }o..o{ exam_delivery_orders : "booking_id"
    exam_attempts }o..o{ exam_delivery_orders : "attempt_id"
    certifications }o..o{ exam_delivery_orders : "certification_id"
    certifications ||--o{ exam_entitlements : "certification_id"
    payments ||--o{ exam_entitlements : "payment_id"
    users }o..o{ exam_entitlements : "user_id"
    exam_bookings }o..o{ exam_entitlements : "booking_id"
    exam_attempts }o..o{ exam_entitlements : "attempt_id"
    exam_authorizations }o..o{ exam_entitlements : "authorization_id"
    exam_attempts }o..o{ exam_evidence : "attempt_id"
    users }o..o{ exam_evidence : "user_id"
    exam_authorizations }o..o{ exam_extension_history : "authorization_id"
    users }o..o{ exam_extension_history : "user_id"
    certifications }o..o{ exam_extension_history : "certification_id"
    users }o..o{ exam_incidents : "user_id"
    certifications }o..o{ exam_incidents : "certification_id"
    exam_attempts }o..o{ exam_incidents : "attempt_id"
    exam_bookings }o..o{ exam_incidents : "booking_id"
```

**Drawn from other domains as anchors:** `payments` *(Payments, finance & partners)*, `users` *(Students & identity)*

> 23 further relationship(s) omitted for legibility. The complete set is in the column reference.

**Tables in this domain:** `bok_domains`, `cert_document_downloads`, `cert_document_versions`, `cert_documents`, `certificate_downloads`, `certification_applications`, `certification_routes`, `certifications`, `exam_attempt_grants`, `exam_attempts`, `exam_authorizations`, `exam_bookings`, `exam_delivery_log`, `exam_delivery_orders`, `exam_delivery_providers`, `exam_entitlements`, `exam_evidence`, `exam_extension_history`, `exam_incidents`, `exam_launch_codes`, `exam_readiness_checks`, `exam_reschedule_history`, `exam_score_snapshots`, `exam_window_rules`, `governance_roles`, `held_certifications`, `issued_credentials`, `practice_attempts`, `proctor_events`, `proctor_messages`, `sample_questions`

---

## Payments, finance & partners

*25 tables in this domain*

```mermaid
erDiagram
    appeals {
        INTEGER id
        INTEGER user_id
        INTEGER attempt_id
        TEXT credential_id
        TEXT status
    }
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    checkout_reservations {
        INTEGER id
        VARCHAR email
        INTEGER certification_id
        INTEGER discount_code_id
        INTEGER partner_id
        VARCHAR stripe_session_id
        VARCHAR status
    }
    code_redemptions {
        INTEGER id
        INTEGER code_id
        TEXT code
        INTEGER user_id
        TEXT email
        INTEGER payment_id
    }
    discount_codes {
        INTEGER id
        TEXT code
        INTEGER owner_user_id
        TEXT batch_id
        INTEGER partner_id
        VARCHAR status
        INTEGER certification_id
    }
    fee_waivers {
        INTEGER id
        INTEGER user_id
        INTEGER certification_id
        INTEGER code_id
        INTEGER payment_id
        VARCHAR status
        TEXT created_at
    }
    partner_agreements {
        INTEGER id
        INTEGER partner_id
        VARCHAR status
        TEXT created_at
    }
    partner_commission_events {
        INTEGER id
        INTEGER transaction_id
        INTEGER actor_id
        TEXT created_at
    }
    partner_commission_rules {
        INTEGER id
        INTEGER agreement_id
        INTEGER partner_id
        INTEGER certification_id
        TEXT created_at
    }
    partner_commission_transactions {
        INTEGER id
        INTEGER partner_id
        INTEGER agreement_id
        INTEGER commission_rule_id
        INTEGER discount_code_id
        INTEGER code_redemption_id
        INTEGER payment_id
    }
    partner_dispute_messages {
        INTEGER id
        INTEGER dispute_id
        INTEGER author_id
        TEXT created_at
    }
    partner_disputes {
        INTEGER id
        INTEGER partner_id
        INTEGER transaction_id
        INTEGER settlement_id
        VARCHAR status
        INTEGER adjustment_transaction_id
        INTEGER raised_by_partner_user_id
    }
    partner_sessions {
        INTEGER id
        INTEGER partner_user_id
        TEXT created_at
    }
    partner_settlement_items {
        INTEGER id
        INTEGER settlement_id
        INTEGER transaction_id
        TEXT created_at
    }
    partner_settlements {
        INTEGER id
        INTEGER partner_id
        VARCHAR status
        INTEGER legacy_payout_id
        TEXT created_at
    }
    partner_sponsorships {
        INTEGER id
        INTEGER partner_id
        INTEGER user_id
        INTEGER application_id
        INTEGER certification_id
        TEXT status
        TEXT created_at
    }
    partner_users {
        INTEGER id
        INTEGER partner_id
        VARCHAR email
        VARCHAR status
        TEXT created_at
    }
    payments {
        INTEGER id
        INTEGER user_id
        INTEGER enrollment_session_id
        TEXT provider_payment_id
        TEXT created_at
        INTEGER discount_code_id
        INTEGER partner_id
    }
    pciworld_applications {
        INTEGER id
        INTEGER posting_id
        INTEGER applicant_user_id
        VARCHAR state
        VARCHAR created_at
    }
    training_partner_application_documents {
        INTEGER id
        INTEGER application_id
        TEXT created_at
    }
    training_partner_applications {
        INTEGER id
        TEXT status
        INTEGER partner_id
        TEXT created_at
    }
    training_partners {
        INTEGER id
        TEXT slug
        INTEGER source_application_id
        TEXT created_at
        VARCHAR status
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    certifications }o..o{ checkout_reservations : "certification_id"
    discount_codes }o..o{ checkout_reservations : "discount_code_id"
    payments }o..o{ checkout_reservations : "payment_id"
    users }o..o{ code_redemptions : "user_id"
    payments }o..o{ code_redemptions : "payment_id"
    certifications }o..o{ discount_codes : "certification_id"
    users }o..o{ fee_waivers : "user_id"
    certifications }o..o{ fee_waivers : "certification_id"
    payments }o..o{ fee_waivers : "payment_id"
    appeals }o..o{ fee_waivers : "appeal_id"
    users }o..o{ partner_commission_events : "actor_id"
    partner_agreements }o..o{ partner_commission_rules : "agreement_id"
    certifications }o..o{ partner_commission_rules : "certification_id"
    partner_agreements }o..o{ partner_commission_transactions : "agreement_id"
    partner_commission_rules }o..o{ partner_commission_transactions : "commission_rule_id"
    discount_codes }o..o{ partner_commission_transactions : "discount_code_id"
    code_redemptions }o..o{ partner_commission_transactions : "code_redemption_id"
    payments }o..o{ partner_commission_transactions : "payment_id"
    users }o..o{ partner_commission_transactions : "user_id"
    certifications }o..o{ partner_commission_transactions : "certification_id"
    partner_disputes }o..o{ partner_dispute_messages : "dispute_id"
    users }o..o{ partner_dispute_messages : "author_id"
    partner_settlements }o..o{ partner_disputes : "settlement_id"
    partner_users }o..o{ partner_sessions : "partner_user_id"
    partner_settlements }o..o{ partner_settlement_items : "settlement_id"
    users }o..o{ partner_sponsorships : "user_id"
    pciworld_applications }o..o{ partner_sponsorships : "application_id"
    certifications }o..o{ partner_sponsorships : "certification_id"
    users ||--o{ payments : "user_id"
    discount_codes }o..o{ payments : "discount_code_id"
    pciworld_applications }o..o{ training_partner_application_documents : "application_id"
    training_partners }o..o{ training_partner_applications : "partner_id"
```

**Drawn from other domains as anchors:** `appeals` *(Support, casework & documents)*, `certifications` *(Examinations & credentials)*, `pciworld_applications` *(PCI World — careers)*, `users` *(Students & identity)*

**Tables in this domain:** `checkout_reservations`, `code_redemptions`, `discount_codes`, `fee_waivers`, `partner_agreements`, `partner_campaign_links`, `partner_commission_events`, `partner_commission_rules`, `partner_commission_transactions`, `partner_dispute_messages`, `partner_disputes`, `partner_link_clicks`, `partner_notices`, `partner_payouts`, `partner_sessions`, `partner_settlement_items`, `partner_settlements`, `partner_sponsorships`, `partner_users`, `payments`, `pricing_rules`, `training_partner_application_documents`, `training_partner_applications`, `training_partners`, `webhook_events`

---

## Simulation Lab

*6 tables in this domain*

```mermaid
erDiagram
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    simulation_attempt_events {
        INTEGER id
        INTEGER attempt_id
        INTEGER user_id
        TEXT created_at
    }
    simulation_attempts {
        INTEGER id
        INTEGER user_id
        INTEGER scenario_id
        VARCHAR status
        TEXT created_at
    }
    simulation_competency {
        INTEGER id
        INTEGER attempt_id
        INTEGER user_id
        TEXT created_at
    }
    simulation_entitlements {
        INTEGER id
        INTEGER user_id
        VARCHAR status
        TEXT created_at
    }
    simulation_scenario_versions {
        INTEGER id
        INTEGER scenario_id
        TEXT created_at
    }
    simulation_scenarios {
        INTEGER id
        INTEGER certification_id
        VARCHAR status
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    simulation_attempts }o..o{ simulation_attempt_events : "attempt_id"
    users }o..o{ simulation_attempt_events : "user_id"
    users }o..o{ simulation_attempts : "user_id"
    simulation_scenarios }o..o{ simulation_attempts : "scenario_id"
    simulation_attempts }o..o{ simulation_competency : "attempt_id"
    users }o..o{ simulation_competency : "user_id"
    users }o..o{ simulation_entitlements : "user_id"
    simulation_scenarios }o..o{ simulation_scenario_versions : "scenario_id"
    certifications }o..o{ simulation_scenarios : "certification_id"
```

**Drawn from other domains as anchors:** `certifications` *(Examinations & credentials)*, `users` *(Students & identity)*

**Tables in this domain:** `simulation_attempt_events`, `simulation_attempts`, `simulation_competency`, `simulation_entitlements`, `simulation_scenario_versions`, `simulation_scenarios`

---

## Content, website & SEO

*40 tables in this domain*

```mermaid
erDiagram
    admin_users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    blog_authors {
        INTEGER id
        VARCHAR slug
        VARCHAR email
        INTEGER admin_user_id
        TEXT created_at
    }
    blog_categories {
        INTEGER id
        VARCHAR slug
        INTEGER parent_id
        INTEGER certification_id
        TEXT created_at
    }
    blog_post_tags {
        INTEGER post_id
        INTEGER tag_id
    }
    blog_post_versions {
        INTEGER id
        INTEGER post_id
        INTEGER editor_id
        TEXT created_at
    }
    blog_posts {
        INTEGER id
        VARCHAR slug
        INTEGER author_id
        INTEGER reviewer_id
        INTEGER editor_id
        INTEGER category_id
        INTEGER certification_id
    }
    blog_reviews {
        INTEGER id
        INTEGER post_id
        INTEGER reviewer_id
        TEXT created_at
    }
    blog_tags {
        INTEGER id
        VARCHAR slug
        TEXT created_at
    }
    cc_analytics_metrics {
        INTEGER id
        INTEGER source_id
    }
    cc_external_items {
        INTEGER id
        INTEGER source_id
        VARCHAR status
        INTEGER pci_post_id
        TEXT created_at
    }
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    pciworld_sources {
        INTEGER id
        TEXT created_at
    }
    public_documents {
        INTEGER id
        INTEGER certification_id
        VARCHAR status
        INTEGER supersedes_id
        TEXT created_at
    }
    reviews {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
    }
    template_download_daily {
        INTEGER template_id
        VARCHAR day
    }
    template_user_downloads {
        INTEGER user_id
        INTEGER template_id
    }
    templates {
        INTEGER id
        VARCHAR slug
        INTEGER certification_id
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    admin_users }o..o{ blog_authors : "admin_user_id"
    certifications }o..o{ blog_categories : "certification_id"
    blog_posts }o..o{ blog_post_tags : "post_id"
    blog_tags }o..o{ blog_post_tags : "tag_id"
    blog_posts }o..o{ blog_post_versions : "post_id"
    blog_authors }o..o{ blog_posts : "author_id"
    users }o..o{ blog_posts : "reviewer_id"
    certifications }o..o{ blog_posts : "certification_id"
    blog_posts }o..o{ blog_reviews : "post_id"
    users }o..o{ blog_reviews : "reviewer_id"
    pciworld_sources }o..o{ cc_analytics_metrics : "source_id"
    pciworld_sources }o..o{ cc_external_items : "source_id"
    certifications }o..o{ public_documents : "certification_id"
    users ||--o{ reviews : "user_id"
    templates }o..o{ template_download_daily : "template_id"
    users }o..o{ template_user_downloads : "user_id"
    templates }o..o{ template_user_downloads : "template_id"
    certifications }o..o{ templates : "certification_id"
```

**Drawn from other domains as anchors:** `admin_users` *(Students & identity)*, `certifications` *(Examinations & credentials)*, `pciworld_sources` *(PCI World — editorial & contributors)*, `users` *(Students & identity)*

**Tables in this domain:** `ai_content_generations`, `ai_content_providers`, `blog_authors`, `blog_categories`, `blog_post_tags`, `blog_post_versions`, `blog_posts`, `blog_reviews`, `blog_tags`, `cc_analytics_metrics`, `cc_analytics_sources`, `cc_backlinks`, `cc_content_links`, `cc_external_items`, `cc_external_sources`, `cc_link_prospects`, `cc_outreach`, `cc_syndicated_posts`, `cc_syndication_destinations`, `content_capabilities`, `content_i18n`, `content_jobs`, `faqs`, `media_assets`, `nav_items`, `news`, `newsletter_subscribers`, `page_blocks`, `pages`, `public_document_downloads`, `public_documents`, `resources`, `reviews`, `seo_redirects`, `seo_submissions`, `site_content`, `site_settings`, `template_download_daily`, `template_user_downloads`, `templates`

---

## Marketing, social & syndication

*34 tables in this domain*

```mermaid
erDiagram
    analytics_events {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    mkt_audiences {
        INTEGER id
        TEXT provider_audience_id
        VARCHAR status
        TEXT created_at
    }
    mkt_budget_approvals {
        INTEGER id
        INTEGER campaign_id
        VARCHAR status
        TEXT created_at
    }
    mkt_campaign_metrics {
        INTEGER id
        INTEGER platform_campaign_id
        TEXT created_at
    }
    mkt_campaigns {
        INTEGER id
        VARCHAR code
        INTEGER owner_admin_id
        INTEGER certification_id
        INTEGER landing_page_id
        INTEGER promotion_id
        VARCHAR status
    }
    mkt_capabilities {
        INTEGER id
        VARCHAR status
        INTEGER connection_id
    }
    mkt_connections {
        INTEGER id
        TEXT external_org_id
        TEXT external_ad_account_id
        TEXT external_page_id
        TEXT external_ig_id
        TEXT external_business_id
        VARCHAR status
    }
    mkt_conversation_ads {
        INTEGER id
        INTEGER campaign_id
        INTEGER connection_id
        INTEGER audience_id
        INTEGER landing_page_id
        INTEGER lead_form_id
        VARCHAR status
    }
    mkt_conversion_events {
        INTEGER id
        INTEGER conversion_id
        INTEGER campaign_id
        TEXT created_at
    }
    mkt_conversions {
        INTEGER id
        TEXT provider_conversion_id
        TEXT created_at
    }
    mkt_creatives {
        INTEGER id
        INTEGER certification_id
        INTEGER promotion_id
        TEXT created_at
    }
    mkt_gsc_properties {
        INTEGER id
        INTEGER connection_id
        TEXT created_at
    }
    mkt_keywords {
        INTEGER id
        INTEGER platform_campaign_id
        VARCHAR status
        TEXT created_at
    }
    mkt_landing_pages {
        INTEGER id
        INTEGER certification_id
        INTEGER promotion_id
        INTEGER conversion_id
        VARCHAR status
        TEXT created_at
    }
    mkt_lead_forms {
        INTEGER id
        INTEGER connection_id
        TEXT provider_form_id
        INTEGER certification_id
        VARCHAR status
        TEXT created_at
    }
    mkt_leads {
        INTEGER id
        TEXT email
        INTEGER campaign_id
        INTEGER platform_campaign_id
        INTEGER form_id
        INTEGER owner_admin_id
        VARCHAR status
    }
    mkt_linkedin_outreach {
        INTEGER id
        INTEGER lead_id
        INTEGER owner_admin_id
        TEXT created_at
    }
    mkt_linkedin_posts {
        INTEGER id
        INTEGER campaign_id
        INTEGER connection_id
        INTEGER certification_id
        VARCHAR status
        TEXT linkedin_post_id
        TEXT created_at
    }
    mkt_platform_campaigns {
        INTEGER id
        INTEGER campaign_id
        INTEGER connection_id
        TEXT provider_campaign_id
        INTEGER landing_page_id
        INTEGER lead_form_id
        INTEGER conversion_id
    }
    mkt_promotions {
        INTEGER id
        VARCHAR code
        INTEGER certification_id
        INTEGER landing_page_id
        VARCHAR status
        TEXT created_at
    }
    social_accounts {
        INTEGER id
        TEXT created_at
    }
    social_audit {
        INTEGER id
        INTEGER account_id
        INTEGER actor_id
        TEXT created_at
    }
    social_drafts {
        INTEGER id
        INTEGER post_id
        INTEGER account_id
        VARCHAR status
        INTEGER job_id
        TEXT created_at
    }
    social_link_checks {
        INTEGER id
        INTEGER account_id
        VARCHAR status
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    users }o..o{ analytics_events : "user_id"
    mkt_campaigns }o..o{ mkt_budget_approvals : "campaign_id"
    mkt_platform_campaigns }o..o{ mkt_campaign_metrics : "platform_campaign_id"
    certifications }o..o{ mkt_campaigns : "certification_id"
    mkt_landing_pages }o..o{ mkt_campaigns : "landing_page_id"
    mkt_promotions }o..o{ mkt_campaigns : "promotion_id"
    mkt_connections }o..o{ mkt_capabilities : "connection_id"
    mkt_campaigns }o..o{ mkt_conversation_ads : "campaign_id"
    mkt_connections }o..o{ mkt_conversation_ads : "connection_id"
    mkt_audiences }o..o{ mkt_conversation_ads : "audience_id"
    mkt_landing_pages }o..o{ mkt_conversation_ads : "landing_page_id"
    mkt_lead_forms }o..o{ mkt_conversation_ads : "lead_form_id"
    mkt_conversions }o..o{ mkt_conversion_events : "conversion_id"
    mkt_campaigns }o..o{ mkt_conversion_events : "campaign_id"
    certifications }o..o{ mkt_creatives : "certification_id"
    mkt_promotions }o..o{ mkt_creatives : "promotion_id"
    mkt_connections }o..o{ mkt_gsc_properties : "connection_id"
    mkt_platform_campaigns }o..o{ mkt_keywords : "platform_campaign_id"
    certifications }o..o{ mkt_landing_pages : "certification_id"
    mkt_promotions }o..o{ mkt_landing_pages : "promotion_id"
    mkt_conversions }o..o{ mkt_landing_pages : "conversion_id"
    mkt_connections }o..o{ mkt_lead_forms : "connection_id"
    certifications }o..o{ mkt_lead_forms : "certification_id"
    mkt_campaigns }o..o{ mkt_leads : "campaign_id"
    mkt_platform_campaigns }o..o{ mkt_leads : "platform_campaign_id"
    users }o..o{ mkt_leads : "user_id"
    mkt_leads }o..o{ mkt_linkedin_outreach : "lead_id"
    mkt_campaigns }o..o{ mkt_linkedin_posts : "campaign_id"
    mkt_connections }o..o{ mkt_linkedin_posts : "connection_id"
    certifications }o..o{ mkt_linkedin_posts : "certification_id"
    mkt_campaigns }o..o{ mkt_platform_campaigns : "campaign_id"
    mkt_connections }o..o{ mkt_platform_campaigns : "connection_id"
    mkt_landing_pages }o..o{ mkt_platform_campaigns : "landing_page_id"
    mkt_lead_forms }o..o{ mkt_platform_campaigns : "lead_form_id"
    mkt_conversions }o..o{ mkt_platform_campaigns : "conversion_id"
    certifications }o..o{ mkt_promotions : "certification_id"
    mkt_landing_pages }o..o{ mkt_promotions : "landing_page_id"
    social_accounts }o..o{ social_audit : "account_id"
    users }o..o{ social_audit : "actor_id"
    social_accounts }o..o{ social_drafts : "account_id"
    social_accounts }o..o{ social_link_checks : "account_id"
```

**Drawn from other domains as anchors:** `certifications` *(Examinations & credentials)*, `users` *(Students & identity)*

**Tables in this domain:** `analytics_events`, `campaign_recipients`, `mkt_alerts`, `mkt_approvals`, `mkt_audiences`, `mkt_budget_approvals`, `mkt_campaign_metrics`, `mkt_campaigns`, `mkt_capabilities`, `mkt_connections`, `mkt_conversation_ads`, `mkt_conversion_events`, `mkt_conversions`, `mkt_creatives`, `mkt_gsc_inspections`, `mkt_gsc_properties`, `mkt_gsc_query_data`, `mkt_gsc_sitemaps`, `mkt_jobs`, `mkt_keywords`, `mkt_landing_pages`, `mkt_lead_forms`, `mkt_leads`, `mkt_linkedin_outreach`, `mkt_linkedin_posts`, `mkt_platform_campaigns`, `mkt_platforms`, `mkt_promotions`, `social_accounts`, `social_audit`, `social_drafts`, `social_link_checks`, `social_pub_accounts`, `social_share_settings`

---

## Communications & notifications

*23 tables in this domain*

```mermaid
erDiagram
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    chat_messages {
        INTEGER id
        INTEGER session_id
        TEXT created_at
    }
    chat_sessions {
        INTEGER id
        VARCHAR status
        TEXT created_at
        INTEGER linked_user_id
    }
    comm_campaigns {
        INTEGER id
        INTEGER certification_id
        VARCHAR status
        TEXT created_at
    }
    comm_conversations {
        INTEGER id
        INTEGER user_id
        INTEGER certification_id
        INTEGER application_id
        INTEGER payment_id
        INTEGER assigned_admin_id
        VARCHAR status
    }
    comm_delivery_attempts {
        INTEGER id
        INTEGER outbox_id
        VARCHAR status
        TEXT created_at
    }
    comm_inbound_messages {
        INTEGER id
        INTEGER conversation_id
        TEXT provider_message_id
        INTEGER author_admin_id
        TEXT created_at
    }
    comm_outbox {
        INTEGER id
        INTEGER user_id
        INTEGER conversation_id
        INTEGER campaign_id
        INTEGER certification_id
        VARCHAR status
        TEXT provider_message_id
    }
    comm_preferences {
        INTEGER id
        INTEGER user_id
    }
    comm_templates {
        INTEGER id
        INTEGER certification_id
        VARCHAR status
        TEXT created_at
    }
    email_logs {
        INTEGER id
        INTEGER user_id
        TEXT email
        TEXT status
    }
    notifications {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    payments {
        INTEGER id
        INTEGER user_id
        INTEGER enrollment_session_id
        TEXT provider_payment_id
        TEXT created_at
        INTEGER discount_code_id
        INTEGER partner_id
    }
    pciworld_applications {
        INTEGER id
        INTEGER posting_id
        INTEGER applicant_user_id
        VARCHAR state
        VARCHAR created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    chat_sessions }o..o{ chat_messages : "session_id"
    certifications }o..o{ comm_campaigns : "certification_id"
    users }o..o{ comm_conversations : "user_id"
    certifications }o..o{ comm_conversations : "certification_id"
    pciworld_applications }o..o{ comm_conversations : "application_id"
    payments }o..o{ comm_conversations : "payment_id"
    comm_outbox }o..o{ comm_delivery_attempts : "outbox_id"
    comm_conversations }o..o{ comm_inbound_messages : "conversation_id"
    users }o..o{ comm_outbox : "user_id"
    comm_conversations }o..o{ comm_outbox : "conversation_id"
    comm_campaigns }o..o{ comm_outbox : "campaign_id"
    certifications }o..o{ comm_outbox : "certification_id"
    users }o..o{ comm_preferences : "user_id"
    certifications }o..o{ comm_templates : "certification_id"
    users }o..o{ email_logs : "user_id"
    users }o..o{ notifications : "user_id"
```

**Drawn from other domains as anchors:** `certifications` *(Examinations & credentials)*, `payments` *(Payments, finance & partners)*, `pciworld_applications` *(PCI World — careers)*, `users` *(Students & identity)*

**Tables in this domain:** `chat_kb`, `chat_messages`, `chat_sessions`, `comm_campaigns`, `comm_conversations`, `comm_delivery_attempts`, `comm_inbound_messages`, `comm_outbox`, `comm_preferences`, `comm_routing_rules`, `comm_sender_profiles`, `comm_suppression`, `comm_template_versions`, `comm_templates`, `comm_triggers`, `comm_whatsapp_accounts`, `email_campaigns`, `email_logs`, `email_suppression`, `form_submissions`, `inquiries`, `notification_history`, `notifications`

---

## Support, casework & documents

*16 tables in this domain*

```mermaid
erDiagram
    accommodation_requests {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
    }
    appeals {
        INTEGER id
        INTEGER user_id
        INTEGER attempt_id
        TEXT credential_id
        TEXT status
    }
    cpd_declarations {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    cpd_entries {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
        INTEGER source_event_id
    }
    document_acknowledgements {
        INTEGER id
        INTEGER document_id
        INTEGER user_id
    }
    document_assignments {
        INTEGER id
        INTEGER document_id
        INTEGER user_id
        VARCHAR status
    }
    document_downloads {
        INTEGER id
        INTEGER document_id
        INTEGER user_id
        TEXT created_at
    }
    documents {
        INTEGER id
        VARCHAR status
        INTEGER root_id
        INTEGER supersedes_id
        TEXT created_at
        INTEGER restored_from_id
    }
    erasure_requests {
        INTEGER id
        INTEGER user_id
        TEXT email
        VARCHAR status
    }
    error_reports {
        INTEGER id
        INTEGER user_id
        INTEGER related_id
        VARCHAR status
        TEXT created_at
    }
    pciworld_attempts {
        INTEGER id
        INTEGER session_id
        INTEGER challenge_id
        VARCHAR status
        INTEGER invite_id
        INTEGER parent_attempt_id
        INTEGER user_id
    }
    support_attachments {
        INTEGER id
        INTEGER ticket_id
        INTEGER user_id
        TEXT created_at
    }
    ticket_messages {
        INTEGER id
        INTEGER ticket_id
        TEXT created_at
    }
    ticket_notes {
        INTEGER id
        INTEGER ticket_id
        INTEGER admin_id
        TEXT created_at
    }
    tickets {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    users }o..o{ accommodation_requests : "user_id"
    users }o..o{ appeals : "user_id"
    pciworld_attempts }o..o{ appeals : "attempt_id"
    users }o..o{ cpd_declarations : "user_id"
    users }o..o{ cpd_entries : "user_id"
    documents }o..o{ document_acknowledgements : "document_id"
    users }o..o{ document_acknowledgements : "user_id"
    documents }o..o{ document_assignments : "document_id"
    users }o..o{ document_assignments : "user_id"
    documents }o..o{ document_downloads : "document_id"
    users }o..o{ document_downloads : "user_id"
    users }o..o{ erasure_requests : "user_id"
    users }o..o{ error_reports : "user_id"
    tickets ||--o{ support_attachments : "ticket_id"
    users }o..o{ support_attachments : "user_id"
    tickets ||--o{ ticket_messages : "ticket_id"
    tickets }o..o{ ticket_notes : "ticket_id"
    users }o..o{ tickets : "user_id"
```

**Drawn from other domains as anchors:** `pciworld_attempts` *(PCI World — challenges, rotation & intelligence)*, `users` *(Students & identity)*

**Tables in this domain:** `accommodation_requests`, `appeals`, `cpd_declarations`, `cpd_entries`, `document_acknowledgements`, `document_assignments`, `document_categories`, `document_downloads`, `documents`, `erasure_requests`, `error_reports`, `support_attachments`, `support_templates`, `ticket_messages`, `ticket_notes`, `tickets`

---

## Events

*2 tables in this domain*

```mermaid
erDiagram
    cpd_entries {
        INTEGER id
        INTEGER user_id
        TEXT status
        TEXT created_at
        INTEGER source_event_id
    }
    event_registrations {
        INTEGER id
        INTEGER event_id
        INTEGER user_id
        TEXT status
        INTEGER cpd_entry_id
    }
    events {
        INTEGER id
        VARCHAR status
        TEXT created_at
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    events }o..o{ event_registrations : "event_id"
    users }o..o{ event_registrations : "user_id"
    cpd_entries }o..o{ event_registrations : "cpd_entry_id"
```

**Drawn from other domains as anchors:** `cpd_entries` *(Support, casework & documents)*, `users` *(Students & identity)*

**Tables in this domain:** `event_registrations`, `events`

---

## Integrations & operations

*17 tables in this domain*

```mermaid
erDiagram
    audit_logs {
        INTEGER id
        INTEGER user_id
        TEXT created_at
    }
    certifications {
        INTEGER id
        TEXT code
        TEXT created_at
        TEXT status
        TEXT slug
        VARCHAR credly_template_id
    }
    certuvo_accounts {
        INTEGER id
        INTEGER user_id
        TEXT external_id
        TEXT status
        TEXT created_at
    }
    events {
        INTEGER id
        VARCHAR status
        TEXT created_at
    }
    founding_applications {
        INTEGER id
        INTEGER user_id
        INTEGER code_id
        TEXT status
        TEXT created_at
    }
    honorary_application_documents {
        INTEGER id
        INTEGER application_id
        TEXT created_at
    }
    honorary_applications {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
        INTEGER certification_id
    }
    honorary_awards {
        INTEGER id
        INTEGER user_id
        TEXT status
    }
    honorary_idv_documents {
        INTEGER id
        INTEGER application_id
        TEXT created_at
    }
    integration_deliveries {
        INTEGER id
        INTEGER event_id
        INTEGER integration_id
        TEXT status
        TEXT created_at
    }
    integrations {
        INTEGER id
        TEXT status
        TEXT created_at
    }
    job_app_events {
        INTEGER id
        INTEGER application_id
        INTEGER actor_id
        TEXT created_at
    }
    job_applications {
        INTEGER id
        INTEGER job_id
        VARCHAR email
        TEXT status
        TEXT created_at
        INTEGER user_id
    }
    users {
        INTEGER id
        TEXT email
        TEXT status
        TEXT created_at
    }
    users }o..o{ audit_logs : "user_id"
    users }o..o{ certuvo_accounts : "user_id"
    users }o..o{ founding_applications : "user_id"
    honorary_applications }o..o{ honorary_application_documents : "application_id"
    certifications }o..o{ honorary_applications : "certification_id"
    users }o..o{ honorary_awards : "user_id"
    honorary_applications }o..o{ honorary_idv_documents : "application_id"
    events }o..o{ integration_deliveries : "event_id"
    integrations }o..o{ integration_deliveries : "integration_id"
    job_applications }o..o{ job_app_events : "application_id"
    users }o..o{ job_app_events : "actor_id"
    users }o..o{ job_applications : "user_id"
```

**Drawn from other domains as anchors:** `certifications` *(Examinations & credentials)*, `events` *(Events)*, `users` *(Students & identity)*

**Tables in this domain:** `audit_logs`, `career_email_templates`, `career_taxonomy`, `certuvo_accounts`, `founding_applications`, `honorary_application_documents`, `honorary_applications`, `honorary_awards`, `honorary_idv_documents`, `integration_deliveries`, `integration_events`, `integrations`, `job_app_events`, `job_applications`, `job_postings`, `job_questions`, `schema_migrations`

---
