namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Common render content types.
    /// </summary>
    public static class CommonRenderContentTypes
    {
        /// <summary>
        /// Text content.
        /// </summary>
        public const string Text = "text";

        /// <summary>
        /// Text content.
        /// </summary>
        /// <param name="text">Text to render.</param>
        /// <returns>Content.</returns>
        public static IRenderContent TextContent(string text)
        {
            return new RenderTextContent(text);
        }

        /// <summary>
        /// Line break content.
        /// </summary>
        public const string LineBreak = "br";

        /// <summary>
        /// Line break content.
        /// </summary>
        public static readonly IRenderContent LineBreakContent = new RenderLineBreakContent();
    }
}