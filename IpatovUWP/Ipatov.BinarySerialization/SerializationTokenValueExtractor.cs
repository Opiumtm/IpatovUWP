using System;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// �������� ��������� �������� �� ������.
    /// </summary>
    /// <typeparam name="T">��� ��������.</typeparam>
    public abstract class SerializationTokenValueExtractor<T>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public abstract T GetValue(SerializationToken token);

        /// <summary>
        /// ������� �����.
        /// </summary>
        /// <param name="value">��������.</param>
        /// <returns>�����.</returns>
        public abstract SerializationToken CreateToken(T value);
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
        public override Byte GetValue(SerializationToken token)
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
    internal sealed class SByteSerializationTokenValueExtractor : SerializationTokenValueExtractor<SByte>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override SByte GetValue(SerializationToken token)
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
    internal sealed class Int16SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int16>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int16 GetValue(SerializationToken token)
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
    internal sealed class UInt16SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt16>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt16 GetValue(SerializationToken token)
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
    internal sealed class Int32SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int32>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int32 GetValue(SerializationToken token)
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
    internal sealed class UInt32SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt32>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt32 GetValue(SerializationToken token)
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
    internal sealed class Int64SerializationTokenValueExtractor : SerializationTokenValueExtractor<Int64>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Int64 GetValue(SerializationToken token)
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
    internal sealed class UInt64SerializationTokenValueExtractor : SerializationTokenValueExtractor<UInt64>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override UInt64 GetValue(SerializationToken token)
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
    internal sealed class SingleSerializationTokenValueExtractor : SerializationTokenValueExtractor<Single>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Single GetValue(SerializationToken token)
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
    internal sealed class DoubleSerializationTokenValueExtractor : SerializationTokenValueExtractor<Double>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Double GetValue(SerializationToken token)
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
    internal sealed class DecimalSerializationTokenValueExtractor : SerializationTokenValueExtractor<Decimal>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Decimal GetValue(SerializationToken token)
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
    internal sealed class BooleanSerializationTokenValueExtractor : SerializationTokenValueExtractor<Boolean>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Boolean GetValue(SerializationToken token)
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
    internal sealed class CharSerializationTokenValueExtractor : SerializationTokenValueExtractor<Char>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Char GetValue(SerializationToken token)
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
    internal sealed class GuidSerializationTokenValueExtractor : SerializationTokenValueExtractor<Guid>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override Guid GetValue(SerializationToken token)
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
    internal sealed class DateTimeSerializationTokenValueExtractor : SerializationTokenValueExtractor<DateTime>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTime GetValue(SerializationToken token)
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
    internal sealed class TimeSpanSerializationTokenValueExtractor : SerializationTokenValueExtractor<TimeSpan>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override TimeSpan GetValue(SerializationToken token)
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
    internal sealed class DateTimeOffsetSerializationTokenValueExtractor : SerializationTokenValueExtractor<DateTimeOffset>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override DateTimeOffset GetValue(SerializationToken token)
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
    internal sealed class ReferenceSerializationTokenValueExtractor<T> : SerializationTokenValueExtractor<T>
    {
        /// <summary>
        /// �������� ��������.
        /// </summary>
        /// <param name="token">�����.</param>
        /// <returns>��������.</returns>
        public override T GetValue(SerializationToken token)
        {
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
            return new SerializationToken()
            {
                TokenType = SerializationTokenType.Reference,
                Reference = value
            };
        }
    }
}