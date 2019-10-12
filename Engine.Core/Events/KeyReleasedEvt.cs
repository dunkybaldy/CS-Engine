namespace Engine.Core.Events
{
    public class KeyReleasedEvt : KeyEvt
    {
        public KeyReleasedEvt()
        {
            EventType = EventType.KEY_RELEASED;
        }
    }
}
