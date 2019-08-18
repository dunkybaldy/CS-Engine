using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface IAssetManager
    {
        Model GetModel(string assetName);
        Texture2D GetTexture2D(string assetName);
        Texture3D GetTexture3D(string assetName);
    }
}
