using Microsoft.Extensions.DependencyInjection;

namespace SystemMonitor.Services.CpuUsageMonitor;

/// <summary>
/// Resolved the CPU usage monitor service to be used in the App.
/// </summary> 
/// <param name="serviceProvider"></param>
public class CpuUsageMonitorResolver(IServiceProvider serviceProvider)
{

    /// <summary>
    /// gets the requirements service for the current OS Platform
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ICpuUsageMonitor ResolveCpuUsageMonitor()
    {
        //needs current Os Type
        var currentOsType = OperatingSystemDetectionService.OperatingSystemDetectionService.GetCurrentPlatform();

        //get the service based on the OS Type
        var service =serviceProvider.GetKeyedService<ICpuUsageMonitor>(currentOsType);
        if (service == null)
        {
            throw new NotImplementedException($"Cannot resolve cpu usage monitor for {currentOsType}");
        }
        
        return service;
    }
}