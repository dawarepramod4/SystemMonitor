using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemMonitor.Core;
using SystemMonitor.Core.Models;
using SystemMonitor.RemoteLoggerPlugin.Configurations;
using SystemMonitor.RemoteLoggerPlugin.Services;
using SystemMonitor.RemoteLoggerPlugin.Services.ApiClient;

namespace SystemMonitor.RemoteLoggerPlugin
{
    /// <summary>
    /// Log Data to Remote API
    /// </summary>
    public class RemoteLoggerPlugin : IMonitorPlugin, IConfigurablePlugin
    {
        public string Name { get; set; } = "logtoapi";
        public string Description { get; set; } = "Logs data to a remote api";

        public AppSettings AppSettings { get; set; }

        /// <summary>
        /// Configures the appsettings for the plugin.
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Configure(IConfigurationSection config)
        {

            AppSettings = config.Get<AppSettings>() ??
                throw new InvalidOperationException("Cannot get Configurations for the remote logging");

        }

        public async Task InvokeAsync(Channel<MonitorDataDto> monitoredDataChannel)
        {
            if (AppSettings == null)
            {
                //appsettings is required for this plugin
                throw new NullReferenceException(nameof(AppSettings));
            }

            //not using DI here
            IApiClient apiClient = new RemoteApiClient();
            RemoteLoggerService loggerService = new(apiClient);

            //log the data to file
            await foreach (var monitoredData in monitoredDataChannel.Reader.ReadAllAsync())
            {
                try
                {
                    await loggerService.LogToRemoteServer(monitoredData, AppSettings);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending: {ex.Message}");
                    // Optionally re-enqueue or log
                }

            }
        }
    }
}
