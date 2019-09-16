using Engine.Core;
using Engine.Core.Diagnostics;
using Engine.Core.Managers.Interfaces;

using ExampleGame.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Threading.Tasks;

namespace ExampleGame
{
    public class Game1 : GameApplication
    {
        public Game1(
            ICameraManager cameraManager,
            IDeviceManager deviceManager,
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,
            ILogger<Game1> logger)
            : base(cameraManager, deviceManager, entityManager, eventManager, diagnosticsController, logger)
        {
        }

        protected override async Task InitialiseAsync()
        {
            await base.InitialiseAsync();

            await _entityManager.Create<Robot>("Robot|1");
        }

        protected override async Task UnloadContentAsync()
        {
            await base.UnloadContentAsync();
        }

        protected override async Task UpdateAsync(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            await _cameraManager.Update(gameTime);

            await base.UpdateAsync(gameTime);
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
