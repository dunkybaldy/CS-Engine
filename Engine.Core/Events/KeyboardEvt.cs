using Engine.Core.Models.Options;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events
{
    public class KeyboardEvt : EngineEvt
    {
        public KeyAction KeyAction { get; set; }

        public KeyboardEvt()
        {
            EventType = EventType.KEYBOARD;
        }
    }
}
