using System;

namespace Ipatov.BinarySerialization.Reflection
{
    /// <summary>
    /// Игнорировать член данных.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnorePropertyAttribute : Attribute
    {
    }
}