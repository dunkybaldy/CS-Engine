using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Core.Models.Options
{
    public class KeyBinding
    {
        public Keys Key { get; set; }
        public KeyBindingActions KeyBindingAction { get; set; }
    }
}