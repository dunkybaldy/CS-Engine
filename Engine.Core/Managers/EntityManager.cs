﻿using Engine.Core.Factories.Interfaces;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class EntityManager : IEntityManager
    {
        private readonly IEntityFactory _entityFactory;

        private ConcurrentDictionary<string, IEntity> Entities { get; set; }

        public EntityManager(IEntityFactory entityFactory)
        {
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));

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

        public async Task DrawEntities(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var drawableEntities = Entities.Where(x => x.Value.EntityLifeCycleAction() != EntityActions.UPDATE).ToList();
            drawableEntities.ForEach(x => x.Value.Render(spriteBatch, gameTime));
            
            List<Task> renderTasks = new List<Task>();
            drawableEntities.ForEach(x => renderTasks.ToList().Add(x.Value.Render(spriteBatch, gameTime)));

            await Task.WhenAll(renderTasks);
        }
    }
}
