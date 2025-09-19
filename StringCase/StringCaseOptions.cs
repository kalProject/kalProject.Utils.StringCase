namespace kalProject.Utils
{
    /// <summary>
    /// Options to control string case conversion behavior.
    /// </summary>
    public sealed class StringCaseOptions
    {
        /// <summary>
        /// When true, keeps acronym tokens (all-uppercase alphabetic tokens with length > 1) unchanged
        /// in title-casing paths (e.g., PascalCase and Title Case). Default: false.
        /// </summary>
        public bool PreserveAcronymsInTitleCasing { get; set; }

        /// <summary>
        /// When true, keeps acronym tokens unchanged in delimited lowercase formats (snake/kebab),
        /// resulting in mixed-case outputs like "HTTP_server". Default: false.
        /// </summary>
        public bool PreserveAcronymsInDelimited { get; set; }

    /// <summary>
    /// When true, keeps acronym tokens unchanged in plain space-separated formats (UPPER/lower outputs),
    /// resulting in mixed-case outputs like "HTTP server" for lower or "HTTP SERVER" for upper depending on non-acronym tokens.
    /// Default: false.
    /// </summary>
    public bool PreserveAcronymsInPlain { get; set; }

        /// <summary>
        /// Legacy aggregate toggle. Prefer specific properties.
        /// Maps to PreserveAcronymsInTitleCasing when set or read.
        /// </summary>
        [System.Obsolete("Use PreserveAcronymsInTitleCasing or PreserveAcronymsInDelimited instead.")]
        public bool PreserveAcronyms
        {
            get => PreserveAcronymsInTitleCasing;
            set => PreserveAcronymsInTitleCasing = value;
        }

        /// <summary>
        /// Returns a default instance with all defaults.
        /// </summary>
        public static StringCaseOptions Default { get; } = new StringCaseOptions();
    }
}
