using System;

namespace Ipatov.BinarySerialization.Reflection
{
    /// <summary>
    /// Type identity attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TypeIdentityAttribute : Attribute
    {
        /// <summary>
        /// Type identity.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Type id.</param>
        public TypeIdentityAttribute(string id)
        {
            Id = id;
        }
    }
}