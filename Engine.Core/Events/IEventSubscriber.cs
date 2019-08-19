using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events
{
    public interface IEventSubscriber
    {
        Task HandleEvent(MyEvent @event);
    }
}
