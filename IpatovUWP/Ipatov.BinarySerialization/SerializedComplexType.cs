using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialized complex type.
    /// </summary>
    public sealed class SerializedComplexType
    {
        /// <summary>
        /// Type.
        /// </summary>
        public Type ObjectType;

        /// <summary>
        /// Reference index.
        /// </summary>
        public int ReferenceIndex;

        /// <summary>
        /// Properties.
        /// </summary>
        public SerializationProperty[] Properties;
    }
}