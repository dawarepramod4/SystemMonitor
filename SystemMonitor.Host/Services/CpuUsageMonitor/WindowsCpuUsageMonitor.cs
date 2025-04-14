using SystemMonitor.Models;
using System.Diagnostics;
namespace SystemMonitor.Services.CpuUsageMonitor;

/// <summary>
/// Cpu Usae Monitor for the windows OS
/// </summary>
public class WindowsCpuUsageMonitor : ICpuUsageMonitor
{
    public async Task<CpuUsageDto> GetCpuUsageAsync(int intervalMs)
    {
        var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        //start monitor
        cpuCounter.NextValue();

        await Task.Delay(intervalMs);

        double cpuUsage = cpuCounter.NextValue();

        return new CpuUsageDto()
        {
            UsagePercent = cpuUsage,
        };
    }
}