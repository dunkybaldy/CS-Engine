using Engine.Core.Models.Enums;

using Microsoft.Xna.Framework;

namespace Engine.Core.Models
{
    public class Transform
    {
        public Vector2 Position2d { get; set; }
        public Vector2 Scale2d { get; set; }
        public Vector3 Position3d { get; set; }
        public Vector3 Scale3d { get; set; }
        public float Angle { get; set; } = 0;
        public Matrix TranslationMatrix { get; set; }
        public Matrix RotationMatrix { get; set; }
        public Matrix WorldMatrix
        {
            get
            {
                return TranslationMatrix * RotationMatrix;
            }
        }

        public Transform()
        {
            Position2d = new Vector2();
            Scale2d = new Vector2();
            Position3d = new Vector3();
            Scale3d = new Vector3();
            TranslationMatrix = new Matrix();
            RotationMatrix = new Matrix();
        }
        public Transform(EntityType type)
        {
            switch (type)
            {
                case EntityType._2D:
                    Position2d = new Vector2();
                    Scale2d = new Vector2();
                    break;
                case EntityType._3D:
                    Position3d = new Vector3();
                    Scale3d = new Vector3();
                    TranslationMatrix = new Matrix();
                    RotationMatrix = new Matrix();
                    break;
                default:
                    break;
            }
        }
    }
}
