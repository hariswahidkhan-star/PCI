namespace PCI.Backend.Data;

public static class Migrate
{
    public static void Run(Db db, string schemaPath)
    {
        db.Exec(File.ReadAllText(schemaPath));
        // ensure production lifecycle tables exist on pre-existing databases
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_score_snapshots(id INTEGER PRIMARY KEY AUTOINCREMENT,attempt_id INTEGER NOT NULL,user_id INTEGER NOT NULL,score INTEGER,max_score INTEGER,percent REAL,result TEXT,domain_breakdown TEXT,unanswered INTEGER,flagged_events INTEGER,duration_seconds INTEGER,submitted_at TEXT,answer_key_version TEXT,bank_version TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_score_snapshot_attempt ON exam_score_snapshots(attempt_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_entitlements(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,payment_id INTEGER,product_type TEXT,status TEXT DEFAULT 'available',valid_until TEXT,booking_id INTEGER,attempt_id INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_entitlement_payment ON exam_entitlements(payment_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS candidate_consents(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,consent_type TEXT NOT NULL,policy_version TEXT NOT NULL,accepted_at TEXT DEFAULT (datetime('now')),ip_address TEXT,user_agent TEXT,metadata_json TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_consents_user ON candidate_consents(user_id, consent_type)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS webhook_events(id INTEGER PRIMARY KEY AUTOINCREMENT,provider TEXT DEFAULT 'stripe',event_id TEXT,processed_at TEXT DEFAULT (datetime('now')),status TEXT,error TEXT)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_webhook_event ON webhook_events(event_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS security_events(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER,kind TEXT,detail TEXT,ip TEXT,user_agent TEXT,created_at TEXT DEFAULT (datetime('now')))");


        // idempotent column upgrades (parity with db.js)
        void EnsureReviews() => db.Exec(@"CREATE TABLE IF NOT EXISTS reviews(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER,name TEXT NOT NULL,designation TEXT,company TEXT,country TEXT,relationship TEXT,rating INTEGER,title TEXT,body TEXT NOT NULL,status TEXT DEFAULT 'pending',featured INTEGER DEFAULT 0,sort_order INTEGER DEFAULT 0,created_at TEXT DEFAULT (datetime('now')),published_at TEXT,reviewed_by INTEGER)");
        EnsureReviews();
        void AddCol(string table, string col, string ddl)
        {
            try
            {
                var have = db.Columns(table);
                if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}");
            }
            catch
            {
                // Table may not exist yet on this provider (e.g. mkt_jobs is created later by MarketingSchema).
            }
        }
        AddCol("users", "is_test", "is_test INTEGER DEFAULT 0");   // admin-created test accounts (excluded from real reporting)
        // Honorary application: explicit eligibility self-confirmation + terms & conditions acceptance,
        // recorded per applicant for audit (in addition to the existing truthfulness declaration).
        AddCol("honorary_applications", "eligibility_confirmed", "eligibility_confirmed INTEGER DEFAULT 0");
        AddCol("honorary_applications", "terms_accepted", "terms_accepted INTEGER DEFAULT 0");
        AddCol("honorary_applications", "terms_accepted_at", "terms_accepted_at TEXT");
        // Structured applicant history (repeatable rows captured on the public form, shown to the board):
        // qualifications [{qualification,institution,year}], certifications [{name,issuer,year}],
        // experience [{role,employer,from_year,to_year}] — sanitised JSON; the legacy flat text columns
        // are still composed server-side so older admin views and exports keep working.
        AddCol("honorary_applications", "qualifications_json", "qualifications_json TEXT");
        AddCol("honorary_applications", "certifications_json", "certifications_json TEXT");
        AddCol("honorary_applications", "experience_json", "experience_json TEXT");
        // Shortlist-gated identity verification (IDV): only shortlisted candidates are invited, via a
        // one-time time-limited token, to submit a passport-style photo + one government ID and a
        // background/truthfulness declaration. Data minimisation: no ID numbers are stored — only the
        // document image, held in a protected category with its own retention/deletion, plus attestation
        // booleans. idv_status: none | invited | submitted | verified | rejected | deleted.
        AddCol("honorary_applications", "shortlisted", "shortlisted INTEGER DEFAULT 0");
        AddCol("honorary_applications", "shortlisted_at", "shortlisted_at TEXT");
        AddCol("honorary_applications", "idv_token", "idv_token VARCHAR(64)");
        AddCol("honorary_applications", "idv_token_expires", "idv_token_expires TEXT");
        AddCol("honorary_applications", "idv_status", "idv_status VARCHAR(16) DEFAULT 'none'");
        AddCol("honorary_applications", "idv_submitted_at", "idv_submitted_at TEXT");
        AddCol("honorary_applications", "background_declaration", "background_declaration INTEGER DEFAULT 0");
        AddCol("honorary_applications", "background_declared_at", "background_declared_at TEXT");
        AddCol("honorary_applications", "idv_deleted_at", "idv_deleted_at TEXT");
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_idv_documents (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_id INTEGER NOT NULL,
            doc_kind VARCHAR(20),                      -- photo | government_id
            filename TEXT, mime VARCHAR(80), size_bytes INTEGER, storage_ref TEXT, sha256 VARCHAR(64),
            created_at TEXT DEFAULT (datetime('now')));");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honidv_app ON honorary_idv_documents(application_id)");
        AddCol("sample_questions", "is_practice", "is_practice INTEGER DEFAULT 0");
        // Certuvo (Phase 8): practice questions carry a teaching explanation + a difficulty band.
        AddCol("sample_questions", "explanation", "explanation TEXT");
        AddCol("sample_questions", "difficulty", "difficulty TEXT");
        AddCol("exam_attempts", "result_status", "result_status TEXT DEFAULT 'not_started'");
        AddCol("exam_attempts", "hold_reason", "hold_reason TEXT");
        AddCol("exam_attempts", "released_at", "released_at TEXT");
        AddCol("exam_attempts", "answer_key_version", "answer_key_version TEXT");
        AddCol("exam_attempts", "bank_version", "bank_version TEXT");
        AddCol("issued_credentials", "attempt_id", "attempt_id INTEGER");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_credential_attempt ON issued_credentials(attempt_id) WHERE attempt_id IS NOT NULL");
        // Verifiable PDF certificate: private storage reference, a SHA-256 tamper hash the public verifier can
        // recompute, a non-guessable verification token, and the render timestamp. The PDF is a rendering of
        // the authoritative record — it can always be regenerated, so these are additive and never a blocker.
        AddCol("issued_credentials", "pdf_ref", "pdf_ref TEXT");
        AddCol("issued_credentials", "pdf_sha256", "pdf_sha256 TEXT");
        AddCol("issued_credentials", "verify_token", "verify_token TEXT");
        AddCol("issued_credentials", "pdf_generated_at", "pdf_generated_at TEXT");
        // Phase 2: casework tables + CPD evidence columns for pre-existing databases
        db.Exec(@"CREATE TABLE IF NOT EXISTS appeals(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,attempt_id INTEGER,credential_id TEXT,type TEXT NOT NULL,reason TEXT NOT NULL,evidence_name TEXT,evidence_data TEXT,status TEXT DEFAULT 'submitted',submitted_at TEXT DEFAULT (datetime('now')),decision TEXT,decided_by INTEGER,decided_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_appeals_user ON appeals(user_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS accommodation_requests(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,request_type TEXT NOT NULL,description TEXT NOT NULL,evidence_name TEXT,evidence_data TEXT,status TEXT DEFAULT 'submitted',approved_extra_minutes INTEGER DEFAULT 0,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),decided_at TEXT,decided_by INTEGER)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_accom_user ON accommodation_requests(user_id)");
        // GDPR right-to-erasure requests: a request creates a tracked ticket with a 30-day due date; an admin
        // acknowledges, then completes (anonymisation) or rejects with a reason. status: pending | acknowledged | completed | rejected.
        db.Exec(@"CREATE TABLE IF NOT EXISTS erasure_requests(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,email TEXT,reason TEXT,
            status VARCHAR(24) DEFAULT 'pending',requested_at TEXT DEFAULT (datetime('now')),due_at TEXT,acknowledged_at TEXT,
            reviewed_by INTEGER,reviewed_at TEXT,admin_note TEXT,completed_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_erasure_user ON erasure_requests(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_erasure_status ON erasure_requests(status)");
        // Annual CPD declaration ("declared, not discovered"): a member attests their CPD position each cycle
        // year. position: compliant | career_break | not_met. hours_snapshot records approved hours at the time.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cpd_declarations(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,
            cycle_year INTEGER NOT NULL,position TEXT NOT NULL,statement TEXT,hours_snapshot REAL DEFAULT 0,ai_hours_snapshot REAL DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cpd_decl_user ON cpd_declarations(user_id)");
        // Membership grade (professional standing): student | associate | professional | fellow. Distinct from
        // membership_type (the product) — the grade carries the post-nominal (APCI/MPCI/FPCI) and progression.
        AddCol("memberships", "grade", "grade TEXT DEFAULT 'associate'");
        // Recurring annual-dues subscription state (Stripe Billing). Populated by the subscription webhooks;
        // empty for members who paid a one-off term. subscription_status mirrors Stripe (active|past_due|canceled…).
        AddCol("memberships", "stripe_customer_id", "stripe_customer_id VARCHAR(64)");
        AddCol("memberships", "stripe_subscription_id", "stripe_subscription_id VARCHAR(64)");
        AddCol("memberships", "subscription_status", "subscription_status VARCHAR(24)");
        AddCol("memberships", "cancel_at_period_end", "cancel_at_period_end INTEGER DEFAULT 0");
        // Grade upgrade / Fellowship-nomination applications reviewed by admins.
        db.Exec(@"CREATE TABLE IF NOT EXISTS membership_upgrades(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,
            from_grade TEXT,to_grade TEXT NOT NULL,statement TEXT,status VARCHAR(24) DEFAULT 'pending',
            reviewed_by INTEGER,reviewed_at TEXT,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),decided_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mupgrade_user ON membership_upgrades(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_mupgrade_status ON membership_upgrades(status)");
        // Member events & webinars (CPD-earning). event_type: webinar | workshop | conference | course.
        // cpd_hours/cpd_category are credited (as an approved CPD entry) when a registrant is marked attended.
        db.Exec(@"CREATE TABLE IF NOT EXISTS events(id INTEGER PRIMARY KEY AUTOINCREMENT,title TEXT NOT NULL,summary TEXT,description TEXT,
            event_type TEXT DEFAULT 'webinar',starts_at TEXT,ends_at TEXT,timezone TEXT,location TEXT,join_url TEXT,capacity INTEGER DEFAULT 0,
            cpd_hours REAL DEFAULT 0,cpd_category TEXT DEFAULT 'Events & webinars',status VARCHAR(24) DEFAULT 'draft',
            created_by INTEGER,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_events_status ON events(status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS event_registrations(id INTEGER PRIMARY KEY AUTOINCREMENT,event_id INTEGER NOT NULL,user_id INTEGER NOT NULL,
            status TEXT DEFAULT 'registered',registered_at TEXT DEFAULT (datetime('now')),attended_at TEXT,cpd_entry_id INTEGER)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_event_reg ON event_registrations(event_id,user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_event_reg_user ON event_registrations(user_id)");
        // Careers / job board. employment_type: full_time|part_time|contract|internship|temporary.
        // remote_type: onsite|remote|hybrid. apply_method: inplatform|url|email. status: draft|published|closed.
        db.Exec(@"CREATE TABLE IF NOT EXISTS job_postings(id INTEGER PRIMARY KEY AUTOINCREMENT,title TEXT NOT NULL,organisation TEXT,location TEXT,
            employment_type TEXT DEFAULT 'full_time',remote_type TEXT DEFAULT 'onsite',sector TEXT,description TEXT,requirements TEXT,responsibilities TEXT,
            salary_min REAL,salary_max REAL,salary_currency TEXT DEFAULT 'USD',salary_period TEXT DEFAULT 'year',
            apply_method TEXT DEFAULT 'inplatform',apply_url TEXT,apply_email TEXT,featured INTEGER DEFAULT 0,status VARCHAR(24) DEFAULT 'draft',
            posted_at TEXT,closes_at TEXT,created_by INTEGER,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_jobs_status ON job_postings(status)");
        // Job reference code (auto-generated, unique) + country (for country-based posting & filtering).
        AddCol("job_postings", "job_code", "job_code VARCHAR(32)");
        AddCol("job_postings", "country", "country VARCHAR(64)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_jobs_code ON job_postings(job_code)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS job_applications(id INTEGER PRIMARY KEY AUTOINCREMENT,job_id INTEGER NOT NULL,name TEXT,email VARCHAR(190),phone TEXT,
            cover_message TEXT,cv_ref TEXT,cv_name TEXT,status TEXT DEFAULT 'new',admin_note TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_jobapp_job ON job_applications(job_id)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_jobapp_email ON job_applications(job_id,email)");
        // fallback shape kept aligned with schema.sql (storage_ref required, data_uri nullable legacy)
        db.Exec(@"CREATE TABLE IF NOT EXISTS support_attachments(id INTEGER PRIMARY KEY AUTOINCREMENT,ticket_id INTEGER NOT NULL,user_id INTEGER,filename TEXT NOT NULL,mime TEXT,size_bytes INTEGER,sha256 TEXT,data_uri TEXT,storage_ref TEXT NOT NULL,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_attach_ticket ON support_attachments(ticket_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_readiness_checks(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,booking_id INTEGER,camera INTEGER DEFAULT 0,microphone INTEGER DEFAULT 0,network INTEGER DEFAULT 0,fullscreen INTEGER DEFAULT 0,environment INTEGER DEFAULT 0,browser TEXT,screen TEXT,passed INTEGER DEFAULT 0,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_readiness_user ON exam_readiness_checks(user_id)");
        // Stage 2: per-page editable content regions (fully dynamic content)
        db.Exec(@"CREATE TABLE IF NOT EXISTS page_blocks(id INTEGER PRIMARY KEY AUTOINCREMENT,slug TEXT NOT NULL,block_key TEXT NOT NULL,label TEXT,ctype TEXT DEFAULT 'text',cvalue TEXT,sort_order INTEGER DEFAULT 0,updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_page_block ON page_blocks(slug, block_key)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_page_block_slug ON page_blocks(slug)");
        // Phase 2: storage-abstraction metadata columns (bytes live in blob storage; DB holds ref+metadata)
        AddCol("exam_evidence", "storage_ref", "storage_ref TEXT");
        AddCol("exam_evidence", "size_bytes", "size_bytes INTEGER");
        AddCol("exam_evidence", "sha256", "sha256 TEXT");
        AddCol("support_attachments", "storage_ref", "storage_ref TEXT");
        AddCol("support_attachments", "sha256", "sha256 TEXT");
        AddCol("cpd_entries", "evidence_name", "evidence_name TEXT");
        AddCol("cpd_entries", "evidence_data", "evidence_data TEXT");
        AddCol("cpd_entries", "admin_note", "admin_note TEXT");
        AddCol("cpd_entries", "reviewed_by", "reviewed_by INTEGER");
        AddCol("cpd_entries", "reviewed_at", "reviewed_at TEXT");
        AddCol("sample_questions", "option_a", "option_a TEXT");
        AddCol("sample_questions", "option_b", "option_b TEXT");
        AddCol("sample_questions", "option_c", "option_c TEXT");
        AddCol("sample_questions", "option_d", "option_d TEXT");
        AddCol("users", "registration_no", "registration_no TEXT");
        AddCol("exam_launch_codes", "code_hash", "code_hash TEXT");
        AddCol("discount_codes", "code_type", "code_type TEXT DEFAULT 'general'");
        AddCol("discount_codes", "org_name", "org_name TEXT");
        AddCol("discount_codes", "owner_user_id", "owner_user_id INTEGER");
        AddCol("discount_codes", "batch_id", "batch_id TEXT");
        AddCol("discount_codes", "per_user_limit", "per_user_limit INTEGER");
        AddCol("discount_codes", "notes", "notes TEXT");
        // Founding-stage access: fee-waiver codes layered on the paid flow (see Endpoints/Founding.cs)
        AddCol("discount_codes", "founding_route", "founding_route TEXT");
        AddCol("discount_codes", "grants_membership", "grants_membership INTEGER DEFAULT 1");
        AddCol("discount_codes", "grants_exam", "grants_exam INTEGER DEFAULT 1");
        AddCol("discount_codes", "grants_study_access", "grants_study_access INTEGER DEFAULT 1");
        AddCol("discount_codes", "requires_application", "requires_application INTEGER DEFAULT 0");
        AddCol("discount_codes", "auto_approve", "auto_approve INTEGER DEFAULT 1");
        AddCol("discount_codes", "membership_months", "membership_months INTEGER DEFAULT 12");
        AddCol("discount_codes", "criteria_json", "criteria_json TEXT");
        db.Exec(@"CREATE TABLE IF NOT EXISTS founding_applications(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,code_id INTEGER NOT NULL,route TEXT,declared_experience_years INTEGER,declared_role TEXT,declared_qualification TEXT,evidence_ref TEXT,evidence_name TEXT,evidence_mime TEXT,evidence_size INTEGER,status TEXT NOT NULL DEFAULT 'pending_review',decided_by INTEGER,decided_at TEXT,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_founding_app_user ON founding_applications(user_id)");
        // Three-route consolidation: the earlier two founding tiers (founding_member = membership only,
        // founding_candidate = membership + exam) are unified into ONE founding route that grants
        // membership + study + exam together. Historical rows are mapped forward, never deleted;
        // application/auto-approve settings on each code are preserved as the board set them.
        db.Exec("UPDATE discount_codes SET grants_membership=1, grants_exam=1, grants_study_access=1, founding_route='founding' WHERE founding_route IN ('founding_member','founding_candidate')");
        db.Exec("UPDATE founding_applications SET route='founding' WHERE route IN ('founding_member','founding_candidate')");
        // Honorary awards: board-conferred recognition, deliberately separate from issued_credentials.
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_awards(id INTEGER PRIMARY KEY AUTOINCREMENT,award_no TEXT UNIQUE NOT NULL,recipient_name TEXT NOT NULL,user_id INTEGER,citation TEXT,designation TEXT DEFAULT 'Honorary Fellow (PCI)',status TEXT DEFAULT 'active',conferred_by INTEGER NOT NULL,conferred_at TEXT DEFAULT (datetime('now')),revoked_by INTEGER,revoked_at TEXT,revoke_reason TEXT)");
        // Honorary certificate PDF (clearly an HONORARY certificate — never an examined credential).
        AddCol("honorary_awards", "pdf_ref", "pdf_ref TEXT");
        AddCol("honorary_awards", "pdf_sha256", "pdf_sha256 TEXT");
        AddCol("honorary_awards", "pdf_generated_at", "pdf_generated_at TEXT");
        // Immutable download audit for every certificate PDF fetch (student or admin).
        db.Exec(@"CREATE TABLE IF NOT EXISTS certificate_downloads(id INTEGER PRIMARY KEY AUTOINCREMENT,credential_id VARCHAR(64),user_id INTEGER,actor TEXT,role TEXT,ip TEXT,kind TEXT,result TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honorary_user ON honorary_awards(user_id)");
        // Honorary-route public applications (apply → board review → conferral) + reusable notification ledger.
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_applications(id INTEGER PRIMARY KEY AUTOINCREMENT,reference TEXT UNIQUE NOT NULL,first_name TEXT,last_name TEXT,email TEXT,mobile TEXT,country TEXT,city TEXT,nationality TEXT,job_title TEXT,employer TEXT,years_experience INTEGER,industry TEXT,highest_qualification TEXT,professional_certifications TEXT,relevant_experience TEXT,professional_summary TEXT,declaration INTEGER DEFAULT 0,status TEXT NOT NULL DEFAULT 'pending_review',award_no TEXT,decided_by INTEGER,decided_at TEXT,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honapp_status ON honorary_applications(status)");
        // Certification/discipline the applicant is aligned to (optional — honorary recognition is not tied
        // to an examination, but the board records which of the PCI credentials the contribution relates to).
        AddCol("honorary_applications", "certification_id", "certification_id INTEGER");
        // Optional applicant explanation for the limited suitability declaration. Sensitive — surfaced only
        // in the owner-only application detail, never in lists, and covered by limited retention.
        AddCol("honorary_applications", "suitability_note", "suitability_note TEXT");
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_application_documents(id INTEGER PRIMARY KEY AUTOINCREMENT,application_id INTEGER NOT NULL,doc_kind TEXT DEFAULT 'supporting',filename TEXT,mime TEXT,size_bytes INTEGER,storage_ref TEXT,sha256 TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honappdoc_app ON honorary_application_documents(application_id)");
        // Training Partner framework (Phase 7): provider directory + public application + review workflow.
        db.Exec(@"CREATE TABLE IF NOT EXISTS training_partners(id INTEGER PRIMARY KEY AUTOINCREMENT,name TEXT NOT NULL,slug TEXT UNIQUE,tier TEXT NOT NULL DEFAULT 'registered',country TEXT,region TEXT,city TEXT,website TEXT,logo_url TEXT,summary TEXT,description TEXT,specialties TEXT,contact_email TEXT,listed INTEGER DEFAULT 0,sort_order INTEGER DEFAULT 0,source_application_id INTEGER,approved_at TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_partners_listed ON training_partners(listed)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_partners_tier ON training_partners(tier)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS training_partner_applications(id INTEGER PRIMARY KEY AUTOINCREMENT,reference TEXT UNIQUE NOT NULL,org_name TEXT,website TEXT,contact_name TEXT,contact_email TEXT,contact_phone TEXT,country TEXT,city TEXT,region TEXT,delivery_modes TEXT,specialties TEXT,learners_per_year INTEGER,description TEXT,declaration INTEGER DEFAULT 0,status TEXT NOT NULL DEFAULT 'pending_review',proposed_tier TEXT,partner_id INTEGER,decided_by INTEGER,decided_at TEXT,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_tpapp_status ON training_partner_applications(status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS training_partner_application_documents(id INTEGER PRIMARY KEY AUTOINCREMENT,application_id INTEGER NOT NULL,doc_kind TEXT DEFAULT 'supporting',filename TEXT,mime TEXT,size_bytes INTEGER,storage_ref TEXT,sha256 TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_tpappdoc_app ON training_partner_application_documents(application_id)");
        // Partner dashboards: commission attribution and candidate sponsorship, riding on the
        // institution portal's partner_users auth. partner_type widens the directory to
        // institutions/sponsors; a discount code carrying partner_id attributes its paid redemptions
        // to that partner, from which commission accrual is DERIVED on demand (no hook in the payment
        // path). Payouts are the only materialized ledger rows; balance = accrued − paid out.
        AddCol("training_partners", "partner_type", "partner_type TEXT DEFAULT 'training'");
        AddCol("training_partners", "contact_name", "contact_name TEXT");
        AddCol("training_partners", "commission_pct", "commission_pct REAL DEFAULT 0");
        AddCol("training_partners", "sponsor_enabled", "sponsor_enabled INTEGER DEFAULT 0");
        AddCol("discount_codes", "partner_id", "partner_id INTEGER");
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_sponsorships(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            partner_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            application_id INTEGER,
            certification_id INTEGER NOT NULL DEFAULT 1,
            route_key TEXT DEFAULT 'sponsored',
            candidate_email TEXT,
            candidate_name TEXT,
            status TEXT DEFAULT 'registered',
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_sponsorships_partner ON partner_sponsorships(partner_id)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_sponsorship_candidate ON partner_sponsorships(partner_id, user_id, certification_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_payouts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            partner_id INTEGER NOT NULL,
            amount REAL NOT NULL,
            currency TEXT DEFAULT 'USD',
            note TEXT,
            paid_by INTEGER,
            paid_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_payouts_partner ON partner_payouts(partner_id)");
        // Certuvo practice attempts (Phase 8): one row per practice quiz / mock — formative, un-proctored,
        // separate from the certifying exam_attempts. Stores the served question set, the answers and the
        // graded score with a per-domain breakdown.
        db.Exec(@"CREATE TABLE IF NOT EXISTS practice_attempts(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,mode TEXT NOT NULL DEFAULT 'quiz',domain TEXT,question_ids TEXT,answers TEXT,score INTEGER,total INTEGER,domain_breakdown TEXT,status TEXT NOT NULL DEFAULT 'in_progress',duration_seconds INTEGER,started_at TEXT DEFAULT (datetime('now')),completed_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_practice_user ON practice_attempts(user_id)");
        // ERP / integrations foundation (Phase 9): a durable event outbox, a connector registry with a
        // write-only secret, and a per-event-per-connector delivery ledger with retry/backoff.
        db.Exec(@"CREATE TABLE IF NOT EXISTS integrations(id INTEGER PRIMARY KEY AUTOINCREMENT,provider TEXT NOT NULL DEFAULT 'webhook',name TEXT,enabled INTEGER DEFAULT 0,endpoint_url TEXT,secret TEXT,event_filter TEXT,config TEXT,status TEXT DEFAULT 'idle',last_delivery_at TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS integration_events(id INTEGER PRIMARY KEY AUTOINCREMENT,event_type TEXT NOT NULL,entity_type TEXT,entity_id INTEGER,payload TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_intevent_created ON integration_events(created_at)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS integration_deliveries(id INTEGER PRIMARY KEY AUTOINCREMENT,event_id INTEGER NOT NULL,integration_id INTEGER NOT NULL,status TEXT NOT NULL DEFAULT 'pending',attempts INTEGER DEFAULT 0,response_code INTEGER,last_error TEXT,next_attempt_at TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_intdel_pending ON integration_deliveries(status, next_attempt_at)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_intdel_event_int ON integration_deliveries(event_id, integration_id)");
        // Exam Delivery integrations: connect the platform to third-party certification exam-delivery /
        // proctoring vendors (Pearson VUE/OnVUE, Kryterion Webassessor, PSI, TestReach, Questionmark).
        //   exam_delivery_providers   — one configured connector per vendor (write-only secret, sandbox/prod,
        //                               per-provider config incl. certification→vendor-exam-code mapping)
        //   exam_delivery_orders      — one row per booking routed to a vendor: the external candidate /
        //                               registration / appointment ids and the lifecycle status + result
        //   exam_delivery_log         — an append-only audit of every vendor API operation (op, code, ok)
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_delivery_providers(id INTEGER PRIMARY KEY AUTOINCREMENT,provider VARCHAR(40) NOT NULL,name TEXT,enabled INTEGER DEFAULT 0,is_default INTEGER DEFAULT 0,environment TEXT DEFAULT 'sandbox',config TEXT,secret TEXT,status TEXT DEFAULT 'idle',last_sync_at TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_exdelprov_provider ON exam_delivery_providers(provider)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_delivery_orders(id INTEGER PRIMARY KEY AUTOINCREMENT,provider_id INTEGER NOT NULL,provider TEXT NOT NULL,user_id INTEGER NOT NULL,booking_id INTEGER,attempt_id INTEGER,certification_id INTEGER,vendor_exam_code TEXT,delivery_type TEXT DEFAULT 'online',status VARCHAR(32) NOT NULL DEFAULT 'pending',external_candidate_id TEXT,external_registration_id TEXT,external_appointment_id TEXT,confirmation_code TEXT,scheduled_at TEXT,timezone TEXT,result_status TEXT,score REAL,max_score REAL,raw_result TEXT,last_error TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_exdelorder_booking ON exam_delivery_orders(booking_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_exdelorder_status ON exam_delivery_orders(status)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_exdelorder_booking_prov ON exam_delivery_orders(booking_id, provider_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_delivery_log(id INTEGER PRIMARY KEY AUTOINCREMENT,order_id INTEGER,provider_id INTEGER,provider TEXT,operation TEXT NOT NULL,ok INTEGER DEFAULT 0,response_code INTEGER,detail TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_exdellog_order ON exam_delivery_log(order_id)");
        // Certuvo external practice platform: an account provisioned for a member (credentials shared in the
        // student panel). Provisioned automatically when a membership is settled, or manually by an admin.
        db.Exec(@"CREATE TABLE IF NOT EXISTS certuvo_accounts(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL UNIQUE,external_id TEXT,username TEXT,secret TEXT,login_url TEXT,status TEXT DEFAULT 'pending',last_error TEXT,provisioned_at TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        // Certuvo hardening: retry queue with backoff, suspend/revoke lifecycle, first-login confirmation
        // (webhook), per-account idempotency key so repeated settlements/webhooks can never double-create.
        AddCol("certuvo_accounts", "retry_count", "retry_count INTEGER DEFAULT 0");
        AddCol("certuvo_accounts", "next_retry_at", "next_retry_at TEXT");
        AddCol("certuvo_accounts", "suspended_at", "suspended_at TEXT");
        AddCol("certuvo_accounts", "revoked_at", "revoked_at TEXT");
        AddCol("certuvo_accounts", "activated_at", "activated_at TEXT");
        AddCol("certuvo_accounts", "credentials_sent_at", "credentials_sent_at TEXT");
        AddCol("certuvo_accounts", "idempotency_key", "idempotency_key TEXT");
        // PCI-generated, PCI-controlled login: a unique username independent of the student's email, a
        // temporary password (stored encrypted, never the email), first-login-change tracking, and an
        // email-conflict marker so an existing Certuvo account under the same email is flagged, never
        // overwritten. eligible_reason records WHY a member qualified (paid/waived/honorary/…).
        AddCol("certuvo_accounts", "must_change_password", "must_change_password INTEGER DEFAULT 1");
        AddCol("certuvo_accounts", "email_conflict", "email_conflict INTEGER DEFAULT 0");
        AddCol("certuvo_accounts", "eligible_reason", "eligible_reason TEXT");
        AddCol("certuvo_accounts", "member_type", "member_type TEXT");
        AddCol("certuvo_accounts", "username_regenerated_at", "username_regenerated_at TEXT");
        AddCol("certuvo_accounts", "password_reset_at", "password_reset_at TEXT");
        // Finance controls: structured metadata on payments (offline settlements, waivers, reversals) so
        // manual money movements carry the same evidence a gateway payment does.
        AddCol("payments", "method", "method TEXT");                       // bank_transfer | cheque | invoice | gateway | other
        AddCol("payments", "bank_reference", "bank_reference TEXT");
        AddCol("payments", "gateway_reference", "gateway_reference TEXT");
        AddCol("payments", "receipt_no", "receipt_no TEXT");
        AddCol("payments", "note", "note TEXT");
        AddCol("payments", "recorded_by", "recorded_by INTEGER");          // admin_users.id for manual entries
        AddCol("payments", "waived_amount", "waived_amount REAL");
        AddCol("payments", "reversed_at", "reversed_at TEXT");
        AddCol("payments", "reversed_by", "reversed_by INTEGER");
        AddCol("payments", "reversal_reason", "reversal_reason TEXT");
        // Waiver ledger: every full/partial waiver as a first-class record (who, what, why, how much).
        db.Exec(@"CREATE TABLE IF NOT EXISTS fee_waivers(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,product_type VARCHAR(24),certification_id INTEGER,kind VARCHAR(16) DEFAULT 'full',original_amount REAL,waived_amount REAL,final_amount REAL,currency VARCHAR(8) DEFAULT 'USD',reason TEXT,note TEXT,approved_by INTEGER,code_id INTEGER,payment_id INTEGER,expires_at TEXT,status VARCHAR(16) DEFAULT 'granted',created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_fee_waivers_user ON fee_waivers(user_id)");

        // ── Exam Exceptions & Authorizations ──────────────────────────────────────────────────────────
        // Admin-managed deadline extensions, rescheduling, reattempts, fee waivers and exam-incident cases.
        // The authorization record is the admin-facing, configurable scheduling-window + attempt-policy +
        // status anchor for one exam entitlement. The booking flow keeps reading payments.exam_schedule_deadline
        // (which an extension writes through to), so scheduling stays backward-compatible; this layer adds
        // history, attempt policy and exception workflow on top. Nothing here hardcodes a one-year period.
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_authorizations(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,certification_id INTEGER DEFAULT 1,payment_id INTEGER,entitlement_id INTEGER,eligibility_start TEXT,original_deadline TEXT,current_deadline TEXT,access_expiry TEXT,attempts_permitted INTEGER DEFAULT 1,attempts_used INTEGER DEFAULT 0,retake_wait_until TEXT,window_days INTEGER,window_source TEXT,route_key TEXT,campaign TEXT,institution_id INTEGER,country TEXT,status TEXT DEFAULT 'active',notes TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examauth_user ON exam_authorizations(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examauth_cert ON exam_authorizations(certification_id)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_examauth_payment ON exam_authorizations(payment_id) WHERE payment_id IS NOT NULL");

        // Configurable scheduling windows: resolved by precedence individual > campaign > institution >
        // country > exam > route > certification > global default (setting exam_default_window_days).
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_window_rules(id INTEGER PRIMARY KEY AUTOINCREMENT,scope_type VARCHAR(40) NOT NULL,scope_value VARCHAR(190),window_days INTEGER,access_expiry_days INTEGER,attempts_permitted INTEGER,retake_wait_days INTEGER,active INTEGER DEFAULT 1,note TEXT,created_by INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examwindow_scope ON exam_window_rules(scope_type, scope_value)");

        // Deadline extension history — the original deadline is preserved on the authorization; each grant is a row.
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_extension_history(id INTEGER PRIMARY KEY AUTOINCREMENT,authorization_id INTEGER,user_id INTEGER NOT NULL,certification_id INTEGER DEFAULT 1,previous_deadline TEXT,new_deadline TEXT,previous_expiry TEXT,new_expiry TEXT,added_days INTEGER,reason TEXT,note TEXT,fee_applies INTEGER DEFAULT 0,is_free INTEGER DEFAULT 1,evidence_ref TEXT,approved_by INTEGER,approved_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examext_auth ON exam_extension_history(authorization_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examext_user ON exam_extension_history(user_id)");

        // Reschedule history — the original appointment is preserved as history, never deleted.
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_reschedule_history(id INTEGER PRIMARY KEY AUTOINCREMENT,booking_id INTEGER,authorization_id INTEGER,user_id INTEGER NOT NULL,certification_id INTEGER DEFAULT 1,previous_scheduled_at TEXT,previous_timezone TEXT,previous_status TEXT,new_scheduled_at TEXT,new_timezone TEXT,delivery_change TEXT,provider_change TEXT,reason TEXT,note TEXT,fee_applies INTEGER DEFAULT 0,fee_waived INTEGER DEFAULT 0,changed_by INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examresched_user ON exam_reschedule_history(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examresched_booking ON exam_reschedule_history(booking_id)");

        // Attempt grants — a replacement/additional/complimentary/paid/etc. opportunity, classified and
        // optionally tied to an incident. counts_as_attempt=0 = a verified system/provider failure that must
        // not consume the candidate's attempt allowance.
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_attempt_grants(id INTEGER PRIMARY KEY AUTOINCREMENT,authorization_id INTEGER,user_id INTEGER NOT NULL,certification_id INTEGER DEFAULT 1,grant_type TEXT DEFAULT 'additional',counts_as_attempt INTEGER DEFAULT 1,reason TEXT,note TEXT,incident_id INTEGER,payment_id INTEGER,fee_applies INTEGER DEFAULT 0,fee_waived INTEGER DEFAULT 1,status TEXT DEFAULT 'granted',consumed_attempt_id INTEGER,approved_by INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examgrant_user ON exam_attempt_grants(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examgrant_auth ON exam_attempt_grants(authorization_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examgrant_incident ON exam_attempt_grants(incident_id)");

        // Exam incidents — first-class case record for exceptional exam situations, modelled on the appeals/
        // accommodation decision workflow. Links technical/proctoring context → disposition → remedy.
        db.Exec(@"CREATE TABLE IF NOT EXISTS exam_incidents(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,certification_id INTEGER DEFAULT 1,attempt_id INTEGER,booking_id INTEGER,authorization_id INTEGER,category TEXT,occurred_at TEXT,student_explanation TEXT,proctor_report TEXT,tech_logs TEXT,evidence_ref TEXT,evidence_name TEXT,severity TEXT DEFAULT 'medium',status VARCHAR(24) DEFAULT 'received',investigation_result TEXT,decision TEXT,remedy TEXT,reported_by TEXT DEFAULT 'student',created_by INTEGER,decided_by INTEGER,decided_at TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examincident_user ON exam_incidents(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examincident_status ON exam_incidents(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_examincident_attempt ON exam_incidents(attempt_id)");

        // Attempt classification + linkage (integrity: every attempt is preserved AND labelled).
        AddCol("exam_attempts", "attempt_class", "attempt_class TEXT DEFAULT 'normal'");   // normal|replacement|additional|complimentary|technical
        AddCol("exam_attempts", "authorization_id", "authorization_id INTEGER");
        AddCol("exam_attempts", "grant_id", "grant_id INTEGER");
        AddCol("exam_attempts", "replaces_attempt_id", "replaces_attempt_id INTEGER");
        AddCol("exam_attempts", "counts_as_attempt", "counts_as_attempt INTEGER DEFAULT 1");
        AddCol("exam_attempts", "invalidation_reason", "invalidation_reason TEXT");
        AddCol("exam_attempts", "invalidated_by", "invalidated_by INTEGER");

        // Waiver ledger extensions — exam/retake/reschedule fee types + sponsor/institution/incident linkage.
        AddCol("fee_waivers", "fee_type", "fee_type TEXT DEFAULT 'exam'");           // exam|retake|reschedule
        AddCol("fee_waivers", "waiver_type", "waiver_type TEXT");                    // full|partial|complimentary|sponsor|institution|campaign|technical|administrative|test_user
        AddCol("fee_waivers", "sponsor", "sponsor TEXT");
        AddCol("fee_waivers", "institution_id", "institution_id INTEGER");
        AddCol("fee_waivers", "incident_id", "incident_id INTEGER");
        AddCol("fee_waivers", "appeal_id", "appeal_id INTEGER");
        AddCol("fee_waivers", "evidence_ref", "evidence_ref TEXT");
        AddCol("fee_waivers", "payable_amount", "payable_amount REAL");

        // Authorization links so history + policy join cleanly.
        AddCol("exam_bookings", "authorization_id", "authorization_id INTEGER");
        AddCol("exam_entitlements", "authorization_id", "authorization_id INTEGER");

        // Configurable-window defaults (no hardcoded year in code; operator-tunable via Settings) + the
        // exam-exception notification master toggle (per-event toggles are declared in Notifications.cs).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('exam_default_window_days','365')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('exam_default_access_expiry_days','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('exam_default_attempts','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('exam_default_retake_wait_days','0')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_exam_exception_enabled','1')");
        // ──────────────────────────────────────────────────────────────────────────────────────────────

        // Institution (training-partner) sponsorship limits + partner-linked discount codes.
        AddCol("training_partners", "max_discount_percent", "max_discount_percent REAL");
        AddCol("training_partners", "max_codes", "max_codes INTEGER");
        AddCol("training_partners", "max_uses_per_code", "max_uses_per_code INTEGER");
        AddCol("training_partners", "total_allocation", "total_allocation INTEGER");
        AddCol("training_partners", "allow_full_sponsorship", "allow_full_sponsorship INTEGER DEFAULT 0");
        AddCol("discount_codes", "partner_id", "partner_id INTEGER");

        // ============ Support, institutions & discount-engine v2 ============
        // Student-facing error references: every important failure gets a PCI-YYYY-NNNNNN reference the
        // student can quote and support can search. Never stores passwords/tokens/card data.
        db.Exec(@"CREATE TABLE IF NOT EXISTS error_reports(id INTEGER PRIMARY KEY AUTOINCREMENT,reference VARCHAR(24) UNIQUE NOT NULL,user_id INTEGER,page TEXT,category VARCHAR(40) DEFAULT 'general',user_message TEXT,tech_summary TEXT,browser TEXT,os TEXT,app_version TEXT,related_type VARCHAR(32),related_id INTEGER,status VARCHAR(16) DEFAULT 'open',resolution_note TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_error_reports_user ON error_reports(user_id)");
        // Impersonation ledger: who acted as whom, why, when — plus every page the staff session touched.
        db.Exec(@"CREATE TABLE IF NOT EXISTS impersonation_sessions(id INTEGER PRIMARY KEY AUTOINCREMENT,token_sha VARCHAR(64) UNIQUE NOT NULL,admin_id INTEGER NOT NULL,user_id INTEGER NOT NULL,reason TEXT,started_at TEXT DEFAULT (datetime('now')),ended_at TEXT,last_seen_at TEXT)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS impersonation_events(id INTEGER PRIMARY KEY AUTOINCREMENT,session_id INTEGER NOT NULL,method VARCHAR(8),path TEXT,at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_imp_events_session ON impersonation_events(session_id)");
        // Customer-service layer on tickets: assignment, tags, escalation, SLA stamps, internal notes,
        // canned templates, satisfaction rating.
        AddCol("tickets", "assigned_to", "assigned_to INTEGER");            // admin_users.id
        AddCol("tickets", "tags", "tags TEXT");
        AddCol("tickets", "escalated", "escalated INTEGER DEFAULT 0");
        AddCol("tickets", "first_response_at", "first_response_at TEXT");
        AddCol("tickets", "resolved_at", "resolved_at TEXT");
        AddCol("tickets", "rating", "rating INTEGER");                      // 1-5 CSAT, student-set after resolve
        AddCol("tickets", "followup_at", "followup_at TEXT");
        db.Exec(@"CREATE TABLE IF NOT EXISTS ticket_notes(id INTEGER PRIMARY KEY AUTOINCREMENT,ticket_id INTEGER NOT NULL,admin_id INTEGER NOT NULL,body TEXT NOT NULL,mentions TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_ticket_notes_ticket ON ticket_notes(ticket_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS support_templates(id INTEGER PRIMARY KEY AUTOINCREMENT,title TEXT NOT NULL,body TEXT NOT NULL,category VARCHAR(40),active INTEGER DEFAULT 1,created_by INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        // (chat_sessions.assigned_to / linked_user_id are added just after that table is created, below.)
        // Institution portal accounts: each institution gets its own logins, wholly separate from both
        // admin_users and students. Sessions mirror admin_sessions (sha token + expiry).
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_users(id INTEGER PRIMARY KEY AUTOINCREMENT,partner_id INTEGER NOT NULL,email VARCHAR(255) UNIQUE NOT NULL,name TEXT,role VARCHAR(24) DEFAULT 'admin',password_hash TEXT,status VARCHAR(16) DEFAULT 'active',must_change_pw INTEGER DEFAULT 1,last_login_at TEXT,created_by INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_partner_users_partner ON partner_users(partner_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_sessions(id INTEGER PRIMARY KEY AUTOINCREMENT,partner_user_id INTEGER NOT NULL,token VARCHAR(64) UNIQUE NOT NULL,expires_at TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS partner_notices(id INTEGER PRIMARY KEY AUTOINCREMENT,partner_id INTEGER NOT NULL,title TEXT,body TEXT,read_at TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_partner_notices_partner ON partner_notices(partner_id)");
        // Institution agreement + privacy scope.
        AddCol("training_partners", "status", "status VARCHAR(16) DEFAULT 'active'");   // active | suspended | terminated
        AddCol("training_partners", "institution_type", "institution_type TEXT");
        AddCol("training_partners", "agreement_start", "agreement_start TEXT");
        AddCol("training_partners", "agreement_end", "agreement_end TEXT");
        AddCol("training_partners", "auto_approve_codes", "auto_approve_codes INTEGER DEFAULT 0");
        AddCol("training_partners", "privacy_fields", "privacy_fields TEXT");            // JSON list of extra fields the partner may see
        AddCol("training_partners", "eligible_countries", "eligible_countries TEXT");
        // Test institutions (mirrors users.is_test): scenario-based partner-portal test accounts,
        // never listed publicly and excluded from discount/commission reporting.
        AddCol("training_partners", "is_test", "is_test INTEGER DEFAULT 0");
        // Discount-engine v2: full lifecycle + constraints. Legacy rows keep status NULL and are governed
        // by the existing `active` flag; new engine rows use the status column as the source of truth.
        AddCol("discount_codes", "status", "status VARCHAR(20)");                        // draft|pending_approval|active|suspended|rejected|cancelled
        AddCol("discount_codes", "created_by_partner_user", "created_by_partner_user INTEGER");
        AddCol("discount_codes", "approved_by", "approved_by INTEGER");
        AddCol("discount_codes", "approved_at", "approved_at TEXT");
        AddCol("discount_codes", "rejection_reason", "rejection_reason TEXT");
        AddCol("discount_codes", "campaign_name", "campaign_name TEXT");
        AddCol("discount_codes", "min_payable", "min_payable REAL");
        AddCol("discount_codes", "eligible_countries", "eligible_countries TEXT");       // CSV of ISO/plain names; NULL = all
        // Fraud/abuse review queue — nothing is auto-blocked without a human path.
        db.Exec(@"CREATE TABLE IF NOT EXISTS fraud_flags(id INTEGER PRIMARY KEY AUTOINCREMENT,kind VARCHAR(40) NOT NULL,code_id INTEGER,user_id INTEGER,email TEXT,detail TEXT,status VARCHAR(16) DEFAULT 'open',actioned_by INTEGER,actioned_at TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_fraud_flags_status ON fraud_flags(status)");
        // Optional TOTP MFA for admin accounts (privileged logins).
        AddCol("admin_users", "totp_secret", "totp_secret TEXT");
        AddCol("admin_users", "totp_last_step", "totp_last_step INTEGER");   // replay guard: last consumed TOTP timestep
        AddCol("admin_users", "totp_recovery", "totp_recovery TEXT");        // one-time MFA recovery codes (SHA-256 hashes, JSON array)
        // Per-account brute-force lockout (complements the per-IP rate limiter) on every password login.
        foreach (var t in new[] { "users", "admin_users", "partner_users" })
        {
            AddCol(t, "failed_logins", "failed_logins INTEGER DEFAULT 0");
            AddCol(t, "lockout_until", "lockout_until TEXT");
        }
        db.Exec(@"CREATE TABLE IF NOT EXISTS notification_history(id INTEGER PRIMARY KEY AUTOINCREMENT,channel TEXT NOT NULL DEFAULT 'email',recipient TEXT,subject TEXT,status TEXT,related_type TEXT,related_id INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_enabled','1')");
        // Whether newly-created / password-reset admins are forced to change their password at first sign-in.
        // Operator-configurable from Settings (default 1 = on, the secure default). "1"/"0".
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('admin_force_password_change','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_admin_email','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_ack_subject','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_admin_subject','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_partners_enabled','1')");
        // ── Social Media Management (Endpoints/Social.cs, Core/SocialIcons.cs, Core/ListSections.cs) ──
        // The database is the source of truth for every public social-media profile and every
        // page-sharing button — nothing is hardcoded in React. social_enabled is the master on/off
        // switch (off by default); the per-network site_settings keys below are the LEGACY store,
        // kept only so an existing deployment's URLs are one-time migrated into social_accounts.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('social_enabled','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('social_analytics_enabled','1')");
        foreach (var k in new[] { "social_linkedin", "social_x", "social_facebook", "social_instagram", "social_youtube", "social_whatsapp" })
            db.Exec($"INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('{k}','')");
        // One admin-configured social-media profile. `platform` and `locations` are free strings so a
        // new platform or a new display location needs NO schema change. Effective/expiry, language,
        // country, approval, official and link-status fields are the source of truth for the public
        // display rules and for structured-data (sameAs) inclusion.
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_accounts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform VARCHAR(40) NOT NULL,
            display_name TEXT,
            handle TEXT,
            url TEXT NOT NULL,
            icon_type VARCHAR(16) DEFAULT 'builtin',      -- builtin | custom
            custom_icon TEXT,                              -- inline SVG when icon_type='custom'
            aria_label TEXT,
            tooltip TEXT,
            locations TEXT DEFAULT 'footer',               -- CSV of location keys (footer, contact, about, …)
            display_order INTEGER DEFAULT 0,
            active INTEGER DEFAULT 1,
            is_official INTEGER DEFAULT 1,                  -- included in sameAs structured data
            open_new_tab INTEGER DEFAULT 1,
            rel_nofollow INTEGER DEFAULT 1,
            language VARCHAR(8),                            -- '' / NULL = all languages
            country VARCHAR(8),                            -- '' / NULL = all countries
            effective_date TEXT,                           -- '' / NULL = live now
            expiry_date TEXT,                              -- '' / NULL = never expires
            approval_status VARCHAR(16) DEFAULT 'approved',-- draft | approved | archived
            link_status VARCHAR(16) DEFAULT 'not_checked', -- not_checked | valid | warning | invalid | unreachable | disabled
            link_checked_at TEXT,
            created_by INTEGER, updated_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_accounts_order ON social_accounts(display_order, id)");
        // Change history for every social-media configuration action (add/edit/activate/archive/check).
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_audit(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            account_id INTEGER, actor_id INTEGER,
            action VARCHAR(40) NOT NULL, detail TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_audit_account ON social_audit(account_id)");
        // Broken-link validation history (one row per check run).
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_link_checks(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            account_id INTEGER NOT NULL,
            status VARCHAR(16), http_code INTEGER, detail TEXT,
            checked_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_link_checks_account ON social_link_checks(account_id)");
        // Page-sharing button configuration, per public content type. buttons is a CSV of share targets.
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_share_settings(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            content_type VARCHAR(40) NOT NULL,
            buttons TEXT DEFAULT 'linkedin,x,facebook,whatsapp,email,copy',
            enabled INTEGER DEFAULT 0)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_social_share_type ON social_share_settings(content_type)");
        foreach (var ct in new[] { "blog", "news", "certification", "resource", "download", "event" })
            db.Execute("INSERT OR IGNORE INTO social_share_settings(content_type,buttons,enabled) VALUES(?,?,0)",
                ct, "linkedin,x,facebook,whatsapp,email,copy");
        // One-time migration: fold any legacy site_settings social URLs into social_accounts, so an
        // existing deployment keeps its links. Runs only while the new table is still empty.
        try
        {
            if (db.Scalar<long>("SELECT COUNT(*) FROM social_accounts") == 0)
            {
                var legacy = new (string plat, string key)[]
                { ("linkedin","social_linkedin"), ("x","social_x"), ("facebook","social_facebook"),
                  ("instagram","social_instagram"), ("youtube","social_youtube"), ("whatsapp","social_whatsapp") };
                int ord = 0;
                foreach (var (plat, key) in legacy)
                {
                    var v = (db.Scalar<string>("SELECT svalue FROM site_settings WHERE skey=?", key) ?? "").Trim();
                    if (v.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || v.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        db.Execute("INSERT INTO social_accounts(platform,url,display_order,locations,approval_status) VALUES(?,?,?,?,?)",
                            plat, v, ord++, "footer", "approved");
                }
            }
        }
        catch { /* legacy migration is best-effort; never block boot */ }
        // Bulk / marketing email campaigns (Endpoints/Campaigns.cs): one campaign row, a per-recipient
        // delivery ledger, and a global suppression list (one-click unsubscribes + manual entries).
        // Anti-spam/CAN-SPAM/GDPR is enforced in code: every send carries an auto-appended footer with a
        // one-click unsubscribe link, and suppression is always honoured before any address is mailed.
        db.Exec(@"CREATE TABLE IF NOT EXISTS email_campaigns(id INTEGER PRIMARY KEY AUTOINCREMENT,name TEXT,subject TEXT,body_html TEXT,audience TEXT,status TEXT DEFAULT 'draft',total INTEGER DEFAULT 0,sent INTEGER DEFAULT 0,failed INTEGER DEFAULT 0,suppressed INTEGER DEFAULT 0,created_at TEXT DEFAULT (datetime('now')),sent_at TEXT)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS campaign_recipients(id INTEGER PRIMARY KEY AUTOINCREMENT,campaign_id INTEGER,email TEXT,first_name TEXT,status TEXT DEFAULT 'pending',error TEXT,sent_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_campaign_recipients_campaign ON campaign_recipients(campaign_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS email_suppression(id INTEGER PRIMARY KEY AUTOINCREMENT,email VARCHAR(255) UNIQUE NOT NULL,reason TEXT,created_at TEXT DEFAULT (datetime('now')))");
        // Optional postal address shown in the campaign footer (CAN-SPAM). Empty ⇒ line is omitted.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('org_postal_address','')");
        // SEO (master-plan Phase 3): per-page canonical/OG overrides + a managed redirect table.
        AddCol("pages", "canonical_url", "canonical_url TEXT");
        AddCol("pages", "og_image", "og_image TEXT");
        db.Exec(@"CREATE TABLE IF NOT EXISTS seo_redirects(id INTEGER PRIMARY KEY AUTOINCREMENT,from_path TEXT NOT NULL,to_url TEXT NOT NULL,status INTEGER DEFAULT 301,active INTEGER DEFAULT 1,note TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_seo_redirect_from ON seo_redirects(from_path)");

        // Search-engine integrations (master-plan Phase 4): tag/verification settings, IndexNow key
        // + submission log. IDs/tokens are admin-set in Admin → SEO → Search engines, never hardcoded.
        db.Exec(@"CREATE TABLE IF NOT EXISTS seo_submissions(id INTEGER PRIMARY KEY AUTOINCREMENT,engine TEXT NOT NULL,url_count INTEGER DEFAULT 0,status TEXT,detail TEXT,created_at TEXT DEFAULT (datetime('now')))");
        foreach (var k in new[] { "ga4_measurement_id", "gtm_container_id", "clarity_project_id", "google_site_verification", "bing_site_verification", "psi_api_key" })
            db.Exec($"INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('{k}','')");
        PCI.Backend.Core.IndexNowService.EnsureKey(db);

        // Public discussion forum (community): anonymous display-name threads/posts, community
        // flagging with auto-hold, and a rate-limit action ledger keyed by a salted truncated IP hash.
        db.Exec(@"CREATE TABLE IF NOT EXISTS forum_threads(id INTEGER PRIMARY KEY AUTOINCREMENT,category VARCHAR(64) NOT NULL,title TEXT NOT NULL,author_name TEXT NOT NULL,status VARCHAR(24) DEFAULT 'live',reply_count INTEGER DEFAULT 0,created_at TEXT DEFAULT (datetime('now')),last_post_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_forum_threads_list ON forum_threads(status, last_post_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_forum_threads_cat ON forum_threads(category, status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS forum_posts(id INTEGER PRIMARY KEY AUTOINCREMENT,thread_id INTEGER NOT NULL,author_name TEXT NOT NULL,body TEXT NOT NULL,status VARCHAR(24) DEFAULT 'live',flags INTEGER DEFAULT 0,ip_hash TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_forum_posts_thread ON forum_posts(thread_id, status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS forum_actions(id INTEGER PRIMARY KEY AUTOINCREMENT,ip_hash VARCHAR(64) NOT NULL,action VARCHAR(32) NOT NULL,created_at VARCHAR(32) DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_forum_actions_lookup ON forum_actions(ip_hash, action, created_at)");

        // Self-hosted site chat (Endpoints/Chat.cs): a bot answers first from the admin-managed
        // knowledge base (chat_kb); the visitor can escalate to a live person; every conversation is
        // kept as a full transcript for admin review. No third-party chat services involved.
        db.Exec(@"CREATE TABLE IF NOT EXISTS chat_sessions(id INTEGER PRIMARY KEY AUTOINCREMENT,token VARCHAR(64) UNIQUE NOT NULL,visitor_name TEXT,status VARCHAR(24) DEFAULT 'bot',created_at TEXT DEFAULT (datetime('now')),last_activity_at VARCHAR(32) DEFAULT (datetime('now')),ip_hash TEXT)");
        // Support-inbox additions to the (Migrate-created) chat table — added here, after the table exists.
        AddCol("chat_sessions", "assigned_to", "assigned_to INTEGER");
        AddCol("chat_sessions", "linked_user_id", "linked_user_id INTEGER");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_chat_sessions_status ON chat_sessions(status, last_activity_at)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS chat_messages(id INTEGER PRIMARY KEY AUTOINCREMENT,session_id INTEGER NOT NULL,sender TEXT NOT NULL,body TEXT NOT NULL,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_chat_messages_session ON chat_messages(session_id, id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS chat_kb(id INTEGER PRIMARY KEY AUTOINCREMENT,question TEXT NOT NULL,answer TEXT NOT NULL,keywords TEXT,enabled INTEGER DEFAULT 1,sort_order INTEGER DEFAULT 0)");
        // First-run knowledge base (only when empty — never overwrites admin edits). Answers are taken
        // from the site's published content (faq.html / handbook / footer legal), British English,
        // honest about founding status; the exam is described in the future tense, as on the site.
        try
        {
            if (db.Scalar<long>("SELECT COUNT(*) FROM chat_kb") == 0)
            {
                var kbSeed = new (string Q, string A, string K)[]
                {
                    ("What is the PCP-AI credential?",
                     "The Certified Project Controls Professional — AI (PCP-AI) is a single, rigorous credential covering project controls, cost engineering and project finance, with the governed use of artificial intelligence treated as part of the discipline. It is awarded by the Project Controls Institute and built with reference to ISO/IEC 17024 personnel-certification principles.",
                     "pcp-ai,pcp,credential,certification,what is pcp,certified project controls professional"),
                    ("What are the thirteen domains and how are they weighted?",
                     "The PCP-AI Body of Knowledge spans thirteen domains (61 Knowledge Areas), weighted 40% project accounting and finance, 40% project management principles and 20% governed AI. The Body of Knowledge page on the website describes each domain.",
                     "domains,domain,weighting,weighted,body of knowledge,knowledge areas,syllabus,thirteen,13,40"),
                    ("Am I eligible to sit the exam?",
                     "There are two routes: an experience route for practitioners with relevant project-controls experience, and a foundation route for those newer to the field. Full eligibility criteria are confirmed during enrolment — see the Eligibility Requirements page.",
                     "eligible,eligibility,qualify,qualification,requirements,experience route,foundation route,prerequisites"),
                    ("How much does it cost?",
                     "Fees are published on the website and confirmed at enrolment; fees for the inaugural cohort are confirmed when you enrol. Members receive reduced rates on examinations and events — see the fees and policies pages for the current schedule.",
                     "cost,price,pricing,fees,fee,how much,pay,payment"),
                    ("How do I enrol?",
                     "Enrolment is online: start on the Enrol page, create your account and follow the application steps. Make sure the name you enrol under matches your official identification, as identity is checked before the examination.",
                     "enrol,enroll,enrolment,enrollment,apply,application,register,sign up,begin"),
                    ("What will the examination involve?",
                     "The examination will test applied judgement rather than recall: scenario-based multiple-choice questions across the thirteen domains, weighted 40% project accounting and finance, 40% project management principles and 20% governed AI. It will be proctored, online or at a test centre.",
                     "exam,examination,format,proctored,proctoring,test,questions,multiple choice,duration,sit"),
                    ("Can I retake the exam if I do not pass?",
                     "Yes. The retake policy on the website sets out how attempts work, the interval between them and how to rebook. A near-miss usually calls for targeted revision of specific domains rather than repeating your whole preparation.",
                     "retake,retakes,resit,fail,failed,did not pass,rebook,attempts"),
                    ("What does student membership include?",
                     "Student membership enrolment is now open. Membership provides recertification and CPD tracking, a verifiable digital credential designed for recognition by employers, body-of-knowledge and AI-practice updates, and member rates on examinations and events.",
                     "membership,member,student,join,benefits,cpd,grades"),
                    ("Where can I find the candidate handbook and downloads?",
                     "The Candidate Handbook and other guidelines are on the Guidelines & Downloads page, and the governing policies (examination rules, ethics code, appeals and complaints) are collected on the Policies page.",
                     "handbook,download,downloads,guidelines,documents,pdf,policies,rules"),
                    ("How can I contact the team?",
                     "You can email hello@projectcontrolsinstitute.org or use the Contact page on the website. If you would rather continue here, ask for a member of the team and this chat will be passed to them.",
                     "contact,email,phone,reach,enquiry,inquiries,support,address"),
                    ("Is PCI accredited?",
                     "PCI is an independent certifying body — a Delaware Non-Stock Corporation pursuing 501(c)(3) tax-exempt recognition, not yet granted. It is not currently accredited by ANAB, IAS or any ISO/IEC 17024 accreditation body; the certification framework is being developed with reference to ISO/IEC 17024 personnel-certification principles. We describe our status honestly at every stage.",
                     "accredited,accreditation,iso,17024,nonprofit,501,founding,status,recognised,recognized,legitimate"),
                    ("Why is AI part of a project-controls credential?",
                     "Because AI is already in the toolkit. The PCP-AI assesses the applied, governed use of AI across forecasting, risk and reporting — not generic AI literacy. Every output a certified professional relies on must be explainable, validated and owned by a competent human. In short: AI proposes; the professional disposes.",
                     "ai,artificial intelligence,ai policy,governed ai,ai standard,governance,automation"),
                };
                var kbOrder = 0;
                foreach (var (q, a, k) in kbSeed)
                    db.Execute("INSERT INTO chat_kb(question,answer,keywords,enabled,sort_order) VALUES(?,?,?,1,?)", q, a, k, kbOrder += 10);
                Console.WriteLine($"[seed] chat knowledge base seeded: {kbSeed.Length} answers");
            }
        }
        catch { /* chat_kb may not exist on a very first pass; ignored */ }

        // First-party analytics (master-plan Phase 5): privacy-first event ledger.
        db.Exec(@"CREATE TABLE IF NOT EXISTS analytics_events(id INTEGER PRIMARY KEY AUTOINCREMENT,event TEXT NOT NULL,path TEXT,visitor TEXT,user_id INTEGER,country TEXT,device TEXT,browser TEXT,utm_source TEXT,utm_medium TEXT,utm_campaign TEXT,referrer TEXT,landing TEXT,value DECIMAL(12,2),currency TEXT,detail TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_analytics_event_time ON analytics_events(event, created_at)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_analytics_time ON analytics_events(created_at)");

        // Public-website translations (Phase 3 multilingual): one row per captured region per language.
        db.Exec(@"CREATE TABLE IF NOT EXISTS content_i18n(id INTEGER PRIMARY KEY AUTOINCREMENT,lang TEXT NOT NULL,scope TEXT NOT NULL,slug TEXT NOT NULL DEFAULT '',ckey TEXT NOT NULL,cvalue TEXT,updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_content_i18n ON content_i18n(lang, scope, slug, ckey)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_content_i18n_lang ON content_i18n(lang)");
        // Backend-owned translation provider (owner-set in Admin → Translations; never hardcoded).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('translate_provider','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('translate_api_key','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('translate_model','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('translate_endpoint','')");
        // NOTE: the three Membership route nav_items are added in SeedContent AFTER its IfEmpty bulk seed
        // — adding them here (Migrate runs first) would make nav_items non-empty and suppress that seed.
        foreach (var (c, d) in new[]{
            ("violations","violations INTEGER DEFAULT 0"),("identity_result","identity_result TEXT"),
            ("identity_confidence","identity_confidence REAL"),("evidence_count","evidence_count INTEGER DEFAULT 0"),
            ("review_status","review_status TEXT DEFAULT 'unreviewed'"),("review_note","review_note TEXT"),
            ("reviewed_at","reviewed_at TEXT"),("client_kind","client_kind TEXT DEFAULT 'browser'"),
            ("last_heartbeat_at","last_heartbeat_at TEXT")})
            AddCol("exam_attempts", c, d);
        AddCol("student_profiles", "linkedin_url", "linkedin_url TEXT");
        AddCol("student_profiles", "profile_photo", "profile_photo TEXT");
        // Opt-in public member directory ("find a certified professional"). Off by default; a member only
        // appears once they opt in AND hold an active credential. Per-field visibility toggles respect consent.
        AddCol("student_profiles", "directory_opt_in", "directory_opt_in INTEGER DEFAULT 0");
        AddCol("student_profiles", "directory_headline", "directory_headline TEXT");
        AddCol("student_profiles", "directory_show_country", "directory_show_country INTEGER DEFAULT 1");
        AddCol("student_profiles", "directory_show_org", "directory_show_org INTEGER DEFAULT 1");
        AddCol("student_profiles", "directory_show_linkedin", "directory_show_linkedin INTEGER DEFAULT 0");
        AddCol("users", "two_factor_enabled", "two_factor_enabled INTEGER DEFAULT 0");
        // Student TOTP secret + replay guard (mirrors admin_users). An admin can clear these to recover a
        // candidate who has lost their authenticator (see /api/admin/students/{id}/reset-2fa).
        AddCol("users", "totp_secret", "totp_secret TEXT");
        AddCol("users", "totp_last_step", "totp_last_step INTEGER");
        // One-time recovery codes (SHA-256 hashed, JSON array) issued when a student enables 2FA, so a lost
        // authenticator never means a permanent lockout. Consumed on use; cleared when 2FA is disabled.
        AddCol("users", "totp_recovery", "totp_recovery TEXT");
        // Stage 4: table-backed public sections — extra display fields for pre-existing databases
        AddCol("resources", "description", "description TEXT");
        AddCol("bok_domains", "bullets", "bullets TEXT");
        AddCol("news", "url", "url TEXT");
        // first-run content for those sections (only when a table is empty — never overwrites edits)
        SeedContent.Run(db);

        // Content correction for live DBs: the donate page was seeded asserting PCI *is* a 501(c)(3)
        // (implying tax-deductible gifts) when it is only *pursuing* recognition. INSERT OR IGNORE
        // won't rewrite an already-seeded row, so correct it in place. Idempotent (LIKE guard).
        db.Exec(@"UPDATE pages SET title='Donate — Support the Project Controls Institute (pursuing 501(c)(3))'
                  WHERE slug='donate.html' AND title LIKE '%(501(c)(3))' AND title NOT LIKE '%pursuing%'");

        // ============ Student Documents & Resources module ============
        // Admin-uploaded documents made available to selected students/groups, with versioning,
        // acknowledgement, secure authenticated download, and a full download/view audit. Distinct
        // from the existing URL-based `resources` list (public links): these are private, per-student
        // assigned FILES in content-addressed storage. All columns are additive/idempotent; indexed
        // columns use VARCHAR (MySQL cannot index bare TEXT).
        //   document_categories       — configurable categories (admin-managed)
        //   documents                 — the master record (metadata + stored file + lifecycle + version chain)
        //   document_assignments      — the concrete per-student grants (who can see each document)
        //   document_downloads        — immutable view/download audit
        //   document_acknowledgements — recorded acknowledgements (one per student per document)
        db.Exec(@"CREATE TABLE IF NOT EXISTS document_categories(id INTEGER PRIMARY KEY AUTOINCREMENT,name VARCHAR(120) NOT NULL,slug VARCHAR(120),description TEXT,active INTEGER DEFAULT 1,sort_order INTEGER DEFAULT 0,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS documents(id INTEGER PRIMARY KEY AUTOINCREMENT,title VARCHAR(255) NOT NULL,description TEXT,category VARCHAR(120),doc_type VARCHAR(60) DEFAULT 'general',
            status VARCHAR(20) NOT NULL DEFAULT 'draft',
            storage_ref TEXT,filename VARCHAR(255),mime VARCHAR(120),size_bytes INTEGER,sha256 VARCHAR(64),
            version INTEGER DEFAULT 1,root_id INTEGER,supersedes_id INTEGER,superseded_by INTEGER,
            assignment_type VARCHAR(40) DEFAULT 'all',assignment_config TEXT,
            view_only INTEGER DEFAULT 0,restricted_until TEXT,ack_required INTEGER DEFAULT 0,watermark INTEGER DEFAULT 0,include_test INTEGER DEFAULT 0,
            publish_at TEXT,expires_at TEXT,published_at TEXT,archived_at TEXT,visible_to_cs INTEGER DEFAULT 1,
            created_by INTEGER,updated_by INTEGER,reject_reason TEXT,is_test INTEGER DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_documents_status ON documents(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_documents_root ON documents(root_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS document_assignments(id INTEGER PRIMARY KEY AUTOINCREMENT,document_id INTEGER NOT NULL,user_id INTEGER NOT NULL,
            assignment_type VARCHAR(40),source VARCHAR(20) DEFAULT 'auto',status VARCHAR(20) DEFAULT 'active',
            assigned_by INTEGER,assigned_at TEXT DEFAULT (datetime('now')),revoked_by INTEGER,revoked_at TEXT,revoke_reason TEXT)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_docassign ON document_assignments(document_id, user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_docassign_user ON document_assignments(user_id, status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS document_downloads(id INTEGER PRIMARY KEY AUTOINCREMENT,document_id INTEGER,user_id INTEGER,actor TEXT,role VARCHAR(20),
            ip VARCHAR(64),action VARCHAR(20),result VARCHAR(30),version INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_docdl_doc ON document_downloads(document_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS document_acknowledgements(id INTEGER PRIMARY KEY AUTOINCREMENT,document_id INTEGER NOT NULL,user_id INTEGER NOT NULL,
            ip VARCHAR(64),acknowledged_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_docack ON document_acknowledgements(document_id, user_id)");
        // Replacement provenance: why a version superseded its predecessor, and — when a version was
        // created by restoring an earlier one — which version it was restored from. Additive AddCol so
        // existing installs upgrade in place.
        AddCol("documents", "replace_reason", "replace_reason TEXT");
        AddCol("documents", "restored_from_id", "restored_from_id INTEGER");
        // First-run category set (only when empty — never overwrites admin edits).
        try
        {
            if (db.Scalar<long>("SELECT COUNT(*) FROM document_categories") == 0)
            {
                var seed = new (string Name, string Slug, string Desc)[]
                {
                    ("Certificates & Credentials", "certificates", "Official certificate copies and credential letters."),
                    ("Study & Exam Materials", "study", "Handbooks, syllabi and preparation resources."),
                    ("Policies & Agreements", "policies", "Policies, terms and agreements requiring acknowledgement."),
                    ("Invoices & Receipts", "billing", "Financial documents issued to the student."),
                    ("Personal Documents", "personal", "Student-specific documents shared privately."),
                    ("General", "general", "General resources and announcements."),
                };
                var so = 0;
                foreach (var (nm, sl, de) in seed)
                    db.Execute("INSERT INTO document_categories(name,slug,description,active,sort_order) VALUES(?,?,?,1,?)", nm, sl, de, so += 10);
            }
        }
        catch { /* table may not exist on a very first pass; ignored */ }
        // Documents module notification settings (owner-configurable in Admin → Notifications).
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_documents_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('doc_email_notify_cap','500')");

        // Performance & integrity indexes on hot lookup/join columns. Every /api/me and admin-student
        // load fans out to these tables by user_id; without indexes they full-scan under the global
        // write lock. Idempotent; safe on fresh and existing databases (SQLite + MySQL via db.Exec).
        foreach (var ix in new[]
        {
            "CREATE INDEX IF NOT EXISTS ix_payments_user ON payments(user_id)",
            "CREATE INDEX IF NOT EXISTS ix_entitlements_user ON exam_entitlements(user_id)",
            "CREATE INDEX IF NOT EXISTS ix_entitlements_booking ON exam_entitlements(booking_id)",
            "CREATE INDEX IF NOT EXISTS ix_credentials_user ON issued_credentials(user_id)",
            "CREATE INDEX IF NOT EXISTS ix_memberships_user ON memberships(user_id)",
            "CREATE INDEX IF NOT EXISTS ix_bookings_payment ON exam_bookings(payment_id)",
        }) { try { db.Exec(ix); } catch { } }
        // De-duplicate any accidental duplicate student_profiles rows before enforcing one-per-user,
        // then add the UNIQUE(user_id) the fresh schema should have carried (idempotent).
        try { db.Exec("DELETE FROM student_profiles WHERE id NOT IN (SELECT MIN(id) FROM student_profiles GROUP BY user_id)"); } catch { }
        try { db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_student_profiles_user ON student_profiles(user_id)"); } catch { }
        // Older DBs whose exam_launch_codes predates the UNIQUE(code_hash) column-constraint: enforce it
        // as a unique index so a launch code can never resolve to two rows.
        try { db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_launch_code_hash ON exam_launch_codes(code_hash)"); } catch { }

        // ── Multi-certification upgrade for pre-existing databases ──
        // certifications is created by schema.sql; here we make sure every lifecycle table carries
        // certification_id and that legacy rows are attributed to the founding certification (id 1).
        foreach (var t in new[] { "sample_questions", "exam_entitlements", "exam_bookings", "exam_attempts", "issued_credentials" })
        {
            AddCol(t, "certification_id", "certification_id INTEGER DEFAULT 1");
            db.Exec($"UPDATE {t} SET certification_id=1 WHERE certification_id IS NULL");
        }
        // Application-route provenance: the route an entitlement was granted through (NULL for ordinary
        // paid entitlements), and the certificate wording snapshotted onto a credential at issue time so
        // it never drifts if the route's wording is later edited.
        AddCol("exam_entitlements", "route_key", "route_key TEXT");
        AddCol("issued_credentials", "route_key", "route_key TEXT");
        AddCol("issued_credentials", "certificate_wording", "certificate_wording TEXT");

        // ── Multi-certification catalogue fields (Phase 1) ──
        // Additive columns that make each certification fully self-describing for the public catalogue,
        // per-certification landing pages and SEO. Idempotent on both providers; no existing data touched.
        foreach (var (col, ddl) in new (string, string)[]
        {
            ("acronym","acronym TEXT"), ("short_name","short_name TEXT"), ("public_title","public_title TEXT"),
            ("tagline","tagline TEXT"), ("short_description","short_description TEXT"), ("category","category TEXT"),
            ("level","level TEXT"), ("status","status TEXT"), ("slug","slug TEXT"), ("audience","audience TEXT"),
            ("overview","overview TEXT"), ("application_fee","application_fee REAL"),
            ("membership_required","membership_required INTEGER DEFAULT 0"), ("next_exam_note","next_exam_note TEXT"),
            ("meta_title","meta_title TEXT"), ("meta_description","meta_description TEXT"), ("keywords","keywords TEXT"),
            ("og_title","og_title TEXT"), ("og_description","og_description TEXT"), ("social_image","social_image TEXT"),
            ("canonical_url","canonical_url TEXT"), ("content_json","content_json TEXT"),
            // Certuvo mapping (Phase 8): which Certuvo prep product this credential maps to, and whether enabled.
            ("certuvo_enabled","certuvo_enabled INTEGER DEFAULT 1"), ("certuvo_product","certuvo_product TEXT"),
            // CPD requirement per recertification cycle (0 = no requirement). Approved CPD hours dated within
            // the credential's current cycle must reach this before the holder can recertify.
            ("cpd_required_hours","cpd_required_hours REAL DEFAULT 0"),
            // Mandatory "AI-currency" component of the CPD requirement (0 = none): a portion of the required
            // hours that must be in the AI-currency category, keeping AI knowledge current (CPD framework).
            ("cpd_ai_hours_required","cpd_ai_hours_required REAL DEFAULT 0"),
            // Optional Credly badge-template id — maps this certification to a Credly badge template so
            // earned credentials can be exported to the Credly network (in addition to PCI's native badges).
            ("credly_template_id","credly_template_id VARCHAR(64)"),
        })
            AddCol("certifications", col, ddl);
        // Credly export tracking on each issued credential (empty until pushed). credly_state: issued|revoked|error.
        AddCol("issued_credentials", "credly_badge_id", "credly_badge_id VARCHAR(64)");
        AddCol("issued_credentials", "credly_state", "credly_state VARCHAR(16)");
        AddCol("issued_credentials", "credly_error", "credly_error TEXT");

        // ── Per-certification documents, books & study materials (Phase 8) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS cert_documents(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            certification_id INTEGER,                 -- NULL = applies to all certifications
            kind TEXT DEFAULT 'general',              -- handbook | bok | study_guide | book | form | policy | general
            title TEXT NOT NULL,
            description TEXT,
            url TEXT,                                 -- link or storage reference to the file
            route_key TEXT,                           -- NULL = all routes; else limit to one application route
            watermark INTEGER DEFAULT 0,              -- 1 = deliver as a personalised watermarked copy
            published INTEGER DEFAULT 1,
            sort_order INTEGER DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cert_documents_cert ON cert_documents(certification_id)");
        // File-backed books: uploaded materials live in private storage and are served through the
        // authenticated download endpoint (personalised watermark when flagged); `url` remains for
        // plain external links. The master file is never exposed directly.
        AddCol("cert_documents", "storage_ref", "storage_ref TEXT");
        AddCol("cert_documents", "filename", "filename TEXT");
        AddCol("cert_documents", "mime", "mime VARCHAR(80)");
        AddCol("cert_documents", "size_bytes", "size_bytes INTEGER");
        AddCol("cert_documents", "sha256", "sha256 VARCHAR(64)");
        // Immutable download audit for books/study materials — separate from document_downloads,
        // whose document_id keys the assigned-documents module.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cert_document_downloads(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            cert_document_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            copy_id VARCHAR(16),
            result VARCHAR(30),
            ip VARCHAR(64),
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_certdocdl_doc ON cert_document_downloads(cert_document_id)");

        // Superseded-file history for books/study materials. The CURRENT file stays on cert_documents
        // (backward compatible); every replace/restore snapshots the outgoing file here first, so a
        // book's bytes are never silently lost and any prior version can be inspected or restored.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cert_document_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            cert_document_id INTEGER NOT NULL,
            version INTEGER NOT NULL,
            storage_ref TEXT,
            filename VARCHAR(255),
            mime VARCHAR(80),
            size_bytes INTEGER,
            sha256 VARCHAR(64),
            replaced_by INTEGER,
            replace_reason TEXT,
            restored_from_id INTEGER,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_certdocver_doc ON cert_document_versions(cert_document_id)");

        // ── Public Downloads Centre: governance/legal/policy documents distributed to anyone, no login. ──
        // Version-chained: every version is its own row sharing a stable doc_group (the public "document ID");
        // is_current flags the live version. A public request only ever sees status='published' + visibility
        // ='public' rows, so drafts, internal, withdrawn and archived documents are never distributed.
        db.Exec(@"CREATE TABLE IF NOT EXISTS public_documents(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            doc_group VARCHAR(64) NOT NULL,            -- stable public identifier shared across versions
            title TEXT NOT NULL,
            description TEXT,
            category VARCHAR(60) DEFAULT 'general',    -- policies | certification-governance | candidate-handbooks | application-routes | exams | privacy-and-legal | fees-and-refunds | marketing-partners | institutions | accessibility | general
            certification_id INTEGER,                  -- NULL = all certifications / general
            route_key VARCHAR(40),                     -- NULL = all routes; else standard|founding|honorary|sponsored|complimentary
            language VARCHAR(10) DEFAULT 'en',
            version VARCHAR(20) DEFAULT '1.0',
            status VARCHAR(30) NOT NULL DEFAULT 'draft',  -- draft|under_review|legal_review_required|approved|scheduled|published|superseded|archived|withdrawn
            legal_review_status VARCHAR(40) DEFAULT 'draft', -- draft|internal_review_completed|external_legal_review_pending|external_legal_review_completed|approved_for_publication
            visibility VARCHAR(12) DEFAULT 'public',    -- public|internal
            owner TEXT, approver TEXT,
            effective_date TEXT, published_at TEXT, scheduled_at TEXT, next_review_date TEXT,
            storage_ref TEXT, filename TEXT, mime VARCHAR(80), size_bytes INTEGER, sha256 VARCHAR(64),
            is_current INTEGER DEFAULT 0,
            supersedes_id INTEGER,                      -- previous version row this one replaces
            related_groups TEXT,                        -- comma-separated doc_group ids of related documents
            sort_order INTEGER DEFAULT 0,
            download_count INTEGER DEFAULT 0,
            created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pubdoc_group ON public_documents(doc_group)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pubdoc_status ON public_documents(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pubdoc_cat ON public_documents(category)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS public_document_downloads(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            document_id INTEGER NOT NULL,
            doc_group VARCHAR(64),
            ip VARCHAR(64),
            ua VARCHAR(255),
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_pubdocdl_doc ON public_document_downloads(document_id)");

        // ═══════════════ Unified Communications Centre ═══════════════
        // All outbound messages (transactional/manual/bulk, email/WhatsApp/in-app) flow through a single
        // MySQL-backed outbox with a dedup key, status lifecycle and retry — so a provider outage never
        // loses the business event. Provider SECRETS live only in env vars, never in these tables.
        // Approved email identities. Credentials are NOT stored here — only the identity + policy.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_sender_profiles(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            `key` VARCHAR(60) UNIQUE NOT NULL,            -- stable code, e.g. 'support','finance','no-reply'
            name TEXT NOT NULL, display_name TEXT,
            from_email TEXT NOT NULL, reply_to TEXT,
            purpose TEXT, category VARCHAR(40) DEFAULT 'operational',
            provider VARCHAR(30) DEFAULT 'resend',
            domain_verified INTEGER DEFAULT 0,
            permitted_roles TEXT,                       -- CSV of admin perms allowed to send as this profile
            is_default INTEGER DEFAULT 0, active INTEGER DEFAULT 1, approval_status VARCHAR(20) DEFAULT 'approved',
            owner TEXT, effective_date TEXT, expiry_date TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        // Approved WhatsApp Business numbers. Tokens/app-secrets are NOT stored here — only which env var holds them.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_whatsapp_accounts(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            `key` VARCHAR(60) UNIQUE NOT NULL,
            name TEXT NOT NULL, display_name TEXT,
            phone_number TEXT NOT NULL, provider VARCHAR(30) DEFAULT 'meta_cloud',
            provider_account_id TEXT,                   -- phone_number_id / WABA id (NOT a secret)
            token_env VARCHAR(80) DEFAULT 'WHATSAPP_ACCESS_TOKEN',   -- name of the env var holding the token
            purpose TEXT, country VARCHAR(4),
            permitted_categories TEXT, permitted_roles TEXT,
            business_hours TEXT, escalation_rule TEXT,
            verification_status VARCHAR(20) DEFAULT 'unverified',
            is_default INTEGER DEFAULT 0, active INTEGER DEFAULT 1, owner TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        // Channel-agnostic templates with versioning + publish lifecycle + dynamic {{variables}}.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_templates(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            `key` VARCHAR(80) NOT NULL,
            name TEXT NOT NULL, kind VARCHAR(24) DEFAULT 'email',   -- email|whatsapp|auto_response|internal|bulk
            category VARCHAR(40) DEFAULT 'operational',
            subject TEXT, body TEXT,                    -- body: HTML (email) or text (whatsapp); {{vars}}
            wa_template_name TEXT,                      -- provider-approved WhatsApp template name, if any
            certification_id INTEGER, route_key VARCHAR(40), language VARCHAR(10) DEFAULT 'en',
            version INTEGER DEFAULT 1, status VARCHAR(20) DEFAULT 'draft',  -- draft|approved|published|archived
            required_vars TEXT, approved_by INTEGER, published_at TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_tpl_key ON comm_templates(`key`,status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_template_versions(
            id INTEGER PRIMARY KEY AUTOINCREMENT, template_id INTEGER NOT NULL, version INTEGER,
            subject TEXT, body TEXT, wa_template_name TEXT, saved_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        // Platform-event triggers: which channels/templates fire for each backend event.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_triggers(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            code VARCHAR(80) UNIQUE NOT NULL, name TEXT NOT NULL, event_group VARCHAR(40), description TEXT,
            backend_wired INTEGER DEFAULT 0,            -- 1 = a real backend event emits this code
            email_enabled INTEGER DEFAULT 1, whatsapp_enabled INTEGER DEFAULT 0, inapp_enabled INTEGER DEFAULT 1,
            email_template_key VARCHAR(80), whatsapp_template_key VARCHAR(80),
            sender_profile_key VARCHAR(60), whatsapp_account_key VARCHAR(60),
            consent_category VARCHAR(24) DEFAULT 'transactional',  -- transactional bypasses opt-out; marketing requires consent
            certification_scope TEXT, route_scope TEXT, delay_minutes INTEGER DEFAULT 0,
            reminder_sequence TEXT, conditions TEXT, dedup_window_minutes INTEGER DEFAULT 0,
            approval_required INTEGER DEFAULT 0, active INTEGER DEFAULT 1,
            effective_date TEXT, expiry_date TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        // The reliable MySQL-backed queue. One row per (business event × recipient × channel).
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_outbox(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            dedup_key VARCHAR(160),                     -- unique-ish business+delivery key; blocks duplicates
            channel VARCHAR(16) NOT NULL,               -- email|whatsapp|inapp
            trigger_code VARCHAR(80), category VARCHAR(24) DEFAULT 'operational',
            user_id INTEGER, conversation_id INTEGER, campaign_id INTEGER,
            to_email TEXT, to_phone TEXT,
            sender_profile_key VARCHAR(60), whatsapp_account_key VARCHAR(60),
            subject TEXT, body TEXT, template_key VARCHAR(80),
            certification_id INTEGER,
            status VARCHAR(20) DEFAULT 'queued',         -- draft|scheduled|queued|processing|sent|delivered|read|failed|hard_bounced|rejected|cancelled|unsubscribed|expired
            scheduled_at VARCHAR(40), attempts INTEGER DEFAULT 0, max_attempts INTEGER DEFAULT 5,   -- VARCHAR (not TEXT): it is indexed by ix_comm_outbox_status; MySQL cannot index a TEXT column
            last_error TEXT, provider VARCHAR(30), provider_message_id TEXT, provider_response TEXT,
            created_by INTEGER, next_attempt_at TEXT, sent_at TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_comm_outbox_dedup ON comm_outbox(dedup_key)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_outbox_status ON comm_outbox(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_outbox_user ON comm_outbox(user_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_delivery_attempts(
            id INTEGER PRIMARY KEY AUTOINCREMENT, outbox_id INTEGER NOT NULL, attempt INTEGER,
            status VARCHAR(20), detail TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_attempt_outbox ON comm_delivery_attempts(outbox_id)");
        // Unified inbox: one conversation per customer thread, with inbound messages of any channel.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_conversations(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reference VARCHAR(40) UNIQUE, channel VARCHAR(16), subject TEXT,
            customer_name TEXT, customer_email TEXT, customer_phone TEXT,
            user_id INTEGER, certification_id INTEGER, application_id INTEGER, payment_id INTEGER,
            received_address TEXT, assigned_admin_id INTEGER, priority VARCHAR(12) DEFAULT 'normal',
            status VARCHAR(20) DEFAULT 'open', sla_due_at TEXT, category VARCHAR(40),
            last_message_at TEXT, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_conv_status ON comm_conversations(status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_inbound_messages(
            id INTEGER PRIMARY KEY AUTOINCREMENT, conversation_id INTEGER NOT NULL,
            direction VARCHAR(8) DEFAULT 'in',          -- in|out
            channel VARCHAR(16), from_addr TEXT, to_addr TEXT, body TEXT,
            provider_message_id TEXT, is_internal_note INTEGER DEFAULT 0, author_admin_id INTEGER,
            attachments TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_inbound_conv ON comm_inbound_messages(conversation_id)");
        // Per-recipient suppression (bounces/complaints/unsubscribes) — checked before every marketing send.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_suppression(
            id INTEGER PRIMARY KEY AUTOINCREMENT, channel VARCHAR(16), address VARCHAR(190),
            reason VARCHAR(40), category VARCHAR(24), source TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_suppress_addr ON comm_suppression(channel,address)");
        // Consent + channel preferences per user (marketing requires opt-in; essential cannot be disabled).
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_preferences(
            id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER UNIQUE,
            email_marketing INTEGER DEFAULT 0, whatsapp_marketing INTEGER DEFAULT 0, whatsapp_optin INTEGER DEFAULT 0,
            newsletter INTEGER DEFAULT 0, events INTEGER DEFAULT 0, surveys INTEGER DEFAULT 0,
            consent_source TEXT, consent_version TEXT, consent_at TEXT, withdrawn_at TEXT,
            updated_at TEXT DEFAULT (datetime('now')))");
        // Bulk email/WhatsApp campaigns. Delivery rides the same outbox (one row per recipient), so
        // consent, suppression, duplicate prevention, retries and monitoring all apply uniformly.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_campaigns(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, channel VARCHAR(16) DEFAULT 'email',   -- email|whatsapp|both
            category VARCHAR(24) DEFAULT 'operational',                -- operational|marketing|newsletter
            subject TEXT, body TEXT, template_key VARCHAR(80),
            sender_profile_key VARCHAR(60), whatsapp_account_key VARCHAR(60),
            certification_id INTEGER, filters TEXT,                    -- JSON audience filters
            status VARCHAR(20) DEFAULT 'draft',                        -- draft|pending_approval|approved|scheduled|sending|paused|sent|cancelled
            scheduled_at TEXT, recurring VARCHAR(16),                  -- once|daily|weekly|monthly (foundation: once)
            approval_required INTEGER DEFAULT 1, approved_by INTEGER, approved_at TEXT,
            total INTEGER DEFAULT 0, queued INTEGER DEFAULT 0,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_campaign_status ON comm_campaigns(status)");
        // Customer-service routing rules (Phase 5): auto-triage inbound conversations. Rules are evaluated
        // in priority order and the first match applies category/priority/assignment/SLA/tags/escalation.
        // These NEVER make adverse decisions — they only classify and route; sensitive topics are always
        // escalated to a human and never auto-answered.
        db.Exec(@"CREATE TABLE IF NOT EXISTS comm_routing_rules(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL, priority INTEGER DEFAULT 100,
            match_channel VARCHAR(16),                   -- email|whatsapp|inapp|null(any)
            match_received_address TEXT,                 -- substring of the to/received address
            match_keywords TEXT,                         -- comma-separated; any token/phrase match
            match_certification_id INTEGER,
            set_category VARCHAR(40), set_priority VARCHAR(12),
            assign_admin_id INTEGER, sla_hours INTEGER,
            add_tags TEXT, escalate INTEGER DEFAULT 0,   -- 1 = force human, block auto-answer
            active INTEGER DEFAULT 1,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_comm_routing_active ON comm_routing_rules(active,priority)");
        // Auto-response bookkeeping on the conversation: a stored draft the agent can accept/edit/send,
        // free-text tags applied by routing, and whether an automated reply already went out.
        AddCol("comm_conversations", "suggested_response", "suggested_response TEXT");
        AddCol("comm_conversations", "tags", "tags TEXT");
        AddCol("comm_conversations", "auto_answered", "auto_answered INTEGER DEFAULT 0");
        AddCol("comm_conversations", "routed_rule_id", "routed_rule_id INTEGER");

        // ── Per-certification applications (Phase 4b): one record per (candidate, certification, route) ──
        db.Exec(@"CREATE TABLE IF NOT EXISTS certification_applications(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            application_no VARCHAR(40) UNIQUE,
            user_id INTEGER NOT NULL,
            certification_id INTEGER NOT NULL,
            route_key TEXT NOT NULL,
            status VARCHAR(24) NOT NULL DEFAULT 'submitted',   -- submitted | under_review | info_requested | approved | rejected | withdrawn
            workflow_stage TEXT DEFAULT 'application_submitted',
            data_json TEXT,                              -- route-specific captured fields
            blocker TEXT,
            decided_by INTEGER, decided_at TEXT, admin_note TEXT,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cert_apps_user ON certification_applications(user_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cert_apps_cert ON certification_applications(certification_id, status)");

        // ── Discount codes: certification / route / fee scoping (Phase 5) ──
        // certification_id NULL = valid for every certification; route_key NULL = every route.
        AddCol("discount_codes", "certification_id", "certification_id INTEGER");
        AddCol("discount_codes", "route_key", "route_key TEXT");
        AddCol("discount_codes", "min_transaction", "min_transaction REAL");
        AddCol("discount_codes", "max_discount", "max_discount REAL");

        // Granular per-certification admin permissions: JSON array of certification ids an admin is
        // restricted to (NULL/empty = all certifications — the default, and forced for owners).
        AddCol("admin_users", "cert_scope", "cert_scope TEXT");

        // Self-service admin password reset: single-use, expiring tokens (stored hashed) issued by the
        // "Forgot password" flow and consumed by the reset page.
        db.Exec(@"CREATE TABLE IF NOT EXISTS admin_reset_tokens(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            admin_id INTEGER NOT NULL,
            token VARCHAR(128) NOT NULL,
            expires_at TEXT NOT NULL,
            used_at TEXT,
            created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_admin_reset_token ON admin_reset_tokens(token)");

        // ══════════════════════════════════════════════════════════════════════════════════════════════
        // Content, SEO & Distribution Centre (Phase 1: dynamic Blog CMS + SEO + AI-discovery foundation)
        // A first-class article/author/category/tag model — the public "blog" was previously hand-authored
        // static HTML. Posts are stored dynamically here, rendered server-side (full article in the initial
        // HTML response) by Core/BlogRender.cs, and distributed via the existing sitemap / IndexNow / feeds.
        // Nothing is hardcoded: categories, tags, authors, statuses and SEO are all admin-managed.
        // ──────────────────────────────────────────────────────────────────────────────────────────────
        // Authors (editorial by-lines, distinct from admin_users; a reviewer/editor references admin_users.id).
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_authors(id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(160) UNIQUE NOT NULL, name VARCHAR(200) NOT NULL, title TEXT, bio TEXT,
            avatar_ref TEXT, credentials TEXT, email VARCHAR(255), links TEXT, admin_user_id INTEGER,
            active INTEGER DEFAULT 1, sort_order INTEGER DEFAULT 0,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_authors_active ON blog_authors(active)");

        // Categories (hierarchical via parent_id) and free tags with a join table.
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_categories(id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(160) UNIQUE NOT NULL, name VARCHAR(200) NOT NULL, description TEXT,
            parent_id INTEGER, certification_id INTEGER, sort_order INTEGER DEFAULT 0, active INTEGER DEFAULT 1,
            seo_title TEXT, meta_description TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_categories_active ON blog_categories(active, sort_order)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_tags(id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(160) UNIQUE NOT NULL, name VARCHAR(200) NOT NULL, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_post_tags(post_id INTEGER NOT NULL, tag_id INTEGER NOT NULL,
            PRIMARY KEY(post_id, tag_id))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_post_tags_tag ON blog_post_tags(tag_id)");

        // Posts. Hot/queried fields are real columns; the long tail of the spec's field list lives in extra_json.
        // status drives the editorial workflow; published=1 AND status='published' AND not noindex => public+indexable.
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_posts(id INTEGER PRIMARY KEY AUTOINCREMENT,
            slug VARCHAR(200) UNIQUE NOT NULL,
            title TEXT NOT NULL, seo_title TEXT, subtitle TEXT, summary TEXT,
            body TEXT, body_format VARCHAR(12) DEFAULT 'html', blocks_json TEXT,
            featured_image TEXT, featured_image_alt TEXT, social_image TEXT,
            author_id INTEGER, reviewer_id INTEGER, editor_id INTEGER,
            category_id INTEGER, topic_cluster TEXT,
            primary_keyword TEXT, secondary_keywords TEXT, search_intent VARCHAR(24), target_audience TEXT,
            language VARCHAR(8) DEFAULT 'en', certification_id INTEGER, route_key TEXT, industry TEXT,
            status VARCHAR(24) DEFAULT 'draft', published INTEGER DEFAULT 0,
            published_at TEXT, scheduled_at TEXT, original_published_at TEXT, updated_at TEXT DEFAULT (datetime('now')),
            reading_time INTEGER,
            meta_description TEXT, canonical_url TEXT, original_source_url TEXT,
            og_title TEXT, og_description TEXT, og_image TEXT,
            robots_noindex INTEGER DEFAULT 0, structured_type VARCHAR(24) DEFAULT 'BlogPosting',
            content_ownership VARCHAR(24) DEFAULT 'original', copyright_status TEXT, license TEXT, attribution TEXT,
            syndication_status VARCHAR(24) DEFAULT 'none', social_distribution INTEGER DEFAULT 1, newsletter INTEGER DEFAULT 0,
            ai_assisted INTEGER DEFAULT 0, ai_disclosure VARCHAR(32),
            internal_notes TEXT, extra_json TEXT, version INTEGER DEFAULT 1,
            approved_by INTEGER, approved_at TEXT, created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_posts_status ON blog_posts(status, published)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_posts_cat ON blog_posts(category_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_posts_author ON blog_posts(author_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_posts_pub ON blog_posts(published)");

        // Version history — a full snapshot per save; the previous version is NEVER overwritten (integrity).
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_post_versions(id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL, version INTEGER NOT NULL, status_at VARCHAR(24), snapshot_json TEXT,
            change_reason TEXT, editor_id INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_versions_post ON blog_post_versions(post_id, version)");

        // Editorial reviews/approvals per post (workflow stages: author/editorial/technical/seo/legal).
        db.Exec(@"CREATE TABLE IF NOT EXISTS blog_reviews(id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL, stage VARCHAR(24) NOT NULL, decision VARCHAR(16),
            reviewer_id INTEGER, note TEXT, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_blog_reviews_post ON blog_reviews(post_id)");

        // Integration Capability Registry — every publishing/distribution destination classified HONESTLY
        // (Direct Publishing Available | Requires Approval | Draft Export | RSS Distribution | Share Link |
        //  Manual Submission | Import Only | Read Only | Temporarily Unavailable | Unsupported | Under Review).
        // Seeded from Core/CapabilityRegistry.cs; admins never see a fake "everything is connected" claim.
        db.Exec(@"CREATE TABLE IF NOT EXISTS content_capabilities(id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_key VARCHAR(48) UNIQUE NOT NULL, platform VARCHAR(120) NOT NULL, kind VARCHAR(24) NOT NULL,
            capability VARCHAR(40) NOT NULL, publish_mode VARCHAR(32), requires_approval INTEGER DEFAULT 0,
            official_api INTEGER DEFAULT 1, doc_url TEXT, notes TEXT, connected INTEGER DEFAULT 0,
            sort_order INTEGER DEFAULT 0, active INTEGER DEFAULT 1, updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_content_caps_kind ON content_capabilities(kind, sort_order)");

        // Content job queue (outbox) — publish/indexnow/social/syndication jobs, idempotent + retried by the
        // ContentDispatcher background worker. Mirrors the integration_deliveries/comm_outbox pattern.
        db.Exec(@"CREATE TABLE IF NOT EXISTS content_jobs(id INTEGER PRIMARY KEY AUTOINCREMENT,
            job_type VARCHAR(32) NOT NULL, idempotency_key VARCHAR(120) UNIQUE, post_id INTEGER,
            target VARCHAR(48), payload TEXT, status VARCHAR(16) DEFAULT 'pending', attempts INTEGER DEFAULT 0,
            response_code INTEGER, last_error TEXT, result_ref TEXT, next_attempt_at TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_content_jobs_status ON content_jobs(status)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_content_jobs_post ON content_jobs(post_id)");

        // Social publishing (Phase 2) — connected accounts (tokens ENCRYPTED at rest via Security.EncryptSecret,
        // never returned by the API) and per-post platform-specific drafts. Only official APIs are used; the
        // no-approval connectors (Discord webhook / Telegram bot / Mastodon / Bluesky) can publish immediately,
        // while approval-gated platforms remain framework-only until the operator completes provider review.
        // NOTE: named social_pub_accounts (not social_accounts) to avoid colliding with the public
        // social-icons table defined earlier in this file, which owns the name social_accounts.
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_pub_accounts(id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_key VARCHAR(48) NOT NULL, label VARCHAR(160), external_id VARCHAR(190),
            config TEXT, secret_enc TEXT, status VARCHAR(20) DEFAULT 'connected', last_error TEXT,
            connected_by INTEGER, active INTEGER DEFAULT 1,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_pub_accounts_platform ON social_pub_accounts(platform_key)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS social_drafts(id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL, platform_key VARCHAR(48) NOT NULL, account_id INTEGER,
            text TEXT, link TEXT, hashtags TEXT, image TEXT, first_comment TEXT,
            status VARCHAR(24) DEFAULT 'draft', scheduled_at TEXT, approved_by INTEGER,
            public_url TEXT, provider_response TEXT, job_id INTEGER, created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_drafts_post ON social_drafts(post_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_social_drafts_status ON social_drafts(status)");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('social_default_base_urls','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('social_utm_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('social_utm_campaign','social')");

        // Content Syndication (Phase 3) — outbound publishing to partner CMS platforms whose official APIs need
        // no provider review (WordPress self-hosted, Ghost, Forem/DEV). Credentials ENCRYPTED at rest, never
        // returned. Every syndicated copy sets its canonical back to the PCI article, so duplicate content is
        // consolidated to the original. cc_* namespaced to avoid colliding with the Marketing Centre.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_syndication_destinations(id INTEGER PRIMARY KEY AUTOINCREMENT,
            platform_key VARCHAR(48) NOT NULL, label VARCHAR(160), base_url VARCHAR(300), config TEXT, secret_enc TEXT,
            mode VARCHAR(24) DEFAULT 'create', default_status VARCHAR(16) DEFAULT 'draft',
            status VARCHAR(20) DEFAULT 'connected', last_error TEXT, connected_by INTEGER, active INTEGER DEFAULT 1,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_syndest_platform ON cc_syndication_destinations(platform_key)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_syndicated_posts(id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL, destination_id INTEGER NOT NULL, external_id VARCHAR(190), external_url TEXT,
            canonical_url TEXT, status VARCHAR(24) DEFAULT 'pending', mode VARCHAR(24), last_error TEXT,
            provider_response TEXT, job_id INTEGER, created_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_synpost_post ON cc_syndicated_posts(post_id)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_cc_synpost_dest ON cc_syndicated_posts(post_id, destination_id)");

        // External content import (Phase 4) — ingest RSS/Atom from APPROVED sources into a review queue with a
        // licence/permission record. PCI never copies a whole third-party article merely because it is public:
        // the default is curated-link mode (a PCI-written summary + a link back to the original), full
        // republication is gated on a recorded licence/permission, and nothing goes public without review
        // unless the source is explicitly trusted for auto-publish. cc_* namespaced to avoid collisions.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_external_sources(id INTEGER PRIMARY KEY AUTOINCREMENT,
            name VARCHAR(160) NOT NULL, domain VARCHAR(190), feed_url VARCHAR(400), source_type VARCHAR(16) DEFAULT 'rss',
            owner_contact VARCHAR(190), license VARCHAR(40) DEFAULT 'all_rights_reserved', permission_ref TEXT,
            allowed_use VARCHAR(24) DEFAULT 'curated_link', attribution_required INTEGER DEFAULT 1,
            canonical_required INTEGER DEFAULT 1, auto_publish INTEGER DEFAULT 0, language VARCHAR(8),
            category_id INTEGER, active INTEGER DEFAULT 1, last_fetched_at TEXT, last_error TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_extsrc_active ON cc_external_sources(active)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_external_items(id INTEGER PRIMARY KEY AUTOINCREMENT,
            source_id INTEGER NOT NULL, guid VARCHAR(400), source_url VARCHAR(500), title TEXT, author VARCHAR(190),
            summary TEXT, image_url VARCHAR(500), published_at TEXT, content_hash VARCHAR(64),
            status VARCHAR(28) DEFAULT 'retrieved', pci_post_id INTEGER, review_note TEXT, reviewed_by INTEGER,
            created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_extitem_source ON cc_external_items(source_id, status)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_cc_extitem_guid ON cc_external_items(source_id, guid)");

        // AI Content Studio — provider/model/use-case configs (secrets stay in env, referenced by key_env) and
        // a full audit ledger of every generation (assist-only; AI never publishes autonomously).
        db.Exec(@"CREATE TABLE IF NOT EXISTS ai_content_providers(id INTEGER PRIMARY KEY AUTOINCREMENT,
            provider VARCHAR(24) NOT NULL, label VARCHAR(120), model VARCHAR(80), use_case VARCHAR(40),
            system_prompt TEXT, prompt_template TEXT, max_tokens INTEGER DEFAULT 1200, temperature REAL DEFAULT 0.7,
            key_env VARCHAR(64), daily_quota INTEGER, require_citations INTEGER DEFAULT 0, require_review INTEGER DEFAULT 1,
            active INTEGER DEFAULT 1, sort_order INTEGER DEFAULT 0, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_ai_providers_use ON ai_content_providers(use_case, active)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS ai_content_generations(id INTEGER PRIMARY KEY AUTOINCREMENT,
            provider VARCHAR(24), model VARCHAR(80), use_case VARCHAR(40), post_id INTEGER,
            prompt TEXT, output TEXT, sources TEXT, tokens_in INTEGER, tokens_out INTEGER,
            status VARCHAR(16) DEFAULT 'generated', human_approved INTEGER DEFAULT 0,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_ai_gen_post ON ai_content_generations(post_id)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_ai_gen_creator ON ai_content_generations(created_by)");

        // Backlink & Outreach CRM (Phase 5) — earn and monitor inbound links the honest way. PCI does NOT
        // scrape the web, buy links, or run automated discovery: prospects, outreach touchpoints and earned
        // backlinks are entered manually or by CSV, or captured from PCI's own referral logs. The one
        // automated action is on-demand VERIFICATION — fetching a SINGLE recorded source page through the
        // SSRF-guarded egress client to confirm the link to PCI still exists. cc_* namespaced; the whole
        // subsystem is gated by the cc_backlinks permission.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_link_prospects(id INTEGER PRIMARY KEY AUTOINCREMENT,
            name VARCHAR(160) NOT NULL, domain VARCHAR(190), url VARCHAR(500), category VARCHAR(40) DEFAULT 'publication',
            authority INTEGER, relevance VARCHAR(12) DEFAULT 'medium', status VARCHAR(20) DEFAULT 'prospect',
            owner VARCHAR(120), contact_name VARCHAR(120), contact_email VARCHAR(190),
            notes TEXT, next_action_at TEXT, active INTEGER DEFAULT 1,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_prospect_status ON cc_link_prospects(status, active)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_outreach(id INTEGER PRIMARY KEY AUTOINCREMENT,
            prospect_id INTEGER NOT NULL, channel VARCHAR(20) DEFAULT 'email', direction VARCHAR(8) DEFAULT 'out',
            subject VARCHAR(200), body TEXT, outcome VARCHAR(20) DEFAULT 'sent',
            occurred_at TEXT DEFAULT (datetime('now')), follow_up_at TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_outreach_prospect ON cc_outreach(prospect_id)");

        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_backlinks(id INTEGER PRIMARY KEY AUTOINCREMENT,
            prospect_id INTEGER, link_hash VARCHAR(64), source_url VARCHAR(500) NOT NULL, source_domain VARCHAR(190),
            target_url VARCHAR(500), anchor_text VARCHAR(300), rel VARCHAR(16) DEFAULT 'unknown',
            link_type VARCHAR(20) DEFAULT 'editorial', status VARCHAR(16) DEFAULT 'candidate',
            discovered_via VARCHAR(24) DEFAULT 'manual', first_seen_at TEXT, last_checked_at TEXT, last_status_code INTEGER,
            notes TEXT, active INTEGER DEFAULT 1,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_backlink_status ON cc_backlinks(status, active)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_cc_backlink_hash ON cc_backlinks(link_hash)");

        // Read-only analytics connectors (Phase 6) — pull search/traffic metrics from providers whose official
        // APIs are read-only: Google Search Console, Bing Webmaster Tools, Google Analytics 4. This surface
        // only READS; nothing is written back to any provider. Credentials (a bearer OAuth token for Google,
        // an API key for Bing) are stored encrypted (secret_enc), never returned to the browser, and every
        // call goes through the SSRF-guarded egress client. cc_* namespaced; gated by the cc_seo permission.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_analytics_sources(id INTEGER PRIMARY KEY AUTOINCREMENT,
            provider VARCHAR(16) NOT NULL, label VARCHAR(160), property VARCHAR(300), api_base VARCHAR(300),
            auth_kind VARCHAR(16) DEFAULT 'bearer', secret_enc TEXT, range_days INTEGER DEFAULT 28,
            status VARCHAR(16) DEFAULT 'not_connected', active INTEGER DEFAULT 1, last_synced_at TEXT, last_error TEXT,
            created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_ansrc_provider ON cc_analytics_sources(provider, active)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_analytics_metrics(id INTEGER PRIMARY KEY AUTOINCREMENT,
            source_id INTEGER NOT NULL, dimension VARCHAR(16) NOT NULL, dim_value VARCHAR(400), metric_date VARCHAR(12),
            clicks INTEGER, impressions INTEGER, ctr REAL, position REAL, sessions INTEGER, users INTEGER, pageviews INTEGER,
            fetched_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_anmetric_source ON cc_analytics_metrics(source_id, dimension)");

        // Content link registry (Phase C) — the hyperlinks a post publishes, extracted from its body. Each row
        // is classified internal (relative or same-site) vs external, carries an editor-set rel policy
        // (auto/dofollow/nofollow/sponsored/ugc), an optional citation + approval flag, and an on-demand HTTP
        // status (checked ONLY for external links, through the SSRF-guarded egress client — never for private
        // targets). url_norm is VARCHAR so it can back the per-post unique index (MySQL can't index TEXT).
        // cc_* namespaced; gated by the cc_links permission.
        db.Exec(@"CREATE TABLE IF NOT EXISTS cc_content_links(id INTEGER PRIMARY KEY AUTOINCREMENT,
            post_id INTEGER NOT NULL, url VARCHAR(500) NOT NULL, url_norm VARCHAR(500),
            kind VARCHAR(12) DEFAULT 'external', anchor_text VARCHAR(300), rel VARCHAR(24) DEFAULT 'auto',
            is_citation INTEGER DEFAULT 0, approved INTEGER DEFAULT 0,
            status VARCHAR(16) DEFAULT 'unchecked', http_code INTEGER, last_checked_at TEXT, clicks INTEGER DEFAULT 0,
            active INTEGER DEFAULT 1, created_by INTEGER, created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_cc_links_post ON cc_content_links(post_id, kind)");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_cc_links_post_url ON cc_content_links(post_id, url_norm)");
        AddCol("cc_content_links", "clicks", "clicks INTEGER DEFAULT 0");   // Phase D: outbound-click counter (idempotent for DBs created before this)

        // Configurable defaults (operator-tunable via Settings; no hardcoded values in React/.NET) + the
        // content-centre notification master toggle.
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('backlink_our_domain','projectcontrolsinstitute.org')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('backlink_verify_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('analytics_default_range_days','28')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('blog_base_path','/blog')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('news_base_path','/news')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('blog_posts_per_page','12')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('blog_indexnow_on_publish','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_content_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('ai_content_enabled','1')");
        try { PCI.Backend.Core.CapabilityRegistry.Seed(db); } catch { /* site_settings/content_capabilities present on later pass */ }

        MultiCert.Seed(db);

        // Public announcement modal defaults (idempotent: only fills absent keys, never overwrites
        // an admin edit or an explicit hide). Editable end-to-end from the admin console.
        try { PCI.Backend.Endpoints.Announcement.Seed(db); } catch { /* site_settings present on first pass */ }

        // bootstrap owner admin on first run (parity with db.js seed)
        try
        {
            var n = db.Scalar<long>("SELECT COUNT(*) FROM admin_users");
            if (n == 0)
            {
                var email = (Environment.GetEnvironmentVariable("ADMIN_OWNER_EMAIL") ?? "owner@pci.local").ToLowerInvariant();
                var pw = Environment.GetEnvironmentVariable("ADMIN_OWNER_PASSWORD") ?? "changeme-owner";
                db.Execute("INSERT INTO admin_users(email,name,password_hash,role,status,must_change_pw) VALUES(?,?,?,?, 'active',1)",
                    email, "Owner", BCrypt.Net.BCrypt.HashPassword(pw), "owner");
                Console.WriteLine($"[seed] bootstrap owner admin created: {email}");
            }
        }
        catch { /* admin_users may not exist on a very first pass; ignored */ }

        // Explicit browser-suite operator. The real bootstrap owner deliberately keeps the seeded
        // password + advisory must_change_pw flag, which is itself an E2E journey. Broader
        // admin/test-user journeys need a separate, deterministic operator that can call the same
        // owner-gated APIs a human uses.
        // This account is created ONLY outside Production and ONLY when the Playwright server opts in
        // with E2E_ADMIN_PASSWORD; a production deployment can never activate it accidentally.
        try
        {
            // Treat an unset environment as Production (the safe default) and honour either .NET
            // environment variable. The opt-in secret alone must never create this account on an
            // ambiguously configured deployment.
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                ?? "Production";
            var isProd = string.Equals(environment, "Production", StringComparison.OrdinalIgnoreCase);
            var e2ePw = Environment.GetEnvironmentVariable("E2E_ADMIN_PASSWORD");
            if (!isProd && !string.IsNullOrWhiteSpace(e2ePw))
            {
                var email = (Environment.GetEnvironmentVariable("E2E_ADMIN_EMAIL") ?? "browser-admin@pci.test").Trim().ToLowerInvariant();
                var hash = BCrypt.Net.BCrypt.HashPassword(e2ePw);
                var existing = db.QueryOne("SELECT id FROM admin_users WHERE lower(email)=?", email);
                long id;
                if (existing is null)
                    id = db.ExecuteReturningId("INSERT INTO admin_users(email,name,password_hash,role,status,must_change_pw) VALUES(?,?,?,?, 'active',0)",
                        email, "Browser Test Admin", hash, "owner");
                else
                {
                    id = Convert.ToInt64(existing["id"]);
                    db.Execute("UPDATE admin_users SET name='Browser Test Admin',password_hash=?,role='owner',status='active',must_change_pw=0 WHERE id=?", hash, id);
                }
                db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", id);
                Console.WriteLine($"[seed] opt-in E2E admin ready: {email}");
            }
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] E2E admin skipped: {e.Message}"); }

        // Break-glass owner recovery: if ADMIN_OWNER_RESET_PASSWORD is set at boot, (re)activate the
        // owner account and set its password to that value, flagging it for a follow-up change. This lets
        // an operator who is locked out regain access by setting one Render environment variable and
        // redeploying — no database console needed. It targets ADMIN_OWNER_EMAIL if that owner exists,
        // otherwise the earliest owner; if no owner exists at all it creates one. REMOVE the env var
        // after logging in — while it is set, every boot re-applies it. Setting the env var already
        // implies full control of the deployment, so this is not a new trust boundary.
        try
        {
            var resetPw = Environment.GetEnvironmentVariable("ADMIN_OWNER_RESET_PASSWORD");
            if (!string.IsNullOrWhiteSpace(resetPw))
            {
                var email = (Environment.GetEnvironmentVariable("ADMIN_OWNER_EMAIL") ?? "").Trim().ToLowerInvariant();
                var hash = BCrypt.Net.BCrypt.HashPassword(resetPw);
                var target = (email.Length > 0 ? db.QueryOne("SELECT id FROM admin_users WHERE lower(email)=? AND role='owner'", email) : null)
                             ?? db.QueryOne("SELECT id FROM admin_users WHERE role='owner' ORDER BY id LIMIT 1");
                if (target is not null)
                {
                    db.Execute("UPDATE admin_users SET password_hash=?, status='active', must_change_pw=1 WHERE id=?", hash, target["id"]);
                    db.Execute("DELETE FROM admin_sessions WHERE admin_id=?", target["id"]);   // invalidate any stale sessions
                    Console.WriteLine("[seed] ADMIN_OWNER_RESET_PASSWORD applied — owner password reset (change it in Settings → Security). REMOVE this env var now.");
                }
                else
                {
                    var newEmail = email.Length > 0 ? email : "owner@pci.local";
                    db.Execute("INSERT INTO admin_users(email,name,password_hash,role,status,must_change_pw) VALUES(?,?,?,?, 'active',1)",
                        newEmail, "Owner", hash, "owner");
                    Console.WriteLine($"[seed] ADMIN_OWNER_RESET_PASSWORD created a new owner ({newEmail}). REMOVE this env var now.");
                }
            }
        }
        catch (Exception e) { Console.Error.WriteLine($"[seed] owner reset skipped: {e.Message}"); }

        // EXT-P0-05 — atomic worker leases: multi-instance dispatchers claim due work with a conditional
        // UPDATE (lease_owner + lease_until). Expired leases are reclaimed so a crashed worker cannot
        // permanently strand a row, and two workers cannot both deliver the same outbound action.
        void AddLeaseCols(string table)
        {
            AddCol(table, "lease_owner", "lease_owner VARCHAR(64)");
            AddCol(table, "lease_until", "lease_until TEXT");
        }
        AddLeaseCols("comm_outbox");
        AddLeaseCols("integration_deliveries");
        AddLeaseCols("mkt_jobs");
        AddLeaseCols("content_jobs");
        AddLeaseCols("certuvo_accounts");
        AddLeaseCols("exam_delivery_orders");

        // EXT-P0-03 — surface external-delivery pending/blocked state on the PCI booking itself.
        AddCol("exam_bookings", "delivery_status", "delivery_status VARCHAR(32)");

        // EXT-P0-02 — partial refunds update financial state without revoking access.
        AddCol("payments", "amount_refunded", "amount_refunded REAL DEFAULT 0");
        AddCol("payments", "refunded_at", "refunded_at TEXT");

        // Backfill Exam Authorizations for any pre-existing settled exam seat (idempotent; best-effort).
        PCI.Backend.Core.ExamAuthorization.BackfillAll(db);

        // Demo student on first run (users table empty): lets the operator try the student panel
        // before payments/SMTP are configured. Mirrors the bootstrap-owner pattern: known default
        // password, loudly logged, and expected to be changed or deactivated before launch. The
        // account carries NO membership, entitlement or credential — logging in shows an empty
        // dashboard; payment remains the only path to anything that matters.
        try
        {
            // In Production, never auto-create a login-able account with a public default password:
            // that seeds a real, authenticatable student on the live site. Seed the demo student only
            // outside Production, OR when the operator explicitly sets DEMO_STUDENT_PASSWORD (an opt-in
            // that proves intent). Everywhere else, launch starts with zero student accounts.
            var isProd = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Production", StringComparison.OrdinalIgnoreCase);
            var demoOptIn = Environment.GetEnvironmentVariable("DEMO_STUDENT_PASSWORD") is not null;
            var n = db.Scalar<long>("SELECT COUNT(*) FROM users");
            if (n == 0 && (!isProd || demoOptIn))
            {
                var email = (Environment.GetEnvironmentVariable("DEMO_STUDENT_EMAIL") ?? "student@pci.local").ToLowerInvariant();
                var pw = Environment.GetEnvironmentVariable("DEMO_STUDENT_PASSWORD") ?? "changeme-student";
                var uid = db.ExecuteReturningId("INSERT INTO users(email,first_name,last_name,role,status,password_hash) VALUES(?,?,?, 'student','active',?)",
                    email, "Demo", "Student", BCrypt.Net.BCrypt.HashPassword(pw));
                db.Execute("INSERT INTO student_profiles(user_id) VALUES(?)", uid);
                Console.WriteLine($"[seed] demo student created: {email} — change the password or deactivate this account before launch");
            }
        }
        catch { /* users may not exist on a very first pass; ignored */ }
    }
}
