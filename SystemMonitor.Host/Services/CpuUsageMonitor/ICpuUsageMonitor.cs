using SystemMonitor.Models;

namespace SystemMonitor.Services.CpuUsageMonitor;

public interface ICpuUsageMonitor
{
    /// <summary>
    /// Gets the CPU usage of the OS.
    /// </summary>
    /// <returns></returns>
    public Task<CpuUsageDto> GetCpuUsageAsync(int intervalMs);
    
}