using System.Linq;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding path.
    /// </summary>
    public struct DataBindingPath
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parts">Path parts.</param>
        public DataBindingPath(string[] parts)
        {
            _parts = parts;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path parts.</param>
        public DataBindingPath(string path)
        {
            _parts = path?.Split('.');
        }

        /// <summary>
        /// Path parts.
        /// </summary>
        private readonly string[] _parts;

        /// <summary>
        /// Current path part.
        /// </summary>
        public string Part => _parts != null && Part.Length > 0 ? _parts[0] : null;

        /// <summary>
        /// Current path length.
        /// </summary>
        public int Length => _parts?.Length ?? 0;

        /// <summary>
        /// Get next path part.
        /// </summary>
        /// <returns>Next path part.</returns>
        public DataBindingPath? Next()
        {
            if (_parts == null)
            {
                return null;
            }
            if (_parts.Length == 1)
            {
                return null;
            }
            return new DataBindingPath(_parts.Skip(1).ToArray());
        }

        public static implicit operator DataBindingPath(string path)
        {
            return new DataBindingPath(path);
        }
    }
}