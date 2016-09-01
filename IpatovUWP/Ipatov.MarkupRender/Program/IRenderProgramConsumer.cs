namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render program consumer.
    /// </summary>
    public interface IRenderProgramConsumer
    {
        /// <summary>
        /// Push program element.
        /// </summary>
        /// <param name="element">Element.</param>
        void Push(IRenderProgramElement element);

        /// <summary>
        /// Clear program state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Flush current state. Call this method at program end.
        /// </summary>
        void Flush();
    }
}