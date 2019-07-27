using Engine.Core.Factories.Interfaces;
using Engine.Core.Models;
using Engine.Core.Models.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Factories
{
    public class EntityFactory : IEntityFactory
    {
        public EntityFactory()
        {
        }

        public Task<T> Create<T>() where T : IEntity, new()
        {
            return Task.FromResult(new T());
        }
    }
}
