using Engine.Core;
using Engine.Core.Diagnostics;
using Engine.Core.Managers.Interfaces;

using ExampleGame.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;

namespace ExampleGame
{
    public class Game1 : GameApplication
    {
        private double UpdateTime { get; set; }

        public Game1(
            ICameraManager cameraManager,
            IDeviceManager deviceManager,
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,
            ILogger<Game1> logger)
            : base(
                  cameraManager,
                  deviceManager, entityManager, eventManager, diagnosticsController, logger)
        {
            //_graphicsDeviceManager.SynchronizeWithVerticalRetrace = false; //Vsync
            //IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 1000);
        }

        protected override async Task InitialiseAsync()
        {
            await base.InitialiseAsync();

            var blob = await _entityManager.Create<Robot>("Robot|1");
            _cameraManager.GetMainCamera().CameraTarget = blob.GetPosition();
        }

        protected override async Task UnloadContentAsync()
        {
            await base.UnloadContentAsync();
        }

        protected override async Task UpdateAsync(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (DrawState.Count < 10)
            {
                UpdateTime += gameTime.ElapsedGameTime.TotalSeconds;
                var updateTime = 1D / 60;

                while (UpdateTime >= updateTime)
                {
                    await _cameraManager.Update(gameTime);

                    await base.UpdateAsync(gameTime);
                    UpdateTime -= updateTime;
                }
            }
        }

        protected override async Task DrawAsync(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None
            };

            await base.DrawAsync(gameTime);
        }
    }
}
