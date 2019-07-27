using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public class GameApplication : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly IEntityManager _entityManager;
        protected readonly ILogger<GameApplication> _logger;
        private SpriteBatch _spriteBatch;

        private ConcurrentQueue<ConcurrentBag<IEntity>> DrawState { get; set; }

        public GameApplication(IEntityManager entityManager, ILogger<GameApplication> logger)
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _entityManager = entityManager ?? throw new ArgumentNullException(nameof(entityManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            DrawState = new ConcurrentQueue<ConcurrentBag<IEntity>>();
        }

        protected virtual Task InitialiseAsync()
        {
            base.Initialize();
            return Task.CompletedTask;
        }

        protected virtual Task LoadContentAsync()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            return Task.CompletedTask;
        }

        protected virtual Task UnloadContentAsync()
        {
            base.UnloadContent();
            return Task.CompletedTask;
        }

        protected virtual async Task UpdateAsync(GameTime gameTime)
        {
            var entitiesToEnqueue = await _entityManager.UpdateEntities(gameTime);

            // Not equal to update because we only want to draw entities which can be drawn
            var bag = new ConcurrentBag<IEntity>(entitiesToEnqueue.Where(x => x.EntityLifeCycleAction() != EntityActions.UPDATE));

            DrawState.Enqueue(bag);
            base.Update(gameTime);
        }

        protected virtual async Task DrawAsync(GameTime gameTime)
        {
            List<Task> renderTasks = new List<Task>();
            if (!DrawState.TryDequeue(out ConcurrentBag<IEntity> entitiesToDraw))
                _logger.LogError("Something in DrawAsync went wrong");

            foreach (var entity in entitiesToDraw)
                renderTasks.ToList().Add(entity.Render(_spriteBatch, gameTime));

            await Task.WhenAll(renderTasks);

            base.Draw(gameTime);
        }

        #region Game Implementation
        protected override async void Initialize()
        {
            await InitialiseAsync();
        }

        protected override async void LoadContent()
        {
            await LoadContentAsync();
        }

        protected override async void UnloadContent()
        {
            await UnloadContentAsync();
        }

        protected override async void Update(GameTime gameTime)
        {
            await UpdateAsync(gameTime);
        }

        protected override async void Draw(GameTime gameTime)
        {
            await DrawAsync(gameTime);
        }
        #endregion Game Implementation
    }
}
