﻿using Engine.Core.Models.Interfaces;

using Microsoft.Xna.Framework;

using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public class Camera
    {
        public Transform Transform { get; set; } = new Transform(Enums.EntityType._3D);
        public Vector3 CameraTarget { get; set; }
        public float AspectRatio { get; set; }
        public Matrix ViewMatrix { get; set; }
        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;

                return Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, AspectRatio, nearClipPlane, farClipPlane);
            }
        }

        public Camera(float aspectRatio)
        {
            AspectRatio = aspectRatio;
            Transform.Position3d = new Vector3(15, 10, 10);
        }

        public void UpdateTarget(Vector3 target) => CameraTarget = target;

        public Task Update(GameTime gameTime)
        {
            ViewMatrix = Matrix.CreateLookAt(Transform.Position3d, CameraTarget, Vector3.UnitZ);

            // Movement code

            return Task.CompletedTask;
        }
    }
}
