using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// The employer tenancy layer for PCI World Careers (docs/pciworld/CCP_PHASE4_DESIGN.md §4).
///
/// AN EMPLOYER IS A TENANT, NOT STAFF. Its people are ordinary Passport accounts joined through a
/// membership row — acting rights are rows, not a password shared around an office, and never
/// WorldRbac roles. Everything an employer-side user may see or do is scoped through one employer
/// row, which makes this file the boundary the design's §2 prohibitions lean on: "an employer reads
/// another employer's applicants" must be impossible *here*, because every caller above trusts
/// these predicates rather than re-deriving them.
///
/// VERIFICATION IS AN ATTRIBUTABLE ACT, NOT A FLAG (§4.2). `verified` is an assertion PCI makes to
/// candidates about who is behind a posting, so the row records WHO asserted it and WHEN — and only
/// a World admin holding `careers.verify` can make the assertion. An employer member can never
/// raise their own employer's state by any route through this class: members live in
/// pciworld_users, verification demands an active pciworld_admin_users row, and the two realms
/// share nothing but the integers in unrelated id sequences.
///
/// EVERY EMPLOYER-ROW WRITE CARRIES THE `version` PREDICATE (§13.6). A lost update here is not a
/// stale page title — it is a suspension silently overwritten by a concurrent edit, i.e. a
/// candidate being told a fake employer is real. Conflicts are reported, never resolved by
/// overwriting.
///
/// SUSPENSION TAKES EFFECT PER REQUEST, NOT PER LOGIN (§4.3, §8). <see cref="MayPublish"/> and
/// <see cref="MayActFor"/> re-read the employer row on every call and cache nothing, so a fake
/// employer discovered mid-session loses the *data*, not just the listing, on its very next
/// request.
/// </summary>
public static class CareersEmployers
{
    public static bool Enabled(Db db) => Settings.Bool(db, "pciworld_careers_enabled", false);

    /// <summary>Bounded because the name is rendered on a crawlable page and inside JSON-LD; an
    /// unbounded name is a layout and payload lever, not a longer name.</summary>
    public const int MaxNameLength = 200;
    public const int MaxSlugLength = 160;   // matches the column; the slug is a permanent URL segment
    public const int MaxWebsiteLength = 255;

    public sealed record Result(bool Ok, long EmployerId, string Slug, string Reason)
    {
        public static Result Rejected(string reason) => new(false, 0, "", reason);
    }

    /// <summary>
    /// Create an employer tenant. Returns a refusal as an ordinary result with a reason code —
    /// never an exception — because every branch is reachable by an authenticated stranger.
    ///
    /// The creator becomes the 'owner' member IN THE SAME TRANSACTION: an employer with nobody able
    /// to act for it is a dead row that can never be administered, only deleted. New employers
    /// start in 'draft' and there is deliberately NO way to pass a state through this signature —
    /// 'verified' exists only as the outcome of an attributable admin act (§4.2), so the INSERT
    /// hard-codes 'draft' rather than defaulting it.
    /// </summary>
    public static Result Create(Db db, long userId, string name, string? website, string? slug)
    {
        if (!Enabled(db)) return Result.Rejected("careers_disabled");

        name = (name ?? "").Trim();
        if (name.Length < 2 || name.Length > MaxNameLength) return Result.Rejected("bad_name");

        // The website is emitted to crawlers as the verified employer's `sameAs` (§7), so a value
        // that is not an absolute http(s) URL is refused at the door rather than published later.
        website = string.IsNullOrWhiteSpace(website) ? null : website.Trim();
        if (website is not null &&
            (website.Length > MaxWebsiteLength ||
             !Uri.TryCreate(website, UriKind.Absolute, out var u) ||
             (u.Scheme != Uri.UriSchemeHttp && u.Scheme != Uri.UriSchemeHttps)))
            return Result.Rejected("bad_website");

        // A caller-supplied slug is validated, never repaired: silently rewriting the address the
        // caller asked for would mint a permanent URL nobody chose. Only the derived-from-name path
        // slugifies, and a name that yields nothing usable is a refusal — not Blog.Slugify's "post"
        // fallback, which would be a meaningless permanent address for a crawlable page.
        var s = string.IsNullOrWhiteSpace(slug) ? Slugify(name) : slug.Trim().ToLowerInvariant();
        if (!ValidSlug(s)) return Result.Rejected("bad_slug");

        long id = 0;
        var reason = "created";
        db.Transaction(() =>
        {
            // Pre-checked inside the transaction for a clean reason code; the schema's UNIQUE on
            // slug remains the backstop for a race across app instances — a second creator must
            // fail loudly rather than share an address.
            if (db.QueryOne("SELECT id FROM pciworld_employers WHERE slug=?", s) is not null)
            {
                reason = "slug_taken";
                return;
            }
            id = db.ExecuteReturningId(
                "INSERT INTO pciworld_employers(slug,name,website,state) VALUES(?,?,?,'draft')",
                s, name, website);
            db.Execute(
                "INSERT INTO pciworld_employer_members(employer_id,user_id,role) VALUES(?,?,'owner')",
                id, userId);
        });
        return reason == "created" ? new Result(true, id, s, reason) : Result.Rejected(reason);
    }

    // ── Membership ────────────────────────────────────────────────────────────────────────────

    public static bool IsMember(Db db, long employerId, long userId) =>
        db.QueryOne("SELECT id FROM pciworld_employer_members WHERE employer_id=? AND user_id=?",
            employerId, userId) is not null;

    public static string? RoleOf(Db db, long employerId, long userId) =>
        db.Scalar<string>("SELECT role FROM pciworld_employer_members WHERE employer_id=? AND user_id=?",
            employerId, userId);

    /// <summary>
    /// Add a member, or change an existing member's role. Only an 'owner' OF THIS EMPLOYER may do
    /// either — the role is read with the employer_id predicate, so owning employer A grants
    /// nothing at employer B. One row per person per employer: a role change is an UPDATE (the
    /// schema's UNIQUE makes a second row impossible, this function makes it unnecessary).
    /// Errors are reason codes; null is success — the WorldLifecycle convention for state acts.
    /// </summary>
    public static string? AddMember(Db db, long employerId, long actingUserId, long newUserId, string role)
    {
        if (!Enabled(db)) return "careers_disabled";
        if (role is not ("owner" or "member")) return "bad_role";
        if (db.QueryOne("SELECT id FROM pciworld_employers WHERE id=?", employerId) is null) return "not_found";
        if (RoleOf(db, employerId, actingUserId) != "owner") return "not_employer_owner";

        var existing = RoleOf(db, employerId, newUserId);
        if (existing is null)
        {
            db.Execute("INSERT INTO pciworld_employer_members(employer_id,user_id,role) VALUES(?,?,?)",
                employerId, newUserId, role);
            return null;
        }
        if (existing == role) return null;
        // Demoting the last owner recreates the dead-row problem Create exists to prevent: an
        // employer nobody can administer. Someone else must hold 'owner' before this one steps down.
        if (existing == "owner" && role == "member" && OwnerCount(db, employerId) <= 1)
            return "last_owner";
        db.Execute("UPDATE pciworld_employer_members SET role=? WHERE employer_id=? AND user_id=?",
            role, employerId, newUserId);
        return null;
    }

    /// <summary>
    /// Remove a member: an owner removes anyone, and any member may remove themselves — revocation
    /// is deliberately NOT gated on the feature flag, because a kill switch that blocks removing a
    /// compromised account would prolong exactly the incident it exists to contain. The last owner
    /// cannot be removed (dead-row rule); membership rows are deleted, not soft-stated, because a
    /// past member's access history lives in the audit and access logs, not in this row.
    /// </summary>
    public static string? RemoveMember(Db db, long employerId, long actingUserId, long targetUserId)
    {
        var target = RoleOf(db, employerId, targetUserId);
        if (target is null) return "not_a_member";
        if (actingUserId != targetUserId && RoleOf(db, employerId, actingUserId) != "owner")
            return "not_employer_owner";
        if (target == "owner" && OwnerCount(db, employerId) <= 1) return "last_owner";

        db.Execute("DELETE FROM pciworld_employer_members WHERE employer_id=? AND user_id=?",
            employerId, targetUserId);
        return null;
    }

    static long OwnerCount(Db db, long employerId) =>
        db.Scalar<long>("SELECT COUNT(*) FROM pciworld_employer_members WHERE employer_id=? AND role='owner'",
            employerId);

    // ── Verification and suspension (§4.2) ────────────────────────────────────────────────────

    /// <summary>
    /// Change an employer's state. This is the ADMIN route, in its entirety: every call requires an
    /// active pciworld_admin_users row whose role grants `careers.verify` — an employer member has
    /// no admin row, so there is no argument value that lets one verify (or otherwise move) their
    /// own employer through here. Deliberately NOT gated on the feature flag: the kill switch must
    /// never prevent an admin from suspending a fake employer mid-incident.
    ///
    /// The transition itself is decided by <see cref="CareersState.EmployerTransitionAllowed"/> —
    /// one state machine, owned in one place.
    ///
    /// `expectedVersion` is the caller's read of the row; the UPDATE carries `version=?` so a
    /// concurrent write surfaces as "conflict" rather than being overwritten. Reaching 'verified'
    /// stamps verified_by_admin_id and verified_at in the same statement — the attributable act —
    /// and every call writes the audit row inside the same transaction, so a state change without
    /// its history cannot exist even across a crash.
    /// </summary>
    public static string? SetState(Db db, long employerId, string newState, long? adminId, int expectedVersion)
    {
        if (adminId is null) return "admin_required";
        var admin = db.QueryOne("SELECT role,status FROM pciworld_admin_users WHERE id=?", adminId);
        if (admin is null || H.Str(admin["status"]) != "active" ||
            !WorldRbac.Allowed(H.Str(admin["role"]), "careers.verify"))
            return "forbidden";

        var row = db.QueryOne("SELECT state,version FROM pciworld_employers WHERE id=?", employerId);
        if (row is null) return "not_found";
        var from = H.Str(row["state"]) ?? "";
        // Refuse the stale read before touching anything: a caller acting on an old version is
        // acting on an old state, and "invalid_transition" computed from a state that no longer
        // exists would be the wrong answer to the wrong question.
        if (H.L(row["version"]) != expectedVersion) return "conflict";
        if (!CareersState.EmployerTransitionAllowed(from, newState)) return "invalid_transition";

        string? result = null;
        db.Transaction(() =>
        {
            var n = newState == "verified"
                ? db.Execute(
                    @"UPDATE pciworld_employers
                      SET state=?, verified_at=datetime('now'), verified_by_admin_id=?,
                          updated_at=datetime('now'), version=version+1
                      WHERE id=? AND version=?",
                    newState, adminId, employerId, expectedVersion)
                // Leaving 'verified' keeps the last attribution columns: the row says who last made
                // the assertion, and the audit trail below carries the full history of its changes.
                : db.Execute(
                    @"UPDATE pciworld_employers
                      SET state=?, updated_at=datetime('now'), version=version+1
                      WHERE id=? AND version=?",
                    newState, employerId, expectedVersion);
            if (n == 0) { result = "conflict"; return; }

            db.Execute("INSERT INTO pciworld_audit(admin_id,action,detail) VALUES(?,?,?)",
                adminId, "careers_employer_state", $"employer:{employerId} {from}->{newState}");
        });
        return result;
    }

    // ── The gate (§4.3) and the acting-rights predicate (§6 a+d) ─────────────────────────────

    /// <summary>
    /// True only while the employer row currently reads 'verified'. Used by every publish
    /// transition AND every public read path — enforcement lives in two independent places so
    /// neither can be argued away as redundant. Re-reads the row on every call: the whole point of
    /// the one-UPDATE suspension (§4.3) is that no caller is holding a stale answer.
    /// </summary>
    public static bool MayPublish(Db db, long employerId) =>
        db.Scalar<string>("SELECT state FROM pciworld_employers WHERE id=?", employerId) == "verified";

    /// <summary>
    /// Conditions (a) and (d) of the §6 four-condition CV/application authorization: the user is a
    /// member of THIS employer, and the employer is verified — 'suspended' fails exactly like
    /// 'draft', which is what cuts a discovered-fake's access to applicant data mid-session.
    /// One query, both predicates: there is no code path where membership is checked against one
    /// employer and state against another. Condition (b) — the application belongs to one of this
    /// employer's postings — and (c) — consent — belong to the layers that own those rows.
    /// </summary>
    public static bool MayActFor(Db db, long employerId, long userId) =>
        // MUTATION 3: employer_id predicate dropped — membership of ANY verified employer passes
        db.QueryOne(
            @"SELECT m.id FROM pciworld_employer_members m
              JOIN pciworld_employers e ON e.id = m.employer_id
              WHERE m.user_id=? AND e.state='verified'",
            userId) is not null;

    // ── internals ─────────────────────────────────────────────────────────────────────────────

    /// <summary>Lowercase ascii letters/digits with single interior hyphens, 3..160 chars — the
    /// shape every crawlable World URL segment already uses. Anything else is refused, because a
    /// slug is a permanent address and repair would mint one nobody chose.</summary>
    public static bool ValidSlug(string s)
    {
        if (s.Length < 3 || s.Length > MaxSlugLength) return false;
        if (s[0] == '-' || s[^1] == '-') return false;
        char prev = '\0';
        foreach (var ch in s)
        {
            var ok = (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') || ch == '-';
            if (!ok || (ch == '-' && prev == '-')) return false;
            prev = ch;
        }
        return true;
    }

    /// <summary>May return something too short or empty — the caller validates and refuses rather
    /// than fall back to a placeholder address.</summary>
    static string Slugify(string name)
    {
        var sb = new System.Text.StringBuilder(name.Length);
        char prev = '-';
        foreach (var ch in name.Trim().ToLowerInvariant())
        {
            char c = (ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9') ? ch
                   : (ch == ' ' || ch == '-' || ch == '_' || ch == '/' || ch == '.') ? '-' : '\0';
            if (c == '\0') continue;
            if (c == '-' && prev == '-') continue;
            sb.Append(c); prev = c;
        }
        return sb.ToString().Trim('-');
    }
}
