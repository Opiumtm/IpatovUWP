namespace Ipatov.BinarySerialization.IO
{
    /// <summary>
    /// Serialization format constants.
    /// </summary>
    internal static class SerializationFormatConsts
    {
        /// <summary>
        /// Header.
        /// </summary>
        public static readonly byte[] Header = new byte[]
        {
            0x49, 0x42, 0x53, 0x46
        };

        /// <summary>
        /// Version.
        /// </summary>
        public static readonly ushort Version = 0x0100;
    }
}