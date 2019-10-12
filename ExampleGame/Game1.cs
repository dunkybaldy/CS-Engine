using Engine.Core;
using Engine.Core.Diagnostics;
using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;

using ExampleGame.Entities;

using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleGame
{
    public class Game1 : GameApplication, IEventSubscriber
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

            eventManager.SubscribeToEvents(
                new List<EventCategory> { EventCategory.KEYBOARD }, 
                this);
        }

        protected override async Task InitialiseAsync()
        {
            await base.InitialiseAsync();

            var robot = await _entityManager.Create<Robot>("Robot|1");
            await _eventManager.SubscribeToEvents(robot.SubscribedToEvents, robot);
            _cameraManager.GetMainCamera().CameraTarget = robot.GetPosition();

            var ground = await _entityManager.Create<Ground>("Ground");

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

            //using (var stream = TitleContainer.OpenStream("Content/checkerboard.png"))
            //{
            //    checkerboardTexture = Texture2D.FromStream(GraphicsDevice, stream);
            //}
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

        public Task HandleEvent(EngineEvt @event)
        {
            // Only handles key board for now
            if (@event.EventType == EventType.KEY_PRESSED)
            {
                var realEvent = (KeyPressEvt)@event;
                if (realEvent.KeyBinding.KeyName == Keys.Escape)
                    Exit(); // actually want to open menu, so perhaps a scene manager or game manager should listen for this key event
            }

            return Task.CompletedTask;
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
    }
}
