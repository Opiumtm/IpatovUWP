namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text content to render.
    /// </summary>
    public class RenderTextContent : IRenderContentText
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="text">Text to render.</param>
        public RenderTextContent(string text)
        {
            Text = text ?? "";
        }

        /// <summary>
        /// Content type.
        /// </summary>
        public string Type => CommonRenderContentTypes.Text;

        /// <summary>
        /// Text to render.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Implicit cast.
        /// </summary>
        /// <param name="text">Text to render.</param>
        public static implicit operator RenderTextContent(string text)
        {
            return new RenderTextContent(text);
        }
    }
}