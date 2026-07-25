using System.Security.Cryptography;
using System.Text;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class SecurityTests
{
    [Fact]
    public void Sha_ProducesLowercaseHexSha256()
    {
        // known SHA-256 vector for "abc"
        Assert.Equal("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", Security.Sha("abc"));
        Assert.Equal(64, Security.Sha("").Length);
        Assert.NotEqual(Security.Sha("a"), Security.Sha("b"));
    }

    [Fact]
    public void RandomHex_LengthAndAlphabet()
    {
        var t = Security.RandomHex(32);
        Assert.Equal(64, t.Length);
        Assert.All(t, c => Assert.True(Uri.IsHexDigit(c) && !char.IsUpper(c)));
        Assert.NotEqual(t, Security.RandomHex(32));
    }

    [Theory]
    [InlineData("secret", "secret", true)]
    [InlineData("secret", "secreT", false)]
    [InlineData("secret", "secrets", false)]
    [InlineData("", "", false)]           // empty never matches — absent secrets must not compare equal
    [InlineData(null, null, false)]
    [InlineData("secret", null, false)]
    public void FixedTimeEquals_Cases(string? a, string? b, bool expected) =>
        Assert.Equal(expected, Security.FixedTimeEquals(a, b));

    [Fact]
    public void VerifyHubSignature_AcceptsValidRejectsTampered()
    {
        var body = Encoding.UTF8.GetBytes("{\"entry\":[]}");
        const string secret = "app-secret";
        using var h = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hex = Convert.ToHexString(h.ComputeHash(body)).ToLowerInvariant();
        Assert.True(Security.VerifyHubSignature("sha256=" + hex, body, secret));
        Assert.True(Security.VerifyHubSignature(hex, body, secret));               // bare hex accepted too
        Assert.False(Security.VerifyHubSignature("sha256=" + hex, body, "other"));
        Assert.False(Security.VerifyHubSignature(null, body, secret));
        Assert.False(Security.VerifyHubSignature("sha256=" + hex, body, null));
    }

    [Fact]
    public void GenPassword_ClampsLengthAndGuaranteesComplexity()
    {
        Assert.Equal(10, Security.GenPassword(3).Length);    // clamped up
        Assert.Equal(64, Security.GenPassword(999).Length);  // clamped down
        var pw = Security.GenPassword(16);
        Assert.Equal(16, pw.Length);
        Assert.Contains(pw, char.IsLower);
        Assert.Contains(pw, char.IsUpper);
        Assert.Contains(pw, char.IsDigit);
        Assert.Contains(pw, c => !char.IsLetterOrDigit(c));
        // unambiguous alphabets: no 0/O/1/l/I anywhere
        Assert.All(pw, c => Assert.DoesNotContain(c, "0O1lI"));
    }

    [Fact]
    public void EncryptSecret_RoundTripsAndToleratesLegacyPlaintext()
    {
        var token = Security.EncryptSecret("Temp-Pass-42!");
        Assert.NotNull(token);
        Assert.StartsWith("enc:v1:", token);
        Assert.Equal("Temp-Pass-42!", Security.DecryptSecret(token));
        Assert.Equal("plain-legacy", Security.DecryptSecret("plain-legacy"));  // untagged passes through
        Assert.Null(Security.DecryptSecret(null));
        Assert.Equal("", Security.DecryptSecret(""));
    }

    [Fact]
    public void DecryptSecret_TamperedTokenReturnsNull()
    {
        var token = Security.EncryptSecret("sensitive")!;
        var blob = Convert.FromBase64String(token["enc:v1:".Length..]);
        blob[^1] ^= 0xFF;   // flip a tag byte
        Assert.Null(Security.DecryptSecret("enc:v1:" + Convert.ToBase64String(blob)));
        Assert.Null(Security.DecryptSecret("enc:v1:%%%not-base64%%%"));
        Assert.Null(Security.DecryptSecret("enc:v1:AAAA"));   // too short for nonce+tag
    }

    [Fact]
    public void EncryptBytes_RoundTripsAndDetectsEnvelope()
    {
        var plain = Encoding.UTF8.GetBytes("government-id image bytes");
        var blob = Security.EncryptBytes(plain);
        Assert.True(Security.IsEncryptedBlob(blob));
        Assert.False(Security.IsEncryptedBlob(plain));
        Assert.False(Security.IsEncryptedBlob(null));
        Assert.Equal(plain, Security.DecryptBytes(blob));
        Assert.Equal(plain, Security.DecryptBytes(plain));   // legacy plaintext passes through
    }

    [Fact]
    public void DecryptBytes_TamperedEnvelopeThrows()
    {
        var blob = Security.EncryptBytes(Encoding.UTF8.GetBytes("evidence"));
        blob[^1] ^= 0xFF;
        Assert.ThrowsAny<CryptographicException>(() => Security.DecryptBytes(blob));
    }

    [Fact]
    public void VerifyPassword_NeverThrowsOnBadStoredHash()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("correct horse");
        Assert.True(Security.VerifyPassword("correct horse", hash));
        Assert.False(Security.VerifyPassword("wrong", hash));
        Assert.False(Security.VerifyPassword("x", "not-a-bcrypt-hash"));   // legacy/corrupt row → false, not 500
        Assert.False(Security.VerifyPassword("x", null));
        Assert.False(Security.VerifyPassword("x", ""));
    }

    [Fact]
    public void Totp_SecretShapeAndVerification()
    {
        var secret = Security.NewTotpSecret();
        Assert.Equal(32, secret.Length);   // 20 random bytes → 32 base32 chars
        Assert.All(secret, c => Assert.Contains(c, "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"));

        // compute the current RFC 6238 code independently; the ±1-step window absorbs a boundary tick
        var code = TotpCode(secret, DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30);
        Assert.True(Security.VerifyTotp(secret, code));
        Assert.True(Security.VerifyTotpStep(secret, code) >= 0);
        Assert.Equal(-1, Security.VerifyTotpStep(secret, "12345"));      // wrong length
        Assert.Equal(-1, Security.VerifyTotpStep(secret, null));
        Assert.Equal(-1, Security.VerifyTotpStep("", code));

        // a 6-digit code provably outside the accepted ±1 window (skip any accidental collision,
        // re-deriving the window each probe so a step tick mid-test cannot flake the assertion)
        for (var wrong = 0; wrong < 10; wrong++)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;
            var valid = new[] { TotpCode(secret, now - 1), TotpCode(secret, now), TotpCode(secret, now + 1) };
            var candidate = wrong.ToString("D6");
            if (valid.Contains(candidate)) continue;
            Assert.Equal(-1, Security.VerifyTotpStep(secret, candidate));
            break;
        }
    }

    static string TotpCode(string base32Secret, long step)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var bytes = new List<byte>();
        int bits = 0, value = 0;
        foreach (var c in base32Secret)
        {
            value = (value << 5) | alphabet.IndexOf(c); bits += 5;
            if (bits >= 8) { bytes.Add((byte)((value >> (bits - 8)) & 0xFF)); bits -= 8; }
        }
        var counter = BitConverter.GetBytes(step);
        if (BitConverter.IsLittleEndian) Array.Reverse(counter);
        using var hmac = new HMACSHA1(bytes.ToArray());
        var hash = hmac.ComputeHash(counter);
        var pos = hash[^1] & 0x0F;
        var truncated = ((hash[pos] & 0x7F) << 24) | (hash[pos + 1] << 16) | (hash[pos + 2] << 8) | hash[pos + 3];
        return (truncated % 1_000_000).ToString("D6");
    }
}

public class RbacTests
{
    [Fact]
    public void PermsFor_OwnerGetsEverySection()
    {
        var perms = Rbac.PermsFor("owner", null);
        Assert.Equal(Rbac.AllSections.Distinct().Count(), perms.Count);
        Assert.Contains("finance", perms);
        Assert.Contains("cc_publish", perms);
    }

    [Fact]
    public void PermsFor_CustomRoleGetsOnlyExplicitGrants()
    {
        var perms = Rbac.PermsFor("custom", "[\"inbox\",\"reports\"]");
        Assert.Equal(new[] { "inbox", "reports" }, perms.OrderBy(x => x));
    }

    [Fact]
    public void PermsFor_MergesRoleWithOverridesAndIgnoresBadJson()
    {
        var perms = Rbac.PermsFor("viewer", "[\"finance\"]");
        Assert.Contains("overview", perms);
        Assert.Contains("reports", perms);
        Assert.Contains("finance", perms);    // explicit per-person grant on top of the bundle
        Assert.Equal(new[] { "overview", "reports" }, Rbac.PermsFor("viewer", "{not json").OrderBy(x => x));
        Assert.Empty(Rbac.PermsFor("no_such_role", null));
    }

    [Fact]
    public void PermsFor_ContentGrandfathersSimLab_ButSimLabStaysLeastPrivilege()
    {
        // The Simulation Lab admin has its own first-class permission, surfaced in the Team & Access catalogue.
        Assert.Contains("sim_lab", Rbac.AllSections);

        // Grandfather: an admin who could manage the Lab via 'content' keeps access through 'sim_lab'.
        Assert.Contains("sim_lab", Rbac.PermsFor("custom", "[\"content\"]"));
        // The seeded website_manager bundle (which carried 'content') now also carries 'sim_lab'.
        Assert.Contains("sim_lab", Rbac.PermsFor("website_manager", null));

        // Least privilege: 'sim_lab' can be granted alone without conferring marketing-'content' rights.
        var simOnly = Rbac.PermsFor("custom", "[\"sim_lab\"]");
        Assert.Contains("sim_lab", simOnly);
        Assert.DoesNotContain("content", simOnly);
    }
}
