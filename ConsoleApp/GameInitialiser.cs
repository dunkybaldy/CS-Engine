namespace ConsoleApp
{
    public static class GameInitialiser
    {
        public static void Main()
        {
            var serviceCollection = Engine.Core.Engine.AddEngineServices<Game1>()
               .AddMyOwnServices();

            Engine.Core.Engine.Run(serviceCollection);
        }
    }
}
