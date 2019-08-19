using Engine.Core.Models.Enums;
using Engine.Core.Models.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public abstract class Entity3D : IEntity3D
    {
        protected Texture3D Texture3D { get; set; }
        protected Model Model { get; set; }
        protected Transform Transform { get; set; }
        protected EntityActions ActionOnEntity { get; set; }

        protected Entity3D()
        {

        }

        protected Entity3D(Transform transform, bool threeDimensional)
        {
            Transform = transform;
        }

        public EntityActions EntityLifeCycleAction() => ActionOnEntity;

        public virtual Task Update(GameTime gameTime)
        {
            return Task.CompletedTask;
        }

        //public virtual Task Render(SpriteBatch spriteBatch, GameTime gameTime)
        //{
        //    if (!ThreeDimensional)
        //        spriteBatch.Draw(Texture2D, Transform.Position2d, Color.AntiqueWhite);
        //    else
        //    {

        //    }
        //    return Task.CompletedTask;
        //}

        public virtual Task Render(GameTime gameTime)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // We could set up custom lights, but this
                    // is the quickest way to get somethign on screen:
                    effect.EnableDefaultLighting();
                    // This makes lighting look more realistic on
                    // round surfaces, but at a slight performance cost:
                    effect.PreferPerPixelLighting = true;

                    // The world matrix can be used to position, rotate
                    // or resize (scale) the model. Identity means that
                    // the model is unrotated, drawn at the origin, and
                    // its size is unchanged from the loaded content file.
                    effect.World = Matrix.Identity;
                    // Move the camera 8 units away from the origin:
                    var cameraPosition = new Vector3(0, 8, 0);
                    // Tell the camera to look at the origin:
                    var cameraLookAtVector = Vector3.Zero;
                    // Tell the camera that positive Z is up
                    var cameraUpVector = Vector3.UnitZ;

                    effect.View = Matrix.CreateLookAt(
                        cameraPosition, cameraLookAtVector, cameraUpVector);

                    // We want the aspect ratio of our display to match
                    // the entire screen's aspect ratio:
                    float aspectRatio = 1600f / 900f;
                    // Field of view measures how wide of a view our camera has.
                    // Increasing this value means it has a wider view, making everything
                    // on screen smaller. This is conceptually the same as "zooming out".
                    // It also 
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    // Anything closer than this will not be drawn (will be clipped)
                    float nearClipPlane = 1;
                    // Anything further than this will not be drawn (will be clipped)
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

        public void ApplyModel(Model model)
        {
            Model = model;
        }

        public void ApplyTexture3D(Texture3D texture3D)
        {
            Texture3D = texture3D;
        }

        public void ApplySoundEffects(IEnumerable<SoundEffect> soundEffects)
        {
            throw new NotImplementedException();
        }
    }
}
