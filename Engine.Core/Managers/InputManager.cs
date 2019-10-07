using Engine.Core.Managers.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class InputManager : IInputManager
    {
        private readonly IDeviceManager _deviceManager;

        public InputManager(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
        }

        public async Task Run()
        {
            while (EngineStatics.Running)
            {
                await _deviceManager.PollKeyboard();
                //await _deviceManager.PollMouse();
            }
        }
    }
}
