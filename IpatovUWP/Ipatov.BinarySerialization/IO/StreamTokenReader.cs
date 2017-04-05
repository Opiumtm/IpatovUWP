using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization.IO
{
    /// <summary>
    /// Binary stream token reader.
    /// </summary>
    public sealed class StreamTokenReader : ISerializationReader
    {
        private readonly BinaryReader _reader;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">Stream reader.</param>
        public StreamTokenReader(BinaryReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            _reader = reader;
        }

        /// <summary>
        /// Validate stream preamble.
        /// </summary>
        public void ValidatePreamble()
        {
            var hd = new byte[]
            {
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte()
            };
            if (
                hd[0] != SerializationFormatConsts.Header[0] ||
                hd[1] != SerializationFormatConsts.Header[1] ||
                hd[2] != SerializationFormatConsts.Header[2] ||
                hd[3] != SerializationFormatConsts.Header[3]
            )
            {
                throw new InvalidOperationException("Deserialization error. Invalid header.");
            }
            var version = _reader.ReadUInt16();
            if (version != SerializationFormatConsts.Version)
            {
                throw new InvalidOperationException("Deserialization error. Invalid version.");
            }
        }

        /// <summary>
        /// Read serialized token.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <returns>Token.</returns>
        public SerializationToken ReadToken(SerializationContext context)
        {
            return DoReadToken(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationToken DoReadToken(SerializationContext context)
        {
            var tokenType = ReadTokenType();
            switch (tokenType)
            {
                case SerializationTokenType.Nothing:
                    return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
                case SerializationTokenType.Reference:
                    return ReadReference(context);
                case SerializationTokenType.Byte:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { ByteValue = _reader.ReadByte() }
                    };
                case SerializationTokenType.SByte:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { SByteValue = _reader.ReadSByte() }
                    };
                case SerializationTokenType.Int16:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { Int16Value = _reader.ReadInt16() }
                    };
                case SerializationTokenType.UInt16:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { UInt16Value = _reader.ReadUInt16() }
                    };
                case SerializationTokenType.Int32:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { Int32Value = _reader.ReadInt32() }
                    };
                case SerializationTokenType.UInt32:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { UInt32Value = _reader.ReadUInt32() }
                    };
                case SerializationTokenType.Int64:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { Int64Value = _reader.ReadInt64() }
                    };
                case SerializationTokenType.UInt64:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { UInt64Value = _reader.ReadUInt64() }
                    };
                case SerializationTokenType.Single:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { SingleValue = _reader.ReadSingle() }
                    };
                case SerializationTokenType.Double:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { DoubleValue = _reader.ReadDouble() }
                    };
                case SerializationTokenType.Decimal:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { DecimalValue = _reader.ReadDecimal() }
                    };
                case SerializationTokenType.Boolean:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { BooleanValue = _reader.ReadBoolean() }
                    };
                case SerializationTokenType.Char:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { CharValue = _reader.ReadChar() }
                    };
                case SerializationTokenType.Guid:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { GuidValue = new Guid(_reader.ReadBytes(16)) }
                    };
                case SerializationTokenType.DateTime:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { DateTimeValue = new DateTime(_reader.ReadInt64()) }
                    };
                case SerializationTokenType.TimeSpan:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { TimeSpanValue = new TimeSpan(_reader.ReadInt64()) }
                    };
                case SerializationTokenType.DateTimeOffset:
                    var dt = new DateTime(_reader.ReadInt64());
                    var ofs = new TimeSpan(_reader.ReadInt64());
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { DateTimeOffsetValue = new DateTimeOffset(dt, ofs) }
                    };
                case SerializationTokenType.Int32Index:
                    return new SerializationToken()
                    {
                        TokenType = tokenType,
                        Value = new SerializationTokenValue() { Int32IndexValue = _reader.ReadIndex() }
                    };
                default:
                    throw new InvalidOperationException($"Deserialization error. Unknown token type {tokenType}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationToken ReadReference(SerializationContext context)
        {
            var rk = (ComplexTypeReferenceKind) _reader.ReadByte();
            switch (rk)
            {
                case ComplexTypeReferenceKind.String:
                    return new SerializationToken()
                    {
                        TokenType = SerializationTokenType.Reference,
                        Reference = ReadString(context)
                    };
                case ComplexTypeReferenceKind.ByteArray:
                    var baLen = _reader.ReadIndex();
                    if (baLen > 32 * 1024 * 1024)
                    {
                        throw new InvalidOperationException("Deserealization error. Byte array too long > 32 Mb");
                    }
                    var ba = new byte[baLen];
                    if (baLen > 0)
                    {
                        var baLen2 = _reader.Read(ba, 0, baLen);
                        if (baLen2 != baLen)
                        {
                            throw new InvalidOperationException($"Deserealization error. Requested byte[] len = {baLen}, actual bytes available = {baLen2}");
                        }
                    }
                    return new SerializationToken()
                    {
                        TokenType = SerializationTokenType.Reference,
                        Reference = ba
                    };
                case ComplexTypeReferenceKind.ComplexTypeReference:
                    return new SerializationToken()
                    {
                        TokenType = SerializationTokenType.Reference,
                        Reference = new SerializedComplexTypeReference()
                        {
                            ReferenceIndex = _reader.ReadIndex()
                        }
                    };
                case ComplexTypeReferenceKind.ComplexType:
                    var idx = _reader.ReadIndex();
                    var tmap = ReadTypeMapping(context);
                    var props = ReadProperties(context);
                    var type = context.TypeMapper.GetType(ref tmap, context);
                    if (type == null)
                    {
                        throw new InvalidOperationException($"Deserialization error. Unknown reference type mapping {tmap}.");
                    }
                    return new SerializationToken()
                    {
                        TokenType = SerializationTokenType.Reference,
                        Reference = new SerializedComplexType()
                        {
                            ObjectType = type,
                            ReferenceIndex = idx,
                            Properties = props
                        }
                    };
                default:
                    throw new InvalidOperationException($"Deserialization error. Unknown reference token type {rk}.");
            }
        }

        private IEnumerable<SerializationProperty> ReadProperties(SerializationContext context)
        {
            int maxCnt = 65535;
            do
            {
                maxCnt--;
                if (maxCnt <= 0)
                {
                    throw new InvalidOperationException("Deserialization error. Too many properties > 65535");
                }
                var sb = _reader.ReadByte();
                if (sb == 0)
                {
                    break;
                }
                if (sb == 1)
                {
                    var name = ReadString(context);
                    var token = DoReadToken(context);
                    yield return new SerializationProperty()
                    {
                        Property = name,
                        Token = token
                    };
                    continue;
                }
                throw new InvalidOperationException($"Deserialization error. Unknown property token type {sb}.");
            } while (true);
        }

        private string ReadString(SerializationContext context)
        {
            var isref = _reader.ReadByte();
            if (isref == 1)
            {
                var idx = _reader.ReadIndex();
                var s = context.GetString(idx);
                if (s == null)
                {
                    throw new InvalidOperationException("Deserialization error. Unknown string reference.");
                }
                return s;
            }
            if (isref == 0)
            {
                var idx = _reader.ReadIndex();
                var s = _reader.ReadString();
                if (s == null)
                {
                    throw new InvalidOperationException("Deserialization error. Null string reference.");
                }
                context.AddString(idx, s);
                return s;
            }
            throw new InvalidOperationException($"Deserialization error. Unknown string token type {isref}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationTypeMapping ReadTypeMapping(SerializationContext context)
        {
            var kind = ReadString(context);
            var type = ReadString(context);
            var len = _reader.ReadIndex();
            if (len > 255)
            {
                throw new InvalidOperationException($"Deserialization error. Too long type param count in type mapping: {len}");
            }
            var a = new SerializationTypeMapping[len];
            for (var i = 0; i < a.Length; i++)
            {
                a[i] = ReadTypeMapping(context);
            }
            return new SerializationTypeMapping()
            {
                Kind = kind,
                Type = type,
                TypeParameters = a
            };
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationTokenType ReadTokenType()
        {
            int tt = _reader.ReadByte();
            return (SerializationTokenType) tt;
        }
    }
}