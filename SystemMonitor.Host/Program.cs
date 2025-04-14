using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using SystemMonitor.Core;
using SystemMonitor.Core.Models;
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
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // ensures correct runtime path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Setup DI container
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration) // inject configuration!
            .AddSingleton<PluginLoaderService>()
            .AddSingleton<OperatingSystemDetectionService>()
            .AddSingleton<CpuUsageMonitorResolver>()
            .AddKeyedSingleton<ICpuUsageMonitor, WindowsCpuUsageMonitor>(OperatingSystemType.Windows)
            .AddKeyedSingleton<ICpuUsageMonitor, LinuxCpuUsageMonitor>(OperatingSystemType.Linux)
            .AddSingleton<SystemPerformanceMonitorService>()
            .AddTransient<SystemPerformanceDataProducer>()
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        // Get CLI arguments
        var pluginToRun = args[0];
        var intervalMs = int.Parse(args[1]);

        // Load the plugin
        var pluginLoader = scope.ServiceProvider.GetRequiredService<PluginLoaderService>();
        var plugins = pluginLoader.GetAllPluginsFromAssembly();
        var currentPlugin = plugins.FirstOrDefault(plugin => plugin.Name.Equals(pluginToRun));

        if (currentPlugin == null)
        {
            Console.WriteLine("Could not load plugin: " + pluginToRun);
            return;
        }

        // OPTIONAL: Pass config to plugin if needed
        if (currentPlugin is IConfigurablePlugin configurable)
        {
            var pluginSection = configuration.GetSection($"PluginSettings:{currentPlugin.Name}");
            configurable.Configure(pluginSection);
        }

        #region Producer-Consumer

        var cts = new CancellationTokenSource();
        Channel<MonitorDataDto> monitorDataChannel = Channel.CreateUnbounded<MonitorDataDto>();

        var performanceDataProducerService = scope.ServiceProvider.GetRequiredService<SystemPerformanceDataProducer>();
        var producer = performanceDataProducerService.ProduceSystemMonitoredData(monitorDataChannel, intervalMs, cts.Token);
        var consumer = currentPlugin.InvokeAsync(monitorDataChannel);

        await Task.WhenAll(producer, consumer);

        #endregion
    }
}
