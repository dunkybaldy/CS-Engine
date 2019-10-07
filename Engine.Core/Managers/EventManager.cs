using Engine.Core.Diagnostics;
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
        private readonly DiagnosticsController _diagnosticsController;

        public ConcurrentQueue<EngineEvt> EventQueue { get; private set; }

        public ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>> Subscribers { get; set; }

        public EventManager(ILogger<EventManager> logger, DiagnosticsController diagnosticsController)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticsController = diagnosticsController ?? throw new ArgumentNullException(nameof(diagnosticsController));

            EventQueue = new ConcurrentQueue<EngineEvt>();
            Subscribers = new ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>>();
        }

        public async void Run()
        {
            while (EngineStatics.Running)
            {
                await ProcessEvents();
            }
        }

        public Task PublishEvent<T>(T @event) where T : EngineEvt
        {
            EventQueue.Enqueue(@event);
            _logger.LogInformation("EventQueue now has {MessageCount} messages.", EventQueue.Count);
            return Task.CompletedTask;
        }

        private async Task ProcessEvents()
        {
            var tasks = new List<Task>();

            if (EventQueue.TryDequeue(out EngineEvt engineEvt))
            {
                _logger.LogDebug("Found event '{EventType}'. Adding publish task for subscribers.", engineEvt.EventType);
                if (Subscribers.ContainsKey(engineEvt.EventType))
                    Subscribers[engineEvt.EventType].ToList().ForEach(x => tasks.Add(x.HandleEvent(engineEvt)));
            }

            await _diagnosticsController.DiagnoseTask(Task.WhenAll(tasks), "HandleEvent");
        }

        public Task SubscribeToEvent(EventType eventType, IEventSubscriber subscriber)
        {
            if (!Subscribers.ContainsKey(eventType))
                Subscribers.TryAdd(eventType, new ConcurrentBag<IEventSubscriber>());
            Subscribers[eventType].Add(subscriber);
            return Task.CompletedTask;
        }

        public Task SubscribeToEvents(IEnumerable<EventType> eventTypes, IEventSubscriber subscriber)
        {
            foreach (var evt in eventTypes)
            {
                if (!Subscribers.ContainsKey(evt))
                    Subscribers.TryAdd(evt, new ConcurrentBag<IEventSubscriber>());
                Subscribers[evt].Add(subscriber);
            }
            return Task.CompletedTask;
        }

        public Task UnsubscribeFromEvent(EventType eventType, IEventSubscriber subscriber)
        {
            Subscribers[eventType] = new ConcurrentBag<IEventSubscriber>(Subscribers[eventType].Except(new[] { subscriber }));
            return Task.CompletedTask;
        }
    }
}
