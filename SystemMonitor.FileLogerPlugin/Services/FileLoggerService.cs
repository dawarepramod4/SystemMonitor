using SystemMonitor.Core;
using SystemMonitor.Core.Models;
using SystemMonitor.FileLogerPlugin.Utility;

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

        //get path 
        var logFilePath = GetOutputDirectoryFilePath();

        //need string lines to write to a file
        var linesToLog = GenerateLoggingMessageList(monitorDataDto);

        //log all the lines to file.
        File.AppendAllLines(logFilePath, linesToLog);
    }

    /// <summary>
    /// Generated a user readable message to be added to the log file.
    /// </summary>
    /// <param name="monitorDataDto">data to create message for.</param>
    /// <returns>list of the messages</returns>
    private List<string> GenerateLoggingMessageList(MonitorDataDto monitorDataDto)
    {
        string memoryUsageMsg =
            GetUserReadablyMemoryUsageMsg(monitorDataDto.MemoryUsage.MemoryUsed, monitorDataDto.MemoryUsage.MemoryTotal);

        //CPU and memory usage
        List<string> lines =
        [
            "-----------------------------------",
            $"{monitorDataDto.DateTime:yyyy-MM-dd HH:mm:ss.fff zzz}:",
            $"CPU usage: {monitorDataDto.CpuUsagePercentage}",
            $"Memory Usage: {memoryUsageMsg}"
        ];

        //need to traverse all the disks to add to the log messages
        lines.Add("Disk Usage: \nName \t Usage");
        lines.AddRange(monitorDataDto.DiskUsage.Select(diskUsage =>
        {
            string diskUsageMsg = GetUserReadablyMemoryUsageMsg(diskUsage.DiskUsed, diskUsage.DiskTotal);
            return $"{diskUsage.DriveName}: \t{diskUsageMsg}";
        }));

        //write to console
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }

        return lines;
    }

    /// <summary>
    /// Returns a message to be shown to users in user readable convetion like GB, MB, KB, B
    /// </summary>
    /// <param name="used">used Portion of the Memory</param>
    /// <param name="total">Total Memory Availabe</param>
    /// <returns></returns>
    private string GetUserReadablyMemoryUsageMsg(long used, long total)
    {
        string usedMemoryString = MemoryConversionUtility.ConvertIntegerBytesToUserReadableString(used);
        string totalMemoryString = MemoryConversionUtility.ConvertIntegerBytesToUserReadableString(total);
        return usedMemoryString + '/' + totalMemoryString;
    }


    /// <summary>
    /// Creates a Directory For Output Log File
    /// </summary>
    /// <returns></returns>
    private string GetOutputDirectoryFilePath()
    {
        var outDirPath = Path.Combine(AppContext.BaseDirectory, "Output");
        var logFilePath = Path.Combine(outDirPath, "log.txt");

        //make sure directory exists for output
        Directory.CreateDirectory(outDirPath);

        //create file if the file does not exist.
        if (!File.Exists(logFilePath))
        {
            Console.WriteLine($"Log file doesn't exist: {logFilePath}, creating...");
            File.Create(logFilePath).Close();
        }

        return logFilePath;

    }




}