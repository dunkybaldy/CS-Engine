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
                .AddEngineLogging();

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
    }
}
