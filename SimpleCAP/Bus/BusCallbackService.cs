using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SimpleCAP.Bus.Models;
using SimpleCAP.Contracts.Bus;
using SimpleCAP.Options;

namespace SimpleCAP.Bus
{
    /// <summary>
    /// Callback awaiter
    /// </summary>
    public class BusCallbackService : IBusCallbackService, IDisposable
    {
        private readonly ConcurrentDictionary<string, object> _callbackMessages;
        private readonly CancellationTokenSource _cts;
        private readonly TimeSpan _timeout;

        private readonly IValueConverterService _valueConverterService;

        /// <summary>
        /// Callback awaiter
        /// </summary>
        public BusCallbackService(IOptions<CallbackOptions> callbackOptions, IValueConverterService valueConverterService)
        {
            _valueConverterService = valueConverterService;
            _timeout = callbackOptions.Value.Timeout ?? DefaultTimeout;
            _callbackMessages = new ConcurrentDictionary<string, object>();
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Wait callback
        /// </summary>
        public Task<CallbackResult<TResponse>> WaitAsync<TResponse>(string messageId)
        {
            return Task.Run(() => OnWait<TResponse>(messageId));
        }

        private CallbackResult<TResponse> OnWait<TResponse>(string messageId)
        {
            var spinner = new SpinWait();
            var startWait = DateTime.UtcNow;

            while (true)
            {
                // Cancel
                if (_cts.IsCancellationRequested)
                {
                    return CallbackResult<TResponse>.Error("Cancelled");
                }

                // Timeout
                var now = DateTime.UtcNow;
                var diff = now.Subtract(startWait);
                if (diff > _timeout)
                {
                    return CallbackResult<TResponse>.Error("Timeout");
                }

                // Prevent CPU burning
                spinner.SpinOnce();

                if (_callbackMessages.TryGetValue(messageId, out var value))
                {
                    _callbackMessages.TryRemove(messageId, out _);

                    var convertedValue = _valueConverterService.Convert<TResponse>(value);
                    return new CallbackResult<TResponse>(convertedValue);
                }
            }
        }

        /// <summary>
        /// Add response to callback container
        /// </summary>
        public void AddResponse(string messageId, object model)
        {
            _callbackMessages.TryAdd(messageId, model);
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _cts.Cancel();
        }

        /// <summary>
        /// Default response timeout, 3 minutes
        /// </summary>
        public static TimeSpan DefaultTimeout = TimeSpan.FromMinutes(3);
    }
}
