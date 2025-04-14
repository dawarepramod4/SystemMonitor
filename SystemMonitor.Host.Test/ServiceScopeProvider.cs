using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SystemMonitor.Models;
using SystemMonitor.Services.CpuUsageMonitor;
using SystemMonitor.Services.OperatingSystemDetectionService;
using SystemMonitor.Services.PluginLoaderService;

namespace SystemMonitor.Host.Test
{
    public class ServiceScopeProvider
    {
        public IServiceProvider ServiceProvider { get; }

        public ServiceScopeProvider()
        {
            ServiceProvider = new ServiceCollection()
         .AddSingleton<PluginLoaderService>()
         .AddSingleton<OperatingSystemDetectionService>()
         .AddSingleton<CpuUsageMonitorResolver>()
         .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.Windows)
         .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.MacOsx)
         .AddKeyedSingleton<ICpuUsageMonitor, WindowsCpuUsageMonitor>(OperatingSystemType.Linux)
         .BuildServiceProvider();
        }
    }
}
