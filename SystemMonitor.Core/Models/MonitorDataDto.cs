namespace SystemMonitor.Core.Models;

/// <summary>
/// Data Transfer Object for Monitored Data
/// </summary>
public class MonitorDataDto
{ 
   public double CpuUsagePercentage { get; set; }
   public MemoryUsageDto? MemoryUsage { get; set; }
   public List<DiskUsageDto> DiskUsage { get; set; } = [];
}