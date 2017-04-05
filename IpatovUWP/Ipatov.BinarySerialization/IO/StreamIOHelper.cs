﻿using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization.IO
{
    /// <summary>
    /// Stream writer helpers.
    /// </summary>
    public static class StreamIoHelper
    {
        /// <summary>
        /// Write index.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        /// <param name="idx">Index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteIndex(this BinaryWriter writer, int idx)
        {
            if (idx < 0)
            {
                throw new InvalidOperationException("Serialization error. Negative index value.");
            }
            if (idx < 0xFE)
            {
                writer.Write((byte)idx);
            }
            else if (idx < 0xFFFF)
            {
                writer.Write((byte)0xFE);
                writer.Write((ushort)idx);
            }
            else
            {
                writer.Write((byte)0xFF);
                writer.Write(idx);
            }
        }
    }
}