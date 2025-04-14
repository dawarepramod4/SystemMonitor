using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Core.Models;
using SystemMonitor.RemoteLoggerPlugin.Configurations;

namespace SystemMonitor.RemoteLoggerPlugin.Services.ApiClient
{
    /// <summary>
    /// Api Client for Talking to the client
    /// </summary>
    public interface IApiClient
    {

        /// <summary>
        /// Send monitored data to the Server
        /// </summary>
        /// <param name="appSettings">appsettings for base url and path</param>
        /// <param name="data">data to send</param>
        /// <param name="token">cancellation token</param>
        /// <returns></returns>
        public Task PostMonitoredDataAsync(AppSettings appSettings, MonitorDataDto data);
    }
}
