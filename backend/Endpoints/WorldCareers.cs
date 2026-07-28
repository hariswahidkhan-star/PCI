using System.Text.Json;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// PCI World Careers — the write path (docs/pciworld/CCP_PHASE4_DESIGN.md §4, §5, §6, §8).
///
/// Public READING is server-rendered elsewhere; everything here is authenticated JSON, and it is
/// split three ways by who is acting:
///
///   employer members   /api/world/careers/employers/…   a Passport account joined to a tenant
///   applicants         /api/world/careers/postings/…/apply, /api/world/careers/my/…
///   World admins       /api/world-admin/careers/…       verification and moderation
///
/// THE ONE RULE THIS FILE EXISTS TO ENFORCE: an unverified employer publishes nothing, and a
/// non-consented employer reads nothing. A fake employer is not a spammer — it is a data-harvesting
/// attack whose prize is a pile of CVs, and once those are exfiltrated no takedown recovers them
/// (§4.3). So verification is checked AT THE TRANSITION (inside the publish transaction) and AGAIN
/// AT EVERY READ, independently. Neither check is a UI guard, and neither trusts the other.
///
/// SUSPENSION BITES MID-SESSION. Every employer-side request re-reads the employer row through
/// <see cref="CareersEmployers.MayActFor"/>; nothing about the tenant is cached in the session. A
/// fake employer discovered an hour after verification loses access to applicant data at the moment
/// an admin suspends it, not at the member's next login.
///
/// WHAT IS DELIBERATELY ABSENT: any scoring, ranking, auto-shortlisting or auto-rejection of
/// candidates (§10.7). Not disabled — absent. There is no code path to misconfigure.
/// </summary>
public static class WorldCareers
{
    /// The purpose string on a consent row. Named and narrow: the applicant consents to THIS
    /// application being disclosed to THIS employer for recruiting, not to a marketplace-wide
    /// blanket. Widening it later requires a new policy version and a fresh grant.
    const string ConsentPurpose = "employer_recruiting";

    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log)
    {
        bool Enabled() => Settings.Str(db, "pciworld_careers_enabled", "0") == "1";
        IResult Off() => Results.NotFound();

        void Audit(long? adminId, string action, string? detail)
        {
            try { db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)", adminId, action, detail); }
            catch { /* auditing must never break the action it records */ }
        }

        // Applicant/employer-member identity. There is no anonymous path anywhere in this file:
        // an anonymous application cannot carry attributable, withdrawable consent (§5.1).
        WorldAccount.UserCtx? Who(HttpContext ctx) => WorldAccount.FromReq(ctx.Request, db);

        IResult? AdminGate(HttpContext ctx, string action, out WorldAdmin.Ctx? adm)
        {
            adm = WorldAdmin.FromReq(ctx.Request, db);
            if (adm is null) return Results.Json(new { error = "no_token" }, statusCode: 401);
            if (!WorldRbac.Allowed(adm.Role, action))
                return Results.Json(new { error = "forbidden", required = action }, statusCode: 403);
            return null;
        }

        // ── Employer tenancy ───────────────────────────────────────────────────────────────────

        app.MapPost("/api/world/careers/employers", async (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var b = await H.Body(ctx.Request);
            var name = Str(b, "name");
            if (string.IsNullOrWhiteSpace(name) || name!.Trim().Length < 2)
                return Results.Json(new { error = "bad_name" }, statusCode: 400);

            var r = CareersEmployers.Create(db, user.Id, name.Trim(), Str(b, "website"), Str(b, "slug"));
            if (!r.Ok) return Results.Json(new { error = r.Reason }, statusCode: r.Reason == "slug_taken" ? 409 : 400);
            Audit(null, "careers.employer.create", $"employer={r.EmployerId} by user={user.Id}");
            // 'draft', always. An employer may never be created verified by any route — verification
            // is an assertion PCI makes, not a field a tenant fills in (§4.2).
            return Results.Json(new { ok = true, employer_id = r.EmployerId, slug = r.Slug, state = "draft" });
        });

        app.MapGet("/api/world/careers/employers", (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            // Scoped by membership, not filtered in the caller. A list endpoint that returns
            // everything and trims afterwards is one refactor away from leaking every tenant.
            var rows = db.Query(
                @"SELECT e.id,e.slug,e.name,e.state,e.website,e.version,m.role
                  FROM pciworld_employers e
                  JOIN pciworld_employer_members m ON m.employer_id = e.id
                  WHERE m.user_id=? ORDER BY e.name", user.Id);
            return Results.Json(new { employers = rows.Select(e => new
            {
                id = H.L(e["id"]), slug = H.Str(e["slug"]), name = H.Str(e["name"]),
                state = H.Str(e["state"]), website = H.Str(e["website"]),
                version = H.L(e["version"]), role = H.Str(e["role"]),
                // Told plainly rather than left to be inferred from `state`, so an employer member
                // knows why the publish button refuses before they write a posting.
                may_publish = H.Str(e["state"]) == "verified",
            }) });
        });

        app.MapPost("/api/world/careers/employers/{id:long}/request-verification", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            if (CareersEmployers.RoleOf(db, id, user.Id) != "owner")
                return Results.Json(new { error = "owner_only" }, statusCode: 403);

            var e = db.QueryOne("SELECT state,version FROM pciworld_employers WHERE id=?", id);
            if (e is null) return Results.NotFound();
            var from = H.Str(e["state"]) ?? "draft";
            if (!CareersState.EmployerTransitionAllowed(from, "pending_verification"))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);

            // Written HERE rather than through CareersEmployers.SetState, and that is deliberate.
            // SetState is the admin route in its entirety — it demands an admin row granting
            // careers.verify, so there is no argument value that lets a tenant move its own state
            // through it. Asking for review is a different act from asserting a verdict: this one
            // never touches verified_at or verified_by_admin_id, which stay empty until a person
            // decides (§4.2). The version predicate is still carried, so a concurrent admin
            // suspension is not overwritten by a request that raced it.
            var moved = db.Execute(
                @"UPDATE pciworld_employers
                     SET state='pending_verification', updated_at=datetime('now'), version=version+1
                   WHERE id=? AND state=? AND version=?",
                id, from, H.L(e["version"]));
            if (moved == 0) return Results.Json(new { error = "conflict" }, statusCode: 409);
            Audit(null, "careers.employer.verification_requested", $"employer={id} by user={user.Id}");
            return Results.Json(new
            {
                ok = true, state = "pending_verification",
                // Honest about what this does and does not mean — the same rule Phase 1 applies to
                // its deterrents and Phase 2 to its scanners.
                notice = "Your organisation is queued for review by a person. Verification checks who is "
                       + "behind a posting; it is not automatic and there is no guaranteed timescale.",
            });
        });

        app.MapPost("/api/world/careers/employers/{id:long}/members", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var b = await H.Body(ctx.Request);
            var email = Str(b, "email");
            var role = Str(b, "role") ?? "member";
            if (string.IsNullOrWhiteSpace(email)) return Results.Json(new { error = "bad_email" }, statusCode: 400);

            var target = db.QueryOne("SELECT id FROM pciworld_users WHERE lower(email)=lower(?)", email!.Trim());
            // A deliberately identical answer whether the account exists or not: this endpoint must
            // not become an oracle telling any employer which emails hold PCI World accounts.
            if (target is null) return Results.Json(new { error = "no_such_account" }, statusCode: 404);

            var failed = CareersEmployers.AddMember(db, id, user.Id, H.L(target["id"]), role);
            if (failed is not null) return Results.Json(new { error = failed }, statusCode: failed == "owner_only" ? 403 : 400);
            Audit(null, "careers.employer.member_added", $"employer={id} user={H.L(target["id"])} role={role} by={user.Id}");
            return Results.Json(new { ok = true });
        });

        app.MapDelete("/api/world/careers/employers/{id:long}/members/{userId:long}", (HttpContext ctx, long id, long userId) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var failed = CareersEmployers.RemoveMember(db, id, user.Id, userId);
            if (failed is not null) return Results.Json(new { error = failed }, statusCode: failed == "owner_only" ? 403 : 400);
            Audit(null, "careers.employer.member_removed", $"employer={id} user={userId} by={user.Id}");
            return Results.Json(new { ok = true });
        });

        // ── Postings: the employer's working copy ──────────────────────────────────────────────

        app.MapPost("/api/world/careers/employers/{id:long}/postings", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            // Drafting needs MEMBERSHIP only, not verification. An unverified employer may draft
            // everything and publish nothing — "draft anything, publish nothing" (§4.3).
            if (!CareersEmployers.IsMember(db, id, user.Id))
                return Results.Json(new { error = "not_a_member" }, statusCode: 403);

            var b = await H.Body(ctx.Request);
            var title = Str(b, "title");
            if (string.IsNullOrWhiteSpace(title) || title!.Trim().Length < 4)
                return Results.Json(new { error = "bad_title" }, statusCode: 400);

            // Per-employer posting budget (§8): bounds listing spam by a verified-then-rogue tenant.
            var live = db.Scalar<long>(
                "SELECT COUNT(*) FROM pciworld_job_postings WHERE employer_id=? AND state IN ('draft','published')", id);
            if (live >= 200) return Results.Json(new { error = "posting_budget_exhausted" }, statusCode: 429);

            var slug = Slugify(Str(b, "slug") ?? title);
            if (slug.Length == 0) return Results.Json(new { error = "bad_slug" }, statusCode: 400);
            if (db.Scalar<long>("SELECT COUNT(*) FROM pciworld_job_postings WHERE employer_id=? AND slug=?", id, slug) > 0)
                return Results.Json(new { error = "slug_taken" }, statusCode: 409);

            var (minMinor, maxMinor, currency, moneyError) = Money(b);
            if (moneyError is not null) return Results.Json(new { error = moneyError }, statusCode: 400);

            var postingId = db.ExecuteReturningId(
                @"INSERT INTO pciworld_job_postings
                    (employer_id,slug,title,description,location,employment_type,
                     salary_min_minor,salary_max_minor,currency,state,closes_at)
                  VALUES(?,?,?,?,?,?,?,?,?,'draft',?)",
                id, slug, title.Trim(), Str(b, "description"), Str(b, "location"),
                Str(b, "employment_type"), minMinor, maxMinor, currency, Str(b, "closes_at"));

            Audit(null, "careers.posting.create", $"posting={postingId} employer={id} by={user.Id}");
            return Results.Json(new { ok = true, posting_id = postingId, slug, state = "draft" });
        });

        app.MapPost("/api/world/careers/postings/{id:long}/questions", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var p = db.QueryOne("SELECT employer_id FROM pciworld_job_postings WHERE id=?", id);
            if (p is null) return Results.NotFound();
            if (!CareersEmployers.IsMember(db, H.L(p["employer_id"]), user.Id))
                return Results.Json(new { error = "not_a_member" }, statusCode: 403);

            var b = await H.Body(ctx.Request);
            var kind = (Str(b, "kind") ?? "text").ToLowerInvariant();
            // Three kinds ship. The legacy 9-type taxonomy is the design ceiling, grown per real
            // need — and `consent` is deliberately NOT among them, because here consent is a
            // first-class record, not a checkbox an employer words themselves (§1).
            if (kind is not ("text" or "boolean" or "choice"))
                return Results.Json(new { error = "bad_kind" }, statusCode: 400);
            var prompt = Str(b, "prompt");
            if (string.IsNullOrWhiteSpace(prompt)) return Results.Json(new { error = "bad_prompt" }, statusCode: 400);

            var qid = db.ExecuteReturningId(
                "INSERT INTO pciworld_job_questions(posting_id,sort,kind,prompt,required) VALUES(?,?,?,?,?)",
                id, (int)(Lng(b, "sort") ?? 100), kind, prompt!.Trim(),
                Bln(b, "required") ? 1 : 0);
            return Results.Json(new { ok = true, question_id = qid });
        });

        app.MapPost("/api/world/careers/postings/{id:long}/publish", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var p = db.QueryOne("SELECT employer_id,state,version FROM pciworld_job_postings WHERE id=?", id);
            if (p is null) return Results.NotFound();
            var employerId = H.L(p["employer_id"]);
            if (!CareersEmployers.IsMember(db, employerId, user.Id))
                return Results.Json(new { error = "not_a_member" }, statusCode: 403);

            var from = H.Str(p["state"]) ?? "draft";
            if (!CareersState.PostingTransitionAllowed(from, "published"))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);

            // CHECK 1 OF 2 (§4.3). The employer row is re-read INSIDE the same statement that flips
            // the posting, as a predicate on the UPDATE — not as an `if` above it. An `if` and an
            // UPDATE are two moments, and a suspension landing between them would publish a job for
            // an employer that is no longer verified.
            var changed = db.Execute(
                @"UPDATE pciworld_job_postings
                     SET state='published', published_at=datetime('now'),
                         updated_at=datetime('now'), version=version+1
                   WHERE id=? AND state=? AND version=?
                     AND EXISTS(SELECT 1 FROM pciworld_employers e
                                 WHERE e.id = pciworld_job_postings.employer_id AND e.state='verified')",
                id, from, H.L(p["version"]));

            if (changed == 0)
            {
                // Distinguish the two causes for the operator's sake, by re-reading — but only
                // AFTER the atomic attempt has already failed, so this read decides nothing.
                var verified = CareersEmployers.MayPublish(db, employerId);
                return Results.Json(new
                {
                    error = verified ? "version_conflict" : "employer_not_verified",
                    notice = verified ? null
                        : "Only a verified organisation can publish a job. Your draft is saved and "
                        + "stays private until verification completes.",
                }, statusCode: verified ? 409 : 403);
            }

            Audit(null, "careers.posting.publish", $"posting={id} employer={employerId} by={user.Id}");
            return Results.Json(new { ok = true, state = "published" });
        });

        app.MapPost("/api/world/careers/postings/{id:long}/close", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var p = db.QueryOne("SELECT employer_id,state,version FROM pciworld_job_postings WHERE id=?", id);
            if (p is null) return Results.NotFound();
            if (!CareersEmployers.IsMember(db, H.L(p["employer_id"]), user.Id))
                return Results.Json(new { error = "not_a_member" }, statusCode: 403);
            var from = H.Str(p["state"]) ?? "draft";
            if (!CareersState.PostingTransitionAllowed(from, "closed"))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);
            db.Execute(
                "UPDATE pciworld_job_postings SET state='closed', updated_at=datetime('now'), version=version+1 WHERE id=? AND version=?",
                id, H.L(p["version"]));
            Audit(null, "careers.posting.close", $"posting={id} by={user.Id}");
            return Results.Json(new { ok = true, state = "closed" });
        });

        // ── Applying: the record, frozen at submission ─────────────────────────────────────────

        app.MapPost("/api/world/careers/postings/{id:long}/apply", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            // 401, and it is the ONLY answer for an anonymous caller. A community guest ticket is
            // not a Passport: consent that cannot later be proven by the person who gave it is
            // legally hollow, and an immutable "who applied" is worth nothing if "who" is a string
            // anyone can type (§5.1).
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var p = db.QueryOne(
                @"SELECT p.id,p.employer_id,p.state,p.closes_at,e.state AS employer_state
                  FROM pciworld_job_postings p JOIN pciworld_employers e ON e.id=p.employer_id
                  WHERE p.id=?", id);
            if (p is null) return Results.NotFound();

            // The SAME honesty predicate the public page and the sitemap use. The page saying
            // "closed" is courtesy; this is the control — an apply POST is refused regardless of
            // what any cached page said (§7).
            if (!CareersState.IsLive(H.Str(p["state"]) ?? "", H.Str(p["employer_state"]) ?? "",
                                     H.Str(p["closes_at"]), H.StampInMinutes(0)))
                return Results.Json(new { error = "not_accepting_applications" }, statusCode: 409);

            var b = await H.Body(ctx.Request);

            // Consent is explicit, and refusing it refuses the application. There is no way to apply
            // without disclosing, so a silent default would be a consent nobody gave.
            if (!Bln(b, "consent"))
                return Results.Json(new { error = "consent_required" }, statusCode: 400);
            var policyVersion = Settings.Str(db, "pciworld_careers_policy_version", "");
            if (string.IsNullOrWhiteSpace(policyVersion))
                // Fail closed. `policy_version` must pin words the applicant actually saw; stamping
                // a placeholder would record a consent to nothing (CCP-P4-014).
                return Results.Json(new
                {
                    error = "consent_policy_unavailable",
                    notice = "Applications are not open yet: the applicant privacy notice for this "
                           + "marketplace has not been published.",
                }, statusCode: 503);

            // Per-account daily budget (§8). Bounds spam-applying without an IP heuristic that a
            // shared office would trip.
            var today = db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_applications
                   WHERE applicant_user_id=? AND submitted_at IS NOT NULL AND submitted_at > ?",
                user.Id, H.StampInMinutes(-1440));
            if (today >= 40) return Results.Json(new { error = "rate_limited" }, statusCode: 429);

            // The CV. Sniffed by MAGIC BYTES — a renamed .exe fails whatever the filename claims —
            // then put through the platform's single fail-closed malware policy.
            byte[]? cvBytes = null; string? cvMime = null;
            var cvName = Str(b, "cv_name");
            if (Str(b, "cv_base64") is { Length: > 0 } cv64)
            {
                try { cvBytes = Convert.FromBase64String(cv64); }
                catch (FormatException) { return Results.Json(new { error = "bad_cv_encoding" }, statusCode: 400); }
                if (cvBytes.Length > CareersState.MaxCvBytes)
                    return Results.Json(new { error = "cv_too_large" }, statusCode: 400);
                cvMime = CareersState.SniffCvMime(cvBytes, cvName ?? "");
                if (cvMime is null) return Results.Json(new { error = "cv_type_not_allowed" }, statusCode: 400);
                var scan = UploadScan.Scan(cvBytes, cvMime);
                if (!scan.Clean) return Results.Json(new { error = "cv_rejected", reason = scan.Reason }, statusCode: 400);
            }

            var answers = b.TryGetValue("answers", out var answersEl) ? answersEl : default;
            var employerId = H.L(p["employer_id"]);

            // Required answers are validated BEFORE the transaction opens, and that ordering is a
            // fix, not a style choice. An early `return` from inside db.Transaction's lambda COMMITS
            // — the rollback only happens on an exception — so validating in there left a
            // half-written application row behind on every refusal, and the next attempt by the same
            // person was rejected as a duplicate of the record their refusal had created. Nothing
            // below this point can fail for a reason a read could have caught.
            var questions = db.Query(
                "SELECT id,prompt,required FROM pciworld_job_questions WHERE posting_id=? ORDER BY sort,id", id);
            string? Answer(long qid) =>
                answers.ValueKind == JsonValueKind.Object && answers.TryGetProperty(qid.ToString(), out var v)
                    ? (v.ValueKind == JsonValueKind.String ? v.GetString() : v.ToString())
                    : null;
            foreach (var q in questions)
                if (H.B(q["required"]) && string.IsNullOrWhiteSpace(Answer(H.L(q["id"]))))
                    return Results.Json(new { error = "answer_required" }, statusCode: 400);

            long applicationId = 0;
            string? failure = null;
            try
            {
            db.Transaction(() =>
            {
                // Reapplying is a state change on the row the person already owns — the unique index
                // has no escape clause, so a withdrawn application holds the slot and an employer can
                // never hold two versions of one candidacy (§5.2).
                var existing = db.QueryOne(
                    "SELECT id,state FROM pciworld_applications WHERE posting_id=? AND applicant_user_id=?",
                    id, user.Id);
                // THROW, never return. A bare `return` here would commit the transaction — see the
                // note above the pre-flight validation. This runs before any write, so the throw
                // costs nothing but the rollback.
                if (existing is not null && H.Str(existing["state"]) is not ("draft" or "withdrawn"))
                    throw new ApplyRefused("already_applied");

                string? cvRef = null, cvSha = null;
                if (cvBytes is not null)
                {
                    // Private, content-addressed, and NOT in the sweep-protected category list: a
                    // marketplace holding every CV for ever is a liability, not an archive (§6).
                    var stored = Storage.Put(cvBytes, cvMime!, "world-cv");
                    cvRef = stored.Reference;
                    // The store already content-addresses; reuse its digest rather than hashing the
                    // same bytes twice and inviting the two values to disagree.
                    cvSha = stored.Sha256;
                }

                if (existing is null)
                {
                    applicationId = db.ExecuteReturningId(
                        @"INSERT INTO pciworld_applications
                            (posting_id,applicant_user_id,state,cv_ref,cv_sha256,cv_name,cv_mime,submitted_at)
                          VALUES(?,?,'submitted',?,?,?,?,datetime('now'))",
                        id, user.Id, cvRef, cvSha, cvName, cvMime);
                }
                else
                {
                    applicationId = H.L(existing["id"]);
                    db.Execute(
                        @"UPDATE pciworld_applications
                             SET state='submitted', cv_ref=?, cv_sha256=?, cv_name=?, cv_mime=?,
                                 submitted_at=datetime('now'), withdrawn_at=NULL,
                                 updated_at=datetime('now'), version=version+1
                           WHERE id=?",
                        cvRef, cvSha, cvName, cvMime, applicationId);
                    // The previous attempt's answers and consent belong to the previous submission;
                    // this one freezes its own. Superseding them here keeps "what did this person
                    // say, to which prompts" a single unambiguous answer.
                    db.Execute("DELETE FROM pciworld_application_answers WHERE application_id=?", applicationId);
                }

                // The answers freeze WITH their prompts. question_id stays as provenance only: the
                // moment a join back to pciworld_job_questions becomes the source of truth, in-place
                // mutability is back through the side door (§5.2).
                foreach (var q in questions)
                    db.Execute(
                        @"INSERT INTO pciworld_application_answers
                            (application_id,question_id,prompt_snapshot,answer_snapshot) VALUES(?,?,?,?)",
                        applicationId, H.L(q["id"]), H.Str(q["prompt"]), Answer(H.L(q["id"])));

                // A fresh grant per submission, pinned to the policy words actually shown. Applying
                // to two employers writes two independent consents; there is no blanket.
                db.Execute(
                    @"INSERT INTO pciworld_application_consents
                        (application_id,employer_id,purpose,granted_at,policy_version)
                      VALUES(?,?,?,datetime('now'),?)",
                    applicationId, employerId, ConsentPurpose, policyVersion);

                db.Execute(
                    @"INSERT INTO pciworld_application_events
                        (application_id,event,from_state,to_state,actor_kind,actor_id)
                      VALUES(?,'submitted',?, 'submitted','applicant',?)",
                    applicationId, existing is null ? null : H.Str(existing["state"]), user.Id);
            });
            }
            catch (ApplyRefused ex) { failure = ex.Reason; }

            if (failure is not null)
                return Results.Json(new { error = failure }, statusCode: failure == "already_applied" ? 409 : 400);

            return Results.Json(new
            {
                ok = true, application_id = applicationId,
                notice = "Your application has been sent. What you wrote, the CV you attached and the "
                       + "consent you gave are recorded as they were at this moment and will not change.",
            });
        });

        app.MapGet("/api/world/careers/my/applications", (HttpContext ctx) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var rows = db.Query(
                @"SELECT a.id,a.state,a.submitted_at,a.withdrawn_at,a.cv_name,
                         p.title,p.slug,e.name AS employer_name,
                         c.granted_at,c.withdrawn_at AS consent_withdrawn_at,c.policy_version
                  FROM pciworld_applications a
                  JOIN pciworld_job_postings p ON p.id=a.posting_id
                  JOIN pciworld_employers e ON e.id=p.employer_id
                  LEFT JOIN pciworld_application_consents c ON c.application_id=a.id
                  WHERE a.applicant_user_id=? ORDER BY a.id DESC", user.Id);
            return Results.Json(new { applications = rows.Select(a => new
            {
                id = H.L(a["id"]), state = H.Str(a["state"]),
                title = H.Str(a["title"]), employer = H.Str(a["employer_name"]),
                submitted_at = H.Str(a["submitted_at"]), withdrawn_at = H.Str(a["withdrawn_at"]),
                cv_name = H.Str(a["cv_name"]),
                // The consent record is shown to the person who gave it, including after they
                // withdraw it — the row survives withdrawal precisely so this stays answerable.
                consent = new
                {
                    granted_at = H.Str(a["granted_at"]),
                    withdrawn_at = H.Str(a["consent_withdrawn_at"]),
                    policy_version = H.Str(a["policy_version"]),
                    active = !string.IsNullOrEmpty(H.Str(a["granted_at"])) && string.IsNullOrEmpty(H.Str(a["consent_withdrawn_at"])),
                },
            }) });
        });

        app.MapPost("/api/world/careers/applications/{id:long}/withdraw", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var a = db.QueryOne("SELECT state,version FROM pciworld_applications WHERE id=? AND applicant_user_id=?", id, user.Id);
            if (a is null) return Results.NotFound();
            var from = H.Str(a["state"]) ?? "";
            if (!CareersState.ApplicationTransitionAllowed(from, "withdrawn"))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);
            db.Transaction(() =>
            {
                db.Execute(
                    @"UPDATE pciworld_applications SET state='withdrawn', withdrawn_at=datetime('now'),
                             updated_at=datetime('now'), version=version+1 WHERE id=? AND version=?",
                    id, H.L(a["version"]));
                db.Execute(
                    @"INSERT INTO pciworld_application_events
                        (application_id,event,from_state,to_state,actor_kind,actor_id)
                      VALUES(?,'withdrawn',?, 'withdrawn','applicant',?)", id, from, user.Id);
            });
            return Results.Json(new { ok = true, state = "withdrawn" });
        });

        app.MapPost("/api/world/careers/applications/{id:long}/consent/withdraw", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var a = db.QueryOne("SELECT id FROM pciworld_applications WHERE id=? AND applicant_user_id=?", id, user.Id);
            if (a is null) return Results.NotFound();
            // A TIMESTAMP, NEVER A DELETE (§5.3). From this instant the employer's access fails
            // condition (c) — but "was there consent on the day the employer read this?" must stay
            // answerable, because that is exactly the dispute the record exists to settle.
            db.Execute(
                "UPDATE pciworld_application_consents SET withdrawn_at=datetime('now') WHERE application_id=? AND withdrawn_at IS NULL",
                id);
            db.Execute(
                @"INSERT INTO pciworld_application_events
                    (application_id,event,actor_kind,actor_id,note)
                  VALUES(?,'note','applicant',?,'consent withdrawn')", id, user.Id);
            return Results.Json(new
            {
                ok = true,
                notice = "Consent withdrawn. The employer can no longer open this application or your CV. "
                       + "The record that you applied, and that you consented at the time, is kept — "
                       + "withdrawing consent is not the same as erasure.",
            });
        });

        // ── The employer's view of applicants, and the CV door ─────────────────────────────────

        app.MapGet("/api/world/careers/postings/{id:long}/applications", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var p = db.QueryOne("SELECT employer_id FROM pciworld_job_postings WHERE id=?", id);
            if (p is null) return Results.NotFound();
            var employerId = H.L(p["employer_id"]);
            // Conditions (a), (b) and (d) of §6 in one call, re-read per request so a suspension
            // takes effect mid-session.
            if (!CareersEmployers.MayActFor(db, employerId, user.Id))
                return Results.Json(new { error = "not_permitted" }, statusCode: 403);

            // Condition (c) is the consent predicate, applied IN THE QUERY. Filtering afterwards
            // would mean the withdrawn rows were already in memory, one logging change away from
            // being disclosed.
            var rows = db.Query(
                @"SELECT a.id,a.state,a.submitted_at,a.cv_name,u.display_name
                  FROM pciworld_applications a
                  JOIN pciworld_users u ON u.id=a.applicant_user_id
                  JOIN pciworld_application_consents c
                    ON c.application_id=a.id AND c.employer_id=? AND c.granted_at IS NOT NULL AND c.withdrawn_at IS NULL
                  WHERE a.posting_id=? AND a.state<>'draft' AND a.state<>'withdrawn'
                  ORDER BY a.submitted_at DESC", employerId, id);
            return Results.Json(new { applications = rows.Select(a => new
            {
                id = H.L(a["id"]), state = H.Str(a["state"]),
                submitted_at = H.Str(a["submitted_at"]),
                applicant = H.Str(a["display_name"]),
                has_cv = !string.IsNullOrEmpty(H.Str(a["cv_name"])),
            }) });
        });

        app.MapGet("/api/world/careers/applications/{id:long}/cv", (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);

            var a = db.QueryOne(
                @"SELECT a.id,a.cv_ref,a.cv_name,a.cv_mime,p.employer_id
                  FROM pciworld_applications a JOIN pciworld_job_postings p ON p.id=a.posting_id
                  WHERE a.id=?", id);
            if (a is null) return Results.NotFound();
            var employerId = H.L(a["employer_id"]);

            void LogAccess(bool allowed, string? reason) =>
                db.Execute(
                    @"INSERT INTO pciworld_cv_access_log
                        (application_id,employer_id,actor_kind,actor_id,allowed,refused_reason)
                      VALUES(?,?,'employer',?,?,?)",
                    id, employerId, user.Id, allowed ? 1 : 0, reason);

            // All four conditions of §6, each checked here on THIS request. Refusals are logged as
            // well as grants: a run of refusals is what probing ids looks like, and logging only
            // successes would hide precisely that (§8).
            if (!CareersEmployers.MayActFor(db, employerId, user.Id))
            {
                LogAccess(false, "not_permitted");
                return Results.Json(new { error = "not_permitted" }, statusCode: 403);
            }
            var consented = db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_application_consents
                   WHERE application_id=? AND employer_id=? AND granted_at IS NOT NULL AND withdrawn_at IS NULL",
                id, employerId) > 0;
            if (!consented)
            {
                LogAccess(false, "consent_withdrawn");
                return Results.Json(new { error = "consent_withdrawn" }, statusCode: 403);
            }

            // Per-member hourly download budget (§8): a legitimate reviewer never reaches it; a bulk
            // pull hits it long before it finishes.
            var recent = db.Scalar<long>(
                @"SELECT COUNT(*) FROM pciworld_cv_access_log
                   WHERE actor_kind='employer' AND actor_id=? AND allowed=1 AND created_at > ?",
                user.Id, H.StampInMinutes(-60));
            if (recent >= 60)
            {
                LogAccess(false, "rate_limited");
                return Results.Json(new { error = "rate_limited" }, statusCode: 429);
            }

            var got = Storage.Get(H.Str(a["cv_ref"]));
            if (got is null) { LogAccess(false, "not_found"); return Results.NotFound(); }

            LogAccess(true, null);
            // ATTACHMENT, never inline. An inline PDF opens in the browser's viewer, which is an
            // execution surface this feature has no need of.
            ctx.Response.Headers["Content-Disposition"] =
                $"attachment; filename=\"{DocStore.SafeName(H.Str(a["cv_name"]), ExtFor(H.Str(a["cv_mime"])))}\"";
            return Results.File(got.Value.bytes!, H.Str(a["cv_mime"]) ?? got.Value.mime);
        });

        app.MapPost("/api/world/careers/applications/{id:long}/decision", async (HttpContext ctx, long id) =>
        {
            if (!Enabled()) return Off();
            var user = Who(ctx);
            if (user is null) return Results.Json(new { error = "no_session" }, statusCode: 401);
            var a = db.QueryOne(
                @"SELECT a.state,a.version,p.employer_id FROM pciworld_applications a
                  JOIN pciworld_job_postings p ON p.id=a.posting_id WHERE a.id=?", id);
            if (a is null) return Results.NotFound();
            var employerId = H.L(a["employer_id"]);
            if (!CareersEmployers.MayActFor(db, employerId, user.Id))
                return Results.Json(new { error = "not_permitted" }, statusCode: 403);

            var b = await H.Body(ctx.Request);
            var to = (Str(b, "decision") ?? "").ToLowerInvariant();
            if (to is not ("shortlisted" or "rejected"))
                return Results.Json(new { error = "bad_decision" }, statusCode: 400);
            var from = H.Str(a["state"]) ?? "";
            if (!CareersState.ApplicationTransitionAllowed(from, to))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);

            db.Transaction(() =>
            {
                db.Execute(
                    "UPDATE pciworld_applications SET state=?, updated_at=datetime('now'), version=version+1 WHERE id=? AND version=?",
                    to, id, H.L(a["version"]));
                // The note is an EVENT, never a column that the next note overwrites. Main-PCI still
                // overwrites admin_note in place; a note that replaces the previous one destroys the
                // reason a candidate was moved, which is exactly what a dispute asks for (§5.2).
                db.Execute(
                    @"INSERT INTO pciworld_application_events
                        (application_id,event,from_state,to_state,actor_kind,actor_id,note)
                      VALUES(?,?,?,?,'employer',?,?)",
                    id, to, from, to, user.Id, Str(b, "note"));
            });
            return Results.Json(new { ok = true, state = to });
        });

        // ── World admin: verification is the assertion PCI makes ───────────────────────────────

        app.MapGet("/api/world-admin/careers/employers", (HttpContext ctx) =>
        {
            if (AdminGate(ctx, "careers.read", out _) is { } deny) return deny;
            var rows = db.Query(
                @"SELECT id,slug,name,website,state,verified_at,verified_by_admin_id,version
                  FROM pciworld_employers ORDER BY
                    CASE state WHEN 'pending_verification' THEN 0 ELSE 1 END, id DESC");
            return Results.Json(new { employers = rows.Select(e => new
            {
                id = H.L(e["id"]), slug = H.Str(e["slug"]), name = H.Str(e["name"]),
                website = H.Str(e["website"]), state = H.Str(e["state"]),
                verified_at = H.Str(e["verified_at"]), verified_by = H.L(e["verified_by_admin_id"]),
                version = H.L(e["version"]),
            }) });
        });

        app.MapPost("/api/world-admin/careers/employers/{id:long}/state", async (HttpContext ctx, long id) =>
        {
            // Verification is gated on its OWN permission, not on content moderation: deciding a
            // company is real is an evidence judgement, and bundling it with taking a posting down
            // would let every content moderator mint the assertion candidates rely on (§4.2).
            if (AdminGate(ctx, "careers.verify", out var adm) is { } deny) return deny;
            var b = await H.Body(ctx.Request);
            var to = (Str(b, "state") ?? "").ToLowerInvariant();
            if (to is not ("verified" or "suspended" or "draft" or "pending_verification"))
                return Results.Json(new { error = "bad_state" }, statusCode: 400);

            var e = db.QueryOne("SELECT state,version FROM pciworld_employers WHERE id=?", id);
            if (e is null) return Results.NotFound();
            var from = H.Str(e["state"]) ?? "draft";
            if (!CareersState.EmployerTransitionAllowed(from, to))
                return Results.Json(new { error = "bad_transition", from }, statusCode: 400);

            var failed = CareersEmployers.SetState(db, id, to, adm!.Id, (int)H.L(e["version"]));
            if (failed is not null)
                return Results.Json(new { error = failed }, statusCode: failed switch
                {
                    "conflict" => 409,
                    "forbidden" or "admin_required" => 403,
                    "not_found" => 404,
                    _ => 400,
                });

            // Suspension needs no second write to unpublish the tenant's surface: every public read
            // predicates on state='verified', so this one UPDATE removes the listings, the markup,
            // the sitemap entries AND the employer's access to applicant data, all at once (§4.3).
            Audit(adm.Id, $"careers.employer.{to}", $"employer={id} from={from} reason={Str(b, "reason")}");
            return Results.Json(new { ok = true, state = to });
        });
    }

    // ── helpers ────────────────────────────────────────────────────────────────────────────────

    /// Rolls the apply transaction back. db.Transaction commits on a plain return and rolls back
    /// only on an exception, so a refusal that must not leave a row behind has to throw.
    sealed class ApplyRefused(string reason) : Exception(reason)
    {
        public string Reason { get; } = reason;
    }

    // Body values arrive as JsonElement (H.Body is the house reader). Reading them through H.L /
    // H.Str directly does NOT work — Convert.ToInt64(JsonElement) throws, which surfaces as a 500
    // on the first numeric field rather than as a validation error. These three accessors are the
    // whole reason this file does not hand JsonElement to the coercion helpers.
    static string? Str(Dictionary<string, JsonElement> b, string key) =>
        b.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() : null;

    static long? Lng(Dictionary<string, JsonElement> b, string key) =>
        b.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out var l) ? l : null;

    static bool Bln(Dictionary<string, JsonElement> b, string key) =>
        b.TryGetValue(key, out var v) && v.ValueKind == JsonValueKind.True;

    /// <summary>
    /// Salary in INTEGER minor units with its currency beside it. A REAL salary rounds, and a
    /// rounded salary is a wrong salary shown to someone deciding whether to apply; an amount with
    /// no currency is not an amount at all, so publishing one without the other is refused rather
    /// than defaulted to a guess about where the employer is.
    /// </summary>
    static (long? min, long? max, string? currency, string? error) Money(Dictionary<string, JsonElement> b)
    {
        var min = Lng(b, "salary_min_minor");
        var max = Lng(b, "salary_max_minor");
        var cur = Str(b, "currency");
        if ((min is not null || max is not null) && string.IsNullOrWhiteSpace(cur))
            return (null, null, null, "currency_required");
        if (min is not null && max is not null && min > max) return (null, null, null, "salary_range_inverted");
        return (min, max, cur?.Trim().ToUpperInvariant(), null);
    }

    /// The extension the download is named with comes from the MIME WE sniffed and stored, never
    /// from the filename the applicant supplied — that is the whole point of sniffing.
    static string ExtFor(string? mime) => mime switch
    {
        "application/pdf" => "pdf",
        "application/msword" => "doc",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "docx",
        _ => "bin",
    };

    static string Slugify(string s)
    {
        var chars = s.Trim().ToLowerInvariant()
            .Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        return slug.Trim('-');
    }
}
