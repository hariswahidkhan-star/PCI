# Runnable Core security checks (no packages, no Windows required)

The desktop **App** is Windows-only (WPF + OpenCV + NAudio) and cannot be built or run on Linux/CI.
But `PCI.SecureExam.Core` has **zero package dependencies**, so its security-critical logic — launch-URI
parsing and **API-host pinning** (Section 15: the client must refuse an untrusted API host) — can be
compiled and executed anywhere.

`SecurityChecks.cs` executes the real `LaunchParameters.Parse`, `ClientConfig.IsTrustedApi`,
`WithLaunch`, and `EnsureTrustedOrThrow` against attack cases (malicious host, look-alike domain, plaintext,
substring-not-subdomain, malicious launch-URI override). It printed **15/15 passing** when last run.

To run it standalone (Linux/macOS/Windows, offline):

    mkdir /tmp/corecheck && cd /tmp/corecheck
    cp ../PCI.SecureExam.Core/LaunchParameters.cs ../PCI.SecureExam.Core/ClientConfig.cs .
    cp ../PCI.SecureExam.Core.RunnableChecks/SecurityChecks.cs .
    printf '%s' '<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable><ImplicitUsings>enable</ImplicitUsings><OutputType>Exe</OutputType></PropertyGroup></Project>' > c.csproj
    dotnet run -c Release

The authoritative xUnit versions live in `PCI.SecureExam.Tests/` and run on a machine with NuGet access
via `dotnet test`.
