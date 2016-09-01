namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text attribute.
    /// </summary>
    public sealed class TextAttribute : ITextAttribute
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        public TextAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Attribute name.
        /// </summary>
        public string Name { get; }
    }

    /// <summary>
    /// Text attribute with data value.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class TextAttribute<T> : ITextAttributeData<T>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <param name="value">Data value.</param>
        public TextAttribute(string name, T value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Attribute name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Data value.
        /// </summary>
        public T Value { get; }
    }
}