# dotnet-setup-doctor

A Windows/.NET environment “doctor” CLI — audits **.NET SDK**, **MSBuild / Visual Studio Build Tools**, **Git**, **NuGet**, plus basic **PATH/env-var** sanity checks.  
Outputs a friendly console report (✅/⚠️/❌) or machine-readable JSON.

## Commands

### `doctor` (default)
Run environment checks and print a report:

```powershell
dotnet-setup-doctor doctor
dotnet-setup-doctor doctor --json
```

### `fix-script`
Print a **PowerShell** script with recommended installs/fixes (it does not auto-run anything; you review then run):

```powershell
dotnet-setup-doctor fix-script
```

## Build & run from source (local dev)

From the repo root:

```powershell
dotnet --info
dotnet restore
dotnet build -c Release
dotnet test -c Release
dotnet run --project .\src\DotnetSetupDoctor -- doctor
dotnet run --project .\src\DotnetSetupDoctor -- doctor --json
dotnet run --project .\src\DotnetSetupDoctor -- fix-script
```

## Pack as a .NET tool (local)

Create a local NuGet package:

```powershell
dotnet pack .\src\DotnetSetupDoctor\DotnetSetupDoctor.csproj -c Release -o .\nupkg
```

Install it globally from the local `nupkg` folder:

```powershell
dotnet tool install -g dotnet-setup-doctor --add-source .\nupkg
```

Run it:

```powershell
dotnet-setup-doctor doctor
```

Update it after changes (re-pack first):

```powershell
dotnet tool update -g dotnet-setup-doctor --add-source .\nupkg
```

Uninstall:

```powershell
dotnet tool uninstall -g dotnet-setup-doctor
```

## GitHub Actions CI

A Windows CI workflow can:
- restore/build/test
- pack the tool
- run a smoke check (`doctor --json`)

(See `.github/workflows/ci.yml`.)

## Notes / Troubleshooting

- If installs happen during testing (SDK/Git/VS Build Tools), **close & reopen your terminal** so PATH changes apply.
- MSBuild detection is best-effort:
  - If `msbuild` is on PATH, it will be used.
  - Otherwise it tries `vswhere.exe` to locate a VS install with MSBuild components.
