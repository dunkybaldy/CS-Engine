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

        // https://stackoverflow.com/questions/13208320/keyboard-button-event-efficiency-in-xna
        public async Task PollKeyboard()
        {
            var error = false;
            try
            {
                KeyboardState = Keyboard.GetState();
                var keysPressed = KeyboardState.GetPressedKeys().ToList();
                if (keysPressed.Any())
                {
                    var eventTasks = CreateKeyboardTasks(keysPressed);
                    await Task.WhenAll(eventTasks);
                }
                PreviousKeyboardState = KeyboardState;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PollKeyboard caught an error. ");
                error = true;
            }
            finally
            {
                //if (error)
                //    ReconnectDevices();
            }
        }

        private List<Task> CreateKeyboardTasks(List<Keys> keysDown)
        {
            var tasks = new List<Task>();

            foreach (var key in keysDown)
            {
                var currentKey = _keyboardOptions.InGameKeyBindings.FirstOrDefault(x => x.Key == key);
                if (currentKey == null)
                {
                    _logger.LogWarning($"{key} not bound to an action");
                    continue;
                }

                if (!PreviousKeyboardState.IsKeyDown(currentKey.Key) && !KeyboardState.IsKeyUp(currentKey.Key))
                {
                    _logger.LogInformation($"{currentKey.Key}");
                    tasks.Add(_eventManager.PublishEvent(
                        new KeyPressEvt
                        {
                            KeyBinding = currentKey
                        }
                    ));
                }
                else if (PreviousKeyboardState.IsKeyDown(currentKey.Key) && KeyboardState.IsKeyUp(currentKey.Key))
                {
                    tasks.Add(_eventManager.PublishEvent(
                        new KeyReleasedEvt
                        {
                            KeyBinding = currentKey
                        }
                    ));
                }
                else if (PreviousKeyboardState.IsKeyDown(currentKey.Key) && KeyboardState.IsKeyDown(currentKey.Key))
                {
                    _logger.LogInformation($"{currentKey.Key}");
                    tasks.Add(_eventManager.PublishEvent(
                        new KeyPressHeldEvt
                        {
                            KeyBinding = currentKey
                        }
                    ));
                }
                else
                {
                    _logger.LogWarning("Unhandled Key check..");
                }
            }
            return tasks;
        }

        public Task PollMouse()
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
