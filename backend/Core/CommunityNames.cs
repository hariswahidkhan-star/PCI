using System.Text;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World community — guest display-name normalization, confusable folding and validation
/// (docs/pciworld/CCP_PHASE1_DESIGN.md §5; user stories PW-US-018 … PW-US-020).
///
/// A typed display name is not an identity — it is a label other participants read, so the only
/// jobs here are: keep it legible, stop it impersonating someone, and stop it carrying content the
/// room does not allow. Everything is a pure function over a string, so the whole rule set is unit
/// testable without a database, a request or a provider.
///
/// **Why this does not call String.Normalize.** The backend builds with
/// <c>InvariantGlobalization=true</c>, and in that mode .NET's <c>Normalize(FormKC)</c> is a silent
/// NO-OP for non-ASCII rather than an error: "ａｂ" (fullwidth) comes back unchanged, not as "ab".
/// Trusting it would leave an evasion hole big enough to register "ＡＤＭＩＮ". The folding below is
/// therefore explicit. That is less a workaround than the honest shape of the problem: NFKC never
/// maps Cyrillic "а" to Latin "a" either, and confusable detection is precisely what PW-US-018
/// asks for, so an explicit skeleton was always required.
///
/// **Two different outputs, deliberately.**
///   • <see cref="Display"/> preserves the participant's own script. An Arabic or Urdu name must
///     survive intact (PW-US-019) — folding it would be a worse failure than the abuse it prevents.
///   • <see cref="Fold"/> produces a lossy comparison skeleton used ONLY for duplicate, reserved and
///     impersonation checks. It is never shown to anyone and never stored as the display value.
/// </summary>
public static class CommunityNames
{
    public const int MinLength = 2;
    public const int MaxLength = 32;

    /// <summary>Outcome of validating a candidate display name. <c>ReasonCode</c> is null when
    /// accepted; <c>Suggestion</c> offers a usable alternative so the visitor is never left
    /// guessing (PW-US-020).</summary>
    public readonly record struct NameVerdict(bool Ok, string? ReasonCode, string? Display, string? Folded, string? Suggestion)
    {
        public static NameVerdict Accept(string display, string folded) => new(true, null, display, folded, null);
        public static NameVerdict Reject(string reason, string? suggestion = null) => new(false, reason, null, null, suggestion);
    }

    // ── Invisible and direction-controlling characters ────────────────────────────────────────
    // Zero-width and bidi controls let one string render identically to another, which defeats both
    // duplicate detection and any rule applied to the visible form. They carry no legitimate meaning
    // in a display name, so they are removed outright rather than folded. Written as escapes on
    // purpose: as raw characters they are invisible in a diff and unreviewable.
    // Listed as numeric code points, not as literal characters: written literally they are
    // invisible in the source and in any diff, which makes them unreviewable — and one of them
    // turned this very file into a "binary file" for grep on the first attempt.
    static readonly HashSet<char> InvisibleChars = new()
    {
        (char)0x200B, (char)0x200C, (char)0x200D,   // ZWSP, ZWNJ, ZWJ
        (char)0x200E, (char)0x200F,                 // LRM, RLM
        (char)0x202A, (char)0x202B, (char)0x202C,   // bidi embedding / pop
        (char)0x202D, (char)0x202E,                 // bidi override (LRO, RLO)
        (char)0x2066, (char)0x2067, (char)0x2068, (char)0x2069, // bidi isolates
        (char)0x2060, (char)0xFEFF, (char)0x00AD,   // word joiner, BOM/ZWNBSP, soft hyphen
        (char)0x180E, (char)0x034F, (char)0x061C,   // Mongolian vowel sep, CGJ, ALM
    };

    static bool IsInvisible(char c) => InvisibleChars.Contains(c);

    // NOTE: ZWNJ (U+200C) is genuinely meaningful in Persian and Urdu orthography, so stripping it
    // changes how a legitimate name renders. It is still removed, because it is equally the cheapest
    // way to mint a visually identical duplicate. The tradeoff is recorded rather than hidden: a
    // Persian or Urdu speaker sees their name joined, which stays legible, whereas an undetected
    // impersonation is not recoverable. Revisit if real users report it.

    /// <summary>The value shown to other participants: trimmed, whitespace-collapsed, stripped of
    /// invisible and control characters — but otherwise the participant's own characters, in their
    /// own script.</summary>
    public static string Display(string? raw)
    {
        if (string.IsNullOrEmpty(raw)) return "";
        var sb = new StringBuilder(raw.Length);
        var lastWasSpace = false;
        foreach (var c in raw)
        {
            if (IsInvisible(c)) continue;
            // Control and whitespace collapse to ONE space rather than vanishing, so "ad min"
            // cannot silently become "admin".
            if (char.IsControl(c) || char.IsWhiteSpace(c))
            {
                if (sb.Length > 0 && !lastWasSpace) { sb.Append(' '); lastWasSpace = true; }
                continue;
            }
            sb.Append(c);
            lastWasSpace = false;
        }
        return sb.ToString().Trim();
    }

    // ── Confusable folding ────────────────────────────────────────────────────────────────────
    // Homoglyphs that render close enough to a Latin letter to impersonate one. A practical
    // high-risk subset, not the whole of Unicode's confusables data: it covers the scripts an
    // attacker actually reaches for (Cyrillic, Greek), the compatibility forms NFKC would have
    // handled if it worked here (fullwidth), and leetspeak digits and symbols.
    static readonly Dictionary<char, char> Confusables = BuildConfusables();

    static Dictionary<char, char> BuildConfusables()
    {
        var m = new Dictionary<char, char>();
        void Map(string from, char to) { foreach (var c in from) m[c] = to; }

        // Cyrillic → Latin
        Map("аА", 'a');                    // а А
        Map("вВ", 'b');                    // в В
        Map("сСϲ", 'c');              // с С ϲ
        Map("ԁ", 'd');                          // ԁ
        Map("еЕёЁ", 'e');        // е Е ё Ё
        Map("нН", 'h');                    // н Н
        Map("іІїЇ", 'i');        // і І ї Ї
        Map("јЈ", 'j');                    // ј Ј
        Map("кК", 'k');                    // к К
        Map("мМ", 'm');                    // м М
        Map("пП", 'n');                    // п П
        Map("оО", 'o');                    // о О
        Map("рР", 'p');                    // р Р
        Map("яЯ", 'r');                    // я Я
        Map("ѕЅ", 's');                    // ѕ Ѕ
        Map("тТ", 't');                    // т Т
        Map("уУүҮ", 'y');        // у У ү Ү
        Map("хХ", 'x');                    // х Х

        // Greek → Latin
        Map("αΑ", 'a');                    // α Α
        Map("βΒ", 'b');                    // β Β
        Map("εΕ", 'e');                    // ε Ε
        Map("ηΗ", 'h');                    // η Η
        Map("ιΙ", 'i');                    // ι Ι
        Map("κΚ", 'k');                    // κ Κ
        Map("μΜ", 'm');                    // μ Μ
        Map("νΝ", 'v');                    // ν Ν
        Map("οΟσ", 'o');              // ο Ο σ
        Map("ρΡ", 'p');                    // ρ Ρ
        Map("τΤ", 't');                    // τ Τ
        Map("υΥγ", 'y');              // υ Υ γ
        Map("χΧ", 'x');                    // χ Χ
        Map("ζΖ", 'z');                    // ζ Ζ

        // Leetspeak digits and symbols. Aggressive on purpose: this skeleton exists to catch
        // "adm1n" and "@dmin". A false collision between "user1" and "userl" is a far cheaper
        // failure than an undetected staff impersonation.
        m['0'] = 'o'; m['3'] = 'e'; m['4'] = 'a'; m['5'] = 's';
        m['6'] = 'g'; m['7'] = 't'; m['8'] = 'b'; m['9'] = 'g';
        m['@'] = 'a'; m['$'] = 's'; m['+'] = 't';

        // The i/l/1 family collapses to a SINGLE symbol. "1" is genuinely ambiguous — it is a
        // credible homoglyph for both "l" and "i" — so no one-to-one mapping can catch both "adm1n"
        // (wants 1→i) and "wa1l" (wants 1→l). Collapsing i, l, 1, |, ! and ı onto one character
        // makes every member of the family compare equal instead of having to guess which was
        // intended: "admin" and "adm1n" both fold to "admln", so they match.
        m['i'] = 'l'; m['1'] = 'l'; m['|'] = 'l'; m['!'] = 'l';
        m['€'] = 'e'; m['£'] = 'l'; m['¡'] = 'i';   // € £ ¡

        // Latin letters carrying diacritics — the compatibility folding NFKC would have performed.
        Map("àáâãäåāăą", 'a');
        Map("çćĉċč", 'c');
        Map("ďđ", 'd');
        Map("èéêëēĕėęě", 'e');
        Map("ĝğġģ", 'g');
        Map("ĥħ", 'h');
        Map("ìíîïĩīĭįı", 'i');
        Map("ĵ", 'j');
        Map("ķ", 'k');
        Map("ĺļľŀł", 'l');
        Map("ñńņň", 'n');
        Map("òóôõöøōŏő", 'o');
        Map("ŕŗř", 'r');
        Map("śŝşš", 's');
        Map("ţťŧ", 't');
        Map("ùúûüũūŭůűų", 'u');
        Map("ŵ", 'w');
        Map("ýÿŷ", 'y');
        Map("źżž", 'z');

        // Resolve the map to a fixpoint. Entries were authored one hop at a time — Cyrillic "і"
        // maps to "i", and "i" in turn collapses into the i/l/1 family — so without this pass the
        // table would be inconsistent: a Cyrillic "і" would fold to "i" while a plain "i" folded to
        // "l", and the two would stop comparing equal. Chains are short and acyclic by construction;
        // the hop limit is a guard against a future edit introducing a cycle, not an expected path.
        foreach (var key in m.Keys.ToList())
        {
            var v = m[key];
            for (var hop = 0; hop < 8 && m.TryGetValue(v, out var next) && next != v; hop++) v = next;
            m[key] = v;
        }
        return m;
    }

    static char FoldChar(char c)
    {
        // Fullwidth ASCII (U+FF01–U+FF5E) maps to its ASCII twin by a fixed offset — the single most
        // common compatibility evasion, and exactly what the no-op Normalize failed to catch.
        if (c >= (char)0xFF01 && c <= (char)0xFF5E) c = (char)(c - 0xFEE0);   // fullwidth ! .. ~
        // Case-fold BEFORE the lookup, not after. The table is keyed on lowercase, so looking up
        // first meant every uppercase letter missed it and fell through unmapped: "ＡＤＭＩＮ" folded
        // to "admin" while "admin" folded to "admln", and the two stopped comparing equal — which
        // is precisely the impersonation this module exists to catch. Uppercase Cyrillic and Greek
        // keys still resolve, because their lowercase forms are in the table too.
        c = char.ToLowerInvariant(c);
        if (Confusables.TryGetValue(c, out var mapped)) return mapped;
        return c;
    }

    /// <summary>The lossy comparison skeleton. Never displayed, never stored as the name — used only
    /// to decide "is this the same as, or pretending to be, something else?".</summary>
    /// <summary>
    /// The same folding as <see cref="Fold"/>, but preserving word breaks: each whitespace-separated
    /// token is folded independently and rejoined with single spaces.
    ///
    /// Message moderation needs BOTH forms, because obfuscation runs in two directions. Padding a
    /// word out ("f u c k") is only caught by the fully collapsed skeleton; a term embedded in a
    /// longer legitimate word (the "Scunthorpe problem" — "classic", "assess", "Cockburn") is only
    /// avoided by matching on whole words. Checking a term against both, with word-boundary rules
    /// on this one, is what keeps the check strict without libelling ordinary English.
    /// </summary>
    public static string FoldWords(string? raw)
    {
        var display = Display(raw);
        if (display.Length == 0) return "";
        var parts = display.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var folded = new List<string>(parts.Length);
        foreach (var p in parts)
        {
            var f = Fold(p);
            if (f.Length > 0) folded.Add(f);
        }
        return string.Join(' ', folded);
    }

    public static string Fold(string? raw)
    {
        var display = Display(raw);
        if (display.Length == 0) return "";
        var sb = new StringBuilder(display.Length);
        foreach (var ch in display)
        {
            // Mathematical alphanumeric symbols (U+1D400–U+1D7FF) arrive as surrogate pairs; a lone
            // surrogate is meaningless, so they are dropped from the skeleton rather than mis-mapped.
            // They are separately rejected outright in Validate.
            if (char.IsSurrogate(ch)) continue;
            var f = FoldChar(ch);
            // Separators and punctuation are noise for comparison: "p c i", "p.c.i" and "pci" must
            // all collide, or the reserved-name list is bypassed by adding a dot.
            if (char.IsWhiteSpace(f) || char.IsPunctuation(f) || char.IsSymbol(f)) continue;
            sb.Append(f);
        }
        return sb.ToString();
    }

    /// <summary>Fold every entry of a readable wordlist into its comparison skeleton, once at
    /// startup. Entries that fold to nothing are dropped rather than silently matching everything.</summary>
    static string[] Foldset(string[] words) =>
        words.Select(Fold).Where(w => w.Length > 0).Distinct().ToArray();

    // ── Reserved and impersonation terms ──────────────────────────────────────────────────────
    // Authored in plain readable English, then run through Fold() once at startup so the comparison
    // happens skeleton-to-skeleton. This is what makes the lists robust rather than literal: the
    // word "admin" folds to the same skeleton as "adm1n", "ａｄｍｉｎ" and Cyrillic-"аdmin", so one
    // readable entry covers every disguise. Hard-coding the folded spelling instead would be
    // unreadable AND would silently rot the moment the fold table changes.
    static readonly string[] ReservedExact = Foldset(new[]
    {
        "pci", "pciworld", "pciadmin", "pciteam", "pcistaff", "pcisupport", "pciofficial",
        "admin", "administrator", "moderator", "mod", "staff", "support", "official",
        "system", "root", "owner", "operator", "helpdesk", "team", "host",
        "projectcontrolsinstitute", "certuvo",
    });

    // Substrings that claim an authority the participant does not have, wherever they appear.
    static readonly string[] ReservedSubstrings = Foldset(new[]
    {
        "pciadmin", "pcistaff", "pciteam", "pcisupport", "pcimod", "pciofficial", "pciworldadmin",
        "administrator", "moderator", "officialpci", "verifiedpci", "pciverified",
    });

    // Badge words implying platform-granted standing. Rejected only when the name is essentially
    // nothing BUT the claim ("Verified", "Official Support") — "Ali Verified Ltd" is a company name,
    // so a bare substring match would be too blunt.
    static readonly string[] BadgeWords = Foldset(new[] { "verified", "official", "staff", "support", "moderator", "admin" });

    static readonly string[] Profanity = Foldset(new[]
    {
        "fuck", "shit", "cunt", "bitch", "bastard", "asshole", "dickhead", "motherfucker",
        "nigger", "nigga", "faggot", "retard", "whore", "slut", "rape", "kike", "spic", "chink",
    });

    /// <summary>Contact-detail and link patterns. A guest room forbids contact exchange outright
    /// (§4.2), so a display name must not become the loophole for it.</summary>
    static bool LooksLikeContact(string display, string folded)
    {
        if (display.Contains('@') && display.Any(char.IsLetter)) return true;               // email-ish
        if (display.Contains("://", StringComparison.Ordinal)) return true;
        if (folded.Contains("http", StringComparison.Ordinal) || folded.Contains("www", StringComparison.Ordinal)
            || folded.Contains("com", StringComparison.Ordinal) && display.Contains('.')
            || folded.Contains("net", StringComparison.Ordinal) && display.Contains('.')
            || folded.Contains("org", StringComparison.Ordinal) && display.Contains('.')) return true;

        // A run of 7+ digits is a phone number in every format worth worrying about. Counted on the
        // ORIGINAL display value: the skeleton folds digits to letters, so it cannot see them.
        var run = 0;
        foreach (var c in display)
        {
            if (char.IsDigit(c)) { if (++run >= 7) return true; }
            else if (c is ' ' or '-' or '(' or ')' or '+' or '.') { /* separators do not break a run */ }
            else run = 0;
        }
        return false;
    }

    /// <summary>
    /// Validate a candidate display name. Pure: no database, no request, no clock. Duplicate
    /// detection against active sessions is the caller's job, enforced by a partial unique index on
    /// the folded value so a race cannot admit two (CCP_PHASE1_DESIGN §2.2).
    /// </summary>
    public static NameVerdict Validate(string? raw)
    {
        var display = Display(raw);
        if (display.Length == 0) return NameVerdict.Reject("name_required");
        if (display.Length < MinLength) return NameVerdict.Reject("name_too_short");
        if (display.Length > MaxLength) return NameVerdict.Reject("name_too_long", display[..MaxLength].Trim());

        // Characters that exist only to defeat comparison: surrogate pairs (the styled-Latin
        // mathematical alphanumeric block) and private-use areas.
        foreach (var c in display)
            if (char.IsSurrogate(c) || (c >= (char)0xE000 && c <= (char)0xF8FF))   // surrogate pair, or private-use area
                return NameVerdict.Reject("name_unsupported_characters");

        var folded = Fold(display);
        // An empty skeleton means the name is pure punctuation or symbols — unreadable as a label.
        if (folded.Length < MinLength) return NameVerdict.Reject("name_not_readable");

        foreach (var w in Profanity)
            if (folded.Contains(w, StringComparison.Ordinal)) return NameVerdict.Reject("name_offensive");

        if (LooksLikeContact(display, folded)) return NameVerdict.Reject("name_contact_details");

        foreach (var r in ReservedExact)
            if (folded == r) return NameVerdict.Reject("name_reserved");

        foreach (var r in ReservedSubstrings)
            if (folded.Contains(r, StringComparison.Ordinal)) return NameVerdict.Reject("name_impersonation");

        // Badge claim: the name is nothing but authority words strung together.
        var stripped = folded;
        foreach (var b in BadgeWords) stripped = stripped.Replace(b, "", StringComparison.Ordinal);
        if (stripped.Length == 0) return NameVerdict.Reject("name_misleading_badge");

        return NameVerdict.Accept(display, folded);
    }

    /// <summary>A safe, non-colliding alternative offered when the chosen name is already taken in
    /// this room. The suffix is caller-supplied so retry stays deterministic, and the candidate is
    /// re-validated so a suggestion can never itself be rejectable.</summary>
    public static string? SuggestAlternative(string display, int suffix)
    {
        var baseName = Display(display);
        var tail = suffix.ToString();
        if (baseName.Length + tail.Length + 1 > MaxLength)
            baseName = baseName[..Math.Max(MinLength, MaxLength - tail.Length - 1)].Trim();
        var candidate = $"{baseName} {tail}";
        return Validate(candidate).Ok ? candidate : null;
    }
}
