using System.Globalization;
using StringCaseConvert = kalProject.Utils.Convert;

var inv = CultureInfo.InvariantCulture;
Console.WriteLine(StringCaseConvert.ToSnakeCase("HelloWorld", inv));
Console.WriteLine(StringCaseConvert.ToKebabCase("HelloWorld", inv));
