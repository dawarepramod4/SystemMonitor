// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using SystemMonitor.Models;
using SystemMonitor.Services.CpuUsageMonitor;
using SystemMonitor.Services.OperatingSystemDetectionService;
using SystemMonitor.Services.PluginLoaderService;
using SystemMonitor.Services.SystemPerformanceMonitor;

namespace SystemMonitor;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<PluginLoaderService>()
            .AddSingleton<OperatingSystemDetectionService>()
            .AddSingleton<CpuUsageMonitorResolver>()
            .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.Windows)
            .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.MacOsx)
            .AddKeyedSingleton<ICpuUsageMonitor, WindowsCpuUsageMonitor>(OperatingSystemType.Linux)
            .BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        //get the arguments
        var pluginToRun = args[0];
        var intervalMs = int.Parse(args[1]);

        //we need to find the plugin with name sent from cli
        var pluginLoader = scope.ServiceProvider.GetRequiredService<PluginLoaderService>();
        
        var plugins = pluginLoader.GetAllPluginsFromAssembly();
        var currentPlugin = plugins.FirstOrDefault(plugin => plugin.Name.Equals(pluginToRun));
        
        if (currentPlugin == null)
        {
            Console.WriteLine("Could not load plugin: " + pluginToRun);
            return;
        }

        //running the plugin with interval of provided milliseconds
        var performanceMonitor = scope.ServiceProvider.GetRequiredService<SystemPerformanceMonitorService>();
        while (true)
        {
            var monitorDataDto = await performanceMonitor.GetPerformanceStats(intervalMs);
            await currentPlugin.InvokeAsync(monitorDataDto);
        }
    }
}