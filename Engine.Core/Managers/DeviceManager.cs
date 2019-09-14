using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;
using Engine.Core.Models.Options;

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

        public List<Keys> BoundKeys { get; set; }

        private KeyboardState KeyboardState { get; set; }
        private MouseState MouseState { get; set; }

        private KeyboardState PreviousKeyboardState { get; set; }
        private MouseState PreviousMouseState { get; set; }


        public DeviceManager(IEventManager eventManager, IOptions<KeyboardOptions> keyboardOptions)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _keyboardOptions = keyboardOptions.Value ?? throw new ArgumentNullException(nameof(keyboardOptions));
            BoundKeys = new List<Keys>();
        }

        public async Task PollState()
        {
            KeyboardState = Keyboard.GetState();

            var keysDown = KeyboardState.GetPressedKeys().ToList();
            var keysICareAbout = keysDown.Where(x => _keyboardOptions.BoundKeyActions.Keys.Contains(x.ToString()));

            if (keysICareAbout.Count() != 0)
            {
                var eventTasks = CreateKeyboardTasks(keysICareAbout);

                await Task.WhenAll(eventTasks);
            }
            PreviousKeyboardState = KeyboardState;
        }

        private List<Task> CreateKeyboardTasks(IEnumerable<Keys> keys)
        { 
            var tasks = new List<Task>();

            foreach (var key in keys)
            {
                tasks.Add(_eventManager.PublishEvent(new KeyboardEvt
                {
                    Key = key
                }));
            }

            return tasks;
        }
    }
}
