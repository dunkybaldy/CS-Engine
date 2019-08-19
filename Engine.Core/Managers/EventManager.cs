using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class EventManager : IEventManager
    {
        private readonly ILogger<EventManager> _logger;

        public ConcurrentQueue<MyEvent> EventQueue { get; private set; }

        public ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>> Subscribers { get; set; }

        public EventManager(ILogger<EventManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            EventQueue = new ConcurrentQueue<MyEvent>();
            Subscribers = new ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>>();
        }

        public Task PublishEvent<T>(T @event) where T : MyEvent
        {
            EventQueue.Enqueue(@event);
            _logger.LogInformation("EventQueue now has {MessageCount} messages.", EventQueue.Count);
            return Task.CompletedTask;
        }

        public async Task ProcessEvent()
        {
            _logger.LogWarning("Process event");
            var tasks = new List<Task>();

            if (EventQueue.TryDequeue(out MyEvent myEvent))
                Subscribers[myEvent.EventType].ToList().ForEach(x => tasks.Add(x.HandleEvent(myEvent)));

            await Task.WhenAll(tasks);
        }

        public Task SubscribeToEvent(EventType eventType, IEventSubscriber subscriber)
        {
            Subscribers[eventType].Add(subscriber);
            return Task.CompletedTask;
        }

        public Task UnsubscribeFromEvent(EventType eventType, IEventSubscriber subscriber)
        {
            Subscribers[eventType] = new ConcurrentBag<IEventSubscriber>(Subscribers[eventType].Except(new[] { subscriber }));
            return Task.CompletedTask;
        }
    }
}
