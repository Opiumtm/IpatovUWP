using System.ComponentModel;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup render control data.
    /// </summary>
    public interface IMarkupRenderData : INotifyPropertyChanged
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