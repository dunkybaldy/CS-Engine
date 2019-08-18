using Engine.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Entities
{
    public class Triangle : Entity
    {
        public Triangle()
        {
        }

        public override Task Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            return base.Render(spriteBatch, gameTime);
        }

        public override Task Update(GameTime gameTime)
        {
            return base.Update(gameTime);
        }
    }
}
