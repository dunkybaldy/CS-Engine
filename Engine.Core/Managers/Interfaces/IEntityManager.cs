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
        Task<T> Create<T>() where T : IEntity3D, new();
        Task<T> Create<T>(string id) where T : IEntity3D, new();
        IEntity3D GetEntity(string id);
        Task<List<IEntity3D>> UpdateEntities(GameTime gameTime);
        Task DrawEntities(GameTime gameTime);
        Task DrawEntities(GameTime gameTime, List<IEntity3D> entitiesToDraw);
    }
}
