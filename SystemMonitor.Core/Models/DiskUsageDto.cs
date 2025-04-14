namespace SystemMonitor.Core.Models;

/// <summary>
/// Data Object for disk UUsage
/// </summary>
public class DiskUsageDto
{
    public required string DriveName { get; set; }
    public long DiskUsed { get; set; }
    public long DiskTotal { get; set; }
}