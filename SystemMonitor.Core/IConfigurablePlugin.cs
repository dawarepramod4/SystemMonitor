using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemMonitor.Core
{
    public interface IConfigurablePlugin
    {
        /// <summary>
        /// Configuration needed for the plugin.
        /// </summary>
        /// <param name="config"></param>
        void Configure(IConfigurationSection config);
    }
}
