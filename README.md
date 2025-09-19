# kalProject.Utils.StringCase
String case conversion utilities for C#

# kalProject.StringCase

Utilidades para conversión de formatos de texto (camelCase, PascalCase, snake_case, kebab-case, etc.) con tokenización robusta y opciones para preservar acrónimos.

## Características

- Tokenizador que separa por delimitadores, transiciones de mayúsculas/minúsculas y límites con dígitos.
- Conversión a:
  - CamelCase, PascalCase
  - snake_case, kebab-case
  - UPPER CASE, lower case, Title Case, Sentence case
- Detección heurística de formato.
- Opciones para preservar acrónimos en formatos relevantes.

## Uso rápido

```
using kalProject.StringCase;
using System.Globalization;

var inv = CultureInfo.InvariantCulture;

Convert.ToSnakeCase("HelloWorld", inv); // "hello_world"
Convert.ToKebabCase("HelloWorld", inv); // "hello-world"
Convert.ToPascalCase("hello world", inv); // "HelloWorld"
Convert.ToCamelCase("Hello World", inv); // "helloWorld"
Convert.ToTitleCase("my xml parser", inv); // "My Xml Parser"
Convert.ToSentenceCase("my XML parser", inv); // "My xml parser"

Convert.DetectCaseType("hello-world"); // CaseType.KebabCase
```

## Opciones: preservar acrónimos

```
var opt = new StringCaseOptions {
  PreserveAcronymsInTitleCasing = true,   // Pascal/Title conservan "HTTP"
  PreserveAcronymsInDelimited   = true,   // snake/kebab conservan "HTTP"
  PreserveAcronymsInPlain       = true    // UPPER/lower con espacios conservan "HTTP"
};

Convert.ToPascalCase("HTTP server", inv, opt); // "HTTPServer"
Convert.ToTitleCase("my xml parser", inv, opt); // "My XML Parser"
Convert.ToSnakeCase("HTTP server", inv, opt);   // "HTTP_server"
Convert.ToKebabCase("HTTP server", inv, opt);   // "HTTP-server"
Convert.ToUpperCase("api http server", inv, opt); // "API HTTP SERVER"
```

Un acrónimo se detecta como token con letras y todas en mayúsculas (longitud > 1). Los números no afectan.

## API destacada

- Convert.To(input, caseType, culture?)
- Convert.To(input, caseType, options, culture?)
- Específicos: ToCamelCase, ToPascalCase, ToSnakeCase, ToKebabCase, ToUpperCase, ToLowerCase, ToTitleCase, ToSentenceCase
- Detección: DetectCaseType(input) y DetectCaseType(input, options)
- Tokenización: Tokenize(input)

## Notas

- CamelCase no conserva acrónimo en el primer token (por definición).
- Este paquete es netstandard2.0; úsalo desde .NET Standard, .NET Core y .NET 5+.
