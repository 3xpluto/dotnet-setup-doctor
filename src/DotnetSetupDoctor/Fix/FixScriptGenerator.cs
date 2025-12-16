namespace DotnetSetupDoctor.Fix;

public static class FixScriptGenerator
{
    public static string GeneratePowerShell()
    {
        return
"""
# dotnet-setup-doctor suggested fix script (PowerShell)
# Review before running. Run in an elevated PowerShell if needed.

$ErrorActionPreference = "Stop"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

Write-Host "== Recommended installs =="

# winget comes from App Installer (Store). If winget is missing, install/update App Installer.
Write-Host "If 'winget' is missing: install/update 'App Installer' from Microsoft Store."

# .NET SDK 8 (LTS)
Write-Host "Installing .NET SDK 8..."
winget install --id Microsoft.DotNet.SDK.8 -e --source winget

# Git
Write-Host "Installing Git..."
winget install --id Git.Git -e --source winget

Write-Host ""
Write-Host "== Visual Studio Build Tools / MSBuild =="
Write-Host "If you need MSBuild for classic .csproj / native workloads:"
Write-Host "  - Install Visual Studio Build Tools (or Visual Studio) and include MSBuild components."
Write-Host "  - Official download: https://visualstudio.microsoft.com/downloads/"

Write-Host ""
Write-Host "== Windows settings (optional but recommended) =="
Write-Host "Enable Win32 long paths (prevents path-too-long build errors)."
Write-Host "Enable Developer Mode (helps with dev features like symlinks)."

Write-Host ""
Write-Host "After installs/settings:"
Write-Host "  - Close and reopen your terminal so PATH updates."
Write-Host "  - Re-run: dotnet-setup-doctor doctor"
""";
    }
}
