# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Solution

```
Sah.Claude.Usage.Wpf.slnx          ← .NET 10 XML solution format (.slnx, not .sln)
src/
  Sah.Claude.Usage.Wpf/            ← WPF application (net10.0-windows)
test/
  Sah.Claude.Usage.Wpf.UnitTests/      ← Unit tests: instantiates and calls classes (TUnit, net10.0-windows)
  Sah.Claude.Usage.Wpf.IntegrationTests/ ← UI tests: simulates user interactions (TUnit + FlaUI.UIA3, net10.0-windows)
```

## Testing

Existing tests should pass without modification for new code.
Existing tests failing for new code are communicated to the user so they can decide if the test needs fixing or the code needs fixing.
Always add tests to cover newly added code.

## Code Coverage

Code coverage should stay above 80%. Notify the use when coverage falls below 80%.

## Commands

```bash
# Build
dotnet build Sah.Claude.Usage.Wpf.slnx

# Run all tests
dotnet test --solution Sah.Claude.Usage.Wpf.slnx

# Run unit tests only
dotnet test test/Sah.Claude.Usage.Wpf.UnitTests

# Run integration/UI tests only
dotnet test test/Sah.Claude.Usage.Wpf.IntegrationTests

# Run the WPF app
dotnet run --project src/Sah.Claude.Usage.Wpf

# Run a single test by name
dotnet test test/Sah.Claude.Usage.Wpf.UnitTests --tunit-filter "FullyQualifiedName=Namespace.Class.MethodName"
```

## Test projects

- **UnitTests** — pure logic tests; no UI involvement. Both test projects target `net10.0-windows` because they hold a project reference to the WPF app (which requires the Windows TFM).
- **IntegrationTests** — drives the live WPF process via FlaUI (UI Automation). The app must be built before running these tests. The `AppPath` constant in test files points to the `Debug` build output; update it or publish first if running in CI.

## Library Dependencies

Dependence on libraries contained within the solution are allowed.
Referencing NuGet packages default to official Microsoft first.
Consideration of NuGet packages is based on

- "official" packages created/supported by established companies
- the number of recent downloads
- the number of total downloads

## Code style

Follow the conventions in [CODE_QUALITY.CSharp.md](CODE_QUALITY.CSharp.md). Key rules:

- Private fields: `_camelCase`
- No `Abstract`, `Base`, `Helper`, or `Utility` suffixes in class names
- `Nullable` is enabled — use `?` annotations and avoid suppression operators
