using System;
using System.Runtime.InteropServices;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Значение токена сериализации.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct SerializationTokenValue
    {
        /// <summary>
        /// Значение типа Byte.
        /// </summary>
        [FieldOffset(0)] public Byte ByteValue;

        /// <summary>
        /// Значение типа SByte.
        /// </summary>
        [FieldOffset(0)] public SByte SByteValue;

        /// <summary>
        /// Значение типа Int16.
        /// </summary>
        [FieldOffset(0)] public Int16 Int16Value;

        /// <summary>
        /// Значение типа UInt16.
        /// </summary>
        [FieldOffset(0)] public UInt16 UInt16Value;

        /// <summary>
        /// Значение типа Int32.
        /// </summary>
        [FieldOffset(0)] public Int32 Int32Value;

        /// <summary>
        /// Значение типа UInt32.
        /// </summary>
        [FieldOffset(0)] public UInt32 UInt32Value;

        /// <summary>
        /// Значение типа Int64.
        /// </summary>
        [FieldOffset(0)] public Int64 Int64Value;

        /// <summary>
        /// Значение типа Int64.
        /// </summary>
        [FieldOffset(0)] public UInt64 UInt64Value;

        /// <summary>
        /// Значение типа Single.
        /// </summary>
        [FieldOffset(0)] public Single SingleValue;

        /// <summary>
        /// Значение типа Double.
        /// </summary>
        [FieldOffset(0)] public Double DoubleValue;

        /// <summary>
        /// Значение типа DecimalDouble.
        /// </summary>
        [FieldOffset(0)] public Decimal DecimalValue;

        /// <summary>
        /// Значение типа Boolean.
        /// </summary>
        [FieldOffset(0)] public Boolean BooleanValue;

        /// <summary>
        /// Значение типа Char.
        /// </summary>
        [FieldOffset(0)] public Char CharValue;

        /// <summary>
        /// Значение типа Guid.
        /// </summary>
        [FieldOffset(0)] public Guid GuidValue;

        /// <summary>
        /// Значение типа DateTime.
        /// </summary>
        [FieldOffset(0)] public DateTime DateTimeValue;

        /// <summary>
        /// Значение типа TimeSpan.
        /// </summary>
        [FieldOffset(0)] public TimeSpan TimeSpanValue;

        /// <summary>
        /// Значение типа DateTimeOffset.
        /// </summary>
        [FieldOffset(0)] public DateTimeOffset DateTimeOffsetValue;
    }
}