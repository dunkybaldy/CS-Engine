﻿using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class EntityManager : IEntityManager
    {
        private readonly IAssetManager _assetManager;
        private readonly IEntityFactory _entityFactory;
        private readonly IEventManager _eventManager;
        private readonly ILogger<EntityManager> _logger;
        private readonly Stopwatch _stopwatch;

        private ConcurrentDictionary<string, IEntity> Entities { get; set; }

        private Matrix ProjectionMatrix;
        private Matrix ViewMatrix;
        private Matrix WorldMatrix;

        public Vector3 CameraPosition { get; set; }
        public float AspectRatio { get; set; }

        public EntityManager(IAssetManager assetManager, IEntityFactory entityFactory, IEventManager eventManager, ILogger<EntityManager> logger, Stopwatch stopwatch)
        {
            _assetManager = assetManager ?? throw new ArgumentNullException(nameof(assetManager));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stopwatch = stopwatch ?? null;

            Entities = new ConcurrentDictionary<string, IEntity>();
        }

        public async Task<T> Create<T>() where T : IEntity, new()
        {
            var entity = await _entityFactory.Create<T>();
            Entities.TryAdd($"{nameof(entity)}|{Guid.NewGuid().ToString()}", entity);
            return entity;
        }

        public async Task<T> Create<T>(string id) where T : IEntity, new()
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

        public IEntity GetEntity(string id)
        {
            if (Entities.TryGetValue(id, out IEntity entity))
                return entity;
            else
                throw new KeyNotFoundException($"No entry in dictionary: {id}");
        }

        public async Task<List<IEntity>> UpdateEntities(GameTime gameTime)
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

        public async Task DrawEntities(GameTime gameTime, List<IEntity> entitiesToDraw)
        {
            List<Task> renderTasks = new List<Task>();
            entitiesToDraw.ForEach(x => renderTasks.Add(x.Render(gameTime, CameraPosition, AspectRatio)));
            await Task.WhenAll(renderTasks);
        }

        public void SetCameraAspectRatio(Vector3 cameraPosition, float aspectRatio)
        {
            CameraPosition = cameraPosition;
            AspectRatio = aspectRatio;
        }

        public Task SetViewForDraw(Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        {
            //ProjectionMatrix = projectionMatrix;
            ViewMatrix = viewMatrix;
            //WorldMatrix = worldMatrix;
            return Task.CompletedTask;
        }
    }
}
