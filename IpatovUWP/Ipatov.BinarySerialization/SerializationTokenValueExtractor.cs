using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// �������� ��������� �������� �� ������ ��� ���������.
    /// </summary>
    public abstract class SerializationTokenValueExtractor
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public abstract object GetValueNonGeneric(ref SerializationToken token);

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public abstract SerializationToken CreateTokenNonGeneric(object value);
    }

    /// <summary>
    /// �������� ��������� �������� �� ������.
    /// </summary>
    /// <typeparam name="T">��� ��������.</typeparam>
    public abstract class SerializationTokenValueExtractor<T> : SerializationTokenValueExtractor
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public abstract T GetValue(ref SerializationToken token);

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public abstract SerializationToken CreateToken(T value);

        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public sealed override object GetValueNonGeneric(ref SerializationToken token)
        {
            return GetValue(ref token);
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public sealed override SerializationToken CreateTokenNonGeneric(object value)
        {
            return CreateToken((T)value);
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class ByteSerializationTokenValueExtractor : SerializationTokenValueExtractor<Byte>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Byte GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Byte, token.TokenType);
            return token.Value.ByteValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Byte value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Byte,
                Value = new SerializationTokenValue() { ByteValue  = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class ByteSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Byte?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Byte? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Byte, token.TokenType);
            return token.Value.ByteValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Byte? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Byte,
                Value = new SerializationTokenValue() { ByteValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class SByteSerializationTokenValueExtractor : SerializationTokenValueExtractor<SByte>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override SByte GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.SByte, token.TokenType);
            return token.Value.SByteValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(SByte value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.SByte,
                Value = new SerializationTokenValue() { SByteValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class SByteSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<SByte?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override SByte? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.SByte, token.TokenType);
            return token.Value.SByteValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(SByte? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.SByte,
                Value = new SerializationTokenValue() { SByteValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int16SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int16>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int16 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Int16, token.TokenType);
            return token.Value.Int16Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int16 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int16,
                Value = new SerializationTokenValue() { Int16Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int16SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Int16?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int16? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Int16, token.TokenType);
            return token.Value.Int16Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int16? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int16,
                Value = new SerializationTokenValue() { Int16Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt16SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt16>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt16 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.UInt16, token.TokenType);
            return token.Value.UInt16Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt16 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt16,
                Value = new SerializationTokenValue() { UInt16Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt16SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<UInt16?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt16? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.UInt16, token.TokenType);
            return token.Value.UInt16Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt16? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt16,
                Value = new SerializationTokenValue() { UInt16Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int32SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int32>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int32 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Int32, token.TokenType);
            return token.Value.Int32Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int32 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int32,
                Value = new SerializationTokenValue() { Int32Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int32SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Int32?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int32? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Int32, token.TokenType);
            return token.Value.Int32Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int32? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int32,
                Value = new SerializationTokenValue() { Int32Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt32SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt32>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt32 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.UInt32, token.TokenType);
            return token.Value.UInt32Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt32 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt32,
                Value = new SerializationTokenValue() { UInt32Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt32SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<UInt32?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt32? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.UInt32, token.TokenType);
            return token.Value.UInt32Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt32? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt32,
                Value = new SerializationTokenValue() { UInt32Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int64SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int64>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int64 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Int64, token.TokenType);
            return token.Value.Int64Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int64 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int64,
                Value = new SerializationTokenValue() { Int64Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int64SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Int64?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int64? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Int64, token.TokenType);
            return token.Value.Int64Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int64? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int64,
                Value = new SerializationTokenValue() { Int64Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt64SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt64>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt64 GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.UInt64, token.TokenType);
            return token.Value.UInt64Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt64 value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt64,
                Value = new SerializationTokenValue() { UInt64Value = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class UInt64SerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<UInt64?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt64? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.UInt64, token.TokenType);
            return token.Value.UInt64Value;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(UInt64? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.UInt64,
                Value = new SerializationTokenValue() { UInt64Value = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class SingleSerializationTokenValueExtractor : SerializationTokenValueExtractor<Single>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Single GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Single, token.TokenType);
            return token.Value.SingleValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Single value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Single,
                Value = new SerializationTokenValue() { SingleValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class SingleSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Single?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Single? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Single, token.TokenType);
            return token.Value.SingleValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Single? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Single,
                Value = new SerializationTokenValue() { SingleValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DoubleSerializationTokenValueExtractor : SerializationTokenValueExtractor<Double>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Double GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Double, token.TokenType);
            return token.Value.DoubleValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Double value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Double,
                Value = new SerializationTokenValue() { DoubleValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DoubleSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Double?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Double? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Double, token.TokenType);
            return token.Value.DoubleValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Double? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Double,
                Value = new SerializationTokenValue() { DoubleValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DecimalSerializationTokenValueExtractor : SerializationTokenValueExtractor<Decimal>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Decimal GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Decimal, token.TokenType);
            return token.Value.DecimalValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Decimal value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Decimal,
                Value = new SerializationTokenValue() { DecimalValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DecimalSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Decimal?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Decimal? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Decimal, token.TokenType);
            return token.Value.DecimalValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Decimal? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Decimal,
                Value = new SerializationTokenValue() { DecimalValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class BooleanSerializationTokenValueExtractor : SerializationTokenValueExtractor<Boolean>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Boolean GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Boolean, token.TokenType);
            return token.Value.BooleanValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Boolean value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Boolean,
                Value = new SerializationTokenValue() { BooleanValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class BooleanSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Boolean?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Boolean? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Boolean, token.TokenType);
            return token.Value.BooleanValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Boolean? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Boolean,
                Value = new SerializationTokenValue() { BooleanValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class CharSerializationTokenValueExtractor : SerializationTokenValueExtractor<Char>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Char GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Char, token.TokenType);
            return token.Value.CharValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Char value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Char,
                Value = new SerializationTokenValue() { CharValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class CharSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Char?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Char? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Char, token.TokenType);
            return token.Value.CharValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Char? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Char,
                Value = new SerializationTokenValue() { CharValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class GuidSerializationTokenValueExtractor : SerializationTokenValueExtractor<Guid>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Guid GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Guid, token.TokenType);
            return token.Value.GuidValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Guid value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Guid,
                Value = new SerializationTokenValue() { GuidValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class GuidSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Guid?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Guid? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Guid, token.TokenType);
            return token.Value.GuidValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Guid? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Guid,
                Value = new SerializationTokenValue() { GuidValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DateTimeSerializationTokenValueExtractor : SerializationTokenValueExtractor<DateTime>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTime GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.DateTime, token.TokenType);
            return token.Value.DateTimeValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(DateTime value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTime,
                Value = new SerializationTokenValue() { DateTimeValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DateTimeSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<DateTime?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTime? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.DateTime, token.TokenType);
            return token.Value.DateTimeValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(DateTime? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTime,
                Value = new SerializationTokenValue() { DateTimeValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class TimeSpanSerializationTokenValueExtractor : SerializationTokenValueExtractor<TimeSpan>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override TimeSpan GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.TimeSpan, token.TokenType);
            return token.Value.TimeSpanValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(TimeSpan value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.TimeSpan,
                Value = new SerializationTokenValue() { TimeSpanValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class TimeSpanSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<TimeSpan?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override TimeSpan? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.TimeSpan, token.TokenType);
            return token.Value.TimeSpanValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(TimeSpan? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.TimeSpan,
                Value = new SerializationTokenValue() { TimeSpanValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DateTimeOffsetSerializationTokenValueExtractor : SerializationTokenValueExtractor<DateTimeOffset>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTimeOffset GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.DateTimeOffset, token.TokenType);
            return token.Value.DateTimeOffsetValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(DateTimeOffset value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTimeOffset,
                Value = new SerializationTokenValue() { DateTimeOffsetValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class DateTimeOffsetSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<DateTimeOffset?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTimeOffset? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.DateTimeOffset, token.TokenType);
            return token.Value.DateTimeOffsetValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(DateTimeOffset? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.DateTimeOffset,
                Value = new SerializationTokenValue() { DateTimeOffsetValue = value.Value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int32IndexSerializationTokenValueExtractor : SerializationTokenValueExtractor<Int32Index>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int32Index GetValue(ref SerializationToken token)
        {
            SerializationToken.CheckType(SerializationTokenType.Int32, token.TokenType);
            return token.Value.Int32IndexValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int32Index value)
        {
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int32Index,
                Value = new SerializationTokenValue() { Int32IndexValue = value }
            };
        }
    }

    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class Int32IndexSerializationTokenValueExtractorNullable : SerializationTokenValueExtractor<Int32Index?>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int32Index? GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Int32Index, token.TokenType);
            return token.Value.Int32IndexValue;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(Int32Index? value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Int32Index,
                Value = new SerializationTokenValue() { Int32IndexValue = value.Value }
            };
        }
    }


    /// <summary>
    /// ��������� ������.
    /// </summary>
    internal sealed class ReferenceSerializationTokenValueExtractor<T> : SerializationTokenValueExtractor<T>
        where T: class 
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override T GetValue(ref SerializationToken token)
        {
            if (token.TokenType == SerializationTokenType.Nothing)
            {
                return null;
            }
            SerializationToken.CheckType(SerializationTokenType.Reference, token.TokenType);
            return (T) token.Reference;
        }

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public override SerializationToken CreateToken(T value)
        {
            if (value == null)
            {
                return new SerializationToken() { TokenType = SerializationTokenType.Nothing };
            }
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = value
            };
        }
    }
}