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

        public ConcurrentDictionary<EventCategory, ConcurrentBag<IEventSubscriber>> Subscribers { get; set; }

        public EventManager(ILogger<EventManager> logger, DiagnosticsController diagnosticsController)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticsController = diagnosticsController ?? throw new ArgumentNullException(nameof(diagnosticsController));

            EventQueue = new ConcurrentQueue<EngineEvt>();
            Subscribers = new ConcurrentDictionary<EventCategory, ConcurrentBag<IEventSubscriber>>();
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
                if (Subscribers.ContainsKey(engineEvt.EventCategory))
                    Subscribers[engineEvt.EventCategory].ToList().ForEach(x => tasks.Add(x.HandleEvent(engineEvt)));
            }

            await _diagnosticsController.DiagnoseTask(Task.WhenAll(tasks), "HandleEvent");
        }

        public Task SubscribeToEvent(EventCategory eventCategory, IEventSubscriber subscriber)
        {
            if (!Subscribers.ContainsKey(eventCategory))
                Subscribers.TryAdd(eventCategory, new ConcurrentBag<IEventSubscriber>());
            Subscribers[eventCategory].Add(subscriber);
            return Task.CompletedTask;
        }

        public async Task SubscribeToEvents(IEnumerable<EventCategory> eventCategorys, IEventSubscriber subscriber)
        {
            foreach (var evt in eventCategorys)            
                await SubscribeToEvent(evt, subscriber);
        }

        public Task UnsubscribeFromEvent(EventCategory eventCategory, IEventSubscriber subscriber)
        {
            Subscribers[eventCategory] = new ConcurrentBag<IEventSubscriber>(Subscribers[eventCategory].Except(new[] { subscriber }));
            return Task.CompletedTask;
        }
    }
}
