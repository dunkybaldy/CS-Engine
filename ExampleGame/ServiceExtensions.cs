using Engine.Core.Models;
using Engine.Core.Models.Interfaces;
using Engine.Core.Models.Options;
using ExampleGame.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleGame
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyOwnServices(this IServiceCollection services)
        {
            // Transient for splitscreen, singleton for 1 entity for all players
            services
                .AddGameOptions()
                .AddSingleton<TestService>();

            return services;
        }

        private static IServiceCollection AddGameOptions(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("DefaultOptions.json", false, true)
                .Build();

            services.AddSingleton(config);

            services.Configure<KeyboardOptions>(config.GetSection("KeyOptions"));

            //var activeConfiguration = config.GetSection("User");
            
            //if (config.GetValue<bool>("UseDefaultSettings"))
            //    activeConfiguration = config.GetSection("Default");

            //services
            //    .Configure<GeneralSettings>(activeConfiguration.GetSection("GeneralSettings"))
            //    .Configure<InGameSettings>(activeConfiguration.GetSection("InGameSettings"));

            return services;
        }
    }
}
