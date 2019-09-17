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
        string GetModelName();
        string GetTextureName();
        void ApplyGraphics(Model model);
        void ApplyGraphics(Texture2D texture);
        void ApplyGraphics(Model model, Texture2D texture);
        Vector3 GetPosition();
    }
}
