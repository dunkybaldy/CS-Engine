using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Validation
{
    public class ValidationResult
    {
        public bool Validated { get; set; } = true;
        public string ErrorMessage { get; set; }
    }
}
