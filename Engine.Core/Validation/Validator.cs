using System.Threading.Tasks;

namespace Engine.Core.Validation
{
    public interface IValidator
    {
        Task<ValidationResult> Validate();
    }
}
