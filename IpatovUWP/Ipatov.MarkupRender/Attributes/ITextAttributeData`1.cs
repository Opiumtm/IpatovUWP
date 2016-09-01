namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text attribute data.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public interface ITextAttributeData<out T> : ITextAttribute
    {
        /// <summary>
        /// Data value.
        /// </summary>
        T Value { get; }                 
    }
}