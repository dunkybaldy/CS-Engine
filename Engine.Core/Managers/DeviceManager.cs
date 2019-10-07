using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Enums;
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

            //var keysICareAbout = keysDown.Where(x => x == _keyboardOptions.KeyActions.First(y => y.KeyName == x));
            var keysICareAbout = new List<KeyAction>();

            foreach (var key in keysDown)
            {
                if (_keyboardOptions.KeyActions.Any(x => x.KeyName == key))
                    keysICareAbout.Add(_keyboardOptions.KeyActions.First(x => x.KeyName == key));
            }

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
                switch (keyAction.KeyboardAction)
                {
                    case KeyboardActions.ON_PRESS:
                        if (!PreviousKeyboardState.IsKeyDown(keyAction.KeyName))
                        {
                            _logger.LogInformation($"{keyAction.KeyName}");
                            tasks.Add(_eventManager.PublishEvent(
                                new KeyboardEvt
                                {
                                    KeyAction = keyAction
                                }
                            ));
                        }
                        break;
                    //case KeyboardActions.ON_RELEASE:
                    //    if (PreviousKeyboardState.IsKeyDown(keyAction.KeyName) && )
                    //    {
                    //        tasks.Add(_eventManager.PublishEvent(
                    //            new KeyboardEvt
                    //            {
                    //                KeyAction = keyAction
                    //            }
                    //        ));
                    //    }
                    //  break;
                    case KeyboardActions.ON_HOLD:
                        if (PreviousKeyboardState.IsKeyDown(keyAction.KeyName))
                        {
                            _logger.LogInformation($"{keyAction.KeyName}");
                            tasks.Add(_eventManager.PublishEvent(
                                new KeyboardEvt
                                {
                                    KeyAction = keyAction
                                }
                            ));
                        }
                        break;
                    default:
                        _logger.LogWarning("Action not supported: {KeyboardAction}", keyAction.KeyboardAction);
                        break;
                }
            }

            return tasks;
        }
    }
}
