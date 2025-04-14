using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Core.Models;
using SystemMonitor.RemoteLoggerPlugin.Configurations;
using SystemMonitor.RemoteLoggerPlugin.Services.ApiClient;

namespace SystemMonitor.RemoteLoggerPlugin.Services
{
    public class RemoteLoggerService
    {
        IApiClient apiClient;

        public RemoteLoggerService(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <summary>
        /// Logs data to a remote server throught api client
        /// </summary>
        /// <param name="monitorData">Monitored Data Object</param>
        /// <returns></returns>
        public async Task LogToRemoteServer( MonitorDataDto monitorData, AppSettings appSettings)
        {

            await apiClient.PostMonitoredDataAsync(appSettings, monitorData);
        }
    }
}
