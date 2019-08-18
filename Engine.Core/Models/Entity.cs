using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public abstract class Entity : IEntity
    {
        protected bool ThreeDimensional { get; set; }
        protected Texture2D Texture2D { get; set; }
        protected Texture3D Texture3D { get; set; }
        protected Rectangle Hitbox2d { get; set; }
        protected Transform Transform { get; set; }
        protected EntityActions ActionOnEntity { get; set; }

        public Entity()
        {

        }

        public Entity(Transform transform, bool threeDimensional)
        {
            Transform = transform;
            ThreeDimensional = threeDimensional;
        }

        public EntityActions EntityLifeCycleAction() => ActionOnEntity;

        public virtual Task Update(GameTime gameTime)
        {
            return Task.CompletedTask;
        }

        public virtual Task Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!ThreeDimensional)
                spriteBatch.Draw(Texture2D, Transform.Position2d, Color.AntiqueWhite);
            return Task.CompletedTask;
        }
    }
}
