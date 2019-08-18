using Engine.Core.Models;
using Engine.Core.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddMyOwnServices(this IServiceCollection services)
        {
            // Transient for splitscreen, singleton for 1 entity for all players
            services
                .AddSingleton<ICamera, Camera>();

            return services;
        }
    }
}
