using Engine.Core.Events;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public abstract class Entity3D : IEntity3D
    {
        protected Texture2D Texture { get; set; }
        protected Model Model { get; set; }
        public string ModelName { get; protected set; }
        public string TextureName { get; protected set; }
        protected Transform Transform { get; set; }
        protected float RotationSpeed { get; set; }
        protected Vector3 TranslationSpeed { get; set; }
        public EntityActions ActionOnEntity { get; protected set; } = EntityActions.UPDATEDRAW;
        public List<EventType> SubscribedToEvents { get; protected set; }
        public EntityType EntityType = EntityType._3D;

        protected Entity3D()
        {
            Transform = new Transform(EntityType);
            RotationSpeed = 10;
            TranslationSpeed = new Vector3(0, 0, 0);
        }

        public EntityActions EntityLifeCycleAction() => ActionOnEntity;

        public virtual Task Update(GameTime gameTime)
        {
            var secs = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Transform.Angle += 1 * secs;
            Transform.Position3d += TranslationSpeed * secs;

            Transform.TranslationMatrix = Matrix.CreateTranslation(Transform.Position3d);
            Transform.RotationMatrix = Matrix.CreateRotationZ(Transform.Angle);

            return Task.CompletedTask;
        }

        public virtual Task Render(GameTime gameTime) { throw new NotImplementedException(); }

        public virtual Task Render(GameTime gameTime, Camera camera)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Transform.WorldMatrix;
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }

            return Task.CompletedTask;
        }

        public Vector3 GetPosition()
        {
            return Transform.Position3d;
        }

        public void ApplyGraphics(Model model)
        {
            Model = model;
        }

        public void ApplyGraphics(Texture2D texture)
        {
            Texture = texture;
        }

        public void ApplyGraphics(Model model, Texture2D texture)
        {
            Model = model;
            Texture = texture;
        }

        public void ApplySoundEffects(IEnumerable<SoundEffect> soundEffects)
        {
            throw new NotImplementedException();
        }

        public string GetModelName() => ModelName;
        public string GetTextureName() => TextureName;
    }
}
