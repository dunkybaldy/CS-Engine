using Engine.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class GameApplicationStartup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services
                .AddSingleton<GraphicsDeviceManager>();
            return services;
        }
    }
}
