using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Int32 index value.
    /// </summary>
    public struct Int32Index
    {
        /// <summary>
        /// Index value.
        /// </summary>
        public Int32 Value;

        public static implicit operator Int32Index(int value)
        {
            return new Int32Index() { Value = value };
        }

        public static implicit operator int(Int32Index value)
        {
            return value.Value;
        }
    }
}