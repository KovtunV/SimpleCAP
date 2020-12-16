using System.Threading.Tasks;
using SimpleCAP.Bus.Models;

namespace SimpleCAP.Contracts.Bus
{
    /// <summary>
    /// Callback awaiter
    /// </summary>
    public interface IBusCallbackService
    {
        /// <summary>
        /// Wait callback
        /// </summary>
        Task<CallbackResult<TResponse>> WaitAsync<TResponse>(string messageId);

        /// <summary>
        /// Add response to callback container
        /// </summary>
        void AddResponse(string messageId, object model);
    }
}
