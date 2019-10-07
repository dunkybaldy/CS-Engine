using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class DeviceManager : IDeviceManager
    {
        private readonly IEventManager _eventManager;
        private readonly KeyboardOptions _keyboardOptions;
        private readonly ILogger<DeviceManager> _logger;

        private KeyboardState KeyboardState { get; set; }
        private MouseState MouseState { get; set; }

        private KeyboardState PreviousKeyboardState { get; set; }
        private MouseState PreviousMouseState { get; set; }


        public DeviceManager(IEventManager eventManager, IOptions<KeyboardOptions> keyboardOptions, ILogger<DeviceManager> logger)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _keyboardOptions = keyboardOptions.Value ?? throw new ArgumentNullException(nameof(keyboardOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PollKeyboard()
        {
            KeyboardState = Keyboard.GetState();

            var keysDown = KeyboardState.GetPressedKeys().ToList();
            foreach (var key in keysDown)
            {
                _logger.LogInformation($"{key.ToString()}");
            }

            //var keysICareAbout = keysDown.Where(x => _keyboardOptions.BoundKeyActions.Keys.Contains(x.ToString()));
            var keysICareAbout = new List<KeyAction>();

            foreach (var key in keysDown)
                keysICareAbout.Add(_keyboardOptions.KeyActions.First(x => x.KeyName == key));

            if (keysICareAbout.Any())
            {
                var eventTasks = CreateKeyboardTasks(keysICareAbout);

                await Task.WhenAll(eventTasks);
            }
            PreviousKeyboardState = KeyboardState;
        }

        private List<Task> CreateKeyboardTasks(IEnumerable<KeyAction> keyActions)
        { 
            var tasks = new List<Task>();

            foreach (var keyAction in keyActions)
            {
                tasks.Add(_eventManager.PublishEvent(
                    new KeyboardEvt
                    {
                        KeyAction = keyAction
                    }
                ));
            }

            return tasks;
        }
    }
}
