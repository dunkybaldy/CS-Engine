using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Core.Models.Options
{
    public class KeyBinding
    {
        public Keys KeyName { get; set; }
        public KeyboardActions KeyboardAction { get; set; }
    }
}