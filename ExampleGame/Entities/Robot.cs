using Engine.Core.Events;
using Engine.Core.Models;
using Engine.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Entities
{
    public class Robot : Player
    {
        public Robot()
        {
            ModelName = "Robot_Model";
            TextureName = "robotTexture_0";

            ActionOnEntity = EntityActions.UPDATEDRAW;
        }

        public override Task Update(GameTime gameTime)
        {
            if (TranslationSpeed.Z > 0)
                TranslationSpeed.Z -= 0.5f;

            return base.Update(gameTime);
        }

        public override Task Render(GameTime gameTime, Camera camera)
        {
            camera.UpdateTarget(Transform.Position3d);

            return base.Render(gameTime, camera);
        }

        protected override Task HandleCollisionEvent(KeyboardEvt @event)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleControllerEvent(KeyboardEvt @event)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleKeyboardEvent(KeyboardEvt @event)
        {
            if (@event.KeyAction.ActionName == "JUMP")
                TranslationSpeed.Z += 5;

            return Task.CompletedTask;
        }

        protected override Task HandleMouseEvent(KeyboardEvt @event)
        {
            throw new NotImplementedException();
        }
    }
}
