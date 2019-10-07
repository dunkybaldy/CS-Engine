using Engine.Core.Managers.Interfaces;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<InputManager> _logger;

        public InputManager(IDeviceManager deviceManager, ILogger<InputManager> logger)
        {
            _deviceManager = deviceManager ?? throw new ArgumentNullException(nameof(deviceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Run()
        {
            try
            {
                while (EngineStatics.Running)
                {
                    await _deviceManager.PollKeyboard();
                    //await _deviceManager.PollMouse();
                }

                _logger.LogInformation("Finished inputmanager run");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong in InputManager.Run");
            }
        }
    }
}
