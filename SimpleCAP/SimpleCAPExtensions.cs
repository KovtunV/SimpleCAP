using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using SimpleCAP.Bus;
using SimpleCAP.Contracts;
using SimpleCAP.Contracts.Bus;
using SimpleCAP.Handlers;
using SimpleCAP.Options;
using SimpleCAP.ReplacedServices;
using SimpleCAP.Services;

namespace SimpleCAP
{
    /// <summary>
    /// SimpleCAPExtensions
    /// </summary>
    public static class SimpleCAPExtensions
    {
        /// <summary>
        /// Add simple CAP wrapper
        /// </summary>
        public static IServiceCollection AddSimpleCap(this IServiceCollection services, Action<CapOptions> capAction)
        {
            // Services
            services.AddSingleton<IConsumerServiceSelector, SimpleCAPConsumerServiceSelector>();
            services.AddSingleton<IEndpointNameService, EndpointNameService>();
            services.AddSingleton<IKeyGeneratorService, KeyGeneratorService>();

            // Bus
            services.AddOptions<CallbackOptions>();
            services.AddTransient<IBusCallbackSubscriber, BusCallbackSubscriber>();
            services.AddSingleton<IBusCallbackService, BusCallbackService>();
            services.AddSingleton<IValueConverterService, ValueConverterService>();
            services.AddTransient<ISimpleCapBus, SimpleCapBus>();

            // Event handlers
            services.AddOptions<CapEventHandlerOptions>();
            services.RegisterCapEventHandlers();

            // Original CAP
            services.AddCap(capAction);

            return services;
        }

        private static void RegisterCapEventHandlers(this IServiceCollection services)
        {
            var currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();
            var handlerInterfaceType = typeof(ICapEventHandler);
            var handlerTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var filteredTypes = assembly.ExportedTypes
                    .Where(w => w.IsClass)
                    .Where(w => !w.IsAbstract)
                    .Where(w => handlerInterfaceType.IsAssignableFrom(w));
                handlerTypes.AddRange(filteredTypes);
            }

            // Add handlers
            services.Configure<CapEventHandlerOptions>(opt => handlerTypes.ForEach(opt.AddHandler));

            // Add DI
            handlerTypes.ForEach(t => services.AddTransient(t));
        }

    }
}
