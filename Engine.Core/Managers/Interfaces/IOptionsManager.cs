using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engine.Core.Managers.Interfaces
{
    public interface IOptionsManager
    {
        Task LoadOptions<TOptions>(IConfigurationSection section);
        Task LoadOptions(IConfigurationRoot configuration);
        Task UpdateOption(string valueName, object value);
        Task UpdateOptions(string jsonSection, Dictionary<string, object> keyValues);
        Task UpdateSection(string jsonSection);
    }
}
