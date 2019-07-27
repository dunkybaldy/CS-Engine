using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        private IServiceProvider Services { get; set; }

        [OneTimeSetUp]
        protected void ConfigureServices()
        {
            Services = new ServiceCollection()
                .AddEngineServices()
                .BuildServiceProvider();
        }

        public T GetService<T>()
            => Services.GetRequiredService<T>();
    }
}
