using Engine.Core;
using Engine.Core.Diagnostics;
using Engine.Core.Initialiser;
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
        //Camera
        private Vector3 camTarget;
        private Vector3 camPosition;
        private float aspectRatio;

        private Matrix projectionMatrix;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        ////BasicEffect for rendering
        private BasicEffect basicEffect;

        //Geometric info
        //private VertexPositionColor[] triangleVertices;
        //private VertexBuffer vertexBuffer;

        //Orbit
        private bool orbit = false;

        /// <summary>
        /// Curently messy initialisation
        /// </summary>
        /// <param name="entityManager"></param>
        /// <param name="diagnosticsController"></param>
        /// <param name="logger"></param>
        public Game1(
            IDeviceManager deviceManager,
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,
            ILogger<Game1> logger)
            : base(deviceManager, entityManager, eventManager, diagnosticsController, logger)
        {
        }

        protected override async Task InitialiseAsync()
        {
            await base.InitialiseAsync();

            await _entityManager.Create<Robot>("Robot|1");

            camPosition = new Vector3(15, 10, 10);
            basicEffect = new BasicEffect(GraphicsDevice);
        }

        protected override async Task UnloadContentAsync()
        {
            await base.UnloadContentAsync();
        }

        protected override async Task UpdateAsync(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //{
            //    camPosition.X -= 1f;
            //    camTarget.X -= 1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //{
            //    camPosition.X += 1f;
            //    camTarget.X += 1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //{
            //    camPosition.Y -= 1f;
            //    camTarget.Y -= 1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //{
            //    camPosition.Y += 1f;
            //    camTarget.Y += 1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            //{
            //    camPosition.Z += 1f;
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            //{
            //    camPosition.Z -= 1f;
            //}
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }
            _logger.LogInformation($"camPos: {camPosition}, camTarget: {camTarget}");

            await base.UpdateAsync(gameTime);
        }

        protected override async Task DrawAsync(GameTime gameTime)
        {
            aspectRatio = _graphicsDeviceManager.PreferredBackBufferWidth / (float)_graphicsDeviceManager.PreferredBackBufferHeight;            
            _entityManager.SetCameraAspectRatio(camPosition, aspectRatio);

            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None
            };

            await base.DrawAsync(gameTime);
        }
    }
}
