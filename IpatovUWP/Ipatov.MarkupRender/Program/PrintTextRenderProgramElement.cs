namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Print text program element.
    /// </summary>
    public sealed class PrintTextRenderProgramElement : IRenderProgramText
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text to print.</param>
        public PrintTextRenderProgramElement(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Command name.
        /// </summary>
        public string Command => CommonProgramElements.PrintText;

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Implicit cast operator.
        /// </summary>
        /// <param name="text">Text to print.</param>
        public static implicit operator PrintTextRenderProgramElement(string text)
        {
            return new PrintTextRenderProgramElement(text);
        }
    }
}