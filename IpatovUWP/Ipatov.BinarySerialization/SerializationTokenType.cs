namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Token type.
    /// </summary>
    public enum SerializationTokenType
    {
        /// <summary>
        /// No value (null).
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// Reference type.
        /// </summary>
        Reference = 1,
        /// <summary>
        /// Byte.
        /// </summary>
        Byte = 2,
        /// <summary>
        /// SByte.
        /// </summary>
        SByte = 3,
        /// <summary>
        /// Int16.
        /// </summary>
        Int16 = 4,
        /// <summary>
        /// UInt16.
        /// </summary>
        UInt16 = 5,
        /// <summary>
        /// Int32.
        /// </summary>
        Int32 = 6,
        /// <summary>
        /// UInt32.
        /// </summary>
        UInt32 = 7,
        /// <summary>
        /// Int64.
        /// </summary>
        Int64 = 8,
        /// <summary>
        /// UInt64.
        /// </summary>
        UInt64 = 9,
        /// <summary>
        /// Single.
        /// </summary>
        Single = 10,
        /// <summary>
        /// Double.
        /// </summary>
        Double = 11,
        /// <summary>
        /// Decimal.
        /// </summary>
        Decimal = 12,
        /// <summary>
        /// Boolean.
        /// </summary>
        Boolean = 13,
        /// <summary>
        /// Char.
        /// </summary>
        Char = 14,
        /// <summary>
        /// Guid.
        /// </summary>
        Guid = 15,
        /// <summary>
        /// DateTime.
        /// </summary>
        DateTime = 16,
        /// <summary>
        /// DateTime.
        /// </summary>
        TimeSpan = 17,
        /// <summary>
        /// DateTimeOffset.
        /// </summary>
        DateTimeOffset = 18
    }
}