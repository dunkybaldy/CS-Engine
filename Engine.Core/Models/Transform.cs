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
        public Vector2 Position2d { get; set; }
        public Vector2 Scale2d { get; set; }
        public Vector3 Position3d { get; set; }
        public Vector3 Scale3d { get; set; }

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
    }
}
