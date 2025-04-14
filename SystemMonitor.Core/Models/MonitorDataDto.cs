namespace SystemMonitor.Core.Models;

/// <summary>
/// Data Transfer Object for Monitored Data
/// </summary>
public class MonitorDataDto
{
    public DateTime DateTime { get; set; } = DateTime.Now;
    public double CpuUsagePercentage { get; set; }
    public required MemoryUsageDto MemoryUsage { get; set; }
    public List<DiskUsageDto> DiskUsage { get; set; } = [];
}