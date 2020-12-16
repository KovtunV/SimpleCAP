using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleCAP.Exceptions;
using SimpleCAP.Handlers;

namespace SimpleCAP.Options
{
    /// <summary>
    /// Handler container
    /// </summary>
    public class CapEventHandlerOptions
    {
        /// <summary>
        /// Event handlers
        /// </summary>
        public List<Type> Handlers { get; }

        /// <summary>
        /// Handler container
        /// </summary>
        public CapEventHandlerOptions()
        {
            Handlers = new List<Type>();
        }

        /// <summary>
        /// Add an event handler
        /// </summary>
        public void AddHandler<THandler>()
        {
            AddHandler(typeof(THandler));
        }

        /// <summary>
        /// Add an event handler
        /// </summary>
        public void AddHandler(Type handlerType)
        {
            var handlerInterfaceTypeInfo = typeof(ICapEventHandler).GetTypeInfo();
            if (!handlerType.GetInterfaces().Any(a => handlerInterfaceTypeInfo.IsAssignableFrom(a)))
            {
                throw new CapEventHandlerTypeException($"Handler type \"{handlerType.FullName}\" must be implemented from \"{nameof(ICapEventHandler)}\"");
            }

            Handlers.Add(handlerType);
        }
    }
}
