namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding group.
    /// </summary>
    public interface IDataBindingGroup : IDataBinding
    {
        /// <summary>
        /// Add data binding to group.
        /// </summary>
        /// <param name="binding">Binding.</param>
        void AddBinding(IDataBinding binding);
    }
}