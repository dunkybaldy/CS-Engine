using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Input;

namespace Engine.Core.Validation
{
    public class KeyBindingValidator : IValidator
    {
        private readonly KeyboardOptions _options;
        private readonly ILogger<KeyBindingValidator> _logger;

        public KeyBindingValidator(IOptions<KeyboardOptions> options, ILogger<KeyBindingValidator> logger)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<ValidationResult> Validate()
        {
            _logger.LogInformation("Beginning KeyBinding Validation");

            var validationResult = new ValidationResult();

            var nameErrors = new List<string>();
            var actionErrors = new List<string>();

            foreach (var binding in _options.KeyBindings)
            {
                if (!Enum.IsDefined(typeof(Keys), binding.KeyName))
                    nameErrors.Add($"'{binding.KeyName}' is not a valid key.");
                if (!Enum.IsDefined(typeof(KeyboardActions), binding.KeyboardAction))
                    actionErrors.Add($"'{binding.KeyboardAction}' is not a valid action.");
            }

            var errors = new List<string>(nameErrors);
            errors.AddRange(actionErrors);

            if (nameErrors.Any())
                errors.Add(GetKeysEnumNames());

            if (actionErrors.Any())
                errors.Add(GetKeyboardActionsEnumNames());

            if (errors.Any())
            {
                validationResult.Validated = false;
                validationResult.ErrorMessage = string.Join(" ...\r", errors);
            }
            else
                _logger.LogInformation("KeyBinding Validation succeeded");

            return Task.FromResult(validationResult);
        }

        private string GetKeysEnumNames()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Valid Keys: ");
            foreach (string value in Enum.GetNames(typeof(Keys)))
            {
                stringBuilder.Append($"{value},\r");
            }
            return stringBuilder.ToString();
        }

        private string GetKeyboardActionsEnumNames()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Valid KeyboardActions: ");
            foreach (string value in Enum.GetNames(typeof(KeyboardActions)))
            {
                stringBuilder.Append($"{value}");
            }
            return stringBuilder.ToString();
        }
    }
}
