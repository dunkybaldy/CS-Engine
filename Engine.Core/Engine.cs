﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class Engine
    {
        /// <summary>
        /// Initialise your application with the engine
        /// </summary>
        /// <typeparam name="T">Class which inherits from GameApplication</typeparam>
        /// <returns>a service collection for you to add any other </returns>
        public static IServiceCollection AddEngineServices<T>() where T : Game
        {
            var services = new ServiceCollection()
                .ConfigureServices()
                .AddEngineServices()

                .AddSingleton<Game, T>();

            //services.GetRequiredService<ILogger<Engine>>().LogInformation("Engine initialised");

            return services;
        }

        public static void Run(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<ILogger<E>>().LogInformation("Engine booting up...");
            sp.GetRequiredService<Game>().Run();
        }

        internal class E
        {

        }
    }
}
