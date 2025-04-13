using SystemMonitor.Models;
using System.Diagnostics;
namespace SystemMonitor.Services.CpuUsageMonitor;

public class WindowsCpuUsageMonitor : ICpuUsageMonitor
{
    public Task<CpuUsageDto> GetCpuUsageAsync(int intervalMs)
    {
        throw new NotImplementedException();
    }
}