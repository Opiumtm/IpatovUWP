namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization type mapping.
    /// </summary>
    public struct SerializationTypeMapping
    {
        /// <summary>
        /// Mapping kind.
        /// </summary>
        public string Kind;

        /// <summary>
        /// Main type.
        /// </summary>
        public string Type;

        /// <summary>
        /// Type parameters.
        /// </summary>
        public string[] TypeParameters;
    }
}