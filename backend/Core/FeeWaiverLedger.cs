using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Durable fee-waiver ledger helpers. Partial and reschedule-only waivers must carry a client
/// idempotency key so retries across network/process boundaries cannot create duplicate ledger
/// rows, discount codes, or seats. Full seat grants may also supply a key; when they do, the same
/// uniqueness guard applies.
/// </summary>
public static class FeeWaiverLedger
{
    public const int MaxKeyLength = 120;

    /// <summary>Trim and bound a client key. Returns null when empty/whitespace.</summary>
    public static string? NormalizeKey(string? raw)
    {
        var key = (raw ?? "").Trim();
        if (key.Length == 0) return null;
        return key.Length > MaxKeyLength ? key[..MaxKeyLength] : key;
    }

    /// <summary>Resolve from JSON body (<c>idempotency_key</c>) or the <c>Idempotency-Key</c> header.</summary>
    public static string? ResolveKey(HttpRequest req, Dictionary<string, System.Text.Json.JsonElement> body)
    {
        var fromBody = NormalizeKey(H.GetS(body, "idempotency_key", "idempotencyKey"));
        if (fromBody is not null) return fromBody;
        if (req.Headers.TryGetValue("Idempotency-Key", out var hdr))
            return NormalizeKey(hdr.ToString());
        return null;
    }

    public static Dictionary<string, object?>? Find(Db db, string key) =>
        db.QueryOne("SELECT * FROM fee_waivers WHERE idempotency_key=?", key);

    /// <summary>
    /// Insert a ledger row. When <paramref name="idempotencyKey"/> is set, uses INSERT OR IGNORE so a
    /// concurrent/replayed request cannot create a second row. Returns (row id, created).
    /// </summary>
    public static (long Id, bool Created) TryInsert(
        Db db,
        string? idempotencyKey,
        long userId,
        string productType,
        long? certificationId,
        string kind,
        string? feeType,
        string? waiverType,
        double originalAmount,
        double waivedAmount,
        double finalAmount,
        double? payableAmount,
        string? reason,
        string? note,
        long? approvedBy,
        long? paymentId = null,
        long? codeId = null,
        string? expiresAt = null,
        string? sponsor = null,
        long? institutionId = null,
        long? incidentId = null,
        string? evidenceRef = null,
        string status = "granted")
    {
        if (idempotencyKey is not null)
        {
            var (id, changes) = db.ExecuteWithChanges(
                @"INSERT OR IGNORE INTO fee_waivers(
                    user_id,product_type,certification_id,kind,fee_type,waiver_type,
                    original_amount,waived_amount,final_amount,payable_amount,currency,
                    reason,note,sponsor,institution_id,incident_id,evidence_ref,
                    approved_by,payment_id,code_id,expires_at,status,idempotency_key)
                  VALUES(?,?,?,?,?,?,?,?,?,?,'USD',?,?,?,?,?,?,?,?,?,?,?,?)",
                userId, productType, certificationId, kind, feeType, waiverType,
                originalAmount, waivedAmount, finalAmount, payableAmount,
                reason, note, sponsor, institutionId, incidentId, evidenceRef,
                approvedBy, paymentId, codeId, expiresAt, status, idempotencyKey);
            if (changes == 0)
            {
                var existing = Find(db, idempotencyKey);
                if (existing is not null) return (H.L(existing["id"]), false);
            }
            return (id, true);
        }

        var newId = db.ExecuteReturningId(
            @"INSERT INTO fee_waivers(
                user_id,product_type,certification_id,kind,fee_type,waiver_type,
                original_amount,waived_amount,final_amount,payable_amount,currency,
                reason,note,sponsor,institution_id,incident_id,evidence_ref,
                approved_by,payment_id,code_id,expires_at,status)
              VALUES(?,?,?,?,?,?,?,?,?,?,'USD',?,?,?,?,?,?,?,?,?,?,?)",
            userId, productType, certificationId, kind, feeType, waiverType,
            originalAmount, waivedAmount, finalAmount, payableAmount,
            reason, note, sponsor, institutionId, incidentId, evidenceRef,
            approvedBy, paymentId, codeId, expiresAt, status);
        return (newId, true);
    }

    /// <summary>Stable replay payload for an existing ledger row (partial or full).</summary>
    public static object ReplayResponse(Db db, Dictionary<string, object?> row)
    {
        var kind = H.Str(row["kind"]) ?? "full";
        var percent = 100.0;
        var original = H.D(row["original_amount"]);
        var waived = H.D(row["waived_amount"]);
        if (original > 0) percent = Math.Round(waived / original * 100.0, 1);
        var payable = row.ContainsKey("payable_amount") && row["payable_amount"] is not null
            ? H.D(row["payable_amount"])
            : H.D(row["final_amount"]);
        string? code = null;
        if (row["code_id"] is not null)
            code = H.Str(db.QueryOne("SELECT code FROM discount_codes WHERE id=?", row["code_id"])?["code"]);

        return new
        {
            ok = true,
            replayed = true,
            waiver_id = H.L(row["id"]),
            kind,
            fee_type = H.Str(row["fee_type"]),
            percent,
            original,
            waived_amount = waived,
            payable,
            payment_id = row["payment_id"] is null ? (long?)null : H.L(row["payment_id"]),
            code,
            skips_checkout = payable <= 0,
            expires = H.Str(row["expires_at"]),
        };
    }
}
