namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup render control data.
    /// </summary>
    public interface IMarkupRenderData
    {
        /// <summary>
        /// Render commands.
        /// </summary>
        IRenderCommandsSource Commands { get; }

        /// <summary>
        /// Render style.
        /// </summary>
        ITextRenderStyle Style { get; }
    }
}