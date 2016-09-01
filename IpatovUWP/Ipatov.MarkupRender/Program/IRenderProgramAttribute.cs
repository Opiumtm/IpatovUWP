namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render program element attribute.
    /// </summary>
    public interface IRenderProgramAttribute : IRenderProgramElement
    {
        /// <summary>
        /// Text attribute.
        /// </summary>
        ITextAttribute Attribute { get; }         
    }
}