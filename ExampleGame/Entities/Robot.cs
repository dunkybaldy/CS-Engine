using Engine.Core.Models;
using Engine.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleGame.Entities
{
    public class Robot : Player
    {
        public Robot()
        {
            ModelName = "Robot_Model";
            TextureName = "robotTexture_0";

            ActionOnEntity = EntityActions.UPDATEDRAW;
        }
    }
}
