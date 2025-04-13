using Microsoft.Extensions.DependencyInjection;

namespace SystemMonitor.Services.CpuUsageMonitor;

public class CpuUsageMonitorResolver(IServiceProvider serviceProvider)
{

    //gets the requirements service for the current OS Platform
    public ICpuUsageMonitor ResolveCpuUsageMonitor()
    {
        var currentOsType = OperatingSystemDetectionService.OperatingSystemDetectionService.GetCurrentPlatform();
        var service =serviceProvider.GetKeyedService<ICpuUsageMonitor>(currentOsType);
        if (service == null)
        {
            throw new InvalidOperationException($"Cannot resolve cpu usage monitor for {currentOsType}");
        }
        
        return service;
    }
}