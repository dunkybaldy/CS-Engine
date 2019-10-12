using Engine.Core.Events;
using Engine.Core.Models;
using Engine.Core.Models.Enums;
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
        private bool _jumping = false;
        private bool _canJump = false;

        public Robot()
        {
            ModelName = "Robot_Model";
            TextureName = "robotTexture_0";

            ActionOnEntity = EntityActions.UPDATEDRAW;
        }

        public override Task Update(GameTime gameTime)
        {
            if (_canJump && !_jumping)
            {
                TranslationSpeed.Z += 5;
                _canJump = false;
                _jumping = true;
            }

            if (Transform.Position3d.Z > 0)
                TranslationSpeed.Z -= 0.1f;
            else if (Transform.Position3d.Z < 0)
            {
                TranslationSpeed.Z = 0;
                Transform.Position3d.Z = 0;
                _jumping = false;
            }

            return base.Update(gameTime);
        }

        public override Task Render(GameTime gameTime, Camera camera)
        {
            camera.UpdateTarget(Transform.Position3d);

            return base.Render(gameTime, camera);
        }

        //protected override Task HandleCollisionEvent(KeyEvt @event)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override Task HandleControllerEvent(KeyEvt @event)
        //{
        //    throw new NotImplementedException();
        //}

        protected override Task HandleKeyboardEvent(KeyEvt @event)
        {
            if (@event.KeyBinding.KeyName == Microsoft.Xna.Framework.Input.Keys.Space)
                _canJump = true;

            return Task.CompletedTask;
        }

        //protected override Task HandleMouseEvent(KeyEvt @event)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
