namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Attribute render program element.
    /// </summary>
    public abstract class AttributeRenderProgramElement : IRenderProgramAttribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attribute">Text attribute.</param>
        protected AttributeRenderProgramElement(ITextAttribute attribute)
        {
            Attribute = attribute;
        }

        /// <summary>
        /// Command name.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Text attribute.
        /// </summary>
        public ITextAttribute Attribute { get; }
    }
}