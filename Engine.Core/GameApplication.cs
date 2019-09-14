using Engine.Core.Diagnostics;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core
{
    public class GameApplication : Game
    {
        private readonly IDeviceManager _deviceManager;
        protected readonly IEntityManager _entityManager;
        protected readonly IEventManager _eventManager;
        protected GraphicsDeviceManager _graphicsDeviceManager;
        protected readonly DiagnosticsController _diagnosticsController;
        protected readonly ILogger _logger;
        protected SpriteBatch _spriteBatch;

        protected string GameTitle { get; set; } = "Game";

        private ConcurrentQueue<ConcurrentBag<IEntity>> DrawState { get; set; }

        public GameApplication(
            IDeviceManager deviceManager,
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,
            ILogger logger)
        {
            Content.RootDirectory = "Content";

            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _entityManager = entityManager ?? throw new ArgumentNullException(nameof(entityManager));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _diagnosticsController = diagnosticsController ?? throw new ArgumentNullException(nameof(diagnosticsController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _graphicsDeviceManager = new GraphicsDeviceManager(this);

            DrawState = new ConcurrentQueue<ConcurrentBag<IEntity>>();
        }

        protected virtual Task InitialiseAsync()
        {
            try
            {
                base.Initialize();
                // Get engine advertisment screen on there
                // Screen set up

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected virtual Task LoadContentAsync()
        {
            try
            { 
                base.LoadContent();
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                // Set up all the models?

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected virtual Task UnloadContentAsync()
        {
            try
            {
                base.UnloadContent();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected virtual async Task UpdateAsync(GameTime gameTime)
        {
            try
            {
                //await _deviceManager.PollState();
                var entitiesToEnqueue = await _entityManager.UpdateEntities(gameTime);

                // Not equal to update because we only want to draw entities which can be drawn
                var bag = new ConcurrentBag<IEntity>(entitiesToEnqueue.Where(x => x.EntityLifeCycleAction() != EntityActions.UPDATE));

                DrawState.Enqueue(bag);

                base.Update(gameTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected virtual async Task DrawAsync(GameTime gameTime)
        {
            try
            {
                if (DrawState.TryDequeue(out ConcurrentBag<IEntity> entitiesToDraw))
                    await _entityManager.DrawEntities(gameTime, entitiesToDraw.ToList());
                else
                    _logger.LogWarning("Attempted to dequeue from DrawState but returned false");

                base.Draw(gameTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
            // quicksave or something
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
