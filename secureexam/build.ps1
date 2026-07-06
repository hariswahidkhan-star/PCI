#requires -Version 5
<#
  PCI Secure Exam — one-shot build & test.
  Usage:   ./build.ps1            # restore, build, test
           ./build.ps1 -Run       # also launch the client (Windows)
           ./build.ps1 -SelfTest  # build then run the machine self-test
#>
param([switch]$Run, [switch]$SelfTest)
$ErrorActionPreference = 'Stop'
Write-Host "== restore ==" -ForegroundColor Cyan
dotnet restore PCI.SecureExam.sln
Write-Host "== build (Release) ==" -ForegroundColor Cyan
dotnet build PCI.SecureExam.sln -c Release --no-restore
Write-Host "== test ==" -ForegroundColor Cyan
dotnet test PCI.SecureExam.Tests/PCI.SecureExam.Tests.csproj -c Release --no-build
if ($SelfTest) {
  Write-Host "== self-test ==" -ForegroundColor Cyan
  dotnet run --project PCI.SecureExam.App -c Release -- --selftest
}
if ($Run) {
  Write-Host "== run client ==" -ForegroundColor Cyan
  $env:PCI_LAUNCH = "pciexam://start?code=PCIDEMO12345&api=http://localhost:5000&token=demo"
  dotnet run --project PCI.SecureExam.App -c Release
}
Write-Host "Done." -ForegroundColor Green
