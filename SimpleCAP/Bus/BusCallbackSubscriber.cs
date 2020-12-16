using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using SimpleCAP.Contracts.Bus;

namespace SimpleCAP.Bus
{
    /// <summary>
    /// Cap subscriber for callbacks
    /// </summary>
    public class BusCallbackSubscriber : IBusCallbackSubscriber
    {
        private readonly IBusCallbackService _callbackService;

        /// <summary>
        /// Cap subscriber for callbacks
        /// </summary>
        public BusCallbackSubscriber(IBusCallbackService callbackService)
        {
            _callbackService = callbackService;
        }

        [CapSubscribe(CALLBACK_NAME)]
        private void SubscribeCallback(object model, [FromCap] CapHeader headers)
        {
            var callbackId = headers[Headers.CorrelationId];
            _callbackService.AddResponse(callbackId, model);
        }

        /// <summary>
        /// CallbackName
        /// </summary>
        public string CallbackName => CALLBACK_NAME;

        /// <summary>
        /// CallbackName
        /// </summary>
        public const string CALLBACK_NAME = "__SimpleCapCallback";
    }
}
