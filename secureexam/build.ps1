#requires -Version 5
<#
  PCI Secure Exam — one-shot build & test.
  Usage:   ./build.ps1            # restore, build, test
           ./build.ps1 -Run       # also launch the client (Windows)
           ./build.ps1 -SelfTest  # build then run the machine self-test
           ./build.ps1 -Publish   # produce the self-contained single-file .exe (win-x64)
#>
param([switch]$Run, [switch]$SelfTest, [switch]$Publish)
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
if ($Publish) {
  Write-Host "== publish (self-contained single-file win-x64) ==" -ForegroundColor Cyan
  dotnet publish PCI.SecureExam.App -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
  $exe = "PCI.SecureExam.App/bin/Release/net8.0-windows/win-x64/publish/PCISecureExam.exe"
  if (Test-Path $exe) { Write-Host "Published: $exe" -ForegroundColor Green }
  else { Write-Warning "Expected exe not found at $exe — check the publish output above." }
}
if ($Run) {
  Write-Host "== run client ==" -ForegroundColor Cyan
  $env:PCI_LAUNCH = "pciexam://start?code=PCIDEMO12345&api=http://localhost:5000&token=demo"
  dotnet run --project PCI.SecureExam.App -c Release
}
Write-Host "Done." -ForegroundColor Green
