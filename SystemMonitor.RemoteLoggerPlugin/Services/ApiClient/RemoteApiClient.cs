using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SystemMonitor.Core.Models;
using SystemMonitor.RemoteLoggerPlugin.Configurations;

namespace SystemMonitor.RemoteLoggerPlugin.Services.ApiClient
{
    /// <summary>
    /// Api Client for remote calls
    /// </summary>
    public class RemoteApiClient : IApiClient
    {
        private readonly HttpClient _httpClient = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task PostMonitoredDataAsync(AppSettings appSettings, MonitorDataDto data)
        {

            var url = new Uri(new Uri(appSettings.BaseUrl), appSettings.EndpointPath);


            //convert to MB
            double RamUsed = data.MemoryUsage.MemoryUsed / (1024.0 * 1024.0);
            //we need to aggregate all the disks usages in MB to send to remote server
            double DiskUsage = data.DiskUsage.Select(diskUsage => diskUsage.DiskUsed).Sum() / (1024.0 * 1024.0);

            var payload = new
            {
                cpu = data.CpuUsagePercentage,
                ram_used = RamUsed.ToString("F3"),
                disk_used = DiskUsage.ToString("F3")
            };

            try
            {

                //apis would need application/json format with utf encoding
                string json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsJsonAsync(url, content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine($"{DateTime.Now:s}: Logged To Server: {json}");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while sending data to server {e.Message}");
            }
        }
    }
}
