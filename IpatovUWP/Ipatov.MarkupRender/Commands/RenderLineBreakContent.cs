namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Line break render content.
    /// </summary>
    public class RenderLineBreakContent : IRenderContent
    {
        /// <summary>
        /// Content type.
        /// </summary>
        public string Type => CommonRenderContentTypes.LineBreak;
    }
}