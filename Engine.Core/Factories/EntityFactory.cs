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
            return Task.FromResult(new T());
        }

        public Task<TChild> Create<TParent, TChild>() where TChild : TParent, new()
        {
            var entity = new TChild();

            if (typeof(TParent) is IEntity3D)
            {
                var e = entity as IEntity3D;
                e.ApplyGraphics(_assetManager.GetModel(nameof(TChild)), _assetManager.GetTexture2D(nameof(TChild)));
            }
            else
            {
                // 2D stuff here 
            }

            return Task.FromResult(entity);
        }
    }
}
