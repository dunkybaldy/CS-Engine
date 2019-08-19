using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models.Interfaces
{
    public interface IEntity3D : IEntity
    {
        void ApplyModel(Model model);
        void ApplyTexture3D(Texture3D texture3D);
    }
}
