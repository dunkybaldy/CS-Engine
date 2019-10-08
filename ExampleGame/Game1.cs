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

        BasicEffect effect;
        Texture2D checkerboardTexture;
        VertexPositionNormalTexture[] floorVerts;

        public Game1(
            ICameraManager cameraManager,
            IEntityManager entityManager,
            IEventManager eventManager,
            DiagnosticsController diagnosticsController,
            ILogger<Game1> logger)
            : base(cameraManager, entityManager, eventManager, diagnosticsController, logger)
        {
            //_graphicsDeviceManager.SynchronizeWithVerticalRetrace = false; //Vsync
            //IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 1000);
        }

        protected override async Task InitialiseAsync()
        {
            await base.InitialiseAsync();

            var blob = await _entityManager.Create<Robot>("Robot|1");
            await _eventManager.SubscribeToEvents(blob.SubscribedToEvents, blob);
            _cameraManager.GetMainCamera().CameraTarget = blob.GetPosition();

            floorVerts = new VertexPositionNormalTexture[6];

            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            int repetitions = 20;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
            {
                checkerboardTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }

        }

        protected override async Task UnloadContentAsync()
        {
            await base.UnloadContentAsync();
        }

        protected override async Task UpdateAsync(GameTime gameTime)
        {
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
            //DrawGround();
            await base.DrawAsync(gameTime);
        }

        private void DrawGround()
        {
            var mainCm = _cameraManager.GetMainCamera();

            effect.View = mainCm.ViewMatrix;
            effect.Projection = mainCm.ProjectionMatrix;
            effect.TextureEnabled = true;
            effect.Texture = checkerboardTexture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives(
                            PrimitiveType.TriangleList,
                    floorVerts,
                    0,
                    2);
            }
        }
    }
}
