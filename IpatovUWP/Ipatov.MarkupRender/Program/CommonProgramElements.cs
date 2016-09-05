namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Common program elements.
    /// </summary>
    public static class CommonProgramElements
    {
        /// <summary>
        /// Add text attribute.
        /// </summary>
        public const string AddAttribute = "add";

        /// <summary>
        /// Add text attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <returns>Program element.</returns>
        public static IRenderProgramElement AddAttributeElement(ITextAttribute attribute)
        {
            return new AddAttributeRenderProgramElement(attribute);
        }

        /// <summary>
        /// Remove text attribute.
        /// </summary>
        public const string RemoveAttribute = "remove";

        /// <summary>
        /// Remove text attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <returns>Program element.</returns>
        public static IRenderProgramElement RemoveAttributeElement(ITextAttribute attribute)
        {
            return new RemoveAttributeRenderProgramElement(attribute);
        }

        /// <summary>
        /// Print text.
        /// </summary>
        public const string PrintText = "print";

        /// <summary>
        /// Print text.
        /// </summary>
        /// <param name="text">Text to print.</param>
        /// <returns>Program element.</returns>
        public static IRenderProgramElement PrintTextElement(string text)
        {
            return new PrintTextRenderProgramElement(text);
        }

        /// <summary>
        /// Line break.
        /// </summary>
        public const string LineBreak = "br";

        /// <summary>
        /// Line break.
        /// </summary>
        public static readonly IRenderProgramElement LineBreakElement = new LineBreakRenderProgramElement();
    }
}