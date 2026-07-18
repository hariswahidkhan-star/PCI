-- PCI student enrollment schema (USD pricing, sessions/resume, discount codes)
CREATE TABLE IF NOT EXISTS users (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  email TEXT UNIQUE NOT NULL,
  first_name TEXT, last_name TEXT,
  registration_no TEXT,
  password_hash TEXT,                       -- set by user via secure link; never an emailed plain password
  role TEXT NOT NULL DEFAULT 'student',
  status TEXT NOT NULL DEFAULT 'pending',   -- pending | active | deactivated
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS enrollment_sessions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  email TEXT NOT NULL, user_id INTEGER REFERENCES users(id),
  current_step INTEGER DEFAULT 1,
  session_status TEXT DEFAULT 'in_progress',-- in_progress | paid | abandoned
  resume_token_hash TEXT, resume_token_expiry TEXT,
  selected_product TEXT,                     -- membership | exam | bundle
  selected_membership TEXT,
  pricing_snapshot TEXT,                     -- JSON of computed prices at selection time
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now')),
  last_activity_at TEXT DEFAULT (datetime('now')),
  reminders_sent INTEGER DEFAULT 0, last_reminder_at TEXT   -- abandoned-enrolment reminder sequence (max 3)
);
CREATE TABLE IF NOT EXISTS student_profiles (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL REFERENCES users(id),
  mobile TEXT, country TEXT, city TEXT, preferred_language TEXT,
  current_role TEXT, company TEXT, industry_sector TEXT, years_experience TEXT,
  highest_qualification TEXT, project_controls_area TEXT, enrollment_purpose TEXT,
  profile_completion_percentage INTEGER DEFAULT 20,
  linkedin_url TEXT, profile_photo TEXT
);
CREATE TABLE IF NOT EXISTS pricing_rules (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  currency TEXT DEFAULT 'USD', product_type TEXT,           -- membership | exam | renewal | retake
  standard_price REAL, default_discount_percentage REAL DEFAULT 0,
  active INTEGER DEFAULT 1, start_date TEXT, end_date TEXT,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS discount_codes (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  code TEXT UNIQUE NOT NULL,
  discount_type TEXT,                        -- percentage | fixed
  discount_value REAL,
  applies_to TEXT DEFAULT 'all',             -- membership | exam | all
  start_date TEXT, end_date TEXT,
  max_uses INTEGER, used_count INTEGER DEFAULT 0,
  single_use_per_email INTEGER DEFAULT 0, active INTEGER DEFAULT 1,
  code_type TEXT DEFAULT 'general',          -- general | institution | student | referral | campaign
  org_name TEXT,                             -- institution / employer the code was issued to
  owner_user_id INTEGER,                     -- referral: the member who owns this code
  batch_id TEXT,                             -- set when generated in bulk
  per_user_limit INTEGER,                    -- max redemptions per email (NULL = unlimited)
  notes TEXT,
  -- founding-stage access (100% fee waiver layered over the paid flow; start/end_date is the window).
  -- ONE founding route: a valid code grants membership + study + exam together (three-route model);
  -- legacy founding_member/founding_candidate values are migrated forward to 'founding'.
  founding_route TEXT,                       -- NULL | founding
  grants_membership INTEGER DEFAULT 1,
  grants_exam INTEGER DEFAULT 1,
  grants_study_access INTEGER DEFAULT 1,
  requires_application INTEGER DEFAULT 0,    -- board-optional gating (application + evidence)
  auto_approve INTEGER DEFAULT 1,
  membership_months INTEGER DEFAULT 12,      -- term of the granted membership
  criteria_json TEXT                         -- board-set thresholds, e.g. {"min_experience_years":3,...}
);
CREATE TABLE IF NOT EXISTS payments (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER REFERENCES users(id), enrollment_session_id INTEGER REFERENCES enrollment_sessions(id),
  product_type TEXT, standard_amount REAL, default_discount_amount REAL,
  discount_code TEXT, discount_code_amount REAL, final_amount REAL, currency TEXT DEFAULT 'USD',
  payment_provider TEXT, provider_payment_id TEXT,
  payment_status TEXT,                       -- paid | failed | refunded
  payment_date TEXT, invoice_url TEXT, receipt_url TEXT,
  reference TEXT, exam_schedule_deadline TEXT,             -- exam: schedule-by date (payment + 12 months)
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS memberships (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL REFERENCES users(id),
  membership_type TEXT, status TEXT DEFAULT 'inactive',   -- inactive | active | expired
  start_date TEXT, expiry_date TEXT,
  renewal_fee REAL DEFAULT 99, renewal_cycle TEXT DEFAULT '3 years',
  amount_paid REAL, currency TEXT DEFAULT 'USD'
);
CREATE TABLE IF NOT EXISTS login_tokens (
  id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER NOT NULL REFERENCES users(id),
  token TEXT UNIQUE NOT NULL, purpose TEXT DEFAULT 'set_password',
  expires_at TEXT, used_at TEXT, created_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS email_logs (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER, email TEXT, email_type TEXT, subject TEXT, status TEXT,
  sent_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS audit_logs (
  id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER, action TEXT, details TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);


-- Reviews / testimonials submitted by students, members or professionals.
CREATE TABLE IF NOT EXISTS reviews (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER REFERENCES users(id),
  name TEXT NOT NULL,
  designation TEXT,
  company TEXT,
  country TEXT,
  relationship TEXT,              -- student | member | professional | other
  rating INTEGER,                 -- 1..5
  title TEXT,
  body TEXT NOT NULL,
  status TEXT DEFAULT 'pending',  -- pending | published | rejected
  featured INTEGER DEFAULT 0,     -- show on the home page
  sort_order INTEGER DEFAULT 0,
  created_at TEXT DEFAULT (datetime('now')),
  published_at TEXT,
  reviewed_by INTEGER
);


-- ── Production lifecycle & integrity tables ──

-- Immutable score snapshot: written once when an attempt is scored; never mutated by question edits.
CREATE TABLE IF NOT EXISTS exam_score_snapshots (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  attempt_id INTEGER NOT NULL REFERENCES exam_attempts(id),
  user_id INTEGER NOT NULL,
  score INTEGER, max_score INTEGER, percent REAL, result TEXT,
  domain_breakdown TEXT, unanswered INTEGER, flagged_events INTEGER,
  duration_seconds INTEGER, submitted_at TEXT, answer_key_version TEXT, bank_version TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_score_snapshot_attempt ON exam_score_snapshots(attempt_id);

-- Formal paid-exam entitlement (one per exam/bundle payment).
CREATE TABLE IF NOT EXISTS exam_entitlements (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  payment_id INTEGER REFERENCES payments(id),
  product_type TEXT,
  certification_id INTEGER DEFAULT 1 REFERENCES certifications(id),
  status TEXT DEFAULT 'available',   -- available | booked | consumed | expired | refunded
  valid_until TEXT,
  booking_id INTEGER,
  attempt_id INTEGER,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_entitlement_payment ON exam_entitlements(payment_id);

-- Candidate consents / policy acceptances (versioned, auditable).
CREATE TABLE IF NOT EXISTS candidate_consents (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  consent_type TEXT NOT NULL,        -- candidate_handbook | exam_rules | proctoring_consent | privacy_notice | refund_policy | misconduct_policy | credential_terms
  policy_version TEXT NOT NULL,
  accepted_at TEXT DEFAULT (datetime('now')),
  ip_address TEXT, user_agent TEXT, metadata_json TEXT
);
CREATE INDEX IF NOT EXISTS ix_consents_user ON candidate_consents(user_id, consent_type);

-- Stripe webhook idempotency ledger (records processing before side effects).
CREATE TABLE IF NOT EXISTS webhook_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  provider TEXT DEFAULT 'stripe',
  event_id TEXT,
  processed_at TEXT DEFAULT (datetime('now')),
  status TEXT, error TEXT
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_webhook_event ON webhook_events(event_id);

-- Security / auth events (rate-limit hits, token use, admin security actions).
CREATE TABLE IF NOT EXISTS security_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER, kind TEXT, detail TEXT, ip TEXT, user_agent TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);


-- ── Phase 2: appeals, accommodations, support attachments, CPD evidence ──

-- Formal appeals & complaints (separate from generic tickets — Section B10).
CREATE TABLE IF NOT EXISTS appeals (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  attempt_id INTEGER,                -- nullable: result/invalidation appeals reference an attempt
  credential_id TEXT,                -- nullable: credential-related appeals
  type TEXT NOT NULL,                -- result_appeal | invalidation_appeal | complaint | ethics
  reason TEXT NOT NULL,
  evidence_name TEXT, evidence_data TEXT,   -- optional small supporting file (data URI, size-capped)
  status TEXT DEFAULT 'submitted',   -- submitted | under_review | upheld | dismissed | withdrawn
  submitted_at TEXT DEFAULT (datetime('now')),
  decision TEXT, decided_by INTEGER, decided_at TEXT
);
CREATE INDEX IF NOT EXISTS ix_appeals_user ON appeals(user_id);

-- Accommodation requests (Section B8). Approved extra minutes genuinely extend exam duration.
CREATE TABLE IF NOT EXISTS accommodation_requests (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  request_type TEXT NOT NULL,        -- extra_time | separate_setting | assistive_technology | other
  description TEXT NOT NULL,
  evidence_name TEXT, evidence_data TEXT,
  status TEXT DEFAULT 'submitted',   -- submitted | under_review | approved | rejected
  approved_extra_minutes INTEGER DEFAULT 0,
  admin_note TEXT,
  created_at TEXT DEFAULT (datetime('now')),
  decided_at TEXT, decided_by INTEGER
);
CREATE INDEX IF NOT EXISTS ix_accom_user ON accommodation_requests(user_id);

-- Support ticket attachments (Section B9): small files stored as size-capped data URIs,
-- consistent with the exam_evidence storage pattern.
CREATE TABLE IF NOT EXISTS support_attachments (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  ticket_id INTEGER NOT NULL REFERENCES tickets(id),
  user_id INTEGER,                   -- uploader (null = admin)
  filename TEXT NOT NULL, mime TEXT, size_bytes INTEGER, sha256 TEXT,
  data_uri TEXT, storage_ref TEXT NOT NULL,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_attach_ticket ON support_attachments(ticket_id);


-- ── Phase 3: pre-exam readiness/system checks (Section C9) ──
CREATE TABLE IF NOT EXISTS exam_readiness_checks (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  booking_id INTEGER,
  camera INTEGER DEFAULT 0, microphone INTEGER DEFAULT 0, network INTEGER DEFAULT 0,
  fullscreen INTEGER DEFAULT 0, environment INTEGER DEFAULT 0,
  browser TEXT, screen TEXT, passed INTEGER DEFAULT 0,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_readiness_user ON exam_readiness_checks(user_id);

-- Seed pricing rules (USD). Admin can edit these. Idempotent: only inserts a product_type once,
-- so re-running the schema on every boot never creates duplicate pricing rows.
INSERT INTO pricing_rules (currency,product_type,standard_price,default_discount_percentage,active)
  SELECT 'USD','membership',99,50,1 WHERE NOT EXISTS(SELECT 1 FROM pricing_rules WHERE product_type='membership');
INSERT INTO pricing_rules (currency,product_type,standard_price,default_discount_percentage,active)
  SELECT 'USD','exam',500,30,1 WHERE NOT EXISTS(SELECT 1 FROM pricing_rules WHERE product_type='exam');
INSERT INTO pricing_rules (currency,product_type,standard_price,default_discount_percentage,active)
  SELECT 'USD','retake',0,0,0 WHERE NOT EXISTS(SELECT 1 FROM pricing_rules WHERE product_type='retake');

-- Seed example discount codes (admin-manageable). EXPIRED2024 is inactive/expired on purpose.
INSERT OR IGNORE INTO discount_codes (code,discount_type,discount_value,applies_to,max_uses,single_use_per_email,active,end_date) VALUES
 ('SAVE10','percentage',10,'all',NULL,0,1,NULL),
 ('MEMBER20','percentage',20,'membership',NULL,1,1,NULL),
 ('EXAM15','percentage',15,'exam',NULL,0,1,NULL),
 ('WELCOME25','fixed',25,'all',500,1,1,NULL),
 ('EXPIRED2024','percentage',15,'all',NULL,0,0,'2024-12-31'),
 -- Organisation / bulk-enrolment codes (issued to institutes & employers; larger cohorts get higher tiers)
 ('ORG15','percentage',15,'all',NULL,0,1,NULL),
 ('INSTITUTE20','percentage',20,'all',NULL,0,1,NULL),
 ('EMPLOYER20','percentage',20,'all',NULL,0,1,NULL),
 ('BULK25','percentage',25,'all',NULL,0,1,NULL),
 ('BULK30','percentage',30,'all',NULL,0,1,NULL);

-- Inquiries (contact / info / corporate / partnership) with auto-response tracking
CREATE TABLE IF NOT EXISTS inquiries (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  type TEXT DEFAULT 'general',               -- general | info | corporate | partnership
  email TEXT NOT NULL, first_name TEXT,
  topic TEXT, seats TEXT, org TEXT, message TEXT,
  reference TEXT, status TEXT DEFAULT 'new', -- new | in_progress | closed
  created_at TEXT DEFAULT (datetime('now'))
);

-- Issued credentials registry (powers public verification at /api/verify and verify.html)
CREATE TABLE IF NOT EXISTS issued_credentials (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  credential_id TEXT UNIQUE NOT NULL,            -- e.g. PCP-AI-2026-04821
  user_id INTEGER REFERENCES users(id),
  attempt_id INTEGER REFERENCES exam_attempts(id),
  holder_name TEXT, credential TEXT DEFAULT 'PCP-AI',
  certification_id INTEGER DEFAULT 1 REFERENCES certifications(id),
  status TEXT DEFAULT 'active',                  -- active | expired | revoked
  issued_at TEXT DEFAULT (datetime('now')), expires_at TEXT
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_credential_attempt ON issued_credentials(attempt_id) WHERE attempt_id IS NOT NULL;

-- ============================================================
--  WEBSITE / CMS TABLES  (managed from the admin dashboard)
-- ============================================================
CREATE TABLE IF NOT EXISTS pages (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  slug TEXT UNIQUE NOT NULL, title TEXT, meta_description TEXT,
  nav_group TEXT, noindex INTEGER DEFAULT 0, published INTEGER DEFAULT 1,
  updated_at TEXT DEFAULT (datetime('now'))
);
-- Per-page editable content regions (fully dynamic content). block_key mirrors the page's
-- data-cms="<slug>:<key>" attribute; the value is injected into the served HTML server-side
-- (SEO-safe, works with JS off) and also exposed via /api/page-content for the client loader.
CREATE TABLE IF NOT EXISTS page_blocks (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  slug TEXT NOT NULL, block_key TEXT NOT NULL,
  label TEXT, ctype TEXT DEFAULT 'text', cvalue TEXT,
  sort_order INTEGER DEFAULT 0,
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_page_block ON page_blocks(slug, block_key);
CREATE INDEX IF NOT EXISTS ix_page_block_slug ON page_blocks(slug);
CREATE TABLE IF NOT EXISTS site_content (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  ckey TEXT UNIQUE NOT NULL, cgroup TEXT, label TEXT, ctype TEXT DEFAULT 'text', cvalue TEXT,
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS faqs (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  question TEXT NOT NULL UNIQUE, answer TEXT, category TEXT DEFAULT 'General',
  sort_order INTEGER DEFAULT 0, published INTEGER DEFAULT 1
);
CREATE TABLE IF NOT EXISTS bok_domains (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  code TEXT UNIQUE, name TEXT NOT NULL, weight INTEGER DEFAULT 0, description TEXT, bullets TEXT, sort_order INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS sample_questions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  question TEXT NOT NULL UNIQUE, options TEXT, option_a TEXT, option_b TEXT, option_c TEXT, option_d TEXT,
  answer_index INTEGER, domain TEXT,
  published INTEGER DEFAULT 1, sort_order INTEGER,
  is_practice INTEGER DEFAULT 0,
  explanation TEXT, difficulty TEXT,          -- Certuvo (Phase 8): teaching explanation + difficulty band (practice rows)
  certification_id INTEGER DEFAULT 1 REFERENCES certifications(id)
);
-- Certuvo practice attempts (Phase 8): formative quiz / mock sittings, separate from exam_attempts.
CREATE TABLE IF NOT EXISTS practice_attempts (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  mode TEXT NOT NULL DEFAULT 'quiz',          -- quiz | mock
  domain TEXT,                                -- null = mixed
  question_ids TEXT, answers TEXT,            -- JSON: served id order, and {qid:index}
  score INTEGER, total INTEGER, domain_breakdown TEXT,
  status TEXT NOT NULL DEFAULT 'in_progress', -- in_progress | completed
  duration_seconds INTEGER,
  started_at TEXT DEFAULT (datetime('now')), completed_at TEXT
);
CREATE INDEX IF NOT EXISTS ix_practice_user ON practice_attempts(user_id);
-- ERP / integrations foundation (Phase 9): durable event outbox + connector registry + delivery ledger.
CREATE TABLE IF NOT EXISTS integrations (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  provider TEXT NOT NULL DEFAULT 'webhook',    -- webhook (generic) | future ERP connectors
  name TEXT, enabled INTEGER DEFAULT 0,
  endpoint_url TEXT, secret TEXT,              -- secret is write-only via the API (never returned)
  event_filter TEXT,                           -- JSON array of event types; empty/null = all
  config TEXT, status TEXT DEFAULT 'idle', last_delivery_at TEXT,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS integration_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  event_type TEXT NOT NULL,                    -- payment.recorded | membership.activated | member.registered | ping
  entity_type TEXT, entity_id INTEGER,
  payload TEXT, created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_intevent_created ON integration_events(created_at);
CREATE TABLE IF NOT EXISTS integration_deliveries (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  event_id INTEGER NOT NULL, integration_id INTEGER NOT NULL,
  status TEXT NOT NULL DEFAULT 'pending',      -- pending | delivered | failed
  attempts INTEGER DEFAULT 0, response_code INTEGER, last_error TEXT, next_attempt_at TEXT,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_intdel_pending ON integration_deliveries(status, next_attempt_at);
CREATE UNIQUE INDEX IF NOT EXISTS ux_intdel_event_int ON integration_deliveries(event_id, integration_id);

-- ============================================================
--  CERTIFICATIONS — the platform is multi-credential: every question bank,
--  entitlement, booking, attempt and issued credential belongs to exactly one
--  certification. PCP-AI is simply the first row. Per-certification columns
--  override the global exam_* settings; NULL means "use the global setting".
-- ============================================================
CREATE TABLE IF NOT EXISTS certifications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  code TEXT UNIQUE NOT NULL,                 -- machine code, e.g. 'PCP-AI' (also the credential prefix)
  name TEXT NOT NULL,                        -- display name, e.g. 'Certified Project Controls Professional — AI'
  description TEXT,
  credential_prefix TEXT,                    -- defaults to code when null
  pass_mark_pct REAL,                        -- NULL → global exam_pass_mark_pct
  duration_minutes INTEGER,                  -- NULL → global exam_duration_minutes
  expiry_years INTEGER DEFAULT 3,
  exam_price REAL,                           -- NULL → pricing_rules product_type='exam'
  active INTEGER DEFAULT 1,
  sort_order INTEGER DEFAULT 0,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
INSERT OR IGNORE INTO certifications(id,code,name,description,expiry_years,sort_order)
  VALUES(1,'PCP-AI','Certified Project Controls Professional — AI',
         'The integrated project-controls credential: planning, cost engineering, project finance and the governed use of AI.',3,1);
CREATE TABLE IF NOT EXISTS governance_roles (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  role TEXT NOT NULL UNIQUE, holder TEXT, status TEXT DEFAULT 'open', remit TEXT, sort_order INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS resources (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  title TEXT NOT NULL, category TEXT, doc_type TEXT DEFAULT 'PDF', url TEXT, description TEXT,
  published INTEGER DEFAULT 1, sort_order INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS news (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  title TEXT NOT NULL, body TEXT, url TEXT, published_date TEXT, published INTEGER DEFAULT 1, sort_order INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS nav_items (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  label TEXT NOT NULL, url TEXT, nav_group TEXT DEFAULT 'Footer', sort_order INTEGER DEFAULT 0, visible INTEGER DEFAULT 1
);
CREATE TABLE IF NOT EXISTS newsletter_subscribers (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  email TEXT UNIQUE NOT NULL, status TEXT DEFAULT 'subscribed', created_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS form_submissions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  form_type TEXT, name TEXT, email TEXT, subject TEXT, message TEXT, reference TEXT,
  status TEXT DEFAULT 'new', created_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS media_assets (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  filename TEXT UNIQUE, alt TEXT, width INTEGER, height INTEGER, usage TEXT
);
CREATE TABLE IF NOT EXISTS site_settings ( skey TEXT PRIMARY KEY, svalue TEXT );

-- seed editable site content (current live values)
INSERT OR IGNORE INTO site_content (ckey,cgroup,label,ctype,cvalue) VALUES
 ('home_hero_title','Home','Hero heading','text','The credential for the people who control projects .'),
 ('home_hero_subtitle','Home','Hero subtitle','textarea','PCP-AI is one rigorous standard uniting planning, scheduling, cost, earned value, forecasting and project finance — with responsible AI built into every part. Open to anyone with three years’ experience, in any field.'),
 ('home_hero_cta','Home','Hero button label','text','Explore certification'),
 ('announcement_enabled','Announcement','Show announcement banner','bool','0'),
 ('announcement_text','Announcement','Announcement text','text','Founding cohort enrolment is now open.'),
 ('footer_tagline','Footer','Footer tagline','textarea','Certifying the integrated discipline of project controls, cost engineering and project finance — with governed AI throughout. An independent professional body — a Delaware Non-Stock Corporation that intends to seek 501(c)(3) tax-exempt recognition (not yet granted).'),
 ('contact_email','Contact','Contact email','text','hello@projectcontrolsinstitute.org'),
 ('contact_phone','Contact','Contact phone','text',''),
 ('newsletter_heading','Home','Newsletter heading','text','Stay in the loop');

-- seed global settings
INSERT OR IGNORE INTO site_settings (skey,svalue) VALUES
 ('org_name','Project Controls Institute Global, Inc.'),
 ('contact_email','hello@projectcontrolsinstitute.org'),
 ('linkedin_url',''),('x_url',''),('youtube_url',''),
 ('analytics_id',''),
 ('brand_primary','#1D4ED8'),('brand_navy','#0E1525'),
 ('currency','USD'),
 ('maintenance_mode','0');

-- ===== FULL REAL-SITE SEED (generated from the live site files) =====
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('404.html','Page Not Found — Project Controls Institute','The page you requested could not be found. Explore the PCI certifications, Knowledge Centre, membership and more.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('about.html','About — Project Controls Institute','An independent independent certifying body for project controls, cost engineering and project finance, with governed AI throughout.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('accessibility-statement.html','Accessibility Statement — Project Controls Institute','Our commitment to making this website usable by everyone. Explore PCP-AI certification and membership with PCI.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('accommodation-form.html','Accommodation Request Form — Project Controls Institute','Request reasonable accommodations for your examination. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('accreditation-status.html','Accreditation Status — Project Controls Institute','PCI&#x27;s accreditation status, stated plainly: developing its framework with reference to ISO/IEC 17024 principles; honest at all times.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('admin-students.html','Enrolled Students — Administration | PCI','Administrative overview of PCI student enrolments, payments, abandoned enrolment sessions, pricing settings and discount codes, with CSV export.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ai-cert.html','AI in Project Controls — Specialist Certificate (AIPC) | PCI','A focused, standalone credential in the applied, governed use of AI for project controls — drawn from the flagship PCP-AI AI core.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ai-decision-policy.html','AI-Assisted Decision Making Policy | PCI','How AI may inform — but never replace — professional decisions in project controls.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ai-ethics-standard.html','AI Ethics Standard — Project Controls Institute','The ethical standard for the use of AI in project controls that certified professionals are held to.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ai-policy.html','AI Governance Policy — Project Controls Institute','PCI&#x27;s responsible-AI framework: human ownership, explainability, validation and privacy — aligned to ISO/IEC 42001 and the NIST AI RMF.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ai-standard.html','The AI Standard — Project Controls Institute','The PCP-AI assesses the applied, governed use of AI across forecasting, risk and reporting. AI proposes; the professional disposes.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('annual-reports.html','Annual Reports — Project Controls Institute','PCI&#x27;s commitment to transparency through regular public reporting on activities, governance, certification and stewardship.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('appeals-form.html','Appeals Form — Project Controls Institute','Appeal an eligibility, examination or certification decision. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('reviews.html','Reviews & Testimonials — Project Controls Institute','What students, members and professionals say about the PCI certification programmes and the Project Controls Institute.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('appeals.html','Appeals Process — Project Controls Institute','PCI&#x27;s independent appeals framework: what you can appeal, how to appeal, independent review and outcomes.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-ai-project-controls.html','AI in Project Controls: How Artificial Intelligence Is…','AI is reshaping how projects are forecast, reported and controlled. This guide explains where AI adds value across project controls — and why human.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-certification.html','Project Controls Certification: Which Certification Is…','How to navigate the project controls certification landscape and choose the path that fits your career — including where the PCI certifications fit and why AI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-cost-engineer.html','Cost Engineer Career Guide: Responsibilities, Salary and…','Cost engineers make project budgets credible and controllable. Here&#x27;s what they do — estimating, budgeting, forecasting and cost control — plus.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-evm.html','What Is Earned Value Management (EVM)? A Complete Guide','Earned value management integrates scope, schedule and cost into one honest measure of performance. This guide explains EV, CPI, SPI, forecasting and real.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-pc-vs-pm.html','Project Controls vs Project Management: What&#x27;s the…','Project controls and project management are often confused. This guide explains the real differences in responsibilities, skills, career paths and.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-planning-engineer.html','Planning Engineer Career Path: How to Become a Planning…','Planning engineers build and defend the schedule on major projects. Here&#x27;s the role, the tools (including Primavera P6), the skills, and the roadmap.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-project-controls-engineer.html','Project Controls Engineer Career Path: Salary, Skills…','What does a project controls engineer do, what skills and salary can you expect, and how do you progress? A complete career guide for one of capital.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-salary-guide.html','Project Controls Salary Guide 2026 | PCI','What do project controls professionals earn in 2026 — by role and region? An honest, structured guide to compensation, the drivers behind it, and how.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-skills.html','Top 10 Skills Every Project Controls Professional Needs','The capabilities that define a great project controls professional — from scheduling and cost control to analytics, AI governance and communication.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog-what-is-project-controls.html','What Is Project Controls? A Complete Guide for 2026 | PCI','Project controls is the discipline that keeps major projects on time and on budget. This complete guide explains what it is, its core components, and why.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('blog.html','Blog &amp; Insights — Project Controls Institute','PCI blog: in-depth guides on project controls, careers, salary, earned value management and AI in project controls.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('body-of-knowledge.html','PCP-AI Body of Knowledge (Draft v0.1) | PCI','The draft competency framework the PCI PCL-AI™ credential assesses — eight domains across project controls and the governed use of AI. Open for practitioner consultation.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('book.html','Book an Exam — Project Controls Institute','Sit a PCI examination or the AIPC online with remote proctoring or at a test centre. Eligibility, what to expect, and how to prepare with Certuvo.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('candidate-identification.html','Candidate Identification Policy — PCI','How candidate identity is verified to protect the value of the credential.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('candidate-journey.html','Candidate Journey | Project Controls Institute Global','The PCP-AI candidate journey from exploring the credential to application, preparation, examination, certification decision, digital badge and.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('career-guides.html','Career Guides — Project Controls Institute','PCI career guides: roles, pathways, skills and how certification advances a career in project controls.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('cert-policies.html','Certification Policies — Project Controls Institute','The rules governing how a PCI credential is earned, how the credential and marks are used, suspension and revocation, and maintenance.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-application.html','Certification Application — Project Controls Institute','Apply to sit your PCI examination. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-decision-policy.html','Certification Decision Policy — Project Controls Institute','How certification decisions are made — independently, on evidence, against the standard.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-integrity.html','Certification Integrity Statement | PCI','What PCI owns and controls — standards, body of knowledge, examination, ethics, appeals and certification decisions — kept independent of training and.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-lifecycle.html','Certification Lifecycle Policy — PCI','How a credential is granted, maintained, renewed, suspended, revoked and reinstated across its life.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-roadmap.html','Certification Roadmap — Project Controls Institute','The step-by-step roadmap to a PCI certification — PCL-AI™, PFL-AI™ or PDL-AI™: eligibility, preparation, examination, the credential and recertification.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification-scheme.html','Certification Scheme Document — Project Controls Institute','The master document defining the PCI certification scheme — its scope, requirements and governance.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certification.html','The PCP-AI Credential — Project Controls Institute','A rigorous credential covering project controls, cost engineering and project finance, with governed AI — built with reference to ISO/IEC 17024.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('certuvo.html','Certuvo','Certuvo is PCI’s official platform for the PCI certifications: fully online study and scenario-based practice that mirrors the exam.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-australia.html','PCI Australia Chapter — Project Controls Institute','The PCI Australia Chapter connects project-controls professionals across Australia&#x27;s infrastructure, construction, mining and resources sectors.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-bahrain.html','PCI Bahrain Chapter | Project Controls Institute Global','Join the PCI Bahrain chapter — a community for project-controls professionals across Bahrain, connected to the global PCP-AI standard. Knowledge, events.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-brazil.html','PCI Brazil Chapter | Project Controls Institute Global','Join the PCI Brazil chapter — a community for project-controls professionals across Brazil, connected to the global PCP-AI standard. Knowledge, events and.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-canada.html','PCI Canada Chapter — Project Controls Institute','The PCI Canada Chapter connects project-controls professionals across Canada&#x27;s engineering, construction, energy and resources sectors.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-egypt.html','PCI Egypt Chapter | Project Controls Institute Global','Join the PCI Egypt chapter — a community for project-controls professionals across Egypt, connected to the global PCP-AI standard. Knowledge, events and.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-europe.html','PCI Europe Chapter — Project Controls Institute','The PCI chapter for Europe — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-germany.html','PCI Germany Chapter | Project Controls Institute Global','Join the PCI Germany chapter — a community for project-controls professionals across Germany, connected to the global PCP-AI standard. Knowledge, events.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-india.html','PCI India Chapter — Project Controls Institute','The PCI chapter for India — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-ireland.html','PCI Ireland Chapter | Project Controls Institute Global','Join the PCI Ireland chapter — a community for project-controls professionals across Ireland, connected to the global PCP-AI standard. Knowledge, events.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-kenya.html','PCI Kenya Chapter | Project Controls Institute Global','Join the PCI Kenya chapter — a community for project-controls professionals across Kenya, connected to the global PCP-AI standard. Knowledge, events and.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-kuwait.html','PCI Kuwait Chapter — Project Controls Institute','The PCI Kuwait Chapter connects project-controls professionals across Kuwait&#x27;s oil, gas and downstream sectors.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-malaysia.html','PCI Malaysia Chapter | Project Controls Institute Global','Join the PCI Malaysia chapter — a community for project-controls professionals across Malaysia, connected to the global PCP-AI standard. Knowledge, events.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-mexico.html','PCI Mexico Chapter | Project Controls Institute Global','Join the PCI Mexico chapter — a community for project-controls professionals across Mexico, connected to the global PCP-AI standard. Knowledge, events and.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-netherlands.html','PCI Netherlands Chapter | Project Controls Institute Global','Join the PCI Netherlands chapter — a community for project-controls professionals across Netherlands, connected to the global PCP-AI standard. Knowledge.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-new-zealand.html','PCI New Zealand Chapter | Project Controls Institute Global','Join the PCI New Zealand chapter — a community for project-controls professionals across New Zealand, connected to the global PCP-AI standard. Knowledge.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-nigeria.html','PCI Nigeria Chapter | Project Controls Institute Global','Join the PCI Nigeria chapter — a community for project-controls professionals across Nigeria, connected to the global PCP-AI standard. Knowledge, events.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-oman.html','PCI Oman Chapter — Project Controls Institute','The PCI Oman Chapter connects project-controls professionals across Oman&#x27;s energy and infrastructure sectors.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-pakistan.html','PCI Pakistan Chapter — Project Controls Institute','The PCI chapter for Pakistan — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-qatar.html','PCI Qatar Chapter — Project Controls Institute','The PCI Qatar Chapter connects project-controls professionals across Qatar&#x27;s infrastructure, energy and development programmes.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-saudi.html','PCI Saudi Arabia Chapter — Project Controls Institute','The PCI chapter for Saudi Arabia — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-singapore.html','PCI Singapore Chapter | Project Controls Institute Global','Join the PCI Singapore chapter — a community for project-controls professionals across Singapore, connected to the global PCP-AI standard. Knowledge.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-south-africa.html','PCI South Africa Chapter | Project Controls Institute Global','Join the PCI South Africa chapter — a community for project-controls professionals across South Africa, connected to the global PCP-AI standard.','Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-uae.html','PCI United Arab Emirates Chapter | PCI','The PCI chapter for the United Arab Emirates — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-uk.html','PCI United Kingdom Chapter — Project Controls Institute','The PCI chapter for the United Kingdom — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapter-us.html','PCI United States Chapter — Project Controls Institute','The PCI chapter for the United States — regional overview, industry trends, careers, events and the PCI certifications.','Global Chapters',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('chapters.html','Global Chapters — Project Controls Institute','PCI&#x27;s worldwide chapters — Canada, Australia, Qatar, Kuwait, Oman and more — connecting project-controls professionals and major employers.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('checkout.html','Secure Checkout — Complete Your Enrolment | PCI','Secure one-time checkout for PCI student membership, your PCI certification exam, or both. Card details are handled by a PCI-DSS-compliant payment provider; PCI never.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('coi.html','Conflict of Interest Policy — Project Controls Institute','How PCI manages conflicts of interest — separation of duties, disclosure and recusal — to protect the impartiality of the PCI certifications.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('committee-terms.html','Committee Terms of Reference — Project Controls Institute','The mandate, membership and conduct of PCI&#x27;s governance committees.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('complaints-form.html','Complaints Form — Project Controls Institute','Raise a complaint with the institute. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('complaints.html','Complaints Process — Project Controls Institute','PCI&#x27;s transparent complaints process: how to raise a concern, how it is handled, timelines and escalation.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('confidentiality-policy.html','Confidentiality Policy — Project Controls Institute','How PCI protects confidential candidate, examination and organisational information.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('contact.html','Contact — Project Controls Institute','Questions about the PCI certifications, membership, examinations or partnerships? Talk to the Project Controls Institute.','Contact',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('continuous-improvement.html','Continuous Improvement Procedure | PCI','How PCI gets better over time, deliberately. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('cookie-policy.html','Cookie Policy — Project Controls Institute','How this website uses cookies and similar technologies. Explore PCP-AI certification and membership with PCI.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('copyright-policy.html','Copyright Policy — Project Controls Institute','Ownership of, and permitted use of, content on this website. Explore PCP-AI certification and membership with PCI.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('corporate-programs.html','Corporate Programmes — Project Controls Institute','Build project-controls capability at scale: team enrolment toward the PCI certifications, corporate membership, recognition and workforce development.','Contact',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('corrective-action.html','Corrective Action Procedure — Project Controls Institute','How PCI fixes problems and prevents them recurring. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('cpd-framework.html','CPD Framework — Project Controls Institute','PCI&#x27;s continuing professional development framework: the three-year cycle, the AI-currency component, what counts, audit and reinstatement.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('cpd-policy.html','CPD Policy — Project Controls Institute','The policy governing continuing professional development and the maintenance of competence.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('cpd-submission.html','CPD Submission Form — Project Controls Institute','Record continuing professional development. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('credential-verification.html','Credential Verification Framework | PCI','How employers and clients confirm PCI credentials quickly and independently through verification and the digital badge registry.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('curriculum.html','Curriculum','Ten sections across 93 Knowledge Areas: project management, planning and scheduling, cost estimating and control, earned value, risk and ERM, commercial.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('data-protection-policy.html','Data Protection Policy — Project Controls Institute','How PCI collects, uses and protects personal data. Explore PCP-AI certification and membership with PCI.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('decision-review.html','Decision Review Procedure — Project Controls Institute','How certification and disciplinary decisions can be reviewed, separately from the original decision-makers.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('digital-badge-registry.html','Digital Badge Registry — Project Controls Institute','The official register of PCI digital credentials — confirm whether a professional holds a current PCP-AI credential.','Verify',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('digital-credentials.html','Digital Credentials — Project Controls Institute','The PCP-AI digital credential: a verifiable, tamper-evident badge, a print-ready certificate and the right to use the post-nominal.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('disciplinary-policy.html','Disciplinary Policy — Project Controls Institute','How allegations of misconduct against certified professionals or members are handled.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('diversity-inclusion.html','Diversity &amp;amp; Inclusion Statement | PCI','PCI is for talent everywhere — our commitment to a fair, inclusive and global profession.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('donate.html','Donate — Support the Project Controls Institute (pursuing 501(c)(3))','PCI is a nonprofit (Delaware) pursuing 501(c)(3) recognition. Your gift keeps the standard rigorous, independent and open to talent.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('downloads.html','Guidelines, Policies & Downloads | PCI','All PCI policies and guidelines in one place — enrolment, payment, examination, membership and conduct — plus the emails you receive at each step and the 12-month exam-scheduling rule.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('eligibility-policy.html','Eligibility Policy — Project Controls Institute','The policy that governs who may sit a PCI examination and how eligibility is assessed.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('eligibility-requirements.html','Eligibility Requirements — Project Controls Institute','The two routes to PCP-AI eligibility: the experience route for practitioners and the Foundation route for those newer to project controls.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('employer-recognition.html','Employer Recognition Framework — PCI','How employers adopt and recognise the PCI certifications as the benchmark for project-controls capability, and how PCI recognises committed organisations.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('employers.html','For Employers — Project Controls Institute','Certify a whole team to one benchmarked standard for planning, cost, cash flow and AI-governed reporting across your portfolio.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('enrol.html','Enrol — Become a Student Member | PCI','Enrolment for the PCI certifications — PCL-AI™, PFL-AI™ and PDL-AI™ — is now open. Register your interest and start your application.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('enroll.html','Student Enrolment — Join PCI as a Student Member | PCI','Enrol as a PCI student member: membership USD 49.50 (USD 99 less a current 50% discount), a PCI certification exam at USD 350 (USD 500 less 30%), or both.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ethics-report.html','Ethics Violation Report — Project Controls Institute','Report a suspected breach of the Code of Ethics. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('ethics.html','Code of Ethics — Project Controls Institute','The PCI Code of Ethics: integrity, competence, objectivity, responsible AI and confidentiality for project-controls professionals.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('event-aiconf.html','AI in Project Controls Conference | PCI','A dedicated conference on applied, governed AI across planning, cost, forecasting, risk and reporting — with governance and MLOps.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('event-congress.html','PCI Global Congress — Project Controls Institute','The flagship annual gathering of the global project-controls community: keynotes, technical tracks and the governed use of AI in major projects.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('event-regional.html','Regional Conferences — Project Controls Institute','PCI regional conferences across Saudi Arabia, the UAE, the USA, the UK and India — local context, one global project-controls standard.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('event-summit.html','PCI World Summit — Project Controls Institute','An executive summit for leaders of major projects: capital strategy, assurance, governance and AI-enabled delivery.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('events.html','Annual Events — Project Controls Institute','PCI&#x27;s flagship gatherings — the Global Congress, World Summit, AI in Project Controls Conference and regional conferences for the project-controls.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('exam-misconduct.html','Exam Misconduct Policy — Project Controls Institute','How examination misconduct is defined, detected and handled. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('exam-rules.html','Examination Rules — Project Controls Institute','PCP-AI examination rules: security, identification, proctoring and candidate conduct, protecting the integrity of the credential.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('exam-structure.html','Exam Structure — Project Controls Institute','The PCP-AI exam: scenario-based multiple-choice questions, a published blueprint, and a criterion-referenced standard.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('examination-administration.html','Examination Administration Policy | PCI','How examinations are scheduled, delivered and supported. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('examination-development.html','Examination Development Policy — PCI','How examination content is developed, validated and kept fair and current.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('examination-security.html','Examination Security Policy — Project Controls Institute','How the integrity and confidentiality of the examination are protected.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('faq.html','FAQ — Project Controls Institute','Answers on the PCI certifications, the examinations, the AI standard, membership and employer certification.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('fellowship-policy.html','Fellowship Policy — Project Controls Institute','How Fellowship — the institute&#x27;s most senior grade — is awarded. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('forgot-password.html','Reset Your Password | Project Controls Institute Global','Request a password reset link for your PCI account. Enter your email and we’ll send instructions to reset your password.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('founding-status.html','Founding Status & Commitments | PCI','An honest account of what the Project Controls Institute has established and what is still in development, including our accreditation roadmap and transparency commitments.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('governance.html','Governance — Project Controls Institute','How the PCI standards are governed: independence, separated duties, the modified-Angoff cut score and the road to ISO/IEC 17024.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('government-programs.html','Government Programmes for Project Controls Capability — PCI','How government bodies build project-controls capability — workforce development, recognition and alignment to a professional standard, with governed AI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('handbook.html','Candidate Handbook — Project Controls Institute','The comprehensive PCP-AI candidate handbook: eligibility, booking, exam-day rules, results, recertification and your rights.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('how-pci-is-different.html','How PCI Is Different','How PCI&#x27;s AI-era project-controls certification focus compares with broader project management, cost engineering, accounting, governance and.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('human-oversight.html','Human Oversight Policy — Project Controls Institute','The requirement that a competent professional governs AI at every consequential step.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('impartiality-policy.html','Impartiality Policy — Project Controls Institute','How PCI safeguards the impartiality on which a trusted credential depends.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('index.html','Project Controls Institute','The Project Controls Institute (PCI) awards the PCI certifications — ISO/IEC 17024-referenced credential uniting project controls, cost engineering and project.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('industry-reports.html','Industry Reports — Project Controls Institute','PCI industry reports: sector trends, project-controls practice and AI adoption across energy, rail, aerospace, construction and more.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('insights.html','Insights — Project Controls Institute','Practical thinking on project controls, cost engineering, project finance and the governed use of AI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('internal-audit.html','Internal Audit Procedure — Project Controls Institute','How PCI checks that its processes work as intended. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('investigation-procedure.html','Investigation Procedure — Project Controls Institute','How concerns and complaints are investigated before any decision is taken.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-ai-cost.html','AI for cost engineers — PCI Knowledge Centre','Sharper estimates and earlier warnings — under control. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-ai-gov.html','AI governance — PCI Knowledge Centre','Making AI trustworthy enough to bet a project on. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-ai-mega.html','AI in mega projects — PCI Knowledge Centre','Where AI earns its place — at the largest, most complex scale. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-ai-planning.html','AI for planning engineers — PCI Knowledge Centre','What schedulers need to know about AI — and how to use it well. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-ai.html','AI in project controls — PCI Knowledge Centre','Applied, governed AI across the whole discipline. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-cost-control.html','Cost control — PCI Knowledge Centre','Keeping the outturn cost under control, in real time. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-cost-eng.html','Cost engineering — PCI Knowledge Centre','Turning scope into a credible number. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-evm.html','Earned value management — PCI Knowledge Centre','One method that unites scope, schedule and cost. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-finance.html','Project finance — PCI Knowledge Centre','How major projects are funded — and why controls must speak its language.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-forecasting.html','Forecasting — PCI Knowledge Centre','Seeing the outcome early enough to change it. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-planning.html','Planning &amp; scheduling guide — PCI Knowledge Centre','Building a schedule you can actually drive a project with. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-risk.html','Risk management — PCI Knowledge Centre','Managing uncertainty before it manages the project. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge-what.html','What is project controls? — PCI Knowledge Centre','The discipline that keeps major projects on time, on cost and under control.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('knowledge.html','Knowledge Centre — Project Controls Institute','A comprehensive library on project controls — planning, cost, forecasting, risk, project finance and the governed use of AI across all of it.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('landscape.html','How PCI Fits Within the Professional Landscape | PCI','PCI complements PMI, AACE, ACCA, CFA and ISACA by focusing on integrated project controls and AI-enabled project delivery. See where PCI fits, who.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('leadership.html','Leadership, Examiners & Governance | PCI','How the Project Controls Institute is governed, the roles that make certification decisions, and our open call for founding examiners and subject-matter experts.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('login.html','Log in | Project Controls Institute Global','Log in to your PCI account to access your candidate profile, certification status and resources. Continue with Google or email.','Verify',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-associate.html','Associate Membership — Project Controls Institute','For early-career professionals and those actively working toward a PCI certification — PCI membership grades, benefits and how to join.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-benefits.html','Membership Benefits — Project Controls Institute','The benefits of PCI membership: knowledge, research, community, events, recognition, CPD and member rates across every grade.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-conduct.html','Membership Code of Conduct — Project Controls Institute','The standards of behaviour expected of all members. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-corporate.html','Corporate Membership — Project Controls Institute','For employers building project-controls capability at scale — PCI membership grades, benefits and how to join.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-fellow.html','Fellow Membership — Project Controls Institute','The institute&#x27;s most senior grade, recognising distinguished contribution to the project-controls profession — PCI membership grades, benefits and.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-policy.html','Membership Policy — Project Controls Institute','The policy governing membership of the institute — grades, rights and obligations.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-professional.html','Professional Membership — Project Controls Institute','For experienced professionals who hold a PCI certification — PCI membership grades, benefits and how to join.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-student.html','Student Membership — Project Controls Institute','For students and recent graduates exploring or entering the project-controls profession — PCI membership grades, benefits and how to join.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership-termination.html','Membership Termination Policy — Project Controls Institute','How membership ends — by resignation, non-payment or breach. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('membership.html','Membership — Project Controls Institute','Keep your PCP-AI credential current with CPD tracking, a verifiable digital credential, body-of-knowledge updates and member rates.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('route-standard.html','Standard route — Project Controls Institute','Enrol on the standard route: create a free account, activate membership, pay the exam fee and earn your PCI credential.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('route-founding.html','Founding route — Project Controls Institute','The founding route: an invitation-only cohort with membership, study and exam access; the credential is still earned by passing the exam.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('route-honorary.html','Honorary route — Project Controls Institute','Apply for the board''s consideration to be conferred Honorary Fellow (PCI) — a distinguished-contribution recognition, separate from the examined PCP-AI credential.','Membership',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('mission-vision.html','Mission &amp; Vision — Project Controls Institute','The mission and vision of the Project Controls Institute: to become the global professional institute for project controls and AI-enabled project delivery.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('organizations.html','Organisations — Project Controls Institute','PCI for organisations: corporate and government programmes, university partnerships, workforce development and employer recognition.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('payment-failed.html','Payment Could Not Be Completed | Project Controls Institute','Your PCI enrolment payment was not completed, so no account was activated. Retry payment or contact support.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('payment-success.html','Enrolment Complete — Welcome to PCI | PCI','Your PCI payment was successful and your access has been activated. A secure setup link has been emailed to you.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('pmp-vs-aace-vs-pcp-ai.html','PMP vs AACE vs PCP-AI','A positioning guide to three certification directions — broad project management, cost engineering, and AI-era project controls. Not an official.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('policies.html','Policies &amp; Governance Documents | PCI','Every PCI policy and governance document: certification scheme, examination, conduct, AI governance, quality, legal and forms — built for reference to.','Governance',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('privacy.html','Privacy Policy — Project Controls Institute','How PCI collects, uses, shares and protects personal information — and the privacy rights you have.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('professional-conduct.html','Professional Conduct Policy — Project Controls Institute','The standards of professional behaviour expected of certified professionals and members.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-india.html','Project Controls Certification in India | PCP-AI by PCI','PCI&#x27;s PCP-AI certification framework for project controls professionals in India — planning, cost control, forecasting, project finance, reporting.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-pakistan.html','Project Controls Certification in Pakistan | PCP-AI by PCI','PCI&#x27;s PCP-AI certification framework for project controls professionals in Pakistan — planning, cost control, forecasting, project finance, reporting.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-saudi-arabia.html','Project Controls Certification in Saudi Arabia','PCI&#x27;s PCP-AI certification framework for project controls professionals in Saudi Arabia — planning, cost control, forecasting, project finance.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-uae.html','Project Controls Certification in the UAE | PCP-AI by PCI','PCI&#x27;s PCP-AI certification framework for project controls professionals in the UAE — planning, cost control, forecasting, project finance, reporting.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-uk.html','Project Controls Certification in the UK | PCP-AI by PCI','PCI&#x27;s PCP-AI certification framework for project controls professionals in the UK — planning, cost control, forecasting, project finance, reporting.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-certification-usa.html','Project Controls Certification in the USA | PCP-AI by PCI','PCI&#x27;s PCP-AI certification framework for project controls professionals in the USA — planning, cost control, forecasting, project finance, reporting.','Certification by Region',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-director-career-path.html','Project Controls Director Career Path | PCI','How project-controls professionals reach director level through portfolio controls, governance, executive reporting, risk escalation and responsible.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('project-controls-manager-career-path.html','Project Controls Manager Career Path | PCI','How project-controls professionals grow into manager roles — integrating planning, cost, forecasting, risk, reporting and project finance, and leading the.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('publications.html','Publications — Project Controls Institute','PCI publications: research, white papers, industry and salary reports, and practice guides — independent, evidence-based and vendor-neutral.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('quality-policy.html','Quality Policy — Project Controls Institute','PCI&#x27;s commitment to quality across certification and everything it does.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('recert.html','Recertification Policy — Project Controls Institute','The PCP-AI recertification policy: the three-year CPD cycle, the mandatory AI-currency component, audit and reinstatement.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('recertification-application.html','Recertification Application — Project Controls Institute','Renew your PCP-AI credential. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('records-retention.html','Records Retention Policy — Project Controls Institute','What records PCI keeps, for how long, and how they are protected and disposed of.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('refund-policy.html','Refund Policy | Project Controls Institute Global','PCI&#x27;s refund and cancellation policy framework for application, examination and membership fees. Fees and final terms are confirmed and enrolment is.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('remote-proctoring.html','Remote Proctoring Policy — Project Controls Institute','How remotely proctored examinations are conducted securely and fairly.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('request-info.html','Request Information — Project Controls Institute','Request information from PCI about certification, membership, corporate, government and university programmes, research and events.','Contact',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('research.html','PCI Research Institute — Project Controls Institute','Independent research on project controls and AI: salary reports, industry benchmarks, AI adoption reports, mega-project studies and maturity assessments.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('reset-password.html','Set a New Password | Project Controls Institute Global','Set a new password for your PCI account. Explore PCP-AI certification and membership with PCI.','Verify',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('resources.html','Resources — Project Controls Institute','PCI resources: Knowledge Centre, research, publications, white papers, industry and salary reports, career guides and webinars.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('responsible-ai-framework.html','Responsible AI Framework — Project Controls Institute','The framework governing how AI is used, taught and assessed across PCI and its certifications.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('retake-policy.html','Retake Policy — Project Controls Institute','How and when candidates may retake the examination. Explore PCP-AI certification and membership with PCI.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('salary-reports.html','Salary Reports — Project Controls Institute','Independent project-controls compensation benchmarks by role, region, sector and certification — with a transparent, published methodology.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sample-questions.html','Sample Questions (Illustrative) — PCP-AI | PCI','Illustrative sample questions showing the style and format of the PCI examinations. For familiarisation only — these are not live exam items.','Certifications',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sanctions-policy.html','Sanctions Policy — Project Controls Institute','The range of sanctions that may follow a substantiated breach, and how they are applied.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-aero.html','Aerospace &amp; Defence — Project Controls Institute','Project controls for aerospace and defence: aircraft, spacecraft, defence platforms and advanced technologies — with governed AI and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-aviation.html','Aviation — Project Controls Institute','Project controls and governed AI in Aviation: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-construction.html','Construction &amp; Infrastructure | PCI','Project controls and governed AI in Construction &amp; Infrastructure: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-datacenters.html','Data Centers — Project Controls Institute','Project controls and governed AI in Data Centers: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-energy.html','Energy &amp; Utilities — Project Controls Institute','Project controls for energy and utilities: oil &amp; gas, power generation, renewables, energy storage and water — with governed AI and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-government.html','Government Sector Project Controls | PCI','Project controls for government and public-sector programmes — planning, cost control, risk and governed AI across infrastructure, defence and civil.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-manufacturing.html','Manufacturing — Project Controls Institute','Project controls and governed AI in Manufacturing: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-oilgas.html','Oil &amp; Gas — Project Controls Institute','Project controls and governed AI in Oil &amp; Gas: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-rail.html','Rail &amp; Transportation — Project Controls Institute','Project controls for rail and transportation: high-speed rail, metro, freight, airports, highways and ITS — with governed AI and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sector-smartcities.html','Smart Cities — Project Controls Institute','Project controls and governed AI in Smart Cities: overview, challenges, applications, careers and the PCI certifications.','Industry Sectors',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('sectors.html','Industry Sectors — Project Controls Institute','PCI industry sectors: energy &amp; utilities, rail &amp; transportation, aerospace &amp; defence and more — one global project-controls standard with.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('special-accommodations.html','Special Accommodations Policy — Project Controls Institute','How PCI provides reasonable accommodations so the examination is fair and accessible.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('standards.html','PCI Standards Committee — Project Controls Institute','Forthcoming PCI standards: AI governance for project controls, forecasting best practices, reporting standards and a competency framework.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('strategic-plan.html','Strategic Plan — Project Controls Institute','PCI&#x27;s strategic priorities: a benchmark credential, accreditation-ready governance, research and standards, a global community and responsible AI.','About PCI',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('student-dashboard.html','Student Dashboard | Project Controls Institute','Your PCI student panel: membership and payment status, fees and renewal, learning pathway, PCP-AI certification preparation, documents and account.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('student-login.html','Student Login | Project Controls Institute','Sign in to your PCI student dashboard to access your learning pathway and certification-preparation resources.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('student-registration.html','Student Registration — Create Your PCI Student Profile | PCI','Create your PCI student profile in a few short steps. Enrolment starts with your email so your progress is saved — return anytime using the same email to.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('student-welcome.html','Welcome to PCI — Student Onboarding | PCI','Welcome to PCI. Set your password, complete your profile, and begin your project controls learning and certification-preparation pathway.','Utility',1,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('terms-of-enrollment.html','Terms of Enrolment | Project Controls Institute','The terms governing PCI student membership and enrolment: what membership provides, payment and refunds, account security, and how certification remains.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('terms.html','Terms of Use — Project Controls Institute','The terms on which the Project Controls Institute website and online services are provided.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('university-partnerships.html','University Partnerships — Project Controls Institute','Connect academic programmes to the global project-controls standard: curriculum alignment, a student pathway to the PCI certifications and research collaboration.','Contact',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('verify.html','Verify a Credential — Project Controls Institute','Confirm a professional&#x27;s PCP-AI status with the Project Controls Institute credential lookup.','Verify',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('webinars.html','Webinars — Project Controls Institute','PCI webinars: live and on-demand sessions on project controls, the PCI certifications and governed AI — counting toward CPD.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('website-disclaimer.html','Website Disclaimer — Project Controls Institute','Important information about the content and status of this website. Explore PCP-AI certification and membership with PCI.','Legal',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('white-papers.html','White Papers — Project Controls Institute','PCI white papers on governed AI, earned value, forecasting, project-controls maturity, standards and assurance.','Resources',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('why-employers.html','Why Employers Value PCP-AI — Project Controls Institute','Why EPC contractors, owners, energy, defence, aerospace and government value PCI certifications — validating planning, cost, forecasting, risk and AI governance.','Other',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('why-pci.html','Why PCI | Project Controls Institute Global','Why PCI is building a focused certification standard for project controls professionals in the AI era — a distinct capability that matters more as AI.','Explore',0,1);
INSERT OR IGNORE INTO pages(slug,title,meta_description,nav_group,noindex,published) VALUES('workforce-development.html','Workforce Development — Project Controls Institute','Build project-controls capability at national, sector and organisational scale through cohort development toward the PCI certifications.','Other',0,1);
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('about-institution.jpg','About Institution','about.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('accreditation-building.jpg','Accreditation Building','accreditation-status.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('ai-data.jpg','Ai Data','ai-policy.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('ai-digital.jpg','Ai Digital','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-01.jpg','Bg 01','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-02.jpg','Bg 02','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-03.jpg','Bg 03','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-04.jpg','Bg 04','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-05.jpg','Bg 05','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-06.jpg','Bg 06','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-07.jpg','Bg 07','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-08.jpg','Bg 08','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-09.jpg','Bg 09','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-10.jpg','Bg 10','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-11.jpg','Bg 11','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-12.jpg','Bg 12','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-13.jpg','Bg 13','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-14.jpg','Bg 14','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-15.jpg','Bg 15','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-16.jpg','Bg 16','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-17.jpg','Bg 17','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-18.jpg','Bg 18','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-19.jpg','Bg 19','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-20.jpg','Bg 20','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-21.jpg','Bg 21','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-22.jpg','Bg 22','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('bg-23.jpg','Bg 23','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('brazil-skyline.jpg','Brazil Skyline','chapter-brazil.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('candidate-study.jpg','Candidate Study','candidate-journey.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('career-outdoors.jpg','Career Outdoors','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('cert-graduation.jpg','Cert Graduation','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('compliance-tiles.jpg','Compliance Tiles','cert-policies.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('corporate-professionals.jpg','Corporate Professionals','corporate-programs.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('credential-graduate.jpg','Credential Graduate','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('employers-team.jpg','Employers Team','employers.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('enrol-yes.jpg','Enrol Yes','enrol.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('ethics-gavel.jpg','Ethics Gavel','appeals.html +1 more');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('europe-landmark.jpg','Europe Landmark','chapter-europe.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('events-session.jpg','Events Session','events.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('exam-room.jpg','Exam Room','exam-structure.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('germany-city.jpg','Germany City','chapter-germany.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('goal-arrow.jpg','Goal Arrow','strategic-plan.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('governance-boardroom.jpg','Governance Boardroom','governance.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('handbook-study.jpg','Handbook Study','handbook.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('knowledge-abstract.jpg','Knowledge Abstract','knowledge.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('membership-team.jpg','Membership Team','unused');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('org-boardroom.jpg','Org Boardroom','organizations.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('pakistan-landmark.jpg','Pakistan Landmark','project-controls-certification-pakistan.html +1 more');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('policy-tiles.jpg','Policy Tiles','policies.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('recognition-team.jpg','Recognition Team','employer-recognition.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('regulation-tiles.jpg','Regulation Tiles','impartiality-policy.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('resources-desk.jpg','Resources Desk','resources.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('security-tiles.jpg','Security Tiles','examination-security.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('strategy-chess.jpg','Strategy Chess','why-pci.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('student-study.jpg','Student Study','membership-student.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('university-students.jpg','University Students','university-partnerships.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('us-capitol.jpg','Us Capitol','chapter-us.html +1 more');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('vision-blocks.jpg','Vision Blocks','mission-vision.html');
INSERT OR IGNORE INTO media_assets(filename,alt,usage) VALUES('workforce-training.jpg','Workforce Training','workforce-development.html');
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('Which metric best indicates cost efficiency in earned value management?','SPI
CPI
BAC
ETC',1,'Earned Value & Performance',1,2);
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('When applying AI to schedule risk analysis, the professional’s primary responsibility is to:','Accept the AI output as final
Oversee, validate and remain accountable for the output
Automate the decision end-to-end
Withhold the methodology',1,'Governed AI',1,3);
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('A schedule performance index (SPI) of 0.85 indicates the project is:','Ahead of schedule
On schedule
Behind schedule
Under budget',2,'Planning & Scheduling',1,4);
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('Contingency reserves are best sized using:','A fixed 10% rule in all cases
Quantitative risk analysis of the specific project
The client’s preference alone
Last year’s average',1,'Risk & Contingency',1,5);
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('The primary purpose of a cost baseline is to:','Replace estimates
Provide the approved reference for performance measurement
Eliminate change control
Set the contractor’s profit',1,'Cost Estimating & Budgeting',1,6);
INSERT OR IGNORE INTO sample_questions(question,options,answer_index,domain,published,sort_order) VALUES('Under PCI’s governed-AI principle, “AI proposes, the professional disposes” means:','AI decisions are final
The professional reviews, adjusts and owns the decision
AI must not be used
Only managers may use AI',1,'Governed AI',1,7);
INSERT INTO pricing_rules(product_type,standard_price,default_discount_percentage,active)
 SELECT 'renewal',99,0,1 WHERE NOT EXISTS(SELECT 1 FROM pricing_rules WHERE product_type='renewal');
INSERT INTO pricing_rules(product_type,standard_price,default_discount_percentage,active)
 SELECT 'recert',99,0,1 WHERE NOT EXISTS(SELECT 1 FROM pricing_rules WHERE product_type='recert');
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES('site_base_url','https://www.projectcontrolsinstitute.org');

-- ===== DISCOUNT v2: redemption ledger + integrity indexes =====
CREATE TABLE IF NOT EXISTS code_redemptions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  code_id INTEGER, code TEXT, user_id INTEGER, email TEXT,
  payment_id INTEGER UNIQUE,                 -- one redemption per payment (idempotent)
  product_type TEXT, amount_before REAL, discount_amount REAL,
  redeemed_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_redemptions_code ON code_redemptions(code_id);
CREATE INDEX IF NOT EXISTS ix_redemptions_email ON code_redemptions(email);
CREATE UNIQUE INDEX IF NOT EXISTS ux_payments_provider ON payments(provider_payment_id);
CREATE INDEX IF NOT EXISTS ix_payments_date ON payments(payment_date);
CREATE INDEX IF NOT EXISTS ix_payments_status ON payments(payment_status);
CREATE INDEX IF NOT EXISTS ix_codes_type ON discount_codes(code_type);

-- ===== STUDENT PORTAL =====
CREATE TABLE IF NOT EXISTS exam_bookings (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL, payment_id INTEGER,
  certification_id INTEGER DEFAULT 1 REFERENCES certifications(id),
  scheduled_at TEXT NOT NULL, timezone TEXT,
  status TEXT DEFAULT 'scheduled',           -- scheduled | completed | missed | cancelled
  reschedule_count INTEGER DEFAULT 0,
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_bookings_user ON exam_bookings(user_id);
CREATE TABLE IF NOT EXISTS exam_attempts (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL, booking_id INTEGER,
  certification_id INTEGER DEFAULT 1 REFERENCES certifications(id),
  kind TEXT DEFAULT 'exam',                  -- exam | practice
  violations INTEGER DEFAULT 0,
  started_at TEXT DEFAULT (datetime('now')), submitted_at TEXT,
  duration_minutes INTEGER DEFAULT 90,
  item_ids TEXT, answers TEXT,
  score REAL, max_score REAL, percent REAL, result TEXT,   -- pass | fail
  domain_breakdown TEXT,
  status TEXT DEFAULT 'in_progress',           -- in_progress | submitted | expired
  result_status TEXT DEFAULT 'not_started', hold_reason TEXT, released_at TEXT, answer_key_version TEXT, bank_version TEXT
);
CREATE INDEX IF NOT EXISTS ix_attempts_user ON exam_attempts(user_id);
CREATE TABLE IF NOT EXISTS tickets (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL, reference TEXT UNIQUE,
  subject TEXT NOT NULL, category TEXT DEFAULT 'General',
  status TEXT DEFAULT 'open',                -- open | awaiting_student | resolved | closed
  priority TEXT DEFAULT 'normal',
  created_at TEXT DEFAULT (datetime('now')), updated_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS ticket_messages (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  ticket_id INTEGER NOT NULL REFERENCES tickets(id),
  sender TEXT NOT NULL,                      -- student | admin
  body TEXT NOT NULL, created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_tmsg_ticket ON ticket_messages(ticket_id);

-- ===== CPD & renewals =====
CREATE TABLE IF NOT EXISTS cpd_entries (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL, activity_date TEXT, category TEXT,
  hours REAL DEFAULT 0, description TEXT,
  evidence_name TEXT, evidence_data TEXT, admin_note TEXT, reviewed_by INTEGER, reviewed_at TEXT,
  status TEXT DEFAULT 'recorded', created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_cpd_user ON cpd_entries(user_id);

-- ===== panel: security, messages, enrollment resume =====
CREATE TABLE IF NOT EXISTS login_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER NOT NULL,
  ip TEXT, user_agent TEXT, device TEXT, outcome TEXT DEFAULT 'success',
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_login_events_user ON login_events(user_id);
CREATE TABLE IF NOT EXISTS notifications (
  id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER,
  category TEXT DEFAULT 'General', title TEXT NOT NULL, body TEXT,
  cta_label TEXT, cta_route TEXT, dedupe_key TEXT,
  read_at TEXT, created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_notif_user ON notifications(user_id);
CREATE TABLE IF NOT EXISTS account_requests (
  id INTEGER PRIMARY KEY AUTOINCREMENT, user_id INTEGER NOT NULL,
  kind TEXT, detail TEXT, status TEXT DEFAULT 'received', created_at TEXT DEFAULT (datetime('now'))
);

-- ===== secure exam client integration: launch, proctoring, evidence, identity =====
CREATE TABLE IF NOT EXISTS exam_launch_codes (
  id INTEGER PRIMARY KEY AUTOINCREMENT, code TEXT, code_hash TEXT UNIQUE NOT NULL,
  user_id INTEGER NOT NULL, booking_id INTEGER, attempt_id INTEGER,
  expires_at TEXT, redeemed_at TEXT, created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_launch_code ON exam_launch_codes(code);
CREATE TABLE IF NOT EXISTS proctor_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT, attempt_id INTEGER NOT NULL, user_id INTEGER,
  type TEXT NOT NULL, severity TEXT DEFAULT 'Info', detail TEXT, evidence_ref TEXT,
  at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_proctor_attempt ON proctor_events(attempt_id);
CREATE TABLE IF NOT EXISTS exam_evidence (
  id INTEGER PRIMARY KEY AUTOINCREMENT, attempt_id INTEGER NOT NULL, user_id INTEGER,
  kind TEXT, mime TEXT DEFAULT 'image/jpeg', data_uri TEXT, storage_ref TEXT, size_bytes INTEGER, sha256 TEXT, note TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_evidence_attempt ON exam_evidence(attempt_id);
CREATE TABLE IF NOT EXISTS identity_checks (
  id INTEGER PRIMARY KEY AUTOINCREMENT, attempt_id INTEGER NOT NULL, user_id INTEGER,
  result TEXT, confidence REAL, note TEXT, face_ref TEXT, id_ref TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS proctor_messages (
  id INTEGER PRIMARY KEY AUTOINCREMENT, attempt_id INTEGER NOT NULL, user_id INTEGER,
  sender TEXT NOT NULL DEFAULT 'proctor',            -- proctor | candidate
  body TEXT NOT NULL, created_at TEXT DEFAULT (datetime('now')), delivered_at TEXT
);
CREATE INDEX IF NOT EXISTS ix_pm_attempt ON proctor_messages(attempt_id);

-- ===== structured settings for the three apps (website / student panel / live exam) =====
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES
 -- Website
 ('web_maintenance_mode','0'),
 ('web_enrolment_open','1'),
 ('web_membership_open','1'),
 ('web_show_pricing','1'),
 ('web_announcement_enabled','0'),
 ('web_announcement_text','Founding cohort enrolment is now open.'),
 ('web_membership_price','99'),
 ('web_membership_discount_pct','50'),
 ('web_exam_price','500'),
 ('web_exam_discount_pct','30'),
 ('web_contact_email','hello@projectcontrolsinstitute.org'),
 -- Student panel
 ('sp_login_enabled','1'),
 ('sp_exam_booking_open','1'),
 ('sp_reschedule_enabled','1'),
 ('sp_reschedule_cutoff_hours','72'),
 ('sp_results_visible','1'),
 ('sp_certificate_download','1'),
 ('sp_cpd_enabled','1'),
 ('sp_cpd_target_hours','30'),
 ('sp_support_tickets_enabled','1'),
 ('sp_practice_enabled','1'),
 ('sp_banner_enabled','0'),
 ('sp_banner_text',''),
 -- Live exam
 ('exam_duration_minutes','90'),
 ('exam_pass_mark_pct','65'),
 ('exam_open_before_minutes','15'),
 ('exam_grace_minutes','30'),
 ('exam_require_identity','1'),
 ('exam_require_room_scan','1'),
 ('exam_proctoring_enabled','1'),
 -- Result publication: immediate auto-publish is the DEFAULT. These hard-invalidation rules are OFF by
 -- default so proctoring/identity evidence is audit-only and never delays a valid result. Enable per policy.
 ('auto_block_result_on_tampered_attempt','0'),
 ('auto_block_result_on_critical_violation','0'),
 ('auto_block_result_on_identity_fail','0'),
 ('critical_violation_threshold','1'),
 ('evidence_retention_days','365'),
 ('exam_block_second_monitor','1'),
 ('exam_terminate_on_prohibited','0'),
 ('exam_chat_enabled','1'),
 ('exam_absence_seconds','8'),
 ('exam_evidence_interval_seconds','15'),
 ('exam_identity_provider','Baseline');

-- ===== admin RBAC: staff accounts, roles, per-section permissions =====
CREATE TABLE IF NOT EXISTS admin_users (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  email TEXT UNIQUE NOT NULL, name TEXT, password_hash TEXT,
  role TEXT NOT NULL DEFAULT 'viewer',           -- owner | website_manager | student_manager | exam_manager | custom | viewer
  permissions TEXT DEFAULT '[]',                 -- JSON array of section ids this admin may access (custom/override)
  status TEXT NOT NULL DEFAULT 'active',          -- active | suspended
  must_change_pw INTEGER DEFAULT 1,
  last_login_at TEXT, created_by INTEGER, created_at TEXT DEFAULT (datetime('now'))
);
CREATE TABLE IF NOT EXISTS admin_sessions (
  id INTEGER PRIMARY KEY AUTOINCREMENT, admin_id INTEGER NOT NULL REFERENCES admin_users(id),
  token TEXT UNIQUE NOT NULL, expires_at TEXT, created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_admin_sess ON admin_sessions(token);

-- ===== profile wizard: per-user experience, qualifications, held certifications =====
CREATE TABLE IF NOT EXISTS work_experiences (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  company TEXT NOT NULL, title TEXT NOT NULL,
  start_date TEXT, end_date TEXT, is_current INTEGER DEFAULT 0,
  country TEXT, industry TEXT, hours_per_week TEXT, summary TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_workexp_user ON work_experiences(user_id);
CREATE TABLE IF NOT EXISTS qualifications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  institution TEXT NOT NULL, degree TEXT NOT NULL,
  field TEXT, year_completed TEXT, country TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_qual_user ON qualifications(user_id);
CREATE TABLE IF NOT EXISTS held_certifications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  name TEXT NOT NULL,
  issuer TEXT, credential_ref TEXT, issued_year TEXT, expires_year TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_heldcert_user ON held_certifications(user_id);

-- ===== identity documents: a government-issued photo ID is required before booking an exam =====
-- The file itself lives in Storage (local/S3); the row holds metadata + the provider reference.
-- status: submitted (satisfies the booking gate) | verified | rejected (re-blocks until re-upload)
CREATE TABLE IF NOT EXISTS identity_documents (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  doc_kind TEXT DEFAULT 'passport',            -- passport | national_id | driving_licence | other
  filename TEXT, mime TEXT, size_bytes INTEGER, storage_ref TEXT, sha256 TEXT,
  status TEXT NOT NULL DEFAULT 'submitted',
  review_note TEXT, reviewed_by INTEGER, reviewed_at TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_iddoc_user ON identity_documents(user_id);
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('sp_require_identity_document','1');

-- ===== founding-stage applications (Route 2: self-declared eligibility + evidence) =====
CREATE TABLE IF NOT EXISTS founding_applications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_id INTEGER NOT NULL,
  code_id INTEGER NOT NULL,
  route TEXT,                                -- founding (legacy tier values migrated forward)
  declared_experience_years INTEGER,
  declared_role TEXT,
  declared_qualification TEXT,
  evidence_ref TEXT,                         -- Storage reference (local:/s3:), required
  evidence_name TEXT, evidence_mime TEXT, evidence_size INTEGER,
  status TEXT NOT NULL DEFAULT 'pending_review', -- auto_approved | pending_review | approved | rejected | revoked
  decided_by INTEGER, decided_at TEXT, admin_note TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_founding_app_user ON founding_applications(user_id);
CREATE INDEX IF NOT EXISTS ix_founding_app_code ON founding_applications(code_id);

-- ===== honorary awards: board-conferred recognition, deliberately SEPARATE from exam credentials =====
-- "Honorary Fellow (PCI)" is NOT the PCL-AI credential: no exam, no entitlement, no issued_credentials
-- row. The PCI-HON award number prefix guarantees it can never be confused with a PCP-AI id.
CREATE TABLE IF NOT EXISTS honorary_awards (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  award_no TEXT UNIQUE NOT NULL,             -- PCI-HON-YYYY-NNNN
  recipient_name TEXT NOT NULL,
  user_id INTEGER,                           -- optional link when the recipient has an account
  citation TEXT,                             -- the board's reason (shown on certificate/verify)
  designation TEXT DEFAULT 'Honorary Fellow (PCI)',
  status TEXT DEFAULT 'active',              -- active | revoked
  conferred_by INTEGER NOT NULL,             -- admin id (board/owner)
  conferred_at TEXT DEFAULT (datetime('now')),
  revoked_by INTEGER, revoked_at TEXT, revoke_reason TEXT
);
CREATE INDEX IF NOT EXISTS ix_honorary_user ON honorary_awards(user_id);

-- ===== honorary-route APPLICATIONS (public apply → board review → conferral). Distinct from the
-- board-only manual conferral above: anyone may apply, the board reviews, and approval confers a
-- normal honorary_awards row. Never touches exam/entitlement/credential data. =====
CREATE TABLE IF NOT EXISTS honorary_applications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  reference TEXT UNIQUE NOT NULL,             -- PCI-HONAPP-YYYY-NNNN
  first_name TEXT, last_name TEXT, email TEXT, mobile TEXT,
  country TEXT, city TEXT, nationality TEXT,
  job_title TEXT, employer TEXT, years_experience INTEGER, industry TEXT,
  highest_qualification TEXT, professional_certifications TEXT,
  relevant_experience TEXT, professional_summary TEXT,
  declaration INTEGER DEFAULT 0,
  status TEXT NOT NULL DEFAULT 'pending_review', -- pending_review | under_review | approved | rejected
  award_no TEXT,                             -- set on approval (links to honorary_awards.award_no)
  decided_by INTEGER, decided_at TEXT, admin_note TEXT,
  created_at TEXT DEFAULT (datetime('now')),
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_honapp_status ON honorary_applications(status);
CREATE TABLE IF NOT EXISTS honorary_application_documents (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  application_id INTEGER NOT NULL,
  doc_kind TEXT DEFAULT 'supporting',        -- resume | academic | certifications | supporting
  filename TEXT, mime TEXT, size_bytes INTEGER, storage_ref TEXT, sha256 TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_honappdoc_app ON honorary_application_documents(application_id);

-- ===== Training Partner framework (Phase 7): third-party training providers apply to be recognised
-- as PCI Training Partners. Applications are reviewed in the admin console; on approval a directory
-- entry (training_partners) is created and, once published (listed=1), rendered on the public site.
-- Certification stays independent of training — a partner delivers exam-prep, never the exam itself. =====
CREATE TABLE IF NOT EXISTS training_partners (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  name TEXT NOT NULL,
  slug TEXT UNIQUE,
  tier TEXT NOT NULL DEFAULT 'registered',   -- registered | authorized | premier
  country TEXT, region TEXT, city TEXT,
  website TEXT, logo_url TEXT,
  summary TEXT, description TEXT, specialties TEXT,
  contact_email TEXT,
  listed INTEGER DEFAULT 0,                   -- 1 = published to the public directory
  sort_order INTEGER DEFAULT 0,
  source_application_id INTEGER,             -- set when created from an approved application
  approved_at TEXT,
  created_at TEXT DEFAULT (datetime('now')),
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_partners_listed ON training_partners(listed);
CREATE INDEX IF NOT EXISTS ix_partners_tier ON training_partners(tier);
CREATE TABLE IF NOT EXISTS training_partner_applications (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  reference TEXT UNIQUE NOT NULL,            -- PCI-TPA-YYYY-NNNN
  org_name TEXT, website TEXT,
  contact_name TEXT, contact_email TEXT, contact_phone TEXT,
  country TEXT, city TEXT, region TEXT,
  delivery_modes TEXT, specialties TEXT, learners_per_year INTEGER,
  description TEXT,
  declaration INTEGER DEFAULT 0,
  status TEXT NOT NULL DEFAULT 'pending_review', -- pending_review | under_review | approved | rejected
  proposed_tier TEXT,                        -- tier granted on approval
  partner_id INTEGER,                        -- set on approval (links to training_partners.id)
  decided_by INTEGER, decided_at TEXT, admin_note TEXT,
  created_at TEXT DEFAULT (datetime('now')),
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_tpapp_status ON training_partner_applications(status);
CREATE TABLE IF NOT EXISTS training_partner_application_documents (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  application_id INTEGER NOT NULL,
  doc_kind TEXT DEFAULT 'supporting',        -- accreditation | company_profile | curriculum | supporting
  filename TEXT, mime TEXT, size_bytes INTEGER, storage_ref TEXT, sha256 TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_tpappdoc_app ON training_partner_application_documents(application_id);

-- ===== reusable notification ledger: one row per notification attempt on any channel (email now;
-- sms / in_app are seams for later). Recipients/sender/enable live in site_settings, never hardcoded. =====
CREATE TABLE IF NOT EXISTS notification_history (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  channel TEXT NOT NULL DEFAULT 'email',     -- email | sms | in_app
  recipient TEXT, subject TEXT,
  status TEXT,                               -- sent | console | failed | skipped | disabled
  related_type TEXT, related_id INTEGER,     -- e.g. honorary_application / 42
  created_at TEXT DEFAULT (datetime('now'))
);
-- ===== SEO: managed path redirects (Admin → SEO → Redirects). from_path is an absolute site path
-- ('/old-page.html'); to_url is a path or absolute URL. Applied server-side before page serving;
-- chains are rejected at write time so every redirect is a single hop. =====
CREATE TABLE IF NOT EXISTS seo_redirects (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  from_path TEXT NOT NULL,
  to_url TEXT NOT NULL,
  status INTEGER DEFAULT 301,
  active INTEGER DEFAULT 1,
  note TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_seo_redirect_from ON seo_redirects(from_path);

-- ===== First-party analytics (Admin -> Analytics): one row per page view / business event.
-- Cookieless + privacy-first: 'visitor' is a non-reversible daily-rotating hash (never a raw IP);
-- country only from a trusted CDN geo header; attribution copied from the pci_attr cookie. =====
CREATE TABLE IF NOT EXISTS analytics_events (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  event TEXT NOT NULL,
  path TEXT, visitor TEXT, user_id INTEGER,
  country TEXT, device TEXT, browser TEXT,
  utm_source TEXT, utm_medium TEXT, utm_campaign TEXT, referrer TEXT, landing TEXT,
  value DECIMAL(12,2), currency TEXT, detail TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);
CREATE INDEX IF NOT EXISTS ix_analytics_event_time ON analytics_events(event, created_at);
CREATE INDEX IF NOT EXISTS ix_analytics_time ON analytics_events(created_at);

-- ===== SEO: search-engine submission ledger (IndexNow now; others later). One row per attempt. =====
CREATE TABLE IF NOT EXISTS seo_submissions (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  engine TEXT NOT NULL,                      -- indexnow | google | bing
  url_count INTEGER DEFAULT 0,
  status TEXT,                               -- submitted | failed
  detail TEXT,
  created_at TEXT DEFAULT (datetime('now'))
);

-- ===== Public-website internationalisation: one human-authored translation per captured text
-- region, per language. scope: 'p' page block (slug + block_key) | 'g' shared/global element (gkey)
-- | 'nav' navigation label (keyed by its English text) | 'meta' page title/description. English is
-- the source and default; with lang=en nothing is injected and pages are served byte-identical. =====
CREATE TABLE IF NOT EXISTS content_i18n (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  lang TEXT NOT NULL,                         -- ko | ar | es | fr | zh | ru
  scope TEXT NOT NULL,                        -- p | g | nav | meta
  slug TEXT NOT NULL DEFAULT '',              -- page slug for p/meta; '' for g/nav
  ckey TEXT NOT NULL,                         -- block_key (t:…) | gkey (g:…) | English nav label | title|meta
  cvalue TEXT,
  updated_at TEXT DEFAULT (datetime('now'))
);
CREATE UNIQUE INDEX IF NOT EXISTS ux_content_i18n ON content_i18n(lang, scope, slug, ckey);
CREATE INDEX IF NOT EXISTS ix_content_i18n_lang ON content_i18n(lang);

-- Configurable notification settings (owner-editable in Admin → Settings; never hardcoded).
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_enabled','1');
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_admin_email','');
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_ack_subject','');
INSERT OR IGNORE INTO site_settings(skey,svalue) VALUES ('notify_honorary_admin_subject','');
