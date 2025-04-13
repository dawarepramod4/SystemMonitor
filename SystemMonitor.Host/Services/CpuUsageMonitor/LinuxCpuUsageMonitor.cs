using SystemMonitor.Models;

namespace SystemMonitor.Services.CpuUsageMonitor;

public class LinuxCpuUsageMonitor :ICpuUsageMonitor
{
    /// <summary>
    /// Get CPU usage for the provided time interval.
    /// </summary>
    /// <param name="intervalMs"></param>
    /// <returns></returns>
    public async Task<CpuUsageDto> GetCpuUsageAsync(int intervalMs)
    {
        double usage = await GetCpuUsagePercentage(intervalMs);
        return new CpuUsageDto()
        {
            UsagePercent = usage
        };
    }
    
    /// <summary>
    /// Calculates CPU Usage in percentages by getting the cpu stats for the provided interval.
    /// </summary>
    /// <param name="intervalMs"></param>
    /// <returns></returns>
    async Task<double> GetCpuUsagePercentage(int intervalMs)
    {
        //cpu stats start
        var cpu1 = ReadCpuStats();
        await Task.Delay(intervalMs);
        //cpu stats end
        var cpu2 = ReadCpuStats();

        //stats before monitor started
        var total1 = cpu1.total;
        var idle1 = cpu1.idle;

        //stats after monitos started
        var total2 = cpu2.total;
        var idle2 = cpu2.idle;

        var totalDiff = total2 - total1;
        var idleDiff = idle2 - idle1;

        //In case of interval is 0 the usage assumed to be near zero
        if (totalDiff == 0)
            return 0;

        //we need percentage
        return 100.0 * (totalDiff - idleDiff) / totalDiff;
    }
    
    /// <summary>
    /// Gets the total time and system idle time of the CPU by reading the "/proc/stat" file
    /// from the system.
    /// </summary>
    /// <returns></returns>
    private (ulong idle, ulong total) ReadCpuStats()
    {
        var parts = File.ReadLines("/proc/stat")
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries); 
        var user = ulong.Parse(parts[1]); 
        var nice = ulong.Parse(parts[2]); 
        var system = ulong.Parse(parts[3]); 
        var idle = ulong.Parse(parts[4]); 
        var iowait = ulong.Parse(parts[5]); 
        var irq = ulong.Parse(parts[6]); 
        var softirq = ulong.Parse(parts[7]); 
        var steal = parts.Length > 8 ? ulong.Parse(parts[8]) : 0;

        var total = user + nice + system + idle + iowait + irq + softirq + steal;
        return (idle + iowait, total);
    }
}