using Engine.Core.Models.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface IEntityManager
    {
        Task<T> Create<T>() where T : IEntity, new();
        Task<T> Create<T>(string id) where T : IEntity, new();
        IEntity GetEntity(string id);
        Task<List<IEntity>> UpdateEntities(GameTime gameTime);
        Task DrawEntities(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
