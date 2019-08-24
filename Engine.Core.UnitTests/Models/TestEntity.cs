﻿using Engine.Core.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.UnitTests.Models
{
    public class TestEntity : Entity3D
    {
        public TestEntity(Transform transform, bool threeDimensional = true) 
            : base(transform, threeDimensional)
        {
        }
    }
}
