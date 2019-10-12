namespace Engine.Core.Events
{
    public class KeyPressEvt : KeyEvt
    {
        public KeyPressEvt()
        {
            EventType = EventType.KEY_PRESSED;
        }
    }
}
