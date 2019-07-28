using Engine.Core.Factories;
using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class EngineServices
    {
        public static IServiceCollection AddEngineServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IEntityManager, EntityManager>()
                .AddSingleton<IEntityFactory, EntityFactory>()
#if DEBUG
                .AddEngineLogging()
                .AddEngineDiagnostics();
#endif

            return services;
        }

        public static IServiceCollection AddEngineLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });

            return services;
        }

        public static IServiceCollection AddEngineDiagnostics(this IServiceCollection services)
        {
            services.AddSingleton<Stopwatch>();

            return services;
        }
    }
}
