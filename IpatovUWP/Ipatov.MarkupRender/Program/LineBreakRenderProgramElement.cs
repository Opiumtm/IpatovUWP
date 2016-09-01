namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Line break program element.
    /// </summary>
    public sealed class LineBreakRenderProgramElement : IRenderProgramElement
    {
        public string Command => CommonProgramElements.LineBreak;
    }
}