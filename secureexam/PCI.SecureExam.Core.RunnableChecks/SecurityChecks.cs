using PCI.SecureExam.Core;
public static class Run {
  static int pass=0, fail=0;
  static void Ck(string n, bool ok){ System.Console.WriteLine((ok?"PASS ":"FAIL ")+n); if(ok)pass++; else fail++; }
  public static void Main() {
    System.Console.WriteLine("=== DESKTOP SECURE-EXAM CORE — real logic execution ===\n");

    // --- Launch URI parsing (real Parse) ---
    var p = LaunchParameters.Parse("pciexam://start?code=PCI-ABC123&api=https%3A%2F%2Fapi.projectcontrolsinstitute.org&token=sess_9");
    Ck("parses full launch URI + url-decodes api", p.IsValid && p.Code=="PCI-ABC123" && p.ApiBaseUrl=="https://api.projectcontrolsinstitute.org" && p.Token=="sess_9");
    Ck("missing code → invalid, no throw", !LaunchParameters.Parse("pciexam://start?api=https://x").IsValid);
    foreach(var g in new string?[]{"", null, "not a uri", "pciexam://"})
      Ck($"garbage '{g}' → invalid", !LaunchParameters.Parse(g).IsValid);

    // --- THE security requirement: API host pinning (Section 15) ---
    var cfg = new ClientConfig(); // default allowlist: projectcontrolsinstitute.org, pci-global.org, localhost

    // 1. legit PCI subdomain over https → trusted
    Ck("https://exam.projectcontrolsinstitute.org TRUSTED", cfg.IsTrustedApi("https://exam.projectcontrolsinstitute.org"));
    // 2. MALICIOUS host in launch URI → rejected
    Ck("https://evil.com REJECTED", !cfg.IsTrustedApi("https://evil.com"));
    // 3. look-alike domain → rejected (suffix must be dot-anchored)
    Ck("https://projectcontrolsinstitute.org.evil.com REJECTED", !cfg.IsTrustedApi("https://projectcontrolsinstitute.org.evil.com"));
    // 4. http (plaintext) to a real host → rejected (no plaintext exam traffic)
    Ck("http:// (plaintext) REJECTED", !cfg.IsTrustedApi("http://exam.projectcontrolsinstitute.org"));
    // 5. substring-but-not-subdomain → rejected
    Ck("https://notprojectcontrolsinstitute.org REJECTED", !cfg.IsTrustedApi("https://notprojectcontrolsinstitute.org"));

    // --- WithLaunch: malicious launch api is IGNORED, trusted config stands ---
    var c2 = new ClientConfig { ApiBaseUrl = "https://exam.projectcontrolsinstitute.org" };
    c2.WithLaunch(LaunchParameters.Parse("pciexam://start?code=X&api=https://attacker.example"));
    Ck("malicious launch api IGNORED, trusted config kept", c2.ApiBaseUrl=="https://exam.projectcontrolsinstitute.org");

    // --- WithLaunch: a TRUSTED launch api override is accepted ---
    var c3 = new ClientConfig { ApiBaseUrl = "https://exam.projectcontrolsinstitute.org" };
    c3.WithLaunch(LaunchParameters.Parse("pciexam://start?code=X&api=https://staging.pci-global.org"));
    Ck("trusted launch override accepted", c3.ApiBaseUrl=="https://staging.pci-global.org");

    // --- EnsureTrustedOrThrow: refuses to start on untrusted endpoint ---
    bool threw=false;
    try { new ClientConfig { ApiBaseUrl="https://evil.com" }.EnsureTrustedOrThrow(); } catch { threw=true; }
    Ck("EnsureTrustedOrThrow THROWS on untrusted endpoint", threw);
    bool ok2=true;
    try { new ClientConfig { ApiBaseUrl="https://exam.projectcontrolsinstitute.org" }.EnsureTrustedOrThrow(); } catch { ok2=false; }
    Ck("EnsureTrustedOrThrow allows trusted endpoint", ok2);

    System.Console.WriteLine($"\n{pass} passing, {fail} failing");
    System.Environment.Exit(fail>0?1:0);
  }
}
