using Engine.Core.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface ICameraManager
    {
        void SetGraphicsDeviceManager(GraphicsDeviceManager graphicsDeviceManager);
        Task Update(GameTime gameTime);
        Camera GetMainCamera();
    }
}
