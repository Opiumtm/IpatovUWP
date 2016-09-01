namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render program element text.
    /// </summary>
    public interface IRenderProgramText : IRenderProgramElement
    {
        /// <summary>
        /// Text.
        /// </summary>
        string Text { get; }         
    }
}