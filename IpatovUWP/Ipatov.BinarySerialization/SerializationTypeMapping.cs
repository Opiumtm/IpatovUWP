using System.Linq;
using System.Text;

namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Serialization type mapping.
    /// </summary>
    public struct SerializationTypeMapping
    {
        /// <summary>
        /// Mapping kind.
        /// </summary>
        public string Kind;

        /// <summary>
        /// Main type.
        /// </summary>
        public string Type;

        /// <summary>
        /// Type parameters.
        /// </summary>
        public SerializationTypeMapping[] TypeParameters;

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var key = $"{Kind}:{Type}";
            if (TypeParameters != null && TypeParameters.Length > 0)
            {
                key = key + "[" + TypeParameters.Aggregate(new StringBuilder(), (sb, s) => (sb.Length > 0 ? sb.Append(",") : sb).Append(s.ToString())) + "]";
            }
            return key;
        }
    }
}