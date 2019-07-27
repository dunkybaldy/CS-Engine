using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models.Interfaces
{
    public interface IEntity
    {
        EntityActions EntityLifeCycleAction();
        Task Render(SpriteBatch spriteBatch, GameTime gameTime);
        Task Update(GameTime gameTime);
    }
}
