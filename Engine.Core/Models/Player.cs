using Engine.Core.Events;
using Engine.Core.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Core.Models
{
    public abstract class Player : Entity3D, IEventSubscriber
    {
        protected Player()
        {
            ActionOnEntity = EntityActions.UPDATEDRAW;

            // Probably should be determined by game, if game doesn't provide then use defaults
            SubscribedToEvents = new List<EventCategory>
            {
                EventCategory.KEYBOARD,
                EventCategory.MOUSE
            };
        }

        //protected abstract Task HandleCollisionEvent(EngineEvt @event);
        //protected abstract Task HandleControllerEvent(EngineEvt @event);
        protected abstract Task HandleKeyboardEvent(KeyEvt @event);
        //protected abstract Task HandleMouseEvent(EngineEvt @event);

        public virtual async Task HandleEvent(EngineEvt @event)
        {
            switch(@event.EventCategory)
            {
                //case EventType.COLLISION:
                    //await HandleCollisionEvent((CollisionEvt)@event);
                    //break;
                //case EventType.CONTROLLER:
                    //await HandleCollisionEvent((CollisionEvt)@event);
                    //break;
                case EventCategory.KEYBOARD:
                    if (@event.EventType == EventType.KEY_PRESSED)
                        await HandleKeyboardEvent((KeyPressEvt)@event);
                    else if (@event.EventType == EventType.KEY_RELEASED)
                        await HandleKeyboardEvent((KeyReleasedEvt)@event);
                    break;
                //case EventCategory.MOUSE:
                //    await HandleMouseEvent((MouseEvt)@event);
                //    break;
                default:
                break;
            }
        }
    }
}
