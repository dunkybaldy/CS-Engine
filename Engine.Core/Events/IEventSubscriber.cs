using System.Threading.Tasks;

namespace Engine.Core.Events
{
    public interface IEventSubscriber
    {
        Task HandleEvent(EngineEvt @event);
    }
}
