﻿using Engine.Core.Diagnostics;
using Engine.Core.Factories;
using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Content;

using System.Diagnostics;

namespace Engine.Core
{
    public static class EngineServices
    {
        public static IServiceCollection AddEngineServices(this IServiceCollection services)
        {
            services
                .AddMonogameServices()
                .AddSingleton<IAssetManager, AssetManager>()
                .AddSingleton<ICameraManager, CameraManager>()
                .AddSingleton<IDeviceManager, DeviceManager>()
                .AddSingleton<IEntityManager, EntityManager>()
                .AddSingleton<IEntityFactory, EntityFactory>()
                .AddSingleton<IEventManager, EventManager>()
                .AddSingleton<IInputManager, InputManager>()
#if DEBUG
                .AddEngineLogging()
#endif
                .AddEngineValidation()
                .AddEngineDiagnostics();
            return services;
        }

        public static IServiceCollection AddMonogameServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<ContentManager>();
        }

        public static IServiceCollection AddEngineLogging(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });

            return services;
        }

        public static IServiceCollection AddEngineValidation(this IServiceCollection services)
        {
            services.AddSingleton<IValidator, KeyBindingValidator>();
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
