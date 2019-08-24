using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ExampleGame
{
    public static class GameInitialiser
    {
        public static void Main()
        {
            var services = new ServiceCollection().AddMyOwnServices();

            Engine.Core.Engine.Run<Game1>(services);
        }
    }
}
