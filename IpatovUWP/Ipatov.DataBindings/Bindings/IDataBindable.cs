namespace Ipatov.DataBindings
{
    /// <summary>
    /// Object supporting data bindings.
    /// </summary>
    public interface IDataBindable
    {
        /// <summary>
        /// Get data binding source.
        /// </summary>
        /// <typeparam name="T">Data binding type.</typeparam>
        /// <param name="name">Name of value.</param>
        /// <returns>Data binding source.</returns>
        IDataBindingSource<T> GetSource<T>(string name);

        /// <summary>
        /// Get child bindable source.
        /// </summary>
        /// <param name="name">Name of value</param>
        /// <returns>Child bindable source.</returns>
        IDataBindable GetChildBindable(string name);
    }
}