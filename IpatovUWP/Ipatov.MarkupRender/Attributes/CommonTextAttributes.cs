namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Common text attributes.
    /// </summary>
    public static class CommonTextAttributes
    {
        /// <summary>
        /// Bold.
        /// </summary>
        public const string Bold = "b";

        /// <summary>
        /// Bold.
        /// </summary>
        public static readonly ITextAttribute BoldAttribute = new TextAttribute(Bold);

        /// <summary>
        /// Italic.
        /// </summary>
        public const string Italic = "i";

        /// <summary>
        /// Italic.
        /// </summary>
        public static readonly ITextAttribute ItalicAttribute = new TextAttribute(Italic);

        /// <summary>
        /// Undeline.
        /// </summary>
        public const string Undeline = "u";

        /// <summary>
        /// Undeline.
        /// </summary>
        public static readonly ITextAttribute UnderlineAttribute = new TextAttribute(Undeline);

        /// <summary>
        /// Overline.
        /// </summary>
        public const string Overline = "o";

        /// <summary>
        /// Overline.
        /// </summary>
        public static readonly ITextAttribute OverlineAttribute = new TextAttribute(Overline);

        /// <summary>
        /// Strikethrough.
        /// </summary>
        public const string Strikethrough = "s";

        /// <summary>
        /// Strikethrough.
        /// </summary>
        public static readonly ITextAttribute StrikethroughAttribute = new TextAttribute(Strikethrough);

        /// <summary>
        /// Quote.
        /// </summary>
        public const string Quote = "quote";

        /// <summary>
        /// Quote.
        /// </summary>
        public static readonly ITextAttribute QuoteAttribute = new TextAttribute(Quote);

        /// <summary>
        /// Spoiler.
        /// </summary>
        public const string Spoiler = "spoiler";

        /// <summary>
        /// Spoiler.
        /// </summary>
        public static readonly ITextAttribute SpoilerAttribute = new TextAttribute(Spoiler);

        /// <summary>
        /// Superscript.
        /// </summary>
        public const string Superscript = "sup";

        /// <summary>
        /// Superscript.
        /// </summary>
        public static readonly ITextAttribute SuperscriptAttribute = new TextAttribute(Superscript);

        /// <summary>
        /// Subscript.
        /// </summary>
        public const string Subscript = "sub";

        /// <summary>
        /// Subscript.
        /// </summary>
        public static readonly ITextAttribute SubscriptAttribute = new TextAttribute(Subscript);

        /// <summary>
        /// Hyperlink.
        /// </summary>
        public const string Link = "link";

        /// <summary>
        /// Create link attribute.
        /// </summary>
        /// <typeparam name="T">Link value type.</typeparam>
        /// <param name="value">Value.</param>
        /// <returns>Link attribute.</returns>
        public static ITextAttribute CreateLink<T>(T value)
        {
            return new TextAttribute<T>(Link, value);
        }

        /// <summary>
        /// Monospace text.
        /// </summary>
        public const string Fixed = "fixed";

        /// <summary>
        /// Monospace text.
        /// </summary>
        public static readonly ITextAttribute FixedAttribute = new TextAttribute(Fixed);
    }
}