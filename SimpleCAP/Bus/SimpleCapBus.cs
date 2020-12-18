using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using Microsoft.Extensions.Logging;
using SimpleCAP.Bus.Models;
using SimpleCAP.Contracts;
using SimpleCAP.Contracts.Bus;

namespace SimpleCAP.Bus
{
    /// <summary>
    /// EventBus
    /// </summary>
    public class SimpleCapBus : ISimpleCapBus
    {
        private readonly ILogger<SimpleCapBus> _logger;
        private readonly IEndpointNameService _endpointNameService;
        private readonly IKeyGeneratorService _keyGeneratorService;
        private readonly IBusCallbackSubscriber _callbackSubscriber;
        private readonly IBusCallbackService _callbackService;

        /// <summary>
        /// Original CAP bus
        /// </summary>
        public ICapPublisher CapBus { get; }

        /// <summary>
        /// EventBus
        /// </summary>
        public SimpleCapBus(ILogger<SimpleCapBus> logger, IEndpointNameService endpointNameService, IKeyGeneratorService keyGeneratorService, 
            ICapPublisher capBus, IBusCallbackService callbackService, IBusCallbackSubscriber callbackSubscriber)
        {
            _logger = logger;
            _endpointNameService = endpointNameService;
            _keyGeneratorService = keyGeneratorService;
            _callbackService = callbackService;
            _callbackSubscriber = callbackSubscriber;

            CapBus = capBus;
        }

        /// <summary>
        /// Make request
        /// </summary>
        public SimpleCapBusWrapper<TRequest> Request<TRequest>(TRequest model, string name = null)
        {
            return new SimpleCapBusWrapper<TRequest>(this, model, name);
        }

        /// <summary>
        /// Send request
        /// </summary>
        public async Task SendAsync<TRequest>(TRequest model, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = _endpointNameService.GetName<TRequest>();
            }

            await CapBus.PublishAsync(name, model);
        }

        /// <summary>
        /// Send request and wait response
        /// </summary>
        public async Task<TResponse> SendAsync<TRequest, TResponse>(TRequest model, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = _endpointNameService.GetName<TRequest>();
            }

            var messageId = _keyGeneratorService.Generate();
            var headers = GenerateHeaders(messageId, _callbackSubscriber.CallbackName);

            // Send message
            await CapBus.PublishAsync(name, model, headers: headers);

            // Wait message
            var callbackRes = await _callbackService.WaitAsync<TResponse>(messageId);

            // Log
            if (!callbackRes.IsOk)
            {
                _logger.LogError($"Response \"{callbackRes.ErrorMessage}\" when request type was \"{typeof(TRequest).Name}\"");
            }

            return callbackRes.Response;
        }

        private Dictionary<string, string> GenerateHeaders(string messageId, string callbackName)
        {
            return new Dictionary<string, string>
            {
                [Headers.MessageId] = messageId,
                [Headers.CallbackName] = callbackName
            };
        }
    }
}
