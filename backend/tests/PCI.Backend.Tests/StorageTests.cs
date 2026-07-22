using System.Security.Cryptography;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

public class StorageTests
{
    // valid PNG magic + unique payload so content-addressing never collides across tests
    internal static byte[] PngBytes() =>
        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
            .Concat(Guid.NewGuid().ToByteArray()).ToArray();

    [Theory]
    [InlineData(null, "no_file")]
    [InlineData("", "no_file")]
    [InlineData("hello", "not_a_data_uri")]
    [InlineData("DATA:image/png;base64,AAAA", "not_a_data_uri")]          // prefix is case-sensitive
    [InlineData("data:image/png", "malformed_data_uri")]                  // no comma
    [InlineData("data:image/pngbase64,iVBORw0KGgo=", "malformed_data_uri")] // no semicolon
    [InlineData("data:image/png,abc;def", "malformed_data_uri")]          // comma before semicolon
    [InlineData("data:text/plain;base64,aGVsbG8=", "file_type_not_allowed")]
    [InlineData("data:image/png;base64,%%%", "invalid_base64")]
    [InlineData("data:image/png;base64,aGVsbG8=", "content_mime_mismatch")] // "hello" is not a PNG
    public void DecodeDataUri_ReportsEachRejection(string? uri, string expectedError)
    {
        var (bytes, _, error) = Storage.DecodeDataUri(uri);
        Assert.Null(bytes);
        Assert.Equal(expectedError, error);
    }

    [Fact]
    public void DecodeDataUri_RejectsOversizedPayloadBeforeDecoding()
    {
        // > (MaxBytes + 1024) * 4/3 base64 chars trips the pre-allocation guard
        var big = "data:image/png;base64," + new string('A', 4_200_000);
        var (bytes, _, error) = Storage.DecodeDataUri(big);
        Assert.Null(bytes);
        Assert.Equal("file_too_large", error);
    }

    [Fact]
    public void DecodeDataUri_AcceptsSniffedPngAndPdf()
    {
        var png = PngBytes();
        var (bytes, mime, error) = Storage.DecodeDataUri("data:image/png;base64," + Convert.ToBase64String(png));
        Assert.Null(error);
        Assert.Equal("image/png", mime);
        Assert.Equal(png, bytes);

        var pdf = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4 unit test");
        var (pbytes, pmime, perror) = Storage.DecodeDataUri("data:application/pdf;base64," + Convert.ToBase64String(pdf));
        Assert.Null(perror);
        Assert.Equal("application/pdf", pmime);
        Assert.Equal(pdf, pbytes);
    }

    [Fact]
    public void PutGet_RoundTripsAndEncryptsAtRest()
    {
        var plain = PngBytes();
        var obj = Storage.Put(plain, "image/png", "evidence");
        Assert.StartsWith("local:evidence/", obj.Reference);
        Assert.Equal(plain.Length, obj.SizeBytes);
        Assert.Equal(Convert.ToHexString(SHA256.HashData(plain)).ToLowerInvariant(), obj.Sha256);
        Assert.DoesNotContain(TestEnv.StorageRoot, obj.Reference);   // reference never leaks a path

        // on disk: envelope-encrypted, never the raw PII bytes
        var onDisk = File.ReadAllBytes(Path.Combine(TestEnv.StorageRoot, obj.Reference["local:".Length..]));
        Assert.True(Security.IsEncryptedBlob(onDisk));
        Assert.NotEqual(plain, onDisk);

        var got = Storage.Get(obj.Reference);
        Assert.NotNull(got);
        Assert.Equal(plain, got!.Value.bytes);
        Assert.Equal("image/png", got.Value.mime);

        Assert.Equal(obj.Reference, Storage.Put(plain, "image/png", "evidence").Reference);  // content-addressed dedup
    }

    [Fact]
    public void Get_And_Deletes_RefusePathTraversal()
    {
        Assert.Null(Storage.Get("local:../secrets.txt"));
        Assert.Null(Storage.Get("local:/etc/passwd"));
        Assert.Null(Storage.Get("noprefix"));
        Assert.Null(Storage.Get(null));
        Assert.False(Storage.DeleteRef("local:../../etc/passwd"));
        Assert.False(Storage.DeleteOne(@"local:evidence\..\x.png"));
        Assert.False(Storage.DeleteOne("noprefix"));
        Assert.False(Storage.DeleteOne(""));
    }

    [Fact]
    public void DeleteOne_RemovesTheObject()
    {
        var obj = Storage.Put(PngBytes(), "image/png", "evidence");
        var full = Path.Combine(TestEnv.StorageRoot, obj.Reference["local:".Length..]);
        Assert.True(File.Exists(full));
        Assert.True(Storage.DeleteOne(obj.Reference));
        Assert.False(File.Exists(full));
        Assert.Null(Storage.Get(obj.Reference));
        Assert.True(Storage.DeleteOne(obj.Reference));   // already gone still reports success
    }

    [Fact]
    public void PurgeOlderThan_SkipsFreshProtectedAndForeignFiles()
    {
        string PathOf(Storage.StoredObject o) => Path.Combine(TestEnv.StorageRoot, o.Reference["local:".Length..]);
        var old = DateTime.UtcNow.AddDays(-40);

        var expired = Storage.Put(PngBytes(), "image/png", "evidence");
        File.SetLastWriteTimeUtc(PathOf(expired), old);
        var fresh = Storage.Put(PngBytes(), "image/png", "evidence");
        var protectedDoc = Storage.Put(PngBytes(), "image/png", "honorary-idv");   // protected category
        File.SetLastWriteTimeUtc(PathOf(protectedDoc), old);
        var stray = Path.Combine(TestEnv.StorageRoot, "evidence", "operator-notes.txt");  // not content-addressed
        File.WriteAllText(stray, "keep me");
        File.SetLastWriteTimeUtc(stray, old);

        Assert.Equal(1, Storage.PurgeOlderThan(30));
        Assert.False(File.Exists(PathOf(expired)));
        Assert.True(File.Exists(PathOf(fresh)));
        Assert.True(File.Exists(PathOf(protectedDoc)));
        Assert.True(File.Exists(stray));
    }
}
