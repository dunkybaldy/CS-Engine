using Engine.Core.Models.Enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Core.Models.Options
{
    public class KeyAction
    {
        public string ActionName { get; set; }
        public Keys KeyName { get; set; }
        public KeyboardActions KeyboardAction { get; set; }
    }
}