using Engine.Core.Initialiser;
using Engine.Core.Managers.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class Engine
    {
        /// <summary>
        /// Engine Entry Point
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameServices"></param>
        /// <returns></returns>
        public static int Run<T>(IServiceCollection gameServices = null) where T : GameApplication
        {
            try
            {
                var services = new ServiceCollection().Initialise(gameServices);
                var serviceProvider = BuildServices<T>(services);

                // Add per thread methods here
                Task.Factory.StartNew(RunEventSystem(serviceProvider));

                // Apply graphics device
                //serviceProvider.GetRequiredService
                // Main Run method must be on the main thread (this thread)
                RunGame<T>(serviceProvider);

                return 0;
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder($"Application Level caught a(n) {nameof(ex)}.\r\n");
                sb.Append($"ExceptionMessage: {ex.Message} \r\n");
                sb.Append($"StackTrace: {ex.StackTrace} \r\n");
                var message = sb.ToString();
                Console.WriteLine(message);
                throw;
            }
        }

        private static IServiceCollection Initialise(this IServiceCollection services, IServiceCollection gameServices = null)
        {
            if (gameServices != null)
            {
                foreach (var s in gameServices)
                {
                    services.Add(s);
                }
            }
            return services.AddEngineServices();
        }

        private static IServiceProvider BuildServices<T>(IServiceCollection services) where T : GameApplication
        {
            services.AddSingleton<T>();
            services.AddSingleton(provider => provider
                .GetRequiredService<T>()
                .Services
                .GetService<IGraphicsDeviceManager>());
            services.AddSingleton(provider => provider
                .GetRequiredService<T>()
                .Services
                .GetService<IGraphicsDeviceService>());
            IServiceProvider sp = services.BuildServiceProvider();
            sp.GetRequiredService<ILogger<EngineInitialiser>>().LogInformation("Engine booting up...");

            return sp;
        }

        private static void RunGame<T>(IServiceProvider services) where T : GameApplication
        {
            var gameApplication = services.GetRequiredService<T>();
            gameApplication.Run();
        }

        private static Action RunEventSystem(IServiceProvider services)
        {
            var eventSystem = services.GetRequiredService<IEventManager>();
            return () => eventSystem.Begin();
        }

        private class EngineInitialiser
        {

        }
    }
}
