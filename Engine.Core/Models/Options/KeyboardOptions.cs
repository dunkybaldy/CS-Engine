using Engine.Core.Models.Enums;

using System.Collections.Generic;

namespace Engine.Core.Models.Options
{
    public class KeyboardOptions
    {
        public List<KeyBinding> KeyBindings { get; set; } = new List<KeyBinding>();
    }
}