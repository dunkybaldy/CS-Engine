using Engine.Core.Models.Enums;

using System.Collections.Generic;

namespace Engine.Core.Models.Options
{
    public class KeyboardOptions
    {
        public List<KeyAction> KeyActions { get; set; } = new List<KeyAction>();
    }
}