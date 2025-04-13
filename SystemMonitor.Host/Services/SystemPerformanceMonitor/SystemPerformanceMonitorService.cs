using System.Diagnostics;
using SystemMonitor.Core.Models;
using SystemMonitor.Services.CpuUsageMonitor;

namespace SystemMonitor.Services.SystemPerformanceMonitor;
/// <summary>
/// Service to monitor performance of the current operating system
/// </summary>
public class SystemPerformanceMonitorService(ICpuUsageMonitor cpuUsageMonitor)
{
    /// <summary>
    /// Retrieves the performance stats at the moment.
    /// </summary>
    /// <returns>Monitored Data from the system</returns>
    public async Task<MonitorDataDto> GetPerformanceStats(int intervalMs)
    {
        var cpuPercent = await cpuUsageMonitor.GetCpuUsageAsync(intervalMs);

        return new MonitorDataDto()
        {
            CpuUsagePercentage = cpuPercent.UsagePercent,
            MemoryUsage = GetMemoryUsage(),
            DiskUsage = GetDiskUsage()
        };

    }


    /// <summary>
    /// Gets Memory usage and 
    /// </summary>
    /// <returns></returns>
    private MemoryUsageDto GetMemoryUsage()
    {
        var info = Process.GetCurrentProcess();
        var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
        var usedMemory = info.WorkingSet64;
        return new MemoryUsageDto()
        {
            MemoryTotal = totalMemory,
            MemoryUsed = usedMemory,
        };
    }

    /// <summary>
    /// Gets the Disk Usage for all the disks in the system.
    /// </summary>
    /// <returns></returns>
    private List<DiskUsageDto> GetDiskUsage()
    {
        List<DiskUsageDto> diskUsage = [];
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady)
            {
                var name = drive.Name;
                var total = drive.TotalSize;
                var free = drive.AvailableFreeSpace;
                var used = total - free;

                diskUsage.Add(new DiskUsageDto
                {
                    DriveName = name,
                    DiskUsed = used,
                    DiskTotal = total
                });
            }
        }

        return diskUsage;
    }
}