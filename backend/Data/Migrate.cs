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
            var have = db.Columns(table);
            if (have.Count > 0 && !have.Contains(col)) db.Exec($"ALTER TABLE {table} ADD COLUMN {ddl}");
        }
        AddCol("sample_questions", "is_practice", "is_practice INTEGER DEFAULT 0");
        AddCol("exam_attempts", "result_status", "result_status TEXT DEFAULT 'not_started'");
        AddCol("exam_attempts", "hold_reason", "hold_reason TEXT");
        AddCol("exam_attempts", "released_at", "released_at TEXT");
        AddCol("exam_attempts", "answer_key_version", "answer_key_version TEXT");
        AddCol("exam_attempts", "bank_version", "bank_version TEXT");
        AddCol("issued_credentials", "attempt_id", "attempt_id INTEGER");
        db.Exec("CREATE UNIQUE INDEX IF NOT EXISTS ux_credential_attempt ON issued_credentials(attempt_id) WHERE attempt_id IS NOT NULL");
        // Phase 2: casework tables + CPD evidence columns for pre-existing databases
        db.Exec(@"CREATE TABLE IF NOT EXISTS appeals(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,attempt_id INTEGER,credential_id TEXT,type TEXT NOT NULL,reason TEXT NOT NULL,evidence_name TEXT,evidence_data TEXT,status TEXT DEFAULT 'submitted',submitted_at TEXT DEFAULT (datetime('now')),decision TEXT,decided_by INTEGER,decided_at TEXT)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_appeals_user ON appeals(user_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS accommodation_requests(id INTEGER PRIMARY KEY AUTOINCREMENT,user_id INTEGER NOT NULL,request_type TEXT NOT NULL,description TEXT NOT NULL,evidence_name TEXT,evidence_data TEXT,status TEXT DEFAULT 'submitted',approved_extra_minutes INTEGER DEFAULT 0,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),decided_at TEXT,decided_by INTEGER)");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_accom_user ON accommodation_requests(user_id)");
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
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honorary_user ON honorary_awards(user_id)");
        // Honorary-route public applications (apply → board review → conferral) + reusable notification ledger.
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_applications(id INTEGER PRIMARY KEY AUTOINCREMENT,reference TEXT UNIQUE NOT NULL,first_name TEXT,last_name TEXT,email TEXT,mobile TEXT,country TEXT,city TEXT,nationality TEXT,job_title TEXT,employer TEXT,years_experience INTEGER,industry TEXT,highest_qualification TEXT,professional_certifications TEXT,relevant_experience TEXT,professional_summary TEXT,declaration INTEGER DEFAULT 0,status TEXT NOT NULL DEFAULT 'pending_review',award_no TEXT,decided_by INTEGER,decided_at TEXT,admin_note TEXT,created_at TEXT DEFAULT (datetime('now')),updated_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honapp_status ON honorary_applications(status)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS honorary_application_documents(id INTEGER PRIMARY KEY AUTOINCREMENT,application_id INTEGER NOT NULL,doc_kind TEXT DEFAULT 'supporting',filename TEXT,mime TEXT,size_bytes INTEGER,storage_ref TEXT,sha256 TEXT,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("CREATE INDEX IF NOT EXISTS ix_honappdoc_app ON honorary_application_documents(application_id)");
        db.Exec(@"CREATE TABLE IF NOT EXISTS notification_history(id INTEGER PRIMARY KEY AUTOINCREMENT,channel TEXT NOT NULL DEFAULT 'email',recipient TEXT,subject TEXT,status TEXT,related_type TEXT,related_id INTEGER,created_at TEXT DEFAULT (datetime('now')))");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_enabled','1')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_admin_email','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_ack_subject','')");
        db.Exec("INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_admin_subject','')");
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
        AddCol("users", "two_factor_enabled", "two_factor_enabled INTEGER DEFAULT 0");
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
