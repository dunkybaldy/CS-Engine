using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface IDeviceManager
    {
        Task PollKeyboard();
        Task PollMouse();
    }
}
