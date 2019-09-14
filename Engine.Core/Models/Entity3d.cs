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
        protected EntityActions ActionOnEntity { get; set; } = EntityActions.UPDATEDRAW;
        public List<EventType> SubscribedToEvents { get; protected set; }

        protected Entity3D()
        {
            Transform = new Transform();
        }

        public EntityActions EntityLifeCycleAction() => ActionOnEntity;

        public virtual Task Update(GameTime gameTime)
        {
            Transform.Angle += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            return Task.CompletedTask;
        }

        public virtual Task Render(GameTime gameTime) { throw new NotImplementedException(); }

        public virtual Task Render(GameTime gameTime, Vector3 cameraPosition, float aspectRatio)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = GetWorldMatrix();
                    var cameraLookAtVector = Vector3.Zero;
                    var cameraUpVector = Vector3.UnitZ;

                    effect.View = Matrix.CreateLookAt(
                        cameraPosition, cameraLookAtVector, cameraUpVector);

                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    float nearClipPlane = 1;
                    float farClipPlane = 200;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }

            return Task.CompletedTask;
        }

        protected virtual Matrix GetWorldMatrix()
        {
            const float circleRadius = 8;
            const float heightOffGround = 3;

            // this matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(
                circleRadius, 0, heightOffGround);

            // this matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationZ(Transform.Angle);

            // We combine the two to have the model move in a circle:
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
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
