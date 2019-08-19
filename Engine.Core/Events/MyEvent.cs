using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Events
{
    public abstract class MyEvent
    {
        public EventType EventType { get; set; }
        public object Data { get; set; }
    }
}
