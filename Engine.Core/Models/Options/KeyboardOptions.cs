using Engine.Core.Models.Enums;

using System.Collections.Generic;

namespace Engine.Core.Models.Options
{
    public class KeyboardOptions
    {
        public Dictionary<string, KeyboardActions> BoundKeyActions { get; set; }
    }
}