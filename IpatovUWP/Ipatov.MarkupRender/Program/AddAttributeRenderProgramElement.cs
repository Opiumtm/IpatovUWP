namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Add text attribute program element.
    /// </summary>
    public sealed class AddAttributeRenderProgramElement : AttributeRenderProgramElement
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attribute">Text attribute.</param>
        public AddAttributeRenderProgramElement(ITextAttribute attribute) : base(attribute)
        {
        }

        /// <summary>
        /// Command name.
        /// </summary>
        public override string Command => CommonProgramElements.AddAttribute;
    }
}