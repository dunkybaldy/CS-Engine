using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Core.Models.Enums;
using Engine.Core.Models.Options;
using Engine.Core.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Xna.Framework.Input;

namespace ExampleGame.Validation
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

            foreach (var binding in _options.InGameKeyBindings)
            {
                if (!Enum.IsDefined(typeof(Keys), binding.Key))
                    nameErrors.Add($"'{binding.Key}' is not a valid key.");
                if (!Enum.IsDefined(typeof(KeyBindingActions), binding.KeyBindingAction))
                    actionErrors.Add($"'{binding.KeyBindingAction}' is not a valid action.");
            }

            var errors = new List<string>(nameErrors);
            errors.AddRange(actionErrors);

            if (nameErrors.Any())
                errors.Add(GetEnumNames(typeof(Keys)));

            if (actionErrors.Any())
                errors.Add(GetEnumNames(typeof(KeyBindingActions)));

            if (errors.Any())
            {
                validationResult.Validated = false;
                validationResult.ErrorMessage = string.Join(" ...\r", errors);
            }
            else
                _logger.LogInformation("KeyBinding Validation succeeded");

            return Task.FromResult(validationResult);
        }

        private string GetEnumNames(Type enumType)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Valid Keys: ");
            foreach (string value in Enum.GetNames(enumType))
            {
                stringBuilder.Append($"{value},\r");
            }
            return stringBuilder.ToString();
        }
    }
}
