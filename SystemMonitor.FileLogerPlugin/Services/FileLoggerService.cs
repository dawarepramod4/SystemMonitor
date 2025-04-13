using SystemMonitor.Core;
using SystemMonitor.Core.Models;

namespace SystemMonitor.FileLogerPlugin.Services;

/// <summary>
/// Logger Service to log the data to a file.
/// </summary>
public class FileLoggerService
{
    /// <summary>
    /// Logs the data to a text file
    /// </summary>
    /// <param name="monitorDataDto">Monitored data to be logged in a file</param>
    public void LogMonitoredDataToFile(MonitorDataDto monitorDataDto)
    {
        var logFilePath = "log.txt";
        
        //create file if the file does not exist.
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine($"Log file doesn't exist: {logFilePath}, creating...");
            File.Create(logFilePath).Close();
        }
        
        //log all the lines to file.
        var linesToLog = GenerateLoggingMessageList(monitorDataDto);
        File.AppendAllLines(logFilePath, linesToLog);
    }

    /// <summary>
    /// Generated a user readable message to be added to the log file.
    /// </summary>
    /// <param name="monitorDataDto">data to create message for.</param>
    /// <returns>list of the messages</returns>
    private List<string> GenerateLoggingMessageList(MonitorDataDto monitorDataDto)
    {
        //CPU and memory usage
        List<string> lines =
        [
            "-----------------------------------",
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff zzz}:",
            $"CPU usage: {monitorDataDto.CpuUsagePercentage}",
            $"Memory Usage: {monitorDataDto.MemoryUsage?.MemoryUsed}/{monitorDataDto.MemoryUsage?.MemoryTotal}"
        ];
        
        //need to traverse all the disks to add to the log messages
        lines.AddRange(monitorDataDto.DiskUsage.Select(diskUsage => $"{diskUsage.DriveName}: \t{diskUsage.DiskUsed}/{diskUsage.DiskTotal}"));
        
        return lines;
    }
    
}