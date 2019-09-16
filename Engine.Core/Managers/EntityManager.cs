using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class EntityManager : IEntityManager
    {
        private readonly IAssetManager _assetManager;
        private readonly ICameraManager _cameraManager;
        private readonly IEntityFactory _entityFactory;
        private readonly IEventManager _eventManager;
        private readonly ILogger<EntityManager> _logger;
        private readonly Stopwatch _stopwatch;

        private ConcurrentDictionary<string, IEntity3D> Entities { get; set; }

        public EntityManager(ICameraManager cameraManager, IAssetManager assetManager, IEntityFactory entityFactory, IEventManager eventManager, ILogger<EntityManager> logger, Stopwatch stopwatch)
        {
            _assetManager = assetManager ?? throw new ArgumentNullException(nameof(assetManager));
            _cameraManager = cameraManager ?? throw new ArgumentNullException(nameof(cameraManager));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stopwatch = stopwatch ?? null;

            Entities = new ConcurrentDictionary<string, IEntity3D>();
        }

        public async Task<T> Create<T>() where T : IEntity3D, new()
        {
            var entity = await _entityFactory.Create<T>();
            Entities.TryAdd($"{nameof(entity)}|{Guid.NewGuid().ToString()}", entity);
            return entity;
        }

        public async Task<T> Create<T>(string id) where T : IEntity3D, new()
        {
            var entity = await _entityFactory.Create<T>();

            if (entity is IEntity3D)
            {
                var entity3d = entity as IEntity3D;
                entity3d.ApplyGraphics(_assetManager.GetModel(entity3d.GetModelName()), _assetManager.GetTexture2D(entity3d.GetTextureName()));
            }

            Entities.TryAdd(id, entity);
            return entity;
        }

        public IEntity3D GetEntity(string id)
        {
            if (Entities.TryGetValue(id, out IEntity3D entity))
                return entity;
            else
                throw new KeyNotFoundException($"No entry in dictionary: {id}");
        }

        public async Task<List<IEntity3D>> UpdateEntities(GameTime gameTime)
        {
            var updatableEntities = Entities.Where(x => x.Value.EntityLifeCycleAction() != EntityActions.DRAW).ToList();

            List<Task> updateTasks = new List<Task>();
            updatableEntities.ForEach(x => updateTasks.ToList().Add(x.Value.Update(gameTime)));

            await Task.WhenAll(updateTasks);

            return updatableEntities.Select(x => x.Value).ToList();
        }

        public async Task DrawEntities(GameTime gameTime)
        {
            var drawableEntities = Entities.Where(x => x.Value.EntityLifeCycleAction() != EntityActions.UPDATE).ToList();

            List<Task> renderTasks = new List<Task>();
            drawableEntities.ForEach(x => renderTasks.ToList().Add(x.Value.Render(gameTime)));

            await Task.WhenAll(renderTasks);
        }

        public async Task DrawEntities(GameTime gameTime, List<IEntity3D> entitiesToDraw)
        {
            List<Task> renderTasks = new List<Task>();
            entitiesToDraw.ForEach(x => renderTasks.Add(x.Render(gameTime, _cameraManager.GetMainCamera())));
            await Task.WhenAll(renderTasks);
        }
    }
}
