using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
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
        public static IServiceCollection Initialise(IServiceCollection services)
        {
            return services.AddEngineServices();
        }

        public static IServiceProvider Run<T>() where T : Game
        {
            var services = new ServiceCollection().AddEngineServices();
            return Run<T>(services);
        }

        public static IServiceProvider Run<T>(IServiceCollection services) where T : Game
        {
            services.AddSingleton<T>();
            var sp = services.BuildServiceProvider();
            sp.GetRequiredService<ILogger<EngineInitialiser>>().LogInformation("Engine booting up...");
            var gameApplication = sp.GetRequiredService<T>();
            gameApplication.Run();
            return sp;
        }

        private class EngineInitialiser
        {

        }
    }
}
