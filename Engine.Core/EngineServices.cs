using Engine.Core.Diagnostics;
using Engine.Core.Factories;
using Engine.Core.Factories.Interfaces;
using Engine.Core.Initialiser;
using Engine.Core.Managers;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models;
using Engine.Core.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
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
        internal static IServiceCollection AddEngineServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IAssetManager, AssetManager>()
                .AddSingleton<IEntityManager, EntityManager>()
                .AddSingleton<IEntityFactory, EntityFactory>()
                .AddSingleton<IEventManager, EventManager>()
                .AddSingleton<IGraphicsDeviceService, MyGraphicsDeviceManager>()
#if DEBUG
                .AddEngineLogging()
#endif
                .AddEngineDiagnostics()
                ;
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
            services
                .AddSingleton<DiagnosticsController>()
                .AddSingleton<Stopwatch>();

            return services;
        }
    }
}
