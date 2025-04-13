namespace SystemMonitor.Core.Models;

public class DiskUsageDto
{
    public required string DriveName { get; set; }
    public double DiskUsed { get; set; }
    public double DiskTotal { get; set; }
}