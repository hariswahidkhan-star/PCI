using PCI.Backend.Data;

namespace PCI.Backend.Core;

/// <summary>
/// Who may enter a guest room at all — the age and jurisdiction limb of CCP-P1-003 (§7.1, §19.3).
///
/// WHAT THIS IS, STATED BEFORE ANYTHING ELSE.
///
/// This is a **declaration and eligibility gate**, not age verification. A person types a date; the
/// server records what they declared, when, and against which policy version. It establishes that
/// the service asked, that an answer was given, and that entry was refused when the answer did not
/// meet the configured minimum. It does **not** establish that the answer was true, and no comment,
/// column name or admin screen in this system may imply otherwise. Real assurance needs identity
/// verification, which is a separate procurement decision with its own privacy consequences.
///
/// Saying so plainly is the point. A gate that quietly gets described as "age verification" in a
/// later summary is worse than no gate, because it invites people to rely on it.
///
/// WHY THE DEFAULTS ARE THE STRICT ONES.
///
/// The minimum age defaults to 18 and the jurisdiction allowlist defaults to EMPTY — and an empty
/// allowlist admits nobody. That looks unhelpful and is deliberate: CCP-P1-003 is open precisely
/// because counsel has not yet said which jurisdictions PCI will serve or what minimum age applies
/// in them. Until somebody with that authority fills the list in, the honest behaviour is to admit
/// no one rather than to guess a default that would quietly become the policy.
///
/// So this module cannot, on its own, close CCP-P1-003. It supplies the mechanism and records the
/// evidence; the *values* are a legal decision, and the register says so.
/// </summary>
public static class CommunityEligibility
{
    public const string MinAgeKey = "pciworld_community_min_age";
    public const string JurisdictionsKey = "pciworld_community_jurisdictions";
    public const string PolicyVersionKey = "pciworld_community_eligibility_version";

    /// <summary>The default minimum age when an operator has set none. 18 rather than 13 or 16
    /// because this is the conservative end of the range counsel might land on, and a default that
    /// is later tightened has admitted nobody it should not have; a default that is later loosened
    /// has.</summary>
    public const int DefaultMinAge = 18;

    /// <summary>Upper sanity bound on the configurable minimum. Not a policy view — a guard against
    /// a fat-fingered setting silently closing the service.</summary>
    public const int MaxConfigurableMinAge = 25;

    public enum Verdict
    {
        Eligible,
        /// <summary>Declared age is below the configured minimum.</summary>
        UnderMinimumAge,
        /// <summary>No date was given, or it was not a usable date.</summary>
        NoDeclaration,
        /// <summary>The declared date is not a date a living person could have.</summary>
        ImplausibleDeclaration,
        /// <summary>No jurisdiction was given.</summary>
        NoJurisdiction,
        /// <summary>The declared jurisdiction is not on the approved list.</summary>
        JurisdictionNotServed,
        /// <summary>No jurisdiction has been approved at all — the service is not open to anyone
        /// yet. Distinguished from <see cref="JurisdictionNotServed"/> so an operator reading a log
        /// can tell "you are in the wrong country" from "nobody has configured this".</summary>
        NoJurisdictionsConfigured,
    }

    public readonly record struct Decision(Verdict Verdict, int MinAge, string? Jurisdiction, int? DeclaredAge)
    {
        public bool Allowed => Verdict == Verdict.Eligible;

        /// <summary>A reason code safe to return to the person. Deliberately does NOT distinguish
        /// "you are too young" from "your country is not served" in a way that would let someone
        /// map the policy by trying values — the message is specific enough to act on and no more.</summary>
        public string ReasonCode => Verdict switch
        {
            Verdict.Eligible => "eligible",
            Verdict.UnderMinimumAge => "below_minimum_age",
            Verdict.NoDeclaration or Verdict.ImplausibleDeclaration => "age_declaration_required",
            Verdict.NoJurisdiction => "jurisdiction_required",
            _ => "not_available_in_your_location",
        };
    }

    public static int MinAge(Db db)
    {
        var raw = Settings.Str(db, MinAgeKey, "");
        if (!int.TryParse(raw, out var n)) return DefaultMinAge;
        // A configured value outside the sane band is treated as unset rather than obeyed. Someone
        // typing 1 must not open the service to children because a settings screen accepted it.
        return n is >= 13 and <= MaxConfigurableMinAge ? n : DefaultMinAge;
    }

    /// <summary>
    /// The approved jurisdictions, upper-cased ISO 3166-1 alpha-2 codes. An EMPTY set means no
    /// jurisdiction has been approved, which admits nobody — see the class comment.
    /// </summary>
    public static IReadOnlySet<string> Jurisdictions(Db db)
    {
        var raw = Settings.Str(db, JurisdictionsKey, "") ?? "";
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var part in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var code = part.ToUpperInvariant();
            // Only well-formed codes. Junk in the setting must not become a country that matches
            // whatever a client sends.
            if (code.Length == 2 && char.IsAsciiLetterUpper(code[0]) && char.IsAsciiLetterUpper(code[1]))
                set.Add(code);
        }
        return set;
    }

    /// <summary>Whether the gate is configured well enough to admit anybody at all. Surfaced to the
    /// admin readiness screen so "nobody can join" is legible as an unset policy rather than a
    /// fault.</summary>
    public static bool Configured(Db db) => Jurisdictions(db).Count > 0;

    /// <summary>
    /// Evaluate a declaration. Pure apart from reading settings, and it never throws — a malformed
    /// date is a refusal with a reason, not a 500, because the entry form is reachable by anyone.
    /// </summary>
    /// <param name="declaredBirthDate">An ISO <c>YYYY-MM-DD</c> date as declared by the person.</param>
    /// <param name="declaredJurisdiction">An ISO 3166-1 alpha-2 country code as declared.</param>
    /// <param name="asOfUtc">Today, injected so the whole thing is testable without a clock.</param>
    public static Decision Evaluate(Db db, string? declaredBirthDate, string? declaredJurisdiction,
                                    DateTime asOfUtc)
    {
        var min = MinAge(db);
        var approved = Jurisdictions(db);
        var code = (declaredJurisdiction ?? "").Trim().ToUpperInvariant();

        // Jurisdiction first: if the service is not open where someone is, their date of birth is
        // not our business and should not be collected, let alone recorded.
        if (approved.Count == 0) return new Decision(Verdict.NoJurisdictionsConfigured, min, null, null);
        if (code.Length == 0) return new Decision(Verdict.NoJurisdiction, min, null, null);
        if (!approved.Contains(code)) return new Decision(Verdict.JurisdictionNotServed, min, code, null);

        if (string.IsNullOrWhiteSpace(declaredBirthDate))
            return new Decision(Verdict.NoDeclaration, min, code, null);
        if (!DateTime.TryParseExact(declaredBirthDate.Trim(), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var dob))
            return new Decision(Verdict.NoDeclaration, min, code, null);

        var age = AgeOn(dob, asOfUtc);
        // A future date or an implausible one is rejected rather than arithmetically accepted: a
        // birth date in 1850 is not a person, it is someone probing the form.
        if (age < 0 || age > 120) return new Decision(Verdict.ImplausibleDeclaration, min, code, null);
        if (age < min) return new Decision(Verdict.UnderMinimumAge, min, code, age);

        return new Decision(Verdict.Eligible, min, code, age);
    }

    /// <summary>Whole years elapsed, counting the birthday properly rather than dividing by 365.25 —
    /// an off-by-one here is the difference between admitting a seventeen-year-old and not.</summary>
    public static int AgeOn(DateTime birthDate, DateTime asOf)
    {
        var age = asOf.Year - birthDate.Year;
        if (asOf.Month < birthDate.Month
            || (asOf.Month == birthDate.Month && asOf.Day < birthDate.Day)) age--;
        return age;
    }

    /// <summary>
    /// Record what was declared, against the policy that was in force. The DATE OF BIRTH IS NOT
    /// STORED — only the derived age band and the jurisdiction, because keeping a date of birth for
    /// an anonymous guest is collecting identifying personal data the service has no use for and
    /// would then have to protect, disclose and delete. What a later dispute needs is evidence that
    /// the gate was applied, not the applicant's identity.
    /// </summary>
    public static void RecordDeclaration(Db db, long guestSessionId, Decision decision, string policyVersion)
        => db.Execute(
            @"INSERT INTO pciworld_community_eligibility_log
                  (guest_session_id,jurisdiction,min_age_required,declared_age_band,outcome,policy_version)
              VALUES(?,?,?,?,?,?)",
            guestSessionId, decision.Jurisdiction, decision.MinAge,
            AgeBand(decision.DeclaredAge), decision.Verdict.ToString().ToLowerInvariant(), policyVersion);

    /// <summary>Coarse bands, so the log is evidence of eligibility rather than a birthday list.</summary>
    public static string AgeBand(int? age) => age switch
    {
        null => "unknown",
        < 13 => "under_13",
        < 16 => "13_15",
        < 18 => "16_17",
        < 25 => "18_24",
        < 40 => "25_39",
        < 60 => "40_59",
        _ => "60_plus",
    };
}
