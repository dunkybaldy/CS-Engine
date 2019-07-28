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
            // Add your own services here.

            return services;
        }
    }
}
