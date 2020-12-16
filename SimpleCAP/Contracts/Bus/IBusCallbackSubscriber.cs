using DotNetCore.CAP;

namespace SimpleCAP.Contracts.Bus
{
    /// <summary>
    /// Cap subscriber for callbacks. Based on <see cref="ICapSubscribe"/>
    /// </summary>
    public interface IBusCallbackSubscriber : ICapSubscribe
    {
        /// <summary>
        /// CallbackName
        /// </summary>
        string CallbackName { get; }
    }
}
