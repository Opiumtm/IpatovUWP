namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup render control data.
    /// </summary>
    public sealed class MarkupRenderData : IMarkupRenderData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="commands">Render commands.</param>
        /// <param name="style">Render style.</param>
        public MarkupRenderData(IRenderCommandsSource commands, ITextRenderStyle style)
        {
            Commands = commands;
            Style = style;
        }

        /// <summary>
        /// Render commands.
        /// </summary>
        public IRenderCommandsSource Commands { get; }

        /// <summary>
        /// Render style.
        /// </summary>
        public ITextRenderStyle Style { get; }
    }
}