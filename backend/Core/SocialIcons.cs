using System.Text.RegularExpressions;

namespace PCI.Backend.Core;

/// <summary>
/// The social-media platform registry: a fixed catalogue of supported networks with a bundled,
/// self-contained monochrome SVG glyph (no remote icon dependency — nothing can 404 or be blocked),
/// the network's brand colour (admin preview only), and its set of approved domains for URL
/// validation. A platform not in the catalogue (or the explicit "custom" platform) is fully
/// supported via an uploaded custom SVG or a neutral link fallback, so new platforms need no code
/// change to appear. Icons are single-colour and inherit `currentColor`, so they read correctly on
/// both light and dark backgrounds.
///
/// This class also owns the two safety gates the public website relies on:
///   • <see cref="ValidateUrl"/> — scheme/domain validation used by the admin write path.
///   • <see cref="SafeCustomIcon"/> — sanitises an admin-supplied SVG before it is ever emitted.
/// </summary>
public static class SocialIcons
{
    public sealed record Platform(string Key, string Label, string Color, string[] Domains, string Glyph);

    /// <summary>A public display location. `footer` is rendered by ListSections today; the rest are
    /// data-ready placements an account can target (the model never hardcodes where an icon appears).</summary>
    public static readonly (string Key, string Label)[] Locations =
    {
        ("footer", "Footer"), ("header", "Header"), ("desktop_nav", "Desktop navigation"),
        ("mobile_nav", "Mobile navigation"), ("contact", "Contact Us page"), ("about", "About PCI page"),
        ("homepage", "Homepage"), ("blog", "Blog pages"), ("news", "News pages"),
        ("certification", "Certification pages"), ("resources", "Candidate resource pages"),
        ("support", "Support page"), ("downloads", "Public Downloads"), ("partner", "Marketing-partner pages"),
        ("institution", "Institution pages"), ("floating_bar", "Floating social bar"),
    };

    // Page-sharing targets (kept separate from PCI profile links, per requirement).
    public static readonly (string Key, string Label)[] ShareTargets =
    {
        ("linkedin", "LinkedIn"), ("x", "X"), ("facebook", "Facebook"), ("whatsapp", "WhatsApp"),
        ("telegram", "Telegram"), ("reddit", "Reddit"), ("email", "Email"), ("copy", "Copy link"),
    };

    // ── glyphs (self-authored, viewBox 0 0 24 24, currentColor) ──────────────────────────────────
    // Kept deliberately simple and single-path/element where possible so they scale cleanly and never
    // render as a broken image. Elements that need an outline set stroke explicitly.
    const string GLinkedin = "<circle cx='5' cy='5' r='2.2'/><rect x='3' y='9' width='4' height='12'/><path d='M10 9h3.8v1.7h.06c.6-1.05 1.9-2 3.74-2 3.9 0 4.6 2.4 4.6 5.7V21h-4v-4.9c0-1.2 0-2.7-1.7-2.7s-1.9 1.3-1.9 2.6V21h-4z'/>";
    const string GFacebook = "<path d='M14 8.5V7c0-.8.2-1.3 1.4-1.3H17V2.3C16.4 2.2 15.5 2 14.4 2 11.9 2 10.3 3.5 10.3 6.3v2.2H7.5V12h2.8v9h3.7v-9h2.8l.5-3.5H14z'/>";
    const string GInstagram = "<rect x='3' y='3' width='18' height='18' rx='5' fill='none' stroke='currentColor' stroke-width='2'/><circle cx='12' cy='12' r='4' fill='none' stroke='currentColor' stroke-width='2'/><circle cx='17.4' cy='6.6' r='1.3'/>";
    const string GX = "<path d='M17.7 3H21l-7.2 8.2L22.3 21H16l-4.6-6-5.3 6H2.8l7.7-8.8L1.9 3h6.4l4.2 5.5L17.7 3zm-1.1 16h1.8L7.5 4.9H5.6z'/>";
    const string GYoutube = "<path d='M22 8.2s-.2-1.4-.8-2c-.8-.8-1.7-.8-2.1-.9C16.3 5 12 5 12 5s-4.3 0-7.1.3c-.4.1-1.3.1-2.1.9-.6.6-.8 2-.8 2S1.8 9.8 1.8 11.5v1c0 1.7.2 3.3.2 3.3s.2 1.4.8 2c.8.8 1.8.8 2.3.9C6.8 19 12 19 12 19s4.3 0 7.1-.3c.4-.1 1.3-.1 2.1-.9.6-.6.8-2 .8-2s.2-1.6.2-3.3v-1c0-1.7-.2-3.3-.2-3.3zM9.9 14.6V9.4l4.6 2.6-4.6 2.6z'/>";
    const string GTiktok = "<path d='M14 3c.3 2 1.5 3.8 3.4 4.5.7.3 1.5.4 2.3.4v3c-1.4 0-2.8-.4-4-1.1v5.6c0 3-2.4 5.6-5.5 5.6S4.7 18.4 4.7 15.4 7.1 9.8 10.2 9.8c.3 0 .6 0 .9.1v3.1c-.3-.1-.6-.2-.9-.2-1.4 0-2.5 1.1-2.5 2.6s1.1 2.6 2.5 2.6 2.6-1.1 2.6-2.6V3H14z'/>";
    const string GThreads = "<path d='M12 2a10 10 0 100 20c2.5 0 4-.8 5.2-1.9l-1.3-1.5C15 19.4 13.7 20 12 20a8 8 0 118-8c0 1.6-.6 2.8-1.7 2.8-.8 0-1.3-.5-1.3-1.6V8h-2v1.2A3.8 3.8 0 008 12a4 4 0 007 2.6c.5 1 1.4 1.5 2.6 1.5 2.3 0 3.4-2 3.4-4.1A10 10 0 0012 2zm0 6a2 2 0 110 4 2 2 0 010-4z'/>";
    const string GWhatsapp = "<path d='M12 2a10 10 0 00-8.6 15L2 22l5.2-1.4A10 10 0 1012 2zm0 2a8 8 0 11-4.2 14.8l-.4-.2-2.4.6.6-2.3-.3-.4A8 8 0 0112 4zm-3 4.4c-.2 0-.5.1-.7.4-.3.3-.9.9-.9 2.2s1 2.6 1.1 2.7c.1.2 2 3 4.7 4.1 2.2.9 2.7.7 3.2.7.6-.1 1.7-.7 2-1.4.2-.7.2-1.2.1-1.4l-1.9-.9c-.3-.1-.5-.2-.7.1l-.7 1c-.2.1-.3.2-.6 0-.3-.1-1.1-.4-2-1.2-.7-.6-1.2-1.4-1.4-1.7-.1-.3 0-.4.1-.6l.5-.5c.1-.2.2-.3.3-.5s0-.4-.1-.5l-.9-2.1c-.2-.5-.4-.4-.6-.4z'/>";
    const string GTelegram = "<path d='M21.9 4.3L2.9 11.6c-.9.4-.9 1.5 0 1.8l4.7 1.5 1.8 5.7c.2.6.9.8 1.4.4l2.6-2.1 4.6 3.4c.6.4 1.4.1 1.6-.6l3.3-16c.2-.9-.7-1.6-1.6-1.4zM8.9 14.2l8.6-5.4c.2-.1.4.2.2.3l-7 6.3-.3 3-1.5-4.2z'/>";
    const string GSnapchat = "<path d='M12 3c2.6 0 4.4 2 4.5 4.6 0 .8.1 1.6.4 2.2.3.5.9.8 1.5.9.4.1.7.3.7.6 0 .4-.4.6-.9.8-.6.2-1.2.4-1.2 1 0 .9 1.6 2.1 2.6 2.4-.1.6-1.4.9-2.6 1-.3.6-.4 1.1-.9 1.2-.4.1-1-.1-1.6-.1-.7 0-1.3.4-1.9.7-.4.2-.8.4-1.1.4s-.7-.2-1.1-.4c-.6-.3-1.2-.7-1.9-.7-.6 0-1.2.2-1.6.1-.5-.1-.6-.6-.9-1.2-1.2-.1-2.5-.4-2.6-1 1-.3 2.6-1.5 2.6-2.4 0-.6-.6-.8-1.2-1-.5-.2-.9-.4-.9-.8 0-.3.3-.5.7-.6.6-.1 1.2-.4 1.5-.9.3-.6.4-1.4.4-2.2C7.6 5 9.4 3 12 3z'/>";
    const string GPinterest = "<path d='M12 2a10 10 0 00-3.6 19.3c-.1-.8-.1-2 .1-2.9l1.1-4.7s-.3-.6-.3-1.4c0-1.3.8-2.3 1.7-2.3.8 0 1.2.6 1.2 1.3 0 .8-.5 2-.8 3.2-.2 1 .5 1.7 1.4 1.7 1.7 0 2.9-2.2 2.9-4.7 0-1.9-1.3-3.4-3.7-3.4-2.7 0-4.3 2-4.3 4.1 0 .8.3 1.7.7 2.1a.3.3 0 01.1.3l-.3 1.1c0 .2-.1.2-.3.1-1.2-.5-1.9-2.2-1.9-3.6 0-2.9 2.1-5.6 6.1-5.6 3.2 0 5.7 2.3 5.7 5.3 0 3.2-2 5.8-4.8 5.8-.9 0-1.8-.5-2.1-1.1l-.6 2.2c-.2.8-.8 1.9-1.2 2.5A10 10 0 1012 2z'/>";
    const string GReddit = "<path d='M22 12c0-1.1-.9-2-2-2-.5 0-1 .2-1.3.5-1.3-.9-3-1.4-4.9-1.5l.8-3.9 2.7.6c0 .7.6 1.3 1.3 1.3s1.4-.6 1.4-1.4-.6-1.3-1.4-1.3c-.5 0-1 .3-1.2.8l-3-.7c-.2 0-.4.1-.4.3l-1 4.3c-1.9.1-3.6.6-4.9 1.5-.4-.3-.8-.5-1.3-.5-1.1 0-2 .9-2 2 0 .8.5 1.5 1.1 1.8v.6c0 2.8 3.2 5 7.1 5s7.1-2.2 7.1-5v-.6c.6-.3 1.1-1 1.1-1.8zM8 13.5A1.3 1.3 0 119.3 15 1.3 1.3 0 018 13.5zm7.8 3.3c-.9.9-2.6 1-3.8 1s-2.9-.1-3.8-1a.4.4 0 01.6-.6c.6.6 1.8.8 3.2.8s2.6-.2 3.2-.8a.4.4 0 01.6.6zm-.1-2a1.3 1.3 0 111.3-1.3 1.3 1.3 0 01-1.3 1.3z'/>";
    const string GDiscord = "<path d='M19.5 5.6A16 16 0 0015.4 4l-.3.5c1.4.4 2 .9 2.9 1.5A11 11 0 007.9 6c.8-.6 1.5-1.1 2.9-1.5L10.5 4c-1.5.3-3 .9-4.1 1.6C4 9 3.3 12.4 3.6 15.7A16 16 0 008.3 18l.6-1c-.8-.3-1.5-.6-2.1-1l.5-.4c2 .9 4.2 1.4 6.1 1.4s4.1-.5 6.1-1.4l.5.4c-.6.4-1.3.7-2.1 1l.6 1a16 16 0 004.7-2.3c.4-3.9-.6-7.3-3.2-10.1zM9.5 14c-.8 0-1.4-.7-1.4-1.6s.6-1.6 1.4-1.6 1.4.7 1.4 1.6S10.3 14 9.5 14zm5 0c-.8 0-1.4-.7-1.4-1.6s.6-1.6 1.4-1.6 1.4.7 1.4 1.6S15.3 14 14.5 14z'/>";
    const string GGithub = "<path d='M12 2a10 10 0 00-3.2 19.5c.5.1.7-.2.7-.5v-1.7c-2.8.6-3.4-1.3-3.4-1.3-.5-1.2-1.1-1.5-1.1-1.5-.9-.6.1-.6.1-.6 1 .1 1.5 1 1.5 1 .9 1.5 2.3 1.1 2.9.8.1-.6.3-1.1.6-1.3-2.2-.3-4.6-1.1-4.6-5 0-1.1.4-2 1-2.7-.1-.3-.4-1.3.1-2.7 0 0 .8-.3 2.7 1a9.4 9.4 0 015 0c1.9-1.3 2.7-1 2.7-1 .5 1.4.2 2.4.1 2.7.6.7 1 1.6 1 2.7 0 3.9-2.4 4.7-4.6 5 .3.3.7.9.7 1.9v2.8c0 .3.2.6.7.5A10 10 0 0012 2z'/>";
    const string GMedium = "<path d='M4 6.5c0-.2 0-.3-.2-.5L2.5 4.6v-.2H6l3 6.6 2.7-6.6H15v.2l-1 1c-.1.1-.2.2-.2.4v7.6c0 .2.1.3.2.4l1 1v.2h-5v-.2l1-1c.1-.1.1-.2.1-.4V6.3l-2.9 7.1h-.4L2.6 6.3v4.8c0 .3 0 .5.2.7l1.4 1.7v.2H1.5v-.2l1.4-1.7c.2-.2.2-.4.2-.7V6.5z'/>";
    const string GVimeo = "<path d='M22 8c-.1 1.9-1.4 4.5-4 7.8-2.6 3.4-4.9 5.1-6.7 5.1-1.1 0-2.1-1-2.9-3.1L6 11.9c-.6-2.1-1.2-3.1-1.9-3.1-.1 0-.6.3-1.5.9L2 8.5c1-.8 1.9-1.7 2.8-2.5C6.1 4.8 7 4.3 7.7 4.2c1.5-.1 2.5.9 2.8 3.1.4 2.4.7 3.9.8 4.5.5 2 1 3 1.6 3 .4 0 1-.6 1.9-1.9.8-1.3 1.3-2.3 1.3-3 .1-1.1-.3-1.7-1.3-1.7-.5 0-1 .1-1.4.3 1-3.1 2.8-4.7 5.5-4.6 2 .1 2.9 1.4 2.8 3.9z'/>";
    const string GWechat = "<path d='M9 4C4.9 4 1.6 6.8 1.6 10.2c0 1.9 1 3.6 2.7 4.7L3.5 17l2.4-1.2c.6.2 1.2.3 1.9.4-.2-.6-.3-1.2-.3-1.8 0-3.3 3.2-5.8 7-5.8h.4C14.3 5.9 11.9 4 9 4zM6.4 8.6a1 1 0 110 2 1 1 0 010-2zm5.2 0a1 1 0 110 2 1 1 0 010-2z'/><path d='M22.4 14.4c0-2.8-2.8-5.1-6.2-5.1S10 11.6 10 14.4s2.8 5.1 6.2 5.1c.7 0 1.4-.1 2-.3l1.9 1-.5-1.7c1.7-1 2.8-2.5 2.8-4.1zm-8.1-1.2a.9.9 0 110 1.8.9.9 0 010-1.8zm3.8 0a.9.9 0 110 1.8.9.9 0 010-1.8z'/>";
    const string GGlobe = "<path d='M12 2a10 10 0 100 20 10 10 0 000-20zm6.9 6h-2.9a15 15 0 00-1.3-3.4A8 8 0 0118.9 8zM12 4c.8 1 1.5 2.4 1.9 4h-3.8c.4-1.6 1.1-3 1.9-4zM4.3 14a7.8 7.8 0 010-4h3.3a17 17 0 000 4H4.3zm.8 2h2.9c.3 1.3.8 2.4 1.3 3.4A8 8 0 015.1 16zm2.9-8H5.1a8 8 0 014.2-3.4A15 15 0 008 8zm4 12c-.8-1-1.5-2.4-1.9-4h3.8c-.4 1.6-1.1 3-1.9 4zm2.3-6H9.7a15 15 0 010-4h4.6a15 15 0 010 4zm.3 5.4c.5-1 1-2.1 1.3-3.4h2.9a8 8 0 01-4.2 3.4zM16.4 14a17 17 0 000-4h3.3a7.8 7.8 0 010 4h-3.3z'/>";
    const string GLink = "<path d='M7 12a3 3 0 013-3h3V7h-3a5 5 0 000 10h3v-2h-3a3 3 0 01-3-3zm2 1h6v-2H9v2zm5-6h-3v2h3a3 3 0 010 6h-3v2h3a5 5 0 000-10z'/>";
    const string GEmail = "<path d='M4 4h16a2 2 0 012 2v12a2 2 0 01-2 2H4a2 2 0 01-2-2V6a2 2 0 012-2zm8 7L4 6.2V7l8 5 8-5v-.8L12 11z'/>";

    /// <summary>The catalogue, in default display order. `website` and `custom` carry no domain
    /// restriction (any https URL is accepted).</summary>
    public static readonly Platform[] All =
    {
        new("linkedin",  "LinkedIn",  "#0A66C2", new[]{ "linkedin.com", "lnkd.in" }, GLinkedin),
        new("facebook",  "Facebook",  "#1877F2", new[]{ "facebook.com", "fb.com", "fb.me" }, GFacebook),
        new("instagram", "Instagram", "#E4405F", new[]{ "instagram.com", "instagr.am" }, GInstagram),
        new("x",         "X (Twitter)","#000000",new[]{ "x.com", "twitter.com", "t.co" }, GX),
        new("youtube",   "YouTube",   "#FF0000", new[]{ "youtube.com", "youtu.be" }, GYoutube),
        new("tiktok",    "TikTok",    "#000000", new[]{ "tiktok.com" }, GTiktok),
        new("threads",   "Threads",   "#000000", new[]{ "threads.net", "threads.com" }, GThreads),
        new("whatsapp",  "WhatsApp",  "#25D366", new[]{ "wa.me", "whatsapp.com", "api.whatsapp.com", "chat.whatsapp.com" }, GWhatsapp),
        new("telegram",  "Telegram",  "#26A5E4", new[]{ "t.me", "telegram.me", "telegram.org" }, GTelegram),
        new("snapchat",  "Snapchat",  "#FFFC00", new[]{ "snapchat.com" }, GSnapchat),
        new("pinterest", "Pinterest", "#BD081C", new[]{ "pinterest.", "pin.it" }, GPinterest),
        new("reddit",    "Reddit",    "#FF4500", new[]{ "reddit.com", "redd.it" }, GReddit),
        new("discord",   "Discord",   "#5865F2", new[]{ "discord.gg", "discord.com", "discordapp.com" }, GDiscord),
        new("github",    "GitHub",    "#181717", new[]{ "github.com" }, GGithub),
        new("medium",    "Medium",    "#000000", new[]{ "medium.com" }, GMedium),
        new("vimeo",     "Vimeo",     "#1AB7EA", new[]{ "vimeo.com" }, GVimeo),
        new("wechat",    "WeChat",    "#07C160", new[]{ "weixin.qq.com", "wechat.com" }, GWechat),
        new("website",   "Website / community", "#334155", Array.Empty<string>(), GGlobe),
        new("custom",    "Custom platform",     "#334155", Array.Empty<string>(), GLink),
    };

    static readonly Dictionary<string, Platform> ByKey =
        All.ToDictionary(p => p.Key, StringComparer.OrdinalIgnoreCase);

    public static Platform? Get(string? key) => key is not null && ByKey.TryGetValue(key, out var p) ? p : null;

    /// <summary>Inline SVG for an account. Custom icon type uses the sanitised uploaded SVG; a builtin
    /// (or unknown) platform uses its catalogue glyph, falling back to the neutral link mark.</summary>
    public static string Svg(string platform, string? iconType, string? customIcon, int size = 20)
    {
        if (string.Equals(iconType, "custom", StringComparison.OrdinalIgnoreCase))
        {
            var safe = SafeCustomIcon(customIcon);
            if (safe is not null) return safe;
        }
        var glyph = Get(platform)?.Glyph ?? GLink;
        return $"<svg viewBox=\"0 0 24 24\" width=\"{size}\" height=\"{size}\" fill=\"currentColor\" aria-hidden=\"true\" focusable=\"false\">{glyph}</svg>";
    }

    // ── URL validation ───────────────────────────────────────────────────────────────────────────
    public sealed record UrlCheck(bool Ok, string Url, string? Error, string? Warning);

    /// <summary>Validate a profile URL for a platform. Hard-fails (Ok=false) on a non-http(s) scheme,
    /// an unparseable URL, or an embedded scriptable scheme; returns a soft Warning for a plain-http
    /// URL or a host that does not match the platform's approved domains (the caller decides whether a
    /// documented override may publish anyway). The returned Url is trimmed and normalised.</summary>
    public static UrlCheck ValidateUrl(string platform, string? raw)
    {
        var url = (raw ?? "").Trim();
        if (url.Length == 0) return new(false, url, "A profile URL is required.", null);

        // Block scriptable / data schemes outright (defence-in-depth; the renderer also strips these).
        var probe = new string(url.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray()).ToLowerInvariant();
        if (probe.StartsWith("javascript:") || probe.StartsWith("vbscript:") || probe.StartsWith("data:") || probe.StartsWith("file:"))
            return new(false, url, "That link uses an unsafe scheme and cannot be saved.", null);

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != "http" && uri.Scheme != "https"))
            return new(false, url, "Enter a full https:// web address.", null);

        string? warning = uri.Scheme == "http" ? "This link is not HTTPS — use a secure https:// address where the platform supports it." : null;

        var plat = Get(platform);
        if (plat is { Domains.Length: > 0 })
        {
            var host = uri.Host.ToLowerInvariant();
            var match = plat.Domains.Any(d => d.EndsWith(".") ? host.Contains(d.TrimEnd('.')) : (host == d || host.EndsWith("." + d)));
            if (!match)
                warning = $"This URL's domain ({uri.Host}) does not look like an official {plat.Label} address.";
        }
        return new(true, url, null, warning);
    }

    // ── custom SVG sanitisation ──────────────────────────────────────────────────────────────────
    static readonly Regex RxScript = new(@"<script[\s\S]*?</script\s*>|<!--[\s\S]*?-->", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxOn = new(@"\son[a-z]+\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static readonly Regex RxBadHref = new(@"(href|xlink:href)\s*=\s*(""[^""]*""|'[^']*'|[^\s>]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>Return a safe inline SVG built from an admin-supplied custom icon, or null if it cannot
    /// be made safe. Accepts either a full &lt;svg&gt;…&lt;/svg&gt; element or a bare path `d` string.
    /// Strips scripts, comments, event handlers and href attributes; rejects anything still carrying a
    /// scriptable scheme.</summary>
    public static string? SafeCustomIcon(string? raw)
    {
        var s = (raw ?? "").Trim();
        if (s.Length == 0 || s.Length > 20000) return null;

        // A bare path definition → wrap it in a known-good svg.
        if (!s.Contains('<') && Regex.IsMatch(s, @"^[Mm][\s0-9.,\-]"))
            return $"<svg viewBox=\"0 0 24 24\" width=\"20\" height=\"20\" fill=\"currentColor\" aria-hidden=\"true\" focusable=\"false\"><path d=\"{s.Replace("\"", "")}\"/></svg>";

        if (!s.Contains("<svg", StringComparison.OrdinalIgnoreCase)) return null;
        var cleaned = RxScript.Replace(s, "");
        cleaned = RxOn.Replace(cleaned, "");
        cleaned = RxBadHref.Replace(cleaned, "");
        var low = cleaned.ToLowerInvariant();
        if (low.Contains("javascript:") || low.Contains("vbscript:") || low.Contains("<script") || low.Contains("<foreignobject") || low.Contains("<iframe"))
            return null;
        return cleaned;
    }
}
