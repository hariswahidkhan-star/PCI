using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Discount-abuse monitoring. Rules raise fraud_flags rows for the human review queue and alert
/// operators — they never auto-block a student: suspension is always an explicit admin action on the
/// queue (Admin → Discount codes → Review queue).
/// </summary>
public static class FraudChecks
{
    /// <summary>Run after each successful redemption.</summary>
    public static void OnRedemption(Db db, long codeId, string code, string? email, long? partnerId)
    {
        email = (email ?? "").ToLowerInvariant();

        // Plus-alias duplicates: jo+1@x and jo+2@x are the same person redeeming twice around a
        // per-email limit. Compare on the normalised local part.
        if (email.Contains('@'))
        {
            var at = email.IndexOf('@');
            var local = email[..at];
            var norm = (local.Contains('+') ? local[..local.IndexOf('+')] : local).Replace(".", "");
            var domain = email[at..];
            var siblings = db.Query("SELECT DISTINCT email FROM code_redemptions WHERE code_id=? AND email<>? AND email LIKE ?", codeId, email, "%" + domain);
            var dupes = siblings.Count(s =>
            {
                var e2 = H.Str(s["email"]) ?? ""; var at2 = e2.IndexOf('@');
                if (at2 <= 0) return false;
                var l2 = e2[..at2];
                return (l2.Contains('+') ? l2[..l2.IndexOf('+')] : l2).Replace(".", "") == norm;
            });
            if (dupes > 0)
                Flag(db, "duplicate_account", codeId, email, $"Normalised address matches {dupes} other redemption(s) of code {code} ({norm}***{domain}).");

            // Domain burst: many redemptions of one code from one email domain inside 24 h.
            if (domain is not ("@gmail.com" or "@outlook.com" or "@hotmail.com" or "@yahoo.com" or "@icloud.com"))
            {
                var burst = db.Scalar<long>("SELECT COUNT(*) FROM code_redemptions WHERE code_id=? AND email LIKE ? AND redeemed_at >= datetime('now','-1 days')", codeId, "%" + domain);
                if (burst >= 5)
                    Flag(db, "domain_burst", codeId, email, $"{burst} redemptions of code {code} from {domain} within 24 h.");
            }
        }

        // Velocity: a code burning through most of its allocation within 24 h.
        var c = db.QueryOne("SELECT max_uses, used_count FROM discount_codes WHERE id=?", codeId);
        if (c?["max_uses"] is not null && H.L(c["max_uses"]) >= 10)
        {
            var recent = db.Scalar<long>("SELECT COUNT(*) FROM code_redemptions WHERE code_id=? AND redeemed_at >= datetime('now','-1 days')", codeId);
            if (recent >= H.L(c["max_uses"]) * 0.8)
                Flag(db, "velocity", codeId, null, $"Code {code}: {recent} of {H.L(c["max_uses"])} permitted uses redeemed within 24 h.");
        }

        // Institution thresholds: notify the partner (and PCI) at 80% of the total allocation.
        if (partnerId is { } pid)
        {
            var partner = db.QueryOne("SELECT name,total_allocation FROM training_partners WHERE id=?", pid);
            if (partner?["total_allocation"] is not null && H.L(partner["total_allocation"]) > 0)
            {
                var used = db.Scalar<long>("SELECT COALESCE(SUM(used_count),0) FROM discount_codes WHERE partner_id=?", pid);
                var cap = H.L(partner["total_allocation"]);
                if (used == (long)Math.Ceiling(cap * 0.8) || used == cap)
                {
                    var msg = used >= cap ? $"Your sponsorship allocation ({cap}) is fully used." : $"You have used {used} of {cap} sponsored registrations.";
                    try { db.Execute("INSERT INTO partner_notices(partner_id,title,body) VALUES(?,?,?)", pid,
                        used >= cap ? "Sponsorship allocation fully used" : "Sponsorship allocation at 80%", msg); } catch { }
                    try { Notify.Alert(db, "partners", $"Institution allocation {(used >= cap ? "exhausted" : "at 80%")}: {H.Str(partner["name"])}",
                        $"<p>{System.Net.WebUtility.HtmlEncode(H.Str(partner["name"]) ?? "")}: {msg}</p>", "training_partner", pid); } catch { }
                }
            }
            try { db.Execute("INSERT INTO partner_notices(partner_id,title,body) VALUES(?,?,?)", pid,
                "New registration with your code", $"A student registered using code {code}."); } catch { }
        }
    }

    static void Flag(Db db, string kind, long codeId, string? email, string detail)
    {
        // One open flag per kind+code+email — repeated triggers refresh, not spam.
        var existing = db.QueryOne("SELECT id FROM fraud_flags WHERE kind=? AND code_id=? AND COALESCE(email,'')=? AND status='open'", kind, codeId, email ?? "");
        if (existing is not null) { db.Execute("UPDATE fraud_flags SET detail=?, created_at=datetime('now') WHERE id=?", detail, existing["id"]); return; }
        db.Execute("INSERT INTO fraud_flags(kind,code_id,email,detail) VALUES(?,?,?,?)", kind, codeId, email, detail);
        try { Notify.Alert(db, "fraud", $"Discount review flag: {kind}", $"<p>{System.Net.WebUtility.HtmlEncode(detail)}</p><p>Review it in Admin → Discount codes → Review queue.</p>", "fraud_flag", codeId); } catch { }
    }
}
