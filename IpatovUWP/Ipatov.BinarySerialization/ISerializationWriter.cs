namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialized data writer.
    /// </summary>
    public interface ISerializationWriter
    {
        /// <summary>
        /// Write stream preamble.
        /// </summary>
        void WritePreamble();

        /// <summary>
        /// Write serialized token.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <param name="token">Token.</param>
        void WriteToken(SerializationContext context, ref SerializationToken token);

        /// <summary>
        /// Flush buffered data.
        /// </summary>
        void Flush();
    }
}