using Engine.Core.Diagnostics;
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
        protected GraphicsDeviceManager _graphicsDeviceManager;
        protected readonly IEntityManager _entityManager;
        protected readonly IEventManager _eventManager;
        protected readonly DiagnosticsController _diagnosticsController;
        protected readonly ILogger _logger;
        protected SpriteBatch _spriteBatch;

        protected string GameTitle { get; set; } = "Game";
        private bool GameRunning { get; set; } = true;

        private ConcurrentQueue<ConcurrentBag<IEntity>> DrawState { get; set; }

        public GameApplication(
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,   
            ILogger logger)
        {
            Content.RootDirectory = "Content";

            _entityManager = entityManager ?? throw new ArgumentNullException(nameof(entityManager));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _diagnosticsController = diagnosticsController ?? throw new ArgumentNullException(nameof(diagnosticsController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            DrawState = new ConcurrentQueue<ConcurrentBag<IEntity>>();
        }

        protected virtual Task InitialiseAsync()
        {
            base.Initialize();
            // Get engine advertisment screen on there
            // Screen set up

            return Task.CompletedTask;
        }

        protected virtual Task LoadContentAsync()
        {
            base.LoadContent();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // Set up all the models?

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
            if (DrawState.TryDequeue(out ConcurrentBag<IEntity> entitiesToDraw))
                await _entityManager.DrawEntities(gameTime, entitiesToDraw.ToList());
            else
                _logger.LogError("Attempted to dequeue from DrawState but returned false");

            await _entityManager.DrawEntities(gameTime);

            base.Draw(gameTime);
        }

        public void RunGame()
        {
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{MethodName} caught an exception. Ending application early.", nameof(RunGame));
                throw;
            }            
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            Window.Title = GameTitle;
            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            Window.Title = $"{GameTitle} | Deactive";
            // Open Menu pause
            base.OnDeactivated(sender, args);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            GameRunning = false;
            base.OnExiting(sender, args);
        }

        #region Game Implementation
        protected override async void Initialize()
        {
            await _diagnosticsController.DiagnoseTask(InitialiseAsync(), nameof(InitialiseAsync));
        }

        protected override async void LoadContent()
        {
            await _diagnosticsController.DiagnoseTask(LoadContentAsync(), nameof(LoadContentAsync));
        }

        protected override async void UnloadContent()
        {
            await _diagnosticsController.DiagnoseTask(UnloadContentAsync(), nameof(UnloadContentAsync));
        }

        protected override async void Update(GameTime gameTime)
        {
            await _diagnosticsController.DiagnoseTask(UpdateAsync(gameTime), nameof(UpdateAsync));
        }

        protected override async void Draw(GameTime gameTime)
        {
            await _diagnosticsController.DiagnoseTask(DrawAsync(gameTime), nameof(DrawAsync));
        }
        #endregion Game Implementation
    }
}
