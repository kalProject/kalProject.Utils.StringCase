using System;
using System.Globalization;
using StringCaseConvert = kalProject.Utils.Convert;

class Program
{
    static void Main()
    {
        var inv = CultureInfo.InvariantCulture;
        Console.WriteLine(StringCaseConvert.ToSnakeCase("HelloWorld", inv));
        Console.WriteLine(StringCaseConvert.ToKebabCase("HelloWorld", inv));
    }
}
