using System.Text;
using PCI.Backend.Core;
using Xunit;

namespace PCI.Backend.Tests;

/// <summary>
/// One malware policy for every upload door (RES-021).
///
/// The point of these is not that the built-in signatures are a substitute for antivirus — they are not,
/// and the connector says so. The point is that (a) the obvious payloads are refused, (b) BOTH upload
/// paths now apply the same rules, where previously evidence/attachments applied none, and (c) a
/// configured-but-unreachable scanner fails CLOSED instead of silently waving files through.
/// </summary>
[Collection("upload-scan")]
public class UploadScanTests : IDisposable
{
    public UploadScanTests() => Environment.SetEnvironmentVariable("UPLOAD_SCANNER_URL", null);
    public void Dispose() => Environment.SetEnvironmentVariable("UPLOAD_SCANNER_URL", null);

    static byte[] Pdf(string extra = "") => Encoding.ASCII.GetBytes("%PDF-1.7\n" + extra + "\n%%EOF");
    static byte[] Png()
    {
        var b = new byte[64];
        b[0] = 0x89; b[1] = 0x50; b[2] = 0x4E; b[3] = 0x47;
        return b;
    }

    // ---------------------------------------------------------------- built-in signatures

    [Fact]
    public void CleanPdfAndPngAreAccepted()
    {
        Assert.True(UploadScan.Scan(Pdf(), "application/pdf").Clean);
        Assert.True(UploadScan.Scan(Png(), "image/png").Clean);
    }

    [Theory]
    [InlineData(new byte[] { 0x4D, 0x5A, 0x90, 0x00 })]                 // Windows PE
    [InlineData(new byte[] { 0x7F, 0x45, 0x4C, 0x46 })]                 // ELF
    [InlineData(new byte[] { 0xCA, 0xFE, 0xBA, 0xBE })]                 // Mach-O fat / Java class
    [InlineData(new byte[] { 0xCF, 0xFA, 0xED, 0xFE })]                 // Mach-O 64
    public void ExecutableImagesAreRefusedWhateverTypeTheyClaim(byte[] payload)
    {
        // The declared MIME is a lie the uploader controls; the bytes are not.
        var v = UploadScan.Scan(payload, "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("executable_header", v.Reason);
    }

    [Fact]
    public void ShebangScriptsAreRefused()
    {
        var v = UploadScan.Scan(Encoding.ASCII.GetBytes("#!/bin/sh\nrm -rf /\n"), "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("script_shebang", v.Reason);
    }

    [Fact]
    public void EicarTestFileIsRefused()
    {
        // The industry-standard test payload — harmless, but every scanner flags it, so it is the one
        // case a test suite can assert without shipping something dangerous.
        var eicar = Encoding.ASCII.GetBytes(@"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*");
        var v = UploadScan.Scan(eicar, "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("eicar_test_file", v.Reason);
    }

    [Fact]
    public void EicarIsFoundEvenWhenItIsEmbeddedMidFile()
    {
        // A scanner that only looked at the first bytes would miss this.
        var payload = Pdf(@"junk X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H* junk");
        Assert.False(UploadScan.Scan(payload, "application/pdf").Clean);
    }

    [Fact]
    public void PdfWithALaunchActionIsRefused()
    {
        var v = UploadScan.Scan(Pdf("/Type /Action /S /Launch /Win (cmd.exe)"), "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("pdf_launch_action", v.Reason);
    }

    [Fact]
    public void EmptyUploadIsRefused() =>
        Assert.Equal("empty_file", UploadScan.Scan(Array.Empty<byte>(), "application/pdf").Reason);

    // ---------------------------------------------------------------- external scanner: fail closed

    [Fact]
    public void NoScannerConfiguredMeansBuiltInSignaturesOnly()
    {
        Assert.False(UploadScan.ExternalScannerConfigured);
        Assert.True(UploadScan.Scan(Pdf(), "application/pdf").Clean);
    }

    [Fact]
    public void ConfiguredButUnreachableScannerFailsClosed()
    {
        // The whole reason this matters: an operator who asked for scanning must get scanning or an
        // error — never a silent bypass because the scanner happened to be down.
        Environment.SetEnvironmentVariable("UPLOAD_SCANNER_URL", "http://127.0.0.1:1/scan");
        Assert.True(UploadScan.ExternalScannerConfigured);
        var v = UploadScan.Scan(Pdf(), "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("scanner_unavailable", v.Reason);
    }

    [Fact]
    public void BuiltInSignaturesStillApplyBeforeTheScannerIsEvenCalled()
    {
        // An unreachable scanner must not mask a payload we can already identify locally, and the local
        // check must not cost a network round-trip.
        Environment.SetEnvironmentVariable("UPLOAD_SCANNER_URL", "http://127.0.0.1:1/scan");
        var v = UploadScan.Scan(new byte[] { 0x4D, 0x5A, 0x90, 0x00 }, "application/pdf");
        Assert.False(v.Clean);
        Assert.Equal("executable_header", v.Reason);
    }

    // ---------------------------------------------------------------- both doors, one policy

    [Fact]
    public void BothUploadPathsRefuseTheSamePayload()
    {
        // Storage.DecodeDataUri is the evidence / identity-document / attachment door; DocStore.Decode is
        // the documents door. Before RES-021 only the second had any content-safety check.
        var eicarB64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(
            @"X5O!P%@AP[4\PZX54(P^)7CC)7}$EICAR-STANDARD-ANTIVIRUS-TEST-FILE!$H+H*"));
        var dataUri = "data:application/pdf;base64," + eicarB64;

        var (bytes, _, storageError) = Storage.DecodeDataUri(dataUri);
        Assert.Null(bytes);
        Assert.NotNull(storageError);

        var (file, docError) = DocStore.Decode(dataUri);
        Assert.Null(file);
        Assert.NotNull(docError);
    }

    [Fact]
    public void AGenuineDocumentStillPassesBothDoors()
    {
        var dataUri = "data:application/pdf;base64," + Convert.ToBase64String(Pdf("hello"));
        var (bytes, _, storageError) = Storage.DecodeDataUri(dataUri);
        Assert.NotNull(bytes);
        Assert.Null(storageError);

        var (file, docError) = DocStore.Decode(dataUri);
        Assert.NotNull(file);
        Assert.Null(docError);
    }
}
