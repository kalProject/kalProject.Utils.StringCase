# Contributing

Thanks for your interest in contributing!

## Development setup

- .NET SDK 8.x installed (CI uses 8.x)
- Windows is recommended for .NET Framework targets (net472/net48)

## Build & test

```bash
# Restore
dotnet restore

# Build all projects
dotnet build kalProject.Utils.StringCase.sln -c Debug

# Run tests
dotnet test kalProject.Utils.StringCase.sln -c Debug
```

## Branching & PRs

- Create feature branches from `main`
- Keep PRs small and focused
- Include tests for new behavior

## Versioning & Publishing

- Package is published to GitHub Packages by pushing a tag starting with `v` (e.g., `v1.2.3`)
- The tag version becomes the `PackageVersion` used by `dotnet pack`

## Code style

- C# latest, Nullable enabled
- Prefer clear naming and concise methods

## Reporting issues

Open an issue with a minimal reproduction and expected/actual behavior. Logs and environment info help.
