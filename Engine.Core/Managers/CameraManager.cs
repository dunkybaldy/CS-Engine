using Engine.Core.Managers.Interfaces;
using Engine.Core.Models;

using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class CameraManager : ICameraManager
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private float AspectRatio { get; set; }        
        public Dictionary<int, Camera> Cameras { get; set; }
        private int Id { get; set; }

        public CameraManager(IGraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = (GraphicsDeviceManager)graphicsDeviceManager;
            AspectRatio = _graphicsDeviceManager.PreferredBackBufferWidth / (float)_graphicsDeviceManager.PreferredBackBufferHeight;
            
            Cameras = new Dictionary<int, Camera>();
            CreateCamera(); // There must always be at least one camera
        }

        public Task<Camera> CreateCamera()
        {
            var camera = new Camera(AspectRatio);

            Cameras.Add(++Id, camera);
            return Task.FromResult(camera);
        }

        public async Task Update(GameTime gameTime)
        {
            var updateTasks = new List<Task>();
            Cameras.Values.ToList().ForEach(camera => updateTasks.Add(camera.Update(gameTime)));
            await Task.WhenAll(updateTasks);
        }

        public Camera GetMainCamera()
        {
            return Cameras[1];
        }
    }
}
