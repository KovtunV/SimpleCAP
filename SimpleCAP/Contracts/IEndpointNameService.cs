using System;

namespace SimpleCAP.Contracts
{
    /// <summary>
    /// Topic name service
    /// </summary>
    public interface IEndpointNameService
    {
        /// <summary>
        /// Returns the topic name
        /// </summary>
        string GetName<TRequest>();

        /// <summary>
        /// Returns the topic name
        /// </summary>
        string GetName(Type type);
    }
}
