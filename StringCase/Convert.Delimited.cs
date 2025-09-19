using System.Globalization;
using System.Linq;

namespace kalProject.Utils
{
    public static partial class Convert
    {
        /// <summary>
        /// Converts to snake_case with options.
        /// </summary>
        public static string ToSnakeCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            return string.Join("_", tokens.Select(t => options.PreserveAcronymsInDelimited && IsAcronym(t) ? t : t.ToLower(culture)));
        }

        /// <summary>
        /// Converts to kebab-case with options.
        /// </summary>
        public static string ToKebabCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            return string.Join("-", tokens.Select(t => options.PreserveAcronymsInDelimited && IsAcronym(t) ? t : t.ToLower(culture)));
        }
    }
}
