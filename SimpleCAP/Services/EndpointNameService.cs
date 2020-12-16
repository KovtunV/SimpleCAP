using System;
using SimpleCAP.Contracts;

namespace SimpleCAP.Services
{
    /// <summary>
    /// Topic name service
    /// </summary>
    public class EndpointNameService : IEndpointNameService
    {
        /// <summary>
        /// Returns the topic name
        /// </summary>
        public string GetName<TRequest>()
        {
            var type = typeof(TRequest);
            return GetName(type);
        }

        /// <summary>
        /// Returns the topic name
        /// </summary>
        public string GetName(Type type)
        {
            return type.Name;
        }
    }
}
