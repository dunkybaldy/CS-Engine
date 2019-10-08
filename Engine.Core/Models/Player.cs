﻿using Engine.Core.Events;
using Engine.Core.Managers.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public abstract class Player : Entity3D, IEventSubscriber
    {
        protected Player()
        {
            // Probably should be determined by game, if game doesn't provide then use defaults
            SubscribedToEvents = new List<EventType>
            {
                EventType.COLLISION,
                EventType.CONTROLLER,
                EventType.KEYBOARD,
                EventType.MOUSE
            };
        }

        protected abstract Task HandleCollisionEvent(KeyboardEvt @event);
        protected abstract Task HandleControllerEvent(KeyboardEvt @event);
        protected abstract Task HandleKeyboardEvent(KeyboardEvt @event);
        protected abstract Task HandleMouseEvent(KeyboardEvt @event);

        public virtual async Task HandleEvent(EngineEvt @event)
        {
            switch(@event.EventType)
            {
                case EventType.COLLISION:
                    //await HandleCollisionEvent((CollisionEvt)@event);
                    break;
                case EventType.CONTROLLER:
                    //await HandleCollisionEvent((CollisionEvt)@event);
                    break;
                case EventType.KEYBOARD:
                    await HandleKeyboardEvent((KeyboardEvt)@event);
                    break;
                case EventType.MOUSE:
                    //await HandleCollisionEvent((CollisionEvt)@event);
                    break;
                default:
                break;
            }
        }
    }
}
