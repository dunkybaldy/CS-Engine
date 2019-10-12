using Engine.Core.Models;
using Engine.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Entities
{
    public class Ground : Entity3D
    {
        public Ground()
        {
            TextureName = "checkerboard";
            ActionOnEntity = EntityActions.DRAW;
        }
    }
}
