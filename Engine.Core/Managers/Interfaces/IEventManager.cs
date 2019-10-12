using Engine.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface IEventManager
    {
        void Run();
        Task PublishEvent<T>(T @event) where T : EngineEvt;
        Task SubscribeToEvent(EventCategory eventCategory, IEventSubscriber subscriber);
        Task SubscribeToEvents(IEnumerable<EventCategory> eventCategorys, IEventSubscriber subscriber);
        Task UnsubscribeFromEvent(EventCategory eventCategory, IEventSubscriber subscriber);
    }
}
