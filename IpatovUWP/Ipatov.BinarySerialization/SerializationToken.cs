using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization token.
    /// </summary>
    public struct SerializationToken
    {
        /// <summary>
        /// Token type.
        /// </summary>
        public SerializationTokenType TokenType;

        /// <summary>
        /// Token value.
        /// </summary>
        public SerializationTokenValue Value;

        /// <summary>
        /// Token reference (or boxed complex reference type).
        /// </summary>
        public object Reference;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Byte src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Byte,
                Value = new SerializationTokenValue() { ByteValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(SByte src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.SByte,
                Value = new SerializationTokenValue() { SByteValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Int16 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int16,
                Value = new SerializationTokenValue() { Int16Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(UInt16 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt16,
                Value = new SerializationTokenValue() { UInt16Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Int32 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int32,
                Value = new SerializationTokenValue() { Int32Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(UInt32 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt32,
                Value = new SerializationTokenValue() { UInt32Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Int64 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int64,
                Value = new SerializationTokenValue() { Int64Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(UInt64 src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt64,
                Value = new SerializationTokenValue() { UInt64Value = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Single src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Single,
                Value = new SerializationTokenValue() { SingleValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Double src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Double,
                Value = new SerializationTokenValue() { DoubleValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Decimal src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Decimal,
                Value = new SerializationTokenValue() { DecimalValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Boolean src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Boolean,
                Value = new SerializationTokenValue() { BooleanValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Char src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Char,
                Value = new SerializationTokenValue() { CharValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(Guid src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Guid,
                Value = new SerializationTokenValue() { GuidValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(DateTime src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTime,
                Value = new SerializationTokenValue() { DateTimeValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(TimeSpan src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.TimeSpan,
                Value = new SerializationTokenValue() { TimeSpanValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(DateTimeOffset src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTimeOffset,
                Value = new SerializationTokenValue() { DateTimeOffsetValue = src },
                Reference = null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator SerializationToken(string src)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = src
            };
        }

        /// <summary>
        /// Get token from reference type object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <returns>Serialization token.</returns>
        public static SerializationToken FromObject<T>(T obj) where T : class 
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = obj
            };
        }

        private static readonly Dictionary<Type, SerializationTokenValueExtractor> ValueExtractors = new Dictionary<Type, SerializationTokenValueExtractor>()
        {
            { typeof(Byte), new ByteSerializationTokenValueExtractor()},
            { typeof(SByte), new SByteSerializationTokenValueExtractor()},
            { typeof(Int16), new Int16SerializationTokenValueExtractor()},
            { typeof(UInt16), new UInt16SerializationTokenValueExtractor()},
            { typeof(Int32), new Int32SerializationTokenValueExtractor()},
            { typeof(UInt32), new UInt32SerializationTokenValueExtractor()},
            { typeof(Int64), new Int64SerializationTokenValueExtractor()},
            { typeof(UInt64), new UInt64SerializationTokenValueExtractor()},
            { typeof(Single), new SingleSerializationTokenValueExtractor()},
            { typeof(Double), new DoubleSerializationTokenValueExtractor()},
            { typeof(Decimal), new DecimalSerializationTokenValueExtractor()},
            { typeof(Boolean), new BooleanSerializationTokenValueExtractor()},
            { typeof(Char), new CharSerializationTokenValueExtractor()},
            { typeof(Guid), new GuidSerializationTokenValueExtractor()},
            { typeof(DateTime), new DateTimeSerializationTokenValueExtractor()},
            { typeof(TimeSpan), new TimeSpanSerializationTokenValueExtractor()},
            { typeof(DateTimeOffset), new DateTimeOffsetSerializationTokenValueExtractor()},
            { typeof(Int32Index), new Int32IndexSerializationTokenValueExtractor()},
            { typeof(Byte?), new ByteSerializationTokenValueExtractorNullable()},
            { typeof(SByte?), new SByteSerializationTokenValueExtractorNullable()},
            { typeof(Int16?), new Int16SerializationTokenValueExtractorNullable()},
            { typeof(UInt16?), new UInt16SerializationTokenValueExtractorNullable()},
            { typeof(Int32?), new Int32SerializationTokenValueExtractorNullable()},
            { typeof(UInt32?), new UInt32SerializationTokenValueExtractorNullable()},
            { typeof(Int64?), new Int64SerializationTokenValueExtractorNullable()},
            { typeof(UInt64?), new UInt64SerializationTokenValueExtractorNullable()},
            { typeof(Single?), new SingleSerializationTokenValueExtractorNullable()},
            { typeof(Double?), new DoubleSerializationTokenValueExtractorNullable()},
            { typeof(Decimal?), new DecimalSerializationTokenValueExtractorNullable()},
            { typeof(Boolean?), new BooleanSerializationTokenValueExtractorNullable()},
            { typeof(Char?), new CharSerializationTokenValueExtractorNullable()},
            { typeof(Guid?), new GuidSerializationTokenValueExtractorNullable()},
            { typeof(DateTime?), new DateTimeSerializationTokenValueExtractorNullable()},
            { typeof(TimeSpan?), new TimeSpanSerializationTokenValueExtractorNullable()},
            { typeof(DateTimeOffset?), new DateTimeOffsetSerializationTokenValueExtractorNullable()},
            { typeof(Int32Index?), new Int32IndexSerializationTokenValueExtractorNullable()},
            { typeof(string), new ReferenceSerializationTokenValueExtractor<string>()},
            { typeof(byte[]), new ReferenceSerializationTokenValueExtractor<byte[]>()},
        };

        /// <summary>
        /// Check token type.
        /// </summary>
        /// <param name="expected">Expected token type.</param>
        /// <param name="actual">Actual token type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CheckType(SerializationTokenType expected, SerializationTokenType actual)
        {
            if (actual != expected)
            {
                throw new InvalidOperationException($"Token of type {expected} is expected");
            }
        }

        /// <summary>
        /// Get token value extractor.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>Value extractor (null if non-standard complex type).</returns>
        public static SerializationTokenValueExtractor<T> GetExtractor<T>()
        {
            var t = typeof(T);
            if (ValueExtractors.ContainsKey(t))
            {
                return ValueExtractors[t] as SerializationTokenValueExtractor<T>;
            }
            return null;
        }

        /// <summary>
        /// Get token value extractor.
        /// </summary>
        /// <param name="type">Value type.</param>
        /// <returns>Value extractor (null if non-standard complex type).</returns>
        public static SerializationTokenValueExtractor GetExtractor(Type type)
        {
            if (ValueExtractors.ContainsKey(type))
            {
                return ValueExtractors[type];
            }
            return null;
        }
    }
}