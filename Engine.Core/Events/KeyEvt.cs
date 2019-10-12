using Engine.Core.Models.Options;

namespace Engine.Core.Events
{
    public abstract class KeyEvt : EngineEvt
    {
        public KeyBinding KeyBinding { get; set; }

        protected KeyEvt()
        {
            EventCategory = EventCategory.KEYBOARD;
        }
    }
}
