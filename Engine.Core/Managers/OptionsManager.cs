using Engine.Core.Managers.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core.Managers
{
    public class OptionsManager : IOptionsManager
    {
        public IConfigurationRoot _config { get; private set; }

        public OptionsManager(IConfigurationRoot configuration)
        {
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task LoadOptions<TOptions>(IConfigurationSection section)
        {
            throw new NotImplementedException();
        }

        public Task LoadOptions(IConfigurationRoot configuration)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOption(string valueName, object value)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOptions(string jsonSection, Dictionary<string, object> keyValues)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSection(string jsonSection)
        {
            throw new NotImplementedException();
        }
    }
}
