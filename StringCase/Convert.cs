using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace kalProject.Utils
{
    /// <summary>
    /// String case conversion helpers.
    /// </summary>
    /// <remarks>
    /// Provides robust tokenization (word splitting) across delimiters, case transitions, and digit boundaries,
    /// plus formatters for common cases like camelCase, PascalCase, snake_case, kebab-case, UPPER CASE, lower case,
    /// Title Case, and Sentence case. Includes a simple heuristic-based detector.
    /// </remarks>
    public static partial class Convert
    {
        private static readonly char[] DefaultDelimiters = new[]
        {
            ' ', '\t', '\n', '\r', '_', '-', '.', '/', '\\', ':', ';', ',', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '+', '*', '&', '#', '@', '!', '?'
        };

        /// <summary>
        /// Converts <paramref name="input"/> into the requested <paramref name="caseType"/>.
        /// </summary>
        /// <param name="input">Input string. Null or empty returns empty string.</param>
        /// <param name="caseType">Desired target case.</param>
        /// <param name="culture">Optional culture; defaults to InvariantCulture.</param>
        public static string To(string? input, CaseType caseType, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return caseType switch
            {
                CaseType.None => input,
                CaseType.CamelCase => ToCamelCase(input, culture),
                CaseType.PascalCase => ToPascalCase(input, culture),
                CaseType.SnakeCase => ToSnakeCase(input, culture),
                CaseType.KebabCase => ToKebabCase(input, culture),
                CaseType.UpperCase => ToUpperCase(input, culture),
                CaseType.LowerCase => ToLowerCase(input, culture),
                CaseType.TitleCase => ToTitleCase(input, culture),
                CaseType.SentenceCase => ToSentenceCase(input, culture),
                _ => input
            };
        }

        /// <summary>
        /// Converts <paramref name="input"/> into the requested <paramref name="caseType"/> with options.
        /// </summary>
        public static string To(string? input, CaseType caseType, StringCaseOptions options, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return caseType switch
            {
                CaseType.None => input,
                CaseType.CamelCase => ToCamelCase(input, culture),
                CaseType.PascalCase => ToPascalCase(input, culture, options),
                CaseType.SnakeCase => ToSnakeCase(input, culture, options),
                CaseType.KebabCase => ToKebabCase(input, culture, options),
                CaseType.UpperCase => ToUpperCase(input, culture),
                CaseType.LowerCase => ToLowerCase(input, culture),
                CaseType.TitleCase => ToTitleCase(input, culture, options),
                CaseType.SentenceCase => ToSentenceCase(input, culture),
                _ => input
            };
        }

        /// <summary>
        /// Converts to camelCase.
        /// </summary>
        public static string ToCamelCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            if (tokens.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            // First token lower-case entirely
            sb.Append(tokens[0].ToLower(culture));
            for (int i = 1; i < tokens.Count; i++)
            {
                sb.Append(UppercaseFirst(tokens[i], culture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts to PascalCase.
        /// </summary>
        public static string ToPascalCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            var sb = new StringBuilder();
            foreach (var t in tokens)
            {
                sb.Append(UppercaseFirst(t, culture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts to PascalCase with options.
        /// </summary>
        public static string ToPascalCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            var sb = new StringBuilder();
            foreach (var t in tokens)
            {
                if (options.PreserveAcronyms && IsAcronym(t))
                {
                    sb.Append(t); // keep as-is
                }
                else
                {
                    sb.Append(UppercaseFirst(t, culture));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts to snake_case (lowercase, underscore separated).
        /// </summary>
        public static string ToSnakeCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            return string.Join("_", tokens.Select(t => t.ToLower(culture)));
        }

        /// <summary>
        /// Converts to kebab-case (lowercase, hyphen separated).
        /// </summary>
        public static string ToKebabCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            return string.Join("-", tokens.Select(t => t.ToLower(culture)));
        }

        /// <summary>
        /// Converts to UPPER CASE (space separated tokens).
        /// </summary>
        public static string ToUpperCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => t.ToUpper(culture)));
        }

        /// <summary>
        /// Converts to UPPER CASE with options (may preserve acronyms as-is).
        /// </summary>
        public static string ToUpperCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => options.PreserveAcronymsInPlain && IsAcronym(t) ? t : t.ToUpper(culture)));
        }

        /// <summary>
        /// Converts to lower case (space separated tokens).
        /// </summary>
        public static string ToLowerCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => t.ToLower(culture)));
        }

        /// <summary>
        /// Converts to lower case with options (may preserve acronyms as-is).
        /// </summary>
        public static string ToLowerCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => options.PreserveAcronymsInPlain && IsAcronym(t) ? t : t.ToLower(culture)));
        }

        /// <summary>
        /// Converts to Title Case (Every Word Capitalized; space separated tokens).
        /// </summary>
        public static string ToTitleCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => UppercaseFirst(t, culture)));
        }

        /// <summary>
        /// Converts to Title Case with options.
        /// </summary>
        public static string ToTitleCase(string input, CultureInfo? culture, StringCaseOptions options)
        {
            culture ??= CultureInfo.InvariantCulture;
            options ??= StringCaseOptions.Default;
            var tokens = Tokenize(input);
            return string.Join(" ", tokens.Select(t => options.PreserveAcronyms && IsAcronym(t) ? t : UppercaseFirst(t, culture)));
        }

        /// <summary>
        /// Converts to Sentence case (First word capitalized, rest lower; space separated tokens).
        /// </summary>
        public static string ToSentenceCase(string input, CultureInfo? culture = null)
        {
            culture ??= CultureInfo.InvariantCulture;
            var tokens = Tokenize(input);
            if (tokens.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            sb.Append(UppercaseFirst(tokens[0], culture));
            for (int i = 1; i < tokens.Count; i++)
            {
                sb.Append(' ');
                sb.Append(tokens[i].ToLower(culture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Detects the most likely <see cref="CaseType"/> for the provided input.
        /// </summary>
        public static CaseType DetectCaseType(string? input)
        {
            if (string.IsNullOrEmpty(input)) return CaseType.None;

            var hasHyphen = input.IndexOf('-') >= 0;
            var hasUnderscore = input.IndexOf('_') >= 0;
            var hasSpace = input.Contains(' ');
            var hasUpper = input.Any(char.IsUpper);
            var hasLower = input.Any(char.IsLower);

            if (hasHyphen && !hasUnderscore) return CaseType.KebabCase;
            if (hasUnderscore && !hasHyphen) return CaseType.SnakeCase;

            if (!hasSpace && !hasHyphen && !hasUnderscore)
            {
                // Single token, look for camel/pascal
                if (char.IsLower(input[0]) && hasUpper) return CaseType.CamelCase;
                if (char.IsUpper(input[0]) && hasLower && hasUpper) return CaseType.PascalCase;
            }

            if (hasSpace)
            {
                if (hasUpper && !hasLower) return CaseType.UpperCase;
                if (hasLower && !hasUpper) return CaseType.LowerCase;

                // Heuristic: Title if most words start with upper
                var tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var titleish = tokens.Count(t => char.IsLetter(t[0]) && char.IsUpper(t[0])) >= Math.Max(1, tokens.Length / 2);
                return titleish ? CaseType.TitleCase : CaseType.SentenceCase;
            }

            // Fallback to None if ambiguous
            return CaseType.None;
        }

        /// <summary>
        /// Detects case with options that can bias toward TitleCase when acronyms are present and preservation is intended.
        /// </summary>
        public static CaseType DetectCaseType(string? input, StringCaseOptions options)
        {
            var detected = DetectCaseType(input);
            if (detected == CaseType.None || string.IsNullOrEmpty(input)) return detected;

            if (options != null && options.PreserveAcronymsInTitleCasing)
            {
                // If string has spaces and contains at least one acronym-like token, prefer TitleCase over SentenceCase.
                if (detected is CaseType.SentenceCase or CaseType.LowerCase or CaseType.UpperCase || detected == CaseType.TitleCase)
                {
                    var tokens = input!.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Any(IsAcronym))
                    {
                        return CaseType.TitleCase;
                    }
                }
            }
            return detected;
        }

        /// <summary>
        /// Tokenizes a string into words based on delimiters, case transitions, and digit boundaries.
        /// </summary>
        /// <remarks>Empty or null input returns an empty list.</remarks>
        public static IReadOnlyList<string> Tokenize(string? input)
        {
            if (string.IsNullOrEmpty(input)) return Array.Empty<string>();

            var list = new List<string>(Math.Max(4, input.Length / 4));
            var sb = new StringBuilder();

            bool IsDelimiter(char ch) => DefaultDelimiters.Contains(ch);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char? prev = i > 0 ? input[i - 1] : (char?)null;
                char? next = i + 1 < input.Length ? input[i + 1] : (char?)null;

                if (IsDelimiter(c))
                {
                    FlushToken(sb, list);
                    continue;
                }

                if (prev.HasValue && IsWordBoundary(prev.Value, c, next))
                {
                    FlushToken(sb, list);
                }

                sb.Append(c);
            }

            FlushToken(sb, list);
            return list;
        }

        private static void FlushToken(StringBuilder sb, List<string> list)
        {
            if (sb.Length == 0) return;
            var token = sb.ToString();
            sb.Clear();
            if (!string.IsNullOrWhiteSpace(token))
            {
                list.Add(token);
            }
        }

        private static bool IsWordBoundary(char prev, char curr, char? next)
        {
            // lower -> Upper (e.g., myHTTP => my | HTTP)
            if (char.IsLetter(prev) && char.IsLower(prev) && char.IsLetter(curr) && char.IsUpper(curr))
                return true;

            // Acronym boundary: UPPER followed by UpperLower: HTTPServer => HTTP | Server at P|S (since next is lower)
            if (char.IsLetter(prev) && char.IsUpper(prev) && char.IsLetter(curr) && char.IsUpper(curr) && next.HasValue && char.IsLetter(next.Value) && char.IsLower(next.Value))
                return true;

            // Letter <-> Digit boundary
            if ((char.IsLetter(prev) && char.IsDigit(curr)) || (char.IsDigit(prev) && char.IsLetter(curr)))
                return true;

            return false;
        }

        private static string UppercaseFirst(string s, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpper(culture);
            var lower = s.ToLower(culture);
            var first = char.ToUpper(lower[0], culture);
            return first + lower.Substring(1);
        }

        private static bool IsAcronym(string s)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= 1) return false;
            bool hasLetter = false;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                    if (!char.IsUpper(c)) return false;
                }
            }
            return hasLetter;
        }
    }
}
