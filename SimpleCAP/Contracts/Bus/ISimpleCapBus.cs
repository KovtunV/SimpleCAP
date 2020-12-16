using System.Threading.Tasks;
using DotNetCore.CAP;
using SimpleCAP.Bus.Models;

namespace SimpleCAP.Contracts.Bus
{
    /// <summary>
    /// EventBus
    /// </summary>
    public interface ISimpleCapBus
    {
        /// <summary>
        /// Original CAP bus
        /// </summary>
        ICapPublisher CapBus { get; }

        /// <summary>
        /// Make request
        /// </summary>
        SimpleCapBusWrapper<TRequest> Request<TRequest>(TRequest model, string name = null);

        /// <summary>
        /// Send request
        /// </summary>
        Task SendAsync<TRequest>(TRequest model, string name = null);

        /// <summary>
        /// Send request and wait reasponse
        /// </summary>
        Task<TResponse> SendAsync<TRequest, TResponse>(TRequest model, string name = null);
    }
}
