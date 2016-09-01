namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text content.
    /// </summary>
    public interface IRenderContentText : IRenderContent
    {
        /// <summary>
        /// Text to render.
        /// </summary>
        string Text { get; }
    }
}