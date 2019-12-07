using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Logic.UnitTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private IServiceProvider Services { get; set; }

        [OneTimeSetUp]
        protected void ConfigureServices()
        {
            Services = new ServiceCollection()
                .AddInventorySystem()
                .BuildServiceProvider();
        }

        public T GetService<T>()
            => Services.GetRequiredService<T>();
    }
}
