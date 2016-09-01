using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Source of render commands.
    /// </summary>
    public interface IRenderCommandsSource
    {
        /// <summary>
        /// Get render commands.
        /// </summary>
        /// <returns>Render commands.</returns>
        IReadOnlyList<IRenderCommand> GetCommands();
    }
}