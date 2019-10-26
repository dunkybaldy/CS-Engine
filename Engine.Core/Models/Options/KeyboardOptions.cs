using Engine.Core.Models.Enums;

using System.Collections.Generic;

namespace Engine.Core.Models.Options
{
    public class KeyboardOptions
    {
        public List<KeyBinding> InGameKeyBindings { get; set; } = new List<KeyBinding>();
        public List<KeyBinding> InMenuKeyBindings { get; set; } = new List<KeyBinding>();

    }
}