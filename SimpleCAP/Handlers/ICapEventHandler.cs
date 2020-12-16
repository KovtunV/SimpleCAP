using System.Threading.Tasks;
using SimpleCAP.Attributes;

namespace SimpleCAP.Handlers
{
    /// <summary>
    /// Event handler
    /// </summary>
    public interface ICapEventHandler
    {

    }

    /// <summary>
    /// Event handler with default name = TRequest.Name
    /// <para>To override name use <see cref="OverrideSubscribeAttribute"/></para>
    /// </summary>
    public interface ICapEventHandler<in TRequest> : ICapEventHandler
    {
        /// <summary>
        /// Handle message
        /// </summary>
        Task HandleAsync(TRequest model);
    }

    /// <summary>
    /// Event handler with a response and a default name = TRequest.Name
    /// <para>To override name use <see cref="OverrideSubscribeAttribute"/></para>
    /// </summary>
    public interface ICapEventHandler<in TRequest, TResponse> : ICapEventHandler
    {
        /// <summary>
        /// Handle message
        /// </summary>
        Task<TResponse> HandleAsync(TRequest model);
    }
}