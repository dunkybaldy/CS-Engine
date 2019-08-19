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
        Task PublishEvent<T>(T @event) where T : MyEvent;
        Task ProcessEvent();
        Task SubscribeToEvent(EventType eventType, IEventSubscriber subscriber);
        Task UnsubscribeFromEvent(EventType eventType, IEventSubscriber subscriber);
    }
}
