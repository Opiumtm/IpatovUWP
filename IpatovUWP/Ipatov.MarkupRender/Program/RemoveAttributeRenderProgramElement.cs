namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Remove text attribute program element.
    /// </summary>
    public sealed class RemoveAttributeRenderProgramElement : AttributeRenderProgramElement
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attribute">Text attribute.</param>
        public RemoveAttributeRenderProgramElement(ITextAttribute attribute) : base(attribute)
        {
        }

        /// <summary>
        /// Command name.
        /// </summary>
        public override string Command => CommonProgramElements.RemoveAttribute;
    }
}