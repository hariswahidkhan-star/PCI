using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// RES-001 / RES-002 — durable checkout reservations.
/// A client idempotency key claims at most one open reservation; discount-code capacity is held via
/// <c>reserved_count</c> until settlement converts the hold to <c>used_count</c>, or expiry/release
/// returns the slot. Partner attribution is snapshotted onto the reservation (and later the payment)
/// so a later change to <c>discount_codes.partner_id</c> cannot restate history.
/// </summary>
public static class CheckoutReservation
{
    public const int MaxKeyLength = 120;
    public const int DefaultTtlMinutes = 60;

    public static string? NormalizeKey(string? raw)
    {
        var key = (raw ?? "").Trim();
        if (key.Length == 0) return null;
        return key.Length > MaxKeyLength ? key[..MaxKeyLength] : key;
    }

    public static string? ResolveKey(HttpRequest req, Dictionary<string, System.Text.Json.JsonElement> body)
    {
        var fromBody = NormalizeKey(H.GetS(body, "idempotency_key", "idempotencyKey"));
        if (fromBody is not null) return fromBody;
        if (req.Headers.TryGetValue("Idempotency-Key", out var hdr))
            return NormalizeKey(hdr.ToString());
        return null;
    }

    public static Dictionary<string, object?>? FindByKey(Db db, string key) =>
        db.QueryOne("SELECT * FROM checkout_reservations WHERE idempotency_key=?", key);

    public static Dictionary<string, object?>? FindBySession(Db db, string? sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId)) return null;
        return db.QueryOne("SELECT * FROM checkout_reservations WHERE stripe_session_id=?", sessionId);
    }

    /// <summary>Release expired open holds so capacity is returned before a new reserve attempt.</summary>
    public static int ExpireStale(Db db, long? codeId = null)
    {
        var rows = codeId is null
            ? db.Query("SELECT id, discount_code_id FROM checkout_reservations WHERE status='reserved' AND expires_at < datetime('now')")
            : db.Query("SELECT id, discount_code_id FROM checkout_reservations WHERE status='reserved' AND expires_at < datetime('now') AND discount_code_id=?", codeId);
        var n = 0;
        foreach (var r in rows)
        {
            long? heldCodeId = r["discount_code_id"] is null or DBNull ? null : H.L(r["discount_code_id"]);
            ReleaseHold(db, H.L(r["id"]), heldCodeId is > 0 ? heldCodeId : null, "expired");
            n++;
        }
        return n;
    }

    public static void ReleaseHold(Db db, long reservationId, long? codeId, string status)
    {
        var changes = db.Execute("UPDATE checkout_reservations SET status=?, updated_at=datetime('now') WHERE id=? AND status='reserved'", status, reservationId);
        if (changes > 0 && codeId is > 0)
            db.Execute("UPDATE discount_codes SET reserved_count=CASE WHEN reserved_count>0 THEN reserved_count-1 ELSE 0 END WHERE id=?", codeId);
    }

    public record ReserveResult(bool Ok, string? Error, Dictionary<string, object?>? Reservation, bool Replayed);

    /// <summary>
    /// Claim an idempotency key and, when a discount code is present, take one capacity hold.
    /// Must be called inside <see cref="Db.Transaction"/>.
    /// </summary>
    public static ReserveResult TryReserve(
        Db db,
        string idempotencyKey,
        string email,
        string productType,
        long? certificationId,
        Dictionary<string, object?>? codeRow,
        long amountMinor,
        int ttlMinutes = DefaultTtlMinutes)
    {
        var key = NormalizeKey(idempotencyKey);
        if (key is null) return new(false, "idempotency_key_required", null, false);

        var em = email.Trim().ToLowerInvariant();
        long? codeId = codeRow is null ? null : H.L(codeRow["id"]);
        long? partnerId = codeRow is null || codeRow["partner_id"] is null ? null : H.L(codeRow["partner_id"]);

        ExpireStale(db, codeId);

        var existing = FindByKey(db, key);
        if (existing is not null)
        {
            var st = H.Str(existing["status"]);
            if (st is "reserved" or "settled")
                return new(true, null, existing, true);
            // released/expired — allow a fresh claim under the same key by deleting the dead row.
            db.Execute("DELETE FROM checkout_reservations WHERE id=? AND status IN ('released','expired')", existing["id"]);
        }

        if (codeId is > 0)
        {
            // Capacity = used + reserved. Atomic take; abort when the code is exhausted.
            var took = db.Execute(
                @"UPDATE discount_codes SET reserved_count=reserved_count+1
                  WHERE id=? AND (max_uses IS NULL OR used_count+reserved_count < max_uses)", codeId);
            if (took == 0) return new(false, "code_exhausted", null, false);

            // Partner total allocation also counts open holds.
            if (partnerId is > 0)
            {
                var partner = db.QueryOne("SELECT total_allocation FROM training_partners WHERE id=?", partnerId);
                if (partner?["total_allocation"] is not null)
                {
                    var usedTotal = db.Scalar<long>(
                        @"SELECT COALESCE(SUM(dc.used_count + COALESCE(dc.reserved_count,0)),0)
                          FROM discount_codes dc WHERE dc.partner_id=?", partnerId);
                    if (usedTotal > H.L(partner["total_allocation"]))
                    {
                        db.Execute("UPDATE discount_codes SET reserved_count=CASE WHEN reserved_count>0 THEN reserved_count-1 ELSE 0 END WHERE id=?", codeId);
                        return new(false, "allocation_exhausted", null, false);
                    }
                }
            }
        }

        var expires = DateTime.UtcNow.AddMinutes(Math.Max(5, ttlMinutes))
            .ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        var (id, changes) = db.ExecuteWithChanges(
            @"INSERT OR IGNORE INTO checkout_reservations(
                idempotency_key,email,product_type,certification_id,discount_code_id,partner_id,
                amount_minor,currency,status,expires_at)
              VALUES(?,?,?,?,?,?,?,'USD','reserved',?)",
            key, em, productType, certificationId, codeId, partnerId, amountMinor, expires);
        if (changes == 0)
        {
            // Lost a race on the unique key — roll back the capacity take we just did.
            if (codeId is > 0)
                db.Execute("UPDATE discount_codes SET reserved_count=CASE WHEN reserved_count>0 THEN reserved_count-1 ELSE 0 END WHERE id=?", codeId);
            var raced = FindByKey(db, key);
            if (raced is not null) return new(true, null, raced, true);
            return new(false, "reserve_failed", null, false);
        }

        return new(true, null, FindByKey(db, key) ?? new Dictionary<string, object?> { ["id"] = id }, false);
    }

    public static void AttachStripeSession(Db db, long reservationId, string sessionId) =>
        db.Execute("UPDATE checkout_reservations SET stripe_session_id=?, updated_at=datetime('now') WHERE id=?", sessionId, reservationId);

    /// <summary>
    /// Convert a reserved hold into a settled redemption capacity take. Returns false when the
    /// reservation is missing/already settled or the code has no remaining hold (oversold settle).
    /// </summary>
    public static bool TrySettle(Db db, Dictionary<string, object?>? reservation, long paymentId)
    {
        if (reservation is null) return true; // legacy checkout with no reservation row
        var st = H.Str(reservation["status"]);
        if (st == "settled") return true;
        if (st != "reserved") return false;

        var codeId = reservation["discount_code_id"] is null ? (long?)null : H.L(reservation["discount_code_id"]);
        if (codeId is > 0)
        {
            // Convert hold → used. Prefer reserved_count>0; fall back to classic used_count take for
            // reservations that somehow lost their hold counter.
            var converted = db.Execute(
                @"UPDATE discount_codes SET used_count=used_count+1,
                    reserved_count=CASE WHEN reserved_count>0 THEN reserved_count-1 ELSE 0 END
                  WHERE id=? AND reserved_count>0", codeId);
            if (converted == 0)
            {
                var took = db.Execute(
                    "UPDATE discount_codes SET used_count=used_count+1 WHERE id=? AND (max_uses IS NULL OR used_count<max_uses)", codeId);
                if (took == 0) return false;
            }
        }

        db.Execute("UPDATE checkout_reservations SET status='settled', payment_id=?, updated_at=datetime('now') WHERE id=? AND status='reserved'",
            paymentId, reservation["id"]);
        return true;
    }

    /// <summary>Apply snapshotted partner/code ids onto the payment row (RES-002).</summary>
    public static void StampPaymentAttribution(Db db, long paymentId, Dictionary<string, object?>? reservation)
    {
        if (reservation is null) return;
        db.Execute("UPDATE payments SET discount_code_id=COALESCE(?,discount_code_id), partner_id=COALESCE(?,partner_id) WHERE id=?",
            reservation["discount_code_id"], reservation["partner_id"], paymentId);
    }
}
