using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization.IO
{
    /// <summary>
    /// Binary stream token writer.
    /// </summary>
    public sealed class StreamTokenWriter : ISerializationWriter
    {
        private readonly BinaryWriter _writer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="writer">Binary writer.</param>
        public StreamTokenWriter(BinaryWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            _writer = writer;
        }

        /// <summary>
        /// Write serialized token.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <param name="token">Token.</param>
        public void WriteToken(SerializationContext context, ref SerializationToken token)
        {
            DoWriteToken(context, ref token);
        }

        /// <summary>
        /// Write serialized token.
        /// </summary>
        /// <param name="context">Serialization context.</param>
        /// <param name="token">Token.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DoWriteToken(SerializationContext context, ref SerializationToken token)
        {
            WriteTokenType(token.TokenType);
            switch (token.TokenType)
            {
                case SerializationTokenType.Nothing:
                    // no data
                    break;
                case SerializationTokenType.Reference:
                    WriteReference(token.Reference, context);
                    break;
                case SerializationTokenType.Byte:
                    _writer.Write(token.Value.ByteValue);
                    break;
                case SerializationTokenType.SByte:
                    _writer.Write(token.Value.SByteValue);
                    break;
                case SerializationTokenType.Int16:
                    _writer.Write(token.Value.Int16Value);
                    break;
                case SerializationTokenType.UInt16:
                    _writer.Write(token.Value.UInt16Value);
                    break;
                case SerializationTokenType.Int32:
                    _writer.Write(token.Value.Int32Value);
                    break;
                case SerializationTokenType.UInt32:
                    _writer.Write(token.Value.UInt32Value);
                    break;
                case SerializationTokenType.Int64:
                    _writer.Write(token.Value.Int64Value);
                    break;
                case SerializationTokenType.UInt64:
                    _writer.Write(token.Value.UInt64Value);
                    break;
                case SerializationTokenType.Single:
                    _writer.Write(token.Value.SingleValue);
                    break;
                case SerializationTokenType.Double:
                    _writer.Write(token.Value.DoubleValue);
                    break;
                case SerializationTokenType.Decimal:
                    _writer.Write(token.Value.DecimalValue);
                    break;
                case SerializationTokenType.Boolean:
                    _writer.Write(token.Value.BooleanValue);
                    break;
                case SerializationTokenType.Char:
                    _writer.Write(token.Value.CharValue);
                    break;
                case SerializationTokenType.Guid:
                    _writer.Write(token.Value.GuidValue.ToByteArray());
                    break;
                case SerializationTokenType.DateTime:
                    _writer.Write(token.Value.DateTimeValue.Ticks);
                    break;
                case SerializationTokenType.TimeSpan:
                    _writer.Write(token.Value.TimeSpanValue.Ticks);
                    break;
                case SerializationTokenType.DateTimeOffset:
                    _writer.Write(token.Value.DateTimeOffsetValue.UtcDateTime.Ticks);
                    _writer.Write(token.Value.DateTimeOffsetValue.Offset.Ticks);
                    break;
                case SerializationTokenType.Int32Index:
                    WriteIndex(token.Value.Int32IndexValue);
                    break;
            }
        }

        private void WriteTokenType(SerializationTokenType type)
        {
            _writer.Write((byte)type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReference(object reference, SerializationContext context)
        {
            if (reference == null) throw new ArgumentNullException(nameof(reference));

            switch (reference)
            {
                case string s:
                    _writer.Write((byte)ComplexTypeReferenceKind.String);
                    WriteString(context, s);
                    break;
                case SerializedComplexTypeReference ctr:
                    _writer.Write((byte)ComplexTypeReferenceKind.ComplexTypeReference);
                    WriteIndex(ctr.ReferenceIndex);
                    break;
                case SerializedComplexType ct:
                    _writer.Write((byte)ComplexTypeReferenceKind.ComplexType);
                    if (ct.ObjectType == null)
                    {
                        throw new InvalidOperationException("Serialization error. Complex token object type not specified.");
                    }
                    var typeName = context.TypeMapper.GetTypeName(ct.ObjectType, context);
                    if (typeName == null)
                    {
                        throw new InvalidOperationException($"Serialization error. Cannot map type {ct.ObjectType.FullName}");
                    }
                    var tname = typeName.Value;
                    WriteTypeMapping(context, ref tname);
                    using (var propEnum = ct.Properties.GetEnumerator())
                    {
                        while (propEnum.MoveNext())
                        {
                            var p = propEnum.Current;
                            _writer.Write((byte)1);
                            WriteProperty(context, ref p);
                        }
                    }
                    _writer.Write((byte)0);
                    break;
                default:
                    throw new InvalidOperationException($"Serialization error. Unrecognized reference type {reference.GetType().FullName}");
            }
        }

        private void WriteTypeMapping(SerializationContext context, ref SerializationTypeMapping typeName)
        {
            WriteString(context, typeName.Kind ?? "");
            WriteString(context, typeName.Type ?? "");
            var a = typeName.TypeParameters ?? new SerializationTypeMapping[0];
            WriteIndex(a.Length);
            for (var i = 0; i < a.Length; i++)
            {
                WriteTypeMapping(context, ref a[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteProperty(SerializationContext context, ref SerializationProperty property)
        {
            if (property.Property == null)
            {
                throw new InvalidOperationException("Serialization error. Null property name.");
            }
            WriteString(context, property.Property);
            DoWriteToken(context, ref property.Token);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteIndex(int idx)
        {
            _writer.WriteIndex(idx);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteString(SerializationContext context, string s)
        {
            var idx = context.IsStringReferenced(s);
            if (idx != null)
            {
                _writer.Write((byte)1);
                WriteIndex(idx.Value);
            }
            else
            {
                var newIdx = context.AddString(s);
                _writer.Write((byte)0);
                WriteIndex(newIdx);
                _writer.Write(s);
            }
        }

        /// <summary>
        /// Flush buffered data.
        /// </summary>
        public void Flush()
        {
            _writer.Flush();
        }

        /// <summary>
        /// Write stream preamble.
        /// </summary>
        public void WritePreamble()
        {
            _writer.Write(SerializationFormatConsts.Header[0]);
            _writer.Write(SerializationFormatConsts.Header[1]);
            _writer.Write(SerializationFormatConsts.Header[2]);
            _writer.Write(SerializationFormatConsts.Header[3]);
            _writer.Write(SerializationFormatConsts.Version);
        }
    }
}