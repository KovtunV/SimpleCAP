using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCAP.Contracts.Bus;

namespace SimpleCAP.Bus.Models
{
    /// <summary>
    /// Wrapper for eventBus result
    /// </summary>
    public readonly struct SimpleCapBusWrapper<TRequest> : IEquatable<SimpleCapBusWrapper<TRequest>>
    {
        private readonly ISimpleCapBus _simpleCapBus;
        private readonly TRequest _model;
        private readonly string _name;

        /// <summary>
        /// Wrapper for eventBus result
        /// </summary>
        public SimpleCapBusWrapper(ISimpleCapBus simpleCapBus, TRequest model, string name)
        {
            _simpleCapBus = simpleCapBus;
            _model = model;
            _name = name;
        }

        /// <summary>
        /// Send request
        /// </summary>
        public Task SendAsync()
        {
            return _simpleCapBus.SendAsync(_model, _name);
        }

        /// <summary>
        /// Send request and wait reasponse
        /// </summary>
        public Task<TResponse> SendAsync<TResponse>()
        {
            return _simpleCapBus.SendAsync<TRequest, TResponse>(_model, _name);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public bool Equals(SimpleCapBusWrapper<TRequest> other)
        {
            return Equals(_simpleCapBus, other._simpleCapBus) && EqualityComparer<TRequest>.Default.Equals(_model, other._model) && _name == other._name;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is SimpleCapBusWrapper<TRequest> other && Equals(other);
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(_simpleCapBus, _model, _name);
        }
    }
}
