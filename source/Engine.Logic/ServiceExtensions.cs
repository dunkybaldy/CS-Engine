using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Logic
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            return services;
        }
    }
}
