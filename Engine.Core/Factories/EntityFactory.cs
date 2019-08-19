using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers.Interfaces;
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
        private readonly IAssetManager _assetManager;

        public EntityFactory(IAssetManager assetManager)
        {
            _assetManager = assetManager ?? throw new ArgumentNullException(nameof(assetManager));
        }

        public Task<T> Create<T>() where T : IEntity, new()
        {
            var entity = new T();

            return Task.FromResult(new T());
        }

        public Task<TChild> Create<TParent, TChild>() where TChild : TParent, new()
        {
            var entity = new TChild();

            if (typeof(TParent) is IEntity3D)
            {
                var e = (IEntity3D)entity;
                e.ApplyModel(_assetManager.GetModel(nameof(TChild)));
                e.ApplyTexture3D(_assetManager.GetTexture3D(nameof(TChild)));
            }
            else
            {
                // 2D stuff here 
            }

            return Task.FromResult(entity);
        }
    }
}
