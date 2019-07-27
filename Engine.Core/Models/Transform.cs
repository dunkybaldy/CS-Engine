using Engine.Core.Models.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public class Transform
    {
        private Vector2 Position2d { get; set; }
        private Vector2 Scale2d { get; set; }
        private Vector3 Position3d { get; set; }
        private Vector3 Scale3d { get; set; }

        public Transform() {}

        public Transform(Vector2 position2d, Vector2 scale2d)
        {
            Position2d = position2d;
            Scale2d = scale2d;
        }

        public Transform(Vector3 position3d, Vector3 scale3d)
        {
            Position3d = position3d;
            Scale3d = scale3d;
        }

        public Vector2 TransformPosition2d(Vector2 transform) => Position2d += transform;
        public Vector2 TransformScale2d(Vector2 transform) => Scale2d += transform;
        public Vector3 TransformPosition3d(Vector3 transform) => Position3d += transform;
        public Vector3 TransformScale3d(Vector3 transform) => Scale3d += transform;
    }
}
