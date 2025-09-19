# kalProject.Utils.StringCase

Robust string case conversion helpers for C# with smart tokenization (delimiters, case transitions, digit boundaries), popular case formatters, and acronym-preservation options.

## Features

- Smart tokenizer splits by delimiters, upper/lower transitions, and digit boundaries
- Convert to:
  - camelCase, PascalCase
  - snake_case, kebab-case
  - UPPER CASE, lower case, Title Case, Sentence case
- Heuristic case detection
- Options to preserve acronyms per format

## Supported frameworks

- net8.0
- netstandard2.0
- .NET Framework 4.7.2 (net472)
- .NET Framework 4.8 (net48)

Nullable Reference Types are enabled across all targets.

## Installation (GitHub Packages)

GitHub Packages requires authentication even for public packages. Add the GitHub Packages feed and your credentials.

NuGet.config example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/kalProject/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_PERSONAL_ACCESS_TOKEN" />
    </github>
  </packageSourceCredentials>
  <activePackageSource>
    <add key="All" value="(Aggregate source)" />
  </activePackageSource>
  <trustedSigners />
</configuration>
```

Then reference the package in your project file after publishing.

```xml
<ItemGroup>
  <PackageReference Include="kalProject.Utils.StringCase" Version="x.y.z" />
</ItemGroup>
```

## Quick start

```csharp
using System.Globalization;
// Avoid name conflict with System.Convert using an alias:
using StringCaseConvert = kalProject.Utils.Convert;

var inv = CultureInfo.InvariantCulture;

StringCaseConvert.ToSnakeCase("HelloWorld", inv); // "hello_world"
StringCaseConvert.ToKebabCase("HelloWorld", inv); // "hello-world"
StringCaseConvert.ToPascalCase("hello world", inv); // "HelloWorld"
StringCaseConvert.ToCamelCase("Hello World", inv); // "helloWorld"
StringCaseConvert.ToTitleCase("my xml parser", inv); // "My Xml Parser"
StringCaseConvert.ToSentenceCase("my XML parser", inv); // "My xml parser"

StringCaseConvert.DetectCaseType("hello-world"); // CaseType.KebabCase
```

## Options: preserve acronyms

```csharp
var opt = new StringCaseOptions {
  PreserveAcronymsInTitleCasing = true,   // Pascal/Title keep "HTTP"
  PreserveAcronymsInDelimited   = true,   // snake/kebab keep "HTTP"
  PreserveAcronymsInPlain       = true    // UPPER/lower (space separated) keep "HTTP"
};

StringCaseConvert.ToPascalCase("HTTP server", inv, opt); // "HTTPServer"
StringCaseConvert.ToTitleCase("my xml parser", inv, opt); // "My XML Parser"
StringCaseConvert.ToSnakeCase("HTTP server", inv, opt);   // "HTTP_server"
StringCaseConvert.ToKebabCase("HTTP server", inv, opt);   // "HTTP-server"
StringCaseConvert.ToUpperCase("api http server", inv, opt); // "API HTTP SERVER"
```

An acronym is detected as a token with letters and all uppercase (length > 1). Digits do not affect detection.

## Highlighted API

- Convert.To(input, caseType, culture?)
- Convert.To(input, caseType, options, culture?)
- Specific helpers: ToCamelCase, ToPascalCase, ToSnakeCase, ToKebabCase, ToUpperCase, ToLowerCase, ToTitleCase, ToSentenceCase
- Detection: DetectCaseType(input) and DetectCaseType(input, options)
- Tokenization: Tokenize(input)

## Notes

- In camelCase, the first token is fully lowercased by design (acronyms are not preserved on first token).
- Frameworks: net8.0, netstandard2.0, .NET Framework 4.7.2 (net472), .NET Framework 4.8 (net48).
- Nullable Reference Types are enabled in all targets.
- For .NET Framework builds, the project uses "Microsoft.NETFramework.ReferenceAssemblies" so you don't need to install targeting packs locally.

## Build and test

```bash
dotnet restore
dotnet build kalProject.Utils.StringCase.sln -c Release
dotnet test kalProject.Utils.StringCase.sln -c Release
```

## CI / Publishing

- CI runs on every push/PR to main and builds/tests the solution.
- Publishing to GitHub Packages happens on pushing tags starting with `v` (e.g., `v1.2.3`). The tag version becomes the NuGet `PackageVersion`.

## License

MIT
