using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Marketing centre helpers. The cardinal rules the spec insists on:
///  • Provider SECRETS live ONLY in environment variables. This class reports booleans
///    (configured / not) — it never returns a secret value.
///  • OAuth access/refresh tokens are stored ENCRYPTED (Security.EncryptSecret) and decrypted only
///    server-side at the moment of a provider call. They are never sent to the browser.
/// </summary>
public static class Marketing
{
    static string? E(string k) => Environment.GetEnvironmentVariable(k);
    static bool Has(string k) => !string.IsNullOrWhiteSpace(E(k));

    /// <summary>Per-provider configuration state — booleans only, safe to send to the Admin UI.</summary>
    public static object ProviderConfig() => new
    {
        token_encryption = Security.HasDedicatedEncryptionKey(),   // dedicated 32-byte key present?
        linkedin = new
        {
            client_id = Has("LINKEDIN_CLIENT_ID"),
            client_secret = Has("LINKEDIN_CLIENT_SECRET"),
            webhook_secret = Has("LINKEDIN_WEBHOOK_SECRET"),
            configured = Has("LINKEDIN_CLIENT_ID") && Has("LINKEDIN_CLIENT_SECRET"),
        },
        google = new
        {
            client_id = Has("GOOGLE_OAUTH_CLIENT_ID"),
            client_secret = Has("GOOGLE_OAUTH_CLIENT_SECRET"),
            ads_developer_token = Has("GOOGLE_ADS_DEVELOPER_TOKEN"),
            configured = Has("GOOGLE_OAUTH_CLIENT_ID") && Has("GOOGLE_OAUTH_CLIENT_SECRET"),
        },
        meta = new
        {
            app_id = Has("META_APP_ID"),
            app_secret = Has("META_APP_SECRET"),
            webhook_secret = Has("META_WEBHOOK_SECRET"),
            configured = Has("META_APP_ID") && Has("META_APP_SECRET"),
        },
    };

    /// <summary>Is the given platform family able to even begin an OAuth connection (client id+secret present)?</summary>
    public static bool FamilyConfigured(string family) => family switch
    {
        "linkedin" => Has("LINKEDIN_CLIENT_ID") && Has("LINKEDIN_CLIENT_SECRET"),
        "google" => Has("GOOGLE_OAUTH_CLIENT_ID") && Has("GOOGLE_OAUTH_CLIENT_SECRET"),
        "meta" => Has("META_APP_ID") && Has("META_APP_SECRET"),
        _ => false,
    };

    /// <summary>Encrypt + persist OAuth tokens onto a connection row. Plaintext is never stored.</summary>
    public static void StoreTokens(Db db, long connectionId, string? access, string? refresh, string? expiresAtIso)
    {
        db.Execute(@"UPDATE mkt_connections SET access_token_enc=?, refresh_token_enc=?, token_expires_at=COALESCE(?,token_expires_at), updated_at=datetime('now') WHERE id=?",
            Security.EncryptSecret(access), Security.EncryptSecret(refresh), expiresAtIso, connectionId);
    }

    /// <summary>Decrypt a stored access token for a server-side provider call (never leaves the backend).</summary>
    public static string? AccessToken(Db db, long connectionId)
        => Security.DecryptSecret(H.Str(db.QueryOne("SELECT access_token_enc FROM mkt_connections WHERE id=?", connectionId)?["access_token_enc"]));

    /// <summary>Recompute a capability's live status against a connection. A feature never advances past
    /// its declared ceiling just because a connection exists: an approval-gated feature stays
    /// approval-required until an operator records the provider's approval; we only ever downgrade an
    /// otherwise-ready feature to not_connected when there is no live connection.</summary>
    public static string EffectiveStatus(string declaredStatus, bool hasLiveConnection)
    {
        // Terminal / manual states are never auto-changed.
        if (declaredStatus is "unsupported" or "deprecated" or "manual_workflow_only") return declaredStatus;
        if (declaredStatus is "provider_approval_required" or "account_upgrade_required") return declaredStatus;
        // Ready-in-principle features (available / read_only / not_connected) reflect connection presence.
        if (!hasLiveConnection) return "not_connected";
        return declaredStatus == "not_connected" ? "available_with_permission" : declaredStatus;
    }
}
