using System.Security.Cryptography;
using static PCI.Backend.Core.CommunityModeration;

namespace PCI.Backend.Core;

/// <summary>
/// The image-classification seam for Phase 2 (docs/pciworld/CCP_PHASE2_DESIGN.md §6.2), and the two
/// implementations that ship with it.
///
/// It deliberately reuses <see cref="CommunityModeration.Signal"/> and the existing
/// <see cref="CommunityModeration.Resolve"/> engine with <c>contentType="image"</c> rather than
/// growing a parallel decision path. Two moderation engines would be two places for the publication
/// rule to be got wrong, and only one of them would have the Phase 1 tests behind it.
///
/// NEITHER IMPLEMENTATION HERE IS A CONTENT-SAFETY VENDOR, AND NEITHER PRETENDS TO BE.
///
/// §28.6 forbids claiming a general classifier detects every illegal image. Nothing in this file
/// detects anything of the sort: <see cref="NullMediaScanner"/> classifies nothing at all, and
/// <see cref="DeterministicMediaScanner"/> recognises hashes it was explicitly told about. A real
/// provider is a third implementation added once one is contracted and benchmarked (CCP-P1-004),
/// and even then it is a signal routed to trained humans — not a detector, and not an adjudicator.
/// </summary>
public static class CommunityMediaScanners
{
    /// <summary>
    /// What a scanner is shown. The sanitised derivative is passed rather than the uploaded bytes:
    /// the derivative is what participants would actually see, so it is what a verdict should be
    /// about, and it is free of the metadata and trailing payloads that would otherwise be sent to a
    /// third party along with the picture.
    /// </summary>
    public readonly record struct MediaSubject(
        byte[] Bytes, string Mime, int Width, int Height, string Sha256);

    /// <summary>
    /// The provider seam. Implementations must NOT throw — an internal failure is reported as
    /// <see cref="Signal.Unavailable"/>, which the engine resolves to "not published". An exception
    /// escaping here would surface as a 500 and lose the upload entirely, turning a moderation
    /// failure into a data-loss bug.
    /// </summary>
    public interface IMediaScanner
    {
        string ProviderName { get; }
        string ProviderVersion { get; }
        Signal Classify(MediaSubject subject);
    }

    /// <summary>
    /// The default when nothing is configured: it classifies NOTHING.
    ///
    /// Every verdict is <see cref="Signal.Unavailable"/>, which resolves to quarantine, which means
    /// no image is published. An operator who has enabled images without choosing a provider does
    /// not get an unscanned public image feed — they get a feature that visibly does not publish,
    /// which is the correct failure (§15, §28.4). This is the fail-closed posture, not a stub
    /// somebody forgot to replace.
    /// </summary>
    public sealed class NullMediaScanner : IMediaScanner
    {
        public string ProviderName => "none";
        public string ProviderVersion => "0";
        public Signal Classify(MediaSubject subject) => Signal.Unavailable;
    }

    /// <summary>
    /// A hash-driven test double, so every branch of the image pipeline — allow, quarantine, block,
    /// restricted escalation, provider failure — can be exercised end to end without a vendor
    /// account, without a network call, and, importantly, <b>without any real harmful material
    /// anywhere near the test suite</b>. A fixture is an ordinary generated PNG; what makes it
    /// "restricted" for the test is that its hash was registered as such.
    ///
    /// **This is not a safety control.** It recognises exactly what it was told to recognise and
    /// nothing else. Wiring it to a production room would be a misuse, which is why the endpoint
    /// layer selects it only under an explicit test/dev setting — the same treatment
    /// <see cref="CommunityModerators.DeterministicModerator"/> gets.
    /// </summary>
    public sealed class DeterministicMediaScanner : IMediaScanner
    {
        readonly Dictionary<string, (string Category, Band Band)> _table = new(StringComparer.OrdinalIgnoreCase);
        readonly bool _failEverything;

        /// <param name="failEverything">Simulates a provider outage: every call reports unavailable,
        /// which must result in nothing being published rather than everything being published.</param>
        public DeterministicMediaScanner(bool failEverything = false) => _failEverything = failEverything;

        public string ProviderName => "deterministic-image";
        public string ProviderVersion => "1";

        /// <summary>Register a verdict for exact bytes. Content-addressed, so the same fixture gets
        /// the same verdict wherever it is used.</summary>
        public DeterministicMediaScanner Register(byte[] bytes, string category, Band band)
            => Register(Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant(), category, band);

        /// <summary>Register a verdict for a known hash.</summary>
        public DeterministicMediaScanner Register(string sha256, string category, Band band)
        {
            _table[sha256] = (category, band);
            return this;
        }

        public Signal Classify(MediaSubject subject)
        {
            try
            {
                if (_failEverything) return Signal.Unavailable;
                // The subject's own hash is not trusted — it is recomputed here. A caller passing a
                // stale or attacker-chosen hash must not be able to select its own verdict.
                var sha = Convert.ToHexString(SHA256.HashData(subject.Bytes)).ToLowerInvariant();
                if (_table.TryGetValue(sha, out var hit))
                    return new Signal(hit.Category, SeverityOf(hit.Band), hit.Band, null, true);
                // An unregistered fixture is ordinary. That is what makes the happy path testable;
                // it is emphatically not a claim that unrecognised images are safe, which is why the
                // production default is NullMediaScanner and not this.
                return Signal.Clean();
            }
            catch
            {
                return Signal.Unavailable;
            }
        }

        static string SeverityOf(Band band) => band switch
        {
            Band.High => "high",
            Band.Medium => "medium",
            _ => "low",
        };
    }
}
