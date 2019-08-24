﻿using Engine.Core.Diagnostics;
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

        public ConcurrentQueue<MyEvent> EventQueue { get; private set; }
        private bool Running { get; set; } = true;

        public ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>> Subscribers { get; set; }

        public EventManager(ILogger<EventManager> logger, DiagnosticsController diagnosticsController)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _diagnosticsController = diagnosticsController ?? throw new ArgumentNullException(nameof(diagnosticsController));

            EventQueue = new ConcurrentQueue<MyEvent>();
            Subscribers = new ConcurrentDictionary<EventType, ConcurrentBag<IEventSubscriber>>();
        }

        public async void Begin()
        {
            await ProcessEvents();
        }

        public Task PublishEvent<T>(T @event) where T : MyEvent
        {
            EventQueue.Enqueue(@event);
            _logger.LogInformation("EventQueue now has {MessageCount} messages.", EventQueue.Count);
            return Task.CompletedTask;
        }

        private async Task ProcessEvents()
        {
            while (Running)
            {
                var tasks = new List<Task>();

                if (EventQueue.TryDequeue(out MyEvent myEvent))
                {
                    _logger.LogInformation("Found event '{EventType}'. Adding publish task for subscribers.", myEvent.EventType);
                    Subscribers[myEvent.EventType].ToList().ForEach(x => tasks.Add(x.HandleEvent(myEvent)));
                }

                await _diagnosticsController.DiagnoseTask(Task.WhenAll(tasks), "HandleEvent");
            }
        }

        public bool OnOffSwitch(bool? @switch)
        {
            if (@switch.HasValue)
                Running = @switch.Value;
            else
                Running = !Running;

            return Running;
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