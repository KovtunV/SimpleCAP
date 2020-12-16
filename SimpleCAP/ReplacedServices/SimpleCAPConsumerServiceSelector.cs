using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.Options;
using SimpleCAP.Contracts;
using SimpleCAP.Handlers;
using SimpleCAP.Options;

namespace SimpleCAP.ReplacedServices
{
    /// <summary>
    /// Overrided <see cref="ConsumerServiceSelector"/>
    /// </summary>
    public class SimpleCAPConsumerServiceSelector : ConsumerServiceSelector
    {
        private readonly IEndpointNameService _nameService;
        private readonly CapEventHandlerOptions _capEventHandlerOptions;

        /// <summary>
        /// Overrided <see cref="ConsumerServiceSelector"/>
        /// </summary>
        public SimpleCAPConsumerServiceSelector(IServiceProvider serviceProvider, IEndpointNameService nameService,
            IOptions<CapEventHandlerOptions> capEventHandlerOptions) : base(serviceProvider)
        {
            _nameService = nameService;
            _capEventHandlerOptions = capEventHandlerOptions.Value;
        }

        /// <summary>
        /// Overrided FindConsumersFromInterfaceTypes
        /// </summary>
        protected override IEnumerable<ConsumerExecutorDescriptor> FindConsumersFromInterfaceTypes(IServiceProvider provider)
        {
            // Attribute-based descriptors
            var executorDescriptorList = base.FindConsumersFromInterfaceTypes(provider).ToList();

            // Handler-based descriptors
            var handlers = _capEventHandlerOptions.Handlers;

            var handletInterfaceType = typeof(ICapEventHandler);
            foreach (var handlerType in handlers)
            {
                var interfaces = handlerType.GetInterfaces();
                foreach (var interf in interfaces)
                {
                    if (!handletInterfaceType.IsAssignableFrom(interf))
                    {
                        continue;
                    }

                    var genericArgs = interf.GetGenericArguments();
                    if (TryGetInterfaceType(genericArgs, out var interfaceType))
                    {
                        var handlerBaseDescriptions = GetHandlerDescriptions(genericArgs[0], handlerType, interfaceType);
                        executorDescriptorList.AddRange(handlerBaseDescriptions);
                    }
                }
            }

            return executorDescriptorList;
        }

        /// <summary>
        /// Returns Handler-based descriptions
        /// </summary>
        protected virtual IEnumerable<ConsumerExecutorDescriptor> GetHandlerDescriptions(Type requestType, Type handlerType, Type serviceInterfaceType)
        {
            var handlerMethodInfo = handlerType.GetMethod(nameof(ICapEventHandler<object>.HandleAsync), new[] { requestType });
            var topicAttr = handlerMethodInfo.GetCustomAttributes<TopicAttribute>(true);
            var topicAttributes = topicAttr.ToList();

            var subscribeAttribute = GetCapSubscribeAttribute(requestType, handlerType);
            topicAttributes.Add(subscribeAttribute);

            foreach (var attr in topicAttributes)
            {
                SetSubscribeAttribute(attr);

                var parameters = handlerMethodInfo.GetParameters()
                    .Select(parameter => new ParameterDescriptor
                    {
                        Name = parameter.Name,
                        ParameterType = parameter.ParameterType,
                        IsFromCap = parameter.GetCustomAttributes(typeof(FromCapAttribute)).Any()
                    }).ToList();

                yield return InitDescriptor(attr, handlerMethodInfo, handlerType.GetTypeInfo(), serviceInterfaceType.GetTypeInfo(), parameters);
            }
        }

        private bool TryGetInterfaceType(Type[] genericArgs, out Type interfaceType)
        {
            switch (genericArgs.Length)
            {
                case 1:
                    interfaceType = GetRequestInterfaceType(genericArgs[0]);
                    return true;
                case 2:
                    interfaceType = GetRequestResponseInterfaceType(genericArgs[0], genericArgs[1]);
                    return true;
                default:
                    interfaceType = default;
                    return false;
            }
        }

        private Type GetRequestResponseInterfaceType(Type requestType, Type responseType)
        {
            return typeof(ICapEventHandler<,>).MakeGenericType(requestType, responseType);
        }

        private Type GetRequestInterfaceType(Type requestType)
        {
            return typeof(ICapEventHandler<>).MakeGenericType(requestType);
        }

        private ConsumerExecutorDescriptor InitDescriptor(TopicAttribute attr, MethodInfo methodInfo, TypeInfo implHandler,
            TypeInfo serviceHandlerInfo, List<ParameterDescriptor> parameters)
        {
            var descriptor = new ConsumerExecutorDescriptor
            {
                Attribute = attr,
                MethodInfo = methodInfo,
                ImplTypeInfo = implHandler,
                ServiceTypeInfo = serviceHandlerInfo,
                Parameters = parameters
            };

            return descriptor;
        }

        private CapSubscribeAttribute GetCapSubscribeAttribute(Type requestType, Type handlerType)
        {
            string group = null;
            var name = _nameService.GetName(requestType);

            var requestAttribute = requestType.GetCustomAttributes(true).OfType<IOverrideSubscribe>().FirstOrDefault();
            if (requestAttribute != null)
            {
                if (requestAttribute.TryGetName(out var overName))
                    name = overName;

                if (requestAttribute.TryGetGroup(out var overGroup))
                    group = overGroup;
            }

            // Handler attribute overrides request attribute
            var handlerAttribute = handlerType.GetCustomAttributes(true).OfType<IOverrideSubscribe>().FirstOrDefault();
            if (handlerAttribute != null)
            {
                if (handlerAttribute.TryGetName(out var overName))
                    name = overName;

                if (handlerAttribute.TryGetGroup(out var overGroup))
                    group = overGroup;
            }

            return new CapSubscribeAttribute(name)
            {
                Group = group
            };
        }
    }
}
