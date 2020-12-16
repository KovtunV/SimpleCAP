namespace SimpleCAP.Contracts.Bus
{
    /// <summary>
    /// Value converter
    /// </summary>
    public interface IValueConverterService
    {
        /// <summary>
        /// Convert
        /// </summary>
        TResponse Convert<TResponse>(object value);
    }
}
